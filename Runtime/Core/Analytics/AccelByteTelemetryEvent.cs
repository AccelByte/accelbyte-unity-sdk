// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    public class AccelByteTelemetryEvent : IAccelByteTelemetryEvent
    {
        public Models.IAccelByteTelemetryPayload Payload
        {
            get;
            private set;
        }

        public System.DateTime CreatedTimestamp
        {
            get;
            private set;
        }

        public string EventName
        {
            get
            {
                string retval = string.Empty;
                if(Payload != null)
                {
                    retval = Payload.GetEventName();
                }
                return retval;
            }
        }

        public AccelByteTelemetryEvent(Models.IAccelByteTelemetryPayload payload)
        {
            Payload = payload;
            CreatedTimestamp = System.DateTime.Now;
        }
    }
}
