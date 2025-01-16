// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    public class AccelByteWebRequest : UnityWebRequest
    {
        internal const string ResponseContentTypeHeader = "Content-Type";
        public string RequestId;
        public DateTime SentTimestamp;
        public DateTime ResponseTimestamp;

        public AccelByteWebRequest(Uri uri, string method) : base(uri, method)
        {
        }
    }
}