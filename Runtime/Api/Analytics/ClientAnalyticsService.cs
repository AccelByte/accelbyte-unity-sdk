// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Models;
using System;

namespace AccelByte.Core
{
    public class ClientAnalyticsService
    {
        private bool initializationSuccess;
        private bool acceptEvent;
        private ClientAnalyticsEventScheduler analyticsControllerEventScheduler;
        private IHttpClient httpClient;
        AccelByteAnalyticsSettings analyticsSettings;
        private ApiSharedMemory sharedMemory;
        
        public ClientAnalyticsService(IHttpClient httpClient = null, AccelByteTimeManager timeManager = null)
        {
            if (httpClient == null)
            {
                WebRequestSchedulerAsync httpSenderScheduler = new WebRequestSchedulerAsync();
                UnityHttpRequestSender requestSender = new UnityHttpRequestSender(httpSenderScheduler);
                this.httpClient = new AccelByteHttpClient(requestSender);
            }
            else
            {
                this.httpClient = httpClient;
            }

            analyticsSettings = new AccelByteAnalyticsSettings();

            sharedMemory = new ApiSharedMemory()
            {
                TimeManager = timeManager
            };
        }

        /// <summary>
        /// Set default event namespace
        /// </summary>
        /// <param name="newDefaultEventNamespace">New event nampesace</param>
        public void SetDefaultEventNamespace(string newDefaultEventNamespace)
        {
            if(analyticsControllerEventScheduler != null)
            {
                analyticsControllerEventScheduler.SetDefaultNamespace(newDefaultEventNamespace);
            }
        }

        /// <summary>
        /// Send telemetry event with analytics credential. Not recommended to use for telemetry.
        /// </summary>
        /// <param name="telemetryEvent">Telemetry model</param>
        /// <param name="callback">Callback when event is sent</param>
        public void SendEvent(IAccelByteTelemetryPayload newPayload, System.Action<Result> onSent = null)
        {
            SendEvent(newPayload, null, onSent);
        }

        /// <summary>
        /// Send telemetry event with analytics credential. Not recommended to use for telemetry.
        /// </summary>
        /// <param name="telemetryEvent">Telemetry model</param>
        /// <param name="eventNamespace">Custom Event Namespace</param>
        /// <param name="callback">Callback when event is sent</param>
        public async void SendEvent(IAccelByteTelemetryPayload newPayload, string eventNamespace, System.Action<Result> onSent = null)
        {
            if (!initializationSuccess)
            {
                const string errorMessage = "Analytic initialization failed. Please check the analytics configuration.";
                onSent?.Invoke(Result.CreateError(ErrorCode.ErrorFromException, errorMessage));
                return;
            }

            var telemetryEvent = new AccelByteTelemetryEvent(newPayload);

            while (analyticsControllerEventScheduler == null && acceptEvent)
            {
                await System.Threading.Tasks.Task.Delay(AccelByteHttpHelper.HttpDelayOneFrameTimeMs);
            }

            if (!acceptEvent)
            {
                const string errorMessage = "Telemetry event is closed";
                Result errorResult = Result.CreateError(new Error(ErrorCode.InvalidRequest, errorMessage));
                onSent?.Invoke(errorResult);
                return;
            }

            ResultCallback onSentApiCallback = (result) =>
            {
                onSent?.Invoke(result);
            };

            if (string.IsNullOrEmpty(eventNamespace))
            {
                analyticsControllerEventScheduler.SendEvent(telemetryEvent, onSentApiCallback);
            }
            else
            {
                analyticsControllerEventScheduler.SendEvent(telemetryEvent, eventNamespace, onSentApiCallback);
            }
        }

        internal void Initialize()
        {
            Action<SettingsEnvironment> executeInitialization = (targetEnvironment) =>
            {
                Error initializationError;
                string activePlatform = AccelByteSettingsV2.GetActivePlatform(false);
                initializationSuccess = Initialize(activePlatform, targetEnvironment, out initializationError);
                if (initializationError != null)
                {
                    AccelByteDebug.LogVerbose($"Client analytics initialization failed.\n{initializationError.Message}");
                }
            };

            if (AccelByteSDK.Environment != null)
            {
                System.Action<SettingsEnvironment> environmentChangedDelegate = (SettingsEnvironment newEnvironment) =>
                {
                    if (analyticsControllerEventScheduler != null)
                    {
                        analyticsControllerEventScheduler.SetEventEnabled(false);
                        analyticsControllerEventScheduler.Dispose();
                        analyticsControllerEventScheduler = null;
                    }

                    executeInitialization.Invoke(newEnvironment);
                };
                AccelByteSDK.Environment.OnEnvironmentChanged += environmentChangedDelegate;
            }

            executeInitialization.Invoke(GetEnvironment());
        }

