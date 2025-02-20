// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Linq;

namespace AccelByte.Server
{
    internal class ServerUserAccountApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param> // TODO: Should this base BaseUrl?
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerUserAccountApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.BaseUrl, session )
        {
        }

        public IEnumerator GetUserData( string userAuthToken
            , ResultCallback<UserData> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(userAuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/public/users/me")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(userAuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback?.Try(result);
        }

        public IEnumerator SearchUserOtherPlatformDisplayName( string platformDisplayName
            , PlatformType platformType
            , ResultCallback<PagedUserOtherPlatformInfo> callback
            , int limit
            , int offset )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , platformDisplayName);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            if (platformType == PlatformType.Nintendo || platformType == PlatformType.Oculus || platformType == PlatformType.Apple)
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest
                    , $"Can not search user using this function. Use SearchUserOtherPlatformUserId instead."));
                yield break;
            }

            string searchBy = "thirdPartyPlatform";
            string platformBy = "platformDisplayName";

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace_}/users/search")
                .WithPathParam("namespace_", Namespace_)
                .WithQueryParam("by", searchBy)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("platformBy", platformBy)
                .WithQueryParam("platformId", platformType.ToString().ToLower())
                .WithQueryParam("query", platformDisplayName)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PagedUserOtherPlatformInfo>();
            callback?.Try(result);
        }

        public IEnumerator SearchUserOtherPlatformUserId( string platformUserId
            , PlatformType platformType
            , ResultCallback<UserOtherPlatformInfo> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , platformUserId);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace_}/platforms/{platformId}/users/{platformUserId}")
                .WithPathParam("namespace_", Namespace_)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithPathParam("platformUserId", platformUserId)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserOtherPlatformInfo>();
            callback?.Try(result);
        }

        public IEnumerator BanUser(string userId, BanCreateRequest banRequest, ResultCallback<UserBanResponseV3> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , userId
                , banRequest);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
               .CreatePost(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/bans")
               .WithPathParam("namespace", Namespace_)
               .WithPathParam("userId", userId)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(banRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanResponseV3>();
            callback?.Try(result);
        }

        public IEnumerator ChangeUserBanStatus(string userId, string banId, bool enabled, ResultCallback<UserBanResponseV3> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , userId
                , banId);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            UserEnableBan changeRequest = new UserEnableBan { enabled = enabled };

            var request = HttpRequestBuilder
               .CreatePatch(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/bans/{banId}")
               .WithPathParam("namespace", Namespace_)
               .WithPathParam("userId", userId)
               .WithPathParam("banId", banId)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(changeRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanResponseV3>();
            callback?.Try(result);
        }

        public IEnumerator GetUserBanInfo(string userId, bool activeOnly, ResultCallback<UserBanPagedList> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/bans")
               .WithPathParam("namespace", Namespace_)
               .WithPathParam("userId", userId)
               .WithQueryParam("activeOnly", activeOnly ? "true" : "false")
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanPagedList>();
            callback?.Try(result);
        }

        public IEnumerator GetUserBannedList(bool activeOnly, BanType banType, int offset, int limit, ResultCallback<UserBanPagedList> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/bans/users")
               .WithPathParam("namespace", Namespace_)
               .WithQueryParam("activeOnly", activeOnly ? "true" : "false")
               .WithQueryParam("banType", banType.ToString())
               .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
               .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanPagedList>();
            callback?.Try(result);
        }

        public IEnumerator GetUserByUserId(string userId 
            , ResultCallback<UserData> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , userId);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback?.Try(result);
        }

        public IEnumerator ListUserByUserId(ListUserDataRequest listUserDataRequest, ResultCallback<ListUserDataResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , listUserDataRequest);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
               .CreatePost(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/bulk")
               .WithPathParam("namespace", Namespace_)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(listUserDataRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<ListUserDataResponse>();
            callback?.Try(result);
        }

        internal void GetBulkUserByEmailAddress(IEnumerable<string> emailAddresses
            , ResultCallback<GetBulkUserByEmailAddressResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , emailAddresses);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            emailAddresses = emailAddresses.Where(email => !string.IsNullOrEmpty(email)).ToArray();

            if (emailAddresses.Count() < 1)
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "Email addresses cannot be empty"));
                return;
            }

            var body = new GetBulkUserByEmailAddressRequest()
            {
                ListEmailAddressRequest = emailAddresses.ToArray()
            };

            var request = HttpRequestBuilder
               .CreatePost(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/search/bulk")
               .WithPathParam("namespace", Namespace_)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(body.ToUtf8Json())
               .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetBulkUserByEmailAddressResponse>();
                callback?.Try(result);
            });
        }

        internal void GetLinkedPlatformAccounts(string userId
            , GetLinkedPlatformAccountsOptionalParams optionalParameters
            , ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , userId);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var queries = new Dictionary<string, string>();
            if (optionalParameters?.PlatformId != null && optionalParameters?.PlatformId != PlatformType.None)
            {
                queries.Add("platformId", optionalParameters.PlatformId.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(optionalParameters?.TargetNamespace))
            {
                queries.Add("targetNamespace", optionalParameters.TargetNamespace);
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/platforms")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueries(queries)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PagedPlatformLinks>();
                callback?.Try(result);
            });
        }
    }
}