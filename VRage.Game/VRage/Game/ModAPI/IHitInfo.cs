// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IHitInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IHitInfo
  {
    Vector3D Position { get; }

    IMyEntity HitEntity { get; }

    Vector3 Normal { get; }

    float Fraction { get; }
  }
}
