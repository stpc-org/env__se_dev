// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyVoxelClipmap
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Entities.Components;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.Mesh;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
  public class MyVoxelClipmap : IMyLodController
  {
    internal int CellBits;
    internal int CellSize;
    private int m_updateDistance;
    private int m_maxLod;
    private MyVoxelClipmapCache m_cache;
    internal int[] Ranges;
    private readonly Vector3I m_voxelSize;
    private MatrixD m_localToWorld;
    private MatrixD m_worldToLocal;
    internal MyVoxelClipmapRing[] Rings;
    private Ref<int> m_lockOwner = Ref.Create<int>(-1);
    private FastResourceLock m_lock = new FastResourceLock();
    private float? m_spherizeRadius;
    private Vector3D m_spherizePosition;
    private const int LOADED_WAIT_TIME = 5;
    private int m_loadedCounter;
    private MyVoxelClipmapSettings m_settings;
    private bool m_settingsChanged;
    private static readonly MyConcurrentPool<MyVoxelClipmap.StitchOperation> m_stitchDependencyPool = new MyConcurrentPool<MyVoxelClipmap.StitchOperation>(100, expectedAllocations: 1000000);
    private static readonly MyConcurrentPool<MyVoxelClipmap.CompoundStitchOperation> m_compoundStitchDependencyPool = new MyConcurrentPool<MyVoxelClipmap.CompoundStitchOperation>(50, expectedAllocations: 1000000);
    private readonly MyWorkTracker<MyCellCoord, MyClipmapMeshJob> m_dataWorkTracker = new MyWorkTracker<MyCellCoord, MyClipmapMeshJob>();
    private readonly MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob> m_fullWorkTracker = new MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob>();
    private readonly List<(MyCellCoord Coord, VrSewGuide Guide, MyClipmapCellVicinity Vicinity, MyVoxelContentConstitution Constitution)> m_cachedCellRequests = new List<(MyCellCoord, VrSewGuide, MyClipmapCellVicinity, MyVoxelContentConstitution)>();
    private readonly MyConcurrentQueue<MyVoxelClipmap.CellRenderUpdate> m_cellRenderUpdates = new MyConcurrentQueue<MyVoxelClipmap.CellRenderUpdate>(128);
    private readonly MyListDictionary<MyCellCoord, MyVoxelClipmap.StitchOperation> m_stitchDependencies = new MyListDictionary<MyCellCoord, MyVoxelClipmap.StitchOperation>((IEqualityComparer<MyCellCoord>) MyCellCoord.Comparer);
    private readonly MyWorkTracker<MyCellCoord, MyClipmapSewJob> m_stitchWorkTracker = new MyWorkTracker<MyCellCoord, MyClipmapSewJob>((IEqualityComparer<MyCellCoord>) MyCellCoord.Comparer);
    private static readonly Vector3I[] m_neighbourOffsets = new Vector3I[8]
    {
      new Vector3I(0, 0, 0),
      new Vector3I(1, 0, 0),
      new Vector3I(0, 1, 0),
      new Vector3I(1, 1, 0),
      new Vector3I(0, 0, 1),
      new Vector3I(1, 0, 1),
      new Vector3I(0, 1, 1),
      new Vector3I(1, 1, 1)
    };
    private static readonly VrSewOperation[] m_compromizes = new VrSewOperation[8]
    {
      VrSewOperation.None,
      VrSewOperation.XFace,
      VrSewOperation.YFace,
      VrSewOperation.XY | VrSewOperation.XYZ,
      VrSewOperation.ZFace,
      VrSewOperation.XZ | VrSewOperation.XYZ,
      VrSewOperation.YZ | VrSewOperation.XYZ,
      VrSewOperation.None
    };
    private MyVoxelClipmap.UpdateState m_updateState;
    private Vector3L m_lastPosition;
    private BoundingBoxI? m_invalidateRange;
    private readonly MyQueue<MyVoxelClipmap.Op> m_debugWorkQueue = new MyQueue<MyVoxelClipmap.Op>(256);
    public static bool DebugDrawDependencies = false;
    public static bool UpdateVisibility = true;
    public static MyVoxelClipmap.StitchMode ActiveStitchMode = MyVoxelClipmap.StitchMode.Stitch;

    internal MyVoxelMesherComponent Mesher { get; private set; }

    public MyVoxelClipmapCache Cache
    {
      get => this.m_cache;
      set
      {
        if (this.m_cache != null && this.Actor != null)
          this.m_cache.Unregister(this.Actor.Id);
        this.m_cache = value;
        this.BindToCache();
      }
    }

    public IMyVoxelRenderDataProcessorProvider VoxelRenderDataProcessorProvider { get; set; }

    public MyVoxelClipmap(
      Vector3I voxelSize,
      MatrixD worldMatrix,
      MyVoxelMesherComponent mesher,
      float? spherizeRadius,
      Vector3D spherizePosition,
      string settingsGroup = null)
    {
      this.m_voxelSize = voxelSize;
      this.m_spherizeRadius = spherizeRadius;
      this.m_spherizePosition = spherizePosition;
      this.Mesher = mesher;
      this.UpdateWorldMatrix(ref worldMatrix);
      this.SettingsGroup = settingsGroup;
      this.UpdateSettings(MyVoxelClipmapSettings.GetSettings(this.SettingsGroup));
      this.ApplySettings(false);
    }

    private void UpdateWorldMatrix(ref MatrixD matrix)
    {
      this.LocalToWorld = matrix;
      MatrixD.Invert(ref this.m_localToWorld, out this.m_worldToLocal);
    }

    public string SettingsGroup { get; private set; }

    public bool UpdateSettings(MyVoxelClipmapSettings settings)
    {
      if (!settings.IsValid || settings.Equals(this.m_settings))
        return false;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        this.m_settings = settings;
        this.m_settingsChanged = true;
      }
      return true;
    }

    private void ApplySettings(bool invalidateCells)
    {
      this.CellBits = this.m_settings.CellSizeLg2;
      this.CellSize = 1 << this.CellBits;
      this.m_updateDistance = this.CellSize * this.CellSize / 4;
      int length = this.m_settings.MinSize <= 0 ? 16 : Math.Min(MathHelper.Log2Ceiling(this.m_voxelSize.AbsMax() / this.m_settings.MinSize), 16);
      if (length > 16)
        MyLog.Default.Error(string.Format("Voxel map LODs(size) exceeded limit ({0} of limit {1}). ", (object) length, (object) 16));
      Vector3I size = this.m_voxelSize + this.CellSize - 1 >> this.CellBits;
      if (this.m_maxLod != length)
      {
        this.Rings = new MyVoxelClipmapRing[length];
        this.Ranges = new int[length];
        for (int lod = 0; lod < length; ++lod)
          this.Rings[lod] = new MyVoxelClipmapRing(this, lod);
      }
      for (int index = 0; index < length; ++index)
      {
        this.Rings[index].UpdateSize(size);
        size = size + 1 >> 1;
        this.Ranges[index] = this.m_settings.LodRanges[index];
      }
      this.Ranges[length - 1] = int.MaxValue;
      this.m_maxLod = length;
      this.m_settingsChanged = false;
      if (!invalidateCells)
        return;
      this.m_invalidateRange = new BoundingBoxI?(BoundingBoxI.CreateInvalid());
    }

    internal MyVoxelContentConstitution ApproximateCellConstitution(
      Vector3I cell,
      int lod)
    {
      BoundingBoxI cellBounds = this.GetCellBounds(new MyCellCoord(lod, cell), false);
      cellBounds.Min -= 1;
      IMyStorage storage = this.Mesher.Storage;
      if (storage == null)
        return MyVoxelContentConstitution.Empty;
      switch (storage.Intersect(ref cellBounds, lod))
      {
        case ContainmentType.Disjoint:
          return MyVoxelContentConstitution.Empty;
        case ContainmentType.Contains:
          return MyVoxelContentConstitution.Full;
        case ContainmentType.Intersects:
          return MyVoxelContentConstitution.Mixed;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    internal void RequestCell(Vector3I cell, int lod, VrSewGuide existingGuide = null)
    {
      MyCellCoord myCellCoord = new MyCellCoord(lod, cell);
      if (this.InstanceStitchMode == MyVoxelClipmap.StitchMode.Stitch)
      {
        VrSewGuide data;
        MyClipmapCellVicinity vicinity;
        MyVoxelContentConstitution constitution;
        if (this.m_cache != null && this.m_cache.TryRead(this.Actor.Id, myCellCoord, out data, out vicinity, out constitution))
        {
          data?.AddReference();
          this.m_cachedCellRequests.Add((myCellCoord, data, vicinity, constitution));
        }
        else if (this.m_dataWorkTracker.Exists(myCellCoord))
        {
          this.m_dataWorkTracker.Invalidate(myCellCoord);
        }
        else
        {
          existingGuide?.AddReference();
          MyClipmapMeshJob.Start(this.m_dataWorkTracker, this, myCellCoord, existingGuide);
        }
      }
      else if (this.m_fullWorkTracker.Exists(myCellCoord))
        this.m_fullWorkTracker.Invalidate(myCellCoord);
      else
        MyClipmapFullMeshJob.Start(this.m_fullWorkTracker, this, myCellCoord);
    }

    private void BindToCache()
    {
      if (this.m_cache == null || this.Actor == null)
        return;
      this.m_cache.Register(this.Actor.Id, this);
    }

    private void ProcessCacheHits()
    {
      int count = this.m_cachedCellRequests.Count;
      for (int index = 0; index < count; ++index)
      {
        (MyCellCoord myCellCoord2, VrSewGuide vrSewGuide2, MyClipmapCellVicinity clipmapCellVicinity2, MyVoxelContentConstitution contentConstitution2) = this.m_cachedCellRequests[index];
        vrSewGuide2?.AddReference();
        this.FeedMeshResult(myCellCoord2, vrSewGuide2, contentConstitution2, clipmapCellVicinity2);
        vrSewGuide2?.RemoveReference();
      }
      this.m_cachedCellRequests.Clear();
    }

    internal void HandleCacheEviction(MyCellCoord coord, VrSewGuide guide)
    {
    }

    private void ProcessUpdates()
    {
      IMyVoxelUpdateBatch updateBatch = (IMyVoxelUpdateBatch) null;
      MyVoxelClipmap.CellRenderUpdate instance;
      while (this.m_cellRenderUpdates.TryDequeue(out instance))
      {
        if (this.IsUnloaded)
        {
          if (instance.Operation != null)
            this.CommitStitchOperation(instance.Operation);
          instance.Data.Dispose();
        }
        else
        {
          MyVoxelClipmapRing ring = this.Rings[instance.Cell.Lod];
          ring.UpdateCellRender(instance.Cell.CoordInLod, ref instance.Data, ref updateBatch);
          if (instance.Operation != null)
          {
            int num = instance.Operation.Recalculate ? 1 : 0;
            this.CommitStitchOperation(instance.Operation);
            if (num != 0)
              this.Stitch(instance.Cell, MyClipmapCellVicinity.Invalid);
          }
          ring.FinishAdd(instance.Cell.CoordInLod);
        }
      }
      updateBatch?.Commit();
    }

    internal void UpdateCellData(
      MyClipmapMeshJob job,
      MyCellCoord cell,
      VrSewGuide guide,
      MyVoxelContentConstitution constitution)
    {
      using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
      {
        if (job.IsReusingGuide)
          guide.RemoveReference();
        if ((this.IsUnloaded || job.IsCanceled) && !job.IsReusingGuide)
        {
          guide?.RemoveReference();
        }
        else
        {
          this.FeedMeshResult(cell, guide, constitution, MyClipmapCellVicinity.Invalid);
          this.m_cache?.Write(this.Actor.Id, cell, guide, this.Rings[cell.Lod].GetCellVicinity(cell.CoordInLod), constitution);
          this.m_dataWorkTracker.Complete(cell);
        }
      }
    }

    private void FeedMeshResult(
      MyCellCoord cell,
      VrSewGuide mesh,
      MyVoxelContentConstitution constitution,
      MyClipmapCellVicinity vicinity)
    {
      MyVoxelClipmapRing ring = this.Rings[cell.Lod];
      if (mesh == null && ring.IsForwardEdge(cell.CoordInLod))
      {
        BoundingBoxI cellBounds = this.GetCellBounds(cell);
        mesh = new VrSewGuide(cell.Lod, cellBounds.Min, cellBounds.Max, this.GetShellCacheForConstitution(constitution));
      }
      ring.UpdateCellData(cell.CoordInLod, mesh, constitution);
      this.ReadyAllStitchDependencies(cell);
      if (mesh == null)
        return;
      this.Stitch(cell, vicinity);
    }

    internal void UpdateCellRender(
      MyCellCoord coord,
      MyVoxelClipmap.StitchOperation stitch,
      ref MyVoxelRenderCellData cellData)
    {
      if (this.IsUnloaded)
      {
        using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
        {
          if (stitch != null)
            this.CommitStitchOperation(stitch);
        }
        cellData.Dispose();
      }
      else
        this.m_cellRenderUpdates.Enqueue(new MyVoxelClipmap.CellRenderUpdate(coord, stitch, ref cellData));
    }

    internal bool Stitch(MyCellCoord cell, MyClipmapCellVicinity vicinity)
    {
      if (this.InstanceStitchMode != MyVoxelClipmap.StitchMode.Stitch)
        return false;
      MyVoxelClipmapRing ring = this.Rings[cell.Lod];
      MyVoxelClipmapRing.CellData data;
      if (!ring.TryGetCell(cell.CoordInLod, out data))
        return false;
      if (this.m_stitchWorkTracker.Exists(cell))
      {
        this.m_stitchWorkTracker.Invalidate(cell);
        return true;
      }
      int num = vicinity == data.Vicinity ? 1 : 0;
      int innerCornerIndex;
      MyVoxelClipmap.StitchOperation stitch;
      if (ring.IsInnerLodEdge(cell.CoordInLod, out innerCornerIndex))
      {
        MyVoxelClipmap.CompoundStitchOperation compound;
        using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
          compound = MyVoxelClipmap.m_compoundStitchDependencyPool.Get();
        if (compound == null)
          return false;
        compound.Init(cell);
        this.PrepareStitch(data, cell, preallocatedOperation: ((MyVoxelClipmap.StitchOperation) compound));
        Vector3I cell1 = cell.CoordInLod + MyVoxelClipmap.m_neighbourOffsets[innerCornerIndex];
        if (ring.IsInsideInnerLod(cell1))
          this.CollectChildStitch(compound, data, cell, MyVoxelClipmap.m_neighbourOffsets[innerCornerIndex]);
        stitch = (MyVoxelClipmap.StitchOperation) compound;
      }
      else
        stitch = this.PrepareStitch(data, cell);
      stitch.CachedVicinity = vicinity;
      if (stitch != null && stitch.Pending == (short) 0)
        this.DispatchStitch(stitch);
      return true;
    }

    private void CollectChildStitch(
      MyVoxelClipmap.CompoundStitchOperation compound,
      MyVoxelClipmapRing.CellData parentData,
      MyCellCoord cell,
      Vector3I neighbourOffset)
    {
      Vector3I vector3I1 = cell.CoordInLod + neighbourOffset;
      Vector3I vector3I2 = Vector3I.One - neighbourOffset;
      for (int lod = cell.Lod - 1; lod >= 0; --lod)
      {
        int num = cell.Lod - lod;
        Vector3I vector3I3 = vector3I1 << num;
        Vector3I vector3I4 = vector3I3 + ((1 << num) - 1) * vector3I2 + 1;
        if (this.Rings[lod] == null || !this.Rings[lod].IsInBounds(vector3I3) && !this.Rings[lod].IsInBounds(vector3I4) || this.Rings[lod].Cells.Count<KeyValuePair<Vector3I, MyVoxelClipmapRing.CellData>>() == 0)
          break;
        foreach (Vector3I vector3I5 in Vector3I.EnumerateRange(vector3I3, vector3I4))
        {
          MyVoxelClipmap.StitchOperation preallocatedOperation = MyVoxelClipmap.m_stitchDependencyPool.Get();
          if (preallocatedOperation == null)
            return;
          preallocatedOperation.Init(cell);
          this.PrepareStitch(parentData, new MyCellCoord(lod, vector3I5 - neighbourOffset), compound, preallocatedOperation);
          compound.Children.Add(preallocatedOperation);
          Vector3I min = vector3I5 - vector3I3 << this.CellBits >> num;
          Vector3I max = min + (1 << this.CellBits >> num);
          if (neighbourOffset.X == 1)
            max.X = this.CellSize;
          if (neighbourOffset.Y == 1)
            max.Y = this.CellSize;
          if (neighbourOffset.Z == 1)
            max.Z = this.CellSize;
          preallocatedOperation.Range = new BoundingBoxI?(new BoundingBoxI(min, max));
        }
      }
    }

    private MyVoxelClipmap.StitchOperation PrepareStitch(
      MyVoxelClipmapRing.CellData parentData,
      MyCellCoord cell,
      MyVoxelClipmap.CompoundStitchOperation parent = null,
      MyVoxelClipmap.StitchOperation preallocatedOperation = null)
    {
      MyVoxelClipmap.StitchOperation op = preallocatedOperation;
      if (preallocatedOperation == null)
      {
        using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
          op = MyVoxelClipmap.m_stitchDependencyPool.Get();
        op.Init(cell);
      }
      MyVoxelClipmap.StitchOperation stitchOperation = (MyVoxelClipmap.StitchOperation) parent ?? op;
      int pending = (int) stitchOperation.Pending;
      if (parentData.Status == MyVoxelClipmapRing.CellStatus.Pending)
      {
        op.Dependencies[0] = op.Cell;
        ++stitchOperation.Pending;
      }
      else
      {
        op.Dependencies[0] = MyVoxelClipmap.MakeFulfilled(op.Cell);
        op.Guides[0] = parentData.Guide;
      }
      VrSewOperation self = VrSewOperation.All;
      if (parent != null)
        self = VrSewOperation.None;
      for (int index = 1; index < MyVoxelClipmap.m_neighbourOffsets.Length; ++index)
      {
        MyCellCoord cell1 = new MyCellCoord(cell.Lod, cell.CoordInLod + MyVoxelClipmap.m_neighbourOffsets[index]);
        if (cell1.Lod != op.Cell.Lod)
          op.BorderOperation = true;
        MyVoxelClipmapRing.CellData data;
        if (this.TryGetCellAt(ref cell1, out data))
        {
          if (data.Status == MyVoxelClipmapRing.CellStatus.Pending)
          {
            op.Dependencies[index] = cell1;
            ++stitchOperation.Pending;
          }
          else
          {
            op.Guides[index] = this.CollectMeshForOperation(op, cell1, data);
            op.Dependencies[index] = MyVoxelClipmap.MakeFulfilled(cell1);
          }
        }
        else
        {
          self = self.Without(MyVoxelClipmap.m_compromizes[index]);
          op.Dependencies[index] = MyVoxelClipmap.MakeFulfilled(cell1);
        }
        if (parent != null && cell1.Lod < op.Cell.Lod)
          self = self.With(MyVoxelClipmap.m_compromizes[index]);
      }
      op.Operation = self;
      this.CheckVicinity(op.Dependencies, 1 << this.m_settings.CellSizeLg2);
      int num = 0;
      if ((int) stitchOperation.Pending != pending)
      {
        for (int index = 0; index < op.Dependencies.Length; ++index)
        {
          MyCellCoord dependency = op.Dependencies[index];
          if (dependency.Lod >= 0)
          {
            this.m_stitchDependencies.Add(dependency, stitchOperation);
            ++num;
          }
        }
      }
      return op;
    }

    private bool TryGetCellAt(ref MyCellCoord cell, out MyVoxelClipmapRing.CellData data)
    {
      while (!this.Rings[cell.Lod].TryGetCell(cell.CoordInLod, out data))
      {
        ++cell.Lod;
        cell.CoordInLod >>= 1;
        if (cell.Lod >= this.m_maxLod)
        {
          cell.Lod = 16;
          data = (MyVoxelClipmapRing.CellData) null;
          return false;
        }
      }
      return true;
    }

    private void DispatchStitch(MyVoxelClipmap.StitchOperation stitch)
    {
      MyVoxelClipmap.CompoundStitchOperation compound = stitch.GetCompound();
      if (compound != null)
      {
        this.CollectMeshes(stitch);
        for (int index = compound.Children.Count - 1; index >= 0; --index)
        {
          if (!this.CollectMeshes(compound.Children[index], true))
          {
            compound.Children[index].Start();
            this.CommitStitchOperation(compound.Children[index]);
            compound.Children.RemoveAtFast<MyVoxelClipmap.StitchOperation>(index);
          }
        }
      }
      else if (!this.CollectMeshes(stitch))
      {
        stitch.Start();
        this.CommitStitchOperation(stitch);
        return;
      }
      stitch.Start();
      stitch.GetCompound()?.Children.ForEach((Action<MyVoxelClipmap.StitchOperation>) (x => x.Start()));
      if (!this.m_stitchWorkTracker.Exists(stitch.Cell))
      {
        if (MyClipmapSewJob.Start(this.m_stitchWorkTracker, this, stitch))
          return;
        this.CommitStitchOperation(stitch);
      }
      else
        this.CommitStitchOperation(stitch);
    }

    private VrSewGuide CollectMeshForOperation(
      MyVoxelClipmap.StitchOperation op,
      MyCellCoord cell,
      MyVoxelClipmapRing.CellData cellData)
    {
      if (op.BorderOperation && cellData.Guide == null)
      {
        BoundingBoxI cellBounds = this.GetCellBounds(cell);
        cellData.Guide = new VrSewGuide(cell.Lod, cellBounds.Min, cellBounds.Max, this.GetShellCacheForConstitution(cellData.Constitution));
      }
      return cellData.Guide;
    }

    private bool CollectMeshes(MyVoxelClipmap.StitchOperation stitch, bool child = false)
    {
      bool flag = false;
      MyVoxelClipmapRing.CellData data;
      for (int index = 0; index < stitch.Dependencies.Length; ++index)
      {
        MyCellCoord dependency = stitch.Dependencies[index];
        if (dependency.Lod >= 0)
        {
          stitch.Dependencies[index] = MyVoxelClipmap.MakeFulfilled(stitch.Dependencies[index]);
          if (this.Rings[dependency.Lod].TryGetCell(dependency.CoordInLod, out data))
            stitch.Guides[index] = this.CollectMeshForOperation(stitch, dependency, data);
        }
        if (stitch.Guides[index] != null && stitch.Guides[index].Mesh != null)
          flag = true;
      }
      if (stitch.Guides[0] == null || !flag)
        return false;
      if (!child)
      {
        MyClipmapCellVicinity clipmapCellVicinity = new MyClipmapCellVicinity(stitch.Guides, stitch.Dependencies);
        MyCellCoord myCellCoord = MyVoxelClipmap.MakeFulfilled(stitch.Dependencies[0]);
        this.Rings[myCellCoord.Lod].TryGetCell(myCellCoord.CoordInLod, out data);
        if (data.Vicinity == clipmapCellVicinity)
          return false;
        if (stitch.CachedVicinity == clipmapCellVicinity)
          stitch.Operation = VrSewOperation.None;
        data.Vicinity = clipmapCellVicinity;
      }
      if (!MyIsoMeshTaylor.CheckVicinity(stitch.Guides))
      {
        VrSewGuide[] guides = stitch.Guides;
        int num = ((IEnumerable<VrSewGuide>) guides).Where<VrSewGuide>((Func<VrSewGuide, bool>) (x => x != null)).Min<VrSewGuide>((Func<VrSewGuide, int>) (x => x.Lod));
        Vector3I vector3I1 = guides[0].End << guides[0].Lod - num;
        for (int index = 1; index < 8; ++index)
        {
          if (guides[index] != null && guides[index] != guides[0])
          {
            Vector3I vector3I2 = guides[index].Start << guides[index].Lod - num;
            if (vector3I1.X != vector3I2.X && vector3I1.Y != vector3I2.Y && vector3I1.Z != vector3I2.Z)
              MyLog.Default.Error(string.Format("Clipmap for {0}:\nMeshes {1} and {2} do not meet anywhere : m0 end {3}, m{4} start {5}.\nTheir cell ids are {6} and {7}, respectively.", (object) this.Mesher.StorageName, (object) guides[0], (object) guides[index], (object) vector3I1, (object) index, (object) vector3I2, (object) stitch.Dependencies[0], (object) stitch.Dependencies[index]));
          }
        }
      }
      return true;
    }

    private void ReadyStitchDependency(MyVoxelClipmap.StitchOperation stitch)
    {
      --stitch.Pending;
      if (stitch.Pending != (short) 0)
        return;
      this.DispatchStitch(stitch);
    }

    internal void CommitStitchOperation(MyVoxelClipmap.StitchOperation stitch, bool dereference = true)
    {
      MyVoxelClipmap.CompoundStitchOperation compound = stitch.GetCompound();
      if (compound != null)
      {
        foreach (MyVoxelClipmap.StitchOperation child in compound.Children)
        {
          child.Clear(dereference);
          MyVoxelClipmap.m_stitchDependencyPool.Return(child);
        }
        stitch.Clear(dereference);
        MyVoxelClipmap.m_compoundStitchDependencyPool.Return(compound);
      }
      else
      {
        stitch.Clear(dereference);
        MyVoxelClipmap.m_stitchDependencyPool.Return(stitch);
      }
    }

    private void ReadyAllStitchDependencies(MyCellCoord cell)
    {
      List<MyVoxelClipmap.StitchOperation> list;
      if (!this.m_stitchDependencies.TryGet(cell, out list))
        return;
      for (int index = 0; index < list.Count; ++index)
        this.ReadyStitchDependency(list[index]);
      this.m_stitchDependencies.Remove(cell);
    }

    private bool CheckVicinity(MyCellCoord[] cells, int cellSize)
    {
      MyCellCoord cell1 = Neutralize(cells[0]);
      Vector3I vector3I1 = CellMax(cell1);
      for (int index = 1; index < 8; ++index)
      {
        MyCellCoord cell2 = Neutralize(cells[index]);
        if (cell2.Lod < 16 && !(cell2 == cell1))
        {
          Vector3I vector3I2 = CellMin(cell2);
          if (vector3I1.X != vector3I2.X && vector3I1.Y != vector3I2.Y && vector3I1.Z != vector3I2.Z)
          {
            MyLog.Default.Error(string.Format("Clipmap for {0}:\nCells {1} and {2} do not meet anywhere : c0 end {3}, c{4} start {5}.", (object) this.Mesher.StorageName, (object) cell1, (object) cell2, (object) vector3I1, (object) index, (object) vector3I2));
            return false;
          }
        }
      }
      return true;

      Vector3I CellMin(MyCellCoord cell) => cell.CoordInLod * cellSize << cell.Lod;

      Vector3I CellMax(MyCellCoord cell) => (cell.CoordInLod + 1) * cellSize << cell.Lod;

      MyCellCoord Neutralize(MyCellCoord cell) => cell.Lod >= 0 ? cell : MyVoxelClipmap.MakeFulfilled(cell);
    }

    internal static MyCellCoord MakeFulfilled(MyCellCoord fullfiled) => new MyCellCoord(~fullfiled.Lod, fullfiled.CoordInLod);

    private VrShellDataCache GetShellCacheForConstitution(
      MyVoxelContentConstitution constitution)
    {
      return constitution != MyVoxelContentConstitution.Empty ? VrShellDataCache.Full : VrShellDataCache.Empty;
    }

    public bool IsUnloaded => this.m_updateState == MyVoxelClipmap.UpdateState.Unloaded;

    public MyVoxelClipmap.StitchMode InstanceStitchMode { get; private set; }

    public IEnumerable<IMyVoxelActorCell> Cells => ((IEnumerable<MyVoxelClipmapRing>) this.Rings).SelectMany<MyVoxelClipmapRing, MyVoxelClipmapRing.CellData>((Func<MyVoxelClipmapRing, IEnumerable<MyVoxelClipmapRing.CellData>>) (x => x.Cells.Values)).Select<MyVoxelClipmapRing.CellData, IMyVoxelActorCell>((Func<MyVoxelClipmapRing.CellData, IMyVoxelActorCell>) (x => x.Cell)).Where<IMyVoxelActorCell>((Func<IMyVoxelActorCell, bool>) (x => x != null));

    public IMyVoxelActor Actor { get; private set; }

    public Vector3I Size => this.m_voxelSize;

    private event Action<IMyLodController> m_loaded;

    public event Action<IMyLodController> Loaded
    {
      add
      {
        this.m_loaded += value;
        Interlocked.Exchange(ref this.m_loadedCounter, 5);
      }
      remove => this.m_loaded -= value;
    }

    public MatrixD LocalToWorld
    {
      get => this.m_localToWorld;
      set => this.m_localToWorld = value;
    }

    private bool WorkPending => this.m_stitchDependencies.KeyCount != 0 || this.m_stitchWorkTracker.HasAny || (this.m_dataWorkTracker.HasAny || this.m_fullWorkTracker.HasAny) || (uint) this.m_cellRenderUpdates.Count > 0U;

    public void Update(
      ref MatrixD view,
      BoundingFrustumD viewFrustum,
      float farClipping,
      bool smoothMotion)
    {
      if (this.Mesher.Storage == null || this.m_updateState == MyVoxelClipmap.UpdateState.NotReady)
        return;
      using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
      {
        this.ProcessUpdates();
        if (this.WorkPending)
          return;
        this.InstanceStitchMode = MyVoxelClipmap.ActiveStitchMode;
        switch (this.m_updateState)
        {
          case MyVoxelClipmap.UpdateState.Idle:
            if (this.m_settingsChanged)
            {
              this.ApplySettings(true);
              break;
            }
            if (MyVoxelClipmap.UpdateVisibility && !MyRenderProxy.Settings.FreezeTerrainQueries && this.MoveUpdate(ref view, viewFrustum, farClipping, smoothMotion))
            {
              this.m_updateState = MyVoxelClipmap.UpdateState.Calculate;
              break;
            }
            if (this.m_invalidateRange.HasValue)
            {
              this.InvalidateInternal(this.m_invalidateRange.Value);
              this.m_invalidateRange = new BoundingBoxI?();
              break;
            }
            if (this.m_loadedCounter == 0 || this.m_loaded == null)
              break;
            Interlocked.Decrement(ref this.m_loadedCounter);
            if (this.m_loadedCounter != 0)
              break;
            MyRenderProxy.EnqueueMainThreadCallback((Action) (() =>
            {
              Action<IMyLodController> loaded = this.m_loaded;
              if (loaded == null)
                return;
              loaded((IMyLodController) this);
            }));
            break;
          case MyVoxelClipmap.UpdateState.Calculate:
            if (this.WorkPending)
              break;
            this.m_updateState = MyVoxelClipmap.UpdateState.Idle;
            bool justLoaded = this.m_loaded != null;
            if (justLoaded)
              MyRenderProxy.EnqueueMainThreadCallback((Action) (() =>
              {
                Action<IMyLodController> loaded = this.m_loaded;
                if (loaded == null)
                  return;
                loaded((IMyLodController) this);
              }));
            MyVoxelClipmapCache cache = this.m_cache;
            this.Actor.EndBatch(justLoaded);
            break;
        }
      }
    }

    private bool MoveUpdate(
      ref MatrixD view,
      BoundingFrustumD viewFrustum,
      float farClipping,
      bool smooth)
    {
      Vector3D xyz = Vector3D.Transform(view.Translation, this.m_worldToLocal) / 1.0;
      if (Vector3D.DistanceSquared(xyz, (Vector3D) this.m_lastPosition) < (double) this.m_updateDistance)
        return false;
      Vector3L relativePosition = new Vector3L(xyz);
      this.m_lastPosition = relativePosition;
      if (!this.Actor.IsBatching)
        this.Actor.BeginBatch(new MyVoxelActorTransitionMode?(smooth ? MyVoxelActorTransitionMode.Fade : MyVoxelActorTransitionMode.Immediate));
      foreach (MyVoxelClipmapRing ring in this.Rings)
        ring.Update(relativePosition);
      foreach (MyVoxelClipmapRing ring in this.Rings)
        ring.ProcessChanges();
      foreach (MyVoxelClipmapRing ring in this.Rings)
        ring.DispatchStitchingRefreshes();
      this.ProcessCacheHits();
      return true;
    }

    public void BindToActor(IMyVoxelActor actor)
    {
      this.Actor = this.Actor == null ? actor : throw new InvalidOperationException("Lod Controller is already bound to actor.");
      this.Actor.TransitionMode = MyVoxelActorTransitionMode.Fade;
      this.Actor.Move += new ActionRef<MatrixD>(this.UpdateWorldMatrix);
      this.BindToCache();
      this.m_updateState = MyVoxelClipmap.UpdateState.Idle;
    }

    public void Unload()
    {
      using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
      {
        this.m_dataWorkTracker.CancelAll();
        this.m_fullWorkTracker.CancelAll();
        this.m_stitchWorkTracker.CancelAll();
        this.m_updateState = MyVoxelClipmap.UpdateState.Unloaded;
        this.Actor.Move -= new ActionRef<MatrixD>(this.UpdateWorldMatrix);
        this.ProcessUpdates();
        foreach (MyVoxelClipmap.StitchOperation stitch in this.m_stitchDependencies.Values.SelectMany<List<MyVoxelClipmap.StitchOperation>, MyVoxelClipmap.StitchOperation>((Func<List<MyVoxelClipmap.StitchOperation>, IEnumerable<MyVoxelClipmap.StitchOperation>>) (x => (IEnumerable<MyVoxelClipmap.StitchOperation>) x)).GroupBy<MyVoxelClipmap.StitchOperation, MyVoxelClipmap.StitchOperation>((Func<MyVoxelClipmap.StitchOperation, MyVoxelClipmap.StitchOperation>) (x => x)).Select<IGrouping<MyVoxelClipmap.StitchOperation, MyVoxelClipmap.StitchOperation>, MyVoxelClipmap.StitchOperation>((Func<IGrouping<MyVoxelClipmap.StitchOperation, MyVoxelClipmap.StitchOperation>, MyVoxelClipmap.StitchOperation>) (x => x.Key)))
        {
          stitch.Pending = (short) 0;
          this.CommitStitchOperation(stitch, false);
        }
        foreach (MyVoxelClipmapRing ring in this.Rings)
          ring.InvalidateAll();
        this.m_stitchDependencies.Clear();
        this.m_cache?.Unregister(this.Actor.Id);
        if (this.m_loaded == null)
          return;
        MyRenderProxy.EnqueueMainThreadCallback((Action) (() => this.m_loaded.InvokeIfNotNull<IMyLodController>((IMyLodController) this)));
      }
    }

    public void InvalidateRange(Vector3I min, Vector3I max)
    {
      BoundingBoxI box = new BoundingBoxI(min - 1, max + 1);
      using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
      {
        if (this.m_updateState == MyVoxelClipmap.UpdateState.NotReady)
          return;
        if (this.m_invalidateRange.HasValue)
          this.m_invalidateRange = new BoundingBoxI?(this.m_invalidateRange.Value.Include(box));
        else
          this.m_invalidateRange = new BoundingBoxI?(box);
      }
    }

    private void InvalidateInternal(BoundingBoxI bounds)
    {
      if (bounds == BoundingBoxI.CreateInvalid())
      {
        if (this.m_cache != null)
          this.m_cache.EvictAll(this.Actor.Id);
        this.Actor.BeginBatch(new MyVoxelActorTransitionMode?(MyVoxelActorTransitionMode.Fade));
        foreach (MyVoxelClipmapRing ring in this.Rings)
          ring.InvalidateAll();
        this.m_lastPosition = (Vector3L) Vector3I.MinValue;
      }
      else
      {
        if (this.m_cache != null)
          this.m_cache.EvictAll(this.Actor.Id, new BoundingBoxI(bounds.Min >> this.CellBits, bounds.Max + (this.CellSize - 1) >> this.CellBits));
        this.Actor.BeginBatch(new MyVoxelActorTransitionMode?(MyVoxelActorTransitionMode.Immediate));
        foreach (MyVoxelClipmapRing ring in this.Rings)
          ring.InvalidateRange(bounds);
        this.m_updateState = MyVoxelClipmap.UpdateState.Calculate;
      }
    }

    public void InvalidateAll()
    {
      using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
      {
        if (this.m_updateState == MyVoxelClipmap.UpdateState.NotReady)
          return;
        this.m_invalidateRange = new BoundingBoxI?(BoundingBoxI.CreateInvalid());
      }
    }

    ~MyVoxelClipmap()
    {
    }

    [Conditional("TRACK_OPERATIONS")]
    private void Record(MyVoxelClipmap.Op operation)
    {
      if (this.m_debugWorkQueue.Count == 256)
        this.m_debugWorkQueue.Dequeue();
      this.m_debugWorkQueue.Enqueue(operation);
    }

    [Conditional("TRACK_OPERATIONS")]
    internal void DumpWorkLog()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Work log for clipmap on storage: " + this.Mesher.StorageName);
      stringBuilder.AppendLine("Clipmap Settings:\n\tSettingsGroup: " + this.SettingsGroup + "\n\t" + this.m_settings.ToString().Replace("\n", "\n\t"));
      stringBuilder.AppendLine("Log:");
      foreach (MyVoxelClipmap.Op debugWork in this.m_debugWorkQueue)
        stringBuilder.AppendLine(debugWork.ToString());
      MyLog.Default.WriteLine(stringBuilder.ToString());
    }

    public void DebugDraw(ref MatrixD cameraMatrix)
    {
      using (this.m_lock.AcquireExclusiveRecursiveUsing(this.m_lockOwner))
      {
        foreach (MyVoxelClipmapRing ring in this.Rings)
          ring.DebugDraw();
        if (!MyVoxelClipmap.DebugDrawDependencies)
          return;
        this.DebugDrawDependenciesInternal();
      }
    }

    private void DebugDrawDependenciesInternal()
    {
      Vector3L lastPosition = this.m_lastPosition;
      foreach (KeyValuePair<MyCellCoord, List<MyVoxelClipmap.StitchOperation>> stitchDependency in (MyCollectionDictionary<MyCellCoord, List<MyVoxelClipmap.StitchOperation>, MyVoxelClipmap.StitchOperation>) this.m_stitchDependencies)
      {
        Vector3D cellCenter1 = this.GetCellCenter(stitchDependency.Key);
        float a = 1f;
        if ((double) a >= 0.0)
        {
          Color color = new Color(Color.Orange, a);
          MyCellCoord key = stitchDependency.Key;
          MyVoxelClipmapRing.CellData data;
          if (this.Rings[key.Lod].TryGetCell(key.CoordInLod, out data))
            MyRenderProxy.DebugDrawText3D(cellCenter1, string.Format("Status: {0}, Constitution: {1}", (object) data.Status, (object) data.Constitution), color, 0.7f, true);
          foreach (MyVoxelClipmap.StitchOperation stitchOperation in stitchDependency.Value)
          {
            Vector3D cellCenter2 = this.GetCellCenter(stitchOperation.Cell);
            color = new Color(Color.Orange, a);
            MyRenderProxy.DebugDrawArrow3D(cellCenter2, cellCenter1, color, new Color?(color), true);
            MyRenderProxy.DebugDrawText3D(cellCenter2, string.Format("Stitch dependency \npending: {0}", (object) stitchOperation.Pending), color, 0.7f, true, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          }
        }
      }
    }

    private Vector3D GetCellCenter(MyCellCoord cell) => Vector3D.Transform((Vector3D) (cell.CoordInLod * this.CellSize + (this.CellSize >> 1) << cell.Lod), this.LocalToWorld);

    public VrVoxelMesh GetCachedMesh(Vector3I coord, int lod)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        MyVoxelClipmapRing ring = this.Rings[lod];
        coord >>= lod + this.CellBits;
        Vector3I cell = coord;
        MyVoxelClipmapRing.CellData cellData;
        ref MyVoxelClipmapRing.CellData local = ref cellData;
        if (ring.TryGetCell(cell, out local))
        {
          if (cellData.Guide != null)
            return cellData.Guide.Mesh;
        }
      }
      return (VrVoxelMesh) null;
    }

    public VrVoxelMesh GetCachedMesh(Vector3I coord)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        for (int index = 0; index < this.m_maxLod; ++index)
        {
          MyVoxelClipmapRing.CellData data;
          if (this.Rings[index].TryGetCell(coord >> index + this.CellBits, out data) && data.Guide != null)
            return data.Guide.Mesh;
        }
      }
      return (VrVoxelMesh) null;
    }

    public BoundingBoxI GetCellBounds(MyCellCoord cell, bool inLod = true)
    {
      int cellSize = this.CellSize;
      Vector3I min = cell.CoordInLod * cellSize;
      Vector3I max = min + cellSize;
      switch (this.InstanceStitchMode)
      {
        case MyVoxelClipmap.StitchMode.None:
        case MyVoxelClipmap.StitchMode.Stitch:
          if (!inLod)
          {
            min <<= cell.Lod;
            max <<= cell.Lod;
          }
          return new BoundingBoxI(min, max);
        case MyVoxelClipmap.StitchMode.BlindMeet:
          max += 1;
          goto case MyVoxelClipmap.StitchMode.None;
        case MyVoxelClipmap.StitchMode.Overlap:
          max += 2;
          goto case MyVoxelClipmap.StitchMode.None;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public float? SpherizeRadius => this.m_spherizeRadius;

    public Vector3D SpherizePosition => this.m_spherizePosition;

    private struct CellRenderUpdate
    {
      public readonly MyCellCoord Cell;
      public readonly MyVoxelClipmap.StitchOperation Operation;
      public MyVoxelRenderCellData Data;

      public CellRenderUpdate(
        MyCellCoord cell,
        MyVoxelClipmap.StitchOperation operation,
        ref MyVoxelRenderCellData data)
      {
        this.Cell = cell;
        this.Data = data;
        this.Operation = operation;
      }
    }

    [GenerateActivator]
    internal class StitchOperation
    {
      public MyCellCoord[] Dependencies = new MyCellCoord[8];
      public VrSewGuide[] Guides = new VrSewGuide[8];
      public VrSewOperation Operation;
      public BoundingBoxI? Range;
      public short Pending;
      public bool Recalculate;
      public bool BorderOperation;
      private MyVoxelClipmap.StitchOperation.OpState m_state;
      public MyClipmapCellVicinity CachedVicinity;

      public MyCellCoord Cell { get; private set; }

      public virtual void Init(MyCellCoord coord)
      {
        this.Cell = coord;
        this.SetState(MyVoxelClipmap.StitchOperation.OpState.Pending);
      }

      public void Start()
      {
        for (int index = 0; index < this.Guides.Length; ++index)
        {
          if (this.Guides[index] != null)
            this.Guides[index].AddReference();
        }
        this.SetState(MyVoxelClipmap.StitchOperation.OpState.Queued);
      }

      public virtual void Clear(bool dereference = true)
      {
        for (int index = 0; index < this.Guides.Length; ++index)
        {
          if (dereference && this.Guides[index] != null)
            this.Guides[index].RemoveReference();
          this.Guides[index] = (VrSewGuide) null;
        }
        this.Recalculate = false;
        if (this.Range.HasValue)
          this.Range = new BoundingBoxI?();
        this.BorderOperation = false;
        this.CachedVicinity = MyClipmapCellVicinity.Invalid;
        this.SetState(MyVoxelClipmap.StitchOperation.OpState.Pooled);
      }

      public virtual MyVoxelClipmap.CompoundStitchOperation GetCompound() => (MyVoxelClipmap.CompoundStitchOperation) null;

      public virtual void SetState(MyVoxelClipmap.StitchOperation.OpState value) => this.m_state = value;

      public MyVoxelClipmap.StitchOperation.OpState State => this.m_state;

      public enum OpState
      {
        Pooled,
        Pending,
        Queued,
        Working,
        Ready,
        Returned,
      }

      private class VRage_Voxels_Clipmap_MyVoxelClipmap\u003C\u003EStitchOperation\u003C\u003EActor : IActivator, IActivator<MyVoxelClipmap.StitchOperation>
      {
        object IActivator.CreateInstance() => (object) new MyVoxelClipmap.StitchOperation();

        MyVoxelClipmap.StitchOperation IActivator<MyVoxelClipmap.StitchOperation>.CreateInstance() => new MyVoxelClipmap.StitchOperation();
      }
    }

    internal class CompoundStitchOperation : MyVoxelClipmap.StitchOperation
    {
      public List<MyVoxelClipmap.StitchOperation> Children = new List<MyVoxelClipmap.StitchOperation>();

      public override void Init(MyCellCoord coord) => base.Init(coord);

      public override void Clear(bool dereference = true)
      {
        base.Clear(dereference);
        this.Children.Clear();
      }

      public override MyVoxelClipmap.CompoundStitchOperation GetCompound() => this;

      public override void SetState(MyVoxelClipmap.StitchOperation.OpState value)
      {
        base.SetState(value);
        for (int index = 0; index < this.Children.Count; ++index)
          this.Children[index].SetState(value);
      }

      private class VRage_Voxels_Clipmap_MyVoxelClipmap\u003C\u003ECompoundStitchOperation\u003C\u003EActor : IActivator, IActivator<MyVoxelClipmap.CompoundStitchOperation>
      {
        object IActivator.CreateInstance() => (object) new MyVoxelClipmap.CompoundStitchOperation();

        MyVoxelClipmap.CompoundStitchOperation IActivator<MyVoxelClipmap.CompoundStitchOperation>.CreateInstance() => new MyVoxelClipmap.CompoundStitchOperation();
      }
    }

    private enum UpdateState
    {
      NotReady,
      Idle,
      Calculate,
      Unloaded,
    }

    private class Op
    {
      public class InvalidateAll : MyVoxelClipmap.Op
      {
        public override string ToString() => nameof (InvalidateAll);
      }

      public class InvalidateRange : MyVoxelClipmap.Op
      {
        public BoundingBoxI Range;

        public InvalidateRange(BoundingBoxI range) => this.Range = range;

        public override string ToString() => string.Format("InvalidateRange: {0}", (object) this.Range);
      }

      public class Update : MyVoxelClipmap.Op
      {
        public Vector3L CameraPosition;

        public Update(Vector3L cameraPosition) => this.CameraPosition = cameraPosition;

        public override string ToString() => string.Format("Update: {0}", (object) this.CameraPosition);
      }
    }

    public enum StitchMode
    {
      None,
      BlindMeet,
      Overlap,
      Stitch,
    }
  }
}
