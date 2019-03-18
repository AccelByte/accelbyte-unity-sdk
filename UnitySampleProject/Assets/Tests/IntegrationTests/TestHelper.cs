// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

namespace Tests
{
	public class TestHelper
	{
		private readonly AsyncTaskDispatcher taskDispatcher;
		private readonly AuthenticationApi authenticationApi;
		private readonly UserApi userApi;
		private readonly string clientId;
		private readonly string clientSecret;
		private readonly string publisherNamespace;

		public TestHelper()
		{
			this.taskDispatcher = new AsyncTaskDispatcher();
			this.authenticationApi = new AuthenticationApi(AccelBytePlugin.Config.IamServerUrl);
			this.clientId = Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID");
			this.clientSecret = Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET");
			this.publisherNamespace = Environment.GetEnvironmentVariable("PUBLISHER_NAMESPACE");
			this.userApi = new UserApi(AccelBytePlugin.Config.IamServerUrl);
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
			[DataMember] public string @namespace { get; set; }
			[DataMember] public string currencyType { get; set; }// REAL/VIRTUAL
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
			[DataMember] public string currencyType { get; set; }// REAL/VIRTUAL
			[DataMember] public string @namespace { get; set; }
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
			[DataMember] public Dictionary<string,string> localizationDisplayNames { get; set; }
		}


		[DataContract]
		public class ItemCreateModel
		{
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
				public string title;
				public string description;
				public Image[] images;
				public Image thumbnailImage;
			}

			[DataMember] public string itemType { get; set; }// APP/COINS/INGAMEITEM/BUNDLE/PRODUCT/CODE
			[DataMember] public string name { get; set; }
			[DataMember] public string entitlementType { get; set; }// DURABLE/CONSUMABLE
			[DataMember] public int useCount { get; set; }
			[DataMember] public string appId { get; set; }
			[DataMember] public string appType { get; set; }// GAME/SOFTWARE/DLC/DEMO
			[DataMember] public string targetCurrencyCode { get; set; }
			[DataMember] public string targetNamespace { get; set; }
			[DataMember] public string categoryPath { get; set; }
			[DataMember] public Localizations localizations { get; set; }
			[DataMember] public string status { get; set; }// ACTIVE/INACTIVE
			[DataMember] public string sku { get; set; }
			[DataMember] public RegionDatas regionData { get; set; }
			[DataMember] public string[] itemIds { get; set; }
			[DataMember] public string[] tags { get; set; }
			[DataMember] public int maxCountPerUser { get; set; }
			[DataMember] public int maxCount { get; set; }
		}

		[DataContract]
		public class StoreInfoModel
		{
			[DataMember] public string storeId { get; set; }
			[DataMember] public string @namespace { get; set; }
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
			[DataMember] public string @namespace { get; set; }
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
			[DataMember] public string @namespace { get; set; }
			[DataMember] public string sku { get; set; }
			[DataMember] public string name { get; set; }
			[DataMember] public string entitlementType { get; set; }
			[DataMember] public int useCount { get; set; }
			[DataMember] public string categoryPath { get; set; }
			[DataMember] public object localizations { get; set; }
			[DataMember] public string status { get; set; }// ACTIVE/INACTIVE
			[DataMember] public string itemType { get; set; }// APP/COINS/INGAMEITEM/BUNDLE/PRODUCT/CODE
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
			[DataMember] public string source { get; set; }// PURCHASE/PROMOTION/ACHIEVEMENT/REFERRAL_BONUS/REDEEM_CODE/OTHER
			[DataMember] public string reason { get; set; }
		}

		public static void LogResult(Result result, string message = "")
		{
			if (result.IsError)
			{
				Debug.Log("Error " + message + ":\n");
				var error = result.Error;

				while (error != null)
				{
					Debug.Log(string.Format("Code: {0}, Message: {1}\n", error.Code, error.Message));
					error = error.InnerError;
				}
			}
			else
			{
				Debug.Log("Success " + message + ".");
			}
		}

