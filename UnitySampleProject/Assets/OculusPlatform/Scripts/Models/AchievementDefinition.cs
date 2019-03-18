// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class AchievementDefinition
  {
    public readonly AchievementType Type;
    public readonly string Name;
    public readonly uint BitfieldLength;
    public readonly ulong Target;


    public AchievementDefinition(IntPtr o)
    {
      this.Type = CAPI.ovr_AchievementDefinition_GetType(o);
      this.Name = CAPI.ovr_AchievementDefinition_GetName(o);
      this.BitfieldLength = CAPI.ovr_AchievementDefinition_GetBitfieldLength(o);
      this.Target = CAPI.ovr_AchievementDefinition_GetTarget(o);
    }
  }

  public class AchievementDefinitionList : DeserializableList<AchievementDefinition> {
    public AchievementDefinitionList(IntPtr a) {
      var count = (int)CAPI.ovr_AchievementDefinitionArray_GetSize(a);
      this._Data = new List<AchievementDefinition>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new AchievementDefinition(CAPI.ovr_AchievementDefinitionArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_AchievementDefinitionArray_GetNextUrl(a);
    }

  }
}
