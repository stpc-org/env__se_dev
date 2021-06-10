// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ToolItemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Gui;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ToolItemDefinition : MyObjectBuilder_PhysicalItemDefinition
  {
    [XmlArrayItem("Mining")]
    [ProtoMember(79)]
    [DefaultValue(null)]
    public MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition[] VoxelMinings;
    [XmlArrayItem("Action")]
    [ProtoMember(82)]
    [DefaultValue(null)]
    public MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] PrimaryActions;
    [XmlArrayItem("Action")]
    [ProtoMember(85)]
    [DefaultValue(null)]
    public MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] SecondaryActions;
    [ProtoMember(88)]
    [DefaultValue(1)]
    public float HitDistance = 1f;

    [ProtoContract]
    public class MyVoxelMiningDefinition
    {
      [ProtoMember(1)]
      [DefaultValue(null)]
      public string MinedOre;
      [ProtoMember(4)]
      [DefaultValue(0)]
      public int HitCount;
      [ProtoMember(7)]
      [DefaultValue(null)]
      public SerializableDefinitionId PhysicalItemId;
      [ProtoMember(10)]
      [DefaultValue(0.0f)]
      public float RemovedRadius;
      [ProtoMember(13)]
      [DefaultValue(false)]
      public bool OnlyApplyMaterial;

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyVoxelMiningDefinition\u003C\u003EMinedOre\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          in string value)
        {
          owner.MinedOre = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          out string value)
        {
          value = owner.MinedOre;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyVoxelMiningDefinition\u003C\u003EHitCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          in int value)
        {
          owner.HitCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          out int value)
        {
          value = owner.HitCount;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyVoxelMiningDefinition\u003C\u003EPhysicalItemId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          in SerializableDefinitionId value)
        {
          owner.PhysicalItemId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          out SerializableDefinitionId value)
        {
          value = owner.PhysicalItemId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyVoxelMiningDefinition\u003C\u003ERemovedRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          in float value)
        {
          owner.RemovedRadius = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          out float value)
        {
          value = owner.RemovedRadius;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyVoxelMiningDefinition\u003C\u003EOnlyApplyMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          in bool value)
        {
          owner.OnlyApplyMaterial = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition owner,
          out bool value)
        {
          value = owner.OnlyApplyMaterial;
        }
      }

      private class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyVoxelMiningDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition();

        MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition IActivator<MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition>.CreateInstance() => new MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition();
      }
    }

    [ProtoContract]
    public class MyToolActionHitCondition
    {
      [ProtoMember(16)]
      [DefaultValue(null)]
      public string[] EntityType;
      [ProtoMember(19)]
      public string Animation;
      [ProtoMember(22)]
      public float AnimationTimeScale = 1f;
      [ProtoMember(25)]
      public string StatsAction;
      [ProtoMember(28)]
      public string StatsActionIfHit;
      [ProtoMember(31)]
      public string StatsModifier;
      [ProtoMember(34)]
      public string StatsModifierIfHit;
      [ProtoMember(37)]
      public string Component;

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EEntityType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string[] value)
        {
          owner.EntityType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string[] value)
        {
          value = owner.EntityType;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EAnimation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string value)
        {
          owner.Animation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string value)
        {
          value = owner.Animation;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EAnimationTimeScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in float value)
        {
          owner.AnimationTimeScale = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out float value)
        {
          value = owner.AnimationTimeScale;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EStatsAction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string value)
        {
          owner.StatsAction = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string value)
        {
          value = owner.StatsAction;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EStatsActionIfHit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string value)
        {
          owner.StatsActionIfHit = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string value)
        {
          value = owner.StatsActionIfHit;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EStatsModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string value)
        {
          owner.StatsModifier = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string value)
        {
          value = owner.StatsModifier;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EStatsModifierIfHit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string value)
        {
          owner.StatsModifierIfHit = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string value)
        {
          value = owner.StatsModifierIfHit;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EComponent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          in string value)
        {
          owner.Component = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition owner,
          out string value)
        {
          value = owner.Component;
        }
      }

      private class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionHitCondition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition();

        MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition IActivator<MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition>.CreateInstance() => new MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition();
      }
    }

    [ProtoContract]
    public class MyToolActionDefinition
    {
      [ProtoMember(40)]
      public string Name;
      [ProtoMember(43)]
      [DefaultValue(0)]
      public float StartTime;
      [ProtoMember(46)]
      [DefaultValue(1)]
      public float EndTime = 1f;
      [ProtoMember(49)]
      [DefaultValue(1f)]
      public float Efficiency = 1f;
      [ProtoMember(52)]
      [DefaultValue(null)]
      public string StatsEfficiency;
      [ProtoMember(55)]
      [DefaultValue(null)]
      public string SwingSound;
      [ProtoMember(58)]
      [DefaultValue(0.0f)]
      public float SwingSoundStart;
      [ProtoMember(61)]
      [DefaultValue(0.0f)]
      public float HitStart;
      [ProtoMember(64)]
      [DefaultValue(1f)]
      public float HitDuration = 1f;
      [ProtoMember(67)]
      [DefaultValue(null)]
      public string HitSound;
      [ProtoMember(70)]
      [DefaultValue(0.0f)]
      public float CustomShapeRadius;
      [ProtoMember(73)]
      public MyHudTexturesEnum Crosshair = MyHudTexturesEnum.HudOre;
      [XmlArrayItem("HitCondition")]
      [ProtoMember(76)]
      [DefaultValue(null)]
      public MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition[] HitConditions;

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EStartTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.StartTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.StartTime;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EEndTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.EndTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.EndTime;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EEfficiency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.Efficiency = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.Efficiency;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EStatsEfficiency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in string value)
        {
          owner.StatsEfficiency = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out string value)
        {
          value = owner.StatsEfficiency;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003ESwingSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in string value)
        {
          owner.SwingSound = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out string value)
        {
          value = owner.SwingSound;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003ESwingSoundStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.SwingSoundStart = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.SwingSoundStart;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EHitStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.HitStart = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.HitStart;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EHitDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.HitDuration = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.HitDuration;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EHitSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in string value)
        {
          owner.HitSound = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out string value)
        {
          value = owner.HitSound;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003ECustomShapeRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in float value)
        {
          owner.CustomShapeRadius = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out float value)
        {
          value = owner.CustomShapeRadius;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003ECrosshair\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, MyHudTexturesEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in MyHudTexturesEnum value)
        {
          owner.Crosshair = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out MyHudTexturesEnum value)
        {
          value = owner.Crosshair;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EHitConditions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition, MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          in MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition[] value)
        {
          owner.HitConditions = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition owner,
          out MyObjectBuilder_ToolItemDefinition.MyToolActionHitCondition[] value)
        {
          value = owner.HitConditions;
        }
      }

      private class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMyToolActionDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition();

        MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition IActivator<MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition>.CreateInstance() => new MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition();
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EVoxelMinings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolItemDefinition owner,
        in MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition[] value)
      {
        owner.VoxelMinings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolItemDefinition owner,
        out MyObjectBuilder_ToolItemDefinition.MyVoxelMiningDefinition[] value)
      {
        value = owner.VoxelMinings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EPrimaryActions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolItemDefinition owner,
        in MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] value)
      {
        owner.PrimaryActions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolItemDefinition owner,
        out MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] value)
      {
        value = owner.PrimaryActions;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ESecondaryActions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolItemDefinition owner,
        in MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] value)
      {
        owner.SecondaryActions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolItemDefinition owner,
        out MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] value)
      {
        value = owner.SecondaryActions;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EHitDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float value) => owner.HitDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float value) => value = owner.HitDistance;
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in Vector3 value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out Vector3 value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EModels\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EHealth\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EHealth\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolItemDefinition owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolItemDefinition owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyFixedPoint value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyFixedPoint value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolItemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolItemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ToolItemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ToolItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ToolItemDefinition();

      MyObjectBuilder_ToolItemDefinition IActivator<MyObjectBuilder_ToolItemDefinition>.CreateInstance() => new MyObjectBuilder_ToolItemDefinition();
    }
  }
}
