// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class AssetFileDownloadUpdate
  {
    public readonly UInt64 AssetFileId;
    public readonly UInt64 AssetId;
    public readonly uint BytesTotal;
    public readonly int BytesTransferred;
    public readonly bool Completed;


    public AssetFileDownloadUpdate(IntPtr o)
    {
      this.AssetFileId = CAPI.ovr_AssetFileDownloadUpdate_GetAssetFileId(o);
      this.AssetId = CAPI.ovr_AssetFileDownloadUpdate_GetAssetId(o);
      this.BytesTotal = CAPI.ovr_AssetFileDownloadUpdate_GetBytesTotal(o);
      this.BytesTransferred = CAPI.ovr_AssetFileDownloadUpdate_GetBytesTransferred(o);
      this.Completed = CAPI.ovr_AssetFileDownloadUpdate_GetCompleted(o);
    }
  }

}
