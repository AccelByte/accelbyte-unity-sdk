// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class RoomInviteNotification
  {
    public readonly UInt64 ID;
    public readonly UInt64 RoomID;
    public readonly UInt64 SenderID;
    public readonly DateTime SentTime;


    public RoomInviteNotification(IntPtr o)
    {
      this.ID = CAPI.ovr_RoomInviteNotification_GetID(o);
      this.RoomID = CAPI.ovr_RoomInviteNotification_GetRoomID(o);
      this.SenderID = CAPI.ovr_RoomInviteNotification_GetSenderID(o);
      this.SentTime = CAPI.ovr_RoomInviteNotification_GetSentTime(o);
    }
  }

  public class RoomInviteNotificationList : DeserializableList<RoomInviteNotification> {
    public RoomInviteNotificationList(IntPtr a) {
      var count = (int)CAPI.ovr_RoomInviteNotificationArray_GetSize(a);
      this._Data = new List<RoomInviteNotification>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new RoomInviteNotification(CAPI.ovr_RoomInviteNotificationArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_RoomInviteNotificationArray_GetNextUrl(a);
    }

  }
}
