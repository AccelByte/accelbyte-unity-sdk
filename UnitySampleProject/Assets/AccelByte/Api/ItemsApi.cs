// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Net;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class ItemsApi
    {
        private readonly string baseUrl;

        internal ItemsApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can't construct CatalogService; BaseUrl parameter is null!");

            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetItem(string @namespace, string accessToken, string itemId, string region,
            string language, ResultCallback<Item> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get item! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get item! AccessToken parameter is null!");
            Assert.IsNotNull(itemId, "Can't get item! ItemId parameter is null!");
            Assert.IsNotNull(region, "Can't get item! Region parameter is null!");
            Assert.IsNotNull(language, "Can't get item! Language parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/items/{itemId}/locale")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("itemId", itemId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Item> result = response.TryParseJsonBody<Item>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetItemsByCriteria(string @namespace, string accessToken, string region,
            string language, ItemCriteria criteria, ResultCallback<PagedItems> callback)
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

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/items/byCriteria")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithQueries(queries)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<PagedItems> result = response.TryParseJsonBody<PagedItems>();
            callback.Try(result);
        }
    }
}