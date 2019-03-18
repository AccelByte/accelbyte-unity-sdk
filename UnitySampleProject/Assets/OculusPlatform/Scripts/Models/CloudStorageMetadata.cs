// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class CloudStorageMetadata
  {
    public readonly string Bucket;
    public readonly long Counter;
    public readonly uint DataSize;
    public readonly string ExtraData;
    public readonly string Key;
    public readonly ulong SaveTime;
    public readonly CloudStorageDataStatus Status;
    public readonly string VersionHandle;


    public CloudStorageMetadata(IntPtr o)
    {
      this.Bucket = CAPI.ovr_CloudStorageMetadata_GetBucket(o);
      this.Counter = CAPI.ovr_CloudStorageMetadata_GetCounter(o);
      this.DataSize = CAPI.ovr_CloudStorageMetadata_GetDataSize(o);
      this.ExtraData = CAPI.ovr_CloudStorageMetadata_GetExtraData(o);
      this.Key = CAPI.ovr_CloudStorageMetadata_GetKey(o);
      this.SaveTime = CAPI.ovr_CloudStorageMetadata_GetSaveTime(o);
      this.Status = CAPI.ovr_CloudStorageMetadata_GetStatus(o);
      this.VersionHandle = CAPI.ovr_CloudStorageMetadata_GetVersionHandle(o);
    }
  }

  public class CloudStorageMetadataList : DeserializableList<CloudStorageMetadata> {
    public CloudStorageMetadataList(IntPtr a) {
      var count = (int)CAPI.ovr_CloudStorageMetadataArray_GetSize(a);
      this._Data = new List<CloudStorageMetadata>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new CloudStorageMetadata(CAPI.ovr_CloudStorageMetadataArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_CloudStorageMetadataArray_GetNextUrl(a);
    }

  }
}
