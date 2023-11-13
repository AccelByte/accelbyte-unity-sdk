// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Api;
using System;

namespace AccelByte.Core
{
    public class AccelByteEnvironment
    { /// <summary>
      /// Trigger on new environment set (To)
      /// </summary>
        public Action<Models.SettingsEnvironment> OnEnvironmentChanged;
        /// <summary>
        /// Trigger on new environment set (From, To)
        /// </summary>
        public Action<Models.SettingsEnvironment, Models.SettingsEnvironment> OnEnvironmentChangedV2;

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
            var olderEnvironment = currentEnvironment;
            currentEnvironment = newEnvironment;
            OnEnvironmentChanged?.Invoke(currentEnvironment);
            OnEnvironmentChangedV2?.Invoke(olderEnvironment, currentEnvironment);
        }
    }
}

