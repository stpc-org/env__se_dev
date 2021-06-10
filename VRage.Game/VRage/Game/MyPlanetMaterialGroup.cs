// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetMaterialGroup
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [XmlType("MaterialGroup")]
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyPlanetMaterialGroup : ICloneable
  {
    [ProtoMember(15)]
    [XmlAttribute(AttributeName = "Value")]
    public byte Value;
    [ProtoMember(16)]
    [XmlAttribute(AttributeName = "Name")]
    public string Name = "Default";
    [ProtoMember(17)]
    [XmlElement("Rule")]
    public MyPlanetMaterialPlacementRule[] MaterialRules;

    public object Clone()
    {
      MyPlanetMaterialGroup planetMaterialGroup = new MyPlanetMaterialGroup();
      planetMaterialGroup.Value = this.Value;
      planetMaterialGroup.Name = this.Name;
      planetMaterialGroup.MaterialRules = new MyPlanetMaterialPlacementRule[this.MaterialRules.Length];
      for (int index = 0; index < this.MaterialRules.Length; ++index)
        planetMaterialGroup.MaterialRules[index] = this.MaterialRules[index].Clone() as MyPlanetMaterialPlacementRule;
      return (object) planetMaterialGroup;
    }

    protected class VRage_Game_MyPlanetMaterialGroup\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialGroup, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialGroup owner, in byte value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialGroup owner, out byte value) => value = owner.Value;
    }

    protected class VRage_Game_MyPlanetMaterialGroup\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialGroup, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialGroup owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialGroup owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyPlanetMaterialGroup\u003C\u003EMaterialRules\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialGroup, MyPlanetMaterialPlacementRule[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyPlanetMaterialGroup owner,
        in MyPlanetMaterialPlacementRule[] value)
      {
        owner.MaterialRules = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyPlanetMaterialGroup owner,
        out MyPlanetMaterialPlacementRule[] value)
      {
        value = owner.MaterialRules;
      }
    }

    private class VRage_Game_MyPlanetMaterialGroup\u003C\u003EActor : IActivator, IActivator<MyPlanetMaterialGroup>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetMaterialGroup();

      MyPlanetMaterialGroup IActivator<MyPlanetMaterialGroup>.CreateInstance() => new MyPlanetMaterialGroup();
    }
  }
}