		public static void LogResult<T>(Result<T> result, string message = "")
		{
			if (result.IsError)
			{
				Debug.Log("Error " + message + ":\n");
				var error = result.Error;

				while (error != null)
				{
					Debug.Log(string.Format("Code: {0}, Message: {1}\n", error.Code, error.Message));
					error = error.InnerError;
				}
			}
			else
			{
				Debug.Log("Success " + message + " with value:");

				foreach (var propertyInfo in typeof(T).GetProperties())
				{
					Debug.Log("		" + propertyInfo.Name + ": " + propertyInfo.GetValue(result.Value, new object[]{}));
				}
			}
		}

		public void DeleteUser(User user, ResultCallback callback)
		{
			this.taskDispatcher.Start(this.DeleteAsync(user, callback));
		}

		public void DeleteUser(string @namespace, string email, string password, ResultCallback callback)
		{
			this.taskDispatcher.Start(this.DeleteAsync(@namespace, email, password, callback));
		}

		public void DeleteUser(string @namespace, PlatformType platformType, string platformToken,
			ResultCallback callback)
		{
			this.taskDispatcher.Start(this.DeleteAsync(@namespace, platformType, platformToken, callback));
		}

		public void GetAccessToken(ResultCallback<TokenData> callback)
		{
			this.taskDispatcher.Start(this.GetAccessTokenAsync(callback));
		}

		public void CreateCurrency(string accessToken, CurrencyCreateModel body, ResultCallback<CurrencyInfoModel> callback)
		{  
			this.taskDispatcher.Start(CreateCurrencyAsync(accessToken, body, callback));
		}

		public void GetPublishedStore(string accessToken, ResultCallback<StoreInfoModel> callback)
		{
			this.taskDispatcher.Start(GetPublishedStoreAsync(accessToken, callback));
		}

		public void CreateStore(string accessToken, StoreCreateModel body, ResultCallback<StoreInfoModel> callback)
		{
			this.taskDispatcher.Start(CreateStoreAsync(accessToken, body, callback));
		}

		public void CloneStore(string accessToken, string sourceStoreId, string targetStoreId, ResultCallback<StoreInfoModel> callback)
		{
			this.taskDispatcher.Start(CloneStoreAsync(accessToken, sourceStoreId, targetStoreId, callback));
		}

		public void PublishStore(string accessToken, string sourceStoreId, ResultCallback<StoreInfoModel> callback)
		{
			this.taskDispatcher.Start(PublishStoreAsync(accessToken, sourceStoreId, callback));
		}

		public void CreateCategory(string accessToken, string storeId, CategoryCreateModel body, ResultCallback<FullCategoryInfo> callback)
		{
			this.taskDispatcher.Start(CreateCategoryAsync(accessToken, storeId, body, callback));
		}

		public void CreateItem(string accessToken, string storeId, ItemCreateModel body, ResultCallback<FullItemInfo> callback)
		{
			this.taskDispatcher.Start(CreateItemAsync(accessToken, storeId, body, callback));
		}

		public void CreditWallet(string accessToken, string userId, string currencyCode, CreditRequestModel body, ResultCallback<WalletInfo> callback)
		{
			this.taskDispatcher.Start(CreditWalletAsync(accessToken, userId, currencyCode, body, callback));
		}

		public void DeleteCategory(string accessToken, string storeId, string categoryPath, ResultCallback<FullCategoryInfo> callback)
		{
			this.taskDispatcher.Start(DeleteCategoryAsync(accessToken, storeId, categoryPath, callback));
		}

		public void DeleteCurrency(string accessToken, string currencyCode, ResultCallback<CurrencyInfoModel> callback)
		{
			this.taskDispatcher.Start(DeleteCurrencyAsync(accessToken, currencyCode, callback));
		}

		public void DeleteStore(string accessToken, string storeId, ResultCallback<StoreInfoModel> callback)
		{
			this.taskDispatcher.Start(DeleteStoreAsync(accessToken, storeId, callback));
		}

		public void DeletePublishedStore(string accessToken, ResultCallback<StoreInfoModel> callback)
		{
			this.taskDispatcher.Start(DeletePublishedStoreAsync(accessToken, callback));
		}

