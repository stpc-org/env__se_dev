// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Autopilots.MySimpleAutopilot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Groups;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Autopilots
{
  [MyAutopilotType(typeof (MyObjectBuilder_SimpleAutopilot))]
  internal class MySimpleAutopilot : MyAutopilotBase
  {
    private const int SHIP_LIFESPAN_MILLISECONDS = 1800000;
    private int m_spawnTime;
    private long[] m_gridIds;
    private Vector3 m_direction;
    private Vector3D m_destination;
    private int m_subgridLookupCounter = -1;

    public MySimpleAutopilot()
    {
    }

    public MySimpleAutopilot(Vector3D destination, Vector3 direction, long[] gridsIds)
    {
      this.m_gridIds = gridsIds;
      this.m_direction = direction;
      this.m_destination = destination;
      this.m_spawnTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    public override MyObjectBuilder_AutopilotBase GetObjectBuilder()
    {
      MyObjectBuilder_SimpleAutopilot newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SimpleAutopilot>();
      newObject.Destination = this.m_destination;
      newObject.Direction = this.m_direction;
      newObject.SpawnTime = new int?(this.m_spawnTime);
      newObject.GridIds = this.m_gridIds;
      return (MyObjectBuilder_AutopilotBase) newObject;
    }

    public override void Init(MyObjectBuilder_AutopilotBase objectBuilder)
    {
      MyObjectBuilder_SimpleAutopilot builderSimpleAutopilot = (MyObjectBuilder_SimpleAutopilot) objectBuilder;
      this.m_gridIds = builderSimpleAutopilot.GridIds;
      this.m_direction = builderSimpleAutopilot.Direction;
      this.m_destination = builderSimpleAutopilot.Destination;
      this.m_spawnTime = builderSimpleAutopilot.SpawnTime ?? MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_gridIds != null)
        return;
      this.m_subgridLookupCounter = 100;
    }

    public override void OnAttachedToShipController(MyCockpit newShipController)
    {
      base.OnAttachedToShipController(newShipController);
      if (this.m_subgridLookupCounter > 0)
        return;
      this.RegisterGridCallbacks();
    }

    private void RegisterGridCallbacks()
    {
      if (!Sync.IsServer)
        return;
      this.ForEachGrid((Action<MyCubeGrid>) (grid =>
      {
        grid.OnGridChanged += new Action<MyCubeGrid>(this.OnGridChanged);
        grid.OnBlockAdded += new Action<MySlimBlock>(this.OnBlockAddedRemovedOrChanged);
        grid.OnBlockRemoved += new Action<MySlimBlock>(this.OnBlockAddedRemovedOrChanged);
        grid.OnBlockIntegrityChanged += new Action<MySlimBlock>(this.OnBlockAddedRemovedOrChanged);
      }));
    }

    private void OnBlockAddedRemovedOrChanged(MySlimBlock obj) => this.PersistShip();

    private void OnGridChanged(MyCubeGrid grid) => this.PersistShip();

    private void PersistShip() => this.ShipController.RemoveAutopilot();

    public override void OnRemovedFromCockpit()
    {
      if (Sync.IsServer)
        this.ForEachGrid((Action<MyCubeGrid>) (grid =>
        {
          grid.OnGridChanged -= new Action<MyCubeGrid>(this.OnGridChanged);
          grid.OnBlockAdded -= new Action<MySlimBlock>(this.OnBlockAddedRemovedOrChanged);
          grid.OnBlockRemoved -= new Action<MySlimBlock>(this.OnBlockAddedRemovedOrChanged);
          grid.OnBlockIntegrityChanged -= new Action<MySlimBlock>(this.OnBlockAddedRemovedOrChanged);
        }));
      base.OnRemovedFromCockpit();
    }

    public override void Update()
    {
      if (!Sync.IsServer)
        return;
      if (this.m_subgridLookupCounter > 0 && --this.m_subgridLookupCounter == 0)
      {
        MyCubeGrid cubeGrid = this.ShipController.CubeGrid;
        this.m_gridIds = MyCubeGridGroups.Static.Logical.GetGroup(cubeGrid).Nodes.Select<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node, long>((Func<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node, long>) (x => x.NodeData.EntityId)).ToArray<long>();
        this.RegisterGridCallbacks();
      }
      MyCockpit shipController = this.ShipController;
      if (shipController == null || ((MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_spawnTime > 1800000 ? 1 : ((shipController.PositionComp.GetPosition() - this.m_destination).Dot(this.m_direction) > 0.0 ? 1 : 0)) == 0 || this.IsPlayerNearby()))
        return;
      this.ShipController.RemoveAutopilot();
      this.ForEachGrid((Action<MyCubeGrid>) (grid => grid.Close()));
    }

    private bool IsPlayerNearby()
    {
      BoundingSphereD boundingSphereD = new BoundingSphereD(this.ShipController.PositionComp.GetPosition(), 2000.0);
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (boundingSphereD.Contains(onlinePlayer.GetPosition()) == ContainmentType.Contains)
          return true;
      }
      return false;
    }

    private void ForEachGrid(Action<MyCubeGrid> action)
    {
      if (this.m_gridIds == null || this.m_gridIds.Length == 0)
        return;
      foreach (long gridId in this.m_gridIds)
      {
        MyCubeGrid entityById = (MyCubeGrid) Sandbox.Game.Entities.MyEntities.GetEntityById(gridId);
        if (entityById != null)
          action(entityById);
      }
    }

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_NEUTRAL_SHIPS || this.ShipController == null)
        return;
      Vector3D position1 = MySector.MainCamera.Position;
      Vector3D vector3D1 = Vector3D.Normalize(this.ShipController.PositionComp.GetPosition() - position1);
      Vector3D vector3D2 = Vector3D.Normalize(this.m_destination - position1);
      Vector3D vector3D3 = Vector3D.Normalize((vector3D1 + vector3D2) * 0.5) + position1;
      Vector3D vector3D4 = vector3D1 + position1;
      Vector3D pointTo = vector3D2 + position1;
      Vector3D position2 = Vector3D.Normalize(this.ShipController.WorldMatrix.Translation - position1) + position1;
      MyRenderProxy.DebugDrawLine3D(vector3D4, vector3D3, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawLine3D(vector3D3, pointTo, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawSphere(position2, 0.01f, (Color) Color.Orange.ToVector3(), depthRead: false);
      MyRenderProxy.DebugDrawSphere(position2 + this.m_direction * 0.015f, 0.005f, (Color) Color.Yellow.ToVector3(), depthRead: false);
      MyRenderProxy.DebugDrawText3D(vector3D4, "Remaining time: " + (object) (1800000 - MySandboxGame.TotalGamePlayTimeInMilliseconds + this.m_spawnTime), Color.Red, 1f, false);
    }
  }
}
