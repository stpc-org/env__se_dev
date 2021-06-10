// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyVoxelGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public static class MyVoxelGenerator
  {
    private const int CELL_SIZE = 16;
    private const int VOXEL_CLAMP_BORDER_DISTANCE = 2;
    [ThreadStatic]
    private static MyStorageData m_cache;
    private static readonly List<MyEntity> m_overlapList = new List<MyEntity>();

    public static void MakeCrater(
      MyVoxelBase voxelMap,
      BoundingSphereD sphere,
      Vector3 direction,
      MyVoxelMaterialDefinition material)
    {
      try
      {
        MyVoxelGenerator.MakeCraterInternal(voxelMap, ref sphere, ref direction, material);
      }
      catch (NullReferenceException ex)
      {
        MyLog.Default.Error("NRE while creating asteroid crater." + MyEnvironment.NewLine + (object) ex);
      }
    }

    private static void MakeCraterInternal(
      MyVoxelBase voxelMap,
      ref BoundingSphereD sphere,
      ref Vector3 direction,
      MyVoxelMaterialDefinition material)
    {
      Vector3 vector1 = Vector3.Normalize(sphere.Center - voxelMap.RootVoxel.WorldMatrix.Translation);
      Vector3D worldPosition1 = sphere.Center - (sphere.Radius - 1.0) * 1.29999995231628;
      Vector3D worldPosition2 = sphere.Center + (sphere.Radius + 1.0) * 1.29999995231628;
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref worldPosition1, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref worldPosition2, out voxelCoord2);
      voxelMap.Storage.ClampVoxelCoord(ref voxelCoord1);
      voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
      Vector3I lodVoxelRangeMin = voxelCoord1 + voxelMap.StorageMin;
      Vector3I lodVoxelRangeMax = voxelCoord2 + voxelMap.StorageMin;
      bool flag = false;
      if (MyVoxelGenerator.m_cache == null)
        MyVoxelGenerator.m_cache = new MyStorageData();
      MyVoxelGenerator.m_cache.Resize(voxelCoord1, voxelCoord2);
      MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
      voxelMap.Storage.ReadRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
      int num1 = 0;
      Vector3I p = (voxelCoord2 - voxelCoord1) / 2;
      byte materialIdx = MyVoxelGenerator.m_cache.Material(ref p);
      float num2 = 1f - Vector3.Dot(vector1, direction);
      Vector3 vector3_1 = (Vector3) (sphere.Center - vector1 * (float) sphere.Radius * 1.1f);
      float num3 = (float) (sphere.Radius * 1.5);
      float num4 = num3 * num3;
      float num5 = (float) (0.5 * (2.0 * (double) num3 + 0.5));
      float num6 = (float) (0.5 * (-2.0 * (double) num3 + 0.5));
      Vector3 vector3_2 = vector3_1 + vector1 * (float) sphere.Radius * (0.7f + num2) + direction * (float) sphere.Radius * 0.65f;
      float radius = (float) sphere.Radius;
      float num7 = radius * radius;
      float num8 = (float) (0.5 * (2.0 * (double) radius + 0.5));
      float num9 = (float) (0.5 * (-2.0 * (double) radius + 0.5));
      Vector3 vector3_3 = vector3_1 + vector1 * (float) sphere.Radius * num2 + direction * (float) sphere.Radius * 0.3f;
      float num10 = (float) (sphere.Radius * 0.100000001490116);
      float num11 = num10 * num10;
      float num12 = (float) (0.5 * (2.0 * (double) num10 + 0.5));
      Vector3I voxelCoord3;
      voxelCoord3.Z = voxelCoord1.Z;
      for (p.Z = 0; voxelCoord3.Z <= voxelCoord2.Z; ++p.Z)
      {
        voxelCoord3.Y = voxelCoord1.Y;
        for (p.Y = 0; voxelCoord3.Y <= voxelCoord2.Y; ++p.Y)
        {
          voxelCoord3.X = voxelCoord1.X;
          for (p.X = 0; voxelCoord3.X <= voxelCoord2.X; ++p.X)
          {
            Vector3D worldPosition3;
            MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref voxelCoord3, out worldPosition3);
            byte num13 = MyVoxelGenerator.m_cache.Content(ref p);
            if (num13 != byte.MaxValue)
            {
              float num14 = (float) (worldPosition3 - vector3_1).LengthSquared();
              float num15 = num14 - num4;
              byte content;
              if ((double) num15 > (double) num5)
                content = (byte) 0;
              else if ((double) num15 < (double) num6)
              {
                content = byte.MaxValue;
              }
              else
              {
                float num16 = (float) Math.Sqrt((double) num14 + (double) num4 - 2.0 * (double) num3 * Math.Sqrt((double) num14));
                if ((double) num15 < 0.0)
                  num16 = -num16;
                content = (byte) ((double) sbyte.MaxValue - (double) num16 / 0.5 * (double) sbyte.MaxValue);
              }
              if ((int) content > (int) num13)
              {
                if (material != null)
                  MyVoxelGenerator.m_cache.Material(ref p, materialIdx);
                flag = true;
                MyVoxelGenerator.m_cache.Content(ref p, content);
              }
            }
            float num17 = (float) (worldPosition3 - vector3_2).LengthSquared();
            float num18 = num17 - num7;
            byte num19;
            if ((double) num18 > (double) num8)
              num19 = (byte) 0;
            else if ((double) num18 < (double) num9)
            {
              num19 = byte.MaxValue;
            }
            else
            {
              float num14 = (float) Math.Sqrt((double) num17 + (double) num7 - 2.0 * (double) radius * Math.Sqrt((double) num17));
              if ((double) num18 < 0.0)
                num14 = -num14;
              num19 = (byte) ((double) sbyte.MaxValue - (double) num14 / 0.5 * (double) sbyte.MaxValue);
            }
            byte num20 = MyVoxelGenerator.m_cache.Content(ref p);
            if (num20 > (byte) 0 && num19 > (byte) 0)
            {
              flag = true;
              int num14 = (int) num20 - (int) num19;
              if (num14 < 0)
                num14 = 0;
              MyVoxelGenerator.m_cache.Content(ref p, (byte) num14);
              num1 += (int) num20 - num14;
            }
            float num21 = (float) (worldPosition3 - vector3_3).LengthSquared() - num11;
            if ((double) num21 <= 1.5)
            {
              MyVoxelMaterialDefinition materialDefinition1 = MyDefinitionManager.Static.GetVoxelMaterialDefinition(MyVoxelGenerator.m_cache.Material(ref p));
              MyVoxelMaterialDefinition materialDefinition2 = material;
              if ((double) num21 > 0.0)
              {
                byte num14 = MyVoxelGenerator.m_cache.Content(ref p);
                if (num14 == byte.MaxValue)
                  materialDefinition2 = materialDefinition1;
                if ((double) num21 >= (double) num12 && num14 != (byte) 0)
                  materialDefinition2 = materialDefinition1;
              }
              if (materialDefinition1 != materialDefinition2)
              {
                MyVoxelGenerator.m_cache.Material(ref p, materialDefinition2.Index);
                flag = true;
              }
              else
                goto label_38;
            }
            if ((double) ((float) (worldPosition3 - vector3_1).LengthSquared() - num4) <= 0.0 && MyVoxelGenerator.m_cache.Content(ref p) > (byte) 0 && MyVoxelGenerator.m_cache.WrinkleVoxelContent(ref p, 0.5f, 0.45f))
              flag = true;
label_38:
            ++voxelCoord3.X;
          }
          ++voxelCoord3.Y;
        }
        ++voxelCoord3.Z;
      }
      if (!flag)
        return;
      MyVoxelGenerator.RemoveSmallVoxelsUsingChachedVoxels();
      Vector3I voxelRangeMin = voxelCoord1 + voxelMap.StorageMin;
      Vector3I voxelRangeMax = voxelCoord2 + voxelMap.StorageMin;
      voxelMap.Storage.WriteRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, voxelRangeMin, voxelRangeMax);
      BoundingBoxD worldBoundaries = new MyShapeSphere()
      {
        Center = sphere.Center,
        Radius = ((float) (sphere.Radius * 1.5))
      }.GetWorldBoundaries();
      MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, voxelMap, ref worldBoundaries);
    }

    public static void RequestPaintInShape(
      IMyVoxelBase voxelMap,
      IMyVoxelShape voxelShape,
      byte materialIdx)
    {
      MyVoxelBase voxel = voxelMap as MyVoxelBase;
      MyShape myShape = voxelShape as MyShape;
      if (voxel == null || myShape == null)
        return;
      myShape.SendPaintRequest(voxel, materialIdx);
    }

    public static void RequestFillInShape(
      IMyVoxelBase voxelMap,
      IMyVoxelShape voxelShape,
      byte materialIdx)
    {
      MyVoxelBase voxel = voxelMap as MyVoxelBase;
      MyShape myShape = voxelShape as MyShape;
      if (voxel == null || myShape == null)
        return;
      myShape.SendFillRequest(voxel, materialIdx);
    }

    public static void RequestRevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
    {
      MyVoxelBase voxel = voxelMap as MyVoxelBase;
      MyShape myShape = voxelShape as MyShape;
      if (voxel == null || myShape == null)
        return;
      myShape.SendRevertRequest(voxel);
    }

    public static void RequestCutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
    {
      MyVoxelBase voxelbool = voxelMap as MyVoxelBase;
      MyShape myShape = voxelShape as MyShape;
      if (voxelbool == null || myShape == null)
        return;
      myShape.SendCutOutRequest(voxelbool);
    }

    public static void CutOutShapeWithProperties(
      MyVoxelBase voxelMap,
      MyShape shape,
      out float voxelsCountInPercent,
      out MyVoxelMaterialDefinition voxelMaterial,
      Dictionary<MyVoxelMaterialDefinition, int> exactCutOutMaterials = null,
      bool updateSync = false,
      bool onlyCheck = false,
      bool applyDamageMaterial = false,
      bool onlyApplyMaterial = false,
      bool skipCache = false)
    {
      if (!MySession.Static.EnableVoxelDestruction || voxelMap == null || (voxelMap.Storage == null || shape == null))
      {
        voxelsCountInPercent = 0.0f;
        voxelMaterial = (MyVoxelMaterialDefinition) null;
      }
      else
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = exactCutOutMaterials != null ? 1 : 0;
        MatrixD transformation = shape.Transformation;
        MatrixD matrixD = transformation * voxelMap.PositionComp.WorldMatrixInvScaled;
        matrixD.Translation += voxelMap.SizeInMetresHalf;
        shape.Transformation = matrixD;
        BoundingBoxD worldBoundaries = shape.GetWorldBoundaries();
        Vector3I minCorner;
        Vector3I maxCorner;
        MyVoxelGenerator.LocalAABBToVoxelStorageMinMax(voxelMap, ref worldBoundaries, out minCorner, out maxCorner);
        bool flag = exactCutOutMaterials != null | applyDamageMaterial;
        Vector3I voxelCoord1 = minCorner - 1;
        Vector3I voxelCoord2 = maxCorner + 1;
        voxelMap.Storage.ClampVoxelCoord(ref voxelCoord1);
        voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
        if (MyVoxelGenerator.m_cache == null)
          MyVoxelGenerator.m_cache = new MyStorageData();
        MyVoxelGenerator.m_cache.Resize(voxelCoord1, voxelCoord2);
        MyVoxelRequestFlags requestFlags = (MyVoxelRequestFlags) ((skipCache ? 0 : 64) | (flag ? 2 : 0));
        voxelMap.Storage.ReadRange(MyVoxelGenerator.m_cache, flag ? MyStorageDataTypeFlags.ContentAndMaterial : MyStorageDataTypeFlags.Content, 0, voxelCoord1, voxelCoord2, ref requestFlags);
        if (num3 != 0)
        {
          Vector3I p = MyVoxelGenerator.m_cache.Size3D / 2;
          voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(MyVoxelGenerator.m_cache.Material(ref p));
        }
        else
        {
          Vector3I voxelCoords = (voxelCoord1 + voxelCoord2) / 2;
          voxelMaterial = voxelMap.Storage.GetMaterialAt(ref voxelCoords);
        }
        MyVoxelMaterialDefinition key = (MyVoxelMaterialDefinition) null;
        Vector3I vector3I;
        for (vector3I.X = minCorner.X; vector3I.X <= maxCorner.X; ++vector3I.X)
        {
          for (vector3I.Y = minCorner.Y; vector3I.Y <= maxCorner.Y; ++vector3I.Y)
          {
            for (vector3I.Z = minCorner.Z; vector3I.Z <= maxCorner.Z; ++vector3I.Z)
            {
              Vector3I p = vector3I - voxelCoord1;
              int linear = MyVoxelGenerator.m_cache.ComputeLinear(ref p);
              byte num4 = MyVoxelGenerator.m_cache.Content(linear);
              if (num4 != (byte) 0)
              {
                Vector3D voxelPosition = (Vector3D) (vector3I - voxelMap.StorageMin) * 1.0;
                float volume = shape.GetVolume(ref voxelPosition);
                if ((double) volume != 0.0)
                {
                  int num5 = (int) ((double) volume * (double) byte.MaxValue);
                  int num6 = Math.Max((int) num4 - num5, 0);
                  int num7 = (int) num4 - num6;
                  if ((int) num4 / 10 != num6 / 10)
                  {
                    if (!onlyCheck && !onlyApplyMaterial)
                      MyVoxelGenerator.m_cache.Content(linear, (byte) num6);
                    num1 += (int) num4;
                    num2 += num7;
                    byte materialIndex = MyVoxelGenerator.m_cache.Material(linear);
                    if (num6 == 0)
                      MyVoxelGenerator.m_cache.Material(linear, byte.MaxValue);
                    if (materialIndex != byte.MaxValue)
                    {
                      if (flag)
                        key = MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIndex);
                      if (exactCutOutMaterials != null)
                      {
                        int num8;
                        exactCutOutMaterials.TryGetValue(key, out num8);
                        num8 += MyFakes.ENABLE_REMOVED_VOXEL_CONTENT_HACK ? (int) ((double) num7 * 3.90000009536743) : num7;
                        exactCutOutMaterials[key] = num8;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        if (num2 > 0 & updateSync && Sync.IsServer && !onlyCheck)
          shape.SendDrillCutOutRequest(voxelMap, applyDamageMaterial);
        if (num2 > 0 && !onlyCheck)
        {
          MyVoxelGenerator.RemoveSmallVoxelsUsingChachedVoxels();
          MyStorageDataTypeFlags dataToWrite = MyStorageDataTypeFlags.ContentAndMaterial;
          if (MyFakes.LOG_NAVMESH_GENERATION && MyAIComponent.Static.Pathfinding != null)
            MyAIComponent.Static.Pathfinding.GetPathfindingLog().LogStorageWrite(voxelMap, MyVoxelGenerator.m_cache, dataToWrite, voxelCoord1, voxelCoord2);
          voxelMap.Storage.WriteRange(MyVoxelGenerator.m_cache, dataToWrite, voxelCoord1, voxelCoord2, false, skipCache);
        }
        voxelsCountInPercent = (double) num1 > 0.0 ? (float) num2 / (float) num1 : 0.0f;
        if (num2 > 0)
        {
          BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
          MySandboxGame.Static.Invoke((Action) (() =>
          {
            if (voxelMap.Storage == null)
              return;
            voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
            MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, voxelMap, ref cutOutBox);
          }), "CutOutShapeWithProperties notify");
        }
        shape.Transformation = transformation;
      }
    }

    public static void ChangeMaterialsInShape(
      MyVoxelBase voxelMap,
      MyShape shape,
      byte materialIdx,
      bool[] materialsToChange)
    {
      if (voxelMap == null || shape == null)
        return;
      using (voxelMap.Pin())
      {
        if (voxelMap.MarkedForClose)
          return;
        MatrixD matrixD = shape.Transformation * voxelMap.PositionComp.WorldMatrixInvScaled;
        matrixD.Translation += voxelMap.SizeInMetresHalf;
        shape.Transformation = matrixD;
        BoundingBoxD worldBoundaries = shape.GetWorldBoundaries();
        Vector3I voxelMin;
        Vector3I voxelMax;
        MyVoxelGenerator.LocalAABBToVoxelStorageMinMax(voxelMap, ref worldBoundaries, out voxelMin, out voxelMax);
        Vector3I voxelCoord1 = voxelMin - 1;
        Vector3I voxelCoord2 = voxelMax + 1;
        voxelMap.Storage.ClampVoxelCoord(ref voxelCoord1);
        voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
        if (MyVoxelGenerator.m_cache == null)
          MyVoxelGenerator.m_cache = new MyStorageData();
        MyVoxelGenerator.m_cache.Resize(voxelCoord1, voxelCoord2);
        MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent | MyVoxelRequestFlags.AdviseCache;
        voxelMap.Storage.ReadRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.Material, 0, voxelCoord1, voxelCoord2, ref requestFlags);
        Vector3I voxelCoord3;
        for (voxelCoord3.X = voxelMin.X; voxelCoord3.X <= voxelMax.X; ++voxelCoord3.X)
        {
          for (voxelCoord3.Y = voxelMin.Y; voxelCoord3.Y <= voxelMax.Y; ++voxelCoord3.Y)
          {
            for (voxelCoord3.Z = voxelMin.Z; voxelCoord3.Z <= voxelMax.Z; ++voxelCoord3.Z)
            {
              Vector3I p = voxelCoord3 - voxelMin;
              int linear = MyVoxelGenerator.m_cache.ComputeLinear(ref p);
              byte num = MyVoxelGenerator.m_cache.Material(linear);
              if (materialsToChange[(int) num])
              {
                Vector3D worldPosition;
                MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref voxelCoord3, out worldPosition);
                if ((double) shape.GetVolume(ref worldPosition) > 0.5 && MyVoxelGenerator.m_cache.Material(ref p) != byte.MaxValue)
                  MyVoxelGenerator.m_cache.Material(ref p, materialIdx);
              }
            }
          }
        }
      }
    }

    public static void RevertShape(MyVoxelBase voxelMap, MyShape shape)
    {
      using (voxelMap.Pin())
      {
        if (voxelMap.MarkedForClose)
          return;
        Vector3I minCorner;
        Vector3I maxCorner;
        MyVoxelGenerator.GetVoxelShapeDimensions(voxelMap, shape, out minCorner, out maxCorner, out Vector3I _);
        minCorner = Vector3I.Max(Vector3I.One, minCorner);
        maxCorner = Vector3I.Max(minCorner, maxCorner - Vector3I.One);
        voxelMap.Storage.DeleteRange(MyStorageDataTypeFlags.ContentAndMaterial, minCorner, maxCorner, false);
        BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (voxelMap.Storage == null)
            return;
          voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
          MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Revert, voxelMap, ref cutOutBox);
        }), "RevertShape notify");
      }
    }

    public static void FillInShape(MyVoxelBase voxelMap, MyShape shape, byte materialIdx)
    {
      using (voxelMap.Pin())
      {
        if (voxelMap.MarkedForClose)
          return;
        ulong num1 = 0;
        Vector3I numCells;
        Vector3I minCorner;
        Vector3I maxCorner;
        MyVoxelGenerator.GetVoxelShapeDimensions(voxelMap, shape, out minCorner, out maxCorner, out numCells);
        minCorner = Vector3I.Max(Vector3I.One, minCorner);
        maxCorner = Vector3I.Max(minCorner, maxCorner - Vector3I.One);
        if (MyVoxelGenerator.m_cache == null)
          MyVoxelGenerator.m_cache = new MyStorageData();
        Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref Vector3I.Zero, ref numCells);
        while (it.IsValid())
        {
          Vector3I cellMinCorner;
          Vector3I cellMaxCorner;
          MyVoxelGenerator.GetCellCorners(ref minCorner, ref maxCorner, ref it, out cellMinCorner, out cellMaxCorner);
          Vector3I originalValue1 = cellMinCorner;
          Vector3I originalValue2 = cellMaxCorner;
          voxelMap.Storage.ClampVoxelCoord(ref cellMinCorner, 0);
          voxelMap.Storage.ClampVoxelCoord(ref cellMaxCorner, 0);
          Vector3I clampedValue = cellMinCorner;
          MyVoxelGenerator.ClampingInfo clampingInfo1 = MyVoxelGenerator.CheckForClamping(originalValue1, clampedValue);
          MyVoxelGenerator.ClampingInfo clampingInfo2 = MyVoxelGenerator.CheckForClamping(originalValue2, cellMaxCorner);
          MyVoxelGenerator.m_cache.Resize(cellMinCorner, cellMaxCorner);
          MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
          voxelMap.Storage.ReadRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, cellMinCorner, cellMaxCorner, ref requestFlags);
          ulong num2 = 0;
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref cellMinCorner, ref cellMaxCorner);
          while (vector3IRangeIterator.IsValid())
          {
            Vector3I p = vector3IRangeIterator.Current - cellMinCorner;
            byte num3 = MyVoxelGenerator.m_cache.Content(ref p);
            if (num3 != byte.MaxValue || (int) MyVoxelGenerator.m_cache.Material(ref p) != (int) materialIdx)
            {
              if (vector3IRangeIterator.Current.X == cellMinCorner.X && clampingInfo1.X || vector3IRangeIterator.Current.X == cellMaxCorner.X && clampingInfo2.X || (vector3IRangeIterator.Current.Y == cellMinCorner.Y && clampingInfo1.Y || vector3IRangeIterator.Current.Y == cellMaxCorner.Y && clampingInfo2.Y) || (vector3IRangeIterator.Current.Z == cellMinCorner.Z && clampingInfo1.Z || vector3IRangeIterator.Current.Z == cellMaxCorner.Z && clampingInfo2.Z))
              {
                if (num3 != (byte) 0)
                  MyVoxelGenerator.m_cache.Material(ref p, materialIdx);
              }
              else
              {
                Vector3D worldPosition;
                MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionComp.WorldMatrix, voxelMap.PositionLeftBottomCorner, voxelMap.SizeInMetresHalf, ref vector3IRangeIterator.Current, out worldPosition);
                float volume = shape.GetVolume(ref worldPosition);
                if ((double) volume > 0.0)
                {
                  int val2 = (int) ((double) volume * (double) byte.MaxValue);
                  long num4 = (long) Math.Max((int) num3, val2);
                  MyVoxelGenerator.m_cache.Content(ref p, (byte) num4);
                  if (num4 != 0L)
                    MyVoxelGenerator.m_cache.Material(ref p, materialIdx);
                  num2 += (ulong) num4 - (ulong) num3;
                }
              }
            }
            vector3IRangeIterator.MoveNext();
          }
          if (num2 > 0UL)
          {
            MyVoxelGenerator.RemoveSmallVoxelsUsingChachedVoxels();
            voxelMap.Storage.WriteRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, cellMinCorner, cellMaxCorner, false, true);
          }
          num1 += num2;
          it.MoveNext();
        }
        if (num1 <= 0UL)
          return;
        BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (voxelMap.Storage == null)
            return;
          voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
          MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Fill, voxelMap, ref cutOutBox);
        }), "FillInShape Notify");
      }
    }

    public static void PaintInShape(MyVoxelBase voxelMap, MyShape shape, byte materialIdx)
    {
      using (voxelMap.Pin())
      {
        if (voxelMap.MarkedForClose)
          return;
        Vector3I numCells;
        Vector3I minCorner;
        Vector3I maxCorner;
        MyVoxelGenerator.GetVoxelShapeDimensions(voxelMap, shape, out minCorner, out maxCorner, out numCells);
        if (MyVoxelGenerator.m_cache == null)
          MyVoxelGenerator.m_cache = new MyStorageData();
        Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref Vector3I.Zero, ref numCells);
        while (it.IsValid())
        {
          Vector3I cellMinCorner;
          Vector3I cellMaxCorner;
          MyVoxelGenerator.GetCellCorners(ref minCorner, ref maxCorner, ref it, out cellMinCorner, out cellMaxCorner);
          MyVoxelGenerator.m_cache.Resize(cellMinCorner, cellMaxCorner);
          MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
          voxelMap.Storage.ReadRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, cellMinCorner, cellMaxCorner, ref requestFlags);
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref cellMinCorner, ref cellMaxCorner);
          while (vector3IRangeIterator.IsValid())
          {
            Vector3I p = vector3IRangeIterator.Current - cellMinCorner;
            Vector3D worldPosition;
            MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionComp.WorldMatrix, voxelMap.PositionLeftBottomCorner, voxelMap.SizeInMetresHalf, ref vector3IRangeIterator.Current, out worldPosition);
            if ((double) shape.GetVolume(ref worldPosition) > 0.5 && MyVoxelGenerator.m_cache.Material(ref p) != byte.MaxValue)
              MyVoxelGenerator.m_cache.Material(ref p, materialIdx);
            vector3IRangeIterator.MoveNext();
          }
          voxelMap.Storage.WriteRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.Material, cellMinCorner, cellMaxCorner, false, true);
          it.MoveNext();
        }
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (voxelMap.Storage == null)
            return;
          voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
        }), "PaintInShape notify");
      }
    }

    public static bool CutOutSphereFast(
      MyVoxelBase voxelMap,
      ref Vector3D center,
      float radius,
      out Vector3I cacheMin,
      out Vector3I cacheMax,
      bool notifyChanged)
    {
      MatrixD matrixD = voxelMap.PositionComp.WorldMatrixInvScaled;
      matrixD.Translation += voxelMap.SizeInMetresHalf;
      BoundingBoxD localAABB = BoundingBoxD.CreateFromSphere(new BoundingSphereD(center, (double) radius)).TransformFast(matrixD);
      Vector3I voxelMin;
      Vector3I voxelMax;
      MyVoxelGenerator.LocalAABBToVoxelStorageMinMax(voxelMap, ref localAABB, out voxelMin, out voxelMax);
      cacheMin = voxelMin - 1;
      cacheMax = voxelMax + 1;
      voxelMap.Storage.ClampVoxelCoord(ref cacheMin);
      voxelMap.Storage.ClampVoxelCoord(ref cacheMax);
      MyVoxelGenerator.CutOutSphere voxelOperator = new MyVoxelGenerator.CutOutSphere();
      voxelOperator.RadSq = radius * radius;
      voxelOperator.Center = Vector3D.Transform(center, matrixD) - (Vector3D) (cacheMin - voxelMap.StorageMin);
      voxelMap.Storage.ExecuteOperationFast<MyVoxelGenerator.CutOutSphere>(ref voxelOperator, MyStorageDataTypeFlags.Content, ref cacheMin, ref cacheMax, notifyChanged);
      return voxelOperator.Changed;
    }

    public static void CutOutShape(MyVoxelBase voxelMap, MyShape shape, bool voxelHand = false)
    {
      if (!MySession.Static.EnableVoxelDestruction && !MySession.Static.HighSimulationQuality)
        return;
      using (voxelMap.Pin())
      {
        if (voxelMap.MarkedForClose)
          return;
        Vector3I numCells;
        Vector3I minCorner;
        Vector3I maxCorner;
        MyVoxelGenerator.GetVoxelShapeDimensions(voxelMap, shape, out minCorner, out maxCorner, out numCells);
        ulong num1 = 0;
        if (MyVoxelGenerator.m_cache == null)
          MyVoxelGenerator.m_cache = new MyStorageData();
        Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref Vector3I.Zero, ref numCells);
        while (it.IsValid())
        {
          Vector3I cellMinCorner;
          Vector3I cellMaxCorner;
          MyVoxelGenerator.GetCellCorners(ref minCorner, ref maxCorner, ref it, out cellMinCorner, out cellMaxCorner);
          Vector3I voxelCoord1 = cellMinCorner - 1;
          Vector3I voxelCoord2 = cellMaxCorner + 1;
          voxelMap.Storage.ClampVoxelCoord(ref voxelCoord1);
          voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
          ulong num2 = 0;
          MyVoxelGenerator.m_cache.Resize(voxelCoord1, voxelCoord2);
          MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
          voxelMap.Storage.ReadRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, 0, voxelCoord1, voxelCoord2, ref requestFlags);
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref cellMinCorner, ref cellMaxCorner);
          while (vector3IRangeIterator.IsValid())
          {
            Vector3I p = vector3IRangeIterator.Current - voxelCoord1;
            byte num3 = MyVoxelGenerator.m_cache.Content(ref p);
            if (num3 != (byte) 0)
            {
              Vector3D worldPosition;
              MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionComp.WorldMatrix, voxelMap.PositionLeftBottomCorner, voxelMap.SizeInMetresHalf, ref vector3IRangeIterator.Current, out worldPosition);
              float volume = shape.GetVolume(ref worldPosition);
              if ((double) volume != 0.0)
              {
                int num4 = Math.Min((int) ((double) byte.MaxValue - (double) volume * (double) byte.MaxValue), (int) num3);
                ulong num5 = (ulong) Math.Abs((int) num3 - num4);
                MyVoxelGenerator.m_cache.Content(ref p, (byte) num4);
                if (num4 == 0)
                  MyVoxelGenerator.m_cache.Material(ref p, byte.MaxValue);
                num2 += num5;
              }
            }
            vector3IRangeIterator.MoveNext();
          }
          if (num2 > 0UL)
          {
            MyVoxelGenerator.RemoveSmallVoxelsUsingChachedVoxels();
            voxelMap.Storage.WriteRange(MyVoxelGenerator.m_cache, MyStorageDataTypeFlags.ContentAndMaterial, voxelCoord1, voxelCoord2, false, true);
          }
          num1 += num2;
          it.MoveNext();
        }
        if (num1 <= 0UL)
          return;
        BoundingBoxD cutOutBox = shape.GetWorldBoundaries();
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (voxelMap.Storage == null)
            return;
          voxelMap.Storage.NotifyChanged(minCorner, maxCorner, MyStorageDataTypeFlags.ContentAndMaterial);
          MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, voxelMap, ref cutOutBox);
        }), "CutOutShape notify");
      }
    }

    private static MyVoxelGenerator.ClampingInfo CheckForClamping(
      Vector3I originalValue,
      Vector3I clampedValue)
    {
      MyVoxelGenerator.ClampingInfo clampingInfo = new MyVoxelGenerator.ClampingInfo(false, false, false);
      if (originalValue.X != clampedValue.X)
        clampingInfo.X = true;
      if (originalValue.Y != clampedValue.Y)
        clampingInfo.Y = true;
      if (originalValue.Z != clampedValue.Z)
        clampingInfo.Z = true;
      return clampingInfo;
    }

    private static void RemoveSmallVoxelsUsingChachedVoxels()
    {
      Vector3I size3D = MyVoxelGenerator.m_cache.Size3D;
      Vector3I max = size3D - 1;
      Vector3I p1;
      for (p1.X = 0; p1.X < size3D.X; ++p1.X)
      {
        for (p1.Y = 0; p1.Y < size3D.Y; ++p1.Y)
        {
          for (p1.Z = 0; p1.Z < size3D.Z; ++p1.Z)
          {
            int num = (int) MyVoxelGenerator.m_cache.Content(ref p1);
            if (num > 0 && num < (int) sbyte.MaxValue)
            {
              Vector3I result1 = p1 - 1;
              Vector3I result2 = p1 + 1;
              Vector3I.Clamp(ref result1, ref Vector3I.Zero, ref max, out result1);
              Vector3I.Clamp(ref result2, ref Vector3I.Zero, ref max, out result2);
              bool flag = false;
              Vector3I p2;
              for (p2.X = result1.X; p2.X <= result2.X; ++p2.X)
              {
                for (p2.Y = result1.Y; p2.Y <= result2.Y; ++p2.Y)
                {
                  for (p2.Z = result1.Z; p2.Z <= result2.Z; ++p2.Z)
                  {
                    if (MyVoxelGenerator.m_cache.Content(ref p2) >= (byte) 127)
                    {
                      flag = true;
                      goto label_15;
                    }
                  }
                }
              }
label_15:
              if (!flag)
              {
                MyVoxelGenerator.m_cache.Content(ref p1, (byte) 0);
                MyVoxelGenerator.m_cache.Material(ref p1, byte.MaxValue);
              }
            }
          }
        }
      }
    }

    private static void WorldAABBToVoxelStorageMinMax(
      MyVoxelBase voxelMap,
      ref BoundingBoxD worldAABB,
      out Vector3I voxelMin,
      out Vector3I voxelMax)
    {
      BoundingBoxD boundingBoxD = worldAABB.TransformFast(voxelMap.PositionComp.WorldMatrixNormalizedInv);
      boundingBoxD.Translate((Vector3D) ((Vector3) voxelMap.StorageMin + voxelMap.SizeInMetresHalf));
      voxelMin = Vector3I.Floor(boundingBoxD.Min);
      voxelMax = Vector3I.Ceiling((Vector3) boundingBoxD.Max);
      Vector3I max = voxelMap.Storage.Size - 1;
      Vector3I.Clamp(ref voxelMin, ref Vector3I.Zero, ref max, out voxelMin);
      Vector3I.Clamp(ref voxelMax, ref Vector3I.Zero, ref max, out voxelMax);
    }

    private static void LocalAABBToVoxelStorageMinMax(
      MyVoxelBase voxelMap,
      ref BoundingBoxD localAABB,
      out Vector3I voxelMin,
      out Vector3I voxelMax)
    {
      BoundingBoxD boundingBoxD = localAABB;
      boundingBoxD.Translate((Vector3D) (voxelMap.StorageMin * voxelMap.VoxelSize));
      voxelMin = Vector3I.Floor(boundingBoxD.Min);
      voxelMax = Vector3I.Ceiling((Vector3) boundingBoxD.Max);
      Vector3I max = voxelMap.Storage.Size - 1;
      Vector3I.Clamp(ref voxelMin, ref Vector3I.Zero, ref max, out voxelMin);
      Vector3I.Clamp(ref voxelMax, ref Vector3I.Zero, ref max, out voxelMax);
    }

    private static void GetVoxelShapeDimensions(
      MyVoxelBase voxelMap,
      MyShape shape,
      out Vector3I minCorner,
      out Vector3I maxCorner,
      out Vector3I numCells)
    {
      BoundingBoxD worldBoundaries = shape.GetWorldBoundaries();
      MyVoxelGenerator.WorldAABBToVoxelStorageMinMax(voxelMap, ref worldBoundaries, out minCorner, out maxCorner);
      numCells = new Vector3I(Math.Abs(maxCorner.X - minCorner.X) / 16, Math.Abs(maxCorner.Y - minCorner.Y) / 16, Math.Abs(maxCorner.Z - minCorner.Z) / 16);
    }

    private static void GetCellCorners(
      ref Vector3I minCorner,
      ref Vector3I maxCorner,
      ref Vector3I_RangeIterator it,
      out Vector3I cellMinCorner,
      out Vector3I cellMaxCorner)
    {
      cellMinCorner = new Vector3I(Math.Min(maxCorner.X, minCorner.X + it.Current.X * 16), Math.Min(maxCorner.Y, minCorner.Y + it.Current.Y * 16), Math.Min(maxCorner.Z, minCorner.Z + it.Current.Z * 16));
      cellMaxCorner = new Vector3I(Math.Min(maxCorner.X, cellMinCorner.X + 16), Math.Min(maxCorner.Y, cellMinCorner.Y + 16), Math.Min(maxCorner.Z, cellMinCorner.Z + 16));
    }

    public static void NotifyVoxelChanged(
      MyVoxelBase.OperationType type,
      MyVoxelBase voxelMap,
      ref BoundingBoxD cutOutBox)
    {
      cutOutBox.Inflate(0.25);
      MyGamePruningStructure.GetTopmostEntitiesInBox(ref cutOutBox, MyVoxelGenerator.m_overlapList);
      if (MyFakes.ENABLE_BLOCKS_IN_VOXELS_TEST)
      {
        foreach (MyEntity overlap in MyVoxelGenerator.m_overlapList)
        {
          if (Sync.IsServer && overlap is MyCubeGrid myCubeGrid && myCubeGrid.IsStatic)
          {
            if (myCubeGrid.Physics != null && myCubeGrid.Physics.Shape != null)
              myCubeGrid.Physics.Shape.RecalculateConnectionsToWorld(myCubeGrid.GetBlocks());
            if (type == MyVoxelBase.OperationType.Cut)
              myCubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
          }
          if (overlap.Physics is MyPhysicsBody physics && !physics.IsStatic && (HkReferenceObject) physics.RigidBody != (HkReferenceObject) null)
            physics.RigidBody.Activate();
        }
      }
      MyVoxelGenerator.m_overlapList.Clear();
      if (!Sync.IsServer)
        return;
      MyPlanetEnvironmentComponent environmentComponent = voxelMap.Components.Get<MyPlanetEnvironmentComponent>();
      if (environmentComponent == null)
        return;
      environmentComponent.GetSectorsInRange(ref cutOutBox, MyVoxelGenerator.m_overlapList);
      foreach (MyEnvironmentSector overlap in MyVoxelGenerator.m_overlapList)
        overlap.DisableItemsInBox(ref cutOutBox);
      MyVoxelGenerator.m_overlapList.Clear();
    }

    internal struct CutOutSphere : IVoxelOperator
    {
      public float RadSq;
      public Vector3D Center;
      public bool Changed;

      public VoxelOperatorFlags Flags => VoxelOperatorFlags.ReadWrite;

      public void Op(ref Vector3I pos, MyStorageDataTypeEnum dataType, ref byte content)
      {
        if (content == (byte) 0 || Vector3D.DistanceSquared(this.Center, (Vector3D) pos) >= (double) this.RadSq)
          return;
        this.Changed = true;
        content = (byte) 0;
      }
    }

    private struct ClampingInfo
    {
      public bool X;
      public bool Y;
      public bool Z;

      public ClampingInfo(bool X, bool Y, bool Z)
      {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
      }
    }
  }
}
