// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Group service.
    /// </summary>
    public class Group
    {
        private readonly GroupApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;
        private readonly string @namespace;

        internal Group(GroupApi api, ISession session, string namespace_, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Create new group. It will use the authorization to determine the user ID which will be used as the group admin.
        /// </summary>
        /// <param name="createGroupRequest">New group detail request.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void CreateGroup(CreateGroupRequest createGroupRequest, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateGroup(this.@namespace, this.session.AuthorizationToken, createGroupRequest, callback));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="groupRegion">The region you want to search, leave it blank will fetch group from all existing region. (optional)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName, string groupRegion, int limit, int offset, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchGroups(this.@namespace, this.session.AuthorizationToken, callback, groupName, groupRegion, limit, offset));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName, int limit, int offset, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchGroups(this.@namespace, this.session.AuthorizationToken, callback, groupName, "", limit, offset));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="groupRegion">The region you want to search, leave it blank will fetch group from all existing region. (optional)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName, string groupRegion, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchGroups(this.@namespace, this.session.AuthorizationToken, callback, groupName, groupRegion));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchGroups(this.@namespace, this.session.AuthorizationToken, callback, groupName));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(int limit, int offset, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchGroups(this.@namespace, this.session.AuthorizationToken, callback, "", "", limit, offset));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchGroups(this.@namespace, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get single group information based on its id.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void GetGroup(string groupId, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group information! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroup(this.@namespace, this.session.AuthorizationToken, groupId, callback));
        }

        /// <summary>
        /// Update specific single group information.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="updateGroupRequest">The new information of the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroup(string groupId, UpdateGroupRequest updateGroupRequest, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group information! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateGroup(this.@namespace, this.session.AuthorizationToken, groupId, updateGroupRequest, callback));
        }

        /// <summary>
        /// Update specific single group custom attributes information.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="customAttributes">The new custom attributes information.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomAttributes(string groupId, Dictionary<string, object> customAttributes, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group information! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            UpdateGroupRequest request = new UpdateGroupRequest
            {
                customAttributes = customAttributes
            };

            this.coroutineRunner.Run(
                this.api.UpdateGroup(this.@namespace, this.session.AuthorizationToken, groupId, request, callback));
        }

        /// <summary>
        /// Delete specific single group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void DeleteGroup(string groupId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't delete group! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteGroup(this.@namespace, this.session.AuthorizationToken, groupId, callback));
        }

        /// <summary>
        /// Update group custom rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="ruleUpdateRequest">The new custom rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomRule(string groupId, Dictionary<string, object> ruleUpdateRequest, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group custom rule! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateGroupCustomRule(this.@namespace, this.session.AuthorizationToken, groupId, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Update predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="ruleUpdateRequest">The new predefined rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed</param>
        public void UpdateGroupPredefinedRule(string groupId, AllowedAction allowedAction, UpdateGroupPredefinedRuleRequest ruleUpdateRequest, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group predefined rule! GroupId parameter is null!");
            Assert.AreNotEqual(AllowedAction.None, allowedAction, "Can't update group predefined rule! allowedAction parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateGroupPredefinedRule(this.@namespace, this.session.AuthorizationToken, groupId, allowedAction, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Delete predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGroupPredefinedRule(string groupId, AllowedAction allowedAction,  ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group predefined rule! GroupId parameter is null!");
            Assert.AreNotEqual(AllowedAction.None, allowedAction, "Can't update group predefined rule! allowedAction parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteGroupPredefinedRule(this.@namespace, this.session.AuthorizationToken, groupId, allowedAction, callback));
        }

        /// <summary>
        /// Get user's group information.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void GetMyGroupInfo(ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserGroupInfo(this.@namespace, this.session.UserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get other user's group information.
        /// </summary>
        /// <param name="userId">other user's Id</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void GetOtherGroupInfo(string userId, ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserGroupInfo(this.@namespace, userId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Accepting group invitation.
        /// </summary>
        /// <param name="groupId">The group id you want to accept.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptGroupInvitation(string groupId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't accept group invitation! groupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.AcceptGroupInvitation(this.@namespace, groupId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Rejecting group invitation.
        /// </summary>
        /// <param name="groupId">The group id you want to reject.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectGroupInvitation(string groupId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't accept group invitation! groupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.RejectGroupInvitation(this.@namespace, groupId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Invite the other user to your group.
        /// </summary>
        /// <param name="otherUserId">The other user id who will be invited into specific group.</param>
        /// <param name="callback">Returns a Result that contains UserInvitationResponse via callback when completed.</param>
        public void InviteOtherUserToGroup(string otherUserId, ResultCallback<UserInvitationResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't invite other user to group! OtherUserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.InviteOtherUserToGroup(this.@namespace, otherUserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Join into specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void JoinGroup(string groupId, ResultCallback<JoinGroupResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't join group! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.JoinGroup(this.@namespace, this.session.AuthorizationToken, groupId, callback));
        }

        /// <summary>
        /// Cancel the Join group request.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void CancelJoinGroupRequest(string groupId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't cancel join group request! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CancelJoinGroupRequest(this.@namespace, this.session.AuthorizationToken, groupId, callback));
        }

        /// <summary>
        /// Get member list of specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void GetGroupMemberList(string groupId, ResultCallback<PaginatedGroupMemberList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group member list! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroupMemberList(this.@namespace, this.session.AuthorizationToken, groupId, callback));
        }

        /// <summary>
        /// Get member list of specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void GetGroupMemberList(string groupId, int limit, int offset, ResultCallback<PaginatedGroupMemberList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group member list! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroupMemberList(this.@namespace, this.session.AuthorizationToken, groupId, callback, limit, offset));
        }

        /// <summary>
        /// Kick a member out of group.
        /// </summary>
        /// <param name="otherUserId">The user id of the member who will be kicked out.</param>
        /// <param name="callback">Returns a Result that contains KickMemberResponse via callback when completed.</param>
        public void KickGroupMember(string otherUserId, ResultCallback<KickMemberResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't kick a group member! OtherUserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.KickGroupMember(this.@namespace, otherUserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Leave the group you're in.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void LeaveGroup(ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.LeaveGroup(this.@namespace, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get list of join request in group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupJoinRequests(string groupId, int limit, int offset, ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group join requests! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroupJoinRequests(this.@namespace, this.session.AuthorizationToken, groupId, callback, limit, offset));
        }

        /// <summary>
        /// Get list of join request in group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupJoinRequests(string groupId, ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group join requests! GroupId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroupJoinRequests(this.@namespace, this.session.AuthorizationToken, groupId, callback));
        }

        /// <summary>
        /// Get list of group invitation request.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupInvitationRequests(int limit, int offset, ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroupInvitationRequests(this.@namespace, this.session.AuthorizationToken, callback, limit, offset));
        }

        /// <summary>
        /// Get list of group invitation request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupInvitationRequests(ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGroupInvitationRequests(this.@namespace, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Accept other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptOtherJoinRequest(string otherUserId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't accept other user join request! UserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.AcceptOtherJoinRequest(this.@namespace, otherUserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Reject other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectOtherJoinRequest(string otherUserId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't reject other user join request! OtherUserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.RejectOtherJoinRequest(this.@namespace, otherUserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Assign a role to a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the assigned role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void AssignRoleToMember(string memberRoleId, string userId, ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(memberRoleId, "Can't assign group role request! MemberRoleId parameter is null!");
            Assert.IsNotNull(userId, "Can't assign group role request! UserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.AssignRoleToMember(this.@namespace, memberRoleId, userId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Remove a role from a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the removed role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void RemoveRoleFromMember(string memberRoleId, string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(memberRoleId, "Can't remove group role request! MemberRoleId parameter is null!");
            Assert.IsNotNull(userId, "Can't remove group role request! UserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.RemoveRoleFromMember(this.@namespace, memberRoleId, userId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get list of member role on the namespace.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedMemberRoles via callback when completed.</param>
        public void GetMemberRoles(int limit, int offset, ResultCallback<PaginatedMemberRoles> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetMemberRoles(this.@namespace, this.session.AuthorizationToken, callback, limit, offset));
        }

        /// <summary>
        /// Get list of member role on the namespace.
        /// </summary>
        /// <param name="callback">Returns a Result that contains PaginatedMemberRoles via callback when completed.</param>
        public void GetMemberRoles(ResultCallback<PaginatedMemberRoles> callback)
        {
            this.GetMemberRoles(0, 0, callback);
        }
    }
}