		public void GetCurrencySummary(string accessToken, string currencyCode, ResultCallback<CurrencySummaryModel> callback)
		{
			this.taskDispatcher.Start(GetCurrencySummaryAsync(accessToken, currencyCode, callback));
		}
		
		public void GetStoreList(string accessToken, ResultCallback<StoreListModel> callback)
		{
			this.taskDispatcher.Start(GetStoreListAsync(accessToken, callback));
		}

		private IEnumerator<ITask> DeleteAsync(string @namespace, string email, string password,
			ResultCallback callback)
		{
			Result<TokenData> clientLoginResult = null;
			yield return Task.Await(
				this.authenticationApi.GetClientToken(this.clientId, this.clientSecret,
					result => clientLoginResult = result));

			if (clientLoginResult.IsError)
			{
				callback.TryError(clientLoginResult.Error.Code, clientLoginResult.Error.Message);
				yield break;
			}

			Result<TokenData> tokenResult = null;

			yield return Task.Await(
				this.authenticationApi.GetUserToken(@namespace, this.clientId, this.clientSecret, email, password,
					result => tokenResult = result));

			if (tokenResult.IsError)
			{
				callback.TryError(tokenResult.Error.Code, tokenResult.Error.Message);
				yield break;
			}

			Result<UserMapResponse> userMapResult = null;

			yield return Task.Await(this.GetUserMapping(tokenResult.Value.user_id, clientLoginResult.Value.access_token,
				result => userMapResult = result));

			if (userMapResult.IsError)
			{
				callback.TryError(userMapResult.Error.Code, userMapResult.Error.Message);
				yield break;
			}

			Result deleteUserResult = null;
			
			yield return Task.Await(
				this.Delete(
					userMapResult.Value.Namespace, userMapResult.Value.UserId, clientLoginResult.Value.access_token, 
					result => deleteUserResult = result));
			
			callback.Try(deleteUserResult);
		}

		private IEnumerator<ITask> Delete(string @namespace, string userId, string clientAccessToken, ResultCallback 
		callback)
		{
			var request =
				HttpRequestBuilder.CreateDelete(AccelBytePlugin.Config.IamServerUrl + "/iam/namespaces/{namespace}/users/{userId}")
					.WithPathParam("namespace", @namespace)
					.WithPathParam("userId", userId)
					.WithBearerAuth(clientAccessToken)
					.WithContentType(MediaType.ApplicationJson)
					.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParse();
			callback.Try(result);
		}

		[DataContract]
		public class UserMapResponse
		{
			[DataMember] public string Namespace { get; set; }
			[DataMember] public string UserId { get; set; }
		}

		private IEnumerator<ITask> GetUserMapping(string userId, string clientAccessToken, ResultCallback<UserMapResponse> callback)
		{
			var request =
				HttpRequestBuilder.CreateGet(AccelBytePlugin.Config.IamServerUrl +
											 "/iam/namespaces/{namespace}/users/{userId}/platforms/justice/{targetNamespace}")
					.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
					.WithPathParam("userId", userId)
					.WithPathParam("targetNamespace", publisherNamespace)
					.WithBearerAuth(clientAccessToken)
					.Accepts(MediaType.ApplicationJson)
					.ToRequest();

			HttpWebResponse response = null;
			
			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<UserMapResponse>();
			callback.Try(result);
		}

		private IEnumerator<ITask> DeleteAsync(User user, ResultCallback callback)
		{
			Result<TokenData> clientLoginResult = null;
			yield return Task.Await(
				this.authenticationApi.GetClientToken(this.clientId,
					this.clientSecret, result => clientLoginResult = result));

			Result<UserMapResponse> userMapResult = null;

			yield return Task.Await(this.GetUserMapping(user.UserId, clientLoginResult.Value.access_token,
				result => userMapResult = result));

			Result deleteUserResult = null;
			
			yield return Task.Await(
				this.Delete(userMapResult.Value.Namespace, userMapResult.Value.UserId,  
					clientLoginResult.Value.access_token, result => { deleteUserResult = result; }));
			
			callback.Try(deleteUserResult.IsError
				? Result.CreateError(deleteUserResult.Error.Code, deleteUserResult.Error.Message)
				: Result.CreateOk());
		}

