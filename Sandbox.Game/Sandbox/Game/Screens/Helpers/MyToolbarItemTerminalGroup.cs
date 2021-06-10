// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemTerminalGroup))]
  internal class MyToolbarItemTerminalGroup : MyToolbarItemActions, IMyToolbarItemEntity
  {
    private static HashSet<Type> tmpBlockTypes = new HashSet<Type>();
    private static List<MyTerminalBlock> m_tmpBlocks = new List<MyTerminalBlock>();
    private static StringBuilder m_tmpStringBuilder = new StringBuilder();
    private StringBuilder m_groupName;
    private long m_blockEntityId;
    private bool m_wasValid;

    private ListReader<MyTerminalBlock> GetBlocks()
    {
      MyCubeBlock entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeBlock>(this.m_blockEntityId, out entity);
      if (entity == null)
        return ListReader<MyTerminalBlock>.Empty;
      MyCubeGrid cubeGrid = entity.CubeGrid;
      if (cubeGrid == null || cubeGrid.GridSystems.TerminalSystem == null)
        return ListReader<MyTerminalBlock>.Empty;
      foreach (MyBlockGroup blockGroup in cubeGrid.GridSystems.TerminalSystem.BlockGroups)
      {
        if (blockGroup.Name.Equals(this.m_groupName))
          return (ListReader<MyTerminalBlock>) blockGroup.Blocks.ToList<MyTerminalBlock>();
      }
      return ListReader<MyTerminalBlock>.Empty;
    }

    private ListReader<ITerminalAction> GetActions(
      ListReader<MyTerminalBlock> blocks,
      out bool genericType)
    {
      try
      {
        bool flag = true;
        foreach (MyTerminalBlock block in blocks)
        {
          flag &= block is MyFunctionalBlock;
          MyToolbarItemTerminalGroup.tmpBlockTypes.Add(block.GetType());
        }
        if (MyToolbarItemTerminalGroup.tmpBlockTypes.Count == 1)
        {
          genericType = false;
          return this.GetValidActions(blocks.ItemAt(0).GetType(), blocks);
        }
        if (MyToolbarItemTerminalGroup.tmpBlockTypes.Count == 0 || !flag)
        {
          genericType = true;
          return ListReader<ITerminalAction>.Empty;
        }
        genericType = true;
        return this.GetValidActions(MyToolbarItemTerminalGroup.FindBaseClass(MyToolbarItemTerminalGroup.tmpBlockTypes.ToArray<Type>(), typeof (MyFunctionalBlock)), blocks);
      }
      finally
      {
        MyToolbarItemTerminalGroup.tmpBlockTypes.Clear();
      }
    }

    public static Type FindBaseClass(Type[] types, Type baseKnownCommonType)
    {
      Type type = types[0];
      Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
      dictionary.Add(baseKnownCommonType, types.Length);
      for (int index = 0; index < types.Length; ++index)
      {
        for (Type key = types[index]; key != baseKnownCommonType; key = key.BaseType)
        {
          if (dictionary.ContainsKey(key))
            ++dictionary[key];
          else
            dictionary[key] = 1;
        }
      }
      Type key1 = types[0];
      while (dictionary[key1] != types.Length)
        key1 = key1.BaseType;
      return key1;
    }

    private ListReader<ITerminalAction> GetValidActions(
      Type blockType,
      ListReader<MyTerminalBlock> blocks)
    {
      UniqueListReader<ITerminalAction> actions = MyTerminalControlFactory.GetActions(blockType);
      List<ITerminalAction> terminalActionList = new List<ITerminalAction>();
      foreach (ITerminalAction terminalAction in actions)
      {
        if (terminalAction.IsValidForGroups())
        {
          bool flag = false;
          foreach (MyTerminalBlock block in blocks)
          {
            if (terminalAction.IsEnabled(block))
            {
              flag = true;
              break;
            }
          }
          if (flag)
            terminalActionList.Add(terminalAction);
        }
      }
      return (ListReader<ITerminalAction>) terminalActionList;
    }

    private ITerminalAction FindAction(
      ListReader<ITerminalAction> actions,
      string name)
    {
      foreach (ITerminalAction action in actions)
      {
        if (action.Id == name)
          return action;
      }
      return (ITerminalAction) null;
    }

    private MyTerminalBlock FirstFunctional(
      ListReader<MyTerminalBlock> blocks,
      MyEntity owner,
      long playerID)
    {
      foreach (MyTerminalBlock block in blocks)
      {
        if (block.IsFunctional && (block.HasPlayerAccess(playerID) || block.HasPlayerAccess((owner as MyTerminalBlock).OwnerId)))
          return block;
      }
      return (MyTerminalBlock) null;
    }

    public override ListReader<ITerminalAction> AllActions => this.GetActions(this.GetBlocks(), out bool _);

    public override ListReader<ITerminalAction> PossibleActions(
      MyToolbarType toolbarType)
    {
      return this.AllActions;
    }

    public override bool Activate()
    {
      ListReader<MyTerminalBlock> blocks = this.GetBlocks();
      ITerminalAction action = this.FindAction(this.GetActions(blocks, out bool _), this.ActionId);
      if (action == null)
        return false;
      try
      {
        foreach (MyTerminalBlock myTerminalBlock in blocks)
          MyToolbarItemTerminalGroup.m_tmpBlocks.Add(myTerminalBlock);
        foreach (MyTerminalBlock tmpBlock in MyToolbarItemTerminalGroup.m_tmpBlocks)
        {
          if (tmpBlock != null && tmpBlock.IsFunctional)
            action.Apply(tmpBlock);
        }
      }
      finally
      {
        MyToolbarItemTerminalGroup.m_tmpBlocks.Clear();
      }
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type != MyToolbarType.Character && type != MyToolbarType.Spectator;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      MyToolbarItem.ChangeInfo changeInfo1 = base.Update(owner, playerID);
      ListReader<MyTerminalBlock> blocks = this.GetBlocks();
      bool genericType;
      ITerminalAction action = this.FindAction(this.GetActions(blocks, out genericType), this.ActionId);
      MyTerminalBlock myTerminalBlock = this.FirstFunctional(blocks, owner, playerID);
      int num1 = (int) (changeInfo1 | this.SetEnabled(action != null && myTerminalBlock != null));
      string[] newIcons;
      if (!genericType)
        newIcons = blocks.ItemAt(0).BlockDefinition.Icons;
      else
        newIcons = new string[1]
        {
          "Textures\\GUI\\Icons\\GroupIcon.dds"
        };
      int num2 = (int) this.SetIcons(newIcons);
      MyToolbarItem.ChangeInfo changeInfo2 = (MyToolbarItem.ChangeInfo) (num1 | num2) | this.SetSubIcon(action?.Icon);
      if (action != null && !this.m_wasValid)
      {
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.Clear();
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.AppendStringBuilder(this.m_groupName);
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.Append(" - ");
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.Append((object) action.Name);
        changeInfo2 |= this.SetDisplayName(MyToolbarItemTerminalGroup.m_tmpStringBuilder.ToString());
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.Clear();
        this.m_wasValid = true;
      }
      else if (action == null)
        this.m_wasValid = false;
      if (action != null && blocks.Count > 0)
      {
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.Clear();
        action.WriteValue(myTerminalBlock ?? blocks.ItemAt(0), MyToolbarItemTerminalGroup.m_tmpStringBuilder);
        changeInfo2 |= this.SetIconText(MyToolbarItemTerminalGroup.m_tmpStringBuilder);
        MyToolbarItemTerminalGroup.m_tmpStringBuilder.Clear();
      }
      return changeInfo2;
    }

    public bool CompareEntityIds(long id) => this.m_blockEntityId == id;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is MyToolbarItemTerminalGroup itemTerminalGroup && this.m_blockEntityId == itemTerminalGroup.m_blockEntityId && this.m_groupName.Equals(itemTerminalGroup.m_groupName) && this.ActionId == itemTerminalGroup.ActionId;
    }

    public override int GetHashCode() => (this.m_blockEntityId.GetHashCode() * 397 ^ this.m_groupName.GetHashCode()) * 397 ^ this.ActionId.GetHashCode();

    public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
    {
      this.WantsToBeActivated = false;
      this.WantsToBeSelected = false;
      this.ActivateOnClick = true;
      MyObjectBuilder_ToolbarItemTerminalGroup itemTerminalGroup = (MyObjectBuilder_ToolbarItemTerminalGroup) objBuilder;
      int num = (int) this.SetDisplayName(itemTerminalGroup.GroupName);
      if (itemTerminalGroup.BlockEntityId == 0L)
      {
        this.m_wasValid = false;
        return false;
      }
      this.m_blockEntityId = itemTerminalGroup.BlockEntityId;
      this.m_groupName = new StringBuilder(itemTerminalGroup.GroupName);
      this.m_wasValid = true;
      this.SetAction(itemTerminalGroup._Action);
      return true;
    }

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
    {
      MyObjectBuilder_ToolbarItemTerminalGroup objectBuilder = (MyObjectBuilder_ToolbarItemTerminalGroup) MyToolbarItemFactory.CreateObjectBuilder((MyToolbarItem) this);
      objectBuilder.GroupName = this.m_groupName.ToString();
      objectBuilder.BlockEntityId = this.m_blockEntityId;
      objectBuilder._Action = this.ActionId;
      return (MyObjectBuilder_ToolbarItem) objectBuilder;
    }
  }
}
