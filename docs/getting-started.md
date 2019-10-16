
# Getting Started

## Install The Plugin

### From Source Code

1) Download the whole repository.

2) Create a file named **AccelByteSDKConfig.json** and add it to *Assets\Resources* directory. Fill the _PublisherNamespace_, _ClientId_, _ClientSecret_, _Namespace_, _BaseUrl_, and _RedirectUri_, _UseSessionManagement_, and all the services according to your own. Here's the content of the json file:

    ```json
    {
        "ClientId":"<GAME_CLIENT_ID>",
        "ClientSecret":"<GAME_CLIENT_SECRET>",
        "UseSessionManagement": true,
        "PublisherNamespace":"<YOUR-PUBLISHER-NAMESPACE>",
        "Namespace":"<YOUR-GAME-NAMESPACE>",
        "BaseUrl": "<SERVER-BASE-URL>",
        "LoginServerUrl":"<AUTH-SERVER-URL>",
        "IamServerUrl":"<IAM-SERVER-URL>",
        "PlatformServerUrl":"<PLATFORM-SERVER-URL>",
        "BasicServerUrl":"<BASIC-SERVER-URL>",
        "LobbyServerUrl":"<LOBBY-SERVER-URL>",
        "TelemetryServerUrl":"<TELEMETRY-SERVER-URL>",
        "RedirectUri":"http://localhost",
        "CloudStorageServerUrl":"<CLOUDSTORAGE-SERVER-URL>",
        "GameProfileServerUrl":"<GAMEPROFILE-SERVER-URL>",
        "StatisticServerUrl": "<STATISTIC-SERVER-URL>"
    }
    ```

    ⚠If a game released without a publisher, everything is done in the game namespace. You can leave PublisherNamespace in the config file empty or null.

3) Copy whole *Assets*/**AccelByte** folder from the repository that has been downloaded and add it to your project in the *Assets* directory.

4) If you need other functionalities, e.g. Steamworks, Xsolla, Oculus, embedded browser; you can include this whole repository to your project because we already provide those plugin/package here.

### From Package

1) Download the unity package.

2) Open your unity editor and open your project. Go to _Assets_ > _Import Package_  on menu bar and choose _Custom Package..._ Search for the package you've downloaded on _step 1_ and choose _Open_.

3) Now just click on _All_ button to choose all the components. You can exclude some _Api_ if you don't really use it, like _CloudStorage_, _Telemetry_ etc. After that, just click *Import* and the editor will install it for you.

4) Now go to *Assets\Resources* and search for a file named **AccelByteSDKConfig.json**. Fill the _PublisherNamespace_, _ClientId_, _ClientSecret_, _Namespace_, _BaseUrl_, and _RedirectUri_, _UseSessionManagement_, and all the services according to your own. Here's the content of the json file:

    ```json
    {
        "ClientId":"<GAME_CLIENT_ID>",
        "ClientSecret":"<GAME_CLIENT_SECRET>",
        "UseSessionManagement": true,
        "PublisherNamespace":"<YOUR-PUBLISHER-NAMESPACE>",
        "Namespace":"<YOUR-GAME-NAMESPACE>",
        "BaseUrl": "<SERVER-BASE-URL>",
        "LoginServerUrl":"<AUTH-SERVER-URL>",
        "IamServerUrl":"<IAM-SERVER-URL>",
        "PlatformServerUrl":"<PLATFORM-SERVER-URL>",
        "BasicServerUrl":"<BASIC-SERVER-URL>",
        "LobbyServerUrl":"<LOBBY-SERVER-URL>",
        "TelemetryServerUrl":"<TELEMETRY-SERVER-URL>",
        "RedirectUri":"http://localhost",
        "CloudStorageServerUrl":"<CLOUDSTORAGE-SERVER-URL>",
        "GameProfileServerUrl":"<GAMEPROFILE-SERVER-URL>",
        "StatisticServerUrl": "<STATISTIC-SERVER-URL>"
    }
    ```

    ⚠If a game released without a publisher, everything is done in the game namespace. You can leave PublisherNamespace in the config file empty or null.

5) Your SDK is ready to use.

## Using The SDK

At first, you need to import AccelByte namespaces:

```csharp
using AccelByte.Api;
using AccelByte.Models;
using AccelByte.Core;
```

## Make Your First API Call

### Register / login with device id

AccelByte provides an easy way to authenticate players. You can use login with device ID (Headless Account) to make it easy for first time players as they do not need to submit any information. This tutorial below shows how to Login with DeviceId that does not require any parameter.

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
```

### Upgrading headless account to full account

Headless accounts (accounts that doesn't have username and password) can be upgraded into full account by adding username (e-mail) and password. The sample below shows the implementation to upgrade existing headless account into full account.

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
```

### Creating user profile

After upgrading headless account into full account, players can create their user profile.

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

### Getting user profile

Players can get their created profile previously using below example.

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

### Updating user profile

For most implementation, players will not be required to enter their profile and can update their profile later from the provided profile page. The example below shows how to implement user profile update.

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

To check out our complete tutorials, please see the [Tutorial Page](tutorial/tutorial.md)
