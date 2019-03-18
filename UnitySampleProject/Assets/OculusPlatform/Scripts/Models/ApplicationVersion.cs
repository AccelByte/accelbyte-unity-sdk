// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class ApplicationVersion
  {
    public readonly int CurrentCode;
    public readonly string CurrentName;
    public readonly int LatestCode;
    public readonly string LatestName;


    public ApplicationVersion(IntPtr o)
    {
      this.CurrentCode = CAPI.ovr_ApplicationVersion_GetCurrentCode(o);
      this.CurrentName = CAPI.ovr_ApplicationVersion_GetCurrentName(o);
      this.LatestCode = CAPI.ovr_ApplicationVersion_GetLatestCode(o);
      this.LatestName = CAPI.ovr_ApplicationVersion_GetLatestName(o);
    }
  }

}
