// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public class ChatApi : ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ChatApi(IHttpClient inHttpClient
            , Config config
            , ISession session)
            : this(inHttpClient, config, session, null)
        {
        }

        [UnityEngine.Scripting.Preserve]
        internal ChatApi(IHttpClient httpClient
            , Config config
            , ISession session
            , HttpOperator httpOperator)
            : base(httpClient, config, config.ChatServerUrl, session, httpOperator)
        {
        }

        public Config GetConfig()
        {
            return Config;
        }

        public void OnBanNotificationReceived(Action<string> onHttpBearerRefreshed)
        {
            Report.GetFunctionLog(GetType().Name);
            ((AccelByteHttpClient)HttpClient).OnBearerAuthRejected(onHttpBearerRefreshed);
        }

        private static string GenerateGroupTopicId(string groupId)
        {
            return $"g.{groupId}";
        }

        public IEnumerator DeleteGroupChat(string groupId, string chatId, ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(groupId, nameof(groupId) + " cannot be null");
            Assert.IsNotNull(chatId, nameof(chatId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/chats/{chats}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithPathParam("chats", chatId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator MuteGroupUserChat(string groupId, MuteGroupChatRequest muteGroupChatRequest,
            ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(groupId, nameof(groupId) + " cannot be null");
            Assert.IsNotNull(muteGroupChatRequest.UserId, nameof(muteGroupChatRequest.UserId) + " cannot be null");
            bool durationIsPositive = muteGroupChatRequest.Duration > 0;
            Assert.IsTrue(durationIsPositive, nameof(muteGroupChatRequest.Duration) + " cannot be less than or equal 0");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/mute")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithBearerAuth(AuthToken)
                .WithBody(muteGroupChatRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator UnmuteGroupUserChat(string groupId, UnmuteGroupChatRequest unmuteGroupChatRequest, ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(groupId, nameof(groupId) + " cannot be null");
            Assert.IsNotNull(unmuteGroupChatRequest.UserId, nameof(unmuteGroupChatRequest.UserId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/unmute")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithBearerAuth(AuthToken)
                .WithBody(unmuteGroupChatRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator GetGroupChatSnapshot(string groupId, string chatId,
            ResultCallback<ChatSnapshotResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(groupId, nameof(groupId) + " cannot be null");
            Assert.IsNotNull(chatId, nameof(chatId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/unmute/snapshot/{snapshot}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithPathParam("snapshot", chatId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ChatSnapshotResponse>();

            callback.Try(result);
        }

        public IEnumerator BanGroupUserChat(string groupId, BanGroupChatRequest banGroupChatRequest,
            ResultCallback<BanGroupChatResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(groupId, nameof(groupId) + " cannot be null");
            Assert.IsNotNull(banGroupChatRequest.UserIds, nameof(banGroupChatRequest.UserIds) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/ban-members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithBearerAuth(AuthToken)
                .WithBody(banGroupChatRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<BanGroupChatResponse>();

            callback.Try(result);
        }

        public IEnumerator UnbanGroupUserChat(string groupId, UnbanGroupChatRequest unbanGroupChatRequest,
            ResultCallback<UnbanGroupChatResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(groupId, nameof(groupId) + " cannot be null");
            Assert.IsNotNull(unbanGroupChatRequest.UserIds, nameof(unbanGroupChatRequest.UserIds) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/unban-members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithBearerAuth(AuthToken)
                .WithBody(unbanGroupChatRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<UnbanGroupChatResponse>();

            callback.Try(result);
        }
    }
}