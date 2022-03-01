// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Text.RegularExpressions;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// User class provides convenient interaction to user authentication and account management service (AccelByte IAM).
    /// This user class will manage user credentials to be used to access other services, including refreshing its token
    /// </summary>
    public class User
    {
        //Constants
        private const string AuthorizationCodeEnvironmentVariable = "JUSTICE_AUTHORIZATION_CODE";
        private const int ttl = 60;

        //Readonly members
        private readonly LoginSession loginSession;
        private readonly UserAccount userAccount;
        private readonly CoroutineRunner coroutineRunner;

        public ISession Session => this.loginSession;

        private UserData userDataCache;

        internal User(LoginSession loginSession, UserAccount userAccount, CoroutineRunner coroutineRunner)
        {
            this.loginSession = loginSession;
            this.userAccount = userAccount;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password
        /// </summary>
        /// <param name="username">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        public void LoginWithUsername(string username, string password, ResultCallback callback, bool rememberMe = false)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithUserNameAsync(username, password, callback, rememberMe));
        }

        /// <summary>
        /// Login to AccelByte account with username (or email) and password using V3 endpoint
        /// </summary>
        /// <param name="username">Could be username or email</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        public void LoginWithUsernameV3(string username, string password, ResultCallback callback, bool rememberMe = false)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithUserNameAsyncV3(username, password, callback, rememberMe));
        }

        private IEnumerator LoginAsync(Func<ResultCallback, IEnumerator> loginMethod, ResultCallback callback)
        {
            if (this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.InvalidRequest, "User is already logged in.");

                yield break;
            }

            Result loginResult = null;

            yield return loginMethod(r => loginResult = r);

            if (loginResult.IsError)
            {
                callback.TryError(loginResult.Error);

                yield break;
            }

            callback.TryOk();
        }

        private IEnumerator LoginWithUserNameAsync(string email, string password, ResultCallback callback, bool rememberMe = false)
        {
            yield return LoginAsync(cb => this.loginSession.LoginWithUsername(email, password, cb, rememberMe), callback);
        }

        private IEnumerator LoginWithUserNameAsyncV3(string email, string password, ResultCallback callback, bool rememberMe = false)
        {
            yield return LoginAsync(cb => this.loginSession.LoginWithUsernameV3(email, password, cb, rememberMe), callback);
        }

        /// <summary>
        /// Login with token from non AccelByte platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithOtherPlatform(PlatformType platformType, string platformToken, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithOtherPlatformAsync(platformType, platformToken, callback));
        }

        private IEnumerator LoginWithOtherPlatformAsync(PlatformType platformType, string platformToken,
            ResultCallback callback)
        {
            yield return LoginAsync(
                cb => this.loginSession.LoginWithOtherPlatform(platformType, platformToken, cb),
                callback);
        }

        /// <summary>
        /// Login With AccelByte Launcher. Use this only if you integrate your game with AccelByte Launcher
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLauncher(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            string authCode = Environment.GetEnvironmentVariable(User.AuthorizationCodeEnvironmentVariable);

            if (string.IsNullOrEmpty(authCode))
            {
                this.coroutineRunner.Run(() =>
                {
                    callback.TryError(ErrorCode.InvalidArgument, "The application was not executed from launcher");
                });
                return;
            }

            this.coroutineRunner.Run(LoginWithAuthorizationCodeAsync(authCode, callback));
        }

        private IEnumerator LoginWithAuthorizationCodeAsync(string authCode, ResultCallback callback)
        {
            yield return LoginAsync(cb => this.loginSession.LoginWithAuthorizationCode(authCode, cb), callback);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithDeviceId(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithDeviceIdAsync(callback));
        }

        private IEnumerator LoginWithDeviceIdAsync(ResultCallback callback)
        {
            yield return LoginAsync(this.loginSession.LoginWithDeviceId, callback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already epired.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLatestRefreshToken(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithLatestRefreshTokenAsync(null, callback));
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLatestRefreshToken(string refreshToken, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithLatestRefreshTokenAsync(refreshToken, callback));
        }

        private IEnumerator LoginWithLatestRefreshTokenAsync(string refreshToken, ResultCallback callback)
        {
            
            yield return LoginAsync(cb => this.loginSession.LoginWithLatestRefreshToken(refreshToken, cb), callback);
        }

        /// <summary>
        /// Refresh current login session. Will update current token.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void RefreshSession(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.loginSession.RefreshSession(callback));
        }

        /// <summary>
        /// Logout current user session. Access tokens, user ID, and other credentials from memory will be removed.
        /// </summary>
        public void Logout(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryOk();

                return;
            }

            this.userDataCache = null;
            this.coroutineRunner.Run(this.loginSession.Logout(callback));
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="emailAddress">Email address of the user</param>
        /// <param name="password">Password to login</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Usernam</param>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void Register(string emailAddress, string password, string displayName, string country,
            DateTime dateOfBirth, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            var registerUserRequest = new RegisterUserRequest
            {
                authType = AuthenticationType.EMAILPASSWD,
                emailAddress = emailAddress,
                password = password,
                displayName = displayName,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd")
            };

            this.coroutineRunner.Run(this.userAccount.Register(registerUserRequest, callback));
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="emailAddress">Email address of the user, can be used as login username</param>
        /// <param name="username">The username can be used as login username, case insensitive, alphanumeric with allowed symbols underscore (_) and dot (.)</param>
        /// <param name="password">Password to login, 8 to 32 characters, satisfy at least 3 out of 4 conditions(uppercase, lowercase letters, numbers and special characters) and should not have more than 2 equal characters in a row.</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Username</param>
        /// <param name="country">User'd country, ISO3166-1 alpha-2 two letter, e.g. US.</param>
        /// <param name="dateOfBirth">User's date of birth, valid values are between 1905-01-01 until current date.</param>
        /// <param name="callback">Returns a Result that contains RegisterUserResponse via callback</param>
        public void Registerv2(string emailAddress, string username, string password, string displayName, string country,
            DateTime dateOfBirth, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            var registerUserRequest = new RegisterUserRequestv2
            {
                authType = AuthenticationType.EMAILPASSWD,
                emailAddress = emailAddress,
                username = username,
                password = password,
                displayName = displayName,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd")
            };

            this.coroutineRunner.Run(this.userAccount.Registerv2(registerUserRequest, callback));
        }

        /// <summary>
        /// Register a user while optionally accepting legal policies, password, and displayName 
        /// </summary>
        /// <param name="request">To accept policies, fill acceptedPolicies field</param>
        /// <param name="callback">Returns a Result that contains RegisterUserResponse via callback</param>
        public void RegisterAndAcceptPolicies(RegisterUserRequestv2 request, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            
            //authType other than EMAILPASSWD is not supported
            request.authType = AuthenticationType.EMAILPASSWD;
            Assert.IsTrue(
                Regex.IsMatch(request.dateOfBirth, "^\\d{4}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$"),
                "Date of birth format is yyyy-MM-dd");

            this.coroutineRunner.Run(this.userAccount.Registerv2(request, callback));
        }
        
        /// <summary>
        /// Get current logged in user data. It will return cached user data if it has been called before
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void GetData(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(GetDataAsync(callback));
        }

        private IEnumerator GetDataAsync(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (this.userDataCache != null)
            {
                callback.TryOk(this.userDataCache);
            }
            else
            {
                yield return RefreshDataAsync(callback);
            }
        }

        /// <summary>
        /// Refresh currrent cached user data.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void RefreshData(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.userDataCache = null;

            this.coroutineRunner.Run(RefreshDataAsync(callback));
        }

        private IEnumerator RefreshDataAsync(ResultCallback<UserData> callback)
        {
            Result<UserData> result = null;

            yield return this.userAccount.GetData(r => result = r);

            if (!result.IsError)
            {
                this.userDataCache = result.Value;

                callback.TryOk(this.userDataCache);

                yield break;
            }

            callback.Try(result);
        }

        /// <summary>
        /// Update some user information (e.g. language or country)
        /// </summary>
        /// <param name="updateRequest">Set its field if you want to change it, otherwise leave it</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Update(UpdateUserRequest updateRequest, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(UpdateAsync(updateRequest, callback));
        }

        private IEnumerator UpdateAsync(UpdateUserRequest updateRequest, ResultCallback<UserData> callback)
        {
            Result<UserData> updateResult = null;

            yield return this.userAccount.Update(updateRequest, result => updateResult = result);

            if (!updateResult.IsError)
            {
                this.userDataCache = updateResult.Value;
            }

            callback.Try(updateResult);
        }

        /// <summary>
        /// Update user email address
        /// </summary>
        /// <param name="updateEmailRequest">Set verify code and user new email on the body</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void UpdateEmail(UpdateEmailRequest updateEmailRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.UpdateEmail(updateEmailRequest, callback));
        }

        /// <summary>
        /// Upgrade a headless account with username and password. User must be logged in before this method can be
        /// used.
        /// </summary>
        /// <param name="userName">Username the user is upgraded to</param>
        /// <param name="password">Password to login with username</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Upgrade(string userName, string password, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(UpgradeAsync(userName, password, callback));
        }

        private IEnumerator UpgradeAsync(string username, string password, ResultCallback<UserData> callback)
        {
            Result<UserData> result = null;

            yield return this.userAccount.Upgrade(username, password, r => result = r);

            if (!result.IsError)
            {
                this.userDataCache = result.Value;
            }

            callback.Try(result);
        }

        /// <summary>
        /// Upgrade a headless account with username and password. User must be logged in before this method can be
        /// used.
        /// </summary>
        /// <param name="emailAddress">Email Address the user is upgraded to</param>
        /// <param name="userName">Username the user is upgraded to</param>
        /// <param name="password">Password to login with username</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Upgradev2(string emailAddress, string userName, string password, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(Upgradev2Async(emailAddress, userName, password, callback));
        }

        private IEnumerator Upgradev2Async(string emailAddress, string username, string password, ResultCallback<UserData> callback)
        {
            Result<UserData> result = null;

            yield return this.userAccount.Upgradev2(emailAddress, username, password, r => result = r);

            if (!result.IsError)
            {
                this.userDataCache = result.Value;
            }

            callback.Try(result);
        }

        /// <summary>
        /// Upgrade a headless account. User must be logged in first then call
        /// SendUpgradeVerificationCode code to get verification code send to their email 
        /// </summary>
        /// <param name="upgradeAndVerifyHeadlessRequest">Contain user data that will be used to upgrade the headless account</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void UpgradeAndVerifyHeadlessAccount(UpgradeAndVerifyHeadlessRequest upgradeAndVerifyHeadlessRequest, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.UpgradeAndVerifyHeadlessAccount(upgradeAndVerifyHeadlessRequest, callback));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in with headless account.
        /// This function context is "upgradeHeadlessAccount".
        /// </summary>
        /// <param name="emailAddress">The email use to send verification code</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendUpgradeVerificationCode(string emailAddress, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.SendVerificationCode(
                VerificationContext.upgradeHeadlessAccount,
                emailAddress,
                callback));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// This function context is "UserAccountRegistration". If you want to set your own context, please use the other overload function.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(SendVerificationCodeAsync(VerificationContext.UserAccountRegistration, callback));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// </summary>
        /// <param name="verifyContext">The context of what verification request</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode(VerificationContext verificationContext, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(SendVerificationCodeAsync(verificationContext, callback));
        }

        private IEnumerator SendVerificationCodeAsync(VerificationContext verificationContext, ResultCallback callback)
        {
            Result<UserData> userDataResult = null;

            yield return GetDataAsync(r => userDataResult = r);

            if (userDataResult.IsError)
            {
                callback.TryError(
                    new Error(
                        ErrorCode.GeneralClientError,
                        "Failed when trying to get username",
                        "",
                        userDataResult.Error));

                yield break;
            }

            yield return this.userAccount.SendVerificationCode(
                verificationContext,
                this.userDataCache.emailAddress,
                callback);
        }

        /// <summary>
        /// Verify a user via an email registered as its username. User must be logged in.
        /// </summary>
        /// <param name="verificationCode">Verification code received from user's email</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void Verify(string verificationCode, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.Verify(verificationCode, "email", callback));
        }

        /// <summary>
        /// Trigger an email that contains reset password code to be sent to user
        /// </summary>
        /// <param name="userName">Username to be sent reset password code to.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendResetPasswordCode(string userName, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.SendPasswordResetCode(userName, callback));
        }

        /// <summary>
        /// Reset password for a username
        /// </summary>
        /// <param name="resetCode">Reset password code</param>
        /// <param name="userName">Username with forgotten password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ResetPassword(string resetCode, string userName, string newPassword, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.ResetPassword(resetCode, userName, newPassword, callback));
        }

        /// <summary>
        /// Link other platform's account to the currently logged in user. 
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformTicket">Ticket / token from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LinkOtherPlatform(PlatformType platformType, string platformTicket, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.LinkOtherPlatform(platformType, platformTicket, callback));
        }

        /// <summary>
        /// Force to Link other platform's account to the currently logged in user. 
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformUserId"> UserId from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ForcedLinkOtherPlatform(PlatformType platformType, string platformUserId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.ForcedLinkOtherPlatform(platformType, platformUserId, callback));
        }

        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login.
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkOtherPlatform(PlatformType platformType, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.UnlinkOtherPlatform(platformType, callback));
        }

        /// <summary>
        /// Get array of other platforms this user linked to
        /// </summary>
        /// <param name="callback">Returns a Result that contains PlatformLink array via callback when
        /// completed.</param>
        public void GetPlatformLinks(ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.GetPlatformLinks(callback));
        }

        /// <summary>
        /// Get user data from another user displayName or username. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query"> Display name or username that needed to get user data.</param>
        /// <param name="by"> Filter the responded PagedPublicUsersInfo by SearchType. Choose the SearchType.ALL if you want to be responded with all query type.</param>
        /// <param name="callback"> Return a Result that contains UsersData when completed. </param>
        public void SearchUsers(string query, SearchType by, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.SearchUsers(query, by, callback));
        }

        /// <summary>
        /// Get user data from another user by displayName or username. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query"> Display name or username that needed to get user data.</param>
        /// <param name="callback"> Return a Result that contains UsersData when completed. </param>
        public void SearchUsers(string query, ResultCallback<PagedPublicUsersInfo> callback)
        {
            this.SearchUsers(query, SearchType.ALL, callback);
        }

        /// <summary>
        /// Get user data from another user by user id
        /// </summary>
        /// <param name="userId"> user id that needed to get user data</param>
        /// <param name="callback"> Return a Result that contains UserData when completed. </param>
        public void GetUserByUserId(string userId, ResultCallback<PublicUserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.GetUserByUserId(userId, callback));
        }

        /// <summary>
        /// Get other user data by other platform userId (such as SteamID, for example)
        /// For Nintendo Platform you need to append Environment ID into the Platorm ID, with this format PlatformID:EnvironmentID. e.g csgas12312323f:dd1
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="otherPlatformUserId"></param>
        /// <param name="callback"></param>
        public void GetUserByOtherPlatformUserId(PlatformType platformType, string otherPlatformUserId,
            ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.userAccount.GetUserByOtherPlatformUserId(platformType, otherPlatformUserId, callback));
        }

        /// <summary>
        /// Get other user data by other platform userId(s) (such as SteamID, for example)
        /// For Nintendo Platform you need to append Environment ID into the Platorm ID, with this format PlatformID:EnvironmentID. e.g csgas12312323f:dd1
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="otherPlatformUserIds"></param>
        /// <param name="callback"></param>
        public void BulkGetUserByOtherPlatformUserIds(PlatformType platformType, string[] otherPlatformUserId,
            ResultCallback<BulkPlatformUserIdResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            BulkPlatformUserIdRequest platformUserIds = new BulkPlatformUserIdRequest { platformUserIDs = otherPlatformUserId };
            this.coroutineRunner.Run(
                this.userAccount.BulkGetUserByOtherPlatformUserIds(platformType, platformUserIds, callback));
        }
        
        /// <summary>
        /// Get spesific country from user IP
        /// </summary>
        /// <param name="callback"> Returns a Result that contains country information via callback when completed</param>
        public void GetCountryFromIP(ResultCallback<CountryInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.userAccount.GetCountryFromIP(callback));
        }

        /// <summary>
        /// Check if user has purchased the subscription and eligible to play
        /// </summary>
        /// <param name="callback"> Returns the boolean result whether the user is subscribed and eligible to play the game via callback when the operation is completed</param>
        public void GetUserEligibleToPlay(ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            ResultCallback<ItemInfo> onGotItemInfo = (itemInfoResult) =>
                {
                    if(itemInfoResult.IsError)
                    {
                        callback.TryError(itemInfoResult.Error.Code);
                        return;
                    }

                    string[] skus = itemInfoResult.Value.features;
                    string[] appIds = { AccelBytePlugin.Config.AppId };

                    AccelBytePlugin.GetEntitlement().GetUserEntitlementOwnershipAny(null, appIds, skus, (ownershipResult) =>
                    {
                        if (ownershipResult.IsError)
                        {
                            callback.TryError(ownershipResult.Error.Code);
                            return;
                        }

                        callback.TryOk(ownershipResult.Value.owned);
                    });
                };

            AccelBytePlugin.GetItems().GetItemByAppId(AccelBytePlugin.Config.AppId, onGotItemInfo);
        }

        public void RefreshTokenCallback(Action<string> refreshTokenCallback)
        {
            this.loginSession.RefreshTokenCallback += refreshTokenCallback;
        }

        /// <summary>
        /// Ban a Single User.
        /// Only Moderator that can ban other user.
        /// </summary>
        /// <param name="userId">Ban user's user ID</param>
        /// <param name="banType">The type of Ban</param>
        /// <param name="reason">The reason of Banning</param>
        /// <param name="endDate">The date when the ban is lifted</param>
        /// <param name="comment">The detail or comment about the banning</param>
        /// <param name="notifyUser">Notify user via email or not</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void BanUser(string userId, BanType banType, BanReason reason, DateTime endDate, string comment, bool notifyUser,
            ResultCallback <UserBanResponseV3> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            var banRequest = new BanCreateRequest
            {
                ban = banType.ToString(),
                comment = comment,
                endDate = endDate.ToString("o"),
                reason = reason.ToString(),
                skipNotif = !notifyUser
            };

            this.coroutineRunner.Run(
                this.userAccount.BanUser(
                    AccelBytePlugin.Config.Namespace,
                    this.loginSession.AuthorizationToken,
                    userId,
                    banRequest,
                    callback));
        }

        /// <summary>
        /// Change Ban Status of a Single User (enabled/disabled).
        /// Only Moderator that can change ban status.
        /// </summary>
        /// <param name="userId">Banned user's user ID</param>
        /// <param name="banId">Banned user's ban ID</param>
        /// <param name="enabled">Banned Status, false to disabled</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ChangeUserBanStatus(string userId, string banId, bool enabled, ResultCallback<UserBanResponseV3> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.userAccount.ChangeUserBanStatus(
                    AccelBytePlugin.Config.Namespace,
                    this.loginSession.AuthorizationToken,
                    userId,
                    banId,
                    enabled,
                    callback));
        }

        /// <summary>
        /// Get User Banned List
        /// Only Moderator that can get the banned list.
        /// </summary>
        /// <param name="banType">The type of Ban</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0) </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="activeOnly">true to only get the enabled banned list</param>
        public void GetUserBannedList(BanType banType, int offset, int limit, ResultCallback<UserBanPagedList> callback, bool activeOnly = true)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.userAccount.GetUserBannedList(
                    AccelBytePlugin.Config.Namespace,
                    this.loginSession.AuthorizationToken,
                    activeOnly,
                    banType,
                    offset,
                    limit,
                    callback));
        }

        /// <summary>
        /// Get multiple user(s) information like user's DisplayName.
        /// </summary>
        /// <param name="userIds">List UserId(s) to get.</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void BulkGetUserInfo(string[] userIds, ResultCallback<ListBulkUserInfoResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.loginSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.BulkGetUserInfo(userIds, callback));
        }
    }
}
