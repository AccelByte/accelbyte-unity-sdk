#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace Utf8Json.Resolvers
{
    using System;
    using Utf8Json;

    public class GeneratedResolver : global::Utf8Json.IJsonFormatterResolver
    {
        public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
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
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::Utf8Json.IJsonFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(168)
            {
                {typeof(global::AccelByte.Models.AchievementIcon[]), 0 },
                {typeof(global::AccelByte.Models.PublicAchievement[]), 1 },
                {typeof(global::System.Collections.Generic.Dictionary<string, string>), 2 },
                {typeof(global::AccelByte.Models.UserAchievement[]), 3 },
                {typeof(global::AccelByte.Models.LocalizedPolicyVersionObject[]), 4 },
                {typeof(global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]), 5 },
                {typeof(global::System.Collections.Generic.Dictionary<string, object>), 6 },
                {typeof(global::AccelByte.Models.WalletTransactionInfo[]), 7 },
                {typeof(global::AccelByte.Models.Image[]), 8 },
                {typeof(global::AccelByte.Models.RegionDataItem[]), 9 },
                {typeof(global::AccelByte.Models.ItemInfo[]), 10 },
                {typeof(global::AccelByte.Models.OrderInfo[]), 11 },
                {typeof(global::AccelByte.Models.EntitlementInfo[]), 12 },
                {typeof(global::AccelByte.Models.GameProfilePublicInfo[]), 13 },
                {typeof(global::AccelByte.Models.UserPoint[]), 14 },
                {typeof(global::AccelByte.Models.Slot[]), 15 },
                {typeof(global::AccelByte.Models.UserGameProfiles[]), 16 },
                {typeof(global::AccelByte.Models.GameProfile[]), 17 },
                {typeof(global::AccelByte.Models.OrderHistoryInfo[]), 18 },
                {typeof(global::AccelByte.Models.PlatformLink[]), 19 },
                {typeof(global::AccelByte.Models.PublicUserProfile[]), 20 },
                {typeof(global::AccelByte.Models.QosServer[]), 21 },
                {typeof(global::AccelByte.Models.PartyMember[]), 22 },
                {typeof(global::AccelByte.Models.MatchParty[]), 23 },
                {typeof(global::AccelByte.Models.MatchingAlly[]), 24 },
                {typeof(global::AccelByte.Models.StatItem[]), 25 },
                {typeof(global::AccelByte.Models.Ban[]), 26 },
                {typeof(global::AccelByte.Models.Permission[]), 27 },
                {typeof(global::AccelByte.Models.PublicUserInfo[]), 28 },
                {typeof(global::AccelByte.Models.PlatformUserIdMap[]), 29 },
                {typeof(global::AccelByte.Models.AccountLinkedPlatform[]), 30 },
                {typeof(global::AccelByte.Models.AccountLinkPublisherAccount[]), 31 },
                {typeof(global::AccelByte.Models.AchievementIcon), 32 },
                {typeof(global::AccelByte.Models.PublicAchievement), 33 },
                {typeof(global::AccelByte.Models.Paging), 34 },
                {typeof(global::AccelByte.Models.PaginatedPublicAchievement), 35 },
                {typeof(global::AccelByte.Models.MultiLanguageAchievement), 36 },
                {typeof(global::AccelByte.Models.CountInfo), 37 },
                {typeof(global::AccelByte.Models.UserAchievement), 38 },
                {typeof(global::AccelByte.Models.PaginatedUserAchievement), 39 },
                {typeof(global::AccelByte.Models.IneligibleUser), 40 },
                {typeof(global::AccelByte.Models.LocalizedPolicyVersionObject), 41 },
                {typeof(global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject), 42 },
                {typeof(global::AccelByte.Models.PublicPolicy), 43 },
                {typeof(global::AccelByte.Models.AcceptAgreementRequest), 44 },
                {typeof(global::AccelByte.Models.AcceptAgreementResponse), 45 },
                {typeof(global::AccelByte.Models.RetrieveUserEligibilitiesResponse), 46 },
                {typeof(global::AccelByte.Models.UserProfile), 47 },
                {typeof(global::AccelByte.Models.PublicUserProfile), 48 },
                {typeof(global::AccelByte.Models.CreateUserProfileRequest), 49 },
                {typeof(global::AccelByte.Models.UpdateUserProfileRequest), 50 },
                {typeof(global::AccelByte.Models.UserRecord), 51 },
                {typeof(global::AccelByte.Models.GameRecord), 52 },
                {typeof(global::AccelByte.Models.Slot), 53 },
                {typeof(global::AccelByte.Models.UpdateMedataRequest), 54 },
                {typeof(global::AccelByte.Models.Config), 55 },
                {typeof(global::AccelByte.Models.CurrencySummary), 56 },
                {typeof(global::AccelByte.Models.BalanceInfo), 57 },
                {typeof(global::AccelByte.Models.WalletInfo), 58 },
                {typeof(global::AccelByte.Models.WalletTransactionInfo), 59 },
                {typeof(global::AccelByte.Models.WalletTransactionPagingSlicedResult), 60 },
                {typeof(global::AccelByte.Models.CreditUserWalletRequest), 61 },
                {typeof(global::AccelByte.Models.CategoryInfo), 62 },
                {typeof(global::AccelByte.Models.RegionDataItem), 63 },
                {typeof(global::AccelByte.Models.ItemSnapshot), 64 },
                {typeof(global::AccelByte.Models.ItemCriteria), 65 },
                {typeof(global::AccelByte.Models.Image), 66 },
                {typeof(global::AccelByte.Models.ItemInfo), 67 },
                {typeof(global::AccelByte.Models.PopulatedItemInfo), 68 },
                {typeof(global::AccelByte.Models.ItemPagingSlicedResult), 69 },
                {typeof(global::AccelByte.Models.PaymentUrl), 70 },
                {typeof(global::AccelByte.Models.Price), 71 },
                {typeof(global::AccelByte.Models.OrderHistoryInfo), 72 },
                {typeof(global::AccelByte.Models.OrderInfo), 73 },
                {typeof(global::AccelByte.Models.OrderPagingSlicedResult), 74 },
                {typeof(global::AccelByte.Models.OrderRequest), 75 },
                {typeof(global::AccelByte.Models.OrderTransaction), 76 },
                {typeof(global::AccelByte.Models.EntitlementInfo), 77 },
                {typeof(global::AccelByte.Models.EntitlementPagingSlicedResult), 78 },
                {typeof(global::AccelByte.Models.ConsumeUserEntitlementRequest), 79 },
                {typeof(global::AccelByte.Models.GrantUserEntitlementRequest), 80 },
                {typeof(global::AccelByte.Models.StackableEntitlementInfo), 81 },
                {typeof(global::AccelByte.Models.Attributes), 82 },
                {typeof(global::AccelByte.Models.DistributionAttributes), 83 },
                {typeof(global::AccelByte.Models.DistributionReceiver), 84 },
                {typeof(global::AccelByte.Models.ServiceError), 85 },
                {typeof(global::AccelByte.Models.OAuthError), 86 },
                {typeof(global::AccelByte.Models.GameProfile), 87 },
                {typeof(global::AccelByte.Models.GameProfileRequest), 88 },
                {typeof(global::AccelByte.Models.GameProfileAttribute), 89 },
                {typeof(global::AccelByte.Models.GameProfilePublicInfo), 90 },
                {typeof(global::AccelByte.Models.UserGameProfiles), 91 },
                {typeof(global::AccelByte.Models.TelemetryBody), 92 },
                {typeof(global::AccelByte.Models.UserPoint), 93 },
                {typeof(global::AccelByte.Models.UserRanking), 94 },
                {typeof(global::AccelByte.Models.UserRankingData), 95 },
                {typeof(global::AccelByte.Models.LeaderboardRankingResult), 96 },
                {typeof(global::AccelByte.Models.DisconnectNotif), 97 },
                {typeof(global::AccelByte.Models.LobbySessionId), 98 },
                {typeof(global::AccelByte.Models.Notification), 99 },
                {typeof(global::AccelByte.Models.ChatMesssage), 100 },
                {typeof(global::AccelByte.Models.PersonalChatRequest), 101 },
                {typeof(global::AccelByte.Models.PartyInfo), 102 },
                {typeof(global::AccelByte.Models.PartyInviteRequest), 103 },
                {typeof(global::AccelByte.Models.PartyInvitation), 104 },
                {typeof(global::AccelByte.Models.PartyChatRequest), 105 },
                {typeof(global::AccelByte.Models.PartyJoinRequest), 106 },
                {typeof(global::AccelByte.Models.PartyKickRequest), 107 },
                {typeof(global::AccelByte.Models.JoinNotification), 108 },
                {typeof(global::AccelByte.Models.KickNotification), 109 },
                {typeof(global::AccelByte.Models.LeaveNotification), 110 },
                {typeof(global::AccelByte.Models.StartMatchmakingRequest), 111 },
                {typeof(global::AccelByte.Models.MatchmakingNotif), 112 },
                {typeof(global::AccelByte.Models.DsNotif), 113 },
                {typeof(global::AccelByte.Models.MatchmakingCode), 114 },
                {typeof(global::AccelByte.Models.ReadyConsentRequest), 115 },
                {typeof(global::AccelByte.Models.ReadyForMatchConfirmation), 116 },
                {typeof(global::AccelByte.Models.RematchmakingNotification), 117 },
                {typeof(global::AccelByte.Models.FriendshipStatus), 118 },
                {typeof(global::AccelByte.Models.Friends), 119 },
                {typeof(global::AccelByte.Models.Friend), 120 },
                {typeof(global::AccelByte.Models.FriendsStatus), 121 },
                {typeof(global::AccelByte.Models.BulkFriendsRequest), 122 },
                {typeof(global::AccelByte.Models.FriendsStatusNotif), 123 },
                {typeof(global::AccelByte.Models.OnlineFriends), 124 },
                {typeof(global::AccelByte.Models.PlatformLink), 125 },
                {typeof(global::AccelByte.Models.Collection), 126 },
                {typeof(global::AccelByte.Models.QosServer), 127 },
                {typeof(global::AccelByte.Models.QosServerList), 128 },
                {typeof(global::AccelByte.Models.ServerConfig), 129 },
                {typeof(global::AccelByte.Models.RegisterServerRequest), 130 },
                {typeof(global::AccelByte.Models.ShutdownServerRequest), 131 },
                {typeof(global::AccelByte.Models.RegisterLocalServerRequest), 132 },
                {typeof(global::AccelByte.Models.PartyMember), 133 },
                {typeof(global::AccelByte.Models.MatchParty), 134 },
                {typeof(global::AccelByte.Models.MatchingAlly), 135 },
                {typeof(global::AccelByte.Models.MatchRequest), 136 },
                {typeof(global::AccelByte.Models.DSMClient), 137 },
                {typeof(global::AccelByte.Models.PubIp), 138 },
                {typeof(global::AccelByte.Models.ServerInfo), 139 },
                {typeof(global::AccelByte.Models.StatConfig), 140 },
                {typeof(global::AccelByte.Models.StatItem), 141 },
                {typeof(global::AccelByte.Models.CreateStatItemRequest), 142 },
                {typeof(global::AccelByte.Models.PagedStatItems), 143 },
                {typeof(global::AccelByte.Models.UserStatItemIncrement), 144 },
                {typeof(global::AccelByte.Models.StatItemIncrement), 145 },
                {typeof(global::AccelByte.Models.StatItemOperationResult), 146 },
                {typeof(global::AccelByte.Models.TelemetryEventTag), 147 },
                {typeof(global::AccelByte.Models.TokenData), 148 },
                {typeof(global::AccelByte.Models.SessionData), 149 },
                {typeof(global::AccelByte.Models.Ban), 150 },
                {typeof(global::AccelByte.Models.Permission), 151 },
                {typeof(global::AccelByte.Models.UserData), 152 },
                {typeof(global::AccelByte.Models.PublicUserInfo), 153 },
                {typeof(global::AccelByte.Models.PagedPublicUsersInfo), 154 },
                {typeof(global::AccelByte.Models.RegisterUserRequest), 155 },
                {typeof(global::AccelByte.Models.RegisterUserResponse), 156 },
                {typeof(global::AccelByte.Models.UpdateUserRequest), 157 },
                {typeof(global::AccelByte.Models.PagedPlatformLinks), 158 },
                {typeof(global::AccelByte.Models.BulkPlatformUserIdRequest), 159 },
                {typeof(global::AccelByte.Models.PlatformUserIdMap), 160 },
                {typeof(global::AccelByte.Models.BulkPlatformUserIdResponse), 161 },
                {typeof(global::AccelByte.Models.CountryInfo), 162 },
                {typeof(global::AccelByte.Models.UpgradeUserRequest), 163 },
                {typeof(global::AccelByte.Models.AccountLinkedPlatform), 164 },
                {typeof(global::AccelByte.Models.AccountLinkPublisherAccount), 165 },
                {typeof(global::AccelByte.Models.AccountLinkConfictMessageVariables), 166 },
                {typeof(global::AccelByte.Models.LinkPlatformAccountRequest), 167 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.AchievementIcon>();
                case 1: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PublicAchievement>();
                case 2: return new global::Utf8Json.Formatters.DictionaryFormatter<string, string>();
                case 3: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.UserAchievement>();
                case 4: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.LocalizedPolicyVersionObject>();
                case 5: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject>();
                case 6: return new global::Utf8Json.Formatters.DictionaryFormatter<string, object>();
                case 7: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.WalletTransactionInfo>();
                case 8: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Image>();
                case 9: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.RegionDataItem>();
                case 10: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.ItemInfo>();
                case 11: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.OrderInfo>();
                case 12: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.EntitlementInfo>();
                case 13: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.GameProfilePublicInfo>();
                case 14: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.UserPoint>();
                case 15: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Slot>();
                case 16: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.UserGameProfiles>();
                case 17: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.GameProfile>();
                case 18: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.OrderHistoryInfo>();
                case 19: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PlatformLink>();
                case 20: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PublicUserProfile>();
                case 21: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.QosServer>();
                case 22: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PartyMember>();
                case 23: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.MatchParty>();
                case 24: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.MatchingAlly>();
                case 25: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.StatItem>();
                case 26: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Ban>();
                case 27: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Permission>();
                case 28: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PublicUserInfo>();
                case 29: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PlatformUserIdMap>();
                case 30: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.AccountLinkedPlatform>();
                case 31: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.AccountLinkPublisherAccount>();
                case 32: return new Utf8Json.Formatters.AccelByte.Models.AchievementIconFormatter();
                case 33: return new Utf8Json.Formatters.AccelByte.Models.PublicAchievementFormatter();
                case 34: return new Utf8Json.Formatters.AccelByte.Models.PagingFormatter();
                case 35: return new Utf8Json.Formatters.AccelByte.Models.PaginatedPublicAchievementFormatter();
                case 36: return new Utf8Json.Formatters.AccelByte.Models.MultiLanguageAchievementFormatter();
                case 37: return new Utf8Json.Formatters.AccelByte.Models.CountInfoFormatter();
                case 38: return new Utf8Json.Formatters.AccelByte.Models.UserAchievementFormatter();
                case 39: return new Utf8Json.Formatters.AccelByte.Models.PaginatedUserAchievementFormatter();
                case 40: return new Utf8Json.Formatters.AccelByte.Models.IneligibleUserFormatter();
                case 41: return new Utf8Json.Formatters.AccelByte.Models.LocalizedPolicyVersionObjectFormatter();
                case 42: return new Utf8Json.Formatters.AccelByte.Models.PolicyVersionWithLocalizedVersionObjectFormatter();
                case 43: return new Utf8Json.Formatters.AccelByte.Models.PublicPolicyFormatter();
                case 44: return new Utf8Json.Formatters.AccelByte.Models.AcceptAgreementRequestFormatter();
                case 45: return new Utf8Json.Formatters.AccelByte.Models.AcceptAgreementResponseFormatter();
                case 46: return new Utf8Json.Formatters.AccelByte.Models.RetrieveUserEligibilitiesResponseFormatter();
                case 47: return new Utf8Json.Formatters.AccelByte.Models.UserProfileFormatter();
                case 48: return new Utf8Json.Formatters.AccelByte.Models.PublicUserProfileFormatter();
                case 49: return new Utf8Json.Formatters.AccelByte.Models.CreateUserProfileRequestFormatter();
                case 50: return new Utf8Json.Formatters.AccelByte.Models.UpdateUserProfileRequestFormatter();
                case 51: return new Utf8Json.Formatters.AccelByte.Models.UserRecordFormatter();
                case 52: return new Utf8Json.Formatters.AccelByte.Models.GameRecordFormatter();
                case 53: return new Utf8Json.Formatters.AccelByte.Models.SlotFormatter();
                case 54: return new Utf8Json.Formatters.AccelByte.Models.UpdateMedataRequestFormatter();
                case 55: return new Utf8Json.Formatters.AccelByte.Models.ConfigFormatter();
                case 56: return new Utf8Json.Formatters.AccelByte.Models.CurrencySummaryFormatter();
                case 57: return new Utf8Json.Formatters.AccelByte.Models.BalanceInfoFormatter();
                case 58: return new Utf8Json.Formatters.AccelByte.Models.WalletInfoFormatter();
                case 59: return new Utf8Json.Formatters.AccelByte.Models.WalletTransactionInfoFormatter();
                case 60: return new Utf8Json.Formatters.AccelByte.Models.WalletTransactionPagingSlicedResultFormatter();
                case 61: return new Utf8Json.Formatters.AccelByte.Models.CreditUserWalletRequestFormatter();
                case 62: return new Utf8Json.Formatters.AccelByte.Models.CategoryInfoFormatter();
                case 63: return new Utf8Json.Formatters.AccelByte.Models.RegionDataItemFormatter();
                case 64: return new Utf8Json.Formatters.AccelByte.Models.ItemSnapshotFormatter();
                case 65: return new Utf8Json.Formatters.AccelByte.Models.ItemCriteriaFormatter();
                case 66: return new Utf8Json.Formatters.AccelByte.Models.ImageFormatter();
                case 67: return new Utf8Json.Formatters.AccelByte.Models.ItemInfoFormatter();
                case 68: return new Utf8Json.Formatters.AccelByte.Models.PopulatedItemInfoFormatter();
                case 69: return new Utf8Json.Formatters.AccelByte.Models.ItemPagingSlicedResultFormatter();
                case 70: return new Utf8Json.Formatters.AccelByte.Models.PaymentUrlFormatter();
                case 71: return new Utf8Json.Formatters.AccelByte.Models.PriceFormatter();
                case 72: return new Utf8Json.Formatters.AccelByte.Models.OrderHistoryInfoFormatter();
                case 73: return new Utf8Json.Formatters.AccelByte.Models.OrderInfoFormatter();
                case 74: return new Utf8Json.Formatters.AccelByte.Models.OrderPagingSlicedResultFormatter();
                case 75: return new Utf8Json.Formatters.AccelByte.Models.OrderRequestFormatter();
                case 76: return new Utf8Json.Formatters.AccelByte.Models.OrderTransactionFormatter();
                case 77: return new Utf8Json.Formatters.AccelByte.Models.EntitlementInfoFormatter();
                case 78: return new Utf8Json.Formatters.AccelByte.Models.EntitlementPagingSlicedResultFormatter();
                case 79: return new Utf8Json.Formatters.AccelByte.Models.ConsumeUserEntitlementRequestFormatter();
                case 80: return new Utf8Json.Formatters.AccelByte.Models.GrantUserEntitlementRequestFormatter();
                case 81: return new Utf8Json.Formatters.AccelByte.Models.StackableEntitlementInfoFormatter();
                case 82: return new Utf8Json.Formatters.AccelByte.Models.AttributesFormatter();
                case 83: return new Utf8Json.Formatters.AccelByte.Models.DistributionAttributesFormatter();
                case 84: return new Utf8Json.Formatters.AccelByte.Models.DistributionReceiverFormatter();
                case 85: return new Utf8Json.Formatters.AccelByte.Models.ServiceErrorFormatter();
                case 86: return new Utf8Json.Formatters.AccelByte.Models.OAuthErrorFormatter();
                case 87: return new Utf8Json.Formatters.AccelByte.Models.GameProfileFormatter();
                case 88: return new Utf8Json.Formatters.AccelByte.Models.GameProfileRequestFormatter();
                case 89: return new Utf8Json.Formatters.AccelByte.Models.GameProfileAttributeFormatter();
                case 90: return new Utf8Json.Formatters.AccelByte.Models.GameProfilePublicInfoFormatter();
                case 91: return new Utf8Json.Formatters.AccelByte.Models.UserGameProfilesFormatter();
                case 92: return new Utf8Json.Formatters.AccelByte.Models.TelemetryBodyFormatter();
                case 93: return new Utf8Json.Formatters.AccelByte.Models.UserPointFormatter();
                case 94: return new Utf8Json.Formatters.AccelByte.Models.UserRankingFormatter();
                case 95: return new Utf8Json.Formatters.AccelByte.Models.UserRankingDataFormatter();
                case 96: return new Utf8Json.Formatters.AccelByte.Models.LeaderboardRankingResultFormatter();
                case 97: return new Utf8Json.Formatters.AccelByte.Models.DisconnectNotifFormatter();
                case 98: return new Utf8Json.Formatters.AccelByte.Models.LobbySessionIdFormatter();
                case 99: return new Utf8Json.Formatters.AccelByte.Models.NotificationFormatter();
                case 100: return new Utf8Json.Formatters.AccelByte.Models.ChatMesssageFormatter();
                case 101: return new Utf8Json.Formatters.AccelByte.Models.PersonalChatRequestFormatter();
                case 102: return new Utf8Json.Formatters.AccelByte.Models.PartyInfoFormatter();
                case 103: return new Utf8Json.Formatters.AccelByte.Models.PartyInviteRequestFormatter();
                case 104: return new Utf8Json.Formatters.AccelByte.Models.PartyInvitationFormatter();
                case 105: return new Utf8Json.Formatters.AccelByte.Models.PartyChatRequestFormatter();
                case 106: return new Utf8Json.Formatters.AccelByte.Models.PartyJoinRequestFormatter();
                case 107: return new Utf8Json.Formatters.AccelByte.Models.PartyKickRequestFormatter();
                case 108: return new Utf8Json.Formatters.AccelByte.Models.JoinNotificationFormatter();
                case 109: return new Utf8Json.Formatters.AccelByte.Models.KickNotificationFormatter();
                case 110: return new Utf8Json.Formatters.AccelByte.Models.LeaveNotificationFormatter();
                case 111: return new Utf8Json.Formatters.AccelByte.Models.StartMatchmakingRequestFormatter();
                case 112: return new Utf8Json.Formatters.AccelByte.Models.MatchmakingNotifFormatter();
                case 113: return new Utf8Json.Formatters.AccelByte.Models.DsNotifFormatter();
                case 114: return new Utf8Json.Formatters.AccelByte.Models.MatchmakingCodeFormatter();
                case 115: return new Utf8Json.Formatters.AccelByte.Models.ReadyConsentRequestFormatter();
                case 116: return new Utf8Json.Formatters.AccelByte.Models.ReadyForMatchConfirmationFormatter();
                case 117: return new Utf8Json.Formatters.AccelByte.Models.RematchmakingNotificationFormatter();
                case 118: return new Utf8Json.Formatters.AccelByte.Models.FriendshipStatusFormatter();
                case 119: return new Utf8Json.Formatters.AccelByte.Models.FriendsFormatter();
                case 120: return new Utf8Json.Formatters.AccelByte.Models.FriendFormatter();
                case 121: return new Utf8Json.Formatters.AccelByte.Models.FriendsStatusFormatter();
                case 122: return new Utf8Json.Formatters.AccelByte.Models.BulkFriendsRequestFormatter();
                case 123: return new Utf8Json.Formatters.AccelByte.Models.FriendsStatusNotifFormatter();
                case 124: return new Utf8Json.Formatters.AccelByte.Models.OnlineFriendsFormatter();
                case 125: return new Utf8Json.Formatters.AccelByte.Models.PlatformLinkFormatter();
                case 126: return new Utf8Json.Formatters.AccelByte.Models.CollectionFormatter();
                case 127: return new Utf8Json.Formatters.AccelByte.Models.QosServerFormatter();
                case 128: return new Utf8Json.Formatters.AccelByte.Models.QosServerListFormatter();
                case 129: return new Utf8Json.Formatters.AccelByte.Models.ServerConfigFormatter();
                case 130: return new Utf8Json.Formatters.AccelByte.Models.RegisterServerRequestFormatter();
                case 131: return new Utf8Json.Formatters.AccelByte.Models.ShutdownServerRequestFormatter();
                case 132: return new Utf8Json.Formatters.AccelByte.Models.RegisterLocalServerRequestFormatter();
                case 133: return new Utf8Json.Formatters.AccelByte.Models.PartyMemberFormatter();
                case 134: return new Utf8Json.Formatters.AccelByte.Models.MatchPartyFormatter();
                case 135: return new Utf8Json.Formatters.AccelByte.Models.MatchingAllyFormatter();
                case 136: return new Utf8Json.Formatters.AccelByte.Models.MatchRequestFormatter();
                case 137: return new Utf8Json.Formatters.AccelByte.Models.DSMClientFormatter();
                case 138: return new Utf8Json.Formatters.AccelByte.Models.PubIpFormatter();
                case 139: return new Utf8Json.Formatters.AccelByte.Models.ServerInfoFormatter();
                case 140: return new Utf8Json.Formatters.AccelByte.Models.StatConfigFormatter();
                case 141: return new Utf8Json.Formatters.AccelByte.Models.StatItemFormatter();
                case 142: return new Utf8Json.Formatters.AccelByte.Models.CreateStatItemRequestFormatter();
                case 143: return new Utf8Json.Formatters.AccelByte.Models.PagedStatItemsFormatter();
                case 144: return new Utf8Json.Formatters.AccelByte.Models.UserStatItemIncrementFormatter();
                case 145: return new Utf8Json.Formatters.AccelByte.Models.StatItemIncrementFormatter();
                case 146: return new Utf8Json.Formatters.AccelByte.Models.StatItemOperationResultFormatter();
                case 147: return new Utf8Json.Formatters.AccelByte.Models.TelemetryEventTagFormatter();
                case 148: return new Utf8Json.Formatters.AccelByte.Models.TokenDataFormatter();
                case 149: return new Utf8Json.Formatters.AccelByte.Models.SessionDataFormatter();
                case 150: return new Utf8Json.Formatters.AccelByte.Models.BanFormatter();
                case 151: return new Utf8Json.Formatters.AccelByte.Models.PermissionFormatter();
                case 152: return new Utf8Json.Formatters.AccelByte.Models.UserDataFormatter();
                case 153: return new Utf8Json.Formatters.AccelByte.Models.PublicUserInfoFormatter();
                case 154: return new Utf8Json.Formatters.AccelByte.Models.PagedPublicUsersInfoFormatter();
                case 155: return new Utf8Json.Formatters.AccelByte.Models.RegisterUserRequestFormatter();
                case 156: return new Utf8Json.Formatters.AccelByte.Models.RegisterUserResponseFormatter();
                case 157: return new Utf8Json.Formatters.AccelByte.Models.UpdateUserRequestFormatter();
                case 158: return new Utf8Json.Formatters.AccelByte.Models.PagedPlatformLinksFormatter();
                case 159: return new Utf8Json.Formatters.AccelByte.Models.BulkPlatformUserIdRequestFormatter();
                case 160: return new Utf8Json.Formatters.AccelByte.Models.PlatformUserIdMapFormatter();
                case 161: return new Utf8Json.Formatters.AccelByte.Models.BulkPlatformUserIdResponseFormatter();
                case 162: return new Utf8Json.Formatters.AccelByte.Models.CountryInfoFormatter();
                case 163: return new Utf8Json.Formatters.AccelByte.Models.UpgradeUserRequestFormatter();
                case 164: return new Utf8Json.Formatters.AccelByte.Models.AccountLinkedPlatformFormatter();
                case 165: return new Utf8Json.Formatters.AccelByte.Models.AccountLinkPublisherAccountFormatter();
                case 166: return new Utf8Json.Formatters.AccelByte.Models.AccountLinkConfictMessageVariablesFormatter();
                case 167: return new Utf8Json.Formatters.AccelByte.Models.LinkPlatformAccountRequestFormatter();
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

namespace Utf8Json.Formatters.AccelByte.Models
{
    using System;
    using Utf8Json;


    public sealed class AchievementIconFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AchievementIcon>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AchievementIconFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("url"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("slug"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("url"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("slug"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AchievementIcon value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.url);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.slug);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AchievementIcon Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __url__ = default(string);
            var __url__b__ = false;
            var __slug__ = default(string);
            var __slug__b__ = false;

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
                        __url__ = reader.ReadString();
                        __url__b__ = true;
                        break;
                    case 1:
                        __slug__ = reader.ReadString();
                        __slug__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AchievementIcon();
            if(__url__b__) ____result.url = __url__;
            if(__slug__b__) ____result.slug = __slug__;

            return ____result;
        }
    }


    public sealed class PublicAchievementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PublicAchievement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PublicAchievementFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("achievementCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lockedIcons"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("unlockedIcons"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("hidden"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("listOrder"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incremental"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("goalValue"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updateAt"), 13},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("achievementCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
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
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updateAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PublicAchievement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Serialize(ref writer, value.lockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Serialize(ref writer, value.unlockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.hidden);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.listOrder);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value.incremental);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.goalValue);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.updateAt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PublicAchievement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __achievementCode__ = default(string);
            var __achievementCode__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __lockedIcons__ = default(global::AccelByte.Models.AchievementIcon[]);
            var __lockedIcons__b__ = false;
            var __unlockedIcons__ = default(global::AccelByte.Models.AchievementIcon[]);
            var __unlockedIcons__b__ = false;
            var __hidden__ = default(bool);
            var __hidden__b__ = false;
            var __listOrder__ = default(int);
            var __listOrder__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __incremental__ = default(bool);
            var __incremental__b__ = false;
            var __goalValue__ = default(int);
            var __goalValue__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updateAt__ = default(string);
            var __updateAt__b__ = false;

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
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 3:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 4:
                        __lockedIcons__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __lockedIcons__b__ = true;
                        break;
                    case 5:
                        __unlockedIcons__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __unlockedIcons__b__ = true;
                        break;
                    case 6:
                        __hidden__ = reader.ReadBoolean();
                        __hidden__b__ = true;
                        break;
                    case 7:
                        __listOrder__ = reader.ReadInt32();
                        __listOrder__b__ = true;
                        break;
                    case 8:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 9:
                        __incremental__ = reader.ReadBoolean();
                        __incremental__b__ = true;
                        break;
                    case 10:
                        __goalValue__ = reader.ReadInt32();
                        __goalValue__b__ = true;
                        break;
                    case 11:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 12:
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 13:
                        __updateAt__ = reader.ReadString();
                        __updateAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PublicAchievement();
            if(__achievementCode__b__) ____result.achievementCode = __achievementCode__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
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
            if(__updateAt__b__) ____result.updateAt = __updateAt__;

            return ____result;
        }
    }


    public sealed class PagingFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Paging>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagingFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("first"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("last"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("next"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("previous"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("first"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("last"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("next"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("previous"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Paging value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.first);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.last);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.next);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.previous);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Paging Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __first__ = default(string);
            var __first__b__ = false;
            var __last__ = default(string);
            var __last__b__ = false;
            var __next__ = default(string);
            var __next__b__ = false;
            var __previous__ = default(string);
            var __previous__b__ = false;

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
                        __first__ = reader.ReadString();
                        __first__b__ = true;
                        break;
                    case 1:
                        __last__ = reader.ReadString();
                        __last__b__ = true;
                        break;
                    case 2:
                        __next__ = reader.ReadString();
                        __next__b__ = true;
                        break;
                    case 3:
                        __previous__ = reader.ReadString();
                        __previous__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Paging();
            if(__first__b__) ____result.first = __first__;
            if(__last__b__) ____result.last = __last__;
            if(__next__b__) ____result.next = __next__;
            if(__previous__b__) ____result.previous = __previous__;

            return ____result;
        }
    }


    public sealed class PaginatedPublicAchievementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PaginatedPublicAchievement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PaginatedPublicAchievementFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PaginatedPublicAchievement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PublicAchievement[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PaginatedPublicAchievement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.PublicAchievement[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PublicAchievement[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PaginatedPublicAchievement();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class MultiLanguageAchievementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MultiLanguageAchievement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MultiLanguageAchievementFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("achievementCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lockedIcons"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("unlockedIcons"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("hidden"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("listOrder"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incremental"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("goalValue"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updateAt"), 13},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("achievementCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
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
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updateAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MultiLanguageAchievement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.name, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.description, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Serialize(ref writer, value.lockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Serialize(ref writer, value.unlockedIcons, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.hidden);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.listOrder);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value.incremental);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.goalValue);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.updateAt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MultiLanguageAchievement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __achievementCode__ = default(string);
            var __achievementCode__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __name__b__ = false;
            var __description__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __description__b__ = false;
            var __lockedIcons__ = default(global::AccelByte.Models.AchievementIcon[]);
            var __lockedIcons__b__ = false;
            var __unlockedIcons__ = default(global::AccelByte.Models.AchievementIcon[]);
            var __unlockedIcons__b__ = false;
            var __hidden__ = default(bool);
            var __hidden__b__ = false;
            var __listOrder__ = default(int);
            var __listOrder__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __incremental__ = default(bool);
            var __incremental__b__ = false;
            var __goalValue__ = default(int);
            var __goalValue__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updateAt__ = default(string);
            var __updateAt__b__ = false;

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
                        __name__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __name__b__ = true;
                        break;
                    case 3:
                        __description__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __description__b__ = true;
                        break;
                    case 4:
                        __lockedIcons__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __lockedIcons__b__ = true;
                        break;
                    case 5:
                        __unlockedIcons__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AchievementIcon[]>().Deserialize(ref reader, formatterResolver);
                        __unlockedIcons__b__ = true;
                        break;
                    case 6:
                        __hidden__ = reader.ReadBoolean();
                        __hidden__b__ = true;
                        break;
                    case 7:
                        __listOrder__ = reader.ReadInt32();
                        __listOrder__b__ = true;
                        break;
                    case 8:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 9:
                        __incremental__ = reader.ReadBoolean();
                        __incremental__b__ = true;
                        break;
                    case 10:
                        __goalValue__ = reader.ReadInt32();
                        __goalValue__b__ = true;
                        break;
                    case 11:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 12:
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 13:
                        __updateAt__ = reader.ReadString();
                        __updateAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MultiLanguageAchievement();
            if(__achievementCode__b__) ____result.achievementCode = __achievementCode__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
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
            if(__updateAt__b__) ____result.updateAt = __updateAt__;

            return ____result;
        }
    }


    public sealed class CountInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CountInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CountInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("numberOfAchievements"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("numberOfHiddenAchievements"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("numberOfVisibleAchievements"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("numberOfAchievements"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("numberOfHiddenAchievements"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("numberOfVisibleAchievements"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CountInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.numberOfAchievements);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.numberOfHiddenAchievements);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.numberOfVisibleAchievements);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CountInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __numberOfAchievements__ = default(int);
            var __numberOfAchievements__b__ = false;
            var __numberOfHiddenAchievements__ = default(int);
            var __numberOfHiddenAchievements__b__ = false;
            var __numberOfVisibleAchievements__ = default(int);
            var __numberOfVisibleAchievements__b__ = false;

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
                        __numberOfAchievements__ = reader.ReadInt32();
                        __numberOfAchievements__b__ = true;
                        break;
                    case 1:
                        __numberOfHiddenAchievements__ = reader.ReadInt32();
                        __numberOfHiddenAchievements__b__ = true;
                        break;
                    case 2:
                        __numberOfVisibleAchievements__ = reader.ReadInt32();
                        __numberOfVisibleAchievements__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.CountInfo();
            if(__numberOfAchievements__b__) ____result.numberOfAchievements = __numberOfAchievements__;
            if(__numberOfHiddenAchievements__b__) ____result.numberOfHiddenAchievements = __numberOfHiddenAchievements__;
            if(__numberOfVisibleAchievements__b__) ____result.numberOfVisibleAchievements = __numberOfVisibleAchievements__;

            return ____result;
        }
    }


    public sealed class UserAchievementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserAchievement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserAchievementFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("achievementCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("achievedAt"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("latestValue"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("achievementCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("achievedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("latestValue"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserAchievement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.achievementCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.achievedAt);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.latestValue);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.status);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserAchievement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __achievementCode__ = default(string);
            var __achievementCode__b__ = false;
            var __achievedAt__ = default(string);
            var __achievedAt__b__ = false;
            var __latestValue__ = default(int);
            var __latestValue__b__ = false;
            var __status__ = default(int);
            var __status__b__ = false;

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
                        __achievedAt__ = reader.ReadString();
                        __achievedAt__b__ = true;
                        break;
                    case 2:
                        __latestValue__ = reader.ReadInt32();
                        __latestValue__b__ = true;
                        break;
                    case 3:
                        __status__ = reader.ReadInt32();
                        __status__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserAchievement();
            if(__achievementCode__b__) ____result.achievementCode = __achievementCode__;
            if(__achievedAt__b__) ____result.achievedAt = __achievedAt__;
            if(__latestValue__b__) ____result.latestValue = __latestValue__;
            if(__status__b__) ____result.status = __status__;

            return ____result;
        }
    }


    public sealed class PaginatedUserAchievementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PaginatedUserAchievement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PaginatedUserAchievementFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countInfo"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("data"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paging"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("countInfo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paging"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PaginatedUserAchievement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CountInfo>().Serialize(ref writer, value.countInfo, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserAchievement[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PaginatedUserAchievement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __countInfo__ = default(global::AccelByte.Models.CountInfo);
            var __countInfo__b__ = false;
            var __data__ = default(global::AccelByte.Models.UserAchievement[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __countInfo__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CountInfo>().Deserialize(ref reader, formatterResolver);
                        __countInfo__b__ = true;
                        break;
                    case 1:
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserAchievement[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 2:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PaginatedUserAchievement();
            if(__countInfo__b__) ____result.countInfo = __countInfo__;
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class IneligibleUserFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.IneligibleUser>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public IneligibleUserFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("eligible"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("deletionStatus"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("country"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("eligible"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("deletionStatus"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.IneligibleUser value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.country);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.eligible);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.deletionStatus);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.IneligibleUser Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __country__ = default(string);
            var __country__b__ = false;
            var __eligible__ = default(bool);
            var __eligible__b__ = false;
            var __deletionStatus__ = default(bool);
            var __deletionStatus__b__ = false;

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
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 1:
                        __country__ = reader.ReadString();
                        __country__b__ = true;
                        break;
                    case 2:
                        __eligible__ = reader.ReadBoolean();
                        __eligible__b__ = true;
                        break;
                    case 3:
                        __deletionStatus__ = reader.ReadBoolean();
                        __deletionStatus__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.IneligibleUser();
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__country__b__) ____result.country = __country__;
            if(__eligible__b__) ____result.eligible = __eligible__;
            if(__deletionStatus__b__) ____result.deletionStatus = __deletionStatus__;

            return ____result;
        }
    }


    public sealed class LocalizedPolicyVersionObjectFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LocalizedPolicyVersionObject>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LocalizedPolicyVersionObjectFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localeCode"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("contentType"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attachmentLocation"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attachmentChecksum"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attachmentVersionIdentifier"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("publishedDate"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isDefaultSelection"), 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localeCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("contentType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("attachmentLocation"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("attachmentChecksum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("attachmentVersionIdentifier"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("publishedDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isDefaultSelection"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LocalizedPolicyVersionObject value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.updatedAt);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.localeCode);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.contentType);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.attachmentLocation);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.attachmentChecksum);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.attachmentVersionIdentifier);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.publishedDate);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteBoolean(value.isDefaultSelection);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LocalizedPolicyVersionObject Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(string);
            var __updatedAt__b__ = false;
            var __localeCode__ = default(string);
            var __localeCode__b__ = false;
            var __contentType__ = default(string);
            var __contentType__b__ = false;
            var __attachmentLocation__ = default(string);
            var __attachmentLocation__b__ = false;
            var __attachmentChecksum__ = default(string);
            var __attachmentChecksum__b__ = false;
            var __attachmentVersionIdentifier__ = default(string);
            var __attachmentVersionIdentifier__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __publishedDate__ = default(string);
            var __publishedDate__b__ = false;
            var __isDefaultSelection__ = default(bool);
            var __isDefaultSelection__b__ = false;

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
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 2:
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    case 3:
                        __localeCode__ = reader.ReadString();
                        __localeCode__b__ = true;
                        break;
                    case 4:
                        __contentType__ = reader.ReadString();
                        __contentType__b__ = true;
                        break;
                    case 5:
                        __attachmentLocation__ = reader.ReadString();
                        __attachmentLocation__b__ = true;
                        break;
                    case 6:
                        __attachmentChecksum__ = reader.ReadString();
                        __attachmentChecksum__b__ = true;
                        break;
                    case 7:
                        __attachmentVersionIdentifier__ = reader.ReadString();
                        __attachmentVersionIdentifier__b__ = true;
                        break;
                    case 8:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 9:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 10:
                        __publishedDate__ = reader.ReadString();
                        __publishedDate__b__ = true;
                        break;
                    case 11:
                        __isDefaultSelection__ = reader.ReadBoolean();
                        __isDefaultSelection__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LocalizedPolicyVersionObject();
            if(__id__b__) ____result.id = __id__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__localeCode__b__) ____result.localeCode = __localeCode__;
            if(__contentType__b__) ____result.contentType = __contentType__;
            if(__attachmentLocation__b__) ____result.attachmentLocation = __attachmentLocation__;
            if(__attachmentChecksum__b__) ____result.attachmentChecksum = __attachmentChecksum__;
            if(__attachmentVersionIdentifier__b__) ____result.attachmentVersionIdentifier = __attachmentVersionIdentifier__;
            if(__description__b__) ____result.description = __description__;
            if(__status__b__) ____result.status = __status__;
            if(__publishedDate__b__) ____result.publishedDate = __publishedDate__;
            if(__isDefaultSelection__b__) ____result.isDefaultSelection = __isDefaultSelection__;

            return ____result;
        }
    }


    public sealed class PolicyVersionWithLocalizedVersionObjectFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PolicyVersionWithLocalizedVersionObjectFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayVersion"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("publishedDate"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizedPolicyVersions"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isCommitted"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isCrucial"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isInEffect"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayVersion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("publishedDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizedPolicyVersions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isCommitted"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isCrucial"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isInEffect"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.updatedAt);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.displayVersion);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.publishedDate);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LocalizedPolicyVersionObject[]>().Serialize(ref writer, value.localizedPolicyVersions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteBoolean(value.isCommitted);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value.isCrucial);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteBoolean(value.isInEffect);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(string);
            var __updatedAt__b__ = false;
            var __displayVersion__ = default(string);
            var __displayVersion__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __publishedDate__ = default(string);
            var __publishedDate__b__ = false;
            var __localizedPolicyVersions__ = default(global::AccelByte.Models.LocalizedPolicyVersionObject[]);
            var __localizedPolicyVersions__b__ = false;
            var __isCommitted__ = default(bool);
            var __isCommitted__b__ = false;
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
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 2:
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    case 3:
                        __displayVersion__ = reader.ReadString();
                        __displayVersion__b__ = true;
                        break;
                    case 4:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 5:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 6:
                        __publishedDate__ = reader.ReadString();
                        __publishedDate__b__ = true;
                        break;
                    case 7:
                        __localizedPolicyVersions__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.LocalizedPolicyVersionObject[]>().Deserialize(ref reader, formatterResolver);
                        __localizedPolicyVersions__b__ = true;
                        break;
                    case 8:
                        __isCommitted__ = reader.ReadBoolean();
                        __isCommitted__b__ = true;
                        break;
                    case 9:
                        __isCrucial__ = reader.ReadBoolean();
                        __isCrucial__b__ = true;
                        break;
                    case 10:
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

            var ____result = new global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject();
            if(__id__b__) ____result.id = __id__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__displayVersion__b__) ____result.displayVersion = __displayVersion__;
            if(__description__b__) ____result.description = __description__;
            if(__status__b__) ____result.status = __status__;
            if(__publishedDate__b__) ____result.publishedDate = __publishedDate__;
            if(__localizedPolicyVersions__b__) ____result.localizedPolicyVersions = __localizedPolicyVersions__;
            if(__isCommitted__b__) ____result.isCommitted = __isCommitted__;
            if(__isCrucial__b__) ____result.isCrucial = __isCrucial__;
            if(__isInEffect__b__) ____result.isInEffect = __isInEffect__;

            return ____result;
        }
    }


    public sealed class PublicPolicyFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PublicPolicy>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PublicPolicyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("readableId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyName"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyType"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countryCode"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countryGroupCode"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseUrls"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("shouldNotifyOnUpdate"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyVersions"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isMandatory"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isDefaultOpted"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isDefaultSelection"), 15},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("readableId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("countryCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("countryGroupCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseUrls"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("shouldNotifyOnUpdate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyVersions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isMandatory"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isDefaultOpted"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isDefaultSelection"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PublicPolicy value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.updatedAt);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.readableId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.policyName);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.policyType);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.countryCode);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.countryGroupCode);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.baseUrls, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteBoolean(value.shouldNotifyOnUpdate);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]>().Serialize(ref writer, value.policyVersions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteBoolean(value.isMandatory);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteBoolean(value.isDefaultOpted);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteBoolean(value.isDefaultSelection);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PublicPolicy Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(string);
            var __updatedAt__b__ = false;
            var __readableId__ = default(string);
            var __readableId__b__ = false;
            var __policyName__ = default(string);
            var __policyName__b__ = false;
            var __policyType__ = default(string);
            var __policyType__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __countryCode__ = default(string);
            var __countryCode__b__ = false;
            var __countryGroupCode__ = default(string);
            var __countryGroupCode__b__ = false;
            var __baseUrls__ = default(string[]);
            var __baseUrls__b__ = false;
            var __shouldNotifyOnUpdate__ = default(bool);
            var __shouldNotifyOnUpdate__b__ = false;
            var __policyVersions__ = default(global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]);
            var __policyVersions__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __isMandatory__ = default(bool);
            var __isMandatory__b__ = false;
            var __isDefaultOpted__ = default(bool);
            var __isDefaultOpted__b__ = false;
            var __isDefaultSelection__ = default(bool);
            var __isDefaultSelection__b__ = false;

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
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 2:
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    case 3:
                        __readableId__ = reader.ReadString();
                        __readableId__b__ = true;
                        break;
                    case 4:
                        __policyName__ = reader.ReadString();
                        __policyName__b__ = true;
                        break;
                    case 5:
                        __policyType__ = reader.ReadString();
                        __policyType__b__ = true;
                        break;
                    case 6:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 7:
                        __countryCode__ = reader.ReadString();
                        __countryCode__b__ = true;
                        break;
                    case 8:
                        __countryGroupCode__ = reader.ReadString();
                        __countryGroupCode__b__ = true;
                        break;
                    case 9:
                        __baseUrls__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __baseUrls__b__ = true;
                        break;
                    case 10:
                        __shouldNotifyOnUpdate__ = reader.ReadBoolean();
                        __shouldNotifyOnUpdate__b__ = true;
                        break;
                    case 11:
                        __policyVersions__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]>().Deserialize(ref reader, formatterResolver);
                        __policyVersions__b__ = true;
                        break;
                    case 12:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 13:
                        __isMandatory__ = reader.ReadBoolean();
                        __isMandatory__b__ = true;
                        break;
                    case 14:
                        __isDefaultOpted__ = reader.ReadBoolean();
                        __isDefaultOpted__b__ = true;
                        break;
                    case 15:
                        __isDefaultSelection__ = reader.ReadBoolean();
                        __isDefaultSelection__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PublicPolicy();
            if(__id__b__) ____result.id = __id__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__readableId__b__) ____result.readableId = __readableId__;
            if(__policyName__b__) ____result.policyName = __policyName__;
            if(__policyType__b__) ____result.policyType = __policyType__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__countryCode__b__) ____result.countryCode = __countryCode__;
            if(__countryGroupCode__b__) ____result.countryGroupCode = __countryGroupCode__;
            if(__baseUrls__b__) ____result.baseUrls = __baseUrls__;
            if(__shouldNotifyOnUpdate__b__) ____result.shouldNotifyOnUpdate = __shouldNotifyOnUpdate__;
            if(__policyVersions__b__) ____result.policyVersions = __policyVersions__;
            if(__description__b__) ____result.description = __description__;
            if(__isMandatory__b__) ____result.isMandatory = __isMandatory__;
            if(__isDefaultOpted__b__) ____result.isDefaultOpted = __isDefaultOpted__;
            if(__isDefaultSelection__b__) ____result.isDefaultSelection = __isDefaultSelection__;

            return ____result;
        }
    }


    public sealed class AcceptAgreementRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AcceptAgreementRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AcceptAgreementRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizedPolicyVersionId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyVersionId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isAccepted"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("localizedPolicyVersionId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyVersionId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isAccepted"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AcceptAgreementRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.localizedPolicyVersionId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.policyVersionId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.policyId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.isAccepted);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AcceptAgreementRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __localizedPolicyVersionId__ = default(string);
            var __localizedPolicyVersionId__b__ = false;
            var __policyVersionId__ = default(string);
            var __policyVersionId__b__ = false;
            var __policyId__ = default(string);
            var __policyId__b__ = false;
            var __isAccepted__ = default(bool);
            var __isAccepted__b__ = false;

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
                        __localizedPolicyVersionId__ = reader.ReadString();
                        __localizedPolicyVersionId__b__ = true;
                        break;
                    case 1:
                        __policyVersionId__ = reader.ReadString();
                        __policyVersionId__b__ = true;
                        break;
                    case 2:
                        __policyId__ = reader.ReadString();
                        __policyId__b__ = true;
                        break;
                    case 3:
                        __isAccepted__ = reader.ReadBoolean();
                        __isAccepted__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AcceptAgreementRequest();
            if(__localizedPolicyVersionId__b__) ____result.localizedPolicyVersionId = __localizedPolicyVersionId__;
            if(__policyVersionId__b__) ____result.policyVersionId = __policyVersionId__;
            if(__policyId__b__) ____result.policyId = __policyId__;
            if(__isAccepted__b__) ____result.isAccepted = __isAccepted__;

            return ____result;
        }
    }


    public sealed class AcceptAgreementResponseFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AcceptAgreementResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AcceptAgreementResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("proceed"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("proceed"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AcceptAgreementResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.proceed);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AcceptAgreementResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __proceed__ = default(bool);
            var __proceed__b__ = false;

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
                        __proceed__ = reader.ReadBoolean();
                        __proceed__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AcceptAgreementResponse();
            if(__proceed__b__) ____result.proceed = __proceed__;

            return ____result;
        }
    }


    public sealed class RetrieveUserEligibilitiesResponseFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RetrieveUserEligibilitiesResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RetrieveUserEligibilitiesResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("readableId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countryCode"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("countryGroupCode"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseUrls"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyVersions"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("policyId"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isMandatory"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isAccepted"), 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("readableId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("countryCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("countryGroupCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseUrls"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyVersions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("policyId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isMandatory"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isAccepted"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RetrieveUserEligibilitiesResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.readableId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.policyName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.policyType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.countryCode);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.countryGroupCode);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.baseUrls, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]>().Serialize(ref writer, value.policyVersions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.policyId);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteBoolean(value.isMandatory);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteBoolean(value.isAccepted);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RetrieveUserEligibilitiesResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __readableId__ = default(string);
            var __readableId__b__ = false;
            var __policyName__ = default(string);
            var __policyName__b__ = false;
            var __policyType__ = default(string);
            var __policyType__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __countryCode__ = default(string);
            var __countryCode__b__ = false;
            var __countryGroupCode__ = default(string);
            var __countryGroupCode__b__ = false;
            var __baseUrls__ = default(string[]);
            var __baseUrls__b__ = false;
            var __policyVersions__ = default(global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]);
            var __policyVersions__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __policyId__ = default(string);
            var __policyId__b__ = false;
            var __isMandatory__ = default(bool);
            var __isMandatory__b__ = false;
            var __isAccepted__ = default(bool);
            var __isAccepted__b__ = false;

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
                        __readableId__ = reader.ReadString();
                        __readableId__b__ = true;
                        break;
                    case 1:
                        __policyName__ = reader.ReadString();
                        __policyName__b__ = true;
                        break;
                    case 2:
                        __policyType__ = reader.ReadString();
                        __policyType__b__ = true;
                        break;
                    case 3:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 4:
                        __countryCode__ = reader.ReadString();
                        __countryCode__b__ = true;
                        break;
                    case 5:
                        __countryGroupCode__ = reader.ReadString();
                        __countryGroupCode__b__ = true;
                        break;
                    case 6:
                        __baseUrls__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __baseUrls__b__ = true;
                        break;
                    case 7:
                        __policyVersions__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PolicyVersionWithLocalizedVersionObject[]>().Deserialize(ref reader, formatterResolver);
                        __policyVersions__b__ = true;
                        break;
                    case 8:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 9:
                        __policyId__ = reader.ReadString();
                        __policyId__b__ = true;
                        break;
                    case 10:
                        __isMandatory__ = reader.ReadBoolean();
                        __isMandatory__b__ = true;
                        break;
                    case 11:
                        __isAccepted__ = reader.ReadBoolean();
                        __isAccepted__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RetrieveUserEligibilitiesResponse();
            if(__readableId__b__) ____result.readableId = __readableId__;
            if(__policyName__b__) ____result.policyName = __policyName__;
            if(__policyType__b__) ____result.policyType = __policyType__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__countryCode__b__) ____result.countryCode = __countryCode__;
            if(__countryGroupCode__b__) ____result.countryGroupCode = __countryGroupCode__;
            if(__baseUrls__b__) ____result.baseUrls = __baseUrls__;
            if(__policyVersions__b__) ____result.policyVersions = __policyVersions__;
            if(__description__b__) ____result.description = __description__;
            if(__policyId__b__) ____result.policyId = __policyId__;
            if(__isMandatory__b__) ____result.isMandatory = __isMandatory__;
            if(__isAccepted__b__) ____result.isAccepted = __isAccepted__;

            return ____result;
        }
    }


    public sealed class UserProfileFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserProfile>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserProfileFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("firstName"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastName"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarSmallUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarLargeUrl"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("email"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("timeZone"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("customAttributes"), 12},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("firstName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarSmallUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarLargeUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("email"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timeZone"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("customAttributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserProfile value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.firstName);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.lastName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.avatarSmallUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.avatarUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.avatarLargeUrl);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.email);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.timeZone);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.customAttributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserProfile Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userId__ = default(string);
            var __userId__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __firstName__ = default(string);
            var __firstName__b__ = false;
            var __lastName__ = default(string);
            var __lastName__b__ = false;
            var __avatarSmallUrl__ = default(string);
            var __avatarSmallUrl__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;
            var __avatarLargeUrl__ = default(string);
            var __avatarLargeUrl__b__ = false;
            var __email__ = default(string);
            var __email__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __timeZone__ = default(string);
            var __timeZone__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __customAttributes__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __customAttributes__b__ = false;

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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __firstName__ = reader.ReadString();
                        __firstName__b__ = true;
                        break;
                    case 3:
                        __lastName__ = reader.ReadString();
                        __lastName__b__ = true;
                        break;
                    case 4:
                        __avatarSmallUrl__ = reader.ReadString();
                        __avatarSmallUrl__b__ = true;
                        break;
                    case 5:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    case 6:
                        __avatarLargeUrl__ = reader.ReadString();
                        __avatarLargeUrl__b__ = true;
                        break;
                    case 7:
                        __email__ = reader.ReadString();
                        __email__b__ = true;
                        break;
                    case 8:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 9:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 10:
                        __timeZone__ = reader.ReadString();
                        __timeZone__b__ = true;
                        break;
                    case 11:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 12:
                        __customAttributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __customAttributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserProfile();
            if(__userId__b__) ____result.userId = __userId__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__firstName__b__) ____result.firstName = __firstName__;
            if(__lastName__b__) ____result.lastName = __lastName__;
            if(__avatarSmallUrl__b__) ____result.avatarSmallUrl = __avatarSmallUrl__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;
            if(__avatarLargeUrl__b__) ____result.avatarLargeUrl = __avatarLargeUrl__;
            if(__email__b__) ____result.email = __email__;
            if(__status__b__) ____result.status = __status__;
            if(__language__b__) ____result.language = __language__;
            if(__timeZone__b__) ____result.timeZone = __timeZone__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__customAttributes__b__) ____result.customAttributes = __customAttributes__;

            return ____result;
        }
    }


    public sealed class PublicUserProfileFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PublicUserProfile>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PublicUserProfileFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("timeZone"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarSmallUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarLargeUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("customAttributes"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timeZone"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarSmallUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarLargeUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("customAttributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PublicUserProfile value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.timeZone);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.avatarSmallUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.avatarUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.avatarLargeUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.customAttributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PublicUserProfile Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userId__ = default(string);
            var __userId__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __timeZone__ = default(string);
            var __timeZone__b__ = false;
            var __avatarSmallUrl__ = default(string);
            var __avatarSmallUrl__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;
            var __avatarLargeUrl__ = default(string);
            var __avatarLargeUrl__b__ = false;
            var __customAttributes__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __customAttributes__b__ = false;

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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __timeZone__ = reader.ReadString();
                        __timeZone__b__ = true;
                        break;
                    case 3:
                        __avatarSmallUrl__ = reader.ReadString();
                        __avatarSmallUrl__b__ = true;
                        break;
                    case 4:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    case 5:
                        __avatarLargeUrl__ = reader.ReadString();
                        __avatarLargeUrl__b__ = true;
                        break;
                    case 6:
                        __customAttributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __customAttributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PublicUserProfile();
            if(__userId__b__) ____result.userId = __userId__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__timeZone__b__) ____result.timeZone = __timeZone__;
            if(__avatarSmallUrl__b__) ____result.avatarSmallUrl = __avatarSmallUrl__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;
            if(__avatarLargeUrl__b__) ____result.avatarLargeUrl = __avatarLargeUrl__;
            if(__customAttributes__b__) ____result.customAttributes = __customAttributes__;

            return ____result;
        }
    }


    public sealed class CreateUserProfileRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CreateUserProfileRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreateUserProfileRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("firstName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarSmallUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarLargeUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("timeZone"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("customAttributes"), 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("firstName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarSmallUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarLargeUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timeZone"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("customAttributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CreateUserProfileRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.firstName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.lastName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.avatarSmallUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.avatarUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.avatarLargeUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.timeZone);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.customAttributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CreateUserProfileRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __firstName__ = default(string);
            var __firstName__b__ = false;
            var __lastName__ = default(string);
            var __lastName__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __avatarSmallUrl__ = default(string);
            var __avatarSmallUrl__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;
            var __avatarLargeUrl__ = default(string);
            var __avatarLargeUrl__b__ = false;
            var __timeZone__ = default(string);
            var __timeZone__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __customAttributes__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __customAttributes__b__ = false;

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
                        __firstName__ = reader.ReadString();
                        __firstName__b__ = true;
                        break;
                    case 1:
                        __lastName__ = reader.ReadString();
                        __lastName__b__ = true;
                        break;
                    case 2:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 3:
                        __avatarSmallUrl__ = reader.ReadString();
                        __avatarSmallUrl__b__ = true;
                        break;
                    case 4:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    case 5:
                        __avatarLargeUrl__ = reader.ReadString();
                        __avatarLargeUrl__b__ = true;
                        break;
                    case 6:
                        __timeZone__ = reader.ReadString();
                        __timeZone__b__ = true;
                        break;
                    case 7:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 8:
                        __customAttributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __customAttributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.CreateUserProfileRequest();
            if(__firstName__b__) ____result.firstName = __firstName__;
            if(__lastName__b__) ____result.lastName = __lastName__;
            if(__language__b__) ____result.language = __language__;
            if(__avatarSmallUrl__b__) ____result.avatarSmallUrl = __avatarSmallUrl__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;
            if(__avatarLargeUrl__b__) ____result.avatarLargeUrl = __avatarLargeUrl__;
            if(__timeZone__b__) ____result.timeZone = __timeZone__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__customAttributes__b__) ____result.customAttributes = __customAttributes__;

            return ____result;
        }
    }


    public sealed class UpdateUserProfileRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UpdateUserProfileRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UpdateUserProfileRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("firstName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarSmallUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarLargeUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("timeZone"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("customAttributes"), 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("firstName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarSmallUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarLargeUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timeZone"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("customAttributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UpdateUserProfileRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.firstName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.lastName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.avatarSmallUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.avatarUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.avatarLargeUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.timeZone);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.customAttributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UpdateUserProfileRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __firstName__ = default(string);
            var __firstName__b__ = false;
            var __lastName__ = default(string);
            var __lastName__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __avatarSmallUrl__ = default(string);
            var __avatarSmallUrl__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;
            var __avatarLargeUrl__ = default(string);
            var __avatarLargeUrl__b__ = false;
            var __timeZone__ = default(string);
            var __timeZone__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __customAttributes__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __customAttributes__b__ = false;

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
                        __firstName__ = reader.ReadString();
                        __firstName__b__ = true;
                        break;
                    case 1:
                        __lastName__ = reader.ReadString();
                        __lastName__b__ = true;
                        break;
                    case 2:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 3:
                        __avatarSmallUrl__ = reader.ReadString();
                        __avatarSmallUrl__b__ = true;
                        break;
                    case 4:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    case 5:
                        __avatarLargeUrl__ = reader.ReadString();
                        __avatarLargeUrl__b__ = true;
                        break;
                    case 6:
                        __timeZone__ = reader.ReadString();
                        __timeZone__b__ = true;
                        break;
                    case 7:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 8:
                        __customAttributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __customAttributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UpdateUserProfileRequest();
            if(__firstName__b__) ____result.firstName = __firstName__;
            if(__lastName__b__) ____result.lastName = __lastName__;
            if(__language__b__) ____result.language = __language__;
            if(__avatarSmallUrl__b__) ____result.avatarSmallUrl = __avatarSmallUrl__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;
            if(__avatarLargeUrl__b__) ____result.avatarLargeUrl = __avatarLargeUrl__;
            if(__timeZone__b__) ____result.timeZone = __timeZone__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__customAttributes__b__) ____result.customAttributes = __customAttributes__;

            return ____result;
        }
    }


    public sealed class UserRecordFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserRecord>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserRecordFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("key"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_id"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("key"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("user_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserRecord value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.key);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.user_id);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.value, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserRecord Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __key__ = default(string);
            var __key__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __user_id__ = default(string);
            var __user_id__b__ = false;
            var __value__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __value__b__ = false;

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
                        __key__ = reader.ReadString();
                        __key__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __user_id__ = reader.ReadString();
                        __user_id__b__ = true;
                        break;
                    case 3:
                        __value__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __value__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserRecord();
            if(__key__b__) ____result.key = __key__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__user_id__b__) ____result.user_id = __user_id__;
            if(__value__b__) ____result.value = __value__;

            return ____result;
        }
    }


    public sealed class GameRecordFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GameRecord>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GameRecordFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("key"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("key"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GameRecord value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.key);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.value, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GameRecord Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __key__ = default(string);
            var __key__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __value__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __value__b__ = false;

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
                        __key__ = reader.ReadString();
                        __key__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __value__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __value__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.GameRecord();
            if(__key__b__) ____result.key = __key__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__value__b__) ____result.value = __value__;

            return ____result;
        }
    }


    public sealed class SlotFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Slot>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SlotFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("checksum"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("customAttribute"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateAccessed"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateCreated"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateModified"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("label"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("mimeType"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespaceId"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("originalName"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("slotId"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("storedName"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 13},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("checksum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("customAttribute"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateAccessed"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateCreated"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateModified"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("label"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mimeType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespaceId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("originalName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("slotId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("storedName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Slot value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.checksum);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.customAttribute);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.dateAccessed, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.dateCreated, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.dateModified, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.label);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.mimeType);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.namespaceId);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.originalName);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.slotId);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.storedName);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Slot Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __checksum__ = default(string);
            var __checksum__b__ = false;
            var __customAttribute__ = default(string);
            var __customAttribute__b__ = false;
            var __dateAccessed__ = default(global::System.DateTime);
            var __dateAccessed__b__ = false;
            var __dateCreated__ = default(global::System.DateTime);
            var __dateCreated__b__ = false;
            var __dateModified__ = default(global::System.DateTime);
            var __dateModified__b__ = false;
            var __label__ = default(string);
            var __label__b__ = false;
            var __mimeType__ = default(string);
            var __mimeType__b__ = false;
            var __namespaceId__ = default(string);
            var __namespaceId__b__ = false;
            var __originalName__ = default(string);
            var __originalName__b__ = false;
            var __slotId__ = default(string);
            var __slotId__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __storedName__ = default(string);
            var __storedName__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
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
                        __checksum__ = reader.ReadString();
                        __checksum__b__ = true;
                        break;
                    case 1:
                        __customAttribute__ = reader.ReadString();
                        __customAttribute__b__ = true;
                        break;
                    case 2:
                        __dateAccessed__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __dateAccessed__b__ = true;
                        break;
                    case 3:
                        __dateCreated__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __dateCreated__b__ = true;
                        break;
                    case 4:
                        __dateModified__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __dateModified__b__ = true;
                        break;
                    case 5:
                        __label__ = reader.ReadString();
                        __label__b__ = true;
                        break;
                    case 6:
                        __mimeType__ = reader.ReadString();
                        __mimeType__b__ = true;
                        break;
                    case 7:
                        __namespaceId__ = reader.ReadString();
                        __namespaceId__b__ = true;
                        break;
                    case 8:
                        __originalName__ = reader.ReadString();
                        __originalName__b__ = true;
                        break;
                    case 9:
                        __slotId__ = reader.ReadString();
                        __slotId__b__ = true;
                        break;
                    case 10:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 11:
                        __storedName__ = reader.ReadString();
                        __storedName__b__ = true;
                        break;
                    case 12:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 13:
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

            var ____result = new global::AccelByte.Models.Slot();
            if(__checksum__b__) ____result.checksum = __checksum__;
            if(__customAttribute__b__) ____result.customAttribute = __customAttribute__;
            if(__dateAccessed__b__) ____result.dateAccessed = __dateAccessed__;
            if(__dateCreated__b__) ____result.dateCreated = __dateCreated__;
            if(__dateModified__b__) ____result.dateModified = __dateModified__;
            if(__label__b__) ____result.label = __label__;
            if(__mimeType__b__) ____result.mimeType = __mimeType__;
            if(__namespaceId__b__) ____result.namespaceId = __namespaceId__;
            if(__originalName__b__) ____result.originalName = __originalName__;
            if(__slotId__b__) ____result.slotId = __slotId__;
            if(__status__b__) ____result.status = __status__;
            if(__storedName__b__) ____result.storedName = __storedName__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class UpdateMedataRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UpdateMedataRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UpdateMedataRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("label"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("customAttribute"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("label"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("customAttribute"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UpdateMedataRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.label);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.customAttribute);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UpdateMedataRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __label__ = default(string);
            var __label__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __customAttribute__ = default(string);
            var __customAttribute__b__ = false;

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
                        __label__ = reader.ReadString();
                        __label__b__ = true;
                        break;
                    case 1:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 2:
                        __customAttribute__ = reader.ReadString();
                        __customAttribute__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UpdateMedataRequest();
            if(__label__b__) ____result.label = __label__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__customAttribute__b__) ____result.customAttribute = __customAttribute__;

            return ____result;
        }
    }


    public sealed class ConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Config>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("UseSessionManagement"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("BaseUrl"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ApiBaseUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("NonApiBaseUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LoginServerUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("IamServerUrl"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PlatformServerUrl"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("BasicServerUrl"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LobbyServerUrl"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CloudStorageServerUrl"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameProfileServerUrl"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("StatisticServerUrl"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("QosManagerServerUrl"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("AgreementServerUrl"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LeaderboardServerUrl"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CloudSaveServerUrl"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameTelemetryServerUrl"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("AchievementServerUrl"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientId"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientSecret"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("RedirectUri"), 21},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("UseSessionManagement"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("BaseUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ApiBaseUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("NonApiBaseUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LoginServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("IamServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PlatformServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("BasicServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LobbyServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CloudStorageServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameProfileServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("StatisticServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("QosManagerServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("AgreementServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LeaderboardServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CloudSaveServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameTelemetryServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("AchievementServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ClientId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ClientSecret"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("RedirectUri"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Config value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteBoolean(value.UseSessionManagement);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.BaseUrl);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.ApiBaseUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.NonApiBaseUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.LoginServerUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.IamServerUrl);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.PlatformServerUrl);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.BasicServerUrl);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.LobbyServerUrl);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.CloudStorageServerUrl);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.GameProfileServerUrl);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.StatisticServerUrl);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.QosManagerServerUrl);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.AgreementServerUrl);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.LeaderboardServerUrl);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.CloudSaveServerUrl);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.GameTelemetryServerUrl);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.AchievementServerUrl);
            writer.WriteRaw(this.____stringByteKeys[19]);
            writer.WriteString(value.ClientId);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteString(value.ClientSecret);
            writer.WriteRaw(this.____stringByteKeys[21]);
            writer.WriteString(value.RedirectUri);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Config Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __UseSessionManagement__ = default(bool);
            var __UseSessionManagement__b__ = false;
            var __BaseUrl__ = default(string);
            var __BaseUrl__b__ = false;
            var __ApiBaseUrl__ = default(string);
            var __ApiBaseUrl__b__ = false;
            var __NonApiBaseUrl__ = default(string);
            var __NonApiBaseUrl__b__ = false;
            var __LoginServerUrl__ = default(string);
            var __LoginServerUrl__b__ = false;
            var __IamServerUrl__ = default(string);
            var __IamServerUrl__b__ = false;
            var __PlatformServerUrl__ = default(string);
            var __PlatformServerUrl__b__ = false;
            var __BasicServerUrl__ = default(string);
            var __BasicServerUrl__b__ = false;
            var __LobbyServerUrl__ = default(string);
            var __LobbyServerUrl__b__ = false;
            var __CloudStorageServerUrl__ = default(string);
            var __CloudStorageServerUrl__b__ = false;
            var __GameProfileServerUrl__ = default(string);
            var __GameProfileServerUrl__b__ = false;
            var __StatisticServerUrl__ = default(string);
            var __StatisticServerUrl__b__ = false;
            var __QosManagerServerUrl__ = default(string);
            var __QosManagerServerUrl__b__ = false;
            var __AgreementServerUrl__ = default(string);
            var __AgreementServerUrl__b__ = false;
            var __LeaderboardServerUrl__ = default(string);
            var __LeaderboardServerUrl__b__ = false;
            var __CloudSaveServerUrl__ = default(string);
            var __CloudSaveServerUrl__b__ = false;
            var __GameTelemetryServerUrl__ = default(string);
            var __GameTelemetryServerUrl__b__ = false;
            var __AchievementServerUrl__ = default(string);
            var __AchievementServerUrl__b__ = false;
            var __ClientId__ = default(string);
            var __ClientId__b__ = false;
            var __ClientSecret__ = default(string);
            var __ClientSecret__b__ = false;
            var __RedirectUri__ = default(string);
            var __RedirectUri__b__ = false;

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
                        __UseSessionManagement__ = reader.ReadBoolean();
                        __UseSessionManagement__b__ = true;
                        break;
                    case 2:
                        __BaseUrl__ = reader.ReadString();
                        __BaseUrl__b__ = true;
                        break;
                    case 3:
                        __ApiBaseUrl__ = reader.ReadString();
                        __ApiBaseUrl__b__ = true;
                        break;
                    case 4:
                        __NonApiBaseUrl__ = reader.ReadString();
                        __NonApiBaseUrl__b__ = true;
                        break;
                    case 5:
                        __LoginServerUrl__ = reader.ReadString();
                        __LoginServerUrl__b__ = true;
                        break;
                    case 6:
                        __IamServerUrl__ = reader.ReadString();
                        __IamServerUrl__b__ = true;
                        break;
                    case 7:
                        __PlatformServerUrl__ = reader.ReadString();
                        __PlatformServerUrl__b__ = true;
                        break;
                    case 8:
                        __BasicServerUrl__ = reader.ReadString();
                        __BasicServerUrl__b__ = true;
                        break;
                    case 9:
                        __LobbyServerUrl__ = reader.ReadString();
                        __LobbyServerUrl__b__ = true;
                        break;
                    case 10:
                        __CloudStorageServerUrl__ = reader.ReadString();
                        __CloudStorageServerUrl__b__ = true;
                        break;
                    case 11:
                        __GameProfileServerUrl__ = reader.ReadString();
                        __GameProfileServerUrl__b__ = true;
                        break;
                    case 12:
                        __StatisticServerUrl__ = reader.ReadString();
                        __StatisticServerUrl__b__ = true;
                        break;
                    case 13:
                        __QosManagerServerUrl__ = reader.ReadString();
                        __QosManagerServerUrl__b__ = true;
                        break;
                    case 14:
                        __AgreementServerUrl__ = reader.ReadString();
                        __AgreementServerUrl__b__ = true;
                        break;
                    case 15:
                        __LeaderboardServerUrl__ = reader.ReadString();
                        __LeaderboardServerUrl__b__ = true;
                        break;
                    case 16:
                        __CloudSaveServerUrl__ = reader.ReadString();
                        __CloudSaveServerUrl__b__ = true;
                        break;
                    case 17:
                        __GameTelemetryServerUrl__ = reader.ReadString();
                        __GameTelemetryServerUrl__b__ = true;
                        break;
                    case 18:
                        __AchievementServerUrl__ = reader.ReadString();
                        __AchievementServerUrl__b__ = true;
                        break;
                    case 19:
                        __ClientId__ = reader.ReadString();
                        __ClientId__b__ = true;
                        break;
                    case 20:
                        __ClientSecret__ = reader.ReadString();
                        __ClientSecret__b__ = true;
                        break;
                    case 21:
                        __RedirectUri__ = reader.ReadString();
                        __RedirectUri__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Config();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__UseSessionManagement__b__) ____result.UseSessionManagement = __UseSessionManagement__;
            if(__BaseUrl__b__) ____result.BaseUrl = __BaseUrl__;
            if(__ApiBaseUrl__b__) ____result.ApiBaseUrl = __ApiBaseUrl__;
            if(__NonApiBaseUrl__b__) ____result.NonApiBaseUrl = __NonApiBaseUrl__;
            if(__LoginServerUrl__b__) ____result.LoginServerUrl = __LoginServerUrl__;
            if(__IamServerUrl__b__) ____result.IamServerUrl = __IamServerUrl__;
            if(__PlatformServerUrl__b__) ____result.PlatformServerUrl = __PlatformServerUrl__;
            if(__BasicServerUrl__b__) ____result.BasicServerUrl = __BasicServerUrl__;
            if(__LobbyServerUrl__b__) ____result.LobbyServerUrl = __LobbyServerUrl__;
            if(__CloudStorageServerUrl__b__) ____result.CloudStorageServerUrl = __CloudStorageServerUrl__;
            if(__GameProfileServerUrl__b__) ____result.GameProfileServerUrl = __GameProfileServerUrl__;
            if(__StatisticServerUrl__b__) ____result.StatisticServerUrl = __StatisticServerUrl__;
            if(__QosManagerServerUrl__b__) ____result.QosManagerServerUrl = __QosManagerServerUrl__;
            if(__AgreementServerUrl__b__) ____result.AgreementServerUrl = __AgreementServerUrl__;
            if(__LeaderboardServerUrl__b__) ____result.LeaderboardServerUrl = __LeaderboardServerUrl__;
            if(__CloudSaveServerUrl__b__) ____result.CloudSaveServerUrl = __CloudSaveServerUrl__;
            if(__GameTelemetryServerUrl__b__) ____result.GameTelemetryServerUrl = __GameTelemetryServerUrl__;
            if(__AchievementServerUrl__b__) ____result.AchievementServerUrl = __AchievementServerUrl__;
            if(__ClientId__b__) ____result.ClientId = __ClientId__;
            if(__ClientSecret__b__) ____result.ClientSecret = __ClientSecret__;
            if(__RedirectUri__b__) ____result.RedirectUri = __RedirectUri__;

            return ____result;
        }
    }


    public sealed class CurrencySummaryFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CurrencySummary>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CurrencySummaryFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CurrencySummary value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

        public global::AccelByte.Models.CurrencySummary Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

            var ____result = new global::AccelByte.Models.CurrencySummary();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__decimals__b__) ____result.decimals = __decimals__;

            return ____result;
        }
    }


    public sealed class BalanceInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BalanceInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BalanceInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("walletId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("balance"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("balanceSource"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("walletId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("balance"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("balanceSource"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BalanceInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.walletId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.balance);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.balanceSource);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.status);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.BalanceInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __walletId__ = default(string);
            var __walletId__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __balance__ = default(int);
            var __balance__b__ = false;
            var __balanceSource__ = default(string);
            var __balanceSource__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;

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
                        __walletId__ = reader.ReadString();
                        __walletId__b__ = true;
                        break;
                    case 2:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 3:
                        __balance__ = reader.ReadInt32();
                        __balance__b__ = true;
                        break;
                    case 4:
                        __balanceSource__ = reader.ReadString();
                        __balanceSource__b__ = true;
                        break;
                    case 5:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 6:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 7:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.BalanceInfo();
            if(__id__b__) ____result.id = __id__;
            if(__walletId__b__) ____result.walletId = __walletId__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__balance__b__) ____result.balance = __balance__;
            if(__balanceSource__b__) ____result.balanceSource = __balanceSource__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__status__b__) ____result.status = __status__;

            return ____result;
        }
    }


    public sealed class WalletInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.WalletInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public WalletInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("balance"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("balance"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.WalletInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.balance);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Serialize(ref writer, value.status, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.WalletInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __balance__ = default(int);
            var __balance__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __status__ = default(global::AccelByte.Models.ItemStatus);
            var __status__b__ = false;

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
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 3:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 4:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 5:
                        __balance__ = reader.ReadInt32();
                        __balance__b__ = true;
                        break;
                    case 6:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 7:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 8:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.WalletInfo();
            if(__id__b__) ____result.id = __id__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__balance__b__) ____result.balance = __balance__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__status__b__) ____result.status = __status__;

            return ____result;
        }
    }


    public sealed class WalletTransactionInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.WalletTransactionInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public WalletTransactionInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("walletId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("amount"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reason"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("operator"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("walletAction"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("balanceSource"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("walletId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("amount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("operator"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("walletAction"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("balanceSource"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.WalletTransactionInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.walletId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.amount);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.reason);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.Operator);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.walletAction);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.balanceSource);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.WalletTransactionInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __walletId__ = default(string);
            var __walletId__b__ = false;
            var __amount__ = default(int);
            var __amount__b__ = false;
            var __reason__ = default(string);
            var __reason__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __Operator__ = default(string);
            var __Operator__b__ = false;
            var __walletAction__ = default(string);
            var __walletAction__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __balanceSource__ = default(string);
            var __balanceSource__b__ = false;
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
                        __walletId__ = reader.ReadString();
                        __walletId__b__ = true;
                        break;
                    case 1:
                        __amount__ = reader.ReadInt32();
                        __amount__b__ = true;
                        break;
                    case 2:
                        __reason__ = reader.ReadString();
                        __reason__b__ = true;
                        break;
                    case 3:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 4:
                        __Operator__ = reader.ReadString();
                        __Operator__b__ = true;
                        break;
                    case 5:
                        __walletAction__ = reader.ReadString();
                        __walletAction__b__ = true;
                        break;
                    case 6:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 7:
                        __balanceSource__ = reader.ReadString();
                        __balanceSource__b__ = true;
                        break;
                    case 8:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 9:
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

            var ____result = new global::AccelByte.Models.WalletTransactionInfo();
            if(__walletId__b__) ____result.walletId = __walletId__;
            if(__amount__b__) ____result.amount = __amount__;
            if(__reason__b__) ____result.reason = __reason__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__Operator__b__) ____result.Operator = __Operator__;
            if(__walletAction__b__) ____result.walletAction = __walletAction__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__balanceSource__b__) ____result.balanceSource = __balanceSource__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class WalletTransactionPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.WalletTransactionPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public WalletTransactionPagingSlicedResultFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.WalletTransactionPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.WalletTransactionInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.WalletTransactionPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.WalletTransactionInfo[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.WalletTransactionInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.WalletTransactionPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class CreditUserWalletRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CreditUserWalletRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreditUserWalletRequestFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CreditUserWalletRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.amount);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CreditUserWalletSource>().Serialize(ref writer, value.source, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.reason);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CreditUserWalletRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __amount__ = default(int);
            var __amount__b__ = false;
            var __source__ = default(global::AccelByte.Models.CreditUserWalletSource);
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
                        __source__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CreditUserWalletSource>().Deserialize(ref reader, formatterResolver);
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

            var ____result = new global::AccelByte.Models.CreditUserWalletRequest();
            if(__amount__b__) ____result.amount = __amount__;
            if(__source__b__) ____result.source = __source__;
            if(__reason__b__) ____result.reason = __reason__;

            return ____result;
        }
    }


    public sealed class CategoryInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CategoryInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CategoryInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("parentCategoryPath"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("root"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parentCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("root"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CategoryInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.root);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CategoryInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            var __displayName__ = default(string);
            var __displayName__b__ = false;
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
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
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

            var ____result = new global::AccelByte.Models.CategoryInfo();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__parentCategoryPath__b__) ____result.parentCategoryPath = __parentCategoryPath__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__root__b__) ____result.root = __root__;

            return ____result;
        }
    }


    public sealed class RegionDataItemFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RegionDataItem>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RegionDataItemFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("price"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountPercentage"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountAmount"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountedPrice"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyNamespace"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("purchaseAt"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("expireAt"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountPurchaseAt"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountExpireAt"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountPercentage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountAmount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountedPrice"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("purchaseAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("expireAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountPurchaseAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountExpireAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RegionDataItem value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.price);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.discountPercentage);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.discountAmount);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.discountedPrice);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.currencyNamespace);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.purchaseAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.expireAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.discountPurchaseAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.discountExpireAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RegionDataItem Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __price__ = default(int);
            var __price__b__ = false;
            var __discountPercentage__ = default(int);
            var __discountPercentage__b__ = false;
            var __discountAmount__ = default(int);
            var __discountAmount__b__ = false;
            var __discountedPrice__ = default(int);
            var __discountedPrice__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __currencyNamespace__ = default(string);
            var __currencyNamespace__b__ = false;
            var __purchaseAt__ = default(global::System.DateTime);
            var __purchaseAt__b__ = false;
            var __expireAt__ = default(global::System.DateTime);
            var __expireAt__b__ = false;
            var __discountPurchaseAt__ = default(global::System.DateTime);
            var __discountPurchaseAt__b__ = false;
            var __discountExpireAt__ = default(global::System.DateTime);
            var __discountExpireAt__b__ = false;

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
                        __price__ = reader.ReadInt32();
                        __price__b__ = true;
                        break;
                    case 1:
                        __discountPercentage__ = reader.ReadInt32();
                        __discountPercentage__b__ = true;
                        break;
                    case 2:
                        __discountAmount__ = reader.ReadInt32();
                        __discountAmount__b__ = true;
                        break;
                    case 3:
                        __discountedPrice__ = reader.ReadInt32();
                        __discountedPrice__b__ = true;
                        break;
                    case 4:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 5:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 6:
                        __currencyNamespace__ = reader.ReadString();
                        __currencyNamespace__b__ = true;
                        break;
                    case 7:
                        __purchaseAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __purchaseAt__b__ = true;
                        break;
                    case 8:
                        __expireAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __expireAt__b__ = true;
                        break;
                    case 9:
                        __discountPurchaseAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __discountPurchaseAt__b__ = true;
                        break;
                    case 10:
                        __discountExpireAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __discountExpireAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RegionDataItem();
            if(__price__b__) ____result.price = __price__;
            if(__discountPercentage__b__) ____result.discountPercentage = __discountPercentage__;
            if(__discountAmount__b__) ____result.discountAmount = __discountAmount__;
            if(__discountedPrice__b__) ____result.discountedPrice = __discountedPrice__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__currencyNamespace__b__) ____result.currencyNamespace = __currencyNamespace__;
            if(__purchaseAt__b__) ____result.purchaseAt = __purchaseAt__;
            if(__expireAt__b__) ____result.expireAt = __expireAt__;
            if(__discountPurchaseAt__b__) ____result.discountPurchaseAt = __discountPurchaseAt__;
            if(__discountExpireAt__b__) ____result.discountExpireAt = __discountExpireAt__;

            return ____result;
        }
    }


    public sealed class ItemSnapshotFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ItemSnapshot>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ItemSnapshotFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseAppId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackable"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailUrl"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetItemId"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionDataItem"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("boothName"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 22},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseAppId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetItemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionDataItem"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("boothName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ItemSnapshot value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Serialize(ref writer, value.appType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.baseAppId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Serialize(ref writer, value.entitlementType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value.stackable);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Serialize(ref writer, value.itemType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.thumbnailUrl);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.targetItemId);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem>().Serialize(ref writer, value.regionDataItem, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[19]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteString(value.boothName);
            writer.WriteRaw(this.____stringByteKeys[21]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[22]);
            writer.WriteString(value.language);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ItemSnapshot Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(global::AccelByte.Models.EntitlementAppType);
            var __appType__b__ = false;
            var __baseAppId__ = default(string);
            var __baseAppId__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(global::AccelByte.Models.EntitlementType);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __stackable__ = default(bool);
            var __stackable__b__ = false;
            var __itemType__ = default(global::AccelByte.Models.ItemType);
            var __itemType__b__ = false;
            var __thumbnailUrl__ = default(string);
            var __thumbnailUrl__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetItemId__ = default(string);
            var __targetItemId__b__ = false;
            var __title__ = default(string);
            var __title__b__ = false;
            var __regionDataItem__ = default(global::AccelByte.Models.RegionDataItem);
            var __regionDataItem__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
            var __boothName__ = default(string);
            var __boothName__b__ = false;
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
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 2:
                        __appType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Deserialize(ref reader, formatterResolver);
                        __appType__b__ = true;
                        break;
                    case 3:
                        __baseAppId__ = reader.ReadString();
                        __baseAppId__b__ = true;
                        break;
                    case 4:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 5:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 6:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 7:
                        __entitlementType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Deserialize(ref reader, formatterResolver);
                        __entitlementType__b__ = true;
                        break;
                    case 8:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 9:
                        __stackable__ = reader.ReadBoolean();
                        __stackable__b__ = true;
                        break;
                    case 10:
                        __itemType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Deserialize(ref reader, formatterResolver);
                        __itemType__b__ = true;
                        break;
                    case 11:
                        __thumbnailUrl__ = reader.ReadString();
                        __thumbnailUrl__b__ = true;
                        break;
                    case 12:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 13:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 14:
                        __targetItemId__ = reader.ReadString();
                        __targetItemId__b__ = true;
                        break;
                    case 15:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 16:
                        __regionDataItem__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem>().Deserialize(ref reader, formatterResolver);
                        __regionDataItem__b__ = true;
                        break;
                    case 17:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 18:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 19:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 20:
                        __boothName__ = reader.ReadString();
                        __boothName__b__ = true;
                        break;
                    case 21:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 22:
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

            var ____result = new global::AccelByte.Models.ItemSnapshot();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__baseAppId__b__) ____result.baseAppId = __baseAppId__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__stackable__b__) ____result.stackable = __stackable__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__thumbnailUrl__b__) ____result.thumbnailUrl = __thumbnailUrl__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetItemId__b__) ____result.targetItemId = __targetItemId__;
            if(__title__b__) ____result.title = __title__;
            if(__regionDataItem__b__) ____result.regionDataItem = __regionDataItem__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
            if(__boothName__b__) ____result.boothName = __boothName__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;

            return ____result;
        }
    }


    public sealed class ItemCriteriaFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ItemCriteria>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ItemCriteriaFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseAppId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("offset"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("limit"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sortBy"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseAppId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("offset"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("limit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sortBy"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ItemCriteria value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Serialize(ref writer, value.itemType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Serialize(ref writer, value.appType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.baseAppId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value.offset, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value.limit, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.sortBy);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ItemCriteria Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemType__ = default(global::AccelByte.Models.ItemType);
            var __itemType__b__ = false;
            var __appType__ = default(global::AccelByte.Models.EntitlementAppType);
            var __appType__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __baseAppId__ = default(string);
            var __baseAppId__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __offset__ = default(int?);
            var __offset__b__ = false;
            var __limit__ = default(int?);
            var __limit__b__ = false;
            var __sortBy__ = default(string);
            var __sortBy__b__ = false;

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
                        __itemType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Deserialize(ref reader, formatterResolver);
                        __itemType__b__ = true;
                        break;
                    case 1:
                        __appType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Deserialize(ref reader, formatterResolver);
                        __appType__b__ = true;
                        break;
                    case 2:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 3:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 4:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 5:
                        __baseAppId__ = reader.ReadString();
                        __baseAppId__b__ = true;
                        break;
                    case 6:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 7:
                        __offset__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        __offset__b__ = true;
                        break;
                    case 8:
                        __limit__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        __limit__b__ = true;
                        break;
                    case 9:
                        __sortBy__ = reader.ReadString();
                        __sortBy__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ItemCriteria();
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__baseAppId__b__) ____result.baseAppId = __baseAppId__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__offset__b__) ____result.offset = __offset__;
            if(__limit__b__) ____result.limit = __limit__;
            if(__sortBy__b__) ____result.sortBy = __sortBy__;

            return ____result;
        }
    }


    public sealed class ImageFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Image>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ImageFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("as"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("caption"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("height"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("width"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("imageUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("smallImageUrl"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("as"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("caption"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("height"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("width"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("imageUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("smallImageUrl"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Image value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.As);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.caption);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.height);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.width);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.imageUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.smallImageUrl);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Image Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __As__ = default(string);
            var __As__b__ = false;
            var __caption__ = default(string);
            var __caption__b__ = false;
            var __height__ = default(int);
            var __height__b__ = false;
            var __width__ = default(int);
            var __width__b__ = false;
            var __imageUrl__ = default(string);
            var __imageUrl__b__ = false;
            var __smallImageUrl__ = default(string);
            var __smallImageUrl__b__ = false;

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
                        __As__ = reader.ReadString();
                        __As__b__ = true;
                        break;
                    case 1:
                        __caption__ = reader.ReadString();
                        __caption__b__ = true;
                        break;
                    case 2:
                        __height__ = reader.ReadInt32();
                        __height__b__ = true;
                        break;
                    case 3:
                        __width__ = reader.ReadInt32();
                        __width__b__ = true;
                        break;
                    case 4:
                        __imageUrl__ = reader.ReadString();
                        __imageUrl__b__ = true;
                        break;
                    case 5:
                        __smallImageUrl__ = reader.ReadString();
                        __smallImageUrl__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Image();
            if(__As__b__) ____result.As = __As__;
            if(__caption__b__) ____result.caption = __caption__;
            if(__height__b__) ____result.height = __height__;
            if(__width__b__) ____result.width = __width__;
            if(__imageUrl__b__) ____result.imageUrl = __imageUrl__;
            if(__smallImageUrl__b__) ____result.smallImageUrl = __smallImageUrl__;

            return ____result;
        }
    }


    public sealed class ItemInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ItemInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ItemInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("longDescription"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseAppId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackable"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetItemId"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("images"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailUrl"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 23},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 24},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 25},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("clazz"), 26},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("boothName"), 27},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayOrder"), 28},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ext"), 29},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 30},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 31},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 32},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 33},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localExt"), 34},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("longDescription"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseAppId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetItemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("images"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("clazz"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("boothName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayOrder"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ext"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localExt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ItemInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.longDescription);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Serialize(ref writer, value.appType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.baseAppId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Serialize(ref writer, value.entitlementType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteBoolean(value.stackable);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Serialize(ref writer, value.status, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Serialize(ref writer, value.itemType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.targetItemId);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Serialize(ref writer, value.images, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteString(value.thumbnailUrl);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem[]>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[23]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[24]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[25]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[26]);
            writer.WriteString(value.clazz);
            writer.WriteRaw(this.____stringByteKeys[27]);
            writer.WriteString(value.boothName);
            writer.WriteRaw(this.____stringByteKeys[28]);
            writer.WriteInt32(value.displayOrder);
            writer.WriteRaw(this.____stringByteKeys[29]);
            writer.WriteString(value.ext);
            writer.WriteRaw(this.____stringByteKeys[30]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[31]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[32]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[33]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[34]);
            writer.WriteString(value.localExt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ItemInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __longDescription__ = default(string);
            var __longDescription__b__ = false;
            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(global::AccelByte.Models.EntitlementAppType);
            var __appType__b__ = false;
            var __baseAppId__ = default(string);
            var __baseAppId__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(global::AccelByte.Models.EntitlementType);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __stackable__ = default(bool);
            var __stackable__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __status__ = default(global::AccelByte.Models.ItemStatus);
            var __status__b__ = false;
            var __itemType__ = default(global::AccelByte.Models.ItemType);
            var __itemType__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetItemId__ = default(string);
            var __targetItemId__b__ = false;
            var __images__ = default(global::AccelByte.Models.Image[]);
            var __images__b__ = false;
            var __thumbnailUrl__ = default(string);
            var __thumbnailUrl__b__ = false;
            var __regionData__ = default(global::AccelByte.Models.RegionDataItem[]);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
            var __clazz__ = default(string);
            var __clazz__b__ = false;
            var __boothName__ = default(string);
            var __boothName__b__ = false;
            var __displayOrder__ = default(int);
            var __displayOrder__b__ = false;
            var __ext__ = default(string);
            var __ext__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __localExt__ = default(string);
            var __localExt__b__ = false;

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
                        __longDescription__ = reader.ReadString();
                        __longDescription__b__ = true;
                        break;
                    case 3:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 4:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 5:
                        __appType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Deserialize(ref reader, formatterResolver);
                        __appType__b__ = true;
                        break;
                    case 6:
                        __baseAppId__ = reader.ReadString();
                        __baseAppId__b__ = true;
                        break;
                    case 7:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 8:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 9:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 10:
                        __entitlementType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Deserialize(ref reader, formatterResolver);
                        __entitlementType__b__ = true;
                        break;
                    case 11:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 12:
                        __stackable__ = reader.ReadBoolean();
                        __stackable__b__ = true;
                        break;
                    case 13:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 14:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    case 15:
                        __itemType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Deserialize(ref reader, formatterResolver);
                        __itemType__b__ = true;
                        break;
                    case 16:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 17:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 18:
                        __targetItemId__ = reader.ReadString();
                        __targetItemId__b__ = true;
                        break;
                    case 19:
                        __images__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Deserialize(ref reader, formatterResolver);
                        __images__b__ = true;
                        break;
                    case 20:
                        __thumbnailUrl__ = reader.ReadString();
                        __thumbnailUrl__b__ = true;
                        break;
                    case 21:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem[]>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 22:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 23:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 24:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 25:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 26:
                        __clazz__ = reader.ReadString();
                        __clazz__b__ = true;
                        break;
                    case 27:
                        __boothName__ = reader.ReadString();
                        __boothName__b__ = true;
                        break;
                    case 28:
                        __displayOrder__ = reader.ReadInt32();
                        __displayOrder__b__ = true;
                        break;
                    case 29:
                        __ext__ = reader.ReadString();
                        __ext__b__ = true;
                        break;
                    case 30:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 31:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 32:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 33:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 34:
                        __localExt__ = reader.ReadString();
                        __localExt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ItemInfo();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__longDescription__b__) ____result.longDescription = __longDescription__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__baseAppId__b__) ____result.baseAppId = __baseAppId__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__stackable__b__) ____result.stackable = __stackable__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__status__b__) ____result.status = __status__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetItemId__b__) ____result.targetItemId = __targetItemId__;
            if(__images__b__) ____result.images = __images__;
            if(__thumbnailUrl__b__) ____result.thumbnailUrl = __thumbnailUrl__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
            if(__clazz__b__) ____result.clazz = __clazz__;
            if(__boothName__b__) ____result.boothName = __boothName__;
            if(__displayOrder__b__) ____result.displayOrder = __displayOrder__;
            if(__ext__b__) ____result.ext = __ext__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__localExt__b__) ____result.localExt = __localExt__;

            return ____result;
        }
    }


    public sealed class PopulatedItemInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PopulatedItemInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PopulatedItemInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("longDescription"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("baseAppId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackable"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetItemId"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("images"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailUrl"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 23},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 24},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 25},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("clazz"), 26},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("boothName"), 27},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayOrder"), 28},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ext"), 29},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 30},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 31},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 32},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 33},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("items"), 34},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localExt"), 35},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("longDescription"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("baseAppId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetItemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("images"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("clazz"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("boothName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayOrder"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ext"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("items"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localExt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PopulatedItemInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.longDescription);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Serialize(ref writer, value.appType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.baseAppId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Serialize(ref writer, value.entitlementType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteBoolean(value.stackable);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Serialize(ref writer, value.status, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Serialize(ref writer, value.itemType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.targetItemId);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Serialize(ref writer, value.images, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteString(value.thumbnailUrl);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem[]>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[23]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[24]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[25]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[26]);
            writer.WriteString(value.clazz);
            writer.WriteRaw(this.____stringByteKeys[27]);
            writer.WriteString(value.boothName);
            writer.WriteRaw(this.____stringByteKeys[28]);
            writer.WriteInt32(value.displayOrder);
            writer.WriteRaw(this.____stringByteKeys[29]);
            writer.WriteString(value.ext);
            writer.WriteRaw(this.____stringByteKeys[30]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[31]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[32]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[33]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[34]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemInfo[]>().Serialize(ref writer, value.items, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[35]);
            writer.WriteString(value.localExt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PopulatedItemInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __longDescription__ = default(string);
            var __longDescription__b__ = false;
            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(global::AccelByte.Models.EntitlementAppType);
            var __appType__b__ = false;
            var __baseAppId__ = default(string);
            var __baseAppId__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(global::AccelByte.Models.EntitlementType);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __stackable__ = default(bool);
            var __stackable__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __status__ = default(global::AccelByte.Models.ItemStatus);
            var __status__b__ = false;
            var __itemType__ = default(global::AccelByte.Models.ItemType);
            var __itemType__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetItemId__ = default(string);
            var __targetItemId__b__ = false;
            var __images__ = default(global::AccelByte.Models.Image[]);
            var __images__b__ = false;
            var __thumbnailUrl__ = default(string);
            var __thumbnailUrl__b__ = false;
            var __regionData__ = default(global::AccelByte.Models.RegionDataItem[]);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
            var __clazz__ = default(string);
            var __clazz__b__ = false;
            var __boothName__ = default(string);
            var __boothName__b__ = false;
            var __displayOrder__ = default(int);
            var __displayOrder__b__ = false;
            var __ext__ = default(string);
            var __ext__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __items__ = default(global::AccelByte.Models.ItemInfo[]);
            var __items__b__ = false;
            var __localExt__ = default(string);
            var __localExt__b__ = false;

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
                        __longDescription__ = reader.ReadString();
                        __longDescription__b__ = true;
                        break;
                    case 3:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 4:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 5:
                        __appType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Deserialize(ref reader, formatterResolver);
                        __appType__b__ = true;
                        break;
                    case 6:
                        __baseAppId__ = reader.ReadString();
                        __baseAppId__b__ = true;
                        break;
                    case 7:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 8:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 9:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 10:
                        __entitlementType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Deserialize(ref reader, formatterResolver);
                        __entitlementType__b__ = true;
                        break;
                    case 11:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 12:
                        __stackable__ = reader.ReadBoolean();
                        __stackable__b__ = true;
                        break;
                    case 13:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 14:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    case 15:
                        __itemType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Deserialize(ref reader, formatterResolver);
                        __itemType__b__ = true;
                        break;
                    case 16:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 17:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 18:
                        __targetItemId__ = reader.ReadString();
                        __targetItemId__b__ = true;
                        break;
                    case 19:
                        __images__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Deserialize(ref reader, formatterResolver);
                        __images__b__ = true;
                        break;
                    case 20:
                        __thumbnailUrl__ = reader.ReadString();
                        __thumbnailUrl__b__ = true;
                        break;
                    case 21:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem[]>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 22:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 23:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 24:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 25:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 26:
                        __clazz__ = reader.ReadString();
                        __clazz__b__ = true;
                        break;
                    case 27:
                        __boothName__ = reader.ReadString();
                        __boothName__b__ = true;
                        break;
                    case 28:
                        __displayOrder__ = reader.ReadInt32();
                        __displayOrder__b__ = true;
                        break;
                    case 29:
                        __ext__ = reader.ReadString();
                        __ext__b__ = true;
                        break;
                    case 30:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 31:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 32:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 33:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 34:
                        __items__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemInfo[]>().Deserialize(ref reader, formatterResolver);
                        __items__b__ = true;
                        break;
                    case 35:
                        __localExt__ = reader.ReadString();
                        __localExt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PopulatedItemInfo();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__longDescription__b__) ____result.longDescription = __longDescription__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__baseAppId__b__) ____result.baseAppId = __baseAppId__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__stackable__b__) ____result.stackable = __stackable__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__status__b__) ____result.status = __status__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetItemId__b__) ____result.targetItemId = __targetItemId__;
            if(__images__b__) ____result.images = __images__;
            if(__thumbnailUrl__b__) ____result.thumbnailUrl = __thumbnailUrl__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
            if(__clazz__b__) ____result.clazz = __clazz__;
            if(__boothName__b__) ____result.boothName = __boothName__;
            if(__displayOrder__b__) ____result.displayOrder = __displayOrder__;
            if(__ext__b__) ____result.ext = __ext__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__items__b__) ____result.items = __items__;
            if(__localExt__b__) ____result.localExt = __localExt__;

            return ____result;
        }
    }


    public sealed class ItemPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ItemPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ItemPagingSlicedResultFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ItemPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ItemPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.ItemInfo[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ItemPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class PaymentUrlFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PaymentUrl>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PaymentUrlFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentProvider"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentUrl"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentToken"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("returnUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentType"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("paymentProvider"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentToken"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("returnUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentType"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PaymentUrl value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.paymentProvider);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.paymentUrl);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.paymentToken);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.returnUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.paymentType);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PaymentUrl Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __paymentProvider__ = default(string);
            var __paymentProvider__b__ = false;
            var __paymentUrl__ = default(string);
            var __paymentUrl__b__ = false;
            var __paymentToken__ = default(string);
            var __paymentToken__b__ = false;
            var __returnUrl__ = default(string);
            var __returnUrl__b__ = false;
            var __paymentType__ = default(string);
            var __paymentType__b__ = false;

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
                        __paymentProvider__ = reader.ReadString();
                        __paymentProvider__b__ = true;
                        break;
                    case 1:
                        __paymentUrl__ = reader.ReadString();
                        __paymentUrl__b__ = true;
                        break;
                    case 2:
                        __paymentToken__ = reader.ReadString();
                        __paymentToken__b__ = true;
                        break;
                    case 3:
                        __returnUrl__ = reader.ReadString();
                        __returnUrl__b__ = true;
                        break;
                    case 4:
                        __paymentType__ = reader.ReadString();
                        __paymentType__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PaymentUrl();
            if(__paymentProvider__b__) ____result.paymentProvider = __paymentProvider__;
            if(__paymentUrl__b__) ____result.paymentUrl = __paymentUrl__;
            if(__paymentToken__b__) ____result.paymentToken = __paymentToken__;
            if(__returnUrl__b__) ____result.returnUrl = __returnUrl__;
            if(__paymentType__b__) ____result.paymentType = __paymentType__;

            return ____result;
        }
    }


    public sealed class PriceFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Price>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PriceFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("value"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Price value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.value);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Price Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __value__ = default(int);
            var __value__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
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
                        __value__ = reader.ReadInt32();
                        __value__b__ = true;
                        break;
                    case 1:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 2:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 3:
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

            var ____result = new global::AccelByte.Models.Price();
            if(__value__b__) ____result.value = __value__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;

            return ____result;
        }
    }


    public sealed class OrderHistoryInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OrderHistoryInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OrderHistoryInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("orderNo"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("operator"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("action"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reason"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("orderNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("operator"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("action"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OrderHistoryInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.orderNo);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Operator);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.action);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.reason);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OrderHistoryInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __orderNo__ = default(string);
            var __orderNo__b__ = false;
            var __Operator__ = default(string);
            var __Operator__b__ = false;
            var __action__ = default(string);
            var __action__b__ = false;
            var __reason__ = default(string);
            var __reason__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
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
                        __orderNo__ = reader.ReadString();
                        __orderNo__b__ = true;
                        break;
                    case 1:
                        __Operator__ = reader.ReadString();
                        __Operator__b__ = true;
                        break;
                    case 2:
                        __action__ = reader.ReadString();
                        __action__b__ = true;
                        break;
                    case 3:
                        __reason__ = reader.ReadString();
                        __reason__b__ = true;
                        break;
                    case 4:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 5:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 6:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 7:
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

            var ____result = new global::AccelByte.Models.OrderHistoryInfo();
            if(__orderNo__b__) ____result.orderNo = __orderNo__;
            if(__Operator__b__) ____result.Operator = __Operator__;
            if(__action__b__) ____result.action = __action__;
            if(__reason__b__) ____result.reason = __reason__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class OrderInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OrderInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OrderInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("orderNo"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentOrderNo"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sandbox"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("price"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountedPrice"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentProvider"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("vat"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("salesTax"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentProviderFee"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentMethodFee"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currency"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentStationUrl"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemSnapshot"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statusReason"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdTime"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("chargedTime"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("fulfilledTime"), 23},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("refundedTime"), 24},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("chargebackTime"), 25},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("chargebackReversedTime"), 26},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 27},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 28},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("orderNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentOrderNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sandbox"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountedPrice"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentProvider"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("vat"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("salesTax"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentProviderFee"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentMethodFee"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currency"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentStationUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemSnapshot"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statusReason"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("chargedTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("fulfilledTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("refundedTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("chargebackTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("chargebackReversedTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OrderInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.orderNo);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.paymentOrderNo);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteBoolean(value.sandbox);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.quantity);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.price);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.discountedPrice);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.paymentProvider);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.vat);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteInt32(value.salesTax);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteInt32(value.paymentProviderFee);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteInt32(value.paymentMethodFee);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CurrencySummary>().Serialize(ref writer, value.currency, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.paymentStationUrl);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemSnapshot>().Serialize(ref writer, value.itemSnapshot, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderStatus>().Serialize(ref writer, value.status, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            writer.WriteString(value.statusReason);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.chargedTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[23]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.fulfilledTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[24]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.refundedTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[25]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.chargebackTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[26]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.chargebackReversedTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[27]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[28]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OrderInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __orderNo__ = default(string);
            var __orderNo__b__ = false;
            var __paymentOrderNo__ = default(string);
            var __paymentOrderNo__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __sandbox__ = default(bool);
            var __sandbox__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;
            var __price__ = default(int);
            var __price__b__ = false;
            var __discountedPrice__ = default(int);
            var __discountedPrice__b__ = false;
            var __paymentProvider__ = default(string);
            var __paymentProvider__b__ = false;
            var __vat__ = default(int);
            var __vat__b__ = false;
            var __salesTax__ = default(int);
            var __salesTax__b__ = false;
            var __paymentProviderFee__ = default(int);
            var __paymentProviderFee__b__ = false;
            var __paymentMethodFee__ = default(int);
            var __paymentMethodFee__b__ = false;
            var __currency__ = default(global::AccelByte.Models.CurrencySummary);
            var __currency__b__ = false;
            var __paymentStationUrl__ = default(string);
            var __paymentStationUrl__b__ = false;
            var __itemSnapshot__ = default(global::AccelByte.Models.ItemSnapshot);
            var __itemSnapshot__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __status__ = default(global::AccelByte.Models.OrderStatus);
            var __status__b__ = false;
            var __statusReason__ = default(string);
            var __statusReason__b__ = false;
            var __createdTime__ = default(global::System.DateTime);
            var __createdTime__b__ = false;
            var __chargedTime__ = default(global::System.DateTime);
            var __chargedTime__b__ = false;
            var __fulfilledTime__ = default(global::System.DateTime);
            var __fulfilledTime__b__ = false;
            var __refundedTime__ = default(global::System.DateTime);
            var __refundedTime__b__ = false;
            var __chargebackTime__ = default(global::System.DateTime);
            var __chargebackTime__b__ = false;
            var __chargebackReversedTime__ = default(global::System.DateTime);
            var __chargebackReversedTime__b__ = false;
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
                        __orderNo__ = reader.ReadString();
                        __orderNo__b__ = true;
                        break;
                    case 1:
                        __paymentOrderNo__ = reader.ReadString();
                        __paymentOrderNo__b__ = true;
                        break;
                    case 2:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 3:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 4:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 5:
                        __sandbox__ = reader.ReadBoolean();
                        __sandbox__b__ = true;
                        break;
                    case 6:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    case 7:
                        __price__ = reader.ReadInt32();
                        __price__b__ = true;
                        break;
                    case 8:
                        __discountedPrice__ = reader.ReadInt32();
                        __discountedPrice__b__ = true;
                        break;
                    case 9:
                        __paymentProvider__ = reader.ReadString();
                        __paymentProvider__b__ = true;
                        break;
                    case 10:
                        __vat__ = reader.ReadInt32();
                        __vat__b__ = true;
                        break;
                    case 11:
                        __salesTax__ = reader.ReadInt32();
                        __salesTax__b__ = true;
                        break;
                    case 12:
                        __paymentProviderFee__ = reader.ReadInt32();
                        __paymentProviderFee__b__ = true;
                        break;
                    case 13:
                        __paymentMethodFee__ = reader.ReadInt32();
                        __paymentMethodFee__b__ = true;
                        break;
                    case 14:
                        __currency__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CurrencySummary>().Deserialize(ref reader, formatterResolver);
                        __currency__b__ = true;
                        break;
                    case 15:
                        __paymentStationUrl__ = reader.ReadString();
                        __paymentStationUrl__b__ = true;
                        break;
                    case 16:
                        __itemSnapshot__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemSnapshot>().Deserialize(ref reader, formatterResolver);
                        __itemSnapshot__b__ = true;
                        break;
                    case 17:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 18:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 19:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    case 20:
                        __statusReason__ = reader.ReadString();
                        __statusReason__b__ = true;
                        break;
                    case 21:
                        __createdTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdTime__b__ = true;
                        break;
                    case 22:
                        __chargedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __chargedTime__b__ = true;
                        break;
                    case 23:
                        __fulfilledTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __fulfilledTime__b__ = true;
                        break;
                    case 24:
                        __refundedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __refundedTime__b__ = true;
                        break;
                    case 25:
                        __chargebackTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __chargebackTime__b__ = true;
                        break;
                    case 26:
                        __chargebackReversedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __chargebackReversedTime__b__ = true;
                        break;
                    case 27:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 28:
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

            var ____result = new global::AccelByte.Models.OrderInfo();
            if(__orderNo__b__) ____result.orderNo = __orderNo__;
            if(__paymentOrderNo__b__) ____result.paymentOrderNo = __paymentOrderNo__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__sandbox__b__) ____result.sandbox = __sandbox__;
            if(__quantity__b__) ____result.quantity = __quantity__;
            if(__price__b__) ____result.price = __price__;
            if(__discountedPrice__b__) ____result.discountedPrice = __discountedPrice__;
            if(__paymentProvider__b__) ____result.paymentProvider = __paymentProvider__;
            if(__vat__b__) ____result.vat = __vat__;
            if(__salesTax__b__) ____result.salesTax = __salesTax__;
            if(__paymentProviderFee__b__) ____result.paymentProviderFee = __paymentProviderFee__;
            if(__paymentMethodFee__b__) ____result.paymentMethodFee = __paymentMethodFee__;
            if(__currency__b__) ____result.currency = __currency__;
            if(__paymentStationUrl__b__) ____result.paymentStationUrl = __paymentStationUrl__;
            if(__itemSnapshot__b__) ____result.itemSnapshot = __itemSnapshot__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;
            if(__status__b__) ____result.status = __status__;
            if(__statusReason__b__) ____result.statusReason = __statusReason__;
            if(__createdTime__b__) ____result.createdTime = __createdTime__;
            if(__chargedTime__b__) ____result.chargedTime = __chargedTime__;
            if(__fulfilledTime__b__) ____result.fulfilledTime = __fulfilledTime__;
            if(__refundedTime__b__) ____result.refundedTime = __refundedTime__;
            if(__chargebackTime__b__) ____result.chargebackTime = __chargebackTime__;
            if(__chargebackReversedTime__b__) ____result.chargebackReversedTime = __chargebackReversedTime__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class OrderPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OrderPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OrderPagingSlicedResultFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OrderPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OrderPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.OrderInfo[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.OrderPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class OrderRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OrderRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OrderRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("price"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountedPrice"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("returnUrl"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountedPrice"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("returnUrl"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OrderRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.price);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.discountedPrice);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.language);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.returnUrl);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OrderRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;
            var __price__ = default(int);
            var __price__b__ = false;
            var __discountedPrice__ = default(int);
            var __discountedPrice__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __language__ = default(string);
            var __language__b__ = false;
            var __returnUrl__ = default(string);
            var __returnUrl__b__ = false;

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
                    case 2:
                        __price__ = reader.ReadInt32();
                        __price__b__ = true;
                        break;
                    case 3:
                        __discountedPrice__ = reader.ReadInt32();
                        __discountedPrice__b__ = true;
                        break;
                    case 4:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 5:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 6:
                        __language__ = reader.ReadString();
                        __language__b__ = true;
                        break;
                    case 7:
                        __returnUrl__ = reader.ReadString();
                        __returnUrl__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.OrderRequest();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__quantity__b__) ____result.quantity = __quantity__;
            if(__price__b__) ____result.price = __price__;
            if(__discountedPrice__b__) ____result.discountedPrice = __discountedPrice__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;
            if(__returnUrl__b__) ____result.returnUrl = __returnUrl__;

            return ____result;
        }
    }


    public sealed class OrderTransactionFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OrderTransaction>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OrderTransactionFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("txId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("amount"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("vat"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("salesTax"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currency"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("provider"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentProviderFee"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentMethod"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentMethodFee"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("merchantId"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("extTxId"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("extStatusCode"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("extMessage"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("txStartTime"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("txEndTime"), 16},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("txId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("amount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("vat"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("salesTax"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currency"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("provider"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentProviderFee"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentMethod"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentMethodFee"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("merchantId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("extTxId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("extStatusCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("extMessage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("txStartTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("txEndTime"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OrderTransaction value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.txId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.amount);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.vat);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.salesTax);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CurrencySummary>().Serialize(ref writer, value.currency, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.type);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.provider);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.paymentProviderFee);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.paymentMethod);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.paymentMethodFee);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.merchantId);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.extTxId);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.extStatusCode);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.extMessage);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.txStartTime);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.txEndTime);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OrderTransaction Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __txId__ = default(string);
            var __txId__b__ = false;
            var __amount__ = default(int);
            var __amount__b__ = false;
            var __vat__ = default(int);
            var __vat__b__ = false;
            var __salesTax__ = default(int);
            var __salesTax__b__ = false;
            var __currency__ = default(global::AccelByte.Models.CurrencySummary);
            var __currency__b__ = false;
            var __type__ = default(string);
            var __type__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __provider__ = default(string);
            var __provider__b__ = false;
            var __paymentProviderFee__ = default(int);
            var __paymentProviderFee__b__ = false;
            var __paymentMethod__ = default(string);
            var __paymentMethod__b__ = false;
            var __paymentMethodFee__ = default(int);
            var __paymentMethodFee__b__ = false;
            var __merchantId__ = default(string);
            var __merchantId__b__ = false;
            var __extTxId__ = default(string);
            var __extTxId__b__ = false;
            var __extStatusCode__ = default(string);
            var __extStatusCode__b__ = false;
            var __extMessage__ = default(string);
            var __extMessage__b__ = false;
            var __txStartTime__ = default(string);
            var __txStartTime__b__ = false;
            var __txEndTime__ = default(string);
            var __txEndTime__b__ = false;

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
                        __txId__ = reader.ReadString();
                        __txId__b__ = true;
                        break;
                    case 1:
                        __amount__ = reader.ReadInt32();
                        __amount__b__ = true;
                        break;
                    case 2:
                        __vat__ = reader.ReadInt32();
                        __vat__b__ = true;
                        break;
                    case 3:
                        __salesTax__ = reader.ReadInt32();
                        __salesTax__b__ = true;
                        break;
                    case 4:
                        __currency__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CurrencySummary>().Deserialize(ref reader, formatterResolver);
                        __currency__b__ = true;
                        break;
                    case 5:
                        __type__ = reader.ReadString();
                        __type__b__ = true;
                        break;
                    case 6:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 7:
                        __provider__ = reader.ReadString();
                        __provider__b__ = true;
                        break;
                    case 8:
                        __paymentProviderFee__ = reader.ReadInt32();
                        __paymentProviderFee__b__ = true;
                        break;
                    case 9:
                        __paymentMethod__ = reader.ReadString();
                        __paymentMethod__b__ = true;
                        break;
                    case 10:
                        __paymentMethodFee__ = reader.ReadInt32();
                        __paymentMethodFee__b__ = true;
                        break;
                    case 11:
                        __merchantId__ = reader.ReadString();
                        __merchantId__b__ = true;
                        break;
                    case 12:
                        __extTxId__ = reader.ReadString();
                        __extTxId__b__ = true;
                        break;
                    case 13:
                        __extStatusCode__ = reader.ReadString();
                        __extStatusCode__b__ = true;
                        break;
                    case 14:
                        __extMessage__ = reader.ReadString();
                        __extMessage__b__ = true;
                        break;
                    case 15:
                        __txStartTime__ = reader.ReadString();
                        __txStartTime__b__ = true;
                        break;
                    case 16:
                        __txEndTime__ = reader.ReadString();
                        __txEndTime__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.OrderTransaction();
            if(__txId__b__) ____result.txId = __txId__;
            if(__amount__b__) ____result.amount = __amount__;
            if(__vat__b__) ____result.vat = __vat__;
            if(__salesTax__b__) ____result.salesTax = __salesTax__;
            if(__currency__b__) ____result.currency = __currency__;
            if(__type__b__) ____result.type = __type__;
            if(__status__b__) ____result.status = __status__;
            if(__provider__b__) ____result.provider = __provider__;
            if(__paymentProviderFee__b__) ____result.paymentProviderFee = __paymentProviderFee__;
            if(__paymentMethod__b__) ____result.paymentMethod = __paymentMethod__;
            if(__paymentMethodFee__b__) ____result.paymentMethodFee = __paymentMethodFee__;
            if(__merchantId__b__) ____result.merchantId = __merchantId__;
            if(__extTxId__b__) ____result.extTxId = __extTxId__;
            if(__extStatusCode__b__) ____result.extStatusCode = __extStatusCode__;
            if(__extMessage__b__) ____result.extMessage = __extMessage__;
            if(__txStartTime__b__) ____result.txStartTime = __txStartTime__;
            if(__txEndTime__b__) ____result.txEndTime = __txEndTime__;

            return ____result;
        }
    }


    public sealed class EntitlementInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.EntitlementInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public EntitlementInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("clazz"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("bundleItemId"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantedCode"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemNamespace"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("source"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("distributedQuantity"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemSnapshot"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("startDate"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("endDate"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantedAt"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 23},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 24},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("clazz"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bundleItemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grantedCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("source"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("distributedQuantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemSnapshot"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("startDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("endDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grantedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.EntitlementInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementClazz>().Serialize(ref writer, value.clazz, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Serialize(ref writer, value.type, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementStatus>().Serialize(ref writer, value.status, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Serialize(ref writer, value.appType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.bundleItemId);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.grantedCode);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.itemNamespace);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteInt32(value.quantity);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementSource>().Serialize(ref writer, value.source, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteInt32(value.distributedQuantity);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemSnapshot>().Serialize(ref writer, value.itemSnapshot, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.startDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.endDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.grantedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[23]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[24]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.EntitlementInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __clazz__ = default(global::AccelByte.Models.EntitlementClazz);
            var __clazz__b__ = false;
            var __type__ = default(global::AccelByte.Models.EntitlementType);
            var __type__b__ = false;
            var __status__ = default(global::AccelByte.Models.EntitlementStatus);
            var __status__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(global::AccelByte.Models.EntitlementAppType);
            var __appType__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __bundleItemId__ = default(string);
            var __bundleItemId__b__ = false;
            var __grantedCode__ = default(string);
            var __grantedCode__b__ = false;
            var __itemNamespace__ = default(string);
            var __itemNamespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;
            var __source__ = default(global::AccelByte.Models.EntitlementSource);
            var __source__b__ = false;
            var __distributedQuantity__ = default(int);
            var __distributedQuantity__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __itemSnapshot__ = default(global::AccelByte.Models.ItemSnapshot);
            var __itemSnapshot__b__ = false;
            var __startDate__ = default(global::System.DateTime);
            var __startDate__b__ = false;
            var __endDate__ = default(global::System.DateTime);
            var __endDate__b__ = false;
            var __grantedAt__ = default(global::System.DateTime);
            var __grantedAt__b__ = false;
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
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __clazz__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementClazz>().Deserialize(ref reader, formatterResolver);
                        __clazz__b__ = true;
                        break;
                    case 3:
                        __type__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Deserialize(ref reader, formatterResolver);
                        __type__b__ = true;
                        break;
                    case 4:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    case 5:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 6:
                        __appType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Deserialize(ref reader, formatterResolver);
                        __appType__b__ = true;
                        break;
                    case 7:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 8:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 9:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 10:
                        __bundleItemId__ = reader.ReadString();
                        __bundleItemId__b__ = true;
                        break;
                    case 11:
                        __grantedCode__ = reader.ReadString();
                        __grantedCode__b__ = true;
                        break;
                    case 12:
                        __itemNamespace__ = reader.ReadString();
                        __itemNamespace__b__ = true;
                        break;
                    case 13:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 14:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 15:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    case 16:
                        __source__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementSource>().Deserialize(ref reader, formatterResolver);
                        __source__b__ = true;
                        break;
                    case 17:
                        __distributedQuantity__ = reader.ReadInt32();
                        __distributedQuantity__b__ = true;
                        break;
                    case 18:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 19:
                        __itemSnapshot__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemSnapshot>().Deserialize(ref reader, formatterResolver);
                        __itemSnapshot__b__ = true;
                        break;
                    case 20:
                        __startDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __startDate__b__ = true;
                        break;
                    case 21:
                        __endDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __endDate__b__ = true;
                        break;
                    case 22:
                        __grantedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __grantedAt__b__ = true;
                        break;
                    case 23:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 24:
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

            var ____result = new global::AccelByte.Models.EntitlementInfo();
            if(__id__b__) ____result.id = __id__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__clazz__b__) ____result.clazz = __clazz__;
            if(__type__b__) ____result.type = __type__;
            if(__status__b__) ____result.status = __status__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__bundleItemId__b__) ____result.bundleItemId = __bundleItemId__;
            if(__grantedCode__b__) ____result.grantedCode = __grantedCode__;
            if(__itemNamespace__b__) ____result.itemNamespace = __itemNamespace__;
            if(__name__b__) ____result.name = __name__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__quantity__b__) ____result.quantity = __quantity__;
            if(__source__b__) ____result.source = __source__;
            if(__distributedQuantity__b__) ____result.distributedQuantity = __distributedQuantity__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__itemSnapshot__b__) ____result.itemSnapshot = __itemSnapshot__;
            if(__startDate__b__) ____result.startDate = __startDate__;
            if(__endDate__b__) ____result.endDate = __endDate__;
            if(__grantedAt__b__) ____result.grantedAt = __grantedAt__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class EntitlementPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.EntitlementPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public EntitlementPagingSlicedResultFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.EntitlementPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.EntitlementPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.EntitlementInfo[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.EntitlementPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class ConsumeUserEntitlementRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ConsumeUserEntitlementRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ConsumeUserEntitlementRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("useCount"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ConsumeUserEntitlementRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.useCount);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ConsumeUserEntitlementRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __useCount__ = default(int);
            var __useCount__b__ = false;

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
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ConsumeUserEntitlementRequest();
            if(__useCount__b__) ____result.useCount = __useCount__;

            return ____result;
        }
    }


    public sealed class GrantUserEntitlementRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GrantUserEntitlementRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GrantUserEntitlementRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantedCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemNamespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("source"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grantedCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("source"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("language"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GrantUserEntitlementRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.grantedCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.itemNamespace);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.quantity);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementSource>().Serialize(ref writer, value.source, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.language);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GrantUserEntitlementRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __grantedCode__ = default(string);
            var __grantedCode__b__ = false;
            var __itemNamespace__ = default(string);
            var __itemNamespace__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;
            var __source__ = default(global::AccelByte.Models.EntitlementSource);
            var __source__b__ = false;
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
                        __grantedCode__ = reader.ReadString();
                        __grantedCode__b__ = true;
                        break;
                    case 2:
                        __itemNamespace__ = reader.ReadString();
                        __itemNamespace__b__ = true;
                        break;
                    case 3:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    case 4:
                        __source__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementSource>().Deserialize(ref reader, formatterResolver);
                        __source__b__ = true;
                        break;
                    case 5:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 6:
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

            var ____result = new global::AccelByte.Models.GrantUserEntitlementRequest();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__grantedCode__b__) ____result.grantedCode = __grantedCode__;
            if(__itemNamespace__b__) ____result.itemNamespace = __itemNamespace__;
            if(__quantity__b__) ____result.quantity = __quantity__;
            if(__source__b__) ____result.source = __source__;
            if(__region__b__) ____result.region = __region__;
            if(__language__b__) ____result.language = __language__;

            return ____result;
        }
    }


    public sealed class StackableEntitlementInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StackableEntitlementInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StackableEntitlementInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("clazz"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantedCode"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemNamespace"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("source"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("distributedQuantity"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemSnapshot"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("startDate"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("endDate"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackable"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantedAt"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 23},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 24},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackedUseCount"), 25},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("stackedQuantity"), 26},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("clazz"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grantedCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("source"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("distributedQuantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemSnapshot"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("startDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("endDate"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackable"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grantedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackedUseCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stackedQuantity"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StackableEntitlementInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementClazz>().Serialize(ref writer, value.clazz, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Serialize(ref writer, value.type, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementStatus>().Serialize(ref writer, value.status, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Serialize(ref writer, value.appType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.grantedCode);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.itemNamespace);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteInt32(value.quantity);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementSource>().Serialize(ref writer, value.source, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteInt32(value.distributedQuantity);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[18]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemSnapshot>().Serialize(ref writer, value.itemSnapshot, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.startDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.endDate, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            writer.WriteBoolean(value.stackable);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.grantedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[23]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[24]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[25]);
            writer.WriteInt32(value.stackedUseCount);
            writer.WriteRaw(this.____stringByteKeys[26]);
            writer.WriteInt32(value.stackedQuantity);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StackableEntitlementInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __clazz__ = default(global::AccelByte.Models.EntitlementClazz);
            var __clazz__b__ = false;
            var __type__ = default(global::AccelByte.Models.EntitlementType);
            var __type__b__ = false;
            var __status__ = default(global::AccelByte.Models.EntitlementStatus);
            var __status__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(global::AccelByte.Models.EntitlementAppType);
            var __appType__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __grantedCode__ = default(string);
            var __grantedCode__b__ = false;
            var __itemNamespace__ = default(string);
            var __itemNamespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __quantity__ = default(int);
            var __quantity__b__ = false;
            var __source__ = default(global::AccelByte.Models.EntitlementSource);
            var __source__b__ = false;
            var __distributedQuantity__ = default(int);
            var __distributedQuantity__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __itemSnapshot__ = default(global::AccelByte.Models.ItemSnapshot);
            var __itemSnapshot__b__ = false;
            var __startDate__ = default(global::System.DateTime);
            var __startDate__b__ = false;
            var __endDate__ = default(global::System.DateTime);
            var __endDate__b__ = false;
            var __stackable__ = default(bool);
            var __stackable__b__ = false;
            var __grantedAt__ = default(global::System.DateTime);
            var __grantedAt__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __stackedUseCount__ = default(int);
            var __stackedUseCount__b__ = false;
            var __stackedQuantity__ = default(int);
            var __stackedQuantity__b__ = false;

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
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __clazz__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementClazz>().Deserialize(ref reader, formatterResolver);
                        __clazz__b__ = true;
                        break;
                    case 3:
                        __type__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementType>().Deserialize(ref reader, formatterResolver);
                        __type__b__ = true;
                        break;
                    case 4:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    case 5:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 6:
                        __appType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementAppType>().Deserialize(ref reader, formatterResolver);
                        __appType__b__ = true;
                        break;
                    case 7:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 8:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 9:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 10:
                        __grantedCode__ = reader.ReadString();
                        __grantedCode__b__ = true;
                        break;
                    case 11:
                        __itemNamespace__ = reader.ReadString();
                        __itemNamespace__b__ = true;
                        break;
                    case 12:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 13:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 14:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    case 15:
                        __source__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.EntitlementSource>().Deserialize(ref reader, formatterResolver);
                        __source__b__ = true;
                        break;
                    case 16:
                        __distributedQuantity__ = reader.ReadInt32();
                        __distributedQuantity__b__ = true;
                        break;
                    case 17:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 18:
                        __itemSnapshot__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemSnapshot>().Deserialize(ref reader, formatterResolver);
                        __itemSnapshot__b__ = true;
                        break;
                    case 19:
                        __startDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __startDate__b__ = true;
                        break;
                    case 20:
                        __endDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __endDate__b__ = true;
                        break;
                    case 21:
                        __stackable__ = reader.ReadBoolean();
                        __stackable__b__ = true;
                        break;
                    case 22:
                        __grantedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __grantedAt__b__ = true;
                        break;
                    case 23:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 24:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 25:
                        __stackedUseCount__ = reader.ReadInt32();
                        __stackedUseCount__b__ = true;
                        break;
                    case 26:
                        __stackedQuantity__ = reader.ReadInt32();
                        __stackedQuantity__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StackableEntitlementInfo();
            if(__id__b__) ____result.id = __id__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__clazz__b__) ____result.clazz = __clazz__;
            if(__type__b__) ____result.type = __type__;
            if(__status__b__) ____result.status = __status__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__grantedCode__b__) ____result.grantedCode = __grantedCode__;
            if(__itemNamespace__b__) ____result.itemNamespace = __itemNamespace__;
            if(__name__b__) ____result.name = __name__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__quantity__b__) ____result.quantity = __quantity__;
            if(__source__b__) ____result.source = __source__;
            if(__distributedQuantity__b__) ____result.distributedQuantity = __distributedQuantity__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__itemSnapshot__b__) ____result.itemSnapshot = __itemSnapshot__;
            if(__startDate__b__) ____result.startDate = __startDate__;
            if(__endDate__b__) ____result.endDate = __endDate__;
            if(__stackable__b__) ____result.stackable = __stackable__;
            if(__grantedAt__b__) ____result.grantedAt = __grantedAt__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__stackedUseCount__b__) ____result.stackedUseCount = __stackedUseCount__;
            if(__stackedQuantity__b__) ____result.stackedQuantity = __stackedQuantity__;

            return ____result;
        }
    }


    public sealed class AttributesFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Attributes>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AttributesFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("serverId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("serverName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("characterId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("characterName"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("serverId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("serverName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("characterId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("characterName"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Attributes value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.serverId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.serverName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.characterId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.characterName);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Attributes Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __serverId__ = default(string);
            var __serverId__b__ = false;
            var __serverName__ = default(string);
            var __serverName__b__ = false;
            var __characterId__ = default(string);
            var __characterId__b__ = false;
            var __characterName__ = default(string);
            var __characterName__b__ = false;

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
                        __serverId__ = reader.ReadString();
                        __serverId__b__ = true;
                        break;
                    case 1:
                        __serverName__ = reader.ReadString();
                        __serverName__b__ = true;
                        break;
                    case 2:
                        __characterId__ = reader.ReadString();
                        __characterId__b__ = true;
                        break;
                    case 3:
                        __characterName__ = reader.ReadString();
                        __characterName__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Attributes();
            if(__serverId__b__) ____result.serverId = __serverId__;
            if(__serverName__b__) ____result.serverName = __serverName__;
            if(__characterId__b__) ____result.characterId = __characterId__;
            if(__characterName__b__) ____result.characterName = __characterName__;

            return ____result;
        }
    }


    public sealed class DistributionAttributesFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.DistributionAttributes>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DistributionAttributesFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attributes"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("attributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.DistributionAttributes value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Attributes>().Serialize(ref writer, value.attributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.DistributionAttributes Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __attributes__ = default(global::AccelByte.Models.Attributes);
            var __attributes__b__ = false;

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
                        __attributes__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Attributes>().Deserialize(ref reader, formatterResolver);
                        __attributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.DistributionAttributes();
            if(__attributes__b__) ____result.attributes = __attributes__;

            return ____result;
        }
    }


    public sealed class DistributionReceiverFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.DistributionReceiver>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DistributionReceiverFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace_"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("extUserId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attributes"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace_"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("extUserId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("attributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.DistributionReceiver value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.extUserId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Attributes>().Serialize(ref writer, value.attributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.DistributionReceiver Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userId__ = default(string);
            var __userId__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __extUserId__ = default(string);
            var __extUserId__b__ = false;
            var __attributes__ = default(global::AccelByte.Models.Attributes);
            var __attributes__b__ = false;

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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 1:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 2:
                        __extUserId__ = reader.ReadString();
                        __extUserId__b__ = true;
                        break;
                    case 3:
                        __attributes__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Attributes>().Deserialize(ref reader, formatterResolver);
                        __attributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.DistributionReceiver();
            if(__userId__b__) ____result.userId = __userId__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__extUserId__b__) ____result.extUserId = __extUserId__;
            if(__attributes__b__) ____result.attributes = __attributes__;

            return ____result;
        }
    }


    public sealed class ServiceErrorFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ServiceError>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ServiceErrorFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("errorCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("numericErrorCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("errorMessage"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("messageVariables"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("errorCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("numericErrorCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorMessage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("messageVariables"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ServiceError value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt64(value.errorCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.numericErrorCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.errorMessage);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.messageVariables, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ServiceError Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __errorCode__ = default(long);
            var __errorCode__b__ = false;
            var __numericErrorCode__ = default(int);
            var __numericErrorCode__b__ = false;
            var __errorMessage__ = default(string);
            var __errorMessage__b__ = false;
            var __messageVariables__ = default(object);
            var __messageVariables__b__ = false;

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
                        __errorCode__ = reader.ReadInt64();
                        __errorCode__b__ = true;
                        break;
                    case 1:
                        __numericErrorCode__ = reader.ReadInt32();
                        __numericErrorCode__b__ = true;
                        break;
                    case 2:
                        __errorMessage__ = reader.ReadString();
                        __errorMessage__b__ = true;
                        break;
                    case 3:
                        __messageVariables__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __messageVariables__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ServiceError();
            if(__errorCode__b__) ____result.errorCode = __errorCode__;
            if(__numericErrorCode__b__) ____result.numericErrorCode = __numericErrorCode__;
            if(__errorMessage__b__) ____result.errorMessage = __errorMessage__;
            if(__messageVariables__b__) ____result.messageVariables = __messageVariables__;

            return ____result;
        }
    }


    public sealed class OAuthErrorFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OAuthError>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OAuthErrorFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("error"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("error_description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("error_uri"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("error"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error_description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error_uri"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OAuthError value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.error);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.error_description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.error_uri);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OAuthError Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __error__ = default(string);
            var __error__b__ = false;
            var __error_description__ = default(string);
            var __error_description__b__ = false;
            var __error_uri__ = default(string);
            var __error_uri__b__ = false;

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
                        __error__ = reader.ReadString();
                        __error__b__ = true;
                        break;
                    case 1:
                        __error_description__ = reader.ReadString();
                        __error_description__b__ = true;
                        break;
                    case 2:
                        __error_uri__ = reader.ReadString();
                        __error_uri__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.OAuthError();
            if(__error__b__) ____result.error = __error__;
            if(__error_description__b__) ____result.error_description = __error_description__;
            if(__error_uri__b__) ____result.error_uri = __error_uri__;

            return ____result;
        }
    }


    public sealed class GameProfileFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GameProfile>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GameProfileFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attributes"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("label"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileName"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("profileId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("attributes"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("label"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("profileName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GameProfile value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.profileId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.attributes, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.avatarUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.label);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.profileName);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GameProfile Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __profileId__ = default(string);
            var __profileId__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __attributes__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __attributes__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;
            var __label__ = default(string);
            var __label__b__ = false;
            var __profileName__ = default(string);
            var __profileName__b__ = false;
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
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 1:
                        __profileId__ = reader.ReadString();
                        __profileId__b__ = true;
                        break;
                    case 2:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 3:
                        __attributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __attributes__b__ = true;
                        break;
                    case 4:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    case 5:
                        __label__ = reader.ReadString();
                        __label__b__ = true;
                        break;
                    case 6:
                        __profileName__ = reader.ReadString();
                        __profileName__b__ = true;
                        break;
                    case 7:
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

            var ____result = new global::AccelByte.Models.GameProfile();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__profileId__b__) ____result.profileId = __profileId__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__attributes__b__) ____result.attributes = __attributes__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;
            if(__label__b__) ____result.label = __label__;
            if(__profileName__b__) ____result.profileName = __profileName__;
            if(__tags__b__) ____result.tags = __tags__;

            return ____result;
        }
    }


    public sealed class GameProfileRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GameProfileRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GameProfileRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("attributes"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("label"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileName"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("attributes"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("label"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("profileName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GameProfileRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.attributes, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.avatarUrl);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.label);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.profileName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GameProfileRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __attributes__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __attributes__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;
            var __label__ = default(string);
            var __label__b__ = false;
            var __profileName__ = default(string);
            var __profileName__b__ = false;
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
                        __attributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __attributes__b__ = true;
                        break;
                    case 1:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    case 2:
                        __label__ = reader.ReadString();
                        __label__b__ = true;
                        break;
                    case 3:
                        __profileName__ = reader.ReadString();
                        __profileName__b__ = true;
                        break;
                    case 4:
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

            var ____result = new global::AccelByte.Models.GameProfileRequest();
            if(__attributes__b__) ____result.attributes = __attributes__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;
            if(__label__b__) ____result.label = __label__;
            if(__profileName__b__) ____result.profileName = __profileName__;
            if(__tags__b__) ____result.tags = __tags__;

            return ____result;
        }
    }


    public sealed class GameProfileAttributeFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GameProfileAttribute>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GameProfileAttributeFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GameProfileAttribute value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.value);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GameProfileAttribute Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __name__ = default(string);
            var __name__b__ = false;
            var __value__ = default(string);
            var __value__b__ = false;

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
                        __value__ = reader.ReadString();
                        __value__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.GameProfileAttribute();
            if(__name__b__) ____result.name = __name__;
            if(__value__b__) ____result.value = __value__;

            return ____result;
        }
    }


    public sealed class GameProfilePublicInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GameProfilePublicInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GameProfilePublicInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileName"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatarUrl"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("profileId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("profileName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GameProfilePublicInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.profileId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.profileName);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.avatarUrl);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GameProfilePublicInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __profileId__ = default(string);
            var __profileId__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __profileName__ = default(string);
            var __profileName__b__ = false;
            var __avatarUrl__ = default(string);
            var __avatarUrl__b__ = false;

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
                        __profileId__ = reader.ReadString();
                        __profileId__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __profileName__ = reader.ReadString();
                        __profileName__b__ = true;
                        break;
                    case 3:
                        __avatarUrl__ = reader.ReadString();
                        __avatarUrl__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.GameProfilePublicInfo();
            if(__profileId__b__) ____result.profileId = __profileId__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__profileName__b__) ____result.profileName = __profileName__;
            if(__avatarUrl__b__) ____result.avatarUrl = __avatarUrl__;

            return ____result;
        }
    }


    public sealed class UserGameProfilesFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserGameProfiles>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserGameProfilesFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("gameProfiles"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("gameProfiles"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserGameProfiles value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.GameProfilePublicInfo[]>().Serialize(ref writer, value.gameProfiles, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserGameProfiles Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userId__ = default(string);
            var __userId__b__ = false;
            var __gameProfiles__ = default(global::AccelByte.Models.GameProfilePublicInfo[]);
            var __gameProfiles__b__ = false;

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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 1:
                        __gameProfiles__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.GameProfilePublicInfo[]>().Deserialize(ref reader, formatterResolver);
                        __gameProfiles__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserGameProfiles();
            if(__userId__b__) ____result.userId = __userId__;
            if(__gameProfiles__b__) ____result.gameProfiles = __gameProfiles__;

            return ____result;
        }
    }


    public sealed class TelemetryBodyFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.TelemetryBody>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TelemetryBodyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("EventName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("EventNamespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Payload"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("EventName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("EventNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Payload"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.TelemetryBody value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.EventName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.EventNamespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.Payload, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.TelemetryBody Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __EventName__ = default(string);
            var __EventName__b__ = false;
            var __EventNamespace__ = default(string);
            var __EventNamespace__b__ = false;
            var __Payload__ = default(object);
            var __Payload__b__ = false;

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
                        __EventName__ = reader.ReadString();
                        __EventName__b__ = true;
                        break;
                    case 1:
                        __EventNamespace__ = reader.ReadString();
                        __EventNamespace__b__ = true;
                        break;
                    case 2:
                        __Payload__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __Payload__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.TelemetryBody();
            if(__EventName__b__) ____result.EventName = __EventName__;
            if(__EventNamespace__b__) ____result.EventNamespace = __EventNamespace__;
            if(__Payload__b__) ____result.Payload = __Payload__;

            return ____result;
        }
    }


    public sealed class UserPointFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserPoint>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserPointFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("point"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("point"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserPoint value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.point);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserPoint Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __point__ = default(float);
            var __point__b__ = false;
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
                        __point__ = reader.ReadSingle();
                        __point__b__ = true;
                        break;
                    case 1:
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

            var ____result = new global::AccelByte.Models.UserPoint();
            if(__point__b__) ____result.point = __point__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class UserRankingFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserRanking>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserRankingFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("point"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rank"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("point"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("rank"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserRanking value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.point);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.rank);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserRanking Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __point__ = default(float);
            var __point__b__ = false;
            var __rank__ = default(int);
            var __rank__b__ = false;

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
                        __point__ = reader.ReadSingle();
                        __point__b__ = true;
                        break;
                    case 1:
                        __rank__ = reader.ReadInt32();
                        __rank__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserRanking();
            if(__point__b__) ____result.point = __point__;
            if(__rank__b__) ____result.rank = __rank__;

            return ____result;
        }
    }


    public sealed class UserRankingDataFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserRankingData>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserRankingDataFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("allTime"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("current"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("daily"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("monthly"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("weekly"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("allTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("current"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("daily"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("monthly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("weekly"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserRankingData value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Serialize(ref writer, value.allTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Serialize(ref writer, value.current, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Serialize(ref writer, value.daily, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Serialize(ref writer, value.monthly, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Serialize(ref writer, value.weekly, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserRankingData Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __allTime__ = default(global::AccelByte.Models.UserRanking);
            var __allTime__b__ = false;
            var __current__ = default(global::AccelByte.Models.UserRanking);
            var __current__b__ = false;
            var __daily__ = default(global::AccelByte.Models.UserRanking);
            var __daily__b__ = false;
            var __monthly__ = default(global::AccelByte.Models.UserRanking);
            var __monthly__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __weekly__ = default(global::AccelByte.Models.UserRanking);
            var __weekly__b__ = false;

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
                        __allTime__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Deserialize(ref reader, formatterResolver);
                        __allTime__b__ = true;
                        break;
                    case 1:
                        __current__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Deserialize(ref reader, formatterResolver);
                        __current__b__ = true;
                        break;
                    case 2:
                        __daily__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Deserialize(ref reader, formatterResolver);
                        __daily__b__ = true;
                        break;
                    case 3:
                        __monthly__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Deserialize(ref reader, formatterResolver);
                        __monthly__b__ = true;
                        break;
                    case 4:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 5:
                        __weekly__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserRanking>().Deserialize(ref reader, formatterResolver);
                        __weekly__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserRankingData();
            if(__allTime__b__) ____result.allTime = __allTime__;
            if(__current__b__) ____result.current = __current__;
            if(__daily__b__) ____result.daily = __daily__;
            if(__monthly__b__) ____result.monthly = __monthly__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__weekly__b__) ____result.weekly = __weekly__;

            return ____result;
        }
    }


    public sealed class LeaderboardRankingResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LeaderboardRankingResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LeaderboardRankingResultFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LeaderboardRankingResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserPoint[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LeaderboardRankingResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.UserPoint[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserPoint[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LeaderboardRankingResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class DisconnectNotifFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.DisconnectNotif>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DisconnectNotifFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("message"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("message"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.DisconnectNotif value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.message);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.DisconnectNotif Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __message__ = default(string);
            var __message__b__ = false;

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
                        __message__ = reader.ReadString();
                        __message__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.DisconnectNotif();
            if(__message__b__) ____result.message = __message__;

            return ____result;
        }
    }


    public sealed class LobbySessionIdFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LobbySessionId>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LobbySessionIdFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lobbySessionID"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("lobbySessionID"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LobbySessionId value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.lobbySessionID);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LobbySessionId Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __lobbySessionID__ = default(string);
            var __lobbySessionID__b__ = false;

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
                        __lobbySessionID__ = reader.ReadString();
                        __lobbySessionID__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LobbySessionId();
            if(__lobbySessionID__b__) ____result.lobbySessionID = __lobbySessionID__;

            return ____result;
        }
    }


    public sealed class NotificationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Notification>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public NotificationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("from"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("to"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("topic"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("payload"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sentAt"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("from"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("to"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("topic"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("payload"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sentAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Notification value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.from);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.to);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.topic);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.payload);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.sentAt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Notification Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __from__ = default(string);
            var __from__b__ = false;
            var __to__ = default(string);
            var __to__b__ = false;
            var __topic__ = default(string);
            var __topic__b__ = false;
            var __payload__ = default(string);
            var __payload__b__ = false;
            var __sentAt__ = default(string);
            var __sentAt__b__ = false;

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
                        __from__ = reader.ReadString();
                        __from__b__ = true;
                        break;
                    case 2:
                        __to__ = reader.ReadString();
                        __to__b__ = true;
                        break;
                    case 3:
                        __topic__ = reader.ReadString();
                        __topic__b__ = true;
                        break;
                    case 4:
                        __payload__ = reader.ReadString();
                        __payload__b__ = true;
                        break;
                    case 5:
                        __sentAt__ = reader.ReadString();
                        __sentAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Notification();
            if(__id__b__) ____result.id = __id__;
            if(__from__b__) ____result.from = __from__;
            if(__to__b__) ____result.to = __to__;
            if(__topic__b__) ____result.topic = __topic__;
            if(__payload__b__) ____result.payload = __payload__;
            if(__sentAt__b__) ____result.sentAt = __sentAt__;

            return ____result;
        }
    }


    public sealed class ChatMesssageFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ChatMesssage>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ChatMesssageFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("from"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("to"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("payload"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("receivedAt"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("from"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("to"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("payload"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("receivedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ChatMesssage value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.from);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.to);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.payload);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.receivedAt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ChatMesssage Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __from__ = default(string);
            var __from__b__ = false;
            var __to__ = default(string);
            var __to__b__ = false;
            var __payload__ = default(string);
            var __payload__b__ = false;
            var __receivedAt__ = default(string);
            var __receivedAt__b__ = false;

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
                        __from__ = reader.ReadString();
                        __from__b__ = true;
                        break;
                    case 2:
                        __to__ = reader.ReadString();
                        __to__b__ = true;
                        break;
                    case 3:
                        __payload__ = reader.ReadString();
                        __payload__b__ = true;
                        break;
                    case 4:
                        __receivedAt__ = reader.ReadString();
                        __receivedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ChatMesssage();
            if(__id__b__) ____result.id = __id__;
            if(__from__b__) ____result.from = __from__;
            if(__to__b__) ____result.to = __to__;
            if(__payload__b__) ____result.payload = __payload__;
            if(__receivedAt__b__) ____result.receivedAt = __receivedAt__;

            return ____result;
        }
    }


    public sealed class PersonalChatRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PersonalChatRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PersonalChatRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("to"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("payload"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("to"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("payload"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PersonalChatRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.to);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.payload);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PersonalChatRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __to__ = default(string);
            var __to__b__ = false;
            var __payload__ = default(string);
            var __payload__b__ = false;

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
                        __to__ = reader.ReadString();
                        __to__b__ = true;
                        break;
                    case 1:
                        __payload__ = reader.ReadString();
                        __payload__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PersonalChatRequest();
            if(__to__b__) ____result.to = __to__;
            if(__payload__b__) ____result.payload = __payload__;

            return ____result;
        }
    }


    public sealed class PartyInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("partyID"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("leaderID"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("members"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("invitees"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("invitationToken"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("partyID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("leaderID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("members"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("invitees"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("invitationToken"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.partyID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.leaderID);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.members, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.invitees, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.invitationToken);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __partyID__ = default(string);
            var __partyID__b__ = false;
            var __leaderID__ = default(string);
            var __leaderID__b__ = false;
            var __members__ = default(string[]);
            var __members__b__ = false;
            var __invitees__ = default(string[]);
            var __invitees__b__ = false;
            var __invitationToken__ = default(string);
            var __invitationToken__b__ = false;

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
                        __partyID__ = reader.ReadString();
                        __partyID__b__ = true;
                        break;
                    case 1:
                        __leaderID__ = reader.ReadString();
                        __leaderID__b__ = true;
                        break;
                    case 2:
                        __members__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __members__b__ = true;
                        break;
                    case 3:
                        __invitees__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __invitees__b__ = true;
                        break;
                    case 4:
                        __invitationToken__ = reader.ReadString();
                        __invitationToken__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyInfo();
            if(__partyID__b__) ____result.partyID = __partyID__;
            if(__leaderID__b__) ____result.leaderID = __leaderID__;
            if(__members__b__) ____result.members = __members__;
            if(__invitees__b__) ____result.invitees = __invitees__;
            if(__invitationToken__b__) ____result.invitationToken = __invitationToken__;

            return ____result;
        }
    }


    public sealed class PartyInviteRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyInviteRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyInviteRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendID"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("friendID"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyInviteRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.friendID);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyInviteRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __friendID__ = default(string);
            var __friendID__b__ = false;

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
                        __friendID__ = reader.ReadString();
                        __friendID__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyInviteRequest();
            if(__friendID__b__) ____result.friendID = __friendID__;

            return ____result;
        }
    }


    public sealed class PartyInvitationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyInvitation>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyInvitationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("from"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("partyID"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("invitationToken"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("from"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("partyID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("invitationToken"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyInvitation value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.from);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.partyID);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.invitationToken);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyInvitation Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __from__ = default(string);
            var __from__b__ = false;
            var __partyID__ = default(string);
            var __partyID__b__ = false;
            var __invitationToken__ = default(string);
            var __invitationToken__b__ = false;

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
                        __from__ = reader.ReadString();
                        __from__b__ = true;
                        break;
                    case 1:
                        __partyID__ = reader.ReadString();
                        __partyID__b__ = true;
                        break;
                    case 2:
                        __invitationToken__ = reader.ReadString();
                        __invitationToken__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyInvitation();
            if(__from__b__) ____result.from = __from__;
            if(__partyID__b__) ____result.partyID = __partyID__;
            if(__invitationToken__b__) ____result.invitationToken = __invitationToken__;

            return ____result;
        }
    }


    public sealed class PartyChatRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyChatRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyChatRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("payload"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("payload"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyChatRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.payload);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyChatRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __payload__ = default(string);
            var __payload__b__ = false;

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
                        __payload__ = reader.ReadString();
                        __payload__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyChatRequest();
            if(__payload__b__) ____result.payload = __payload__;

            return ____result;
        }
    }


    public sealed class PartyJoinRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyJoinRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyJoinRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("partyID"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("invitationToken"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("partyID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("invitationToken"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyJoinRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.partyID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.invitationToken);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyJoinRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __partyID__ = default(string);
            var __partyID__b__ = false;
            var __invitationToken__ = default(string);
            var __invitationToken__b__ = false;

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
                        __partyID__ = reader.ReadString();
                        __partyID__b__ = true;
                        break;
                    case 1:
                        __invitationToken__ = reader.ReadString();
                        __invitationToken__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyJoinRequest();
            if(__partyID__b__) ____result.partyID = __partyID__;
            if(__invitationToken__b__) ____result.invitationToken = __invitationToken__;

            return ____result;
        }
    }


    public sealed class PartyKickRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyKickRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyKickRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("memberID"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("memberID"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyKickRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.memberID);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyKickRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __memberID__ = default(string);
            var __memberID__b__ = false;

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
                        __memberID__ = reader.ReadString();
                        __memberID__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyKickRequest();
            if(__memberID__b__) ____result.memberID = __memberID__;

            return ____result;
        }
    }


    public sealed class JoinNotificationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.JoinNotification>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public JoinNotificationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userID"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userID"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.JoinNotification value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userID);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.JoinNotification Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userID__ = default(string);
            var __userID__b__ = false;

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
                        __userID__ = reader.ReadString();
                        __userID__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.JoinNotification();
            if(__userID__b__) ____result.userID = __userID__;

            return ____result;
        }
    }


    public sealed class KickNotificationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.KickNotification>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public KickNotificationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("leaderID"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userID"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("partyID"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("leaderID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("partyID"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.KickNotification value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.leaderID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.userID);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.partyID);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.KickNotification Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __leaderID__ = default(string);
            var __leaderID__b__ = false;
            var __userID__ = default(string);
            var __userID__b__ = false;
            var __partyID__ = default(string);
            var __partyID__b__ = false;

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
                        __leaderID__ = reader.ReadString();
                        __leaderID__b__ = true;
                        break;
                    case 1:
                        __userID__ = reader.ReadString();
                        __userID__b__ = true;
                        break;
                    case 2:
                        __partyID__ = reader.ReadString();
                        __partyID__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.KickNotification();
            if(__leaderID__b__) ____result.leaderID = __leaderID__;
            if(__userID__b__) ____result.userID = __userID__;
            if(__partyID__b__) ____result.partyID = __partyID__;

            return ____result;
        }
    }


    public sealed class LeaveNotificationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LeaveNotification>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LeaveNotificationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("leaderID"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userID"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("leaderID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userID"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LeaveNotification value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.leaderID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.userID);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LeaveNotification Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __leaderID__ = default(string);
            var __leaderID__b__ = false;
            var __userID__ = default(string);
            var __userID__b__ = false;

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
                        __leaderID__ = reader.ReadString();
                        __leaderID__b__ = true;
                        break;
                    case 1:
                        __userID__ = reader.ReadString();
                        __userID__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LeaveNotification();
            if(__leaderID__b__) ____result.leaderID = __leaderID__;
            if(__userID__b__) ____result.userID = __userID__;

            return ____result;
        }
    }


    public sealed class StartMatchmakingRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StartMatchmakingRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StartMatchmakingRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("gameMode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("serverName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("clientVersion"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("latencies"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("partyAttributes"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("gameMode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("serverName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("clientVersion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("latencies"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("partyAttributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StartMatchmakingRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.gameMode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.serverName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.clientVersion);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.latencies);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.partyAttributes);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StartMatchmakingRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __gameMode__ = default(string);
            var __gameMode__b__ = false;
            var __serverName__ = default(string);
            var __serverName__b__ = false;
            var __clientVersion__ = default(string);
            var __clientVersion__b__ = false;
            var __latencies__ = default(string);
            var __latencies__b__ = false;
            var __partyAttributes__ = default(string);
            var __partyAttributes__b__ = false;

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
                        __gameMode__ = reader.ReadString();
                        __gameMode__b__ = true;
                        break;
                    case 1:
                        __serverName__ = reader.ReadString();
                        __serverName__b__ = true;
                        break;
                    case 2:
                        __clientVersion__ = reader.ReadString();
                        __clientVersion__b__ = true;
                        break;
                    case 3:
                        __latencies__ = reader.ReadString();
                        __latencies__b__ = true;
                        break;
                    case 4:
                        __partyAttributes__ = reader.ReadString();
                        __partyAttributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StartMatchmakingRequest();
            if(__gameMode__b__) ____result.gameMode = __gameMode__;
            if(__serverName__b__) ____result.serverName = __serverName__;
            if(__clientVersion__b__) ____result.clientVersion = __clientVersion__;
            if(__latencies__b__) ____result.latencies = __latencies__;
            if(__partyAttributes__b__) ____result.partyAttributes = __partyAttributes__;

            return ____result;
        }
    }


    public sealed class MatchmakingNotifFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MatchmakingNotif>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchmakingNotifFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matchId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("matchId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MatchmakingNotif value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.matchId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MatchmakingNotif Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __status__ = default(string);
            var __status__b__ = false;
            var __matchId__ = default(string);
            var __matchId__b__ = false;

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
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 1:
                        __matchId__ = reader.ReadString();
                        __matchId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MatchmakingNotif();
            if(__status__b__) ____result.status = __status__;
            if(__matchId__b__) ____result.matchId = __matchId__;

            return ____result;
        }
    }


    public sealed class DsNotifFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.DsNotif>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DsNotifFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matchId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("podName"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("message"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("isOK"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("matchId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("podName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ip"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("message"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isOK"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.DsNotif value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.matchId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.podName);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.ip);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.port);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.message);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.isOK);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.DsNotif Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __status__ = default(string);
            var __status__b__ = false;
            var __matchId__ = default(string);
            var __matchId__b__ = false;
            var __podName__ = default(string);
            var __podName__b__ = false;
            var __ip__ = default(string);
            var __ip__b__ = false;
            var __port__ = default(int);
            var __port__b__ = false;
            var __message__ = default(string);
            var __message__b__ = false;
            var __isOK__ = default(string);
            var __isOK__b__ = false;

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
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 1:
                        __matchId__ = reader.ReadString();
                        __matchId__b__ = true;
                        break;
                    case 2:
                        __podName__ = reader.ReadString();
                        __podName__b__ = true;
                        break;
                    case 3:
                        __ip__ = reader.ReadString();
                        __ip__b__ = true;
                        break;
                    case 4:
                        __port__ = reader.ReadInt32();
                        __port__b__ = true;
                        break;
                    case 5:
                        __message__ = reader.ReadString();
                        __message__b__ = true;
                        break;
                    case 6:
                        __isOK__ = reader.ReadString();
                        __isOK__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.DsNotif();
            if(__status__b__) ____result.status = __status__;
            if(__matchId__b__) ____result.matchId = __matchId__;
            if(__podName__b__) ____result.podName = __podName__;
            if(__ip__b__) ____result.ip = __ip__;
            if(__port__b__) ____result.port = __port__;
            if(__message__b__) ____result.message = __message__;
            if(__isOK__b__) ____result.isOK = __isOK__;

            return ____result;
        }
    }


    public sealed class MatchmakingCodeFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MatchmakingCode>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchmakingCodeFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("code"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("code"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MatchmakingCode value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.code);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MatchmakingCode Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __code__ = default(int);
            var __code__b__ = false;

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
                        __code__ = reader.ReadInt32();
                        __code__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MatchmakingCode();
            if(__code__b__) ____result.code = __code__;

            return ____result;
        }
    }


    public sealed class ReadyConsentRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ReadyConsentRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ReadyConsentRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matchId"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("matchId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ReadyConsentRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.matchId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ReadyConsentRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __matchId__ = default(string);
            var __matchId__b__ = false;

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
                        __matchId__ = reader.ReadString();
                        __matchId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ReadyConsentRequest();
            if(__matchId__b__) ____result.matchId = __matchId__;

            return ____result;
        }
    }


    public sealed class ReadyForMatchConfirmationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ReadyForMatchConfirmation>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ReadyForMatchConfirmationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matchId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("matchId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ReadyForMatchConfirmation value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.matchId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ReadyForMatchConfirmation Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __matchId__ = default(string);
            var __matchId__b__ = false;
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
                        __matchId__ = reader.ReadString();
                        __matchId__b__ = true;
                        break;
                    case 1:
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

            var ____result = new global::AccelByte.Models.ReadyForMatchConfirmation();
            if(__matchId__b__) ____result.matchId = __matchId__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class RematchmakingNotificationFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RematchmakingNotification>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RematchmakingNotificationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("banDuration"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("banDuration"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RematchmakingNotification value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.banDuration);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RematchmakingNotification Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __banDuration__ = default(int);
            var __banDuration__b__ = false;

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
                        __banDuration__ = reader.ReadInt32();
                        __banDuration__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RematchmakingNotification();
            if(__banDuration__b__) ____result.banDuration = __banDuration__;

            return ____result;
        }
    }


    public sealed class FriendshipStatusFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.FriendshipStatus>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FriendshipStatusFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendshipStatus"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("friendshipStatus"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.FriendshipStatus value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RelationshipStatusCode>().Serialize(ref writer, value.friendshipStatus, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.FriendshipStatus Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __friendshipStatus__ = default(global::AccelByte.Models.RelationshipStatusCode);
            var __friendshipStatus__b__ = false;

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
                        __friendshipStatus__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RelationshipStatusCode>().Deserialize(ref reader, formatterResolver);
                        __friendshipStatus__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.FriendshipStatus();
            if(__friendshipStatus__b__) ____result.friendshipStatus = __friendshipStatus__;

            return ____result;
        }
    }


    public sealed class FriendsFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Friends>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FriendsFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendsId"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("friendsId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Friends value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.friendsId, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Friends Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __friendsId__ = default(string[]);
            var __friendsId__b__ = false;

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
                        __friendsId__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __friendsId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Friends();
            if(__friendsId__b__) ____result.friendsId = __friendsId__;

            return ____result;
        }
    }


    public sealed class FriendFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Friend>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FriendFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendId"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("friendId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Friend value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.friendId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Friend Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __friendId__ = default(string);
            var __friendId__b__ = false;

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
                        __friendId__ = reader.ReadString();
                        __friendId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Friend();
            if(__friendId__b__) ____result.friendId = __friendId__;

            return ____result;
        }
    }


    public sealed class FriendsStatusFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.FriendsStatus>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FriendsStatusFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendsId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("availability"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("activity"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastSeenAt"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("friendsId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("availability"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("activity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastSeenAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.FriendsStatus value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.friendsId, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.availability, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.activity, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime[]>().Serialize(ref writer, value.lastSeenAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.FriendsStatus Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __friendsId__ = default(string[]);
            var __friendsId__b__ = false;
            var __availability__ = default(string[]);
            var __availability__b__ = false;
            var __activity__ = default(string[]);
            var __activity__b__ = false;
            var __lastSeenAt__ = default(global::System.DateTime[]);
            var __lastSeenAt__b__ = false;

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
                        __friendsId__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __friendsId__b__ = true;
                        break;
                    case 1:
                        __availability__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __availability__b__ = true;
                        break;
                    case 2:
                        __activity__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __activity__b__ = true;
                        break;
                    case 3:
                        __lastSeenAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime[]>().Deserialize(ref reader, formatterResolver);
                        __lastSeenAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.FriendsStatus();
            if(__friendsId__b__) ____result.friendsId = __friendsId__;
            if(__availability__b__) ____result.availability = __availability__;
            if(__activity__b__) ____result.activity = __activity__;
            if(__lastSeenAt__b__) ____result.lastSeenAt = __lastSeenAt__;

            return ____result;
        }
    }


    public sealed class BulkFriendsRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BulkFriendsRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BulkFriendsRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendIds"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("friendIds"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BulkFriendsRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.friendIds, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.BulkFriendsRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __friendIds__ = default(string[]);
            var __friendIds__b__ = false;

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
                        __friendIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __friendIds__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.BulkFriendsRequest();
            if(__friendIds__b__) ____result.friendIds = __friendIds__;

            return ____result;
        }
    }


    public sealed class FriendsStatusNotifFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.FriendsStatusNotif>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FriendsStatusNotifFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userID"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("availability"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("activity"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastSeenAt"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("availability"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("activity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastSeenAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.FriendsStatusNotif value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.availability);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.activity);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.lastSeenAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.FriendsStatusNotif Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userID__ = default(string);
            var __userID__b__ = false;
            var __availability__ = default(string);
            var __availability__b__ = false;
            var __activity__ = default(string);
            var __activity__b__ = false;
            var __lastSeenAt__ = default(global::System.DateTime);
            var __lastSeenAt__b__ = false;

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
                        __userID__ = reader.ReadString();
                        __userID__b__ = true;
                        break;
                    case 1:
                        __availability__ = reader.ReadString();
                        __availability__b__ = true;
                        break;
                    case 2:
                        __activity__ = reader.ReadString();
                        __activity__b__ = true;
                        break;
                    case 3:
                        __lastSeenAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __lastSeenAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.FriendsStatusNotif();
            if(__userID__b__) ____result.userID = __userID__;
            if(__availability__b__) ____result.availability = __availability__;
            if(__activity__b__) ____result.activity = __activity__;
            if(__lastSeenAt__b__) ____result.lastSeenAt = __lastSeenAt__;

            return ____result;
        }
    }


    public sealed class OnlineFriendsFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OnlineFriends>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OnlineFriendsFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("onlineFriendsId"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("onlineFriendsId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.OnlineFriends value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.onlineFriendsId, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.OnlineFriends Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __onlineFriendsId__ = default(string[]);
            var __onlineFriendsId__b__ = false;

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
                        __onlineFriendsId__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __onlineFriendsId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.OnlineFriends();
            if(__onlineFriendsId__b__) ____result.onlineFriendsId = __onlineFriendsId__;

            return ____result;
        }
    }


    public sealed class PlatformLinkFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PlatformLink>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PlatformLinkFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("linkedAt"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("originNamespace"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformUserId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("linkedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("originNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PlatformLink value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.linkedAt);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.originNamespace);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.platformId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.platformUserId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PlatformLink Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __linkedAt__ = default(string);
            var __linkedAt__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __originNamespace__ = default(string);
            var __originNamespace__b__ = false;
            var __platformId__ = default(string);
            var __platformId__b__ = false;
            var __platformUserId__ = default(string);
            var __platformUserId__b__ = false;
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
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 1:
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 2:
                        __linkedAt__ = reader.ReadString();
                        __linkedAt__b__ = true;
                        break;
                    case 3:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 4:
                        __originNamespace__ = reader.ReadString();
                        __originNamespace__b__ = true;
                        break;
                    case 5:
                        __platformId__ = reader.ReadString();
                        __platformId__b__ = true;
                        break;
                    case 6:
                        __platformUserId__ = reader.ReadString();
                        __platformUserId__b__ = true;
                        break;
                    case 7:
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

            var ____result = new global::AccelByte.Models.PlatformLink();
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__linkedAt__b__) ____result.linkedAt = __linkedAt__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__originNamespace__b__) ____result.originNamespace = __originNamespace__;
            if(__platformId__b__) ____result.platformId = __platformId__;
            if(__platformUserId__b__) ____result.platformUserId = __platformUserId__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class CollectionFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Collection>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CollectionFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("slots"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userGameProfiles"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("gameProfiles"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("orderHistories"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformLinks"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("publicUserProfiles"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("slots"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userGameProfiles"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("gameProfiles"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("orderHistories"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformLinks"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("publicUserProfiles"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Collection value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Slot[]>().Serialize(ref writer, value.slots, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserGameProfiles[]>().Serialize(ref writer, value.userGameProfiles, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.GameProfile[]>().Serialize(ref writer, value.gameProfiles, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderHistoryInfo[]>().Serialize(ref writer, value.orderHistories, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PlatformLink[]>().Serialize(ref writer, value.platformLinks, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PublicUserProfile[]>().Serialize(ref writer, value.publicUserProfiles, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Collection Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __slots__ = default(global::AccelByte.Models.Slot[]);
            var __slots__b__ = false;
            var __userGameProfiles__ = default(global::AccelByte.Models.UserGameProfiles[]);
            var __userGameProfiles__b__ = false;
            var __gameProfiles__ = default(global::AccelByte.Models.GameProfile[]);
            var __gameProfiles__b__ = false;
            var __orderHistories__ = default(global::AccelByte.Models.OrderHistoryInfo[]);
            var __orderHistories__b__ = false;
            var __platformLinks__ = default(global::AccelByte.Models.PlatformLink[]);
            var __platformLinks__b__ = false;
            var __publicUserProfiles__ = default(global::AccelByte.Models.PublicUserProfile[]);
            var __publicUserProfiles__b__ = false;

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
                        __slots__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Slot[]>().Deserialize(ref reader, formatterResolver);
                        __slots__b__ = true;
                        break;
                    case 1:
                        __userGameProfiles__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.UserGameProfiles[]>().Deserialize(ref reader, formatterResolver);
                        __userGameProfiles__b__ = true;
                        break;
                    case 2:
                        __gameProfiles__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.GameProfile[]>().Deserialize(ref reader, formatterResolver);
                        __gameProfiles__b__ = true;
                        break;
                    case 3:
                        __orderHistories__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderHistoryInfo[]>().Deserialize(ref reader, formatterResolver);
                        __orderHistories__b__ = true;
                        break;
                    case 4:
                        __platformLinks__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PlatformLink[]>().Deserialize(ref reader, formatterResolver);
                        __platformLinks__b__ = true;
                        break;
                    case 5:
                        __publicUserProfiles__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PublicUserProfile[]>().Deserialize(ref reader, formatterResolver);
                        __publicUserProfiles__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Collection();
            if(__slots__b__) ____result.slots = __slots__;
            if(__userGameProfiles__b__) ____result.userGameProfiles = __userGameProfiles__;
            if(__gameProfiles__b__) ____result.gameProfiles = __gameProfiles__;
            if(__orderHistories__b__) ____result.orderHistories = __orderHistories__;
            if(__platformLinks__b__) ____result.platformLinks = __platformLinks__;
            if(__publicUserProfiles__b__) ____result.publicUserProfiles = __publicUserProfiles__;

            return ____result;
        }
    }


    public sealed class QosServerFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.QosServer>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public QosServerFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("last_update"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ip"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("last_update"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.QosServer value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ip);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.port);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.last_update);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.QosServer Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __ip__ = default(string);
            var __ip__b__ = false;
            var __port__ = default(int);
            var __port__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __last_update__ = default(string);
            var __last_update__b__ = false;

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
                        __ip__ = reader.ReadString();
                        __ip__b__ = true;
                        break;
                    case 1:
                        __port__ = reader.ReadInt32();
                        __port__b__ = true;
                        break;
                    case 2:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 3:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 4:
                        __last_update__ = reader.ReadString();
                        __last_update__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.QosServer();
            if(__ip__b__) ____result.ip = __ip__;
            if(__port__b__) ____result.port = __port__;
            if(__region__b__) ____result.region = __region__;
            if(__status__b__) ____result.status = __status__;
            if(__last_update__b__) ____result.last_update = __last_update__;

            return ____result;
        }
    }


    public sealed class QosServerListFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.QosServerList>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public QosServerListFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("servers"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("servers"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.QosServerList value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.QosServer[]>().Serialize(ref writer, value.servers, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.QosServerList Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __servers__ = default(global::AccelByte.Models.QosServer[]);
            var __servers__b__ = false;

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
                        __servers__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.QosServer[]>().Deserialize(ref reader, formatterResolver);
                        __servers__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.QosServerList();
            if(__servers__b__) ____result.servers = __servers__;

            return ____result;
        }
    }


    public sealed class ServerConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ServerConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ServerConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("BaseUrl"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("IamServerUrl"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("DSMControllerServerUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("StatisticServerUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PlatformServerUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("QosManagerServerUrl"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameTelemetryServerUrl"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("AchievementServerUrl"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientId"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientSecret"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("RedirectUri"), 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("BaseUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("IamServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("DSMControllerServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("StatisticServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PlatformServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("QosManagerServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameTelemetryServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("AchievementServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ClientId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ClientSecret"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("RedirectUri"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ServerConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.BaseUrl);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.IamServerUrl);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.DSMControllerServerUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.StatisticServerUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.PlatformServerUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.QosManagerServerUrl);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.GameTelemetryServerUrl);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.AchievementServerUrl);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.ClientId);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.ClientSecret);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.RedirectUri);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ServerConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __BaseUrl__ = default(string);
            var __BaseUrl__b__ = false;
            var __IamServerUrl__ = default(string);
            var __IamServerUrl__b__ = false;
            var __DSMControllerServerUrl__ = default(string);
            var __DSMControllerServerUrl__b__ = false;
            var __StatisticServerUrl__ = default(string);
            var __StatisticServerUrl__b__ = false;
            var __PlatformServerUrl__ = default(string);
            var __PlatformServerUrl__b__ = false;
            var __QosManagerServerUrl__ = default(string);
            var __QosManagerServerUrl__b__ = false;
            var __GameTelemetryServerUrl__ = default(string);
            var __GameTelemetryServerUrl__b__ = false;
            var __AchievementServerUrl__ = default(string);
            var __AchievementServerUrl__b__ = false;
            var __ClientId__ = default(string);
            var __ClientId__b__ = false;
            var __ClientSecret__ = default(string);
            var __ClientSecret__b__ = false;
            var __RedirectUri__ = default(string);
            var __RedirectUri__b__ = false;

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
                        __BaseUrl__ = reader.ReadString();
                        __BaseUrl__b__ = true;
                        break;
                    case 2:
                        __IamServerUrl__ = reader.ReadString();
                        __IamServerUrl__b__ = true;
                        break;
                    case 3:
                        __DSMControllerServerUrl__ = reader.ReadString();
                        __DSMControllerServerUrl__b__ = true;
                        break;
                    case 4:
                        __StatisticServerUrl__ = reader.ReadString();
                        __StatisticServerUrl__b__ = true;
                        break;
                    case 5:
                        __PlatformServerUrl__ = reader.ReadString();
                        __PlatformServerUrl__b__ = true;
                        break;
                    case 6:
                        __QosManagerServerUrl__ = reader.ReadString();
                        __QosManagerServerUrl__b__ = true;
                        break;
                    case 7:
                        __GameTelemetryServerUrl__ = reader.ReadString();
                        __GameTelemetryServerUrl__b__ = true;
                        break;
                    case 8:
                        __AchievementServerUrl__ = reader.ReadString();
                        __AchievementServerUrl__b__ = true;
                        break;
                    case 9:
                        __ClientId__ = reader.ReadString();
                        __ClientId__b__ = true;
                        break;
                    case 10:
                        __ClientSecret__ = reader.ReadString();
                        __ClientSecret__b__ = true;
                        break;
                    case 11:
                        __RedirectUri__ = reader.ReadString();
                        __RedirectUri__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ServerConfig();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__BaseUrl__b__) ____result.BaseUrl = __BaseUrl__;
            if(__IamServerUrl__b__) ____result.IamServerUrl = __IamServerUrl__;
            if(__DSMControllerServerUrl__b__) ____result.DSMControllerServerUrl = __DSMControllerServerUrl__;
            if(__StatisticServerUrl__b__) ____result.StatisticServerUrl = __StatisticServerUrl__;
            if(__PlatformServerUrl__b__) ____result.PlatformServerUrl = __PlatformServerUrl__;
            if(__QosManagerServerUrl__b__) ____result.QosManagerServerUrl = __QosManagerServerUrl__;
            if(__GameTelemetryServerUrl__b__) ____result.GameTelemetryServerUrl = __GameTelemetryServerUrl__;
            if(__AchievementServerUrl__b__) ____result.AchievementServerUrl = __AchievementServerUrl__;
            if(__ClientId__b__) ____result.ClientId = __ClientId__;
            if(__ClientSecret__b__) ____result.ClientSecret = __ClientSecret__;
            if(__RedirectUri__b__) ____result.RedirectUri = __RedirectUri__;

            return ____result;
        }
    }


    public sealed class RegisterServerRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RegisterServerRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RegisterServerRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_version"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("pod_name"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("provider"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("game_version"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ip"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("pod_name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("provider"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RegisterServerRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.game_version);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.ip);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.pod_name);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.port);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.provider);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RegisterServerRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __game_version__ = default(string);
            var __game_version__b__ = false;
            var __ip__ = default(string);
            var __ip__b__ = false;
            var __pod_name__ = default(string);
            var __pod_name__b__ = false;
            var __port__ = default(int);
            var __port__b__ = false;
            var __provider__ = default(string);
            var __provider__b__ = false;

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
                        __game_version__ = reader.ReadString();
                        __game_version__b__ = true;
                        break;
                    case 1:
                        __ip__ = reader.ReadString();
                        __ip__b__ = true;
                        break;
                    case 2:
                        __pod_name__ = reader.ReadString();
                        __pod_name__b__ = true;
                        break;
                    case 3:
                        __port__ = reader.ReadInt32();
                        __port__b__ = true;
                        break;
                    case 4:
                        __provider__ = reader.ReadString();
                        __provider__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RegisterServerRequest();
            if(__game_version__b__) ____result.game_version = __game_version__;
            if(__ip__b__) ____result.ip = __ip__;
            if(__pod_name__b__) ____result.pod_name = __pod_name__;
            if(__port__b__) ____result.port = __port__;
            if(__provider__b__) ____result.provider = __provider__;

            return ____result;
        }
    }


    public sealed class ShutdownServerRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ShutdownServerRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ShutdownServerRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("kill_me"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("pod_name"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("session_id"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("kill_me"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("pod_name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("session_id"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ShutdownServerRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteBoolean(value.kill_me);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.pod_name);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.session_id);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ShutdownServerRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __kill_me__ = default(bool);
            var __kill_me__b__ = false;
            var __pod_name__ = default(string);
            var __pod_name__b__ = false;
            var __session_id__ = default(string);
            var __session_id__b__ = false;

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
                        __kill_me__ = reader.ReadBoolean();
                        __kill_me__b__ = true;
                        break;
                    case 1:
                        __pod_name__ = reader.ReadString();
                        __pod_name__b__ = true;
                        break;
                    case 2:
                        __session_id__ = reader.ReadString();
                        __session_id__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ShutdownServerRequest();
            if(__kill_me__b__) ____result.kill_me = __kill_me__;
            if(__pod_name__b__) ____result.pod_name = __pod_name__;
            if(__session_id__b__) ____result.session_id = __session_id__;

            return ____result;
        }
    }


    public sealed class RegisterLocalServerRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RegisterLocalServerRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RegisterLocalServerRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ip"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RegisterLocalServerRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ip);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteUInt32(value.port);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RegisterLocalServerRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __ip__ = default(string);
            var __ip__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __port__ = default(uint);
            var __port__b__ = false;

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
                        __ip__ = reader.ReadString();
                        __ip__b__ = true;
                        break;
                    case 1:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 2:
                        __port__ = reader.ReadUInt32();
                        __port__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RegisterLocalServerRequest();
            if(__ip__b__) ____result.ip = __ip__;
            if(__name__b__) ____result.name = __name__;
            if(__port__b__) ____result.port = __port__;

            return ____result;
        }
    }


    public sealed class PartyMemberFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PartyMember>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PartyMemberFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_id"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("user_id"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PartyMember value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.user_id);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PartyMember Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __user_id__ = default(string);
            var __user_id__b__ = false;

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
                        __user_id__ = reader.ReadString();
                        __user_id__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PartyMember();
            if(__user_id__b__) ____result.user_id = __user_id__;

            return ____result;
        }
    }


    public sealed class MatchPartyFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MatchParty>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchPartyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("party_id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("party_members"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("party_attributes"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("party_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("party_members"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("party_attributes"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MatchParty value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.party_id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PartyMember[]>().Serialize(ref writer, value.party_members, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Serialize(ref writer, value.party_attributes, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MatchParty Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __party_id__ = default(string);
            var __party_id__b__ = false;
            var __party_members__ = default(global::AccelByte.Models.PartyMember[]);
            var __party_members__b__ = false;
            var __party_attributes__ = default(global::System.Collections.Generic.Dictionary<string, object>);
            var __party_attributes__b__ = false;

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
                        __party_id__ = reader.ReadString();
                        __party_id__b__ = true;
                        break;
                    case 1:
                        __party_members__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PartyMember[]>().Deserialize(ref reader, formatterResolver);
                        __party_members__b__ = true;
                        break;
                    case 2:
                        __party_attributes__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, object>>().Deserialize(ref reader, formatterResolver);
                        __party_attributes__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MatchParty();
            if(__party_id__b__) ____result.party_id = __party_id__;
            if(__party_members__b__) ____result.party_members = __party_members__;
            if(__party_attributes__b__) ____result.party_attributes = __party_attributes__;

            return ____result;
        }
    }


    public sealed class MatchingAllyFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MatchingAlly>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchingAllyFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matching_parties"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("matching_parties"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MatchingAlly value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.MatchParty[]>().Serialize(ref writer, value.matching_parties, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MatchingAlly Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __matching_parties__ = default(global::AccelByte.Models.MatchParty[]);
            var __matching_parties__b__ = false;

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
                        __matching_parties__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.MatchParty[]>().Deserialize(ref reader, formatterResolver);
                        __matching_parties__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MatchingAlly();
            if(__matching_parties__b__) ____result.matching_parties = __matching_parties__;

            return ____result;
        }
    }


    public sealed class MatchRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.MatchRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_mode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("matching_allies"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("session_id"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("game_mode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("matching_allies"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("session_id"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.MatchRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.game_mode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.MatchingAlly[]>().Serialize(ref writer, value.matching_allies, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.session_id);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.MatchRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __game_mode__ = default(string);
            var __game_mode__b__ = false;
            var __matching_allies__ = default(global::AccelByte.Models.MatchingAlly[]);
            var __matching_allies__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __session_id__ = default(string);
            var __session_id__b__ = false;

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
                        __game_mode__ = reader.ReadString();
                        __game_mode__b__ = true;
                        break;
                    case 1:
                        __matching_allies__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.MatchingAlly[]>().Deserialize(ref reader, formatterResolver);
                        __matching_allies__b__ = true;
                        break;
                    case 2:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 3:
                        __session_id__ = reader.ReadString();
                        __session_id__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.MatchRequest();
            if(__game_mode__b__) ____result.game_mode = __game_mode__;
            if(__matching_allies__b__) ____result.matching_allies = __matching_allies__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__session_id__b__) ____result.session_id = __session_id__;

            return ____result;
        }
    }


    public sealed class DSMClientFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.DSMClient>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DSMClientFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("host_address"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("host_address"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.DSMClient value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.host_address);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.status);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.DSMClient Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __host_address__ = default(string);
            var __host_address__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;

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
                        __host_address__ = reader.ReadString();
                        __host_address__b__ = true;
                        break;
                    case 1:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 2:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.DSMClient();
            if(__host_address__b__) ____result.host_address = __host_address__;
            if(__region__b__) ____result.region = __region__;
            if(__status__b__) ____result.status = __status__;

            return ____result;
        }
    }


    public sealed class PubIpFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PubIp>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PubIpFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ip"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PubIp value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ip);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PubIp Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __ip__ = default(string);
            var __ip__b__ = false;

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
                        __ip__ = reader.ReadString();
                        __ip__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PubIp();
            if(__ip__b__) ____result.ip = __ip__;

            return ____result;
        }
    }


    public sealed class ServerInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.ServerInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ServerInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("pod_name"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("image_version"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("alternate_ips"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("provider"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("game_version"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("last_update"), 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("pod_name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("image_version"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ip"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("alternate_ips"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("provider"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("game_version"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("last_update"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.ServerInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.pod_name);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.image_version);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.ip);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.alternate_ips, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.port);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.provider);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.game_version);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.last_update);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ServerInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __pod_name__ = default(string);
            var __pod_name__b__ = false;
            var __image_version__ = default(string);
            var __image_version__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __ip__ = default(string);
            var __ip__b__ = false;
            var __alternate_ips__ = default(string[]);
            var __alternate_ips__b__ = false;
            var __port__ = default(int);
            var __port__b__ = false;
            var __provider__ = default(string);
            var __provider__b__ = false;
            var __game_version__ = default(string);
            var __game_version__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __last_update__ = default(string);
            var __last_update__b__ = false;

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
                        __pod_name__ = reader.ReadString();
                        __pod_name__b__ = true;
                        break;
                    case 1:
                        __image_version__ = reader.ReadString();
                        __image_version__b__ = true;
                        break;
                    case 2:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 3:
                        __ip__ = reader.ReadString();
                        __ip__b__ = true;
                        break;
                    case 4:
                        __alternate_ips__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __alternate_ips__b__ = true;
                        break;
                    case 5:
                        __port__ = reader.ReadInt32();
                        __port__b__ = true;
                        break;
                    case 6:
                        __provider__ = reader.ReadString();
                        __provider__b__ = true;
                        break;
                    case 7:
                        __game_version__ = reader.ReadString();
                        __game_version__b__ = true;
                        break;
                    case 8:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 9:
                        __last_update__ = reader.ReadString();
                        __last_update__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ServerInfo();
            if(__pod_name__b__) ____result.pod_name = __pod_name__;
            if(__image_version__b__) ____result.image_version = __image_version__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__ip__b__) ____result.ip = __ip__;
            if(__alternate_ips__b__) ____result.alternate_ips = __alternate_ips__;
            if(__port__b__) ____result.port = __port__;
            if(__provider__b__) ____result.provider = __provider__;
            if(__game_version__b__) ____result.game_version = __game_version__;
            if(__status__b__) ____result.status = __status__;
            if(__last_update__b__) ____result.last_update = __last_update__;

            return ____result;
        }
    }


    public sealed class StatConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultValue"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("incrementOnly"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maximum"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("minimum"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("setAsGlobal"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("setBy"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 12},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultValue"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("incrementOnly"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maximum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minimum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("setAsGlobal"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("setBy"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteSingle(value.defaultValue);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.incrementOnly);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteSingle(value.maximum);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteSingle(value.minimum);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteBoolean(value.setAsGlobal);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatisticSetBy>().Serialize(ref writer, value.setBy, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatisticStatus>().Serialize(ref writer, value.status, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.updatedAt);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
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
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __setAsGlobal__ = default(bool);
            var __setAsGlobal__b__ = false;
            var __setBy__ = default(global::AccelByte.Models.StatisticSetBy);
            var __setBy__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __status__ = default(global::AccelByte.Models.StatisticStatus);
            var __status__b__ = false;
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
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 1:
                        __defaultValue__ = reader.ReadSingle();
                        __defaultValue__b__ = true;
                        break;
                    case 2:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 3:
                        __incrementOnly__ = reader.ReadBoolean();
                        __incrementOnly__b__ = true;
                        break;
                    case 4:
                        __maximum__ = reader.ReadSingle();
                        __maximum__b__ = true;
                        break;
                    case 5:
                        __minimum__ = reader.ReadSingle();
                        __minimum__b__ = true;
                        break;
                    case 6:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 7:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 8:
                        __setAsGlobal__ = reader.ReadBoolean();
                        __setAsGlobal__b__ = true;
                        break;
                    case 9:
                        __setBy__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatisticSetBy>().Deserialize(ref reader, formatterResolver);
                        __setBy__b__ = true;
                        break;
                    case 10:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 11:
                        __status__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatisticStatus>().Deserialize(ref reader, formatterResolver);
                        __status__b__ = true;
                        break;
                    case 12:
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

            var ____result = new global::AccelByte.Models.StatConfig();
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__defaultValue__b__) ____result.defaultValue = __defaultValue__;
            if(__description__b__) ____result.description = __description__;
            if(__incrementOnly__b__) ____result.incrementOnly = __incrementOnly__;
            if(__maximum__b__) ____result.maximum = __maximum__;
            if(__minimum__b__) ____result.minimum = __minimum__;
            if(__name__b__) ____result.name = __name__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__setAsGlobal__b__) ____result.setAsGlobal = __setAsGlobal__;
            if(__setBy__b__) ____result.setBy = __setBy__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__status__b__) ____result.status = __status__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class StatItemFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatItem>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatItemFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statName"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatItem value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.createdAt);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.statName);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.updatedAt);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteSingle(value.value);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatItem Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __statName__ = default(string);
            var __statName__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __updatedAt__ = default(string);
            var __updatedAt__b__ = false;
            var __value__ = default(float);
            var __value__b__ = false;

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
                        __createdAt__ = reader.ReadString();
                        __createdAt__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 3:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 4:
                        __statName__ = reader.ReadString();
                        __statName__b__ = true;
                        break;
                    case 5:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 6:
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    case 7:
                        __value__ = reader.ReadSingle();
                        __value__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StatItem();
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__statName__b__) ____result.statName = __statName__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__value__b__) ____result.value = __value__;

            return ____result;
        }
    }


    public sealed class CreateStatItemRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CreateStatItemRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CreateStatItemRequestFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CreateStatItemRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

        public global::AccelByte.Models.CreateStatItemRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

            var ____result = new global::AccelByte.Models.CreateStatItemRequest();
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class PagedStatItemsFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedStatItems>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedStatItemsFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedStatItems value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatItem[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PagedStatItems Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.StatItem[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatItem[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PagedStatItems();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class UserStatItemIncrementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserStatItemIncrement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserStatItemIncrementFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("inc"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("inc"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserStatItemIncrement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.inc);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserStatItemIncrement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __inc__ = default(float);
            var __inc__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
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
                        __inc__ = reader.ReadSingle();
                        __inc__b__ = true;
                        break;
                    case 1:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 2:
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

            var ____result = new global::AccelByte.Models.UserStatItemIncrement();
            if(__inc__b__) ____result.inc = __inc__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class StatItemIncrementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatItemIncrement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatItemIncrementFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("inc"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("inc"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatItemIncrement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.inc);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatItemIncrement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __inc__ = default(float);
            var __inc__b__ = false;
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
                        __inc__ = reader.ReadSingle();
                        __inc__b__ = true;
                        break;
                    case 1:
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

            var ____result = new global::AccelByte.Models.StatItemIncrement();
            if(__inc__b__) ____result.inc = __inc__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class StatItemOperationResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatItemOperationResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatItemOperationResultFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("details"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("success"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("details"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("success"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatItemOperationResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.details, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.success);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatItemOperationResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __details__ = default(object);
            var __details__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __success__ = default(bool);
            var __success__b__ = false;

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
                        __details__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __details__b__ = true;
                        break;
                    case 1:
                        __statCode__ = reader.ReadString();
                        __statCode__b__ = true;
                        break;
                    case 2:
                        __success__ = reader.ReadBoolean();
                        __success__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StatItemOperationResult();
            if(__details__b__) ____result.details = __details__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__success__b__) ____result.success = __success__;

            return ____result;
        }
    }


    public sealed class TelemetryEventTagFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.TelemetryEventTag>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TelemetryEventTagFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("AppId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Id"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Level"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Type"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("UX"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("AppId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Level"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("UX"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.TelemetryEventTag value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteUInt32(value.AppId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt32(value.Id);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteUInt32(value.Level);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteUInt32(value.Type);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteUInt32(value.UX);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.TelemetryEventTag Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __AppId__ = default(uint);
            var __AppId__b__ = false;
            var __Id__ = default(uint);
            var __Id__b__ = false;
            var __Level__ = default(uint);
            var __Level__b__ = false;
            var __Type__ = default(uint);
            var __Type__b__ = false;
            var __UX__ = default(uint);
            var __UX__b__ = false;

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
                        __AppId__ = reader.ReadUInt32();
                        __AppId__b__ = true;
                        break;
                    case 1:
                        __Id__ = reader.ReadUInt32();
                        __Id__b__ = true;
                        break;
                    case 2:
                        __Level__ = reader.ReadUInt32();
                        __Level__b__ = true;
                        break;
                    case 3:
                        __Type__ = reader.ReadUInt32();
                        __Type__b__ = true;
                        break;
                    case 4:
                        __UX__ = reader.ReadUInt32();
                        __UX__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.TelemetryEventTag();
            if(__AppId__b__) ____result.AppId = __AppId__;
            if(__Id__b__) ____result.Id = __Id__;
            if(__Level__b__) ____result.Level = __Level__;
            if(__Type__b__) ____result.Type = __Type__;
            if(__UX__b__) ____result.UX = __UX__;

            return ____result;
        }
    }


    public sealed class TokenDataFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.TokenData>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TokenDataFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("access_token"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("refresh_token"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("expires_in"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("token_type"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("user_id"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("display_name"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("access_token"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("refresh_token"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("expires_in"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("token_type"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("user_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("display_name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.TokenData value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.access_token);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.refresh_token);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.expires_in);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.token_type);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.user_id);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.display_name);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.Namespace);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.TokenData Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __access_token__ = default(string);
            var __access_token__b__ = false;
            var __refresh_token__ = default(string);
            var __refresh_token__b__ = false;
            var __expires_in__ = default(int);
            var __expires_in__b__ = false;
            var __token_type__ = default(string);
            var __token_type__b__ = false;
            var __user_id__ = default(string);
            var __user_id__b__ = false;
            var __display_name__ = default(string);
            var __display_name__b__ = false;
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
                        __access_token__ = reader.ReadString();
                        __access_token__b__ = true;
                        break;
                    case 1:
                        __refresh_token__ = reader.ReadString();
                        __refresh_token__b__ = true;
                        break;
                    case 2:
                        __expires_in__ = reader.ReadInt32();
                        __expires_in__b__ = true;
                        break;
                    case 3:
                        __token_type__ = reader.ReadString();
                        __token_type__b__ = true;
                        break;
                    case 4:
                        __user_id__ = reader.ReadString();
                        __user_id__b__ = true;
                        break;
                    case 5:
                        __display_name__ = reader.ReadString();
                        __display_name__b__ = true;
                        break;
                    case 6:
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

            var ____result = new global::AccelByte.Models.TokenData();
            if(__access_token__b__) ____result.access_token = __access_token__;
            if(__refresh_token__b__) ____result.refresh_token = __refresh_token__;
            if(__expires_in__b__) ____result.expires_in = __expires_in__;
            if(__token_type__b__) ____result.token_type = __token_type__;
            if(__user_id__b__) ____result.user_id = __user_id__;
            if(__display_name__b__) ____result.display_name = __display_name__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;

            return ____result;
        }
    }


    public sealed class SessionDataFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.SessionData>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SessionDataFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("session_id"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("expires_in"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("refresh_id"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("session_id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("expires_in"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("refresh_id"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.SessionData value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.session_id);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.expires_in);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.refresh_id);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.SessionData Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __session_id__ = default(string);
            var __session_id__b__ = false;
            var __expires_in__ = default(int);
            var __expires_in__b__ = false;
            var __refresh_id__ = default(string);
            var __refresh_id__b__ = false;

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
                        __session_id__ = reader.ReadString();
                        __session_id__b__ = true;
                        break;
                    case 1:
                        __expires_in__ = reader.ReadInt32();
                        __expires_in__b__ = true;
                        break;
                    case 2:
                        __refresh_id__ = reader.ReadString();
                        __refresh_id__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.SessionData();
            if(__session_id__b__) ____result.session_id = __session_id__;
            if(__expires_in__b__) ____result.expires_in = __expires_in__;
            if(__refresh_id__b__) ____result.refresh_id = __refresh_id__;

            return ____result;
        }
    }


    public sealed class BanFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Ban>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BanFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ban"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("banId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("endDate"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ban"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("endDate"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Ban value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ban);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.banId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.endDate, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Ban Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __ban__ = default(string);
            var __ban__b__ = false;
            var __banId__ = default(string);
            var __banId__b__ = false;
            var __endDate__ = default(global::System.DateTime);
            var __endDate__b__ = false;

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
                        __ban__ = reader.ReadString();
                        __ban__b__ = true;
                        break;
                    case 1:
                        __banId__ = reader.ReadString();
                        __banId__b__ = true;
                        break;
                    case 2:
                        __endDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __endDate__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Ban();
            if(__ban__b__) ____result.ban = __ban__;
            if(__banId__b__) ____result.banId = __banId__;
            if(__endDate__b__) ____result.endDate = __endDate__;

            return ____result;
        }
    }


    public sealed class PermissionFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Permission>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PermissionFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("action"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("resource"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("schedAction"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("schedCron"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("schedRange"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("action"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("resource"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("schedAction"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("schedCron"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("schedRange"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Permission value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.action);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.resource);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.schedAction);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.schedCron);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.schedRange, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Permission Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __action__ = default(int);
            var __action__b__ = false;
            var __resource__ = default(string);
            var __resource__b__ = false;
            var __schedAction__ = default(int);
            var __schedAction__b__ = false;
            var __schedCron__ = default(string);
            var __schedCron__b__ = false;
            var __schedRange__ = default(string[]);
            var __schedRange__b__ = false;

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
                        __action__ = reader.ReadInt32();
                        __action__b__ = true;
                        break;
                    case 1:
                        __resource__ = reader.ReadString();
                        __resource__b__ = true;
                        break;
                    case 2:
                        __schedAction__ = reader.ReadInt32();
                        __schedAction__b__ = true;
                        break;
                    case 3:
                        __schedCron__ = reader.ReadString();
                        __schedCron__b__ = true;
                        break;
                    case 4:
                        __schedRange__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __schedRange__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.Permission();
            if(__action__b__) ____result.action = __action__;
            if(__resource__b__) ____result.resource = __resource__;
            if(__schedAction__b__) ____result.schedAction = __schedAction__;
            if(__schedCron__b__) ____result.schedCron = __schedCron__;
            if(__schedRange__b__) ____result.schedRange = __schedRange__;

            return ____result;
        }
    }


    public sealed class UserDataFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UserData>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserDataFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("authType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("bans"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("deletionStatus"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailVerified"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("enabled"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastDateOfBirthChangedTime"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("lastEnabledChangedTime"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("newEmailAddress"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("oldEmailAddress"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("permissions"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("phoneNumber"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("phoneVerified"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformId"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformUserId"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("roles"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userName"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("eligible"), 23},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("authType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bans"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("country"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("deletionStatus"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailVerified"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("enabled"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastDateOfBirthChangedTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastEnabledChangedTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("newEmailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("oldEmailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("phoneNumber"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("phoneVerified"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("roles"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("eligible"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UserData value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.authType);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Ban[]>().Serialize(ref writer, value.bans, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.country);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteBoolean(value.deletionStatus);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteBoolean(value.emailVerified);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value.enabled);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.lastDateOfBirthChangedTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.lastEnabledChangedTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.newEmailAddress);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.oldEmailAddress);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Permission[]>().Serialize(ref writer, value.permissions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.phoneNumber);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteBoolean(value.phoneVerified);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.platformId);
            writer.WriteRaw(this.____stringByteKeys[19]);
            writer.WriteString(value.platformUserId);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.roles, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[22]);
            writer.WriteString(value.userName);
            writer.WriteRaw(this.____stringByteKeys[23]);
            writer.WriteBoolean(value.eligible);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UserData Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __authType__ = default(string);
            var __authType__b__ = false;
            var __bans__ = default(global::AccelByte.Models.Ban[]);
            var __bans__b__ = false;
            var __country__ = default(string);
            var __country__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __deletionStatus__ = default(bool);
            var __deletionStatus__b__ = false;
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __emailVerified__ = default(bool);
            var __emailVerified__b__ = false;
            var __enabled__ = default(bool);
            var __enabled__b__ = false;
            var __lastDateOfBirthChangedTime__ = default(global::System.DateTime);
            var __lastDateOfBirthChangedTime__b__ = false;
            var __lastEnabledChangedTime__ = default(global::System.DateTime);
            var __lastEnabledChangedTime__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __newEmailAddress__ = default(string);
            var __newEmailAddress__b__ = false;
            var __oldEmailAddress__ = default(string);
            var __oldEmailAddress__b__ = false;
            var __permissions__ = default(global::AccelByte.Models.Permission[]);
            var __permissions__b__ = false;
            var __phoneNumber__ = default(string);
            var __phoneNumber__b__ = false;
            var __phoneVerified__ = default(bool);
            var __phoneVerified__b__ = false;
            var __platformId__ = default(string);
            var __platformId__b__ = false;
            var __platformUserId__ = default(string);
            var __platformUserId__b__ = false;
            var __roles__ = default(string[]);
            var __roles__b__ = false;
            var __userId__ = default(string);
            var __userId__b__ = false;
            var __userName__ = default(string);
            var __userName__b__ = false;
            var __eligible__ = default(bool);
            var __eligible__b__ = false;

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
                        __authType__ = reader.ReadString();
                        __authType__b__ = true;
                        break;
                    case 1:
                        __bans__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Ban[]>().Deserialize(ref reader, formatterResolver);
                        __bans__b__ = true;
                        break;
                    case 2:
                        __country__ = reader.ReadString();
                        __country__b__ = true;
                        break;
                    case 3:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 4:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 5:
                        __deletionStatus__ = reader.ReadBoolean();
                        __deletionStatus__b__ = true;
                        break;
                    case 6:
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 7:
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 8:
                        __emailVerified__ = reader.ReadBoolean();
                        __emailVerified__b__ = true;
                        break;
                    case 9:
                        __enabled__ = reader.ReadBoolean();
                        __enabled__b__ = true;
                        break;
                    case 10:
                        __lastDateOfBirthChangedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __lastDateOfBirthChangedTime__b__ = true;
                        break;
                    case 11:
                        __lastEnabledChangedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __lastEnabledChangedTime__b__ = true;
                        break;
                    case 12:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 13:
                        __newEmailAddress__ = reader.ReadString();
                        __newEmailAddress__b__ = true;
                        break;
                    case 14:
                        __oldEmailAddress__ = reader.ReadString();
                        __oldEmailAddress__b__ = true;
                        break;
                    case 15:
                        __permissions__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Permission[]>().Deserialize(ref reader, formatterResolver);
                        __permissions__b__ = true;
                        break;
                    case 16:
                        __phoneNumber__ = reader.ReadString();
                        __phoneNumber__b__ = true;
                        break;
                    case 17:
                        __phoneVerified__ = reader.ReadBoolean();
                        __phoneVerified__b__ = true;
                        break;
                    case 18:
                        __platformId__ = reader.ReadString();
                        __platformId__b__ = true;
                        break;
                    case 19:
                        __platformUserId__ = reader.ReadString();
                        __platformUserId__b__ = true;
                        break;
                    case 20:
                        __roles__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __roles__b__ = true;
                        break;
                    case 21:
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 22:
                        __userName__ = reader.ReadString();
                        __userName__b__ = true;
                        break;
                    case 23:
                        __eligible__ = reader.ReadBoolean();
                        __eligible__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UserData();
            if(__authType__b__) ____result.authType = __authType__;
            if(__bans__b__) ____result.bans = __bans__;
            if(__country__b__) ____result.country = __country__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__deletionStatus__b__) ____result.deletionStatus = __deletionStatus__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__emailVerified__b__) ____result.emailVerified = __emailVerified__;
            if(__enabled__b__) ____result.enabled = __enabled__;
            if(__lastDateOfBirthChangedTime__b__) ____result.lastDateOfBirthChangedTime = __lastDateOfBirthChangedTime__;
            if(__lastEnabledChangedTime__b__) ____result.lastEnabledChangedTime = __lastEnabledChangedTime__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__newEmailAddress__b__) ____result.newEmailAddress = __newEmailAddress__;
            if(__oldEmailAddress__b__) ____result.oldEmailAddress = __oldEmailAddress__;
            if(__permissions__b__) ____result.permissions = __permissions__;
            if(__phoneNumber__b__) ____result.phoneNumber = __phoneNumber__;
            if(__phoneVerified__b__) ____result.phoneVerified = __phoneVerified__;
            if(__platformId__b__) ____result.platformId = __platformId__;
            if(__platformUserId__b__) ____result.platformUserId = __platformUserId__;
            if(__roles__b__) ____result.roles = __roles__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__userName__b__) ____result.userName = __userName__;
            if(__eligible__b__) ____result.eligible = __eligible__;

            return ____result;
        }
    }


    public sealed class PublicUserInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PublicUserInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PublicUserInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("phoneNumber"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("country"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("phoneNumber"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PublicUserInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.country);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.phoneNumber);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PublicUserInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __country__ = default(string);
            var __country__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __phoneNumber__ = default(string);
            var __phoneNumber__b__ = false;
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
                        __country__ = reader.ReadString();
                        __country__b__ = true;
                        break;
                    case 1:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 2:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 3:
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 4:
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 5:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 6:
                        __phoneNumber__ = reader.ReadString();
                        __phoneNumber__b__ = true;
                        break;
                    case 7:
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

            var ____result = new global::AccelByte.Models.PublicUserInfo();
            if(__country__b__) ____result.country = __country__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__phoneNumber__b__) ____result.phoneNumber = __phoneNumber__;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class PagedPublicUsersInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedPublicUsersInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedPublicUsersInfoFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedPublicUsersInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PublicUserInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PagedPublicUsersInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.PublicUserInfo[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PublicUserInfo[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PagedPublicUsersInfo();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class RegisterUserRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RegisterUserRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RegisterUserRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("authType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("password"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("authType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("country"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("password"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RegisterUserRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AuthenticationType>().Serialize(ref writer, value.authType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.country);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.password);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RegisterUserRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __authType__ = default(global::AccelByte.Models.AuthenticationType);
            var __authType__b__ = false;
            var __country__ = default(string);
            var __country__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __password__ = default(string);
            var __password__b__ = false;

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
                        __authType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AuthenticationType>().Deserialize(ref reader, formatterResolver);
                        __authType__b__ = true;
                        break;
                    case 1:
                        __country__ = reader.ReadString();
                        __country__b__ = true;
                        break;
                    case 2:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 3:
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 4:
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 5:
                        __password__ = reader.ReadString();
                        __password__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RegisterUserRequest();
            if(__authType__b__) ____result.authType = __authType__;
            if(__country__b__) ____result.country = __country__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__password__b__) ____result.password = __password__;

            return ____result;
        }
    }


    public sealed class RegisterUserResponseFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RegisterUserResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RegisterUserResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("authType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("authType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("country"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RegisterUserResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AuthenticationType>().Serialize(ref writer, value.authType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.country);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RegisterUserResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __authType__ = default(global::AccelByte.Models.AuthenticationType);
            var __authType__b__ = false;
            var __country__ = default(string);
            var __country__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
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
                        __authType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AuthenticationType>().Deserialize(ref reader, formatterResolver);
                        __authType__b__ = true;
                        break;
                    case 1:
                        __country__ = reader.ReadString();
                        __country__b__ = true;
                        break;
                    case 2:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 3:
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 4:
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 5:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 6:
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

            var ____result = new global::AccelByte.Models.RegisterUserResponse();
            if(__authType__b__) ____result.authType = __authType__;
            if(__country__b__) ____result.country = __country__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class UpdateUserRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UpdateUserRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UpdateUserRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("dateOfBirth"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("emailAddress"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("languageTag"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("country"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("dateOfBirth"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("emailAddress"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("languageTag"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UpdateUserRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.country);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.dateOfBirth);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.emailAddress);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.languageTag);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UpdateUserRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __country__ = default(string);
            var __country__b__ = false;
            var __dateOfBirth__ = default(string);
            var __dateOfBirth__b__ = false;
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __emailAddress__ = default(string);
            var __emailAddress__b__ = false;
            var __languageTag__ = default(string);
            var __languageTag__b__ = false;

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
                        __country__ = reader.ReadString();
                        __country__b__ = true;
                        break;
                    case 1:
                        __dateOfBirth__ = reader.ReadString();
                        __dateOfBirth__b__ = true;
                        break;
                    case 2:
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 3:
                        __emailAddress__ = reader.ReadString();
                        __emailAddress__b__ = true;
                        break;
                    case 4:
                        __languageTag__ = reader.ReadString();
                        __languageTag__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UpdateUserRequest();
            if(__country__b__) ____result.country = __country__;
            if(__dateOfBirth__b__) ____result.dateOfBirth = __dateOfBirth__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__emailAddress__b__) ____result.emailAddress = __emailAddress__;
            if(__languageTag__b__) ____result.languageTag = __languageTag__;

            return ____result;
        }
    }


    public sealed class PagedPlatformLinksFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedPlatformLinks>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedPlatformLinksFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedPlatformLinks value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PlatformLink[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PagedPlatformLinks Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.PlatformLink[]);
            var __data__b__ = false;
            var __paging__ = default(global::AccelByte.Models.Paging);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PlatformLink[]>().Deserialize(ref reader, formatterResolver);
                        __data__b__ = true;
                        break;
                    case 1:
                        __paging__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Deserialize(ref reader, formatterResolver);
                        __paging__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.PagedPlatformLinks();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class BulkPlatformUserIdRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BulkPlatformUserIdRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BulkPlatformUserIdRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformUserIDs"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("platformUserIDs"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BulkPlatformUserIdRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.platformUserIDs, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.BulkPlatformUserIdRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __platformUserIDs__ = default(string[]);
            var __platformUserIDs__b__ = false;

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
                        __platformUserIDs__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __platformUserIDs__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.BulkPlatformUserIdRequest();
            if(__platformUserIDs__b__) ____result.platformUserIDs = __platformUserIDs__;

            return ____result;
        }
    }


    public sealed class PlatformUserIdMapFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PlatformUserIdMap>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PlatformUserIdMapFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PlatformUserIdMap value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PlatformUserIdMap Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

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

            var ____result = new global::AccelByte.Models.PlatformUserIdMap();
            if(__userId__b__) ____result.userId = __userId__;

            return ____result;
        }
    }


    public sealed class BulkPlatformUserIdResponseFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BulkPlatformUserIdResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BulkPlatformUserIdResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userIdPlatforms"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userIdPlatforms"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BulkPlatformUserIdResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PlatformUserIdMap[]>().Serialize(ref writer, value.userIdPlatforms, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.BulkPlatformUserIdResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userIdPlatforms__ = default(global::AccelByte.Models.PlatformUserIdMap[]);
            var __userIdPlatforms__b__ = false;

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
                        __userIdPlatforms__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PlatformUserIdMap[]>().Deserialize(ref reader, formatterResolver);
                        __userIdPlatforms__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.BulkPlatformUserIdResponse();
            if(__userIdPlatforms__b__) ____result.userIdPlatforms = __userIdPlatforms__;

            return ____result;
        }
    }


    public sealed class CountryInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.CountryInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CountryInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CountryCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CountryName"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("State"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("City"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("CountryCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CountryName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("State"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("City"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.CountryInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.CountryCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.CountryName);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.State);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.City);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.CountryInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __CountryCode__ = default(string);
            var __CountryCode__b__ = false;
            var __CountryName__ = default(string);
            var __CountryName__b__ = false;
            var __State__ = default(string);
            var __State__b__ = false;
            var __City__ = default(string);
            var __City__b__ = false;

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
                        __CountryCode__ = reader.ReadString();
                        __CountryCode__b__ = true;
                        break;
                    case 1:
                        __CountryName__ = reader.ReadString();
                        __CountryName__b__ = true;
                        break;
                    case 2:
                        __State__ = reader.ReadString();
                        __State__b__ = true;
                        break;
                    case 3:
                        __City__ = reader.ReadString();
                        __City__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.CountryInfo();
            if(__CountryCode__b__) ____result.CountryCode = __CountryCode__;
            if(__CountryName__b__) ____result.CountryName = __CountryName__;
            if(__State__b__) ____result.State = __State__;
            if(__City__b__) ____result.City = __City__;

            return ____result;
        }
    }


    public sealed class UpgradeUserRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.UpgradeUserRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UpgradeUserRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("temporary_session_id"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("temporary_session_id"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.UpgradeUserRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.temporary_session_id);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.UpgradeUserRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __temporary_session_id__ = default(string);
            var __temporary_session_id__b__ = false;

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
                        __temporary_session_id__ = reader.ReadString();
                        __temporary_session_id__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.UpgradeUserRequest();
            if(__temporary_session_id__b__) ____result.temporary_session_id = __temporary_session_id__;

            return ____result;
        }
    }


    public sealed class AccountLinkedPlatformFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AccountLinkedPlatform>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AccountLinkedPlatformFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformUserId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AccountLinkedPlatform value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.platformUserId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AccountLinkedPlatform Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __platformUserId__ = default(string);
            var __platformUserId__b__ = false;

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
                        __platformUserId__ = reader.ReadString();
                        __platformUserId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AccountLinkedPlatform();
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__platformUserId__b__) ____result.platformUserId = __platformUserId__;

            return ____result;
        }
    }


    public sealed class AccountLinkPublisherAccountFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AccountLinkPublisherAccount>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AccountLinkPublisherAccountFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("linkedPlatforms"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("linkedPlatforms"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AccountLinkPublisherAccount value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.namespace_);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AccountLinkedPlatform[]>().Serialize(ref writer, value.linkedPlatforms, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AccountLinkPublisherAccount Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __userId__ = default(string);
            var __userId__b__ = false;
            var __namespace___ = default(string);
            var __namespace___b__ = false;
            var __linkedPlatforms__ = default(global::AccelByte.Models.AccountLinkedPlatform[]);
            var __linkedPlatforms__b__ = false;

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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 1:
                        __namespace___ = reader.ReadString();
                        __namespace___b__ = true;
                        break;
                    case 2:
                        __linkedPlatforms__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AccountLinkedPlatform[]>().Deserialize(ref reader, formatterResolver);
                        __linkedPlatforms__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AccountLinkPublisherAccount();
            if(__userId__b__) ____result.userId = __userId__;
            if(__namespace___b__) ____result.namespace_ = __namespace___;
            if(__linkedPlatforms__b__) ____result.linkedPlatforms = __linkedPlatforms__;

            return ____result;
        }
    }


    public sealed class AccountLinkConfictMessageVariablesFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.AccountLinkConfictMessageVariables>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AccountLinkConfictMessageVariablesFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformUserID"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("publisherAccounts"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("platformUserID"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("publisherAccounts"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.AccountLinkConfictMessageVariables value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.platformUserID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AccountLinkPublisherAccount[]>().Serialize(ref writer, value.publisherAccounts, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.AccountLinkConfictMessageVariables Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __platformUserID__ = default(string);
            var __platformUserID__b__ = false;
            var __publisherAccounts__ = default(global::AccelByte.Models.AccountLinkPublisherAccount[]);
            var __publisherAccounts__b__ = false;

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
                        __platformUserID__ = reader.ReadString();
                        __platformUserID__b__ = true;
                        break;
                    case 1:
                        __publisherAccounts__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.AccountLinkPublisherAccount[]>().Deserialize(ref reader, formatterResolver);
                        __publisherAccounts__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.AccountLinkConfictMessageVariables();
            if(__platformUserID__b__) ____result.platformUserID = __platformUserID__;
            if(__publisherAccounts__b__) ____result.publisherAccounts = __publisherAccounts__;

            return ____result;
        }
    }


    public sealed class LinkPlatformAccountRequestFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.LinkPlatformAccountRequest>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LinkPlatformAccountRequestFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("platformUserId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("platformId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.LinkPlatformAccountRequest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.platformId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.platformUserId);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.LinkPlatformAccountRequest Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __platformId__ = default(string);
            var __platformId__b__ = false;
            var __platformUserId__ = default(string);
            var __platformUserId__b__ = false;

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
                        __platformId__ = reader.ReadString();
                        __platformId__b__ = true;
                        break;
                    case 1:
                        __platformUserId__ = reader.ReadString();
                        __platformUserId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.LinkPlatformAccountRequest();
            if(__platformId__b__) ____result.platformId = __platformId__;
            if(__platformUserId__b__) ____result.platformUserId = __platformUserId__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
