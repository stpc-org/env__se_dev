// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.My2DClipmap`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.__helper_namespace;
using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public class My2DClipmap<THandler> where THandler : class, IMy2DClipmapNodeHandler, new()
  {
    private int m_root;
    private double m_size;
    private int m_splits;
    private double[] m_lodSizes;
    private double[] m_keepLodSizes;
    private MyFreeList<THandler> m_leafHandlers;
    private Node[] m_nodes;
    private THandler[] m_nodeHandlers;
    private int m_firstFree;
    private const int NullNode = -2147483648;
    public int NodeAllocDeallocs;
    private readonly List<int> m_nodesToDealloc = new List<int>();
    private IMy2DClipmapManager m_manager;
    private readonly Stack<My2DClipmap<THandler>.StackInfo> m_nodesToScanNext = new Stack<My2DClipmap<THandler>.StackInfo>();
    private BoundingBox2D[] m_queryBounds;
    private BoundingBox2D[] m_keepBounds;
    private readonly BoundingBox2D[] m_nodeBoundsTmp = new BoundingBox2D[4];
    private readonly IMy2DClipmapNodeHandler[] m_tmpNodeHandlerList = new IMy2DClipmapNodeHandler[4];

    public Vector3D LastPosition { get; set; }

    public MatrixD WorldMatrix { get; private set; }

    public MatrixD InverseWorldMatrix { get; private set; }

    public double FaceHalf => this.m_lodSizes[this.m_splits];

    public double LeafSize => this.m_lodSizes[1];

    public int Depth => this.m_splits;

    private unsafe void PrepareAllocator()
    {
      int length = 16;
      this.m_nodes = new Node[length];
      fixed (Node* nodePtr = this.m_nodes)
      {
        for (int index = 0; index < length; ++index)
          nodePtr[index].Lod = ~(index + 1);
      }
      this.m_firstFree = 0;
      this.m_nodeHandlers = new THandler[length];
      this.m_leafHandlers = new MyFreeList<THandler>();
    }

    private unsafe int AllocNode()
    {
      ++this.NodeAllocDeallocs;
      if (this.m_firstFree == this.m_nodes.Length)
      {
        int length = this.m_nodes.Length;
        Array.Resize<Node>(ref this.m_nodes, this.m_nodes.Length << 1);
        Array.Resize<THandler>(ref this.m_nodeHandlers, this.m_nodes.Length);
        fixed (Node* nodePtr = this.m_nodes)
        {
          for (int index = length; index < this.m_nodes.Length; ++index)
            nodePtr[index].Lod = ~(index + 1);
        }
        this.m_firstFree = length;
      }
      int firstFree = this.m_firstFree;
      fixed (Node* nodePtr = this.m_nodes)
      {
        for (int index = 0; index < 4; ++index)
          nodePtr[this.m_firstFree].Children[index] = int.MinValue;
        this.m_firstFree = ~nodePtr[this.m_firstFree].Lod;
      }
      return firstFree;
    }

    private unsafe void FreeNode(int node)
    {
      ++this.NodeAllocDeallocs;
      fixed (Node* nodePtr = this.m_nodes)
      {
        nodePtr[node].Lod = ~this.m_firstFree;
        this.m_firstFree = node;
        this.m_nodeHandlers[node] = default (THandler);
      }
    }

    private void Compact()
    {
    }

    private unsafe int Child(int node, int index)
    {
      fixed (Node* nodePtr = this.m_nodes)
        return nodePtr[node].Children[index];
    }

    private unsafe void CollapseSubtree(int parent, int childIndex, Node* nodes)
    {
      int parent1 = nodes[parent].Children[childIndex];
      this.m_nodesToDealloc.Add(parent1);
      Node* nodePtr = nodes + parent1;
      for (int childIndex1 = 0; childIndex1 < 4; ++childIndex1)
      {
        if (nodePtr->Children[childIndex1] >= 0)
          this.CollapseSubtree(parent1, childIndex1, nodes);
      }
      IMy2DClipmapNodeHandler[] tmpNodeHandlerList = this.m_tmpNodeHandlerList;
      for (int index = 0; index < 4; ++index)
        tmpNodeHandlerList[index] = (IMy2DClipmapNodeHandler) this.m_leafHandlers[~nodePtr->Children[index]];
      THandler nodeHandler = this.m_nodeHandlers[parent1];
      nodeHandler.InitJoin(tmpNodeHandlerList);
      for (int index = 0; index < 4; ++index)
      {
        this.m_leafHandlers.Free(~nodePtr->Children[index]);
        tmpNodeHandlerList[index].Close();
      }
      int index1 = this.m_leafHandlers.Allocate();
      nodes[parent].Children[childIndex] = ~index1;
      this.m_leafHandlers[index1] = nodeHandler;
    }

    private unsafe void CollapseRoot()
    {
      fixed (Node* nodes = this.m_nodes)
      {
        Node* nodePtr = nodes + this.m_root;
        // ISSUE: reference to a compiler-generated field
        if (nodePtr->Children.FixedElementField == int.MinValue)
          return;
        for (int childIndex = 0; childIndex < 4; ++childIndex)
        {
          if (nodePtr->Children[childIndex] >= 0)
            this.CollapseSubtree(this.m_root, childIndex, nodes);
        }
        IMy2DClipmapNodeHandler[] tmpNodeHandlerList = this.m_tmpNodeHandlerList;
        for (int index = 0; index < 4; ++index)
          tmpNodeHandlerList[index] = (IMy2DClipmapNodeHandler) this.m_leafHandlers[~nodePtr->Children[index]];
        this.m_nodeHandlers[this.m_root].InitJoin(tmpNodeHandlerList);
        for (int index = 0; index < 4; ++index)
        {
          this.m_leafHandlers.Free(~nodePtr->Children[index]);
          tmpNodeHandlerList[index].Close();
          nodePtr->Children[index] = int.MinValue;
        }
      }
      foreach (int node in this.m_nodesToDealloc)
        this.FreeNode(node);
      this.m_nodesToDealloc.Clear();
    }

    public unsafe void Init(
      IMy2DClipmapManager manager,
      ref MatrixD worldMatrix,
      double sectorSize,
      double faceSize)
    {
      this.m_manager = manager;
      this.WorldMatrix = worldMatrix;
      Matrix matrix = Matrix.Invert((Matrix) ref worldMatrix);
      this.InverseWorldMatrix = (MatrixD) ref matrix;
      this.m_size = faceSize;
      this.m_splits = Math.Max(MathHelper.Log2Floor((int) (faceSize / sectorSize)), 1);
      this.m_lodSizes = new double[this.m_splits + 1];
      for (int index = 0; index <= this.m_splits; ++index)
        this.m_lodSizes[this.m_splits - index] = faceSize / (double) (1 << index + 1);
      this.m_keepLodSizes = new double[this.m_splits + 1];
      for (int index = 0; index <= this.m_splits; ++index)
        this.m_keepLodSizes[index] = 1.5 * this.m_lodSizes[index];
      this.m_queryBounds = new BoundingBox2D[this.m_splits + 1];
      this.m_keepBounds = new BoundingBox2D[this.m_splits + 1];
      this.PrepareAllocator();
      this.m_root = this.AllocNode();
      fixed (Node* nodePtr = this.m_nodes)
        nodePtr[this.m_root].Lod = this.m_splits;
      BoundingBox2D bounds = new BoundingBox2D(new Vector2D(-faceSize / 2.0), new Vector2D(faceSize / 2.0));
      this.m_nodeHandlers[this.m_root] = new THandler();
      this.m_nodeHandlers[this.m_root].Init(this.m_manager, 0, 0, this.m_splits, ref bounds);
    }

    public unsafe void Update(Vector3D localPosition)
    {
      double num1 = localPosition.Z * 0.1;
      double num2 = num1 * num1;
      if (Vector3D.DistanceSquared(this.LastPosition, localPosition) < num2)
        return;
      this.LastPosition = localPosition;
      Vector2D vector2D = new Vector2D(localPosition.X, localPosition.Y);
      for (int splits = this.m_splits; splits >= 0; --splits)
      {
        this.m_queryBounds[splits] = new BoundingBox2D(vector2D - this.m_lodSizes[splits], vector2D + this.m_lodSizes[splits]);
        this.m_keepBounds[splits] = new BoundingBox2D(vector2D - this.m_keepLodSizes[splits], vector2D + this.m_keepLodSizes[splits]);
      }
      if (localPosition.Z > this.m_keepLodSizes[this.m_splits])
      {
        if (this.Child(this.m_root, 0) != int.MinValue)
          this.CollapseRoot();
      }
      else
      {
        this.m_nodesToScanNext.Push(new My2DClipmap<THandler>.StackInfo(this.m_root, Vector2D.Zero, this.m_size / 2.0, this.m_splits));
        fixed (BoundingBox2D* childBoxes = this.m_nodeBoundsTmp)
          fixed (BoundingBox2D* boundingBox2DPtr1 = this.m_keepBounds)
            fixed (BoundingBox2D* boundingBox2DPtr2 = this.m_queryBounds)
            {
              while (this.m_nodesToScanNext.Count != 0)
              {
                My2DClipmap<THandler>.StackInfo stackInfo = this.m_nodesToScanNext.Pop();
                double size = stackInfo.Size / 2.0;
                int lod = stackInfo.Lod - 1;
                int num3 = 0;
                for (int index = 0; index < 4; ++index)
                {
                  childBoxes[index].Min = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[index] * stackInfo.Size - stackInfo.Size;
                  childBoxes[index].Max = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[index] * stackInfo.Size;
                  if (childBoxes[index].Intersects(ref boundingBox2DPtr2[lod]) && localPosition.Z <= boundingBox2DPtr2[lod].Height)
                    num3 |= 1 << index;
                }
                if (this.Child(stackInfo.Node, 0) == int.MinValue)
                {
                  THandler nodeHandler = this.m_nodeHandlers[stackInfo.Node];
                  IMy2DClipmapNodeHandler[] tmpNodeHandlerList = this.m_tmpNodeHandlerList;
                  fixed (Node* nodePtr = this.m_nodes)
                  {
                    for (int index1 = 0; index1 < 4; ++index1)
                    {
                      int index2 = this.m_leafHandlers.Allocate();
                      this.m_leafHandlers[index2] = new THandler();
                      tmpNodeHandlerList[index1] = (IMy2DClipmapNodeHandler) this.m_leafHandlers[index2];
                      nodePtr[stackInfo.Node].Children[index1] = ~index2;
                    }
                  }
                  nodeHandler.Split(childBoxes, ref tmpNodeHandlerList);
                  nodeHandler.Close();
                }
                if (stackInfo.Lod != 1)
                {
                  for (int index = 0; index < 4; ++index)
                  {
                    int node = this.Child(stackInfo.Node, index);
                    if ((num3 & 1 << index) != 0)
                    {
                      if (node < 0)
                      {
                        THandler handler = this.m_leafHandlers[~node];
                        this.m_leafHandlers.Free(~node);
                        node = this.AllocNode();
                        this.m_nodeHandlers[node] = handler;
                        fixed (Node* nodePtr = this.m_nodes)
                        {
                          nodePtr[node].Lod = lod;
                          nodePtr[stackInfo.Node].Children[index] = node;
                        }
                      }
                    }
                    else if (node >= 0 && (!childBoxes[index].Intersects(ref boundingBox2DPtr1[lod]) || localPosition.Z > boundingBox2DPtr1[lod].Height))
                    {
                      fixed (Node* nodes = this.m_nodes)
                        this.CollapseSubtree(stackInfo.Node, index, nodes);
                    }
                    if (node >= 0)
                      this.m_nodesToScanNext.Push(new My2DClipmap<THandler>.StackInfo(node, stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[index] * stackInfo.Size - size, size, lod));
                  }
                }
              }
            }
      }
      foreach (int node in this.m_nodesToDealloc)
        this.FreeNode(node);
      this.m_nodesToDealloc.Clear();
    }

    public unsafe THandler GetHandler(Vector2D point)
    {
      this.m_nodesToScanNext.Push(new My2DClipmap<THandler>.StackInfo(this.m_root, Vector2D.Zero, this.m_size / 2.0, this.m_splits));
      int node = this.m_root;
      fixed (Node* nodePtr = this.m_nodes)
      {
        while (this.m_nodesToScanNext.Count != 0)
        {
          My2DClipmap<THandler>.StackInfo stackInfo = this.m_nodesToScanNext.Pop();
          double size = stackInfo.Size / 2.0;
          int lod = stackInfo.Lod - 1;
          for (int index = 0; index < 4; ++index)
          {
            BoundingBox2D boundingBox2D;
            boundingBox2D.Min = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[index] * stackInfo.Size - stackInfo.Size;
            boundingBox2D.Max = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[index] * stackInfo.Size;
            if (boundingBox2D.Contains(point) != ContainmentType.Disjoint)
            {
              int num = nodePtr[stackInfo.Node].Children[index];
              if (num != int.MinValue)
              {
                node = num;
                if (lod > 0 && num >= 0)
                  this.m_nodesToScanNext.Push(new My2DClipmap<THandler>.StackInfo(node, stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[index] * stackInfo.Size - size, size, lod));
              }
            }
          }
        }
      }
      return node < 0 ? this.m_leafHandlers[~node] : this.m_nodeHandlers[node];
    }

    public void Clear() => this.CollapseRoot();

    private struct StackInfo
    {
      public int Node;
      public Vector2D Center;
      public double Size;
      public int Lod;

      public StackInfo(int node, Vector2D center, double size, int lod)
      {
        this.Node = node;
        this.Center = center;
        this.Size = size;
        this.Lod = lod;
      }
    }
  }
}
