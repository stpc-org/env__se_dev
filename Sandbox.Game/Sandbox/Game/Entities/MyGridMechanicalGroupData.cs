// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGridMechanicalGroupData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Groups;

namespace Sandbox.Game.Entities
{
  public class MyGridMechanicalGroupData : IGroupData<MyCubeGrid>
  {
    public void OnRelease()
    {
    }

    public void OnNodeAdded(MyCubeGrid entity)
    {
    }

    public void OnNodeRemoved(MyCubeGrid entity)
    {
    }

    public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
    {
    }
  }
}
