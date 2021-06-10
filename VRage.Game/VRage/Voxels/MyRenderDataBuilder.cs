// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyRenderDataBuilder
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Voxels;

namespace VRage.Voxels
{
  public class MyRenderDataBuilder
  {
    [ThreadStatic]
    private static MyRenderDataBuilder m_instance;
    private static readonly MyConcurrentPool<MyRenderDataBuilder.Part> m_partPool = new MyConcurrentPool<MyRenderDataBuilder.Part>();
    private readonly SortedDictionary<int, MyRenderDataBuilder.Part> m_parts = new SortedDictionary<int, MyRenderDataBuilder.Part>();

    public static MyRenderDataBuilder Instance => MyRenderDataBuilder.m_instance ?? (MyRenderDataBuilder.m_instance = new MyRenderDataBuilder());

    public unsafe void Build(
      VrVoxelMesh mesh,
      out MyVoxelRenderCellData data,
      IMyVoxelRenderDataProcessorProvider dataProcessorProvider)
    {
      data = new MyVoxelRenderCellData();
      if (mesh.TriangleCount == 0)
        return;
      VrVoxelVertex* vertices = mesh.Vertices;
      VrVoxelTriangle* triangles = mesh.Triangles;
      data.CellBounds = BoundingBox.CreateInvalid();
      int vertexCount1 = mesh.VertexCount;
      for (int index = 0; index < vertexCount1; ++index)
        data.CellBounds.Include(vertices[index].Position);
      int triangleCount = mesh.TriangleCount;
      for (int index = 0; index < triangleCount; ++index)
      {
        VrVoxelTriangle triangle = triangles[index];
        MyRenderDataBuilder.MaterialTriple material = new MyRenderDataBuilder.MaterialTriple(ref vertices[triangle.V0], ref vertices[triangle.V1], ref vertices[triangle.V2]);
        MyRenderDataBuilder.Part part;
        if (!this.m_parts.TryGetValue((int) material, out part))
        {
          part = MyRenderDataBuilder.m_partPool.Get();
          part.Init(material);
          this.m_parts[(int) material] = part;
        }
        part.AddTriangle(triangle, vertices);
      }
      int vertexCount2 = 0;
      int num = 0;
      foreach (MyRenderDataBuilder.Part part in this.m_parts.Values)
      {
        vertexCount2 += part.Vertices.Count;
        num += part.Triangles.Count;
      }
      IMyVoxelRenderDataProcessor renderDataProcessor = dataProcessorProvider.GetRenderDataProcessor(vertexCount2, num * 3, this.m_parts.Count);
      foreach (MyRenderDataBuilder.Part instance in this.m_parts.Values)
      {
        fixed (VrVoxelTriangle* vrVoxelTrianglePtr = instance.Triangles.GetInternalArray())
        {
          int indicesCount = instance.Triangles.Count * 3;
          renderDataProcessor.AddPart(instance.Vertices, (ushort*) vrVoxelTrianglePtr, indicesCount, (MyVoxelMaterialTriple) instance.Material);
        }
        instance.Clear();
        MyRenderDataBuilder.m_partPool.Return(instance);
      }
      data.VertexCount = vertexCount2;
      data.IndexCount = num * 3;
      renderDataProcessor.GetDataAndDispose(ref data);
      this.m_parts.Clear();
    }

    private struct MaterialTriple
    {
      public readonly byte M0;
      public readonly byte M1;
      public readonly byte M2;

      public bool SingleMaterial => this.M1 == byte.MaxValue;

      public bool MultiMaterial => this.M1 != byte.MaxValue;

      public MaterialTriple(ref VrVoxelVertex v0, ref VrVoxelVertex v1, ref VrVoxelVertex v2)
      {
        this.M0 = v0.Material;
        this.M1 = v1.Material;
        this.M2 = v2.Material;
        if ((int) this.M0 == (int) this.M1)
          this.M1 = byte.MaxValue;
        if ((int) this.M0 == (int) this.M2)
          this.M2 = byte.MaxValue;
        if ((int) this.M1 == (int) this.M2)
          this.M2 = byte.MaxValue;
        if ((int) this.M0 > (int) this.M1)
          MyUtils.Swap<byte>(ref this.M0, ref this.M1);
        if ((int) this.M1 > (int) this.M2)
          MyUtils.Swap<byte>(ref this.M1, ref this.M2);
        if ((int) this.M0 <= (int) this.M1)
          return;
        MyUtils.Swap<byte>(ref this.M0, ref this.M1);
      }

