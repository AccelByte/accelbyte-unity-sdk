// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
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
    public class CloudStorageApi
    {
        #region Fields 

        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        #endregion

        #region Constructor

        internal CloudStorageApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        #endregion

        #region Public Methods

        public IEnumerator GetAllSlots(string @namespace, string userId, string accessToken,
            ResultCallback<Slot[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get all slots! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all slots! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get all slots! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/slots")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Slot[]>();
            callback.Try(result);
        }

        public IEnumerator GetSlot(string @namespace, string userId, string accessToken, string slotId,
            ResultCallback<byte[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get the slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get the slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get the slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't get the slot! slotId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationOctetStream)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

        public IEnumerator CreateSlot(string @namespace, string userId, string accessToken, byte[] data,
            string filename, ResultCallback<Slot> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create a slot! accessToken parameter is null!");
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
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/slots")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("checksum", checkSum)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(formDataContent.GetMediaType())
                .WithBody(formDataContent)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Slot>();
            callback.Try(result);
        }

        public IEnumerator UpdateSlot(string @namespace, string userId, string accessToken, string slotId, byte[] data,
            string filename, ResultCallback<Slot> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't update a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");
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
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithQueryParam("checksum", checkSum)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(formDataContent.GetMediaType())
                .WithBody(formDataContent)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Slot>();
            callback.Try(result);
        }

        public IEnumerator UpdateSlotMetadata(string @namespace, string userId, string accessToken, string slotId,
            string[] tags, string label, string customMetadata, ResultCallback<Slot> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't update a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't update a slot! slotId parameter is null!");

            UpdateMedataRequest updateMedataRequest = new UpdateMedataRequest();
            {
                updateMedataRequest.tags = tags;
                updateMedataRequest.label = label;
                updateMedataRequest.customAttribute = customMetadata;
            }

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}/metadata")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithQueryParam("tags", tags)
                .WithQueryParam("label", label)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateMedataRequest.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Slot>();
            callback.Try(result);
        }

        public IEnumerator DeleteSlot(string @namespace, string userId, string accessToken, string slotId,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create a slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't create a slot! fileSection parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        #endregion
    }
}