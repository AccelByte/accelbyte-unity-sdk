// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.Server.Interface
{
    [Obsolete("This interface will be removed on AGS 3.84, please manage your legal document on Admin Portal")]
    public interface IServerAgreement
    {
        /// <summary>
        /// Retrieve all base legal policies in the current namespace.
        /// </summary>
        /// <param name="visibleOnly">Set to only retrieve base legal policies that are not hidden.</param>
        /// <param name="callback">Result callback of retrieved base legal policies.</param>
        public void GetAllBaseLegalPolicies(bool visibleOnly
            , ResultCallback<GetBasePolicyResponse[]> callback);

        /// <summary>
        /// Create a base legal policy in the current namespace.
        /// </summary>
        /// <param name="policyType">Type of legal policy.</param>
        /// <param name="basePolicyName">Name of legal policy.</param>
        /// <param name="description">Description of legal policy.</param>
        /// <param name="affectedCountries">Countries affected by legal policy.</param>
        /// <param name="affectedClientIds">ClientIds affected by legal policy.</param>
        /// <param name="tags">Tags of legal policy.</param>
        /// <param name="isHidden">Set if policy should be hidden from requests.</param>
        /// <param name="isHiddenPublic">Set if policy should be hidden from public requests.</param>
        /// <param name="callback">Result callback of created base legal policy.</param>
        public void CreateBaseLegalPolicy(AgreementPolicyType policyType
            , string basePolicyName
            , string description
            , string[] affectedCountries
            , string[] affectedClientIds
            , string[] tags
            , bool isHidden
            , bool isHiddenPublic
            , ResultCallback<CreateBasePolicyResponse> callback);

        /// <summary>
        /// Get a base legal policy by id in the current namespace.
        /// </summary>
        /// <param name="basePolicyId">Id of base policy to get.</param>
        /// <param name="callback">Result callback of retrieved base legal policy.</param>
        public void GetBaseLegalPolicy(string basePolicyId
            , ResultCallback<GetBasePolicyResponse> callback);

        /// <summary>
        /// Update a base legal policy in the current namespace.
        /// </summary>
        /// <param name="basePolicyId">Id of legal policy to update.</param>
        /// <param name="basePolicyName">Name of legal policy.</param>
        /// <param name="description">Description of legal policy.</param>
        /// <param name="affectedCountries">Countries affected by legal policy.</param>
        /// <param name="affectedClientIds">ClientIds affected by legal policy.</param>
        /// <param name="tags">Tags of legal policy.</param>
        /// <param name="isHidden">Set if legal policy should be hidden from requests.</param>
        /// <param name="isHiddenPublic">Set if legal policy should be hidden from public requests.</param>
        /// <param name="callback">Result callback of updated base legal policy.</param>
        public void UpdateBaseLegalPolicy(string basePolicyId
            , string basePolicyName
            , string description
            , string[] affectedCountries
            , string[] affectedClientIds
            , string[] tags
            , bool isHidden
            , bool isHiddenPublic
            , ResultCallback<UpdateBasePolicyResponse> callback);
    }
}