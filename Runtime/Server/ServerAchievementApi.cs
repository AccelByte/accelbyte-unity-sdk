// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Server
{
    public class ServerAchievementApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==AchievementServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerAchievementApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.AchievementServerUrl,session )
        {
        }

        public IEnumerator UnlockAchievement(string userId
            , string accessToken
            , string achievementCode
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UnlockAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UnlockAchievement(userId, accessToken, achievementCode, optionalParameters, callback);

            yield return null;
        }

        internal void UnlockAchievement(string userId
            , string accessToken
            , string achievementCode
            , UnlockAchievementOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, accessToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/achievements/{achievementCode}/unlock";
            var request = HttpRequestBuilder
                .CreatePut(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParse();
                    callback?.Try(result);
                });
        }

        public void BulkUnlockAchievement(string userId
            , string[] achievementCodes
            , ResultCallback<BulkUnlockAchievementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkUnlockAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkUnlockAchievement(userId, achievementCodes, optionalParameters, callback);
        }

        internal void BulkUnlockAchievement(string userId
            , string[] achievementCodes
            , BulkUnlockAchievementOptionalParameters optionalParameters
            , ResultCallback<BulkUnlockAchievementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCodes);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new BulkUnlockAchievementRequest()
            {
                AchievementCodes = achievementCodes
            };

            string requestUrlFormat = BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/achievements/bulkUnlock";
            var request = HttpRequestBuilder
                .CreatePut(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<BulkUnlockAchievementResponse[]>();
                    callback?.Try(result);
                });
        }
    }
}
