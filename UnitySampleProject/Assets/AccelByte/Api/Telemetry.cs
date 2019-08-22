// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Telemetry
    {
        private readonly TelemetryApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly string clientId;
        private readonly CoroutineRunner coroutineRunner;

        internal Telemetry(TelemetryApi api, ISession session, string @namespace, string clientId,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsFalse(string.IsNullOrEmpty(clientId), "clientId paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.clientId = clientId;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Send any data with class T to Telemetry service. The data type must be serializable by implementing
        /// DataContract attribute.
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="eventData">Event data</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        /// <typeparam name="T">A class that implements DataContract and DataMember attribute</typeparam>
        public void SendEvent<T>(TelemetryEventTag eventTag, T eventData, ResultCallback callback) where T : class
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SendEvent(this.@namespace, this.clientId, this.session.UserId, eventTag, eventData, callback));
        }
    }
}