// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    #region Enum

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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
        LOOTBOX,
        EXTENSION
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ItemStatus
    {
        NONE,
        ACTIVE,
        INACTIVE
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum CurrencyType
    {
        NONE,
        REAL,
        VIRTUAL
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum EntitlementStatus
    {
        NONE,
        ACTIVE,
        INACTIVE,
        CONSUMED,
        REVOKED,
        SOLD
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum EntitlementAppType
    {
        NONE,
        GAME,
        SOFTWARE,
        DLC,
        DEMO
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum EntitlementType
    {
        NONE,
        DURABLE,
        CONSUMABLE
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum SeasonType
    {
        PASS = 0,
        TIER
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum RewardSortBy
    {
        NAMESPACE,
        NAMESPACE_ASC,
        NAMESPACE_DESC,
        REWARDCODE,
        REWARDCODE_ASC,
        REWARDCODE_DESC
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum Cycle
    {
        WEEKLY,
        MONTHLY,
        QUARTERLY,
        YEARLY
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum PredicateType
    {
        EntitlementPredicate,
        SeasonPassPredicate,
        SeasonTierPredicate,
        StatisticCodePredicate
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum RewardType
    {
        REWARD,
        REWARD_GROUP,
        PROBABILITY_GROUP
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum SubscribedBy
    {
        None,
        User,
        Platform
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum SubscriptionStatus
    {
        None,
        Init,
        Active,
        Cancelled,
        Expired
    }
    
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum IAPOrderStatus
    {
        Verified,
        Fulfilled,
        Failed
    }     

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum EntitlementItemIdentityType
    {
        None = 0,
        [EnumMember(Value = "ITEM_ID")]
        ItemId,
        [EnumMember(Value = "ITEM_SKU")]
        ItemSku, 
    }     

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum EntitlementIAPOrderStatus
    {
        None = 0,
        [EnumMember(Value = "VERIFIED")]
        Verified,
        [EnumMember(Value = "FULFILLED")]
        Fulfilled,
        [EnumMember(Value = "FAILED")]
        Failed
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum PaymentProvider
    {
        None = 0,
        [EnumMember(Value = "WALLET")] 
        Wallet, 
        [EnumMember(Value = "XSOLLA")] 
        Xsolla, 
        [EnumMember(Value = "ADYEN")] 
        Adyen,
        [EnumMember(Value = "STRIPE")] 
        Stripe,
        [EnumMember(Value = "CHECKOUT")] 
        Checkout, 
        [EnumMember(Value = "ALIPAY")] 
        Alipay,
        [EnumMember(Value = "WXPAY")]
        Wxpay,
        [EnumMember(Value = "PAYPAL")] 
        Paypal 
    }

    #endregion

    #region Wallet

    [DataContract, Preserve]
    public class CurrencySummary
    {
        [DataMember] public string currencyCode;
        [DataMember] public string currencySymbol;
        [DataMember] public string currencyType; // = ['REAL', 'VIRTUAL'],
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public int decimals;
    }

    [DataContract, Preserve]
    public class BalanceInfo
    {
        [DataMember] public string id;
        [DataMember] public string walletId;
        [DataMember] public string currencyCode;
        [DataMember] public int balance;
        [DataMember] public string balanceSource;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public string status;
    }

    [DataContract, Preserve]
    public class TimeLimitedBalance
    {
        [DataMember] public int balance;
        [DataMember] public string balanceSource;
        [DataMember] public DateTime expiredAt;
    }

    [DataContract, Preserve]
    public class WalletInfo
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string currencyCode;
        [DataMember] public string currencySymbol;
        [DataMember] public int balance;
        [DataMember] public string balanceOrigin;
        [DataMember] public TimeLimitedBalance[] timeLimitedBalances;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public int totalPermanentBalance;
        [DataMember] public int totalTimeLimitedBalance;
        [DataMember] public ItemStatus status;
    }

    [DataContract, Preserve]
    public class WalletInfoResponse
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string currencyCode;
        [DataMember] public string currencySymbol;
        [DataMember] public int balance;
        [DataMember] public WalletInfo[] walletInfos;
        [DataMember] public ItemStatus walletStatus;
        [DataMember] public ItemStatus status;
        [DataMember] public string id;
    }

    [DataContract, Preserve]
    public class WalletTransactionInfo
    {
        [DataMember] public string walletId;
        [DataMember] public int amount;
        [DataMember] public string reason;
        [DataMember] public string userId;
        [DataMember(Name = "operator")] public string Operator;
        [DataMember] public string walletAction;
        [DataMember] public string currencyCode;
        [DataMember] public string balanceSource;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
    }

    [DataContract, Preserve]
    public class WalletTransactionPagingSlicedResult
    {
        [DataMember] public WalletTransactionInfo[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class CreditUserWalletRequest
    {
        [DataMember] public int amount;
        [DataMember] public CreditUserWalletSource source;
        [DataMember] public string reason;
        [DataMember] public WalletTable origin;
    }

    [DataContract, Preserve]
    public class CreditUserWalletResponse
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string currencySymbol;
        [DataMember] public string currencyCode;
        [DataMember] public int balance;
        [DataMember] public string balanceOrigin;
        [DataMember] public TimeLimitedBalance[] timeLimitedBalances;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public int totalPermanentBalance;
        [DataMember] public int totalTimeLimitedBalance;
        [DataMember] public ItemStatus status;
    }

    #endregion

    #region Categories

    [DataContract, Preserve]
    public class CategoryInfo
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string parentCategoryPath;
        [DataMember] public string categoryPath;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public string displayName;
        [DataMember] public bool root;
    }

    #endregion

    #region Items

    [DataContract, Preserve]
    public class RegionDataItem
    {
        [DataMember] public int price;
        [DataMember] public int discountPercentage;
        [DataMember] public int discountAmount;
        [DataMember] public int discountedPrice;
        [DataMember] public string currencyCode;
        [DataMember] public string currencyType;
        [DataMember] public string currencyNamespace;
        [DataMember] public int trialPrice;
        [DataMember] public DateTime purchaseAt;
        [DataMember] public DateTime expireAt;
        [DataMember] public DateTime discountPurchaseAt;
        [DataMember] public DateTime discountExpireAt;
    }

    [DataContract, Preserve]
    public class ItemSnapshot
    {
        [DataMember] public string itemId;
        [DataMember] public string appId;
        [DataMember] public EntitlementAppType appType;
        [DataMember] public string baseAppId;
        [DataMember] public string sku;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public bool listable;
        [DataMember] public EntitlementType entitlementType;
        [DataMember] public int useCount;
        [DataMember] public bool stackable;
        [DataMember] public bool purchasable;
        [DataMember] public ItemType itemType;
        [DataMember] public string thumbnailUrl;
        [DataMember] public string targetNamespace;
        [DataMember] public string targetCurrencyCode;
        [DataMember] public string targetItemId;
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public ItemRecurring recurring;
        [DataMember] public RegionDataItem regionDataItem;
        [DataMember] public string[] itemIds;
        [DataMember] public Dictionary<string, int> itemQty;
        [DataMember] public string[] features;
        [DataMember] public int maxCountPerUser;
        [DataMember] public int maxCount;
        [DataMember] public string boothName;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public ItemOptionBoxConfig optionBoxConfig;
    }

    [DataContract, Preserve]
    public class ItemCriteria
    {
        [DataMember] public bool AutoCalcEstimatedPrice = false;
        [DataMember] public string storeId;
        [DataMember] public ItemType itemType;
        [DataMember] public EntitlementAppType appType;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public string categoryPath;
        [DataMember] public bool includeSubCategoryItem = false;// Default == false. Can change to "true". Anything != "true" will remain default
        [DataMember] public string baseAppId;
        [DataMember] public string[] tags;
        [DataMember] public string[] features;
        [DataMember] public int? offset;
        [DataMember] public int? limit;
        [DataMember] public string sortBy;
    }

    [DataContract, Preserve]
    public class ItemCriteriaV2
    {
        [DataMember] public string storeId;
        [DataMember] public string categoryPath;
        [DataMember] public bool includeSubCategoryItem = false;// Default == false. Can change to "true". Anything != "true" will remain default
        [DataMember] public ItemType itemType;
        [DataMember] public EntitlementAppType appType;
        [DataMember] public string baseAppId;
        [DataMember] public string[] tags;
        [DataMember] public string[] features;
        [DataMember] public bool activeOnly;
        [DataMember] public string region;
        [DataMember] public DateTime availableDate;
        [DataMember] public string targetNamespace;
        [DataMember] public int? offset;
        [DataMember] public int? limit;
        [DataMember] public string sortBy;
    }

    [DataContract, Preserve]
    public class ItemCriteriaV3
    {
        [DataMember] public string StoreId;
        [DataMember] public string CategoryPath;
        [DataMember] public bool IncludeSubCategoryItem = false;// Default == false. Can change to "true". Anything != "true" will remain default
        [DataMember] public ItemType ItemType;
        [DataMember] public EntitlementAppType AppType;
        [DataMember] public string BaseAppId;
        [DataMember] public string[] Tags;
        [DataMember] public string[] Features;
        [DataMember] public ItemStatus ItemStatus;
        [DataMember] public string Region;
        [DataMember] public DateTime AvailableDate;
        [DataMember] public string TargetNamespace;
        [DataMember] public string ItemName;
        [DataMember] public bool SectionExclusive;
        [DataMember] public int? Offset;
        [DataMember] public int? Limit;
        [DataMember] public string[] SortBy;
    }

    [DataContract, Preserve]
    public class Image
    {
        [DataMember(Name = "as")] public string As;
        [DataMember] public string caption;
        [DataMember] public int height;
        [DataMember] public int width;
        [DataMember] public string imageUrl;
        [DataMember] public string smallImageUrl;
    }

    [DataContract, Preserve]
    public class ItemRecurring
    {
        [DataMember] public Cycle cycle;
        [DataMember] public int fixedFreeDays;
        [DataMember] public int fixedTrialCycles;
        [DataMember] public int graceDays;
    }

    [DataContract, Preserve]
    public class ItemPredicate
    {
        [DataMember] public string name;
        [DataMember] public PredicateType predicateType;
        [DataMember] public string comparison;
        [DataMember] public int anyOf;
        [DataMember] public string[] values;
        [DataMember] public string value;
        [DataMember(Name = "code")] public string Code;
    }

    [DataContract, Preserve]
    public class ItemConditionGroup
    {
        [DataMember] public ItemPredicate[] predicates;
        [DataMember(Name = "operator")] public string Operator;
    }

    [DataContract, Preserve]
    public class ItemPurchaseCondition
    {
        [DataMember] public ItemConditionGroup[] conditionGroups;
    }

    [DataContract, Preserve]
    public class ItemBoxItem
    {
        [DataMember] public string itemId;
        [DataMember] public string itemSku;
        [DataMember] public string ItemType;
        [DataMember] public int Duration;
        [DataMember] public DateTime EndDate;
        [DataMember] public int count;
    }

    [DataContract, Preserve]
    public class ItemOptionBoxConfig
    {
        [DataMember] public ItemBoxItem[] boxItems;
    }

    [DataContract, Preserve]
    public class LootBoxItems
    {
        [DataMember] public string itemId;
        [DataMember] public string itemSku;
        [DataMember] public string itemType;
        [DataMember] public int Duration;
        [DataMember] public DateTime EndDate;
        [DataMember] public int count;
    }

    [DataContract, Preserve]
    public class ItemRewards
    {
        [DataMember] public string name;
        [DataMember] public RewardType type;//REWARD, REWARD_GROUP, PROBABILITY_GROUP
        [DataMember] public LootBoxItems[] lootBoxItems;
        [DataMember] public int weight;
        [DataMember] public double odds;
    }

    [DataContract, Preserve]
    public class LootBoxConfig
    {
        [DataMember] public int rewardCount;
        [DataMember] public ItemRewards[] rewards;
    }

    [DataContract, Preserve]
    public class ItemSaleConfig
    {
        [DataMember] public string CurrencyCode;
        [DataMember] public int Price;
    }

    [DataContract, Preserve]
    public class PopulatedItemInfo
    {
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public string longDescription;
        [DataMember] public string itemId;
        [DataMember] public string appId;
        [DataMember] public EntitlementAppType appType;  
        [DataMember] public string baseAppId;
        [DataMember] public string sku;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public EntitlementType entitlementType;
        [DataMember] public int useCount;
        [DataMember] public bool stackable;
        [DataMember] public string categoryPath;
        [DataMember] public ItemStatus status;
        [DataMember] public ItemType itemType;
        [DataMember] public string targetNamespace;
        [DataMember] public string targetCurrencyCode;
        [DataMember] public string targetItemId;
        [DataMember] public Image[] images;
        [DataMember] public string thumbnailUrl;
        [DataMember] public RegionDataItem[] regionData;
        [DataMember] public string[] itemIds;
        [DataMember] public string[] tags;
        [DataMember] public string[] features;
        [DataMember] public int maxCountPerUser;
        [DataMember] public int maxCount;
        [DataMember] public string clazz;
        [DataMember] public string boothName;
        [DataMember] public int displayOrder;
        [DataMember] public object ext;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public ItemInfo[] items;
        [DataMember] public Dictionary<string, object> localExt;
        [DataMember] public Dictionary<string, int> itemQty;
        [DataMember(Name = "flexible")] public bool Flexible;
        [DataMember(Name = "listable")] public bool Listable;
        [DataMember(Name = "purchasable")] public bool Purchasable;
        [DataMember(Name = "sellable")] public bool Sellable;
        [DataMember(Name = "sectionExclusive")] public bool SectionExclusive;
        [DataMember(Name = "boundItemIds")] public string[] BoundItemIds;
    }

    [DataContract, Preserve]
    public class ItemInfo
    {
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public string longDescription;
        [DataMember] public string itemId;
        [DataMember] public string appId;
        [DataMember] public EntitlementAppType appType;  
        [DataMember] public SeasonType SeasonType;
        [DataMember] public string baseAppId;
        [DataMember] public string sku;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public EntitlementType entitlementType;
        [DataMember] public int useCount;
        // 	Whether stack the CONSUMABLE entitlement
        [DataMember] public bool stackable;
        [DataMember] public string categoryPath;
        [DataMember] public ItemStatus status;
        // Whether can be visible in Store for public user
        [DataMember] public bool listable;
        // Whether can be purchased
        [DataMember] public bool purchasable;
        // Whether it is sold in section only
        [DataMember(Name = "sectionExclusive")] public bool SectionExclusive;
        [DataMember] public ItemType itemType;
        [DataMember] public string targetNamespace;
        [DataMember] public string targetCurrencyCode;
        [DataMember] public string targetItemId;
        [DataMember] public Image[] images;
        [DataMember] public string thumbnailUrl;
        [DataMember] public RegionDataItem[] regionData;
        [DataMember] public ItemRecurring recurring;
        [DataMember] public string[] itemIds;
        [DataMember] public string[] boundItemIds;
        [DataMember] public string[] tags;
        [DataMember] public string[] features;
        [DataMember] public int maxCountPerUser;
        [DataMember] public int maxCount;
        [DataMember] public string clazz;
        [DataMember] public string boothName;
        [DataMember] public int displayOrder;
        [DataMember] public object ext;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public ItemPurchaseCondition purchaseCondition;
        [DataMember] public ItemOptionBoxConfig optionBoxConfig;
        [DataMember] public bool Fresh;
        // Whether allow to sell back to store
        [DataMember(Name = "sellable")] public bool Sellable;
        [DataMember] public ItemSaleConfig SaleConfig;
        //[DataMember] public string localExt;
        [DataMember] public Dictionary<string, int> itemQty;
        [DataMember] public LootBoxConfig lootBoxConfig;
        // Whether flexible pricing applied, only applied if itemType is BUNDLE
        [DataMember(Name = "flexible")] public bool Flexible;
    }

    [DataContract, Preserve]
    public class ItemPagingSlicedResult
    {
        [DataMember] public ItemInfo[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class ItemInfoV2
    {
        [DataMember] public string itemId;
        [DataMember] public string appId;
        [DataMember] public EntitlementAppType appType;  
        [DataMember] public SeasonType SeasonType;
        [DataMember] public string baseAppId;
        [DataMember] public string sku;
        [DataMember] public string name;
        [DataMember] public EntitlementType entitlementType;
        [DataMember] public int useCount;
        [DataMember] public bool stackable;
        [DataMember] public string categoryPath;
        [DataMember] public Image[] images;
        [DataMember] public string thumbnailUrl;
        [DataMember] public Dictionary<string, object> localizations;
        [DataMember] public ItemStatus status;
        [DataMember] public bool listable;
        [DataMember] public bool purchasable;
        [DataMember] public bool SectionExclusive;
        [DataMember] public ItemType itemType;
        [DataMember] public string targetNamespace;
        [DataMember] public string targetCurrencyCode;
        [DataMember] public string targetItemId;
        [DataMember] public Dictionary<string, object> regionData;
        [DataMember] public ItemRecurring recurring;
        [DataMember] public string[] itemIds;
        [DataMember] public Dictionary<string, int> itemQty;
        [DataMember] public string[] boundItemIds;
        [DataMember] public string[] tags;
        [DataMember] public string[] features;
        [DataMember] public int maxCountPerUser;
        [DataMember] public int maxCount;
        [DataMember] public string clazz;
        [DataMember] public Dictionary<string, object> ext;
        [DataMember] public string boothName;
        [DataMember] public int displayOrder;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public ItemPurchaseCondition purchaseCondition;
        [DataMember] public ItemOptionBoxConfig optionBoxConfig;
        [DataMember] public LootBoxConfig lootBoxConfig;
        [DataMember] public bool Sellable;
        [DataMember] public ItemSaleConfig SaleConfig;
    }

    [DataContract, Preserve]
    public class ItemPagingSlicedResultV2
    {
        [DataMember] public ItemInfoV2[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class CurrencyList
    {
        [DataMember] public string currencyCode;
        [DataMember] public Dictionary<string, string> localizationDescriptions;
        [DataMember] public string currencySymbol;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public CurrencyType currencyType;
        [DataMember] public int decimals;
        [DataMember] public int maxAmountPerTransaction;
        [DataMember] public int maxTransactionAmountPerDay;
        [DataMember] public int maxBalanceAmount;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
    }

    [DataContract, Preserve]
    public class PlatformStore
    {
        [DataMember] public string storeId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public bool published;
        [DataMember] public string[] supportedLanguages;
        [DataMember] public string[] supportedRegions;
        [DataMember] public string defaultRegion;
        [DataMember] public string defaultLanguage;
        [DataMember] DateTime publishedTime;
        [DataMember] DateTime createdAt;
        [DataMember] DateTime updatedAt;
    }

    [DataContract, Preserve]
    public class PriceDetail
    {
        [DataMember(Name = "price")] public int Price;
        // Current discounted price per item
        [DataMember(Name = "discountedPrice")] public int DiscountedPrice; 
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemSku")] public string ItemSku;
        [DataMember(Name = "quantity")] public int Quantity;
        // Whether owns this durable item id, consumable item is always false or null.
        [DataMember(Name = "owned")] public string Owned; 
        [DataMember(Name = "itemName")] public string ItemName;
    }

    [DataContract, Preserve]
    public class EstimatedPriceInfo
    {
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        [DataMember(Name = "currencyNamespace")] public string CurrencyNamespace;
        // Current full price, only calc total price with un-owned durable items if it's a flexible bundle item.
        [DataMember(Name = "price")] public int Price; 
        // Current available discounted price, only calc total discounted price with un-owned durable items if it's a flexible bundle item.
        [DataMember(Name = "discountedPrice")] public int DiscountedPrice; 
        [DataMember(Name = "priceDetails")] public PriceDetail[] PriceDetails;
    }

    [DataContract, Preserve]
    public class EstimatedPricesInfo
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "region")] public string Region;
        // Estimated prices in different currency under region, this field maybe null or empty if have not any available price at this time,
        // Possible reason: This item is not yet for sale, or miss set correct region currency for flexible bundle item.
        [DataMember(Name = "estimatedPrices")] public EstimatedPriceInfo[] EstimatedPrices;
    }

    #endregion

    #region Orders

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum OrderDeductionType
    {
        None,
        [EnumMember(Value = "DISCOUNT_CODE")]
        DiscountCode,
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum OrderDiscountType
    {
        None,
        [EnumMember(Value = "AMOUNT")]
        Amount,
        [EnumMember(Value = "PERCENTAGE")]
        Percentage,
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum OrderRestrictType
    {
        [EnumMember(Value = "NONE")]
        None,
        [EnumMember(Value = "ITEMS_AND_CATEGORIES")]
        ItemsAndCategories,
    }

    [DataContract, Preserve]
    public class PaymentUrl
    {
        [DataMember] public string paymentProvider; //['XSOLLA', 'WALLET'],
        [DataMember] public string paymentUrl;
        [DataMember] public string paymentToken;
        [DataMember] public string returnUrl;
        [DataMember] public string paymentType; //['QR_CODE', 'LINK']
    }

    [DataContract, Preserve]
    public class Price
    {
        [DataMember] public int value;
        [DataMember] public string currencyCode;
        [DataMember] public string currencyType;
        [DataMember(Name = "namespace")] public string Namespace;
    }

    [DataContract, Preserve]
    public class OrderHistoryInfo
    {
        [DataMember] public string orderNo;
        [DataMember(Name = "operator")] public string Operator;
        [DataMember] public string action;
        [DataMember] public string reason;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
    }

    [DataContract, Preserve]
    public class OrderBundleItemInfo
    { 
        [DataMember(Name = "price")] public int Price;
        [DataMember(Name = "discountedPrice")] public int DiscountedPrice;
        [DataMember(Name = "itemName")] public string ItemName;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemSku")] public string ItemSku;
        [DataMember(Name = "quantity")] public int Quantity;
        [DataMember(Name = "purchased")] public bool Purchased; 
    }
    
    [DataContract, Preserve]
    public class OrderDiscountItem
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemName")] public string ItemName;
    }
    
    [DataContract, Preserve]
    public class OrderDiscountCategory
    {
        [DataMember(Name = "categoryPath")] public string CategoryPath;
        [DataMember(Name = "includeSubCategories")] public bool IncludeSubCategories;
    }
    
    [DataContract, Preserve]
    public class OrderDiscountConfig
    {
        [DataMember(Name = "currencyNamespace")] public string CurrencyNamespace;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        [DataMember(Name = "discountType")] public OrderDiscountType DiscountType;
        [DataMember(Name = "discountPercentage")] public int DiscountPercentage;
        [DataMember(Name = "discountAmount")] public int DiscountAmount;
        [DataMember(Name = "stackable")] public bool Stackable;
        [DataMember(Name = "restrictType")] public OrderRestrictType RestrictType;
        [DataMember(Name = "items")] public OrderDiscountItem[] Items;
        [DataMember(Name = "categories")] public OrderDiscountCategory[] Categories;
    }
    
    [DataContract, Preserve]
    public class OrderDiscountCodeInfo
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "campaignId")] public string CampaignId;
        [DataMember(Name = "campaignName")] public string CampaignName;
        [DataMember(Name = "deduction")] public int Deduction;
        [DataMember(Name = "discountConfig")] public OrderDiscountConfig DiscountConfig;
    }
    
    [DataContract, Preserve]
    public class OrderDiscountCodeDeductionDetail
    {
        [DataMember(Name = "totalDeduction")] public int TotalDeduction;
        [DataMember(Name = "totalPercentageDeduction")] public int TotalPercentageDeduction;
        [DataMember(Name = "totalAmountDeduction")] public int TotalAmountDeduction;
        [DataMember(Name = "discountPercentageCodes")] public OrderDiscountCodeInfo[] DiscountPercentageCodes;
        [DataMember(Name = "discountAmountCodes")] public OrderDiscountCodeInfo[] DiscountAmountCodes;
    }
    
    [DataContract, Preserve]
    public class OrderDeductionDetail
    {
        [DataMember(Name = "deductionType")] public OrderDeductionType DeductionType;
        [DataMember(Name = "discountCodeDeductionDetail")] public OrderDiscountCodeDeductionDetail DiscountCodeDeductionDetail;
    }

    [DataContract, Preserve]
    public class OrderInfo
    {
        [DataMember] public string orderNo;
        [DataMember] public string paymentOrderNo;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string itemId;
        [DataMember] public bool sandbox;
        [DataMember] public int quantity;
        [DataMember] public int price;
        [DataMember] public int discountedPrice;
        [DataMember] public string paymentProvider;
        [DataMember(Name = "tax")] public double Tax;
        [DataMember] public int vat;
        [DataMember] public int salesTax;
        [DataMember] public int paymentProviderFee;
        [DataMember] public int paymentMethodFee;
        [DataMember] public CurrencySummary currency;
        [DataMember] public string paymentStationUrl;
        [DataMember] public ItemSnapshot itemSnapshot;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public OrderStatus status; //['INIT', 'CHARGED', 'CHARGE_FAILED', 'FULFILLED', 'FULFILL_FAILED', 'REFUNDING', 'REFUNDED', 'DELETED'],
        [DataMember] public string statusReason;
        [DataMember] public DateTime createdTime;
        [DataMember] public DateTime chargedTime;
        [DataMember] public DateTime fulfilledTime;
        [DataMember] public DateTime refundedTime;
        [DataMember] public DateTime chargebackTime;
        [DataMember] public DateTime chargebackReversedTime;
        [DataMember(Name = "expireTime")] public DateTime ExpireTime;
        [DataMember(Name = "paymentRemainSeconds")] public int PaymentRemainSeconds; 
        [DataMember(Name = "ext")] public object Ext;
        [DataMember(Name = "totalTax")] public int TotalTax;
        [DataMember(Name = "totalPrice")] public int TotalPrice;
        [DataMember(Name = "subtotalPrice")] public int SubtotalPrice;
        [DataMember(Name = "orderBundleItemInfos")] public OrderBundleItemInfo[] OrderBundleItemInfos;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember(Name = "deduction")] public int Deduction;
        [DataMember(Name = "deductionDetails")] public OrderDeductionDetail[] DeductionDetails;
    }

    [DataContract, Preserve]
    public class OrderPagingSlicedResult
    {
        [DataMember] public OrderInfo[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class OrderRequest
    {
        [DataMember] public string itemId;
        [DataMember] public int quantity;
        [DataMember] public int price;
        [DataMember] public int discountedPrice;
        [DataMember] public string currencyCode;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public string returnUrl;
        [DataMember(Name = "sectionId")] public string SectionId;
        [DataMember(Name = "discountCodes")] public string[] DiscountCodes;
    }

    [DataContract, Preserve]
    public class OrderDiscountPreviewRequest
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "quantity")] public int Quantity;
        [DataMember(Name = "price")] public int Price;
        [DataMember(Name = "discountedPrice")] public int DiscountedPrice;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        [DataMember(Name = "discountCodes")] public string[] DiscountCodes;
    }

    [DataContract, Preserve]
    public class OrderDiscountPreviewResponse
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "quantity")] public int Quantity;
        [DataMember(Name = "price")] public int Price;
        [DataMember(Name = "discountedPrice")] public int DiscountedPrice;
        [DataMember(Name = "deduction")] public int Deduction;
        [DataMember(Name = "deductionDetails")] public OrderDeductionDetail[] DeductionDetails;
        [DataMember(Name = "finalPrice")] public int FinalPrice;
    }

    [DataContract, Preserve]
    public class OrderTransaction
    {
        [DataMember] public string txId;
        [DataMember] public int amount;
        [DataMember] public int vat;
        [DataMember] public int salesTax;
        [DataMember] public CurrencySummary currency;
        [DataMember] public string type; // = ['CHARGE', 'REFUND'],
        [DataMember] public string status; // = ['INIT', 'FINISHED', 'FAILED'],
        [DataMember] public string provider; // = ['XSOLLA', 'WALLET'],
        [DataMember] public int paymentProviderFee;
        [DataMember] public string paymentMethod;
        [DataMember] public int paymentMethodFee;
        [DataMember] public string merchantId;
        [DataMember] public string extTxId;
        [DataMember] public string extStatusCode;
        [DataMember] public string extMessage;
        [DataMember] public string txStartTime;
        [DataMember] public string txEndTime;
    }

    #endregion

    #region Entitlements

    [DataContract, Preserve]
    public class EntitlementReward
    {
        [DataMember] public string itemId;
        [DataMember] public string itemSku;
        [DataMember] public int count;
    }

    [Preserve]
    public class PlatformStoreId
    {
        private string currentPlatformStoreId;
        private readonly Dictionary<PlatformType, string> platformTypeAndPlatformStoreNameMap = new Dictionary<PlatformType, string>()
        {
            {PlatformType.Steam, "STEAM"},
            {PlatformType.EpicGames, "EPICGAMES"},
            {PlatformType.PS4, "PLAYSTATION"},
            {PlatformType.PS4Web, "PLAYSTATION"},
            {PlatformType.PS5, "PLAYSTATION"},
            {PlatformType.Google, "GOOGLE"},
            {PlatformType.GooglePlayGames, "GOOGLE"},
            {PlatformType.Android, "GOOGLE"},
            {PlatformType.Live, "XBOX"},
            {PlatformType.Apple, "APPLE"},
            {PlatformType.iOS, "APPLE"},
            {PlatformType.Oculus, "OCULUS"},
            {PlatformType.Twitch, "TWITCH"}
        };

        public PlatformStoreId(string platformId)
        {
            currentPlatformStoreId = platformId;
        }

        public PlatformStoreId(PlatformType pType)
        {
            var successGetValue = platformTypeAndPlatformStoreNameMap.TryGetValue(pType, out currentPlatformStoreId);
            if (!successGetValue)
            {
                AccelByteDebug.LogWarning($"subscription for {pType} doesn't exist");
            }
        }

        internal string GetStorePlatformName()
        {
            return currentPlatformStoreId;
        }
    }

    [Preserve]
    public class QueryUserSubscriptionRequestOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Optional param for set the result offset
        /// </summary>
        public int Offset = 0;
        /// <summary>
        /// Optional param for set the result limit
        /// </summary>
        public int Limit = 20;
        /// <summary>
        /// Optional param to show active subscription only
        /// </summary>
        public bool? ActiveOnly;
        /// <summary>
        /// Optional param to show only specific product Id
        /// </summary>
        public string ProductId;
        /// <summary>
        /// Optional param to show only specific group id
        /// </summary>
        public string GroupId;
    }

    [DataContract, Preserve]
    public class ThirdPartyUserSubscriptionInfo
    {
        [DataMember (Name="id")] public string Id;
        [DataMember (Name="namespace")] public string Namespace;
        [DataMember (Name="platform")] public string Platform;
        [DataMember (Name="active")] public bool Active;
        [DataMember (Name="status")] public string Status;
        [DataMember (Name="subscriptionGroupId")] public string SubscriptionGroupId;
        [DataMember (Name="subscriptionProductId")] public string SubscriptionProductId;
        [DataMember (Name="startAt")] public DateTime StartAt;
        [DataMember (Name="expiredAt")] public DateTime ExpiredAt;
        [DataMember (Name="lastTransactionId")] public string LastTransactionId;
        [DataMember (Name="createdAt")] public DateTime CreatedAt;
        [DataMember (Name="updatedAt")] public DateTime UpdatedAt;
    }
    
    [DataContract, Preserve]
    public class SubscriptionPagingSlicedResult
    {
        [DataMember(Name = "data")] public ThirdPartyUserSubscriptionInfo[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }
    
    [DataContract, Preserve]
    public class EntitlementInfo
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public EntitlementClazz clazz; // ['APP', 'ENTITLEMENT', 'CODE']
        [DataMember] public EntitlementType type; //  ['DURABLE', 'CONSUMABLE']
        [DataMember] public EntitlementStatus status; // ['ACTIVE', 'INACTIVE', 'CONSUMED', 'REVOKED']
        [DataMember] public string appId;
        [DataMember] public EntitlementAppType appType; // ['GAME', 'SOFTWARE', 'DLC', 'DEMO']
        [DataMember] public string sku;
        [DataMember] public string userId;
        [DataMember] public string itemId;
        [DataMember] public string bundleItemId;
        [DataMember] public string grantedCode;
        [DataMember] public string itemNamespace;
        [DataMember] public string name;
        [DataMember] public string[] features;
        [DataMember] public int useCount;
        [DataMember] public EntitlementSource source; // ['PURCHASE', 'IAP', 'PROMOTION', 'ACHIEVEMENT', 'REFERRAL_BONUS', 'REDEEM_CODE', 'OTHER']
        [DataMember] public ItemSnapshot itemSnapshot;
        [DataMember] public DateTime startDate;
        [DataMember] public DateTime endDate;
        [DataMember] public bool stackable;
        [DataMember] public DateTime grantedAt;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public ItemOptionBoxConfig optionBoxConfig;
        [DataMember] public string requestId;
        [DataMember] public bool replayed;
        [DataMember] public EntitlementReward[] rewards;
        [DataMember] public LootBoxConfig lootBoxConfig;
    }

    [Preserve]
    public class GetUserEntitlementHistoryOptionalParams : OptionalParametersBase
    {
        /// <summary>
        /// Filter entitlement query based on Clazz.
        /// </summary>
        public EntitlementClazz EntitlementClazz = EntitlementClazz.NONE;

        /// <summary>
        /// Filter entitlement query based on start date.
        /// </summary>
        public DateTime? StartDate = null;

        /// <summary>
        /// Filter entitlement query based on end date.
        /// </summary>
        public DateTime? EndDate = null;

        /// <summary>
        /// Offset of the paginated list that has been sliced.
        /// </summary>
        public int Offset = 0;

        /// <summary>
        /// Limit of items to be displayed on each page.
        /// </summary>
        public int Limit = 20;
    }

    [DataContract, Preserve]
    public class UserEntitlementHistory
    {
        [DataMember(Name = "entitlementId")] public string EntitlementId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "action")] public string Action;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "useCount")] public int UseCount;
        [DataMember(Name = "useCountChange")] public int UseCountChange;
        [DataMember(Name = "reason")] public string Reason;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
        [DataMember(Name = "origin")] public WalletTable Origin;
        [DataMember(Name = "clazz")] public EntitlementClazz Clazz;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "sku")] public string Sku;
    }

    [DataContract, Preserve]
    public class UserEntitlementHistoryResponse
    {
        [DataMember(Name = "data")] public UserEntitlementHistory[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class EntitlementPagingSlicedResult
    {
        [DataMember] public EntitlementInfo[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class EntitlementSummary
    {
        [DataMember] public string id;
        [DataMember] public string itemId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "name")] public string Name;
        [DataMember] public string userId;
        [DataMember] public EntitlementClazz clazz;
        [DataMember] public EntitlementType type;
        [DataMember] public bool stackable;
        [DataMember] public int stackedUseCount;
        [DataMember(Name = "storeId")] public int StoreId;
        [DataMember] public int stackedQuantity;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public string grantedCode;
        [DataMember] public DateTime startDate;
        [DataMember] public DateTime endDate;
    }

    [DataContract, Preserve]
    public class ConsumeUserEntitlementRequest
    {
        [DataMember] public int useCount;
        [DataMember] public string[] options;
        [DataMember] public string requestId;
    };

    [DataContract, Preserve]
    public class EntitlementOwnershipItemIds
    {
        [DataMember] public bool owned;
        [DataMember] public string itemId;
    };

    [DataContract, Preserve]
    public class GrantUserEntitlementRequest
    {
        [DataMember] public string itemId;
        [DataMember] public string grantedCode;
        [DataMember] public string itemNamespace;
        [DataMember] public int quantity;
        [DataMember] public EntitlementSource source;
        [DataMember] public string region;
        [DataMember] public string language;
        [DataMember] public DateTime? startDate;
        [DataMember] public DateTime? endDate;
    }

    [DataContract, Preserve]
    public class StackableEntitlementInfo
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public EntitlementClazz clazz; // ['APP', 'ENTITLEMENT', 'DISTRIBUTION', 'CODE']
        [DataMember] public EntitlementType type; //  ['DURABLE', 'CONSUMABLE']
        [DataMember] public EntitlementStatus status; // ['ACTIVE', 'INACTIVE', 'CONSUMED', 'DISTRIBUTED', 'REVOKED']
        [DataMember] public string appId;
        [DataMember] public EntitlementAppType appType; // ['GAME', 'SOFTWARE', 'DLC', 'DEMO']
        [DataMember] public string sku;
        [DataMember] public string userId;
        [DataMember(Name = "storeId")] public string StoreId;
        [DataMember] public string itemId;
        [DataMember] public string grantedCode;
        [DataMember] public string itemNamespace;
        [DataMember] public string name;
        [DataMember] public int useCount;
        [DataMember] public int quantity;
        [DataMember] public EntitlementSource source; // ['PURCHASE', 'IAP', 'PROMOTION', 'ACHIEVEMENT', 'REFERRAL_BONUS', 'REDEEM_CODE', 'OTHER']
        [DataMember] public int distributedQuantity;
        [DataMember] public string targetNamespace;
        [DataMember] public ItemSnapshot itemSnapshot;
        [DataMember] public DateTime startDate;
        [DataMember] public DateTime endDate;
        [DataMember] public bool stackable;
        [DataMember] public DateTime grantedAt;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public int stackedUseCount;
        [DataMember] public int stackedQuantity;
    }

    [DataContract, Preserve]
    public class DistributionAttributes
    {
        [DataMember] public Attributes attributes;
    }

    [DataContract, Preserve]
    public class Attributes
    {
        [DataMember] public string serverId;
        [DataMember] public string serverName;
        [DataMember] public string characterId;
        [DataMember] public string characterName;
    }

    [DataContract, Preserve]
    public class DistributionReceiver
    {
        [DataMember] public string userId;
        [DataMember] public string namespace_;
        [DataMember] public string extUserId;
        [DataMember] public Attributes attributes;
    }

    [DataContract, Preserve]
    public class Ownership
    {
        [DataMember] public bool owned;
        [DataMember] public DateTime endDate;
    }

    [DataContract, Preserve]
    public class OwnershipToken
    {
        [DataMember] public string ownershipToken;
    }

    [DataContract, Preserve]
    public class OwnershipTokenPayload
    {
        [DataMember] public string sub;
        [DataMember] public int iat;
        [DataMember] public int exp;
        [DataMember] public OwnershipEntitlement[] entitlements;
    }

    [DataContract, Preserve]
    public class OwnershipEntitlement
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string appId;
        [DataMember] public string appType;
        [DataMember] public string sku;
        [DataMember] public string itemId;
        [DataMember] public string itemNamespace;
        [DataMember] public DateTime grantedAt;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
    }

    [DataContract, Preserve]
    internal class SyncSteamInventoryRequest
    {
        [DataMember(Name = "steamId")] public string SteamId;
        [DataMember(Name = "appId")] public string AppId;
        [DataMember(Name = "region")] public string Region;
        [DataMember(Name = "language")] public string Language;
        [DataMember(Name = "productId")] public string ProductId;
        [DataMember(Name = "price")] public double Price;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;

        public SyncSteamInventoryRequest()
        {

        }

        internal SyncSteamInventoryRequest(string steamId
            , string appId
            , string productId
            , double price
            , string currencyCode
            , SyncSteamInventoryOptionalParameters optionalParameters)
        {
            SteamId = steamId;
            AppId = appId;
            ProductId = productId;
            Price = price;
            CurrencyCode = currencyCode;
            Region = optionalParameters?.Region;
            Language = optionalParameters?.Language;

            if (Region == "")
            {
                Region = null;
            }

            if (Language == "")
            {
                Language = null;
            }
        }
    }

    [DataContract, Preserve]
    public class XBoxDLCSync
    {
        [DataMember] public string xstsToken;
    }

    [DataContract, Preserve]
    public class PlayStationDLCSync
    {
        [DataMember] public string productId;
        [DataMember] public int price;
        [DataMember] public string currencyCode;
        [DataMember] public int serviceLabel;
    }
    
    [DataContract, Preserve]
    public class PlayStationDLCSyncMultipleService
    {
        [DataMember(Name = "serviceLabel")] public int[] ServiceLabel;
    }
    
    [DataContract, Preserve]
    public class PlayStationMultipleServiceRequest
    {
        [DataMember(Name = "productId")] public string ProductId;
        [DataMember(Name = "price")] public int Price;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        [DataMember(Name = "serviceLabel")] public int[] ServiceLabel;
    }
    
    [DataContract, Preserve]
    public class PlayStationMultipleServiceResponse
    {
        [DataMember(Name = "transactionId")] public string TransactionId;
        [DataMember(Name = "psnItemId")] public string PsnItemId;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "sku")] public string Sku;
        [DataMember(Name = "status")] public IAPOrderStatus Status;
    }

    [DataContract, Preserve]
    public class TwitchDropSync
    {
        [DataMember] public string gameId;
        [DataMember] public string region;
        [DataMember] public string language;
    }

    public class UserEntitlementSoldParams
    {
        public string UserId;
        public string EntitlementId;
    }

    [DataContract, Preserve]
    public class EntitlementSoldRequest
    {
        [DataMember(Name = "useCount")] public int UseCount;

        [DataMember(Name = "requestId")] internal string RequestId;
    }

    [DataContract, Preserve]
    public class SellItemEntitlementInfo
    {
        [DataMember(Name = "requestId")] public string RequestId;
        [DataMember(Name = "replayed")] public bool Replayed;
        [DataMember(Name = "creditSummaries")] public CreditSummary[] CreditSummaries;
        [DataMember(Name = "entitlementInfo")] public EntitlementInfo EntitlementInfo;
    }

    [DataContract, Preserve]
    public class SyncOculusConsumableEntitlementResponse
    {
        [DataMember(Name = "transactionId")] public string TransactionId;
        [DataMember(Name = "oculusItemSku")] public string OculusItemSku;
        [DataMember(Name = "itemIdentityType")] public EntitlementItemIdentityType ItemIdentityType;
        [DataMember(Name = "itemIdentity")] public string ItemIdentity;
        [DataMember(Name = "iapOrderStatus")] public EntitlementIAPOrderStatus IapOrderStatus;
    }

    [DataContract, Preserve]
    public class SyncEpicGamesInventoryResponse
    {
        [DataMember(Name = "transactionId")] public string TransactionId;
        [DataMember(Name = "epicGamesItemId")] public string EpicGamesItemId;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "sku")] public string Sku;
        [DataMember(Name = "status")] public EntitlementIAPOrderStatus Status;
    }

    [DataContract, Preserve]
    public class DlcConfigRewardShortInfo
    {
        [DataMember(Name = "dlcType")] public string DlcType;
        [DataMember(Name = "data")] public Dictionary<string, object> Data;
    }

    #endregion

    #region Fulfillment

    [DataContract, Preserve]
    public class FulfillmentRequest
    {
        [DataMember] public string itemId;
        [DataMember] public int quantity;
        [DataMember] public string orderNo;
        [DataMember] public ItemSource source;
        [DataMember] public string region;
        [DataMember] public string language;
    }

    [DataContract, Preserve]
    public class FulFillCodeRequest
    {
        [DataMember] public string code;
        [DataMember] public string region;
        [DataMember] public string language;
    }

    [DataContract, Preserve]
    public class CreditSummary
    {
        [DataMember] public string walletId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public int amount;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        [DataMember] public int stackedQuantity;
    }

    [DataContract, Preserve]
    public class SubscriptionSummary
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "sku")] public string Sku;
        [DataMember(Name = "status")] public SubscriptionStatus Status;
        [DataMember(Name = "currentPeriodStart")] public DateTime CurrentPeriodStart;
        [DataMember(Name = "currentPeriodEnd")] public DateTime CurrentPeriodEnd;
        [DataMember(Name = "subscribedBy")] public SubscribedBy SubscribedBy;
    }

    [DataContract, Preserve]
    public class FulfillmentResult
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public EntitlementSummary[] entitlementSummaries;
        [DataMember] public CreditSummary[] creditSummaries;
        [DataMember(Name = "subscriptionSummaries")] public SubscriptionSummary[] subscriptionSummaries;
    }

    #endregion

    #region Reward

    [DataContract, Preserve]
    public class RewardItem
    {
        [DataMember] public string itemId;
        [DataMember] public int quantity;
        [DataMember] public int Duration;
        [DataMember] DateTime EndDate;
    }

    [DataContract, Preserve]
    public class RewardCondition
    {
        [DataMember] public string conditionName;
        [DataMember] public string condition;
        [DataMember] public string eventName;
        [DataMember] public RewardItem[] rewardItems;
    }

    [DataContract, Preserve]
    public class RewardInfo
    {
        [DataMember] public string rewardId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string rewardCode;
        [DataMember] public string description;
        [DataMember] public string eventTopic;
        [DataMember] public RewardCondition[] rewardConditions;
        [DataMember] int maxAwarded;
        [DataMember] int maxAwardedPerUser;
        [DataMember] DateTime createdAt;
        [DataMember] DateTime updatedAt;
        [DataMember] public string UserIdExpression;
        [DataMember] public string NamespaceExpression;
    }

    [DataContract, Preserve]
    public class QueryRewardInfo
    {
        [DataMember] public RewardInfo[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class PlatformPredicateValidateResults
    {
        [DataMember] public string predicateName;
        [DataMember] public string validate;
        [DataMember] public string[] matched;
        [DataMember] public string[] unmatched;
    }

    [DataContract, Preserve]
    public class PlatformValidateDetails
    {
        [DataMember] public PlatformPredicateValidateResults[] predicateValidateResults;
    }

    [DataContract, Preserve]
    public class ValidateUserItemPurchaseConditionRequest
    {
        [DataMember(Name = "itemIds")] public string[] ItemIds;
    }

    [DataContract, Preserve]
    public class PlatformValidateUserItemPurchaseResponse
    {
        [DataMember] public string itemId;
        [DataMember] public string sku;
        [DataMember] public bool purchaseable;
        [DataMember] public PlatformValidateDetails[] validateDetails;
    }

    #endregion

    #region SyncPurchaseMobile

    [DataContract, Preserve]
    public class CurrentAppleConfigVersion
    {
        [DataMember(Name = "version")] public string Version;
    }
    
    [DataContract, Preserve]
    public class PlatformSyncMobileAppleV2
    {        
        [DataMember(Name = "transactionId")] public string TransactionId;
    }

    [DataContract, Preserve]
    public class PlatformSyncMobileApple
    {
        [DataMember] public string productId;
        [DataMember] public string transactionId;
        [DataMember] public string receiptData;
        [DataMember] public bool excludeOldTransactions;
        [DataMember] public string region; //optional
        [DataMember] public string language; //optional

        public PlatformSyncMobileApple()
        {
        }

        internal PlatformSyncMobileApple(string productId, string transactionId, string receiptData, PlatformSyncMobileAppleOptionalParam optionalParam)
        {
            this.productId = productId;
            this.transactionId = transactionId;
            this.receiptData = receiptData;
            this.excludeOldTransactions = optionalParam == null? true : optionalParam.ExcludeOldTransactions;
            this.region = optionalParam?.Region;
            this.language = optionalParam?.Language;

            if (string.IsNullOrEmpty(region))
            {
                this.region = null;
            }

            if (string.IsNullOrEmpty(language))
            {
                this.language = null;
            }
        }
    }

    [Preserve]
    public class PlatformSyncMobileAppleOptionalParam : PlatformSyncMobileApple
    {
        /// <summary>
        /// Either exclude or include old transaction
        /// </summary>
        public bool ExcludeOldTransactions = true;
        /// <summary>
        /// Optional param for product region
        /// </summary>
        public string Region = null;
        /// <summary>
        /// Optional param for product language
        /// </summary>
        public string Language = null;
        /// <summary>
        /// Accelbyte logger instance to use logging functions within the interface.
        /// </summary>
        internal IDebugger Logger;
        internal Utils.IApiTracker ApiTracker;
    }

    [Preserve]
    public class PlatformSyncMobileGoogleOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Optional param for AutoConsume.
        /// Automatically consume product after fulfill item.
        /// </summary>
        public bool? AutoConsume = null;
        /// <summary>
        /// Optional param for product region
        /// </summary>
        public string Region = null;
        /// <summary>
        /// Optional param for product language
        /// </summary>
        public string Language = null;
    }

    [Preserve]
    public class SyncSteamInventoryOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Optional param for product region
        /// </summary>
        public string Region = null;
        /// <summary>
        /// Optional param for product language
        /// </summary>
        public string Language = null;
    }

    [DataContract, Preserve]
    public class PlatformSyncMobileGoogle
    {
        [DataMember] public string orderId;
        [DataMember] public string packageName;
        [DataMember] public string productId;
        [DataMember] public long purchaseTime;
        [DataMember] public string purchaseToken;
        [DataMember] public string region; //optional
        [DataMember] public string language; //optional
        [DataMember] public bool autoAck;  //should be true for sync DURABLE item entitlement
        [DataMember(Name = "autoConsume"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoConsume;
        [DataMember(Name = "subscriptionPurchase"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SubscriptionPurchase;
        
        public PlatformSyncMobileGoogle()
        {
            
        }

        internal PlatformSyncMobileGoogle(string orderId
            , string packageName
            , string productId
            , long purchaseTime
            , string purchaseToken
            , bool autoAck
            , bool subscriptionPurchase
            , PlatformSyncMobileGoogleOptionalParameters optionalParameters)
        {
            this.orderId = orderId;
            this.packageName = packageName;
            this.productId = productId;
            this.purchaseTime = purchaseTime;
            this.purchaseToken = purchaseToken;
            this.autoAck = autoAck;
            this.SubscriptionPurchase = subscriptionPurchase;
            this.region = optionalParameters?.Region;
            this.language = optionalParameters?.Language;
            this.AutoConsume = optionalParameters?.AutoConsume;

            if (region == "")
            {
                region = null;
            }

            if (language == "")
            {
                language = null;
            }
        }
    }

    [DataContract, Preserve]
    public class GoogleReceiptResolveResult
    {
        [DataMember(Name = "needConsume")] public bool NeedConsume;
    }

    #endregion

    [DataContract, Preserve]
    public class SectionInfo
    {
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public string longDescription;
        [DataMember] public string sectionId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string viewId;
        [DataMember] public string name;
        [DataMember] public bool active;
        [DataMember] public DateTime startDate;
        [DataMember] public DateTime endDate;
        [DataMember] public ItemInfo[] currentRotationItems;
        [DataMember] public DateTime currentRotationExpireAt;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public object localExt;
    }

    [DataContract, Preserve]
    public class ViewInfo
    {
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public string longDescription;
        [DataMember] public string viewId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public int displayOrder;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public object localExt;
    }

    [DataContract, Preserve]
    public class XboxInventoryRequest
    {
        [DataMember(Name = "xstsToken")] public string XstsToken;
        [DataMember(Name = "productId")] public string ProductId;
        [DataMember(Name = "price")] public int Price;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
    }
    
    [DataContract, Preserve]
    public class XboxInventoryResponse
    {
        [DataMember(Name = "transactionId")] public string TransactionId;
        [DataMember(Name = "xboxProductId")] public string XboxProductId;
        [DataMember(Name = "itemId")] public int ItemId;
        [DataMember(Name = "sku")] public string Sku;
        [DataMember(Name = "status")] public IAPOrderStatus Status;
    }

    [DataContract, Preserve]
    public class UserOrdersRequest
    {
        [DataMember] public bool Discounted;
        [DataMember] public string ItemId;
        [DataMember] public OrderStatus Status = OrderStatus.INIT;
        [DataMember] public int Offset = 0;
        [DataMember] public int Limit = 20;
    }

    [Preserve]
    public class GetCurrencyListOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter returned results via their Currency Type.
        /// </summary>
        public CurrencyType? CurrencyType = Models.CurrencyType.NONE;
    }

    [Preserve]
    public class SellUserEntitlementOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Client should provide a unique request id to perform at most once execution
        /// </summary>
        public string RequestId;
    }

    [Preserve]
    public class QueryUserEntitlementsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Query based on entitlement name
        /// </summary>
        public string EntitlementName = null;

        /// <summary>
        /// Query based on Item's id
        /// </summary>
        public string ItemId = null;

        /// <summary>
        /// Offset of the list that has been sliced based on Limit parameter
        /// </summary>
        public int Offset = 0;

        /// <summary>
        /// The limit of item on page
        /// </summary>
        public int Limit = 20;

        /// <summary>
        /// Class of the entitlement
        /// </summary>
        public EntitlementClazz EntitlementClazz = EntitlementClazz.NONE;

        /// <summary>
        /// This is the type of application that entitled
        /// </summary>
        public EntitlementAppType EntitlementAppType = EntitlementAppType.NONE;
        
        /// <summary>
        /// Query based on the features
        /// </summary>
        public string[] Features = null;
    }
}