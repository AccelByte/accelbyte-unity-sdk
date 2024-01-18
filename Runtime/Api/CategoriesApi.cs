// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class CategoriesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, language, categoryPath);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, language);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, categoryPath, language);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, categoryPath, language);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

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
