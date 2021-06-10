// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MySharedTensorData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
  internal class MySharedTensorData : IGroupData<MyCubeGrid>
  {
    public MyGroups<MyCubeGrid, MySharedTensorData>.Group m_group { get; set; }

    public void OnNodeAdded(MyCubeGrid grid)
    {
      this.MarkDirty();
      MySharedTensorData.MarkGridTensorDirty(grid);
    }

    public void OnNodeRemoved(MyCubeGrid grid)
    {
      this.MarkDirty();
      MySharedTensorData.MarkGridTensorDirty(grid);
    }

    public void MarkDirty()
    {
      foreach (MyGroups<MyCubeGrid, MySharedTensorData>.Node node in this.m_group.Nodes)
        MySharedTensorData.MarkGridTensorDirty(node.NodeData);
    }

    public static void MarkGridTensorDirty(MyCubeGrid grid) => grid.Physics?.Shape.MarkSharedTensorDirty();

    public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new() => this.m_group = group as MyGroups<MyCubeGrid, MySharedTensorData>.Group;

    public void OnRelease() => this.m_group = (MyGroups<MyCubeGrid, MySharedTensorData>.Group) null;
  }
}
