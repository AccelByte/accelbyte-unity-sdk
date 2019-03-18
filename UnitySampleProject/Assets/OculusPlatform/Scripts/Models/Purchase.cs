// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class Purchase
  {
    public readonly DateTime ExpirationTime;
    public readonly DateTime GrantTime;
    public readonly UInt64 ID;
    public readonly string Sku;


    public Purchase(IntPtr o)
    {
      this.ExpirationTime = CAPI.ovr_Purchase_GetExpirationTime(o);
      this.GrantTime = CAPI.ovr_Purchase_GetGrantTime(o);
      this.ID = CAPI.ovr_Purchase_GetPurchaseID(o);
      this.Sku = CAPI.ovr_Purchase_GetSKU(o);
    }
  }

  public class PurchaseList : DeserializableList<Purchase> {
    public PurchaseList(IntPtr a) {
      var count = (int)CAPI.ovr_PurchaseArray_GetSize(a);
      this._Data = new List<Purchase>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new Purchase(CAPI.ovr_PurchaseArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_PurchaseArray_GetNextUrl(a);
    }

  }
}
