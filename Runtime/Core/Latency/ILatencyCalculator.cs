// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

namespace AccelByte.Core
{
    internal interface ILatencyCalculator
    {
        AccelByteResult<int, Error> CalculateLatency(string url, int port);
    }
    
    internal static class LatencyCalculatorFactory
    {
        public static ILatencyCalculator CreateDefaultCalculator()
        {
            return new DefaultLatencyCalculator();
        }
    }
}

