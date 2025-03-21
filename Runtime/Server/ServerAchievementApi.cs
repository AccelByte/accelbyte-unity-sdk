// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

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

        public IEnumerator UnlockAchievement( string userId
            , string accessToken
            , string achievementCode
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(Namespace_, "Can't unlock achievement! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't unlock achievement! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't unlock achievement! UserId parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't unlock achievement! AchievementCode parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/achievements/{achievementCode}/unlock")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public void BulkUnlockAchievement(string userId
            , string[] achievementCodes
            , ResultCallback<BulkUnlockAchievementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

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

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/achievements/bulkUnlock")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<BulkUnlockAchievementResponse[]>();
                callback?.Try(result);
            });
        }
    }
}
