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
    public class ReportingTest
    {
        private Reporting reporting = null;
        private string helperAccessToken = null;
        private TestHelper helper;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private User user;
        private string accountNamespace = "sdktest";

        private static string[] ReportingReasonsTest = { "ReasonTest0", "ReasonTest1", "ReasonTest2", "ReasonTest3" };
        private static string[] ReportingReasonGroupsTest = { "ReasonGroupTest0", "ReasonGroupTest1", "ReasonGroupTest2" };

        private User user2;
        private static string user2email = "reportinguser+unity@game.test";

        [UnityTest, TestLog, Order(0), Timeout(150000)]
        public IEnumerator Setup()
        {
            if (this.httpClient == null)
            {
                this.httpClient = new AccelByteHttpClient();
            }

            if (this.coroutineRunner == null)
            {
                this.coroutineRunner = new CoroutineRunner();
            }

            if (this.helper == null)
            {
                this.helper = new TestHelper();
            }
            if (this.user != null) yield break;

            this.user2 = AccelBytePlugin.GetUser();

            this.user = AccelBytePlugin.GetUser();
            this.reporting = AccelBytePlugin.GetReporting();

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

            // Try to login
            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);
            TestHelper.Assert.IsTrue(!loginWithDevice.IsError || loginWithDevice.Error.Code == ErrorCode.InvalidRequest, "User 1 cannot login.");

            // Delete current user if available
            Result deleteResult = null;
            this.helper.DeleteUser(this.user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);
            TestHelper.LogResult(deleteResult, "Delete user");
            TestHelper.Assert.IsResultOk(deleteResult);

            // Login again
            loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);
            TestHelper.Assert.IsTrue(!loginWithDevice.IsError || loginWithDevice.Error.Code == ErrorCode.InvalidRequest, "User 1 cannot login.");

            Result<TokenData> getAccessToken = null;
            this.helper.GetAccessToken(result => { getAccessToken = result; });
            yield return TestHelper.WaitForValue(() => getAccessToken);
            TestHelper.Assert.IsResultOk(getAccessToken);
            this.helperAccessToken = getAccessToken.Value.access_token;

            Result<PagedPublicUsersInfo> searchUser2 = null;
            this.user.SearchUsers(user2email, result =>
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
                        "reportinguser2",
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);
                TestHelper.LogResult(registerResult, "Setup: Register reporting user 2");
            }

            Result user2LoginResult = null;
            user2.LoginWithUsername(user2email, "Password123", result => user2LoginResult = result);
            yield return TestHelper.WaitForValue(() => user2LoginResult);
            TestHelper.Assert.IsTrue(!user2LoginResult.IsError || user2LoginResult.Error.Code == ErrorCode.InvalidRequest, "User 2 cannot login.");

            // Setup Reporting configuration.
            // Clean up reason test.
            Result<TestHelper.ReportingAdminReasonsResponse> ReasonsResponse = null;
            String ReasonGroup = "";
            this.helper.ReportingAdminGetReasons(accountNamespace, helperAccessToken, ReasonGroup, result => { ReasonsResponse = result; });
            yield return TestHelper.WaitForValue(() => ReasonsResponse);
            TestHelper.Assert.IsResultOk(ReasonsResponse);
            TestHelper.LogResult(ReasonsResponse, "Get Admin Reasons");
            
            foreach (String ReportingReasonTest in ReportingReasonsTest)
            {
                foreach (TestHelper.ReportingAdminReasonItem ReasonItem in ReasonsResponse.Value.data)
                {
                    if (ReportingReasonTest == ReasonItem.title)
                    {
                        Result DeleteReason = null;
                        this.helper.ReportingDeleteReason(accountNamespace, helperAccessToken, ReasonItem.id, result => { DeleteReason = result; });
                        yield return TestHelper.WaitForValue(() => DeleteReason);
                        TestHelper.Assert.IsResultOk(DeleteReason);
                        TestHelper.LogResult(DeleteReason, "Delete Reason");
                    }
                }
            }

            // Clean up reason group test.
            Result<ReportingReasonGroupsResponse> ReasonGroupResponse = null;
            this.helper.ReportingAdminGetReasonGroups(accountNamespace, helperAccessToken, result => { ReasonGroupResponse = result; });
            yield return TestHelper.WaitForValue(() => ReasonGroupResponse);
            TestHelper.Assert.IsResultOk(ReasonGroupResponse);
            TestHelper.LogResult(ReasonGroupResponse, "Get Admin Reason Group");

            foreach (String ReportingReasonGroupTest in ReportingReasonGroupsTest)
            {
                foreach (ReportingReasonGroupItem ReasonGroupItem in ReasonGroupResponse.Value.data)
                {
                    if (ReportingReasonGroupTest == ReasonGroupItem.title)
                    {
                        Result DeleteReasonGroup = null;
                        this.helper.ReportingDeleteReasonGroup(accountNamespace, helperAccessToken, ReasonGroupItem.id, result => { DeleteReasonGroup = result; });
                        yield return TestHelper.WaitForValue(() => DeleteReasonGroup);
                        TestHelper.Assert.IsResultOk(DeleteReasonGroup);
                        TestHelper.LogResult(DeleteReasonGroup, "Delete Reason Group");
                    }
                }
            }

            // Add Reasons
            foreach (String ReportingReasonTest in ReportingReasonsTest)
            {
                TestHelper.ReportingAddReasonRequest AddReasonRequest = new TestHelper.ReportingAddReasonRequest();
                AddReasonRequest.title = ReportingReasonTest;
                AddReasonRequest.description = ReportingReasonTest + " Description";
                Result AddReason = null;
                this.helper.ReportingAddReason(accountNamespace, helperAccessToken, AddReasonRequest, result => { AddReason = result; });
                yield return TestHelper.WaitForValue(() => AddReason);
                TestHelper.Assert.IsResultOk(AddReason);
                TestHelper.LogResult(AddReason, "Add Reason");
            }

            ReasonsResponse = null;
            ReasonGroup = "";
            this.helper.ReportingAdminGetReasons(accountNamespace, helperAccessToken, ReasonGroup, result => { ReasonsResponse = result; });
            yield return TestHelper.WaitForValue(() => ReasonsResponse);
            TestHelper.Assert.IsResultOk(ReasonsResponse);
            TestHelper.LogResult(ReasonsResponse, "Get Admin Reasons");

            // Add Reason Group
            Result AddReasonGroup = null;
            TestHelper.ReportingAddReasonGroupRequest AddReasonRequest1 = new TestHelper.ReportingAddReasonGroupRequest();
            String[] ReasonIds = { ReasonsResponse.Value.data[0].id, ReasonsResponse.Value.data[1].id, ReasonsResponse.Value.data[2].id };
            AddReasonRequest1.reasonIds = ReasonIds;
            AddReasonRequest1.title = ReportingReasonGroupsTest[0];
            this.helper.ReportingAddReasonGroup(accountNamespace, helperAccessToken, AddReasonRequest1, result => { AddReasonGroup = result; });
            yield return TestHelper.WaitForValue(() => AddReasonGroup);
            TestHelper.Assert.IsResultOk(AddReasonGroup);
            TestHelper.LogResult(AddReasonGroup, "Add Reason Group");

            AddReasonGroup = null;
            TestHelper.ReportingAddReasonGroupRequest AddReasonRequest2 = new TestHelper.ReportingAddReasonGroupRequest();
            AddReasonRequest2.title = ReportingReasonGroupsTest[1];
            this.helper.ReportingAddReasonGroup(accountNamespace, helperAccessToken, AddReasonRequest2, result => { AddReasonGroup = result; });
            yield return TestHelper.WaitForValue(() => AddReasonGroup);
            TestHelper.Assert.IsResultOk(AddReasonGroup);
            TestHelper.LogResult(AddReasonGroup, "Add Reason Group");

            AddReasonGroup = null;
            TestHelper.ReportingAddReasonGroupRequest AddReasonRequest3 = new TestHelper.ReportingAddReasonGroupRequest();
            AddReasonRequest3.title = ReportingReasonGroupsTest[2];
            this.helper.ReportingAddReasonGroup(accountNamespace, helperAccessToken, AddReasonRequest3, result => { AddReasonGroup = result; });
            yield return TestHelper.WaitForValue(() => AddReasonGroup);
            TestHelper.Assert.IsResultOk(AddReasonGroup);
            TestHelper.LogResult(AddReasonGroup, "Add Reason Group");

        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Teardown()
        {
            Result deleteResult = null;
            this.helper.DeleteUser(this.user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);
            TestHelper.LogResult(deleteResult, "Delete user");
            TestHelper.Assert.IsResultOk(deleteResult);

            Result logoutResult = null;
            this.user2.Logout(result => logoutResult = result);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.IsResultOk(logoutResult);
            TestHelper.LogResult(logoutResult, "Logged out user 2");
        }
        
        [UnityTest, TestLog, Order(1)]
        public IEnumerator GetReasons()
        {
            String reasonGroup = "";
            // Get Reason with empty ReasonGroup
            Result<ReportingReasonsResponse> GetReasonsResponse = null;
            this.reporting.GetReasons(reasonGroup, result => { GetReasonsResponse = result; }, 0, 2);
            yield return TestHelper.WaitForValue(() => GetReasonsResponse);
            TestHelper.Assert.IsResultOk(GetReasonsResponse);
            TestHelper.LogResult(GetReasonsResponse, "Get Reasons response");
            TestHelper.Assert.IsTrue(GetReasonsResponse.Value.data.Length != 0);
            TestHelper.Assert.Equals(GetReasonsResponse.Value.data.Length, 2);
        }
        
        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetReasonGroup()
        {
            // Get Reason Groups
            Result<ReportingReasonGroupsResponse> GetReasonGroupsResponse = null;
            this.reporting.GetReasonGroups(result => { GetReasonGroupsResponse = result; }, 0, 2);
            yield return TestHelper.WaitForValue(() => GetReasonGroupsResponse);
            TestHelper.Assert.IsResultOk(GetReasonGroupsResponse);
            TestHelper.LogResult(GetReasonGroupsResponse, "Get Reason Groups response");
            TestHelper.Assert.IsTrue(GetReasonGroupsResponse.Value.data.Length != 0);
            TestHelper.Assert.Equals(GetReasonGroupsResponse.Value.data.Length, 2);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator GetReasons_WithReasonGroup()
        {
            Result<ReportingReasonsResponse> GetReasonsResponse_Group1 = null;
            Result<ReportingReasonsResponse> GetReasonsResponse_Group2 = null;

            // Get Reason ReasonGroup
            Result<ReportingReasonGroupsResponse> GetReasonGroupsResponse = null;
            this.reporting.GetReasonGroups(result => { GetReasonGroupsResponse = result; });
            yield return TestHelper.WaitForValue(() => GetReasonGroupsResponse);
            TestHelper.Assert.IsResultOk(GetReasonGroupsResponse);
            TestHelper.LogResult(GetReasonGroupsResponse, "Get Reason Groups response");
            TestHelper.Assert.IsTrue(GetReasonGroupsResponse.Value.data.Length != 0);

            // Get Reason with ReasonGroup
            this.reporting.GetReasons(GetReasonGroupsResponse.Value.data[0].title, result => { GetReasonsResponse_Group1 = result; }, 0, 2);
            yield return TestHelper.WaitForValue(() => GetReasonsResponse_Group1);
            TestHelper.LogResult(GetReasonsResponse_Group1, "Get Reasons response");

            // Get Reason with unlinked ReasonGroup
            this.reporting.GetReasons(GetReasonGroupsResponse.Value.data[1].title, result => { GetReasonsResponse_Group2 = result; });
            yield return TestHelper.WaitForValue(() => GetReasonsResponse_Group2);
            TestHelper.LogResult(GetReasonsResponse_Group2, "Get Reasons response");

            TestHelper.Assert.IsResultOk(GetReasonsResponse_Group1);
            TestHelper.Assert.IsTrue(GetReasonsResponse_Group1.Value.data.Length != 0);
            TestHelper.Assert.Equals(GetReasonsResponse_Group1.Value.data.Length, 2);
            TestHelper.Assert.IsResultOk(GetReasonsResponse_Group2);
            TestHelper.Assert.IsTrue(GetReasonsResponse_Group2.Value.data.Length == 0);
        }

        [UnityTest, TestLog, Order(4), Timeout(150000)]
        public IEnumerator SubmitReport()
        {
            String reasonGroup = "";
            // Get Reason with empty ReasonGroup
            Result<ReportingReasonsResponse> GetReasonsResponse = null;
            this.reporting.GetReasons(reasonGroup, result => { GetReasonsResponse = result; });
            yield return TestHelper.WaitForValue(() => GetReasonsResponse);
            TestHelper.Assert.IsResultOk(GetReasonsResponse);
            TestHelper.LogResult(GetReasonsResponse, "Get Reasons response");
            TestHelper.Assert.IsTrue(GetReasonsResponse.Value.data.Length != 0);

            ReportingSubmitData submitData = new ReportingSubmitData();
            Dictionary<string, string> additionalInfo = new Dictionary<string, string>();
            additionalInfo.Add("Prop1", "Additional Property 1");
            additionalInfo.Add("Prop2", "Additional Property 2");
            submitData.additionalInfo = additionalInfo;
            submitData.category = ReportingCategory.UGC;
            submitData.comment = "AdditionalComment";
            submitData.objectId = "";
            submitData.objectType = "";
            submitData.reason = GetReasonsResponse.Value.data[0].title;
            submitData.userId = this.user2.Session.UserId;

            Result<ReportingSubmitResponse> SubmitReportResponse = null;
            this.reporting.SubmitReport(submitData, result => SubmitReportResponse = result);
            yield return TestHelper.WaitForValue(() => SubmitReportResponse);
            TestHelper.Assert.IsResultOk(SubmitReportResponse);
            TestHelper.LogResult(GetReasonsResponse, "Submit Report");
        }
    }
}