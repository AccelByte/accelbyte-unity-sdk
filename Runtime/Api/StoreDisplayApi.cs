// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Api
{
    public class StoreDisplayApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
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

            var error = ApiHelperUtils.CheckForNullOrEmpty(userId
                , storeId
                , language
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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
            callback?.Try(result);
        }

        public IEnumerator ListActiveSectionContents(string userId
         , string storeId
         , string viewId
         , string region
         , string language
         , ResultCallback<SectionInfo[]> callback)
        {
            yield return ListActiveSectionContents(userId, storeId, viewId, region, language, false, callback);
        }

        public IEnumerator ListActiveSectionContents(string userId
            , string storeId
            , string viewId
            , string region
            , string language
            , bool autoCalcEstimatedPrice
            , ResultCallback<SectionInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(userId
                , storeId
                , viewId
                , region
                , language
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/sections")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("storeId", storeId)
                .WithQueryParam("viewId", viewId)
                .WithQueryParam("region", region)
                .WithQueryParam("language", language)
                .WithQueryParam("autoCalcEstimatedPrice", autoCalcEstimatedPrice.ToString())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SectionInfo[]>();
            callback?.Try(result);
        }
    }
}
