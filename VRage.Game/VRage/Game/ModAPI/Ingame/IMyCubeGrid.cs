// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMyCubeGrid
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
  public interface IMyCubeGrid : IMyEntity
  {
    string CustomName { get; set; }

    float GridSize { get; }

    MyCubeSize GridSizeEnum { get; }

    bool IsStatic { get; }

    Vector3I Max { get; }

    Vector3I Min { get; }

    bool CubeExists(Vector3I pos);

    IMySlimBlock GetCubeBlock(Vector3I pos);

    Vector3D GridIntegerToWorld(Vector3I gridCoords);

    Vector3I WorldToGridInteger(Vector3D coords);

    bool IsSameConstructAs(IMyCubeGrid other);
  }
}
