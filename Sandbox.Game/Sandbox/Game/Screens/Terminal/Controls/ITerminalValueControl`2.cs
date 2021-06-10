// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Terminal.Controls.ITerminalValueControl`2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces;
using System;
using VRage.Library.Collections;

namespace Sandbox.Game.Screens.Terminal.Controls
{
  internal interface ITerminalValueControl<TBlock, TValue> : ITerminalProperty<TValue>, ITerminalProperty, ITerminalControl, ITerminalControlSync
    where TBlock : MyTerminalBlock
  {
    TValue GetValue(TBlock block);

    void SetValue(TBlock block, TValue value);

    TValue GetDefaultValue(TBlock block);

    [Obsolete("Use GetMinimum instead")]
    TValue GetMininum(TBlock block);

    TValue GetMinimum(TBlock block);

    TValue GetMaximum(TBlock block);

    void Serialize(BitStream stream, TBlock block);
  }
}
