// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Items service
    /// </summary>
    public class Items
    {
        private readonly ItemsApi api;
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal Items(ItemsApi api, User user, AsyncTaskDispatcher taskDispatcher, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can't construct Catalog manager; CatalogService parameter is null!");
            Assert.IsNotNull(user, "Can't construct Catalog manager; UserAccount parameter isnull!");
            Assert.IsNotNull(taskDispatcher, "taskReactor must not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner must not be null");

            this.api = api;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get Item by itemId
        /// </summary>
        /// <param name="itemId">Item ID to get item with</param>
        /// <param name="region">Region of the item</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains Item via callback when completed.</param>
        public void GetItemById(string itemId, string region, string language, ResultCallback<Item> callback)
        {
            Assert.IsNotNull(itemId, "Can't get item; ItemId parameter is null!");
            Assert.IsNotNull(region, "Can't get item; Region parameter is null!");
            Assert.IsNotNull(language, "Can't get item; Language parameter is null!");

            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetItem(
                        this.user.Namespace,
                        this.user.AccessToken,
                        itemId,
                        region,
                        language,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<Item>) result)),
                    this.user));
        }

        /// <summary>
        /// Get Items by criteria. Set ItemCriteria fields as null if you don't want to specify the criteria. The result
        /// callback will returns a PagedItems that contains Items within it.
        /// </summary>
        /// <param name="criteria">Criteria to search items</param>
        /// <param name="region">Region of items</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains PagedItems via callback when completed.</param>
        public void GetItemsByCriteria(ItemCriteria criteria, string region, string language,
            ResultCallback<PagedItems> callback)
        {
            Assert.IsNotNull(criteria, "Can't get items by criteria; Criteria parameter is null!");
            Assert.IsNotNull(language, "Can't get items by criteria; Language parameter is null!");

            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetItemsByCriteria(
                        this.user.Namespace,
                        this.user.AccessToken,
                        region,
                        language,
                        criteria,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<PagedItems>) result)),
                    this.user));
        }
    }
}