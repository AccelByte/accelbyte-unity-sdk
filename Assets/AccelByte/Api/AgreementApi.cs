// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace AccelByte.Api
{
    internal class AgreementApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal AgreementApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator GetLegalPolicies(string namespace_, AgreementPolicyType agreementPolicyType, string[] tags, bool defaultOnEmpty, string accessToken, 
            ResultCallback<PublicPolicy[]> callback)
        {
            string functionName = "GetLegalPolicies";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(namespace_, "Can't " + functionName + "! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(baseUrl + "/public/policies/namespaces/{namespace}")
                .WithPathParam("namespace", namespace_)
                .WithQueryParam("policyType", (agreementPolicyType == AgreementPolicyType.EMPTY) ? "" : agreementPolicyType.ToString())
                .WithQueryParam("tags", string.Join(",",tags))
                .WithQueryParam("defaultOnEmpty", defaultOnEmpty.ToString())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PublicPolicy[]>();
            callback.Try(result);
        }

        public IEnumerator GetLegalPoliciesByCountry(string countryCode, AgreementPolicyType agreementPolicyType, string[] tags, bool defaultOnEmpty, string accessToken, 
            ResultCallback<PublicPolicy[]> callback)
        {
            string functionName = "GetLegalPoliciesByCountry";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(countryCode, "Can't " + functionName + "! CountryCode parameter is null!");
            Assert.IsNotNull(accessToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(baseUrl + "/public/policies/countries/{countryCode}")
                .WithPathParam("countryCode", countryCode)
                .WithQueryParam("policyType", (agreementPolicyType == AgreementPolicyType.EMPTY) ? "" : agreementPolicyType.ToString())
                .WithQueryParam("tags", string.Join(",", tags))
                .WithQueryParam("defaultOnEmpty", defaultOnEmpty.ToString())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PublicPolicy[]>();
            callback.Try(result);
        }

        public IEnumerator BulkAcceptPolicyVersions(string accessToken, AcceptAgreementRequest[] acceptAgreementRequests, 
            ResultCallback<AcceptAgreementResponse> callback)
        {
            string functionName = "BulkAcceptPolicyVersions";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(accessToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(baseUrl + "/public/agreements/policies")
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(acceptAgreementRequests.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<AcceptAgreementResponse>();
            callback.Try(result);
        }

        public IEnumerator AcceptPolicyVersion(string accessToken, string localizedPolicyVersionId, 
            ResultCallback callback)
        {
            string functionName = "AcceptPolicyVersion";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(accessToken, "Can't " + functionName + "! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(baseUrl + "/public/agreements/localized-policy-versions/{localizedPolicyVersionId}")
                .WithPathParam("localizedPolicyVersionId", localizedPolicyVersionId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator QueryLegalEligibilities(string namespace_, string accessToken, ResultCallback<RetrieveUserEligibilitiesResponse[]> callback)
        {
            string functionName = "CheckLegalEligibilities";
            Report.GetFunctionLog(GetType().Name, functionName);
            Assert.IsNotNull(namespace_, "Can't " + functionName + "! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't " + functionName + "! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(baseUrl + "/public/eligibilities/namespaces/{namespace}")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<RetrieveUserEligibilitiesResponse[]>();
            callback.Try(result);
        }

        public IEnumerator GetLegalDocument(string url, ResultCallback<string> callback)
        {
            string functionName = "GetLegalDocument";
            Report.GetFunctionLog(GetType().Name, functionName);
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();

            Result<string> result;
            if (!webRequest.isNetworkError && !webRequest.isHttpError)
            {
                result = Result<string>.CreateOk(webRequest.downloadHandler.text);
            }
            else
            {
                result = Result<string>.CreateError((ErrorCode) webRequest.responseCode);
            }
            callback(result);
        }
    }
}