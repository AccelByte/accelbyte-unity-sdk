// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class OrgScopedID
  {
    public readonly UInt64 ID;


    public OrgScopedID(IntPtr o)
    {
      this.ID = CAPI.ovr_OrgScopedID_GetID(o);
    }
  }

}
