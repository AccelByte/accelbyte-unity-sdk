// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

#pragma warning disable 0618

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class MatchmakingEnqueuedUser
  {
    public readonly Dictionary<string, string> CustomData;
    // May be null. Check before using.
    public readonly User UserOptional;
    [Obsolete("Deprecated in favor of UserOptional")]
    public readonly User User;


    public MatchmakingEnqueuedUser(IntPtr o)
    {
      this.CustomData = CAPI.DataStoreFromNative(CAPI.ovr_MatchmakingEnqueuedUser_GetCustomData(o));
      {
        var pointer = CAPI.ovr_MatchmakingEnqueuedUser_GetUser(o);
        this.User = new User(pointer);
        if (pointer == IntPtr.Zero) {
          this.UserOptional = null;
        } else {
          this.UserOptional = this.User;
        }
      }
    }
  }

  public class MatchmakingEnqueuedUserList : DeserializableList<MatchmakingEnqueuedUser> {
    public MatchmakingEnqueuedUserList(IntPtr a) {
      var count = (int)CAPI.ovr_MatchmakingEnqueuedUserArray_GetSize(a);
      this._Data = new List<MatchmakingEnqueuedUser>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new MatchmakingEnqueuedUser(CAPI.ovr_MatchmakingEnqueuedUserArray_GetElement(a, (UIntPtr)i)));
      }

    }

  }
}
