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

    internal class WalletApi
    {
        private readonly string baseUrl;

        internal WalletApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can't construct purchase service! BaseUrl parameter is null!");

            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetWalletInfoByCurrencyCode(string @namespace, string userId,
            string userAccessToken,
            string currencyCode, ResultCallback<WalletInfo> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get wallet info by currency code! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get wallet info by currency code! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken,
                "Can't get wallet info by currency code! UserAccessToken parameter is null!");
            Assert.IsNotNull(currencyCode, "Can't get wallet info by currency code! CurrencyCode parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl +
                           "/platform/public/namespaces/{namespace}/users/me/wallets/{currencyCode}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<WalletInfo> result = response.TryParseJsonBody<WalletInfo>();
            callback.Try(result);
        }

        //TODO: Wallet doesn't support single global account yet (get transcations from publisher wallet)
//        public IEnumerator<ITask> GetTransactions(string @namespace, string userId, string userAccessToken,
//            string currencyCode,
//            uint startPage, uint size, ResultCallback<PagedWalletTransactions> callback)
//        {
//            Assert.IsNotNull(@namespace, "Can't get transactions! Namespace parameter is null!");
//            Assert.IsNotNull(userId, "Can't get transactions! UserId parameter is null!");
//            Assert.IsNotNull(userAccessToken, "Can't get transactions! UserAccessToken parameter is null!");
//            Assert.IsNotNull(currencyCode, "Can't get transactions! CurrencyCode parameter is null!");
//
//            var request = HttpRequestBuilder
//                .CreateGet(this.baseUrl +
//                    "/platform/public/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/transactions")
//                .WithPathParam("namespace", @namespace)
//                .WithPathParam("userId", userId)
//                .WithPathParam("currencyCode", currencyCode)
//                .WithQueryParam("page", startPage.ToString())
//                .WithQueryParam("size", size.ToString())
//                .WithBearerAuth(userAccessToken)
//                .ToRequest();
//
//            HttpWebResponse response = null;
//
//            yield return Task.Await(request, rsp => response = rsp);
//
//            var result = response.TryParseJsonBody<PagedWalletTransactions>();
//            callback.Try(result);
//        }
    }
    
}