// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
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
    internal class ItemsApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal ItemsApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator GetItem(string @namespace, string accessToken, string itemId, string region, string language,
            ResultCallback<PopulatedItemInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get item! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get item! AccessToken parameter is null!");
            Assert.IsNotNull(itemId, "Can't get item! ItemId parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/items/{itemId}/locale")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("itemId", itemId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson);

            if(region != null)
            {
                builder.WithQueryParam("region", region);
            }

            if(language != null)
            {
                builder.WithQueryParam("language", language);
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PopulatedItemInfo>();

            callback.Try(result);
        }

        public IEnumerator GetItemsByCriteria(string @namespace, string accessToken,
            ItemCriteria criteria, ResultCallback<ItemPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get items by criteria! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get items by criteria! AccessToken parameter is null!");
            Assert.IsNotNull(criteria, "Can't get items by criteria! Criteria parameter is null!");

            var queries = new Dictionary<string, string>();

            if (criteria != null)
            {
                if (criteria.categoryPath != null)
                {
                    queries.Add("categoryPath", criteria.categoryPath);
                }

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
            }

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/items/byCriteria")
                .WithPathParam("namespace", @namespace)
                .WithQueries(queries)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ItemPagingSlicedResult>();

            callback.Try(result);
        }
    }
}