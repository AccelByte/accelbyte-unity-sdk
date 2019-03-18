# AccelByte SDK Tutorial

- [AccelByte SDK Tutorial](#accelbyte-sdk-tutorial)
  - [Features](#features)
    - [Overview](#overview)
    - [Custom Coroutine System Under The Hood](#custom-coroutine-system-under-the-hood)
    - [Easy to Use API](#easy-to-use-api)
    - [Automatically retry temporary server errors](#automatically-retry-temporary-server-errors)
    - [Access Token Management](#access-token-management)
  - [User Management and Authorization](#user-management-and-authorization)
  - [Basic Services](#basic-services)
    - [User Profiles](#user-profiles)
  - [Platform Services](#platform-services)
    - [Catalog (Categories and Items)](#catalog-categories-and-items)
    - [Orders](#orders)
    - [Wallet](#wallet)
  - [Social Services](#social-services)
    - [Lobby Protocol](#lobby-protocol)
    - [Party](#party)
    - [Personal chat](#personal-chat)
    - [Notifications](#notifications)
    - [Friends](#friends)
    - [Matchmaking](#matchmaking)

---

## Features

### Overview

AccelByte SDK for Unity is not auto-generated so that we can provide excellent developer experience. It interfaces with multiple AccelByte backend services, so make sure that you have access to those services before you can use corresponding APIs in the SDK.

![sdk-components](http://www.plantuml.com/plantuml/png/ZP3BQiGm34Nt_WgH_Vn2cSmKIcWBWIn35ewZ55FR2hRI4ah-lGzeiC1zRA-Eq3qw9zQHL-yKMtO8jJ7eBXiZTBVtS2M_KC30IHb7wmHLDiVuOWsc2jvKtzIFty6W-ejErXp0Hz0wpJD5dsSFD8vRjdJzEj0nHVs4MNwdFK8RQROkYtUb5csUFz5AT3xgr6qpm6cYrt2wWM91IVnSMd2_k0Zk3p_9_ijThfJVKbGxtb5qae0maTE2GvcIhSlGhiuOfjMxTm00)

AccelByte SDK consists of:

1. IAM API
2. Basic API
3. Platform API
4. Lobby API
5. Cloud Storage API
6. Game Profile API (coming soon)

### Custom Coroutine System Under The Hood

AccelByte SDK is compatible with Unity 2017 or newer. Due to Unity 2017 still lags behind state of the art .NET technology, we make our own coroutine system inspired by Unity. Having our own coroutine system has some advantages:

1. It enables us to have it run on separate thread from main thread
2. It can leverage more mature HttpWebRequest from .NET framework, instead of UnityWebRequest
3. It can return value, unlike Unity coroutine that doesn't really care about returning a value
4. It can have multiple HTTP requests running with only a single worker thread

We use this custom coroutine system under the hood, so most of the time user don't need to care about it. Any HTTP call will be mostly executed in background thread, except a little bit at the end of its execution. Unity doesn't support manipulating Unity objects from other thread. So, for convenience, we have our own coroutine runner just to give back objects from HTTP call to the main thread.

![typical-sdk-call](http://www.plantuml.com/plantuml/png/XL9BQiCm4Dth57E1N22Bac93Kxe9YKlN4tbAXyYoAeq3wUahHuX4haeMnfhn-poUjGc2NgR1A64n1Dt5iKO3xvRb0eVs3Pvthz2WCSf586jsQ0LUSA36C5bHjoEidcR6-Wg_yZudguL7gp2-pYWqVrVncQ4TXbFBmQ7eB_9hE93jmTFtH46WHy4RguGcWQXmbwAdmg8arDqye-4VAYn1HjXstIulAq9R4VT1ntq2fDY9arOfIprutYz4L3d2gf9tJvhuJDafkD46IbOroN-xUznCIo5tKxhDvVXydjlwAvRkJJFwSHAsbDfPa1xNir1HvGZ5du_DtAk3HcEEg4_9fYgtYPccmVm2)



This is actually a stop-gap solution before we upgrade to Unity 2018 and .NET 4.x as target platform, where a much nicer async/await syntax is available in Unity.

### Easy to Use API

AccelByte SDK APIs uses continuation passing style for calling HTTP REST API, which pass a callback that can be used to return the value from a HTTP response. Typically, an HTTP call is abstracted as

```csharp
public void CallHttpRestApi(TReqParam1 param1, TReqParam2 param2, ResultCallback<TResponse> callback)
```

ResultCallback\<T\> is a delegate that accepts Result\<T\> as its parameter, while ResultCallback accepts Result. We use Result\<T\> type because we want to avoid exception in our code base, so we combine error code and returned value. The class Result\<T\> is generic class of T and have 3 properties:

1. Result\<T\>.IsError indicates that the result of a request is normal or has an error.
2. Result\<T\>.Error have error code and error message
3. Result\<T\>.Value have the type of T and only meaningful if Result\<T\>.IsError is false.

Because of this, making HTTP requests is as easy as calling a method.

### Automatically retry temporary server errors

Temporary server errors (HTTP status 5xx) will be automatically retried if it failed, with exponential backoff and some randomization.

### Access Token Management

AccelByte SDK will automatically handle user access tokens expiry, by calling refresh token to get new access token at about 70% - 90% near expiration time (randomly spread).

---

## User Management and Authorization

![iam-api](http://www.plantuml.com/plantuml/png/TP31ReCm38RlVGgEsw4li5FHgbMLs5gniPsjn81LS51iDcrFFw15WRRSuk_xRKVUY8BNjgvMIbfXb0dww1KO2goHDGAsKTngwJpzWPBZrP1mw_KGZHiAuD07Ut96JqDYRxk7g2SkHlm6DKe31czCR0oAVBWJSd2ZJ5n1_4LQzdK_u-7nHXsqH7-iG7Fdwpj-YzzaS6_lg0HQDbttJBbMG_tYlC4KSuzI1ffYGQBJlXtg5XG2o1eBYg5-HxQVw2adCzpqsTc97SplF9sYe1v1vCTvEu_Fr7HduS5UBRt0vTsNGCps-XS9yMdXkXbNotA7Ei9-X7HcWspRkle7)

For convenience, AccelByte maintains and keeps user credentials under the hood. Except for some user management cases (e.g. registration, reset password, and login itself), user needs to login before accessing backend services. Below are some examples on how to do common user management tasks in Unity SDK. User class holds user credentials (access token, refresh token, and userId) and will be used implicitly by other API to make requests.

- Registering a user account

    If a user use his/her email as an account, it has to be registered using CreateUserAccount function.

    Usage:

    ```csharp
    private static void OnRegister(Result<UserModel> result)
    {
        if (result.IsError)
        {
            Debug.Log("Register failed:", result.Error.Message);
        }
        else
        {
            Debug.Log("Register successful.");
        }
    }

    public static void Main(string[] args)
    {
        var user = AccelBytePlugin.GetUser();
        string email = "useremail@example.com";
        string password = "password";
        string displayName = "displayed";
        user.Register(email, password, displayName, OnRegister);
    }
    ```

- Login with username and password

    Login the user with their email account. User should insert their **email** and **password** as parameters. It'll return the login result for the callback.

    Usage:

    ```csharp
    static string email = "johnDoe@example.com";
    static string password = "password";

    public static void OnLogin(Result result)
    {
        if (result.IsError)
        {
            // show the login result
            Debug.Log("Login failed:" + result.IsError);
        }
        else
        {
            Debug.Log("Login successful");
        }
    }

    public static void Main(string[] args)
    {
        User user = AccelBytePlugin.GetUser();
        user.LoginWithUserName(userName, password, OnLogin);
    }
    ```

- Register / login other platform accounts

    AccelByte IAM supports login with other platform accounts (e.g. Google, Facebook, Twitch, Steam, Oculus). It requires **[PlatformType](../models/#platformtype)** and **platformToken** as parameters. Each platform has different type of platform token. Here's the simple list of **platformToken** for each platform.

    |PlatformType   |PlatformToken|
    |---|---|
    Steam|Please include a Steamworks SDK to generate authentication ticket. And then use it as platform token.
    Google|Open a link to allow user to login with Google account from a browser. After user successfully login with their Google account, browser will redirect the login page to another site and then obtain the **platformToken** from the URL form parameter.
    Facebook|Open a link to allow user to login with Facebook account from a browser. After user successfully login with their Facebook account, browser will redirect the login page to another site and then obtain the **platformToken** from the URL form parameter.
    Twitch|Open a link to allow user to login with Twitch account from a browser. After user successfully login with their Twitch account, browser will redirect the login page to another site and then obtain the **platformToken** from the URL form parameter.
    Oculus|Insert Oculus SDK to your project. Create a login interface in the game that ask user's Oculus email and password. Initialize the Oculus platform and then use the email and password to get userProof() then obtain the **nonce**. Use the **nonce** as platform token.

    Registering is done automatically on first login for third party platform account. When registering this way, the account created is called headless account, because it doesn't have username and password. It only relies to third party platform token to do login.

    Usage:

    ```csharp
    public static void OnLoginWithOtherPlatform(Result result)
    {
        // show the login with platform result
        Debug.Log(result.IsError);
    }

    public static void Main(string[] args)
    {
        User user = AccelBytePlugin.GetUser();

        //Example login with steam

        if (SteamManager.Initialized)
        {
            var ticket = new byte[1024];
            uint actualTicketLength;
            HAuthTicket ret = SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
            Array.Resize(ref ticket, (int) actualTicketLength);
            var sb = new StringBuilder();

            foreach (byte b in ticket)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            user.LoginWithOtherPlatform(PlatformType.Steam, sb.ToString(), OnLoginWithOtherPlatform);
        }
    }
    ```

- Register / login with device id

    Register / login with device id is very similar with login with other platform. User account created from this process is also a headless account. Login with DeviceId doesn't need any parameter.

    ```csharp
    public void LoginWithDeviceId(ResultCallback callback) {}
    ```

    Usage:

    ```csharp
    public static void OnLoginWithDeviceId(Result result)
    {
        // show the login with device id
        Debug.Log(result.IsError);
    }

    public static void Main(string[] args)
    {
        User user = AccelBytePlugin.GetUser();
        user.LoginWithDeviceId(OnLoginWithDeviceId);
    }

- Login With Launcher

    If you use AccelByte Launcher, the game will login into the same user as the user that login to Launcher.

    ```csharp
    public void LoginWithLauncher(ResultCallback callback) {}
    ```

    Usage:

    ```csharp
    public static void OnLoginWithLauncher(Result result)
    {
        // show the login with device id
        Debug.Log(result.IsError);
    }

    public static void Main(string[] args)
    {
        User user = AccelBytePlugin.GetUser();
        user.LoginWithLauncher(OnLoginWithDeviceId);
    }

- Logout
    Logout will removes user credentials from memory. User needs to re-login after logout.

    ```csharp
    public void Logout(){}
    ```

    Usage:

    ```csharp
    static User user = null;
    static string email = "johnDoe@example.com";
    static string password = "password";

    public static void OnLogin(Result result)
    {
        if (!result.IsError)
        {
            // show the login result
            Debug.Log("Login failed:" + result.IsError);
        }
        else
        {
            Debug.Log("Login successful");
        }
    }

    //Let's say logout button is clicked
    public static OnButtonLogoutClicked()
    {
        UserAccount user = AccelBytePlugin.GetUserAccount();
        user.Logout();
    }

    public static void Main(string[] args)
    {
        UserAccount user = AccelBytePlugin.GetUserAccount();
        user.LoginWithDeviceId(OnLogin);
    }
    ```

- Updating user

    User attributes such as displayName, languageTag, or country can be updated with Update method. This is particularly important for Platform services that needs user to have country attribute to purchase items.

    Usage:

    ```csharp
    static string email = "johnDoe@example.com";
    static string password = "freshPassword";

    public static void OnLogin(Result result)
    {
        if (!result.IsError)
        {
            var user = AccelBytePlugin.GetUser();
            var updateRequest = new UpdateUserRequest
            {
                Country = "en",
                DisplayName = "exampleDisplayName"
            };

            user.Update(userAccount, updateRequest, OnUpdate);
        }
        else
        {
            Debug.Log("Reset Password Failed.");
        }
    }

    public static void OnUpdate(Result<UserModel> result)
    {
        if (!result.IsError)
        {
            Debug.Log("User account has been updated.");
        }
        else
        {
            Debug.Log("User account update failed.");
        }
    }

    public static void Main(string[] args)
    {
        user.LoginWithUserName(email, password, OnLogin);
    }
    ```

- Verifying user accounts

    After a user account is created / registered, verifying it can be done in 3 steps:

    1. Trigger verification code to be sent to email used as username
    2. Login with username and password
    3. Verify the user account with verification code

    ```csharp
    public void Verify(UserAccount account, string verificationCode, ResultCallback callback) {}
    ```

    Usage:

    ```csharp
    private static void OnLogin(Result result)
    {
        var user = AccelBytePlugin.GetUser();
        user.SendVerificationCode(OnSendVerificationCode);
    }

    private static void OnSendVerificationCode(Result result)
    {
        if (!result.IsError)
        {
            //Verification code sent to email
        }
    }

    //Let's assume that user has inputted verification code somehow
    private static void OnVerifyClicked(object sender, string verificationCode)
    {
        var user = AccelBytePlugin.GetUser();

        user.Verify(userAccount, verificationCode, OnVerify);
    }

    private static void OnVerify(Result result)
    {
        if (result.IsError)
        {
            Debug.Log("Verify failed:", result.Error.Message);
        }
        else
        {
            Debug.Log("Verify successful. User verified");
        }
    }

    public static void Main(string[] args)
    {
        var user = AccelBytePlugin.GetUser();
        string email = "test@example.com";
        string password = "123456";
        user.LoginWithUserName(email, password, OnLogin);
    }
    ```

- Upgrading headless account to full account

    Headless accounts (accounts that doesn't have username and password, and must login by third party platform) can be upgraded into full account by adding username (e-mail) and password.

    Usage:

    ```csharp
    public static void OnLoginWithDevice(Result result)
    {
        if (!result.IsError)
        {
            var user = AccelBytePlugin.GetUser();
            var email = "johndoe@example.com";
            var password = "password";
            user.Upgrade(email, password, OnUpgrade);
        }
    }

    public static void OnUpgrade(Result<UserData> result)
    {
        Debug.Log(result.Value.LoginId);// "johndoe@example.com"
    }

    public static void Main(string[] args)
    {
        User user = AccelBytePlugin.GetUser();
        user.LoginWithDeviceId(OnLoginWithDeviceId);
    }

- Upgrading headless account with verification code

    Headless accounts can be upgraded **and** verified at the same time using this method.

    Usage:

    ```csharp
    public static void OnLoginWithDevice(Result result)
    {
        if (!result.IsError)
        {
            var user = AccelBytePlugin.GetUser();
            var email = "johndoe@example.com";
            user.SendUpgradeVerificationCode(email, OnSendUpgradeVerificationCode);
        }
    }

    public static void OnSendUpgradeVerificationCode(Result result)
    {
        if (!result.IsError)
        {
            //Verification code sent to email
        }
    }

    public static void OnUpgradeClick(string verificationCode)
    {
            var user = AccelBytePlugin.GetUser();
            var email = "johndoe@example.com";
            var password = "password";
            user.UpgradeAndVerify(email, password, verificationCode, OnUpgrade);
    }

    public static OnUpgrade(Result<UserData> result)
    {
        Debug.Log(result.Value);
    }

    public static void OnUpgrade(Result<UserData> result)
    {
        Debug.Log(result.Value.LoginId);// "johndoe@example.com"
    }

    public static void Main(string[] args)
    {
        User user = AccelBytePlugin.GetUser();
        user.LoginWithDeviceId(OnLoginWithDeviceId);
    }

- Resetting password

    Resetting password can be done in three steps:

    1. Trigger reset password code to be sent to email used as username
    2. Login with username and password
    3. Reset password with also giving reset password code

    Usage:

    ```csharp
    public static void OnSendResetPasswordCode(Result result)
    {
        if (!result.IsError)
        {
            //Reset password code code sent to email
        }
    }

    //Let's assume that user has inputted reset password code somehow
    public static void OnResetClicked(object sender, string resetPasswordCode)
    {
        var user = AccelBytePlugin.GetUser();

        user.ResetPassword(OnSendVerificationCode);
        string email = "useremail@example.com";
        user.Verify(resetPasswordCode, email, newPassword, OnResetPassword);
    }

    public static void OnResetPassword(Result result)
    {
        if (result.IsError)
        {
            Debug.Log("Reset password failed failed:", result.Error.Message);
        }
        else
        {
            Debug.Log("Reset password successful. Password has been changed.");
        }
    }

    public static void Main(string[] args)
    {
        var user = AccelBytePlugin.GetUser();
        string email = "useremail@example.com";
        user.SendResetPasswordCode(email, OnSendResetPasswordCode);
    }
    ```

---

## Basic Services

User must be logged in before he can use basic services.

### User Profiles

![user-profiles-api](http://www.plantuml.com/plantuml/png/bPJ1JeD048RlF0L7FV02FPXM3yR4g55xyJQ10JVEBhWpw_DBIPffop8fj_R_lndWssItB10bUIh42M6vPupbM-nRHyZ5z6ypXK_D-8Cbkj0TunVmo0FKE6jsTOtCd_qF1kscyhYwx2l2LgffzsLpI3NO3UuSgD9GtPqYkVeX8WXgWU_ucv0bksfeykmvl9allRczH1vHp5wVfKYnzB8Ztxh8CffMoHPDi0A6Fn287nw8zf5MZAoZdo5sUEs8E8zVVS2hu8F9r_RUHVbMlfWPDcKQD841sp9NZDYqBLD7R9acRCjKT4nJvYBGDkXza0DK_o90OFI6v1nb0QhIeD23MonPG18lVqVe4dIOx_LWylt2MMmnDCdJFm00)

The service stores user profiles at platform level, which means it can be accesed either from Launcher or from game client. 

- Getting user profile

    The profile should have created before. It doesn't require any parameter. It'll return the UserProfile from the callback result.

    ```csharp
    public void GetUserProfile(ResultCallback<UserProfile> callback){}
    ```

    Usage:

    ```csharp
    public static void Main(string[] args)
    {
        //user should be logged in first
        UserProfile user = AccelBytePlugin.GetUserProfiles();
        user.GetUserProfile(result => {
            // result type is Result<UserProfile>
            // result.Value type is UserProfile

            // showing the display name of the user
            Debug.Log(result.Value.displayName);
        });
    }
    ```

- Updating user profile

    The profile should have existed before we can update user profile.

    ```csharp
    public void UpdateUserProfile(UpdateUserProfileRequest request, ResultCallback<UserProfile> callback){}
    ```

    Usage:

    ```csharp
    public static void Main(string[] args)
    {
        UpdateUserProfileRequest request = new UpdateUserProfileRequest()
        {  
            firstName = "test",
            lastName = "unitysdk",
            dateOfBirth = "2001-01-01",
            avatarSmallUrl = "https://image.com/example/small.jpg",
            avatarUrl = "https://image.com/example/medium.jpg",
            avatarLargeUrl = "https://image.com/example/large.jpg",
            customAttributes = null,
            timeZone = "Asia/Shanghai"
        };

        UserProfile user = AccelBytePlugin.GetUserProfiles();
        user.UpdateUserProfile(request, result => {
            // result type is Result<UserProfile>
            // result.Value type is UserProfile

            // showing the display name of the user
            Debug.Log(result.Value.displayName);
        });
    }
    ```

- Creating user profile
  
    Create a user profile if a user doesn't have it before.

    ```csharp
    public void CreateUserProfile(CreateUserProfileRequest request, ResultCallback<UserProfile> callback){}
    ```

    Usage:

    ```csharp
    public static void Main(string[] args)
    {
        CreateProfileRequest request = new CreateProfileRequest()
        {  
            firstName = "test",
            lastName = "unitysdk",
            avatarSmallUrl = "https://image.com/example/small.jpg",
            avatarUrl = "https://image.com/example/medium.jpg",
            avatarLargeUrl = "https://image.com/example/large.jpg",
            dateOfBirth = "2001-01-01",
            timeZone = "Asia/Shanghai"
        };

        UserProfile user = AccelBytePlugin.GetUserProfiles();
        user.CreateUserProfile(request, result => {
            // result type is Result<UserProfile>
            // result.Value type is UserProfile

            // showing the display name of the user
            Debug.Log(result.Value.displayName);
        });
    }
    ```
---

## Platform Services

User should be logged in first before it can use platform service modules.

![platform-services](http://www.plantuml.com/plantuml/png/VP312e9054NtynM3UtyXb5Ok8Y6YXRGOd9T8CqwyUTOY_FUKIBk1RTyvtBbtiYY6fVTEIIkETEcCN09xVI2-jpL5LRlGM_rKrTO8DO5RYXzuLbKXvECiZuSzM-9lusIS8raEWreYYtm7u6Rggsb89nF8ooAPf6CaHZgN14MdDZowTACX5dgF_gBz6uJYsaYqrfQZzViTw_tUgx0H8ko135z7fWQLKMTxxru0)

### Catalog (Categories and Items)

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

- Get Category

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

- Get Root Categories

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

- Get Child Categories

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

- Get Descendant Categories

    Get every descendant category of a category. Required parameter is **categoryPath** and optional parameter is **language**.
    This will retrieve all category in a flat array that needs to be rearranged into proper category tree.

    <!--language lang-cs -->

        public void GetDescendantCategories(string categoryPath, string language, ResultCallback<Category[]> callback){}

    Usage:
    <!--language lang-cs -->

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

- Get Item by Id

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

- Get Items by Criteria

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
        items.GetItemsByCriteria("US", criteria, "en", result => {
            // result type is Result<PagedItems>
            // result.Value type is PagedItems
            // result.Value.data type is Item[]

            // showing the amount of items
            Debug.Log(result.Value.data.Length);
        });
    }
    ```

### Orders

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

- Create Order

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

- Fulfilling an order

    On order can be fulfilled by two methods:

    1. Cash, using Xsolla payment integration

        This method needs user to open web browser and input their payment details to complete order.

    2. Wallet (virtual currency)

        The order will be automatically fulfilled if a user has enough amount of virtual currency. To use this method, change the currency code to currency code for the virtual currency.

- Get User Order

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

- Get User Orders

    Get all of user's order that has been created. It has two optional parameters; **startPage** and **size**. It'll return a PagedOrderInfo from the result callback.

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

- Get User Order History

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

### Wallet

    Wallet stores in-game virtual currency to make purchases (create orders) on in-game items in in-game store.

- Get WalletInfo by Currency Code

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

---

## Social Services

![lobby-api](http://www.plantuml.com/plantuml/png/TP3DgeCm44RtynI3Uz_WSj5xIw7q1y6bk8ZZL8CnAN5i4V7TAxLGA6GvdCF7S991b9UnDo1Q3EF9LTK4GCQDDSxmEl4dhjt3nbXagpjXplwkYSjl-jyg2SBCyy2ME2ZilMPR3q5-SQAlcfHePuFIMGcrlUgqRJxE0d1JrAc6CKg9sFnDmfZlZX7EK6mBQNOeSLlvVKTO85aVMjCy0G00)

Lobby is a collection of services that are connected together through a websocket connection. Those services are:

1. Party Service
2. Chat Service
3. Friends Service
4. Presence Service
5. Notification Service
6. Matchmaking Service

We have to connect to Lobby first

```csharp
string userId
var lobby = AccelBytePlugin.GetLobby();
lobby.Connect();
```

before we start interfacing with those services

### Lobby Protocol

AccelByte Lobby Protocol closely follows RPC model and is described by its message format, which is a subset of YAML. The message is divided into two parts: header and payload. Header fields are type, id, and code (optional), while payload fields depends on the type.

Request Example:

```text
type: someRequest\n
id: id123\n
payloadFieldBool1: true\n
payloadFieldDouble2: 2.0\n
payloadFieldInt3: 3\n
payloadFieldStr4: some text message
```

Response code 0 means the request returned an OK response, while other code means the request returned an error response. Request and Response come in pair, so that a pair of request and response have the same id, while an Notification doesn't have an id.

Response OK Example:

```text
type: someResponse\n
id: id123\n
code: 0\n
payloadFieldBool1: true\n
payloadFieldDouble2: 2.0\n
payloadFieldInt3: 3\n
payloadFieldStr4: some text message\n
payloadFieldStrArray5: [item1,item2,item3,item4]
```

Response Error Example:

```text
type: someResponse\n
id: id123\n
code: 14777
```

Notification/Event Example:

```text
type: someNotif\n
payloadFieldBool1: true\n
payloadFieldStrArray5: [item1]
```

You don't need to worry about this protocol. AccelByte SDK abstracted Request-Response as method call, while Notification will look like a C# event.

### Party

A party can be created by any user, but a user can only create a single party. Party creator is the leader of the party, which means he/she can invite or kick another user. There is no limit on how many members can be in a party.

```csharp
[DataContract]
public class PartyInfo
{
    [DataMember] public string partyID;
    [DataMember] public string leaderID;
    [DataMember] public string[] members;
    [DataMember] public string[] invitees;
    [DataMember] public string invitationToken;
}
```

- Get party info

    Get an already created party info

    Usage:

    ```csharp
    public static void OnGetPartyInfo(Result<PartyInfo> result)
    {
        if (!result.IsError)
        {
            Debug.Log(result.Value.partyID);
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.GetPartyInfo(OnGetPartyInfo);
    }
    ```

- Creating a party

    A user can only create one party. An attempt to create a second party will fail.

    Usage:

    ```csharp
    public static void OnCreateParty(Result<PartyInfo> result)
    {
        if (!result.IsError)
        {
            Debug.Log(result.Value.partyID);
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.CreateParty(OnCreateParty);
    }

    ```
- Inviting another user to a party

    Party leader can invite another user to join his party.

    Usage:


    ```csharp
    public static void OnInvited(Result result)
    {
        if (!result.IsError)
        {
            Debug.Log("Invited a user to your party");
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.InviteToParty("anotherUserId", OnInvited);
    }
    ```

- Kick another user from a party

    Party leader can kick a member out of his party

    Usage:

    ```csharp
    public static void OnMemberKicked(Result result)
    {
        if (!result.IsError)
        {
            Debug.Log("Kicked a user from your party");
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.KickPartyMember("anotherUserId", OnMemberKicked);
    }
    ```

- Leaving a party

    Party member (including party leader) can leave a party he is in.

    Usage:

    ```csharp
    public static void OnLeaveParty(Result result)
    {
        if (!result.IsError)
        {
            Debug.Log("Leaved a party");
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.LeaveParty(OnLeaveParty);
    }
    ```

- Joining a party

    A user must be invited before he can join a party. He / she can only join a single party.

    Usage:

    ```csharp
    public static PartyInvitation partyInvitation;

    public static void OnInvitedToParty(Result<PartyInvitation> result)
    {
        if (!result.IsError)
        {
            partyInvitation = result.Value;
        }
    }

    public static void OnJoinPartyClicked()
    {
        var lobby = AccelBytePlugin.GetLobby();

        if (partyInvitation != null)
        {
            lobby.JoinParty(partyInvitation.partyId, partyInvitaion.invitationToken, OnJoinedParty);
        }
    }

    public static void OnJoinedParty(Result<PartyInfo> result)
    {
        if (!result.IsError)
        {
            Debug.Log("Joined party " + result.partyID);
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.InvitedToParty += OnInvitedToParty;
        lobby.Connect();
    }
    ```

- Party chat

    A user can send chat to all other user on his party.

    Usage:

    ```csharp
    public static void OnSendPartyChatClicked()
    {
        var lobby = AccelBytePlugin.GetLobby();

        lobby.SendPartyChat("Hi party members!!!", OnPartyChatSent);
    }

    public static void OnPartyChatSent(Result result)
    {
        Debug.Log(result.IsError);
    }

    public static void OnPartyChatReceived(Result<ChatMessage> result)
    {
        if (!result.IsError)
        {
            Debug.Log(result.Value.from + " : " + result.Value.payload);
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.PartyChatReceived += OnPartyChatReceived;
        lobby.Connect();
    }
    ```

### Personal chat

A user can chat directly another user with personal chat.

Usage:

```csharp
public static void OnSendChatClicked()
{
    var lobby = AccelBytePlugin.GetLobby();

    lobby.SendPersonalChat("anotherUserId", "Hi, how are you?", OnChatSent);
}

public static void OnChatSent(Result result)
{
    Debug.Log(result.IsError);
}

public static void OnChatReceived(Result<ChatMessage> result)
{
    if (!result.IsError)
    {
        Debug.Log(result.Value.from + " : " + result.Value.payload);
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.PersonalChatReceived += OnChatReceived;
    lobby.Connect();
}
```

### Notifications

Lobby can also be used to send notification to all user. To make user receive system notification, subscribe to OnNotification event. Use **PullAsyncNotifications** to trigger Lobby to send pending notifications while user is offline.

```csharp
public static void OnChatSent(Result<MessageNotification> result)
{
    Debug.Log(result.IsError);
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.OnNotification += OnReceiveNotification;
    lobby.PullAsyncNotifications();
    lobby.Connect();
}
```

### Friends

For a user to make friends with other users, he has to know other user id.

- Requesting other user to be friend

    The first step in making friend is to request other user to be friend.

    Usage:

    ```csharp
    public static void OnRequestFriend(Result result)
    {
        Debug.Log(result.IsError);
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.RequestFriend("otherUserId", OnRequestFriend);
    }
    ```

- Accepting / rejecting a friend request

    To complete a friend request, the other should accept / reject the request.

    Usage:

     ```csharp
    public static void CompleteFriendRequest(bool accept, string userId)
    {
        var lobby = AccelBytePlugin.GetLobby();

        if (accept)
        {
            lobby.AcceptFriend(userId, OnAcceptFriend);
        }
        else
        {
            lobby.RejectFriend(userId, OnRejectFriend);
        }
    }

    public static OnAcceptFriend(Result result)
    {
        Debug.Log("Friend accepted");
    }

    public static OnAcceptFriend(Result result)
    {
        Debug.Log("Friend rejected");
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
    }
    ```

- Getting list of friends

    User can get list of other users who are already friend.

    Usage:

    ```csharp
    public static OnLoadFriendsList(Result<FriendList> result)
    {
        if (!result.IsError)
        {
            foreach (string userId in result.Value.friendsId)
            {
                Debug.Log(userId);
            }
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.LoadFriendsList(OnLoadFriendsList);
    }
    ```

- Getting list of incoming friends

    User can get list of incoming friends (other users who has sent friend request to him/her)

    Usage:

    ```csharp
    public static OnListIncomingFriends(Result<ListIncomingFriends> result)
    {
        if (!result.IsError)
        {
            foreach (string userId in result.Value.friendsId)
            {
                Debug.Log(userId);
            }
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.ListIncomingFriends(OnListIncomingFriends);
    }
    ```

- Getting list of outgoing friends

    User can  get list of outgoing friends (other users who has not accepted his/her friend request)

    Usage:

    ```csharp
    public static OnListOutgoingFriends(Result<ListOutgoingFriends> result)
    {
        if (!result.IsError)
        {
            foreach (string userId in result.Value.friendsId)
            {
                Debug.Log(userId);
            }
        }
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.ListOutgoingFriends(OnListOutgoingFriends);
    }
    ```

- Friends status

    User can set his status and also can see his friends status.

    Usage:

    ```csharp
    public static OnListFriendsStatus(Result<FriendsStatus> result)
    {
        if (!result.IsError)
        {
            for (int i = 0; i < result.Value.friendsId.Length; i++)
            {
                Debug.Log(resut.Value.friendsId[i]);
                Debug.Log(resut.Value.availability[i]);
                Debug.Log(resut.Value.activity[i]);
            }

        }
    }

    public static OnChangeStatus(UserStatus status, string activity)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.SetUserStatus(status, activity, OnSetUserStatus);
    }

    public static OnSetUserStatus(Result result)
    {
        Debug.Log("Status changed");
    }

    public static OnUserStatusNotification(Result<UserStatusNotif> result)
    {
        Debug.Log(resut.Value.friendsId);
        Debug.Log(resut.Value.availability);
        Debug.Log(resut.Value.activity);
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.UserStatusNotification += OnUserStatusNotification;
        lobby.Connect();
        lobby.ListFriendsStatus(OnListFriendsStatus);
    }
    ```
- Unfriend a user

    User can remove another user from their friends list by unfriending him / her

    Usage:

    ```csharp
    public static void OnUnfriend(Result result)
    {
        Debug.Log(result.IsError);
    }

    public static void Main(string[] args)
    {
        var lobby = AccelBytePlugin.GetLobby();
        lobby.Connect();
        lobby.Unfriend("otherUserId", OnUnfriend);
    }
    ```

### Matchmaking

Party leader can start matchmaking. While waiting for matchmaking process, he/she can also cancel matchmaking. If match is found, a notification sent to all party members that matchmaking is done.

Usage:

```csharp
public static void OnMatchMakingNotification(Result<MatchmakingNOtif> result)
{
    if (!result.IsError)
    {
        if (result.Value.status == "done")
        {
            Debug.Log("Matchmaking done. Match found");
        }
        else if (result.Value.status == "cancel")
        {
            Debug.Log("Matchmaking cancelled.");
        }
    }
}

public static void StartMatchMaking()
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.StartMatchmaking("game-mode", OnStartMatchMaking);
}

public void OnStartMatchMaking(Result result)
{
    Debug.Log(result.IsError)
}

public static void CancelMatchMaking()
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.CancelMatchmaking("game-mode", OnCancelMatchmaking);
}

public void OnCancelMatchmaking(Result result)
{
    Debug.Log(result.IsError)
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.MatchmakingNotification += OnMatchMakingNotification;
    lobby.Connect();
}
```
