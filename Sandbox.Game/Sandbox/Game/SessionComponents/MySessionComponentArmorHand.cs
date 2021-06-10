// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentArmorHand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MySessionComponentArmorHand : MySessionComponentBase
  {
    private MyCubeGrid m_lastCubeGrid;
    private Vector3I? m_lastBone;
    private Vector3I? m_lastCube;
    private Vector3D m_localBonePosition;
    private MyCubeGrid m_movingCubeGrid;
    private Vector3I? m_movingBone;

    public override void Draw()
    {
      base.Draw();
      if (!MyFakes.ENABLE_ARMOR_HAND)
        return;
      Vector3 forwardVector = MySector.MainCamera.ForwardVector;
      Vector3D position1 = MySector.MainCamera.Position;
      Vector3D linePointB = position1 + forwardVector * 100f;
      this.m_lastCubeGrid = (MyCubeGrid) null;
      this.m_lastBone = new Vector3I?();
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(position1, linePointB, 29);
      if (!((nullable.HasValue ? ((MyPhysicsComponentBase) nullable.Value.HkHitInfo.Body.UserObject).Entity : (IMyEntity) null) is MyCubeGrid myCubeGrid))
        return;
      this.m_lastCubeGrid = myCubeGrid;
      double num1 = double.MaxValue;
      LineD line = new LineD(position1, linePointB);
      Vector3I position2 = new Vector3I();
      double maxValue = double.MaxValue;
      this.m_lastCube = !this.m_lastCubeGrid.GetLineIntersectionExactGrid(ref line, ref position2, ref maxValue) ? new Vector3I?() : new Vector3I?(position2);
      foreach (KeyValuePair<Vector3I, Vector3> bone in myCubeGrid.Skeleton.Bones)
      {
        Vector3D point = Vector3D.Transform((Vector3D) (bone.Key / 2f) * (double) myCubeGrid.GridSize + bone.Value - new Vector3D((double) myCubeGrid.GridSize / 2.0), myCubeGrid.PositionComp.WorldMatrixRef);
        Color color = Color.Red;
        if (MyUtils.GetPointLineDistance(ref position1, ref linePointB, ref point) < 0.100000001490116)
        {
          double num2 = (position1 - point).LengthSquared();
          if (num2 < num1)
          {
            num1 = num2;
            color = Color.Blue;
            this.m_lastBone = new Vector3I?(bone.Key);
          }
        }
        MyRenderProxy.DebugDrawSphere(point, 0.05f, (Color) color.ToVector3(), 0.5f, false, true);
      }
    }

    public override void HandleInput()
    {
      base.HandleInput();
      if (!MyFakes.ENABLE_ARMOR_HAND)
        return;
      Vector3? nullable1;
      if (MyInput.Static.IsNewLeftMousePressed() && this.m_lastCubeGrid != null && this.m_lastBone.HasValue)
      {
        Vector3I? lastBone = this.m_lastBone;
        float num = 2f;
        nullable1 = lastBone.HasValue ? new Vector3?(lastBone.GetValueOrDefault() / num) : new Vector3?();
        this.m_localBonePosition = Vector3.Transform((Vector3) Vector3D.Transform((Vector3D) nullable1.Value * (double) this.m_lastCubeGrid.GridSize + this.m_lastCubeGrid.Skeleton.Bones[this.m_lastBone.Value] - new Vector3D((double) this.m_lastCubeGrid.GridSize / 2.0), this.m_lastCubeGrid.PositionComp.WorldMatrixRef), MySession.Static.LocalCharacter.PositionComp.WorldMatrixNormalizedInv);
        this.m_movingCubeGrid = this.m_lastCubeGrid;
        this.m_movingBone = this.m_lastBone;
        this.m_lastCubeGrid.Skeleton.GetDefinitionOffsetWithNeighbours(this.m_lastCube.Value, this.m_movingBone.Value, this.m_lastCubeGrid);
      }
      if (MyInput.Static.IsLeftMousePressed() && this.m_movingCubeGrid != null && this.m_movingBone.HasValue)
      {
        if (MyInput.Static.IsAnyShiftKeyPressed())
        {
          this.m_movingCubeGrid.Skeleton.Bones[this.m_movingBone.Value] = this.GetBoneOnSphere(new Vector3I(2, 0, 0), this.m_movingBone.Value, this.m_movingCubeGrid);
        }
        else
        {
          Vector3D vector3D1 = Vector3D.Transform(this.m_localBonePosition, MySession.Static.LocalCharacter.PositionComp.WorldMatrixRef);
          Vector3D vector3D2 = Vector3D.Transform(vector3D1, this.m_movingCubeGrid.PositionComp.WorldMatrixInvScaled) + new Vector3D((double) this.m_movingCubeGrid.GridSize / 2.0);
          ConcurrentDictionary<Vector3I, Vector3> bones = this.m_movingCubeGrid.Skeleton.Bones;
          Vector3I key = this.m_movingBone.Value;
          Vector3D vector3D3 = vector3D2;
          Vector3I? movingBone = this.m_movingBone;
          float num = 2f;
          Vector3? nullable2;
          if (!movingBone.HasValue)
          {
            nullable1 = new Vector3?();
            nullable2 = nullable1;
          }
          else
            nullable2 = new Vector3?(movingBone.GetValueOrDefault() / num);
          nullable1 = nullable2;
          Vector3D vector3D4 = (Vector3D) nullable1.Value * (double) this.m_movingCubeGrid.GridSize;
          Vector3 vector3 = (Vector3) (vector3D3 - vector3D4);
          bones[key] = vector3;
          Vector3I gridInteger = this.m_movingCubeGrid.WorldToGridInteger(vector3D1);
          for (int x = -1; x <= 1; ++x)
          {
            for (int y = -1; y <= 1; ++y)
            {
              for (int z = -1; z <= 1; ++z)
                this.m_movingCubeGrid.SetCubeDirty(new Vector3I(x, y, z) + gridInteger);
            }
          }
        }
      }
      if (!MyInput.Static.IsNewLeftMouseReleased())
        return;
      this.m_movingCubeGrid = (MyCubeGrid) null;
      this.m_movingBone = new Vector3I?();
    }

    private Vector3D BoneToWorld(Vector3I bone, Vector3 offset, MyCubeGrid grid) => Vector3D.Transform((Vector3D) (bone / 2f) * (double) grid.GridSize + offset - new Vector3D((double) grid.GridSize / 2.0), grid.PositionComp.WorldMatrixRef);

    private Vector3 GetBoneOnSphere(Vector3I center, Vector3I bonePos, MyCubeGrid grid)
    {
      Vector3D world1 = this.BoneToWorld(center, Vector3.Zero, grid);
      Vector3D world2 = this.BoneToWorld(bonePos, Vector3.Zero, grid);
      BoundingSphereD boundingSphereD = new BoundingSphereD(world1, (double) grid.GridSize);
      Vector3D direction = world1 - world2;
      direction.Normalize();
      RayD ray = new RayD(world2, direction);
      double tmin;
      return boundingSphereD.IntersectRaySphere(ray, out tmin, out double _) ? (Vector3) (Vector3D.Transform(world2 + direction * tmin, grid.PositionComp.WorldMatrixInvScaled) + new Vector3D((double) grid.GridSize / 2.0) - (Vector3D) (bonePos / 2f) * (double) grid.GridSize) : Vector3.Zero;
    }
  }
}
