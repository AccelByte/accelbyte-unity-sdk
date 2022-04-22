// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
using Newtonsoft.Json;

namespace AccelByte.Api
{
    internal class GroupApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal GroupApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsFalse(string.IsNullOrEmpty(baseUrl), "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator CreateGroup(string namespace_, string accessToken, CreateGroupRequest createGroupRequest,
            ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't create new group! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't create new group! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/groups")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(createGroupRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback.Try(result);
        }

        public IEnumerator SearchGroups( string namespace_, string accessToken,
            ResultCallback<PaginatedGroupListResponse> callback, string groupName = "", string groupRegion = "", int limit = 0, int offset = 0)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get list of groups! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get list of groups! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/groups")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("groupName", groupName)
                .WithQueryParam("groupRegion", groupRegion)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupListResponse>();
            callback.Try(result);
        }

        public IEnumerator GetGroup(string namespace_, string accessToken, string groupId,
            ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get group information! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get group information! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't get group information! GroupId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback.Try(result);
        }

        public IEnumerator UpdateGroup(string namespace_, string accessToken, string groupId, UpdateGroupRequest updateGroupRequest,
            ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(namespace_, "Can't update group information! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't update group information! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't update group information! GroupId parameter is null!");


            var builder = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if(updateGroupRequest.groupType == GroupType.NONE)
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback.Try(result);
        }

        public IEnumerator DeleteGroup(string namespace_, string accessToken, string groupId,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't delete group! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't delete group! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't delete group! GroupId parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator UpdateGroupCustomRule(string namespace_, string accessToken, string groupId, Dictionary<string, object> ruleUpdateRequest,
            ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't update group custon rule! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't update group custon rule! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't update group custon rule! GroupId parameter is null!");

            UpdateGroupCustomRuleRequest customRule = new UpdateGroupCustomRuleRequest{
                groupCustomRule = ruleUpdateRequest
            };

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/rules/custom")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(customRule.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback.Try(result);
        }

        public IEnumerator UpdateGroupPredefinedRule(string namespace_, string accessToken, string groupId, AllowedAction allowedAction, UpdateGroupPredefinedRuleRequest ruleUpdateRequest,
            ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't update group predefined rule! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't update group predefined rule! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't update group predefined rule! GroupId parameter is null!");
            Assert.AreNotEqual(AllowedAction.None, allowedAction, "Can't update group predefined rule! allowedAction parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/rules/defined/{allowedAction}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithPathParam("allowedAction", allowedAction.ToString())
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(ruleUpdateRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupInformation>();
            callback.Try(result);
        }

        public IEnumerator DeleteGroupPredefinedRule(string namespace_, string accessToken, string groupId, AllowedAction allowedAction, 
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't delete group predefined rule! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't delete group predefined rule! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't delete group predefined rule! GroupId parameter is null!");
            Assert.AreNotEqual(AllowedAction.None, allowedAction, "Can't delete group predefined rule! allowedAction parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/rules/defined/{allowedAction}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithPathParam("allowedAction", allowedAction.ToString())
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetUserGroupInfo(string namespace_, string userId, string accessToken,
            ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get user's group information! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get user's group information! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't get user's group information! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupMemberInformation>();
            callback.Try(result);
        }

        public IEnumerator AcceptGroupInvitation(string namespace_, string groupId, string accessToken,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't accept group invitation! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't accept group invitation! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't accept group invitation! groupId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/invite/accept")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback.Try(result);
        }

        public IEnumerator RejectGroupInvitation(string namespace_, string groupId, string accessToken,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't reject group invitation! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't reject group invitation! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't reject group invitation! groupId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/invite/reject")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback.Try(result);
        }

        public IEnumerator InviteOtherUserToGroup(string namespace_, string userId, string accessToken,
            ResultCallback<UserInvitationResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't invite other user to group! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't invite other user to group! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't invite other user to group! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/invite")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserInvitationResponse>();
            callback.Try(result);
        }

        public IEnumerator JoinGroup(string namespace_, string accessToken, string groupId,
            ResultCallback<JoinGroupResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't join group! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't join group! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't join group! GroupId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/join")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<JoinGroupResponse>();
            callback.Try(result);
        }

        public IEnumerator CancelJoinGroupRequest(string namespace_, string accessToken, string groupId,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't cancel join group request! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't cancel join group request! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't cancel join group request! GroupId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/join/cancel")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback.Try(result);
        }

        public IEnumerator GetGroupMemberList(string namespace_, string accessToken, string groupId, ResultCallback<PaginatedGroupMemberList> callback, int limit = 0, int offset = 0)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get group member list! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get group member list ! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't get group member list! groupId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/members")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupMemberList>();
            callback.Try(result);
        }

        public IEnumerator KickGroupMember(string namespace_, string userId, string accessToken,
            ResultCallback<KickMemberResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't kick a group member! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't kick a group member! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't kick a group member! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/kick")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<KickMemberResponse>();
            callback.Try(result);
        }

        public IEnumerator LeaveGroup(string namespace_, string accessToken,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't leave group! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't leave group! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/leave")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback.Try(result);
        }

        public IEnumerator GetGroupJoinRequests(string namespace_, string accessToken, string groupId,
            ResultCallback<PaginatedGroupRequestList> callback, int limit = 0, int offset = 0)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get group join requests! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get group join requests! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(groupId), "Can't get group join requests! GroupId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/groups/{groupId}/join/request")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("groupId", groupId)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback.Try(result);
        }

        public IEnumerator GetGroupInvitationRequests(string namespace_, string accessToken,
            ResultCallback<PaginatedGroupRequestList> callback, int limit = 0, int offset = 0)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get group join requests! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get group join requests! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/users/me/invite/request")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupRequestList>();
            callback.Try(result);
        }

        public IEnumerator AcceptOtherJoinRequest(string namespace_, string userId, string accessToken,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't accept other user join request! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't accept other user join request! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't accept other user join request! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/join/accept")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback.Try(result);
        }

        public IEnumerator RejectOtherJoinRequest(string namespace_, string userId, string accessToken,
             ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't reject other user join request! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't reject other user join request! AccessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't reject other user join request! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/join/reject")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupGeneralResponse>();
            callback.Try(result);
        }

        public IEnumerator AssignRoleToMember(string namespace_, string memberRoleId, string userId, string accessToken, ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't assign group role request! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(memberRoleId), "Can't assign group role request! MemberRoleId parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't assign group role request! UserId parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't assign group role request! AccessToken parameter is null!");

            AssignRoleRequest assignedUser = new AssignRoleRequest
            {
                userId = userId
            };

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/roles/{memberRoleId}/members")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithBearerAuth(accessToken)
                .WithBody(assignedUser.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GroupMemberInformation>();
            callback.Try(result);
        }

        public IEnumerator RemoveRoleFromMember(string namespace_, string memberRoleId, string userId, string accessToken, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't remove group role request! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(memberRoleId), "Can't remove group role request! MemberRoleId parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "Can't remove group role request! UserId parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't remove group role request! AccessToken parameter is null!");

            AssignRoleRequest removedUser = new AssignRoleRequest
            {
                userId = userId
            };

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v1/public/namespaces/{namespace}/roles/{memberRoleId}/members")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithBearerAuth(accessToken)
                .WithBody(removedUser.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetMemberRoles(string namespace_, string accessToken, ResultCallback<PaginatedMemberRoles> callback, int limit = 0, int offset = 0)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can't get group role request! Namespace parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Can't get group role request! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/roles")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedMemberRoles>();
            callback(result);
        }
    }
}
