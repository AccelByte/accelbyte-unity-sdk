// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core
{
#if (UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && UNITY_SERVER
    using PlatformMain = LinuxServerMain;
#else
    using PlatformMain = NullMain;
#endif
    internal static class AccelByteSDKMain
    {
        internal static PlatformMain Main;

        private static IAccelByteGameThreadSignaller gameThreadSignaller;

        private static System.Action<float> onGameUpdate;

        internal static System.Action<float> OnGameUpdate
        {
            get
            {
                CheckMainThreadSignallerAlive();
                return onGameUpdate;
            }
            set
            {
                CheckMainThreadSignallerAlive();
                onGameUpdate = value;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void StartAccelByteSDK()
        {
            AccelByteDebug.LogVerbose("AccelByte API Initialized!");

            if (Main == null)
            {
                Main = new PlatformMain();
            }

            onGameUpdate = null;

            Main.Run();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += EditorApplicationPlayyModeStateChanged;
#endif
            Application.quitting += ApplicationQuitting;
        }

#if UNITY_EDITOR
        private static void EditorApplicationPlayyModeStateChanged(UnityEditor.PlayModeStateChange newState)
        {
            if(newState == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                UnityEditor.EditorApplication.playModeStateChanged -= EditorApplicationPlayyModeStateChanged;
                Main.Stop();
            }
        }
#endif

        public static void AttachGameUpdateSignaller(IAccelByteGameThreadSignaller newSignaller)
        {
            if(gameThreadSignaller != null)
            {
                gameThreadSignaller.GameThreadSignal -= OnGameThreadUpdate;
            }

            gameThreadSignaller = newSignaller;

            if (gameThreadSignaller != null)
            {
                gameThreadSignaller.GameThreadSignal -= OnGameThreadUpdate;
                gameThreadSignaller.GameThreadSignal += OnGameThreadUpdate;
            }
            else
            {
                AccelByteDebug.LogWarning("AccelByte update signaller set to null.");
            }
        }

        private static void CheckMainThreadSignallerAlive()
        {
            if(gameThreadSignaller == null)
            {
                GameObject signallerGameObject = Utils.AccelByteGameObject.GetOrCreateGameObject();
                var accelByteSignaller = signallerGameObject.GetComponent<AccelByteGameThreadSignaller>();
                if(accelByteSignaller == null)
                {
                    accelByteSignaller = signallerGameObject.AddComponent<AccelByteGameThreadSignaller>();
                }
                AttachGameUpdateSignaller(accelByteSignaller);
            }
        }

        private static void OnGameThreadUpdate(float deltaTime)
        {
            onGameUpdate?.Invoke(deltaTime);
        }

        private static void ApplicationQuitting()
        {
            Main.Stop();
        }
    }
}
