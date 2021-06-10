// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyTerminalBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyTerminalBlock : IMyCubeBlock, IMyEntity
  {
    string CustomName { get; set; }

    string CustomNameWithFaction { get; }

    string DetailedInfo { get; }

    string CustomInfo { get; }

    string CustomData { get; set; }

    bool HasLocalPlayerAccess();

    bool HasPlayerAccess(long playerId);

    [Obsolete("Use the setter of Customname")]
    void SetCustomName(string text);

    [Obsolete("Use the setter of Customname")]
    void SetCustomName(StringBuilder text);

    bool ShowOnHUD { get; set; }

    bool ShowInTerminal { get; set; }

    bool ShowInToolbarConfig { get; set; }

    void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

    void SearchActionsOfName(
      string name,
      List<ITerminalAction> resultList,
      Func<ITerminalAction, bool> collect = null);

    ITerminalAction GetActionWithName(string name);

    ITerminalProperty GetProperty(string id);

    void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null);

    bool ShowInInventory { get; set; }

    bool IsSameConstructAs(IMyTerminalBlock other);
  }
}
