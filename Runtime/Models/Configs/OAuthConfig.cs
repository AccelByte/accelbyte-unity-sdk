// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class OAuthConfig
    {
        [DataMember] public string ClientId;
        [DataMember] public string ClientSecret;
        [DataMember] public string GoogleWebClientId;

        /// <summary>
        ///  Copy member values
        /// </summary>
        public OAuthConfig ShallowCopy()
        {
            return (OAuthConfig)MemberwiseClone();
        }

        public bool Compare(OAuthConfig anotherConfig)
        {
            return this.ClientId == anotherConfig.ClientId &&
                   this.GoogleWebClientId == anotherConfig.GoogleWebClientId &&
                   this.ClientSecret == anotherConfig.ClientSecret;
        }

        /// <summary>
        ///  Assign missing config values.
        /// </summary>
        public void Expand()
        {
            if (this.ClientId == null)
            {
                this.ClientId = "";
            }
            if (this.ClientSecret == null)
            {
                this.ClientSecret = "";
            }
            if (this.GoogleWebClientId == null)
            {
                this.GoogleWebClientId = "";
            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.ClientId))
            {
                throw new System.Exception("Init AccelByte SDK failed, Client ID must not null or empty.");
            }
        }

        public bool IsRequiredFieldEmpty()
        {
            System.Collections.Generic.List<string> checkedStringFields = new System.Collections.Generic.List<string>()
            {
                ClientId
            };
            var retval = checkedStringFields.Exists((field) => string.IsNullOrEmpty(field));
            return retval;
        }
    }

    [DataContract, Preserve]
    public class MultiOAuthConfigs
    {
        [DataMember] public OAuthConfig Development;
        [DataMember] public OAuthConfig Certification;
        [DataMember] public OAuthConfig Production;
        [DataMember] public OAuthConfig Sandbox;
        [DataMember] public OAuthConfig Integration;
        [DataMember] public OAuthConfig QA;
        [DataMember] public OAuthConfig Default;

        public void Expand()
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                OAuthConfig envOAuthConfig = fieldInfo.GetValue(this) as OAuthConfig ;
                if (envOAuthConfig == null)
                {
                    envOAuthConfig = new OAuthConfig();
                }
                envOAuthConfig.Expand();

                fieldInfo.SetValue(this, envOAuthConfig);
            }
        }

        public void InitializeNullEnv()
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                OAuthConfig envConfig = fieldInfo.GetValue(this) as OAuthConfig;
                if (envConfig == null)
                {
                    envConfig = new OAuthConfig();
                    fieldInfo.SetValue(this, envConfig);
                }
            }
        }

        public OAuthConfig GetConfigFromEnvironment(SettingsEnvironment targetEnvironment)
        {
            OAuthConfig retval = null;
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == targetEnvironment.ToString())
                {
                    retval = fieldInfo.GetValue(this) as OAuthConfig;
                    break;
                }
            }

            return retval;
        }

        public void SetConfigValueToAllEnv(OAuthConfig newOAuthConfig)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                fieldInfo.SetValue(this, newOAuthConfig);
            }
        }

        public void SetConfigToEnv(OAuthConfig newOAuthConfig, SettingsEnvironment targetEnvironment)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == targetEnvironment.ToString())
                {
                    fieldInfo.SetValue(this, newOAuthConfig);
                    break;
                }
            }
        }
    }
}
