// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyExtendedPistonBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using System;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ExtendedPistonBase))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyExtendedPistonBase), typeof (Sandbox.ModAPI.Ingame.IMyExtendedPistonBase)})]
  public class MyExtendedPistonBase : MyPistonBase, Sandbox.ModAPI.IMyExtendedPistonBase, Sandbox.ModAPI.IMyPistonBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyPistonBase, Sandbox.ModAPI.Ingame.IMyExtendedPistonBase
  {
    private class Sandbox_Game_Entities_Blocks_MyExtendedPistonBase\u003C\u003EActor : IActivator, IActivator<MyExtendedPistonBase>
    {
      object IActivator.CreateInstance() => (object) new MyExtendedPistonBase();

      MyExtendedPistonBase IActivator<MyExtendedPistonBase>.CreateInstance() => new MyExtendedPistonBase();
    }
  }
}
