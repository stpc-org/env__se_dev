// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_JetpackDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_JetpackDefinition
  {
    [ProtoMember(4)]
    [XmlArrayItem("Thrust")]
    public List<MyJetpackThrustDefinition> Thrusts;
    [ProtoMember(5)]
    public MyObjectBuilder_ThrustDefinition ThrustProperties;

    protected class VRage_Game_MyObjectBuilder_JetpackDefinition\u003C\u003EThrusts\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_JetpackDefinition, List<MyJetpackThrustDefinition>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_JetpackDefinition owner,
        in List<MyJetpackThrustDefinition> value)
      {
        owner.Thrusts = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_JetpackDefinition owner,
        out List<MyJetpackThrustDefinition> value)
      {
        value = owner.Thrusts;
      }
    }

    protected class VRage_Game_MyObjectBuilder_JetpackDefinition\u003C\u003EThrustProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_JetpackDefinition, MyObjectBuilder_ThrustDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_JetpackDefinition owner,
        in MyObjectBuilder_ThrustDefinition value)
      {
        owner.ThrustProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_JetpackDefinition owner,
        out MyObjectBuilder_ThrustDefinition value)
      {
        value = owner.ThrustProperties;
      }
    }

    private class VRage_Game_MyObjectBuilder_JetpackDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_JetpackDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_JetpackDefinition();

      MyObjectBuilder_JetpackDefinition IActivator<MyObjectBuilder_JetpackDefinition>.CreateInstance() => new MyObjectBuilder_JetpackDefinition();
    }
  }
}
