// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    public static class AccelByteServerPlugin
    {
        public static event Action<SettingsEnvironment> environmentChanged;

        internal static OAuthConfig OAuthConfig
        {
            get
            {
                OAuthConfig retval = AccelByteSDK.GetServerRegistry().OAuthConfig;
                return retval;
            }
        }

        public static ServerConfig Config
        {
            get
            {
                ServerConfig retval = AccelByteSDK.GetServerRegistry().Config;
                return retval;
            }
        }

        static AccelByteServerPlugin()
        {
            const string deprecatedWarning = "AccelByteServerPlugin is deprecated and will be sunset on 3.79 release. Please migrate to AccelByteSDK.GetServerRegistry.\nGuideline how to migrate: https://docs.accelbyte.io/gaming-services/developers/sdk-tools/sdk-guides/multiple-registries/migrate-to-multiple-registry-unity/";
            UnityEngine.Debug.LogWarning(deprecatedWarning);
            AccelByteSDK.Environment.OnEnvironmentChanged += environmentChanged;
        }

        public static ServiceVersion GetServiceVersion()
        {
            ServiceVersion retval = AccelByteSDK.GetServerRegistry().GetApi().GetVersionService();
            return retval;
        }

        public static Version GetPluginVersion()
        {
            return new Version(AccelByteSettingsV2.AccelByteSDKVersion);
        }

        public static DedicatedServer GetDedicatedServer()
        {
            DedicatedServer retval = AccelByteSDK.GetServerRegistry().GetApi().GetDedicatedServer();
            return retval;
        }

        public static ServerDSHub GetDsHub()
        {
            ServerDSHub retval = AccelByteSDK.GetServerRegistry().GetApi().GetDsHub();
            return retval;
        }

        public static ServerAMS GetAMS()
        {
            ServerAMS retval = AccelByteSDK.GetServerRegistry().GetAMS();
            return retval;
        }

        public static DedicatedServerManager GetDedicatedServerManager()
        {
            DedicatedServerManager retval = AccelByteSDK.GetServerRegistry().GetApi().GetDedicatedServerManager();
            return retval;
        }

        public static ServerEcommerce GetEcommerce()
        {
            ServerEcommerce retval = AccelByteSDK.GetServerRegistry().GetApi().GetEcommerce();
            return retval;
        }

        public static ServerStatistic GetStatistic()
        {
            ServerStatistic retval = AccelByteSDK.GetServerRegistry().GetApi().GetStatistic();
            return retval;
        }

        public static ServerUGC GetUGC()
        {
            ServerUGC retval = AccelByteSDK.GetServerRegistry().GetApi().GetUGC();
            return retval;
        }

        public static ServerQosManager GetQos()
        {
            ServerQosManager retval = AccelByteSDK.GetServerRegistry().GetApi().GetQos();
            return retval;
        }

        public static ServerGameTelemetry GetGameTelemetry()
        {
            ServerGameTelemetry retval = AccelByteSDK.GetServerRegistry().GetApi().GetGameTelemetry();
            return retval;
        }

        public static ServerAchievement GetAchievement()
        {
            ServerAchievement retval = AccelByteSDK.GetServerRegistry().GetApi().GetAchievement();
            return retval;
        }

        public static ServerLobby GetLobby()
        {
            ServerLobby retval = AccelByteSDK.GetServerRegistry().GetApi().GetLobby();
            return retval;
        }

        public static ServerSession GetSession()
        {
            ServerSession retval = AccelByteSDK.GetServerRegistry().GetApi().GetSession();
            return retval;
        }

        public static ServerCloudSave GetCloudSave()
        {
            ServerCloudSave retval = AccelByteSDK.GetServerRegistry().GetApi().GetCloudSave();
            return retval;
        }

        public static ServerMatchmaking GetMatchmaking()
        {
            ServerMatchmaking retval = AccelByteSDK.GetServerRegistry().GetApi().GetMatchmaking();
            return retval;
        }

        public static ServerMatchmakingV2 GetMatchmakingV2()
        {
            ServerMatchmakingV2 retval = AccelByteSDK.GetServerRegistry().GetApi().GetMatchmakingV2();
            return retval;
        }

        public static ServerUserAccount GetUserAccount()
        {
            ServerUserAccount retval = AccelByteSDK.GetServerRegistry().GetApi().GetUserAccount();
            return retval;
        }

        public static ServerSeasonPass GetSeasonPass()
        {
            ServerSeasonPass retval = AccelByteSDK.GetServerRegistry().GetApi().GetSeasonPass();
            return retval;
        }

        public static ServerAnalyticsService GetServerAnalyticsService()
        {
            ServerAnalyticsService retval = AccelByteSDK.GetServerRegistry().GetApi().GetAnalyticsService();
            return retval;
        }

        public static AccelByteStatsDMetricExporterApi GetStatsDMetricExporter()
        {
            AccelByteStatsDMetricExporterApi retval = AccelByteSDK.GetServerRegistry().GetApi().GetStatsMetricExporterService();
            return retval;
        }

        #region Environment
        [Obsolete("Use AccelByteSDK.Environment.Set() to update environment target")]
        public static void SetEnvironment(SettingsEnvironment newEnvironment)
        {
            if (AccelByteSDK.Environment != null)
            {
                AccelByteSDK.Environment.Set(newEnvironment);
            }
        }

        [Obsolete("Use AccelByteSDK.Environment.Current to get current environment target")]
        public static SettingsEnvironment GetEnvironment()
        {
            return AccelByteSDK.Environment != null ? AccelByteSDK.Environment.Current : SettingsEnvironment.Default;
        }
        #endregion

        public static void ClearEnvironmentChangedEvent()
        {
            environmentChanged = null;
        }

        internal static PredefinedEventScheduler CreatePredefinedEventScheduler(ServerAnalyticsService analyticsService, ServerConfig config)
        {
            var newPredefinedEventScheduler = new PredefinedEventScheduler(analyticsService);
            newPredefinedEventScheduler.SetEventEnabled(config.EnablePreDefinedEvent);
            return newPredefinedEventScheduler;
        }
    }
}
