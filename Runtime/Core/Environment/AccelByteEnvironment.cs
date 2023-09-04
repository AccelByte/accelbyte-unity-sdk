// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Api;
using System;

namespace AccelByte.Core
{
    public class AccelByteEnvironment
    {
        public Action<Models.SettingsEnvironment> OnEnvironmentChanged;

        Models.SettingsEnvironment currentEnvironment;

        /// <summary>
        /// Get current environment target
        /// </summary>
        public Models.SettingsEnvironment Current
        {
            get
            {
                return currentEnvironment;
            }
        }

        public AccelByteEnvironment()
        {
            currentEnvironment = Models.SettingsEnvironment.Default;
        }

        /// <summary>
        /// Change the target of SDK environment.
        /// </summary>
        /// <param name="newEnvironment">New Environment Target</param>
        public void Set(Models.SettingsEnvironment newEnvironment)
        {
            currentEnvironment = newEnvironment;
            OnEnvironmentChanged?.Invoke(currentEnvironment);
        }
    }
}

