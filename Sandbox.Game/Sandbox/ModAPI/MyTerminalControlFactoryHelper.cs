// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyTerminalControlFactoryHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Groups;

namespace Sandbox.ModAPI
{
  public class MyTerminalControlFactoryHelper : IMyTerminalActionsHelper
  {
    private static MyTerminalControlFactoryHelper m_instance;
    private List<Sandbox.Game.Gui.ITerminalAction> m_actionList = new List<Sandbox.Game.Gui.ITerminalAction>();
    private List<ITerminalProperty> m_valueControls = new List<ITerminalProperty>();

    public static MyTerminalControlFactoryHelper Static
    {
      get
      {
        if (MyTerminalControlFactoryHelper.m_instance == null)
          MyTerminalControlFactoryHelper.m_instance = new MyTerminalControlFactoryHelper();
        return MyTerminalControlFactoryHelper.m_instance;
      }
    }

    void IMyTerminalActionsHelper.GetActions(
      Type blockType,
      List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList,
      Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect)
    {
      if (!typeof (MyTerminalBlock).IsAssignableFrom(blockType))
        return;
      MyTerminalControlFactory.GetActions(blockType, this.m_actionList);
      foreach (Sandbox.Game.Gui.ITerminalAction action in this.m_actionList)
      {
        if ((collect == null || collect((Sandbox.ModAPI.Interfaces.ITerminalAction) action)) && action.IsValidForToolbarType(MyToolbarType.ButtonPanel))
          resultList.Add((Sandbox.ModAPI.Interfaces.ITerminalAction) action);
      }
      this.m_actionList.Clear();
    }

    void IMyTerminalActionsHelper.SearchActionsOfName(
      string name,
      Type blockType,
      List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList,
      Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
    {
      if (!typeof (MyTerminalBlock).IsAssignableFrom(blockType))
        return;
      MyTerminalControlFactory.GetActions(blockType, this.m_actionList);
      foreach (Sandbox.Game.Gui.ITerminalAction action in this.m_actionList)
      {
        if ((collect == null || collect((Sandbox.ModAPI.Interfaces.ITerminalAction) action)) && (action.Id.ToString().Contains(name) && action.IsValidForToolbarType(MyToolbarType.ButtonPanel)))
          resultList.Add((Sandbox.ModAPI.Interfaces.ITerminalAction) action);
      }
      this.m_actionList.Clear();
    }

    Sandbox.ModAPI.Interfaces.ITerminalAction IMyTerminalActionsHelper.GetActionWithName(
      string name,
      Type blockType)
    {
      if (!typeof (MyTerminalBlock).IsAssignableFrom(blockType))
        return (Sandbox.ModAPI.Interfaces.ITerminalAction) null;
      MyTerminalControlFactory.GetActions(blockType, this.m_actionList);
      foreach (Sandbox.Game.Gui.ITerminalAction action in this.m_actionList)
      {
        if (action.Id.ToString() == name && action.IsValidForToolbarType(MyToolbarType.ButtonPanel))
        {
          this.m_actionList.Clear();
          return (Sandbox.ModAPI.Interfaces.ITerminalAction) action;
        }
      }
      this.m_actionList.Clear();
      return (Sandbox.ModAPI.Interfaces.ITerminalAction) null;
    }

    public ITerminalProperty GetProperty(string id, Type blockType)
    {
      if (!typeof (MyTerminalBlock).IsAssignableFrom(blockType))
        return (ITerminalProperty) null;
      MyTerminalControlFactory.GetValueControls(blockType, this.m_valueControls);
      foreach (ITerminalProperty valueControl in this.m_valueControls)
      {
        if (valueControl.Id == id)
        {
          this.m_valueControls.Clear();
          return valueControl;
        }
      }
      this.m_valueControls.Clear();
      return (ITerminalProperty) null;
    }

    public void GetProperties(
      Type blockType,
      List<ITerminalProperty> resultList,
      Func<ITerminalProperty, bool> collect = null)
    {
      if (!typeof (MyTerminalBlock).IsAssignableFrom(blockType))
        return;
      MyTerminalControlFactory.GetValueControls(blockType, this.m_valueControls);
      foreach (ITerminalProperty valueControl in this.m_valueControls)
      {
        if (collect == null || collect(valueControl))
          resultList.Add(valueControl);
      }
      this.m_valueControls.Clear();
    }

    IMyGridTerminalSystem IMyTerminalActionsHelper.GetTerminalSystemForGrid(
      IMyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid as MyCubeGrid);
      return group != null && group.GroupData != null ? (IMyGridTerminalSystem) group.GroupData.TerminalSystem : (IMyGridTerminalSystem) null;
    }
  }
}
