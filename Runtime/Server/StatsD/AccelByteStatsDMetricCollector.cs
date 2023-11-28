// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Server;

public class AccelByteStatsDMetricCollector : IAccelByteStatsDMetricCollector
{
    public AccelByteStatsDMetricCollector()
    {
    }

    public int GetPlayerCapacity()
    {
        int playerCapacity = 0;
        AccelByteDebug.LogWarning($"GetPlayerCapacity is not implemented");
        return playerCapacity;
    }

    public int GetPlayerCount()
    {
        int playerCount = 0;
        AccelByteDebug.LogWarning($"GetPlayerCount is not implemented");
        return playerCount;
    }

    public int GetAiCount()
    {
        int aiCount = 0;
        AccelByteDebug.LogWarning($"GetAiCount is not implemented");
        return aiCount;
    }

    public int GetClientCount()
    {
        int clientCount = 0;
        AccelByteDebug.LogWarning($"GetClientCount is not implemented");
        return clientCount;
    }

    public double GetFrameStartDelayAverage()
    {
        double frameStartDelayAverage = 0;
        AccelByteDebug.LogWarning($"GetFrameStartDelayAverage is not implemented");
        return frameStartDelayAverage;
    }

    public double GetFrameStartDelayMax()
    {
        double frameStartDelayMax = 0;
        AccelByteDebug.LogWarning($"GetFrameStartDelayMax is not implemented");
        return frameStartDelayMax;
    }

    public double GetFrameTimeAverage()
    {
        double frameTimeAverage = 0;
        AccelByteDebug.LogWarning($"GetFrameTimeAverage is not implemented");
        return frameTimeAverage;
    }

    public double GetFrameTimeMax()
    {
        double frameTimeMax = 0;
        AccelByteDebug.LogWarning($"GetFrametimeMax is not implemented");
        return frameTimeMax;
    }
}
