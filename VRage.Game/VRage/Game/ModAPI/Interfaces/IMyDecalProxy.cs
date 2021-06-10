// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Interfaces.IMyDecalProxy
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
  public interface IMyDecalProxy
  {
    void AddDecals(
      ref MyHitInfo hitInfo,
      MyStringHash source,
      Vector3 forwardDirection,
      object customdata,
      IMyDecalHandler decalHandler,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial,
      bool isTrail);
  }
}
