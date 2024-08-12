// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    public interface IClientMiscellaneousWrapper : ICommonMiscellaneousWrapper
    {
        /// <summary>
        /// Get server current time back calculated from the last cached query
        /// </summary>
        /// <param name="callback">Returns a Result that contains Time via callback when completed.</param>
        void GetBackCalculatedServerTime(ResultCallback<Time> callback);
    }
}