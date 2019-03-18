// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class SdkAccount
  {
    public readonly SdkAccountType AccountType;
    public readonly UInt64 UserId;


    public SdkAccount(IntPtr o)
    {
      this.AccountType = CAPI.ovr_SdkAccount_GetAccountType(o);
      this.UserId = CAPI.ovr_SdkAccount_GetUserId(o);
    }
  }

  public class SdkAccountList : DeserializableList<SdkAccount> {
    public SdkAccountList(IntPtr a) {
      var count = (int)CAPI.ovr_SdkAccountArray_GetSize(a);
      this._Data = new List<SdkAccount>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new SdkAccount(CAPI.ovr_SdkAccountArray_GetElement(a, (UIntPtr)i)));
      }

    }

  }
}
