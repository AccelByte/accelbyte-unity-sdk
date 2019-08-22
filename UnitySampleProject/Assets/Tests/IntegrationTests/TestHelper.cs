// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine.Networking;
using Utf8Json;
using Debug = UnityEngine.Debug;

namespace Tests
{
    public class TestHelper
    {
        private readonly ILoginSession loginSession;
        private readonly UserAccount userAccount;
        private readonly string baseServerUrl;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string publisherNamespace;
        private readonly UnityHttpWorker httpWorker;
        private CoroutineRunner coroutineRunner;

        public TestHelper()
        {
            this.baseServerUrl = Environment.GetEnvironmentVariable("ADMIN_BASE_URL");
            this.clientId = Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID");
            this.clientSecret = Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET");
            this.publisherNamespace = Environment.GetEnvironmentVariable("PUBLISHER_NAMESPACE");
            //for test purpose, sometimes we need an admin permissions to make some test preparation like making a currency, etc.
            this.coroutineRunner = new CoroutineRunner();
            this.httpWorker = new UnityHttpWorker();

            this.loginSession = new OauthLoginSession(
                this.baseServerUrl + "/iam",
                AccelBytePlugin.Config.Namespace,
                this.clientId,
                this.clientSecret,
                AccelBytePlugin.Config.RedirectUri,
                this.httpWorker,
                this.coroutineRunner);

            this.userAccount = new UserAccount(
                this.baseServerUrl + "/iam",
                AccelBytePlugin.Config.Namespace,
                this.loginSession,
                this.httpWorker);
        }

        public static void LogStartTest()
        {
            var stackFrame = new StackFrame(1, true);
            string methodName = Regex.Replace(stackFrame.GetMethod().DeclaringType.Name, @".*<([^)]+)>.*", "$1");
            
            Debug.Log("=== START TEST: " + methodName + " ===");
        }

        public static void LogEndTest()
        {
            var stackFrame = new StackFrame(1, true);
            string methodName = Regex.Replace(stackFrame.GetMethod().DeclaringType.Name, @".*<([^)]+)>.*", "$1");
            
            Debug.Log("=== END TEST:  " + methodName + " ===");
        }

        public static void LogResult(Result result, string message = "")
        {
            if (result.IsError)
            {
                Debug.Log("Error " + message + ":\n");
                Error error = result.Error;

                while (error != null)
                {
                    Debug.Log(string.Format("Code: {0}, Message: {1}\n", error.Code, error.Message));
                    error = error.InnerError;
                }
            }
            else
            {
                Debug.Log("Success" + (string.IsNullOrEmpty(message) ? "" : " " + message) + ".");
            }
        }

        public static void LogResult<T>(Result<T> result, string message = "")
        {
            if (result.IsError)
            {
                Debug.Log("Error " + message + ":\n");
                Error error = result.Error;

                while (error != null)
                {
                    Debug.Log(string.Format("Code: {0}, Message: {1}\n", error.Code, error.Message));
                    error = error.InnerError;
                }
            }
            else
            {
                Debug.Log("Success" + (string.IsNullOrEmpty(message) ? "" : " " + message) + " with value:");

                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    Debug.Log("		" + propertyInfo.Name + ": " + propertyInfo.GetValue(result.Value, new object[] { }));
                }
            }
        }

