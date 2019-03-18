// This file was @generated with LibOVRPlatform/codegen/main. Do not modify it!

namespace Oculus.Platform.Models
{
  using System;
  using System.Collections;
  using Models;
  using System.Collections.Generic;
  using UnityEngine;

  public class SystemVoipState
  {
    public readonly VoipMuteState MicrophoneMuted;
    public readonly SystemVoipStatus Status;


    public SystemVoipState(IntPtr o)
    {
      this.MicrophoneMuted = CAPI.ovr_SystemVoipState_GetMicrophoneMuted(o);
      this.Status = CAPI.ovr_SystemVoipState_GetStatus(o);
    }
  }

}
