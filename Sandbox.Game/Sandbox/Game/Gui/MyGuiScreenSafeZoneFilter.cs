// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenSafeZoneFilter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenSafeZoneFilter : MyGuiScreenDebugBase
  {
    public MyGuiControlListbox m_entityListbox;
    public MyGuiControlListbox m_restrictedListbox;
    private MyGuiControlCombobox m_accessCombobox;
    private MyGuiControlCombobox m_restrictionTypeCombobox;
    private MyGuiControlButton m_playersFilter;
    private MyGuiControlButton m_gridsFilter;
    private MyGuiControlButton m_floatingObjectsFilter;
    private MyGuiControlButton m_factionsFilter;
    private MyGuiControlButton m_moveLeftButton;
    private MyGuiControlButton m_moveLeftAllButton;
    private MyGuiControlButton m_moveRightButton;
    private MyGuiControlButton m_moveRightAllButton;
    private new MyGuiControlButton m_closeButton;
    private MyGuiControlButton m_addContainedToSafeButton;
    private MyGuiControlLabel m_modeLabel;
    private MyGuiControlLabel m_controlLabelList;
    private MyGuiControlLabel m_controlLabelEntity;
    public MySafeZone m_selectedSafeZone;
    private long? m_safeZoneBlockId;
    private MyGuiScreenAdminMenu.MyRestrictedTypeEnum m_selectedFilter;

    public MyGuiScreenSafeZoneFilter(Vector2 position, MySafeZone safeZone, long? safeZoneBlockId = null)
      : base(position, new Vector2?(new Vector2(0.7f, 0.644084f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity), true)
    {
      this.m_safeZoneBlockId = safeZoneBlockId;
      this.CloseButtonEnabled = true;
      this.m_selectedSafeZone = safeZone;
      this.RecreateControls(true);
      this.m_canCloseInCloseAllScreenCalls = true;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter), new Vector4?(Color.White.ToVector4()), new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.879999995231628 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.88f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.879999995231628 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.88f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_playersFilter = this.MakeButtonCategory(new Vector2(-0.293f, -0.205f), "Character", MyTexts.GetString(MyCommonTexts.JoinGame_ColumnTitle_Players), new Action<MyGuiControlButton>(this.OnFilterChange), MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player);
      this.m_factionsFilter = this.MakeButtonCategory(new Vector2(-0.257f, -0.205f), "Animation", MyTexts.GetString(MyCommonTexts.ScreenPlayers_Factions), new Action<MyGuiControlButton>(this.OnFilterChange), MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction);
      this.m_gridsFilter = this.MakeButtonCategory(new Vector2(-0.221f, -0.205f), "Block", MyTexts.GetString(MySpaceTexts.Grids), new Action<MyGuiControlButton>(this.OnFilterChange), MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid);
      this.m_floatingObjectsFilter = this.MakeButtonCategory(new Vector2(-0.185f, -0.205f), "Modpack", MyTexts.GetString(MySpaceTexts.FloatingObjects), new Action<MyGuiControlButton>(this.OnFilterChange), MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects);
      Vector2 vector2_1 = new Vector2(0.0f, -0.223f);
      this.Controls.Add((MyGuiControlBase) (this.m_moveLeftButton = this.MakeButtonTiny(vector2_1 + 5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyTexts.GetString(MySpaceTexts.Remove), MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, new Action<MyGuiControlButton>(this.OnRemoveRestricted))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveLeftAllButton = this.MakeButtonTiny(vector2_1 + 6f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyTexts.GetString(MySpaceTexts.RemoveAll), MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, new Action<MyGuiControlButton>(this.OnRemoveAllRestricted))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveRightAllButton = this.MakeButtonTiny(vector2_1 + 2f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0.0f, MyTexts.GetString(MySpaceTexts.AddAll), MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, new Action<MyGuiControlButton>(this.OnAddAllRestricted))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveRightButton = this.MakeButtonTiny(vector2_1 + 3f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0.0f, MyTexts.GetString(MySpaceTexts.Add), MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, new Action<MyGuiControlButton>(this.OnAddRestricted))));
      this.m_moveLeftButton.Enabled = false;
      this.m_moveRightButton.Enabled = false;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = this.m_currentPosition + new Vector2(0.022f, -0.214f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.SafeZone_Mode);
      this.m_modeLabel = myGuiControlLabel1;
      this.Controls.Add((MyGuiControlBase) this.m_modeLabel);
      this.m_accessCombobox = this.AddCombo<MySafeZoneAccess>(this.m_selectedSafeZone.AccessTypePlayers, new Action<MySafeZoneAccess>(this.OnAccessChanged), openAreaItemsCount: 4);
      this.m_accessCombobox.Position = new Vector2(0.308f, -0.224f);
      this.m_accessCombobox.Size = new Vector2((float) (0.287000000476837 - (double) this.m_modeLabel.Size.X - 0.00999999977648258), 0.1f);
      this.m_accessCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_accessCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_accessCombobox_ItemSelected);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(0.03f, -0.173f);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.SafeZone_SafeZoneFilter);
      this.m_controlLabelList = myGuiControlLabel2;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(this.m_controlLabelList.PositionX - 0.0085f, this.m_controlLabelList.Position.Y - 0.005f)), new Vector2?(new Vector2(0.2865f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel1);
      this.Controls.Add((MyGuiControlBase) this.m_controlLabelList);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(-0.3f, -0.173f);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities);
      this.m_controlLabelEntity = myGuiControlLabel3;
      MyGuiControlPanel myGuiControlPanel2 = new MyGuiControlPanel(new Vector2?(new Vector2(this.m_controlLabelEntity.PositionX - 0.0085f, this.m_controlLabelEntity.Position.Y - 0.005f)), new Vector2?(new Vector2(0.2865f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel2.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      this.Controls.Add((MyGuiControlBase) this.m_controlLabelEntity);
      this.m_restrictedListbox = new MyGuiControlListbox(new Vector2?(Vector2.Zero), MyGuiControlListboxStyleEnum.Blueprints);
      this.m_restrictedListbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_restrictedListbox.Enabled = true;
      this.m_restrictedListbox.VisibleRowsCount = 9;
      this.m_restrictedListbox.Position = this.m_restrictedListbox.Size / 2f + this.m_currentPosition;
      this.m_restrictedListbox.MultiSelect = true;
      this.Controls.Add((MyGuiControlBase) this.m_restrictedListbox);
      this.m_restrictedListbox.Position = new Vector2(0.022f, -0.145f);
      this.m_restrictedListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnSelectRestrictedItem);
      this.m_restrictedListbox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnDoubleClickRestrictedItem);
      this.m_entityListbox = new MyGuiControlListbox(new Vector2?(Vector2.Zero), MyGuiControlListboxStyleEnum.Blueprints);
      this.m_entityListbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_entityListbox.Enabled = true;
      this.m_entityListbox.VisibleRowsCount = 9;
      this.m_entityListbox.Position = this.m_restrictedListbox.Size / 2f + this.m_currentPosition;
      this.m_entityListbox.MultiSelect = true;
      this.Controls.Add((MyGuiControlBase) this.m_entityListbox);
      this.m_entityListbox.Position = new Vector2(-0.308f, -0.145f);
      this.m_entityListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnSelectEntityItem);
      this.m_entityListbox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnDoubleClickEntityItem);
      this.m_closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Close), onButtonClick: new Action<MyGuiControlButton>(this.OnCancel));
      this.m_addContainedToSafeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_FilterContained), onButtonClick: new Action<MyGuiControlButton>(this.OnAddContainedToSafe));
      Vector2 vector2_2 = new Vector2(1f / 500f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0710000023245811));
      Vector2 vector2_3 = new Vector2(0.018f, 0.0f);
      this.m_closeButton.Position = vector2_2 - vector2_3;
      this.m_addContainedToSafeButton.Position = vector2_2 + vector2_3;
      this.m_addContainedToSafeButton.SetToolTip(MySpaceTexts.ToolTipSafeZone_AddContained);
      this.m_closeButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close));
      this.Controls.Add((MyGuiControlBase) this.m_closeButton);
      this.Controls.Add((MyGuiControlBase) this.m_addContainedToSafeButton);
      this.m_playersFilter.Selected = true;
      this.m_playersFilter.Checked = true;
      this.m_controlLabelList.Text = MyTexts.GetString(MySpaceTexts.SafeZone_SafeZoneFilter) + " " + (object) this.m_playersFilter.Tooltips.ToolTips[0].Text;
      this.m_controlLabelEntity.Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities) + " " + (object) this.m_playersFilter.Tooltips.ToolTips[0].Text;
      this.UpdateRestrictedData();
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void m_accessCombobox_ItemSelected()
    {
    }

    private void OnFilterChange(MyGuiControlButton button)
    {
      this.m_selectedFilter = (MyGuiScreenAdminMenu.MyRestrictedTypeEnum) button.UserData;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
        this.m_accessCombobox.SelectItemByIndex((int) this.m_selectedSafeZone.AccessTypePlayers);
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
        this.m_accessCombobox.SelectItemByIndex((int) this.m_selectedSafeZone.AccessTypeFactions);
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
        this.m_accessCombobox.SelectItemByIndex((int) this.m_selectedSafeZone.AccessTypeGrids);
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
        this.m_accessCombobox.SelectItemByIndex((int) this.m_selectedSafeZone.AccessTypeFloatingObjects);
      this.m_playersFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_factionsFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_gridsFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_floatingObjectsFilter.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      button.Selected = true;
      button.Checked = true;
      this.m_controlLabelList.Text = MyTexts.GetString(MySpaceTexts.SafeZone_SafeZoneFilter) + " " + (object) button.Tooltips.ToolTips[0].Text;
      this.m_controlLabelEntity.Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities) + " " + (object) button.Tooltips.ToolTips[0].Text;
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void OnAddContainedToSafe(MyGuiControlButton button)
    {
      if (this.m_selectedSafeZone == null)
        return;
      this.m_selectedSafeZone.AddContainedToList();
      this.UpdateRestrictedData();
      this.RequestUpdateSafeZone();
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void OnCancel(MyGuiControlButton button) => this.CloseScreen();

    private void OnSelectRestrictedItem(MyGuiControlListbox list) => this.m_moveLeftButton.Enabled = list.SelectedItems.Count > 0;

    private void OnDoubleClickRestrictedItem(MyGuiControlListbox list) => this.OnRemoveRestricted((MyGuiControlButton) null);

    private void OnSelectEntityItem(MyGuiControlListbox list) => this.m_moveRightButton.Enabled = list.SelectedItems.Count > 0;

    private void OnDoubleClickEntityItem(MyGuiControlListbox list) => this.OnAddRestricted((MyGuiControlButton) null);

    private void OnRestrictionChanged(
      MyGuiScreenAdminMenu.MyRestrictedTypeEnum restrictionType)
    {
      this.UpdateRestrictedData();
      switch (restrictionType)
      {
        case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player:
          this.m_entityListbox.Items.Clear();
          using (IEnumerator<MyPlayer.PlayerId> enumerator = MySession.Static.Players.GetAllPlayers().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyPlayer.PlayerId current = enumerator.Current;
              MyPlayer player = (MyPlayer) null;
              if (Sync.Players.TryGetPlayerById(current, out player) && !this.m_selectedSafeZone.Players.Contains(player.Identity.IdentityId))
                this.m_entityListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(player.DisplayName), userData: ((object) player.Identity.IdentityId)));
            }
            break;
          }
        case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction:
          this.m_entityListbox.Items.Clear();
          using (Dictionary<long, IMyFaction>.Enumerator enumerator = MySession.Static.Factions.Factions.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<long, IMyFaction> current = enumerator.Current;
              if (!((IEnumerable<IMyFaction>) this.m_selectedSafeZone.Factions).Contains<IMyFaction>(current.Value))
                this.m_entityListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(current.Value.Name), userData: ((object) current.Value)));
            }
            break;
          }
        case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid:
          this.ShowFilteredEntities(MyEntityList.MyEntityTypeEnum.Grids);
          break;
        case MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects:
          this.ShowFilteredEntities(MyEntityList.MyEntityTypeEnum.FloatingObjects);
          break;
      }
    }

    private void OnAddRestricted(MyGuiControlButton button)
    {
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_entityListbox.SelectedItems)
          this.m_selectedSafeZone.Players.Add((long) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_entityListbox.SelectedItems)
          this.m_selectedSafeZone.Entities.Add((long) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_entityListbox.SelectedItems)
          this.m_selectedSafeZone.Factions.Add((MyFaction) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_entityListbox.SelectedItems)
          this.m_selectedSafeZone.Entities.Add((long) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      this.RequestUpdateSafeZone();
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void OnAddAllRestricted(MyGuiControlButton button)
    {
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_entityListbox.Items)
          this.m_selectedSafeZone.Players.Add((long) obj.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_entityListbox.Items)
          this.m_selectedSafeZone.Entities.Add((long) obj.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_entityListbox.Items)
          this.m_selectedSafeZone.Factions.Add((MyFaction) obj.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_entityListbox.Items)
          this.m_selectedSafeZone.Entities.Add((long) obj.UserData);
        this.UpdateRestrictedData();
      }
      this.RequestUpdateSafeZone();
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void OnRemoveRestricted(MyGuiControlButton button)
    {
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_restrictedListbox.SelectedItems)
          this.m_selectedSafeZone.Players.Remove((long) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_restrictedListbox.SelectedItems)
          this.m_selectedSafeZone.Entities.Remove((long) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_restrictedListbox.SelectedItems)
          this.m_selectedSafeZone.Factions.Remove((MyFaction) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
      {
        foreach (MyGuiControlListbox.Item selectedItem in this.m_restrictedListbox.SelectedItems)
          this.m_selectedSafeZone.Entities.Remove((long) selectedItem.UserData);
        this.UpdateRestrictedData();
      }
      this.RequestUpdateSafeZone();
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void OnRemoveAllRestricted(MyGuiControlButton button)
    {
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_restrictedListbox.Items)
          this.m_selectedSafeZone.Players.Remove((long) obj.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_restrictedListbox.Items)
          this.m_selectedSafeZone.Entities.Remove((long) obj.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_restrictedListbox.Items)
          this.m_selectedSafeZone.Factions.Remove((MyFaction) obj.UserData);
        this.UpdateRestrictedData();
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
      {
        foreach (MyGuiControlListbox.Item obj in this.m_restrictedListbox.Items)
          this.m_selectedSafeZone.Entities.Remove((long) obj.UserData);
        this.UpdateRestrictedData();
      }
      this.RequestUpdateSafeZone();
      this.OnRestrictionChanged(this.m_selectedFilter);
    }

    private void UpdateRestrictedData()
    {
      this.m_restrictedListbox.ClearItems();
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
      {
        foreach (long player in this.m_selectedSafeZone.Players)
        {
          MyPlayer.PlayerId result;
          if (Sync.Players.TryGetPlayerId(player, out result))
          {
            MyIdentity playerIdentity = Sync.Players.TryGetPlayerIdentity(result);
            if (playerIdentity != null)
              this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(playerIdentity.DisplayName), userData: ((object) player)));
            else
              this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(player.ToString()), userData: ((object) player)));
          }
          else
            this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(player.ToString()), userData: ((object) player)));
        }
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
      {
        foreach (long entity1 in this.m_selectedSafeZone.Entities)
        {
          MyEntity entity2;
          if (MyEntities.TryGetEntityById(entity1, out entity2))
          {
            if (entity2 is MyCubeGrid myCubeGrid && !myCubeGrid.Closed && myCubeGrid.Physics != null)
              this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity2.DisplayName ?? entity2.Name ?? entity2.ToString()), userData: ((object) entity1)));
          }
          else
            this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity1.ToString()), userData: ((object) entity1)));
        }
      }
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
      {
        foreach (long entity1 in this.m_selectedSafeZone.Entities)
        {
          MyEntity entity2;
          if (MyEntities.TryGetEntityById(entity1, out entity2))
          {
            if (entity2 is MyFloatingObject myFloatingObject)
            {
              if (!myFloatingObject.Closed && myFloatingObject.Physics != null)
                this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity2.DisplayName ?? entity2.Name ?? entity2.ToString()), userData: ((object) entity1)));
              else
                continue;
            }
            if (entity2 is MyInventoryBagEntity inventoryBagEntity && !inventoryBagEntity.Closed && inventoryBagEntity.Physics != null)
              this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity2.DisplayName ?? entity2.Name ?? entity2.ToString()), userData: ((object) entity1)));
          }
          else
            this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(entity1.ToString()), userData: ((object) entity1)));
        }
      }
      else
      {
        if (this.m_selectedFilter != MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
          return;
        foreach (MyFaction faction in this.m_selectedSafeZone.Factions)
          this.m_restrictedListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(faction.Name), userData: ((object) faction)));
      }
    }

    private void RequestUpdateSafeZone()
    {
      if (this.m_selectedSafeZone == null)
        return;
      MyObjectBuilder_SafeZone objectBuilder = (MyObjectBuilder_SafeZone) this.m_selectedSafeZone.GetObjectBuilder(false);
      if (this.m_safeZoneBlockId.HasValue)
        MySessionComponentSafeZones.RequestUpdateSafeZonePlayer(this.m_safeZoneBlockId.Value, objectBuilder);
      else
        MySessionComponentSafeZones.RequestUpdateSafeZone(objectBuilder);
    }

    private void ShowFilteredEntities(MyEntityList.MyEntityTypeEnum restrictionType) => MyGuiScreenAdminMenu.RequestEntityList(restrictionType);

    private void OnAccessChanged(MySafeZoneAccess access)
    {
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Player)
        this.m_selectedSafeZone.AccessTypePlayers = access;
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Faction)
        this.m_selectedSafeZone.AccessTypeFactions = access;
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.Grid)
        this.m_selectedSafeZone.AccessTypeGrids = access;
      else if (this.m_selectedFilter == MyGuiScreenAdminMenu.MyRestrictedTypeEnum.FloatingObjects)
        this.m_selectedSafeZone.AccessTypeFloatingObjects = access;
      this.RequestUpdateSafeZone();
    }

    private MyGuiControlButton MakeButtonTiny(
      Vector2 position,
      float rotation,
      string toolTip,
      MyGuiHighlightTexture icon,
      Action<MyGuiControlButton> onClick,
      Vector2? size = null)
    {
      Vector2? position1 = new Vector2?(position);
      string str = toolTip;
      Action<MyGuiControlButton> action = onClick;
      Vector2? size1 = size;
      Vector4? colorMask = new Vector4?();
      string toolTip1 = str;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position1, MyGuiControlButtonStyleEnum.Square, size1, colorMask, toolTip: toolTip1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      icon.SizePx = new Vector2(64f, 64f);
      guiControlButton.Icon = new MyGuiHighlightTexture?(icon);
      guiControlButton.IconRotation = rotation;
      guiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      return guiControlButton;
    }

    private MyGuiControlButton MakeButtonCategoryTiny(
      Vector2 position,
      float rotation,
      string toolTip,
      MyGuiHighlightTexture icon,
      Action<MyGuiControlButton> onClick,
      Vector2? size = null)
    {
      Vector2? position1 = new Vector2?(position);
      string str = toolTip;
      Action<MyGuiControlButton> action = onClick;
      Vector2? size1 = size;
      Vector4? colorMask = new Vector4?();
      string toolTip1 = str;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position1, MyGuiControlButtonStyleEnum.Square48, size1, colorMask, toolTip: toolTip1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      icon.SizePx = new Vector2(48f, 48f);
      guiControlButton.Icon = new MyGuiHighlightTexture?(icon);
      guiControlButton.IconRotation = rotation;
      guiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      return guiControlButton;
    }

    private MyGuiControlButton MakeButtonCategory(
      Vector2 position,
      string textureName,
      string tooltip,
      Action<MyGuiControlButton> myAction,
      MyGuiScreenAdminMenu.MyRestrictedTypeEnum myEnum)
    {
      MyGuiControlButton guiControlButton = this.MakeButtonCategoryTiny(position, 0.0f, tooltip, new MyGuiHighlightTexture()
      {
        Normal = string.Format("Textures\\GUI\\Icons\\buttons\\small_variant\\{0}.dds", (object) textureName),
        Highlight = string.Format("Textures\\GUI\\Icons\\buttons\\small_variant\\{0}.dds", (object) textureName),
        Focus = string.Format("Textures\\GUI\\Icons\\buttons\\small_variant\\{0}_focus.dds", (object) textureName),
        SizePx = new Vector2(48f, 48f)
      }, myAction);
      guiControlButton.UserData = (object) myEnum;
      this.Controls.Add((MyGuiControlBase) guiControlButton);
      guiControlButton.Size = new Vector2(0.005f, 0.005f);
      return guiControlButton;
    }

    public override string GetFriendlyName() => "MyGuiRenameDialog";
  }
}
