// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyNavigationMesh
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Algorithms;
using VRage.Generics;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public abstract class MyNavigationMesh : MyPathFindingSystem<MyNavigationPrimitive>, IMyNavigationGroup
  {
    private MyDynamicObjectPool<MyNavigationTriangle> m_triPool;
    private readonly MyNavgroupLinks m_externalLinks;
    private Vector3 m_vertex;
    private Vector3 m_left;
    private Vector3 m_right;
    private Vector3 m_normal;
    private static readonly List<Vector3> m_debugPointsLeft = new List<Vector3>();
    private static readonly List<Vector3> m_debugPointsRight = new List<Vector3>();
    private static readonly List<Vector3> m_path = new List<Vector3>();
    private static List<Vector3> m_path2;
    private static readonly List<MyNavigationMesh.FunnelState> m_debugFunnel = new List<MyNavigationMesh.FunnelState>();
    public static int m_debugFunnelIdx = 0;

    public MyWingedEdgeMesh Mesh { get; }

    protected MyNavigationMesh(
      MyNavgroupLinks externalLinks,
      int trianglePrealloc = 16,
      Func<long> timestampFunction = null)
      : base(timestampFunction: timestampFunction)
    {
      this.m_triPool = new MyDynamicObjectPool<MyNavigationTriangle>(trianglePrealloc);
      this.Mesh = new MyWingedEdgeMesh();
      this.m_externalLinks = externalLinks;
    }

    protected MyNavigationTriangle AddTriangle(
      ref Vector3 A,
      ref Vector3 B,
      ref Vector3 C,
      ref int edgeAB,
      ref int edgeBC,
      ref int edgeCA)
    {
      MyNavigationTriangle newTri = this.m_triPool.Allocate();
      int triangleIndex;
      switch (0 + (edgeAB == -1 ? 1 : 0) + (edgeBC == -1 ? 1 : 0) + (edgeCA == -1 ? 1 : 0))
      {
        case 1:
          triangleIndex = edgeAB != -1 ? (edgeBC != -1 ? this.GetTriangleOneNewEdge(ref edgeCA, ref edgeAB, ref edgeBC, newTri) : this.GetTriangleOneNewEdge(ref edgeBC, ref edgeCA, ref edgeAB, newTri)) : this.GetTriangleOneNewEdge(ref edgeAB, ref edgeBC, ref edgeCA, newTri);
          break;
        case 2:
          triangleIndex = edgeAB == -1 ? (edgeBC == -1 ? this.Mesh.ExtrudeTriangleFromEdge(ref B, edgeCA, (object) newTri, out edgeAB, out edgeBC) : this.Mesh.ExtrudeTriangleFromEdge(ref A, edgeBC, (object) newTri, out edgeCA, out edgeAB)) : this.Mesh.ExtrudeTriangleFromEdge(ref C, edgeAB, (object) newTri, out edgeBC, out edgeCA);
          break;
        case 3:
          triangleIndex = this.Mesh.MakeNewTriangle((object) newTri, ref A, ref B, ref C, out edgeAB, out edgeBC, out edgeCA);
          break;
        default:
          MyWingedEdgeMesh.Edge edge1 = this.Mesh.GetEdge(edgeAB);
          MyWingedEdgeMesh.Edge edge2 = this.Mesh.GetEdge(edgeBC);
          MyWingedEdgeMesh.Edge edge3 = this.Mesh.GetEdge(edgeCA);
          int sharedVertex1 = edge3.TryGetSharedVertex(ref edge1);
          int sharedVertex2 = edge1.TryGetSharedVertex(ref edge2);
          int sharedVertex3 = edge2.TryGetSharedVertex(ref edge3);
          switch (0 + (sharedVertex1 == -1 ? 0 : 1) + (sharedVertex2 == -1 ? 0 : 1) + (sharedVertex3 == -1 ? 0 : 1))
          {
            case 1:
              triangleIndex = sharedVertex1 == -1 ? (sharedVertex2 == -1 ? this.GetTriangleOneSharedVertex(edgeBC, edgeCA, ref edgeAB, sharedVertex3, newTri) : this.GetTriangleOneSharedVertex(edgeAB, edgeBC, ref edgeCA, sharedVertex2, newTri)) : this.GetTriangleOneSharedVertex(edgeCA, edgeAB, ref edgeBC, sharedVertex1, newTri);
              break;
            case 2:
              triangleIndex = sharedVertex1 != -1 ? (sharedVertex2 != -1 ? this.GetTriangleTwoSharedVertices(edgeCA, edgeAB, ref edgeBC, sharedVertex1, sharedVertex2, newTri) : this.GetTriangleTwoSharedVertices(edgeBC, edgeCA, ref edgeAB, sharedVertex3, sharedVertex1, newTri)) : this.GetTriangleTwoSharedVertices(edgeAB, edgeBC, ref edgeCA, sharedVertex2, sharedVertex3, newTri);
              break;
            case 3:
              triangleIndex = this.Mesh.MakeFace((object) newTri, edgeAB);
              break;
            default:
              int newEdgeS;
              int newEdgeP;
              triangleIndex = this.Mesh.ExtrudeTriangleFromEdge(ref C, edgeAB, (object) newTri, out newEdgeS, out newEdgeP);
              this.Mesh.MergeEdges(newEdgeP, edgeCA);
              this.Mesh.MergeEdges(newEdgeS, edgeBC);
              break;
          }
          break;
      }
      newTri.Init(this, triangleIndex);
      return newTri;
    }

    protected void RemoveTriangle(MyNavigationTriangle tri)
    {
      this.Mesh.RemoveFace(tri.Index);
      this.m_triPool.Deallocate(tri);
    }

    private int GetTriangleOneNewEdge(
      ref int newEdge,
      ref int succ,
      ref int pred,
      MyNavigationTriangle newTri)
    {
      MyWingedEdgeMesh.Edge edge1 = this.Mesh.GetEdge(pred);
      MyWingedEdgeMesh.Edge edge2 = this.Mesh.GetEdge(succ);
      int sharedVertex = edge1.TryGetSharedVertex(ref edge2);
      if (sharedVertex != -1)
        return this.Mesh.MakeEdgeFace(edge1.OtherVertex(sharedVertex), edge2.OtherVertex(sharedVertex), pred, succ, (object) newTri, out newEdge);
      int edge1_1 = succ;
      Vector3 vertexPosition = this.Mesh.GetVertexPosition(edge2.GetFacePredVertex(-1));
      int num = this.Mesh.ExtrudeTriangleFromEdge(ref vertexPosition, pred, (object) newTri, out newEdge, out succ);
      this.Mesh.MergeEdges(edge1_1, succ);
      return num;
    }

    private int GetTriangleOneSharedVertex(
      int edgeCA,
      int edgeAB,
      ref int edgeBC,
      int sharedA,
      MyNavigationTriangle newTri)
    {
      int vert1 = this.Mesh.GetEdge(edgeAB).OtherVertex(sharedA);
      int vert2 = this.Mesh.GetEdge(edgeCA).OtherVertex(sharedA);
      int edge1 = edgeBC;
      int num = this.Mesh.MakeEdgeFace(vert1, vert2, edgeAB, edgeCA, (object) newTri, out edgeBC);
      this.Mesh.MergeEdges(edge1, edgeBC);
      return num;
    }

    private int GetTriangleTwoSharedVertices(
      int edgeAB,
      int edgeBC,
      ref int edgeCA,
      int sharedB,
      int sharedC,
      MyNavigationTriangle newTri)
    {
      int vert2 = this.Mesh.GetEdge(edgeAB).OtherVertex(sharedB);
      int leftEdge = edgeCA;
      int num = this.Mesh.MakeEdgeFace(sharedC, vert2, edgeBC, edgeAB, (object) newTri, out edgeCA);
      this.Mesh.MergeAngle(leftEdge, edgeCA, sharedC);
      return num;
    }

    public MyNavigationTriangle GetTriangle(int index) => this.Mesh.GetFace(index).GetUserData<MyNavigationTriangle>();

    protected MyNavigationTriangle GetEdgeTriangle(int edgeIndex)
    {
      MyWingedEdgeMesh.Edge edge = this.Mesh.GetEdge(edgeIndex);
      return edge.LeftFace == -1 ? this.GetTriangle(edge.RightFace) : this.GetTriangle(edge.LeftFace);
    }

    protected List<Vector4D> FindRefinedPath(
      MyNavigationTriangle start,
      MyNavigationTriangle end,
      ref Vector3 startPoint,
      ref Vector3 endPoint)
    {
      MyPath<MyNavigationPrimitive> path = this.FindPath((MyNavigationPrimitive) start, (MyNavigationPrimitive) end, (Predicate<MyNavigationPrimitive>) null, (Predicate<IMyPathEdge<MyNavigationPrimitive>>) null);
      if (path == null)
        return (List<Vector4D>) null;
      List<Vector4D> refinedPath = new List<Vector4D>();
      refinedPath.Add(new Vector4D((Vector3D) startPoint, 1.0));
      new MyNavigationMesh.Funnel().Calculate(path, refinedPath, ref startPoint, ref endPoint, 0, path.Count - 1);
      MyNavigationMesh.m_path.Clear();
      foreach (Vector4D xyz in refinedPath)
        MyNavigationMesh.m_path.Add((Vector3) new Vector3D(xyz));
      return refinedPath;
    }

    public void RefinePath(
      MyPath<MyNavigationPrimitive> path,
      List<Vector4D> output,
      ref Vector3 startPoint,
      ref Vector3 endPoint,
      int begin,
      int end)
    {
      new MyNavigationMesh.Funnel().Calculate(path, output, ref startPoint, ref endPoint, begin, end);
    }

    public abstract Vector3 GlobalToLocal(Vector3D globalPos);

    public abstract Vector3D LocalToGlobal(Vector3 localPos);

    public abstract MyHighLevelGroup HighLevelGroup { get; }

    public abstract MyHighLevelPrimitive GetHighLevelPrimitive(
      MyNavigationPrimitive myNavigationTriangle);

    public abstract IMyHighLevelComponent GetComponent(
      MyHighLevelPrimitive highLevelPrimitive);

    public abstract MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistanceSq);

    public void ErasePools() => this.m_triPool = (MyDynamicObjectPool<MyNavigationTriangle>) null;

    [Conditional("DEBUG")]
    public virtual void DebugDraw(ref Matrix drawMatrix)
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != MyWEMDebugDrawMode.NONE)
      {
        this.Mesh.DebugDraw(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES);
        this.Mesh.CustomDebugDrawFaces(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES, (Func<object, string>) (obj => (obj as MyNavigationTriangle).Index.ToString()));
      }
      if (!MyFakes.DEBUG_DRAW_FUNNEL)
        return;
      Vector3D position1 = (Vector3D) Vector3.Transform(this.m_vertex, drawMatrix);
      Color color = Color.Yellow;
      Color vector3_1 = (Color) color.ToVector3();
      MyRenderProxy.DebugDrawSphere(position1, 0.05f, vector3_1, depthRead: false);
      Vector3D position2 = (Vector3D) Vector3.Transform(this.m_vertex + this.m_normal, drawMatrix);
      color = Color.Orange;
      Color vector3_2 = (Color) color.ToVector3();
      MyRenderProxy.DebugDrawSphere(position2, 0.05f, vector3_2, depthRead: false);
      Vector3D position3 = (Vector3D) Vector3.Transform(this.m_left, drawMatrix);
      color = Color.Red;
      Color vector3_3 = (Color) color.ToVector3();
      MyRenderProxy.DebugDrawSphere(position3, 0.05f, vector3_3, depthRead: false);
      Vector3D position4 = (Vector3D) Vector3.Transform(this.m_right, drawMatrix);
      color = Color.Green;
      Color vector3_4 = (Color) color.ToVector3();
      MyRenderProxy.DebugDrawSphere(position4, 0.05f, vector3_4, depthRead: false);
      foreach (Vector3 position5 in MyNavigationMesh.m_debugPointsLeft)
      {
        Vector3D position6 = (Vector3D) Vector3.Transform(position5, drawMatrix);
        color = Color.Red;
        Color vector3_5 = (Color) color.ToVector3();
        MyRenderProxy.DebugDrawSphere(position6, 0.03f, vector3_5, depthRead: false);
      }
      foreach (Vector3 position5 in MyNavigationMesh.m_debugPointsRight)
        MyRenderProxy.DebugDrawSphere((Vector3D) Vector3.Transform(position5, drawMatrix), 0.04f, (Color) Color.Green.ToVector3(), depthRead: false);
      Vector3? nullable1 = new Vector3?();
      if (MyNavigationMesh.m_path != null)
      {
        foreach (Vector3 position5 in MyNavigationMesh.m_path)
        {
          Vector3 vector3_5 = Vector3.Transform(position5, drawMatrix);
          MyRenderProxy.DebugDrawSphere((Vector3D) (vector3_5 + Vector3.Up * 0.2f), 0.02f, (Color) Color.Orange.ToVector3(), depthRead: false);
          if (nullable1.HasValue)
            MyRenderProxy.DebugDrawLine3D((Vector3D) (nullable1.Value + Vector3.Up * 0.2f), (Vector3D) (vector3_5 + Vector3.Up * 0.2f), Color.Orange, Color.Orange, true);
          nullable1 = new Vector3?(vector3_5);
        }
      }
      Vector3? nullable2 = new Vector3?();
      if (MyNavigationMesh.m_path2 != null)
      {
        foreach (Vector3 position5 in MyNavigationMesh.m_path2)
        {
          Vector3 vector3_5 = Vector3.Transform(position5, drawMatrix);
          if (nullable2.HasValue)
            MyRenderProxy.DebugDrawLine3D((Vector3D) (nullable2.Value + Vector3.Up * 0.1f), (Vector3D) (vector3_5 + Vector3.Up * 0.1f), Color.Violet, Color.Violet, true);
          nullable2 = new Vector3?(vector3_5);
        }
      }
      if (MyNavigationMesh.m_debugFunnel.Count <= 0)
        return;
      MyNavigationMesh.FunnelState funnelState = MyNavigationMesh.m_debugFunnel[MyNavigationMesh.m_debugFunnelIdx % MyNavigationMesh.m_debugFunnel.Count];
      Vector3 vector3_6 = Vector3.Transform(funnelState.Apex, drawMatrix);
      Vector3 vector3_7 = Vector3.Transform(funnelState.Left, drawMatrix);
      Vector3 vector3_8 = Vector3.Transform(funnelState.Right, drawMatrix);
      Vector3 vector3_9 = vector3_6 + (vector3_7 - vector3_6) * 10f;
      Vector3 vector3_10 = vector3_6 + (vector3_8 - vector3_6) * 10f;
      Color cyan = Color.Cyan;
      MyRenderProxy.DebugDrawLine3D((Vector3D) (vector3_6 + Vector3.Up * 0.1f), (Vector3D) (vector3_9 + Vector3.Up * 0.1f), cyan, cyan, true);
      MyRenderProxy.DebugDrawLine3D((Vector3D) (vector3_6 + Vector3.Up * 0.1f), (Vector3D) (vector3_10 + Vector3.Up * 0.1f), cyan, cyan, true);
    }

    public void RemoveFace(int index) => this.Mesh.RemoveFace(index);

    public virtual MatrixD GetWorldMatrix() => MatrixD.Identity;

    [Conditional("DEBUG")]
    public void CheckMeshConsistency()
    {
    }

    public int ApproximateMemoryFootprint() => this.Mesh.ApproximateMemoryFootprint() + this.m_triPool.Count * (Environment.Is64BitProcess ? 88 : 56);

    public int GetExternalNeighborCount(MyNavigationPrimitive primitive)
    {
      MyNavgroupLinks externalLinks = this.m_externalLinks;
      return externalLinks == null ? 0 : externalLinks.GetLinkCount(primitive);
    }

    public MyNavigationPrimitive GetExternalNeighbor(
      MyNavigationPrimitive primitive,
      int index)
    {
      return this.m_externalLinks?.GetLinkedNeighbor(primitive, index);
    }

    public IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(
      MyNavigationPrimitive primitive,
      int index)
    {
      return this.m_externalLinks?.GetEdge(primitive, index);
    }

    private class Funnel
    {
      private Vector3 m_end;
      private int m_endIndex;
      private MyPath<MyNavigationPrimitive> m_input;
      private List<Vector4D> m_output;
      private Vector3 m_apex;
      private Vector3 m_apexNormal;
      private Vector3 m_leftPoint;
      private Vector3 m_rightPoint;
      private int m_leftIndex;
      private int m_rightIndex;
      private Vector3 m_leftPlaneNormal;
      private Vector3 m_rightPlaneNormal;
      private float m_leftD;
      private float m_rightD;
      private bool m_funnelConstructed;
      private bool m_segmentDangerous;
      private static readonly float SAFE_DISTANCE = 0.7f;
      private static readonly float SAFE_DISTANCE_SQ = MyNavigationMesh.Funnel.SAFE_DISTANCE * MyNavigationMesh.Funnel.SAFE_DISTANCE;
      private static readonly float SAFE_DISTANCE2_SQ = (float) (((double) MyNavigationMesh.Funnel.SAFE_DISTANCE + (double) MyNavigationMesh.Funnel.SAFE_DISTANCE) * ((double) MyNavigationMesh.Funnel.SAFE_DISTANCE + (double) MyNavigationMesh.Funnel.SAFE_DISTANCE));

      public void Calculate(
        MyPath<MyNavigationPrimitive> inputPath,
        List<Vector4D> refinedPath,
        ref Vector3 start,
        ref Vector3 end,
        int startIndex,
        int endIndex)
      {
        MyNavigationMesh.m_debugFunnel.Clear();
        MyNavigationMesh.m_debugPointsLeft.Clear();
        MyNavigationMesh.m_debugPointsRight.Clear();
        this.m_end = end;
        this.m_endIndex = endIndex;
        this.m_input = inputPath;
        this.m_output = refinedPath;
        this.m_apex = start;
        this.m_funnelConstructed = false;
        this.m_segmentDangerous = false;
        int num = startIndex;
        while (num < endIndex)
        {
          num = this.AddTriangle(num);
          if (num == endIndex)
          {
            MyNavigationMesh.Funnel.PointTestResult pointTestResult = this.TestPoint(end);
            switch (pointTestResult)
            {
              case MyNavigationMesh.Funnel.PointTestResult.LEFT:
                this.m_apex = this.m_leftPoint;
                this.m_funnelConstructed = false;
                this.ConstructFunnel(this.m_leftIndex);
                num = this.m_leftIndex + 1;
                break;
              case MyNavigationMesh.Funnel.PointTestResult.RIGHT:
                this.m_apex = this.m_rightPoint;
                this.m_funnelConstructed = false;
                this.ConstructFunnel(this.m_rightIndex);
                num = this.m_rightIndex + 1;
                break;
            }
            if (pointTestResult == MyNavigationMesh.Funnel.PointTestResult.INSIDE || num == endIndex)
              this.AddPoint((Vector3D) this.ProjectEndOnTriangle(num));
          }
        }
        if (startIndex == endIndex)
          this.AddPoint((Vector3D) this.ProjectEndOnTriangle(num));
        this.m_input = (MyPath<MyNavigationPrimitive>) null;
        this.m_output = (List<Vector4D>) null;
      }

      private void AddPoint(Vector3D point)
      {
        float num = this.m_segmentDangerous ? 0.5f : 2f;
        this.m_output.Add(new Vector4D(point, (double) num));
        int index = this.m_output.Count - 1;
        if (index >= 0)
        {
          Vector4D vector4D = this.m_output[index];
          if (vector4D.W > (double) num)
          {
            vector4D.W = (double) num;
            this.m_output[index] = vector4D;
          }
        }
        this.m_segmentDangerous = false;
      }

      private Vector3 ProjectEndOnTriangle(int i) => (this.m_input[i].Vertex as MyNavigationTriangle).ProjectLocalPoint(this.m_end);

      private int AddTriangle(int index)
      {
        if (!this.m_funnelConstructed)
        {
          this.ConstructFunnel(index);
        }
        else
        {
          MyPath<MyNavigationPrimitive>.PathNode pathNode = this.m_input[index];
          MyNavigationTriangle vertex = pathNode.Vertex as MyNavigationTriangle;
          vertex.GetNavigationEdge(pathNode.nextVertex);
          Vector3 left;
          Vector3 right;
          this.GetEdgeVerticesSafe(vertex, pathNode.nextVertex, out left, out right);
          MyNavigationMesh.Funnel.PointTestResult pointTestResult1 = this.TestPoint(left);
          MyNavigationMesh.Funnel.PointTestResult pointTestResult2 = this.TestPoint(right);
          if (pointTestResult1 == MyNavigationMesh.Funnel.PointTestResult.INSIDE)
            this.NarrowFunnel(left, index, true);
          if (pointTestResult2 == MyNavigationMesh.Funnel.PointTestResult.INSIDE)
            this.NarrowFunnel(right, index, false);
          if (pointTestResult1 == MyNavigationMesh.Funnel.PointTestResult.RIGHT)
          {
            this.m_apex = this.m_rightPoint;
            this.m_funnelConstructed = false;
            this.ConstructFunnel(this.m_rightIndex + 1);
            return this.m_rightIndex + 1;
          }
          if (pointTestResult2 == MyNavigationMesh.Funnel.PointTestResult.LEFT)
          {
            this.m_apex = this.m_leftPoint;
            this.m_funnelConstructed = false;
            this.ConstructFunnel(this.m_leftIndex + 1);
            return this.m_leftIndex + 1;
          }
          if (pointTestResult1 == MyNavigationMesh.Funnel.PointTestResult.INSIDE || pointTestResult2 == MyNavigationMesh.Funnel.PointTestResult.INSIDE)
            MyNavigationMesh.m_debugFunnel.Add(new MyNavigationMesh.FunnelState()
            {
              Apex = this.m_apex,
              Left = this.m_leftPoint,
              Right = this.m_rightPoint
            });
        }
        return index + 1;
      }

      private void GetEdgeVerticesSafe(
        MyNavigationTriangle triangle,
        int edgeIndex,
        out Vector3 left,
        out Vector3 right)
      {
        triangle.GetEdgeVertices(edgeIndex, out left, out right);
        float num1 = (left - right).LengthSquared();
        bool flag1 = triangle.IsEdgeVertexDangerous(edgeIndex, true);
        bool flag2 = triangle.IsEdgeVertexDangerous(edgeIndex, false);
        this.m_segmentDangerous |= flag1 | flag2;
        if (flag1)
        {
          if (flag2)
          {
            if ((double) MyNavigationMesh.Funnel.SAFE_DISTANCE2_SQ > (double) num1)
            {
              left = (left + right) * 0.5f;
              right = left;
            }
            else
            {
              float num2 = MyNavigationMesh.Funnel.SAFE_DISTANCE / (float) Math.Sqrt((double) num1);
              Vector3 vector3 = right * num2 + left * (1f - num2);
              right = left * num2 + right * (1f - num2);
              left = vector3;
            }
          }
          else if ((double) MyNavigationMesh.Funnel.SAFE_DISTANCE_SQ > (double) num1)
          {
            left = right;
          }
          else
          {
            float num2 = MyNavigationMesh.Funnel.SAFE_DISTANCE / (float) Math.Sqrt((double) num1);
            left = right * num2 + left * (1f - num2);
          }
        }
        else if (flag2)
        {
          if ((double) MyNavigationMesh.Funnel.SAFE_DISTANCE_SQ > (double) num1)
          {
            right = left;
          }
          else
          {
            float num2 = MyNavigationMesh.Funnel.SAFE_DISTANCE / (float) Math.Sqrt((double) num1);
            right = left * num2 + right * (1f - num2);
          }
        }
        MyNavigationMesh.m_debugPointsLeft.Add(left);
        MyNavigationMesh.m_debugPointsRight.Add(right);
      }

      private void NarrowFunnel(Vector3 point, int index, bool left)
      {
        if (left)
        {
          this.m_leftPoint = point;
          this.m_leftIndex = index;
          this.RecalculateLeftPlane();
        }
        else
        {
          this.m_rightPoint = point;
          this.m_rightIndex = index;
          this.RecalculateRightPlane();
        }
      }

      private void ConstructFunnel(int index)
      {
        if (index >= this.m_endIndex)
        {
          this.AddPoint((Vector3D) this.m_apex);
        }
        else
        {
          MyPath<MyNavigationPrimitive>.PathNode pathNode = this.m_input[index];
          MyNavigationTriangle vertex = pathNode.Vertex as MyNavigationTriangle;
          vertex.GetNavigationEdge(pathNode.nextVertex);
          this.GetEdgeVerticesSafe(vertex, pathNode.nextVertex, out this.m_leftPoint, out this.m_rightPoint);
          if (Vector3.IsZero(this.m_leftPoint - this.m_apex))
            this.m_apex = vertex.Center;
          else if (Vector3.IsZero(this.m_rightPoint - this.m_apex))
          {
            this.m_apex = vertex.Center;
          }
          else
          {
            this.m_apexNormal = vertex.Normal;
            float num = this.m_leftPoint.Dot(this.m_apexNormal);
            this.m_apex -= this.m_apexNormal * (this.m_apex.Dot(this.m_apexNormal) - num);
            this.m_leftIndex = this.m_rightIndex = index;
            this.RecalculateLeftPlane();
            this.RecalculateRightPlane();
            this.m_funnelConstructed = true;
            this.AddPoint((Vector3D) this.m_apex);
            MyNavigationMesh.m_debugFunnel.Add(new MyNavigationMesh.FunnelState()
            {
              Apex = this.m_apex,
              Left = this.m_leftPoint,
              Right = this.m_rightPoint
            });
          }
        }
      }

      private MyNavigationMesh.Funnel.PointTestResult TestPoint(Vector3 point)
      {
        if ((double) point.Dot(this.m_leftPlaneNormal) < -(double) this.m_leftD)
          return MyNavigationMesh.Funnel.PointTestResult.LEFT;
        return (double) point.Dot(this.m_rightPlaneNormal) < -(double) this.m_rightD ? MyNavigationMesh.Funnel.PointTestResult.RIGHT : MyNavigationMesh.Funnel.PointTestResult.INSIDE;
      }

      private void RecalculateLeftPlane()
      {
        Vector3 vector1 = this.m_leftPoint - this.m_apex;
        double num1 = (double) vector1.Normalize();
        this.m_leftPlaneNormal = Vector3.Cross(vector1, this.m_apexNormal);
        double num2 = (double) this.m_leftPlaneNormal.Normalize();
        this.m_leftD = -this.m_leftPoint.Dot(this.m_leftPlaneNormal);
      }

      private void RecalculateRightPlane()
      {
        Vector3 vector2 = this.m_rightPoint - this.m_apex;
        double num1 = (double) vector2.Normalize();
        this.m_rightPlaneNormal = Vector3.Cross(this.m_apexNormal, vector2);
        double num2 = (double) this.m_rightPlaneNormal.Normalize();
        this.m_rightD = -this.m_rightPoint.Dot(this.m_rightPlaneNormal);
      }

      private enum PointTestResult
      {
        LEFT,
        INSIDE,
        RIGHT,
      }
    }

    public struct FunnelState
    {
      public Vector3 Apex;
      public Vector3 Left;
      public Vector3 Right;
    }
  }
}
