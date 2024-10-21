// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Threading.Tasks;
using System;
using AccelByte.Core;
using System.Collections;
using UnityEngine;

namespace AccelByte.Utils
{
    public static class RetryBackoffUtils
    {
        /// <summary>
        /// Coroutine ver. This function is to let a Task Operation to perform retry backoff mechanism
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation"></param>
        /// <param name="initialDelayInSeconds"></param>
        /// <param name="maxDelayInSeconds"></param>
        /// <param name="maxRetries"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IEnumerator RunCoroutine<T>(Task<T> operation,
            int initialDelayInSeconds = 1,
            int maxDelayInSeconds = 2,
            int maxRetries = 5,
            IDebugger logger = null)
        {
            int retryCount = 0;
            int currentDelay = initialDelayInSeconds;

            while (retryCount < maxRetries)
            {
                yield return new WaitUntil(() => operation.IsCompleted);
                T result = operation.Result;
                if (IsOperationSuccess(result))
                {
                    yield break;
                }

                retryCount++;
                if (retryCount < maxRetries)
                {
                    logger?.LogVerbose($"[{retryCount}/{maxRetries}] Websocket send failed, attempting to retry.");

                    // Calculate the next delay using exponential backoff
                    currentDelay = Math.Min(currentDelay * 2, maxDelayInSeconds);
                    yield return new WaitForSeconds(currentDelay);
                }
            }
        }

        /// <summary>
        /// This function is to let a Task Operation to perform retry backoff mechanism
        /// PLEASE BE AWARE 
        /// The return value for a successfull IsOperationSuccess is the result itself
        /// If the function still doesn't work after max number of retires, then it will returned into default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation"></param>
        /// <param name="initialDelayInMs"></param>
        /// <param name="maxDelayInMs"></param>
        /// <param name="maxRetries"></param>
        /// <returns></returns>
        public static async Task<T> Run<T>(Func<Task<T>> operation,
            int initialDelayInMs = 1000,
            int maxDelayInMs = 2000,
            int maxRetries = 5,
            IDebugger logger = null)
        {
            int retryCount = 0;
            int currentDelay = initialDelayInMs;

            while (retryCount < maxRetries)
            {
                try
                {
                    T result = await operation();
                    if (IsOperationSuccess(result))
                    {
                        return result;
                    }
                    else
                    {
                        throw new Exception("[RetryBackoffUtils] The operation is not a success");
                    }
                }
                catch (Exception e) 
                {
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        logger?.LogVerbose($"[{retryCount}/{maxRetries}] Received Exception {e.Message}. Attempting to retry");

                        // Calculate the next delay using exponential backoff
                        currentDelay = Math.Min(currentDelay * 2, maxDelayInMs);
                        await Task.Delay(currentDelay);
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// This is the validator for the rety backoff
        /// This function should be adjusted in the future
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool IsOperationSuccess<T>(T result)
        {
            if (result is bool boolResult)
            {
                return boolResult;
            }
            else if (result is int intResult)
            {
                return intResult == 0;
            }

            return true;
        }
    }
}