// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyVoxelClipmapCache
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Library.Collections;
using VRage.Utils;
using VRage.Voxels.Sewing;
using VRageMath;

namespace VRage.Voxels.Clipmap
{
  public class MyVoxelClipmapCache
  {
    public static int DefaultCacheSize = 1024;
    private static MyVoxelClipmapCache m_instance;
    private readonly LRUCache<MyVoxelClipmapCache.CellKey, MyVoxelClipmapCache.CellData> m_cells;
    private readonly ConcurrentDictionary<uint, MyVoxelClipmap> m_evictionHandlers = new ConcurrentDictionary<uint, MyVoxelClipmap>();
    private int m_lodThreshold;
    private MyDebugHitCounter m_hitCounter = new MyDebugHitCounter();
    private long m_hits;
    private long m_tries;

    public static MyVoxelClipmapCache Instance => MyVoxelClipmapCache.m_instance ?? (MyVoxelClipmapCache.m_instance = new MyVoxelClipmapCache(MyVoxelClipmapCache.DefaultCacheSize));

    public MyVoxelClipmapCache(int maxCachedCells, int lodThreshold = 6)
    {
      this.m_lodThreshold = lodThreshold;
      this.m_cells = new LRUCache<MyVoxelClipmapCache.CellKey, MyVoxelClipmapCache.CellData>(maxCachedCells, MyVoxelClipmapCache.CellKey.Comparer);
      this.m_cells.OnItemDiscarded += (Action<MyVoxelClipmapCache.CellKey, MyVoxelClipmapCache.CellData>) ((key, cell) =>
      {
        if (cell.Guide == null)
          return;
        cell.Guide.RemoveReference();
        this.m_evictionHandlers[key.ClipmapId].HandleCacheEviction(key.Coord, cell.Guide);
      });
    }

    public int LodThreshold
    {
      get => this.m_lodThreshold;
      set => this.m_lodThreshold = MathHelper.Clamp(value, 0, 15);
    }

    public float CacheUtilization => this.m_cells.Usage;

    public float HitRate
    {
      get
      {
        float[] array = this.m_hitCounter.Select<MyDebugHitCounter.Sample, float>((Func<MyDebugHitCounter.Sample, float>) (x => x.Value)).Where<float>((Func<float, bool>) (x => !float.IsNaN(x))).ToArray<float>();
        return array.Length == 0 ? 0.0f : ((IEnumerable<float>) array).Average();
      }
    }

    public void Register(uint clipmapId, MyVoxelClipmap clipmap) => this.m_evictionHandlers.TryAdd(clipmapId, clipmap);

    public void Unregister(uint clipmapId)
    {
      this.EvictAll(clipmapId);
      this.m_evictionHandlers.Remove<uint, MyVoxelClipmap>(clipmapId);
    }

    public void EvictAll(uint clipmapId)
    {
      if (!this.m_evictionHandlers.ContainsKey(clipmapId))
        throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
      this.m_cells.RemoveWhere((Func<MyVoxelClipmapCache.CellKey, MyVoxelClipmapCache.CellData, bool>) ((k, v) => (int) k.ClipmapId == (int) clipmapId));
    }

    public unsafe void EvictAll(uint clipmapId, BoundingBoxI range)
    {
      if (!this.m_evictionHandlers.ContainsKey(clipmapId))
        throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
      if (range.Size.Size > this.m_cells.Count)
      {
        MyVoxelClipmapCache.EvictionRanges evictionRanges;
        BoundingBoxI* ranges = evictionRanges.Ranges;
        for (int index = 0; index <= this.LodThreshold; ++index)
          ranges[index] = new BoundingBoxI(range.Min >> index, range.Max + ((1 << index) - 1) >> index);
        this.m_cells.RemoveWhere((Func<MyVoxelClipmapCache.CellKey, MyVoxelClipmapCache.CellData, bool>) ((k, v) => (int) k.ClipmapId == (int) clipmapId && ranges[k.Coord.Lod].Contains(k.Coord.CoordInLod) == ContainmentType.Contains));
      }
      else
      {
        for (int lod = 0; lod <= this.m_lodThreshold; ++lod)
        {
          foreach (Vector3I enumeratePoint in BoundingBoxI.EnumeratePoints(new BoundingBoxI(range.Min >> lod, range.Max + ((1 << lod) - 1) >> lod)))
            this.m_cells.Remove(new MyVoxelClipmapCache.CellKey(clipmapId, new MyCellCoord(lod, enumeratePoint)));
        }
      }
    }

