// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using Utf8Json;

namespace Tests.UnitTests
{
    internal class Child
    {
        public string id => "a23";
        public string name => "somename";
    }

    internal class Parent
    {
        public string id => "a23";
        public int start => 5;
        public int num => 7;
        public Child child => new Child();
    }

    internal class PublicFields
    {
        public string id = "a23";
        public int start = 5;
        public int num = 7;
        public DateTime date = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime();
    }

    internal class UnnamedProperties
    {
        public string id => "a23";
        public int start => 5;
        public int num => 7;
        public DateTime date { get; } = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime();
    }

    internal class SomeNamedProperties
    {
        [DataMember(Name = "id")] public string Id => "a23";
        [DataMember(Name = "start")] public int Start => 5;
        [DataMember] public int num => 7;

        [DataMember(Name = "date")]
        public DateTime Date { get; } = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime();
    }

    internal class SomeIgnoredProperties
    {
        [DataMember(Name = "id")] public string Id => "a23";
        [DataMember(Name = "start")] public int Start => 5;
        public int num => 7;

        [IgnoreDataMember] public DateTime Date { get; } = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime();
    }

    internal class SomeNullableProperties
    {
        [DataMember(Name = "id")] public string Id => "a23";
        [DataMember(Name = "name")] public string Name => null;
        [DataMember(Name = "start")] public int? Start => 5;
        [DataMember(Name = "num")] public int? Num => null;
        public DateTime? date { get; } = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime();
    }

    internal enum GrantType
    {
        client_credentials,
        password,
        authorization_code,
        [DataMember(Name = "refresh_token")] RefreshToken,
    }
        
    [TestFixture]
    public class HttpRequestBuilderTest
    {
        [Test]
        public void CreateGet_MethodIsGET()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "GET";

            var request = HttpRequestBuilder.CreateGet(url).GetResult();

            Assert.AreEqual(expectedMethod, request.Method);
        }

        [Test]
        public void CreatePost_MethodIsPOST()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "POST";

            var request = HttpRequestBuilder.CreatePost(url).GetResult();

