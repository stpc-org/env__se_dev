// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemTerminalBlock))]
  internal class MyToolbarItemTerminalBlock : MyToolbarItemActions, IMyToolbarItemEntity
  {
    private long m_blockEntityId;
    private bool m_wasValid;
    private bool m_nameChanged;
    private MyTerminalBlock m_block;
    private List<TerminalActionParameter> m_parameters = new List<TerminalActionParameter>();
    private static List<ITerminalAction> m_tmpEnabledActions = new List<ITerminalAction>();
    private static StringBuilder m_tmpStringBuilder = new StringBuilder();
    private MyTerminalBlock m_registeredBlock;

    private bool TryGetBlock()
    {
      int num = Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyTerminalBlock>(this.m_blockEntityId, out this.m_block) ? 1 : 0;
      if (num == 0)
        return num != 0;
      this.RegisterEvents();
      return num != 0;
    }

    public override ListReader<ITerminalAction> AllActions => this.GetActions(new MyToolbarType?());

    public List<TerminalActionParameter> Parameters => this.m_parameters;

    public override ListReader<ITerminalAction> PossibleActions(
      MyToolbarType type)
    {
      return this.GetActions(new MyToolbarType?(type));
    }

    private ListReader<ITerminalAction> GetActions(MyToolbarType? type)
    {
      if (this.m_block == null)
        return ListReader<ITerminalAction>.Empty;
      MyToolbarItemTerminalBlock.m_tmpEnabledActions.Clear();
      foreach (ITerminalAction action in MyTerminalControls.Static.GetActions((Sandbox.ModAPI.IMyTerminalBlock) this.m_block))
      {
        if (action.IsEnabled(this.m_block) && (!type.HasValue || action.IsValidForToolbarType(type.Value)))
          MyToolbarItemTerminalBlock.m_tmpEnabledActions.Add(action);
      }
      return (ListReader<ITerminalAction>) MyToolbarItemTerminalBlock.m_tmpEnabledActions;
    }

    public override bool Activate()
    {
      ITerminalAction currentAction = this.GetCurrentAction();
      if (this.m_block == null || currentAction == null)
        return false;
      currentAction.Apply(this.m_block, (ListReader<TerminalActionParameter>) this.Parameters);
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type != MyToolbarType.Character && type != MyToolbarType.Spectator;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      MyToolbarItem.ChangeInfo changeInfo1 = base.Update(owner, playerID);
      if (this.m_block == null)
        this.TryGetBlock();
      ITerminalAction currentAction = this.GetCurrentAction();
      bool flag = this.m_block != null && currentAction != null && owner != null && MyCubeGridGroups.Static.Physical.HasSameGroup((owner as MyTerminalBlock).CubeGrid, this.m_block.CubeGrid);
      MyToolbarItem.ChangeInfo changeInfo2 = changeInfo1 | this.SetEnabled(flag && this.m_block.IsFunctional && (this.m_block.HasPlayerAccess(playerID) || owner != null && this.m_block.HasPlayerAccess((owner as MyTerminalBlock).OwnerId)));
      if (this.m_block != null)
        changeInfo2 |= this.SetIcons(this.m_block.BlockDefinition.Icons);
      if (flag)
      {
        if (!this.m_wasValid || this.ActionChanged)
          changeInfo2 = changeInfo2 | this.SetIcons(this.m_block.BlockDefinition.Icons) | this.SetSubIcon(currentAction.Icon) | this.UpdateCustomName(currentAction);
        else if (this.m_nameChanged)
          changeInfo2 |= this.UpdateCustomName(currentAction);
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.Clear();
        currentAction.WriteValue(this.m_block, MyToolbarItemTerminalBlock.m_tmpStringBuilder);
        changeInfo2 |= this.SetIconText(MyToolbarItemTerminalBlock.m_tmpStringBuilder);
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.Clear();
      }
      this.m_wasValid = flag;
      this.m_nameChanged = false;
      this.ActionChanged = false;
      return changeInfo2;
    }

    public string GetBlockName() => this.m_block == null ? string.Empty : this.m_block.CustomName.ToString();

    public string GetActionName()
    {
      ITerminalAction currentAction = this.GetCurrentAction();
      return currentAction == null ? string.Empty : currentAction.Name.ToString();
    }

    private MyToolbarItem.ChangeInfo UpdateCustomName(ITerminalAction action)
    {
      try
      {
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.Clear();
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.AppendStringBuilder(this.m_block.CustomName);
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.Append(" - ");
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.AppendStringBuilder(action.Name);
        return this.SetDisplayName(MyToolbarItemTerminalBlock.m_tmpStringBuilder.ToString());
      }
      finally
      {
        MyToolbarItemTerminalBlock.m_tmpStringBuilder.Clear();
      }
    }

    public bool CompareEntityIds(long id) => id == this.m_blockEntityId;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      if (!(obj is MyToolbarItemTerminalBlock itemTerminalBlock) || this.m_blockEntityId != itemTerminalBlock.m_blockEntityId || (!(this.ActionId == itemTerminalBlock.ActionId) || this.m_parameters.Count != itemTerminalBlock.Parameters.Count))
        return false;
      for (int index = 0; index < this.m_parameters.Count; ++index)
      {
        TerminalActionParameter parameter1 = this.m_parameters[index];
        TerminalActionParameter parameter2 = itemTerminalBlock.Parameters[index];
        if (parameter1.TypeCode != parameter2.TypeCode || !object.Equals(parameter1.Value, parameter2.Value))
          return false;
      }
      return true;
    }

    public override int GetHashCode() => this.m_blockEntityId.GetHashCode() * 397 ^ this.ActionId.GetHashCode();

    public override bool Init(MyObjectBuilder_ToolbarItem objectBuilder)
    {
      this.WantsToBeActivated = false;
      this.WantsToBeSelected = false;
      this.ActivateOnClick = true;
      this.m_block = (MyTerminalBlock) null;
      MyObjectBuilder_ToolbarItemTerminalBlock itemTerminalBlock = (MyObjectBuilder_ToolbarItemTerminalBlock) objectBuilder;
      this.m_blockEntityId = itemTerminalBlock.BlockEntityId;
      if (this.m_blockEntityId == 0L)
      {
        this.m_wasValid = false;
        return false;
      }
      this.TryGetBlock();
      this.SetAction(itemTerminalBlock._Action);
      if (itemTerminalBlock.Parameters != null && itemTerminalBlock.Parameters.Count > 0)
      {
        this.m_parameters.Clear();
        foreach (MyObjectBuilder_ToolbarItemActionParameter parameter in itemTerminalBlock.Parameters)
          this.m_parameters.Add(TerminalActionParameter.Deserialize(parameter.Value, parameter.TypeCode));
      }
      return true;
    }

    private void RegisterEvents()
    {
      this.UnregisterEvents();
      this.m_block.CustomNameChanged += new Action<MyTerminalBlock>(this.block_CustomNameChanged);
      this.m_block.OnClose += new Action<MyEntity>(this.block_OnClose);
      this.m_registeredBlock = this.m_block;
    }

    private void UnregisterEvents()
    {
      if (this.m_registeredBlock == null)
        return;
      this.m_registeredBlock.CustomNameChanged -= new Action<MyTerminalBlock>(this.block_CustomNameChanged);
      this.m_registeredBlock.OnClose -= new Action<MyEntity>(this.block_OnClose);
      this.m_registeredBlock = (MyTerminalBlock) null;
      MyToolbarItemTerminalBlock.m_tmpEnabledActions.Clear();
    }

    private void block_CustomNameChanged(MyTerminalBlock obj) => this.m_nameChanged = true;

    private void block_OnClose(MyEntity obj)
    {
      this.UnregisterEvents();
      this.m_block = (MyTerminalBlock) null;
    }

    public override void OnRemovedFromToolbar(MyToolbar toolbar)
    {
      if (this.m_block != null)
        this.UnregisterEvents();
      base.OnRemovedFromToolbar(toolbar);
    }

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
    {
      MyObjectBuilder_ToolbarItemTerminalBlock objectBuilder = (MyObjectBuilder_ToolbarItemTerminalBlock) MyToolbarItemFactory.CreateObjectBuilder((MyToolbarItem) this);
      objectBuilder.BlockEntityId = this.m_blockEntityId;
      objectBuilder._Action = this.ActionId;
      objectBuilder.Parameters.Clear();
      foreach (TerminalActionParameter parameter in this.m_parameters)
        objectBuilder.Parameters.Add(parameter.GetObjectBuilder());
      return (MyObjectBuilder_ToolbarItem) objectBuilder;
    }
  }
}
