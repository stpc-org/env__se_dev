// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyButtonPanel
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using SpaceEngineers.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ButtonPanel))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyButtonPanel), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel)})]
  public class MyButtonPanel : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyButtonPanel, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
  {
    private const string DETECTOR_NAME = "panel";
    private List<string> m_emissiveNames;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;
    private int m_selectedButton = -1;
    private MyHudNotification m_activationFailedNotification = new MyHudNotification(MySpaceTexts.Notification_ActivationFailed, font: "Red");
    private static List<MyToolbar> m_openedToolbars;
    private static bool m_shouldSetOtherToolbars;
    private SerializableDictionary<int, string> m_customButtonNames = new SerializableDictionary<int, string>();
    private List<MyUseObjectPanelButton> m_buttonsUseObjects = new List<MyUseObjectPanelButton>();
    private StringBuilder m_emptyName = new StringBuilder("");
    private bool m_syncing;
    private bool m_isTextPanelOpen;
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBoxMultiPanel;
    private static StringBuilder m_helperSB = new StringBuilder();

    public MyToolbar Toolbar { get; set; }

    public MyButtonPanelDefinition BlockDefinition => base.BlockDefinition as MyButtonPanelDefinition;

    public bool AnyoneCanUse
    {
      get => (bool) this.m_anyoneCanUse;
      set => this.m_anyoneCanUse.Value = value;
    }

    public bool IsTextPanelOpen
    {
      get => this.m_isTextPanelOpen;
      set
      {
        if (this.m_isTextPanelOpen == value)
          return;
        this.m_isTextPanelOpen = value;
        this.RaisePropertiesChanged();
      }
    }

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return this.m_multiPanel == null ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : this.m_multiPanel.GetSurface(index);
    }

    public MyButtonPanel()
    {
      this.CreateTerminalControls();
      MyButtonPanel.m_openedToolbars = new List<MyToolbar>();
      this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyButtonPanel>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyButtonPanel> checkbox = new MyTerminalControlCheckbox<MyButtonPanel>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse);
      checkbox.Getter = (MyTerminalValueControl<MyButtonPanel, bool>.GetterDelegate) (x => x.AnyoneCanUse);
      checkbox.Setter = (MyTerminalValueControl<MyButtonPanel, bool>.SetterDelegate) ((x, v) => x.AnyoneCanUse = v);
      checkbox.EnableAction<MyButtonPanel>();
      MyTerminalControlFactory.AddControl<MyButtonPanel>((MyTerminalControl<MyButtonPanel>) checkbox);
      MyTerminalControlFactory.AddControl<MyButtonPanel>((MyTerminalControl<MyButtonPanel>) new MyTerminalControlButton<MyButtonPanel>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, (Action<MyButtonPanel>) (self =>
      {
        MyButtonPanel.m_openedToolbars.Add(self.Toolbar);
        if (MyGuiScreenToolbarConfigBase.Static != null)
          return;
        MyButtonPanel.m_shouldSetOtherToolbars = true;
        MyToolbarComponent.CurrentToolbar = self.Toolbar;
        MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) self, null);
        MyToolbarComponent.AutoUpdate = false;
        screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          MyToolbarComponent.AutoUpdate = true;
          MyButtonPanel.m_openedToolbars.Clear();
        });
        MyGuiSandbox.AddScreen(screen);
      })));
      MyTerminalControlFactory.AddControl<MyButtonPanel>((MyTerminalControl<MyButtonPanel>) new MyTerminalControlListbox<MyButtonPanel>("ButtonText", MySpaceTexts.BlockPropertyText_ButtonList, MySpaceTexts.Blank)
      {
        ListContent = (MyTerminalControlListbox<MyButtonPanel>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillListContent(list1, list2)),
        ItemSelected = (MyTerminalControlListbox<MyButtonPanel>.SelectItemDelegate) ((x, y) => x.SelectButtonToName(y))
      });
      MyTerminalControlTextbox<MyButtonPanel> terminalControlTextbox = new MyTerminalControlTextbox<MyButtonPanel>("ButtonName", MySpaceTexts.BlockPropertyText_ButtonName, MySpaceTexts.Blank);
      terminalControlTextbox.Getter = (MyTerminalControlTextbox<MyButtonPanel>.GetterDelegate) (x => x.GetButtonName());
      terminalControlTextbox.Setter = (MyTerminalControlTextbox<MyButtonPanel>.SetterDelegate) ((x, v) => x.SetCustomButtonName(v));
      terminalControlTextbox.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyButtonPanel>((MyTerminalControl<MyButtonPanel>) terminalControlTextbox);
      MyMultiTextPanelComponent.CreateTerminalControls<MyButtonPanel>();
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, 0.0001f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : 0.0001f));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      resourceSinkComponent.IsPoweredChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(builder, cubeGrid);
      this.m_emissiveNames = new List<string>(this.BlockDefinition.ButtonCount);
      for (int index = 1; index <= this.BlockDefinition.ButtonCount; ++index)
        this.m_emissiveNames.Add(string.Format("Emissive{0}", (object) index));
      MyObjectBuilder_ButtonPanel builderButtonPanel = builder as MyObjectBuilder_ButtonPanel;
      this.Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, Math.Min(this.BlockDefinition.ButtonCount, 9), this.BlockDefinition.ButtonCount / 9 + 1);
      this.Toolbar.DrawNumbers = false;
      this.Toolbar.GetSymbol = (Func<int, ColoredIcon>) (slot =>
      {
        ColoredIcon coloredIcon = new ColoredIcon();
        if (this.Toolbar.SlotToIndex(slot) < this.BlockDefinition.ButtonCount)
        {
          coloredIcon.Icon = this.BlockDefinition.ButtonSymbols[this.Toolbar.SlotToIndex(slot) % this.BlockDefinition.ButtonSymbols.Length];
          Vector4 buttonColor = this.BlockDefinition.ButtonColors[this.Toolbar.SlotToIndex(slot) % this.BlockDefinition.ButtonColors.Length];
          buttonColor.W = 1f;
          coloredIcon.Color = buttonColor;
        }
        return coloredIcon;
      });
      this.Toolbar.Init(builderButtonPanel.Toolbar, (MyEntity) this);
      this.Toolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      this.m_anyoneCanUse.SetLocalValue(builderButtonPanel.AnyoneCanUse);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.Update();
      if (builderButtonPanel.CustomButtonNames != null)
      {
        foreach (int key in builderButtonPanel.CustomButtonNames.Dictionary.Keys)
          this.m_customButtonNames.Dictionary.Add(key, MyStatControlText.SubstituteTexts(builderButtonPanel.CustomButtonNames[key]));
      }
      if (this.BlockDefinition.ScreenAreas != null && this.BlockDefinition.ScreenAreas.Count > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.BlockDefinition.ScreenAreas, builderButtonPanel.TextPanels);
        this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UseObjectsComponent.GetInteractiveObjects<MyUseObjectPanelButton>(this.m_buttonsUseObjects);
    }

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateEmissivity();
      this.UpdateScreen();
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      MyContainerDropComponent component;
      if (!this.Components.TryGet<MyContainerDropComponent>(out component))
        return;
      component.UpdateSound();
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_syncing)
        return;
      ToolbarItem toolbarItem = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
      this.UpdateButtonEmissivity(index.ItemIndex);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, ToolbarItem, int>(this, (Func<MyButtonPanel, Action<ToolbarItem, int>>) (x => new Action<ToolbarItem, int>(x.SendToolbarItemChanged)), toolbarItem, index.ItemIndex);
      if (MyButtonPanel.m_shouldSetOtherToolbars)
      {
        MyButtonPanel.m_shouldSetOtherToolbars = false;
        foreach (MyToolbar openedToolbar in MyButtonPanel.m_openedToolbars)
        {
          if (openedToolbar != self)
            openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
        }
        MyButtonPanel.m_shouldSetOtherToolbars = true;
      }
      MyToolbarItem itemAtIndex = this.Toolbar.GetItemAtIndex(index.ItemIndex);
      if (itemAtIndex != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, string, int>(this, (Func<MyButtonPanel, Action<string, int>>) (x => new Action<string, int>(x.SetButtonName)), itemAtIndex.DisplayName.ToString(), index.ItemIndex);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, string, int>(this, (Func<MyButtonPanel, Action<string, int>>) (x => new Action<string, int>(x.SetButtonName)), MyTexts.GetString(MySpaceTexts.NotificationHintNoAction), index.ItemIndex);
    }

    private void UpdateEmissivity()
    {
      for (int index = 0; index < this.BlockDefinition.ButtonCount; ++index)
        this.UpdateButtonEmissivity(index);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.InScene)
        this.UpdateEmissivity();
      if (this.m_multiPanel != null)
        this.m_multiPanel.Reset();
      this.UpdateScreen();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
      this.m_buttonsUseObjects.Clear();
      this.UseObjectsComponent.GetInteractiveObjects<MyUseObjectPanelButton>(this.m_buttonsUseObjects);
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.UpdateEmissivity();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }

    private void UpdateButtonEmissivity(int index)
    {
      if (!this.InScene)
        return;
      Vector4 vector4 = this.BlockDefinition.ButtonColors[index % this.BlockDefinition.ButtonColors.Length];
      if (this.Toolbar.GetItemAtIndex(index) == null)
        vector4 = this.BlockDefinition.UnassignedButtonColor;
      float emissivity = vector4.W;
      if (!this.IsWorking)
      {
        if (this.IsFunctional)
        {
          vector4 = Color.Red.ToVector4();
          emissivity = 0.0f;
        }
        else
        {
          vector4 = Color.Black.ToVector4();
          emissivity = 0.0f;
        }
      }
      MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], this.m_emissiveNames[index], new Color(vector4.X, vector4.Y, vector4.Z), emissivity);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ButtonPanel builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ButtonPanel;
      builderCubeBlock.Toolbar = this.Toolbar.GetObjectBuilder();
      builderCubeBlock.AnyoneCanUse = this.AnyoneCanUse;
      builderCubeBlock.CustomButtonNames = this.m_customButtonNames;
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void PressButton(int i)
    {
      if (this.ButtonPressed == null)
        return;
      this.ButtonPressed(i);
    }

    private event Action<int> ButtonPressed;

    [Event(null, 379)]
    [Reliable]
    [Server(ValidationType.Access)]
    public void ActivateButton(int index, long userId)
    {
      MyCharacter entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(userId, out entity);
      long playerId = entity != null ? entity.GetPlayerIdentityId() : 0L;
      if (MyVisualScriptLogicProvider.ButtonPressedTerminalName != null)
        MyVisualScriptLogicProvider.ButtonPressedTerminalName(this.CustomName.ToString(), index, playerId, this.EntityId);
      if (MyVisualScriptLogicProvider.ButtonPressedEntityName != null)
        MyVisualScriptLogicProvider.ButtonPressedEntityName(this.Name, index, playerId, this.EntityId);
      this.Toolbar.UpdateItem(index);
      int num = this.Toolbar.ActivateItemAtIndex(index) ? 1 : 0;
      this.PressButton(index);
      if (num != 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel>(this, (Func<MyButtonPanel, Action>) (x => new Action(x.NotifyActivationFailed)), MyEventContext.Current.Sender);
    }

    [Event(null, 398)]
    [Reliable]
    [Client]
    private void NotifyActivationFailed() => MyHud.Notifications.Add((MyHudNotificationBase) this.m_activationFailedNotification);

    protected override void Closing()
    {
      base.Closing();
      foreach (MyUseObjectPanelButton buttonsUseObject in this.m_buttonsUseObjects)
        buttonsUseObject.RemoveButtonMarker();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SetRender((MyRenderComponentScreenAreas) null);
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_multiPanel == null)
        return;
      this.UpdateScreen();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_multiPanel != null && this.m_multiPanel.SurfaceCount > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene();
    }

    public void FillListContent(
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
    {
      string str = MyTexts.GetString(MySpaceTexts.BlockPropertyText_Button);
      for (int index = 0; index < this.m_buttonsUseObjects.Count; ++index)
      {
        MyButtonPanel.m_helperSB.Clear().Append(str + " " + (index + 1).ToString());
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(MyButtonPanel.m_helperSB, userData: ((object) index));
        listBoxContent.Add(obj);
        if (index == this.m_selectedButton)
          listBoxSelectedItems.Add(obj);
      }
    }

    public void SelectButtonToName(List<MyGuiControlListbox.Item> imageIds)
    {
      if (imageIds.Count <= 0)
        return;
      this.m_selectedButton = (int) imageIds[0].UserData;
      this.RaisePropertiesChanged();
    }

    public StringBuilder GetButtonName()
    {
      if (this.m_selectedButton == -1)
        return this.m_emptyName;
      string str = (string) null;
      if (this.m_customButtonNames.Dictionary.TryGetValue(this.m_selectedButton, out str))
        return new StringBuilder(str);
      MyToolbarItem itemAtIndex = this.Toolbar.GetItemAtIndex(this.m_selectedButton);
      return itemAtIndex != null ? itemAtIndex.DisplayName : this.m_emptyName;
    }

    [Event(null, 489)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void SetButtonName(string name, int position)
    {
      string str = (string) null;
      if (name == null)
        this.m_customButtonNames.Dictionary.Remove(position);
      else if (this.m_customButtonNames.Dictionary.TryGetValue(position, out str))
        this.m_customButtonNames.Dictionary[position] = name.ToString();
      else
        this.m_customButtonNames.Dictionary.Add(position, name.ToString());
    }

    public bool IsButtonAssigned(int pos) => this.Toolbar.GetItemAtIndex(pos) != null;

    public bool HasCustomButtonName(int pos) => this.m_customButtonNames.Dictionary.ContainsKey(pos);

    public void SetCustomButtonName(string name, int pos) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, string, int>(this, (Func<MyButtonPanel, Action<string, int>>) (x => new Action<string, int>(x.SetButtonName)), name, pos);

    public void SetCustomButtonName(StringBuilder name)
    {
      if (this.m_selectedButton == -1)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, string, int>(this, (Func<MyButtonPanel, Action<string, int>>) (x => new Action<string, int>(x.SetButtonName)), name.ToString(), this.m_selectedButton);
    }

    public string GetCustomButtonName(int pos)
    {
      string str = (string) null;
      if (this.m_customButtonNames.Dictionary.TryGetValue(pos, out str))
        return str;
      MyToolbarItem itemAtIndex = this.Toolbar.GetItemAtIndex(pos);
      return itemAtIndex != null ? itemAtIndex.DisplayName.ToString() : MyTexts.GetString(MySpaceTexts.NotificationHintNoAction);
    }

    [Event(null, 546)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
    {
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (sentItem.EntityID != 0L)
        myToolbarItem = ToolbarItem.ToItem(sentItem);
      this.Toolbar.SetItemAtIndex(index, myToolbarItem);
      this.UpdateButtonEmissivity(index);
      this.m_syncing = false;
    }

    event Action<int> SpaceEngineers.Game.ModAPI.IMyButtonPanel.ButtonPressed
    {
      add => this.ButtonPressed += value;
      remove => this.ButtonPressed -= value;
    }

    string SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.GetButtonName(int index) => this.GetCustomButtonName(index);

    void SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.SetCustomButtonName(
      int index,
      string name)
    {
      this.SetCustomButtonName(name, index);
    }

    void SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.ClearCustomButtonName(
      int index)
    {
      this.SetCustomButtonName((string) null, index);
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.HasCustomButtonName(
      int index)
    {
      return this.HasCustomButtonName(index);
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.IsButtonAssigned(int index) => this.IsButtonAssigned(index);

    public void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    private void ChangeTextRequest(int panelIndex, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, int, string>(this, (Func<MyButtonPanel, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 605)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, int, MySerializableSpriteCollection>(this, (Func<MyButtonPanel, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 621)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, int, int[]>(this, (Func<MyButtonPanel, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, int, int[]>(this, (Func<MyButtonPanel, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 637)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.RemoveItems(panelIndex, selection);
    }

    [Event(null, 648)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SelectItems(panelIndex, selection);
    }

    void IMyMultiTextPanelComponentOwner.SelectPanel(
      List<MyGuiControlListbox.Item> panelItems)
    {
      if (this.m_multiPanel != null)
        this.m_multiPanel.SelectPanel((int) panelItems[0].UserData);
      this.RaisePropertiesChanged();
    }

    MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => this.m_multiPanel;

    public MyTextPanelComponent PanelComponent => this.m_multiPanel == null ? (MyTextPanelComponent) null : this.m_multiPanel.PanelComponent;

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sandbox.Game.Multiplayer.Sync.MyId, isPublic);
      }
      else
      {
        this.CreateTextBox(isEditable, new StringBuilder(this.PanelComponent.Text.ToString()), isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBoxMultiPanel == null)
        return;
      if (this.m_textBoxMultiPanel.Description.Text.Length > 100000)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedPanelMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText), callback: callback));
      }
      else
        this.CloseWindow(true);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBoxMultiPanel.Description.Text.Remove(100000, this.m_textBoxMultiPanel.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBoxMultiPanel.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    [Event(null, 736)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, bool, bool, ulong, bool>(this, (Func<MyButtonPanel, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 747)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, bool, bool, ulong, bool>(this, (Func<MyButtonPanel, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sandbox.Game.Multiplayer.Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBoxMultiPanel.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        this.PanelComponent.Text = description;
      }
      else
      {
        if (description.CompareTo(this.PanelComponent.Text) == 0)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyButtonPanel, int, string, bool>(this, (Func<MyButtonPanel, Action<int, string, bool>>) (x => new Action<int, string, bool>(x.OnChangeDescription)), this.m_multiPanel.SelectedPanelIndex, description.ToString(), isPublic);
      }
    }

    [Event(null, 805)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(int panelIndex, string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.m_multiPanel.GetPanelComponent(panelIndex).Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected sealed class ActivateButton\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<MyButtonPanel, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in int index,
        in long userId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ActivateButton(index, userId);
      }
    }

    protected sealed class NotifyActivationFailed\u003C\u003E : ICallSite<MyButtonPanel, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.NotifyActivationFailed();
      }
    }

    protected sealed class SetButtonName\u003C\u003ESystem_String\u0023System_Int32 : ICallSite<MyButtonPanel, string, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in string name,
        in int position,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetButtonName(name, position);
      }
    }

    protected sealed class SendToolbarItemChanged\u003C\u003ESandbox_Game_Entities_Blocks_ToolbarItem\u0023System_Int32 : ICallSite<MyButtonPanel, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in ToolbarItem sentItem,
        in int index,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemChanged(sentItem, index);
      }
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyButtonPanel, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in int panelIndex,
        in string text,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTextRequest(panelIndex, text);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyButtonPanel, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in int panelIndex,
        in MySerializableSpriteCollection sprites,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(panelIndex, sprites);
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyButtonPanel, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyButtonPanel, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyButtonPanel, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyButtonPanel, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeDescription\u003C\u003ESystem_Int32\u0023System_String\u0023System_Boolean : ICallSite<MyButtonPanel, int, string, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyButtonPanel @this,
        in int panelIndex,
        in string description,
        in bool isPublic,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(panelIndex, description, isPublic);
      }
    }

    protected class m_anyoneCanUse\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyButtonPanel) obj0).m_anyoneCanUse = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
