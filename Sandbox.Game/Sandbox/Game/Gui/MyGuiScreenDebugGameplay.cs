// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugGameplay
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Gameplay")]
  [StaticEventOwner]
  internal class MyGuiScreenDebugGameplay : MyGuiScreenDebugBase
  {
    private const float TWO_BUTTON_XOFFSET = 0.05f;

    public MyGuiScreenDebugGameplay()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Gameplay", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddVerticalSpacing(0.01f * this.m_scale);
      this.AddCheckBox("Debris enabled", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_DEBRIS)));
      this.AddCheckBox("Drill rocks enabled", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_DRILL_ROCKS)));
      this.AddCheckBox("Revoke Game Inventory Items", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MySessionComponentGameInventory.DEBUG_REVOKE_ITEM_OWNERSHIP)));
      if (MySession.Static != null)
        this.AddCheckBox("Adjustable Vehicle Max Speed", (object) MySession.Static.Settings, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MySession.Static.Settings.AdjustableMaxVehicleSpeed)));
      this.AddVerticalSpacing(0.02f);
      if (MySession.Static?.LocalCharacter != null)
      {
        MyCharacter character = MySession.Static.LocalCharacter;
        this.AddLabel("Local Character", (Vector4) Color.Yellow, 1f);
        this.AddVerticalSpacing();
        MyCharacterStatComponent stats = character.StatComp;
        if (stats != null)
          this.AddSlider("Health", stats.Health.MinValue, stats.Health.MaxValue, stats.Health.MaxValue, (Func<float>) (() => stats.Health.Value), (Action<float>) (value => stats.Health.Value = value));
        MyCharacterOxygenComponent oxygen = character.OxygenComponent;
        if (oxygen != null)
        {
          MyDefinitionId oxygenId = MyCharacterOxygenComponent.OxygenId;
          this.AddSlider("Oxygen", 0.0f, 100f, 100f, (Func<float>) (() => oxygen.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) * 100f), (Action<float>) (value => oxygen.UpdateStoredGasLevel(ref oxygenId, value * 0.01f)));
          MyDefinitionId hydrogenId = MyCharacterOxygenComponent.HydrogenId;
          this.AddSlider("Hydrogen", 0.0f, 100f, 100f, (Func<float>) (() => oxygen.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) * 100f), (Action<float>) (value => oxygen.UpdateStoredGasLevel(ref hydrogenId, value * 0.01f)));
        }
        MyBattery energy = character.SuitBattery;
        if (energy != null)
          this.AddSlider("Energy", 0.0f, 100f, 100f, (Func<float>) (() => (float) ((double) energy.ResourceSource.RemainingCapacity / 9.99999974737875E-06 * 100.0)), (Action<float>) (value => energy.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, value * 1E-07f)));
        this.AddVerticalSpacing(0.02f);
        this.AddButton("Set toolbar to shared", (Action<MyGuiControlButton>) (_ => MySession.Static.SharedToolbar = character.ControlSteamId));
      }
      else
        this.AddLabel("No Character present", (Vector4) Color.Orange, 1f);
      this.AddVerticalSpacing(0.02f);
      this.AddSlider("Battery Depletion Multiplier", 0.0f, 100f, 1f, (Func<float>) (() => MyBattery.BATTERY_DEPLETION_MULTIPLIER), new Action<float>(MyGuiScreenDebugGameplay.SetDepletionMultiplierLocal));
      this.AddSlider("Reactor Fuel Consumption Multiplier", 0.0f, 100f, 1f, (Func<float>) (() => MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER), new Action<float>(MyGuiScreenDebugGameplay.SetFuelConsumptionMultiplierLocal));
      this.AddVerticalSpacing(0.02f);
      if (!(MySession.Static?.ControlledEntity?.Entity is MyCubeGrid myCubeGrid))
        myCubeGrid = MySession.Static?.ControlledEntity?.Entity is MyCockpit entity ? entity.CubeGrid : (MyCubeGrid) null;
      MyCubeGrid grid = myCubeGrid;
      if (grid?.Physics != null)
      {
        this.AddLabel("Controlled Ship", (Vector4) Color.Yellow, 1f);
        this.AddVerticalSpacing();
        Vector2 currentPosition1 = this.m_currentPosition;
        this.m_buttonXOffset = -0.05f;
        this.AddButton("Stop", (Action<MyGuiControlButton>) (_ => grid.Physics.LinearVelocity = Vector3.Zero));
        this.m_currentPosition = currentPosition1;
        this.m_buttonXOffset = 0.05f;
        Vector3 velocityVector = !Vector3.IsZero(grid.Physics.LinearVelocity) ? Vector3.Normalize(grid.Physics.LinearVelocity) : Vector3.Forward;
        this.AddButton("Max Speed", (Action<MyGuiControlButton>) (_ => MyGuiScreenDebugGameplay.SetMaxSpeed(grid, velocityVector)));
        if (grid.GridSizeEnum == MyCubeSize.Large)
        {
          Vector2 currentPosition2 = this.m_currentPosition;
          this.m_buttonXOffset = -0.05f;
          this.AddButton("Recharge Jump Drives", (Action<MyGuiControlButton>) (_ => MyGuiScreenDebugGameplay.RechargeJumpDrives(grid)));
          this.m_currentPosition = currentPosition2;
          this.m_buttonXOffset = 0.05f;
          this.AddButton("Discharge Jump Drives", (Action<MyGuiControlButton>) (_ => MyGuiScreenDebugGameplay.DischargeJumpDrives(grid)));
        }
        this.m_buttonXOffset = 0.0f;
      }
      else
        this.AddLabel("No Ship is being controlled", (Vector4) Color.Orange, 1f);
      this.AddVerticalSpacing(0.02f);
      this.AddSlider("Container Drop Multiplier", 1f, 10f, 1f, new Func<float>(MyGuiScreenDebugGameplay.GetRespawnTimeMultiplier), new Action<float>(MyGuiScreenDebugGameplay.SetRespawnTimeMultiplier));
      this.AddVerticalSpacing();
      this.AddButton("Trigger Meteor Shower", (Action<MyGuiControlButton>) (_ => MyGuiScreenDebugGameplay.TriggerMeteorShower()));
      this.AddButton("Trigger Cargo Ship", (Action<MyGuiControlButton>) (_ => MyGuiScreenDebugGameplay.TriggerCargoShip()));
      this.AddVerticalSpacing(0.02f);
      this.AddButton("Force cluster reorder", (Action<MyGuiControlButton>) (x => this.ForceClusterReorder()));
      this.AddButton("Draw positions of stations", (Action<MyGuiControlButton>) (x => MyGuiScreenDebugGameplay.DrawStations()));
      this.AddButton("Force economy update", (Action<MyGuiControlButton>) (x => this.UpdateEconomy()));
      this.AddCheckBox("Force Add Trash Removal Menu", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU)));
    }

    private void ForceClusterReorder() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyGuiScreenDebugGameplay.ForceClusterReorderRequest)));

    [Event(null, 179)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ForceClusterReorderRequest() => MyGuiScreenDebugGameplay.ForceClusterReorderInternal();

    private static void ForceClusterReorderInternal() => MyFakes.FORCE_CLUSTER_REORDER = true;

    private void UpdateEconomy()
    {
      if (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
        MyGuiScreenDebugGameplay.UpdateEconomyInternal();
      else
        MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyGuiScreenDebugGameplay.UpdateEconomyRequest)));
    }

    [Event(null, 202)]
    [Reliable]
    [Server]
    private static void UpdateEconomyRequest() => MyGuiScreenDebugGameplay.UpdateEconomyInternal();

    private static void UpdateEconomyInternal() => MySession.Static.GetComponent<MySessionComponentEconomy>()?.ForceEconomyTick();

    private static void DrawStations()
    {
      if (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
      {
        if (MySession.Static.Factions == null)
          return;
        foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
        {
          foreach (MyStation station in faction.Value.Stations)
          {
            switch (station.Type)
            {
              case MyStationTypeEnum.MiningStation:
                MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.Red, depthRead: false, cull: false, persistent: true);
                MyRenderProxy.DebugDrawLine3D(station.Position, Vector3D.Zero, Color.Red, Color.Red, false, true);
                continue;
              case MyStationTypeEnum.OrbitalStation:
                MyPlanet closestPlanet1 = MyGamePruningStructure.GetClosestPlanet(station.Position);
                Vector3D pointTo1 = closestPlanet1 != null ? closestPlanet1.PositionComp.GetPosition() : Vector3D.Zero;
                MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.CornflowerBlue, depthRead: false, cull: false, persistent: true);
                MyRenderProxy.DebugDrawLine3D(station.Position, pointTo1, Color.CornflowerBlue, Color.CornflowerBlue, false, true);
                continue;
              case MyStationTypeEnum.Outpost:
                MyPlanet closestPlanet2 = MyGamePruningStructure.GetClosestPlanet(station.Position);
                Vector3D pointTo2 = closestPlanet2 != null ? closestPlanet2.PositionComp.GetPosition() : Vector3D.Zero;
                MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.Yellow, depthRead: false, cull: false, persistent: true);
                MyRenderProxy.DebugDrawLine3D(station.Position, pointTo2, Color.Yellow, Color.Yellow, false, true);
                continue;
              case MyStationTypeEnum.SpaceStation:
                Color color = !station.IsDeepSpaceStation ? Color.Green : Color.Purple;
                MyRenderProxy.DebugDrawSphere(station.Position, 150f, color, depthRead: false, cull: false, persistent: true);
                MyRenderProxy.DebugDrawLine3D(station.Position, Vector3D.Zero, color, color, false, true);
                continue;
              default:
                continue;
            }
          }
        }
      }
      else
        MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyGuiScreenDebugGameplay.RequestStationPositions)));
    }

    [Event(null, 290)]
    [Reliable]
    [Server]
    private static void RequestStationPositions()
    {
      if (MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer || MySession.Static.Factions == null)
        return;
      List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure> debugDrawStructureList = new List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure>();
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        foreach (MyStation station in faction.Value.Stations)
        {
          MyGuiScreenDebugGameplay.MyStationDebugDrawStructure debugDrawStructure = new MyGuiScreenDebugGameplay.MyStationDebugDrawStructure();
          debugDrawStructure.Start = (SerializableVector3D) Vector3D.Zero;
          debugDrawStructure.End = (SerializableVector3D) station.Position;
          switch (station.Type)
          {
            case MyStationTypeEnum.MiningStation:
              debugDrawStructure.TypeId = 0;
              break;
            case MyStationTypeEnum.OrbitalStation:
              debugDrawStructure.TypeId = 1;
              MyPlanet closestPlanet1 = MyGamePruningStructure.GetClosestPlanet(station.Position);
              if (closestPlanet1 != null)
              {
                debugDrawStructure.Start = (SerializableVector3D) closestPlanet1.PositionComp.GetPosition();
                break;
              }
              break;
            case MyStationTypeEnum.Outpost:
              debugDrawStructure.TypeId = 2;
              MyPlanet closestPlanet2 = MyGamePruningStructure.GetClosestPlanet(station.Position);
              if (closestPlanet2 != null)
              {
                debugDrawStructure.Start = (SerializableVector3D) closestPlanet2.PositionComp.GetPosition();
                break;
              }
              break;
            case MyStationTypeEnum.SpaceStation:
              debugDrawStructure.TypeId = !station.IsDeepSpaceStation ? 4 : 3;
              break;
          }
          debugDrawStructureList.Add(debugDrawStructure);
        }
      }
      MyMultiplayer.RaiseStaticEvent<List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure>>((Func<IMyEventOwner, Action<List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure>>>) (s => new Action<List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure>>(MyGuiScreenDebugGameplay.DrawStationsClient)), debugDrawStructureList, MyEventContext.Current.Sender);
    }

    [Event(null, 348)]
    [Reliable]
    [Client]
    private static void DrawStationsClient(
      List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure> stations)
    {
      foreach (MyGuiScreenDebugGameplay.MyStationDebugDrawStructure station in stations)
      {
        Color color;
        switch (station.TypeId)
        {
          case 0:
            color = Color.Red;
            break;
          case 1:
            color = Color.CornflowerBlue;
            break;
          case 2:
            color = Color.Yellow;
            break;
          case 3:
            color = Color.Purple;
            break;
          case 4:
            color = Color.Green;
            break;
          default:
            color = Color.Pink;
            break;
        }
        MyRenderProxy.DebugDrawSphere((Vector3D) station.End, 150f, color, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D((Vector3D) station.End, (Vector3D) station.Start, color, color, false, true);
      }
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugGameplay);

    private static void SetMaxSpeed(MyCubeGrid grid, Vector3 direction)
    {
      if (grid?.Physics == null)
        return;
      if (grid.GridSizeEnum == MyCubeSize.Large)
        grid.Physics.LinearVelocity = direction * MySector.EnvironmentDefinition.LargeShipMaxSpeed;
      else
        grid.Physics.LinearVelocity = direction * MySector.EnvironmentDefinition.SmallShipMaxSpeed;
    }

    private static void RechargeJumpDrives(MyCubeGrid grid)
    {
      if (grid == null)
        return;
      foreach (MyJumpDrive fatBlock in grid.GetFatBlocks<MyJumpDrive>())
        fatBlock.SetStoredPower(1f);
    }

    private static void DischargeJumpDrives(MyCubeGrid grid)
    {
      if (grid == null)
        return;
      foreach (MyJumpDrive fatBlock in grid.GetFatBlocks<MyJumpDrive>())
        fatBlock.SetStoredPower(0.0f);
    }

    private static void SetRespawnTimeMultiplier(float multiplier)
    {
    }

    private static float GetRespawnTimeMultiplier()
    {
      float num = 1f;
      float? respawnTimeMultiplier = MySession.Static?.GetComponent<MySessionComponentContainerDropSystem>()?.GetRespawnTimeMultiplier();
      return (respawnTimeMultiplier.HasValue ? new float?(num / respawnTimeMultiplier.GetValueOrDefault()) : new float?()) ?? 1f;
    }

    private static void TriggerMeteorShower()
    {
      MyGlobalEventBase globalEvent = MyGlobalEventFactory.CreateEvent(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "MeteorWave"));
      MyGlobalEvents.RemoveGlobalEvent(globalEvent);
      globalEvent.SetActivationTime(TimeSpan.Zero);
      MyGlobalEvents.AddGlobalEvent(globalEvent);
    }

    private static void TriggerCargoShip()
    {
      MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
      MyGlobalEvents.RemoveGlobalEvent(eventById);
      eventById.SetActivationTime(TimeSpan.Zero);
      MyGlobalEvents.AddGlobalEvent(eventById);
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Cargo ship will spawn soon™", 5000));
    }

    private static void SetDepletionMultiplierLocal(float multiplier)
    {
      MyBattery.BATTERY_DEPLETION_MULTIPLIER = multiplier;
      MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (s => new Action<float>(MyGuiScreenDebugGameplay.SetDepletionMultiplier)), MyBattery.BATTERY_DEPLETION_MULTIPLIER);
    }

    [Event(null, 454)]
    [Reliable]
    [Server]
    private static void SetDepletionMultiplier(float multiplier)
    {
      MyBattery.BATTERY_DEPLETION_MULTIPLIER = multiplier;
      MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (s => new Action<float>(MyGuiScreenDebugGameplay.SetDepletionMultiplierSuccess)), MyBattery.BATTERY_DEPLETION_MULTIPLIER);
    }

    [Event(null, 462)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void SetDepletionMultiplierSuccess(float multiplier) => MyBattery.BATTERY_DEPLETION_MULTIPLIER = multiplier;

    private static void SetFuelConsumptionMultiplierLocal(float multiplier)
    {
      MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER = multiplier;
      MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (s => new Action<float>(MyGuiScreenDebugGameplay.SetFuelConsumptionMultiplier)), MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER);
    }

    [Event(null, 475)]
    [Reliable]
    [Server]
    private static void SetFuelConsumptionMultiplier(float multiplier)
    {
      MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER = multiplier;
      MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (s => new Action<float>(MyGuiScreenDebugGameplay.SetFuelConsumptionMultiplierSuccess)), MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER);
    }

    [Event(null, 483)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void SetFuelConsumptionMultiplierSuccess(float multiplier) => MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER = multiplier;

    [Serializable]
    public struct MyStationDebugDrawStructure
    {
      public SerializableVector3D Start;
      public SerializableVector3D End;
      public int TypeId;

      protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay\u003C\u003EMyStationDebugDrawStructure\u003C\u003EStart\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure, SerializableVector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugGameplay.MyStationDebugDrawStructure owner,
          in SerializableVector3D value)
        {
          owner.Start = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugGameplay.MyStationDebugDrawStructure owner,
          out SerializableVector3D value)
        {
          value = owner.Start;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay\u003C\u003EMyStationDebugDrawStructure\u003C\u003EEnd\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure, SerializableVector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugGameplay.MyStationDebugDrawStructure owner,
          in SerializableVector3D value)
        {
          owner.End = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugGameplay.MyStationDebugDrawStructure owner,
          out SerializableVector3D value)
        {
          value = owner.End;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay\u003C\u003EMyStationDebugDrawStructure\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugGameplay.MyStationDebugDrawStructure owner,
          in int value)
        {
          owner.TypeId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugGameplay.MyStationDebugDrawStructure owner,
          out int value)
        {
          value = owner.TypeId;
        }
      }
    }

    protected sealed class ForceClusterReorderRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.ForceClusterReorderRequest();
      }
    }

    protected sealed class UpdateEconomyRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.UpdateEconomyRequest();
      }
    }

    protected sealed class RequestStationPositions\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.RequestStationPositions();
      }
    }

    protected sealed class DrawStationsClient\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Gui_MyGuiScreenDebugGameplay\u003C\u003EMyStationDebugDrawStructure\u003E : ICallSite<IMyEventOwner, List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MyGuiScreenDebugGameplay.MyStationDebugDrawStructure> stations,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.DrawStationsClient(stations);
      }
    }

    protected sealed class SetDepletionMultiplier\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float multiplier,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.SetDepletionMultiplier(multiplier);
      }
    }

    protected sealed class SetDepletionMultiplierSuccess\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float multiplier,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.SetDepletionMultiplierSuccess(multiplier);
      }
    }

    protected sealed class SetFuelConsumptionMultiplier\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float multiplier,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.SetFuelConsumptionMultiplier(multiplier);
      }
    }

    protected sealed class SetFuelConsumptionMultiplierSuccess\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float multiplier,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugGameplay.SetFuelConsumptionMultiplierSuccess(multiplier);
      }
    }
  }
}
