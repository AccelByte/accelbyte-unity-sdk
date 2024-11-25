// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Server
{
    internal class ServerBinaryCloudSaveApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==CloudSaveServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerBinaryCloudSaveApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.CloudSaveServerUrl, session)
        {
        }

        public void CreateGameBinaryRecord(string key
            , FileType fileType
            , RecordSetBy setBy
            , TTLConfig ttlConfig
            , ResultCallback<BinaryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var payload = new CreateAdminGameBinaryRecordRequest()
            {
                FileType = fileType,
                Key = key,
                SetBy = setBy
            };

            if (ttlConfig != null)
            {
                payload.TtlConfig = ttlConfig;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithBearerAuth(AuthToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson)
                    .WithBody(payload.ToUtf8Json())
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<BinaryInfo>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void DeleteGameBinaryRecordTTLConfig(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries/{key}/ttl")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void DeleteGameBinaryRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries/{key}")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithPathParam("key", key)
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson);

            IHttpRequest request = builder.GetResult();


            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }

        public void GetGameBinaryRecord(string key
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries/{key}")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithPathParam("key", key)
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameBinaryRecord>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void QueryGameBinaryRecords(string query
            , ICollection<string> tags
            , int offset
            , int limit
            , ResultCallback<PaginatedGameBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithQueryParam("offset", offset.ToString())
                    .WithQueryParam("limit", limit.ToString());

            if (query != null)
            {
                builder.WithQueryParam("query", query);
            }

            if (tags != null && tags.Count > 0)
            {
                builder.WithQueryParam("tags", string.Join(",", tags));
            }

            IHttpRequest request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PaginatedGameBinaryRecords>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void RequestGameBinaryRecordPresignedUrl(string key
            , FileType fileType
            , ResultCallback<BinaryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var body = new RequestUserBinaryRecordPresignedUrlRequest()
            {
                FileType = fileType.ToString()
            };

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries/{key}/presigned")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithPathParam("key", key)
                    .WithBearerAuth(AuthToken)
                    .WithBody(body.ToUtf8Json())
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson);

            IHttpRequest request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<BinaryInfo>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void UpdateGameBinaryRecord(string key
            , FileType contentType
            , string fileLocation
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key
                , fileLocation
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var body = new UpdateGameBinaryRecordFileRequest
            {
                ContentType = contentType,
                FileLocation = fileLocation
            };

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries/{key}")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithPathParam("key", key)
                    .WithBearerAuth(AuthToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(body.ToUtf8Json())
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameBinaryRecord>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void UpdateGameBinaryRecordMetadata(string key
            , RecordSetBy setBy
            , ICollection<string> tags
            , TTLConfig ttlConfig
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/binaries/{key}/metadata")
                    .WithPathParam("namespace", ServerConfig.Namespace)
                    .WithPathParam("key", key)
                    .WithBearerAuth(AuthToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson);

            var payload = new UpdateGameBinaryRecordMetadataRequest()
            {
                SetBy = setBy
            };

            if (tags != null && tags.Count > 0)
            {
                payload.Tags = (string[])tags;
            }

            if (ttlConfig != null)
            {
                payload.TtlConfig = ttlConfig;
            }

            builder.WithBody(payload.ToUtf8Json());

            IHttpRequest request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameBinaryRecord>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }
    }
}
