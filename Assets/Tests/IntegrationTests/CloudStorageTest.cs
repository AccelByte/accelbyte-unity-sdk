// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Linq;
using System.Text;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class CloudStorageTest
    {
        private string[] payloads = new string[2] {"payloadNumberOne", "payloadNumberTwo"};
        private string[] originalNames = new string[2] {"file1.txt", "file2.txt"};
        private Slot createdSlot = null;
        private Slot updatedSlotMeta = null;

        [UnityTest, TestLog, Order(0)]
        public IEnumerator Setup()
        {
            var user = AccelBytePlugin.GetUser();

            Result loginWithDevice = null;
            user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);

            Debug.Log("Error:" + loginWithDevice.Error?.Code + " | " + loginWithDevice.Error?.Message);

            Result<UserData> getDataResult = null;
            
            user.GetData(r => getDataResult = r);
            yield return TestHelper.WaitForValue(() => getDataResult);
            
            TestHelper.LogResult(getDataResult);
            TestHelper.Assert.IsResultOk(getDataResult);
            TestHelper.Assert.That(!loginWithDevice.IsError || loginWithDevice.Error?.Code == ErrorCode.InvalidRequest);
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator CreateSlot_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();

            Result<Slot> createSlotResult = null;
            cloudStorage.CreateSlot(
                Encoding.ASCII.GetBytes(this.payloads[0]),
                this.originalNames[0],
                result => { createSlotResult = result; });
            yield return TestHelper.WaitForValue(() => createSlotResult);

            TestHelper.LogResult(createSlotResult, "Create slot");
            this.createdSlot = createSlotResult.Value;
            TestHelper.Assert.IsResultOk(createSlotResult);
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetCreatedSlot_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result<Slot[]> getAllSlotsResults = null;
            bool bGetCreatedSlot = false;

            cloudStorage.GetAllSlots(result => { getAllSlotsResults = result; });
            yield return TestHelper.WaitForValue(() => getAllSlotsResults);

            TestHelper.LogResult(getAllSlotsResults, "Get all slots 1, after created");
            TestHelper.Assert.IsResultOk(getAllSlotsResults);

            //this.createdSlot = null;
            foreach (Slot slot in getAllSlotsResults.Value)
            {
                /*if (slot.originalName == originalNames[0])
                {
                    this.createdSlot = slot;
                    break;
                }*/
                if (slot.slotId == this.createdSlot.slotId)
                {
                    this.createdSlot = null;
                    this.createdSlot = slot;
                    bGetCreatedSlot = true;

                    break;
                }
            }

            //Assert.That(this.createdSlot != null);
            TestHelper.Assert.That(bGetCreatedSlot);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator UpdateSlot_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();

            Result<Slot> updateSlotResult = null;
            cloudStorage.UpdateSlot(
                this.createdSlot.slotId,
                Encoding.ASCII.GetBytes(this.payloads[1]),
                this.originalNames[1],
                result => { updateSlotResult = result; });
            yield return TestHelper.WaitForValue(() => updateSlotResult);

            TestHelper.LogResult(updateSlotResult, "Update slot");
            TestHelper.Assert.IsResultOk(updateSlotResult);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator UpdateSlotMetadata_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            string[] tags = {"newTag1", "newTag2"};
            string label = "updatedLabel";
            string customMeta = "updatedCustom";

            Result<Slot> updateSlotMetadataResult = null;
            cloudStorage.UpdateSlotMetadata(
                this.createdSlot.slotId,
                tags,
                label,
                customMeta,
                result => { updateSlotMetadataResult = result; });
            yield return TestHelper.WaitForValue(() => updateSlotMetadataResult);

            TestHelper.LogResult(updateSlotMetadataResult, "Update slot");
            this.updatedSlotMeta = updateSlotMetadataResult.Value;
            TestHelper.Assert.IsResultOk(updateSlotMetadataResult);
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator GetUpdatedSlot_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result<Slot[]> getAllSlotsResults = null;

            cloudStorage.GetAllSlots(result => { getAllSlotsResults = result; });
            yield return TestHelper.WaitForValue(() => getAllSlotsResults);

            TestHelper.LogResult(getAllSlotsResults, "Get all slots 2, after updated");
            TestHelper.Assert.IsResultOk(getAllSlotsResults);

            bool bSlotUpdated = false;

            foreach (Slot slot in getAllSlotsResults.Value)
            {
                if (slot.slotId == this.createdSlot.slotId)
                    if (slot.originalName == this.originalNames[1])
                    {
                        bSlotUpdated = true;

                        break;
                    }
                    else if (slot.originalName == this.originalNames[0])
                    {
                        bSlotUpdated = false;

                        break;
                    }
            }

            TestHelper.Assert.That(bSlotUpdated);
        }

        [UnityTest, TestLog, Order(6)]
        public IEnumerator GetUpdatedSlotMeta_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result<Slot[]> getAllSlotsResults = null;

            cloudStorage.GetAllSlots(result => { getAllSlotsResults = result; });
            yield return TestHelper.WaitForValue(() => getAllSlotsResults);

            TestHelper.LogResult(getAllSlotsResults, "Get all slots, after updated");
            TestHelper.Assert.IsResultOk(getAllSlotsResults);

            bool bSlotMetaUpdated = false;

            foreach (Slot slot in getAllSlotsResults.Value)
            {
                if (slot.slotId == this.createdSlot.slotId)
                {
                    if (slot.tags == this.updatedSlotMeta.tags)
                    {
                        bSlotMetaUpdated = true;
                    }

                    if (slot.label == this.updatedSlotMeta.label)
                    {
                        bSlotMetaUpdated = true;
                    }

                    if (slot.customAttribute == this.updatedSlotMeta.customAttribute)
                    {
                        bSlotMetaUpdated = true;
                    }

                    if (slot.tags == this.createdSlot.tags)
                    {
                        bSlotMetaUpdated = false;

                        break;
                    }

                    if (slot.label == this.createdSlot.label)
                    {
                        bSlotMetaUpdated = false;

                        break;
                    }

                    if (slot.customAttribute == this.createdSlot.customAttribute)
                    {
                        bSlotMetaUpdated = false;

                        break;
                    }
                }
            }

            TestHelper.Assert.That(bSlotMetaUpdated);
        }

        [UnityTest, TestLog, Order(7)]
        public IEnumerator GetSlot_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result<byte[]> getSlotResult = null;

            cloudStorage.GetSlot(this.createdSlot.slotId, result => { getSlotResult = result; });
            yield return TestHelper.WaitForValue(() => getSlotResult);

            TestHelper.LogResult(getSlotResult, "Get a slot");
            TestHelper.Assert.IsResultOk(getSlotResult);
            TestHelper.Assert.That(getSlotResult.Value.SequenceEqual(Encoding.ASCII.GetBytes(this.payloads[1])));
        }

        [UnityTest, TestLog, Order(8)]
        public IEnumerator DeleteSlot_Success()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result deleteSlotResult = null;

            cloudStorage.DeleteSlot(this.createdSlot.slotId, result => { deleteSlotResult = result; });
            yield return TestHelper.WaitForValue(() => deleteSlotResult);

            TestHelper.LogResult(deleteSlotResult, "Delete a slot");
            TestHelper.Assert.IsResultOk(deleteSlotResult);
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator GetAllSlots_DoesntContainUpdatedSlot()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result<Slot[]> getAllSlotsResults = null;

            cloudStorage.GetAllSlots(result => { getAllSlotsResults = result; });
            yield return TestHelper.WaitForValue(() => getAllSlotsResults);

            bool updatedSlotNotFound = true;

            foreach (Slot slot in getAllSlotsResults.Value)
            {
                if (slot.slotId == this.createdSlot.slotId)
                {
                    updatedSlotNotFound = false;
                }
            }

            TestHelper.LogResult(getAllSlotsResults, "Get all slots 3, after deleted");
            TestHelper.Assert.IsResultOk(getAllSlotsResults);
            TestHelper.Assert.That(updatedSlotNotFound, "Slot still exist after deleted!");
        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Teardown()
        {
            CloudStorage cloudStorage = AccelBytePlugin.GetCloudStorage();
            Result<Slot[]> getAllSlotResult = null;
            cloudStorage.GetAllSlots(result => { getAllSlotResult = result; } );
            yield return TestHelper.WaitForValue(() => getAllSlotResult);

            Debug.Log("get all slot is error: " + getAllSlotResult.IsError);
            foreach (var slot in getAllSlotResult.Value)
            {
                Result deleteResult = null;
                cloudStorage.DeleteSlot(slot.slotId, result => { deleteResult = result; });
                yield return TestHelper.WaitForValue(() => deleteResult);
                Debug.Log("delete 1 slot is error: " + deleteResult.IsError);
            }
        }
    }
}