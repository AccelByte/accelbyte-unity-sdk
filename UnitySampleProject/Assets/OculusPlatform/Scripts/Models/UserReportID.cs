// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class UserReportID
  {
    public readonly UInt64 ID;


    public UserReportID(IntPtr o)
    {
      this.ID = CAPI.ovr_UserReportID_GetID(o);
    }
  }

}
