// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class MatchmakingOptions {

    public MatchmakingOptions() {
      this.Handle = CAPI.ovr_MatchmakingOptions_Create();
    }

    public void SetCreateRoomDataStore(string key, string value) {
      CAPI.ovr_MatchmakingOptions_SetCreateRoomDataStoreString(this.Handle, key, value);
    }

    public void ClearCreateRoomDataStore() {
      CAPI.ovr_MatchmakingOptions_ClearCreateRoomDataStore(this.Handle);
    }

    public void SetCreateRoomJoinPolicy(RoomJoinPolicy value) {
      CAPI.ovr_MatchmakingOptions_SetCreateRoomJoinPolicy(this.Handle, value);
    }

    public void SetCreateRoomMaxUsers(uint value) {
      CAPI.ovr_MatchmakingOptions_SetCreateRoomMaxUsers(this.Handle, value);
    }

    public void AddEnqueueAdditionalUser(UInt64 userID) {
      CAPI.ovr_MatchmakingOptions_AddEnqueueAdditionalUser(this.Handle, userID);
    }

    public void ClearEnqueueAdditionalUsers() {
      CAPI.ovr_MatchmakingOptions_ClearEnqueueAdditionalUsers(this.Handle);
    }

    public void SetEnqueueDataSettings(string key, int value) {
      CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsInt(this.Handle, key, value);
    }

    public void SetEnqueueDataSettings(string key, double value) {
      CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsDouble(this.Handle, key, value);
    }

    public void SetEnqueueDataSettings(string key, string value) {
      CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsString(this.Handle, key, value);
    }

    public void ClearEnqueueDataSettings() {
      CAPI.ovr_MatchmakingOptions_ClearEnqueueDataSettings(this.Handle);
    }

    public void SetEnqueueIsDebug(bool value) {
      CAPI.ovr_MatchmakingOptions_SetEnqueueIsDebug(this.Handle, value);
    }

    public void SetEnqueueQueryKey(string value) {
      CAPI.ovr_MatchmakingOptions_SetEnqueueQueryKey(this.Handle, value);
    }


    // For passing to native C
    public static explicit operator IntPtr(MatchmakingOptions options) {
      return options != null ? options.Handle : IntPtr.Zero;
    }

    ~MatchmakingOptions() {
      CAPI.ovr_MatchmakingOptions_Destroy(this.Handle);
    }

    IntPtr Handle;
  }
}
