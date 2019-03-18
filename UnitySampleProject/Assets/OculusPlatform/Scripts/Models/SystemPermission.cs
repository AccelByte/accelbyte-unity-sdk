// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class SystemPermission
  {
    public readonly bool HasPermission;
    public readonly PermissionGrantStatus PermissionGrantStatus;


    public SystemPermission(IntPtr o)
    {
      this.HasPermission = CAPI.ovr_SystemPermission_GetHasPermission(o);
      this.PermissionGrantStatus = CAPI.ovr_SystemPermission_GetPermissionGrantStatus(o);
    }
  }

}
