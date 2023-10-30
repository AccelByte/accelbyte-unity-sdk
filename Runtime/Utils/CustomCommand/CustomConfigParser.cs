// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Utils
{
    internal class CustomConfigParser
    {
        internal const string OverrideSDKConfigCommand = "-OverrideSDKConfig";
        internal const string OverrideOAuthConfigCommand = "-OverrideOAuthConfig";

        private ConfigT ParseConfigFromArgs<ConfigT>(string command) where ConfigT : class
        {
            ConfigT retval = null;
            string[] args = GetGameArgs();
            string overrideConfig = CommandLineArgs.GetArg(args, command);
            if (!string.IsNullOrEmpty(overrideConfig))
            {
                try
                {
                    overrideConfig = overrideConfig.Replace("'", "\"");
                    retval = overrideConfig.ToObject<ConfigT>();
                }
                catch (System.Exception ex)
                {
                    AccelByteDebug.LogWarning($"Failed to parse {command} args.\nException:{ex.Message}");
                }
            }

            return retval;
        }

        public MultiSDKConfigsArgs ParseSDKConfigFromArgs()
        {
            MultiSDKConfigsArgs retval = null;
            var configArgs = ParseConfigFromArgs<SDKConfigArgs>(OverrideSDKConfigCommand);

            if (configArgs != null)
            {
                retval = new MultiSDKConfigsArgs();
                retval.Default = configArgs;
                retval.Certification = configArgs;
                retval.Development = configArgs;
                retval.Production = configArgs;
            }
            return retval;
        }

        public MultiOAuthConfigs ParseOAuthConfigFromArgs()
        {
            MultiOAuthConfigs retval = null;
            var oAuthArgs = ParseConfigFromArgs<OAuthConfig>(OverrideOAuthConfigCommand);

            if (oAuthArgs != null)
            {
                retval = new MultiOAuthConfigs();
                retval.Default = oAuthArgs;
                retval.Certification = oAuthArgs;
                retval.Development = oAuthArgs;
                retval.Production = oAuthArgs;
            }
            return retval;
        }

        protected virtual string[] GetGameArgs()
        {
            return System.Environment.GetCommandLineArgs();
        }
    }
}