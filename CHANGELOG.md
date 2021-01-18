# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [2.22.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.22.0%0D2.21.0) (2020-10-28)


### Features

* **leaderboard:** get leaderboard list ([384495f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/384495f666fa22dde2be1e8f8367b9cec42a6714))


### Bug Fixes

* **dsm:** temporary disabling dsm-related test ([99485e6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/99485e668558e876e6e11e65fee6bc1558048faa))


### Tests

* **Daily Test:** SDET-1166 Run SDK Automation Test Daily and Send report to slack channel ([acb8033](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/acb80338e38dd0961af50f5d6240a8835d83d1ec))

## [2.21.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.21.0%0D2.20.0) (2020-10-14)


### Features

* **lobby:** block and unblock player functions ([1be94dc](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1be94dc8d1b4e278f64d38a1fb4c8a6ac5cfd3cb))
* **lobby:** reject party invitation ([c4c9bae](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c4c9bae3ec9eb09f9283ad6b60aaec9b2df0f313))

## [2.20.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.20.0%0D2.19.0) (2020-10-01)


### Features

* add cloudsave API to Server SDK ([739b2c9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/739b2c97d6d2e88c82a4d9588a2d94b935a599e4))
* **leaderboard:** add additionalKey and additionalData for character leaderboard ([7a25033](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7a25033b70dc74c8560c82682a488739c01e4ac2))
* **lobby:** party update notif & its REST APIs ([defdd4b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/defdd4b81b983e39386c613ab1ddd330a5bde3cc))
* **serverLobby:** change WritePartyStorage signature to prevent overwrite ([fae5501](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fae550184a89ff28bbde5b65a869c8be362063be))


### Bug Fixes

* **plugin:** hotifx add CheckPlugin() on GetConfig() and set the order of bool setter in the CheckPlugin ([f496158](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f49615833140a45f324280015f1e2c1bc818663e))
* **plugin:** plugin failure on Unity Editor disable domain/scene reload ([d18bb42](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/d18bb426b4e334728ebb83ae3b03e7c1cd195d3f))


### Tests

* **sub:** fixed sub test by changing user ([59eb188](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/59eb18818c74c85db59da9f292e2c2fe431d47d0))


### Refactors

* **search:** add filter param in search users ([72bd941](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/72bd941945f8a40f73739bee37ef2d24caf9073f))

## [2.19.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.19.0%0D2.18.0) (2020-09-16)


### Features

* **lobby:** add global chat implementation ([e9fe2fc](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e9fe2fc331fcb57ecf5a8946335ab9041f16df96))
* **statistic:** add additionalKey and additionalData to update statItem endpoint ([0e86053](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0e860539fb41c165b2b95d95974bf2eadcde25c7))
* **statistic:** add reset and update statistic ([c09c301](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c09c30132205d14c32cffeb7873dfa88284f8942))
* **subscription:** check subscription in SKD changed flow to not check sub endpoints. ([bb84530](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bb845304b40af4be7bcf67b74d108639be7dc5bf))
* **Subscription:** update using the new flow & API endpoint ([95af021](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/95af02132b8122b4ef65957b2e3e05bb6b84ce12))
* **subscriptionTest:** integration test ([b3475b0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b3475b0c75feca18ff4495427ea8f1d5bc7ef344))
* **user:** register and upgrade with username param ([38d3ddd](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/38d3ddde989545e8bc1f71adb19e4009bc0b7ea7))


### Bug Fixes

* **subscription:** added flag when final result is found before the end to make it more consistent ([5b7bb0d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5b7bb0d7f3ad84e081b1b7457250d8a57b0f0ab2))
* **subscription:** changed API endpoint so it can check publisher namespace using game namespace token ([2c97d5c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2c97d5ce5f75a610e3209002af7d995904eb23f8))
* **Subscription:** added Publisher Namespace and Changed label "Store App ID" to "App Id" in accelbytePlatrofmSettingEditor ([6f83873](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/6f838734a6091052da05b2121463a8f9792fa0ac))
* **Subscription:** added publisher namespace in AccelbyteSettings ([b29392f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b29392f76fb11823aab9b46e4d83b0d0060124b4))
* **Subscription:** changed accessing config from AccelbyteSettings to AccelbytePlugin.Config ([0a60df3](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0a60df3c0230cf313637f856d00abd984ee5cd7a))
* **Subscription:** changed sub test name to SubscriptionTest ([fb69c30](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fb69c30b07af2f0b4aa732e778f57196f8a47bf7))
* **Subscription:** some endpoints need to be accessed with publisher namespace (get entitlements by appId, sku and getItems by appid) ([7677110](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7677110e43deaf4e37216de3e1825f02ccb28217))


