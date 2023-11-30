using System;

namespace AccelByte.Core
{
    internal class FlightIDBootstrap
    {
        internal static void Execute()
        {
            if (string.IsNullOrEmpty(AccelByteSDK.FlightId))
            {
                string newId = Guid.NewGuid().ToString();
                AccelByteSDK.FlightId = newId.Replace("-", string.Empty);
            }
        }
    }
}
