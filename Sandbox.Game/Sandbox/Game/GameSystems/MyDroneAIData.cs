// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyDroneAIData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.GameSystems
{
  public class MyDroneAIData
  {
    public string Name = "";
    public float Height = 10f;
    public float Depth = 5f;
    public float Width = 10f;
    public bool AvoidCollisions = true;
    public float SpeedLimit = 25f;
    public bool RotateToPlayer = true;
    public float PlayerYAxisOffset = 0.9f;
    public int WaypointDelayMsMin = 1000;
    public int WaypointDelayMsMax = 3000;
    public float WaypointThresholdDistance = 0.5f;
    public float PlayerTargetDistance = 200f;
    public float MaxManeuverDistance = 250f;
    public float MaxManeuverDistanceSq = 62500f;
    public int WaypointMaxTime = 15000;
    public int LostTimeMs = 20000;
    public float MinStrafeDistance = 2f;
    public float MinStrafeDistanceSq = 4f;
    public bool UseStaticWeaponry = true;
    public float StaticWeaponryUsage = 300f;
    public float StaticWeaponryUsageSq = 90000f;
    public bool UseTools = true;
    public float ToolsUsage = 5f;
    public float ToolsUsageSq = 25f;
    public bool UseKamikazeBehavior = true;
    public bool CanBeDisabled = true;
    public float KamikazeBehaviorDistance = 75f;
    public string AlternativeBehavior = "";
    public bool UsePlanetHover;
    public float PlanetHoverMin = 2f;
    public float PlanetHoverMax = 25f;
    public float RotationLimitSq;
    public bool UsesWeaponBehaviors;
    public float WeaponBehaviorNotFoundDelay = 3f;
    public List<MyWeaponBehavior> WeaponBehaviors;
    public string SoundLoop = "";

    public MyDroneAIData() => this.PostProcess();

    public MyDroneAIData(MyObjectBuilder_DroneBehaviorDefinition definition)
    {
      this.Name = definition.Id.SubtypeId;
      this.Height = definition.StrafeHeight;
      this.Depth = definition.StrafeDepth;
      this.Width = definition.StrafeWidth;
      this.AvoidCollisions = definition.AvoidCollisions;
      this.SpeedLimit = definition.SpeedLimit;
      this.RotateToPlayer = definition.RotateToPlayer;
      this.PlayerYAxisOffset = definition.PlayerYAxisOffset;
      this.WaypointDelayMsMin = definition.WaypointDelayMsMin;
      this.WaypointDelayMsMax = definition.WaypointDelayMsMax;
      this.WaypointThresholdDistance = definition.WaypointThresholdDistance;
      this.PlayerTargetDistance = definition.TargetDistance;
      this.MaxManeuverDistance = definition.MaxManeuverDistance;
      this.WaypointMaxTime = definition.WaypointMaxTime;
      this.LostTimeMs = definition.LostTimeMs;
      this.MinStrafeDistance = definition.MinStrafeDistance;
      this.UseStaticWeaponry = definition.UseStaticWeaponry;
      this.StaticWeaponryUsage = definition.StaticWeaponryUsage;
      this.UseKamikazeBehavior = definition.UseRammingBehavior;
      this.KamikazeBehaviorDistance = definition.RammingBehaviorDistance;
      this.AlternativeBehavior = definition.AlternativeBehavior;
      this.UseTools = definition.UseTools;
      this.ToolsUsage = definition.ToolsUsage;
      this.UsePlanetHover = definition.UsePlanetHover;
      this.PlanetHoverMin = definition.PlanetHoverMin;
      this.PlanetHoverMax = definition.PlanetHoverMax;
      this.UsesWeaponBehaviors = definition.UsesWeaponBehaviors && definition.WeaponBehaviors.Count > 0 && this.UseStaticWeaponry;
      this.WeaponBehaviorNotFoundDelay = definition.WeaponBehaviorNotFoundDelay;
      this.WeaponBehaviors = definition.WeaponBehaviors;
      this.SoundLoop = definition.SoundLoop;
      this.PostProcess();
    }

    private void PostProcess()
    {
      this.MaxManeuverDistanceSq = this.MaxManeuverDistance * this.MaxManeuverDistance;
      this.MinStrafeDistanceSq = this.MinStrafeDistance * this.MinStrafeDistance;
      this.ToolsUsageSq = this.ToolsUsage * this.ToolsUsage;
      this.StaticWeaponryUsageSq = this.StaticWeaponryUsage * this.StaticWeaponryUsage;
      this.RotationLimitSq = Math.Max(this.ToolsUsageSq, Math.Max(this.StaticWeaponryUsageSq, this.MaxManeuverDistanceSq));
      if (this.WeaponBehaviors == null)
        return;
      foreach (MyWeaponBehavior weaponBehavior in this.WeaponBehaviors)
      {
        for (int index = 0; index < weaponBehavior.Requirements.Count; ++index)
        {
          if (!weaponBehavior.Requirements[index].Contains("MyObjectBuilder_"))
            weaponBehavior.Requirements[index] = "MyObjectBuilder_" + weaponBehavior.Requirements[index];
        }
        foreach (MyWeaponRule weaponRule in weaponBehavior.WeaponRules)
        {
          if (!string.IsNullOrEmpty(weaponRule.Weapon) && !weaponRule.Weapon.Contains("MyObjectBuilder_"))
            weaponRule.Weapon = "MyObjectBuilder_" + weaponRule.Weapon;
        }
      }
    }
  }
}
