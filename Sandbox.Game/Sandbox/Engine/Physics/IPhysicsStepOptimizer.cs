// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.IPhysicsStepOptimizer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;

namespace Sandbox.Engine.Physics
{
  internal interface IPhysicsStepOptimizer
  {
    void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings);

    void DisableOptimizations();

    void Unload();
  }
}
