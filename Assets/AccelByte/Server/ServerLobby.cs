// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    /// <summary>
    /// Lobby utilities.
    /// </summary>
    public class ServerLobby
    {
        private readonly ServerLobbyApi api;
        private readonly IServerSession session;
        private readonly string namespace_;
        private readonly CoroutineRunner coroutineRunner;
        
        internal ServerLobby(ServerLobbyApi api, IServerSession session, string namespace_, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsNotNull(namespace_, "namespace parameter can not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }
        
        /// <summary>
        /// Write party storage data to the targeted party ID.
        /// Beware:
        /// Object will not be write immediately, please take care of the original object until it written.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        /// <param name="payloadModifier">Function to modify the latest party data with your customized modifier.</param>
        /// <param name="retryAttempt">the number of retry to do when there is an error in writing to party storage (likely due to write conflicts)</param>
        public void WritePartyStorage(string partyId, ResultCallback<PartyDataUpdateNotif> callback, Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier, int retryAttempt = 1)
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId), "Party ID should not be null.");

            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            WritePartyStorageRecursive(retryAttempt, partyId, callback, payloadModifier);
        }
        
        private void WritePartyStorageRecursive(int remainingAttempt, string partyId, ResultCallback<PartyDataUpdateNotif> callback, Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier)
        {
            if (remainingAttempt <= 0)
            {
                callback.TryError(new Error(ErrorCode.PreconditionFailed, "Exhaust all retry attempt to modify party storage. Please try again."));
                return;
            }
            
            GetPartyStorage(partyId, getPartyStorageResult =>
            {
                if (getPartyStorageResult.IsError)
                {
                    callback.TryError(getPartyStorageResult.Error);
                }
                else
                {
                    getPartyStorageResult.Value.custom_attribute = payloadModifier(getPartyStorageResult.Value.custom_attribute);
                    
                    var updateRequest = new PartyDataUpdateRequest();
                    updateRequest.custom_attribute = getPartyStorageResult.Value.custom_attribute;
                    updateRequest.updatedAt = getPartyStorageResult.Value.updatedAt;
                    
                    this.coroutineRunner.Run(
                        this.api.WritePartyStorage(
                            this.namespace_,
                            this.session.AuthorizationToken,
                            updateRequest,
                            partyId,
                            callback,
                            () =>
                            {
                                WritePartyStorageRecursive(remainingAttempt - 1, partyId, callback, payloadModifier);
                            }));
                }
            });
        }
        
        /// <summary>
        /// Get party storage by party ID.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void GetPartyStorage(string partyId, ResultCallback<PartyDataUpdateNotif> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId), "Party ID should not be null.");
            
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            this.coroutineRunner.Run(
                this.api.GetPartyStorage(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    partyId,
                    callback));
        }

        /// <summary>
        /// Get active parties.
        /// </summary>
        /// <param name="limit">The amount of returned item per page. Set is to 0 to retrieve by maximum limit.</param>
        /// <param name="offset">Offset of the item (active party). First item is = 0.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void GetActiveParties(int limit, int offset, ResultCallback<ActivePartiesData> callback)
        {
            Assert.IsFalse(limit <= 0, "Limit be greater than zero.");
            Assert.IsFalse(offset < 0, "Offset should not negative.");

            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            this.coroutineRunner.Run(
                this.api.GetActiveParties(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    limit,
                    offset,
                    callback));
        }

        /// <summary>
        /// Get active parties based on the pagination result.
        /// </summary>
        /// <param name="paging">The processed paging response.</param>
        /// <param name="paginationType">Which page that will be opened.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void GetActiveParties(Paging paging, PaginationType paginationType, ResultCallback<ActivePartiesData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            string pagingUrl = "";
            switch (paginationType)
            {
                case PaginationType.FIRST:
                    pagingUrl = paging.first;
                    break;
                case PaginationType.NEXT:
                    pagingUrl = paging.next;
                    break;
                case PaginationType.PREVIOUS:
                    pagingUrl = paging.previous;
                    break;
            }

            var OnInvalidPagination = Result<ActivePartiesData>.CreateError(ErrorCode.NotFound, "Provided pagination type is empty.");
            var questionMarkIndex = pagingUrl.LastIndexOf("?");
            if (string.IsNullOrEmpty(pagingUrl) || questionMarkIndex == -1)
            {
                callback.Invoke(OnInvalidPagination);
                return;
            }

            pagingUrl = pagingUrl.Substring(questionMarkIndex + 1, pagingUrl.Length - (questionMarkIndex + 1));
            var queryParams = pagingUrl.Split('&');
            
            int limit = 0;
            int offset = 0;
            foreach (var param_ in queryParams)
            {
                int limitIndex = param_.IndexOf("limit=");
                int offsetIndex = param_.IndexOf("offset=");
                if (limitIndex > -1)
                {
                    var value = param_.Substring(limitIndex + 6, param_.Length - (limitIndex + 6));
                    limit = Convert.ToInt32(value);
                }else if (offsetIndex > -1)
                {
                    var value = param_.Substring(offsetIndex + 7, param_.Length - (offsetIndex + 7));
                    offset = Convert.ToInt32(value);
                }
            }

            this.coroutineRunner.Run(
                this.api.GetActiveParties(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    limit,
                    offset,
                    callback));
        }

        /// <summary>
        /// Get active party info by user Id
        /// </summary>
        /// <param name="userId">The user ID to be searched</param>
        /// <param name="callback">Returns PartyDataUpdateNotif via callback when party is found</param>
        public void GetPartyDataByUserId(string userId, ResultCallback<PartyDataUpdateNotif> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            Assert.IsFalse(string.IsNullOrEmpty(userId), "Parameter userId cannot be null or empty string");

            if(!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.api.GetPartyDataByUserId(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    userId,
                    callback));
        }
    }
}