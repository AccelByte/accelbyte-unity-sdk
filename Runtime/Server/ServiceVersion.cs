// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServiceVersion : WrapperBase, IServiceVersion
    {
        private readonly ServiceVersionApi api;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServiceVersion(ServiceVersionApi inApi, ISession inLoginSession, CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "inApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner==null (@ constructor)");

            api = inApi;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Get a service's version information.
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        /// <param name="callback">Returns a Result when completed</param>
        public void GetVersionInfo(string serviceName, ResultCallback<ServiceVersionInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(
                api.GetVersionInfo(serviceName, callback));
        }
    }
}
