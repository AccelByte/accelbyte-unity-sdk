// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
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
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class AgreementTest
    {
        struct AgreementTestUserInfo
        {
            public const string EmailAddress = "Agreement_UnitySDKTest@example.com";
            public const string Password = "Password+123";
            public const string CountryCode = "MG";
            public const string DisplayName = "AgreeMan";
            public static string UserId;
        };

        static string policyId;
        static readonly string[] policyTags = {"tag1", "tag2"};

        static readonly User user = AccelBytePlugin.GetUser();
        static readonly Agreement agreement = AccelBytePlugin.GetAgreement();

        static readonly TestHelper testHelper = new TestHelper();
        static string accessToken;

        [UnityTest, TestLog, Order(0), Timeout(120000)]
        public IEnumerator Setup()
        {
            // GET ACCESS TOKEN FOR TEST HELPER
            accessToken = null;
            Result<TokenData> getAccessToken = null;
            testHelper.GetAccessToken(result => { getAccessToken = result; });
            yield return TestHelper.WaitForValue(() => getAccessToken);
            
            TestHelper.LogResult(getAccessToken, "Get access token.");
            accessToken = getAccessToken.Value.access_token;

            #region User Setup
            // CLEAN UP PREVIOUS USER
            Result deleteResult = null;
            testHelper.DeleteUserByDisplayName(AgreementTestUserInfo.DisplayName, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete user");

            // USER CREATION
            DateTime dateOfBirth = DateTime.Now.AddYears(-22);

            Result<RegisterUserResponse> registerResult = null;
            user.Register(
                AgreementTestUserInfo.EmailAddress,
                AgreementTestUserInfo.Password,
                AgreementTestUserInfo.DisplayName,
                AgreementTestUserInfo.CountryCode,
                dateOfBirth,
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            TestHelper.LogResult(registerResult, "Register" + AgreementTestUserInfo.EmailAddress);

            TestHelper.Assert.IsResultOk(registerResult);
            #endregion

            Result<TestHelper.AgreementBasePolicy[]> agreementBasePoliciesResult = null;
            testHelper.AgreementGetBasePolicies(accessToken, result => agreementBasePoliciesResult = result);
            yield return TestHelper.WaitForValue(() => agreementBasePoliciesResult);

            TestHelper.LogResult(agreementBasePoliciesResult, "Get Base Policies");

            TestHelper.AgreementBasePolicy agreementBasePolicy = new TestHelper.AgreementBasePolicy();
            foreach (var policy in agreementBasePoliciesResult.Value)
            {
                if (policy.namespace_ == AccelBytePlugin.Config.Namespace)
                {
                    agreementBasePolicy = policy;
                }
            }

            if (string.IsNullOrEmpty(agreementBasePolicy.id))
            {
                Result<TestHelper.AgreementPolicyTypeObject[]> policyTypes = null;
                testHelper.AgreementGetPolicyTypes(accessToken, result => policyTypes = result);
                yield return TestHelper.WaitForValue(() => policyTypes);

                TestHelper.LogResult(policyTypes, "Get policy types");

                string policyTypeId = null;
                foreach (var type in policyTypes.Value)
                {
                    if (type.policyTypeName.Contains("Legal Document"))
                    {
                        policyTypeId = type.id;
                        break;
                    }
                }

                Result<TestHelper.AgreementBasePolicy> agreementBasePolicyResult = null;
                TestHelper.AgreementBasePolicyCreate createRequest = new TestHelper.AgreementBasePolicyCreate
                {
                    basePolicyName = "SDK Test Policy",
                    affectedCountries = new string[] { AgreementTestUserInfo.CountryCode },
                    affectedClientIds = new string[] { AccelBytePlugin.Config.ClientId },
                    tags = policyTags,
                    typeId = policyTypeId,
                    isMandatory = true,
                    namespace_ = AccelBytePlugin.Config.Namespace
                };

                testHelper.AgreementCreateBasePolicy(accessToken, createRequest, result => agreementBasePolicyResult = result);
                yield return TestHelper.WaitForValue(() => agreementBasePolicyResult);

                TestHelper.Assert.IsResultOk(agreementBasePolicyResult, "Create Base Policy");
                agreementBasePolicy = agreementBasePolicyResult.Value;
            }

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(agreementBasePolicy.id));

            foreach (var policy in agreementBasePolicy.policies)
            {
                if (policy.countryCode == AgreementTestUserInfo.CountryCode)
                {
                    policyId = policy.id;
                    break;
                }
            }

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(policyId));

            Result<TestHelper.AgreementCountryPolicy> countryPolicy = null;
            testHelper.AgreementGetCountryBasePolicy(
                accessToken,
                agreementBasePolicy.id,
                AgreementTestUserInfo.CountryCode,
                result => countryPolicy = result);
            yield return TestHelper.WaitForValue(() => countryPolicy);

            TestHelper.LogResult(countryPolicy, "Get country base policy");
            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(countryPolicy.Value.id));

            TestHelper.AgreementPolicyVersion policyVersion = null;
            Result<TestHelper.AgreementPolicyVersion> policyVersionResult = null;
            bool isPolicyPublished = false;

            if (countryPolicy.Value.policyVersions.Length == 0)
            {
                TestHelper.AgreementPolicyVersionCreate createRequest = new TestHelper.AgreementPolicyVersionCreate
                {
                    displayVersion = "1.0.0",
                    isCommitted = true,
                    description = "SDK Policy Test"
                };

                testHelper.AgreementCreatePolicyVersion(
                    accessToken,
                    policyId,
                    createRequest,
                    result => policyVersionResult = result);
                yield return TestHelper.WaitForValue(() => policyVersionResult);

                TestHelper.LogResult(policyVersionResult, "Create policy version");
                policyVersion = policyVersionResult.Value;
            }
            else
            {
                foreach (var policy in countryPolicy.Value.policyVersions)
                {
                    if (policy.isInEffect)
                    {
                        isPolicyPublished = true;
                        policyVersion = policy;
                        break;
                    }
                }

                // no active policy
                if (!isPolicyPublished)
                {
                    policyVersion = countryPolicy.Value.policyVersions[0];
                }
            }

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(policyVersion.id));

            Result<TestHelper.AgreementLocalizedPolicy[]> agreementLocalizedPoliciesResult = null;
            testHelper.AgreementGetLocalizedPolicies(
                accessToken,
                policyVersion.id,
                result => agreementLocalizedPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => agreementLocalizedPoliciesResult);

            TestHelper.LogResult(agreementLocalizedPoliciesResult, "Get local policies");

            if (agreementLocalizedPoliciesResult.Value.Length == 0)
            {
                Result<TestHelper.AgreementLocalizedPolicy> localizedPolicy = null;
                TestHelper.AgreementLocalizedPolicyCreate createRequest = new TestHelper.AgreementLocalizedPolicyCreate
                {
                    localeCode = "en", contentType = "text/plain", description = "SDK Policy Test"
                };

                testHelper.AgreementCreateLocalizedPolicy(
                    accessToken,
                    policyVersion.id,
                    createRequest,
                    result => localizedPolicy = result);
                yield return TestHelper.WaitForValue(() => localizedPolicy);

                TestHelper.Assert.IsResultOk(localizedPolicy, "Create local policy");
            }

            if (!isPolicyPublished)
            {
                Result publishResult = null;
                testHelper.AgreementPublishPolicyVersion(
                    accessToken,
                    policyVersion.id,
                    false,
                    result => publishResult = result);
                yield return TestHelper.WaitForValue(() => publishResult);

                TestHelper.Assert.IsResultOk(publishResult, "Publish policy version");
            }

            Result loginResult = null;
            AgreementTest.user.LoginWithUsername(
                AgreementTestUserInfo.EmailAddress,
                AgreementTestUserInfo.Password,
                result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login with " + AgreementTestUserInfo.EmailAddress);
        }

        [UnityTest, TestLog, Order(int.MaxValue), Timeout(300000)]
        public IEnumerator Teardown()
        {
            if (!string.IsNullOrEmpty(user.Session.AuthorizationToken))
            {
                Result logoutResult = null;
                user.Logout(result => logoutResult = result);
                yield return TestHelper.WaitForValue(() => logoutResult);

                TestHelper.LogResult(logoutResult, "Logout with " + AgreementTestUserInfo.EmailAddress);
            }

            if (!string.IsNullOrEmpty(AgreementTestUserInfo.UserId))
            {
                Result deleteResult = null;
                testHelper.DeleteUser(AgreementTestUserInfo.UserId, result => deleteResult = result);
                yield return TestHelper.WaitForValue(() => deleteResult);

                TestHelper.Assert.IsResultOk(deleteResult, "Delete user");
            }
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetLegalPoliciesByCountryWithoutTags()
        {
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPoliciesByCountry(
                AgreementTestUserInfo.CountryCode,
                AgreementPolicyType.EMPTY,
                false,
                result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.Assert.IsResultOk(publicPoliciesResult, "Get legal policies by country without tag");
            TestHelper.Assert.That(publicPoliciesResult.Value, Is.Not.Null);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].countryCode == AgreementTestUserInfo.CountryCode);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetLegalPoliciesByCountryWithSomeTags()
        {
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPoliciesByCountry(
                AgreementTestUserInfo.CountryCode,
                AgreementPolicyType.EMPTY,
                policyTags,
                false,
                result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.Assert.IsResultOk(publicPoliciesResult, "Get legal policies by country using some tags");
            TestHelper.Assert.That(publicPoliciesResult.Value, Is.Not.Null);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].countryCode == AgreementTestUserInfo.CountryCode);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetLegalPoliciesByCountryWithOneTag()
        {
            string[] tags = new string[] {policyTags[0]};
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPoliciesByCountry(
                AgreementTestUserInfo.CountryCode,
                AgreementPolicyType.EMPTY,
                policyTags,
                false,
                result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.Assert.IsResultOk(publicPoliciesResult, "Get legal policies by country using one tag");
            TestHelper.Assert.That(publicPoliciesResult.Value, Is.Not.Null);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].countryCode == AgreementTestUserInfo.CountryCode);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetLegalPoliciesByCountryWithUnmatchTag()
        {
            string[] tags = new string[] {"tags3"};
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPoliciesByCountry(
                AgreementTestUserInfo.CountryCode,
                AgreementPolicyType.EMPTY,
                tags,
                false,
                result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.Assert.IsResultOk(publicPoliciesResult, "Get legal policies by country using unmatch tag");
            TestHelper.Assert.That(publicPoliciesResult.Value, Is.Empty);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetLegalPoliciesAndBulkAcceptPolicyVersions()
        {
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPolicies(AgreementPolicyType.EMPTY, false, result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.LogResult(publicPoliciesResult, "Get legal policies");

            TestHelper.Assert.That(publicPoliciesResult.Value, Is.Not.Null);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].Namespace == AccelBytePlugin.Config.Namespace);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].countryCode == AgreementTestUserInfo.CountryCode);

            List<AcceptAgreementRequest> acceptAgreementRequests = new List<AcceptAgreementRequest>();
            foreach (var policy in publicPoliciesResult.Value)
            {
                foreach (var policyVersion in policy.policyVersions)
                {
                    if (policyVersion.isInEffect)
                    {
                        AcceptAgreementRequest acceptAgreementRequest = new AcceptAgreementRequest
                        {
                            isAccepted = true,
                            policyId = policy.id,
                            policyVersionId = policyVersion.id,
                            localizedPolicyVersionId = policyVersion.localizedPolicyVersions[0].id
                        };
                        acceptAgreementRequests.Add(acceptAgreementRequest);
                    }
                }
            }

            TestHelper.Assert.That(acceptAgreementRequests.Count != 0);

            Result<AcceptAgreementResponse> acceptAgreementResponseResult = null;
            agreement.BulkAcceptPolicyVersions(
                acceptAgreementRequests.ToArray(),
                result => acceptAgreementResponseResult = result);
            yield return TestHelper.WaitForValue(() => acceptAgreementResponseResult);

            TestHelper.LogResult(acceptAgreementResponseResult, "Bulk accept legal policy versions");

            TestHelper.Assert.IsResultOk(publicPoliciesResult);
            TestHelper.Assert.IsResultOk(acceptAgreementResponseResult);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetLegalPoliciesAndAcceptPolicyVersion()
        {
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPolicies(AgreementPolicyType.EMPTY, false, result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.LogResult(publicPoliciesResult, "Get legal policies");

            string localizedPolicyVersionId = null;
            foreach (var policy in publicPoliciesResult.Value)
            {
                foreach (var policyVersion in policy.policyVersions)
                {
                    if (policyVersion.isInEffect)
                    {
                        localizedPolicyVersionId = policyVersion.localizedPolicyVersions[0].id;
                        break;
                    }
                }
            }

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(localizedPolicyVersionId));

            Result acceptPolicyVersionResult = null;
            agreement.AcceptPolicyVersion(localizedPolicyVersionId, result => acceptPolicyVersionResult = result);
            yield return TestHelper.WaitForValue(() => acceptPolicyVersionResult);

            TestHelper.LogResult(acceptPolicyVersionResult, "Accept policy version");

            TestHelper.Assert.IsResultOk(publicPoliciesResult);
            TestHelper.Assert.IsResultOk(acceptPolicyVersionResult);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetPublisherLegalPoliciesAndGetTheContent()
        {
            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPolicies(
                testHelper.GetPublisherNamespace(),
                AgreementPolicyType.EMPTY,
                true,
                result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.LogResult(publicPoliciesResult, "Get legal policies");

            string localizedPolicyVersionId = null;
            string localizedPolicyUrl = null;
            foreach (var policy in publicPoliciesResult.Value)
            {
                foreach (var policyVersion in policy.policyVersions)
                {
                    if (policyVersion.isInEffect)
                    {
                        localizedPolicyVersionId = policyVersion.localizedPolicyVersions[0].id;
                        localizedPolicyUrl = policyVersion.localizedPolicyVersions[0].attachmentLocation;
                        break;
                    }
                }
            }

            Result<string> docResult = null;
            agreement.GetLegalDocument(localizedPolicyUrl, result => { docResult = result; });

            while (docResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            TestHelper.Assert.IsFalse(docResult.IsError);

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(docResult.Value));

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(localizedPolicyVersionId));

            TestHelper.Assert.IsResultOk(publicPoliciesResult);
        }

        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator CheckUserEligibilities_NotComply()
        {
            AgreementTestUserInfo.UserId = AgreementTest.user.Session.UserId;

            Result<RetrieveUserEligibilitiesResponse[]> eligibilitiesResult = null;
            agreement.QueryLegalEligibilities(AccelBytePlugin.Config.Namespace, result => eligibilitiesResult = result);

            while (eligibilitiesResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            TestHelper.LogResult(eligibilitiesResult, "Get Eligibilities!");

            bool isComply = false;
            foreach (var eligibility in eligibilitiesResult.Value)
            {
                if (eligibility.policyId == AgreementTest.policyId)
                {
                    if (eligibility.isAccepted)
                    {
                        isComply = true;
                    }

                    break;
                }
            }

            Assert.IsFalse(eligibilitiesResult.IsError);
            Assert.IsFalse(isComply);
        }

        [UnityTest, TestLog, Order(3), Timeout(100000)]
        public IEnumerator CheckUserEligibilities_AlreadyComply()
        {
            Result<RetrieveUserEligibilitiesResponse[]> eligibilitiesResult = null;
            agreement.QueryLegalEligibilities(AccelBytePlugin.Config.Namespace, result => eligibilitiesResult = result);
            yield return TestHelper.WaitForValue(() => eligibilitiesResult);

            TestHelper.LogResult<RetrieveUserEligibilitiesResponse[]>(eligibilitiesResult, "Get Eligibilities!");

            bool isComply = false;
            foreach (var eligibility in eligibilitiesResult.Value)
            {
                if (eligibility.policyId == AgreementTest.policyId)
                {
                    if (eligibility.isAccepted)
                    {
                        isComply = true;
                    }

                    break;
                }
            }

            Assert.IsFalse(eligibilitiesResult.IsError);
            Assert.IsTrue(isComply);
        }

        [UnityTest, TestLog, Timeout(150000)]
        public IEnumerator RegisterWithEmailAndAcceptPolicies_ThenLogin_Success()
        {
            var helper = new TestHelper();

            Result<PublicPolicy[]> publicPoliciesResult = null;
            agreement.GetLegalPoliciesByCountry(
                "MG",
                AgreementPolicyType.EMPTY,
                false,
                result => publicPoliciesResult = result);
            yield return TestHelper.WaitForValue(() => publicPoliciesResult);

            TestHelper.LogResult(publicPoliciesResult, "Get legal policies");

            TestHelper.Assert.That(publicPoliciesResult.Value, Is.Not.Null);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].Namespace == AccelBytePlugin.Config.Namespace);
            TestHelper.Assert.That(publicPoliciesResult.Value[0].countryCode == AgreementTestUserInfo.CountryCode);

            List<PolicyAcceptance> acceptedPolicies = (from policy in publicPoliciesResult.Value
                from policyVersion in policy.policyVersions
                where policyVersion.isInEffect
                select new PolicyAcceptance
                {
                    isAccepted = true,
                    policyId = policy.id,
                    policyVersionId = policyVersion.id,
                    localizedPolicyVersionId = policyVersion.localizedPolicyVersions[0].id
                }).ToList();

            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            string username = $"sdk{Guid.NewGuid():N}";
            string email = $"testeraccelbyte+{username}@gmail.com";
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register With Email: {0}, {1}", email, password));
            RegisterUserRequestv2 registerRequest = new RegisterUserRequestv2
            {
                emailAddress = email,
                password = password,
                username = username,
                displayName = "testersdk",
                country = "MG",
                dateOfBirth = DateTime.Now.AddYears(-22).ToString("yyyy-MM-dd"),
                acceptedPolicies = acceptedPolicies
            };
            user.RegisterAndAcceptPolicies(registerRequest, result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result<UserData> getDataResult = null;
            user.GetData(result => getDataResult = result);
            yield return TestHelper.WaitForValue(() => getDataResult);

            TestHelper.LogResult(getDataResult, "Get User Data");
            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(getDataResult.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);
        }
    }
}