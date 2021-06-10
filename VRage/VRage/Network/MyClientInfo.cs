// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyClientInfo
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Network
{
  public struct MyClientInfo
  {
    private readonly MyClient m_client;

    public bool IsValid => this.m_client != null;

    public MyClientStateBase State => this.m_client.State;

    public Endpoint EndpointId => this.m_client.State.EndpointId;

    public float PriorityMultiplier => this.m_client.PriorityMultiplier;

    internal MyClientInfo(MyClient client) => this.m_client = client;

    public bool HasReplicable(IMyReplicable replicable) => this.m_client.Replicables.ContainsKey(replicable);

    public bool IsReplicableReady(IMyReplicable replicable) => this.m_client.IsReplicableReady(replicable);

    public bool PlayerControllableUsesPredictedPhysics => this.m_client.PlayerControllableUsesPredictedPhysics;
  }
}
