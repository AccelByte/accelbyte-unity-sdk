using System;

namespace AccelByte.Core
{
    internal class FlightIDBootstrap
    {
        internal static void Execute()
        {
            if (string.IsNullOrEmpty(AccelByteSDK.FlightId))
            {
                AccelByteSDK.FlightId = Guid.NewGuid().ToString();
            }
        }
    }
}
