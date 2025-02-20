// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;
using System.Text.RegularExpressions;
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
    public partial class User : WrapperBase
    {
        //Constants
        internal const string AuthorizationCodeEnvironmentVariable = "JUSTICE_AUTHORIZATION_CODE";
        public const int MaxNumOfBulkGetUserByOtherPlatformUserId = 100;

#if UNITY_SWITCH && !UNITY_EDITOR
        internal static readonly string DefaultPlatformCacheDirectory = "AccelByte/PlatformLoginCache/";
#else
        internal static readonly string DefaultPlatformCacheDirectory = $"{GameCommonInfo.PersistentPath}/AccelByte/PlatformLoginCache/{GameCommonInfo.ProductName}";
#endif

        internal Action<TokenData> OnLoginSuccess;
        internal Action OnLogout;

        //Readonly members
        private readonly UserSession userSession;//renamed from LoginSession
        public readonly OAuth2 oAuth2;
        private readonly UserApi api;
        private readonly LoginQueueApi loginQueueApi;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly CoroutineRunner coroutineRunner;
#pragma warning restore IDE0052 // Remove unread private members

        public UserSession Session { get { return userSession; } }

        private UserData userDataCache;

        public bool TwoFAEnable { get; private set; } = false;

        /// <summary>
        /// User class constructor
        /// </summary>
        /// <param name="inLoginSession">
        /// UserSession; not ISession (unlike similar modules like this)
        /// </param>
        /// <param name="inApi"></param>
        /// <param name="inCoroutineRunner"></param>
        [UnityEngine.Scripting.Preserve]
        internal User( UserApi inApi
            , UserSession inLoginSession
            , CoroutineRunner inCoroutineRunner)
        {
            userSession = inLoginSession;
            api = inApi;
            coroutineRunner = inCoroutineRunner;

            oAuth2 = new OAuth2(
                inApi.HttpClient,
                inApi.Config,
                userSession);

            loginQueueApi = new LoginQueueApi(
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
        [Obsolete("For pattern parity, to conform with other classes, use the overload that starts with api param"), UnityEngine.Scripting.Preserve]
        internal User( UserSession inLoginSession
            , UserApi inApi
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inLoginSession, inCoroutineRunner)
        {
            // Curry this obsolete data to the new overload ->
        }
        
        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            base.SetSharedMemory(newSharedMemory);
            oAuth2?.SetSharedMemory(newSharedMemory);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithUserName(username, password, callback, rememberMe);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithUserName(username, password, callback, rememberMe);
        }

        /// <summary>
        /// Login to AccelByte account with username (or email) and password using V3 endpoint
        /// </summary>
        /// <param name="username">Could be username or email</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete]
        public void LoginWithUsernameV3( string username
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithUserNameV3(username, password, callback, rememberMe);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithUserNameV3(username, password, callback, rememberMe);
        }
        
        private void Login(System.Action<ResultCallback> loginMethod
            , Action<Error> onAlreadyLogin
            , Action<Error> onLoginFailed
            , Action onLoginSuccess)
        {
            if (userSession.IsValid())
            {
                var error = new Error(ErrorCode.InvalidRequest, "User is already logged in.");
                onAlreadyLogin?.Invoke(error);
                return;
            }

            loginMethod(loginResult =>
            {
                TriggerLoginResult(loginResult, onLoginFailed, onLoginSuccess);
            });
        }

        internal virtual void Login(
            System.Action<ResultCallback<TokenData, OAuthError>> loginMethod,
            Action<OAuthError> onAlreadyLogin,
            Action<OAuthError> onLoginFailed,
            Action<TokenData> onLoginSuccess
        )
        {
            if (userSession.IsValid())
            { 
                var error = new OAuthError() 
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "User is already logged in."
                };

                onAlreadyLogin?.Invoke(error);
                return;
            }

            loginMethod(loginResult =>
            {
                TriggerLoginResult(loginResult, onLoginFailed, onLoginSuccess);
            });
        }

        internal virtual void Login(
            System.Action<ResultCallback<TokenDataV4, OAuthError>> loginMethod,
            Action<OAuthError> onAlreadyLogin,
            Action<OAuthError> onLoginFailed,
            Action<TokenDataV4> onLoginCompleted
        )
        {
            if (userSession.IsValid())
            { 
                var error = new OAuthError() 
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "User is already logged in."
                };

                onAlreadyLogin?.Invoke(error);
                return;
            }

            loginMethod(loginResult =>
            {
                TriggerLoginResult(loginResult, onLoginFailed, onLoginCompleted);
            });
        }

        protected void TriggerLoginResult(Result loginResult, Action<Error> onLoginFailed, Action onLoginSuccess)
        {
            if (loginResult.IsError)
            {
                onLoginFailed?.Invoke(loginResult.Error);
            }
            else
            {
                onLoginSuccess?.Invoke();
            }
        }

        protected void TriggerLoginResult<T,U>(Result<T, U> loginResult, Action<U> onLoginFailed, Action<T> onLoginSuccess)
        {
            if (loginResult.IsError)
            {
                onLoginFailed?.Invoke(loginResult.Error);
            }
            else
            {
                onLoginSuccess?.Invoke(loginResult.Value);
            }
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password.
        /// </summary>
        /// <param name="email">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete("Instead, use LoginWithUserNameV3()")]
        private void LoginWithUserName(string email
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Login using username is deprecated, please use email for the replacement.");
            }

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };
            Login(cb => oAuth2.LoginWithUsername(email, password, cb, rememberMe)
                , onAlreadyLogin
                , onLoginFailed
                , onLoginSuccess);
        }

        /// <summary>
        /// Login to AccelByte account with username (e.g. email) and password.
        /// </summary>
        /// <param name="email">Could be email or phone (right now, only email supported)</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [Obsolete("Instead, use LoginWithUserNameV3()")]
        private void LoginWithUserName(string email
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false)
        {
            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Login using username is deprecated, please use email for the replacement.");
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess)=>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };
            Login(cb => oAuth2.LoginWithUsername(email, password, cb, rememberMe)
                , onAlreadyLogin
                , onLoginFailed
                , onLoginSuccess);
        }

        [Obsolete("Instead, use LoginWithUserNameV3() which use different callback type")]
        private void LoginWithUserNameV3( string email
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Login using username is deprecated, please use email for the replacement.");
            }

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveAuthTrustId(onDone: isSuccess =>
                {
                    Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(Session.TokenData);
                        SendLoginSuccessPredefinedEventFromCurrentSession();
                        callback.TryOk();
                    });
                });
                
            };
            Session.LoadAuthTrustId((isSuccess, authTrustId) =>
            {
                Login(cb => oAuth2.LoginWithUsernameV3(email, password, cb, rememberMe, authTrustId)
                    , onAlreadyLogin
                    , onLoginFailed
                    , onLoginSuccess);
            });
        }
        
        private void LoginWithUserNameV3( string email
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Login using username is deprecated, please use email for the replacement.");
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveAuthTrustId(isSuccess =>
                {
                    Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                        callback.TryOk(tokenData);
                    });
                });
            };
            Session.LoadAuthTrustId((isSuccess, authTrustId) =>
            {
                Login(cb => oAuth2.LoginWithUsernameV3(email, password, cb, rememberMe, authTrustId)
                    , onAlreadyLogin
                    , onLoginFailed
                    , onLoginSuccess);
            });
        }

        /// <summary>
        /// Login with token from non AccelByte platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void LoginWithOtherPlatformId( PlatformType platformType
            , string platformToken
            , ResultCallback callback
            , bool createHeadless = true )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            string platformId = platformType.ToString().ToLower();
            LoginWithOtherPlatformId(platformId, platformToken, callback, createHeadless);
        }

        /// <summary>
        /// Login with token from non AccelByte platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        public void LoginWithOtherPlatform(PlatformType platformType
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless = true
            , LoginWithMacAddress loginWithMacAddress = null)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            string platformId = platformType.ToString().ToLower();
            LoginWithOtherPlatformId(platformId: platformId
                , platformToken: platformToken
                , callback: callback
                , createHeadless: createHeadless
                , loginWithMacAddress: loginWithMacAddress);
        }

        /// <summary>
        /// Login with token from PS4/PS5 platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">Set it to true  because it doesn't have username yet </param>
        /// <param name="serviceLabel">(Early-access: for PS5 only currently)Used to validate PSN app when AppId is set on Admin Portal for PS4/PS5</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        [AccelByte.Utils.Attributes.AccelBytePreview]
        public void LoginWithOtherPlatform(PlatformType platformType
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless
            , string serviceLabel
            , LoginWithMacAddress loginWithMacAddress = null)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            string platformId = platformType.ToString().ToLower();
#pragma warning disable AB0001
            LoginWithOtherPlatformId(platformId, platformToken, callback, createHeadless, serviceLabel, loginWithMacAddress);