        public void DeleteUser(User user, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteAsync(user, callback));
        }

        public void DeleteUser(PlatformType platformType, string platformToken,
            ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteAsync(platformType, platformToken, callback));
        }

        public void GetAccessToken(ResultCallback<TokenData> callback)
        {
            this.coroutineRunner.Run(GetAccessTokenAsync(callback));
        }

        public void CreateCurrency(string accessToken, CurrencyCreateModel body,
            ResultCallback<CurrencyInfoModel> callback)
        {
            this.coroutineRunner.Run(CreateCurrencyAsync(accessToken, body, callback));
        }

        public void GetPublishedStore(string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(GetPublishedStoreAsync(accessToken, callback));
        }

        public void CreateStore(string accessToken, StoreCreateModel body, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(CreateStoreAsync(accessToken, body, callback));
        }

        public void CloneStore(string accessToken, string sourceStoreId, string targetStoreId,
            ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(CloneStoreAsync(accessToken, sourceStoreId, targetStoreId, callback));
        }

        public void PublishStore(string accessToken, string sourceStoreId, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(PublishStoreAsync(accessToken, sourceStoreId, callback));
        }

        public void CreateCategory(string accessToken, string storeId, CategoryCreateModel body,
            ResultCallback<FullCategoryInfo> callback)
        {
            this.coroutineRunner.Run(CreateCategoryAsync(accessToken, storeId, body, callback));
        }

        public void CreateItem(string accessToken, string storeId, ItemCreateModel body,
            ResultCallback<FullItemInfo> callback)
        {
            this.coroutineRunner.Run(CreateItemAsync(accessToken, storeId, body, callback));
        }

        public void CreditWallet(string accessToken, string userId, string currencyCode, CreditRequestModel body,
            ResultCallback<WalletInfo> callback)
        {
            this.coroutineRunner.Run(CreditWalletAsync(accessToken, userId, currencyCode, body, callback));
        }

        public void DeleteCategory(string accessToken, string storeId, string categoryPath,
            ResultCallback<FullCategoryInfo> callback)
        {
            this.coroutineRunner.Run(DeleteCategoryAsync(accessToken, storeId, categoryPath, callback));
        }

        public void DeleteCurrency(string accessToken, string currencyCode, ResultCallback<CurrencyInfoModel> callback)
        {
            this.coroutineRunner.Run(DeleteCurrencyAsync(accessToken, currencyCode, callback));
        }

        public void DeleteStore(string accessToken, string storeId, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(DeleteStoreAsync(accessToken, storeId, callback));
        }

        public void DeletePublishedStore(string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(DeletePublishedStoreAsync(accessToken, callback));
        }

        public void GetCurrencySummary(string accessToken, string currencyCode,
            ResultCallback<CurrencySummaryModel> callback)
        {
            this.coroutineRunner.Run(GetCurrencySummaryAsync(accessToken, currencyCode, callback));
        }

        public void GetStoreList(string accessToken, ResultCallback<StoreListModel> callback)
        {
            this.coroutineRunner.Run(GetStoreListAsync(accessToken, callback));
        }

        public void CreateMatchmakingChannel(string accessToken, string channel, ResultCallback callback)
        {
            this.coroutineRunner.Run(CreateMatchmakingChannelAsync(accessToken, channel, callback));
        }

        public void GetStatByStatCode(string statCode, string accessToken, ResultCallback<StatInfo> callback)
        {
            this.coroutineRunner.Run(GetStatByStatCodeAsync(statCode, accessToken, callback));
        }

        public void createStat(string accessToken, StatCreateModel body, ResultCallback<StatInfo> callback)
        {
            this.coroutineRunner.Run(CreateStatAsync(accessToken, body, callback));
        }

        public void BulkCreateStatItem(string userId, string profileId, string[] statCode, string accessToken, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            this.coroutineRunner.Run(BulkCreateStatItemAsync(userId, profileId, statCode, accessToken, callback));
        }

        private IEnumerator CreateMatchmakingChannelAsync(string accessToken, string channel, ResultCallback callback)
        {
            string requestBody = string.Format(
                @"{{
                    ""description"": ""1v1 game mode for test"",
                    ""game_mode"": ""{0}"",
                    ""rule_set"": 
                    {{
                        ""alliance_number"": 2,
                        ""flexing_rule"": null,
                        ""matching_rule"": null,
                        ""symmetric_match"": true,
                        ""symmetric_party_number"": 1
                    }}
                }}",
                channel);

            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseServerUrl + "/matchmaking/namespaces/{namespace}/channels")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        public void DeleteMatchmakingChannel(string accessToken, string channel, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteMatchmakingChannelAsync(accessToken, channel, callback));
        }

        private IEnumerator DeleteMatchmakingChannelAsync(string accessToken, string channel, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseServerUrl + "/matchmaking/namespaces/{namespace}/channels/{channel}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("channel", channel)
                .WithBearerAuth(accessToken)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        public IEnumerator ClientLogin(ResultCallback<TokenData> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseServerUrl + "/iam/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        private IEnumerator Delete(string @namespace, string userId, string clientAccessToken, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseServerUrl + "/iam/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(clientAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator GetUserMapping(string userId, string clientAccessToken,
            ResultCallback<UserMapResponse> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(
                    this.baseServerUrl +
                    "/iam/namespaces/{namespace}/users/{userId}/platforms/justice/{targetNamespace}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("targetNamespace", this.publisherNamespace)
                .WithBearerAuth(clientAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<UserMapResponse> result = request.GetHttpResponse().TryParseJson<UserMapResponse>();
            callback.Try(result);
        }

        private IEnumerator DeleteAsync(User user, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return ClientLogin(result => clientLoginResult = result);

            Result<UserMapResponse> userMapResult = null;

            yield return GetUserMapping(
                user.Session.UserId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);


            yield return Delete(
                userMapResult.Value.Namespace,
                userMapResult.Value.UserId,
                clientLoginResult.Value.access_token,
                callback);
        }

        private IEnumerator DeleteAsync(PlatformType platformType, string platformToken, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return this.ClientLogin(
                result => { clientLoginResult = result; });

            Result loginResult = null;

            yield return this.loginSession.LoginWithOtherPlatform(
                platformType,
                platformToken,
                result => { loginResult = result; });

            Result<UserData> userResult = null;

            yield return this.userAccount.GetData(r => userResult = r);

            if (userResult.IsError)
            {
                callback.TryError(userResult.Error);

                yield break;
            }

            Result<UserMapResponse> userMapResult = null;

            yield return GetUserMapping(
                userResult.Value.UserId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);

            yield return Delete(
                userMapResult.Value.Namespace,
                userMapResult.Value.UserId,
                clientLoginResult.Value.access_token,
                callback);
        }

        public void SendNotification(string userId, bool isAsync, string message, ResultCallback callback)
        {
            this.coroutineRunner.Run(SendNotificationAsync(userId, isAsync, message, callback));
        }

        private IEnumerator SendNotificationAsync(string userId, bool isAsync, string message, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return this.ClientLogin(
                clientResult => clientLoginResult = clientResult);

            string body = string.Format("{{\"message\": \"{0}\",\"topic\": \"none\" }}", message);

            string url = AccelBytePlugin.Config.LobbyServerUrl.Replace("wss", "https") +
                "/notification/namespaces/{namespace}/users/{userId}/freeform";

            url = url.Replace("/lobby/", "");
            UnityWebRequest request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("targetNamespace", this.publisherNamespace)
                .WithQueryParam("async", isAsync ? "true" : "false")
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(body)
                .WithBearerAuth(clientLoginResult.Value.access_token)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator GetAccessTokenAsync(ResultCallback<TokenData> callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return this.ClientLogin(
                clientResult => clientLoginResult = clientResult);

            callback.Try(clientLoginResult);
        }

        private IEnumerator CreateCurrencyAsync(string accessToken, CurrencyCreateModel body,
            ResultCallback<CurrencyInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/currencies")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CurrencyInfoModel> result = request.GetHttpResponse().TryParseJson<CurrencyInfoModel>();
            callback.Try(result);
        }

        private IEnumerator GetPublishedStoreAsync(string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores/published")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator CreateStoreAsync(string accessToken, StoreCreateModel body,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator CloneStoreAsync(string accessToken, string sourceStoreId, string targetStoreId,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores/{storeId}/clone")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("storeId", sourceStoreId)
                .WithQueryParam("targetStoreId", targetStoreId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator PublishStoreAsync(string accessToken, string sourceStoreId,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores/{storeId}/clone")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("storeId", sourceStoreId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator CreateCategoryAsync(string accessToken, string storeId, CategoryCreateModel body,
            ResultCallback<FullCategoryInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/categories")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithQueryParam("storeId", storeId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<FullCategoryInfo> result = request.GetHttpResponse().TryParseJson<FullCategoryInfo>();
            callback.Try(result);
        }

        private IEnumerator CreateItemAsync(string accessToken, string storeId, ItemCreateModel body,
            ResultCallback<FullItemInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/items")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithQueryParam("storeId", storeId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<FullItemInfo> result = request.GetHttpResponse().TryParseJson<FullItemInfo>();
            callback.Try(result);
        }

        private IEnumerator CreditWalletAsync(string accessToken, string userId, string currencyCode,
            CreditRequestModel body, ResultCallback<WalletInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(
                    this.baseServerUrl +
                    "/platform/admin/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/credit")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<WalletInfo> result = request.GetHttpResponse().TryParseJson<WalletInfo>();
            callback.Try(result);
        }

        private IEnumerator DeleteCategoryAsync(string accessToken, string storeId, string categoryPath,
            ResultCallback<FullCategoryInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/categories/{categoryPath}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("categoryPath", categoryPath)
                .WithQueryParam("storeId", storeId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<FullCategoryInfo> result = request.GetHttpResponse().TryParseJson<FullCategoryInfo>();
            callback.Try(result);
        }

        private IEnumerator DeleteCurrencyAsync(string accessToken, string currencyCode,
            ResultCallback<CurrencyInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/currencies/{currencyCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CurrencyInfoModel> result = request.GetHttpResponse().TryParseJson<CurrencyInfoModel>();
            callback.Try(result);
        }

        private IEnumerator DeleteStoreAsync(string accessToken, string storeId,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores/{storeId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("storeId", storeId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator DeletePublishedStoreAsync(string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores/published")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator GetCurrencySummaryAsync(string accessToken, string currencyCode,
            ResultCallback<CurrencySummaryModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(
                    this.baseServerUrl + "/platform/admin/namespaces/{namespace}/currencies/{currencyCode}/summary")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CurrencySummaryModel> result = request.GetHttpResponse().TryParseJson<CurrencySummaryModel>();
            callback.Try(result);
        }

        private IEnumerator GetStoreListAsync(string accessToken, ResultCallback<StoreListModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseServerUrl + "/platform/admin/namespaces/{namespace}/stores")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreListModel> result = request.GetHttpResponse().TryParseJson<StoreListModel>();
            callback.Try(result);
        }

        private IEnumerator GetStatByStatCodeAsync(string statCode, string accessToken, ResultCallback<StatInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(baseServerUrl + "/statistic/admin/namespaces/{namespace}/stats/{statCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("statCode", statCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatInfo> result = request.GetHttpResponse().TryParseJson<StatInfo>();
            callback.Try(result);
        }

        private IEnumerator CreateStatAsync(string accessToken, StatCreateModel body,
            ResultCallback<StatInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(baseServerUrl + "/statistic/admin/namespaces/{namespace}/stats")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatInfo> result = request.GetHttpResponse().TryParseJson<StatInfo>();
            callback.Try(result);
        }

        private IEnumerator BulkCreateStatItemAsync(string userId, string profileId, string[] statCode, string accessToken,
            ResultCallback<BulkStatItemOperationResult[]> callback)
        {

            string body = "[";
            for (int i = 0; i < statCode.Length;i++)
            {
                body += string.Format(@"{{""statCode"":""{0}""}}", statCode[i]);
                if(i < statCode.Length - 1)
                {
                    body += ",";
                }
                else
                {
                    body += "]";
                }
            }
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(baseServerUrl + "/statistic/admin/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems/bulk/create")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(body)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<BulkStatItemOperationResult[]> result = request.GetHttpResponse().TryParseJson<BulkStatItemOperationResult[]>();
            callback.Try(result);
        }

        private static void TryRun(Action action)
        {
            var stackFrame = new StackFrame(2, true);
            string methodName = Regex.Replace(stackFrame.GetMethod().DeclaringType.Name, @".*<([^)]+)>.*", "$1");

            try
            {
                action.Invoke();
                Debug.Log("PASSED TEST " + methodName + " LINE " + stackFrame.GetFileLineNumber());
            }
            catch (AssertionException ex)
            {
                Debug.Log("FAILED TEST " + methodName + " LINE " + stackFrame.GetFileLineNumber() + ": " + ex.Message);
#if UNITY_EDITOR
                if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                    UnityEditor.EditorApplication.Exit(1);
                }
#endif

                throw;
            }
        }

        private static T TryRun<T>(Func<T> func)
        {
            var stackFrame = new StackFrame(2, true);
            string methodName = Regex.Replace(stackFrame.GetMethod().DeclaringType.Name, @".*<([^)]+)>.*", "$1");

            try
            {
                Debug.Log("PASSED TEST " + methodName + " LINE " + stackFrame.GetFileLineNumber());

                return func.Invoke();
            }
            catch (AssertionException ex)
            {
                Debug.Log("FAILED TEST " + methodName +  " LINE " + stackFrame.GetFileLineNumber() + ": " + ex.Message);
#if UNITY_EDITOR
                if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                    UnityEditor.EditorApplication.Exit(1);
                }
#endif

                throw;
            }
        }

        [DataContract]
        public class CurrencyCreateModel
        {
            [DataMember] public string currencyCode { get; set; }
            [DataMember] public string currencySymbol { get; set; }
            [DataMember] public string currencyType { get; set; } //REAL/VIRTUAL
            [DataMember] public int decimals { get; set; }
            [DataMember] public int maxAmountPerTransaction { get; set; }
            [DataMember] public int maxTransactionAmountPerDay { get; set; }
            [DataMember] public int maxBalanceAmount { get; set; }
        }

        [DataContract]
        public class CurrencyInfoModel
        {
            [DataMember] public string currencyCode { get; set; }
            [DataMember] public object localizationDescriptions { get; set; }
            [DataMember] public string currencySymbol { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string currencyType { get; set; } // REAL/VIRTUAL
            [DataMember] public int decimals { get; set; }
            [DataMember] public int maxAmountPerTransaction { get; set; }
            [DataMember] public int maxTransactionAmountPerDay { get; set; }
            [DataMember] public int maxBalanceAmount { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class CurrencySummaryModel
        {
            [DataMember] public string currencyCode { get; set; }
            [DataMember] public string currencySymbol { get; set; }
            [DataMember] public string currencyType { get; set; } // REAL/VIRTUAL
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public int decimals { get; set; }
        }

        [DataContract]
        public class StoreCreateModel
        {
            [DataMember] public string title { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string[] supportedLanguages { get; set; }
            [DataMember] public string[] supportedRegions { get; set; }
            [DataMember] public string defaultRegion { get; set; }
            [DataMember] public string defaultLanguage { get; set; }
        }

        [DataContract]
        public class CategoryCreateModel
        {
            [DataMember] public string categoryPath { get; set; }
            [DataMember] public Dictionary<string, string> localizationDisplayNames { get; set; }
        }

        [DataContract]
        public class ItemCreateModel
        {
            [DataMember] public string itemType { get; set; } // APP/COINS/INGAMEITEM/BUNDLE/PRODUCT/CODE
            [DataMember] public string name { get; set; }
            [DataMember] public string entitlementType { get; set; } // DURABLE/CONSUMABLE
            [DataMember] public int useCount { get; set; }
            [DataMember] public string appId { get; set; }
            [DataMember] public string appType { get; set; } // GAME/SOFTWARE/DLC/DEMO
            [DataMember] public string targetCurrencyCode { get; set; }
            [DataMember] public string targetNamespace { get; set; }
            [DataMember] public string categoryPath { get; set; }
            [DataMember] public Localizations localizations { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE/INACTIVE
            [DataMember] public string sku { get; set; }
            [DataMember] public RegionDatas regionData { get; set; }
            [DataMember] public string[] itemIds { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public int maxCountPerUser { get; set; }
            [DataMember] public int maxCount { get; set; }

            public class RegionDatas
            {
                public RegionData[] US;
            }

            public class Localizations
            {
                public Localization en;
            }

            public class Localization
            {
                public string description;
                public Image[] images;
                public Image thumbnailImage;
                public string title;
            }
        }

        [DataContract]
        public class StoreInfoModel
        {
            [DataMember] public string storeId { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string title { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public bool published { get; set; }
            [DataMember] public string[] supportedLanguages { get; set; }
            [DataMember] public string[] supportedRegions { get; set; }
            [DataMember] public string defaultRegion { get; set; }
            [DataMember] public string defaultLanguage { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class StoreListModel
        {
            [DataMember] public StoreInfoModel[] storeInfo { get; set; }
        }

        [DataContract]
        public class FullCategoryInfo
        {
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string parentCategoryPath { get; set; }
            [DataMember] public string categoryPath { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
            [DataMember] public object localizationDisplayNames { get; set; }
            [DataMember] public bool root { get; set; }
        }

        [DataContract]
        public class FullItemInfo
        {
            [DataMember] public string itemId { get; set; }
            [DataMember] public string appId { get; set; }
            [DataMember] public string appType { get; set; } // GAME/SOFTWARE/DLC/DEMO
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string sku { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public string entitlementType { get; set; }
            [DataMember] public int useCount { get; set; }
            [DataMember] public string categoryPath { get; set; }
            [DataMember] public object localizations { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE/INACTIVE
            [DataMember] public string itemType { get; set; } // APP/COINS/INGAMEITEM/BUNDLE/PRODUCT/CODE
            [DataMember] public string targetCurrencyCode { get; set; }
            [DataMember] public string targetNamespace { get; set; }
            [DataMember] public object regionData { get; set; }
            [DataMember] public string[] itemIds { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public int maxCountPerUser { get; set; }
            [DataMember] public int maxCount { get; set; }
            [DataMember] public string[] codeFiles { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class CreditRequestModel
        {
            [DataMember] public int amount { get; set; }

            [DataMember]
            public string source { get; set; } // PURCHASE/PROMOTION/ACHIEVEMENT/REFERRAL_BONUS/REDEEM_CODE/OTHER

            [DataMember] public string reason { get; set; }
        }

        [DataContract]
        public class UserMapResponse
        {
            [DataMember] public string Namespace { get; set; }
            [DataMember] public string UserId { get; set; }
        }

        [DataContract]
            public class StatCreateModel
        {
            [DataMember] public float defaultValue { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public bool incrementOnly { get; set; }
            [DataMember] public float maximum { get; set; }
            [DataMember] public float minimum { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public bool setAsGlobal { get; set; }
            [DataMember] public StatisticSetBy setBy { get; set; }
            [DataMember] public string statCode { get; set; }
        }


        public static class Assert
        {
            public static void IsTrue(bool? condition, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsTrue(condition, message, args));
            }

            public static void IsTrue(bool condition, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsTrue(condition, message, args));
            }

            public static void IsTrue(bool? condition)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsTrue(condition));
            }

            public static void IsTrue(bool condition)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsTrue(condition));
            }

            public static void IsFalse(bool? condition, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsFalse(condition, message, args));
            }

            public static void IsFalse(bool condition, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsFalse(condition, message, args));
            }

            public static void IsFalse(bool? condition)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsFalse(condition));
            }

            public static void IsFalse(bool condition)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.IsFalse(condition));
            }

            public static Exception Throws(IResolveConstraint expression, TestDelegate code, string message,
                params object[] args)
            {
                return TestHelper.TryRun(() => NUnit.Framework.Assert.Throws(expression, code, message, args));
            }

            public static Exception Throws(IResolveConstraint expression, TestDelegate code)
            {
                return TestHelper.TryRun(() => NUnit.Framework.Assert.Throws(expression, code));
            }

            public static Exception Throws(Type expectedExceptionType, TestDelegate code, string message,
                params object[] args)
            {
                return TestHelper.TryRun(
                    () => NUnit.Framework.Assert.Throws(expectedExceptionType, code, message, args));
            }

            public static Exception Throws(Type expectedExceptionType, TestDelegate code)
            {
                return TestHelper.TryRun(() => NUnit.Framework.Assert.Throws(expectedExceptionType, code));
            }

            public static TActual Throws<TActual>(TestDelegate code, string message, params object[] args)
                where TActual : Exception
            {
                return TestHelper.TryRun(() => NUnit.Framework.Assert.Throws<TActual>(code, message, args));
            }

            public static TActual Throws<TActual>(TestDelegate code) where TActual : Exception
            {
                return TestHelper.TryRun(() => NUnit.Framework.Assert.Throws<TActual>(code));
            }

            public static void DoesNotThrow(TestDelegate code, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.DoesNotThrow(code, message, args));
            }

            public static void DoesNotThrow(TestDelegate code)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.DoesNotThrow(code));
            }

            public static void That(bool condition, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(condition, message, args));
            }

            public static void That(bool condition)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(condition));
            }

            public static void That(bool condition, Func<string> getExceptionMessage)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(condition, getExceptionMessage));
            }

            public static void That(Func<bool> condition, string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(condition, message, args));
            }

            public static void That(Func<bool> condition)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(condition));
            }

            public static void That(Func<bool> condition, Func<string> getExceptionMessage)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(condition, getExceptionMessage));
            }

            public static void That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint expr)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(del, expr));
            }

            public static void That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint expr, string message,
                params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(del, expr, message, args));
            }

            public static void That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint expr,
                Func<string> getExceptionMessage)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(del, expr, getExceptionMessage));
            }

            public static void That(TestDelegate code, IResolveConstraint constraint)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(code, constraint));
            }

            public static void That(TestDelegate code, IResolveConstraint constraint, string message,
                params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(code, constraint, message, args));
            }

            public static void That(TestDelegate code, IResolveConstraint constraint, Func<string> getExceptionMessage)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(code, constraint, getExceptionMessage));
            }

            public static void That<TActual>(TActual actual, IResolveConstraint expression)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(actual, expression));
            }

            public static void That<TActual>(TActual actual, IResolveConstraint expression, string message,
                params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(actual, expression));
            }

            public static void That<TActual>(TActual actual, IResolveConstraint expression,
                Func<string> getExceptionMessage)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.That(actual, expression, getExceptionMessage));
            }

            public static void Pass(string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Pass(message, args));
            }

            public static void Pass(string message)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Pass(message));
            }

            public static void Pass()
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Pass());
            }

            public static void Fail(string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Fail(message, args));
            }

            public static void Fail(string message)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Fail(message));
            }

            public static void Fail()
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Fail());
            }

            public static void Ignore(string message, params object[] args)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Ignore(message, args));
            }

            public static void Ignore(string message)
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Ignore(message));
            }

            public static void Ignore()
            {
                TestHelper.TryRun(() => NUnit.Framework.Assert.Ignore());
            }
        }
    }
}