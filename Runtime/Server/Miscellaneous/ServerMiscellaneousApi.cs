// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections.Generic;

namespace AccelByte.Server
{
    internal class ServerMiscellaneousApi : ServerApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ServerMiscellaneousApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session) 
            : base( httpClient, config, config.BasicServerUrl, session )
        {
        }
        
        [UnityEngine.Scripting.Preserve]
        internal ServerMiscellaneousApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session
            , HttpOperator httpOperator) 
            : base( httpClient, config, config.BasicServerUrl, session, httpOperator)
        {
        }

        public void GetCurrentTime( ResultCallback<Time> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetCurrentTime(
                httpOperator: HttpOperator
                , @namespace: Namespace_
                , baseUrl: BaseUrl
                , callback);
        }
        
        public void GetCountryGroups( ResultCallback<Country[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetCountryGroups(
                httpOperator: HttpOperator
                , baseUrl: BaseUrl
                , @namespace: Namespace_
                , adminEndpoint: true
                , authToken: Session.AuthorizationToken
                , callback);
        }

        public void GetLanguages( ResultCallback<Dictionary<string,string>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetLanguages(
                httpOperator: HttpOperator
                , baseUrl: BaseUrl
                , @namespace: Namespace_
                , adminEndpoint: true
                , authToken: Session.AuthorizationToken
                , callback);
        }

        public void GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetTimeZones(
                httpOperator: HttpOperator
                , baseUrl: BaseUrl
                , @namespace: Namespace_
                , adminEndpoint: true
                , authToken: Session.AuthorizationToken
                , callback);
        }

        public void ListCountryGroups(string groupCode, ResultCallback<CountryGroup[]> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            const string groupCodeQueryKey = "groupCode";

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/misc/countrygroups")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(Session.AuthorizationToken);
            
            if (!string.IsNullOrEmpty(groupCode))
            {
                requestBuilder.WithQueryParam(groupCodeQueryKey, groupCode);
            }

            IHttpRequest request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<CountryGroup[]>();
                callback.Try(result);
            });
        }
        
        public void AddCountryGroup(CountryGroup newCountryGroupData, ResultCallback<CountryGroup> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, newCountryGroupData);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/misc/countrygroups")
                .WithPathParam("namespace", Namespace_)
                .WithBody(newCountryGroupData.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(Session.AuthorizationToken);

            IHttpRequest request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<CountryGroup>();
                callback.Try(result);
            });
        }
        
        public void UpdateCountryGroup(string groupCode, CountryGroup newCountryGroupData, ResultCallback<CountryGroup> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, groupCode, newCountryGroupData);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/misc/countrygroups/{countryGroupCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("countryGroupCode", groupCode)
                .WithBody(newCountryGroupData.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(Session.AuthorizationToken);

            IHttpRequest request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<CountryGroup>();
                callback.Try(result);
            });
        }
        
        public void DeleteCountryGroup(string groupCode, ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, groupCode);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/misc/countrygroups/{countryGroupCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("countryGroupCode", groupCode)
                .WithBearerAuth(Session.AuthorizationToken);

            IHttpRequest request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }
    }
}