// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentVoxelMap
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentVoxelMap : MyDebugRenderComponent
  {
    private MyVoxelBase m_voxelMap;

    public MyDebugRenderComponentVoxelMap(MyVoxelBase voxelMap)
      : base((IMyEntity) voxelMap)
      => this.m_voxelMap = voxelMap;

    public override void DebugDraw()
    {
      Vector3D leftBottomCorner = this.m_voxelMap.PositionLeftBottomCorner;
      if (!MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB)
        return;
      MyRenderProxy.DebugDrawAABB(this.m_voxelMap.PositionComp.WorldAABB, Color.White, 0.2f);
      MyRenderProxy.DebugDrawLine3D(leftBottomCorner, leftBottomCorner + new Vector3(1f, 0.0f, 0.0f), Color.Red, Color.Red, true);
      MyRenderProxy.DebugDrawLine3D(leftBottomCorner, leftBottomCorner + new Vector3(0.0f, 1f, 0.0f), Color.Green, Color.Green, true);
      MyRenderProxy.DebugDrawLine3D(leftBottomCorner, leftBottomCorner + new Vector3(0.0f, 0.0f, 1f), Color.Blue, Color.Blue, true);
      MyRenderProxy.DebugDrawAxis(this.m_voxelMap.PositionComp.WorldMatrixRef, 2f, false);
      MyRenderProxy.DebugDrawSphere(this.m_voxelMap.PositionComp.GetPosition(), 1f, Color.OrangeRed, depthRead: false);
    }
  }
}
