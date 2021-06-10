// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MySphereDensityFunction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal class MySphereDensityFunction : IMyAsteroidFieldDensityFunction, IMyModule
  {
    private Vector3D m_center;
    private BoundingSphereD m_sphereMax;
    private double m_innerRadius;
    private double m_outerRadius;
    private double m_middleRadius;
    private double m_halfFalloff;

    public MySphereDensityFunction(Vector3D center, double radius, double additionalFalloff)
    {
      this.m_center = center;
      this.m_sphereMax = new BoundingSphereD(center, radius + additionalFalloff);
      this.m_innerRadius = radius;
      this.m_halfFalloff = additionalFalloff / 2.0;
      this.m_middleRadius = radius + this.m_halfFalloff;
      this.m_outerRadius = radius + additionalFalloff;
    }

    public bool ExistsInCell(ref BoundingBoxD bbox) => (uint) this.m_sphereMax.Contains(bbox) > 0U;

    public double GetValue(double x) => throw new NotImplementedException();

    public double GetValue(double x, double y) => throw new NotImplementedException();

    public double GetValue(double x, double y, double z)
    {
      double num = Vector3D.Distance(this.m_center, new Vector3D(x, y, z));
      if (num > this.m_outerRadius)
        return 1.0;
      if (num < this.m_innerRadius)
        return -1.0;
      return num > this.m_middleRadius ? (this.m_middleRadius - num) / -this.m_halfFalloff : (num - this.m_middleRadius) / this.m_halfFalloff;
    }
  }
}
