// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Agreement
    {
        private readonly AgreementApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Agreement(AgreementApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");
            
            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="namespace">Filter the responded policy by namespace</param>
        /// <param name="agreementPolicyType">Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY if you want to be responded with all policy type.</param>
        /// <param name="defaultOnEmpty">Specify with true if you want to be responded with default country-specific policy if your requested country is not exist.</param>
        /// <param name="callback">Returns a Result that contains an array of public policy via callback when completed</param>
        public void GetLegalPolicies(string namespace_, AgreementPolicyType agreementPolicyType, string[] tags, bool defaultOnEmpty, ResultCallback<PublicPolicy[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (session == null || session.AuthorizationToken == null)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetLegalPolicies(namespace_, agreementPolicyType, tags, defaultOnEmpty, session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="namespace">Filter the responded policy by namespace</param>
        /// <param name="agreementPolicyType">Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY if you want to be responded with all policy type.</param>
        /// <param name="defaultOnEmpty">Specify with true if you want to be responded with default country-specific policy if your requested country is not exist.</param>
        /// <param name="callback">Returns a Result that contains an array of public policy via callback when completed</param>
        public void GetLegalPolicies(string namespace_, AgreementPolicyType agreementPolicyType, bool defaultOnEmpty, ResultCallback<PublicPolicy[]> callback)
        {
            string[] tags = new string[1];
            this.GetLegalPolicies(namespace_, agreementPolicyType, tags, defaultOnEmpty, callback);
        }

        /// <summary>
        /// Retrieve all active latest policies on game namespace.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="agreementPolicyType">Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY if you want to be responded with all policy type.</param>
        /// <param name="defaultOnEmpty">Specify with true if you want to be responded with default country-specific policy if your requested country is not exist.</param>
        /// <param name="callback">Returns a Result that contains an array of public policy via callback when completed</param>
        public void GetLegalPolicies(AgreementPolicyType agreementPolicyType, bool defaultOnEmpty, ResultCallback<PublicPolicy[]> callback)
        {
            string[] tags = new string[1];
            this.GetLegalPolicies(agreementPolicyType, tags, defaultOnEmpty, callback);
        }

        /// <summary>
        /// Retrieve all active latest policies based on game namespace.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="agreementPolicyType">Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY if you want to be responded with all policy type.</param>
        /// <param name="tags">Filter the responded policy by tags.</param>
        /// <param name="defaultOnEmpty">Specify with true if you want to be responded with default country-specific policy if your requested country is not exist.</param>
        /// <param name="callback">Returns a Result that contains an array of public policy via callback when completed</param>
        public void GetLegalPolicies(AgreementPolicyType agreementPolicyType, string[] tags, bool defaultOnEmpty, ResultCallback<PublicPolicy[]> callback)
        {
            this.GetLegalPolicies(@namespace, agreementPolicyType, tags, defaultOnEmpty, callback);
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace and country.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="defaultOnEmpty">Specify with true if you want to be responded with default country-specific policy if your requested country is not exist.</param>
        /// <param name="agreementPolicyType">Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY if you want to be responded with all policy type.</param>
        /// <param name="callback">Returns a Result that contains an array of public policy via callback when completed</param>
        public void GetLegalPoliciesByCountry(string countryCode, AgreementPolicyType agreementPolicyType, bool defaultOnEmpty, ResultCallback<PublicPolicy[]> callback)
        {
            string[] tags = new string[1];

            this.GetLegalPoliciesByCountry(countryCode, agreementPolicyType, tags, defaultOnEmpty, callback);
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace and country.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="defaultOnEmpty">Specify with true if you want to be responded with default country-specific policy if your requested country is not exist.</param>
        /// <param name="agreementPolicyType">Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY if you want to be responded with all policy type.</param>
        /// <param name="tags">Filter the responded policy by tags.</param>
        /// <param name="callback">Returns a Result that contains an array of public policy via callback when completed</param>
        public void GetLegalPoliciesByCountry(string countryCode, AgreementPolicyType agreementPolicyType, string[] tags, bool defaultOnEmpty, ResultCallback<PublicPolicy[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            // if (session == null || session.AuthorizationToken == null)
            // {
            //     callback.TryError(ErrorCode.IsNotLoggedIn);
            //     return;
            // }

            coroutineRunner.Run(
                api.GetLegalPoliciesByCountry(countryCode, agreementPolicyType, tags, defaultOnEmpty, session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Sign multiple user's legal eligibility documents.
        /// </summary>
        /// <param name="acceptAgreementRequests">Signed agreements</param>
        /// <param name="callback">Returns a Result that contains an AcceptAgreementResponse via callback when completed</param>
        public void BulkAcceptPolicyVersions(AcceptAgreementRequest[] acceptAgreementRequests, ResultCallback<AcceptAgreementResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (session == null || session.AuthorizationToken == null)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkAcceptPolicyVersions(session.AuthorizationToken, acceptAgreementRequests, callback));
        }

        /// <summary>
        /// Sign a user's legal eligibility document.
        /// </summary>
        /// <param name="localizedPolicyVersionId">Localized Policy Version Id to accept</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void AcceptPolicyVersion(string localizedPolicyVersionId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (session == null || session.AuthorizationToken == null)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptPolicyVersion(session.AuthorizationToken, localizedPolicyVersionId, callback));
        }

        /// <summary>
        /// Query all player's legal eligibilities on a namespace, use to check is player already commited to legal or not.
        /// </summary>
        /// <param name="namespace_">Filter the eligibilities by namespace</param>
        /// <param name="callback">Returns a Result that contains an array of RetrieveUserEligibilitiesResponse via callback when completed</param>
        public void QueryLegalEligibilities(string namespace_, ResultCallback<RetrieveUserEligibilitiesResponse[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (session == null || session.AuthorizationToken == null)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryLegalEligibilities(namespace_, session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get the content of the legal document.
        /// </summary>
        /// <param name="url">The url of the legal document, you can get it from GetLegalPolicies query</param>
        /// <param name="callback">Returns a Result that contains a string via callback when completed</param>
        public void GetLegalDocument(string url, ResultCallback<string> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            coroutineRunner.Run(api.GetLegalDocument(url, callback));
        }
    }
}