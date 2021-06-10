// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MySteeringBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Diagnostics;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
  public abstract class MySteeringBase
  {
    public float Weight { get; protected set; }

    public MyBotNavigation Parent { get; }

    protected MySteeringBase(MyBotNavigation parent, float weight)
    {
      this.Weight = weight;
      this.Parent = parent;
    }

    public abstract void AccumulateCorrection(ref Vector3 correction, ref float weight);

    public virtual void Update()
    {
    }

    public virtual void Cleanup()
    {
    }

    public abstract string GetName();

    [Conditional("DEBUG")]
    public virtual void DebugDraw()
    {
    }
  }
}
