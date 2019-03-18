// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class LivestreamingVideoStats
  {
    public readonly int CommentCount;
    public readonly int ReactionCount;
    public readonly string TotalViews;


    public LivestreamingVideoStats(IntPtr o)
    {
      this.CommentCount = CAPI.ovr_LivestreamingVideoStats_GetCommentCount(o);
      this.ReactionCount = CAPI.ovr_LivestreamingVideoStats_GetReactionCount(o);
      this.TotalViews = CAPI.ovr_LivestreamingVideoStats_GetTotalViews(o);
    }
  }

}
