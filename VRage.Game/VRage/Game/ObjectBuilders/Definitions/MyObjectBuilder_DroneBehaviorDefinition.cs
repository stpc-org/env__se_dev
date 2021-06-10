// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_DroneBehaviorDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_DroneBehaviorDefinition : MyObjectBuilder_DefinitionBase
  {
    public float StrafeWidth = 10f;
    public float StrafeHeight = 10f;
    public float StrafeDepth = 5f;
    public float MinStrafeDistance = 2f;
    public bool AvoidCollisions = true;
    public bool RotateToPlayer = true;
    public bool UseStaticWeaponry = true;
    public bool UseTools = true;
    public bool UseRammingBehavior;
    public bool CanBeDisabled = true;
    public bool UsePlanetHover;
    public string AlternativeBehavior = "";
    public float PlanetHoverMin = 2f;
    public float PlanetHoverMax = 25f;
    public float SpeedLimit = 50f;
    public float PlayerYAxisOffset = 0.9f;
    public float TargetDistance = 200f;
    public float MaxManeuverDistance = 250f;
    public float StaticWeaponryUsage = 300f;
    public float RammingBehaviorDistance = 75f;
    public float ToolsUsage = 8f;
    public int WaypointDelayMsMin = 1000;
    public int WaypointDelayMsMax = 3000;
    public int WaypointMaxTime = 15000;
    public float WaypointThresholdDistance = 0.5f;
    public int LostTimeMs = 20000;
    public bool UsesWeaponBehaviors;
    public float WeaponBehaviorNotFoundDelay = 3f;
    public string SoundLoop = "";
    [XmlArrayItem("WeaponBehavior")]
    public List<MyWeaponBehavior> WeaponBehaviors;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EStrafeWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.StrafeWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.StrafeWidth;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EStrafeHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.StrafeHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.StrafeHeight;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EStrafeDepth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.StrafeDepth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.StrafeDepth;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EMinStrafeDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.MinStrafeDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.MinStrafeDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EAvoidCollisions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.AvoidCollisions = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.AvoidCollisions;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ERotateToPlayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.RotateToPlayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.RotateToPlayer;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EUseStaticWeaponry\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.UseStaticWeaponry = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.UseStaticWeaponry;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EUseTools\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.UseTools = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.UseTools;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EUseRammingBehavior\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.UseRammingBehavior = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.UseRammingBehavior;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ECanBeDisabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.CanBeDisabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.CanBeDisabled;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EUsePlanetHover\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.UsePlanetHover = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.UsePlanetHover;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EAlternativeBehavior\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => owner.AlternativeBehavior = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => value = owner.AlternativeBehavior;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EPlanetHoverMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.PlanetHoverMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.PlanetHoverMin;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EPlanetHoverMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.PlanetHoverMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.PlanetHoverMax;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ESpeedLimit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.SpeedLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.SpeedLimit;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EPlayerYAxisOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.PlayerYAxisOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.PlayerYAxisOffset;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ETargetDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.TargetDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.TargetDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EMaxManeuverDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.MaxManeuverDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.MaxManeuverDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EStaticWeaponryUsage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.StaticWeaponryUsage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.StaticWeaponryUsage;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ERammingBehaviorDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.RammingBehaviorDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.RammingBehaviorDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EToolsUsage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.ToolsUsage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.ToolsUsage;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EWaypointDelayMsMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value) => owner.WaypointDelayMsMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value) => value = owner.WaypointDelayMsMin;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EWaypointDelayMsMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value) => owner.WaypointDelayMsMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value) => value = owner.WaypointDelayMsMax;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EWaypointMaxTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value) => owner.WaypointMaxTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value) => value = owner.WaypointMaxTime;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EWaypointThresholdDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.WaypointThresholdDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.WaypointThresholdDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ELostTimeMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value) => owner.LostTimeMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value) => value = owner.LostTimeMs;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EUsesWeaponBehaviors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => owner.UsesWeaponBehaviors = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => value = owner.UsesWeaponBehaviors;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EWeaponBehaviorNotFoundDelay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value) => owner.WeaponBehaviorNotFoundDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value) => value = owner.WeaponBehaviorNotFoundDelay;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ESoundLoop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => owner.SoundLoop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => value = owner.SoundLoop;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EWeaponBehaviors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, List<MyWeaponBehavior>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        in List<MyWeaponBehavior> value)
      {
        owner.WeaponBehaviors = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        out List<MyWeaponBehavior> value)
      {
        value = owner.WeaponBehaviors;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DroneBehaviorDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DroneBehaviorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DroneBehaviorDefinition();

      MyObjectBuilder_DroneBehaviorDefinition IActivator<MyObjectBuilder_DroneBehaviorDefinition>.CreateInstance() => new MyObjectBuilder_DroneBehaviorDefinition();
    }
  }
}
