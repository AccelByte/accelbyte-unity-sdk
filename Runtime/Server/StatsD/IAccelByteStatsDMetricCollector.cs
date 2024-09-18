// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Server
{
    public interface IAccelByteStatsDMetricCollector
    {
        // Basic Metrics
        public int GetPlayerCapacity();
        public int GetPlayerCount();
        public int GetClientCount();
        public int GetAiCount();

        // Performance Metrics
        public double GetFrameTimeAverage();
        public double GetFrameTimeMax();
        public double GetFrameStartDelayAverage();
        public double GetFrameStartDelayMax();

        void SetLogger(IDebugger logger);
    }
}