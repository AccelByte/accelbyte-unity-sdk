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
        private readonly User user;
        private readonly string clientId;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal Telemetry(TelemetryApi api, User user, string clientId, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can't construct purchase manager! PurchaseService parameter is null!");
            Assert.IsNotNull(user, "Can't construct purchase manager! UserAccount parameter is null!");
            Assert.IsNotNull(clientId, "clientId must not be null");
            Assert.IsNotNull(taskDispatcher, "taskReactor must not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner must not be null");

            this.api = api;
            this.user = user;
            this.clientId = clientId;
            this.taskDispatcher = taskDispatcher;
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
            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            if (eventData is string)
            {
                callback.TryError(ErrorCode.InvalidRequest, "string is not allowed as event data");

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.SendEvent(
                        this.user.Namespace,
                        this.clientId,
                        this.user.UserId,
                        eventTag,
                        eventData,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this.user));
        }
    }
}