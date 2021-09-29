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

        /// <summary>
        /// Get a user's session attribute by key
        /// </summary>
        /// <param name="userId">the user ID to be searched</param>
        /// <param name="key">The user's session attribute key</param>
        /// <param name="callback">Returns ServerGetSessionAttributeResponse via callback when found"</param>
        public void GetSessionAttribute(string userId, string key, ResultCallback<ServerGetSessionAttributeResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if(!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.api.GetSessionAttribute(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    userId,
                    key,
                    callback));
        }

        /// <summary>
        /// Get all of user's session attribute
        /// </summary>
        /// <param name="userId">the user ID to be searched</param>
        /// <param name="callback">Returns GetSessionAttributeAllResponse via callback when found"</param>
        public void GetSessionAttributeAll(string userId, ResultCallback<GetSessionAttributeAllResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.api.GetSessionAttributeAll(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    userId,
                    callback));
        }

        public void SetSessionAttribute(string userId, Dictionary<string, string> attributes, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.api.SetSessionAttribute(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    userId,
                    attributes,
                    callback));
        }

        public void SetSessionAttribute(string userId, string key, string value, ResultCallback callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(key), "key parameter cannot be null or empty");

            SetSessionAttribute(userId, new Dictionary<string, string>() { { key, value } }, callback);
        }
    }
}