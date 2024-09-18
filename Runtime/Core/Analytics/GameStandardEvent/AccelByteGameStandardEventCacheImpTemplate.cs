// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public abstract class AccelByteGameStandardEventCacheImpTemplate
    {
        private IDebugger logger;

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
        }
        
        internal virtual void LoadCache(string environment, Action<List<TelemetryBody>> callback)
        {
            LoadTelemetryEventsCache(environment, (fileText) =>
            {
                if (string.IsNullOrEmpty(fileText))
                {
                    callback?.Invoke(null);
                    return;
                }

                DecryptCache(fileText, (plainText) =>
                {
                    System.Collections.Generic.List<TelemetryBody> queueCollection = null;
                    try
                    {
                        queueCollection = plainText.ToObject<System.Collections.Generic.List<TelemetryBody>>();
                    }
                    catch (System.Exception ex)
                    {
                        logger?.LogWarning($"Failed loading standard event file.\n{ex.Message}");
                    }
                    callback?.Invoke(queueCollection);
                });
            });
        }

        internal virtual void SaveToCache(string environment, List<TelemetryBody> telemetries, Action<bool> callback)
        {
            if(telemetries == null || telemetries.Count == 0)
            {
                callback?.Invoke(false);
                return;
            }

            string telemetryJson = telemetries.ToJsonString();
            EncryptCache(telemetryJson, (cipherText) =>
            {
                CacheTelemetryEvents(cipherText, environment, callback);
            });
        }

        internal abstract void DeleteCache(string environment);

        protected abstract void EncryptCache(string content, Action<string> callback);

        protected abstract void DecryptCache(string content, Action<string> callback);

        protected abstract void CacheTelemetryEvents(string content, string environment, Action<bool> callback);

        protected abstract void LoadTelemetryEventsCache(string environment, Action<string> callback);
    }
}