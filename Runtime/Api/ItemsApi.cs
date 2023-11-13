// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class ItemsApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ItemsApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator GetItem( string itemId
            , string region
            , string language
            , ResultCallback<PopulatedItemInfo> callback
            , string storeId
            , bool populateBundle )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(itemId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(itemId) + " cannot be null or empty"));
                yield break;
            } 

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/{itemId}/locale")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("itemId", itemId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithQueryParam("storeId", storeId)
                .WithQueryParam("populateBundle", populateBundle ? "true" : "false")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<PopulatedItemInfo>();

            callback.Try(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="publisherNamespace">Different than Config Namespace</param>
        /// <param name="appId"></param>
        /// <param name="callback"></param>
        /// <param name="language"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public IEnumerator GetItemByAppId( string publisherNamespace
            , string appId
            , ResultCallback<ItemInfo> callback
            , string language
            , string region )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(publisherNamespace))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(publisherNamespace) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(appId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(appId) + " cannot be null or empty"));
                yield break;
            } 

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/byAppId")
                .WithPathParam("namespace", publisherNamespace)
                .WithQueryParam("appId", appId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ItemInfo>();

            callback.Try(result);
        }

        public IEnumerator GetItemsByCriteria( ItemCriteria criteria
            , ResultCallback<ItemPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (criteria == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(criteria) + " cannot be null"));
                yield break;
            } 

            var queries = new Dictionary<string, string>();

            if (criteria.categoryPath != null)
            {
                queries.Add("categoryPath", criteria.categoryPath);
            }

            queries.Add("includeSubCategoryItem", criteria.includeSubCategoryItem ? "true" : "false");

            if (criteria.itemType != ItemType.NONE)
            {
                queries.Add("itemType", criteria.itemType.ToString());
            }

            if (criteria.appType != EntitlementAppType.NONE)
            {
                queries.Add("appType", criteria.appType.ToString());
            }

            if(criteria.region != null)
            {
                queries.Add("region", criteria.region);
            }

            if(criteria.language != null)
            {
                queries.Add("language", criteria.language);
            }

            if(criteria.tags != null)
            {
                string tags = "";
                for(int i=0;i<criteria.tags.Length;i++){
                    tags += (i < criteria.tags.Length - 1 ? criteria.tags[i] + "," : criteria.tags[i]);
                }
                queries.Add("tags", tags);
            }

            if(criteria.features != null)
            {
                string feats = "";
                for (int i = 0; i < criteria.features.Length; i++)
                {
                    feats += (i < criteria.features.Length - 1 ? criteria.features[i] + "," : criteria.features[i]);
                }
                queries.Add("features", feats);
            }

            if (criteria.offset != null)
            {
                queries.Add("offset", Convert.ToString(criteria.offset));
            }

            if (criteria.limit != null)
            {
                queries.Add("limit", Convert.ToString(criteria.limit));
            }
            if(criteria.sortBy != null)
            {
                queries.Add("sortBy", criteria.sortBy);
            }
            if (criteria.storeId != null)
            {
                queries.Add("storeId", criteria.storeId);
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/byCriteria")
                .WithPathParam("namespace", Namespace_)
                .WithQueries(queries)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ItemPagingSlicedResult>();

            callback.Try(result);
        }

        public IEnumerator SearchItem(string language
            , string keyword
            , int offset
            , int limit
            , string region
            , ResultCallback<ItemPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(language))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(language) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(keyword))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(keyword) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/search")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("language", language)
                .WithQueryParam("keyword", keyword)
                .WithQueryParam("region", region)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit >= 0) ? limit.ToString() : "")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ItemPagingSlicedResult>();

            callback.Try(result);
        }

        public IEnumerator GetItemBySku(string sku
            , string language
            , string region
            , ResultCallback<ItemInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(sku))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(sku) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/bySku")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sku", sku)
                .WithQueryParam("language", language)
                .WithQueryParam("region", region)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ItemInfo>();

            callback.Try(result);
        }

        public IEnumerator BulkGetLocaleItems(string[] itemIds
            , string language
            , string region
            , ResultCallback<ItemInfo[]> callback
            , string storeId)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (itemIds == null || itemIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/locale/byIds")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("storeId", storeId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (itemIds != null && itemIds.Length > 0)
            {
                builder.WithQueryParam("itemIds", string.Join(",", itemIds));
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<ItemInfo[]>();

            callback.Try(result);
        }

        public IEnumerator GetListAllStore(ResultCallback<PlatformStore[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/stores")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<PlatformStore[]>();

            callback.Try(result);
        }

        public IEnumerator GetEstimatedPrice(string[] itemIds, string region, ResultCallback<EstimatedPricesInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (itemIds == null || itemIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(itemIds) + " cannot be null or empty"));
                yield break;
            } 

            // There will only one published stored in a live game environment. 
            // Here the StoreId is not included on the query param, 
            // then the Platform Service will fill by default value/published store Id. 
            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/items/estimatedPrice")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken) 
                .WithPathParam("region", region)
                .Accepts(MediaType.ApplicationJson);

            if (itemIds != null && itemIds.Length > 0)
            {
                builder.WithQueryParam("itemIds", string.Join(",", itemIds));
            }

            var request = builder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => 
            { 
                response = rsp; 
            });

            var result = response.TryParseJson<EstimatedPricesInfo[]>();

            callback.Try(result);
        }
    }
}
