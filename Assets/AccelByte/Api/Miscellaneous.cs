// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.


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
    }
}