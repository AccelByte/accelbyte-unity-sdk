// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Utils
{
    public interface IApiTracker
    {
        void NewHttpRequestScheduled(IHttpRequest request);
        void NewHttpRequestSent(IHttpRequest request);
        void NewHttpResponseReceived(IHttpRequest request, IHttpResponse response);
    }
}