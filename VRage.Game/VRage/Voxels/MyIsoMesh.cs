// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyIsoMesh
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Voxels
{
  public sealed class MyIsoMesh
  {
    public readonly MyList<Vector3> Positions = new MyList<Vector3>();
    public readonly MyList<Vector3> Normals = new MyList<Vector3>();
    public readonly MyList<byte> Materials = new MyList<byte>();
    public readonly MyList<sbyte> Densities = new MyList<sbyte>();
    public readonly MyList<Vector3I> Cells = new MyList<Vector3I>();
    public readonly MyList<uint> ColorShiftHSV = new MyList<uint>();
    public readonly MyList<MyVoxelTriangle> Triangles = new MyList<MyVoxelTriangle>();
    public int Lod;
    public Vector3 PositionScale;
    public Vector3D PositionOffset;
    public Vector3I CellStart;
    public Vector3I CellEnd;

    public static MyIsoMesh FromNative(VrVoxelMesh nativeMesh)
    {
      if (nativeMesh == null)
        return (MyIsoMesh) null;
      MyIsoMesh myIsoMesh = new MyIsoMesh();
      myIsoMesh.CopyFromNative(nativeMesh);
      return myIsoMesh;
    }

    public int VerticesCount => this.Positions.Count;

    public int TrianglesCount => this.Triangles.Count;

    public Vector3I Size => this.CellEnd - this.CellStart + 1;

    public void Reserve(int vertexCount, int triangleCount)
    {
      if (this.Positions.Capacity < vertexCount)
      {
        this.Positions.Capacity = vertexCount;
        this.Normals.Capacity = vertexCount;
        this.Materials.Capacity = vertexCount;
        this.ColorShiftHSV.Capacity = vertexCount;
        this.Cells.Capacity = vertexCount;
        this.Densities.Capacity = vertexCount;
      }
      if (this.Triangles.Capacity >= triangleCount)
        return;
      this.Triangles.Capacity = triangleCount;
    }

    public void Resize(int vertexCount, int triangleCount)
    {
      if (this.Positions.Capacity >= vertexCount)
      {
        this.Positions.SetSize(vertexCount);
        this.Normals.SetSize(vertexCount);
        this.Materials.SetSize(vertexCount);
        this.ColorShiftHSV.SetSize(vertexCount);
        this.Cells.SetSize(vertexCount);
        this.Densities.SetSize(vertexCount);
      }
      if (this.Triangles.Capacity < triangleCount)
        return;
      this.Triangles.SetSize(triangleCount);
    }

    public void WriteTriangle(int v0, int v1, int v2) => this.Triangles.Add(new MyVoxelTriangle()
    {
      V0 = (ushort) v0,
      V1 = (ushort) v1,
      V2 = (ushort) v2
    });

    public int WriteVertex(
      ref Vector3I cell,
      ref Vector3 position,
      ref Vector3 normal,
      byte material,
      uint colorShift)
    {
      int count = this.Positions.Count;
      this.Positions.Add(position);
      this.Normals.Add(normal);
      this.Materials.Add(material);
      this.Cells.Add(cell);
      this.ColorShiftHSV.Add(colorShift);
      return count;
    }

    public void Clear()
    {
      this.Cells.Clear();
      this.Positions.Clear();
      this.Normals.Clear();
      this.Materials.Clear();
      this.Densities.Clear();
      this.ColorShiftHSV.Clear();
      this.Triangles.Clear();
    }

    public void GetUnpackedPosition(int idx, out Vector3 position) => position = (Vector3) (this.Positions[idx] * this.PositionScale + this.PositionOffset);

    public void GetUnpackedVertex(int idx, out MyVoxelVertex vertex)
    {
      vertex.Position = (Vector3) (this.Positions[idx] * this.PositionScale + this.PositionOffset);
      vertex.Normal = this.Normals[idx];
      vertex.Material = (int) this.Materials[idx];
      vertex.ColorShiftHSV = this.ColorShiftHSV[idx];
      vertex.Cell = this.Cells[idx];
    }

    public static bool IsEmpty(MyIsoMesh self) => self == null || self.Triangles.Count == 0;

    public unsafe void CopyFromNative(VrVoxelMesh vrMesh)
    {
      this.Lod = vrMesh.Lod;
      this.CellStart = vrMesh.Start;
      this.CellEnd = vrMesh.End;
      this.PositionScale = new Vector3((float) (1 << this.Lod));
      this.PositionOffset = (Vector3D) (this.CellStart * this.PositionScale);
      int vertexCount = vrMesh.VertexCount;
      int triangleCount = vrMesh.TriangleCount;
      this.Reserve(vertexCount, triangleCount);
      fixed (Vector3* positions = this.Positions.GetInternalArray())
        fixed (Vector3* normals = this.Normals.GetInternalArray())
          fixed (byte* materials = this.Materials.GetInternalArray())
            fixed (Vector3I* cells = this.Cells.GetInternalArray())
              fixed (MyVoxelTriangle* myVoxelTrianglePtr = this.Triangles.GetInternalArray())
                fixed (uint* numPtr = this.ColorShiftHSV.GetInternalArray())
                  vrMesh.GetMeshData(positions, normals, materials, cells, (Byte4*) numPtr, (VrVoxelTriangle*) myVoxelTrianglePtr);
      this.Positions.SetSize(vertexCount);
      this.Normals.SetSize(vertexCount);
      this.Materials.SetSize(vertexCount);
      this.Cells.SetSize(vertexCount);
      this.ColorShiftHSV.SetSize(vertexCount);
      this.Triangles.SetSize(triangleCount);
    }

    public bool IsEdge(ref Vector3I cell)
    {
      Vector3I vector3I = this.CellEnd - this.CellStart - 1;
      return cell.X == 0 || cell.X == vector3I.X || (cell.Y == 0 || cell.Y == vector3I.Y) || cell.Z == 0 || cell.Z == vector3I.Z;
    }
  }
}
