﻿// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// User class provides convenient interaction to user authentication
    /// and account management service (AccelByte IAM). This user class
    /// will manage user credentials to be used to access other services,
    /// including refreshing its token.
    /// 
    /// <remarks>
    /// This is essentially a gateway to the "User" API; not to be confused
    /// with UserSession:ISession (which contains UserId, AuthorizationToken, etc). 
    /// </remarks> 
    /// </summary>
    public class User : WrapperBase
    {
        //Constants
        internal const string AuthorizationCodeEnvironmentVariable = "JUSTICE_AUTHORIZATION_CODE";
        private const int ttl = 60;

        //Readonly members
        private readonly UserSession userSession;//renamed from LoginSession
        public readonly OAuth2 oAuth2;
        private readonly UserApi api;
        private readonly CoroutineRunner coroutineRunner;

        public UserSession Session { get { return userSession; } }


        private UserData userDataCache;

        public bool TwoFAEnable { get; private set; } = false;

        /// <summary>
        /// </summary>
        /// <param name="inLoginSession">
        /// UserSession; not ISession (unlike similar modules like this)
        /// </param>
        /// <param name="inApi"></param>
        /// <param name="inCoroutineRunner"></param>
        internal User( UserApi inApi
            , UserSession inLoginSession
            , CoroutineRunner inCoroutineRunner )
        {
            userSession = inLoginSession;
            api = inApi;
            coroutineRunner = inCoroutineRunner;
            oAuth2 = new OAuth2(
                inApi.HttpClient,
                inApi.Config,
                userSession);
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inLoginSession">
        /// UserSession; not ISession (unlike similar modules like this)
        /// </param>
        /// <param name="inApi"></param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("For pattern parity, to conform with other classes, use the overload that starts with api param")]
        internal User( UserSession inLoginSession
            , UserApi inApi
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inLoginSession, inCoroutineRunner )
        {
            // Curry this obsolete data to the new overload ->
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password.
        /// </summary>
        /// <param name="username">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete("This end point is going to be deprected, use LoginWithUsername with other callback type instead")]
        public void LoginWithUsername( string username
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithUserNameAsync(
                username, password, callback, rememberMe));
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password.
        /// </summary>
        /// <param name="username">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete("This end point is going to be deprecated, use LoginWithUsernameV3 instead")]
        public void LoginWithUsername( string username
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithUserNameAsync(
                username, password, callback, rememberMe));
        }

        /// <summary>
        /// Login to AccelByte account with username (or email) and password using V3 endpoint
        /// </summary>
        /// <param name="username">Could be username or email</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        public void LoginWithUsernameV3( string username
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithUserNameAsyncV3(
                username, password, callback, rememberMe));
        }
        
        /// <summary>
        /// Login to AccelByte account with username (or email) and password using V3 endpoint
        /// </summary>
        /// <param name="username">Could be username or email</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        public void LoginWithUsernameV3( string username
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithUserNameAsyncV3(
                username, password, callback, rememberMe));
        }
        
        private IEnumerator LoginAsync( Func<ResultCallback, IEnumerator> loginMethod
            ,  ResultCallback callback )
        {
            if (userSession.IsValid())
            {
                callback.TryError(ErrorCode.InvalidRequest, 
                    "User is already logged in.");
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
        
        private IEnumerator LoginAsync( Func<ResultCallback<TokenData, OAuthError>, IEnumerator> loginMethod
            , ResultCallback<TokenData, OAuthError> callback )
        {
            if (userSession.IsValid())
            { 
                var error = new OAuthError() 
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "User is already logged in."
                };
                
                callback.TryError(error);
                yield break;
            }

            Result<TokenData, OAuthError> loginResult = null;

            yield return loginMethod(r =>
                {
                    loginResult = r;
                }
            );

            if (loginResult.IsError)
            {
                callback.TryError(loginResult.Error);
                yield break;
            }

            callback.Try(loginResult);
        }
        
        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password.
        /// </summary>
        /// <param name="email">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete("Instead, use LoginWithUserNameAsyncV3()")]
        private IEnumerator LoginWithUserNameAsync( string email
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            yield return LoginAsync(cb => 
                oAuth2.LoginWithUsernameV3(email, password, cb, rememberMe),
                callback);
        }
        
        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password.
        /// </summary>
        /// <param name="email">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete("Instead, use LoginWithUserNameAsync() which use different callback type")]
        private IEnumerator LoginWithUserNameAsync( string email
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithUsername(
                email, password, cb, rememberMe), callback);
        }
        
        [Obsolete("Instead, use LoginWithUserNameAsyncV3() which use different callback type")]
        private IEnumerator LoginWithUserNameAsyncV3( string email
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithUsernameV3(
                email, password, cb, rememberMe), callback);
        }
        
        private IEnumerator LoginWithUserNameAsyncV3( string email
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithUsernameV3(
                email, password, cb, rememberMe), callback);
        }

        /// <summary>
        /// Login with token from non AccelByte platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithOtherPlatform( PlatformType platformType
            , string platformToken
            , ResultCallback callback
            , bool createHeadless = true )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithOtherPlatformAsync(platformType, platformToken, callback, createHeadless));
        }

        /// <summary>
        /// Login with token from non AccelByte platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platfrom type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithOtherPlatform( PlatformType platformType
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless = true )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithOtherPlatformAsync(
                platformType,
                platformToken,
                callback,
                createHeadless));
        }
        
        private IEnumerator LoginWithOtherPlatformAsync( PlatformType platformType
            , string platformToken
            , ResultCallback callback
            , bool createHeadless = true )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithOtherPlatform(
                platformType, platformToken, cb, createHeadless), callback);
        }
        
        private IEnumerator LoginWithOtherPlatformAsync( PlatformType platformType
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless = true )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithOtherPlatform(
                platformType, platformToken, cb, createHeadless), callback);
        }

        /// <summary>
        /// Login with token from non AccelByte platforms, especially to support OIDC (with 2FA enable)
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="createHeadless">Set it to true  because it doesn't have username yet </param>
        public void LoginWithOtherPlatformId(string platformId
            , string platformToken
            , ResultCallback callback
            , bool createHeadless = true)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithOtherPlatformIdAsync(platformId, platformToken, callback, createHeadless));
        }

        /// <summary>
        /// Login with token from non AccelByte platforms, especially to support OIDC (with 2FA enable)
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Token for other platfrom type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">Set it to true  because it doesn't have username yet </param>
        public void LoginWithOtherPlatformId(string platformId
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless = true)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithOtherPlatformIdAsync(
                platformId,
                platformToken,
                callback,
                createHeadless));
        }
        
        private IEnumerator LoginWithOtherPlatformIdAsync(string platformId
            , string platformToken
            , ResultCallback callback
            , bool createHeadless = true)
        {
            yield return LoginAsync(cb => oAuth2.LoginWithOtherPlatformId(
                platformId, platformToken, cb, createHeadless), callback);
        }
        
        private IEnumerator LoginWithOtherPlatformIdAsync(string platformId
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless = true)
        {
            yield return LoginAsync(cb => oAuth2.LoginWithOtherPlatformId(
                platformId, platformToken, cb, createHeadless), callback);
        }

        /// <summary>
        /// Login With AccelByte Launcher. Use this only if you integrate your game with AccelByte Launcher
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLauncher( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            string authCode = Environment.GetEnvironmentVariable(AuthorizationCodeEnvironmentVariable);

            if (string.IsNullOrEmpty(authCode))
            {
                coroutineRunner.Run(() =>
                {
                    callback.TryError(ErrorCode.InvalidArgument, 
                        "The application was not executed from launcher");
                });
                return;
            }

            coroutineRunner.Run(LoginWithAuthorizationCodeAsync(authCode, callback));
        }

        /// <summary>
        /// Login With AccelByte Launcher. Use this only if you integrate your game with AccelByte Launcher
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLauncher( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            string authCode = Environment.GetEnvironmentVariable(AuthorizationCodeEnvironmentVariable);

            if (string.IsNullOrEmpty(authCode))
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidArgument.ToString(),
                    error_description = "The application was not executed from launcher"
                };
                callback?.TryError(error);
                return;
            }

            coroutineRunner.Run(LoginWithAuthorizationCodeAsync(authCode, callback));
        }
        
        private IEnumerator LoginWithAuthorizationCodeAsync( string authCode
            , ResultCallback callback )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithAuthorizationCode(authCode, cb), callback);
        }
        
        private IEnumerator LoginWithAuthorizationCodeAsync( string authCode
            , ResultCallback<TokenData, OAuthError> callback )
        {
            yield return LoginAsync(cb => oAuth2.LoginWithAuthorizationCodeV3(authCode, cb), callback);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithDeviceId( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithDeviceIdAsync(callback));
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithDeviceId( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithDeviceIdAsync(callback));
        }
        
        private IEnumerator LoginWithDeviceIdAsync( ResultCallback callback )
        {
            yield return LoginAsync(oAuth2.LoginWithDeviceId, callback);
        }
        
        private IEnumerator LoginWithDeviceIdAsync( ResultCallback<TokenData, OAuthError> callback )
        {
            yield return LoginAsync(oAuth2.LoginWithDeviceId, callback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already epired.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLatestRefreshToken( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithLatestRefreshTokenAsync(null, callback));
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already epired.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLatestRefreshToken( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithLatestRefreshTokenAsync(null, callback));
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void LoginWithLatestRefreshToken( string refreshToken
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithLatestRefreshTokenAsync(refreshToken, callback));
        }
        
        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLatestRefreshToken( string refreshToken
            , ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(LoginWithLatestRefreshTokenAsync(refreshToken, callback));
        }
        
        [Obsolete("Instead, use the overload with the extended callback")]
        private IEnumerator LoginWithLatestRefreshTokenAsync( string refreshToken
            , ResultCallback callback )
        {
            if (refreshToken != null)
            {
                userSession.ForceSetTokenData(new TokenData { refresh_token = refreshToken });
                yield return oAuth2.RefreshSession(userSession.refreshToken, callback);
            }
            else if (File.Exists(UserSession.TokenPath))
            {
                if (userSession.localTokenData == null)
                {
                    userSession.LoadRefreshToken();
                }

                if (userSession.localTokenData == null)
                {
                    callback.TryError(ErrorCode.InvalidRequest, "Refresh token not found!");
                }
                else
                {
                    yield return oAuth2.RefreshSession(userSession.refreshToken, callback);
                }
            }
            else
            {
                callback.TryError(ErrorCode.InvalidRequest, "Refresh Token is null or PlayerPrefs is disabled!");
            }
        }

        private IEnumerator LoginWithLatestRefreshTokenAsync( string refreshToken
            , ResultCallback<TokenData, OAuthError> callback )
        {
            if (refreshToken != null)
            {
                userSession.ForceSetTokenData(new TokenData { refresh_token = refreshToken });
                yield return oAuth2.RefreshSession(userSession.refreshToken, callback);
            }
            else if (File.Exists(UserSession.TokenPath))
            {
                if (userSession.localTokenData == null)
                {
                    userSession.LoadRefreshToken();
                }

                if (userSession.localTokenData == null)
                {
                    OAuthError error = new OAuthError()
                    {
                        error = ErrorCode.InvalidRequest.ToString(),
                        error_description = "Refresh token not found!"
                    };
                    callback.TryError(error);
                }
                else
                {
                    yield return oAuth2.RefreshSession(userSession.refreshToken, callback);
                }
            }
            else
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "Refresh Token is null or PlayerPrefs is disabled!"
                };
                callback.TryError(error);
            }
        }

        /// <summary>
        /// Refresh current login session. Will update current token.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void RefreshSession( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(oAuth2.RefreshSession(userSession.refreshToken, callback));
        }


        /// <summary>
        /// Refresh current login session. Will update current token.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void RefreshSession( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(oAuth2.RefreshSession(userSession.refreshToken, callback));
        }

        /// <summary>
        /// Logout current user session. Access tokens, user ID, and other credentials from memory will be removed.
        /// </summary>
        public void Logout( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryOk();
                return;
            }

            userDataCache = null;
            coroutineRunner.Run(oAuth2.Logout(userSession.AuthorizationToken,
                result=>
            {
                userSession.ClearSession();
                callback.Invoke(result);
            }));
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="emailAddress">Email address of the user</param>
        /// <param name="password">Password to login</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Usernam</param>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void Register( string emailAddress
            , string password
            , string displayName
            , string country
            , DateTime dateOfBirth
            , ResultCallback<RegisterUserResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var registerUserRequest = new RegisterUserRequest
            {
                authType = AuthenticationType.EMAILPASSWD,
                emailAddress = emailAddress,
                password = password,
                displayName = displayName,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd")
            };

            coroutineRunner.Run(api.Register(registerUserRequest, callback));
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
        public void Registerv2( string emailAddress
            , string username
            , string password
            , string displayName
            , string country
            , DateTime dateOfBirth
            , ResultCallback<RegisterUserResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
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

            coroutineRunner.Run(api.Registerv2(registerUserRequest, callback));
        }

        /// <summary>
        /// Register a user while optionally accepting legal policies, password, and displayName 
        /// </summary>
        /// <param name="request">To accept policies, fill acceptedPolicies field</param>
        /// <param name="callback">Returns a Result that contains RegisterUserResponse via callback</param>
        public void RegisterAndAcceptPolicies( RegisterUserRequestv2 request
            , ResultCallback<RegisterUserResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            
            //authType other than EMAILPASSWD is not supported
            request.authType = AuthenticationType.EMAILPASSWD;
            Assert.IsTrue(
                Regex.IsMatch(request.dateOfBirth, "^\\d{4}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$"),
                "Date of birth format is yyyy-MM-dd");

            coroutineRunner.Run(api.Registerv2(request, callback));
        }
        
        /// <summary>
        /// Get current logged in user data. It will return cached user data if it has been called before
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void GetData( ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(GetDataAsync(callback));
        }
        
        private IEnumerator GetDataAsync( ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (userDataCache != null)
            {
                callback.TryOk(userDataCache);
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
        public void RefreshData( ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            userDataCache = null;

            coroutineRunner.Run(RefreshDataAsync(callback));
        }
        
        private IEnumerator RefreshDataAsync( ResultCallback<UserData> callback )
        {
            Result<UserData> result = null;

            yield return api.GetData(r => result = r);

            if (!result.IsError)
            {
                userDataCache = result.Value;

                callback.TryOk(userDataCache);

                yield break;
            }

            callback.Try(result);
        }

        /// <summary>
        /// Update some user information (e.g. language or country)
        /// </summary>
        /// <param name="updateRequest">Set its field if you want to change it, otherwise leave it</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Update( UpdateUserRequest updateRequest
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(UpdateAsync(updateRequest, callback));
        }

        private IEnumerator UpdateAsync( UpdateUserRequest updateRequest
            , ResultCallback<UserData> callback )        
        {
            Result<UserData> updateResult = null;

            yield return api.Update(updateRequest, result => updateResult = result);

            if (!updateResult.IsError)
            {
                userDataCache = updateResult.Value;
            }

            callback.Try(updateResult);
        }

        /// <summary>
        /// Update user email address
        /// </summary>
        /// <param name="updateEmailRequest">Set verify code and user new email on the body</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void UpdateEmail( UpdateEmailRequest updateEmailRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.UpdateEmail(updateEmailRequest, callback));
        }

        /// <summary>
        /// Upgrade a headless account with username and password. User must be logged in before this method can be
        /// used.
        /// </summary>
        /// <param name="userName">Username the user is upgraded to</param>
        /// <param name="password">Password to login with username</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Upgrade( string userName
            , string password
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(UpgradeAsync(userName, password, callback));
        }

        private IEnumerator UpgradeAsync( string username
            , string password
            , ResultCallback<UserData> callback )
        {
            Result<UserData> result = null;

            yield return api.Upgrade(username, password, r => result = r);

            if (!result.IsError)
            {
                userDataCache = result.Value;
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
        public void Upgradev2( string emailAddress
            , string userName
            , string password
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(Upgradev2Async(emailAddress, userName, password, callback));
        }

        private IEnumerator Upgradev2Async( string emailAddress
            , string username
            , string password
            , ResultCallback<UserData> callback )
        {
            Result<UserData> result = null;

            yield return api.Upgradev2(emailAddress, username, password, r => result = r);

            if (!result.IsError)
            {
                userDataCache = result.Value;
            }

            callback.Try(result);
        }

        /// <summary>
        /// Upgrade a headless account. User must be logged in first then call
        /// SendUpgradeVerificationCode code to get verification code send to their email 
        /// </summary>
        /// <param name="upgradeAndVerifyHeadlessRequest">Contain user data that will be used to upgrade the headless account</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void UpgradeAndVerifyHeadlessAccount( UpgradeAndVerifyHeadlessRequest upgradeAndVerifyHeadlessRequest
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.UpgradeAndVerifyHeadlessAccount(upgradeAndVerifyHeadlessRequest, callback));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in with headless account.
        /// This function context is "upgradeHeadlessAccount".
        /// </summary>
        /// <param name="emailAddress">The email use to send verification code</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendUpgradeVerificationCode( string emailAddress
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.SendVerificationCode(
                VerificationContext.upgradeHeadlessAccount,
                emailAddress,
                callback));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// This function context is "UserAccountRegistration". If you want to set your own context, please use the other overload function.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(SendVerificationCodeAsync(VerificationContext.UserAccountRegistration, callback));
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// </summary>
        /// <param name="verifyContext">The context of what verification request</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode( VerificationContext verificationContext
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(SendVerificationCodeAsync(verificationContext, callback));
        }

        private IEnumerator SendVerificationCodeAsync( VerificationContext verificationContext
            , ResultCallback callback )
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

            yield return api.SendVerificationCode(
                verificationContext,
                userDataCache.emailAddress,
                callback);
        }

        /// <summary>
        /// Verify a user via an email registered as its username. User must be logged in.
        /// </summary>
        /// <param name="verificationCode">Verification code received from user's email</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void Verify( string verificationCode
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.Verify(verificationCode, "email", callback));
        }

        /// <summary>
        /// Trigger an email that contains reset password code to be sent to user
        /// </summary>
        /// <param name="userName">Username to be sent reset password code to.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendResetPasswordCode(string userName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.SendPasswordResetCode(userName, callback));
        }

        /// <summary>
        /// Reset password for a username
        /// </summary>
        /// <param name="resetCode">Reset password code</param>
        /// <param name="userName">Username with forgotten password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ResetPassword( string resetCode
            , string userName
            , string newPassword
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.ResetPassword(resetCode, userName, newPassword, callback));
        }

        /// <summary>
        /// Link other platform's account to the currently logged in user. 
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformTicket">Ticket / token from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LinkOtherPlatform( PlatformType platformType
            , string platformTicket
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.LinkOtherPlatform(platformType, platformTicket, callback));
        }

        /// <summary>
        /// Force to Link other platform's account to the currently logged in user. 
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="platformUserId"> UserId from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ForcedLinkOtherPlatform( PlatformType platformType
            , string platformUserId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.ForcedLinkOtherPlatform(platformType, platformUserId, callback));
        }

        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login.
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkOtherPlatform( PlatformType platformType
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.UnlinkOtherPlatform(platformType, callback));
        }

        /// <summary>
        /// Get array of other platforms this user linked to
        /// </summary>
        /// <param name="callback">Returns a Result that contains PlatformLink array via callback when
        /// completed.</param>
        public void GetPlatformLinks( ResultCallback<PagedPlatformLinks> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetPlatformLinks(userSession.UserId, callback));
        }

        /// <summary>
        /// Get user data from another user displayName or username. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query"> Display name or username that needed to get user data.</param>
        /// <param name="by"> Filter the responded PagedPublicUsersInfo by SearchType. Choose the SearchType.ALL if you want to be responded with all query type.</param>
        /// <param name="callback"> Return a Result that contains UsersData when completed. </param>
        /// <param name="limit"> Targeted offset query filter. </param>
        /// <param name="offset"> Targeted offset query filter. </param>
        public void SearchUsers( string query
            , SearchType by
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset = 0 
            , int limit = 100 )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.SearchUsers(query, by, callback, offset, limit));
        }

        /// <summary>
        /// Get user data from another user by displayName or username. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query"> Display name or username that needed to get user data.</param>
        /// <param name="callback"> Return a Result that contains UsersData when completed. </param>
        /// <param name="limit"> Targeted offset query filter. </param>
        /// <param name="offset"> Targeted offset query filter. </param>
        public void SearchUsers( string query
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset = 0
            , int limit = 100)
        {
            SearchUsers(query, SearchType.ALL, callback, offset, limit);
        }

        /// <summary>
        /// Get user data from another user by user id
        /// </summary>
        /// <param name="userId"> user id that needed to get user data</param>
        /// <param name="callback"> Return a Result that contains UserData when completed. </param>
        public void GetUserByUserId( string userId
            , ResultCallback<PublicUserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetUserByUserId(userId, callback));
        }

        /// <summary>
        /// Get other user data by other platform userId (such as SteamID, for example)
        /// For Nintendo Platform you need to append Environment ID into the Platorm ID, with this format PlatformID:EnvironmentID. e.g csgas12312323f:dd1
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="otherPlatformUserId">Platform UserId that needed to get user data</param>
        /// <param name="callback">Return a Result that contains UserData when completed.</param>
        public void GetUserByOtherPlatformUserId( PlatformType platformType
            , string otherPlatformUserId
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserByOtherPlatformUserId(platformType, otherPlatformUserId, callback));
        }

        /// <summary>
        /// Get other user data by other platform userId(s) (such as SteamID, for example)
        /// For Nintendo Platform you need to append Environment ID into the Platorm ID, with this format PlatformID:EnvironmentID. e.g csgas12312323f:dd1
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="otherPlatformUserId">Platform UserId that needed to get user data</param>
        /// <param name="callback">Return a Result that contains UserData when completed.</param>
        public void BulkGetUserByOtherPlatformUserIds( PlatformType platformType
            , string[] otherPlatformUserId
            , ResultCallback<BulkPlatformUserIdResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            BulkPlatformUserIdRequest platformUserIds = new BulkPlatformUserIdRequest { platformUserIDs = otherPlatformUserId };
            coroutineRunner.Run(
                api.BulkGetUserByOtherPlatformUserIds(platformType, platformUserIds, callback));
        }
        
        /// <summary>
        /// Get spesific country from user IP
        /// </summary>
        /// <param name="callback"> Returns a Result that contains country information via callback when completed</param>
        public void GetCountryFromIP( ResultCallback<CountryInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.GetCountryFromIP(callback));
        }

        /// <summary>
        /// Check if user has purchased the subscription and eligible to play
        /// </summary>
        /// <param name="callback"> Returns the boolean result whether the user is subscribed and eligible to play the game via callback when the operation is completed</param>
        public void GetUserEligibleToPlay( ResultCallback<bool> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            ResultCallback<ItemInfo> onGotItemInfo = itemInfoResult =>
                {
                    if(itemInfoResult.IsError)
                    {
                        callback.TryError(itemInfoResult.Error.Code);
                        return;
                    }

                    string[] skus = itemInfoResult.Value.features;
                    string[] appIds = { AccelBytePlugin.Config.AppId };

                    AccelBytePlugin.GetEntitlement().GetUserEntitlementOwnershipAny(null, appIds, skus, ownershipResult =>
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

        public void RefreshTokenCallback( Action<string> refreshTokenCallback )
        {
            userSession.RefreshTokenCallback += refreshTokenCallback;
        }

        /// <summary>
        /// Get multiple user(s) information like user's DisplayName.
        /// </summary>
        /// <param name="userIds">List UserId(s) to get.</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void BulkGetUserInfo( string[] userIds
            , ResultCallback<ListBulkUserInfoResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.BulkGetUserInfo(userIds, callback));
        }

        /// <summary>
        /// Verify 2FA Code 
        /// </summary>
        /// <param name="mfaToken">Multi-factor authentication Token</param>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="code">Verification code</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="rememberDevice">Will record device token when true</param>
        public void Verify2FACode( string mfaToken
            , TwoFAFactorType factor
            , string code
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberDevice = false )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(Verify2FACodeAsync(mfaToken, factor, code, callback, rememberDevice));
        }

        private IEnumerator Verify2FACodeAsync( string mfaToken
            , TwoFAFactorType factor
            , string code
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberDevice = false )
        {
            if (userSession.IsValid())
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "User is already logged in."
                };
                callback.TryError(error);

                yield break;
            }

            yield return oAuth2.Verify2FACode(mfaToken, factor, code, callback, rememberDevice);
        }

        /// <summary>
        ///  OAuth2 token verification API 
        /// </summary>
        /// <param name="callback"></param>
        public void VerifyToken(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(VerifyTokenAsync(callback));
        }

        private IEnumerator VerifyTokenAsync(ResultCallback callback)
        {
            if (!userSession.IsValid())
            {
                const string errorMessage = "User is not log in.";
                callback.TryError(new Error(ErrorCode.InvalidRequest, errorMessage));
                yield break;
            }
            yield return oAuth2.VerifyToken(Session.AuthorizationToken, callback);
        }

        /// <summary>
        /// Change 2FA Factor 
        /// </summary>
        /// <param name="mfaToken">Multi-factor authentication Token</param>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Change2FAFactor( string mfaToken
            , TwoFAFactorType factor
            , ResultCallback<TokenData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.Change2FAFactor(mfaToken, factor, callback));
        }

        /// <summary>
        /// Disable 2FA Authenticator
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Disable2FAAuthenticator( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            TwoFAEnable = false;
            coroutineRunner.Run(api.Disable2FAAuthenticator(callback));
        }

        /// <summary>
        /// Enable 2FA Authenticator, to enable the backup code 2FA factor, you should also call Enable2FABackupcodes.
        /// </summary>
        /// <param name="code">Verification code</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Enable2FAAuthenticator( string code
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            TwoFAEnable = true;
            coroutineRunner.Run(api.Enable2FAAuthenticator(code, callback));
        }

        /// <summary>
        /// Generate Secret Key For 3rd Party Authenticate Application 
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GenerateSecretKeyFor3rdPartyAuthenticateApp( ResultCallback<SecretKey3rdPartyApp> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.GenerateSecretKeyFor3rdPartyAuthenticateApp(callback));
        }

        /// <summary>
        /// Generate 2FA BackUp Code, will give a new list of new backup code and make codes generated before invalid.
        /// </summary>
        /// <param name="callback"></param>
        public void GenerateBackUpCode( ResultCallback<TwoFACode> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            } 

            coroutineRunner.Run(api.GenerateBackUpCode(callback));
        }

        /// <summary>
        /// Disable 2FA Backup Codes
        /// </summary>
        /// <param name="callback"></param>
        public void Disable2FABackupCodes( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

			TwoFAEnable = false;
            coroutineRunner.Run(api.Disable2FABackupCodes(callback));
        }

        /// <summary>
        /// Enable 2FA Backup Codes, this should be called if the 2FA not only using authenticator/3rd party factor. 
        /// </summary>
        /// <param name="callback"></param>
        public void Enable2FABackupCodes( ResultCallback<TwoFACode> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

			TwoFAEnable = true;
            coroutineRunner.Run(api.Enable2FABackupCodes(callback));
        }

        /// <summary>
        /// Get 2FA BackUp Code
        /// </summary>
        /// <param name="callback"></param>
        public void GetBackUpCode( ResultCallback<TwoFACode> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.GetBackUpCode(callback));
        }

        /// <summary>
        /// Get User Enabled Factors
        /// </summary>
        /// <param name="callback"></param>
        public void GetUserEnabledFactors( ResultCallback<Enable2FAFactors> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.GetUserEnabledFactors(callback));
        }

        /// <summary>
        /// Make 2FA Factor Default
        /// </summary>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Make2FAFactorDefault( TwoFAFactorType factor
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.Make2FAFactorDefault(factor, callback));
        }

        /// <summary>
        /// Get IAM Input Validation 
        /// </summary>
        /// <param name="languageCode">Language Code for description</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="defaultOnEmpty">will return default language if languageCode is empty or language not available</param>
        public void GetInputValidations( string languageCode
            , ResultCallback<InputValidation> callback
            , bool defaultOnEmpty = true )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.GetInputValidations(languageCode, callback, defaultOnEmpty));
        }

        /// <summary>
        /// Update current user 
        /// </summary>
        /// <param name="updateUserRequest">Update user request variables to be updated</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void UpdateUser(UpdateUserRequest updateUserRequest
            , ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(api.UpdateUser(updateUserRequest, callback));
        }

        /// <summary>
        /// Create Headless Account for Account Linking
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void CreateHeadlessAccountAndResponseToken(string linkingToken
            , bool extendExp
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(CreateHeadlessAccountAndResponseTokenAsync(linkingToken, extendExp, callback));
        }

        /// <summary>
        /// Create Headless Account for Account Linking
        /// </summary>
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void CreateHeadlessAccountAndResponseToken(string linkingToken
            , bool extendExp
            , ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(CreateHeadlessAccountAndResponseTokenAsync(
                linkingToken,
                extendExp,
                callback));
        }

        /// <summary>
        /// Create Headless Account for Account Linking
        /// </summary>
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <returns></returns>
        private IEnumerator CreateHeadlessAccountAndResponseTokenAsync(string linkingToken
            , bool extendExp
            , ResultCallback callback)
        {
            yield return LoginAsync(cb => oAuth2.CreateHeadlessAccountAndResponseToken(
                linkingToken, extendExp, cb), callback);
        }

        private IEnumerator CreateHeadlessAccountAndResponseTokenAsync(string linkingToken
            , bool extendExp
            , ResultCallback<TokenData, OAuthError> callback)
        {
            yield return LoginAsync(cb => oAuth2.CreateHeadlessAccountAndResponseToken(
                linkingToken, extendExp, cb), callback);
        }

        /// <summary>
        /// Authentication With PlatformLink for Account Linking
        /// </summary>
        /// <param name="username">Username to login</param>
        /// <param name="password">Password to login</param>
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void AuthenticationWithPlatformLink(string username
            , string password
            , string linkingToken
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(AuthenticationWithPlatformLinkAsync(username, password, linkingToken, callback));
        }

        /// <summary>
        /// Authentication With PlatformLink for Account Linking
        /// </summary>
        /// <param name="username">Username to login</param>
        /// <param name="password">Password to login</param>
        /// /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void AuthenticationWithPlatformLink(string username
            , string password
            , string linkingToken
            , ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(AuthenticationWithPlatformLinkAsync(
                username,
                password,
                linkingToken,
                callback));
        }

        /// <summary>
        /// Request the Avatar of the given UserProfile
        /// </summary>
        /// <param name="userId">The UserId of a public Profile</param>
        /// <param name="callback">Returns a result that contains a <see cref="Texture2D"/></param>
        public void GetUserAvatar(string userId
            , ResultCallback<Texture2D> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            } 

            GetUserByUserId(userId, result =>
            {
                if (result.IsError)
                {
                    Debug.LogError(
                        $"Unable to get Bulk Get User Info Code:{result.Error.Code} Message:{result.Error.Message}");
                    callback.TryError(result.Error);
                }
                else
                {
                    if (string.IsNullOrEmpty(result.Value.avatarUrl))
                    {
                        callback.TryError(new Error(ErrorCode.GameRecordNotFound, "avatarUrl value is null or empty"));
                    }
                    coroutineRunner.Run(ABUtilities.DownloadTexture2D(result.Value.avatarUrl, callback));
                }
            });
        }

        /// <summary>
        /// Authentication With PlatformLink for Account Linking
        /// </summary>
        /// <param name="username">Username to login</param>
        /// <param name="password">Password to login</param>
        /// /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        private IEnumerator AuthenticationWithPlatformLinkAsync(string username
            , string password
            , string linkingToken
            , ResultCallback callback)
        {
            yield return LoginAsync(cb => oAuth2.AuthenticationWithPlatformLink(
                username, password, linkingToken, cb), callback);
        }

        private IEnumerator AuthenticationWithPlatformLinkAsync(string username
            , string password
            , string linkingToken
            , ResultCallback<TokenData, OAuthError> callback)
        {
            yield return LoginAsync(cb => oAuth2.AuthenticationWithPlatformLink(
                username, password, linkingToken, cb), callback);
        }
        
        /// <summary>
        /// Get Publisher User 
        /// </summary>
        /// <param name="userId"> user id that needed to get publisher user</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetPublisherUser(string userId
            , ResultCallback<GetPublisherUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(api.GetPublisherUser(userId, callback));
        }
        
        /// <summary>
        /// Get User Information 
        /// </summary>
        /// <param name="userId"> user id that needed to get user information</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetUserInformation(string userId
            , ResultCallback<GetUserInformationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(api.GetUserInformation(userId, callback));
        }
        
        /// <summary>
        /// Generate one time linking code
        /// </summary>
        /// <param name="platformId">The platform ID</param>
        /// <param name="callback">Return Result via callback when completed</param>
        public void GenerateOneTimeCode(PlatformType platformId
            , ResultCallback<GeneratedOneTimeCode> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(GenerateOneTimeCodeAsync(platformId, callback));
        }

        private IEnumerator GenerateOneTimeCodeAsync(PlatformType platformId
            , ResultCallback<GeneratedOneTimeCode> callback)
        {
            if (!userSession.IsValid())
            {
                const string errorMessage = "User is not log in.";
                callback.TryError(new Error(ErrorCode.InvalidRequest, errorMessage));
                yield break;
            }
            yield return oAuth2.GenerateOneTimeCode(Session.AuthorizationToken, platformId, callback);
        }
        
        /// <summary>
        /// Generate publisher user's game token. Required a code from request game token
        /// </summary>
        /// <param name="code">code from request game token</param>
        /// <param name="callback">Return Result via callback when completed</param>
        public void GenerateGameToken(string code
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(GenerateGameTokenAsync(code, callback));
        }

        private IEnumerator GenerateGameTokenAsync(string code
            , ResultCallback callback)
        {
            if (userSession.IsValid())
            {
                callback.TryError(ErrorCode.InvalidRequest, 
                    "User is already logged in.");
                yield break;
            }
            yield return oAuth2.GenerateGameToken(code, callback);
        }
        
        /// <summary>
        /// Link headless account to current full account
        /// </summary>
        /// <param name="linkHeadlessAccountRequest"> struct that containing chosen namespace and one time link code</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void LinkHeadlessAccountToCurrentFullAccount(LinkHeadlessAccountRequest linkHeadlessAccountRequest
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(api.LinkHeadlessAccountToCurrentFullAccount(linkHeadlessAccountRequest, callback));
        }
        
        /// <summary>
        /// Get conflict result when link headless account to current account by one time code
        /// </summary>
        /// <param name="oneTimeLinkCode"> One time link code value</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetConflictResultWhenLinkHeadlessAccountToFullAccount(string oneTimeLinkCode
            , ResultCallback<ConflictLinkHeadlessAccountResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(api.GetConflictResultWhenLinkHeadlessAccountToFullAccount(oneTimeLinkCode, callback));
        }
    }
}
