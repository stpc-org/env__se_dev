// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.ITerminalAction
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Ingame;
using System.Text;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Interfaces
{
  public interface ITerminalAction
  {
    string Id { get; }

    string Icon { get; }

    StringBuilder Name { get; }

    void Apply(IMyCubeBlock block);

    void Apply(
      IMyCubeBlock block,
      ListReader<TerminalActionParameter> terminalActionParameters);

    void WriteValue(IMyCubeBlock block, StringBuilder appendTo);

    bool IsEnabled(IMyCubeBlock block);
  }
}
