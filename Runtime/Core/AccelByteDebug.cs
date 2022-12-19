// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using UnityEngine;
using AccelByte.Api;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public enum AccelByteLogType
    {
        Error = 0,
        Assert = 1,
        Warning = 2,
        Log = 3,
        Verbose = 4,
        Exception = 5
    }

    public static class AccelByteDebugHelper
    {
        private static bool printStackTrace = false;

        public static void SetPrintStackTrace(bool enable)
        {
            printStackTrace = enable;
        }

        public static bool GetIsPrintStackTrace()
        {
            return printStackTrace;
        }

        public static string GetSeverityLogConfig()
        {
            if(AccelBytePlugin.Config == null)
            {
                return AccelByteLogType.Verbose.ToString();
            }
            return AccelBytePlugin.Config.DebugLogFilter;
        }

        public static bool GetEnableLogConfig()
        {
            if (AccelBytePlugin.Config == null)
            {
                return true;
            }
            return AccelBytePlugin.Config.EnableDebugLog;
        }
    }

    public static class AccelByteDebug
    {
        public static Lazy<ILogger> logger = new Lazy<ILogger>(() => new Logger(new AccelByteLogHandler()));
        
        private static string messageTagFormat = "[Severity][<b>AccelByteSDK</b>][Time]";
        private static AccelByteLogType currentSeverity;

        static AccelByteDebug()
        {
            string filterLogTypeString = AccelByteDebugHelper.GetSeverityLogConfig();
            bool enableLogging = AccelByteDebugHelper.GetEnableLogConfig();

            if (!Enum.TryParse(filterLogTypeString, true, out currentSeverity))
            {
                currentSeverity = AccelByteLogType.Verbose;

                object warningMessage = $"{GetMessageTag(AccelByteLogType.Warning)}: Debug log filter is not valid or empty. Debug log filter will use the default verbose.";
                warningMessage = AppendMessageTrailInfo(warningMessage, AccelByteLogType.Warning, true);
                Debug.LogWarning(warningMessage);
            }

            SetEnableLogging(enableLogging);
            SetFilterLogType(currentSeverity);
        }

        public static void SetEnableLogging(bool enable)
        {
            logger.Value.logEnabled = enable;
        }

        public static void SetFilterLogType(LogType type)
        {
            string unityLogTypeStr = type.ToString();
            if(!Enum.TryParse(unityLogTypeStr, true, out currentSeverity))
            {
                currentSeverity = AccelByteLogType.Log;
                logger.Value.filterLogType = LogType.Log;
                object warningMessage = $"{GetMessageTag(AccelByteLogType.Warning)}: Failed assign Unity log severity type : {type}.";
                warningMessage = AppendMessageTrailInfo(warningMessage, AccelByteLogType.Warning, true);
                Debug.LogWarning(warningMessage);
            }
            else
            {
                logger.Value.filterLogType = type;
            }
        }

        public static void SetFilterLogType(AccelByteLogType type)
        {
            currentSeverity = type;
            logger.Value.filterLogType = ConvertAccelByteLogType(type);
        }

        public static void LogVerbose(object message)
        {
            InvokeLog(AccelByteLogType.Verbose, message);
        }

        public static void LogVerbose(object message, Object context)
        {
            InvokeLog(AccelByteLogType.Verbose, message, context);
        }

        public static void Log(object message)
        {
            InvokeLog(AccelByteLogType.Log, message);
        }

        public static void Log(object message, Object context)
        {
            InvokeLog(AccelByteLogType.Log, message, context);
        }

        public static void LogWarning(object message)
        {
            InvokeLog(AccelByteLogType.Warning, message);
        }

        public static void LogWarning(object message, Object context)
        {
            InvokeLog(AccelByteLogType.Warning, message, context);
        }

        public static void LogError(object message)
        {
            InvokeLog(AccelByteLogType.Error, message);
        }

        public static void LogError(object message, Object context)
        {
            InvokeLog(AccelByteLogType.Error, message, context);
        }

        public static void LogException(Exception exception)
        {
            if (!FilterLogSeverity(currentSeverity, AccelByteLogType.Exception))
            {
                return;
            }
            logger.Value.LogException(exception);
        }

        public static void LogException(Exception exception, Object context)
        {
            if (!FilterLogSeverity(currentSeverity, AccelByteLogType.Exception))
            {
                return;
            }
            logger.Value.LogException(exception, context);
        }

        private static object AppendMessageTrailInfo(object message, AccelByteLogType severity, bool appendStackTrace)
        {
            var retval = message;
            if (appendStackTrace)
            {
                string stackTrace = StackTraceUtility.ExtractStackTrace();
                retval = $"{retval}\n{stackTrace}";
            }
            retval = $"{retval}[/{severity}]";
            return retval;
        }

        // LogType level
        //  - Log (default setting) will display all log messages.
        //  - Warning will display warning, assert, error and exception log messages.
        //  - Assert will display assert, error and exception log messages.
        //  - Error will display error and exception log messages.
        //  - Exception will display exception log messages.
        private static bool FilterLogSeverity(AccelByteLogType activeSeverity, AccelByteLogType logSeverity)
        {
            return logSeverity <= activeSeverity;
        }

        private static string GetMessageTag(AccelByteLogType severity)
        {
            string retval = messageTagFormat;
            retval = retval.Replace("[Time]", "[" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + "]");
            retval = retval.Replace("[Severity]", "[" + severity.ToString() + "]");
            return retval;
        }

        private static void InvokeLog(AccelByteLogType logType, object message)
        {
            if (!FilterLogSeverity(currentSeverity, logType))
            {
                return;
            }
            logger.Value.Log(ConvertAccelByteLogType(logType), GetMessageTag(logType), AppendMessageTrailInfo(message, logType, AccelByteDebugHelper.GetIsPrintStackTrace()));
        }

        private static void InvokeLog(AccelByteLogType logType, object message, Object context)
        {
            if (!FilterLogSeverity(currentSeverity, logType))
            {
                return;
            }
            logger.Value.Log(ConvertAccelByteLogType(logType), GetMessageTag(logType), AppendMessageTrailInfo(message, logType, AccelByteDebugHelper.GetIsPrintStackTrace()), context);
        }

        private static LogType ConvertAccelByteLogType(AccelByteLogType type)
        {
            string accelByteLogTypeStr = type.ToString();
            LogType unityLogType;
            if (!Enum.TryParse(accelByteLogTypeStr, true, out unityLogType))
            {
                unityLogType = LogType.Log;
            }
            return unityLogType;
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
