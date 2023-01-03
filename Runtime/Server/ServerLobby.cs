// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    /// <summary>
    /// Lobby utilities.
    /// </summary>
    public class ServerLobby : WrapperBase
    {
        private readonly ServerLobbyApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        
        internal ServerLobby( ServerLobbyApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
        internal ServerLobby( ServerLobbyApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
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
        public void WritePartyStorage( string partyId
            , ResultCallback<PartyDataUpdateNotif> callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier
            , int retryAttempt = 1 )
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId), "Party ID should not be null.");

            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            WritePartyStorageRecursive(retryAttempt, partyId, callback, payloadModifier);
        }
        
        private void WritePartyStorageRecursive( int remainingAttempt
            , string partyId
            , ResultCallback<PartyDataUpdateNotif> callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
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
                    
                    coroutineRunner.Run(
                        api.WritePartyStorage(
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
        public void GetPartyStorage( string partyId
            , ResultCallback<PartyDataUpdateNotif> callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId), "Party ID should not be null.");
            
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            coroutineRunner.Run(
                api.GetPartyStorage(partyId, callback));
        }

        /// <summary>
        /// Get active party info by user Id
        /// </summary>
        /// <param name="userId">The user ID to be searched</param>
        /// <param name="callback">Returns PartyDataUpdateNotif via callback when party is found</param>
        public void GetPartyDataByUserId( string userId
            , ResultCallback<PartyDataUpdateNotif> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            Assert.IsFalse(string.IsNullOrEmpty(userId), "Parameter userId cannot be null or empty string");

            if(!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetPartyDataByUserId(userId, callback));
        }

        /// <summary>
        /// Get a user's session attribute by key
        /// </summary>
        /// <param name="userId">the user ID to be searched</param>
        /// <param name="key">The user's session attribute key</param>
        /// <param name="callback">Returns ServerGetSessionAttributeResponse via callback when found"</param>
        public void GetSessionAttribute( string userId
            , string key
            , ResultCallback<ServerGetSessionAttributeResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if(!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetSessionAttribute(userId, key, callback));
        }

        /// <summary>
        /// Get all of user's session attribute
        /// </summary>
        /// <param name="userId">the user ID to be searched</param>
        /// <param name="callback">Returns GetSessionAttributeAllResponse via callback when found"</param>
        public void GetSessionAttributeAll( string userId
            , ResultCallback<GetSessionAttributeAllResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetSessionAttributeAll(userId, callback));
        }

        /// <summary>
        /// set a user's session attributes
        /// </summary>
        /// <param name="userId">user Id to be set</param>
        /// <param name="attributes">the attributes dictionary to set</param>
        /// <param name="callback">Returns result via callback when operation is done</param>
        public void SetSessionAttribute( string userId
            , Dictionary<string, string> attributes
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SetSessionAttribute(userId, attributes, callback));
        }

        /// <summary>
        /// Set a user's session attribute
        /// </summary>
        /// <param name="userId">user Id to be set</param>
        /// <param name="key">the attribute's key</param>
        /// <param name="value">the attribute's value</param>
        /// <param name="callback">Returns result via callback when operation is done</param>
        public void SetSessionAttribute( string userId
            , string key
            , string value
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(key), "key parameter cannot be null or empty");

            SetSessionAttribute(userId, new Dictionary<string, string>() { { key, value } }, callback);
        }
    }
}