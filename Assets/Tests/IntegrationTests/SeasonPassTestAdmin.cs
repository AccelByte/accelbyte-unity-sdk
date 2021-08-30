// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AccelByte.Core;
using AccelByte.Models;

namespace Tests
{
    public class SeasonPassTestAdmin
    {
        private readonly AccelByteHttpClient httpClient;
        private readonly CoroutineRunner coroutineRunner;
        private readonly Config config;

        public SeasonPassTestAdmin()
        {
            this.config = new Config
            {
                BaseUrl = Environment.GetEnvironmentVariable("ADMIN_BASE_URL"),
                ClientId = Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET"),
                PublisherNamespace = Environment.GetEnvironmentVariable("PUBLISHER_NAMESPACE")
            };
            this.config.Expand();

            this.coroutineRunner = new CoroutineRunner();
            this.httpClient = new AccelByteHttpClient();
            this.httpClient.SetCredentials(this.config.ClientId, this.config.ClientSecret);
        }

        [DataContract]
        public class SeasonPassCreateSeasonRequest
        {
            [DataMember] public string name { get; set; }
            [DataMember] public string start { get; set; }
            [DataMember] public string end { get; set; }
            [DataMember] public string defaultLanguage { get; set; }
            [DataMember] public int defaultRequiredExp { get; set; }
            [DataMember] public string draftStoreId { get; set; }
            [DataMember] public string tierItemId { get; set; }
            [DataMember] public bool autoClaim { get; set; }
            [DataMember] public SeasonPassExcessStrategy excessStrategy { get; set; }
            [DataMember] public Dictionary<string, TestHelper.ItemCreateModel.Localization> localizations { get; set; }
            [DataMember] public Image[] images { get; set; }
        }

