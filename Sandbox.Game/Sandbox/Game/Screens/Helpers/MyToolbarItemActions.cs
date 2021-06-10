// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemActions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  internal abstract class MyToolbarItemActions : MyToolbarItem
  {
    private string m_actionId;

    protected bool ActionChanged { get; set; }

    public string ActionId
    {
      get => this.m_actionId;
      set
      {
        if (this.m_actionId != null && this.m_actionId.Equals(value) || this.m_actionId == null && value == null)
          return;
        this.m_actionId = value;
        this.ActionChanged = true;
      }
    }

    public abstract ListReader<ITerminalAction> AllActions { get; }

    public abstract ListReader<ITerminalAction> PossibleActions(
      MyToolbarType toolbarType);

    public ITerminalAction GetCurrentAction() => this.GetActionOrNull(this.ActionId);

    public ITerminalAction GetActionOrNull(string id)
    {
      foreach (ITerminalAction allAction in this.AllActions)
      {
        if (allAction.Id == id)
          return allAction;
      }
      return (ITerminalAction) null;
    }

    protected void SetAction(string action)
    {
      this.ActionId = action;
      if (this.ActionId != null)
        return;
      ListReader<ITerminalAction> allActions = this.AllActions;
      if (allActions.Count <= 0)
        return;
      this.ActionId = allActions.ItemAt(0).Id;
    }

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      if (this.ActionId == null)
      {
        ListReader<ITerminalAction> allActions = this.AllActions;
        if (allActions.Count > 0)
          this.ActionId = allActions.ItemAt(0).Id;
      }
      return MyToolbarItem.ChangeInfo.None;
    }
  }
}
