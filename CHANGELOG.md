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