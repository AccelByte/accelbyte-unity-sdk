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
            ResultCallback<Item> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get item! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get item! AccessToken parameter is null!");
            Assert.IsNotNull(itemId, "Can't get item! ItemId parameter is null!");
            Assert.IsNotNull(region, "Can't get item! Region parameter is null!");
            Assert.IsNotNull(language, "Can't get item! Language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/items/{itemId}/locale")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("itemId", itemId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Item>();

            callback.Try(result);
        }

        public IEnumerator GetItemsByCriteria(string @namespace, string accessToken, string region, string language,
            ItemCriteria criteria, ResultCallback<PagedItems> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get items by criteria! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get items by criteria! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get items by criteria! Language parameter is null!");
            Assert.IsNotNull(criteria, "Can't get items by criteria! Criteria parameter is null!");
            Assert.IsNotNull(language, "Can't get items by criteria! Language parameter is null!");

            var queries = new Dictionary<string, string>();

            if (criteria != null)
            {
                if (criteria.CategoryPath != null)
                {
                    queries.Add("categoryPath", criteria.CategoryPath);
                }

                if (criteria.ItemType != null)
                {
                    queries.Add("itemType", criteria.ItemType.ToString());
                }

                if (criteria.ItemStatus != null)
                {
                    queries.Add("status", criteria.ItemStatus.ToString());
                }

                if (criteria.Page != null)
                {
                    queries.Add("page", Convert.ToString(criteria.Page));
                }

                if (criteria.Size != null)
                {
                    queries.Add("size", Convert.ToString(criteria.Size));
                }
            }

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/items/byCriteria")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithQueries(queries)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedItems>();

            callback.Try(result);
        }
    }
}