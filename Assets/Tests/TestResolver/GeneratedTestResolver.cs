#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace AccelByte.Models.Resolvers
{
    using System;
    using Utf8Json;

    public class GeneratedTestResolver : global::Utf8Json.IJsonFormatterResolver
    {
        public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new GeneratedTestResolver();

        GeneratedTestResolver()
        {

        }

        public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedTestResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::Utf8Json.IJsonFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedTestResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedTestResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(112)
            {
                {typeof(Permission[]), 0 },
                {typeof(global::Tests.TestHelper.RoleV4Response[]), 1 },
                {typeof(global::Tests.TestHelper.RedeemableItem[]), 2 },
                {typeof(global::Tests.TestHelper.CampaignInfo[]), 3 },
                {typeof(global::Tests.TestHelper.CodeInfo[]), 4 },
                {typeof(global::Tests.TestHelper.FlexingRule[]), 5 },
                {typeof(global::Tests.TestHelper.MatchingRule[]), 6 },
                {typeof(global::System.Collections.Generic.Dictionary<string, string>), 7 },
                {typeof(RegionDataItem[]), 8 },
                {typeof(Image[]), 9 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.ItemCreateModel.Localization>), 10 },
                {typeof(global::System.Collections.Generic.Dictionary<string, RegionDataItem[]>), 11 },
                {typeof(AchievementIcon[]), 12 },
                {typeof(global::Tests.TestHelper.ReportingAdminReasonItem[]), 13 },
                {typeof(Rules[]), 14 },
                {typeof(global::Tests.TestHelper.CreateGroupConfigResponse[]), 15 },
                {typeof(MemberRolePermission[]), 16 },
                {typeof(global::Tests.TestHelper.AgreementPolicyObject[]), 17 },
                {typeof(global::Tests.TestHelper.AgreementPolicyVersion[]), 18 },
                {typeof(global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest[]), 19 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>), 20 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.PodConfig>), 21 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentWithOverride>), 22 },
                {typeof(global::System.Collections.Generic.Dictionary<string, int>), 23 },
                {typeof(global::AccelByte.Models.FlexingRule[]), 24 },
                {typeof(global::AccelByte.Models.MatchingRule[]), 25 },
                {typeof(global::System.Collections.Generic.Dictionary<string, object>), 26 },
                {typeof(global::Tests.TestHelper.RoleV4Response), 27 },
                {typeof(global::Tests.TestHelper.ListRoleV4Response), 28 },
                {typeof(global::Tests.TestHelper.AssignUserV4Request), 29 },
                {typeof(global::Tests.TestHelper.AssignUserV4Response), 30 },
                {typeof(global::Tests.TestHelper.RedeemableItem), 31 },
                {typeof(global::Tests.TestHelper.CampaignCreateModel), 32 },
                {typeof(global::Tests.TestHelper.CampaignInfo), 33 },
                {typeof(global::Tests.TestHelper.CampaignPagingSlicedResult), 34 },
                {typeof(global::Tests.TestHelper.CampaignUpdateModel), 35 },
                {typeof(global::Tests.TestHelper.CampaignCodeCreateModel), 36 },
                {typeof(global::Tests.TestHelper.CampaignCodeCreateResult), 37 },
                {typeof(global::Tests.TestHelper.CodeInfo), 38 },
                {typeof(global::Tests.TestHelper.CodeInfoPagingSlicedResult), 39 },
                {typeof(global::Tests.TestHelper.AllianceRule), 40 },
                {typeof(global::Tests.TestHelper.FlexingRule), 41 },
                {typeof(global::Tests.TestHelper.MatchingRule), 42 },
                {typeof(global::Tests.TestHelper.RuleSet), 43 },
                {typeof(global::Tests.TestHelper.CreateChannelRequest), 44 },
                {typeof(global::Tests.TestHelper.CurrencyCreateModel), 45 },
                {typeof(global::Tests.TestHelper.CurrencyInfoModel), 46 },
                {typeof(global::Tests.TestHelper.CurrencySummaryModel), 47 },
                {typeof(global::Tests.TestHelper.StoreCreateModel), 48 },
                {typeof(global::Tests.TestHelper.CategoryCreateModel), 49 },
                {typeof(global::Tests.TestHelper.LocalExt), 50 },
                {typeof(global::Tests.TestHelper.RegionDataUS), 51 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.Localization), 52 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.Recurring), 53 },
                {typeof(global::Tests.TestHelper.ItemCreateModel), 54 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.RegionDatas), 55 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.Localizations), 56 },
                {typeof(global::Tests.TestHelper.StoreInfoModel), 57 },
                {typeof(global::Tests.TestHelper.FullCategoryInfo), 58 },
                {typeof(global::Tests.TestHelper.FullItemInfo), 59 },
                {typeof(global::Tests.TestHelper.CreditRequestModel), 60 },
                {typeof(global::Tests.TestHelper.FulfillmentModel), 61 },
                {typeof(global::Tests.TestHelper.UserMapResponse), 62 },
                {typeof(global::Tests.TestHelper.StatCreateModel), 63 },
                {typeof(global::Tests.TestHelper.UserVerificationCode), 64 },
                {typeof(global::Tests.TestHelper.AchievementRequest), 65 },
                {typeof(global::Tests.TestHelper.AchievementResponse), 66 },
                {typeof(global::Tests.TestHelper.ReportingAdminReasonItem), 67 },
                {typeof(global::Tests.TestHelper.ReportingAdminReasonsResponse), 68 },
                {typeof(global::Tests.TestHelper.ReportingAddReasonRequest), 69 },
                {typeof(global::Tests.TestHelper.ReportingAddReasonGroupRequest), 70 },
                {typeof(global::Tests.TestHelper.LeaderboardDailyConfig), 71 },
                {typeof(global::Tests.TestHelper.LeaderboardMonthlyConfig), 72 },
                {typeof(global::Tests.TestHelper.LeaderboardWeeklyConfig), 73 },
                {typeof(global::Tests.TestHelper.LeaderboardConfigRequest), 74 },
                {typeof(global::Tests.TestHelper.LeaderboardConfigResponse), 75 },
                {typeof(global::Tests.TestHelper.CreateGroupConfigResponse), 76 },
                {typeof(global::Tests.TestHelper.PaginatedGroupConfig), 77 },
                {typeof(global::Tests.TestHelper.CreateMemberRoleRequest), 78 },
                {typeof(global::Tests.TestHelper.CreateMemberRoleResponse), 79 },
                {typeof(global::Tests.TestHelper.AgreementBasePolicyCreate), 80 },
                {typeof(global::Tests.TestHelper.AgreementPolicyTypeObject), 81 },
                {typeof(global::Tests.TestHelper.AgreementPolicyObject), 82 },
                {typeof(global::Tests.TestHelper.AgreementBasePolicy), 83 },
                {typeof(global::Tests.TestHelper.AgreementPolicyVersionCreate), 84 },
                {typeof(global::Tests.TestHelper.AgreementPolicyVersion), 85 },
                {typeof(global::Tests.TestHelper.AgreementCountryPolicy), 86 },
                {typeof(global::Tests.TestHelper.AgreementLocalizedPolicyCreate), 87 },
                {typeof(global::Tests.TestHelper.AgreementLocalizedPolicy), 88 },
                {typeof(global::Tests.TestHelper.AdminCreateProfanityListRequest), 89 },
                {typeof(global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest), 90 },
                {typeof(global::Tests.TestHelper.AdminAddProfanityFiltersRequest), 91 },
                {typeof(global::Tests.TestHelper.AdminSetProfanityRuleForNamespaceRequest), 92 },
                {typeof(global::Tests.TestHelper.FreeSubscritptionRequest), 93 },
                {typeof(global::Tests.TestHelper.PodConfig), 94 },
                {typeof(global::Tests.TestHelper.DeploymentConfig), 95 },
                {typeof(global::Tests.TestHelper.DeploymentWithOverride), 96 },
                {typeof(global::Tests.TestHelper.DSMConfig), 97 },
                {typeof(global::AccelByte.Models.StatCreateRequest), 98 },
                {typeof(global::AccelByte.Models.StatCode), 99 },
                {typeof(global::AccelByte.Models.LeaderboardDailyConfig), 100 },
                {typeof(global::AccelByte.Models.LeaderboardMonthlyConfig), 101 },
                {typeof(global::AccelByte.Models.LeaderboardWeeklyConfig), 102 },
                {typeof(global::AccelByte.Models.LeaderboardConfig), 103 },
                {typeof(global::AccelByte.Models.SetUserVisibilityRequest), 104 },
                {typeof(global::AccelByte.Models.AllianceRule), 105 },
                {typeof(global::AccelByte.Models.FlexingRule), 106 },
                {typeof(global::AccelByte.Models.MatchingRule), 107 },
                {typeof(global::AccelByte.Models.RuleSet), 108 },
                {typeof(global::AccelByte.Models.CreateChannelRequest), 109 },
                {typeof(global::AccelByte.Models.CreateChannelResponse), 110 },
                {typeof(global::Tests.IntegrationTests.MatchmakingRequest), 111 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.ArrayFormatter<Permission>();
                case 1: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.RoleV4Response>();
                case 2: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.RedeemableItem>();
                case 3: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.CampaignInfo>();
                case 4: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.CodeInfo>();
                case 5: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.FlexingRule>();
                case 6: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.MatchingRule>();
                case 7: return new global::Utf8Json.Formatters.DictionaryFormatter<string, string>();
                case 8: return new global::Utf8Json.Formatters.ArrayFormatter<RegionDataItem>();
                case 9: return new global::Utf8Json.Formatters.ArrayFormatter<Image>();
                case 10: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Tests.TestHelper.ItemCreateModel.Localization>();
                case 11: return new global::Utf8Json.Formatters.DictionaryFormatter<string, RegionDataItem[]>();
                case 12: return new global::Utf8Json.Formatters.ArrayFormatter<AchievementIcon>();
                case 13: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.ReportingAdminReasonItem>();
                case 14: return new global::Utf8Json.Formatters.ArrayFormatter<Rules>();
                case 15: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.CreateGroupConfigResponse>();
                case 16: return new global::Utf8Json.Formatters.ArrayFormatter<MemberRolePermission>();
                case 17: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.AgreementPolicyObject>();
                case 18: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.AgreementPolicyVersion>();
                case 19: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest>();
                case 20: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Tests.TestHelper.DeploymentConfig>();
                case 21: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Tests.TestHelper.PodConfig>();
                case 22: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Tests.TestHelper.DeploymentWithOverride>();
                case 23: return new global::Utf8Json.Formatters.DictionaryFormatter<string, int>();
                case 24: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.FlexingRule>();
                case 25: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.MatchingRule>();
                case 26: return new global::Utf8Json.Formatters.DictionaryFormatter<string, object>();
                case 27: return new AccelByte.Models.Formatters.Tests.TestHelper_RoleV4ResponseFormatter();
                case 28: return new AccelByte.Models.Formatters.Tests.TestHelper_ListRoleV4ResponseFormatter();
                case 29: return new AccelByte.Models.Formatters.Tests.TestHelper_AssignUserV4RequestFormatter();
                case 30: return new AccelByte.Models.Formatters.Tests.TestHelper_AssignUserV4ResponseFormatter();
                case 31: return new AccelByte.Models.Formatters.Tests.TestHelper_RedeemableItemFormatter();
                case 32: return new AccelByte.Models.Formatters.Tests.TestHelper_CampaignCreateModelFormatter();
                case 33: return new AccelByte.Models.Formatters.Tests.TestHelper_CampaignInfoFormatter();
                case 34: return new AccelByte.Models.Formatters.Tests.TestHelper_CampaignPagingSlicedResultFormatter();
                case 35: return new AccelByte.Models.Formatters.Tests.TestHelper_CampaignUpdateModelFormatter();
                case 36: return new AccelByte.Models.Formatters.Tests.TestHelper_CampaignCodeCreateModelFormatter();
                case 37: return new AccelByte.Models.Formatters.Tests.TestHelper_CampaignCodeCreateResultFormatter();
                case 38: return new AccelByte.Models.Formatters.Tests.TestHelper_CodeInfoFormatter();
                case 39: return new AccelByte.Models.Formatters.Tests.TestHelper_CodeInfoPagingSlicedResultFormatter();
                case 40: return new AccelByte.Models.Formatters.Tests.TestHelper_AllianceRuleFormatter();
                case 41: return new AccelByte.Models.Formatters.Tests.TestHelper_FlexingRuleFormatter();
                case 42: return new AccelByte.Models.Formatters.Tests.TestHelper_MatchingRuleFormatter();
                case 43: return new AccelByte.Models.Formatters.Tests.TestHelper_RuleSetFormatter();
                case 44: return new AccelByte.Models.Formatters.Tests.TestHelper_CreateChannelRequestFormatter();
                case 45: return new AccelByte.Models.Formatters.Tests.TestHelper_CurrencyCreateModelFormatter();
                case 46: return new AccelByte.Models.Formatters.Tests.TestHelper_CurrencyInfoModelFormatter();
                case 47: return new AccelByte.Models.Formatters.Tests.TestHelper_CurrencySummaryModelFormatter();
                case 48: return new AccelByte.Models.Formatters.Tests.TestHelper_StoreCreateModelFormatter();
                case 49: return new AccelByte.Models.Formatters.Tests.TestHelper_CategoryCreateModelFormatter();
                case 50: return new AccelByte.Models.Formatters.Tests.TestHelper_LocalExtFormatter();
                case 51: return new AccelByte.Models.Formatters.Tests.TestHelper_RegionDataUSFormatter();
                case 52: return new AccelByte.Models.Formatters.Tests.TestHelper_ItemCreateModel_LocalizationFormatter();
                case 53: return new AccelByte.Models.Formatters.Tests.TestHelper_ItemCreateModel_RecurringFormatter();
                case 54: return new AccelByte.Models.Formatters.Tests.TestHelper_ItemCreateModelFormatter();
                case 55: return new AccelByte.Models.Formatters.Tests.TestHelper_ItemCreateModel_RegionDatasFormatter();
                case 56: return new AccelByte.Models.Formatters.Tests.TestHelper_ItemCreateModel_LocalizationsFormatter();
                case 57: return new AccelByte.Models.Formatters.Tests.TestHelper_StoreInfoModelFormatter();
                case 58: return new AccelByte.Models.Formatters.Tests.TestHelper_FullCategoryInfoFormatter();
                case 59: return new AccelByte.Models.Formatters.Tests.TestHelper_FullItemInfoFormatter();
                case 60: return new AccelByte.Models.Formatters.Tests.TestHelper_CreditRequestModelFormatter();
                case 61: return new AccelByte.Models.Formatters.Tests.TestHelper_FulfillmentModelFormatter();
                case 62: return new AccelByte.Models.Formatters.Tests.TestHelper_UserMapResponseFormatter();
                case 63: return new AccelByte.Models.Formatters.Tests.TestHelper_StatCreateModelFormatter();
                case 64: return new AccelByte.Models.Formatters.Tests.TestHelper_UserVerificationCodeFormatter();
                case 65: return new AccelByte.Models.Formatters.Tests.TestHelper_AchievementRequestFormatter();
                case 66: return new AccelByte.Models.Formatters.Tests.TestHelper_AchievementResponseFormatter();
                case 67: return new AccelByte.Models.Formatters.Tests.TestHelper_ReportingAdminReasonItemFormatter();
                case 68: return new AccelByte.Models.Formatters.Tests.TestHelper_ReportingAdminReasonsResponseFormatter();
                case 69: return new AccelByte.Models.Formatters.Tests.TestHelper_ReportingAddReasonRequestFormatter();
                case 70: return new AccelByte.Models.Formatters.Tests.TestHelper_ReportingAddReasonGroupRequestFormatter();
                case 71: return new AccelByte.Models.Formatters.Tests.TestHelper_LeaderboardDailyConfigFormatter();
                case 72: return new AccelByte.Models.Formatters.Tests.TestHelper_LeaderboardMonthlyConfigFormatter();
                case 73: return new AccelByte.Models.Formatters.Tests.TestHelper_LeaderboardWeeklyConfigFormatter();
                case 74: return new AccelByte.Models.Formatters.Tests.TestHelper_LeaderboardConfigRequestFormatter();
                case 75: return new AccelByte.Models.Formatters.Tests.TestHelper_LeaderboardConfigResponseFormatter();
                case 76: return new AccelByte.Models.Formatters.Tests.TestHelper_CreateGroupConfigResponseFormatter();
                case 77: return new AccelByte.Models.Formatters.Tests.TestHelper_PaginatedGroupConfigFormatter();
                case 78: return new AccelByte.Models.Formatters.Tests.TestHelper_CreateMemberRoleRequestFormatter();
                case 79: return new AccelByte.Models.Formatters.Tests.TestHelper_CreateMemberRoleResponseFormatter();
                case 80: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementBasePolicyCreateFormatter();
                case 81: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementPolicyTypeObjectFormatter();
                case 82: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementPolicyObjectFormatter();
                case 83: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementBasePolicyFormatter();
                case 84: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementPolicyVersionCreateFormatter();
                case 85: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementPolicyVersionFormatter();
                case 86: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementCountryPolicyFormatter();
                case 87: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementLocalizedPolicyCreateFormatter();
                case 88: return new AccelByte.Models.Formatters.Tests.TestHelper_AgreementLocalizedPolicyFormatter();
                case 89: return new AccelByte.Models.Formatters.Tests.TestHelper_AdminCreateProfanityListRequestFormatter();
                case 90: return new AccelByte.Models.Formatters.Tests.TestHelper_AdminAddProfanityFilterIntoListRequestFormatter();
                case 91: return new AccelByte.Models.Formatters.Tests.TestHelper_AdminAddProfanityFiltersRequestFormatter();
                case 92: return new AccelByte.Models.Formatters.Tests.TestHelper_AdminSetProfanityRuleForNamespaceRequestFormatter();
                case 93: return new AccelByte.Models.Formatters.Tests.TestHelper_FreeSubscritptionRequestFormatter();
                case 94: return new AccelByte.Models.Formatters.Tests.TestHelper_PodConfigFormatter();
                case 95: return new AccelByte.Models.Formatters.Tests.TestHelper_DeploymentConfigFormatter();
                case 96: return new AccelByte.Models.Formatters.Tests.TestHelper_DeploymentWithOverrideFormatter();
                case 97: return new AccelByte.Models.Formatters.Tests.TestHelper_DSMConfigFormatter();
                case 98: return new AccelByte.Models.Formatters.AccelByte.Models.StatCreateRequestFormatter();
                case 99: return new AccelByte.Models.Formatters.AccelByte.Models.StatCodeFormatter();
                case 100: return new AccelByte.Models.Formatters.AccelByte.Models.LeaderboardDailyConfigFormatter();
                case 101: return new AccelByte.Models.Formatters.AccelByte.Models.LeaderboardMonthlyConfigFormatter();
                case 102: return new AccelByte.Models.Formatters.AccelByte.Models.LeaderboardWeeklyConfigFormatter();
                case 103: return new AccelByte.Models.Formatters.AccelByte.Models.LeaderboardConfigFormatter();
                case 104: return new AccelByte.Models.Formatters.AccelByte.Models.SetUserVisibilityRequestFormatter();
                case 105: return new AccelByte.Models.Formatters.AccelByte.Models.AllianceRuleFormatter();
                case 106: return new AccelByte.Models.Formatters.AccelByte.Models.FlexingRuleFormatter();
                case 107: return new AccelByte.Models.Formatters.AccelByte.Models.MatchingRuleFormatter();
                case 108: return new AccelByte.Models.Formatters.AccelByte.Models.RuleSetFormatter();
                case 109: return new AccelByte.Models.Formatters.AccelByte.Models.CreateChannelRequestFormatter();
                case 110: return new AccelByte.Models.Formatters.AccelByte.Models.CreateChannelResponseFormatter();
                case 111: return new AccelByte.Models.Formatters.Tests.IntegrationTests.MatchmakingRequestFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace AccelByte.Models.Formatters.Tests
{
    using System;
    using Utf8Json;


    public sealed class TestHelper_RoleV4ResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.RoleV4Response>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_RoleV4ResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("adminRole"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isWildcard"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("permissions"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("roleId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("roleName"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("adminRole"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isWildcard"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("roleId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("roleName"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.RoleV4Response value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.adminRole);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteBoolean(value.isWildcard);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<Permission[]>().Serialize(ref writer, value.permissions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.roleId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.roleName);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.RoleV4Response Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __adminRole__ = default(bool);
            var __adminRole__b__ = false;
            var __isWildcard__ = default(bool);
            var __isWildcard__b__ = false;
            var __permissions__ = default(Permission[]);
            var __permissions__b__ = false;
            var __roleId__ = default(string);
            var __roleId__b__ = false;
            var __roleName__ = default(string);
            var __roleName__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __adminRole__ = reader.ReadBoolean();
                        __adminRole__b__ = true;
                        break;
                    case 1:
                        __isWildcard__ = reader.ReadBoolean();
                        __isWildcard__b__ = true;
                        break;
                    case 2:
                        __permissions__ = formatterResolver.GetFormatterWithVerify<Permission[]>().Deserialize(ref reader, formatterResolver);
                        __permissions__b__ = true;
                        break;
                    case 3:
                        __roleId__ = reader.ReadString();
                        __roleId__b__ = true;
                        break;
                    case 4:
                        __roleName__ = reader.ReadString();
                        __roleName__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.RoleV4Response();
            if(__adminRole__b__) ____result.adminRole = __adminRole__;
            if(__isWildcard__b__) ____result.isWildcard = __isWildcard__;
            if(__permissions__b__) ____result.permissions = __permissions__;
            if(__roleId__b__) ____result.roleId = __roleId__;
            if(__roleName__b__) ____result.roleName = __roleName__;

            return ____result;
        }
    }


    public sealed class TestHelper_ListRoleV4ResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ListRoleV4Response>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ListRoleV4ResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paging"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("data"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paging"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ListRoleV4Response value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RoleV4Response[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ListRoleV4Response Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::Tests.TestHelper.RoleV4Response[]);
            var __data__b__ = false;
            var __paging__ = default(Paging);
            var __paging__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __data__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RoleV4Response[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ListRoleV4Response();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class TestHelper_AssignUserV4RequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AssignUserV4Request>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AssignUserV4RequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("assignedNamespaces"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("assignedNamespaces"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AssignUserV4Request value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.assignedNamespaces, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AssignUserV4Request Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __assignedNamespaces__ = default(string[]);
            var __assignedNamespaces__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __assignedNamespaces__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __assignedNamespaces__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AssignUserV4Request();
            if(__assignedNamespaces__b__) ____result.assignedNamespaces = __assignedNamespaces__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class TestHelper_AssignUserV4ResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AssignUserV4Response>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AssignUserV4ResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("assignedNamespaces"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("roleId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("assignedNamespaces"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("email"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("roleId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AssignUserV4Response value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.assignedNamespaces, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.email);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.roleId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AssignUserV4Response Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __assignedNamespaces__ = default(string[]);
            var __assignedNamespaces__b__ = false;
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __email__ = default(string);
            var __email__b__ = false;
            var __roleId__ = default(string);
            var __roleId__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __assignedNamespaces__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __assignedNamespaces__b__ = true;
                        break;
                    case 1:
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 2:
                        __email__ = reader.ReadString();
                        __email__b__ = true;
                        break;
                    case 3:
                        __roleId__ = reader.ReadString();
                        __roleId__b__ = true;
                        break;
                    case 4:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AssignUserV4Response();
            if(__assignedNamespaces__b__) ____result.assignedNamespaces = __assignedNamespaces__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__email__b__) ____result.email = __email__;
            if(__roleId__b__) ____result.roleId = __roleId__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class TestHelper_RedeemableItemFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.RedeemableItem>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_RedeemableItemFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.RedeemableItem value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.itemName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.quantity);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.RedeemableItem Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __itemName__ = default(string);
            var __itemName__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 1:
                        __itemName__ = reader.ReadString();
                        __itemName__b__ = true;
                        break;
                    case 2:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.RedeemableItem();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__itemName__b__) ____result.itemName = __itemName__;
            if(__quantity__b__) ____result.quantity = __quantity__;

            return ____result;
        }
    }


    public sealed class TestHelper_CampaignCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CampaignCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CampaignCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCode"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCodePerUser"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCampaignPerUser"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxSaleCount"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemStart"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemEnd"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemType"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("items"), 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCodePerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCampaignPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxSaleCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemStart"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemEnd"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("items"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CampaignCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.maxRedeemCountPerCode);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.maxRedeemCountPerCodePerUser);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxRedeemCountPerCampaignPerUser);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.maxSaleCount);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemStart, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemEnd, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.redeemType);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Serialize(ref writer, value.items, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CampaignCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __name__ = default(string);
            var __name__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __maxRedeemCountPerCode__ = default(int);
            var __maxRedeemCountPerCode__b__ = false;
            var __maxRedeemCountPerCodePerUser__ = default(int);
            var __maxRedeemCountPerCodePerUser__b__ = false;
            var __maxRedeemCountPerCampaignPerUser__ = default(int);
            var __maxRedeemCountPerCampaignPerUser__b__ = false;
            var __maxSaleCount__ = default(int);
            var __maxSaleCount__b__ = false;
            var __redeemStart__ = default(global::System.DateTime);
            var __redeemStart__b__ = false;
            var __redeemEnd__ = default(global::System.DateTime);
            var __redeemEnd__b__ = false;
            var __redeemType__ = default(string);
            var __redeemType__b__ = false;
            var __items__ = default(global::Tests.TestHelper.RedeemableItem[]);
            var __items__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 3:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 4:
                        __maxRedeemCountPerCode__ = reader.ReadInt32();
                        __maxRedeemCountPerCode__b__ = true;
                        break;
                    case 5:
                        __maxRedeemCountPerCodePerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCodePerUser__b__ = true;
                        break;
                    case 6:
                        __maxRedeemCountPerCampaignPerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCampaignPerUser__b__ = true;
                        break;
                    case 7:
                        __maxSaleCount__ = reader.ReadInt32();
                        __maxSaleCount__b__ = true;
                        break;
                    case 8:
                        __redeemStart__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemStart__b__ = true;
                        break;
                    case 9:
                        __redeemEnd__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemEnd__b__ = true;
                        break;
                    case 10:
                        __redeemType__ = reader.ReadString();
                        __redeemType__b__ = true;
                        break;
                    case 11:
                        __items__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Deserialize(ref reader, formatterResolver);
                        __items__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CampaignCreateModel();
            if(__name__b__) ____result.name = __name__;
            if(__description__b__) ____result.description = __description__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__status__b__) ____result.status = __status__;
            if(__maxRedeemCountPerCode__b__) ____result.maxRedeemCountPerCode = __maxRedeemCountPerCode__;
            if(__maxRedeemCountPerCodePerUser__b__) ____result.maxRedeemCountPerCodePerUser = __maxRedeemCountPerCodePerUser__;
            if(__maxRedeemCountPerCampaignPerUser__b__) ____result.maxRedeemCountPerCampaignPerUser = __maxRedeemCountPerCampaignPerUser__;
            if(__maxSaleCount__b__) ____result.maxSaleCount = __maxSaleCount__;
            if(__redeemStart__b__) ____result.redeemStart = __redeemStart__;
            if(__redeemEnd__b__) ____result.redeemEnd = __redeemEnd__;
            if(__redeemType__b__) ____result.redeemType = __redeemType__;
            if(__items__b__) ____result.items = __items__;

            return ____result;
        }
    }


    public sealed class TestHelper_CampaignInfoFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CampaignInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CampaignInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemStart"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemEnd"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCode"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCodePerUser"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCampaignPerUser"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxSaleCount"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemType"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("items"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("boothName"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 17},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemStart"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemEnd"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCodePerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCampaignPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxSaleCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("items"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("boothName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CampaignInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.type);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemStart, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemEnd, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteInt32(value.maxRedeemCountPerCode);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.maxRedeemCountPerCodePerUser);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteInt32(value.maxRedeemCountPerCampaignPerUser);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteInt32(value.maxSaleCount);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.redeemType);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Serialize(ref writer, value.items, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.boothName);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CampaignInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __type__ = default(string);
            var __type__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __redeemStart__ = default(global::System.DateTime);
            var __redeemStart__b__ = false;
            var __redeemEnd__ = default(global::System.DateTime);
            var __redeemEnd__b__ = false;
            var __maxRedeemCountPerCode__ = default(int);
            var __maxRedeemCountPerCode__b__ = false;
            var __maxRedeemCountPerCodePerUser__ = default(int);
            var __maxRedeemCountPerCodePerUser__b__ = false;
            var __maxRedeemCountPerCampaignPerUser__ = default(int);
            var __maxRedeemCountPerCampaignPerUser__b__ = false;
            var __maxSaleCount__ = default(int);
            var __maxSaleCount__b__ = false;
            var __redeemType__ = default(string);
            var __redeemType__b__ = false;
            var __items__ = default(global::Tests.TestHelper.RedeemableItem[]);
            var __items__b__ = false;
            var __boothName__ = default(string);
            var __boothName__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __type__ = reader.ReadString();
                        __type__b__ = true;
                        break;
                    case 2:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 3:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 4:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 5:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 6:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 7:
                        __redeemStart__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemStart__b__ = true;
                        break;
                    case 8:
                        __redeemEnd__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemEnd__b__ = true;
                        break;
                    case 9:
                        __maxRedeemCountPerCode__ = reader.ReadInt32();
                        __maxRedeemCountPerCode__b__ = true;
                        break;
                    case 10:
                        __maxRedeemCountPerCodePerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCodePerUser__b__ = true;
                        break;
                    case 11:
                        __maxRedeemCountPerCampaignPerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCampaignPerUser__b__ = true;
                        break;
                    case 12:
                        __maxSaleCount__ = reader.ReadInt32();
                        __maxSaleCount__b__ = true;
                        break;
                    case 13:
                        __redeemType__ = reader.ReadString();
                        __redeemType__b__ = true;
                        break;
                    case 14:
                        __items__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Deserialize(ref reader, formatterResolver);
                        __items__b__ = true;
                        break;
                    case 15:
                        __boothName__ = reader.ReadString();
                        __boothName__b__ = true;
                        break;
                    case 16:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 17:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CampaignInfo();
            if(__id__b__) ____result.id = __id__;
            if(__type__b__) ____result.type = __type__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__name__b__) ____result.name = __name__;
            if(__description__b__) ____result.description = __description__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__status__b__) ____result.status = __status__;
            if(__redeemStart__b__) ____result.redeemStart = __redeemStart__;
            if(__redeemEnd__b__) ____result.redeemEnd = __redeemEnd__;
            if(__maxRedeemCountPerCode__b__) ____result.maxRedeemCountPerCode = __maxRedeemCountPerCode__;
            if(__maxRedeemCountPerCodePerUser__b__) ____result.maxRedeemCountPerCodePerUser = __maxRedeemCountPerCodePerUser__;
            if(__maxRedeemCountPerCampaignPerUser__b__) ____result.maxRedeemCountPerCampaignPerUser = __maxRedeemCountPerCampaignPerUser__;
            if(__maxSaleCount__b__) ____result.maxSaleCount = __maxSaleCount__;
            if(__redeemType__b__) ____result.redeemType = __redeemType__;
            if(__items__b__) ____result.items = __items__;
            if(__boothName__b__) ____result.boothName = __boothName__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_CampaignPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CampaignPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CampaignPagingSlicedResultFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paging"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("data"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paging"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CampaignPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.CampaignInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CampaignPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::Tests.TestHelper.CampaignInfo[]);
            var __data__b__ = false;
            var __paging__ = default(Paging);
            var __paging__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __data__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.CampaignInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CampaignPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class TestHelper_CampaignUpdateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CampaignUpdateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CampaignUpdateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCode"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCodePerUser"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCampaignPerUser"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxSaleCount"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemStart"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemEnd"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemType"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("items"), 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCodePerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCampaignPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxSaleCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemStart"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemEnd"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("items"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CampaignUpdateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.maxRedeemCountPerCode);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.maxRedeemCountPerCodePerUser);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxRedeemCountPerCampaignPerUser);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.maxSaleCount);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemStart, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemEnd, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.redeemType);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Serialize(ref writer, value.items, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CampaignUpdateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __name__ = default(string);
            var __name__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __maxRedeemCountPerCode__ = default(int);
            var __maxRedeemCountPerCode__b__ = false;
            var __maxRedeemCountPerCodePerUser__ = default(int);
            var __maxRedeemCountPerCodePerUser__b__ = false;
            var __maxRedeemCountPerCampaignPerUser__ = default(int);
            var __maxRedeemCountPerCampaignPerUser__b__ = false;
            var __maxSaleCount__ = default(int);
            var __maxSaleCount__b__ = false;
            var __redeemStart__ = default(global::System.DateTime);
            var __redeemStart__b__ = false;
            var __redeemEnd__ = default(global::System.DateTime);
            var __redeemEnd__b__ = false;
            var __redeemType__ = default(string);
            var __redeemType__b__ = false;
            var __items__ = default(global::Tests.TestHelper.RedeemableItem[]);
            var __items__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 3:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 4:
                        __maxRedeemCountPerCode__ = reader.ReadInt32();
                        __maxRedeemCountPerCode__b__ = true;
                        break;
                    case 5:
                        __maxRedeemCountPerCodePerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCodePerUser__b__ = true;
                        break;
                    case 6:
                        __maxRedeemCountPerCampaignPerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCampaignPerUser__b__ = true;
                        break;
                    case 7:
                        __maxSaleCount__ = reader.ReadInt32();
                        __maxSaleCount__b__ = true;
                        break;
                    case 8:
                        __redeemStart__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemStart__b__ = true;
                        break;
                    case 9:
                        __redeemEnd__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemEnd__b__ = true;
                        break;
                    case 10:
                        __redeemType__ = reader.ReadString();
                        __redeemType__b__ = true;
                        break;
                    case 11:
                        __items__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Deserialize(ref reader, formatterResolver);
                        __items__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CampaignUpdateModel();
            if(__name__b__) ____result.name = __name__;
            if(__description__b__) ____result.description = __description__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__status__b__) ____result.status = __status__;
            if(__maxRedeemCountPerCode__b__) ____result.maxRedeemCountPerCode = __maxRedeemCountPerCode__;
            if(__maxRedeemCountPerCodePerUser__b__) ____result.maxRedeemCountPerCodePerUser = __maxRedeemCountPerCodePerUser__;
            if(__maxRedeemCountPerCampaignPerUser__b__) ____result.maxRedeemCountPerCampaignPerUser = __maxRedeemCountPerCampaignPerUser__;
            if(__maxSaleCount__b__) ____result.maxSaleCount = __maxSaleCount__;
            if(__redeemStart__b__) ____result.redeemStart = __redeemStart__;
            if(__redeemEnd__b__) ____result.redeemEnd = __redeemEnd__;
            if(__redeemType__b__) ____result.redeemType = __redeemType__;
            if(__items__b__) ____result.items = __items__;

            return ____result;
        }
    }


    public sealed class TestHelper_CampaignCodeCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CampaignCodeCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CampaignCodeCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("quantity"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CampaignCodeCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.quantity);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CampaignCodeCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __quantity__ = default(int);
            var __quantity__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CampaignCodeCreateModel();
            if(__quantity__b__) ____result.quantity = __quantity__;

            return ____result;
        }
    }


    public sealed class TestHelper_CampaignCodeCreateResultFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CampaignCodeCreateResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CampaignCodeCreateResultFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("numCreated"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("numCreated"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CampaignCodeCreateResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.numCreated);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CampaignCodeCreateResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __numCreated__ = default(int);
            var __numCreated__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __numCreated__ = reader.ReadInt32();
                        __numCreated__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CampaignCodeCreateResult();
            if(__numCreated__b__) ____result.numCreated = __numCreated__;

            return ____result;
        }
    }


    public sealed class TestHelper_CodeInfoFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CodeInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CodeInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("campaignId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCode"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCodePerUser"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxRedeemCountPerCampaignPerUser"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("remainder"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemedCount"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemStart"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemEnd"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("redeemType"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("items"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("batchNo"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("acquireOrderNo"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("acquireUserId"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 19},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("campaignId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCodePerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxRedeemCountPerCampaignPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("remainder"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemedCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemStart"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemEnd"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("redeemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("items"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("batchNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("acquireOrderNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("acquireUserId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CodeInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.type);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.campaignId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.value);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.maxRedeemCountPerCode);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxRedeemCountPerCodePerUser);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.maxRedeemCountPerCampaignPerUser);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.remainder);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteInt32(value.redeemedCount);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemStart, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.redeemEnd, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.redeemType);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Serialize(ref writer, value.items, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteInt32(value.batchNo);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.acquireOrderNo);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.acquireUserId);
            writer.WriteRaw(this.____stringByteKeys[18]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CodeInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __type__ = default(string);
            var __type__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __campaignId__ = default(string);
            var __campaignId__b__ = false;
            var __value__ = default(string);
            var __value__b__ = false;
            var __maxRedeemCountPerCode__ = default(int);
            var __maxRedeemCountPerCode__b__ = false;
            var __maxRedeemCountPerCodePerUser__ = default(int);
            var __maxRedeemCountPerCodePerUser__b__ = false;
            var __maxRedeemCountPerCampaignPerUser__ = default(int);
            var __maxRedeemCountPerCampaignPerUser__b__ = false;
            var __remainder__ = default(int);
            var __remainder__b__ = false;
            var __redeemedCount__ = default(int);
            var __redeemedCount__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __redeemStart__ = default(global::System.DateTime);
            var __redeemStart__b__ = false;
            var __redeemEnd__ = default(global::System.DateTime);
            var __redeemEnd__b__ = false;
            var __redeemType__ = default(string);
            var __redeemType__b__ = false;
            var __items__ = default(global::Tests.TestHelper.RedeemableItem[]);
            var __items__b__ = false;
            var __batchNo__ = default(int);
            var __batchNo__b__ = false;
            var __acquireOrderNo__ = default(string);
            var __acquireOrderNo__b__ = false;
            var __acquireUserId__ = default(string);
            var __acquireUserId__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __type__ = reader.ReadString();
                        __type__b__ = true;
                        break;
                    case 2:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 3:
                        __campaignId__ = reader.ReadString();
                        __campaignId__b__ = true;
                        break;
                    case 4:
                        __value__ = reader.ReadString();
                        __value__b__ = true;
                        break;
                    case 5:
                        __maxRedeemCountPerCode__ = reader.ReadInt32();
                        __maxRedeemCountPerCode__b__ = true;
                        break;
                    case 6:
                        __maxRedeemCountPerCodePerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCodePerUser__b__ = true;
                        break;
                    case 7:
                        __maxRedeemCountPerCampaignPerUser__ = reader.ReadInt32();
                        __maxRedeemCountPerCampaignPerUser__b__ = true;
                        break;
                    case 8:
                        __remainder__ = reader.ReadInt32();
                        __remainder__b__ = true;
                        break;
                    case 9:
                        __redeemedCount__ = reader.ReadInt32();
                        __redeemedCount__b__ = true;
                        break;
                    case 10:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 11:
                        __redeemStart__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemStart__b__ = true;
                        break;
                    case 12:
                        __redeemEnd__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __redeemEnd__b__ = true;
                        break;
                    case 13:
                        __redeemType__ = reader.ReadString();
                        __redeemType__b__ = true;
                        break;
                    case 14:
                        __items__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RedeemableItem[]>().Deserialize(ref reader, formatterResolver);
                        __items__b__ = true;
                        break;
                    case 15:
                        __batchNo__ = reader.ReadInt32();
                        __batchNo__b__ = true;
                        break;
                    case 16:
                        __acquireOrderNo__ = reader.ReadString();
                        __acquireOrderNo__b__ = true;
                        break;
                    case 17:
                        __acquireUserId__ = reader.ReadString();
                        __acquireUserId__b__ = true;
                        break;
                    case 18:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 19:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CodeInfo();
            if(__id__b__) ____result.id = __id__;
            if(__type__b__) ____result.type = __type__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__campaignId__b__) ____result.campaignId = __campaignId__;
            if(__value__b__) ____result.value = __value__;
            if(__maxRedeemCountPerCode__b__) ____result.maxRedeemCountPerCode = __maxRedeemCountPerCode__;
            if(__maxRedeemCountPerCodePerUser__b__) ____result.maxRedeemCountPerCodePerUser = __maxRedeemCountPerCodePerUser__;
            if(__maxRedeemCountPerCampaignPerUser__b__) ____result.maxRedeemCountPerCampaignPerUser = __maxRedeemCountPerCampaignPerUser__;
            if(__remainder__b__) ____result.remainder = __remainder__;
            if(__redeemedCount__b__) ____result.redeemedCount = __redeemedCount__;
            if(__status__b__) ____result.status = __status__;
            if(__redeemStart__b__) ____result.redeemStart = __redeemStart__;
            if(__redeemEnd__b__) ____result.redeemEnd = __redeemEnd__;
            if(__redeemType__b__) ____result.redeemType = __redeemType__;
            if(__items__b__) ____result.items = __items__;
            if(__batchNo__b__) ____result.batchNo = __batchNo__;
            if(__acquireOrderNo__b__) ____result.acquireOrderNo = __acquireOrderNo__;
            if(__acquireUserId__b__) ____result.acquireUserId = __acquireUserId__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_CodeInfoPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CodeInfoPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CodeInfoPagingSlicedResultFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paging"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("data"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paging"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CodeInfoPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.CodeInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CodeInfoPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::Tests.TestHelper.CodeInfo[]);
            var __data__b__ = false;
            var __paging__ = default(Paging);
            var __paging__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __data__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.CodeInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CodeInfoPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class TestHelper_AllianceRuleFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AllianceRule>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AllianceRuleFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("max_number"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("min_number"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("player_max_number"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("player_min_number"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("max_number"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("min_number"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("player_max_number"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("player_min_number"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AllianceRule value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.max_number);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.min_number);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.player_max_number);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.player_min_number);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AllianceRule Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __max_number__ = default(int);
            var __max_number__b__ = false;
            var __min_number__ = default(int);
            var __min_number__b__ = false;
            var __player_max_number__ = default(int);
            var __player_max_number__b__ = false;
            var __player_min_number__ = default(int);
            var __player_min_number__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __max_number__ = reader.ReadInt32();
                        __max_number__b__ = true;
                        break;
                    case 1:
                        __min_number__ = reader.ReadInt32();
                        __min_number__b__ = true;
                        break;
                    case 2:
                        __player_max_number__ = reader.ReadInt32();
                        __player_max_number__b__ = true;
                        break;
                    case 3:
                        __player_min_number__ = reader.ReadInt32();
                        __player_min_number__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AllianceRule();
            if(__max_number__b__) ____result.max_number = __max_number__;
            if(__min_number__b__) ____result.min_number = __min_number__;
            if(__player_max_number__b__) ____result.player_max_number = __player_max_number__;
            if(__player_min_number__b__) ____result.player_min_number = __player_min_number__;

            return ____result;
        }
    }


    public sealed class TestHelper_FlexingRuleFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FlexingRule>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FlexingRuleFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attribute"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("criteria"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("duration"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reference"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("attribute"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("criteria"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("duration"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reference"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FlexingRule value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.attribute);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.criteria);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.duration);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.reference);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FlexingRule Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __attribute__ = default(string);
            var __attribute__b__ = false;
            var __criteria__ = default(string);
            var __criteria__b__ = false;
            var __duration__ = default(int);
            var __duration__b__ = false;
            var __reference__ = default(int);
            var __reference__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __attribute__ = reader.ReadString();
                        __attribute__b__ = true;
                        break;
                    case 1:
                        __criteria__ = reader.ReadString();
                        __criteria__b__ = true;
                        break;
                    case 2:
                        __duration__ = reader.ReadInt32();
                        __duration__b__ = true;
                        break;
                    case 3:
                        __reference__ = reader.ReadInt32();
                        __reference__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FlexingRule();
            if(__attribute__b__) ____result.attribute = __attribute__;
            if(__criteria__b__) ____result.criteria = __criteria__;
            if(__duration__b__) ____result.duration = __duration__;
            if(__reference__b__) ____result.reference = __reference__;

            return ____result;
        }
    }


    public sealed class TestHelper_MatchingRuleFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.MatchingRule>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_MatchingRuleFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attribute"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("criteria"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reference"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("attribute"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("criteria"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reference"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.MatchingRule value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.attribute);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.criteria);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.reference);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.MatchingRule Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __attribute__ = default(string);
            var __attribute__b__ = false;
            var __criteria__ = default(string);
            var __criteria__b__ = false;
            var __reference__ = default(int);
            var __reference__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __attribute__ = reader.ReadString();
                        __attribute__b__ = true;
                        break;
                    case 1:
                        __criteria__ = reader.ReadString();
                        __criteria__b__ = true;
                        break;
                    case 2:
                        __reference__ = reader.ReadInt32();
                        __reference__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.MatchingRule();
            if(__attribute__b__) ____result.attribute = __attribute__;
            if(__criteria__b__) ____result.criteria = __criteria__;
            if(__reference__b__) ____result.reference = __reference__;

            return ____result;
        }
    }


    public sealed class TestHelper_RuleSetFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.RuleSet>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_RuleSetFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("alliance"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("flexing_rule"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matching_rule"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("alliance"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("flexing_rule"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("matching_rule"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.RuleSet value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AllianceRule>().Serialize(ref writer, value.alliance, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.FlexingRule[]>().Serialize(ref writer, value.flexing_rule, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.MatchingRule[]>().Serialize(ref writer, value.matching_rule, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.RuleSet Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __alliance__ = default(global::Tests.TestHelper.AllianceRule);
            var __alliance__b__ = false;
            var __flexing_rule__ = default(global::Tests.TestHelper.FlexingRule[]);
            var __flexing_rule__b__ = false;
            var __matching_rule__ = default(global::Tests.TestHelper.MatchingRule[]);
            var __matching_rule__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __alliance__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AllianceRule>().Deserialize(ref reader, formatterResolver);
                        __alliance__b__ = true;
                        break;
                    case 1:
                        __flexing_rule__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.FlexingRule[]>().Deserialize(ref reader, formatterResolver);
                        __flexing_rule__b__ = true;
                        break;
                    case 2:
                        __matching_rule__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.MatchingRule[]>().Deserialize(ref reader, formatterResolver);
                        __matching_rule__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.RuleSet();
            if(__alliance__b__) ____result.alliance = __alliance__;
            if(__flexing_rule__b__) ____result.flexing_rule = __flexing_rule__;
            if(__matching_rule__b__) ____result.matching_rule = __matching_rule__;

            return ____result;
        }
    }


    public sealed class TestHelper_CreateChannelRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CreateChannelRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CreateChannelRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("find_match_timeout_seconds"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_mode"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rule_set"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("joinable"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("find_match_timeout_seconds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("game_mode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("rule_set"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("joinable"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CreateChannelRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt32(value.find_match_timeout_seconds);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.game_mode);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RuleSet>().Serialize(ref writer, value.rule_set, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.joinable);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CreateChannelRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __description__ = default(string);
            var __description__b__ = false;
            var __find_match_timeout_seconds__ = default(uint);
            var __find_match_timeout_seconds__b__ = false;
            var __game_mode__ = default(string);
            var __game_mode__b__ = false;
            var __rule_set__ = default(global::Tests.TestHelper.RuleSet);
            var __rule_set__b__ = false;
            var __joinable__ = default(bool);
            var __joinable__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 1:
                        __find_match_timeout_seconds__ = reader.ReadUInt32();
                        __find_match_timeout_seconds__b__ = true;
                        break;
                    case 2:
                        __game_mode__ = reader.ReadString();
                        __game_mode__b__ = true;
                        break;
                    case 3:
                        __rule_set__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.RuleSet>().Deserialize(ref reader, formatterResolver);
                        __rule_set__b__ = true;
                        break;
                    case 4:
                        __joinable__ = reader.ReadBoolean();
                        __joinable__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CreateChannelRequest();
            if(__description__b__) ____result.description = __description__;
            if(__find_match_timeout_seconds__b__) ____result.find_match_timeout_seconds = __find_match_timeout_seconds__;
            if(__game_mode__b__) ____result.game_mode = __game_mode__;
            if(__rule_set__b__) ____result.rule_set = __rule_set__;
            if(__joinable__b__) ____result.joinable = __joinable__;

            return ____result;
        }
    }


    public sealed class TestHelper_CurrencyCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CurrencyCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CurrencyCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("decimals"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxAmountPerTransaction"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxTransactionAmountPerDay"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxBalanceAmount"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("decimals"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxAmountPerTransaction"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxTransactionAmountPerDay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxBalanceAmount"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CurrencyCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.decimals);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.maxAmountPerTransaction);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.maxTransactionAmountPerDay);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxBalanceAmount);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CurrencyCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __decimals__ = default(int);
            var __decimals__b__ = false;
            var __maxAmountPerTransaction__ = default(int);
            var __maxAmountPerTransaction__b__ = false;
            var __maxTransactionAmountPerDay__ = default(int);
            var __maxTransactionAmountPerDay__b__ = false;
            var __maxBalanceAmount__ = default(int);
            var __maxBalanceAmount__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 1:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 2:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 3:
                        __decimals__ = reader.ReadInt32();
                        __decimals__b__ = true;
                        break;
                    case 4:
                        __maxAmountPerTransaction__ = reader.ReadInt32();
                        __maxAmountPerTransaction__b__ = true;
                        break;
                    case 5:
                        __maxTransactionAmountPerDay__ = reader.ReadInt32();
                        __maxTransactionAmountPerDay__b__ = true;
                        break;
                    case 6:
                        __maxBalanceAmount__ = reader.ReadInt32();
                        __maxBalanceAmount__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CurrencyCreateModel();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__decimals__b__) ____result.decimals = __decimals__;
            if(__maxAmountPerTransaction__b__) ____result.maxAmountPerTransaction = __maxAmountPerTransaction__;
            if(__maxTransactionAmountPerDay__b__) ____result.maxTransactionAmountPerDay = __maxTransactionAmountPerDay__;
            if(__maxBalanceAmount__b__) ____result.maxBalanceAmount = __maxBalanceAmount__;

            return ____result;
        }
    }


    public sealed class TestHelper_CurrencyInfoModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CurrencyInfoModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CurrencyInfoModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizationDescriptions"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("decimals"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxAmountPerTransaction"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxTransactionAmountPerDay"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxBalanceAmount"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizationDescriptions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("decimals"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxAmountPerTransaction"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxTransactionAmountPerDay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxBalanceAmount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CurrencyInfoModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.localizationDescriptions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.decimals);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxAmountPerTransaction);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.maxTransactionAmountPerDay);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.maxBalanceAmount);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CurrencyInfoModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __localizationDescriptions__ = default(object);
            var __localizationDescriptions__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __decimals__ = default(int);
            var __decimals__b__ = false;
            var __maxAmountPerTransaction__ = default(int);
            var __maxAmountPerTransaction__b__ = false;
            var __maxTransactionAmountPerDay__ = default(int);
            var __maxTransactionAmountPerDay__b__ = false;
            var __maxBalanceAmount__ = default(int);
            var __maxBalanceAmount__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 1:
                        __localizationDescriptions__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __localizationDescriptions__b__ = true;
                        break;
                    case 2:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 3:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 4:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 5:
                        __decimals__ = reader.ReadInt32();
                        __decimals__b__ = true;
                        break;
                    case 6:
                        __maxAmountPerTransaction__ = reader.ReadInt32();
                        __maxAmountPerTransaction__b__ = true;
                        break;
                    case 7:
                        __maxTransactionAmountPerDay__ = reader.ReadInt32();
                        __maxTransactionAmountPerDay__b__ = true;
                        break;
                    case 8:
                        __maxBalanceAmount__ = reader.ReadInt32();
                        __maxBalanceAmount__b__ = true;
                        break;
                    case 9:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 10:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CurrencyInfoModel();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__localizationDescriptions__b__) ____result.localizationDescriptions = __localizationDescriptions__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__decimals__b__) ____result.decimals = __decimals__;
            if(__maxAmountPerTransaction__b__) ____result.maxAmountPerTransaction = __maxAmountPerTransaction__;
            if(__maxTransactionAmountPerDay__b__) ____result.maxTransactionAmountPerDay = __maxTransactionAmountPerDay__;
            if(__maxBalanceAmount__b__) ____result.maxBalanceAmount = __maxBalanceAmount__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_CurrencySummaryModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CurrencySummaryModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CurrencySummaryModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("decimals"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("decimals"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CurrencySummaryModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.decimals);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CurrencySummaryModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __decimals__ = default(int);
            var __decimals__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 1:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 2:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 3:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 4:
                        __decimals__ = reader.ReadInt32();
                        __decimals__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CurrencySummaryModel();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__decimals__b__) ____result.decimals = __decimals__;

            return ____result;
        }
    }


    public sealed class TestHelper_StoreCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.StoreCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_StoreCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedLanguages"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedRegions"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultRegion"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultLanguage"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedLanguages"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedRegions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultRegion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultLanguage"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.StoreCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedLanguages, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedRegions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.defaultRegion);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.defaultLanguage);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.StoreCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __supportedLanguages__ = default(string[]);
            var __supportedLanguages__b__ = false;
            var __supportedRegions__ = default(string[]);
            var __supportedRegions__b__ = false;
            var __defaultRegion__ = default(string);
            var __defaultRegion__b__ = false;
            var __defaultLanguage__ = default(string);
            var __defaultLanguage__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __supportedLanguages__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedLanguages__b__ = true;
                        break;
                    case 3:
                        __supportedRegions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedRegions__b__ = true;
                        break;
                    case 4:
                        __defaultRegion__ = reader.ReadString();
                        __defaultRegion__b__ = true;
                        break;
                    case 5:
                        __defaultLanguage__ = reader.ReadString();
                        __defaultLanguage__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.StoreCreateModel();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__supportedLanguages__b__) ____result.supportedLanguages = __supportedLanguages__;
            if(__supportedRegions__b__) ____result.supportedRegions = __supportedRegions__;
            if(__defaultRegion__b__) ____result.defaultRegion = __defaultRegion__;
            if(__defaultLanguage__b__) ____result.defaultLanguage = __defaultLanguage__;

            return ____result;
        }
    }


    public sealed class TestHelper_CategoryCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CategoryCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CategoryCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizationDisplayNames"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizationDisplayNames"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CategoryCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.localizationDisplayNames, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CategoryCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __localizationDisplayNames__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __localizationDisplayNames__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 1:
                        __localizationDisplayNames__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __localizationDisplayNames__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CategoryCreateModel();
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__localizationDisplayNames__b__) ____result.localizationDisplayNames = __localizationDisplayNames__;

            return ____result;
        }
    }


    public sealed class TestHelper_LocalExtFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.LocalExt>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_LocalExtFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("properties"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("functions"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("properties"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("functions"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.LocalExt value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.properties, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.functions, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.LocalExt Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __properties__ = default(string[]);
            var __properties__b__ = false;
            var __functions__ = default(string[]);
            var __functions__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __properties__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __properties__b__ = true;
                        break;
                    case 1:
                        __functions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __functions__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.LocalExt();
            if(__properties__b__) ____result.properties = __properties__;
            if(__functions__b__) ____result.functions = __functions__;

            return ____result;
        }
    }


    public sealed class TestHelper_RegionDataUSFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.RegionDataUS>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_RegionDataUSFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("US"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("US"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.RegionDataUS value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<RegionDataItem[]>().Serialize(ref writer, value.US, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.RegionDataUS Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __US__ = default(RegionDataItem[]);
            var __US__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __US__ = formatterResolver.GetFormatterWithVerify<RegionDataItem[]>().Deserialize(ref reader, formatterResolver);
                        __US__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.RegionDataUS();
            if(__US__b__) ____result.US = __US__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_LocalizationFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.Localization>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_LocalizationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("longDescription"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("images"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailImage"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("longDescription"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("images"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailImage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.Localization value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.longDescription);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<Image[]>().Serialize(ref writer, value.images, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<Image>().Serialize(ref writer, value.thumbnailImage, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.title);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.Localization Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __longDescription__ = default(string);
            var __longDescription__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __images__ = default(Image[]);
            var __images__b__ = false;
            var __thumbnailImage__ = default(Image);
            var __thumbnailImage__b__ = false;
            var __title__ = default(string);
            var __title__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __longDescription__ = reader.ReadString();
                        __longDescription__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __images__ = formatterResolver.GetFormatterWithVerify<Image[]>().Deserialize(ref reader, formatterResolver);
                        __images__b__ = true;
                        break;
                    case 3:
                        __thumbnailImage__ = formatterResolver.GetFormatterWithVerify<Image>().Deserialize(ref reader, formatterResolver);
                        __thumbnailImage__b__ = true;
                        break;
                    case 4:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.Localization();
            if(__longDescription__b__) ____result.longDescription = __longDescription__;
            if(__description__b__) ____result.description = __description__;
            if(__images__b__) ____result.images = __images__;
            if(__thumbnailImage__b__) ____result.thumbnailImage = __thumbnailImage__;
            if(__title__b__) ____result.title = __title__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_RecurringFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.Recurring>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_RecurringFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("cycle"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("fixedFreeDays"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("fixedTrialCycles"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("graceDays"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("cycle"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("fixedFreeDays"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("fixedTrialCycles"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("graceDays"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.Recurring value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.cycle);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.fixedFreeDays);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.fixedTrialCycles);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.graceDays);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.Recurring Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __cycle__ = default(string);
            var __cycle__b__ = false;
            var __fixedFreeDays__ = default(int);
            var __fixedFreeDays__b__ = false;
            var __fixedTrialCycles__ = default(int);
            var __fixedTrialCycles__b__ = false;
            var __graceDays__ = default(int);
            var __graceDays__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __cycle__ = reader.ReadString();
                        __cycle__b__ = true;
                        break;
                    case 1:
                        __fixedFreeDays__ = reader.ReadInt32();
                        __fixedFreeDays__b__ = true;
                        break;
                    case 2:
                        __fixedTrialCycles__ = reader.ReadInt32();
                        __fixedTrialCycles__b__ = true;
                        break;
                    case 3:
                        __graceDays__ = reader.ReadInt32();
                        __graceDays__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.Recurring();
            if(__cycle__b__) ____result.cycle = __cycle__;
            if(__fixedFreeDays__b__) ____result.fixedFreeDays = __fixedFreeDays__;
            if(__fixedTrialCycles__b__) ____result.fixedTrialCycles = __fixedTrialCycles__;
            if(__graceDays__b__) ____result.graceDays = __graceDays__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("seasonType"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackable"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseAppId"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizations"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("images"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailUrl"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("boothName"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayOrder"), 23},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("clazz"), 24},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("recurring"), 25},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("seasonType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseAppId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizations"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("images"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("boothName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayOrder"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("clazz"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("recurring"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemType);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<SeasonType>().Serialize(ref writer, value.seasonType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.entitlementType);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteBoolean(value.stackable);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.appType);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.baseAppId);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.ItemCreateModel.Localization>>().Serialize(ref writer, value.localizations, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<Image[]>().Serialize(ref writer, value.images, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.thumbnailUrl);
            writer.WriteRaw(this.____stringByteKeys[17]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, RegionDataItem[]>>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[18]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[21]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[22]);
            writer.WriteString(value.boothName);
            writer.WriteRaw(this.____stringByteKeys[23]);
            writer.WriteInt32(value.displayOrder);
            writer.WriteRaw(this.____stringByteKeys[24]);
            writer.WriteString(value.clazz);
            writer.WriteRaw(this.____stringByteKeys[25]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Recurring>().Serialize(ref writer, value.recurring, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemType__ = default(string);
            var __itemType__b__ = false;
            var __seasonType__ = default(SeasonType);
            var __seasonType__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(string);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __stackable__ = default(bool);
            var __stackable__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(string);
            var __appType__b__ = false;
            var __baseAppId__ = default(string);
            var __baseAppId__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __localizations__ = default(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.ItemCreateModel.Localization>);
            var __localizations__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __images__ = default(Image[]);
            var __images__b__ = false;
            var __thumbnailUrl__ = default(string);
            var __thumbnailUrl__b__ = false;
            var __regionData__ = default(global::System.Collections.Generic.Dictionary<string, RegionDataItem[]>);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
            var __boothName__ = default(string);
            var __boothName__b__ = false;
            var __displayOrder__ = default(int);
            var __displayOrder__b__ = false;
            var __clazz__ = default(string);
            var __clazz__b__ = false;
            var __recurring__ = default(global::Tests.TestHelper.ItemCreateModel.Recurring);
            var __recurring__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemType__ = reader.ReadString();
                        __itemType__b__ = true;
                        break;
                    case 1:
                        __seasonType__ = formatterResolver.GetFormatterWithVerify<SeasonType>().Deserialize(ref reader, formatterResolver);
                        __seasonType__b__ = true;
                        break;
                    case 2:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 3:
                        __entitlementType__ = reader.ReadString();
                        __entitlementType__b__ = true;
                        break;
                    case 4:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 5:
                        __stackable__ = reader.ReadBoolean();
                        __stackable__b__ = true;
                        break;
                    case 6:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 7:
                        __appType__ = reader.ReadString();
                        __appType__b__ = true;
                        break;
                    case 8:
                        __baseAppId__ = reader.ReadString();
                        __baseAppId__b__ = true;
                        break;
                    case 9:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 10:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 11:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 12:
                        __localizations__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.ItemCreateModel.Localization>>().Deserialize(ref reader, formatterResolver);
                        __localizations__b__ = true;
                        break;
                    case 13:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 14:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 15:
                        __images__ = formatterResolver.GetFormatterWithVerify<Image[]>().Deserialize(ref reader, formatterResolver);
                        __images__b__ = true;
                        break;
                    case 16:
                        __thumbnailUrl__ = reader.ReadString();
                        __thumbnailUrl__b__ = true;
                        break;
                    case 17:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, RegionDataItem[]>>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 18:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 19:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 20:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 21:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 22:
                        __boothName__ = reader.ReadString();
                        __boothName__b__ = true;
                        break;
                    case 23:
                        __displayOrder__ = reader.ReadInt32();
                        __displayOrder__b__ = true;
                        break;
                    case 24:
                        __clazz__ = reader.ReadString();
                        __clazz__b__ = true;
                        break;
                    case 25:
                        __recurring__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Recurring>().Deserialize(ref reader, formatterResolver);
                        __recurring__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel();
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__seasonType__b__) ____result.seasonType = __seasonType__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__stackable__b__) ____result.stackable = __stackable__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__baseAppId__b__) ____result.baseAppId = __baseAppId__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__localizations__b__) ____result.localizations = __localizations__;
            if(__status__b__) ____result.status = __status__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__images__b__) ____result.images = __images__;
            if(__thumbnailUrl__b__) ____result.thumbnailUrl = __thumbnailUrl__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
            if(__boothName__b__) ____result.boothName = __boothName__;
            if(__displayOrder__b__) ____result.displayOrder = __displayOrder__;
            if(__clazz__b__) ____result.clazz = __clazz__;
            if(__recurring__b__) ____result.recurring = __recurring__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_RegionDatasFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.RegionDatas>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_RegionDatasFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("US"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("US"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.RegionDatas value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<RegionDataItem[]>().Serialize(ref writer, value.US, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.RegionDatas Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __US__ = default(RegionDataItem[]);
            var __US__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __US__ = formatterResolver.GetFormatterWithVerify<RegionDataItem[]>().Deserialize(ref reader, formatterResolver);
                        __US__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.RegionDatas();
            if(__US__b__) ____result.US = __US__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_LocalizationsFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.Localizations>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_LocalizationsFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("en"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("en"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.Localizations value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Localization>().Serialize(ref writer, value.en, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.Localizations Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __en__ = default(global::Tests.TestHelper.ItemCreateModel.Localization);
            var __en__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __en__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Localization>().Deserialize(ref reader, formatterResolver);
                        __en__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.Localizations();
            if(__en__b__) ____result.en = __en__;

            return ____result;
        }
    }


    public sealed class TestHelper_StoreInfoModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.StoreInfoModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_StoreInfoModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("storeId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("published"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedLanguages"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedRegions"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultRegion"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultLanguage"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("storeId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("published"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedLanguages"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedRegions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultRegion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultLanguage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.StoreInfoModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.storeId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.published);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedLanguages, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedRegions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.defaultRegion);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.defaultLanguage);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.StoreInfoModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __storeId__ = default(string);
            var __storeId__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __published__ = default(bool);
            var __published__b__ = false;
            var __supportedLanguages__ = default(string[]);
            var __supportedLanguages__b__ = false;
            var __supportedRegions__ = default(string[]);
            var __supportedRegions__b__ = false;
            var __defaultRegion__ = default(string);
            var __defaultRegion__b__ = false;
            var __defaultLanguage__ = default(string);
            var __defaultLanguage__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __storeId__ = reader.ReadString();
                        __storeId__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 3:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 4:
                        __published__ = reader.ReadBoolean();
                        __published__b__ = true;
                        break;
                    case 5:
                        __supportedLanguages__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedLanguages__b__ = true;
                        break;
                    case 6:
                        __supportedRegions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedRegions__b__ = true;
                        break;
                    case 7:
                        __defaultRegion__ = reader.ReadString();
                        __defaultRegion__b__ = true;
                        break;
                    case 8:
                        __defaultLanguage__ = reader.ReadString();
                        __defaultLanguage__b__ = true;
                        break;
                    case 9:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 10:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.StoreInfoModel();
            if(__storeId__b__) ____result.storeId = __storeId__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__published__b__) ____result.published = __published__;
            if(__supportedLanguages__b__) ____result.supportedLanguages = __supportedLanguages__;
            if(__supportedRegions__b__) ____result.supportedRegions = __supportedRegions__;
            if(__defaultRegion__b__) ____result.defaultRegion = __defaultRegion__;
            if(__defaultLanguage__b__) ____result.defaultLanguage = __defaultLanguage__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_FullCategoryInfoFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FullCategoryInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FullCategoryInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("parentCategoryPath"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizationDisplayNames"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("root"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parentCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizationDisplayNames"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("root"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FullCategoryInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.parentCategoryPath);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.localizationDisplayNames, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.root);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FullCategoryInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __parentCategoryPath__ = default(string);
            var __parentCategoryPath__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __localizationDisplayNames__ = default(object);
            var __localizationDisplayNames__b__ = false;
            var __root__ = default(bool);
            var __root__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 1:
                        __parentCategoryPath__ = reader.ReadString();
                        __parentCategoryPath__b__ = true;
                        break;
                    case 2:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 3:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 4:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 5:
                        __localizationDisplayNames__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __localizationDisplayNames__b__ = true;
                        break;
                    case 6:
                        __root__ = reader.ReadBoolean();
                        __root__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FullCategoryInfo();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__parentCategoryPath__b__) ____result.parentCategoryPath = __parentCategoryPath__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__localizationDisplayNames__b__) ____result.localizationDisplayNames = __localizationDisplayNames__;
            if(__root__b__) ____result.root = __root__;

            return ____result;
        }
    }


    public sealed class TestHelper_FullItemInfoFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FullItemInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FullItemInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizations"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("codeFiles"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 21},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizations"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("codeFiles"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FullItemInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.appType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.entitlementType);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.localizations, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.itemType);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.codeFiles, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FullItemInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(string);
            var __appType__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(string);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __localizations__ = default(object);
            var __localizations__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __itemType__ = default(string);
            var __itemType__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __regionData__ = default(object);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
            var __codeFiles__ = default(string[]);
            var __codeFiles__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 1:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 2:
                        __appType__ = reader.ReadString();
                        __appType__b__ = true;
                        break;
                    case 3:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 4:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 5:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 6:
                        __entitlementType__ = reader.ReadString();
                        __entitlementType__b__ = true;
                        break;
                    case 7:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 8:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 9:
                        __localizations__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __localizations__b__ = true;
                        break;
                    case 10:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 11:
                        __itemType__ = reader.ReadString();
                        __itemType__b__ = true;
                        break;
                    case 12:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 13:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 14:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 15:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 16:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 17:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 18:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 19:
                        __codeFiles__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __codeFiles__b__ = true;
                        break;
                    case 20:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 21:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FullItemInfo();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__localizations__b__) ____result.localizations = __localizations__;
            if(__status__b__) ____result.status = __status__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
            if(__codeFiles__b__) ____result.codeFiles = __codeFiles__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_CreditRequestModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CreditRequestModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CreditRequestModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("amount"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("source"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reason"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("amount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("source"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CreditRequestModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.amount);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.source);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.reason);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CreditRequestModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __amount__ = default(int);
            var __amount__b__ = false;
            var __source__ = default(string);
            var __source__b__ = false;
            var __reason__ = default(string);
            var __reason__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __amount__ = reader.ReadInt32();
                        __amount__b__ = true;
                        break;
                    case 1:
                        __source__ = reader.ReadString();
                        __source__b__ = true;
                        break;
                    case 2:
                        __reason__ = reader.ReadString();
                        __reason__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CreditRequestModel();
            if(__amount__b__) ____result.amount = __amount__;
            if(__source__b__) ____result.source = __source__;
            if(__reason__b__) ____result.reason = __reason__;

            return ____result;
        }
    }


    public sealed class TestHelper_FulfillmentModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FulfillmentModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FulfillmentModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FulfillmentModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.quantity);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FulfillmentModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 1:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FulfillmentModel();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__quantity__b__) ____result.quantity = __quantity__;

            return ____result;
        }
    }


    public sealed class TestHelper_UserMapResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.UserMapResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_UserMapResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("UserId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("UserId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.UserMapResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.UserId);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.UserMapResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __UserId__ = default(string);
            var __UserId__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 1:
                        __UserId__ = reader.ReadString();
                        __UserId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.UserMapResponse();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__UserId__b__) ____result.UserId = __UserId__;

            return ____result;
        }
    }


    public sealed class TestHelper_StatCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.StatCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_StatCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultValue"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incrementOnly"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maximum"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("minimum"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("setAsGlobal"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("setBy"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("defaultValue"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("incrementOnly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maximum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minimum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("setAsGlobal"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("setBy"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.StatCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.defaultValue);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.incrementOnly);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteSingle(value.maximum);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteSingle(value.minimum);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.setAsGlobal);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<StatisticSetBy>().Serialize(ref writer, value.setBy, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.StatCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __defaultValue__ = default(float);
            var __defaultValue__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __incrementOnly__ = default(bool);
            var __incrementOnly__b__ = false;
            var __maximum__ = default(float);
            var __maximum__b__ = false;
            var __minimum__ = default(float);
            var __minimum__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __setAsGlobal__ = default(bool);
            var __setAsGlobal__b__ = false;
            var __setBy__ = default(StatisticSetBy);
            var __setBy__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __defaultValue__ = reader.ReadSingle();
                        __defaultValue__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __incrementOnly__ = reader.ReadBoolean();
                        __incrementOnly__b__ = true;
                        break;
                    case 3:
                        __maximum__ = reader.ReadSingle();
                        __maximum__b__ = true;
                        break;
                    case 4:
                        __minimum__ = reader.ReadSingle();
                        __minimum__b__ = true;
                        break;
                    case 5:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 6:
                        __setAsGlobal__ = reader.ReadBoolean();
                        __setAsGlobal__b__ = true;
                        break;
                    case 7:
                        __setBy__ = formatterResolver.GetFormatterWithVerify<StatisticSetBy>().Deserialize(ref reader, formatterResolver);
                        __setBy__b__ = true;
                        break;
                    case 8:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 9:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.StatCreateModel();
            if(__defaultValue__b__) ____result.defaultValue = __defaultValue__;
            if(__description__b__) ____result.description = __description__;
            if(__incrementOnly__b__) ____result.incrementOnly = __incrementOnly__;
            if(__maximum__b__) ____result.maximum = __maximum__;
            if(__minimum__b__) ____result.minimum = __minimum__;
            if(__name__b__) ____result.name = __name__;
            if(__setAsGlobal__b__) ____result.setAsGlobal = __setAsGlobal__;
            if(__setBy__b__) ____result.setBy = __setBy__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__tags__b__) ____result.tags = __tags__;

            return ____result;
        }
    }


    public sealed class TestHelper_UserVerificationCodeFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.UserVerificationCode>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_UserVerificationCodeFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("accountRegistration"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("accountUpgrade"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("passwordReset"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updateEmail"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("accountRegistration"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("accountUpgrade"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("passwordReset"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updateEmail"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.UserVerificationCode value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.accountRegistration);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.accountUpgrade);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.passwordReset);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.updateEmail);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.UserVerificationCode Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __accountRegistration__ = default(string);
            var __accountRegistration__b__ = false;
            var __accountUpgrade__ = default(string);
            var __accountUpgrade__b__ = false;
            var __passwordReset__ = default(string);
            var __passwordReset__b__ = false;
            var __updateEmail__ = default(string);
            var __updateEmail__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __accountRegistration__ = reader.ReadString();
                        __accountRegistration__b__ = true;
                        break;
                    case 1:
                        __accountUpgrade__ = reader.ReadString();
                        __accountUpgrade__b__ = true;
                        break;
                    case 2:
                        __passwordReset__ = reader.ReadString();
                        __passwordReset__b__ = true;
                        break;
                    case 3:
                        __updateEmail__ = reader.ReadString();
                        __updateEmail__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.UserVerificationCode();
            if(__accountRegistration__b__) ____result.accountRegistration = __accountRegistration__;
            if(__accountUpgrade__b__) ____result.accountUpgrade = __accountUpgrade__;
            if(__passwordReset__b__) ____result.passwordReset = __passwordReset__;
            if(__updateEmail__b__) ____result.updateEmail = __updateEmail__;

            return ____result;
        }
    }


    public sealed class TestHelper_AchievementRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AchievementRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AchievementRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("achievementCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultLanguage"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lockedIcons"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("unlockedIcons"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("hidden"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incremental"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("goalValue"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("achievementCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultLanguage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lockedIcons"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("unlockedIcons"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hidden"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("incremental"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("goalValue"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AchievementRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.achievementCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.defaultLanguage);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.name, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.description, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Serialize(ref writer, value.lockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Serialize(ref writer, value.unlockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.hidden);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteBoolean(value.incremental);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteSingle(value.goalValue);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AchievementRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __achievementCode__ = default(string);
            var __achievementCode__b__ = false;
            var __defaultLanguage__ = default(string);
            var __defaultLanguage__b__ = false;
            var __name__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __name__b__ = false;
            var __description__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __description__b__ = false;
            var __lockedIcons__ = default(AchievementIcon[]);
            var __lockedIcons__b__ = false;
            var __unlockedIcons__ = default(AchievementIcon[]);
            var __unlockedIcons__b__ = false;
            var __hidden__ = default(bool);
            var __hidden__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __incremental__ = default(bool);
            var __incremental__b__ = false;
            var __goalValue__ = default(float);
            var __goalValue__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __achievementCode__ = reader.ReadString();
                        __achievementCode__b__ = true;
                        break;
                    case 1:
                        __defaultLanguage__ = reader.ReadString();
                        __defaultLanguage__b__ = true;
                        break;
                    case 2:
                        __name__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __name__b__ = true;
                        break;
                    case 3:
                        __description__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __description__b__ = true;
                        break;
                    case 4:
                        __lockedIcons__ = formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __lockedIcons__b__ = true;
                        break;
                    case 5:
                        __unlockedIcons__ = formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __unlockedIcons__b__ = true;
                        break;
                    case 6:
                        __hidden__ = reader.ReadBoolean();
                        __hidden__b__ = true;
                        break;
                    case 7:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 8:
                        __incremental__ = reader.ReadBoolean();
                        __incremental__b__ = true;
                        break;
                    case 9:
                        __goalValue__ = reader.ReadSingle();
                        __goalValue__b__ = true;
                        break;
                    case 10:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AchievementRequest();
            if(__achievementCode__b__) ____result.achievementCode = __achievementCode__;
            if(__defaultLanguage__b__) ____result.defaultLanguage = __defaultLanguage__;
            if(__name__b__) ____result.name = __name__;
            if(__description__b__) ____result.description = __description__;
            if(__lockedIcons__b__) ____result.lockedIcons = __lockedIcons__;
            if(__unlockedIcons__b__) ____result.unlockedIcons = __unlockedIcons__;
            if(__hidden__b__) ____result.hidden = __hidden__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__incremental__b__) ____result.incremental = __incremental__;
            if(__goalValue__b__) ____result.goalValue = __goalValue__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class TestHelper_AchievementResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AchievementResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AchievementResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("achievementCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultLanguage"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lockedIcons"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("unlockedIcons"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("hidden"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("listOrder"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incremental"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("goalValue"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 14},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("achievementCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultLanguage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lockedIcons"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("unlockedIcons"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hidden"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("listOrder"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("incremental"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("goalValue"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AchievementResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.achievementCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.defaultLanguage);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.name, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.description, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Serialize(ref writer, value.lockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Serialize(ref writer, value.unlockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteBoolean(value.hidden);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.listOrder);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteBoolean(value.incremental);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteSingle(value.goalValue);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.updatedAt);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AchievementResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __achievementCode__ = default(string);
            var __achievementCode__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __defaultLanguage__ = default(string);
            var __defaultLanguage__b__ = false;
            var __name__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __name__b__ = false;
            var __description__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __description__b__ = false;
            var __lockedIcons__ = default(AchievementIcon[]);
            var __lockedIcons__b__ = false;
            var __unlockedIcons__ = default(AchievementIcon[]);
            var __unlockedIcons__b__ = false;
            var __hidden__ = default(bool);
            var __hidden__b__ = false;
            var __listOrder__ = default(int);
            var __listOrder__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __incremental__ = default(bool);
            var __incremental__b__ = false;
            var __goalValue__ = default(float);
            var __goalValue__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(string);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __achievementCode__ = reader.ReadString();
                        __achievementCode__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __defaultLanguage__ = reader.ReadString();
                        __defaultLanguage__b__ = true;
                        break;
                    case 3:
                        __name__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __name__b__ = true;
                        break;
                    case 4:
                        __description__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __description__b__ = true;
                        break;
                    case 5:
                        __lockedIcons__ = formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __lockedIcons__b__ = true;
                        break;
                    case 6:
                        __unlockedIcons__ = formatterResolver.GetFormatterWithVerify<AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __unlockedIcons__b__ = true;
                        break;
                    case 7:
                        __hidden__ = reader.ReadBoolean();
                        __hidden__b__ = true;
                        break;
                    case 8:
                        __listOrder__ = reader.ReadInt32();
                        __listOrder__b__ = true;
                        break;
                    case 9:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 10:
                        __incremental__ = reader.ReadBoolean();
                        __incremental__b__ = true;
                        break;
                    case 11:
                        __goalValue__ = reader.ReadSingle();
                        __goalValue__b__ = true;
                        break;
                    case 12:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 13:
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 14:
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AchievementResponse();
            if(__achievementCode__b__) ____result.achievementCode = __achievementCode__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__defaultLanguage__b__) ____result.defaultLanguage = __defaultLanguage__;
            if(__name__b__) ____result.name = __name__;
            if(__description__b__) ____result.description = __description__;
            if(__lockedIcons__b__) ____result.lockedIcons = __lockedIcons__;
            if(__unlockedIcons__b__) ____result.unlockedIcons = __unlockedIcons__;
            if(__hidden__b__) ____result.hidden = __hidden__;
            if(__listOrder__b__) ____result.listOrder = __listOrder__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__incremental__b__) ____result.incremental = __incremental__;
            if(__goalValue__b__) ____result.goalValue = __goalValue__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_ReportingAdminReasonItemFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ReportingAdminReasonItem>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ReportingAdminReasonItemFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ReportingAdminReasonItem value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.updatedAt);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ReportingAdminReasonItem Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(string);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 2:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 3:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 4:
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 5:
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ReportingAdminReasonItem();
            if(__id__b__) ____result.id = __id__;
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_ReportingAdminReasonsResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ReportingAdminReasonsResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ReportingAdminReasonsResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("data"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ReportingAdminReasonsResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ReportingAdminReasonItem[]>().Serialize(ref writer, value.data, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ReportingAdminReasonsResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::Tests.TestHelper.ReportingAdminReasonItem[]);
            var __data__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __data__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ReportingAdminReasonItem[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ReportingAdminReasonsResponse();
            if(__data__b__) ____result.data = __data__;

            return ____result;
        }
    }


    public sealed class TestHelper_ReportingAddReasonRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ReportingAddReasonRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ReportingAddReasonRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ReportingAddReasonRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ReportingAddReasonRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ReportingAddReasonRequest();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;

            return ____result;
        }
    }


    public sealed class TestHelper_ReportingAddReasonGroupRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ReportingAddReasonGroupRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ReportingAddReasonGroupRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reasonIds"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reasonIds"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ReportingAddReasonGroupRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.reasonIds, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ReportingAddReasonGroupRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __reasonIds__ = default(string[]);
            var __reasonIds__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 1:
                        __reasonIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __reasonIds__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ReportingAddReasonGroupRequest();
            if(__title__b__) ____result.title = __title__;
            if(__reasonIds__b__) ____result.reasonIds = __reasonIds__;

            return ____result;
        }
    }


    public sealed class TestHelper_LeaderboardDailyConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.LeaderboardDailyConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_LeaderboardDailyConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetTime"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("resetTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.LeaderboardDailyConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.resetTime);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.LeaderboardDailyConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __resetTime__ = default(string);
            var __resetTime__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __resetTime__ = reader.ReadString();
                        __resetTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.LeaderboardDailyConfig();
            if(__resetTime__b__) ____result.resetTime = __resetTime__;

            return ____result;
        }
    }


    public sealed class TestHelper_LeaderboardMonthlyConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.LeaderboardMonthlyConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_LeaderboardMonthlyConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetDate"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetTime"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("resetDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("resetTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.LeaderboardMonthlyConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.resetDate);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.resetTime);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.LeaderboardMonthlyConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __resetDate__ = default(int);
            var __resetDate__b__ = false;
            var __resetTime__ = default(string);
            var __resetTime__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __resetDate__ = reader.ReadInt32();
                        __resetDate__b__ = true;
                        break;
                    case 1:
                        __resetTime__ = reader.ReadString();
                        __resetTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.LeaderboardMonthlyConfig();
            if(__resetDate__b__) ____result.resetDate = __resetDate__;
            if(__resetTime__b__) ____result.resetTime = __resetTime__;

            return ____result;
        }
    }


    public sealed class TestHelper_LeaderboardWeeklyConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.LeaderboardWeeklyConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_LeaderboardWeeklyConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetDay"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetTime"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("resetDay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("resetTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.LeaderboardWeeklyConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.resetDay);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.resetTime);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.LeaderboardWeeklyConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __resetDay__ = default(int);
            var __resetDay__b__ = false;
            var __resetTime__ = default(string);
            var __resetTime__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __resetDay__ = reader.ReadInt32();
                        __resetDay__b__ = true;
                        break;
                    case 1:
                        __resetTime__ = reader.ReadString();
                        __resetTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.LeaderboardWeeklyConfig();
            if(__resetDay__b__) ____result.resetDay = __resetDay__;
            if(__resetTime__b__) ____result.resetTime = __resetTime__;

            return ____result;
        }
    }


    public sealed class TestHelper_LeaderboardConfigRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.LeaderboardConfigRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_LeaderboardConfigRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("daily"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("monthly"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("weekly"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("descending"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("iconURL"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("leaderboardCode"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("seasonPeriod"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("startTime"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("daily"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("monthly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("weekly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("descending"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("iconURL"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("leaderboardCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("seasonPeriod"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("startTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.LeaderboardConfigRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardDailyConfig>().Serialize(ref writer, value.daily, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardMonthlyConfig>().Serialize(ref writer, value.monthly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardWeeklyConfig>().Serialize(ref writer, value.weekly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.descending);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.iconURL);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.leaderboardCode);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.seasonPeriod);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.startTime);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.LeaderboardConfigRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __daily__ = default(global::Tests.TestHelper.LeaderboardDailyConfig);
            var __daily__b__ = false;
            var __monthly__ = default(global::Tests.TestHelper.LeaderboardMonthlyConfig);
            var __monthly__b__ = false;
            var __weekly__ = default(global::Tests.TestHelper.LeaderboardWeeklyConfig);
            var __weekly__b__ = false;
            var __descending__ = default(bool);
            var __descending__b__ = false;
            var __iconURL__ = default(string);
            var __iconURL__b__ = false;
            var __leaderboardCode__ = default(string);
            var __leaderboardCode__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __seasonPeriod__ = default(int);
            var __seasonPeriod__b__ = false;
            var __startTime__ = default(string);
            var __startTime__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __daily__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardDailyConfig>().Deserialize(ref reader, formatterResolver);
                        __daily__b__ = true;
                        break;
                    case 1:
                        __monthly__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardMonthlyConfig>().Deserialize(ref reader, formatterResolver);
                        __monthly__b__ = true;
                        break;
                    case 2:
                        __weekly__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardWeeklyConfig>().Deserialize(ref reader, formatterResolver);
                        __weekly__b__ = true;
                        break;
                    case 3:
                        __descending__ = reader.ReadBoolean();
                        __descending__b__ = true;
                        break;
                    case 4:
                        __iconURL__ = reader.ReadString();
                        __iconURL__b__ = true;
                        break;
                    case 5:
                        __leaderboardCode__ = reader.ReadString();
                        __leaderboardCode__b__ = true;
                        break;
                    case 6:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 7:
                        __seasonPeriod__ = reader.ReadInt32();
                        __seasonPeriod__b__ = true;
                        break;
                    case 8:
                        __startTime__ = reader.ReadString();
                        __startTime__b__ = true;
                        break;
                    case 9:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.LeaderboardConfigRequest();
            if(__daily__b__) ____result.daily = __daily__;
            if(__monthly__b__) ____result.monthly = __monthly__;
            if(__weekly__b__) ____result.weekly = __weekly__;
            if(__descending__b__) ____result.descending = __descending__;
            if(__iconURL__b__) ____result.iconURL = __iconURL__;
            if(__leaderboardCode__b__) ____result.leaderboardCode = __leaderboardCode__;
            if(__name__b__) ____result.name = __name__;
            if(__seasonPeriod__b__) ____result.seasonPeriod = __seasonPeriod__;
            if(__startTime__b__) ____result.startTime = __startTime__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class TestHelper_LeaderboardConfigResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.LeaderboardConfigResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_LeaderboardConfigResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("daily"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("monthly"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("weekly"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("descending"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("iconURL"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("leaderboardCode"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("seasonPeriod"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("startTime"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("daily"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("monthly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("weekly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("descending"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("iconURL"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("leaderboardCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("seasonPeriod"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("startTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.LeaderboardConfigResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardDailyConfig>().Serialize(ref writer, value.daily, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardMonthlyConfig>().Serialize(ref writer, value.monthly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardWeeklyConfig>().Serialize(ref writer, value.weekly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.descending);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.iconURL);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.leaderboardCode);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.seasonPeriod);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.startTime);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.LeaderboardConfigResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __daily__ = default(global::Tests.TestHelper.LeaderboardDailyConfig);
            var __daily__b__ = false;
            var __monthly__ = default(global::Tests.TestHelper.LeaderboardMonthlyConfig);
            var __monthly__b__ = false;
            var __weekly__ = default(global::Tests.TestHelper.LeaderboardWeeklyConfig);
            var __weekly__b__ = false;
            var __descending__ = default(bool);
            var __descending__b__ = false;
            var __iconURL__ = default(string);
            var __iconURL__b__ = false;
            var __leaderboardCode__ = default(string);
            var __leaderboardCode__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __seasonPeriod__ = default(int);
            var __seasonPeriod__b__ = false;
            var __startTime__ = default(string);
            var __startTime__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __daily__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardDailyConfig>().Deserialize(ref reader, formatterResolver);
                        __daily__b__ = true;
                        break;
                    case 1:
                        __monthly__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardMonthlyConfig>().Deserialize(ref reader, formatterResolver);
                        __monthly__b__ = true;
                        break;
                    case 2:
                        __weekly__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.LeaderboardWeeklyConfig>().Deserialize(ref reader, formatterResolver);
                        __weekly__b__ = true;
                        break;
                    case 3:
                        __descending__ = reader.ReadBoolean();
                        __descending__b__ = true;
                        break;
                    case 4:
                        __iconURL__ = reader.ReadString();
                        __iconURL__b__ = true;
                        break;
                    case 5:
                        __leaderboardCode__ = reader.ReadString();
                        __leaderboardCode__b__ = true;
                        break;
                    case 6:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 7:
                        __seasonPeriod__ = reader.ReadInt32();
                        __seasonPeriod__b__ = true;
                        break;
                    case 8:
                        __startTime__ = reader.ReadString();
                        __startTime__b__ = true;
                        break;
                    case 9:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.LeaderboardConfigResponse();
            if(__daily__b__) ____result.daily = __daily__;
            if(__monthly__b__) ____result.monthly = __monthly__;
            if(__weekly__b__) ____result.weekly = __weekly__;
            if(__descending__b__) ____result.descending = __descending__;
            if(__iconURL__b__) ____result.iconURL = __iconURL__;
            if(__leaderboardCode__b__) ____result.leaderboardCode = __leaderboardCode__;
            if(__name__b__) ____result.name = __name__;
            if(__seasonPeriod__b__) ____result.seasonPeriod = __seasonPeriod__;
            if(__startTime__b__) ____result.startTime = __startTime__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class TestHelper_CreateGroupConfigResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CreateGroupConfigResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CreateGroupConfigResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("configurationCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("globalRules"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("groupAdminRoleId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("groupMaxMember"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("groupMemberRoleId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("configurationCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("globalRules"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupAdminRoleId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupMaxMember"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupMemberRoleId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CreateGroupConfigResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.configurationCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<Rules[]>().Serialize(ref writer, value.globalRules, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.groupAdminRoleId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.groupMaxMember);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.groupMemberRoleId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.Namespace);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CreateGroupConfigResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __configurationCode__ = default(string);
            var __configurationCode__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __globalRules__ = default(Rules[]);
            var __globalRules__b__ = false;
            var __groupAdminRoleId__ = default(string);
            var __groupAdminRoleId__b__ = false;
            var __groupMaxMember__ = default(int);
            var __groupMaxMember__b__ = false;
            var __groupMemberRoleId__ = default(string);
            var __groupMemberRoleId__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __configurationCode__ = reader.ReadString();
                        __configurationCode__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __globalRules__ = formatterResolver.GetFormatterWithVerify<Rules[]>().Deserialize(ref reader, formatterResolver);
                        __globalRules__b__ = true;
                        break;
                    case 3:
                        __groupAdminRoleId__ = reader.ReadString();
                        __groupAdminRoleId__b__ = true;
                        break;
                    case 4:
                        __groupMaxMember__ = reader.ReadInt32();
                        __groupMaxMember__b__ = true;
                        break;
                    case 5:
                        __groupMemberRoleId__ = reader.ReadString();
                        __groupMemberRoleId__b__ = true;
                        break;
                    case 6:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 7:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CreateGroupConfigResponse();
            if(__configurationCode__b__) ____result.configurationCode = __configurationCode__;
            if(__description__b__) ____result.description = __description__;
            if(__globalRules__b__) ____result.globalRules = __globalRules__;
            if(__groupAdminRoleId__b__) ____result.groupAdminRoleId = __groupAdminRoleId__;
            if(__groupMaxMember__b__) ____result.groupMaxMember = __groupMaxMember__;
            if(__groupMemberRoleId__b__) ____result.groupMemberRoleId = __groupMemberRoleId__;
            if(__name__b__) ____result.name = __name__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;

            return ____result;
        }
    }


    public sealed class TestHelper_PaginatedGroupConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.PaginatedGroupConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_PaginatedGroupConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paging"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("data"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paging"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.PaginatedGroupConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.CreateGroupConfigResponse[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.PaginatedGroupConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::Tests.TestHelper.CreateGroupConfigResponse[]);
            var __data__b__ = false;
            var __paging__ = default(Paging);
            var __paging__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __data__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.CreateGroupConfigResponse[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.PaginatedGroupConfig();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class TestHelper_CreateMemberRoleRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CreateMemberRoleRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CreateMemberRoleRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("memberRoleName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("memberRolePermissions"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("memberRoleName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("memberRolePermissions"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CreateMemberRoleRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.memberRoleName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<MemberRolePermission[]>().Serialize(ref writer, value.memberRolePermissions, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CreateMemberRoleRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __memberRoleName__ = default(string);
            var __memberRoleName__b__ = false;
            var __memberRolePermissions__ = default(MemberRolePermission[]);
            var __memberRolePermissions__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __memberRoleName__ = reader.ReadString();
                        __memberRoleName__b__ = true;
                        break;
                    case 1:
                        __memberRolePermissions__ = formatterResolver.GetFormatterWithVerify<MemberRolePermission[]>().Deserialize(ref reader, formatterResolver);
                        __memberRolePermissions__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CreateMemberRoleRequest();
            if(__memberRoleName__b__) ____result.memberRoleName = __memberRoleName__;
            if(__memberRolePermissions__b__) ____result.memberRolePermissions = __memberRolePermissions__;

            return ____result;
        }
    }


    public sealed class TestHelper_CreateMemberRoleResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CreateMemberRoleResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CreateMemberRoleResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("memberRoleId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("memberRoleName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("memberRolePermissions"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("memberRoleId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("memberRoleName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("memberRolePermissions"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CreateMemberRoleResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.memberRoleId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.memberRoleName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<MemberRolePermission[]>().Serialize(ref writer, value.memberRolePermissions, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CreateMemberRoleResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __memberRoleId__ = default(string);
            var __memberRoleId__b__ = false;
            var __memberRoleName__ = default(string);
            var __memberRoleName__b__ = false;
            var __memberRolePermissions__ = default(MemberRolePermission[]);
            var __memberRolePermissions__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __memberRoleId__ = reader.ReadString();
                        __memberRoleId__b__ = true;
                        break;
                    case 1:
                        __memberRoleName__ = reader.ReadString();
                        __memberRoleName__b__ = true;
                        break;
                    case 2:
                        __memberRolePermissions__ = formatterResolver.GetFormatterWithVerify<MemberRolePermission[]>().Deserialize(ref reader, formatterResolver);
                        __memberRolePermissions__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CreateMemberRoleResponse();
            if(__memberRoleId__b__) ____result.memberRoleId = __memberRoleId__;
            if(__memberRoleName__b__) ____result.memberRoleName = __memberRoleName__;
            if(__memberRolePermissions__b__) ____result.memberRolePermissions = __memberRolePermissions__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementBasePolicyCreateFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementBasePolicyCreate>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementBasePolicyCreateFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("typeId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("basePolicyName"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("affectedCountries"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("affectedClientIds"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isMandatory"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("typeId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("basePolicyName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("affectedCountries"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("affectedClientIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isMandatory"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementBasePolicyCreate value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.typeId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.basePolicyName);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.affectedCountries, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.affectedClientIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteBoolean(value.isMandatory);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementBasePolicyCreate Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __typeId__ = default(string);
            var __typeId__b__ = false;
            var __basePolicyName__ = default(string);
            var __basePolicyName__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __affectedCountries__ = default(string[]);
            var __affectedCountries__b__ = false;
            var __affectedClientIds__ = default(string[]);
            var __affectedClientIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __isMandatory__ = default(bool);
            var __isMandatory__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 1:
                        __typeId__ = reader.ReadString();
                        __typeId__b__ = true;
                        break;
                    case 2:
                        __basePolicyName__ = reader.ReadString();
                        __basePolicyName__b__ = true;
                        break;
                    case 3:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 4:
                        __affectedCountries__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __affectedCountries__b__ = true;
                        break;
                    case 5:
                        __affectedClientIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __affectedClientIds__b__ = true;
                        break;
                    case 6:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 7:
                        __isMandatory__ = reader.ReadBoolean();
                        __isMandatory__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementBasePolicyCreate();
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__typeId__b__) ____result.typeId = __typeId__;
            if(__basePolicyName__b__) ____result.basePolicyName = __basePolicyName__;
            if(__description__b__) ____result.description = __description__;
            if(__affectedCountries__b__) ____result.affectedCountries = __affectedCountries__;
            if(__affectedClientIds__b__) ____result.affectedClientIds = __affectedClientIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__isMandatory__b__) ____result.isMandatory = __isMandatory__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementPolicyTypeObjectFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementPolicyTypeObject>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementPolicyTypeObjectFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyTypeName"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyTypeName"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementPolicyTypeObject value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.policyTypeName);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementPolicyTypeObject Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __policyTypeName__ = default(string);
            var __policyTypeName__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __policyTypeName__ = reader.ReadString();
                        __policyTypeName__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementPolicyTypeObject();
            if(__id__b__) ____result.id = __id__;
            if(__policyTypeName__b__) ____result.policyTypeName = __policyTypeName__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementPolicyObjectFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementPolicyObject>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementPolicyObjectFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countryCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyName"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("countryCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyName"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementPolicyObject value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.countryCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.policyName);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementPolicyObject Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __countryCode__ = default(string);
            var __countryCode__b__ = false;
            var __policyName__ = default(string);
            var __policyName__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __countryCode__ = reader.ReadString();
                        __countryCode__b__ = true;
                        break;
                    case 2:
                        __policyName__ = reader.ReadString();
                        __policyName__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementPolicyObject();
            if(__id__b__) ____result.id = __id__;
            if(__countryCode__b__) ____result.countryCode = __countryCode__;
            if(__policyName__b__) ____result.policyName = __policyName__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementBasePolicyFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementBasePolicy>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementBasePolicyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("basePolicyName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policies"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("basePolicyName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policies"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementBasePolicy value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.basePolicyName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AgreementPolicyObject[]>().Serialize(ref writer, value.policies, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementBasePolicy Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __basePolicyName__ = default(string);
            var __basePolicyName__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __policies__ = default(global::Tests.TestHelper.AgreementPolicyObject[]);
            var __policies__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __basePolicyName__ = reader.ReadString();
                        __basePolicyName__b__ = true;
                        break;
                    case 2:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 3:
                        __policies__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AgreementPolicyObject[]>().Deserialize(ref reader, formatterResolver);
                        __policies__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementBasePolicy();
            if(__id__b__) ____result.id = __id__;
            if(__basePolicyName__b__) ____result.basePolicyName = __basePolicyName__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__policies__b__) ____result.policies = __policies__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementPolicyVersionCreateFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementPolicyVersionCreate>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementPolicyVersionCreateFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayVersion"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isCrucial"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isCommitted"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("displayVersion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isCrucial"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isCommitted"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementPolicyVersionCreate value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.displayVersion);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.isCrucial);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.isCommitted);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementPolicyVersionCreate Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __displayVersion__ = default(string);
            var __displayVersion__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __isCrucial__ = default(bool);
            var __isCrucial__b__ = false;
            var __isCommitted__ = default(bool);
            var __isCommitted__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __displayVersion__ = reader.ReadString();
                        __displayVersion__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __isCrucial__ = reader.ReadBoolean();
                        __isCrucial__b__ = true;
                        break;
                    case 3:
                        __isCommitted__ = reader.ReadBoolean();
                        __isCommitted__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementPolicyVersionCreate();
            if(__displayVersion__b__) ____result.displayVersion = __displayVersion__;
            if(__description__b__) ____result.description = __description__;
            if(__isCrucial__b__) ____result.isCrucial = __isCrucial__;
            if(__isCommitted__b__) ____result.isCommitted = __isCommitted__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementPolicyVersionFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementPolicyVersion>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementPolicyVersionFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayVersion"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("basePolicyId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isCrucial"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isInEffect"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayVersion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("basePolicyId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isCrucial"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isInEffect"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementPolicyVersion value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.displayVersion);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.basePolicyId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.isCrucial);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.isInEffect);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementPolicyVersion Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __displayVersion__ = default(string);
            var __displayVersion__b__ = false;
            var __basePolicyId__ = default(string);
            var __basePolicyId__b__ = false;
            var __isCrucial__ = default(bool);
            var __isCrucial__b__ = false;
            var __isInEffect__ = default(bool);
            var __isInEffect__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __displayVersion__ = reader.ReadString();
                        __displayVersion__b__ = true;
                        break;
                    case 2:
                        __basePolicyId__ = reader.ReadString();
                        __basePolicyId__b__ = true;
                        break;
                    case 3:
                        __isCrucial__ = reader.ReadBoolean();
                        __isCrucial__b__ = true;
                        break;
                    case 4:
                        __isInEffect__ = reader.ReadBoolean();
                        __isInEffect__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementPolicyVersion();
            if(__id__b__) ____result.id = __id__;
            if(__displayVersion__b__) ____result.displayVersion = __displayVersion__;
            if(__basePolicyId__b__) ____result.basePolicyId = __basePolicyId__;
            if(__isCrucial__b__) ____result.isCrucial = __isCrucial__;
            if(__isInEffect__b__) ____result.isInEffect = __isInEffect__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementCountryPolicyFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementCountryPolicy>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementCountryPolicyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countryCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyName"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isCrucial"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyVersions"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("countryCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isCrucial"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyVersions"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementCountryPolicy value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.countryCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.policyName);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.isCrucial);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AgreementPolicyVersion[]>().Serialize(ref writer, value.policyVersions, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementCountryPolicy Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __countryCode__ = default(string);
            var __countryCode__b__ = false;
            var __policyName__ = default(string);
            var __policyName__b__ = false;
            var __isCrucial__ = default(bool);
            var __isCrucial__b__ = false;
            var __policyVersions__ = default(global::Tests.TestHelper.AgreementPolicyVersion[]);
            var __policyVersions__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __countryCode__ = reader.ReadString();
                        __countryCode__b__ = true;
                        break;
                    case 2:
                        __policyName__ = reader.ReadString();
                        __policyName__b__ = true;
                        break;
                    case 3:
                        __isCrucial__ = reader.ReadBoolean();
                        __isCrucial__b__ = true;
                        break;
                    case 4:
                        __policyVersions__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AgreementPolicyVersion[]>().Deserialize(ref reader, formatterResolver);
                        __policyVersions__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementCountryPolicy();
            if(__id__b__) ____result.id = __id__;
            if(__countryCode__b__) ____result.countryCode = __countryCode__;
            if(__policyName__b__) ____result.policyName = __policyName__;
            if(__isCrucial__b__) ____result.isCrucial = __isCrucial__;
            if(__policyVersions__b__) ____result.policyVersions = __policyVersions__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementLocalizedPolicyCreateFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementLocalizedPolicyCreate>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementLocalizedPolicyCreateFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localeCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("contentType"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("localeCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("contentType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementLocalizedPolicyCreate value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.localeCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.contentType);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.description);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementLocalizedPolicyCreate Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __localeCode__ = default(string);
            var __localeCode__b__ = false;
            var __contentType__ = default(string);
            var __contentType__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __localeCode__ = reader.ReadString();
                        __localeCode__b__ = true;
                        break;
                    case 1:
                        __contentType__ = reader.ReadString();
                        __contentType__b__ = true;
                        break;
                    case 2:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementLocalizedPolicyCreate();
            if(__localeCode__b__) ____result.localeCode = __localeCode__;
            if(__contentType__b__) ____result.contentType = __contentType__;
            if(__description__b__) ____result.description = __description__;

            return ____result;
        }
    }


    public sealed class TestHelper_AgreementLocalizedPolicyFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AgreementLocalizedPolicy>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AgreementLocalizedPolicyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localeCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("contentType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attachmentLocation"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localeCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("contentType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("attachmentLocation"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AgreementLocalizedPolicy value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.localeCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.contentType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.attachmentLocation);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AgreementLocalizedPolicy Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __localeCode__ = default(string);
            var __localeCode__b__ = false;
            var __contentType__ = default(string);
            var __contentType__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __attachmentLocation__ = default(string);
            var __attachmentLocation__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __id__ = reader.ReadString();
                        __id__b__ = true;
                        break;
                    case 1:
                        __localeCode__ = reader.ReadString();
                        __localeCode__b__ = true;
                        break;
                    case 2:
                        __contentType__ = reader.ReadString();
                        __contentType__b__ = true;
                        break;
                    case 3:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 4:
                        __attachmentLocation__ = reader.ReadString();
                        __attachmentLocation__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AgreementLocalizedPolicy();
            if(__id__b__) ____result.id = __id__;
            if(__localeCode__b__) ____result.localeCode = __localeCode__;
            if(__contentType__b__) ____result.contentType = __contentType__;
            if(__description__b__) ____result.description = __description__;
            if(__attachmentLocation__b__) ____result.attachmentLocation = __attachmentLocation__;

            return ____result;
        }
    }


    public sealed class TestHelper_AdminCreateProfanityListRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AdminCreateProfanityListRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AdminCreateProfanityListRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isEnabled"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isMandatory"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("isEnabled"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isMandatory"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AdminCreateProfanityListRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.isEnabled);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteBoolean(value.isMandatory);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.name);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AdminCreateProfanityListRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __isEnabled__ = default(bool);
            var __isEnabled__b__ = false;
            var __isMandatory__ = default(bool);
            var __isMandatory__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __isEnabled__ = reader.ReadBoolean();
                        __isEnabled__b__ = true;
                        break;
                    case 1:
                        __isMandatory__ = reader.ReadBoolean();
                        __isMandatory__b__ = true;
                        break;
                    case 2:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AdminCreateProfanityListRequest();
            if(__isEnabled__b__) ____result.isEnabled = __isEnabled__;
            if(__isMandatory__b__) ____result.isMandatory = __isMandatory__;
            if(__name__b__) ____result.name = __name__;

            return ____result;
        }
    }


    public sealed class TestHelper_AdminAddProfanityFilterIntoListRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AdminAddProfanityFilterIntoListRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("filter"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("note"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("filter"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("note"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.filter);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.note);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __filter__ = default(string);
            var __filter__b__ = false;
            var __note__ = default(string);
            var __note__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __filter__ = reader.ReadString();
                        __filter__b__ = true;
                        break;
                    case 1:
                        __note__ = reader.ReadString();
                        __note__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest();
            if(__filter__b__) ____result.filter = __filter__;
            if(__note__b__) ____result.note = __note__;

            return ____result;
        }
    }


    public sealed class TestHelper_AdminAddProfanityFiltersRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AdminAddProfanityFiltersRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AdminAddProfanityFiltersRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("filters"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("filters"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AdminAddProfanityFiltersRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest[]>().Serialize(ref writer, value.filters, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AdminAddProfanityFiltersRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __filters__ = default(global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest[]);
            var __filters__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __filters__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.AdminAddProfanityFilterIntoListRequest[]>().Deserialize(ref reader, formatterResolver);
                        __filters__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AdminAddProfanityFiltersRequest();
            if(__filters__b__) ____result.filters = __filters__;

            return ____result;
        }
    }


    public sealed class TestHelper_AdminSetProfanityRuleForNamespaceRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.AdminSetProfanityRuleForNamespaceRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_AdminSetProfanityRuleForNamespaceRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rule"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("rule"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.AdminSetProfanityRuleForNamespaceRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.rule);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.AdminSetProfanityRuleForNamespaceRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __rule__ = default(string);
            var __rule__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __rule__ = reader.ReadString();
                        __rule__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.AdminSetProfanityRuleForNamespaceRequest();
            if(__rule__b__) ____result.rule = __rule__;

            return ____result;
        }
    }


    public sealed class TestHelper_FreeSubscritptionRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FreeSubscritptionRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FreeSubscritptionRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantDays"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("source"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reason"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grantDays"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("source"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FreeSubscritptionRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.grantDays);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.source);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.reason);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.language);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FreeSubscritptionRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __grantDays__ = default(int);
            var __grantDays__b__ = false;
            var __source__ = default(string);
            var __source__b__ = false;
            var __reason__ = default(string);
            var __reason__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 1:
                        __grantDays__ = reader.ReadInt32();
                        __grantDays__b__ = true;
                        break;
                    case 2:
                        __source__ = reader.ReadString();
                        __source__b__ = true;
                        break;
                    case 3:
                        __reason__ = reader.ReadString();
                        __reason__b__ = true;
                        break;
                    case 4:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 5:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FreeSubscritptionRequest();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__grantDays__b__) ____result.grantDays = __grantDays__;
            if(__source__b__) ____result.source = __source__;
            if(__reason__b__) ____result.reason = __reason__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;

            return ____result;
        }
    }


    public sealed class TestHelper_PodConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.PodConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_PodConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("cpu_limit"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("mem_limit"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("params"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("cpu_limit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mem_limit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("params"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.PodConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.cpu_limit);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.mem_limit);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.params_);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.PodConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __cpu_limit__ = default(int);
            var __cpu_limit__b__ = false;
            var __mem_limit__ = default(int);
            var __mem_limit__b__ = false;
            var __params___ = default(string);
            var __params___b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __cpu_limit__ = reader.ReadInt32();
                        __cpu_limit__b__ = true;
                        break;
                    case 1:
                        __mem_limit__ = reader.ReadInt32();
                        __mem_limit__b__ = true;
                        break;
                    case 2:
                        __params___ = reader.ReadString();
                        __params___b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.PodConfig();
            if(__cpu_limit__b__) ____result.cpu_limit = __cpu_limit__;
            if(__mem_limit__b__) ____result.mem_limit = __mem_limit__;
            if(__params___b__) ____result.params_ = __params___;

            return ____result;
        }
    }


    public sealed class TestHelper_DeploymentConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.DeploymentConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_DeploymentConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("buffer_count"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("configuration"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_version"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("max_count"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("min_count"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regions"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("buffer_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("configuration"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("game_version"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("max_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("min_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regions"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.DeploymentConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.buffer_count);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.configuration);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.game_version);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.max_count);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.min_count);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.regions, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.DeploymentConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __buffer_count__ = default(int);
            var __buffer_count__b__ = false;
            var __configuration__ = default(string);
            var __configuration__b__ = false;
            var __game_version__ = default(string);
            var __game_version__b__ = false;
            var __max_count__ = default(int);
            var __max_count__b__ = false;
            var __min_count__ = default(int);
            var __min_count__b__ = false;
            var __regions__ = default(string[]);
            var __regions__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __buffer_count__ = reader.ReadInt32();
                        __buffer_count__b__ = true;
                        break;
                    case 1:
                        __configuration__ = reader.ReadString();
                        __configuration__b__ = true;
                        break;
                    case 2:
                        __game_version__ = reader.ReadString();
                        __game_version__b__ = true;
                        break;
                    case 3:
                        __max_count__ = reader.ReadInt32();
                        __max_count__b__ = true;
                        break;
                    case 4:
                        __min_count__ = reader.ReadInt32();
                        __min_count__b__ = true;
                        break;
                    case 5:
                        __regions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __regions__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.DeploymentConfig();
            if(__buffer_count__b__) ____result.buffer_count = __buffer_count__;
            if(__configuration__b__) ____result.configuration = __configuration__;
            if(__game_version__b__) ____result.game_version = __game_version__;
            if(__max_count__b__) ____result.max_count = __max_count__;
            if(__min_count__b__) ____result.min_count = __min_count__;
            if(__regions__b__) ____result.regions = __regions__;

            return ____result;
        }
    }


    public sealed class TestHelper_DeploymentWithOverrideFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.DeploymentWithOverride>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_DeploymentWithOverrideFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("allow_version_override"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("buffer_count"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("configuration"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_version"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("max_count"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("min_count"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("overrides"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regions"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("allow_version_override"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("buffer_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("configuration"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("game_version"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("max_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("min_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("overrides"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regions"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.DeploymentWithOverride value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.allow_version_override);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.buffer_count);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.configuration);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.game_version);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.max_count);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.min_count);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>>().Serialize(ref writer, value.overrides, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.regions, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.DeploymentWithOverride Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __allow_version_override__ = default(bool);
            var __allow_version_override__b__ = false;
            var __buffer_count__ = default(int);
            var __buffer_count__b__ = false;
            var __configuration__ = default(string);
            var __configuration__b__ = false;
            var __game_version__ = default(string);
            var __game_version__b__ = false;
            var __max_count__ = default(int);
            var __max_count__b__ = false;
            var __min_count__ = default(int);
            var __min_count__b__ = false;
            var __overrides__ = default(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>);
            var __overrides__b__ = false;
            var __regions__ = default(string[]);
            var __regions__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __allow_version_override__ = reader.ReadBoolean();
                        __allow_version_override__b__ = true;
                        break;
                    case 1:
                        __buffer_count__ = reader.ReadInt32();
                        __buffer_count__b__ = true;
                        break;
                    case 2:
                        __configuration__ = reader.ReadString();
                        __configuration__b__ = true;
                        break;
                    case 3:
                        __game_version__ = reader.ReadString();
                        __game_version__b__ = true;
                        break;
                    case 4:
                        __max_count__ = reader.ReadInt32();
                        __max_count__b__ = true;
                        break;
                    case 5:
                        __min_count__ = reader.ReadInt32();
                        __min_count__b__ = true;
                        break;
                    case 6:
                        __overrides__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>>().Deserialize(ref reader, formatterResolver);
                        __overrides__b__ = true;
                        break;
                    case 7:
                        __regions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __regions__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.DeploymentWithOverride();
            if(__allow_version_override__b__) ____result.allow_version_override = __allow_version_override__;
            if(__buffer_count__b__) ____result.buffer_count = __buffer_count__;
            if(__configuration__b__) ____result.configuration = __configuration__;
            if(__game_version__b__) ____result.game_version = __game_version__;
            if(__max_count__b__) ____result.max_count = __max_count__;
            if(__min_count__b__) ____result.min_count = __min_count__;
            if(__overrides__b__) ____result.overrides = __overrides__;
            if(__regions__b__) ____result.regions = __regions__;

            return ____result;
        }
    }


    public sealed class TestHelper_DSMConfigFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.DSMConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_DSMConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("allow_version_override"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("buffer_count"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("claim_timeout"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("configurations"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("cpu_limit"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("mem_limit"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("params"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("creation_timeout"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("default_version"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("deployments"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("heartbeat_timeout"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("image_version_mapping"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("max_count"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("min_count"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("overrides"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ports"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("protocol"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("providers"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("session_timeout"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("unreachable_timeout"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("version_image_size_mapping"), 22},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("allow_version_override"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("buffer_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("claim_timeout"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("configurations"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("cpu_limit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mem_limit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("params"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("creation_timeout"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default_version"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("deployments"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("heartbeat_timeout"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("image_version_mapping"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("max_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("min_count"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("overrides"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ports"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("protocol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("providers"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("session_timeout"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("unreachable_timeout"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("version_image_size_mapping"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.DSMConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteBoolean(value.allow_version_override);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.buffer_count);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.claim_timeout);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.PodConfig>>().Serialize(ref writer, value.configurations, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.cpu_limit);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.mem_limit);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.params_);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.creation_timeout);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.default_version);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentWithOverride>>().Serialize(ref writer, value.deployments, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteInt32(value.heartbeat_timeout);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.image_version_mapping, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteInt32(value.max_count);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteInt32(value.min_count);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>>().Serialize(ref writer, value.overrides, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteInt32(value.port);
            writer.WriteRaw(this.____stringByteKeys[17]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Serialize(ref writer, value.ports, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.protocol);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.providers, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteInt32(value.session_timeout);
            writer.WriteRaw(this.____stringByteKeys[21]);
            writer.WriteInt32(value.unreachable_timeout);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Serialize(ref writer, value.version_image_size_mapping, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.DSMConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __allow_version_override__ = default(bool);
            var __allow_version_override__b__ = false;
            var __buffer_count__ = default(int);
            var __buffer_count__b__ = false;
            var __claim_timeout__ = default(int);
            var __claim_timeout__b__ = false;
            var __configurations__ = default(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.PodConfig>);
            var __configurations__b__ = false;
            var __cpu_limit__ = default(int);
            var __cpu_limit__b__ = false;
            var __mem_limit__ = default(int);
            var __mem_limit__b__ = false;
            var __params___ = default(string);
            var __params___b__ = false;
            var __creation_timeout__ = default(int);
            var __creation_timeout__b__ = false;
            var __default_version__ = default(string);
            var __default_version__b__ = false;
            var __deployments__ = default(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentWithOverride>);
            var __deployments__b__ = false;
            var __heartbeat_timeout__ = default(int);
            var __heartbeat_timeout__b__ = false;
            var __image_version_mapping__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __image_version_mapping__b__ = false;
            var __max_count__ = default(int);
            var __max_count__b__ = false;
            var __min_count__ = default(int);
            var __min_count__b__ = false;
            var __overrides__ = default(global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>);
            var __overrides__b__ = false;
            var __port__ = default(int);
            var __port__b__ = false;
            var __ports__ = default(global::System.Collections.Generic.Dictionary<string, int>);
            var __ports__b__ = false;
            var __protocol__ = default(string);
            var __protocol__b__ = false;
            var __providers__ = default(string[]);
            var __providers__b__ = false;
            var __session_timeout__ = default(int);
            var __session_timeout__b__ = false;
            var __unreachable_timeout__ = default(int);
            var __unreachable_timeout__b__ = false;
            var __version_image_size_mapping__ = default(global::System.Collections.Generic.Dictionary<string, int>);
            var __version_image_size_mapping__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 1:
                        __allow_version_override__ = reader.ReadBoolean();
                        __allow_version_override__b__ = true;
                        break;
                    case 2:
                        __buffer_count__ = reader.ReadInt32();
                        __buffer_count__b__ = true;
                        break;
                    case 3:
                        __claim_timeout__ = reader.ReadInt32();
                        __claim_timeout__b__ = true;
                        break;
                    case 4:
                        __configurations__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.PodConfig>>().Deserialize(ref reader, formatterResolver);
                        __configurations__b__ = true;
                        break;
                    case 5:
                        __cpu_limit__ = reader.ReadInt32();
                        __cpu_limit__b__ = true;
                        break;
                    case 6:
                        __mem_limit__ = reader.ReadInt32();
                        __mem_limit__b__ = true;
                        break;
                    case 7:
                        __params___ = reader.ReadString();
                        __params___b__ = true;
                        break;
                    case 8:
                        __creation_timeout__ = reader.ReadInt32();
                        __creation_timeout__b__ = true;
                        break;
                    case 9:
                        __default_version__ = reader.ReadString();
                        __default_version__b__ = true;
                        break;
                    case 10:
                        __deployments__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentWithOverride>>().Deserialize(ref reader, formatterResolver);
                        __deployments__b__ = true;
                        break;
                    case 11:
                        __heartbeat_timeout__ = reader.ReadInt32();
                        __heartbeat_timeout__b__ = true;
                        break;
                    case 12:
                        __image_version_mapping__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __image_version_mapping__b__ = true;
                        break;
                    case 13:
                        __max_count__ = reader.ReadInt32();
                        __max_count__b__ = true;
                        break;
                    case 14:
                        __min_count__ = reader.ReadInt32();
                        __min_count__b__ = true;
                        break;
                    case 15:
                        __overrides__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Tests.TestHelper.DeploymentConfig>>().Deserialize(ref reader, formatterResolver);
                        __overrides__b__ = true;
                        break;
                    case 16:
                        __port__ = reader.ReadInt32();
                        __port__b__ = true;
                        break;
                    case 17:
                        __ports__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Deserialize(ref reader, formatterResolver);
                        __ports__b__ = true;
                        break;
                    case 18:
                        __protocol__ = reader.ReadString();
                        __protocol__b__ = true;
                        break;
                    case 19:
                        __providers__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __providers__b__ = true;
                        break;
                    case 20:
                        __session_timeout__ = reader.ReadInt32();
                        __session_timeout__b__ = true;
                        break;
                    case 21:
                        __unreachable_timeout__ = reader.ReadInt32();
                        __unreachable_timeout__b__ = true;
                        break;
                    case 22:
                        __version_image_size_mapping__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Deserialize(ref reader, formatterResolver);
                        __version_image_size_mapping__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.DSMConfig();
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__allow_version_override__b__) ____result.allow_version_override = __allow_version_override__;
            if(__buffer_count__b__) ____result.buffer_count = __buffer_count__;
            if(__claim_timeout__b__) ____result.claim_timeout = __claim_timeout__;
            if(__configurations__b__) ____result.configurations = __configurations__;
            if(__cpu_limit__b__) ____result.cpu_limit = __cpu_limit__;
            if(__mem_limit__b__) ____result.mem_limit = __mem_limit__;
            if(__params___b__) ____result.params_ = __params___;
            if(__creation_timeout__b__) ____result.creation_timeout = __creation_timeout__;
            if(__default_version__b__) ____result.default_version = __default_version__;
            if(__deployments__b__) ____result.deployments = __deployments__;
            if(__heartbeat_timeout__b__) ____result.heartbeat_timeout = __heartbeat_timeout__;
            if(__image_version_mapping__b__) ____result.image_version_mapping = __image_version_mapping__;
            if(__max_count__b__) ____result.max_count = __max_count__;
            if(__min_count__b__) ____result.min_count = __min_count__;
            if(__overrides__b__) ____result.overrides = __overrides__;
            if(__port__b__) ____result.port = __port__;
            if(__ports__b__) ____result.ports = __ports__;
            if(__protocol__b__) ____result.protocol = __protocol__;
            if(__providers__b__) ____result.providers = __providers__;
            if(__session_timeout__b__) ____result.session_timeout = __session_timeout__;
            if(__unreachable_timeout__b__) ____result.unreachable_timeout = __unreachable_timeout__;
            if(__version_image_size_mapping__b__) ____result.version_image_size_mapping = __version_image_size_mapping__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace AccelByte.Models.Formatters.AccelByte.Models
{
    using System;
    using Utf8Json;


    public sealed class StatCreateRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatCreateRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatCreateRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultValue"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incrementOnly"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maximum"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("minimum"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("setAsGlobal"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("setBy"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("defaultValue"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("incrementOnly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maximum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minimum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("setAsGlobal"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("setBy"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatCreateRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.defaultValue);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.incrementOnly);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteSingle(value.maximum);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteSingle(value.minimum);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.setAsGlobal);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<StatisticSetBy>().Serialize(ref writer, value.setBy, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatCreateRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __defaultValue__ = default(float);
            var __defaultValue__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __incrementOnly__ = default(bool);
            var __incrementOnly__b__ = false;
            var __maximum__ = default(float);
            var __maximum__b__ = false;
            var __minimum__ = default(float);
            var __minimum__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __setAsGlobal__ = default(bool);
            var __setAsGlobal__b__ = false;
            var __setBy__ = default(StatisticSetBy);
            var __setBy__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __defaultValue__ = reader.ReadSingle();
                        __defaultValue__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __incrementOnly__ = reader.ReadBoolean();
                        __incrementOnly__b__ = true;
                        break;
                    case 3:
                        __maximum__ = reader.ReadSingle();
                        __maximum__b__ = true;
                        break;
                    case 4:
                        __minimum__ = reader.ReadSingle();
                        __minimum__b__ = true;
                        break;
                    case 5:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 6:
                        __setAsGlobal__ = reader.ReadBoolean();
                        __setAsGlobal__b__ = true;
                        break;
                    case 7:
                        __setBy__ = formatterResolver.GetFormatterWithVerify<StatisticSetBy>().Deserialize(ref reader, formatterResolver);
                        __setBy__b__ = true;
                        break;
                    case 8:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 9:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StatCreateRequest();
            if(__defaultValue__b__) ____result.defaultValue = __defaultValue__;
            if(__description__b__) ____result.description = __description__;
            if(__incrementOnly__b__) ____result.incrementOnly = __incrementOnly__;
            if(__maximum__b__) ____result.maximum = __maximum__;
            if(__minimum__b__) ____result.minimum = __minimum__;
            if(__name__b__) ____result.name = __name__;
            if(__setAsGlobal__b__) ____result.setAsGlobal = __setAsGlobal__;
            if(__setBy__b__) ____result.setBy = __setBy__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__tags__b__) ____result.tags = __tags__;

            return ____result;
        }
    }


    public sealed class StatCodeFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatCode>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatCodeFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatCode value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatCode Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __statCode__ = default(string);
            var __statCode__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StatCode();
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class LeaderboardDailyConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LeaderboardDailyConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LeaderboardDailyConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetTime"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("resetTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LeaderboardDailyConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.resetTime);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LeaderboardDailyConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __resetTime__ = default(string);
            var __resetTime__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __resetTime__ = reader.ReadString();
                        __resetTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LeaderboardDailyConfig();
            if(__resetTime__b__) ____result.resetTime = __resetTime__;

            return ____result;
        }
    }


    public sealed class LeaderboardMonthlyConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LeaderboardMonthlyConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LeaderboardMonthlyConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetDate"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetTime"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("resetDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("resetTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LeaderboardMonthlyConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.resetDate);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.resetTime);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LeaderboardMonthlyConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __resetDate__ = default(int);
            var __resetDate__b__ = false;
            var __resetTime__ = default(string);
            var __resetTime__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __resetDate__ = reader.ReadInt32();
                        __resetDate__b__ = true;
                        break;
                    case 1:
                        __resetTime__ = reader.ReadString();
                        __resetTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LeaderboardMonthlyConfig();
            if(__resetDate__b__) ____result.resetDate = __resetDate__;
            if(__resetTime__b__) ____result.resetTime = __resetTime__;

            return ____result;
        }
    }


    public sealed class LeaderboardWeeklyConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LeaderboardWeeklyConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LeaderboardWeeklyConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetDay"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resetTime"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("resetDay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("resetTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LeaderboardWeeklyConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.resetDay);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.resetTime);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LeaderboardWeeklyConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __resetDay__ = default(int);
            var __resetDay__b__ = false;
            var __resetTime__ = default(string);
            var __resetTime__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __resetDay__ = reader.ReadInt32();
                        __resetDay__b__ = true;
                        break;
                    case 1:
                        __resetTime__ = reader.ReadString();
                        __resetTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LeaderboardWeeklyConfig();
            if(__resetDay__b__) ____result.resetDay = __resetDay__;
            if(__resetTime__b__) ____result.resetTime = __resetTime__;

            return ____result;
        }
    }


    public sealed class LeaderboardConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LeaderboardConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LeaderboardConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("daily"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("monthly"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("weekly"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("descending"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("iconURL"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("leaderboardCode"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("seasonPeriod"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("startTime"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("daily"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("monthly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("weekly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("descending"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("iconURL"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("leaderboardCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("seasonPeriod"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("startTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LeaderboardConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LeaderboardDailyConfig>().Serialize(ref writer, value.daily, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LeaderboardMonthlyConfig>().Serialize(ref writer, value.monthly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LeaderboardWeeklyConfig>().Serialize(ref writer, value.weekly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.descending);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.iconURL);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.leaderboardCode);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.seasonPeriod);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.startTime);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LeaderboardConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __daily__ = default(global::AccelByte.Models.LeaderboardDailyConfig);
            var __daily__b__ = false;
            var __monthly__ = default(global::AccelByte.Models.LeaderboardMonthlyConfig);
            var __monthly__b__ = false;
            var __weekly__ = default(global::AccelByte.Models.LeaderboardWeeklyConfig);
            var __weekly__b__ = false;
            var __descending__ = default(bool);
            var __descending__b__ = false;
            var __iconURL__ = default(string);
            var __iconURL__b__ = false;
            var __leaderboardCode__ = default(string);
            var __leaderboardCode__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __seasonPeriod__ = default(int);
            var __seasonPeriod__b__ = false;
            var __startTime__ = default(string);
            var __startTime__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __daily__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LeaderboardDailyConfig>().Deserialize(ref reader, formatterResolver);
                        __daily__b__ = true;
                        break;
                    case 1:
                        __monthly__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LeaderboardMonthlyConfig>().Deserialize(ref reader, formatterResolver);
                        __monthly__b__ = true;
                        break;
                    case 2:
                        __weekly__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LeaderboardWeeklyConfig>().Deserialize(ref reader, formatterResolver);
                        __weekly__b__ = true;
                        break;
                    case 3:
                        __descending__ = reader.ReadBoolean();
                        __descending__b__ = true;
                        break;
                    case 4:
                        __iconURL__ = reader.ReadString();
                        __iconURL__b__ = true;
                        break;
                    case 5:
                        __leaderboardCode__ = reader.ReadString();
                        __leaderboardCode__b__ = true;
                        break;
                    case 6:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 7:
                        __seasonPeriod__ = reader.ReadInt32();
                        __seasonPeriod__b__ = true;
                        break;
                    case 8:
                        __startTime__ = reader.ReadString();
                        __startTime__b__ = true;
                        break;
                    case 9:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LeaderboardConfig();
            if(__daily__b__) ____result.daily = __daily__;
            if(__monthly__b__) ____result.monthly = __monthly__;
            if(__weekly__b__) ____result.weekly = __weekly__;
            if(__descending__b__) ____result.descending = __descending__;
            if(__iconURL__b__) ____result.iconURL = __iconURL__;
            if(__leaderboardCode__b__) ____result.leaderboardCode = __leaderboardCode__;
            if(__name__b__) ____result.name = __name__;
            if(__seasonPeriod__b__) ____result.seasonPeriod = __seasonPeriod__;
            if(__startTime__b__) ____result.startTime = __startTime__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class SetUserVisibilityRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.SetUserVisibilityRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SetUserVisibilityRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("visibility"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("visibility"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.SetUserVisibilityRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.visibility);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.SetUserVisibilityRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __visibility__ = default(bool);
            var __visibility__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __visibility__ = reader.ReadBoolean();
                        __visibility__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.SetUserVisibilityRequest();
            if(__visibility__b__) ____result.visibility = __visibility__;

            return ____result;
        }
    }


    public sealed class AllianceRuleFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AllianceRule>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AllianceRuleFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("max_number"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("min_number"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("player_max_number"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("player_min_number"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("max_number"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("min_number"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("player_max_number"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("player_min_number"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AllianceRule value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.max_number);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.min_number);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.player_max_number);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.player_min_number);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AllianceRule Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __max_number__ = default(int);
            var __max_number__b__ = false;
            var __min_number__ = default(int);
            var __min_number__b__ = false;
            var __player_max_number__ = default(int);
            var __player_max_number__b__ = false;
            var __player_min_number__ = default(int);
            var __player_min_number__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __max_number__ = reader.ReadInt32();
                        __max_number__b__ = true;
                        break;
                    case 1:
                        __min_number__ = reader.ReadInt32();
                        __min_number__b__ = true;
                        break;
                    case 2:
                        __player_max_number__ = reader.ReadInt32();
                        __player_max_number__b__ = true;
                        break;
                    case 3:
                        __player_min_number__ = reader.ReadInt32();
                        __player_min_number__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AllianceRule();
            if(__max_number__b__) ____result.max_number = __max_number__;
            if(__min_number__b__) ____result.min_number = __min_number__;
            if(__player_max_number__b__) ____result.player_max_number = __player_max_number__;
            if(__player_min_number__b__) ____result.player_min_number = __player_min_number__;

            return ____result;
        }
    }


    public sealed class FlexingRuleFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.FlexingRule>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FlexingRuleFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attribute"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("criteria"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("duration"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reference"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("attribute"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("criteria"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("duration"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reference"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.FlexingRule value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.attribute);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.criteria);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.duration);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.reference);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.FlexingRule Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __attribute__ = default(string);
            var __attribute__b__ = false;
            var __criteria__ = default(string);
            var __criteria__b__ = false;
            var __duration__ = default(int);
            var __duration__b__ = false;
            var __reference__ = default(int);
            var __reference__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __attribute__ = reader.ReadString();
                        __attribute__b__ = true;
                        break;
                    case 1:
                        __criteria__ = reader.ReadString();
                        __criteria__b__ = true;
                        break;
                    case 2:
                        __duration__ = reader.ReadInt32();
                        __duration__b__ = true;
                        break;
                    case 3:
                        __reference__ = reader.ReadInt32();
                        __reference__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.FlexingRule();
            if(__attribute__b__) ____result.attribute = __attribute__;
            if(__criteria__b__) ____result.criteria = __criteria__;
            if(__duration__b__) ____result.duration = __duration__;
            if(__reference__b__) ____result.reference = __reference__;

            return ____result;
        }
    }


    public sealed class MatchingRuleFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MatchingRule>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchingRuleFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attribute"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("criteria"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reference"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("attribute"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("criteria"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reference"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MatchingRule value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.attribute);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.criteria);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.reference);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MatchingRule Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __attribute__ = default(string);
            var __attribute__b__ = false;
            var __criteria__ = default(string);
            var __criteria__b__ = false;
            var __reference__ = default(int);
            var __reference__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __attribute__ = reader.ReadString();
                        __attribute__b__ = true;
                        break;
                    case 1:
                        __criteria__ = reader.ReadString();
                        __criteria__b__ = true;
                        break;
                    case 2:
                        __reference__ = reader.ReadInt32();
                        __reference__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MatchingRule();
            if(__attribute__b__) ____result.attribute = __attribute__;
            if(__criteria__b__) ____result.criteria = __criteria__;
            if(__reference__b__) ____result.reference = __reference__;

            return ____result;
        }
    }


    public sealed class RuleSetFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RuleSet>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RuleSetFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("alliance"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("flexing_rule"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matching_rule"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("alliance"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("flexing_rule"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("matching_rule"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RuleSet value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AllianceRule>().Serialize(ref writer, value.alliance, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.FlexingRule[]>().Serialize(ref writer, value.flexing_rule, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.MatchingRule[]>().Serialize(ref writer, value.matching_rule, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RuleSet Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __alliance__ = default(global::AccelByte.Models.AllianceRule);
            var __alliance__b__ = false;
            var __flexing_rule__ = default(global::AccelByte.Models.FlexingRule[]);
            var __flexing_rule__b__ = false;
            var __matching_rule__ = default(global::AccelByte.Models.MatchingRule[]);
            var __matching_rule__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __alliance__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AllianceRule>().Deserialize(ref reader, formatterResolver);
                        __alliance__b__ = true;
                        break;
                    case 1:
                        __flexing_rule__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.FlexingRule[]>().Deserialize(ref reader, formatterResolver);
                        __flexing_rule__b__ = true;
                        break;
                    case 2:
                        __matching_rule__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.MatchingRule[]>().Deserialize(ref reader, formatterResolver);
                        __matching_rule__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RuleSet();
            if(__alliance__b__) ____result.alliance = __alliance__;
            if(__flexing_rule__b__) ____result.flexing_rule = __flexing_rule__;
            if(__matching_rule__b__) ____result.matching_rule = __matching_rule__;

            return ____result;
        }
    }


    public sealed class CreateChannelRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CreateChannelRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreateChannelRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("find_match_timeout_seconds"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_mode"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rule_set"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("joinable"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("find_match_timeout_seconds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("game_mode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("rule_set"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("joinable"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CreateChannelRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt32(value.find_match_timeout_seconds);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.game_mode);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RuleSet>().Serialize(ref writer, value.rule_set, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.joinable);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CreateChannelRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __description__ = default(string);
            var __description__b__ = false;
            var __find_match_timeout_seconds__ = default(uint);
            var __find_match_timeout_seconds__b__ = false;
            var __game_mode__ = default(string);
            var __game_mode__b__ = false;
            var __rule_set__ = default(global::AccelByte.Models.RuleSet);
            var __rule_set__b__ = false;
            var __joinable__ = default(bool);
            var __joinable__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 1:
                        __find_match_timeout_seconds__ = reader.ReadUInt32();
                        __find_match_timeout_seconds__b__ = true;
                        break;
                    case 2:
                        __game_mode__ = reader.ReadString();
                        __game_mode__b__ = true;
                        break;
                    case 3:
                        __rule_set__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RuleSet>().Deserialize(ref reader, formatterResolver);
                        __rule_set__b__ = true;
                        break;
                    case 4:
                        __joinable__ = reader.ReadBoolean();
                        __joinable__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.CreateChannelRequest();
            if(__description__b__) ____result.description = __description__;
            if(__find_match_timeout_seconds__b__) ____result.find_match_timeout_seconds = __find_match_timeout_seconds__;
            if(__game_mode__b__) ____result.game_mode = __game_mode__;
            if(__rule_set__b__) ____result.rule_set = __rule_set__;
            if(__joinable__b__) ____result.joinable = __joinable__;

            return ____result;
        }
    }


    public sealed class CreateChannelResponseFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CreateChannelResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreateChannelResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("find_match_timeout_seconds"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_mode"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rule_set"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("joinable"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("deployment"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("slug"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("social_matchmaking"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("session_queue_timeout_seconds"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updated_at"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("find_match_timeout_seconds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("game_mode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("rule_set"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("joinable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("deployment"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("slug"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("social_matchmaking"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("session_queue_timeout_seconds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updated_at"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CreateChannelResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt32(value.find_match_timeout_seconds);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.game_mode);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RuleSet>().Serialize(ref writer, value.rule_set, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.joinable);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.deployment);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.slug);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteBoolean(value.social_matchmaking);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteUInt32(value.session_queue_timeout_seconds);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updated_at, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CreateChannelResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __description__ = default(string);
            var __description__b__ = false;
            var __find_match_timeout_seconds__ = default(uint);
            var __find_match_timeout_seconds__b__ = false;
            var __game_mode__ = default(string);
            var __game_mode__b__ = false;
            var __rule_set__ = default(global::AccelByte.Models.RuleSet);
            var __rule_set__b__ = false;
            var __joinable__ = default(bool);
            var __joinable__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __deployment__ = default(string);
            var __deployment__b__ = false;
            var __slug__ = default(string);
            var __slug__b__ = false;
            var __social_matchmaking__ = default(bool);
            var __social_matchmaking__b__ = false;
            var __session_queue_timeout_seconds__ = default(uint);
            var __session_queue_timeout_seconds__b__ = false;
            var __updated_at__ = default(global::System.DateTime);
            var __updated_at__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 1:
                        __find_match_timeout_seconds__ = reader.ReadUInt32();
                        __find_match_timeout_seconds__b__ = true;
                        break;
                    case 2:
                        __game_mode__ = reader.ReadString();
                        __game_mode__b__ = true;
                        break;
                    case 3:
                        __rule_set__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RuleSet>().Deserialize(ref reader, formatterResolver);
                        __rule_set__b__ = true;
                        break;
                    case 4:
                        __joinable__ = reader.ReadBoolean();
                        __joinable__b__ = true;
                        break;
                    case 5:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 6:
                        __deployment__ = reader.ReadString();
                        __deployment__b__ = true;
                        break;
                    case 7:
                        __slug__ = reader.ReadString();
                        __slug__b__ = true;
                        break;
                    case 8:
                        __social_matchmaking__ = reader.ReadBoolean();
                        __social_matchmaking__b__ = true;
                        break;
                    case 9:
                        __session_queue_timeout_seconds__ = reader.ReadUInt32();
                        __session_queue_timeout_seconds__b__ = true;
                        break;
                    case 10:
                        __updated_at__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updated_at__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.CreateChannelResponse();
            if(__description__b__) ____result.description = __description__;
            if(__find_match_timeout_seconds__b__) ____result.find_match_timeout_seconds = __find_match_timeout_seconds__;
            if(__game_mode__b__) ____result.game_mode = __game_mode__;
            if(__rule_set__b__) ____result.rule_set = __rule_set__;
            if(__joinable__b__) ____result.joinable = __joinable__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__deployment__b__) ____result.deployment = __deployment__;
            if(__slug__b__) ____result.slug = __slug__;
            if(__social_matchmaking__b__) ____result.social_matchmaking = __social_matchmaking__;
            if(__session_queue_timeout_seconds__b__) ____result.session_queue_timeout_seconds = __session_queue_timeout_seconds__;
            if(__updated_at__b__) ____result.updated_at = __updated_at__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace AccelByte.Models.Formatters.Tests.IntegrationTests
{
    using System;
    using Utf8Json;


    public sealed class MatchmakingRequestFormatter : global::Utf8Json.IJsonFormatter<global::Tests.IntegrationTests.MatchmakingRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchmakingRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ChannelName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ServerName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientVersion"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PreferredLatencies"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PartyAttributes"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("TempPartyMembers"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ExtraAttributes"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ChannelName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ServerName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ClientVersion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PreferredLatencies"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PartyAttributes"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("TempPartyMembers"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ExtraAttributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.IntegrationTests.MatchmakingRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ChannelName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.ServerName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.ClientVersion);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Serialize(ref writer, value.PreferredLatencies, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.PartyAttributes, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.TempPartyMembers, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.ExtraAttributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.IntegrationTests.MatchmakingRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __ChannelName__ = default(string);
            var __ChannelName__b__ = false;
            var __ServerName__ = default(string);
            var __ServerName__b__ = false;
            var __ClientVersion__ = default(string);
            var __ClientVersion__b__ = false;
            var __PreferredLatencies__ = default(global::System.Collections.Generic.Dictionary<string, int>);
            var __PreferredLatencies__b__ = false;
            var __PartyAttributes__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __PartyAttributes__b__ = false;
            var __TempPartyMembers__ = default(string[]);
            var __TempPartyMembers__b__ = false;
            var __ExtraAttributes__ = default(string[]);
            var __ExtraAttributes__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __ChannelName__ = reader.ReadString();
                        __ChannelName__b__ = true;
                        break;
                    case 1:
                        __ServerName__ = reader.ReadString();
                        __ServerName__b__ = true;
                        break;
                    case 2:
                        __ClientVersion__ = reader.ReadString();
                        __ClientVersion__b__ = true;
                        break;
                    case 3:
                        __PreferredLatencies__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Deserialize(ref reader, formatterResolver);
                        __PreferredLatencies__b__ = true;
                        break;
                    case 4:
                        __PartyAttributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __PartyAttributes__b__ = true;
                        break;
                    case 5:
                        __TempPartyMembers__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __TempPartyMembers__b__ = true;
                        break;
                    case 6:
                        __ExtraAttributes__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __ExtraAttributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.IntegrationTests.MatchmakingRequest();
            if(__ChannelName__b__) ____result.ChannelName = __ChannelName__;
            if(__ServerName__b__) ____result.ServerName = __ServerName__;
            if(__ClientVersion__b__) ____result.ClientVersion = __ClientVersion__;
            if(__PreferredLatencies__b__) ____result.PreferredLatencies = __PreferredLatencies__;
            if(__PartyAttributes__b__) ____result.PartyAttributes = __PartyAttributes__;
            if(__TempPartyMembers__b__) ____result.TempPartyMembers = __TempPartyMembers__;
            if(__ExtraAttributes__b__) ____result.ExtraAttributes = __ExtraAttributes__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
