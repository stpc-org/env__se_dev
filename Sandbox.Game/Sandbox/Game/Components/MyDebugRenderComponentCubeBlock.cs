// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderComponentCubeBlock : MyDebugRenderComponent
  {
    private MyCubeBlock m_cubeBlock;

    public MyDebugRenderComponentCubeBlock(MyCubeBlock cubeBlock)
      : base((IMyEntity) cubeBlock)
      => this.m_cubeBlock = cubeBlock;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_CUBE_BLOCK_AABBS)
        return;
      Color red = Color.Red;
      Color green = Color.Green;
      Vector3I center = this.m_cubeBlock.BlockDefinition.Center;
      BoundingBoxD localbox = new BoundingBoxD((Vector3D) (this.m_cubeBlock.Min * this.m_cubeBlock.CubeGrid.GridSize - new Vector3(this.m_cubeBlock.CubeGrid.GridSize / 2f)), (Vector3D) (this.m_cubeBlock.Max * this.m_cubeBlock.CubeGrid.GridSize + new Vector3(this.m_cubeBlock.CubeGrid.GridSize / 2f)));
      MatrixD worldMatrix = this.m_cubeBlock.CubeGrid.WorldMatrix;
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref red, MySimpleObjectRasterizer.Wireframe, 1, 0.01f);
    }
  }
}
