// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licsed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using System.Linq;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerUGCApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==UGCServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerUGCApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.UGCServerUrl, session)
        {
        }

        /// <summary>
        /// Convert UGC Sort By Enum to String Value
        /// </summary>
        private string ConvertUGCSortByToString(UGCSortBy sortBy)
        {
            switch (sortBy)
            {
                case UGCSortBy.NAME:
                    return "name";
                case UGCSortBy.DATE:
                    return "date";
                case UGCSortBy.DOWNLOAD:
                    return "download";
                case UGCSortBy.LIKE:
                    return "like";
                case UGCSortBy.UPDATED_TIME:
                    return "updatedTime";
            }
            return "";
        }

        /// <summary>
        /// Convert UGC Order By Enum to String Value
        /// </summary>
        private string ConvertUGCOrderByToString(UGCOrderBy orderBy)
        {
            switch (orderBy)
            {
                case UGCOrderBy.ASC:
                    return "name";
                case UGCOrderBy.DESC:
                    return "date";
            }
            return "";
        }

        public IEnumerator SearchContent(SearchContentRequest searchContentRequest
            , ResultCallback<UGCSearchContentsPagingResponse> callback
            , string userId)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/contents/search")
                 .WithPathParam("namespace", Namespace_)
                 .WithQueryParam("name", searchContentRequest.name)
                 .WithQueryParam("creator", searchContentRequest.creator)
                 .WithQueryParam("type", searchContentRequest.type)
                 .WithQueryParam("subtype", searchContentRequest.subtype)
                 .WithQueryParam("tags", string.Join(",", searchContentRequest.tags))
                 .WithQueryParam("isOfficial", searchContentRequest.isOfficial ? "true" : "false")
                 .WithQueryParam("ishidden", searchContentRequest.IsHidden ? "true" : "false")
                 .WithQueryParam("sortBy", ConvertUGCSortByToString(searchContentRequest.sortBy))
                 .WithQueryParam("orderBy", ConvertUGCOrderByToString(searchContentRequest.orderBy))
                 .WithQueryParam("offset", searchContentRequest.offset >= 0 ? searchContentRequest.offset.ToString() : string.Empty)
                 .WithQueryParam("limit", searchContentRequest.limit >= 0 ? searchContentRequest.limit.ToString() : string.Empty)
                 .WithQueryParam("userId", userId)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCSearchContentsPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator SearchContentsSpesificToChannel(string channelId
            , SearchContentRequest searchContentRequest
            , ResultCallback<UGCSearchContentsPagingResponse> callback
            , string userId)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");
            Assert.IsNotNull(channelId, "Can't search content! channelId parameter is null!");

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/channels/{channelId}/contents/search")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("channelId", channelId)
                 .WithQueryParam("name", searchContentRequest.name)
                 .WithQueryParam("creator", searchContentRequest.creator)
                 .WithQueryParam("type", searchContentRequest.type)
                 .WithQueryParam("subtype", searchContentRequest.subtype)
                 .WithQueryParam("tags", string.Join(",", searchContentRequest.tags))
                 .WithQueryParam("isOfficial", searchContentRequest.isOfficial ? "true" : "false")
                 .WithQueryParam("ishidden", searchContentRequest.IsHidden ? "true" : "false")
                 .WithQueryParam("sortBy", ConvertUGCSortByToString(searchContentRequest.sortBy))
                 .WithQueryParam("orderBy", ConvertUGCOrderByToString(searchContentRequest.orderBy))
                 .WithQueryParam("offset", searchContentRequest.offset >= 0 ? searchContentRequest.offset.ToString() : string.Empty)
                 .WithQueryParam("limit", searchContentRequest.limit >= 0 ? searchContentRequest.limit.ToString() : string.Empty)
                 .WithQueryParam("userId", userId)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCSearchContentsPagingResponse>();
            callback.Try(result);
        }
    }        
}