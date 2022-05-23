// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class CategoriesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        internal CategoriesApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator GetCategory( string categoryPath
            , string language
            , ResultCallback<CategoryInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get category! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get category! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get category! Language parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get category! CategoryPath parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/categories/{categoryPath}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo>();
            callback.Try(result);
        }

        public IEnumerator GetRootCategories( string language
            , ResultCallback<CategoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get root categories! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get root categories! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't get root categories! Language parameter is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/categories")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithQueryParam("language", language)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo[]>();
            callback.Try(result);
        }

        public IEnumerator GetChildCategories( string categoryPath
            , string language
            , ResultCallback<CategoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            
            Assert.IsNotNull(Namespace_, "Can't get child categories! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get child categories! AccessToken parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get child categories! CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get child categories! Language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/categories/{categoryPath}/children")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo[]>();
            callback.Try(result);
        }

        public IEnumerator GetDescendantCategories( string categoryPath
            , string language
            , ResultCallback<CategoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get descendant categories! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get descendant categories! AccessToken parameter is null!");
            Assert.IsNotNull(categoryPath, "Can't get descendant categories! CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get descendant categories! Language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/categories/{categoryPath}/descendants")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CategoryInfo[]>();
            callback.Try(result);
        }
    }
}
