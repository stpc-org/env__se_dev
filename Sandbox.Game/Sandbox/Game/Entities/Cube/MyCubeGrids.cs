// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGrids
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Components;

namespace Sandbox.Game.Entities.Cube
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MyCubeGrids : MySessionComponentBase
  {
    public static event Action<MyCubeGrid, MySlimBlock> BlockBuilt;

    public static event Action<MyCubeGrid, MySlimBlock> BlockDestroyed;

    public static event Action<MyCubeGrid, MySlimBlock, bool> BlockFinished;

    public static event Action<MyCubeGrid, MySlimBlock, bool> BlockFunctional;

    private long Now => DateTime.Now.Ticks;

    internal static void NotifyBlockBuilt(MyCubeGrid grid, MySlimBlock block) => MyCubeGrids.BlockBuilt.InvokeIfNotNull<MyCubeGrid, MySlimBlock>(grid, block);

    internal static void NotifyBlockDestroyed(MyCubeGrid grid, MySlimBlock block) => MyCubeGrids.BlockDestroyed.InvokeIfNotNull<MyCubeGrid, MySlimBlock>(grid, block);

    internal static void NotifyBlockFinished(MyCubeGrid grid, MySlimBlock block, bool handWelded) => MyCubeGrids.BlockFinished.InvokeIfNotNull<MyCubeGrid, MySlimBlock, bool>(grid, block, handWelded);

    internal static void NotifyBlockFunctional(MyCubeGrid grid, MySlimBlock block, bool handWelded) => MyCubeGrids.BlockFunctional.InvokeIfNotNull<MyCubeGrid, MySlimBlock, bool>(grid, block, handWelded);

    protected override void UnloadData()
    {
      base.UnloadData();
      MyCubeGrids.BlockBuilt = (Action<MyCubeGrid, MySlimBlock>) null;
      MyCubeGrids.BlockDestroyed = (Action<MyCubeGrid, MySlimBlock>) null;
    }
  }
}
