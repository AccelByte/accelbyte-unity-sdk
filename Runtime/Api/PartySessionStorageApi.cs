// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    internal class PartySessionStorageApi : ApiBase
    {
        /// <summary>
        /// Provide function to let Matchmaking service wrapper to connect to party session storage endpoints.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal PartySessionStorageApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.SessionServerUrl, session)
        {
        }

        internal void GetPartySessionStorage(string partyId, MatchmakingV2CreateTicketRequestOptionalParams optionalParams, ResultCallback<GetPartySessionStorageResult> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

            var error = Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , partyId);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/storage")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GetPartySessionStorageResult>();
                callback?.Try(result);
            });
        }

        internal void StorePersonalDataToReservedPartySessionStorage(string partyId
            , PartySessionStorageReservedData body
            , ResultCallback<PartySessionStorageReservedData> callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_
                , AuthToken
                , partyId);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/storage/users/{userId}/reserved")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(body.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PartySessionStorageReservedData>();
                callback?.Try(result);
            });
        }
    }
}