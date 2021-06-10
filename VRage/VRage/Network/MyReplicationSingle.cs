// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyReplicationSingle
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Network
{
  public class MyReplicationSingle : MyReplicationLayerBase
  {
    public MyReplicationSingle(EndpointId localEndpoint) => this.SetLocalEndpoint(localEndpoint);

    protected override void DispatchEvent<T1, T2, T3, T4, T5, T6, T7, T8>(
      CallSite callSite,
      EndpointId recipient,
      Vector3D? position,
      ref T1 arg1,
      ref T2 arg2,
      ref T3 arg3,
      ref T4 arg4,
      ref T5 arg5,
      ref T6 arg6,
      ref T7 arg7,
      ref T8 arg8)
    {
      if (!MyReplicationLayerBase.ShouldServerInvokeLocally(callSite, this.m_localEndpoint, recipient))
        return;
      this.InvokeLocally<T1, T2, T3, T4, T5, T6, T7>((CallSite<T1, T2, T3, T4, T5, T6, T7>) callSite, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public override void AdvanceSyncTime()
    {
    }
  }
}
