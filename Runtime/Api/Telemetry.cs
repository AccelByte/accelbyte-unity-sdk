// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Telemetry : WrapperBase
    {
        private readonly TelemetryApi api;
        private readonly UserSession session;
        private readonly string clientId;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Telemetry( TelemetryApi inApi
            , UserSession inSession
            , string inClientId
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");
            Assert.IsFalse(string.IsNullOrEmpty(inClientId), "clientId paramater couldn't be empty");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null. Construction failed");

            api = inApi;
            session = inSession;
            clientId = inClientId;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inClientId"></param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
        internal Telemetry( TelemetryApi inApi
            , UserSession inSession
            , string inNamespace
            , string inClientId
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inClientId, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Send any data with class T to Telemetry service. The data type must be serializable by implementing
        /// DataContract, Preserve attribute.
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="eventData">Event data</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        /// <typeparam name="T">A class that implements DataContract, Preserve and DataMember attribute</typeparam>
        public void SendEvent<T>( TelemetryEventTag eventTag
            , T eventData
            , ResultCallback callback) 
            where T : class
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SendEvent(clientId, session.UserId, eventTag, eventData, callback));
        }
    }
}