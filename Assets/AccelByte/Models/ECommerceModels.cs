// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;

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
        CODE
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
        DISTRIBUTION,
        CODE
    }

    public enum EntitlementStatus
    {
        NONE,
        ACTIVE,
        INACTIVE,
        CONSUMED,
        DISTRIBUTED,
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

    public enum EntitlementSource
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
        [DataMember] public double balance { get; set; }
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
    }

    [DataContract]
    public class ItemPagingSlicedResult
    {
        [DataMember] public ItemInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
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
        [DataMember] public EntitlementClazz clazz { get; set; } // ['APP', 'ENTITLEMENT', 'DISTRIBUTION', 'CODE']
        [DataMember] public EntitlementType type { get; set; } //  ['DURABLE', 'CONSUMABLE']
        [DataMember] public EntitlementStatus status { get; set; } // ['ACTIVE', 'INACTIVE', 'CONSUMED', 'DISTRIBUTED', 'REVOKED']
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
        [DataMember] public int quantity { get; set; }
        [DataMember] public EntitlementSource source { get; set; } // ['PURCHASE', 'IAP', 'PROMOTION', 'ACHIEVEMENT', 'REFERRAL_BONUS', 'REDEEM_CODE', 'OTHER']
        [DataMember] public int distributedQuantity { get; set; }
        [DataMember] public string targetNamespace { get; set; }
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

    #endregion
}