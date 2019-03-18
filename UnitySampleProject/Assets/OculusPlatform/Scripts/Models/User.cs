// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class User
  {
    public readonly UInt64 ID;
    public readonly string ImageURL;
    public readonly string InviteToken;
    public readonly string OculusID;
    public readonly string Presence;
    public readonly UserPresenceStatus PresenceStatus;
    public readonly string SmallImageUrl;


    public User(IntPtr o)
    {
      this.ID = CAPI.ovr_User_GetID(o);
      this.ImageURL = CAPI.ovr_User_GetImageUrl(o);
      this.InviteToken = CAPI.ovr_User_GetInviteToken(o);
      this.OculusID = CAPI.ovr_User_GetOculusID(o);
      this.Presence = CAPI.ovr_User_GetPresence(o);
      this.PresenceStatus = CAPI.ovr_User_GetPresenceStatus(o);
      this.SmallImageUrl = CAPI.ovr_User_GetSmallImageUrl(o);
    }
  }

  public class UserList : DeserializableList<User> {
    public UserList(IntPtr a) {
      var count = (int)CAPI.ovr_UserArray_GetSize(a);
      this._Data = new List<User>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new User(CAPI.ovr_UserArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_UserArray_GetNextUrl(a);
    }

  }
}
