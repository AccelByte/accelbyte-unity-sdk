// Copyright (c) 2019-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Server;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public class DummyBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            Object.DontDestroyOnLoad(this.gameObject);
        }

#if UNITY_SERVER
        bool drainMode = false;

        private void Start()
        {
            var ds = AccelByteServerPlugin.GetDedicatedServer();
            ds.LoginWithClientCredentials(result =>
            {
                OnServerLogin();
            });
        }

        private void OnServerLogin()
        {
            AccelByteServerPlugin.GetDsHub().OnConnected += OnServerConnected;
            AccelByteServerPlugin.GetDsHub().Connect("LOCAL_SERVER_NAME");
        }

        private void OnServerConnected()
        {
            AccelByteServerPlugin.GetDsHub().MatchmakingV2BackfillProposalReceived += notificationPayload =>
            {
                if (notificationPayload.IsError)
                {
                    AccelByteDebug.LogWarning($"{notificationPayload.Error.Message}{notificationPayload.Error.messageVariables}");
                }

                if (drainMode)
                {
                    AccelByteServerPlugin.GetMatchmakingV2().RejectBackfillProposal(notificationPayload.Value, true, null);
                }
                else
                {
                    AccelByteServerPlugin.GetMatchmakingV2().AcceptBackfillProposal(notificationPayload.Value, false, null);
                }
            };
            ServerWatchdog watchdog = AccelByteServerPlugin.GetWatchdog();
            watchdog.OnDrainReceived += OnDrainReceived;
            watchdog.SendReadyMessage();
        }

        private void OnApplicationQuit()
        {
            ServerWatchdog watchdog = AccelByteServerPlugin.GetWatchdog();
            if(watchdog != null)
            {
                watchdog.Disconnect();
            }
        }

        private void OnDrainReceived()
        {
            InvokeRepeating(nameof(CheckIfShouldShutdown), 60.0f, 1.0f);
        }

        private void CheckIfShouldShutdown()
        {
            AccelByteDebug.LogWarning("Shutdown the server after entering drain mode");
            UnityEngine.Application.Quit();
        }
#endif
    }
}