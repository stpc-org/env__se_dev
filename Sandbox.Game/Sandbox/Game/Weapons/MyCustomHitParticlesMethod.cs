// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyCustomHitParticlesMethod
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  public delegate void MyCustomHitParticlesMethod(
    ref Vector3D hitPoint,
    ref Vector3 normal,
    ref Vector3D direction,
    IMyEntity entity,
    MyEntity weapon,
    float scale,
    MyEntity ownerEntity = null);
}
