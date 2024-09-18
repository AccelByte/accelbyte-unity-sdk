// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Utils
{
    internal static class ServiceVersionUtils
    {
        /// <summary>
        /// Checks services conpatibility from local Compatibility matrix with existing running service 
        /// </summary>
        /// <param name="serviceVersionInterface"></param>
        /// <param name="abServiceVersion"></param>
        /// <returns></returns>
        public static bool CheckServicesCompatibility(IServiceVersion serviceVersionInterface, AccelByteServiceVersion abServiceVersion, IDebugger logger = null)
        {
            bool retval = true;
            var servicesName = abServiceVersion.ServicesName;
            
            if (servicesName != null)
            {
                foreach (var serviceName in servicesName)
                {
                    if (serviceName.Length <= 0)
                    {
                        continue;
                    }

                    ResultCallback<ServiceVersionInfo> responseCallback = (result) =>
                    {
                        bool matchResult = abServiceVersion.IsCompatible(serviceName, result.Value.Version, logger);
                        if (!matchResult)
                        {
                            logger?.LogWarning($"Incompatible service: {result.Value.Name}");
                            retval = false;
                        }
                    };
                    serviceVersionInterface.GetVersionInfo(serviceName, responseCallback);
                }
            }
            else
            {
                logger?.LogWarning($"Compatibility file not found!");
                retval = false;
            }

            return retval;
        }
    }

    internal interface IServiceVersion
    {
        void GetVersionInfo(string serviceName, ResultCallback<ServiceVersionInfo> callback);
    }
}
