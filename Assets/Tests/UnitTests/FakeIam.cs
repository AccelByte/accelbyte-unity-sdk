// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;

namespace Tests.UnitTests
{
    class FakeIam : IHttpRequestSender
    {
        public class MockData
        {
            public string id = "someId";
            public int count = 357;
            public double avg = 3.5;
        }

        public IEnumerator Send(IHttpRequest request, Action<IHttpResponse, Error> callback, int timeoutMs)
        {
            yield return new WaitForSeconds(0.05f);

            if (request.Url.Contains("oauth"))
            {
                FakeIam.HandleLoginRequest(request, callback);
                yield break;
            }

            switch (request.Method + " " + request.Url)
            {
            case "GET https://ab.example.io/iam/fakenamespace/res" when !FakeIam.IsAuthorized(request):
            {
                FakeIam.HandleUnauthorizedRequest(request, callback);
                yield break;
            }
            case "GET https://ab.example.io/iam/fakenamespace/res":
            {
                FakeIam.HandleAuthorizedRequest(request, callback);
                yield break;
            }
            case "GET https://ab.example.io/iam/fakenamespace/res2":
            {
                FakeIam.HandleAuthorizedRequest(request, callback);
                yield break;
            }
            case "GET https://ab.example.io/iam/fakenamespace/res/abc235":
            {
                FakeIam.HandleAuthorizedRequest(request, callback);
                yield break;
            }
            default:
                callback?.Invoke(new MockHttpResponse { Url = request.Url, Code = 404 }, null);
                yield break;
            }
        }

        public void ClearCookies(Uri baseUri)
        {
            throw new NotImplementedException();
        }

        private static void HandleAuthorizedRequest(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            callback?.Invoke(
                new MockHttpResponse { Url = request.Url, Code = 200, BodyBytes = new MockData().ToUtf8Json() },
                null);
        }

        private static bool IsAuthorized(IHttpRequest request)
        {
            return request.Headers.TryGetValue("Authorization", out string auth) && auth == "Bearer first_access_token";
        }

        private static void HandleUnauthorizedRequest(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            MockHttpResponse response;
            var err = new ServiceError { errorCode = 2357, errorMessage = "fake authorization error" };
            response = new MockHttpResponse { Url = request.Url, Code = 401, BodyBytes = err.ToUtf8Json() };

            callback?.Invoke(response, null);
        }

        private static void HandleLoginRequest(IHttpRequest request, Action<IHttpResponse, Error> callback)
        {
            string requestBody = System.Text.Encoding.UTF8.GetString(request.BodyBytes);
            MockHttpResponse response = null;

            const string responseTemplate = @"{{
                ""access_token"": ""{0}"",
                ""bans"": 
                    [
                    ],
                ""display_name"": ""string"", 
                ""expires_in"": 3,
                ""jflgs"": 0,
                ""namespace"": ""fakenamespace"",
                ""permissions"": 
                    [
                    ],
                ""platform_id"": ""justice"",
                ""platform_user_id"": ""12345678"",
                ""refresh_token"": ""{1}"",
                ""roles"": [ ""string"" ],
                ""token_type"": ""string"",
                ""user_id"": ""abc235""
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
                response = new MockHttpResponse { Url = request.Url, Code = 200 };

                switch (requestBody.Substring(requestBody.IndexOf("refresh_token=") + 14))
                {
                case "first_refresh_token":
                    response.Body = string.Format(responseTemplate, "second_access_token", "second_refresh_token");

                    break;
                case "second_refresh_token":
                    response.Body = string.Format(responseTemplate, "third_access_token", "third_refresh_token");

                    break;
                default:
                    response.Body = string.Format(responseTemplate, "last_access_token", "last_refresh_token");

                    break;
                }
            }

            callback?.Invoke(response, null);
        }
    }
}