            Assert.AreEqual(expectedMethod, request.Method);
        }

        [Test]
        public void CreatePut_MethodIsPUT()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "PUT";

            var request = HttpRequestBuilder.CreatePut(url).GetResult();

            Assert.AreEqual(expectedMethod, request.Method);
        }

        [Test]
        public void CreateDelete_MethodIsDELETE()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "DELETE";

            var request = HttpRequestBuilder.CreateDelete(url).GetResult();

            Assert.AreEqual(expectedMethod, request.Method);
        }

        [Test]
        public void WithPathParam_PathParametersSubstitutedInUrl()
        {
            const string url = "https://example.com/{param1}/{param2}/{param3}/";
            string[] pathParameters = { "path1", "path2", "path3" };
            const string expectedAddress = "https://example.com/path1/path2/path3/";

            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("param1", pathParameters[0])
                .WithPathParam("param2", pathParameters[1])
                .WithPathParam("param3", pathParameters[2])
                .GetResult();

            Assert.AreEqual(expectedAddress, request.Url);
        }

        [Test]
        public void WithPathParam_SpecialCharactersIsEscaped()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com/{firstName}/{lastName}")
                .WithContentType(MediaType.ApplicationForm)
                .WithPathParam("firstName", "w/r0")
                .WithPathParam("lastName", "$@&|eng")
                .GetResult();

            Assert.That(request.Url, Is.EqualTo("https://example.com/w%2Fr0/%24%40%26%7Ceng"));
        }

        [Test]
        public void WithQueryParam_QueryParamsAppendedInUrl()
        {
            const string url = "https://example.com/";
            string[] queryParams = { "query1", "query2", "query3" };
            const string expectedAddress = "https://example.com/?param1=query1&param2=query2&param3=query3";

            var request = HttpRequestBuilder.CreateGet(url)
                .WithQueryParam("param1", queryParams[0])
                .WithQueryParam("param2", queryParams[1])
                .WithQueryParam("param3", queryParams[2])
                .GetResult();

            Assert.AreEqual(expectedAddress, request.Url);
        }

        [Test]
        public void WithQueryParam_SpecialCharactersEscapedInUrl()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com")
                .WithContentType(MediaType.ApplicationForm)
                .WithQueryParam("firstName", "w/r0")
                .WithQueryParam("lastName", "$@&|eng")
                .GetResult();

            Assert.That(request.Url, Is.EqualTo("https://example.com?firstName=w%2Fr0&lastName=%24%40%26%7Ceng"));
        }

        [Test]
        public void WithQueryParam_MultipleSameKeys_Repeated()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com")
                .WithContentType(MediaType.ApplicationForm)
                .WithQueryParam("ids", "abc")
                .WithQueryParam("ids", "def")
                .GetResult();

            Assert.That(request.Url, Is.EqualTo("https://example.com?ids=abc&ids=def"));
        }


        [Test]
        public void WithQueryParam_CollectionValues_Repeated()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com")
                .WithContentType(MediaType.ApplicationForm)
                .WithQueryParam("ids", new[] { "abc", "def" })
                .GetResult();

            Assert.That(request.Url, Is.EqualTo("https://example.com?ids=abc&ids=def"));
        }

        [Test]
        public void WithQueries_DictionaryTranslatedIntoMultipleQueries()
        {
            const string url = "https://example.com/";
            Dictionary<string, string> queries = new Dictionary<string, string>
            {
                { "param1", "query1" }, { "param2", "query2" }, { "param3", "query3" }
            };
            const string expectedAddress = "https://example.com/?param1=query1&param2=query2&param3=query3";

            var httpRequest = HttpRequestBuilder.CreateGet(url).WithQueries(queries).GetResult();

            Assert.AreEqual(expectedAddress, httpRequest.Url);
        }

        [Test]
        public void WithQueries_FlatAnonymousType_AppendedToUrl()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com/q")
                .WithQueries(
                    new
                    {
                        id = "a23",
                        start = 5,
                        num = 7,
                        date = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime()
                    })
                .GetResult();

            Assert.AreEqual(
                "https://example.com/q?id=a23&start=5&num=7&date=1985-04-12T23%3A20%3A50.5200000Z",
                request.Url);
        }

        [Test]
        public void WithQueries_PropertiesWithNullableTypes_NullValuesNotApplied()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com/q")
                .WithQueries(new SomeNullableProperties())
                .GetResult();

            Assert.AreEqual("https://example.com/q?id=a23&start=5&date=1985-04-12T23%3A20%3A50.5200000Z", request.Url);
        }

        [Test]
        public void NoAuth_AuthTypeIsNone()
        {
            const string url = "https://example.com/";

            var request = HttpRequestBuilder.CreateGet(url).GetResult();

            Assert.AreEqual(HttpAuth.None, request.AuthType);
        }

        [Test]
        public void WithBasicAuth_CredentialsAreEmptyOrNull_ThrowsException()
        {
            const string url = "https://example.com/";

            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth(null, "notNullOrEmpty").GetResult());
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth("", "notNullOrEmpty").GetResult());
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth("notNullOrEmpty", null).GetResult());
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth("notNullOrEmpty", "").GetResult());
            Assert.Throws<ArgumentException>(() => HttpRequestBuilder.CreateGet(url).WithBasicAuth("", "").GetResult());
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth("", null).GetResult());
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth(null, "").GetResult());
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreateGet(url).WithBasicAuth(null, null).GetResult());
        }

        [Test]
        public void WithBasicAuth_IdAndSecretEncodedAsBase64AndPrependedWithBasic()
        {
            const string url = "https://example.com/";
            const string userId = "johndoe";
            const string userSecret = "secretOfJohnDoe";
            const string expectedBasicAuthorization = "Basic am9obmRvZTpzZWNyZXRPZkpvaG5Eb2U=";

            var request = HttpRequestBuilder.CreateGet(url).WithBasicAuth(userId, userSecret).GetResult();

            Assert.AreEqual(request.Headers["Authorization"], expectedBasicAuthorization);
        }

        [Test]
        public void WithBasicAuth_ExplicitCredentials_AuthTypeIsBasic()
        {
            const string url = "https://example.com/";
            const string userId = "johndoe";
            const string userSecret = "secretOfJohnDoe";

            var request = HttpRequestBuilder.CreateGet(url).WithBasicAuth(userId, userSecret).GetResult();

            Assert.AreEqual(HttpAuth.Basic, request.AuthType);
        }

        [Test]
        public void WithBasicAuth_ImplicitCredentials_AuthTypeIsBasicAndAuthorizationHeaderIsEmpty()
        {
            const string url = "https://example.com/";

            var request = HttpRequestBuilder.CreateGet(url).WithBasicAuth().GetResult();

            Assert.AreEqual(HttpAuth.Basic, request.AuthType);
            Assert.False(request.Headers.ContainsKey("Authorization"));
        }

        [Test]
        public void WithBearerAuth_TokenIsNullOrEmpty_ThrowsException()
        {
            const string url = "https://example.com/";

            Assert.Throws<ArgumentException>(() => HttpRequestBuilder.CreateGet(url).WithBearerAuth(null).GetResult());
            Assert.Throws<ArgumentException>(() => HttpRequestBuilder.CreateGet(url).WithBearerAuth("").GetResult());
        }

        [Test]
        public void WithBearerAuth_TokenIsNotNullOrEmpty_TokenIsPrependedWithBearer()
        {
            const string url = "https://example.com/";
            const string token = "exampleOfBearerToken";
            const string expectedBearerAuthorization = "Bearer " + token;

            var request = HttpRequestBuilder.CreateGet(url).WithBearerAuth(token).GetResult();

            Assert.AreEqual(request.Headers["Authorization"], expectedBearerAuthorization);
        }

        [Test]
        public void WithBearerAuth_ExplicitCredentials_AuthTypeIsBearer()
        {
            const string url = "https://example.com/";
            const string token = "exampleOfBearerToken";

            var request = HttpRequestBuilder.CreateGet(url).WithBearerAuth(token).GetResult();

            Assert.AreEqual(HttpAuth.Bearer, request.AuthType);
        }

        [Test]
        public void WithBearerAuth_ImplicitCredentials_AuthTypeIsBearerAndAuthorizationHeaderIsEmpty()
        {
            const string url = "https://example.com/";

            var request = HttpRequestBuilder.CreateGet(url).WithBearerAuth().GetResult();

            Assert.AreEqual(HttpAuth.Bearer, request.AuthType);
            Assert.False(request.Headers.ContainsKey("Authorization"));
        }

        [Test]
        public void WithContentType_ApplicationForm_Success()
        {
            const string url = "https://example.com/";
            var actualContentType = MediaType.ApplicationForm;
            const string expectedContentType = "application/x-www-form-urlencoded";

            var request = HttpRequestBuilder.CreatePost(url).WithContentType(actualContentType).GetResult();

            Assert.IsTrue(request.Headers["Content-Type"].Contains(expectedContentType));
        }

        [Test]
        public void WithContentType_ApplicationJson_Success()
        {
            const string url = "https://example.com/";
            var actualContentType = MediaType.ApplicationJson;
            const string expectedContentType = "application/json";

            var request = HttpRequestBuilder.CreatePost(url).WithContentType(actualContentType).GetResult();

            Assert.IsTrue(request.Headers["Content-Type"].Contains(expectedContentType));
        }

        [Test]
        public void WithContentType_TextPlain_Success()
        {
            const string url = "https://example.com/";
            var actualContentType = MediaType.TextPlain;
            const string expectedContentType = "text/plain";

            var request = HttpRequestBuilder.CreatePost(url).WithContentType(actualContentType).GetResult();

            Assert.IsTrue(request.Headers["Content-Type"].Contains(expectedContentType));
        }

        [Test]
        public void Accepts_ApplicationJson_Success()
        {
            const string url = "https://example.com/";
            var actualAcceptType = MediaType.ApplicationJson;
            const string expectedAcceptType = "application/json";

            var request = HttpRequestBuilder.CreateGet(url).Accepts(actualAcceptType).GetResult();

            Assert.IsTrue(request.Headers["Accept"].Contains(expectedAcceptType));
        }

        [Test]
        public void Accepts_ApplicationForm_Success()
        {
            const string url = "https://example.com/";
            var actualAcceptType = MediaType.ApplicationForm;
            const string expectedAcceptType = "application/x-www-form-urlencoded";

            var request = HttpRequestBuilder.CreateGet(url).Accepts(actualAcceptType).GetResult();

            Assert.IsTrue(request.Headers["Accept"].Contains(expectedAcceptType));
        }

        [Test]
        public void Accepts_TextPlain_Success()
        {
            const string url = "https://example.com/";
            var actualAcceptType = MediaType.TextPlain;
            const string expectedAcceptType = "text/plain";

            var request = HttpRequestBuilder.CreateGet(url).Accepts(actualAcceptType).GetResult();

            Assert.IsTrue(request.Headers["Accept"].Contains(expectedAcceptType));
        }

        [Test]
        public void WithFormParam_FormParamsIsSetAsBody()
        {
            const string url = "https://example.com/";
            string[,] formParameters = { { "key1", "value1" }, { "key2", "value2" }, { "key3", "value3" } };
            const string formBody = "key1=value1&key2=value2&key3=value3";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithBasicAuth("username", "password")
                .WithContentType(MediaType.ApplicationForm)
                .WithFormParam(formParameters[0, 0], formParameters[0, 1])
                .WithFormParam(formParameters[1, 0], formParameters[1, 1])
                .WithFormParam(formParameters[2, 0], formParameters[2, 1])
                .GetResult();

            Assert.That(formBody, Is.EqualTo(Encoding.UTF8.GetString(request.BodyBytes)));
        }

        [Test]
        public void WithFormParam_ContentTypeIsApplicationForm()
        {
            const string url = "https://example.com/";
            string[,] formParameters = { { "key1", "value1" }, { "key2", "value2" }, { "key3", "value3" } };
            const string formBody = "key1=value1&key2=value2&key3=value3";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithBasicAuth("username", "password")
                .WithFormParam(formParameters[0, 0], formParameters[0, 1])
                .WithFormParam(formParameters[1, 0], formParameters[1, 1])
                .WithFormParam(formParameters[2, 0], formParameters[2, 1])
                .GetResult();

            Assert.AreEqual(MediaType.ApplicationForm.ToString(), request.Headers["Content-Type"]);
            Assert.AreEqual(formBody, Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormParam_WithSpecialCharacters_Escaped()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com/")
                .WithContentType(MediaType.ApplicationForm)
                .WithFormParam("firstName", "w/r0")
                .WithFormParam("lastName", "$@&|eng")
                .GetResult();

            Assert.That(
                request.BodyBytes,
                Is.EqualTo(Encoding.UTF8.GetBytes("firstName=w%2Fr0&lastName=%24%40%26%7Ceng")));
        }


        [Test]
        public void WithFormParam_MultipleSameKeys_Repeated()
        {
            var request = HttpRequestBuilder.CreateGet("https://example.com")
                .WithContentType(MediaType.ApplicationForm)
                .WithFormParam("ids", "abc")
                .WithFormParam("ids", "def")
                .GetResult();

            Assert.That(request.BodyBytes, Is.EqualTo(Encoding.UTF8.GetBytes("ids=abc&ids=def")));
        }

        [Test]
        public void WithBody_FormDataContentSetBeforeContentType_ExplicitContentTypeTakesPrecedence()
        {
            const string url = "https://example.com/";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithContentType(MediaType.TextPlain)
                .WithBody(new FormDataContent())
                .GetResult();

            Assert.True(request.Headers.TryGetValue("Content-Type", out string contentType));
            Assert.AreEqual(MediaType.TextPlain.ToString(), contentType);
        }

        [Test]
        public void WithBody_FormDataContentSetAfterContentType_ExplicitContentTypeTakesPrecedence()
        {
            const string url = "https://example.com/";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithBody(new FormDataContent())
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            Assert.True(request.Headers.TryGetValue("Content-Type", out string contentType));
            Assert.AreEqual(MediaType.TextPlain.ToString(), contentType);
        }

        [Test]
        public void WithBody_StringBody_ContentTypeIsPlainText()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithBody("some plaintext")
                .GetResult();

            Assert.That(request.Headers.ContainsKey("Content-Type"));
            Assert.That(request.Headers["Content-Type"], Is.EqualTo(MediaType.TextPlain.ToString()));
        }

        public class BodyModel
        {
            public int A;
            public string B;
            public DateTime C;
        }

        [Test]
        public void WithJsonBody_ContentTypeNotSet_ContentTypeIsJson()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithJsonBody(new BodyModel { A = 23, B = "string", C = DateTime.Now })
                .GetResult();

            var body = JsonSerializer.Deserialize<BodyModel>(request.BodyBytes);

            Assert.NotNull(body);
            Assert.True(request.Headers.ContainsKey("Content-Type"));
            Assert.AreEqual(MediaType.ApplicationJson.ToString(), request.Headers["Content-Type"]);
        }

        [Test]
        public void WithJsonBody_GenericBody_BodyIsJson()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithJsonBody(new BodyModel { A = 23, B = "string", C = DateTime.Now })
                .GetResult();

            var body = JsonSerializer.Deserialize<BodyModel>(request.BodyBytes);

            Assert.NotNull(body);
        }

        [Test]
        public void WithJsonBody_ContentTypeSetBefore_ContentTypeTakesPrecedence()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithContentType(MediaType.TextPlain)
                .WithJsonBody(new BodyModel { A = 23, B = "string", C = DateTime.Now })
                .GetResult();

            Assert.AreEqual(MediaType.TextPlain.ToString(), request.Headers["Content-Type"]);
        }

        [Test]
        public void WithJsonBody_ContentTypeSetAfter_ContentTypeTakesPrecedence()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithJsonBody(new BodyModel { A = 23, B = "string", C = DateTime.Now })
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            Assert.AreEqual(MediaType.TextPlain.ToString(), request.Headers["Content-Type"]);
        }

        [Test]
        public void WithBody_Success()
        {
            const string url = "https://example.com/";
            const string bodyContent = "example";

            var request = HttpRequestBuilder.CreateGet(url).WithBody(bodyContent).GetResult();

            Assert.IsTrue(Encoding.UTF8.GetString(request.BodyBytes).Contains(bodyContent));
        }

        [Test]
        public void WithFormBody_FlatAnonymousType_Applied()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(
                    new
                    {
                        id = "a23",
                        start = 5,
                        num = 7,
                        date = DateTime.Parse("1985-04-12T23:20:50.52Z").ToUniversalTime()
                    })
                .GetResult();

            Assert.AreEqual(
                "id=a23&start=5&num=7&date=1985-04-12T23%3A20%3A50.5200000Z",
                Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_NoProperties_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreatePost("https://example.com/q")
                    .WithFormBody(new PublicFields())
                    .GetResult());
        }

        [Test]
        public void WithFormBody_NullProperties_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreatePost("https://example.com/q")
                    .WithFormBody<UnnamedProperties>(null)
                    .GetResult());
        }

        [Test]
        public void WithFormBody_NonNullUnnamedProperties_Applied()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(new UnnamedProperties())
                .GetResult();

            Assert.AreEqual(
                "id=a23&start=5&num=7&date=1985-04-12T23%3A20%3A50.5200000Z",
                Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_NestedProperties_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => HttpRequestBuilder.CreatePost("https://example.com/q").WithFormBody(new Parent()).GetResult());
        }

        [Test]
        public void WithFormBody_NamedProperties_NamesApplied()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(new SomeNamedProperties())
                .GetResult();

            Assert.AreEqual(
                "id=a23&start=5&num=7&date=1985-04-12T23%3A20%3A50.5200000Z",
                Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_NamedPropertiesWithIgnore_IgnoredPropertyNotApplied()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(new SomeIgnoredProperties())
                .GetResult();

            Assert.AreEqual("id=a23&start=5&num=7", Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_PropertiesWithNullableTypes_NullValuesNotApplied()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(new SomeNullableProperties())
                .GetResult();

            Assert.AreEqual(
                "id=a23&start=5&date=1985-04-12T23%3A20%3A50.5200000Z",
                Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_AnonymousTypeWithEnumValue_EnumValuesEncoded()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(new { grant_type = GrantType.authorization_code })
                .GetResult();

            Assert.AreEqual(
                "grant_type=authorization_code",
                Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_AnonymousTypeWithEnumValueAndDataMemberAttribute_EnumValuesEncoded()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/q")
                .WithFormBody(new { grant_type = GrantType.RefreshToken })
                .GetResult();

            Assert.AreEqual(
                "grant_type=refresh_token",
                Encoding.UTF8.GetString(request.BodyBytes));
        }

        [Test]
        public void WithFormBody_ContentTypeSetBefore_ContentTypeTakesPrecedence()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithContentType(MediaType.TextPlain)
                .WithFormBody(new { A = 23, B = "string", C = DateTime.Now })
                .GetResult();

            Assert.AreEqual(MediaType.TextPlain.ToString(), request.Headers["Content-Type"]);
        }

        [Test]
        public void WithFormBody_ContentTypeSetAfter_ContentTypeTakesPrecedence()
        {
            var request = HttpRequestBuilder.CreatePost("https://example.com/")
                .WithFormBody(new { A = 23, B = "string", C = DateTime.Now })
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            Assert.AreEqual(MediaType.TextPlain.ToString(), request.Headers["Content-Type"]);
        }


        [Test]
        public void CheckSum_MD5_test()
        {
            string checkSum;
            byte[] data = Encoding.UTF8.GetBytes("example");

            using (MD5 md5 = MD5.Create())
            {
                byte[] computeHash = md5.ComputeHash(data);
                checkSum = BitConverter.ToString(computeHash).Replace("-", "");
            }

            Assert.IsTrue(checkSum == "1A79A4D60DE6718E8E5B326E338AE533");
        }
    }
}