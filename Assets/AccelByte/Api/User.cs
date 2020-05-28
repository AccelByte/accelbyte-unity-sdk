// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;

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

        //Readonly members
        private readonly ILoginSession loginSession;
        private readonly IUserAccount userAccount;
        private readonly CoroutineRunner coroutineRunner;
        private readonly bool needsUserId;
        private readonly AccelByteSession sessionAdapter;

        public ISession Session { get { return this.sessionAdapter; } }

        private UserData userDataCache;

        internal User(ILoginSession loginSession, IUserAccount userAccount, CoroutineRunner coroutineRunner,
            bool needsUserId)
        {
            this.loginSession = loginSession;
            this.userAccount = userAccount;
            this.coroutineRunner = coroutineRunner;
            this.needsUserId = needsUserId;
            this.sessionAdapter = new AccelByteSession();
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password
        /// </summary>
        /// <param name="username">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithUsername(string username, string password, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(LoginWithUserNameAsync(username, password, callback));
        }

        private IEnumerator LoginAsync(Func<ResultCallback, IEnumerator> loginMethod, ResultCallback callback)
        {
            if (this.sessionAdapter.IsValid())
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

            this.sessionAdapter.AuthorizationToken = this.loginSession.AuthorizationToken;

            if (this.needsUserId)
            {
                Result<UserData> userDataResult = null;

                yield return RefreshDataAsync(result => userDataResult = result);

                if (userDataResult.IsError)
                {
                    callback.TryError(userDataResult.Error);

                    yield break;
                }

                this.sessionAdapter.UserId = this.userDataCache.userId;
            }
            else
            {
                this.sessionAdapter.UserId = this.loginSession.UserId;
            }

            callback.TryOk();
        }

        private IEnumerator LoginWithUserNameAsync(string email, string password, ResultCallback callback)
        {
            yield return LoginAsync(cb => this.loginSession.LoginWithUsername(email, password, cb), callback);
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
        /// Logout current user session
        /// </summary>
        public void Logout(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            
            if (!this.sessionAdapter.IsValid())
            {
                callback.TryOk();

                return;
            }

            this.sessionAdapter.UserId = null;

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
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(SendVerificationCodeAsync(callback));
        }

        private IEnumerator SendVerificationCodeAsync(ResultCallback callback)
        {
            Result<UserData> userDataResult = null;

            yield return GetDataAsync(r => userDataResult = r);

            if (userDataResult.IsError)
            {
                callback.TryError(
                    new Error(
                        ErrorCode.GeneralClientError,
                        "Failed when trying to get username",
                        userDataResult.Error));

                yield break;
            }

            yield return this.userAccount.SendVerificationCode(
                VerificationContext.UserAccountRegistration,
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
        /// Link other platform to the currently logged in user. 
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformTicket">Ticket / token from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LinkOtherPlatform(PlatformType platformType, string platformTicket, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.sessionAdapter.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.LinkOtherPlatform(platformType, platformTicket, callback));
        }

        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login.
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformTicket">Ticket / token from other platform to unlink from this user</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkOtherPlatform(PlatformType platformType, string platformTicket, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.sessionAdapter.IsValid())
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

            if (!this.sessionAdapter.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.GetPlatformLinks(callback));
        }

        private class AccelByteSession : ISession
        {
            public string AuthorizationToken { get; set; }
            public string UserId { get; set; }
        }

        /// <summary>
        /// Get user data from another user by email
        /// </summary>
        /// <param name="emailOrDisplayName"> email or display name that needed to get user data</param>
        /// <param name="callback"> Return a Result that contains UserData when completed. </param>
        public void SearchUsers(string emailOrDisplayName, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.sessionAdapter.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.SearchUsers(emailOrDisplayName, callback));
        }

        /// <summary>
        /// Get user data from another user by user id
        /// </summary>
        /// <param name="userId"> user id that needed to get user data</param>
        /// <param name="callback"> Return a Result that contains UserData when completed. </param>
        public void GetUserByUserId(string userId, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.sessionAdapter.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.userAccount.GetUserByUserId(userId, callback));
        }

        /// <summary>
        /// Get other user data by other platform userId (such as SteamID, for example)
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="otherPlatformUserId"></param>
        /// <param name="callback"></param>
        public void GetUserByOtherPlatformUserId(PlatformType platformType, string otherPlatformUserId,
            ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.sessionAdapter.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.userAccount.GetUserByOtherPlatformUserId(platformType, otherPlatformUserId, callback));
        }
    }
}