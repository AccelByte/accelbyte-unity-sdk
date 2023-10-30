// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Reflection;

namespace AccelByte.Models
{
    internal static class ConfigArgsExtension
    {
        public static Config CopyToConfig(this SDKConfigArgs source, Config target)
        {
            return TransferArgsConfigToConfig(source, target);
        }

        public static ServerConfig CopyToConfig(this SDKConfigArgs source, ServerConfig target)
        {
            return TransferArgsConfigToConfig(source, target);
        }

        public static SDKConfigArgs GetByEnvironment(this MultiSDKConfigsArgs source, SettingsEnvironment environment)
        {
            return GetConfigByEnvironment<SDKConfigArgs, MultiSDKConfigsArgs>(source, environment);
        }

        public static OAuthConfig GetByEnvironment(this MultiOAuthConfigs source, SettingsEnvironment environment)
        {
            return GetConfigByEnvironment<OAuthConfig, MultiOAuthConfigs>(source, environment);
        }

        private static ConfigT TransferArgsConfigToConfig<ArgsSourceT, ConfigT>(ArgsSourceT argsSourceConfig, ConfigT targetConfig) where ArgsSourceT : class where ConfigT : class
        {
            UnityEngine.Assertions.Assert.IsNotNull(targetConfig);
            ConfigT retval = targetConfig;

            Type configType = retval.GetType();
            FieldInfo[] argsConfigFields = argsSourceConfig.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var configArgsFieldInfo in argsConfigFields)
            {
                object configArgsFieldValue = configArgsFieldInfo.GetValue(argsSourceConfig);
                if (configArgsFieldValue != null)
                {
                    var configField = configType.GetField(configArgsFieldInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (configField != null)
                    {
                        Type nullableType = Nullable.GetUnderlyingType(configArgsFieldInfo.FieldType);
                        if (nullableType != null)
                        {
                            var nullableValue = configArgsFieldInfo.FieldType.GetProperty("Value").GetValue(configArgsFieldValue);
                            configField.SetValue(retval, nullableValue);
                        }
                        else
                        {
                            configField.SetValue(retval, configArgsFieldValue);
                        }
                    }
                }
            }

            return retval;
        }

        private static ConfigT GetConfigByEnvironment<ConfigT, MultiConfigT>(MultiConfigT multiConfig, SettingsEnvironment environment) where ConfigT : class where MultiConfigT : class
        {
            if (multiConfig == null)
            {
                return null;
            }

            string environmentName = environment.ToString();
            System.Reflection.FieldInfo environmentField = multiConfig.GetType().GetField(environmentName);
            ConfigT retval = (ConfigT)environmentField.GetValue(multiConfig);
            return retval;
        }
    }
}
