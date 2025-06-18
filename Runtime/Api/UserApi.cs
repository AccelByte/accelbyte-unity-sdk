// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new RegisterUserOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Register(requestModel, optionalParameters, callback);
        }

        internal void Register(RegisterUserRequest requestModel, RegisterUserOptionalParameters optionalParameters, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , requestModel
                , requestModel.authType
                , requestModel.country
                , requestModel.emailAddress
                , requestModel.password);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<RegisterUserResponse>();
                callback?.Try(result);
            });
        }

        public void RegisterV2(RegisterUserRequestv2 requestModel, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new RegisterUserOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            RegisterV2(requestModel, optionalParameters, callback);
        }

        internal void RegisterV2(RegisterUserRequestv2 requestModel, RegisterUserOptionalParameters optionalParameters, ResultCallback<RegisterUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , requestModel
                , requestModel.authType
                , requestModel.emailAddress
                , requestModel.username
                , requestModel.password
                , requestModel.country);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/users")
                 .WithPathParam("namespace", Namespace_)
                 .WithContentType(MediaType.ApplicationJson)
                 .WithBody(requestModel.ToUtf8Json())
                 .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<RegisterUserResponse>();
                callback?.Try(result);
            });
        }

        public void SendVerificationCodeToNewUser(string emailAddress
            , SendVerificationCodeToNewUserOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, emailAddress);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new SendVerificationCodeToNewUserRequest()
            {
                EmailAddress = emailAddress,
                LanguageTag = optionalParameters?.LanguageTag
            };

            var request =
                HttpRequestBuilder.CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/code/request")
                    .WithPathParam("namespace", Namespace_)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(requestBody.ToUtf8Json())
                    .GetResult();

            var additionalParams = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void GetData(ResultCallback<UserData> callback, bool isIncludeAllPlatforms = false)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me")
                .WithQueryParam("includeAllPlatforms", isIncludeAllPlatforms.ToString().ToLower())
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        internal void GetData(GetOrRefreshDataOptionalParameters optionalParameters, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var isIncludeAllPlatforms = optionalParameters?.IsIncludeAllPlatforms is true ? "true" : "false";

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me")
                .WithQueryParam("includeAllPlatforms", isIncludeAllPlatforms)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void Update(UpdateUserRequest updateUserRequest, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new UpdateUserDataOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Update(updateUserRequest, optionalParameters, callback);
        }

        internal void Update(UpdateUserRequest updateUserRequest, UpdateUserDataOptionalParameters optionalParameters, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            
            if (updateUserRequest == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Update failed. updateUserRequest is null!"));
                return;
            }
            if (!string.IsNullOrEmpty(updateUserRequest.emailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Cannot update user email using this function. Use UpdateEmail instead."));
                return;
            }

            var request = HttpRequestBuilder.CreatePatch(BaseUrl + "/v4/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(
                additionalParameters
                , request
                , response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void UpdateEmail(UpdateEmailRequest updateEmailRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new UpdateEmailOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateEmail(updateEmailRequest, optionalParameters, callback);
        }

        internal void UpdateEmail(UpdateEmailRequest updateEmailRequest, UpdateEmailOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (updateEmailRequest == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Update failed. updateEmailRequest is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v4/public/namespaces/{namespace}/users/me/email")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateEmailRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void Upgrade(UpgradeRequest requestModel, UpgradeParameter requestParameter, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new UpgradeHeadlessAccountOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Upgrade(requestModel, requestParameter, optionalParameters, callback);
        }

        internal void Upgrade(UpgradeRequest requestModel
            , UpgradeParameter requestParameter
            , UpgradeHeadlessAccountOptionalParameters optionalParameters
            , ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! Email address/username parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.Password))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! password parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("needVerificationCode", requestParameter.NeedVerificationCode.ToString().ToLower())
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void UpgradeV2(UpgradeV2Request requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new UpgradeHeadlessAccountV2OptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpgradeV2(requestModel, optionalParameters, callback);
        }

        internal void UpgradeV2(UpgradeV2Request requestModel, UpgradeHeadlessAccountV2OptionalParameters optionalParameters, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! EmailAddress parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.Password))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade headless account! password parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void UpgradeAndVerifyHeadlessAccount(UpgradeAndVerifyHeadlessRequest requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new UpgradeAndVerifyHeadlessAccountOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpgradeAndVerifyHeadlessAccount(requestModel, optionalParameters, callback);
        }

        internal void UpgradeAndVerifyHeadlessAccount(UpgradeAndVerifyHeadlessRequest requestModel, UpgradeAndVerifyHeadlessAccountOptionalParameters optionalParameters, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.code))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! code parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.emailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! emailAddress parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.password))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! password parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.username))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't upgrade the user! username parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/headless/code/verify";

            var request = HttpRequestBuilder
                .CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void SendVerificationCode(SendVerificationCodeRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new SendUpgradeVerificationCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            SendVerificationCode(requestModel, optionalParameters, callback);
        }

        internal void SendVerificationCode(SendVerificationCodeRequest requestModel, SendUpgradeVerificationCodeOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't send verification code! request is null!"));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't send verification code! Username parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/code/request")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void Verify(VerifyRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new VerifyUserEmailOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Verify(requestModel, optionalParameters, callback);
        }

        internal void Verify(VerifyRequest requestModel, VerifyUserEmailOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't post verification code! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.VerificationCode))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't post verification code! Verification Code parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.ContactType))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't post verification code! Contact Type parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/me/code/verify")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void SendPasswordResetCode(SendPasswordResetCodeRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new SendPasswordResetCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            SendPasswordResetCode(requestModel, optionalParameters, callback);
        }

        internal void SendPasswordResetCode(SendPasswordResetCodeRequest requestModel, SendPasswordResetCodeOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't request reset password code! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't request reset password code! emailAddress parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/forgot";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void ResetPassword(ResetPasswordRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new ResetPasswordOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetPassword(requestModel, optionalParameters, callback);
        }

        internal void ResetPassword(ResetPasswordRequest requestModel, ResetPasswordOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.ResetCode))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! Reset Code parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.EmailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! Email address parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.NewPassword))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't reset password! New password parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/reset";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void LinkOtherPlatform(LinkOtherPlatformRequest requestModel, LinkOtherPlatformParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new LinkOtherPlatformOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            LinkOtherPlatform(requestModel, requestParameter, optionalParameters, callback);
        }

        internal void LinkOtherPlatform(LinkOtherPlatformRequest requestModel
            , LinkOtherPlatformParameter requestParameter
            , LinkOtherPlatformOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.PlatformId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Platform Id parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.Ticket))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Password parameter is null!"));
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
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void ForcedLinkOtherPlatform(LinkPlatformAccountRequest requestModel, LinkPlatformAccountParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new ForceLinkOtherPlatformOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ForcedLinkOtherPlatform(requestModel, requestParameter, optionalParameters, callback);
        }

        /// <summary>
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal void ForcedLinkOtherPlatform(LinkPlatformAccountRequest requestModel
            , LinkPlatformAccountParameter requestParameter
            , ForceLinkOtherPlatformOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.platformId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Platform Id parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.platformUserId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! Platform User Id parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.UserId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link platform account! User Id parameter is null!"));
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
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void UnlinkOtherPlatform(UnlinkPlatformAccountRequest requestModel, UnlinkPlatformAccountParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new UnlinkOtherPlatformOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UnlinkOtherPlatform(requestModel, requestParameter, optionalParameters, callback);
        }

        internal void UnlinkOtherPlatform(UnlinkPlatformAccountRequest requestModel
            , UnlinkPlatformAccountParameter requestParameter
            , UnlinkOtherPlatformOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't unlink platform account! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.PlatformId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't unlink platform account! Platform Id parameter is null!"));
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
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void UnlinkAllOtherPlatform(UnlinkPlatformAccountParameter requestParameter, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UnlinkAllOtherPlatformOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UnlinkAllOtherPlatform(requestParameter, optionalParameters, callback);
        }

        internal void UnlinkAllOtherPlatform(UnlinkPlatformAccountParameter requestParameter, UnlinkAllOtherPlatformOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (string.IsNullOrEmpty(requestParameter.PlatformId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't unlink platform account! Platform Id parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v3/public/namespaces/{namespace}/users/me/platforms/{platformId}/all";

            var builder = HttpRequestBuilder
                .CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", requestParameter.PlatformId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson);

            IHttpRequest request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void GetPlatformLinks(GetPlatformLinkRequest requestModel
            , ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new GetPlatformLinksOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetPlatformLinks(requestModel, optionalParameters, callback);
        }

        internal void GetPlatformLinks(GetPlatformLinkRequest requestModel
            , GetPlatformLinksOptionalParameters optionalParameters
            , ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get platform link! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get platform account link! User Id parameter is null!"));
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
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedPlatformLinks>();
                callback?.Try(result);
            });
        }

        public void SearchUsers(SearchUsersRequest requestModel, ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new SearchUsersOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            SearchUsers(requestModel, optionalParameters, callback);
        }

        internal void SearchUsers(SearchUsersRequest requestModel
            , SearchUsersOptionalParameters optionalParameters
            , ResultCallback<PagedPublicUsersInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.Query))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Query parameter is null!"));
                return;
            }

            string platformBy = PlatformSearchTypeEnumToString(requestModel.PlatformBy);

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("query", requestModel.Query)
                .WithQueryParam("offset", (requestModel.Offset >= 0) ? requestModel.Offset.ToString() : "")
                .WithQueryParam("limit", (requestModel.Limit >= 0) ? requestModel.Limit.ToString() : "")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(requestModel.PlatformId))
            {
                builder.WithQueryParam("platformId", requestModel.PlatformId);
            }

            if (!string.IsNullOrEmpty(platformBy))
            {
                builder.WithQueryParam("platformBy", platformBy);
            }

            if (requestModel.SearchBy != SearchType.ALL)
            {
                builder.WithQueryParam("by", requestModel.FilterType[(int)requestModel.SearchBy]);
            }

            IHttpRequest request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedPublicUsersInfo>();
                callback?.Try(result);
            });
        }

        public void GetUserPublicInfo(string userId, ResultCallback<GetUserPublicInfoResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            GetUserPublicInfo(userId, null, callback);
        }
        
        internal void GetUserPublicInfo(string userId, GetUserPublicInfoOptionalParameters optionalParams, ResultCallback<GetUserPublicInfoResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(userId);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v4/public/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            AdditionalHttpParameters additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);

            HttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParseJson<GetUserPublicInfoResponse>();
                callback?.Try(result);
            });
        }

        internal void GetUserByOtherPlatformUserIdV4(string platformId, string platformUserId, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserByOtherPlatformUserIdV4OptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetUserByOtherPlatformUserIdV4(platformId, platformUserId, optionalParameters, callback);
        }

        internal void GetUserByOtherPlatformUserIdV4(string platformId
            , string platformUserId
            , GetUserByOtherPlatformUserIdV4OptionalParameters optionalParameters
            , ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(platformId
                , platformUserId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v4/public/namespaces/{namespace}/platforms/{platformId}/users/{platformUserId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", platformId)
                .WithPathParam("platformUserId", platformUserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void BulkGetUserByOtherPlatformUserIdsV4(BulkPlatformUserIdRequest requestModel, BulkPlatformUserIdParameter requestParameter, ResultCallback<BulkPlatformUserIdResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new BulkGetUserByOtherPlatformUserIdsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkGetUserByOtherPlatformUserIdsV4(requestModel, requestParameter, optionalParameters, callback);
        }

        internal void BulkGetUserByOtherPlatformUserIdsV4(BulkPlatformUserIdRequest requestModel
            , BulkPlatformUserIdParameter requestParameter
            , BulkGetUserByOtherPlatformUserIdsOptionalParameters optionalParameters
            , ResultCallback<BulkPlatformUserIdResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestParameter.PlatformId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Platform Id is null!"));
                return;
            }
            if (requestModel.platformUserIDs == null || requestModel.platformUserIDs.Length == 0)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get user data! Platform User Ids are null!"));
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v4/public/namespaces/{namespace}/platforms/{platformId}/users")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("platformId", requestParameter.PlatformId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson);

            if (requestParameter.RawPuid != null)
            {
                builder = builder.WithQueryParam("rawPUID", requestParameter.RawPuid.Value.ToString());
            }
            IHttpRequest request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<BulkPlatformUserIdResponse>();
                callback?.Try(result);
            });
        }

        public void GetCountryFromIP(ResultCallback<CountryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetCountryFromIPOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetCountryFromIP(optionalParameters, callback);
        }

        internal void GetCountryFromIP(GetCountryFromIPOptionalParameters optionalParameters, ResultCallback<CountryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/location/country")
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<CountryInfo>();
                callback?.Try(result);
            });
        }

        public void Change2FAFactor(Change2FAFactorParameter requestModel, ResultCallback<TokenData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new Change2FAFactorOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Change2FAFactor(requestModel, optionalParameters, callback);
        }

        internal void Change2FAFactor(Change2FAFactorParameter requestModel
            , Change2FAFactorOptionalParameters optionalParameters
            , ResultCallback<TokenData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't change MFA Factor! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.MfaToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't change MFA Factor! Mfa Token parameter is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.Factor))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't change MFA Factor! Factor parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/mfa/factor/change")
                .WithBearerAuth(AuthToken)
                .WithFormParam("mfaToken", requestModel.MfaToken)
                .WithFormParam("factor", requestModel.Factor)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<TokenData>();
                callback?.Try(result);
            });
        }

        public void Disable2FAAuthenticator(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new Disable2FAAuthenticatorOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Disable2FAAuthenticator(optionalParameters, callback);
        }

        internal void Disable2FAAuthenticator(Disable2FAAuthenticatorOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/disable";
            var request = HttpRequestBuilder.CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void Enable2FAAuthenticator(Enable2FAAuthenticatorParameter requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new Enable2FAAuthenticatorOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Enable2FAAuthenticator(requestModel, optionalParameters, callback);
        }

        internal void Enable2FAAuthenticator(Enable2FAAuthenticatorParameter requestModel
            , Enable2FAAuthenticatorOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't enable MFA! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.Code))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't enable MFA! Code parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithFormParam("code", requestModel.Code)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void GenerateSecretKeyFor3rdPartyAuthenticateApp(ResultCallback<SecretKey3rdPartyApp> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GenerateSecretKeyFor3rdPartyAuthenticateAppOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GenerateSecretKeyFor3rdPartyAuthenticateApp(optionalParameters, callback);
        }

        internal void GenerateSecretKeyFor3rdPartyAuthenticateApp(GenerateSecretKeyFor3rdPartyAuthenticateAppOptionalParameters optionalParameters
            , ResultCallback<SecretKey3rdPartyApp> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/authenticator/key";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<SecretKey3rdPartyApp>();
                callback?.Try(result);
            });
        }

        public void GenerateBackUpCode(ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GenerateBackUpCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GenerateBackUpCode(optionalParameters, callback);
        }

        internal void GenerateBackUpCode(GenerateBackUpCodeOptionalParameters optionalParameters, ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback?.Try(result);
            });
        }

        public void Disable2FABackupCodes(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new Disable2FABackupCodesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Disable2FABackupCodes(optionalParameters, callback);
        }

        internal void Disable2FABackupCodes(Disable2FABackupCodesOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/disable";
            var request = HttpRequestBuilder.CreateDelete(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void Enable2FABackupCodes(ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback?.Try(result);
            });
        }

        internal void Enable2FABackupCodes(Enable2FABackupCodesOptionalParameters optionalParameters, ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode/enable";
            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback?.Try(result);
            });
        }

        public void GetBackUpCode(ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetBackUpCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetBackUpCode(optionalParameters, callback);
        }

        internal void GetBackUpCode(GetBackUpCodeOptionalParameters optionalParameters, ResultCallback<TwoFACode> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/backupCode";
            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<TwoFACode>();
                callback?.Try(result);
            });
        }

        public void GetUserEnabledFactors(ResultCallback<Enable2FAFactors> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserEnabledFactorsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetUserEnabledFactors(optionalParameters, callback);
        }

        internal void GetUserEnabledFactors(GetUserEnabledFactorsOptionalParameters optionalParameters, ResultCallback<Enable2FAFactors> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor";

            var request = HttpRequestBuilder.CreateGet(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<Enable2FAFactors>();
                callback?.Try(result);
            });
        }

        public void Make2FAFactorDefault(Make2FAFactorDefaultParameter requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new Make2FAFactorDefaultOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            Make2FAFactorDefault(requestModel, optionalParameters, callback);
        }

        internal void Make2FAFactorDefault(Make2FAFactorDefaultParameter requestModel
            , Make2FAFactorDefaultOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't set default MFA! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.FactorType))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't set default MFA! Factor Type parameter is null!"));
                return;
            }

            string url = BaseUrl + "/v4/public/namespaces/{namespace}/users/me/mfa/factor";

            var request = HttpRequestBuilder.CreatePost(url)
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithFormParam("factor", requestModel.FactorType)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void GetInputValidations(GetInputValidationsParameter requestModel, ResultCallback<InputValidation> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new GetInputValidationsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetInputValidations(requestModel, optionalParameters, callback);
        }

        internal void GetInputValidations(GetInputValidationsParameter requestModel
            , GetInputValidationsOptionalParameters optionalParameters
            , ResultCallback<InputValidation> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't request input validation! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.LanguageCode))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't request input validation! Language Code parameter is null!"));
                return;
            }

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/v3/public/inputValidations")
               .WithQueryParam("languageCode", requestModel.LanguageCode)
               .WithQueryParam("defaultOnEmpty", requestModel.DefaultOnEmpty ? "true" : "false")
               .WithBearerAuth(AuthToken)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<InputValidation>();
                callback?.Try(result);
            });
        }

        public void UpdateUser(UpdateUserRequest requestModel, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateUser(requestModel, optionalParameters, callback);
        }

        internal void UpdateUser(UpdateUserRequest requestModel, UpdateUserOptionalParameters optionalParameters, ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't update user! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.emailAddress))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Cannot update user email using this function. Use UpdateEmail instead."));
                return;
            }

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v3/public/namespaces/{namespace}/users/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserData>();
                callback?.Try(result);
            });
        }

        public void GetPublisherUser(GetPublisherUserParameter requestModel, ResultCallback<GetPublisherUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new GetPublisherUserOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetPublisherUser(requestModel, optionalParameters, callback);
        }

        internal void GetPublisherUser(GetPublisherUserParameter requestModel
            , GetPublisherUserOptionalParameters optionalParameters
            , ResultCallback<GetPublisherUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Get Publisher User failed! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Get Publisher User failed. User Id is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/publisher")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestModel.UserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GetPublisherUserResponse>();
                callback?.Try(result);
            });
        }

        public void GetUserInformation(GetUserInformationParameter requestModel, ResultCallback<GetUserInformationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new GetUserInformationOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetUserInformation(requestModel, optionalParameters, callback);
        }

        internal void GetUserInformation(GetUserInformationParameter requestModel
            , GetUserInformationOptionalParameters optionalParameters
            , ResultCallback<GetUserInformationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Get User Information failed! request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.UserId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Get User Information failed. User Id is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/{userId}/information")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", requestModel.UserId)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GetUserInformationResponse>();
                callback?.Try(result);
            });
        }

        public void LinkHeadlessAccountToCurrentFullAccount(LinkHeadlessAccountRequest requestModel, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new LinkHeadlessAccountToCurrentFullAccountOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            LinkHeadlessAccountToCurrentFullAccount(requestModel, optionalParameters, callback);
        }

        internal void LinkHeadlessAccountToCurrentFullAccount(LinkHeadlessAccountRequest requestModel
            , LinkHeadlessAccountToCurrentFullAccountOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't link account! request is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/public/users/me/headless/linkWithProgression")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void GetConflictResultWhenLinkHeadlessAccountToFullAccount(
            GetConflictResultWhenLinkHeadlessAccountToFullAccountRequest requestModel,
            ResultCallback<ConflictLinkHeadlessAccountResult> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            var optionalParameters = new GetConflictResultWhenLinkHeadlessAccountToFullAccountOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetConflictResultWhenLinkHeadlessAccountToFullAccount(requestModel, optionalParameters, callback);
        }

        internal void GetConflictResultWhenLinkHeadlessAccountToFullAccount(
            GetConflictResultWhenLinkHeadlessAccountToFullAccountRequest requestModel
            , GetConflictResultWhenLinkHeadlessAccountToFullAccountOptionalParameters optionalParameters
            , ResultCallback<ConflictLinkHeadlessAccountResult> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            if (requestModel == null)
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get headless link conflict. request is null!"));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.OneTimeLinkCode))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, "Can't get headless link conflict. One Time Link Code is null!"));
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/users/me/headless/link/conflict")
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("oneTimeLinkCode", requestModel.OneTimeLinkCode)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ConflictLinkHeadlessAccountResult>();
                callback?.Try(result);
            });
        }

        public void CheckUserAccountAvailability(string displayName,
            ResultCallback callback)
        {
            var optionalParameters = new CheckUserAccountAvailabilityByFieldNameOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CheckUserAccountAvailability(displayName, optionalParameters, callback);
        }

        internal void CheckUserAccountAvailability(string displayName
            , CheckUserAccountAvailabilityByFieldNameOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            CheckUserAccountAvailabilityByFieldName(displayName, "displayName", optionalParameters, callback);
        }

        public void GetConfigUniqueDisplayNameEnabled(ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetConfigValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetConfigUniqueDisplayNameEnabled(optionalParameters, callback);
        }

        internal void GetConfigUniqueDisplayNameEnabled(GetConfigValueOptionalParameters optionalParameters, ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            GetConfigValue<UniqueDisplayNameEnabledResponse>("uniqueDisplayNameEnabled", optionalParameters, result =>
            {
                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value.UniqueDisplayNameEnabled);
            });
        }

        public void GetConfigUserNameDisabled(ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetConfigValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetConfigUserNameDisabled(optionalParameters, callback);
        }

        internal void GetConfigUserNameDisabled(GetConfigValueOptionalParameters optionalParameters, ResultCallback<bool> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            GetConfigValue<DisplayNameDisabledResponse>("usernameDisabled", optionalParameters, result =>
            {
                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value.UserNameDisabled);
            });
        }

        private void GetConfigValue<T>(string configKey
            , GetConfigValueOptionalParameters optionalParameters
            , ResultCallback<T> callback) where T : class, new()
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/config/{configKey}")
                 .WithPathParam("namespace", Namespace_)
                 .WithPathParam("configKey", configKey)
                 .WithContentType(MediaType.ApplicationJson)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ConfigValueResponse<T>>();
                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.Try(Result<T>.CreateOk(result.Value.Result));
            });
        }

        internal void GetPublicSystemConfigValue(GetPublicSystemConfigValueOptionalParameters optionalParameters
            , ResultCallback<GetPublicSystemConfigValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/config/public")
                 .WithContentType(MediaType.ApplicationJson)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<GetPublicSystemConfigValueResponse>();
                    if (result.IsError)
                    {
                        callback?.TryError(result.Error);
                        return;
                    }

                    callback?.Try(Result<GetPublicSystemConfigValueResponse>.CreateOk(result.Value));
                });
        }

        public void GetUserOtherPlatformBasicPublicInfo(
            PlatformAccountInfoRequest requestPayload
            , ResultCallback<AccountUserPlatformInfosResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserOtherPlatformBasicPublicInfoOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetUserOtherPlatformBasicPublicInfo(requestPayload, optionalParameters, callback);
        }

        internal void GetUserOtherPlatformBasicPublicInfo(
            PlatformAccountInfoRequest requestPayload
            , GetUserOtherPlatformBasicPublicInfoOptionalParameters optionalParameters
            , ResultCallback<AccountUserPlatformInfosResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, Session.AuthorizationToken, requestPayload);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/platforms")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(requestPayload.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<AccountUserPlatformInfosResponse>();
                callback?.Try(result);
            });
        }

        public void CheckUserAccountAvailabilityByFieldName(string valueToCheck, string fieldName,
            ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CheckUserAccountAvailabilityByFieldNameOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CheckUserAccountAvailabilityByFieldName(valueToCheck, fieldName, optionalParameters, callback);
        }

        internal void CheckUserAccountAvailabilityByFieldName(string valueToCheck
            , string fieldName
            , CheckUserAccountAvailabilityByFieldNameOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, valueToCheck, fieldName);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/users/availability")
                 .WithPathParam("namespace", Namespace_)
                 .WithBearerAuth(Session.AuthorizationToken)
                 .WithContentType(MediaType.ApplicationJson)
                 .WithQueryParam("field", fieldName)
                 .WithQueryParam("query", valueToCheck)
                 .Accepts(MediaType.ApplicationJson)
                 .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                if (response.Code == (int)ErrorCode.NotFound)
                {
                    callback?.TryError(new Error(ErrorCode.NotFound,
                        $"Account doesn't exist. If a new account is added with the defined {fieldName}, " +
                        "the service will be able to perform the action."));
                }
                else
                {
                    var result = response.TryParse();
                    callback?.Try(result);
                }
            });
        }

        public void GetCountryGroupV3(ResultCallback<Country[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetCountryGroupV3OptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetCountryGroupV3(optionalParameters, callback);
        }

        internal void GetCountryGroupV3(GetCountryGroupV3OptionalParameters optionalParameters, ResultCallback<Country[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/countries")
                .WithPathParam("namespace", Namespace_)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<Country[]>();
                callback?.Try(result);
            });
        }

        public void ValidateUserInput(ValidateInputRequest requestBody, ResultCallback<ValidateInputResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            ValidateUserInput(requestBody, null, callback);
        }
        
        internal void ValidateUserInput(ValidateInputRequest requestBody, ValidateUserInputOptionalParameters optionalParams , ResultCallback<ValidateInputResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, requestBody);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v3/public/namespaces/{namespace}/users/input/validation")
                .WithPathParam("namespace", Namespace_)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);

            HttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParseJson<ValidateInputResponse>();
                callback?.Try(result);
            });
        }

        private string PlatformSearchTypeEnumToString(SearchPlatformType platformBy)
        {
            string retVal;
            switch (platformBy)
            {
                case SearchPlatformType.None:
                    retVal = string.Empty;
                    break;
                case SearchPlatformType.PlatformDisplayName:
                    retVal = "platformDisplayName";
                    break;
                default:
                    retVal = null;
                    break;
            }

            return retVal;
        }
    }
}