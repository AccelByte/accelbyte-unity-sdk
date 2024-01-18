// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    public interface IPlatformWrapper
    {
        /// <summary>
        /// This function is expected to fetch an Authentication Token from any game console platform
        /// </summary>
        /// <param name="callback">a lambda or delegate function with a string callback. It can be an error or the fetched AuthCode. 
        /// You can refers to AccelByte.Utils.PlatformHandlerUtils for serialize and deserialize the error message.</param>
        public void FetchPlatformToken(Action<string> callback);

        public string PlatformId
        {
            get;
        }
    }
}