// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

#if (UNITY_STANDALONE_LINUX && UNITY_SERVER) 
using AccelByte.Server;

namespace AccelByte.Core
{
    internal class LinuxServerMain : IPlatformMain
    {
        private static System.Action<int> onReceivedSignalEvent;
        internal System.Action<int> OnReceivedSignalEvent
        {
            get
            {
                return LinuxServerMain.onReceivedSignalEvent;
            }
            set
            {
                LinuxServerMain.onReceivedSignalEvent = value;
            }
        }

        private AccelByteSignalHandler signalHandler;
        private ServerAMS ams;

        protected virtual AccelByteSignalHandler SignalHandler
        {
            get
            {
                return signalHandler;
            }
            set
            {
                signalHandler = value;
            }
        }

        protected ServerAMS AMS
        {
            get
            {
                return ams;
            }
        }

        public LinuxServerMain()
        {
            ClearSignalCallback();
        }

        public void Run()
        {
            if (SignalHandler == null)
            {
                InitializeSignalHandler();
                SignalHandler.SetSignalHandlerAction(OnReceivedSignal);
            }

            var argsConfigParser = new Utils.CustomConfigParser();
            Models.MultiSDKConfigsArgs configArgs = argsConfigParser.ParseSDKConfigFromArgs();
            if (configArgs != null)
            {
                AccelByteSDK.OverrideConfigs.SDKConfigOverride = configArgs;
            }
            else
            {
                AccelByteSDK.OverrideConfigs.SDKConfigOverride = new Models.MultiSDKConfigsArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification = new Models.SDKConfigArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default = new Models.SDKConfigArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development = new Models.SDKConfigArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production = new Models.SDKConfigArgs();
            }

            Models.MultiOAuthConfigs oAuthArgs = argsConfigParser.ParseOAuthConfigFromArgs();
            if (oAuthArgs != null)
            {
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride = oAuthArgs;
            }
            else
            {
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride = new Models.MultiOAuthConfigs();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Certification = new Models.OAuthConfig();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Default = new Models.OAuthConfig();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Development = new Models.OAuthConfig();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Production = new Models.OAuthConfig();
            }

            if (AMS == null)
            {
                string amsServerUrl = GetCommandLineArg(ServerAMS.CommandLineAMSWatchdogUrlId);
                if (string.IsNullOrEmpty(amsServerUrl))
                {
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.AMSServerUrl = amsServerUrl;
                }

                string amsHeartbeatInterval = GetCommandLineArg(ServerAMS.CommandLineAMSHeartbeatId);
                int heartbeatInterval = 0;
                if (!string.IsNullOrEmpty(amsHeartbeatInterval))
                {
                    if (!int.TryParse(amsHeartbeatInterval, out heartbeatInterval))
                    {
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AMSHeartbeatInterval = heartbeatInterval;
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval = heartbeatInterval;
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.AMSHeartbeatInterval = heartbeatInterval;
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.AMSHeartbeatInterval = heartbeatInterval;
                    }
                }

                string dsId = GetCommandLineArg(DedicatedServer.CommandLineDsId);
                if (!string.IsNullOrEmpty(dsId))
                {
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AchievementServerUrl = dsId;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId = dsId;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.DsId = dsId;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.DsId = dsId;
                }

                if(!string.IsNullOrEmpty(dsId))
                {
                    if (string.IsNullOrEmpty(amsServerUrl)) 
                    {
                        amsServerUrl = ServerAMS.DefaultServerUrl;
                    }

                    if (heartbeatInterval == 0)
                    {
                        heartbeatInterval = ServerAMS.DefaulHeatbeatSeconds;
                    }

                    ams = CreateServerAMS(dsId, amsServerUrl, heartbeatInterval);
                }
                
                LinuxServerMain.onReceivedSignalEvent += CheckExitSignal;
            }
        }

        public void Stop()
        {
            LinuxServerMain.onReceivedSignalEvent -= CheckExitSignal;
        }

        public void ClearSignalCallback()
        {
            LinuxServerMain.onReceivedSignalEvent = null;
        }

        protected virtual string GetCommandLineArg(string arg)
        {
            return Utils.CommandLineArgs.GetArg(arg);
        }

        [AOT.MonoPInvokeCallback(typeof(AccelByteSignalHandler.SignalHandlerDelegate))]
        private static void OnReceivedSignal(int signalCode)
        {
            onReceivedSignalEvent?.Invoke(signalCode);
        }

        private void CheckExitSignal(int signalCode)
        {
            if (signalCode == (int)LinuxSignalCode.SigTerm)
            {
                ExitGame();
            }
        }

        protected virtual void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }

        protected virtual void InitializeSignalHandler()
        {
            SignalHandler = new AccelByteSignalHandler();
        }

        protected virtual ServerAMS CreateServerAMS(string dsId, string amsServerUrl, int amsIntervalSeconds)
        {
            ServerAMS retval = AccelByteServerPlugin.CreateAMSConnection(dsId, amsServerUrl, amsIntervalSeconds);
            return retval;
        }
    }
}
#endif
