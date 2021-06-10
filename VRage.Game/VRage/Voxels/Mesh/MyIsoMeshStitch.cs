// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Mesh.MyIsoMeshStitch
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Library.Threading;
using VRageMath;

namespace VRage.Voxels.Mesh
{
  public class MyIsoMeshStitch
  {
    public readonly MyIsoMesh Mesh;
    private Dictionary<Vector3I, ushort> m_edgeIndex;
    private SpinLock m_lock;
    private readonly MyStorageData[] m_signField = new MyStorageData[6];
    private byte[] m_signFieldCache;
    private int m_signFieldSize;
    private Vector3I m_forwardLimit;
    private readonly int m_originlaVxCnt;
    private readonly int m_originalTriangleCnt;

    public MyIsoMeshStitch(MyIsoMesh mesh, MyStorageData meshContent)
    {
      this.Mesh = mesh;
      this.SliceSignField(meshContent);
      this.m_originlaVxCnt = mesh.VerticesCount;
      this.m_originalTriangleCnt = mesh.TrianglesCount;
    }

    public bool TryGetVertex(Vector3I coord, out ushort index)
    {
      if (this.m_edgeIndex != null)
        return this.m_edgeIndex.TryGetValue(coord, out index);
      index = (ushort) 0;
      return false;
    }

    public void IndexEdges()
    {
      lock (this)
      {
        if (this.m_edgeIndex != null)
          return;
        this.m_edgeIndex = new Dictionary<Vector3I, ushort>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
        for (int index = 0; index < this.Mesh.Cells.Count; ++index)
        {
          Vector3I cell = this.Mesh.Cells[index];
          if (this.IsEdge(ref cell))
            this.m_edgeIndex[cell] = (ushort) index;
        }
      }
    }

    internal void AddVertexToIndex(ushort vx)
    {
    }

    public bool IsEdge(ref Vector3I edge)
    {
      Vector3I vector3I = this.Mesh.CellEnd - this.Mesh.CellStart - 1;
      return edge.X == 0 || edge.X == vector3I.X || (edge.Y == 0 || edge.Y == vector3I.Y) || edge.Z == 0 || edge.Z == vector3I.Z;
    }

    internal ushort WriteVertex(
      ref Vector3I cell,
      ref Vector3 pos,
      ref Vector3 normal,
      byte material,
      uint colorShift)
    {
      this.m_lock.Enter();
      try
      {
        return (ushort) this.Mesh.WriteVertex(ref cell, ref pos, ref normal, material, colorShift);
      }
      finally
      {
        this.m_lock.Exit();
      }
    }

    internal void WriteTriangle(ushort v2, ushort v1, ushort v0)
    {
      this.m_lock.Enter();
      try
      {
        this.Mesh.WriteTriangle((int) v2, (int) v1, (int) v0);
      }
      finally
      {
        this.m_lock.Exit();
      }
    }

    private void SliceSignField(MyStorageData field)
    {
      Vector3I vector3I = field.Size3D - 1;
      this.m_forwardLimit = vector3I - 1;
      this.m_signFieldSize = vector3I.X;
      int num = vector3I.X - 2;
      this.m_signFieldCache = new byte[this.m_signFieldSize * this.m_signFieldSize * 3 + this.m_signFieldSize * num * 3 + num * num * 3];
      for (int index = 0; index < 6; ++index)
        this.m_signField[index] = new MyStorageData();
      this.ExtractRange(this.GetBorderSignField(0, false), field, new Vector3I(0, 0, 0), new Vector3I(0, vector3I.Y, vector3I.Z));
      this.ExtractRange(this.GetBorderSignField(0, true), field, new Vector3I(vector3I.X - 1, 0, 0), new Vector3I(vector3I.X, vector3I.Y, vector3I.Z));
      this.ExtractRange(this.GetBorderSignField(1, false), field, new Vector3I(0, 0, 0), new Vector3I(vector3I.X, 0, vector3I.Z));
      this.ExtractRange(this.GetBorderSignField(1, true), field, new Vector3I(0, vector3I.Y - 1, 0), new Vector3I(vector3I.X, vector3I.Y, vector3I.Z));
      this.ExtractRange(this.GetBorderSignField(2, false), field, new Vector3I(0, 0, 0), new Vector3I(vector3I.X, vector3I.Y, 0));
      this.ExtractRange(this.GetBorderSignField(2, true), field, new Vector3I(0, 0, vector3I.Z - 1), new Vector3I(vector3I.X, vector3I.Y, vector3I.Z));
    }

    private void ExtractRange(MyStorageData data, MyStorageData field, Vector3I min, Vector3I max)
    {
      data.Resize(max - min + 1);
      data.CopyRange(field, min, max, Vector3I.Zero, MyStorageDataTypeEnum.Content);
      data.CopyRange(field, min, max, Vector3I.Zero, MyStorageDataTypeEnum.Material);
    }

    public MyStorageData GetBorderSignField(int axis, bool side) => this.m_signField[(axis << 1) + (side ? 1 : 0)];

    public void SampleEdge(Vector3I localPosition, out byte material, out byte content)
    {
      MyStorageData borderSignField;
      if (localPosition.X == 0)
        borderSignField = this.GetBorderSignField(0, false);
      else if (localPosition.X >= this.m_forwardLimit.X)
      {
        borderSignField = this.GetBorderSignField(0, true);
        localPosition.X -= this.m_forwardLimit.X;
      }
      else if (localPosition.Y == 0)
        borderSignField = this.GetBorderSignField(1, false);
      else if (localPosition.Y >= this.m_forwardLimit.Y)
      {
        borderSignField = this.GetBorderSignField(1, true);
        localPosition.Y -= this.m_forwardLimit.Y;
      }
      else if (localPosition.Z == 0)
      {
        borderSignField = this.GetBorderSignField(2, false);
      }
      else
      {
        if (localPosition.Z < this.m_forwardLimit.Z)
          throw new InvalidOperationException();
        borderSignField = this.GetBorderSignField(2, true);
        localPosition.Z -= this.m_forwardLimit.Z;
      }
      int linear = borderSignField.ComputeLinear(ref localPosition);
      material = borderSignField.Material(linear);
      content = borderSignField.Content(linear);
    }

    public void Reset()
    {
      for (int originlaVxCnt = this.m_originlaVxCnt; originlaVxCnt < this.Mesh.VerticesCount; ++originlaVxCnt)
        this.m_edgeIndex.Remove(this.Mesh.Cells[originlaVxCnt]);
      this.Mesh.Resize(this.m_originlaVxCnt, this.m_originalTriangleCnt);
    }

    public bool IsStitched => this.m_originlaVxCnt != this.Mesh.VerticesCount;
  }
}
