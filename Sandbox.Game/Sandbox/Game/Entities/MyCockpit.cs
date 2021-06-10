// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCockpit
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Autopilots;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Gui;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Cockpit))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyCockpit), typeof (Sandbox.ModAPI.Ingame.IMyCockpit)})]
  public class MyCockpit : MyShipController, IMyCameraController, IMyUsableEntity, Sandbox.ModAPI.IMyCockpit, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyCockpit, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, Sandbox.ModAPI.IMyTextSurfaceProvider, IMyConveyorEndpointBlock, IMyGasBlock, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner
  {
    private readonly float DEFAULT_FPS_CAMERA_X_ANGLE = -10f;
    public static float MAX_SHAKE_DAMAGE = 500f;
    public const double MAX_DRAW_DISTANCE = 200.0;
    private bool m_isLargeCockpit;
    private Vector3 m_playerHeadSpring;
    private Vector3 m_playerHeadShakeDir;
    protected float MinHeadLocalXAngle = -60f;
    protected float MaxHeadLocalXAngle = 70f;
    protected float MinHeadLocalYAngle = -90f;
    protected float MaxHeadLocalYAngle = 90f;
    private MatrixD m_cameraDummy = MatrixD.Identity;
    protected MatrixD m_characterDummy = MatrixD.Identity;
    protected MyCharacter m_pilot;
    private MyCharacter m_savedPilot;
    private long m_serverSidePilotId;
    private Matrix? m_pilotRelativeWorld;
    private MyAutopilotBase m_aiPilot;
    protected MyDefinitionId? m_pilotGunDefinition;
    private bool m_updateSink;
    private float m_headLocalXAngle;
    private float m_headLocalYAngle;
    private long m_lastGasInputUpdateTick;
    private string m_cockpitInteriorModel;
    private bool m_defferAttach;
    private bool m_playIdleSound;
    private float m_currentCameraShakePower;
    private bool? m_lastNearFlag;
    private int m_forcedFpsTimeoutMs;
    private const int m_forcedFpsTimeoutDefaultMs = 500;
    private float MIN_SHAKE_ACC = 1f;
    private float MAX_SHAKE_ACC = 10f;
    private float MAX_SHAKE = 0.5f;
    protected readonly Action<MyEntity> m_pilotClosedHandler;
    private bool? m_pilotJetpackEnabledBackup;
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBox;
    private bool m_isInFirstPersonView = true;
    private bool m_wasCameraForced;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private readonly VRage.Sync.Sync<float, SyncDirection.FromServer> m_oxygenFillLevel;
    private bool m_retryAttachPilot;
    private bool m_pilotFirstPerson;
    private bool m_isTextPanelOpen;
    private readonly Vector3I[] m_neighbourPositions = new Vector3I[26]
    {
      new Vector3I(1, 0, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(0, 0, -1),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 1, 0),
      new Vector3I(0, -1, 0),
      new Vector3I(1, 1, 0),
      new Vector3I(-1, 1, 0),
      new Vector3I(1, -1, 0),
      new Vector3I(-1, -1, 0),
      new Vector3I(1, 1, -1),
      new Vector3I(-1, 1, -1),
      new Vector3I(1, -1, -1),
      new Vector3I(-1, -1, -1),
      new Vector3I(1, 0, -1),
      new Vector3I(-1, 0, -1),
      new Vector3I(0, 1, -1),
      new Vector3I(0, -1, -1),
      new Vector3I(1, 1, 1),
      new Vector3I(-1, 1, 1),
      new Vector3I(1, -1, 1),
      new Vector3I(-1, -1, 1),
      new Vector3I(1, 0, 1),
      new Vector3I(-1, 0, 1),
      new Vector3I(0, 1, 1),
      new Vector3I(0, -1, 1)
    };
    private static readonly MyDefinitionId[] m_forgetTheseWeapons = new MyDefinitionId[1]
    {
      new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer))
    };

    public MyAutopilotBase AiPilot => this.m_aiPilot;

    public bool PilotJetpackEnabledBackup => this.m_pilotJetpackEnabledBackup.HasValue && this.m_pilotJetpackEnabledBackup.Value;

    public virtual bool IsInFirstPersonView
    {
      get => this.m_isInFirstPersonView;
      set
      {
        bool inFirstPersonView = this.m_isInFirstPersonView;
        this.m_isInFirstPersonView = value;
        if (MySession.Static != null && !MySession.Static.Enable3RdPersonView)
          this.m_isInFirstPersonView = true;
        if (this.m_isInFirstPersonView == inFirstPersonView || this.ForceFirstPersonCamera)
          return;
        this.UpdateCameraAfterChange(true);
      }
    }

    public override bool ForceFirstPersonCamera
    {
      get => (base.ForceFirstPersonCamera || MyThirdPersonSpectator.Static.IsCameraForced()) && this.m_forcedFpsTimeoutMs <= 0;
      set
      {
        if (value && !base.ForceFirstPersonCamera)
          this.m_forcedFpsTimeoutMs = 500;
        base.ForceFirstPersonCamera = value;
      }
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public float OxygenFillLevel
    {
      get => (float) this.m_oxygenFillLevel;
      private set => this.m_oxygenFillLevel.Value = MathHelper.Clamp(value, 0.0f, 1f);
    }

    float Sandbox.ModAPI.Ingame.IMyCockpit.OxygenFilledRatio => this.OxygenFillLevel;

    public float OxygenAmount
    {
      get => this.OxygenFillLevel * this.BlockDefinition.OxygenCapacity;
      set
      {
        if ((double) this.BlockDefinition.OxygenCapacity != 0.0)
          this.ChangeGasFillLevel(MathHelper.Clamp(value / this.BlockDefinition.OxygenCapacity, 0.0f, 1f));
        this.ResourceSink.Update();
      }
    }

    public bool CanPressurizeRoom => false;

    float Sandbox.ModAPI.Ingame.IMyCockpit.OxygenCapacity => this.BlockDefinition.OxygenCapacity;

    public float OxygenAmountMissing => (1f - this.OxygenFillLevel) * this.BlockDefinition.OxygenCapacity;

    public MyCockpit()
    {
      this.m_pilotClosedHandler = new Action<MyEntity>(this.m_pilot_OnMarkForClose);
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.EmitterMethods[1].Add((Delegate) new Func<bool>(this.ShouldPlay2D));
    }

    public override void InitComponents()
    {
      this.ResourceSink = new MyResourceSinkComponent(2);
      this.Render = (MyRenderComponentBase) new MyRenderComponentCockpit((MyEntity) this);
      base.InitComponents();
    }

    private bool ShouldPlay2D() => MySession.Static.LocalCharacter != null && this.Pilot == MySession.Static.LocalCharacter;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_isLargeCockpit = MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId()).CubeSize == MyCubeSize.Large;
      this.m_cockpitInteriorModel = this.BlockDefinition.InteriorModel;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyCockpit_IsWorkingChanged);
      if (this.m_cockpitInteriorModel == null)
        this.Render = (MyRenderComponentBase) new MyRenderComponentCockpit((MyEntity) this);
      base.Init(objectBuilder, cubeGrid);
      this.PostBaseInit();
      MyObjectBuilder_Cockpit objectBuilderCockpit = (MyObjectBuilder_Cockpit) objectBuilder;
      if (objectBuilderCockpit.Pilot != null)
      {
        this.m_pilotJetpackEnabledBackup = objectBuilderCockpit.PilotJetpackEnabled;
        MyEntity entity;
        MyCharacter myCharacter;
        if (MyEntities.TryGetEntityById(objectBuilderCockpit.Pilot.EntityId, out entity))
        {
          myCharacter = (MyCharacter) entity;
          if (myCharacter.IsUsing is MyShipController && myCharacter.IsUsing != this)
            myCharacter = (MyCharacter) null;
        }
        else
          myCharacter = (MyCharacter) MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) objectBuilderCockpit.Pilot, this.Render.FadeIn);
        if (myCharacter != null)
        {
          this.m_savedPilot = myCharacter;
          this.m_defferAttach = true;
          this.m_singleWeaponMode = objectBuilderCockpit.UseSingleWeaponMode;
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
            this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
        }
        this.IsInFirstPersonView = objectBuilderCockpit.IsInFirstPersonView;
      }
      if (objectBuilderCockpit.Autopilot != null)
      {
        MyAutopilotBase autopilot = MyAutopilotFactory.CreateAutopilot(objectBuilderCockpit.Autopilot);
        autopilot.Init(objectBuilderCockpit.Autopilot);
        Action<MyEntity> delayedAttachAutopilot = (Action<MyEntity>) null;
        delayedAttachAutopilot = (Action<MyEntity>) (x =>
        {
          this.AttachAutopilot(autopilot, false);
          this.AddedToScene -= delayedAttachAutopilot;
        });
        this.AddedToScene += delayedAttachAutopilot;
      }
      SerializableDefinitionId? pilotGunDefinition = objectBuilderCockpit.PilotGunDefinition;
      this.m_pilotGunDefinition = pilotGunDefinition.HasValue ? new MyDefinitionId?((MyDefinitionId) pilotGunDefinition.GetValueOrDefault()) : new MyDefinitionId?();
      Matrix? nullable;
      if (!objectBuilderCockpit.PilotRelativeWorld.HasValue)
      {
        nullable = new Matrix?();
      }
      else
      {
        MatrixD matrix = objectBuilderCockpit.PilotRelativeWorld.Value.GetMatrix();
        nullable = new Matrix?((Matrix) ref matrix);
      }
      this.m_pilotRelativeWorld = nullable;
      if (this.m_pilotGunDefinition.HasValue && this.m_pilotGunDefinition.Value.TypeId == typeof (MyObjectBuilder_AutomaticRifle) && string.IsNullOrEmpty(this.m_pilotGunDefinition.Value.SubtypeName))
        this.m_pilotGunDefinition = new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AutomaticRifle), "RifleGun"));
      if (!string.IsNullOrEmpty(this.m_cockpitInteriorModel))
      {
        if (MyModels.GetModelOnlyDummies(this.m_cockpitInteriorModel).Dummies.ContainsKey("head"))
          this.m_headLocalPosition = MyModels.GetModelOnlyDummies(this.m_cockpitInteriorModel).Dummies["head"].Matrix.Translation;
      }
      else if (MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies.ContainsKey("head"))
        this.m_headLocalPosition = MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies["head"].Matrix.Translation;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentCockpit(this));
      this.InitializeConveyorEndpoint();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
      this.m_oxygenFillLevel.SetLocalValue(objectBuilderCockpit.OxygenLevel);
      List<MyResourceSinkInfo> resourceSinkInfoList = new List<MyResourceSinkInfo>();
      MyResourceSinkInfo resourceSinkInfo = new MyResourceSinkInfo();
      resourceSinkInfo.ResourceTypeId = MyResourceDistributorComponent.ElectricityId;
      resourceSinkInfo.MaxRequiredInput = 0.0f;
      resourceSinkInfo.RequiredInputFunc = new Func<float>(this.CalculateRequiredPowerInput);
      resourceSinkInfoList.Add(resourceSinkInfo);
      resourceSinkInfo = new MyResourceSinkInfo();
      resourceSinkInfo.ResourceTypeId = MyCharacterOxygenComponent.OxygenId;
      resourceSinkInfo.MaxRequiredInput = this.BlockDefinition.OxygenCapacity;
      resourceSinkInfo.RequiredInputFunc = new Func<float>(this.ComputeRequiredGas);
      resourceSinkInfoList.Add(resourceSinkInfo);
      List<MyResourceSinkInfo> sinkData = resourceSinkInfoList;
      this.ResourceSink.Init(MyStringHash.GetOrCompute("Utility"), sinkData);
      this.ResourceSink.CurrentInputChanged += new MyCurrentResourceInputChangedDelegate(this.Sink_CurrentInputChanged);
      this.m_lastGasInputUpdateTick = MySession.Static.ElapsedGameTime.Ticks;
      if (MyEntityExtensions.GetInventory(this) == null && this.BlockDefinition.HasInventory)
      {
        Vector3 size = Vector3.One * 1f;
        this.Components.Add<MyInventoryBase>((MyInventoryBase) new MyInventory(size.Volume, size, MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend));
      }
      if (this.BlockDefinition.ScreenAreas == null || this.BlockDefinition.ScreenAreas.Count <= 0)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.BlockDefinition.ScreenAreas, objectBuilderCockpit.TextPanels);
      this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
    }

    private void MyCockpit_IsWorkingChanged(MyCubeBlock obj) => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void CreateTerminalControls()
    {
      base.CreateTerminalControls();
      if (MyTerminalControlFactory.AreControlsCreated<MyCockpit>())
        return;
      MyMultiTextPanelComponent.CreateTerminalControls<MyCockpit>();
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

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, int, int[]>(this, (Func<MyCockpit, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 425)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.RemoveItems(panelIndex, selection);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, int, int[]>(this, (Func<MyCockpit, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    [Event(null, 436)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.SelectItems(panelIndex, selection);

    private void ChangeTextRequest(int panelIndex, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, int, string>(this, (Func<MyCockpit, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 447)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, int, MySerializableSpriteCollection>(this, (Func<MyCockpit, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 461)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

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
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBox);
      }
    }

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, bool, bool, ulong, bool>(this, (Func<MyCockpit, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 484)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, bool, bool, ulong, bool>(this, (Func<MyCockpit, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    [Event(null, 495)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sandbox.Game.Multiplayer.Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBox = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBox == null)
        return;
      if (this.m_textBox.Description.Text.Length > 100000)
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
        this.m_textBox.Description.Text.Remove(100000, this.m_textBox.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBox.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBox);
      }
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBox.Description.Text, isPublic);
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
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, string, bool>(this, (Func<MyCockpit, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 588)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.PanelComponent.Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected virtual void PostBaseInit() => this.TryGetDummies();

    private float CalculateRequiredPowerInput() => this.IsFunctional && this.BlockDefinition.EnableShipControl && this.CubeGrid.GridSystems.ResourceDistributor.ResourceState != MyResourceStateEnum.NoPower ? 3f / 1000f : 0.0f;

    private float ComputeRequiredGas() => !this.IsWorking ? 0.0f : Math.Min((float) ((double) this.OxygenAmountMissing * 60.0 / 100.0), this.ResourceSink.MaxRequiredInputByType(MyCharacterOxygenComponent.OxygenId) * 0.1f);

    protected override void ComponentStack_IsFunctionalChanged()
    {
      base.ComponentStack_IsFunctionalChanged();
      if (!this.IsFunctional)
      {
        if (this.m_pilot != null)
          this.RemovePilot();
        this.ChangeGasFillLevel(0.0f);
        this.ResourceSink.Update();
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Cockpit builderCubeBlock = (MyObjectBuilder_Cockpit) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Pilot = this.m_pilot == null || !this.m_pilot.Save ? (this.m_savedPilot == null || !this.m_savedPilot.Save ? (MyObjectBuilder_Character) null : (MyObjectBuilder_Character) this.m_savedPilot.GetObjectBuilder(copy)) : (MyObjectBuilder_Character) this.m_pilot.GetObjectBuilder(copy);
      builderCubeBlock.PilotJetpackEnabled = builderCubeBlock.Pilot != null ? this.m_pilotJetpackEnabledBackup : new bool?();
      builderCubeBlock.Autopilot = this.m_aiPilot != null ? this.m_aiPilot.GetObjectBuilder() : (MyObjectBuilder_AutopilotBase) null;
      MyObjectBuilder_Cockpit objectBuilderCockpit1 = builderCubeBlock;
      MyDefinitionId? pilotGunDefinition = this.m_pilotGunDefinition;
      SerializableDefinitionId? nullable1 = pilotGunDefinition.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) pilotGunDefinition.GetValueOrDefault()) : new SerializableDefinitionId?();
      objectBuilderCockpit1.PilotGunDefinition = nullable1;
      if (this.m_pilotRelativeWorld.HasValue)
      {
        MyObjectBuilder_Cockpit objectBuilderCockpit2 = builderCubeBlock;
        Matrix matrix = this.m_pilotRelativeWorld.Value;
        MyPositionAndOrientation? nullable2 = new MyPositionAndOrientation?(new MyPositionAndOrientation((MatrixD) ref matrix));
        objectBuilderCockpit2.PilotRelativeWorld = nullable2;
      }
      else
        builderCubeBlock.PilotRelativeWorld = new MyPositionAndOrientation?();
      builderCubeBlock.IsInFirstPersonView = this.IsInFirstPersonView;
      builderCubeBlock.OxygenLevel = this.OxygenFillLevel;
      builderCubeBlock.TextPanels = this.m_multiPanel != null ? this.m_multiPanel.Serialize() : (List<MySerializedTextPanelData>) null;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public override MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceHeadAnim = false,
      bool forceHeadBone = false)
    {
      MatrixD matrixD1 = this.PositionComp.WorldMatrixRef;
      float degrees = this.m_headLocalXAngle;
      float headLocalYangle = this.m_headLocalYAngle;
      if (!includeX)
        degrees = this.DEFAULT_FPS_CAMERA_X_ANGLE;
      MatrixD fromAxisAngle = MatrixD.CreateFromAxisAngle(Vector3D.Right, (double) MathHelper.ToRadians(degrees));
      if (includeY)
        fromAxisAngle *= Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(headLocalYangle));
      MatrixD matrixD2 = fromAxisAngle * this.m_cameraDummy * matrixD1;
      Vector3D vector3D = matrixD2.Translation;
      if (this.m_headLocalPosition != Vector3.Zero)
        vector3D = Vector3D.Transform(this.m_headLocalPosition + this.m_playerHeadSpring, this.PositionComp.WorldMatrixRef);
      else if (this.Pilot != null)
        vector3D = this.Pilot.GetHeadMatrix(includeY, includeX, true, true, true).Translation;
      matrixD2.Translation = vector3D;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
      {
        MyRenderProxy.DebugDrawSphere(matrixD2.Translation, 0.05f, Color.Yellow, depthRead: false);
        MyRenderProxy.DebugDrawText3D(matrixD2.Translation, "Cockpit camera", Color.Yellow, 0.5f, false);
      }
      MatrixD matrixD3 = matrixD2;
      matrixD3.Translation = vector3D;
      return matrixD3;
    }

    public override float HeadLocalXAngle
    {
      get => this.m_headLocalXAngle;
      set => this.m_headLocalXAngle = value;
    }

    public override float HeadLocalYAngle
    {
      get => this.m_headLocalYAngle;
      set => this.m_headLocalYAngle = value;
    }

    public void Rotate(Vector2 rotationIndicator, float roll)
    {
      float num = MyInput.Static.GetMouseSensitivity() * 0.13f;
      if ((double) rotationIndicator.X != 0.0)
        this.m_headLocalXAngle = MathHelper.Clamp(this.m_headLocalXAngle - rotationIndicator.X * num, this.MinHeadLocalXAngle, this.MaxHeadLocalXAngle);
      if ((double) rotationIndicator.Y != 0.0)
      {
        if ((double) this.MinHeadLocalYAngle != 0.0 & this.IsInFirstPersonView)
          this.m_headLocalYAngle = MathHelper.Clamp(this.m_headLocalYAngle - rotationIndicator.Y * num, this.MinHeadLocalYAngle, this.MaxHeadLocalYAngle);
        else
          this.m_headLocalYAngle -= rotationIndicator.Y * num;
      }
      if (!this.IsInFirstPersonView)
        MyThirdPersonSpectator.Static.Rotate(rotationIndicator, roll);
      rotationIndicator = Vector2.Zero;
    }

    public void RotateStopped() => this.MoveAndRotateStopped();

    public void OnAssumeControl(IMyCameraController previousCameraController)
    {
      MyHud.SetHudDefinition(this.BlockDefinition.HUD);
      this.UpdateCameraAfterChange(true);
    }

    public override MatrixD GetViewMatrix()
    {
      if (!this.IsInFirstPersonView & !this.ForceFirstPersonCamera)
        return MyThirdPersonSpectator.Static.GetViewMatrix();
      MatrixD headMatrix = this.GetHeadMatrix(this.IsInFirstPersonView || this.ForceFirstPersonCamera, this.IsInFirstPersonView || this.ForceFirstPersonCamera, false, false);
      MatrixD result;
      MatrixD.Invert(ref headMatrix, out result);
      return result;
    }

    public static event Action OnPilotAttached;

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.m_updateSink)
      {
        this.ResourceSink.Update();
        this.m_updateSink = false;
      }
      if (this.m_savedPilot != null && !this.MarkedForClose && (!this.Closed && !this.m_savedPilot.MarkedForClose) && !this.m_savedPilot.Closed)
      {
        if ((this.m_savedPilot.NeedsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != MyEntityUpdateEnum.NONE)
        {
          this.m_savedPilot.UpdateOnceBeforeFrame();
          this.m_savedPilot.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
          MySession.Static.Players.UpdatePlayerControllers(this.EntityId);
          MySession.Static.Players.UpdatePlayerControllers(this.m_savedPilot.EntityId);
        }
        this.AttachPilot(this.m_savedPilot, false, true);
      }
      this.m_savedPilot = (MyCharacter) null;
      this.m_defferAttach = false;
      this.UpdateScreen();
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && (double) this.ResourceSink.RequiredInput > 0.0 && this.ResourceSink.IsPowered;

    public override void CheckEmissiveState(bool force = false)
    {
    }

    public void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.m_multiPanel?.Reset();
      if (this.ResourceSink != null)
        this.UpdateScreen();
      if (!this.CheckIsWorking())
        return;
      ((MyRenderComponentScreenAreas) this.Render).UpdateModelProperties();
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.m_soundEmitter != null && (double) this.m_soundEmitter.VolumeMultiplier < 1.0)
        this.m_soundEmitter.VolumeMultiplier = Math.Min(1f, this.m_soundEmitter.VolumeMultiplier + 0.005f);
      if (this.m_forcedFpsTimeoutMs > 0)
        this.m_forcedFpsTimeoutMs -= 16;
      if (this.m_pilot != null && this.ControllerInfo.IsLocallyHumanControlled() && this.CubeGrid.Physics != null)
      {
        float num1 = this.CubeGrid.Physics.LinearAcceleration.Length();
        float num2 = this.CubeGrid.Physics.LinearVelocity.Length();
        if ((double) num1 > 0.0 && (double) num2 > 0.0)
          this.AddShake(this.MAX_SHAKE * MathHelper.Clamp((float) (((double) Vector3.Dot(Vector3.Normalize(this.CubeGrid.Physics.LinearVelocity), Vector3.Normalize(this.CubeGrid.Physics.LinearAcceleration)) * (double) num1 - (double) this.MIN_SHAKE_ACC) / ((double) this.MAX_SHAKE_ACC - (double) this.MIN_SHAKE_ACC)), 0.0f, 1f));
      }
      bool flag = !this.IsInFirstPersonView && this.ForceFirstPersonCamera;
      if (this.m_wasCameraForced != flag)
        this.UpdateCameraAfterChange(false);
      this.m_wasCameraForced = flag;
      if (!MyDebugDrawSettings.DEBUG_DRAW_COCKPIT || !this.m_pilotRelativeWorld.HasValue)
        return;
      Matrix matrix1 = this.m_pilotRelativeWorld.Value;
      MatrixD matrix2 = MatrixD.Multiply((MatrixD) ref matrix1, this.WorldMatrix);
      if (this.m_lastPilot == null || this.m_lastPilot.Physics == null || this.m_lastPilot.Physics.CharacterProxy == null)
        return;
      int shapeIndex = 0;
      HkShape collisionShape = this.m_lastPilot.Physics.CharacterProxy.GetCollisionShape();
      Vector3D translation1 = matrix2.Translation;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix2);
      Vector3D vector3D = Vector3D.TransformNormal(this.m_lastPilot.Physics.Center, matrix2);
      Vector3D translation2 = translation1 + vector3D;
      matrix2.Translation += vector3D;
      MyPhysicsDebugDraw.DrawCollisionShape(collisionShape, matrix2, 1f, ref shapeIndex, "Pilot");
      List<HkBodyCollision> results = new List<HkBodyCollision>();
      MyPhysics.GetPenetrationsShape(collisionShape, ref translation2, ref fromRotationMatrix, results, 18);
      foreach (HkBodyCollision collision in results)
      {
        VRage.ModAPI.IMyEntity collisionEntity = collision.GetCollisionEntity();
        if (collisionEntity != null && collisionEntity.Physics != null && !collisionEntity.Physics.IsPhantom)
          MyRenderProxy.DebugDrawArrow3D(matrix2.Translation, collisionEntity.PositionComp.GetPosition(), Color.Lime, text: collisionEntity.DisplayName, textSize: 0.6f);
      }
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (this.m_soundEmitter != null)
      {
        if (this.hasPower && this.m_playIdleSound && (!this.m_soundEmitter.IsPlaying || !this.m_soundEmitter.SoundPair.Equals((object) this.m_baseIdleSound) && !this.m_soundEmitter.SoundPair.Equals((object) this.GetInCockpitSound)) && !this.m_baseIdleSound.Equals((object) MySoundPair.Empty))
        {
          this.m_soundEmitter.VolumeMultiplier = 0.0f;
          this.m_soundEmitter.PlaySound(this.m_baseIdleSound, true);
        }
        else if ((!this.hasPower || !this.IsWorking) && (this.m_soundEmitter.IsPlaying && this.m_soundEmitter.SoundPair.Equals((object) this.m_baseIdleSound)))
          this.m_soundEmitter.StopSound(true);
      }
      if (this.GridResourceDistributor == null || this.GridGyroSystem == null || this.EntityThrustComponent == null)
        return;
      bool flag1 = false;
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      if (entityThrustComponent != null)
        flag1 = entityThrustComponent.AutopilotEnabled;
      bool flag2 = this.CubeGrid.GridSystems.ControlSystem.IsControlled | flag1;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (!flag2 && this.m_aiPilot != null)
          this.m_aiPilot.Update();
        else if (flag2 && this.m_aiPilot != null && this.m_aiPilot.RemoveOnPlayerControl)
          this.RemoveAutopilot();
      }
      if (this.m_pilot == null || !this.ControllerInfo.IsLocallyHumanControlled())
        return;
      this.m_pilot.RadioReceiver.UpdateHud();
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_pilot != null && ((double) this.OxygenFillLevel < 0.200000002980232 && this.CubeGrid.GridSizeEnum == MyCubeSize.Small))
        this.RefillFromBottlesOnGrid();
      this.ResourceSink.Update();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        TimeSpan elapsedPlayTime = MySession.Static.ElapsedPlayTime;
        float num = (float) (elapsedPlayTime.Ticks - this.m_lastGasInputUpdateTick) / 1E+07f;
        elapsedPlayTime = MySession.Static.ElapsedPlayTime;
        this.m_lastGasInputUpdateTick = elapsedPlayTime.Ticks;
        this.ChangeGasFillLevel(this.OxygenFillLevel + this.ResourceSink.CurrentInputByType(MyCharacterOxygenComponent.OxygenId) * num);
        if (this.BlockDefinition.IsPressurized)
        {
          float oxygenInPoint = MyOxygenProviderSystem.GetOxygenInPoint(this.CubeGrid.GridIntegerToWorld(this.Position));
          if ((double) this.OxygenFillLevel < (double) oxygenInPoint)
            this.ChangeGasFillLevel(oxygenInPoint);
        }
      }
      if (!this.m_retryAttachPilot)
        return;
      if (this.m_serverSidePilotId != 0L)
        this.TryAttachPilot(this.m_serverSidePilotId);
      else
        this.m_retryAttachPilot = false;
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.m_multiPanel?.UpdateAfterSimulation(this.IsWorking);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.Pilot != MySession.Static.LocalCharacter || !this.CubeGrid.IsRespawnGrid)
        return;
      MyIngameHelpPod1.StartingInPod = true;
    }

    private void Sink_CurrentInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      this.OnInputChanged(resourceTypeId, oldInput, sink);
    }

    protected virtual void OnInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      this.UpdateIsWorking();
      if (resourceTypeId != MyCharacterOxygenComponent.OxygenId)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
      else
      {
        float num = (float) (MySession.Static.ElapsedPlayTime.Ticks - this.m_lastGasInputUpdateTick) / 1E+07f;
        this.m_lastGasInputUpdateTick = MySession.Static.ElapsedPlayTime.Ticks;
        this.ChangeGasFillLevel(this.OxygenFillLevel + oldInput * num);
        this.m_updateSink = true;
      }
    }

    private void RefillFromBottlesOnGrid()
    {
      List<IMyConveyorEndpoint> reachableVertices = new List<IMyConveyorEndpoint>();
      MyGridConveyorSystem.FindReachable(this.ConveyorEndpoint, reachableVertices, (Predicate<IMyConveyorEndpoint>) (vertex => vertex.CubeBlock != null && this.FriendlyWithBlock(vertex.CubeBlock) && vertex.CubeBlock.HasInventory));
      bool flag = false;
      foreach (IMyConveyorEndpoint conveyorEndpoint in reachableVertices)
      {
        MyCubeBlock cubeBlock = conveyorEndpoint.CubeBlock;
        int inventoryCount = cubeBlock.InventoryCount;
        for (int index = 0; index < inventoryCount; ++index)
        {
          foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(cubeBlock, index).GetItems())
          {
            if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content && (double) content.GasLevel != 0.0)
            {
              MyOxygenContainerDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) content) as MyOxygenContainerDefinition;
              if (!(physicalItemDefinition.StoredGasId != MyCharacterOxygenComponent.OxygenId))
              {
                float val1 = content.GasLevel * physicalItemDefinition.Capacity;
                float num = Math.Min(val1, this.OxygenAmountMissing);
                if ((double) num != 0.0)
                {
                  content.GasLevel = (val1 - num) / physicalItemDefinition.Capacity;
                  if ((double) content.GasLevel < 0.0)
                    content.GasLevel = 0.0f;
                  double gasLevel = (double) content.GasLevel;
                  flag = true;
                  this.OxygenAmount += num;
                  if ((double) this.OxygenFillLevel >= 1.0)
                  {
                    this.ChangeGasFillLevel(1f);
                    this.ResourceSink.Update();
                    break;
                  }
                }
              }
            }
          }
        }
      }
      if (!flag)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationBottleRefill, level: MyNotificationLevel.Important));
    }

    public override void ShowInventory() => MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, this.m_pilot, (MyEntity) this);

    public override void ShowTerminal()
    {
      if (!this.CubeGrid.InScene)
        return;
      MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, this.m_pilot, (MyEntity) this);
    }

    public override void OnRemovedFromScene(object source)
    {
      if (!MyEntities.CloseAllowed)
      {
        this.m_savedPilot = this.m_pilot;
        this.RemovePilot();
      }
      base.OnRemovedFromScene(source);
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this), true);
      base.OnDestroy();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_savedPilot != null || this.m_multiPanel != null && this.m_multiPanel.SurfaceCount > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene(new int?(this.BlockDefinition.InteriorModel != null ? 1 : 0));
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SetRender((MyRenderComponentScreenAreas) null);
    }

    protected override void OnControlReleased(MyEntityController controller)
    {
      if (this.m_pilot == null || this.m_pilot != null && !MySessionComponentReplay.Static.HasEntityReplayData(this.CubeGrid.EntityId))
        base.OnControlReleased(controller);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    private void m_pilot_OnMarkForClose(MyEntity obj)
    {
      if (this.m_pilot == null)
        return;
      this.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) this.m_pilot);
      this.m_rechargeSocket.Unplug();
      this.m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = (VRage.ModAPI.IMyEntity) null;
      this.m_pilot = (MyCharacter) null;
    }

    public void GiveControlToPilot()
    {
      MyCharacter entity = this.m_pilot ?? this.m_savedPilot;
      if (entity.ControllerInfo == null || entity.ControllerInfo.Controller == null)
        return;
      entity.SwitchControl((IMyControllableEntity) this);
    }

    public bool RemovePilot()
    {
      if (this.m_pilot == null)
        return true;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.CubeGrid.IsBlockTrasferInProgress)
      {
        this.m_serverSidePilotId = 0L;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, long, Matrix?>(this, (Func<MyCockpit, Action<long, Matrix?>>) (x => new Action<long, Matrix?>(x.NotifyClientPilotChanged)), this.m_serverSidePilotId, this.m_pilotRelativeWorld);
      }
      if (this.m_pilot.Physics == null)
      {
        this.m_pilot = (MyCharacter) null;
        return true;
      }
      this.StopLoopSound();
      this.m_pilot.OnMarkForClose -= this.m_pilotClosedHandler;
      if (MyVisualScriptLogicProvider.PlayerLeftCockpit != null)
        MyVisualScriptLogicProvider.PlayerLeftCockpit(this.Name, this.m_pilot.GetPlayerIdentityId(), this.CubeGrid.Name);
      this.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) this.m_pilot);
      this.NeedsWorldMatrix = false;
      this.InvalidateOnMove = this.NeedsWorldMatrix;
      if (this.m_pilot.IsDead)
      {
        if (this.ControllerInfo.Controller != null)
          this.SwitchControl((IMyControllableEntity) this.m_pilot);
        MyEntities.Add((MyEntity) this.m_pilot);
        this.m_pilot.WorldMatrix = this.WorldMatrix;
        this.m_pilotGunDefinition = new MyDefinitionId?();
        this.m_rechargeSocket.Unplug();
        this.m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = (VRage.ModAPI.IMyEntity) null;
        if (this.m_pilot == MySession.Static.LocalCharacter)
          MyLocalCache.LoadInventoryConfig(this.m_pilot, false);
        this.m_pilot = (MyCharacter) null;
        return true;
      }
      bool flag = false;
      MatrixD worldMatrix1 = MatrixD.Identity;
      if (this.m_pilotRelativeWorld.HasValue)
      {
        Vector3D to = Vector3D.Transform(this.Position * this.CubeGrid.GridSize, this.CubeGrid.WorldMatrix);
        Matrix matrix = this.m_pilotRelativeWorld.Value;
        worldMatrix1 = MatrixD.Multiply((MatrixD) ref matrix, this.WorldMatrix);
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(worldMatrix1.Translation, to, 15);
        if (nullable.HasValue)
        {
          if (this.CubeGrid.Equals((object) nullable.Value.HkHitInfo.GetHitEntity()) && this.m_pilot.CanPlaceCharacter(ref worldMatrix1))
            flag = true;
        }
        else if (this.m_pilot.CanPlaceCharacter(ref worldMatrix1))
          flag = true;
      }
      Vector3D? nullable1 = new Vector3D?();
      if (!flag)
      {
        nullable1 = this.FindFreeNeighbourPosition();
        if (!nullable1.HasValue)
          nullable1 = new Vector3D?(this.PositionComp.GetPosition());
      }
      this.RemovePilotFromSeat(this.m_pilot);
      this.EndShootAll();
      this.CubeGrid.GridSystems.RadioSystem.Unregister((MyDataBroadcaster) this.m_pilot.RadioBroadcaster);
      this.CubeGrid.GridSystems.RadioSystem.Unregister((MyDataReceiver) this.m_pilot.RadioReceiver);
      MyIdentity identity = this.m_pilot.GetIdentity();
      if (identity != null)
        identity.FactionChanged -= new Action<MyFaction, MyFaction>(this.OnCharacterFactionChanged);
      if (this.CubeGrid.IsBlockTrasferInProgress)
      {
        MyCharacter pilot = this.m_pilot;
        this.m_pilot = (MyCharacter) null;
        if (this.ControllerInfo.Controller != null)
          this.SwitchControl((IMyControllableEntity) pilot);
      }
      else if (flag || nullable1.HasValue)
      {
        this.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) this.m_pilot);
        MatrixD worldMatrix2 = flag ? worldMatrix1 : MatrixD.CreateWorld(nullable1.Value - this.WorldMatrix.Up, this.WorldMatrix.Forward, this.WorldMatrix.Up);
        if (!MyEntities.CloseAllowed)
          this.m_pilot.PositionComp.SetWorldMatrix(ref worldMatrix2, (object) this);
        MyEntities.Add((MyEntity) this.m_pilot);
        this.m_pilot.Physics.Enabled = true;
        this.m_rechargeSocket.Unplug();
        this.m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = (VRage.ModAPI.IMyEntity) null;
        this.m_pilot.Stand();
        if (this.m_pilotJetpackEnabledBackup.HasValue && this.m_pilot.JetpackComp != null)
          this.m_pilot.JetpackComp.TurnOnJetpack(this.m_pilotJetpackEnabledBackup.Value);
        if (this.Parent != null && this.Parent.Physics != null)
        {
          MyEntity entityById = MyEntities.GetEntityById(this.m_pilot.ClosestParentId);
          if (entityById != null && !Sandbox.Game.Multiplayer.Sync.IsServer)
            this.m_pilot.Physics.LinearVelocity = entityById.Physics.LinearVelocity - this.Parent.Physics.LinearVelocity;
          else
            this.m_pilot.Physics.LinearVelocity = this.Parent.Physics.LinearVelocity;
          if ((double) this.Parent.Physics.LinearVelocity.LengthSquared() > 100.0)
          {
            MyCharacterJetpackComponent jetpackComp = this.m_pilot.JetpackComp;
            if (jetpackComp != null)
            {
              jetpackComp.EnableDampeners(true);
              jetpackComp.TurnOnJetpack(true);
              this.m_pilot.RelativeDampeningEntity = (MyEntity) this.CubeGrid;
              if (Sandbox.Game.Multiplayer.Sync.IsServer)
                Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyPlayerCollection.SetDampeningEntityClient)), this.m_pilot.EntityId, this.m_pilot.RelativeDampeningEntity.EntityId);
            }
          }
        }
        MyCharacter pilot = this.m_pilot;
        this.m_pilot = (MyCharacter) null;
        if (this.ControllerInfo.Controller != null)
        {
          if (this.ControllerInfo.Controller.Player.IsLocalPlayer && pilot != null)
            pilot.RadioReceiver.Clear();
          this.SwitchControl((IMyControllableEntity) pilot);
        }
        if (this.m_pilotGunDefinition.HasValue)
          pilot.SwitchToWeapon(this.m_pilotGunDefinition, false);
        else
          pilot.SwitchToWeapon(new MyDefinitionId?(), false);
        if (pilot == MySession.Static.LocalCharacter)
          MyLocalCache.LoadInventoryConfig(pilot, false);
        if (MySession.Static.CameraController == this && pilot == MySession.Static.LocalCharacter)
        {
          int num = this.IsInFirstPersonView ? 1 : 0;
          MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) pilot, new Vector3D?());
        }
        pilot.IsInFirstPersonView = this.m_pilotFirstPerson;
        this.CheckEmissiveState(false);
        return true;
      }
      this.CheckEmissiveState(false);
      return false;
    }

    public void RequestRemovePilot() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit>(this, (Func<MyCockpit, Action>) (x => new Action(this.OnRequestRemovePilot)));

    [Event(null, 1426)]
    [Reliable]
    [Server(ValidationType.IgnoreDLC)]
    public void OnRequestRemovePilot()
    {
      if (!MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
        return;
      this.RemovePilot();
    }

    protected virtual void RemovePilotFromSeat(MyCharacter pilot)
    {
      this.CubeGrid.UnregisterOccupiedBlock(this);
      this.CubeGrid.SetInventoryMassDirty();
    }

    public void AttachAutopilot(MyAutopilotBase newAutopilot, bool updateSync = true)
    {
      this.RemoveAutopilot(false);
      this.m_aiPilot = newAutopilot;
      this.m_aiPilot.OnAttachedToShipController(this);
      if (updateSync && Sandbox.Game.Multiplayer.Sync.IsServer)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, MyObjectBuilder_AutopilotBase>(this, (Func<MyCockpit, Action<MyObjectBuilder_AutopilotBase>>) (x => new Action<MyObjectBuilder_AutopilotBase>(x.SetAutopilot_Client)), newAutopilot.GetObjectBuilder());
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    public void RemoveAutopilot(bool updateSync = true)
    {
      if (this.m_aiPilot != null)
      {
        this.m_aiPilot.OnRemovedFromCockpit();
        this.m_aiPilot = (MyAutopilotBase) null;
        if (updateSync && Sandbox.Game.Multiplayer.Sync.IsServer)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, MyObjectBuilder_AutopilotBase>(this, (Func<MyCockpit, Action<MyObjectBuilder_AutopilotBase>>) (x => new Action<MyObjectBuilder_AutopilotBase>(x.SetAutopilot_Client)), (MyObjectBuilder_AutopilotBase) null);
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.ControllerInfo.Controller != null && this.ControllerInfo.IsLocallyControlled() || this.m_multiPanel != null)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    public void RemoveOriginalPilotPosition() => this.m_pilotRelativeWorld = new Matrix?();

    public void OnReleaseControl(IMyCameraController newCameraController)
    {
      this.UpdateNearFlag();
      if (!this.m_enableFirstPerson)
        return;
      this.UpdateCockpitModel();
    }

    protected override void UpdateCameraAfterChange(bool resetHeadLocalAngle = true)
    {
      base.UpdateCameraAfterChange(resetHeadLocalAngle);
      if (resetHeadLocalAngle)
      {
        this.m_headLocalXAngle = 0.0f;
        this.m_headLocalYAngle = 0.0f;
      }
      this.UpdateNearFlag();
      if (this.m_enableFirstPerson)
      {
        this.UpdateCockpitModel();
      }
      else
      {
        if (!MySession.Static.IsCameraControlledObject() || !MySession.Static.Settings.Enable3rdPersonView || (this.Pilot == null || !this.Pilot.ControllerInfo.IsLocallyControlled()))
          return;
        MySession.Static.SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, (VRage.ModAPI.IMyEntity) null, new Vector3D?());
      }
    }

    private void UpdateNearFlag()
    {
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.TryGetDummies();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public virtual void UpdateCockpitModel()
    {
      if (this.m_cockpitInteriorModel == null || !(this.Render is MyRenderComponentCockpit render) || (render.RenderObjectIDs.Length < 2 || render.ExteriorRenderId == uint.MaxValue) || render.InteriorRenderId == uint.MaxValue)
        return;
      if ((MySession.Static.CameraController != this ? 0 : (this.IsInFirstPersonView ? 1 : (this.ForceFirstPersonCamera ? 1 : 0))) != 0)
      {
        MyRenderProxy.UpdateRenderObjectVisibility(render.ExteriorRenderId, false, false);
        MyRenderProxy.UpdateRenderObjectVisibility(render.InteriorRenderId, render.Visible, false);
      }
      else
      {
        MyRenderProxy.UpdateRenderObjectVisibility(render.ExteriorRenderId, render.Visible, false);
        MyRenderProxy.UpdateRenderObjectVisibility(render.InteriorRenderId, false, false);
      }
    }

    public Vector3I[] NeighbourPositions => this.m_neighbourPositions;

    protected Vector3D? FindFreeNeighbourPosition()
    {
      int num1 = 512;
      int num2 = 1;
      for (; num1 > 0; --num1)
      {
        foreach (Vector3I neighbourPosition in this.m_neighbourPositions)
        {
          Vector3D translation;
          if (this.IsNeighbourPositionFree(neighbourPosition * num2, out translation))
            return new Vector3D?(translation);
        }
        ++num2;
      }
      return new Vector3D?();
    }

    public bool IsNeighbourPositionFree(Vector3I neighbourOffsetI, out Vector3D translation)
    {
      Vector3D vector3D = 0.5 * (double) this.PositionComp.LocalAABB.Size.X * (double) neighbourOffsetI.X * this.PositionComp.WorldMatrixRef.Right + 0.5 * (double) this.PositionComp.LocalAABB.Size.Y * (double) neighbourOffsetI.Y * this.PositionComp.WorldMatrixRef.Up - 0.5 * (double) this.PositionComp.LocalAABB.Size.Z * (double) neighbourOffsetI.Z * this.PositionComp.WorldMatrixRef.Forward + (0.899999976158142 * (double) neighbourOffsetI.X * this.PositionComp.WorldMatrixRef.Right + 0.899999976158142 * (double) neighbourOffsetI.Y * this.PositionComp.WorldMatrixRef.Up - 0.899999976158142 * (double) neighbourOffsetI.Z * this.PositionComp.WorldMatrixRef.Forward);
      MatrixD matrixD = this.PositionComp.WorldMatrixRef;
      Vector3D position = matrixD.Translation + vector3D;
      matrixD = this.PositionComp.WorldMatrixRef;
      Vector3D forward = matrixD.Forward;
      matrixD = this.PositionComp.WorldMatrixRef;
      Vector3D up = matrixD.Up;
      MatrixD world = MatrixD.CreateWorld(position, forward, up);
      translation = world.Translation;
      return this.m_pilot.CanPlaceCharacter(ref world, true, true);
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      if (this.m_defferAttach || this.m_savedPilot == null)
        return;
      this.AttachPilot(this.m_savedPilot, false, merged: true);
      this.m_savedPilot = (MyCharacter) null;
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      if (this.m_pilot == null)
        return;
      MyCharacter pilot = this.m_pilot;
      if (!MyEntities.CloseAllowed)
      {
        this.RemovePilot();
        pilot.DoDamage(1000f, MyDamageType.Destruction, false, 0L);
      }
      else
      {
        if (MySession.Static.CameraController != this)
          return;
        MySession.Static.SetCameraController(MySession.Static.GetCameraControllerEnum(), (VRage.ModAPI.IMyEntity) this.m_pilot, new Vector3D?());
      }
    }

    public void AttachPilot(
      MyCharacter pilot,
      bool storeOriginalPilotWorld = true,
      bool calledFromInit = false,
      bool merged = false)
    {
      if (!MyEntities.IsInsideWorld(pilot.PositionComp.GetPosition()) || !MyEntities.IsInsideWorld(this.PositionComp.GetPosition()))
        return;
      long playerIdentityId = pilot.GetPlayerIdentityId();
      this.m_pilot = pilot;
      this.m_pilot.OnMarkForClose += this.m_pilotClosedHandler;
      this.m_pilot.IsUsing = (MyEntity) this;
      this.m_pilot.ResetHeadRotation();
      int num = !merged ? 1 : 0;
      if (num != 0)
      {
        if (storeOriginalPilotWorld)
        {
          MatrixD matrixD = MatrixD.Multiply(pilot.WorldMatrix, this.PositionComp.WorldMatrixNormalizedInv);
          this.m_pilotRelativeWorld = new Matrix?((Matrix) ref matrixD);
        }
        else if (!calledFromInit)
          this.m_pilotRelativeWorld = new Matrix?();
      }
      if (pilot.InScene)
        MyEntities.Remove((MyEntity) pilot);
      MatrixD worldMatrix = this.WorldMatrix;
      this.m_pilot.Physics.Enabled = false;
      this.m_pilot.PositionComp.SetWorldMatrix(ref worldMatrix, (object) this, ignoreAssert: true);
      this.m_pilot.Physics.Clear();
      if (!this.Hierarchy.Children.Any<MyHierarchyComponentBase>((Func<MyHierarchyComponentBase, bool>) (x => x.Entity == this.m_pilot)))
        this.Hierarchy.AddChild((VRage.ModAPI.IMyEntity) this.m_pilot, true);
      this.NeedsWorldMatrix = true;
      if (num != 0)
      {
        this.m_pilotGunDefinition = !(this.m_pilot.CurrentWeapon is MyEntity) || MyCockpit.m_forgetTheseWeapons.Contains<MyDefinitionId>(this.m_pilot.CurrentWeapon.DefinitionId) ? new MyDefinitionId?() : new MyDefinitionId?(this.m_pilot.CurrentWeapon.DefinitionId);
        this.m_pilotFirstPerson = pilot.IsInFirstPersonView;
      }
      this.PlacePilotInSeat(pilot);
      this.m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = (VRage.ModAPI.IMyEntity) this;
      this.m_rechargeSocket.PlugIn(this.m_pilot.SuitBattery.ResourceSink);
      if (pilot.ControllerInfo.Controller != null)
        Sandbox.Game.Multiplayer.Sync.Players.SetPlayerToCockpit(pilot.ControllerInfo.Controller.Player, (MyEntity) this);
      if (!calledFromInit)
      {
        this.GiveControlToPilot();
        this.m_pilot.SwitchToWeapon((MyToolbarItemWeapon) null, new uint?(), false);
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_serverSidePilotId = this.m_pilot.EntityId;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, long, Matrix?>(this, (Func<MyCockpit, Action<long, Matrix?>>) (x => new Action<long, Matrix?>(x.NotifyClientPilotChanged)), this.m_serverSidePilotId, this.m_pilotRelativeWorld);
      }
      if (num != 0)
      {
        MyCharacterJetpackComponent jetpackComp = this.m_pilot.JetpackComp;
        if (jetpackComp != null && !calledFromInit)
          this.m_pilotJetpackEnabledBackup = new bool?(jetpackComp.TurnedOn);
      }
      if (this.m_pilot.JetpackComp != null)
        this.m_pilot.JetpackComp.TurnOnJetpack(false);
      this.m_lastPilot = pilot;
      if (this.GetInCockpitSound != MySoundPair.Empty && !calledFromInit && !merged)
        this.PlayUseSound(true);
      this.m_playIdleSound = true;
      if (pilot == MySession.Static.LocalCharacter && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
      {
        if (calledFromInit && (MySession.Static.CameraController == null || MySession.Static.CameraController == this))
          MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
        else if (MySession.Static.CameraController == pilot && pilot == MySession.Static.LocalCharacter)
          MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
      }
      string name1 = this.Name;
      if (string.IsNullOrWhiteSpace(name1))
        name1 = this.EntityId.ToString();
      string name2 = this.CubeGrid.Name;
      if (string.IsNullOrWhiteSpace(name2))
        name2 = this.CubeGrid.EntityId.ToString();
      if (MyVisualScriptLogicProvider.PlayerEnteredCockpit != null && playerIdentityId != -1L)
        MyVisualScriptLogicProvider.PlayerEnteredCockpit(name1, playerIdentityId, name2);
      if (this.m_pilot == MySession.Static.LocalCharacter)
        MyLocalCache.LoadInventoryConfig(pilot, false);
      this.CubeGrid.GridSystems.RadioSystem.Register((MyDataBroadcaster) this.m_pilot.RadioBroadcaster);
      this.CubeGrid.GridSystems.RadioSystem.Register((MyDataReceiver) this.m_pilot.RadioReceiver);
      MyIdentity identity = pilot.GetIdentity();
      if (identity != null)
        identity.FactionChanged += new Action<MyFaction, MyFaction>(this.OnCharacterFactionChanged);
      if (this.m_pilot != MySession.Static.LocalCharacter)
        return;
      MyCockpit.OnPilotAttached.InvokeIfNotNull();
    }

    protected virtual void PlacePilotInSeat(MyCharacter pilot)
    {
      this.m_pilot.Sit(this.m_enableFirstPerson, MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Identity.Character == pilot, false, this.BlockDefinition.CharacterAnimation);
      MatrixD worldMatrix = this.m_characterDummy * this.WorldMatrix;
      pilot.PositionComp.SetWorldMatrix(ref worldMatrix, (object) this);
      this.CubeGrid.RegisterOccupiedBlock(this);
      this.CubeGrid.SetInventoryMassDirty();
    }

    public void AddShake(float shakePower)
    {
      this.m_currentCameraShakePower += shakePower;
      this.m_currentCameraShakePower = Math.Min(this.m_currentCameraShakePower, MySector.MainCamera.CameraShake.MaxShake);
    }

    private void ChangeGasFillLevel(float newFillLevel)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || (double) this.OxygenFillLevel == (double) newFillLevel)
        return;
      this.OxygenFillLevel = newFillLevel;
      this.CheckEmissiveState(false);
    }

    public override bool IsLargeShip() => this.m_isLargeCockpit;

    public MyCockpitDefinition BlockDefinition => base.BlockDefinition as MyCockpitDefinition;

    public override string CalculateCurrentModel(out Matrix orientation)
    {
      this.Orientation.GetMatrix(out orientation);
      if (!this.Render.NearFlag)
        return this.BlockDefinition.Model;
      return !string.IsNullOrEmpty(this.m_cockpitInteriorModel) ? this.m_cockpitInteriorModel : this.BlockDefinition.Model;
    }

    private void TryGetDummies()
    {
      if (this.Model == null)
        return;
      MyModelDummy myModelDummy1;
      this.Model.Dummies.TryGetValue("camera", out myModelDummy1);
      if (myModelDummy1 != null)
        this.m_cameraDummy = MatrixD.Normalize((MatrixD) ref myModelDummy1.Matrix);
      MyModelDummy myModelDummy2;
      this.Model.Dummies.TryGetValue("character", out myModelDummy2);
      if (myModelDummy2 == null)
        return;
      this.m_characterDummy = MatrixD.Normalize((MatrixD) ref myModelDummy2.Matrix);
    }

    public override MyCharacter Pilot => this.m_pilot == null && this.m_savedPilot != null ? this.m_savedPilot : this.m_pilot;

    public MyEntity IsBeingUsedBy => (MyEntity) this.m_pilot;

    public virtual UseActionResult CanUse(
      UseActionEnum actionEnum,
      IMyControllableEntity user)
    {
      if (user == null || user is MyCharacter myCharacter && myCharacter.IsDead)
        return UseActionResult.AccessDenied;
      if (this.m_pilot != null && this.m_pilot.IsConnected(out bool _))
        return UseActionResult.UsedBySomeoneElse;
      if (this.MarkedForClose)
        return UseActionResult.Closed;
      if (!this.IsFunctional)
        return UseActionResult.CockpitDamaged;
      long controllingIdentityId = user.ControllerInfo.ControllingIdentityId;
      if (controllingIdentityId == 0L)
        return UseActionResult.AccessDenied;
      switch (this.HasPlayerAccessReason(controllingIdentityId))
      {
        case MyTerminalBlock.AccessRightsResult.Enemies:
        case MyTerminalBlock.AccessRightsResult.Other:
          return UseActionResult.AccessDenied;
        case MyTerminalBlock.AccessRightsResult.MissingDLC:
          return UseActionResult.MissingDLC;
        default:
          return UseActionResult.OK;
      }
    }

    protected override void UpdateSoundState() => base.UpdateSoundState();

    protected override void StartLoopSound()
    {
      this.m_playIdleSound = true;
      if (this.m_soundEmitter == null || !this.hasPower || this.m_baseIdleSound.SoundId.IsNull)
        return;
      this.m_soundEmitter.PlaySound(this.m_baseIdleSound, true);
    }

    protected override void StopLoopSound()
    {
      this.m_playIdleSound = false;
      if (this.m_soundEmitter == null || !this.m_soundEmitter.IsPlaying)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    protected override bool IsCameraController() => true;

    protected override void OnControlAcquired_UpdateCamera()
    {
      this.CubeGrid.RaiseGridChanged();
      base.OnControlAcquired_UpdateCamera();
      this.m_currentCameraShakePower = 0.0f;
    }

    protected override void OnControlledEntity_Used()
    {
      MyCharacter pilot = this.m_pilot;
      this.RemovePilot();
      base.OnControlledEntity_Used();
    }

    protected override void OnControlReleased_UpdateCamera()
    {
      base.OnControlReleased_UpdateCamera();
      this.m_currentCameraShakePower = 0.0f;
    }

    protected override void RemoveLocal()
    {
      if (MyCubeBuilder.Static.IsActivated)
        MySession.Static.GameFocusManager.Clear();
      base.RemoveLocal();
      this.RemovePilot();
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      this.CheckPilotRelation();
    }

    private void CheckPilotRelation()
    {
      if (this.m_pilot == null || !Sandbox.Game.Multiplayer.Sync.IsServer || this.ControllerInfo.Controller == null)
        return;
      switch (this.GetUserRelationToOwner(this.ControllerInfo.ControllingIdentityId))
      {
        case MyRelationsBetweenPlayerAndBlock.Neutral:
        case MyRelationsBetweenPlayerAndBlock.Enemies:
          this.RaiseControlledEntityUsed();
          break;
      }
    }

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      List<MyHudEntityParams> hudParams = base.GetHudParams(allowBlink);
      int num = this.ControllerInfo.ControllingIdentityId == (MySession.Static.LocalHumanPlayer == null ? 0L : MySession.Static.LocalHumanPlayer.Identity.IdentityId) ? 0 : (this.Pilot != null ? 1 : 0);
      if (this.ShowOnHUD || this.IsBeingHacked)
        hudParams[0].Text.AppendLine();
      else
        hudParams[0].Text.Clear();
      if (num != 0 && this.Pilot != null)
        hudParams[0].Text.Append((object) this.Pilot.UpdateCustomNameWithFaction());
      if (!this.ShowOnHUD)
        this.m_hudParams.Clear();
      return hudParams;
    }

    protected override bool ShouldSit() => this.m_isLargeCockpit || base.ShouldSit();

    protected override bool CanBeMainCockpit() => this.BlockDefinition.EnableShipControl;

    void IMyCameraController.ControlCamera(MyCamera currentCamera)
    {
      if (!this.m_enableFirstPerson)
        this.IsInFirstPersonView = false;
      if (this.Closed && MySession.Static.LocalCharacter != null)
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) MySession.Static.LocalCharacter, new Vector3D?());
      currentCamera.SetViewMatrix(this.GetViewMatrix());
      currentCamera.CameraSpring.Enabled = true;
      currentCamera.CameraSpring.SetCurrentCameraControllerVelocity(this.CubeGrid.Physics != null ? this.CubeGrid.Physics.LinearVelocity : Vector3.Zero);
      if ((double) this.m_currentCameraShakePower > 0.0)
      {
        currentCamera.CameraShake.AddShake(this.m_currentCameraShakePower);
        this.m_currentCameraShakePower = 0.0f;
      }
      if (this.Pilot == null || !this.Pilot.InScene || this.Pilot != MySession.Static.LocalCharacter)
        return;
      this.Pilot.EnableHead(!this.IsInFirstPersonView && !this.ForceFirstPersonCamera);
    }

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.Rotate(rotationIndicator, rollIndicator);
    }

    void IMyCameraController.RotateStopped() => this.RotateStopped();

    void IMyCameraController.OnAssumeControl(
      IMyCameraController previousCameraController)
    {
      this.OnAssumeControl(previousCameraController);
      this.m_currentCameraShakePower = 0.0f;
    }

    void IMyCameraController.OnReleaseControl(
      IMyCameraController newCameraController)
    {
      this.OnReleaseControl(newCameraController);
      if (this.Pilot == null || !this.Pilot.InScene)
        return;
      this.Pilot.EnableHead(true);
    }

    bool IMyCameraController.IsInFirstPersonView
    {
      get => this.IsInFirstPersonView;
      set => this.IsInFirstPersonView = value;
    }

    bool IMyCameraController.ForceFirstPersonCamera
    {
      get => this.ForceFirstPersonCamera || !MySession.Static.Settings.Enable3rdPersonView;
      set => this.ForceFirstPersonCamera = value;
    }

    bool IMyCameraController.HandleUse() => false;

    bool IMyCameraController.HandlePickUp() => false;

    bool IMyCameraController.AllowCubeBuilding => false;

    bool IMyGasBlock.IsWorking() => this.IsWorking && this.BlockDefinition.IsPressurized;

    public void RequestUse(UseActionEnum actionEnum, MyCharacter user)
    {
      if (user.IsDead)
        return;
      IMyControllableEntity user1 = (IMyControllableEntity) user;
      UseActionResult actionResult;
      if ((actionResult = this.CanUse(actionEnum, user1)) == UseActionResult.OK)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, UseActionEnum, long>(this, (Func<MyCockpit, Action<UseActionEnum, long>>) (x => new Action<UseActionEnum, long>(x.AttachPilotEvent)), actionEnum, user.EntityId);
      else
        this.AttachPilotEventFailed(actionResult);
    }

    [Event(null, 2198)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    public void AttachPilotEvent(UseActionEnum actionEnum, long characterID)
    {
      IMyUsableEntity myUsableEntity = (IMyUsableEntity) this;
      MyEntity entity;
      int num = MyEntities.TryGetEntityById<MyEntity>(characterID, out entity) ? 1 : 0;
      IMyControllableEntity user = entity as IMyControllableEntity;
      MyCharacter pilot = user as MyCharacter;
      if (num == 0 || myUsableEntity == null || myUsableEntity.CanUse(actionEnum, user) != UseActionResult.OK)
        return;
      if (this.m_pilot != null)
        this.RemovePilot();
      this.AttachPilot(pilot, true, false, false);
    }

    public void AttachPilotEventFailed(UseActionResult actionResult)
    {
      switch (actionResult)
      {
        case UseActionResult.UsedBySomeoneElse:
          MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, font: "Red"));
          break;
        case UseActionResult.AccessDenied:
          MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
          break;
        case UseActionResult.Unpowered:
          MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MySpaceTexts.BlockIsNotPowered, font: "Red"));
          break;
        case UseActionResult.CockpitDamaged:
          MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Notification_ControllableBlockIsDamaged, font: "Red");
          myHudNotification.SetTextFormatArguments((object[]) new string[1]
          {
            this.DefinitionDisplayNameText
          });
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
          break;
        case UseActionResult.MissingDLC:
          MySession.Static.CheckDLCAndNotify((MyDefinitionBase) this.BlockDefinition);
          break;
      }
    }

    private void TryAttachPilot(long pilotId)
    {
      this.m_retryAttachPilot = false;
      if ((this.m_pilot != null || this.m_savedPilot != null && this.m_savedPilot.EntityId == pilotId) && (this.m_pilot == null || this.m_pilot.EntityId == pilotId))
        return;
      this.m_savedPilot = (MyCharacter) null;
      this.RemovePilot();
      MyEntity entity;
      if (MyEntities.TryGetEntityById<MyEntity>(pilotId, out entity))
      {
        if (!(entity is MyCharacter pilot) || !MyEntities.IsInsideWorld(pilot.PositionComp.GetPosition()))
          return;
        this.AttachPilot(pilot, this.m_pilotRelativeWorld.HasValue);
      }
      else
        this.m_retryAttachPilot = true;
    }

    public void ClearSavedpilot()
    {
      this.m_serverSidePilotId = 0L;
      this.m_savedPilot = (MyCharacter) null;
    }

    [Event(null, 2268)]
    [Reliable]
    [Broadcast]
    private void SetAutopilot_Client([Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_AutopilotBase autopilot)
    {
      if (autopilot == null)
        this.RemoveAutopilot(false);
      else
        this.AttachAutopilot(MyAutopilotFactory.CreateAutopilot(autopilot), false);
    }

    [Event(null, 2281)]
    [Reliable]
    [Broadcast]
    private void NotifyClientPilotChanged(long pilotEntityId, Matrix? pilotRelativeWorld)
    {
      this.m_serverSidePilotId = pilotEntityId;
      this.m_pilotRelativeWorld = pilotRelativeWorld;
      if (pilotEntityId != 0L)
      {
        this.TryAttachPilot(pilotEntityId);
      }
      else
      {
        if (this.m_pilot == null)
          return;
        this.RemovePilot();
      }
    }

    private void OnCharacterFactionChanged(MyFaction oldFaction, MyFaction newFaction) => this.CheckPilotRelation();

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    float Sandbox.ModAPI.IMyCockpit.OxygenFilledRatio
    {
      get => this.OxygenFillLevel;
      set => this.OxygenAmount = value * this.BlockDefinition.OxygenCapacity;
    }

    void Sandbox.ModAPI.IMyCockpit.AttachPilot(IMyCharacter pilot)
    {
      if (pilot.IsDead)
        return;
      UseActionEnum actionEnum = UseActionEnum.Manipulate;
      IMyControllableEntity user = pilot as IMyControllableEntity;
      if (this.CanUse(actionEnum, user) != UseActionResult.OK)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCockpit, UseActionEnum, long>(this, (Func<MyCockpit, Action<UseActionEnum, long>>) (x => new Action<UseActionEnum, long>(x.AttachPilotEvent)), actionEnum, ((VRage.ModAPI.IMyEntity) pilot).EntityId);
    }

    void Sandbox.ModAPI.IMyCockpit.RemovePilot() => this.RemoveLocal();

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return this.m_multiPanel == null ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : this.m_multiPanel.GetSurface(index);
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyCockpit, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
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

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyCockpit, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
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

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyCockpit, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
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

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyCockpit, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
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

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyCockpit, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
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

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyCockpit, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
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

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyCockpit, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
        in string description,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(description, isPublic);
      }
    }

    protected sealed class OnRequestRemovePilot\u003C\u003E : ICallSite<MyCockpit, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRequestRemovePilot();
      }
    }

    protected sealed class AttachPilotEvent\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64 : ICallSite<MyCockpit, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
        in UseActionEnum actionEnum,
        in long characterID,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AttachPilotEvent(actionEnum, characterID);
      }
    }

    protected sealed class SetAutopilot_Client\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_AutopilotBase : ICallSite<MyCockpit, MyObjectBuilder_AutopilotBase, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
        in MyObjectBuilder_AutopilotBase autopilot,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetAutopilot_Client(autopilot);
      }
    }

    protected sealed class NotifyClientPilotChanged\u003C\u003ESystem_Int64\u0023System_Nullable`1\u003CVRageMath_Matrix\u003E : ICallSite<MyCockpit, long, Matrix?, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCockpit @this,
        in long pilotEntityId,
        in Matrix? pilotRelativeWorld,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.NotifyClientPilotChanged(pilotEntityId, pilotRelativeWorld);
      }
    }

    protected class m_oxygenFillLevel\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyCockpit) obj0).m_oxygenFillLevel = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyCockpit\u003C\u003EActor : IActivator, IActivator<MyCockpit>
    {
      object IActivator.CreateInstance() => (object) new MyCockpit();

      MyCockpit IActivator<MyCockpit>.CreateInstance() => new MyCockpit();
    }
  }
}
