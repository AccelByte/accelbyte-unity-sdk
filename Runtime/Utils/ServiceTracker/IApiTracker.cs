// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Utils
{
    public interface IApiTracker
    {
        void NewHttpRequestScheduled(string apiMethod, string apiUrl);
        void NewHttpRequestSent(string apiMethod, string apiUrl);
    }
}