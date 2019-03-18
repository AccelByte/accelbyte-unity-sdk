// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Net;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class CategoriesApi
    {
        private readonly string baseUrl;

        internal CategoriesApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can't construct CatalogService; BaseUrl parameter is null!");

            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetCategory(string @namespace, string accessToken, string categoryPath,
            string language, ResultCallback<Category> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get category! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get category! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get category! Language parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get category! CategoryPath parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/categories/{categoryPath}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Category> result = response.TryParseJsonBody<Category>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetRootCategories(string @namespace, string accessToken, string language,
            ResultCallback<Category[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get root categories! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get root categories! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get root categories! Language parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/categories")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(accessToken)
                .WithQueryParam("language", language)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Category[]> result = response.TryParseJsonBody<Category[]>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetChildCategories(string @namespace, string accessToken, string categoryPath,
            string language, ResultCallback<Category[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get child categories! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get child categories! AccessToken parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get child categories! CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get child categoreis! Language parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/categories/{categoryPath}/children")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Category[]> result = response.TryParseJsonBody<Category[]>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetDescendantCategories(string @namespace, string accessToken, string categoryPath,
            string language, ResultCallback<Category[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get descendant categories! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get descendant categories! AccessToken parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get descendant categories! CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get descendant categories! Language parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(
                    this.baseUrl + "/platform/public/namespaces/{namespace}/categories/{categoryPath}/descendants")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Category[]> result = response.TryParseJsonBody<Category[]>();
            callback.Try(result);
        }
    }
}