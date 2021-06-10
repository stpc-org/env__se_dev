// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyOctreeStorage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Utils;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Plugins;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  public class MyOctreeStorage : MyStorageBase, IMyCompositeShape
  {
    private const int CURRENT_FILE_VERSION = 1;
    public const int LeafLodCount = 4;
    public const int LeafSizeInVoxels = 16;
    private readonly Dictionary<byte, byte> m_oldToNewIndexMap = new Dictionary<byte, byte>();
    [ThreadStatic]
    private static MyStorageData m_temporaryCache;
    private int m_treeHeight;
    private readonly Dictionary<ulong, MyOctreeNode> m_contentNodes = new Dictionary<ulong, MyOctreeNode>();
    private readonly Dictionary<ulong, IMyOctreeLeafNode> m_contentLeaves = new Dictionary<ulong, IMyOctreeLeafNode>();
    private readonly Dictionary<ulong, MyOctreeNode> m_materialNodes = new Dictionary<ulong, MyOctreeNode>();
    private readonly Dictionary<ulong, IMyOctreeLeafNode> m_materialLeaves = new Dictionary<ulong, IMyOctreeLeafNode>();
    private IMyStorageDataProvider m_dataProvider;
    [ThreadStatic]
    private static MyStorageData m_storageCached;
    private static readonly Dictionary<int, MyStorageDataProviderAttribute> m_attributesById = new Dictionary<int, MyStorageDataProviderAttribute>();
    private static readonly Dictionary<Type, MyStorageDataProviderAttribute> m_attributesByType = new Dictionary<Type, MyStorageDataProviderAttribute>();
    private const int VERSION_OCTREE_NODES_32BIT_KEY = 1;
    private const int CURRENT_VERSION_OCTREE_NODES = 2;
    private const int VERSION_OCTREE_LEAVES_32BIT_KEY = 2;
    private const int CURRENT_VERSION_OCTREE_LEAVES = 3;

    private static MyStorageData TempStorage => MyOctreeStorage.m_temporaryCache ?? (MyOctreeStorage.m_temporaryCache = new MyStorageData());

    public override IMyStorageDataProvider DataProvider
    {
      get => this.m_dataProvider;
      set
      {
        this.m_dataProvider = value;
        foreach (IMyOctreeLeafNode myOctreeLeafNode in this.m_contentLeaves.Values)
          myOctreeLeafNode.OnDataProviderChanged(value);
        foreach (IMyOctreeLeafNode myOctreeLeafNode in this.m_materialLeaves.Values)
          myOctreeLeafNode.OnDataProviderChanged(value);
        this.OnRangeChanged(Vector3I.Zero, this.Size - 1, MyStorageDataTypeFlags.ContentAndMaterial);
      }
    }

    public MyOctreeStorage()
    {
    }

    public MyOctreeStorage(IMyStorageDataProvider dataProvider, Vector3I size)
    {
      this.Size = new Vector3I(MathHelper.GetNearestBiggerPowerOfTwo(MathHelper.Max(size.X, size.Y, size.Z)));
      this.m_dataProvider = dataProvider;
      this.InitTreeHeight();
      this.ResetInternal(MyStorageDataTypeFlags.ContentAndMaterial);
      this.Geometry.Init((MyStorageBase) this);
    }

    private void InitTreeHeight()
    {
      Vector3I vector3I1 = this.Size >> 4;
      this.m_treeHeight = -1;
      Vector3I vector3I2 = vector3I1;
      while (vector3I2 != Vector3I.Zero)
      {
        vector3I2 >>= 1;
        ++this.m_treeHeight;
      }
      if (this.m_treeHeight >= 0)
        return;
      this.m_treeHeight = 1;
    }

    protected override void ResetInternal(MyStorageDataTypeFlags dataToReset)
    {
      this.AccessReset();
      bool flag1 = (uint) (dataToReset & MyStorageDataTypeFlags.Content) > 0U;
      bool flag2 = (uint) (dataToReset & MyStorageDataTypeFlags.Material) > 0U;
      if (flag1)
      {
        this.m_contentLeaves.Clear();
        this.m_contentNodes.Clear();
      }
      if (flag2)
      {
        this.m_materialLeaves.Clear();
        this.m_materialNodes.Clear();
      }
      if (this.m_dataProvider != null)
      {
        MyCellCoord cell = new MyCellCoord(this.m_treeHeight, ref Vector3I.Zero);
        ulong key = cell.PackId64();
        cell.Lod += 4;
        Vector3I vector3I = this.Size - 1;
        if (flag1)
          this.m_contentLeaves.Add(key, (IMyOctreeLeafNode) new MyProviderLeaf(this.m_dataProvider, MyStorageDataTypeEnum.Content, ref cell));
        if (!flag2)
          return;
        this.m_materialLeaves.Add(key, (IMyOctreeLeafNode) new MyProviderLeaf(this.m_dataProvider, MyStorageDataTypeEnum.Material, ref cell));
      }
      else
      {
        MyCellCoord coord = new MyCellCoord(this.m_treeHeight - 1, ref Vector3I.Zero);
        ulong key = coord.PackId64();
        coord.Lod += 5;
        if (flag1)
        {
          this.m_contentNodes.Add(key, new MyOctreeNode());
          this.AccessRange(MyStorageBase.MyAccessType.Write, MyStorageDataTypeEnum.Content, ref coord);
        }
        if (!flag2)
          return;
        this.m_materialNodes.Add(key, new MyOctreeNode(byte.MaxValue));
        this.AccessRange(MyStorageBase.MyAccessType.Write, MyStorageDataTypeEnum.Material, ref coord);
      }
    }

    protected override void OverwriteAllMaterialsInternal(MyVoxelMaterialDefinition material)
    {
    }

    protected override void LoadInternal(int fileVersion, Stream stream, ref bool isOldFormat)
    {
      if (fileVersion == 2)
        this.ReadStorageAccess(stream);
      MyOctreeStorage.ChunkHeader header = new MyOctreeStorage.ChunkHeader();
      Dictionary<byte, MyVoxelMaterialDefinition> dictionary = (Dictionary<byte, MyVoxelMaterialDefinition>) null;
      HashSet<ulong> outKeySet1 = new HashSet<ulong>();
      HashSet<ulong> outKeySet2 = new HashSet<ulong>();
      while (header.ChunkType != MyOctreeStorage.ChunkTypeEnum.EndOfFile)
      {
        header.ReadFrom(stream);
        ulong key;
        switch (header.ChunkType)
        {
          case MyOctreeStorage.ChunkTypeEnum.StorageMetaData:
            this.ReadStorageMetaData(stream, header, ref isOldFormat);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.MaterialIndexTable:
            dictionary = MyOctreeStorage.ReadMaterialTable(stream, header, ref isOldFormat);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.MacroContentNodes:
            MyOctreeStorage.ReadOctreeNodes(stream, header, ref isOldFormat, this.m_contentNodes, (Action<MyCellCoord>) (coord => this.LoadAccess(stream, coord)));
            continue;
          case MyOctreeStorage.ChunkTypeEnum.MacroMaterialNodes:
            MyOctreeStorage.ReadOctreeNodes(stream, header, ref isOldFormat, this.m_materialNodes, (Action<MyCellCoord>) null);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.ContentLeafProvider:
            this.ReadProviderLeaf(stream, header, ref isOldFormat, outKeySet2);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.ContentLeafOctree:
            MyMicroOctreeLeaf contentLeaf1;
            this.ReadOctreeLeaf(stream, header, ref isOldFormat, MyStorageDataTypeEnum.Content, out key, out contentLeaf1);
            this.m_contentLeaves.Add(key, (IMyOctreeLeafNode) contentLeaf1);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.MaterialLeafProvider:
            this.ReadProviderLeaf(stream, header, ref isOldFormat, outKeySet1);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.MaterialLeafOctree:
            MyMicroOctreeLeaf contentLeaf2;
            this.ReadOctreeLeaf(stream, header, ref isOldFormat, MyStorageDataTypeEnum.Material, out key, out contentLeaf2);
            this.m_materialLeaves.Add(key, (IMyOctreeLeafNode) contentLeaf2);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.DataProvider:
            MyOctreeStorage.ReadDataProvider(stream, header, ref isOldFormat, out this.m_dataProvider);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.EndOfFile:
            continue;
          default:
            throw new InvalidBranchException();
        }
      }
      MyCellCoord cell = new MyCellCoord();
      foreach (ulong num in outKeySet2)
      {
        cell.SetUnpack(num);
        cell.Lod += 4;
        this.m_contentLeaves.Add(num, (IMyOctreeLeafNode) new MyProviderLeaf(this.m_dataProvider, MyStorageDataTypeEnum.Content, ref cell));
      }
      foreach (ulong num in outKeySet1)
      {
        cell.SetUnpack(num);
        cell.Lod += 4;
        this.m_materialLeaves.Add(num, (IMyOctreeLeafNode) new MyProviderLeaf(this.m_dataProvider, MyStorageDataTypeEnum.Material, ref cell));
      }
      bool flag = false;
      foreach (KeyValuePair<byte, MyVoxelMaterialDefinition> keyValuePair in dictionary)
      {
        if ((int) keyValuePair.Key != (int) keyValuePair.Value.Index)
          flag = true;
        this.m_oldToNewIndexMap.Add(keyValuePair.Key, keyValuePair.Value.Index);
      }
      if (flag)
      {
        if (this.m_dataProvider != null)
          this.m_dataProvider.ReindexMaterials(this.m_oldToNewIndexMap);
        foreach (KeyValuePair<ulong, IMyOctreeLeafNode> materialLeaf in this.m_materialLeaves)
          materialLeaf.Value.ReplaceValues(this.m_oldToNewIndexMap);
        MySparseOctree.ReplaceValues<ulong>((IDictionary<ulong, MyOctreeNode>) this.m_materialNodes, this.m_oldToNewIndexMap);
      }
      this.m_oldToNewIndexMap.Clear();
    }

    protected override void SaveInternal(Stream stream)
    {
      this.WriteStorageAccess(stream);
      this.WriteStorageMetaData(stream);
      MyOctreeStorage.WriteMaterialTable(stream);
      MyOctreeStorage.WriteDataProvider(stream, this.m_dataProvider);
      MyOctreeStorage.WriteOctreeNodes(stream, MyOctreeStorage.ChunkTypeEnum.MacroContentNodes, this.m_contentNodes, new Action<Stream, MyCellCoord>(((MyStorageBase) this).SaveAccess));
      MyOctreeStorage.WriteOctreeNodes(stream, MyOctreeStorage.ChunkTypeEnum.MacroMaterialNodes, this.m_materialNodes, (Action<Stream, MyCellCoord>) null);
      MyOctreeStorage.WriteOctreeLeaves<IMyOctreeLeafNode>(stream, this.m_contentLeaves);
      MyOctreeStorage.WriteOctreeLeaves<IMyOctreeLeafNode>(stream, this.m_materialLeaves);
      new MyOctreeStorage.ChunkHeader()
      {
        ChunkType = MyOctreeStorage.ChunkTypeEnum.EndOfFile
      }.WriteTo(stream);
    }

    public override VRage.Game.Voxels.IMyStorage Copy()
    {
      bool isOldFormat = false;
      byte[] outCompressedData;
      this.Save(out outCompressedData);
      MyStorageBase myStorageBase = MyStorageBase.Load(outCompressedData, out isOldFormat);
      myStorageBase.Shared = false;
      return (VRage.Game.Voxels.IMyStorage) myStorageBase;
    }

    private static void ConsiderContent(MyStorageData storage)
    {
      byte[] numArray1 = storage[MyStorageDataTypeEnum.Content];
      byte[] numArray2 = storage[MyStorageDataTypeEnum.Material];
      for (int index = 0; index < storage.SizeLinear; ++index)
      {
        if (numArray1[index] == (byte) 0)
          numArray2[index] = byte.MaxValue;
      }
    }

    protected override void ReadRangeInternal(
      MyStorageData target,
      ref Vector3I targetWriteOffset,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      ref Vector3I lodVoxelCoordStart,
      ref Vector3I lodVoxelCoordEnd,
      ref MyVoxelRequestFlags flags)
    {
      bool flag = lodIndex <= this.m_treeHeight + 4;
      MyVoxelRequestFlags flags1 = (MyVoxelRequestFlags) 0;
      MyVoxelRequestFlags flags2 = (MyVoxelRequestFlags) 0;
      if ((dataToRead & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None && flag)
      {
        flags1 = flags;
        this.ReadRange(target, ref targetWriteOffset, MyStorageDataTypeFlags.Content, this.m_treeHeight, this.m_contentNodes, this.m_contentLeaves, lodIndex, ref lodVoxelCoordStart, ref lodVoxelCoordEnd, ref flags1);
      }
      if (dataToRead.Requests(MyStorageDataTypeEnum.Material) && !flags1.HasFlags(MyVoxelRequestFlags.EmptyData) && flag)
      {
        flags2 = flags;
        this.ReadRange(target, ref targetWriteOffset, dataToRead.Without(MyStorageDataTypeEnum.Content), this.m_treeHeight, this.m_materialNodes, this.m_materialLeaves, lodIndex, ref lodVoxelCoordStart, ref lodVoxelCoordEnd, ref flags2);
        if ((dataToRead & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
          flags2 &= ~MyVoxelRequestFlags.EmptyData;
        if (flags.HasFlags(MyVoxelRequestFlags.ConsiderContent))
          MyOctreeStorage.ConsiderContent(target);
      }
      flags = flags1 | flags2;
    }

    protected override unsafe bool IsModifiedInternal(ref BoundingBoxI range)
    {
      int num1 = 0;
      MyCellCoord* myCellCoordPtr1 = stackalloc MyCellCoord[MySparseOctree.EstimateStackSize(this.m_treeHeight)];
      MyCellCoord myCellCoord1 = new MyCellCoord(this.m_treeHeight + 4, ref Vector3I.Zero);
      MyCellCoord* myCellCoordPtr2 = myCellCoordPtr1;
      int num2 = num1;
      int num3 = num2 + 1;
      IntPtr num4 = (IntPtr) num2 * sizeof (MyCellCoord);
      *(MyCellCoord*) ((IntPtr) myCellCoordPtr2 + num4) = myCellCoord1;
      MyCellCoord myCellCoord2 = new MyCellCoord();
      BoundingBoxI boundingBoxI = range;
      while (num3 > 0)
      {
        MyCellCoord myCellCoord3 = myCellCoordPtr1[--num3];
        myCellCoord2.Lod = Math.Max(myCellCoord3.Lod - 4, 0);
        myCellCoord2.CoordInLod = myCellCoord3.CoordInLod;
        IMyOctreeLeafNode myOctreeLeafNode;
        if (this.m_contentLeaves.TryGetValue(myCellCoord2.PackId64(), out myOctreeLeafNode))
        {
          if (!(myOctreeLeafNode is MyProviderLeaf))
            return true;
        }
        else
        {
          --myCellCoord2.Lod;
          int num5 = myCellCoord3.Lod - 1;
          MyOctreeNode contentNode = this.m_contentNodes[myCellCoord2.PackId64()];
          Vector3I vector3I1 = boundingBoxI.Min >> num5;
          Vector3I max = boundingBoxI.Max >> num5;
          Vector3I vector3I2 = myCellCoord3.CoordInLod << 1;
          Vector3I min = vector3I1 - vector3I2;
          max -= vector3I2;
          for (int index = 0; index < 8; ++index)
          {
            Vector3I relativeCoord;
            MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
            if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
            {
              if (myCellCoord3.Lod == 0)
                return true;
              if (contentNode.HasChild(index))
                myCellCoordPtr1[num3++] = new MyCellCoord(myCellCoord3.Lod - 1, vector3I2 + relativeCoord);
            }
          }
        }
      }
      return false;
    }

    public override void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode)
    {
      base.DebugDraw(ref worldMatrix, mode);
      Color cornflowerBlue = Color.CornflowerBlue;
      cornflowerBlue.A = (byte) 25;
      this.DebugDraw(ref worldMatrix, mode, cornflowerBlue);
    }

    public void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode, Color color)
    {
      switch (mode)
      {
        case MyVoxelDebugDrawMode.Content_MicroNodes:
        case MyVoxelDebugDrawMode.Content_MicroNodesScaled:
          MyOctreeStorage.DrawSparseOctrees(ref worldMatrix, color, mode, this.m_contentLeaves);
          break;
        case MyVoxelDebugDrawMode.Content_MacroNodes:
          MyOctreeStorage.DrawNodes(ref worldMatrix, color, this.m_contentNodes);
          break;
        case MyVoxelDebugDrawMode.Content_MacroLeaves:
          MyOctreeStorage.DrawLeaves(ref worldMatrix, color, this.m_contentLeaves);
          break;
        case MyVoxelDebugDrawMode.Content_MacroScaled:
          MyOctreeStorage.DrawScaledNodes(ref worldMatrix, color, this.m_contentNodes);
          break;
        case MyVoxelDebugDrawMode.Materials_MacroNodes:
          MyOctreeStorage.DrawNodes(ref worldMatrix, color, this.m_materialNodes);
          break;
        case MyVoxelDebugDrawMode.Materials_MacroLeaves:
          MyOctreeStorage.DrawLeaves(ref worldMatrix, color, this.m_materialLeaves);
          break;
        case MyVoxelDebugDrawMode.Content_DataProvider:
          if (this.m_dataProvider == null)
            break;
          this.m_dataProvider.DebugDraw(ref worldMatrix);
          break;
      }
    }

    public void Voxelize(MyStorageDataTypeFlags data)
    {
      MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.EmptyData;
      MyStorageData myStorageData = new MyStorageData();
      myStorageData.Resize(new Vector3I(16));
      Vector3I vector3I1 = this.Size / 16;
      Vector3I next = Vector3I.Zero;
      Vector3I end = vector3I1 - 1;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref Vector3I.Zero, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        Vector3I vector3I2 = next * 16;
        Vector3I vector3I3 = vector3I2 + 15;
        this.ReadRangeInternal(myStorageData, ref Vector3I.Zero, data, 0, ref vector3I2, ref vector3I3, ref requestFlags);
        MyStorageDataWriteOperator source = new MyStorageDataWriteOperator(myStorageData);
        this.WriteRangeInternal<MyStorageDataWriteOperator>(ref source, data, ref vector3I2, ref vector3I3);
        vector3IRangeIterator.GetNext(out next);
      }
      this.OnRangeChanged(Vector3I.Zero, this.Size - 1, data);
    }

    private unsafe void ReadRange(
      MyStorageData target,
      ref Vector3I targetWriteOffset,
      MyStorageDataTypeFlags types,
      int treeHeight,
      Dictionary<ulong, MyOctreeNode> nodes,
      Dictionary<ulong, IMyOctreeLeafNode> leaves,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      ref MyVoxelRequestFlags flags)
    {
      int num1 = 0;
      MyCellCoord* myCellCoordPtr1 = stackalloc MyCellCoord[MySparseOctree.EstimateStackSize(treeHeight)];
      MyCellCoord myCellCoord1 = new MyCellCoord(treeHeight + 4, ref Vector3I.Zero);
      MyCellCoord* myCellCoordPtr2 = myCellCoordPtr1;
      int num2 = num1;
      int num3 = num2 + 1;
      IntPtr num4 = (IntPtr) num2 * sizeof (MyCellCoord);
      *(MyCellCoord*) ((IntPtr) myCellCoordPtr2 + num4) = myCellCoord1;
      MyCellCoord myCellCoord2 = new MyCellCoord();
      MyStorageDataTypeEnum type = types.Requests(MyStorageDataTypeEnum.Content) ? MyStorageDataTypeEnum.Content : MyStorageDataTypeEnum.Material;
      Vector3I step = target.Step;
      byte num5 = types.Requests(MyStorageDataTypeEnum.Content) ? (byte) 0 : byte.MaxValue;
      byte maxValue = byte.MaxValue;
      flags |= MyVoxelRequestFlags.FullContent;
      this.FillOutOfBounds(target, type, ref targetWriteOffset, lodIndex, minInLod, maxInLod);
      while (num3 > 0)
      {
        MyCellCoord myCellCoord3 = myCellCoordPtr1[--num3];
        myCellCoord2.Lod = Math.Max(myCellCoord3.Lod - 4, 0);
        myCellCoord2.CoordInLod = myCellCoord3.CoordInLod;
        IMyOctreeLeafNode myOctreeLeafNode;
        if (leaves.TryGetValue(myCellCoord2.PackId64(), out myOctreeLeafNode))
        {
          int num6 = myCellCoord3.Lod - lodIndex;
          Vector3I min = minInLod >> num6;
          Vector3I max1 = maxInLod >> num6;
          if (myCellCoord3.CoordInLod.IsInsideInclusiveEnd(ref min, ref max1))
          {
            Vector3I vector3I = myCellCoord3.CoordInLod << num6;
            Vector3I result = vector3I - minInLod;
            Vector3I.Max(ref result, ref Vector3I.Zero, out result);
            result += targetWriteOffset;
            Vector3I max2 = new Vector3I((1 << num6) - 1);
            Vector3I minInLod1 = minInLod - vector3I;
            Vector3I maxInLod1 = maxInLod - vector3I;
            if (!minInLod1.IsInsideInclusiveEnd(Vector3I.Zero, max2) || !maxInLod1.IsInsideInclusiveEnd(Vector3I.Zero, max2))
            {
              minInLod1 = Vector3I.Clamp(minInLod - vector3I, Vector3I.Zero, max2);
              maxInLod1 = Vector3I.Clamp(maxInLod - vector3I, Vector3I.Zero, max2);
            }
            MyVoxelRequestFlags flags1 = flags;
            myOctreeLeafNode.ReadRange(target, types, ref result, lodIndex, ref minInLod1, ref maxInLod1, ref flags1);
            flags = flags & (flags1 & (MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent)) | flags & ~(MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent);
          }
        }
        else
        {
          --myCellCoord2.Lod;
          int num6 = myCellCoord3.Lod - 1 - lodIndex;
          MyOctreeNode myOctreeNode;
          if (nodes.TryGetValue(myCellCoord2.PackId64(), out myOctreeNode))
          {
            Vector3I min = minInLod >> num6;
            Vector3I max = maxInLod >> num6;
            Vector3I vector3I1 = myCellCoord3.CoordInLod << 1;
            min -= vector3I1;
            max -= vector3I1;
            for (int index1 = 0; index1 < 8; ++index1)
            {
              Vector3I relativeCoord;
              MyOctreeStorage.ComputeChildCoord(index1, out relativeCoord);
              if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
              {
                if (lodIndex < myCellCoord3.Lod && myOctreeNode.HasChild(index1))
                {
                  myCellCoordPtr1[num3++] = new MyCellCoord(myCellCoord3.Lod - 1, vector3I1 + relativeCoord);
                }
                else
                {
                  flags &= MyVoxelRequestFlags.RequestFlags;
                  Vector3I result1 = vector3I1 + relativeCoord;
                  result1 <<= num6;
                  Vector3I result2 = result1 - minInLod;
                  Vector3I.Max(ref result2, ref Vector3I.Zero, out result2);
                  Vector3I vector3I2 = result2 + targetWriteOffset;
                  byte data = myOctreeNode.GetData(index1);
                  if ((int) data != (int) num5)
                    flags &= ~MyVoxelRequestFlags.EmptyData;
                  if ((int) data != (int) maxValue)
                    flags &= ~MyVoxelRequestFlags.FullContent;
                  if (num6 == 0)
                  {
                    int index2 = vector3I2.Dot(ref step);
                    target[type][index2] = data;
                  }
                  else
                  {
                    Vector3I result3 = result1 + ((1 << num6) - 1);
                    Vector3I.Max(ref result1, ref minInLod, out result1);
                    Vector3I.Min(ref result3, ref maxInLod, out result3);
                    for (int z = result1.Z; z <= result3.Z; ++z)
                    {
                      for (int y = result1.Y; y <= result3.Y; ++y)
                      {
                        for (int x = result1.X; x <= result3.X; ++x)
                        {
                          Vector3I vector3I3 = vector3I2;
                          vector3I3.X += x - result1.X;
                          vector3I3.Y += y - result1.Y;
                          vector3I3.Z += z - result1.Z;
                          int index2 = vector3I3.Dot(ref step);
                          target[type][index2] = data;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private void FillOutOfBounds(
      MyStorageData target,
      MyStorageDataTypeEnum type,
      ref Vector3I woffset,
      int lodIndex,
      Vector3I minInLod,
      Vector3I maxInLod)
    {
      byte content = MyVoxelDataConstants.DefaultValue(type);
      Vector3I max = new Vector3I((1 << this.m_treeHeight + 4 - lodIndex) - 1);
      Vector3I vector3I1 = woffset - minInLod;
      BoundingBoxI boundingBoxI = new BoundingBoxI(minInLod, maxInLod);
      BoundingBoxI box = new BoundingBoxI(Vector3I.Zero, max);
      if (!boundingBoxI.Intersects(ref box))
      {
        target.BlockFill(type, vector3I1 + minInLod, vector3I1 + maxInLod, content);
      }
      else
      {
        if (minInLod.X < 0)
        {
          Vector3I vector3I2 = minInLod;
          Vector3I vector3I3 = maxInLod;
          vector3I3.X = -1;
          minInLod.X = 0;
          target.BlockFill(type, vector3I2 + vector3I1, vector3I3 + vector3I1, content);
        }
        if (maxInLod.X > max.X)
        {
          Vector3I vector3I2 = minInLod;
          Vector3I vector3I3 = maxInLod;
          vector3I2.X = max.X + 1;
          minInLod.X = max.X;
          target.BlockFill(type, vector3I2 + vector3I1, vector3I3 + vector3I1, content);
        }
        if (minInLod.Y < 0)
        {
          Vector3I vector3I2 = minInLod;
          Vector3I vector3I3 = maxInLod;
          vector3I3.Y = -1;
          minInLod.Y = 0;
          target.BlockFill(type, vector3I2 + vector3I1, vector3I3 + vector3I1, content);
        }
        if (maxInLod.Y > max.Y)
        {
          Vector3I vector3I2 = minInLod;
          Vector3I vector3I3 = maxInLod;
          vector3I2.Y = max.Y + 1;
          minInLod.Y = max.Y;
          target.BlockFill(type, vector3I2 + vector3I1, vector3I3 + vector3I1, content);
        }
        if (minInLod.Y < 0)
        {
          Vector3I vector3I2 = minInLod;
          Vector3I vector3I3 = maxInLod;
          vector3I3.Y = -1;
          minInLod.Y = 0;
          target.BlockFill(type, vector3I2 + vector3I1, vector3I3 + vector3I1, content);
        }
        if (maxInLod.Y <= max.Y)
          return;
        Vector3I vector3I4 = minInLod;
        Vector3I vector3I5 = maxInLod;
        vector3I4.Y = max.Y + 1;
        minInLod.Y = max.Y;
        target.BlockFill(type, vector3I4 + vector3I1, vector3I5 + vector3I1, content);
      }
    }

    protected override void WriteRangeInternal<TOperator>(
      ref TOperator source,
      MyStorageDataTypeFlags dataToWrite,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax)
    {
      if ((dataToWrite & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
      {
        MyOctreeStorage.TraverseArgs<MyOctreeStorage.WriteRangeOps<TOperator>> args = this.ContentArgs<MyOctreeStorage.WriteRangeOps<TOperator>>(ref voxelRangeMin, ref voxelRangeMax);
        args.Operator.Source = source;
        int num = (int) MyOctreeStorage.Traverse<MyOctreeStorage.WriteRangeOps<TOperator>>(ref args, (byte) 0, this.m_treeHeight + 4, Vector3I.Zero);
        source = args.Operator.Source;
      }
      if ((dataToWrite & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      MyOctreeStorage.TraverseArgs<MyOctreeStorage.WriteRangeOps<TOperator>> args1 = this.MaterialArgs<MyOctreeStorage.WriteRangeOps<TOperator>>(ref voxelRangeMin, ref voxelRangeMax);
      args1.Operator.Source = source;
      int num1 = (int) MyOctreeStorage.Traverse<MyOctreeStorage.WriteRangeOps<TOperator>>(ref args1, (byte) 0, this.m_treeHeight + 4, Vector3I.Zero);
      source = args1.Operator.Source;
    }

    protected override void DeleteRangeInternal(
      MyStorageDataTypeFlags dataToDelete,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax)
    {
      if ((dataToDelete & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
      {
        MyOctreeStorage.TraverseArgs<MyOctreeStorage.DeleteRangeOps> args = this.ContentArgs<MyOctreeStorage.DeleteRangeOps>(ref voxelRangeMin, ref voxelRangeMax);
        int num = (int) MyOctreeStorage.Traverse<MyOctreeStorage.DeleteRangeOps>(ref args, (byte) 0, this.m_treeHeight + 4, Vector3I.Zero);
      }
      if ((dataToDelete & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      MyOctreeStorage.TraverseArgs<MyOctreeStorage.DeleteRangeOps> args1 = this.MaterialArgs<MyOctreeStorage.DeleteRangeOps>(ref voxelRangeMin, ref voxelRangeMax);
      int num1 = (int) MyOctreeStorage.Traverse<MyOctreeStorage.DeleteRangeOps>(ref args1, (byte) 0, this.m_treeHeight + 4, Vector3I.Zero);
    }

    protected override void SweepInternal(MyStorageDataTypeFlags dataToSweep)
    {
      new MyOctreeStorage.SweepRangeOps().Storage = this;
      if ((dataToSweep & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
      {
        Vector3I size = this.Size;
        MyOctreeStorage.TraverseArgs<MyOctreeStorage.SweepRangeOps> args = this.ContentArgs<MyOctreeStorage.SweepRangeOps>(ref Vector3I.Zero, ref size);
        int num = (int) MyOctreeStorage.Traverse<MyOctreeStorage.SweepRangeOps>(ref args, (byte) 0, this.m_treeHeight + 4, Vector3I.Zero);
      }
      if ((dataToSweep & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      Vector3I size1 = this.Size;
      MyOctreeStorage.TraverseArgs<MyOctreeStorage.SweepRangeOps> args1 = this.MaterialArgs<MyOctreeStorage.SweepRangeOps>(ref Vector3I.Zero, ref size1);
      int num1 = (int) MyOctreeStorage.Traverse<MyOctreeStorage.SweepRangeOps>(ref args1, (byte) 0, this.m_treeHeight + 4, Vector3I.Zero);
    }

    private MyOctreeStorage.TraverseArgs<TOperator> ContentArgs<TOperator>(
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax)
      where TOperator : struct, MyOctreeStorage.ITraverseOps
    {
      return new MyOctreeStorage.TraverseArgs<TOperator>()
      {
        Storage = this,
        Operator = default (TOperator),
        DataFilter = MyOctreeNode.ContentFilter,
        DataType = MyStorageDataTypeEnum.Content,
        Leaves = this.m_contentLeaves,
        Nodes = this.m_contentNodes,
        Min = voxelRangeMin,
        Max = voxelRangeMax
      };
    }

    private MyOctreeStorage.TraverseArgs<TOperator> MaterialArgs<TOperator>(
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax)
      where TOperator : struct, MyOctreeStorage.ITraverseOps
    {
      return new MyOctreeStorage.TraverseArgs<TOperator>()
      {
        Storage = this,
        Operator = default (TOperator),
        DataFilter = MyOctreeNode.MaterialFilter,
        DataType = MyStorageDataTypeEnum.Material,
        Leaves = this.m_materialLeaves,
        ContentLeaves = this.m_contentLeaves,
        Nodes = this.m_materialNodes,
        Min = voxelRangeMin,
        Max = voxelRangeMax
      };
    }

    private static MyOctreeStorage.ChildType Traverse<TOps>(
      ref MyOctreeStorage.TraverseArgs<TOps> args,
      byte defaultData,
      int lodIdx,
      Vector3I lodCoord)
      where TOps : struct, MyOctreeStorage.ITraverseOps
    {
      MyCellCoord coord = new MyCellCoord(lodIdx, ref lodCoord);
      MyOctreeNode node1;
      MyOctreeStorage.ChildType childType = args.Operator.Init<TOps>(ref args, ref coord, defaultData, out node1);
      if (childType != MyOctreeStorage.ChildType.Node)
        return childType;
      int num = 0;
      if (lodIdx <= 5)
      {
        childType = args.Operator.LeafOp<TOps>(ref args, ref coord, defaultData, ref node1);
      }
      else
      {
        MyCellCoord myCellCoord1 = new MyCellCoord()
        {
          Lod = lodIdx - 2 - 4
        };
        Vector3I vector3I1 = lodCoord << 1;
        Vector3I min = (args.Min >> lodIdx - 1) - vector3I1;
        Vector3I max = (args.Max >> lodIdx - 1) - vector3I1;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord1;
          MyOctreeStorage.ComputeChildCoord(index, out relativeCoord1);
          myCellCoord1.CoordInLod = vector3I1 + relativeCoord1;
          if (!relativeCoord1.IsInsideInclusiveEnd(ref min, ref max))
          {
            MyCellCoord myCellCoord2 = myCellCoord1;
            myCellCoord2.Lod = lodIdx - 1 - 4;
            ulong key = myCellCoord2.PackId64();
            IMyOctreeLeafNode myOctreeLeafNode;
            if (args.Leaves.TryGetValue(key, out myOctreeLeafNode) && myOctreeLeafNode.ReadOnly)
              ++num;
          }
          else
          {
            switch (MyOctreeStorage.Traverse<TOps>(ref args, node1.GetData(index), lodIdx - 1, myCellCoord1.CoordInLod))
            {
              case MyOctreeStorage.ChildType.NodeMissing:
                ++num;
                continue;
              case MyOctreeStorage.ChildType.NodeEmpty:
                ulong key1 = myCellCoord1.PackId64();
                MyOctreeNode node2 = args.Nodes[key1];
                node1.SetChild(index, false);
                byte data = node2.GetData(0);
                node1.SetData(index, data);
                args.Nodes.Remove(key1);
                continue;
              case MyOctreeStorage.ChildType.Node:
                ulong key2 = myCellCoord1.PackId64();
                MyOctreeNode node3 = args.Nodes[key2];
                node1.SetChild(index, true);
                byte filteredValue = node3.ComputeFilteredValue(args.DataFilter, myCellCoord1.Lod);
                node1.SetData(index, filteredValue);
                continue;
              case MyOctreeStorage.ChildType.LeafReadonly:
                ++num;
                continue;
              case MyOctreeStorage.ChildType.NodeWithLeafReadonly:
                ++num;
                ulong key3 = myCellCoord1.PackId64();
                args.Nodes.Remove(key3);
                int lod = lodIdx - 2 - 4;
                Vector3I vector3I2 = myCellCoord1.CoordInLod << 1;
                for (int childIdx = 0; childIdx < 8; ++childIdx)
                {
                  Vector3I relativeCoord2;
                  MyOctreeStorage.ComputeChildCoord(childIdx, out relativeCoord2);
                  ulong key4 = new MyCellCoord(lod, vector3I2 + relativeCoord2).PackId64();
                  IMyOctreeLeafNode myOctreeLeafNode;
                  if (args.Leaves.TryGetValue(key4, out myOctreeLeafNode))
                  {
                    myOctreeLeafNode.Dispose();
                    args.Leaves.Remove(key4);
                  }
                }
                MyCellCoord myCellCoord3 = new MyCellCoord(lod + 1, ref myCellCoord1.CoordInLod);
                ulong key5 = myCellCoord3.PackId64();
                MyCellCoord myCellCoord4 = myCellCoord3;
                myCellCoord4.Lod += 4;
                IMyOctreeLeafNode myOctreeLeafNode1 = (IMyOctreeLeafNode) new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref myCellCoord4);
                args.Leaves[key5] = myOctreeLeafNode1;
                node1.SetData(index, myOctreeLeafNode1.GetFilteredValue());
                args.Storage.AccessRange(MyStorageBase.MyAccessType.Delete, args.DataType, ref myCellCoord4);
                continue;
              default:
                continue;
            }
          }
        }
        if (num == 8)
          childType = MyOctreeStorage.ChildType.NodeWithLeafReadonly;
      }
      args.Nodes[new MyCellCoord(lodIdx - 1 - 4, ref lodCoord).PackId64()] = node1;
      if (!node1.HasChildren && node1.AllDataSame())
        childType = MyOctreeStorage.ChildType.NodeEmpty;
      return childType;
    }

    private static void DrawNodes(
      ref MatrixD worldMatrix,
      Color color,
      Dictionary<ulong, MyOctreeNode> octree)
    {
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        foreach (KeyValuePair<ulong, MyOctreeNode> keyValuePair in octree)
        {
          myCellCoord.SetUnpack(keyValuePair.Key);
          myCellCoord.Lod += 4;
          MyOctreeNode myOctreeNode = keyValuePair.Value;
          for (int index = 0; index < 8; ++index)
          {
            if (!myOctreeNode.HasChild(index))
            {
              Vector3I relativeCoord;
              MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
              Vector3I vector3I = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
              float num = 1f * (float) (1 << myCellCoord.Lod);
              Vector3 vector3 = vector3I * 1f + 0.5f * num;
              BoundingBoxD aabb = new BoundingBoxD((Vector3D) (vector3 - 0.5f * num), (Vector3D) (vector3 + 0.5f * num));
              debugDrawBatchAabb.Add(ref aabb);
            }
          }
        }
      }
    }

    private static void DrawLeaves(
      ref MatrixD worldMatrix,
      Color color,
      Dictionary<ulong, IMyOctreeLeafNode> octree)
    {
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        foreach (KeyValuePair<ulong, IMyOctreeLeafNode> keyValuePair in octree)
        {
          myCellCoord.SetUnpack(keyValuePair.Key);
          myCellCoord.Lod += 4;
          IMyOctreeLeafNode myOctreeLeafNode = keyValuePair.Value;
          Vector3I vector3I = myCellCoord.CoordInLod << myCellCoord.Lod;
          BoundingBoxD aabb = new BoundingBoxD((Vector3D) (vector3I * 1f), (Vector3D) ((vector3I + (1 << myCellCoord.Lod)) * 1f));
          debugDrawBatchAabb.Add(ref aabb);
        }
      }
    }

    private static void DrawScaledNodes(
      ref MatrixD worldMatrix,
      Color color,
      Dictionary<ulong, MyOctreeNode> octree)
    {
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        foreach (KeyValuePair<ulong, MyOctreeNode> keyValuePair in octree)
        {
          myCellCoord.SetUnpack(keyValuePair.Key);
          myCellCoord.Lod += 4;
          MyOctreeNode myOctreeNode = keyValuePair.Value;
          for (int index = 0; index < 8; ++index)
          {
            if (!myOctreeNode.HasChild(index) || myCellCoord.Lod == 4)
            {
              Vector3I relativeCoord;
              MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
              float num1 = (float) myOctreeNode.GetData(index) / (float) byte.MaxValue;
              if ((double) num1 != 0.0)
              {
                Vector3I vector3I = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
                float num2 = 1f * (float) (1 << myCellCoord.Lod);
                Vector3 vector3 = vector3I * 1f + 0.5f * num2;
                float num3 = (float) Math.Pow((double) num1 * 1.0, 0.3333);
                float num4 = num2 * num3;
                BoundingBoxD aabb = new BoundingBoxD((Vector3D) (vector3 - 0.5f * num4), (Vector3D) (vector3 + 0.5f * num4));
                debugDrawBatchAabb.Add(ref aabb);
              }
            }
          }
        }
      }
    }

    private static void DrawSparseOctrees(
      ref MatrixD worldMatrix,
      Color color,
      MyVoxelDebugDrawMode mode,
      Dictionary<ulong, IMyOctreeLeafNode> octree)
    {
      MyCamera mainCamera = MySector.MainCamera;
      if (mainCamera == null)
        return;
      MatrixD worldMatrix1 = mainCamera.WorldMatrix;
      Vector3 vector3 = (Vector3) Vector3D.Transform(worldMatrix1.Translation + worldMatrix1.Forward * 10.0, MatrixD.Invert(worldMatrix));
      using (IMyDebugDrawBatchAabb batch = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        foreach (KeyValuePair<ulong, IMyOctreeLeafNode> keyValuePair in octree)
        {
          if (keyValuePair.Value is MyMicroOctreeLeaf myMicroOctreeLeaf)
          {
            myCellCoord.SetUnpack(keyValuePair.Key);
            Vector3 min = (myCellCoord.CoordInLod << 4) * 1f;
            Vector3 max = min + 16f;
            if (vector3.IsInsideInclusive(ref min, ref max))
              myMicroOctreeLeaf.DebugDraw(batch, min, mode);
          }
        }
      }
    }

    private static void ComputeChildCoord(int childIdx, out Vector3I relativeCoord)
    {
      relativeCoord.X = childIdx & 1;
      relativeCoord.Y = childIdx >> 1 & 1;
      relativeCoord.Z = childIdx >> 2 & 1;
    }

    public override ContainmentType Intersect(ref BoundingBox box, bool lazy)
    {
      using (this.StorageLock.AcquireSharedUsing())
      {
        if (this.Closed)
          return ContainmentType.Disjoint;
        BoundingBoxI box1 = new BoundingBoxI(new Vector3I(box.Min), new Vector3I(Vector3.Ceiling(box.Max)));
        return this.IntersectInternal(ref box1, 0, !lazy);
      }
    }

    protected override unsafe ContainmentType IntersectInternal(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck)
    {
      int num1 = 0;
      MyCellCoord* myCellCoordPtr1 = stackalloc MyCellCoord[MySparseOctree.EstimateStackSize(this.m_treeHeight)];
      MyCellCoord myCellCoord1 = new MyCellCoord(this.m_treeHeight + 4, ref Vector3I.Zero);
      MyCellCoord* myCellCoordPtr2 = myCellCoordPtr1;
      int num2 = num1;
      int num3 = num2 + 1;
      IntPtr num4 = (IntPtr) num2 * sizeof (MyCellCoord);
      *(MyCellCoord*) ((IntPtr) myCellCoordPtr2 + num4) = myCellCoord1;
      MyCellCoord myCellCoord2 = new MyCellCoord();
      BoundingBoxI box1 = box;
      ContainmentType containmentType1 = ~ContainmentType.Disjoint;
      while (num3 > 0)
      {
        MyCellCoord myCellCoord3 = myCellCoordPtr1[--num3];
        myCellCoord2.Lod = Math.Max(myCellCoord3.Lod - 4, 0);
        myCellCoord2.CoordInLod = myCellCoord3.CoordInLod;
        IMyOctreeLeafNode myOctreeLeafNode;
        if (this.m_contentLeaves.TryGetValue(myCellCoord2.PackId64(), out myOctreeLeafNode))
        {
          int lod1 = myCellCoord3.Lod;
          Vector3I min1 = box1.Min >> lod1;
          Vector3I max = box1.Max >> lod1;
          if (myCellCoord3.CoordInLod.IsInsideInclusiveEnd(ref min1, ref max))
          {
            Vector3I min2 = myCellCoord2.CoordInLod << lod1;
            BoundingBoxI box2 = new BoundingBoxI(min2, min2 + (1 << lod1));
            box2.IntersectWith(ref box1);
            ContainmentType containmentType2 = myOctreeLeafNode.Intersect(ref box2, lod);
            if (containmentType1 == ~ContainmentType.Disjoint)
              containmentType1 = containmentType2;
            if (containmentType2 == ContainmentType.Intersects || containmentType2 != containmentType1 || containmentType1 == ContainmentType.Contains & exhaustiveContainmentCheck)
              return ContainmentType.Intersects;
          }
        }
        else
        {
          --myCellCoord2.Lod;
          int num5 = myCellCoord3.Lod - 1;
          MyOctreeNode myOctreeNode;
          if (this.m_contentNodes.TryGetValue(myCellCoord2.PackId64(), out myOctreeNode))
          {
            Vector3I vector3I1 = box1.Min >> num5;
            Vector3I max = box1.Max >> num5;
            Vector3I vector3I2 = myCellCoord3.CoordInLod << 1;
            Vector3I min = vector3I1 - vector3I2;
            max -= vector3I2;
            for (int index = 0; index < 8; ++index)
            {
              Vector3I relativeCoord;
              MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
              if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
              {
                if (myCellCoord3.Lod == 0)
                  return ContainmentType.Intersects;
                if (myOctreeNode.HasChild(index))
                  myCellCoordPtr1[num3++] = new MyCellCoord(myCellCoord3.Lod - 1, vector3I2 + relativeCoord);
                else if (myOctreeNode.GetData(index) != (byte) 0 && containmentType1 != ContainmentType.Contains)
                  containmentType1 = ContainmentType.Intersects;
              }
            }
          }
        }
      }
      if (containmentType1 == ~ContainmentType.Disjoint)
        containmentType1 = ContainmentType.Disjoint;
      return containmentType1;
    }

    public override unsafe bool IntersectInternal(ref LineD line)
    {
      MyCellCoord* myCellCoordPtr1 = stackalloc MyCellCoord[MySparseOctree.EstimateStackSize(this.m_treeHeight)];
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      LineD lineD;
      lineD.From = line.To;
      lineD.To = line.From;
      MyCellCoord myCellCoord1 = new MyCellCoord(this.m_treeHeight + 4, ref Vector3I.Zero);
      int num1 = 0;
      MyCellCoord* myCellCoordPtr2 = myCellCoordPtr1;
      int num2 = num1;
      int num3 = num2 + 1;
      IntPtr num4 = (IntPtr) num2 * sizeof (MyCellCoord);
      *(MyCellCoord*) ((IntPtr) myCellCoordPtr2 + num4) = myCellCoord1;
      MyCellCoord myCellCoord2 = new MyCellCoord();
      BoundingBoxI boundingBox = (BoundingBoxI) line.GetBoundingBox();
      while (num3 > 0)
      {
        MyCellCoord myCellCoord3 = myCellCoordPtr1[--num3];
        myCellCoord2.Lod = Math.Max(myCellCoord3.Lod - 4, 0);
        myCellCoord2.CoordInLod = myCellCoord3.CoordInLod;
        IMyOctreeLeafNode myOctreeLeafNode;
        if (this.m_contentLeaves.TryGetValue(myCellCoord2.PackId64(), out myOctreeLeafNode))
        {
          int lod = myCellCoord3.Lod;
          Vector3I min = boundingBox.Min >> lod;
          Vector3I max = boundingBox.Max >> lod;
          if (myCellCoord3.CoordInLod.IsInsideInclusiveEnd(ref min, ref max))
          {
            LineD intersectedLine;
            new BoundingBoxD((Vector3D) (min << myCellCoord3.Lod), (Vector3D) (max << myCellCoord3.Lod)).Intersect(ref line, out intersectedLine);
            if (myOctreeLeafNode.Intersect(ref intersectedLine, out double _, out double _))
            {
              if (invalid.Contains(intersectedLine.From) == ContainmentType.Disjoint)
              {
                lineD.From = intersectedLine.From;
                invalid.Include(intersectedLine.From);
              }
              if (invalid.Contains(intersectedLine.To) == ContainmentType.Disjoint)
              {
                lineD.To = intersectedLine.To;
                invalid.Include(intersectedLine.To);
              }
            }
          }
        }
        else
        {
          --myCellCoord2.Lod;
          int num5 = myCellCoord3.Lod - 1;
          MyOctreeNode myOctreeNode;
          if (this.m_contentNodes.TryGetValue(myCellCoord2.PackId64(), out myOctreeNode))
          {
            Vector3I min = boundingBox.Min >> num5;
            Vector3I max = boundingBox.Max >> num5;
            Vector3I vector3I = myCellCoord3.CoordInLod << 1;
            min -= vector3I;
            max -= vector3I;
            for (int index = 0; index < 8; ++index)
            {
              Vector3I relativeCoord;
              MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
              if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
              {
                BoundingBoxD box;
                box.Min = (Vector3D) (relativeCoord + myCellCoord2.CoordInLod << num5);
                box.Max = box.Min + (float) (1 << myCellCoord3.Lod);
                if (invalid.Contains(box) != ContainmentType.Contains)
                {
                  if (myOctreeNode.HasChild(index))
                  {
                    myCellCoordPtr1[num3++] = new MyCellCoord(myCellCoord3.Lod - 1, vector3I + relativeCoord);
                  }
                  else
                  {
                    int data = (int) myOctreeNode.GetData(index);
                  }
                }
              }
            }
          }
        }
      }
      if (!invalid.Valid)
        return false;
      line.To = lineD.To;
      line.From = lineD.From;
      line.Length = (lineD.To - lineD.From).Length();
      return true;
    }

    public ContainmentType Contains(
      ref BoundingBox queryBox,
      ref BoundingSphere querySphere,
      int lodVoxelSize)
    {
      return this.Intersect(ref queryBox, false);
    }

    public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
    {
      MyStorageData target = MyOctreeStorage.m_storageCached;
      MyOctreeStorage.m_storageCached = (MyStorageData) null;
      if (target == null)
      {
        target = new MyStorageData(MyStorageDataTypeFlags.Content);
        target.Resize(Vector3I.One);
      }
      Vector3I vector3I = new Vector3I(localPos);
      this.ReadRange(target, MyStorageDataTypeFlags.Content, 0, vector3I, vector3I);
      int num = (int) target.Get(MyStorageDataTypeEnum.Content, 0);
      MyOctreeStorage.m_storageCached = target;
      return (float) (((double) num / (double) byte.MaxValue - 0.5) * -2.0);
    }

    public void ComputeContent(
      MyStorageData storage,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax,
      int lodVoxelSize)
    {
      this.ReadRange(storage, MyStorageDataTypeFlags.Content, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
    }

    public void DebugDraw(ref MatrixD worldMatrix, Color color)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION_CONTENT)
        return;
      color.A = (byte) 10;
      this.DebugDraw(ref worldMatrix, MyVoxelDebugDrawMode.Content_MacroLeaves, color);
    }

    void IMyCompositeShape.Close()
    {
      if (this.Shared)
        return;
      this.Close();
    }

    public override void CloseInternal()
    {
      base.CloseInternal();
      foreach (IDisposable disposable in this.m_contentLeaves.Values)
        disposable.Dispose();
      foreach (IDisposable disposable in this.m_materialLeaves.Values)
        disposable.Dispose();
    }

    static MyOctreeStorage()
    {
      MyOctreeStorage.RegisterTypes(Assembly.GetExecutingAssembly());
      MyOctreeStorage.RegisterTypes(MyPlugins.GameAssembly);
      MyOctreeStorage.RegisterTypes(MyPlugins.SandboxAssembly);
      MyOctreeStorage.RegisterTypes(MyPlugins.UserAssemblies);
    }

    public static void RegisterTypes(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyOctreeStorage.RegisterTypes(assembly);
    }

    private static void RegisterTypes(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        object[] customAttributes = type.GetCustomAttributes(typeof (MyStorageDataProviderAttribute), false);
        if (customAttributes != null && customAttributes.Length != 0)
        {
          MyStorageDataProviderAttribute providerAttribute = (MyStorageDataProviderAttribute) customAttributes[0];
          providerAttribute.ProviderType = type;
          MyOctreeStorage.m_attributesById.Add(providerAttribute.ProviderTypeId, providerAttribute);
          MyOctreeStorage.m_attributesByType.Add(providerAttribute.ProviderType, providerAttribute);
        }
      }
    }

    private void WriteStorageMetaData(Stream stream)
    {
      new MyOctreeStorage.ChunkHeader()
      {
        ChunkType = MyOctreeStorage.ChunkTypeEnum.StorageMetaData,
        Version = 1,
        Size = 17
      }.WriteTo(stream);
      stream.WriteNoAlloc(4);
      stream.WriteNoAlloc(this.Size.X);
      stream.WriteNoAlloc(this.Size.Y);
      stream.WriteNoAlloc(this.Size.Z);
      stream.WriteNoAlloc((byte) 0);
    }

    private void ReadStorageMetaData(
      Stream stream,
      MyOctreeStorage.ChunkHeader header,
      ref bool isOldFormat)
    {
      stream.ReadInt32();
      Vector3I vector3I;
      vector3I.X = stream.ReadInt32();
      vector3I.Y = stream.ReadInt32();
      vector3I.Z = stream.ReadInt32();
      this.Size = vector3I;
      int num = (int) stream.ReadByteNoAlloc();
      this.InitTreeHeight();
      this.AccessReset();
    }

    private static unsafe void WriteOctreeNodes(
      Stream stream,
      MyOctreeStorage.ChunkTypeEnum type,
      Dictionary<ulong, MyOctreeNode> nodes,
      Action<Stream, MyCellCoord> saveAccess)
    {
      new MyOctreeStorage.ChunkHeader()
      {
        ChunkType = type,
        Version = 2,
        Size = (nodes.Count * 17)
      }.WriteTo(stream);
      foreach (KeyValuePair<ulong, MyOctreeNode> node in nodes)
      {
        stream.WriteNoAlloc(node.Key);
        MyOctreeNode myOctreeNode = node.Value;
        stream.WriteNoAlloc(myOctreeNode.ChildMask);
        stream.WriteNoAlloc(myOctreeNode.Data, 0, 8);
        if (saveAccess != null)
        {
          MyCellCoord myCellCoord = new MyCellCoord(MyCellCoord.UnpackLod(node.Key), MyCellCoord.UnpackCoord(node.Key));
          myCellCoord.Lod += 5;
          saveAccess(stream, myCellCoord);
        }
      }
    }

    private static unsafe void ReadOctreeNodes(
      Stream stream,
      MyOctreeStorage.ChunkHeader header,
      ref bool isOldFormat,
      Dictionary<ulong, MyOctreeNode> contentNodes,
      Action<MyCellCoord> loadAccess)
    {
      switch (header.Version)
      {
        case 1:
          int num1 = header.Size / 13;
          MyCellCoord myCellCoord1 = new MyCellCoord();
          for (int index = 0; index < num1; ++index)
          {
            myCellCoord1.SetUnpack(stream.ReadUInt32());
            MyOctreeNode myOctreeNode;
            myOctreeNode.ChildMask = stream.ReadByteNoAlloc();
            stream.ReadNoAlloc(myOctreeNode.Data, 0, 8);
            ulong num2 = myCellCoord1.PackId64();
            contentNodes.Add(num2, myOctreeNode);
            if (loadAccess != null)
            {
              MyCellCoord myCellCoord2 = new MyCellCoord(MyCellCoord.UnpackLod(num2), MyCellCoord.UnpackCoord(num2));
              myCellCoord2.Lod += 5;
              loadAccess(myCellCoord2);
            }
          }
          isOldFormat = true;
          break;
        case 2:
          int num3 = header.Size / 17;
          for (int index = 0; index < num3; ++index)
          {
            ulong num2 = stream.ReadUInt64();
            MyOctreeNode myOctreeNode;
            myOctreeNode.ChildMask = stream.ReadByteNoAlloc();
            stream.ReadNoAlloc(myOctreeNode.Data, 0, 8);
            contentNodes.Add(num2, myOctreeNode);
            if (loadAccess != null)
            {
              MyCellCoord myCellCoord2 = new MyCellCoord(MyCellCoord.UnpackLod(num2), MyCellCoord.UnpackCoord(num2));
              myCellCoord2.Lod += 5;
              loadAccess(myCellCoord2);
            }
          }
          break;
        default:
          throw new InvalidBranchException();
      }
    }

    private static void WriteMaterialTable(Stream stream)
    {
      DictionaryValuesReader<string, MyVoxelMaterialDefinition> materialDefinitions = MyDefinitionManager.Static.GetVoxelMaterialDefinitions();
      MemoryStream stream1;
      using (stream1 = new MemoryStream(1024))
      {
        stream1.WriteNoAlloc(materialDefinitions.Count);
        foreach (MyVoxelMaterialDefinition materialDefinition in materialDefinitions)
        {
          stream1.Write7BitEncodedInt((int) materialDefinition.Index);
          stream1.WriteNoAlloc(materialDefinition.Id.SubtypeName);
        }
      }
      byte[] array = stream1.ToArray();
      new MyOctreeStorage.ChunkHeader()
      {
        ChunkType = MyOctreeStorage.ChunkTypeEnum.MaterialIndexTable,
        Version = 1,
        Size = array.Length
      }.WriteTo(stream);
      stream.Write(array, 0, array.Length);
    }

    private static Dictionary<byte, MyVoxelMaterialDefinition> ReadMaterialTable(
      Stream stream,
      MyOctreeStorage.ChunkHeader header,
      ref bool isOldFormat)
    {
      int capacity = stream.ReadInt32();
      Dictionary<byte, MyVoxelMaterialDefinition> dictionary = new Dictionary<byte, MyVoxelMaterialDefinition>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        int num = stream.Read7BitEncodedInt();
        MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(stream.ReadString()) ?? new MyVoxelMaterialDefinition();
        dictionary.Add((byte) num, materialDefinition);
      }
      return dictionary;
    }

    private static void WriteOctreeLeaves<TLeaf>(Stream stream, Dictionary<ulong, TLeaf> leaves) where TLeaf : IMyOctreeLeafNode
    {
      foreach (KeyValuePair<ulong, TLeaf> leaf1 in leaves)
      {
        MyOctreeStorage.ChunkHeader chunkHeader1 = new MyOctreeStorage.ChunkHeader();
        ref MyOctreeStorage.ChunkHeader local1 = ref chunkHeader1;
        TLeaf leaf2 = leaf1.Value;
        int serializedChunkType = (int) leaf2.SerializedChunkType;
        local1.ChunkType = (MyOctreeStorage.ChunkTypeEnum) serializedChunkType;
        ref MyOctreeStorage.ChunkHeader local2 = ref chunkHeader1;
        leaf2 = leaf1.Value;
        int num = leaf2.SerializedChunkSize + 8;
        local2.Size = num;
        chunkHeader1.Version = 3;
        MyOctreeStorage.ChunkHeader chunkHeader2 = chunkHeader1;
        chunkHeader2.WriteTo(stream);
        stream.WriteNoAlloc(leaf1.Key);
        switch (chunkHeader2.ChunkType)
        {
          case MyOctreeStorage.ChunkTypeEnum.ContentLeafProvider:
          case MyOctreeStorage.ChunkTypeEnum.MaterialLeafProvider:
            continue;
          case MyOctreeStorage.ChunkTypeEnum.ContentLeafOctree:
            ((object) leaf1.Value as MyMicroOctreeLeaf).WriteTo(stream);
            continue;
          case MyOctreeStorage.ChunkTypeEnum.MaterialLeafOctree:
            ((object) leaf1.Value as MyMicroOctreeLeaf).WriteTo(stream);
            continue;
          default:
            throw new InvalidBranchException();
        }
      }
    }

    private void ReadOctreeLeaf(
      Stream stream,
      MyOctreeStorage.ChunkHeader header,
      ref bool isOldFormat,
      MyStorageDataTypeEnum dataType,
      out ulong key,
      out MyMicroOctreeLeaf contentLeaf)
    {
      MyCellCoord myCellCoord = new MyCellCoord();
      if (header.Version <= 2)
      {
        uint id = stream.ReadUInt32();
        myCellCoord.SetUnpack(id);
        key = myCellCoord.PackId64();
        header.Size -= 4;
        isOldFormat = true;
      }
      else
      {
        key = stream.ReadUInt64();
        myCellCoord.SetUnpack(key);
        header.Size -= 8;
      }
      contentLeaf = new MyMicroOctreeLeaf(dataType, 4, myCellCoord.CoordInLod << myCellCoord.Lod + 4);
      contentLeaf.ReadFrom(header, stream);
    }

    private void ReadProviderLeaf(
      Stream stream,
      MyOctreeStorage.ChunkHeader header,
      ref bool isOldFormat,
      HashSet<ulong> outKeySet)
    {
      ulong num;
      if (header.Version <= 2)
      {
        uint id = stream.ReadUInt32();
        MyCellCoord myCellCoord = new MyCellCoord();
        myCellCoord.SetUnpack(id);
        num = myCellCoord.PackId64();
        header.Size -= 4;
        isOldFormat = true;
      }
      else
      {
        num = stream.ReadUInt64();
        header.Size -= 8;
      }
      outKeySet.Add(num);
      stream.SkipBytes(header.Size);
    }

    private static void WriteDataProvider(Stream stream, IMyStorageDataProvider provider)
    {
      if (provider == null)
        return;
      new MyOctreeStorage.ChunkHeader()
      {
        ChunkType = MyOctreeStorage.ChunkTypeEnum.DataProvider,
        Version = 2,
        Size = (provider.SerializedSize + 4)
      }.WriteTo(stream);
      stream.WriteNoAlloc(MyOctreeStorage.m_attributesByType[provider.GetType()].ProviderTypeId);
      provider.WriteTo(stream);
    }

    private static void ReadDataProvider(
      Stream stream,
      MyOctreeStorage.ChunkHeader header,
      ref bool isOldFormat,
      out IMyStorageDataProvider provider)
    {
      switch (header.Version)
      {
        case 1:
          provider = (IMyStorageDataProvider) Activator.CreateInstance(MyOctreeStorage.m_attributesById[1].ProviderType);
          provider.ReadFrom(header.Version, stream, header.Size, ref isOldFormat);
          isOldFormat = true;
          break;
        case 2:
          int key = stream.ReadInt32();
          provider = (IMyStorageDataProvider) Activator.CreateInstance(MyOctreeStorage.m_attributesById[key].ProviderType);
          header.Size -= 4;
          provider.ReadFrom(header.Version, stream, header.Size, ref isOldFormat);
          break;
        default:
          throw new InvalidBranchException();
      }
    }

    public enum ChunkTypeEnum : ushort
    {
      StorageMetaData = 1,
      MaterialIndexTable = 2,
      MacroContentNodes = 3,
      MacroMaterialNodes = 4,
      ContentLeafProvider = 5,
      ContentLeafOctree = 6,
      MaterialLeafProvider = 7,
      MaterialLeafOctree = 8,
      DataProvider = 9,
      EndOfFile = 65535, // 0xFFFF
    }

    public struct ChunkHeader
    {
      public MyOctreeStorage.ChunkTypeEnum ChunkType;
      public int Version;
      public int Size;

      public void WriteTo(Stream stream)
      {
        stream.Write7BitEncodedInt((int) this.ChunkType);
        stream.Write7BitEncodedInt(this.Version);
        stream.Write7BitEncodedInt(this.Size);
      }

      public void ReadFrom(Stream stream)
      {
        this.ChunkType = (MyOctreeStorage.ChunkTypeEnum) stream.Read7BitEncodedInt();
        this.Version = stream.Read7BitEncodedInt();
        this.Size = stream.Read7BitEncodedInt();
      }
    }

    internal struct WriteRangeOps<TOperator> : MyOctreeStorage.ITraverseOps where TOperator : struct, IVoxelOperator
    {
      public TOperator Source;

      public MyOctreeStorage.ChildType Init<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        out MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps
      {
        args.Storage.AccessRange(MyStorageBase.MyAccessType.Write, args.DataType, ref coord);
        MyCellCoord myCellCoord1 = coord;
        myCellCoord1.Lod -= 4;
        ulong key1 = myCellCoord1.PackId64();
        IMyOctreeLeafNode myOctreeLeafNode1;
        if (args.Leaves.TryGetValue(key1, out myOctreeLeafNode1) && args.Storage.DataProvider != null)
        {
          myOctreeLeafNode1.Dispose();
          args.Leaves.Remove(key1);
          Vector3I vector3I = myCellCoord1.CoordInLod << 1;
          MyCellCoord myCellCoord2 = new MyCellCoord()
          {
            Lod = myCellCoord1.Lod - 1
          };
          node = new MyOctreeNode();
          for (int index = 0; index < 8; ++index)
          {
            Vector3I relativeCoord;
            MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
            myCellCoord2.CoordInLod = vector3I + relativeCoord;
            MyCellCoord cell = myCellCoord2;
            cell.Lod += 4;
            IMyOctreeLeafNode myOctreeLeafNode2 = (IMyOctreeLeafNode) new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
            args.Leaves.Add(myCellCoord2.PackId64(), myOctreeLeafNode2);
            node.SetData(index, myOctreeLeafNode2.GetFilteredValue());
          }
          node.SetChildren();
        }
        else
        {
          --myCellCoord1.Lod;
          ulong key2 = myCellCoord1.PackId64();
          if (!args.Nodes.TryGetValue(key2, out node))
          {
            node = new MyOctreeNode();
            node.SetAllData(defaultData);
          }
        }
        return MyOctreeStorage.ChildType.Node;
      }

      public MyOctreeStorage.ChildType LeafOp<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        ref MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps
      {
        int num1 = 0;
        MyCellCoord myCellCoord = new MyCellCoord();
        Vector3I vector3I1 = coord.CoordInLod << 1;
        Vector3I min = args.Min >> 4;
        Vector3I max = args.Max >> 4;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord;
          MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
          myCellCoord.CoordInLod = vector3I1 + relativeCoord;
          ulong key = myCellCoord.PackId64();
          IMyOctreeLeafNode myOctreeLeafNode1;
          if (args.Leaves.TryGetValue(key, out myOctreeLeafNode1) && myOctreeLeafNode1.ReadOnly)
            ++num1;
          if (myCellCoord.CoordInLod.IsInsideInclusiveEnd(ref min, ref max))
          {
            Vector3I result1 = myCellCoord.CoordInLod << 4;
            Vector3I result2 = result1 + 16 - 1;
            Vector3I.Max(ref result1, ref args.Min, out result1);
            Vector3I.Min(ref result2, ref args.Max, out result2);
            Vector3I readOffset = result1 - args.Min;
            Vector3I vector3I2 = result1 - (myCellCoord.CoordInLod << 4);
            Vector3I vector3I3 = result2 - (myCellCoord.CoordInLod << 4);
            byte data = node.GetData(index);
            if (myOctreeLeafNode1 == null)
            {
              MyMicroOctreeLeaf myMicroOctreeLeaf = new MyMicroOctreeLeaf(args.DataType, 4, myCellCoord.CoordInLod << myCellCoord.Lod + 4);
              myMicroOctreeLeaf.BuildFrom(data);
              myOctreeLeafNode1 = (IMyOctreeLeafNode) myMicroOctreeLeaf;
            }
            if (myOctreeLeafNode1.ReadOnly)
            {
              --num1;
              MyStorageData tempStorage = MyOctreeStorage.TempStorage;
              Vector3I maxInLod = new Vector3I(15);
              tempStorage.Resize(Vector3I.Zero, maxInLod);
              tempStorage.Clear(args.DataType, defaultData);
              if (vector3I2 != Vector3I.Zero || vector3I3 != maxInLod || this.Source.Flags != VoxelOperatorFlags.WriteAll)
              {
                MyVoxelRequestFlags voxelRequestFlags = MyVoxelRequestFlags.EmptyData;
                if (args.DataType == MyStorageDataTypeEnum.Content)
                {
                  myOctreeLeafNode1.ReadRange(tempStorage, args.DataType.ToFlags(), ref Vector3I.Zero, 0, ref Vector3I.Zero, ref maxInLod, ref voxelRequestFlags);
                }
                else
                {
                  tempStorage.Clear(MyStorageDataTypeEnum.Content, (byte) 0);
                  IMyOctreeLeafNode myOctreeLeafNode2;
                  if (args.ContentLeaves.TryGetValue(key, out myOctreeLeafNode2))
                  {
                    myOctreeLeafNode2.ReadRange(tempStorage, MyStorageDataTypeFlags.Content, ref Vector3I.Zero, 0, ref Vector3I.Zero, ref maxInLod, ref voxelRequestFlags);
                  }
                  else
                  {
                    Vector3I lodVoxelRangeMin = myCellCoord.CoordInLod << 4;
                    Vector3I lodVoxelRangeMax = lodVoxelRangeMin + 16 - 1;
                    args.Storage.ReadRangeInternal(tempStorage, ref Vector3I.Zero, MyStorageDataTypeFlags.Content, 0, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref voxelRequestFlags);
                  }
                  voxelRequestFlags = MyVoxelRequestFlags.ConsiderContent;
                  myOctreeLeafNode1.ReadRange(tempStorage, args.DataType.ToFlags(), ref Vector3I.Zero, 0, ref Vector3I.Zero, ref maxInLod, ref voxelRequestFlags);
                }
              }
              byte[] numArray = tempStorage[args.DataType];
              Vector3I next = vector3I2;
              Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref vector3I2, ref vector3I3);
              while (vector3IRangeIterator.IsValid())
              {
                Vector3I position = readOffset + (next - vector3I2);
                int linear = tempStorage.ComputeLinear(ref next);
                this.Source.Op(ref position, args.DataType, ref numArray[linear]);
                vector3IRangeIterator.GetNext(out next);
              }
              MyMicroOctreeLeaf myMicroOctreeLeaf = new MyMicroOctreeLeaf(args.DataType, 4, myCellCoord.CoordInLod << myCellCoord.Lod + 4);
              myMicroOctreeLeaf.BuildFrom(tempStorage);
              myOctreeLeafNode1 = (IMyOctreeLeafNode) myMicroOctreeLeaf;
            }
            else
              myOctreeLeafNode1.ExecuteOperation<TOperator>(ref this.Source, ref readOffset, ref vector3I2, ref vector3I3);
            int num2 = myOctreeLeafNode1.TryGetUniformValue(out byte _) ? 1 : 0;
            node.SetData(index, myOctreeLeafNode1.GetFilteredValue());
            if (num2 == 0)
            {
              args.Leaves[key] = myOctreeLeafNode1;
              node.SetChild(index, true);
            }
            else
            {
              myOctreeLeafNode1?.Dispose();
              args.Leaves.Remove(key);
              node.SetChild(index, false);
            }
          }
        }
        return num1 == 8 ? MyOctreeStorage.ChildType.NodeWithLeafReadonly : MyOctreeStorage.ChildType.Node;
      }
    }

    private struct DeleteRangeOps : MyOctreeStorage.ITraverseOps
    {
      public MyOctreeStorage Storage;

      public MyOctreeStorage.ChildType Init<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        out MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps
      {
        node = new MyOctreeNode();
        MyCellCoord myCellCoord = coord;
        myCellCoord.Lod -= 4;
        ulong key1 = myCellCoord.PackId64();
        IMyOctreeLeafNode myOctreeLeafNode;
        if (args.Leaves.TryGetValue(key1, out myOctreeLeafNode) && myCellCoord.Lod > 0)
          return !myOctreeLeafNode.ReadOnly ? MyOctreeStorage.ChildType.Leaf : MyOctreeStorage.ChildType.LeafReadonly;
        --myCellCoord.Lod;
        ulong key2 = myCellCoord.PackId64();
        return !args.Nodes.TryGetValue(key2, out node) ? MyOctreeStorage.ChildType.NodeMissing : MyOctreeStorage.ChildType.Node;
      }

      public MyOctreeStorage.ChildType LeafOp<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        ref MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        Vector3I vector3I = coord.CoordInLod << 1;
        Vector3I inclusiveMin = args.Min >> 4;
        Vector3I exclusiveMax = args.Max >> 4;
        int num = 0;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord;
          MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
          myCellCoord.CoordInLod = vector3I + relativeCoord;
          ulong key = myCellCoord.PackId64();
          if (myCellCoord.CoordInLod.IsInside(ref inclusiveMin, ref exclusiveMax))
          {
            if (node.HasChild(index))
            {
              IMyOctreeLeafNode myOctreeLeafNode;
              if (!args.Leaves.TryGetValue(key, out myOctreeLeafNode))
              {
                ++num;
                continue;
              }
              if (myOctreeLeafNode.ReadOnly)
              {
                ++num;
                continue;
              }
              myOctreeLeafNode.Dispose();
            }
            MyCellCoord cell = myCellCoord;
            cell.Lod += 4;
            IMyOctreeLeafNode myOctreeLeafNode1 = (IMyOctreeLeafNode) new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
            args.Leaves[key] = myOctreeLeafNode1;
            node.SetData(index, myOctreeLeafNode1.GetFilteredValue());
            node.SetChild(index, true);
            ++num;
          }
        }
        return num == 8 ? MyOctreeStorage.ChildType.NodeWithLeafReadonly : MyOctreeStorage.ChildType.Node;
      }
    }

    private struct SweepRangeOps : MyOctreeStorage.ITraverseOps
    {
      public MyOctreeStorage Storage;

      public MyOctreeStorage.ChildType Init<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        out MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps
      {
        node = new MyOctreeNode();
        MyCellCoord myCellCoord = coord;
        myCellCoord.Lod -= 4;
        ulong key1 = myCellCoord.PackId64();
        if (args.Leaves.TryGetValue(key1, out IMyOctreeLeafNode _) && args.Storage.DataProvider != null)
          return MyOctreeStorage.ChildType.LeafReadonly;
        --myCellCoord.Lod;
        ulong key2 = myCellCoord.PackId64();
        if (args.Nodes.TryGetValue(key2, out node))
          return MyOctreeStorage.ChildType.Node;
        MyCellCoord cell = myCellCoord;
        ++cell.Lod;
        MyProviderLeaf myProviderLeaf = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
        byte uniformValue;
        if (!myProviderLeaf.TryGetUniformValue(out uniformValue) || (int) defaultData != (int) uniformValue)
          return MyOctreeStorage.ChildType.NodeMissing;
        args.Leaves[key1] = (IMyOctreeLeafNode) myProviderLeaf;
        return MyOctreeStorage.ChildType.LeafReadonly;
      }

      public MyOctreeStorage.ChildType LeafOp<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        ref MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        Vector3I vector3I = coord.CoordInLod << 1;
        int num = 0;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord;
          MyOctreeStorage.ComputeChildCoord(index, out relativeCoord);
          myCellCoord.CoordInLod = vector3I + relativeCoord;
          ulong key = myCellCoord.PackId64();
          if (!node.HasChild(index))
          {
            MyCellCoord cell = myCellCoord;
            cell.Lod += 4;
            MyProviderLeaf myProviderLeaf = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
            byte uniformValue;
            if (myProviderLeaf.TryGetUniformValue(out uniformValue) && (int) node.GetData(index) == (int) uniformValue)
            {
              args.Leaves[key] = (IMyOctreeLeafNode) myProviderLeaf;
              ++num;
            }
          }
          else
          {
            IMyOctreeLeafNode myOctreeLeafNode;
            if (args.Leaves.TryGetValue(key, out myOctreeLeafNode) && myOctreeLeafNode.ReadOnly)
              ++num;
          }
        }
        return num == 8 ? MyOctreeStorage.ChildType.NodeWithLeafReadonly : MyOctreeStorage.ChildType.Node;
      }
    }

    internal enum ChildType
    {
      NodeMissing,
      NodeEmpty,
      Node,
      LeafReadonly,
      NodeWithLeafReadonly,
      Leaf,
    }

    internal interface ITraverseOps
    {
      MyOctreeStorage.ChildType Init<THis>(
        ref MyOctreeStorage.TraverseArgs<THis> args,
        ref MyCellCoord coord,
        byte defaultData,
        out MyOctreeNode node)
        where THis : struct, MyOctreeStorage.ITraverseOps;

      MyOctreeStorage.ChildType LeafOp<TThis>(
        ref MyOctreeStorage.TraverseArgs<TThis> args,
        ref MyCellCoord coord,
        byte defaultData,
        ref MyOctreeNode node)
        where TThis : struct, MyOctreeStorage.ITraverseOps;
    }

    internal struct TraverseArgs<TOperator> where TOperator : struct, MyOctreeStorage.ITraverseOps
    {
      public TOperator Operator;
      public MyOctreeStorage Storage;
      public Dictionary<ulong, MyOctreeNode> Nodes;
      public Dictionary<ulong, IMyOctreeLeafNode> Leaves;
      public Dictionary<ulong, IMyOctreeLeafNode> ContentLeaves;
      public MyOctreeNode.FilterFunction DataFilter;
      public MyStorageDataTypeEnum DataType;
      public Vector3I Min;
      public Vector3I Max;
    }
  }
}
