// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide interfaces for Agreement service wrapper to connect to endpoints.
    /// </summary>
    internal class ServerAgreementApi : ServerApiBase
    {
        [Preserve]
        internal ServerAgreementApi(IHttpClient inHttpClient
            , ServerConfig inServerConfig
            , ISession inSession) 
            : base(inHttpClient, inServerConfig, inServerConfig.AgreementServerUrl, inSession)
        {
        }

        internal Dictionary<AgreementPolicyType, string> PolicyUuidMap;

        public void CreateBaseLegalPolicy(AgreementPolicyType policyType
            , string basePolicyName
            , string description
            , string[] affectedCountries
            , string[] affectedClientIds
            , string[] tags
            , bool isHidden
            , bool isHiddenPublic
            , ResultCallback<CreateBasePolicyResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                AuthToken
                , Namespace_
                , basePolicyName
                , description
                , affectedCountries
                , affectedClientIds
                , tags);

            error = policyType == AgreementPolicyType.EMPTY ? new Error(ErrorCode.InvalidRequest) : error;
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (PolicyUuidMap != null)
            {
                SendRequest();
                return;
            }

            GetPolicyTypeIds(result =>
            {
                PopulatePolicyTypeUuidMap(result);
                if (PolicyUuidMap == null)
                {
                    callback?.TryError(ErrorCode.InvalidRequest);
                    return;
                }

                SendRequest();
            });

            void SendRequest()
            {
                var requestBody = new CreateBasePolicyRequest()
                {
                    AffectedClientIds = affectedClientIds,
                    AffectedCountries = affectedCountries,
                    BasePolicyName = basePolicyName,
                    Description = description,
                    IsHidden = isHidden,
                    IsHiddenPublic = isHiddenPublic,
                    Namespace = Namespace_,
                    Tags = tags,
                    TypeId = PolicyUuidMap[policyType]
                };

                var request = HttpRequestBuilder
                    .CreatePost(BaseUrl + "/admin/namespaces/{namespace}/base-policies")
                    .WithBearerAuth(AuthToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", Namespace_)
                    .WithBody(requestBody.ToUtf8Json())
                    .GetResult();

                HttpOperator.SendRequest(request, response =>
                {
                    var result = response.TryParseJson<CreateBasePolicyResponse>();
                    callback?.Try(result);
                });
            }
        }

        public void GetAllBaseLegalPolicies(bool visibleOnly
            , ResultCallback<GetBasePolicyResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/admin/namespaces/{namespace}/base-policies")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("visibleOnly", visibleOnly.ToString())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetBasePolicyResponse[]>();
                callback?.Try(result);
            });
        }

        public void GetBaseLegalPolicy(string basePolicyId
            , ResultCallback<GetBasePolicyResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, basePolicyId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/admin/namespaces/{namespace}/base-policies/{basePolicyId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("basePolicyId", basePolicyId)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetBasePolicyResponse>();
                callback?.Try(result);
            });
        }

        public void UpdateBaseLegalPolicy(string basePolicyId
            , string basePolicyName
            , string description
            , string[] affectedCountries
            , string[] affectedClientIds
            , string[] tags
            , bool isHidden
            , bool isHiddenPublic
            , ResultCallback<UpdateBasePolicyResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                AuthToken
                , Namespace_
                , basePolicyId
                , basePolicyName
                , description
                , affectedCountries
                , affectedClientIds
                , tags);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new UpdateBasePolicyRequest()
            {
                AffectedClientIds = affectedClientIds,
                AffectedCountries = affectedCountries,
                BasePolicyName = basePolicyName,
                Description = description,
                IsHidden = isHidden,
                IsHiddenPublic = isHiddenPublic,
                Namespace = Namespace_,
                Tags = tags
            };

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/admin/namespaces/{namespace}/base-policies/{basePolicyId}")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("basePolicyId", basePolicyId)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UpdateBasePolicyResponse>();
                callback?.Try(result);
            });
        }

        private void GetPolicyTypeIds(ResultCallback<GetPolicyTypeIdResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/admin/namespaces/{namespace}/policy-types")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", 100.ToString())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetPolicyTypeIdResponse[]>();
                callback?.Try(result);
            });
        }

        private void PopulatePolicyTypeUuidMap(Result<GetPolicyTypeIdResponse[]> result)
        {
            if (result == null || result.IsError || result.Value == null)
            {
                return;
            }

            PolicyUuidMap = new Dictionary<AgreementPolicyType, string>();

            var legalDocumentType = result.Value.FirstOrDefault(type => type.PolicyTypeName == "Legal Document");
            PolicyUuidMap.Add(AgreementPolicyType.LEGAL_DOCUMENT_TYPE, legalDocumentType.Id);

            var marketingPreferenceType = result.Value.FirstOrDefault(type => type.PolicyTypeName == "Marketing Preference");
            PolicyUuidMap.Add(AgreementPolicyType.MARKETING_PREFERENCE_TYPE, marketingPreferenceType.Id);
        }
    }
}