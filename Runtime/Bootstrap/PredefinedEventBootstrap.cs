// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System.IO;
using System;
using UnityEngine;

namespace AccelByte.Core
{
    internal static class PredefinedEventBootstrap
    {
        private static PredefinedGameStateCommand gameStateEventCommand;

        internal static void Execute()
        {
            gameStateEventCommand = new PredefinedGameStateCommand(true);
            gameStateEventCommand.StartAcceptEvent();

            var predefinedEventOnSdkInitializedParam = new PredefinedEventSdkInitializedParameter(PredefinedEventSdkInitializedParameter.LowLevelEventName, Api.AccelByteSettingsV2.AccelByteSDKVersion);
#if UNITY_EDITOR
            const string editorSdkInitalizeEventFlagName = "AccelByte_SDK_Initialized_Event_Sent";
            if (UnityEditor.SessionState.GetBool(editorSdkInitalizeEventFlagName, false))
            {
                gameStateEventCommand.ExecSdkInitializedEvent(predefinedEventOnSdkInitializedParam, () =>
                {
                    UnityEditor.SessionState.SetBool(editorSdkInitalizeEventFlagName, true);
                });
            }
#else
            gameStateEventCommand.ExecSdkInitializedEvent(predefinedEventOnSdkInitializedParam, null);
#endif
        }

        public static void Stop()
        {
            gameStateEventCommand.StopAcceptEvent();
        }
    }
}
