// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccelByte.Utils
{
    public class PlatformHandlerUtils
    {
        public const string PlatformHandlerErrorTag = "[Error]";
        public const char PlatformHandlerErrorDelimiter = ';';

        /// <summary>
        /// A function to ease user while building a consistent Error code from PlatformHandler
        /// </summary>
        /// <param name="errorCode">It is recommended to use the error code from AccelByte.Core.ErrorCode</param>
        /// <param name="message">Any descriptive error message</param>
        /// <returns></returns>
        public static string SerializeErrorMessage(string errorCode, string message)
        {   
            string result = "";
            if (!string.IsNullOrEmpty(errorCode))
            {
                result += PlatformHandlerErrorTag +
                    PlatformHandlerErrorDelimiter +
                    "Code:" + errorCode +
                    PlatformHandlerErrorDelimiter +
                    "Message:" + message;
            }
            return result;
        }

        /// <summary>
        /// Deserialize or break down PlatformHandlers error message callback and returns it in form of array of string
        /// </summary>
        /// <param name="message">This is the callback received from PlatformHandler</param>
        /// <returns>Array of strings as follows
        /// [0] Prefix
        /// [1] ErrorCode
        /// [2] Message/Description</returns>
        public static string[] DeserializeErrorMessage(string message)
        {
            List<string> result = new List<string>();

            string[] messagesArr = message.Split(PlatformHandlerErrorDelimiter);
            if (messagesArr.Length > 0)
            {
                foreach(string line in messagesArr)
                {
                    int colonIndex = line.IndexOf(':') != -1 ? 
                        line.IndexOf(':')+1 :  0;                    
                    result.Add(line.Substring(colonIndex));
                }
            }

            return result.ToArray();
        }
    }
}
