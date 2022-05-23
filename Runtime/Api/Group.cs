// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
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
        private readonly IUserSession session;

        internal Group( GroupApi inApi
            , IUserSession inSession
            , CoroutineRunner inCoroutineRunner )
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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
        internal Group( GroupApi inApi
            , IUserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Create new group. It will use the authorization to determine the user ID which will be used as the group admin.
        /// </summary>
        /// <param name="createGroupRequest">New group detail request.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void CreateGroup( CreateGroupRequest createGroupRequest
            , ResultCallback<GroupInformation> callback )
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
        /// Get list of groups. It will only show OPEN and PUBLIC group type.
        /// </summary>
        /// <param name="groupName">The group name query, leave it blank will fetch all the group list. (optional)</param>
        /// <param name="groupRegion">The region you want to search, leave it blank will fetch group from all existing region. (optional)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedGroupListResponse via callback when completed.</param>
        public void SearchGroups( string groupName
            , string groupRegion
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupListResponse> callback )
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
        public void SearchGroups( string groupName
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupListResponse> callback )
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
        public void SearchGroups( string groupName
            , string groupRegion
            , ResultCallback<PaginatedGroupListResponse> callback )
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
        public void SearchGroups( string groupName
            , ResultCallback<PaginatedGroupListResponse> callback )
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
        public void SearchGroups( int limit
            , int offset
            , ResultCallback<PaginatedGroupListResponse> callback )
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
        public void SearchGroups( ResultCallback<PaginatedGroupListResponse> callback)
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
            , ResultCallback<GroupInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group information! GroupId parameter is null!");

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
        public void UpdateGroup( string groupId
            , UpdateGroupRequest updateGroupRequest
            , ResultCallback<GroupInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group information! GroupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroup(groupId, updateGroupRequest, callback));
        }

        /// <summary>
        /// Update specific single group custom attributes information.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="customAttributes">The new custom attributes information.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomAttributes( string groupId
            , Dictionary<string, object> customAttributes
            , ResultCallback<GroupInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group information! GroupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            UpdateGroupRequest request = new UpdateGroupRequest
            {
                customAttributes = customAttributes
            };

            coroutineRunner.Run(
                api.UpdateGroup(groupId, request, callback));
        }

        /// <summary>
        /// Delete specific single group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void DeleteGroup( string groupId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't delete group! GroupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGroup(groupId, callback));
        }

        /// <summary>
        /// Update group custom rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="ruleUpdateRequest">The new custom rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed.</param>
        public void UpdateGroupCustomRule( string groupId
            , Dictionary<string, object> ruleUpdateRequest
            , ResultCallback<GroupInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group custom rule! GroupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupCustomRule(groupId, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Update predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="ruleUpdateRequest">The new predefined rule for the group.</param>
        /// <param name="callback">Returns a Result that contains GroupInformation via callback when completed</param>
        public void UpdateGroupPredefinedRule( string groupId
            , AllowedAction allowedAction
            , UpdateGroupPredefinedRuleRequest ruleUpdateRequest
            , ResultCallback<GroupInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group predefined rule! GroupId parameter is null!");
            Assert.AreNotEqual(AllowedAction.None, allowedAction, "Can't update group predefined rule! allowedAction parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGroupPredefinedRule(groupId, allowedAction, ruleUpdateRequest, callback));
        }

        /// <summary>
        /// Delete predefined group rule.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="allowedAction">The rule action of the group.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGroupPredefinedRule( string groupId
            , AllowedAction allowedAction
            ,  ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't update group predefined rule! GroupId parameter is null!");
            Assert.AreNotEqual(AllowedAction.None, allowedAction, "Can't update group predefined rule! allowedAction parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGroupPredefinedRule(groupId, allowedAction, callback));
        }

        /// <summary>
        /// Get user's group information.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void GetMyGroupInfo( ResultCallback<GroupMemberInformation> callback )
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
        public void GetOtherGroupInfo( string userId
            , ResultCallback<GroupMemberInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);

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
        public void AcceptGroupInvitation( string groupId
            , ResultCallback<GroupGeneralResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't accept group invitation! groupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptGroupInvitation(groupId, callback));
        }

        /// <summary>
        /// Rejecting group invitation.
        /// </summary>
        /// <param name="groupId">The group id you want to reject.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectGroupInvitation( string groupId
            , ResultCallback<GroupGeneralResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't accept group invitation! groupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RejectGroupInvitation(groupId, callback));
        }

        /// <summary>
        /// Invite the other user to your group.
        /// </summary>
        /// <param name="otherUserId">The other user id who will be invited into specific group.</param>
        /// <param name="callback">Returns a Result that contains UserInvitationResponse via callback when completed.</param>
        public void InviteOtherUserToGroup( string otherUserId
            , ResultCallback<UserInvitationResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't invite other user to group! OtherUserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.InviteOtherUserToGroup(otherUserId, callback));
        }

        /// <summary>
        /// Join into specific group.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void JoinGroup( string groupId
            , ResultCallback<JoinGroupResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't join group! GroupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.JoinGroup(groupId, callback));
        }

        /// <summary>
        /// Cancel the Join group request.
        /// </summary>
        /// <param name="groupId">The expected group id.</param>
        /// <param name="callback">Returns a Result that contains JoinGroupResponse via callback when completed.</param>
        public void CancelJoinGroupRequest( string groupId
            , ResultCallback<GroupGeneralResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't cancel join group request! GroupId parameter is null!");

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
        public void GetGroupMemberList( string groupId
            , ResultCallback<PaginatedGroupMemberList> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group member list! GroupId parameter is null!");

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
        public void GetGroupMemberList( string groupId
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupMemberList> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group member list! GroupId parameter is null!");

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
        public void KickGroupMember( string otherUserId
            , ResultCallback<KickMemberResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't kick a group member! OtherUserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.KickGroupMember(otherUserId, callback));
        }

        /// <summary>
        /// Leave the group you're in.
        /// </summary>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void LeaveGroup( ResultCallback<GroupGeneralResponse> callback )
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
        /// Get list of join request in group.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupJoinRequests( string groupId
            , int limit
            , int offset
            , ResultCallback<PaginatedGroupRequestList> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group join requests! GroupId parameter is null!");

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
        public void GetGroupJoinRequests( string groupId
            , ResultCallback<PaginatedGroupRequestList> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(groupId, "Can't get group join requests! GroupId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGroupJoinRequests(groupId, callback));
        }

        /// <summary>
        /// Get list of group invitation request.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains GroupRequestResponse via callback when completed.</param>
        public void GetGroupInvitationRequests( int limit
            , int offset
            , ResultCallback<PaginatedGroupRequestList> callback )
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
        public void GetGroupInvitationRequests( ResultCallback<PaginatedGroupRequestList> callback )
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
        /// Accept other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void AcceptOtherJoinRequest( string otherUserId
            , ResultCallback<GroupGeneralResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't accept other user join request! UserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptOtherJoinRequest(otherUserId, callback));
        }

        /// <summary>
        /// Reject other user join group request.
        /// </summary>
        /// <param name="otherUserId">The id of the other user who request to join group.</param>
        /// <param name="callback">Returns a Result that contains GroupGeneralResponse via callback when completed.</param>
        public void RejectOtherJoinRequest( string otherUserId
            , ResultCallback<GroupGeneralResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(otherUserId, "Can't reject other user join request! OtherUserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RejectOtherJoinRequest(otherUserId, callback));
        }

        /// <summary>
        /// Assign a role to a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the assigned role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="callback">Returns a Result that contains GroupMemberInformation via callback when completed.</param>
        public void AssignRoleToMember( string memberRoleId
            , string userId
            , ResultCallback<GroupMemberInformation> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(memberRoleId, "Can't assign group role request! MemberRoleId parameter is null!");
            Assert.IsNotNull(userId, "Can't assign group role request! UserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AssignRoleToMember(memberRoleId, userId, callback));
        }

        /// <summary>
        /// Remove a role from a group member
        /// </summary>
        /// <param name="memberRoleId">The roleId of the removed role</param>
        /// <param name="userId">The userId of the group member</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void RemoveRoleFromMember( string memberRoleId
            , string userId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(memberRoleId, "Can't remove group role request! MemberRoleId parameter is null!");
            Assert.IsNotNull(userId, "Can't remove group role request! UserId parameter is null!");

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
        /// Get list of member role on the namespace.
        /// </summary>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="callback">Returns a Result that contains PaginatedMemberRoles via callback when completed.</param>
        public void GetMemberRoles( int limit
            , int offset
            , ResultCallback<PaginatedMemberRoles> callback )
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
        public void GetMemberRoles( ResultCallback<PaginatedMemberRoles> callback )
        {
            GetMemberRoles(0, 0, callback);
        }
    }
}
