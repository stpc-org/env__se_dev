// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyStorageBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels.Storage;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  public abstract class MyStorageBase : VRage.Game.Voxels.IMyStorage, VRage.ModAPI.IMyStorage
  {
    private const int ACCESS_GRID_LOD_DEFAULT = 10;
    private int m_accessGridLod;
    private int m_streamGridLod = (int) ushort.MaxValue;
    private readonly ConcurrentDictionary<Vector3I, MyTimeSpan> m_access = new ConcurrentDictionary<Vector3I, MyTimeSpan>();
    private const int MaxChunksToDiscard = 10;
    private const int MaximumHitsForDiscard = 100;
    private MyConcurrentQueue<Vector3I> m_pendingChunksToWrite;
    private MyQueue<Vector3I> m_chunksbyAge;
    private MyConcurrentDictionary<Vector3I, MyStorageBase.VoxelChunk> m_cachedChunks;
    private MyDynamicAABBTree m_cacheMap;
    private FastResourceLock m_cacheLock;
    [ThreadStatic]
    private static List<MyStorageBase.VoxelChunk> m_tmpChunks;
    private bool m_writeCacheThrough;
    private const int WriteCacheCap = 1024;
    private const int MaxWriteJobWorkMillis = 6;
    private bool m_cachedWrites;
    public const int STORAGE_TYPE_VERSION_CELL = 2;
    private const string STORAGE_TYPE_NAME_CELL = "Cell";
    private const string STORAGE_TYPE_NAME_OCTREE = "Octree";
    protected const int STORAGE_TYPE_VERSION_OCTREE = 1;
    protected const int STORAGE_TYPE_VERSION_OCTREE_ACCESS = 2;
    private readonly object m_compressedDataLock = new object();
    private byte[] m_cachedData;
    private bool m_cachedDataCompressed;
    private bool m_setDataCacheAllowed;
    private readonly MyVoxelGeometry m_geometry = new MyVoxelGeometry();
    protected readonly FastResourceLock StorageLock = new FastResourceLock();
    public static bool UseStorageCache = true;
    private static readonly LRUCache<int, Lazy<MyStorageBase>> m_storageCache = new LRUCache<int, Lazy<MyStorageBase>>(512);
    private static int m_runningStorageId = -1;
    private const int CLOSE_MASK = -2147483648;
    private const int PIN_MASK = 2147483647;
    private int m_pinAndCloseMark;
    private int m_closed;

    public IEnumerator<KeyValuePair<Vector3I, MyTimeSpan>> AccessEnumerator => this.m_access.GetEnumerator();

    public void ConvertAccessCoordinates(ref Vector3I coord, out BoundingBoxD bb)
    {
      MyCellCoord myCellCoord = new MyCellCoord(this.m_accessGridLod, ref coord);
      Vector3I vector3I = myCellCoord.CoordInLod << myCellCoord.Lod;
      float num = 1f * (float) (1 << myCellCoord.Lod);
      Vector3 vector3 = vector3I * 1f + 0.5f * num;
      bb = new BoundingBoxD((Vector3D) (vector3 - 0.5f * num), (Vector3D) (vector3 + 0.5f * num));
    }

    public void AccessDeleteFirst()
    {
      if (this.m_access.IsEmpty)
        return;
      Vector3I key = this.m_access.First<KeyValuePair<Vector3I, MyTimeSpan>>().Key;
      this.AccessDelete(ref key, MyStorageDataTypeFlags.ContentAndMaterial);
    }

    public void MarkClear(ref Vector3I coord)
    {
      this.GetAccessRange(coord, out Vector3I _, out Vector3I _);
      MyCellCoord coord1 = new MyCellCoord(this.m_accessGridLod, coord);
      this.AccessRange(MyStorageBase.MyAccessType.Delete, MyStorageDataTypeEnum.Content, ref coord1);
      this.AccessRange(MyStorageBase.MyAccessType.Delete, MyStorageDataTypeEnum.Material, ref coord1);
    }

    public void AccessDelete(ref Vector3I coord, MyStorageDataTypeFlags dataType, bool notify = true)
    {
      Vector3I min;
      Vector3I max;
      this.GetAccessRange(coord, out min, out max);
      this.DeleteRange(dataType, min, max, notify);
      MyCellCoord coord1 = new MyCellCoord(this.m_accessGridLod, coord);
      if ((dataType & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
        this.AccessRange(MyStorageBase.MyAccessType.Delete, MyStorageDataTypeEnum.Content, ref coord1);
      if ((dataType & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      this.AccessRange(MyStorageBase.MyAccessType.Delete, MyStorageDataTypeEnum.Material, ref coord1);
    }

    public void GetAccessRange(Vector3I coord, out Vector3I min, out Vector3I max)
    {
      int num = 1 << this.m_accessGridLod;
      min = coord << this.m_accessGridLod;
      max = min + num;
    }

    protected void AccessReset()
    {
      this.m_access.Clear();
      int num = 0;
      Vector3I size = this.Size;
      while (size != Vector3I.Zero)
      {
        size >>= 1;
        ++num;
      }
      this.m_accessGridLod = Math.Min(num - 1, 10);
    }

    public void AccessRange(
      MyStorageBase.MyAccessType accessType,
      MyStorageDataTypeEnum dataType,
      ref MyCellCoord coord)
    {
      if (coord.Lod != this.m_accessGridLod || dataType != MyStorageDataTypeEnum.Content)
        return;
      if (accessType == MyStorageBase.MyAccessType.Write)
      {
        MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
        this.m_access[coord.CoordInLod] = myTimeSpan;
      }
      else
      {
        if (accessType != MyStorageBase.MyAccessType.Delete)
          return;
        this.m_access.Remove<Vector3I, MyTimeSpan>(coord.CoordInLod);
      }
    }

    protected void ReadStorageAccess(Stream stream) => this.m_streamGridLod = (int) stream.ReadUInt16();

    protected void WriteStorageAccess(Stream stream) => stream.WriteNoAlloc((ushort) this.m_accessGridLod);

    protected void LoadAccess(Stream stream, MyCellCoord coord)
    {
      if (coord.Lod != this.m_streamGridLod && coord.Lod != this.m_accessGridLod)
        return;
      MyTimeSpan myTimeSpan1 = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
      MyTimeSpan myTimeSpan2 = myTimeSpan1;
      if (coord.Lod == this.m_streamGridLod)
      {
        long num = (long) stream.ReadUInt16();
        myTimeSpan2 = MyTimeSpan.FromSeconds(myTimeSpan1.Seconds - (double) (num * 60L));
      }
      if (coord.Lod != this.m_accessGridLod)
        return;
      this.m_access[coord.CoordInLod] = myTimeSpan2;
    }

    protected void SaveAccess(Stream stream, MyCellCoord coord)
    {
      if (coord.Lod != this.m_accessGridLod)
        return;
      MyTimeSpan myTimeSpan1 = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
      MyTimeSpan myTimeSpan2;
      if (!this.m_access.TryGetValue(coord.CoordInLod, out myTimeSpan2))
        myTimeSpan2 = myTimeSpan1;
      long num = (long) ((myTimeSpan1 - myTimeSpan2).Seconds / 60.0);
      if (num > (long) ushort.MaxValue)
        num = (long) ushort.MaxValue;
      stream.WriteNoAlloc((ushort) num);
    }

    private void DebugDrawAccess(ref MatrixD worldMatrix)
    {
      Color green = Color.Green;
      green.A = (byte) 64;
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, green))
      {
        foreach (KeyValuePair<Vector3I, MyTimeSpan> keyValuePair in this.m_access)
        {
          Vector3I key = keyValuePair.Key;
          BoundingBoxD bb;
          this.ConvertAccessCoordinates(ref key, out bb);
          debugDrawBatchAabb.Add(ref bb);
        }
      }
    }

    public void DebugDrawDelete(Vector3I coord, MyVoxelBase voxelCurrentBase)
    {
      BoundingBoxD bb;
      this.ConvertAccessCoordinates(ref coord, out bb);
      bb.TransformSlow(voxelCurrentBase.WorldMatrix);
      MyRenderProxy.DebugDrawAABB(bb, Color.Red, 0.3f, persistent: true);
    }

    private static MyVoxelOperationsSessionComponent OperationsComponent => MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();

    public bool CachedWrites
    {
      get => this.m_cachedWrites && MyVoxelOperationsSessionComponent.EnableCache;
      set => this.m_cachedWrites = value;
    }

    public bool HasPendingWrites => this.m_pendingChunksToWrite.Count > 0;

    public bool HasCachedChunks => this.m_chunksbyAge.Count - this.m_pendingChunksToWrite.Count > 0;

    public int CachedChunksCount => this.m_cachedChunks.Count;

    public int PendingCachedChunksCount => this.m_pendingChunksToWrite.Count;

    public void InitWriteCache(int prealloc = 128)
    {
      if (this.m_cachedChunks != null || MyStorageBase.OperationsComponent == null)
        return;
      this.CachedWrites = true;
      this.m_cachedChunks = new MyConcurrentDictionary<Vector3I, MyStorageBase.VoxelChunk>(prealloc, (IEqualityComparer<Vector3I>) Vector3I.Comparer);
      this.m_pendingChunksToWrite = new MyConcurrentQueue<Vector3I>(prealloc / 10);
      this.m_chunksbyAge = new MyQueue<Vector3I>(prealloc);
      this.m_cacheMap = new MyDynamicAABBTree(Vector3.Zero);
      this.m_cacheLock = new FastResourceLock();
      if (!MyVoxelOperationsSessionComponent.EnableCache)
        return;
      MyStorageBase.OperationsComponent.Add(this);
    }

    private void DeleteChunk(ref Vector3I coord, MyStorageDataTypeFlags dataToDelete)
    {
      MyStorageBase.VoxelChunk voxelChunk;
      if (!this.m_cachedChunks.TryGetValue(coord, out voxelChunk))
        return;
      if ((dataToDelete & voxelChunk.Cached) == voxelChunk.Cached)
      {
        this.m_cachedChunks.Remove(coord);
        this.m_cacheMap.RemoveProxy(voxelChunk.TreeProxy);
      }
      else
      {
        voxelChunk.Cached &= ~dataToDelete;
        voxelChunk.Dirty &= ~dataToDelete;
      }
    }

    private void WriteChunk(MyStorageBase.VoxelChunk chunk)
    {
      using (this.StorageLock.AcquireExclusiveUsing())
      {
        using (chunk.Lock.AcquireExclusiveUsing())
        {
          if (chunk.Dirty == MyStorageDataTypeFlags.None)
            return;
          Vector3I voxelRangeMin = chunk.Coords << 3;
          Vector3I voxelRangeMax = (chunk.Coords + 1 << 3) - 1;
          MyStorageDataWriteOperator source = new MyStorageDataWriteOperator(chunk.MakeData());
          this.WriteRangeInternal<MyStorageDataWriteOperator>(ref source, chunk.Dirty, ref voxelRangeMin, ref voxelRangeMax);
          chunk.Dirty = MyStorageDataTypeFlags.None;
        }
      }
    }

    private void GetChunk(
      ref Vector3I coord,
      out MyStorageBase.VoxelChunk chunk,
      MyStorageDataTypeFlags required)
    {
      using (this.m_cacheLock.AcquireExclusiveUsing())
      {
        if (!this.m_cachedChunks.TryGetValue(coord, out chunk))
        {
          chunk = new MyStorageBase.VoxelChunk(coord);
          Vector3I vector3I1 = coord << 3;
          Vector3I vector3I2 = (coord + 1 << 3) - 1;
          if (required != MyStorageDataTypeFlags.None)
          {
            using (this.StorageLock.AcquireSharedUsing())
              this.ReadDatForChunk(chunk, required);
          }
          this.m_chunksbyAge.Enqueue(coord);
          this.m_cachedChunks.Add(coord, chunk);
          BoundingBox aabb = new BoundingBox((Vector3) vector3I1, (Vector3) vector3I2);
          chunk.TreeProxy = this.m_cacheMap.AddProxy(ref aabb, (object) chunk, 0U);
        }
        else
        {
          if ((chunk.Cached & required) == required)
            return;
          using (this.StorageLock.AcquireSharedUsing())
            this.ReadDatForChunk(chunk, required & (~chunk.Cached & MyStorageDataTypeFlags.ContentAndMaterial));
        }
      }
    }

    private void ReadDatForChunk(MyStorageBase.VoxelChunk chunk, MyStorageDataTypeFlags data)
    {
      using (chunk.Lock.AcquireExclusiveUsing())
      {
        Vector3I lodVoxelRangeMin = chunk.Coords << 3;
        Vector3I lodVoxelRangeMax = (chunk.Coords + 1 << 3) - 1;
        MyStorageData target = chunk.MakeData();
        MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
        this.ReadRangeInternal(target, ref Vector3I.Zero, data, 0, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
        chunk.Cached |= data;
        chunk.MaxLod = (byte) 0;
      }
    }

    private void DequeueDirtyChunk(out MyStorageBase.VoxelChunk chunk, out Vector3I coords)
    {
      coords = this.m_pendingChunksToWrite.Dequeue();
      this.m_cachedChunks.TryGetValue(coords, out chunk);
    }

    public void CleanCachedChunks()
    {
      int num = 0;
      int count = this.m_chunksbyAge.Count;
      for (int index = 0; index < count && num < 10; ++index)
      {
        Vector3I key;
        MyStorageBase.VoxelChunk voxelChunk;
        bool flag;
        using (this.m_cacheLock.AcquireExclusiveUsing())
        {
          key = this.m_chunksbyAge.Dequeue();
          flag = this.m_cachedChunks.TryGetValue(key, out voxelChunk);
        }
        if (flag)
        {
          if (voxelChunk.Dirty == MyStorageDataTypeFlags.None && voxelChunk.HitCount <= 100)
          {
            using (voxelChunk.Lock.AcquireSharedUsing())
            {
              if (voxelChunk.Dirty == MyStorageDataTypeFlags.None && voxelChunk.HitCount <= 100)
              {
                using (this.m_cacheLock.AcquireExclusiveUsing())
                {
                  this.m_cachedChunks.Remove(key);
                  this.m_cacheMap.RemoveProxy(voxelChunk.TreeProxy);
                }
              }
              else
              {
                using (this.m_cacheLock.AcquireExclusiveUsing())
                {
                  this.m_chunksbyAge.Enqueue(key);
                  voxelChunk.HitCount = 0;
                }
              }
            }
          }
          else
          {
            using (this.m_cacheLock.AcquireExclusiveUsing())
            {
              this.m_chunksbyAge.Enqueue(key);
              voxelChunk.HitCount = 0;
            }
          }
        }
      }
    }

    public bool WritePending(bool force = false)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      using (this.m_cacheLock.AcquireSharedUsing())
      {
        while (stopwatch.ElapsedMilliseconds < 6L | force)
        {
          if (this.m_pendingChunksToWrite.Count > 0)
          {
            MyStorageBase.VoxelChunk chunk;
            this.DequeueDirtyChunk(out chunk, out Vector3I _);
            if (chunk != null && chunk.Dirty != MyStorageDataTypeFlags.None)
              this.WriteChunk(chunk);
          }
          else
            break;
        }
      }
      return this.m_pendingChunksToWrite.Count == 0;
    }

    public void GetStats(out MyStorageBase.WriteCacheStats stats)
    {
      stats.CachedChunks = this.m_cachedChunks.Count;
      stats.QueuedWrites = this.m_pendingChunksToWrite.Count;
      stats.Chunks = (IEnumerable<KeyValuePair<Vector3I, MyStorageBase.VoxelChunk>>) this.m_cachedChunks;
    }

    private bool OverlapsAnyCachedCell(Vector3I voxelRangeMin, Vector3I voxelRangeMax)
    {
      BoundingBox bbox = new BoundingBox((Vector3) voxelRangeMin, (Vector3) voxelRangeMax);
      using (this.m_cacheLock.AcquireSharedUsing())
        return this.m_cacheMap.OverlapsAnyLeafBoundingBox(ref bbox);
    }

    protected void FlushCache(ref BoundingBoxI box, int lodIndex)
    {
      if (!this.CachedWrites || this.m_cacheMap.GetLeafCount() == 0)
        return;
      if (MyStorageBase.m_tmpChunks == null)
        MyStorageBase.m_tmpChunks = new List<MyStorageBase.VoxelChunk>();
      BoundingBox bbox = new BoundingBox((Vector3) (box.Min << lodIndex), (Vector3) (box.Max << lodIndex));
      using (this.m_cacheLock.AcquireSharedUsing())
        this.m_cacheMap.OverlapAllBoundingBox<MyStorageBase.VoxelChunk>(ref bbox, MyStorageBase.m_tmpChunks, clear: false);
      foreach (MyStorageBase.VoxelChunk tmpChunk in MyStorageBase.m_tmpChunks)
      {
        if (tmpChunk.Dirty != MyStorageDataTypeFlags.None)
          this.WriteChunk(tmpChunk);
      }
      MyStorageBase.m_tmpChunks.Clear();
    }

    public static MyStorageBase CreateAsteroidStorage(string asteroid)
    {
      if (string.IsNullOrEmpty(asteroid))
      {
        MyLog.Default.Error("Error: asteroid should not be null!");
        return (MyStorageBase) null;
      }
      MyVoxelMapStorageDefinition definition;
      return MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(asteroid, out definition) ? MyStorageBase.LoadFromFile(Path.Combine(definition.Context.IsBaseGame ? MyFileSystem.ContentPath : definition.Context.ModPath, definition.StorageFile)) : (MyStorageBase) null;
    }

    public abstract IMyStorageDataProvider DataProvider { get; set; }

    static MyStorageBase() => MyVRage.RegisterExitCallback(new Action(MyStorageBase.ResetCache));

    public bool DeleteSupported => this.DataProvider != null;

    public bool Shared { get; internal set; }

    public uint StorageId { get; private set; }

    public MyVoxelGeometry Geometry => this.m_geometry;

    public Vector3I Size { get; protected set; }

    public bool AreDataCached
    {
      get
      {
        lock (this.m_compressedDataLock)
          return this.m_cachedData != null;
      }
    }

    public bool AreDataCachedCompressed => this.m_cachedDataCompressed;

    public event Action<Vector3I, Vector3I, MyStorageDataTypeFlags> RangeChanged;

    protected MyStorageBase() => this.StorageId = (uint) Interlocked.Increment(ref MyStorageBase.m_runningStorageId);

    public void NotifyChanged(
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      MyStorageDataTypeFlags changedData)
    {
      this.OnRangeChanged(voxelRangeMin, voxelRangeMax, changedData);
    }

    protected void OnRangeChanged(
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      MyStorageDataTypeFlags changedData)
    {
      if (this.Closed)
        return;
      this.ResetDataCache();
      if (this.RangeChanged == null)
        return;
      this.ClampVoxelCoord(ref voxelRangeMin);
      this.ClampVoxelCoord(ref voxelRangeMax);
      this.RangeChanged.InvokeIfNotNull<Vector3I, Vector3I, MyStorageDataTypeFlags>(voxelRangeMin, voxelRangeMax, changedData);
    }

    public void ResetDataCache()
    {
      lock (this.m_compressedDataLock)
      {
        this.m_setDataCacheAllowed = false;
        this.m_cachedData = (byte[]) null;
      }
    }

    public void SetDataCache(byte[] data, bool compressed)
    {
      lock (this.m_compressedDataLock)
      {
        if (!this.m_setDataCacheAllowed)
          return;
        this.m_setDataCacheAllowed = false;
        this.m_cachedData = data;
        this.m_cachedDataCompressed = compressed;
      }
    }

    private unsafe void ChangeMaterials(Dictionary<byte, byte> map)
    {
      int num1 = 0;
      if ((this.Size + 1).Size > 4194304)
      {
        MyLog.Default.Error("Cannot overwrite materials for a storage 4 MB or larger.");
      }
      else
      {
        Vector3I zero = Vector3I.Zero;
        Vector3I vector3I = this.Size - 1;
        MyStorageData myStorageData = new MyStorageData();
        myStorageData.Resize(this.Size);
        this.ReadRange(myStorageData, MyStorageDataTypeFlags.Material, 0, zero, vector3I);
        int sizeLinear = myStorageData.SizeLinear;
        fixed (byte* numPtr = myStorageData[MyStorageDataTypeEnum.Material])
        {
          for (int index = 0; index < sizeLinear; ++index)
          {
            byte num2;
            if (map.TryGetValue(numPtr[index], out num2))
            {
              numPtr[index] = num2;
              ++num1;
            }
          }
        }
        if (num1 <= 0)
          return;
        this.WriteRange(myStorageData, MyStorageDataTypeFlags.Material, zero, vector3I, true, false);
      }
    }

    public static MyStorageBase LoadFromFile(
      string absoluteFilePath,
      Dictionary<byte, byte> modifiers = null,
      bool cache = true)
    {
      if (absoluteFilePath.Contains(".vox") && absoluteFilePath.Contains(".vx2"))
      {
        int startIndex = absoluteFilePath.LastIndexOf(".vx2");
        if (startIndex != -1)
        {
          absoluteFilePath = absoluteFilePath.Remove(startIndex);
          absoluteFilePath = Path.ChangeExtension(absoluteFilePath, "vx2");
        }
      }
      MyStorageBase.MyVoxelObjectDefinition definition = new MyStorageBase.MyVoxelObjectDefinition(absoluteFilePath, modifiers);
      int hashCode = definition.GetHashCode();
      if (!cache || !MyStorageBase.UseStorageCache)
        return PerformLoad();
      Lazy<MyStorageBase> lazy;
      lock (MyStorageBase.m_storageCache)
      {
        lazy = MyStorageBase.m_storageCache.Read(hashCode);
        if (lazy == null)
        {
          lazy = new Lazy<MyStorageBase>(new Func<MyStorageBase>(PerformLoad));
          MyStorageBase.m_storageCache.Write(hashCode, lazy);
        }
      }
      MyStorageBase myStorageBase = lazy.Value;
      if (myStorageBase != null)
        myStorageBase.Shared = true;
      return myStorageBase;

      MyStorageBase PerformLoad()
      {
        if (!MyFileSystem.FileExists(absoluteFilePath))
        {
          string str = Path.ChangeExtension(absoluteFilePath, "vox");
          MySandboxGame.Log.WriteLine(string.Format("Loading voxel storage from file '{0}'", (object) str));
          if (!MyFileSystem.FileExists(str))
          {
            int startIndex = absoluteFilePath.LastIndexOf(".vx2");
            if (startIndex == -1)
              return (MyStorageBase) null;
            str = absoluteFilePath.Remove(startIndex);
            if (!MyFileSystem.FileExists(str))
              return (MyStorageBase) null;
          }
          MyStorageBase.UpdateFileFormat(str);
        }
        else
          MySandboxGame.Log.WriteLine(string.Format("Loading voxel storage from file '{0}'", (object) absoluteFilePath));
        byte[] numArray = (byte[]) null;
        using (Stream stream = MyFileSystem.OpenRead(absoluteFilePath))
        {
          numArray = new byte[stream.Length];
          stream.Read(numArray, 0, numArray.Length);
        }
        MyStorageBase myStorageBase = MyStorageBase.Load(numArray, out bool _);
        if (definition.Changes != null)
          myStorageBase.ChangeMaterials(definition.Changes);
        return myStorageBase;
      }
    }

    public static void ResetCache()
    {
      lock (MyStorageBase.m_storageCache)
      {
        foreach (KeyValuePair<int, Lazy<MyStorageBase>> pair in MyStorageBase.m_storageCache)
        {
          Lazy<MyStorageBase> v;
          pair.Deconstruct<int, Lazy<MyStorageBase>>(out int _, out v);
          Lazy<MyStorageBase> lazy = v;
          if (lazy != null && lazy.IsValueCreated)
            lazy.Value?.Close();
        }
        MyStorageBase.m_storageCache.Reset();
      }
    }

    public static MyStorageBase Load(string name, bool cache = true, bool local = false)
    {
      if (local || MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
        return MyStorageBase.LoadFromFile(Path.IsPathRooted(name) ? name : Path.Combine(MySession.Static.CurrentPath, name + ".vx2"), cache: cache);
      bool isOldFormat = false;
      return MyStorageBase.Load(MyMultiplayer.Static.VoxelMapData.Read(name) ?? throw new Exception(string.Format("Missing voxel map data! : {0}", (object) name)), out isOldFormat);
    }

    public static MyStorageBase Load(byte[] memoryBuffer, out bool isOldFormat)
    {
      bool compressed = false;
      MyStorageBase storage;
      using (MemoryStream stream1 = new MemoryStream(memoryBuffer, false))
      {
        using (Stream stream2 = stream1.UnwrapGZip())
        {
          compressed = stream2 != stream1;
          using (BufferedStream bufferedStream = new BufferedStream(stream2, 32768))
            MyStorageBase.Load((Stream) bufferedStream, out storage, out isOldFormat);
        }
      }
      if (!isOldFormat)
      {
        storage.SetDataCache(memoryBuffer, compressed);
      }
      else
      {
        MySandboxGame.Log.WriteLine("Voxel storage was in old format. It is updated but needs to be saved.");
        storage.ResetDataCache();
      }
      return storage;
    }

    private static void Load(Stream stream, out MyStorageBase storage, out bool isOldFormat)
    {
      try
      {
        isOldFormat = false;
        string str = stream.ReadString();
        int fileVersion = stream.Read7BitEncodedInt();
        if (str == "Cell")
        {
          MyStorageBaseCompatibility baseCompatibility = new MyStorageBaseCompatibility();
          storage = baseCompatibility.Compatibility_LoadCellStorage(fileVersion, stream);
          isOldFormat = true;
        }
        else
        {
          if (!(str == "Octree"))
            throw new InvalidBranchException();
          storage = (MyStorageBase) new MyOctreeStorage();
          storage.LoadInternal(fileVersion, stream, ref isOldFormat);
          storage.m_geometry.Init(storage);
          storage.m_setDataCacheAllowed = true;
        }
      }
      finally
      {
      }
    }

    public void Save(out byte[] outCompressedData)
    {
      if (this.CachedWrites)
        this.WritePending(true);
      try
      {
        using (this.StorageLock.AcquireSharedUsing())
        {
          lock (this.m_compressedDataLock)
          {
            if (this.m_cachedData == null)
            {
              this.m_cachedData = this.GetData(true);
              this.m_cachedDataCompressed = true;
            }
            outCompressedData = this.m_cachedData;
          }
        }
      }
      finally
      {
      }
    }

    private byte[] GetData(bool compressed)
    {
      MemoryStream memoryStream;
      using (memoryStream = new MemoryStream(16384))
      {
        Stream stream1 = !compressed ? (Stream) memoryStream : (Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress);
        using (BufferedStream stream2 = new BufferedStream(stream1, 16384))
        {
          if (!(this.GetType() == typeof (MyOctreeStorage)))
            throw new InvalidBranchException();
          string text = "Octree";
          int num = 2;
          stream2.WriteNoAlloc(text);
          stream2.Write7BitEncodedInt(num);
          this.SaveInternal((Stream) stream2);
        }
        if (compressed)
          stream1.Dispose();
      }
      return memoryStream.ToArray();
    }

    public byte[] GetVoxelData()
    {
      if (this.CachedWrites)
        this.WritePending(true);
      byte[] numArray = (byte[]) null;
      try
      {
        using (this.StorageLock.AcquireSharedUsing())
        {
          numArray = this.GetData(false);
          this.m_setDataCacheAllowed = true;
        }
      }
      finally
      {
      }
      return numArray;
    }

    public void Reset(MyStorageDataTypeFlags dataToReset)
    {
      using (this.StorageLock.AcquireExclusiveUsing())
      {
        this.ResetDataCache();
        this.ResetInternal(dataToReset);
      }
      if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (this.Closed)
            return;
          this.OnRangeChanged(Vector3I.Zero, this.Size - 1, dataToReset);
        }), "MyStorageBase.Reset: RangeChanged");
      else
        this.OnRangeChanged(Vector3I.Zero, this.Size - 1, dataToReset);
    }

    public void OverwriteAllMaterials(MyVoxelMaterialDefinition material)
    {
      using (this.StorageLock.AcquireExclusiveUsing())
      {
        this.ResetDataCache();
        this.OverwriteAllMaterialsInternal(material);
      }
      this.OnRangeChanged(Vector3I.Zero, this.Size - 1, MyStorageDataTypeFlags.Material);
    }

    public void ExecuteOperationFast<TVoxelOperator>(
      ref TVoxelOperator voxelOperator,
      MyStorageDataTypeFlags dataToWrite,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      bool notifyRangeChanged,
      bool skipCache)
      where TVoxelOperator : struct, IVoxelOperator
    {
      int num1 = notifyRangeChanged ? 1 : 0;
      try
      {
        this.ResetDataCache();
        if (this.CachedWrites && !skipCache && (this.m_pendingChunksToWrite.Count < 1024 || this.OverlapsAnyCachedCell(voxelRangeMin, voxelRangeMax)))
        {
          int num2 = 3;
          Vector3I vector3I1 = voxelRangeMin >> num2;
          Vector3I vector3I2 = voxelRangeMax >> num2;
          Vector3I zero = Vector3I.Zero;
          for (zero.Z = vector3I1.Z; zero.Z <= vector3I2.Z; ++zero.Z)
          {
            for (zero.Y = vector3I1.Y; zero.Y <= vector3I2.Y; ++zero.Y)
            {
              for (zero.X = vector3I1.X; zero.X <= vector3I2.X; ++zero.X)
              {
                Vector3I vector3I3 = zero << num2;
                Vector3I vector3I4 = Vector3I.Max(zero << num2, voxelRangeMin);
                Vector3I vector3I5 = Vector3I.Min((zero + 1 << num2) - 1, voxelRangeMax);
                Vector3I targetOffset = vector3I4 - voxelRangeMin;
                MyStorageDataTypeFlags required = dataToWrite;
                if ((vector3I5 - vector3I4 + 1).Size == 512 && voxelOperator.Flags == VoxelOperatorFlags.WriteAll)
                  required = MyStorageDataTypeFlags.None;
                MyStorageBase.VoxelChunk chunk;
                this.GetChunk(ref zero, out chunk, required);
                Vector3I min = vector3I4 - vector3I3;
                Vector3I max = vector3I5 - vector3I3;
                using (chunk.Lock.AcquireExclusiveUsing())
                {
                  int num3 = (uint) chunk.Dirty > 0U ? 1 : 0;
                  chunk.ExecuteOperator<TVoxelOperator>(ref voxelOperator, dataToWrite, ref targetOffset, ref min, ref max);
                  if (num3 == 0)
                    this.m_pendingChunksToWrite.Enqueue(zero);
                }
              }
            }
          }
        }
        else
        {
          if (skipCache)
          {
            BoundingBoxI box = new BoundingBoxI(voxelRangeMin, voxelRangeMax);
            this.FlushCache(ref box, 0);
          }
          using (this.StorageLock.AcquireExclusiveUsing())
            this.WriteRangeInternal<TVoxelOperator>(ref voxelOperator, dataToWrite, ref voxelRangeMin, ref voxelRangeMax);
        }
      }
      finally
      {
        if (notifyRangeChanged)
          this.OnRangeChanged(voxelRangeMin, voxelRangeMax, dataToWrite);
      }
    }

    public void WriteRange(
      MyStorageData source,
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notifyRangeChanged = true,
      bool skipCache = false)
    {
      MyStorageDataWriteOperator voxelOperator = new MyStorageDataWriteOperator(source);
      this.ExecuteOperationFast<MyStorageDataWriteOperator>(ref voxelOperator, dataToWrite, ref voxelRangeMin, ref voxelRangeMax, notifyRangeChanged, skipCache);
    }

    public void Sweep(MyStorageDataTypeFlags dataToSweep)
    {
      try
      {
        this.ResetDataCache();
        if (this.CachedWrites)
        {
          using (this.m_cacheLock.AcquireExclusiveUsing())
          {
            while (this.m_cachedChunks.Count > 0)
            {
              Vector3I key = this.m_cachedChunks.FirstPair().Key;
              this.DeleteChunk(ref key, dataToSweep);
            }
          }
        }
        using (this.StorageLock.AcquireExclusiveUsing())
          this.SweepInternal(dataToSweep);
      }
      finally
      {
        this.OnRangeChanged(Vector3I.Zero, this.Size, dataToSweep);
      }
    }

    private void DeleteRange(
      MyStorageDataTypeFlags dataToDelete,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify)
    {
      int num1 = notify ? 1 : 0;
      try
      {
        this.ResetDataCache();
        if (this.CachedWrites && this.OverlapsAnyCachedCell(voxelRangeMin, voxelRangeMax))
        {
          using (this.m_cacheLock.AcquireExclusiveUsing())
          {
            int num2 = 3;
            Vector3I vector3I1 = voxelRangeMin >> num2;
            Vector3I vector3I2 = voxelRangeMax >> num2;
            Vector3I zero = Vector3I.Zero;
            for (zero.Z = vector3I1.Z; zero.Z <= vector3I2.Z; ++zero.Z)
            {
              for (zero.Y = vector3I1.Y; zero.Y <= vector3I2.Y; ++zero.Y)
              {
                for (zero.X = vector3I1.X; zero.X <= vector3I2.X; ++zero.X)
                  this.DeleteChunk(ref zero, dataToDelete);
              }
            }
          }
        }
        using (this.StorageLock.AcquireExclusiveUsing())
          this.DeleteRangeInternal(dataToDelete, ref voxelRangeMin, ref voxelRangeMax);
      }
      finally
      {
        if (notify)
          this.OnRangeChanged(voxelRangeMin, voxelRangeMax, dataToDelete);
      }
    }

    public bool IsRangeModified(ref BoundingBoxI box)
    {
      if (this.DataProvider == null)
        return true;
      using (this.StorageLock.AcquireSharedUsing())
        return this.IsModifiedInternal(ref box);
    }

    public void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax)
    {
      MyVoxelRequestFlags requestFlags = dataToRead == MyStorageDataTypeFlags.ContentAndMaterial ? MyVoxelRequestFlags.ConsiderContent : (MyVoxelRequestFlags) 0;
      this.ReadRange(target, dataToRead, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
    }

    private void ReadRangeAdviseCache(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax)
    {
      if (this.m_pendingChunksToWrite.Count > 1024)
      {
        this.ReadRange(target, dataToRead, 0, lodVoxelRangeMin, lodVoxelRangeMax);
      }
      else
      {
        if (!this.CachedWrites)
          return;
        int num = 3;
        Vector3I vector3I1 = lodVoxelRangeMin >> num;
        Vector3I vector3I2 = lodVoxelRangeMax >> num;
        Vector3I zero = Vector3I.Zero;
        for (zero.Z = vector3I1.Z; zero.Z <= vector3I2.Z; ++zero.Z)
        {
          for (zero.Y = vector3I1.Y; zero.Y <= vector3I2.Y; ++zero.Y)
          {
            for (zero.X = vector3I1.X; zero.X <= vector3I2.X; ++zero.X)
            {
              Vector3I vector3I3 = zero << num;
              Vector3I vector3I4 = Vector3I.Max(zero << num, lodVoxelRangeMin);
              Vector3I targetOffset = vector3I4 - lodVoxelRangeMin;
              Vector3I vector3I5 = Vector3I.Min((zero + 1 << num) - 1, lodVoxelRangeMax);
              MyStorageBase.VoxelChunk chunk;
              this.GetChunk(ref zero, out chunk, dataToRead);
              Vector3I min = vector3I4 - vector3I3;
              Vector3I max = vector3I5 - vector3I3;
              using (chunk.Lock.AcquireSharedUsing())
                chunk.ReadLod(target, dataToRead, ref targetOffset, 0, ref min, ref max);
            }
          }
        }
      }
    }

    public void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax,
      ref MyVoxelRequestFlags requestFlags)
    {
      if ((dataToRead & MyStorageDataTypeFlags.Material) != MyStorageDataTypeFlags.None && (requestFlags & MyVoxelRequestFlags.SurfaceMaterial) == (MyVoxelRequestFlags) 0)
      {
        target.ClearMaterials(byte.MaxValue);
        requestFlags |= MyVoxelRequestFlags.EmptyData;
        if ((dataToRead & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
          requestFlags |= MyVoxelRequestFlags.ConsiderContent;
      }
      if ((dataToRead & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
      {
        target.ClearContent((byte) 0);
        requestFlags |= MyVoxelRequestFlags.EmptyData;
      }
      if (requestFlags.HasFlags(MyVoxelRequestFlags.AdviseCache) && lodIndex == 0 && this.CachedWrites)
      {
        this.ReadRangeAdviseCache(target, dataToRead, lodVoxelRangeMin, lodVoxelRangeMax);
      }
      else
      {
        if (this.CachedWrites && lodIndex <= 3 && this.m_cachedChunks.Count > 0)
        {
          if (MyStorageBase.m_tmpChunks == null)
            MyStorageBase.m_tmpChunks = new List<MyStorageBase.VoxelChunk>();
          int num = 3 - lodIndex;
          BoundingBox bbox = new BoundingBox((Vector3) (lodVoxelRangeMin << lodIndex), (Vector3) (lodVoxelRangeMax << lodIndex));
          using (this.m_cacheLock.AcquireSharedUsing())
            this.m_cacheMap.OverlapAllBoundingBox<MyStorageBase.VoxelChunk>(ref bbox, MyStorageBase.m_tmpChunks, clear: false);
          if (MyStorageBase.m_tmpChunks.Count > 0)
          {
            Vector3I vector3I1 = lodVoxelRangeMin >> num;
            Vector3I vector3I2 = lodVoxelRangeMax >> num;
            bool flag = false;
            Vector3I vector3I3 = vector3I1;
            if ((vector3I2 - vector3I3 + 1).Size > MyStorageBase.m_tmpChunks.Count)
            {
              using (this.StorageLock.AcquireSharedUsing())
                this.ReadRangeInternal(target, ref Vector3I.Zero, dataToRead, lodIndex, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
              requestFlags &= ~(MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent | MyVoxelRequestFlags.OneMaterial);
              flag = true;
            }
            for (int index = 0; index < MyStorageBase.m_tmpChunks.Count; ++index)
            {
              MyStorageBase.VoxelChunk tmpChunk = MyStorageBase.m_tmpChunks[index];
              Vector3I coords = tmpChunk.Coords;
              Vector3I vector3I4 = coords << num;
              Vector3I vector3I5 = Vector3I.Max(coords << num, lodVoxelRangeMin);
              Vector3I targetOffset = vector3I5 - lodVoxelRangeMin;
              Vector3I vector3I6 = Vector3I.Min((coords + 1 << num) - 1, lodVoxelRangeMax);
              Vector3I min = vector3I5 - vector3I4;
              Vector3I max = vector3I6 - vector3I4;
              if ((tmpChunk.Cached & dataToRead) != dataToRead && !flag)
              {
                using (this.StorageLock.AcquireSharedUsing())
                {
                  if ((tmpChunk.Cached & dataToRead) != dataToRead)
                    this.ReadDatForChunk(tmpChunk, dataToRead);
                }
              }
              using (tmpChunk.Lock.AcquireSharedUsing())
                tmpChunk.ReadLod(target, !flag ? dataToRead : dataToRead & tmpChunk.Cached, ref targetOffset, lodIndex, ref min, ref max);
            }
            MyStorageBase.m_tmpChunks.Clear();
            return;
          }
        }
        using (this.StorageLock.AcquireSharedUsing())
          this.ReadRangeInternal(target, ref Vector3I.Zero, dataToRead, lodIndex, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
      }
    }

    public abstract ContainmentType Intersect(ref BoundingBox box, bool lazy);

    public abstract bool IntersectInternal(ref LineD line);

    public void PinAndExecute(Action action)
    {
      using (StoragePin storagePin = this.Pin())
      {
        if (!storagePin.Valid)
          return;
        action.InvokeIfNotNull();
      }
    }

    public void PinAndExecute(Action<VRage.ModAPI.IMyStorage> action)
    {
      using (StoragePin storagePin = this.Pin())
      {
        if (!storagePin.Valid)
          return;
        action.InvokeIfNotNull<VRage.ModAPI.IMyStorage>((VRage.ModAPI.IMyStorage) this);
      }
    }

    public ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true)
    {
      this.FlushCache(ref box, lod);
      using (this.StorageLock.AcquireSharedUsing())
        return this.Closed ? ContainmentType.Disjoint : this.IntersectInternal(ref box, lod, exhaustiveContainmentCheck);
    }

    public bool Intersect(ref LineD line)
    {
      using (this.StorageLock.AcquireSharedUsing())
        return !this.Closed && this.IntersectInternal(ref line);
    }

    protected abstract ContainmentType IntersectInternal(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck);

    protected abstract void LoadInternal(int fileVersion, Stream stream, ref bool isOldFormat);

    protected abstract void SaveInternal(Stream stream);

    protected abstract void ResetInternal(MyStorageDataTypeFlags dataToReset);

    protected abstract void OverwriteAllMaterialsInternal(MyVoxelMaterialDefinition material);

    protected abstract void WriteRangeInternal<TOperator>(
      ref TOperator source,
      MyStorageDataTypeFlags dataToWrite,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax)
      where TOperator : struct, IVoxelOperator;

    protected abstract void DeleteRangeInternal(
      MyStorageDataTypeFlags dataToDelete,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax);

    protected abstract void SweepInternal(MyStorageDataTypeFlags dataToSweep);

    protected abstract void ReadRangeInternal(
      MyStorageData target,
      ref Vector3I targetWriteRange,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      ref Vector3I lodVoxelRangeMin,
      ref Vector3I lodVoxelRangeMax,
      ref MyVoxelRequestFlags requestFlags);

    protected abstract bool IsModifiedInternal(ref BoundingBoxI range);

    public virtual void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode)
    {
      if (mode != MyVoxelDebugDrawMode.Content_Access)
        return;
      this.DebugDrawAccess(ref worldMatrix);
    }

    private static void UpdateFileFormat(string originalVoxFile)
    {
      string path = Path.ChangeExtension(originalVoxFile, ".vx2");
      if (!File.Exists(originalVoxFile))
      {
        MySandboxGame.Log.Error("Voxel file '{0}' does not exist!", (object) originalVoxFile);
      }
      else
      {
        if (Path.GetExtension(originalVoxFile) != ".vox")
          MySandboxGame.Log.Warning("Unexpected voxel file extensions in path: '{0}'", (object) originalVoxFile);
        try
        {
          using (MyCompressionFileLoad compressionFileLoad = new MyCompressionFileLoad(originalVoxFile))
          {
            using (Stream stream1 = MyFileSystem.OpenWrite(path))
            {
              using (GZipStream gzipStream = new GZipStream(stream1, CompressionMode.Compress))
              {
                using (BufferedStream stream2 = new BufferedStream((Stream) gzipStream))
                {
                  stream2.WriteNoAlloc("Cell");
                  stream2.Write7BitEncodedInt(compressionFileLoad.GetInt32());
                  byte[] numArray = new byte[16384];
                  for (int bytes = compressionFileLoad.GetBytes(numArray.Length, numArray); bytes != 0; bytes = compressionFileLoad.GetBytes(numArray.Length, numArray))
                    stream2.Write(numArray, 0, bytes);
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.Error("While updating voxel storage '{0}' to new format: {1}", (object) originalVoxFile, (object) ex.Message);
        }
      }
    }

    public virtual VRage.Game.Voxels.IMyStorage Copy() => (VRage.Game.Voxels.IMyStorage) null;

    protected void PerformClose()
    {
      if (Interlocked.CompareExchange(ref this.m_closed, 1, 0) != 0)
        return;
      this.CloseInternal();
    }

    public virtual void CloseInternal()
    {
      using (this.StorageLock.AcquireExclusiveUsing())
      {
        this.RangeChanged = (Action<Vector3I, Vector3I, MyStorageDataTypeFlags>) null;
        this.DataProvider?.Close();
      }
      if (!MyVoxelOperationsSessionComponent.EnableCache)
        return;
      MyStorageBase.OperationsComponent?.Remove(this);
    }

    public bool Closed => (uint) Interlocked.CompareExchange(ref this.m_closed, 0, 0) > 0U;

    public bool MarkedForClose => (uint) (Interlocked.CompareExchange(ref this.m_pinAndCloseMark, 0, 0) & int.MinValue) > 0U;

    public void Close()
    {
      int num;
      for (int comparand = this.m_pinAndCloseMark; (comparand & int.MinValue) == 0; comparand = num)
      {
        num = Interlocked.CompareExchange(ref this.m_pinAndCloseMark, comparand | int.MinValue, comparand);
        if (comparand == num)
        {
          if ((comparand & int.MaxValue) != 0)
            break;
          this.PerformClose();
          break;
        }
      }
    }

    public StoragePin Pin()
    {
      if ((Interlocked.Increment(ref this.m_pinAndCloseMark) & int.MinValue) == 0)
        return new StoragePin((VRage.Game.Voxels.IMyStorage) this);
      if ((Interlocked.Decrement(ref this.m_pinAndCloseMark) & int.MaxValue) == 0)
        this.PerformClose();
      return new StoragePin((VRage.Game.Voxels.IMyStorage) null);
    }

    public void Unpin()
    {
      if (Interlocked.Decrement(ref this.m_pinAndCloseMark) != int.MinValue)
        return;
      this.PerformClose();
    }

    public static string GetStoragePath(string storageName) => Path.Combine(MySession.Static.CurrentPath, storageName + ".vx2");

    Vector3I VRage.ModAPI.IMyStorage.Size => this.Size;

    void VRage.ModAPI.IMyStorage.ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax)
    {
      if ((uint) lodIndex >= 16U)
        return;
      this.ReadRange(target, dataToRead, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
    }

    void VRage.ModAPI.IMyStorage.WriteRange(
      MyStorageData source,
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify,
      bool skipCache)
    {
      this.WriteRange(source, dataToWrite, voxelRangeMin, voxelRangeMax, notify, skipCache);
    }

    void VRage.ModAPI.IMyStorage.DeleteRange(
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify)
    {
      this.DeleteRange(dataToWrite, voxelRangeMin, voxelRangeMax, notify);
    }

    void VRage.ModAPI.IMyStorage.ExecuteOperationFast<TVoxelOperator>(
      ref TVoxelOperator voxelOperator,
      MyStorageDataTypeFlags dataToWrite,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      bool notifyRangeChanged)
    {
      this.ExecuteOperationFast<TVoxelOperator>(ref voxelOperator, dataToWrite, ref voxelRangeMin, ref voxelRangeMax, notifyRangeChanged, false);
    }

    void VRage.ModAPI.IMyStorage.NotifyRangeChanged(
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      MyStorageDataTypeFlags dataChanged)
    {
      this.OnRangeChanged(voxelRangeMin, voxelRangeMax, dataChanged);
    }

    public void OverwriteAllMaterials(byte materialIndex)
    {
    }

    public enum MyAccessType
    {
      Read,
      Write,
      Delete,
    }

    public class VoxelChunk
    {
      public const int SizeBits = 3;
      public const int Size = 8;
      public const int Volume = 512;
      public readonly Vector3I Coords;
      public byte MaxLod;
      public static readonly int TotalVolume = 841;
      public static readonly Vector3I SizeVector = new Vector3I(8);
      public static readonly Vector3I MaxVector = new Vector3I(7);
      public byte[] Material;
      public byte[] Content;
      public MyStorageDataTypeFlags Dirty;
      public MyStorageDataTypeFlags Cached;
      public int HitCount;
      internal int TreeProxy;
      public FastResourceLock Lock = new FastResourceLock();

      public VoxelChunk(Vector3I coords)
      {
        this.Coords = coords;
        this.Material = new byte[MyStorageBase.VoxelChunk.TotalVolume];
        this.Content = new byte[MyStorageBase.VoxelChunk.TotalVolume];
      }

      public void UpdateLodData(int lod)
      {
        for (int lod1 = (int) this.MaxLod + 1; lod1 <= lod; ++lod1)
        {
          MyStorageBase.VoxelChunk.UpdateLodDataInternal(lod1, this.Content, MyOctreeNode.ContentFilter);
          MyStorageBase.VoxelChunk.UpdateLodDataInternal(lod1, this.Material, MyOctreeNode.MaterialFilter);
        }
        this.MaxLod = (byte) lod;
      }

      private static unsafe void UpdateLodDataInternal(
        int lod,
        byte[] dataArray,
        MyOctreeNode.FilterFunction filter)
      {
        int num1 = 0;
        for (int index = 0; index < lod - 1; ++index)
          num1 += 512 >> index + index + index;
        int num2 = 8 >> lod;
        int num3 = num2 * num2;
        int num4 = num3 * num2;
        int num5 = 8 >> lod - 1;
        int num6 = num5 * num5;
        int num7 = num6 * num5;
        ulong num8;
        byte* pData = (byte*) &num8;
        fixed (byte* numPtr1 = dataArray)
        {
          byte* numPtr2 = numPtr1 + num1;
          byte* numPtr3 = numPtr2 + num7;
          for (int index1 = 0; index1 < num4; index1 += num3)
          {
            int num9 = index1 << 3;
            int num10 = (index1 << 3) + num6;
            for (int index2 = 0; index2 < num3; index2 += num2)
            {
              int num11 = index2 << 2;
              int num12 = (index2 << 2) + num5;
              for (int index3 = 0; index3 < num2; ++index3)
              {
                int num13 = index3 << 1;
                int num14 = (index3 << 1) + 1;
                *pData = numPtr2[num9 + num11 + num13];
                pData[1] = numPtr2[num9 + num11 + num14];
                pData[2] = numPtr2[num9 + num12 + num13];
                pData[3] = numPtr2[num9 + num12 + num14];
                pData[4] = numPtr2[num10 + num11 + num13];
                pData[5] = numPtr2[num10 + num11 + num14];
                pData[6] = numPtr2[num10 + num12 + num13];
                pData[7] = numPtr2[num10 + num12 + num14];
                numPtr3[index3 + index2 + index1] = filter(pData, lod);
              }
            }
          }
        }
      }

      public MyStorageData MakeData() => new MyStorageData(MyStorageBase.VoxelChunk.SizeVector, this.Content, this.Material);

      public void ReadLod(
        MyStorageData target,
        MyStorageDataTypeFlags dataTypes,
        ref Vector3I targetOffset,
        int lodIndex,
        ref Vector3I min,
        ref Vector3I max)
      {
        if (lodIndex > (int) this.MaxLod)
          this.UpdateLodData(lodIndex);
        if (dataTypes.Requests(MyStorageDataTypeEnum.Content))
          this.ReadLod(target, MyStorageDataTypeEnum.Content, this.Content, targetOffset, lodIndex, min, max);
        if (dataTypes.Requests(MyStorageDataTypeEnum.Material))
          this.ReadLod(target, MyStorageDataTypeEnum.Material, this.Material, targetOffset, lodIndex, min, max);
        ++this.HitCount;
      }

      private unsafe void ReadLod(
        MyStorageData target,
        MyStorageDataTypeEnum dataType,
        byte[] dataArray,
        Vector3I tofft,
        int lod,
        Vector3I min,
        Vector3I max)
      {
        int num1 = 0;
        for (int index = 0; index < lod; ++index)
          num1 += 512 >> index + index + index;
        int num2 = 8 >> lod;
        int num3 = num2 * num2;
        min.Y *= num2;
        min.Z *= num3;
        max.Y *= num2;
        max.Z *= num3;
        int stepX = target.StepX;
        int stepY = target.StepY;
        int stepZ = target.StepZ;
        tofft.Y *= stepY;
        tofft.Z *= stepZ;
        fixed (byte* numPtr1 = dataArray)
          fixed (byte* numPtr2 = target[dataType])
          {
            byte* numPtr3 = numPtr1 + num1;
            int z1 = min.Z;
            int z2 = tofft.Z;
            while (z1 <= max.Z)
            {
              int y1 = min.Y;
              int y2 = tofft.Y;
              while (y1 <= max.Y)
              {
                int x1 = min.X;
                int x2 = tofft.X;
                while (x1 <= max.X)
                {
                  numPtr2[z2 + y2 + x2] = numPtr3[z1 + y1 + x1];
                  ++x1;
                  x2 += stepX;
                }
                y1 += num2;
                y2 += stepY;
              }
              z1 += num3;
              z2 += stepZ;
            }
          }
      }

      public void ExecuteOperator<TVoxelOperator>(
        ref TVoxelOperator voxelOperator,
        MyStorageDataTypeFlags dataTypes,
        ref Vector3I targetOffset,
        ref Vector3I min,
        ref Vector3I max)
        where TVoxelOperator : IVoxelOperator
      {
        if (dataTypes.Requests(MyStorageDataTypeEnum.Content))
          this.ExecuteOperator<TVoxelOperator>(ref voxelOperator, MyStorageDataTypeEnum.Content, this.Content, targetOffset, min, max);
        if (dataTypes.Requests(MyStorageDataTypeEnum.Material))
          this.ExecuteOperator<TVoxelOperator>(ref voxelOperator, MyStorageDataTypeEnum.Material, this.Material, targetOffset, min, max);
        this.Cached |= dataTypes;
        this.Dirty |= dataTypes;
        this.MaxLod = (byte) 0;
      }

      private void ExecuteOperator<TVoxelOperator>(
        ref TVoxelOperator voxelOperator,
        MyStorageDataTypeEnum dataType,
        byte[] dataArray,
        Vector3I tofft,
        Vector3I min,
        Vector3I max)
        where TVoxelOperator : IVoxelOperator
      {
        int num1 = 8;
        int num2 = num1 * num1;
        min.Y *= num1;
        min.Z *= num2;
        max.Y *= num1;
        max.Z *= num2;
        int z = min.Z;
        Vector3I position;
        for (position.Z = tofft.Z; z <= max.Z; ++position.Z)
        {
          int y = min.Y;
          for (position.Y = tofft.Y; y <= max.Y; ++position.Y)
          {
            int x = min.X;
            for (position.X = tofft.X; x <= max.X; ++position.X)
            {
              voxelOperator.Op(ref position, dataType, ref dataArray[z + y + x]);
              ++x;
            }
            y += num1;
          }
          z += num2;
        }
      }
    }

    public struct WriteCacheStats
    {
      public int QueuedWrites;
      public int CachedChunks;
      public IEnumerable<KeyValuePair<Vector3I, MyStorageBase.VoxelChunk>> Chunks;
    }

    private struct MyVoxelObjectDefinition
    {
      public readonly string FilePath;
      public readonly Dictionary<byte, byte> Changes;

      public MyVoxelObjectDefinition(string filePath, Dictionary<byte, byte> changes)
      {
        this.FilePath = filePath;
        this.Changes = changes;
      }

      public override int GetHashCode()
      {
        int num1 = 17 * 486187739 + this.FilePath.GetHashCode();
        if (this.Changes != null)
        {
          foreach (KeyValuePair<byte, byte> change in this.Changes)
          {
            int num2 = num1 * 486187739;
            byte key = change.Key;
            int hashCode1 = key.GetHashCode();
            num1 = num2 + hashCode1;
            int num3 = num1 * 486187739;
            key = change.Value;
            int hashCode2 = key.GetHashCode();
            num1 = num3 + hashCode2;
          }
        }
        return num1;
      }
    }
  }
}
