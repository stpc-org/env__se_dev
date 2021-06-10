// Decompiled with JetBrains decompiler
// Type: Sandbox.MyCubeGridExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox
{
  public static class MyCubeGridExtensions
  {
    internal static bool HasSameGroupAndIsGrid<TGroupData>(
      this MyGroups<MyCubeGrid, TGroupData> groups,
      IMyEntity gridA,
      IMyEntity gridB)
      where TGroupData : IGroupData<MyCubeGrid>, new()
    {
      MyCubeGrid nodeA = gridA as MyCubeGrid;
      MyCubeGrid nodeB = gridB as MyCubeGrid;
      return nodeA != null && nodeB != null && groups.HasSameGroup(nodeA, nodeB);
    }

    public static BoundingSphere CalculateBoundingSphere(
      this MyObjectBuilder_CubeGrid grid)
    {
      return BoundingSphere.CreateFromBoundingBox(grid.CalculateBoundingBox());
    }

    public static BoundingBox CalculateBoundingBox(this MyObjectBuilder_CubeGrid grid)
    {
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(grid.GridSizeEnum);
      BoundingBox boundingBox = new BoundingBox(Vector3.MaxValue, Vector3.MinValue);
      try
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
        {
          MyCubeBlockDefinition blockDefinition;
          if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(cubeBlock.GetId(), out blockDefinition))
          {
            MyBlockOrientation blockOrientation = (MyBlockOrientation) cubeBlock.BlockOrientation;
            Vector3 vector3 = Vector3.Abs(Vector3.TransformNormal(new Vector3(blockDefinition.Size) * cubeSize, blockOrientation));
            Vector3 point1 = new Vector3((Vector3I) cubeBlock.Min) * cubeSize - new Vector3(cubeSize / 2f);
            Vector3 point2 = point1 + vector3;
            boundingBox.Include(point1);
            boundingBox.Include(point2);
          }
        }
      }
      catch (KeyNotFoundException ex)
      {
        MySandboxGame.Log.WriteLine((Exception) ex);
        return new BoundingBox();
      }
      return boundingBox;
    }

    public static int CalculatePCUs(this MyObjectBuilder_CubeGrid grid)
    {
      int num = 0;
      MyDefinitionManager definitionManager = MyDefinitionManager.Static;
      foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
      {
        MyCubeBlockDefinition blockDefinition;
        if (definitionManager.TryGetCubeBlockDefinition(cubeBlock.GetId(), out blockDefinition))
        {
          if (new MyComponentStack(blockDefinition, cubeBlock.IntegrityPercent, cubeBlock.BuildPercent).IsFunctional)
            num += blockDefinition.PCU;
          else
            num += MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
        }
      }
      return num;
    }

    public static void HookMultiplayer(this MyCubeBlock cubeBlock)
    {
      if (cubeBlock == null)
        return;
      MyEntities.RaiseEntityCreated((MyEntity) cubeBlock);
    }
  }
}
