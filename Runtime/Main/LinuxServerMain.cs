// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
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
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.InitializeNullEnv();
            }

            Models.MultiOAuthConfigs oAuthArgs = argsConfigParser.ParseOAuthConfigFromArgs();
            if (oAuthArgs != null)
            {
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride = oAuthArgs;
            }
            else
            {
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride = new Models.MultiOAuthConfigs();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.InitializeNullEnv();
            }

            string amsServerUrl = null;
            if (!string.IsNullOrEmpty(GetOverrideConfigWatchdogURL()))
            {
                amsServerUrl = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.WatchdogUrl;
            }
            else if (!string.IsNullOrEmpty(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl))
            {
                amsServerUrl = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl;
            }
            else
            {
                amsServerUrl = GetCommandLineArg(ServerAMS.CommandLineAMSWatchdogUrlId);
                if (!string.IsNullOrEmpty(amsServerUrl))
                {
                    string fieldName = nameof(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl);
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.SetFieldValueToAllEnv(fieldName, amsServerUrl);
                }
            }

            int heartbeatInterval = 0;
            if (GetOverrideConfigAMSInterval().HasValue)
            {
                heartbeatInterval = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval.Value;
            }
            else
            {
                string amsHeartbeatInterval = GetCommandLineArg(ServerAMS.CommandLineAMSHeartbeatId);
                if (!string.IsNullOrEmpty(amsHeartbeatInterval))
                {
                    if (int.TryParse(amsHeartbeatInterval, out heartbeatInterval))
                    {
                        string fieldName = nameof(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval);
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.SetFieldValueToAllEnv(fieldName, heartbeatInterval);
                    }
                }
            }

            string dsId = null;
            if (!string.IsNullOrEmpty(GetOverrideConfigDsId()))
            {
                dsId = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId;
            }
            else
            {
                dsId = GetCommandLineArg(DedicatedServer.CommandLineDsId);
                if (!string.IsNullOrEmpty(dsId))
                {
                    string fieldName = nameof(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId);
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.SetFieldValueToAllEnv(fieldName, dsId);
                }
            }

            if (!string.IsNullOrEmpty(dsId))
            {
                if (string.IsNullOrEmpty(amsServerUrl))
                {
                    amsServerUrl = ServerAMS.DefaultServerUrl;
                    string fieldName = nameof(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl);
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.SetFieldValueToAllEnv(fieldName, amsServerUrl);
                }

                if (heartbeatInterval == 0)
                {
                    heartbeatInterval = ServerAMS.DefaulHeatbeatSeconds;
                    string fieldName = nameof(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval);
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.SetFieldValueToAllEnv(fieldName, heartbeatInterval);
                }

                ServerAMS ams = AccelByteSDK.GetServerRegistry().GetAMS(autoCreate: false);
                if(ams != null)
                {
                    ams.Disconnect();
                    AccelByteSDK.GetServerRegistry().SetAMS(null);
                }
                AutoConnectAMS();
            }

            LinuxServerMain.onReceivedSignalEvent += CheckExitSignal;
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

        protected virtual void AutoConnectAMS()
        {
            AccelByteSDK.GetServerRegistry().GetAMS(autoCreate: true, autoConnect: true);
        }

        protected virtual string GetOverrideConfigWatchdogURL()
        {
            return AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.WatchdogUrl;
        }

        protected virtual string GetOverrideConfigDsId()
        {
            return AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId;
        }

        protected virtual int? GetOverrideConfigAMSInterval()
        {
            return AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval;
        }
    }
}
#endif
