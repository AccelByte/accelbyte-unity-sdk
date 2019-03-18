// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class LivestreamingStatus
  {
    public readonly bool CommentsVisible;
    public readonly bool IsPaused;
    public readonly bool LivestreamingEnabled;
    public readonly int LivestreamingType;
    public readonly bool MicEnabled;


    public LivestreamingStatus(IntPtr o)
    {
      this.CommentsVisible = CAPI.ovr_LivestreamingStatus_GetCommentsVisible(o);
      this.IsPaused = CAPI.ovr_LivestreamingStatus_GetIsPaused(o);
      this.LivestreamingEnabled = CAPI.ovr_LivestreamingStatus_GetLivestreamingEnabled(o);
      this.LivestreamingType = CAPI.ovr_LivestreamingStatus_GetLivestreamingType(o);
      this.MicEnabled = CAPI.ovr_LivestreamingStatus_GetMicEnabled(o);
    }
  }

}