### Code Style Fixes

* **Subscription:** added newline before callback parameter to keep style inline with other functions. ([4c697da](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4c697da9312a9ea50b19bea8c90ea59921523948))


### Tests

* **subcription:** changed appId to static to comply to Jenkins Test ([375153f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/375153fadd6d2a950259d2177bf21dedca07eefb))

## [2.18.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.18.0%0D2.17.0) (2020-09-02)


### Features

* **Lobby:** add partyAttributes for matchmaking ([68a1603](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/68a16039a78ba947fe07f9dc70e51e51a9a09efd))
* **User:** add ForcedLinkOtherPlatform function ([08ebe3a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/08ebe3a8f1c9af9c8219c8a56bc30e7c6910fac2))
* **User:** fix refreshed token not distributed correctly to other services ([009e1b6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/009e1b6a341725eb1a1232c11c78ee0a53721595))

## [2.17.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.17.0%0D2.16.0) (2020-08-19)


### Features

* **Achievement:** bring this API back from customer Versus Evil Unity SDK repository. ([fcd5ed6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fcd5ed6371c0bd80452e3b58d6178e23c8ad2403))


### Bug Fixes

* **test:** additional incremental Achievement test ([2809e5a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2809e5a624c6e814b662d194e4c3c0182bfc3138))

## [2.16.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.16.0%0D2.15.3) (2020-08-05)


### Features

* **editor:** AccelBytePlugin static class need to be forcefully initialize on Unity Editor entering the play mode ([afa43bf](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/afa43bfa70b8376511b36c34b6d2973a1db3cfa1))
* **gameTelemetry:** APIs are added for both client & server ([fc03e03](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fc03e036cd79f51a270aec7c52bf1581ca253151))


### Bug Fixes

* **cloudstorage:** Fix UpdateSlotMetadata function ([bdf465a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bdf465ac5b0dc714b0875648113f1c0799ed9b1a))
* **dsm:** only select healthy DSM url ([5cf6118](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5cf6118b12cedee60510f95aa5a42b069e74f16e))
* **statistic:** Fix statistic error codes. ([035ff92](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/035ff92b60f44ad8df74934bf7110b17bd264aab))


### CI

* **jenkins:** archive test log file ([e42fbb1](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e42fbb117495c2faa5fcf6fa99f27746afbb7302))
* **jenkins:** temporary social service env flag ([a746962](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a746962bfc07c6a2088b6c4020cb6addcf57b7a5))


### Tests

* add leaderboard log result ([abbb461](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/abbb4614cb8d0c1d3b46d29a9a6d59185873f809))
* log the test name at beginning test run ([160246b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/160246b3ad4fc811d8acd8f5657cdeb3ebe56adf))


### Refactors

* **Agremeent:** add missing function from bring back ([c7c5d7e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c7c5d7eb8fa4fd34132f1daa024fe71f44427154))

### [2.15.3](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.15.3%0D2.15.2) (2020-07-08)


### Bug Fixes

* **serverDSM:** agones heartbeat delegate return should not be null ([4026c5c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4026c5c1718440f677f651c89ba2c300483ea76f))

## 2.15.2 (2020-07-01)
### Changed
- ServerDSM
    - ENABLE_AGONES_PLUGIN definition added
    - Required to compile Agones by AccelByte SDK

## 2.15.1 (2020-06-26)
### Changed
- Agreement
    - GetLegalPolicies
        - Add policyType and tags param
    - GetLegalPoliciesByCountry
        - Add policyType and tags param

## 2.15.0 (2020-06-25)
### Added
- CloudSave
    - Add SaveUserRecord
    - Add ReplaceUserRecord
    - Add GetUserRecord
    - Add GetPublicUserRecord
    - Add DeleteUserRecord
    - Add SaveGameRecord
    - Add ReplaceGameRecord
    - Add GetGameRecord
    - Add DeleteGameRecord
