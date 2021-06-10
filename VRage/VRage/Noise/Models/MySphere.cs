// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Models.MySphere
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise.Models
{
  internal class MySphere : IMyModule
  {
    protected void LatLonToXYZ(double lat, double lon, out double x, out double y, out double z)
    {
      double num = Math.Cos(Math.PI / 180.0 * lat);
      x = Math.Cos(Math.PI / 180.0 * lon) * num;
      y = Math.Sin(Math.PI / 180.0 * lat);
      z = Math.Sin(Math.PI / 180.0 * lon) * num;
    }

    public IMyModule Module { get; set; }

    public MySphere(IMyModule module) => this.Module = module;

    public double GetValue(double x) => throw new NotImplementedException();

    public double GetValue(double latitude, double longitude)
    {
      double x;
      double y;
      double z;
      this.LatLonToXYZ(latitude, longitude, out x, out y, out z);
      return this.Module.GetValue(x, y, z);
    }

    public double GetValue(double x, double y, double z) => throw new NotImplementedException();
  }
}
