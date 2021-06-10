// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyGridGroups
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
  public interface IMyGridGroups
  {
    [Obsolete("Use GetGroup with passing your own collection, it is better for simulation speed", false)]
    List<IMyCubeGrid> GetGroup(IMyCubeGrid node, GridLinkTypeEnum type);

    void GetGroup(IMyCubeGrid node, GridLinkTypeEnum type, ICollection<IMyCubeGrid> collection);

    bool HasConnection(IMyCubeGrid grid1, IMyCubeGrid grid2, GridLinkTypeEnum type);
  }
}
