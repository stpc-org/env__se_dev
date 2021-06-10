// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityTerrainHeightProviderComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.EntityComponents
{
  internal class MyEntityTerrainHeightProviderComponent : MyEntityComponentBase, IMyTerrainHeightProvider
  {
    private List<MyPhysics.HitInfo> m_raycastHits = new List<MyPhysics.HitInfo>(32);
    private MatrixD m_worldMatrix;
    private MatrixD m_worldMatrixInv;
    private float m_bbBottom;
    private Dictionary<int, MyEntityTerrainHeightProviderComponent.AsyncQuery> m_cachedTasks = new Dictionary<int, MyEntityTerrainHeightProviderComponent.AsyncQuery>();

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      this.Entity.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.OnPositionChanged);
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      this.Entity.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.OnPositionChanged);
    }

    private void OnPositionChanged(MyPositionComponentBase obj)
    {
      this.m_worldMatrix = this.Entity.WorldMatrix;
      this.m_worldMatrixInv = this.Entity.WorldMatrixNormalizedInv;
      this.m_bbBottom = this.Entity.PositionComp.LocalAABB.Min.Y;
    }

    public override string ComponentTypeDebugString => "SkinnedEntityTerrainHeightProvider";

    bool IMyTerrainHeightProvider.GetTerrainHeight(
      int key,
      Vector3 bonePosition,
      Vector3 boneRigPosition,
      out float terrainHeight,
      out Vector3 terrainNormal)
    {
      MatrixD worldMatrix = this.m_worldMatrix;
      Vector3D down = worldMatrix.Down;
      Vector3D vector3D1 = Vector3D.Transform((Vector3D) new Vector3(bonePosition.X, this.m_bbBottom, bonePosition.Z), ref worldMatrix);
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS)
        MyRenderProxy.DebugDrawLine3D(vector3D1 - down, vector3D1 + down, Color.Red, Color.Yellow, false);
      if (key == 0)
      {
        using (MyUtils.ReuseCollection<MyPhysics.HitInfo>(ref this.m_raycastHits))
        {
          MyPhysics.CastRay(vector3D1 - down, vector3D1 + down, this.m_raycastHits, 18);
          return this.ProcessResult(this.m_raycastHits, out terrainHeight, out terrainNormal);
        }
      }
      else
      {
        MyEntityTerrainHeightProviderComponent.AsyncQuery asyncQuery;
        if (!this.m_cachedTasks.TryGetValue(key, out asyncQuery))
          this.m_cachedTasks[key] = asyncQuery = new MyEntityTerrainHeightProviderComponent.AsyncQuery(this);
        if (Interlocked.Exchange(ref asyncQuery.Queued, 1) == 0)
        {
          Vector3D from = vector3D1 - down;
          Vector3D to = vector3D1 + down;
          asyncQuery.ProcessQuery();
          MyPhysics.CastRayParallel(ref from, ref to, asyncQuery.HitList, 18, new Action<List<MyPhysics.HitInfo>>(asyncQuery.Complete));
        }
        if (asyncQuery.HasValue)
        {
          terrainHeight = asyncQuery.Height;
          terrainNormal = asyncQuery.Normal;
          if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS)
          {
            Vector3D vector3D2 = Vector3D.Transform(bonePosition + new Vector3(0.0f, terrainHeight, 0.0f), worldMatrix);
            MyRenderProxy.DebugDrawSphere(vector3D2, 0.05f, Color.Yellow, depthRead: false);
            MyRenderProxy.DebugDrawArrow3DDir(vector3D2, Vector3D.TransformNormal(terrainNormal, worldMatrix), Color.Yellow);
          }
          return true;
        }
        terrainHeight = this.m_bbBottom;
        terrainNormal = Vector3.Zero;
        return false;
      }
    }

    private bool ProcessResult(
      List<MyPhysics.HitInfo> hits,
      out float terrainHeight,
      out Vector3 terrainNormal)
    {
      foreach (MyPhysics.HitInfo hit in hits)
      {
        if (!((HkReferenceObject) hit.HkHitInfo.Body == (HkReferenceObject) null) && !hit.HkHitInfo.Body.IsDisposed)
        {
          IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
          if (hitEntity != this.Entity && !(hitEntity is MyCharacter))
          {
            if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS)
              MyRenderProxy.DebugDrawSphere(hit.Position, 0.05f, Color.Red, depthRead: false);
            Vector3D vector3D = Vector3D.Transform(hit.Position, this.m_worldMatrixInv);
            terrainHeight = (float) vector3D.Y - this.m_bbBottom;
            float convexRadius = hit.HkHitInfo.GetConvexRadius();
            terrainHeight -= (double) convexRadius < 0.0599999986588955 ? convexRadius : 0.06f;
            terrainNormal = (Vector3) Vector3D.Transform(hit.HkHitInfo.Normal, this.m_worldMatrixInv.GetOrientation());
            return true;
          }
        }
      }
      terrainHeight = 0.0f;
      terrainNormal = Vector3.Zero;
      return false;
    }

    float IMyTerrainHeightProvider.GetReferenceTerrainHeight() => this.m_bbBottom;

    private class AsyncQuery
    {
      public float Height;
      public Vector3 Normal = Vector3.Up;
      public bool HasValue;
      public int Queued;
      public List<MyPhysics.HitInfo> HitList = new List<MyPhysics.HitInfo>(32);
      private MyEntityTerrainHeightProviderComponent m_component;

      public AsyncQuery(MyEntityTerrainHeightProviderComponent component)
      {
        this.m_component = component;
        this.Height = ((IMyTerrainHeightProvider) this.m_component).GetReferenceTerrainHeight();
      }

      public void Complete(List<MyPhysics.HitInfo> hits) => this.Queued = 0;

      public void ProcessQuery()
      {
        float terrainHeight;
        Vector3 terrainNormal;
        this.HasValue = this.m_component.ProcessResult(this.HitList, out terrainHeight, out terrainNormal);
        this.Height = terrainHeight;
        this.Normal = terrainNormal;
        this.HitList.Clear();
      }
    }

    private class Sandbox_Game_EntityComponents_MyEntityTerrainHeightProviderComponent\u003C\u003EActor : IActivator, IActivator<MyEntityTerrainHeightProviderComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityTerrainHeightProviderComponent();

      MyEntityTerrainHeightProviderComponent IActivator<MyEntityTerrainHeightProviderComponent>.CreateInstance() => new MyEntityTerrainHeightProviderComponent();
    }
  }
}
