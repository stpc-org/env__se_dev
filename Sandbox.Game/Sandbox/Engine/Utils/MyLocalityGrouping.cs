// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyLocalityGrouping
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  public class MyLocalityGrouping
  {
    public MyLocalityGrouping.GroupingMode Mode;
    private SortedSet<MyLocalityGrouping.InstanceInfo> m_instances = new SortedSet<MyLocalityGrouping.InstanceInfo>((IComparer<MyLocalityGrouping.InstanceInfo>) new MyLocalityGrouping.InstanceInfoComparer());

    private int TimeMs => MySandboxGame.TotalGamePlayTimeInMilliseconds;

    public MyLocalityGrouping(MyLocalityGrouping.GroupingMode mode) => this.Mode = mode;

    public bool AddInstance(TimeSpan lifeTime, Vector3 position, float radius, bool removeOld = true)
    {
      if (removeOld)
        this.RemoveOld();
      foreach (MyLocalityGrouping.InstanceInfo instance in this.m_instances)
      {
        float num = this.Mode == MyLocalityGrouping.GroupingMode.ContainsCenter ? Math.Max(radius, instance.Radius) : radius + instance.Radius;
        if ((double) Vector3.DistanceSquared(position, instance.Position) < (double) num * (double) num)
          return false;
      }
      this.m_instances.Add(new MyLocalityGrouping.InstanceInfo()
      {
        EndTimeMs = this.TimeMs + (int) lifeTime.TotalMilliseconds,
        Position = position,
        Radius = radius
      });
      return true;
    }

    public void RemoveOld()
    {
      int timeMs = this.TimeMs;
      while (this.m_instances.Count > 0 && this.m_instances.Min.EndTimeMs < timeMs)
        this.m_instances.Remove(this.m_instances.Min);
    }

    public void Clear() => this.m_instances.Clear();

    public enum GroupingMode
    {
      ContainsCenter,
      Overlaps,
    }

    private struct InstanceInfo
    {
      public Vector3 Position;
      public float Radius;
      public int EndTimeMs;
    }

    private class InstanceInfoComparer : IComparer<MyLocalityGrouping.InstanceInfo>
    {
      public int Compare(MyLocalityGrouping.InstanceInfo x, MyLocalityGrouping.InstanceInfo y) => x.EndTimeMs - y.EndTimeMs;
    }
  }
}
