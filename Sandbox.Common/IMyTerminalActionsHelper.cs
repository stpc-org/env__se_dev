// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyTerminalActionsHelper
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI
{
  public interface IMyTerminalActionsHelper
  {
    void GetActions(
      Type blockType,
      List<ITerminalAction> resultList,
      Func<ITerminalAction, bool> collect = null);

    void SearchActionsOfName(
      string name,
      Type blockType,
      List<ITerminalAction> resultList,
      Func<ITerminalAction, bool> collect = null);

    ITerminalAction GetActionWithName(string nameType, Type blockType);

    ITerminalProperty GetProperty(string id, Type blockType);

    void GetProperties(
      Type blockType,
      List<ITerminalProperty> resultList,
      Func<ITerminalProperty, bool> collect = null);

    IMyGridTerminalSystem GetTerminalSystemForGrid(IMyCubeGrid grid);
  }
}
