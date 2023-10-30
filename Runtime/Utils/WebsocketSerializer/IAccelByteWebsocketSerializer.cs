// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System.IO;

namespace AccelByte.Utils
{
    internal interface IAccelByteWebsocketSerializer
    {
        public void WriteHeader(StringWriter writer, Models.MessageType type, long id = -1, ErrorCode? code = null);

        public void WritePayload<T>(StringWriter writer, T inputPayload) where T : class;

        public ErrorCode ReadHeader(string message, out Models.MessageType type, out long id);

        public ErrorCode ReadPayload<T>(string message, out T payload) where T : class, new();
    }
}
