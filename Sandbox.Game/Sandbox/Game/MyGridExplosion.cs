// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyGridExplosion
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game
{
  public class MyGridExplosion
  {
    public bool GridWasHit;
    public readonly HashSet<MyCubeGrid> AffectedCubeGrids = new HashSet<MyCubeGrid>();
    public readonly HashSet<MySlimBlock> AffectedCubeBlocks = new HashSet<MySlimBlock>();
    private Dictionary<MySlimBlock, float> m_damagedBlocks = new Dictionary<MySlimBlock, float>();
    private Dictionary<MySlimBlock, MyGridExplosion.MyRaycastDamageInfo> m_damageRemaining = new Dictionary<MySlimBlock, MyGridExplosion.MyRaycastDamageInfo>();
    private Stack<MySlimBlock> m_castBlocks = new Stack<MySlimBlock>();
    private BoundingSphereD m_explosion;
    private float m_explosionDamage;
    private int stackOverflowGuard;
    private const int MAX_PHYSICS_RECURSION_COUNT = 10;
    private List<Vector3I> m_cells = new List<Vector3I>();

    public Dictionary<MySlimBlock, float> DamagedBlocks => this.m_damagedBlocks;

    public Dictionary<MySlimBlock, MyGridExplosion.MyRaycastDamageInfo> DamageRemaining => this.m_damageRemaining;

    public float Damage => this.m_explosionDamage;

    public BoundingSphereD Sphere => this.m_explosion;

    public void Init(BoundingSphereD explosion, float explosionDamage)
    {
      this.m_explosion = explosion;
      this.m_explosionDamage = explosionDamage;
    }

    public void Clear()
    {
      this.AffectedCubeBlocks.Clear();
      this.AffectedCubeGrids.Clear();
      this.m_damageRemaining.Clear();
      this.m_damagedBlocks.Clear();
      this.m_castBlocks.Clear();
    }

    public void ComputeDamagedBlocks()
    {
      foreach (MySlimBlock affectedCubeBlock in this.AffectedCubeBlocks)
      {
        this.m_castBlocks.Clear();
        MyGridExplosion.MyRaycastDamageInfo raycastDamageInfo = this.CastDDA(affectedCubeBlock);
        while (this.m_castBlocks.Count > 0)
        {
          MySlimBlock key = this.m_castBlocks.Pop();
          if (key.FatBlock is MyWarhead)
          {
            this.m_damagedBlocks[key] = 1E+07f;
          }
          else
          {
            float num1 = (float) (key.WorldAABB.Center - this.m_explosion.Center).Length();
            if ((double) raycastDamageInfo.DamageRemaining > 0.0)
            {
              float num2 = MathHelper.Clamp((float) (1.0 - ((double) num1 - (double) raycastDamageInfo.DistanceToExplosion) / (this.m_explosion.Radius - (double) raycastDamageInfo.DistanceToExplosion)), 0.0f, 1f);
              if ((double) num2 > 0.0)
              {
                this.m_damagedBlocks.Add(key, raycastDamageInfo.DamageRemaining * num2 * key.DeformationRatio);
                raycastDamageInfo.DamageRemaining = Math.Max(0.0f, (float) ((double) raycastDamageInfo.DamageRemaining * (double) num2 - (double) key.Integrity / (double) key.DeformationRatio));
              }
              else
                this.m_damagedBlocks.Add(key, raycastDamageInfo.DamageRemaining);
            }
            else
              raycastDamageInfo.DamageRemaining = 0.0f;
            raycastDamageInfo.DistanceToExplosion = Math.Abs(num1);
            this.m_damageRemaining.Add(key, raycastDamageInfo);
          }
        }
      }
    }

    public MyGridExplosion.MyRaycastDamageInfo ComputeDamageForEntity(
      Vector3D worldPosition)
    {
      return new MyGridExplosion.MyRaycastDamageInfo(this.m_explosionDamage, (float) (worldPosition - this.m_explosion.Center).Length());
    }

    private MyGridExplosion.MyRaycastDamageInfo CastDDA(MySlimBlock cubeBlock)
    {
      if (this.m_damageRemaining.ContainsKey(cubeBlock))
        return this.m_damageRemaining[cubeBlock];
      this.stackOverflowGuard = 0;
      this.m_castBlocks.Push(cubeBlock);
      Vector3D worldCenter;
      cubeBlock.ComputeWorldCenter(out worldCenter);
      this.m_cells.Clear();
      cubeBlock.CubeGrid.RayCastCells(worldCenter, this.m_explosion.Center, this.m_cells, new Vector3I?(), false, true);
      (this.m_explosion.Center - worldCenter).Normalize();
      foreach (Vector3I cell in this.m_cells)
      {
        Vector3D fromWorldPos = Vector3D.Transform(cell * cubeBlock.CubeGrid.GridSize, cubeBlock.CubeGrid.WorldMatrix);
        int num = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_DDA_RAYCASTS ? 1 : 0;
        MySlimBlock cubeBlock1 = cubeBlock.CubeGrid.GetCubeBlock(cell);
        if (cubeBlock1 == null)
          return this.IsExplosionInsideCell(cell, cubeBlock.CubeGrid) ? new MyGridExplosion.MyRaycastDamageInfo(this.m_explosionDamage, (float) (fromWorldPos - this.m_explosion.Center).Length()) : this.CastPhysicsRay(fromWorldPos);
        if (cubeBlock1 != cubeBlock)
        {
          if (this.m_damageRemaining.ContainsKey(cubeBlock1))
            return this.m_damageRemaining[cubeBlock1];
          if (!this.m_castBlocks.Contains(cubeBlock1))
            this.m_castBlocks.Push(cubeBlock1);
        }
        else if (this.IsExplosionInsideCell(cell, cubeBlock.CubeGrid))
          return new MyGridExplosion.MyRaycastDamageInfo(this.m_explosionDamage, (float) (fromWorldPos - this.m_explosion.Center).Length());
      }
      return new MyGridExplosion.MyRaycastDamageInfo(this.m_explosionDamage, (float) (worldCenter - this.m_explosion.Center).Length());
    }

    private bool IsExplosionInsideCell(Vector3I cell, MyCubeGrid cellGrid) => cellGrid.WorldToGridInteger(this.m_explosion.Center) == cell;

    private MyGridExplosion.MyRaycastDamageInfo CastPhysicsRay(Vector3D fromWorldPos)
    {
      Vector3D position = Vector3D.Zero;
      IMyEntity myEntity = (IMyEntity) null;
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(fromWorldPos, this.m_explosion.Center, 29);
      if (nullable.HasValue)
      {
        myEntity = nullable.Value.HkHitInfo.Body.UserObject != null ? ((MyPhysicsComponentBase) nullable.Value.HkHitInfo.Body.UserObject).Entity : (IMyEntity) null;
        position = nullable.Value.Position;
      }
      Vector3D normal = this.m_explosion.Center - fromWorldPos;
      float distanceToExplosion = (float) normal.Normalize();
      if (!(myEntity is MyCubeGrid myCubeGrid) && myEntity is MyCubeBlock myCubeBlock)
        myCubeGrid = myCubeBlock.CubeGrid;
      if (myCubeGrid != null)
      {
        Vector3D vector3D1 = Vector3D.Transform(position, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) * (double) myCubeGrid.GridSizeR;
        Vector3D vector3D2 = Vector3D.TransformNormal(normal, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) * 1.0 / 8.0;
        for (int index = 0; index < 5; ++index)
        {
          Vector3I pos = Vector3I.Round(vector3D1);
          MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(pos);
          if (cubeBlock != null)
            return this.m_castBlocks.Contains(cubeBlock) ? new MyGridExplosion.MyRaycastDamageInfo(0.0f, distanceToExplosion) : this.CastDDA(cubeBlock);
          vector3D1 += vector3D2;
        }
        Vector3D fromWorldPos1 = Vector3D.Transform(vector3D1 * (double) myCubeGrid.GridSize, myCubeGrid.WorldMatrix);
        if (new BoundingBoxD(Vector3D.Min(fromWorldPos, fromWorldPos1), Vector3D.Max(fromWorldPos, fromWorldPos1)).Contains(this.m_explosion.Center) == ContainmentType.Contains)
          return new MyGridExplosion.MyRaycastDamageInfo(this.m_explosionDamage, distanceToExplosion);
        ++this.stackOverflowGuard;
        if (this.stackOverflowGuard > 10)
        {
          int num = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS ? 1 : 0;
          return new MyGridExplosion.MyRaycastDamageInfo(0.0f, distanceToExplosion);
        }
        int num1 = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS ? 1 : 0;
        return this.CastPhysicsRay(fromWorldPos1);
      }
      if (!nullable.HasValue)
        return new MyGridExplosion.MyRaycastDamageInfo(this.m_explosionDamage, distanceToExplosion);
      int num2 = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS ? 1 : 0;
      return new MyGridExplosion.MyRaycastDamageInfo(0.0f, distanceToExplosion);
    }

    [Conditional("DEBUG")]
    private void DrawRay(Vector3D from, Vector3D to, float damage, bool depthRead = true)
    {
      if ((double) damage <= 0.0)
      {
        Color blue = Color.Blue;
      }
      else
        Color.Lerp(Color.Green, Color.Red, damage / this.m_explosionDamage);
    }

    [Conditional("DEBUG")]
    private void DrawRay(Vector3D from, Vector3D to, Color color, bool depthRead = true)
    {
      if (MyAlexDebugInputComponent.Static == null)
        return;
      MyAlexDebugInputComponent.Static.AddDebugLine(new MyAlexDebugInputComponent.LineInfo((Vector3) from, (Vector3) to, color, false));
    }

    public struct MyRaycastDamageInfo
    {
      public float DamageRemaining;
      public float DistanceToExplosion;

      public MyRaycastDamageInfo(float damageRemaining, float distanceToExplosion)
      {
        this.DamageRemaining = damageRemaining;
        this.DistanceToExplosion = distanceToExplosion;
      }
    }
  }
}
