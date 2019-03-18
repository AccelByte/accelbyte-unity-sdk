namespace Oculus.Platform
{
  using UnityEngine;
  using System;
  using System.Collections.Generic;

  public class VoipAudioSourceHiLevel : MonoBehaviour
  {
    // This is a delegate that exists as a surface for OnAudioFilterRead
    // It will be callled on unity's audio thread
    public class FilterReadDelegate : MonoBehaviour
    {
      public VoipAudioSourceHiLevel parent;
      float[] scratchBuffer;

      void Awake()
      {
        int bufferSizeElements = (int)CAPI.ovr_Voip_GetOutputBufferMaxSize();
        this.scratchBuffer = new float[bufferSizeElements];
      }

      void OnAudioFilterRead(float[] data, int channels)
      {
        int sizeToFetch = data.Length / channels;
        int sourceBufferSize = sizeToFetch;
        if (sourceBufferSize > this.scratchBuffer.Length)
        {
          Array.Clear(data, 0, data.Length);
          throw new Exception(string.Format("Audio system tried to pull {0} bytes, max voip internal ring buffer size {1}", sizeToFetch, this.scratchBuffer.Length));
        }

        int available = this.parent.pcmSource.PeekSizeElements();
        if (available < sourceBufferSize)
        {
          if (VoipAudioSourceHiLevel.verboseLogging)
          {
            Debug.LogFormat(
              "Voip starved! Want {0}, but only have {1} available",
              sourceBufferSize,
              available);
          }
          return;
        }

        int copied = this.parent.pcmSource.GetPCM(this.scratchBuffer, sourceBufferSize);
        if (copied < sourceBufferSize)
        {
          Debug.LogWarningFormat(
            "GetPCM() returned {0} samples, expected {1}",
            copied,
            sourceBufferSize);

          return;
        }

        int dest = 0;
        float tmpPeakAmp = -1;
        for (int i = 0; i < sizeToFetch; i++)
        {
          float val = this.scratchBuffer[i];
            
          for (int j = 0; j < channels; j++)
          {
            data[dest++] = val;
            if (val > tmpPeakAmp)
            {
              tmpPeakAmp = val;
            }
          }
        }

        this.parent.peakAmplitude = tmpPeakAmp;
      }
    }


    int initialPlaybackDelayMS;
    public UInt64 senderID
    {
      set
      {
        this.pcmSource.SetSenderID(value);
      }
    }

    public AudioSource audioSource;
    public float peakAmplitude;

    protected IVoipPCMSource pcmSource;

    static int audioSystemPlaybackFrequency;
    static bool verboseLogging = false;

    protected void Stop() {}

    VoipSampleRate SampleRateToEnum(int rate) {
      switch(rate) {
      case 48000:
        return VoipSampleRate.HZ48000;
      case 44100:
        return VoipSampleRate.HZ44100;
      case 24000:
        return VoipSampleRate.HZ24000;
      default:
        return VoipSampleRate.Unknown;
      }
    }

    protected void Awake()
    {
      CreatePCMSource();
      if(this.audioSource == null) {
        this.audioSource = this.gameObject.AddComponent<AudioSource>();
      }

      this.audioSource.gameObject.AddComponent<FilterReadDelegate>();
      var filterDelegate = this.audioSource.gameObject.GetComponent<FilterReadDelegate>();
      filterDelegate.parent = this;

      this.initialPlaybackDelayMS = 40;

      VoipAudioSourceHiLevel.audioSystemPlaybackFrequency = AudioSettings.outputSampleRate;
      CAPI.ovr_Voip_SetOutputSampleRate(SampleRateToEnum(VoipAudioSourceHiLevel.audioSystemPlaybackFrequency));
      if(VoipAudioSourceHiLevel.verboseLogging) {
        Debug.LogFormat("freq {0}", VoipAudioSourceHiLevel.audioSystemPlaybackFrequency);
      }
    }

    void Start() {
      this.audioSource.Stop();
    }

    protected virtual void CreatePCMSource()
    {
      this.pcmSource = new VoipPCMSourceNative();
    }

    protected static int MSToElements(int ms) {
      return ms * VoipAudioSourceHiLevel.audioSystemPlaybackFrequency / 1000;
    }
      
    void Update()
    {
      this.pcmSource.Update();

      if (!this.audioSource.isPlaying && this.pcmSource.PeekSizeElements() >= VoipAudioSourceHiLevel.MSToElements(this.initialPlaybackDelayMS)) {
        if(VoipAudioSourceHiLevel.verboseLogging) {
          Debug.LogFormat("buffered {0} elements, starting playback", this.pcmSource.PeekSizeElements());
        }

        this.audioSource.Play();
      }
    }
  }
}
