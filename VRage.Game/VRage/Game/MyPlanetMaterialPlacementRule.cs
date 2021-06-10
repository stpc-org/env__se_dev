// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetMaterialPlacementRule
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public class MyPlanetMaterialPlacementRule : MyPlanetMaterialDefinition, ICloneable
  {
    [ProtoMember(7)]
    public SerializableRange Height = new SerializableRange(0.0f, 1f);
    [ProtoMember(8)]
    public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);
    [ProtoMember(9)]
    public SerializableRange Longitude = new SerializableRange(-180f, 180f);
    [ProtoMember(10)]
    public SerializableRange Slope = new SerializableRange(0.0f, 90f);

    public override bool IsRule => true;

    public MyPlanetMaterialPlacementRule()
    {
    }

    public MyPlanetMaterialPlacementRule(MyPlanetMaterialPlacementRule copyFrom)
    {
      this.Height = copyFrom.Height;
      this.Latitude = copyFrom.Latitude;
      this.Longitude = copyFrom.Longitude;
      this.Slope = copyFrom.Slope;
      this.Material = copyFrom.Material;
      this.Value = copyFrom.Value;
      this.MaxDepth = copyFrom.MaxDepth;
      this.Layers = copyFrom.Layers;
    }

    public bool Check(float height, float latitude, float slope) => this.Height.ValueBetween(height) && this.Latitude.ValueBetween(latitude) && this.Slope.ValueBetween(slope);

    public new object Clone() => (object) new MyPlanetMaterialPlacementRule(this);

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003EHeight\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialPlacementRule owner, in SerializableRange value) => owner.Height = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialPlacementRule owner, out SerializableRange value) => value = owner.Height;
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003ELatitude\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SymmetricSerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyPlanetMaterialPlacementRule owner,
        in SymmetricSerializableRange value)
      {
        owner.Latitude = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyPlanetMaterialPlacementRule owner,
        out SymmetricSerializableRange value)
      {
        value = owner.Latitude;
      }
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003ELongitude\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialPlacementRule owner, in SerializableRange value) => owner.Longitude = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialPlacementRule owner, out SerializableRange value) => value = owner.Longitude;
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003ESlope\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialPlacementRule owner, in SerializableRange value) => owner.Slope = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialPlacementRule owner, out SerializableRange value) => value = owner.Slope;
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003EMaterial\u003C\u003EAccessor : MyPlanetMaterialDefinition.VRage_Game_MyPlanetMaterialDefinition\u003C\u003EMaterial\u003C\u003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialPlacementRule owner, in string value) => this.Set((MyPlanetMaterialDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialPlacementRule owner, out string value) => this.Get((MyPlanetMaterialDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003EValue\u003C\u003EAccessor : MyPlanetMaterialDefinition.VRage_Game_MyPlanetMaterialDefinition\u003C\u003EValue\u003C\u003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialPlacementRule owner, in byte value) => this.Set((MyPlanetMaterialDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialPlacementRule owner, out byte value) => this.Get((MyPlanetMaterialDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003EMaxDepth\u003C\u003EAccessor : MyPlanetMaterialDefinition.VRage_Game_MyPlanetMaterialDefinition\u003C\u003EMaxDepth\u003C\u003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialPlacementRule owner, in float value) => this.Set((MyPlanetMaterialDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialPlacementRule owner, out float value) => this.Get((MyPlanetMaterialDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003ELayers\u003C\u003EAccessor : MyPlanetMaterialDefinition.VRage_Game_MyPlanetMaterialDefinition\u003C\u003ELayers\u003C\u003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, MyPlanetMaterialLayer[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyPlanetMaterialPlacementRule owner,
        in MyPlanetMaterialLayer[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyPlanetMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyPlanetMaterialPlacementRule owner,
        out MyPlanetMaterialLayer[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyPlanetMaterialDefinition&) ref owner, out value);
      }
    }

    private class VRage_Game_MyPlanetMaterialPlacementRule\u003C\u003EActor : IActivator, IActivator<MyPlanetMaterialPlacementRule>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetMaterialPlacementRule();

      MyPlanetMaterialPlacementRule IActivator<MyPlanetMaterialPlacementRule>.CreateInstance() => new MyPlanetMaterialPlacementRule();
    }
  }
}
