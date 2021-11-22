// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Category service.
    /// </summary>
    public class Categories
    {
        private readonly CategoriesApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;
        private readonly string @namespace;

        internal Categories(CategoriesApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get category info by its exact category path.
        /// </summary>
        /// <param name="categoryPath">Category path this category identified by</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo via callback when completed</param>
        public void GetCategory(string categoryPath, string language, ResultCallback<CategoryInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(categoryPath, "Can't get category; CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get category; Language parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetCategory(this.@namespace, this.session.AuthorizationToken, categoryPath, language, callback));
        }

        /// <summary>
        /// Get all categories in root path
        /// </summary>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo array via callback when completed</param>
        public void GetRootCategories(string language, ResultCallback<CategoryInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(language, "Can't get root categories; Language parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetRootCategories(this.@namespace, this.session.AuthorizationToken, language, callback));
        }

        /// <summary>
        /// Get child categories under category path
        /// </summary>
        /// <param name="categoryPath">Parent category path</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo array via callback when completed</param>
        public void GetChildCategories(string categoryPath, string language, ResultCallback<CategoryInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(categoryPath, "Can't get child categories; CategoryPath parameter is null!");
            Assert.IsNotNull(language, "Can't get child categories; Language parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetChildCategories(this.@namespace, this.session.AuthorizationToken, categoryPath, language, callback));
        }

        /// <summary>
        /// Get all descendants of the category identified by category path. Descendant categories will also include
        /// grand children categories in flat CategoryInfo array
        /// </summary>
        /// <param name="categoryPath">Parent category path</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains CategoryInfo array via callback when completed</param>
        public void GetDescendantCategories(string categoryPath, string language, ResultCallback<CategoryInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(categoryPath, "Can't get descendant categories; Language parameter is null!");
            Assert.IsNotNull(language, "Can't get descendant categories; Language parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetDescendantCategories(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    categoryPath,
                    language,
                    callback));
        }
    }
}