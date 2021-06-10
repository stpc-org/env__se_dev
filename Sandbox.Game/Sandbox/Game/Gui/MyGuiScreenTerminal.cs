// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenTerminal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Replication;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenTerminal : MyGuiScreenBase
  {
    private static MyGuiScreenTerminal m_instance;
    private static MyEntity m_interactedEntity;
    private static MyEntity m_openInventoryInteractedEntity;
    private static Action<MyEntity> m_closeHandler;
    private static bool m_screenOpen;
    public static bool IsRemote;
    private MyGuiControlTabControl m_terminalTabs;
    private MyGuiControlParent m_propertiesTopMenuParent;
    private MyGuiControlParent m_propertiesTableParent;
    private MyTerminalControlPanel m_controllerControlPanel;
    private MyTerminalInventoryController m_controllerInventory;
    private MyTerminalProductionController m_controllerProduction;
    private MyTerminalInfoController m_controllerInfo;
    private MyTerminalFactionController m_controllerFactions;
    private MyTerminalPropertiesController m_controllerProperties;
    private MyTerminalChatController m_controllerChat;
    private MyTerminalGpsController m_controllerGps;
    private MyGridColorHelper m_colorHelper;
    private MyGuiControlLabel m_terminalNotConnected;
    private bool m_autoSelectControlSearch = true;
    private MyCharacter m_user;
    private MyTerminalPageEnum m_initialPage;
    private Dictionary<long, Action<long>> m_requestedEntities = new Dictionary<long, Action<long>>();
    private Dictionary<MyTerminalPageEnum, MyGuiControlBase> m_defaultFocusedControlKeyboard = new Dictionary<MyTerminalPageEnum, MyGuiControlBase>();
    private Dictionary<MyTerminalPageEnum, MyGuiControlBase> m_defaultFocusedControlGamepad = new Dictionary<MyTerminalPageEnum, MyGuiControlBase>();
    private Dictionary<MyTerminalPageEnum, MyTerminalController> m_terminalControllers = new Dictionary<MyTerminalPageEnum, MyTerminalController>();
    private bool m_connected = true;
    private MyGuiControlButton m_convertToShipBtn;
    private MyGuiControlButton m_convertToStationBtn;
    private MyGuiControlRadioButton m_assemblingButton;
    private MyGuiControlRadioButton m_disassemblingButton;
    private MyGuiControlButton m_selectShipButton;
    private MyGuiControlCombobox m_assemblersCombobox;
    private MyGuiControlLabel m_blockNameLabel;
    private MyGuiControlLabel m_groupTitleLabel;
    private MyGuiControlTextbox m_groupName;
    private MyGuiControlButton m_groupSave;
    private MyGuiControlButton m_groupDelete;

    internal static bool IsOpen => MyGuiScreenTerminal.m_screenOpen;

    public static MyEntity InteractedEntity
    {
      get => MyGuiScreenTerminal.m_interactedEntity;
      set
      {
        if (MyGuiScreenTerminal.m_interactedEntity != null)
          MyGuiScreenTerminal.m_interactedEntity.OnClose -= MyGuiScreenTerminal.m_closeHandler;
        if (MyGuiScreenTerminal.m_instance?.m_controllerControlPanel != null)
          MyGuiScreenTerminal.m_instance.m_controllerControlPanel.ClearBlockList();
        MyGuiScreenTerminal.m_interactedEntity = value;
        if (MyGuiScreenTerminal.m_interactedEntity != null)
        {
          MyGuiScreenTerminal.m_interactedEntity.OnClose += MyGuiScreenTerminal.m_closeHandler;
          if (MyGuiScreenTerminal.m_interactedEntity != MyGuiScreenTerminal.m_openInventoryInteractedEntity && MyGuiScreenTerminal.m_instance != null)
            MyGuiScreenTerminal.m_instance.m_initialPage = MyTerminalPageEnum.ControlPanel;
        }
        if (!MyGuiScreenTerminal.m_screenOpen || MyGuiScreenTerminal.m_instance == null)
          return;
        MyGuiScreenTerminal.m_instance.RecreateTabs();
      }
    }

    private MyGuiScreenTerminal()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(1.0157f, 0.9172f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      MyGuiScreenTerminal.m_closeHandler = new Action<MyEntity>(this.OnInteractedClose);
      this.m_colorHelper = new MyGridColorHelper();
    }

    private void OnInteractedClose(MyEntity entity)
    {
      MyGuiScreenTerminal.InteractedEntity = (MyEntity) null;
      MyGuiScreenTerminal.Hide();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenTerminal);

    public override bool RegisterClicks() => true;

    public override bool CloseScreen(bool isUnloading = false)
    {
      if (!base.CloseScreen(isUnloading))
        return false;
      MyGuiScreenTerminal.InteractedEntity = (MyEntity) null;
      return true;
    }

    private void CreateFixedTerminalElements()
    {
      this.m_terminalNotConnected = MyGuiScreenTerminal.CreateErrorLabel(MySpaceTexts.ScreenTerminalError_ShipHasBeenDisconnected, "DisconnectedMessage");
      this.m_terminalNotConnected.Visible = false;
      this.Controls.Add((MyGuiControlBase) this.m_terminalNotConnected);
      this.AddCaption(MySpaceTexts.Terminal, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2(this.m_size.Value.X * 0.447f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.894f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2(this.m_size.Value.X * 0.447f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.143500000238419)), this.m_size.Value.X * 0.894f);
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2(this.m_size.Value.X * 0.447f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0480000004172325));
      controlSeparatorList.AddHorizontal(start, this.m_size.Value.X * 0.894f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
      {
        MyGuiControlParent guiControlParent1 = new MyGuiControlParent();
        guiControlParent1.Position = new Vector2(-0.855f, -0.514f);
        guiControlParent1.Size = new Vector2(0.8f, 0.15f);
        guiControlParent1.Name = "PropertiesPanel";
        guiControlParent1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_propertiesTopMenuParent = guiControlParent1;
        MyGuiControlParent guiControlParent2 = new MyGuiControlParent();
        guiControlParent2.Position = new Vector2(-0.02f, -0.37f);
        guiControlParent2.Size = new Vector2(0.93f, 0.78f);
        guiControlParent2.Name = "PropertiesTable";
        guiControlParent2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
        this.m_propertiesTableParent = guiControlParent2;
        this.CreatePropertiesPageControls(this.m_propertiesTopMenuParent, this.m_propertiesTableParent);
        if (this.m_controllerProperties == null)
          this.m_controllerProperties = new MyTerminalPropertiesController();
        else
          this.m_controllerProperties.Close();
        this.m_controllerProperties.ButtonClicked += new Action(this.PropertiesButtonClicked);
        this.Controls.Add((MyGuiControlBase) this.m_propertiesTableParent);
        this.Controls.Add((MyGuiControlBase) this.m_propertiesTopMenuParent);
      }
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, start.Y + minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
    }

    private void CreateTabs()
    {
      MyGuiControlTabControl controlTabControl = new MyGuiControlTabControl();
      controlTabControl.Position = new Vector2(-1f / 1000f, -0.367f);
      controlTabControl.Size = new Vector2(0.907f, 0.78f);
      controlTabControl.Name = "TerminalTabs";
      controlTabControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      controlTabControl.CanAutoFocusOnInputHandling = false;
      this.m_terminalTabs = controlTabControl;
      if (MyFakes.ENABLE_COMMUNICATION)
        this.m_terminalTabs.TabButtonScale = 0.875f;
      MyGuiControlTabPage tabSubControl1 = this.m_terminalTabs.GetTabSubControl(0);
      MyGuiControlTabPage tabSubControl2 = this.m_terminalTabs.GetTabSubControl(1);
      MyGuiControlTabPage tabSubControl3 = this.m_terminalTabs.GetTabSubControl(2);
      MyGuiControlTabPage tabSubControl4 = this.m_terminalTabs.GetTabSubControl(3);
      MyGuiControlTabPage tabSubControl5 = this.m_terminalTabs.GetTabSubControl(4);
      MyGuiControlTabPage chatPage = (MyGuiControlTabPage) null;
      if (MyFakes.ENABLE_COMMUNICATION)
        chatPage = this.m_terminalTabs.GetTabSubControl(5);
      MyGuiControlTabPage gpsPage = (MyGuiControlTabPage) null;
      if (MyFakes.ENABLE_GPS)
      {
        gpsPage = this.m_terminalTabs.GetTabSubControl(6);
        this.m_terminalTabs.TabButtonScale = 0.75f;
      }
      this.CreateInventoryPageControls(tabSubControl1);
      this.CreateControlPanelPageControls(tabSubControl2);
      this.CreateProductionPageControls(tabSubControl3);
      this.CreateInfoPageControls(tabSubControl4);
      this.CreateFactionsPageControls(tabSubControl5);
      if (MyFakes.ENABLE_GPS)
        this.CreateGpsPageControls(gpsPage);
      if (MyFakes.ENABLE_COMMUNICATION)
        this.CreateChatPageControls(chatPage);
      MyCubeGrid myCubeGrid = MyGuiScreenTerminal.InteractedEntity != null ? MyGuiScreenTerminal.InteractedEntity.Parent as MyCubeGrid : (MyCubeGrid) null;
      this.m_colorHelper.Init(myCubeGrid);
      if (this.m_controllerInventory == null)
      {
        this.m_controllerInventory = new MyTerminalInventoryController();
        this.m_terminalControllers.Add(MyTerminalPageEnum.Inventory, (MyTerminalController) this.m_controllerInventory);
      }
      else
        this.m_controllerInventory.Close();
      if (this.m_controllerControlPanel == null)
      {
        this.m_controllerControlPanel = new MyTerminalControlPanel();
        this.m_terminalControllers.Add(MyTerminalPageEnum.ControlPanel, (MyTerminalController) this.m_controllerControlPanel);
        this.m_controllerControlPanel.SetTerminalScreen(this);
      }
      else
        this.m_controllerControlPanel.Close();
      if (this.m_controllerProduction == null)
      {
        this.m_controllerProduction = new MyTerminalProductionController();
        this.m_terminalControllers.Add(MyTerminalPageEnum.Production, (MyTerminalController) this.m_controllerProduction);
      }
      else
        this.m_controllerProduction.Close();
      if (this.m_controllerInfo == null)
      {
        this.m_controllerInfo = new MyTerminalInfoController();
        this.m_terminalControllers.Add(MyTerminalPageEnum.Info, (MyTerminalController) this.m_controllerInfo);
      }
      else
        this.m_controllerInfo.Close();
      if (this.m_controllerFactions == null)
      {
        this.m_controllerFactions = new MyTerminalFactionController();
        this.m_terminalControllers.Add(MyTerminalPageEnum.Factions, (MyTerminalController) this.m_controllerFactions);
      }
      else
        this.m_controllerFactions.Close();
      if (MyFakes.ENABLE_GPS)
      {
        if (this.m_controllerGps == null)
        {
          this.m_controllerGps = new MyTerminalGpsController();
          this.m_terminalControllers.Add(MyTerminalPageEnum.Gps, (MyTerminalController) this.m_controllerGps);
        }
        else
          this.m_controllerGps.Close();
      }
      if (MyFakes.ENABLE_COMMUNICATION)
      {
        if (this.m_controllerChat == null)
        {
          this.m_controllerChat = new MyTerminalChatController();
          this.m_terminalControllers.Add(MyTerminalPageEnum.Comms, (MyTerminalController) this.m_controllerChat);
        }
        else
          this.m_controllerChat.Close();
      }
      this.m_controllerInventory.Init((IMyGuiControlsParent) tabSubControl1, (MyEntity) this.m_user, MyGuiScreenTerminal.InteractedEntity, this.m_colorHelper, (MyGuiScreenBase) this);
      this.m_controllerControlPanel.Init((IMyGuiControlsParent) tabSubControl2, MySession.Static.LocalHumanPlayer, myCubeGrid, MyGuiScreenTerminal.InteractedEntity as MyTerminalBlock, this.m_colorHelper);
      this.m_controllerProduction.Init((IMyGuiControlsParent) tabSubControl3, myCubeGrid, MyGuiScreenTerminal.InteractedEntity as MyCubeBlock);
      this.m_controllerInfo.Init(tabSubControl4, MyGuiScreenTerminal.InteractedEntity != null ? MyGuiScreenTerminal.InteractedEntity.Parent as MyCubeGrid : (MyCubeGrid) null);
      this.m_controllerFactions.Init((IMyGuiControlsParent) tabSubControl5);
      if (MyFakes.ENABLE_GPS)
        this.m_controllerGps.Init((IMyGuiControlsParent) gpsPage);
      if (MyFakes.ENABLE_COMMUNICATION)
        this.m_controllerChat.Init((IMyGuiControlsParent) chatPage);
      this.m_terminalTabs.SelectedPage = (int) this.m_initialPage;
      if (this.m_terminalTabs.SelectedPage != -1 && !this.m_terminalTabs.GetTabSubControl(this.m_terminalTabs.SelectedPage).Enabled)
        this.m_terminalTabs.SelectedPage = this.m_terminalTabs.Controls.IndexOf((MyGuiControlBase) tabSubControl2);
      this.CloseButtonEnabled = true;
      this.SetDefaultCloseButtonOffset();
      this.Controls.Add((MyGuiControlBase) this.m_terminalTabs);
      if (!MyFakes.ENABLE_TERMINAL_PROPERTIES)
        return;
      this.m_terminalTabs.OnPageChanged += new Action(this.tabs_OnPageChanged);
    }

    public void GetGroupInjectableControls(
      ref MyGuiControlLabel blockName,
      ref MyGuiControlTextbox groupName,
      ref MyGuiControlButton groupSave,
      ref MyGuiControlButton groupDelete)
    {
      blockName = this.m_blockNameLabel;
      groupName = this.m_groupName;
      groupSave = this.m_groupSave;
      groupDelete = this.m_groupDelete;
    }

    private void CreateProperties()
    {
      if (this.m_controllerProperties == null)
        this.m_controllerProperties = new MyTerminalPropertiesController();
      else
        this.m_controllerProperties.Close();
      this.m_controllerProperties.Init(this.m_propertiesTopMenuParent, this.m_propertiesTableParent, MyGuiScreenTerminal.InteractedEntity, MyGuiScreenTerminal.m_openInventoryInteractedEntity, MyGuiScreenTerminal.IsRemote);
      if (this.m_propertiesTableParent == null)
        return;
      this.m_propertiesTableParent.Visible = this.m_initialPage == MyTerminalPageEnum.Properties;
    }

    private void RecreateTabs()
    {
      this.Controls.RemoveControlByName("TerminalTabs");
      this.CreateTabs();
      if (!MyFakes.ENABLE_TERMINAL_PROPERTIES)
        return;
      this.CreateProperties();
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.CreateFixedTerminalElements();
      this.CreateTabs();
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
        this.CreateProperties();
      this.tabs_OnPageChanged();
    }

    private void CreateInventoryPageControls(MyGuiControlTabPage page)
    {
      page.Name = "PageInventory";
      page.TextEnum = MySpaceTexts.Inventory;
      page.TextScale = 0.7005405f;
      MyGuiControlList inventoryPageLeftSection = MyGuiScreenTerminal.CreateInventoryPageLeftSection(page);
      MyGuiScreenTerminal.CreateInventoryPageRightSection(page);
      MyGuiScreenTerminal.CreateInventoryPageCenterSection(page);
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Inventory] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Inventory] = (MyGuiControlBase) inventoryPageLeftSection;
    }

    private static void CreateInventoryPageCenterSection(MyGuiControlTabPage page)
    {
      float y = -0.225f;
      float num1 = 0.1f;
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
      guiControlButton1.Position = new Vector2(0.0f, y);
      guiControlButton1.Size = new Vector2(0.044375f, 0.1366667f);
      guiControlButton1.Name = "ThrowOutButton";
      guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton1.TextEnum = MySpaceTexts.Afterburner;
      guiControlButton1.TextScale = 0.0f;
      guiControlButton1.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton1.DrawCrossTextureWhenDisabled = true;
      guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.InventoryTrash;
      guiControlButton1.ActivateOnMouseRelease = false;
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      MyGuiControlButton guiControlButton3 = new MyGuiControlButton();
      float num2;
      guiControlButton3.Position = new Vector2(0.0f, num2 = y + num1);
      guiControlButton3.Size = new Vector2(0.044375f, 0.1366667f);
      guiControlButton3.Name = "WithdrawButton";
      guiControlButton3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton3.TextEnum = MySpaceTexts.Afterburner;
      guiControlButton3.TextScale = 0.0f;
      guiControlButton3.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton3.DrawCrossTextureWhenDisabled = true;
      guiControlButton3.VisualStyle = MyGuiControlButtonStyleEnum.Withdraw;
      guiControlButton3.ActivateOnMouseRelease = false;
      MyGuiControlButton guiControlButton4 = guiControlButton3;
      MyGuiControlButton guiControlButton5 = new MyGuiControlButton();
      float num3;
      guiControlButton5.Position = new Vector2(0.0f, num3 = num2 + num1);
      guiControlButton5.Size = new Vector2(0.044375f, 0.1366667f);
      guiControlButton5.Name = "DepositAllButton";
      guiControlButton5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton5.TextEnum = MySpaceTexts.Afterburner;
      guiControlButton5.TextScale = 0.0f;
      guiControlButton5.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton5.DrawCrossTextureWhenDisabled = true;
      guiControlButton5.VisualStyle = MyGuiControlButtonStyleEnum.DepositAll;
      guiControlButton5.ActivateOnMouseRelease = false;
      MyGuiControlButton guiControlButton6 = guiControlButton5;
      MyGuiControlButton guiControlButton7 = new MyGuiControlButton();
      float num4;
      guiControlButton7.Position = new Vector2(0.0f, num4 = num3 + num1);
      guiControlButton7.Size = new Vector2(0.044375f, 0.1366667f);
      guiControlButton7.Name = "AddToProductionButton";
      guiControlButton7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton7.TextEnum = MySpaceTexts.Afterburner;
      guiControlButton7.TextScale = 0.0f;
      guiControlButton7.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton7.DrawCrossTextureWhenDisabled = true;
      guiControlButton7.VisualStyle = MyGuiControlButtonStyleEnum.AddToProduction;
      guiControlButton7.ActivateOnMouseRelease = false;
      MyGuiControlButton guiControlButton8 = guiControlButton7;
      MyGuiControlButton guiControlButton9 = new MyGuiControlButton();
      float num5;
      guiControlButton9.Position = new Vector2(0.0f, num5 = num4 + num1);
      guiControlButton9.Size = new Vector2(0.044375f, 0.1366667f);
      guiControlButton9.Name = "SelectedToProductionButton";
      guiControlButton9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton9.TextEnum = MySpaceTexts.Afterburner;
      guiControlButton9.TextScale = 0.0f;
      guiControlButton9.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton9.DrawCrossTextureWhenDisabled = true;
      guiControlButton9.VisualStyle = MyGuiControlButtonStyleEnum.SelectedToProduction;
      guiControlButton9.ActivateOnMouseRelease = false;
      MyGuiControlButton guiControlButton10 = guiControlButton9;
      page.Controls.Add((MyGuiControlBase) guiControlButton2);
      page.Controls.Add((MyGuiControlBase) guiControlButton4);
      page.Controls.Add((MyGuiControlBase) guiControlButton6);
      page.Controls.Add((MyGuiControlBase) guiControlButton8);
      page.Controls.Add((MyGuiControlBase) guiControlButton10);
    }

    private static void CreateInventoryPageRightSection(MyGuiControlTabPage page)
    {
      float num = 0.004f;
      Vector2 vector2 = new Vector2(0.045f, 0.05666667f);
      float y = -0.338f;
      MyGuiControlRadioButton controlRadioButton1 = new MyGuiControlRadioButton();
      controlRadioButton1.Position = new Vector2(0.0145f + num, y);
      controlRadioButton1.Size = new Vector2(0.056875f, 0.0575f);
      controlRadioButton1.Name = "RightSuitButton";
      controlRadioButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton1.Key = 0;
      controlRadioButton1.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterCharacter;
      controlRadioButton1.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton2 = controlRadioButton1;
      MyGuiControlRadioButton controlRadioButton3 = new MyGuiControlRadioButton();
      controlRadioButton3.Position = new Vector2(0.061f + num, y);
      controlRadioButton3.Size = new Vector2(0.056875f, 0.0575f);
      controlRadioButton3.Name = "RightGridButton";
      controlRadioButton3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton3.Key = 0;
      controlRadioButton3.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterGrid;
      controlRadioButton3.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton4 = controlRadioButton3;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2((float) ((double) controlRadioButton4.PositionX + (double) controlRadioButton4.Size.X + 3.0 / 500.0), y);
      myGuiControlLabel1.Size = new Vector2(0.05f, vector2.Y * 2f);
      myGuiControlLabel1.Name = "RightFilterGamepadHelp";
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.CanHaveFocus = false;
      myGuiControlLabel1.Visible = MyInput.Static.IsJoystickLastUsed;
      myGuiControlLabel1.TextScale = 0.64f;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlRadioButton controlRadioButton5 = new MyGuiControlRadioButton();
      controlRadioButton5.Position = new Vector2(0.3675f + num, y);
      controlRadioButton5.Size = vector2;
      controlRadioButton5.Name = "RightFilterShipButton";
      controlRadioButton5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton5.Key = 0;
      controlRadioButton5.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterShip;
      controlRadioButton5.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton6 = controlRadioButton5;
      MyGuiControlRadioButton controlRadioButton7 = new MyGuiControlRadioButton();
      controlRadioButton7.Position = new Vector2(0.414f + num, y);
      controlRadioButton7.Size = vector2;
      controlRadioButton7.Name = "RightFilterStorageButton";
      controlRadioButton7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton7.Key = 0;
      controlRadioButton7.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterStorage;
      controlRadioButton7.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton8 = controlRadioButton7;
      MyGuiControlRadioButton controlRadioButton9 = new MyGuiControlRadioButton();
      controlRadioButton9.Position = new Vector2(0.4605f + num, y);
      controlRadioButton9.Size = vector2;
      controlRadioButton9.Name = "RightFilterSystemButton";
      controlRadioButton9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton9.Key = 0;
      controlRadioButton9.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterSystem;
      controlRadioButton9.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton10 = controlRadioButton9;
      MyGuiControlRadioButton controlRadioButton11 = new MyGuiControlRadioButton();
      controlRadioButton11.Position = new Vector2(0.321f + num, y);
      controlRadioButton11.Size = vector2;
      controlRadioButton11.Name = "RightFilterEnergyButton";
      controlRadioButton11.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton11.Key = 0;
      controlRadioButton11.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterEnergy;
      controlRadioButton11.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton12 = controlRadioButton11;
      MyGuiControlRadioButton controlRadioButton13 = new MyGuiControlRadioButton();
      controlRadioButton13.Position = new Vector2(0.275f + num, y);
      controlRadioButton13.Size = vector2;
      controlRadioButton13.Name = "RightFilterAllButton";
      controlRadioButton13.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton13.Key = 0;
      controlRadioButton13.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterAll;
      controlRadioButton13.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton14 = controlRadioButton13;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox();
      guiControlCheckbox1.Position = new Vector2(0.463f + num, -0.255f);
      guiControlCheckbox1.Name = "CheckboxHideEmptyRight";
      guiControlCheckbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlCheckbox guiControlCheckbox2 = guiControlCheckbox1;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(0.415f + num, -0.255f);
      myGuiControlLabel3.Name = "LabelHideEmptyRight";
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      myGuiControlLabel3.TextEnum = MySpaceTexts.HideEmpty;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlSearchBox controlSearchBox1 = new MyGuiControlSearchBox(new Vector2?(new Vector2(0.0185f + num, -0.26f)), new Vector2?(new Vector2(0.361f - myGuiControlLabel4.Size.X, 0.052f)), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      controlSearchBox1.Name = "BlockSearchRight";
      MyGuiControlSearchBox controlSearchBox2 = controlSearchBox1;
      MyGuiControlList myGuiControlList1 = new MyGuiControlList();
      myGuiControlList1.Position = new Vector2(0.465f, -0.295f);
      myGuiControlList1.Size = new Vector2(0.44f, 0.65f);
      myGuiControlList1.Name = "RightInventory";
      myGuiControlList1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      MyGuiControlList myGuiControlList2 = myGuiControlList1;
      page.Controls.Add((MyGuiControlBase) controlRadioButton2);
      page.Controls.Add((MyGuiControlBase) controlRadioButton4);
      page.Controls.Add((MyGuiControlBase) controlRadioButton6);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      page.Controls.Add((MyGuiControlBase) controlRadioButton8);
      page.Controls.Add((MyGuiControlBase) controlRadioButton10);
      page.Controls.Add((MyGuiControlBase) controlRadioButton12);
      page.Controls.Add((MyGuiControlBase) controlRadioButton14);
      page.Controls.Add((MyGuiControlBase) controlSearchBox2);
      page.Controls.Add((MyGuiControlBase) guiControlCheckbox2);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      page.Controls.Add((MyGuiControlBase) myGuiControlList2);
    }

    private static MyGuiControlList CreateInventoryPageLeftSection(
      MyGuiControlTabPage page)
    {
      float num1 = -0.008f;
      float num2 = 0.1254f;
      Vector2 vector2 = new Vector2(0.045f, 0.05666667f);
      float y = -0.338f;
      MyGuiControlRadioButton controlRadioButton1 = new MyGuiControlRadioButton();
      controlRadioButton1.Position = new Vector2(num1 - 0.4565f, y);
      controlRadioButton1.Size = new Vector2(0.056875f, 0.0575f);
      controlRadioButton1.Name = "LeftSuitButton";
      controlRadioButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton1.Key = 0;
      controlRadioButton1.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterCharacter;
      controlRadioButton1.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton2 = controlRadioButton1;
      MyGuiControlRadioButton controlRadioButton3 = new MyGuiControlRadioButton();
      controlRadioButton3.Position = new Vector2(num1 - 0.41f, y);
      controlRadioButton3.Size = new Vector2(0.056875f, 0.0575f);
      controlRadioButton3.Name = "LeftGridButton";
      controlRadioButton3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton3.Key = 0;
      controlRadioButton3.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterGrid;
      controlRadioButton3.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton4 = controlRadioButton3;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2((float) ((double) controlRadioButton4.PositionX + (double) controlRadioButton4.Size.X + 3.0 / 500.0), y);
      myGuiControlLabel1.Size = new Vector2(0.1f, vector2.Y * 2f);
      myGuiControlLabel1.Name = "LeftFilterGamepadHelp";
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.CanHaveFocus = false;
      myGuiControlLabel1.Visible = MyInput.Static.IsJoystickLastUsed;
      myGuiControlLabel1.TextScale = 0.64f;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlRadioButton controlRadioButton5 = new MyGuiControlRadioButton();
      controlRadioButton5.Position = new Vector2(num1 - 0.2285f + num2, y);
      controlRadioButton5.Size = vector2;
      controlRadioButton5.Name = "LeftFilterShipButton";
      controlRadioButton5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton5.Key = 0;
      controlRadioButton5.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterShip;
      controlRadioButton5.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton6 = controlRadioButton5;
      MyGuiControlRadioButton controlRadioButton7 = new MyGuiControlRadioButton();
      controlRadioButton7.Position = new Vector2(num1 - 0.182f + num2, y);
      controlRadioButton7.Size = vector2;
      controlRadioButton7.Name = "LeftFilterStorageButton";
      controlRadioButton7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton7.Key = 0;
      controlRadioButton7.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterStorage;
      controlRadioButton7.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton8 = controlRadioButton7;
      MyGuiControlRadioButton controlRadioButton9 = new MyGuiControlRadioButton();
      controlRadioButton9.Position = new Vector2(num1 - 0.1355f + num2, y);
      controlRadioButton9.Size = vector2;
      controlRadioButton9.Name = "LeftFilterSystemButton";
      controlRadioButton9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton9.Key = 0;
      controlRadioButton9.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterSystem;
      controlRadioButton9.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton10 = controlRadioButton9;
      MyGuiControlRadioButton controlRadioButton11 = new MyGuiControlRadioButton();
      controlRadioButton11.Position = new Vector2(num1 - 0.275f + num2, y);
      controlRadioButton11.Size = vector2;
      controlRadioButton11.Name = "LeftFilterEnergyButton";
      controlRadioButton11.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton11.Key = 0;
      controlRadioButton11.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterEnergy;
      controlRadioButton11.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton12 = controlRadioButton11;
      MyGuiControlRadioButton controlRadioButton13 = new MyGuiControlRadioButton();
      controlRadioButton13.Position = new Vector2(num1 - 0.3215f + num2, y);
      controlRadioButton13.Size = vector2;
      controlRadioButton13.Name = "LeftFilterAllButton";
      controlRadioButton13.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlRadioButton13.Key = 0;
      controlRadioButton13.VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterAll;
      controlRadioButton13.CanHaveFocus = false;
      MyGuiControlRadioButton controlRadioButton14 = controlRadioButton13;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox();
      guiControlCheckbox1.Position = new Vector2(num1 - 0.0075f, -0.255f);
      guiControlCheckbox1.Name = "CheckboxHideEmptyLeft";
      guiControlCheckbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlCheckbox guiControlCheckbox2 = guiControlCheckbox1;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(num1 - 0.055f, -0.255f);
      myGuiControlLabel3.Name = "LabelHideEmptyLeft";
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      myGuiControlLabel3.TextEnum = MySpaceTexts.HideEmpty;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlSearchBox controlSearchBox1 = new MyGuiControlSearchBox(new Vector2?(new Vector2(num1 - 0.452f, -0.26f)), new Vector2?(new Vector2(0.361f - myGuiControlLabel4.Size.X, 0.052f)), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      controlSearchBox1.Name = "BlockSearchLeft";
      MyGuiControlSearchBox controlSearchBox2 = controlSearchBox1;
      MyGuiControlList myGuiControlList1 = new MyGuiControlList();
      myGuiControlList1.Position = new Vector2(-0.465f, -0.26f);
      myGuiControlList1.Size = new Vector2(0.44f, 0.616f);
      myGuiControlList1.Name = "LeftInventory";
      myGuiControlList1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlList myGuiControlList2 = myGuiControlList1;
      page.Controls.Add((MyGuiControlBase) controlRadioButton2);
      page.Controls.Add((MyGuiControlBase) controlRadioButton4);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      page.Controls.Add((MyGuiControlBase) controlRadioButton6);
      page.Controls.Add((MyGuiControlBase) controlRadioButton8);
      page.Controls.Add((MyGuiControlBase) controlRadioButton10);
      page.Controls.Add((MyGuiControlBase) controlRadioButton12);
      page.Controls.Add((MyGuiControlBase) controlRadioButton14);
      page.Controls.Add((MyGuiControlBase) controlSearchBox2);
      page.Controls.Add((MyGuiControlBase) guiControlCheckbox2);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      page.Controls.Add((MyGuiControlBase) myGuiControlList2);
      return myGuiControlList2;
    }

    private void CreateControlPanelPageControls(MyGuiControlTabPage page)
    {
      page.Name = "PageControlPanel";
      page.TextEnum = MySpaceTexts.ControlPanel;
      page.TextScale = 0.7005405f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(0.145f, -0.333f), 0.676f, 1f / 500f);
      controlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 1f / 500f);
      page.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlSearchBox controlSearchBox1 = new MyGuiControlSearchBox(new Vector2?(new Vector2(-0.452f, -0.342f)), new Vector2?(new Vector2(0.255f, 0.052f)));
      controlSearchBox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlSearchBox1.Name = "FunctionalBlockSearch";
      MyGuiControlSearchBox controlSearchBox2 = controlSearchBox1;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(-0.442f, -0.271f);
      myGuiControlLabel1.Name = "ControlLabel";
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ControlScreen_GridBlocksLabel);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.452f, -0.276f)), new Vector2?(new Vector2(0.29f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      page.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlListbox guiControlListbox1 = new MyGuiControlListbox();
      guiControlListbox1.Position = new Vector2(-0.452f, -0.2426f);
      guiControlListbox1.Size = new Vector2(0.29f, 0.5f);
      guiControlListbox1.Name = "FunctionalBlockListbox";
      guiControlListbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlListbox1.VisualStyle = MyGuiControlListboxStyleEnum.ChatScreen;
      guiControlListbox1.VisibleRowsCount = 17;
      guiControlListbox1.SelectItemOnItemFocusChangeByKeyboard = true;
      MyGuiControlListbox guiControlListbox2 = guiControlListbox1;
      MyGuiControlCompositePanel controlCompositePanel = new MyGuiControlCompositePanel();
      controlCompositePanel.Position = new Vector2(-0.1525f, 0.0f);
      controlCompositePanel.Size = new Vector2(0.615f, 0.7125f);
      controlCompositePanel.Name = "Control";
      controlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlCompositePanel.InnerHeight = 0.685f;
      controlCompositePanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      MyGuiControlPanel myGuiControlPanel3 = new MyGuiControlPanel();
      myGuiControlPanel3.Position = new Vector2(-0.1425f, -0.32f);
      myGuiControlPanel3.Size = new Vector2(0.595f, 0.035f);
      myGuiControlPanel3.Name = "SelectedBlockNamePanel";
      myGuiControlPanel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlPanel3.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      Vector2 vector2 = new Vector2(-0.155f, 0.0f);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(-0.1325f, -0.322f) + vector2;
      myGuiControlLabel3.Size = new Vector2(0.04702703f, 0.02666667f);
      myGuiControlLabel3.Name = "BlockNameLabel";
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel3.Visible = false;
      myGuiControlLabel3.TextEnum = MySpaceTexts.Afterburner;
      this.m_blockNameLabel = myGuiControlLabel3;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = new Vector2(0.165f, -0.32f) + vector2;
      myGuiControlLabel4.Size = new Vector2(0.04702703f, 0.02666667f);
      myGuiControlLabel4.Name = "GroupTitleLabel";
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel4.TextEnum = MySpaceTexts.Terminal_GroupTitle;
      this.m_groupTitleLabel = myGuiControlLabel4;
      MyGuiControlTextbox guiControlTextbox = new MyGuiControlTextbox();
      guiControlTextbox.Position = new Vector2(0.165f, -0.283f) + vector2;
      guiControlTextbox.Size = new Vector2(0.29f, 0.052f);
      guiControlTextbox.Name = "GroupName";
      guiControlTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_groupName = guiControlTextbox;
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(new Vector2?(new Vector2(0.167f, -0.228f) + vector2), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(222f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.TerminalButton_GroupSave));
      guiControlButton1.Name = "GroupSave";
      this.m_groupSave = guiControlButton1;
      MyGuiControlButton guiControlButton2 = new MyGuiControlButton(new Vector2?(this.m_groupSave.Position + new Vector2(this.m_groupSave.Size.X + 0.013f, 0.0f)), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(222f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Delete));
      guiControlButton2.Name = "GroupDelete";
      this.m_groupDelete = guiControlButton2;
      MyGuiControlButton guiControlButton3 = new MyGuiControlButton(new Vector2?(new Vector2(-0.19f, -0.34f)), MyGuiControlButtonStyleEnum.SquareSmall, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, buttonScale: 0.5f);
      guiControlButton3.Name = "ShowAll";
      MyGuiControlButton guiControlButton4 = guiControlButton3;
      page.Controls.Add((MyGuiControlBase) controlSearchBox2);
      page.Controls.Add((MyGuiControlBase) guiControlListbox2);
      page.Controls.Add((MyGuiControlBase) guiControlButton4);
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.ControlPanel] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.ControlPanel] = (MyGuiControlBase) controlSearchBox2.GetTextbox();
    }

    public void AttachGroups(MyGuiControls parent)
    {
      parent.Add((MyGuiControlBase) this.m_blockNameLabel);
      parent.Add((MyGuiControlBase) this.m_groupTitleLabel);
      parent.Add((MyGuiControlBase) this.m_groupName);
      parent.Add((MyGuiControlBase) this.m_groupSave);
      parent.Add((MyGuiControlBase) this.m_groupDelete);
    }

    public void DetachGroups(MyGuiControls parent)
    {
      parent.Remove((MyGuiControlBase) this.m_blockNameLabel);
      parent.Remove((MyGuiControlBase) this.m_groupTitleLabel);
      parent.Remove((MyGuiControlBase) this.m_groupName);
      parent.Remove((MyGuiControlBase) this.m_groupSave);
      parent.Remove((MyGuiControlBase) this.m_groupDelete);
    }

    private void CreateFactionsPageControls(MyGuiControlTabPage page)
    {
      page.Name = "PageFactions";
      page.TextEnum = MySpaceTexts.TerminalTab_Factions;
      page.TextScale = 0.7005405f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.343f), 0.696f, 1f / 500f);
      page.Controls.Add((MyGuiControlBase) controlSeparatorList);
      float num1 = -0.462f;
      float num2 = -0.34f;
      float num3 = 0.0045f;
      float num4 = 0.01f;
      Vector2 vector2_1 = new Vector2(0.29f, 0.052f);
      Vector2 vector2_2 = new Vector2(0.13f, 0.04f);
      float num5 = num1 + num3;
      float y1 = num2 + num4;
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox();
      guiControlCombobox1.Position = new Vector2(-0.452f, y1);
      guiControlCombobox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      Vector2? size1 = this.Size;
      guiControlCombobox1.Size = new Vector2(0.29f, size1.Value.Y);
      guiControlCombobox1.Name = "FactionFilters";
      MyGuiControlCombobox guiControlCombobox2 = guiControlCombobox1;
      float y2 = y1 + (guiControlCombobox2.Size.Y + num4);
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.452f, y2)), new Vector2?(new Vector2(0.29f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = myGuiControlPanel2.Position + new Vector2(0.01f, 0.005f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.TerminalTab_FactionsTableLabel);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      float y3 = y2 + (myGuiControlPanel2.Size.Y - 1f / 1000f);
      MyGuiControlTable myGuiControlTable1 = new MyGuiControlTable();
      myGuiControlTable1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlTable1.Position = new Vector2(num5 + 0.0055f, y3);
      myGuiControlTable1.Size = new Vector2(0.29f, 0.15f);
      myGuiControlTable1.Name = "FactionsTable";
      myGuiControlTable1.ColumnsCount = 3;
      myGuiControlTable1.VisibleRowsCount = 13;
      MyGuiControlTable myGuiControlTable2 = myGuiControlTable1;
      myGuiControlTable2.SetCustomColumnWidths(new float[3]
      {
        0.23f,
        0.64f,
        0.13f
      });
      myGuiControlTable2.SetColumnName(0, MyTexts.Get(MyCommonTexts.Tag));
      myGuiControlTable2.SetHeaderColumnMargin(0, new Thickness(0.01f, 0.0f, 0.0f, 0.0f));
      myGuiControlTable2.SetColumnName(1, MyTexts.Get(MyCommonTexts.Name));
      myGuiControlTable2.SetHeaderColumnMargin(1, new Thickness(0.005f, 0.0f, 0.005f, 0.0f));
      myGuiControlTable2.SetHeaderColumnMargin(2, new Thickness(0.0f));
      float num6 = y3 + (myGuiControlTable2.Size.Y + num4);
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(new Vector2?(new Vector2(-0.449f, 0.305f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton1.Name = "buttonJoin";
      guiControlButton1.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton3 = new MyGuiControlButton(new Vector2?(new Vector2(-0.449f, 0.305f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton3.Name = "buttonCancelJoin";
      guiControlButton3.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton4 = guiControlButton3;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton5 = new MyGuiControlButton(new Vector2?(new Vector2(-0.449f, 0.305f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton5.Name = "buttonLeave";
      guiControlButton5.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton6 = guiControlButton5;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton7 = new MyGuiControlButton(new Vector2?(new Vector2(-0.16f, 0.305f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlButton7.Name = "buttonEnemy";
      guiControlButton7.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton8 = guiControlButton7;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton9 = new MyGuiControlButton(new Vector2?(new Vector2(-0.449f, 0.255f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton9.Name = "buttonCreate";
      guiControlButton9.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton10 = guiControlButton9;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton11 = new MyGuiControlButton(new Vector2?(new Vector2(-0.16f, 0.255f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlButton11.Name = "buttonSendPeace";
      guiControlButton11.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton12 = guiControlButton11;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton13 = new MyGuiControlButton(new Vector2?(new Vector2(-0.16f, 0.255f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlButton13.Name = "buttonCancelPeace";
      guiControlButton13.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton14 = guiControlButton13;
      size1 = new Vector2?(new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      MyGuiControlButton guiControlButton15 = new MyGuiControlButton(new Vector2?(new Vector2(-0.16f, 0.255f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlButton15.Name = "buttonAcceptPeace";
      guiControlButton15.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton16 = guiControlButton15;
      page.Controls.Add((MyGuiControlBase) guiControlCombobox2);
      page.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      page.Controls.Add((MyGuiControlBase) myGuiControlTable2);
      page.Controls.Add((MyGuiControlBase) guiControlButton10);
      page.Controls.Add((MyGuiControlBase) guiControlButton2);
      page.Controls.Add((MyGuiControlBase) guiControlButton4);
      page.Controls.Add((MyGuiControlBase) guiControlButton6);
      page.Controls.Add((MyGuiControlBase) guiControlButton12);
      page.Controls.Add((MyGuiControlBase) guiControlButton14);
      page.Controls.Add((MyGuiControlBase) guiControlButton16);
      page.Controls.Add((MyGuiControlBase) guiControlButton8);
      float num7 = -0.0475f;
      float y4 = -0.34f;
      MyGuiControlCompositePanel controlCompositePanel = new MyGuiControlCompositePanel();
      controlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlCompositePanel.Position = new Vector2(-0.05f, y4);
      controlCompositePanel.Size = new Vector2(0.5f, 0.69f);
      controlCompositePanel.Name = "compositeFaction";
      float num8 = num7 + num3;
      float y5 = y4 + num4;
      Vector2 vector2_3 = new Vector2(0.58f, 0.04f);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.124f, y5)), new Vector2?(new Vector2(0.4f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel3.Name = "labelFactionDesc";
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      float y6 = y5 + (myGuiControlLabel4.Size.Y + num4);
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      float num9 = (float) safeGuiRectangle.Height / (float) safeGuiRectangle.Width;
      Vector2? position1 = new Vector2?(new Vector2(-0.125f, y6));
      MyGuiBorderThickness? nullable = new MyGuiBorderThickness?(new MyGuiBorderThickness(1f / 500f, 0.0f, 0.0f, 0.0f));
      Vector2? size2 = new Vector2?(new Vector2((float) (0.579999983310699 - 0.100000001490116 * (double) num9 - (double) num3 * 2.0), 0.1f));
      Vector4? backgroundColor1 = new Vector4?();
      int? visibleLinesCount1 = new int?();
      MyGuiBorderThickness? textPadding1 = nullable;
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(position1, size2, backgroundColor1, textScale: 0.7f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, visibleLinesCount: visibleLinesCount1, textPadding: textPadding1);
      controlMultilineText1.VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBordered;
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.Name = "textFactionDesc";
      controlMultilineText1.CanHaveFocus = true;
      MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Position = new Vector2((float) ((double) controlMultilineText2.PositionX + (double) controlMultilineText2.Size.X + (double) num3 * 2.0), y6);
      myGuiControlImage1.Size = new Vector2(controlMultilineText2.Size.Y * num9, controlMultilineText2.Size.Y);
      myGuiControlImage1.Name = "factionIcon";
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      myGuiControlImage1.Padding = new MyGuiBorderThickness(1f);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      float y7 = y6 + (controlMultilineText2.Size.Y + 2f * num4);
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.124f, y7)), new Vector2?(vector2_3 - new Vector2(0.01f, 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel5.Name = "labelReputation";
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      myGuiControlLabel6.Visible = false;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2?(new Vector2(0.166f, y7)), new Vector2?(vector2_3 - new Vector2(0.01f, 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      myGuiControlLabel7.Name = "textReputation";
      MyGuiControlLabel myGuiControlLabel8 = myGuiControlLabel7;
      myGuiControlLabel8.Visible = false;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.124f, y7)), new Vector2?(vector2_3 - new Vector2(0.01f, 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel9.Name = "labelFactionPrivate";
      MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
      float y8 = y7 + (myGuiControlLabel10.Size.Y + num4);
      MyGuiReputationProgressBar reputationProgressBar1 = new MyGuiReputationProgressBar(new Vector2?(new Vector2(-0.124f, y8)), new Vector2?(new Vector2(0.58f, 0.06f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      reputationProgressBar1.Name = "progressReputation";
      MyGuiReputationProgressBar reputationProgressBar2 = reputationProgressBar1;
      reputationProgressBar2.Visible = false;
      if (MySession.Static != null)
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component != null)
          reputationProgressBar2.SetBorderValues(component.GetHostileMax(), component.GetFriendlyMax(), component.GetNeutralMin(), component.GetFriendlyMin());
      }
      Vector2? position2 = new Vector2?(new Vector2(-0.125f, y8));
      nullable = new MyGuiBorderThickness?(new MyGuiBorderThickness(1f / 500f, 0.0f, 0.0f, 0.0f));
      Vector2? size3 = new Vector2?(new Vector2(0.58f, 0.1f));
      Vector4? backgroundColor2 = new Vector4?();
      int? visibleLinesCount2 = new int?();
      MyGuiBorderThickness? textPadding2 = nullable;
      MyGuiControlMultilineText controlMultilineText3 = new MyGuiControlMultilineText(position2, size3, backgroundColor2, textScale: 0.7f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, visibleLinesCount: visibleLinesCount2, textPadding: textPadding2);
      controlMultilineText3.VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBordered;
      controlMultilineText3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText3.Name = "textFactionPrivate";
      controlMultilineText3.CanHaveFocus = true;
      MyGuiControlMultilineText controlMultilineText4 = controlMultilineText3;
      float y9 = y8 + (controlMultilineText2.Size.Y + 0.0275f);
      MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.125f, y9)), new Vector2?(vector2_3 * 0.5f), MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAccept).ToString(), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel11.Name = "labelFactionMembersAcceptEveryone";
      MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(new Vector2?(new Vector2((float) ((double) myGuiControlLabel12.PositionX + (double) myGuiControlLabel12.Size.X + 0.0199999995529652), myGuiControlLabel12.PositionY + 0.012f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlCheckbox1.Name = "checkFactionMembersAcceptEveryone";
      MyGuiControlCheckbox guiControlCheckbox2 = guiControlCheckbox1;
      MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel(new Vector2?(new Vector2((float) ((double) myGuiControlLabel12.PositionX + (double) myGuiControlLabel12.Size.X + 0.0799999982118607), myGuiControlLabel12.PositionY)), new Vector2?(vector2_3 * 0.5f), MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequest).ToString(), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel13.Name = "labelFactionMembersAcceptPeace";
      MyGuiControlLabel myGuiControlLabel14 = myGuiControlLabel13;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(new Vector2?(new Vector2((float) ((double) myGuiControlLabel14.PositionX + (double) myGuiControlLabel14.Size.X + 0.0199999995529652), myGuiControlLabel14.PositionY + 0.012f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlCheckbox3.Name = "checkFactionMembersAcceptPeace";
      MyGuiControlCheckbox guiControlCheckbox4 = guiControlCheckbox3;
      float y10 = y9 + (myGuiControlLabel12.Size.Y + 2f * num4);
      MyGuiControlPanel myGuiControlPanel3 = new MyGuiControlPanel();
      myGuiControlPanel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlPanel3.Position = new Vector2(-0.125f, y10);
      myGuiControlPanel3.Size = new Vector2(0.44f, 0.04f);
      myGuiControlPanel3.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      myGuiControlPanel3.Name = "panelFactionMembersNamePanel";
      MyGuiControlPanel myGuiControlPanel4 = myGuiControlPanel3;
      float y11 = y10 + 0.007f;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel(new Vector2?(new Vector2(-57f / 500f, y11)), new Vector2?(vector2_3 - new Vector2(0.01f, 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel15.Name = "labelFactionMembers";
      MyGuiControlLabel myGuiControlLabel16 = myGuiControlLabel15;
      float y12 = y11 + (myGuiControlPanel4.Size.Y - 0.007f - MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(1f)).Y);
      Vector2 vector2_4 = vector2_3 - new Vector2(0.14f, 0.01f);
      MyGuiControlTable myGuiControlTable3 = new MyGuiControlTable();
      myGuiControlTable3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlTable3.Position = new Vector2(-0.125f, y12);
      myGuiControlTable3.Size = new Vector2(vector2_4.X, 0.15f);
      myGuiControlTable3.Name = "tableMembers";
      myGuiControlTable3.ColumnsCount = 2;
      myGuiControlTable3.VisibleRowsCount = 6;
      myGuiControlTable3.HeaderVisible = false;
      MyGuiControlTable myGuiControlTable4 = myGuiControlTable3;
      myGuiControlTable4.SetCustomColumnWidths(new float[2]
      {
        0.5f,
        0.5f
      });
      myGuiControlTable4.SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
      myGuiControlTable4.SetColumnName(1, MyTexts.Get(MyCommonTexts.Status));
      float num10 = (float) ((double) vector2_2.Y + (double) num4 - 0.00270000007003546);
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton17 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), guiControlCheckbox2.PositionY - 0.017f)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton17.Name = "buttonEdit";
      guiControlButton17.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton18 = guiControlButton17;
      float y13 = myGuiControlPanel4.PositionY + 1f / 500f;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton19 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y13)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton19.Name = "buttonPromote";
      guiControlButton19.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton20 = guiControlButton19;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton21 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y13 + num10)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton21.Name = "buttonKick";
      guiControlButton21.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton22 = guiControlButton21;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton23 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y13 + 2f * num10)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton23.Name = "buttonAcceptJoin";
      guiControlButton23.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton24 = guiControlButton23;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton25 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y13 + 3f * num10)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton25.Name = "buttonDemote";
      guiControlButton25.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton26 = guiControlButton25;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton27 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y13 + 4f * num10)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton27.Name = "buttonShareProgress";
      guiControlButton27.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton28 = guiControlButton27;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton29 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y13 + 5f * num10)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton29.Name = "buttonAddNpc";
      guiControlButton29.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton30 = guiControlButton29;
      float y14 = (float) ((double) y13 + 6.0 * (double) num10 + 0.00499999988824129);
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton31 = new MyGuiControlButton(new Vector2?(new Vector2((float) ((double) num8 + (double) myGuiControlTable4.Size.X + (double) num4 - 0.0810000002384186), y14)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      guiControlButton31.Name = "withdrawBtn";
      guiControlButton31.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton32 = guiControlButton31;
      size1 = new Vector2?(vector2_2);
      MyGuiControlButton guiControlButton33 = new MyGuiControlButton(new Vector2?(new Vector2(guiControlButton32.PositionX - num4, y14)), MyGuiControlButtonStyleEnum.Rectangular, size1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlButton33.Name = "depositBtn";
      guiControlButton33.ShowTooltipWhenDisabled = true;
      MyGuiControlButton guiControlButton34 = guiControlButton33;
      float y15 = y12 + (myGuiControlTable4.Size.Y + 0.018f);
      MyGuiControlLabel myGuiControlLabel17 = new MyGuiControlLabel();
      myGuiControlLabel17.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel17.Position = new Vector2(-0.125f, y15);
      myGuiControlLabel17.Size = new Vector2(0.3f, 0.08f);
      myGuiControlLabel17.TextEnum = MySpaceTexts.Currency_Default_Account_Label;
      myGuiControlLabel17.Name = "labelBalance";
      MyGuiControlLabel myGuiControlLabel18 = myGuiControlLabel17;
      float num11 = (float) safeGuiRectangle.Width / (float) safeGuiRectangle.Height;
      Vector2 vector2_5 = new Vector2(0.018f, num11 * 0.018f);
      MyGuiControlLabel myGuiControlLabel19 = new MyGuiControlLabel();
      myGuiControlLabel19.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      myGuiControlLabel19.Position = new Vector2((float) ((double) guiControlButton34.PositionX - (double) guiControlButton34.Size.X - 0.00999999977648258) - vector2_5.X, y15);
      myGuiControlLabel19.Size = new Vector2(0.3f, 0.08f);
      myGuiControlLabel19.Text = "N/A";
      myGuiControlLabel19.Name = "labelBalanceValue";
      MyGuiControlLabel myGuiControlLabel20 = myGuiControlLabel19;
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
      myGuiControlImage3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage3.Position = myGuiControlLabel20.Position + new Vector2(0.005f, 0.0f);
      myGuiControlImage3.Name = "imageCurrency";
      myGuiControlImage3.Size = new Vector2(0.018f, num11 * 0.018f);
      myGuiControlImage3.Visible = false;
      MyGuiControlImage myGuiControlImage4 = myGuiControlImage3;
      page.Controls.Add((MyGuiControlBase) myGuiControlPanel4);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      page.Controls.Add((MyGuiControlBase) controlMultilineText2);
      page.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      page.Controls.Add((MyGuiControlBase) reputationProgressBar2);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      page.Controls.Add((MyGuiControlBase) controlMultilineText4);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
      page.Controls.Add((MyGuiControlBase) guiControlCheckbox2);
      page.Controls.Add((MyGuiControlBase) guiControlCheckbox4);
      page.Controls.Add((MyGuiControlBase) myGuiControlTable4);
      page.Controls.Add((MyGuiControlBase) guiControlButton18);
      page.Controls.Add((MyGuiControlBase) guiControlButton20);
      page.Controls.Add((MyGuiControlBase) guiControlButton22);
      page.Controls.Add((MyGuiControlBase) guiControlButton26);
      page.Controls.Add((MyGuiControlBase) guiControlButton24);
      page.Controls.Add((MyGuiControlBase) guiControlButton28);
      page.Controls.Add((MyGuiControlBase) guiControlButton30);
      page.Controls.Add((MyGuiControlBase) guiControlButton32);
      page.Controls.Add((MyGuiControlBase) guiControlButton34);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel18);
      page.Controls.Add((MyGuiControlBase) myGuiControlLabel20);
      page.Controls.Add((MyGuiControlBase) myGuiControlImage4);
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Factions] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Factions] = (MyGuiControlBase) myGuiControlTable2;
    }

    private void CreateChatPageControls(MyGuiControlTabPage chatPage)
    {
      chatPage.Name = "PageComms";
      chatPage.TextEnum = MySpaceTexts.TerminalTab_Chat;
      chatPage.TextScale = 0.7005405f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 1f / 500f);
      chatPage.Controls.Add((MyGuiControlBase) controlSeparatorList);
      float x1 = -0.452f;
      float num1 = -x1;
      float num2 = -0.332f;
      int num3 = 11;
      float x2 = 0.35f;
      float num4 = 0.02f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(x1 + 0.01f, num2 + 0.005f);
      myGuiControlLabel1.Name = "PlayerLabel";
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.TerminalTab_PlayersTableLabel);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.452f, -0.332f)), new Vector2?(new Vector2(0.29f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      chatPage.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      chatPage.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      float y1 = num2 + (myGuiControlLabel2.GetTextSize().Y + 0.012f);
      MyGuiControlListbox guiControlListbox1 = new MyGuiControlListbox();
      guiControlListbox1.Position = new Vector2(x1, y1);
      guiControlListbox1.Size = new Vector2(x2, 0.0f);
      guiControlListbox1.Name = "PlayerListbox";
      guiControlListbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlListbox1.VisibleRowsCount = num3;
      guiControlListbox1.VisualStyle = MyGuiControlListboxStyleEnum.ChatScreen;
      MyGuiControlListbox guiControlListbox2 = guiControlListbox1;
      chatPage.Controls.Add((MyGuiControlBase) guiControlListbox2);
      float num5 = guiControlListbox2.ItemSize.Y * (float) num3;
      float num6 = y1 + (float) ((double) num5 + (double) num4 + 0.00400000018998981);
      int num7 = 6;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(x1 + 0.01f, num6 + 3f / 1000f);
      myGuiControlLabel3.Name = "PlayerLabel";
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.TerminalTab_FactionsTableLabel);
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlPanel myGuiControlPanel3 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.452f, 0.097f)), new Vector2?(new Vector2(0.29f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel3.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel4 = myGuiControlPanel3;
      chatPage.Controls.Add((MyGuiControlBase) myGuiControlPanel4);
      chatPage.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      float y2 = num6 + (myGuiControlLabel2.GetTextSize().Y + 0.01f);
      MyGuiControlListbox guiControlListbox3 = new MyGuiControlListbox();
      guiControlListbox3.Position = new Vector2(x1, y2);
      guiControlListbox3.Size = new Vector2(x2, 0.0f);
      guiControlListbox3.Name = "FactionListbox";
      guiControlListbox3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlListbox3.VisibleRowsCount = num7;
      guiControlListbox3.VisualStyle = MyGuiControlListboxStyleEnum.ChatScreen;
      MyGuiControlListbox guiControlListbox4 = guiControlListbox3;
      chatPage.Controls.Add((MyGuiControlBase) guiControlListbox4);
      float num8 = -0.34f;
      float num9 = 0.6f;
      float num10 = 0.515f;
      float num11 = 0.038f;
      MyGuiControlPanel myGuiControlPanel5 = new MyGuiControlPanel();
      myGuiControlPanel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlPanel5.Position = new Vector2(-0.125f, num8 + 0.008f);
      myGuiControlPanel5.Size = new Vector2(num9 - 0.019f, num10 + 0.1f);
      myGuiControlPanel5.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
      MyGuiControlPanel myGuiControlPanel6 = myGuiControlPanel5;
      chatPage.Controls.Add((MyGuiControlBase) myGuiControlPanel6);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(num1 + 3f / 1000f, num8 + 0.02f)), new Vector2?(new Vector2(num9 - 0.033f, num10 + 0.08f)), textScale: 0.7394595f);
      controlMultilineText.Name = "ChatHistory";
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      chatPage.Controls.Add((MyGuiControlBase) controlMultilineText);
      float num12 = num8 + (num10 + num11);
      float y3 = 0.05f;
      MyGuiControlTextbox guiControlTextbox1 = new MyGuiControlTextbox();
      guiControlTextbox1.Position = new Vector2(num1 - 0.5765f, num12 + 0.104f);
      guiControlTextbox1.Size = new Vector2(num9 - 0.165f, y3);
      guiControlTextbox1.Name = "Chatbox";
      guiControlTextbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      MyGuiControlTextbox guiControlTextbox2 = guiControlTextbox1;
      chatPage.Controls.Add((MyGuiControlBase) guiControlTextbox2);
      float x3 = 0.75f;
      float num13 = num12 + (y3 + num11);
      float y4 = 0.05f;
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
      guiControlButton1.Position = new Vector2(num1 + 0.007f, num13 + 23f / 1000f);
      guiControlButton1.Text = MyTexts.Get(MyCommonTexts.SendMessage).ToString();
      guiControlButton1.Name = "SendButton";
      guiControlButton1.Size = new Vector2(x3, y4);
      guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      guiControlButton2.VisualStyle = MyGuiControlButtonStyleEnum.ComboBoxButton;
      chatPage.Controls.Add((MyGuiControlBase) guiControlButton2);
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Comms] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Comms] = (MyGuiControlBase) guiControlListbox2;
    }

    private void CreateInfoPageControls(MyGuiControlTabPage infoPage)
    {
      infoPage.Name = "PageInfo";
      infoPage.TextEnum = MySpaceTexts.TerminalTab_Info;
      infoPage.TextScale = 0.7005405f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(0.145f, -0.333f), 0.676f, 1f / 500f);
      controlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 1f / 500f);
      controlSeparatorList.AddHorizontal(new Vector2(0.168f, 0.148f), 0.27f, 1f / 1000f);
      infoPage.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel();
      myGuiControlPanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlPanel1.Position = new Vector2(-0.452f, -0.332f);
      myGuiControlPanel1.Size = new Vector2(0.29f, 0.035f);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(-0.442f, -0.327f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_GridInfoLabel);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      myGuiControlLabel2.Name = "Infolabel";
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlList myGuiControlList = new MyGuiControlList(new Vector2?(new Vector2(-0.452f, -0.299f)), new Vector2?(new Vector2(0.29f, 0.6405f)));
      myGuiControlList.Name = "InfoList";
      myGuiControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlList);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(0.168f, 0.05f);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShipName);
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      myGuiControlLabel4.Name = "RenameShipLabel";
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.m_convertToShipBtn = new MyGuiControlButton();
      this.m_convertToShipBtn.Position = new Vector2(0.31f, 0.225f);
      this.m_convertToShipBtn.TextEnum = MySpaceTexts.TerminalTab_Info_ConvertButton;
      this.m_convertToShipBtn.SetToolTip(MySpaceTexts.TerminalTab_Info_ConvertButton_TT);
      this.m_convertToShipBtn.ShowTooltipWhenDisabled = true;
      this.m_convertToShipBtn.Name = "ConvertBtn";
      infoPage.Controls.Add((MyGuiControlBase) this.m_convertToShipBtn);
      this.m_convertToStationBtn = new MyGuiControlButton();
      this.m_convertToStationBtn.Position = new Vector2(0.31f, 0.285f);
      this.m_convertToStationBtn.TextEnum = MySpaceTexts.TerminalTab_Info_ConvertToStationButton;
      this.m_convertToStationBtn.SetToolTip(MySpaceTexts.TerminalTab_Info_ConvertToStationButton_TT);
      this.m_convertToStationBtn.ShowTooltipWhenDisabled = true;
      this.m_convertToStationBtn.Name = "ConvertToStationBtn";
      this.m_convertToStationBtn.Visible = MySession.Static.EnableConvertToStation;
      infoPage.Controls.Add((MyGuiControlBase) this.m_convertToStationBtn);
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(new Vector2?(), new Vector2?(), new Vector4?(), "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, (StringBuilder) null, true, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, new int?(), false, false, (MyGuiCompositeTexture) null, new MyGuiBorderThickness?());
      controlMultilineText1.Name = "InfoHelpMultilineText";
      controlMultilineText1.Position = new Vector2(0.167f, -0.3345f);
      controlMultilineText1.Size = new Vector2(0.297f, 0.36f);
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Info_Description);
      MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
      infoPage.Controls.Add((MyGuiControlBase) controlMultilineText2);
      if (MyFakes.ENABLE_CENTER_OF_MASS)
      {
        MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, -0.313f)), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowMassCenter));
        myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
        MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.135f, myGuiControlLabel5.Position.Y)));
        guiControlCheckbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        guiControlCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowMassCenter_ToolTip));
        guiControlCheckbox.Name = "CenterBtn";
        infoPage.Controls.Add((MyGuiControlBase) guiControlCheckbox);
        this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Info] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Info] = (MyGuiControlBase) guiControlCheckbox;
      }
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, -0.263f)), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowGravityGizmo));
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.135f, myGuiControlLabel6.Position.Y)));
      guiControlCheckbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlCheckbox1.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowGravityGizmo_ToolTip));
      guiControlCheckbox1.Name = "ShowGravityGizmo";
      infoPage.Controls.Add((MyGuiControlBase) guiControlCheckbox1);
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, -0.213f)), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowSenzorGizmo), isAutoEllipsisEnabled: true, maxWidth: 0.218f, isAutoScaleEnabled: true);
      myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.135f, myGuiControlLabel7.Position.Y)));
      guiControlCheckbox2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlCheckbox2.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowSenzorGizmo_ToolTip));
      guiControlCheckbox2.Name = "ShowSenzorGizmo";
      infoPage.Controls.Add((MyGuiControlBase) guiControlCheckbox2);
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, -0.163f)), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowAntenaGizmo));
      myGuiControlLabel8.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.135f, myGuiControlLabel8.Position.Y)));
      guiControlCheckbox3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlCheckbox3.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowAntenaGizmo_ToolTip));
      guiControlCheckbox3.Name = "ShowAntenaGizmo";
      infoPage.Controls.Add((MyGuiControlBase) guiControlCheckbox3);
      MyGuiScreenTerminal.CreateAntennaSlider(infoPage, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_FriendlyAntennaRange), "FriendAntennaRange", -0.05f, true, 0.32f);
      MyGuiScreenTerminal.CreateAntennaSlider(infoPage, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_EnemyAntennaRange), "EnemyAntennaRange", 0.09f, true, 0.32f);
      MyGuiScreenTerminal.CreateAntennaSlider(infoPage, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_OwnedAntennaRange), "OwnedAntennaRange", 0.23f, true, 0.32f);
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, -0.113f)), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_PivotBtn));
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel9);
      MyGuiControlCheckbox guiControlCheckbox4 = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.135f, myGuiControlLabel9.Position.Y)));
      guiControlCheckbox4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlCheckbox4.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_PivotBtn_ToolTip));
      guiControlCheckbox4.Name = "PivotBtn";
      infoPage.Controls.Add((MyGuiControlBase) guiControlCheckbox4);
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
      {
        MyGuiControlTextbox guiControlTextbox = new MyGuiControlTextbox();
        guiControlTextbox.Name = "RenameShipText";
        guiControlTextbox.Position = new Vector2(0.168f, myGuiControlLabel4.PositionY + 0.048f);
        guiControlTextbox.Size = new Vector2(0.225f, 0.005f);
        guiControlTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        MyGuiControlTextbox control = guiControlTextbox;
        MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
        guiControlButton1.Name = "RenameShipButton";
        guiControlButton1.Position = new Vector2((float) ((double) control.PositionX + (double) control.Size.X + 0.025000000372529), control.PositionY + 3f / 500f);
        guiControlButton1.Text = MyTexts.Get(MyCommonTexts.Ok).ToString();
        guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
        guiControlButton1.Size = new Vector2(0.036f, 0.0392f);
        MyGuiControlButton guiControlButton2 = guiControlButton1;
        guiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
        infoPage.Controls.Add((MyGuiControlBase) guiControlButton2);
        control.SetTooltip(MyTexts.Get(MySpaceTexts.TerminalName).ToString());
        control.ShowTooltipWhenDisabled = true;
        infoPage.Controls.Add((MyGuiControlBase) control);
      }
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, 0.28f)), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_DestructibleBlocks));
      myGuiControlLabel10.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlLabel10.Visible = MySession.Static.Settings.ScenarioEditMode || MySession.Static.IsScenario;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      MyGuiControlCheckbox guiControlCheckbox5 = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.135f, myGuiControlLabel10.Position.Y)), toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_DestructibleBlocks_Tooltip));
      guiControlCheckbox5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlCheckbox5.Name = "SetDestructibleBlocks";
      infoPage.Controls.Add((MyGuiControlBase) guiControlCheckbox5);
    }

    private static bool OnAntennaSliderClicked(MyGuiControlSlider arg)
    {
      if (!MyInput.Static.IsAnyCtrlKeyPressed())
        return false;
      float min = MyHudMarkerRender.Denormalize(0.0f);
      float max = MyHudMarkerRender.Denormalize(1f);
      float num = MyHudMarkerRender.Denormalize(arg.Value);
      bool parseAsInteger = true;
      if (parseAsInteger && (double) Math.Abs(min) < 1.0)
        min = 0.0f;
      MyGuiScreenDialogAmount screenDialogAmount = new MyGuiScreenDialogAmount(min, max, MyCommonTexts.DialogAmount_SetValueCaption, parseAsInteger: parseAsInteger, defaultAmount: new float?(num), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity);
      screenDialogAmount.OnConfirmed += (Action<float>) (v => arg.Value = MyHudMarkerRender.Normalize(v));
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenDialogAmount);
      return true;
    }

    private static void CreateAntennaSlider(
      MyGuiControlTabPage infoPage,
      string labelText,
      string name,
      float startY,
      bool isAutoScaleEnabled = false,
      float maxTextWidth = 1f)
    {
      MyGuiControlLabel friendAntennaRangeValueLabel = new MyGuiControlLabel(new Vector2?(new Vector2(-0.123f, startY + 0.09f)));
      friendAntennaRangeValueLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      infoPage.Controls.Add((MyGuiControlBase) friendAntennaRangeValueLabel);
      MyGuiControlSlider guiControlSlider = new MyGuiControlSlider(new Vector2?(new Vector2(0.126f, startY + 0.05f)));
      guiControlSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlSlider.Name = name;
      guiControlSlider.Size = new Vector2(0.25f, 1f);
      guiControlSlider.MinValue = 0.0f;
      guiControlSlider.MaxValue = 1f;
      guiControlSlider.DefaultValue = new float?(guiControlSlider.MaxValue);
      guiControlSlider.ValueChanged += (Action<MyGuiControlSlider>) (s => friendAntennaRangeValueLabel.Text = MyValueFormatter.GetFormatedFloat(MyHudMarkerRender.Denormalize(s.Value), 0) + "m");
      guiControlSlider.SliderClicked = new Func<MyGuiControlSlider, bool>(MyGuiScreenTerminal.OnAntennaSliderClicked);
      infoPage.Controls.Add((MyGuiControlBase) guiControlSlider);
      Vector2? position = new Vector2?(new Vector2(-0.123f, startY));
      Vector2? size = new Vector2?();
      string text = labelText;
      Vector4? colorMask = new Vector4?();
      bool flag = isAutoScaleEnabled;
      double num1 = (double) maxTextWidth;
      int num2 = flag ? 1 : 0;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(position, size, text, colorMask, maxWidth: ((float) num1), isAutoScaleEnabled: (num2 != 0));
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      infoPage.Controls.Add((MyGuiControlBase) myGuiControlLabel);
    }

    private void CreateProductionPageControls(MyGuiControlTabPage productionPage)
    {
      productionPage.Name = "PageProduction";
      productionPage.TextEnum = MySpaceTexts.TerminalTab_Production;
      productionPage.TextScale = 0.7005405f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(0.145f, -0.333f), 0.676f, 1f / 500f);
      controlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 1f / 500f);
      productionPage.Controls.Add((MyGuiControlBase) controlSeparatorList);
      float num1 = 0.03f;
      float num2 = 0.01f;
      float num3 = 0.05f;
      float y = 0.08f;
      MyGuiControlCombobox guiControlCombobox = new MyGuiControlCombobox(new Vector2?(-0.5f * productionPage.Size + new Vector2(1f / 1000f, num2 + 0.174f)));
      guiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlCombobox.Name = "AssemblersCombobox";
      this.m_assemblersCombobox = guiControlCombobox;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(-0.5f * productionPage.Size + new Vector2(0.0f, num2 + 0.028f) + new Vector2(1f / 1000f, (float) ((double) this.m_assemblersCombobox.Size.Y + (double) num2 - 1.0 / 1000.0 - 0.0480000004172325))), new Vector2?(new Vector2(1f, y)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      myGuiControlPanel1.Name = "BlueprintsBackgroundPanel";
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      MyGuiControlPanel myGuiControlPanel3 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.452f, -0.332f)), new Vector2?(new Vector2(0.29f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel3.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel4 = myGuiControlPanel3;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(myGuiControlPanel2.Position + new Vector2(num2, num2 - 0.005f)), text: MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_Blueprints), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel1.Name = "BlueprintsLabel";
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlSearchBox controlSearchBox1 = new MyGuiControlSearchBox(new Vector2?(myGuiControlPanel2.Position + new Vector2(0.0f, y + num2)), new Vector2?(this.m_assemblersCombobox.Size), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlSearchBox1.Name = "BlueprintsSearchBox";
      MyGuiControlSearchBox controlSearchBox2 = controlSearchBox1;
      MyGuiControlGrid myGuiControlGrid1 = new MyGuiControlGrid();
      myGuiControlGrid1.VisualStyle = MyGuiControlGridStyleEnum.Blueprints;
      myGuiControlGrid1.RowsCount = MyTerminalProductionController.BLUEPRINT_GRID_ROWS;
      myGuiControlGrid1.ColumnsCount = 5;
      myGuiControlGrid1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlGrid1.ShowTooltipWhenDisabled = true;
      MyGuiControlScrollablePanel controlScrollablePanel1 = new MyGuiControlScrollablePanel((MyGuiControlBase) myGuiControlGrid1);
      controlScrollablePanel1.Name = "BlueprintsScrollableArea";
      controlScrollablePanel1.ScrollbarVEnabled = true;
      controlScrollablePanel1.Position = this.m_assemblersCombobox.Position + new Vector2(0.0f, this.m_assemblersCombobox.Size.Y + num2);
      controlScrollablePanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
      controlScrollablePanel1.Size = new Vector2(myGuiControlPanel2.Size.X, 0.5f);
      controlScrollablePanel1.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
      controlScrollablePanel1.DrawScrollBarSeparator = true;
      MyGuiControlScrollablePanel controlScrollablePanel2 = controlScrollablePanel1;
      controlScrollablePanel2.FitSizeToScrolledControl();
      this.m_assemblersCombobox.Size = new Vector2(controlScrollablePanel2.Size.X, this.m_assemblersCombobox.Size.Y);
      controlSearchBox2.Size = this.m_assemblersCombobox.Size;
      myGuiControlPanel2.Size = new Vector2(controlScrollablePanel2.Size.X, y);
      myGuiControlGrid1.RowsCount = 20;
      productionPage.Controls.Add((MyGuiControlBase) this.m_assemblersCombobox);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlPanel4);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      productionPage.Controls.Add((MyGuiControlBase) controlSearchBox2);
      productionPage.Controls.Add((MyGuiControlBase) controlScrollablePanel2);
      Vector2 vector2 = myGuiControlPanel2.Position + new Vector2((float) ((double) myGuiControlPanel2.Size.X + (double) num1 + 0.0500000007450581), 0.0f);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(vector2 + new Vector2(num2, num2) + new Vector2(-0.05f, 1f / 500f)), text: MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_StoredMaterials), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2?(vector2 + new Vector2(num2, num2) + new Vector2(-0.05f, 0.028f)), text: MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_MaterialType), textScale: 0.704f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(vector2 + new Vector2(num2, num2) + new Vector2(0.2f, 0.028f)), textScale: 0.704f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      myGuiControlLabel5.Name = "RequiredLabel";
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      MyGuiControlComponentList controlComponentList1 = new MyGuiControlComponentList();
      controlComponentList1.Position = vector2 + new Vector2(-0.062f, num3 - 1f / 500f);
      controlComponentList1.Size = new Vector2(0.29f, num3 + controlScrollablePanel2.Size.Y - num3);
      controlComponentList1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlComponentList1.BackgroundTexture = (MyGuiCompositeTexture) null;
      controlComponentList1.Name = "MaterialsList";
      MyGuiControlComponentList controlComponentList2 = controlComponentList1;
      productionPage.Controls.Add((MyGuiControlBase) controlComponentList2);
      MyGuiControlRadioButton controlRadioButton1 = new MyGuiControlRadioButton(new Vector2?(vector2 + new Vector2((float) ((double) myGuiControlPanel2.Size.X + (double) num1 - 0.0710000023245811), 0.0f)), new Vector2?(new Vector2(210f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE));
      controlRadioButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton1.Icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_BUTTON_ICON_COMPONENT);
      controlRadioButton1.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlRadioButton1.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      controlRadioButton1.Text = MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_AssemblingButton);
      controlRadioButton1.Name = "AssemblingButton";
      this.m_assemblingButton = controlRadioButton1;
      this.m_assemblingButton.SetToolTip(MySpaceTexts.ToolTipTerminalProduction_AssemblingMode);
      MyGuiControlRadioButton controlRadioButton2 = new MyGuiControlRadioButton(new Vector2?(this.m_assemblingButton.Position + new Vector2(this.m_assemblingButton.Size.X + num2, 0.0f)), new Vector2?(new Vector2(238f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE), isAutoScaleEnabled: true);
      controlRadioButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton2.Icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_BUTTON_ICON_DISASSEMBLY);
      controlRadioButton2.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlRadioButton2.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      controlRadioButton2.Text = MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_DisassemblingButton);
      controlRadioButton2.Name = "DisassemblingButton";
      this.m_disassemblingButton = controlRadioButton2;
      this.m_disassemblingButton.SetToolTip(MySpaceTexts.ToolTipTerminalProduction_DisassemblingMode);
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.Position = this.m_assemblingButton.Position + new Vector2(0.0f, this.m_assemblingButton.Size.Y + num2);
      controlCompositePanel1.Size = new Vector2(0.4f, y);
      controlCompositePanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlCompositePanel controlCompositePanel2 = controlCompositePanel1;
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(new Vector2?(controlCompositePanel2.Position + new Vector2(num2, num2) + new Vector2(0.0f, 0.017f)), text: MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_ProductionQueue), isAutoEllipsisEnabled: true, maxWidth: 0.12f, isAutoScaleEnabled: true);
      MyGuiControlGrid myGuiControlGrid2 = new MyGuiControlGrid();
      myGuiControlGrid2.VisualStyle = MyGuiControlGridStyleEnum.Blueprints;
      myGuiControlGrid2.RowsCount = 2;
      myGuiControlGrid2.ColumnsCount = 5;
      myGuiControlGrid2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlGrid2.ShowTooltipWhenDisabled = true;
      MyGuiControlScrollablePanel controlScrollablePanel3 = new MyGuiControlScrollablePanel((MyGuiControlBase) myGuiControlGrid2);
      controlScrollablePanel3.Name = "QueueScrollableArea";
      controlScrollablePanel3.ScrollbarVEnabled = true;
      controlScrollablePanel3.Position = controlCompositePanel2.Position + new Vector2(0.0f, controlCompositePanel2.Size.Y);
      controlScrollablePanel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel3.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
      controlScrollablePanel3.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
      controlScrollablePanel3.DrawScrollBarSeparator = true;
      MyGuiControlScrollablePanel controlScrollablePanel4 = controlScrollablePanel3;
      controlScrollablePanel4.FitSizeToScrolledControl();
      myGuiControlGrid2.RowsCount = 10;
      controlCompositePanel2.Size = new Vector2(controlScrollablePanel4.Size.X, controlCompositePanel2.Size.Y);
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(new Vector2?(controlCompositePanel2.Position + new Vector2(controlCompositePanel2.Size.X - num2, num2)), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_RepeatMode), visualStyle: MyGuiControlCheckboxStyleEnum.Repeat, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlCheckbox1.Name = "RepeatCheckbox";
      MyGuiControlCheckbox guiControlCheckbox2 = guiControlCheckbox1;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(new Vector2?(controlCompositePanel2.Position + new Vector2(controlCompositePanel2.Size.X - 0.1f - num2, num2)), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_SlaveMode), visualStyle: MyGuiControlCheckboxStyleEnum.Slave, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      guiControlCheckbox3.Name = "SlaveCheckbox";
      MyGuiControlCheckbox guiControlCheckbox4 = guiControlCheckbox3;
      MyGuiControlCompositePanel controlCompositePanel3 = new MyGuiControlCompositePanel();
      controlCompositePanel3.Position = controlScrollablePanel4.Position + new Vector2(0.0f, controlScrollablePanel4.Size.Y + num2);
      controlCompositePanel3.Size = new Vector2(0.4f, y);
      controlCompositePanel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlCompositePanel3.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlCompositePanel controlCompositePanel4 = controlCompositePanel3;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2?(controlCompositePanel4.Position + new Vector2(num2, num2)), text: MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_Inventory), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      MyGuiControlGrid myGuiControlGrid3 = new MyGuiControlGrid();
      myGuiControlGrid3.VisualStyle = MyGuiControlGridStyleEnum.Blueprints;
      myGuiControlGrid3.RowsCount = 3;
      myGuiControlGrid3.ColumnsCount = 5;
      myGuiControlGrid3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlGrid3.ShowTooltipWhenDisabled = true;
      MyGuiControlScrollablePanel controlScrollablePanel5 = new MyGuiControlScrollablePanel((MyGuiControlBase) myGuiControlGrid3);
      controlScrollablePanel5.Name = "InventoryScrollableArea";
      controlScrollablePanel5.ScrollbarVEnabled = true;
      controlScrollablePanel5.Position = controlCompositePanel4.Position + new Vector2(0.0f, controlCompositePanel4.Size.Y);
      controlScrollablePanel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel5.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
      controlScrollablePanel5.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
      controlScrollablePanel5.DrawScrollBarSeparator = true;
      MyGuiControlScrollablePanel controlScrollablePanel6 = controlScrollablePanel5;
      controlScrollablePanel6.FitSizeToScrolledControl();
      myGuiControlGrid3.RowsCount = 10;
      controlCompositePanel4.Size = new Vector2(controlScrollablePanel6.Size.X, controlCompositePanel4.Size.Y);
      Vector2? position = new Vector2?(controlCompositePanel4.Position + new Vector2(controlCompositePanel4.Size.X - num2, num2));
      Vector2? size = new Vector2?(new Vector2(220f, 40f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_DisassembleAllButton);
      string toolTip = MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_DisassembleAll);
      StringBuilder text = stringBuilder;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, toolTip, text, buttonIndex: buttonIndex);
      guiControlButton1.Name = "DisassembleAllButton";
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      MyGuiControlButton guiControlButton3 = new MyGuiControlButton(new Vector2?(controlScrollablePanel6.Position + new Vector2(1f / 500f, (float) ((double) controlScrollablePanel6.Size.Y + (double) num2 + 0.0480000004172325))), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(224f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_InventoryButton));
      guiControlButton3.Name = "InventoryButton";
      MyGuiControlButton guiControlButton4 = guiControlButton3;
      MyGuiControlButton guiControlButton5 = new MyGuiControlButton(new Vector2?(guiControlButton4.Position + new Vector2(guiControlButton4.Size.X + num2, 0.0f)), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(guiControlButton4.Size), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_ControlPanelButton));
      guiControlButton5.Name = "ControlPanelButton";
      MyGuiControlButton guiControlButton6 = guiControlButton5;
      productionPage.Controls.Add((MyGuiControlBase) this.m_assemblingButton);
      productionPage.Controls.Add((MyGuiControlBase) this.m_disassemblingButton);
      productionPage.Controls.Add((MyGuiControlBase) controlCompositePanel2);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      productionPage.Controls.Add((MyGuiControlBase) guiControlCheckbox2);
      productionPage.Controls.Add((MyGuiControlBase) guiControlCheckbox4);
      productionPage.Controls.Add((MyGuiControlBase) controlScrollablePanel4);
      productionPage.Controls.Add((MyGuiControlBase) controlCompositePanel4);
      productionPage.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      productionPage.Controls.Add((MyGuiControlBase) guiControlButton2);
      productionPage.Controls.Add((MyGuiControlBase) controlScrollablePanel6);
      productionPage.Controls.Add((MyGuiControlBase) guiControlButton4);
      productionPage.Controls.Add((MyGuiControlBase) guiControlButton6);
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Production] = (MyGuiControlBase) controlSearchBox2.TextBox;
      this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Production] = (MyGuiControlBase) controlSearchBox2.TextBox;
    }

    private void CreateGpsPageControls(MyGuiControlTabPage gpsPage)
    {
      gpsPage.Name = "PageIns";
      gpsPage.TextEnum = MySpaceTexts.TerminalTab_GPS;
      gpsPage.TextScale = 0.7005405f;
      float num1 = 0.01f;
      float num2 = 0.01f;
      Vector2 vector2_1 = new Vector2(0.29f, 0.052f);
      Vector2 vector2_2 = new Vector2(0.13f, 0.04f);
      float num3 = -0.4625f;
      float y1 = -0.325f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 1f / 500f);
      gpsPage.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(-0.442f, -0.267f);
      myGuiControlLabel1.Name = "GpsLabel";
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.GpsScreen_GpsListLabel);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.452f, -0.272f)), new Vector2?(new Vector2(0.29f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlSearchBox controlSearchBox = new MyGuiControlSearchBox(new Vector2?(new Vector2(-0.452f, y1)), new Vector2?(new Vector2(0.29f, 0.02f)), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlSearchBox.Name = "SearchIns";
      gpsPage.Controls.Add((MyGuiControlBase) controlSearchBox);
      float num4 = y1 + (controlSearchBox.Size.Y + 0.01f + num2);
      MyGuiControlTable myGuiControlTable1 = new MyGuiControlTable();
      myGuiControlTable1.Position = new Vector2(num3 + 0.0105f, num4 + 0.044f);
      myGuiControlTable1.Size = new Vector2(0.29f, 0.5f);
      myGuiControlTable1.Name = "TableINS";
      myGuiControlTable1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlTable1.ColumnsCount = 1;
      myGuiControlTable1.VisibleRowsCount = 13;
      myGuiControlTable1.HeaderVisible = false;
      MyGuiControlTable myGuiControlTable2 = myGuiControlTable1;
      myGuiControlTable2.SetCustomColumnWidths(new float[1]
      {
        1f
      });
      float y2 = num4 + (float) ((double) myGuiControlTable2.Size.Y + (double) num2 + 0.0549999997019768);
      float x1 = num3 + 0.013f;
      Vector2? position1 = new Vector2?(new Vector2(x1, y2));
      Vector2? size1 = new Vector2?(new Vector2(140f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      Vector4? colorMask1 = new Vector4?();
      StringBuilder stringBuilder1 = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Add);
      string toolTip1 = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Add_ToolTip);
      StringBuilder text1 = stringBuilder1;
      int? buttonIndex1 = new int?();
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(position1, MyGuiControlButtonStyleEnum.Rectangular, size1, colorMask1, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip1, text1, buttonIndex: buttonIndex1);
      guiControlButton1.Name = "buttonAdd";
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      Vector2? position2 = new Vector2?(new Vector2(x1, y2 + guiControlButton2.Size.Y + num2));
      Vector2? size2 = new Vector2?(new Vector2(140f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      Vector4? colorMask2 = new Vector4?();
      StringBuilder stringBuilder2 = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Delete);
      string toolTip2 = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Delete_ToolTip);
      StringBuilder text2 = stringBuilder2;
      int? buttonIndex2 = new int?();
      MyGuiControlButton guiControlButton3 = new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Rectangular, size2, colorMask2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip2, text2, buttonIndex: buttonIndex2);
      guiControlButton3.Name = "buttonDelete";
      MyGuiControlButton guiControlButton4 = guiControlButton3;
      guiControlButton4.ShowTooltipWhenDisabled = true;
      Vector2? position3 = new Vector2?(new Vector2(x1 + guiControlButton2.Size.X + num1, y2));
      Vector2? size3 = new Vector2?(new Vector2(310f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      Vector4? colorMask3 = new Vector4?();
      StringBuilder stringBuilder3 = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromCurrent);
      string toolTip3 = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewFromCurrent_ToolTip);
      StringBuilder text3 = stringBuilder3;
      int? buttonIndex3 = new int?();
      MyGuiControlButton guiControlButton5 = new MyGuiControlButton(position3, MyGuiControlButtonStyleEnum.Rectangular, size3, colorMask3, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip3, text3, buttonIndex: buttonIndex3, isAutoscaleEnabled: true);
      guiControlButton5.Name = "buttonFromCurrent";
      guiControlButton5.IsAutoEllipsisEnabled = true;
      MyGuiControlButton guiControlButton6 = guiControlButton5;
      Vector2? position4 = new Vector2?(new Vector2(x1 + guiControlButton2.Size.X + num1, y2 + guiControlButton2.Size.Y + num2));
      Vector2? size4 = new Vector2?(new Vector2(310f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      Vector4? colorMask4 = new Vector4?();
      StringBuilder stringBuilder4 = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromClipboard);
      string toolTip4 = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewFromClipboard_ToolTip);
      StringBuilder text4 = stringBuilder4;
      int? buttonIndex4 = new int?();
      MyGuiControlButton guiControlButton7 = new MyGuiControlButton(position4, MyGuiControlButtonStyleEnum.Rectangular, size4, colorMask4, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip4, text4, buttonIndex: buttonIndex4, isAutoscaleEnabled: true);
      guiControlButton7.Name = "buttonFromClipboard";
      guiControlButton7.IsAutoEllipsisEnabled = true;
      MyGuiControlButton guiControlButton8 = guiControlButton7;
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlTable2);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlButton2);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlButton4);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlButton6);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlButton8);
      float num5 = -0.15f;
      float num6 = -0.345f;
      float num7 = num5 + num1;
      float y3 = num6 + num2;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.125f, y3)), new Vector2?(new Vector2(0.4f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel3.Name = "labelInsName";
      myGuiControlLabel3.Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Name).ToString();
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      float y4 = y3 + (myGuiControlLabel4.Size.Y + num2);
      MyGuiControlTextbox guiControlTextbox1 = new MyGuiControlTextbox(maxLength: 32);
      guiControlTextbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlTextbox1.Position = new Vector2(-0.125f, y4);
      guiControlTextbox1.Size = new Vector2(0.58f, 0.035f);
      guiControlTextbox1.Name = "panelInsName";
      MyGuiControlTextbox control1 = guiControlTextbox1;
      control1.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Name_ToolTip));
      float y5 = y4 + (control1.Size.Y + 2f * num2);
      Vector2 vector2_3 = control1.Size - new Vector2(0.14f, 0.01f);
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.125f, y5)), new Vector2?(new Vector2(0.288f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel5.Name = "labelInsDesc";
      myGuiControlLabel5.Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Description).ToString();
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      float y6 = y5 + (float) ((double) myGuiControlLabel6.Size.Y * 0.5 + 2.0 * (double) num2);
      MyGuiControlMultilineEditableText multilineEditableText = new MyGuiControlMultilineEditableText();
      multilineEditableText.Position = new Vector2(-0.125f, y6);
      multilineEditableText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      multilineEditableText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      multilineEditableText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      multilineEditableText.Name = "textInsDesc";
      multilineEditableText.Size = new Vector2(0.58f, 0.271f);
      multilineEditableText.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER;
      multilineEditableText.TextPadding = new MyGuiBorderThickness(3f / 500f);
      MyGuiControlMultilineEditableText control2 = multilineEditableText;
      control2.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Desc_ToolTip));
      float y7 = y6 + (control2.Size.Y + num2);
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.125f, y7)), new Vector2?(new Vector2(0.4f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel7.Name = "gpsColorLabel";
      myGuiControlLabel7.Text = MyTexts.GetString(MySpaceTexts.BlockPropertyTitle_FontColor) + ":";
      MyGuiControlLabel myGuiControlLabel8 = myGuiControlLabel7;
      float y8 = y7 + (myGuiControlLabel8.Size.Y + num2);
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.125f, y8)), new Vector2?(new Vector2(0.4f, 0.035f)));
      myGuiControlLabel9.Name = "gpsHueLabel";
      myGuiControlLabel9.Text = MyTexts.GetString(MySpaceTexts.GPSScreen_hueLabel);
      MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
      MyGuiControlSlider guiControlSlider1 = new MyGuiControlSlider(new Vector2?(new Vector2((float) ((double) myGuiControlLabel10.PositionX + (double) myGuiControlLabel10.Size.X + (double) num2 + 1.0 / 500.0), y8)), maxValue: 360f, width: 0.18f, labelText: string.Empty, labelDecimalPlaces: 0, toolTip: "", visualStyle: MyGuiControlSliderStyleEnum.Hue, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlSlider1.Name = "gpsHueSlider";
      MyGuiControlSlider guiControlSlider2 = guiControlSlider1;
      guiControlSlider2.Size = new Vector2((float) ((0.59799998998642 - (double) num1) / 3.0 - 2.0 * (double) num1 - (double) myGuiControlLabel10.Size.X + 1.0 / 1000.0), 0.035f);
      guiControlSlider2.PositionY += guiControlSlider2.Size.Y - myGuiControlLabel10.Size.Y;
      myGuiControlLabel10.PositionY = guiControlSlider2.PositionY;
      MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(new Vector2?(new Vector2(guiControlSlider2.PositionX + guiControlSlider2.Size.X + num2, myGuiControlLabel10.PositionY)), new Vector2?(new Vector2(0.4f, 0.035f)));
      myGuiControlLabel11.Name = "gpsSaturationLabel";
      myGuiControlLabel11.Text = MyTexts.GetString(MySpaceTexts.GPSScreen_saturationLabel);
      MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
      MyGuiControlSlider guiControlSlider3 = new MyGuiControlSlider(new Vector2?(new Vector2((float) ((double) myGuiControlLabel12.PositionX + (double) myGuiControlLabel12.Size.X + (double) num2 - 1.0 / 1000.0), guiControlSlider2.PositionY)), width: guiControlSlider2.Size.X, labelText: string.Empty, labelDecimalPlaces: 0, toolTip: "", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlSlider3.Name = "gpsSaturationSlider";
      MyGuiControlSlider guiControlSlider4 = guiControlSlider3;
      MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel(new Vector2?(new Vector2(guiControlSlider4.PositionX + guiControlSlider4.Size.X + num2, myGuiControlLabel12.PositionY)), new Vector2?(new Vector2(0.4f, 0.035f)));
      myGuiControlLabel13.Name = "gpsValueLabel";
      myGuiControlLabel13.Text = MyTexts.GetString(MySpaceTexts.GPSScreen_valueLabel);
      MyGuiControlLabel myGuiControlLabel14 = myGuiControlLabel13;
      MyGuiControlSlider guiControlSlider5 = new MyGuiControlSlider(new Vector2?(new Vector2((float) ((double) myGuiControlLabel14.PositionX + (double) myGuiControlLabel14.Size.X + (double) num2 - 1.0 / 500.0), guiControlSlider4.PositionY)), width: guiControlSlider2.Size.X, labelText: string.Empty, labelDecimalPlaces: 0, toolTip: "", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlSlider5.Name = "gpsValueSlider";
      MyGuiControlSlider guiControlSlider6 = guiControlSlider5;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel(new Vector2?(new Vector2(guiControlSlider6.PositionX - 1f / 1000f, guiControlSlider6.PositionY + guiControlSlider6.Size.Y)), new Vector2?(new Vector2(0.4f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      myGuiControlLabel15.Name = "gpsHexLabel";
      myGuiControlLabel15.Text = MyTexts.GetString(MySpaceTexts.GPSScreen_hexLabel);
      MyGuiControlLabel myGuiControlLabel16 = myGuiControlLabel15;
      MyGuiControlTextbox guiControlTextbox2 = new MyGuiControlTextbox();
      guiControlTextbox2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlTextbox2.Position = new Vector2(guiControlSlider6.PositionX + 1f / 1000f, guiControlSlider6.PositionY + guiControlSlider6.Size.Y);
      guiControlTextbox2.Size = new Vector2(guiControlSlider6.Size.X - 1f / 500f, guiControlSlider6.Size.Y);
      guiControlTextbox2.Name = "gpsColorHexTextbox";
      MyGuiControlTextbox control3 = guiControlTextbox2;
      control3.SetTooltip(MyTexts.GetString(MySpaceTexts.GPSScreen_hexTooltip));
      float y9 = y8 + (guiControlSlider2.Size.Y + num2 * 4f);
      MyGuiControlLabel myGuiControlLabel17 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.125f, y9)), new Vector2?(new Vector2(0.4f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel17.Name = "labelInsCoordinates";
      myGuiControlLabel17.Text = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Coordinates);
      MyGuiControlLabel myGuiControlLabel18 = myGuiControlLabel17;
      float y10 = y9 + (myGuiControlLabel18.Size.Y + 3f * num2);
      MyGuiControlLabel myGuiControlLabel19 = new MyGuiControlLabel(new Vector2?(new Vector2(num7 + 0.017f, y10)), new Vector2?(new Vector2(0.01f, 0.035f)), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_X).ToString());
      myGuiControlLabel19.Name = "labelInsX";
      MyGuiControlLabel myGuiControlLabel20 = myGuiControlLabel19;
      float num8 = num7 + (myGuiControlLabel20.Size.X + num1);
      MyGuiControlTextbox guiControlTextbox3 = new MyGuiControlTextbox();
      guiControlTextbox3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlTextbox3.Position = new Vector2(num8 + 0.017f, y10);
      guiControlTextbox3.Size = new Vector2((float) ((0.59799998998642 - (double) num1) / 3.0 - 2.0 * (double) num1) - myGuiControlLabel20.Size.X, 0.035f);
      guiControlTextbox3.Name = "textInsX";
      MyGuiControlTextbox control4 = guiControlTextbox3;
      control4.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_X_ToolTip));
      float num9 = num8 + (control4.Size.X + num1);
      MyGuiControlLabel myGuiControlLabel21 = new MyGuiControlLabel(new Vector2?(new Vector2(num9 + 0.017f, y10)), new Vector2?(new Vector2(0.586f, 0.035f)), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Y).ToString());
      myGuiControlLabel21.Name = "labelInsY";
      MyGuiControlLabel myGuiControlLabel22 = myGuiControlLabel21;
      float num10 = num9 + (myGuiControlLabel20.Size.X + num1);
      MyGuiControlTextbox guiControlTextbox4 = new MyGuiControlTextbox();
      guiControlTextbox4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlTextbox4.Position = new Vector2(num10 + 0.017f, y10);
      guiControlTextbox4.Size = new Vector2((float) ((0.59799998998642 - (double) num1) / 3.0 - 2.0 * (double) num1) - myGuiControlLabel20.Size.X, 0.035f);
      guiControlTextbox4.Name = "textInsY";
      MyGuiControlTextbox control5 = guiControlTextbox4;
      control5.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Y_ToolTip));
      float num11 = num10 + (control5.Size.X + num1);
      MyGuiControlLabel myGuiControlLabel23 = new MyGuiControlLabel(new Vector2?(new Vector2(num11 + 0.017f, y10)), new Vector2?(new Vector2(0.01f, 0.035f)), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Z).ToString());
      myGuiControlLabel23.Name = "labelInsZ";
      MyGuiControlLabel myGuiControlLabel24 = myGuiControlLabel23;
      float num12 = num11 + (myGuiControlLabel20.Size.X + num1);
      MyGuiControlTextbox guiControlTextbox5 = new MyGuiControlTextbox();
      guiControlTextbox5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlTextbox5.Position = new Vector2(num12 + 0.017f, y10);
      guiControlTextbox5.Size = new Vector2((float) ((0.59799998998642 - (double) num1) / 3.0 - 2.0 * (double) num1) - myGuiControlLabel20.Size.X, 0.035f);
      guiControlTextbox5.Name = "textInsZ";
      MyGuiControlTextbox control6 = guiControlTextbox5;
      control6.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Z_ToolTip));
      Vector2? position5 = new Vector2?(new Vector2(control6.PositionX + 1f / 500f, control6.PositionY + 4f * num1 + control6.Size.Y));
      Vector2? size5 = new Vector2?(new Vector2(300f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      Vector4? colorMask5 = new Vector4?();
      StringBuilder stringBuilder5 = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_CopyToClipboard);
      string toolTip5 = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_CopyToClipboard_ToolTip);
      StringBuilder text5 = stringBuilder5;
      int? buttonIndex5 = new int?();
      MyGuiControlButton guiControlButton9 = new MyGuiControlButton(position5, MyGuiControlButtonStyleEnum.Rectangular, size5, colorMask5, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, toolTip5, text5, buttonIndex: buttonIndex5, isAutoscaleEnabled: true);
      guiControlButton9.Name = "buttonToClipboard";
      guiControlButton9.IsAutoEllipsisEnabled = true;
      MyGuiControlButton guiControlButton10 = guiControlButton9;
      float y11 = guiControlButton6.PositionY + guiControlButton6.Size.Y / 2f;
      float x2 = num1 - 0.135f;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(new Vector2?(new Vector2(x2, y11)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlCheckbox1.Name = "checkInsShowOnHud";
      MyGuiControlCheckbox guiControlCheckbox2 = guiControlCheckbox1;
      MyGuiControlLabel myGuiControlLabel25 = new MyGuiControlLabel(new Vector2?(new Vector2(x2 + (guiControlCheckbox2.Size.X + num1), y11)), new Vector2?(guiControlCheckbox2.Size - new Vector2(0.01f, 0.01f)));
      myGuiControlLabel25.Name = "labelInsShowOnHud";
      myGuiControlLabel25.Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_ShowOnHud).ToString();
      MyGuiControlLabel myGuiControlLabel26 = myGuiControlLabel25;
      guiControlButton10.Size = new Vector2(control6.Size.X, guiControlButton10.Size.Y);
      float y12 = guiControlButton8.PositionY + guiControlButton8.Size.Y / 2f;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(new Vector2?(new Vector2(x2, y12)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlCheckbox3.Name = "checkInsAlwaysVisible";
      MyGuiControlCheckbox guiControlCheckbox4 = guiControlCheckbox3;
      guiControlCheckbox4.SetToolTip(MySpaceTexts.TerminalTab_GPS_AlwaysVisible_Tooltip);
      MyGuiControlLabel myGuiControlLabel27 = new MyGuiControlLabel(new Vector2?(new Vector2(x2 + guiControlCheckbox2.Size.X + num1, y12)), new Vector2?(guiControlCheckbox2.Size - new Vector2(0.01f, 0.01f)));
      myGuiControlLabel27.Name = "labelInsAlwaysVisible";
      myGuiControlLabel27.Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_AlwaysVisible).ToString();
      MyGuiControlLabel myGuiControlLabel28 = myGuiControlLabel27;
      MyGuiControlLabel myGuiControlLabel29 = new MyGuiControlLabel(new Vector2?(new Vector2(0.456f, y12)), new Vector2?(guiControlCheckbox2.Size - new Vector2(0.01f, 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      myGuiControlLabel29.Name = "labelClipboardGamepadHelp";
      myGuiControlLabel29.TextEnum = MyStringId.NullOrEmpty;
      MyGuiControlLabel myGuiControlLabel30 = myGuiControlLabel29;
      float y13 = y12 + guiControlCheckbox2.Size.Y;
      MyGuiControlLabel myGuiControlLabel31 = new MyGuiControlLabel(new Vector2?(new Vector2(x2 + num1, y13)), new Vector2?(new Vector2(0.288f, 0.035f)));
      myGuiControlLabel31.Name = "TerminalTab_GPS_SaveWarning";
      myGuiControlLabel31.Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_SaveWarning).ToString();
      myGuiControlLabel31.ColorMask = Color.Red.ToVector4();
      MyGuiControlLabel myGuiControlLabel32 = myGuiControlLabel31;
      gpsPage.Controls.Add((MyGuiControlBase) control1);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      gpsPage.Controls.Add((MyGuiControlBase) control2);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlSlider2);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlSlider4);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlSlider6);
      gpsPage.Controls.Add((MyGuiControlBase) control3);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel18);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel20);
      gpsPage.Controls.Add((MyGuiControlBase) control4);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel22);
      gpsPage.Controls.Add((MyGuiControlBase) control5);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel24);
      gpsPage.Controls.Add((MyGuiControlBase) control6);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlButton10);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlCheckbox2);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel26);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel32);
      gpsPage.Controls.Add((MyGuiControlBase) guiControlCheckbox4);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel28);
      gpsPage.Controls.Add((MyGuiControlBase) myGuiControlLabel30);
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Gps] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Gps] = (MyGuiControlBase) controlSearchBox.TextBox;
    }

    private void CreatePropertiesPageControls(
      MyGuiControlParent menuParent,
      MyGuiControlParent panelParent)
    {
      this.m_propertiesTableParent.Name = "PropertiesTable";
      this.m_propertiesTopMenuParent.Name = "PropertiesTopMenu";
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox();
      guiControlCombobox1.Position = new Vector2(0.0f, 0.0f);
      guiControlCombobox1.Size = new Vector2(0.25f, 0.1f);
      guiControlCombobox1.Name = "ShipsInRange";
      guiControlCombobox1.Visible = false;
      guiControlCombobox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlCombobox guiControlCombobox2 = guiControlCombobox1;
      guiControlCombobox2.SetToolTip(MySpaceTexts.ScreenTerminal_ShipCombobox);
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.Position = new Vector2(0.258f, 0.004f);
      guiControlButton.Size = new Vector2(0.2f, 0.05f);
      guiControlButton.Name = "SelectShip";
      guiControlButton.Text = MyTexts.GetString(MySpaceTexts.Terminal_RemoteControl_Button);
      guiControlButton.TextScale = 0.7005405f;
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Small;
      this.m_selectShipButton = guiControlButton;
      this.m_selectShipButton.SetToolTip(MySpaceTexts.ScreenTerminal_ShipList);
      menuParent.Controls.Add((MyGuiControlBase) guiControlCombobox2);
      menuParent.Controls.Add((MyGuiControlBase) this.m_selectShipButton);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddVertical(new Vector2(0.164f, -0.31f), 0.675f, 1f / 500f);
      panelParent.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(new Vector2?(), new Vector2?(), new Vector4?(), "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, (StringBuilder) null, true, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, new int?(), false, false, (MyGuiCompositeTexture) null, new MyGuiBorderThickness?());
      controlMultilineText1.Name = "InfoHelpMultilineText";
      controlMultilineText1.Position = new Vector2(0.186f, -0.31f);
      controlMultilineText1.Size = new Vector2(0.3f, 0.68f);
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.Text = new StringBuilder(MyTexts.GetString(MySpaceTexts.RemoteAccess_Description));
      MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
      panelParent.Controls.Add((MyGuiControlBase) controlMultilineText2);
      MyGuiControlTable myGuiControlTable1 = new MyGuiControlTable();
      myGuiControlTable1.Position = new Vector2(-0.142f, -0.31f);
      myGuiControlTable1.Size = new Vector2(0.582f, 0.88f);
      myGuiControlTable1.Name = "ShipsData";
      myGuiControlTable1.ColumnsCount = 5;
      myGuiControlTable1.VisibleRowsCount = 19;
      myGuiControlTable1.HeaderVisible = true;
      myGuiControlTable1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      MyGuiControlTable myGuiControlTable2 = myGuiControlTable1;
      myGuiControlTable2.SetCustomColumnWidths(new float[5]
      {
        0.263f,
        0.15f,
        0.15f,
        0.22f,
        0.224f
      });
      myGuiControlTable2.SetColumnName(0, MyTexts.Get(MySpaceTexts.TerminalName));
      myGuiControlTable2.SetColumnName(3, MyTexts.Get(MySpaceTexts.TerminalControl));
      myGuiControlTable2.SetColumnName(1, MyTexts.Get(MySpaceTexts.TerminalDistance));
      myGuiControlTable2.SetColumnName(2, MyTexts.Get(MySpaceTexts.TerminalStatus));
      myGuiControlTable2.SetColumnName(4, MyTexts.Get(MySpaceTexts.TerminalAccess));
      myGuiControlTable2.SetColumnComparison(0, (Comparison<MyGuiControlTable.Cell>) ((a, b) => a.Text.CompareTo(b.Text)));
      myGuiControlTable2.SetColumnComparison(1, (Comparison<MyGuiControlTable.Cell>) ((a, b) => ((double) a.UserData).CompareTo((double) b.UserData)));
      panelParent.Controls.Add((MyGuiControlBase) myGuiControlTable2);
      panelParent.Visible = false;
      this.m_defaultFocusedControlKeyboard[MyTerminalPageEnum.Properties] = this.m_defaultFocusedControlGamepad[MyTerminalPageEnum.Properties] = (MyGuiControlBase) null;
    }

    public override bool Draw()
    {
      MyTerminalController terminalController;
      if (this.m_terminalControllers.TryGetValue((MyTerminalPageEnum) this.m_terminalTabs.SelectedPage, out terminalController))
        terminalController.UpdateBeforeDraw((MyGuiScreenBase) this);
      return base.Draw();
    }

    protected override void OnClosed()
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
      MyGuiScreenTerminal.m_interactedEntity = (MyEntity) null;
      MyGuiScreenTerminal.m_closeHandler = (Action<MyEntity>) null;
      if (MyFakes.ENABLE_GPS)
        this.m_controllerGps.Close();
      this.m_controllerControlPanel.Close();
      if (this.m_controllerInventory != null)
        this.m_controllerInventory.Close();
      this.m_controllerProduction.Close();
      this.m_controllerInfo.Close();
      this.Controls.Clear();
      this.m_terminalTabs = (MyGuiControlTabControl) null;
      this.m_controllerInventory = (MyTerminalInventoryController) null;
      if (MyFakes.SHOW_FACTIONS_GUI)
        this.m_controllerFactions.Close();
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
      {
        this.m_controllerProperties.Close();
        this.m_controllerProperties.ButtonClicked -= new Action(this.PropertiesButtonClicked);
        this.m_propertiesTableParent = (MyGuiControlParent) null;
        this.m_propertiesTopMenuParent = (MyGuiControlParent) null;
      }
      if (MyFakes.ENABLE_COMMUNICATION)
        this.m_controllerChat.Close();
      if (this.m_requestedEntities.Count > 0)
        MyMultiplayer.GetReplicationClient().OnReplicableReady -= new Action<IMyReplicable>(this.InvokeWhenLoaded);
      foreach (KeyValuePair<long, Action<long>> requestedEntity in this.m_requestedEntities)
        MyMultiplayer.GetReplicationClient()?.RequestReplicable(requestedEntity.Key, (byte) 0, false);
      this.m_requestedEntities.Clear();
      MyGuiScreenTerminal.m_openInventoryInteractedEntity = (MyEntity) null;
      MyGuiScreenTerminal.m_instance = (MyGuiScreenTerminal) null;
      MyGuiScreenTerminal.m_screenOpen = false;
      base.OnClosed();
    }

    private void InfoButton_OnButtonClick(MyGuiControlButton sender) => MyGuiSandbox.OpenUrlWithFallback(MySteamConstants.URL_HELP_TERMINAL_SCREEN, "Steam Guide");

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      int num = this.FocusedControl is MyGuiControlTextbox ? 1 : 0;
      if (num == 0 && (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TERMINAL) || MyInput.Static.IsNewGameControlPressed(MyControlsSpace.USE)))
      {
        MyGuiSoundManager.PlaySound(this.m_closingCueEnum.HasValue ? this.m_closingCueEnum.Value : GuiSounds.MouseClick);
        this.CloseScreen(false);
      }
      if (num == 0 && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.INVENTORY))
      {
        if (this.m_terminalTabs.SelectedPage == 0)
        {
          MyGuiSoundManager.PlaySound(this.m_closingCueEnum.HasValue ? this.m_closingCueEnum.Value : GuiSounds.MouseClick);
          this.CloseScreen(false);
        }
        else
          MyGuiScreenTerminal.SwitchToInventory();
      }
      if (num == 0 && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PAUSE_GAME))
        MySandboxGame.PauseToggle();
      if (num == 0 && MyInput.Static.IsAnyCtrlKeyPressed() && (MyInput.Static.IsKeyPress(MyKeys.A) && this.m_terminalTabs.SelectedPage == 1))
        this.m_controllerControlPanel.SelectAllBlocks();
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
    }

    public void PropertiesButtonClicked()
    {
      this.m_terminalTabs.SelectedPage = -1;
      this.m_controllerProperties.Refresh();
      this.m_propertiesTableParent.Visible = true;
    }

    public void Info_ShipRenamed() => this.m_controllerProperties.Refresh();

    private MyGuiControlBase GetDefaultControl(MyTerminalPageEnum page) => !MyInput.Static.IsJoystickLastUsed ? this.m_defaultFocusedControlKeyboard[page] : this.m_defaultFocusedControlGamepad[page];

    public void tabs_OnPageChanged()
    {
      if (MyVRage.Platform.ImeProcessor != null)
        MyVRage.Platform.ImeProcessor.Deactivate();
      MyTerminalPageEnum selectedPage = (MyTerminalPageEnum) this.m_terminalTabs.SelectedPage;
      if (this.m_propertiesTableParent.Visible && selectedPage != MyTerminalPageEnum.Properties)
        this.m_propertiesTableParent.Visible = false;
      if (selectedPage == MyTerminalPageEnum.Inventory && this.m_controllerInventory != null)
        this.m_controllerInventory.Refresh();
      if (selectedPage == MyTerminalPageEnum.Info)
        this.m_controllerInfo?.MarkControlsDirty();
      MyTerminalController terminalController;
      if (this.m_terminalControllers.TryGetValue(selectedPage, out terminalController))
        terminalController.InvalidateBeforeDraw();
      MyCubeGrid myCubeGrid = MyGuiScreenTerminal.InteractedEntity != null ? MyGuiScreenTerminal.InteractedEntity.Parent as MyCubeGrid : (MyCubeGrid) null;
      this.GamepadHelpText = string.Empty;
      switch (selectedPage)
      {
        case MyTerminalPageEnum.Properties:
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.TerminalProperties_Help_Screen);
          this.FocusedControl = this.GetDefaultControl(selectedPage);
          break;
        case MyTerminalPageEnum.Inventory:
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.TerminalInventory_Help_Screen);
          this.FocusedControl = (MyGuiControlBase) this.m_controllerInventory.GetDefaultFocus();
          break;
        case MyTerminalPageEnum.ControlPanel:
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.TerminalControlPanel_Help_Screen);
          if (myCubeGrid != null)
          {
            this.FocusedControl = this.GetDefaultControl(selectedPage);
            break;
          }
          this.FocusedControl = (MyGuiControlBase) this.m_selectShipButton;
          break;
        case MyTerminalPageEnum.Production:
          if (myCubeGrid != null && this.m_assemblersCombobox.GetItemsCount() != 0)
          {
            this.FocusedControl = this.GetDefaultControl(selectedPage);
            break;
          }
          this.FocusedControl = (MyGuiControlBase) this.m_selectShipButton;
          break;
        case MyTerminalPageEnum.Info:
          this.FocusedControl = this.GetDefaultControl(selectedPage);
          break;
        case MyTerminalPageEnum.Factions:
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.TerminalFactions_Help_Screen);
          this.FocusedControl = this.GetDefaultControl(selectedPage);
          break;
        case MyTerminalPageEnum.Comms:
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.TerminalComms_Help_Screen);
          this.FocusedControl = this.GetDefaultControl(selectedPage);
          break;
        case MyTerminalPageEnum.Gps:
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.TerminalGps_Help_Screen);
          this.FocusedControl = this.GetDefaultControl(selectedPage);
          break;
      }
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (this.m_terminalTabs == null)
        return;
      MyTerminalController terminalController;
      if (this.m_terminalControllers.TryGetValue((MyTerminalPageEnum) this.m_terminalTabs.SelectedPage, out terminalController))
        terminalController.HandleInput();
      if (this.m_terminalTabs.SelectedPage == -1)
        this.m_controllerProperties.HandleInput();
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public static void Show(
      MyTerminalPageEnum page,
      MyCharacter user,
      MyEntity interactedEntity,
      bool isRemote = false)
    {
      if (!MyPerGameSettings.TerminalEnabled || !MyPerGameSettings.GUI.EnableTerminalScreen)
        return;
      bool flag = MyInput.Static.IsAnyShiftKeyPressed();
      if (MyGuiScreenTerminal.m_instance != null)
        return;
      MyGuiScreenTerminal.m_instance = new MyGuiScreenTerminal();
      MyGuiScreenTerminal.m_instance.m_user = user;
      MyGuiScreenTerminal.IsRemote = isRemote;
      MyGuiScreenTerminal.m_openInventoryInteractedEntity = interactedEntity;
      MyGuiScreenTerminal.m_instance.m_initialPage = !MyFakes.ENABLE_TERMINAL_PROPERTIES ? page : (flag ? MyTerminalPageEnum.Properties : page);
      MyGuiScreenTerminal.InteractedEntity = interactedEntity;
      MyGuiScreenTerminal.m_instance.RecreateControls(true);
      MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) MyGuiScreenTerminal.m_instance);
      MyGuiScreenTerminal.m_screenOpen = true;
      if (interactedEntity == null)
        return;
      string name = interactedEntity.GetType().Name;
    }

    internal static void Hide()
    {
      if (MyGuiScreenTerminal.m_instance == null)
        return;
      MyGuiScreenTerminal.m_instance.CloseScreen(false);
    }

    public static void ChangeInteractedEntity(MyEntity interactedEntity, bool isRemote)
    {
      MyGuiScreenTerminal.IsRemote = isRemote;
      MyGuiScreenTerminal.InteractedEntity = interactedEntity;
    }

    public static MyGuiControlLabel CreateErrorLabel(MyStringId text, string name)
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: MyTexts.GetString(text), textScale: 1.2f, font: "Red", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      myGuiControlLabel.Name = name;
      myGuiControlLabel.Visible = true;
      return myGuiControlLabel;
    }

    public static void SwitchToControlPanelBlock(MyTerminalBlock block)
    {
      MyGuiScreenTerminal.m_instance.m_terminalTabs.SelectedPage = 1;
      MyGuiScreenTerminal.m_instance.m_controllerControlPanel.SelectBlocks(new MyTerminalBlock[1]
      {
        block
      });
    }

    public static void SwitchToInventory(MyTerminalBlock block = null)
    {
      MyGuiScreenTerminal.m_instance.m_terminalTabs.SelectedPage = 0;
      if (MyGuiScreenTerminal.m_instance.m_controllerInventory == null || MyGuiScreenTerminal.m_interactedEntity == block || block == null)
        return;
      MyGuiScreenTerminal.m_instance.m_controllerInventory.SetSearch(block.DisplayNameText);
    }

    public override bool Update(bool hasFocus)
    {
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
      {
        if (this.m_connected && this.m_terminalTabs.SelectedPage != -1 && !this.m_controllerProperties.TestConnection())
        {
          this.m_connected = false;
          this.ShowDisconnectScreen();
        }
        else if (!this.m_connected && this.m_controllerProperties.TestConnection())
        {
          this.m_connected = true;
          this.ShowConnectScreen();
        }
        this.m_controllerProperties.Update(this.m_terminalTabs.SelectedPage == -1);
        if (MyFakes.ENABLE_COMMUNICATION)
          this.m_controllerChat.Update();
      }
      MyCubeGrid grid = MyGuiScreenTerminal.InteractedEntity == null || MyGuiScreenTerminal.InteractedEntity.Closed ? (MyCubeGrid) null : MyGuiScreenTerminal.InteractedEntity.Parent as MyCubeGrid;
      if (grid != null && grid.GridSystems.TerminalSystem != this.m_controllerControlPanel.TerminalSystem)
      {
        if (this.m_controllerControlPanel != null)
        {
          this.m_controllerControlPanel.Close();
          this.m_controllerControlPanel.Init((IMyGuiControlsParent) this.m_terminalTabs.Controls.GetControlByName("PageControlPanel"), MySession.Static.LocalHumanPlayer, grid, MyGuiScreenTerminal.InteractedEntity as MyTerminalBlock, this.m_colorHelper);
        }
        if (this.m_controllerProduction != null)
        {
          this.m_controllerProduction.Close();
          this.m_controllerProduction.Init((IMyGuiControlsParent) this.m_terminalTabs.GetTabSubControl(2), grid, MyGuiScreenTerminal.InteractedEntity as MyCubeBlock);
        }
        if (this.m_controllerInventory != null)
        {
          this.m_controllerInventory.Close();
          this.m_controllerInventory.Init((IMyGuiControlsParent) this.m_terminalTabs.Controls.GetControlByName("PageInventory"), (MyEntity) this.m_user, MyGuiScreenTerminal.InteractedEntity, this.m_colorHelper, (MyGuiScreenBase) this);
        }
      }
      this.m_controllerFactions.Update();
      return base.Update(hasFocus);
    }

    public void ShowDisconnectScreen()
    {
      this.m_terminalTabs.Visible = false;
      this.m_propertiesTableParent.Visible = false;
      this.m_terminalNotConnected.Visible = true;
    }

    public void ShowConnectScreen()
    {
      this.m_terminalTabs.Visible = true;
      this.m_propertiesTableParent.Visible = this.m_terminalTabs.SelectedPage == -1;
      this.m_terminalNotConnected.Visible = false;
    }

    private void InvokeWhenLoaded(IMyReplicable replicable)
    {
      MyCubeGridReplicable cubeGridReplicable = replicable as MyCubeGridReplicable;
      MyTerminalReplicable terminalReplicable = replicable as MyTerminalReplicable;
      long entityId;
      if (cubeGridReplicable != null)
      {
        entityId = cubeGridReplicable.Instance.EntityId;
      }
      else
      {
        if (terminalReplicable == null)
          return;
        entityId = terminalReplicable.Instance.EntityId;
      }
      foreach (KeyValuePair<long, Action<long>> requestedEntity in this.m_requestedEntities)
      {
        if (requestedEntity.Value != null && requestedEntity.Key == entityId)
          requestedEntity.Value(entityId);
      }
    }

    public static void RequestReplicable(long requestedId, long waitForId, Action<long> loadAction)
    {
      MyReplicationClient replicationClient = MyMultiplayer.GetReplicationClient();
      if (replicationClient == null || MyGuiScreenTerminal.m_instance == null || MyGuiScreenTerminal.m_instance.m_requestedEntities.ContainsKey(requestedId))
        return;
      replicationClient.RequestReplicable(requestedId, (byte) 0, true);
      if (MyGuiScreenTerminal.m_instance.m_requestedEntities.Count == 0)
        MyMultiplayer.GetReplicationClient().OnReplicableReady += new Action<IMyReplicable>(MyGuiScreenTerminal.m_instance.InvokeWhenLoaded);
      MyGuiScreenTerminal.m_instance.m_requestedEntities.Add(requestedId, requestedId == waitForId ? loadAction : (Action<long>) null);
      if (requestedId == waitForId || MyGuiScreenTerminal.m_instance.m_requestedEntities.ContainsKey(waitForId))
        return;
      MyGuiScreenTerminal.m_instance.m_requestedEntities.Add(waitForId, loadAction);
    }

    public static MyTerminalPageEnum GetCurrentScreen() => MyGuiScreenTerminal.IsOpen ? (MyTerminalPageEnum) MyGuiScreenTerminal.m_instance.m_terminalTabs.SelectedPage : MyTerminalPageEnum.None;
  }
}
