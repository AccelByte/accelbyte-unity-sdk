// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
using UnityEngine.UIElements.Experimental;

namespace AccelByte.Api
{
    public class WalletApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal WalletApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }
        
        [Obsolete("This does not support for multiplatform wallet, use GetWalletInfoByCurrencyCodeV2 instead")] 
        public IEnumerator GetWalletInfoByCurrencyCode( string userId
            , string currencyCode
            , ResultCallback<WalletInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get wallet info by currency code! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get wallet info by currency code! UserId parameter is null!");
            Assert.IsNotNull(
                AuthToken,
                "Can't get wallet info by currency code! accessToken parameter is null!");

            Assert.IsNotNull(currencyCode, "Can't get wallet info by currency code! CurrencyCode parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/wallets/{currencyCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);
            
            var result = response.TryParseJson<WalletInfoResponse>();
            WalletInfo walletInfo = null;
            if (result.Value.walletInfos.Length > 0)
            {
                walletInfo = result.Value.walletInfos[0];
            }
            else
            {
                walletInfo.id = result.Value.id;
                walletInfo.Namespace = result.Value.Namespace;
                walletInfo.userId = result.Value.userId;
                walletInfo.currencyCode = result.Value.currencyCode;
                walletInfo.currencySymbol = result.Value.currencySymbol;
                walletInfo.balance = result.Value.balance;
                walletInfo.status = result.Value.status;
            }
            callback.TryOk(walletInfo);
        }
        
        public IEnumerator GetWalletInfoByCurrencyCodeV2( string userId
            , string currencyCode
            , ResultCallback<WalletInfoResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get wallet info by currency code! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get wallet info by currency code! UserId parameter is null!");
            Assert.IsNotNull(
                AuthToken,
                "Can't get wallet info by currency code! accessToken parameter is null!");

            Assert.IsNotNull(currencyCode, "Can't get wallet info by currency code! CurrencyCode parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/wallets/{currencyCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<WalletInfoResponse>();
            callback.Try(result);
        }
    }
}
