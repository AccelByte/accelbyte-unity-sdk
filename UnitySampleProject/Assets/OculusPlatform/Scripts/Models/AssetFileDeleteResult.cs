// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class AssetFileDeleteResult
  {
    public readonly UInt64 AssetFileId;
    public readonly UInt64 AssetId;
    public readonly string Filepath;
    public readonly bool Success;


    public AssetFileDeleteResult(IntPtr o)
    {
      this.AssetFileId = CAPI.ovr_AssetFileDeleteResult_GetAssetFileId(o);
      this.AssetId = CAPI.ovr_AssetFileDeleteResult_GetAssetId(o);
      this.Filepath = CAPI.ovr_AssetFileDeleteResult_GetFilepath(o);
      this.Success = CAPI.ovr_AssetFileDeleteResult_GetSuccess(o);
    }
  }

}
