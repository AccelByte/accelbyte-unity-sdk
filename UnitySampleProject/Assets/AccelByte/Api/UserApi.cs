// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Net;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;


namespace AccelByte.Api
{
    internal class UserApi
    {
        private readonly string baseUrl;
        
        internal UserApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Cannot construct UserManagement! baseUrl parameter is null!");
            this.baseUrl = baseUrl;
        }
        
        public IEnumerator<ITask> Register(string @namespace, string clientAccessToken, RegisterUserRequest 
        registerUserRequest,
            ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(@namespace, "Can't create user! Namespace parameter is null!");
            Assert.IsNotNull(clientAccessToken, "Can't create user! ClientAccessToken parameter is null!");
            Assert.IsNotNull(registerUserRequest, "Can't create user! Info parameter is null!");

            string jsonInfo = string.Format(
                "{{" +
                "\"AuthType\": \"{0}\"," +
                "\"DisplayName\": \"{1}\"," +
                "\"LoginId\": \"{2}\"," +
                "\"Password\": \"{3}\"" +
                "}}",
                registerUserRequest.AuthType,
                registerUserRequest.DisplayName,
                registerUserRequest.UserName,
                registerUserRequest.Password);

            HttpWebRequest request =
                HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users")
                    .WithPathParam("namespace", @namespace)
                    .WithBearerAuth(clientAccessToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(jsonInfo)
                    .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJsonBody<UserData>();
            callback.Try(result);
        }
        
        public IEnumerator<ITask> GetData(string @namespace, string userId, string userAccessToken,
            ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user data! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user data! UserAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't get user data! UserId parameter is null!");

            HttpWebRequest request =
                HttpRequestBuilder.CreateGet(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}")
                    .WithPathParam("namespace", @namespace)
                    .WithPathParam("userId", userId)
                    .WithBearerAuth(userAccessToken)
                    .Accepts(MediaType.ApplicationJson)
                    .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJsonBody<UserData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> Update(string @namespace, string userId, string userAccessToken,
            UpdateUserRequest updateUserRequest, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(@namespace, "Can't update user! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't update user! UserAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't update user! UserId parameter is null!");
            Assert.IsNotNull(updateUserRequest, "Can't update user! Request parameter is null!");

            string strUpdateRequest = SimpleJson.SimpleJson.SerializeObject(updateUserRequest);

            HttpWebRequest request =
                HttpRequestBuilder.CreatePut(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}")
                    .WithPathParam("namespace", @namespace)
                    .WithPathParam("userId", userId)
                    .WithBearerAuth(userAccessToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(strUpdateRequest)
                    .Accepts(MediaType.ApplicationJson)
                    .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJsonBody<UserData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> Upgrade(string @namespace, string userAccessToken,
            string userId, string userName, string password, ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(@namespace, "Can't upgrade headless account! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't upgrade headless account! ClientAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't upgrade headless account! UserId parameter is null!");
            Assert.IsNotNull(userName, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/upgradeHeadlessAccount")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"LoginID\": \"{0}\", \"Password\": \"{1}\"}}", userName, password))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJsonBody<UserData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> SendVerificationCode(string @namespace, string userAccessToken, 
            string userId, VerificationContext context, string userName, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't send verification code! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't send verification code! UserAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't send verification code! UserId parameter is null!");
            Assert.IsNotNull(userName, "Can't send verification code! Username parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/verificationcode")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"LoginID\": \"{0}\", \"Context\": \"{1:G}\"}}", userName, context))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator<ITask> Verify(string @namespace, string userAccessToken, string userId,
            string verificationCode, string contactType, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't post verification code! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't post verification code! UserAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't post verification code! UserId parameter is null!");
            Assert.IsNotNull(verificationCode, "Can't post verification code! VerificationCode parameter is null!");
            Assert.IsNotNull(contactType, "Can't post verification code! ContactType parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/verification")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"Code\": \"{0}\", \"ContactType\": \"{1}\"}}", verificationCode,
                    contactType))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator<ITask> UpgradeAndVerify(string @namespace,
            string userAccessToken, string userId, string userName, string password, string verificationCode,
            ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(@namespace, "Can't upgrade headless account! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't upgrade headless account! ClientAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't upgrade headless account! UserId parameter is null!");
            Assert.IsNotNull(userName, "Can't upgrade headless account! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't upgrade headless account! Password parameter is null!");
            Assert.IsNotNull(verificationCode, "Can't upgrade headless account! Password parameter is null!");

            string requestBody = string.Format("{{\"Code\":\"{0}\", \"LoginID\": \"{1}\", \"Password\": \"{2}\"}}",
                verificationCode, userName, password);

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/upgradeHeadlessAccountWithVerificationCode")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserData> result = response.TryParseJsonBody<UserData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> SendPasswordResetCode(string @namespace, string clientId, 
            string clientSecret, string userName, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't request reset password code! Namespace parameter is null!");
            Assert.IsNotNull(clientId, "Can't reset password! clientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't reset password! clientSecret parameter is null!");
            Assert.IsNotNull(userName, "Can't request reset password code! LoginId parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/forgotPassword")
                .WithPathParam("namespace", @namespace)
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"LoginID\": \"{0}\"}}", userName))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator<ITask> ResetPassword(string @namespace, string clientId, string clientSecret,
            string resetCode, string userName, string newPassword, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't reset password! Namespace parameter is null!");
            Assert.IsNotNull(clientId, "Can't reset password! clientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't reset password! clientSecret parameter is null!");

            string jsonResetRequest = string.Format(
                "{{" +
                "\"Code\": \"{0}\"," +
                "\"LoginID\": \"{1}\"," +
                "\"NewPassword\": \"{2}\"" +
                "}}",
                resetCode,
                userName,
                newPassword);
            
            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/resetPassword")
                .WithPathParam("namespace", @namespace)
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(jsonResetRequest)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator<ITask> LinkOtherPlatform(string @namespace, string userAccessToken, string userId, 
            string platformId, string ticket, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't link platfrom account! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't link platfrom account! UserAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't link platfrom account! UserId parameter is null!");
            Assert.IsNotNull(platformId, "Can't link platfrom account! Email parameter is null!");
            Assert.IsNotNull(ticket, "Can't link platfrom account! Password parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("platformId", platformId)
                .WithFormParam("ticket", ticket)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationForm)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator<ITask> UnlinkOtherPlatform(string @namespace, string userAccessToken, string userId,
            string platformId, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't unlink platfrom account! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't unlink platfrom account! UserAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't unlink platfrom account! UserId parameter is null!");
            Assert.IsNotNull(platformId, "Can't unlink platfrom account! Email parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/platforms/{platformId}/unlink")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("platformId", platformId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.TextPlain)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetPlatformLinks(string @namespace, string userAccessToken,
            string userId, ResultCallback<PlatformLink[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get linked platfrom account! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken,
                "Can't get linked platfrom account! ClientAccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't get linked platfrom account! UserId parameter is null!");
            
            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/iam/namespaces/{namespace}/users/{userId}/platforms")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<PlatformLink[]> result = response.TryParseJsonBody<PlatformLink[]>();
            callback.Try(result);
        }
    }
}
