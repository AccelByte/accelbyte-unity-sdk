// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Miscellaneous : WrapperBase
    {
        private readonly MiscellaneousApi api;
        private readonly CoroutineRunner coroutineRunner;

        internal Miscellaneous( MiscellaneousApi inApi
            , IUserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Get server current time.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Time via callback when completed.</param>
        public void GetCurrentTime( ResultCallback<Time> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(api.GetCurrentTime(callback));
        }
        /// <summary>
        /// Get all valid country codes for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an Array of <see cref="Country"/> via callback when completed</param>
        public void GetCountryGroups( ResultCallback<Country[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(api.GetCountryGroups(callback));
        }
        /// <summary>
        /// Get all valid Languages for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains a Dictionary of Language Codes to Native Language via callback when completed</param>
        public void GetLanguages( ResultCallback<Dictionary<string,string>> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(api.GetLanguages(callback));
        }
        /// <summary>
        /// Get all valid Time Zones for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an array of TimeZone strings via callback when completed</param>
        public void GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(api.GetTimeZones(callback));
        }
        
    }
}