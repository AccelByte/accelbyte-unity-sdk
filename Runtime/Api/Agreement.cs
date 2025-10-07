// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetLegalPoliciesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetLegalPolicies(agreementPolicyType, tags, defaultOnEmpty, optionalParameters, callback);
        }

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
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        internal void GetLegalPolicies(AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , GetLegalPoliciesOptionalParameters optionalParameters
            , ResultCallback<PublicPolicy[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetLegalPolicies(agreementPolicyType, tags, defaultOnEmpty, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetLegalPoliciesByCountryOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetLegalPoliciesByCountry(
                countryCode
                , agreementPolicyType
                , tags
                , defaultOnEmpty
                , optionalParameters
                , callback);
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
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        internal void GetLegalPoliciesByCountry(
            string countryCode
            , AgreementPolicyType agreementPolicyType
            , string[] tags
            , bool defaultOnEmpty
            , GetLegalPoliciesByCountryOptionalParameters optionalParameters
            , ResultCallback<PublicPolicy[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            api.GetLegalPoliciesByCountry(
                countryCode
                , agreementPolicyType
                , tags
                , defaultOnEmpty
                , optionalParameters
                , callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkAcceptPolicyVersionsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkAcceptPolicyVersions(acceptAgreementRequests, optionalParameters, callback);
        }

        /// <summary>
        /// Sign multiple user's legal eligibility documents.
        /// </summary>
        /// <param name="acceptAgreementRequests">Signed agreements</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">
        /// Returns a Result that contains an AcceptAgreementResponse via callback when completed
        /// </param>
        internal void BulkAcceptPolicyVersions(AcceptAgreementRequest[] acceptAgreementRequests
            , BulkAcceptPolicyVersionsOptionalParameters optionalParameters
            , ResultCallback<AcceptAgreementResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkAcceptPolicyVersions(acceptAgreementRequests, optionalParameters, callback);
        }

        /// <summary>
        /// Sign a user's legal eligibility document.
        /// </summary>
        /// <param name="localizedPolicyVersionId">Localized Policy Version Id to accept</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void AcceptPolicyVersion( string localizedPolicyVersionId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new AcceptPolicyVersionOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            AcceptPolicyVersion(localizedPolicyVersionId, optionalParameters, callback);
        }

        /// <summary>
        /// Sign a user's legal eligibility document.
        /// </summary>
        /// <param name="localizedPolicyVersionId">Localized Policy Version Id to accept</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        internal void AcceptPolicyVersion(string localizedPolicyVersionId
            , AcceptPolicyVersionOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.AcceptPolicyVersion(localizedPolicyVersionId, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new QueryLegalEligibilitiesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            QueryLegalEligibilities(optionalParameters, callback);
        }

        /// <summary>
        /// Query all player's legal eligibilities on a namespace,
        /// used to check is player already commited to legal or not.
        /// </summary>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of RetrieveUserEligibilitiesResponse
        /// via callback when completed
        /// </param>
        internal void QueryLegalEligibilities(QueryLegalEligibilitiesOptionalParameters optionalParameters, ResultCallback<RetrieveUserEligibilitiesResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryLegalEligibilities(optionalParameters, callback);
        }

        /// <summary>
        /// Get the content of the legal document.
        /// </summary>
        /// <param name="url">The url of the legal document, you can get it from GetLegalPolicies query</param>
        /// <param name="callback">Returns a Result that contains a string via callback when completed</param>
        public void GetLegalDocument( string url
            , ResultCallback<string> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            coroutineRunner.Run(api.GetLegalDocument(url, callback));
        }

        /// <summary>
        /// Accept/Revoke Marketing Preference Consent
        /// </summary>
        /// <param name="requestBody">Request body to be sent</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ChangePolicyPreferences(ChangeAgreementRequest[] requestBody, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ChangePolicyPreferencesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ChangePolicyPreferences(requestBody, optionalParameters, callback);
        }

        /// <summary>
        /// Accept/Revoke Marketing Preference Consent
        /// </summary>
        /// <param name="requestBody">Request body to be sent</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        internal void ChangePolicyPreferences(ChangeAgreementRequest[] requestBody, ChangePolicyPreferencesOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ChangePolicyPreferences(requestBody, optionalParameters, callback);
        }
        
        /// <summary>
        /// Retrieve all active latest policies based on a namespace and country.
        /// - Main overload
        /// </summary>
        /// <param name="countryCode">Country code.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        public void GetLegalPoliciesByNamespaceAndCountry(string countryCode
            , ResultCallback<PublicPolicy[]> callback)
        {
            GetLegalPoliciesByNamespaceAndCountry(countryCode, null, callback);
        }

        /// <summary>
        /// Retrieve all active latest policies based on a namespace and country.
        /// - Main overload
        /// </summary>
        /// <param name="countryCode">Country code.</param>
        /// <param name="optionalParameters">Contains optional parameters.</param>
        /// <param name="callback">
        /// Returns a Result that contains an array of public policy via callback when completed
        /// </param>
        public void GetLegalPoliciesByNamespaceAndCountry(string countryCode
            , GetPoliciesByNamespaceAndCountryOptionalParameters optionalParameters
            , ResultCallback<PublicPolicy[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            api.GetLegalPoliciesByNamespaceAndCountry(countryCode, optionalParameters, callback);
        }
    }
}