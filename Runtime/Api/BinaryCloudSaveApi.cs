// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public class BinaryCloudSaveApi : ApiBase
    {
        /// <summary>
        /// Api class for Binary CloudSave service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==CloudSaveServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal BinaryCloudSaveApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.CloudSaveServerUrl, session)
        {
        }

        #region Internal Methods
        internal static readonly string[] validBinaryFileTypes = { "jpeg", "jpg", "png", "bmp", "gif", "mp3", "webp", "bin" };

        internal static bool ValidateBinaryFileType(string fileType)
        {
            foreach (var validType in validBinaryFileTypes)
            {
                if (fileType.ToLower() == validType)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        public void SaveUserBinaryRecord(string key
            , string fileType
            , bool isPublic
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }
            if (string.IsNullOrEmpty(fileType))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(fileType) + " cannot be null!"));
                return;
            }
            if (!ValidateBinaryFileType(fileType))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(fileType) + " contains unsupported file type!"));
                return;
            }

            var requestBody = new SaveUserBinaryRecordRequest()
            {
                Key = key,
                FileType = fileType,
                IsPublic = isPublic
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/{user}/binaries")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("user", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request
                , response =>
                {
                    var result = response.TryParseJson<UserBinaryRecord>();
                    callback.Try(result);
                });
        }

        public void GetCurrentUserBinaryRecord(string key
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request
                , response =>
                {
                    var result = response.TryParseJson<UserBinaryRecord>();
                    callback.Try(result);
                });
        }

        public void GetPublicUserBinaryRecord(string key
            , string userId
            , ResultCallback<UserBinaryRecord> callback
            , bool isPublic = false)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(userId) + " cannot be null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/{key}/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<UserBinaryRecord>();
                    callback.Try(result);
                });
        }

        public void BulkGetCurrentUserBinaryRecords(string[] keys
            , ResultCallback<ListUserBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (keys == null || keys.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(keys) + " cannot be null!"));
                return;
            }

            var requestBody = new BulkGetRecordsByKeyRequest()
            {
                keys = keys
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/me/binaries/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<ListUserBinaryRecords>();
                    callback.Try(result);
                });
        }

        public void BulkGetPublicUserBinaryRecords(string[] keys
            , string userId
            , ResultCallback<ListUserBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (keys == null || keys.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(keys) + " cannot be null!"));
                return;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(userId) + " cannot be null!"));
                return;
            }

            var requestBody = new BulkGetRecordsByKeyRequest()
            {
                keys = keys
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/public/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<ListUserBinaryRecords>();
                    callback.Try(result);
                });
        }

        public void BulkGetPublicUserBinaryRecords(string key
            , string[] userIds
            , ResultCallback<ListUserBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }
            if (userIds == null || userIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(userIds) + " cannot be null!"));
                return;
            }

            var requestBody = new ListBulkUserInfoRequest()
            {
                userIds = userIds
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/bulk/binaries/{key}/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<ListUserBinaryRecords>();
                    callback.Try(result);
                });
        }

        public void BulkQueryCurrentUserBinaryRecords(string query
            , ResultCallback<PaginatedUserBinaryRecords> callback
            , int offset
            , int limit)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(query))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(query) + " cannot be null!"));
                return;
            }

            var requestBody = new BulkQueryBinaryRecordsRequest()
            {
                Query = query,
                Offset = offset,
                Limit = limit
            };

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/users/me/binaries")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<PaginatedUserBinaryRecords>();
                    callback.Try(result);
                });
        }

        public void BulkQueryPublicUserBinaryRecords(string userId
            , ResultCallback<PaginatedUserBinaryRecords> callback
            , int offset
            , int limit)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(userId) + " cannot be null!"));
                return;
            }

            var requestBody = new BulkQueryPublicUserBinaryRecordsRequest()
            {
                Offset = offset,
                Limit = limit
            };

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<PaginatedUserBinaryRecords>();
                    callback.Try(result);
                });
        }

        public void UpdateUserBinaryRecordFile(string key
            , string contentType
            , string fileLocation
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }
            if (string.IsNullOrEmpty(contentType))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(contentType) + " cannot be null!"));
                return;
            }
            if (string.IsNullOrEmpty(fileLocation))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(fileLocation) + " cannot be null!"));
                return;
            }

            var requestBody = new UpdateUserBinaryRecordFileRequest()
            {
                ContentType = contentType,
                FileLocation = fileLocation
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<UserBinaryRecord>();
                    callback.Try(result);
                });
        }

        public void UpdateUserBinaryRecordMetadata(string key
            , bool isPublic
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }

            var requestBody = new UpdateUserBinaryRecordMetadataRequest()
            {
                IsPublic = isPublic
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/{key}/metadata")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<UserBinaryRecord>();
                    callback.Try(result);
                });
        }

        public void DeleteUserBinaryRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParse();
                    callback.Try(result);
                });
        }

        public void RequestUserBinaryRecordPresignedUrl(string key
            , string fileType
            , ResultCallback<BinaryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }
            if (string.IsNullOrEmpty(fileType))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(fileType) + " cannot be null!"));
                return;
            }
            if (!ValidateBinaryFileType(fileType))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(fileType) + " contains unsupported file type!"));
                return;
            }

            var requestBody = new RequestUserBinaryRecordPresignedUrlRequest()
            {
                FileType = fileType
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/binaries/{key}/presigned")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<BinaryInfo>();
                    callback.Try(result);
                });
        }

        public void GetGameBinaryRecord(string key
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(key) + " cannot be null!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/binaries/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<GameBinaryRecord>();
                    callback.Try(result);
                });
        }

        public void BulkGetGameBinaryRecords(string[] keys
            , ResultCallback<ListGameBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (keys == null || keys.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(keys) + " cannot be null!"));
                return;
            }

            var requestBody = new BulkGetRecordsByKeyRequest()
            {
                keys = keys
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/binaries/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<ListGameBinaryRecords>();
                    callback.Try(result);
                });
        }

        public void BulkQueryGameBinaryRecords(string query
            , ResultCallback<PaginatedGameBinaryRecords> callback
            , int offset
            , int limit)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(query))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to execute " + GetType().Name + ". " + nameof(query) + " cannot be null!"));
                return;
            }

            var requestBody = new BulkQueryBinaryRecordsRequest()
            {
                Query = query,
                Offset = offset,
                Limit = limit
            };

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/binaries")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(request,
                response =>
                {
                    var result = response.TryParseJson<PaginatedGameBinaryRecords>();
                    callback.Try(result);
                });
        }
    }
}