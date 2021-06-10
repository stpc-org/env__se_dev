// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AmmoMagazineDefinition
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
  public class MyObjectBuilder_AmmoMagazineDefinition : MyObjectBuilder_PhysicalItemDefinition
  {
    [ProtoMember(4)]
    public int Capacity;
    [ProtoMember(7)]
    public MyAmmoCategoryEnum Category;
    [ProtoMember(10)]
    public MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition AmmoDefinitionId;

    [ProtoContract]
    public class AmmoDefinition
    {
      [XmlIgnore]
      public MyObjectBuilderType Type = (MyObjectBuilderType) typeof (MyObjectBuilder_AmmoDefinition);
      [XmlAttribute]
      [ProtoMember(1)]
      public string Subtype;

      protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EAmmoDefinition\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition, MyObjectBuilderType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition owner,
          in MyObjectBuilderType value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition owner,
          out MyObjectBuilderType value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EAmmoDefinition\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition owner,
          in string value)
        {
          owner.Subtype = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition owner,
          out string value)
        {
          value = owner.Subtype;
        }
      }

      private class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EAmmoDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition();

        MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition IActivator<MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition>.CreateInstance() => new MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition();
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ECapacity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => owner.Capacity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => value = owner.Capacity;
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ECategory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyAmmoCategoryEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in MyAmmoCategoryEnum value)
      {
        owner.Category = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out MyAmmoCategoryEnum value)
      {
        value = owner.Category;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EAmmoDefinitionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition value)
      {
        owner.AmmoDefinitionId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out MyObjectBuilder_AmmoMagazineDefinition.AmmoDefinition value)
      {
        value = owner.AmmoDefinitionId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in Vector3 value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out Vector3 value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EModels\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string[] value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string[] value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EModelVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EHealth\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EHealth\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in MyFixedPoint value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out MyFixedPoint value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoMagazineDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazineDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazineDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazineDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AmmoMagazineDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AmmoMagazineDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AmmoMagazineDefinition();

      MyObjectBuilder_AmmoMagazineDefinition IActivator<MyObjectBuilder_AmmoMagazineDefinition>.CreateInstance() => new MyObjectBuilder_AmmoMagazineDefinition();
    }
  }
}