      public MaterialTriple(byte m0, byte m1, byte m2)
      {
        this.M0 = m0;
        this.M1 = m1;
        this.M2 = m2;
      }

      public static implicit operator int(MyRenderDataBuilder.MaterialTriple triple) => -((int) triple.M0 | (int) triple.M1 << 8 | (int) triple.M2 << 16);

      public static implicit operator MyRenderDataBuilder.MaterialTriple(
        int packed)
      {
        packed = -packed;
        return new MyRenderDataBuilder.MaterialTriple((byte) (packed & (int) byte.MaxValue), (byte) (packed >> 8 & (int) byte.MaxValue), (byte) (packed >> 16 & (int) byte.MaxValue));
      }

      public static implicit operator MyVoxelMaterialTriple(
        MyRenderDataBuilder.MaterialTriple triple)
      {
        return new MyVoxelMaterialTriple(triple.M0, triple.M1, triple.M2);
      }

      public override string ToString() => this.SingleMaterial ? string.Format("S{{{0}}}", (object) this.M0) : string.Format("M{{{0}, {1}, {2}}}", (object) this.M0, (object) this.M1, (object) this.M2);
    }

    [GenerateActivator]
    private class Part
    {
      public MyRenderDataBuilder.MaterialTriple Material;
      private readonly Dictionary<ushort, ushort> m_indexMap = new Dictionary<ushort, ushort>();
      public readonly MyList<MyVertexFormatVoxelSingleData> Vertices = new MyList<MyVertexFormatVoxelSingleData>();
      public readonly MyList<VrVoxelTriangle> Triangles = new MyList<VrVoxelTriangle>();

      public void Init(MyRenderDataBuilder.MaterialTriple material) => this.Material = material;

      public void Clear()
      {
        this.m_indexMap.Clear();
        this.Vertices.Clear();
        this.Triangles.Clear();
      }

      public unsafe void AddTriangle(VrVoxelTriangle triangle, VrVoxelVertex* vertices)
      {
        this.RemapVertex(ref triangle.V0, vertices);
        this.RemapVertex(ref triangle.V1, vertices);
        this.RemapVertex(ref triangle.V2, vertices);
        this.Triangles.Add(triangle);
      }

      private unsafe void RemapVertex(ref ushort vertex, VrVoxelVertex* vertices)
      {
        ushort num;
        if (this.m_indexMap.TryGetValue(vertex, out num))
        {
          vertex = num;
        }
        else
        {
          int count = this.Vertices.Count;
          this.Vertices.Add(new MyVertexFormatVoxelSingleData()
          {
            Position = vertices[vertex].Position,
            Normal = vertices[vertex].Normal,
            PackedColorShift = vertices[vertex].Color.PackedValue,
            Material = new Byte4((float) this.Material.M0, (float) this.Material.M1, (float) this.Material.M2, (float) this.GetMaterialIndex(vertices[vertex].Material))
          });
          this.m_indexMap[vertex] = (ushort) count;
          vertex = (ushort) count;
        }
      }

      private int GetMaterialIndex(byte material)
      {
        if ((int) material == (int) this.Material.M0)
          return 0;
        if ((int) material == (int) this.Material.M1)
          return 1;
        return (int) material == (int) this.Material.M2 ? 2 : -1;
      }

      private class VRage_Voxels_MyRenderDataBuilder\u003C\u003EPart\u003C\u003EActor : IActivator, IActivator<MyRenderDataBuilder.Part>
      {
        object IActivator.CreateInstance() => (object) new MyRenderDataBuilder.Part();

        MyRenderDataBuilder.Part IActivator<MyRenderDataBuilder.Part>.CreateInstance() => new MyRenderDataBuilder.Part();
      }
    }
  }
}
