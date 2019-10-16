
## User Management and Authorization

![iam-api](http://www.plantuml.com/plantuml/png/TP31JeD048RlVGgEUkWBU2gnITC4RLD5pvFTlp0fp9ATGQDFBs08aBOtvli-PfFPg56qjgvMnbOXoHKnUI6X0ZN44XAVfAsL8V8l6GTP3KLOhuSqRNW2XpxioJzw6egzxpxK8ainubUgeGqvN9dKEQY-XmjaSgFCGi7ooLRsSx_ZuV5A7Qn4F5Pavn3h6twBBtRO-nigerPSz_T2XTQc-OI2rUR53hObemdaZIV3DBz1_0SYdzaDrtiEVcO7yxEiJr3OaLI_G_JBVA7cRnQHvbt6ShbpY5og-t876s6_GZkpWVYshlu0)

For your convenience, AccelByte maintains and keeps user credentials under the hood. Except, for some user management cases (e.g. registration, reset password, and login itself), user needs to login before accessing backend services. Below are some examples on how to do common user management tasks in Unity SDK. User class holds user credentials (access token, refresh token, and userId) and will be used implicitly by other API to make requests.

## Registering a user account

If a user use his/her email as an account, it has to be registered using CreateUserAccount function.

Usage:

```csharp
private static void OnRegister(Result<UserData> result)
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
    string country = "US";
    string dateOfBirth = "1995-12-30";
    user.Register(email, password, displayName, OnRegister, country, dateOfBirth);
}
```

## Login with username and password

Login the user with their email account. User should insert their **email** and **password** as parameters. It'll return the login result for the callback.

Usage:

```csharp
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

public static void Main(string[] args)
{
    User user = AccelBytePlugin.GetUser();
    user.LoginWithUserName(userName, password, OnLogin);
}
```

## Register / login other platform accounts

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

## Register / login with device id

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
```

## Login With Launcher

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
    user.LoginWithLauncher(OnLoginWithLauncher);
}
```

## Logout
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

## Updating user

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

        user.Update(updateRequest, OnUpdate);
    }
    else
    {
        Debug.Log("Reset Password Failed.");
    }
}

public static void OnUpdate(Result<UserData> result)
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

## Verifying user accounts

After a user account is created / registered, verifying it can be done in 3 steps:

1. Trigger verification code to be sent to email used as username
2. Login with username and password
3. Verify the user account with verification code

```csharp
public void Verify(string verificationCode, ResultCallback callback) {}
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

    user.Verify(verificationCode, OnVerify);
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

## Upgrading headless account to full account

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
```

## Resetting password

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

    string email = "useremail@example.com";
    user.ResetPassword(resetPasswordCode, email, newPassword, OnResetPassword);
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