// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyIntersectionResultLineBoundingSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;

namespace Sandbox.Engine.Utils
{
  internal struct MyIntersectionResultLineBoundingSphere
  {
    public readonly float Distance;
    public readonly MyEntity PhysObject;

    public MyIntersectionResultLineBoundingSphere(float distance, MyEntity physObject)
    {
      this.Distance = distance;
      this.PhysObject = physObject;
    }

    public static MyIntersectionResultLineBoundingSphere? GetCloserIntersection(
      ref MyIntersectionResultLineBoundingSphere? a,
      ref MyIntersectionResultLineBoundingSphere? b)
    {
      return !a.HasValue && b.HasValue || a.HasValue && b.HasValue && (double) b.Value.Distance < (double) a.Value.Distance ? b : a;
    }
  }
}
