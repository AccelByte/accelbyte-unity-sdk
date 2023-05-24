﻿// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using System.Collections;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of StoreDisplay owned by the user
    /// </summary>
    public class StoreDisplay : WrapperBase
    {
        private readonly StoreDisplayApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal StoreDisplay(StoreDisplayApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Get all views.
        /// </summary>
        /// <param name="storeId">storeId</param>
        /// <param name="language">language</param>
        /// <param name="callback">
        /// Returns a Result that contains ViewInfo via callback when completed
        /// </param> 
        public void GetAllViews(string storeId
            , string language
            , ResultCallback<ViewInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(storeId, "Can't List Active Section Contents! storeId parameter is null!");
            Assert.IsNotNull(language, "Can't List Active Section Contents! language parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetAllViews(session.UserId, storeId, language, callback));
        }


        /// <summary>
        /// List active section contents.
        /// </summary>
        /// <param name="storeId">storeId</param>
        /// <param name="viewId">viewId</param>
        /// <param name="region">region</param>
        /// <param name="language">language</param>
        /// <param name="callback">
        /// Returns a Result that contains SectionInfo via callback when completed
        /// </param> 
        public void ListActiveSectionContents(string storeId
            , string viewId
            , string region
            , string language
            , ResultCallback<SectionInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(storeId, "Can't List Active Section Contents! storeId parameter is null!");
            Assert.IsNotNull(viewId, "Can't List Active Section Contents! viewId parameter is null!");
            Assert.IsNotNull(region, "Can't List Active Section Contents! region parameter is null!");
            Assert.IsNotNull(language, "Can't List Active Section Contents! language parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ListActiveSectionContents(
                    session.UserId, storeId, viewId, region, language, callback));
        }
    }


}
