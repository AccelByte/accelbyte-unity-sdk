// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;

namespace AccelByte.Core
{
    public abstract class AutoLoginAnalyticsEventScheduler : AnalyticsEventScheduler
    {
        internal Action<ResultCallback<TokenData>> OnAutoLogin;
        protected bool executeValidator;

        protected AutoLoginAnalyticsEventScheduler(IAccelByteAnalyticsWrapper analyticsWrapper) : base(analyticsWrapper)
        {
            executeValidator = false;
        }

        protected async void RunAutoLoginValidator()
        {
            bool currentSessionValid;
            bool? previousSessionValid = null;

            while (keepValidating)
            {
                if (executeValidator)
                {
                    ISession session = analyticsWrapper != null ? analyticsWrapper.GetSession() : null;

                    if (session == null || !session.IsValid())
                    {
                        Result<TokenData> loginResult = null;
                        if (OnAutoLogin != null)
                        {
                            OnAutoLogin.Invoke((callbackResult) =>
                            {
                                loginResult = callbackResult;
                            });

                            while (loginResult == null)
                            {
                                await System.Threading.Tasks.Task.Delay(AccelByteHttpHelper.HttpDelayOneFrameTimeMs);
                            }
                        }
                    }

                    currentSessionValid = session != null ? session.IsValid() : false;
                    if (previousSessionValid == null)
                    {
                        if (currentSessionValid)
                        {
                            maintainer.Start();
                        }
                    }
                    else
                    {
                        if (currentSessionValid != previousSessionValid)
                        {
                            if (currentSessionValid)
                            {
                                if (!maintainer.IsHeartBeatJobRunning)
                                {
                                    maintainer.Start();
                                }
                                else
                                {
                                    maintainer.UnPause();
                                }
                            }
                            else
                            {
                                maintainer.Pause();
                            }
                        }
                    }

                    previousSessionValid = currentSessionValid;
                }

                await System.Threading.Tasks.Task.Delay(AccelByteHttpHelper.HttpDelayOneFrameTimeMs);
            }
        }
    }
}
