// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGravityProviderSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666)]
  public class MyGravityProviderSystem : MySessionComponentBase
  {
    public const float G = 9.81f;
    private static Dictionary<IMyGravityProvider, int> m_proxyIdMap = new Dictionary<IMyGravityProvider, int>();
    private static MyDynamicAABBTreeD m_artificialGravityGenerators = new MyDynamicAABBTreeD(Vector3D.One * 10.0, 10.0);
    private static ConcurrentCachingList<IMyGravityProvider> m_naturalGravityGenerators = new ConcurrentCachingList<IMyGravityProvider>();
    [ThreadStatic]
    private static MyGravityProviderSystem.GravityCollector m_gravityCollector;

    protected override void UnloadData()
    {
      base.UnloadData();
      MyGravityProviderSystem.m_naturalGravityGenerators.ApplyChanges();
      if (MyGravityProviderSystem.m_proxyIdMap.Count <= 0)
      {
        int count = MyGravityProviderSystem.m_naturalGravityGenerators.Count;
      }
      MyGravityProviderSystem.m_proxyIdMap.Clear();
      MyGravityProviderSystem.m_artificialGravityGenerators.Clear();
      MyGravityProviderSystem.m_naturalGravityGenerators.ClearImmediate();
    }

    public static bool IsGravityReady() => !MyGravityProviderSystem.m_artificialGravityGenerators.IsRootNull();

    public static Vector3 CalculateTotalGravityInPoint(Vector3D worldPoint)
    {
      float naturalGravityMultiplier;
      return MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPoint, out naturalGravityMultiplier) + MyGravityProviderSystem.CalculateArtificialGravityInPoint(worldPoint, MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(naturalGravityMultiplier));
    }

    public static Vector3 CalculateArtificialGravityInPoint(
      Vector3D worldPoint,
      float gravityMultiplier = 1f)
    {
      if ((double) gravityMultiplier == 0.0)
        return Vector3.Zero;
      if (MyGravityProviderSystem.m_gravityCollector == null)
        MyGravityProviderSystem.m_gravityCollector = new MyGravityProviderSystem.GravityCollector();
      MyGravityProviderSystem.m_gravityCollector.Gravity = Vector3.Zero;
      MyGravityProviderSystem.m_gravityCollector.Collect(MyGravityProviderSystem.m_artificialGravityGenerators, ref worldPoint);
      return MyGravityProviderSystem.m_gravityCollector.Gravity * gravityMultiplier;
    }

    public static Vector3 CalculateNaturalGravityInPoint(Vector3D worldPoint) => MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPoint, out float _);

    public static Vector3 CalculateNaturalGravityInPoint(
      Vector3D worldPoint,
      out float naturalGravityMultiplier)
    {
      naturalGravityMultiplier = 0.0f;
      Vector3 zero = Vector3.Zero;
      MyGravityProviderSystem.m_naturalGravityGenerators.ApplyChanges();
      foreach (IMyGravityProvider gravityGenerator in MyGravityProviderSystem.m_naturalGravityGenerators)
      {
        if (gravityGenerator.IsPositionInRange(worldPoint))
        {
          Vector3 worldGravity = gravityGenerator.GetWorldGravity(worldPoint);
          float gravityMultiplier = gravityGenerator.GetGravityMultiplier(worldPoint);
          if ((double) gravityMultiplier > (double) naturalGravityMultiplier)
            naturalGravityMultiplier = gravityMultiplier;
          zero += worldGravity;
        }
      }
      return zero;
    }

    public static float CalculateHighestNaturalGravityMultiplierInPoint(Vector3D worldPoint)
    {
      float num = 0.0f;
      MyGravityProviderSystem.m_naturalGravityGenerators.ApplyChanges();
      foreach (IMyGravityProvider gravityGenerator in MyGravityProviderSystem.m_naturalGravityGenerators)
      {
        if (gravityGenerator.IsPositionInRange(worldPoint))
        {
          float gravityMultiplier = gravityGenerator.GetGravityMultiplier(worldPoint);
          if ((double) gravityMultiplier > (double) num)
            num = gravityMultiplier;
        }
      }
      return num;
    }

    public static float CalculateArtificialGravityStrengthMultiplier(float naturalGravityMultiplier) => MathHelper.Clamp((float) (1.0 - (double) naturalGravityMultiplier * 2.0), 0.0f, 1f);

    public static double GetStrongestNaturalGravityWell(
      Vector3D worldPosition,
      out IMyGravityProvider nearestProvider)
    {
      double num1 = double.MinValue;
      nearestProvider = (IMyGravityProvider) null;
      MyGravityProviderSystem.m_naturalGravityGenerators.ApplyChanges();
      foreach (IMyGravityProvider gravityGenerator in MyGravityProviderSystem.m_naturalGravityGenerators)
      {
        float num2 = gravityGenerator.GetWorldGravity(worldPosition).Length();
        if ((double) num2 > num1)
        {
          num1 = (double) num2;
          nearestProvider = gravityGenerator;
        }
      }
      return num1;
    }

    public static bool IsPositionInNaturalGravity(Vector3D position, double sphereSize = 0.0)
    {
      sphereSize = MathHelper.Max(sphereSize, 0.0);
      MyGravityProviderSystem.m_naturalGravityGenerators.ApplyChanges();
      foreach (IMyGravityProvider gravityGenerator in MyGravityProviderSystem.m_naturalGravityGenerators)
      {
        if (gravityGenerator != null && gravityGenerator.IsPositionInRange(position))
          return true;
      }
      return false;
    }

    public static bool DoesTrajectoryIntersectNaturalGravity(
      Vector3D start,
      Vector3D end,
      double raySize = 0.0)
    {
      Vector3D vector3D = start - end;
      if (Vector3D.IsZero(vector3D))
        return MyGravityProviderSystem.IsPositionInNaturalGravity(start, raySize);
      Ray ray = new Ray((Vector3) start, Vector3.Normalize(vector3D));
      raySize = MathHelper.Max(raySize, 0.0);
      MyGravityProviderSystem.m_naturalGravityGenerators.ApplyChanges();
      foreach (IMyGravityProvider gravityGenerator in MyGravityProviderSystem.m_naturalGravityGenerators)
      {
        if (gravityGenerator != null && gravityGenerator is MySphericalNaturalGravityComponent gravityComponent)
        {
          BoundingSphereD boundingSphereD = new BoundingSphereD(gravityComponent.Position, (double) gravityComponent.GravityLimit + raySize);
          if (ray.Intersects((BoundingSphere) boundingSphereD).HasValue)
            return true;
        }
      }
      return false;
    }

    public static void AddGravityGenerator(IMyGravityProvider gravityGenerator)
    {
      if (MyGravityProviderSystem.m_proxyIdMap.ContainsKey(gravityGenerator))
        return;
      BoundingBoxD aabb;
      gravityGenerator.GetProxyAABB(out aabb);
      int num = MyGravityProviderSystem.m_artificialGravityGenerators.AddProxy(ref aabb, (object) gravityGenerator, 0U);
      MyGravityProviderSystem.m_proxyIdMap.Add(gravityGenerator, num);
    }

    public static void RemoveGravityGenerator(IMyGravityProvider gravityGenerator)
    {
      int proxyId;
      if (!MyGravityProviderSystem.m_proxyIdMap.TryGetValue(gravityGenerator, out proxyId))
        return;
      MyGravityProviderSystem.m_artificialGravityGenerators.RemoveProxy(proxyId);
      MyGravityProviderSystem.m_proxyIdMap.Remove(gravityGenerator);
    }

    public static void OnGravityGeneratorMoved(
      IMyGravityProvider gravityGenerator,
      ref Vector3 velocity)
    {
      int proxyId;
      if (!MyGravityProviderSystem.m_proxyIdMap.TryGetValue(gravityGenerator, out proxyId))
        return;
      BoundingBoxD aabb;
      gravityGenerator.GetProxyAABB(out aabb);
      MyGravityProviderSystem.m_artificialGravityGenerators.MoveProxy(proxyId, ref aabb, (Vector3D) velocity);
    }

    public static void AddNaturalGravityProvider(IMyGravityProvider gravityGenerator) => MyGravityProviderSystem.m_naturalGravityGenerators.Add(gravityGenerator);

    public static void RemoveNaturalGravityProvider(IMyGravityProvider gravityGenerator) => MyGravityProviderSystem.m_naturalGravityGenerators.Remove(gravityGenerator);

    private class GravityCollector
    {
      public Vector3 Gravity;
      private readonly Func<int, bool> CollectAction;
      private Vector3D WorldPoint;
      private MyDynamicAABBTreeD Tree;

      public GravityCollector() => this.CollectAction = new Func<int, bool>(this.CollectCallback);

      public void Collect(MyDynamicAABBTreeD tree, ref Vector3D worldPoint)
      {
        this.Tree = tree;
        this.WorldPoint = worldPoint;
        tree.QueryPoint(this.CollectAction, ref worldPoint);
      }

      private bool CollectCallback(int proxyId)
      {
        IMyGravityProvider userData = this.Tree.GetUserData<IMyGravityProvider>(proxyId);
        if (userData.IsWorking && userData.IsPositionInRange(this.WorldPoint))
          this.Gravity += userData.GetWorldGravity(this.WorldPoint);
        return true;
      }
    }
  }
}
