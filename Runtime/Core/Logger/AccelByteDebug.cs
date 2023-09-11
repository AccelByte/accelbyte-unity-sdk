// Copyright (c) 2020-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public static class AccelByteDebug
    {
        private static Lazy<AccelByteILogger> logger = new Lazy<AccelByteILogger>(() => DefaultLogger);

        private static List<AccelByteILogger> loggers = new List<AccelByteILogger>();
        
        private static AccelByteLogType currentSeverity;

        private static readonly AccelByteILogger defaultLogger = new AccelByteLogHandler();

        private static Action<AccelByteLogType, object, Object> onLog;

        private static Action<Exception, Object> onException;

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

            AddLogger(logger.Value);
        }

        public static void SetEnableLogging(bool enable)
        {
            logEnabled = enable;
        }

        internal static void SetLogger(AccelByteILogger newLogger=null)
        {
            if(newLogger != null)
            {
                SetLoggers(new AccelByteILogger[] { newLogger });
            }
            else
            {
                ClearLoggers();
            }
        }

        internal static void SetLoggers(AccelByteILogger[] newLoggers)
        {
            ClearLoggers();

            AddLoggers(newLoggers);

            SetFilterLogType(currentSeverity);
        }

        private static void ClearLoggers()
        {
            onLog = null;
            onException = null;
            loggers.Clear();
        }

        internal static void AddLogger(AccelByteILogger newLogger)
        {
            AddLoggers(new AccelByteILogger[] { newLogger });
        }

        internal static void AddLoggers(AccelByteILogger[] newLoggers)
        {
            foreach (var logger in newLoggers)
            {
                if (!loggers.Contains(logger))
                {
                    onLog += logger.InvokeLog;
                    onException += logger.InvokeException;
                    loggers.Add(logger);
                }
            }
        }

        internal static void RemoveLogger(AccelByteILogger loggerToRemove)
        {
            RemoveLoggers(new AccelByteILogger[] { loggerToRemove });
        }

        internal static void RemoveLoggers(AccelByteILogger[] loggersToRemove)
        {
            foreach (var logger in loggersToRemove)
            {
                if (loggers.Contains(logger))
                {
                    onLog -= logger.InvokeLog;
                    onException -= logger.InvokeException;
                    loggers.Remove(logger);
                }
            }
        }

        internal static AccelByteILogger[] GetLoggers()
        {
            return loggers.ToArray();
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

        public static void LogException(Exception exception, Object context=null)
        {
            if (onException == null || !FilterLogSeverity(currentSeverity, AccelByteLogType.Exception) || !logEnabled)
            {
                return;
            }
            onException?.Invoke(exception, context);
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

        private static void InvokeLog(AccelByteLogType logType, object message, Object context=null)
        {
            if (onLog == null || !FilterLogSeverity(currentSeverity, logType) || !logEnabled)
            {
                return;
            }
            onLog?.Invoke(logType, message, context);
        }
    }
}
