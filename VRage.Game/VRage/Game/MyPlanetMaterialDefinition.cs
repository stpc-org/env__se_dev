// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetMaterialDefinition
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
  [ProtoContract]
  public class MyPlanetMaterialDefinition : ICloneable
  {
    [ProtoMember(3)]
    [XmlAttribute(AttributeName = "Material")]
    public string Material;
    [ProtoMember(4)]
    [XmlAttribute(AttributeName = "Value")]
    public byte Value;
    [ProtoMember(5)]
    [XmlAttribute(AttributeName = "MaxDepth")]
    public float MaxDepth = 1f;
    [ProtoMember(6)]
    [XmlArrayItem("Layer")]
    public MyPlanetMaterialLayer[] Layers;

    public virtual bool IsRule => false;

    public bool HasLayers => this.Layers != null && (uint) this.Layers.Length > 0U;

    public string FirstOrDefault
    {
      get
      {
        if (this.Material != null)
          return this.Material;
        return this.HasLayers ? this.Layers[0].Material : (string) null;
      }
    }

    public object Clone() => (object) new MyPlanetMaterialDefinition()
    {
      Material = this.Material,
      Value = this.Value,
      MaxDepth = this.MaxDepth,
      Layers = (this.Layers == null ? (MyPlanetMaterialLayer[]) null : this.Layers.Clone() as MyPlanetMaterialLayer[])
    };

    protected class VRage_Game_MyPlanetMaterialDefinition\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialDefinition owner, in string value) => owner.Material = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialDefinition owner, out string value) => value = owner.Material;
    }

    protected class VRage_Game_MyPlanetMaterialDefinition\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialDefinition owner, in byte value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialDefinition owner, out byte value) => value = owner.Value;
    }

    protected class VRage_Game_MyPlanetMaterialDefinition\u003C\u003EMaxDepth\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialDefinition owner, in float value) => owner.MaxDepth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialDefinition owner, out float value) => value = owner.MaxDepth;
    }

    protected class VRage_Game_MyPlanetMaterialDefinition\u003C\u003ELayers\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, MyPlanetMaterialLayer[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyPlanetMaterialDefinition owner,
        in MyPlanetMaterialLayer[] value)
      {
        owner.Layers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyPlanetMaterialDefinition owner,
        out MyPlanetMaterialLayer[] value)
      {
        value = owner.Layers;
      }
    }

    private class VRage_Game_MyPlanetMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyPlanetMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetMaterialDefinition();

      MyPlanetMaterialDefinition IActivator<MyPlanetMaterialDefinition>.CreateInstance() => new MyPlanetMaterialDefinition();
    }
  }
}
