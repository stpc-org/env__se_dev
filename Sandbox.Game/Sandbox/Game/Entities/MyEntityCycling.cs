// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityCycling
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public static class MyEntityCycling
  {
    public static float GetMetric(MyEntityCyclingOrder order, MyEntity entity)
    {
      MyCubeGrid grid = entity as MyCubeGrid;
      MyPhysicsComponentBase physics = entity.Physics;
      switch (order)
      {
        case MyEntityCyclingOrder.Characters:
          return entity is MyCharacter ? 1f : 0.0f;
        case MyEntityCyclingOrder.BiggestGrids:
          return grid != null ? (float) grid.GetBlocks().Count : 0.0f;
        case MyEntityCyclingOrder.Fastest:
          return physics == null ? 0.0f : (float) Math.Round((double) physics.LinearVelocity.Length(), 2);
        case MyEntityCyclingOrder.BiggestDistanceFromPlayers:
          switch (entity)
          {
            case MyVoxelBase _:
            case MySafeZone _:
              return 0.0f;
            default:
              return MyEntityCycling.GetPlayerDistance(entity);
          }
        case MyEntityCyclingOrder.MostActiveDrills:
          return MyEntityCycling.GetActiveBlockCount<MyShipDrill>(grid);
        case MyEntityCyclingOrder.MostActiveReactors:
          return MyEntityCycling.GetActiveBlockCount<MyReactor>(grid);
        case MyEntityCyclingOrder.MostActiveProductionBuildings:
          return MyEntityCycling.GetActiveBlockCount<MyProductionBlock>(grid);
        case MyEntityCyclingOrder.MostActiveSensors:
          return MyEntityCycling.GetActiveBlockCount<MySensorBlock>(grid);
        case MyEntityCyclingOrder.MostActiveThrusters:
          return MyEntityCycling.GetActiveBlockCount<MyThrust>(grid);
        case MyEntityCyclingOrder.MostWheels:
          return MyEntityCycling.GetActiveBlockCount<MyMotorSuspension>(grid, true);
        case MyEntityCyclingOrder.StaticObjects:
          return entity.Physics == null || entity.Physics.IsPhantom || (double) entity.Physics.AngularVelocity.AbsMax() >= 0.0500000007450581 || (double) entity.Physics.LinearVelocity.AbsMax() >= 0.0500000007450581 ? 0.0f : 1f;
        case MyEntityCyclingOrder.FloatingObjects:
          return entity is MyFloatingObject ? 1f : 0.0f;
        case MyEntityCyclingOrder.Planets:
          return entity is MyPlanet ? 1f : 0.0f;
        case MyEntityCyclingOrder.OwnerLoginTime:
          return MyEntityCycling.GetOwnerLoginTime(grid);
        default:
          return 0.0f;
      }
    }

    private static float GetOwnerLoginTime(MyCubeGrid grid)
    {
      if (grid == null || grid.BigOwners.Count == 0)
        return 0.0f;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(grid.BigOwners[0]);
      return identity == null ? 0.0f : (float) Math.Round((DateTime.Now - identity.LastLoginTime).TotalDays, 2);
    }

    private static float GetActiveBlockCount<T>(MyCubeGrid grid, bool includePassive = false) where T : MyFunctionalBlock
    {
      if (grid == null)
        return 0.0f;
      int num = 0;
      foreach (MySlimBlock block in grid.GetBlocks())
      {
        if (block.FatBlock is T fatBlock && (includePassive || fatBlock.IsWorking))
          ++num;
      }
      return (float) num;
    }

    private static float GetPlayerDistance(MyEntity entity)
    {
      Vector3D translation = entity.WorldMatrix.Translation;
      float num1 = float.MaxValue;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
        if (controlledEntity != null)
        {
          float num2 = Vector3.DistanceSquared((Vector3) controlledEntity.Entity.WorldMatrix.Translation, (Vector3) translation);
          if ((double) num2 < (double) num1)
            num1 = num2;
        }
      }
      return (float) Math.Sqrt((double) num1);
    }

    public static void FindNext(
      MyEntityCyclingOrder order,
      ref float metric,
      ref long entityId,
      bool findLarger,
      CyclingOptions options)
    {
      MyEntityCycling.Metric metric1 = new MyEntityCycling.Metric()
      {
        Value = metric,
        EntityId = entityId
      };
      MyEntityCycling.Metric metric2 = findLarger ? MyEntityCycling.Metric.Max : MyEntityCycling.Metric.Min;
      MyEntityCycling.Metric metric3 = metric2;
      MyEntityCycling.Metric metric4 = metric2;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (options.Enabled)
        {
          MyCubeGrid myCubeGrid = entity as MyCubeGrid;
          if (options.OnlyLargeGrids && (myCubeGrid == null || myCubeGrid.GridSizeEnum != MyCubeSize.Large) || options.OnlySmallGrids && (myCubeGrid == null || myCubeGrid.GridSizeEnum != MyCubeSize.Small))
            continue;
        }
        MyEntityCycling.Metric metric5 = new MyEntityCycling.Metric(MyEntityCycling.GetMetric(order, entity), entity.EntityId);
        if ((double) metric5.Value != 0.0)
        {
          if (findLarger)
          {
            if (metric5 > metric1 && metric5 < metric3)
              metric3 = metric5;
            if (metric5 < metric4)
              metric4 = metric5;
          }
          else
          {
            if (metric5 < metric1 && metric5 > metric3)
              metric3 = metric5;
            if (metric5 > metric4)
              metric4 = metric5;
          }
        }
      }
      if (metric3 == metric2)
        metric3 = metric4;
      metric = metric3.Value;
      entityId = metric3.EntityId;
    }

    public struct Metric
    {
      public static readonly MyEntityCycling.Metric Min = new MyEntityCycling.Metric()
      {
        Value = float.MinValue,
        EntityId = 0
      };
      public static readonly MyEntityCycling.Metric Max = new MyEntityCycling.Metric()
      {
        Value = float.MaxValue,
        EntityId = 0
      };
      public float Value;
      public long EntityId;

      public Metric(float value, long entityId)
      {
        this.Value = value;
        this.EntityId = entityId;
      }

      public static bool operator >(MyEntityCycling.Metric a, MyEntityCycling.Metric b)
      {
        if ((double) a.Value > (double) b.Value)
          return true;
        return (double) a.Value == (double) b.Value && a.EntityId > b.EntityId;
      }

      public static bool operator <(MyEntityCycling.Metric a, MyEntityCycling.Metric b) => b > a;

      public static bool operator >=(MyEntityCycling.Metric a, MyEntityCycling.Metric b)
      {
        if ((double) a.Value > (double) b.Value)
          return true;
        return (double) a.Value == (double) b.Value && a.EntityId >= b.EntityId;
      }

      public static bool operator <=(MyEntityCycling.Metric a, MyEntityCycling.Metric b) => b >= a;

      public static bool operator ==(MyEntityCycling.Metric a, MyEntityCycling.Metric b) => (double) a.Value == (double) b.Value && a.EntityId == b.EntityId;

      public static bool operator !=(MyEntityCycling.Metric a, MyEntityCycling.Metric b) => !(a == b);
    }
  }
}
