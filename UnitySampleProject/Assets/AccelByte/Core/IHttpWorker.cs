// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AccelByte.Core {
    public interface IHttpRequest
    {
        string Method { get; }
        string Url { get; }
        Dictionary<string, string> Headers { get; }
        byte[] BodyBytes { get; }
    }

    public interface IHttpResponse 
    {
        string Url { get; }
        long Code { get; }
        byte[] BodyBytes { get; }
    }
    
    public interface IHttpWorker
    {
        event Action<UnityWebRequest> ServerErrorOccured;
        event Action<UnityWebRequest> NetworkErrorOccured;
        IEnumerator SendRequest(IHttpRequest request, Action<IHttpResponse> responseCallback);
    }
}