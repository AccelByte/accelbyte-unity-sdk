// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class MatchmakingAdminSnapshot
  {
    public readonly MatchmakingAdminSnapshotCandidateList Candidates;
    public readonly double MyCurrentThreshold;


    public MatchmakingAdminSnapshot(IntPtr o)
    {
      this.Candidates = new MatchmakingAdminSnapshotCandidateList(CAPI.ovr_MatchmakingAdminSnapshot_GetCandidates(o));
      this.MyCurrentThreshold = CAPI.ovr_MatchmakingAdminSnapshot_GetMyCurrentThreshold(o);
    }
  }

}
