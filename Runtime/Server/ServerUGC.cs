// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
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
        private readonly ServerUGCApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServerUGC( ServerUGCApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct UGC manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct UGC manager; inCoroutineRunner is null!");

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
                api.SearchContent(searchContentRequest, callback, userId));
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
                api.SearchContentsSpesificToChannel(channelId, searchContentRequest, callback, userId));
        }
    }
}