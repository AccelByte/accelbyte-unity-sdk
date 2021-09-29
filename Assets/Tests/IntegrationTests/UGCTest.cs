// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class UGCTest
    {
        private UgcTestAdmin ugcTestAdmin;
        private UGC Ugc = null;
        private string helperAccessToken = null;
        private TestHelper helper;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private User user;
        private string accountNamespace = "sdktest";

        private static string UGCChannelName = "Integration Test Channel Unity";
        private string UGCChannelId = "";
        private static string UGCType = "Plane";
        private static string UGCTypeId = "";
        private static string[] UGCSubType = { "Cockpit", "Wings", "Engine" };
        private static string[] UGCTags = { "Airbus", "Boeing", "Atr" };
        List<string> UGCTagId = new List<string>();
        private string[] UGCTagIds = null;
        private static string UGCInvalidChannelId = "InvalidChannelId";
        private static string UGCInvalidContentId = "InvalidContentId";
        private static string UGCInvalidShareCode = "InvalidShareCode";
        UGCRequest UGCCreateContentRequest = new UGCRequest
        {
            fileExtension = "png",
            name = "Integration Test Unity",
            preview = "",
            type = UGCType,
            subtype = UGCSubType[0],
            tags = new[] { UGCTags[0], UGCTags[1] }
        };
        UGCRequest UGCModifyContentRequest0 = new UGCRequest
        {
            fileExtension = "txt",
            name = "Modified-0 Integration Test Unity",
            preview = "",
            type = UGCType,
            subtype = UGCSubType[1],
            tags = new[] { UGCTags[1], UGCTags[2] }
        };
        UGCRequest UGCModifyContentRequest1 = new UGCRequest
        {
            fileExtension = "png",
            name = "Modified-1 Integration Test Unity",
            type = UGCType,
            subtype = UGCSubType[0],
            tags = new[] { UGCTags[0], UGCTags[2] }
        };
        List<UGCTagResponse> UGCCreatedTags = new List<UGCTagResponse>();
        private static byte[] UGCContentBytes = { 106, 35, 188, 171, 106, 138, 197, 77, 94, 182, 18, 99, 9, 245, 110, 45, 197, 22, 35, 171 };
        private static byte[] UGCPreviewBytes = { 154, 221, 92, 161, 163, 42, 108, 18, 111, 191, 203, 239, 249, 43, 248, 255, 191, 132, 105, 26 };
        private User user2;
        private UserData user2Data;
        private static string user2email = "ugcuser+unity@game.test";
        private static string user2DisplayName = "ugcuser2";

        public bool UGCCheckContainChannel(string expectedChannelId, UGCChannelResponse[] channels)
        {
            foreach (var channel in channels)
            {
                if (channel.id.Equals(expectedChannelId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UGCCheckContainTag(string expectedTagName, string expectedTagId, UGCTagResponse[] tags)
        {
            foreach (var tag in tags)
            {
                if (tag.tag.Equals(expectedTagName) && tag.id.Equals(expectedTagId))
                {  
                    return true;
                }
            }
            return false;
        }

        public bool UGCCheckContainTag(string expectedTagName, string[] tags)
        {
            foreach (var tag in tags)
            {
                if (tag.Equals(expectedTagName))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UGCCheckContainSubType(string expectedSubTypeName, string[] subtypes)
        {
            foreach (var subtype in subtypes)
            {
                if (subtype.Equals(expectedSubTypeName))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UGCCheckContainType(string expectedTypeName, string expectedTypeId, UGCTypeResponse[] types)
        {
            foreach (var type in types)
            {
                if (type.type.Equals(expectedTypeName) && type.id.Equals(expectedTypeId))
                {
                    return(
                        UGCCheckContainSubType(UGCSubType[0], type.subtype)
                        && UGCCheckContainSubType(UGCSubType[1], type.subtype)
                        && UGCCheckContainSubType(UGCSubType[2], type.subtype)
                        );
                }
            }
            return false;
        }

        public bool UGCCheckContentEqual(UGCContentResponse contentResponse, UGCResponse response)
        {
            if (!response.id.Equals(contentResponse.id)) { Debug.Log(("Id1 : {0} != Id2 : {1}", response.id, contentResponse.id)); return false; }
            if (!response.name.Equals(contentResponse.name)) { Debug.Log(("Name1 : {0} != Name2 : {1}", response.name, contentResponse.name)); return false; }
            if (!response.Namespace.Equals(contentResponse.Namespace)) { Debug.Log(("Namespace1 : {0} != Namespace2 : {1}", response.Namespace, contentResponse.Namespace)); return false; }
            if (!response.channelId.Equals(contentResponse.channelId)) { Debug.Log(("ChannelId1 : {0} != ChannelId2 : {1}", response.channelId, contentResponse.channelId)); return false; }
            if (!response.creatorName.Equals(contentResponse.creatorName)) { Debug.Log(("CreatorName1 : {0} != CreatorName2 : {1}", response.creatorName, contentResponse.creatorName)); return false; }
            if (!response.fileExtension.Equals(contentResponse.fileExtension)) { Debug.Log(("FileExtension1 : {0} != FileExtension2 : {1}", response.fileExtension, contentResponse.fileExtension)); return false; }
            if (!response.shareCode.Equals(contentResponse.shareCode)) { Debug.Log(("ShareCode1 : {0} != ShareCode2 : {1}", response.shareCode, contentResponse.shareCode)); return false; }
            if (!response.userId.Equals(contentResponse.userId)) { Debug.Log(("UserId1 : {0} != UserId2 : {1}", response.userId, contentResponse.userId)); return false; }
            if (!response.createdTime.Equals(contentResponse.createdTime)) { Debug.Log(("CreatedTime1 : {0} != CreatedTime2 : {1}", response.createdTime.ToString(), contentResponse.createdTime.ToString())); return false; }
            if (!response.isOfficial.Equals(contentResponse.isOfficial)) 
            { 
                Debug.Log(("IsOfficial1 : {0} != IsOfficial2 : {1}", response.isOfficial ? "true" : "false", contentResponse.isOfficial ? "true" : "false"));
                return false; 
            }
            if (!response.type.Equals(contentResponse.type)) { Debug.Log(("Type1 : {0} != Type2 : {1}", response.type, contentResponse.type)); return false; }
            if (!response.subType.Equals(contentResponse.subType)) { Debug.Log(("SubType1 : {0} != SubType2 : {1}", response.subType, contentResponse.subType)); return false; }

            foreach (string tag in response.tags)
            {
                bool bIsFound = false;
                foreach (string tag2 in contentResponse.tags)
                {
                    if (tag.Equals(tag2))
                    {
                        bIsFound = true;
                        break;
                    }
                }
                if (!bIsFound) { Debug.Log(("Tag : {0} is not found", tag)); return false; }
            }
            return true;
        }

        [UnityTest, TestLog, Order(0), Timeout(150000)]
        public IEnumerator Setup()
        {
            if (this.httpClient == null)
            {
                this.httpClient = new AccelByteHttpClient();
                this.httpClient.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            }

            if (this.coroutineRunner == null)
            {
                this.coroutineRunner = new CoroutineRunner();
            }

            if (this.helper == null)
            {
                this.helper = new TestHelper();
            }

            if (this.ugcTestAdmin == null)
            {
                this.ugcTestAdmin = new UgcTestAdmin();
            }

            if (this.user != null) yield break;

            this.user2 = AccelBytePlugin.GetUser();
            this.user2Data = new UserData();

            this.user = AccelBytePlugin.GetUser();
            this.Ugc = AccelBytePlugin.GetUgc();

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;

                this.user.Logout(result => logoutResult = result);

                while (logoutResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsFalse(logoutResult.IsError, "User 1 cannot log out.");
            }

            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);
            TestHelper.Assert.IsTrue(!loginWithDevice.IsError || loginWithDevice.Error.Code == ErrorCode.InvalidRequest, "User 1 cannot login.");

            Result<TokenData> getAccessToken = null;
            this.helper.GetSuperUserAccessToken(result => { getAccessToken = result; });
            yield return TestHelper.WaitForValue(() => getAccessToken);
            TestHelper.Assert.IsResultOk(getAccessToken);
            this.helperAccessToken = getAccessToken.Value.access_token;

            Result<UGCTypesPagingResponse> GetTypesResponse = null;
            this.Ugc.GetTypes(result => { GetTypesResponse = result; });
            yield return TestHelper.WaitForValue(() => GetTypesResponse);
            TestHelper.LogResult(GetTypesResponse, "Get type response");

            foreach (var ResponseType in GetTypesResponse.Value.data)
            {
                if (ResponseType.type.Equals(UGCType))
                {
                    Result deleteType = null;
                    this.ugcTestAdmin.DeleteType(accountNamespace, helperAccessToken, ResponseType.id, result => { deleteType = result; });
                    yield return TestHelper.WaitForValue(() => deleteType);
                    TestHelper.Assert.IsResultOk(deleteType);
                    TestHelper.LogResult(deleteType, "Delete type response");
                }
            }

            Result<UGCTagsPagingResponse> GetTagsResponse = null;
            this.Ugc.GetTags(result => { GetTagsResponse = result; });
            yield return TestHelper.WaitForValue(() => GetTagsResponse);
            TestHelper.LogResult(GetTagsResponse, "Get tags response");

            foreach (var UGCTag in UGCTags)
            {
                foreach (var responseTag in GetTagsResponse.Value.data)
                {
                    if (responseTag.tag.Equals(UGCTag))
                    {
                        Result deleteTag = null;
                        this.ugcTestAdmin.DeleteTag(accountNamespace, helperAccessToken, responseTag.id, result => { deleteTag = result; });
                        yield return TestHelper.WaitForValue(() => deleteTag);
                        TestHelper.Assert.IsResultOk(deleteTag);
                        TestHelper.LogResult(deleteTag, "Delete tag response");
                    }
                }
            }

            string previousChannelId = null;
            Result<UGCChannelPagingResponse> GetChannelResponse = null;
            this.Ugc.GetChannels(result => { GetChannelResponse = result; });
            yield return TestHelper.WaitForValue(() => GetChannelResponse);
            TestHelper.LogResult(GetChannelResponse, "Get channel response");

            foreach(var channel in GetChannelResponse.Value.data)
            {
                if (channel.name.Equals(UGCChannelName))
                {
                    previousChannelId = GetChannelResponse.Value.data[0].id;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(previousChannelId))
            {
                Result deleteChannel = null;
                this.Ugc.DeleteChannel(previousChannelId, result => { deleteChannel = result; });
                yield return TestHelper.WaitForValue(() => deleteChannel);
                TestHelper.Assert.IsResultOk(deleteChannel);
                TestHelper.LogResult(deleteChannel, "Delete channel done");
            }

            Result<UGCTypeResponse> UGCCreateTypeResponse = null;
            this.ugcTestAdmin.CreateType(accountNamespace, this.helperAccessToken, UGCType, UGCSubType, result => 
            {
                UGCCreateTypeResponse = result;
                UGCTypeId = result.Value.id;
            });
            yield return TestHelper.WaitForValue(() => UGCCreateTypeResponse);
            TestHelper.Assert.IsResultOk(UGCCreateTypeResponse);
            TestHelper.LogResult(UGCCreateTypeResponse, "Create type response");
            
            foreach (string UGCTag in UGCTags)
            {
                Result<UGCTagResponse> CreateTagResponse = null;
                this.ugcTestAdmin.CreateTags(accountNamespace, this.helperAccessToken, UGCTag, result => 
                { 
                    CreateTagResponse = result; 
                    UGCTagId.Add(result.Value.id);
                    UGCTagIds = UGCTagId.ToArray();
                });
                yield return TestHelper.WaitForValue(() => CreateTagResponse);
                TestHelper.Assert.IsResultOk(CreateTagResponse);
                TestHelper.LogResult(CreateTagResponse, "Create tag response");
            }

            Result<PagedPublicUsersInfo> searchUser2 = null;
            this.user.SearchUsers(user2DisplayName, SearchType.DISPLAYNAME, result => 
            {
                searchUser2 = result;
            });
            yield return TestHelper.WaitForValue(() => searchUser2);
            TestHelper.Assert.IsResultOk(searchUser2);
            TestHelper.LogResult(searchUser2, "Search user 2 response");

            if (searchUser2.Value.data.Length < 1)
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                        AccelBytePlugin.Config.IamServerUrl,
                        AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.RedirectUri,
                        this.httpClient,
                        this.coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    this.httpClient);

                var newUser = new User(
                    loginSession,
                    userAccount,
                    this.coroutineRunner);

                newUser
                    .Register(
                        user2email,
                        "Password123",
                        user2DisplayName,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);
                TestHelper.LogResult(registerResult, "Setup: Register ugc user 2");
            }
        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Teardown()
        {
            Result deleteType = null;
            this.ugcTestAdmin.DeleteType(accountNamespace, this.helperAccessToken, UGCTypeId, result => { deleteType = result; });
            yield return TestHelper.WaitForValue(() => deleteType);
            TestHelper.Assert.IsResultOk(deleteType);
            TestHelper.LogResult(deleteType, "Delete type done");
            
            foreach (var UGCTag in UGCCreatedTags)
            {
                Result deleteTag = null;
                this.ugcTestAdmin.DeleteTag(accountNamespace, this.helperAccessToken, UGCTag.id, result => { deleteTag = result; });
                yield return TestHelper.WaitForValue(() => deleteTag);
                TestHelper.Assert.IsResultOk(deleteTag);
                TestHelper.LogResult(deleteTag, "Delete tag done");
            }

            Result logoutResult = null;
            this.user.Logout(result => logoutResult = result);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.IsResultOk(logoutResult);
            TestHelper.LogResult(logoutResult, "Logged out");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator Create_Get_Delete_Channel()
        {
            String createdChannelName = "";
            Result<UGCChannelResponse> CreatedChannelResponse = null;
            this.Ugc.CreateChannel(UGCChannelName, result => 
            {
                CreatedChannelResponse = result;
                createdChannelName = CreatedChannelResponse.Value.name;
                UGCChannelId = CreatedChannelResponse.Value.id;
            });
            yield return TestHelper.WaitForValue(() => CreatedChannelResponse);
            TestHelper.Assert.IsResultOk(CreatedChannelResponse);
            TestHelper.LogResult(CreatedChannelResponse, "Created channel response");
            TestHelper.Assert.IsTrue(createdChannelName.Equals(UGCChannelName));

            Result<UGCChannelPagingResponse> GetChannelResponse = null;
            this.Ugc.GetChannels(result => { GetChannelResponse = result; });
            yield return TestHelper.WaitForValue(() => GetChannelResponse);
            TestHelper.LogResult(GetChannelResponse, "Get channel response");
            TestHelper.Assert.IsTrue(UGCCheckContainChannel(UGCChannelId, GetChannelResponse.Value.data));

            Result deleteChannel = null;
            this.Ugc.DeleteChannel(UGCChannelId, result => { deleteChannel = result; });
            yield return TestHelper.WaitForValue(() => deleteChannel);
            TestHelper.Assert.IsResultOk(deleteChannel);
            TestHelper.LogResult(deleteChannel, "Delete channel done");

        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator GetTags()
        {
            Result<UGCTagsPagingResponse> GetTagsResponse = null;
            this.Ugc.GetTags(result => { GetTagsResponse = result; });
            yield return TestHelper.WaitForValue(() => GetTagsResponse);
            TestHelper.Assert.IsResultOk(GetTagsResponse);
            TestHelper.LogResult(GetTagsResponse, "Get tags response");
            TestHelper.Assert.IsTrue(UGCCheckContainTag(UGCTags[0], UGCTagIds[0], GetTagsResponse.Value.data));
            TestHelper.Assert.IsTrue(UGCCheckContainTag(UGCTags[1], UGCTagIds[1], GetTagsResponse.Value.data));
            TestHelper.Assert.IsTrue(UGCCheckContainTag(UGCTags[2], UGCTagIds[2], GetTagsResponse.Value.data));
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator GetTypes()
        {
            Result<UGCTypesPagingResponse> GetTypeResponse = null;
            this.Ugc.GetTypes(result => { GetTypeResponse = result; });
            yield return TestHelper.WaitForValue(() => GetTypeResponse);
            TestHelper.Assert.IsResultOk(GetTypeResponse);
            TestHelper.LogResult(GetTypeResponse, "Get type response");
            TestHelper.Assert.IsTrue(UGCCheckContainType(UGCType, UGCTypeId, GetTypeResponse.Value.data));
        }

        [UnityTest, TestLog, Order(2), Timeout(150000)]
        public IEnumerator Create_Get_Modify_Delete_Content_As_String()
        {
            String createdChannelName = "";
            Result<UGCChannelResponse> CreatedChannelResponse = null;
            this.Ugc.CreateChannel(UGCChannelName, result =>
            {
                CreatedChannelResponse = result;
                createdChannelName = CreatedChannelResponse.Value.name;
                UGCChannelId = CreatedChannelResponse.Value.id;
            });
            yield return TestHelper.WaitForValue(() => CreatedChannelResponse);
            TestHelper.Assert.IsResultOk(CreatedChannelResponse);
            TestHelper.LogResult(CreatedChannelResponse, "Created channel response");
            TestHelper.Assert.IsTrue(createdChannelName.Equals(UGCChannelName));

            Result<UGCResponse> CreatedContentResponse = null;
            this.Ugc.CreateContent(UGCChannelId, UGCCreateContentRequest, result => { CreatedContentResponse = result; });
            yield return TestHelper.WaitForValue(() => CreatedContentResponse);
            TestHelper.LogResult(CreatedContentResponse, "Create content response");
            TestHelper.Assert.IsResultOk(CreatedContentResponse);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.channelId, UGCChannelId);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.fileExtension, UGCCreateContentRequest.fileExtension);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.name, UGCCreateContentRequest.name);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.preview, UGCCreateContentRequest.preview);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.type, UGCCreateContentRequest.type);
            TestHelper.Assert.IsTrue(UGCCheckContainTag(CreatedContentResponse.Value.tags[0], UGCCreateContentRequest.tags));
            TestHelper.Assert.IsTrue(UGCCheckContainTag(CreatedContentResponse.Value.tags[1], UGCCreateContentRequest.tags));

            Result UploadResult = null;
            AccelByteNetUtilities.UploadTo(CreatedContentResponse.Value.payloadURL[0].url, UGCContentBytes, result => { UploadResult = result; });
            yield return TestHelper.WaitForValue(() => UploadResult);
            TestHelper.LogResult(UploadResult, "Upload content done");
            TestHelper.Assert.IsResultOk(UploadResult);

            Result<UGCContentResponse> GetContentResponseById = null;
            this.Ugc.GetContentByContentId(CreatedContentResponse.Value.id, result => { GetContentResponseById = result; });
            yield return TestHelper.WaitForValue(() => GetContentResponseById);
            TestHelper.LogResult(GetContentResponseById, "Get content response by id");
            TestHelper.Assert.IsResultOk(GetContentResponseById);
            TestHelper.Assert.IsTrue(UGCCheckContentEqual(GetContentResponseById.Value, CreatedContentResponse.Value));

            Result<byte[]> DownloadBytes = null;
            AccelByteNetUtilities.DownloadFrom(GetContentResponseById.Value.payloadURL[0].url, result => { DownloadBytes = result; });
            yield return TestHelper.WaitForValue(() => DownloadBytes);
            TestHelper.LogResult(DownloadBytes, "Download Bytes response");
            TestHelper.Assert.IsResultOk(DownloadBytes);
            TestHelper.Assert.Equals(DownloadBytes, UGCContentBytes);

            Result<UGCContentResponse> GetContentResponseByShareCodeByCreator = null;
            this.Ugc.GetContentByShareCode(CreatedContentResponse.Value.shareCode, result => { GetContentResponseByShareCodeByCreator = result; });
            yield return TestHelper.WaitForValue(() => GetContentResponseByShareCodeByCreator);
            TestHelper.LogResult(GetContentResponseByShareCodeByCreator, "Get content response by share code");
            TestHelper.Assert.IsResultOk(GetContentResponseByShareCodeByCreator);
            TestHelper.Assert.Equals(GetContentResponseByShareCodeByCreator.Value, CreatedContentResponse.Value);

            Result user1LogoutResult = null;
            this.user.Logout(result => user1LogoutResult = result);
            yield return TestHelper.WaitForValue(() => user1LogoutResult);
            TestHelper.Assert.IsResultOk(user1LogoutResult);
            TestHelper.LogResult(user1LogoutResult, "User 1 logged out");

            Result user2LoginWithUsername = null;
            this.user2
                .LoginWithUsername(
                    user2email,
                    "Password123",
                    result => user2LoginWithUsername = result);
            yield return TestHelper.WaitForValue(() => user2LoginWithUsername);
            TestHelper.LogResult(user2LoginWithUsername, "Login ugc user 2");
            TestHelper.Assert.IsTrue(!user2LoginWithUsername.IsError || user2LoginWithUsername.Error.Code == ErrorCode.InvalidRequest, "User 2 cannot login.");

            //get content by share code by user 2
            Result<UGCContentResponse> GetContentResponseByShareCodeByUser2 = null;
            this.Ugc.GetContentByShareCode(CreatedContentResponse.Value.shareCode, result => { GetContentResponseByShareCodeByUser2 = result; });
            yield return TestHelper.WaitForValue(() => GetContentResponseByShareCodeByUser2);
            TestHelper.LogResult(GetContentResponseByShareCodeByUser2, "Get content response by share code");
            TestHelper.Assert.IsResultOk(GetContentResponseByShareCodeByUser2);
            TestHelper.Assert.Equals(GetContentResponseByShareCodeByUser2.Value, CreatedContentResponse.Value);

            Result user2LogoutResult = null;
            this.user2.Logout(result => user2LogoutResult = result);
            yield return TestHelper.WaitForValue(() => user2LogoutResult);
            TestHelper.Assert.IsResultOk(user2LogoutResult);
            TestHelper.LogResult(user2LogoutResult, "User 2 logged out");

            Result deleteResult = null;
            this.helper.DeleteUserByDisplayName(user2DisplayName, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);
            TestHelper.LogResult(deleteResult, "Delete user 2");
            TestHelper.Assert.IsResultOk(deleteResult);

            Result user1LoginWithDevice = null;
            this.user.LoginWithDeviceId(result => { user1LoginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => user1LoginWithDevice);
            TestHelper.Assert.IsTrue(!user1LoginWithDevice.IsError || user1LoginWithDevice.Error.Code == ErrorCode.InvalidRequest, "User 1 cannot login.");

            Result<UGCPreview> GetContentPreviewAsString = null;
            this.Ugc.GetContentPreview(CreatedContentResponse.Value.id, result => { GetContentPreviewAsString = result; });
            yield return TestHelper.WaitForValue(() => GetContentPreviewAsString);
            TestHelper.LogResult(GetContentPreviewAsString, "Get content preview as string");
            TestHelper.Assert.IsResultOk(GetContentPreviewAsString);
            TestHelper.Assert.Equals(GetContentPreviewAsString.Value.preview, UGCCreateContentRequest.preview);

            Result<UGCResponse> ModifyContentResponse = null;
            this.Ugc.ModifyContent(UGCChannelId, CreatedContentResponse.Value.id, UGCModifyContentRequest0, result => { ModifyContentResponse = result; } );
            yield return TestHelper.WaitForValue(() => ModifyContentResponse);
            TestHelper.LogResult(ModifyContentResponse, "Modify content response");
            TestHelper.Assert.IsResultOk(ModifyContentResponse);

            Result<UGCContentResponse> GetModifyContentResponseById = null;
            this.Ugc.GetContentByContentId(CreatedContentResponse.Value.id, result => { GetModifyContentResponseById = result; });
            yield return TestHelper.WaitForValue(() => GetModifyContentResponseById);
            TestHelper.LogResult(GetModifyContentResponseById, "Get modified content response by id");
            TestHelper.Assert.IsResultOk(GetModifyContentResponseById);
            TestHelper.Assert.IsTrue(UGCCheckContentEqual(GetModifyContentResponseById.Value, ModifyContentResponse.Value));

            ModifyContentResponse = null;
            this.Ugc.ModifyContent(UGCChannelId, CreatedContentResponse.Value.id, UGCModifyContentRequest1.name, UGCModifyContentRequest1.type,
                UGCModifyContentRequest1.subtype, UGCModifyContentRequest1.tags, UGCPreviewBytes, UGCModifyContentRequest1.fileExtension, result => { ModifyContentResponse = result; });
            yield return TestHelper.WaitForValue(() => ModifyContentResponse);
            TestHelper.LogResult(ModifyContentResponse, "Modify content response");
            TestHelper.Assert.IsResultOk(ModifyContentResponse);

            GetModifyContentResponseById = null;
            this.Ugc.GetContentByContentId(CreatedContentResponse.Value.id, result => { GetModifyContentResponseById = result; });
            yield return TestHelper.WaitForValue(() => GetModifyContentResponseById);
            TestHelper.LogResult(GetModifyContentResponseById, "Get modified content response by id");
            TestHelper.Assert.IsResultOk(GetModifyContentResponseById);
            TestHelper.Assert.IsTrue(UGCCheckContentEqual(GetModifyContentResponseById.Value, ModifyContentResponse.Value));

            Result<byte[]> GetContentPreviewAsByteArray = null;
            this.Ugc.GetContentPreview(CreatedContentResponse.Value.id, result => { GetContentPreviewAsByteArray = result; });
            yield return TestHelper.WaitForValue(() => GetContentPreviewAsByteArray);
            TestHelper.LogResult(GetContentPreviewAsByteArray, "Get content preview as byte array");
            TestHelper.Assert.IsResultOk(GetContentPreviewAsByteArray);
            TestHelper.Assert.Equals(GetContentPreviewAsByteArray.Value, UGCPreviewBytes);

            Result DeleteContentResult = null;
            this.Ugc.DeleteContent(UGCChannelId, CreatedContentResponse.Value.id, result => { DeleteContentResult = result; });
            yield return TestHelper.WaitForValue(() => DeleteContentResult);
            TestHelper.LogResult(DeleteContentResult, "Delete content response");
            TestHelper.Assert.IsResultOk(DeleteContentResult);

            GetContentResponseById = null;
            bool contenNotFound = false;
            this.Ugc.GetContentByContentId(CreatedContentResponse.Value.id, result => 
            { 
                GetContentResponseById = result;
                if (!string.IsNullOrEmpty(GetContentResponseById.Error.ToString())) { contenNotFound = true; }
            });
            yield return TestHelper.WaitForValue(() => GetContentResponseById);
            TestHelper.LogResult(GetContentResponseById, "UGC content not found " + GetContentResponseById.Error);
            TestHelper.Assert.IsTrue(contenNotFound);

            Result deleteChannel = null;
            this.Ugc.DeleteChannel(UGCChannelId, result => { deleteChannel = result; });
            yield return TestHelper.WaitForValue(() => deleteChannel);
            TestHelper.LogResult(deleteChannel, "Delete channel done");
            TestHelper.Assert.IsResultOk(deleteChannel);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator Create_Get_Delete_Content_As_Bytes()
        {
            String createdChannelName = "";
            Result<UGCChannelResponse> CreatedChannelResponse = null;
            this.Ugc.CreateChannel(UGCChannelName, result =>
            {
                CreatedChannelResponse = result;
                createdChannelName = CreatedChannelResponse.Value.name;
                UGCChannelId = CreatedChannelResponse.Value.id;
            });
            yield return TestHelper.WaitForValue(() => CreatedChannelResponse);
            TestHelper.Assert.IsResultOk(CreatedChannelResponse);
            TestHelper.LogResult(CreatedChannelResponse, "Created channel response");
            TestHelper.Assert.IsTrue(createdChannelName.Equals(UGCChannelName));

            Result<UGCResponse> CreatedContentResponse = null;
            this.Ugc.CreateContent(UGCChannelId, UGCCreateContentRequest.name, UGCCreateContentRequest.type, UGCCreateContentRequest.subtype,
                UGCCreateContentRequest.tags, UGCPreviewBytes, UGCCreateContentRequest.fileExtension, result => { CreatedContentResponse = result; });
            yield return TestHelper.WaitForValue(() => CreatedContentResponse);
            TestHelper.LogResult(CreatedContentResponse, "Create content response");
            TestHelper.Assert.IsResultOk(CreatedContentResponse);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.channelId, UGCChannelId);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.fileExtension, UGCCreateContentRequest.fileExtension);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.name, UGCCreateContentRequest.name);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.preview, UGCCreateContentRequest.preview);
            TestHelper.Assert.Equals(CreatedContentResponse.Value.type, UGCCreateContentRequest.type);
            TestHelper.Assert.IsTrue(UGCCheckContainTag(CreatedContentResponse.Value.tags[0], UGCCreateContentRequest.tags));
            TestHelper.Assert.IsTrue(UGCCheckContainTag(CreatedContentResponse.Value.tags[1], UGCCreateContentRequest.tags));

            Result<UGCContentResponse> GetContentResponseById = null;
            this.Ugc.GetContentByContentId(CreatedContentResponse.Value.id, result => { GetContentResponseById = result; });
            yield return TestHelper.WaitForValue(() => GetContentResponseById);
            TestHelper.LogResult(GetContentResponseById, "Get content response by id");
            TestHelper.Assert.IsResultOk(GetContentResponseById);
            TestHelper.Assert.IsTrue(UGCCheckContentEqual(GetContentResponseById.Value, CreatedContentResponse.Value));

            Result<byte[]> GetContentPreviewAsByteArray = null;
            this.Ugc.GetContentPreview(CreatedContentResponse.Value.id, result => { GetContentPreviewAsByteArray = result; });
            yield return TestHelper.WaitForValue(() => GetContentPreviewAsByteArray);
            TestHelper.LogResult(GetContentPreviewAsByteArray, "Get content preview as byte array");
            TestHelper.Assert.IsResultOk(GetContentPreviewAsByteArray);
            TestHelper.Assert.Equals(GetContentPreviewAsByteArray.Value, UGCPreviewBytes);

            Result DeleteContentResult = null;
            this.Ugc.DeleteContent(UGCChannelId, CreatedContentResponse.Value.id, result => { DeleteContentResult = result; });
            yield return TestHelper.WaitForValue(() => DeleteContentResult);
            TestHelper.LogResult(DeleteContentResult, "Delete content response");
            TestHelper.Assert.IsResultOk(DeleteContentResult);

            GetContentResponseById = null;
            bool contenNotFound = false;
            this.Ugc.GetContentByContentId(CreatedContentResponse.Value.id, result =>
            {
                GetContentResponseById = result;
                if (!string.IsNullOrEmpty(GetContentResponseById.Error.ToString())) { contenNotFound = true; }
            });
            yield return TestHelper.WaitForValue(() => GetContentResponseById);
            TestHelper.LogResult(GetContentResponseById, "UGC content not found " + GetContentResponseById.Error);
            TestHelper.Assert.IsTrue(contenNotFound);

            Result deleteChannel = null;
            this.Ugc.DeleteChannel(UGCChannelId, result => { deleteChannel = result; });
            yield return TestHelper.WaitForValue(() => deleteChannel);
            TestHelper.LogResult(deleteChannel, "Delete channel done");
            TestHelper.Assert.IsResultOk(deleteChannel);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator Create_Content_Invalid_Channel_Id()
        {
            Result<UGCResponse> CreatedContentResponse = null;
            bool createContentDone = false;
            this.Ugc.CreateContent(UGCInvalidChannelId, UGCCreateContentRequest, result =>
            {
                CreatedContentResponse = result;
                if (!string.IsNullOrEmpty(CreatedContentResponse.Error.ToString())) { createContentDone = true; }
            });
            yield return TestHelper.WaitForValue(() => CreatedContentResponse);
            TestHelper.LogResult(CreatedContentResponse, "Create content response " + CreatedContentResponse.Error);
            TestHelper.Assert.IsTrue(createContentDone);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetContent_InvalidContentId()
        {
            Result<UGCContentResponse> GetContentResponseById = null;
            bool contenNotFound = false;
            this.Ugc.GetContentByContentId(UGCInvalidContentId, result =>
            {
                GetContentResponseById = result;
                if (!string.IsNullOrEmpty(GetContentResponseById.Error.ToString())) { contenNotFound = true; }
            });
            yield return TestHelper.WaitForValue(() => GetContentResponseById);
            TestHelper.LogResult(GetContentResponseById, "UGC content not found " + GetContentResponseById.Error);
            TestHelper.Assert.IsTrue(contenNotFound);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetContent_InvalidShareCode()
        {
            Result<UGCContentResponse> GetContentResponseByShareCode = null;
            bool contenNotFound = false;
            this.Ugc.GetContentByShareCode(UGCInvalidShareCode, result =>
            {
                GetContentResponseByShareCode = result;
                if (!string.IsNullOrEmpty(GetContentResponseByShareCode.Error.ToString())) { contenNotFound = true; }
            });
            yield return TestHelper.WaitForValue(() => GetContentResponseByShareCode);
            TestHelper.LogResult(GetContentResponseByShareCode, "UGC content not found " + GetContentResponseByShareCode.Error);
            TestHelper.Assert.IsTrue(contenNotFound);
        }
    }
}