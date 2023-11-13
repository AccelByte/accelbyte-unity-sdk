// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
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
    /// <summary>
    /// UGC == User-Generated Content
    /// </summary>
    internal class UGCApi : ApiBase
    {
        private const int maxBulkContentIds = 20;

        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==UGCServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal UGCApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
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
        /// Convert UGC Liked Content Sort By Enum to String Value
        /// </summary>
        private string ConvertUGCLikedContentSortByToString(UGCLikedContentSortBy sortBy)
        {
            switch (sortBy)
            {
                case UGCLikedContentSortBy.NAME:
                    return "name";
                case UGCLikedContentSortBy.DATE:
                    return "date";
                case UGCLikedContentSortBy.DOWNLOAD:
                    return "download";
                case UGCLikedContentSortBy.LIKE:
                    return "like";
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

        /// <summary>
        /// Convert Get UGC Contents Sort From Enum to String Value
        /// </summary>
        private string ConvertGetUGCContentsSortByToString(UGCContentSortBy sortBy)
        {
            switch (sortBy)
            {
                case UGCContentSortBy.Name:
                    return "name";
                case UGCContentSortBy.NameAsc:
                    return "name:asc";
                case UGCContentSortBy.NameDesc:
                    return "name:desc";
                case UGCContentSortBy.Download:
                    return "download";
                case UGCContentSortBy.DownloadAsc:
                    return "download:asc";
                case UGCContentSortBy.DownloadDesc:
                    return "download:desc";
                case UGCContentSortBy.Like:
                    return "like";
                case UGCContentSortBy.LikeAsc:
                    return "like:asc";
                case UGCContentSortBy.LikeDesc:
                    return "like:desc";
                case UGCContentSortBy.CreatedTime:
                    return "createdTime";
                case UGCContentSortBy.CreatedTimeAsc:
                    return "createdTime:asc";
                case UGCContentSortBy.CreatedTimeDesc:
                    return "createdTime:desc";
            }
            return "";
        }

        /// <summary>
        /// Convert Get UGC Downloader Sort From Enum to String Value
        /// </summary>
        private string ConvertGetUGCDownloaderSortByToString(UGCContentDownloaderSortBy sortBy)
        {
            switch (sortBy)
            {
                case UGCContentDownloaderSortBy.CreatedTime:
                    return "createdTime";
                case UGCContentDownloaderSortBy.CreatedTimeAsc:
                    return "createdTime:asc";
                case UGCContentDownloaderSortBy.CreatedTimeDesc:
                    return "createdTime:desc";
            }
            return "";
        }

        public IEnumerator CreateContent( string userId
            , string channelId
            , UGCRequest createRequest
            , ResultCallback<UGCResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't create content! channelId parameter is null!");

            UGCRequest Req = createRequest;
            if (string.IsNullOrEmpty(Req.contentType))
            {
                Req.contentType = "application/octet-stream";
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/s3")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Req.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCResponse>();
            callback.Try(result);
        }

        public IEnumerator CreateContent( string userId
            , string channelId
            , string name
            , string type
            , string subtype
            , string[] tags
            , byte[] preview
            , string fileExtension
            , ResultCallback<UGCResponse> callback
            , string contentType )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't create content! channelId parameter is null!");
            Assert.IsNotNull(name, "Can't create content! name parameter is null!");
            Assert.IsNotNull(type, "Can't create content! type parameter is null!");
            Assert.IsNotNull(subtype, "Can't create content! subType parameter is null!");
            Assert.IsNotNull(tags, "Can't create content! tags parameter is null!");
            Assert.IsNotNull(preview, "Can't create content! preview parameter is null!");
            Assert.IsNotNull(fileExtension, "Can't create content! fileExtension parameter is null!");

            UGCRequest createRequest = new UGCRequest
            {
                name = name,
                type = type,
                subtype = subtype,
                tags = tags,
                preview = Convert.ToBase64String(preview),
                fileExtension = fileExtension,
                contentType = contentType
            };

            yield return CreateContent(userId, channelId, createRequest, callback);
        }

        public IEnumerator ModifyContent( string userId
            , string channelId
            , string contentId
            , UGCUpdateRequest modifyRequest
            , ResultCallback<UGCResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't modify content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't modify content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't modify content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't modify content! channelId parameter is null!");
            Assert.IsNotNull(contentId, "Can't modify content! name parameter is null!");
            Assert.IsNotNull(modifyRequest, "Can't modify content! type parameter is null!");

            UGCUpdateRequest Req = modifyRequest;
            if (string.IsNullOrEmpty(Req.ContentType))
            {
                Req.ContentType = "application/octet-stream";
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/s3/{contentId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithPathParam("contentId", contentId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Req.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCResponse>();
            callback.Try(result);
        }

        public IEnumerator ModifyContent( string userId
            , string channelId
            , string contentId
            , UGCRequest modifyRequest
            , ResultCallback<UGCResponse> callback 
            , bool updateContent)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't modify content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't modify content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't modify content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't modify content! channelId parameter is null!");
            Assert.IsNotNull(contentId, "Can't modify content! name parameter is null!");
            Assert.IsNotNull(modifyRequest, "Can't modify content! type parameter is null!");

            UGCUpdateRequest Req = new UGCUpdateRequest
            {
                Name = modifyRequest.name,
                Type = modifyRequest.type,
                Subtype = modifyRequest.subtype,
                Tags = modifyRequest.tags,
                Preview = modifyRequest.preview,
                FileExtension = modifyRequest.fileExtension,
                ContentType = modifyRequest.contentType,
                PreviewMetadata = modifyRequest.PreviewMetadata,
                CustomAttributes = modifyRequest.customAttributes,
                UpdateContentFile = updateContent
            };

            yield return ModifyContent(userId, channelId, contentId, Req, callback);
        }

        public IEnumerator ModifyContent( string userId
            , string channelId
            , string contentId
            , string name
            , string type
            , string subtype
            , string[] tags
            , byte[] preview
            , string fileExtension
            , ResultCallback<UGCResponse> callback
            , string contentType 
            , bool updateContent)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't modify content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't modify content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't modify content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't modify content! channelId parameter is null!");
            Assert.IsNotNull(contentId, "Can't modify content! name parameter is null!");
            Assert.IsNotNull(name, "Can't modify content! name parameter is null!");
            Assert.IsNotNull(type, "Can't modify content! type parameter is null!");
            Assert.IsNotNull(subtype, "Can't modify content! subType parameter is null!");
            Assert.IsNotNull(tags, "Can't modify content! tags parameter is null!");
            Assert.IsNotNull(preview, "Can't modify content! preview parameter is null!");
            Assert.IsNotNull(fileExtension, "Can't modify content! fileExtension parameter is null!");

            UGCRequest modifyRequest = new UGCRequest
            {
                name = name,
                type = type,
                subtype = subtype,
                tags = tags,
                preview = Convert.ToBase64String(preview),
                fileExtension = fileExtension,
                contentType = contentType
            };

            yield return ModifyContent(userId, channelId, contentId, modifyRequest, callback, updateContent);
        }

        public IEnumerator SearchContent( SearchContentRequest searchContentRequest
            , string userId
            , ResultCallback<UGCSearchContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents")
                 .WithPathParam("namespace", Namespace_)
                 .WithQueryParam("name", searchContentRequest.name)
                 .WithQueryParam("creator", searchContentRequest.creator)
                 .WithQueryParam("type", searchContentRequest.type)
                 .WithQueryParam("subtype", searchContentRequest.subtype)
                 .WithQueryParam("tags", string.Join(",", searchContentRequest.tags))
                 .WithQueryParam("isOfficial", searchContentRequest.isOfficial ? "true" : "false")
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
                rsp => response = rsp);

            var result = response.TryParseJson<UGCSearchContentsPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator SearchContentsSpesificToChannel(string channelId
            , SearchContentRequest searchContentRequest
            , string userId
            , ResultCallback<UGCSearchContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");
            Assert.IsNotNull(channelId, "Can't search content! channelId parameter is null!");

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/channels/{channelId}/contents")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("channelId", channelId)
                 .WithQueryParam("name", searchContentRequest.name)
                 .WithQueryParam("creator", searchContentRequest.creator)
                 .WithQueryParam("type", searchContentRequest.type)
                 .WithQueryParam("subtype", searchContentRequest.subtype)
                 .WithQueryParam("tags", string.Join(",", searchContentRequest.tags))
                 .WithQueryParam("isOfficial", searchContentRequest.isOfficial ? "true" : "false")
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
                rsp => response = rsp);

            var result = response.TryParseJson<UGCSearchContentsPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator DeleteContent( string userId
            , string channelId
            , string contentId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't delete content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't delete content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't delete content! ChannelId parameter is null!");
            Assert.IsNotNull(contentId, "Can't delete content! ContentId parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/{contentId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithPathParam("contentId", contentId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetContentByContentId( string userId
            , string contentId
            , ResultCallback<UGCContentResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get content by content id! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get content by content id! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get content by content id! AccessToken parameter is null!");
            Assert.IsNotNull(contentId, "Can't get content by content id! ContentId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents/{contentId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("contentId", contentId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCContentResponse>();
            callback.Try(result);
        }

        public IEnumerator GetContentByShareCode( string shareCode
            , ResultCallback<UGCContentResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get content by share code! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get content by share code! AccessToken parameter is null!");
            Assert.IsNotNull(shareCode, "Can't get content by share code! ShareCoded parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents/sharecodes/{shareCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("shareCode", shareCode)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCContentResponse>();
            callback.Try(result);
        }

        public IEnumerator GetContentPreview( string userId
            , string contentId
            , ResultCallback<UGCPreview> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get content! AccessToken parameter is null!");
            Assert.IsNotNull(contentId, "Can't get content! contentId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents/{contentId}/preview")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("contentId", contentId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCPreview>();
            callback.Try(result);
        }

        public IEnumerator GetContentPreview( string userId
            , string contentId
            , ResultCallback<byte[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get content! AccessToken parameter is null!");
            Assert.IsNotNull(contentId, "Can't get content! contentId parameter is null!");

            yield return GetContentPreview(userId, contentId, result =>
            {
                if (result.IsError)
                {
                    callback.TryError(result.Error);
                    return;
                }

                byte[] bytes = null;
                try
                {
                    bytes = Convert.FromBase64String(result.Value.preview);
                }
                catch(Exception ex)
                {
                    Error error = new Error(ErrorCode.ErrorFromException, $"Failed to convert preview value.\nError: {ex.Message}");
                    return;
                }
                callback.TryOk(bytes);
            });
        }

        public IEnumerator GetTags( ResultCallback<UGCTagsPagingResponse> callback
            , int offset
            , int limit )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Tags! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Get Tags! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/tags")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCTagsPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator GetTypes( ResultCallback<UGCTypesPagingResponse> callback
            , int offset
            , int limit )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get types! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get types! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/types")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCTypesPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator CreateChannel( string userId
            , string channelName
            , ResultCallback<UGCChannelResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create channel! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create channel! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create channel! AccessToken parameter is null!");
            Assert.IsNotNull(channelName, "Can't create channel! channelName parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{ \"name\": \"{0}\" }}", channelName))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCChannelResponse>();
            callback.Try(result);
        }

        public IEnumerator GetChannels( string userId
            , ResultCallback<UGCChannelPagingResponse> callback
            , int offset
            , int limit 
            , string channelName)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get channels! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get channels! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get channels! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("name", channelName)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UGCChannelPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator DeleteChannel( string userId
            , string channelId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't delete channel! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete channel! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't delete channel! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't delete channel! ChannelId parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels/{channelId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator UpdateLikeStatusToContent(UpdateLikeStatusToContentRequest requestModel
            , UpdateLikeStatusToContentParameter requestParameter
            , ResultCallback<UGCUpdateLikeStatusToContentResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");

            var request = HttpRequestBuilder
                 .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/contents/{contentId}/like")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("contentId", requestParameter.ContentId)
                 .WithBody(requestModel.ToUtf8Json())
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCUpdateLikeStatusToContentResponse>();
            callback.Try(result);
        }
        public IEnumerator GetListFollowers(string userId
            , ResultCallback<UGCGetListFollowersPagingResponse> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get List Followers! Namespace parameter is null!");

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/followers")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("userId", userId)
                 .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                 .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCGetListFollowersPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator UpdateFollowStatus(UpdateFollowStatusRequest requestModel
            , UpdateFollowStatusParameter requestParameter
            , ResultCallback<UGCUpdateFollowStatusToUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get List Followers! Namespace parameter is null!");

            var request = HttpRequestBuilder
                 .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/follow")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("userId", requestParameter.UserId)
                 .WithBody(requestModel.ToUtf8Json())
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCUpdateFollowStatusToUserResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetBulkContentId(GetBulkContentIdRequest requestModel
            , ResultCallback<UGCModelsContentsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Content Followed! Namespace parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/contents/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCModelsContentsResponse[]>();
            callback.Try(result);
        }
        
        public IEnumerator GetUserContents(string userId
            , ResultCallback<UGCContentsPagingResponse> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Followed Creators! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't Get Followed Creators! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/contents")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCContentsPagingResponse>();
            callback.Try(result);
        }
        
        public IEnumerator UploadScreenshotContent(string contentId
            , string userId
            , ScreenshotsRequest screenshotsRequest
            , ResultCallback<ScreenshotsResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Followed Creators! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't Get Followed Creators! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/contents/{contentId}/screenshots")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("contentId", contentId)
                .WithBody(screenshotsRequest.ToJsonString().ToLower())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<ScreenshotsResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetContentFollowed(ResultCallback<UGCContentsPagingResponse> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Content Followed! Namespace parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents/followed")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCContentsPagingResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetFollowedCreators(ResultCallback<UGCGetListFollowersPagingResponse> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Followed Creators! Namespace parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/followed")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCGetListFollowersPagingResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetListFollowing(string userId
            , ResultCallback<UGCGetListFollowersPagingResponse> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Followed Creators! Namespace parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/following")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCGetListFollowersPagingResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetLikedContents( GetLikedContentRequest getLikedContentRequest
            , ResultCallback<UGCContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents/liked")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("tags", string.Join(",", getLikedContentRequest.tags))
                .WithQueryParam("name", getLikedContentRequest.name)
                .WithQueryParam("type", getLikedContentRequest.type)
                .WithQueryParam("subtype", getLikedContentRequest.subtype)
                .WithQueryParam("isOfficial", getLikedContentRequest.isOfficial ? "true" : "false")
                .WithQueryParam("limit", getLikedContentRequest.limit >= 0 ? getLikedContentRequest.limit.ToString() : string.Empty)
                .WithQueryParam("offset", getLikedContentRequest.offset >= 0 ? getLikedContentRequest.offset.ToString() : string.Empty)
                .WithQueryParam("sortBy", ConvertUGCSortByToString(getLikedContentRequest.sortBy))
                .WithQueryParam("orderBy", ConvertUGCOrderByToString(getLikedContentRequest.orderBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCContentsPagingResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetLikedContents( GetAllLikedContentRequest getLikedContentRequest
            , ResultCallback<UGCContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't search content! Namespace parameter is null!");

            string tags = "";
            if(getLikedContentRequest.Tags.Length > 0 && getLikedContentRequest.Tags != null)
            {
                tags = string.Join(",", getLikedContentRequest.Tags);
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/contents/liked")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("tags", tags)
                .WithQueryParam("name", getLikedContentRequest.Name)
                .WithQueryParam("type", getLikedContentRequest.Type)
                .WithQueryParam("subtype", getLikedContentRequest.Subtype)
                .WithQueryParam("isOfficial", getLikedContentRequest.IsOfficial ? "true" : "false")
                .WithQueryParam("limit", getLikedContentRequest.Limit >= 0 ? getLikedContentRequest.Limit.ToString() : string.Empty)
                .WithQueryParam("offset", getLikedContentRequest.Offset >= 0 ? getLikedContentRequest.Offset.ToString() : string.Empty)
                .WithQueryParam("sortBy", ConvertUGCLikedContentSortByToString(getLikedContentRequest.SortBy))
                .WithQueryParam("orderBy", ConvertUGCOrderByToString(getLikedContentRequest.OrderBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => 
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCContentsPagingResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetCreatorStats(string userId
            , ResultCallback<UGCGetCreatorStatsResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't GetCreatorStats! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't GetCreatorStats! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCGetCreatorStatsResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetUserGroups(string userId
            , ResultCallback<UGCGetUserGroupsPagingResponse> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Followed Creators! Namespace parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/groups")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCGetUserGroupsPagingResponse>();
            callback.Try(result);
        }

        public IEnumerator UpdateChannel(UpdateChannelRequest requestModel
            , UpdateChannelParameter requestParameter
            , ResultCallback<UGCChannelResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get List Followers! Namespace parameter is null!");
            Assert.IsNotNull(requestParameter.UserId, "Can't UpdateChannel! userId parameter is null!");
            Assert.IsNotNull(requestParameter.ChannelId, "Can't UpdateChannel! channelId parameter is null!");
            Assert.IsNotNull(requestModel.Name, "Can't UpdateChannel! name parameter is null!");

            var request = HttpRequestBuilder
                 .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/channels/{channelId}")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("userId", requestParameter.UserId)
                 .WithPathParam("channelId", requestParameter.ChannelId)
                 .WithBody(requestModel.ToUtf8Json())
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UGCChannelResponse>();
            callback.Try(result);
        }

        public IEnumerator SearchContentsSpecificToChannelV2(string channelId
            , ResultCallback<UGCSearchContentsPagingResponseV2> callback
            , int limit
            , int offset
            , UGCContentDownloaderSortBy sortBy)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/channels/{channelId}/contents")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("channelId", channelId)
                 .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                 .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                 .WithQueryParam("sortBy", ConvertGetUGCDownloaderSortByToString(sortBy))
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCSearchContentsPagingResponseV2>();
            callback.Try(result);
        }

        public IEnumerator SearchContentsV2(UGCGetContentFilterRequestV2 filterRequest
            , ResultCallback<UGCSearchContentsPagingResponseV2> callback
            , int limit
            , int offset
            , UGCContentSortBy sortBy)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/contents")
                 .WithPathParam("namespace", Namespace_)
                 .WithQueryParam("name", filterRequest.Name)
                 .WithQueryParam("type", filterRequest.Type)
                 .WithQueryParam("subtype", filterRequest.SubType)
                 .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                 .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                 .WithQueryParam("sortBy", ConvertGetUGCContentsSortByToString(sortBy))
                 .WithQueryParam("tags", string.Join(",", filterRequest.Tags))
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCSearchContentsPagingResponseV2>();
            callback.Try(result);
        }

        public IEnumerator GetContentBulkByIdsV2(string[] contentIds
            , ResultCallback<UGCModelsContentsResponseV2[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (contentIds.Length <= 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentIds) + " cannot be null or empty"));
                yield break;
            }
            if (contentIds.Length > maxBulkContentIds)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentIds) + " cannot exceed " + maxBulkContentIds));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/contents/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBody(new { contentIds = contentIds }.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsContentsResponseV2[]>();
            callback.Try(result);
        }

        public IEnumerator GetContentByShareCodeV2(string shareCode
            , ResultCallback<UGCModelsContentsResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(shareCode))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(shareCode) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/contents/sharecodes/{shareCode}")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("shareCode", shareCode)
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsContentsResponseV2>();
            callback.Try(result);
        }

        public IEnumerator GetContentByContentIdV2(string contentId
            , ResultCallback<UGCModelsContentsResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/contents/{contentId}")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("contentId", contentId)
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsContentsResponseV2>();
            callback.Try(result);
        }

        public IEnumerator CreateContentV2(string userId
            , string channelId
            , CreateUGCRequestV2 createRequest
            , ResultCallback<UGCModelsCreateUGCResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }
            if (createRequest == null)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(createRequest) + " cannot be null or empty"));
                yield break;
            }

            CreateUGCRequestV2 Req = createRequest;
            if (string.IsNullOrEmpty(Req.ContentType))
            {
                Req.ContentType = "application/octet-stream";
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Req.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsCreateUGCResponseV2>();
            callback.Try(result);
        }

        public IEnumerator DeleteContentV2(string userId
            , string channelId
            , string contentId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/{contentId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithPathParam("contentId", contentId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ModifyContentV2(string userId
            , string channelId
            , string contentId
            , ModifyUGCRequestV2 modifyRequest
            , ResultCallback<UGCModelsModifyUGCResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreatePatch(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/{contentId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithPathParam("contentId", contentId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(modifyRequest.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsModifyUGCResponseV2>();
            callback.Try(result);
        }

        public IEnumerator GenerateUploadContentURLV2(string userId
            , string channelId
            , string contentId
            , UGCUploadContentURLRequestV2 uploadRequest
            , ResultCallback<UGCModelsUploadContentURLResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(uploadRequest.FileExtension))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }

            UGCUploadContentURLRequestV2 Req = uploadRequest;
            if (string.IsNullOrEmpty(Req.ContentType))
            {
                Req.ContentType = "application/octet-stream";
            }

            var request = HttpRequestBuilder
                 .CreatePatch(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/{contentId}/uploadUrl")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithPathParam("contentId", contentId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Req.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsUploadContentURLResponseV2>();
            callback.Try(result);
        }

        public IEnumerator UpdateContentFileLocationV2(string userId
            , string channelId
            , string contentId
            , string fileExtension
            , string s3Key
            , ResultCallback<UpdateContentFileLocationResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(channelId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(fileExtension))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(fileExtension) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(s3Key))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(s3Key) + " cannot be null or empty"));
                yield break;
            }

            UpdateContentFileLocationRequestV2 Req = new UpdateContentFileLocationRequestV2
            {
                FileExtension = fileExtension,
                FileLocation = s3Key
            };

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/channels/{channelId}/contents/{contentId}/fileLocation")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("channelId", channelId)
                .WithPathParam("contentId", contentId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Req.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UpdateContentFileLocationResponseV2>();
            callback.Try(result);
        }

        public IEnumerator GetUserContentsV2(string userId
            , ResultCallback<UGCSearchContentsPagingResponseV2> callback
            , int limit
            , int offset
            , UGCContentDownloaderSortBy sortBy)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/contents")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("userId", userId)
                 .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                 .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                 .WithQueryParam("sortBy", ConvertGetUGCDownloaderSortByToString(sortBy))
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCSearchContentsPagingResponseV2>();
            callback.Try(result);
        }

        public IEnumerator UpdateContentScreenshotV2(string userId
            , string contentId
            , ScreenshotsUpdatesV2 screenshotsRequest
            , ResultCallback<ScreenshotsUpdatesV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/contents/{contentId}/screenshots")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("contentId", contentId)
                .WithBody(screenshotsRequest.ToJsonString().ToLower())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<ScreenshotsUpdatesV2>();
            callback.Try(result);
        }

        public IEnumerator UploadContentScreenshotV2(string userId
            , string contentId
            , ScreenshotsRequest screenshotsRequest
            , ResultCallback<ScreenshotsResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }
            if (screenshotsRequest == null)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(screenshotsRequest) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/contents/{contentId}/screenshots")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("contentId", contentId)
                .WithBody(screenshotsRequest.ToJsonString().ToLower())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<ScreenshotsResponseV2>();
            callback.Try(result);
        }

        public IEnumerator DeleteContentScreenshotV2(string userId
            , string contentId
            , string screenshotId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(screenshotId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(screenshotId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/contents/{contentId}/screenshots/{screenshotId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("contentId", contentId)
                .WithPathParam("screenshotId", screenshotId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator AddDownloadContentCountV2(string contentId
            , ResultCallback<UGCModelsAddDownloadContentCountResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/contents/{contentId}/downloadcount")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("contentId", contentId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsAddDownloadContentCountResponseV2>();
            callback.Try(result);
        }

        public IEnumerator GetListContentDownloaderV2(string contentId
            , ResultCallback<UGCModelsPaginatedContentDownloaderResponseV2> callback
            , string userId
            , int limit
            , int offset
            , UGCContentDownloaderSortBy sortBy)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/contents/{contentId}/downloader")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("contentId", contentId)
                 .WithQueryParam("userId", userId)
                 .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                 .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                 .WithQueryParam("sortBy", ConvertGetUGCDownloaderSortByToString(sortBy))
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsPaginatedContentDownloaderResponseV2>();
            callback.Try(result);
        }

        public IEnumerator GetListContentLikerV2(string contentId
            , ResultCallback<UGCModelsPaginatedContentLikerResponseV2> callback
            , int limit
            , int offset
            , UGCContentDownloaderSortBy sortBy)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(contentId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/contents/{contentId}/like")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("contentId", contentId)
                 .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                 .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                 .WithQueryParam("sortBy", ConvertGetUGCDownloaderSortByToString(sortBy))
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCModelsPaginatedContentLikerResponseV2>();
            callback.Try(result);
        }

        public IEnumerator UpdateLikeStatusToContentV2(string contentId
            , bool likeStatus
            , ResultCallback<UGCUpdateLikeStatusToContentResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(contentId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                 .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/contents/{contentId}/like")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("contentId", contentId)
                 .WithBody(new
                 {
                     likeStatus = likeStatus
                 }.ToUtf8Json())
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBearerAuth(AuthToken)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UGCUpdateLikeStatusToContentResponse>();
            callback.Try(result);
        }
    }
}
