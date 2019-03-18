// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access service related to user orders
    /// </summary>
    public class Orders
    {
        private readonly OrdersApi api;
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal Orders(OrdersApi api, User user, AsyncTaskDispatcher taskDispatcher, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can't construct purchase manager! PurchaseService parameter is null!");
            Assert.IsNotNull(user, "Can't construct purchase manager! UserAccount parameter is null!");
            Assert.IsNotNull(taskDispatcher, "taskReactor must not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner must not be null");

            this.api = api;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Create an order to purchase an item
        /// </summary>
        /// <param name="orderRequest">Details about order to be created</param>
        /// <param name="callback">Returns a Result that contains OrderInfo via callback when completed</param>
        public void CreateOrder(OrderRequest orderRequest, ResultCallback<OrderInfo> callback)
        {
            Assert.IsNotNull(orderRequest, "Can't create order; OrderRequest parameter is null!");

            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.CreateOrder(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        orderRequest,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<OrderInfo>) result)),
                    this.user));
        }

        /// <summary>
        /// Get a specific order by orderNo
        /// </summary>
        /// <param name="orderNo">Order number</param>
        /// <param name="callback">Returns a Result that contains OrderInfo via callback when completed</param>
        public void GetUserOrder(string orderNo, ResultCallback<OrderInfo> callback)
        {
            Assert.IsNotNull(orderNo, "Can't get user's order; OrderNo parameter is null!");

            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetUserOrder(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        orderNo,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<OrderInfo>) result)),
                    this.user));
        }

        /// <summary>
        /// Get all orders limited by paging parameters. Returns a list of OrderInfo contained by a page.
        /// </summary>
        /// <param name="startPage">Page number</param>
        /// <param name="size">Size of each page</param>
        /// <param name="callback">Returns a Result that contains PagedOrderInfo via callback when completed</param>
        public void GetUserOrders(uint startPage, uint size, ResultCallback<PagedOrderInfo> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetUserOrders(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        startPage,
                        size,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<PagedOrderInfo>) result)),
                    this.user));
        }

        /// <summary>
        /// Get history of an order specified by orderNo
        /// </summary>
        /// <param name="orderNo">Order number</param>
        /// <param name="callback">Returns a Result that contains OrderHistoryInfo array via callback
        /// when completed.</param>
        public void GetUserOrderHistory(string orderNo, ResultCallback<OrderHistoryInfo[]> callback)
        {
            Assert.IsNotNull(orderNo, "Can't get user's order history info; OrderNo parameter is null!");

            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetUserOrderHistory(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        orderNo,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<OrderHistoryInfo[]>) result)),
                    this.user));
        }
    }
}