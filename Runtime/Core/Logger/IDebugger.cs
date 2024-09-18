// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public interface IDebugger
    {
        void SetEnableLogging(bool enable);
        void SetFilterLogType(AccelByteLogType type);
        void LogVerbose(object message, bool forceLog = false);
        public void LogVerbose(object message, Object context, bool forceLog = false);
        public void Log(object message, bool forceLog = false);
        public void Log(object message, Object context, bool forceLog = false);
        public void LogWarning(object message, bool forceLog = false);
        public void LogWarning(object message, Object context, bool forceLog = false);
        public void LogError(object message, bool forceLog = false);
        public void LogError(object message, Object context, bool forceLog = false);
        public void LogException(Exception exception, Object context = null);
    }
}