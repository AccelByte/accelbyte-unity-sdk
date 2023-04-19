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
        private ServerWatchdog watchdog;

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

        protected ServerWatchdog Watchdog
        {
            get
            {
                return watchdog;
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

            if(Watchdog == null)
            {
                string dsId = GetCommandLineArg(DedicatedServer.CommandLineDsId);
                string watchdogServerUrl = GetCommandLineArg(ServerWatchdog.CommandLineWatchdogUrlId);
                string watchdogHeartbeatInterval = GetCommandLineArg(ServerWatchdog.CommandLineWatchdogHeartbeatId);

                if(!string.IsNullOrEmpty(dsId) && !string.IsNullOrEmpty(watchdogServerUrl))
                {
                    int heartbeatInterval = 0;

                    if (!string.IsNullOrEmpty(watchdogHeartbeatInterval))
                    {
                        if(!int.TryParse(watchdogHeartbeatInterval, out heartbeatInterval))
                        {
                            heartbeatInterval = AccelByteServerPlugin.Config.WatchdogHeartbeatInterval;
                        }
                    }

                    if (heartbeatInterval == 0)
                    {
                        heartbeatInterval = ServerWatchdog.DefaulHeatbeatSeconds;
                    }

                    watchdog = CreateServerWatchDog(dsId, watchdogServerUrl, heartbeatInterval);
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

        protected virtual ServerWatchdog CreateServerWatchDog(string dsId, string watchdogServerUrl, int watchdogIntervalSeconds)
        {
            ServerWatchdog retval = AccelByteServerPlugin.CreateWatchdogConnection(dsId, watchdogServerUrl, watchdogIntervalSeconds);
            return retval;
        }
    }
}
#endif