		private IEnumerator<ITask> DeleteAsync(string @namespace, PlatformType platformType, string platformToken,
			ResultCallback callback)
		{
			Result<TokenData> clientLoginResult = null;
			yield return Task.Await(
				this.authenticationApi.GetClientToken(this.clientId,
					this.clientSecret, result => { clientLoginResult = result; }));

			Result<TokenData> loginResult = null;
			yield return Task.Await(
				this.authenticationApi.GetUserTokenWithOtherPlatform(@namespace, this.clientId, this.clientSecret, platformType,
					platformToken, result => { loginResult = result; }));

			Debug.Log("Name:" + loginResult.Value.display_name+ ", UserId:" + loginResult.Value.user_id);

			Result<UserMapResponse> userMapResult = null;
			yield return Task.Await(this.GetUserMapping(loginResult.Value.user_id, clientLoginResult.Value.access_token,
				result => userMapResult = result));

			Result deleteUserResult = null;
			yield return Task.Await(
				this.Delete(userMapResult.Value.Namespace, userMapResult.Value.UserId,
					 clientLoginResult.Value.access_token, result => { deleteUserResult = result; }));

			callback.Try(deleteUserResult.IsError
				? Result.CreateError(deleteUserResult.Error.Code, deleteUserResult.Error.Message)
				: Result.CreateOk());
		}

		public void SendNotification(string userId, bool isAsync, string message, ResultCallback callback)
		{
			this.taskDispatcher.Start(SendNotificationAsync(userId, isAsync, message, callback));
		}

		private IEnumerator<ITask> SendNotificationAsync(string userId, bool isAsync, string message, ResultCallback callback)
		{
			Result<TokenData> clientLoginResult = null;
			yield return Task.Await(
				this.authenticationApi.GetClientToken(this.clientId,
					this.clientSecret, clientResult => clientLoginResult = clientResult));

			string body = string.Format("{{\"message\": \"{0}\",\"topic\": \"none\" }}", message);
			
			 var request =
				HttpRequestBuilder.CreatePost(AccelBytePlugin.Config.LobbyServerUrl.Replace("wss", "https") +
											 "/notification/namespaces/{namespace}/users/{userId}/freeform")
					.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
					.WithPathParam("userId", userId)
					.WithPathParam("targetNamespace", publisherNamespace)
					.WithQueryParam("async", isAsync ? "true" : "false")
					.WithContentType(MediaType.ApplicationJson)
					.WithBody(body)
					.WithBearerAuth(clientLoginResult.Value.access_token)
					.Accepts(MediaType.ApplicationJson)
					.ToRequest();

			HttpWebResponse response = null;
			
			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParse();
			callback.Try(result);
		}

		private IEnumerator<ITask> GetAccessTokenAsync(ResultCallback<TokenData> callback)
		{
			Result<TokenData> clientLoginResult = null;
			yield return Task.Await(
				this.authenticationApi.GetClientToken(this.clientId,
					this.clientSecret, clientResult => clientLoginResult = clientResult));
			callback.Try(clientLoginResult);
		}

