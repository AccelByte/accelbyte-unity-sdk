// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace Core
{
    public class WsMessageFragmentProcessor
    {
        private string fragmentedMessageCache;

        public bool Process(string message, out string completeMessage)
        {
            // if we don't have envelope start or end set, then we assume it's a complete message
            if (string.IsNullOrEmpty(envelopeStart) || string.IsNullOrEmpty(envelopeEnd))
            {
                AccelByteDebug.Log("envelope start or end is empty, skipping message fragment processing");
                
                // remove start if it's set
                if (!string.IsNullOrEmpty(envelopeStart) && message.StartsWith(envelopeStart))
                {
                    message = message.Remove(0, envelopeStart.Length);
                }

                // remove end if it's set
                if (!string.IsNullOrEmpty(envelopeEnd) && message.EndsWith(envelopeEnd))
                {
                    message = message.Substring(0, message.Length - envelopeEnd.Length);
                }
                
                completeMessage = message;
                fragmentedMessageCache = string.Empty;
                return true;
            }

            // if this message have both start and end, then it's a complete message
            if (message.StartsWith(envelopeStart) && message.EndsWith(envelopeEnd))
            {
                completeMessage = message.Substring(envelopeStart.Length, 
                    message.Length - envelopeEnd.Length - envelopeStart.Length);
                fragmentedMessageCache = string.Empty;
                return true;
            }

            // if it reaches this part, it means it's a fragmented message
            // if doesn't have envelope end,
            if (!message.EndsWith(envelopeEnd))
            {
                // if it have start then assign to message cache
                if (message.StartsWith(envelopeStart))
                {
                    fragmentedMessageCache = message.Remove(0, envelopeStart.Length);               
                }
                // if it doesn't have a start then just append to cache
                else
                {
                    fragmentedMessageCache += message;
                }

                completeMessage = string.Empty;
                return false; // message is not complete, return false
            }
            else
            {
                // if it have envelope end then after appending to fragmented cache, it is complete
                completeMessage = fragmentedMessageCache + message.Substring(0,
                    message.Length - envelopeEnd.Length);
                fragmentedMessageCache = string.Empty;
                return true;
            }
        }

        public string envelopeEnd { get; set; }

        public string envelopeStart { get; set; }
    }
}