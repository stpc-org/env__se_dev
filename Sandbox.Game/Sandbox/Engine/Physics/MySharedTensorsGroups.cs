// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MySharedTensorsGroups
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
  internal class MySharedTensorsGroups : MyGroups<MyCubeGrid, MySharedTensorData>, IMySceneComponent
  {
    private static MySharedTensorsGroups m_static;

    private static MySharedTensorsGroups Static => MySharedTensorsGroups.m_static;

    public void Load() => MySharedTensorsGroups.m_static = this;

    public void Unload() => MySharedTensorsGroups.m_static = (MySharedTensorsGroups) null;

    [DebuggerStepThrough]
    [Conditional("DEBUG")]
    private static void AssertThread()
    {
      Thread updateThread = MySandboxGame.Static.UpdateThread;
      Thread currentThread = Thread.CurrentThread;
    }

    public static void Link(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock) => MySharedTensorsGroups.Static.CreateLink(linkingBlock.EntityId, parent, child);

    public static bool BreakLinkIfExists(
      MyCubeGrid parent,
      MyCubeGrid child,
      MyCubeBlock linkingBlock)
    {
      return MySharedTensorsGroups.Static.BreakLink(linkingBlock.EntityId, parent, child);
    }

    public static void MarkGroupDirty(MyCubeGrid grid) => MySharedTensorsGroups.Static.GetGroup(grid)?.GroupData.MarkDirty();

    public static HashSetReader<MyGroups<MyCubeGrid, MySharedTensorData>.Node> GetGridsInSameGroup(
      MyCubeGrid groupRepresentative)
    {
      MyGroups<MyCubeGrid, MySharedTensorData>.Group group = MySharedTensorsGroups.Static.GetGroup(groupRepresentative);
      return group == null ? new HashSetReader<MyGroups<MyCubeGrid, MySharedTensorData>.Node>() : group.Nodes;
    }

    public MySharedTensorsGroups()
      : base()
    {
    }
  }
}
