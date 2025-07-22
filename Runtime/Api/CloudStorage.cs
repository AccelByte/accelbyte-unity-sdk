// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    [Obsolete("This interface is deprecated. Please use Api.BinaryCloudSave instead")]
    public class CloudStorage : WrapperBase
    {
        private readonly CloudStorageApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal CloudStorage( CloudStorageApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal CloudStorage( CloudStorageApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Get all available slots.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Slot info array via callback when completed.
        /// </param>
        [Obsolete("Cloud Storage is deprecated - please use Binary Cloudsave for the replacement")]
        public void GetAllSlots( ResultCallback<Slot[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetAllSlots(session.UserId, callback));
        }

        /// <summary>
        /// Get slot by slotId. Download data identified by slot id. 
        /// </summary>
        /// <param name="slotId">Slot Id, generated when a new slot is created</param>
        /// <param name="callback">Returns a Result that contains data via callback when completed.</param>
        [Obsolete("Cloud Storage is deprecated - please use Binary Cloudsave for the replacement")]
        public void GetSlot( string slotId
            , ResultCallback<byte[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetSlot(session.UserId, slotId, callback));
        }

        /// <summary>
        /// Create a new slot and directly fill it with data.
        /// </summary>
        /// <param name="data">Data to fill the slot</param>
        /// <param name="dataName">Data name</param>
        /// <param name="callback">Returns a Result that contains Slot info via callback when completed.</param>
        [Obsolete("Cloud Storage is deprecated - please use Binary Cloudsave for the replacement")]
        public void CreateSlot( byte[] data
            , string dataName
            , ResultCallback<Slot> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateSlot(session.UserId, data, dataName, callback));
        }

        /// <summary>
        /// Update data for slot identified with slot id.
        /// </summary>
        /// <param name="slotId">Slot Id for slot to update data with</param>
        /// <param name="data">Data to replace data already in slot</param>
        /// <param name="dataName">Data name</param>
        /// <param name="callback">Returns a Result that contains Slot info via callback when completed.</param>
        [Obsolete("Cloud Storage is deprecated - please use Binary Cloudsave for the replacement")]
        public void UpdateSlot( string slotId
            , byte[] data
            , string dataName
            , ResultCallback<Slot> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateSlot(
                    session.UserId,
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
        [Obsolete("Cloud Storage is deprecated - please use Binary Cloudsave for the replacement")]
        public void UpdateSlotMetadata( string slotId
            , string[] tags
            , string label
            , string customMetadata
            , ResultCallback<Slot> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.Try(Result<Slot>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            coroutineRunner.Run(
                api.UpdateSlotMetadata(
                    session.UserId,
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
        [Obsolete("Cloud Storage is deprecated - please use Binary Cloudsave for the replacement")]
        public void DeleteSlot( string slotId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteSlot(session.UserId, slotId, callback));
        }
    }
}