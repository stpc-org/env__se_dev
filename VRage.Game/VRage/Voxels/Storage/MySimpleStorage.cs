// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Storage.MySimpleStorage
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Threading;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Voxels.Storage
{
  public class MySimpleStorage : VRage.Game.Voxels.IMyStorage, VRage.ModAPI.IMyStorage
  {
    private static int m_storegeIds;
    private MyStorageData m_data;

    public MyStorageData Data => this.m_data;

    public bool AreDataCached => true;

    public bool AreDataCachedCompressed => false;

    public MySimpleStorage(int size)
    {
      this.Size = new Vector3I(size);
      this.m_data = new MyStorageData();
      this.m_data.Resize(this.Size);
      this.StorageId = (uint) Interlocked.Increment(ref MySimpleStorage.m_storegeIds);
    }

    public ContainmentType Intersect(ref BoundingBox box, bool lazy) => ContainmentType.Disjoint;

    public bool MarkedForClose => false;

    public void PinAndExecute(Action action)
    {
    }

    public void PinAndExecute(Action<VRage.ModAPI.IMyStorage> action)
    {
    }

    public void Reset(MyStorageDataTypeFlags dataToReset)
    {
    }

    public unsafe void Save(out byte[] outCompressedData)
    {
      outCompressedData = new byte[this.m_data.SizeLinear * 2 + 4];
      int num1 = 0;
      fixed (byte* numPtr1 = outCompressedData)
      {
        *(int*) numPtr1 = this.m_data.Size3D.X;
        int num2 = num1 + 4;
        for (int linearIdx = 0; linearIdx < this.m_data.SizeLinear; ++linearIdx)
        {
          byte* numPtr2 = numPtr1;
          int index1 = num2;
          int num3 = index1 + 1;
          numPtr2[index1] = this.m_data.Content(linearIdx);
          byte* numPtr3 = numPtr1;
          int index2 = num3;
          num2 = index2 + 1;
          numPtr3[index2] = this.m_data.Material(linearIdx);
        }
      }
    }

    public byte[] GetVoxelData()
    {
      byte[] outCompressedData;
      this.Save(out outCompressedData);
      return outCompressedData;
    }

    public Vector3I Size { get; private set; }

    public void OverwriteAllMaterials(byte materialIndex)
    {
    }

    public uint StorageId { get; private set; }

    public void Close() => this.Closed = true;

    public bool Shared { get; private set; }

    public VRage.Game.Voxels.IMyStorage Copy() => throw new NotImplementedException();

    public StoragePin Pin() => new StoragePin((VRage.Game.Voxels.IMyStorage) this);

    public void Unpin()
    {
    }

    public bool Closed { get; private set; }

    public void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax)
    {
      MyVoxelRequestFlags requestFlags = (MyVoxelRequestFlags) 0;
      this.ReadRange(target, dataToRead, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
    }

    public void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax,
      ref MyVoxelRequestFlags requestFlags)
    {
      lodVoxelRangeMax = Vector3I.Min(lodVoxelRangeMax, (this.Size >> lodIndex) - 1);
      if ((dataToRead & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
        target.CopyRange(this.m_data, lodVoxelRangeMin, lodVoxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Content);
      if ((dataToRead & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      target.CopyRange(this.m_data, lodVoxelRangeMin, lodVoxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Material);
    }

    public void WriteRange(
      MyStorageData source,
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify,
      bool skipCache)
    {
      if ((dataToWrite & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
        this.m_data.CopyRange(source, voxelRangeMin, voxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Content);
      if ((dataToWrite & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      this.m_data.CopyRange(source, voxelRangeMin, voxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Material);
    }

    public bool DeleteSupported => false;

    public void DeleteRange(
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify)
    {
    }

    public void ExecuteOperationFast<TVoxelOperator>(
      ref TVoxelOperator voxelOperator,
      MyStorageDataTypeFlags dataToWrite,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      bool notifyRangeChanged)
      where TVoxelOperator : struct, IVoxelOperator
    {
      if ((dataToWrite & MyStorageDataTypeFlags.Content) != MyStorageDataTypeFlags.None)
        this.ExecuteOperationInternal<TVoxelOperator>(ref voxelOperator, MyStorageDataTypeEnum.Content, ref voxelRangeMin, ref voxelRangeMax);
      if ((dataToWrite & MyStorageDataTypeFlags.Material) == MyStorageDataTypeFlags.None)
        return;
      this.ExecuteOperationInternal<TVoxelOperator>(ref voxelOperator, MyStorageDataTypeEnum.Material, ref voxelRangeMin, ref voxelRangeMax);
    }

    private void ExecuteOperationInternal<TVoxelOperator>(
      ref TVoxelOperator voxelOperator,
      MyStorageDataTypeEnum dataType,
      ref Vector3I min,
      ref Vector3I max)
      where TVoxelOperator : struct, IVoxelOperator
    {
      Vector3I step = this.m_data.Step;
      byte[] numArray = this.m_data[dataType];
      Vector3I position;
      position.Z = min.Z;
      int num1 = 0;
      while (position.Z <= max.Z)
      {
        position.Y = min.Y;
        int num2 = 0;
        while (position.Y <= max.Y)
        {
          int num3 = num2 + num1;
          position.X = min.X;
          int num4 = 0;
          while (position.X <= max.X)
          {
            voxelOperator.Op(ref position, dataType, ref numArray[num4 + num3]);
            ++position.X;
            num4 += step.X;
          }
          ++position.Y;
          num2 += step.Y;
        }
        ++position.Z;
        num1 += step.Z;
      }
    }

    public void NotifyRangeChanged(
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      MyStorageDataTypeFlags dataChanged)
    {
    }

    public ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true)
    {
      return ContainmentType.Intersects;
    }

    public bool Intersect(ref LineD line) => true;

    public event Action<Vector3I, Vector3I, MyStorageDataTypeFlags> RangeChanged;

    public void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode)
    {
    }

    public void SetDataCache(byte[] data, bool compressed)
    {
    }

    public IMyStorageDataProvider DataProvider => (IMyStorageDataProvider) null;

    public void NotifyChanged(
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      MyStorageDataTypeFlags changedData)
    {
    }
  }
}
