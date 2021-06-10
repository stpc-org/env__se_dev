// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyFourEdgeInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyFourEdgeInfo
  {
    public const int MaxInfoCount = 4;
    private MyFourEdgeInfo.Data m_data;
    public int Timestamp;

    public Vector4 LocalOrthoMatrix => this.m_data.LocalOrthoMatrix;

    public MyCubeEdgeType EdgeType => this.m_data.EdgeType;

    public bool Empty => this.m_data.Empty;

    public bool Full => this.m_data.Full;

    public int DebugCount => this.m_data.Count;

    public int FirstAvailable => this.m_data.FirstAvailable;

    public MyFourEdgeInfo(Vector4 localOrthoMatrix, MyCubeEdgeType edgeType)
    {
      this.m_data.LocalOrthoMatrix = localOrthoMatrix;
      this.m_data.EdgeType = edgeType;
    }

    public bool AddInstance(
      Vector3 blockPos,
      Color color,
      MyStringHash skinSubtype,
      MyStringHash edgeModel,
      Base27Directions.Direction normal0,
      Base27Directions.Direction normal1)
    {
      if (!this.m_data.Set(this.GetIndex(ref blockPos), color, skinSubtype, edgeModel, normal0, normal1))
        return false;
      ++this.Timestamp;
      return true;
    }

    public bool RemoveInstance(Vector3 blockPos)
    {
      if (!this.m_data.Reset(this.GetIndex(ref blockPos)))
        return false;
      ++this.Timestamp;
      return true;
    }

    private int GetIndex(ref Vector3 blockPos)
    {
      Vector3 vector3 = blockPos - new Vector3(this.LocalOrthoMatrix);
      if ((double) Math.Abs(vector3.X) < 9.99999974737875E-06)
        return ((double) vector3.Y > 0.0 ? 1 : 0) + ((double) vector3.Z > 0.0 ? 2 : 0);
      return (double) Math.Abs(vector3.Y) < 9.99999974737875E-06 ? ((double) vector3.X > 0.0 ? 1 : 0) + ((double) vector3.Z > 0.0 ? 2 : 0) : ((double) vector3.X > 0.0 ? 1 : 0) + ((double) vector3.Y > 0.0 ? 2 : 0);
    }

    public bool GetNormalInfo(
      int index,
      out Color color,
      out MyStringHash skinSubtypeId,
      out MyStringHash edgeModel,
      out Base27Directions.Direction normal0,
      out Base27Directions.Direction normal1)
    {
      this.m_data.Get(index, out color, out skinSubtypeId, out edgeModel, out normal0, out normal1);
      color.A = (byte) 0;
      return (uint) normal0 > 0U;
    }

    private struct Data
    {
      public Vector4 LocalOrthoMatrix;
      public MyCubeEdgeType EdgeType;
      private unsafe fixed uint m_data[4];
      private unsafe fixed byte m_data2[4];
      private MyStringHash m_edgeModel1;
      private MyStringHash m_edgeModel2;
      private MyStringHash m_edgeModel3;
      private MyStringHash m_edgeModel4;
      private MyStringHash m_skinSubtype1;
      private MyStringHash m_skinSubtype2;
      private MyStringHash m_skinSubtype3;
      private MyStringHash m_skinSubtype4;

      public unsafe bool Full => this.m_data.FixedElementField != 0U && this.m_data[1] != 0U && this.m_data[2] != 0U && this.m_data[3] > 0U;

      public unsafe bool Empty
      {
        get
        {
          fixed (uint* numPtr = this.m_data)
            return *(ulong*) numPtr == 0UL && ((ulong*) numPtr)[1] == 0UL;
        }
      }

      public unsafe int Count => (this.m_data.FixedElementField != 0U ? 1 : 0) + (this.m_data[1] != 0U ? 1 : 0) + (this.m_data[2] != 0U ? 1 : 0) + (this.m_data[3] != 0U ? 1 : 0);

      public unsafe int FirstAvailable
      {
        get
        {
          if (this.m_data.FixedElementField != 0U)
            return 0;
          if (this.m_data[1] != 0U)
            return 1;
          if (this.m_data[2] != 0U)
            return 2;
          return this.m_data[3] == 0U ? -1 : 3;
        }
      }

      public unsafe uint Get(int index) => this.m_data[index];

      public unsafe void Get(
        int index,
        out Color color,
        out MyStringHash skinSubtypeId,
        out MyStringHash edgeModel,
        out Base27Directions.Direction normal0,
        out Base27Directions.Direction normal1)
      {
        color = new Color(this.m_data[index]);
        normal0 = (Base27Directions.Direction) color.A;
        normal1 = (Base27Directions.Direction) this.m_data2[index];
        fixed (MyStringHash* myStringHashPtr = &this.m_edgeModel1)
          edgeModel = myStringHashPtr[index];
        fixed (MyStringHash* myStringHashPtr = &this.m_skinSubtype1)
          skinSubtypeId = myStringHashPtr[index];
      }

      public unsafe bool Set(
        int index,
        Color value,
        MyStringHash skinSubtype,
        MyStringHash edgeModel,
        Base27Directions.Direction normal0,
        Base27Directions.Direction normal1)
      {
        value.A = (byte) normal0;
        uint packedValue = value.PackedValue;
        bool flag = false;
        if ((int) this.m_data[index] != (int) packedValue)
        {
          flag = true;
          this.m_data[index] = packedValue;
        }
        this.m_data2[index] = (byte) normal1;
        fixed (MyStringHash* myStringHashPtr = &this.m_edgeModel1)
          myStringHashPtr[index] = edgeModel;
        fixed (MyStringHash* myStringHashPtr = &this.m_skinSubtype1)
        {
          if (myStringHashPtr[index] != skinSubtype)
          {
            flag = true;
            myStringHashPtr[index] = skinSubtype;
          }
        }
        return flag;
      }

      public unsafe bool Reset(int index)
      {
        int num = this.m_data[index] > 0U ? 1 : 0;
        this.m_data[index] = 0U;
        fixed (MyStringHash* myStringHashPtr = &this.m_edgeModel1)
          myStringHashPtr[index] = MyStringHash.NullOrEmpty;
        fixed (MyStringHash* myStringHashPtr = &this.m_skinSubtype1)
          myStringHashPtr[index] = MyStringHash.NullOrEmpty;
        return num != 0;
      }
    }
  }
}
