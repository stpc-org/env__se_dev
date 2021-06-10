// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MySnapshotCache
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Groups;
using VRageMath;

namespace Sandbox.Game.Replication.History
{
  public static class MySnapshotCache
  {
    public static long DEBUG_ENTITY_ID;
    public static bool PROPAGATE_TO_CONNECTIONS = true;
    private static readonly Dictionary<MyEntity, MySnapshotCache.MyItem> m_cache = new Dictionary<MyEntity, MySnapshotCache.MyItem>();

    public static void Add(
      MyEntity entity,
      ref MySnapshot snapshot,
      MySnapshotFlags snapshotFlags,
      bool reset)
    {
      MySnapshotCache.m_cache[entity] = new MySnapshotCache.MyItem()
      {
        Snapshot = snapshot,
        SnapshotFlags = snapshotFlags,
        Reset = reset
      };
    }

    public static void Apply()
    {
      foreach (KeyValuePair<MyEntity, MySnapshotCache.MyItem> keyValuePair in MySnapshotCache.m_cache)
      {
        MyEntity key = keyValuePair.Key;
        if (!key.Closed && !key.MarkedForClose)
        {
          if (MyFakes.SNAPSHOTCACHE_HIERARCHY)
          {
            MyEntity myEntity = key;
            do
            {
              myEntity = MySnapshot.GetParent(myEntity, out bool _);
            }
            while (myEntity != null && !MySnapshotCache.m_cache.ContainsKey(myEntity));
            if (myEntity != null)
              continue;
          }
          MySnapshotCache.MyItem myItem = keyValuePair.Value;
          if (myItem.SnapshotFlags.ApplyPhysicsLinear || myItem.SnapshotFlags.ApplyPhysicsAngular)
            MySnapshotCache.ApplyPhysics(key, myItem);
          bool applyPosition = myItem.SnapshotFlags.ApplyPosition;
          bool applyRotation = myItem.SnapshotFlags.ApplyRotation;
          if (applyPosition | applyRotation)
          {
            MatrixD mat;
            myItem.Snapshot.GetMatrix(key, out mat, applyPosition, applyRotation);
            bool reset = MySnapshot.ApplyReset && myItem.Reset;
            MyCubeGrid myCubeGrid = key as MyCubeGrid;
            if (MyFakes.SNAPSHOTCACHE_HIERARCHY && myCubeGrid != null)
            {
              MatrixD diffMat;
              Vector3 diffPos;
              MySnapshotCache.CalculateDiffs(key, ref mat, out diffMat, out diffPos);
              MySnapshotCache.ApplyChildMatrix((MyEntity) myCubeGrid, ref mat, ref diffMat, ref diffPos, reset);
            }
            else
              MySnapshotCache.ApplyChildMatrixLite(key, ref mat, reset);
          }
        }
      }
      MySnapshotCache.m_cache.Clear();
    }

    private static void CalculateDiffs(
      MyEntity entity,
      ref MatrixD mat,
      out MatrixD diffMat,
      out Vector3 diffPos)
    {
      MatrixD matrixD = entity.PositionComp.WorldMatrixInvScaled;
      diffMat = matrixD * mat;
      Vector3 position = entity.Physics == null || !((HkReferenceObject) entity.Physics.RigidBody != (HkReferenceObject) null) ? entity.PositionComp.LocalAABB.Center : entity.Physics.CenterOfMassLocal;
      Vector3D result1;
      Vector3D.Transform(ref position, ref mat, out result1);
      MatrixD matrix = entity.PositionComp.WorldMatrixRef;
      Vector3D result2;
      Vector3D.Transform(ref position, ref matrix, out result2);
      diffPos = (Vector3) (result1 - result2);
    }

    private static void ApplyPhysics(MyEntity entity, MySnapshotCache.MyItem value)
    {
      bool applyPhysicsLinear = value.SnapshotFlags.ApplyPhysicsLinear;
      bool applyPhysicsAngular = value.SnapshotFlags.ApplyPhysicsAngular;
      value.Snapshot.ApplyPhysics(entity, applyPhysicsAngular, applyPhysicsLinear, value.SnapshotFlags.ApplyPhysicsLocal);
    }

    private static void PropagateToConnections(
      MyEntity grid,
      ref MatrixD diffMat,
      ref Vector3 diffPos,
      bool reset)
    {
      MatrixD localDiffMat = diffMat;
      Vector3 localDiffPos = diffPos;
      MyGridPhysicalHierarchy.Static.ApplyOnAllChildren(grid, (Action<MyEntity>) (child => MySnapshotCache.PropagateToChild(child, localDiffMat, localDiffPos, reset)));
    }

