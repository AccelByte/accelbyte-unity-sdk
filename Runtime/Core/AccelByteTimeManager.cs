// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Diagnostics;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    public class AccelByteTimeManager
    {
        public Action<AccelByteServerTimeData> OnServerTimeUpdated;
        public Action<bool> OnFetchServerTimeComplete;

        private Stopwatch timelapseSinceSessionStart;

        public Stopwatch TimelapseSinceSessionStart
        {
            get
            {
                return timelapseSinceSessionStart;
            }
        }

        private AccelByteServerTimeData cachedServerTimeData;

        private bool isFetchingServerTime;

        public AccelByteTimeManager()
        {
            StartGameSession();
        }
        
        ~AccelByteTimeManager()
        {
            StopGameSession();
        }
        
        public AccelByteResult<AccelByteServerTimeData, Error> FetchServerTime(ICommonMiscellaneousWrapper miscWrapper)
        {
            Assert.IsNotNull(TimelapseSinceSessionStart, "Stopwatch must be initialized on game startup");
            
            var result = new AccelByteResult<AccelByteServerTimeData, Error>();
            
            if (!isFetchingServerTime)
            {
                isFetchingServerTime = true;
                miscWrapper.GetCurrentTime(callbackResult =>
                {
                    OnReceiveGetServerTimeCallback(result, callbackResult);
                });
            }
            else
            {
                result.Reject(new Error(ErrorCode.TooManyRequests, "Another fetching server time request is active"));
            }
            
            return result;
        }
        
        public void SetCachedServerTime(AccelByteServerTimeData newServerTime)
        {
            cachedServerTimeData = newServerTime;
        }
        
        public AccelByteServerTimeData GetCachedServerTime()
        {
            return cachedServerTimeData;
        }

        internal DateTime GetCurrentTime()
        {
            DateTime currentTime = DateTime.UtcNow;
            if (cachedServerTimeData != null)
            {
                TimeSpan elapsedTimeSinceSessionStart = TimelapseSinceSessionStart.Elapsed;
                TimeSpan timeDifference = elapsedTimeSinceSessionStart - cachedServerTimeData.ServerTimeSpanSinceStartup;
                currentTime = cachedServerTimeData.ServerTime + timeDifference;
            }

            return currentTime;
        }

        private void OnReceiveGetServerTimeCallback(AccelByteResult<AccelByteServerTimeData, Error> result, Result<Time> callbackResult)
        {
            isFetchingServerTime = false;

            if (timelapseSinceSessionStart == null || !timelapseSinceSessionStart.IsRunning)
            {
                result.Reject(new Error(ErrorCode.InvalidSession, "Game session is stopped"));
                OnFetchServerTimeComplete?.Invoke(false);
                return;
            }

            Error error = null;
            if (!callbackResult.IsError)
            {
                if (callbackResult.Value == null)
                {
                    error = new Error(ErrorCode.InvalidResponse, "Unexpected get time response");
                }
                else
                {
                    cachedServerTimeData = new AccelByteServerTimeData();
                    cachedServerTimeData.ServerTime = callbackResult.Value.currentTime;
                    cachedServerTimeData.ServerTimeSpanSinceStartup = TimelapseSinceSessionStart.Elapsed;

                    AccelByteDebug.LogVerbose($"Fetched Server Time {cachedServerTimeData.ServerTime}");
                    OnServerTimeUpdated?.Invoke(cachedServerTimeData);
                }
            }
            else
            {
                error = callbackResult.Error;
            }
            
            if (error != null)
            {
                result.Reject(error);
            }
            else
            {
                result.Resolve(cachedServerTimeData);
            }
            OnFetchServerTimeComplete?.Invoke(error == null);
        }
        
        internal AccelByteResult<AccelByteServerTimeData, Error> FetchServerTime(
            IHttpClient httpClient
            , string @namespace
            , string basicServerUrl)
        {
            Assert.IsNotNull(httpClient, "Assigned http client is null");
            
            var newOperator = new HttpAsyncOperator(httpClient);
            AccelByteResult<AccelByteServerTimeData, Error> retval = FetchServerTime(newOperator, @namespace, basicServerUrl);
            return retval;
        }
        
        internal AccelByteResult<AccelByteServerTimeData, Error> FetchServerTime(
            HttpOperator httpOperator
            , string @namespace
            , string basicServerUrl)
        {
            Assert.IsNotNull(TimelapseSinceSessionStart, "Stopwatch must be initialized on game startup");
            
            var result = new AccelByteResult<AccelByteServerTimeData, Error>();
            
            if (!isFetchingServerTime)
            {
                isFetchingServerTime = true;
                AccelByteMiscellaneousApi.GetCurrentTime(httpOperator, @namespace, basicServerUrl,callbackResult =>
                {
                    OnReceiveGetServerTimeCallback(result, callbackResult);
                });
            }
            else
            {
                result.Reject(new Error(ErrorCode.TooManyRequests, "Another fetching server time request is active"));
            }
            
            return result;
        }
        
        internal void ClearCachedServerTime()
        {
            cachedServerTimeData = null;
        }

        internal void StartGameSession()
        {
            if (timelapseSinceSessionStart == null)
            {
                timelapseSinceSessionStart = new Stopwatch();
                timelapseSinceSessionStart.Start();
            }
        }

        internal void StopGameSession()
        {
            if (timelapseSinceSessionStart != null)
            {
                timelapseSinceSessionStart.Stop();
                timelapseSinceSessionStart = null;
            }
            OnServerTimeUpdated = null;
            OnFetchServerTimeComplete = null;
        }
    }

    public class AccelByteServerTimeData
    {
        public DateTime ServerTime;
        public TimeSpan ServerTimeSpanSinceStartup;
    }
}