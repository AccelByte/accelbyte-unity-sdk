// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access service related to user orders
    /// </summary>
    public class Orders : WrapperBase
    {
        private readonly OrdersApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Orders( OrdersApi inApi
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
        internal Orders(OrdersApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Create an order to purchase an item
        /// </summary>
        /// <param name="orderRequest">Details about order to be created</param>
        /// <param name="callback">Returns a Result that contains OrderInfo via callback when completed</param>
        public void CreateOrder( OrderRequest orderRequest
            , ResultCallback<OrderInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(orderRequest, "Can't create order; OrderRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateOrder(
                    session.UserId,
                    session.AuthorizationToken,
                    orderRequest,
                    callback));
        }

        /// <summary>
        /// Cancel the Order after Create the Order
        /// </summary>
        /// <param name="orderNo">need orderNo parameter to cancel the payment</param>
        /// <param name="callback">callback delegate that will send the OrderInfo models parameter value</param>
        public void CancelOrder( string orderNo
            , ResultCallback<OrderInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(orderNo, "Can't cancel the order. orderNo parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CancelOrderApi(
                    orderNo,
                    session.UserId,
                    session.AuthorizationToken,
                    callback));
        }

        /// <summary>
        /// Get a specific order by orderNo
        /// </summary>
        /// <param name="orderNo">Order number</param>
        /// <param name="callback">Returns a Result that contains OrderInfo via callback when completed</param>
        public void GetUserOrder( string orderNo
            , ResultCallback<OrderInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(orderNo, "Can't get user's order; OrderNo parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserOrder(session.UserId, session.AuthorizationToken, orderNo, callback));
        }

        /// <summary>
        /// Get all orders limited by paging parameters. Returns a list of OrderInfo contained by a page.
        /// </summary>
        /// <param name="startPage">Page number</param>
        /// <param name="size">Size of each page</param>
        /// <param name="callback">Returns a Result that contains OrderPagingSlicedResult via callback when completed</param>
        public void GetUserOrders( uint startPage
            , uint size
            , ResultCallback<OrderPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserOrders(
                    session.UserId,
                    session.AuthorizationToken,
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
        public void GetUserOrderHistory( string orderNo
            , ResultCallback<OrderHistoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(orderNo, "Can't get user's order history info; OrderNo parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserOrderHistory(
                    session.UserId,
                    session.AuthorizationToken,
                    orderNo,
                    callback));
        }
    }
}