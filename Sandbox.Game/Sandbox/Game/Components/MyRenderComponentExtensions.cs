// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using VRage.Game.Components;
using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Game.Components
{
  public static class MyRenderComponentExtensions
  {
    public static unsafe void CalculateBlockDepthBias(
      this MyRenderComponent renderComponent,
      MyCubeBlock block)
    {
      if (block.Hierarchy == null || block.Hierarchy.Parent == null || !(block.Hierarchy.Parent.Entity is MyCompoundCubeBlock entity))
        return;
      bool* flagPtr = stackalloc bool[64];
      foreach (MySlimBlock block1 in entity.GetBlocks())
      {
        if (block1.FatBlock != null && block1.FatBlock != block)
        {
          MyRenderComponentBase render = block1.FatBlock.Render;
          if (render != null)
            flagPtr[(int) render.DepthBias] = true;
        }
      }
      int num = 0;
      if (renderComponent.ModelStorage is MyModel modelStorage)
      {
        Vector3 center = modelStorage.BoundingSphere.Center;
        MatrixI matrix = new MatrixI(block.SlimBlock.Orientation);
        Vector3 result = new Vector3();
        Vector3.Transform(ref center, ref matrix, out result);
        if ((double) result.LengthSquared() > 0.5)
          num = (double) Math.Abs(result.X) <= (double) Math.Abs(result.Y) ? ((double) Math.Abs(result.Z) <= (double) Math.Abs(result.Y) ? ((double) result.Y > 0.0 ? 6 : 8) : ((double) result.Z > 0.0 ? 10 : 12)) : ((double) Math.Abs(result.X) <= (double) Math.Abs(result.Z) ? ((double) result.Z > 0.0 ? 10 : 12) : ((double) result.X > 0.0 ? 2 : 4));
      }
      for (int index = num; index < 64; ++index)
      {
        if (!flagPtr[index])
        {
          renderComponent.DepthBias = (byte) index;
          break;
        }
      }
    }
  }
}
