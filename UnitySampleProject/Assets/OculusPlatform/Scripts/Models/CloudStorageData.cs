// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class CloudStorageData
  {
    public readonly string Bucket;
    public readonly byte[] Data;
    public readonly uint DataSize;
    public readonly string Key;


    public CloudStorageData(IntPtr o)
    {
      this.Bucket = CAPI.ovr_CloudStorageData_GetBucket(o);
      this.Data = CAPI.ovr_CloudStorageData_GetData(o);
      this.DataSize = CAPI.ovr_CloudStorageData_GetDataSize(o);
      this.Key = CAPI.ovr_CloudStorageData_GetKey(o);
    }
  }

}
