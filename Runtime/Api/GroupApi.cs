﻿// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;
using System.Collections.Generic;

namespace AccelByte.Api
{
    public class GroupApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==GroupServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal GroupApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.GroupServerUrl, session)
        {
        }

        public IEnumerator CreateGroup(CreateGroupRequest createGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/groups")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(createGroupRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator CreateGroupV2(CreateGroupRequest createGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/groups")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(createGroupRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator SearchGroups(ResultCallback<PaginatedGroupListResponse> callback
            , string groupName = ""
            , string groupRegion = ""
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/groups")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("groupName", groupName)
                .WithQueryParam("groupRegion", groupRegion)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupListResponse>();
            callback?.Try(result);
        }

        public IEnumerator GetGroup(string groupId
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator UpdateGroup(string groupId
            , UpdateGroupRequest updateGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (updateGroupRequest.groupType == GroupType.NONE)
            {
                UpdateGroupNoTypeRequest updateRequest = new UpdateGroupNoTypeRequest
                {
                    customAttributes = updateGroupRequest.customAttributes,
                    groupDescription = updateGroupRequest.groupDescription,
                    groupIcon = updateGroupRequest.groupIcon,
                    groupName = updateGroupRequest.groupName,
                    groupRegion = updateGroupRequest.groupRegion
                };
                builder.WithBody(updateRequest.ToUtf8Json());
            }
            else
            {
                builder.WithBody(updateGroupRequest.ToUtf8Json());
            }

            var request = builder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator UpdateGroupV2(string groupId
            , UpdateGroupRequest updateGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (updateGroupRequest.groupType == GroupType.NONE)
            {
                UpdateGroupNoTypeRequest updateRequest = new UpdateGroupNoTypeRequest
                {
                    customAttributes = updateGroupRequest.customAttributes,
                    groupDescription = updateGroupRequest.groupDescription,
                    groupIcon = updateGroupRequest.groupIcon,
                    groupName = updateGroupRequest.groupName,
                    groupRegion = updateGroupRequest.groupRegion
                };
                builder.WithBody(updateRequest.ToUtf8Json());
            }
            else
            {
                builder.WithBody(updateGroupRequest.ToUtf8Json());
            }

            var request = builder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator DeleteGroup(string groupId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator DeleteGroupV2(string groupId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator UpdateGroupCustomRule(string groupId
            , Dictionary<string, object> ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            UpdateGroupCustomRuleRequest customRule =
                new UpdateGroupCustomRuleRequest { groupCustomRule = ruleUpdateRequest };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/rules/custom")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(customRule.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }
        
        public IEnumerator UpdateGroupCustomRuleV2(string groupId
            , Dictionary<string, object> ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            UpdateGroupCustomRuleRequest customRule =
                new UpdateGroupCustomRuleRequest { groupCustomRule = ruleUpdateRequest };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/rules/custom")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithJsonBody(customRule)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator UpdateGroupPredefinedRule(string groupId
            , AllowedAction allowedAction
            , UpdateGroupPredefinedRuleRequest ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error == null && allowedAction == AllowedAction.None)
            {
                error = new Error(ErrorCode.InvalidRequest, $"allowedAction parameter cannot be AllowedAction.None!");
            }
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/rules/defined/{allowedAction}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithPathParam("allowedAction", allowedAction.ToString())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(ruleUpdateRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }
        
        public IEnumerator UpdateGroupPredefinedRuleV2(string groupId
            , AllowedAction allowedAction
            , UpdateGroupPredefinedRuleRequest ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error == null && allowedAction == AllowedAction.None)
            {
                error = new Error(ErrorCode.InvalidRequest, $"allowedAction parameter cannot be AllowedAction.None!");
            }
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/rules/defined/{allowedAction}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithPathParam("allowedAction", allowedAction.ToString())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(ruleUpdateRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator DeleteGroupPredefinedRule(string groupId
            , AllowedAction allowedAction
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error == null && allowedAction == AllowedAction.None)
            {
                error = new Error(ErrorCode.InvalidRequest, $"allowedAction parameter cannot be AllowedAction.None!");
            }
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl +
                              "/v1/public/namespaces/{namespace}/groups/{groupId}/rules/defined/{allowedAction}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithPathParam("allowedAction", allowedAction.ToString())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }
        
        public IEnumerator DeleteGroupPredefinedRuleV2(string groupId
            , AllowedAction allowedAction
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error == null && allowedAction == AllowedAction.None)
            {
                error = new Error(ErrorCode.InvalidRequest, $"allowedAction parameter cannot be AllowedAction.None!");
            }
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl +
                              "/v2/public/namespaces/{namespace}/groups/{groupId}/rules/defined/{allowedAction}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithPathParam("allowedAction", allowedAction.ToString())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator GetUserGroupInfo(string userId
            , ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupMemberInformation>();
            callback?.Try(result);
        }

        public IEnumerator AcceptGroupInvitation(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/invite/accept")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator AcceptGroupInvitationV2(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/invite/accept")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator RejectGroupInvitation(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/invite/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator RejectGroupInvitationV2(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/invite/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator InviteOtherUserToGroup(string userId
            , ResultCallback<UserInvitationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/invite")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserInvitationResponse>();
            callback?.Try(result);
        }

        public IEnumerator InviteOtherUserToGroupV2(string userId, string groupId
            , ResultCallback<UserInvitationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/groups/{groupId}/invite")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserInvitationResponse>();
            callback?.Try(result);
        }

        public IEnumerator JoinGroup(string groupId
            , ResultCallback<JoinGroupResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/join")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<JoinGroupResponse>();
            callback?.Try(result);
        }

        public IEnumerator JoinGroupV2(string groupId
            , ResultCallback<JoinGroupResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/join")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<JoinGroupResponse>();
            callback?.Try(result);
        }

        public IEnumerator CancelJoinGroupRequest(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/join/cancel")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator GetGroupMemberList(string groupId
            , ResultCallback<PaginatedGroupMemberList> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupMemberList>();
            callback?.Try(result);
        }

        public IEnumerator KickGroupMember(string userId
            , ResultCallback<KickMemberResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/kick")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<KickMemberResponse>();
            callback?.Try(result);
        }

        public IEnumerator KickGroupMemberV2(string userId, string groupId
            , ResultCallback<KickMemberResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/groups/{groupId}/kick")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<KickMemberResponse>();
            callback?.Try(result);
        }

        public IEnumerator LeaveGroup(ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/leave")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator LeaveGroupV2(string groupId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/leave")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator GetGroupJoinRequests(string groupId
            , ResultCallback<PaginatedGroupRequestList> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/join/request")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback?.Try(result);
        }
        
        public IEnumerator GetGroupJoinRequestsV2(string groupId
            , ResultCallback<PaginatedGroupRequestList> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/join/request")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback?.Try(result);
        }
        
        public IEnumerator GetMyJoinRequest(ResultCallback<PaginatedGroupRequestList> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/me/join/request")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback?.Try(result);
        }

        public IEnumerator GetGroupInvitationRequests(ResultCallback<PaginatedGroupRequestList> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/invite/request")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback?.Try(result);
        }
        
        public IEnumerator GetGroupInvitationRequestsV2(string groupId,ResultCallback<PaginatedGroupRequestList> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/invite/request")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithPathParam("groupId",groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback?.Try(result);
        }
        

        public IEnumerator AcceptOtherJoinRequest(string userId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/join/accept")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator AcceptOtherJoinRequestV2(string userId, string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/groups/{groupId}/join/accept")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator RejectOtherJoinRequest(string userId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/join/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator RejectOtherJoinRequestV2(string userId, string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/groups/{groupId}/join/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback?.Try(result);
        }

        public IEnumerator AssignRoleToMember(string memberRoleId
            , string userId
            , ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, memberRoleId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            AssignRoleRequest assignedUser = new AssignRoleRequest { userId = userId };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/roles/{memberRoleId}/members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithBearerAuth(AuthToken)
                .WithBody(assignedUser.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupMemberInformation>();
            callback?.Try(result);
        }

        public IEnumerator AssignRoleToMemberV2(string memberRoleId
            , string userId, string groupId
            , ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, memberRoleId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            AssignRoleRequest assignedUser = new AssignRoleRequest { userId = userId };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/roles/{memberRoleId}/groups/{groupId}/members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithBody(assignedUser.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GroupMemberInformation>();
            callback?.Try(result);
        }

        public IEnumerator RemoveRoleFromMember(string memberRoleId
            , string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, memberRoleId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            AssignRoleRequest removedUser = new AssignRoleRequest { userId = userId };

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/roles/{memberRoleId}/members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithBearerAuth(AuthToken)
                .WithBody(removedUser.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator RemoveRoleFromMemberV2(string memberRoleId
            , string userId, string groupId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, memberRoleId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            AssignRoleRequest removedUser = new AssignRoleRequest { userId = userId };

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl +
                              "/v2/public/namespaces/{namespace}/roles/{memberRoleId}/groups/{groupId}/members")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithBody(removedUser.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator GetMemberRoles(ResultCallback<PaginatedMemberRoles> callback
            , int limit = 0
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/roles")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedMemberRoles>();
            callback?.Try(result);
        }

        public IEnumerator GetGroupsByGroupIds(string[] groupIds, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupIds);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestModel = new GetGroupsByGroupIdsRequest { groupIDs = groupIds };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/groups/bulk")
                .WithBearerAuth(AuthToken)
                .WithPathParam("namespace", Namespace_)
                .WithBody(requestModel.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupListResponse>();

            callback?.Try(result);
        }

        public IEnumerator GetUserJoinedGroups(ResultCallback<PaginatedGroupMemberList> callback, int limit = 0,
            int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/me/groups")
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupMemberList>();

            callback?.Try(result);
        }

        public IEnumerator CancelGroupMemberInvitation(string userId, string groupId,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/groups/{groupId}/invite/cancel")
                .WithPathParam("userId", userId)
                .WithPathParam("groupId", groupId)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();

            callback?.Try(result);
        }

        public IEnumerator UpdateGroupCustomAttributes(string groupId, Dictionary<string, object> customAttributes,
            ResultCallback<GroupInformation> callback)
        {

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, groupId, customAttributes);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var updateRequest = new UpdateCustomAttributesRequest { customAttributes = customAttributes };
            
            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/groups/{groupId}/attributes/custom")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(AuthToken)
                .WithJsonBody(updateRequest)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback?.Try(result);
        }

        public IEnumerator GetUserGroupStatusInfo(string userId, string groupId,
            ResultCallback<GroupMemberInformation> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, groupId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/groups/{groupId}/status")
                .WithPathParam("userId", userId)
                .WithPathParam("groupId", groupId)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupMemberInformation>();
            callback?.Try(result);
        }
    }
}