    private static void PropagateToChild(
      MyEntity child,
      MatrixD diffMat,
      Vector3 diffPos,
      bool reset)
    {
      MatrixD mat = new MatrixD();
      bool flag = false;
      MySnapshotCache.MyItem myItem;
      bool inheritRotation;
      if (MySnapshotCache.m_cache.TryGetValue(child, out myItem))
      {
        bool applyPosition = myItem.SnapshotFlags.ApplyPosition;
        bool applyRotation = myItem.SnapshotFlags.ApplyRotation;
        inheritRotation = myItem.SnapshotFlags.InheritRotation;
        if (applyPosition | applyRotation)
        {
          MatrixD newChildMatrix;
          if (applyPosition != applyRotation)
            MySnapshotCache.CalculateMatrix(child, ref diffMat, ref diffPos, inheritRotation, out newChildMatrix);
          else
            newChildMatrix = child.WorldMatrix;
          myItem.Snapshot.GetMatrix(out mat, ref newChildMatrix, applyPosition, applyRotation);
          MySnapshotCache.CalculateDiffs(child, ref mat, out diffMat, out diffPos);
          flag = true;
        }
        if (myItem.SnapshotFlags.ApplyPhysicsLinear || myItem.SnapshotFlags.ApplyPhysicsAngular)
          MySnapshotCache.ApplyPhysics(child, myItem);
        reset |= myItem.Reset;
      }
      else
        inheritRotation = child.LastSnapshotFlags == null || child.LastSnapshotFlags.InheritRotation;
      if (!flag)
        MySnapshotCache.CalculateMatrix(child, ref diffMat, ref diffPos, inheritRotation, out mat);
      MySnapshotCache.ApplyChildMatrix(child, ref mat, ref diffMat, ref diffPos, reset);
    }

    private static void CalculateMatrix(
      MyEntity child,
      ref MatrixD diffMat,
      ref Vector3 diffPos,
      bool inheritRotation,
      out MatrixD newChildMatrix)
    {
      if (inheritRotation)
      {
        if (child is MyCharacter myCharacter && (double) myCharacter.Gravity.LengthSquared() > 0.100000001490116)
        {
          if (myCharacter.Physics != null && myCharacter.Physics.CharacterProxy != null)
          {
            if (myCharacter.Physics.CharacterProxy.Supported)
            {
              Vector3D up1 = child.WorldMatrix.Up;
              newChildMatrix = child.WorldMatrix * diffMat;
              Vector3D up2 = newChildMatrix.Up;
              double d = up1.Dot(ref up2);
              if (Math.Abs(Math.Abs(d) - 1.0) > 9.99999974737875E-05)
              {
                double angle = -Math.Acos(d);
                Vector3D result1;
                Vector3D.Cross(ref up1, ref up2, out result1);
                result1.Normalize();
                MatrixD result2;
                MatrixD.CreateFromAxisAngle(ref result1, angle, out result2);
                Vector3D translation = newChildMatrix.Translation;
                newChildMatrix.Translation = Vector3D.Zero;
                MatrixD.Multiply(ref newChildMatrix, ref result2, out newChildMatrix);
                newChildMatrix.Translation = translation;
              }
            }
            else
            {
              newChildMatrix = child.WorldMatrix;
              newChildMatrix.Translation += diffPos;
            }
          }
          else
            newChildMatrix = child.WorldMatrix * diffMat;
        }
        else
          newChildMatrix = child.WorldMatrix * diffMat;
        newChildMatrix.Orthogonalize();
      }
      else
      {
        newChildMatrix = child.WorldMatrix;
        newChildMatrix.Translation += diffPos;
      }
    }

    private static void ApplyChildMatrix(
      MyEntity child,
      ref MatrixD mat,
      ref MatrixD diffMat,
      ref Vector3 diffPos,
      bool reset)
    {
      MySnapshotCache.ApplyChildMatrixLite(child, ref mat, reset);
      if (!MySnapshotCache.PROPAGATE_TO_CONNECTIONS)
        return;
      MySnapshotCache.PropagateToConnections(child, ref diffMat, ref diffPos, reset);
    }

    private static void ApplyChildMatrixLite(MyEntity child, ref MatrixD mat, bool reset)
    {
      if (child is MyCubeGrid node && node.IsStatic)
        return;
      if (MySession.Static.CameraController is MyEntity cameraController && (child == cameraController || child == cameraController.GetTopMostParent((System.Type) null)))
      {
        MatrixD transformDelta = child.PositionComp.WorldMatrixInvScaled * mat;
        MyThirdPersonSpectator.Static.CompensateQuickTransformChange(ref transformDelta);
      }
      child.m_positionResetFromServer = reset;
      child.PositionComp.SetWorldMatrix(ref mat, (object) MyGridPhysicalHierarchy.Static, forceUpdateAllChildren: reset);
      if (node == null || !node.InScene)
        return;
      MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node1 = MyCubeGridGroups.Static.Mechanical.GetNode(node);
      foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> parentLink in node1.ParentLinks)
      {
        if (MyEntities.GetEntityById(parentLink.Key) is MyPistonBase entityById)
          entityById.SetCurrentPosByTopGridMatrix();
      }
      foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> childLink in node1.ChildLinks)
      {
        if (MyEntities.GetEntityById(childLink.Key) is MyPistonBase entityById)
          entityById.SetCurrentPosByTopGridMatrix();
      }
    }

    private struct MyItem
    {
      public MySnapshot Snapshot;
      public MySnapshotFlags SnapshotFlags;
      public bool Reset;
    }
  }
}
