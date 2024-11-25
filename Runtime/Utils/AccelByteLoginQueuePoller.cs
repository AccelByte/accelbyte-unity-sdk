// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Threading;
using UnityEngine.Assertions;

namespace AccelByte.Utils
{
    internal class AccelByteLoginQueuePoller
    {
        public Action OnTimeout;

        private Action stopPollTrigger;

        private Action<string, string, AccelByteResult<LoginQueueTicket, Error>> updateTicketTrigger;
        private Action<string, string, AccelByteResult<Error>> cancelTicketTrigger;
        private Action<string, string> claimTicketTrigger;

        private uint timeoutDurationMs;
        private CoreHeartBeat heartbeat;

        private const float minDurationUntilRefresh = 1000f;
        
        public AccelByteLoginQueuePoller(CoreHeartBeat heartbeat, uint timeoutDurationMs, Action<string, string, AccelByteResult<LoginQueueTicket, Error>> updateTicketTrigger, Action<string, string, AccelByteResult<Error>> cancelTicketTrigger, Action<string, string> claimTicketTrigger)
        {
            Assert.IsNotNull(heartbeat);
            Assert.IsNotNull(updateTicketTrigger);
            Assert.IsNotNull(cancelTicketTrigger);
            Assert.IsNotNull(claimTicketTrigger);
            
            this.heartbeat = heartbeat;
            this.timeoutDurationMs = timeoutDurationMs;
            this.updateTicketTrigger = updateTicketTrigger;
            this.cancelTicketTrigger = cancelTicketTrigger;
            this.claimTicketTrigger = claimTicketTrigger;
        }

        public void StartPoll(LoginQueueTicket ticketModel, CancellationToken? ct)
        {
            CancellationTokenSource maintainerCancellationTokenSource = new CancellationTokenSource();
            CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource();
            Action stopMaintainer = () =>
            {
                maintainerCancellationTokenSource.Cancel();
                maintainerCancellationTokenSource.Dispose();
                timeoutCancellationTokenSource.Cancel();
                timeoutCancellationTokenSource.Dispose();
            };

            Action onTimeout = () =>
            {
                stopMaintainer();
                OnTimeout?.Invoke();
            };

            stopPollTrigger = () =>
            {
                stopMaintainer();
            };

            float lastDurationUntilRefresh = 0;
            float durationUntilRefresh = 0;
            string ticketId = null;
            string ticketNamespace = null;
            string userIdentifier = null;
            Action<LoginQueueTicket> updateTicketData = (newTicketModel) =>
            {
                durationUntilRefresh = newTicketModel.PlayerPollingTimeInSeconds * 1000f;
                lastDurationUntilRefresh = durationUntilRefresh;
                ticketNamespace = newTicketModel.Namespace;
                userIdentifier = newTicketModel.Identifier;
                ticketId = newTicketModel.Ticket ?? ticketId;

                if (durationUntilRefresh >= 0 && newTicketModel.Position != 0)
                {
                    durationUntilRefresh = minDurationUntilRefresh;
                }
            };
            updateTicketData(ticketModel);

            heartbeat.Wait(new WaitTimeCommand((double) timeoutDurationMs / 1000, onDone: onTimeout, cancellationToken: timeoutCancellationTokenSource.Token));

            bool onRequestingNewTicket = false;
            heartbeat.Wait(new IndefiniteFixedUpdateLoopCommand(cancellationToken: maintainerCancellationTokenSource.Token, onUpdate:
                dt =>
                {

                    if (onRequestingNewTicket)
                    {
                        return;
                    }
                    
                    if (ct != null && ct.Value.IsCancellationRequested)
                    {
                        cancelTicketTrigger?.Invoke(ticketId, ticketNamespace, null);
                        stopMaintainer();
                        return;
                    }

                    float elapsedTime = dt * 1000f;
                    durationUntilRefresh -= elapsedTime;

                    if (durationUntilRefresh <= 0)
                    {
                        onRequestingNewTicket = true;
                        var onUpdateTicketResult =
                            new AccelByteResult<LoginQueueTicket, Error>();
                        onUpdateTicketResult.OnSuccess((newTicketModel) =>
                        {
                            if (newTicketModel != null)
                            {
                                updateTicketData(newTicketModel);
                                if (newTicketModel.Position == 0)
                                {
                                    stopMaintainer();
                                    claimTicketTrigger?.Invoke(ticketId, userIdentifier);
                                }
                                else
                                {
                                    onRequestingNewTicket = false;
                                }
                            }
                            else
                            {
                                durationUntilRefresh = lastDurationUntilRefresh;
                                onRequestingNewTicket = true;
                            }
                        });
                        onUpdateTicketResult.OnFailed(error =>
                        {
                            durationUntilRefresh = lastDurationUntilRefresh;
                            onRequestingNewTicket = false;
                        });
                        updateTicketTrigger?.Invoke(ticketId, ticketNamespace, onUpdateTicketResult);
                    }
                }));
        }

        public void StopPoll()
        {
            cancelTicketTrigger?.Invoke(null, null, null);
            stopPollTrigger?.Invoke();
        }

    }
}