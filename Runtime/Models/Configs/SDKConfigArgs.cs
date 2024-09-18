// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    internal class SDKConfigArgs
    {
        [DataMember] public string Namespace;
        [DataMember] public bool? UsePlayerPrefs;
        [DataMember] public bool? EnableDebugLog;
        [DataMember] public string DebugLogFilter;
        [DataMember] public bool? RandomizeDeviceId;
        [DataMember] public string BaseUrl;
        [DataMember] public string IamServerUrl;
        [DataMember] public string PlatformServerUrl;
        [DataMember] public string BasicServerUrl;
        [DataMember] public string LobbyServerUrl;
        [DataMember] public string CloudStorageServerUrl;
        [DataMember] public string GameProfileServerUrl;
        [DataMember] public string StatisticServerUrl;
        [DataMember] public string QosManagerServerUrl;
        [DataMember] public string AgreementServerUrl;
        [DataMember] public string LeaderboardServerUrl;
        [DataMember] public string CloudSaveServerUrl;
        [DataMember] public string ChallengeServerUrl;
        [DataMember] public string InventoryServerUrl;
        [DataMember] public string GameTelemetryServerUrl;
        [DataMember] public string AchievementServerUrl;
        [DataMember] public string UGCServerUrl;
        [DataMember] public string ReportingServerUrl;
        [DataMember] public string SeasonPassServerUrl;
        [DataMember] public string SessionBrowserServerUrl;
        [DataMember] public string SessionServerUrl;
        [DataMember] public string MatchmakingV2ServerUrl;
        [DataMember] public bool? UseTurnManager;
        [DataMember] public string TurnManagerServerUrl;
        [DataMember] public string TurnServerHost;
        [DataMember] public string TurnServerPort;
        [DataMember] public string TurnServerPassword;
        [DataMember] public string TurnServerSecret;
        [DataMember] public string TurnServerUsername;
        [DataMember] public int? PeerMonitorIntervalMs;
        [DataMember] public int? PeerMonitorTimeoutMs;
        [DataMember] public int? HostCheckTimeoutInSeconds;
        [DataMember] public string GroupServerUrl;
        [DataMember] public string ChatServerWsUrl;
        [DataMember] public string ChatServerUrl;
        [DataMember] public string GdprServerUrl;
        [DataMember] public string LoginQueueServerUrl;
        [DataMember] public string RedirectUri;
        [DataMember] public string AppId;
        [DataMember] public string PublisherNamespace;
        [DataMember] public string CustomerName;
        [DataMember] public bool? EnableAuthHandshake;
        [DataMember] public int? MaximumCacheSize;
        [DataMember] public int? MaximumCacheLifeTime;
        [DataMember] public bool? EnablePresenceBroadcastEvent;
        [DataMember] public int? PresenceBroadcastEventInterval;
        [DataMember] public int? PresenceBroadcastEventGameState;
        [DataMember] public string PresenceBroadcastEventGameStateDescription;
        [DataMember] public bool? EnablePreDefinedEvent;
        [DataMember] public bool? EnableClientAnalyticsEvent;
        [DataMember] public float? ClientAnalyticsEventInterval;
        [DataMember] public bool? EnableAmsServerQos;
        [DataMember] public string DSHubServerUrl;
        [DataMember] public string DSMControllerServerUrl;
        [DataMember] public string MatchmakingServerUrl;
        [DataMember] public string AMSServerUrl;
        [DataMember] public string WatchdogUrl;
        [DataMember] public int? AMSHeartbeatInterval;
        [DataMember] public int? Port;
        [DataMember] public string StatsDServerUrl;
        [DataMember] public int? StatsDServerPort;
        [DataMember] public int? StatsDMetricInterval;
        [DataMember] public int? DSHubReconnectTotalTimeout;
        [DataMember] public int? AMSReconnectTotalTimeout;
        [DataMember] public bool? ServerUseAMS;
        [DataMember] public string DsId;
        [DataMember] public string ClientId;
        [DataMember] public string ClientSecret;
        [DataMember] public bool? OverrideServiceUrl;
        [DataMember] public bool? EnableMatchmakingTicketCheck;
        [DataMember] public int? MatchmakingTicketCheckPollRate;
        [DataMember] public int? MatchmakingTicketCheckInitialDelay;
    }

    [DataContract, Preserve]
    internal class MultiSDKConfigsArgs
    {
        [DataMember] public SDKConfigArgs Development;
        [DataMember] public SDKConfigArgs Certification;
        [DataMember] public SDKConfigArgs Production;
        [DataMember] public SDKConfigArgs Sandbox;
        [DataMember] public SDKConfigArgs Integration;
        [DataMember] public SDKConfigArgs QA;
        [DataMember] public SDKConfigArgs Default;

        public void InitializeNullEnv()
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                SDKConfigArgs envConfig = fieldInfo.GetValue(this) as SDKConfigArgs;
                if (envConfig == null)
                {
                    envConfig = new SDKConfigArgs();
                    fieldInfo.SetValue(this, envConfig);
                }
            }
        }

        public void SetFieldValueToAllEnv(string fieldName, object value)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                SDKConfigArgs envConfig = fieldInfo.GetValue(this) as SDKConfigArgs;
                if (envConfig != null)
                {
                    System.Reflection.FieldInfo targetField = envConfig.GetType().GetField(fieldName);
                    if(targetField != null)
                    {
                        targetField.SetValue(envConfig, value);
                        fieldInfo.SetValue(this, envConfig);
                    }
                }
            }
        }

        public void SetConfigValueToAllEnv(SDKConfigArgs newConfig)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                fieldInfo.SetValue(this, newConfig);
            }
        }

        public SDKConfigArgs GetConfigFromEnvironment(SettingsEnvironment targetEnvironment)
        {
            SDKConfigArgs retval = null;
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == targetEnvironment.ToString())
                {
                    retval = fieldInfo.GetValue(this) as SDKConfigArgs;
                    break;
                }
            }

            return retval;
        }
    }
}
