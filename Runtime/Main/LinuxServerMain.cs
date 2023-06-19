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

            if(AMS == null)
            {
                string dsId = GetCommandLineArg(DedicatedServer.CommandLineDsId);
                string amsServerUrl = GetCommandLineArg(ServerAMS.CommandLineAMSWatchdogUrlId);
                string amsHeartbeatInterval = GetCommandLineArg(ServerAMS.CommandLineAMSHeartbeatId);

                if(!string.IsNullOrEmpty(dsId) && !string.IsNullOrEmpty(amsServerUrl))
                {
                    int heartbeatInterval = 0;

                    if (!string.IsNullOrEmpty(amsHeartbeatInterval))
                    {
                        if(!int.TryParse(amsHeartbeatInterval, out heartbeatInterval))
                        {
                            heartbeatInterval = AccelByteServerPlugin.Config.AMSHeartbeatInterval;
                        }
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
