// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class Product
  {
    public readonly string Description;
    public readonly string FormattedPrice;
    public readonly string Name;
    public readonly string Sku;


    public Product(IntPtr o)
    {
      this.Description = CAPI.ovr_Product_GetDescription(o);
      this.FormattedPrice = CAPI.ovr_Product_GetFormattedPrice(o);
      this.Name = CAPI.ovr_Product_GetName(o);
      this.Sku = CAPI.ovr_Product_GetSKU(o);
    }
  }

  public class ProductList : DeserializableList<Product> {
    public ProductList(IntPtr a) {
      var count = (int)CAPI.ovr_ProductArray_GetSize(a);
      this._Data = new List<Product>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new Product(CAPI.ovr_ProductArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_ProductArray_GetNextUrl(a);
    }

  }
}
