// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyInfiniteDensityFunction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Library.Utils;
using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal class MyInfiniteDensityFunction : IMyAsteroidFieldDensityFunction, IMyModule
  {
    private IMyModule noise;

    public MyInfiniteDensityFunction(MyRandom random, double frequency) => this.noise = (IMyModule) new MySimplexFast(random.Next(), frequency);

    public bool ExistsInCell(ref BoundingBoxD bbox) => true;

    public double GetValue(double x) => this.noise.GetValue(x);

    public double GetValue(double x, double y) => this.noise.GetValue(x, y);

    public double GetValue(double x, double y, double z) => this.noise.GetValue(x, y, z);
  }
}
