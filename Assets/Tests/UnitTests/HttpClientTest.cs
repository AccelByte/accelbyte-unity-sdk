// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using Tests.IntegrationTests;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class HttpClientTest
    {
        [TestFixture(Description = "Given a AccelByteHttpClient")]
        public class Given_a_AccelByteHttpClient
        {
            private IHttpClient client;
            private Result nothing;
            private Result<Data> data;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.nothing = null;
                this.data = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_GET_without_parameter__Then_should_return_data()
            {
                yield return this.client.Get<Data>("https://ab.example.io/path/noauth", r => this.data = r);

                AssertNoError(this.data);
            }

            [UnityTest, TestLog]
            public IEnumerator When_GET_with_query_parameters__Then_should_return_data()
            {
                string url = "https://ab.example.io/path/noauth";
                var queries = new { userId = "ab12", offset = 36, limit = 12 };

                yield return this.client.Get(url, queries, (Result<Data> r) => this.data = r);

                AssertNoError(this.data);
            }

            [UnityTest, TestLog]
            public IEnumerator When_POST_with_body__And_expect_no_reponse_body__Then_should_return_nothing()
            {
                string url = "https://ab.example.io/path/noauth";

                yield return this.client.Post(url, new Data(), r => this.nothing = r);

                AssertNoError(this.nothing);
            }

            [UnityTest, TestLog]
            public IEnumerator When_POST_with_body__And_expect_response_body__Then_should_return_data()
            {
                string url = "https://ab.example.io/path/noauth/echo";

                yield return this.client.Post<Data, Data>(url, new Data(), r => this.data = r);

                AssertNoError(this.data);
            }

            [UnityTest, TestLog]
            public IEnumerator When_POST_form__And_expect_no_response_body__Then_should_return_nothing()
            {
                string url = "https://ab.example.io/path/noauth/formEmpty";
                var form = new { userId = "ab12", offset = 36, limit = 12 };

                yield return this.client.PostForm(url, form, r => this.nothing = r);

                AssertNoError(this.nothing);
            }

            [UnityTest, TestLog]
            public IEnumerator When_POST_form__And_expect_response_body__Then_should_return_data()
            {
                string url = "https://ab.example.io/path/noauth/formValue";
                var form = new { userId = "ab12", offset = 36, limit = 12 };

                yield return this.client.PostForm(url, form, (Result<Data> r) => this.data = r);

                AssertNoError(this.data);
            }

            [UnityTest, TestLog]
            public IEnumerator When_PUT_with_body__And_expect_response_body__Then_should_return_data()
            {
                string url = "https://ab.example.io/path/noauth";

                yield return this.client.Put<Data, Data>(url, new Data(), r => this.data = r);

                AssertNoError(this.data);
            }

            [UnityTest, TestLog]
            public IEnumerator When_PATCH_with_body__And_expect_response_body__Then_should_return_data()
            {
                string url = "https://ab.example.io/path/noauth";

                yield return this.client.Patch<Data, Data>(url, new Data(), r => this.data = r);

                AssertNoError(this.data);
            }

            [UnityTest, TestLog]
            public IEnumerator When_DELETE__Then_should_return_nothing()
            {
                string url = "https://ab.example.io/path/noauth";

                yield return this.client.Delete(url, r => this.nothing = r);

                AssertNoError(this.nothing);
            }

            [UnityTest, TestLog]
            public IEnumerator When_DELETE_with_body__Then_should_return_nothing()
            {
                string url = "https://ab.example.io/path/noauth";

                yield return this.client.Delete(url, new Data(), r => this.nothing = r);

                AssertNoError(this.nothing);
            }
        }

        [TestFixture]
        public class Given_a_AccelByteHttpClient_With_BaseUri_not_set
        {
            private IHttpClient client;
            private Result<Data> result;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_url_is_relative__Then_should_return_error()
            {
                yield return this.client.Put<Data, Data>("/path", new Data(), r => this.result = r);

                AssertHasError(this.result);
            }

            [TestCase("http", ExpectedResult = null)]
            [TestCase("https", ExpectedResult = null)]
            [UnityTest, TestLog]
            public IEnumerator When_url_is_absolute__And_scheme_is_http_or_https__Then_should_return_data(string scheme)
            {
                yield return this.client.Put<Data, Data>(
                    $"{scheme}://ab.example.io/path",
                    new Data(),
                    r => this.result = r);

                AssertNoError(this.result);
            }

            [TestCase("file", ExpectedResult = null)]
            [TestCase("ftp", ExpectedResult = null)]
            [TestCase("telnet", ExpectedResult = null)]
            [UnityTest, TestLog]
            public IEnumerator When_url_is_absolute__And_scheme_is_not_http_or_https__Then_should_error(string scheme)
            {
                yield return this.client.Put(
                    $"{scheme}://ab.example.io/path",
                    new Data(),
                    (Result<Data> r) => this.result = r);

                AssertHasError(this.result);
            }
        }

        [TestFixture]
        public class Given_a_AccelByteHttpClient_with_BaseUri_Set
        {
            private IHttpClient client;
            private Result<Data> result;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.client.SetBaseUri(new Uri("https://ab.example.io"));
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_url_is_relative__Then_should_combine_BaseUri_and_request_url()
            {
                yield return this.client.Put("/path", new Data(), (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_url_is_absolute__Then_should_not_change_request_url()
            {
                yield return this.client.Put(
                    "https://ab.example.io/path",
                    new Data(),
                    (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }
        }

        [TestFixture]
        public class Given_a_AccelByteHttpClient_with_no_implicit_credentials_set
        {
            private IHttpClient client;
            private Result<Data> result;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_no_auth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_BasicAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_BearerAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_no_auth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BasicAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_no_auth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BasicAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BearerAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBearerAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }
        }

        [TestFixture]
        public class Given_a_AccelByteHttpClient_with_client_credentials_set
        {
            private IHttpClient client;
            private Result<Data> result;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.client.SetCredentials("username", "password");
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_no_auth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_BasicAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_BearerAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithBearerAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_no_auth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BasicAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_wrong_BasicAuth_specified__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBasicAuth("username", "invalid_password")
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BearerAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBearerAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_no_auth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BasicAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BasicAuth_specified__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBasicAuth("username", "password")
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BearerAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBearerAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }
        }

        [TestFixture]
        public class Given_a_AccelByteHttpClient_with_public_client_credentials_set
        {
            private IHttpClient client;
            private Result<Data> result;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.client.SetCredentials("username", "");
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BasicAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePost(FakeService.BasicUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }
        }


        [TestFixture]
        public class Given_a_AccelByteHttpClient_with_client_credentials_and_implicit_bearer_token_set
        {
            private IHttpClient client;
            private Result<Data> result;

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.client.SetCredentials("username", "password");
                this.client.SetImplicitBearerAuth("AuthorizedAccessToken");
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_no_auth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_BasicAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.NoAuthUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_auth_required__And_BearerAuth_implied__Then_return_no_error()
            {
                yield return this.client.Put<Data, Data>(FakeService.NoAuthUrl, new Data(), r => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_NoAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BasicAuth_implied__Then_return_no_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_wrong_BasicAuth_specified__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBasicAuth("username", "invalid password")
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BearerAuth_implied__Then_return_auth_error()
            {
                yield return this.client.Put<Data, Data>(FakeService.BasicUrl, new Data(), r => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BasicAuth_required__And_BearerAuth_specified__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BasicUrl)
                    .WithBearerAuth("AuthorizedAccessToken")
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_no_auth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BasicAuth_implied__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBasicAuth()
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_BearerAuth_implied__Then_return_no_error()
            {
                yield return this.client.Put<Data, Data>(FakeService.BearerUrl, new Data(), r => this.result = r);

                AssertNoError(this.result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_BearerAuth_required__And_wrong_BearerAuth_specified__Then_return_auth_error()
            {
                IHttpRequest request = HttpRequestBuilder.CreatePut(FakeService.BearerUrl)
                    .WithBearerAuth("WrongUnauthorizedAccessToken")
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertAuthError(this.result);
            }
        }

        public class Given_a_url_with_path_parameters
        {
            private IHttpClient client;
            private Result<Data> result;
            private string urlWithPathParams = "https://ab.example.io/{param1}/{param2}";

            [SetUp]
            public void SetUp()
            {
                this.client = new AccelByteHttpClient(new FakeService());
                this.result = null;
            }

            [UnityTest, TestLog]
            public IEnumerator When_path_parameters_set__Then_implicit_path_parameters_is_not_used()
            {
                IHttpClient client = new AccelByteHttpClient(new FakeService());
                client.SetImplicitPathParams(
                    new Dictionary<string, string> { { "param1", "radong" }, { "param2", "mbuh" } });

                IHttpRequest request = HttpRequestBuilder.CreatePut(this.urlWithPathParams)
                    .WithPathParam("param1", "path")
                    .WithPathParam("param2", "noauth")
                    .WithJsonBody(new Data())
                    .GetResult();
                Result<Data> result = null;

                yield return client.Send(request, (Result<Data> r) => result = r);

                Assert.NotNull(result);
                Assert.False(result.IsError);
                Assert.NotNull(result.Value);
            }

            [UnityTest, TestLog]
            public IEnumerator When_no_path_parameters_set__Then_implicit_path_parameters_is_used()
            {
                var pathParams = new Dictionary<string, string> { { "param1", "path" }, { "param2", "noauth" } };
                this.client.SetImplicitPathParams(pathParams);

                IHttpRequest request = HttpRequestBuilder.CreatePut(this.urlWithPathParams)
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                AssertNoError(this.result);
            }
            
            [UnityTest, TestLog]
            public IEnumerator When_implicit_and_explicit_path_parameters_are_set__Then_both_are_used()
            {
                var pathParams = new Dictionary<string, string> { { "param1", "path" } };
                this.client.SetImplicitPathParams(pathParams);

                IHttpRequest request = HttpRequestBuilder.CreatePut(this.urlWithPathParams)
                    .WithPathParams(new Dictionary<string, string> { { "param2", "noauth" } })
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);

                HttpClientTest.AssertNoError(this.result);
            }

            private static void AssertValidationError(Result<Data> result)
            {
                Assert.NotNull(result);
                Assert.True(result.IsError);
                Assert.AreEqual(ErrorCode.InvalidRequest, result.Error.Code);
            }

            [UnityTest, TestLog]
            public IEnumerator When_path_parameters_are_set__And_a_parameter_unresolved__Then_return_validation_error()
            {
                this.client.SetImplicitPathParams(new Dictionary<string, string> { { "param1", "path" } });

                IHttpRequest request = HttpRequestBuilder.CreatePut(this.urlWithPathParams)
                    .WithPathParams(new Dictionary<string, string> { { "param3", "mbuh" } })
                    .WithJsonBody(new Data())
                    .GetResult();
                Result<Data> result = null;

                yield return this.client.Send(request, (Result<Data> r) => result = r);

                AssertValidationError(result);
            }

            [UnityTest, TestLog]
            public IEnumerator When_path_parameters_are_set__And_a_parameter_unresolved__And_url_is_relative__Then_return_validation_error()
            {
                this.client.SetBaseUri(new Uri("https://ab.example.io"));
                this.client.SetImplicitPathParams(new Dictionary<string, string> { { "param1", "path" } });

                IHttpRequest request = HttpRequestBuilder.CreatePut("/{param1}/{param2}")
                    .WithPathParams(new Dictionary<string, string> { { "param3", "mbuh" } })
                    .WithJsonBody(new Data())
                    .GetResult();

                yield return this.client.Send(request, (Result<Data> r) => this.result = r);
                
                AssertValidationError(this.result);
            }
        }

        private static void AssertAuthError(Result<Data> result)
        {
            Assert.NotNull(result);
            Assert.True(result.IsError);
            Assert.AreEqual(FakeService.AuthErrorCode, (int)result.Error.Code);
        }

        private static void AssertNoError(Result<Data> result)
        {
            Assert.NotNull(result);
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
        }

        private static void AssertNoError(Result result)
        {
            Assert.NotNull(result);
            Assert.False(result.IsError);
        }

        private static void AssertHasError(Result<Data> result)
        {
            Assert.NotNull(result);
            Assert.True(result.IsError);
        }
    }

    public class ChildData
    {
        public string ChildId = Guid.NewGuid().ToString();
        public int Age = 11;
        public DateTime LastMet = DateTime.Today.Subtract(TimeSpan.FromDays(7));
    }

    public class Data
    {
        public string Home = "Jogjakarta";

        public Dictionary<string, ChildData> Childs = new Dictionary<string, ChildData>
        {
            { "Andri", new ChildData { Age = 23 } },
            { "Bagas", new ChildData { Age = 19 } },
            { "Cika", new ChildData { Age = 17 } },
            { "Dita", new ChildData { Age = 13 } }
        };
    }

    internal class FakeService : IHttpRequestSender
    {
        public const string NoAuthUrl = "https://ab.example.io/path/noauth";
        public const string BasicUrl = "https://ab.example.io/path/basicauth";
        public const string BearerUrl = "https://ab.example.io/path/bearerauth";
        public const int AuthErrorCode = 2357;

        public IEnumerator Send(IHttpRequest request, Action<IHttpResponse, Error> callback, int timeoutMs)
        {
            yield return new WaitForSeconds(0.05f);

            string auth;
            switch (request.Method + " " + request.Url)
            {
            case "PUT http://ab.example.io/path":
                FakeService.RespondWithEcho(request, callback);

                break;
            case "PUT https://ab.example.io/path":
                FakeService.RespondWithEcho(request, callback);

                break;
            case "GET " + FakeService.NoAuthUrl:
                FakeService.RespondWithDefault(request, callback);

                break;
            case "GET " + FakeService.NoAuthUrl + "?userId=ab12&offset=36&limit=12":
                FakeService.RespondWithDefault(request, callback);

                break;
            case "POST " + FakeService.NoAuthUrl:
                FakeService.RespondWithEmpty(request, callback);

                break;
            case "POST " + FakeService.NoAuthUrl + "/echo":
                FakeService.RespondWithEcho(request, callback);

                break;
            case "POST " + FakeService.NoAuthUrl + "/formEmpty":
                if (Encoding.UTF8.GetString(request.BodyBytes) == "userId=ab12&offset=36&limit=12")
                {
                    FakeService.RespondWithEmpty(request, callback);
                }
                else
                {
                    FakeService.RespondWithError(request, 400, callback);
                }

                break;
            case "POST " + FakeService.NoAuthUrl + "/formValue":
                if (Encoding.UTF8.GetString(request.BodyBytes) == "userId=ab12&offset=36&limit=12")
                {
                    FakeService.RespondWithDefault(request, callback);
                }
                else
                {
                    FakeService.RespondWithError(request, 400, callback);
                }

                break;
            case "POST " + FakeService.BasicUrl:
                string base64PublicClientId = Convert.ToBase64String(Encoding.UTF8.GetBytes("username:"));
                if (request.Headers.TryGetValue("Authorization", out auth) && auth == "Basic " + base64PublicClientId)
                {
                    FakeService.RespondWithEcho(request, callback);
                }
                else
                {
                    FakeService.RespondWithAuthError(request, callback);
                }

                break;
                case "PUT " + FakeService.NoAuthUrl:
                FakeService.RespondWithEcho(request, callback);

                break;
            case "PATCH " + FakeService.NoAuthUrl:
                FakeService.RespondWithEcho(request, callback);

                break;
            case "DELETE " + FakeService.NoAuthUrl:
                FakeService.RespondWithEmpty(request, callback);

                break;
            case "PUT " + FakeService.BasicUrl:
                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("username:password"));
                if (request.Headers.TryGetValue("Authorization", out auth) && auth == "Basic " + base64)
                {
                    FakeService.RespondWithEcho(request, callback);
                }
                else
                {
                    FakeService.RespondWithAuthError(request, callback);
                }

                break;
            case "PUT " + FakeService.BearerUrl:
                string token = "AuthorizedAccessToken";
                if (request.Headers.TryGetValue("Authorization", out auth) && auth == "Bearer " + token)
                {
                    FakeService.RespondWithEcho(request, callback);
                }
                else
                {
                    FakeService.RespondWithAuthError(request, callback);
                }

                break;

            default:
                FakeService.RespondWithError(request, 404, callback);

                break;
            }
        }

        public void ClearCookies(Uri baseUri)
        {
            throw new NotImplementedException();
        }

        private static void RespondWithEmpty(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            var response = new MockHttpResponse { Url = request.Url, Code = 204 };
            callback?.Invoke(response, null);
        }

        private static void RespondWithDefault(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            var response = new MockHttpResponse { Url = request.Url, Code = 200, BodyBytes = new Data().ToUtf8Json() };
            callback?.Invoke(response, null);
        }

        private static void RespondWithEcho(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            var response = new MockHttpResponse { Url = request.Url, Code = 200, BodyBytes = request.BodyBytes };
            callback?.Invoke(response, null);
        }

        private static void RespondWithAuthError(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            var err = new ServiceError { errorCode = AuthErrorCode, errorMessage = "fake authorization error" };
            var response = new MockHttpResponse { Url = request.Url, Code = 401, BodyBytes = err.ToUtf8Json() };
            callback?.Invoke(response, null);
        }

        private static void RespondWithError(IHttpRequest request, long code, Action<IHttpResponse, Error> callback)
        {
            var response = new MockHttpResponse { Url = request.Url, Code = code };
            callback?.Invoke(response, null);
        }
    }
}