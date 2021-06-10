// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyGuiControlGenericFunctionalBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
  public class MyGuiControlGenericFunctionalBlock : MyGuiControlBase
  {
    private List<ITerminalControl> m_currentControls = new List<ITerminalControl>();
    private MyGuiControlSeparatorList m_separatorList;
    private MyGuiControlList m_terminalControlList;
    private MyGuiControlMultilineText m_blockPropertiesMultilineText;
    private MyTerminalBlock[] m_currentBlocks;
    private Dictionary<ITerminalControl, int> m_tmpControlDictionary = new Dictionary<ITerminalControl, int>((IEqualityComparer<ITerminalControl>) InstanceComparer<ITerminalControl>.Default);
    private bool m_recreatingControls;
    private MyGuiControlCombobox m_transferToCombobox;
    private MyGuiControlCombobox m_shareModeCombobox;
    private MyGuiControlLabel m_ownershipLabel;
    private MyGuiControlLabel m_ownerLabel;
    private MyGuiControlLabel m_transferToLabel;
    private MyGuiControlLabel m_shareWithLabel;
    private MyGuiControlButton m_npcButton;
    private bool m_isDetailedInfoBeingUpdated;
    private List<MyCubeGrid.MySingleOwnershipRequest> m_requests = new List<MyCubeGrid.MySingleOwnershipRequest>();
    private bool m_askForConfirmation = true;
    private bool m_canChangeShareMode = true;
    private bool m_propertiesChanged;
    private MyScenarioBuildingBlock dummy = new MyScenarioBuildingBlock();

    internal MyGuiControlGenericFunctionalBlock(MyTerminalBlock block)
      : this(new MyTerminalBlock[1]{ block })
    {
    }

    internal MyGuiControlGenericFunctionalBlock(MyTerminalBlock[] blocks)
      : base(isActiveControl: false, canHaveFocus: true)
    {
      this.m_currentBlocks = blocks;
      this.m_separatorList = new MyGuiControlSeparatorList();
      this.Elements.Add((MyGuiControlBase) this.m_separatorList);
      this.m_terminalControlList = new MyGuiControlList();
      this.m_terminalControlList.VisualStyle = MyGuiControlListStyleEnum.Simple;
      this.m_terminalControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_terminalControlList.Position = new Vector2(0.1f, 0.1f);
      this.Elements.Add((MyGuiControlBase) this.m_terminalControlList);
      this.m_blockPropertiesMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(0.05f, -0.195f)), new Vector2?(new Vector2(0.4f, 0.635f)), textScale: 0.85f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_blockPropertiesMultilineText.CanHaveFocus = true;
      this.m_blockPropertiesMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_blockPropertiesMultilineText.Text = new StringBuilder();
      this.Elements.Add((MyGuiControlBase) this.m_blockPropertiesMultilineText);
      this.m_transferToCombobox = new MyGuiControlCombobox(new Vector2?(Vector2.Zero), new Vector2?(new Vector2(0.245f, 0.1f)));
      this.m_transferToCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_transferToCombobox_ItemSelected);
      this.m_transferToCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_TransferCombobox));
      this.m_transferToCombobox.ShowTooltipWhenDisabled = true;
      this.Elements.Add((MyGuiControlBase) this.m_transferToCombobox);
      this.m_shareModeCombobox = new MyGuiControlCombobox(new Vector2?(Vector2.Zero), new Vector2?(new Vector2(0.287f, 0.1f)));
      this.m_shareModeCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_shareModeCombobox_ItemSelected);
      this.m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareCombobox));
      this.m_shareModeCombobox.ShowTooltipWhenDisabled = true;
      this.Elements.Add((MyGuiControlBase) this.m_shareModeCombobox);
      this.m_ownershipLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: (MyTexts.GetString(MySpaceTexts.BlockOwner_Owner) + ":"));
      this.Elements.Add((MyGuiControlBase) this.m_ownershipLabel);
      this.m_ownerLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: string.Empty);
      this.Elements.Add((MyGuiControlBase) this.m_ownerLabel);
      this.m_transferToLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: MyTexts.GetString(MySpaceTexts.BlockOwner_TransferTo));
      this.Elements.Add((MyGuiControlBase) this.m_transferToLabel);
      this.m_shareWithLabel = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: MyTexts.GetString(MySpaceTexts.ControlScreen_ShareLabel));
      this.Elements.Add((MyGuiControlBase) this.m_shareWithLabel);
      this.m_npcButton = new MyGuiControlButton(new Vector2?(new Vector2(0.27f, -0.13f)), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(0.04f, 0.053f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip: MyTexts.GetString(MyCommonTexts.AddNewNPC), text: new StringBuilder("+"), onButtonClick: new Action<MyGuiControlButton>(this.OnNewNpcClick), buttonScale: 0.75f);
      this.Elements.Add((MyGuiControlBase) this.m_npcButton);
      this.m_npcButton.Enabled = false;
      this.m_npcButton.Enabled = MySession.Static.IsUserSpaceMaster(Sync.MyId);
      this.RecreateBlockControls();
      this.RecreateOwnershipControls();
      if (this.m_currentBlocks.Length != 0)
      {
        this.m_currentBlocks[0].PropertiesChanged += new Action<MyTerminalBlock>(this.OnPropertiesChanged);
        this.m_currentBlocks[0].IsOpenedInTerminal = true;
        this.m_currentBlocks[0].OnOpenedInTerminal(true);
      }
      foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
      {
        currentBlock.OwnershipChanged += new Action<MyTerminalBlock>(this.block_OwnershipChanged);
        currentBlock.VisibilityChanged += new Action<MyTerminalBlock>(this.block_VisibilityChanged);
      }
      Sync.Players.IdentitiesChanged += new Action(this.Players_IdentitiesChanged);
      if (this.m_currentBlocks.Length == 1)
        this.m_currentBlocks[0].SetDetailedInfoDirty();
      this.UpdateDetailedInfo();
      this.Size = new Vector2(0.595f, 0.64f);
      this.CanFocusChildren = true;
    }

    private void Players_IdentitiesChanged() => this.UpdateOwnerGui();

    private void block_OwnershipChanged(MyTerminalBlock sender)
    {
      if (!this.m_canChangeShareMode)
        return;
      this.RecreateOwnershipControls();
      this.UpdateOwnerGui();
    }

    public override void OnRemoving()
    {
      this.m_currentControls.Clear();
      if (this.m_currentBlocks.Length != 0)
      {
        this.m_currentBlocks[0].PropertiesChanged -= new Action<MyTerminalBlock>(this.OnPropertiesChanged);
        this.m_currentBlocks[0].IsOpenedInTerminal = false;
        this.m_currentBlocks[0].OnOpenedInTerminal(false);
      }
      foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
      {
        currentBlock.OwnershipChanged -= new Action<MyTerminalBlock>(this.block_OwnershipChanged);
        currentBlock.VisibilityChanged -= new Action<MyTerminalBlock>(this.block_VisibilityChanged);
      }
      Sync.Players.IdentitiesChanged -= new Action(this.Players_IdentitiesChanged);
      base.OnRemoving();
    }

    private void block_VisibilityChanged(MyTerminalBlock obj)
    {
      foreach (ITerminalControl currentControl in this.m_currentControls)
      {
        if (currentControl.GetGuiControl().Visible != currentControl.IsVisible(obj))
          currentControl.GetGuiControl().Visible = currentControl.IsVisible(obj);
      }
    }

    public override void Update()
    {
      base.Update();
      if (!this.m_propertiesChanged)
        return;
      this.UpdateBlockControls();
    }

    private void OnPropertiesChanged(MyTerminalBlock sender) => this.m_propertiesChanged = true;

    private void UpdateBlockControls()
    {
      if (!this.m_canChangeShareMode)
        return;
      MyScrollbar scrollBar = this.m_terminalControlList.GetScrollBar();
      float num = scrollBar.Value;
      this.RecreateBlockControls();
      foreach (ITerminalControl currentControl in this.m_currentControls)
        currentControl.UpdateVisual();
      scrollBar.Value = MathHelper.Min(num, scrollBar.MaxSize);
      this.UpdateDetailedInfo();
      this.m_propertiesChanged = false;
    }

    private void UpdateDetailedInfo()
    {
      if (this.m_isDetailedInfoBeingUpdated)
        return;
      this.m_isDetailedInfoBeingUpdated = true;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      if (this.m_currentBlocks.Length == 1)
      {
        MyTerminalBlock currentBlock = this.m_currentBlocks[0];
        stringBuilder.AppendStringBuilder(currentBlock.DetailedInfo);
        if (currentBlock.CustomInfo.Length > 0)
        {
          stringBuilder.TrimTrailingWhitespace().AppendLine();
          stringBuilder.AppendStringBuilder(currentBlock.CustomInfo);
        }
        stringBuilder.Autowrap(0.26f, "Blue", 0.8f * MyGuiManager.LanguageTextScale);
        flag = true;
      }
      this.m_blockPropertiesMultilineText.Text.Clear();
      if (flag)
      {
        this.m_blockPropertiesMultilineText.Text = stringBuilder;
        this.m_blockPropertiesMultilineText.RefreshText(false);
      }
      this.m_isDetailedInfoBeingUpdated = false;
    }

    private void RecreateBlockControls()
    {
      if (this.m_recreatingControls)
        return;
      int index1 = -1;
      for (int index2 = 0; index2 < this.m_terminalControlList.Controls.Count; ++index2)
      {
        if (this.m_terminalControlList.Controls[index2].HasFocus)
          index1 = index2;
      }
      this.m_currentControls.Clear();
      this.m_terminalControlList.Controls.Clear();
      try
      {
        this.m_recreatingControls = true;
        foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
        {
          currentBlock.GetType();
          foreach (ITerminalControl control in MyTerminalControls.Static.GetControls((IMyTerminalBlock) currentBlock))
          {
            if (control != null)
            {
              int num;
              this.m_tmpControlDictionary.TryGetValue(control, out num);
              this.m_tmpControlDictionary[control] = num + (control.IsVisible(currentBlock) ? 1 : 0);
            }
          }
        }
        if (MySession.Static.Settings.ScenarioEditMode && MyFakes.ENABLE_NEW_TRIGGERS)
        {
          foreach (ITerminalControl control in MyTerminalControlFactory.GetControls(typeof (MyTerminalBlock)))
            this.m_tmpControlDictionary[control] = this.m_currentBlocks.Length;
        }
        int length = this.m_currentBlocks.Length;
        foreach (KeyValuePair<ITerminalControl, int> tmpControl in this.m_tmpControlDictionary)
        {
          bool flag = (uint) tmpControl.Value > 0U;
          if ((length <= 1 || tmpControl.Key.SupportsMultipleBlocks) && (tmpControl.Value == length && tmpControl.Key.GetGuiControl() != null))
          {
            tmpControl.Key.GetGuiControl().Visible = flag;
            this.m_terminalControlList.Controls.Add(tmpControl.Key.GetGuiControl());
            tmpControl.Key.TargetBlocks = this.m_currentBlocks;
            tmpControl.Key.UpdateVisual();
            this.m_currentControls.Add(tmpControl.Key);
          }
        }
      }
      finally
      {
        if (index1 != -1)
        {
          if (index1 >= this.m_terminalControlList.Controls.Count)
            index1 = this.m_terminalControlList.Controls.Count - 1;
          this.GetTopMostOwnerScreen().FocusedControl = this.m_terminalControlList.Controls[index1];
        }
        this.m_tmpControlDictionary.Clear();
        this.m_recreatingControls = false;
      }
    }

    private void RecreateOwnershipControls()
    {
      bool flag = false;
      foreach (MyCubeBlock currentBlock in this.m_currentBlocks)
      {
        if (currentBlock.IDModule != null)
          flag = true;
      }
      if (flag && MyFakes.SHOW_FACTIONS_GUI)
      {
        this.m_ownershipLabel.Visible = true;
        this.m_ownerLabel.Visible = true;
        this.m_transferToLabel.Visible = true;
        this.m_shareWithLabel.Visible = true;
        this.m_transferToCombobox.Visible = true;
        this.m_shareModeCombobox.Visible = true;
        if (this.m_npcButton != null)
          this.m_npcButton.Visible = true;
        Vector2 vector2_1 = Vector2.One * -0.5f;
        Vector2 vector2_2 = new Vector2(0.3f, 0.55f);
        this.m_ownershipLabel.Position = vector2_1 + new Vector2(vector2_2.X + 0.212f, 0.315f);
        this.m_ownerLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_ownerLabel.Position = this.m_ownershipLabel.Position + new Vector2(this.m_ownershipLabel.Size.X + 0.015f, 0.0f);
        this.m_transferToLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_transferToLabel.Position = vector2_1 + new Vector2(vector2_2.X + 0.212f, 0.335f);
        this.m_transferToCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_transferToCombobox.Position = vector2_1 + new Vector2(vector2_2.X + 0.212f, 0.368f);
        this.m_shareWithLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_shareWithLabel.Position = vector2_1 + new Vector2(vector2_2.X + 0.212f, 0.42f);
        this.m_shareModeCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_shareModeCombobox.Position = vector2_1 + new Vector2(vector2_2.X + 0.212f, 0.45f);
        this.m_shareModeCombobox.ClearItems();
        this.m_shareModeCombobox.AddItem(0L, MyTexts.Get(MySpaceTexts.BlockOwner_ShareNone));
        this.m_shareModeCombobox.AddItem(1L, MyTexts.Get(MySpaceTexts.BlockOwner_ShareFaction));
        this.m_shareModeCombobox.AddItem(2L, MyTexts.Get(MySpaceTexts.BlockOwner_ShareAll));
        this.UpdateOwnerGui();
      }
      else
      {
        this.m_ownershipLabel.Visible = false;
        this.m_ownerLabel.Visible = false;
        this.m_transferToLabel.Visible = false;
        this.m_shareWithLabel.Visible = false;
        this.m_transferToCombobox.Visible = false;
        this.m_shareModeCombobox.Visible = false;
        if (this.m_npcButton == null)
          return;
        this.m_npcButton.Visible = false;
      }
    }

    public override MyGuiControlBase HandleInput()
    {
      base.HandleInput();
      return this.HandleInputElements();
    }

    protected override void OnSizeChanged()
    {
      if (this.m_currentBlocks.Length == 0)
        return;
      Vector2 vector2_1 = this.Size * -0.5f;
      Vector2 vector2_2 = new Vector2(0.3f, 0.55f);
      this.m_separatorList.Clear();
      this.m_separatorList.AddHorizontal(vector2_1 + new Vector2(vector2_2.X + 0.008f, 0.11f), vector2_2.X * 0.96f);
      this.m_terminalControlList.Position = vector2_1 + new Vector2((float) ((double) vector2_2.X * 0.5 - 3.0 / 500.0), -0.032f);
      this.m_terminalControlList.Size = new Vector2(vector2_2.X - 0.013f, 0.675f);
      float num = 0.06f;
      if (MyFakes.SHOW_FACTIONS_GUI)
      {
        foreach (MyCubeBlock currentBlock in this.m_currentBlocks)
        {
          if (currentBlock.IDModule != null)
          {
            num = 0.22f;
            this.m_separatorList.AddHorizontal(vector2_1 + new Vector2(vector2_2.X + 0.008f, num + 0.11f), vector2_2.X * 0.96f);
            break;
          }
        }
      }
      this.m_blockPropertiesMultilineText.Position = vector2_1 + new Vector2(vector2_2.X + 0.012f, num + 0.133f);
      this.m_blockPropertiesMultilineText.Size = 0.5f * this.Size - this.m_blockPropertiesMultilineText.Position + new Vector2(0.03f, 0.0f);
      base.OnSizeChanged();
    }

    private void m_shareModeCombobox_ItemSelected()
    {
      if (!this.m_canChangeShareMode)
        return;
      this.m_canChangeShareMode = false;
      bool flag = false;
      MyOwnershipShareModeEnum selectedKey = (MyOwnershipShareModeEnum) this.m_shareModeCombobox.GetSelectedKey();
      if (this.m_currentBlocks.Length != 0)
      {
        this.m_requests.Clear();
        foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
        {
          if (currentBlock.IDModule != null && selectedKey >= MyOwnershipShareModeEnum.None && (currentBlock.OwnerId == MySession.Static.LocalPlayerId || MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals)))
          {
            this.m_requests.Add(new MyCubeGrid.MySingleOwnershipRequest()
            {
              BlockId = currentBlock.EntityId,
              Owner = currentBlock.IDModule.Owner
            });
            flag = true;
          }
        }
        if (this.m_requests.Count > 0)
          MyCubeGrid.ChangeOwnersRequest(selectedKey, this.m_requests, MySession.Static.LocalPlayerId);
      }
      this.m_canChangeShareMode = true;
      if (!flag)
        return;
      this.OnPropertiesChanged((MyTerminalBlock) null);
    }

    private void m_transferToCombobox_ItemSelected()
    {
      if (this.m_transferToCombobox.GetSelectedIndex() == -1)
        return;
      if (this.m_askForConfirmation)
      {
        long ownerKey = this.m_transferToCombobox.GetSelectedKey();
        StringBuilder stringBuilder = this.m_transferToCombobox.GetItemByIndex(this.m_transferToCombobox.GetSelectedIndex()).Value;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextChangeOwner), (object) stringBuilder.ToString()), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
        {
          if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
          {
            if (this.m_currentBlocks.Length != 0)
            {
              this.m_requests.Clear();
              bool flag = MySession.Static.IsUserAdmin(Sync.MyId) && MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
              foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
              {
                if (currentBlock.IDModule != null && (currentBlock.OwnerId == 0L | flag || currentBlock.OwnerId == MySession.Static.LocalPlayerId))
                  this.m_requests.Add(new MyCubeGrid.MySingleOwnershipRequest()
                  {
                    BlockId = currentBlock.EntityId,
                    Owner = ownerKey
                  });
              }
              if (this.m_requests.Count > 0)
              {
                if (flag && Sync.Players.IdentityIsNpc(ownerKey))
                  MyCubeGrid.ChangeOwnersRequest(MyOwnershipShareModeEnum.Faction, this.m_requests, MySession.Static.LocalPlayerId);
                else if (MySession.Static.LocalPlayerId == ownerKey)
                  MyCubeGrid.ChangeOwnersRequest(MyOwnershipShareModeEnum.Faction, this.m_requests, MySession.Static.LocalPlayerId);
                else
                  MyCubeGrid.ChangeOwnersRequest(MyOwnershipShareModeEnum.None, this.m_requests, MySession.Static.LocalPlayerId);
              }
            }
            this.RecreateOwnershipControls();
            this.UpdateOwnerGui();
          }
          else
          {
            this.m_askForConfirmation = false;
            this.m_transferToCombobox.SelectItemByIndex(-1);
            this.m_askForConfirmation = true;
          }
        })), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO);
        messageBox.CanHideOthers = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
      else
        this.UpdateOwnerGui();
    }

    private void UpdateOwnerGui()
    {
      long? owner;
      bool ownershipStatus = this.GetOwnershipStatus(out owner);
      this.m_transferToCombobox.ClearItems();
      if (!ownershipStatus && !owner.HasValue)
        return;
      if (ownershipStatus || owner.Value != 0L)
        this.m_transferToCombobox.AddItem(0L, MyTexts.Get(MySpaceTexts.BlockOwner_Nobody), sort: false);
      if (ownershipStatus || owner.Value != MySession.Static.LocalPlayerId)
        this.m_transferToCombobox.AddItem(MySession.Static.LocalPlayerId, MyTexts.Get(MySpaceTexts.BlockOwner_Me), sort: false);
      if (MySession.Static.IsUserAdmin(Sync.MyId))
      {
        foreach (KeyValuePair<long, MyIdentity> keyValuePair in (IEnumerable<KeyValuePair<long, MyIdentity>>) MySession.Static.Players.GetAllIdentitiesOrderByName())
        {
          if (keyValuePair.Value.IdentityId != MySession.Static.LocalPlayerId && !MySession.Static.Players.IdentityIsNpc(keyValuePair.Value.IdentityId) && (MySession.Static.LocalHumanPlayer.GetRelationTo(keyValuePair.Value.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies || MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
            this.m_transferToCombobox.AddItem(keyValuePair.Value.IdentityId, new StringBuilder(keyValuePair.Value.DisplayName), sort: false);
        }
      }
      else
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          MyIdentity identity = onlinePlayer.Identity;
          if (identity.IdentityId != MySession.Static.LocalPlayerId && !identity.IsDead && (MySession.Static.LocalHumanPlayer.GetRelationTo(identity.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies || MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
            this.m_transferToCombobox.AddItem(identity.IdentityId, new StringBuilder(identity.DisplayName), sort: false);
        }
      }
      foreach (long npcIdentity in Sync.Players.GetNPCIdentities())
      {
        MyIdentity identity = Sync.Players.TryGetIdentity(npcIdentity);
        if (identity != null)
        {
          int relationTo = (int) MySession.Static.LocalHumanPlayer.GetRelationTo(identity.IdentityId);
          if (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId))
            this.m_transferToCombobox.AddItem(identity.IdentityId, new StringBuilder(identity.DisplayName), sort: false);
        }
      }
      if (!ownershipStatus)
      {
        bool flag = MySession.Static.IsUserAdmin(Sync.MyId) && MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
        if (owner.Value == MySession.Static.LocalPlayerId | flag)
        {
          this.m_shareModeCombobox.Enabled = true;
          this.m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareCombobox));
        }
        else
        {
          this.m_shareModeCombobox.Enabled = false;
          this.m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareComboboxDisabled));
        }
        if (owner.Value == 0L)
        {
          this.m_transferToCombobox.Enabled = true;
          this.m_npcButton.Enabled = this.m_transferToCombobox.Enabled && MySession.Static.IsUserSpaceMaster(Sync.MyId);
          this.m_ownerLabel.TextEnum = MySpaceTexts.BlockOwner_Nobody;
        }
        else
        {
          this.m_transferToCombobox.Enabled = owner.Value == MySession.Static.LocalPlayerId | flag;
          this.m_npcButton.Enabled = this.m_transferToCombobox.Enabled && MySession.Static.IsUserSpaceMaster(Sync.MyId);
          this.m_ownerLabel.TextEnum = MySpaceTexts.BlockOwner_Me;
          if (owner.Value != MySession.Static.LocalPlayerId)
          {
            MyIdentity identity = Sync.Players.TryGetIdentity(owner.Value);
            if (identity != null)
              this.m_ownerLabel.Text = identity.DisplayName + (identity.IsDead ? " [" + MyTexts.Get(MyCommonTexts.PlayerInfo_Dead).ToString() + "]" : "");
            else
              this.m_ownerLabel.TextEnum = MySpaceTexts.BlockOwner_Unknown;
          }
        }
        MyOwnershipShareModeEnum? shareMode1;
        bool shareMode2 = this.GetShareMode(out shareMode1);
        this.m_canChangeShareMode = false;
        if (!shareMode2 && shareMode1.HasValue && owner.Value != 0L)
          this.m_shareModeCombobox.SelectItemByKey((long) shareMode1.Value);
        else
          this.m_shareModeCombobox.SelectItemByIndex(-1);
        this.m_canChangeShareMode = true;
      }
      else
      {
        this.m_shareModeCombobox.Enabled = true;
        this.m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareCombobox));
        this.m_ownerLabel.Text = "";
        this.m_canChangeShareMode = false;
        this.m_shareModeCombobox.SelectItemByIndex(-1);
        this.m_canChangeShareMode = true;
      }
      this.m_transferToCombobox.Sort();
    }

    private bool GetOwnershipStatus(out long? owner)
    {
      bool flag = false;
      owner = new long?();
      foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
      {
        if (currentBlock.IDModule != null)
        {
          if (!owner.HasValue)
            owner = new long?(currentBlock.IDModule.Owner);
          else if (owner.Value != currentBlock.IDModule.Owner)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private bool GetShareMode(out MyOwnershipShareModeEnum? shareMode)
    {
      bool flag = false;
      shareMode = new MyOwnershipShareModeEnum?();
      foreach (MyTerminalBlock currentBlock in this.m_currentBlocks)
      {
        if (currentBlock.IDModule != null)
        {
          if (!shareMode.HasValue)
            shareMode = new MyOwnershipShareModeEnum?(currentBlock.IDModule.ShareMode);
          else if (shareMode.Value != currentBlock.IDModule.ShareMode)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private void OnNewNpcClick(MyGuiControlButton button) => Sync.Players.RequestNewNpcIdentity();
  }
}
