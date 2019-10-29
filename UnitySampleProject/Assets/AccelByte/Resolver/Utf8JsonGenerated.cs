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
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(101)
            {
                {typeof(global::AccelByte.Models.WalletTransaction[]), 0 },
                {typeof(global::AccelByte.Models.Category[]), 1 },
                {typeof(global::AccelByte.Models.Image[]), 2 },
                {typeof(global::AccelByte.Models.RegionData[]), 3 },
                {typeof(global::AccelByte.Models.Item[]), 4 },
                {typeof(global::AccelByte.Models.OrderTransaction[]), 5 },
                {typeof(global::AccelByte.Models.OrderInfo[]), 6 },
                {typeof(global::AccelByte.Models.Entitlement[]), 7 },
                {typeof(global::System.Collections.Generic.Dictionary<string, string>), 8 },
                {typeof(global::AccelByte.Models.GameProfilePublicInfo[]), 9 },
                {typeof(global::AccelByte.Models.Slot[]), 10 },
                {typeof(global::AccelByte.Models.UserGameProfiles[]), 11 },
                {typeof(global::AccelByte.Models.GameProfile[]), 12 },
                {typeof(global::AccelByte.Models.OrderHistoryInfo[]), 13 },
                {typeof(global::AccelByte.Models.PlatformLink[]), 14 },
                {typeof(global::AccelByte.Models.PublicUserProfile[]), 15 },
                {typeof(global::AccelByte.Models.StatItemInfo[]), 16 },
                {typeof(global::AccelByte.Models.Ban[]), 17 },
                {typeof(global::AccelByte.Models.Permission[]), 18 },
                {typeof(global::AccelByte.Models.PublicUserInfo[]), 19 },
                {typeof(global::AccelByte.Models.UserProfile), 20 },
                {typeof(global::AccelByte.Models.PublicUserProfile), 21 },
                {typeof(global::AccelByte.Models.CreateUserProfileRequest), 22 },
                {typeof(global::AccelByte.Models.UpdateUserProfileRequest), 23 },
                {typeof(global::AccelByte.Models.Slot), 24 },
                {typeof(global::AccelByte.Models.Config), 25 },
                {typeof(global::AccelByte.Models.CurrencySummary), 26 },
                {typeof(global::AccelByte.Models.BalanceInfo), 27 },
                {typeof(global::AccelByte.Models.WalletInfo), 28 },
                {typeof(global::AccelByte.Models.WalletTransaction), 29 },
                {typeof(global::AccelByte.Models.Paging), 30 },
                {typeof(global::AccelByte.Models.PagedWalletTransactions), 31 },
                {typeof(global::AccelByte.Models.Category), 32 },
                {typeof(global::AccelByte.Models.RegionData), 33 },
                {typeof(global::AccelByte.Models.Image), 34 },
                {typeof(global::AccelByte.Models.ItemSnapshot), 35 },
                {typeof(global::AccelByte.Models.ItemCriteria), 36 },
                {typeof(global::AccelByte.Models.Item), 37 },
                {typeof(global::AccelByte.Models.PagedItems), 38 },
                {typeof(global::AccelByte.Models.PaymentUrl), 39 },
                {typeof(global::AccelByte.Models.Price), 40 },
                {typeof(global::AccelByte.Models.OrderHistoryInfo), 41 },
                {typeof(global::AccelByte.Models.OrderTransaction), 42 },
                {typeof(global::AccelByte.Models.OrderInfo), 43 },
                {typeof(global::AccelByte.Models.PagedOrderInfo), 44 },
                {typeof(global::AccelByte.Models.OrderRequest), 45 },
                {typeof(global::AccelByte.Models.Entitlement), 46 },
                {typeof(global::AccelByte.Models.PagedEntitlements), 47 },
                {typeof(global::AccelByte.Models.ServiceError), 48 },
                {typeof(global::AccelByte.Models.OAuthError), 49 },
                {typeof(global::AccelByte.Models.GameProfile), 50 },
                {typeof(global::AccelByte.Models.GameProfileRequest), 51 },
                {typeof(global::AccelByte.Models.GameProfileAttribute), 52 },
                {typeof(global::AccelByte.Models.GameProfilePublicInfo), 53 },
                {typeof(global::AccelByte.Models.UserGameProfiles), 54 },
                {typeof(global::AccelByte.Models.Notification), 55 },
                {typeof(global::AccelByte.Models.ChatMesssage), 56 },
                {typeof(global::AccelByte.Models.PersonalChatRequest), 57 },
                {typeof(global::AccelByte.Models.PartyInfo), 58 },
                {typeof(global::AccelByte.Models.PartyInviteRequest), 59 },
                {typeof(global::AccelByte.Models.PartyInvitation), 60 },
                {typeof(global::AccelByte.Models.PartyChatRequest), 61 },
                {typeof(global::AccelByte.Models.PartyJoinRequest), 62 },
                {typeof(global::AccelByte.Models.PartyKickRequest), 63 },
                {typeof(global::AccelByte.Models.JoinNotification), 64 },
                {typeof(global::AccelByte.Models.KickNotification), 65 },
                {typeof(global::AccelByte.Models.LeaveNotification), 66 },
                {typeof(global::AccelByte.Models.GameMode), 67 },
                {typeof(global::AccelByte.Models.MatchmakingNotif), 68 },
                {typeof(global::AccelByte.Models.DsNotif), 69 },
                {typeof(global::AccelByte.Models.MatchmakingCode), 70 },
                {typeof(global::AccelByte.Models.ReadyConsentRequest), 71 },
                {typeof(global::AccelByte.Models.ReadyForMatchConfirmation), 72 },
                {typeof(global::AccelByte.Models.RematchmakingNotification), 73 },
                {typeof(global::AccelByte.Models.FriendshipStatus), 74 },
                {typeof(global::AccelByte.Models.Friends), 75 },
                {typeof(global::AccelByte.Models.Friend), 76 },
                {typeof(global::AccelByte.Models.FriendsStatus), 77 },
                {typeof(global::AccelByte.Models.FriendsStatusNotif), 78 },
                {typeof(global::AccelByte.Models.OnlineFriends), 79 },
                {typeof(global::AccelByte.Models.PlatformLink), 80 },
                {typeof(global::AccelByte.Models.Collection), 81 },
                {typeof(global::AccelByte.Models.StatInfo), 82 },
                {typeof(global::AccelByte.Models.StatItemInfo), 83 },
                {typeof(global::AccelByte.Models.StatItemIncResult), 84 },
                {typeof(global::AccelByte.Models.StatItemPagingSlicedResult), 85 },
                {typeof(global::AccelByte.Models.BulkUserStatItemInc), 86 },
                {typeof(global::AccelByte.Models.BulkStatItemInc), 87 },
                {typeof(global::AccelByte.Models.BulkStatItemOperationResult), 88 },
                {typeof(global::AccelByte.Models.TelemetryEventTag), 89 },
                {typeof(global::AccelByte.Models.TokenData), 90 },
                {typeof(global::AccelByte.Models.SessionData), 91 },
                {typeof(global::AccelByte.Models.Ban), 92 },
                {typeof(global::AccelByte.Models.Permission), 93 },
                {typeof(global::AccelByte.Models.UserData), 94 },
                {typeof(global::AccelByte.Models.PublicUserInfo), 95 },
                {typeof(global::AccelByte.Models.PagedPublicUsersInfo), 96 },
                {typeof(global::AccelByte.Models.RegisterUserRequest), 97 },
                {typeof(global::AccelByte.Models.RegisterUserResponse), 98 },
                {typeof(global::AccelByte.Models.UpdateUserRequest), 99 },
                {typeof(global::AccelByte.Models.PagedPlatformLinks), 100 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.WalletTransaction>();
                case 1: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Category>();
                case 2: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Image>();
                case 3: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.RegionData>();
                case 4: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Item>();
                case 5: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.OrderTransaction>();
                case 6: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.OrderInfo>();
                case 7: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Entitlement>();
                case 8: return new global::Utf8Json.Formatters.DictionaryFormatter<string, string>();
                case 9: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.GameProfilePublicInfo>();
                case 10: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Slot>();
                case 11: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.UserGameProfiles>();
                case 12: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.GameProfile>();
                case 13: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.OrderHistoryInfo>();
                case 14: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PlatformLink>();
                case 15: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PublicUserProfile>();
                case 16: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.StatItemInfo>();
                case 17: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Ban>();
                case 18: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Permission>();
                case 19: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.PublicUserInfo>();
                case 20: return new Utf8Json.Formatters.AccelByte.Models.UserProfileFormatter();
                case 21: return new Utf8Json.Formatters.AccelByte.Models.PublicUserProfileFormatter();
                case 22: return new Utf8Json.Formatters.AccelByte.Models.CreateUserProfileRequestFormatter();
                case 23: return new Utf8Json.Formatters.AccelByte.Models.UpdateUserProfileRequestFormatter();
                case 24: return new Utf8Json.Formatters.AccelByte.Models.SlotFormatter();
                case 25: return new Utf8Json.Formatters.AccelByte.Models.ConfigFormatter();
                case 26: return new Utf8Json.Formatters.AccelByte.Models.CurrencySummaryFormatter();
                case 27: return new Utf8Json.Formatters.AccelByte.Models.BalanceInfoFormatter();
                case 28: return new Utf8Json.Formatters.AccelByte.Models.WalletInfoFormatter();
                case 29: return new Utf8Json.Formatters.AccelByte.Models.WalletTransactionFormatter();
                case 30: return new Utf8Json.Formatters.AccelByte.Models.PagingFormatter();
                case 31: return new Utf8Json.Formatters.AccelByte.Models.PagedWalletTransactionsFormatter();
                case 32: return new Utf8Json.Formatters.AccelByte.Models.CategoryFormatter();
                case 33: return new Utf8Json.Formatters.AccelByte.Models.RegionDataFormatter();
                case 34: return new Utf8Json.Formatters.AccelByte.Models.ImageFormatter();
                case 35: return new Utf8Json.Formatters.AccelByte.Models.ItemSnapshotFormatter();
                case 36: return new Utf8Json.Formatters.AccelByte.Models.ItemCriteriaFormatter();
                case 37: return new Utf8Json.Formatters.AccelByte.Models.ItemFormatter();
                case 38: return new Utf8Json.Formatters.AccelByte.Models.PagedItemsFormatter();
                case 39: return new Utf8Json.Formatters.AccelByte.Models.PaymentUrlFormatter();
                case 40: return new Utf8Json.Formatters.AccelByte.Models.PriceFormatter();
                case 41: return new Utf8Json.Formatters.AccelByte.Models.OrderHistoryInfoFormatter();
                case 42: return new Utf8Json.Formatters.AccelByte.Models.OrderTransactionFormatter();
                case 43: return new Utf8Json.Formatters.AccelByte.Models.OrderInfoFormatter();
                case 44: return new Utf8Json.Formatters.AccelByte.Models.PagedOrderInfoFormatter();
                case 45: return new Utf8Json.Formatters.AccelByte.Models.OrderRequestFormatter();
                case 46: return new Utf8Json.Formatters.AccelByte.Models.EntitlementFormatter();
                case 47: return new Utf8Json.Formatters.AccelByte.Models.PagedEntitlementsFormatter();
                case 48: return new Utf8Json.Formatters.AccelByte.Models.ServiceErrorFormatter();
                case 49: return new Utf8Json.Formatters.AccelByte.Models.OAuthErrorFormatter();
                case 50: return new Utf8Json.Formatters.AccelByte.Models.GameProfileFormatter();
                case 51: return new Utf8Json.Formatters.AccelByte.Models.GameProfileRequestFormatter();
                case 52: return new Utf8Json.Formatters.AccelByte.Models.GameProfileAttributeFormatter();
                case 53: return new Utf8Json.Formatters.AccelByte.Models.GameProfilePublicInfoFormatter();
                case 54: return new Utf8Json.Formatters.AccelByte.Models.UserGameProfilesFormatter();
                case 55: return new Utf8Json.Formatters.AccelByte.Models.NotificationFormatter();
                case 56: return new Utf8Json.Formatters.AccelByte.Models.ChatMesssageFormatter();
                case 57: return new Utf8Json.Formatters.AccelByte.Models.PersonalChatRequestFormatter();
                case 58: return new Utf8Json.Formatters.AccelByte.Models.PartyInfoFormatter();
                case 59: return new Utf8Json.Formatters.AccelByte.Models.PartyInviteRequestFormatter();
                case 60: return new Utf8Json.Formatters.AccelByte.Models.PartyInvitationFormatter();
                case 61: return new Utf8Json.Formatters.AccelByte.Models.PartyChatRequestFormatter();
                case 62: return new Utf8Json.Formatters.AccelByte.Models.PartyJoinRequestFormatter();
                case 63: return new Utf8Json.Formatters.AccelByte.Models.PartyKickRequestFormatter();
                case 64: return new Utf8Json.Formatters.AccelByte.Models.JoinNotificationFormatter();
                case 65: return new Utf8Json.Formatters.AccelByte.Models.KickNotificationFormatter();
                case 66: return new Utf8Json.Formatters.AccelByte.Models.LeaveNotificationFormatter();
                case 67: return new Utf8Json.Formatters.AccelByte.Models.GameModeFormatter();
                case 68: return new Utf8Json.Formatters.AccelByte.Models.MatchmakingNotifFormatter();
                case 69: return new Utf8Json.Formatters.AccelByte.Models.DsNotifFormatter();
                case 70: return new Utf8Json.Formatters.AccelByte.Models.MatchmakingCodeFormatter();
                case 71: return new Utf8Json.Formatters.AccelByte.Models.ReadyConsentRequestFormatter();
                case 72: return new Utf8Json.Formatters.AccelByte.Models.ReadyForMatchConfirmationFormatter();
                case 73: return new Utf8Json.Formatters.AccelByte.Models.RematchmakingNotificationFormatter();
                case 74: return new Utf8Json.Formatters.AccelByte.Models.FriendshipStatusFormatter();
                case 75: return new Utf8Json.Formatters.AccelByte.Models.FriendsFormatter();
                case 76: return new Utf8Json.Formatters.AccelByte.Models.FriendFormatter();
                case 77: return new Utf8Json.Formatters.AccelByte.Models.FriendsStatusFormatter();
                case 78: return new Utf8Json.Formatters.AccelByte.Models.FriendsStatusNotifFormatter();
                case 79: return new Utf8Json.Formatters.AccelByte.Models.OnlineFriendsFormatter();
                case 80: return new Utf8Json.Formatters.AccelByte.Models.PlatformLinkFormatter();
                case 81: return new Utf8Json.Formatters.AccelByte.Models.CollectionFormatter();
                case 82: return new Utf8Json.Formatters.AccelByte.Models.StatInfoFormatter();
                case 83: return new Utf8Json.Formatters.AccelByte.Models.StatItemInfoFormatter();
                case 84: return new Utf8Json.Formatters.AccelByte.Models.StatItemIncResultFormatter();
                case 85: return new Utf8Json.Formatters.AccelByte.Models.StatItemPagingSlicedResultFormatter();
                case 86: return new Utf8Json.Formatters.AccelByte.Models.BulkUserStatItemIncFormatter();
                case 87: return new Utf8Json.Formatters.AccelByte.Models.BulkStatItemIncFormatter();
                case 88: return new Utf8Json.Formatters.AccelByte.Models.BulkStatItemOperationResultFormatter();
                case 89: return new Utf8Json.Formatters.AccelByte.Models.TelemetryEventTagFormatter();
                case 90: return new Utf8Json.Formatters.AccelByte.Models.TokenDataFormatter();
                case 91: return new Utf8Json.Formatters.AccelByte.Models.SessionDataFormatter();
                case 92: return new Utf8Json.Formatters.AccelByte.Models.BanFormatter();
                case 93: return new Utf8Json.Formatters.AccelByte.Models.PermissionFormatter();
                case 94: return new Utf8Json.Formatters.AccelByte.Models.UserDataFormatter();
                case 95: return new Utf8Json.Formatters.AccelByte.Models.PublicUserInfoFormatter();
                case 96: return new Utf8Json.Formatters.AccelByte.Models.PagedPublicUsersInfoFormatter();
                case 97: return new Utf8Json.Formatters.AccelByte.Models.RegisterUserRequestFormatter();
                case 98: return new Utf8Json.Formatters.AccelByte.Models.RegisterUserResponseFormatter();
                case 99: return new Utf8Json.Formatters.AccelByte.Models.UpdateUserRequestFormatter();
                case 100: return new Utf8Json.Formatters.AccelByte.Models.PagedPlatformLinksFormatter();
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
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timeZone"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarSmallUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatarLargeUrl"),
                
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
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.customAttributes, formatterResolver);
            
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
            var __customAttributes__ = default(object);
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
                        __customAttributes__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
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
            writer.WriteDouble(value.dateAccessed);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteDouble(value.dateCreated);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteDouble(value.dateModified);
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
            var __dateAccessed__ = default(double);
            var __dateAccessed__b__ = false;
            var __dateCreated__ = default(double);
            var __dateCreated__b__ = false;
            var __dateModified__ = default(double);
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
                        __dateAccessed__ = reader.ReadDouble();
                        __dateAccessed__b__ = true;
                        break;
                    case 3:
                        __dateCreated__ = reader.ReadDouble();
                        __dateCreated__b__ = true;
                        break;
                    case 4:
                        __dateModified__ = reader.ReadDouble();
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


    public sealed class ConfigFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Config>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PublisherNamespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("UseSessionManagement"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("BaseUrl"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LoginServerUrl"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("IamServerUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PlatformServerUrl"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("BasicServerUrl"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LobbyServerUrl"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CloudStorageServerUrl"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("TelemetryServerUrl"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameProfileServerUrl"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("StatisticServerUrl"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientId"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ClientSecret"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("RedirectUri"), 15},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("PublisherNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("UseSessionManagement"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("BaseUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LoginServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("IamServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PlatformServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("BasicServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LobbyServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CloudStorageServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("TelemetryServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameProfileServerUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("StatisticServerUrl"),
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
            writer.WriteString(value.PublisherNamespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.UseSessionManagement);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.BaseUrl);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.LoginServerUrl);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.IamServerUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.PlatformServerUrl);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.BasicServerUrl);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.LobbyServerUrl);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.CloudStorageServerUrl);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.TelemetryServerUrl);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.GameProfileServerUrl);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.StatisticServerUrl);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.ClientId);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.ClientSecret);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.RedirectUri);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Config Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __PublisherNamespace__ = default(string);
            var __PublisherNamespace__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __UseSessionManagement__ = default(bool);
            var __UseSessionManagement__b__ = false;
            var __BaseUrl__ = default(string);
            var __BaseUrl__b__ = false;
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
            var __TelemetryServerUrl__ = default(string);
            var __TelemetryServerUrl__b__ = false;
            var __GameProfileServerUrl__ = default(string);
            var __GameProfileServerUrl__b__ = false;
            var __StatisticServerUrl__ = default(string);
            var __StatisticServerUrl__b__ = false;
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
                        __PublisherNamespace__ = reader.ReadString();
                        __PublisherNamespace__b__ = true;
                        break;
                    case 1:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 2:
                        __UseSessionManagement__ = reader.ReadBoolean();
                        __UseSessionManagement__b__ = true;
                        break;
                    case 3:
                        __BaseUrl__ = reader.ReadString();
                        __BaseUrl__b__ = true;
                        break;
                    case 4:
                        __LoginServerUrl__ = reader.ReadString();
                        __LoginServerUrl__b__ = true;
                        break;
                    case 5:
                        __IamServerUrl__ = reader.ReadString();
                        __IamServerUrl__b__ = true;
                        break;
                    case 6:
                        __PlatformServerUrl__ = reader.ReadString();
                        __PlatformServerUrl__b__ = true;
                        break;
                    case 7:
                        __BasicServerUrl__ = reader.ReadString();
                        __BasicServerUrl__b__ = true;
                        break;
                    case 8:
                        __LobbyServerUrl__ = reader.ReadString();
                        __LobbyServerUrl__b__ = true;
                        break;
                    case 9:
                        __CloudStorageServerUrl__ = reader.ReadString();
                        __CloudStorageServerUrl__b__ = true;
                        break;
                    case 10:
                        __TelemetryServerUrl__ = reader.ReadString();
                        __TelemetryServerUrl__b__ = true;
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
                        __ClientId__ = reader.ReadString();
                        __ClientId__b__ = true;
                        break;
                    case 14:
                        __ClientSecret__ = reader.ReadString();
                        __ClientSecret__b__ = true;
                        break;
                    case 15:
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
            if(__PublisherNamespace__b__) ____result.PublisherNamespace = __PublisherNamespace__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__UseSessionManagement__b__) ____result.UseSessionManagement = __UseSessionManagement__;
            if(__BaseUrl__b__) ____result.BaseUrl = __BaseUrl__;
            if(__LoginServerUrl__b__) ____result.LoginServerUrl = __LoginServerUrl__;
            if(__IamServerUrl__b__) ____result.IamServerUrl = __IamServerUrl__;
            if(__PlatformServerUrl__b__) ____result.PlatformServerUrl = __PlatformServerUrl__;
            if(__BasicServerUrl__b__) ____result.BasicServerUrl = __BasicServerUrl__;
            if(__LobbyServerUrl__b__) ____result.LobbyServerUrl = __LobbyServerUrl__;
            if(__CloudStorageServerUrl__b__) ____result.CloudStorageServerUrl = __CloudStorageServerUrl__;
            if(__TelemetryServerUrl__b__) ____result.TelemetryServerUrl = __TelemetryServerUrl__;
            if(__GameProfileServerUrl__b__) ____result.GameProfileServerUrl = __GameProfileServerUrl__;
            if(__StatisticServerUrl__b__) ____result.StatisticServerUrl = __StatisticServerUrl__;
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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("balance"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("id"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("balance"),
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
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteDouble(value.balance);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.status);
            
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
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __balance__ = default(double);
            var __balance__b__ = false;
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
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 6:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 7:
                        __balance__ = reader.ReadDouble();
                        __balance__b__ = true;
                        break;
                    case 8:
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

            var ____result = new global::AccelByte.Models.WalletInfo();
            if(__id__b__) ____result.id = __id__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__userId__b__) ____result.userId = __userId__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__balance__b__) ____result.balance = __balance__;
            if(__status__b__) ____result.status = __status__;

            return ____result;
        }
    }


    public sealed class WalletTransactionFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.WalletTransaction>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public WalletTransactionFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.WalletTransaction value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

        public global::AccelByte.Models.WalletTransaction Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

            var ____result = new global::AccelByte.Models.WalletTransaction();
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


    public sealed class PagedWalletTransactionsFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedWalletTransactions>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedWalletTransactionsFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedWalletTransactions value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.WalletTransaction[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PagedWalletTransactions Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.WalletTransaction[]);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.WalletTransaction[]>().Deserialize(ref reader, formatterResolver);
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

            var ____result = new global::AccelByte.Models.PagedWalletTransactions();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class CategoryFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Category>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CategoryFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("parentCategoryPath"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("displayName"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("childCategories"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("root"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parentCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("displayName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("childCategories"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("root"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Category value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.displayName);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Category[]>().Serialize(ref writer, value.childCategories, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteBoolean(value.root);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Category Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            var __displayName__ = default(string);
            var __displayName__b__ = false;
            var __childCategories__ = default(global::AccelByte.Models.Category[]);
            var __childCategories__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
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
                        __displayName__ = reader.ReadString();
                        __displayName__b__ = true;
                        break;
                    case 4:
                        __childCategories__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Category[]>().Deserialize(ref reader, formatterResolver);
                        __childCategories__b__ = true;
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

            var ____result = new global::AccelByte.Models.Category();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__parentCategoryPath__b__) ____result.parentCategoryPath = __parentCategoryPath__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__displayName__b__) ____result.displayName = __displayName__;
            if(__childCategories__b__) ____result.childCategories = __childCategories__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__root__b__) ____result.root = __root__;

            return ____result;
        }
    }


    public sealed class RegionDataFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.RegionData>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RegionDataFormatter()
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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("totalNum"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("totalNumPerAccount"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountPurchaseAt"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountExpireAt"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountTotalNum"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountTotalNumPerAccount"), 14},
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
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("totalNum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("totalNumPerAccount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountPurchaseAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountExpireAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountTotalNum"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountTotalNumPerAccount"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.RegionData value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteInt32(value.totalNum);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.totalNumPerAccount);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.discountPurchaseAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.discountExpireAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteInt32(value.discountTotalNum);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteInt32(value.discountTotalNumPerAccount);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.RegionData Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            var __totalNum__ = default(int);
            var __totalNum__b__ = false;
            var __totalNumPerAccount__ = default(int);
            var __totalNumPerAccount__b__ = false;
            var __discountPurchaseAt__ = default(global::System.DateTime);
            var __discountPurchaseAt__b__ = false;
            var __discountExpireAt__ = default(global::System.DateTime);
            var __discountExpireAt__b__ = false;
            var __discountTotalNum__ = default(int);
            var __discountTotalNum__b__ = false;
            var __discountTotalNumPerAccount__ = default(int);
            var __discountTotalNumPerAccount__b__ = false;

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
                        __totalNum__ = reader.ReadInt32();
                        __totalNum__b__ = true;
                        break;
                    case 10:
                        __totalNumPerAccount__ = reader.ReadInt32();
                        __totalNumPerAccount__b__ = true;
                        break;
                    case 11:
                        __discountPurchaseAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __discountPurchaseAt__b__ = true;
                        break;
                    case 12:
                        __discountExpireAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __discountExpireAt__b__ = true;
                        break;
                    case 13:
                        __discountTotalNum__ = reader.ReadInt32();
                        __discountTotalNum__b__ = true;
                        break;
                    case 14:
                        __discountTotalNumPerAccount__ = reader.ReadInt32();
                        __discountTotalNumPerAccount__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.RegionData();
            if(__price__b__) ____result.price = __price__;
            if(__discountPercentage__b__) ____result.discountPercentage = __discountPercentage__;
            if(__discountAmount__b__) ____result.discountAmount = __discountAmount__;
            if(__discountedPrice__b__) ____result.discountedPrice = __discountedPrice__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__currencyNamespace__b__) ____result.currencyNamespace = __currencyNamespace__;
            if(__purchaseAt__b__) ____result.purchaseAt = __purchaseAt__;
            if(__expireAt__b__) ____result.expireAt = __expireAt__;
            if(__totalNum__b__) ____result.totalNum = __totalNum__;
            if(__totalNumPerAccount__b__) ____result.totalNumPerAccount = __totalNumPerAccount__;
            if(__discountPurchaseAt__b__) ____result.discountPurchaseAt = __discountPurchaseAt__;
            if(__discountExpireAt__b__) ____result.discountExpireAt = __discountExpireAt__;
            if(__discountTotalNum__b__) ____result.discountTotalNum = __discountTotalNum__;
            if(__discountTotalNumPerAccount__b__) ____result.discountTotalNumPerAccount = __discountTotalNumPerAccount__;

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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("height"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("width"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("imageUrl"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("smallImageUrl"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("height"),
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
            writer.WriteInt32(value.height);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.width);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.imageUrl);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.smallImageUrl);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Image Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

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
                        __height__ = reader.ReadInt32();
                        __height__b__ = true;
                        break;
                    case 1:
                        __width__ = reader.ReadInt32();
                        __width__b__ = true;
                        break;
                    case 2:
                        __imageUrl__ = reader.ReadString();
                        __imageUrl__b__ = true;
                        break;
                    case 3:
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
            if(__height__b__) ____result.height = __height__;
            if(__width__b__) ____result.width = __width__;
            if(__imageUrl__b__) ____result.imageUrl = __imageUrl__;
            if(__smallImageUrl__b__) ____result.smallImageUrl = __smallImageUrl__;

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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailImage"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionDataItem"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("language"), 17},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailImage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionDataItem"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
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
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[7]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Serialize(ref writer, value.itemType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image>().Serialize(ref writer, value.thumbnailImage, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionData>().Serialize(ref writer, value.regionDataItem, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.region);
            writer.WriteRaw(this.____stringByteKeys[17]);
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
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __itemType__ = default(global::AccelByte.Models.ItemType);
            var __itemType__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __title__ = default(string);
            var __title__b__ = false;
            var __thumbnailImage__ = default(global::AccelByte.Models.Image);
            var __thumbnailImage__b__ = false;
            var __regionDataItem__ = default(global::AccelByte.Models.RegionData);
            var __regionDataItem__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
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
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 4:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 5:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 6:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 7:
                        __itemType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Deserialize(ref reader, formatterResolver);
                        __itemType__b__ = true;
                        break;
                    case 8:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 9:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 10:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 11:
                        __thumbnailImage__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image>().Deserialize(ref reader, formatterResolver);
                        __thumbnailImage__b__ = true;
                        break;
                    case 12:
                        __regionDataItem__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionData>().Deserialize(ref reader, formatterResolver);
                        __regionDataItem__b__ = true;
                        break;
                    case 13:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 14:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 15:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 16:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
                        break;
                    case 17:
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
            if(__sku__b__) ____result.sku = __sku__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__name__b__) ____result.name = __name__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__title__b__) ____result.title = __title__;
            if(__thumbnailImage__b__) ____result.thumbnailImage = __thumbnailImage__;
            if(__regionDataItem__b__) ____result.regionDataItem = __regionDataItem__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ItemType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CategoryPath"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ItemStatus"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Page"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Size"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("SortBy"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ItemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ItemStatus"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Page"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Size"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("SortBy"),
                
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
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Serialize(ref writer, value.ItemType, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.CategoryPath);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Serialize(ref writer, value.ItemStatus, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value.Page, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<int?>().Serialize(ref writer, value.Size, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.SortBy);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ItemCriteria Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __ItemType__ = default(global::AccelByte.Models.ItemType);
            var __ItemType__b__ = false;
            var __CategoryPath__ = default(string);
            var __CategoryPath__b__ = false;
            var __ItemStatus__ = default(global::AccelByte.Models.ItemStatus);
            var __ItemStatus__b__ = false;
            var __Page__ = default(int?);
            var __Page__b__ = false;
            var __Size__ = default(int?);
            var __Size__b__ = false;
            var __SortBy__ = default(string);
            var __SortBy__b__ = false;

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
                        __ItemType__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemType>().Deserialize(ref reader, formatterResolver);
                        __ItemType__b__ = true;
                        break;
                    case 1:
                        __CategoryPath__ = reader.ReadString();
                        __CategoryPath__b__ = true;
                        break;
                    case 2:
                        __ItemStatus__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.ItemStatus>().Deserialize(ref reader, formatterResolver);
                        __ItemStatus__b__ = true;
                        break;
                    case 3:
                        __Page__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        __Page__b__ = true;
                        break;
                    case 4:
                        __Size__ = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        __Size__b__ = true;
                        break;
                    case 5:
                        __SortBy__ = reader.ReadString();
                        __SortBy__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ItemCriteria();
            if(__ItemType__b__) ____result.ItemType = __ItemType__;
            if(__CategoryPath__b__) ____result.CategoryPath = __CategoryPath__;
            if(__ItemStatus__b__) ____result.ItemStatus = __ItemStatus__;
            if(__Page__b__) ____result.Page = __Page__;
            if(__Size__b__) ____result.Size = __Size__;
            if(__SortBy__b__) ____result.SortBy = __SortBy__;

            return ____result;
        }
    }


    public sealed class ItemFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Item>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ItemFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("longDescription"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("images"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailImage"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementName"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 21},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("longDescription"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("images"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailImage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Item value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Serialize(ref writer, value.images, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image>().Serialize(ref writer, value.thumbnailImage, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.appType);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.entitlementName);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.entitlementType);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[14]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteString(value.itemType);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionData[]>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Item Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            var __images__ = default(global::AccelByte.Models.Image[]);
            var __images__b__ = false;
            var __thumbnailImage__ = default(global::AccelByte.Models.Image);
            var __thumbnailImage__b__ = false;
            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(string);
            var __appType__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __entitlementName__ = default(string);
            var __entitlementName__b__ = false;
            var __entitlementType__ = default(string);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __itemType__ = default(string);
            var __itemType__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __regionData__ = default(global::AccelByte.Models.RegionData[]);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
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
                        __images__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Deserialize(ref reader, formatterResolver);
                        __images__b__ = true;
                        break;
                    case 4:
                        __thumbnailImage__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image>().Deserialize(ref reader, formatterResolver);
                        __thumbnailImage__b__ = true;
                        break;
                    case 5:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
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
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 9:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 10:
                        __entitlementName__ = reader.ReadString();
                        __entitlementName__b__ = true;
                        break;
                    case 11:
                        __entitlementType__ = reader.ReadString();
                        __entitlementType__b__ = true;
                        break;
                    case 12:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 13:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 14:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 15:
                        __itemType__ = reader.ReadString();
                        __itemType__b__ = true;
                        break;
                    case 16:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 17:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 18:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 19:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionData[]>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 20:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 21:
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

            var ____result = new global::AccelByte.Models.Item();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__longDescription__b__) ____result.longDescription = __longDescription__;
            if(__images__b__) ____result.images = __images__;
            if(__thumbnailImage__b__) ____result.thumbnailImage = __thumbnailImage__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__entitlementName__b__) ____result.entitlementName = __entitlementName__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__status__b__) ____result.status = __status__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;

            return ____result;
        }
    }


    public sealed class PagedItemsFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedItems>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedItemsFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedItems value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Item[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PagedItems Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.Item[]);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Item[]>().Deserialize(ref reader, formatterResolver);
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

            var ____result = new global::AccelByte.Models.PagedItems();
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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("orderNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("operator"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("action"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason"),
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
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 5:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 6:
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
            if(__userId__b__) ____result.userId = __userId__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

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


    public sealed class OrderInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.OrderInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OrderInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("orderNo"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("userId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sandbox"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("quantity"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("price"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("discountedPrice"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("vat"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("salesTax"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentProviderFee"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentMethodFee"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currency"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentUrl"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("paymentStationUrl"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("transactions"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementIds"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statusReason"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdTime"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("chargedTime"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("fulfilledTime"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("refundedTime"), 22},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("orderNo"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sandbox"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountedPrice"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("vat"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("salesTax"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentProviderFee"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentMethodFee"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currency"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("paymentStationUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("transactions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statusReason"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("chargedTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("fulfilledTime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("refundedTime"),
                
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
            writer.WriteString(value.userId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteBoolean(value.sandbox);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.quantity);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.price);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.discountedPrice);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.vat);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.salesTax);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteInt32(value.paymentProviderFee);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteInt32(value.paymentMethodFee);
            writer.WriteRaw(this.____stringByteKeys[11]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CurrencySummary>().Serialize(ref writer, value.currency, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PaymentUrl>().Serialize(ref writer, value.paymentUrl, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.paymentStationUrl);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderTransaction[]>().Serialize(ref writer, value.transactions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.entitlementIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteString(value.statusReason);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.chargedTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.fulfilledTime, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.refundedTime, formatterResolver);
            
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
            var __paymentUrl__ = default(global::AccelByte.Models.PaymentUrl);
            var __paymentUrl__b__ = false;
            var __paymentStationUrl__ = default(string);
            var __paymentStationUrl__b__ = false;
            var __transactions__ = default(global::AccelByte.Models.OrderTransaction[]);
            var __transactions__b__ = false;
            var __entitlementIds__ = default(string[]);
            var __entitlementIds__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __statusReason__ = default(string);
            var __statusReason__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __createdTime__ = default(global::System.DateTime);
            var __createdTime__b__ = false;
            var __chargedTime__ = default(global::System.DateTime);
            var __chargedTime__b__ = false;
            var __fulfilledTime__ = default(global::System.DateTime);
            var __fulfilledTime__b__ = false;
            var __refundedTime__ = default(global::System.DateTime);
            var __refundedTime__b__ = false;

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
                        __userId__ = reader.ReadString();
                        __userId__b__ = true;
                        break;
                    case 2:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 3:
                        __sandbox__ = reader.ReadBoolean();
                        __sandbox__b__ = true;
                        break;
                    case 4:
                        __quantity__ = reader.ReadInt32();
                        __quantity__b__ = true;
                        break;
                    case 5:
                        __price__ = reader.ReadInt32();
                        __price__b__ = true;
                        break;
                    case 6:
                        __discountedPrice__ = reader.ReadInt32();
                        __discountedPrice__b__ = true;
                        break;
                    case 7:
                        __vat__ = reader.ReadInt32();
                        __vat__b__ = true;
                        break;
                    case 8:
                        __salesTax__ = reader.ReadInt32();
                        __salesTax__b__ = true;
                        break;
                    case 9:
                        __paymentProviderFee__ = reader.ReadInt32();
                        __paymentProviderFee__b__ = true;
                        break;
                    case 10:
                        __paymentMethodFee__ = reader.ReadInt32();
                        __paymentMethodFee__b__ = true;
                        break;
                    case 11:
                        __currency__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.CurrencySummary>().Deserialize(ref reader, formatterResolver);
                        __currency__b__ = true;
                        break;
                    case 12:
                        __paymentUrl__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.PaymentUrl>().Deserialize(ref reader, formatterResolver);
                        __paymentUrl__b__ = true;
                        break;
                    case 13:
                        __paymentStationUrl__ = reader.ReadString();
                        __paymentStationUrl__b__ = true;
                        break;
                    case 14:
                        __transactions__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.OrderTransaction[]>().Deserialize(ref reader, formatterResolver);
                        __transactions__b__ = true;
                        break;
                    case 15:
                        __entitlementIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __entitlementIds__b__ = true;
                        break;
                    case 16:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 17:
                        __statusReason__ = reader.ReadString();
                        __statusReason__b__ = true;
                        break;
                    case 18:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 19:
                        __createdTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdTime__b__ = true;
                        break;
                    case 20:
                        __chargedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __chargedTime__b__ = true;
                        break;
                    case 21:
                        __fulfilledTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __fulfilledTime__b__ = true;
                        break;
                    case 22:
                        __refundedTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __refundedTime__b__ = true;
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
            if(__userId__b__) ____result.userId = __userId__;
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__sandbox__b__) ____result.sandbox = __sandbox__;
            if(__quantity__b__) ____result.quantity = __quantity__;
            if(__price__b__) ____result.price = __price__;
            if(__discountedPrice__b__) ____result.discountedPrice = __discountedPrice__;
            if(__vat__b__) ____result.vat = __vat__;
            if(__salesTax__b__) ____result.salesTax = __salesTax__;
            if(__paymentProviderFee__b__) ____result.paymentProviderFee = __paymentProviderFee__;
            if(__paymentMethodFee__b__) ____result.paymentMethodFee = __paymentMethodFee__;
            if(__currency__b__) ____result.currency = __currency__;
            if(__paymentUrl__b__) ____result.paymentUrl = __paymentUrl__;
            if(__paymentStationUrl__b__) ____result.paymentStationUrl = __paymentStationUrl__;
            if(__transactions__b__) ____result.transactions = __transactions__;
            if(__entitlementIds__b__) ____result.entitlementIds = __entitlementIds__;
            if(__status__b__) ____result.status = __status__;
            if(__statusReason__b__) ____result.statusReason = __statusReason__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__createdTime__b__) ____result.createdTime = __createdTime__;
            if(__chargedTime__b__) ____result.chargedTime = __chargedTime__;
            if(__fulfilledTime__b__) ____result.fulfilledTime = __fulfilledTime__;
            if(__refundedTime__b__) ____result.refundedTime = __refundedTime__;

            return ____result;
        }
    }


    public sealed class PagedOrderInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedOrderInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedOrderInfoFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedOrderInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

        public global::AccelByte.Models.PagedOrderInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

            var ____result = new global::AccelByte.Models.PagedOrderInfo();
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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("returnUrl"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("region"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("quantity"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("price"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("discountedPrice"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("returnUrl"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("region"),
                
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
            writer.WriteString(value.returnUrl);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.region);
            
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
            var __returnUrl__ = default(string);
            var __returnUrl__b__ = false;
            var __region__ = default(string);
            var __region__b__ = false;

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
                        __returnUrl__ = reader.ReadString();
                        __returnUrl__b__ = true;
                        break;
                    case 6:
                        __region__ = reader.ReadString();
                        __region__b__ = true;
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
            if(__returnUrl__b__) ____result.returnUrl = __returnUrl__;
            if(__region__b__) ____result.region = __region__;

            return ____result;
        }
    }


    public sealed class EntitlementFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.Entitlement>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public EntitlementFormatter()
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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("distributedQuantity"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemSnapshot"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("startDate"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("endDate"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grantedAt"), 21},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 22},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 23},
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.Entitlement value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.clazz);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.type);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.appType);
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
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.grantedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[22]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[23]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.Entitlement Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __id__ = default(string);
            var __id__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __clazz__ = default(string);
            var __clazz__b__ = false;
            var __type__ = default(string);
            var __type__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(string);
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
                        __clazz__ = reader.ReadString();
                        __clazz__b__ = true;
                        break;
                    case 3:
                        __type__ = reader.ReadString();
                        __type__b__ = true;
                        break;
                    case 4:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 5:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 6:
                        __appType__ = reader.ReadString();
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
                        __grantedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __grantedAt__b__ = true;
                        break;
                    case 22:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 23:
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

            var ____result = new global::AccelByte.Models.Entitlement();
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


    public sealed class PagedEntitlementsFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.PagedEntitlements>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PagedEntitlementsFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.PagedEntitlements value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Entitlement[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.PagedEntitlements Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.Entitlement[]);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Entitlement[]>().Deserialize(ref reader, formatterResolver);
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

            var ____result = new global::AccelByte.Models.PagedEntitlements();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("numericErrorCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("errorCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("errorMessage"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("numericErrorCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorMessage"),
                
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
            writer.WriteInt32(value.numericErrorCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt64(value.errorCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.errorMessage);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.ServiceError Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __numericErrorCode__ = default(int);
            var __numericErrorCode__b__ = false;
            var __errorCode__ = default(long);
            var __errorCode__b__ = false;
            var __errorMessage__ = default(string);
            var __errorMessage__b__ = false;

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
                        __numericErrorCode__ = reader.ReadInt32();
                        __numericErrorCode__b__ = true;
                        break;
                    case 1:
                        __errorCode__ = reader.ReadInt64();
                        __errorCode__b__ = true;
                        break;
                    case 2:
                        __errorMessage__ = reader.ReadString();
                        __errorMessage__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.ServiceError();
            if(__numericErrorCode__b__) ____result.numericErrorCode = __numericErrorCode__;
            if(__errorCode__b__) ____result.errorCode = __errorCode__;
            if(__errorMessage__b__) ____result.errorMessage = __errorMessage__;

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


    public sealed class GameModeFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.GameMode>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GameModeFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("gameMode"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("gameMode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.GameMode value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.gameMode);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.GameMode Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __gameMode__ = default(string);
            var __gameMode__b__ = false;

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
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.GameMode();
            if(__gameMode__b__) ____result.gameMode = __gameMode__;

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


    public sealed class StatInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatInfoFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

        public global::AccelByte.Models.StatInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

            var ____result = new global::AccelByte.Models.StatInfo();
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


    public sealed class StatItemInfoFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatItemInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatItemInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileId"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statName"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("profileId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatItemInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
            writer.WriteString(value.profileId);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.statName);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.updatedAt);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteSingle(value.value);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatItemInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __createdAt__ = default(string);
            var __createdAt__b__ = false;
            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __profileId__ = default(string);
            var __profileId__b__ = false;
            var __statCode__ = default(string);
            var __statCode__b__ = false;
            var __statName__ = default(string);
            var __statName__b__ = false;
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
                        __profileId__ = reader.ReadString();
                        __profileId__b__ = true;
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
                        __updatedAt__ = reader.ReadString();
                        __updatedAt__b__ = true;
                        break;
                    case 6:
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

            var ____result = new global::AccelByte.Models.StatItemInfo();
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__profileId__b__) ____result.profileId = __profileId__;
            if(__statCode__b__) ____result.statCode = __statCode__;
            if(__statName__b__) ____result.statName = __statName__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__value__b__) ____result.value = __value__;

            return ____result;
        }
    }


    public sealed class StatItemIncResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatItemIncResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatItemIncResultFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currentValue"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currentValue"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatItemIncResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.currentValue);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatItemIncResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currentValue__ = default(float);
            var __currentValue__b__ = false;

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
                        __currentValue__ = reader.ReadSingle();
                        __currentValue__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.StatItemIncResult();
            if(__currentValue__b__) ____result.currentValue = __currentValue__;

            return ____result;
        }
    }


    public sealed class StatItemPagingSlicedResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.StatItemPagingSlicedResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StatItemPagingSlicedResultFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.StatItemPagingSlicedResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatItemInfo[]>().Serialize(ref writer, value.data, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Paging>().Serialize(ref writer, value.paging, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.StatItemPagingSlicedResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __data__ = default(global::AccelByte.Models.StatItemInfo[]);
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
                        __data__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatItemInfo[]>().Deserialize(ref reader, formatterResolver);
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

            var ____result = new global::AccelByte.Models.StatItemPagingSlicedResult();
            if(__data__b__) ____result.data = __data__;
            if(__paging__b__) ____result.paging = __paging__;

            return ____result;
        }
    }


    public sealed class BulkUserStatItemIncFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BulkUserStatItemInc>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BulkUserStatItemIncFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("inc"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("profileId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("inc"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("profileId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BulkUserStatItemInc value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteSingle(value.inc);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.profileId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.statCode);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.BulkUserStatItemInc Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __inc__ = default(float);
            var __inc__b__ = false;
            var __profileId__ = default(string);
            var __profileId__b__ = false;
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
                        __profileId__ = reader.ReadString();
                        __profileId__b__ = true;
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

            var ____result = new global::AccelByte.Models.BulkUserStatItemInc();
            if(__inc__b__) ____result.inc = __inc__;
            if(__profileId__b__) ____result.profileId = __profileId__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class BulkStatItemIncFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BulkStatItemInc>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BulkStatItemIncFormatter()
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

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BulkStatItemInc value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

        public global::AccelByte.Models.BulkStatItemInc Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

            var ____result = new global::AccelByte.Models.BulkStatItemInc();
            if(__inc__b__) ____result.inc = __inc__;
            if(__statCode__b__) ____result.statCode = __statCode__;

            return ____result;
        }
    }


    public sealed class BulkStatItemOperationResultFormatter : global::Utf8Json.IJsonFormatter<global::AccelByte.Models.BulkStatItemOperationResult>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BulkStatItemOperationResultFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("detail"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("statCode"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("success"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("detail"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("statCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("success"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::AccelByte.Models.BulkStatItemOperationResult value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatItemIncResult>().Serialize(ref writer, value.detail, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.statCode);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.success);
            
            writer.WriteEndObject();
        }

        public global::AccelByte.Models.BulkStatItemOperationResult Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __detail__ = default(global::AccelByte.Models.StatItemIncResult);
            var __detail__b__ = false;
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
                        __detail__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.StatItemIncResult>().Deserialize(ref reader, formatterResolver);
                        __detail__b__ = true;
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

            var ____result = new global::AccelByte.Models.BulkStatItemOperationResult();
            if(__detail__b__) ____result.detail = __detail__;
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
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("session_id"),
                
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
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::AccelByte.Models.SessionData();
            if(__session_id__b__) ____result.session_id = __session_id__;

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

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
