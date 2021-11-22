// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.


using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Miscellaneous
    {
        private readonly MiscellaneousApi api;
        private readonly CoroutineRunner coroutineRunner;

        internal Miscellaneous(MiscellaneousApi api, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get server current time.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Time via callback when completed.</param>
        public void GetCurrentTime(ResultCallback<Time> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.coroutineRunner.Run(this.api.GetCurrentTime(callback));
        }
        /// <summary>
        /// Get all valid country codes for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an Array of <see cref="Country"/> via callback when completed</param>
        public void GetCountryGroups(ResultCallback<Country[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.coroutineRunner.Run(this.api.GetCountryGroups(callback));
        }
        /// <summary>
        /// Get all valid Languages for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains a Dictionary of Language Codes to Native Language via callback when completed</param>
        public void GetLanguages(ResultCallback<Dictionary<string,string>> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.coroutineRunner.Run(this.api.GetLanguages(callback));
        }
        /// <summary>
        /// Get all valid Time Zones for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an array of TimeZone strings via callback when completed</param>
        public void GetTimeZones(ResultCallback<string[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.coroutineRunner.Run(this.api.GetTimeZones(callback));
        }
        
    }
}