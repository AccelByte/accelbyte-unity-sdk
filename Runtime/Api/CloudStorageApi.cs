// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Net;
using System.Security.Cryptography;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class CloudStorageApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==CloudStorageServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal CloudStorageApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.CloudStorageServerUrl, session )
        {
        }

        public IEnumerator GetAllSlots( string userId
            , ResultCallback<Slot[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get all slots! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all slots! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get all slots! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/slots")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Slot[]>();
            callback.Try(result);
        }

        public IEnumerator GetSlot( string userId
            , string slotId
            , ResultCallback<byte[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get the slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get the slot! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get the slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't get the slot! slotId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationOctetStream)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<byte[]> result;

            switch ((HttpStatusCode) response.Code)
            {
            case HttpStatusCode.OK:
                result = Result<byte[]>.CreateOk(response.BodyBytes);

                break;

            default:
                result = Result<byte[]>.CreateError((ErrorCode) response.Code);

                break;
            }

            callback.Try(result);
        }

        public IEnumerator CreateSlot( string userId
            , byte[] data
            , string filename
            , ResultCallback<Slot> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a slot! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create a slot! accessToken parameter is null!");
            Assert.IsNotNull(data, "Can't create a slot! data parameter is null!");
            Assert.IsNotNull(filename, "Can't create a slot! filename parameter is null!");

            string checkSum;

            using (MD5 md5 = MD5.Create())
            {
                byte[] computeHash = md5.ComputeHash(data);
                checkSum = BitConverter.ToString(computeHash).Replace("-", "");
            }

            FormDataContent formDataContent = new FormDataContent();
            formDataContent.Add(filename, data);

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/slots")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("checksum", checkSum)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithContentType(formDataContent.GetMediaType())
                .WithBody(formDataContent)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Slot>();
            callback.Try(result);
        }

        public IEnumerator UpdateSlot( string userId
            , string slotId
            , byte[] data
            , string filename
            , ResultCallback<Slot> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't update a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a slot! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update a slot! accessToken parameter is null!");
            Assert.IsNotNull(data, "Can't update a slot! data parameter is null!");
            Assert.IsNotNull(filename, "Can't update a slot! filename parameter is null!");
            Assert.IsNotNull(slotId, "Can't update a slot! slotId parameter is null!");

            string checkSum;

            using (MD5 md5 = MD5.Create())
            {
                byte[] computeHash = md5.ComputeHash(data);
                checkSum = BitConverter.ToString(computeHash).Replace("-", "");
            }

            FormDataContent formDataContent = new FormDataContent();
            formDataContent.Add(filename, data);

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithQueryParam("checksum", checkSum)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithContentType(formDataContent.GetMediaType())
                .WithBody(formDataContent)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Slot>();
            callback.Try(result);
        }

        public IEnumerator UpdateSlotMetadata( string userId
            , string slotId
            , string[] tags
            , string label
            , string customMetadata
            , ResultCallback<Slot> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't update a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a slot! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update a slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't update a slot! slotId parameter is null!");

            UpdateMedataRequest updateMedataRequest = new UpdateMedataRequest();
            {
                updateMedataRequest.tags = tags;
                updateMedataRequest.label = label;
                updateMedataRequest.customAttribute = customMetadata;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}/metadata")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithQueryParam("tags", tags)
                .WithQueryParam("label", label)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateMedataRequest.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Slot>();
            callback.Try(result);
        }

        public IEnumerator DeleteSlot( string userId
            , string slotId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a slot! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create a slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't create a slot! fileSection parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
    }
}
