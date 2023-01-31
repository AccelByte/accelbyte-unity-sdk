// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;

namespace AccelByte.Api
{
    internal class TelemetryApi : ApiBase
    {        
        private readonly uint agentType;
        private readonly string deviceId;

        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param> // TODO: Should this be raw BaseUrl?
        /// <param name="session"></param>
        public TelemetryApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.BaseUrl, session )
        {
            Report.GetFunctionLog(GetType().Name);
            
            agentType = getAgentTypeByPlatform();
            deviceId = DeviceProvider.GetFromSystemInfo(Config.PublisherNamespace).DeviceId;
        }
        
        private static uint getAgentTypeByPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    return 70;

                case RuntimePlatform.OSXPlayer:
                    return 80;

                case RuntimePlatform.LinuxPlayer:
                    return 90;

                case RuntimePlatform.Android:
                    return 110;

                case RuntimePlatform.IPhonePlayer:
                    return 120;

                case RuntimePlatform.XboxOne:
                    return 130;

                case RuntimePlatform.PS4:
                    return 140;

                case RuntimePlatform.Switch:
                    return 170;

                case RuntimePlatform.tvOS:
                    return 200;

                case RuntimePlatform.WSAPlayerX86:
                    return 210;

                case RuntimePlatform.WSAPlayerX64:
                    return 211;

                case RuntimePlatform.WSAPlayerARM:
                    return 212;

                case RuntimePlatform.WebGLPlayer:
                    return 220;
                
                default: 
                    return 0;
            }
        }

        public IEnumerator SendEvent<T>( string clientId
            , string userID
            , TelemetryEventTag eventTag
            , T eventData
            , ResultCallback callback ) 
            where T : class
        {
            Report.GetFunctionLog(GetType().Name);
            string nowTime = DateTime.UtcNow.ToString("O");
            string strEventData;

            if (eventData is string)
            {
                strEventData = "\"" + eventData + "\"";
            }
            else
            {
                strEventData = eventData.ToJsonString();
            }

            string jsonString = string.Format(
                "{{" +
                "\"AgentType\": {0}," +
                "\"AppID\": {1}," +
                "\"ClientID\": \"{2}\"," +
                "\"Data\": {3}," +
                "\"DeviceID\": \"{4}\"," +
                "\"EventID\": {5}," +
                "\"EventLevel\": {6}," +
                "\"EventTime\": \"{7}\"," +
                "\"EventType\": {8}," +
                "\"UUID\": \"{9:N}\"," +
                "\"UX\": {10}," +
                "\"UserID\": \"{11}\"" +
                "}}",
                agentType,
                eventTag.AppId,
                clientId,
                strEventData,
                deviceId,
                eventTag.Id,
                eventTag.Level,
                nowTime,
                eventTag.Type,
                Guid.NewGuid(),
                eventTag.UX,
                userID);

            var request = HttpRequestBuilder
                .CreatePost(
                    BaseUrl +
                    "/public/namespaces/{namespace}/events/gameclient/{appID}/{eventType}/{eventLevel}/{eventID}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("appID", eventTag.AppId.ToString())
                .WithPathParam("eventType", eventTag.Type.ToString())
                .WithPathParam("eventLevel", eventTag.Level.ToString())
                .WithPathParam("eventID", eventTag.Id.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(jsonString)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
		
    }
}
