// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyStorageData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace VRage.Voxels
{
  [Serializable]
  public class MyStorageData
  {
    private byte[][] m_dataByType;
    private int m_sZ;
    private int m_sY;
    private Vector3I m_size3d;
    private int m_sizeLinear;
    private int m_dataSizeLinear = -1;
    private MyStorageDataTypeFlags m_storedTypes;

    public byte[] this[MyStorageDataTypeEnum type]
    {
      get => this.m_dataByType[(int) type];
      set
      {
        if (this.m_dataSizeLinear == -1)
          this.m_dataSizeLinear = value.Length;
        this.m_dataByType[(int) type] = value;
      }
    }

    public int SizeLinear => this.m_sizeLinear;

    public int StepLinear => 1;

    public int StepX => 1;

    public int StepY => this.m_sY;

    public int StepZ => this.m_sZ;

    public Vector3I Step => new Vector3I(1, this.m_sY, this.m_sZ);

    public Vector3I Size3D => this.m_size3d;

    public MyStorageData()
    {
      this.m_storedTypes = MyStorageDataTypeFlags.ContentAndMaterial;
      this.m_dataByType = new byte[2][];
    }

    public MyStorageData(MyStorageDataTypeFlags typesToStore)
    {
      this.m_storedTypes = typesToStore;
      this.m_dataByType = new byte[2][];
    }

    public MyStorageData(Vector3I size, byte[] content = null, byte[] material = null)
    {
      this.m_dataByType = new byte[2][];
      this.Resize(size);
      if (content != null)
      {
        this.m_storedTypes |= MyStorageDataTypeFlags.Content;
        this[MyStorageDataTypeEnum.Content] = content;
      }
      if (material == null)
        return;
      this.m_storedTypes |= MyStorageDataTypeFlags.Material;
      this[MyStorageDataTypeEnum.Material] = material;
    }

    public void Resize(Vector3I start, Vector3I end) => this.Resize(end - start + 1);

    public void Resize(Vector3I size3D)
    {
      this.m_size3d = size3D;
      int size = size3D.Size;
      this.m_sY = size3D.X;
      this.m_sZ = size3D.Y * this.m_sY;
      this.m_sizeLinear = size * this.StepLinear;
      for (int index = 0; index < this.m_dataByType.Length; ++index)
      {
        if ((this.m_dataByType[index] == null || this.m_dataByType[index].Length < this.m_sizeLinear) && this.m_storedTypes.Requests((MyStorageDataTypeEnum) index))
          this.m_dataByType[index] = new byte[this.m_sizeLinear];
      }
    }

    public byte Get(MyStorageDataTypeEnum type, ref Vector3I p) => this[type][p.X + p.Y * this.m_sY + p.Z * this.m_sZ];

    public byte Get(MyStorageDataTypeEnum type, int linearIdx) => this[type][linearIdx];

    public byte Get(MyStorageDataTypeEnum type, int x, int y, int z) => this[type][x + y * this.m_sY + z * this.m_sZ];

    public void Set(MyStorageDataTypeEnum type, ref Vector3I p, byte value) => this[type][p.X + p.Y * this.m_sY + p.Z * this.m_sZ] = value;

    public void Content(ref Vector3I p, byte content) => this[MyStorageDataTypeEnum.Content][p.X + p.Y * this.m_sY + p.Z * this.m_sZ] = content;

    public void Content(int linearIdx, byte content) => this[MyStorageDataTypeEnum.Content][linearIdx] = content;

    public byte Content(ref Vector3I p) => this[MyStorageDataTypeEnum.Content][p.X + p.Y * this.m_sY + p.Z * this.m_sZ];

    public byte Content(int x, int y, int z) => this[MyStorageDataTypeEnum.Content][x + y * this.m_sY + z * this.m_sZ];

    public byte Content(int linearIdx) => this[MyStorageDataTypeEnum.Content][linearIdx];

    public void Material(ref Vector3I p, byte materialIdx) => this[MyStorageDataTypeEnum.Material][p.X + p.Y * this.m_sY + p.Z * this.m_sZ] = materialIdx;

    public byte Material(ref Vector3I p) => this[MyStorageDataTypeEnum.Material][p.X + p.Y * this.m_sY + p.Z * this.m_sZ];

    public byte Material(int linearIdx) => this[MyStorageDataTypeEnum.Material][linearIdx];

    public void Material(int linearIdx, byte materialIdx) => this[MyStorageDataTypeEnum.Material][linearIdx] = materialIdx;

    public int ComputeLinear(ref Vector3I p) => p.X + p.Y * this.m_sY + p.Z * this.m_sZ;

    public void ComputePosition(int linear, out Vector3I p)
    {
      int x = linear % this.m_sY;
      int y = (linear - x) % this.m_sZ / this.m_sY;
      int z = (linear - x - y * this.m_sY) / this.m_sZ;
      p = new Vector3I(x, y, z);
    }

    public bool WrinkleVoxelContent(
      ref Vector3I p,
      float wrinkleWeightAdd,
      float wrinkleWeightRemove)
    {
      int num1 = int.MinValue;
      int num2 = int.MaxValue;
      int num3 = (int) ((double) wrinkleWeightAdd * (double) byte.MaxValue);
      int num4 = (int) ((double) wrinkleWeightRemove * (double) byte.MaxValue);
      for (int index1 = -1; index1 <= 1; ++index1)
      {
        Vector3I p1;
        p1.Z = index1 + p.Z;
        if ((uint) p1.Z < (uint) this.m_size3d.Z)
        {
          for (int index2 = -1; index2 <= 1; ++index2)
          {
            p1.Y = index2 + p.Y;
            if ((uint) p1.Y < (uint) this.m_size3d.Y)
            {
              for (int index3 = -1; index3 <= 1; ++index3)
              {
                p1.X = index3 + p.X;
                if ((uint) p1.X < (uint) this.m_size3d.X)
                {
                  byte num5 = this.Content(ref p1);
                  num1 = Math.Max(num1, (int) num5);
                  num2 = Math.Min(num2, (int) num5);
                }
              }
            }
          }
        }
      }
      if (num2 == num1)
        return false;
      int num6 = (int) this.Content(ref p);
      byte clampInt = (byte) MyUtils.GetClampInt(num6 + MyUtils.GetRandomInt(num3 + num4) - num4, num2, num1);
      if ((int) clampInt == num6)
        return false;
      this.Content(ref p, clampInt);
      return true;
    }

    public unsafe void BlockFill(
      MyStorageDataTypeEnum type,
      Vector3I min,
      Vector3I max,
      byte content)
    {
      min.Z *= this.m_sZ;
      max.Z *= this.m_sZ;
      min.Y *= this.m_sY;
      max.Y *= this.m_sY;
      ref int local1 = ref min.X;
      local1 = local1;
      ref int local2 = ref max.X;
      local2 = local2;
      fixed (byte* numPtr = &this[type][0])
      {
        Vector3I vector3I;
        for (vector3I.Z = min.Z; vector3I.Z <= max.Z; vector3I.Z += this.m_sZ)
        {
          int z = vector3I.Z;
          for (vector3I.Y = min.Y; vector3I.Y <= max.Y; vector3I.Y += this.m_sY)
          {
            int num = z + vector3I.Y;
            for (vector3I.X = min.X; vector3I.X <= max.X; ++vector3I.X)
              numPtr[vector3I.X + num] = content;
          }
        }
      }
    }

    public void BlockFillContent(Vector3I min, Vector3I max, byte content) => this.BlockFill(MyStorageDataTypeEnum.Content, min, max, content);

    public unsafe void BlockFillMaterialConsiderContent(
      Vector3I min,
      Vector3I max,
      byte materialIdx)
    {
      min.Z *= this.m_sZ;
      max.Z *= this.m_sZ;
      min.Y *= this.m_sY;
      max.Y *= this.m_sY;
      ref int local1 = ref min.X;
      local1 = local1;
      ref int local2 = ref max.X;
      local2 = local2;
      fixed (byte* numPtr1 = &this[MyStorageDataTypeEnum.Content][0])
        fixed (byte* numPtr2 = &this[MyStorageDataTypeEnum.Material][0])
        {
          Vector3I vector3I;
          for (vector3I.Z = min.Z; vector3I.Z <= max.Z; vector3I.Z += this.m_sZ)
          {
            int z = vector3I.Z;
            for (vector3I.Y = min.Y; vector3I.Y <= max.Y; vector3I.Y += this.m_sY)
            {
              int num = z + vector3I.Y;
              for (vector3I.X = min.X; vector3I.X <= max.X; ++vector3I.X)
                numPtr2[vector3I.X + num] = numPtr1[vector3I.X + num] != (byte) 0 ? materialIdx : byte.MaxValue;
            }
          }
        }
    }

    public void BlockFillMaterial(Vector3I min, Vector3I max, byte materialIdx) => this.BlockFill(MyStorageDataTypeEnum.Material, min, max, materialIdx);

    public void CopyRange(
      MyStorageData src,
      Vector3I min,
      Vector3I max,
      Vector3I offset,
      MyStorageDataTypeEnum dataType)
    {
      this.OpRange<MyStorageData.CopyOperator>(src, min, max, offset, dataType);
    }

    public void OpRange<Op>(
      MyStorageData src,
      Vector3I min,
      Vector3I max,
      Vector3I offset,
      MyStorageDataTypeEnum dataType)
      where Op : struct, MyStorageData.IOperator
    {
      byte[] numArray1 = this[dataType];
      Vector3I step1 = this.Step;
      Vector3I step2 = src.Step;
      byte[] numArray2 = src[dataType];
      min *= step2;
      max *= step2;
      offset *= step1;
      Op op = default (Op);
      int z1 = min.Z;
      int x1 = offset.X;
      while (z1 <= max.Z)
      {
        int y1 = min.Y;
        int y2 = offset.Y;
        while (y1 <= max.Y)
        {
          int num1 = y1 + z1;
          int num2 = y2 + x1;
          int x2 = min.X;
          int z2 = offset.Z;
          while (x2 <= max.X)
          {
            op.Op(ref numArray1[z2 + num2], numArray2[x2 + num1]);
            x2 += step2.X;
            z2 += step1.X;
          }
          y1 += step2.Y;
          y2 += step1.Y;
        }
        z1 += step2.Z;
        x1 += step1.Z;
      }
    }

    public bool ContainsIsoSurface()
    {
      try
      {
        byte[] numArray = this[MyStorageDataTypeEnum.Content];
        bool flag1 = numArray[0] < (byte) 127;
        for (int index = 1; index < this.m_sizeLinear; index += this.StepLinear)
        {
          bool flag2 = numArray[index] < (byte) 127;
          if (flag1 != flag2)
            return true;
        }
        return false;
      }
      finally
      {
      }
    }

    public MyVoxelContentConstitution ComputeContentConstitution()
    {
      try
      {
        byte[] numArray = this[MyStorageDataTypeEnum.Content];
        bool flag1 = numArray[0] < (byte) 127;
        for (int index = 1; index < this.m_sizeLinear; index += this.StepLinear)
        {
          bool flag2 = numArray[index] < (byte) 127;
          if (flag1 != flag2)
            return MyVoxelContentConstitution.Mixed;
        }
        return flag1 ? MyVoxelContentConstitution.Empty : MyVoxelContentConstitution.Full;
      }
      finally
      {
      }
    }

    public bool ContainsVoxelsAboveIsoLevel()
    {
      byte[] numArray = this[MyStorageDataTypeEnum.Content];
      try
      {
        for (int index = 0; index < this.m_sizeLinear; index += this.StepLinear)
        {
          if (numArray[index] > (byte) 127)
            return true;
        }
        return false;
      }
      finally
      {
      }
    }

    public int ValueWhenAllEqual(MyStorageDataTypeEnum dataType)
    {
      byte[] numArray = this[dataType];
      byte num = numArray[0];
      for (int index = 1; index < this.m_sizeLinear; index += this.StepLinear)
      {
        if ((int) num != (int) numArray[index])
          return -1;
      }
      return (int) num;
    }

    [Conditional("DEBUG")]
    private void AssertPosition(ref Vector3I position)
    {
    }

    [Conditional("DEBUG")]
    private void AssertPosition(int x, int y, int z)
    {
    }

    public void ClearContent(byte p) => this.Clear(MyStorageDataTypeEnum.Content, p);

    public void ClearMaterials(byte p) => this.Clear(MyStorageDataTypeEnum.Material, p);

    public unsafe void Clear(MyStorageDataTypeEnum type, byte p)
    {
      fixed (byte* numPtr = this[type])
      {
        for (int index = 0; index < this.m_sizeLinear; ++index)
          numPtr[index] = p;
      }
    }

    public string ToBase64()
    {
      MemoryStream memoryStream = new MemoryStream();
      new BinaryFormatter().Serialize((Stream) memoryStream, (object) this);
      return Convert.ToBase64String(memoryStream.GetBuffer());
    }

    public static MyStorageData FromBase64(string str) => (MyStorageData) new BinaryFormatter().Deserialize((Stream) new MemoryStream(Convert.FromBase64String(str)));

    public interface IOperator
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      void Op(ref byte target, byte source);
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CopyOperator : MyStorageData.IOperator
    {
      public void Op(ref byte target, byte source) => target = source;
    }

    public struct MortonEnumerator : IEnumerator<byte>, IEnumerator, IDisposable
    {
      private MyStorageDataTypeEnum m_type;
      private MyStorageData m_source;
      private int m_maxMortonCode;
      private int m_mortonCode;
      private Vector3I m_pos;
      private byte m_current;

      public MortonEnumerator(MyStorageData source, MyStorageDataTypeEnum type)
      {
        this.m_type = type;
        this.m_source = source;
        this.m_maxMortonCode = source.Size3D.Size;
        this.m_mortonCode = -1;
        this.m_pos = new Vector3I();
        this.m_current = (byte) 0;
      }

      public byte Current => this.m_current;

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this.m_current;

      public bool MoveNext()
      {
        ++this.m_mortonCode;
        if (this.m_mortonCode >= this.m_maxMortonCode)
          return false;
        MyMortonCode3D.Decode(this.m_mortonCode, out this.m_pos);
        this.m_current = this.m_source.Get(this.m_type, ref this.m_pos);
        return true;
      }

      public void Reset()
      {
        this.m_mortonCode = -1;
        this.m_current = (byte) 0;
      }
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_dataByType\u003C\u003EAccessor : IMemberAccessor<MyStorageData, byte[][]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in byte[][] value) => owner.m_dataByType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out byte[][] value) => value = owner.m_dataByType;
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_sZ\u003C\u003EAccessor : IMemberAccessor<MyStorageData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in int value) => owner.m_sZ = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out int value) => value = owner.m_sZ;
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_sY\u003C\u003EAccessor : IMemberAccessor<MyStorageData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in int value) => owner.m_sY = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out int value) => value = owner.m_sY;
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_size3d\u003C\u003EAccessor : IMemberAccessor<MyStorageData, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in Vector3I value) => owner.m_size3d = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out Vector3I value) => value = owner.m_size3d;
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_sizeLinear\u003C\u003EAccessor : IMemberAccessor<MyStorageData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in int value) => owner.m_sizeLinear = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out int value) => value = owner.m_sizeLinear;
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_dataSizeLinear\u003C\u003EAccessor : IMemberAccessor<MyStorageData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in int value) => owner.m_dataSizeLinear = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out int value) => value = owner.m_dataSizeLinear;
    }

    protected class VRage_Voxels_MyStorageData\u003C\u003Em_storedTypes\u003C\u003EAccessor : IMemberAccessor<MyStorageData, MyStorageDataTypeFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStorageData owner, in MyStorageDataTypeFlags value) => owner.m_storedTypes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStorageData owner, out MyStorageDataTypeFlags value) => value = owner.m_storedTypes;
    }
  }
}
