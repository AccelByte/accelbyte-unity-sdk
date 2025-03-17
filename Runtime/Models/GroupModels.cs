// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum GroupType
    {
        NONE,
        OPEN,
        PUBLIC,
        PRIVATE
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum RuleCriteria
    {
        MINIMUM,
        MAXIMUM,
        EQUAL
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum JoinGroupStatus
    {
        JOINING,
        JOINED,
        REQUESTED
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum RequestType
    {
        INVITE,
        JOIN,
        JOINED
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum MemberStatus
    {
        NONE,
        INVITE,
        JOIN,
        JOINED
    }

    [DataContract, Preserve]
    public class RuleInformation
    {
        [DataMember] public string ruleAttribute;
        [DataMember] public RuleCriteria ruleCriteria;
        [DataMember] public float ruleValue;
    }

    [DataContract, Preserve]
    public class Rules
    {
        [DataMember] public AllowedAction allowedAction;
        [DataMember] public RuleInformation[] ruleDetail;
    }

    [DataContract, Preserve]
    public class GroupRules
    {
        [DataMember] public object groupCustomRule;
        [DataMember] public Rules[] groupPredefinedRules;
    }

    [DataContract, Preserve]
    public class CreateGroupRequest
    {
        [DataMember] public string configurationCode;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string groupDescription;
        [DataMember] public string groupIcon;
        [DataMember] public int groupMaxMember;
        [DataMember] public string groupName;
        [DataMember] public string groupRegion;
        [DataMember] public GroupRules groupRules;
        [DataMember] public GroupType groupType;
    }

    [DataContract, Preserve]
    public class GroupMember
    {
        [DataMember] public string userId;
        [DataMember] public string[] memberRoleId;
    }

    [DataContract, Preserve]
    public class GroupMemberInformation
    {
        [DataMember] public string userId;
        [DataMember] public string[] memberRoleId;
        [DataMember] public string groupId;
        [DataMember] public MemberStatus status;
    }

    [DataContract, Preserve]
    public class GroupInformation
    {
        [DataMember] public string configurationCode;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string groupDescription;
        [DataMember] public string groupIcon;
        [DataMember] public string groupId;
        [DataMember] public int groupMaxMember;
        [DataMember] public GroupMember[] groupMembers;
        [DataMember] public string groupName;
        [DataMember] public string groupRegion;
        [DataMember] public GroupRules groupRules;
        [DataMember] public GroupType groupType;
    }

    [DataContract, Preserve]
    public class PaginatedGroupListResponse
    {
        [DataMember] public GroupInformation[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UpdateGroupRequest
    {
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string groupDescription;
        [DataMember] public string groupIcon;
        [DataMember] public string groupName;
        [DataMember] public string groupRegion;
        [DataMember] public GroupType groupType;
    }

    [DataContract, Preserve]
    public class UpdateGroupNoTypeRequest
    {
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string groupDescription;
        [DataMember] public string groupIcon;
        [DataMember] public string groupName;
        [DataMember] public string groupRegion;
    }

    [DataContract, Preserve]
    public class UserInvitationResponse
    {
        [DataMember] public string userId;
        [DataMember] public string groupId;
    }

    [DataContract, Preserve]
    public class JoinGroupResponse
    {
        [DataMember] public string userId;
        [DataMember] public string groupId;
        [DataMember] public JoinGroupStatus status;
    }

    [DataContract, Preserve]
    public class KickMemberResponse
    {
        [DataMember] public string kickedUserId;
        [DataMember] public string groupId;
    }

    [DataContract, Preserve]
    public class RejectOtherJoinResponse
    {
        [DataMember] public string rejectedUserId;
        [DataMember] public string groupId;
    }

    [DataContract, Preserve]
    public class UpdateGroupCustomRuleRequest
    {
        [DataMember] public object groupCustomRule;
    }

    [DataContract, Preserve]
    public class UpdateGroupPredefinedRuleRequest
    {
        [DataMember] public List<RuleInformation> ruleDetail;
    }

    [DataContract, Preserve]
    public class GroupGeneralResponse
    {
        [DataMember] public string groupId;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class PaginatedGroupMemberList
    {
        [DataMember] public GroupMemberInformation[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class GroupRequest
    {
        [DataMember] public string userId;
        [DataMember] public string groupId;
        [DataMember] public RequestType requestType;
    }

    [DataContract, Preserve]
    public class PaginatedGroupRequestList
    {
        [DataMember] public GroupRequest[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class AssignRoleRequest
    {
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class MemberRolePermission
    {
        [DataMember] public int action;
        [DataMember] public string resourceName;
    }

    [DataContract, Preserve]
    public class MemberRole
    {
        [DataMember] public string memberRoleId;
        [DataMember] public string memberRoleName;
        [DataMember] public MemberRolePermission[] memberRolePermissions;
    }

    [DataContract, Preserve]
    public class PaginatedMemberRoles
    {
        [DataMember] public MemberRole[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class GetGroupsByGroupIdsRequest
    {
        [DataMember] public string[] groupIDs;
    }

    [DataContract, Preserve]
    public class UpdateCustomAttributesRequest
    {
        [DataMember] public Dictionary<string,object> customAttributes;
    }

}