        private bool Initialize(string platform, SettingsEnvironment environment, out Error error)
        {
            error = null;

            try
            {
                Config clientConfig;
                OAuthConfig oAuthConfig;
                clientConfig = LoadClientConfig(environment);
                if (clientConfig != null && !clientConfig.EnableClientAnalyticsEvent)
                {
                    return false;
                }

                oAuthConfig = LoadOAuthConfig(platform, environment);

                AccelByteDebug.Initialize(clientConfig.EnableDebugLog, clientConfig.DebugLogFilter);

                string iAmUrl = clientConfig.IamServerUrl;

                CoroutineRunner taskRunner = new CoroutineRunner();

                Server.ServerOauthLoginSession analyticsControllerSession = new Server.ServerOauthLoginSession(
                    iAmUrl,
                    oAuthConfig.ClientId,
                    oAuthConfig.ClientSecret,
                    httpClient,
                    taskRunner);

                var analyticsApi = new ClientGameTelemetryApi(
                        httpClient,
                        clientConfig,
                        analyticsControllerSession);

                AnalyticsService analyticsService = new AnalyticsService(
                                    analyticsApi,
                                    analyticsControllerSession,
                                    taskRunner);
                analyticsService.SetSharedMemory(sharedMemory);

                CreateAnalyticsControllerEventScheduler(analyticsControllerSession, analyticsService, clientConfig.ClientAnalyticsEventInterval);
            }
            catch (Exception ex)
            {
                error = new Error(ErrorCode.ErrorFromException, ex.Message);
                return false;
            }

            return true;
        }

        protected virtual OAuthConfig LoadOAuthConfig(string platform, SettingsEnvironment environment)
        {
            var oAuthConfig = analyticsSettings.LoadOAuthFile(platform, environment, true);
            if (string.IsNullOrEmpty(oAuthConfig.ClientId))
            {
                throw new System.Exception("Analytics client id must not null or empty.");
            }
            if (string.IsNullOrEmpty(oAuthConfig.ClientSecret))
            {
                throw new System.Exception("Analytics client secret must not null or empty.");
            }
            return oAuthConfig;
        }

        protected virtual Config LoadClientConfig(SettingsEnvironment environment)
        {
            var config = analyticsSettings.LoadClientConfigFile(environment);
            config.CheckRequiredField();
            return config;
        }

        private SettingsEnvironment GetEnvironment()
        {
            return AccelByteSDK.Environment != null ? AccelByteSDK.Environment.Current : SettingsEnvironment.Default;
        }

        private void CreateAnalyticsControllerEventScheduler(Server.ServerOauthLoginSession analyticsControllerSession, AnalyticsService analyticsService, float intervalInMs)
        {
            analyticsControllerEventScheduler = new ClientAnalyticsEventScheduler(analyticsService);
            analyticsControllerEventScheduler.SetSharedMemory(ref sharedMemory);
            analyticsControllerEventScheduler.SetInterval(intervalInMs);
            analyticsControllerEventScheduler.SetEventEnabled(true);
            System.Action<ResultCallback<TokenData>> onAutoLoginDelegate = (callback) =>
            {
                AutoLogin(analyticsControllerSession, callback);
            };
            analyticsControllerEventScheduler.OnAutoLogin += onAutoLoginDelegate;
        }

        internal void StartAcceptEvent()
        {
            acceptEvent = true;
        }

        internal void StopAcceptEvent()
        {
            acceptEvent = false;
        }

        internal void DisposeScheduler()
        {
            if (analyticsControllerEventScheduler != null)
            {
                analyticsControllerEventScheduler.Dispose();
            }
        }

        protected virtual internal void AutoLogin(Server.ServerOauthLoginSession oAuth2, ResultCallback<TokenData> resultCallback)
        {
            if (oAuth2 != null)
            {
                oAuth2.LoginWithClientCredentials(resultCallback);
            }
            else
            {
                resultCallback.TryError(new Error(ErrorCode.ErrorFromException, "Analytics oAuth not implemented"));
            }
        }
    }
}
