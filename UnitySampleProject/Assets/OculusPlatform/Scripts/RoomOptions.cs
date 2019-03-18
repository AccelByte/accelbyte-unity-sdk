// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class RoomOptions {

    public RoomOptions() {
      this.Handle = CAPI.ovr_RoomOptions_Create();
    }

    public void SetDataStore(string key, string value) {
      CAPI.ovr_RoomOptions_SetDataStoreString(this.Handle, key, value);
    }

    public void ClearDataStore() {
      CAPI.ovr_RoomOptions_ClearDataStore(this.Handle);
    }

    public void SetExcludeRecentlyMet(bool value) {
      CAPI.ovr_RoomOptions_SetExcludeRecentlyMet(this.Handle, value);
    }

    public void SetMaxUserResults(uint value) {
      CAPI.ovr_RoomOptions_SetMaxUserResults(this.Handle, value);
    }

    public void SetOrdering(UserOrdering value) {
      CAPI.ovr_RoomOptions_SetOrdering(this.Handle, value);
    }

    public void SetRecentlyMetTimeWindow(TimeWindow value) {
      CAPI.ovr_RoomOptions_SetRecentlyMetTimeWindow(this.Handle, value);
    }

    public void SetRoomId(UInt64 value) {
      CAPI.ovr_RoomOptions_SetRoomId(this.Handle, value);
    }

    public void SetTurnOffUpdates(bool value) {
      CAPI.ovr_RoomOptions_SetTurnOffUpdates(this.Handle, value);
    }


    // For passing to native C
    public static explicit operator IntPtr(RoomOptions options) {
      return options != null ? options.Handle : IntPtr.Zero;
    }

    ~RoomOptions() {
      CAPI.ovr_RoomOptions_Destroy(this.Handle);
    }

    IntPtr Handle;
  }
}
