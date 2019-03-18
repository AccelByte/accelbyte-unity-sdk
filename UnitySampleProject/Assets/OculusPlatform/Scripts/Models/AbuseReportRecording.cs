// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class AbuseReportRecording
  {
    public readonly string RecordingUuid;


    public AbuseReportRecording(IntPtr o)
    {
      this.RecordingUuid = CAPI.ovr_AbuseReportRecording_GetRecordingUuid(o);
    }
  }

}
