## Platform Services
User should be logged in first before it can use platform service modules.

![platform-services](http://www.plantuml.com/plantuml/png/VP312e9054NtynM3UtyXb5Ok8Y6YXRGOd9T8CqwyUTOY_FUKIBk1RTyvtBbtiYY6fVTEIIkETEcCN09xVI2-jpL5LRlGM_rKrTO8DO5RYXzuLbKXvECiZuSzM-9lusIS8raEWreYYtm7u6Rggsb89nF8ooAPf6CaHZgN14MdDZowTACX5dgF_gBz6uJYsaYqrfQZzViTw_tUgx0H8ko135z7fWQLKMTxxru0)

## Catalog (Categories and Items)

Catalog module manipulates categories and items inside a store.

```csharp
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
    [DataMember] public string appType { get; set; }
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
```

## Categories

Each category has items inside it. You can get a list of items by criteria or by its ID.


### Get Category

Get category from a store in a namespace. Required parameter is **categoryPath** and optional parameter is **language**. It'll return the Category from the callback result.

```csharp
public static void Main(string[] args)
{
    Categories categories = AccelBytePlugin.GetCategories();
    categories.GetCategory("/game/weapon/example", "en", result => {
        // showing display name of category
        Debug.Log(result.Value.displayName);
    });
}
```

### Get Root Categories

Get root categories that exist in the store. Optional parameter is **language**. It'll return an array of Category from the result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    AccelByte.Api.Categories category= AccelBytePlugin.GetCategories();
    category.GetRootCategories("en", result => {
        foreach (c in result.Value)
        {
            Debug.Log(c.displayName);
        }
    });
}
```

### Get Child Categories

Get child categories of a category. This method requires **categoryPath** parameter and optional **language** parameter. It'll return an array of Category from result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    AccelByte.Api.Categories category= AccelBytePlugin.GetCategories();
    category.GetChildCategories("/game/spell", "en", result => {
        foreach (c in result.Value)
        {
            Debug.Log(c.displayName);
        }
    });
}
```

### Get Descendant Categories

Get every descendant category of a category. Required parameter is **categoryPath** and optional parameter is **language**.
This will retrieve all category in a flat array that needs to be rearranged into proper category tree.

<!--language lang-cs -->

    public void GetDescendantCategories(string categoryPath, string language, ResultCallback<Category[]> callback){}

Usage:

```csharp
public static void Main(string[] args)
{
    AccelByte.Api.Categories category = AccelBytePlugin.GetCategories();
    category.GetDescendantCategories("/game/potion", "en", result => {
        // result type is Result<Category[]>
        // result.Value type is Category[]

        // showing the amount of descendant categories
        Debug.Log(result.Value.Length);
    });
}
```

## Items

An item represents a single product sold in the online store.


### Get Item by Id

Get an item information from a store. Required parameters are **itemId** and **region**. Optional parameter is **language**. It'll return an Item object.

Usage:

```csharp
public static void Main(string[] args)
{
    Items items = AccelBytePlugin.GetItems();
    items.GetItemById("0123456789", "US", "en", result => {            // showing the name of the item
        Debug.Log(result.Value.title);
    });
}
```

### Get Items by Criteria

Get a paged items with specific criteria/filter from a store. Required parameters are **region** and **item criteria**. Optional parameter is **language**. It'll return a paged items that fulfilled the criteria. ItemCriteria is required but it doesn't have to be completely filled.

Usage:

```csharp
public static void Main(string[] args)
{
    ItemCriteria criteria = new ItemCriteria(){
        ItemType = null,
        CategoryPath = "/games/weapon",
        ItemStatus = "ACTIVE",
        Page = 0,
        Size = 20
    };

    Items items = AccelBytePlugin.GetItems();
    items.GetItemsByCriteria(criteria, "US", "en", result => {
        // result type is Result<PagedItems>
        // result.Value type is PagedItems
        // result.Value.data type is Item[]

        // showing the amount of items
        Debug.Log(result.Value.data.Length);
    });
}
```

## Orders

Orders is used to make purchases or tracking them from inside the game.

```csharp
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
    [DataMember] public string status { get; set; } //['INIT', 'CHARGED', 'CHARGE_FAILED', 'FULFILLED', 'FULFILL_FAILED', 'REFUNDING', 'REFUNDED', 'DELETED'],
    [DataMember] public string statusReason { get; set; }
    [DataMember] public string @namespace { get; set; }
    [DataMember] public DateTime createdTime { get; set; }
    [DataMember] public DateTime chargedTime { get; set; }
    [DataMember] public DateTime fulfilledTime { get; set; }
    [DataMember] public DateTime refundedTime { get; set; }
}
```

