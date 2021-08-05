// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Models;

namespace AccelByte.Core
{
    public interface ITokenGenerator
    {
        string Token { get; }

        event Action<string> TokenReceivedEvent;

        void RequestToken();

        bool IsValid();
    }
}