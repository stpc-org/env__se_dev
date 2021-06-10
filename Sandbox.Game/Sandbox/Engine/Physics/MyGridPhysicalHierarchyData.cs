// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyGridPhysicalHierarchyData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
  public class MyGridPhysicalHierarchyData : IGroupData<MyCubeGrid>
  {
    public MyCubeGrid m_root;
    private MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group m_group;

    public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new() => this.m_group = group as MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group;

    public void OnRelease()
    {
      this.m_root = (MyCubeGrid) null;
      this.m_group = (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group) null;
    }

    public void OnNodeAdded(MyCubeGrid entity)
    {
    }

    public void OnNodeRemoved(MyCubeGrid entity)
    {
    }
  }
}
