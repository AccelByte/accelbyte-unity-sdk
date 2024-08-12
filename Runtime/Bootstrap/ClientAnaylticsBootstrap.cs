// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal static class ClientAnaylticsBootstrap
    {
        private static ClientAnalyticsService analyticsController;

        internal static void Execute()
        {
            analyticsController = new ClientAnalyticsService(timeManager: AccelByteSDK.TimeManager);
            analyticsController.Initialize();
            AccelByteSDK.ClientAnalytics = analyticsController;

            analyticsController.StartAcceptEvent();
        }

        public static void Stop()
        {
            analyticsController.StopAcceptEvent();
            analyticsController.DisposeScheduler();
        }
    }
}
