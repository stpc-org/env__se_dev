// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WeaponItemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WeaponItemDefinition : MyObjectBuilder_PhysicalItemDefinition
  {
    [ProtoMember(4)]
    public MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId WeaponDefinitionId;
    [ProtoMember(7)]
    public bool ShowAmmoCount;

    [ProtoContract]
    public class PhysicalItemWeaponDefinitionId
    {
      [XmlIgnore]
      public MyObjectBuilderType Type = (MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition);
      [XmlAttribute]
      [ProtoMember(1)]
      public string Subtype;

      protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EPhysicalItemWeaponDefinitionId\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId, MyObjectBuilderType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId owner,
          in MyObjectBuilderType value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId owner,
          out MyObjectBuilderType value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EPhysicalItemWeaponDefinitionId\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId owner,
          in string value)
        {
          owner.Subtype = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId owner,
          out string value)
        {
          value = owner.Subtype;
        }
      }

      private class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EPhysicalItemWeaponDefinitionId\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId();

        MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId IActivator<MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId>.CreateInstance() => new MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId();
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EWeaponDefinitionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        in MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId value)
      {
        owner.WeaponDefinitionId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        out MyObjectBuilder_WeaponItemDefinition.PhysicalItemWeaponDefinitionId value)
      {
        value = owner.WeaponDefinitionId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EShowAmmoCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => owner.ShowAmmoCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => value = owner.ShowAmmoCount;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in Vector3 value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out Vector3 value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EModels\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EHealth\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EHealth\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in MyFixedPoint value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        out MyFixedPoint value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponItemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_WeaponItemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeaponItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeaponItemDefinition();

      MyObjectBuilder_WeaponItemDefinition IActivator<MyObjectBuilder_WeaponItemDefinition>.CreateInstance() => new MyObjectBuilder_WeaponItemDefinition();
    }
  }
}
