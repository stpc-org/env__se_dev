// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyNavgroupLinks
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
  public class MyNavgroupLinks
  {
    private readonly Dictionary<MyNavigationPrimitive, List<MyNavigationPrimitive>> m_links;

    public MyNavgroupLinks() => this.m_links = new Dictionary<MyNavigationPrimitive, List<MyNavigationPrimitive>>();

    public void AddLink(
      MyNavigationPrimitive primitive1,
      MyNavigationPrimitive primitive2,
      bool onlyIfNotPresent = false)
    {
      this.AddLinkInternal(primitive1, primitive2, onlyIfNotPresent);
      this.AddLinkInternal(primitive2, primitive1, onlyIfNotPresent);
      primitive1.HasExternalNeighbors = true;
      primitive2.HasExternalNeighbors = true;
    }

    public void RemoveLink(MyNavigationPrimitive primitive1, MyNavigationPrimitive primitive2)
    {
      if (this.RemoveLinkInternal(primitive1, primitive2))
        primitive1.HasExternalNeighbors = false;
      if (!this.RemoveLinkInternal(primitive2, primitive1))
        return;
      primitive2.HasExternalNeighbors = false;
    }

    public int GetLinkCount(MyNavigationPrimitive primitive)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive, out navigationPrimitiveList);
      // ISSUE: explicit non-virtual call
      return navigationPrimitiveList == null ? 0 : __nonvirtual (navigationPrimitiveList.Count);
    }

    public MyNavigationPrimitive GetLinkedNeighbor(
      MyNavigationPrimitive primitive,
      int index)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive, out navigationPrimitiveList);
      return navigationPrimitiveList?[index];
    }

    public IMyPathEdge<MyNavigationPrimitive> GetEdge(
      MyNavigationPrimitive primitive,
      int index)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive, out navigationPrimitiveList);
      if (navigationPrimitiveList == null)
        return (IMyPathEdge<MyNavigationPrimitive>) null;
      MyNavigationPrimitive primitive2 = navigationPrimitiveList[index];
      return (IMyPathEdge<MyNavigationPrimitive>) MyNavgroupLinks.PathEdge.GetEdge(primitive, primitive2);
    }

    public List<MyNavigationPrimitive> GetLinks(
      MyNavigationPrimitive primitive)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive, out navigationPrimitiveList);
      return navigationPrimitiveList;
    }

    public void RemoveAllLinks(MyNavigationPrimitive primitive)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList1 = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive, out navigationPrimitiveList1);
      if (navigationPrimitiveList1 == null)
        return;
      foreach (MyNavigationPrimitive key in navigationPrimitiveList1)
      {
        List<MyNavigationPrimitive> navigationPrimitiveList2 = (List<MyNavigationPrimitive>) null;
        this.m_links.TryGetValue(key, out navigationPrimitiveList2);
        if (navigationPrimitiveList2 == null)
          return;
        navigationPrimitiveList2.Remove(primitive);
        if (navigationPrimitiveList2.Count == 0)
          this.m_links.Remove(key);
      }
      this.m_links.Remove(primitive);
    }

    public void DebugDraw(Color lineColor)
    {
      if (!MyFakes.DEBUG_DRAW_NAVMESH_LINKS)
        return;
      foreach (KeyValuePair<MyNavigationPrimitive, List<MyNavigationPrimitive>> link in this.m_links)
      {
        MyNavigationPrimitive key = link.Key;
        List<MyNavigationPrimitive> navigationPrimitiveList = link.Value;
        for (int index = 0; index < navigationPrimitiveList.Count; ++index)
        {
          MyNavigationPrimitive navigationPrimitive = navigationPrimitiveList[index];
          Vector3D worldPosition1 = key.WorldPosition;
          Vector3D worldPosition2 = navigationPrimitive.WorldPosition;
          Vector3D vector3D1 = (worldPosition1 + worldPosition2) * 0.5;
          Vector3D vector3D2 = (vector3D1 + worldPosition1) * 0.5;
          Vector3D up = Vector3D.Up;
          MyRenderProxy.DebugDrawLine3D(worldPosition1, vector3D2 + up * 0.4, lineColor, lineColor, false);
          MyRenderProxy.DebugDrawLine3D(vector3D2 + up * 0.4, vector3D1 + up * 0.5, lineColor, lineColor, false);
        }
      }
    }

    private void AddLinkInternal(
      MyNavigationPrimitive primitive1,
      MyNavigationPrimitive primitive2,
      bool onlyIfNotPresent = false)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive1, out navigationPrimitiveList);
      if (navigationPrimitiveList == null)
      {
        navigationPrimitiveList = new List<MyNavigationPrimitive>();
        this.m_links.Add(primitive1, navigationPrimitiveList);
      }
      if (onlyIfNotPresent)
      {
        if (navigationPrimitiveList.Contains(primitive2))
          return;
        navigationPrimitiveList.Add(primitive2);
      }
      else
        navigationPrimitiveList.Add(primitive2);
    }

    private bool RemoveLinkInternal(
      MyNavigationPrimitive primitive1,
      MyNavigationPrimitive primitive2)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      this.m_links.TryGetValue(primitive1, out navigationPrimitiveList);
      if (navigationPrimitiveList != null)
      {
        navigationPrimitiveList.Remove(primitive2);
        if (navigationPrimitiveList.Count == 0)
        {
          this.m_links.Remove(primitive1);
          return true;
        }
      }
      return false;
    }

    private class PathEdge : IMyPathEdge<MyNavigationPrimitive>
    {
      private static readonly MyNavgroupLinks.PathEdge m_static = new MyNavgroupLinks.PathEdge();
      private MyNavigationPrimitive m_primitive1;
      private MyNavigationPrimitive m_primitive2;

      public static MyNavgroupLinks.PathEdge GetEdge(
        MyNavigationPrimitive primitive1,
        MyNavigationPrimitive primitive2)
      {
        MyNavgroupLinks.PathEdge.m_static.m_primitive1 = primitive1;
        MyNavgroupLinks.PathEdge.m_static.m_primitive2 = primitive2;
        return MyNavgroupLinks.PathEdge.m_static;
      }

      public float GetWeight() => this.m_primitive1.Group == this.m_primitive2.Group ? Vector3.Distance(this.m_primitive1.Position, this.m_primitive2.Position) : (float) Vector3D.Distance(this.m_primitive1.WorldPosition, this.m_primitive2.WorldPosition);

      public MyNavigationPrimitive GetOtherVertex(MyNavigationPrimitive vertex1) => vertex1 == this.m_primitive1 ? this.m_primitive2 : this.m_primitive1;
    }
  }
}
