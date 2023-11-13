// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
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

        private enum PredefinedApiCallbackReturnType
        {
            OrderInfo,
            OrderPagingSlicedResult
        }

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

        private List<PredefinedPaymentModel> RefinePaymentApiCallback<T>(Result<T> apiCallResult, PredefinedApiCallbackReturnType returnType)
        {
            List<PredefinedPaymentModel> paymentModels = new List<PredefinedPaymentModel>();

            switch (returnType)
            {
                case PredefinedApiCallbackReturnType.OrderInfo:
                    {
                        var orderInfo = apiCallResult as Result<OrderInfo>;
                        paymentModels.Add(new PredefinedPaymentModel(
                            orderInfo.Value.orderNo,
                            orderInfo.Value.paymentOrderNo,
                            orderInfo.Value.userId,
                            orderInfo.Value.itemId,
                            orderInfo.Value.price,
                            orderInfo.Value.status.ToString()
                            ));
                        break;

                    }
                case PredefinedApiCallbackReturnType.OrderPagingSlicedResult:
                    {
                        var pagingSlicicngResult = apiCallResult as Result<OrderPagingSlicedResult>;
                        foreach (var orderInfo in pagingSlicicngResult.Value.data)
                        {
                            paymentModels.Add(
                                new PredefinedPaymentModel(
                                    orderInfo.orderNo,
                                    orderInfo.paymentOrderNo,
                                    orderInfo.userId,
                                    orderInfo.itemId,
                                    orderInfo.price,
                                    orderInfo.status.ToString()
                               ));
                        }
                    }
                    break;
            }

            return paymentModels;
        }

        private void RefinePaymentApiCallback(Result<OrderInfo> apiCallResult)
        {
            List<PredefinedPaymentModel> paymentModels;
            paymentModels = RefinePaymentApiCallback<OrderInfo>(apiCallResult, PredefinedApiCallbackReturnType.OrderInfo);
            SendPredefinedEventPayload(paymentModels);
        }

        private void RefineSlicingPaymentApiCallback(Result<OrderPagingSlicedResult> apiCallResult)
        {
            List<PredefinedPaymentModel> paymentModels;
            paymentModels = RefinePaymentApiCallback<OrderPagingSlicedResult>(apiCallResult, PredefinedApiCallbackReturnType.OrderPagingSlicedResult);
            SendPredefinedEventPayload(paymentModels);
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            RefinePaymentApiCallback(cb);
                        }
                        else if (cb.IsError)
                        {
                            RefineFailedApiCallback(session.UserId, orderRequest.itemId, orderRequest.price);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserOrder(session.UserId, session.AuthorizationToken, orderNo, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        RefinePaymentApiCallback(cb);
                    }
                    else if (cb.IsError)
                    {
                        RefineFailedApiCallback(session.UserId);
                    }
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Get all orders limited by paging parameters. Returns a list of OrderInfo contained by a page.
        /// This Function is obsolete, please use QueryUserOrders(UserOrderRequest userOrderRequest, ResultCallback<OrderPagingSlicedResult> callback) instead. 
        /// </summary>
        /// <param name="startPage">Page number</param>
        /// <param name="size">Size of each page</param>
        /// <param name="callback">Returns a Result that contains OrderPagingSlicedResult via callback when completed</param>
        [Obsolete]
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            RefineSlicingPaymentApiCallback(cb);
                        }
                        else if (cb.IsError)
                        {
                            RefineFailedApiCallback(session.UserId);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Get history of an order specified by orderNo
        /// </summary>
        /// <param name="orderNo">Order number</param>
        /// <param name="callback">Returns a Result that contains OrderHistoryInfo array via callback when completed.</param>
        public void GetUserOrderHistory( string orderNo
            , ResultCallback<OrderHistoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

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

        /// <summary>
        ///  Get all of user's orders that have been created with paging.
        /// </summary>
        /// <param name="UserOrderRequest">Contains some parameters for query</param>
        /// <param name="callback">Returns a Result that contains OrderPagingSlicedResult via callback when completed.</param>
        public void QueryUserOrders(UserOrdersRequest userOrderRequest
            , ResultCallback<OrderPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryUserOrders(
                    session.UserId,
                    session.AuthorizationToken,
                    userOrderRequest,
                    callback));
        }

        #region PredefinedEvents

        private PredefinedEventScheduler predefinedEventScheduler;

        /// <summary>
        /// Set predefined event scheduler to the wrapper
        /// </summary>
        /// <param name="predefinedEventScheduler">Predefined event scheduler object reference</param>
        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventScheduler)
        {
            this.predefinedEventScheduler = predefinedEventScheduler;
        }

        private void SendPredefinedEventPayload(List<PredefinedPaymentModel> model)
        {
            if (model != null)
            {
                List<PredefinedPaymentModel> successData = new List<PredefinedPaymentModel>();
                List<PredefinedPaymentModel> failedData = new List<PredefinedPaymentModel>();

                foreach (var paymentModel in model)
                {
                    var success = Enum.TryParse(paymentModel.Status, false, out OrderStatus result);
                    if (success)
                    {
                        switch (result)
                        {
                            case OrderStatus.CHARGED:
                            case OrderStatus.FULFILLED:
                            case OrderStatus.CHARGEBACK_REVERSED:
                            case OrderStatus.FULFILL_FAILED:
                                successData.Add(paymentModel);
                                break;
                            case OrderStatus.CHARGEBACK:
                            case OrderStatus.REFUNDING:
                            case OrderStatus.REFUNDED:
                            case OrderStatus.REFUND_FAILED:
                                failedData.Add(paymentModel);
                                break;
                            default:
                                break;
                        }
                    }
                }

                IAccelByteTelemetryPayload payload;

                if (successData.Count > 0)
                {
                    payload = new PredefinedPaymentSucceededPayload(successData);
                    SendPredefinedEvent(payload);
                }

                if (failedData.Count > 0)
                {
                    payload = new PredefinedPaymentFailedPayload(failedData);
                    SendPredefinedEvent(payload);
                }
            }
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            if (payload != null && predefinedEventScheduler != null)
            {
                var userProfileEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(userProfileEvent, null);
            }
        }

        private void RefineFailedApiCallback(string userId, string itemId = "", int price = 0)
        {
            List<PredefinedPaymentModel> failedData = new List<PredefinedPaymentModel>
            {
                new PredefinedPaymentModel(
                "",
                "",
                userId,
                itemId,
                price,
                "")
            };

            var payload = new PredefinedPaymentFailedPayload(failedData);
            SendPredefinedEvent(payload);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }
            callback.Try(result);
        }
        #endregion
    }
}