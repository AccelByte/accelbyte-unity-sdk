// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

#pragma warning disable 0618

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class MatchmakingEnqueueResult
  {
    // May be null. Check before using.
    public readonly MatchmakingAdminSnapshot AdminSnapshotOptional;
    [Obsolete("Deprecated in favor of AdminSnapshotOptional")]
    public readonly MatchmakingAdminSnapshot AdminSnapshot;
    public readonly uint AverageWait;
    public readonly uint MatchesInLastHourCount;
    public readonly uint MaxExpectedWait;
    public readonly string Pool;
    public readonly uint RecentMatchPercentage;
    public readonly string RequestHash;


    public MatchmakingEnqueueResult(IntPtr o)
    {
      {
        var pointer = CAPI.ovr_MatchmakingEnqueueResult_GetAdminSnapshot(o);
        this.AdminSnapshot = new MatchmakingAdminSnapshot(pointer);
        if (pointer == IntPtr.Zero) {
          this.AdminSnapshotOptional = null;
        } else {
          this.AdminSnapshotOptional = this.AdminSnapshot;
        }
      }
      this.AverageWait = CAPI.ovr_MatchmakingEnqueueResult_GetAverageWait(o);
      this.MatchesInLastHourCount = CAPI.ovr_MatchmakingEnqueueResult_GetMatchesInLastHourCount(o);
      this.MaxExpectedWait = CAPI.ovr_MatchmakingEnqueueResult_GetMaxExpectedWait(o);
      this.Pool = CAPI.ovr_MatchmakingEnqueueResult_GetPool(o);
      this.RecentMatchPercentage = CAPI.ovr_MatchmakingEnqueueResult_GetRecentMatchPercentage(o);
      this.RequestHash = CAPI.ovr_MatchmakingEnqueueResult_GetRequestHash(o);
    }
  }

}
