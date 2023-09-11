// Copyright (c) 2020-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using UnityEngine;

namespace AccelByte.Core
{
    internal class AccelByteLogHandler : AccelByteILogger
    {
        public void InvokeException(Exception exception, UnityEngine.Object context=null)
        {
            Debug.LogException(exception, context);
        }

        public void InvokeLog(AccelByteLogType logType, object message, UnityEngine.Object context=null)
        {
            PrintLog(logType, message, context);
        }

        private void PrintLog(AccelByteLogType logType, object message, UnityEngine.Object context)
        {
            string stackTrace = string.Empty;
            string time = AccelByteDebugHelper.GetCurrentTimeString();
            AccelByteLogFormat logFormat = AccelByteDebugHelper.GetLogFormatting(logType.ToString(), time, message, stackTrace);
            string format = logFormat.Format;
            object[] messageParams = logFormat.Params;

            LogType unityLogType = AccelByteDebugHelper.ConvertAccelByteLogType(logType);
            switch (unityLogType)
            {
                case LogType.Error:
                    if (context != null)
                    {
                        Debug.LogErrorFormat(format, messageParams, context);
                    }
                    else
                    {
                        Debug.LogErrorFormat(format, messageParams);
                    }
                    break;
                case LogType.Assert:
                    if (context != null)
                    {
                        Debug.LogAssertionFormat(format, messageParams, context);
                    }
                    else
                    {
                        Debug.LogAssertionFormat(format, messageParams);
                    }
                    break;
                case LogType.Warning:
                    if (context != null)
                    {
                        Debug.LogWarningFormat(format, messageParams, context);
                    }
                    else
                    {
                        Debug.LogWarningFormat(format, messageParams);
                    }
                    break;
                case LogType.Log:
                    if (context != null)
                    {
                        Debug.LogFormat(format, messageParams, context);
                    }
                    else
                    {
                        Debug.LogFormat(format, messageParams);
                    }
                    break;
            }
        }
    }
}
