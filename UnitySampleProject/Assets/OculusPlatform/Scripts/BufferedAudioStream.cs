//#define VERBOSE_LOGGING
using UnityEngine;
using System.Collections;
using System;

using Oculus.Platform;

public class BufferedAudioStream {
  const bool VerboseLogging = false;
  AudioSource audio;

  float[] audioBuffer;
  int writePos;

  const float bufferLengthSeconds = 0.25f;
  const int sampleRate = 48000;
  const int bufferSize = (int)(BufferedAudioStream.sampleRate * BufferedAudioStream.bufferLengthSeconds);
  const float playbackDelayTimeSeconds = 0.05f;

  float playbackDelayRemaining;
  float remainingBufferTime;

  public BufferedAudioStream(AudioSource audio) {
    this.audioBuffer = new float[BufferedAudioStream.bufferSize];
    this.audio = audio;

    audio.loop = true;
    audio.clip = AudioClip.Create("", BufferedAudioStream.bufferSize, 1, BufferedAudioStream.sampleRate, false);

    Stop();
  }

  public void Update () {
    
    if(this.remainingBufferTime > 0)
    {
#if VERBOSE_LOGGING
      Debug.Log(string.Format("current time: {0}, remainingBufferTime: {1}", Time.time, remainingBufferTime));
#endif

      if (!this.audio.isPlaying && this.remainingBufferTime > BufferedAudioStream.playbackDelayTimeSeconds)
      {
        this.playbackDelayRemaining -= Time.deltaTime;
        if (this.playbackDelayRemaining <= 0)
        {
#if VERBOSE_LOGGING
          Debug.Log("Starting playback");
#endif
          this.audio.Play();
        }
      }

      if (this.audio.isPlaying)
      {
        this.remainingBufferTime -= Time.deltaTime;
        if (this.remainingBufferTime < 0)
        {
          this.remainingBufferTime = 0;
        }
      }
    }

    if (this.remainingBufferTime <= 0)
    {
      if (this.audio.isPlaying)
      {
        Debug.Log("Buffer empty, stopping " + DateTime.Now);
        Stop();
      }
      else
      {
        if (this.writePos != 0)
        {
          Debug.LogError("writePos non zero while not playing, how did this happen?");
        }
      }
    }
  }

  void Stop()
  {
    this.audio.Stop();
    this.audio.time = 0;
    this.writePos = 0;
    this.playbackDelayRemaining = BufferedAudioStream.playbackDelayTimeSeconds;
  }

  public void AddData(float[] samples) {
    int remainingWriteLength = samples.Length;

    if(this.writePos > this.audioBuffer.Length) {
      throw new Exception();
    }

    do {
      int writeLength = remainingWriteLength;
      int remainingSpace = this.audioBuffer.Length - this.writePos;

      if(writeLength > remainingSpace) {
        writeLength = remainingSpace;
      }

      Array.Copy(samples, 0, this.audioBuffer, this.writePos, writeLength);

      remainingWriteLength -= writeLength;
      this.writePos += writeLength;
      if(this.writePos > this.audioBuffer.Length) {
        throw new Exception();
      }
      if(this.writePos == this.audioBuffer.Length) {
        this.writePos = 0;
      }
    } while(remainingWriteLength > 0);

#if VERBOSE_LOGGING
    float prev = remainingBufferTime;
#endif
    this.remainingBufferTime += (float)samples.Length / BufferedAudioStream.sampleRate;
#if VERBOSE_LOGGING
    Debug.Log(string.Format("previous remaining: {0}, new remaining: {1}, added {2} samples", prev, remainingBufferTime, samples.Length));
#endif
    this.audio.clip.SetData(this.audioBuffer, 0);
  }


}
