// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class AchievementUpdate
  {
    public readonly bool JustUnlocked;
    public readonly string Name;


    public AchievementUpdate(IntPtr o)
    {
      this.JustUnlocked = CAPI.ovr_AchievementUpdate_GetJustUnlocked(o);
      this.Name = CAPI.ovr_AchievementUpdate_GetName(o);
    }
  }

}
