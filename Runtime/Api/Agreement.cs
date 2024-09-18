// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Agreement : WrapperBase
    {
        private readonly AgreementApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Agreement( AgreementApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");
            
            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal Agreement( AgreementApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this(inApi, inSession, inCoroutineRunner) // Curry this obsolete data to the new overload ->
        {
        }
        
        #region GetLegalPolicies Overloads
        /// <summary>
        /// Retrieve all active latest policies based on a namespace.
        /// The country will be read from user token.
        /// - Main overload
        /// </summary>
        /// <param name="agreementPolicyType">
        /// Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY
        /// if you want to be responded with all policy type.
        /// </param>
        /// <param name="tags"></param>
        /// <param name="defaultOnEmpty">
        /// Specify with true if you want to be responded with default country-specific
        /// policy if your requested country is not exist.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        public void GetLegalPolicies( AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , ResultCallback<PublicPolicy[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetLegalPolicies(agreementPolicyType, tags, defaultOnEmpty, callback));
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="agreementPolicyType">
        /// Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY
        /// if you want to be responded with all policy type.
        /// </param>
        /// <param name="defaultOnEmpty">
        /// Specify with true if you want to be responded with default country-specific
        /// policy if your requested country is not exist.
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        public void GetLegalPolicies( AgreementPolicyType agreementPolicyType
            , bool defaultOnEmpty
            , ResultCallback<PublicPolicy[]> callback )
        {
            string[] tags = new string[1];
            GetLegalPolicies(agreementPolicyType, tags, defaultOnEmpty, callback);
        }
        #endregion /GetLegalPolicies Overloads

        #region GetLegalPoliciesByCountry Overloads
        /// <summary>
        /// Retrieve all active latest policies based on a namespace and country.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="defaultOnEmpty">
        /// Specify with true if you want to be responded with default
        /// country-specific policy if your requested country is not exist.
        /// </param>
        /// <param name="countryCode"></param>
        /// <param name="agreementPolicyType">
        /// Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY
        /// if you want to be responded with all policy type.
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        public void GetLegalPoliciesByCountry(
            string countryCode, 
            AgreementPolicyType agreementPolicyType,
            bool defaultOnEmpty, 
            ResultCallback<PublicPolicy[]> callback)
        {
            string[] tags = new string[1];
            GetLegalPoliciesByCountry(countryCode, agreementPolicyType, tags, defaultOnEmpty, callback);
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace and country.
        /// The country will be read from user token.
        /// </summary>
        /// <param name="defaultOnEmpty">
        /// Specify with true if you want to be responded with default
        /// country-specific policy if your requested country is not exist.
        /// </param>
        /// <param name="countryCode"></param>
        /// <param name="agreementPolicyType">
        /// Filter the responded policy by policy type. Choose the AgreementPolicyType.EMPTY
        /// if you want to be responded with all policy type.</param>
        /// <param name="tags">Filter the responded policy by tags.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        public void GetLegalPoliciesByCountry(
            string countryCode, 
            AgreementPolicyType agreementPolicyType, 
            string[] tags, 
            bool defaultOnEmpty, 
            ResultCallback<PublicPolicy[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(
                api.GetLegalPoliciesByCountry(
                    countryCode, agreementPolicyType, 
                    tags, 
                    defaultOnEmpty, 
                    callback));
        }
        #endregion /GetLegalPoliciesByCountry Overloads
        
        /// <summary>
        /// Sign multiple user's legal eligibility documents.
        /// </summary>
        /// <param name="acceptAgreementRequests">Signed agreements</param>
        /// <param name="callback">
        /// Returns a Result that contains an AcceptAgreementResponse via callback when completed
        /// </param>
        public void BulkAcceptPolicyVersions( AcceptAgreementRequest[] acceptAgreementRequests
            , ResultCallback<AcceptAgreementResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkAcceptPolicyVersions(acceptAgreementRequests, callback));
        }

        /// <summary>
        /// Sign a user's legal eligibility document.
        /// </summary>
        /// <param name="localizedPolicyVersionId">Localized Policy Version Id to accept</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void AcceptPolicyVersion( string localizedPolicyVersionId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.AcceptPolicyVersion(localizedPolicyVersionId, callback));
        }

        /// <summary>
        /// Query all player's legal eligibilities on a namespace,
        /// used to check is player already commited to legal or not.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains an array of RetrieveUserEligibilitiesResponse
        /// via callback when completed
        /// </param>
        public void QueryLegalEligibilities( ResultCallback<RetrieveUserEligibilitiesResponse[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryLegalEligibilities(callback));
        }

        /// <summary>
        /// Get the content of the legal document.
        /// </summary>
        /// <param name="url">The url of the legal document, you can get it from GetLegalPolicies query</param>
        /// <param name="callback">Returns a Result that contains a string via callback when completed</param>
        public void GetLegalDocument( string url
            , ResultCallback<string> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(api.GetLegalDocument(url, callback));
        }

        /// <summary>
        /// Accept/Revoke Marketing Preference Consent
        /// </summary>
        /// <param name="requestBody">Request body to be sent</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ChangePolicyPreferences(ChangeAgreementRequest[] requestBody, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ChangePolicyPreferences(requestBody, callback);
        }
    }
}