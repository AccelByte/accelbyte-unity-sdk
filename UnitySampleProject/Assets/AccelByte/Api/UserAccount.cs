// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class UserAccount : IUserAccount
    {
        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly ISession session;
        private readonly IHttpWorker httpWorker;

        internal UserAccount(string baseUrl, string @namespace, ISession session, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            Assert.IsFalse(
                string.IsNullOrEmpty(@namespace),
                "Creating " + GetType().Name + " failed. Parameter ns is null.");

            this.@namespace = @namespace;
            this.session = session;
            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator Register(RegisterUserRequest registerUserRequest, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(registerUserRequest, "Can't create user! Info parameter is null!");

            string jsonInfo = string.Format(
                "{{" +
                "\"AuthType\": \"{0}\"," +
                "\"DisplayName\": \"{1}\"," +
                "\"LoginId\": \"{2}\"," +
                "\"Password\": \"{3}\"," +
                "\"Country\": \"{4}\"," +
                "\"DateOfBirth\": \"{5}\"" +
                "}}",
                registerUserRequest.AuthType,
                registerUserRequest.DisplayName,
                registerUserRequest.Username,
                registerUserRequest.Password,
                registerUserRequest.Country,
                registerUserRequest.DateOfBirth);

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(jsonInfo)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator GetData(ResultCallback<UserData> callback)
        {
            string relativeUrl;

            if (this.session.UserId == "me")
            {
                relativeUrl = "/v2/public/users/{userId}";
            }
            else
            {
                relativeUrl = "/v2/public/namespaces/{namespace}/users/{userId}";
            }
        
            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + relativeUrl)
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator Update(UpdateUserRequest updateUserRequest, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(updateUserRequest, "Can't update user! Request parameter is null!");

            string relativeUrl;

            if (this.session.UserId == "me")
            {
                relativeUrl = "/v2/public/users/{userId}";
            }
            else
            {
                relativeUrl = "/v2/public/namespaces/{namespace}/users/{userId}";
            }

            var request = HttpRequestBuilder
                .CreatePatch(this.baseUrl + relativeUrl)
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator Upgrade(string username, string password, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(username, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/headless/verify")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"LoginID\": \"{0}\", \"Password\": \"{1}\"}}", username, password))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator SendVerificationCode(VerificationContext context, string username, ResultCallback callback)
        {
            Assert.IsNotNull(username, "Can't send verification code! Username parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/code/request")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"LoginID\": \"{0}\", \"Context\": \"{1:G}\"}}", username, context))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator Verify(string verificationCode, string contactType, ResultCallback callback)
        {
            Assert.IsNotNull(verificationCode, "Can't post verification code! VerificationCode parameter is null!");
            Assert.IsNotNull(contactType, "Can't post verification code! ContactType parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/code/verify")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(
                    string.Format(
                        "{{" + "\"Code\": \"{0}\", " + "\"ContactType\": \"{1}\"" + "}}",
                        verificationCode,
                        contactType))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SendPasswordResetCode(string username, ResultCallback callback)
        {
            Assert.IsNotNull(username, "Can't request reset password code! LoginId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v2/public/namespaces/{namespace}/users/forgotPassword")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"LoginID\": \"{0}\"}}", username))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ResetPassword(string resetCode, string userName, string newPassword, ResultCallback callback)
        {
            string jsonResetRequest = string.Format(
                "{{" + "\"Code\": \"{0}\"," + "\"LoginID\": \"{1}\"," + "\"NewPassword\": \"{2}\"" + "}}",
                resetCode,
                userName,
                newPassword);

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v2/public/namespaces/{namespace}/users/resetPassword")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(jsonResetRequest)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator LinkOtherPlatform(string platformId, string ticket, ResultCallback callback)
        {
            Assert.IsNotNull(platformId, "Can't link platform account! Email parameter is null!");
            Assert.IsNotNull(ticket, "Can't link platform account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(
                    this.baseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithPathParam("platformId", platformId)
                .WithFormParam("ticket", ticket)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationForm)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator UnlinkOtherPlatform(string platformId, ResultCallback callback)
        {
            Assert.IsNotNull(platformId, "Can't unlink platfrom account! Email parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(
                    this.baseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithPathParam("platformId", platformId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetPlatformLinks(ResultCallback<PlatformLink[]> callback)
        {
            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/platforms")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", this.session.UserId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PlatformLink[]>();
            callback.Try(result);
        }

        public IEnumerator GetUserByLoginId(string loginId, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(loginId, "Can't get user data! loginId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/namespaces/{namespace}/users/byLoginId")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("loginId", loginId)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator GetUserByUserId(string userId, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(userId, "Can't get user data! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJson<UserData>();
            callback.Try(result);
        }
    }
}