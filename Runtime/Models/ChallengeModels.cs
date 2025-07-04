// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine.Scripting;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ChallengeSortBy
    {
        [Description("updatedAt:asc"), EnumMember(Value = "updatedAt:asc")]
        UpdatedAtAsc,
        [Description("updatedAt:desc"), EnumMember(Value = "updatedAt:desc")]
        UpdatedAtDesc,
        [Description("createdAt:asc"), EnumMember(Value = "createdAt:asc")]
        CreatedAtAsc,
        [Description("createdAt:desc"), EnumMember(Value = "createdAt:desc")]
        CreatedAtDesc,
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ChallengeStatus
    {
        None,
        [Description("INIT"), EnumMember(Value = "INIT")]
        Init,
        [Description("RETIRED"), EnumMember(Value = "RETIRED")]
        Retired,
        [Description("TIED"), EnumMember(Value = "TIED")]
        Tied
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ChallengeRewardStatus
    {
        None,
        [Description("CLAIMED"), EnumMember(Value = "CLAIMED")]
        Claimed,
        [Description("UNCLAIMED"), EnumMember(Value = "UNCLAIMED")]
        Unclaimed
    }

    [DataContract, Preserve]
    public class ChallengeMeta
    {
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "endDate")] public string EndDate;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "startDate")] public string StartDate;
    }

    [DataContract, Preserve]
    public class ChallengePeriodMeta
    {
        [DataMember(Name = "endTime")] public string EndTime;
        [DataMember(Name = "startTime")] public string StartTime;
    }

    [DataContract, Preserve]
    public class ChallengeMetaResponse : ChallengeMeta
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "period")] public ChallengePeriodMeta Period;
    }

    [DataContract, Preserve]
    public class ChallengeBase : ChallengeMeta
    {
        [DataMember(Name = "activeGoalsPerRotation")] public int ActiveGoalsPerRotation;
        [DataMember(Name = "assignmentRule")] public string AssignmentRule;
        [DataMember(Name = "endAfter")] public int EndAfter;
        [DataMember(Name = "goalsVisibility")] public string GoalsVisibility;
        [DataMember(Name = "rotation")] public string Rotation;
        [DataMember(Name = "resetConfig")] public ChallengeResetConfig ResetConfig;
        [DataMember(Name = "repeatAfter")] public int RepeatAfter;
        [DataMember(Name = "randomizedPerRotation")] public bool RandomizedPerRotation;
        [DataMember(Name = "tags")] public string[] Tags;
    }

    [DataContract, Preserve]
    public class ChallengeResponseInfo : ChallengeBase
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "status")] public string Status;
        [DataMember(Name = "updatedAt")] public string UpdatedAt;
        [DataMember(Name = "createdAt")] public string CreatedAt;
    }
    
    [DataContract, Preserve]
    public class RandomizedChallengeResponse
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "challengeCode")] public string ChallengeCode;
        [DataMember(Name = "startTime")] public string StartTime;
        [DataMember(Name = "endTime")] public string EndTime;        
        [DataMember(Name = "updatedAt")] public string UpdatedAt;
        [DataMember(Name = "createdAt")] public string CreatedAt;
        [DataMember(Name = "goals")] public GoalResponseInfo[] Goals;
    }

    [DataContract, Preserve]
    public class ChallengeResetConfig
    {
        [DataMember(Name = "resetDate")] public int ResetDate;
        [DataMember(Name = "resetDay")] public int ResetDay;
        [DataMember(Name = "resetTime")] public string ResetTime;
    }

    [DataContract, Preserve]
    public class CreateChallengeRequest : ChallengeBase
    {
        [DataMember(Name = "code")] public string Code;
    }

    [DataContract, Preserve]
    public class UpdateChallengeRequest : ChallengeBase
    {
    }

    [DataContract, Preserve]
    public class GetChallengesOptionalParamenters : OptionalParametersBase
    {
        /// <summary>
        /// Filter challenges by code
        /// </summary>
        public string Keyword;
        /// <summary>
        /// Filter challenges by tags.
        /// </summary>
        public string[] Tags;
        /// <summary>
        /// Maximum number of items to retrieve.
        /// </summary>
        public int? Limit;
        /// <summary>
        /// Number of item to skip.
        /// </summary>
        public int? Offset;
        /// <summary>
        /// Sort list by attributes.
        /// </summary>
        public ChallengeSortBy? SortBy;
        /// <summary>
        /// Challenge status
        /// </summary>
        public ChallengeStatus? Status;
    }

    [DataContract, Preserve]
    public class ChallengeResponse
    {
        [DataMember(Name = "data")] public ChallengeResponseInfo[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class GoalResponseBase
    {
        [DataMember(Name = "challengeCode")] public string ChallengeCode;
    }

    [DataContract, Preserve]
    public class GoalResponseInfo : GoalResponseBase
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "createdAt")] public string CreatedAt;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "isActive")] public bool IsActive;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "requirementGroups")] public ChallengeRequirement[] RequirementGroups;
        [DataMember(Name = "rewards")] public ChallengeReward[] Rewards;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "updatedAt")] public string UpdatedAt;
    }

    [DataContract, Preserve]
    public class ChallengeGoalMeta
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "requirementGroups")] public ChallengeRequirement[] RequirementGroups;
        [DataMember(Name = "rewards")] public ChallengeReward[] Rewards;
        [DataMember(Name = "tags")] public string[] Tags;
    }

    [DataContract, Preserve]
    public class GoalProgressionInfo : GoalResponseBase
    {
        [DataMember(Name = "goal")] public ChallengeGoalMeta Goal;
        [DataMember(Name = "goalCode")] public string GoalCode;
        [DataMember(Name = "goalProgressionId")] public string GoalProgressionId;
        [DataMember(Name = "requirementProgressions")] public ChallengeRequirementProgressionResponse[] RequirementProgressions;
        [DataMember(Name = "status")] public string Status;
        [DataMember(Name = "toClaimRewards")] public ChallengeClaimableUserReward[] ToClaimRewards;
    }

    [DataContract, Preserve]
    public class GoalResponse
    {
        [DataMember(Name = "data")] public GoalResponseInfo[] Data;
        [DataMember(Name = "meta")] public ChallengeResponseInfo Meta;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class ChallengeGoalRequestBase
    {
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "isActive")] public bool IsActive;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "requirementGroups")] public ChallengeRequirement[] RequirementGroups;
        [DataMember(Name = "rewards")] public ChallengeReward[] Rewards;
        [DataMember(Name = "schedule")] public ChallengeGoalSchedule Schedule;
        [DataMember(Name = "tags")] public string[] Tags;
    }

    [DataContract, Preserve]
    public class CreateChallengeGoalRequest : ChallengeGoalRequestBase
    {
        [DataMember(Name = "code")]public string Code;
    }
    [DataContract, Preserve]
    public class UpdateChallengeGoalRequest : ChallengeGoalRequestBase
    {
    }

    [DataContract, Preserve]
    public class ChallengePeriod
    {
        [DataMember(Name = "startTime")] public string StartTime;
        [DataMember(Name = "endTime")] public string EndTime;
        [DataMember(Name = "slot")] public int Slot;
    }

    [DataContract, Preserve]
    public class ChallengePeriodResponse
    {
        [DataMember(Name = "data")] public ChallengePeriod[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class ChallengeGoalSchedule
    {
        [DataMember(Name = "endTime")] public string EndTime;
        [DataMember(Name = "order")] public int Order;
        [DataMember(Name = "startTime")] public string StartTime;
    }

    [DataContract, Preserve]
    public class ChallengeReward
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemName")] public string ItemName;
        [DataMember(Name = "qty")] public float Quantity;
        [DataMember(Name = "type")] public string Type;
    }

    [DataContract, Preserve]
    public class ChallengeClaimableUserReward : ChallengeReward
    {
        [DataMember(Name = "goalProgressionId")] public string GoalProgressionId;
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "status")] public ChallengeRewardStatus Status;
    }

    [DataContract, Preserve]
    public class GoalProgressionResponse
    {
        [DataMember(Name = "data")] public GoalProgressionInfo[] Data;
        [DataMember(Name = "meta")] public ChallengeMetaResponse Meta;
        [DataMember(Name = "paging")] internal GoalProgressionPaging Paging;
    }
    
    [DataContract, Preserve]
    public class GoalProgressionPaging
    {
        [DataMember(Name = "first")] public string First;
        [DataMember(Name = "last")] public string Last;
        [DataMember(Name = "next")] public string Next;
        [DataMember(Name = "previous")] public string Previous;
    }

    [DataContract, Preserve]
    public class ChallengeRequirementProgressionResponse
    {
        [DataMember(Name = "completedAt")] public string CompletedAt;
        [DataMember(Name = "currentValue")] public double CurrentValue;
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "targetValue")] public double TargetValue;
        [DataMember(Name = "matcher")] public string Matcher;
        [DataMember(Name = "parameterName")] public string ParameterName;
        [DataMember(Name = "parameterType")] public string ParameterType;
        [DataMember(Name = "statCycleId")] public string StatCycleId;
    }

    [DataContract, Preserve]
    public class ChallengeRequirement
    {
        [DataMember(Name = "operator")] public string Operator;
        [DataMember(Name = "predicates")] public ChallengePredicate[] Predicates;
    }

    [DataContract, Preserve]
    public class ChallengePredicate
    {
        [DataMember(Name = "matcher")] public string Matcher;
        [DataMember(Name = "parameterName")] public string ParameterName;
        [DataMember(Name = "parameterType")] public string ParameterType;
        [DataMember(Name = "targetValue")] public double TargetValue;
        [DataMember(Name = "statCycleId")] public string StatCycleId;
    }

    [DataContract, Preserve]
    public class UserRewards
    {
        [DataMember(Name = "data")] public UserReward[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class UserReward
    {
        [DataMember(Name = "challengeCode")] public string ChallengeCode;
        [DataMember(Name = "createdAt")] public string CreatedAt;
        [DataMember(Name = "goalCode")] public string GoalCode;
        [DataMember(Name = "goalProgressionId")] public string GoalProgressionId;
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemName")] public string ItemName;
        [DataMember(Name = "qty")] public float Quantity;
        [DataMember(Name = "status")] public string Status;
        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "updatedAt")] public string UpdatedAt;
        [DataMember(Name = "userId")] public string UserId;
    }

    [DataContract, Preserve]
    public class ClaimRewardRequest
    {
        [DataMember(Name = "rewardIDs")] public string[] RewardIDs;
    }

    [DataContract, Preserve]
    public class ChallengeBulkClaimRewardRequest : ClaimRewardRequest
    {
        [DataMember(Name = "userId")] public string UserId;
    }

    [DataContract, Preserve]
    public class ChallengeBulkClaimRewardResponse
    {
        [DataMember(Name = "errorDetail")] public ChallengeBulkClaimError ErrorDetail;
        [DataMember(Name = "isSuccess")] public bool IsSuccess;
        [DataMember(Name = "rewards")] public UserReward[] Rewards;
        [DataMember(Name = "userId")] public string UserId;
    }

    public class ChallengeBulkClaimError
    {
        [DataMember(Name = "attributes")] public Dictionary<string, string> Attributes;
        [DataMember(Name = "errorCode")] public int ErrorCode;
        [DataMember(Name = "errorMessage")] public string ErrorMessage;
        [DataMember(Name = "message")] public string Message;
        [DataMember(Name = "name")] public string Name;
    }
    
    [DataContract, Preserve]
    public class ChallengeEvaluatePlayerProgressionRequest
    {
        [DataMember(Name = "userIds")] public string[] UserIds;
    }
    
    [DataContract, Preserve]
    public class ChallengeScheduleByGoalResponse
    {
        [DataMember(Name = "endTime")] public DateTime EndTime;
        [DataMember(Name = "startTime")] public DateTime StartTime;
    }
    
    [DataContract, Preserve]
    public class ChallengeListScheduleByGoalOptionalParameters : OptionalParametersBase
    {
        public int? Limit;
        public int? Offset;
    }
    
    [DataContract, Preserve]
    public class ChallengeListScheduleByGoalResponse
    {
        [DataMember(Name = "data")] public ChallengeScheduleByGoalResponse[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class ChallengeGoalInSchedulesResponse
    {
        [DataMember(Name = "challengeCode")] public string ChallengeCode;
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "isActive")] public bool IsActive;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "requirementGroups")] public ChallengeRequirement[] RequirementGroups;
        [DataMember(Name = "rewards")] public ChallengeReward[] Rewards;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
    }

    [DataContract, Preserve]
    public class ChallengeScheduleResponse
    {
        [DataMember(Name = "endTime")] public DateTime EndTime;
        [DataMember(Name = "goals")] public ChallengeGoalInSchedulesResponse[] Goals;
        [DataMember(Name = "startTime")] public DateTime StartTime;
    }

    [DataContract, Preserve]
    public class ChallengeListSchedulesOptionalParameters : OptionalParametersBase
    {
        public DateTime? DateTime;
        public int? Limit;
        public int? Offset;
    }
    
    [DataContract, Preserve]
    public class ChallengeListSchedulesResponse
    {
        [DataMember(Name = "data")] public ChallengeScheduleResponse[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [Preserve]
    public class GetScheduledChallengeGoalsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter returned scheduled challenge goals based on their tags.
        /// </summary>
        public string[] Tags = null;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class GetChallengeProgressOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class GetRewardsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Sort returned entries by provided method.
        /// </summary>
        public ChallengeSortBy? SortBy = ChallengeSortBy.UpdatedAtDesc;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class ClaimRewardOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetChallengeProgressWithRotationIndexOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter results returned via their goal code.
        /// </summary>
        public string GoalCode = string.Empty;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class EvaluateChallengeProgressOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class BulkClaimRewardOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class CreateChallengeOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class CreateChallengeGoalOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class DeleteChallengeOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class DeleteChallengeGoalOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class DeleteTiedChallengeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetChallengeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetChallengeGoalOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class GetChallengeGoalsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Sort returned entries by provided method.
        /// </summary>
        public ChallengeSortBy? SortBy = ChallengeSortBy.UpdatedAtDesc;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class GetChallengePeriodsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class GetUserRewardsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter returned entries by their reward status.
        /// </summary>
        public ChallengeRewardStatus? RewardStatus = ChallengeRewardStatus.None;

        /// <summary>
        /// Sort returned entries by provided method.
        /// </summary>
        public ChallengeSortBy? SortBy = ChallengeSortBy.UpdatedAtDesc;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class RandomizeChallengeGoalsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpdateChallengeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpdateChallengeGoalOptionalParameters : OptionalParametersBase
    {

    }
}