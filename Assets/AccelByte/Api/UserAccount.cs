// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
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
        private readonly string apiBaseUrl;
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

            this.apiBaseUrl = "https://" + AccelBytePlugin.Config.ApiBaseUrl;
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

        public IEnumerator Registerv2(RegisterUserRequestv2 registerUserRequest,
            ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(registerUserRequest, "Register failed. registerUserRequest is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users")
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

        public IEnumerator Upgradev2(string emailAddress, string username, string password, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(emailAddress, "Can't upgrade headless account! EmailAddress parameter is null!");
            Assert.IsNotNull(username, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"password\": \"{1}\", \"username\": \"{2}\"}}", emailAddress, password, username))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator UpgradeWithPlayerPortal(string returnUrl, int ttl, ResultCallback<UpgradeUserRequest> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(returnUrl, "Upgrade failed. return url is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.apiBaseUrl + "/v1/public/temporarysessions")
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"return_url\": \"{0}\", \"ttl\": {1}}}", returnUrl, ttl))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UpgradeUserRequest>();
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
                .WithBearerAuth(this.session.AuthorizationToken)
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

        public IEnumerator LinkOtherPlatform(PlatformType platformType, string ticket, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(ticket, "Can't link platform account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("platformId", platformType.ToString().ToLower())
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

        public IEnumerator ForcedLinkOtherPlatform(PlatformType platformType, string platformUserId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(platformUserId, "Can't link platform account! platformUserId parameter is null!");

            LinkPlatformAccountRequest linkedPlatformRequest = new LinkPlatformAccountRequest
            {
                platformId = platformType.ToString().ToLower(),
                platformUserId = platformUserId
            };

            Result<UserData> userDataResult = null;
            yield return this.GetData(r =>
            {
                userDataResult = r;
            });

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/platforms/link")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("userId", userDataResult.Value.userId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(linkedPlatformRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator UnlinkOtherPlatform(PlatformType platformType, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("platformId", platformType.ToString().ToLower())
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

        public IEnumerator SearchUsers(string query, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(query, nameof(query) + " cannot be null.");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", this.@namespace)
                .WithQueryParam("query", query)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedPublicUsersInfo>();
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

        public IEnumerator GetUserByOtherPlatformUserId(PlatformType platformType, string otherPlatformUserId,
            ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(otherPlatformUserId, nameof(otherPlatformUserId) + " cannot be null.");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v3/public/namespaces/{namespace}/platforms/{platformId}/users/{platformUserId}")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithPathParam("platformUserId", otherPlatformUserId)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator BulkGetUserByOtherPlatformUserIds(PlatformType platformType, BulkPlatformUserIdRequest otherPlatformUserId,
            ResultCallback<BulkPlatformUserIdResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(otherPlatformUserId, nameof(otherPlatformUserId) + " cannot be null.");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/platforms/{platformId}/users")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(otherPlatformUserId.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<BulkPlatformUserIdResponse> result = response.TryParseJson<BulkPlatformUserIdResponse>();
            callback.Try(result);
        }

        public IEnumerator GetCountryFromIP(ResultCallback<CountryInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(this.apiBaseUrl + "/location/country")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CountryInfo>();
            callback.Try(result);
        }
    }
}
