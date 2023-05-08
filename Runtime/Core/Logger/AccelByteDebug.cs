// Copyright (c) 2020-2023 AccelByte Inc. All Rights Reserved.
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
        private static Lazy<AccelByteILogger> logger = new Lazy<AccelByteILogger>(() => DefaultLogger);
        
        private static AccelByteLogType currentSeverity;

        private static readonly AccelByteILogger defaultLogger = new AccelByteLogHandler();

        internal static AccelByteILogger DefaultLogger
        {
            get
            {
                return defaultLogger;
            }
        }

        private static bool logEnabled;

        static AccelByteDebug()
        {
            SetEnableLogging(true);
            SetFilterLogType(AccelByteLogType.Verbose);
        }

        public static void SetEnableLogging(bool enable)
        {
            logEnabled = enable;
        }

        internal static void SetLogger(AccelByteILogger newLogger)
        {
            if(newLogger != null)
            {
                logger = new Lazy<AccelByteILogger>(() => newLogger);
                SetFilterLogType(currentSeverity);
            }
            else
            {
                logger = null;
            }
        }

        internal static AccelByteILogger GetLogger()
        {
            return logger.Value;
        }

        public static void SetFilterLogType(LogType type)
        {
            string unityLogTypeStr = type.ToString();
            if(!Enum.TryParse(unityLogTypeStr, true, out currentSeverity))
            {
                currentSeverity = AccelByteLogType.Log;
                throw new System.InvalidOperationException($"Failed assign Unity log severity type : {type}.");
            }
        }

        public static void SetFilterLogType(AccelByteLogType type)
        {
            currentSeverity = type;
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
            if (logger == null || !FilterLogSeverity(currentSeverity, AccelByteLogType.Exception) || !logEnabled)
            {
                return;
            }
            logger.Value.InvokeException(exception);
        }

        public static void LogException(Exception exception, Object context)
        {
            if (logger == null || !FilterLogSeverity(currentSeverity, AccelByteLogType.Exception) || !logEnabled)
            {
                return;
            }
            logger.Value.InvokeException(exception, context);
        }

        private static object AppendMessageTrailInfo(object message, AccelByteLogType severity)
        {
            object retval = $"{message}[/{severity}]";
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

        private static void InvokeLog(AccelByteLogType logType, object message)
        {
            if (logger == null || !FilterLogSeverity(currentSeverity, logType) || !logEnabled)
            {
                return;
            }
            logger.Value.InvokeLog(logType, message);
        }

        private static void InvokeLog(AccelByteLogType logType, object message, Object context)
        {
            if (logger == null || !FilterLogSeverity(currentSeverity, logType) || !logEnabled)
            {
                return;
            }
            logger.Value.InvokeLog(logType, message, context);
        }
    }
}
