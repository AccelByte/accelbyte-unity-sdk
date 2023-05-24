// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
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
        [UnityEngine.Scripting.Preserve]
        public UserApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : this(httpClient, config, session, null)
        {
        }

        [UnityEngine.Scripting.Preserve]
        public UserApi( IHttpClient httpClient
            , Config config
            , ISession session
            , HttpOperator httpOperator) 
            : base( httpClient, config, config.IamServerUrl, session , httpOperator)
        {
        }

        public void Register(RegisterUserRequest requestModel, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Register failed. registerUserRequest is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<RegisterUserResponse>();
                callback.Try(result);
            });
        }

        public void RegisterV2(RegisterUserRequestv2 requestModel, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Register failed. registerUserRequest is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/users")
                 .WithPathParam("namespace", Namespace_)
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBody(requestModel.ToUtf8Json())
                 .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<RegisterUserResponse>();
                callback.Try(result);
            });
        }

        public void GetData(ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me")
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void Update(UpdateUserRequest updateUserRequest, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (updateUserRequest == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Update failed. updateUserRequest is null!"));
                return;
            }
            if (!string.IsNullOrEmpty(updateUserRequest.emailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest,"Cannot update user email using this function. Use UpdateEmail instead."));
                return;
            }

            var request = HttpRequestBuilder.CreatePatch(BaseUrl + "/v4/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void UpdateEmail(UpdateEmailRequest updateEmailRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (updateEmailRequest == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Update failed. updateEmailRequest is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v4/public/namespaces/{namespace}/users/me/email")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateEmailRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void Upgrade(UpgradeRequest requestModel, UpgradeParameter requestParameter, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! Email address/username parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.Password))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! password parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("needVerificationCode", requestParameter.NeedVerificationCode ? "true" : "false")
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void UpgradeV2(UpgradeV2Request requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! EmailAddress parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.Password))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! password parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.Username))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! UserName parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void UpgradeAndVerifyHeadlessAccount(UpgradeAndVerifyHeadlessRequest requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.code))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! code parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.emailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! emailAddress parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.password))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! password parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.username))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! username parameter is null!"));
            }

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/code/verify";

            var request = HttpRequestBuilder
                .CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void SendVerificationCode(SendVerificationCodeRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't send verification code! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't send verification code! Username parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/code/request")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void Verify(VerifyRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't post verification code! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.VerificationCode))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't post verification code! Verification Code parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.ContactType))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't post verification code! Contact Type parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/code/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void SendPasswordResetCode(SendPasswordResetCodeRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't request reset password code! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't request reset password code! emailAddress parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/forgot";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void ResetPassword(ResetPasswordRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.ResetCode))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! Reset Code parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! Email address parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.NewPassword))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! New password parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/reset";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void LinkOtherPlatform(LinkOtherPlatformRequest requestModel, LinkOtherPlatformParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.PlatformId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Platform Id parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.Ticket))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Password parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}";

            var request = HttpRequestBuilder
                .CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", requestModel.PlatformId)
                .WithFormParam("ticket", requestParameter.Ticket)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationForm)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void ForcedLinkOtherPlatform(LinkPlatformAccountRequest requestModel, LinkPlatformAccountParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.platformId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Platform Id parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.platformUserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Platform User Id parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.UserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! User Id parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/platforms/link")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestParameter.UserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void UnlinkOtherPlatform(UnlinkPlatformAccountRequest requestModel, UnlinkPlatformAccountParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't unlink platform account! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.PlatformId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't unlink platform account! Platform Id parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}";

            string inNamespace = string.IsNullOrEmpty(requestModel.platformNamespace) ? Namespace_ : requestModel.platformNamespace;

            var builder = HttpRequestBuilder
                .CreateDelete(url)
                .WithPathParam("namespace", inNamespace)
                .WithPathParam("platformId", requestParameter.PlatformId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(requestModel.platformNamespace))
            {
                builder.WithBody(requestModel.ToUtf8Json());
            }

            IHttpRequest request = builder.GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void GetPlatformLinks(GetPlatformLinkRequest requestModel, ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get platform link! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get platform account link! User Id parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/platforms")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestModel.UserId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PagedPlatformLinks>();
                callback.Try(result);
            });
        }

        public void SearchUsers(SearchUsersRequest requestModel, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.Query))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Query parameter is null!"));
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("query", requestModel.Query)
                .WithQueryParam("offset", (requestModel.Offset >= 0) ? requestModel.Offset.ToString() : "")
                .WithQueryParam("limit", (requestModel.Limit >= 0) ? requestModel.Limit.ToString() : "")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);
            
            if (requestModel.SearchBy != SearchType.ALL)
            {
                builder.WithQueryParam("by", requestModel.FilterType[(int)requestModel.SearchBy]);
            }

            IHttpRequest request = builder.GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PagedPublicUsersInfo>();
                callback.Try(result);
            });
        }

        public void GetUserByUserId(GetUserByUserIdRequest requestModel, ResultCallback<PublicUserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! User Id parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestModel.UserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PublicUserData>();
                callback.Try(result);
            });
        }

        public void GetUserByOtherPlatformUserId(GetUserByOtherPlatformUserIdRequest requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.PlatformId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Platform Id is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.PlatformUserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Platform User Ids is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/platforms/{platformId}/users/{platformUserId}";

            var request = HttpRequestBuilder
                .CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", requestModel.PlatformId)
                .WithPathParam("platformUserId", requestModel.PlatformUserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void BulkGetUserByOtherPlatformUserIds(BulkPlatformUserIdRequest requestModel, BulkPlatformUserIdParameter requestParameter, ResultCallback<BulkPlatformUserIdResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.PlatformId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Platform Id is null!"));
                return;
            }
            if (requestModel.platformUserIDs == null || requestModel.platformUserIDs.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Platform User Ids are null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/platforms/{platformId}/users")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", requestParameter.PlatformId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<BulkPlatformUserIdResponse>();
                callback.Try(result);
            });
        }

        public void GetCountryFromIP(ResultCallback<CountryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/location/country")
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<CountryInfo>();
                callback.Try(result);
            });
        }

        public void BulkGetUserInfo(ListBulkUserInfoRequest requestModel, ResultCallback<ListBulkUserInfoResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (requestModel.userIds == null || requestModel.userIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! User Ids are null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/bulk/basic")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ListBulkUserInfoResponse>();
                callback.Try(result);
            });
        }

        public void Change2FAFactor(Change2FAFactorParameter requestModel, ResultCallback<TokenData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't change MFA Factor! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.MfaToken))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't change MFA Factor! Mfa Token parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.Factor))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't change MFA Factor! Factor parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/mfa/factor/change")
                .WithBearerAuth(AuthToken)
                .WithFormParam("mfaToken", requestModel.MfaToken)
                .WithFormParam("factor", requestModel.Factor)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<TokenData>();
                callback.Try(result);
            });
        }

        public void Disable2FAAuthenticator(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/disable";
            var request = HttpRequestBuilder.CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void Enable2FAAuthenticator(Enable2FAAuthenticatorParameter requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't enable MFA! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.Code))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't enable MFA! Code parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithFormParam("code", requestModel.Code)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void GenerateSecretKeyFor3rdPartyAuthenticateApp(ResultCallback<SecretKey3rdPartyApp> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/key";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<SecretKey3rdPartyApp>();
                callback.Try(result);
            });
        }

        public void GenerateBackUpCode(ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback.Try(result);
            });
        }

        public void Disable2FABackupCodes(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/disable";
            var request = HttpRequestBuilder.CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void Enable2FABackupCodes(ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback.Try(result);
            });
        }

        public void GetBackUpCode(ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode";
            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback.Try(result);
            });
        }

        public void GetUserEnabledFactors(ResultCallback<Enable2FAFactors> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor";

            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<Enable2FAFactors>();
                callback.Try(result);
            });
        }

        public void Make2FAFactorDefault(Make2FAFactorDefaultParameter requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't set default MFA! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.FactorType))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't set default MFA! Factor Type parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithFormParam("factor", requestModel.FactorType)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void GetInputValidations(GetInputValidationsParameter requestModel, ResultCallback<InputValidation> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't request input validation! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.LanguageCode))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't request input validation! Language Code parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/v3/public/inputValidations")
               .WithQueryParam("languageCode", requestModel.LanguageCode)
               .WithQueryParam("defaultOnEmpty", requestModel.DefaultOnEmpty ? "true" : "false")
               .WithBearerAuth(AuthToken)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<InputValidation>();
                callback.Try(result);
            });
        }

        public void UpdateUser(UpdateUserRequest requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't update user! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.emailAddress))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Cannot update user email using this function. Use UpdateEmail instead."));
                return;
            }

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v3/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback.Try(result);
            });
        }

        public void GetPublisherUser(GetPublisherUserParameter requestModel, ResultCallback<GetPublisherUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Get Publisher User failed! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Get Publisher User failed. User Id is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/publisher")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestModel.UserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetPublisherUserResponse>();
                callback.Try(result);
            });
        }

        public void GetUserInformation(GetUserInformationParameter requestModel, ResultCallback<GetUserInformationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Get User Information failed! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Get User Information failed. User Id is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/information")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestModel.UserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetUserInformationResponse>();
                callback.Try(result);
            });
        }

        public void LinkHeadlessAccountToCurrentFullAccount(LinkHeadlessAccountRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't link account! request is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/public/users/me/headless/linkWithProgression")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void GetConflictResultWhenLinkHeadlessAccountToFullAccount(
            GetConflictResultWhenLinkHeadlessAccountToFullAccountRequest requestModel,
            ResultCallback<ConflictLinkHeadlessAccountResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (requestModel == null)
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get headless link conflict. request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.OneTimeLinkCode))
            {
                callback.TryError(new Error(ErrorCode.BadRequest, "Can't get headless link conflict. One Time Link Code is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me/headless/link/conflict")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("oneTimeLinkCode", requestModel.OneTimeLinkCode)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ConflictLinkHeadlessAccountResult>();
                callback.Try(result);
            });
        }
    }
}
