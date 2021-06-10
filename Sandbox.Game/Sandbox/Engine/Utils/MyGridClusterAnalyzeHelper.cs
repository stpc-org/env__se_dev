// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyGridClusterAnalyzeHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
  public class MyGridClusterAnalyzeHelper
  {
    public float GetHighestHeatPoint(out Vector3D heatPoint, double heatRange = 3000.0)
    {
      MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
      Vector3D min1 = new Vector3D(double.MaxValue);
      Vector3D max1 = new Vector3D(double.MinValue);
      List<Vector3D> vector3DList = new List<Vector3D>();
      double radius = heatRange;
      foreach (MyEntity myEntity in entities)
      {
        if (myEntity is MyCubeGrid myCubeGrid && !myCubeGrid.IsStatic)
        {
          Vector3D position = myEntity.PositionComp.GetPosition();
          this.ExpandMinMax(ref min1, ref max1, position);
          vector3DList.Add(position);
        }
      }
      Vector3D min2 = min1 - radius;
      Vector3D max2 = max1 + radius;
      MyGridClusterAnalyzeHelper.OctreeHeatmap octreeHeatmap = new MyGridClusterAnalyzeHelper.OctreeHeatmap();
      octreeHeatmap.BuildRootFromBounds(min2, max2);
      foreach (Vector3D position in vector3DList)
        octreeHeatmap.Root.WriteQueryRecursive(1f, ref position, radius);
      return octreeHeatmap.GetHighestHeat(out heatPoint);
    }

    public static void TEST_01()
    {
      MyGridClusterAnalyzeHelper.OctreeHeatmap octreeHeatmap = new MyGridClusterAnalyzeHelper.OctreeHeatmap();
      Vector3D min = new Vector3D(0.0, 0.0, 0.0);
      Vector3D max = new Vector3D(300.0, 300.0, 300.0);
      octreeHeatmap.BuildRootFromBounds(min, max);
      min = new Vector3D(0.0, 0.0, 0.0);
      max = new Vector3D(600.0, 600.0, 600.0);
      octreeHeatmap.BuildRootFromBounds(min, max);
      min = new Vector3D(-1000.0, -1000.0, -1000.0);
      max = new Vector3D(999.0, 999.0, 999.0);
      octreeHeatmap.BuildRootFromBounds(min, max);
      Vector3D position = new Vector3D(250.0, 250.0, 250.0);
      octreeHeatmap.Root.WriteQueryRecursive(1f, ref position, 501.0);
      octreeHeatmap.Root.DebugPrintChildrenDraw();
      octreeHeatmap.Root.DebugPrintChildrenRecursion("");
    }

    public static void TEST_02()
    {
      MyGridClusterAnalyzeHelper.OctreeHeatmap octreeHeatmap = new MyGridClusterAnalyzeHelper.OctreeHeatmap();
      octreeHeatmap.BuildRootFromBounds(new Vector3D(0.0, 0.0, 0.0), new Vector3D(10000.0, 10000.0, 10000.0));
      Vector3D position = new Vector3D(750.0, 750.0, 750.0);
      octreeHeatmap.Root.WriteQueryRecursive(1f, ref position, 3000.0);
      position = new Vector3D(4750.0, 2750.0, 1750.0);
      octreeHeatmap.Root.WriteQueryRecursive(1.5f, ref position, 3000.0);
      position = new Vector3D(2500.0, 1500.0, 3000.0);
      octreeHeatmap.Root.WriteQueryRecursive(2f, ref position, 2000.0);
      octreeHeatmap.DebugDrawTree();
    }

    private void ExpandMinMax(ref Vector3D min, ref Vector3D max, Vector3D position)
    {
      if (position.X < min.X)
        min.X = position.X;
      if (position.Y < min.Y)
        min.Y = position.Y;
      if (position.Z < min.Z)
        min.Z = position.Z;
      if (position.X > max.X)
        max.X = position.X;
      if (position.Y > max.Y)
        max.Y = position.Y;
      if (position.Z <= max.Z)
        return;
      max.Z = position.Z;
    }

    private class OctreeHeatmap
    {
      public MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode Root;

      public void BuildRootFromBounds(Vector3D min, Vector3D max)
      {
        Vector3D vector3D = max - min;
        Vector3D midpoint = 0.5 * (max + min);
        double num1 = double.MinValue;
        if (vector3D.X > num1)
          num1 = vector3D.X;
        if (vector3D.Y > num1)
          num1 = vector3D.Y;
        if (vector3D.Z > num1)
          num1 = vector3D.Z;
        int level = -1;
        for (int index = 0; index < MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNT_AMOUNT; ++index)
        {
          if (num1 < 2.0 * MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[index])
          {
            level = index;
            double num2 = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[index];
            break;
          }
        }
        if (level == -1)
          this.Root = (MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode) null;
        else
          this.Root = new MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode(level, midpoint);
      }

      public void AddValueInSphere(float delta, Vector3D position, double radius)
      {
        Vector3D queryMin = position - radius;
        Vector3D queryMax = position + radius;
        while (!this.Root.IsAABBQueryIInside(queryMin, queryMax))
          this.Root = this.Root.BuildParentByQuery(queryMin, queryMax);
        this.Root.WriteQueryRecursive(delta, ref position, radius);
      }

      public void DebugPrint()
      {
      }

      public void DebugDrawTree()
      {
        if (this.Root == null)
          return;
        Vector3D vector3D1 = this.Root.Midpoint - MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Root.Level];
        double leafSize = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.LEAF_SIZE;
        double num1 = 0.5 * MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.LEAF_SIZE;
        Vector3D vector3D2 = vector3D1 + num1;
        double num2 = 0.5;
        int num3 = 2 << this.Root.Level;
        for (int index1 = 0; index1 < num3; ++index1)
        {
          for (int index2 = 0; index2 < num3; ++index2)
          {
            for (int index3 = 0; index3 < num3; ++index3)
            {
              float sample = this.Root.Sample(vector3D2 + new Vector3D((double) index1 * leafSize, (double) index2 * leafSize, (double) index3 * leafSize));
              if ((double) sample > 0.0)
                MyRenderProxy.DebugDrawPoint(new Vector3D((double) index1 * num2, (double) index2 * num2, (double) index3 * num2), this.ColorDecoder(sample), false, true);
            }
          }
        }
        double num4 = (double) num3 * num2;
        MyRenderProxy.DebugDrawLine3D(Vector3D.Zero, new Vector3D(num4, 0.0, 0.0), Color.Yellow, Color.Yellow, false, true);
        MyRenderProxy.DebugDrawLine3D(Vector3D.Zero, new Vector3D(0.0, num4, 0.0), Color.Yellow, Color.Yellow, false, true);
        MyRenderProxy.DebugDrawLine3D(Vector3D.Zero, new Vector3D(0.0, 0.0, num4), Color.Yellow, Color.Yellow, false, true);
        MyRenderProxy.DebugDrawLine3D(new Vector3D(num4), new Vector3D(0.0, num4, num4), Color.Yellow, Color.Yellow, false, true);
        MyRenderProxy.DebugDrawLine3D(new Vector3D(num4), new Vector3D(num4, 0.0, num4), Color.Yellow, Color.Yellow, false, true);
        MyRenderProxy.DebugDrawLine3D(new Vector3D(num4), new Vector3D(num4, num4, 0.0), Color.Yellow, Color.Yellow, false, true);
      }

      public Color ColorDecoder(float sample)
      {
        if ((double) sample < 1.0)
          return Color.Black;
        if ((double) sample < 2.0)
          return Color.PaleGoldenrod;
        if ((double) sample < 3.0)
          return Color.Yellow;
        if ((double) sample < 4.0)
          return Color.Orange;
        return (double) sample < 5.0 ? Color.OrangeRed : Color.White;
      }

      internal float GetHighestHeat(out Vector3D heatPoint)
      {
        if (this.Root != null)
          return this.Root.GetHighestHeatPoint(out heatPoint);
        heatPoint = Vector3D.Zero;
        return -1f;
      }

      public class OctreeHeatmapNode
      {
        public static readonly double LEAF_SIZE = 500.0;
        public static readonly List<double> PRECOUNTED_HALF_EXTENTS = new List<double>();
        public static readonly int PRECOUNT_AMOUNT = 25;
        public int Level;
        public Vector3D Midpoint;
        public float Value;
        public List<MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode> Children;

        static OctreeHeatmapNode()
        {
          MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS.Add(0.5 * MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.LEAF_SIZE);
          for (int index = 1; index < MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNT_AMOUNT; ++index)
            MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS.Add(2.0 * MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS.Count - 1]);
        }

        public bool IsLeaf => this.Children == null;

        public OctreeHeatmapNode(int level, Vector3D midpoint)
        {
          this.Level = level;
          this.Midpoint = midpoint;
        }

        public bool IsPointInside(Vector3D point)
        {
          double num = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          Vector3D vector3D1 = this.Midpoint - num;
          Vector3D vector3D2 = this.Midpoint + num;
          return point.X >= vector3D1.X && point.Y >= vector3D1.Y && (point.Z >= vector3D1.Z && point.X < vector3D2.X) && (point.Y < vector3D2.Y && point.Z < vector3D2.Z);
        }

        public Vector3D GetNewMidpointOfParent(int index)
        {
          int num1 = index;
          double num2 = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          Vector3D vector3D = new Vector3D();
          vector3D.Z = num1 % 2 != 1 ? this.Midpoint.Z - num2 : this.Midpoint.Z + num2;
          int num3 = num1 / 2;
          vector3D.Y = num3 % 2 != 1 ? this.Midpoint.Y - num2 : this.Midpoint.Y + num2;
          vector3D.X = num3 / 2 % 2 != 1 ? this.Midpoint.X - num2 : this.Midpoint.X + num2;
          return vector3D;
        }

        public Vector3D GetNewMidpointOfChild(int index)
        {
          if (this.Level == 0)
            return Vector3D.Zero;
          int num1 = index;
          double num2 = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level - 1];
          Vector3D vector3D = new Vector3D();
          vector3D.Z = num1 % 2 != 1 ? this.Midpoint.Z - num2 : this.Midpoint.Z + num2;
          int num3 = num1 / 2;
          vector3D.Y = num3 % 2 != 1 ? this.Midpoint.Y - num2 : this.Midpoint.Y + num2;
          vector3D.X = num3 / 2 % 2 != 1 ? this.Midpoint.X - num2 : this.Midpoint.X + num2;
          return vector3D;
        }

        internal int GetChildIndex(Vector3D position)
        {
          int num = 0;
          if (position.X >= this.Midpoint.X)
            num += 4;
          if (position.Y >= this.Midpoint.Y)
            num += 2;
          if (position.Z >= this.Midpoint.Z)
            ++num;
          return num;
        }

        public bool BuildChildren()
        {
          if (this.Level == 0)
            return false;
          this.Children = new List<MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode>();
          for (int index = 0; index < 8; ++index)
            this.Children.Add(new MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode(this.Level - 1, this.GetNewMidpointOfChild(index))
            {
              Value = this.Value
            });
          this.Value = 0.0f;
          return true;
        }

        internal bool IsAABBQueryIInside(Vector3D queryMin, Vector3D queryMax)
        {
          Vector3D vector3D1 = this.Midpoint - MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          Vector3D vector3D2 = this.Midpoint + MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          return queryMin.X >= vector3D1.X && queryMin.Y >= vector3D1.Y && (queryMin.Z >= vector3D1.Z && queryMax.X < vector3D2.X) && (queryMax.Y < vector3D2.Y && queryMax.Z < vector3D2.Z);
        }

        internal MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode BuildParentByQuery(
          Vector3D queryMin,
          Vector3D queryMax)
        {
          Vector3D vector3D1 = this.Midpoint - MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          Vector3D vector3D2 = this.Midpoint + MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          int index = 0;
          if (queryMin.X < vector3D1.X)
            index += 4;
          if (queryMin.Y < vector3D1.Y)
            index += 2;
          if (queryMin.Z < vector3D1.Z)
            ++index;
          MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode octreeHeatmapNode = new MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode(this.Level + 1, this.GetNewMidpointOfParent(index));
          octreeHeatmapNode.BuildChildren();
          octreeHeatmapNode.Children[index] = this;
          return octreeHeatmapNode;
        }

        public void WriteQueryRecursive(float delta, ref Vector3D position, double radius)
        {
          switch (this.TestQuery(position, radius))
          {
            case MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.MyQueryResult.Contain:
              this.AddValueRecursive(delta);
              break;
            case MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.MyQueryResult.Intersect:
              if (this.Level <= 0)
                break;
              if (this.Children == null)
                this.BuildChildren();
              using (List<MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode>.Enumerator enumerator = this.Children.GetEnumerator())
              {
                while (enumerator.MoveNext())
                  enumerator.Current.WriteQueryRecursive(delta, ref position, radius);
                break;
              }
          }
        }

        private void AddValueRecursive(float delta)
        {
          if (this.Level == 0 || this.Children == null)
          {
            this.Value += delta;
          }
          else
          {
            foreach (MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode child in this.Children)
              child.AddValueRecursive(delta);
          }
        }

        private List<Vector3D> GetCorners()
        {
          double num = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          List<Vector3D> vector3DList = new List<Vector3D>();
          for (int index1 = -1; index1 < 2; index1 += 2)
          {
            for (int index2 = -1; index2 < 2; index2 += 2)
            {
              for (int index3 = -1; index3 < 2; index3 += 2)
                vector3DList.Add(this.Midpoint + new Vector3D((double) index1 * num, (double) index2 * num, (double) index3 * num));
            }
          }
          return vector3DList;
        }

        public bool SphereIntersection(Vector3D position, double radius)
        {
          double num1 = MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          Vector3D vector3D1 = this.Midpoint - num1;
          Vector3D vector3D2 = this.Midpoint + num1;
          double num2 = 0.0;
          if (position.X < vector3D1.X)
            num2 += (vector3D1.X - position.X) * (vector3D1.X - position.X);
          else if (position.X > vector3D2.X)
            num2 += (vector3D2.X - position.X) * (vector3D2.X - position.X);
          if (position.Y < vector3D1.Y)
            num2 += (vector3D1.Y - position.Y) * (vector3D1.Y - position.Y);
          else if (position.Y > vector3D2.Y)
            num2 += (vector3D2.Y - position.Y) * (vector3D2.Y - position.Y);
          if (position.Z < vector3D1.Z)
            num2 += (vector3D1.Z - position.Z) * (vector3D1.Z - position.Z);
          else if (position.Z > vector3D2.Z)
            num2 += (vector3D2.Z - position.Z) * (vector3D2.Z - position.Z);
          return num2 < radius * radius;
        }

        private MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.MyQueryResult TestQuery(
          Vector3D position,
          double radius)
        {
          if (!this.SphereIntersection(position, radius))
            return MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.MyQueryResult.Disjoint;
          List<Vector3D> corners = this.GetCorners();
          int num1 = 0;
          double num2 = radius * radius;
          foreach (Vector3D vector3D in corners)
          {
            if ((vector3D - position).LengthSquared() <= num2)
              ++num1;
          }
          return num1 == corners.Count ? MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.MyQueryResult.Contain : MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.MyQueryResult.Intersect;
        }

        public void DebugPrintChildrenRecursion(string key)
        {
          if (this.IsLeaf)
            return;
          this.DebugPrintChildrenDraw();
          for (int index = 0; index < 8; ++index)
            this.Children[index].DebugPrintChildrenRecursion(key + (object) index);
        }

        public void DebugPrintChildrenDraw()
        {
          int num = this.IsLeaf ? 1 : 0;
        }

        internal float Sample(Vector3D point)
        {
          Vector3D vector3D1 = this.Midpoint - MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          Vector3D vector3D2 = this.Midpoint + MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[this.Level];
          if (point.X < vector3D1.X || point.Y < vector3D1.Y || (point.Z < vector3D1.Z || point.X >= vector3D2.X) || (point.Y >= vector3D2.Y || point.Z >= vector3D2.Z))
            return -1f;
          if (this.Children == null)
            return this.Value;
          int index = 0;
          if (point.X >= this.Midpoint.X)
            index += 4;
          if (point.Y >= this.Midpoint.Y)
            index += 2;
          if (point.Z >= this.Midpoint.Z)
            ++index;
          return this.Children[index].Sample(point);
        }

        internal float GetHighestHeatPoint(out Vector3D heatPoint)
        {
          if (this.Level == 0 || this.Children == null)
          {
            heatPoint = this.Midpoint;
            return this.Value;
          }
          heatPoint = Vector3D.Zero;
          float num = 0.0f;
          foreach (MyGridClusterAnalyzeHelper.OctreeHeatmap.OctreeHeatmapNode child in this.Children)
          {
            Vector3D heatPoint1;
            float highestHeatPoint = child.GetHighestHeatPoint(out heatPoint1);
            if ((double) highestHeatPoint > (double) num)
            {
              heatPoint = heatPoint1;
              num = highestHeatPoint;
            }
          }
          return num;
        }

        public enum MyQueryResult
        {
          Contain,
          Disjoint,
          Intersect,
        }
      }
    }
  }
}
