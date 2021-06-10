// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyDepositQuery
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Profiler;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyDepositQuery : IPrioritizedWork, IWork
  {
    private static readonly MyObjectsPool<MyDepositQuery> m_instancePool = new MyObjectsPool<MyDepositQuery>(16);
    [ThreadStatic]
    private static MyStorageData m_cache;
    [ThreadStatic]
    private static MyDepositQuery.MaterialPositionData[] m_materialData;
    private List<MyEntityOreDeposit> m_result;
    private List<Vector3I> m_emptyCells;
    private readonly Action m_onComplete;
    private bool m_canceled;

    public static void Start(
      Vector3I min,
      Vector3I max,
      long detectorId,
      MyVoxelBase voxelMap,
      Action<List<MyEntityOreDeposit>, List<Vector3I>> completionCallback)
    {
      MyDepositQuery query = (MyDepositQuery) null;
      MyDepositQuery.m_instancePool.AllocateOrCreate(out query);
      if (query == null)
        return;
      query.Min = min;
      query.Max = max;
      query.DetectorId = detectorId;
      query.VoxelMap = voxelMap;
      query.CompletionCallback = completionCallback;
      MyOreDetectorSessionComponent.Static.Track(query);
      Parallel.Start((IWork) query, query.m_onComplete);
    }

    private static MyStorageData Cache
    {
      get
      {
        if (MyDepositQuery.m_cache == null)
          MyDepositQuery.m_cache = new MyStorageData();
        return MyDepositQuery.m_cache;
      }
    }

    private static MyDepositQuery.MaterialPositionData[] MaterialData
    {
      get
      {
        if (MyDepositQuery.m_materialData == null)
          MyDepositQuery.m_materialData = new MyDepositQuery.MaterialPositionData[256];
        return MyDepositQuery.m_materialData;
      }
    }

    public Vector3I Min { get; set; }

    public Vector3I Max { get; set; }

    public MyVoxelBase VoxelMap { get; set; }

    public long DetectorId { get; set; }

    public Action<List<MyEntityOreDeposit>, List<Vector3I>> CompletionCallback { get; set; }

    public MyDepositQuery() => this.m_onComplete = new Action(this.OnComplete);

    private void OnComplete()
    {
      this.CompletionCallback.InvokeIfNotNull<List<MyEntityOreDeposit>, List<Vector3I>>(this.m_result, this.m_emptyCells);
      this.m_result = (List<MyEntityOreDeposit>) null;
      this.VoxelMap = (MyVoxelBase) null;
      this.CompletionCallback = (Action<List<MyEntityOreDeposit>, List<Vector3I>>) null;
      MyDepositQuery.m_instancePool.Deallocate(this);
      MyOreDetectorSessionComponent.Static.Untrack(this);
    }

    WorkPriority IPrioritizedWork.Priority => WorkPriority.VeryLow;

    void IWork.DoWork(WorkData workData)
    {
      try
      {
        if (this.m_result == null)
        {
          this.m_result = new List<MyEntityOreDeposit>();
          this.m_emptyCells = new List<Vector3I>();
        }
        if (this.m_canceled)
          return;
        MyStorageData cache = MyDepositQuery.Cache;
        cache.Resize(new Vector3I(8));
        IMyStorage storage = this.VoxelMap.Storage;
        if (storage == null)
          return;
        using (StoragePin storagePin = storage.Pin())
        {
          if (!storagePin.Valid)
            return;
          Vector3I cell;
          for (cell.Z = this.Min.Z; cell.Z <= this.Max.Z; ++cell.Z)
          {
            for (cell.Y = this.Min.Y; cell.Y <= this.Max.Y; ++cell.Y)
            {
              for (cell.X = this.Min.X; cell.X <= this.Max.X; ++cell.X)
              {
                if (this.m_canceled)
                  return;
                this.ProcessCell(cache, storage, cell, this.DetectorId);
              }
            }
          }
        }
      }
      finally
      {
      }
    }

    private void ProcessCell(
      MyStorageData cache,
      IMyStorage storage,
      Vector3I cell,
      long detectorId)
    {
      Vector3I lodVoxelRangeMin = cell << 3;
      Vector3I lodVoxelRangeMax = lodVoxelRangeMin + 7;
      storage.ReadRange(cache, MyStorageDataTypeFlags.Content, 2, lodVoxelRangeMin, lodVoxelRangeMax);
      if (!cache.ContainsVoxelsAboveIsoLevel())
        return;
      MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.PreciseOrePositions;
      storage.ReadRange(cache, MyStorageDataTypeFlags.Material, 2, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
      MyDepositQuery.MaterialPositionData[] materialData = MyDepositQuery.MaterialData;
      Vector3I p;
      for (p.Z = 0; p.Z < 8; ++p.Z)
      {
        for (p.Y = 0; p.Y < 8; ++p.Y)
        {
          for (p.X = 0; p.X < 8; ++p.X)
          {
            int linear = cache.ComputeLinear(ref p);
            if (cache.Content(linear) > (byte) 127)
            {
              byte num = cache.Material(linear);
              Vector3D vector3D = (Vector3D) ((p + lodVoxelRangeMin) * 4f + 2f);
              ref Vector3 local = ref materialData[(int) num].Sum;
              local = (Vector3) (local + vector3D);
              ++materialData[(int) num].Count;
            }
          }
        }
      }
      MyEntityOreDeposit entityOreDeposit = (MyEntityOreDeposit) null;
      for (int index = 0; index < materialData.Length; ++index)
      {
        if (materialData[index].Count != 0)
        {
          MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte) index);
          if (materialDefinition != null && materialDefinition.IsRare)
          {
            if (entityOreDeposit == null)
              entityOreDeposit = new MyEntityOreDeposit(this.VoxelMap, cell, detectorId);
            List<MyEntityOreDeposit.Data> materials = entityOreDeposit.Materials;
            MyVoxelMaterialDefinition material = materialDefinition;
            Vector3D vector3D = (Vector3D) (materialData[index].Sum / (float) materialData[index].Count - this.VoxelMap.SizeInMetresHalf);
            MatrixD matrix = this.VoxelMap.WorldMatrix;
            Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
            Vector3 averageLocalPosition = (Vector3) Vector3D.Transform(vector3D, fromRotationMatrix);
            MyEntityOreDeposit.Data data = new MyEntityOreDeposit.Data(material, averageLocalPosition);
            materials.Add(data);
          }
        }
      }
      if (entityOreDeposit != null)
        this.m_result.Add(entityOreDeposit);
      else
        this.m_emptyCells.Add(cell);
      Array.Clear((Array) materialData, 0, materialData.Length);
    }

    WorkOptions IWork.Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Block, "OreDetector");

    public void Cancel() => this.m_canceled = true;

    private struct MaterialPositionData
    {
      public Vector3 Sum;
      public int Count;
    }
  }
}
