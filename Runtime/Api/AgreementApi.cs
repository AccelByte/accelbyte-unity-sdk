// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
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
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(Namespace_, "Can't " + functionName + "! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/policies/namespaces/{namespace}")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("policyType", (agreementPolicyType == AgreementPolicyType.EMPTY) ? "" : agreementPolicyType.ToString())
                .WithQueryParam("tags", string.Join(",",tags))
                .WithQueryParam("defaultOnEmpty", defaultOnEmpty.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PublicPolicy[]>();
            callback.Try(result);
        }

        public IEnumerator GetLegalPoliciesByCountry(string countryCode
            , AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , ResultCallback<PublicPolicy[]> callback )
        {
            string functionName = "GetLegalPoliciesByCountry";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(countryCode, "Can't " + functionName + "! CountryCode parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/policies/countries/{countryCode}")
                .WithPathParam("countryCode", countryCode)
                .WithQueryParam("policyType", (agreementPolicyType == AgreementPolicyType.EMPTY) ? "" : agreementPolicyType.ToString())
                .WithQueryParam("tags", string.Join(",", tags))
                .WithQueryParam("defaultOnEmpty", defaultOnEmpty.ToString())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PublicPolicy[]>();
            callback.Try(result);
        }

        public IEnumerator BulkAcceptPolicyVersions( AcceptAgreementRequest[] acceptAgreementRequests
            , ResultCallback<AcceptAgreementResponse> callback )
        {
            string functionName = "BulkAcceptPolicyVersions";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(AuthToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/agreements/policies")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(acceptAgreementRequests.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<AcceptAgreementResponse>();
            callback.Try(result);
        }

        public IEnumerator AcceptPolicyVersion( string localizedPolicyVersionId
            , ResultCallback callback )
        {
            string functionName = "AcceptPolicyVersion";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(AuthToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/agreements/localized-policy-versions/{localizedPolicyVersionId}")
                .WithPathParam("localizedPolicyVersionId", localizedPolicyVersionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator QueryLegalEligibilities( ResultCallback<RetrieveUserEligibilitiesResponse[]> callback )
        {
            string functionName = "CheckLegalEligibilities";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(Namespace_, "Can't " + functionName + "! namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't " + functionName + "! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/eligibilities/namespaces/{namespace}")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<RetrieveUserEligibilitiesResponse[]>();
            callback.Try(result);
        }

        public IEnumerator GetLegalDocument( string url
            , ResultCallback<string> callback )
        {
            string functionName = "GetLegalDocument";
            Report.GetFunctionLog(GetType().Name, functionName);
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

    }
}
