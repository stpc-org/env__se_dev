// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.IMyNavigationGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public interface IMyNavigationGroup
  {
    int GetExternalNeighborCount(MyNavigationPrimitive primitive);

    MyNavigationPrimitive GetExternalNeighbor(
      MyNavigationPrimitive primitive,
      int index);

    IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(
      MyNavigationPrimitive primitive,
      int index);

    void RefinePath(
      MyPath<MyNavigationPrimitive> path,
      List<Vector4D> output,
      ref Vector3 startPoint,
      ref Vector3 endPoint,
      int begin,
      int end);

    Vector3 GlobalToLocal(Vector3D globalPos);

    Vector3D LocalToGlobal(Vector3 localPos);

    IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive);

    MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistanceSq);
  }
}
