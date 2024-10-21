// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
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
        bool? previousSessionValid = null;
        private bool isInLogin;

        protected AutoLoginAnalyticsEventScheduler(IAccelByteAnalyticsWrapper analyticsWrapper) : base(analyticsWrapper)
        {
            executeValidator = false;
        }

        protected void RunAutoLoginValidator()
        {
            if (!validatorCts.IsCancellationRequested && SharedMemory != null && SharedMemory?.CoreHeartBeat != null)
            {
                previousSessionValid = null;
                
                Action onUpdate = null;
                onUpdate += () =>
                {
                    if (executeValidator)
                    {
                        ISession session = analyticsWrapper != null ? analyticsWrapper.GetSession() : null;

                        if (session == null || !session.IsValid())
                        {
                            if (OnAutoLogin != null)
                            {
                                isInLogin = true;
                                OnAutoLogin.Invoke((callbackResult) =>
                                {
                                    isInLogin = false;
                                });
                            }
                        }
                    }
                };
                
                onUpdate += () =>
                {
                    if (executeValidator)
                    {
                        if (isInLogin)
                        {
                            return;
                        }
                        
                        ISession session = analyticsWrapper != null ? analyticsWrapper.GetSession() : null;
                        bool currentSessionValid = session != null ? session.IsValid() : false;
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
                };
                
                onUpdate?.Invoke();
                SharedMemory?.CoreHeartBeat.Wait(new IndefiniteLoopCommand(cancellationToken: validatorCts.Token, onUpdate: onUpdate));
            }
        }
    }
}
