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
                api.CreateGroup(createGroupRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupCreate);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.CreateGroupV2(createGroupRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupCreate);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.SearchGroups(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGroupFindPredefinedEvent(session.UserId, groupName, groupRegion);
                    }
                    HandleCallback(cb, callback);
                }, groupName, groupRegion, limit, offset));
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
                api.SearchGroups(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGroupFindPredefinedEvent(session.UserId, groupName, "");
                    }
                    HandleCallback(cb, callback);
                }, groupName, "", limit, offset));
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
                api.SearchGroups(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGroupFindPredefinedEvent(session.UserId, groupName, groupRegion);
                    }
                    HandleCallback(cb, callback);
                }, groupName, groupRegion));
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
                api.SearchGroups(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGroupFindPredefinedEvent(session.UserId, groupName, "");
                    }
                    HandleCallback(cb, callback);
                }, groupName));
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
                api.SearchGroups(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGroupFindPredefinedEvent(session.UserId, "", "");
                    }
                    HandleCallback(cb, callback);
                }, "", "", limit, offset));
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
                api.SearchGroups(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGroupFindPredefinedEvent(session.UserId, "", "");
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.GetGroup(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupGetInformation);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroup(groupId, updateGroupRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdate);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroupV2(groupId, updateGroupRequest, cb => 
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdate);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroup(groupId, request, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdateCustomAttribute);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.DeleteGroup(groupId, cb =>
                {
                    if (cb != null && !cb.IsError)
                    {
                        SendPredefinedEvent(groupId, EventMode.GroupDelete);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.DeleteGroupV2(groupId, cb =>
                {
                    if (cb != null && !cb.IsError)
                    {
                        SendPredefinedEvent(groupId, EventMode.GroupDelete);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroupCustomRule(groupId, ruleUpdateRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdateCustomRule);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroupCustomRuleV2(groupId, ruleUpdateRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdateCustomRule);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroupPredefinedRule(groupId, allowedAction, ruleUpdateRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdatePredefinedRule);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.UpdateGroupPredefinedRuleV2(groupId, allowedAction, ruleUpdateRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupUpdatePredefinedRule);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.DeleteGroupPredefinedRule(groupId, allowedAction, cb =>
                {
                    if (cb != null && !cb.IsError)
                    {
                        SendGroupPredefinedRuleDeletedPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.DeleteGroupPredefinedRuleV2(groupId, allowedAction, cb =>
                {
                    if (cb != null && !cb.IsError)
                    {
                        SendGroupPredefinedRuleDeletedPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.AcceptGroupInvitation(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupInviteAccept);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.AcceptGroupInvitationV2(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupInviteAccept);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.RejectGroupInvitation(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupInviteReject);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.RejectGroupInvitationV2(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupInviteReject);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.InviteOtherUserToGroup(otherUserId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupInviteUser);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.InviteOtherUserToGroupV2(otherUserId, groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupInviteUser);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.JoinGroup(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupJoin);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.JoinGroupV2(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupJoin);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.CancelJoinGroupRequest(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendCancelJoinRequestPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.GetGroupMemberList(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetGroupMemberPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.GetGroupMemberList(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetGroupMemberPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }, limit, offset));
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
                api.KickGroupMember(otherUserId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupMemberKick);
                    }
                    HandleCallback(cb, callback);
                }));
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

            if (string.IsNullOrEmpty(groupId))
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
                api.KickGroupMemberV2(otherUserId, groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupMemberKick);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.LeaveGroup(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupLeft);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.LeaveGroupV2(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupLeft);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.GetGroupJoinRequests(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetJoinRequestsPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }, limit, offset));
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
                api.GetGroupJoinRequests(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetJoinRequestsPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.GetGroupJoinRequestsV2(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetJoinRequestsPredefinedEvent(groupId, session.UserId);
                    }
                    HandleCallback(cb, callback);
                }, limit, offset));
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
                api.GetGroupInvitationRequests(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetInvitationListPredefinedEvent(session.UserId);
                    }
                    HandleCallback(cb, callback);
                }, limit, offset));
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
                api.GetGroupInvitationRequests(cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetInvitationListPredefinedEvent(session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.GetGroupInvitationRequestsV2(groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendGetInvitationListPredefinedEvent(session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.AcceptOtherJoinRequest(otherUserId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupRequestAccept);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.AcceptOtherJoinRequestV2(otherUserId, groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupRequestAccept);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.RejectOtherJoinRequest(otherUserId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupRequestReject);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.RejectOtherJoinRequestV2(otherUserId, groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.GroupRequestReject);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.AssignRoleToMember(memberRoleId, userId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendMemberRoleUpdatedPredefinedEvent(userId, memberRoleId, cb.Value.groupId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                api.AssignRoleToMemberV2(memberRoleId, userId, groupId, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendMemberRoleUpdatedPredefinedEvent(userId, memberRoleId, cb.Value.groupId);
                    }
                    HandleCallback(cb, callback);
                }));
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
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendMemberRoleDeletedPredefinedEvent(userId, memberRoleId);
                        }
                        HandleCallback(cb, callback);
                    }));
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
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendMemberRoleDeletedPredefinedEvent(userId, memberRoleId, groupId);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (groupIds == null || groupIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "GroupId can't null or empty!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetGroupsByGroupIds(groupIds, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendGroupFindByIdsPredefinedEvent(session.UserId, groupIds);
                }
                HandleCallback(cb, callback);
            }));
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

            coroutineRunner.Run(api.CancelGroupMemberInvitation(userId, groupId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb.Value, EventMode.GroupInviteCancel);
                }
                HandleCallback(cb, callback);
            }));
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

            coroutineRunner.Run(api.UpdateGroupCustomAttributes(groupId, customAttributes, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb.Value, EventMode.GroupUpdateCustomAttribute);
                }
                HandleCallback(cb, callback);
            }));
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

        #region PredefinedEvents
        private enum EventMode
        {
            GroupCreate,
            GroupUpdate,
            GroupJoin,
            GroupDelete,
            GroupLeft,
            GroupInviteAccept,
            GroupInviteReject,
            GroupInviteCancel,
            GroupRequestAccept,
            GroupRequestReject,
            GroupMemberKick,
            GroupMemberRoleUpdate,
            GroupMemberRoleDelete,
            GroupUpdateCustomAttribute,
            GroupUpdateCustomRule,
            GroupUpdatePredefinedRule,
            GroupDeletePredefinedRule,
            GroupGetInformation,
            GroupFind,
            GroupFindByIds,
            GroupInviteUser,
            GroupGetInvitationList,
            GroupJoinRequestCancel,
            GroupJoinRequestGet,
            GroupGetGroupMember
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(T result, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (eventMode)
            {
                case EventMode.GroupCreate:
                    var createGroupInformation = result as GroupInformation;
                    payload = new PredefinedGroupCreatedPayload(createGroupInformation);
                    break;

                case EventMode.GroupUpdate:
                    var updateGroupInformation = result as GroupInformation;
                    payload = new PredefinedGroupUpdatedPayload(updateGroupInformation);
                    break;

                case EventMode.GroupJoin:
                    var joinGroupResponse = result as JoinGroupResponse;
                    payload = new PredefinedGroupJoinedPayload(joinGroupResponse);
                    break;

                case EventMode.GroupDelete:
                    var groupId = result as string;
                    payload = new PredefinedGroupDeletedPayload(groupId, localUserId);
                    break;

                case EventMode.GroupLeft:
                    var leftGroupResponse = result as GroupGeneralResponse;
                    payload = new PredefinedGroupLeftPayload(leftGroupResponse);
                    break;

                case EventMode.GroupInviteAccept:
                    var inviteAcceptResponse = result as GroupGeneralResponse;
                    payload = new PredefinedGroupInviteAcceptedPayload(inviteAcceptResponse);
                    break;

                case EventMode.GroupInviteReject:
                    var inviteRejectResponse = result as GroupGeneralResponse;
                    payload = new PredefinedGroupInviteRejectedPayload(inviteRejectResponse);
                    break;

                case EventMode.GroupInviteCancel:
                    var inviteCanceledResponse = result as GroupGeneralResponse;
                    payload = new PredefinedGroupInviteCanceledPayload(localUserId, inviteCanceledResponse);
                    break;

                case EventMode.GroupRequestAccept:
                    var requestAcceptResponse = result as GroupGeneralResponse;
                    payload = new PredefinedGroupJoinRequestAcceptedPayload(localUserId, requestAcceptResponse);
                    break;

                case EventMode.GroupRequestReject:
                    var requestRejectResponse = result as GroupGeneralResponse;
                    payload = new PredefinedGroupJoinRequestRejectedPayload(localUserId, requestRejectResponse);
                    break;

                case EventMode.GroupMemberKick:
                    var memberKickResponse = result as KickMemberResponse;
                    payload = new PredefinedGroupMemberKickedPayload(localUserId, memberKickResponse);
                    break;

                case EventMode.GroupUpdateCustomAttribute:
                    var updateCustomAttributeResponse = result as GroupInformation;
                    payload = new PredefinedGroupCustomAttributesUpdatedPayload(updateCustomAttributeResponse, localUserId);
                    break;

                case EventMode.GroupUpdateCustomRule:
                    var updateCustomRuleResponse = result as GroupInformation;
                    payload = new PredefinedGroupCustomRuleUpdatedPayload(updateCustomRuleResponse, localUserId);
                    break;

                case EventMode.GroupUpdatePredefinedRule:
                    var updatePredefinedRuleResponse = result as GroupInformation;
                    payload = new PredefinedGroupPredefinedRuleUpdatedPayload(updatePredefinedRuleResponse, localUserId);
                    break;

                case EventMode.GroupGetInformation:
                    var getInformationResponse = result as GroupInformation;
                    payload = new PredefinedGroupGetInformationPayload(getInformationResponse, localUserId);
                    break;

                case EventMode.GroupInviteUser:
                    var inviteUserResponse = result as UserInvitationResponse;
                    payload = new PredefinedGroupInviteUserPayload(inviteUserResponse, localUserId);
                    break;
            }

            return payload;
        }

        private void SendGroupPredefinedRuleDeletedPredefinedEvent(string groupId, string userId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupPredefinedRuleDeletedPayload(groupId, userId);
            SendPredefinedEvent(payload);
        }

        private void SendGroupFindPredefinedEvent(string userId, string groupName, string groupRegion)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupFindPayload(userId, groupName, groupRegion);
            SendPredefinedEvent(payload);
        }

        private void SendGroupFindByIdsPredefinedEvent(string userId, string[] groupIds)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupFindByIdsPayload(userId, groupIds);
            SendPredefinedEvent(payload);
        }

        private void SendGetInvitationListPredefinedEvent(string userId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupGetInvitationListPayload(userId);
            SendPredefinedEvent(payload);
        }

        private void SendCancelJoinRequestPredefinedEvent(string groupId, string userId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupCancelJoinRequestPayload(groupId, userId);
            SendPredefinedEvent(payload);
        }

        private void SendGetJoinRequestsPredefinedEvent(string groupId, string userId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupGetJoinRequestPayload(groupId, userId);
            SendPredefinedEvent(payload);
        }

        private void SendGetGroupMemberPredefinedEvent(string groupId, string userId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupGetGroupMemberPayload(groupId, userId);
            SendPredefinedEvent(payload);
        }

        private void SendMemberRoleDeletedPredefinedEvent(string userId, string roleIdToRemove, string groupId = null)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupMemberRoleDeletedPayload(userId, roleIdToRemove, groupId);
            SendPredefinedEvent(payload);
        }

        private void SendMemberRoleUpdatedPredefinedEvent(string userId, string roleIdToAdd, string groupId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedGroupMemberRoleUpdatedPayload(userId, roleIdToAdd, groupId);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent<T>(T result, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = CreatePayload(result, eventMode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            if (payload == null)
            {
                return;
            }

            AccelByteTelemetryEvent groupEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(groupEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        #endregion
    }
}