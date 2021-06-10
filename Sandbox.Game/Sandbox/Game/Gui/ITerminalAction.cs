// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.ITerminalAction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;

namespace Sandbox.Game.Gui
{
  public interface ITerminalAction : Sandbox.ModAPI.Interfaces.ITerminalAction
  {
    new string Id { get; }

    new string Icon { get; }

    new StringBuilder Name { get; }

    void Apply(MyTerminalBlock block);

    void WriteValue(MyTerminalBlock block, StringBuilder appendTo);

    bool IsEnabled(MyTerminalBlock block);

    bool IsValidForToolbarType(MyToolbarType toolbarType);

    bool IsValidForGroups();

    ListReader<TerminalActionParameter> GetParameterDefinitions();

    void RequestParameterCollection(
      IList<TerminalActionParameter> parameters,
      Action<bool> callback);

    void Apply(MyTerminalBlock block, ListReader<TerminalActionParameter> parameters);
  }
}
