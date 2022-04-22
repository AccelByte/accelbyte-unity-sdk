// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    public enum GroupType
    {
        NONE,
        OPEN,
        PUBLIC,
        PRIVATE
    }

    public enum RuleCriteria
    {
        MINIMUM,
        MAXIMUM,
        EQUAL
    }

    public enum JoinGroupStatus
    {
        JOINING,
        JOINED,
        REQUESTED
    }

    public enum AllowedAction
    {
        None,
        createGroup,
        joinGroup,
        inviteUser,
        kickUser,
        leaveGroup,
        responseJoinRequest
    }

    public enum RequestType
    {
        INVITE,
        JOIN,
        JOINED
    }

    public enum MemberStatus
    {
        NONE,
        INVITE,
        JOIN,
        JOINED
    }

    [DataContract]
    public class RuleInformation
    {
        [DataMember] public string ruleAttribute { get; set; }
        [DataMember] public RuleCriteria ruleCriteria { get; set; }
        [DataMember] public float ruleValue { get; set; }
    }

    [DataContract]
    public class Rules
    {
        [DataMember] public AllowedAction allowedAction { get; set; }
        [DataMember] public RuleInformation[] ruleDetail { get; set; }
    }

    [DataContract]
    public class GroupRules
    {
        [DataMember] public object groupCustomRule { get; set; }
        [DataMember] public Rules[] groupPredefinedRules { get; set; }
    }

    [DataContract]
    public class CreateGroupRequest
    {
        [DataMember] public string configurationCode { get; set; }
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
        [DataMember] public string groupDescription { get; set; }
        [DataMember] public string groupIcon { get; set; }
        [DataMember] public int groupMaxMember { get; set; } 
        [DataMember] public string groupName { get; set; }
        [DataMember] public string groupRegion { get; set; }
        [DataMember] public GroupRules groupRules { get; set; }
        [DataMember] public GroupType groupType { get; set; }
    }

    [DataContract]
    public class GroupMember
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string []memberRoleId { get; set; }
    }

    [DataContract]
    public class GroupMemberInformation
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string[] memberRoleId { get; set; }
        [DataMember] public string groupId { get; set; }
        [DataMember] public MemberStatus status { get; set; }
    }

    [DataContract]
    public class GroupInformation
    {
        [DataMember] public string configurationCode { get; set; }
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
        [DataMember] public string groupDescription { get; set; }
        [DataMember] public string groupIcon { get; set; }
        [DataMember] public string groupId { get; set; }
        [DataMember] public int groupMaxMember { get; set; }
        [DataMember] public GroupMember[] groupMembers { get; set; }
        [DataMember] public string groupName { get; set; }
        [DataMember] public string groupRegion { get; set; }
        [DataMember] public GroupRules groupRules { get; set; }
        [DataMember] public GroupType groupType { get; set; }
    }

    [DataContract]
    public class PaginatedGroupListResponse
    {
        [DataMember] public GroupInformation[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UpdateGroupRequest
    {
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
        [DataMember] public string groupDescription { get; set; }
        [DataMember] public string groupIcon { get; set; }
        [DataMember] public string groupName { get; set; }
        [DataMember] public string groupRegion { get; set; }
        [DataMember] public GroupType groupType { get; set; }
    }

    [DataContract]
    public class UpdateGroupNoTypeRequest
    {
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
        [DataMember] public string groupDescription { get; set; }
        [DataMember] public string groupIcon { get; set; }
        [DataMember] public string groupName { get; set; }
        [DataMember] public string groupRegion { get; set; }
    }

    [DataContract]
    public class UserInvitationResponse
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string groupId { get; set; }
    }

    [DataContract]
    public class JoinGroupResponse
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string groupId { get; set; }
        [DataMember] public JoinGroupStatus status { get; set; }
    }

    [DataContract]
    public class KickMemberResponse
    {
        [DataMember] public string kickedUserId { get; set; }
        [DataMember] public string groupId { get; set; }
    }

    [DataContract]
    public class RejectOtherJoinResponse
    {
        [DataMember] public string rejectedUserId { get; set; }
        [DataMember] public string groupId { get; set; }
    }

    [DataContract]
    public class UpdateGroupCustomRuleRequest
    {
        [DataMember] public object groupCustomRule { get; set; }
    }

    [DataContract]
    public class UpdateGroupPredefinedRuleRequest
    {
        [DataMember] public List<RuleInformation> ruleDetail { get; set; }
    }

    [DataContract]
    public class GroupGeneralResponse
    {
        [DataMember] public string groupId { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class PaginatedGroupMemberList
    {
        [DataMember] public GroupMemberInformation[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class GroupRequest
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string groupId { get; set; }
        [DataMember] public RequestType requestType { get; set; }
    }

    [DataContract]
    public class PaginatedGroupRequestList
    {
        [DataMember] public GroupRequest[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class AssignRoleRequest
    {
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class MemberRolePermission
    {
        [DataMember] public int action { get; set; }
        [DataMember] public string resourceName { get; set; }
    }

    [DataContract]
    public class MemberRole
    {
        [DataMember] public string memberRoleId { get; set; }
        [DataMember] public string memberRoleName { get; set; }
        [DataMember] public MemberRolePermission[] memberRolePermissions { get; set; }
    }

    [DataContract]
    public class PaginatedMemberRoles
    {
        [DataMember] public MemberRole[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }
}
