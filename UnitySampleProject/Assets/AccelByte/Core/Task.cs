// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using AccelByte.Api;

namespace AccelByte.Core
{
    public static class Task
    {
        public static ITask Await(HttpWebRequest request, Action<HttpWebResponse> callback)
        {
            return new HttpTask(request, callback);
        }

        public static ITask Await(IEnumerator<ITask> tasks) { return new SequenceTask(tasks); }

        public static ITask Delay(uint milliseconds) { return new DelayTask(milliseconds); }

        public static IEnumerator<ITask> Retry(Func<Action<IResult>, IEnumerator<ITask>> tasks,
            Action<IResult> callback, User user, int initialDelay = 1000, int maxDelay = 30000,
            int totalTimeout = 60000)
        {
            Random rand = new Random();
            IResult result = null;
            int nextDelay = initialDelay;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                yield return Task.Await(tasks(res => result = res));

                if (!result.IsError)
                {
                    callback(result);

                    yield break;
                }

                switch (result.Error.Code)
                {
                case ErrorCode.Unauthorized:

                    if (user != null)
                    {
                        yield return Task.Await(user.ForceRefresh());
                    }
                    else
                    {
                        callback(result);

                        yield break;
                    }

                    break;
                case ErrorCode.InternalServerError:
                case ErrorCode.NotImplemented:
                case ErrorCode.BadGateway:
                case ErrorCode.ServiceUnavailable:
                case ErrorCode.GatewayTimeout:

                    yield return Task.Delay((uint) (nextDelay + rand.Next(-nextDelay / 4, nextDelay / 4)));

                    nextDelay *= 2;

                    if (nextDelay > maxDelay)
                    {
                        nextDelay = maxDelay;
                    }

                    break;

                default:
                    callback(result);

                    yield break;
                }
            } while (stopwatch.Elapsed < TimeSpan.FromSeconds(totalTimeout));

            callback(result);
        }
    }
}