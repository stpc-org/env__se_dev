// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CubeBlockDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CubeBlockDefinition : MyObjectBuilder_PhysicalModelDefinition
  {
    public VoxelPlacementOverride? VoxelPlacement;
    [ProtoMember(42)]
    [DefaultValue(false)]
    public bool SilenceableByShipSoundSystem;
    [ProtoMember(43)]
    public MyCubeSize CubeSize;
    [ProtoMember(44)]
    public MyBlockTopology BlockTopology;
    [ProtoMember(45)]
    public SerializableVector3I Size;
    [ProtoMember(46)]
    public SerializableVector3 ModelOffset;
    [ProtoMember(47)]
    public MyObjectBuilder_CubeBlockDefinition.PatternDefinition CubeDefinition;
    [XmlArrayItem("Component")]
    [ProtoMember(48)]
    public MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] Components;
    [XmlArrayItem("Effect")]
    [ProtoMember(49)]
    public MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] Effects;
    [ProtoMember(50)]
    public MyObjectBuilder_CubeBlockDefinition.CriticalPart CriticalComponent;
    [ProtoMember(51)]
    public MyObjectBuilder_CubeBlockDefinition.MountPoint[] MountPoints;
    [ProtoMember(52)]
    public MyObjectBuilder_CubeBlockDefinition.Variant[] Variants;
    [XmlArrayItem("Component")]
    [ProtoMember(53)]
    public MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] EntityComponents;
    [ProtoMember(54)]
    [DefaultValue(MyPhysicsOption.Box)]
    public MyPhysicsOption PhysicsOption = MyPhysicsOption.Box;
    [XmlArrayItem("Model")]
    [ProtoMember(55)]
    [DefaultValue(null)]
    public List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> BuildProgressModels;
    [ProtoMember(56)]
    public string BlockPairName;
    [ProtoMember(57)]
    public SerializableVector3I? Center;
    [ProtoMember(58)]
    [DefaultValue(MySymmetryAxisEnum.None)]
    public MySymmetryAxisEnum MirroringX;
    [ProtoMember(59)]
    [DefaultValue(MySymmetryAxisEnum.None)]
    public MySymmetryAxisEnum MirroringY;
    [ProtoMember(60)]
    [DefaultValue(MySymmetryAxisEnum.None)]
    public MySymmetryAxisEnum MirroringZ;
    [ProtoMember(61)]
    [DefaultValue(1f)]
    public float DeformationRatio = 1f;
    [ProtoMember(62)]
    public string EdgeType;
    [ProtoMember(63)]
    [DefaultValue(10f)]
    public float BuildTimeSeconds = 10f;
    [ProtoMember(64)]
    [DefaultValue(1f)]
    public float DisassembleRatio = 1f;
    [ProtoMember(65)]
    public MyAutorotateMode AutorotateMode;
    [ProtoMember(66)]
    public string MirroringBlock;
    [ProtoMember(67)]
    public bool UseModelIntersection;
    [ProtoMember(68)]
    public string PrimarySound;
    [ProtoMember(69)]
    public string ActionSound;
    [ProtoMember(70)]
    [DefaultValue(null)]
    public string BuildType;
    [ProtoMember(71)]
    [DefaultValue(null)]
    public string BuildMaterial;
    [XmlArrayItem("Template")]
    [ProtoMember(72)]
    [DefaultValue(null)]
    public string[] CompoundTemplates;
    [ProtoMember(73)]
    [DefaultValue(true)]
    public bool CompoundEnabled = true;
    [XmlArrayItem("Definition")]
    [ProtoMember(74)]
    [DefaultValue(null)]
    public MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] SubBlockDefinitions;
    [ProtoMember(75)]
    [DefaultValue(null)]
    public string MultiBlock;
    [ProtoMember(76)]
    [DefaultValue(null)]
    public string NavigationDefinition;
    [ProtoMember(77)]
    [DefaultValue(true)]
    public bool GuiVisible = true;
    [XmlArrayItem("BlockVariant")]
    [ProtoMember(78)]
    [DefaultValue(null)]
    public SerializableDefinitionId[] BlockVariants;
    [ProtoMember(79)]
    [DefaultValue(MyBlockDirection.Both)]
    public MyBlockDirection Direction = MyBlockDirection.Both;
    [ProtoMember(80)]
    [DefaultValue(MyBlockRotation.Both)]
    public MyBlockRotation Rotation = MyBlockRotation.Both;
    [XmlArrayItem("GeneratedBlock")]
    [ProtoMember(81)]
    [DefaultValue(null)]
    public SerializableDefinitionId[] GeneratedBlocks;
    [ProtoMember(82)]
    [DefaultValue(null)]
    public string GeneratedBlockType;
    [ProtoMember(83)]
    [DefaultValue(false)]
    public bool Mirrored;
    [ProtoMember(84, IsRequired = false)]
    [DefaultValue(0)]
    public int DamageEffectId;
    [ProtoMember(85)]
    [DefaultValue("")]
    public string DestroyEffect = "";
    [ProtoMember(86)]
    [DefaultValue("PoofExplosionCat1")]
    public string DestroySound = "PoofExplosionCat1";
    [ProtoMember(87)]
    [DefaultValue(null)]
    public List<BoneInfo> Skeleton;
    [ProtoMember(88)]
    [DefaultValue(false)]
    public bool RandomRotation;
    [ProtoMember(89)]
    [DefaultValue(null)]
    public bool? IsAirTight;
    [ProtoMember(90)]
    [DefaultValue(true)]
    public bool IsStandAlone = true;
    [ProtoMember(91)]
    [DefaultValue(true)]
    public bool HasPhysics = true;
    public bool UseNeighbourOxygenRooms;
    [ProtoMember(92)]
    [DefaultValue(1)]
    [Obsolete]
    public int Points;
    [ProtoMember(93)]
    [DefaultValue(0)]
    public int MaxIntegrity;
    [ProtoMember(94)]
    [DefaultValue(1)]
    public float BuildProgressToPlaceGeneratedBlocks = 1f;
    [ProtoMember(95)]
    [DefaultValue(null)]
    public string DamagedSound;
    [ProtoMember(96)]
    [DefaultValue(true)]
    public bool CreateFracturedPieces = true;
    [ProtoMember(97)]
    [DefaultValue(null)]
    public string EmissiveColorPreset;
    [ProtoMember(98)]
    [DefaultValue(1f)]
    public float GeneralDamageMultiplier = 1f;
    [ProtoMember(99, IsRequired = false)]
    [DefaultValue("")]
    public string DamageEffectName = "";
    [ProtoMember(100, IsRequired = false)]
    [DefaultValue(true)]
    public bool UsesDeformation = true;
    [ProtoMember(101, IsRequired = false)]
    [DefaultValue(null)]
    public Vector3? DestroyEffectOffset;
    [ProtoMember(102)]
    [DefaultValue(1)]
    public int PCU = 1;
    [ProtoMember(104, IsRequired = false)]
    public int? PCUConsole;
    [ProtoMember(103, IsRequired = false)]
    [DefaultValue(true)]
    public bool PlaceDecals = true;
    [ProtoMember(105, IsRequired = false)]
    [DefaultValue(null)]
    public SerializableVector3? DepressurizationEffectOffset;
    [ProtoMember(107)]
    public MySerializableList<uint> TieredUpdateTimes = new MySerializableList<uint>();

    public bool ShouldSerializeCenter() => this.Center.HasValue;

    [ProtoContract]
    public class MountPoint
    {
      [XmlAttribute]
      [ProtoMember(3)]
      public BlockSideEnum Side;
      [XmlIgnore]
      [ProtoMember(4)]
      public SerializableVector2 Start;
      [XmlIgnore]
      [ProtoMember(5)]
      public SerializableVector2 End;
      [XmlAttribute]
      [ProtoMember(6)]
      [DefaultValue(0)]
      public byte ExclusionMask;
      [XmlAttribute]
      [ProtoMember(7)]
      [DefaultValue(0)]
      public byte PropertiesMask;
      [XmlAttribute]
      [ProtoMember(8)]
      [DefaultValue(true)]
      public bool Enabled = true;
      [XmlAttribute]
      [ProtoMember(9)]
      [DefaultValue(false)]
      public bool Default;
      [XmlAttribute]
      [ProtoMember(10)]
      [DefaultValue(false)]
      public bool PressurizedWhenOpen = true;

      [XmlAttribute]
      public float StartX
      {
        get => this.Start.X;
        set => this.Start.X = value;
      }

      [XmlAttribute]
      public float StartY
      {
        get => this.Start.Y;
        set => this.Start.Y = value;
      }

      [XmlAttribute]
      public float EndX
      {
        get => this.End.X;
        set => this.End.X = value;
      }

      [XmlAttribute]
      public float EndY
      {
        get => this.End.Y;
        set => this.End.Y = value;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003ESide\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, BlockSideEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in BlockSideEnum value)
        {
          owner.Side = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out BlockSideEnum value)
        {
          value = owner.Side;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, SerializableVector2>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in SerializableVector2 value)
        {
          owner.Start = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out SerializableVector2 value)
        {
          value = owner.Start;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EEnd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, SerializableVector2>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in SerializableVector2 value)
        {
          owner.End = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out SerializableVector2 value)
        {
          value = owner.End;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EExclusionMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, byte>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in byte value)
        {
          owner.ExclusionMask = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out byte value)
        {
          value = owner.ExclusionMask;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EPropertiesMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, byte>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in byte value)
        {
          owner.PropertiesMask = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out byte value)
        {
          value = owner.PropertiesMask;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in bool value)
        {
          owner.Enabled = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out bool value)
        {
          value = owner.Enabled;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EDefault\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in bool value)
        {
          owner.Default = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out bool value)
        {
          value = owner.Default;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EPressurizedWhenOpen\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in bool value)
        {
          owner.PressurizedWhenOpen = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out bool value)
        {
          value = owner.PressurizedWhenOpen;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EStartX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in float value)
        {
          owner.StartX = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out float value)
        {
          value = owner.StartX;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EStartY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in float value)
        {
          owner.StartY = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out float value)
        {
          value = owner.StartY;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EEndX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in float value)
        {
          owner.EndX = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out float value)
        {
          value = owner.EndX;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EEndY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MountPoint, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          in float value)
        {
          owner.EndY = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MountPoint owner,
          out float value)
        {
          value = owner.EndY;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoint\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.MountPoint>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.MountPoint();

        MyObjectBuilder_CubeBlockDefinition.MountPoint IActivator<MyObjectBuilder_CubeBlockDefinition.MountPoint>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.MountPoint();
      }
    }

    [ProtoContract]
    public class CubeBlockComponent
    {
      [XmlIgnore]
      public MyObjectBuilderType Type = (MyObjectBuilderType) typeof (MyObjectBuilder_Component);
      [XmlAttribute]
      [ProtoMember(10)]
      public string Subtype;
      [XmlAttribute]
      [ProtoMember(11)]
      public ushort Count;
      [ProtoMember(12)]
      public SerializableDefinitionId DeconstructId;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockComponent\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent, MyObjectBuilderType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          in MyObjectBuilderType value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          out MyObjectBuilderType value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockComponent\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          in string value)
        {
          owner.Subtype = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          out string value)
        {
          value = owner.Subtype;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockComponent\u003C\u003ECount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent, ushort>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          in ushort value)
        {
          owner.Count = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          out ushort value)
        {
          value = owner.Count;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockComponent\u003C\u003EDeconstructId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          in SerializableDefinitionId value)
        {
          owner.DeconstructId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent owner,
          out SerializableDefinitionId value)
        {
          value = owner.DeconstructId;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent();

        MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent IActivator<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent();
      }
    }

    [ProtoContract]
    public class CriticalPart
    {
      [XmlIgnore]
      public MyObjectBuilderType Type = (MyObjectBuilderType) typeof (MyObjectBuilder_Component);
      [XmlAttribute]
      [ProtoMember(13)]
      public string Subtype;
      [XmlAttribute]
      [ProtoMember(14)]
      public int Index;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalPart\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CriticalPart, MyObjectBuilderType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CriticalPart owner,
          in MyObjectBuilderType value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CriticalPart owner,
          out MyObjectBuilderType value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalPart\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CriticalPart, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CriticalPart owner,
          in string value)
        {
          owner.Subtype = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CriticalPart owner,
          out string value)
        {
          value = owner.Subtype;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalPart\u003C\u003EIndex\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CriticalPart, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CriticalPart owner,
          in int value)
        {
          owner.Index = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CriticalPart owner,
          out int value)
        {
          value = owner.Index;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalPart\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.CriticalPart>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.CriticalPart();

        MyObjectBuilder_CubeBlockDefinition.CriticalPart IActivator<MyObjectBuilder_CubeBlockDefinition.CriticalPart>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.CriticalPart();
      }
    }

    [ProtoContract]
    public class Variant
    {
      [XmlAttribute]
      [ProtoMember(15)]
      public string Color;
      [XmlAttribute]
      [ProtoMember(16)]
      public string Suffix;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariant\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Variant, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.Variant owner,
          in string value)
        {
          owner.Color = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.Variant owner,
          out string value)
        {
          value = owner.Color;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariant\u003C\u003ESuffix\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Variant, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.Variant owner,
          in string value)
        {
          owner.Suffix = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.Variant owner,
          out string value)
        {
          value = owner.Suffix;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariant\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.Variant>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.Variant();

        MyObjectBuilder_CubeBlockDefinition.Variant IActivator<MyObjectBuilder_CubeBlockDefinition.Variant>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.Variant();
      }
    }

    [ProtoContract]
    public class PatternDefinition
    {
      [ProtoMember(17)]
      public MyCubeTopology CubeTopology;
      [ProtoMember(18)]
      public MyObjectBuilder_CubeBlockDefinition.Side[] Sides;
      [ProtoMember(19)]
      public bool ShowEdges;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPatternDefinition\u003C\u003ECubeTopology\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.PatternDefinition, MyCubeTopology>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.PatternDefinition owner,
          in MyCubeTopology value)
        {
          owner.CubeTopology = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.PatternDefinition owner,
          out MyCubeTopology value)
        {
          value = owner.CubeTopology;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPatternDefinition\u003C\u003ESides\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.PatternDefinition, MyObjectBuilder_CubeBlockDefinition.Side[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.PatternDefinition owner,
          in MyObjectBuilder_CubeBlockDefinition.Side[] value)
        {
          owner.Sides = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.PatternDefinition owner,
          out MyObjectBuilder_CubeBlockDefinition.Side[] value)
        {
          value = owner.Sides;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPatternDefinition\u003C\u003EShowEdges\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.PatternDefinition, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.PatternDefinition owner,
          in bool value)
        {
          owner.ShowEdges = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.PatternDefinition owner,
          out bool value)
        {
          value = owner.ShowEdges;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPatternDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.PatternDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.PatternDefinition();

        MyObjectBuilder_CubeBlockDefinition.PatternDefinition IActivator<MyObjectBuilder_CubeBlockDefinition.PatternDefinition>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.PatternDefinition();
      }
    }

    [ProtoContract]
    public class Side
    {
      [XmlAttribute]
      [ProtoMember(20)]
      [ModdableContentFile("mwm")]
      public string Model;
      [XmlIgnore]
      [ProtoMember(21)]
      public SerializableVector2I PatternSize;
      [XmlAttribute]
      public int ScaleTileU = 1;
      [XmlAttribute]
      public int ScaleTileV = 1;

      [XmlAttribute]
      public int PatternWidth
      {
        get => this.PatternSize.X;
        set => this.PatternSize.X = value;
      }

      [XmlAttribute]
      public int PatternHeight
      {
        get => this.PatternSize.Y;
        set => this.PatternSize.Y = value;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Side, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition.Side owner, in string value) => owner.Model = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.Side owner,
          out string value)
        {
          value = owner.Model;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EPatternSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Side, SerializableVector2I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.Side owner,
          in SerializableVector2I value)
        {
          owner.PatternSize = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.Side owner,
          out SerializableVector2I value)
        {
          value = owner.PatternSize;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EScaleTileU\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Side, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition.Side owner, in int value) => owner.ScaleTileU = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition.Side owner, out int value) => value = owner.ScaleTileU;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EScaleTileV\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Side, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition.Side owner, in int value) => owner.ScaleTileV = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition.Side owner, out int value) => value = owner.ScaleTileV;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EPatternWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Side, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition.Side owner, in int value) => owner.PatternWidth = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition.Side owner, out int value) => value = owner.PatternWidth;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EPatternHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.Side, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition.Side owner, in int value) => owner.PatternHeight = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition.Side owner, out int value) => value = owner.PatternHeight;
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESide\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.Side>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.Side();

        MyObjectBuilder_CubeBlockDefinition.Side IActivator<MyObjectBuilder_CubeBlockDefinition.Side>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.Side();
      }
    }

    [ProtoContract]
    public class BuildProgressModel
    {
      [XmlAttribute]
      [ProtoMember(22)]
      public float BuildPercentUpperBound;
      [XmlAttribute]
      [ProtoMember(23)]
      [ModdableContentFile("mwm")]
      public string File;
      [XmlAttribute]
      [ProtoMember(24)]
      [DefaultValue(false)]
      public bool RandomOrientation;
      [ProtoMember(25)]
      [XmlArray("MountPointOverrides")]
      [XmlArrayItem("MountPoint")]
      [DefaultValue(null)]
      public MyObjectBuilder_CubeBlockDefinition.MountPoint[] MountPoints;
      [XmlAttribute]
      [ProtoMember(26)]
      [DefaultValue(true)]
      public bool Visible = true;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModel\u003C\u003EBuildPercentUpperBound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          in float value)
        {
          owner.BuildPercentUpperBound = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          out float value)
        {
          value = owner.BuildPercentUpperBound;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModel\u003C\u003EFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          in string value)
        {
          owner.File = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          out string value)
        {
          value = owner.File;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModel\u003C\u003ERandomOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          in bool value)
        {
          owner.RandomOrientation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          out bool value)
        {
          value = owner.RandomOrientation;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModel\u003C\u003EMountPoints\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel, MyObjectBuilder_CubeBlockDefinition.MountPoint[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          in MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
        {
          owner.MountPoints = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          out MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
        {
          value = owner.MountPoints;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModel\u003C\u003EVisible\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          in bool value)
        {
          owner.Visible = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.BuildProgressModel owner,
          out bool value)
        {
          value = owner.Visible;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModel\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.BuildProgressModel();

        MyObjectBuilder_CubeBlockDefinition.BuildProgressModel IActivator<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.BuildProgressModel();
      }
    }

    [ProtoContract]
    public class MySubBlockDefinition
    {
      [XmlAttribute]
      [ProtoMember(27)]
      public string SubBlock;
      [ProtoMember(28)]
      public SerializableDefinitionId Id;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMySubBlockDefinition\u003C\u003ESubBlock\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition owner,
          in string value)
        {
          owner.SubBlock = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition owner,
          out string value)
        {
          value = owner.SubBlock;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMySubBlockDefinition\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition owner,
          in SerializableDefinitionId value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition owner,
          out SerializableDefinitionId value)
        {
          value = owner.Id;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMySubBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition();

        MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition IActivator<MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition();
      }
    }

    [ProtoContract]
    public class EntityComponentDefinition
    {
      [XmlAttribute]
      [ProtoMember(29)]
      public string ComponentType;
      [XmlAttribute]
      [ProtoMember(30)]
      public string BuilderType;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponentDefinition\u003C\u003EComponentType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition owner,
          in string value)
        {
          owner.ComponentType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition owner,
          out string value)
        {
          value = owner.ComponentType;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponentDefinition\u003C\u003EBuilderType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition owner,
          in string value)
        {
          owner.BuilderType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition owner,
          out string value)
        {
          value = owner.BuilderType;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition();

        MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition IActivator<MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition();
      }
    }

    [ProtoContract]
    public class CubeBlockEffectBase
    {
      [XmlAttribute]
      [ProtoMember(31)]
      public string Name = "";
      [XmlAttribute]
      [ProtoMember(32)]
      public float ParameterMin = float.MinValue;
      [XmlAttribute]
      [ProtoMember(33)]
      public float ParameterMax = float.MaxValue;
      [XmlArrayItem("ParticleEffect")]
      [ProtoMember(34)]
      public MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect[] ParticleEffects;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffectBase\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffectBase\u003C\u003EParameterMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          in float value)
        {
          owner.ParameterMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          out float value)
        {
          value = owner.ParameterMin;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffectBase\u003C\u003EParameterMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          in float value)
        {
          owner.ParameterMax = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          out float value)
        {
          value = owner.ParameterMax;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffectBase\u003C\u003EParticleEffects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase, MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          in MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect[] value)
        {
          owner.ParticleEffects = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase owner,
          out MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect[] value)
        {
          value = owner.ParticleEffects;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffectBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase();

        MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase IActivator<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase();
      }
    }

    [ProtoContract]
    public class CubeBlockEffect
    {
      [XmlAttribute]
      [ProtoMember(35)]
      public string Name = "";
      [XmlAttribute]
      [ProtoMember(36)]
      public string Origin = "";
      [XmlAttribute]
      [ProtoMember(37)]
      public float Delay;
      [XmlAttribute]
      [ProtoMember(38)]
      public float Duration;
      [XmlAttribute]
      [ProtoMember(39)]
      public bool Loop;
      [XmlAttribute]
      [ProtoMember(40)]
      public float SpawnTimeMin;
      [XmlAttribute]
      [ProtoMember(41)]
      public float SpawnTimeMax;

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003EOrigin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in string value)
        {
          owner.Origin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out string value)
        {
          value = owner.Origin;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003EDelay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in float value)
        {
          owner.Delay = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out float value)
        {
          value = owner.Delay;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003EDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in float value)
        {
          owner.Duration = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out float value)
        {
          value = owner.Duration;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003ELoop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in bool value)
        {
          owner.Loop = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out bool value)
        {
          value = owner.Loop;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003ESpawnTimeMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in float value)
        {
          owner.SpawnTimeMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out float value)
        {
          value = owner.SpawnTimeMin;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003ESpawnTimeMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          in float value)
        {
          owner.SpawnTimeMax = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect owner,
          out float value)
        {
          value = owner.SpawnTimeMax;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeBlockEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect();

        MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect IActivator<MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect();
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, VoxelPlacementOverride?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in VoxelPlacementOverride? value)
      {
        owner.VoxelPlacement = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out VoxelPlacementOverride? value)
      {
        value = owner.VoxelPlacement;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.SilenceableByShipSoundSystem = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.SilenceableByShipSoundSystem;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyCubeSize>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyCubeSize value) => owner.CubeSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyCubeSize value) => value = owner.CubeSize;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyBlockTopology>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyBlockTopology value)
      {
        owner.BlockTopology = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyBlockTopology value)
      {
        value = owner.BlockTopology;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableVector3I value)
      {
        owner.Size = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableVector3I value)
      {
        value = owner.Size;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EModelOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableVector3 value)
      {
        owner.ModelOffset = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableVector3 value)
      {
        value = owner.ModelOffset;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.PatternDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        owner.CubeDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        value = owner.CubeDefinition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        owner.Components = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        value = owner.Components;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEffects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        owner.Effects = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        value = owner.Effects;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.CriticalPart>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        owner.CriticalComponent = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        value = owner.CriticalComponent;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoints\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.MountPoint[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        owner.MountPoints = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        value = owner.MountPoints;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariants\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.Variant[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        owner.Variants = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        value = owner.Variants;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        owner.EntityComponents = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        value = owner.EntityComponents;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyPhysicsOption>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyPhysicsOption value)
      {
        owner.PhysicsOption = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyPhysicsOption value)
      {
        value = owner.PhysicsOption;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        owner.BuildProgressModels = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        value = owner.BuildProgressModels;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.BlockPairName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.BlockPairName;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableVector3I? value)
      {
        owner.Center = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableVector3I? value)
      {
        value = owner.Center;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MySymmetryAxisEnum value)
      {
        owner.MirroringX = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MySymmetryAxisEnum value)
      {
        value = owner.MirroringX;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MySymmetryAxisEnum value)
      {
        owner.MirroringY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MySymmetryAxisEnum value)
      {
        value = owner.MirroringY;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MySymmetryAxisEnum value)
      {
        owner.MirroringZ = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MySymmetryAxisEnum value)
      {
        value = owner.MirroringZ;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value) => owner.DeformationRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value) => value = owner.DeformationRatio;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEdgeType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.EdgeType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.EdgeType;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value) => owner.BuildTimeSeconds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value) => value = owner.BuildTimeSeconds;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value) => owner.DisassembleRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value) => value = owner.DisassembleRatio;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyAutorotateMode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyAutorotateMode value)
      {
        owner.AutorotateMode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyAutorotateMode value)
      {
        value = owner.AutorotateMode;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.MirroringBlock = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.MirroringBlock;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.UseModelIntersection = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.UseModelIntersection;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.PrimarySound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.PrimarySound;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EActionSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.ActionSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.ActionSound;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.BuildType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.BuildType;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.BuildMaterial = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.BuildMaterial;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string[] value) => owner.CompoundTemplates = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string[] value) => value = owner.CompoundTemplates;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.CompoundEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.CompoundEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        owner.SubBlockDefinitions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        value = owner.SubBlockDefinitions;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.MultiBlock = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.MultiBlock;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.NavigationDefinition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.NavigationDefinition;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.GuiVisible = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.GuiVisible;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableDefinitionId[] value)
      {
        owner.BlockVariants = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableDefinitionId[] value)
      {
        value = owner.BlockVariants;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDirection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyBlockDirection>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyBlockDirection value)
      {
        owner.Direction = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyBlockDirection value)
      {
        value = owner.Direction;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERotation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyBlockRotation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MyBlockRotation value)
      {
        owner.Rotation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MyBlockRotation value)
      {
        value = owner.Rotation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableDefinitionId[] value)
      {
        owner.GeneratedBlocks = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableDefinitionId[] value)
      {
        value = owner.GeneratedBlocks;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.GeneratedBlockType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.GeneratedBlockType;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirrored\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.Mirrored = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.Mirrored;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value) => owner.DamageEffectId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value) => value = owner.DamageEffectId;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.DestroyEffect = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.DestroyEffect;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroySound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.DestroySound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.DestroySound;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESkeleton\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, List<BoneInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in List<BoneInfo> value)
      {
        owner.Skeleton = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out List<BoneInfo> value)
      {
        value = owner.Skeleton;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.RandomRotation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.RandomRotation;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool? value) => owner.IsAirTight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool? value) => value = owner.IsAirTight;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.IsStandAlone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.IsStandAlone;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.HasPhysics = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.HasPhysics;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.UseNeighbourOxygenRooms = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.UseNeighbourOxygenRooms;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPoints\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value) => owner.Points = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value) => value = owner.Points;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value) => owner.MaxIntegrity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value) => value = owner.MaxIntegrity;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value) => owner.BuildProgressToPlaceGeneratedBlocks = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value) => value = owner.BuildProgressToPlaceGeneratedBlocks;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.DamagedSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.DamagedSound;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.CreateFracturedPieces = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.CreateFracturedPieces;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.EmissiveColorPreset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.EmissiveColorPreset;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value) => owner.GeneralDamageMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value) => value = owner.GeneralDamageMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => owner.DamageEffectName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => value = owner.DamageEffectName;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.UsesDeformation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.UsesDeformation;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, Vector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in Vector3? value) => owner.DestroyEffectOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out Vector3? value) => value = owner.DestroyEffectOffset;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCU\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value) => owner.PCU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value) => value = owner.PCU;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int? value) => owner.PCUConsole = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int? value) => value = owner.PCUConsole;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => owner.PlaceDecals = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => value = owner.PlaceDecals;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableVector3? value)
      {
        owner.DepressurizationEffectOffset = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableVector3? value)
      {
        value = owner.DepressurizationEffectOffset;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySerializableList<uint>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in MySerializableList<uint> value)
      {
        owner.TieredUpdateTimes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out MySerializableList<uint> value)
      {
        value = owner.TieredUpdateTimes;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlockDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlockDefinition();

      MyObjectBuilder_CubeBlockDefinition IActivator<MyObjectBuilder_CubeBlockDefinition>.CreateInstance() => new MyObjectBuilder_CubeBlockDefinition();
    }
  }
}
