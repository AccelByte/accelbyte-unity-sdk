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
        private OverrideConfigs overrideConfigs;
        private Server.ServerOauthLoginSession activeSession;
        
        internal ClientAnalyticsService(IHttpRequestSenderFactory httpRequestSenderFactory, CoreHeartBeat coreHeartBeat, AccelByteTimeManager timeManager, Config clientConfig, SettingsEnvironment environment)
        {
            this.overrideConfigs = overrideConfigs;
            IHttpRequestSender requestSender = httpRequestSenderFactory.CreateHttpRequestSender();
            this.httpClient = new AccelByteHttpClient(requestSender);

            analyticsSettings = new AccelByteAnalyticsSettings();

            sharedMemory = new ApiSharedMemory()
            {
                TimeManager = timeManager,
                CoreHeartBeat = coreHeartBeat
            };
            
            string activePlatform = AccelByteSettingsV2.GetActivePlatform(false);
            initializationSuccess = Initialize(clientConfig, activePlatform, environment, out Error initializationError);
            if (initializationError != null)
            {
                AccelByteDebug.LogVerbose($"Client analytics initialization failed.\n{initializationError.Message}");
            }
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

        private bool Initialize(Config clientConfig, string platform, SettingsEnvironment environment, out Error error)
        {
            error = null;

            try
            {
                if (!clientConfig.EnableClientAnalyticsEvent)
                {
                    return false;
                }

                OAuthConfig oAuthConfig;
                oAuthConfig = LoadOAuthConfig(platform, environment);

                AccelByteDebug.Initialize(clientConfig.EnableDebugLog, clientConfig.DebugLogFilter);

                string iAmUrl = clientConfig.IamServerUrl;

                CoroutineRunner taskRunner = new CoroutineRunner();

                activeSession = new Server.ServerOauthLoginSession(
                    iAmUrl,
                    oAuthConfig.ClientId,
                    oAuthConfig.ClientSecret,
                    httpClient,
                    taskRunner);
                activeSession.SetSharedMemory(sharedMemory);
                activeSession.Reset();

                var analyticsApi = new ClientGameTelemetryApi(
                        httpClient,
                        clientConfig,
                        activeSession);

                AnalyticsService analyticsService = new AnalyticsService(
                                    analyticsApi,
                                    activeSession,
                                    taskRunner);
                analyticsService.SetSharedMemory(sharedMemory);

                CreateAnalyticsControllerEventScheduler(activeSession, analyticsService, clientConfig.ClientAnalyticsEventInterval);
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
            activeSession.Reset();
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
