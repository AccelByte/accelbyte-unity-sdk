// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerUGC : WrapperBase
    {
        internal readonly ServerUGCApi Api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServerUGC( ServerUGCApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct UGC manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct UGC manager; inCoroutineRunner is null!");

            Api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal ServerUGC( ServerUGCApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner)
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Search Content player's channel based on the its channel id.
        /// </summary>
        /// <param name="searchContentRequest ">Detail information for the search content request..</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void SearchContent(SearchContentRequest searchContentRequest
            , ResultCallback<UGCSearchContentsPagingResponse> callback
            , string userId = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            coroutineRunner.Run(
                Api.SearchContent(searchContentRequest, callback, userId));
        }

        /// <summary>
        /// Search Content player's channel based on the its channel id.
        /// </summary>
        /// <param name="channelId ">The id of the content's channel.</param>
        /// <param name="searchContentRequest ">Detail information for the content request..</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void SearchContentsSpesificToChannel(string channelId
            , SearchContentRequest searchContentRequest
            , ResultCallback<UGCSearchContentsPagingResponse> callback
            , string userId = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            coroutineRunner.Run(
                Api.SearchContentsSpesificToChannel(channelId, searchContentRequest, callback, userId));
        }
        
        /// <summary>
        /// Modify existing content to update some information by share code.
        /// </summary>
        /// <param name="userId">The user id who modify the Content.</param>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="modifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is UGCResponse Model.
        /// </param>
        public void ModifyContentByShareCode(string userId
            , string channelId
            , string shareCode
            , UGCUpdateRequest modifyRequest
            , ResultCallback<UGCResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.ModifyContentByShareCode(userId, channelId, shareCode, modifyRequest, callback));
        }
        
        /// <summary>
        /// Delete a content based on its channel id and share code.
        /// </summary>
        /// <param name="userId">The user id who delete the Content.</param>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteContentByShareCode(string userId
            , string channelId
            , string shareCode
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.DeleteContentByShareCode(userId, channelId, shareCode, callback));
        }
    }
}