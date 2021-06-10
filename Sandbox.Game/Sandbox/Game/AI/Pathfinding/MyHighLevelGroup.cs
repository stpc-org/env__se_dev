// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyHighLevelGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
  public class MyHighLevelGroup : MyPathFindingSystem<MyNavigationPrimitive>, IMyNavigationGroup
  {
    private readonly Dictionary<int, MyHighLevelPrimitive> m_primitives;
    private readonly Dictionary<int, List<IMyHighLevelPrimitiveObserver>> m_primitiveObservers;
    private readonly MyNavgroupLinks m_links;
    private int m_removingPrimitive = -1;
    private static readonly List<int> m_tmpNeighbors = new List<int>();

    public IMyNavigationGroup LowLevelGroup { get; }

    public MyHighLevelGroup(
      IMyNavigationGroup lowLevelPathfinding,
      MyNavgroupLinks links,
      Func<long> timestampFunction)
      : base(timestampFunction: timestampFunction)
    {
      this.LowLevelGroup = lowLevelPathfinding;
      this.m_primitives = new Dictionary<int, MyHighLevelPrimitive>();
      this.m_primitiveObservers = new Dictionary<int, List<IMyHighLevelPrimitiveObserver>>();
      this.m_links = links;
    }

    public override string ToString() => this.LowLevelGroup == null ? "Invalid HLPFG" : "HLPFG of " + (object) this.LowLevelGroup;

    public void AddPrimitive(int index, Vector3 localPosition) => this.m_primitives.Add(index, new MyHighLevelPrimitive(this, index, localPosition));

    public MyHighLevelPrimitive TryGetPrimitive(int index)
    {
      MyHighLevelPrimitive highLevelPrimitive = (MyHighLevelPrimitive) null;
      this.m_primitives.TryGetValue(index, out highLevelPrimitive);
      return highLevelPrimitive;
    }

    public MyHighLevelPrimitive GetPrimitive(int index)
    {
      MyHighLevelPrimitive highLevelPrimitive = (MyHighLevelPrimitive) null;
      this.m_primitives.TryGetValue(index, out highLevelPrimitive);
      return highLevelPrimitive;
    }

    public void RemovePrimitive(int index)
    {
      this.m_removingPrimitive = index;
      MyHighLevelPrimitive highLevelPrimitive1 = (MyHighLevelPrimitive) null;
      if (!this.m_primitives.TryGetValue(index, out highLevelPrimitive1))
      {
        this.m_removingPrimitive = -1;
      }
      else
      {
        List<IMyHighLevelPrimitiveObserver> primitiveObserverList = (List<IMyHighLevelPrimitiveObserver>) null;
        if (this.m_primitiveObservers.TryGetValue(index, out primitiveObserverList))
        {
          foreach (IMyHighLevelPrimitiveObserver primitiveObserver in primitiveObserverList)
            primitiveObserver.Invalidate();
        }
        this.m_primitiveObservers.Remove(index);
        this.m_links.RemoveAllLinks((MyNavigationPrimitive) highLevelPrimitive1);
        MyHighLevelGroup.m_tmpNeighbors.Clear();
        highLevelPrimitive1.GetNeighbours(MyHighLevelGroup.m_tmpNeighbors);
        foreach (int tmpNeighbor in MyHighLevelGroup.m_tmpNeighbors)
        {
          MyHighLevelPrimitive highLevelPrimitive2 = (MyHighLevelPrimitive) null;
          this.m_primitives.TryGetValue(tmpNeighbor, out highLevelPrimitive2);
          highLevelPrimitive2?.Disconnect(index);
        }
        this.m_primitives.Remove(index);
        this.m_removingPrimitive = -1;
      }
    }

    public void ConnectPrimitives(int a, int b) => this.Connect(a, b);

    public void DisconnectPrimitives(int a, int b) => this.Disconnect(a, b);

    private void Connect(int a, int b)
    {
      MyHighLevelPrimitive primitive1 = this.GetPrimitive(a);
      MyHighLevelPrimitive primitive2 = this.GetPrimitive(b);
      if (primitive1 == null || primitive2 == null)
        return;
      primitive1.Connect(b);
      primitive2.Connect(a);
    }

    private void Disconnect(int a, int b)
    {
      MyHighLevelPrimitive primitive1 = this.GetPrimitive(a);
      MyHighLevelPrimitive primitive2 = this.GetPrimitive(b);
      if (primitive1 == null || primitive2 == null)
        return;
      primitive1.Disconnect(b);
      primitive2.Disconnect(a);
    }

    public MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistanceSq)
    {
      throw new NotImplementedException();
    }

    public int GetExternalNeighborCount(MyNavigationPrimitive primitive) => this.m_links.GetLinkCount(primitive);

    public MyNavigationPrimitive GetExternalNeighbor(
      MyNavigationPrimitive primitive,
      int index)
    {
      return this.m_links.GetLinkedNeighbor(primitive, index);
    }

    public IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(
      MyNavigationPrimitive primitive,
      int index)
    {
      return this.m_links.GetEdge(primitive, index);
    }

    public void RefinePath(
      MyPath<MyNavigationPrimitive> path,
      List<Vector4D> output,
      ref Vector3 startPoint,
      ref Vector3 endPoint,
      int begin,
      int end)
    {
      throw new NotImplementedException();
    }

    public Vector3 GlobalToLocal(Vector3D globalPos) => this.LowLevelGroup.GlobalToLocal(globalPos);

    public Vector3D LocalToGlobal(Vector3 localPos) => this.LowLevelGroup.LocalToGlobal(localPos);

    public void ObservePrimitive(
      MyHighLevelPrimitive primitive,
      IMyHighLevelPrimitiveObserver observer)
    {
      if (primitive.Parent != this)
        return;
      List<IMyHighLevelPrimitiveObserver> primitiveObserverList = (List<IMyHighLevelPrimitiveObserver>) null;
      int index = primitive.Index;
      if (!this.m_primitiveObservers.TryGetValue(index, out primitiveObserverList))
      {
        primitiveObserverList = new List<IMyHighLevelPrimitiveObserver>(4);
        this.m_primitiveObservers.Add(index, primitiveObserverList);
      }
      primitiveObserverList.Add(observer);
    }

    public void StopObservingPrimitive(
      MyHighLevelPrimitive primitive,
      IMyHighLevelPrimitiveObserver observer)
    {
      if (primitive.Parent != this)
        return;
      List<IMyHighLevelPrimitiveObserver> primitiveObserverList = (List<IMyHighLevelPrimitiveObserver>) null;
      int index = primitive.Index;
      if (index == this.m_removingPrimitive || !this.m_primitiveObservers.TryGetValue(index, out primitiveObserverList))
        return;
      primitiveObserverList.Remove(observer);
      if (primitiveObserverList.Count != 0)
        return;
      this.m_primitiveObservers.Remove(index);
    }

    public void DebugDraw(bool lite)
    {
      long highLevelTimestamp = ((MyPathfinding) MyAIComponent.Static.Pathfinding).LastHighLevelTimestamp;
      foreach (KeyValuePair<int, MyHighLevelPrimitive> primitive in this.m_primitives)
      {
        if (lite)
        {
          MyRenderProxy.DebugDrawPoint(primitive.Value.WorldPosition, Color.CadetBlue, false);
        }
        else
        {
          MyHighLevelPrimitive highLevelPrimitive = primitive.Value;
          Vector3D vector3D = MySector.MainCamera.WorldMatrix.Down * 0.300000011920929;
          float num = (float) Vector3D.Distance(highLevelPrimitive.WorldPosition, MySector.MainCamera.Position);
          float scale = 7f / num;
          if ((double) scale > 30.0)
            scale = 30f;
          if ((double) scale < 0.5)
            scale = 0.5f;
          if ((double) num < 100.0)
          {
            List<IMyHighLevelPrimitiveObserver> primitiveObserverList = (List<IMyHighLevelPrimitiveObserver>) null;
            if (this.m_primitiveObservers.TryGetValue(primitive.Key, out primitiveObserverList))
              MyRenderProxy.DebugDrawText3D(highLevelPrimitive.WorldPosition + vector3D, primitiveObserverList.Count.ToString(), Color.Red, scale * 3f, false);
            MyRenderProxy.DebugDrawText3D(highLevelPrimitive.WorldPosition + vector3D, primitive.Key.ToString(), Color.CadetBlue, scale, false);
          }
          for (int index = 0; index < highLevelPrimitive.GetOwnNeighborCount(); ++index)
          {
            MyHighLevelPrimitive ownNeighbor = highLevelPrimitive.GetOwnNeighbor(index) as MyHighLevelPrimitive;
            MyRenderProxy.DebugDrawLine3D(highLevelPrimitive.WorldPosition, ownNeighbor.WorldPosition, Color.CadetBlue, Color.CadetBlue, false);
          }
          if (highLevelPrimitive.PathfindingData.GetTimestamp() == highLevelTimestamp)
            MyRenderProxy.DebugDrawSphere(highLevelPrimitive.WorldPosition, 0.5f, Color.DarkRed, depthRead: false);
        }
      }
    }

    public MyHighLevelGroup HighLevelGroup => (MyHighLevelGroup) null;

    public MyHighLevelPrimitive GetHighLevelPrimitive(
      MyNavigationPrimitive myNavigationTriangle)
    {
      return (MyHighLevelPrimitive) null;
    }

    public IMyHighLevelComponent GetComponent(
      MyHighLevelPrimitive highLevelPrimitive)
    {
      return (IMyHighLevelComponent) null;
    }

    public void GetPrimitivesOnPath(ref List<MyHighLevelPrimitive> primitives)
    {
      foreach (KeyValuePair<int, List<IMyHighLevelPrimitiveObserver>> primitiveObserver in this.m_primitiveObservers)
      {
        MyHighLevelPrimitive primitive = this.TryGetPrimitive(primitiveObserver.Key);
        primitives.Add(primitive);
      }
    }
  }
}
