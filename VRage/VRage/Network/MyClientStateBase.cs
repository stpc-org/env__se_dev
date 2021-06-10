// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyClientStateBase
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Network
{
  public abstract class MyClientStateBase
  {
    public MyTimeSpan ClientTimeStamp;
    public float? ReplicationRange;
    private short m_ping;

    public Endpoint EndpointId { get; set; }

    public int PlayerSerialId { get; set; }

    public abstract void Serialize(BitStream stream, bool outOfOrder);

    public virtual Vector3D? Position { get; protected set; }

    public short Ping
    {
      get => this.m_ping;
      set => this.m_ping = value;
    }

    public abstract void Update();

    public abstract IMyReplicable ControlledReplicable { get; }

    public abstract IMyReplicable CharacterReplicable { get; }

    public bool IsControllingCharacter { get; protected set; }

    public bool IsControllingJetpack { get; protected set; }

    public bool IsControllingGrid { get; protected set; }

    public abstract void ResetControlledEntityControls();
  }
}