- CloudSaveModels
    - Add UserRecord
    - Add GameRecord

## 2.14.0 (2020-06-24)
### Added
- Leaderboard
    - Add GetRankings
    - Add GetUserRanking

## 2.13.0 (2020-06-23)
### Added
- Agones SDK
- Dedicated Server Manager Api
    - Agones provider support
### Fixed
- Pretty print AccelByteConfig.json

## 2.12.0 (2020-06-18)
### Added
- Agreement
    - Add GetLegalPolicies
    - Add GetLegalPoliciesByCountry
    - Add BulkAcceptPolicyVersions
    - Add AcceptPolicyVersion

## 2.11.0 (2020-06-08)
### Added
- User Account
    - Add UpgradeWithPlayerPortal 
    - Add GetCountryFromIP
- Entitlement
    - Add CreateDistributionReceiver
    - Add GetDistributionReceiver
    - Add DeleteDistributionReceiver
    - Add UpdateDistributionReceiver
- Config
    - Add ApiBaseUrl for GetCountryFromIP feature
    - Add NonApiBaseUrl for UpgradeWithPlayerPortal feature
- User Models
    - Add CountryInfo
    - Add UpgradeUserRequest 
- Ecommerce Models
    - Add DistributesAttributes
    - Add Attributes
    - Add Distribution Receiver

## 2.10.2 (2020-06-03)
### Changed
- Entitlement
    - GetEntitlements() -> GetEntitlement()
    
## 2.10.1 (2020-05-28)
### Added
- User
    - add `ps4` and xbox `live` enum for 3rd party login
- Plugin
    - add `UNITY_PS4`, `UNITY_XBOXONE` and `UNITY_SWITCH` macro to support development on 3rd platform

## 2.10.0 (2020-05-26)
### Added
- User
    - Bulk Get User Id by Third Party Platform
- Lobby
    - Add LobbyApi
    - Add Bulk Friend Request

## 2.9.0 (2020-04-14)
### Added
- UserProfile Api
    - GetCustomAttributes
    - UpdateCustomAttributes

## 2.8.0 (2020-04-09)
### Added
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

## 2.7.0 (2020-03-26)
### Added
- Session Management
    - Add automatic refresh session coroutine

## 2.6.0 (2020-03-03)
### Added
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

### Fixed
- Lobby
  - Stop reconnecting when lobby server disconnects client because token revoked, expired or another session for the same user connecting
- Integration Test
  - Fix user test

### Changed
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

## 2.5.0 (2019-11-25)
### Fixed
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
### Changed
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
### Added
- Log
  - Add HTTP Request/Response Log
  - Add Current Function Log
- Release
  - Add Release folder for Unity SDK Package (in GitHub)

## 2.4.0 (2019-11-11)
### Changed
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

## 2.3.0 (2019-10-15)
### Added

- Enum members for IAM error handling

### Fixed

- Jenkins Automation
  - Adding Order param on Ecommerce Test
  - Fix set proxy on jenkins file
  - Add delay for steam to startup on jenkins file
  - Ignore some test on RetryTest because can't be done on automated machine
  - Change virtual coinCode from "SdkCoin" to "SDKC"
  - Adding "sortBy" param for GetItemByCriteria endpoint
  - Adding set proxy on Lobby WebSocket test

### Changed

- Remove intermediate conversion from UTF8 to .NET string before converting to object

### Breaking changes

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

## v2.3.0 (2019-10-15)

### Added

- Enum members for IAM error handling

### Fixed

- Jenkins Automation
  - Adding Order param on Ecommerce Test
  - Fix set proxy on jenkins file
  - Add delay for steam to startup on jenkins file
  - Ignore some test on RetryTest because can't be done on automated machine
  - Change virtual coinCode from "SdkCoin" to "SDKC"
  - Adding "sortBy" param for GetItemByCriteria endpoint
  - Adding set proxy on Lobby WebSocket test

### Changed

- Remove intermediate conversion from UTF8 to .NET string before converting to object

### Breaking changes

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

## v2.0.0 (2019-08-19)
### Added
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

### Changed
- Coroutine
  - Change from Custom Coroutine to Unity Coroutine
- Updated to v2.0.0 for Justice release v2.0.0

## v1.0.0-beta.1 (2019-03-13)
### Added
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
