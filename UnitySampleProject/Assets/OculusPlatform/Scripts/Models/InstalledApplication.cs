// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class InstalledApplication
  {
    public readonly string ApplicationId;
    public readonly string PackageName;
    public readonly string Status;
    public readonly int VersionCode;
    public readonly string VersionName;


    public InstalledApplication(IntPtr o)
    {
      this.ApplicationId = CAPI.ovr_InstalledApplication_GetApplicationId(o);
      this.PackageName = CAPI.ovr_InstalledApplication_GetPackageName(o);
      this.Status = CAPI.ovr_InstalledApplication_GetStatus(o);
      this.VersionCode = CAPI.ovr_InstalledApplication_GetVersionCode(o);
      this.VersionName = CAPI.ovr_InstalledApplication_GetVersionName(o);
    }
  }

  public class InstalledApplicationList : DeserializableList<InstalledApplication> {
    public InstalledApplicationList(IntPtr a) {
      var count = (int)CAPI.ovr_InstalledApplicationArray_GetSize(a);
      this._Data = new List<InstalledApplication>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new InstalledApplication(CAPI.ovr_InstalledApplicationArray_GetElement(a, (UIntPtr)i)));
      }

    }

  }
}
