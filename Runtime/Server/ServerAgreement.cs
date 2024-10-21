// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace AccelByte.Server
{
    /// <summary>
    /// Provides APIs to access Agreement service.
    /// </summary>
    public class ServerAgreement : WrapperBase, IServerAgreement
    {
        private readonly ServerAgreementApi api;
        private readonly ISession session;

        [Preserve]
        internal ServerAgreement(ServerAgreementApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            UnityEngine.Assertions.Assert.IsNotNull(inApi, "Cannot construct Agreement maanger, api is null!");

            api = inApi;
            session = inSession;
        }

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

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateBaseLegalPolicy(policyType, basePolicyName, description, affectedCountries
                , affectedClientIds, tags, isHidden, isHiddenPublic, callback);
        }

        public void GetAllBaseLegalPolicies(bool visibleOnly
            , ResultCallback<GetBasePolicyResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetAllBaseLegalPolicies(visibleOnly, callback);
        }

        public void GetBaseLegalPolicy(string basePolicyId
            , ResultCallback<GetBasePolicyResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetBaseLegalPolicy(basePolicyId, callback);
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

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateBaseLegalPolicy(basePolicyId, basePolicyName, description, affectedCountries
                , affectedClientIds, tags, isHidden, isHiddenPublic, callback);
        }

        internal Dictionary<AgreementPolicyType, string> GetPolicyTypeIds()
        {
            return api.PolicyUuidMap;
        }

        internal void OverridePolicyTypeIds(Dictionary<AgreementPolicyType, string> policyTypes)
        {
            api.PolicyUuidMap = policyTypes;
        }
    }
}