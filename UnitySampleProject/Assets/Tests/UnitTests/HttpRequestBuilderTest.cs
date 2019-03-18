// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AccelByte.Core;
using NUnit.Framework;

namespace Tests.AUnitTests
{
    [TestFixture]
    public class HttpRequestBuilderTest
    {
        [Test]
        public void CreateGet_Success()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "GET";
            
            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Method, expectedMethod));
        }
        
        [Test]
        public void CreatePost_Success()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "POST";
            
            var httpRequest = HttpRequestBuilder
                .CreatePost(url)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Method, expectedMethod));
        }
        
        [Test]
        public void CreatePut_Success()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "PUT";
            
            var httpRequest = HttpRequestBuilder
                .CreatePut(url)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Method, expectedMethod));
        }
        
        [Test]
        public void CreateDelete_Success()
        {
            const string url = "https://example.com/";
            const string expectedMethod = "DELETE";
            
            var httpRequest = HttpRequestBuilder
                .CreateDelete(url)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Method, expectedMethod));
        }
        
        [Test]
        public void WithPathParam_Success()
        {
            const string url = "https://example.com/{param1}/{param2}/{param3}/";
            string[] pathParameters = new string[]{"path1", "path2", "path3"};
            const string expectedAddress = "https://example.com/path1/path2/path3/";
            
            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .WithPathParam("param1", pathParameters[0])
                .WithPathParam("param2", pathParameters[1])
                .WithPathParam("param3", pathParameters[2])
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Address.ToString(), expectedAddress));
        }
        
        [Test]
        public void WithQueryParam_Success()
        {
            const string url = "https://example.com/";
            string[] queryParams = new string[]{"query1", "query2", "query3"};
            const string expectedAddress = "https://example.com/?param1=query1&param2=query2&param3=query3";
            
            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .WithQueryParam("param1", queryParams[0])
                .WithQueryParam("param2", queryParams[1])
                .WithQueryParam("param3", queryParams[2])
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Address.ToString(), expectedAddress));
        }
        
        [Test]
        public void WithQueries_Success()
        {
            const string url = "https://example.com/";
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add("param1", "query1");
            queries.Add("param2", "query2");
            queries.Add("param3", "query3");
            const string expectedAddress = "https://example.com/?param1=query1&param2=query2&param3=query3";
            
            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .WithQueries(queries)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Address.ToString(), expectedAddress));
        }
        
        [Test]
        public void WithBasicAuth_Success()
        {
            const string url = "https://example.com/"; 
            const string userId = "johndoe";
            const string userSecret = "secretOfJohnDoe";
            const string expectedBasicAuthorization = "Basic am9obmRvZTpzZWNyZXRPZkpvaG5Eb2U=";

            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .WithBasicAuth(userId, userSecret)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Headers["Authorization"], expectedBasicAuthorization));
        }
        
        [Test]
        public void WithBearerAuth_Success()
        {
            const string url = "https://example.com/";
            const string token = "exampleOfBearerToken";
            const string expectedBearerAuthorization = "Bearer " + token;

            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .WithBearerAuth(token)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.AreEqual(httpRequest.Headers["Authorization"], expectedBearerAuthorization));
        }
        
        [Test]
        public void WithContentType_ApplicationForm_Success()
        {
            const string url = "https://example.com/";
            var actualContentType = MediaType.ApplicationForm;
            const string expectedContentType = "application/x-www-form-urlencoded";
            
            var httpRequest = HttpRequestBuilder
                .CreatePost(url)
                .WithContentType(actualContentType)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.True(httpRequest.ContentType.Contains(expectedContentType)));
        }
        
        [Test]
        public void WithContentType_ApplicationJson_Success()
        {
            const string url = "https://example.com/";
            var actualContentType = MediaType.ApplicationJson;
            const string expectedContentType = "application/json";

            var httpRequest = HttpRequestBuilder
                .CreatePost(url)
                .WithContentType(actualContentType)
                .ToRequest();

            TestHelper.Assert(() =>  Assert.True(httpRequest.ContentType.Contains(expectedContentType)));
        }
        
        [Test]
        public void WithContentType_TextPlain_Success()
        {
            const string url = "https://example.com/";
            var actualContentType = MediaType.TextPlain;
            const string expectedContentType = "text/plain";
            
            var httpRequest = HttpRequestBuilder
                .CreatePost(url)
                .WithContentType(actualContentType)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.True(httpRequest.ContentType.Contains(expectedContentType)));
        }

        [Test]
        public void Accepts_ApplicationJson_Success()
        {
            const string url = "https://example.com/";
            var actualAcceptType = MediaType.ApplicationJson;
            const string expectedAcceptType = "application/json";

            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .Accepts(actualAcceptType)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.True(httpRequest.Accept.Contains(expectedAcceptType)));
        }
        
        [Test]
        public void Accepts_ApplicationForm_Success()
        {
            const string url = "https://example.com/";
            var actualAcceptType = MediaType.ApplicationForm;
            const string expectedAcceptType = "application/x-www-form-urlencoded";

            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .Accepts(actualAcceptType)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.True(httpRequest.Accept.Contains(expectedAcceptType)));
        }
        
        [Test]
        public void Accepts_TextPlain_Success()
        {
            const string url = "https://example.com/";
            var actualAcceptType = MediaType.TextPlain;
            const string expectedAcceptType = "text/plain";

            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .Accepts(actualAcceptType)
                .ToRequest();
            
            TestHelper.Assert(() =>  Assert.True(httpRequest.Accept.Contains(expectedAcceptType)));
        }

        [Test][Ignore("HTTP request's content/body is private")]
        public void WithFormParam_Success()
        {
            const string url = "https://example.com/";
            string[,] formParameters = new string[3,2]{{"key1","value1"},{"key2","value2"},{"key3","value3"}};
            const string expectedAddress = url + "?key1=value1&key2=value2&key3=value3";

            var httpRequest = HttpRequestBuilder
                .CreatePost(url)
                .WithBasicAuth("","")
                .WithContentType(MediaType.ApplicationForm)
                .WithFormParam(formParameters[0,0], formParameters[0,1])
                .WithFormParam(formParameters[1,0], formParameters[1,1])
                .WithFormParam(formParameters[2,0], formParameters[2,1])
                .ToRequest();
        }

        [Test][Ignore("HTTP request's content/body is private")]
        public void WithBody_Success()
        {
            const string url = "https://example.com/";
            const string bodyContent = "example";
            
            var httpRequest = HttpRequestBuilder
                .CreateGet(url)
                .WithBody(bodyContent)
                .ToRequest();
        }

        [Test]
        public void CheckSum_MD5_test()
        {
            string checkSum;
            byte[] data = Encoding.ASCII.GetBytes("example");
            
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeHash = md5.ComputeHash(data);
                checkSum = BitConverter.ToString(computeHash).Replace("-", "");
            }

            Assert.That(checkSum=="1A79A4D60DE6718E8E5B326E338AE533");
        }
    }
}