// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.PhysicsStepOptimizerBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;

namespace Sandbox.Engine.Physics
{
  internal abstract class PhysicsStepOptimizerBase : IPhysicsStepOptimizer
  {
    public abstract void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings);

    public abstract void DisableOptimizations();

    public abstract void Unload();

    protected static void ForEverySignificantWorld(
      List<MyTuple<HkWorld, MyTimeSpan>> timings,
      Action<HkWorld> action)
    {
      double num1 = 0.0;
      foreach (MyTuple<HkWorld, MyTimeSpan> timing in timings)
        num1 += timing.Item2.Milliseconds;
      double num2 = num1 / (double) timings.Count;
      foreach (MyTuple<HkWorld, MyTimeSpan> timing in timings)
      {
        if (timing.Item2.Milliseconds >= num2)
          action(timing.Item1);
      }
    }

    protected static void ForEveryActivePhysicsBodyOfType<TBody>(
      HkWorld world,
      Action<TBody> action)
      where TBody : class
    {
      foreach (HkEntity activeRigidBody in world.ActiveRigidBodies)
      {
        if (activeRigidBody.UserObject is TBody userObject)
          action(userObject);
      }
    }
  }
}
