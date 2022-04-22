// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class CategoriesApi
    {
        #region Fields 

        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        #endregion

        #region Constructor

        internal CategoriesApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        #endregion

        #region Public Methods

        public IEnumerator GetCategory(string @namespace, string accessToken, string categoryPath, string language,
            ResultCallback<CategoryInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get category! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get category! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get category! Language parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get category! CategoryPath parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/categories/{categoryPath}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo>();
            callback.Try(result);
        }

        public IEnumerator GetRootCategories(string @namespace, string accessToken, string language,
            ResultCallback<CategoryInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get root categories! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get root categories! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get root categories! Language parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/categories")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(accessToken)
                .WithQueryParam("language", language)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo[]>();
            callback.Try(result);
        }

        public IEnumerator GetChildCategories(string @namespace, string accessToken, string categoryPath,
            string language, ResultCallback<CategoryInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get child categories! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get child categories! AccessToken parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get child categories! CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get child categories! Language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/categories/{categoryPath}/children")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo[]>();
            callback.Try(result);
        }

        public IEnumerator GetDescendantCategories(string @namespace, string accessToken, string categoryPath,
            string language, ResultCallback<CategoryInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get descendant categories! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get descendant categories! AccessToken parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get descendant categories! CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get descendant categories! Language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/categories/{categoryPath}/descendants")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo[]>();
            callback.Try(result);
        }

        #endregion 
    }
}