using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using AccelByte.Core;
using AccelByte.Api;
using System.Threading;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using AccelByte.Server;

namespace Tests.IntegrationTests
{
    public class SubscriptionTest
    {

        /***********************************************************************************
         * Setup needed to run test
         *
         * 1. create an item that have appId
         * 2. create an item that is a subscription
         * 3. Create User
         * 4. set user have entitlement to App Item (item that have appId)
         * 5. set user have entitlement to subscription item // this changes recently and user need to buy subscription manually. need to confirm how to do this via code
         *
         ***********************************************************************************/

        struct UserCreateData
        {
            public const string EmailAddress = "SubcriptionSDKTest@example.com";
            public const string Password = "Password+123";
            public const string CountryCode = "MG";
            public const string DisplayName = "Subs Test";
            public static string UserId;
        };

        private string[] usersEmail = new string[3];
        private string password = "Password+123";

        private const string appTypeItemSku = "sdktestSkuApp001";
        private const string subsTypeItemSku = "sdktestSkuSubs001";

        private string createdStoreId;
        private string publishedStoreId;
        private string accessToken;
        private string language;
        private string region;
        private string publisherNamespace;

        private string appTypeItemId;
        private string subsTypeItemId;

        private static TestHelper testHelper = new TestHelper();
        private static User user = AccelBytePlugin.GetUser();
        private static Entitlement entitlement = AccelBytePlugin.GetEntitlement();

        private const string APPID = "sdkTest0001";
        private string originalAppId;

        private string invalidPublicKey = "ThisIsInvalidPublicKey";
        // The public key is generated from https://dev.accelbyte.io/iam/apidocs/#/OAuth/GetJWKS
        private string devPublicKey = @"-----BEGIN PUBLIC KEY-----
                                MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyPXSSXIVvDDOUYv/mOUw
                                e8cstE1FfEVaQjBrDX9UQmDsA7gyMyYEp3+OPJpMVc3eQvUlE1FxhwXW+oIL0xsU
                                0AjssFfBhFsXBGk72Q2QCHLD9WvMZao319h1f4TCmnr5DcTHRwl86qlyLmvAN1Sh
                                fHZ6//d2XqD/gKRKqU3X2UdVBYM54/uJgblZKjD8aVnRKdhMyHHzyrF1qi0/nlnv
                                KfFYvfaoHgBK+JB+5W8gFhDuQ9c/wncI6/2NilSHiaMfcIvpUbHCL3f/e06bkyy5
                                EA8R3jQ63AD9oBICkVamp/rG2pWVUK9kE0HnRziX0phAoHealIKDtv8RYjon9z5V
                                yQIDAQAB
                                -----END PUBLIC KEY-----";
        // The public key is generated from https://dev.accelbyte.io/iam/apidocs/#/OAuth/GetJWKS
        private string demoPublicKey = @"-----BEGIN PUBLIC KEY-----
                                    MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA+u8A8klvZ6V4ba/JTEji
                                    IvhPy8uimJGAMfpWFrOntX3YUgoTM/RdPyRDQ8FdNEKNyyvQESObVoqx6ElcdFWv
                                    L3PN4huVDo5fUe8EK8jRhUe1tsjyswT14zV/a5LSgIi40VNFqGcgvwHI/Z2KOk86
                                    83LEMIBARq3pBDBPVoqW7EFknNsWLMpIR+d15qkCViht20bBCbP7hSZ7qPKMZHpl
                                    PC39zAYO8GjuOyWQfu5VrL5jvTdj0j2jK6d4tOvvXPLIAqfwvjFYgQFbDUVUDJb0
                                    LXs+aQ7Jos6EQImKqlk9bnB1neK3ZjpPuzsEkrQrnqz9+zfInD8aC/efFpkgWMOG
                                    2QIDAQAB
                                    -----END PUBLIC KEY-----";
        private string publicKey = null;

