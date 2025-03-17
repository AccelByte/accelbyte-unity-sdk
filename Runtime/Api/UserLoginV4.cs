// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;

namespace AccelByte.Api
{
    public partial class User
    {
        private AccelByteLoginQueuePoller queuePoller;

        /// <summary>
        /// Login to AccelByte account with email and password.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="password">Password to login</param>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void LoginWithEmailV4(string email
            , string password
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            LoginWithEmailV4(email, password, optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Login to AccelByte account with email and password.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="password">Password to login</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void LoginWithEmailV4(string email
            , string password
            , LoginWithEmailV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (optionalParams == null)
            {
                optionalParams = new LoginWithEmailV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                bool rememberMe = optionalParams.RememberMe;
                LoginWithEmailV4(email, password, loginActionCallback, rememberMe);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void LoginWithDeviceIdV4(ResultCallback<TokenData, OAuthError> loginCallback)
        {
            LoginWithDeviceIdV4(optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Login with device id. A user registered with this method is called a headless account because it doesn't
        /// have username yet.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void LoginWithDeviceIdV4(LoginWithDeviceIdV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (optionalParams == null)
            {
                optionalParams = new LoginWithDeviceIdV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                LoginWithDeviceIdV4(loginActionCallback);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
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
        /// <param name="loginPlatformType">Specify platform type</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithOtherPlatformV4(LoginPlatformType loginPlatformType
            , string platformToken
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            LoginWithOtherPlatformV4(loginPlatformType, platformToken, optionalParams: null, loginCallback);
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
        /// <param name="loginPlatformType">Specify platform type</param>
        /// <param name="platformToken">Token for other platform type</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithOtherPlatformV4(LoginPlatformType loginPlatformType
            , string platformToken
            , LoginWithOtherPlatformV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (loginPlatformType == null)
            {
                loginCallback?.TryError(new OAuthError()
                {
                    error_description = "loginPlatformType is null. Please assign this value."
                });
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new LoginWithOtherPlatformV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                bool createHeadless = optionalParams.CreateHeadless;
                string serviceLabel = optionalParams.ServiceLabel;
                LoginWithMacAddress loginWithMacAddress = optionalParams.LoginWithMacAddress;
                LoginWithOtherPlatformIdV4(loginPlatformType.PlatformId, platformToken, loginActionCallback, createHeadless, serviceLabel, loginWithMacAddress);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLastRefreshTokenV4(ResultCallback<TokenData, OAuthError> loginCallback)
        {
            LoginWithLastRefreshTokenV4(optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="optionalParams">Optional parameter to modify the function</param
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithLastRefreshTokenV4(LoginWithRefreshTokenV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            LoginWithCachedRefreshTokenV4(UserSession.LastLoginUserCacheKey, optionalParams, loginCallback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithRefreshTokenV4(string refreshToken
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            LoginWithRefreshTokenV4(refreshToken, optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Login with the latest refresh token stored on the device. Will returning an error if the token already expired.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="refreshToken">The latest user's refresh token</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithRefreshTokenV4(string refreshToken
            , LoginWithRefreshTokenV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                LoginWithRefreshTokenV4(refreshToken, UserSession.LastLoginUserCacheKey, optionalParams, loginCallback);
            }
            else
            {
                LoginWithCachedRefreshTokenV4(UserSession.LastLoginUserCacheKey, optionalParams, loginCallback);
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
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithCachedRefreshTokenV4(string cacheKey
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            LoginWithCachedRefreshTokenV4(cacheKey, optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Login with refresh token from local cache file.
        /// The callback will consist of login queue ticket and token data.
        /// If token data is empty, game client is expected to poll the login ticket status
        /// until they receive a response that their position is zero and then claim the ticket.
        /// Ticket status can be get using LogInQueue API, and for claiming the token please call ClaimAccessToken method.
        /// </summary>
        /// <param name="cacheKey">Login unique cache name</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result with OAuth Error via callback when completed</param>
        public void LoginWithCachedRefreshTokenV4(string cacheKey
            , LoginWithRefreshTokenV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            TriggerLoginWithCachedRefreshTokenV4(cacheKey, optionalParams, loginCallback);
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
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void CreateHeadlessAccountAndResponseTokenV4(string linkingToken
            , bool extendExp
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            CreateHeadlessAccountAndResponseTokenV4(linkingToken, extendExp, optionalParams: null, loginCallback);
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
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void CreateHeadlessAccountAndResponseTokenV4(string linkingToken
            , bool extendExp
            , CreateHeadlessAccountAndResponseTokenV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (optionalParams == null)
            {
                optionalParams = new CreateHeadlessAccountAndResponseTokenV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                CreateHeadlessAccountAndResponseTokenV4(linkingToken, extendExp, loginActionCallback);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
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
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void AuthenticationWithPlatformLinkAndLoginV4(string email
            , string password
            , string linkingToken
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            AuthenticationWithPlatformLinkAndLoginV4(email, password, linkingToken, optionalParams: null, loginCallback);
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
        /// <param name="linkingToken">Token for platfrom type</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns Result via callback when completed</param>
        public void AuthenticationWithPlatformLinkAndLoginV4(string email
            , string password
            , string linkingToken
            , AuthenticationWithPlatformLinkAndLoginV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (optionalParams == null)
            {
                optionalParams = new AuthenticationWithPlatformLinkAndLoginV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                AuthenticationWithPlatformLinkAndLoginV4(email, password, linkingToken, loginActionCallback);

            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
        }

        /// <summary>
        /// Generate publisher user's game token. Required a code from request game token
        /// </summary>
        /// <param name="code">code from request game token</param>
        /// <param name="loginCallback">Return Result via callback when completed</param>
        public void GenerateGameTokenV4(string code
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            GenerateGameTokenV4(code, optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Generate publisher user's game token. Required a code from request game token
        /// </summary>
        /// <param name="code">code from request game token</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Return Result via callback when completed</param>
        public void GenerateGameTokenV4(string code
            , GenerateGameTokenV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (optionalParams == null)
            {
                optionalParams = new GenerateGameTokenV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                GenerateGameTokenV4(code, loginActionCallback);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
        }

        /// <summary>
        /// Verify 2FA Code 
        /// </summary>
        /// <param name="mfaToken">Multi-factor authentication Token</param>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="code">Verification code</param>
        /// <param name="loginCallback">Returns a result via callback when completed</param>
        public void Verify2FACodeV4(string mfaToken
            , TwoFAFactorType factor
            , string code
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Verify2FACodeV4(mfaToken, factor, code, optionalParams: null, loginCallback);
        }

        /// <summary>
        /// Verify 2FA Code 
        /// </summary>
        /// <param name="mfaToken">Multi-factor authentication Token</param>
        /// <param name="factor">The factor will return factor based on what factors is enabled</param>
        /// <param name="code">Verification code</param>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="loginCallback">Returns a result via callback when completed</param>
        public void Verify2FACodeV4(string mfaToken
            , TwoFAFactorType factor
            , string code
            , Verify2FACodeV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (optionalParams == null)
            {
                optionalParams = new Verify2FACodeV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                bool rememberDevice = optionalParams.RememberDevice;
                Verify2FACodeV4(mfaToken, factor, code, loginActionCallback, rememberDevice);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
        }

        protected virtual void TriggerLoginWithCachedRefreshTokenV4(string cacheKey
            , LoginV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
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
                    loginCallback?.TryError(newError);
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
                    loginCallback.TryError(newError);
                    return;
                }

                LoginWithRefreshTokenV4(refreshTokenData.refresh_token, cacheKey, optionalParams, loginCallback);
            });
        }

        private void LoginWithRefreshTokenV4(string refreshToken
            , string cacheKey
            , LoginV4OptionalParameters optionalParams
            , ResultCallback<TokenData, OAuthError> loginCallback)
        {
            if (optionalParams == null)
            {
                optionalParams = new LoginV4OptionalParameters();
            }

            ResultCallback<TokenDataV4, OAuthError> loginActionCallback = null;
            Action loginAction = () =>
            {
                LoginWithRefreshTokenV4(refreshToken, cacheKey, loginActionCallback);
            };

            LoginV4(
                ref loginAction
                , ref loginActionCallback
                , loginCallback
                , optionalParams);
        }

        private void LoginV4(
            ref Action loginAction
            , ref ResultCallback<TokenDataV4, OAuthError> loginActionCallback
            , ResultCallback<TokenData, OAuthError> interfaceCallback
            , LoginV4OptionalParameters optionalParameters)
        {
            UnityEngine.Assertions.Assert.IsNotNull(optionalParameters);

            if (queuePoller != null)
            {
                interfaceCallback?.TryError(new OAuthError() { error_description = "Queue is already running" });
                return;
            }

            loginActionCallback = result =>
            {
                if (result.IsError)
                {
                    interfaceCallback?.TryError(result.Error);
                    return;
                }

                if (result.Value.Queue == null)
                {
                    interfaceCallback?.TryOk(result.Value);
                    return;
                }

                optionalParameters?.OnQueueUpdatedEvent?.Invoke(result.Value.Queue);

                Action<string, string, AccelByteResult<LoginQueueTicket, Error>> onUpdateTicketRequested = (queueTicket, ticketNamespace, result) =>
                {
                    var targetNamespace = ticketNamespace != null ? ticketNamespace : api.Config.Namespace;

                    loginQueueApi.RefreshTicket(queueTicket, targetNamespace, refreshTicketResponse =>
                    {
                        if (refreshTicketResponse.IsError)
                        {
                            result?.Reject(refreshTicketResponse.Error);
                        }
                        else
                        {
                            optionalParameters.OnQueueUpdatedEvent?.Invoke(refreshTicketResponse.Value);
                            result?.Resolve(refreshTicketResponse.Value);
                        }
                    });
                };

                Action<string, string, AccelByteResult<Error>> onCancelTicketRequested = (queueTicket, ticketNamespace, result) =>
                {
                    if (queueTicket == null)
                    {
                        queuePoller = null;

                        result?.Resolve();
                        return;
                    }
                    
                    var targetNamespace = ticketNamespace != null ? ticketNamespace : api.Config.Namespace;

                    loginQueueApi.CancelTicket(queueTicket, targetNamespace, cancelTicketResponse =>
                    {
                        queuePoller = null;

                        if (cancelTicketResponse.IsError)
                        {
                            result?.Reject(cancelTicketResponse.Error);
                        }
                        else
                        {
                            result?.Resolve();
                        }
                        optionalParameters.OnCancelledEvent?.Invoke();
                    });
                };

                Action<string, string> onClaimTicketRequested = (queueTicket, identifier) =>
                {
                    Session.LoadAuthTrustId((isSuccess, authTrustId) =>
                    {
                        oAuth2.GetTokenWithLoginTicket(loginTicket: queueTicket, authTrustId: authTrustId, callback: result =>
                        {
                            queuePoller = null;

                            if (result.IsError)
                            {
                                interfaceCallback?.TryError(result.Error);
                                return;
                            }

                            if (identifier == string.Empty)
                            {
                                identifier = result.Value.user_id;
                            }

                            const bool saveTokenAsLatestUser = true;
                            Session.SaveRefreshToken(identifier, saveTokenAsLatestUser, (saveSuccess) =>
                            {
                                OnLoginSuccess?.Invoke(result.Value);
                                SendLoginSuccessPredefinedEvent(result.Value);
                            });

                            interfaceCallback?.TryOk(result.Value);
                        });
                    });
                };

                queuePoller = new AccelByteLoginQueuePoller(
                    SharedMemory?.CoreHeartBeat
                    , optionalParameters.LoginTimeout.TimeoutMs
                    , onUpdateTicketRequested
                    , onCancelTicketRequested
                    , onClaimTicketRequested);
                queuePoller.OnTimeout = () =>
                {
                    interfaceCallback?.TryError(new OAuthError() { error_description = "Queue timeout" });
                };
                queuePoller.StartPoll(result.Value.Queue, optionalParameters.CancellationToken);
            };
            loginAction?.Invoke();
        }
        
#region Login Queue Implementation
        private void LoginWithEmailV4(string email
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
        
        private void LoginWithDeviceIdV4(ResultCallback<TokenDataV4, OAuthError> callback)
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

        private void LoginWithOtherPlatformIdV4(string platformId
            , string platformToken
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool createHeadless
            , string serviceLabel
            , LoginWithMacAddress loginWithMacAddress = null)
        {
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

        private void CreateHeadlessAccountAndResponseTokenV4(string linkingToken
            , bool extendExp
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
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

        private void AuthenticationWithPlatformLinkAndLoginV4(string email
            , string password
            , string linkingToken
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
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

        private void GenerateGameTokenV4(string code
            , ResultCallback<TokenDataV4, OAuthError> callback)
        {
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
        
        private void Verify2FACodeV4(string mfaToken
            , TwoFAFactorType factor
            , string code
            , ResultCallback<TokenDataV4, OAuthError> callback
            , bool rememberDevice = false)
        {
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
#endregion
    }
}
