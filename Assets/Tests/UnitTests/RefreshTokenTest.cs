using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class RefreshTokenTest
    {
        private class MockHttpRequest : IHttpRequest
        {
            public string Method { get; set; }
            public string Url { get; set; }
            public Dictionary<string, string> Headers { get; set; }
            public byte[] BodyBytes { get; set; }
        }

        private class MockHttpResponse : IHttpResponse
        {
            public string Url { get; set; }
            public long Code { get; set; }
            public byte[] BodyBytes { get { return System.Text.Encoding.UTF8.GetBytes(this.Body); } }
            public string Body { get; set; }
        }

        private class MockHttpWorker : IHttpWorker
        {
            private MockHttpResponse response;
            public event Action<UnityWebRequest> ServerErrorOccured;
            public event Action<UnityWebRequest> NetworkErrorOccured;

            public IEnumerator SendRequest(IHttpRequest request, Action<IHttpResponse> responseCallback)
            {
                string requestBody = System.Text.Encoding.UTF8.GetString(request.BodyBytes);
                MockHttpResponse response = null;
                
                string responseTemplate = 
                    @"{{
                        ""access_token"": ""{0}"",
                        ""bans"": 
                            [
                                {{
                                    ""Ban"": """",
                                    ""EndDate"": ""2019-07-05T01:59:45.046Z""
                                }}
                            ],
                        ""display_name"": ""string"", 
                        ""expires_in"": 3,
                        ""jflgs"": 0,
                        ""namespace"": ""string"",
                        ""permissions"": 
                            [
                                {{ 
                                    ""Action"": 0,
                                    ""Resource"": ""string"",
                                    ""SchedAction"": 0,
                                    ""SchedCron"": ""string"",
                                    ""SchedRange"": [ ""string"" ]
                                }}
                            ],
                        ""platform_id"": ""string"",
                        ""platform_user_id"": ""string"",
                        ""refresh_token"": ""{1}"",
                        ""roles"": [ ""string"" ],
                        ""token_type"": ""string"",
                        ""user_id"": ""string""
                    }}";

                if (requestBody.Contains("device_id"))
                {
                    response = new MockHttpResponse
                    {
                        Url = request.Url,
                        Code = 200,
                        Body = string.Format(responseTemplate, "first_access_token", "first_refresh_token")
                    };
                }
                else if (requestBody.Contains("refresh_token"))
                {
                    response = new MockHttpResponse
                    {
                        Url = request.Url,
                        Code = 200
                    };
                    
                    switch (requestBody.Substring(requestBody.IndexOf("refresh_token=") + 14))
                    {
                    case "first_refresh_token":
                        response.Body = string.Format(
                            responseTemplate,
                            "second_access_token",
                            "second_refresh_token");

                        break;
                    case "second_refresh_token":
                        response.Body = string.Format(
                            responseTemplate,
                            "third_access_token",
                            "third_refresh_token");

                        break;
                    default:
                        response.Body = string.Format(responseTemplate, "last_access_token", "last_refresh_token");

                        break;
                    }
                }


                responseCallback(response);

                yield break;
            }
        }

        [UnityTest]
        public IEnumerator LoginWithDeviceId_OnExpiring_AccessTokenRefreshed()
        {
            var expected_first_access_token = "first_access_token";
            var expected_second_access_token = "second_access_token";
            var expected_third_access_token = "third_access_token";
            var expected_last_access_token = "last_access_token";
            
            var worker = new MockHttpWorker();
            var loginSession = new OauthLoginSession(
                "https://api-alpha.accelbyte.example/login/",
                "gameNamespace",
                "some_random_clientId",
                "some_random_clientSecret",
                "some_unimportant_redirect_uri",
                worker,
                new CoroutineRunner());

            Result loginResult = null;

            yield return loginSession.LoginWithDeviceId(r => loginResult = r);

            var actual_first_access_token = loginSession.AuthorizationToken;
            
            yield return new WaitForSeconds(3.0f);
            
            var actual_second_access_token = loginSession.AuthorizationToken;
            
            yield return new WaitForSeconds(2.4f);
            
            var actual_third_access_token = loginSession.AuthorizationToken;
            
            yield return new WaitForSeconds(1.6f);
            
            var actual_last_access_token = loginSession.AuthorizationToken;
            
            Assert.That(actual_first_access_token, Is.EqualTo(expected_first_access_token));
            Assert.That(expected_second_access_token, Is.EqualTo(actual_second_access_token));
            Assert.That(expected_third_access_token, Is.EqualTo(actual_third_access_token));
            Assert.That(expected_last_access_token, Is.EqualTo(actual_last_access_token));
        }
    }
}