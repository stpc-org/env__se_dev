﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyQuantizedBvhAllTrianglesResult
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using BulletXNA.BulletCollision;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
  public class MyQuantizedBvhAllTrianglesResult
  {
    private MyModel m_model;
    private readonly List<MyIntersectionResultLineTriangle> m_result = new List<MyIntersectionResultLineTriangle>();
    private Line m_line;
    private IntersectionFlags m_flags;
    private readonly MyResultComparer m_comparer = new MyResultComparer();
    public readonly ProcessCollisionHandler ProcessTriangleHandler;
    private Plane[] m_planes;

    public MyQuantizedBvhAllTrianglesResult() => this.ProcessTriangleHandler = new ProcessCollisionHandler(this.ProcessTriangle);

    public List<MyIntersectionResultLineTriangle> Result => this.m_result;

    public void Start(MyModel model, Line line, Plane[] planes, IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES)
    {
      this.m_result.Clear();
      this.m_model = model;
      this.m_line = line;
      this.m_flags = flags;
      this.m_planes = planes;
    }

    private float? ProcessTriangle(int triangleIndex)
    {
      MyTriangleVertexIndices triangle1 = this.m_model.Triangles[triangleIndex];
      MyTriangle_Vertices triangle2;
      this.m_model.GetVertex(triangle1.I0, triangle1.I2, triangle1.I1, out triangle2.Vertex0, out triangle2.Vertex1, out triangle2.Vertex2);
      Vector3 normal = this.m_planes[triangleIndex].Normal;
      if ((this.m_flags & IntersectionFlags.FLIPPED_TRIANGLES) == (IntersectionFlags) 0 && (double) this.m_line.Direction.Dot(ref normal) > 0.0)
        return new float?();
      float? triangleIntersection = MyUtils.GetLineTriangleIntersection(ref this.m_line, ref triangle2);
      if (triangleIntersection.HasValue)
      {
        MyTriangle_BoneIndicesWeigths? boneIndicesWeights = this.m_model.GetBoneIndicesWeights(triangleIndex);
        this.m_result.Add(new MyIntersectionResultLineTriangle(triangleIndex, ref triangle2, ref boneIndicesWeights, ref normal, triangleIntersection.Value));
      }
      return triangleIntersection;
    }

    public void End() => this.m_result.Sort((IComparer<MyIntersectionResultLineTriangle>) this.m_comparer);
  }
}