    public bool TryRead(
      uint clipmapId,
      MyCellCoord cell,
      out VrSewGuide data,
      out MyClipmapCellVicinity vicinity,
      out MyVoxelContentConstitution constitution)
    {
      if (!this.m_evictionHandlers.ContainsKey(clipmapId))
        throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
      if (cell.Lod > this.m_lodThreshold)
      {
        data = (VrSewGuide) null;
        vicinity = MyClipmapCellVicinity.Invalid;
        constitution = MyVoxelContentConstitution.Empty;
        return false;
      }
      MyVoxelClipmapCache.CellData cellData;
      if (this.m_cells.TryRead(new MyVoxelClipmapCache.CellKey(clipmapId, cell), out cellData))
      {
        data = cellData.Guide;
        vicinity = cellData.Vicinity;
        constitution = cellData.Constitution;
        return true;
      }
      data = (VrSewGuide) null;
      vicinity = MyClipmapCellVicinity.Invalid;
      constitution = MyVoxelContentConstitution.Empty;
      return false;
    }

    public bool IsCached(uint clipmapId, MyCellCoord cell, VrSewGuide dataGuide)
    {
      if (!this.m_evictionHandlers.ContainsKey(clipmapId))
        throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
      MyVoxelClipmapCache.CellData cellData;
      return cell.Lod <= this.m_lodThreshold && this.m_cells.TryPeek(new MyVoxelClipmapCache.CellKey(clipmapId, cell), out cellData) && cellData.Guide == dataGuide;
    }

    public void Write(
      uint clipmapId,
      MyCellCoord cell,
      VrSewGuide guide,
      MyClipmapCellVicinity vicinity,
      MyVoxelContentConstitution constitution)
    {
      if (!this.m_evictionHandlers.ContainsKey(clipmapId))
        throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
      if (cell.Lod > this.m_lodThreshold)
        return;
      guide?.AddReference();
      this.m_cells.Write(new MyVoxelClipmapCache.CellKey(clipmapId, cell), new MyVoxelClipmapCache.CellData(guide, constitution, vicinity));
    }

    [Conditional("DEBUG")]
    internal void CycleDebugCounters() => this.m_hitCounter.Cycle();

    public MyDebugHitCounter DebugHitCounter => this.m_hitCounter;

    [Conditional("DEBUG")]
    private void Hit()
    {
      Interlocked.Increment(ref this.m_hits);
      Interlocked.Increment(ref this.m_tries);
    }

    [Conditional("DEBUG")]
    private void Miss() => Interlocked.Increment(ref this.m_tries);

    private struct CellKey
    {
      public readonly uint ClipmapId;
      public readonly MyCellCoord Coord;
      private static readonly IEqualityComparer<MyVoxelClipmapCache.CellKey> ComparerInstance = (IEqualityComparer<MyVoxelClipmapCache.CellKey>) new MyVoxelClipmapCache.CellKey.ClipmapIdCoordEqualityComparer();

      public CellKey(uint clipmapId, MyCellCoord cell)
      {
        this.ClipmapId = clipmapId;
        this.Coord = cell;
      }

      public static IEqualityComparer<MyVoxelClipmapCache.CellKey> Comparer => MyVoxelClipmapCache.CellKey.ComparerInstance;

      private sealed class ClipmapIdCoordEqualityComparer : IEqualityComparer<MyVoxelClipmapCache.CellKey>
      {
        public bool Equals(MyVoxelClipmapCache.CellKey x, MyVoxelClipmapCache.CellKey y) => (int) x.ClipmapId == (int) y.ClipmapId && x.Coord.Equals(y.Coord);

        public int GetHashCode(MyVoxelClipmapCache.CellKey obj) => (int) obj.ClipmapId * 397 ^ obj.Coord.GetHashCode();
      }
    }

    private struct CellData
    {
      public VrSewGuide Guide;
      public MyVoxelContentConstitution Constitution;
      public MyClipmapCellVicinity Vicinity;

      public CellData(
        VrSewGuide guide,
        MyVoxelContentConstitution constitution,
        MyClipmapCellVicinity vicinity)
      {
        this.Guide = guide;
        this.Constitution = constitution;
        this.Vicinity = vicinity;
      }
    }

    private struct EvictionRanges
    {
      private const int StructSize = 24;
      private unsafe fixed byte m_data[384];

      public unsafe BoundingBoxI* Ranges
      {
        get
        {
          fixed (byte* numPtr = this.m_data)
            return (BoundingBoxI*) numPtr;
        }
      }
    }
  }
}
