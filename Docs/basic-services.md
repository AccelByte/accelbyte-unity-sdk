
## Basic Services

User must be logged in before he can use basic services.

### User Profiles

![user-profiles-api](http://www.plantuml.com/plantuml/png/bPJ1JeD048RlF0L7FV02FPXM3yR4g55xyJQ10JVEBhWpw_DBIPffop8fj_R_lndWssItB10bUIh42M6vPupbM-nRHyZ5z6ypXK_D-8Cbkj0TunVmo0FKE6jsTOtCd_qF1kscyhYwx2l2LgffzsLpI3NO3UuSgD9GtPqYkVeX8WXgWU_ucv0bksfeykmvl9allRczH1vHp5wVfKYnzB8Ztxh8CffMoHPDi0A6Fn287nw8zf5MZAoZdo5sUEs8E8zVVS2hu8F9r_RUHVbMlfWPDcKQD841sp9NZDYqBLD7R9acRCjKT4nJvYBGDkXza0DK_o90OFI6v1nb0QhIeD23MonPG18lVqVe4dIOx_LWylt2MMmnDCdJFm00)

The service stores user profiles at platform level, which means it can be accesed either from Launcher or from game client.

## Getting user profile

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

## Creating user profile

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

## Updating user profile

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

---