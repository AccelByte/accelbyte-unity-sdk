using UnityEngine;
using System.Collections;
using System;

namespace Oculus.Platform
{

  public class MicrophoneInput : IMicrophone
  {
    AudioClip microphoneClip;
    int lastMicrophoneSample;
    int micBufferSizeSamples;

    public MicrophoneInput()
    {
      int bufferLenSeconds = 1; //minimum size unity allows
      int inputFreq = 48000; //this frequency is fixed throughout the voip system atm
      this.microphoneClip = Microphone.Start(null, true, bufferLenSeconds, inputFreq);
      this.micBufferSizeSamples = bufferLenSeconds * inputFreq;
    }

    public void Start()
    {

    }

    public void Stop()
    {
    }

    public float[] Update()
    {
      int pos = Microphone.GetPosition(null);
      int copySize = 0;
      if (pos < this.lastMicrophoneSample)
      {
        int endOfBufferSize = this.micBufferSizeSamples - this.lastMicrophoneSample;
        copySize = endOfBufferSize + pos;
      }
      else
      {
        copySize = pos - this.lastMicrophoneSample;
      }

      if (copySize == 0) {
        return null;
      }

      float[] samples = new float[copySize]; //TODO 10376403 - pool this
      this.microphoneClip.GetData(samples, this.lastMicrophoneSample);
      this.lastMicrophoneSample = pos;
      return samples;

    }
  }
}