		private IEnumerator<ITask> CreateCurrencyAsync(string accessToken, CurrencyCreateModel body, ResultCallback<CurrencyInfoModel> callback)
		{

			var request = HttpRequestBuilder
				.CreatePost(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/currencies")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithBearerAuth(accessToken)
				.WithContentType(MediaType.ApplicationJson)
				.WithBody(SimpleJson.SimpleJson.SerializeObject(body))
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<CurrencyInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> GetPublishedStoreAsync(string accessToken, ResultCallback<StoreInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreateGet(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores/published")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> CreateStoreAsync(string accessToken,
			 StoreCreateModel body, ResultCallback<StoreInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreatePost(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithBearerAuth(accessToken)
				.WithContentType(MediaType.ApplicationJson)
				.WithBody(SimpleJson.SimpleJson.SerializeObject(body))
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> CloneStoreAsync(string accessToken, string sourceStoreId, string targetStoreId, ResultCallback<StoreInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreatePut(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores/{storeId}/clone")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("storeId", sourceStoreId)
				.WithQueryParam("targetStoreId", targetStoreId)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> PublishStoreAsync(string accessToken, string sourceStoreId, ResultCallback<StoreInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreatePut(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores/{storeId}/clone")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("storeId", sourceStoreId)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> CreateCategoryAsync(string accessToken, string storeId, CategoryCreateModel body, ResultCallback<FullCategoryInfo> callback)
		{
			var request = HttpRequestBuilder
				.CreatePost(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/categories")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithQueryParam("storeId", storeId)
				.WithContentType(MediaType.ApplicationJson)
				.WithBody(SimpleJson.SimpleJson.SerializeObject(body))
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<FullCategoryInfo>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> CreateItemAsync(string accessToken, string storeId, ItemCreateModel body, ResultCallback<FullItemInfo> callback)
		{
			var request = HttpRequestBuilder
				.CreatePost(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/items")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithQueryParam("storeId", storeId)
				.WithContentType(MediaType.ApplicationJson)
				.WithBody(SimpleJson.SimpleJson.SerializeObject(body))
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<FullItemInfo>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> CreditWalletAsync(string accessToken, string userId, string currencyCode, CreditRequestModel body, ResultCallback<WalletInfo> callback)
		{
			var request = HttpRequestBuilder
				.CreatePut(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/credit")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("userId", userId)
				.WithPathParam("currencyCode", currencyCode)
				.WithContentType(MediaType.ApplicationJson)
				.WithBody(SimpleJson.SimpleJson.SerializeObject(body))
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<WalletInfo>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> DeleteCategoryAsync(string accessToken, string storeId, string categoryPath, ResultCallback<FullCategoryInfo> callback)
		{
			var request = HttpRequestBuilder
				.CreateDelete(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/categories/{categoryPath}")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("categoryPath", categoryPath)
				.WithQueryParam("storeId", storeId)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<FullCategoryInfo>();
			callback.Try(result);
		}

		private IEnumerator<ITask> DeleteCurrencyAsync(string accessToken, string currencyCode, ResultCallback<CurrencyInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreateDelete(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/currencies/{currencyCode}")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("currencyCode", currencyCode)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<CurrencyInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> DeleteStoreAsync(string accessToken, string storeId, ResultCallback<StoreInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreateDelete(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores/{storeId}")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("storeId", storeId)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreInfoModel>();
			callback.Try(result);
		}

		private static IEnumerator<ITask> DeletePublishedStoreAsync(string accessToken, ResultCallback<StoreInfoModel> callback)
		{
			var request = HttpRequestBuilder
				.CreateDelete(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores/published")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreInfoModel>();
			callback.Try(result);
		}

		private IEnumerator<ITask> GetCurrencySummaryAsync(string accessToken, string currencyCode, ResultCallback<CurrencySummaryModel> callback)
		{
			var request = HttpRequestBuilder
				.CreateGet(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/currencies/{currencyCode}/summary")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithPathParam("currencyCode", currencyCode)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<CurrencySummaryModel>();
			callback.Try(result);
		}

		private IEnumerator<ITask> GetStoreListAsync(string accessToken, ResultCallback<StoreListModel> callback)
		{
			var request = HttpRequestBuilder
				.CreateGet(AccelBytePlugin.Config.PlatformServerUrl + "/platform/admin/namespaces/{namespace}/stores")
				.WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
				.WithBearerAuth(accessToken)
				.Accepts(MediaType.ApplicationJson)
				.ToRequest();

			HttpWebResponse response = null;

			yield return Task.Await(request, rsp => response = rsp);

			var result = response.TryParseJsonBody<StoreListModel>();
			callback.Try(result);
		}

		public static void Assert(Action action)
		{
			var stackFrame = new StackFrame(1, true);
			var methodName = Regex.Replace(stackFrame.GetMethod().DeclaringType.Name, @".*<([^)]+)>.*", "$1");
			
			try
			{
				action.Invoke();
				Debug.Log("PASSED TEST " + methodName);
			}
			catch (AssertionException ex)
			{
				Debug.Log("FAILED TEST " + methodName + ": " + ex.Message);
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
	}
}
