// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Networking;
using Utf8Json;
using Debug = UnityEngine.Debug;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Tests
{
    public class TestHelper
    {
        private readonly LoginSession loginSession;
        private readonly UserAccount userAccount;
        private readonly AccelByteHttpClient httpClient;
        private readonly CoroutineRunner coroutineRunner;
        private TokenData superUserAccess = null;
        private readonly string superUserAdminUsername;
        private readonly string superUserAdminPassword;
        private readonly Config config;

        public TestHelper()
        {
            this.config = new Config
            {
                BaseUrl = Environment.GetEnvironmentVariable("ADMIN_BASE_URL"),
                ClientId = Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET"),
                PublisherNamespace = Environment.GetEnvironmentVariable("PUBLISHER_NAMESPACE")
            };
            this.config.Expand();

            this.superUserAdminUsername = Environment.GetEnvironmentVariable("ADMIN_USER_NAME");
            this.superUserAdminPassword = Environment.GetEnvironmentVariable("ADMIN_USER_PASS");

            CheckConfigValue(this.config.BaseUrl, "ADMIN_BASE_URL");
            CheckConfigValue(this.config.ClientId, "ADMIN_CLIENT_ID");
            CheckConfigValue(this.config.ClientSecret, "ADMIN_CLIENT_SECRET");
            CheckConfigValue(this.config.PublisherNamespace, "PUBLISHER_NAMESPACE");
            CheckConfigValue(this.superUserAdminUsername, "ADMIN_USER_NAME");
            CheckConfigValue(this.superUserAdminPassword, "ADMIN_USER_PASS");
            //for test purpose, sometimes we need an admin permissions to make some test preparation like making a currency, etc.
            this.coroutineRunner = new CoroutineRunner();
            this.httpClient = new AccelByteHttpClient();
            this.httpClient.SetCredentials(this.config.ClientId, this.config.ClientSecret);

            this.loginSession = new LoginSession(
                this.config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                AccelBytePlugin.Config.RedirectUri,
                this.httpClient,
                this.coroutineRunner);

            this.userAccount = new UserAccount(
                this.config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                this.loginSession,
                this.httpClient);
        }

        private static void CheckConfigValue(string value, string varName)
        {
            if (string.IsNullOrEmpty(value))
                Debug.LogError(string.Format("Env variable \"{0}\" is not found.\n" +
                    "Please check your windows env variable." +
                    "Restart Unity after you added it.", varName));
        }

        private static IEnumerator SendAndLogRequest(IHttpRequest request, UnityWebRequest unityWebRequest)
        {
            unityWebRequest.certificateHandler = new BypassCertificate();

            Report.GetHttpRequest(request, unityWebRequest);

            yield return unityWebRequest.SendWebRequest();

            Report.GetHttpResponse(unityWebRequest);
        }

        // Wait until the condition is true, throws TimeoutException on timeout
        public static IEnumerator WaitUntil(Func<bool> condition, string message = null,
            int timeoutMs = 60 * 1000,
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (message != null)
            {
                Debug.Log($"Waiting: {message}");
            }

            while (!condition.Invoke())
            {
                if (timeoutMs <= 0)
                {
                    throw new TimeoutException();
                }

                yield return new WaitForSeconds(0.1f);
                timeoutMs -= 100;
            }
        }

        // Wait until value is not null, throws TimeoutException on timeout
        public static IEnumerator WaitForValue(Func<object> function, string message = null,
            int timeoutMs = 60*1000,
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
#if DEBUG
            Debug.Log($"{nameof(WaitForValue)}: {fileName}:{lineNumber}\n");
#endif
            
            return WaitUntil(() => function() != null, message, timeoutMs, 
                fileName, lineNumber); // Pass file name and line number explicitly from WaitForValue
        }

#if !DISABLESTEAMWORKS
        public static string GenerateSteamTicket()
        {
            var stringBuilder = new StringBuilder();

            if (SteamManager.Initialized)
            {
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int)actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            var steamTicket = stringBuilder.ToString();

            Thread.Sleep(2000);

            return steamTicket;
        }
#endif

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
            if (result == null)
            {
                Debug.Log("Result is null\n");
            }
            else if (result.IsError)
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

                if (result.Value == null)
                {
                    Debug.Log("<null>");
                }
                else
                {
                    foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                    {
                        Debug.Log("		" + propertyInfo.Name + ": " + propertyInfo.GetValue(result.Value, new object[] { }));
                    }
                }
            }
        }

        public static string GenerateUnique(string prefix)
        {
            return prefix + Guid.NewGuid().ToString().Replace("-", "");
        }

        public string GetPublisherNamespace()
        {
            return this.config.PublisherNamespace;
        }


        public Enviroment GetEnviroment()
        {
            string baseUrl = this.config.BaseUrl.ToLower();
            if (baseUrl.Contains("dev"))
            {
                return Enviroment.DEV;
            }
            else if (baseUrl.Contains("demo"))
            {
                return Enviroment.DEMO;

            }
            return Enviroment.UNKNOWN;
        }

        public void DeleteUser(string userId, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteAsync(userId, callback));
        }

        public void DeleteUser(User user, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteAsync(user, callback));
        }

        public void DeleteUserByDisplayName(string displayName, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteUserByDisplayNameAsync(displayName, callback));
        }

        public void DeleteUser(PlatformType platformType, string platformToken, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteAsync(platformType, platformToken, callback));
        }

        public void DeleteUserProfile(User user, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteUserProfileAsync(user, callback));
        }

        public void Register(RegisterUserRequest registerUserRequest, ResultCallback<RegisterUserResponse> callback)
        {
            this.coroutineRunner.Run(RegisterAsync(registerUserRequest, callback));
        }

        public void LoginThenGetUserData(string username, string password, ResultCallback<UserData> callback)
        {
            this.coroutineRunner.Run(LoginThenGetUserDataAsync(username, password, callback));
        }

        public void GetAuthCode(string userAccessToken, ResultCallback<string> callback)
        {
            this.coroutineRunner.Run(GetAuthCodeAsync(userAccessToken, callback));
        }

        public void LoginInLauncher(string username, string password, ResultCallback<string> callback)
        {
            this.coroutineRunner.Run( LoginInLauncherAsync(username, password, callback));
        }

        public void GetAccessToken(ResultCallback<TokenData> callback)
        {
            this.coroutineRunner.Run(GetAccessTokenAsync(callback));
        }

        public void GetSuperUserAccessToken(ResultCallback<TokenData> callback)
        {
            if (superUserAccess != null)
            {
                callback.TryOk(superUserAccess);
            }
            else
            {
                coroutineRunner.Run(GetSuperUserAccessTokenAsync(result =>
                {
                    callback.Try(result);
                    if (!result.IsError)
                    {
                        superUserAccess = result.Value;
                    }
                }));
            }
        }

        public void GetUserMap(string userId, ResultCallback<UserMapResponse> callback)
        {
            this.coroutineRunner.Run(GetUserMapAsync(userId, callback));
        }
        
        public void CreateCurrency(string accessToken, CurrencyCreateModel body,
            ResultCallback<CurrencyInfoModel> callback)
        {
            this.coroutineRunner.Run(CreateCurrencyAsync(accessToken, body, callback));
        }

        public void GetPublishedStore(string namespace_, string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(GetPublishedStoreAsync(namespace_, accessToken, callback));
        }

        public void CreateStore(string accessToken, StoreCreateModel body, ResultCallback<StoreInfoModel> callback)
        {
            string namespace_ = AccelBytePlugin.Config.Namespace;
            this.coroutineRunner.Run(CreateStoreAsync(namespace_, accessToken, body, callback));
        }

        public void CreateStore(string namespace_, string accessToken, StoreCreateModel body, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(CreateStoreAsync(namespace_, accessToken, body, callback));
        }

        public void CloneStore(string namespace_, string accessToken, string sourceStoreId, string targetStoreId,
            ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(CloneStoreAsync(namespace_, accessToken, sourceStoreId, targetStoreId, callback));
        }

        public void RollbackStore(string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(RollbackStoreAsync(accessToken, callback));
        }

        public void PublishStore(string namespace_,string accessToken, string sourceStoreId, ResultCallback<StoreInfoModel> callback)
        {
            this.coroutineRunner.Run(PublishStoreAsync(namespace_, accessToken, sourceStoreId, callback));
        }

        public void CreateCategory(string accessToken, string storeId, CategoryCreateModel body,
            ResultCallback<FullCategoryInfo> callback)
        {
            string namespace_ = AccelBytePlugin.Config.Namespace;
            this.coroutineRunner.Run(CreateCategoryAsync(namespace_, accessToken, storeId, body, callback));
        }

        public void CreateCategory(string namespace_, string accessToken, string storeId, CategoryCreateModel body,
            ResultCallback<FullCategoryInfo> callback)
        {
            //AccelBytePlugin.Config.Namespace
            this.coroutineRunner.Run(CreateCategoryAsync(namespace_, accessToken, storeId, body, callback));
        }

        public void CreateItem(string accessToken, string storeId, ItemCreateModel body,
            ResultCallback<FullItemInfo> callback)
        {
            string namespace_ = AccelBytePlugin.Config.Namespace;
            this.coroutineRunner.Run(CreateItemAsync(namespace_, accessToken, storeId, body, callback));
        }

        public void CreateItem(string namespace_, string accessToken, string storeId, ItemCreateModel body,
            ResultCallback<FullItemInfo> callback)
        {
            this.coroutineRunner.Run(CreateItemAsync(namespace_, accessToken, storeId, body, callback));
        }

        public void GetItemBySKU(string namespace_, string accessToken, string storeId, string sku, bool activeOnly,
             ResultCallback<FullItemInfo> callback)
        {
            this.coroutineRunner.Run(GetItemBySkuAsync(namespace_, accessToken, storeId, sku, activeOnly, callback));
        }

        public void FulfillItem(string namespace_, string accessToken, string userId, FulfillmentModel body,
             ResultCallback callback)
        {
            this.coroutineRunner.Run(FulfillItemAsync(namespace_, accessToken, userId, body, callback));
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

        public void GetStoreList(string accessToken, string namespace_, ResultCallback<StoreInfoModel[]> callback)
        {
            this.coroutineRunner.Run(GetStoreListAsync(accessToken, namespace_, callback));
        }

        public void GetStoreList(string accessToken, ResultCallback<StoreInfoModel[]> callback)
        {
            this.coroutineRunner.Run(GetStoreListAsync(accessToken, AccelBytePlugin.Config.Namespace, callback));
        }

        public void GetStatConfigByStatCode(string statCode, string accessToken, ResultCallback<StatConfig> callback)
        {
            this.coroutineRunner.Run(GetStatConfigByStatCodeAsync(statCode, accessToken, callback));
        }

        public void CreateStatConfig(string accessToken, StatCreateModel body, ResultCallback<StatConfig> callback)
        {
            this.coroutineRunner.Run(CreateStatConfigAsync(accessToken, body, callback));
        }

        public void CreateUserStatItems(string userId, string[] statCodes, string accessToken,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            this.coroutineRunner.Run(CreateUserStatItemsAsync(userId, statCodes, accessToken, callback));
        }

        public void DeleteStatItem(string accessToken, string userId, string statCode, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteStatItemAsync(accessToken, userId, statCode, callback));
        }

        public void GetUserVerificationCode(string userId, string accessToken,
            ResultCallback<UserVerificationCode> callback)
        {
            this.coroutineRunner.Run(GetUserVerificationCodeAsync(userId, accessToken, callback));
        }

        public void CreateAchievement(string accessToken, AchievementRequest achievementRequest,
            ResultCallback<AchievementResponse> callback)
        {
            this.coroutineRunner.Run(CreateAchievementAsync(accessToken, achievementRequest, callback));
        }

        public void DeleteAchievement(string accessToken, string achievementCode,
            ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteAchievementAsync(accessToken, achievementCode, callback));
        }

        public void CreateCampaign(string accessToken, CampaignCreateModel body, ResultCallback<CampaignInfo> callback)
        {
            this.coroutineRunner.Run(CreateCampaignAsync(accessToken, body, callback));
        }

        public void GetCampaignByName(string accessToken, string name, ResultCallback<CampaignPagingSlicedResult> callback)
        {
            this.coroutineRunner.Run(GetCampaignByNameAsync(accessToken, name, callback));
        }

        public void UpdateCampaign(string accessToken, string campaignId, CampaignUpdateModel body, ResultCallback<CampaignInfo> callback)
        {
            this.coroutineRunner.Run(UpdateCampaignAsync(accessToken, campaignId, body, callback));
        }

        public void CreateCampaignCodes(string accessToken, string campaignId, CampaignCodeCreateModel body, ResultCallback<CampaignCodeCreateResult> callback)
        {
            this.coroutineRunner.Run(CreateCampaignCodesAsync(accessToken, campaignId, body, callback));
        }

        public void GetCampaignCodes(string accessToken, string campaignId, ResultCallback<CodeInfoPagingSlicedResult> callback)
        {
            this.coroutineRunner.Run(GetCampaignCodesAsync(accessToken, campaignId, callback));
        }

        public void DisableCampaignCode(string accessToken, string code, ResultCallback<CodeInfo> callback)
        {
            this.coroutineRunner.Run(DisableCampaignCodesAsync(accessToken, code, callback));
        }

        public void CreateReward(string accessToken, RewardCreateModel body, ResultCallback<RewardInfo> callback)
        {
            this.coroutineRunner.Run(CreateRewardAsync(accessToken, body, callback));
        }

        public void QueryRewards(string accessToken, string eventTopic, int offset, int limit, string sortBy, ResultCallback<QueryRewardInfo> callback)
        {
            this.coroutineRunner.Run(QueryRewardsAsync(accessToken, eventTopic, offset, limit, sortBy, callback));
        }

        public void DeleteReward(string accessToken, string rewardId, ResultCallback<QueryRewardInfo> callback)
        {
            this.coroutineRunner.Run(DeleteRewardAsync(accessToken, rewardId, callback));
        }

        #region Leaderboard
        public void LeaderboardCreateLeaderboard(LeaderboardConfigRequest body,
            ResultCallback<LeaderboardConfigRequest> callback)
        {
            this.coroutineRunner.Run(LeaderboardCreateLeaderboardAsync(body, callback));
        }

        public void LeaderboardGetAllLeaderboard(string accessToken, string leaderboardCode,
            ResultCallback<LeaderboardConfigResponse> callback)
        {
            this.coroutineRunner.Run(LeaderboardGetAllLeaderboardAsync(accessToken, leaderboardCode, callback));
        }

        public void LeaderboardDeleteLeaderboard(string leaderboardCode,
            ResultCallback callback)
        {
            this.coroutineRunner.Run(LeaderboardDeleteLeaderboardAsync(leaderboardCode, callback));
        }
        #endregion Leaderboard

        #region Group
        public void CreateDefaultGroupConfig(string accessToken, ResultCallback<CreateGroupConfigResponse> callback)
        {
            this.coroutineRunner.Run(CreateDefaultGroupConfigAsync(accessToken, callback));
        }

        public void GetGroupConfig(string accessToken, ResultCallback<PaginatedGroupConfig> callback)
        {
            this.coroutineRunner.Run(GetGroupConfigAsync(accessToken, callback));
        }

        public void DeleteGroupConfig(string accessToken, string configurationCode, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteGroupConfigAsync(accessToken, AccelBytePlugin.Config.Namespace, configurationCode, callback));
        }

        public void AdminSearchGroup(string accessToken, string groupName, ResultCallback<PaginatedGroupListResponse> callback)
        {
            this.coroutineRunner.Run(AdminSearchGroupsAsync(accessToken, callback, groupName));
        }

        public void AdminDeleteGroup(string accessToken, string groupId, ResultCallback callback)
        {
            this.coroutineRunner.Run(AdminDeleteGroupAsync(accessToken, groupId, callback));
        }

        public void CreateMemberRole(string accessToken, CreateMemberRoleRequest createMemberRoleRequest, ResultCallback<CreateMemberRoleResponse> callback)
        {
            this.coroutineRunner.Run(CreateMemberRoleAsync(accessToken, createMemberRoleRequest, callback));
        }

        public void DeleteMemberRole(string accessToken, string memberRoleId, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteMemberRoleAsync(accessToken, memberRoleId, callback));
        }
        #endregion Group

        #region Agreement
        public void AgreementCreateBasePolicy(string accessToken, AgreementBasePolicyCreate body, 
            ResultCallback<AgreementBasePolicy> callback)
        {
            this.coroutineRunner.Run(AgreementCreateBasePolicyAsync(accessToken, body, callback));
        }

        public void AgreementCreatePolicyVersion(string accessToken, string policyId, AgreementPolicyVersionCreate body, 
            ResultCallback<AgreementPolicyVersion> callback)
        {
            this.coroutineRunner.Run(AgreementCreatePolicyVersionAsync(accessToken, policyId, body, callback));
        }

        public void AgreementCreateLocalizedPolicy(string accessToken, string policyVersionId, 
            AgreementLocalizedPolicyCreate body, ResultCallback<AgreementLocalizedPolicy> callback)
        {
            this.coroutineRunner.Run(AgreementCreateLocalizedPolicyAsync(accessToken, policyVersionId, body, callback));
        }

        public void AgreementPublishPolicyVersion(string accessToken, string policyVersionId, bool shouldNotify, 
            ResultCallback callback)
        {
            this.coroutineRunner.Run(AgreementPublishPolicyVersionAsync(accessToken, policyVersionId, shouldNotify, callback));
        }

        public void AgreementGetBasePolicies(string accessToken, ResultCallback<AgreementBasePolicy[]> callback)
        {
            this.coroutineRunner.Run(AgreementGetBasePoliciesAsync(accessToken, callback));
        }

        public void AgreementGetCountryBasePolicy(string accessToken, string policyId, string countryCode, 
            ResultCallback<AgreementCountryPolicy> callback)
        {
            this.coroutineRunner.Run(AgreementGetCountryBasePolicyAsync(accessToken, policyId, countryCode, callback));
        }

        public void AgreementGetPolicyTypes(string accessToken, ResultCallback<AgreementPolicyTypeObject[]> callback)
        {
            this.coroutineRunner.Run(AgreementGetPolicyTypesAsync(accessToken, callback));
        }

        public void AgreementGetLocalizedPolicies(string accessToken, string policyVersionId, 
            ResultCallback<AgreementLocalizedPolicy[]> callback)
        {
            this.coroutineRunner.Run(AgreementGetLocalizedPoliciesAsync(accessToken, policyVersionId, callback));
        }
        #endregion Agreement

        #region Profanity Filter
        public void CreateProfanityFilterList(string accessToken, bool isEnabled, bool isMandatory, string name,
            ResultCallback callback)
        {
            this.coroutineRunner.Run(CreateProfanityFilterListAsync(accessToken, isEnabled, isMandatory, name, callback));
        }

        public void SetProfanityFilterRule(string accessToken, ProfanityNamespaceRule rule,
            ResultCallback callback)
        {
            this.coroutineRunner.Run(SetProfanityFilterRuleAsync(accessToken, rule, callback));
        }

        public void GetProfanityFilterRule(string accessToken, ResultCallback<AdminSetProfanityRuleForNamespaceRequest> callback)
        {
            this.coroutineRunner.Run(GetProfanityFilterRuleAsync(accessToken, callback));
        }

        // insert bulk to filter list
        public void AddFilterIntoListBulk(string accessToken, string listName, string[] filters,
            ResultCallback callback)
        {
            this.coroutineRunner.Run(AddFilterIntoListBulkAsync(accessToken, listName, filters, callback));
        }

        // delete filter list (with all filter included)
        public void DeleteProfanityFilterList(string accessToken, string listName,
             ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteProfanityFilterListAsync(accessToken, listName, callback));
        }
        #endregion

        #region Reporting
        public void ReportingAdminGetReasons(string namespace_, string accessToken, string reasonGroup, ResultCallback<ReportingAdminReasonsResponse> callback)
        {
            this.coroutineRunner.Run(ReportingAdminGetReasonsAsync(namespace_, accessToken, reasonGroup, callback));
        }

        public void ReportingAddReason(string namespace_, string accessToken, ReportingAddReasonRequest reasonRequest, ResultCallback callback)
        {
            this.coroutineRunner.Run(ReportingAddReasonAsync(namespace_, accessToken, reasonRequest, callback));
        }

        public void ReportingDeleteReason(string namespace_, string accessToken, string reasonId, ResultCallback callback)
        {
            this.coroutineRunner.Run(ReportingDeleteReasonAsync(namespace_, accessToken, reasonId, callback));
        }

        public void ReportingAdminGetReasonGroups(string namespace_, string accessToken, ResultCallback<ReportingReasonGroupsResponse> callback)
        {
            this.coroutineRunner.Run(ReportingAdminGetReasonGroupsAsync(namespace_, accessToken, callback));
        }

        public void ReportingAddReasonGroup(string namespace_, string accessToken, ReportingAddReasonGroupRequest reasonGroupRequest, ResultCallback callback)
        {
            this.coroutineRunner.Run(ReportingAddReasonGroupAsync(namespace_, accessToken, reasonGroupRequest, callback));
        }

        public void ReportingDeleteReasonGroup(string namespace_, string accessToken, string reasonGroupId, ResultCallback callback)
        {
            this.coroutineRunner.Run(ReportingDeleteReasonGroupAsync(namespace_, accessToken, reasonGroupId, callback));
        }
        #endregion

        public void FreeSubscribeByPlatform(string namespace_, string accessToken, string userId, FreeSubscritptionRequest body,
            ResultCallback<FullItemInfo> callback)
        {
            this.coroutineRunner.Run(FreeSubscribeByPlatformAsync(namespace_, accessToken, userId, body, callback));
        }

        [DataContract]
        public class RoleV4Response
        {
            [DataMember] public bool adminRole;
            [DataMember] public bool isWildcard;
            [DataMember] public Permission[] permissions;
            [DataMember] public string roleId;
            [DataMember] public string roleName;
        }

        [DataContract]
        public class ListRoleV4Response
        {
            [DataMember] public RoleV4Response[] data;
            [DataMember] public Paging paging;
        }

        public void GetRoles(bool isWildcard, bool adminRole, int limit, string after, string before, ResultCallback<ListRoleV4Response> callback)
        {
            this.coroutineRunner.Run(GetRolesAsync(isWildcard, adminRole, limit, after, before, callback));
        }

        public IEnumerator GetRolesAsync(bool isWildcard, bool adminRole, int limit, string after, string before, ResultCallback<ListRoleV4Response> callback)
        {
            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(tokenResult => getAccessTokenResult = tokenResult);
            yield return new WaitUntil(() => getAccessTokenResult != null);

            var request = HttpRequestBuilder
                .CreateGet(this.config.BaseUrl + "/iam/v4/admin/roles")
                .WithQueryParam("isWildcard", isWildcard.ToString())
                .WithQueryParam("adminRole", adminRole.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("after", after)
                .WithQueryParam("before", before)
                .WithBearerAuth(getAccessTokenResult.Value.access_token)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ListRoleV4Response>();
            callback.Try(result);
        }

        [DataContract]
        public class AssignUserV4Request
        {
            [DataMember] public string[] assignedNamespaces;
            [DataMember(Name = "namespace")] public string Namespace;
            [DataMember] public string userId;
        }

        [DataContract]
        public class AssignUserV4Response
        {
            [DataMember] public string[] assignedNamespaces;
            [DataMember] public string displayName;
            [DataMember] public string email;
            [DataMember] public string roleId;
            [DataMember] public string userId;
        }

        public void AddUserRole(string userId, string roleId, string[] assignedNamespace, ResultCallback<AssignUserV4Response> callback)
        {
            this.coroutineRunner.Run(AddUserRoleAsync(userId, roleId, assignedNamespace, callback));
        }

        private IEnumerator AddUserRoleAsync(string userId, string roleId, string[] assignedNamespace, ResultCallback<AssignUserV4Response> callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return ClientLogin(result => clientLoginResult = result);

            Result<UserMapResponse> userMapResult = null;

            yield return GetUserMapping(
                userId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);

            if (!userMapResult.IsError)
            {
                var assignRoleRequest = new AssignUserV4Request
                {
                    assignedNamespaces = assignedNamespace,
                    Namespace = this.config.PublisherNamespace,
                    userId = userMapResult.Value.UserId
                };

                yield return AddUserRoleAsync(roleId, assignRoleRequest, callback);
            }
            else
            {
                callback.TryError(userMapResult.Error.Code);
            }
        }

        private IEnumerator AddUserRoleAsync(string roleId, AssignUserV4Request assignRoleRequest, ResultCallback<AssignUserV4Response> callback)
        {

            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(tokenResult => getAccessTokenResult = tokenResult);
            yield return new WaitUntil(() => getAccessTokenResult != null);

            var request = HttpRequestBuilder
                .CreatePost(this.config.BaseUrl + "/iam/v4/admin/roles/{roleId}/users")
                .WithPathParam("roleId", roleId)
                .WithBearerAuth(getAccessTokenResult.Value.access_token)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(assignRoleRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<AssignUserV4Response>();
            callback.Try(result);
        }

        private IEnumerator GetUserVerificationCodeAsync(string userId, string accessToken,
            ResultCallback<UserVerificationCode> callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return ClientLogin(result => clientLoginResult = result);

            Result<UserMapResponse> userMapResult = null;

            yield return GetUserMapping(userId, clientLoginResult.Value.access_token, result => userMapResult = result);

            yield return GetVerificationCodeAsync(
                userMapResult.Value.Namespace,
                userMapResult.Value.UserId,
                clientLoginResult.Value.access_token,
                callback);
        }

        public IEnumerator CreateMatchmakingChannelAsync(string accessToken, string channel, bool joinable, RuleSet ruleSet, ResultCallback callback)
        {
            var requestBody = new CreateChannelRequest
            {
                description = "joinableTest " + (joinable?"joinable":"nonJoinable"),
                find_match_timeout_seconds = 30,
                game_mode = channel,
                joinable = joinable,
                rule_set = ruleSet ?? new RuleSet
                {
                    alliance = new AllianceRule
                    {
                        min_number = 1,
                        max_number = 2,
                        player_min_number = 1,
                        player_max_number = 2
                    }
                }
            };

            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.BaseUrl + "/matchmaking/namespaces/{namespace}/channels")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        public IEnumerator DeleteMatchmakingChannelAsync(string accessToken, string channel, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.config.BaseUrl + "/matchmaking/namespaces/{namespace}/channels/{channel}")
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
            IHttpRequest request = HttpRequestBuilder.CreatePost(this.config.IamServerUrl + "/v3/oauth/token")
                .WithBasicAuth(this.config.ClientId, this.config.ClientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        private IEnumerator Delete(string namespace_, string userId, string clientAccessToken, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.config.IamServerUrl + "/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(clientAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator DeleteUserProfile(string namespace_, string userId, string clientAccessToken,
            ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.config.BasicServerUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", namespace_)
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
                    this.config.IamServerUrl +
                    "/namespaces/{namespace}/users/{userId}/platforms/justice/{targetNamespace}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("targetNamespace", this.config.PublisherNamespace)
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
            yield return DeleteAsync(user.Session.UserId, callback);
        }

        private IEnumerator DeleteAsync(string userId, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return ClientLogin(result => clientLoginResult = result);

            Result<UserMapResponse> userMapResult = null;

            yield return GetUserMapping(
                userId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);

            if (!userMapResult.IsError)
            {
                yield return Delete(
                    userMapResult.Value.Namespace,
                    userMapResult.Value.UserId,
                    clientLoginResult.Value.access_token,
                    callback);
            }
            else
            {
                callback.TryOk();
            }
        }

        private IEnumerator DeleteAsync(PlatformType platformType, string platformToken, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return this.ClientLogin(result => { clientLoginResult = result; });

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
                userResult.Value.userId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);

            yield return Delete(
                userMapResult.Value.Namespace,
                userMapResult.Value.UserId,
                clientLoginResult.Value.access_token,
                callback);
        }

        private IEnumerator RegisterAsync(RegisterUserRequest registerUserRequest,
            ResultCallback<RegisterUserResponse> callback)
        {
            Result<RegisterUserResponse> regResult = null;
            yield return this.userAccount.Register(registerUserRequest, res => regResult = res);
            callback.Try(regResult);
        }

        private IEnumerator LoginThenGetUserDataAsync(string username, string password, ResultCallback<UserData> callback)
        {
            Result loginResult = null;
            yield return this.loginSession.LoginWithUsername(username, password, result => loginResult = result, false);

            Result<UserData> userResult = null;
            yield return this.userAccount.GetData(r => userResult = r);
            callback.Try(userResult);
        }

        private IEnumerator GetUserMapAsync(string userId, ResultCallback<UserMapResponse> callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return ClientLogin(result => clientLoginResult = result);

            Result<UserMapResponse> userMapResult = null;

            yield return GetUserMapping(
                userId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);

            callback.Try(userMapResult);
        }

        private IEnumerator DeleteUserByDisplayNameAsync(string displayName, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return this.ClientLogin(result => { clientLoginResult = result; });

            Result loginResult = null;

            yield return this.loginSession.LoginWithDeviceId(result => loginResult = result);

            Result<PagedPublicUsersInfo> usersInfo = null;

            yield return this.userAccount.SearchUsers(displayName, SearchType.DISPLAYNAME, result => usersInfo = result);

            if (usersInfo.Value.data.Length == 0)
            {
                callback.TryOk();
                
                yield break;
            }

            foreach (var data in usersInfo.Value.data)
            {
                Result<UserMapResponse> userMapResult = null;

                yield return GetUserMapping(
                data.userId,
                clientLoginResult.Value.access_token,
                result => userMapResult = result);

                if (!userMapResult.IsError)
                {
                    yield return Delete(
                        userMapResult.Value.Namespace,
                        userMapResult.Value.UserId,
                        clientLoginResult.Value.access_token,
                        callback);
                }
                else
                {
                    callback.TryOk();
                }
            }
        }

        private IEnumerator DeleteUserProfileAsync(User user, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return ClientLogin(result => clientLoginResult = result);

            yield return DeleteUserProfile(
                AccelBytePlugin.Config.Namespace,
                user.Session.UserId,
                clientLoginResult.Value.access_token,
                callback);
        }

        // simulate login in launcher (simplified)
        public IEnumerator LoginInLauncherAsync(string username, string password, ResultCallback<string> callback)
        {
            // need to use client from publisher namespace
            var request = HttpRequestBuilder.CreatePost(this.config.IamServerUrl + "/oauth/token")
                .WithBasicAuth(this.config.ClientId, this.config.ClientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", this.config.PublisherNamespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (result.IsError)
            {
                callback.TryError(result.Error);
            }
            else
            {
                callback.TryOk(result.Value?.access_token);
            }
        }

        private IEnumerator GetAuthCodeAsync(string userAccessToken,
            ResultCallback<string> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.IamServerUrl + "/oauth/authorize")
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationForm)
                .WithFormParam("response_type", "code")
                .WithFormParam("client_id", AccelBytePlugin.Config.ClientId)
                .WithFormParam("redirect_uri", AccelBytePlugin.Config.RedirectUri)
                .GetResult()
                .GetUnityWebRequest();

            request.redirectLimit = 0;

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();

            if (result.IsError && (int)result.Error.Code != 302)
            {
                callback.TryError(result.Error);
            }
            else
            {
                string locationHeader = request.GetResponseHeader("Location");

                Match match = Regex.Match(locationHeader, "code=([^&]*)");
                if (match.Success)
                {
                    callback.TryOk(match.Groups[1].Value);
                }
                else
                {
                    callback.TryError(ErrorCode.InvalidResponse);
                }
            }
        }

        public void SendNotification(string userId, bool isAsync, string message, ResultCallback callback)
        {
            this.coroutineRunner.Run(SendNotificationAsync(userId, isAsync, message, callback));
        }

        private IEnumerator SendNotificationAsync(string userId, bool isAsync, string message, ResultCallback callback)
        {
            Result<TokenData> clientLoginResult = null;

            yield return this.ClientLogin(clientResult => clientLoginResult = clientResult);

            string body = string.Format("{{\"message\": \"{0}\",\"topic\": \"none\" }}", message);

            string url = AccelBytePlugin.Config.LobbyServerUrl.Replace("wss", "https") +
                "/notification/namespaces/{namespace}/users/{userId}/freeform";

            url = url.Replace("/lobby/", "");
            UnityWebRequest request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("targetNamespace", this.config.PublisherNamespace)
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

            yield return this.ClientLogin(clientResult => clientLoginResult = clientResult);

            callback.Try(clientLoginResult);
        }
        
        private IEnumerator GetSuperUserAccessTokenAsync(ResultCallback<TokenData> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(this.config.IamServerUrl + "/oauth/token")
                .WithBasicAuth(this.config.ClientId, this.config.ClientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", this.superUserAdminUsername)
                .WithFormParam("password", this.superUserAdminPassword)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        private IEnumerator CreateCurrencyAsync(string accessToken, CurrencyCreateModel body,
            ResultCallback<CurrencyInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/currencies")
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

        private IEnumerator GetPublishedStoreAsync(string namespace_, string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores/published")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator CreateStoreAsync(string namespace_, string accessToken, StoreCreateModel body,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores")
                .WithPathParam("namespace", namespace_)
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

        private IEnumerator CloneStoreAsync(string namespace_, string accessToken, string sourceStoreId, string targetStoreId,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores/{storeId}/clone")
                .WithPathParam("namespace", namespace_)
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

        private IEnumerator RollbackStoreAsync(string accessToken, ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores/published/rollback")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator PublishStoreAsync(string namespace_, string accessToken, string sourceStoreId,
            ResultCallback<StoreInfoModel> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores/{storeId}/clone")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("storeId", sourceStoreId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StoreInfoModel> result = request.GetHttpResponse().TryParseJson<StoreInfoModel>();
            callback.Try(result);
        }

        private IEnumerator CreateCategoryAsync(string namespace_, string accessToken, string storeId, CategoryCreateModel body,
            ResultCallback<FullCategoryInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/categories")
                .WithPathParam("namespace", namespace_)
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

        private IEnumerator CreateItemAsync(string namespace_, string accessToken, string storeId, ItemCreateModel body,
            ResultCallback<FullItemInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/items")
                .WithPathParam("namespace", namespace_)
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

        private IEnumerator GetItemBySkuAsync(string namespace_, string accessToken, string storeId, string sku, bool activeOnly,
            ResultCallback<FullItemInfo> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateGet(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/items/bySku")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("storeId", storeId)
                .WithQueryParam("sku", sku)
                .WithQueryParam("activeOnly", activeOnly.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<FullItemInfo>();
            callback.Try(result);
        }

        private IEnumerator FulfillItemAsync(string namespace_, string accessToken, string userId, FulfillmentModel body,
            ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/users/{userId}/fulfillment")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBody(body.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        private IEnumerator CreditWalletAsync(string accessToken, string userId, string currencyCode,
            CreditRequestModel body, ResultCallback<WalletInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(
                    this.config.PlatformServerUrl +
                    "/admin/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/credit")
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
                .CreateDelete(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/categories/{categoryPath}")
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
                .CreateDelete(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/currencies/{currencyCode}")
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
                .CreateDelete(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores/{storeId}")
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
                .CreateDelete(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores/published")
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
                    this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/currencies/{currencyCode}/summary")
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

        private IEnumerator GetStoreListAsync(string accessToken, string namespace_, ResultCallback<StoreInfoModel[]> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateGet(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/stores")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StoreInfoModel[]>();
            callback.Try(result);
        }

        private IEnumerator GetStatConfigByStatCodeAsync(string statCode, string accessToken,
            ResultCallback<StatConfig> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.config.StatisticServerUrl + "/v1/admin/namespaces/{namespace}/stats/{statCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("statCode", statCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatConfig> result = request.GetHttpResponse().TryParseJson<StatConfig>();
            callback.Try(result);
        }

        private IEnumerator CreateStatConfigAsync(string accessToken, StatCreateModel body,
            ResultCallback<StatConfig> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.StatisticServerUrl + "/v1/admin/namespaces/{namespace}/stats")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatConfig> result = request.GetHttpResponse().TryParseJson<StatConfig>();
            callback.Try(result);
        }

        private IEnumerator CreateUserStatItemsAsync(string userId, string[] statCodes, string accessToken,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            string body = "[";

            for (int i = 0; i < statCodes.Length; i++)
            {
                body += string.Format(@"{{""statCode"":""{0}""}}", statCodes[i]);

                if (i < statCodes.Length - 1)
                {
                    body += ",";
                }
                else
                {
                    body += "]";
                }
            }

            statCodes.Aggregate("", (acc, curr) => acc + "," + string.Format(@"{{""statCode"":""{0}""}}", curr));

            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.StatisticServerUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(body)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatItemOperationResult[]> result =
                request.GetHttpResponse().TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        private IEnumerator DeleteStatItemAsync(string accessToken, string userId, string statCode,
            ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(
                    this.config.StatisticServerUrl +
                    "/v1/admin/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("statCode", statCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator GetVerificationCodeAsync(string Namespace, string userId, string accessToken,
            ResultCallback<UserVerificationCode> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.config.IamServerUrl + "/v3/admin/namespaces/{namespace}/users/{userId}/codes")
                .WithPathParam("namespace", Namespace)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<UserVerificationCode> result = request.GetHttpResponse().TryParseJson<UserVerificationCode>();
            callback.Try(result);
        }

        private IEnumerator CreateAchievementAsync(string accessToken, AchievementRequest achievementRequest,
            ResultCallback<AchievementResponse> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.AchievementServerUrl + "/v1/admin/namespaces/{namespace}/achievements")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(achievementRequest))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParseJson<AchievementResponse>();
            callback.Try(result);
        }

        private IEnumerator DeleteAchievementAsync(string accessToken, string achievementCode,
            ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.config.AchievementServerUrl + "/v1/admin/namespaces/{namespace}/achievements/{achievementCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("achievementCode", achievementCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator GetCampaignByNameAsync(string accessToken, string name,
            ResultCallback<CampaignPagingSlicedResult> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(
                    this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/campaigns")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithQueryParam("name", name)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CampaignPagingSlicedResult> result = request.GetHttpResponse().TryParseJson<CampaignPagingSlicedResult>();
            callback.Try(result);
        }

        private IEnumerator CreateCampaignAsync(string accessToken, CampaignCreateModel body,
            ResultCallback<CampaignInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/campaigns")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CampaignInfo> result = request.GetHttpResponse().TryParseJson<CampaignInfo>();
            callback.Try(result);
        }

        private IEnumerator UpdateCampaignAsync(string accessToken, string campaignId, CampaignUpdateModel body,
            ResultCallback<CampaignInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/campaigns/{campaignId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("campaignId", campaignId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CampaignInfo> result = request.GetHttpResponse().TryParseJson<CampaignInfo>();
            callback.Try(result);
        }

        private IEnumerator CreateCampaignCodesAsync(string accessToken, string campaignId, CampaignCodeCreateModel body,
            ResultCallback<CampaignCodeCreateResult> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/codes/campaigns/{campaignId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("campaignId", campaignId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CampaignCodeCreateResult> result = request.GetHttpResponse().TryParseJson<CampaignCodeCreateResult>();
            callback.Try(result);
        }

        private IEnumerator GetCampaignCodesAsync(string accessToken, string campaignId,
            ResultCallback<CodeInfoPagingSlicedResult> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(
                    this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/codes/campaigns/{campaignId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("campaignId", campaignId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CodeInfoPagingSlicedResult> result = request.GetHttpResponse().TryParseJson<CodeInfoPagingSlicedResult>();
            callback.Try(result);
        }

        private IEnumerator DisableCampaignCodesAsync(string accessToken, string code,
            ResultCallback<CodeInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(
                    this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/codes/{code}/disable")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("code", code)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<CodeInfo> result = request.GetHttpResponse().TryParseJson<CodeInfo>();
            callback.Try(result);
        }

        private IEnumerator CreateRewardAsync(string accessToken, RewardCreateModel body,
            ResultCallback<RewardInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/rewards")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<RewardInfo> result = request.GetHttpResponse().TryParseJson<RewardInfo>();
            callback.Try(result);
        }

        private IEnumerator QueryRewardsAsync(string accessToken, string eventTopic, int offset, int limit, string sortBy,
            ResultCallback<QueryRewardInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/rewards/byCriteria")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithQueryParam("eventTopic", eventTopic)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("sortBy", sortBy)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<QueryRewardInfo> result = request.GetHttpResponse().TryParseJson<QueryRewardInfo>();
            callback.Try(result);
        }

        private IEnumerator DeleteRewardAsync(string accessToken, string rewardid,
            ResultCallback<QueryRewardInfo> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/rewards/{rewardId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("rewardId", rewardid)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<QueryRewardInfo> result = request.GetHttpResponse().TryParseJson<QueryRewardInfo>();
            callback.Try(result);
        }

        #region DSM

        public void DsmDeleteServer(string podName, ResultCallback callback)
        {
            this.coroutineRunner.Run(DsmDeleteServerAsync(podName, callback));
        }

        private IEnumerator DsmDeleteServerAsync(string podName, ResultCallback callback)
        {
            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(tokenResult => getAccessTokenResult = tokenResult);
            yield return new WaitUntil(() => getAccessTokenResult != null);
            if (getAccessTokenResult.Error != null)
            {
                callback.TryError(getAccessTokenResult.Error.Code, getAccessTokenResult.Error.Message);
            }
            else
            {
                IHttpRequest request = HttpRequestBuilder.CreateDelete($"{this.config.BaseUrl}/dsmcontroller/admin/namespaces/{AccelByteServerPlugin.Config.Namespace}/servers/local/{podName}")
                    .WithBearerAuth(getAccessTokenResult.Value.access_token)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();
    
                IHttpResponse response = null;
    
                yield return this.httpClient.SendRequest(request, rsp => response = rsp);
    
                var result = response.TryParse();
                callback.Try(result);
            }
        }

        public void GetDsmConfig(string @namespace, ResultCallback<DSMConfig> callback)
        {
            this.coroutineRunner.Run(GetDsmConfigAsync(@namespace, callback));
        }

        private IEnumerator GetDsmConfigAsync(string @namespace, ResultCallback<DSMConfig> callback)
        {
            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(tokenResult => getAccessTokenResult = tokenResult);
            yield return new WaitUntil(() => getAccessTokenResult != null);
            if (getAccessTokenResult.Error != null)
            {
                callback.TryError(getAccessTokenResult.Error);
            }
            else
            {
                IHttpRequest request = HttpRequestBuilder.CreateGet($"{this.config.BaseUrl}/dsmcontroller/admin/namespaces/{@namespace}/configs")
                    .WithBearerAuth(getAccessTokenResult.Value.access_token)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();

                IHttpResponse response = null;

                yield return this.httpClient.SendRequest(request, rsp => response = rsp);

                var result = response.TryParseJson<DSMConfig>();

                callback.Try(result);
            }
        }

        public void SetDsmConfig(DSMConfig body, ResultCallback callback)
        {
            this.coroutineRunner.Run(SetDsmConfigAsync(body, callback));
        }

        private IEnumerator SetDsmConfigAsync(DSMConfig body, ResultCallback callback)
        {
            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(tokenResult => getAccessTokenResult = tokenResult);
            yield return new WaitUntil(() => getAccessTokenResult != null);
            if (getAccessTokenResult.Error != null)
            {
                callback.TryError(getAccessTokenResult.Error);
            }
            else
            {
                IHttpRequest request = HttpRequestBuilder.CreatePost($"{this.config.BaseUrl}/dsmcontroller/admin/configs")
                    .WithBearerAuth(getAccessTokenResult.Value.access_token)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(body.ToJsonString())
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();

                IHttpResponse response = null;

                yield return this.httpClient.SendRequest(request, rsp => response = rsp);

                var result = response.TryParse();
                callback.Try(result);
            }
        }

        #endregion

        #region Leaderboard
        private IEnumerator LeaderboardCreateLeaderboardAsync(LeaderboardConfigRequest body,
            ResultCallback<LeaderboardConfigRequest> callback)
        {
            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(result => { getAccessTokenResult = result; });
            yield return new WaitUntil(() => getAccessTokenResult != null);

            if (getAccessTokenResult.IsError)
            {
                Debug.LogError("GetSuperUserAccessToken failed");
                callback.TryError(getAccessTokenResult.Error.Code);
                yield break;
            }

            Result<LeaderboardConfigRequest> responseResult = null;

            yield return LeaderboardCreateLeaderboardApi(
                getAccessTokenResult.Value.access_token,
                body,
                result => { responseResult = result; });

            callback.TryOk<LeaderboardConfigRequest>(responseResult.Value);
        }

        private IEnumerator LeaderboardCreateLeaderboardApi(string accessToken, LeaderboardConfigRequest body,
            ResultCallback<LeaderboardConfigRequest> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(
                    this.config.LeaderboardServerUrl +
                    "/v1/admin/namespaces/{namespace}/leaderboards")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<LeaderboardConfigRequest> result = request.GetHttpResponse().TryParseJson<LeaderboardConfigRequest>();
            callback.Try(result);
        }

        private IEnumerator LeaderboardGetAllLeaderboardAsync(string accessToken, string leaderboardCode,
            ResultCallback<LeaderboardConfigResponse> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(
                    this.config.LeaderboardServerUrl +
                    "/v1/admin/namespaces/{namespace}/leaderboards/{leaderboardCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<LeaderboardConfigResponse> result = request.GetHttpResponse().TryParseJson<LeaderboardConfigResponse>();
            callback.Try(result);
        }

        private IEnumerator LeaderboardDeleteLeaderboardAsync(string leaderboardCode,
            ResultCallback callback)
        {
            Result<TokenData> getAccessTokenResult = null;
            GetSuperUserAccessToken(result => { getAccessTokenResult = result; });
            yield return new WaitUntil(() => getAccessTokenResult != null);

            Result deleteResult = null;
            
            yield return LeaderboardDeleteLeaderboardApi(
                getAccessTokenResult.Value.access_token,
                leaderboardCode, 
                result => { deleteResult = result; });
            yield return new WaitUntil(() => deleteResult != null);
            
            callback.TryOk();
        }

        private IEnumerator LeaderboardDeleteLeaderboardApi(string accessToken, string leaderboardCode,
            ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(
                    this.config.LeaderboardServerUrl +
                    "/v1/admin/namespaces/{namespace}/leaderboards/{leaderboardCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }
        #endregion Leaderboard

        #region Group
        private IEnumerator CreateDefaultGroupConfigAsync(string accessToken, ResultCallback<CreateGroupConfigResponse> callback)
        {
            UnityWebRequest request = HttpRequestBuilder.CreatePost(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/configuration/initiate")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParseJson<CreateGroupConfigResponse>();
            callback.Try(result);
        }

        private IEnumerator GetGroupConfigAsync(string accessToken, ResultCallback<PaginatedGroupConfig> callback)
        {
            UnityWebRequest request = HttpRequestBuilder.CreateGet(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/configuration")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParseJson<PaginatedGroupConfig>();
            callback.Try(result);
        }

        private IEnumerator DeleteGroupConfigAsync(string accessToken, string namespace_, string configurationCode, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder.CreateDelete(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/configuration/{configurationCode}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("configurationCode", configurationCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator AdminSearchGroupsAsync(string accessToken,
            ResultCallback<PaginatedGroupListResponse> callback, string groupName = "", int limit = 0, int offset = 0)
        {
            var request = HttpRequestBuilder
                .CreateGet(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/groups")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithQueryParam("groupName", groupName)
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGroupListResponse>();
            callback.Try(result);
        }

        private IEnumerator AdminDeleteGroupAsync(string accessToken, string groupId,
            ResultCallback callback)
        {
            var request = HttpRequestBuilder
                .CreateDelete(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/groups/{groupId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("groupId", groupId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        private IEnumerator CreateMemberRoleAsync(string accessToken, CreateMemberRoleRequest createMemberRoleRequest, ResultCallback<CreateMemberRoleResponse> callback)
        {
            var request = HttpRequestBuilder
                .CreatePost(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/roles")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(createMemberRoleRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CreateMemberRoleResponse>();
            callback.Try(result);
        }

        private IEnumerator DeleteMemberRoleAsync(string accessToken, string memberRoleId, ResultCallback callback)
        {
            var request = HttpRequestBuilder
                .CreateDelete(this.config.GroupServerUrl + "/v1/admin/namespaces/{namespace}/roles/{memberRoleId}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("memberRoleId", memberRoleId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
        #endregion Group

        #region Agreement
        private IEnumerator AgreementCreateBasePolicyAsync(string accessToken, AgreementBasePolicyCreate body, 
            ResultCallback<AgreementBasePolicy> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.config.AgreementServerUrl + "/admin/base-policies")
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementBasePolicy> result = request.GetHttpResponse().TryParseJson<AgreementBasePolicy>();
            callback.Try(result);
        }

        private IEnumerator AgreementCreatePolicyVersionAsync(string accessToken, string policyId, 
            AgreementPolicyVersionCreate body, ResultCallback<AgreementPolicyVersion> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
            .CreatePost(this.config.AgreementServerUrl + "/admin/policies/{policyId}/versions")
            .WithPathParam("policyId", policyId)
            .WithContentType(MediaType.ApplicationJson)
            .WithBody(JsonSerializer.Serialize(body))
            .WithBearerAuth(accessToken)
            .Accepts(MediaType.ApplicationJson)
            .GetResult()
            .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementPolicyVersion> result = request.GetHttpResponse().TryParseJson<AgreementPolicyVersion>();
            callback.Try(result);
        }

        private IEnumerator AgreementCreateLocalizedPolicyAsync(string accessToken, string policyVersionId, 
            AgreementLocalizedPolicyCreate body, ResultCallback<AgreementLocalizedPolicy> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
            .CreatePost(this.config.AgreementServerUrl + "/admin/localized-policy-versions/versions/{policyVersionId}")
            .WithPathParam("policyVersionId", policyVersionId)
            .WithContentType(MediaType.ApplicationJson)
            .WithBody(JsonSerializer.Serialize(body))
            .WithBearerAuth(accessToken)
            .Accepts(MediaType.ApplicationJson)
            .GetResult()
            .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementLocalizedPolicy> result = request.GetHttpResponse().TryParseJson<AgreementLocalizedPolicy>();
            callback.Try(result);
        }
    
        private IEnumerator AgreementPublishPolicyVersionAsync(string accessToken, string policyVersionId, 
            bool shouldNotify, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
           .CreatePatch(this.config.AgreementServerUrl + "/admin/policies/versions/{policyVersionId}/latest")
           .WithPathParam("policyVersionId", policyVersionId)
           .WithQueryParam("shouldNotify", shouldNotify.ToString())
           .WithContentType(MediaType.ApplicationJson)
           .WithBearerAuth(accessToken)
           .Accepts(MediaType.ApplicationJson)
           .GetResult()
           .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        private IEnumerator AgreementGetBasePoliciesAsync(string accessToken, 
            ResultCallback<AgreementBasePolicy[]> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
            .CreateGet(this.config.AgreementServerUrl + "/admin/base-policies")
            .WithContentType(MediaType.ApplicationJson)
            .WithBearerAuth(accessToken)
            .Accepts(MediaType.ApplicationJson)
            .GetResult()
            .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementBasePolicy[]> result = request.GetHttpResponse().TryParseJson<AgreementBasePolicy[]>();
            callback.Try(result);
        }

        private IEnumerator AgreementGetCountryBasePolicyAsync(string accessToken, string policyId, string countryCode,
            ResultCallback<AgreementCountryPolicy> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
            .CreateGet(this.config.AgreementServerUrl + "/admin/base-policies/{policyId}/countries/{countryCode}")
            .WithPathParam("policyId", policyId)
            .WithPathParam("countryCode", countryCode)
            .WithContentType(MediaType.ApplicationJson)
            .WithBearerAuth(accessToken)
            .Accepts(MediaType.ApplicationJson)
            .GetResult()
            .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementCountryPolicy> result = request.GetHttpResponse().TryParseJson<AgreementCountryPolicy>();
            callback.Try(result);
        }

        private IEnumerator AgreementGetPolicyTypesAsync(string accessToken, 
            ResultCallback<AgreementPolicyTypeObject[]> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
            .CreateGet(this.config.AgreementServerUrl + "/admin/policy-types?limit=100")
            .WithContentType(MediaType.ApplicationJson)
            .WithBearerAuth(accessToken)
            .Accepts(MediaType.ApplicationJson)
            .GetResult()
            .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementPolicyTypeObject[]> result = request.GetHttpResponse().TryParseJson<AgreementPolicyTypeObject[]>();
            callback.Try(result);
        }

        private IEnumerator AgreementGetLocalizedPoliciesAsync(string accessToken, string policyVersionId, 
            ResultCallback<AgreementLocalizedPolicy[]> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
            .CreateGet(this.config.AgreementServerUrl + "/admin/localized-policy-versions/versions/{policyVersionId}")
            .WithContentType(MediaType.ApplicationJson)
            .WithPathParam("policyVersionId", policyVersionId)
            .WithBearerAuth(accessToken)
            .Accepts(MediaType.ApplicationJson)
            .GetResult()
            .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<AgreementLocalizedPolicy[]> result = request.GetHttpResponse().TryParseJson<AgreementLocalizedPolicy[]>();
            callback.Try(result);
        }
        #endregion Agreement

        #region Profanity

        // create filter list
        public IEnumerator CreateProfanityFilterListAsync(string accessToken, bool isEnabled, bool isMandatory, string name,
            ResultCallback callback)
        {
            AdminCreateProfanityListRequest body = new AdminCreateProfanityListRequest()
            {
                isEnabled = isEnabled,
                isMandatory = isMandatory,
                name = name
            };

            var request = HttpRequestBuilder
                .CreatePost(this.config.BaseUrl + "/lobby/v1/admin/profanity/namespaces/{namespace}/lists")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .GetResult();

            var unityWebRequest = request.GetUnityWebRequest();

            yield return SendAndLogRequest(request, unityWebRequest);

            var result = unityWebRequest.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        public class BypassCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                //Simply return true no matter what
                return true;
            }
        }

        public IEnumerator SetProfanityFilterRuleAsync(string accessToken, ProfanityNamespaceRule rule,
            ResultCallback callback)
        {
            AdminSetProfanityRuleForNamespaceRequest body = new AdminSetProfanityRuleForNamespaceRequest()
            {
                rule = rule.ToString()
            };

            var request = HttpRequestBuilder
                .CreatePost(this.config.BaseUrl + "/lobby/v1/admin/profanity/namespaces/{namespace}/rule")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBody(JsonSerializer.Serialize(body))
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .GetResult();

            var unityWebRequest = request.GetUnityWebRequest();

            yield return SendAndLogRequest(request, unityWebRequest);

            var result = unityWebRequest.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        public IEnumerator GetProfanityFilterRuleAsync(string accessToken, 
            ResultCallback<AdminSetProfanityRuleForNamespaceRequest> callback)
        {
            var request = HttpRequestBuilder
                .CreateGet(this.config.BaseUrl + "/lobby/v1/admin/profanity/namespaces/{namespace}/rule")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .GetResult();

            var unityWebRequest = request.GetUnityWebRequest();

            yield return SendAndLogRequest(request, unityWebRequest);

            var result = unityWebRequest.GetHttpResponse().TryParseJson<AdminSetProfanityRuleForNamespaceRequest>();
            callback.Try(result);
        }

        // insert bulk to filter list
        public IEnumerator AddFilterIntoListBulkAsync(string accessToken, string listName, string[] filters,
            ResultCallback callback)
        {
            AdminAddProfanityFiltersRequest body = new AdminAddProfanityFiltersRequest();
            body.filters = new AdminAddProfanityFilterIntoListRequest[filters.Length];
            for(int i = 0; i < body.filters.Length; i++)
            {
                body.filters[i] = new AdminAddProfanityFilterIntoListRequest()
                {
                    filter = filters[i],
                    note = ""
                };
            }

            var request = HttpRequestBuilder
                .CreatePost(this.config.BaseUrl + "/lobby/v1/admin/profanity/namespaces/{namespace}/list/{list}/filters/bulk")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("list", listName)
                .WithBearerAuth(accessToken)
                .WithBody(JsonSerializer.Serialize(body))
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var unityWebRequest = request.GetUnityWebRequest();

            yield return SendAndLogRequest(request, unityWebRequest);

            var result = unityWebRequest.GetHttpResponse().TryParse();
            callback.Try(result);
        }

        // delete filter list (with all filter included)
        public IEnumerator DeleteProfanityFilterListAsync(string accessToken, string listName,
             ResultCallback callback)
        {
            var request = HttpRequestBuilder
                .CreateDelete(this.config.BaseUrl + "/lobby/v1/admin/profanity/namespaces/{namespace}/lists/{list}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("list", listName)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var unityWebRequest = request.GetUnityWebRequest();

            yield return SendAndLogRequest(request, unityWebRequest);

            var result = unityWebRequest.GetHttpResponse().TryParse();

            callback.Try(result);
        }
        #endregion

        #region Subscripption
        private IEnumerator FreeSubscribeByPlatformAsync(string namespace_, string accessToken, string userId, FreeSubscritptionRequest body,
            ResultCallback<FullItemInfo> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.PlatformServerUrl + "/admin/namespaces/{namespace}/users/{userId}/subscriptions/platformSubscribe")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(body))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<FullItemInfo>();
            callback.Try(result);
        }
        #endregion

        #region Reporting
        public IEnumerator ReportingAdminGetReasonsAsync(string namespace_, string accessToken, string reasonGroup, ResultCallback<ReportingAdminReasonsResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateGet(this.config.ReportingServerUrl + "/v1/admin/namespaces/{namespace}/reasons")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("group", reasonGroup)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ReportingAdminReasonsResponse>();
            callback.Try(result);
        }

        public IEnumerator ReportingAddReasonAsync(string namespace_, string accessToken, ReportingAddReasonRequest reasonRequest, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.ReportingServerUrl + "/v1/admin/namespaces/{namespace}/reasons")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(reasonRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReportingDeleteReasonAsync(string namespace_, string accessToken, string reasonId, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.ReportingServerUrl + "/v1/admin/namespaces/{namespace}/reasons/{reasonId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("reasonId", reasonId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReportingAddReasonGroupAsync(string namespace_, string accessToken, ReportingAddReasonGroupRequest reasonGroupRequest, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.ReportingServerUrl + "/v1/admin/namespaces/{namespace}/reasonGroups")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(reasonGroupRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReportingAdminGetReasonGroupsAsync(string namespace_, string accessToken, ResultCallback<ReportingReasonGroupsResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateGet(this.config.ReportingServerUrl + "/v1/admin/namespaces/{namespace}/reasonGroups")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ReportingReasonGroupsResponse>();
            callback.Try(result);
        }

        public IEnumerator ReportingDeleteReasonGroupAsync(string namespace_, string accessToken, string reasonGroupId, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.ReportingServerUrl + "/v1/admin/namespaces/{namespace}/reasonGroups/{reasonGroupId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("reasonGroupId", reasonGroupId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        #endregion

        public enum Enviroment
        {
            UNKNOWN,
            DEV,
            DEMO
        }

        [DataContract]
        public class RedeemableItem
        {
            [DataMember] public string itemId;
            [DataMember] public string itemName;
            [DataMember] public int quantity;
        }

        [DataContract]
        public class CampaignCreateModel
        {
            [DataMember] public string name { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE/ INACTIVE
            [DataMember] public int maxRedeemCountPerCode { get; set; }
            [DataMember] public int maxRedeemCountPerCodePerUser { get; set; }
            [DataMember] public int maxRedeemCountPerCampaignPerUser { get; set; }
            [DataMember] public int maxSaleCount { get; set; }
            [DataMember] public DateTime redeemStart { get; set; }
            [DataMember] public DateTime redeemEnd { get; set; }
            [DataMember] public string redeemType { get; set; } // ITEM
            [DataMember] public RedeemableItem[] items { get; set; }
        }

        [DataContract]
        public class CampaignInfo
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string type { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE/ INACTIVE
            [DataMember] public DateTime redeemStart { get; set; }
            [DataMember] public DateTime redeemEnd { get; set; }
            [DataMember] public int maxRedeemCountPerCode { get; set; }
            [DataMember] public int maxRedeemCountPerCodePerUser { get; set; }
            [DataMember] public int maxRedeemCountPerCampaignPerUser { get; set; }
            [DataMember] public int maxSaleCount { get; set; }
            [DataMember] public string redeemType { get; set; } // ITEM
            [DataMember] public RedeemableItem[] items { get; set; }
            [DataMember] public string boothName { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class CampaignPagingSlicedResult
        {
            [DataMember] public CampaignInfo[] data { get; set; }
            [DataMember] public Paging paging { get; set; }
        }

        [DataContract]
        public class CampaignUpdateModel
        {
            [DataMember] public string name { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE/ INACTIVE
            [DataMember] public int maxRedeemCountPerCode { get; set; }
            [DataMember] public int maxRedeemCountPerCodePerUser { get; set; }
            [DataMember] public int maxRedeemCountPerCampaignPerUser { get; set; }
            [DataMember] public int maxSaleCount { get; set; }
            [DataMember] public DateTime redeemStart { get; set; }
            [DataMember] public DateTime redeemEnd { get; set; }
            [DataMember] public string redeemType { get; set; } // ITEM
            [DataMember] public RedeemableItem[] items { get; set; }
        }

        [DataContract]
        public class CampaignCodeCreateModel
        {
            [DataMember] public int quantity { get; set; }
        }

        [DataContract]
        public class CampaignCodeCreateResult
        {
            [DataMember] public int numCreated { get; set; }
        }

        [DataContract]
        public class CodeInfo
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string type { get; set; } // REDEMPTION
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string campaignId { get; set; }
            [DataMember] public string value { get; set; }
            [DataMember] public int maxRedeemCountPerCode { get; set; }
            [DataMember] public int maxRedeemCountPerCodePerUser { get; set; }
            [DataMember] public int maxRedeemCountPerCampaignPerUser { get; set; }
            [DataMember] public int remainder { get; set; }
            [DataMember] public int redeemedCount { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE / INACTIVE
            [DataMember] public DateTime redeemStart { get; set; }
            [DataMember] public DateTime redeemEnd { get; set; }
            [DataMember] public string redeemType { get; set; } // ITEM
            [DataMember] public RedeemableItem[] items { get; set; }
            [DataMember] public int batchNo { get; set; }
            [DataMember] public string acquireOrderNo { get; set; }
            [DataMember] public string acquireUserId { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class CodeInfoPagingSlicedResult
        {
            [DataMember] public CodeInfo[] data { get; set; }
            [DataMember] public Paging paging { get; set; }
        }

        [DataContract]
        public class RewardItem
        {
            [DataMember] public string itemId { get; set; }
            [DataMember] public int quantity { get; set; }
        }

        [DataContract]
        public class RewardCondition
        {
            [DataMember] public string conditionName { get; set; }
            [DataMember] public string condition { get; set; }
            [DataMember] public string eventName { get; set; }
            [DataMember] public RewardItem[] rewardItems { get; set; }
        }

        [DataContract]
        public class RewardCreateModel
        {
            [DataMember] public string rewardCode { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string eventTopic { get; set; }
            [DataMember] public RewardCondition[] rewardConditions { get; set; }
            [DataMember] public int maxAwarded { get; set; }
            [DataMember] public int maxAwardedPerUser { get; set; }
        }

        [DataContract]
        public class RewardInfo
        {
            [DataMember] public string rewardId { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string rewardCode { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string eventTopic { get; set; }
            [DataMember] public RewardCondition[] rewardConditions { get; set; }
            [DataMember] public int maxAwarded { get; set; }
            [DataMember] public int maxAwardedPerUser { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class QueryRewardPagingSlicedResult
        {
            [DataMember] public string previous { get; set; }
            [DataMember] public string next { get; set; }
        }

        [DataContract]
        public class QueryRewardInfo
        {
            [DataMember] public RewardInfo[] data { get; set; }
            [DataMember] public QueryRewardPagingSlicedResult paging { get; set; }
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
                Debug.LogError("FAILED TEST " + methodName + " LINE " + stackFrame.GetFileLineNumber() + ": " + ex.Message);

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
                Debug.Log("FAILED TEST " + methodName + " LINE " + stackFrame.GetFileLineNumber() + ": " + ex.Message);

                throw;
            }
        }

        [DataContract]
        public class AllianceRule
        {
            [DataMember] public int max_number { get; set; }
            [DataMember] public int min_number { get; set; }
            [DataMember] public int player_max_number { get; set; }
            [DataMember] public int player_min_number { get; set; }
        }

        [DataContract]
        public class FlexingRule
        {
            [DataMember] public string attribute { get; set; }
            [DataMember] public string criteria { get; set; }
            [DataMember] public int duration { get; set; }
            [DataMember] public int reference { get; set; }
        }

        public enum MatchingRuleCriteria
        {
            none,
            distance
        }

        [DataContract]
        public class MatchingRule
        {
            [DataMember] public string attribute { get; set; }
            [DataMember] public string criteria { get; set; }
            [DataMember] public int reference { get; set; }
        }

        [DataContract]
        public class RuleSet
        {
            [DataMember] public AllianceRule alliance { get; set; }
            [DataMember] public FlexingRule[] flexing_rule { get; set; }
            [DataMember] public MatchingRule[] matching_rule { get; set; }
        }

        [DataContract]
        public class CreateChannelRequest
        {
            [DataMember] public string description { get; set; }
            [DataMember] public uint find_match_timeout_seconds { get; set; }
            [DataMember] public string game_mode { get; set; }
            [DataMember] public RuleSet rule_set { get; set; }
            [DataMember] public bool joinable { get; set; }
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
        public class LocalExt
        {
            [DataMember] public string[] properties { get; set; }
            [DataMember] public string[] functions { get; set; }
        }

        [DataContract]
        public class RegionDataUS
        {
            [DataMember] public RegionDataItem[] US { get; set; }
        }

    [DataContract]
        public class ItemCreateModel
        {
            [DataMember] public string itemType { get; set; } // APP/COINS/INGAMEITEM/BUNDLE/PRODUCT/CODE/SUBSCRIPTION/SEASON
            [DataMember] public SeasonType seasonType { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public string entitlementType { get; set; } // DURABLE/CONSUMABLE
            [DataMember] public int useCount { get; set; }
            [DataMember] public bool stackable { get; set; }
            [DataMember] public string appId { get; set; }
            [DataMember] public string appType { get; set; } // GAME/SOFTWARE/DLC/DEMO
            [DataMember] public string baseAppId { get; set; } 
            [DataMember] public string targetCurrencyCode { get; set; }
            [DataMember] public string targetNamespace { get; set; }
            [DataMember] public string categoryPath { get; set; }
            [DataMember] public Dictionary<string, Localization> localizations { get; set; }
            [DataMember] public string status { get; set; } // ACTIVE/INACTIVE
            [DataMember] public string sku { get; set; }
            [DataMember] public Image[] images { get; set; }
            [DataMember] public string thumbnailUrl { get; set; }
            [DataMember] public Dictionary<string, RegionDataItem[]> regionData { get; set; }
            [DataMember] public string[] itemIds { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public int maxCountPerUser { get; set; }
            [DataMember] public int maxCount { get; set; }
            [DataMember] public string boothName { get; set; }
            [DataMember] public int displayOrder { get; set; }
            [DataMember] public string clazz { get; set; }
            [DataMember] public Recurring recurring { get; set; }

            public class RegionDatas
            {
                public RegionDataItem[] US;
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
                public string longDescription { get; set; }
            }

            public class Recurring
            {
                public string cycle; // WEEKLY/MONTHLY/QUARTERLY/YEARLY
                public int fixedFreeDays;
                public int fixedTrialCycles;
                public int graceDays;
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
            [DataMember] public string source { get; set; } // PURCHASE/PROMOTION/ACHIEVEMENT/REFERRAL_BONUS/REDEEM_CODE/OTHER
            [DataMember] public string reason { get; set; }
        }

        [DataContract]
        public class FulfillmentModel
        {
            [DataMember] public string itemId { get; set; }
            [DataMember] public int quantity { get; set; }
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
            [DataMember] public string[] tags { get; set; }
        }

        [DataContract]
        public class UserVerificationCode
        {
            [DataMember] public string accountRegistration { get; set; }
            [DataMember] public string accountUpgrade { get; set; }
            [DataMember] public string passwordReset { get; set; }
            [DataMember] public string updateEmail { get; set; }
        }

        [DataContract]
        public class AchievementRequest
        {
            [DataMember] public string achievementCode { get; set; }
            [DataMember] public string defaultLanguage { get; set; }
            [DataMember] public Dictionary<string, string> name { get; set; }
            [DataMember] public Dictionary<string, string> description { get; set; }
            [DataMember] public AchievementIcon[] lockedIcons { get; set; }
            [DataMember] public AchievementIcon[] unlockedIcons { get; set; }
            [DataMember] public bool hidden { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public bool incremental { get; set; }
            [DataMember] public float goalValue { get; set; }
            [DataMember] public string statCode { get; set; }
        }

        [DataContract]
        public class AchievementResponse
        {
            [DataMember] public string achievementCode { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string defaultLanguage { get; set; }
            [DataMember] public Dictionary<string, string> name { get; set; }
            [DataMember] public Dictionary<string, string> description { get; set; }
            [DataMember] public AchievementIcon[] lockedIcons { get; set; }
            [DataMember] public AchievementIcon[] unlockedIcons { get; set; }
            [DataMember] public bool hidden { get; set; }
            [DataMember] public int listOrder { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public bool incremental { get; set; }
            [DataMember] public float goalValue { get; set; }
            [DataMember] public string statCode { get; set; }
            [DataMember] public string createdAt { get; set; }
            [DataMember] public string updatedAt { get; set; }
        }

        [DataContract]
        public class ReportingAdminReasonItem
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string title { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string createdAt { get; set; }
            [DataMember] public string updatedAt { get; set; }
        }

        [DataContract]
        public class ReportingAdminReasonsResponse
        {
            [DataMember] public ReportingAdminReasonItem[] data { get; set; }
        }

        [DataContract]
        public class ReportingAddReasonRequest
        {
            [DataMember] public string title { get; set; }
            [DataMember] public string description { get; set; }
        }

        [DataContract]
        public class ReportingAddReasonGroupRequest
        {
            [DataMember] public string title { get; set; }
            [DataMember] public string[] reasonIds { get; set; }
        }

        #region Leaderboard
        [DataContract]
        public class LeaderboardDailyConfig
        {
            [DataMember] public string resetTime { get; set; } = "00:00";
        }

        [DataContract]
        public class LeaderboardMonthlyConfig
        {
            [DataMember] public int resetDate { get; set; } = 1;
            [DataMember] public string resetTime { get; set; } = "00:00";
        }

        [DataContract]
        public class LeaderboardWeeklyConfig
        {
            [DataMember] public int resetDay { get; set; } = 0;
            [DataMember] public string resetTime { get; set; } = "00:00";
        }

        [DataContract]
        public class LeaderboardConfigRequest
        {
            [DataMember] public LeaderboardDailyConfig daily { get; set; } = new LeaderboardDailyConfig();
            [DataMember] public LeaderboardMonthlyConfig monthly { get; set; } = new LeaderboardMonthlyConfig();
            [DataMember] public LeaderboardWeeklyConfig weekly { get; set; } = new LeaderboardWeeklyConfig();
            [DataMember] public bool descending { get; set; } = true;
            [DataMember] public string iconURL { get; set; } = "";
            [DataMember] public string leaderboardCode { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public int seasonPeriod { get; set; } = 32;
            [DataMember] public string startTime { get; set; }
            [DataMember] public string statCode { get; set; }
        }

        [DataContract]
        public class LeaderboardConfigResponse
        {
            [DataMember] public LeaderboardDailyConfig daily { get; set; } = new LeaderboardDailyConfig();
            [DataMember] public LeaderboardMonthlyConfig monthly { get; set; } = new LeaderboardMonthlyConfig();
            [DataMember] public LeaderboardWeeklyConfig weekly { get; set; } = new LeaderboardWeeklyConfig();
            [DataMember] public bool descending { get; set; } = true;
            [DataMember] public string iconURL { get; set; } = "";
            [DataMember] public string leaderboardCode { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public int seasonPeriod { get; set; } = 32;
            [DataMember] public string startTime { get; set; }
            [DataMember] public string statCode { get; set; }
        }
        #endregion Leaderboard

        #region Group
        [DataContract]
        public class CreateGroupConfigResponse
        {
            [DataMember] public string configurationCode { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public Rules[] globalRules { get; set; }
            [DataMember] public string groupAdminRoleId { get; set; }
            [DataMember] public int groupMaxMember { get; set; }
            [DataMember] public string groupMemberRoleId { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
        }

        [DataContract]
        public class PaginatedGroupConfig
        {
            [DataMember] public CreateGroupConfigResponse[] data { get; set; }
            [DataMember] public Paging paging { get; set; }
        }

        [DataContract]
        public class CreateMemberRoleRequest
        {
            [DataMember] public string memberRoleName { get; set; }
            [DataMember] public MemberRolePermission[] memberRolePermissions { get; set; }
        }

        [DataContract]
        public class CreateMemberRoleResponse
        {
            [DataMember] public string memberRoleId { get; set; }
            [DataMember] public string memberRoleName { get; set; }
            [DataMember] public MemberRolePermission[] memberRolePermissions { get; set; }
        }
        #endregion Group

        #region Agreement
        [DataContract]
        public class AgreementBasePolicyCreate
        {
            [DataMember(Name = "namespace")] public string namespace_ { get; set; }
            [DataMember] public string typeId { get; set; }
            [DataMember] public string basePolicyName { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string[] affectedCountries { get; set; }
            [DataMember] public string[] affectedClientIds { get; set; }
            [DataMember] public string[] tags { get; set; }
            [DataMember] public bool isMandatory { get; set; }
        }

        [DataContract]
        public class AgreementPolicyTypeObject
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string policyTypeName { get; set; }
            // contains more fields but unused for test
        }

        [DataContract]
        public class AgreementPolicyObject
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string countryCode { get; set; }
            [DataMember] public string policyName { get; set; }
            // contains more fields but unused for test
        }

        [DataContract]
        public class AgreementBasePolicy
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string basePolicyName { get; set; }
            [DataMember(Name = "namespace")] public string namespace_ { get; set; }
            [DataMember] public AgreementPolicyObject[] policies { get; set; }
            // contains more fields but unused for test
        }
        [DataContract]
        public class AgreementPolicyVersionCreate
        {
            [DataMember] public string displayVersion { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public bool isCrucial { get; set; }
            [DataMember] public bool isCommitted { get; set; }
        };
        [DataContract]
        public class AgreementPolicyVersion
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string displayVersion { get; set; }
            [DataMember] public string basePolicyId { get; set; }
            [DataMember] public bool isCrucial { get; set; }
            [DataMember] public bool isInEffect { get; set; }
            // contains more fields but unused for test
        };
        [DataContract]
        public class AgreementCountryPolicy
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string countryCode { get; set; }
            [DataMember] public string policyName { get; set; }
            [DataMember] public bool isCrucial { get; set; }
            [DataMember] public AgreementPolicyVersion[] policyVersions { get; set; }
            // contains more fields but unused for test
        };
        [DataContract]
        public class AgreementLocalizedPolicyCreate
        {
            [DataMember] public string localeCode { get; set; }
            [DataMember] public string contentType { get; set; }
            [DataMember] public string description { get; set; }
        };
        [DataContract]
        public class AgreementLocalizedPolicy
        {
            [DataMember] public string id { get; set; }
            [DataMember] public string localeCode { get; set; }
            [DataMember] public string contentType { get; set; }
            [DataMember] public string description { get; set; }
            [DataMember] public string attachmentLocation { get; set; }
        };
        #endregion Agreement

        #region Profanity
        [DataContract]
        public class AdminCreateProfanityListRequest
        {
            [DataMember] public bool isEnabled { get; set; }
            [DataMember] public bool isMandatory { get; set; }
            [DataMember] public string name { get; set; }
        }

        [DataContract]
        public class AdminAddProfanityFiltersRequest
        {
            [DataMember] public AdminAddProfanityFilterIntoListRequest[] filters { get; set; }
        }

        [DataContract]
        public class AdminAddProfanityFilterIntoListRequest
        {
            [DataMember] public string filter { get; set; }
            [DataMember] public string note { get; set; }
        }

        public enum ProfanityNamespaceRule
        {
            none,
            all,
            tail,
            middle
        }

        [DataContract]
        public class AdminSetProfanityRuleForNamespaceRequest
        {
            [DataMember] public string rule { get; set; } // none, all, tail, middle
        }
        #endregion

        #region Subscription
        [DataContract]
        public class FreeSubscritptionRequest
        {
            [DataMember] public string itemId;
            [DataMember] public int grantDays;
            [DataMember] public string source;
            [DataMember] public string reason;
            [DataMember] public string region;
            [DataMember] public string language;
        }
        #endregion

        #region DSM
        [DataContract]
        public class PodConfig
        {
            [DataMember] public int cpu_limit { get; set; }
            [DataMember] public int mem_limit { get; set; }
            [DataMember(Name ="params")] public string params_ { get; set; }
        }

        [DataContract]
        public class DeploymentConfig
        {
            [DataMember] public int buffer_count { get; set; }
            [DataMember] public string configuration { get; set; }
            [DataMember] public string game_version { get; set; }
            [DataMember] public int max_count { get; set; }
            [DataMember] public int min_count { get; set; }
            [DataMember] public string[] regions { get; set; }
        }

        [DataContract]
        public class DeploymentWithOverride
        {
            [DataMember] public bool allow_version_override { get; set; }
            [DataMember] public int buffer_count { get; set; }
            [DataMember] public string configuration { get; set; }
            [DataMember] public string game_version { get; set; }
            [DataMember] public int max_count { get; set; }
            [DataMember] public int min_count { get; set; }
            [DataMember] public Dictionary<string, DeploymentConfig> overrides { get; set; }
            [DataMember] public string[] regions { get; set; }
        }

        [DataContract]
        public class DSMConfig
        {
            [DataMember(Name = "namespace")] public string namespace_ { get; set; }
            [DataMember] public bool allow_version_override { get; set; }
            [DataMember] public int buffer_count { get; set; }
            [DataMember] public int claim_timeout { get; set; }
            [DataMember] public Dictionary<string, PodConfig> configurations { get; set; }
            [DataMember] public int cpu_limit { get; set; }
            [DataMember] public int mem_limit { get; set; }
            [DataMember(Name = "params")] public string params_ { get; set; }
            [DataMember] public int creation_timeout { get; set; }
            [DataMember] public string default_version { get; set; }
            [DataMember] public Dictionary<string, DeploymentWithOverride> deployments { get; set; }
            [DataMember] public int heartbeat_timeout { get; set; }
            [DataMember] public Dictionary<string, string> image_version_mapping { get; set; }
            [DataMember] public int max_count { get; set; }
            [DataMember] public int min_count { get; set; }
            [DataMember] public Dictionary<string, DeploymentConfig> overrides { get; set; }
            [DataMember] public int port { get; set; }
            [DataMember] public Dictionary<string, int> ports { get; set; }
            [DataMember] public string protocol { get; set; }
            [DataMember] public string[] providers { get; set; }
            [DataMember] public int session_timeout { get; set; }
            [DataMember] public int unreachable_timeout { get; set; }
            [DataMember] public Dictionary<string, int> version_image_size_mapping { get; set; }
        }

        #endregion

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
            
            public static void IsResultOk(IResult result, string message = "", params object[] args)
            {
                TestHelper.TryRun(() =>
                {
                    if (result == null)
                    {
                        NUnit.Framework.Assert.Fail($"{message}: result is null\n");
                    }
                    else
                    {
                        NUnit.Framework.Assert.IsTrue(!result.IsError, $"{message}: [{result?.Error?.Code}] {result?.Error?.Message}\n", args);
                    }
                });
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
