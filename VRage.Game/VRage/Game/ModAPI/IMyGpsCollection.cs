// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyGpsCollection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyGpsCollection
  {
    List<IMyGps> GetGpsList(long identityId);

    void GetGpsList(long identityId, List<IMyGps> list);

    IMyGps Create(
      string name,
      string description,
      Vector3D coords,
      bool showOnHud,
      bool temporary = false);

    void AddGps(long identityId, IMyGps gps);

    void ModifyGps(long identityId, IMyGps gps);

    void RemoveGps(long identityId, IMyGps gps);

    void RemoveGps(long identityId, int gpsHash);

    void SetShowOnHud(long identityId, IMyGps gps, bool show);

    void SetShowOnHud(long identityId, int gpsHash, bool show);

    void AddLocalGps(IMyGps gps);

    void RemoveLocalGps(IMyGps gps);

    void RemoveLocalGps(int gpsHash);
  }
}
