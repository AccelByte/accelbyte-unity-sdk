namespace Oculus.Platform.Models
{
  using UnityEngine;
  using System;
  using System.ComponentModel;

  public class PingResult
  {
    public PingResult(UInt64 id, UInt64? pingTimeUsec) {
      this.ID = id;
      this.pingTimeUsec = pingTimeUsec;
    }

    public UInt64 ID { get; private set; }
    public UInt64 PingTimeUsec {
      get {
        return this.pingTimeUsec.HasValue ? this.pingTimeUsec.Value : 0;
      }
    }
    public bool IsTimeout {
      get {
        return !this.pingTimeUsec.HasValue;
      }
    }

    private UInt64? pingTimeUsec;
  }
}
