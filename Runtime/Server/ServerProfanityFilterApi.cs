// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Linq;
using UnityEngine.Scripting;

namespace AccelByte.Server
{
    public class ServerProfanityFilterApi : ServerApiBase
    {
        [Preserve]
        public ServerProfanityFilterApi(IHttpClient inHttpClient
            , ServerConfig inServerConfig
            , ISession inSession) 
            : base(inHttpClient, inServerConfig, inServerConfig.ProfanityFilterServerUrl, inSession)
        {
        }

        public void BulkCreateProfanityWords(CreateProfanityWordRequest[] bulkCreateRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, ServerConfig.PublisherNamespace, bulkCreateRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var missingParamRequest = bulkCreateRequest.FirstOrDefault(request =>
            {
                if (string.IsNullOrEmpty(request.Word))
                {
                    return true;
                }

                return false;
            });
            if (missingParamRequest != null)
            {
                callback?.TryError(ErrorCode.InvalidRequest, errorMessage: "word fields cannot be null or empty");
                return;
            }

            var requestBody = new BulkCreateProfanityWordRequest()
            {
                Dictionaries = bulkCreateRequest
            };
            var request =
                HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/default-profanity/namespaces/{namespace}/dictionary/bulk")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithPathParam("namespace", ServerConfig.PublisherNamespace)
                    .WithBody(requestBody.ToUtf8Json())
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void CreateProfanityWord(string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, ServerConfig.PublisherNamespace, word);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new CreateProfanityWordRequest()
            {
                Word = word,
                FalseNegatives = falseNegatives,
                FalsePositives = falsePositives
            };
            var request =
                HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/default-profanity/namespaces/{namespace}/dictionary")
                    .WithBearerAuth(AuthToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", ServerConfig.PublisherNamespace)
                    .WithBody(requestBody.ToUtf8Json())
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ProfanityDictionaryEntry>();
                callback?.Try(result);
            });
        }

        public void DeleteProfanityWord(string id, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, ServerConfig.PublisherNamespace, id);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request =
                HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/default-profanity/namespaces/{namespace}/dictionary/{id}")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", ServerConfig.PublisherNamespace)
                    .WithPathParam("id", id)
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void GetProfanityWordGroups(GetProfanityWordGroupsOptionalParameters optionalParameters
            , ResultCallback<ProfanityWordGroupResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, ServerConfig.PublisherNamespace);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder =
                HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/default-profanity/namespaces/{namespace}/dictionary/groups")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", ServerConfig.PublisherNamespace);

            if (optionalParameters != null)
            {
                if (optionalParameters.SortBy != ProfanityGroupSortBy.None)
                {
                    requestBuilder.WithQueryParam("sortBy", ConverterUtils.EnumToEnumMemberValue(optionalParameters.SortBy));
                }
                if (optionalParameters.Page != null)
                {
                    requestBuilder.WithQueryParam("page", optionalParameters.Page.ToString());
                }
                if (optionalParameters.PageSize != null)
                {
                    requestBuilder.WithQueryParam("pageSize", optionalParameters.PageSize.ToString());
                }
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ProfanityWordGroupResponse>();
                callback?.Try(result);
            });
        }

        public void QueryProfanityWords(QueryProfanityWordsOptionalParameters optionalParams
            , ResultCallback<QueryProfanityWordsResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, ServerConfig.PublisherNamespace);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder =
                HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/default-profanity/namespaces/{namespace}/dictionary")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", ServerConfig.PublisherNamespace);

            if (optionalParams != null)
            {
                if (!string.IsNullOrEmpty(optionalParams.StartsWith))
                {
                    requestBuilder.WithQueryParam("startsWith", optionalParams.StartsWith);
                }
                if (optionalParams.IncludeChildren != null)
                {
                    requestBuilder.WithQueryParam("includeChildren", optionalParams.IncludeChildren.ToString());
                }
                if (!string.IsNullOrEmpty(optionalParams.FilterMask))
                {
                    requestBuilder.WithQueryParam("filterMask", optionalParams.FilterMask);
                }
                if (optionalParams.SortBy != ProfanityEntrySortBy.None)
                {
                    requestBuilder.WithQueryParam("sortBy", ConverterUtils.EnumToEnumMemberValue(optionalParams.SortBy));
                }
                if (optionalParams.Page != null)
                {
                    requestBuilder.WithQueryParam("page", optionalParams.Page.ToString());
                }
                if (optionalParams.PageSize != null)
                {
                    requestBuilder.WithQueryParam("pageSize", optionalParams.PageSize.ToString());
                }
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<QueryProfanityWordsResponse>();
                callback?.Try(result);
            });
        }

        public void UpdateProfanityWord(string id
            , string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, ServerConfig.PublisherNamespace, word);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new CreateProfanityWordRequest()
            {
                Word = word,
                FalseNegatives = falseNegatives,
                FalsePositives = falsePositives
            };

            var request =
                HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/default-profanity/namespaces/{namespace}/dictionary/{id}")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithContentType(MediaType.ApplicationJson)
                    .WithPathParam("namespace", ServerConfig.PublisherNamespace)
                    .WithPathParam("id", id)
                    .WithBody(requestBody.ToUtf8Json())
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ProfanityDictionaryEntry>();
                callback?.Try(result);
            });
        }
    }
}