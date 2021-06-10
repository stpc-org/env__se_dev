// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyShipMass
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

namespace Sandbox.ModAPI.Ingame
{
  public struct MyShipMass
  {
    public readonly float BaseMass;
    public readonly float TotalMass;
    public readonly float PhysicalMass;

    public MyShipMass(float mass, float totalMass, float physicalMass)
    {
      this.BaseMass = mass;
      this.TotalMass = totalMass;
      this.PhysicalMass = physicalMass;
    }
  }
}
