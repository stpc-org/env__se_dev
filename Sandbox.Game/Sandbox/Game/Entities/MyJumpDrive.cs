// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyJumpDrive
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_JumpDrive))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyJumpDrive), typeof (Sandbox.ModAPI.Ingame.IMyJumpDrive)})]
  public class MyJumpDrive : MyFunctionalBlock, Sandbox.ModAPI.IMyJumpDrive, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyJumpDrive
  {
    private readonly VRage.Sync.Sync<float, SyncDirection.FromServer> m_storedPower;
    private IMyGps m_selectedGps;
    private IMyGps m_jumpTarget;
    private readonly VRage.Sync.Sync<int?, SyncDirection.BothWays> m_targetSync;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_jumpDistanceRatio;
    private int? m_storedJumpTarget;
    private float m_timeRemaining;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isRecharging;
    public bool IsJumping;
    private static readonly string[] m_emissiveTextureNames = new string[4]
    {
      "Emissive0",
      "Emissive1",
      "Emissive2",
      "Emissive3"
    };
    private Color m_prevColor = Color.White;
    private int m_prevFillCount = -1;

    public MyJumpDriveDefinition BlockDefinition => (MyJumpDriveDefinition) base.BlockDefinition;

    public float CurrentStoredPower
    {
      get => (float) this.m_storedPower;
      set
      {
        if ((double) this.m_storedPower.Value == (double) value)
          return;
        this.m_storedPower.Value = value;
        this.UpdateEmissivity();
      }
    }

    public bool CanJump => this.IsWorking && this.IsFunctional && this.IsFull;

    public bool CanJumpAndHasAccess(long userId) => this.CanJump && this.IDModule.GetUserRelationToOwner(userId).IsFriendly();

    public bool IsFull => (double) (float) this.m_storedPower >= (double) this.BlockDefinition.PowerNeededForJump;

    float Sandbox.ModAPI.Ingame.IMyJumpDrive.CurrentStoredPower => (float) this.m_storedPower;

    float Sandbox.ModAPI.Ingame.IMyJumpDrive.MaxStoredPower => this.BlockDefinition.PowerNeededForJump;

    float Sandbox.ModAPI.IMyJumpDrive.CurrentStoredPower
    {
      get => this.CurrentStoredPower;
      set => this.CurrentStoredPower = value;
    }

    void Sandbox.ModAPI.IMyJumpDrive.Jump(bool usePilot) => this.RequestJump(usePilot);

    MyJumpDriveStatus Sandbox.ModAPI.Ingame.IMyJumpDrive.Status
    {
      get
      {
        if (this.IsJumping)
          return MyJumpDriveStatus.Jumping;
        return this.CanJump ? MyJumpDriveStatus.Ready : MyJumpDriveStatus.Charging;
      }
    }

    public MyJumpDrive()
    {
      this.CreateTerminalControls();
      this.m_isRecharging.ValueChanged += (Action<SyncBase>) (x => this.RaisePropertiesChanged());
      this.m_targetSync.ValueChanged += (Action<SyncBase>) (x => this.TargetChanged());
      this.m_storedPower.AlwaysReject<float, SyncDirection.FromServer>();
    }

    private void TargetChanged()
    {
      this.m_jumpTarget = !this.m_targetSync.Value.HasValue ? (IMyGps) null : (IMyGps) MySession.Static.Gpss.GetGps(this.m_targetSync.Value.Value);
      this.RaisePropertiesChanged();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyJumpDrive>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlButton<MyJumpDrive> button = new MyTerminalControlButton<MyJumpDrive>("Jump", MySpaceTexts.BlockActionTitle_Jump, MySpaceTexts.Blank, (Action<MyJumpDrive>) (x => x.RequestJump()));
      button.Enabled = (Func<MyJumpDrive, bool>) (x => x.CanJump);
      button.SupportsMultipleBlocks = false;
      button.Visible = (Func<MyJumpDrive, bool>) (x => false);
      MyTerminalAction<MyJumpDrive> myTerminalAction = button.EnableAction<MyJumpDrive>(MyTerminalActionIcons.TOGGLE);
      if (myTerminalAction != null)
      {
        myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>()
        {
          MyToolbarType.ButtonPanel,
          MyToolbarType.Character,
          MyToolbarType.Seat
        };
        myTerminalAction.ValidForGroups = false;
      }
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) button);
      MyTerminalControlOnOffSwitch<MyJumpDrive> onOff = new MyTerminalControlOnOffSwitch<MyJumpDrive>("Recharge", MySpaceTexts.BlockPropertyTitle_Recharge, MySpaceTexts.Blank);
      onOff.Getter = (MyTerminalValueControl<MyJumpDrive, bool>.GetterDelegate) (x => (bool) x.m_isRecharging);
      onOff.Setter = (MyTerminalValueControl<MyJumpDrive, bool>.SetterDelegate) ((x, v) => x.m_isRecharging.Value = v);
      onOff.EnableToggleAction<MyJumpDrive>();
      onOff.EnableOnOffActions<MyJumpDrive>();
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) onOff);
      MyTerminalControlSlider<MyJumpDrive> slider = new MyTerminalControlSlider<MyJumpDrive>("JumpDistance", MySpaceTexts.BlockPropertyTitle_JumpDistance, MySpaceTexts.Blank);
      slider.SetLimits(0.0f, 100f);
      slider.DefaultValue = new float?(100f);
      slider.Enabled = (Func<MyJumpDrive, bool>) (x => x.m_jumpTarget == null);
      slider.Getter = (MyTerminalValueControl<MyJumpDrive, float>.GetterDelegate) (x => (float) x.m_jumpDistanceRatio * 100f);
      slider.Setter = (MyTerminalValueControl<MyJumpDrive, float>.SetterDelegate) ((x, v) => x.m_jumpDistanceRatio.Value = v * 0.01f);
      slider.Writer = (MyTerminalControl<MyJumpDrive>.WriterDelegate) ((x, v) => v.AppendFormatedDecimal((MathHelper.RoundOn2((float) x.m_jumpDistanceRatio) * 100f).ToString() + "% (", (float) (x.ComputeMaxDistance() / 1000.0), 0, " km").Append(")"));
      slider.EnableActions<MyJumpDrive>(0.01f);
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) slider);
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) new MyTerminalControlListbox<MyJumpDrive>("SelectedTarget", MySpaceTexts.BlockPropertyTitle_DestinationGPS, MySpaceTexts.Blank, visibleRowsCount: 1)
      {
        ListContent = (MyTerminalControlListbox<MyJumpDrive>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillSelectedTarget(list1, list2))
      });
      MyTerminalControlButton<MyJumpDrive> terminalControlButton1 = new MyTerminalControlButton<MyJumpDrive>("RemoveBtn", MySpaceTexts.RemoveProjectionButton, MySpaceTexts.Blank, (Action<MyJumpDrive>) (x => x.RemoveSelected()));
      terminalControlButton1.Enabled = (Func<MyJumpDrive, bool>) (x => x.CanRemove());
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) terminalControlButton1);
      MyTerminalControlButton<MyJumpDrive> terminalControlButton2 = new MyTerminalControlButton<MyJumpDrive>("SelectBtn", MyCommonTexts.SelectBlueprint, MySpaceTexts.Blank, (Action<MyJumpDrive>) (x => x.SelectTarget()));
      terminalControlButton2.Enabled = (Func<MyJumpDrive, bool>) (x => x.CanSelect());
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) terminalControlButton2);
      MyTerminalControlFactory.AddControl<MyJumpDrive>((MyTerminalControl<MyJumpDrive>) new MyTerminalControlListbox<MyJumpDrive>("GpsList", MySpaceTexts.BlockPropertyTitle_GpsLocations, MySpaceTexts.Blank, true)
      {
        ListContent = (MyTerminalControlListbox<MyJumpDrive>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillGpsList(list1, list2)),
        ItemSelected = (MyTerminalControlListbox<MyJumpDrive>.SelectItemDelegate) ((x, y) => x.SelectGps(y))
      });
    }

    private bool CanSelect() => this.m_selectedGps != null;

    private void SelectTarget()
    {
      if (!this.CanSelect())
        return;
      this.m_targetSync.Value = new int?(this.m_selectedGps.Hash);
    }

    private bool CanRemove() => this.m_jumpTarget != null;

    private void RemoveSelected()
    {
      if (!this.CanRemove())
        return;
      this.m_targetSync.Value = new int?();
    }

    private void RequestJump(bool usePlayer = true)
    {
      if (this.CanJump)
      {
        if (usePlayer && MySession.Static.LocalCharacter != null)
        {
          if (!(MySession.Static.LocalCharacter.Parent is MyShipController myShipController) && MySession.Static.ControlledEntity != null)
            myShipController = MySession.Static.ControlledEntity.Entity as MyShipController;
          this.RequestJumpInternal((Sandbox.ModAPI.Ingame.IMyShipController) myShipController);
        }
        else
        {
          if (usePlayer)
            return;
          MyShipController shipController = this.CubeGrid.GridSystems.ControlSystem.GetShipController();
          if (shipController == null)
            return;
          this.RequestJumpInternal((Sandbox.ModAPI.Ingame.IMyShipController) shipController);
        }
      }
      else
      {
        if (this.IsJumping || this.IsFull)
          return;
        MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationJumpDriveNotFullyCharged, 1500);
        myHudNotification.SetTextFormatArguments((object) ((float) this.m_storedPower / this.BlockDefinition.PowerNeededForJump).ToString("P"));
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
    }

    private void RequestJumpInternal(Sandbox.ModAPI.Ingame.IMyShipController shipController)
    {
      if (this.m_jumpTarget != null)
      {
        this.CubeGrid.GridSystems.JumpSystem.RequestJump(this.m_jumpTarget.Name, this.m_jumpTarget.Coords, shipController.OwnerId);
      }
      else
      {
        Vector3D vector3D = Vector3D.Transform(Base6Directions.GetVector(shipController.Orientation.Forward), shipController.CubeGrid.WorldMatrix.GetOrientation());
        vector3D.Normalize();
        Vector3D destination = this.CubeGrid.WorldMatrix.Translation + vector3D * this.ComputeMaxDistance();
        this.CubeGrid.GridSystems.JumpSystem.RequestJump(MyTexts.Get(MySpaceTexts.Jump_Blind).ToString(), destination, shipController.OwnerId);
      }
    }

    private double ComputeMaxDistance()
    {
      double maxJumpDistance = this.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(this.IDModule.Owner);
      return maxJumpDistance < 5000.0 ? 5000.0 : 5001.0 + (maxJumpDistance - 5000.0) * (double) (float) this.m_jumpDistanceRatio;
    }

    private void FillGpsList(
      ICollection<MyGuiControlListbox.Item> gpsItemList,
      ICollection<MyGuiControlListbox.Item> selectedGpsItemList)
    {
      List<IMyGps> list = new List<IMyGps>();
      MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
      foreach (IMyGps myGps in list)
      {
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(new StringBuilder(myGps.Name), userData: ((object) myGps));
        gpsItemList.Add(obj);
        if (this.m_selectedGps == myGps)
          selectedGpsItemList.Add(obj);
      }
    }

    private void FillSelectedTarget(
      ICollection<MyGuiControlListbox.Item> selectedTargetList,
      ICollection<MyGuiControlListbox.Item> emptyList)
    {
      if (this.m_jumpTarget != null)
        selectedTargetList.Add(new MyGuiControlListbox.Item(new StringBuilder(this.m_jumpTarget.Name), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget), userData: ((object) this.m_jumpTarget)));
      else
        selectedTargetList.Add(new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.BlindJump), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget)));
    }

    private void SelectGps(List<MyGuiControlListbox.Item> selection)
    {
      if (selection.Count <= 0)
        return;
      this.m_selectedGps = (IMyGps) selection[0].UserData;
      this.RaisePropertiesChanged();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(this.ComputeRequiredPower));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_JumpDrive builderJumpDrive = objectBuilder as MyObjectBuilder_JumpDrive;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_storedPower.Value = Math.Min(builderJumpDrive.StoredPower, this.BlockDefinition.PowerNeededForJump);
      this.m_storedJumpTarget = builderJumpDrive.JumpTarget;
      if (builderJumpDrive.JumpTarget.HasValue)
        this.m_jumpTarget = (IMyGps) MySession.Static.Gpss.GetGps(builderJumpDrive.JumpTarget.Value);
      this.m_jumpDistanceRatio.SetLocalValue(MathHelper.Clamp(builderJumpDrive.JumpRatio, 0.0f, 1f));
      this.m_isRecharging.SetLocalValue(builderJumpDrive.Recharging);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyJumpDrive_IsWorkingChanged);
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }

    private void MyJumpDrive_IsWorkingChanged(MyCubeBlock obj) => this.CheckForAbort();

    private void ComponentStack_IsFunctionalChanged() => this.CheckForAbort();

    private void CheckForAbort()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsJumping || this.IsWorking && this.IsFunctional)
        return;
      this.IsJumping = false;
      this.CubeGrid.GridSystems.JumpSystem.RequestAbort();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_JumpDrive builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_JumpDrive;
      builderCubeBlock.StoredPower = (float) this.m_storedPower;
      if (this.m_jumpTarget != null)
        builderCubeBlock.JumpTarget = new int?(this.m_jumpTarget.Hash);
      builderCubeBlock.JumpRatio = (float) this.m_jumpDistanceRatio;
      builderCubeBlock.Recharging = (bool) this.m_isRecharging;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.CubeGrid.GridSystems.JumpSystem.RegisterJumpDrive(this);
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      this.CubeGrid.GridSystems.JumpSystem.AbortJump(MyGridJumpDriveSystem.MyJumpFailReason.None);
      this.CubeGrid.GridSystems.JumpSystem.UnregisterJumpDrive(this);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.m_storedJumpTarget.HasValue)
        return;
      this.m_jumpTarget = (IMyGps) MySession.Static.Gpss.GetGps(this.m_storedJumpTarget.Value);
      if (this.m_jumpTarget == null)
        return;
      this.m_targetSync.Value = new int?(this.m_jumpTarget.Hash);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.ResourceSink.Update();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.IsFunctional && !this.IsFull && (bool) this.m_isRecharging)
        this.StorePower(1666.667f, this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.BlockDefinition.RequiredPowerInput, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxStoredPower));
      MyValueFormatter.AppendWorkHoursInBestUnit(this.BlockDefinition.PowerNeededForJump, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_StoredPower));
      MyValueFormatter.AppendWorkHoursInBestUnit((float) this.m_storedPower, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RechargedIn));
      MyValueFormatter.AppendTimeInBestUnit(this.m_timeRemaining, detailedInfo);
      detailedInfo.Append("\n");
      int num1 = (int) (this.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(this.OwnerId) / 1000.0);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxJump));
      detailedInfo.Append(num1).Append(" km");
      if (this.m_jumpTarget == null)
        return;
      detailedInfo.Append("\n");
      double num2 = (this.m_jumpTarget.Coords - this.CubeGrid.WorldMatrix.Translation).Length();
      float num3 = Math.Min(1f, (float) num1 / (float) num2);
      detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_CurrentJump).ToString() + (num3 * 100f).ToString("F2") + "%");
    }

    private float ComputeRequiredPower() => this.IsFunctional && this.IsWorking && ((bool) this.m_isRecharging && !this.IsFull) ? this.BlockDefinition.RequiredPowerInput : 0.0f;

    private void StorePower(float deltaTime, float input)
    {
      float num1 = input / 3600000f;
      float num2 = (float) ((double) deltaTime * (double) num1 * 0.800000011920929);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_storedPower.Value += num2;
      deltaTime /= 1000f;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && (double) (float) this.m_storedPower > (double) this.BlockDefinition.PowerNeededForJump)
        this.m_storedPower.Value = this.BlockDefinition.PowerNeededForJump;
      if ((double) num2 > 0.0)
        this.m_timeRemaining = (this.BlockDefinition.PowerNeededForJump - (float) this.m_storedPower) * deltaTime / num2;
      else
        this.m_timeRemaining = 0.0f;
    }

    public void SetStoredPower(float filledRatio)
    {
      if ((double) filledRatio < 0.0)
        filledRatio = 0.0f;
      if ((double) filledRatio >= 1.0)
      {
        filledRatio = 1f;
        this.m_timeRemaining = 0.0f;
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.CurrentStoredPower = filledRatio * this.BlockDefinition.PowerNeededForJump;
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.m_prevFillCount = -1;
      this.UpdateEmissivity();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateEmissivity(true);
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    private void UpdateEmissivity(bool force = false)
    {
      Color red = Color.Red;
      float fill = 1f;
      float emissivity = 1f;
      Color color;
      if (this.IsWorking)
      {
        if (this.IsFull)
        {
          color = Color.Green;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
            color = result.EmissiveColor;
        }
        else if (!(bool) this.m_isRecharging)
        {
          fill = (float) this.m_storedPower / this.BlockDefinition.PowerNeededForJump;
          color = Color.Red;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
            color = result.EmissiveColor;
        }
        else if ((double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) > 0.0)
        {
          fill = (float) this.m_storedPower / this.BlockDefinition.PowerNeededForJump;
          color = Color.Yellow;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
            color = result.EmissiveColor;
        }
        else
        {
          fill = (float) this.m_storedPower / this.BlockDefinition.PowerNeededForJump;
          color = Color.Red;
          emissivity = 1f;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
            color = result.EmissiveColor;
        }
      }
      else if (this.IsFunctional)
      {
        fill = 0.0f;
        color = Color.Red;
        emissivity = 1f;
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
          color = result.EmissiveColor;
      }
      else
      {
        fill = 0.0f;
        color = Color.Black;
        emissivity = 0.0f;
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
          color = result.EmissiveColor;
      }
      this.SetEmissive(color, fill, emissivity, force);
    }

    private void SetEmissive(Color color, float fill, float emissivity, bool force)
    {
      int num = (int) ((double) fill * (double) MyJumpDrive.m_emissiveTextureNames.Length);
      if (!force && (this.Render.RenderObjectIDs[0] == uint.MaxValue || !(color != this.m_prevColor) && num == this.m_prevFillCount))
        return;
      for (int index = 0; index < MyJumpDrive.m_emissiveTextureNames.Length; ++index)
      {
        if (index <= num)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyJumpDrive.m_emissiveTextureNames[index], color, emissivity);
        else
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyJumpDrive.m_emissiveTextureNames[index], Color.Black, 0.0f);
      }
      MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], "Emissive", color, emissivity);
      this.m_prevColor = color;
      this.m_prevFillCount = num;
    }

    protected class m_storedPower\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyJumpDrive) obj0).m_storedPower = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_targetSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int?, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int?, SyncDirection.BothWays>(obj1, obj2));
        ((MyJumpDrive) obj0).m_targetSync = (VRage.Sync.Sync<int?, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_jumpDistanceRatio\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyJumpDrive) obj0).m_jumpDistanceRatio = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isRecharging\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyJumpDrive) obj0).m_isRecharging = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyJumpDrive\u003C\u003EActor : IActivator, IActivator<MyJumpDrive>
    {
      object IActivator.CreateInstance() => (object) new MyJumpDrive();

      MyJumpDrive IActivator<MyJumpDrive>.CreateInstance() => new MyJumpDrive();
    }
  }
}
