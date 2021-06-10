// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyContractChancePair
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [XmlType("ContractChance")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [Serializable]
  public class MyContractChancePair
  {
    [ProtoMember(1)]
    public SerializableDefinitionId DefinitionId;
    [ProtoMember(3)]
    [DefaultValue(0.0f)]
    public float Value;

    protected class VRage_Game_ObjectBuilders_Definitions_MyContractChancePair\u003C\u003EDefinitionId\u003C\u003EAccessor : IMemberAccessor<MyContractChancePair, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContractChancePair owner, in SerializableDefinitionId value) => owner.DefinitionId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContractChancePair owner, out SerializableDefinitionId value) => value = owner.DefinitionId;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyContractChancePair\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyContractChancePair, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContractChancePair owner, in float value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContractChancePair owner, out float value) => value = owner.Value;
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyContractChancePair\u003C\u003EActor : IActivator, IActivator<MyContractChancePair>
    {
      object IActivator.CreateInstance() => (object) new MyContractChancePair();

      MyContractChancePair IActivator<MyContractChancePair>.CreateInstance() => new MyContractChancePair();
    }
  }
}
