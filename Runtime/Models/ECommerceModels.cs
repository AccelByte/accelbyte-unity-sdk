// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    #region Enum

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemType
    {
        NONE,
        COINS,
        INGAMEITEM,
        BUNDLE,
        APP,
        CODE,
        SUBSCRIPTION,
        SEASON,
        OPTIONBOX,
        MEDIA,
        LOOTBOX
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemStatus
    {
        NONE,
        ACTIVE,
        INACTIVE
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CurrencyType
    {
        NONE,
        REAL,
        VIRTUAL
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntitlementClazz
    {
        NONE,
        APP,
        ENTITLEMENT,
        CODE,
        MEDIA,
        SUBSCRIPTION,
        OPTIONBOX,
        LOOTBOX
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntitlementStatus
    {
        NONE,
        ACTIVE,
        INACTIVE,
        CONSUMED,
        REVOKED
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntitlementAppType
    {
        NONE,
        GAME,
        SOFTWARE,
        DLC,
        DEMO
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntitlementType
    {
        NONE,
        DURABLE,
        CONSUMABLE
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        NONE,
        INIT,
        CHARGED,
        CHARGEBACK,
        CHARGEBACK_REVERSED,
        FULFILLED,
        FULFILL_FAILED,
        REFUNDING,
        REFUNDED,
        REFUND_FAILED,
        CLOSED,
        DELETED
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemSource
    {
        NONE,
        PURCHASE,
        IAP,
        PROMOTION,
        ACHIEVEMENT,
        REFERRAL_BONUS,
        REDEEM_CODE,
        OTHER
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntitlementSource
    {
        NONE,
        PURCHASE,
        IAP,
        PROMOTION,
        ACHIEVEMENT,
        REFERRAL_BONUS,
        REDEEM_CODE,
        REWARD,
        GIFT,
        OTHER
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CreditUserWalletSource
    {
        PURCHASE,
        IAP,
        PROMOTION,
        ACHIEVEMENT,
        REFERRAL_BONUS,
        REDEEM_CODE,
        REFUND,
        OTHER
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SeasonType
    {
        PASS = 0,
        TIER
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RewardSortBy
    {
        NAMESPACE,
        NAMESPACE_ASC,
        NAMESPACE_DESC,
        REWARDCODE,
        REWARDCODE_ASC,
        REWARDCODE_DESC
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WalletTable
    {
        Playstation = 0,
        Xbox,
        Steam,
        Epic, 
        IOS,
        GooglePlay,
        Twitch,
        System,
        Nintendo,
        Other
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Cycle
    {
        WEEKLY,
        MONTHLY,
        QUARTERLY,
        YEARLY
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PredicateType
    {
        EntitlementPredicate,
        SeasonPassPredicate,
        SeasonTierPredicate
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RewardType
    {
        REWARD,
        REWARD_GROUP,
        PROBABILITY_GROUP
    }

    #endregion

    #region Wallet

    [DataContract]
    public class CurrencySummary
    {
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public string currencyType { get; set; } // = ['REAL', 'VIRTUAL'],
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public int decimals { get; set; }
    }

    [DataContract]
    public class BalanceInfo
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string walletId { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public int balance { get; set; }
        [DataMember] public string balanceSource { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public string status { get; set; }
    }

    [DataContract]
    public class TimeLimitedBalance
    {
        [DataMember] public int balance { get; set; }
        [DataMember] public string balanceSource { get; set; }
        [DataMember] public DateTime expiredAt { get; set; }
    }

    [DataContract]
    public class WalletInfo
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public int balance { get; set; }
        [DataMember] public string balanceOrigin { get; set; }
        [DataMember] public TimeLimitedBalance[] timeLimitedBalances { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public int totalPermanentBalance { get; set; }
        [DataMember] public int totalTimeLimitedBalance { get; set; }
        [DataMember] public ItemStatus status { get; set; }
    }

    [DataContract]
    public class WalletInfoResponse
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public int balance { get; set; }
        [DataMember] public WalletInfo[] walletInfos { get; set; }
        [DataMember] public ItemStatus walletStatus { get; set; }
        [DataMember] public ItemStatus status { get; set; }
        [DataMember] public string id { get; set; }
    }

    [DataContract]
    public class WalletTransactionInfo
    {
        [DataMember] public string walletId { get; set; }
        [DataMember] public int amount { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember(Name = "operator")] public string Operator { get; set; }
        [DataMember] public string walletAction { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string balanceSource { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class WalletTransactionPagingSlicedResult
    {
        [DataMember] public WalletTransactionInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class CreditUserWalletRequest
    {
        [DataMember] public int amount { get; set; }
        [DataMember] public CreditUserWalletSource source { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember] public WalletTable origin { get; set; }
    }

    [DataContract]
    public class CreditUserWalletResponse
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public int balance { get; set; }
        [DataMember] public string balanceOrigin { get; set; }
        [DataMember] public TimeLimitedBalance[] timeLimitedBalances { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public int totalPermanentBalance { get; set; }
        [DataMember] public int totalTimeLimitedBalance { get; set; }
        [DataMember] public ItemStatus status { get; set; }
    }

    #endregion

    #region Categories

    [DataContract]
    public class CategoryInfo
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string parentCategoryPath { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public bool root { get; set; }
    }

    #endregion

    #region Items

    [DataContract]
    public class RegionDataItem
    {
        [DataMember] public int price { get; set; }
        [DataMember] public int discountPercentage { get; set; }
        [DataMember] public int discountAmount { get; set; }
        [DataMember] public int discountedPrice { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencyType { get; set; }
        [DataMember] public string currencyNamespace { get; set; }
        [DataMember] public int trialPrice { get; set; }
        [DataMember] public DateTime purchaseAt { get; set; }
        [DataMember] public DateTime expireAt { get; set; }
        [DataMember] public DateTime discountPurchaseAt { get; set; }
        [DataMember] public DateTime discountExpireAt { get; set; }
    }

    [DataContract]
    public class ItemSnapshot
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; }
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public bool listable { get; set; }
        [DataMember] public EntitlementType entitlementType { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public bool purchasable { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public string thumbnailUrl { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public string targetItemId { get; set; }
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public ItemRecurring recurring { get; set; }
        [DataMember] public RegionDataItem regionDataItem { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public string itemQty { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public int maxCountPerUser { get; set; }
        [DataMember] public int maxCount { get; set; }
        [DataMember] public string boothName { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public ItemOptionBoxConfig optionBoxConfig { get; set; }
    }

    [DataContract]
    public class ItemCriteria
    {
        [DataMember] public string storeId { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public bool includeSubCategoryItem { get; set; } = false;// Default == false. Can change to "true". Anything != "true" will remain default
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public int? offset { get; set; }
        [DataMember] public int? limit { get; set; }
        [DataMember] public string sortBy { get; set; }
    }

    [DataContract]
    public class ItemCriteriaV2
    {
        [DataMember] public string storeId { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public bool includeSubCategoryItem { get; set; } = false;// Default == false. Can change to "true". Anything != "true" will remain default
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; }
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public bool activeOnly { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public DateTime availableDate { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public int? offset { get; set; }
        [DataMember] public int? limit { get; set; }
        [DataMember] public string sortBy { get; set; }
    }

    [DataContract]
    public class Image
    {
        [DataMember(Name = "as")] public string As { get; set; }
        [DataMember] public string caption { get; set; }
        [DataMember] public int height { get; set; }
        [DataMember] public int width { get; set; }
        [DataMember] public string imageUrl { get; set; }
        [DataMember] public string smallImageUrl { get; set; }
    }

    [DataContract]
    public class ItemRecurring
    {
        [DataMember] public Cycle cycle { get; set; }
        [DataMember] public int fixedFreeDays { get; set; }
        [DataMember] public int fixedTrialCycles { get; set; }
        [DataMember] public int graceDays { get; set; }
    }

    [DataContract]
    public class ItemPredicate
    {
        [DataMember] public string name { get; set; }
        [DataMember] public PredicateType predicateType { get; set; }
        [DataMember] public string comparison { get; set; }
        [DataMember] public int anyOf { get; set; }
        [DataMember] public string[] values { get; set; }
        [DataMember] public string value { get; set; }
    }

    [DataContract]
    public class ItemConditionGroup
    {
        [DataMember] public ItemPredicate[] predicates { get; set; }
        [DataMember(Name = "operator")] public string Operator { get; set; }
    }

    [DataContract]
    public class ItemPurchaseCondition
    {
        [DataMember] public ItemConditionGroup[] conditionGroups { get; set; }
    }

    [DataContract]
    public class ItemBoxItem
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string itemSku { get; set; }
        [DataMember] public int count { get; set; }
    }

    [DataContract]
    public class ItemOptionBoxConfig
    {
        [DataMember] public ItemBoxItem[] boxItems { get; set; }
    }

    [DataContract]
    public class LootBoxItems
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string itemSku { get; set; }
        [DataMember] public string itemType { get; set; }
        [DataMember] public int count { get; set; }
    }

    [DataContract]
    public class ItemRewards
    {
        [DataMember] public string name { get; set; }
        [DataMember] public RewardType type { get; set; }//REWARD, REWARD_GROUP, PROBABILITY_GROUP
        [DataMember] public LootBoxItems[] lootBoxItems { get; set; }
        [DataMember] public int weight { get; set; }
        [DataMember] public double odds { get; set; }
    }

    [DataContract]
    public class LootBoxConfig
    {
        [DataMember] public int rewardCount { get; set; }
        [DataMember] public ItemRewards[] rewards { get; set; }
    }

    [DataContract]
    public class PopulatedItemInfo
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string longDescription { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; } //"GAME"
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public EntitlementType entitlementType { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public ItemStatus status { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public string targetItemId { get; set; }
        [DataMember] public Image[] images { get; set; }
        [DataMember] public string thumbnailUrl { get; set; }
        [DataMember] public RegionDataItem[] regionData { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public int maxCountPerUser { get; set; }
        [DataMember] public int maxCount { get; set; }
        [DataMember] public string clazz { get; set; }
        [DataMember] public string boothName { get; set; }
        [DataMember] public int displayOrder { get; set; }
        [DataMember] public object ext { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public ItemInfo[] items { get; set; }
        [DataMember] public string localExt { get; set; }
        [DataMember] public Dictionary<string, int> itemQty { get; set; }
    }

    [DataContract]
    public class ItemInfo
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string longDescription { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; } //"GAME" 
        [DataMember] public SeasonType SeasonType { get; set; }
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public EntitlementType entitlementType { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public ItemStatus status { get; set; }
        [DataMember] public bool listable { get; set; }
        [DataMember] public bool purchasable { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public string targetItemId { get; set; }
        [DataMember] public Image[] images { get; set; }
        [DataMember] public string thumbnailUrl { get; set; }
        [DataMember] public RegionDataItem[] regionData { get; set; }
        [DataMember] public ItemRecurring recurring { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public string[] boundItemIds { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public int maxCountPerUser { get; set; }
        [DataMember] public int maxCount { get; set; }
        [DataMember] public string clazz { get; set; }
        [DataMember] public string boothName { get; set; }
        [DataMember] public int displayOrder { get; set; }
        [DataMember] public object ext { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public ItemPurchaseCondition purchaseCondition { get; set; }
        [DataMember] public ItemOptionBoxConfig optionBoxConfig { get; set; }
        [DataMember] public string localExt { get; set; }
        [DataMember] public Dictionary<string, int> itemQty { get; set; }
        [DataMember] public LootBoxConfig lootBoxConfig { get; set; }
    }

    [DataContract]
    public class ItemPagingSlicedResult
    {
        [DataMember] public ItemInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class ItemInfoV2
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; } //"GAME" 
        [DataMember] public SeasonType SeasonType { get; set; }
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public EntitlementType entitlementType { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public Image[] images { get; set; }
        [DataMember] public string thumbnailUrl { get; set; }
        [DataMember] public Dictionary<string, object> localizations { get; set; }
        [DataMember] public ItemStatus status { get; set; }
        [DataMember] public bool listable { get; set; }
        [DataMember] public bool purchasable { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public string targetItemId { get; set; }
        [DataMember] public Dictionary<string, object> regionData { get; set; }
        [DataMember] public ItemRecurring recurring { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public Dictionary<string, int> itemQty { get; set; }
        [DataMember] public string[] boundItemIds { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public int maxCountPerUser { get; set; }
        [DataMember] public int maxCount { get; set; }
        [DataMember] public string clazz { get; set; }
        [DataMember] public Dictionary<string, object> ext { get; set; }
        [DataMember] public string boothName { get; set; }
        [DataMember] public int displayOrder { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public ItemPurchaseCondition purchaseCondition { get; set; }
        [DataMember] public ItemOptionBoxConfig optionBoxConfig { get; set; }
        [DataMember] public LootBoxConfig lootBoxConfig { get; set; }
    }

    [DataContract]
    public class ItemPagingSlicedResultV2
    {
        [DataMember] public ItemInfoV2[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class CurrencyList
    {
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public Dictionary<string, string> localizationDescriptions { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public CurrencyType currencyType { get; set; }
        [DataMember] public int decimals { get; set; }
        [DataMember] public int maxAmountPerTransaction { get; set; }
        [DataMember] public int maxTransactionAmountPerDay { get; set; }
        [DataMember] public int maxBalanceAmount { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class PlatformStore
    {
        [DataMember] public string storeId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public bool published { get; set; }
        [DataMember] public string[] supportedLanguages { get; set; }
        [DataMember] public string[] supportedRegions { get; set; }
        [DataMember] public string defaultRegion { get; set; }
        [DataMember] public string defaultLanguage { get; set; }
        [DataMember] DateTime publishedTime { get; set; }
        [DataMember] DateTime createdAt { get; set; }
        [DataMember] DateTime updatedAt { get; set; }
    }

    #endregion

    #region Orders

    [DataContract]
    public class PaymentUrl
    {
        [DataMember] public string paymentProvider { get; set; } //['XSOLLA', 'WALLET'],
        [DataMember] public string paymentUrl { get; set; }
        [DataMember] public string paymentToken { get; set; }
        [DataMember] public string returnUrl { get; set; }
        [DataMember] public string paymentType { get; set; } //['QR_CODE', 'LINK']
    }

    [DataContract]
    public class Price
    {
        [DataMember] public int value { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencyType { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
    }

    [DataContract]
    public class OrderHistoryInfo
    {
        [DataMember] public string orderNo { get; set; }
        [DataMember(Name = "operator")] public string Operator { get; set; }
        [DataMember] public string action { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class OrderInfo
    {
        [DataMember] public string orderNo { get; set; }
        [DataMember] public string paymentOrderNo { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public bool sandbox { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public int price { get; set; }
        [DataMember] public int discountedPrice { get; set; }
        [DataMember] public string paymentProvider { get; set; }
        [DataMember] public int vat { get; set; }
        [DataMember] public int salesTax { get; set; }
        [DataMember] public int paymentProviderFee { get; set; }
        [DataMember] public int paymentMethodFee { get; set; }
        [DataMember] public CurrencySummary currency { get; set; }
        [DataMember] public string paymentStationUrl { get; set; }
        [DataMember] public ItemSnapshot itemSnapshot { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public OrderStatus status { get; set; } //['INIT', 'CHARGED', 'CHARGE_FAILED', 'FULFILLED', 'FULFILL_FAILED', 'REFUNDING', 'REFUNDED', 'DELETED'],
        [DataMember] public string statusReason { get; set; }
        [DataMember] public DateTime createdTime { get; set; }
        [DataMember] public DateTime chargedTime { get; set; }
        [DataMember] public DateTime fulfilledTime { get; set; }
        [DataMember] public DateTime refundedTime { get; set; }
        [DataMember] public DateTime chargebackTime { get; set; }
        [DataMember] public DateTime chargebackReversedTime { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class OrderPagingSlicedResult
    {
        [DataMember] public OrderInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class OrderRequest
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public int price { get; set; }
        [DataMember] public int discountedPrice { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public string returnUrl { get; set; }
    }

    [DataContract]
    public class OrderTransaction
    {
        [DataMember] public string txId { get; set; }
        [DataMember] public int amount { get; set; }
        [DataMember] public int vat { get; set; }
        [DataMember] public int salesTax { get; set; }
        [DataMember] public CurrencySummary currency { get; set; }
        [DataMember] public string type { get; set; } // = ['CHARGE', 'REFUND'],
        [DataMember] public string status { get; set; } // = ['INIT', 'FINISHED', 'FAILED'],
        [DataMember] public string provider { get; set; } // = ['XSOLLA', 'WALLET'],
        [DataMember] public int paymentProviderFee { get; set; }
        [DataMember] public string paymentMethod { get; set; }
        [DataMember] public int paymentMethodFee { get; set; }
        [DataMember] public string merchantId { get; set; }
        [DataMember] public string extTxId { get; set; }
        [DataMember] public string extStatusCode { get; set; }
        [DataMember] public string extMessage { get; set; }
        [DataMember] public string txStartTime { get; set; }
        [DataMember] public string txEndTime { get; set; }
    }

    #endregion

    #region Entitlements

    [DataContract]
    public class EntitlementReward
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string itemSku { get; set; }
        [DataMember] public int count { get; set; }
    }

    [DataContract]
    public class EntitlementInfo
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public EntitlementClazz clazz { get; set; } // ['APP', 'ENTITLEMENT', 'CODE']
        [DataMember] public EntitlementType type { get; set; } //  ['DURABLE', 'CONSUMABLE']
        [DataMember] public EntitlementStatus status { get; set; } // ['ACTIVE', 'INACTIVE', 'CONSUMED', 'REVOKED']
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; } // ['GAME', 'SOFTWARE', 'DLC', 'DEMO']
        [DataMember] public string sku { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string bundleItemId { get; set; }
        [DataMember] public string grantedCode { get; set; }
        [DataMember] public string itemNamespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string[] features { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public EntitlementSource source { get; set; } // ['PURCHASE', 'IAP', 'PROMOTION', 'ACHIEVEMENT', 'REFERRAL_BONUS', 'REDEEM_CODE', 'OTHER']
        [DataMember] public ItemSnapshot itemSnapshot { get; set; }
        [DataMember] public DateTime startDate { get; set; }
        [DataMember] public DateTime endDate { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public DateTime grantedAt { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public ItemOptionBoxConfig optionBoxConfig { get; set; }
        [DataMember] public string requestId { get; set; }
        [DataMember] public bool replayed { get; set; }
        [DataMember] public EntitlementReward[] rewards { get; set; }
        [DataMember] public LootBoxConfig lootBoxConfig { get; set; }
    }

    [DataContract]
    public class EntitlementPagingSlicedResult
    {
        [DataMember] public EntitlementInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class EntitlementSummary
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public EntitlementClazz clazz { get; set; }
        [DataMember] public EntitlementType type { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public int stackedUseCount { get; set; }
        [DataMember] public int stackedQuantity { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public string grantedCode { get; set; }
        [DataMember] public DateTime startDate { get; set; }
        [DataMember] public DateTime endDate { get; set; }
    }

    [DataContract]
    public class ConsumeUserEntitlementRequest
    {
        [DataMember] public int useCount { get; set; }
        [DataMember] public string[] options { get; set; }
        [DataMember] public string requestId { get; set; }
    };

    [DataContract]
    public class EntitlementOwnershipItemIds
    {
        [DataMember] public bool owned { get; set; }
        [DataMember] public string itemId { get; set; }
    };

    [DataContract]
    public class GrantUserEntitlementRequest
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string grantedCode { get; set; }
        [DataMember] public string itemNamespace { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public EntitlementSource source { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime? startDate { get; set; }
        [DataMember] public DateTime? endDate { get; set; }
    }

    [DataContract]
    public class StackableEntitlementInfo
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public EntitlementClazz clazz { get; set; } // ['APP', 'ENTITLEMENT', 'DISTRIBUTION', 'CODE']
        [DataMember] public EntitlementType type { get; set; } //  ['DURABLE', 'CONSUMABLE']
        [DataMember] public EntitlementStatus status { get; set; } // ['ACTIVE', 'INACTIVE', 'CONSUMED', 'DISTRIBUTED', 'REVOKED']
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; } // ['GAME', 'SOFTWARE', 'DLC', 'DEMO']
        [DataMember] public string sku { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string grantedCode { get; set; }
        [DataMember] public string itemNamespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public EntitlementSource source { get; set; } // ['PURCHASE', 'IAP', 'PROMOTION', 'ACHIEVEMENT', 'REFERRAL_BONUS', 'REDEEM_CODE', 'OTHER']
        [DataMember] public int distributedQuantity { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public ItemSnapshot itemSnapshot { get; set; }
        [DataMember] public DateTime startDate { get; set; }
        [DataMember] public DateTime endDate { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public DateTime grantedAt { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public int stackedUseCount { get; set; }
        [DataMember] public int stackedQuantity { get; set; }
    }

    [DataContract]
    public class DistributionAttributes
    {
        [DataMember] public Attributes attributes { get; set; }
    }

    [DataContract]
    public class Attributes
    {
        [DataMember] public string serverId { get; set; }
        [DataMember] public string serverName { get; set; }
        [DataMember] public string characterId { get; set; }
        [DataMember] public string characterName { get; set; }
    }

    [DataContract]
    public class DistributionReceiver
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string namespace_ { get; set; }
        [DataMember] public string extUserId { get; set; }
        [DataMember] public Attributes attributes { get; set; }
    }

    [DataContract]
    public class Ownership
    {
        [DataMember] public bool owned { get; set; }
        [DataMember] public DateTime endDate { get; set; }
    }

    [DataContract]
    public class OwnershipToken
    {
        [DataMember] public string ownershipToken { get; set; }
    }

    [DataContract]
    public class OwnershipTokenPayload
    {
        [DataMember] public string sub { get; set; }
        [DataMember] public int iat { get; set; }
        [DataMember] public int exp { get; set; }
        [DataMember] public OwnershipEntitlement[] entitlements { get; set; }
    }

    [DataContract]
    public class OwnershipEntitlement
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public string appType { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string itemNamespace { get; set; }
        [DataMember] public DateTime grantedAt { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class XBoxDLCSync
    {
        [DataMember] public string xstsToken { get; set; }
    }

    [DataContract]
    public class PlayStationDLCSync
    {
        [DataMember] public string productId { get; set; }
        [DataMember] public int price { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public int serviceLabel { get; set; }
    }

    [DataContract]
    public class TwitchDropSync
    {
        [DataMember] public string gameId { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
    }

    #endregion

    #region Fulfillment

    [DataContract]
    public class FulfillmentRequest
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public string orderNo { get; set; }
        [DataMember] public ItemSource source { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
    }

    [DataContract]
    public class FulFillCodeRequest
    {
        [DataMember] public string code { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
    }

    [DataContract]
    public class CreditSummary
    {
        [DataMember] public string walletId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public int amount { get; set; }
        [DataMember] public int stackedQuantity { get; set; }
    }

    [DataContract]
    public class FulfillmentResult
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public EntitlementSummary[] entitlementSummaries { get; set; }
        [DataMember] public CreditSummary[] creditSummaries { get; set; }
    }

    #endregion

    #region Reward

    [DataContract]
    public class RewardItem
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public int quantity { get; set; }
    }

    [DataContract]
    public class RewardCondition
    {
        [DataMember] public string conditionName { get; set; }
        [DataMember] public string condition { get; set; }
        [DataMember] public string eventName { get; set; }
        [DataMember] public RewardItem[] rewardItems { get; set; }
    }

    [DataContract]
    public class RewardInfo
    {
        [DataMember] public string rewardId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string rewardCode { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string eventTopic { get; set; }
        [DataMember] public RewardCondition[] rewardConditions { get; set; }
        [DataMember] int maxAwarded { get; set; }
        [DataMember] int maxAwardedPerUser { get; set; }
        [DataMember] DateTime createdAt { get; set; }
        [DataMember] DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class QueryRewardInfo
    {
        [DataMember] public RewardInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class PlatformPredicateValidateResults
    {
        [DataMember] public string predicateName { get; set; }
        [DataMember] public string validate { get; set; }
        [DataMember] public string[] matched { get; set; }
        [DataMember] public string[] unmatched { get; set; }
    }

    [DataContract]
    public class PlatformValidateDetails
    {
        [DataMember] public PlatformPredicateValidateResults[] predicateValidateResults { get; set; }
    }

    [DataContract]
    public class PlatformValidateUserItemPurchaseResponse
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember] public bool purchaseable { get; set; }
        [DataMember] public PlatformValidateDetails[] validateDetails { get; set; }
    }

    #endregion

    #region SyncPurchaseMobile

    [DataContract]
    public class PlatformSyncMobileApple
    {
        [DataMember] public string productId { get; set; }
        [DataMember] public string transactionId { get; set; }
        [DataMember] public string receiptData { get; set; }
        [DataMember] public bool excludeOldTransactions { get; set; }
        [DataMember] public string region { get; set; } //optional
        [DataMember] public string language { get; set; } //optional
    }

    [DataContract]
    public class PlatformSyncMobileGoogle
    {
        [DataMember] public string orderId { get; set; }
        [DataMember] public string packageName { get; set; }
        [DataMember] public string productId { get; set; }
        [DataMember] public long purchaseTime { get; set; }
        [DataMember] public string purchaseToken { get; set; }
        [DataMember] public string region { get; set; } //optional
        [DataMember] public string language { get; set; } //optional
        [DataMember] public bool autoAck { get; set; }  //should be true for sync DURABLE item entitlement
    }

    #endregion

    [DataContract]
    public class SectionInfo
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string longDescription { get; set; }
        [DataMember] public string sectionId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string viewId { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public bool active { get; set; }
        [DataMember] public DateTime startDate { get; set; }
        [DataMember] public DateTime endDate { get; set; }
        [DataMember] public ItemInfo[] currentRotationItems { get; set; }
        [DataMember] public DateTime currentRotationExpireAt { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public object localExt { get; set; }
    }

    [DataContract]
    public class ViewInfo
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string longDescription { get; set; }
        [DataMember] public string viewId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int displayOrder { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public object localExt { get; set; }
    }
}