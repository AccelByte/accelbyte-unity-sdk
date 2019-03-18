// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using Random = System.Random;

namespace AccelByte.Api
{
    /// <summary>
    /// User class provides convenient interaction to user authentication and account management service (AccelByte IAM).
    /// This user class will manage user credentials to be used to access other services, including refreshing its token
    /// </summary>
    public class User
    {
        //Constants
        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);
        private const string AuthorizationCodeEnvironmentVariable = "JUSTICE_AUTHORIZATION_CODE";

        //Readonly members
        private readonly AuthenticationApi authApi;
        private readonly UserApi userApi;
        private readonly string @namespace;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUri;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        private TokenData tokenData;
        private DateTime nextRefreshTime;

        private string clientToken;
        private DateTime clientTokenExpiryTime;
        
        /// <summary>
        /// Check if user is logged in or not
        /// </summary>
        public bool IsLoggedIn
        {
            get { return this.tokenData != null; }
        }

        /// <summary>
        /// Logged in username (email or phone number)
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Logged in user display name
        /// </summary>
        public string DisplayName
        {
            get { return this.tokenData != null ? this.tokenData.display_name : null; }
        }

        /// <summary>
        /// Logged in user namespace
        /// </summary>
        public string Namespace
        {
            get { return this.tokenData != null ? this.tokenData.@namespace : null; }
        }

        /// <summary>
        /// Logged in user id
        /// </summary>
        public string UserId
        {
            get { return this.tokenData != null ? this.tokenData.user_id : null; }
        }

        /// <summary>
        /// User access token to be used on other services
        /// </summary>
        public string AccessToken
        {
            get { return this.tokenData != null ? this.tokenData.access_token : null; }
        }

        internal User(AuthenticationApi authApi, UserApi userApi, string @namespace, 
            string clientId, string clientSecret, string redirectUri, AsyncTaskDispatcher taskDispatcher, 
            CoroutineRunner coroutineRunner)
        {
            this.authApi = authApi;
            this.userApi = userApi;
            this.@namespace = @namespace;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
            this.taskDispatcher.Start(Refresh());
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password
        /// </summary>
        /// <param name="userName">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithUserName(string userName, string password, ResultCallback callback)
        {
            this.UserName = userName;
            
            this.taskDispatcher.Start(
                Task.Retry(
                    cb => LoginWithUserNameAsync(userName, password, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    null));
        }

        private IEnumerator<ITask> LoginWithUserNameAsync(string email, string password, ResultCallback callback)
        {
            if (this.IsLoggedIn)
            {
                Logout();
            }

            Result<TokenData> loginResult = null;
        
            yield return Task.Await(this.authApi.GetUserToken(this.@namespace, 
                this.clientId, this.clientSecret, email, password, 
                result => { loginResult = result; }));

            if (loginResult.IsError)
            {
                callback.TryError(ErrorCode.GenerateTokenFailed, "Generate token with password grant failed.",
                    loginResult.Error);
                yield break;
            }
            
            this.tokenData = loginResult.Value;
            this.nextRefreshTime = User.ScheduleNormalRefresh(this.tokenData.expires_in);
            callback.TryOk();
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
            this.taskDispatcher.Start(
                Task.Retry(
                    cb => LoginWithOtherPlatformAsync(platformType, platformToken, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    null));
        }

        private IEnumerator<ITask> LoginWithOtherPlatformAsync(PlatformType platformType, string platformToken,
            ResultCallback callback)
        {
            if (this.IsLoggedIn)
            {
                Logout();
            }

            Result<TokenData> loginResult = null;

            yield return Task.Await(this.authApi.GetUserTokenWithOtherPlatform(this.@namespace,
                this.clientId, this.clientSecret, platformType, platformToken,
                result => { loginResult = result; }));

            if (loginResult.IsError)
            {
                callback.TryError(ErrorCode.GenerateTokenFailed, 
                    "cannot generate platform token for " + platformType, loginResult.Error);
                
                yield break;
            }

            this.tokenData = loginResult.Value;
            this.nextRefreshTime = User.ScheduleNormalRefresh(this.tokenData.expires_in);

            callback.TryOk();
        }

        /// <summary>
        /// Login With AccelByte Launcher. Use this only if you integrate your game with AccelByte Launcher
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLauncher(ResultCallback callback)
        {
            string authCode = Environment.GetEnvironmentVariable(User.AuthorizationCodeEnvironmentVariable);

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => LoginWithLauncherAsync(authCode, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    null));
        }
        
        private IEnumerator<ITask> LoginWithLauncherAsync(string authCode, ResultCallback callback)
        {
            if (this.IsLoggedIn)
            {
                Logout();
            }

            Result<TokenData> loginResult = null;

            yield return Task.Await(this.authApi.GetUserTokenWithAuthorizationCode(this.clientId,
                this.clientSecret, authCode, this.redirectUri,
                result => { loginResult = result; }));

            if (loginResult.IsError)
            {
                callback.Try(Result.CreateError(ErrorCode.GenerateTokenFailed, 
                    "Generate token with authorization code grant failed.", loginResult.Error));
                yield break;
            }

            this.tokenData = loginResult.Value;
            this.nextRefreshTime = User.ScheduleNormalRefresh(this.tokenData.expires_in);

            callback.Try(Result.CreateOk());
        }
        
        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithDeviceId(ResultCallback callback)
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => LoginWithDeviceIdAsync(deviceProvider, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    null));
        }

