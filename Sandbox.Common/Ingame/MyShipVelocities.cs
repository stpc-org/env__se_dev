// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyShipVelocities
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public struct MyShipVelocities
  {
    public readonly Vector3D LinearVelocity;
    public readonly Vector3D AngularVelocity;

    public MyShipVelocities(Vector3D linearVelocity, Vector3D angularVelocity)
      : this()
    {
      this.LinearVelocity = linearVelocity;
      this.AngularVelocity = angularVelocity;
    }
  }
}
