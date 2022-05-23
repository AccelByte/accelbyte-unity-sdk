﻿// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
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
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==UGCServerUrl</param>
        /// <param name="session"></param>
        internal UGCApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base(httpClient, config, config.UGCServerUrl, session)
        {
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
            , UGCRequest modifyRequest
            , ResultCallback<UGCResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't modify content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't modify content! UserId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't modify content! AccessToken parameter is null!");
            Assert.IsNotNull(channelId, "Can't modify content! channelId parameter is null!");
            Assert.IsNotNull(contentId, "Can't modify content! name parameter is null!");
            Assert.IsNotNull(modifyRequest, "Can't modify content! type parameter is null!");

            UGCRequest Req = modifyRequest;
            if (string.IsNullOrEmpty(Req.contentType))
            {
                Req.contentType = "application/octet-stream";
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

            yield return ModifyContent(userId, channelId, contentId, modifyRequest, callback);
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
                byte[] bytes = Convert.FromBase64String(result.Value.preview);
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
            , int limit )
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

    }
}
