// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using UnityEngine;
using AccelByte.Api;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public static class AccelByteDebug
    {
        private static string messageTag = "<b>AccelByteSDK</b>";
        public static Lazy<ILogger> logger = new Lazy<ILogger>(() => new Logger(new AccelByteLogHandler()));
     
        static AccelByteDebug()
        {
            string filterLogTypeString = AccelBytePlugin.Config.DebugLogFilter;
            bool enableLogging = AccelBytePlugin.Config.EnableDebugLog;

            LogType filterLogType;

            if (!Enum.TryParse(filterLogTypeString, true, out filterLogType))
            {
                filterLogType = LogType.Log;

                Debug.LogWarning($"{messageTag}: Debug log filter is not valid or empty. Debug log filter will use the default log.");
            }

            // LogType level
            //  - Log (default setting) will display all log messages.
            //  - Warning will display warning, assert, error and exception log messages.
            //  - Assert will display assert, error and exception log messages.
            //  - Error will display error and exception log messages.
            //  - Exception will display exception log messages.
            SetFilterLogType(filterLogType);
            SetEnableLogging(enableLogging);
        }

        public static void SetFilterLogType(LogType type)
        {
            logger.Value.filterLogType = type;
        }

        public static void SetEnableLogging(bool enable)
        {
            logger.Value.logEnabled = enable;
        }

        public static void Log(object message)
        {
            logger.Value.Log(messageTag, message);
        }
        public static void Log(object message, Object context)
        {
            logger.Value.Log(messageTag, message, context);
        }

        public static void LogWarning(object message)
        {
            logger.Value.LogWarning(messageTag, message);
        }

        public static void LogWarning(object message, Object context)
        {
            logger.Value.LogWarning(messageTag, message, context);
        }

        public static void LogError(object message)
        {
            logger.Value.LogError(messageTag, message);
        }

        public static void LogError(object message, Object context)
        {
            logger.Value.LogError(messageTag, message, context);
        }

        public static void LogException(Exception exception)
        {
            logger.Value.LogException(exception);
        }

        public static void LogException(Exception exception, Object context)
        {
            logger.Value.LogException(exception, context);
        }

        private class AccelByteLogHandler : ILogHandler
        {
            public void LogFormat(LogType logType, Object context, string format, params object[] args)
            {
                Debug.unityLogger.logHandler.LogFormat(logType, context, format, args);
            }

            public void LogException(Exception exception, Object context)
            {
                Debug.unityLogger.LogException(exception, context);
            }
        }
    }
}
