// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public interface ICommonMiscellaneousWrapper
    {
        /// <summary>
        /// Get server current time.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Time via callback when completed.</param>
        void GetCurrentTime(ResultCallback<Time> callback);
        
        /// <summary>
        /// Get all valid Languages for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains a Dictionary of Language Codes to Native Language via callback when completed</param>
        void GetLanguages(ResultCallback<Dictionary<string, string>> callback);

        /// <summary>
        /// Get all valid Time Zones for User Registration
        /// </summary>
        /// <param name="callback">Returns a Result that contains an array of TimeZone strings via callback when completed</param>
        void GetTimeZones(ResultCallback<string[]> callback);
    }
}