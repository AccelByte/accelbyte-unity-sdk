// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

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
            , ResultCallback<OrderInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create order! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't create order! UserAccessToken parameter is null!");
            Assert.IsNotNull(orderRequest, "Can't create order! OrderRequest parameter is null!");

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
            Assert.IsNotNull(orderNo, "Can't cancel the order! orderNo parameter is null!");
            Assert.IsNotNull(Namespace_, "Can't cancel the order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't cancel the order! userId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't cancel the order! userAccessToken parameter is null!");

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
            , ResultCallback<OrderInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user's order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user's order! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user's order! UserAccessToken parameter is null!");
            Assert.IsNotNull(orderNumber, "Can't get user's order! OrderNumber parameter is null!");

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
            , ResultCallback<OrderPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user's order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user's order! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user's order! UserAccessToken parameter is null!");

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
            Assert.IsNotNull(Namespace_, "Can't get user's order history! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user's order history! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user's order history! UserAccessToken parameter is null!");
            Assert.IsNotNull(orderNo, "Can't get user's order history! OrderNo parameter is null!");

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