// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class TelemetryBody
    {
        [DataMember] public string EventName;
        [DataMember] public string EventNamespace;
        [DataMember] public object Payload;
        [DataMember(Name = "FlightId")] internal string FlightId;
        [DataMember(Name = "DeviceType")] internal string DeviceType;
        [DataMember(Name = "ClientTimestamp")] private DateTime clientTimestamp;

        public DateTime ClientTimestamp
        {
            get
            {
                return clientTimestamp;
            }
            set
            {
                clientTimestamp = value;
                IsTimeStampSet = true;
            }
        }

        internal bool IsTimeStampSet;

        internal TimeSpan? CreatedElapsedTime
        {
            get;
            private set;
        }

        public TelemetryBody()
        {
            clientTimestamp = DateTime.UtcNow;
            FlightId = AccelByteSDK.FlightId;
            DeviceType = GameCommonInfo.PlatformName;
        }

        public TelemetryBody(Core.IAccelByteTelemetryEvent telemetryEvent)
        {
            clientTimestamp = DateTime.UtcNow;
            EventName = telemetryEvent.EventName;
            Payload = telemetryEvent.Payload;
            FlightId = AccelByteSDK.FlightId;
            DeviceType = GameCommonInfo.PlatformName;
        }

        internal void SetTimeReference(TimeSpan timeReference)
        {
            CreatedElapsedTime = timeReference;
        }

        internal DateTime CalculateClientTimestampFromServerTime(AccelByteServerTimeData serverTimeData)
        {
            TimeSpan createdElapsedTime = CreatedElapsedTime != null ? CreatedElapsedTime.Value : new TimeSpan(ticks:0);
            TimeSpan timeDifference = createdElapsedTime - serverTimeData.ServerTimeSpanSinceStartup;
            DateTime retval = serverTimeData.ServerTime + timeDifference;
            return retval;
        }
    }
}