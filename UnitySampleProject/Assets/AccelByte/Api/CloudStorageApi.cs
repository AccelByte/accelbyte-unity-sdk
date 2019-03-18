// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class CloudStorageApi
    {
        private readonly string baseUrl;

        internal CloudStorageApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can not construct CloudStorage service; baseUrl is null!");
            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetAllSlots(string @namespace, string userId, string accessToken,
            ResultCallback<Slot[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get all slots! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all slots! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get all slots! accessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/binary-store/public/namespaces/{namespace}/users/{userId}/slots")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Slot[]> result = response.TryParseJsonBody<Slot[]>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetSlot(string @namespace, string userId, string accessToken, string slotId,
            ResultCallback<byte[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get the slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get the slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get the slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't get the slot! slotId parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/binary-store/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.OctedStream)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<byte[]>.CreateError(ErrorCode.NetworkError, "There is no response"));

                yield break;
            }

            //TODO: Might need to transfer data via stream for big files
            byte[] responseBytes = response.GetBodyRaw();
            response.Close();
            Result<byte[]> result;

            switch (response.StatusCode)
            {
            case HttpStatusCode.OK:
                result = Result<byte[]>.CreateOk(responseBytes);

                break;
            default:
                result = Result<byte[]>.CreateError((ErrorCode) response.StatusCode);

                break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> CreateSlot(string @namespace, string userId, string accessToken, byte[] data,
            string filename, ResultCallback<Slot> callback)
        {
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
            formDataContent.Add(data, filename);

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/binary-store/public/namespaces/{namespace}/users/{userId}/slots")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("checksum", checkSum)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(formDataContent.GetMediaType())
                .WithBody(formDataContent)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Slot> result = response.TryParseJsonBody<Slot>();
            callback.Try(result);
        }

        public IEnumerator<ITask> UpdateSlot(string @namespace, string userId, string accessToken, string slotId,
            byte[] data, string filename, ResultCallback<Slot> callback)
        {
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
            formDataContent.Add(data, filename);

            HttpWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/binary-store/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithQueryParam("checksum", checkSum)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(formDataContent.GetMediaType())
                .WithBody(formDataContent)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Slot> result = response.TryParseJsonBody<Slot>();
            callback.Try(result);
        }

        public IEnumerator<ITask> UpdateSlotMetadata(string @namespace, string userId, string accessToken,
            string slotId, string[] tags, string label, string customMetadata, ResultCallback<Slot> callback)
        {
            Assert.IsNotNull(@namespace, "Can't update a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't update a slot! slotId parameter is null!");

            FormDataContent customAttribute = new FormDataContent();
            customAttribute.Add("customAttribute", customMetadata);

            HttpWebRequest request = HttpRequestBuilder
                .CreatePut(
                    this.baseUrl + "/binary-store/public/namespaces/{namespace}/users/{userId}/slots/{slotId}/metadata")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .WithQueryParam("tags", tags)
                .WithQueryParam("label", label)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(customAttribute.GetMediaType())
                .WithBody(customAttribute)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<Slot> result = response.TryParseJsonBody<Slot>();
            callback.Try(result);
        }

        public IEnumerator<ITask> DeleteSlot(string @namespace, string userId, string accessToken, string slotId,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't create a slot! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a slot! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create a slot! accessToken parameter is null!");
            Assert.IsNotNull(slotId, "Can't create a slot! fileSection parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateDelete(
                    this.baseUrl + "/binary-store/public/namespaces/{namespace}/users/{userId}/slots/{slotId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("slotId", slotId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }
    }
}