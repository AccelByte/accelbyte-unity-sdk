// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace AccelByte.Models
{
    #region Enum

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
        MEDIA
    }

    public enum ItemStatus
    {
        NONE,
        ACTIVE,
        INACTIVE
    }

    public enum CurrencyType
    {
        NONE,
        REAL,
        VIRTUAL
    }

    public enum EntitlementClazz
    {
        NONE,
        APP,
        ENTITLEMENT,
        CODE,
        MEDIA
    }

    public enum EntitlementStatus
    {
        NONE,
        ACTIVE,
        INACTIVE,
        CONSUMED,
        REVOKED
    }

    public enum EntitlementAppType
    {
        NONE,
        GAME,
        SOFTWARE,
        DLC,
        DEMO
    }

    public enum EntitlementType
    {
        NONE,
        DURABLE,
        CONSUMABLE
    }

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

    public enum SeasonType
    {
        PASS = 0,
        TIER
    }

    public enum RewardSortBy
    {
        NAMESPACE,
        NAMESPACE_ASC,
        NAMESPACE_DESC,
        REWARDCODE,
        REWARDCODE_ASC,
        REWARDCODE_DESC
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
    public class WalletInfo
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public int balance { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public ItemStatus status { get; set; }
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
        [DataMember] public EntitlementType entitlementType { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public bool stackable { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public string thumbnailUrl { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public string targetItemId { get; set; }
        [DataMember] public string title { get; set; }
        [DataMember] public RegionDataItem regionDataItem { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public int maxCountPerUser { get; set; }
        [DataMember] public int maxCount { get; set; }
        [DataMember] public string boothName { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
    }

    [DataContract]
    public class ItemCriteria
    {
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public string baseAppId { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string[] features { get; set; }
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
        [DataMember] public string ext { get; set; }
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
        [DataMember] public string ext { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public string localExt { get; set; }
        [DataMember] public Dictionary<string, int> itemQty { get; set; }
    }

    [DataContract]
    public class ItemPagingSlicedResult
    {
        [DataMember] public ItemInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class CurrencyList
    {
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public Dictionary<string, string> localizationDescriptions { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string currencyType { get; set; }
        [DataMember] public int decimals { get; set; }
        [DataMember] public int maxAmountPerTransaction { get; set; }
        [DataMember] public int maxTransactionAmountPerDay { get; set; }
        [DataMember] public int maxBalanceAmount { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
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
        [DataMember] public int useCount { get; set; }
        [DataMember] public EntitlementSource source { get; set; } // ['PURCHASE', 'IAP', 'PROMOTION', 'ACHIEVEMENT', 'REFERRAL_BONUS', 'REDEEM_CODE', 'OTHER']
        [DataMember] public ItemSnapshot itemSnapshot { get; set; }
        [DataMember] public DateTime startDate { get; set; }
        [DataMember] public DateTime endDate { get; set; }
        [DataMember] public DateTime grantedAt { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
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
        [DataMember(Name ="namespace")] public string Namespace { get; set; }
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
}
