// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

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

            var error = ApiHelperUtils.CheckForNullOrEmpty(userId
                , userOrderRequest
                , userAccessToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);
            
            if (userOrderRequest.ItemId != null)
            {
                builder.WithQueryParam("itemId", userOrderRequest.ItemId);
            }

            if (userOrderRequest.Status != OrderStatus.NONE)
            {
                builder.WithQueryParam("status", userOrderRequest.Status.ToString());
            }

            if (userOrderRequest.Offset != 0)
            {
                builder.WithQueryParam("offset", userOrderRequest.Offset.ToString());
            }

            if (userOrderRequest.Limit != 0)
            {
                builder.WithQueryParam("limit", userOrderRequest.Limit.ToString());
            }

            var request = builder.GetResult(); 
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OrderPagingSlicedResult>();
            callback?.Try(result);
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

        internal void PreviewOrderPriceWithDiscountCode(string itemId
            , int quantity
            , int price
            , int discountedPrice
            , string currencyCode
            , string[] discountCodes
            , ResultCallback<OrderDiscountPreviewResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(itemId
                , Namespace_
                , AuthToken
                , Session.UserId);
            
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var previewRequest = new OrderDiscountPreviewRequest()
            {
                ItemId = itemId,
                Quantity = quantity,
                Price = price,
                DiscountedPrice = discountedPrice,
                CurrencyCode = currencyCode,
                DiscountCodes = (string[])discountCodes
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/orders/discount/preview")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithJsonBody(previewRequest)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<OrderDiscountPreviewResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }
    }
}