#pragma warning restore AB0001
        }

        public void ReloginWithOtherPlatform(PlatformType platformType, ResultCallback<TokenData, OAuthError> callback)
        {
            LoginWithCachedRefreshToken(platformType.ToString().ToLower(), callback);
        }

        public void ReloginWithOtherPlatform(string platformId, ResultCallback<TokenData, OAuthError> callback)
        {
            LoginWithCachedRefreshToken(platformId, callback);
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
        [Obsolete]
        public void LoginWithOtherPlatformId(string platformId
            , string platformToken
            , ResultCallback callback
            , bool createHeadless = true)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, platformId);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(platformId, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };

            Login(cb =>
                {
                    oAuth2.LoginWithOtherPlatformId(platformId, platformToken, cb, createHeadless);
                }
                , onAlreadyLogin
                , onLoginFailed
                , onLoginSuccess);
        }

        /// <summary>
        /// Login with token from non AccelByte platforms, especially to support OIDC (with 2FA enable)
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">Set it to true  because it doesn't have username yet </param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        public void LoginWithOtherPlatformId(string platformId
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless = true
            , LoginWithMacAddress loginWithMacAddress = null)
        {
#pragma warning disable AB0001
            LoginWithOtherPlatformId(platformId: platformId
                , platformToken: platformToken
                , callback: callback
                , createHeadless: createHeadless
                , serviceLabel: null
                , loginWithMacAddress: loginWithMacAddress);
#pragma warning restore AB0001
        }
        
        /// <summary>
        /// Login with token from non AccelByte platforms, especially to support OIDC (with 2FA enable)
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">Set it to true  because it doesn't have username yet </param>
        /// <param name="serviceLabel">(Early-access: for PS5 only currently)Used to validate PSN app when AppId is set on Admin Portal for PS4/PS5</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        [AccelByte.Utils.Attributes.AccelBytePreview]
        public void LoginWithOtherPlatformId(string platformId
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback
            , bool createHeadless
            , string serviceLabel
            , LoginWithMacAddress loginWithMacAddress = null)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, platformId);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(platformId, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };

            Login(
                cb =>
                {
                    oAuth2.LoginWithOtherPlatformId(platformId, platformToken, cb, createHeadless, serviceLabel, loginWithMacAddress);
                }
                , onAlreadyLogin
                , onLoginFailed
                , onLoginSuccess
            );
        }

        /// <summary>
        /// Login With AccelByte Launcher. Use this only if you integrate your game with AccelByte Launcher
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void LoginWithLauncher( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            string authCode = Environment.GetEnvironmentVariable(AuthorizationCodeEnvironmentVariable);

            if (string.IsNullOrEmpty(authCode))
            {
                callback.TryError(ErrorCode.InvalidArgument, "The application was not executed from launcher");
                return;
            }

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(authCode, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };

            Login(cb =>
            {
                oAuth2.LoginWithAuthorizationCode(authCode, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Login With AccelByte Launcher. Use this only if you integrate your game with AccelByte Launcher
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLauncher( ResultCallback<TokenData, OAuthError> callback )
        {
            string authCode = Environment.GetEnvironmentVariable(AuthorizationCodeEnvironmentVariable);
            LoginWithLauncher(authCode, callback);
        }

        internal void LoginWithLauncher(string authCode, ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
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

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(authCode, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };

            Login(cb =>
            {
                oAuth2.LoginWithAuthorizationCodeV3(authCode, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void LoginWithDeviceId( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(Session.PlatformUserId, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };

            Login(cb =>
            {
                oAuth2.LoginWithDeviceId(cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithDeviceId( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(tokenData.platform_user_id, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };

            Login(cb =>
            {
                oAuth2.LoginWithDeviceId(cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already epired.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void LoginWithLatestRefreshToken( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithCachedRefreshToken(UserSession.LastLoginUserCacheKey, callback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already epired.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLatestRefreshToken( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithCachedRefreshToken(UserSession.LastLoginUserCacheKey, callback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete("Instead, use the overload with the extended callback")]
        public void LoginWithLatestRefreshToken( string refreshToken
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                LoginWithRefreshToken(refreshToken, UserSession.LastLoginUserCacheKey, callback);
            }
            else
            {
                LoginWithCachedRefreshToken(UserSession.LastLoginUserCacheKey, callback);
            }
        }
        
        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLatestRefreshToken( string refreshToken
            , ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                LoginWithRefreshToken(refreshToken, UserSession.LastLoginUserCacheKey, callback);
            }
            else
            {
                LoginWithCachedRefreshToken(UserSession.LastLoginUserCacheKey, callback);
            }
        }

        [Obsolete]
        private void LoginWithCachedRefreshToken(string cacheKey, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            userSession.GetRefreshTokenFromCache(cacheKey, (refreshTokenData) =>
            {
                if (refreshTokenData == null)
                {
                    callback.TryError(ErrorCode.UnableToSerializeDeserializeCachedToken, $"Failed to find token cache file.");
                    return;
                }

                DateTime refreshTokenExpireTime = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(refreshTokenData.expiration_date);
                if (DateTime.UtcNow >= refreshTokenExpireTime)
                {
                    callback.TryError(ErrorCode.CachedTokenExpired, $"Cached token is expired");
                    return;
                }

                LoginWithRefreshToken(cacheKey, cacheKey, callback);
            });
        }

        /// <summary>
        /// Login with refresh token from local cache file
        /// </summary>
        /// <param name="cacheKey">Login unique cache name</param>
        public void LoginWithCachedRefreshToken(string cacheKey
            , ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            TriggerLoginWithCachedRefreshToken(cacheKey, callback);
        }

        protected virtual void TriggerLoginWithCachedRefreshToken(string cacheKey
            , ResultCallback<TokenData, OAuthError> callback)
        {
            userSession.GetRefreshTokenFromCache(cacheKey, (refreshTokenData) =>
            {
                if (refreshTokenData == null)
                {
                    var newError = new OAuthError
                    {
                        error = ErrorCode.CachedTokenNotFound.ToString(),
                        error_description = $"Failed to find token cache file."
                    };
                    callback.TryError(newError);
                    return;
                }

                DateTime refreshTokenExpireTime = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(refreshTokenData.expiration_date);
                if (DateTime.UtcNow >= refreshTokenExpireTime)
                {
                    var newError = new OAuthError
                    {
                        error = ErrorCode.CachedTokenExpired.ToString(),
                        error_description = $"Cached token is expired"
                    };
                    callback.TryError(newError);
                    return;
                }

                LoginWithRefreshToken(refreshTokenData.refresh_token, cacheKey, callback);
            });
        }

        [Obsolete]
        private void LoginWithRefreshToken(string refreshToken, string cacheKey, ResultCallback callback)
        {
            Assert.IsNotNull(refreshToken, "Refresh token is null");
            Assert.AreNotEqual(string.Empty, refreshToken, "Refresh token is empty");

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(cacheKey, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };

            Login(cb =>
            {
                oAuth2.RefreshSession(refreshToken, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        private void LoginWithRefreshToken(string refreshToken, string cacheKey, ResultCallback<TokenData, OAuthError> callback)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                callback.TryError(new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "refreshToken cannot be null or empty"
                });
                return;
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(cacheKey, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };

            Login(cb =>
            {
                oAuth2.RefreshSession(refreshToken, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Refresh current login session. Will update current token.
        /// </summary>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void RefreshSession( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            oAuth2.RefreshSession(userSession.refreshToken, callback);
        }

        /// <summary>
        /// Refresh current login session. Will update current token.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        public void RefreshSession( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            oAuth2.RefreshSession(userSession.refreshToken, callback);
        }

        /// <summary>
        /// Refresh current login session. Will update current token.
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void RefreshSession(string refreshToken, ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            oAuth2.RefreshSession(refreshToken, callback);
        }

        /// <summary>
        /// Logout current user session. Access tokens, user ID, and other credentials from memory will be removed.
        /// </summary>
        public void Logout( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback?.TryOk();
                return;
            }

            TriggerLogout((logoutResult) =>
            {
                if (!logoutResult.IsError)
                {
                    userDataCache = null;
                    userSession.ClearSession(true);
                }
                OnLogout?.Invoke();
                callback?.Invoke(logoutResult);
            });
        }

        protected virtual void TriggerLogout(ResultCallback callback)
        {
            oAuth2.Logout(userSession.AuthorizationToken,
               result =>
               {
                   callback?.Invoke(result);
               });
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            var registerUserRequest = new RegisterUserRequest
            {
                authType = AuthenticationType.EMAILPASSWD,
                emailAddress = emailAddress,
                password = password,
                displayName = displayName,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd")
            };

            api.Register(registerUserRequest, callback);
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="emailAddress">Email address of the user, can be used as login username</param>
        /// <param name="password">Password to login, 8 to 32 characters, satisfy at least 3 out of 4 conditions(uppercase, lowercase letters, numbers and special characters) and should not have more than 2 equal characters in a row.</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Username</param>
        /// <param name="country">User's country, ISO3166-1 alpha-2 two letter, e.g. US.</param>
        /// <param name="dateOfBirth">User's date of birth, valid values are between 1905-01-01 until current date.</param>
        /// <param name="uniqueDisplayName">This is required when uniqueDisplayNameEnabled/UNIQUE_DISPLAY_NAME_ENABLED is true.</param>
        /// <param name="callback">Returns a Result that contains RegisterUserResponse via callback</param>
        public void Register(string emailAddress
            , string password
            , string displayName
            , string country
            , DateTime dateOfBirth
            , string uniqueDisplayName
            , ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var registerUserRequest = new RegisterUserRequest
            {
                authType = AuthenticationType.EMAILPASSWD,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd"),
                displayName = displayName,
                emailAddress = emailAddress,
                password = password,
                UniqueDisplayName = uniqueDisplayName
            };

            api.Register(registerUserRequest, callback);
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="emailAddress">Email address of the user, can be used as login username</param>
        /// <param name="username">The username can be used as login username, case insensitive, alphanumeric with allowed symbols underscore (_) and dot (.)</param>
        /// <param name="password">Password to login, 8 to 32 characters, satisfy at least 3 out of 4 conditions(uppercase, lowercase letters, numbers and special characters) and should not have more than 2 equal characters in a row.</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Username</param>
        /// <param name="country">User's country, ISO3166-1 alpha-2 two letter, e.g. US. Use GetCountryV3() to fetch the latest Country list</param>
        /// <param name="dateOfBirth">User's date of birth, valid values are between 1905-01-01 until current date.</param>
        /// <param name="uniqueDisplayName">This is required when uniqueDisplayNameEnabled/UNIQUE_DISPLAY_NAME_ENABLED is true.</param>
        /// <param name="callback">Returns a Result that contains RegisterUserResponse via callback</param>
        public void RegisterV2(string emailAddress
            , string username
            , string password
            , string displayName
            , string country
            , DateTime dateOfBirth
            , string uniqueDisplayName
            , ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var registerUserRequest = new RegisterUserRequestv2
            {
                authType = AuthenticationType.EMAILPASSWD,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd"),
                displayName = displayName,
                emailAddress = emailAddress,
                password = password,
                UniqueDisplayName = uniqueDisplayName,
                username = username
            };

            api.RegisterV2(registerUserRequest, callback);
        }

        /// <summary>
        /// Register a user by giving username, password, and displayName 
        /// </summary>
        /// <param name="emailAddress">Email address of the user, can be used as login username</param>
        /// <param name="username">The username can be used as login username, case insensitive, alphanumeric with allowed symbols underscore (_) and dot (.)</param>
        /// <param name="password">Password to login, 8 to 32 characters, satisfy at least 3 out of 4 conditions(uppercase, lowercase letters, numbers and special characters) and should not have more than 2 equal characters in a row.</param>
        /// <param name="displayName">Any string can be used as display name, make it more flexible than Username</param>
        /// <param name="country">User's country, ISO3166-1 alpha-2 two letter, e.g. US. Use GetCountryV3() to fetch the latest Country list</param>
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            var registerUserRequest = new RegisterUserRequestv2
            {
                authType = AuthenticationType.EMAILPASSWD,
                emailAddress = emailAddress,
                username = username,
                password = password,
                displayName = displayName,
                UniqueDisplayName = displayName,
                country = country,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd")
            };

            api.RegisterV2(registerUserRequest, callback);
        }

        /// <summary>
        /// Register a user while optionally accepting legal policies, password, and displayName 
        /// </summary>
        /// <param name="request">To accept policies, fill acceptedPolicies field</param>
        /// <param name="callback">Returns a Result that contains RegisterUserResponse via callback</param>
        public void RegisterAndAcceptPolicies(RegisterUserRequestv2 request
            , ResultCallback<RegisterUserResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            //authType other than EMAILPASSWD is not supported
            request.authType = AuthenticationType.EMAILPASSWD;
            bool regexMatch = Regex.IsMatch(request.dateOfBirth, "^\\d{4}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$");
            if (!regexMatch)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Date of birth format is yyyy-MM-dd"));
                return;
            }

            api.RegisterV2(request, callback);
        }
        
        /// <summary>
        /// Get current logged in user data. It will return cached user data if it has been called before
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void GetData(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (userDataCache != null)
            {
                callback.TryOk(userDataCache);
            }
            else
            {
                RefreshData(callback);
            }
        }

        /// <summary>
        /// Get current logged in user data with platform data. It will return cached user data if it has been called before
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        public void GetDataWithLinkedPlatform(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (userDataCache != null && userDataCache.PlatformInfos != null)
            {
                callback.TryOk(userDataCache);
            }
            else
            {
                RefreshData(callback, isIncludeAllPlatforms: true);
            }
        }

        /// <summary>
        /// This function will get user basic and public info of 3rd party account
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC).</param>
        /// <param name="userIds">Array of user ids to get information on</param>
        /// <param name="callback">Returns a result that contains users' platform info via callback</param>
        public void GetUserOtherPlatformBasicPublicInfo(
            string platformId
            , string[] userIds
            , ResultCallback<AccountUserPlatformInfosResponse> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var error = ApiHelperUtils.CheckForNullOrEmpty(platformId, userIds);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = new PlatformAccountInfoRequest
            {
                PlatformId = platformId,
                UserIds = userIds
            };

            api.GetUserOtherPlatformBasicPublicInfo(request, callback);
        }

        /// <summary>
        /// Refresh currrent cached user data.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserData via callback</param>
        /// <param name="isIncludeAllPlatforms">Set to true to also get other platform data. Default: false</param>
        public void RefreshData(ResultCallback<UserData> callback, bool isIncludeAllPlatforms = false)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            userDataCache = null;

            api.GetData(
                result =>
                {
                    if (!result.IsError)
                    {
                        userDataCache = result.Value;
                        callback.TryOk(userDataCache);
                        return;
                    }

                    callback.Try(result);
                }
                , isIncludeAllPlatforms
            );
        }

        /// <summary>
        /// Update some user information (e.g. language or country)
        /// </summary>
        /// <param name="updateRequest">Set its field if you want to change it, otherwise leave it</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Update( UpdateUserRequest updateRequest
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            api.Update(updateRequest, updateResult => {
                if (!updateResult.IsError)
                {
                    userDataCache = updateResult.Value;
                }

                callback.Try(updateResult);
            });
        }

        /// <summary>
        /// Update user email address
        /// </summary>
        /// <param name="updateEmailRequest">Set verify code and user new email on the body</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void UpdateEmail( UpdateEmailRequest updateEmailRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            api.UpdateEmail(updateEmailRequest, callback);
        }

        /// <summary>
        /// Upgrade a headless account with username and password. User must be logged in before this method can be
        /// used.
        /// </summary>
        /// <param name="userName">Username the user is upgraded to</param>
        /// <param name="password">Password to login with username</param>
        /// <param name="needVerificationCode">Will send verification code to email if true, default false</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Upgrade( string userName
            , string password
            , ResultCallback<UserData> callback
            , bool needVerificationCode = false)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            var requestModel = new UpgradeRequest
            {
                EmailAddress = userName,
                Password = password
            };
            var requestParameter = new UpgradeParameter
            {
                NeedVerificationCode = needVerificationCode
            };

            if (!EmailUtils.IsValidEmailAddress(userName))
            {
                SharedMemory?.Logger?.LogWarning("Upgrade username is deprecated, please use email for the replacement.");
            }

            api.Upgrade(requestModel, requestParameter, result =>
            {
                if (!result.IsError)
                {
                    userDataCache = result.Value;
                }

                callback.Try(result);
            });
        }

        /// <summary>
        /// Upgrade a headless account with username and password. User must be logged in before this method can be
        /// used.
        /// </summary>
        /// <param name="emailAddress">Email Address the user is upgraded to</param>
        /// <param name="userName">Username the user is upgraded to</param>
        /// <param name="password">Password to login with username</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        [Obsolete("userName param is deprecated, use Upgradev2 overload "), UnityEngine.Scripting.Preserve]
        public void Upgradev2( string emailAddress
            , string userName
            , string password
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var requestModel = new UpgradeV2Request
            {
                EmailAddress = emailAddress,
                Password = password,
                Username = userName
            };

            api.UpgradeV2(requestModel, result =>
            {
                if (!result.IsError)
                {
                    userDataCache = result.Value;
                }

                callback.Try(result);
            });
        }

        /// <summary>
        /// Upgrade a headless account with username and password. User must be logged in before this method can be
        /// used.
        /// </summary>
        /// <param name="emailAddress">Email Address the user is upgraded to</param>
        /// <param name="password">Password to login with username</param>
        /// <param name="callback">Returns a Result that contains UserData via callback when completed</param>
        public void Upgradev2( string emailAddress
            , string password
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var requestModel = new UpgradeV2Request
            {
                EmailAddress = emailAddress,
                Password = password,
            };

            api.UpgradeV2(requestModel, result =>
            {
                if (!result.IsError)
                {
                    userDataCache = result.Value;
                }

                callback.Try(result);
            });
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            api.UpgradeAndVerifyHeadlessAccount(upgradeAndVerifyHeadlessRequest, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var requestModel = new SendVerificationCodeRequest
            {
                EmailAddress = emailAddress,
                Context = VerificationContext.upgradeHeadlessAccount.ToString()
            };
            api.SendVerificationCode(requestModel, callback);
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// This function context is "UserAccountRegistration". If you want to set your own context, please use the other overload function.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            SendVerificationCode(VerificationContext.UserAccountRegistration, callback);
        }

        /// <summary>
        /// Trigger an email that contains verification code to be sent to user's email. User must be logged in.
        /// </summary>
        /// <param name="verifyContext">The context of what verification request</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendVerificationCode(VerificationContext verificationContext, ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            GetData(userDataResult =>
            {
                if (userDataResult.IsError)
                {
                    callback.TryError(
                        new Error(
                            ErrorCode.GeneralClientError,
                            "Failed when trying to get username",
                            "",
                            userDataResult.Error));
                    return;
                }

                var requestModel = new SendVerificationCodeRequest
                {
                    EmailAddress = userDataCache.emailAddress,
                    Context = verificationContext.ToString()
                };
                api.SendVerificationCode(requestModel, callback);
            });
        }

        /// <summary>
        /// Verify a user via an email registered as its username. User must be logged in.
        /// </summary>
        /// <param name="verificationCode">Verification code received from user's email</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void Verify( string verificationCode
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            const string contactType = "email";
            var requestModel = new VerifyRequest
            {
                VerificationCode = verificationCode,
                ContactType = contactType
            };

            api.Verify(requestModel, callback);
        }

        /// <summary>
        /// Trigger an email that contains reset password code to be sent to user
        /// </summary>
        /// <param name="userName">Username to be sent reset password code to.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendResetPasswordCode(string userName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!EmailUtils.IsValidEmailAddress(userName))
            {
                SharedMemory?.Logger?.LogWarning("username is deprecated, please use email for the replacement.");
            }

            var requestModel = new SendPasswordResetCodeRequest
            {
                EmailAddress = userName
            };
            api.SendPasswordResetCode(requestModel, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!EmailUtils.IsValidEmailAddress(userName))
            {
                SharedMemory?.Logger?.LogWarning("username is deprecated, please use email for the replacement.");
            }

            var requestModel = new ResetPasswordRequest
            {
                ResetCode = resetCode,
                EmailAddress = userName,
                NewPassword = newPassword
            };
            api.ResetPassword(requestModel, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new LinkOtherPlatformRequest
            {
                PlatformId = platformType.ToString().ToLower(),
            };

            var requestParameter = new LinkOtherPlatformParameter
            {
                Ticket = platformTicket
            };
            api.LinkOtherPlatform(requestModel, requestParameter, callback);
        }
        
        /// <summary>
        /// Link other platform's account to the currently logged in user. especially to support OIDC. 
        /// </summary>
        /// <param name="platformId">Specify platform's type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformTicket">Ticket / token from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LinkOtherPlatformId(string platformId
            , string platformTicket
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new LinkOtherPlatformRequest
            {
                PlatformId = platformId
            };

            var requestParameter = new LinkOtherPlatformParameter
            {
                Ticket = platformTicket
            };
            api.LinkOtherPlatform(requestModel, requestParameter, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetData(getUserDataResult =>
            {
                if (getUserDataResult.IsError)
                {
                    callback.TryError(getUserDataResult.Error);
                    return;
                }

                var requestModel = new LinkPlatformAccountRequest
                {
                    platformId = platformType.ToString().ToLower(),
                    platformUserId = platformUserId
                };

                var requestParameter = new LinkPlatformAccountParameter
                {
                    UserId = getUserDataResult.Value.userId
                };

                api.ForcedLinkOtherPlatform(requestModel, requestParameter, callback);
            });
        }
        
        /// <summary>
        /// Force to Link other platform's account to the currently logged in user. 
        /// </summary>
        /// <param name="platformId">Specify platform's type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformUserId"> UserId from other platform to be linked to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ForcedLinkOtherPlatformId(string platformId
            , string platformUserId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetData(getUserDataResult =>
            {
                if (getUserDataResult.IsError)
                {
                    callback.TryError(getUserDataResult.Error);
                    return;
                }

                var requestModel = new LinkPlatformAccountRequest
                {
                    platformId = platformId,
                    platformUserId = platformUserId
                };

                var requestParameter = new LinkPlatformAccountParameter
                {
                    UserId = getUserDataResult.Value.userId
                };

                api.ForcedLinkOtherPlatform(requestModel, requestParameter, callback);
            });
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new UnlinkPlatformAccountRequest
            {
                platformNamespace = string.Empty
            };

            var requestParameter = new UnlinkPlatformAccountParameter
            {
                PlatformId = platformType.ToString().ToLower()
            };
            api.UnlinkOtherPlatform(requestModel, requestParameter, callback);
        }
        
        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login. This function specially to support OIDC
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkOtherPlatformId(string platformId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new UnlinkPlatformAccountRequest
            {
                platformNamespace = string.Empty
            };

            var requestParameter = new UnlinkPlatformAccountParameter
            {
                PlatformId = platformId
            };
            api.UnlinkOtherPlatform(requestModel, requestParameter, callback);
        }
        
        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login.
        /// Note: Use this API to unlink all the user's current account from their other accounts in other platforms within the game namespace.
        /// It resolves issues with the old API by ensuring successful unlinking across multiple namespaces.
        /// After calling this API, if a user logs in to any namespace with the same 3rd platform account,
        /// they will be logged in as a different account.
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkAllOtherPlatform( PlatformType platformType
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestParameter = new UnlinkPlatformAccountParameter
            {
                PlatformId = platformType.ToString().ToLower()
            };
            api.UnlinkAllOtherPlatform(requestParameter, callback);
        }
        
        /// <summary>
        /// Unlink other platform that has been linked to the currently logged in user. The change will take effect
        /// after user has been re-login. This function specially to support OIDC.
        /// Note: Use this API to unlink all the user's current account from their other accounts in other platforms within the game namespace.
        /// It resolves issues with the old API by ensuring successful unlinking across multiple namespaces.
        /// After calling this API, if a user logs in to any namespace with the same 3rd platform account,
        /// they will be logged in as a different account.
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnlinkAllOtherPlatformId( string PlatformId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestParameter = new UnlinkPlatformAccountParameter
            {
                PlatformId = PlatformId
            };
            api.UnlinkAllOtherPlatform(requestParameter, callback);
        }

        /// <summary>
        /// Get array of other platforms this user linked to
        /// </summary>
        /// <param name="callback">Returns a Result that contains PlatformLink array via callback when
        /// completed.</param>
        public void GetPlatformLinks( ResultCallback<PagedPlatformLinks> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetPlatformLinkRequest
            {
                UserId = userSession.UserId
            };
            api.GetPlatformLinks(requestModel, callback);
        }

        /// <summary>
        /// Get user data from another user displayName or username. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query"> Display name or username that needed to get user data.</param>
        /// <param name="searchBy"> Filter the responded PagedPublicUsersInfo by SearchType. Choose the <c>SearchType.ALL</c> if you want to be responded with all query type.</param>
        /// <param name="callback"> Return a Result that contains UsersData when completed. </param>
        /// <param name="limit"> Targeted limit query filter. </param>
        /// <param name="offset"> Targeted offset query filter. </param>
        /// <param name="platformId"> Specify platform type, string type of this field makes support OpenID Connect (OIDC). </param>
        /// <param name="platformBy"> Filter the responded PagedPublicUsersInfo by SearchPlatformType. </param>
        public void SearchUsers( string query
            , SearchType searchBy
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset = 0 
            , int limit = 100
            , string platformId = null
            , SearchPlatformType platformBy = SearchPlatformType.None)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (searchBy == SearchType.USERNAME)
            {
                SharedMemory?.Logger?.LogWarning("Search by Username is deprecated, please use other search type for the replacement.");
            }

            var requestModel = new SearchUsersRequest
            {
                Query = query,
                SearchBy = searchBy,
                Offset = offset,
                Limit = limit,
                PlatformId = platformId,
                PlatformBy = platformBy
            };

            api.SearchUsers(requestModel, callback);
        }

        /// <summary>
        /// Get user data from another user by displayName or username. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query"> Display name or username that needed to get user data.</param>
        /// <param name="callback"> Return a Result that contains UsersData when completed. </param>
        /// <param name="offset"> Targeted offset query filter. </param>
        /// <param name="limit"> Targeted limit query filter. </param>
        public void SearchUsers( string query
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset = 0
            , int limit = 100)
        {
            SearchUsers(query, SearchType.ALL, callback, offset, limit);
        }

        /// <summary>
        /// Get user data from another user by displayName or username with respect to platformType. The query will be used to find the user with the most approximate username or display name.
        /// </summary>
        /// <param name="query">Targeted user's Username or Display Name.</param>
        /// <param name="platformType">The PlatformType (Steam, PS4, Xbox, etc).</param>
        /// <param name="platformBy">Filter the responded PagedPublicUsersInfo by SearchPlatformType.</param>
        /// <param name="callback">Return a Result that contains UsersData when completed.</param>
        /// <param name="offset">Targeted offset query filter.</param>
        /// <param name="limit">Targeted limit query filter.</param>
        public void SearchUsersByOtherPlatformType(
            string query
            , PlatformType platformType
            , SearchPlatformType platformBy
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset = 0
            , int limit = 100)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var platformId = platformType.ToString().ToLower();
            SearchUsers(query, SearchType.ALL, callback, offset, limit, platformId, platformBy);
        }

        /// <summary>
        /// Searches for users on third-party platforms using their Username or Display Name. 
        /// This function specifically targets users on platforms and utilizes the platform's DisplayName for the search.
        /// </summary>
        /// <param name="query">Targeted user's Username or Display Name.</param>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC).</param>
        /// <param name="platformBy">Filter the responded PagedPublicUsersInfo by SearchPlatformType.</param>
        /// <param name="callback">Return a Result that contains UsersData when completed.</param>
        /// <param name="offset">Targeted offset query filter.</param>
        /// <param name="limit">Targeted limit query filter.</param>
        public void SearchUsersByOtherPlatformId(
            string query
            , string platformId
            , SearchPlatformType platformBy
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset = 0
            , int limit = 100)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(platformId);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            SearchUsers(query, SearchType.ALL, callback, offset, limit, platformId, platformBy);
        }

        /// <summary>
        /// Get user data from another user by user id
        /// </summary>
        /// <param name="userId"> user id that needed to get user data</param>
        /// <param name="callback"> Return a Result that contains UserData when completed. </param>

        [Obsolete("This method is deprecated and will be removed soon. Please use User.GetUserPublicInfo instead")]
        public void GetUserByUserId( string userId
            , ResultCallback<PublicUserData> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if(!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetUserByUserIdRequest
            {
                UserId = userId
            };
            api.GetUserByUserId(requestModel, callback);
        }

        /// <summary>
        /// Get public user info by user id
        /// </summary>
        /// <param name="userId"> user id that needed to get user data</param>
        /// <param name="callback"> Return a Result that contains usre public info when completed. </param>
        public void GetUserPublicInfo(string userId
            , ResultCallback<GetUserPublicInfoResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if(!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!userSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserPublicInfo(userId, callback);
        }

        /// <summary>
        /// Get other user data by other platform userId (such as SteamID, for example)
        /// For Nintendo Platform you need to append Environment ID into the Platorm ID, with this format PlatformID:EnvironmentID. e.g csgas12312323f:dd1
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="otherPlatformUserId">Platform UserId that needed to get user data</param>
        /// <param name="callback">Return a Result that contains UserData when completed.</param>

        [Obsolete("This method is deprecated and will be removed soon. Please use User.GetUserByOtherPlatformUserIdV4 instead")]
        public void GetUserByOtherPlatformUserId( PlatformType platformType
            , string otherPlatformUserId
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetUserByOtherPlatformUserIdRequest
            {
                PlatformId = platformType.ToString().ToLower(),
                PlatformUserId = otherPlatformUserId
            };
            api.GetUserByOtherPlatformUserId(requestModel, callback);
        }

        /// <summary>
        /// Get other user data by other platform userId.
        /// </summary>
        /// <param name="platformType">User platform's type that needed to get user data (Google, Steam, Facebook, etc).</param>
        /// <param name="platformUserId">
        /// Platform UserId that needed to get user data. 
        /// For Nintendo Platform, NSA ID need to be appended with Environment ID using colon as separator. e.g kmzwa8awaa:dd1.
        /// </param>
        /// <param name="callback">Return a Result that contains UserData when completed.</param>
        public void GetUserByOtherPlatformUserIdV4(GetUserPlatformType platformType
            , string platformUserId
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (platformType == null || !platformType.IsValid())
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, $"Invalid platformType"));
                return;
            }

            api.GetUserByOtherPlatformUserIdV4(platformType.PlatformId, platformUserId, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            BulkGetUserByOtherPlatformUserIds(platformType, otherPlatformUserId, optionalParameters: null, callback);
        }

        /// <summary>
        /// Get other user data by other platform userId(s) (such as SteamID, for example)
        /// For Nintendo Platform you need to append Environment ID into the Platorm ID, with this format PlatformID:EnvironmentID. e.g csgas12312323f:dd1
        /// </summary>
        /// <param name="platformType">Other platform's type (Google, Steam, Facebook, etc)</param>
        /// <param name="otherPlatformUserId">Platform UserId that needed to get user data</param>
        /// <param name="optionalParameters">Additional parameters of the interface</param>
        /// <param name="callback">Return a Result that contains UserData when completed.</param>
        public void BulkGetUserByOtherPlatformUserIds( PlatformType platformType
            , string[] otherPlatformUserId
            , BulkGetUserByOtherPlatformUserIdsOptionalParameters optionalParameters
            , ResultCallback<BulkPlatformUserIdResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var error = ApiHelperUtils.CheckForNullOrEmpty(otherPlatformUserId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestModel = new BulkPlatformUserIdRequest
            {
                platformUserIDs = otherPlatformUserId,
            };

            var requestParameter = new BulkPlatformUserIdParameter
            {
                PlatformId = platformType.ToString().ToLower()
            };

            if (optionalParameters != null)
            {
                requestModel.PidType = optionalParameters.PidType != null ? optionalParameters.PidType.ConvertToString() : null;
                requestParameter.RawPuid = optionalParameters.RawPuid;
            }

            if (otherPlatformUserId.Length > MaxNumOfBulkGetUserByOtherPlatformUserId)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, $"Unable to send more than {MaxNumOfBulkGetUserByOtherPlatformUserId} user ids."));
                return;
            }
            else
            {
                api.BulkGetUserByOtherPlatformUserIdsV4(requestModel, requestParameter, callback);
            }
        }
        
        /// <summary>
        /// Get spesific country from user IP
        /// </summary>
        /// <param name="callback"> Returns a Result that contains country information via callback when completed</param>
        public void GetCountryFromIP( ResultCallback<CountryInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            api.GetCountryFromIP(callback);
        }

        /// <summary>
        /// Get all valid country codes for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an Array of <see cref="Country"/> via callback when completed</param>
        public void GetCountryGroupV3(ResultCallback<Country[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            api.GetCountryGroupV3(callback);
        }

        /// <summary>
        /// Check if user has purchased the subscription and eligible to play
        /// </summary>
        /// <param name="callback"> Returns the boolean result whether the user is subscribed and eligible to play the game via callback when the operation is completed</param>
        public void GetUserEligibleToPlay(ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            ResultCallback<ItemInfo> onGotItemInfo = itemInfoResult =>
            {
                if(itemInfoResult.IsError)
                {
                    callback.TryError(itemInfoResult.Error.Code);
                    return;
                }

                string[] skus = itemInfoResult.Value.features;
                string[] appIds = { AccelByteSDK.GetClientConfig().AppId };

                AccelByteSDK.GetClientRegistry().GetApi().GetEntitlement().GetUserEntitlementOwnershipAny(null, appIds, skus, ownershipResult =>
                {
                    if (ownershipResult.IsError)
                    {
                        callback.TryError(ownershipResult.Error.Code);
                        return;
                    }

                    callback.TryOk(ownershipResult.Value.owned);
                });
            };

            AccelByteSDK.GetClientRegistry().GetApi().GetItems().GetItemByAppId(AccelByteSDK.GetClientConfig().AppId, onGotItemInfo);
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
        [Obsolete("This method is deprecated and will be removed soon. Please use User.GetUserOtherPlatformBasicPublicInfo instead")]
        public void BulkGetUserInfo( string[] userIds
            , ResultCallback<ListBulkUserInfoResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ListBulkUserInfoRequest requestModel = new ListBulkUserInfoRequest
            {
                userIds = userIds
            };
            api.BulkGetUserInfo(requestModel, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (userSession.IsValid())
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "User is already logged in."
                };
                callback.TryError(error);
                return;
            }

            oAuth2.Verify2FACode(mfaToken, factor, code, callback, rememberDevice);
        }

        /// <summary>
        ///  OAuth2 token verification API 
        /// </summary>
        /// <param name="callback"></param>
        public void VerifyToken(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                const string errorMessage = "User is not log in.";
                callback.TryError(new Error(ErrorCode.InvalidRequest, errorMessage));
                return;
            }

            oAuth2.VerifyToken(Session.AuthorizationToken, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new Change2FAFactorParameter
            {
                MfaToken = mfaToken,
                Factor = factor.GetString()
            };
            api.Change2FAFactor(requestModel, callback);
        }

        /// <summary>
        /// Disable 2FA Authenticator
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Disable2FAAuthenticator( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            TwoFAEnable = false;
            api.Disable2FAAuthenticator(callback);
        }

        /// <summary>
        /// Enable 2FA Authenticator, to enable the backup code 2FA factor, you should also call Enable2FABackupcodes.
        /// </summary>
        /// <param name="code">Verification code</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Enable2FAAuthenticator( string code
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            TwoFAEnable = true;

            var requestModel = new Enable2FAAuthenticatorParameter
            {
                Code = code
            };
            api.Enable2FAAuthenticator(requestModel, callback);
        }

        /// <summary>
        /// Generate Secret Key For 3rd Party Authenticate Application 
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GenerateSecretKeyFor3rdPartyAuthenticateApp( ResultCallback<SecretKey3rdPartyApp> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            api.GenerateSecretKeyFor3rdPartyAuthenticateApp(callback);
        }

        /// <summary>
        /// Generate 2FA BackUp Code, will give a new list of new backup code and make codes generated before invalid.
        /// </summary>
        /// <param name="callback"></param>
        public void GenerateBackUpCode( ResultCallback<TwoFACode> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            api.GenerateBackUpCode(callback);
        }

        /// <summary>
        /// Disable 2FA Backup Codes
        /// </summary>
        /// <param name="callback"></param>
        public void Disable2FABackupCodes( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

			TwoFAEnable = false;
            api.Disable2FABackupCodes(callback);
        }

        /// <summary>
        /// Enable 2FA Backup Codes, this should be called if the 2FA not only using authenticator/3rd party factor. 
        /// </summary>
        /// <param name="callback"></param>
        public void Enable2FABackupCodes( ResultCallback<TwoFACode> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

			TwoFAEnable = true;
            api.Enable2FABackupCodes(callback);
        }

        /// <summary>
        /// Get 2FA BackUp Code
        /// </summary>
        /// <param name="callback"></param>
        public void GetBackUpCode( ResultCallback<TwoFACode> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            api.GetBackUpCode(callback);
        }

        /// <summary>
        /// Get User Enabled Factors
        /// </summary>
        /// <param name="callback"></param>
        public void GetUserEnabledFactors( ResultCallback<Enable2FAFactors> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            api.GetUserEnabledFactors(callback);
        }

        /// <summary>
        /// Make 2FA Factor Default
        /// </summary>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void Make2FAFactorDefault( TwoFAFactorType factor
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            var requestModel = new Make2FAFactorDefaultParameter
            {
                FactorType = factor.GetString()
            };
            api.Make2FAFactorDefault(requestModel, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            var requestModel = new GetInputValidationsParameter
            {
                LanguageCode = languageCode,
                DefaultOnEmpty = defaultOnEmpty
            };
            api.GetInputValidations(requestModel, callback);
        }

        /// <summary>
        /// Update current user 
        /// </summary>
        /// <param name="updateUserRequest">Update user request variables to be updated</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void UpdateUser(UpdateUserRequest updateUserRequest
            , ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            api.UpdateUser(updateUserRequest, callback);
        }

        /// <summary>
        /// Create Headless Account for Account Linking
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void CreateHeadlessAccountAndResponseToken(string linkingToken
            , bool extendExp
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(linkingToken, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };

            Login(cb => oAuth2.CreateHeadlessAccountAndResponseToken(linkingToken, extendExp, cb)
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Create Headless Account for Account Linking
        /// </summary>
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Extend expiration date of refresh token</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void CreateHeadlessAccountAndResponseToken(string linkingToken
            , bool extendExp
            , ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(linkingToken, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };

            Login(cb => oAuth2.CreateHeadlessAccountAndResponseToken(linkingToken, extendExp, cb)
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Authentication With PlatformLink for Account Linking
        /// </summary>
        /// <param name="email">Email to login</param>
        /// <param name="password">Password to login</param>
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Token for other platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [Obsolete]
        public void AuthenticationWithPlatformLink(string email
            , string password
            , string linkingToken
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Authentication using username is deprecated, please use user's email.");
            }

            Action<Error> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<Error> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action onLoginSuccess = () =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(Session.TokenData);
                    SendLoginSuccessPredefinedEventFromCurrentSession();
                    callback.TryOk();
                });
            };

            Login(cb => oAuth2.AuthenticationWithPlatformLink(email, password, linkingToken, cb)
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Authentication With PlatformLink for Account Linking
        /// </summary>
        /// <param name="email">Email to login</param>
        /// <param name="password">Password to login</param>
        /// /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        public void AuthenticationWithPlatformLink(string email
            , string password
            , string linkingToken
            , ResultCallback<TokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Authentication using username is deprecated, please use user's email.");
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenData> onLoginSuccess = (tokenData) =>
            {
                const bool saveTokenAsLatestUser = true;
                Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess) =>
                {
                    OnLoginSuccess?.Invoke(tokenData);
                    SendLoginSuccessPredefinedEvent(tokenData);
                    callback.TryOk(tokenData);
                });
            };

            Login(cb => oAuth2.AuthenticationWithPlatformLink(email, password, linkingToken, cb)
            , onAlreadyLogin
            , onLoginFailed
            , onLoginSuccess);
        }

        /// <summary>
        /// Request the Avatar of the given UserProfile
        /// </summary>
        /// <param name="userId">The UserId of a public Profile</param>
        /// <param name="callback">Returns a result that contains a <see cref="Texture2D"/></param>
        public void GetUserAvatar(string userId
            , ResultCallback<Texture2D> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

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
                    ABUtilities.DownloadTexture2DAsync(result.Value.avatarUrl, callback, SharedMemory?.Logger);
                }
            });
        }
        
        /// <summary>
        /// Get Publisher User 
        /// </summary>
        /// <param name="userId"> user id that needed to get publisher user</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetPublisherUser(string userId
            , ResultCallback<GetPublisherUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetPublisherUserParameter
            {
                UserId = userId
            };
            api.GetPublisherUser(requestModel, callback);
        }
        
        /// <summary>
        /// Get User Information 
        /// </summary>
        /// <param name="userId"> user id that needed to get user information</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetUserInformation(string userId
            , ResultCallback<GetUserInformationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetUserInformationParameter
            {
                UserId = userId
            };
            api.GetUserInformation(requestModel, callback);
        }
        
        /// <summary>
        /// Generate one time linking code
        /// </summary>
        /// <param name="platformId">The platform ID</param>
        /// <param name="callback">Return Result via callback when completed</param>
        public void GenerateOneTimeCode(PlatformType platformId
            , ResultCallback<GeneratedOneTimeCode> callback)
        {
            GenerateOneTimeCode(platformId, null, callback);
        }
        
        /// <summary>
        /// Generate one time linking code
        /// </summary>
        /// <param name="platformId">The platform ID</param>
        /// <param name="optionalParameters">Optional parameter to be sent</param>
        /// <param name="callback">Result via callback when completed</param>
        public void GenerateOneTimeCode(PlatformType platformId
            , GenerateOneTimeCodeOptionalParameters optionalParameters
            , ResultCallback<GeneratedOneTimeCode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            if (!userSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            oAuth2.GenerateOneTimeCode(Session.AuthorizationToken, platformId, optionalParameters, callback);
        }
        
        /// <summary>
        /// Generate user's token by one time link code.
        /// It requires a code which can be generated from GenerateOneTimeCode function.
        /// </summary>
        /// <param name="oneTimeLinkingCode">Generated code from GenerateOneTimeCode</param>
        /// <param name="callback">Result via callback when completed</param>
        public void RequestTokenByOneTimeLinkCode(string oneTimeLinkingCode
            , ResultCallback<TokenData> callback)
        {
            RequestTokenByOneTimeLinkCode(oneTimeLinkingCode, null, callback);
        }
        
        /// <summary>
        /// Generate user's token by one time link code.
        /// It requires a code which can be generated from GenerateOneTimeCode function.
        /// </summary>
        /// <param name="oneTimeLinkingCode">Generated code from GenerateOneTimeCode</param>
        /// <param name="optionalParameters">Optional parameter to be sent</param>
        /// <param name="callback">Result via callback when completed</param>
        public void RequestTokenByOneTimeLinkCode(string oneTimeLinkingCode
            , RequestTokenByOneTimeLinkCodeOptionalParameters optionalParameters
            , ResultCallback<TokenData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            if (!userSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            oAuth2.RequestTokenByOneTimeLinkCode(oneTimeLinkingCode, optionalParameters, callback);
        }

        /// <summary>
        /// This function generate a code that can be exchanged into publisher namespace token (i.e. by web portal)
        /// </summary>
        /// <param name="publisherClientId">The targeted game's publisher ClientID.</param>
        /// <param name="callback">A callback that will be called when the operation succeeded.</param>
        public void GenerateCodeForPublisherTokenExchange(string publisherClientId
            , ResultCallback<CodeForTokenExchangeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            TriggerGenerateCodeForPublisherTokenExchange(Session.AuthorizationToken, api.Config.PublisherNamespace, publisherClientId, callback);
        }

        protected virtual void TriggerGenerateCodeForPublisherTokenExchange(string authToken
            , string publisherNamespace
            , string publisherClientId
            , ResultCallback<CodeForTokenExchangeResponse> callback)
        {
            oAuth2.GenerateCodeForPublisherTokenExchange(authToken, publisherNamespace, publisherClientId, callback);
        }

        /// <summary>
        /// Generate publisher user's game token. Required a code from request game token
        /// </summary>
        /// <param name="code">code from request game token</param>
        /// <param name="callback">Return Result via callback when completed</param>
        public void GenerateGameToken(string code
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            if (userSession.IsValid())
            {
                callback?.TryError(ErrorCode.InvalidRequest,
                    "User is already logged in.");
                return;
            }

            oAuth2.GenerateGameToken(code, generateGameTokenResult =>
            {
                Session.SaveAuthTrustId(onDone: isSuccess =>
                {
                    callback?.Try(generateGameTokenResult);
                });
            });
        }
        
        /// <summary>
        /// Link headless account to current full account
        /// </summary>
        /// <param name="linkHeadlessAccountRequest"> struct that containing chosen namespace and one time link code</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void LinkHeadlessAccountToCurrentFullAccount(LinkHeadlessAccountRequest linkHeadlessAccountRequest
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.LinkHeadlessAccountToCurrentFullAccount(linkHeadlessAccountRequest, callback);
        }
        
        /// <summary>
        /// Get conflict result when link headless account to current account by one time code
        /// </summary>
        /// <param name="oneTimeLinkCode"> One time link code value</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetConflictResultWhenLinkHeadlessAccountToFullAccount(string oneTimeLinkCode
            , ResultCallback<ConflictLinkHeadlessAccountResult> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetConflictResultWhenLinkHeadlessAccountToFullAccountRequest
            {
                OneTimeLinkCode = oneTimeLinkCode
            };
            api.GetConflictResultWhenLinkHeadlessAccountToFullAccount(requestModel, callback);
        }

        /// <summary>
        /// Check user's account availability using displayName field.
        /// If the result is success or no error, it means the account already exists. 
        /// </summary>
        /// <param name="displayName">User's display name value to be checked</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void CheckUserAccountAvailability(string displayName
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            string fieldName = JsonUtils.SerializeWithStringEnum(AccountAvailabilityField.DisplayName);
            api.CheckUserAccountAvailabilityByFieldName(displayName, fieldName, callback);
        }

        /// <summary>
        /// Check user's account availability.
        /// If the result is success or no error, it means the account already exists. 
        /// </summary>
        /// <param name="valueToCheck">User's account value to be checked</param>
        /// <param name="field">Field to be checked</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void CheckUserAccountAvailability(string valueToCheck, AccountAvailabilityField field
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            string fieldName = JsonUtils.SerializeWithStringEnum(field);
            api.CheckUserAccountAvailabilityByFieldName(valueToCheck, fieldName, callback);
        }

        /// <summary>
        /// Get user config value of uniqueDisplayNameEnabled
        /// </summary>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetConfigUniqueDisplayNameEnabled(ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            api.GetConfigUniqueDisplayNameEnabled(callback);
        }

        /// <summary>
        /// Get user config value of userNameDisabled
        /// </summary>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void GetConfigUserNameDisabled(ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            api.GetConfigUserNameDisabled(callback);
        }

        /// <summary>
        /// This function is used for retrieving third party platform token for user that login using third party, 
        /// if user have not link requested platform in game namespace, will try to retrieving third party platform token from publisher namespace. 
        /// Passing platform group name or it's member will return same access token that can be used across the platform members.
        /// Note: The third party platform and platform group covered for this is:
        ///   (psn) ps4web, (psn) ps4, (psn) ps5, epicgames, twitch, awscognito.
        /// </summary>
        /// <param name="platformType">Platform type value</param>
        /// <param name="callback">Return Result via callback when completed</param> 
        public void RetrieveUserThirdPartyPlatformToken(PlatformType platformType 
            , ResultCallback<ThirdPartyPlatformTokenData, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!userSession.IsValid())
            {
                var Error = new OAuthError();
                Error.error = ErrorCode.IsNotLoggedIn.ToString();
                callback.TryError(Error);
                return;
            }

            string userId = userSession.UserId;
            coroutineRunner.Run(
                oAuth2.RetrieveUserThirdPartyPlatformToken(userId, platformType, callback));
        }

        /// <summary>
        /// Validate user inputs by checking against existing input validation and profanity filter service (if enabled).
        /// </summary>
        /// <param name="request">Request body, unused fields can be null</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ValidateUserInput(ValidateInputRequest request, ResultCallback<ValidateInputResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            api.ValidateUserInput(request, callback);
        }

        #region V4

        /// <summary>
        /// Login to AccelByte account with email and password.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="password">Password to login</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        /// <param name="rememberMe">Set it to true to extend the refresh token expiration time</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithEmailV4(string email, string password, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "LoginWithEmailV4(string email, string password, LoginWithEmailV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithEmailV4(string email
            , string password
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool rememberMe = false)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!EmailUtils.IsValidEmailAddress(email))
            {
                SharedMemory?.Logger?.LogWarning("Login using username is deprecated, please use email for the replacement.");
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveAuthTrustId(isSuccess =>
                    {
                        Session.SaveRefreshToken(email, saveTokenAsLatestUser, (saveSuccess) =>
                        {
                            OnLoginSuccess?.Invoke(tokenData);
                            SendLoginSuccessPredefinedEvent(tokenData);
                        });
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = email;
                }
                callback.TryOk(tokenData);
            };

            Login(
                cb =>
                {
                    Session.LoadAuthTrustId((isSuccess, authTrustId) =>
                    {
                        oAuth2.LoginWithEmailV4(email, password, rememberMe, cb, authTrustId);
                    });
                }
                , onAlreadyLogin
                , onLoginFailed
                , onProcessCompleted);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithDeviceIdV4(ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "LoginWithDeviceIdV4(LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithDeviceIdV4(ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(tokenData.platform_user_id, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = string.Empty;
                }
                callback.TryOk(tokenData);
            };

            Login(
                cb =>
                {
                    oAuth2.LoginWithDeviceIdV4(cb);
                }
                , onAlreadyLogin
                , onLoginFailed
                , onProcessCompleted);
        }

        /// <summary>
        /// Login with token from non AccelByte platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">If directly create new account when not linked yet</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithOtherPlatformV4(LoginPlatformType loginPlatformType, string platformToken, LoginWithOtherPlatformOptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithOtherPlatformV4(PlatformType platformType
            , string platformToken
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool createHeadless = true
            , LoginWithMacAddress loginWithMacAddress = null)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

