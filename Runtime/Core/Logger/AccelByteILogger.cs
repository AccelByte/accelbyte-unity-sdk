// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    public interface AccelByteILogger
    {
        public void InvokeLog(AccelByteLogType logType, object message, UnityEngine.Object context=null);
        public void InvokeException(Exception exception, UnityEngine.Object context=null);
    }
}
