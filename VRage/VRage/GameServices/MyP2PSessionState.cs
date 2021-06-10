// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyP2PSessionState
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public struct MyP2PSessionState
  {
    public bool ConnectionActive;
    public bool Connecting;
    public MyP2PSessionErrorEnum LastSessionError;
    public bool UsingRelay;
    public int BytesQueuedForSend;
    public int PacketsQueuedForSend;
    public uint RemoteIP;
    public ushort RemotePort;
  }
}