        [UnityTest, TestLog, Order(0), Timeout(100000)]
        public IEnumerator Setup()
        {
            if (testHelper.GetEnviroment() == TestHelper.Enviroment.DEV)
            {
                publicKey = devPublicKey;
            }
            if (testHelper.GetEnviroment() == TestHelper.Enviroment.DEMO)
            {
                publicKey = demoPublicKey;
            }

            TestHelper.Assert.That(publicKey, Is.Not.Null);

            originalAppId = AccelBytePlugin.Config.AppId;
            AccelBytePlugin.Config.AppId = APPID;

            publisherNamespace = "accelbyte";
            language = "en-US";
            region = "US";

            usersEmail[0] = "Subs1SDKTest@example.com";
            usersEmail[1] = "Subs2SDKTest@example.com";
            usersEmail[2] = "Subs3SDKTest@example.com";

            bool notHasAppItem = false;
            bool notHasSubsItem = false;

            Result loginServerResult = null;
            AccelByteServerPlugin.GetDedicatedServer().LoginWithClientCredentials(result => loginServerResult = result);
            yield return TestHelper.WaitForValue(() => loginServerResult);

            TestHelper.Assert.IsResultOk(loginServerResult, "Login Server");
            TestHelper.Assert.That(loginServerResult, Is.Not.Null);

            //Get access Token
            Result<TokenData> getAccessToken = null;
            testHelper.GetSuperUserAccessToken(result => { getAccessToken = result; });
            yield return TestHelper.WaitForValue(() => getAccessToken);

            TestHelper.Assert.IsResultOk(getAccessToken, "Get Access Token");
            TestHelper.Assert.That(getAccessToken.Value, Is.Not.Null);

            accessToken = getAccessToken.Value.access_token;

            //Get published store
            Result<TestHelper.StoreInfoModel> getPublishedStore = null;
            testHelper.GetPublishedStore(publisherNamespace, accessToken, res => getPublishedStore = res);
            yield return TestHelper.WaitForValue(() => getPublishedStore);

            TestHelper.LogResult(getPublishedStore, "Get Subs type Item");
            publishedStoreId = getPublishedStore.Value.storeId;

            //Try Get Active Item SKU
            Result<TestHelper.FullItemInfo> getAppTypeResult = null;
            testHelper.GetItemBySKU(publisherNamespace, accessToken, publishedStoreId, appTypeItemSku, true, result => getAppTypeResult = result);
            yield return TestHelper.WaitForValue(() => getAppTypeResult);

            TestHelper.LogResult(getAppTypeResult, "Get App type Item");
            if (getAppTypeResult.IsError)
            {
                notHasAppItem = true;
            }
            else
            {
                appTypeItemId = getAppTypeResult.Value.itemId;
            }

            Result<TestHelper.FullItemInfo> getSubsTypeResult = null;
            testHelper.GetItemBySKU(publisherNamespace, accessToken, publishedStoreId, subsTypeItemSku, true, result => getSubsTypeResult = result);
            yield return TestHelper.WaitForValue(() => getSubsTypeResult);

            TestHelper.LogResult(getSubsTypeResult, "Get Subs type Item");
            if (getSubsTypeResult.IsError)
            {
                notHasSubsItem = true;
            }
            else
            {
                subsTypeItemId = getSubsTypeResult.Value.itemId;
            }

            bool needToCreateItem = notHasAppItem || notHasSubsItem;

            if (needToCreateItem)
            {
                Result<TestHelper.StoreInfoModel[]> getStoresResult = null;
                testHelper.GetStoreList(accessToken, publisherNamespace, result => getStoresResult = result);
                yield return TestHelper.WaitForValue(() => getStoresResult);

                TestHelper.Assert.IsResultOk(getStoresResult, "Get Store");
                TestHelper.Assert.That(getStoresResult.Value, Is.Not.Null);

                int draftStoreNum = 0;
                for( int i = 0; i < getStoresResult.Value.Length; i++)
                {
                    draftStoreNum += getStoresResult.Value[i].published ? 0 : 1;
                }

                //Create temp store to accept clone
                TestHelper.StoreCreateModel temporaryStore = new TestHelper.StoreCreateModel
                {
                    title = "ACCELBYTE STORE",
                    description = "Created from Unity SDK Subscription Test",
                    supportedLanguages = new string[] { "en" },
                    supportedRegions = new string[] { "US" },
                    defaultLanguage = "en-US",
                    defaultRegion = "US"
                };
                if (draftStoreNum > 0)
                {
                    for (int i = 0; i < getStoresResult.Value.Length; i++)
                    {
                        if (getStoresResult.Value[i].title == temporaryStore.title)
                        {
                            createdStoreId = getStoresResult.Value[i].storeId;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(createdStoreId))
                    {
                        for (int i = 0; i < getStoresResult.Value.Length; i++)
                        {
                            if (getStoresResult.Value[i].published == false)
                            {
                                createdStoreId = getStoresResult.Value[i].storeId;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Result<TestHelper.StoreInfoModel> createStoreResult = null;
                    testHelper.CreateStore(publisherNamespace, accessToken, temporaryStore, result => createStoreResult = result);
                    yield return TestHelper.WaitForValue(() => createStoreResult);

                    TestHelper.Assert.IsResultOk(createStoreResult, "Create Store");
                    TestHelper.Assert.That(createStoreResult.Value, Is.Not.Null);

                    createdStoreId = createStoreResult.Value.storeId;

                    //Clone published store
                    Result<TestHelper.StoreInfoModel> cloneStoreResult = null;
                    testHelper.CloneStore(publisherNamespace, accessToken, publishedStoreId, createdStoreId, result => cloneStoreResult = result);
                    yield return TestHelper.WaitForValue(() => cloneStoreResult);

                    TestHelper.LogResult(cloneStoreResult, "Clone Published Store");
                    TestHelper.Assert.IsResultOk(cloneStoreResult, "Clone Published Store");
                    TestHelper.Assert.That(cloneStoreResult, Is.Not.Null);
                }

                RegionDataItem[] itemRegionData = new RegionDataItem[1];
                itemRegionData[0] = new RegionDataItem
                {
                    price = 0,
                    discountPercentage = 0,
                    discountAmount = 0,
                    discountedPrice = 0,
                    currencyCode = "USD",
                    currencyType = "REAL",
                    currencyNamespace = publisherNamespace,
                    purchaseAt = DateTime.UtcNow,
                    expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                    discountPurchaseAt = DateTime.UtcNow,
                    discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000)
                };
                TestHelper.ItemCreateModel.Localization itemLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "SDK Item",
                    description = "SDK Item, real currency, USD."
                };

                if (notHasAppItem)
                {
                    //Create an Item, itemType = "App"
                    TestHelper.ItemCreateModel itemData = new TestHelper.ItemCreateModel
                    {
                        itemType = "APP",
                        appId = APPID,
                        name = "SDK Test Game",
                        appType = "GAME",
                        categoryPath = "/game",
                        entitlementType = "DURABLE",
                        sku = appTypeItemSku,
                        status = "ACTIVE",
                        regionData = new Dictionary<string, RegionDataItem[]>(),
                        localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>()
                    };
                    itemData.regionData.Add("US", itemRegionData);
                    itemData.localizations.Add("en-US", itemLocalization);

                    Result<TestHelper.FullItemInfo> createItemResult = null;
                    testHelper.CreateItem(publisherNamespace, accessToken, createdStoreId, itemData, result => createItemResult = result);
                    yield return TestHelper.WaitForValue(() => createItemResult);

                    TestHelper.Assert.IsResultOk(createItemResult, "Create Item, App itemType");
                    TestHelper.Assert.That(createItemResult, Is.Not.Null);
                    appTypeItemId = createItemResult.Value.itemId;
                }

                if (notHasSubsItem)
                {
                    //Create an Subs Item, itemType = "Subscription"
                    TestHelper.ItemCreateModel.Recurring subscriptionData = new TestHelper.ItemCreateModel.Recurring
                    {
                        cycle = "YEARLY",
                        graceDays = 7,
                        fixedFreeDays = 0,
                        fixedTrialCycles = 0
                    };

                    TestHelper.ItemCreateModel subsItemData = new TestHelper.ItemCreateModel
                    {
                        itemType = "SUBSCRIPTION",
                        name = "SDK Test Yearly Subs",
                        entitlementType = "DURABLE",
                        categoryPath = "/subscription",
                        sku = subsTypeItemSku,
                        status = "ACTIVE",
                        recurring = subscriptionData,
                        maxCountPerUser = 1,
                        regionData = new Dictionary<string, RegionDataItem[]>(),
                        localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>()
                    };
                    subsItemData.regionData.Add("US", itemRegionData);
                    subsItemData.localizations.Add("en-US", itemLocalization);

                    Result<TestHelper.FullItemInfo> createSubsItemResult = null;
                    testHelper.CreateItem(publisherNamespace, accessToken, createdStoreId, subsItemData, result => createSubsItemResult = result);
                    yield return TestHelper.WaitForValue(() => createSubsItemResult);

                    TestHelper.Assert.IsResultOk(createSubsItemResult, "Create Item, Subcription itemType");
                    TestHelper.Assert.That(createSubsItemResult, Is.Not.Null);
                    subsTypeItemId = createSubsItemResult.Value.itemId;
                }

                //Publish cloned store
                Result<TestHelper.StoreInfoModel> publishResult = null;
                testHelper.PublishStore(publisherNamespace, accessToken, createdStoreId, result => publishResult = result);
                yield return TestHelper.WaitForValue(() => publishResult);

                TestHelper.LogResult(publishResult, "Publish Clone Store");
                TestHelper.Assert.IsResultOk(publishResult, "Publish Clone Store");
                TestHelper.Assert.That(publishResult, Is.Not.Null);
            }

            #region UserSetup

            Result loginDeviceResult = null;
            user.LoginWithDeviceId(result => { loginDeviceResult = result; });
            yield return TestHelper.WaitForValue(() => loginDeviceResult);

            TestHelper.LogResult(loginDeviceResult, "Login With Device ID");

            // loop check 3 user
            Result<PagedPublicUsersInfo> usersInfo = null;
            for (int i = 0; i < 3; i++)
            {
                user.SearchUsers(usersEmail[i], result => usersInfo = result);
                yield return TestHelper.WaitForValue(() => usersInfo);
                TestHelper.LogResult(usersInfo, "Get User Data With Not Registered Email");

                if (usersInfo.Value.data.Length == 0)
                {
                    // User Create
                    DateTime dateOfBirth = DateTime.Now.AddYears(-22);

                    Result<RegisterUserResponse> registerResult = null;
                    user.Register(
                        usersEmail[i],
                        UserCreateData.Password,
                        string.Format("Subs SDK Test {0}", i.ToString()),
                        UserCreateData.CountryCode,
                        dateOfBirth,
                        result => registerResult = result);
                    yield return TestHelper.WaitForValue(() => registerResult);
                    TestHelper.LogResult(registerResult, "Register " + i + usersEmail[i]);

                    TestHelper.Assert.IsResultOk(registerResult);
                }
            }
            #endregion


            Result logoutDeviceId = null;
            user.Logout(r => logoutDeviceId = r);
            yield return TestHelper.WaitForValue(() => logoutDeviceId);
            TestHelper.LogResult(logoutDeviceId, "Log out user device ID");

            // Get All Users Publisher ID
            string[] usersPublisherIds = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //Login User 1, own both app and subs
                string userId = "";
                Result userLoginResult = null;
                user.LoginWithUsername(usersEmail[i], UserCreateData.Password, res => userLoginResult = res);
                yield return TestHelper.WaitForValue(() => userLoginResult);

                TestHelper.LogResult(userLoginResult, "User Login " + i);
                TestHelper.Assert.IsResultOk(userLoginResult, "User Login " + i);

                userId = user.Session.UserId;

                //Get Publisher UserId
                Result<TestHelper.UserMapResponse> userMapResult = null;
                testHelper.GetUserMap(userId, res => userMapResult = res);
                yield return TestHelper.WaitForValue(() => userMapResult);

                TestHelper.LogResult(userMapResult, "Get User Map " + i);
                TestHelper.Assert.IsResultOk(userMapResult, "Get User Map " + i);

                usersPublisherIds[i] = userMapResult.Value.UserId;

                //Logout User 
                Result logoutResult = null;
                user.Logout(r => logoutResult = r);
                yield return TestHelper.WaitForValue(() => logoutResult);
                TestHelper.LogResult(logoutResult, "Log out user device ID");
            }

            //Grant APP type Item to User 1
            GrantUserEntitlementRequest grantUserEntitlementRequest = new GrantUserEntitlementRequest
            {
                itemId = appTypeItemId,
                itemNamespace = publisherNamespace,
                grantedCode = "123456789",
                quantity = 1,
                source = EntitlementSource.OTHER,
                region = this.region,
                language = this.language
            };

            GrantUserEntitlementRequest[] grantEntitlementData = { grantUserEntitlementRequest };

            ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

            Result<StackableEntitlementInfo[]> stackEntitlementInfoResult = null;
            serverEcommerce.GrantUserEntitlement(
                publisherNamespace,
                usersPublisherIds[0],
                grantEntitlementData,
                result => stackEntitlementInfoResult = result);
            yield return TestHelper.WaitForValue(() => stackEntitlementInfoResult);

            TestHelper.LogResult(stackEntitlementInfoResult, "Grant User 1 App type Entitlements");
            TestHelper.Assert.IsResultOk(stackEntitlementInfoResult, "Grant 1 User App type Entitlements");

            //Grant Subs type Item to User 2
            TestHelper.FreeSubscritptionRequest body = new TestHelper.FreeSubscritptionRequest
            {
                itemId = subsTypeItemId,
                grantDays = 7,
                source = "sdk test",
                reason = "free subs test",
                region = region,
                language = language
            };

            //Login User 2 to grant subs
            Result user2LoginResult = null;
            user.LoginWithUsername(usersEmail[1], UserCreateData.Password, res => user2LoginResult = res);
            yield return TestHelper.WaitForValue(() => user2LoginResult);

            //Check own subs or not
            Result<Ownership> userEligibleResult = null;
            entitlement.GetUserEntitlementOwnershipBySku(subsTypeItemSku, res => userEligibleResult = res);
            yield return TestHelper.WaitForValue(() => userEligibleResult);

            //Grant Subs when not owned
            if (!userEligibleResult.Value.owned)
            {
                Result<TestHelper.FullItemInfo> grantSubsResult = null;
                testHelper.FreeSubscribeByPlatform(
                    publisherNamespace,
                    accessToken,
                    usersPublisherIds[1],
                    body,
                    result => grantSubsResult = result);
                yield return TestHelper.WaitForValue(() => grantSubsResult);

                TestHelper.LogResult(grantSubsResult, "Grant User 2 Subs type Entitlements");
                TestHelper.Assert.IsResultOk(grantSubsResult, "Grant User 2 Subs type Entitlements");
            }

            Result logoutRes = null;
            user.Logout(r => logoutRes = r);
            yield return TestHelper.WaitForValue(() => logoutRes);
            TestHelper.LogResult(logoutRes, "Log out after granted");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator User_Subscription_Entitlement_OwnedApp()
        {
            Result loginResult = null;
            user.LoginWithUsername(usersEmail[0], UserCreateData.Password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.Assert.IsResultOk(loginResult, "Login Failed with user " + UserCreateData.EmailAddress);

            // ACT
            Result<bool> userEligibleResult = null;
            user.GetUserEligibleToPlay(r => userEligibleResult = r);
            yield return TestHelper.WaitForValue(() => userEligibleResult);

            TestHelper.Assert.IsResultOk(userEligibleResult, "error checking user eligibility\n" + userEligibleResult.Error);
            TestHelper.Assert.IsTrue(userEligibleResult.Value);

            // Teardown logout
            Result logoutResult2 = null;
            user.Logout(r => logoutResult2 = r);
            yield return TestHelper.WaitForValue(() => logoutResult2);

        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator User_Subscription_Entitlement_NotOwnedApp_OwnedSub()
        {
            Result loginResult = null;
            user.LoginWithUsername(usersEmail[1], UserCreateData.Password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.Assert.IsResultOk(loginResult, "Login Failed with user " + UserCreateData.EmailAddress);

            // ACT
            Result<Ownership> userEligibleResult = null;
            entitlement.GetUserEntitlementOwnershipBySku(subsTypeItemSku, res => userEligibleResult = res);
            yield return TestHelper.WaitForValue(() => userEligibleResult);

            TestHelper.Assert.IsResultOk(userEligibleResult, "error checking user eligibility\n" + userEligibleResult.Error);
            TestHelper.Assert.IsTrue(userEligibleResult.Value.owned);

            // Teardown logout
            Result logoutResult2 = null;
            user.Logout(r => logoutResult2 = r);
            yield return TestHelper.WaitForValue(() => logoutResult2);

        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator User_Subscription_NotOwnedAll()
        {
            Result loginResult = null;
            user.LoginWithUsername(usersEmail[2], UserCreateData.Password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.Assert.IsResultOk(loginResult, "Login Failed with user " + usersEmail[2]);

            // ACT
            Result<bool> userEligibleResult = null;
            user.GetUserEligibleToPlay(r => userEligibleResult = r);
            yield return TestHelper.WaitForValue(() => userEligibleResult);

            TestHelper.Assert.IsResultOk(userEligibleResult, "error checking user eligibility \n" + userEligibleResult.Error);
            TestHelper.Assert.IsFalse(userEligibleResult.Value);

            // Teardown logout
            Result logoutResult2 = null;
            user.Logout(r => logoutResult2 = r);
            yield return TestHelper.WaitForValue(() => logoutResult2);

        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetEntitlementOwnershipToken_OwnedApp_ReturnOwnershipEntitlement()
        {
            // Arrange
            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.IsResultOk(logoutResult, "Logout Failed.");

            Result loginResult = null;
            user.LoginWithUsername(usersEmail[1], UserCreateData.Password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.Assert.IsResultOk(loginResult, "Login Failed with user " + UserCreateData.EmailAddress);

            // ACT
            Result<OwnershipEntitlement[]> ownershipTokenResult = null;
            entitlement.GetUserEntitlementOwnershipToken(publicKey, null, null, new string[] { subsTypeItemSku }, res => ownershipTokenResult = res);
            yield return TestHelper.WaitForValue(() => ownershipTokenResult);

            TestHelper.Assert.IsResultOk(ownershipTokenResult, "error checking user ownership token\n" + ownershipTokenResult.Error);
            TestHelper.Assert.IsTrue(ownershipTokenResult.Value.Length > 0);
            TestHelper.Assert.IsTrue(ownershipTokenResult.Value[0].sku == subsTypeItemSku);

            if (!ownershipTokenResult.IsError)
            {
                AccelByteDebug.Log("itemId =" + ownershipTokenResult.Value[0].itemId);
                AccelByteDebug.Log("appType =" + ownershipTokenResult.Value[0].appType);
                AccelByteDebug.Log("appId =" + ownershipTokenResult.Value[0].appId);
                AccelByteDebug.Log("namespace =" + ownershipTokenResult.Value[0].Namespace);
                AccelByteDebug.Log("itemNamespace =" + ownershipTokenResult.Value[0].itemNamespace);
                AccelByteDebug.Log("sku =" + ownershipTokenResult.Value[0].sku);
            }

            // Teardown logout
            Result logoutResult2 = null;
            user.Logout(r => logoutResult2 = r);
            yield return TestHelper.WaitForValue(() => logoutResult2);

        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetEntitlementOwnershipToken_NotOwnedApp_ReturnEmptyOwnershipEntitlement()
        {
            // Arrange
            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.IsResultOk(logoutResult, "Logout Failed.");

            Result loginResult = null;
            user.LoginWithUsername(usersEmail[2], UserCreateData.Password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.Assert.IsResultOk(loginResult, "Login Failed with user " + UserCreateData.EmailAddress);

            // ACT
            Result<OwnershipEntitlement[]> ownershipTokenResult = null;
            entitlement.GetUserEntitlementOwnershipToken(publicKey, null, null, new string[] { subsTypeItemSku }, res => ownershipTokenResult = res);
            yield return TestHelper.WaitForValue(() => ownershipTokenResult);

            TestHelper.Assert.IsResultOk(ownershipTokenResult, "error checking user ownership token\n" + ownershipTokenResult.Error);
            TestHelper.Assert.IsFalse(ownershipTokenResult.Value.Length > 0);

            // Teardown logout
            Result logoutResult2 = null;
            user.Logout(r => logoutResult2 = r);
            yield return TestHelper.WaitForValue(() => logoutResult2);

        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetEntitlementOwnershipToken_IncorrectPublicKey_ReturnInvalidResponse()
        {
            // Arrange
            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.IsResultOk(logoutResult, "Logout Failed.");

            Result loginResult = null;
            user.LoginWithUsername(usersEmail[1], UserCreateData.Password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.Assert.IsResultOk(loginResult, "Login Failed with user " + UserCreateData.EmailAddress);

            // ACT
            Result<OwnershipEntitlement[]> ownershipTokenResult = null;
            entitlement.GetUserEntitlementOwnershipToken(invalidPublicKey, null, null, new string[] { subsTypeItemSku }, res => ownershipTokenResult = res);
            yield return TestHelper.WaitForValue(() => ownershipTokenResult);

            TestHelper.Assert.IsTrue(ownershipTokenResult.IsError);
            TestHelper.Assert.IsTrue(ownershipTokenResult.Error.Code == ErrorCode.InvalidResponse);

            // Teardown logout
            Result logoutResult2 = null;
            user.Logout(r => logoutResult2 = r);
            yield return TestHelper.WaitForValue(() => logoutResult2);

        }

    }
}
