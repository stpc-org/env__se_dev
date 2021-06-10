// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlPanel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyTerminalControlPanel : MyTerminalController
  {
    public static readonly Vector4 RED_TEXT_COLOR = new Vector4(1f, 0.25f, 0.25f, 1f);
    private static readonly MyTerminalComparer m_nameComparer = new MyTerminalComparer();
    private static bool m_showAllTerminalBlocks = false;
    private static HashSet<Type> tmpBlockTypes = new HashSet<Type>();
    private static MyGuiHighlightTexture ICON_HIDE = new MyGuiHighlightTexture()
    {
      Normal = "Textures\\GUI\\Controls\\button_hide.dds",
      Highlight = "Textures\\GUI\\Controls\\button_hide.dds",
      Focus = "Textures\\GUI\\Controls\\button_hide_focus.dds",
      SizePx = new Vector2(40f, 40f)
    };
    private static MyGuiHighlightTexture ICON_UNHIDE = new MyGuiHighlightTexture()
    {
      Normal = "Textures\\GUI\\Controls\\button_unhide.dds",
      Highlight = "Textures\\GUI\\Controls\\button_unhide.dds",
      Focus = "Textures\\GUI\\Controls\\button_unhide_focus.dds",
      SizePx = new Vector2(40f, 40f)
    };
    private IMyGuiControlsParent m_controlsParent;
    private MyGuiControlListbox m_blockListbox;
    private MyGuiControlLabel m_blockNameLabel;
    private MyGuiControlBase blockControl;
    private List<MyBlockGroup> m_currentGroups = new List<MyBlockGroup>();
    private MyBlockGroup m_tmpGroup;
    private MyGuiControlSearchBox m_searchBox;
    private MyGuiControlTextbox m_groupName;
    private MyGuiControlButton m_groupSave;
    private MyGuiControlButton m_showAll;
    private MyGuiControlButton m_groupDelete;
    private List<MyBlockGroup> m_oldGroups = new List<MyBlockGroup>();
    private MyTerminalBlock m_originalBlock;
    private MyGridColorHelper m_colorHelper;
    private MyPlayer m_controller;
    private ulong m_last_showInTerminalChanged;
    private MyGuiScreenTerminal m_terminalScreen;

    private MyGuiControlBase m_blockControl
    {
      get => this.blockControl;
      set
      {
        if (this.blockControl == value)
          return;
        if (this.m_terminalScreen != null && this.blockControl != null)
          this.m_terminalScreen.DetachGroups(this.blockControl.Elements);
        if (this.m_terminalScreen != null && value != null)
          this.m_terminalScreen.AttachGroups(value.Elements);
        this.blockControl = value;
      }
    }

    private HashSet<MyTerminalBlock> CurrentBlocks => this.m_tmpGroup.Blocks;

    public MyGridTerminalSystem TerminalSystem { get; private set; }

    public void Init(
      IMyGuiControlsParent controlsParent,
      MyPlayer controller,
      MyCubeGrid grid,
      MyTerminalBlock currentBlock,
      MyGridColorHelper colorHelper)
    {
      this.m_controlsParent = controlsParent;
      this.m_controller = controller;
      this.m_colorHelper = colorHelper;
      if (grid == null)
      {
        foreach (MyGuiControlBase control in controlsParent.Controls)
          control.Visible = false;
        MyGuiControlLabel errorLabel = MyGuiScreenTerminal.CreateErrorLabel(MySpaceTexts.ScreenTerminalError_ShipNotConnected, "ErrorMessage");
        controlsParent.Controls.Add((MyGuiControlBase) errorLabel);
      }
      else
      {
        this.TerminalSystem = grid.GridSystems.TerminalSystem;
        this.m_tmpGroup = new MyBlockGroup();
        this.m_terminalScreen.GetGroupInjectableControls(ref this.m_blockNameLabel, ref this.m_groupName, ref this.m_groupSave, ref this.m_groupDelete);
        this.m_searchBox = (MyGuiControlSearchBox) this.m_controlsParent.Controls.GetControlByName("FunctionalBlockSearch");
        this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.blockSearch_TextChanged);
        this.m_blockListbox = (MyGuiControlListbox) this.m_controlsParent.Controls.GetControlByName("FunctionalBlockListbox");
        this.m_blockNameLabel.Text = "";
        this.m_groupName.TextChanged += new Action<MyGuiControlTextbox>(this.m_groupName_TextChanged);
        this.m_groupName.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroup));
        this.m_groupName.ShowTooltipWhenDisabled = true;
        this.m_showAll = (MyGuiControlButton) this.m_controlsParent.Controls.GetControlByName("ShowAll");
        this.m_showAll.Selected = MyTerminalControlPanel.m_showAllTerminalBlocks;
        this.m_showAll.ButtonClicked += new Action<MyGuiControlButton>(this.showAll_Clicked);
        this.m_showAll.SetToolTip(MySpaceTexts.Terminal_ShowAllInTerminal);
        this.m_showAll.IconRotation = 0.0f;
        this.m_showAll.Icon = new MyGuiHighlightTexture?(MyTerminalControlPanel.ICON_UNHIDE);
        this.m_showAll.Size = new Vector2(0.0f, 0.0f);
        this.m_groupSave.TextEnum = MySpaceTexts.TerminalButton_GroupSave;
        this.m_groupSave.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        this.m_groupSave.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
        this.m_groupSave.ButtonClicked += new Action<MyGuiControlButton>(this.groupSave_ButtonClicked);
        this.m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSave));
        this.m_groupSave.ShowTooltipWhenDisabled = true;
        this.m_groupDelete.ButtonClicked += new Action<MyGuiControlButton>(this.groupDelete_ButtonClicked);
        this.m_groupDelete.ShowTooltipWhenDisabled = true;
        this.m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDeleteDisabled));
        this.m_groupDelete.Enabled = false;
        this.m_blockListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.blockListbox_ItemSelected);
        this.m_originalBlock = currentBlock;
        MyTerminalBlock[] selectedBlocks = (MyTerminalBlock[]) null;
        if (this.m_originalBlock != null)
          selectedBlocks = new MyTerminalBlock[1]
          {
            this.m_originalBlock
          };
        this.RefreshBlockList(selectedBlocks);
        this.TerminalSystem.BlockAdded += new Action<MyTerminalBlock>(this.TerminalSystem_BlockAdded);
        this.TerminalSystem.BlockRemoved += new Action<MyTerminalBlock>(this.TerminalSystem_BlockRemoved);
        this.TerminalSystem.BlockManipulationFinished += new Action(this.TerminalSystem_BlockManipulationFinished);
        this.TerminalSystem.GroupAdded += new Action<MyBlockGroup>(this.TerminalSystem_GroupAdded);
        this.TerminalSystem.GroupRemoved += new Action<MyBlockGroup>(this.TerminalSystem_GroupRemoved);
        this.blockSearch_TextChanged(this.m_searchBox.SearchText);
        this.m_blockListbox.ScrollToFirstSelection();
      }
    }

    private void m_groupName_TextChanged(MyGuiControlTextbox obj)
    {
      if (string.IsNullOrEmpty(obj.Text) || this.CurrentBlocks.Count == 0)
      {
        this.m_groupSave.Enabled = false;
        this.m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSaveDisabled));
      }
      else
      {
        this.m_groupSave.Enabled = true;
        this.m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSave));
      }
    }

    private void TerminalSystem_GroupRemoved(MyBlockGroup group)
    {
      if (this.m_blockListbox == null)
        return;
      foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
      {
        if (obj.UserData == group)
        {
          this.m_blockListbox.Items.Remove(obj);
          break;
        }
      }
    }

    private void TerminalSystem_GroupAdded(MyBlockGroup group)
    {
      if (this.m_blockListbox == null)
        return;
      this.AddGroupToList(group, new int?(0));
    }

    private void groupDelete_ButtonClicked(MyGuiControlButton obj)
    {
      bool flag = false;
      foreach (MyBlockGroup currentGroup in this.m_currentGroups)
      {
        foreach (MyTerminalBlock block in currentGroup.Blocks)
        {
          if (!block.HasLocalPlayerAccess())
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCannotDeleteGroup), messageCaption: messageCaption));
      }
      else
      {
        while (this.m_currentGroups.Count > 0)
          this.TerminalSystem.RemoveGroup(this.m_currentGroups[0], true);
      }
    }

    private void showAll_Clicked(MyGuiControlButton obj)
    {
      MyTerminalControlPanel.m_showAllTerminalBlocks = !MyTerminalControlPanel.m_showAllTerminalBlocks;
      this.m_showAll.Selected = MyTerminalControlPanel.m_showAllTerminalBlocks;
      List<MyGuiControlListbox.Item> selectedItems = this.m_blockListbox.SelectedItems;
      MyTerminalBlock[] selectedBlocks = new MyTerminalBlock[selectedItems.Count];
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        if (selectedItems[index].UserData is MyTerminalBlock)
          selectedBlocks[index] = (MyTerminalBlock) selectedItems[index].UserData;
      }
      this.ClearBlockList();
      this.PopulateBlockList(selectedBlocks);
      this.m_blockListbox.ScrollToolbarToTop();
      this.blockSearch_TextChanged(this.m_searchBox.SearchText);
      this.UpdateShowAllTextures();
    }

    private void UpdateShowAllTextures()
    {
      if (MyTerminalControlPanel.m_showAllTerminalBlocks)
        this.m_showAll.Icon = new MyGuiHighlightTexture?(MyTerminalControlPanel.ICON_HIDE);
      else
        this.m_showAll.Icon = new MyGuiHighlightTexture?(MyTerminalControlPanel.ICON_UNHIDE);
    }

    private void groupSave_ButtonClicked(MyGuiControlButton obj)
    {
      bool flag = false;
      foreach (MyTerminalBlock block in this.m_tmpGroup.Blocks)
      {
        if (!block.HasLocalPlayerAccess())
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCannotCreateGroup), messageCaption: messageCaption));
      }
      else
      {
        if (!(this.m_groupName.Text != ""))
          return;
        this.m_currentGroups.Clear();
        this.m_tmpGroup.Name.Clear().Append(this.m_groupName.Text);
        this.m_tmpGroup = this.TerminalSystem.AddUpdateGroup(this.m_tmpGroup, true, true);
        this.m_currentGroups.Add(this.m_tmpGroup);
        this.m_tmpGroup = new MyBlockGroup();
        this.CurrentBlocks.UnionWith((IEnumerable<MyTerminalBlock>) this.m_currentGroups[0].Blocks);
        this.SelectBlocks();
      }
    }

    internal void SetTerminalScreen(MyGuiScreenTerminal terminalScreen) => this.m_terminalScreen = terminalScreen;

    private void blockSearch_TextChanged(string text)
    {
      if (this.m_blockListbox == null)
        return;
      if (text != "")
      {
        string[] strArray = text.Split(' ');
        foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
        {
          bool flag = true;
          if (obj.UserData is MyTerminalBlock)
            flag = ((MyTerminalBlock) obj.UserData).ShowInTerminal || MyTerminalControlPanel.m_showAllTerminalBlocks || obj.UserData == this.m_originalBlock;
          if (flag)
          {
            string lower = obj.Text.ToString().ToLower();
            foreach (string str in strArray)
            {
              if (!lower.Contains(str.ToLower()))
              {
                flag = false;
                break;
              }
            }
            obj.Visible = flag;
          }
        }
      }
      else
      {
        foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
        {
          if (obj.UserData is MyTerminalBlock)
          {
            MyTerminalBlock userData = (MyTerminalBlock) obj.UserData;
            obj.Visible = userData.ShowInTerminal || MyTerminalControlPanel.m_showAllTerminalBlocks || userData == this.m_originalBlock;
          }
          else
            obj.Visible = true;
        }
      }
      this.m_blockListbox.ScrollToolbarToTop();
    }

    private void TerminalSystem_BlockAdded(MyTerminalBlock obj) => this.AddBlockToList(obj);

    private void TerminalSystem_BlockRemoved(MyTerminalBlock obj)
    {
      obj.CustomNameChanged -= new Action<MyTerminalBlock>(this.block_CustomNameChanged);
      obj.PropertiesChanged -= new Action<MyTerminalBlock>(this.block_CustomNameChanged);
      if (this.m_blockListbox == null || !obj.ShowInTerminal && !MyTerminalControlPanel.m_showAllTerminalBlocks)
        return;
      this.m_blockListbox.Remove((Predicate<MyGuiControlListbox.Item>) (item => item.UserData == obj));
    }

    private void TerminalSystem_BlockManipulationFinished() => this.blockSearch_TextChanged(this.m_searchBox.SearchText);

    public void Close()
    {
      if (this.TerminalSystem != null)
      {
        if (this.m_blockListbox != null)
        {
          this.ClearBlockList();
          this.m_blockListbox.ItemsSelected -= new Action<MyGuiControlListbox>(this.blockListbox_ItemSelected);
        }
        this.TerminalSystem.BlockAdded -= new Action<MyTerminalBlock>(this.TerminalSystem_BlockAdded);
        this.TerminalSystem.BlockRemoved -= new Action<MyTerminalBlock>(this.TerminalSystem_BlockRemoved);
        this.TerminalSystem.BlockManipulationFinished -= new Action(this.TerminalSystem_BlockManipulationFinished);
        this.TerminalSystem.GroupAdded -= new Action<MyBlockGroup>(this.TerminalSystem_GroupAdded);
        this.TerminalSystem.GroupRemoved -= new Action<MyBlockGroup>(this.TerminalSystem_GroupRemoved);
      }
      if (this.m_tmpGroup != null)
        this.m_tmpGroup.Blocks.Clear();
      if (this.m_showAll != null)
        this.m_showAll.ButtonClicked -= new Action<MyGuiControlButton>(this.showAll_Clicked);
      this.m_controlsParent = (IMyGuiControlsParent) null;
      this.m_blockListbox = (MyGuiControlListbox) null;
      this.m_blockNameLabel = (MyGuiControlLabel) null;
      this.TerminalSystem = (MyGridTerminalSystem) null;
      this.m_currentGroups.Clear();
    }

    public void RefreshBlockList(MyTerminalBlock[] selectedBlocks = null)
    {
      if (this.m_blockListbox == null)
        return;
      this.ClearBlockList();
      this.PopulateBlockList(selectedBlocks);
    }

    public void ClearBlockList()
    {
      if (this.m_blockListbox == null)
        return;
      foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
      {
        if (obj.UserData is MyTerminalBlock)
        {
          MyTerminalBlock userData = (MyTerminalBlock) obj.UserData;
          userData.CustomNameChanged -= new Action<MyTerminalBlock>(this.block_CustomNameChanged);
          userData.PropertiesChanged -= new Action<MyTerminalBlock>(this.block_CustomNameChanged);
          userData.ShowInTerminalChanged -= new Action<MyTerminalBlock>(this.block_ShowInTerminalChanged_Delayed);
        }
      }
      this.m_blockListbox.Items.Clear();
    }

    public void PopulateBlockList(MyTerminalBlock[] selectedBlocks = null)
    {
      if (this.TerminalSystem == null)
        return;
      if (this.TerminalSystem.BlockGroups == null)
        MySandboxGame.Log.WriteLine("m_terminalSystem.BlockGroups is null");
      HashSetReader<MyTerminalBlock> blocks = this.TerminalSystem.Blocks;
      if (!blocks.IsValid)
        MySandboxGame.Log.WriteLine("m_terminalSystem.Blocks.IsValid is false");
      if (this.CurrentBlocks == null)
        MySandboxGame.Log.WriteLine("CurrentBlocks is null");
      if (this.m_blockListbox == null)
        MySandboxGame.Log.WriteLine("m_blockListbox is null");
      MyBlockGroup[] array1 = this.TerminalSystem.BlockGroups.ToArray();
      Array.Sort<MyBlockGroup>(array1, (IComparer<MyBlockGroup>) MyTerminalComparer.Static);
      foreach (MyBlockGroup group in array1)
        this.AddGroupToList(group);
      blocks = this.TerminalSystem.Blocks;
      MyTerminalBlock[] array2 = blocks.ToArray();
      Array.Sort<MyTerminalBlock>(array2, (IComparer<MyTerminalBlock>) MyTerminalComparer.Static);
      this.m_blockListbox.SelectedItems.Clear();
      this.m_blockListbox.IsInBulkInsert = true;
      foreach (MyTerminalBlock block in array2)
        this.AddBlockToList(block, new bool?(block == this.m_originalBlock || block.ShowInTerminal || MyTerminalControlPanel.m_showAllTerminalBlocks));
      this.m_blockListbox.IsInBulkInsert = false;
      if (selectedBlocks == null)
      {
        if (this.CurrentBlocks.Count > 0)
        {
          this.SelectBlocks();
        }
        else
        {
          foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
          {
            if (obj.UserData is MyTerminalBlock)
            {
              this.SelectBlocks(new MyTerminalBlock[1]
              {
                (MyTerminalBlock) obj.UserData
              });
              break;
            }
          }
        }
      }
      else
        this.SelectBlocks(selectedBlocks);
    }

    private bool IsGeneric(MyBlockGroup group)
    {
      try
      {
        bool flag = true;
        foreach (MyTerminalBlock block in group.Blocks)
        {
          flag &= block is MyFunctionalBlock;
          MyTerminalControlPanel.tmpBlockTypes.Add(block.GetType());
        }
        return MyTerminalControlPanel.tmpBlockTypes.Count != 1;
      }
      finally
      {
        MyTerminalControlPanel.tmpBlockTypes.Clear();
      }
    }

    private void AddGroupToList(MyBlockGroup group, int? position = null)
    {
      foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
      {
        if (obj.UserData is MyBlockGroup userData && userData.Name.CompareTo(group.Name) == 0)
        {
          this.m_blockListbox.Items.Remove(obj);
          break;
        }
      }
      object userData1 = (object) group;
      MyGuiControlListbox.Item obj1 = new MyGuiControlListbox.Item(toolTip: group.Name.ToString(), icon: this.GetIconForGroup(group), userData: userData1);
      obj1.Text.Clear().Append("*").AppendStringBuilder(group.Name).Append("*");
      this.m_blockListbox.Add(obj1, position);
    }

    private string GetIconForBlock(MyTerminalBlock block) => block.BlockDefinition == null || block.BlockDefinition.Icons.IsNullOrEmpty<string>() ? MyGuiConstants.TEXTURE_ICON_FAKE.Texture : block.BlockDefinition.Icons[0];

    private string GetIconForGroup(MyBlockGroup group)
    {
      if (group == null || this.IsGeneric(group))
        return MyGuiConstants.TEXTURE_TERMINAL_GROUP;
      MyTerminalBlock myTerminalBlock = group.Blocks.First<MyTerminalBlock>();
      return myTerminalBlock.BlockDefinition == null || myTerminalBlock.BlockDefinition.Icons.IsNullOrEmpty<string>() ? MyGuiConstants.TEXTURE_TERMINAL_GROUP : myTerminalBlock.BlockDefinition.Icons[0];
    }

    private MyGuiControlListbox.Item AddBlockToList(
      MyTerminalBlock block,
      bool? visibility = null)
    {
      StringBuilder result = new StringBuilder();
      block.GetTerminalName(result);
      object userData = (object) block;
      MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(toolTip: result.ToString(), icon: this.GetIconForBlock(block), userData: userData);
      this.UpdateItemAppearance(block, obj);
      block.CustomNameChanged += new Action<MyTerminalBlock>(this.block_CustomNameChanged);
      block.PropertiesChanged += new Action<MyTerminalBlock>(this.block_CustomNameChanged);
      block.ShowInTerminalChanged += new Action<MyTerminalBlock>(this.block_ShowInTerminalChanged_Delayed);
      if (visibility.HasValue)
        obj.Visible = visibility.Value;
      this.m_blockListbox.Add(obj);
      return obj;
    }

    private void UpdateItemAppearance(MyTerminalBlock block, MyGuiControlListbox.Item item)
    {
      item.Text.Clear();
      block.GetTerminalName(item.Text);
      if (!block.IsFunctional)
      {
        item.ColorMask = new Vector4?(MyTerminalControlPanel.RED_TEXT_COLOR);
        item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockIncomplete));
      }
      else
      {
        MyTerminalBlock.AccessRightsResult accessRightsResult;
        if (this.m_controller != null && this.m_controller.Identity != null && (accessRightsResult = block.HasPlayerAccessReason(this.m_controller.Identity.IdentityId)) != MyTerminalBlock.AccessRightsResult.Granted)
        {
          item.ColorMask = new Vector4?(MyTerminalControlPanel.RED_TEXT_COLOR);
          if (accessRightsResult == MyTerminalBlock.AccessRightsResult.MissingDLC)
          {
            if (block.BlockDefinition == null || block.BlockDefinition.DLCs == null)
              return;
            foreach (string dlC in block.BlockDefinition.DLCs)
            {
              if (MyDLCs.TryGetDLC(dlC, out MyDLCs.MyDLC _))
                item.Text.Append(" (").Append((object) MyTexts.Get(MyCommonTexts.RequiresAnyDlc)).Append(")");
            }
          }
          else
            item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
        }
        else if (!block.ShowInTerminal)
        {
          Color? gridColor = this.m_colorHelper.GetGridColor(block.CubeGrid);
          item.ColorMask = new Vector4?(0.6f * (gridColor.HasValue ? gridColor.Value.ToVector4() : Vector4.One));
          item.FontOverride = (string) null;
        }
        else
        {
          if (this.m_controller != null && this.m_controller.Identity != null && (block.IDModule == null && block.CubeGrid != null))
          {
            List<long> bigOwners = block.CubeGrid.BigOwners;
            // ISSUE: explicit non-virtual call
            if ((bigOwners != null ? ((uint) __nonvirtual (bigOwners.Count) > 0U ? 1 : 0) : 1) != 0)
            {
              List<long> smallOwners = block.CubeGrid.SmallOwners;
              // ISSUE: explicit non-virtual call
              if ((smallOwners != null ? ((uint) __nonvirtual (smallOwners.Count) > 0U ? 1 : 0) : 1) != 0 && !block.HasLocalPlayerAdminUseTerminals() && !block.CubeGrid.SmallOwners.Contains(this.m_controller.Identity.IdentityId))
              {
                item.ColorMask = new Vector4?(MyTerminalControlPanel.RED_TEXT_COLOR);
                item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
                return;
              }
            }
          }
          Color? gridColor = this.m_colorHelper.GetGridColor(block.CubeGrid);
          if (gridColor.HasValue)
            item.ColorMask = new Vector4?(gridColor.Value.ToVector4());
          item.FontOverride = (string) null;
        }
      }
    }

    private void block_CustomNameChanged(MyTerminalBlock obj)
    {
      if (this.m_blockListbox == null)
        return;
      foreach (MyGuiControlListbox.Item obj1 in this.m_blockListbox.Items)
      {
        if (obj1.UserData == obj)
        {
          this.UpdateItemAppearance(obj, obj1);
          break;
        }
      }
      if (this.CurrentBlocks.Count <= 0 || this.CurrentBlocks.FirstElement<MyTerminalBlock>() != obj)
        return;
      this.m_blockNameLabel.Text = obj.CustomName.ToString();
    }

    public void SelectBlocks(MyTerminalBlock[] blocks)
    {
      this.m_tmpGroup.Blocks.Clear();
      this.m_tmpGroup.Blocks.UnionWith((IEnumerable<MyTerminalBlock>) blocks);
      this.m_currentGroups.Clear();
      this.CurrentBlocks.Clear();
      foreach (MyTerminalBlock block in blocks)
      {
        if (block != null)
          this.CurrentBlocks.Add(block);
      }
      this.SelectBlocks();
    }

    private void SelectBlocks()
    {
      if (this.m_blockControl != null)
      {
        this.m_controlsParent.Controls.Remove(this.m_blockControl);
        this.m_blockControl = (MyGuiControlBase) null;
      }
      this.m_blockNameLabel.Text = "";
      this.m_groupName.Text = "";
      if (this.m_currentGroups.Count == 1)
      {
        this.m_blockNameLabel.Text = this.m_currentGroups[0].Name.ToString();
        this.m_groupName.Text = this.m_blockNameLabel.Text;
      }
      if (this.CurrentBlocks.Count > 0)
      {
        if (this.CurrentBlocks.Count == 1)
          this.m_blockNameLabel.Text = this.CurrentBlocks.FirstElement<MyTerminalBlock>().CustomName.ToString();
        this.m_blockControl = (MyGuiControlBase) new MyGuiControlGenericFunctionalBlock(this.CurrentBlocks.ToArray<MyTerminalBlock>());
        this.m_controlsParent.Controls.Add(this.m_blockControl);
        this.m_blockControl.Size = new Vector2(0.595f, 0.64f);
        this.m_blockControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_blockControl.Position = new Vector2(-0.1415f, -0.3f);
      }
      this.UpdateGroupControl();
      this.m_blockListbox.SelectedItems.Clear();
      foreach (MyTerminalBlock currentBlock in this.CurrentBlocks)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
        {
          if (obj.UserData == currentBlock)
          {
            this.m_blockListbox.SelectedItems.Add(obj);
            break;
          }
        }
      }
      foreach (MyBlockGroup currentGroup in this.m_currentGroups)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_blockListbox.Items)
        {
          if (obj.UserData == currentGroup)
          {
            this.m_blockListbox.SelectedItems.Add(obj);
            break;
          }
        }
      }
    }

    public void SelectAllBlocks()
    {
      if (this.m_blockListbox == null)
        return;
      this.m_blockListbox.SelectAllVisible();
    }

    private void UpdateGroupControl()
    {
      if (this.m_currentGroups.Count > 0)
      {
        this.m_groupDelete.Enabled = true;
        this.m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDelete));
      }
      else
      {
        this.m_groupDelete.Enabled = false;
        this.m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDeleteDisabled));
      }
    }

    public void UpdateCubeBlock(MyTerminalBlock block)
    {
      if (block == null)
        return;
      if (this.TerminalSystem != null)
      {
        this.TerminalSystem.BlockAdded -= new Action<MyTerminalBlock>(this.TerminalSystem_BlockAdded);
        this.TerminalSystem.BlockRemoved -= new Action<MyTerminalBlock>(this.TerminalSystem_BlockRemoved);
        this.TerminalSystem.BlockManipulationFinished -= new Action(this.TerminalSystem_BlockManipulationFinished);
        this.TerminalSystem.GroupAdded -= new Action<MyBlockGroup>(this.TerminalSystem_GroupAdded);
        this.TerminalSystem.GroupRemoved -= new Action<MyBlockGroup>(this.TerminalSystem_GroupRemoved);
      }
      this.TerminalSystem = block.CubeGrid.GridSystems.TerminalSystem;
      this.m_tmpGroup = new MyBlockGroup();
      this.TerminalSystem.BlockAdded += new Action<MyTerminalBlock>(this.TerminalSystem_BlockAdded);
      this.TerminalSystem.BlockRemoved += new Action<MyTerminalBlock>(this.TerminalSystem_BlockRemoved);
      this.TerminalSystem.BlockManipulationFinished += new Action(this.TerminalSystem_BlockManipulationFinished);
      this.TerminalSystem.GroupAdded += new Action<MyBlockGroup>(this.TerminalSystem_GroupAdded);
      this.TerminalSystem.GroupRemoved += new Action<MyBlockGroup>(this.TerminalSystem_GroupRemoved);
      this.SelectBlocks(new MyTerminalBlock[1]{ block });
    }

    private void blockListbox_ItemSelected(MyGuiControlListbox sender)
    {
      this.m_oldGroups.Clear();
      this.m_oldGroups.AddRange((IEnumerable<MyBlockGroup>) this.m_currentGroups);
      this.m_currentGroups.Clear();
      this.m_tmpGroup.Blocks.Clear();
      foreach (MyGuiControlListbox.Item selectedItem in sender.SelectedItems)
      {
        if (selectedItem.UserData is MyBlockGroup)
          this.m_currentGroups.Add((MyBlockGroup) selectedItem.UserData);
        else if (selectedItem.UserData is MyTerminalBlock)
          this.CurrentBlocks.Add(selectedItem.UserData as MyTerminalBlock);
      }
      for (int index = 0; index < this.m_currentGroups.Count; ++index)
      {
        if (!this.m_oldGroups.Contains(this.m_currentGroups[index]) || this.m_currentGroups[index].Blocks.Intersect<MyTerminalBlock>((IEnumerable<MyTerminalBlock>) this.CurrentBlocks).Count<MyTerminalBlock>() == 0)
        {
          foreach (MyTerminalBlock block in this.m_currentGroups[index].Blocks)
          {
            if (!this.CurrentBlocks.Contains(block))
              this.CurrentBlocks.Add(block);
          }
        }
      }
      this.SelectBlocks();
    }

    private void block_ShowInTerminalChanged_Delayed(MyTerminalBlock obj)
    {
      if ((long) this.m_last_showInTerminalChanged == (long) MySandboxGame.Static.SimulationFrameCounter)
        return;
      this.m_last_showInTerminalChanged = MySandboxGame.Static.SimulationFrameCounter;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        MyTerminalBlock[] selectedBlocks = (MyTerminalBlock[]) null;
        if (this.m_blockListbox != null)
        {
          List<MyGuiControlListbox.Item> selectedItems = this.m_blockListbox.SelectedItems;
          selectedBlocks = new MyTerminalBlock[selectedItems.Count];
          for (int index = 0; index < selectedItems.Count; ++index)
          {
            if (selectedItems[index].UserData is MyTerminalBlock userData)
              selectedBlocks[index] = userData;
          }
        }
        this.ClearBlockList();
        this.PopulateBlockList(selectedBlocks);
        if (this.m_blockListbox != null)
          this.m_blockListbox.ScrollToolbarToTop();
        this.blockSearch_TextChanged(this.m_searchBox.SearchText);
      }), "ShowInTerminalChanged");
    }

    public override void HandleInput()
    {
      base.HandleInput();
      if (this.m_blockListbox != null && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
      {
        List<MyGuiControlListbox.Item> selectedItems = this.m_blockListbox.SelectedItems;
        for (int index = 0; index < selectedItems.Count; ++index)
        {
          if (selectedItems[index].UserData is MyFunctionalBlock userData)
            userData.Enabled = !userData.Enabled;
        }
      }
      if (this.m_blockListbox != null && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y))
      {
        List<MyGuiControlListbox.Item> selectedItems = this.m_blockListbox.SelectedItems;
        for (int index = 0; index < selectedItems.Count; ++index)
        {
          if (selectedItems[index].UserData is MyTerminalBlock userData)
            userData.ShowInTerminal = !userData.ShowInTerminal;
        }
      }
      if (this.m_blockListbox != null && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
      {
        List<MyGuiControlListbox.Item> selectedItems = this.m_blockListbox.SelectedItems;
        for (int index = 0; index < selectedItems.Count; ++index)
        {
          if (selectedItems[index].UserData is MyTerminalBlock userData)
            userData.ShowOnHUD = !userData.ShowOnHUD;
        }
      }
      if (this.m_blockListbox == null || !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
        return;
      List<MyGuiControlListbox.Item> selectedItems1 = this.m_blockListbox.SelectedItems;
      for (int index = 0; index < selectedItems1.Count; ++index)
      {
        if (selectedItems1[index].UserData is MyTerminalBlock userData)
          userData.ShowInToolbarConfig = !userData.ShowInToolbarConfig;
      }
    }
  }
}
