// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentPlanet
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.World;
using VRage;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentPlanet : MyDebugRenderComponent
  {
    private MyPlanet m_planet;

    public MyDebugRenderComponentPlanet(MyPlanet voxelMap)
      : base((IMyEntity) voxelMap)
      => this.m_planet = voxelMap;

    public override void DebugDraw()
    {
      Vector3D leftBottomCorner = this.m_planet.PositionLeftBottomCorner;
      if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB)
      {
        this.m_planet.Components.Get<MyPlanetEnvironmentComponent>().DebugDraw();
        this.m_planet.DebugDrawPhysics();
        MyRenderProxy.DebugDrawAABB(this.m_planet.PositionComp.WorldAABB, Color.White);
        MyRenderProxy.DebugDrawLine3D(leftBottomCorner, leftBottomCorner + new Vector3(1f, 0.0f, 0.0f), Color.Red, Color.Red, true);
        MyRenderProxy.DebugDrawLine3D(leftBottomCorner, leftBottomCorner + new Vector3(0.0f, 1f, 0.0f), Color.Green, Color.Green, true);
        MyRenderProxy.DebugDrawLine3D(leftBottomCorner, leftBottomCorner + new Vector3(0.0f, 0.0f, 1f), Color.Blue, Color.Blue, true);
        MyRenderProxy.DebugDrawAxis(this.m_planet.PositionComp.WorldMatrixRef, 2f, false);
        MyRenderProxy.DebugDrawSphere(this.m_planet.PositionComp.GetPosition(), 1f, Color.OrangeRed, depthRead: false);
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_VOXEL_GEOMETRY_CELL)
        return;
      MyCamera mainCamera = MySector.MainCamera;
      LineD line = new LineD(mainCamera.Position, mainCamera.Position + 25f * mainCamera.ForwardVector);
      MyIntersectionResultLineTriangleEx? t;
      if (!this.m_planet.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES))
        return;
      MyTriangle_Vertices inputTriangle = t.Value.Triangle.InputTriangle;
      MyRenderProxy.DebugDrawTriangle(inputTriangle.Vertex0 + leftBottomCorner, inputTriangle.Vertex1 + leftBottomCorner, inputTriangle.Vertex2 + leftBottomCorner, Color.Red, true, false);
      Vector3D pointInWorldSpace = t.Value.IntersectionPointInWorldSpace;
      Vector3I voxelCoord;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(leftBottomCorner, ref pointInWorldSpace, out voxelCoord);
      BoundingBoxD worldAABB;
      MyVoxelCoordSystems.VoxelCoordToWorldAABB(leftBottomCorner, ref voxelCoord, out worldAABB);
      MyRenderProxy.DebugDrawAABB(worldAABB, (Color) Vector3.UnitY);
      Vector3I geometryCellCoord;
      MyVoxelCoordSystems.WorldPositionToGeometryCellCoord(leftBottomCorner, ref pointInWorldSpace, out geometryCellCoord);
      MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(leftBottomCorner, ref geometryCellCoord, out worldAABB);
      MyRenderProxy.DebugDrawAABB(worldAABB, (Color) Vector3.UnitZ);
    }
  }
}
