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
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Items(ItemsApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
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

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetItem(this.@namespace, this.session.AuthorizationToken, itemId, region, language, callback));
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

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetItemsByCriteria(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    region,
                    language,
                    criteria,
                    callback));
        }
    }
}