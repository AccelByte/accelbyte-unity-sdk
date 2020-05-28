# 2.10.0 (2020-05-26)
## Added
- User
    - Bulk Get User Id by Third Party Platform
- Lobby
    - Add LobbyApi
    - Add Bulk Friend Request

# 2.9.0 (2020-04-14)
## Added
- UserProfile Api
    - GetCustomAttributes
    - UpdateCustomAttributes

# 2.8.0 (2020-04-09)
## Added
- Session Management
    - Add automatic refresh session coroutine
- DedicatedServerManager
    - Implement register static ds
    - implement get regional dsm url
    - Add errorRetry param on ConfigureHeartBeat
    - Will stop sending heartbeat after errorRetry number reached
- ServerQos
    - Get latencies from game server

### Changed
- Cloudstorage model dateAccessed, dateCreated, dateModified type from float to DateTime

### Fixed
- Lobby
    - Latencies format when startmatchmaking
- ServerModels
    - MatchingAlly
        - string partyId
        - string[] partyMember -> MatchParty[] matching_parties
- Jenkins pipeline
    - Fix tests

# 2.7.0 (2020-03-26)
## Added
- Session Management
    - Add automatic refresh session coroutine

# 2.6.0 (2020-03-03)
## Added
- Statistic
  - CreateUserStatItems
  - GetUserStatItems
  - IncrementUserStatItems
- Entitlement
    - Get user entitlement by its entitlement id.
    - Consume user entitlement.
- ServerEcommerce
	- Credit user wallet.
	- Grant user entitlement.
    - Get user entitlement by its entitlement id.
- Ecommerce Model:
    - Enum CreditUserWalletSource
    - ConsumeUserEntitlementRequest
    - CreditUserWalletRequest
    - GrantUserEntitlementRequest
    - StackableEntitlementInfo
- QosManager
  - Get Qos Servers
- Qos
  - Get Server Latencies

## Fixed
- Lobby
  - Stop reconnecting when lobby server disconnects client because token revoked, expired or another session for the same user connecting
- Integration Test
  - Fix user test

## Changed
- Statistic API Standardization
  - Model:
    - StatItemInfo into StatItem
    - BulkUserStatItemInc into UserStatItemIncrement
    - StatInfo into StatConfig
    - StatItemIncResult into CreateStatItemRequest
      - Data Member delete: currentValue
      - Data Member add: statCode
    - StatItemPagingSlicedResult into PagedStatItems
    - BulkStatItemInc into StatItemIncrement
    - BulkStatItemOperationResult into StatItemOperationResult
      - Data Member change: detail -> details
    - StatItemInfo
      - Add member public string[] tags
- User
  - GetUserByEmailAddress into SearchUsers
    - parameter: string emailadress -> string emailOrDisplayName
  - UnlinkOtherPlatform
    - parameter: string platformId -> PlatformType platformType
  - LinkOtherPlatform
    - parameter: string platformId -> PlatformType platformType
- Entitlement
  - Change class name from entitlements into entitlement

# 2.5.0 (2019-11-25)
## Fixed
- Platform
  - Models
    - Change sealed class to enum
    - Added enum:
      - OrderStatus
      - EntitlementSource
    - Refactor some class name and fix some class members:
      - WalletTransaction -> WalletTransactionInfo
      - PagedWalletTransactions -> WalletTransactionPagingSlicedResult
      - WalletInfo
        - string status -> ItemStatus status
      - Category -> CategoryInfo
        - childCategories -> Deleted
      - RegionData -> RegionDataItem
        - int totalNum -> Deleted
        - int totalNumPerAccount -> Deleted
        - int discountTotalNum -> Deleted
        - int discountTotalNumPerAccount -> Deleted
      - ItemCriteria
        - EntitlementAppType AppType -> Added
        - string Region -> Added
        - string Language -> Added
        - string BaseAppId -> Added
        - string[] Tags -> Added
        - ItemStatus ItemStatus -> Deleted
        - int? Page -> int? Offset
        - int? Size -> int? Limit
      - Item -> ItemInfo
        - Image thumbnailImage -> Deleted
        - string thumbnailUrl -> Added
        - bool stackable -> Added
        - string status -> ItemStatus status
        - string baseAppId -> Added
        - string name -> Added
        - string targetNamespace -> Added
        - string targetItemId -> Added
        - int maxCountPerUser -> Added
        - int maxCount -> Added
      - ItemSnapshot
        - string baseAppId -> Added
        - EntitlementType entitlementType -> Added
        - bool stackable -> Added
        - string thumbnailUrl -> Added
        - Image thumbnailImage -> Deleted
        - string targetItemId -> Added
        - string boothName -> Added
        - string clazz -> Added
        - string boothName -> Added
        - int displayOrder -> Added
        - string ext -> Added
        - string region -> Added
        - string language -> Added
        - string localExt -> Added
      - Image
        - string As -> Added
        - string caption -> Added
      - PagedItems -> ItemPagingSlicedResult
      - PagedOrderInfo -> OrderPagingSlicedResult
      - Entitlement -> EntitlementInfo
        - EntitlementSource source -> Added
      - PagedEntitlements -> EntitlementPagingSlicedResult
      - OrderHistoryInfo
        - string Namespace -> Added
      - OrderInfo
        - string paymentOrderNo -> Added
        - string paymentProvider -> Added
        - PaymentUrl -> paymentUrl -> Deleted
        - ItemSnapshot itemSnapshot -> Added
        - OrderTransaction[] transactions -> Deleted
        - string entitlementIds -> Deleted
        - string region -> Added
        - string language -> Added
        - string status -> OrderStatus status
        - DateTime chargebackTime -> Added
        - DateTime chargebackReversedTime -> Added
        - DateTime createdAt -> Added
        - DateTime updatedAt -> Added
      - OrderRequest
        - string language -> Added
    - Added class:
      - PopulatedItemInfo
