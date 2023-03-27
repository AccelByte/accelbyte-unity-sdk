// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Formerly called UserAccount
    /// </summary>
    public class UserApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        /// <param name="session">
        /// BaseUrl==IamServerUrl
        /// (!) This will soon be replaced with ISession instead of UserSession
        /// </param>
        public UserApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.IamServerUrl, session )
        {
        }

        public IEnumerator Register(RegisterUserRequest registerUserRequest
            , ResultCallback<RegisterUserResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(registerUserRequest, "Register failed. registerUserRequest is null!");

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users";
            
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(registerUserRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<RegisterUserResponse>();
            callback.Try(result);
        }

        public IEnumerator Registerv2( RegisterUserRequestv2 registerUserRequest
            , ResultCallback<RegisterUserResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(registerUserRequest, "Register failed. registerUserRequest is null!");

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(registerUserRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<RegisterUserResponse>();
            callback.Try(result);
        }

        public IEnumerator GetData( ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me")
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator Update( UpdateUserRequest updateUserRequest
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(updateUserRequest, "Update failed. updateUserRequest is null!");
            if (!string.IsNullOrEmpty(updateUserRequest.emailAddress))
            {
                Error error = new Error(ErrorCode.BadRequest, 
                    "Cannot update user email using this function. Use UpdateEmail instead.");
                callback.TryError(error);
            }

            var request = HttpRequestBuilder.CreatePatch(BaseUrl + "/v4/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator UpdateEmail( UpdateEmailRequest updateEmailRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(updateEmailRequest, "Update failed. updateEmailRequest is null!");

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v4/public/namespaces/{namespace}/users/me/email")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateEmailRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator Upgrade( string username
            , string password
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(username, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"password\": \"{1}\"}}", 
                    username, 
                    password))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator Upgradev2( string emailAddress
            , string username
            , string password
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(emailAddress, "Can't upgrade headless account! EmailAddress parameter is null!");
            Assert.IsNotNull(username, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"password\": \"{1}\", \"username\": \"{2}\"}}", 
                    emailAddress, 
                    password, 
                    username))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator UpgradeAndVerifyHeadlessAccount( UpgradeAndVerifyHeadlessRequest upgradeAndVerifyHeadlessRequest
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.code, 
                "Can't upgrade the user! code parameter is null!");
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.emailAddress, 
                "Can't upgrade the user! emailAddress parameter is null!");
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.password, 
                "Can't upgrade the user! password parameter is null!");
            Assert.IsNotNull(upgradeAndVerifyHeadlessRequest.username, 
                "Can't upgrade the user! username parameter is null!");

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/code/verify";
            
            var request = HttpRequestBuilder
                .CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(upgradeAndVerifyHeadlessRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator SendVerificationCode( VerificationContext context
            , string emailAddress
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(emailAddress, 
                "Can't send verification code! Username parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/code/request")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\", \"context\": \"{1:G}\"}}", 
                    emailAddress, 
                    context))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator Verify( string verificationCode
            , string contactType
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(verificationCode, 
                "Can't post verification code! VerificationCode parameter is null!");
            Assert.IsNotNull(contactType, 
                "Can't post verification code! ContactType parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/code/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(
                    string.Format(
                        "{{" + "\"code\": \"{0}\", " + "\"contactType\": \"{1}\"" + "}}",
                        verificationCode,
                        contactType))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SendPasswordResetCode( string emailAddress
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(emailAddress, 
                "Can't request reset password code! emailAddress parameter is null!");

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/forgot";
            
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"emailAddress\": \"{0}\"}}", emailAddress))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ResetPassword( string resetCode
            , string emailAddress
            , string newPassword
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            string jsonResetRequest = string.Format(
                "{{" + "\"code\": \"{0}\"," + "\"emailAddress\": \"{1}\"," + "\"newPassword\": \"{2}\"" + "}}",
                resetCode,
                emailAddress,
                newPassword);

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/reset";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(jsonResetRequest)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator LinkOtherPlatform( PlatformType platformType
            , string ticket
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(ticket, 
                "Can't link platform account! Password parameter is null!"); 

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}";
            
            var request = HttpRequestBuilder
                .CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithFormParam("ticket", ticket)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationForm)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ForcedLinkOtherPlatform( PlatformType platformType
            , string platformUserId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(platformUserId, 
                "Can't link platform account! platformUserId parameter is null!");

            var linkedPlatformRequest = new LinkPlatformAccountRequest
            {
                platformId = platformType.ToString().ToLower(),
                platformUserId = platformUserId,
            };

            Result<UserData> userDataResult = null;
            yield return GetData(r =>
            {
                userDataResult = r;
            });

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/platforms/link")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userDataResult.Value.userId)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(linkedPlatformRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="callback"></param>
        /// <param name="inNamespace">Potentially different than base.Namespace_</param>
        /// <returns></returns>
        public IEnumerator UnlinkOtherPlatform( PlatformType platformType
            , ResultCallback callback
            , string inNamespace = "" )
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}";

            inNamespace = string.IsNullOrEmpty( inNamespace ) ? Namespace_ : inNamespace;

            var builder = HttpRequestBuilder
                .CreateDelete(url)
                .WithPathParam("namespace", inNamespace)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson);

            if(!string.IsNullOrEmpty(inNamespace))
            {
                UnlinkPlatformAccountRequest unlinkPlatformAccountRequest = new UnlinkPlatformAccountRequest
                {
                    platformNamespace = inNamespace,
                };
                builder.WithBody(unlinkPlatformAccountRequest.ToUtf8Json());
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetPlatformLinks( string userId
            , ResultCallback<PagedPlatformLinks> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/platforms")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PagedPlatformLinks>();
            callback.Try(result);
        }

        public IEnumerator SearchUsers( string query
            , SearchType by
            , ResultCallback<PagedPublicUsersInfo> callback
            , int offset
            , int limit)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(query, nameof(query) + " cannot be null.");

            string[] filter = { "", "displayName", "username" };

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("query", query)
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit >= 0 ) ? limit.ToString() : "" )
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (by != SearchType.ALL) builder.WithQueryParam("by", filter[(int)by]);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PagedPublicUsersInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserByUserId( string userId
            , ResultCallback<PublicUserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userId, "Can't get user data! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<PublicUserData> result = response.TryParseJson<PublicUserData>();
            callback.Try(result);
        }

        public IEnumerator GetUserByOtherPlatformUserId( PlatformType platformType
            , string otherPlatformUserId
            , ResultCallback<UserData> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(otherPlatformUserId, nameof(otherPlatformUserId) + " cannot be null.");

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/platforms/{platformId}/users/{platformUserId}";
                
            var request = HttpRequestBuilder
                .CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithPathParam("platformUserId", otherPlatformUserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<UserData> result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator BulkGetUserByOtherPlatformUserIds( PlatformType platformType
            , BulkPlatformUserIdRequest otherPlatformUserId
            , ResultCallback<BulkPlatformUserIdResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(otherPlatformUserId, nameof(otherPlatformUserId) + " cannot be null.");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/platforms/{platformId}/users")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(otherPlatformUserId.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<BulkPlatformUserIdResponse>();
            callback.Try(result);
        }

        public IEnumerator GetCountryFromIP( ResultCallback<CountryInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/location/country")
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CountryInfo>();
            callback.Try(result);
        }

        public IEnumerator BulkGetUserInfo( string[] userIds
            , ResultCallback<ListBulkUserInfoResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userIds, "userIds cannot be null.");

            ListBulkUserInfoRequest bulkUserInfoRequest = new ListBulkUserInfoRequest
            {
                userIds = userIds,
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/bulk/basic")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(bulkUserInfoRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<ListBulkUserInfoResponse> result = response.TryParseJson<ListBulkUserInfoResponse>();
            callback.Try(result);
        }

        public IEnumerator Change2FAFactor( string mfaToken
            , TwoFAFactorType factor
            , ResultCallback<TokenData> callback )
        {
            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/mfa/factor/change")
                .WithBearerAuth(AuthToken)
                .WithFormParam("mfaToken", mfaToken)
                .WithFormParam("factor", factor.GetString())
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        public IEnumerator Disable2FAAuthenticator( ResultCallback callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/disable";
            var request = HttpRequestBuilder.CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken) 
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator Enable2FAAuthenticator( string code
            , ResultCallback callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithFormParam("code", code)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GenerateSecretKeyFor3rdPartyAuthenticateApp( ResultCallback<SecretKey3rdPartyApp> callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/key";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken) 
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result< SecretKey3rdPartyApp> result = response.TryParseJson<SecretKey3rdPartyApp>();
            callback.Try(result);
        }
        
        public IEnumerator GenerateBackUpCode( ResultCallback<TwoFACode> callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<TwoFACode> result = response.TryParseJson<TwoFACode>();
            callback.Try(result); 
        }

        public IEnumerator Disable2FABackupCodes( ResultCallback callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/disable";
            var request = HttpRequestBuilder.CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator Enable2FABackupCodes( ResultCallback<TwoFACode> callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<TwoFACode> result = response.TryParseJson<TwoFACode>();
            callback.Try(result); 
        }

        public IEnumerator GetBackUpCode( ResultCallback<TwoFACode> callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode";
            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken) 
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<TwoFACode> result = response.TryParseJson<TwoFACode>();
            callback.Try(result);
        }

        public IEnumerator GetUserEnabledFactors( ResultCallback<Enable2FAFactors> callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor";
            
            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<Enable2FAFactors> result = response.TryParseJson<Enable2FAFactors>();
            callback.Try(result);
        }
        
        public IEnumerator Make2FAFactorDefault( TwoFAFactorType factor
            , ResultCallback callback )
        {
            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor";
            
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithFormParam("factor", factor.GetString())
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }      

        public IEnumerator GetInputValidations( string languageCode
            , ResultCallback<InputValidation> callback
            , bool defaultOnEmpty = true )
        {
            Assert.IsNotNull(languageCode, nameof(languageCode) + " cannot be null");

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/v3/public/inputValidations")
               .WithQueryParam("languageCode", languageCode)
               .WithQueryParam("defaultOnEmpty", defaultOnEmpty ? "true" : "false")
               .WithBearerAuth(AuthToken)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<InputValidation>();

            callback.Try(result);
        }

        public IEnumerator UpdateUser(UpdateUserRequest updateUserRequest
            , ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(updateUserRequest, "Update failed. updateEmailRequest is null!");

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v3/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }
        
        public IEnumerator GetPublisherUser(string userId
            , ResultCallback<GetPublisherUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userId, "Get Publisher User failed. userId is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/publisher")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GetPublisherUserResponse>();
            callback.Try(result);
        }
        public IEnumerator GetUserInformation(string userId
            , ResultCallback<GetUserInformationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userId, "Get User Information failed. userId is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/information")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GetUserInformationResponse>();
            callback.Try(result);
        }
        
        public IEnumerator LinkHeadlessAccountToCurrentFullAccount(LinkHeadlessAccountRequest linkHeadlessAccountRequest
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/public/users/me/headless/linkWithProgression")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(linkHeadlessAccountRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            Result result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator GetConflictResultWhenLinkHeadlessAccountToFullAccount(string oneTimeLinkCode
            , ResultCallback<ConflictLinkHeadlessAccountResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me/headless/link/conflict")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("oneTimeLinkCode", oneTimeLinkCode)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ConflictLinkHeadlessAccountResult>();
            callback.Try(result);
        }
    }
}
