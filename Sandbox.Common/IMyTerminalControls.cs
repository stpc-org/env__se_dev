// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyTerminalControls
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Interfaces.Terminal;
using System.Collections.Generic;

namespace Sandbox.ModAPI
{
  public interface IMyTerminalControls
  {
    event CustomControlGetDelegate CustomControlGetter;

    event CustomActionGetDelegate CustomActionGetter;

    void GetControls<TBlock>(out List<IMyTerminalControl> items);

    void AddControl<TBlock>(IMyTerminalControl item);

    void RemoveControl<TBlock>(IMyTerminalControl item);

    TControl CreateControl<TControl, TBlock>(string id);

    IMyTerminalControlProperty<TValue> CreateProperty<TValue, TBlock>(
      string id);

    void GetActions<TBlock>(out List<IMyTerminalAction> items);

    void AddAction<TBlock>(IMyTerminalAction action);

    void RemoveAction<TBlock>(IMyTerminalAction action);

    IMyTerminalAction CreateAction<TBlock>(string id);
  }
}
