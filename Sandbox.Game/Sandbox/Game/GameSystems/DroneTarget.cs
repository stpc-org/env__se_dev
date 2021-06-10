// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.DroneTarget
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Entity;

namespace Sandbox.Game.GameSystems
{
  public class DroneTarget : IComparable<DroneTarget>
  {
    public MyEntity Target;
    public int Priority;

    public DroneTarget(MyEntity target)
    {
      this.Target = target;
      this.Priority = 1;
    }

    public DroneTarget(MyEntity target, int priority)
    {
      this.Target = target;
      this.Priority = priority;
    }

    public int CompareTo(DroneTarget other) => this.Priority.CompareTo(other.Priority);
  }
}
