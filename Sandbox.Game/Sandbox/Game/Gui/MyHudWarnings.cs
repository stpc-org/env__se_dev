// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudWarnings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  internal class MyHudWarnings : MySessionComponentBase
  {
    public static readonly int FRAMES_BETWEEN_UPDATE = 30;
    public static bool EnableWarnings = true;
    private static List<MyHudWarningGroup> m_hudWarnings = new List<MyHudWarningGroup>();
    private static List<MyGuiSounds> m_soundQueue = new List<MyGuiSounds>();
    private static IMySourceVoice m_sound;
    private static int m_lastSoundPlayed = 0;
    private int m_updateCounter;

    public static void EnqueueSound(MyGuiSounds sound)
    {
      if (!MyGuiAudio.HudWarnings)
        return;
      if ((MyHudWarnings.m_sound == null || !MyHudWarnings.m_sound.IsPlaying) && MySandboxGame.TotalGamePlayTimeInMilliseconds - MyHudWarnings.m_lastSoundPlayed > 5000)
      {
        MyHudWarnings.m_sound = MyGuiAudio.PlaySound(sound);
        MyHudWarnings.m_lastSoundPlayed = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
      else
        MyHudWarnings.m_soundQueue.Add(sound);
    }

    public static void RemoveSound(MyGuiSounds cueEnum)
    {
      if (MyHudWarnings.m_sound != null && MyHudWarnings.m_sound.CueEnum == MyGuiAudio.GetCue(cueEnum) && !MyHudWarnings.m_sound.IsPlaying)
      {
        MyHudWarnings.m_sound.Stop();
        MyHudWarnings.m_sound = (IMySourceVoice) null;
      }
      MyHudWarnings.m_soundQueue.RemoveAll((Predicate<MyGuiSounds>) (cue => cue == cueEnum));
    }

    public static void Add(MyHudWarningGroup hudWarningGroup) => MyHudWarnings.m_hudWarnings.Add(hudWarningGroup);

    public static void Remove(MyHudWarningGroup hudWarningGroup) => MyHudWarnings.m_hudWarnings.Remove(hudWarningGroup);

    public override void LoadData()
    {
      base.LoadData();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      List<MyHudWarning> hudWarnings = new List<MyHudWarning>();
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.HealthLowWarningMethod), 1, 300000, disappearTime: 2500));
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.HealthCritWarningMethod), 0, 300000, disappearTime: 5000));
      MyHudWarnings.Add(new MyHudWarningGroup(hudWarnings, false));
      hudWarnings.Clear();
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.FuelLowWarningMethod), 1, 300000, disappearTime: 2500));
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.FuelCritWarningMethod), 0, 300000, disappearTime: 5000));
      MyHudWarnings.Add(new MyHudWarningGroup(hudWarnings, false));
      hudWarnings.Clear();
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.EnergyLowWarningMethod), 2, 300000, disappearTime: 2500));
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.EnergyCritWarningMethod), 1, 300000, disappearTime: 5000));
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.EnergyNoWarningMethod), 0, 300000, disappearTime: 5000));
      MyHudWarnings.Add(new MyHudWarningGroup(hudWarnings, false));
      hudWarnings.Clear();
      hudWarnings.Add(new MyHudWarning(new MyWarningDetectionMethod(MyHudWarnings.MeteorInboundWarningMethod), 0, 600000, disappearTime: 5000));
      MyHudWarnings.Add(new MyHudWarningGroup(hudWarnings, false));
    }

    private static bool HealthWarningMethod(float treshold) => MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.StatComp != null && (double) MySession.Static.LocalCharacter.StatComp.HealthRatio < (double) treshold && !MySession.Static.LocalCharacter.IsDead;

    private static bool IsEnergyUnderTreshold(float treshold)
    {
      if (MySession.Static.CreativeMode || MySession.Static.ControlledEntity == null)
        return false;
      if (MySession.Static.ControlledEntity.Entity is MyCharacter || MySession.Static.ControlledEntity == null)
      {
        MyCharacter localCharacter = MySession.Static.LocalCharacter;
        return localCharacter != null && (double) localCharacter.SuitBattery.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) <= 0.0 && (double) localCharacter.SuitBattery.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId) / 9.99999974737875E-06 <= (double) treshold && !localCharacter.IsDead;
      }
      if (!(MySession.Static.ControlledEntity.Entity is MyCockpit))
        return false;
      MyCubeGrid cubeGrid = (MySession.Static.ControlledEntity.Entity as MyCockpit).CubeGrid;
      if (cubeGrid.GridSystems == null || cubeGrid.GridSystems.ResourceDistributor == null)
        return false;
      MyMultipleEnabledEnum multipleEnabledEnum = cubeGrid.GridSystems.ResourceDistributor.SourcesEnabledByType(MyResourceDistributorComponent.ElectricityId);
      return (double) MyHud.ShipInfo.FuelRemainingTime <= (double) treshold && multipleEnabledEnum != MyMultipleEnabledEnum.AllDisabled && (uint) multipleEnabledEnum > 0U;
    }

    private static bool IsFuelUnderThreshold(float treshold)
    {
      if (MySession.Static.CreativeMode || MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity.Entity is MyCharacter))
        return false;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      return localCharacter != null && (double) localCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) < (double) treshold;
    }

    private static bool HealthLowWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (!MyHudWarnings.HealthWarningMethod(MyCharacterStatComponent.HEALTH_RATIO_LOW))
        return false;
      cue = MyGuiSounds.HudVocHealthLow;
      text = MySpaceTexts.NotificationHealthLow;
      return true;
    }

    private static bool HealthCritWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (!MyHudWarnings.HealthWarningMethod(MyCharacterStatComponent.HEALTH_RATIO_CRITICAL))
        return false;
      cue = MyGuiSounds.HudVocHealthCritical;
      text = MySpaceTexts.NotificationHealthCritical;
      return true;
    }

    private static bool MeteorInboundWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.HudVocMeteorInbound;
      text = MySpaceTexts.NotificationMeteorInbound;
      if (!MyMeteorShower.CurrentTarget.HasValue || MySession.Static.ControlledEntity == null)
        return false;
      BoundingSphereD? currentTarget = MyMeteorShower.CurrentTarget;
      double num1 = (double) Vector3.Distance((Vector3) currentTarget.Value.Center, (Vector3) MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition());
      currentTarget = MyMeteorShower.CurrentTarget;
      double num2 = 2.0 * currentTarget.Value.Radius + 500.0;
      return num1 < num2;
    }

    private static bool EnergyLowWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (MySession.Static.ControlledEntity == null)
        return false;
      if (MySession.Static.ControlledEntity.Entity is MyCharacter)
      {
        if (!MyHudWarnings.IsEnergyUnderTreshold(0.25f))
          return false;
        cue = MyGuiSounds.HudVocEnergyLow;
        text = MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.OxygenComponent == null || (!MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit || !MySession.Static.Settings.EnableOxygen) ? MySpaceTexts.NotificationSuitEnergyLow : MySpaceTexts.NotificationSuitEnergyLowNoDamage;
      }
      else
      {
        if (!(MySession.Static.ControlledEntity.Entity is MyCockpit) || !MyHudWarnings.IsEnergyUnderTreshold(0.125f))
          return false;
        MyCockpit entity = (MyCockpit) MySession.Static.ControlledEntity.Entity;
        bool flag = false;
        List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.Logical.GetGroupNodes(entity.CubeGrid);
        if (groupNodes == null || groupNodes.Count == 0)
          return false;
        foreach (MyCubeGrid myCubeGrid in groupNodes)
        {
          if (myCubeGrid.NumberOfReactors > 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
        cue = !entity.CubeGrid.IsStatic ? MyGuiSounds.HudVocShipFuelLow : MyGuiSounds.HudVocStationFuelLow;
        text = MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.OxygenComponent == null || (!MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit || !MySession.Static.Settings.EnableOxygen) ? MySpaceTexts.NotificationShipEnergyLow : MySpaceTexts.NotificationShipEnergyLowNoDamage;
      }
      return true;
    }

    private static bool EnergyCritWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (MySession.Static.ControlledEntity == null)
        return false;
      if (MySession.Static.ControlledEntity.Entity is MyCharacter)
      {
        if (!MyHudWarnings.IsEnergyUnderTreshold(0.1f))
          return false;
        cue = MyGuiSounds.HudVocEnergyCrit;
        text = MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.OxygenComponent == null || (!MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit || !MySession.Static.Settings.EnableOxygen) ? MySpaceTexts.NotificationSuitEnergyCritical : MySpaceTexts.NotificationSuitEnergyCriticalNoDamage;
      }
      else
      {
        if (!(MySession.Static.ControlledEntity.Entity is MyCockpit) || !MyHudWarnings.IsEnergyUnderTreshold(0.05f))
          return false;
        MyCockpit entity = (MyCockpit) MySession.Static.ControlledEntity.Entity;
        bool flag = false;
        List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.Logical.GetGroupNodes(entity.CubeGrid);
        if (groupNodes == null || groupNodes.Count == 0)
          return false;
        foreach (MyCubeGrid myCubeGrid in groupNodes)
        {
          if (myCubeGrid.NumberOfReactors > 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
        cue = !entity.CubeGrid.IsStatic ? MyGuiSounds.HudVocShipFuelCrit : MyGuiSounds.HudVocStationFuelCrit;
        text = MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.OxygenComponent == null || (!MySession.Static.LocalCharacter.OxygenComponent.NeedsOxygenFromSuit || !MySession.Static.Settings.EnableOxygen) ? MySpaceTexts.NotificationShipEnergyCritical : MySpaceTexts.NotificationShipEnergyCriticalNoDamage;
      }
      return true;
    }

    private static bool FuelLowWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity.Entity is MyCharacter entity) || (entity.JetpackComp == null || !MyHudWarnings.IsFuelUnderThreshold(0.1f)))
        return false;
      cue = MyGuiSounds.HudVocFuelLow;
      text = MySpaceTexts.NotificationSuitFuelLow;
      return true;
    }

    private static bool FuelCritWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity.Entity is MyCharacter entity) || (entity.JetpackComp == null || !MyHudWarnings.IsFuelUnderThreshold(0.05f)))
        return false;
      cue = MyGuiSounds.HudVocFuelCrit;
      text = MySpaceTexts.NotificationSuitFuelCritical;
      return true;
    }

    private static bool EnergyNoWarningMethod(out MyGuiSounds cue, out MyStringId text)
    {
      cue = MyGuiSounds.None;
      text = MySpaceTexts.Blank;
      if (!MyHudWarnings.IsEnergyUnderTreshold(0.0f))
        return false;
      if (MySession.Static.ControlledEntity.Entity is MyCharacter)
      {
        cue = MyGuiSounds.HudVocEnergyNo;
        text = MySpaceTexts.NotificationEnergyNo;
      }
      else
      {
        if (!(MySession.Static.ControlledEntity.Entity is MyCockpit))
          return false;
        MyCockpit entity = (MyCockpit) MySession.Static.ControlledEntity.Entity;
        bool flag = false;
        List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.Logical.GetGroupNodes(entity.CubeGrid);
        if (groupNodes == null || groupNodes.Count == 0)
          return false;
        foreach (MyCubeGrid myCubeGrid in groupNodes)
        {
          if (myCubeGrid.NumberOfReactors > 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
        cue = !entity.CubeGrid.IsStatic ? MyGuiSounds.HudVocShipFuelNo : MyGuiSounds.HudVocStationFuelNo;
        text = MySpaceTexts.NotificationFuelNo;
      }
      return true;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      foreach (MyHudWarningGroup hudWarning in MyHudWarnings.m_hudWarnings)
        hudWarning.Clear();
      MyHudWarnings.m_hudWarnings.Clear();
      MyHudWarnings.m_soundQueue.Clear();
      if (MyHudWarnings.m_sound == null)
        return;
      MyHudWarnings.m_sound.Stop(true);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MyHudWarnings.m_hudWarnings.Clear();
        MyHudWarnings.m_soundQueue.Clear();
      }
      else
      {
        ++this.m_updateCounter;
        if (this.m_updateCounter % MyHudWarnings.FRAMES_BETWEEN_UPDATE != 0)
          return;
        foreach (MyHudWarningGroup hudWarning in MyHudWarnings.m_hudWarnings)
          hudWarning.Update();
        if (MyHudWarnings.m_soundQueue.Count <= 0 || MySandboxGame.TotalGamePlayTimeInMilliseconds - MyHudWarnings.m_lastSoundPlayed <= 5000)
          return;
        MyHudWarnings.m_lastSoundPlayed = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        MyHudWarnings.m_sound = MyGuiAudio.PlaySound(MyHudWarnings.m_soundQueue[0]);
        MyHudWarnings.m_soundQueue.RemoveAt(0);
      }
    }
  }
}