#pragma warning disable AB0001
            LoginWithOtherPlatformV4(platformType: platformType
                , platformToken: platformToken
                , callback: callback
                , createHeadless:createHeadless
                , serviceLabel: null
                , loginWithMacAddress: loginWithMacAddress);
#pragma warning restore AB0001
        }

        /// <summary>
        /// Login with token from PS4/PS5 platforms. This will automatically register a user if the user
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="platformType">Other platform type</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">If directly create new account when not linked yet</param>
        /// <param name="serviceLabel">(Early-access: for PS5 only currently)Used to validate PSN app when AppId is set on Admin Portal for PS4/PS5</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithOtherPlatformV4(LoginPlatformType loginPlatformType, string platformToken, LoginWithOtherPlatformOptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithOtherPlatformV4(PlatformType platformType
            , string platformToken
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool createHeadless
            , string serviceLabel
            , LoginWithMacAddress loginWithMacAddress = null)
        {
            string platformId = platformType.ToString().ToLower();

#pragma warning disable AB0001
            LoginWithOtherPlatformIdV4(platformId: platformId
                , platformToken: platformToken
                , callback: callback
                , createHeadless: createHeadless
                , serviceLabel: serviceLabel
                , loginWithMacAddress: loginWithMacAddress);
#pragma warning restore AB0001
        }

        /// <summary>
        /// Login with token from non AccelByte platforms, especially to support OIDC (with 2FA enable)
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">If directly create new account when not linked yet</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithOtherPlatformV4(LoginPlatformType loginPlatformType, string platformToken, LoginWithOtherPlatformOptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithOtherPlatformIdV4(string platformId
            , string platformToken
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool createHeadless = true
            , LoginWithMacAddress loginWithMacAddress = null)
        {
#pragma warning disable AB0001
            LoginWithOtherPlatformIdV4(platformId: platformId
                , platformToken: platformToken
                , callback: callback
                , createHeadless: createHeadless
                , serviceLabel: null
                , loginWithMacAddress: loginWithMacAddress);
#pragma warning restore AB0001
        }

        /// <summary>
        /// Login with token from non AccelByte platforms, especially to support OIDC (with 2FA enable)
        /// identified by its platform type and platform token doesn't exist yet. A user registered with this method
        /// is called a headless account because it doesn't have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="platformId">Specify platform type, string type of this field makes support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        /// <param name="createHeadless">If directly create new account when not linked yet</param>
        /// <param name="serviceLabel">(Early-access: for PS5 only currently)Used to validate PSN app when AppId is set on Admin Portal for PS4/PS5</param>
        /// <param name="loginWithMacAddress">Include mac Address information for PSN and Xbox ban reporting</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithOtherPlatformV4(LoginPlatformType loginPlatformType, string platformToken, LoginWithOtherPlatformOptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithOtherPlatformIdV4(string platformId
            , string platformToken
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool createHeadless
            , string serviceLabel
            , LoginWithMacAddress loginWithMacAddress = null)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, platformId);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(platformId, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = platformId;
                }
                callback.TryOk(tokenData);
            };

            Login(
                cb =>
                {
                    oAuth2.LoginWithOtherPlatformIdV4(platformId, platformToken, createHeadless, serviceLabel, loginWithMacAddress, cb);
                }
                , onAlreadyLogin
                , onLoginFailed
                , onProcessCompleted
            );
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithLastRefreshTokenV4(ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "LoginWithLastRefreshTokenV4(LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithLastRefreshTokenV4(ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            LoginWithCachedRefreshTokenV4(UserSession.LastLoginUserCacheKey, callback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithRefreshTokenV4(string refreshToken, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "LoginWithRefreshTokenV4(string refreshToken, LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithRefreshTokenV4(string refreshToken
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                LoginWithRefreshTokenV4(refreshToken, UserSession.LastLoginUserCacheKey, callback);
            }
            else
            {
                LoginWithCachedRefreshTokenV4(UserSession.LastLoginUserCacheKey, callback);
            }
        }

        /// <summary>
        /// Login with refresh token from local cache file.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="cacheKey">Login unique cache name</param>
        /// <param name="callback">Returns Result with OAuth Error via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "LoginWithCachedRefreshTokenV4(string cacheKey, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "LoginWithCachedRefreshTokenV4(string cacheKey, LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void LoginWithCachedRefreshTokenV4(string cacheKey
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            TriggerLoginWithCachedRefreshTokenV4(cacheKey, callback);
        }

        protected virtual void TriggerLoginWithCachedRefreshTokenV4(string cacheKey
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            userSession.GetRefreshTokenFromCache(cacheKey, (refreshTokenData) =>
            {
                if (refreshTokenData == null)
                {
                    var newError = new OAuthError()
                    {
                        error = ErrorCode.CachedTokenNotFound.ToString(),
                        error_description = $"Failed to find token cache file."
                    };
                    callback.TryError(newError);
                    return;
                }

                DateTime refreshTokenExpireTime = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(refreshTokenData.expiration_date);
                if (DateTime.UtcNow >= refreshTokenExpireTime)
                {
                    var newError = new OAuthError()
                    {
                        error = ErrorCode.CachedTokenExpired.ToString(),
                        error_description = $"Cached token is expired"
                    };
                    callback.TryError(newError);
                    return;
                }

                LoginWithRefreshTokenV4(refreshTokenData.refresh_token, cacheKey, callback);
            });
        }

        private void LoginWithRefreshTokenV4(string refreshToken
            , string cacheKey
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                callback.TryError(new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "refreshToken cannot be null or empty"
                });
                return;
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(cacheKey, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = cacheKey;
                }
                callback.TryOk(tokenData);
            };

            Login(cb =>
            {
                oAuth2.RefreshSessionV4(refreshToken, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onProcessCompleted);
        }

        /// <summary>
        /// Create Headless Account for Account Linking.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="extendExp">Extend expiration date of refresh token</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "CreateHeadlessAccountAndResponseTokenV4(string linkingToken, bool extendExp, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "CreateHeadlessAccountAndResponseTokenV4(string linkingToken, bool extendExp, LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void CreateHeadlessAccountAndResponseTokenV4(string linkingToken
            , bool extendExp
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };
            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };
            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(linkingToken, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = linkingToken;
                }
                callback.TryOk(tokenData);
            };

            Login(cb =>
            {
                oAuth2.CreateHeadlessAccountAndResponseTokenV4(linkingToken, extendExp, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onProcessCompleted);
        }

        /// <summary>
        /// Authentication With PlatformLink for Account Linking.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="email">Email address to login</param>
        /// <param name="password">Password to login</param>
        ///  <param name="linkingToken">Token for platfrom type</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "AuthenticationWithPlatformLinkAndLoginV4(string email, string password, string linkingToken, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "AuthenticationWithPlatformLinkAndLoginV4(string email, string password, string linkingToken, LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void AuthenticationWithPlatformLinkAndLoginV4(string email
            , string password
            , string linkingToken
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (string.IsNullOrEmpty(linkingToken))
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidArgument.ToString(),
                    error_description = "The application was not executed from launcher"
                };
                callback?.TryError(error);
                return;
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(linkingToken, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = linkingToken;
                }
                callback.TryOk(tokenData);
            };

            Login(cb =>
            {
                oAuth2.AuthenticationWithPlatformLinkV4(email, password, linkingToken, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onProcessCompleted);
        }

        /// <summary>
        /// Generate publisher user's game token. Required a code from request game token
        /// </summary>
        /// <param name="code">code from request game token</param>
        /// <param name="callback">Return Result via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "GenerateGameTokenV4(string code, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "GenerateGameTokenV4(string code, LoginV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void GenerateGameTokenV4(string code
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (string.IsNullOrEmpty(code))
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidArgument.ToString(),
                    error_description = "The application was not executed from launcher"
                };
                callback?.TryError(error);
                return;
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(code, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = code;
                }
                callback.TryOk(tokenData);
            };

            Login(cb =>
            {
                oAuth2.GenerateGameTokenV4(code, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onProcessCompleted);
        }

        /// <summary>
        /// Verify 2FA Code 
        /// </summary>
        /// <param name="mfaToken">Multi-factor authentication Token</param>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="code">Verification code</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="rememberDevice">Will record device token when true</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82. Please use " +
            "Verify2FACodeV4(string mfaToken, TwoFAFactorType factor, string code, ResultCallback<TokenData, OAuthError> loginCallback) or " +
            "Verify2FACodeV4(string mfaToken, TwoFAFactorType factor, string code, Verify2FACodeV4OptionalParameters optionalParams, ResultCallback<TokenData, OAuthError> loginCallback) " +
            "function instead.")]
        public void Verify2FACodeV4(string mfaToken
            , TwoFAFactorType factor
            , string code
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool rememberDevice = false)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (userSession.IsValid())
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "User is already logged in."
                };
                callback.TryError(error);
                return;
            }

            Action<OAuthError> onAlreadyLogin = (error) =>
            {
                callback.TryError(error);
            };

            Action<OAuthError> onLoginFailed = (error) =>
            {
                SendLoginFailedPredefinedEvent(api.Config.Namespace, null);
                callback.TryError(error);
            };

            Action<TokenDataV4> onProcessCompleted = (tokenData) =>
            {
                if (tokenData.Queue == null)
                {
                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(code, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(tokenData);
                        SendLoginSuccessPredefinedEvent(tokenData);
                    });
                }
                else
                {
                    tokenData.Queue.Identifier = code;
                }
                callback.TryOk(tokenData);
            };

            Login(cb =>
            {
                oAuth2.Verify2FACodeV4(mfaToken, factor, code, rememberDevice, cb);
            }
            , onAlreadyLogin
            , onLoginFailed
            , onProcessCompleted);
        }

        /// <summary>
        /// Claim Access Token using Login Ticket.
        /// </summary>
        /// <param name="loginTicket">LoginTicket Login Ticket to claim the access token
        /// <param name="callback">Returns a result via callback when completed.
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("This function will be removed by AGS 3.82 " +
            "and the access token will be automatically claimed in the ResultCallback<TokenData, OAuthError> loginCallback " +
            "once the queue position is 0.")]
        public void ClaimAccessToken(TokenDataV4 loginTicket
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            Session.LoadAuthTrustId((isSuccess, authTrustId) =>
            {
                oAuth2.GetTokenWithLoginTicket(loginTicket: loginTicket.Queue.Ticket, authTrustId: authTrustId, callback: result =>
                {
                    if(result.IsError)
                    {
                        callback.TryError(result.Error);
                        return;
                    }

                    if (loginTicket.Queue.Identifier == string.Empty)
                    {
                        loginTicket.Queue.Identifier = result.Value.user_id;
                    }

                    const bool saveTokenAsLatestUser = true;
                    Session.SaveRefreshToken(loginTicket.Queue.Identifier, saveTokenAsLatestUser, (saveSuccess) =>
                    {
                        OnLoginSuccess?.Invoke(result.Value);
                        SendLoginSuccessPredefinedEvent(result.Value);
                    });

                    callback.TryOk(result.Value);
                });
            });
        }

        #region LoginQueueApi

        /// <summary>
        /// Refresh ticket.
        /// </summary>
        /// <param name="loginTicket">Login queue ticket</param>
        /// <param name="namespace_">user namespace</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("Since login queue process simplified since AGS 3.79, This function will be removed on AGS 3.82. " +
            "Please follow this documentation how to call the simplified login queue: https://docs.accelbyte.io/gaming-services/services/utilities/login-queue")]
        public void RefreshLoginQueueTicket(TokenDataV4 loginTicket
            , ResultCallback<RefreshTicketResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var ticketNamespace = loginTicket.Queue.Namespace ?? api.Config.Namespace;

            loginQueueApi.RefreshTicket(loginTicket.Queue.Ticket, ticketNamespace, callback);
        }

        /// <summary>
        /// Cancel ticket.
        /// </summary>
        /// <param name="loginTicket">Login queue ticket</param>
        /// <param name="callback">Returns Result via callback when completed</param>
        [AccelByte.Utils.Attributes.AccelBytePreview, Obsolete("Since login queue process simplified since AGS 3.79, This function will be removed on AGS 3.82. " +
            "Please follow this documentation how to cancel the simplified login queue: https://docs.accelbyte.io/gaming-services/services/utilities/login-queue")]
        public void CancelLoginQueueTicket(TokenDataV4 loginTicket, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var ticketNamespace = loginTicket.Queue.Namespace ?? api.Config.Namespace;

            loginQueueApi.CancelTicket(loginTicket.Queue.Ticket, ticketNamespace, callback);

            if (queuePoller != null) 
            {
                queuePoller.StopPoll();
            }
        }
        
        #endregion

        #endregion

        #region PredefinedEvents

        private void SendLoginSuccessPredefinedEventFromCurrentSession()
        {
            var token = new TokenData()
            {
                Namespace = Session.Namespace,
                user_id = Session.UserId,
                platform_id = Session.PlatformId,
                platform_user_id = Session.PlatformUserId,
                DeviceId = Session.DeviceId
            };
            SendLoginSuccessPredefinedEvent(token);
        }

        internal virtual void SendLoginSuccessPredefinedEvent(TokenData loginTokenData)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (loginTokenData != null && predefinedEventScheduler != null)
            {
                var loginPayload = new PredefinedLoginSucceededPayload(loginTokenData.Namespace, loginTokenData.user_id, loginTokenData.platform_id, loginTokenData.platform_user_id, loginTokenData.DeviceId);
                var loginEvent = new AccelByteTelemetryEvent(loginPayload);
                predefinedEventScheduler.SendEvent(loginEvent, null);
            }
        }

        internal virtual void SendLoginFailedPredefinedEvent(string @namespace, string platformId)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler != null)
            {
                var loginPayload = new PredefinedLoginFailedPayload(@namespace, platformId);
                var loginEvent = new AccelByteTelemetryEvent(loginPayload);
                predefinedEventScheduler.SendEvent(loginEvent, null);
            }
        }

        #endregion
    }
}