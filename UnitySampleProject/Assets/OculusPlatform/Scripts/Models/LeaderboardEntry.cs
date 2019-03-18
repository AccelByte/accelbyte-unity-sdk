// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class LeaderboardEntry
  {
    public readonly byte[] ExtraData;
    public readonly int Rank;
    public readonly long Score;
    public readonly DateTime Timestamp;
    public readonly User User;


    public LeaderboardEntry(IntPtr o)
    {
      this.ExtraData = CAPI.ovr_LeaderboardEntry_GetExtraData(o);
      this.Rank = CAPI.ovr_LeaderboardEntry_GetRank(o);
      this.Score = CAPI.ovr_LeaderboardEntry_GetScore(o);
      this.Timestamp = CAPI.ovr_LeaderboardEntry_GetTimestamp(o);
      this.User = new User(CAPI.ovr_LeaderboardEntry_GetUser(o));
    }
  }

  public class LeaderboardEntryList : DeserializableList<LeaderboardEntry> {
    public LeaderboardEntryList(IntPtr a) {
      var count = (int)CAPI.ovr_LeaderboardEntryArray_GetSize(a);
      this._Data = new List<LeaderboardEntry>(count);
      for (int i = 0; i < count; i++) {
        this._Data.Add(new LeaderboardEntry(CAPI.ovr_LeaderboardEntryArray_GetElement(a, (UIntPtr)i)));
      }

      this.TotalCount = CAPI.ovr_LeaderboardEntryArray_GetTotalCount(a);
      this._PreviousUrl = CAPI.ovr_LeaderboardEntryArray_GetPreviousUrl(a);
      this._NextUrl = CAPI.ovr_LeaderboardEntryArray_GetNextUrl(a);
    }

    public readonly ulong TotalCount;
  }
}
