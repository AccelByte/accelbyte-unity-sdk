// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using Tests.IntegrationTests;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    class Given_a_GameClient
    {
        private GameClient gameClient;

        [SetUp]
        public void SetUp()
        {
            Config config = new Config
            {
                BaseUrl = "https://ab.example.io/",
                Namespace = "fakenamespace",
                RedirectUri = "http://127.0.0.1:3456/"
            };

            this.gameClient = new GameClient(config, new AccelByteHttpClient(new FakeIam()));
        }

        [Test, TestLog]
        public void When_constructed_with_null_parameters__Then_throws_exception()
        {
            Assert.Throws<ArgumentException>(() => new HttpApiContainer(null));
        }

        class EmptyHttpApi : HttpApiBase
        {
        }

        [Test, TestLog]
        public void When_get_HttpApi_without_configure__Then_throws_exception()
        {
            Assert.Throws<ArgumentException>(() => this.gameClient.GetHttpApi<EmptyHttpApi>());
        }

        [Test, TestLog]
        public void When_configure_a_HttpApi_class_twice__Then_throws_exception()
        {
            this.gameClient.ConfigureHttpApi<EmptyHttpApi>();

            Assert.Throws<InvalidOperationException>(() => this.gameClient.ConfigureHttpApi<EmptyHttpApi>());
        }

        [Test, TestLog]
        public void When_get_a_HttpApi_class_after_configure__Then_returns_non_null_object__And_Http_is_not_null()
        {
            this.gameClient.ConfigureHttpApi<EmptyHttpApi>();

            var api = this.gameClient.GetHttpApi<EmptyHttpApi>();

            Assert.NotNull(api);
            Assert.NotNull(api.Http);
        }

        [Test, TestLog]
        public void When_call_GetHttpApi_with_the_same_type_repeatedly__Then_should_return_the_same_object()
        {
            this.gameClient.ConfigureHttpApi<EmptyHttpApi>();

            var api1 = this.gameClient.GetHttpApi<EmptyHttpApi>();
            var api2 = this.gameClient.GetHttpApi<EmptyHttpApi>();
            var api3 = this.gameClient.GetHttpApi<EmptyHttpApi>();

            Assert.NotNull(api1);
            Assert.NotNull(api2);
            Assert.NotNull(api3);
            Assert.AreEqual(api1, api2);
            Assert.AreEqual(api2, api3);
        }

        [Test, TestLog]
        public void When_set_Http_to_another_instance__Then_throws_exception()
        {
            this.gameClient.ConfigureHttpApi<EmptyHttpApi>();

            var api1 = this.gameClient.GetHttpApi<EmptyHttpApi>();

            Assert.Throws<InvalidOperationException>(() => api1.Http = null);
        }

        [Test, TestLog]
        public void When_configure_HttpApi_with_mismatched_constructor_parameters__Then_throws_exception()
        {
            Assert.Catch<Exception>(() => this.gameClient.ConfigureHttpApi<EmptyHttpApi>("param1"));
        }

        class ApiWithConstructorParameters : HttpApiBase
        {
            public readonly string param;

            public ApiWithConstructorParameters(string param1, string param2, string param3)
            {
                this.param = param1 + param2 + param3;
            }
        }

        [Test, TestLog]
        public void When_configure_a_HttpApi_with_multiple_constructor_parameters__Then_parameters_forwarded()
        {
            this.gameClient.ConfigureHttpApi<ApiWithConstructorParameters>("param1", "param2", "param3");

            var api = this.gameClient.GetHttpApi<ApiWithConstructorParameters>();

            Assert.AreEqual("param1param2param3", api.param);
        }

        [Test, TestLog]
        public void When_get_two_Apis_with_different_types__Then_share_the_same_HttpClient()
        {
            this.gameClient.ConfigureHttpApi<EmptyHttpApi>();
            this.gameClient.ConfigureHttpApi<ApiWithConstructorParameters>("ji", "ro", "lu");

            var api1 = this.gameClient.GetHttpApi<EmptyHttpApi>();
            var api2 = this.gameClient.GetHttpApi<ApiWithConstructorParameters>();

            Assert.NotNull(api1?.Http);
            Assert.NotNull(api2?.Http);
            Assert.AreEqual(api1.Http, api2.Http);
        }

        class SimpleMockApi : HttpApiBase
        {
            public IEnumerator GetResource(ResultCallback<Data> callback)
            {
                return this.Http.Get("iam/{namespace}/res", callback);
            }
        }

        [UnityTest, TestLog]
        public IEnumerator When_session_is_logged_in__Then_custom_Api_is_authorized()
        {
            this.gameClient.ConfigureHttpApi<SimpleMockApi>();
            var api = this.gameClient.GetHttpApi<SimpleMockApi>();

            Result loginResult = null;
            yield return this.gameClient.Session.LoginWithDeviceId(r => loginResult = r);

            Result<Data> getResourceResult = null;
            yield return api.GetResource(r => getResourceResult = r);

            Assert.NotNull(getResourceResult);
            Assert.False(getResourceResult.IsError);
            Assert.NotNull(getResourceResult.Value);
        }
    }
}