        [DataContract]
        public class SeasonPassCreateSeasonResponse : SeasonPassCreateSeasonRequest
        {
            [DataMember] public string id { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string tierItemName { get; set; }
            [DataMember] public string[] passCodes { get; set; }
            [DataMember] public SeasonPassStatus status { get; set; }
            [DataMember] public DateTime publishedAt { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class SeasonPassCreateGetSeasonResponse
        {
            [DataMember] public string id { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string name { get; set; }
            [DataMember] public DateTime start { get; set; }
            [DataMember] public DateTime end { get; set; }
            [DataMember] public string defaultLanguage { get; set; }
            [DataMember] public string[] passCodes { get; set; }
            [DataMember] public SeasonPassStatus status { get; set; }
            [DataMember] public DateTime publishedAt { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class SeasonPassCreateGetSeasonPagingRespon
        {
            [DataMember] public SeasonPassCreateGetSeasonResponse[] data { get; set; }
            [DataMember] public Paging paging { get; set; }
        }

        [DataContract]
        public class SeasonPassCreatePassRequest
        {
            [DataMember] public string code { get; set; }
            [DataMember] public int displayOrder { get; set; }
            [DataMember] public bool autoEnroll { get; set; }
            [DataMember] public string passItemId { get; set; }
            [DataMember] public Dictionary<string, TestHelper.ItemCreateModel.Localization> localizations { get; set; }
            [DataMember] public Image[] images { get; set; }
        }

        [DataContract]
        public class SeasonPassCreatePassResponse
        {
            [DataMember] public string seasonId { get; set; }
            [DataMember] public string code { get; set; }
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string displayOrder { get; set; }
            [DataMember] public bool autoEnroll { get; set; }
            [DataMember] public string passItemId { get; set; }
            [DataMember] public string passItemName { get; set; }
            [DataMember] public Dictionary<string, TestHelper.ItemCreateModel.Localization> localizations { get; set; }
            [DataMember] public Image[] images { get; set; }
            [DataMember] public DateTime createdAt { get; set; }
            [DataMember] public DateTime updatedAt { get; set; }
        }

        [DataContract]
        public class SeasonPassCreateRewardRequest
        {
            [DataMember] public string code { get; set; }
            [DataMember] public string type { get; set; }
            [DataMember] public string itemId { get; set; }
            [DataMember] public int quantity { get; set; }
            [DataMember] public Image[] images { get; set; }
        }

        [DataContract]
        public class SeasonPassCreateRewardResponse : SeasonPassCreateRewardRequest
        {
            [DataMember(Name = "namespace")] public string Namespace { get; set; }
            [DataMember] public string seasonId { get; set; }
            [DataMember] public string itemName { get; set; }
        }

        [DataContract]
        public class SeasonPassTierRequest
        {
            [DataMember] public int requiredExp { get; set; }
            [DataMember] public Dictionary<string, object> rewards { get; set; }
        }

        [DataContract]
        public class SeasonPassCreateTierRequest
        {
            [DataMember] public int index { get; set; }
            [DataMember] public int quantity { get; set; }
            [DataMember] public SeasonPassTierRequest tier { get; set; }
        }

        public void SeasonCreateSeason(string namespace_, string accesstoken, SeasonPassCreateSeasonRequest req,
            ResultCallback<SeasonPassCreateSeasonResponse> callback)
        {
            this.coroutineRunner.Run(SeasonCreateSeasonAsync(namespace_, accesstoken, req, callback));
        }

        public void SeasonQuerySeason(string namespace_, string accesstoken, SeasonPassStatus[] status,
            ResultCallback<SeasonPassCreateGetSeasonPagingRespon> callback, int offset = 0, int limit = 0)
        {
            this.coroutineRunner.Run(SeasonQuerySeasonAsync(namespace_, accesstoken, status, callback, offset, limit));
        }

        public void SeasonDeleteSeason(string namespace_, string accesstoken, string seasonId, ResultCallback callback)
        {
            this.coroutineRunner.Run(SeasonDeleteSeasonAsync(namespace_, accesstoken, seasonId, callback));
        }

        public void SeasonPublishSeason(string namespace_, string accesstoken, string seasonId, ResultCallback<SeasonPassCreateSeasonResponse> callback)
        {
            this.coroutineRunner.Run(SeasonPublishSeasonAsync(namespace_, accesstoken, seasonId, callback));
        }

        public void SeasonForceUnpublishSeason(string namespace_, string accesstoken, string seasonId, ResultCallback<SeasonPassCreateSeasonResponse> callback)
        {
            this.coroutineRunner.Run(SeasonForceUnpublishSeasonAsync(namespace_, accesstoken, seasonId, callback));
        }

        public void SeasonCreatePass(string namespace_, string accesstoken, string seasonId, SeasonPassCreatePassRequest req,
            ResultCallback<SeasonPassCreatePassResponse> callback)
        {
            this.coroutineRunner.Run(SeasonCreatePassAsync(namespace_, accesstoken, seasonId, req, callback));
        }

        public void SeasonDeletePass(string namespace_, string accesstoken, string seasonId, string passCode, ResultCallback callback)
        {
            this.coroutineRunner.Run(SeasonDeletePassAsync(namespace_, accesstoken, seasonId, passCode, callback));
        }

        public void SeasonCreateReward(string namespace_, string accesstoken, string seasonId, SeasonPassCreateRewardRequest req,
            ResultCallback<SeasonPassCreateRewardResponse> callback)
        {
            this.coroutineRunner.Run(SeasonCreateRewardAsync(namespace_, accesstoken, seasonId, req, callback));
        }

        public void SeasonDeleteReward(string namespace_, string accesstoken, string seasonId, string rewardCode, ResultCallback callback)
        {
            this.coroutineRunner.Run(SeasonDeleteRewardAsync(namespace_, accesstoken, seasonId, rewardCode, callback));
        }

        public void SeasonCreateTier(string namespace_, string accesstoken, string seasonId, SeasonPassCreateTierRequest req,
            ResultCallback<SeasonPassTierJsonObject[]> callback)
        {
            this.coroutineRunner.Run(SeasonCreateTierAsync(namespace_, accesstoken, seasonId, req, callback));
        }

        public void SeasonModifyTier(string namespace_, string accesstoken, string seasonId, string tierId, SeasonPassTierRequest req,
            ResultCallback<SeasonPassTierJsonObject[]> callback)
        {
            this.coroutineRunner.Run(SeasonModifyTierAsync(namespace_, accesstoken, seasonId, tierId, req, callback));
        }

        public void SeasonDeleteTier(string namespace_, string accesstoken, string seasonId, string tierId, ResultCallback callback)
        {
            this.coroutineRunner.Run(SeasonDeleteTierAsync(namespace_, accesstoken, seasonId, tierId, callback));
        }

        public IEnumerator SeasonCreateSeasonAsync(string namespace_, string accessToken, SeasonPassCreateSeasonRequest req,
            ResultCallback<SeasonPassCreateSeasonResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons")
                .WithPathParam("namespace", namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(req.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassCreateSeasonResponse>();
            callback.Try(result);
        }

        public IEnumerator SeasonQuerySeasonAsync(string namespace_, string accessToken, SeasonPassStatus[] status,
            ResultCallback<SeasonPassCreateGetSeasonPagingRespon> callback, int offset, int limit)
        {
            string parameters = "";
            foreach (var stat in status)
            {
                if (stat == SeasonPassStatus.DRAFT)
                {
                    parameters += "status=DRAFT";
                }
                if (stat == SeasonPassStatus.PUBLISHED)
                {
                    parameters += string.IsNullOrEmpty(parameters) ? "status=PUBLISHED" : "&status=PUBLISHED";
                }
                if (stat == SeasonPassStatus.RETIRED)
                {
                    parameters += string.IsNullOrEmpty(parameters) ? "status=RETIRED" : "&status=RETIRED";
                }
            }
            parameters += string.IsNullOrEmpty(parameters) ? "" : "&";
            parameters += string.Format("offset={0}", offset);
            parameters += string.IsNullOrEmpty(parameters) ? "" : "&";
            parameters += string.Format("limit={0}", limit);

            IHttpRequest request = HttpRequestBuilder
                .CreateGet(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("parameters", parameters)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassCreateGetSeasonPagingRespon>();
            callback.Try(result);
        }

        public IEnumerator SeasonDeleteSeasonAsync(string namespace_, string accessToken, string seasonId, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SeasonPublishSeasonAsync(string namespace_, string accessToken, string seasonId, ResultCallback<SeasonPassCreateSeasonResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePut(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/publish")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassCreateSeasonResponse>();
            callback.Try(result);
        }

        public IEnumerator SeasonForceUnpublishSeasonAsync(string namespace_, string accessToken, string seasonId,
            ResultCallback<SeasonPassCreateSeasonResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePut(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/unpublish?force=true")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassCreateSeasonResponse>();
            callback.Try(result);
        }

        public IEnumerator SeasonCreatePassAsync(string namespace_, string accessToken, string seasonId, SeasonPassCreatePassRequest req,
            ResultCallback<SeasonPassCreatePassResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/passes")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(req.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassCreatePassResponse>();
            callback.Try(result);
        }

        public IEnumerator SeasonDeletePassAsync(string namespace_, string accessToken, string seasonId, string passCode,
            ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/passes/{passCode}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithPathParam("passCode", passCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SeasonCreateRewardAsync(string namespace_, string accessToken, string seasonId, SeasonPassCreateRewardRequest req,
            ResultCallback<SeasonPassCreateRewardResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/rewards")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(req.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassCreateRewardResponse>();
            callback.Try(result);
        }

        public IEnumerator SeasonDeleteRewardAsync(string namespace_, string accessToken, string seasonId, string rewardCode, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/rewards/{rewardCode}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithPathParam("rewardCode", rewardCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SeasonCreateTierAsync(string namespace_, string accessToken, string seasonId, SeasonPassCreateTierRequest req,
            ResultCallback<SeasonPassTierJsonObject[]> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/tiers")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(req.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassTierJsonObject[]>();
            callback.Try(result);
        }

        public IEnumerator SeasonModifyTierAsync(string namespace_, string accessToken, string seasonId, string tierId, SeasonPassTierRequest req,
            ResultCallback<SeasonPassTierJsonObject[]> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePut(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/tiers/{tierId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithPathParam("tierId", tierId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(req.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<SeasonPassTierJsonObject[]>();
            callback.Try(result);
        }

        public IEnumerator SeasonDeleteTierAsync(string namespace_, string accessToken, string seasonId, string tierId,
            ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.SeasonPassServerUrl + "/admin/namespaces/{namespace}/seasons/{seasonId}/tiers/{tierId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("seasonId", seasonId)
                .WithPathParam("tierId", tierId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
    }
}