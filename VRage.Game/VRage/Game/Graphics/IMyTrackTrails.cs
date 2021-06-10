// Decompiled with JetBrains decompiler
// Type: VRage.Game.Graphics.IMyTrackTrails
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
  public interface IMyTrackTrails
  {
    MyTrailProperties LastTrail { get; set; }

    void AddTrails(MyTrailProperties trailProperties);

    void AddTrails(
      Vector3D position,
      Vector3D normal,
      Vector3D forwardDirection,
      long entityId,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial);
  }
}
