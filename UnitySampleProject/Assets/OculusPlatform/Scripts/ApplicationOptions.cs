// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class ApplicationOptions {

    public ApplicationOptions() {
      this.Handle = CAPI.ovr_ApplicationOptions_Create();
    }

    public void SetDeeplinkMessage(string value) {
      CAPI.ovr_ApplicationOptions_SetDeeplinkMessage(this.Handle, value);
    }


    // For passing to native C
    public static explicit operator IntPtr(ApplicationOptions options) {
      return options != null ? options.Handle : IntPtr.Zero;
    }

    ~ApplicationOptions() {
      CAPI.ovr_ApplicationOptions_Destroy(this.Handle);
    }

    IntPtr Handle;
  }
}
