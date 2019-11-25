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

        public IEnumerator Register(RegisterUserRequest registerUserRequest,
            ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(registerUserRequest, "Register failed. registerUserRequest is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(registerUserRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<RegisterUserResponse>();
            callback.Try(result);
        }

        public IEnumerator GetData(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v3/public/users/me")
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
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(updateUserRequest, "Update failed. updateUserRequest is null!");

            var request = HttpRequestBuilder.CreatePatch(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", this.@namespace)
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
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(username, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"password\": \"{1}\"}}", username, password))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator SendVerificationCode(VerificationContext context, string emailAddress,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(emailAddress, "Can't send verification code! Username parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/code/request")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"context\": \"{1:G}\"}}", emailAddress, context))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator Verify(string verificationCode, string contactType, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(verificationCode, "Can't post verification code! VerificationCode parameter is null!");
            Assert.IsNotNull(contactType, "Can't post verification code! ContactType parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/code/verify")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(
                    string.Format(
                        "{{" + "\"code\": \"{0}\", " + "\"contactType\": \"{1}\"" + "}}",
                        verificationCode,
                        contactType))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SendPasswordResetCode(string emailAddress, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(emailAddress, "Can't request reset password code! emailAddress parameter is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/forgot")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\"}}", emailAddress))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ResetPassword(string resetCode, string emailAddress, string newPassword,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            string jsonResetRequest = string.Format(
                "{{" + "\"code\": \"{0}\"," + "\"emailAddress\": \"{1}\"," + "\"newPassword\": \"{2}\"" + "}}",
                resetCode,
                emailAddress,
                newPassword);

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/reset")
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
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(platformId, "Can't link platform account! Email parameter is null!");
            Assert.IsNotNull(ticket, "Can't link platform account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}")
                .WithPathParam("namespace", this.@namespace)
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
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(platformId, "Can't unlink platfrom account! Email parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("platformId", platformId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetPlatformLinks(ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
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

            var result = response.TryParseJson<PagedPlatformLinks>();
            callback.Try(result);
        }

        public IEnumerator GetUserByEmailAddress(string emailAdress, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(emailAdress, "Can't get user data! loginId parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", this.@namespace)
                .WithQueryParam("query", emailAdress)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<PagedPublicUsersInfo> result = response.TryParseJson<PagedPublicUsersInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserByUserId(string userId, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(userId, "Can't get user data! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v3/public/namespaces/{namespace}/users/{userId}")
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