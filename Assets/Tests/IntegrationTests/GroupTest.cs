// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class GroupTest
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private Dictionary<string, UserData> usersData = new Dictionary<string, UserData>();
        private string[] userTypes = { "userOpenGroup", "userPublicGroup", "userPrivateGroup", "userMember" };
        private Dictionary<string, string> groupIds = new Dictionary<string, string>();
        private TestHelper helper;
        private string helperAccessToken = null;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private TestHelper.CreateGroupConfigResponse groupConfig = null;
        private TestHelper.CreateMemberRoleResponse customMemberRole = null;
        private string groupName = "SDKUnityGroupTest";
        private bool isGroupConfigExist = false;

        Group CreateGroupService(User user)
        {
            return new Group(new GroupApi(AccelBytePlugin.Config.GroupServerUrl, this.httpClient), user.Session, AccelBytePlugin.Config.Namespace, coroutineRunner);
        }

        Dictionary<string, object> record1Test = new Dictionary<string, object>
        {
            {"numRegion", 6 }, {"oilsReserve", 125.10 }, {"islandName", "tartar friendly land" },
            {"buildings", new string[4] { "oilRefinery", "oilWell", "watchTower", "defendsTower" }},
            {"resources", new Dictionary<string, int>{{"gas", 20 }, {"water", 100 }, {"gold", 10 }}}
        };
        Dictionary<string, object> newRecord1Test = new Dictionary<string, object>
        {
            {"numRegion", 10 }, {"oilsReserve", 100 }, {"islandName", "salad friendly land" },
            {"buildings", new string[4] { "gasRefinery", "gasWell", "waterTower", "mainTower" } },
            {"resources", new Dictionary<string, object>{{"gas", 50 }, {"water", 70}, {"gold", 30} }}
        };

        Dictionary<string, object> record2Test = new Dictionary<string, object>
        {
            {"numIsland", 2 }, {"waterReserve", 125.10 }, {"countryName", "happyland" },
            {"islands", new string[2] { "smile_island", "dance_island" }},
            {"population", new Dictionary<string, int>{{"smile_island", 198 }, {"dance_island", 77 }}}
        };

        [UnityTest, TestLog, Order(0)]
        public IEnumerator Setup()
        {
            var user1 = AccelBytePlugin.GetUser();

            if (user1.Session.IsValid())
            {
                Result logoutResult = null;

                user1.Logout(r => logoutResult = r);

                yield return TestHelper.WaitForValue(() =>logoutResult );

                Assert.IsTrue(!logoutResult.IsError, "User cannot log out.");
            }

            Result loginWithDevice = null;
            user1.LoginWithDeviceId(result => { loginWithDevice = result; });

            yield return TestHelper.WaitForValue(() =>loginWithDevice );

            Assert.IsTrue(!loginWithDevice.IsError, "User cannot log in with device.");

            Result<UserData> user1Result = null;
            user1.GetData(r => user1Result = r);

            yield return TestHelper.WaitForValue(() =>user1Result );

            users.Add(userTypes[0], user1);
            usersData.Add(userTypes[0], user1Result.Value);

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


            string[] makeUserTypes = new string[] { userTypes[1], userTypes[2], userTypes[3] };

            foreach (var userType in makeUserTypes)
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession;

                loginSession = new LoginSession(
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
                    var user = new User(
                        loginSession,
                        userAccount,
                        this.coroutineRunner);

                user.Register(
                        string.Format("groupUser{0}+accelbyteunitysdk@example.com", userType).ToLower(),
                        "Password123",
                        string.Format("groupUser{0}", userType),
                        "US",
                        System.DateTime.Now.AddYears(-22),
                        result => registerResult = result);

                yield return TestHelper.WaitForValue(() =>registerResult );

                TestHelper.LogResult(registerResult, string.Format("Setup: Registered groupUser{0}", userType));

                Assert.True(!registerResult.IsError || registerResult.Error.Code == ErrorCode.EmailAlreadyUsed);

                Result loginResult = null;

                user.LoginWithUsername(
                        string.Format("groupUser{0}+accelbyteunitysdk@example.com", userType).ToLower(),
                        "Password123",
                        result => loginResult = result);

                yield return TestHelper.WaitForValue(() =>loginResult );

                Assert.False(loginResult.IsError);

                Result<UserData> userResult = null;
                user.GetData(r => userResult = r);

                yield return TestHelper.WaitForValue(() =>userResult );

                TestHelper.LogResult(loginResult, "Setup: Logged in " + userResult.Value.displayName);

                yield return new UnityEngine.WaitForSeconds(0.1f);

                users.Add(userType, user);
                usersData.Add(userType, userResult.Value);
            }

            helper.GetSuperUserAccessToken(result =>
            {
                if (!result.IsError)
                {
                    helperAccessToken = result.Value.access_token;
                }
            });

            yield return TestHelper.WaitForValue(() =>helperAccessToken );

            Result<TestHelper.CreateGroupConfigResponse> createGroupConfigResult = null;
            helper.CreateDefaultGroupConfig(helperAccessToken, result =>
            {
                createGroupConfigResult = result;
                if (!createGroupConfigResult.IsError)
                {
                    groupConfig = result.Value;
                }
            });
            
            yield return TestHelper.WaitForValue(() =>createGroupConfigResult );

            Assert.IsTrue((!createGroupConfigResult.IsError) || (int)createGroupConfigResult.Error.Code ==  73130);

            if (createGroupConfigResult.IsError)
            {
                if((int)createGroupConfigResult.Error.Code == 73130)
                {
                    isGroupConfigExist = true;
                }
            }

            Result<TestHelper.PaginatedGroupConfig> getGroupConfigResult = null;
            helper.GetGroupConfig(helperAccessToken, result =>
            {
                getGroupConfigResult = result;
                if (!getGroupConfigResult.IsError && groupConfig == null)
                {
                    groupConfig = result.Value.data[0];
                }
            });

            yield return TestHelper.WaitForValue(() =>getGroupConfigResult );

            Assert.IsFalse(getGroupConfigResult.IsError);

            Result<TestHelper.CreateMemberRoleResponse> createRoleResult = null;
            TestHelper.CreateMemberRoleRequest memberRoleRequest = new TestHelper.CreateMemberRoleRequest
            {
                memberRoleName = "SDKTestRole"
            };
            helper.CreateMemberRole(helperAccessToken, memberRoleRequest, result =>
            {
                createRoleResult = result;
                customMemberRole = result.Value;
            });

            yield return TestHelper.WaitForValue(() =>createRoleResult );

            Assert.IsFalse(createRoleResult.IsError);
        }

        [UnityTest, TestLog, Order(999), Timeout(60000)]
        public IEnumerator Teardown()
        {
            TestHelper testHelper = new TestHelper();

            Result<PaginatedGroupListResponse> searchResult = null;
            testHelper.AdminSearchGroup(helperAccessToken, groupName, result =>
            {
                searchResult = result;
            });

            yield return TestHelper.WaitForValue(() =>searchResult );

            foreach(var group in searchResult.Value.data)
            {
                testHelper.AdminDeleteGroup(helperAccessToken, group.groupId, result => { TestHelper.LogResult(result, "Group Deleted Successfully"); });
            }

            Result deleteMemberRole = null;
            testHelper.DeleteMemberRole(helperAccessToken, customMemberRole.memberRoleId, result =>
            {
                deleteMemberRole = result;
            });

            yield return TestHelper.WaitForValue(() =>deleteMemberRole );
            TestHelper.Assert.That(!deleteMemberRole.IsError);

            if (isGroupConfigExist)
            {
                Result deleteGroupConfig = null;
                testHelper.DeleteGroupConfig(helperAccessToken, groupConfig.configurationCode, result =>
                {
                    deleteGroupConfig = result;
                });

                yield return TestHelper.WaitForValue(() =>deleteGroupConfig );
                TestHelper.Assert.That(!deleteGroupConfig.IsError);

                yield return new WaitForSeconds(5.0f);

                deleteMemberRole = null;
                testHelper.DeleteMemberRole(helperAccessToken, groupConfig.groupMemberRoleId, result =>
                {
                    deleteMemberRole = result;
                });

                yield return TestHelper.WaitForValue(() =>deleteMemberRole );
                TestHelper.Assert.That(!deleteMemberRole.IsError);

                deleteMemberRole = null;
                testHelper.DeleteMemberRole(helperAccessToken, groupConfig.groupAdminRoleId, result =>
                {
                    deleteMemberRole = result;
                });

                yield return TestHelper.WaitForValue(() => deleteMemberRole );
                TestHelper.Assert.That(!deleteMemberRole.IsError);
            }

            foreach (var user in users)
            {
                Result deleteResult = null;
                testHelper.DeleteUser(user.Value, result => deleteResult = result);

                yield return TestHelper.WaitForValue(() => deleteResult );

                TestHelper.Assert.That(!deleteResult.IsError);
            }

            this.users = null;
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator CreateOpenGroup_Success()
        {
            var group = AccelBytePlugin.GetGroup();
            CreateGroupRequest request = new CreateGroupRequest
            {
                configurationCode = groupConfig.configurationCode,
                groupDescription = string.Format("SDK Unity Group Test {0}", userTypes[0]),
                groupName = groupName + userTypes[0],
                groupRegion = "US",
                groupType = GroupType.OPEN
            };

            Result<GroupInformation> groupInfo = null;
            group.CreateGroup(request, result => 
            {
                groupInfo = result;
                groupIds.Add(userTypes[0], result.Value.groupId);
            });
            yield return TestHelper.WaitForValue(() =>groupInfo );

            Assert.False(groupInfo.IsError);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator CreatePublicGroup_Success()
        {
            var group = CreateGroupService(users[userTypes[1]]);
            CreateGroupRequest request = new CreateGroupRequest
            {
                configurationCode = groupConfig.configurationCode,
                groupDescription = string.Format("SDK Unity Group Test {0}", userTypes[1]),
                groupName = groupName + userTypes[1],
                groupRegion = "ID",
                groupType = GroupType.PUBLIC
            };

            Result<GroupInformation> groupInfo = null;
            group.CreateGroup(request, result =>
            {
                groupInfo = result;
                groupIds.Add(userTypes[1], result.Value.groupId);
            });
            yield return TestHelper.WaitForValue(() =>groupInfo );

            Assert.False(groupInfo.IsError);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator CreatePrivateGroup_GetCustomAttribute_Success()
        {
            var group = CreateGroupService(users[userTypes[2]]);
            CreateGroupRequest request = new CreateGroupRequest
            {
                configurationCode = groupConfig.configurationCode,
                groupDescription = string.Format("SDK Unity Group Test {0}", userTypes[2]),
                groupName = groupName + userTypes[2],
                groupRegion = "US",
                groupType = GroupType.PRIVATE,
                customAttributes = record1Test
            };

            Result<GroupInformation> groupInfo = null;
            group.CreateGroup(request, result =>
            {
                groupInfo = result;
                groupIds.Add(userTypes[2], result.Value.groupId);
            });
            yield return TestHelper.WaitForValue(() =>groupInfo );

            Result<GroupInformation> getGroupResult = null;
            group.GetGroup(groupIds[userTypes[2]], result => getGroupResult = result);
            yield return TestHelper.WaitForValue(() =>getGroupResult );

            Assert.False(groupInfo.IsError);
            Assert.False(getGroupResult.IsError);
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["numRegion"], Is.EqualTo(record1Test["numRegion"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["oilsReserve"], Is.EqualTo(record1Test["oilsReserve"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["islandName"], Is.EqualTo(record1Test["islandName"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["buildings"], Is.EqualTo(record1Test["buildings"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["resources"], Is.EqualTo(record1Test["resources"]));
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator CreateGroup_AlreadyJoined()
        {
            var group = AccelBytePlugin.GetGroup();
            CreateGroupRequest request = new CreateGroupRequest
            {
                configurationCode = groupConfig.configurationCode,
                groupDescription = string.Format("SDK Unity Group Test {0}", userTypes[0]),
                groupRegion = "US",
                groupName = groupName+userTypes[0],
                groupType = GroupType.OPEN
            };

            Result<GroupInformation> groupInfo = null;
            group.CreateGroup(request, result =>
            {
                groupInfo = result;
            });
            yield return TestHelper.WaitForValue(() =>groupInfo );

            Assert.True(groupInfo.IsError);
            Assert.True(groupInfo.Error.Code == ErrorCode.UserAlreadyJoinedGroup);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetGroup_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<GroupInformation> groupInfo = null;
            group.GetGroup(groupIds[userTypes[0]], result => { groupInfo = result; });
            yield return TestHelper.WaitForValue(() =>groupInfo );

            Assert.False(groupInfo.IsError);
            Assert.True(groupInfo.Value.groupId == groupIds[userTypes[0]]);
            Assert.True(groupInfo.Value.groupName.Contains(groupName));
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetGroupList_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupListResponse> groupInfos = null;
            group.SearchGroups(result => { groupInfos = result; });
            yield return TestHelper.WaitForValue(() =>groupInfos );

            bool isOpenGroupExist = false;
            bool isPublicGroupExist = false;
            bool isPrivateGroupExist = false;

            foreach(var groupInfo in groupInfos.Value.data)
            {
                if(groupInfo.groupId == groupIds[userTypes[0]])
                {
                    isOpenGroupExist = true;
                }
                if(groupInfo.groupId == groupIds[userTypes[1]])
                {
                    isPublicGroupExist = true;
                }
                if(groupInfo.groupId == groupIds[userTypes[2]])
                {
                    isPrivateGroupExist = true;
                }
            }

            Assert.False(groupInfos.IsError);
            Assert.True(groupInfos.Value.data.Length > 0);
            Assert.True(isOpenGroupExist);
            Assert.True(isPublicGroupExist);
            Assert.False(isPrivateGroupExist);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetGroupListbyGroupName_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupListResponse> groupInfos = null;
            group.SearchGroups(groupName, result => { groupInfos = result; });
            yield return TestHelper.WaitForValue(() =>groupInfos );

            bool isNameExpected = true;
            foreach(var groupInfo in groupInfos.Value.data)
            {
                if (!groupInfo.groupName.Contains(groupName))
                {
                    isNameExpected = false;
                    break;
                }
            }

            Assert.False(groupInfos.IsError);
            Assert.True(groupInfos.Value.data.Length > 0);
            Assert.True(isNameExpected);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetGroupListbyGroupRegion_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            string groupRegion = "US";
            Result<PaginatedGroupListResponse> groupInfos = null;
            group.SearchGroups("", groupRegion, result => { groupInfos = result; });
            yield return TestHelper.WaitForValue(() =>groupInfos );

            bool isIdExpected = false;
            bool isRegionExpected = true;
            foreach (var groupInfo in groupInfos.Value.data)
            {
                if (groupInfo.groupId.Equals(groupIds[userTypes[0]]))
                {
                    isIdExpected = true;
                }
                if (!groupInfo.groupRegion.Equals(groupRegion))
                {
                    isRegionExpected = false;
                    break;
                }
            }

            Assert.False(groupInfos.IsError);
            Assert.True(groupInfos.Value.data.Length > 0);
            Assert.True(isIdExpected);
            Assert.True(isRegionExpected);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetAllGroupList_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupListResponse> groupInfos = null;
            group.SearchGroups(result => { groupInfos = result; });
            yield return TestHelper.WaitForValue(() =>groupInfos );

            if (groupInfos.IsError)
            {
                Debug.Log("ERROR: " + groupInfos.Error.Code + " | MESSAGE: " + groupInfos.Error.Message);
            }

            Assert.False(groupInfos.IsError);
            Assert.True(groupInfos.Value.data.Length > 1);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetGroup_Invalid()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<GroupInformation> groupInfo = null;
            group.GetGroup("Invalid", result => { groupInfo = result; });
            yield return TestHelper.WaitForValue(() =>groupInfo );

            Assert.True(groupInfo.IsError);
            Assert.True(groupInfo.Error.Code == ErrorCode.GroupNotFound);
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator UpdateGroup_Success()
        {
            var group = AccelBytePlugin.GetGroup();
            UpdateGroupRequest request = new UpdateGroupRequest
            {
                groupRegion = "ID"
            };

            Result<GroupInformation> updateResult = null;
            group.UpdateGroup(groupIds[userTypes[0]], request, result =>
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );

            Assert.False(updateResult.IsError);
            Assert.False(string.IsNullOrEmpty(updateResult.Value.groupRegion));
        }

        [UnityTest, TestLog, Order(6)]
        public IEnumerator UpdateGroupType_Success()
        {
            var group = AccelBytePlugin.GetGroup();
            UpdateGroupRequest request = new UpdateGroupRequest
            {
                groupType = GroupType.PUBLIC
            };

            Result<GroupInformation> updateResult = null;
            group.UpdateGroup(groupIds[userTypes[0]], request, result =>
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );

            Assert.False(updateResult.IsError);
            Assert.True(updateResult.Value.groupType == GroupType.PUBLIC);
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator UpdateGroup_CustomAttribute_Success()
        {
            var group = CreateGroupService(users[userTypes[2]]);
            UpdateGroupRequest request = new UpdateGroupRequest
            {
                groupRegion = "ID",
                customAttributes = newRecord1Test
            };

            Result<GroupInformation> updateResult = null;
            group.UpdateGroup(groupIds[userTypes[2]], request, result =>
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );

            Result<GroupInformation> getGroupResult = null;
            group.GetGroup(groupIds[userTypes[2]], result => getGroupResult = result);
            yield return TestHelper.WaitForValue(() =>getGroupResult );

            Assert.False(updateResult.IsError);
            Assert.False(string.IsNullOrEmpty(updateResult.Value.groupRegion));
            Assert.False(getGroupResult.IsError);
            Assert.False(record1Test == getGroupResult.Value.customAttributes);
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["numRegion"], Is.EqualTo(newRecord1Test["numRegion"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["oilsReserve"], Is.EqualTo(newRecord1Test["oilsReserve"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["islandName"], Is.EqualTo(newRecord1Test["islandName"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["buildings"], Is.EqualTo(newRecord1Test["buildings"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["resources"], Is.EqualTo(newRecord1Test["resources"]));
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator UpdateGroup_CustomAttributeOnly_Success()
        {
            var group = CreateGroupService(users[userTypes[2]]);

            Result<GroupInformation> updateResult = null;
            group.UpdateGroupCustomAttributes(groupIds[userTypes[2]], record2Test, result =>
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );

            Result<GroupInformation> getGroupResult = null;
            group.GetGroup(groupIds[userTypes[2]], result => getGroupResult = result);
            yield return TestHelper.WaitForValue(() =>getGroupResult );

            const string updatedDescription = "The Description is updated!";
            UpdateGroupRequest updateGroup = new UpdateGroupRequest
            {
                groupDescription = updatedDescription
            };

            Result<GroupInformation> updateOtherResult = null;
            group.UpdateGroup(groupIds[userTypes[2]], updateGroup, result =>
            {
                updateOtherResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateOtherResult );

            Result<GroupInformation> getGroupUpdatedResult = null;
            group.GetGroup(groupIds[userTypes[2]], result => getGroupUpdatedResult = result);
            yield return TestHelper.WaitForValue(() =>getGroupUpdatedResult );

            Assert.False(updateResult.IsError);
            Assert.False(getGroupResult.IsError);
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["numIsland"], Is.EqualTo(record2Test["numIsland"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["waterReserve"], Is.EqualTo(record2Test["waterReserve"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["islands"], Is.EqualTo(record2Test["islands"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["numIsland"], Is.EqualTo(record2Test["numIsland"]));
            TestHelper.Assert.That(getGroupResult.Value.customAttributes["population"], Is.EqualTo(record2Test["population"]));
            Assert.False(updateOtherResult.IsError);
            Assert.False(getGroupUpdatedResult.IsError);
            TestHelper.Assert.That(getGroupUpdatedResult.Value.customAttributes["numIsland"], Is.EqualTo(record2Test["numIsland"]));
            TestHelper.Assert.That(getGroupUpdatedResult.Value.customAttributes["waterReserve"], Is.EqualTo(record2Test["waterReserve"]));
            TestHelper.Assert.That(getGroupUpdatedResult.Value.customAttributes["islands"], Is.EqualTo(record2Test["islands"]));
            TestHelper.Assert.That(getGroupUpdatedResult.Value.customAttributes["numIsland"], Is.EqualTo(record2Test["numIsland"]));
            TestHelper.Assert.That(getGroupUpdatedResult.Value.customAttributes["population"], Is.EqualTo(record2Test["population"]));
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator UpdateOtherGroup_Failed()
        {
            var group = AccelBytePlugin.GetGroup();
            UpdateGroupRequest request = new UpdateGroupRequest
            {
                groupRegion = "id"
            };

            Result<GroupInformation> updateResult = null;
            group.UpdateGroup("OtherGroup", request, result =>
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );

            Assert.True(updateResult.IsError);
            Assert.True(updateResult.Error.Code == ErrorCode.UserAccessDifferentGroup);
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator UpdateGroup_UserNotBelongToAnyGroup_Failed()
        {
            var group = AccelBytePlugin.GetGroup();
            UpdateGroupRequest request = new UpdateGroupRequest
            {
                groupRegion = "id"
            };

            Result<GroupInformation> updateResult = null;
            group.UpdateGroup("MyGroup", request, result =>
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );

            Assert.True(updateResult.IsError);
            Assert.True(updateResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator UpdatePredefinedRule_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            RuleInformation ruleInformation = new RuleInformation
            {
                ruleAttribute = "Level",
                ruleCriteria = RuleCriteria.MINIMUM,
                ruleValue = 10
            };
            UpdateGroupPredefinedRuleRequest updateRequest = new UpdateGroupPredefinedRuleRequest
            {
                ruleDetail = new List<RuleInformation> { ruleInformation }
            };

            Result<GroupInformation> predefinedUpdate = null;
            group.UpdateGroupPredefinedRule(groupIds[userTypes[0]], AllowedAction.joinGroup, updateRequest, result =>
            {
                predefinedUpdate = result;
            });

            yield return TestHelper.WaitForValue(() =>predefinedUpdate );

            Assert.False(predefinedUpdate.IsError);
            Assert.True(predefinedUpdate.Value.groupRules.groupPredefinedRules.Length > 0);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator DeletePredefinedRule_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result predefinedDelete = null;
            group.DeleteGroupPredefinedRule(groupIds[userTypes[0]], AllowedAction.joinGroup, result =>
            {
                predefinedDelete = result;
            });

            yield return TestHelper.WaitForValue(() =>predefinedDelete );

            Result<GroupInformation> getResponse = null;
            group.GetGroup(groupIds[userTypes[0]], result => 
            {
                getResponse = result;
            });

            yield return TestHelper.WaitForValue(() =>getResponse );

            bool isPredefinedRuleAlreadyDeleted = true;
            foreach(var rule in getResponse.Value.groupRules.groupPredefinedRules)
            {
                if(rule.allowedAction == AllowedAction.joinGroup)
                {
                    isPredefinedRuleAlreadyDeleted = false;
                }
            }

            Assert.False(predefinedDelete.IsError);
            Assert.False(getResponse.IsError);
            Assert.True(isPredefinedRuleAlreadyDeleted);
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator UpdateCustomRule_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Dictionary<string, object> updateRequest = new Dictionary<string, object>
            {
                {"ruleDetail", new Dictionary<string, object> { { "attribute", "friendCount" }, { "criteria", "MININUM"}, { "value", 10 }}}
            };

            Result<GroupInformation> updateResult = null;
            group.UpdateGroupCustomRule(groupIds[userTypes[0]], updateRequest, result => 
            {
                updateResult = result;
            });

            yield return TestHelper.WaitForValue(() =>updateResult );
            
            Assert.False(updateResult.IsError);
            Assert.False(object.Equals(updateResult.Value.groupRules.groupCustomRule, null));
            Assert.That(updateResult.Value.groupRules.groupCustomRule, Is.EqualTo(updateRequest.ToJsonString()));
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator InviteToGroup_Accepted_InvitationListWiped_Success()
        {
            var group = AccelBytePlugin.GetGroup();
            var group2 = CreateGroupService(users[userTypes[3]]);
            var group3 = CreateGroupService(users[userTypes[1]]);

            Result<UserInvitationResponse> inviteResult = null;
            group.InviteOtherUserToGroup(usersData[userTypes[3]].userId, result =>
            {
                inviteResult = result;
            });

            yield return TestHelper.WaitForValue(() =>inviteResult );

            Result<UserInvitationResponse> inviteResult2 = null;
            group3.InviteOtherUserToGroup(usersData[userTypes[3]].userId, result =>
            {
                inviteResult2 = result;
            });

            yield return TestHelper.WaitForValue(() =>inviteResult2 );

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<PaginatedGroupRequestList> invitationResult = null;
            group2.GetGroupInvitationRequests(result => 
            {
                invitationResult = result;
            });

            yield return TestHelper.WaitForValue(() =>invitationResult );

            Result<GroupGeneralResponse> acceptInvitationResult = null;
            group2.AcceptGroupInvitation(groupIds[userTypes[0]], result =>
            {
                acceptInvitationResult = result;
            });

            yield return TestHelper.WaitForValue(() =>acceptInvitationResult );

            Result<GroupMemberInformation> groupInfoJoinedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinedResult );

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach(var member in memberList.Value.data)
            {
                if(member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Result<PaginatedGroupRequestList> invitationResult2 = null;
            group2.GetGroupInvitationRequests(result =>
            {
                invitationResult2 = result;
            });

            yield return TestHelper.WaitForValue(() =>invitationResult2 );

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() =>kickResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberKicked = true;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Result<GroupMemberInformation> groupInfoKickedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoKickedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoKickedResult );

            Assert.False(invitationResult.IsError);
            Assert.False(inviteResult.IsError);
            Assert.False(inviteResult2.IsError);
            Assert.False(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Value.status == MemberStatus.INVITE);
            Assert.True(invitationResult.Value.data.Length > 1);
            Assert.False(acceptInvitationResult.IsError);
            Assert.False(groupInfoJoinedResult.IsError);
            Assert.True(groupInfoJoinedResult.Value.status == MemberStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.False(invitationResult2.IsError);
            Assert.True(invitationResult2.Value.data.Length == 0);
            Assert.False(kickResult.IsError);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberKicked);
            Assert.True(groupInfoKickedResult.IsError);
            Assert.True(groupInfoKickedResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator InviteToGroup_AcceptedInvalid_Failed()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupGeneralResponse> acceptInvitationResult = null;
            group2.AcceptGroupInvitation("Invalid", result =>
            {
                acceptInvitationResult = result;
            });

            yield return TestHelper.WaitForValue(() =>acceptInvitationResult );

            Assert.True(acceptInvitationResult.IsError);
            //Assert.True(acceptInvitationResult.Error.Code == ErrorCode.GroupNotFound);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator InviteToGroup_Rejected_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<UserInvitationResponse> inviteResult = null;
            group.InviteOtherUserToGroup(usersData[userTypes[3]].userId, result =>
            {
                inviteResult = result;
            });

            yield return TestHelper.WaitForValue(() =>inviteResult );

            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<PaginatedGroupRequestList> invitationResult = null;
            group2.GetGroupInvitationRequests(result =>
            {
                invitationResult = result;
            });

            yield return TestHelper.WaitForValue(() =>invitationResult );

            Result<GroupGeneralResponse> rejectInvitationResult = null;
            group2.RejectGroupInvitation(groupIds[userTypes[0]], result =>
            {
                rejectInvitationResult = result;
            });

            yield return TestHelper.WaitForValue(() =>rejectInvitationResult );

            Result<GroupMemberInformation> groupInfoRejectedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoRejectedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoRejectedResult );

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Assert.False(invitationResult.IsError);
            Assert.False(inviteResult.IsError);
            Assert.False(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Value.status == MemberStatus.INVITE);
            Assert.True(invitationResult.Value.data.Length > 0);
            Assert.True(groupInfoRejectedResult.IsError);
            Assert.True(groupInfoRejectedResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(memberList.IsError);
            Assert.False(isMemberExist);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator joinOpenGroup_Success()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[0]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            Result<GroupMemberInformation> groupInfoJoinedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinedResult );

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() =>kickResult );

            Result<GroupMemberInformation> groupInfoKickedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoKickedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberKicked = true;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Assert.True(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.JOINED);
            Assert.False(groupInfoJoinedResult.IsError);
            Assert.True(groupInfoJoinedResult.Value.status == MemberStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.False(kickResult.IsError);
            Assert.True(groupInfoKickedResult.IsError);
            Assert.True(groupInfoKickedResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberKicked);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator joinPublicGroup_Success()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[1]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            Result<GroupMemberInformation> groupInfoJoinResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinResult );

            var group = CreateGroupService(users[userTypes[1]]);

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Result<GroupMemberInformation> groupInfoOtherJoinResult = null;
            group.GetOtherGroupInfo(users[userTypes[3]].Session.UserId, result =>
            {
                groupInfoOtherJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoOtherJoinResult );

            Result<PaginatedGroupRequestList> joinReqResult = null;
            group.GetGroupJoinRequests(groupIds[userTypes[1]], 0, 0, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                joinReqResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinReqResult );

            Result<GroupGeneralResponse> acceptJoinResult = null;
            group.AcceptOtherJoinRequest(joinReqResult.Value.data[0].userId, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                acceptJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>acceptJoinResult );

            Result<GroupMemberInformation> groupInfoJoinedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinedResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberExist2 = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist2 = true;
                }
            }

            Result<GroupGeneralResponse> leaveResult = null;
            group2.LeaveGroup(result =>
            {
                leaveResult = result;
            });

            yield return TestHelper.WaitForValue(() =>leaveResult );

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList3 );

            bool isMemberLeave = true;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberLeave = false;
                }
            }

            Result<GroupMemberInformation> groupInfoLeaveResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoLeaveResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoLeaveResult );

            Assert.True(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.REQUESTED);
            Assert.False(groupInfoJoinResult.IsError);
            Assert.True(groupInfoJoinResult.Value.status == MemberStatus.JOIN);
            Assert.False(memberList.IsError);
            Assert.False(isMemberExist);
            Assert.False(groupInfoOtherJoinResult.IsError);
            Assert.True(groupInfoOtherJoinResult.Value.status == MemberStatus.JOIN);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberExist2);
            Assert.False(groupInfoJoinedResult.IsError);
            Assert.True(groupInfoJoinedResult.Value.status == MemberStatus.JOINED);
            Assert.True(joinReqResult.Value.data.Length > 0);
            Assert.False(leaveResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberLeave);
            Assert.True(groupInfoLeaveResult.IsError);
            Assert.True(groupInfoLeaveResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator joinPublicGroup_Rejected()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[1]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            Result<GroupMemberInformation> groupInfoJoinResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinResult );

            var group = CreateGroupService(users[userTypes[1]]);

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Result<PaginatedGroupRequestList> joinReqResult = null;
            group.GetGroupJoinRequests(groupIds[userTypes[1]], 0, 0, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                joinReqResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinReqResult );

            Result<GroupGeneralResponse> rejectJoinResult = null;
            group.RejectOtherJoinRequest(joinReqResult.Value.data[0].userId, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                rejectJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>rejectJoinResult );

            Result<GroupMemberInformation> groupInfoRejectedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoRejectedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoRejectedResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberExist2 = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist2 = true;
                }
            }

            Assert.True(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.REQUESTED);
            Assert.False(groupInfoJoinResult.IsError);
            Assert.True(groupInfoJoinResult.Value.status == MemberStatus.JOIN);
            Assert.False(memberList.IsError);
            Assert.False(isMemberExist);
            Assert.False(memberList2.IsError);
            Assert.False(isMemberExist2);
            Assert.True(joinReqResult.Value.data.Length > 0);
            Assert.True(groupInfoRejectedResult.IsError);
            Assert.True(groupInfoRejectedResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator joinPublicGroup_CancelIt()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[1]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            Result<GroupMemberInformation> groupInfoJoinResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinResult );

            var group = CreateGroupService(users[userTypes[1]]);

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Result<PaginatedGroupRequestList> joinReqResult = null;
            group.GetGroupJoinRequests(groupIds[userTypes[1]], 0, 0, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                joinReqResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinReqResult );

            Result<GroupGeneralResponse> cancelJoinResult = null;
            group2.CancelJoinGroupRequest(groupIds[userTypes[1]], result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                cancelJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>cancelJoinResult );

            Result<GroupMemberInformation> groupInfoCancelledResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoCancelledResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoCancelledResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberExist2 = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist2 = true;
                }
            }

            Assert.True(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.REQUESTED);
            Assert.False(groupInfoJoinResult.IsError);
            Assert.True(groupInfoJoinResult.Value.status == MemberStatus.JOIN);
            Assert.False(memberList.IsError);
            Assert.False(isMemberExist);
            Assert.False(cancelJoinResult.IsError);
            Assert.False(memberList2.IsError);
            Assert.False(isMemberExist2);
            Assert.True(joinReqResult.Value.data.Length > 0);
            Assert.True(groupInfoCancelledResult.IsError);
            Assert.True(groupInfoCancelledResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator joinPublicGroup_CancelAlreadyAccepted()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<GroupMemberInformation> getGroupInfoResult = null;
            group2.GetMyGroupInfo(result =>
            {
                getGroupInfoResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getGroupInfoResult );

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[1]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            Result<GroupMemberInformation> groupInfoJoinResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinResult );

            var group = CreateGroupService(users[userTypes[1]]);

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Result<GroupMemberInformation> groupInfoOtherJoinResult = null;
            group.GetOtherGroupInfo(users[userTypes[3]].Session.UserId, result =>
            {
                groupInfoOtherJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoOtherJoinResult );

            Result<PaginatedGroupRequestList> joinReqResult = null;
            group.GetGroupJoinRequests(groupIds[userTypes[1]], 0, 0, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                joinReqResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinReqResult );

            Result<GroupGeneralResponse> acceptJoinResult = null;
            group.AcceptOtherJoinRequest(joinReqResult.Value.data[0].userId, result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                acceptJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>acceptJoinResult );

            Result<GroupGeneralResponse> cancelJoinResult = null;
            group2.CancelJoinGroupRequest(groupIds[userTypes[1]], result =>
            {
                if (result.IsError)
                {
                    Debug.Log("Error: " + result.Error.Code + " | Message: " + result.Error.Message);
                }
                cancelJoinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>cancelJoinResult );

            Result<GroupMemberInformation> groupInfoJoinedResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoJoinedResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoJoinedResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberExist2 = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist2 = true;
                }
            }

            Result<GroupGeneralResponse> leaveResult = null;
            group2.LeaveGroup(result =>
            {
                leaveResult = result;
            });

            yield return TestHelper.WaitForValue(() =>leaveResult );

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[1]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList3 );

            bool isMemberLeave = true;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberLeave = false;
                }
            }

            Result<GroupMemberInformation> groupInfoLeaveResult = null;
            group2.GetMyGroupInfo(result =>
            {
                groupInfoLeaveResult = result;
            });

            yield return TestHelper.WaitForValue(() =>groupInfoLeaveResult );

            Assert.True(getGroupInfoResult.IsError);
            Assert.True(getGroupInfoResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.REQUESTED);
            Assert.False(groupInfoJoinResult.IsError);
            Assert.True(groupInfoJoinResult.Value.status == MemberStatus.JOIN);
            Assert.False(memberList.IsError);
            Assert.False(isMemberExist);
            Assert.False(groupInfoOtherJoinResult.IsError);
            Assert.True(groupInfoOtherJoinResult.Value.status == MemberStatus.JOIN);
            Assert.True(cancelJoinResult.IsError);
            Assert.True(cancelJoinResult.Error.Code == ErrorCode.MemberRequestNotFound);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberExist2);
            Assert.False(groupInfoJoinedResult.IsError);
            Assert.True(groupInfoJoinedResult.Value.status == MemberStatus.JOINED);
            Assert.True(joinReqResult.Value.data.Length > 0);
            Assert.False(leaveResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberLeave);
            Assert.True(groupInfoLeaveResult.IsError);
            Assert.True(groupInfoLeaveResult.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator joinPrivateGroup_Failed()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[2]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                }
            }

            Assert.True(joinResult.IsError);
            Assert.True(joinResult.Error.Code == ErrorCode.PrivateGroupIsNotJoinable);
            Assert.False(memberList.IsError);
            Assert.False(isMemberExist);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator GetGroupRoles_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedMemberRoles> getRolesResult = null;
            group.GetMemberRoles(result =>
            {
                getRolesResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getRolesResult );

            Assert.False(getRolesResult.IsError);
            Assert.True(getRolesResult.Value.data.Length > 0);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator AssignMemberRole_Success()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[0]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            bool isMemberHaveCustomRole = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                    foreach(var role in member.memberRoleId)
                    {
                        if(role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRole = true;
                            break;
                        }
                    }
                }
            }

            Result<GroupMemberInformation> assignRoleResult = null;
            group.AssignRoleToMember(customMemberRole.memberRoleId, usersData[userTypes[3]].userId, result =>
            {
                assignRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() =>assignRoleResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberHaveCustomRoleNow = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRoleNow = true;
                            break;
                        }
                    }
                }
            }

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() =>kickResult );

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList3 );

            bool isMemberKicked = true;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.False(isMemberHaveCustomRole);
            Assert.False(assignRoleResult.IsError);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberHaveCustomRoleNow);
            Assert.False(kickResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberKicked);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator AssignMemberRole_NonAdminRole()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[0]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            bool isMemberHaveCustomRole = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRole = true;
                            break;
                        }
                    }
                }
            }

            Result<GroupMemberInformation> assignRoleResult = null;
            group2.AssignRoleToMember(customMemberRole.memberRoleId, usersData[userTypes[3]].userId, result =>
            {
                assignRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() =>assignRoleResult );

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList2 );

            bool isMemberHaveCustomRoleNow = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRoleNow = true;
                            break;
                        }
                    }
                }
            }

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() =>kickResult );

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList3 );

            bool isMemberKicked = true;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.False(isMemberHaveCustomRole);
            Assert.True(assignRoleResult.IsError);
            Assert.False(memberList2.IsError);
            Assert.False(isMemberHaveCustomRoleNow);
            Assert.False(kickResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberKicked);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator RemoveMemberRole_Success()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[0]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            bool isMemberHaveCustomRole = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRole = true;
                            break;
                        }
                    }
                }
            }

            Result<GroupMemberInformation> assignRoleResult = null;
            group.AssignRoleToMember(customMemberRole.memberRoleId, usersData[userTypes[3]].userId, result =>
            {
                assignRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() => assignRoleResult);

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() => memberList2);

            bool isMemberHaveCustomRoleNow = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRoleNow = true;
                            break;
                        }
                    }
                }
            }

            Result removeRoleResult = null;
            group.RemoveRoleFromMember(customMemberRole.memberRoleId, usersData[userTypes[3]].userId, result =>
            {
                removeRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() =>removeRoleResult );

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() => memberList3);

            bool isMemberHaveCustomRoleAfterRemoved = false;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRoleAfterRemoved = true;
                            break;
                        }
                    }
                }
            }

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() =>kickResult );

            Result<PaginatedGroupMemberList> memberList4 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList4 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList4 );

            bool isMemberKicked = true;
            foreach (var member in memberList4.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.False(isMemberHaveCustomRole);
            Assert.False(assignRoleResult.IsError);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberHaveCustomRoleNow);
            Assert.False(removeRoleResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.False(isMemberHaveCustomRoleAfterRemoved);
            Assert.False(kickResult.IsError);
            Assert.False(memberList4.IsError);
            Assert.True(isMemberKicked);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator RemoveMemberRole_NonAdminRole()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[0]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() =>joinResult );

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList );

            bool isMemberExist = false;
            bool isMemberHaveCustomRole = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRole = true;
                            break;
                        }
                    }
                }
            }

            Result<GroupMemberInformation> assignRoleResult = null;
            group.AssignRoleToMember(customMemberRole.memberRoleId, usersData[userTypes[3]].userId, result =>
            {
                assignRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() => assignRoleResult);

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() => memberList2);

            bool isMemberHaveCustomRoleNow = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRoleNow = true;
                            break;
                        }
                    }
                }
            }

            Result removeRoleResult = null;
            group2.RemoveRoleFromMember(customMemberRole.memberRoleId, usersData[userTypes[3]].userId, result =>
            {
                removeRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() =>removeRoleResult );

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList3 );

            bool isMemberHaveCustomRoleAfterRemoved = false;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == customMemberRole.memberRoleId)
                        {
                            isMemberHaveCustomRoleAfterRemoved = true;
                            break;
                        }
                    }
                }
            }

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() =>kickResult );

            Result<PaginatedGroupMemberList> memberList4 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList4 = result;
            });

            yield return TestHelper.WaitForValue(() =>memberList4 );

            bool isMemberKicked = true;
            foreach (var member in memberList4.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.False(isMemberHaveCustomRole);
            Assert.False(assignRoleResult.IsError);
            Assert.False(memberList2.IsError);
            Assert.True(isMemberHaveCustomRoleNow);
            Assert.True(removeRoleResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberHaveCustomRoleAfterRemoved);
            Assert.False(kickResult.IsError);
            Assert.False(memberList4.IsError);
            Assert.True(isMemberKicked);
        }

        [UnityTest, Order(5)]
        public IEnumerator RemoveMemberRole_ErrorMemberMustHaveRole()
        {
            var group2 = CreateGroupService(users[userTypes[3]]);

            Result<JoinGroupResponse> joinResult = null;
            group2.JoinGroup(groupIds[userTypes[0]], result =>
            {
                joinResult = result;
            });

            yield return TestHelper.WaitForValue(() => joinResult);

            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> memberList = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList = result;
            });

            yield return TestHelper.WaitForValue(() => memberList);

            bool isMemberExist = false;
            bool isMemberHaveRole = false;
            foreach (var member in memberList.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberExist = true;
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == groupConfig.groupMemberRoleId)
                        {
                            isMemberHaveRole = true;
                            break;
                        }
                    }
                }
            }

            Result removeRoleResult = null;
            group.RemoveRoleFromMember(groupConfig.groupMemberRoleId, usersData[userTypes[3]].userId, result =>
            {
                removeRoleResult = result;
            });

            yield return TestHelper.WaitForValue(() => removeRoleResult);

            Result<PaginatedGroupMemberList> memberList2 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList2 = result;
            });

            yield return TestHelper.WaitForValue(() => memberList2);

            bool isMemberStillHaveRole = false;
            foreach (var member in memberList2.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    foreach (var role in member.memberRoleId)
                    {
                        if (role == groupConfig.groupMemberRoleId)
                        {
                            isMemberStillHaveRole = true;
                            break;
                        }
                    }
                }
            }

            Result<KickMemberResponse> kickResult = null;
            group.KickGroupMember(usersData[userTypes[3]].userId, result =>
            {
                kickResult = result;
            });

            yield return TestHelper.WaitForValue(() => kickResult);

            Result<PaginatedGroupMemberList> memberList3 = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                memberList3 = result;
            });

            yield return TestHelper.WaitForValue(() => memberList3);

            bool isMemberKicked = true;
            foreach (var member in memberList3.Value.data)
            {
                if (member.userId == usersData[userTypes[3]].userId)
                {
                    isMemberKicked = false;
                }
            }

            Assert.False(joinResult.IsError);
            Assert.True(joinResult.Value.status == JoinGroupStatus.JOINED);
            Assert.False(memberList.IsError);
            Assert.True(isMemberExist);
            Assert.True(isMemberHaveRole);
            Assert.True(removeRoleResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberStillHaveRole);
            Assert.False(kickResult.IsError);
            Assert.False(memberList3.IsError);
            Assert.True(isMemberKicked);
        }

        [UnityTest, TestLog, Order(6)]
        public IEnumerator GetUserGroupInfo()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<GroupMemberInformation> myGroupResultExist = null;
            group.GetMyGroupInfo(result =>
            {
                myGroupResultExist = result;
            });

            yield return TestHelper.WaitForValue(() =>myGroupResultExist );

            Result<GroupMemberInformation> otherGroupResultExist = null;
            group.GetOtherGroupInfo(usersData[userTypes[1]].userId, result => 
            {
                otherGroupResultExist = result;
            });
            yield return TestHelper.WaitForValue(() =>otherGroupResultExist );

            Result<GroupMemberInformation> otherGroupResultNotExist = null;
            group.GetOtherGroupInfo(usersData[userTypes[3]].userId, result =>
            {
                otherGroupResultNotExist = result;
            });
            yield return TestHelper.WaitForValue(() =>otherGroupResultNotExist );

            Assert.False(myGroupResultExist.IsError);
            Assert.False(otherGroupResultExist.IsError);
            Assert.True(otherGroupResultNotExist.IsError);
            Assert.True(otherGroupResultNotExist.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(6)]
        public IEnumerator GetGroupMember_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result<PaginatedGroupMemberList> getMemberResult = null;
            group.GetGroupMemberList(groupIds[userTypes[0]], result =>
            {
                getMemberResult = result;
            });

            yield return TestHelper.WaitForValue(() =>getMemberResult );

            bool isExpectedMember = true;
            foreach(var member in getMemberResult.Value.data)
            {
                if(member.groupId != groupIds[userTypes[0]])
                {
                    isExpectedMember = false;
                }
            }

            Assert.False(getMemberResult.IsError);
            Assert.True(getMemberResult.Value.data.Length > 0);
            Assert.True(isExpectedMember);
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator DeleteGroup_Success()
        {
            var group = AccelBytePlugin.GetGroup();

            Result deleteGroup = null;
            group.DeleteGroup(groupIds[userTypes[0]], result => { deleteGroup = result; });
            yield return TestHelper.WaitForValue(() =>deleteGroup );

            Assert.False(deleteGroup.IsError);
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator DeleteOtherGroup_Failed()
        {
            var group = AccelBytePlugin.GetGroup();

            Result deleteGroup = null;
            group.DeleteGroup(groupIds[userTypes[1]], result => { deleteGroup = result; });
            yield return TestHelper.WaitForValue(() =>deleteGroup );

            Assert.True(deleteGroup.IsError);
            Assert.True(deleteGroup.Error.Code == ErrorCode.UserAccessDifferentGroup || deleteGroup.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator DeleteGroup_UserNotBelongToAnyGroup_Failed()
        {
            var group = CreateGroupService(users[userTypes[3]]);

            Result deleteGroup = null;
            group.DeleteGroup(groupIds[userTypes[0]], result => { deleteGroup = result; });
            yield return TestHelper.WaitForValue(() =>deleteGroup );

            Assert.True(deleteGroup.IsError);
            Assert.True(deleteGroup.Error.Code == ErrorCode.UserNotBelongToAnyGroup);
        }

    }
}
