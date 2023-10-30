// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.IO;

namespace AccelByte.Utils
{
	internal static class WebsocketUtils
    {
        internal static string CreateMessage<T>(this IAccelByteWebsocketSerializer serializer, long messageId, Models.MessageType requestType
            , T requestPayload) where T : class, new()
        {
            var writer = new StringWriter();
            serializer.WriteHeader(writer, requestType, messageId);
            serializer.WritePayload(writer, requestPayload);

            return writer.ToString();
        }

        internal static string CreateMessage(this IAccelByteWebsocketSerializer serializer, long messageId, Models.MessageType requestType)
        {
            var writer = new StringWriter();
            serializer.WriteHeader(writer, requestType, messageId);

            return writer.ToString();
        }
    }
}
