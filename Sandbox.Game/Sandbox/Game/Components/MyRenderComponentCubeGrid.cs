// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentCubeGrid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  public class MyRenderComponentCubeGrid : MyRenderComponent
  {
    private static readonly MyStringId ID_RED_DOT_IGNORE_DEPTH = MyStringId.GetOrCompute("RedDotIgnoreDepth");
    private static readonly MyStringId ID_WEAPON_LASER_IGNORE_DEPTH = MyStringId.GetOrCompute("WeaponLaserIgnoreDepth");
    private static readonly List<MyPhysics.HitInfo> m_tmpHitList = new List<MyPhysics.HitInfo>();
    private MyCubeGrid m_grid;
    private bool m_deferRenderRelease;
    private bool m_shouldReleaseRenderObjects;
    private MyCubeGridRenderData m_renderData;
    private MyParticleEffect m_atmosphericEffect;
    private const float m_atmosphericEffectMinSpeed = 75f;
    private const float m_atmosphericEffectMinFade = 0.85f;
    private const int m_atmosphericEffectVoxelContactDelay = 5000;
    private int m_lastVoxelContactTime;
    private float m_lastWorkingIntersectDistance;
    private static List<Vector3> m_tmpCornerList = new List<Vector3>();

    public MyRenderComponentCubeGrid() => this.m_renderData = new MyCubeGridRenderData(this);

    public MyCubeGrid CubeGrid => this.m_grid;

    public bool DeferRenderRelease
    {
      get => this.m_deferRenderRelease;
      set
      {
        this.m_deferRenderRelease = value;
        if (value || !this.m_shouldReleaseRenderObjects)
          return;
        this.RemoveRenderObjects();
      }
    }

    public MyCubeGridRenderData RenderData => this.m_renderData;

    public MyCubeSize GridSizeEnum => this.m_grid.GridSizeEnum;

    public float GridSize => this.m_grid.GridSize;

    public bool IsStatic => this.m_grid.IsStatic;

    public void RebuildDirtyCells() => this.m_renderData.RebuildDirtyCells(this.GetRenderFlags());

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_grid = this.Container.Entity as MyCubeGrid;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (this.m_atmosphericEffect == null)
        return;
      MyParticlesManager.RemoveParticleEffect(this.m_atmosphericEffect);
      this.m_atmosphericEffect = (MyParticleEffect) null;
    }

    public override void Draw()
    {
      base.Draw();
      this.DrawBlocks();
      if (MyCubeGrid.ShowCenterOfMass && !this.IsStatic && (this.Container.Entity.Physics != null && this.Container.Entity.Physics.HasRigidBody))
        this.DrawCenterOfMass();
      if (MyCubeGrid.ShowGridPivot)
        this.DrawGridPivot();
      if (!this.m_grid.MarkedAsTrash)
        return;
      this.DrawMarkedAsTrash();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void DrawMarkedAsTrash()
    {
      BoundingBoxD localAabb = (BoundingBoxD) this.m_grid.PositionComp.LocalAABB;
      localAabb.Max += 0.2f;
      localAabb.Min -= 0.200000002980232;
      MatrixD worldMatrix = this.m_grid.PositionComp.WorldMatrixRef;
      Color red = Color.Red;
      red.A = (byte) (100.0 * (Math.Sin((double) this.m_grid.TrashHighlightCounter / 10.0) + 1.0) / 2.0 + 100.0);
      red.R = (byte) (200.0 * (Math.Sin((double) this.m_grid.TrashHighlightCounter / 10.0) + 1.0) / 2.0 + 50.0);
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localAabb, ref red, ref red, MySimpleObjectRasterizer.SolidAndWireframe, 1, 0.008f, blendType: MyBillboard.BlendTypeEnum.LDR);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void DrawGridPivot()
    {
      MatrixD worldMatrix = this.Container.Entity.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      Vector3D position = MySector.MainCamera.Position;
      float num1 = Vector3.Distance((Vector3) position, (Vector3) translation);
      bool flag = false;
      if ((double) num1 < 30.0)
        flag = true;
      else if ((double) num1 < 200.0)
      {
        flag = true;
        MyPhysics.CastRay(position, translation, MyRenderComponentCubeGrid.m_tmpHitList, 16);
        foreach (MyPhysics.HitInfo tmpHit in MyRenderComponentCubeGrid.m_tmpHitList)
        {
          if (tmpHit.HkHitInfo.GetHitEntity() != this)
          {
            flag = false;
            break;
          }
        }
        MyRenderComponentCubeGrid.m_tmpHitList.Clear();
      }
      if (!flag)
        return;
      float num2 = MathHelper.Lerp(1f, 9f, num1 / 200f);
      MyStringId laserIgnoreDepth = MyRenderComponentCubeGrid.ID_WEAPON_LASER_IGNORE_DEPTH;
      float thickness = 0.02f * num2;
      Vector4 vector4_1 = Color.Green.ToVector4();
      MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix.Up * 0.5 * (double) num2, new MyStringId?(laserIgnoreDepth), ref vector4_1, thickness);
      Vector4 vector4_2 = Color.Blue.ToVector4();
      MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix.Forward * 0.5 * (double) num2, new MyStringId?(laserIgnoreDepth), ref vector4_2, thickness);
      Color color = Color.Red;
      vector4_2 = color.ToVector4();
      MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix.Right * 0.5 * (double) num2, new MyStringId?(laserIgnoreDepth), ref vector4_2, thickness);
      MyStringId redDotIgnoreDepth = MyRenderComponentCubeGrid.ID_RED_DOT_IGNORE_DEPTH;
      color = Color.White;
      Vector4 vector4_3 = color.ToVector4();
      Vector3D origin = translation;
      Vector3 leftVector = MySector.MainCamera.LeftVector;
      Vector3 upVector = MySector.MainCamera.UpVector;
      double num3 = 0.100000001490116 * (double) num2;
      MyTransparentGeometry.AddBillboardOriented(redDotIgnoreDepth, vector4_3, origin, leftVector, upVector, (float) num3);
      MyRenderProxy.DebugDrawAxis(worldMatrix, 0.5f, false);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void DrawCenterOfMass()
    {
      MatrixD worldMatrix = this.Container.Entity.Physics.GetWorldMatrix();
      Vector3D centerOfMassWorld = this.Container.Entity.Physics.CenterOfMassWorld;
      Vector3D position = MySector.MainCamera.Position;
      float num1 = Vector3.Distance((Vector3) position, (Vector3) centerOfMassWorld);
      bool flag = false;
      if ((double) num1 < 30.0)
        flag = true;
      else if ((double) num1 < 200.0)
      {
        flag = true;
        MyPhysics.CastRay(position, centerOfMassWorld, MyRenderComponentCubeGrid.m_tmpHitList, 16);
        foreach (MyPhysics.HitInfo tmpHit in MyRenderComponentCubeGrid.m_tmpHitList)
        {
          if (tmpHit.HkHitInfo.GetHitEntity() != this)
          {
            flag = false;
            break;
          }
        }
        MyRenderComponentCubeGrid.m_tmpHitList.Clear();
      }
      if (!flag)
        return;
      float num2 = MathHelper.Lerp(1f, 9f, num1 / 200f);
      MyStringId laserIgnoreDepth = MyRenderComponentCubeGrid.ID_WEAPON_LASER_IGNORE_DEPTH;
      Color color = Color.Yellow;
      Vector4 vector4_1 = color.ToVector4();
      float thickness = 0.02f * num2;
      MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Up * 0.5 * (double) num2, centerOfMassWorld + worldMatrix.Up * 0.5 * (double) num2, new MyStringId?(laserIgnoreDepth), ref vector4_1, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
      MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Forward * 0.5 * (double) num2, centerOfMassWorld + worldMatrix.Forward * 0.5 * (double) num2, new MyStringId?(laserIgnoreDepth), ref vector4_1, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
      MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Right * 0.5 * (double) num2, centerOfMassWorld + worldMatrix.Right * 0.5 * (double) num2, new MyStringId?(laserIgnoreDepth), ref vector4_1, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
      MyStringId redDotIgnoreDepth = MyRenderComponentCubeGrid.ID_RED_DOT_IGNORE_DEPTH;
      color = Color.White;
      Vector4 vector4_2 = color.ToVector4();
      Vector3D origin = centerOfMassWorld;
      Vector3 leftVector = MySector.MainCamera.LeftVector;
      Vector3 upVector = MySector.MainCamera.UpVector;
      double num3 = 0.100000001490116 * (double) num2;
      MyTransparentGeometry.AddBillboardOriented(redDotIgnoreDepth, vector4_2, origin, leftVector, upVector, (float) num3, MyBillboard.BlendTypeEnum.AdditiveTop);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void DrawBlocks()
    {
      this.m_grid.BlocksForDraw.ApplyChanges();
      foreach (MyCubeBlock myCubeBlock in this.m_grid.BlocksForDraw)
      {
        if (MyRenderProxy.VisibleObjectsRead.Contains(myCubeBlock.Render.RenderObjectIDs[0]))
          myCubeBlock.Render.Draw();
      }
    }

    public void ResetLastVoxelContactTimer() => this.m_lastVoxelContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;

    public override void AddRenderObjects()
    {
      MyCubeGrid entity = this.Container.Entity as MyCubeGrid;
      if (this.m_renderObjectIDs[0] != uint.MaxValue || !entity.IsDirty() && !this.m_renderData.HasDirtyCells)
        return;
      entity.UpdateInstanceData();
    }

    public override void RemoveRenderObjects()
    {
      if (this.m_deferRenderRelease)
      {
        this.m_shouldReleaseRenderObjects = true;
      }
      else
      {
        this.m_shouldReleaseRenderObjects = false;
        this.m_renderData.OnRemovedFromRender();
        for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
        {
          if (this.m_renderObjectIDs[index] != uint.MaxValue)
            this.ReleaseRenderObjectID(index);
        }
      }
    }

    protected override void UpdateRenderObjectVisibility(bool visible) => base.UpdateRenderObjectVisibility(visible);

    public void UpdateRenderObjectMatrices(Matrix matrix)
    {
      MatrixD worldMatrix = (MatrixD) ref matrix;
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
      {
        if (this.m_renderObjectIDs[index] != uint.MaxValue)
          MyRenderProxy.UpdateRenderObject(this.RenderObjectIDs[index], in worldMatrix, in BoundingBox.Invalid, false, this.LastMomentUpdateIndex);
      }
    }

    private class Sandbox_Game_Components_MyRenderComponentCubeGrid\u003C\u003EActor : IActivator, IActivator<MyRenderComponentCubeGrid>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentCubeGrid();

      MyRenderComponentCubeGrid IActivator<MyRenderComponentCubeGrid>.CreateInstance() => new MyRenderComponentCubeGrid();
    }
  }
}
