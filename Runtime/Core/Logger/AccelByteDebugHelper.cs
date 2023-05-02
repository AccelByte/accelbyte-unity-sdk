// Copyright (c) 2020-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core
{
    public static class AccelByteDebugHelper
    {
        internal const string DefaultLogFormat = "[{Severity}][<b>AccelByteSDK</b>][{Time}]{Message}[/{Severity}]";
        internal const int SeverityFormatIndex = 0;
        internal const int TimeFormatIndex = 1;
        internal const int MessageFormatIndex = 2;
        internal const int StackTraceFormatIndex = 3;
        private static string logFormat = DefaultLogFormat;

        internal static void SetLogFormatting(string newFormat)
        {
            logFormat = newFormat;
        }

        internal static AccelByteLogFormat GetLogFormatting(string severity, string time, object message, string stackTrace)
        {
            var stringParams = new List<object>();
            string format = logFormat;
            if(logFormat.Contains("{Severity}"))
            {
                format = format.Replace("{Severity}", "{"+ SeverityFormatIndex + "}");
                stringParams.Add(severity);
            }
            if (logFormat.Contains("{Time}"))
            {
                format = format.Replace("{Time}", "{" + TimeFormatIndex + "}");
                stringParams.Add(time);
            }
            if (logFormat.Contains("{Message}"))
            {
                format = format.Replace("{Message}", "{" + MessageFormatIndex + "}");
                stringParams.Add(message);
            }
            if (logFormat.Contains("{StackTrace}"))
            {
                format = format.Replace("{StackTrace}", "{" + StackTraceFormatIndex + "}");
                stringParams.Add(stackTrace);
            }
            var retval = new AccelByteLogFormat()
            {
                Format = format,
                Params = stringParams.ToArray()
            };
            return retval;
        }

        internal static LogType ConvertAccelByteLogType(AccelByteLogType type)
        {
            string accelByteLogTypeStr = type.ToString();
            LogType unityLogType;
            if (!Enum.TryParse(accelByteLogTypeStr, true, out unityLogType))
            {
                unityLogType = LogType.Log;
            }
            return unityLogType;
        }

        internal static string GetCurrentTimeString()
        {
            string time = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            return time;
        }
    }
}
