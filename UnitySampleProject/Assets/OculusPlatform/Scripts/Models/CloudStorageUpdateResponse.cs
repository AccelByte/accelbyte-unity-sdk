// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class CloudStorageUpdateResponse
  {
    public readonly string Bucket;
    public readonly string Key;
    public readonly CloudStorageUpdateStatus Status;
    public readonly string VersionHandle;


    public CloudStorageUpdateResponse(IntPtr o)
    {
      this.Bucket = CAPI.ovr_CloudStorageUpdateResponse_GetBucket(o);
      this.Key = CAPI.ovr_CloudStorageUpdateResponse_GetKey(o);
      this.Status = CAPI.ovr_CloudStorageUpdateResponse_GetStatus(o);
      this.VersionHandle = CAPI.ovr_CloudStorageUpdateResponse_GetVersionHandle(o);
    }
  }

}
