using System;
using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using Tests.IntegrationTests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class SessionTest
    {
        [UnityTest, TestLog]
        public IEnumerator LoginWithDeviceId_OnExpiring_AccessTokenRefreshed()
        {
            const string expectedFirstAccessToken = "first_access_token";
            const string expectedSecondAccessToken = "second_access_token";
            const string expectedThirdAccessToken = "third_access_token";
            const string expectedLastAccessToken = "last_access_token";

            var loginSession = new LoginSession(
                "https://ab.example.io/iam",
                "fakenamespace",
                "some_unimportant_redirect_uri",
                new AccelByteHttpClient(new FakeIam()),
                new CoroutineRunner());

            Result loginResult = null;

            yield return loginSession.LoginWithDeviceId(r => loginResult = r);

            var actualFirstAccessToken = loginSession.AuthorizationToken;

            yield return new WaitForSeconds(2.0f);

            var actualSecondAccessToken = loginSession.AuthorizationToken;

            yield return new WaitForSeconds(2.0f);

            var actualThirdAccessToken = loginSession.AuthorizationToken;

            yield return new WaitForSeconds(1.6f);

            var actualLastAccessToken = loginSession.AuthorizationToken;

            Assert.That(expectedFirstAccessToken, Is.EqualTo(actualFirstAccessToken));
            Assert.That(expectedSecondAccessToken, Is.EqualTo(actualSecondAccessToken));
            Assert.That(expectedThirdAccessToken, Is.EqualTo(actualThirdAccessToken));
            Assert.That(expectedLastAccessToken, Is.EqualTo(actualLastAccessToken));
        }

        [UnityTest, TestLog]
        public IEnumerator LoginWithDeviceId_AccessRestrictedData_Authorized()
        {
            var client = new AccelByteHttpClient(new FakeIam());
            client.SetBaseUri(new Uri("https://ab.example.io"));
            client.SetCredentials("some_random_clientId", "some_random_clientSecret");

            var loginSession = new LoginSession(
                "https://ab.example.io/iam",
                "fakenamespace",
                "https://some_unimportant_redirect_uri",
                client,
                new CoroutineRunner());

            Result loginResult = null;
            yield return loginSession.LoginWithDeviceId(r => loginResult = r);

            Result<Data> result = null;
            yield return client.Get("iam/{namespace}/res", (Result<Data> r) => result = r);

            Assert.NotNull(result);
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
        }

        [UnityTest, TestLog]
        public IEnumerator NotLoggedIn_AccessRestrictedData_Unauthorized()
        {
            var client = new AccelByteHttpClient(new FakeIam());
            client.SetBaseUri(new Uri("https://ab.example.io"));
            client.SetCredentials("some_random_clientId", "some_random_clientSecret");

            var loginSession = new LoginSession(
                "https://ab.example.io/iam",
                "fakenamespace",
                "https://some_unimportant_redirect_uri",
                client,
                new CoroutineRunner());

            Result<Data> result = null;
            yield return client.Get("iam/fakenamespace/res", (Result<Data> r) => result = r);

            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.AreEqual((ErrorCode)2357, result.Error.Code);
        }

        [UnityTest, TestLog]
        public IEnumerator Logout_AccessRestrictedData_Unauthorized()
        {
            var client = new AccelByteHttpClient(new FakeIam());
            client.SetBaseUri(new Uri("https://ab.example.io"));
            client.SetCredentials("some_random_clientId", "some_random_clientSecret");

            var loginSession = new LoginSession(
                "https://ab.example.io/iam",
                "fakenamespace",
                "https://some_unimportant_redirect_uri",
                client,
                new CoroutineRunner());

            Result loginResult = null;
            yield return loginSession.LoginWithDeviceId(r => loginResult = r);

            yield return loginSession.Logout(null);

            Result<Data> result = null;
            yield return client.Get("iam/fakenamespace/res", (Result<Data> r) => result = r);

            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.AreEqual((ErrorCode)2357, result.Error.Code);
        }

        [UnityTest, TestLog]
        public IEnumerator Logout_AccessPublicDataWithImplicitPathParams_NotUnauthorized()
        {
            var client = new AccelByteHttpClient(new FakeIam());
            client.SetBaseUri(new Uri("https://ab.example.io"));
            client.SetCredentials("some_random_clientId", "some_random_clientSecret");

            var loginSession = new LoginSession(
                "https://ab.example.io/iam",
                "fakenamespace",
                "https://some_unimportant_redirect_uri",
                client,
                new CoroutineRunner());

            Result loginResult = null;
            yield return loginSession.LoginWithDeviceId(r => loginResult = r);

            yield return loginSession.Logout(null);

            Result<Data> result = null;
            yield return client.Get("iam/{namespace}/res2", (Result<Data> r) => result = r);

            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.AreNotEqual((ErrorCode)2357, result.Error.Code);
        }
    }
}