### Create Order

Create order to purchase something from store. It requires OrderRequest as a parameter. It'll return from the result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    OrderRequest orderRequest = new OrderRequest(){
        itemId = "012345678",
        quantity = 4,
        price = 40,
        discounted price = 40,
        currencyCode = "USD",
        returnUrl = "https://example.com"
    };

    Orders orders = AccelBytePlugin.GetOrders();
    orders.CreateOrder(orderRequest, result => {
        // result type is Result<OrderInfo>
        // result.Value type is OrderInfo

        // showing the order number
        Debug.Log(result.Value.orderNo);
    });
}
```

### Fulfilling an Order

On order can be fulfilled by two methods:

1. Cash, using Xsolla payment integration

    This method needs user to open web browser and input their payment details to complete order.

2. Wallet (virtual currency)

    The order will be automatically fulfilled if a user has enough amount of virtual currency. To use this method, change the currency code to currency code for the virtual currency.

### Get User Order

Get user's order information. It requires the order's number (**orderNo**) as parameter. It'll return the OrderInfo from the result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    Orders orders = AccelBytePlugin.GetOrders();
    orders.GetUserOrder("1223334444", result => {
        // result type is Result<OrderInfo>
        // result.Value type is OrderInfo

        // showing the order number
        Debug.Log(result.Value.orderNo);
    });
}
```

### Get User Orders

Get every order that has been created for a user with paging. It has two optional parameters; **startPage** and **size**. It'll return a PagedOrderInfo from the result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    Orders orders = AccelBytePlugin.GetOrders();
    orders.GetUserOrders(0, 20, result => {
        // result type is Result<PagedOrderInfo>
        // result.Value type is PagedOrderInfo
        // result.Value.data type is OrderInfo[]

        // showing the amount of orders
        Debug.Log(result.Value.data.Length);
    });
}
```

### Get User Order History

Get history of an order that has been created. It requires an **orderNo** parameter. It'll return an array of OrderHistoryInfo from the result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    AccelByte.Api.Orders orders = AccelBytePlugin.GetOrders();
    order.GetUserOrderHistory("1223334444", result => {
        // result type is Result<OrderHistoryInfo[]>
        // result.Value type is OrderHistoryInfo[]

        // showing the amount of order's history
        Debug.Log(result.Value.Length);
    });
}
```

## Wallet

Wallet stores in-game virtual currency to make purchases (create orders) on in-game items in in-game store.

### Get WalletInfo by Currency Code

Get WalletInfo for a specific currency code. It requires **currencyCode** as a parameter. It'll return a WalletInfo from the result callback.

Usage:

```csharp
public static void Main(string[] args)
{
    AccelByte.Api.Wallet wallet = AccelBytePlugin.GetWallet();
    wallet.GetWalletInfoByCurrencyCode("1223334444", result => {
        // result type is Result<WalletInfo>
        // result.Value type is WalletInfo

        // showing the wallet's id
        Debug.Log(result.Value.id);
    });
}
```

## Entitlement

Entitlement contains in-game items from current user.

```csharp
[DataContract]
public class Entitlement
{
    [DataMember] public string id { get; set; }
    [DataMember(Name = "namespace")] public string Namespace { get; set; }
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
```

```csharp
[DataContract]
public class PagedEntitlements
{
    [DataMember] public Entitlement[] data { get; set; }
    [DataMember] public Paging paging { get; set; }
}
```

### Get User Entitlement

    Get Entitlement from a current user. It requires **offset** and **limit** as the parameters. It'll return a PagedEntitlements from the result callback.

    ```csharp
    public void GetUserEntitlements(int offset, int limit, ResultCallback<PagedEntitlements> callback) {}
    ```

    Usage without limit:

    ```csharp
    public static void Main(string[] args)
    {
        AccelByte.Api.Entitlements entitlements = AccelBytePlugin.GetEntitlements();
        entitlements.GetUserEntitlements("0", "10", result => {
            // result type is Result<PagedEntitlements>
            // result.Value type is PagedEntitlements

            // showing the entitlement's item id from first entitlement 
            Debug.Log(result.Value.data[0].itemId);
        });
    }
    ```
    
---