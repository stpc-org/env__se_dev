// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyRadioBroadcasters
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  internal static class MyRadioBroadcasters
  {
    private static MyDynamicAABBTreeD m_aabbTree = new MyDynamicAABBTreeD(MyConstants.GAME_PRUNING_STRUCTURE_AABB_EXTENSION);

    public static void AddBroadcaster(MyRadioBroadcaster broadcaster)
    {
      if (broadcaster.RadioProxyID != -1)
        return;
      BoundingBoxD fromSphere = BoundingBoxD.CreateFromSphere(new BoundingSphereD(broadcaster.BroadcastPosition, (double) broadcaster.BroadcastRadius));
      broadcaster.RadioProxyID = MyRadioBroadcasters.m_aabbTree.AddProxy(ref fromSphere, (object) broadcaster, 0U);
    }

    public static void RemoveBroadcaster(MyRadioBroadcaster broadcaster)
    {
      if (broadcaster.RadioProxyID == -1)
        return;
      MyRadioBroadcasters.m_aabbTree.RemoveProxy(broadcaster.RadioProxyID);
      broadcaster.RadioProxyID = -1;
    }

    public static void MoveBroadcaster(MyRadioBroadcaster broadcaster)
    {
      if (broadcaster.RadioProxyID == -1)
        return;
      BoundingBoxD fromSphere = BoundingBoxD.CreateFromSphere(new BoundingSphereD(broadcaster.BroadcastPosition, (double) broadcaster.BroadcastRadius));
      MyRadioBroadcasters.m_aabbTree.MoveProxy(broadcaster.RadioProxyID, ref fromSphere, (Vector3D) Vector3.Zero);
    }

    public static void Clear() => MyRadioBroadcasters.m_aabbTree.Clear();

    public static void GetAllBroadcastersInSphere(
      BoundingSphereD sphere,
      List<MyDataBroadcaster> result)
    {
      MyRadioBroadcasters.m_aabbTree.OverlapAllBoundingSphere<MyDataBroadcaster>(ref sphere, result, false);
      for (int index = result.Count - 1; index >= 0; --index)
      {
        if (result[index] is MyRadioBroadcaster radioBroadcaster && radioBroadcaster.Entity != null)
        {
          double num1 = sphere.Radius + (double) radioBroadcaster.BroadcastRadius;
          double num2 = num1 * num1;
          if (Vector3D.DistanceSquared(sphere.Center, radioBroadcaster.BroadcastPosition) > num2)
            result.RemoveAtFast<MyDataBroadcaster>(index);
        }
      }
    }

    public static void DebugDraw()
    {
      List<MyRadioBroadcaster> elementsList = new List<MyRadioBroadcaster>();
      List<BoundingBoxD> boxsList = new List<BoundingBoxD>();
      MyRadioBroadcasters.m_aabbTree.GetAll<MyRadioBroadcaster>(elementsList, true, boxsList);
      for (int index = 0; index < elementsList.Count; ++index)
        MyRenderProxy.DebugDrawSphere(elementsList[index].BroadcastPosition, elementsList[index].BroadcastRadius, Color.White, depthRead: false);
    }
  }
}
