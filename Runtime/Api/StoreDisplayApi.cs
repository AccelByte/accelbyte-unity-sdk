// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
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
    public class StoreDisplayApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        internal StoreDisplayApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.PlatformServerUrl, session)
        {
        }

        public IEnumerator GetAllViews(string userId
          , string storeId
          , string language
          , ResultCallback<ViewInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't List Active Section Contents! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't List Active Section Contents! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't List Active Section Contents! userId parameter is null!");
            Assert.IsNotNull(storeId, "Can't List Active Section Contents! storeId parameter is null!");
            Assert.IsNotNull(language, "Can't List Active Section Contents! language parameter is null!");


            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/views")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("storeId", storeId)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<ViewInfo[]>();
            callback.Try(result);
        }

        public IEnumerator ListActiveSectionContents(string userId
         , string storeId
         , string viewId
         , string region
         , string language
         , ResultCallback<SectionInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't List Active Section Contents! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't List Active Section Contents! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't List Active Section Contents! userId parameter is null!");
            Assert.IsNotNull(storeId, "Can't List Active Section Contents! storeId parameter is null!");
            Assert.IsNotNull(viewId, "Can't List Active Section Contents! viewId parameter is null!");
            Assert.IsNotNull(region, "Can't List Active Section Contents! region parameter is null!");
            Assert.IsNotNull(language, "Can't List Active Section Contents! language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/sections")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("storeId", storeId)
                .WithQueryParam("viewId", viewId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SectionInfo[]>();
            callback.Try(result);
        }
    }
}
