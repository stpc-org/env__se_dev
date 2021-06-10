// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyDefaultPlacementProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyDefaultPlacementProvider : IMyPlacementProvider
  {
    private int m_lastUpdate;
    private MyPhysics.HitInfo? m_hitInfo;
    private MyCubeGrid m_closestGrid;
    private MySlimBlock m_closestBlock;
    private MyVoxelBase m_closestVoxelMap;
    private readonly List<MyPhysics.HitInfo> m_tmpHitList = new List<MyPhysics.HitInfo>();

    public MyDefaultPlacementProvider(float intersectionDistance) => this.IntersectionDistance = intersectionDistance;

    public Vector3D RayStart
    {
      get
      {
        switch (MySession.Static.GetCameraControllerEnum())
        {
          case MyCameraControllerEnum.Entity:
          case MyCameraControllerEnum.ThirdPersonSpectator:
            if (MySession.Static.ControlledEntity != null)
              return MySession.Static.ControlledEntity.GetHeadMatrix(false).Translation;
            if (MySector.MainCamera != null)
              return MySector.MainCamera.Position;
            break;
          default:
            if (MySector.MainCamera != null)
              return MySector.MainCamera.Position;
            break;
        }
        return (Vector3D) Vector3.Zero;
      }
    }

    public Vector3D RayDirection
    {
      get
      {
        switch (MySession.Static.GetCameraControllerEnum())
        {
          case MyCameraControllerEnum.Entity:
          case MyCameraControllerEnum.ThirdPersonSpectator:
            if (MySession.Static.ControlledEntity != null)
              return MySession.Static.ControlledEntity.GetHeadMatrix(false).Forward;
            if (MySector.MainCamera != null)
              return (Vector3D) MySector.MainCamera.ForwardVector;
            break;
          default:
            if (MySector.MainCamera != null)
              return (Vector3D) MySector.MainCamera.ForwardVector;
            break;
        }
        return (Vector3D) Vector3.Forward;
      }
    }

    public MyPhysics.HitInfo? HitInfo
    {
      get
      {
        if (MySession.Static.GameplayFrameCounter != this.m_lastUpdate)
          this.UpdatePlacement();
        return this.m_hitInfo;
      }
    }

    public MyCubeGrid ClosestGrid
    {
      get
      {
        if (MySession.Static.GameplayFrameCounter != this.m_lastUpdate)
          this.UpdatePlacement();
        return this.m_closestGrid;
      }
    }

    public MyVoxelBase ClosestVoxelMap
    {
      get
      {
        if (MySession.Static.GameplayFrameCounter != this.m_lastUpdate)
          this.UpdatePlacement();
        return this.m_closestVoxelMap;
      }
    }

    public bool CanChangePlacementObjectSize => false;

    public float IntersectionDistance { get; set; }

    public void RayCastGridCells(
      MyCubeGrid grid,
      List<Vector3I> outHitPositions,
      Vector3I gridSizeInflate,
      float maxDist)
    {
      grid.RayCastCells(this.RayStart, this.RayStart + this.RayDirection * (double) maxDist, outHitPositions, new Vector3I?(gridSizeInflate), false, true);
    }

    public void UpdatePlacement()
    {
      this.m_lastUpdate = MySession.Static.GameplayFrameCounter;
      this.m_hitInfo = new MyPhysics.HitInfo?();
      this.m_closestGrid = (MyCubeGrid) null;
      this.m_closestVoxelMap = (MyVoxelBase) null;
      LineD lineD = new LineD(this.RayStart, this.RayStart + this.RayDirection * (double) this.IntersectionDistance);
      MyPhysics.CastRay(lineD.From, lineD.To, this.m_tmpHitList, 24);
      if (MySession.Static.ControlledEntity != null)
        this.m_tmpHitList.RemoveAll((Predicate<MyPhysics.HitInfo>) (hitInfo => hitInfo.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity));
      if (this.m_tmpHitList.Count == 0)
        return;
      MyPhysics.HitInfo tmpHit = this.m_tmpHitList[0];
      if (tmpHit.HkHitInfo.GetHitEntity() != null)
        this.m_closestGrid = tmpHit.HkHitInfo.GetHitEntity().GetTopMostParent() as MyCubeGrid;
      if (this.m_closestGrid != null)
      {
        this.m_hitInfo = new MyPhysics.HitInfo?(tmpHit);
        if (this.ClosestGrid.Editable)
          return;
        this.m_closestGrid = (MyCubeGrid) null;
      }
      else
      {
        this.m_closestVoxelMap = tmpHit.HkHitInfo.GetHitEntity() as MyVoxelBase;
        if (this.m_closestVoxelMap == null)
          return;
        this.m_hitInfo = new MyPhysics.HitInfo?(tmpHit);
      }
    }
  }
}
