// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class UserAccount
    {
        #region Fields 

        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly ISession session;
        private readonly IHttpClient httpClient;

        #endregion

        #region Constructor

        internal UserAccount(string baseUrl, string @namespace, ISession session, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            Assert.IsFalse(
                string.IsNullOrEmpty(@namespace),
                "Creating " + GetType().Name + " failed. Parameter ns is null.");

            this.@namespace = @namespace;
            this.session = session;
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        #endregion

        #region Public Methods


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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator Update(UpdateUserRequest updateUserRequest, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(updateUserRequest, "Update failed. updateUserRequest is null!");
            if (!string.IsNullOrEmpty(updateUserRequest.emailAddress))
            {
                Error error = new Error(ErrorCode.BadRequest, "Cannot update user email using this function. Use UpdateEmail instead.");
                callback.TryError(error);
            }

            var request = HttpRequestBuilder.CreatePatch(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator UpdateEmail(UpdateEmailRequest updateEmailRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(updateEmailRequest, "Update failed. updateEmailRequest is null!");

            var request = HttpRequestBuilder.CreatePut(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/email")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateEmailRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator UpgradeAndVerifyHeadlessAccount(UpgradeAndVerifyHeadlessRequest upgradeAndVerifyHeadlessRequest, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.code, "Can't upgrade the user! code parameter is null!");
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.emailAddress, "Can't upgrade the user! emailAddress parameter is null!");
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.password, "Can't upgrade the user! password parameter is null!");
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.username, "Can't upgrade the user! username parameter is null!");
            
            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/code/verify")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(upgradeAndVerifyHeadlessRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"context\": \"{1:G}\"}}", emailAddress, context))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator LinkOtherPlatform(PlatformType platformType, string ticket, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(ticket, "Can't link platform account! Password parameter is null!");

            if (platformType == PlatformType.Stadia)
            {
                ticket = ticket.TrimEnd('=');
            }

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator UnlinkOtherPlatform(PlatformType platformType, ResultCallback callback, string namespace_ = "")
        {
            Report.GetFunctionLog(this.GetType().Name);

            var builder = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}")
                .WithPathParam("namespace", this.@namespace)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson);

            if(!string.IsNullOrEmpty(namespace_))
            {
                UnlinkPlatformAccountRequest unlinkPlatformAccountRequest = new UnlinkPlatformAccountRequest
                {
                    platformNamespace = namespace_
                };
                builder.WithBody(unlinkPlatformAccountRequest.ToUtf8Json());
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedPlatformLinks>();
            callback.Try(result);
        }

        public IEnumerator SearchUsers(string query, SearchType by, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(query, nameof(query) + " cannot be null.");

            string[] filter = { "", "displayName", "username" };

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", this.@namespace)
                .WithQueryParam("query", query)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (by != SearchType.ALL) builder.WithQueryParam("by", filter[(int)by]);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedPublicUsersInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserByUserId(string userId, ResultCallback<PublicUserData> callback)
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<PublicUserData> result = response.TryParseJson<PublicUserData>();
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<BulkPlatformUserIdResponse> result = response.TryParseJson<BulkPlatformUserIdResponse>();
            callback.Try(result);
        }

        public IEnumerator GetCountryFromIP(ResultCallback<CountryInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v3/location/country")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CountryInfo>();
            callback.Try(result);
        }

        public IEnumerator BanUser(string @namespace, string accessToken, string userId, BanCreateRequest banRequest, ResultCallback<UserBanResponseV3> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var request = HttpRequestBuilder
               .CreatePost(this.baseUrl + "/v3/admin/namespaces/{namespace}/users/{userId}/bans")
               .WithPathParam("namespace", @namespace)
               .WithPathParam("userId", userId)
               .WithBearerAuth(accessToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(banRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserBanResponseV3>();

            callback.Try(result);
        }

        public IEnumerator ChangeUserBanStatus(string @namespace, string accessToken, string userId, string banId, bool enabled, ResultCallback<UserBanResponseV3> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            UserEnableBan changeRequest = new UserEnableBan { enabled = enabled };

            var request = HttpRequestBuilder
               .CreatePatch(this.baseUrl + "/v3/admin/namespaces/{namespace}/users/{userId}/bans/{banId}")
               .WithPathParam("namespace", @namespace)
               .WithPathParam("userId", userId)
               .WithPathParam("banId", banId)
               .WithBearerAuth(accessToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(changeRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserBanResponseV3>();

            callback.Try(result);
        }

        public IEnumerator GetUserBannedList(string @namespace, string accessToken, bool activeOnly, BanType banType, int offset, int limit, ResultCallback<UserBanPagedList> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var request = HttpRequestBuilder
               .CreateGet(this.baseUrl + "/v3/admin/namespaces/{namespace}/bans/users")
               .WithPathParam("namespace", @namespace)
               .WithQueryParam("activeOnly", activeOnly ? "true" : "false")
               .WithQueryParam("banType", banType.ToString())
               .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
               .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
               .WithBearerAuth(accessToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserBanPagedList>();

            callback.Try(result);
        }

        public IEnumerator BulkGetUserInfo(string[] userIds, ResultCallback<ListBulkUserInfoResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(userIds, "userIds cannot be null.");

            ListBulkUserInfoRequest bulkUserInfoRequest = new ListBulkUserInfoRequest
            {
                userIds = userIds
            };

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v3/public/namespaces/{namespace}/users/bulk/basic")
                .WithPathParam("namespace", this.@namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(bulkUserInfoRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<ListBulkUserInfoResponse> result = response.TryParseJson<ListBulkUserInfoResponse>();
            callback.Try(result);
        }

        public IEnumerator Change2FAFactor(string mfaToken, TwoFAFactorType factor, ResultCallback<TokenData> callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/oauth/mfa/factor/change")
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithFormParam("mfaToken", mfaToken)
                .WithFormParam("factor", factor.GetString())
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        public IEnumerator Disable2FAAuthenticator(ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreateDelete(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/disable")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(this.session.AuthorizationToken) 
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator Enable2FAAuthenticator(string code, ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/enable")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithFormParam("code", code)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GenerateSecretKeyFor3rdPartyAuthenticateApp(ResultCallback<SecretKey3rdPartyApp> callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/key")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(this.session.AuthorizationToken) 
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result< SecretKey3rdPartyApp> result = response.TryParseJson<SecretKey3rdPartyApp>();
            callback.Try(result);
        }
        
        public IEnumerator GenerateBackUpCode(ResultCallback<TwoFACode> callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<TwoFACode> result = response.TryParseJson<TwoFACode>();
            callback.Try(result); 
        }

        public IEnumerator Disable2FABackupCodes(ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreateDelete(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/disable")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator Enable2FABackupCodes(ResultCallback<TwoFACode> callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/enable")
                .WithPathParam("namespace", this.@namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<TwoFACode> result = response.TryParseJson<TwoFACode>();
            callback.Try(result); 
        }

        public IEnumerator GetBackUpCode(ResultCallback<TwoFACode> callback)
        {
            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(this.session.AuthorizationToken) 
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<TwoFACode> result = response.TryParseJson<TwoFACode>();
            callback.Try(result);
        }

        public IEnumerator GetUserEnabledFactors(ResultCallback<Enable2FAFactors> callback)
        {
            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<Enable2FAFactors> result = response.TryParseJson<Enable2FAFactors>();
            callback.Try(result);
        }
        
        public IEnumerator Make2FAFactorDefault(TwoFAFactorType factor, ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(this.session.AuthorizationToken)
                .WithFormParam("factor", factor.GetString())
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }      

        public IEnumerator GetInputValidations(string languageCode, ResultCallback<InputValidation> callback, bool defaultOnEmpty = true)
        {
            Assert.IsNotNull(languageCode, nameof(languageCode) + " cannot be null");

            var request = HttpRequestBuilder
               .CreateGet(this.baseUrl + "/v3/public/inputValidations")
               .WithQueryParam("languageCode", languageCode)
               .WithQueryParam("defaultOnEmpty", defaultOnEmpty ? "true" : "false")
               .WithBearerAuth(this.session.AuthorizationToken)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<InputValidation>();

            callback.Try(result);
        }
		
        #endregion
    }
}
