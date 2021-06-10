// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGridGroupsHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
  public class MyGridGroupsHelper : IMyGridGroups
  {
    [Obsolete("Use GetGroup with passing your own collection, it is better for simulation speed", false)]
    public List<IMyCubeGrid> GetGroup(IMyCubeGrid node, GridLinkTypeEnum type) => MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes((MyCubeGrid) node).Cast<IMyCubeGrid>().ToList<IMyCubeGrid>();

    public void GetGroup(
      IMyCubeGrid node,
      GridLinkTypeEnum type,
      ICollection<IMyCubeGrid> collection)
    {
      foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes((MyCubeGrid) node))
        collection.Add((IMyCubeGrid) groupNode);
    }

    public bool HasConnection(IMyCubeGrid grid1, IMyCubeGrid grid2, GridLinkTypeEnum type) => MyCubeGridGroups.Static.GetGroups(type).HasSameGroup((MyCubeGrid) grid1, (MyCubeGrid) grid2);
  }
}
