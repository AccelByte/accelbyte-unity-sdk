// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class CloudStorage
    {
        private readonly CloudStorageApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal CloudStorage(CloudStorageApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get all available slots.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Slot info array via callback when completed.</param>
        public void GetAllSlots(ResultCallback<Slot[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetAllSlots(this.@namespace, this.session.UserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get slot by slotId. Download data identified by slot id. 
        /// </summary>
        /// <param name="slotId">Slot Id, generated when a new slot is created</param>
        /// <param name="callback">Returns a Result that contains data via callback when completed.</param>
        public void GetSlot(string slotId, ResultCallback<byte[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetSlot(this.@namespace, this.session.UserId, this.session.AuthorizationToken, slotId, callback));
        }

        /// <summary>
        /// Create a new slot and directly fill it with data.
        /// </summary>
        /// <param name="data">Data to fill the slot</param>
        /// <param name="dataName">Data name</param>
        /// <param name="callback">Returns a Result that contains Slot info via callback when completed.</param>
        public void CreateSlot(byte[] data, string dataName, ResultCallback<Slot> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateSlot(this.@namespace, this.session.UserId, this.session.AuthorizationToken, data, dataName, callback));
        }

        /// <summary>
        /// Update data for slot identified with slot id.
        /// </summary>
        /// <param name="slotId">Slot Id for slot to update data with</param>
        /// <param name="data">Data to replace data already in slot</param>
        /// <param name="dataName">Data name</param>
        /// <param name="callback">Returns a Result that contains Slot info via callback when completed.</param>
        public void UpdateSlot(string slotId, byte[] data, string dataName, ResultCallback<Slot> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateSlot(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    slotId,
                    data,
                    dataName,
                    callback));
        }

        /// <summary>
        /// update metadata of a slot identified by it slot id.
        /// </summary>
        /// <param name="slotId">slot id of a slot</param>
        /// <param name="tags">tags to be updated/added</param>
        /// <param name="label">label to be updated/added</param>
        /// <param name="customMetadata">custom attribute tu be updated/added</param>
        /// <param name="callback">return a result that contain updated slot metadata after completed</param>
        public void UpdateSlotMetadata(string slotId, string[] tags, string label, string customMetadata,
            ResultCallback<Slot> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.Try(Result<Slot>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateSlotMetadata(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    slotId,
                    tags,
                    label,
                    customMetadata,
                    callback));
        }

        /// <summary>
        /// Delete slot identified by Slot Id.
        /// </summary>
        /// <param name="slotId">Slot Id to be deleted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteSlot(string slotId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteSlot(this.@namespace, this.session.UserId, this.session.AuthorizationToken, slotId, callback));
        }
    }
}