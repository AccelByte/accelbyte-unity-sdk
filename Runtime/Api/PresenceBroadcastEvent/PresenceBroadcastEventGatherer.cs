// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Api
{
    public class PresenceBroadcastEventGatherer
    {
        internal const string EventName = "enhanced_presence";
        private Dictionary<string, object> presenceBroadcastEventAdditionalData = new Dictionary<string, object>();
        private static string flightId = string.Empty;

        internal static string GenerateFlightID()
        {
            return System.Guid.NewGuid().ToString();
        }

        internal static string GetFlightID()
        {
            if (string.IsNullOrEmpty(flightId))
            {
                flightId = GenerateFlightID();
            }

            return flightId;
        }

        internal string GetPlatformName()
        {
            return DeviceProvider.GetPlatformName();
        }

        internal void AddAdditionalData(string key, string value)
        {
            if (presenceBroadcastEventAdditionalData.ContainsKey(key))
            {
                presenceBroadcastEventAdditionalData[key] = value;
            }
            else
            {
                presenceBroadcastEventAdditionalData.Add(key, value);
            }
        }

        internal bool RemoveAdditionalData(string key)
        {
            bool successfullyRemoved = presenceBroadcastEventAdditionalData.Remove(key);
            return successfullyRemoved;
        }

        internal Dictionary<string, object> GetAdditionalData()
        {
            return presenceBroadcastEventAdditionalData;
        }

    }

}