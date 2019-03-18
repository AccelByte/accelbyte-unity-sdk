// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    #region General

    [DataContract]
    public class Paging
    {
        [DataMember] string previous { get; set; }
        [DataMember] string next { get; set; }
    }

    #endregion

    #region Wallet

    [DataContract]
    public class CurrencySummary
    {
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public string currencyType { get; set; } // = ['REAL', 'VIRTUAL'],
        [DataMember] public string @namespace { get; set; }
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
        [DataMember] public string @namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string currencySymbol { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public double balance { get; set; }
        [DataMember] public string status { get; set; }
    }

    [DataContract]
    public class WalletTransaction
    {
        [DataMember] public string walletId { get; set; }
        [DataMember] public int amount { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string @operator { get; set; }
        [DataMember] public string walletAction { get; set; }
        [DataMember] public string currencyCode { get; set; }
        [DataMember] public string balanceSource { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class PagedWalletTransactions
    {
        [DataMember] public WalletTransaction[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    #endregion

    #region Categories

    [DataContract]
    public class Category
    {
        [DataMember] public string @namespace { get; set; }
        [DataMember] public string parentCategoryPath { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public Category[] childCategories { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public bool root { get; set; }
    }

    #endregion

    #region Items

    [DataContract]
    public class RegionData
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
        [DataMember] public int totalNum { get; set; }
        [DataMember] public int totalNumPerAccount { get; set; }
        [DataMember] public DateTime discountPurchaseAt { get; set; }
        [DataMember] public DateTime discountExpireAt { get; set; }
        [DataMember] public int discountTotalNum { get; set; }
        [DataMember] public int discountTotalNumPerAccount { get; set; }
    }

    //Type-safe enum for ItemType
    public sealed class ItemType
    {
        private readonly string name;

        public static readonly ItemType Product = new ItemType("PRODUCT");
        public static readonly ItemType Coins = new ItemType("COINS");
        public static readonly ItemType InGameItem = new ItemType("INGAMEITEM");
        public static readonly ItemType Bundle = new ItemType("BUNDLE");
        public static readonly ItemType App = new ItemType("APP");
        public static readonly ItemType Code = new ItemType("CODE");

        private ItemType(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }

    public sealed class ItemStatus
    {
        private readonly string name;

        public static readonly ItemStatus Active = new ItemStatus("ACTIVE");
        public static readonly ItemStatus Inactive = new ItemStatus("INACTIVE");
        public static readonly ItemStatus Deleted = new ItemStatus("DELETED");

        private ItemStatus(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }

    [DataContract]
    public class ItemSnapshot
    {
        [DataMember] public string itemId { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public EntitlementAppType appType { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember] public string @namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public ItemType itemType { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public string targetNamespace { get; set; }
        [DataMember] public string title { get; set; }
        [DataMember] public Image thumbnailImage { get; set; }
        [DataMember] public RegionData regionDataItem { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public int maxCountPerUser { get; set; }
        [DataMember] public int maxCount { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string language { get; set; }
    }

    [DataContract]
    public class ItemCriteria
    {
        [DataMember] public ItemType ItemType { get; set; }
        [DataMember] public string CategoryPath { get; set; }
        [DataMember] public ItemStatus ItemStatus { get; set; }
        [DataMember] public int? Page { get; set; }
        [DataMember] public int? Size { get; set; }
    }

    [DataContract]
    public class Image
    {
        [DataMember] public int height { get; set; }
        [DataMember] public int width { get; set; }
        [DataMember] public string imageUrl { get; set; }
        [DataMember] public string smallImageUrl { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string longDescription { get; set; }
        [DataMember] public Image[] images { get; set; }
        [DataMember] public Image thumbnailImage { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public string appType { get; set; } //"GAME"
        [DataMember] public string sku { get; set; }
        [DataMember] public string @namespace { get; set; }
        [DataMember] public string entitlementName { get; set; }
        [DataMember] public string entitlementType { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public string categoryPath { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string itemType { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public string targetCurrencyCode { get; set; }
        [DataMember] public RegionData[] regionData { get; set; }
        [DataMember] public string[] itemIds { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class PagedItems
    {
        [DataMember] public Item[] data { get; set; }
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
        [DataMember] public string @namespace { get; set; }
    }

    [DataContract]
    public class OrderHistoryInfo
    {
        [DataMember] public string orderNo { get; set; }
        [DataMember] public string @operator { get; set; }
        [DataMember] public string action { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class OrderInfo
    {
        [DataMember] public string orderNo { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public bool sandbox { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public int price { get; set; }
        [DataMember] public int discountedPrice { get; set; }
        [DataMember] public int vat { get; set; }
        [DataMember] public int salesTax { get; set; }
        [DataMember] public int paymentProviderFee { get; set; }
        [DataMember] public int paymentMethodFee { get; set; }
        [DataMember] public CurrencySummary currency { get; set; }
        [DataMember] public PaymentUrl paymentUrl { get; set; }
        [DataMember] public string paymentStationUrl { get; set; }
        [DataMember] public OrderTransaction[] transactions { get; set; }
        [DataMember] public string[] entitlementIds { get; set; }

        //['INIT', 'CHARGED', 'CHARGE_FAILED', 'FULFILLED', 'FULFILL_FAILED', 'REFUNDING', 'REFUNDED', 'DELETED'],
        [DataMember] public string status { get; set; }

        [DataMember] public string statusReason { get; set; }
        [DataMember] public string @namespace { get; set; }
        [DataMember] public DateTime createdTime { get; set; }
        [DataMember] public DateTime chargedTime { get; set; }
        [DataMember] public DateTime fulfilledTime { get; set; }
        [DataMember] public DateTime refundedTime { get; set; }
    }

    [DataContract]
    public class PagedOrderInfo
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
        [DataMember] public string returnUrl { get; set; }
        [DataMember] public string region { get; set; }
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

    //Type-safe enum for EntitlementClazz
    public sealed class EntitlementClazz
    {
        private readonly string name;

        public static readonly EntitlementClazz App = new EntitlementClazz("APP");
        public static readonly EntitlementClazz Entitlement = new EntitlementClazz("ENTITLEMENT");
        public static readonly EntitlementClazz Distribution = new EntitlementClazz("DISTRIBUTION");
        public static readonly EntitlementClazz Code = new EntitlementClazz("CODE");

        private EntitlementClazz(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }

    public sealed class EntitlementStatus
    {
        private readonly string name;

        public static readonly EntitlementStatus Active = new EntitlementStatus("ACTIVE");
        public static readonly EntitlementStatus Inactive = new EntitlementStatus("INACTIVE");
        public static readonly EntitlementStatus Distributed = new EntitlementStatus("DISTRIBUTED");
        public static readonly EntitlementStatus Revoked = new EntitlementStatus("REVOKED");
        public static readonly EntitlementStatus Deleted = new EntitlementStatus("DELETED");

        private EntitlementStatus(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }

    //Type-safe enum for EntitlementAppType
    public sealed class EntitlementAppType
    {
        private readonly string name;

        public static readonly EntitlementAppType Game = new EntitlementAppType("GAME");
        public static readonly EntitlementAppType Software = new EntitlementAppType("SOFTWARE");
        public static readonly EntitlementAppType DLC = new EntitlementAppType("DLC");
        public static readonly EntitlementAppType Demo = new EntitlementAppType("DEMO");

        private EntitlementAppType(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }

    //Type-safe enum for EntitlementType
    public sealed class EntitlementType
    {
        private readonly string name;

        public static readonly EntitlementType Durable = new EntitlementType("DURABLE");
        public static readonly EntitlementType Consumable = new EntitlementType("CONSUMABLE");

        private EntitlementType(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }

    [DataContract]
    public class Entitlement
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string @namespace { get; set; }
        [DataMember] public string clazz { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string appId { get; set; }
        [DataMember] public string appType { get; set; }
        [DataMember] public string sku { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string itemId { get; set; }
        [DataMember] public string bundleItemId { get; set; }
        [DataMember] public string grantedCode { get; set; }
        [DataMember] public string itemNamespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int useCount { get; set; }
        [DataMember] public int quantity { get; set; }
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
    public class PagedEntitlements
    {
        [DataMember] public Entitlement[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }
    
    #endregion
}