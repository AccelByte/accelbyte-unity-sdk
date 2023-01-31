// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AccelByte.Core
{
    internal class AccelByteServiceVersion
    {
        private JObject mappedVersionJson;
        public string[] ServicesName
        {
            get;
            private set;
        }

        public AccelByteServiceVersion(string configName)
        {
            var jsonObject = Utils.AccelByteFileUtils.ReadJson(configName);
            InitCompatibilityChecker(jsonObject);
        }

        public AccelByteServiceVersion(JObject json)
        {
            InitCompatibilityChecker(json);
        }

        private void InitCompatibilityChecker(JObject json)
        {
            mappedVersionJson = json;
            if(mappedVersionJson != null)
            {
                List<string> result = new List<string>();
                foreach (var map in mappedVersionJson)
                {
                    result.Add(map.Key);
                }
                ServicesName = result.ToArray();
            }
        }

        public bool IsCompatible(string serviceName, string serviceVersion)
        {
            bool isCompatible = false;
            try
            {
                Version min = null;
                Version max = null;
                if(mappedVersionJson[serviceName] != null)
                {
                    if (mappedVersionJson[serviceName]["minVersion"] != null)
                    {
                        min = Version.Parse(mappedVersionJson[serviceName]["minVersion"].ToString());
                    }
                    else
                    {
                        throw new InvalidOperationException("Service min version is missing");
                    }

                    if (mappedVersionJson[serviceName]["maxVersion"] != null)
                    {
                        max = Version.Parse(mappedVersionJson[serviceName]["maxVersion"].ToString());
                    }
                    else
                    {
                        throw new InvalidOperationException("Service max version is missing");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Service \"{serviceName}\" not found");
                }

                Version ver = Version.Parse(serviceVersion);

                isCompatible = (ver.CompareTo(min) >= 0 && ver.CompareTo(max) <= 0);
            }
            catch (Exception e)
            {
                AccelByteDebug.LogWarning(e.Message);
            }
            return isCompatible;
        }
    }
}