## Changed
- Rearrange Folder
  - UnitySampleProject folder removed
  - All files and folders in UnitySampleProject has been moved to root folder
- Statistic API Standardization
  - Model:
    - StatItemInfo
      - Data Member change: profileId -> userId
    - BulkUserStatItemInc
      - Data Member change: profileId -> userId
- Delete Jenkinsfile function to create doxygen document
## Added
- Log
  - Add HTTP Request/Response Log
  - Add Current Function Log
- Release
  - Add Release folder for Unity SDK Package (in GitHub)

# 2.4.0 (2019-11-11)
## Changed
- Statistic API Standardization
  - API:
    - GetAllStatItems() & GetStatItemsByStatCodes() -> GetUserStatItems()
      - URL change: `GET` "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems" -> `GET` "/v1/public/namespaces/{namespace}/users/{userId}/statitems"
    - BulkAddStatItemValue()
      - URL change: `POST` "/public/namespaces/{namespace}/statitems/bulk/inc" -> `PUT` "/v1/public/namespaces/{namespace}/statitems/value/bulk"
    - BulkAddUserStatItemValue()
      - URL change: `POST` "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems/bulk/inc" -> `PUT` "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk"
    - AddUserStatItemValue()
      - URL change: `POST` "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/stats/{statCode}/statitems/inc" -> `PUT` "/v1/public/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value"
  - Model:
    - StatItemInfo
      - Add member public string[] tags
#Added
- Statistic
  - Add GetUserStatItemsByTags()
    - URL: `GET` "/v1/public/namespaces/{namespace}/users/{userId}/statitems"
    - Query param: "tags"
#Fixed
- Basic
  - Follow API Standardization
    - Endpoint Url:
        - UserProfilesApi.CreateUserProfile() "/basic/public/namespaces/{namespace}/users/me/profiles" -> "/basic/v1/public/namespaces/{namespace}/users/me/profiles"
        - UserProfilesApi.GetUserProfilePublicInfo() "/basic/public/namespaces/{namespace}/users/{userId}/profiles/public" -> "/basic/v1/public/namespaces/{namespace}/users/{userId}/profiles/public"
        - UserProfilesApi.UpdateUserProfile() "/basic/public/namespaces/{namespace}/users/me/profiles" -> "/basic/v1/public/namespaces/{namespace}/users/me/profiles"
        - UserProfilesApi.GetUserProfile() "/basic/public/namespaces/{namespace}/users/me/profiles" -> "/basic/v1/public/namespaces/{namespace}/users/me/profiles"
        - UserProfilesApi.GetUserProfilePublicInfosByIds "/basic/public/namespaces/{namespace}/profiles/public" -> "/basic/v1/public/namespaces/{namespace}/profiles/public"
        - UserProfilesApi.GetTimeZones "/basic/public/namespaces/{namespace}/misc/timezones" -> "/basic/v1/public/namespaces/{namespace}/misc/timezones"
  - Cleanup user profile on integration test

# 2.3.0 (2019-10-15)
## Added

- Enum members for IAM error handling

## Fixed

- Jenkins Automation
  - Adding Order param on Ecommerce Test
  - Fix set proxy on jenkins file
  - Add delay for steam to startup on jenkins file
  - Ignore some test on RetryTest because can't be done on automated machine
  - Change virtual coinCode from "SdkCoin" to "SDKC"
  - Adding "sortBy" param for GetItemByCriteria endpoint
  - Adding set proxy on Lobby WebSocket test

## Changed

- Remove intermediate conversion from UTF8 to .NET string before converting to object

## Breaking changes

