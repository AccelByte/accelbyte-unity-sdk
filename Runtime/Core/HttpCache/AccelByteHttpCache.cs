// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class HttpCacheStoreResult
    {
        internal string Key = null;
        internal AccelByteCacheItem<AccelByteHttpCacheData> StoredCache = null;
    }

    internal class HttpCacheRetrieveResult
    {
        internal AccelByteHttpCacheData ItemResult = null;

        internal IHttpRequest Request
        {
            get
            {
                if (ItemResult == null)
                {
                    return null;
                }
                return ItemResult.Request;
            }
        }

        internal IHttpResponse Response
        {
            get
            {
                if (ItemResult == null)
                {
                    return null;
                }
                return ItemResult.Response;
            }
        }

        internal Error ResponseError
        {
            get
            {
                if (ItemResult == null)
                {
                    return null;
                }
                return ItemResult.ResponseError;
            }
        }
    }

    internal struct BasicHeader
    {
        public const string Age = "Age";

        // Directives for caching mechanisms in both requests and responses
        public const string Control = "Cache-Control";

        // Directives to identify unique version of a resource
        public const string ETag = "ETag";

        public const string ContentLocation = "Content-Location";
        public const string Date = "Date";
        public const string Expires = "Expires";

        // Directives to makes the request conditional
        public const string IfNoneMatch = "If-None-Match";
    };

    internal struct ControlDirectiveHeader
    {
        // indicates that the response remains fresh until N seconds after the response is generated
        public const string MaxAge = "max-age";

        // allows caches to store a response, but requires them to revalidate it before reuse
        public const string NoCache = "no-cache";

        // The no-store response directive indicates that any caches of any kind (private or shared) should not store this response.
        public const string NoStore = "no-store";

        // The private response directive indicates that the response can be stored only in a private cache (e.g. local caches in browsers).
        public const string Private = "private";

        // Enables responses for requests with Authorization header fields to be stored in a shared cache.
        public const string Public = "public";

        // Static resources that are never modified
        public const string Immutable = "immutable";

        // the response can be stored in caches and can be reused while fresh. Once it becomes stale, it must be validated with the origin server before reuse
        public const string MustRevalidate = "must-revalidate";
    };

    internal class AccelByteHttpCache<T> where T : IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>
    {
        internal const int MaxAgeCacheThreshold = 100;

        private readonly T itemCache;
        private readonly int maxAge;

        public AccelByteHttpCache(T itemCache, int maxAge = MaxAgeCacheThreshold)
        {
            this.itemCache = itemCache;
            this.maxAge = maxAge;
        }

        public HttpCacheRetrieveResult TryRetrieving(IHttpRequest request)
        {
            HttpCacheRetrieveResult retval = null;
            lock (itemCache)
            {
                string key = ConstructKey(request);
                if (itemCache.Contains(key))
                {
                    var cachedHttpItem = itemCache.Retrieve(key);
                    var cacheFreshness = CheckCachedItemFreshness(cachedHttpItem);
                    if (cacheFreshness == AccelByteCacheFreshness.Fresh)
                    {
                        if (cachedHttpItem != null && cachedHttpItem.Item.Response != null)
                        {
                            retval = new HttpCacheRetrieveResult();
                            retval.ItemResult = new AccelByteHttpCacheData(request, cachedHttpItem.Item.Response, cachedHttpItem.Item.ResponseError);
                        }
                    }
                    else if (cacheFreshness == AccelByteCacheFreshness.WaitingRefresh)
                    {
                        if (cachedHttpItem != null && cachedHttpItem.Item.Response != null)
                        {
                            var eTagValue = HttpHeaderHelper.GetHeaderValue(cachedHttpItem.Item.Response.Headers, BasicHeader.ETag);
                            if (!string.IsNullOrEmpty(eTagValue))
                            {
                                var copiedRequest = request;
                                copiedRequest.Headers.Add(BasicHeader.IfNoneMatch, eTagValue);
                                retval = new HttpCacheRetrieveResult();
                                retval.ItemResult = new AccelByteHttpCacheData(copiedRequest, null, null);
                            }
                        }
                    }
                    else if (cacheFreshness == AccelByteCacheFreshness.Stale)
                    {
                        itemCache.Remove(key);
                    }
                }
            }
            return retval;
        }

        public HttpCacheStoreResult TryStoring(IHttpRequest request, IHttpResponse response, Error responseError)
        {
            HttpCacheStoreResult retval = null;
            if (response != null && IsResponseCacheable(request, response, maxAge))
            {
                retval = new HttpCacheStoreResult();
                retval.Key = ConstructKey(request);
                int timeInProxyCache = 0;
                string cacheAgeHeader = HttpHeaderHelper.GetHeaderValue(response.Headers, BasicHeader.Age);
                if (!string.IsNullOrEmpty(cacheAgeHeader))
                {
                    int.TryParse(cacheAgeHeader, out timeInProxyCache);
                }

                double responseAgeThreshold = 0;
                string responseControlDirectiveValue = HttpHeaderHelper.GetHeaderValue(response.Headers, BasicHeader.Control);
                if (!string.IsNullOrEmpty(responseControlDirectiveValue))
                {
                    string maxAge = ExtractHeaderDirectiveValue(responseControlDirectiveValue, ControlDirectiveHeader.MaxAge);
                    if (!string.IsNullOrEmpty(maxAge))
                    {
                        double.TryParse(maxAge, out responseAgeThreshold);
                    }
                }

                var timeNow = System.DateTime.Now;
                var expireTime = timeNow + System.TimeSpan.FromSeconds(responseAgeThreshold) - System.TimeSpan.FromSeconds(timeInProxyCache);

                // IF the response from the online storage service return 304
                // THEN we can reuse the old cache and extend the usage because the response should be same
                lock (itemCache)
                {
                    if (itemCache.Contains(retval.Key) && (ErrorCode)response.Code == ErrorCode.NotModified)
                    {
                        if (response.Headers != null)
                        {
                            string[] transferredHeaders =
                            {
                                BasicHeader.Control,
                                BasicHeader.ContentLocation,
                                BasicHeader.Date,
                                BasicHeader.ETag,
                                BasicHeader.Expires
                            };

                            var modifiedCachedItem = itemCache.Peek(retval.Key);
                            modifiedCachedItem.ExpireTime = expireTime;
                            foreach (var headerKey in transferredHeaders)
                            {
                                string value = HttpHeaderHelper.GetHeaderValue(response.Headers, headerKey);
                                if (!string.IsNullOrEmpty(value))
                                {
                                    modifiedCachedItem.Item.Response.Headers[headerKey] = value;
                                }
                            }
                            itemCache.Update(retval.Key, modifiedCachedItem);
                            retval.StoredCache = modifiedCachedItem;
                        }
                    }
                    else
                    {
                        var newHttpCache = new AccelByteHttpCacheData(request, response, responseError);
                        var newCacheItem = new AccelByteCacheItem<AccelByteHttpCacheData>(expireTime, newHttpCache);
                        itemCache.Emplace(retval.Key, newCacheItem);
                        retval.StoredCache = newCacheItem;
                    }
                }
            }
            return retval;
        }

        private static bool IsResponseCacheable(IHttpRequest request, IHttpResponse response, int maxAge)
        {
            if (request.Method == HttpRequestBuilder.DeleteMethod)
            {
                return false;
            }
            if (response == null)
            {
                return false;
            }
            //IF 304 or between 200-204
            var responseCode = (ErrorCode)response.Code;
            if (responseCode != ErrorCode.NotModified && (responseCode < ErrorCode.Ok || responseCode > ErrorCode.NoContent))
            {
                return false;
            }

            bool cacheable = false;
            string cacheControlHeaderValue = null;
            if (response.Headers != null)
            {
                cacheControlHeaderValue = HttpHeaderHelper.GetHeaderValue(response.Headers, BasicHeader.Control);
            }
            if (!string.IsNullOrEmpty(cacheControlHeaderValue))
            {
                // check hard prevention (no-store)
                cacheable = !IsDirectiveHasValue(cacheControlHeaderValue, ControlDirectiveHeader.NoStore);
                // re-check the requirement (immutable)
                if (!cacheable && IsDirectiveHasValue(cacheControlHeaderValue, ControlDirectiveHeader.Immutable))
                {
                    cacheable = true;
                }
                if (cacheable)
                {
                    string responseMaxAge = ExtractHeaderDirectiveValue(cacheControlHeaderValue, ControlDirectiveHeader.MaxAge);
                    if (responseMaxAge != null)
                    {
                        bool successParse = int.TryParse(responseMaxAge, out int responseMaxAgeIntValue);
                        if (successParse && responseMaxAgeIntValue > maxAge)
                        {
                            cacheable = false;
                        }
                    }
                }
            }

            return cacheable;
        }

        private static string ExtractHeaderDirectiveValue(string header, string controlDirective)
        {
            string retval = null;
            if (header == null)
            {
                return retval;
            }
            var headerDirectives = header.Split(',');
            if (headerDirectives != null)
            {
                foreach (var directive in headerDirectives)
                {
                    var keyValuePair = directive.Split('=');
                    if (keyValuePair != null && keyValuePair.Length == 2 && keyValuePair[0].ToLower() == controlDirective.ToLower())
                    {
                        retval = keyValuePair[1];
                        break;
                    }
                }
            }

            return retval;
        }

        private static bool IsDirectiveHasValue(string directive, string controlDirectiveKey)
        {
            return directive.Contains(controlDirectiveKey, System.StringComparison.OrdinalIgnoreCase);
        }

        private string ConstructKey(IHttpRequest request)
        {
            string retval = $"{request.Method}-{request.Url}";
            return retval;
        }

        private AccelByteCacheFreshness CheckCachedItemFreshness(AccelByteCacheItem<AccelByteHttpCacheData> cachedItem)
        {
            var retval = AccelByteCacheFreshness.Fresh;

            var cachedResponse = cachedItem.Item.Response;
            var cachedItemExpireTime = cachedItem.ExpireTime;

            string cacheControlHeaderValue = null;

            if (cachedResponse != null)
            {
                cacheControlHeaderValue = HttpHeaderHelper.GetHeaderValue(cachedResponse.Headers, BasicHeader.Control);
            }

            var timeNow = System.DateTime.Now;
            bool isStaleResponse = false;
            if (!string.IsNullOrEmpty(cacheControlHeaderValue) && IsDirectiveHasValue(cacheControlHeaderValue, ControlDirectiveHeader.Immutable))
            {
                isStaleResponse = false;
            }
            else if (cachedItemExpireTime < timeNow) //Expired
            {
                isStaleResponse = true;
            }
            else // Fresh
            {
                if (!string.IsNullOrEmpty(cacheControlHeaderValue))
                {
                    //@TODO this needs to check for validation, for now we are dropping this as stale
                    if (IsDirectiveHasValue(cacheControlHeaderValue, ControlDirectiveHeader.NoCache))
                    {
                        isStaleResponse = true;
                        // no-cache 
                        //A cache will send the request to the origin server for validation before releasing a cached copy.

                    }
                    else if (IsDirectiveHasValue(cacheControlHeaderValue, ControlDirectiveHeader.MustRevalidate))
                    {
                        // must-revalidate
                        //When using the "must-revalidate" directive, the cache must verify the status of stale resources before using them

                        //TODO
                        //Revalidate(Key);
                        isStaleResponse = true;
                    }

                    //The "public" directive indicates that the response may be cached by any cache. 
                    // This can be useful if pages with HTTP authentication or response status codes that aren't normally cacheable should now be cached.
                    //On the other hand, "private" indicates that the response is intended for a single user only
                    //and must not be stored by a shared cache.A private browser cache may store the response in this case.
                }
            }

            if (isStaleResponse)
            {
                Result responseResult = null;
                string eTagHeaderValue = null;

                if (cachedResponse != null)
                {
                    responseResult = cachedResponse.TryParse();
                    eTagHeaderValue = HttpHeaderHelper.GetHeaderValue(cachedResponse.Headers, BasicHeader.ETag);
                }

                // Don't remove the cached item for this KEY yet, it has an ETag an might be refreshed by the following request
                if (responseResult != null && !responseResult.IsError && !string.IsNullOrEmpty(eTagHeaderValue))
                {
                    retval = AccelByteCacheFreshness.WaitingRefresh;
                }
                else
                {
                    retval = AccelByteCacheFreshness.Stale;
                }
            }

            return retval;
        }
    }
}