        private IEnumerator<ITask> LoginWithDeviceIdAsync(DeviceProvider deviceProvider, ResultCallback callback)
        {
            if (this.IsLoggedIn)
            {
                Logout();
            }

            Result<TokenData> loginResult = null;

            yield return Task.Await(
                this.authApi.GetUserTokenWithDeviceId(
                    this.@namespace, this.clientId, this.clientSecret, deviceProvider.DeviceType, 
                    deviceProvider.DeviceId, 
                    result => loginResult = result));

            if (loginResult.IsError)
            {
                callback.TryError(loginResult.Error.Code, loginResult.Error.Message);
                
                yield break;
            }

            this.tokenData = loginResult.Value;
            this.nextRefreshTime = User.ScheduleNormalRefresh(this.tokenData.expires_in);

            callback.TryOk();
        }

        /// <summary>
        /// Logout current user session
        /// </summary>
        public void Logout()
        {
            this.tokenData = null;
            this.nextRefreshTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Force refresh session. This method is AccelByte coroutine-like special method.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ITask> ForceRefresh()
        {
            string oldAccessToken = this.AccessToken;
            this.nextRefreshTime = DateTime.UtcNow;

            //TODO: Handle if refresh failed
            while (oldAccessToken == this.AccessToken)
            {
                yield return Task.Delay(User.WaitExpiryDelay);
            }
        }

        private IEnumerator<ITask> Refresh()
        {
            TimeSpan tokenRefreshBackoff = TimeSpan.FromSeconds(10);
            var rand = new Random();

            while (true)
            {
                if (tokenRefreshBackoff >= User.MaxBackoffInterval)
                {
                    yield break;
                }

                if (this.tokenData == null || DateTime.UtcNow < this.nextRefreshTime)
                {
                    yield return Task.Delay(User.WaitExpiryDelay);
                    
                    continue;
                }

                Result<TokenData> refreshResult = null;

                yield return Task.Await(
                    this.authApi.GetUserTokenWithRefreshToken(
                        this.clientId, this.clientSecret, this.tokenData.refresh_token,
                        result => { refreshResult = result; }));

                if (!refreshResult.IsError)
                {
                    this.tokenData = refreshResult.Value;
                    this.nextRefreshTime = User.ScheduleNormalRefresh(refreshResult.Value.expires_in);
                }
                else
                {
                    tokenRefreshBackoff = User.CalculateBackoffInterval(tokenRefreshBackoff, rand.Next(1, 60));
                    this.nextRefreshTime = DateTime.UtcNow + tokenRefreshBackoff;
                }
            }
        }

        private static DateTime ScheduleNormalRefresh(int expiresIn)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn + 1) * 0.8);
        }

        private static TimeSpan CalculateBackoffInterval(TimeSpan previousRefreshBackoff, int randomNum)
        {
            previousRefreshBackoff = TimeSpan.FromSeconds(previousRefreshBackoff.Seconds * 2);

            return previousRefreshBackoff + TimeSpan.FromSeconds(randomNum);
        }
        
        private IEnumerator<ITask> GetClientToken(ResultCallback<string> callback)
        {
            if (this.clientToken == null || this.clientTokenExpiryTime < DateTime.UtcNow)
            {
                Result<TokenData> tokenResult = null;
                yield return Task.Await(this.authApi.GetClientToken(this.clientId,
                    this.clientSecret, result => tokenResult = result));

                if (tokenResult.IsError)
                {
                    callback.TryError(tokenResult.Error.Code, tokenResult.Error.Message);
                    yield break;
                }

                this.clientToken = tokenResult.Value.access_token;
                this.clientTokenExpiryTime = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.Value.expires_in);
            }
            
            callback.TryOk(this.clientToken);
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="userName">Username to identify (and verify) user with (email or phone)</param>
        /// <param name="password">Password to login</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Usernam</param>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void Register(string userName, string password, string displayName,
            ResultCallback<UserData> callback)
        {
            var userInfo = new RegisterUserRequest
            {
                AuthType = AuthenticationType.EMAILPASSWD, //Right now, it's hardcoded to email
                UserName = userName,
                Password = password,
                DisplayName = displayName
            };

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => RegisterAsync(userInfo, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserData>) result)),
                    null));
        }

        private IEnumerator<ITask> RegisterAsync(RegisterUserRequest registerUserRequest, ResultCallback<UserData> callback)
        {
            Result<string> clientTokenResult = null;

            yield return Task.Await(GetClientToken(result => clientTokenResult = result));

            if (clientTokenResult.IsError)
            {
                callback.TryError(ErrorCode.GeneralClientError, "Getting client access token failed.", clientTokenResult.Error);
                yield break;
            }

            yield return Task.Await(this.userApi.Register(this.@namespace, clientTokenResult.Value, registerUserRequest,
                callback));
        }
        
        /// <summary>
        /// Get current logged in user data
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void GetData(ResultCallback<UserData> callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.GetData(this.Namespace, this.UserId, this.AccessToken, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserData>) result)),
                    this));
        }

        /// <summary>
        /// Update some user information (e.g. language or country)
        /// </summary>
        /// <param name="updateRequest">Set its field if you want to change it, otherwise leave it</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Update(UpdateUserRequest updateRequest,
            ResultCallback<UserData> callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => UpdateAsync(updateRequest, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserData>) result)),
                    this));
        }

        private IEnumerator<ITask> UpdateAsync(UpdateUserRequest updateRequest,
            ResultCallback<UserData> callback)
        {
            Result<UserData> updateResult = null;

            yield return Task.Await(this.userApi.Update(this.Namespace, this.UserId, 
                this.AccessToken, updateRequest, result => updateResult = result));

            if (!updateResult.IsError)
            {
                yield return Task.Await(ForceRefresh());
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
        public void Upgrade(string userName, string password,
            ResultCallback<UserData> callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => UpgradeAsync(userName, password, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserData>) result)),
                    this));
        }

        private IEnumerator<ITask> UpgradeAsync(string userName, string password,
            ResultCallback<UserData> callback)
        {
            Result<UserData> apiResult = null;
            yield return Task.Await(this.userApi.Upgrade(this.Namespace, this.AccessToken, 
                this.UserId, userName, password, 
                result => { apiResult = result; }));

            if (!apiResult.IsError)
            {
                yield return Task.Await(ForceRefresh());
            }
            
            callback.Try(apiResult);
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode(ResultCallback callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.SendVerificationCode(this.Namespace, this.AccessToken,
                        this.UserId, VerificationContext.UserAccountRegistration, this.UserName,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this));
        }

        /// <summary>
        /// Verify a user via an email registered as its username. User must be logged in.
        /// </summary>
        /// <param name="verificationCode">Verification code received from user's email</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void Verify(string verificationCode, ResultCallback callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            //TODO: Hard-coded contact type, if phone is activated in the future, we should add UserName to User
            //class and determine whether it's email or phone by regex.
            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.Verify(this.Namespace, this.AccessToken, this.UserId, verificationCode, "email", 
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email, if he/she wants to also verify
        /// while upgrading.
        /// </summary>
        /// <param name="userName">Username to upgrade to (headless/device account doesn't have username)</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void SendUpgradeVerificationCode(string userName, ResultCallback callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.SendVerificationCode(this.Namespace, this.AccessToken, this.UserId, 
                        VerificationContext.UpgradeHeadlessAccount, userName, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this));
        }

        /// <summary>
        /// Upgrade and verify the upgraded account. User must be logged in
        /// </summary>
        /// <param name="userName">Username to upgrade</param>
        /// <param name="password">Password to login</param>
        /// <param name="verificationCode">Verification code retrieved from email or phone </param>
        /// <param name="callback">Returns Result that contains UserData via callback when completed</param>
        public void UpgradeAndVerify(string userName, string password,
            string verificationCode, ResultCallback<UserData> callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => UpgradeAndVerifyAsync(userName, password, verificationCode, 
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserData>) result)),
                    this));
        }

        private IEnumerator<ITask> UpgradeAndVerifyAsync(string userName, string password,
            string verificationCode, ResultCallback<UserData> callback)
        {
            Result<UserData> apiResult = null;
            
            yield return Task.Await(this.userApi.UpgradeAndVerify(this.Namespace, 
                this.AccessToken, this.UserId, userName, password, verificationCode, 
                result => { apiResult = result; }));

            if (!apiResult.IsError)
            {
                yield return Task.Await(ForceRefresh());
            }
            
            callback.Try(apiResult);
        }

        /// <summary>
        /// Trigger an email that contains reset password code to be sent to user
        /// </summary>
        /// <param name="userName">Username to be sent reset password code to.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendResetPasswordCode(string userName, ResultCallback callback)
        {
            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.SendPasswordResetCode(AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret, userName,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    null));
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
            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.ResetPassword(AccelBytePlugin.Config.Namespace, AccelBytePlugin.Config.ClientId,
                        AccelBytePlugin.Config.ClientSecret, resetCode, userName, newPassword, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    null));
        }

        /// <summary>
        /// Link other platform to the currently logged in user. 
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformTicket">Ticket / token from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LinkOtherPlatform(PlatformType platformType, string platformTicket,
            ResultCallback callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.LinkOtherPlatform(this.Namespace, this.AccessToken, 
                        this.UserId, platformType.ToString().ToLower(), platformTicket, 
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this));
        }

        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login.
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformTicket">Ticket / token from other platform to unlink from this user</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkOtherPlatform(PlatformType platformType, string platformTicket,
            ResultCallback callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.UnlinkOtherPlatform(this.Namespace, this.AccessToken, 
                        this.UserId, platformType.ToString(), 
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this));
        }

        /// <summary>
        /// Get array of other platforms this user linked to
        /// </summary>
        /// <param name="callback">Returns a Result that contains PlatformLink array via callback when
        /// completed.</param>
        public void GetPlatformLinks(ResultCallback<PlatformLink[]> callback)
        {
            if (!this.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.userApi.GetPlatformLinks(this.Namespace, this.AccessToken, this.UserId,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<PlatformLink[]>) result)),
                    this));
        }
    }
}