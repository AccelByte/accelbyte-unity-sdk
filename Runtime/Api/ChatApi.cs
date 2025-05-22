// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

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
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId, chatId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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

            callback?.Try(result);
        }

        public IEnumerator MuteGroupUserChat(string groupId, MuteGroupChatRequest muteGroupChatRequest,
            ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , groupId
                , muteGroupChatRequest
                , muteGroupChatRequest?.UserId
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            bool durationIsPositive = muteGroupChatRequest.Duration > 0;
            if (!durationIsPositive)
            {
                callback?.TryError(
                    new Error(
                        ErrorCode.InvalidRequest
                        , message: $"{nameof(muteGroupChatRequest.Duration)} cannot be less than or equal to 0"
                    )
                );
                yield break;
            }

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

            callback?.Try(result);
        }

        public IEnumerator UnmuteGroupUserChat(string groupId, UnmuteGroupChatRequest unmuteGroupChatRequest, ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , groupId
                , unmuteGroupChatRequest
                , unmuteGroupChatRequest?.UserId
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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

            callback?.Try(result);
        }

        public IEnumerator GetGroupChatSnapshot(string groupId, string chatId,
            ResultCallback<ChatSnapshotResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId, chatId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/topic/{topic}/snapshot/{chatId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("topic", GenerateGroupTopicId(groupId))
                .WithPathParam("chatId", chatId)
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

            callback?.Try(result);
        }

        public IEnumerator BanGroupUserChat(string groupId, BanGroupChatRequest banGroupChatRequest,
            ResultCallback<BanGroupChatResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , groupId
                , banGroupChatRequest
                , banGroupChatRequest?.UserIds
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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

            callback?.Try(result);
        }

        public IEnumerator UnbanGroupUserChat(string groupId, UnbanGroupChatRequest unbanGroupChatRequest,
            ResultCallback<UnbanGroupChatResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , groupId
                , unbanGroupChatRequest
                , unbanGroupChatRequest?.UserIds
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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

            callback?.Try(result);
        }
    }
}