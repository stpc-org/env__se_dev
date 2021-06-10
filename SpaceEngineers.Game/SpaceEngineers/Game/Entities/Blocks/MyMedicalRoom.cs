// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyMedicalRoom
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MedicalRoom))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyMedicalRoom), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyMedicalRoom)})]
  public class MyMedicalRoom : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyMedicalRoom, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyMedicalRoom, IMyLifeSupportingBlock, IMyRechargeSocketOwner, IMyGasBlock, IMyConveyorEndpointBlock, IMySpawnBlock
  {
    private static readonly string[] m_emissiveTextureNames = new string[2]
    {
      "Emissive2",
      "Emissive3"
    };
    private bool m_healingAllowed;
    private bool m_refuelAllowed;
    private bool m_suitChangeAllowed;
    private bool m_customWardrobesEnabled;
    private bool m_forceSuitChangeOnRespawn;
    private bool m_spawnWithoutOxygenEnabled;
    private HashSet<string> m_customWardrobeNames = new HashSet<string>();
    private string m_respawnSuitName;
    private MySoundPair m_idleSound;
    private MySoundPair m_progressSound;
    private MyCharacter m_wardrobeUser;
    private MatrixD m_wardrobeUserSpectatorMatrix;
    private byte m_wardrobeUserAwayCounter;
    private MyLight m_light;
    private readonly MyEntity3DSoundEmitter m_idleSoundEmitter;
    private MyMedicalRoomDefinition m_medicalRoomDefinition;
    private MyLifeSupportingComponent m_lifeSupportingComponent;
    private bool m_forcedWardrobeKick;
    protected bool m_takeSpawneeOwnership;
    protected bool m_setFactionToSpawnee;
    private MyResourceSinkComponent m_sinkComponent;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private long m_wardrobeUserId;
    private List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();

    public bool SetFactionToSpawnee => this.m_setFactionToSpawnee;

    public MyResourceSinkComponent SinkComp
    {
      get => this.m_sinkComponent;
      set
      {
        if (this.Components.Contains(typeof (MyResourceSinkComponent)))
          this.Components.Remove<MyResourceSinkComponent>();
        this.Components.Add<MyResourceSinkComponent>(value);
        this.m_sinkComponent = value;
      }
    }

    private ulong SteamUserId { get; set; }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public bool CanPressurizeRoom => false;

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    protected override bool CheckIsWorking() => this.SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public bool HealingAllowed
    {
      set => this.m_healingAllowed = value;
      get => this.m_healingAllowed;
    }

    public bool RefuelAllowed
    {
      set => this.m_refuelAllowed = value;
      get => this.m_refuelAllowed;
    }

    public bool RespawnAllowed
    {
      set
      {
        if (value)
        {
          if (this.Components.Get<MyEntityRespawnComponentBase>() != null)
            return;
          this.Components.Add<MyEntityRespawnComponentBase>((MyEntityRespawnComponentBase) new MyRespawnComponent());
        }
        else
          this.Components.Remove<MyEntityRespawnComponentBase>();
      }
      get => this.Components.Get<MyEntityRespawnComponentBase>() != null;
    }

    public StringBuilder SpawnName { get; private set; }

    string IMySpawnBlock.SpawnName => this.SpawnName.ToString();

    private void SetSpawnName(StringBuilder text)
    {
      if (!this.SpawnName.CompareUpdate(text))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMedicalRoom, string>(this, (Func<MyMedicalRoom, Action<string>>) (x => new Action<string>(x.SetSpawnTextEvent)), text.ToString());
    }

    [Event(null, 172)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    protected void SetSpawnTextEvent(string text) => this.SpawnName.CompareUpdate(text);

    public bool SuitChangeAllowed
    {
      set => this.m_suitChangeAllowed = value;
      get => this.m_suitChangeAllowed && this.m_wardrobeUser == null;
    }

    public bool CustomWardrobesEnabled
    {
      set => this.m_customWardrobesEnabled = value;
      get => this.m_customWardrobesEnabled;
    }

    public HashSet<string> CustomWardrobeNames
    {
      set => this.m_customWardrobeNames = value;
      get => this.m_customWardrobeNames;
    }

    public bool ForceSuitChangeOnRespawn
    {
      set
      {
        if (!value || this.m_respawnSuitName == null)
          return;
        this.m_forceSuitChangeOnRespawn = value;
      }
      get => this.m_forceSuitChangeOnRespawn;
    }

    public string RespawnSuitName
    {
      set => this.m_respawnSuitName = value;
      get => this.m_respawnSuitName;
    }

    public bool SpawnWithoutOxygenEnabled
    {
      set => this.m_spawnWithoutOxygenEnabled = value;
      get => this.m_spawnWithoutOxygenEnabled;
    }

    public bool IsGridPreview
    {
      get
      {
        if (this.CubeGrid == null)
          return false;
        return this.CubeGrid.IsPreview || this.CubeGrid.Projector != null;
      }
    }

    public MyMedicalRoom()
    {
      this.CreateTerminalControls();
      this.SpawnName = new StringBuilder();
      this.m_idleSoundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyMedicalRoom>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlTextbox<MyMedicalRoom> terminalControlTextbox = new MyTerminalControlTextbox<MyMedicalRoom>("SpawnName", MySpaceTexts.MedicalRoom_SpawnNameLabel, MySpaceTexts.MedicalRoom_SpawnNameToolTip);
      terminalControlTextbox.Getter = (MyTerminalControlTextbox<MyMedicalRoom>.GetterDelegate) (x => x.SpawnName);
      terminalControlTextbox.Setter = (MyTerminalControlTextbox<MyMedicalRoom>.SetterDelegate) ((x, v) => x.SetSpawnName(v));
      terminalControlTextbox.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyMedicalRoom>((MyTerminalControl<MyMedicalRoom>) terminalControlTextbox);
      MyTerminalControlLabel<MyMedicalRoom> terminalControlLabel = new MyTerminalControlLabel<MyMedicalRoom>(MySpaceTexts.TerminalScenarioSettingsLabel);
      MyTerminalControlCheckbox<MyMedicalRoom> terminalControlCheckbox1 = new MyTerminalControlCheckbox<MyMedicalRoom>("TakeOwnership", MySpaceTexts.MedicalRoom_ownershipAssignmentLabel, MySpaceTexts.MedicalRoom_ownershipAssignmentTooltip, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true, maxWidth: 0.19f);
      terminalControlCheckbox1.Getter = (MyTerminalValueControl<MyMedicalRoom, bool>.GetterDelegate) (x => x.m_takeSpawneeOwnership);
      terminalControlCheckbox1.Setter = (MyTerminalValueControl<MyMedicalRoom, bool>.SetterDelegate) ((x, val) => x.m_takeSpawneeOwnership = val);
      terminalControlCheckbox1.Enabled = (Func<MyMedicalRoom, bool>) (x => MySession.Static.Settings.ScenarioEditMode);
      MyTerminalControlFactory.AddControl<MyMedicalRoom>((MyTerminalControl<MyMedicalRoom>) terminalControlLabel);
      MyTerminalControlFactory.AddControl<MyMedicalRoom>((MyTerminalControl<MyMedicalRoom>) terminalControlCheckbox1);
      MyTerminalControlCheckbox<MyMedicalRoom> terminalControlCheckbox2 = new MyTerminalControlCheckbox<MyMedicalRoom>("SetFaction", MySpaceTexts.MedicalRoom_factionAssignmentLabel, MySpaceTexts.MedicalRoom_factionAssignmentTooltip);
      terminalControlCheckbox2.Getter = (MyTerminalValueControl<MyMedicalRoom, bool>.GetterDelegate) (x => x.m_setFactionToSpawnee);
      terminalControlCheckbox2.Setter = (MyTerminalValueControl<MyMedicalRoom, bool>.SetterDelegate) ((x, val) => x.m_setFactionToSpawnee = val);
      terminalControlCheckbox2.Enabled = (Func<MyMedicalRoom, bool>) (x => MySession.Static.Settings.ScenarioEditMode);
      MyTerminalControlFactory.AddControl<MyMedicalRoom>((MyTerminalControl<MyMedicalRoom>) terminalControlCheckbox2);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_medicalRoomDefinition = this.BlockDefinition as MyMedicalRoomDefinition;
      MyStringHash orCompute;
      if (this.m_medicalRoomDefinition != null)
      {
        this.m_idleSound = new MySoundPair(this.m_medicalRoomDefinition.IdleSound);
        this.m_progressSound = new MySoundPair(this.m_medicalRoomDefinition.ProgressSound);
        orCompute = MyStringHash.GetOrCompute(this.m_medicalRoomDefinition.ResourceSinkGroup);
      }
      else
      {
        this.m_idleSound = new MySoundPair("BlockMedical");
        this.m_progressSound = new MySoundPair("BlockMedicalProgress");
        orCompute = MyStringHash.GetOrCompute("Utility");
      }
      this.SinkComp = new MyResourceSinkComponent();
      this.SinkComp.Init(orCompute, 1f / 500f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.SinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.SinkComp.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      base.Init(objectBuilder, cubeGrid);
      this.m_lifeSupportingComponent = new MyLifeSupportingComponent((MyEntity) this, this.m_progressSound, "MedRoomHeal", 5f);
      this.Components.Add<MyLifeSupportingComponent>(this.m_lifeSupportingComponent);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      MyObjectBuilder_MedicalRoom builderMedicalRoom = objectBuilder as MyObjectBuilder_MedicalRoom;
      this.SpawnName.Clear();
      if (builderMedicalRoom.SpawnName != null)
        this.SpawnName.Append(builderMedicalRoom.SpawnName);
      this.SteamUserId = builderMedicalRoom.SteamUserId;
      if (this.SteamUserId != 0UL)
      {
        MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(this.SteamUserId));
        if (playerById != null)
        {
          this.IDModule.Owner = playerById.Identity.IdentityId;
          this.IDModule.ShareMode = MyOwnershipShareModeEnum.Faction;
        }
      }
      this.SteamUserId = 0UL;
      this.m_takeSpawneeOwnership = builderMedicalRoom.TakeOwnership;
      this.m_setFactionToSpawnee = builderMedicalRoom.SetFaction;
      this.m_wardrobeUserId = builderMedicalRoom.WardrobeUserId;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.InitializeConveyorEndpoint();
      this.SinkComp.Update();
      this.Components.Remove<MyRespawnComponent>();
      if (this.CubeGrid.CreatePhysics)
        this.Components.Add<MyEntityRespawnComponentBase>((MyEntityRespawnComponentBase) new MyRespawnComponent());
      this.m_healingAllowed = this.m_medicalRoomDefinition.HealingAllowed;
      this.m_refuelAllowed = this.m_medicalRoomDefinition.RefuelAllowed;
      this.m_suitChangeAllowed = this.m_medicalRoomDefinition.SuitChangeAllowed;
      this.m_customWardrobesEnabled = this.m_medicalRoomDefinition.CustomWardrobesEnabled;
      this.m_forceSuitChangeOnRespawn = this.m_medicalRoomDefinition.ForceSuitChangeOnRespawn;
      this.m_customWardrobeNames = this.m_medicalRoomDefinition.CustomWardrobeNames;
      this.m_respawnSuitName = this.m_medicalRoomDefinition.RespawnSuitName;
      this.m_spawnWithoutOxygenEnabled = this.m_medicalRoomDefinition.SpawnWithoutOxygenEnabled;
      this.RespawnAllowed = this.m_medicalRoomDefinition.RespawnAllowed;
      this.m_light = MyLights.AddLight();
      if (this.m_light != null)
      {
        this.m_light.Start((Vector4) Color.White, 2f, "Med bay light");
        this.m_light.Falloff = 1.3f;
        this.m_light.LightOn = false;
        this.m_light.UpdateLight();
      }
      if (this.m_medicalRoomDefinition.ScreenAreas != null && this.m_medicalRoomDefinition.ScreenAreas.Count > 0)
      {
        this.m_panels = new List<MyTextPanelComponent>();
        for (int index = 0; index < this.m_medicalRoomDefinition.ScreenAreas.Count; ++index)
        {
          MyTextPanelComponent textPanelComponent = new MyTextPanelComponent(index, (MyTerminalBlock) this, this.m_medicalRoomDefinition.ScreenAreas[index].Name, this.m_medicalRoomDefinition.ScreenAreas[index].DisplayName, this.m_medicalRoomDefinition.ScreenAreas[index].TextureResolution);
          this.m_panels.Add(textPanelComponent);
          this.SyncType.Append((object) textPanelComponent);
          textPanelComponent.Init();
        }
      }
      if (builderMedicalRoom.WardrobeUserId <= 0L)
        return;
      this.m_forcedWardrobeKick = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_panels.Count > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      foreach (MyTextPanelComponent panel in this.m_panels)
      {
        panel.SetRender((MyRenderComponentScreenAreas) this.Render);
        ((MyRenderComponentScreenAreas) this.Render).AddScreenArea(this.Render.RenderObjectIDs, panel.Name);
      }
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnStopWorking()
    {
      this.StopIdleSound();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateEmissivity();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnStartWorking()
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.StartIdleSound();
      this.UpdateEmissivity();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.SinkComp.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateVisual();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_MedicalRoom builderCubeBlock = (MyObjectBuilder_MedicalRoom) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.SpawnName = this.SpawnName.ToString();
      builderCubeBlock.SteamUserId = this.SteamUserId;
      builderCubeBlock.IdleSound = this.m_idleSound.ToString();
      builderCubeBlock.ProgressSound = this.m_progressSound.ToString();
      builderCubeBlock.TakeOwnership = this.m_takeSpawneeOwnership;
      builderCubeBlock.SetFaction = this.m_setFactionToSpawnee;
      if (this.m_wardrobeUser != null)
        builderCubeBlock.WardrobeUserId = this.m_wardrobeUser.EntityId;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateSoundEmitters()
    {
      base.UpdateSoundEmitters();
      if (this.m_idleSoundEmitter != null)
        this.m_idleSoundEmitter.Update();
      this.m_lifeSupportingComponent.UpdateSoundEmitters();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.m_wardrobeUser == null || this.m_wardrobeUser != MySession.Static.LocalCharacter)
        return;
      this.SetSpectatorCamera();
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (this.m_wardrobeUser != null)
      {
        if (this.m_wardrobeUser == MySession.Static.LocalCharacter)
        {
          double num = Math.Abs(Vector3D.Distance(this.m_wardrobeUser.PositionComp.GetPosition(), this.PositionComp.GetPosition()) - (double) this.m_medicalRoomDefinition.WardrobeCharacterOffsetLength);
          if (this.m_wardrobeUser.IsDead || num > 0.5)
          {
            ++this.m_wardrobeUserAwayCounter;
            if (this.m_wardrobeUserAwayCounter > (byte) 6)
              this.StopUsingWardrobe();
          }
          else
            this.m_wardrobeUserAwayCounter = (byte) 0;
          if (!this.IsFunctional || !this.IsWorking)
          {
            this.StopUsingWardrobe();
            this.m_wardrobeUser = (MyCharacter) null;
          }
        }
        else if (Sync.IsServer && this.m_wardrobeUser.ControllerInfo.Controller == null)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMedicalRoom>(this, (Func<MyMedicalRoom, Action>) (x => new Action(x.StopUsingWardrobeSync)));
      }
      else if (this.m_wardrobeUserId != 0L)
      {
        MyCharacter entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(this.m_wardrobeUserId, out entity);
        if (entity != null)
        {
          this.m_wardrobeUser = entity;
          this.m_wardrobeUserId = 0L;
        }
      }
      this.m_lifeSupportingComponent.Update10();
    }

    public void UseWardrobe(MyCharacter user)
    {
      this.m_wardrobeUserSpectatorMatrix = MySpectatorCameraController.Static.GetViewMatrix();
      user.UpdateRotationsOverride = true;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMedicalRoom, long>(this, (Func<MyMedicalRoom, Action<long>>) (x => new Action<long>(x.UseWardrobeSync)), user.EntityId);
    }

    private void SetSpectatorCamera()
    {
      MatrixD worldMatrix1 = this.WorldMatrix;
      float num = 70f / MySector.MainCamera.FieldOfViewDegrees;
      ref MatrixD local = ref worldMatrix1;
      Vector3D translation1 = local.Translation;
      MatrixD worldMatrix2 = this.WorldMatrix;
      Vector3D vector3D1 = worldMatrix2.Right * (this.m_medicalRoomDefinition.WardrobeCharacterOffset.X - 1.14999997615814);
      worldMatrix2 = this.WorldMatrix;
      Vector3D vector3D2 = worldMatrix2.Up * (0.699999988079071 + this.m_medicalRoomDefinition.WardrobeCharacterOffset.Y);
      Vector3D vector3D3 = vector3D1 + vector3D2;
      worldMatrix2 = this.WorldMatrix;
      Vector3D vector3D4 = worldMatrix2.Forward * (1.5 + this.m_medicalRoomDefinition.WardrobeCharacterOffset.Z) * (double) num;
      Vector3D vector3D5 = vector3D3 + vector3D4;
      local.Translation = translation1 + vector3D5;
      worldMatrix1.Left = this.WorldMatrix.Right;
      worldMatrix1.Forward = this.WorldMatrix.Backward;
      if (this.m_light != null)
      {
        MatrixD worldMatrix3 = this.WorldMatrix;
        Vector3D translation2 = worldMatrix3.Translation;
        worldMatrix3 = this.WorldMatrix;
        Vector3D vector3D6 = worldMatrix3.Right * (this.m_medicalRoomDefinition.WardrobeCharacterOffset.X - 0.5);
        Vector3D vector3D7 = translation2 + vector3D6;
        worldMatrix3 = this.WorldMatrix;
        Vector3D vector3D8 = worldMatrix3.Up * (1.0 + this.m_medicalRoomDefinition.WardrobeCharacterOffset.Y);
        Vector3D vector3D9 = vector3D7 + vector3D8;
        worldMatrix3 = this.WorldMatrix;
        Vector3D vector3D10 = worldMatrix3.Forward * (0.600000023841858 + this.m_medicalRoomDefinition.WardrobeCharacterOffset.Z) * (double) num;
        this.m_light.Position = vector3D9 + vector3D10;
        this.m_light.UpdateLight();
      }
      MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (VRage.ModAPI.IMyEntity) null, new Vector3D?(worldMatrix1.Translation));
      MySpectatorCameraController.Static.SetTarget(worldMatrix1.Translation + worldMatrix1.Forward, new Vector3D?(worldMatrix1.Up));
      if (MySpectatorCameraController.Static.IsLightOn)
        return;
      MySpectatorCameraController.Static.SwitchLight();
    }

    [Event(null, 537)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void UseWardrobeSync(long userId)
    {
      MyCharacter entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(userId, out entity);
      if (entity == null)
        return;
      this.m_wardrobeUser = entity;
      MatrixD worldMatrix1 = this.WorldMatrix;
      if (this.Model.Dummies.ContainsKey("detector_wardrobe"))
      {
        worldMatrix1 = MatrixD.Multiply(MatrixD.Normalize((MatrixD) ref this.Model.Dummies["detector_wardrobe"].Matrix), this.WorldMatrix);
        worldMatrix1.Translation -= this.WorldMatrix.Up * 0.98;
      }
      else
      {
        ref MatrixD local = ref worldMatrix1;
        Vector3D translation = local.Translation;
        MatrixD worldMatrix2 = this.WorldMatrix;
        Vector3D vector3D1 = worldMatrix2.Right * this.m_medicalRoomDefinition.WardrobeCharacterOffset.X;
        worldMatrix2 = this.WorldMatrix;
        Vector3D vector3D2 = worldMatrix2.Up * this.m_medicalRoomDefinition.WardrobeCharacterOffset.Y;
        Vector3D vector3D3 = vector3D1 + vector3D2;
        worldMatrix2 = this.WorldMatrix;
        Vector3D vector3D4 = worldMatrix2.Forward * this.m_medicalRoomDefinition.WardrobeCharacterOffset.Z;
        Vector3D vector3D5 = vector3D3 + vector3D4;
        local.Translation = translation + vector3D5;
      }
      if (Sync.IsServer)
        entity.PositionComp.SetWorldMatrix(ref worldMatrix1);
      if (entity == MySession.Static.LocalCharacter)
      {
        if (entity.JetpackRunning)
          entity.SwitchJetpack();
        entity.ForceDisablePrediction = true;
        entity.UpdateCharacterPhysics();
        entity.PositionComp.SetPosition(worldMatrix1.Translation);
        entity.PositionComp.SetWorldMatrix(ref worldMatrix1);
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      if (this.m_light != null)
      {
        this.m_light.LightOn = true;
        this.m_light.UpdateLight();
      }
      this.UpdateEmissivity();
    }

    private void SetEmissive(Color color, int index, float emissivity = 1f)
    {
      if (this.Render.RenderObjectIDs[0] == uint.MaxValue || MyMedicalRoom.m_emissiveTextureNames.Length <= index)
        return;
      MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyMedicalRoom.m_emissiveTextureNames[index], color, emissivity);
    }

    private void UpdateEmissivity()
    {
      if (this.IsFunctional)
      {
        if (this.IsWorking)
        {
          this.SetEmissive(Color.Green, 0);
          if (this.m_wardrobeUser != null)
          {
            this.SetEmissive(Color.Cyan, 0);
            this.SetEmissive(Color.White, 1);
          }
          else
          {
            this.SetEmissive(Color.Green, 0);
            this.SetEmissive(Color.White, 1, 0.0f);
          }
        }
        else
        {
          this.SetEmissive(Color.Red, 0);
          this.SetEmissive(Color.Red, 1);
        }
      }
      else
      {
        this.SetEmissive(Color.Black, 0, 0.0f);
        this.SetEmissive(Color.Black, 1, 0.0f);
      }
    }

    public void StopUsingWardrobe()
    {
      if (this.m_wardrobeUser == null)
        return;
      this.m_wardrobeUser.UpdateRotationsOverride = true;
      this.m_wardrobeUserAwayCounter = (byte) 0;
      if (MyGuiScreenGamePlay.ActiveGameplayScreen is MyGuiScreenLoadInventory)
        MyGuiScreenGamePlay.ActiveGameplayScreen.CloseScreen();
      MySpectatorCameraController.Static.SetViewMatrix(this.m_wardrobeUserSpectatorMatrix);
      MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this.m_wardrobeUser, new Vector3D?());
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMedicalRoom>(this, (Func<MyMedicalRoom, Action>) (x => new Action(x.StopUsingWardrobeSync)));
      if (this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    [Event(null, 645)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void StopUsingWardrobeSync()
    {
      if (this.m_wardrobeUser != null)
      {
        MatrixD worldMatrix;
        if (this.Components.Get<MyEntityRespawnComponentBase>() is MyRespawnComponent respawnComponent)
        {
          worldMatrix = respawnComponent.GetSpawnPosition();
        }
        else
        {
          worldMatrix = this.WorldMatrix;
          worldMatrix.Translation += worldMatrix.Forward * (double) this.BlockDefinition.Size.AbsMax();
        }
        if (Sync.IsServer)
          this.m_wardrobeUser.PositionComp.SetWorldMatrix(ref worldMatrix);
        else if (this.m_wardrobeUser == MySession.Static.LocalCharacter)
        {
          this.m_wardrobeUser.ForceDisablePrediction = false;
          this.m_wardrobeUser.UpdateCharacterPhysics();
          this.m_wardrobeUser.PositionComp.SetWorldMatrix(ref worldMatrix);
        }
      }
      this.m_wardrobeUser = (MyCharacter) null;
      if (this.m_light != null)
      {
        this.m_light.LightOn = false;
        this.m_light.UpdateLight();
      }
      this.UpdateEmissivity();
    }

    void IMyLifeSupportingBlock.ShowTerminal(MyCharacter user) => MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, (MyEntity) this);

    void IMyLifeSupportingBlock.BroadcastSupportRequest(MyCharacter user) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMedicalRoom, long>(this, (Func<MyMedicalRoom, Action<long>>) (x => new Action<long>(x.RequestSupport)), user.EntityId);

    MyLifeSupportingBlockType IMyLifeSupportingBlock.BlockType => MyLifeSupportingBlockType.MedicalRoom;

    [Event(null, 703)]
    [Reliable]
    [Server(ValidationType.Access)]
    [Broadcast]
    private void RequestSupport(long userId)
    {
      if (!this.GetUserRelationToOwner(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0)).IsFriendly() && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
        return;
      MyCharacter entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(userId, out entity);
      if (entity == null)
        return;
      this.m_lifeSupportingComponent.ProvideSupport(entity);
    }

    MyRechargeSocket IMyRechargeSocketOwner.RechargeSocket => this.m_lifeSupportingComponent.RechargeSocket;

    protected override void Closing()
    {
      this.StopIdleSound();
      MyLights.RemoveLight(this.m_light);
      base.Closing();
    }

    private void StopIdleSound() => this.m_idleSoundEmitter.StopSound(false);

    private void StartIdleSound() => this.m_idleSoundEmitter.PlaySound(this.m_idleSound, true);

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.SinkComp.Update();
    }

    bool IMyGasBlock.IsWorking() => this.IsWorking;

    public void TrySetFaction(MyPlayer player)
    {
      if (!MySession.Static.IsScenario || !this.m_setFactionToSpawnee || (!Sync.IsServer || this.OwnerId == 0L))
        return;
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(this.OwnerId);
      if (playerFaction == null)
        return;
      MyFactionCollection.SendJoinRequest(playerFaction.FactionId, player.Identity.IdentityId);
      if (playerFaction.AutoAcceptMember)
        return;
      MyFactionCollection.AcceptJoin(playerFaction.FactionId, player.Identity.IdentityId);
    }

    public void TryTakeSpawneeOwnership(MyPlayer player)
    {
      if (!MySession.Static.IsScenario || !this.m_takeSpawneeOwnership || (!Sync.IsServer || this.OwnerId != 0L))
        return;
      this.ChangeBlockOwnerRequest(player.Identity.IdentityId, MyOwnershipShareModeEnum.None);
    }

    public void UpdateScreen()
    {
      if (!this.CheckIsWorking())
      {
        for (int index = 0; index < this.m_panels.Count; ++index)
          ((MyRenderComponentScreenAreas) this.Render).ChangeTexture(index, this.m_panels[index].GetPathForID("Offline"));
      }
      else
      {
        for (int area = 0; area < this.m_panels.Count; ++area)
          ((MyRenderComponentScreenAreas) this.Render).ChangeTexture(area, (string) null);
      }
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.m_wardrobeUserId > 0L && this.m_wardrobeUser == null)
      {
        MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_wardrobeUserId);
        if (entityById != null && entityById is MyCharacter myCharacter)
        {
          this.m_wardrobeUser = myCharacter;
        }
        else
        {
          this.m_wardrobeUser = (MyCharacter) null;
          this.m_wardrobeUserId = 0L;
        }
      }
      if (this.m_forcedWardrobeKick)
      {
        if (this.m_wardrobeUserId == MySession.Static.LocalCharacterEntityId && Sync.IsServer)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyMedicalRoom>(this, (Func<MyMedicalRoom, Action>) (x => new Action(x.StopUsingWardrobeSync)));
        this.m_forcedWardrobeKick = false;
      }
      this.UpdateScreen();
      this.UpdateEmissivity();
    }

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    protected sealed class SetSpawnTextEvent\u003C\u003ESystem_String : ICallSite<MyMedicalRoom, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMedicalRoom @this,
        in string text,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetSpawnTextEvent(text);
      }
    }

    protected sealed class UseWardrobeSync\u003C\u003ESystem_Int64 : ICallSite<MyMedicalRoom, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMedicalRoom @this,
        in long userId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UseWardrobeSync(userId);
      }
    }

    protected sealed class StopUsingWardrobeSync\u003C\u003E : ICallSite<MyMedicalRoom, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMedicalRoom @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.StopUsingWardrobeSync();
      }
    }

    protected sealed class RequestSupport\u003C\u003ESystem_Int64 : ICallSite<MyMedicalRoom, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyMedicalRoom @this,
        in long userId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RequestSupport(userId);
      }
    }
  }
}
