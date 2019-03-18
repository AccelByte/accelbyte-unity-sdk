// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Threading;

namespace AccelByte.Core
{
    public interface ITask
    {
        event Action Completed;
        WaitHandle GetAwaiter();
        bool Execute();
    }
}