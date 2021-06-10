// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyTerminalBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_TerminalBlock))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyTerminalBlock), typeof (Sandbox.ModAPI.Ingame.IMyTerminalBlock)})]
  public class MyTerminalBlock : MySyncedBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock
  {
    private static readonly Guid m_storageGuid = new Guid("74DE02B3-27F9-4960-B1C4-27351F2B06D1");
    private const int DATA_CHARACTER_LIMIT = 64000;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_showOnHUD;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_showInTerminal;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_showInToolbarConfig;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_showInInventory;
    private bool m_isBeingHackedPrevValue;
    private MyGuiScreenTextPanel m_textBox;
    protected bool m_textboxOpen;
    private ulong m_currentUser;
    public int? HackAttemptTime;
    public bool IsAccessibleForProgrammableBlock = true;
    private bool m_detailedInfoDirty;
    private readonly StringBuilder m_detailedInfo = new StringBuilder();
    private static FastResourceLock m_createControlsLock = new FastResourceLock();

    public StringBuilder CustomName { get; private set; }

    public StringBuilder CustomNameWithFaction { get; private set; }

    public string CustomData
    {
      get
      {
        string str;
        return this.Storage == null || !this.Storage.TryGetValue(MyTerminalBlock.m_storageGuid, out str) ? string.Empty : str;
      }
      set => this.SetCustomData_Internal(value, true);
    }

    private void SetCustomData_Internal(string value, bool sync)
    {
      if (this.Storage == null)
      {
        this.Storage = (MyModStorageComponentBase) new MyModStorageComponent();
        this.Components.Add<MyModStorageComponentBase>(this.Storage);
      }
      if (value.Length > 64000)
        value = value.Substring(0, 64000);
      string str;
      if (this.Storage.TryGetValue(MyTerminalBlock.m_storageGuid, out str) && !(str != value))
        return;
      this.Storage[MyTerminalBlock.m_storageGuid] = value;
      if (sync)
      {
        this.RaiseCustomDataChanged();
      }
      else
      {
        Action<MyTerminalBlock> customDataChanged = this.CustomDataChanged;
        if (customDataChanged == null)
          return;
        customDataChanged(this);
      }
    }

    public bool ShowOnHUD
    {
      get => (bool) this.m_showOnHUD;
      set
      {
        if ((bool) this.m_showOnHUD == value || !this.CanShowOnHud)
          return;
        this.m_showOnHUD.Value = value;
        this.RaiseShowOnHUDChanged();
      }
    }

    public bool ShowInTerminal
    {
      get => (bool) this.m_showInTerminal;
      set
      {
        if ((bool) this.m_showInTerminal == value)
          return;
        this.m_showInTerminal.Value = value;
        this.RaiseShowInTerminalChanged();
      }
    }

    public bool ShowInInventory
    {
      get => (bool) this.m_showInInventory;
      set
      {
        if ((bool) this.m_showInInventory == value)
          return;
        this.m_showInInventory.Value = value;
        this.RaiseShowInInventoryChanged();
      }
    }

    public bool ShowInToolbarConfig
    {
      get => (bool) this.m_showInToolbarConfig;
      set
      {
        if ((bool) this.m_showInToolbarConfig == value)
          return;
        this.m_showInToolbarConfig.Value = value;
        this.RaiseShowInToolbarConfigChanged();
      }
    }

    public new bool IsBeingHacked
    {
      get
      {
        if (!this.HackAttemptTime.HasValue)
          return false;
        bool flag = MySandboxGame.TotalSimulationTimeInMilliseconds - this.HackAttemptTime.Value < 1000;
        if (flag != this.m_isBeingHackedPrevValue)
        {
          this.m_isBeingHackedPrevValue = flag;
          this.RaiseIsBeingHackedChanged();
        }
        return flag;
      }
    }

    public StringBuilder DetailedInfo
    {
      get
      {
        if (this.m_detailedInfoDirty)
        {
          this.m_detailedInfoDirty = false;
          this.UpdateDetailedInfo(this.m_detailedInfo);
        }
        return this.m_detailedInfo;
      }
    }

    public StringBuilder CustomInfo { get; private set; }

    public event Action<MyTerminalBlock> CustomDataChanged;

    public bool HasUnsafeValues { get; private set; }

    public event Action<MyTerminalBlock> CustomNameChanged;

    public event Action<MyTerminalBlock> PropertiesChanged;

    public event Action<MyTerminalBlock> OwnershipChanged;

    public event Action<MyTerminalBlock> VisibilityChanged;

    public event Action<MyTerminalBlock> ShowOnHUDChanged;

    public event Action<MyTerminalBlock> ShowInTerminalChanged;

    public event Action<MyTerminalBlock> ShowInIventoryChanged;

    public event Action<MyTerminalBlock> ShowInToolbarConfigChanged;

    public event Action<MyTerminalBlock> IsBeingHackedChanged;

    public event Action<MyTerminalBlock, StringBuilder> AppendingCustomInfo;

    public bool IsOpenedInTerminal { get; set; }

    protected virtual bool CanShowOnHud => true;

    public MyTerminalBlock()
    {
      using (MyTerminalBlock.m_createControlsLock.AcquireExclusiveUsing())
        this.CreateTerminalControls();
      this.CustomInfo = new StringBuilder();
      this.CustomNameWithFaction = new StringBuilder();
      this.CustomName = new StringBuilder();
      this.SyncType.PropertyChanged += (Action<SyncBase>) (sync => this.RaisePropertiesChanged());
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_TerminalBlock builderTerminalBlock = (MyObjectBuilder_TerminalBlock) objectBuilder;
      if (builderTerminalBlock.CustomName != null)
      {
        this.CustomName.Clear().Append(builderTerminalBlock.CustomName);
        this.DisplayNameText = builderTerminalBlock.CustomName;
      }
      else
        this.CustomName.Append(this.DisplayNameText);
      if (Sandbox.Game.Multiplayer.Sync.IsServer && Sandbox.Game.Multiplayer.Sync.Clients != null)
        Sandbox.Game.Multiplayer.Sync.Clients.ClientRemoved += new Action<ulong>(this.ClientRemoved);
      this.m_showOnHUD.ValueChanged += new Action<SyncBase>(this.m_showOnHUD_ValueChanged);
      this.m_showOnHUD.SetLocalValue(builderTerminalBlock.ShowOnHUD);
      this.m_showInTerminal.SetLocalValue(builderTerminalBlock.ShowInTerminal);
      this.m_showInInventory.SetLocalValue(builderTerminalBlock.ShowInInventory);
      this.m_showInToolbarConfig.SetLocalValue(builderTerminalBlock.ShowInToolbarConfig);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentTerminal(this));
    }

    private void m_showOnHUD_ValueChanged(SyncBase obj)
    {
      MyCubeGrid cubeGrid = this.CubeGrid;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (!(this is IMyControllableEntity))
        return;
      MyPlayerCollection.UpdateControl((MyEntity) this);
    }

    public override void OnRemovedFromScene(object source)
    {
      if (this.HasUnsafeValues)
        this.CubeGrid.UnregisterUnsafeBlock((MyCubeBlock) this);
      base.OnRemovedFromScene(source);
    }

    protected override void Closing()
    {
      base.Closing();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || Sandbox.Game.Multiplayer.Sync.Clients == null)
        return;
      Sandbox.Game.Multiplayer.Sync.Clients.ClientRemoved -= new Action<ulong>(this.ClientRemoved);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_TerminalBlock builderCubeBlock = (MyObjectBuilder_TerminalBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.CustomName = this.DisplayNameText.ToString();
      builderCubeBlock.ShowOnHUD = this.ShowOnHUD;
      builderCubeBlock.ShowInTerminal = this.ShowInTerminal;
      builderCubeBlock.ShowInInventory = this.ShowInInventory;
      builderCubeBlock.ShowInToolbarConfig = this.ShowInToolbarConfig;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void NotifyTerminalValueChanged(ITerminalControl control)
    {
    }

    public void RefreshCustomInfo()
    {
      this.CustomInfo.Clear();
      Action<MyTerminalBlock, StringBuilder> appendingCustomInfo = this.AppendingCustomInfo;
      if (appendingCustomInfo == null)
        return;
      appendingCustomInfo(this, this.CustomInfo);
    }

    public void SetCustomName(string text)
    {
      this.UpdateCustomName(text);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTerminalBlock, string>(this, (Func<MyTerminalBlock, Action<string>>) (x => new Action<string>(x.SetCustomNameEvent)), text);
    }

    public void UpdateCustomName(string text)
    {
      if (!this.CustomName.CompareUpdate(text))
        return;
      this.RaiseCustomNameChanged();
      this.RaiseShowOnHUDChanged();
      this.DisplayNameText = text;
    }

    public void SetCustomName(StringBuilder text)
    {
      this.UpdateCustomName(text);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTerminalBlock, string>(this, (Func<MyTerminalBlock, Action<string>>) (x => new Action<string>(x.SetCustomNameEvent)), text.ToString());
    }

    [Event(null, 362)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    public void SetCustomNameEvent(string name) => this.UpdateCustomName(name);

    public void UpdateCustomName(StringBuilder text)
    {
      if (!this.CustomName.CompareUpdate(text))
        return;
      this.DisplayNameText = text.ToString();
      this.RaiseCustomNameChanged();
      this.RaiseShowOnHUDChanged();
    }

    private void RaiseCustomNameChanged()
    {
      Action<MyTerminalBlock> customNameChanged = this.CustomNameChanged;
      if (customNameChanged == null)
        return;
      customNameChanged(this);
    }

    public void RaisePropertiesChanged()
    {
      Action<MyTerminalBlock> propertiesChanged = this.PropertiesChanged;
      if (propertiesChanged == null)
        return;
      propertiesChanged(this);
    }

    public void SetDetailedInfoDirty() => this.m_detailedInfoDirty = true;

    protected void RaiseVisibilityChanged()
    {
      Action<MyTerminalBlock> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged(this);
    }

    protected void RaiseShowOnHUDChanged()
    {
      Action<MyTerminalBlock> showOnHudChanged = this.ShowOnHUDChanged;
      if (showOnHudChanged == null)
        return;
      showOnHudChanged(this);
    }

    protected void RaiseShowInTerminalChanged()
    {
      Action<MyTerminalBlock> inTerminalChanged = this.ShowInTerminalChanged;
      if (inTerminalChanged == null)
        return;
      inTerminalChanged(this);
    }

    protected void RaiseShowInInventoryChanged()
    {
      Action<MyTerminalBlock> inIventoryChanged = this.ShowInIventoryChanged;
      if (inIventoryChanged == null)
        return;
      inIventoryChanged(this);
    }

    protected void RaiseShowInToolbarConfigChanged()
    {
      Action<MyTerminalBlock> toolbarConfigChanged = this.ShowInToolbarConfigChanged;
      if (toolbarConfigChanged == null)
        return;
      toolbarConfigChanged(this);
    }

    protected void RaiseIsBeingHackedChanged()
    {
      Action<MyTerminalBlock> beingHackedChanged = this.IsBeingHackedChanged;
      if (beingHackedChanged == null)
        return;
      beingHackedChanged(this);
    }

    public bool HasLocalPlayerAccess() => this.HasPlayerAccess(MySession.Static.LocalPlayerId);

    public bool HasPlayerAccess(long identityId) => this.HasPlayerAccessReason(identityId) == MyTerminalBlock.AccessRightsResult.Granted;

    public MyTerminalBlock.AccessRightsResult HasPlayerAccessReason(long identityId)
    {
      if (!MyFakes.SHOW_FACTIONS_GUI)
        return MyTerminalBlock.AccessRightsResult.Other;
      return this.HasAdminUseTerminals(identityId) || this.GetUserRelationToOwner(identityId).IsFriendly() ? MyTerminalBlock.AccessRightsResult.Granted : MyTerminalBlock.AccessRightsResult.Enemies;
    }

    internal bool HasLocalPlayerAdminUseTerminals() => this.HasAdminUseTerminals(MySession.Static.LocalPlayerId);

    internal bool HasAdminUseTerminals(long identityId)
    {
      ulong steamId = MySession.Static.Players.TryGetSteamId(identityId);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        AdminSettingsEnum adminSettingsEnum;
        if (MySession.Static.RemoteAdminSettings.TryGetValue(steamId, out adminSettingsEnum) && (adminSettingsEnum & AdminSettingsEnum.UseTerminals) != AdminSettingsEnum.None)
          return true;
      }
      else if (identityId == MySession.Static.LocalPlayerId)
      {
        if ((MySession.Static.AdminSettings & AdminSettingsEnum.UseTerminals) != AdminSettingsEnum.None)
          return true;
      }
      else
      {
        AdminSettingsEnum adminSettingsEnum;
        if (MySession.Static.RemoteAdminSettings.TryGetValue(steamId, out adminSettingsEnum) && (adminSettingsEnum & AdminSettingsEnum.UseTerminals) != AdminSettingsEnum.None)
          return true;
      }
      return false;
    }

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      this.CustomNameWithFaction.Clear();
      if (!string.IsNullOrEmpty(this.GetOwnerFactionTag()))
      {
        this.CustomNameWithFaction.Append(this.GetOwnerFactionTag());
        this.CustomNameWithFaction.Append(".");
      }
      this.CustomNameWithFaction.AppendStringBuilder(this.CustomName);
      this.m_hudParams.Clear();
      this.m_hudParams.Add(new MyHudEntityParams()
      {
        FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL,
        Text = this.CustomNameWithFaction,
        Owner = this.IDModule != null ? this.IDModule.Owner : 0L,
        Share = this.IDModule != null ? this.IDModule.ShareMode : MyOwnershipShareModeEnum.None,
        Entity = (VRage.ModAPI.IMyEntity) this,
        BlinkingTime = !allowBlink || !this.IsBeingHacked ? 0.0f : 10f
      });
      return this.m_hudParams;
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      this.RaiseOwnershipChanged();
      this.RaiseShowOnHUDChanged();
      this.RaisePropertiesChanged();
    }

    private void RaiseOwnershipChanged()
    {
      if (this.OwnershipChanged == null)
        return;
      this.OwnershipChanged(this);
    }

    public virtual void GetTerminalName(StringBuilder result) => result.AppendStringBuilder(this.CustomName);

    protected void PrintUpgradeModuleInfo(StringBuilder output)
    {
      if (this.GetComponent().ConnectionPositions.Count == 0)
        return;
      int num1 = 0;
      if (this.CurrentAttachedUpgradeModules != null)
      {
        foreach (MyCubeBlock.AttachedUpgradeModule attachedUpgradeModule in this.CurrentAttachedUpgradeModules.Values)
          num1 += attachedUpgradeModule.SlotCount;
      }
      output.Append(MyTexts.Get(MyCommonTexts.Module_UsedSlots).ToString() + num1.ToString() + " / " + this.GetComponent().ConnectionPositions.Count.ToString() + "\n");
      if (this.CurrentAttachedUpgradeModules != null)
      {
        int num2 = 0;
        foreach (MyCubeBlock.AttachedUpgradeModule attachedUpgradeModule in this.CurrentAttachedUpgradeModules.Values)
          num2 += attachedUpgradeModule.Block == null || !attachedUpgradeModule.Block.IsWorking ? 0 : 1;
        output.Append(MyTexts.Get(MyCommonTexts.Module_Attached).ToString() + this.CurrentAttachedUpgradeModules.Count.ToString());
        if (num2 != this.CurrentAttachedUpgradeModules.Count)
          output.Append(" (" + num2.ToString() + MyTexts.Get(MyCommonTexts.Module_Functioning).ToString());
        output.Append("\n");
        foreach (MyCubeBlock.AttachedUpgradeModule attachedUpgradeModule in this.CurrentAttachedUpgradeModules.Values)
        {
          if (attachedUpgradeModule.Block != null)
            output.Append(" - " + attachedUpgradeModule.Block.DisplayNameText + (attachedUpgradeModule.Block.IsFunctional ? (attachedUpgradeModule.Compatible ? (attachedUpgradeModule.Block.Enabled ? "" : MyTexts.Get(MyCommonTexts.Module_Off).ToString()) : MyTexts.Get(MyCommonTexts.Module_Incompatible).ToString()) : MyTexts.Get(MyCommonTexts.Module_Damaged).ToString()));
          else
            output.Append(MyTexts.Get(MyCommonTexts.Module_Unknown).ToString());
          output.Append("\n");
        }
      }
      output.AppendFormat("\n");
    }

    protected void FixSingleInventory()
    {
      MyInventoryBase component;
      if (!this.Components.TryGet<MyInventoryBase>(out component))
        return;
      MyInventoryAggregate inventoryAggregate = component as MyInventoryAggregate;
      MyInventory myInventory1 = (MyInventory) null;
      if (inventoryAggregate != null)
      {
        foreach (MyComponentBase myComponentBase in inventoryAggregate.ChildList.Reader)
        {
          if (myComponentBase is MyInventory myInventory2)
          {
            if (myInventory1 == null)
              myInventory1 = myInventory2;
            else if (myInventory1.GetItemsCount() < myInventory2.GetItemsCount())
              myInventory1 = myInventory2;
          }
        }
      }
      if (myInventory1 == null)
        return;
      this.Components.Remove<MyInventoryBase>();
      this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory1);
    }

    protected virtual void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyTerminalBlock>())
        return;
      MyTerminalControlOnOffSwitch<MyTerminalBlock> controlOnOffSwitch1 = new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInTerminal", MySpaceTexts.Terminal_ShowInTerminal, MySpaceTexts.Terminal_ShowInTerminalToolTip);
      controlOnOffSwitch1.Getter = (MyTerminalValueControl<MyTerminalBlock, bool>.GetterDelegate) (x => (bool) x.m_showInTerminal);
      controlOnOffSwitch1.Setter = (MyTerminalValueControl<MyTerminalBlock, bool>.SetterDelegate) ((x, v) => x.ShowInTerminal = v);
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) controlOnOffSwitch1);
      MyTerminalControlOnOffSwitch<MyTerminalBlock> controlOnOffSwitch2 = new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInInventory", MySpaceTexts.Terminal_ShowInInventory, MySpaceTexts.Terminal_ShowInInventoryToolTip, max_Width: 0.25f, is_AutoEllipsisEnabled: true, is_AutoScaleEnabled: true);
      controlOnOffSwitch2.Getter = (MyTerminalValueControl<MyTerminalBlock, bool>.GetterDelegate) (x => (bool) x.m_showInInventory);
      controlOnOffSwitch2.Setter = (MyTerminalValueControl<MyTerminalBlock, bool>.SetterDelegate) ((x, v) => x.ShowInInventory = v);
      controlOnOffSwitch2.Visible = (Func<MyTerminalBlock, bool>) (x => x.HasInventory);
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) controlOnOffSwitch2);
      MyTerminalControlOnOffSwitch<MyTerminalBlock> controlOnOffSwitch3 = new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInToolbarConfig", MySpaceTexts.Terminal_ShowInToolbarConfig, MySpaceTexts.Terminal_ShowInToolbarConfigToolTip, max_Width: 0.25f, is_AutoEllipsisEnabled: true, is_AutoScaleEnabled: true);
      controlOnOffSwitch3.Getter = (MyTerminalValueControl<MyTerminalBlock, bool>.GetterDelegate) (x => (bool) x.m_showInToolbarConfig);
      controlOnOffSwitch3.Setter = (MyTerminalValueControl<MyTerminalBlock, bool>.SetterDelegate) ((x, v) => x.ShowInToolbarConfig = v);
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) controlOnOffSwitch3);
      MyTerminalControlTextbox<MyTerminalBlock> terminalControlTextbox = new MyTerminalControlTextbox<MyTerminalBlock>("Name", MyCommonTexts.Name, MySpaceTexts.Blank);
      terminalControlTextbox.Getter = (MyTerminalControlTextbox<MyTerminalBlock>.GetterDelegate) (x => x.CustomName);
      terminalControlTextbox.Setter = (MyTerminalControlTextbox<MyTerminalBlock>.SetterDelegate) ((x, v) => x.SetCustomName(v));
      terminalControlTextbox.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) terminalControlTextbox);
      MyTerminalControlOnOffSwitch<MyTerminalBlock> onOff = new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowOnHUD", MySpaceTexts.Terminal_ShowOnHUD, MySpaceTexts.Terminal_ShowOnHUDToolTip);
      onOff.Getter = (MyTerminalValueControl<MyTerminalBlock, bool>.GetterDelegate) (x => x.ShowOnHUD);
      onOff.Setter = (MyTerminalValueControl<MyTerminalBlock, bool>.SetterDelegate) ((x, v) => x.ShowOnHUD = v);
      onOff.EnableToggleAction<MyTerminalBlock>();
      onOff.EnableOnOffActions<MyTerminalBlock>();
      onOff.Visible = (Func<MyTerminalBlock, bool>) (x => x.CanShowOnHud);
      onOff.Enabled = (Func<MyTerminalBlock, bool>) (x => x.CanShowOnHud);
      foreach (MyTerminalAction<MyTerminalBlock> action in onOff.Actions)
        action.Enabled = (Func<MyTerminalBlock, bool>) (x => x.CanShowOnHud);
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) onOff);
      MyTerminalControlButton<MyTerminalBlock> terminalControlButton = new MyTerminalControlButton<MyTerminalBlock>("CustomData", MySpaceTexts.Terminal_CustomData, MySpaceTexts.Terminal_CustomDataTooltip, new Action<MyTerminalBlock>(MyTerminalBlock.CustomDataClicked));
      terminalControlButton.Enabled = (Func<MyTerminalBlock, bool>) (x => !x.m_textboxOpen);
      terminalControlButton.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) terminalControlButton);
    }

    protected static void CustomDataClicked(MyTerminalBlock myTerminalBlock) => myTerminalBlock.OpenWindow(true, true);

    private void RaiseCustomDataChanged() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTerminalBlock, string>(this, (Func<MyTerminalBlock, Action<string>>) (x => new Action<string>(x.OnCustomDataChanged)), this.CustomData);

    [Event(null, 692)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    private void OnCustomDataChanged(string data) => this.SetCustomData_Internal(data, false);

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTerminalBlock, bool, bool, ulong>(this, (Func<MyTerminalBlock, Action<bool, bool, ulong>>) (x => new Action<bool, bool, ulong>(x.OnChangeOpenRequest)), isOpen, editable, user);

    [Event(null, 703)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user)
    {
      if (((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (this.m_textboxOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTerminalBlock, bool, bool, ulong>(this, (Func<MyTerminalBlock, Action<bool, bool, ulong>>) (x => new Action<bool, bool, ulong>(x.OnChangeOpenSuccess)), isOpen, editable, user);
    }

    [Event(null, 714)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user) => this.OnChangeOpen(isOpen, editable, user);

    private void OnChangeOpen(bool isOpen, bool editable, ulong user)
    {
      this.m_textboxOpen = isOpen;
      this.m_currentUser = user;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sandbox.Game.Multiplayer.Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false);
    }

    private void CreateTextBox(bool isEditable, string description)
    {
      string missionTitle = this.CustomName.ToString();
      string currentObjective = MyTexts.GetString(MySpaceTexts.Terminal_CustomData);
      string description1 = description;
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedTextBox);
      int num = flag ? 1 : 0;
      this.m_textBox = new MyGuiScreenTextPanel(missionTitle, "", currentObjective, description1, resultCallback, editable: (num != 0));
    }

    public void OpenWindow(bool isEditable, bool sync)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sandbox.Game.Multiplayer.Sync.MyId);
      }
      else
      {
        this.CreateTextBox(isEditable, this.CustomData);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBox);
      }
    }

    public void OnClosedTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBox == null)
        return;
      this.CloseWindow();
    }

    public void OnClosedMessageBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (result == VRage.Game.ModAPI.ResultEnum.OK)
      {
        this.CloseWindow();
      }
      else
      {
        this.CreateTextBox(true, this.m_textBox.Description.Text.ToString());
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBox);
      }
    }

    private void CloseWindow()
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.CustomData = this.m_textBox.Description.Text.ToString();
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    private void ClientRemoved(ulong steamId)
    {
      if ((long) steamId != (long) this.m_currentUser)
        return;
      this.SendChangeOpenMessage(false);
    }

    protected void OnUnsafeSettingsChanged() => MySandboxGame.Static.Invoke("", (object) this, (Action<object>) (x => MyTerminalBlock.OnUnsafeSettingsChangedInternal(x)));

    private static void OnUnsafeSettingsChangedInternal(object o)
    {
      MyTerminalBlock myTerminalBlock = (MyTerminalBlock) o;
      if (myTerminalBlock.MarkedForClose)
        return;
      bool flag = myTerminalBlock.HasUnsafeSettingsCollector();
      if (myTerminalBlock.HasUnsafeValues == flag)
        return;
      myTerminalBlock.HasUnsafeValues = flag;
      if (flag)
        myTerminalBlock.CubeGrid.RegisterUnsafeBlock((MyCubeBlock) myTerminalBlock);
      else
        myTerminalBlock.CubeGrid.UnregisterUnsafeBlock((MyCubeBlock) myTerminalBlock);
    }

    protected virtual bool HasUnsafeSettingsCollector() => false;

    protected virtual void UpdateDetailedInfo(StringBuilder detailedInfo) => detailedInfo.Clear();

    public override string ToString() => base.ToString() + " " + (object) this.CustomName;

    public virtual void OnOpenedInTerminal(bool state)
    {
    }

    string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomName
    {
      get => this.CustomName.ToString();
      set => this.SetCustomName(value);
    }

    string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomNameWithFaction => this.CustomNameWithFaction.ToString();

    string Sandbox.ModAPI.Ingame.IMyTerminalBlock.DetailedInfo => this.DetailedInfo.ToString();

    string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomInfo => this.CustomInfo.ToString();

    private Action<MyTerminalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock> value) => (Action<MyTerminalBlock>) Delegate.CreateDelegate(typeof (Action<MyTerminalBlock>), value.Target, value.Method);

    private Action<MyTerminalBlock, StringBuilder> GetDelegate(
      Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> value)
    {
      return (Action<MyTerminalBlock, StringBuilder>) Delegate.CreateDelegate(typeof (Action<MyTerminalBlock, StringBuilder>), value.Target, value.Method);
    }

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.CustomNameChanged
    {
      add => this.CustomNameChanged += this.GetDelegate(value);
      remove => this.CustomNameChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.OwnershipChanged
    {
      add => this.OwnershipChanged += this.GetDelegate(value);
      remove => this.OwnershipChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.PropertiesChanged
    {
      add => this.PropertiesChanged += this.GetDelegate(value);
      remove => this.PropertiesChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.ShowOnHUDChanged
    {
      add => this.ShowOnHUDChanged += this.GetDelegate(value);
      remove => this.ShowOnHUDChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.VisibilityChanged
    {
      add => this.VisibilityChanged += this.GetDelegate(value);
      remove => this.VisibilityChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> Sandbox.ModAPI.IMyTerminalBlock.AppendingCustomInfo
    {
      add => this.AppendingCustomInfo += this.GetDelegate(value);
      remove => this.AppendingCustomInfo -= this.GetDelegate(value);
    }

    bool Sandbox.ModAPI.IMyTerminalBlock.IsInSameLogicalGroupAs(
      Sandbox.ModAPI.IMyTerminalBlock other)
    {
      return this.CubeGrid.IsInSameLogicalGroupAs(other.CubeGrid);
    }

    bool Sandbox.ModAPI.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.IMyTerminalBlock other) => this.CubeGrid.IsSameConstructAs(other.CubeGrid);

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.CustomDataChanged
    {
      add => this.CustomDataChanged += this.GetDelegate(value);
      remove => this.CustomDataChanged -= this.GetDelegate(value);
    }

    void Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActions(
      List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList,
      Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect)
    {
      ((IMyTerminalActionsHelper) MyTerminalControlFactoryHelper.Static).GetActions(this.GetType(), resultList, collect);
    }

    void Sandbox.ModAPI.Ingame.IMyTerminalBlock.SearchActionsOfName(
      string name,
      List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList,
      Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
    {
      ((IMyTerminalActionsHelper) MyTerminalControlFactoryHelper.Static).SearchActionsOfName(name, this.GetType(), resultList, collect);
    }

    Sandbox.ModAPI.Interfaces.ITerminalAction Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActionWithName(
      string name)
    {
      return ((IMyTerminalActionsHelper) MyTerminalControlFactoryHelper.Static).GetActionWithName(name, this.GetType());
    }

    public ITerminalProperty GetProperty(string id) => MyTerminalControlFactoryHelper.Static.GetProperty(id, this.GetType());

    public void GetProperties(
      List<ITerminalProperty> resultList,
      Func<ITerminalProperty, bool> collect = null)
    {
      MyTerminalControlFactoryHelper.Static.GetProperties(this.GetType(), resultList, collect);
    }

    bool Sandbox.ModAPI.Ingame.IMyTerminalBlock.IsSameConstructAs(
      Sandbox.ModAPI.Ingame.IMyTerminalBlock other)
    {
      return ((VRage.Game.ModAPI.Ingame.IMyCubeGrid) this.CubeGrid).IsSameConstructAs(other.CubeGrid);
    }

    public enum AccessRightsResult
    {
      Granted,
      Enemies,
      MissingDLC,
      Other,
      None,
    }

    protected sealed class SetCustomNameEvent\u003C\u003ESystem_String : ICallSite<MyTerminalBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTerminalBlock @this,
        in string name,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetCustomNameEvent(name);
      }
    }

    protected sealed class OnCustomDataChanged\u003C\u003ESystem_String : ICallSite<MyTerminalBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTerminalBlock @this,
        in string data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnCustomDataChanged(data);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64 : ICallSite<MyTerminalBlock, bool, bool, ulong, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTerminalBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64 : ICallSite<MyTerminalBlock, bool, bool, ulong, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTerminalBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user);
      }
    }

    protected class m_showOnHUD\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTerminalBlock) obj0).m_showOnHUD = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_showInTerminal\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTerminalBlock) obj0).m_showInTerminal = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_showInToolbarConfig\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTerminalBlock) obj0).m_showInToolbarConfig = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_showInInventory\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTerminalBlock) obj0).m_showInInventory = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyTerminalBlock\u003C\u003EActor : IActivator, IActivator<MyTerminalBlock>
    {
      object IActivator.CreateInstance() => (object) new MyTerminalBlock();

      MyTerminalBlock IActivator<MyTerminalBlock>.CreateInstance() => new MyTerminalBlock();
    }
  }
}
