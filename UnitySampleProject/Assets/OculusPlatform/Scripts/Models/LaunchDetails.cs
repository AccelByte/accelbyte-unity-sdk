// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

#pragma warning disable 0618

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class LaunchDetails
  {
    public readonly string DeeplinkMessage;
    public readonly string LaunchSource;
    public readonly LaunchType LaunchType;
    public readonly UInt64 RoomID;
    // May be null. Check before using.
    public readonly UserList UsersOptional;
    [Obsolete("Deprecated in favor of UsersOptional")]
    public readonly UserList Users;


    public LaunchDetails(IntPtr o)
    {
      this.DeeplinkMessage = CAPI.ovr_LaunchDetails_GetDeeplinkMessage(o);
      this.LaunchSource = CAPI.ovr_LaunchDetails_GetLaunchSource(o);
      this.LaunchType = CAPI.ovr_LaunchDetails_GetLaunchType(o);
      this.RoomID = CAPI.ovr_LaunchDetails_GetRoomID(o);
      {
        var pointer = CAPI.ovr_LaunchDetails_GetUsers(o);
        this.Users = new UserList(pointer);
        if (pointer == IntPtr.Zero) {
          this.UsersOptional = null;
        } else {
          this.UsersOptional = this.Users;
        }
      }
    }
  }

}
