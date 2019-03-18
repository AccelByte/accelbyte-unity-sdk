// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

#pragma warning disable 0618

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class Room
  {
    public readonly UInt64 ApplicationID;
    public readonly Dictionary<string, string> DataStore;
    public readonly string Description;
    public readonly UInt64 ID;
    // May be null. Check before using.
    public readonly UserList InvitedUsersOptional;
    [Obsolete("Deprecated in favor of InvitedUsersOptional")]
    public readonly UserList InvitedUsers;
    public readonly bool IsMembershipLocked;
    public readonly RoomJoinPolicy JoinPolicy;
    public readonly RoomJoinability Joinability;
    // May be null. Check before using.
    public readonly MatchmakingEnqueuedUserList MatchedUsersOptional;
    [Obsolete("Deprecated in favor of MatchedUsersOptional")]
    public readonly MatchmakingEnqueuedUserList MatchedUsers;
    public readonly uint MaxUsers;
    public readonly string Name;
    // May be null. Check before using.
    public readonly User OwnerOptional;
    [Obsolete("Deprecated in favor of OwnerOptional")]
    public readonly User Owner;
    public readonly RoomType Type;
    // May be null. Check before using.
    public readonly UserList UsersOptional;
    [Obsolete("Deprecated in favor of UsersOptional")]
    public readonly UserList Users;
    public readonly uint Version;


    public Room(IntPtr o)
    {
      this.ApplicationID = CAPI.ovr_Room_GetApplicationID(o);
      this.DataStore = CAPI.DataStoreFromNative(CAPI.ovr_Room_GetDataStore(o));
      this.Description = CAPI.ovr_Room_GetDescription(o);
      this.ID = CAPI.ovr_Room_GetID(o);
      {
        var pointer = CAPI.ovr_Room_GetInvitedUsers(o);
        this.InvitedUsers = new UserList(pointer);
        if (pointer == IntPtr.Zero) {
          this.InvitedUsersOptional = null;
        } else {
          this.InvitedUsersOptional = this.InvitedUsers;
        }
      }
      this.IsMembershipLocked = CAPI.ovr_Room_GetIsMembershipLocked(o);
      this.JoinPolicy = CAPI.ovr_Room_GetJoinPolicy(o);
      this.Joinability = CAPI.ovr_Room_GetJoinability(o);
      {
        var pointer = CAPI.ovr_Room_GetMatchedUsers(o);
        this.MatchedUsers = new MatchmakingEnqueuedUserList(pointer);
        if (pointer == IntPtr.Zero) {
          this.MatchedUsersOptional = null;
        } else {
          this.MatchedUsersOptional = this.MatchedUsers;
        }
      }
      this.MaxUsers = CAPI.ovr_Room_GetMaxUsers(o);
      this.Name = CAPI.ovr_Room_GetName(o);
      {
        var pointer = CAPI.ovr_Room_GetOwner(o);
        this.Owner = new User(pointer);
        if (pointer == IntPtr.Zero) {
          this.OwnerOptional = null;
        } else {
          this.OwnerOptional = this.Owner;
        }
      }
      this.Type = CAPI.ovr_Room_GetType(o);
      {
        var pointer = CAPI.ovr_Room_GetUsers(o);
        this.Users = new UserList(pointer);
        if (pointer == IntPtr.Zero) {
          this.UsersOptional = null;
        } else {
          this.UsersOptional = this.Users;
        }
      }
      this.Version = CAPI.ovr_Room_GetVersion(o);
    }
  }

  public class RoomList : DeserializableList<Room> {
    public RoomList(IntPtr a) {
      var count = (int)CAPI.ovr_RoomArray_GetSize(a);
      this._Data = new List<Room>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new Room(CAPI.ovr_RoomArray_GetElement(a, (UIntPtr)i)));
      }

      this._NextUrl = CAPI.ovr_RoomArray_GetNextUrl(a);
    }

  }
}
