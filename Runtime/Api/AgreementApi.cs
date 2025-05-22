// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;
using UnityEngine.Networking;

namespace AccelByte.Api
{
    public class AgreementApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==AgreementServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal AgreementApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.AgreementServerUrl, session )
        {
        }

        public IEnumerator GetLegalPolicies( AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , ResultCallback<PublicPolicy[]> callback )
        {
            string functionName = "GetLegalPolicies";
            Report.GetFunctionLog(GetType().Name, functionName, logger: SharedMemory?.Logger);

            var optionalParameters = new GetLegalPoliciesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetLegalPolicies(agreementPolicyType, tags, defaultOnEmpty, optionalParameters, callback);

            yield return null;
        }

        internal void GetLegalPolicies(AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , GetLegalPoliciesOptionalParameters optionalParameters
            , ResultCallback<PublicPolicy[]> callback)
        {
            string functionName = "GetLegalPolicies";
            Report.GetFunctionLog(GetType().Name, functionName, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/policies/namespaces/{namespace}")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("policyType", (agreementPolicyType == AgreementPolicyType.EMPTY) ? "" : agreementPolicyType.ToString())
                .WithQueryParam("tags", string.Join(",", tags))
                .WithQueryParam("defaultOnEmpty", defaultOnEmpty.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PublicPolicy[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetLegalPoliciesByCountry(string countryCode
            , AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , ResultCallback<PublicPolicy[]> callback )
        {
            string functionName = "GetLegalPoliciesByCountry";
            Report.GetFunctionLog(GetType().Name, functionName, logger: SharedMemory?.Logger);

            var optionalParameters = new GetLegalPoliciesByCountryOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetLegalPoliciesByCountry(countryCode, agreementPolicyType, tags, defaultOnEmpty, optionalParameters, callback);

            yield return null;
        }

        internal void GetLegalPoliciesByCountry(string countryCode
            , AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , GetLegalPoliciesByCountryOptionalParameters optionalParameters
            , ResultCallback<PublicPolicy[]> callback)
        {
            string functionName = "GetLegalPoliciesByCountry";
            Report.GetFunctionLog(GetType().Name, functionName, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(countryCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/policies/countries/{countryCode}")
                .WithPathParam("countryCode", countryCode)
                .WithQueryParam("policyType", (agreementPolicyType == AgreementPolicyType.EMPTY) ? "" : agreementPolicyType.ToString())
                .WithQueryParam("tags", string.Join(",", tags))
                .WithQueryParam("defaultOnEmpty", defaultOnEmpty.ToString())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PublicPolicy[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator BulkAcceptPolicyVersions( AcceptAgreementRequest[] acceptAgreementRequests
            , ResultCallback<AcceptAgreementResponse> callback )
        {
            string functionName = "BulkAcceptPolicyVersions";
            Report.GetFunctionLog(GetType().Name, functionName, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkAcceptPolicyVersionsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkAcceptPolicyVersions(acceptAgreementRequests, optionalParameters, callback);

            yield return null;
        }

        internal void BulkAcceptPolicyVersions(AcceptAgreementRequest[] acceptAgreementRequests
            , BulkAcceptPolicyVersionsOptionalParameters optionalParameters
            , ResultCallback<AcceptAgreementResponse> callback)
        {
            string functionName = "BulkAcceptPolicyVersions";
            Report.GetFunctionLog(GetType().Name, functionName, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                acceptAgreementRequests
                , Namespace_
                , AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/agreements/policies")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(acceptAgreementRequests.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<AcceptAgreementResponse>();
                callback?.Try(result);
            });
        }

        public IEnumerator AcceptPolicyVersion( string localizedPolicyVersionId
            , ResultCallback callback )
        {
            string functionName = "AcceptPolicyVersion";
            Report.GetFunctionLog(GetType().Name, functionName, logger: SharedMemory?.Logger);

            var optionalParameters = new AcceptPolicyVersionOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            AcceptPolicyVersion(localizedPolicyVersionId, optionalParameters, callback);

            yield return null;
        }

        internal void AcceptPolicyVersion(string localizedPolicyVersionId
            , AcceptPolicyVersionOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            string functionName = "AcceptPolicyVersion";
            Report.GetFunctionLog(GetType().Name, functionName, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(localizedPolicyVersionId
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/agreements/localized-policy-versions/{localizedPolicyVersionId}")
                .WithPathParam("localizedPolicyVersionId", localizedPolicyVersionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public IEnumerator QueryLegalEligibilities( ResultCallback<RetrieveUserEligibilitiesResponse[]> callback )
        {
            string functionName = "CheckLegalEligibilities";
            Report.GetFunctionLog(GetType().Name, functionName, logger: SharedMemory?.Logger);

            var optionalParameters = new QueryLegalEligibilitiesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            QueryLegalEligibilities(optionalParameters, callback);

            yield return null;
        }

        internal void QueryLegalEligibilities(QueryLegalEligibilitiesOptionalParameters optionalParameters, ResultCallback<RetrieveUserEligibilitiesResponse[]> callback)
        {
            string functionName = "CheckLegalEligibilities";
            Report.GetFunctionLog(GetType().Name, functionName, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/eligibilities/namespaces/{namespace}")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<RetrieveUserEligibilitiesResponse[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetLegalDocument( string url
            , ResultCallback<string> callback )
        {
            string functionName = "GetLegalDocument";
            Report.GetFunctionLog(GetType().Name, functionName, logger: SharedMemory?.Logger);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                Result<string> result;
                if (webRequest.result != UnityWebRequest.Result.ConnectionError && webRequest.result != UnityWebRequest.Result.ProtocolError)
                {
                    result = Result<string>.CreateOk(webRequest.downloadHandler.text);
                }
                else
                {
                    result = Result<string>.CreateError((ErrorCode)webRequest.responseCode);
                }
                callback(result);
            }
        }

        internal void ChangePolicyPreferences(ChangeAgreementRequest[] requestBody, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ChangePolicyPreferencesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ChangePolicyPreferences(requestBody, optionalParameters, callback);
        }

        internal void ChangePolicyPreferences(ChangeAgreementRequest[] requestBody, ChangePolicyPreferencesOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(requestBody, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/public/agreements/localized-policy-versions/preferences")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }
    }
}
