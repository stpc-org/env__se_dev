// Decompiled with JetBrains decompiler
// Type: VRage.Game.OxygenRoom
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public struct OxygenRoom
  {
    [ProtoMember(112)]
    public Vector3I StartingPosition;
    [ProtoMember(115)]
    [XmlAttribute]
    public float OxygenAmount;

    protected class VRage_Game_OxygenRoom\u003C\u003EStartingPosition\u003C\u003EAccessor : IMemberAccessor<OxygenRoom, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref OxygenRoom owner, in Vector3I value) => owner.StartingPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref OxygenRoom owner, out Vector3I value) => value = owner.StartingPosition;
    }

    protected class VRage_Game_OxygenRoom\u003C\u003EOxygenAmount\u003C\u003EAccessor : IMemberAccessor<OxygenRoom, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref OxygenRoom owner, in float value) => owner.OxygenAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref OxygenRoom owner, out float value) => value = owner.OxygenAmount;
    }

    private class VRage_Game_OxygenRoom\u003C\u003EActor : IActivator, IActivator<OxygenRoom>
    {
      object IActivator.CreateInstance() => (object) new OxygenRoom();

      OxygenRoom IActivator<OxygenRoom>.CreateInstance() => new OxygenRoom();
    }
  }
}