- IAM API Standardization
  - UserAccount.Register():
    - Result type change: `UserData` -> `RegisterUserResponse`, incompatible fields
  - UserAccount.GetData():
    - URL change: `GET /v2/public/namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Update():
    - URL change: `PATCH /v2/public/namespaces/{namespace}/users/{userId}` -> `PATCH /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Upgrade():
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/headless/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/headless/verify`
  - UserAccount.SendVerificationCode()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/request` -> `POST /v3/public/namespaces/{namespace}/users/me/code/request`
  - UserAccount.Verify()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/code/verify`
  - UserAccount.SendPasswordResetCode()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/forgotPassword` -> `POST /v3/public/namespaces/{namespace}/users/forgot"`
    - Request Body change: JSON structure changed incompatibly
  - UserAccount.ResetPassword()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/resetPassword` -> `POST /v3/public/namespaces/{namespace}/users/reset`
  - UserAccount.LinkOtherPlatform()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `POST /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.UnlinkOtherPlatform()
    - URL change: `DELETE /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `DELETE /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.GetPlatformLinks()
    - Result type change: `PlatformLink[]` -> `PagedPlatformLinks`, add paging and change fields naming to camelCase
  - UserAccount.GetUserByLoginId()
    - Signature change: `GetUserByLoginId(string loginId, ResultCallback<UserData> callback)` -> `GetUserByEmailAddress(string emailAdress, ResultCallback<PagedPublicUsersInfo> callback)`
    - URL change: `GET /namespaces/{namespace}/users/byLoginId` -> `GET /v3/public/namespaces/{namespace}/users`
    - Request Body change: add query param `query` with value = email address
    - Result type change: `UserData` -> `PagedPublicUsersInfo`, incompatible fields with paging
  - UserAccount.GetUserByUserId()
    - URL change: `GET /namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/{userId}`

# v2.3.0 (2019-10-15)

## Added

- Enum members for IAM error handling

## Fixed

- Jenkins Automation
  - Adding Order param on Ecommerce Test
  - Fix set proxy on jenkins file
  - Add delay for steam to startup on jenkins file
  - Ignore some test on RetryTest because can't be done on automated machine
  - Change virtual coinCode from "SdkCoin" to "SDKC"
  - Adding "sortBy" param for GetItemByCriteria endpoint
  - Adding set proxy on Lobby WebSocket test

## Changed

- Remove intermediate conversion from UTF8 to .NET string before converting to object

## Breaking changes

- IAM API Standardization
  - UserAccount.Register():
    - Result type change: `UserData` -> `RegisterUserResponse`, incompatible fields
  - UserAccount.GetData():
    - URL change: `GET /v2/public/namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Update():
    - URL change: `PATCH /v2/public/namespaces/{namespace}/users/{userId}` -> `PATCH /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Upgrade():
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/headless/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/headless/verify`
  - UserAccount.SendVerificationCode()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/request` -> `POST /v3/public/namespaces/{namespace}/users/me/code/request`
  - UserAccount.Verify()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/code/verify`
  - UserAccount.SendPasswordResetCode()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/forgotPassword` -> `POST /v3/public/namespaces/{namespace}/users/forgot"`
    - Request Body change: JSON structure changed incompatibly
  - UserAccount.ResetPassword()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/resetPassword` -> `POST /v3/public/namespaces/{namespace}/users/reset`
  - UserAccount.LinkOtherPlatform()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `POST /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.UnlinkOtherPlatform()
    - URL change: `DELETE /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `DELETE /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.GetPlatformLinks()
    - Result type change: `PlatformLink[]` -> `PagedPlatformLinks`, add paging and change fields naming to camelCase
  - UserAccount.GetUserByLoginId()
    - Signature change: `GetUserByLoginId(string loginId, ResultCallback<UserData> callback)` -> `GetUserByEmailAddress(string emailAdress, ResultCallback<PagedPublicUsersInfo> callback)`
    - URL change: `GET /namespaces/{namespace}/users/byLoginId` -> `GET /v3/public/namespaces/{namespace}/users`
    - Request Body change: add query param `query` with value = email address
    - Result type change: `UserData` -> `PagedPublicUsersInfo`, incompatible fields with paging
  - UserAccount.GetUserByUserId()
    - URL change: `GET /namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/{userId}`

# v2.0.0 (2019-08-19)
## Added
- Session Management
  - Implementing API Gateway
  - Add Switchable Session Management
- Supported Game Profile Service Features
  - Create Game Profile
  - Get Game Profile
  - Update Game Profile
  - Delete Game Profile
- Supported Statistic Service Features
  - Get StatItem
  - Update StatItem from Client
- Supported Lobby Service Features
  - Ready Consent
  - DS Notif 

## Changed
- Coroutine
  - Change from Custom Coroutine to Unity Coroutine
- Updated to v2.0.0 for Justice release v2.0.0

# v1.0.0-beta.1 (2019-03-13)
## Added
- Supported IAM Service Features:
  - Login With Username and Password
  - Login With Other Platforms (Google, Facebook, and Steam)
  - Login With AccelByte Launcher
  - Login With Device ID
  - User Registration
  - User Verification (via email)
  - Upgrade device account and other platform account to proper AccelByte account with username and password
  - Update User Data
- Supported Basic Service Features
  - Create User Profile
  - Get User Profile
  - Update User Profile
  - Delete User Profile
- Supported Platform and E-Commerce Service Features:
  - Get Wallet Info
  - Create Order
  - Get Entitlements
  - Browse catalog (categories and items)
- Supported Lobby Service Features
  - Private Chat
  - Party
  - Friends
  - Matchmaking
  - Notification
