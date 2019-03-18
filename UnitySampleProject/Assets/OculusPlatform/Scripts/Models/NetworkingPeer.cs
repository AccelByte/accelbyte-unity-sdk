namespace Oculus.Platform.Models
{
  using UnityEngine;
  using System;
  using System.ComponentModel;

  public class NetworkingPeer
  {
    public NetworkingPeer(UInt64 id, PeerConnectionState state) {
      this.ID = id;
      this.State = state;
    }

    public UInt64 ID { get; private set; }
    public PeerConnectionState State { get; private set; }
  }
}
