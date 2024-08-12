// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections.Generic;

namespace AccelByte.Core
{
    internal static class AccelByteMiscellaneousApi
    {
        public static void GetCurrentTime(HttpOperator httpOperator, string @namespace, string baseUrl, ResultCallback<Time> callback )
        {
            var requestBuilder = HttpRequestBuilder
                .CreateGet(baseUrl + "/v1/public/misc/time")
                .WithNamespace(@namespace)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson);

            IHttpRequest request = requestBuilder.GetResult();
            
            httpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<Time>();
                callback?.Try(result);
            });
        }
        
        public static void GetCountryGroups(HttpOperator httpOperator, string baseUrl, string @namespace, bool adminEndpoint, string authToken, ResultCallback<Country[]> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(@namespace);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }
            
            string version = $"/v1";
            string credential = "/" + (adminEndpoint ? "admin" : "public");

            var requestBuilder = HttpRequestBuilder
                .CreateGet(baseUrl + version + credential + "/namespaces/{namespace}/misc/countries")
                .WithPathParam("namespace", @namespace);

            if (authToken != null)
            {
                requestBuilder.WithBearerAuth(authToken);
            }

            IHttpRequest request = requestBuilder.GetResult();

            httpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<Country[]>();
                callback?.Try(result);
            });
        }

        public static void GetLanguages(HttpOperator httpOperator, string baseUrl, string @namespace, bool adminEndpoint, string authToken, ResultCallback<Dictionary<string,string>> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(@namespace);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }
            
            string version = $"/v1";
            string credential = "/" + (adminEndpoint ? "admin" : "public");

            var requestBuilder = HttpRequestBuilder
                .CreateGet(baseUrl + version + credential + "/namespaces/{namespace}/misc/languages")
                .WithPathParam("namespace", @namespace);
            
            if (authToken != null)
            {
                requestBuilder.WithBearerAuth(authToken);
            }

            IHttpRequest request = requestBuilder.GetResult();

            httpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<Dictionary<string,string>>();
                callback?.Try(result);
            });
        }

        public static void GetTimeZones(HttpOperator httpOperator, string baseUrl, string @namespace, bool adminEndpoint, string authToken, ResultCallback<string[]> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(@namespace);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            string version = $"/v1";
            string credential = "/" + (adminEndpoint ? "admin" : "public");

            var requestBuilder = HttpRequestBuilder
                .CreateGet(baseUrl + version + credential + "/namespaces/{namespace}/misc/timezones")
                .WithPathParam("namespace", @namespace);
            
            if (authToken != null)
            {
                requestBuilder.WithBearerAuth(authToken);
            }

            IHttpRequest request = requestBuilder.GetResult();

            httpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<string[]>();
                callback?.Try(result);
            });
        }
    }
}