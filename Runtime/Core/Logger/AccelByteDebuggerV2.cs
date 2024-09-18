// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public class AccelByteDebuggerV2 : IDebugger
    {
        private List<AccelByteILogger> loggers = new List<AccelByteILogger>();

        internal AccelByteLogType currentSeverity;
        
        private Action<AccelByteLogType, object, Object> onLog;

        private Action<Exception, Object> onException;

        internal AccelByteILogger DefaultLogger
        {
            get;
            private set;
        }

        internal bool LogEnabled
        {
            get;
            private set;
        }

        internal AccelByteDebuggerV2()
        {
            DefaultLogger = new AccelByteLogHandler();
            AddLogWriter(DefaultLogger);
            
            LogEnabled = false;
        }

        internal AccelByteDebuggerV2(bool enableLog, AccelByteLogType logFilter)
        {
            DefaultLogger = new AccelByteLogHandler();
            AddLogWriter(DefaultLogger);
            
            SetEnableLogging(enableLog);
            SetFilterLogType(logFilter);
        }

        public void SetEnableLogging(bool enable)
        {
            LogEnabled = enable;
        }

        public void SetFilterLogType(AccelByteLogType type)
        {
            currentSeverity = type;
        }

        public void LogVerbose(object message, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Verbose, message, forceLog);
        }

        public void LogVerbose(object message, Object context, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Verbose, message, forceLog, context);
        }

        public void Log(object message, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Log, message, forceLog);
        }

        public void Log(object message, Object context, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Log, message, forceLog, context);
        }

        public void LogWarning(object message, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Warning, message, forceLog);
        }

        public void LogWarning(object message, Object context, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Warning, message, forceLog, context);
        }

        public void LogError(object message, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Error, message, forceLog);
        }

        public void LogError(object message, Object context, bool forceLog = false)
        {
            InvokeLog(AccelByteLogType.Error, message, forceLog, context);
        }

        public void LogException(Exception exception, Object context = null)
        {
            if (onException == null || !FilterLogSeverity(currentSeverity, AccelByteLogType.Exception) || !LogEnabled)
            {
                return;
            }
            onException?.Invoke(exception, context);
        }

        internal void SetLogWriter(AccelByteILogger newLogger = null)
        {
            if (newLogger != null)
            {
                SetLogWriters(new AccelByteILogger[] { newLogger });
            }
            else
            {
                ClearLoggers();
            }
        }

        internal void SetLogWriters(AccelByteILogger[] newLoggers)
        {
            ClearLoggers();
            AddLogWriters(newLoggers);
        }

        internal void AddLogWriter(AccelByteILogger newLogger)
        {
            AddLogWriters(new AccelByteILogger[] { newLogger });
        }

        internal void AddLogWriters(AccelByteILogger[] newLoggers)
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

        internal void RemoveLogger(AccelByteILogger loggerToRemove)
        {
            RemoveLoggers(new AccelByteILogger[] { loggerToRemove });
        }

        internal void RemoveLoggers(AccelByteILogger[] loggersToRemove)
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

        internal AccelByteILogger[] GetLoggers()
        {
            return loggers.ToArray();
        }

        internal void SetFilterLogType(LogType type)
        {
            string unityLogTypeStr = type.ToString();
            if (!Enum.TryParse(unityLogTypeStr, true, out currentSeverity))
            {
                currentSeverity = AccelByteLogType.Log;
                throw new System.InvalidOperationException($"Failed assign Unity log severity type : {type}.");
            }
        }

        private void ClearLoggers()
        {
            onLog = null;
            onException = null;
            loggers.Clear();
        }

        // LogType level
        //  - Log (default setting) will display all log messages.
        //  - Warning will display warning, assert, error and exception log messages.
        //  - Assert will display assert, error and exception log messages.
        //  - Error will display error and exception log messages.
        //  - Exception will display exception log messages.
        private bool FilterLogSeverity(AccelByteLogType activeSeverity, AccelByteLogType logSeverity)
        {
            return logSeverity <= activeSeverity;
        }

        private void InvokeLog(AccelByteLogType logType, object message, bool forceLog, Object context = null)
        {
            bool isLogAccepted = FilterLogSeverity(currentSeverity, logType) || forceLog;
            if (onLog == null || !isLogAccepted || !LogEnabled)
            {
                return;
            }
            onLog?.Invoke(logType, message, context);
        }
    }
}
