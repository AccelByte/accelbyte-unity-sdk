// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
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
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal CloudStorage(CloudStorageApi api, User user, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct CloudStorage manager; api is null!");
            Assert.IsNotNull(user, "Can not construct CloudStorage manager; userAccount is null!");
            Assert.IsNotNull(taskDispatcher, "Can not construct CloudStorage manager; taskDispatcher is null!");
            Assert.IsNotNull(coroutineRunner, "Can not construct CloudStorage manager; coroutineRunner is null!");

            this.api = api;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get all available slots.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Slot info array via callback when completed.</param>
        public void GetAllSlots(ResultCallback<Slot[]> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<Slot[]>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetAllSlots(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<Slot[]>) result)),
                    this.user));
        }

        /// <summary>
        /// Get slot by slotId. Download data identified by slot id. 
        /// </summary>
        /// <param name="slotId">Slot Id, generated when a new slot is created</param>
        /// <param name="callback">Returns a Result that contains data via callback when completed.</param>
        public void GetSlot(string slotId, ResultCallback<byte[]> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<byte[]>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetSlot(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        slotId,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<byte[]>) result)),
                    this.user));
        }

        /// <summary>
        /// Create a new slot and directly fill it with data.
        /// </summary>
        /// <param name="data">Data to fill the slot</param>
        /// <param name="dataName">Data name</param>
        /// <param name="callback">Returns a Result that contains Slot info via callback when completed.</param>
        public void CreateSlot(byte[] data, string dataName, ResultCallback<Slot> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<Slot>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.CreateSlot(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        data,
                        dataName,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<Slot>) result)),
                    this.user));
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
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<Slot>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.UpdateSlot(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        slotId,
                        data,
                        dataName,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<Slot>) result)),
                    this.user));
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
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<Slot>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.UpdateSlotMetadata(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        slotId,
                        tags,
                        label,
                        customMetadata,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<Slot>) result)),
                    this.user));
        }

        /// <summary>
        /// Delete slot identified by Slot Id.
        /// </summary>
        /// <param name="slotId">Slot Id to be deleted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteSlot(string slotId, ResultCallback callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.DeleteSlot(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        slotId,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this.user));
        }
    }
}