// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyNetworking
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public interface IMyNetworking
  {
    string ServiceName { get; }

    IMyPeer2Peer Peer2Peer { get; }

    IMyNetworkingChat Chat { get; }

    IMyNetworkingInvite Invite { get; }

    string ProductName { get; }
  }
}
