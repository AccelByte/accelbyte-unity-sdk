// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Category service.
    /// </summary>
    public class Categories : WrapperBase
    {
        private readonly CategoriesApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly UserSession session;

        [UnityEngine.Scripting.Preserve]
        internal Categories( CategoriesApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

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
        internal Categories( CategoriesApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get category info by its exact category path.
        /// </summary>
        /// <param name="categoryPath">Category path this category identified by</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo via callback when completed</param>
        public void GetCategory( string categoryPath
            , string language
            , ResultCallback<CategoryInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(categoryPath, language);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetCategory(categoryPath, language, callback));
        }

        /// <summary>
        /// Get all categories in root path
        /// </summary>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo array via callback when completed</param>
        public void GetRootCategories( string language
            , ResultCallback<CategoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(language);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetRootCategories(language, callback));
        }

        /// <summary>
        /// Get child categories under category path
        /// </summary>
        /// <param name="categoryPath">Parent category path</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo array via callback when completed</param>
        public void GetChildCategories( string categoryPath
            , string language
            , ResultCallback<CategoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(categoryPath, language);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetChildCategories(categoryPath, language, callback));
        }

        /// <summary>
        /// Get all descendants of the category identified by category path. Descendant categories
        /// will also include grand children categories in flat CategoryInfo array.
        /// </summary>
        /// <param name="categoryPath">Parent category path</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">
        /// Returns a Result that contains CategoryInfo array via callback when completed
        /// </param>
        public void GetDescendantCategories( string categoryPath
            , string language
            , ResultCallback<CategoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(categoryPath, language);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }
            
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetDescendantCategories(
                    categoryPath,
                    language,
                    callback));
        }
    }
}