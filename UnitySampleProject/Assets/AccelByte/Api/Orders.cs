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
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Orders(OrdersApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
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
        /// Create an order to purchase an item
        /// </summary>
        /// <param name="orderRequest">Details about order to be created</param>
        /// <param name="callback">Returns a Result that contains OrderInfo via callback when completed</param>
        public void CreateOrder(OrderRequest orderRequest, ResultCallback<OrderInfo> callback)
        {
            Assert.IsNotNull(orderRequest, "Can't create order; OrderRequest parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateOrder(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    orderRequest,
                    callback));
        }

        /// <summary>
        /// Get a specific order by orderNo
        /// </summary>
        /// <param name="orderNo">Order number</param>
        /// <param name="callback">Returns a Result that contains OrderInfo via callback when completed</param>
        public void GetUserOrder(string orderNo, ResultCallback<OrderInfo> callback)
        {
            Assert.IsNotNull(orderNo, "Can't get user's order; OrderNo parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserOrder(this.@namespace, this.session.UserId, this.session.AuthorizationToken, orderNo, callback));
        }

        /// <summary>
        /// Get all orders limited by paging parameters. Returns a list of OrderInfo contained by a page.
        /// </summary>
        /// <param name="startPage">Page number</param>
        /// <param name="size">Size of each page</param>
        /// <param name="callback">Returns a Result that contains PagedOrderInfo via callback when completed</param>
        public void GetUserOrders(uint startPage, uint size, ResultCallback<PagedOrderInfo> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserOrders(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    startPage,
                    size,
                    callback));
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

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserOrderHistory(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    orderNo,
                    callback));
        }
    }
}