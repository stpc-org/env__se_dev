// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.TerminalActionExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.ModAPI.Ingame;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.Game.Gui
{
  public static class TerminalActionExtensions
  {
    public static Sandbox.ModAPI.Interfaces.ITerminalAction GetAction(
      this IMyTerminalBlock block,
      string name)
    {
      return block.GetActionWithName(name);
    }

    public static void ApplyAction(this IMyTerminalBlock block, string name) => block.GetAction(name).Apply((IMyCubeBlock) block);

    public static void ApplyAction(
      this IMyTerminalBlock block,
      string name,
      ListReader<TerminalActionParameter> parameters)
    {
      block.GetAction(name).Apply((IMyCubeBlock) block, parameters);
    }
  }
}
