// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Miscellaneous : WrapperBase, IClientMiscellaneousWrapper
    {
        private readonly ClientMiscellaneousApi api;
        private readonly CoroutineRunner coroutineRunner;

        static private System.DateTime LastServerTime;
        static private System.TimeSpan ServerTimestamp;

        [UnityEngine.Scripting.Preserve]
        internal Miscellaneous( ClientMiscellaneousApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            coroutineRunner = inCoroutineRunner;
            LastServerTime = System.DateTime.MinValue;
            ServerTimestamp = System.TimeSpan.MinValue;
        }

        /// <summary>
        /// Get server current time.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Time via callback when completed.</param>
        public void GetCurrentTime( ResultCallback<Time> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            api.GetCurrentTime(
                (Result<Time> result) =>
                {
                    if (!result.IsError)
                    {
                        ServerTimestamp = System.TimeSpan.FromSeconds(UnityEngine.Time.realtimeSinceStartup);
                        LastServerTime = result.Value.currentTime;
                    }
                    callback.Try(result);
                });
        }

        /// <summary>
        /// Get server current time back calculated from the last cached query
        /// </summary>
        /// <param name="callback">Returns a Result that contains Time via callback when completed.</param>
        public void GetBackCalculatedServerTime(ResultCallback<Time> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if(ServerTimestamp == System.TimeSpan.MinValue)
            {
                GetCurrentTime(callback);
            }
            else
            {
                System.TimeSpan LocalTimeSpan = System.TimeSpan.FromSeconds(UnityEngine.Time.realtimeSinceStartup);
                System.TimeSpan ServerTimeSpan = LocalTimeSpan - ServerTimestamp;
                Time Value =  new Time();
                Value.currentTime = LastServerTime + ServerTimeSpan;
                Result<Time> result = Result<Time>.CreateOk(Value);
                callback.Try(result);
            }
        }

        /// <summary>
        /// Get all valid Languages for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains a Dictionary of Language Codes to Native Language via callback when completed</param>
        public void GetLanguages( ResultCallback<Dictionary<string,string>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            api.GetLanguages(callback);
        }
        /// <summary>
        /// Get all valid Time Zones for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an array of TimeZone strings via callback when completed</param>
        public void GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            api.GetTimeZones(callback);
        }
        
    }
}