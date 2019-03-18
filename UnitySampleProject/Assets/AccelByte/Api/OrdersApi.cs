// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Net;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class OrdersApi
    {
        private readonly string baseUrl;

        internal OrdersApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can't construct purchase service! BaseUrl parameter is null!");

            this.baseUrl = baseUrl;
        }


        public IEnumerator<ITask> CreateOrder(string @namespace, string userId, string userAccessToken, 
            OrderRequest orderRequest,
            ResultCallback<OrderInfo> callback)
        {
            Assert.IsNotNull(@namespace, "Can't create order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create order! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't create order! UserAccessToken parameter is null!");
            Assert.IsNotNull(orderRequest, "Can't create order! OrderRequest parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/platform/public/namespaces/{namespace}/users/{userId}/orders")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(SimpleJson.SimpleJson.SerializeObject(orderRequest))
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<OrderInfo> result = response.TryParseJsonBody<OrderInfo>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserOrder(string @namespace, string userId, string userAccessToken,
            string orderNumber, ResultCallback<OrderInfo> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user's order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user's order! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user's order! UserAccessToken parameter is null!");
            Assert.IsNotNull(orderNumber, "Can't get user's order! OrderNumber parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/users/{userId}/orders/{orderNo}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("orderNo", orderNumber)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<OrderInfo> result = response.TryParseJsonBody<OrderInfo>();
            callback.Try(result);
        }
        
        public IEnumerator<ITask> GetUserOrders(string @namespace, string userId, string userAccessToken,
            uint page, uint size, ResultCallback<PagedOrderInfo> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user's order! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user's order! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user's order! UserAccessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/users/{userId}/orders")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithQueryParam("page", page.ToString())
                .WithQueryParam("size", size.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<PagedOrderInfo> result = response.TryParseJsonBody<PagedOrderInfo>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserOrderHistory(string @namespace, string userId, string userAccessToken,
            string orderNo, ResultCallback<OrderHistoryInfo[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user's order history! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user's order history! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user's order history! UserAccessToken parameter is null!");
            Assert.IsNotNull(orderNo, "Can't get user's order history! OrderNo parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/users/{userId}/orders/{orderNo}/history")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("orderNo", orderNo)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<OrderHistoryInfo[]> result = response.TryParseJsonBody<OrderHistoryInfo[]>();
            callback.Try(result);
        }
    }    
}
