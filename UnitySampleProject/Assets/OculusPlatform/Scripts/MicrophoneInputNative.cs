using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace Oculus.Platform
{
  public class MicrophoneInputNative : IMicrophone
  {
    IntPtr mic;

    int tempBufferSize = 960 * 10;
    float[] tempBuffer;

    public MicrophoneInputNative()
    {
      this.mic = CAPI.ovr_Microphone_Create();
      CAPI.ovr_Microphone_Start(this.mic);
      this.tempBuffer = new float[this.tempBufferSize];
      Debug.Log(this.mic);
    }

    public float[] Update()
    {
      ulong readSize = (ulong)CAPI.ovr_Microphone_ReadData(this.mic, this.tempBuffer, (UIntPtr) this.tempBufferSize);
      if (readSize > 0)
      {

        float[] outBuffer = new float[readSize];
        Array.Copy(this.tempBuffer, outBuffer, (int)readSize);
        return outBuffer;
      }
      return null;
    }

    public void Start()
    {

    }

    public void Stop()
    {
      CAPI.ovr_Microphone_Stop(this.mic);
      CAPI.ovr_Microphone_Destroy(this.mic);
    }
  }
}
