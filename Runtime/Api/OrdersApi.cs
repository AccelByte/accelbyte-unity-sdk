// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class OrdersApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal OrdersApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator CreateOrder( string userId
            , string userAccessToken
            , OrderRequest orderRequest
            , ResultCallback<OrderInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty")); 
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userAccessToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be null or empty"));
                yield break;
            }
            if (orderRequest == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(orderRequest) + " cannot be null"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(orderRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OrderInfo>();
            callback.Try(result);
        }

        public IEnumerator CancelOrderApi( string orderNo
            , string userId
            , string userAccessToken
            , ResultCallback<OrderInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userAccessToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(orderNo))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(orderNo) + " cannot be null or empty"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders/{orderNo}/cancel")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("orderNo", orderNo)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OrderInfo>();

            callback.Try(result);
        }

        public IEnumerator GetUserOrder( string userId
            , string userAccessToken
            , string orderNumber
            , ResultCallback<OrderInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userAccessToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(orderNumber))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(orderNumber) + " cannot be null or empty"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders/{orderNo}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("orderNo", orderNumber)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OrderInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserOrders( string userId
            , string userAccessToken
            , uint page
            , uint size
            , ResultCallback<OrderPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userAccessToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be null or empty"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithQueryParam("page", page.ToString())
                .WithQueryParam("size", size.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<OrderPagingSlicedResult>();
            callback.Try(result);
        }

        public IEnumerator QueryUserOrders(string userId
            , string userAccessToken
            , UserOrdersRequest userOrderRequest
            , ResultCallback<OrderPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userAccessToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be null or empty"));
                yield break;
            }
            if (userOrderRequest == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userOrderRequest) + " cannot be null"));
                yield break;
            }
            if (userOrderRequest.Status == OrderStatus.NONE)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be OrderStatus.NONE"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithQueryParam("itemId", userOrderRequest.ItemId)
                .WithQueryParam("status", userOrderRequest.Status.ToString())
                .WithQueryParam("offset", userOrderRequest.Offset.ToString())
                .WithQueryParam("limit", userOrderRequest.Limit.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult(); 
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OrderPagingSlicedResult>();
            callback.Try(result);
        }

        public IEnumerator GetUserOrderHistory( string userId
            , string userAccessToken
            , string orderNo
            , ResultCallback<OrderHistoryInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userAccessToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(userAccessToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(orderNo))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, nameof(orderNo) + " cannot be null or empty"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders/{orderNo}/history")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("orderNo", orderNo)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OrderHistoryInfo[]>();
            callback.Try(result);
        }
    }
}