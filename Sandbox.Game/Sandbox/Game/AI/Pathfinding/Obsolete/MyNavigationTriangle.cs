// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyNavigationTriangle
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyNavigationTriangle : MyNavigationPrimitive
  {
    public bool Registered;

    public MyNavigationMesh Parent { get; private set; }

    public int Index { get; private set; }

    public int ComponentIndex { get; set; }

    public Vector3 Center
    {
      get
      {
        int num = 0;
        Vector3 zero = Vector3.Zero;
        MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();
        while (vertexEnumerator.MoveNext())
        {
          zero += vertexEnumerator.Current;
          ++num;
        }
        return zero / (float) num;
      }
    }

    public Vector3 Normal
    {
      get
      {
        MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();
        vertexEnumerator.MoveNext();
        Vector3 current1 = vertexEnumerator.Current;
        vertexEnumerator.MoveNext();
        Vector3 current2 = vertexEnumerator.Current;
        vertexEnumerator.MoveNext();
        Vector3 vector3 = (vertexEnumerator.Current - current1).Cross(current2 - current1);
        double num = (double) vector3.Normalize();
        return vector3;
      }
    }

    public void Init(MyNavigationMesh mesh, int triangleIndex)
    {
      this.Parent = mesh;
      this.Index = triangleIndex;
      this.ComponentIndex = -1;
      this.Registered = false;
      this.HasExternalNeighbors = false;
    }

    public override string ToString() => this.Parent.ToString() + "; Tri: " + (object) this.Index;

    public void GetVertices(out Vector3 a, out Vector3 b, out Vector3 c)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      a = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      b = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      c = vertexEnumerator.Current;
    }

    public void GetVertices(
      out int indA,
      out int indB,
      out int indC,
      out Vector3 a,
      out Vector3 b,
      out Vector3 c)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      indA = vertexEnumerator.CurrentIndex;
      a = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      indB = vertexEnumerator.CurrentIndex;
      b = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      indC = vertexEnumerator.CurrentIndex;
      c = vertexEnumerator.Current;
    }

    public void GetTransformed(
      ref MatrixI tform,
      out Vector3 newA,
      out Vector3 newB,
      out Vector3 newC)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      newA = vertexEnumerator.Current;
      Vector3.Transform(ref newA, ref tform, out newA);
      vertexEnumerator.MoveNext();
      newB = vertexEnumerator.Current;
      Vector3.Transform(ref newB, ref tform, out newB);
      vertexEnumerator.MoveNext();
      newC = vertexEnumerator.Current;
      Vector3.Transform(ref newC, ref tform, out newC);
    }

    public MyWingedEdgeMesh.FaceVertexEnumerator GetVertexEnumerator() => this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();

    public MyNavigationEdge GetNavigationEdge(int index)
    {
      MyWingedEdgeMesh mesh = this.Parent.Mesh;
      int edgeIndex = this.GetEdgeIndex(index);
      MyWingedEdgeMesh.Edge edge = mesh.GetEdge(edgeIndex);
      MyNavigationTriangle navigationTriangle1 = (MyNavigationTriangle) null;
      MyNavigationTriangle navigationTriangle2 = (MyNavigationTriangle) null;
      if (edge.LeftFace != -1)
        navigationTriangle1 = mesh.GetFace(edge.LeftFace).GetUserData<MyNavigationTriangle>();
      if (edge.RightFace != -1)
        navigationTriangle2 = mesh.GetFace(edge.RightFace).GetUserData<MyNavigationTriangle>();
      MyNavigationEdge.Static.Init((MyNavigationPrimitive) navigationTriangle1, (MyNavigationPrimitive) navigationTriangle2, edgeIndex);
      return MyNavigationEdge.Static;
    }

    public void GetEdgeVertices(int index, out Vector3 pred, out Vector3 succ)
    {
      MyWingedEdgeMesh mesh = this.Parent.Mesh;
      int edgeIndex = this.GetEdgeIndex(index);
      MyWingedEdgeMesh.Edge edge = mesh.GetEdge(edgeIndex);
      pred = mesh.GetVertexPosition(edge.GetFacePredVertex(this.Index));
      succ = mesh.GetVertexPosition(edge.GetFaceSuccVertex(this.Index));
    }

    public bool IsEdgeVertexDangerous(int index, bool predVertex)
    {
      MyWingedEdgeMesh mesh = this.Parent.Mesh;
      int edgeIndex1 = this.GetEdgeIndex(index);
      int edgeIndex2 = edgeIndex1;
      MyWingedEdgeMesh.Edge edge = mesh.GetEdge(edgeIndex2);
      int vertexIndex = predVertex ? edge.GetFacePredVertex(this.Index) : edge.GetFaceSuccVertex(this.Index);
      while (!MyNavigationTriangle.IsTriangleDangerous(edge.VertexLeftFace(vertexIndex)))
      {
        int nextVertexEdge = edge.GetNextVertexEdge(vertexIndex);
        edge = mesh.GetEdge(nextVertexEdge);
        if (nextVertexEdge == edgeIndex1)
          return false;
      }
      return true;
    }

    public void FindDangerousVertices(List<int> output)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = this.Parent.Mesh.GetFace(this.Index).GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      int currentIndex1 = vertexEnumerator.CurrentIndex;
      vertexEnumerator.MoveNext();
      int currentIndex2 = vertexEnumerator.CurrentIndex;
      vertexEnumerator.MoveNext();
      int currentIndex3 = vertexEnumerator.CurrentIndex;
    }

    public int GetEdgeIndex(int index)
    {
      MyWingedEdgeMesh.FaceEdgeEnumerator faceEdgeEnumerator = new MyWingedEdgeMesh.FaceEdgeEnumerator(this.Parent.Mesh, this.Index);
      faceEdgeEnumerator.MoveNext();
      for (; index != 0; --index)
        faceEdgeEnumerator.MoveNext();
      return faceEdgeEnumerator.Current;
    }

    private static bool IsTriangleDangerous(int triIndex) => triIndex == -1;

    public override Vector3 Position => this.Center;

    public override Vector3D WorldPosition
    {
      get
      {
        MatrixD worldMatrix = this.Parent.GetWorldMatrix();
        Vector3D center = (Vector3D) this.Center;
        Vector3D result;
        Vector3D.Transform(ref center, ref worldMatrix, out result);
        return result;
      }
    }

    public override Vector3 ProjectLocalPoint(Vector3 point)
    {
      Vector3 a;
      Vector3 b;
      Vector3 c;
      this.GetVertices(out a, out b, out c);
      Vector3 result1;
      Vector3.Subtract(ref b, ref a, out result1);
      Vector3 result2;
      Vector3.Subtract(ref c, ref a, out result2);
      Vector3 result3;
      Vector3.Subtract(ref point, ref a, out result3);
      Vector3 result4;
      Vector3.Cross(ref result1, ref result2, out result4);
      Vector3 result5;
      Vector3.Cross(ref result1, ref result3, out result5);
      Vector3 result6;
      Vector3.Cross(ref result3, ref result2, out result6);
      float num1 = 1f / result4.LengthSquared();
      double num2 = (double) Vector3.Dot(result5, result4);
      float num3 = Vector3.Dot(result6, result4) * num1;
      double num4 = (double) num1;
      float num5 = (float) (num2 * num4);
      float num6 = 1f - num3 - num5;
      if ((double) num6 < 0.0)
      {
        if ((double) num3 < 0.0)
          return c;
        if ((double) num5 < 0.0)
          return b;
        float num7 = (float) (1.0 / (1.0 - (double) num6));
        num6 = 0.0f;
        num3 *= num7;
        num5 *= num7;
      }
      else if ((double) num3 < 0.0)
      {
        if ((double) num5 < 0.0)
          return a;
        float num7 = (float) (1.0 / (1.0 - (double) num3));
        num3 = 0.0f;
        num6 *= num7;
        num5 *= num7;
      }
      else if ((double) num5 < 0.0)
      {
        float num7 = (float) (1.0 / (1.0 - (double) num5));
        num5 = 0.0f;
        num6 *= num7;
        num3 *= num7;
      }
      return a * num6 + b * num3 + c * num5;
    }

    public override IMyNavigationGroup Group => (IMyNavigationGroup) this.Parent;

    public override int GetOwnNeighborCount() => 3;

    public override IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(
      int index)
    {
      int faceIndex = this.Parent.Mesh.GetEdge(this.GetEdgeIndex(index)).OtherFace(this.Index);
      return faceIndex != -1 ? (IMyPathVertex<MyNavigationPrimitive>) this.Parent.Mesh.GetFace(faceIndex).GetUserData<MyNavigationPrimitive>() : (IMyPathVertex<MyNavigationPrimitive>) null;
    }

    public override IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(
      int index)
    {
      return (IMyPathEdge<MyNavigationPrimitive>) this.GetNavigationEdge(index);
    }

    public override MyHighLevelPrimitive GetHighLevelPrimitive() => this.Parent.GetHighLevelPrimitive((MyNavigationPrimitive) this);
  }
}
