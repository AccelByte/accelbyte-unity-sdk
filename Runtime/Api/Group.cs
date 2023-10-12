// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Group service.
    /// </summary>
    public class Group : WrapperBase
    {
        private readonly GroupApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly UserSession session;

        [UnityEngine.Scripting.Preserve]
        internal Group(GroupApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal Group(GroupApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner)
            : this(inApi, inSession, inCoroutineRunner) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Create new group. It will use the authorization to determine the user ID which will be used as the group admin.
        /// </summary>
        /// <param name="createGroupRequest">New group detail request.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void CreateGroup(CreateGroupRequest createGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateGroup(createGroupRequest, callback));
        }

        /// <summary>
        /// Create new group. It will use the authorization to determine the user ID which will be used as the group admin.
        /// </summary>
        /// <param name="createGroupRequest">New group detail request.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void CreateGroupV2(CreateGroupRequest createGroupRequest, ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateGroupV2(createGroupRequest, callback));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="groupRegion">The region you want to search, leave it blank will fetch group from all existing region. (optional)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName
            , string groupRegion
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchGroups(callback, groupName, groupRegion, limit, offset));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchGroups(callback, groupName, "", limit, offset));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="groupRegion">The region you want to search, leave it blank will fetch group from all existing region. (optional)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName
            , string groupRegion
            , ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchGroups(callback, groupName, groupRegion));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(string groupName
            , ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchGroups(callback, groupName));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(int limit
            , int offset
            , ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchGroups(callback, "", "", limit, offset));
        }

        /// <summary>
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups(ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchGroups(callback));
        }

        /// <summary>
        /// Get single group information based on its id.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void GetGroup(string groupId
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group information! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroup(groupId, callback));
        }

        /// <summary>
        /// Update specific single group information.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="updateGroupRequest">The new information of the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroup(string groupId
            , UpdateGroupRequest updateGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group information! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroup(groupId, updateGroupRequest, callback));
        }

        /// <summary>
        /// Update specific single group information.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="updateGroupRequest">The new information of the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupV2(string groupId
            , UpdateGroupRequest updateGroupRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group information! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupV2(groupId, updateGroupRequest, callback));
        }

        /// <summary>
        /// Update specific single group custom attributes information.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="customAttributes">The new custom attributes information.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomAttributes(string groupId
            , Dictionary<string, object> customAttributes
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group information! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            UpdateGroupRequest request = new UpdateGroupRequest { customAttributes = customAttributes };

            coroutineRunner.Run(
                api.UpdateGroup(groupId, request, callback));
        }

        /// <summary>
        /// Delete specific single group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void DeleteGroup(string groupId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't delete group! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGroup(groupId, callback));
        }

        /// <summary>
        /// Delete specific single group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void DeleteGroupV2(string groupId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't delete group! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGroupV2(groupId, callback));
        }

        /// <summary>
        /// Update group custom rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="ruleUpdateRequest">The new custom rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomRule(string groupId
            , Dictionary<string, object> ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group custom rule! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupCustomRule(groupId, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Update group custom rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="ruleUpdateRequest">The new custom rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomRuleV2(string groupId
            , Dictionary<string, object> ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group custom rule! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupCustomRuleV2(groupId, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Update predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="ruleUpdateRequest">The new predefined rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed</param>
        public void UpdateGroupPredefinedRule(string groupId
            , AllowedAction allowedAction
            , UpdateGroupPredefinedRuleRequest ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group predefined rule! groupId parameter is null!"));
                return;
            }

            if(allowedAction == AllowedAction.None)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group predefined rule! allowedAction parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupPredefinedRule(groupId, allowedAction, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Update predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="ruleUpdateRequest">The new predefined rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed</param>
        public void UpdateGroupPredefinedRuleV2(string groupId
            , AllowedAction allowedAction
            , UpdateGroupPredefinedRuleRequest ruleUpdateRequest
            , ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group predefined rule! groupId parameter is null!"));
                return;
            }

            if (allowedAction == AllowedAction.None)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group predefined rule! allowedAction parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupPredefinedRuleV2(groupId, allowedAction, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Delete predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGroupPredefinedRule(string groupId
            , AllowedAction allowedAction
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't delete group predefined rule! groupId parameter is null!"));
                return;
            }

            if (allowedAction == AllowedAction.None)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't delete group predefined rule! allowedAction parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGroupPredefinedRule(groupId, allowedAction, callback));
        }

        /// <summary>
        /// Delete predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGroupPredefinedRuleV2(string groupId
            , AllowedAction allowedAction
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't delete group predefined rule! groupId parameter is null!"));
                return;
            }

            if (allowedAction == AllowedAction.None)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't delete group predefined rule! allowedAction parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGroupPredefinedRuleV2(groupId, allowedAction, callback));
        }

        /// <summary>
        /// Get user's group information.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void GetMyGroupInfo(ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserGroupInfo(session.UserId, callback));
        }

        /// <summary>
        /// Get other user's group information.
        /// </summary>
        /// <param name="userId">other user's Id</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void GetOtherGroupInfo(string userId
            , ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserGroupInfo(userId, callback));
        }

        /// <summary>
        /// Accepting group invitation.
        /// </summary>
        /// <param name="groupId">The group id you want to accept.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptGroupInvitation(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't accept group invitation! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptGroupInvitation(groupId, callback));
        }

        /// <summary>
        /// Accepting group invitation.
        /// </summary>
        /// <param name="groupId">The group id who sent you the invitation</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptGroupInvitationV2(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't accept group invitation! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptGroupInvitationV2(groupId, callback));
        }

        /// <summary>
        /// Rejecting group invitation.
        /// </summary>
        /// <param name="groupId">The group id you want to reject.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectGroupInvitation(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't reject group invitation! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RejectGroupInvitation(groupId, callback));
        }

        /// <summary>
        /// Rejecting group invitation.
        /// </summary>
        /// <param name="groupId">The group id you want to reject.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectGroupInvitationV2(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't reject group invitation! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RejectGroupInvitationV2(groupId, callback));
        }

        /// <summary>
        /// Invite the other user to your group.
        /// </summary>
        /// <param name="otherUserId">The other user id who will be invited into specific group.</param>
        /// <param name="callback">Returns a Result that contains UserInvitationResponse via callback when completed.</param>
        public void InviteOtherUserToGroup(string otherUserId
            , ResultCallback<UserInvitationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.InviteOtherUserToGroup(otherUserId, callback));
        }

        /// <summary>
        /// Invite the other user to your group.
        /// </summary>
        /// <param name="otherUserId">The other user id who will be invited into specific group.</param>
        /// <param name="callback">Returns a Result that contains UserInvitationResponse via callback when completed.</param>
        public void InviteOtherUserToGroupV2(string otherUserId, string groupId
            , ResultCallback<UserInvitationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't invite other user to group! groupId parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.InviteOtherUserToGroupV2(otherUserId, groupId, callback));
        }

        /// <summary>
        /// Join into specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void JoinGroup(string groupId
            , ResultCallback<JoinGroupResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't invite other user to group! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.JoinGroup(groupId, callback));
        }

        /// <summary>
        /// Join into specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void JoinGroupV2(string groupId
            , ResultCallback<JoinGroupResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't join member! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.JoinGroupV2(groupId, callback));
        }

        /// <summary>
        /// Cancel the Join group request.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void CancelJoinGroupRequest(string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't cancel join group request! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CancelJoinGroupRequest(groupId, callback));
        }

        /// <summary>
        /// Get member list of specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void GetGroupMemberList(string groupId
            , ResultCallback<PaginatedGroupMemberList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group member list! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupMemberList(groupId, callback));
        }

        /// <summary>
        /// Get member list of specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void GetGroupMemberList(string groupId
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupMemberList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group member list! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupMemberList(groupId, callback, limit, offset));
        }

        /// <summary>
        /// Kick a member out of group.
        /// </summary>
        /// <param name="otherUserId">The user id of the member who will be kicked out.</param>
        /// <param name="callback">Returns a Result that contains KickMemberResponse via callback when completed.</param>
        public void KickGroupMember(string otherUserId
            , ResultCallback<KickMemberResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.KickGroupMember(otherUserId, callback));
        }

        /// <summary>
        /// Kick a member out of group.
        /// </summary>
        /// <param name="otherUserId">The user id of the member who will be kicked out.</param>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains KickMemberResponse via callback when completed.</param>
        public void KickGroupMemberV2(string otherUserId, string groupId
            , ResultCallback<KickMemberResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if(string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't kick a group member! groupId parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.KickGroupMemberV2(otherUserId, groupId, callback));
        }

        /// <summary>
        /// Leave the group you're in.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void LeaveGroup(ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.LeaveGroup(callback));
        }

        /// <summary>
        /// Leave the group you're in.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void LeaveGroupV2(string groupId, ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't leave group! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.LeaveGroupV2(groupId, callback));
        }

        /// <summary>
        /// Get list of join request in group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupJoinRequests(string groupId
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group join requests! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupJoinRequests(groupId, callback, limit, offset));
        }

        /// <summary>
        /// Get list of join request in group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupJoinRequests(string groupId
            , ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group join requests! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupJoinRequests(groupId, callback));
        }

        /// <summary>
        /// Get list of join request in group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupJoinRequestsV2(string groupId, int limit, int offset
            , ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group join requests! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupJoinRequestsV2(groupId, callback, limit, offset));
        }

        /// <summary>
        /// Get list of group invitation request.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupInvitationRequests(int limit
            , int offset
            , ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupInvitationRequests(callback, limit, offset));
        }

        /// <summary>
        /// Get list of group invitation request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupInvitationRequests(ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupInvitationRequests(callback));
        }

        /// <summary>
        /// Get list of group invitation request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupInvitationRequestsV2(string groupId, ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get group join requests! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupInvitationRequestsV2(groupId, callback));
        }

        /// <summary>
        /// Accept other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptOtherJoinRequest(string otherUserId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptOtherJoinRequest(otherUserId, callback));
        }

        /// <summary>
        /// Accept other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptOtherJoinRequestV2(string otherUserId, string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't accept other user join request! groupId parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptOtherJoinRequestV2(otherUserId, groupId, callback));
        }

        /// <summary>
        /// Reject other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectOtherJoinRequest(string otherUserId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {   
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RejectOtherJoinRequest(otherUserId, callback));
        }

        /// <summary>
        /// Reject other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectOtherJoinRequestV2(string otherUserId, string groupId
            , ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't reject group join requests! groupId parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RejectOtherJoinRequestV2(otherUserId, groupId, callback));
        }

        /// <summary>
        /// Assign a role to a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the assigned role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void AssignRoleToMember(string memberRoleId
            , string userId
            , ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(memberRoleId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't assign group role request! MemberRoleId parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't assign group role request! UserId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AssignRoleToMember(memberRoleId, userId, callback));
        }

        /// <summary>
        /// Assign a role to a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the assigned role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void AssignRoleToMemberV2(string memberRoleId
            , string userId, string groupId
            , ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(memberRoleId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't assign group role request! MemberRoleId parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't assign group role request! groupId parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't assign group role request! UserId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AssignRoleToMemberV2(memberRoleId, userId, groupId, callback));
        }

        /// <summary>
        /// Remove a role from a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the removed role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void RemoveRoleFromMember(string memberRoleId
            , string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(memberRoleId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't remove group role request! MemberRoleId parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't remove group role request! UserId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RemoveRoleFromMember(
                    memberRoleId,
                    userId,
                    callback));
        }

        /// <summary>
        /// Remove a role from a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the removed role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="groupId">The groupId the user belongs to</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void RemoveRoleFromMemberV2(string memberRoleId
            , string userId, string groupId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(memberRoleId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't remove group role request! MemberRoleId parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't remove group role request! groupId parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't remove group role request! UserId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RemoveRoleFromMemberV2(
                    memberRoleId,
                    userId,
                    groupId,
                    callback));
        }

        /// <summary>
        /// Get list of member role on the namespace.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedMemberRoles via callback when completed.</param>
        public void GetMemberRoles(int limit
            , int offset
            , ResultCallback<PaginatedMemberRoles> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetMemberRoles(callback, limit, offset));
        }

        /// <summary>
        /// Get list of member role on the namespace.
        /// </summary>
        /// <param name="callback">Returns a Result that contains PaginatedMemberRoles via callback when completed.</param>
        public void GetMemberRoles(ResultCallback<PaginatedMemberRoles> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            GetMemberRoles(0, 0, callback);
        }

        /// <summary>
        /// Get list of groups based on group ids
        /// </summary>
        /// <param name="groupIds">List of group ids</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupList via callback when completed</param>
        public void GetGroupsByGroupIds(string[] groupIds, ResultCallback<PaginatedGroupListResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if(groupIds == null || groupIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "GroupId can't null or empty!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetGroupsByGroupIds(groupIds, callback));
        }

        /// <summary>
        /// Get user joined groups information
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupMemberList via callback when completed</param>
        public void GetUserJoinedGroups(int limit, int offset, ResultCallback<PaginatedGroupMemberList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetUserJoinedGroups(callback, limit, offset));
        }

        /// <summary>
        /// Cancel group member invitation
        /// </summary>
        /// <param name="userId">The user ID who has been invited</param>
        /// <param name="groupId">the group ID of the user based on</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed</param>
        public void CancelGroupMemberInvitation(string userId, string groupId,
            ResultCallback<GroupGeneralResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't cancel group member invitation! groupId parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.CancelGroupMemberInvitation(userId, groupId, callback));
        }

        /// <summary>
        /// Update  group custom attributes
        /// </summary>
        /// <param name="groupId">the group ID of the user based on</param>
        /// <param name="customAttributes">Collection of custom attributes in a key-value fashion</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed</param>
        public void UpdateGroupCustomAttributesV2(string groupId, Dictionary<string, object> customAttributes,
            ResultCallback<GroupInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't update group custom attributes! groupId parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.UpdateGroupCustomAttributes(groupId, customAttributes, callback));
        }

        /// <summary>
        /// Get user group status information. This API will check the member and group information,
        /// and also the role permission.
        /// </summary>
        /// <param name="userId">The user ID of the group member</param>
        /// <param name="groupId">the group ID of the user based on</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed</param>
        public void GetUserGroupStatusInfo(string userId, string groupId,
            ResultCallback<GroupMemberInformation> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(groupId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't get user group status info! groupId parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetUserGroupStatusInfo(userId, groupId, callback));
        }

        /// <summary>
        /// Get My Join Request To The Groups
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupRequestList via callback when completed</param>
        public void GetMyJoinRequest(int limit, int offset, ResultCallback<PaginatedGroupRequestList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetMyJoinRequest(callback, limit, offset));
        }
    }
}