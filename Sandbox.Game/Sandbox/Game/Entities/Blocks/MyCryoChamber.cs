// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyCryoChamber
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_CryoChamber))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyCryoChamber), typeof (Sandbox.ModAPI.Ingame.IMyCryoChamber)})]
  public class MyCryoChamber : MyCockpit, Sandbox.ModAPI.IMyCryoChamber, Sandbox.ModAPI.IMyCockpit, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyCockpit, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyCameraController, Sandbox.ModAPI.IMyTextSurfaceProvider, Sandbox.ModAPI.Ingame.IMyCryoChamber
  {
    private string m_overlayTextureName = "Textures\\GUI\\Screens\\cryopod_interior.dds";
    private MyPlayer.PlayerId? m_currentPlayerId;
    private readonly VRage.Sync.Sync<MyPlayer.PlayerId?, SyncDirection.FromServer> m_attachedPlayerId;
    private bool m_retryAttachPilot;
    private bool m_pilotLights;
    private bool m_pilotJetpack;
    private bool m_pilotCameraInFP = true;

    public override bool IsInFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    private MyCryoChamberDefinition BlockDefinition => (MyCryoChamberDefinition) base.BlockDefinition;

    public MyCryoChamber()
    {
      this.ControllerInfo.ControlAcquired += new Action<MyEntityController>(this.OnCryoChamberControlAcquired);
      this.m_attachedPlayerId.ValueChanged += (Action<SyncBase>) (x => this.AttachedPlayerChanged());
      this.MinHeadLocalXAngle = -50f;
      this.MaxHeadLocalXAngle = 60f;
      this.MinHeadLocalYAngle = -30f;
      this.MaxHeadLocalYAngle = 30f;
    }

    protected override bool CanHaveHorizon() => false;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_characterDummy = (MatrixD) ref Matrix.Identity;
      base.Init(objectBuilder, cubeGrid);
      if (this.ResourceSink == null)
      {
        MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
        resourceSinkComponent.Init(MyStringHash.GetOrCompute(this.BlockDefinition.ResourceSinkGroup), this.BlockDefinition.IdlePowerConsumption, new Func<float>(this.CalculateRequiredPowerInput));
        resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
        this.ResourceSink = resourceSinkComponent;
      }
      else
      {
        this.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, this.BlockDefinition.IdlePowerConsumption);
        this.ResourceSink.SetRequiredInputFuncByType(MyResourceDistributorComponent.ElectricityId, new Func<float>(this.CalculateRequiredPowerInput));
        this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      }
      MyObjectBuilder_CryoChamber builderCryoChamber = objectBuilder as MyObjectBuilder_CryoChamber;
      this.m_currentPlayerId = !builderCryoChamber.SteamId.HasValue || !builderCryoChamber.SerialId.HasValue ? new MyPlayer.PlayerId?() : new MyPlayer.PlayerId?(new MyPlayer.PlayerId(builderCryoChamber.SteamId.Value, builderCryoChamber.SerialId.Value));
      string overlayTexture = this.BlockDefinition.OverlayTexture;
      if (!string.IsNullOrEmpty(overlayTexture))
        this.m_overlayTextureName = overlayTexture;
      this.HorizonIndicatorEnabled = false;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private float CalculateRequiredPowerInput() => !this.IsFunctional ? 0.0f : this.BlockDefinition.IdlePowerConsumption;

    private void PowerDistributor_PowerStateChaged(MyResourceStateEnum newState) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.Closed)
        return;
      this.UpdateIsWorking();
    }), "MyCryoChamber::UpdateIsWorking");

    private void Receiver_IsPoweredChanged() => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.Closed)
        return;
      this.UpdateIsWorking();
    }), "MyCryoChamber::UpdateIsWorking");

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateIsWorking();
      this.CheckEmissiveState(false);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyPlayer.PlayerId? nullable = this.m_attachedPlayerId.Value;
      MyPlayer.PlayerId? currentPlayerId = this.m_currentPlayerId;
      if ((nullable.HasValue == currentPlayerId.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != currentPlayerId.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
        return;
      this.m_attachedPlayerId.Value = this.m_currentPlayerId;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CryoChamber builderCubeBlock = (MyObjectBuilder_CryoChamber) base.GetObjectBuilderCubeBlock(copy);
      if (this.m_currentPlayerId.HasValue)
      {
        builderCubeBlock.SteamId = new ulong?(this.m_currentPlayerId.Value.SteamId);
        builderCubeBlock.SerialId = new int?(this.m_currentPlayerId.Value.SerialId);
      }
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void PlacePilotInSeat(MyCharacter pilot)
    {
      this.m_pilotLights = pilot.LightEnabled;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        pilot.EnableLights(false);
      this.m_pilotCameraInFP = pilot.IsInFirstPersonView;
      MyCharacterJetpackComponent jetpackComp = pilot.JetpackComp;
      if (jetpackComp != null)
      {
        this.m_pilotJetpack = jetpackComp.TurnedOn;
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          jetpackComp.TurnOnJetpack(false);
      }
      pilot.Sit(true, MySession.Static.LocalCharacter == pilot, false, this.BlockDefinition.CharacterAnimation);
      pilot.SuitBattery.ResourceSource.Enabled = true;
      MatrixD worldMatrix = this.m_characterDummy * this.WorldMatrix;
      pilot.PositionComp.SetWorldMatrix(ref worldMatrix, (object) this);
      this.CheckEmissiveState(false);
    }

    protected void OnCryoChamberControlAcquired(MyEntityController controller) => this.m_currentPlayerId = new MyPlayer.PlayerId?(controller.Player.Id);

    protected override void RemovePilotFromSeat(MyCharacter pilot)
    {
      if (pilot == MySession.Static.LocalCharacter)
      {
        MyHudCameraOverlay.Enabled = false;
        this.Render.Visible = true;
      }
      this.m_currentPlayerId = new MyPlayer.PlayerId?();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_attachedPlayerId.Value = new MyPlayer.PlayerId?();
        if (this.m_pilotLights)
          pilot.EnableLights(true);
        if (this.m_pilotJetpack && pilot.JetpackComp != null)
          pilot.JetpackComp.TurnOnJetpack(true);
      }
      pilot.IsInFirstPersonView = this.m_pilotCameraInFP;
      this.m_pilotLights = false;
      this.m_pilotJetpack = false;
      this.m_pilotCameraInFP = true;
      this.CheckEmissiveState(false);
    }

    public override void CheckEmissiveState(bool force = false)
    {
      if (this.IsWorking)
        this.SetEmissiveStateWorking();
      else if (this.IsFunctional)
        this.SetEmissiveStateDisabled();
      else
        this.SetEmissiveStateDamaged();
    }

    public override UseActionResult CanUse(
      UseActionEnum actionEnum,
      Sandbox.Game.Entities.IMyControllableEntity user)
    {
      if (!this.IsFunctional)
        return UseActionResult.CockpitDamaged;
      if (!this.IsWorking)
        return UseActionResult.Unpowered;
      return this.m_pilot != null ? UseActionResult.UsedBySomeoneElse : base.CanUse(actionEnum, user);
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (!MyFakes.ENABLE_OXYGEN_SOUNDS)
        return;
      this.UpdateSound(this.Pilot != null && this.Pilot == MySession.Static.LocalCharacter);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.ResourceSink.Update();
      if (!this.m_retryAttachPilot)
        return;
      this.m_retryAttachPilot = false;
      this.AttachedPlayerChanged();
    }

    private void SetOverlay()
    {
      if (!this.IsLocalCharacterInside())
        return;
      MyHudCameraOverlay.TextureName = this.m_overlayTextureName;
      MyHudCameraOverlay.Enabled = true;
      this.Render.Visible = false;
    }

    public override bool SetEmissiveStateWorking()
    {
      if (!this.IsWorking)
        return false;
      if (this.Pilot != null)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]);
      return (double) this.OxygenFillLevel > 0.0 || MySession.Static.CreativeMode ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]);
    }

    protected override void OnInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.Closed)
          return;
        base.OnInputChanged(resourceTypeId, oldInput, sink);
      }), "MyCryoChamber::OnInputChanged");
      this.CheckEmissiveState(false);
    }

    public override MyToolbarType ToolbarType => MyToolbarType.None;

    protected override void ComponentStack_IsFunctionalChanged()
    {
      MyCharacter pilot = this.m_pilot;
      MyEntityController controller = this.ControllerInfo.Controller;
      base.ComponentStack_IsFunctionalChanged();
      if (this.IsFunctional || pilot == null || controller != null)
        return;
      if (MySession.Static.CreativeMode)
        pilot.Close();
      else
        pilot.DoDamage(1000f, MyDamageType.Destruction, false, 0L);
    }

    public override void OnUnregisteredFromGridSystems()
    {
      MyCharacter pilot = this.m_pilot;
      MyEntityController controller = this.ControllerInfo.Controller;
      base.OnUnregisteredFromGridSystems();
      if (pilot != null && controller == null && MySession.Static.CreativeMode)
        pilot.Close();
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    private bool IsLocalCharacterInside() => MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter == this.Pilot;

    private void UpdateSound(bool isUsed)
    {
      if (this.m_soundEmitter == null || !this.IsWorking)
        return;
      if (isUsed)
      {
        if (!(this.m_soundEmitter.SoundId != this.BlockDefinition.InsideSound.Arcade) || !(this.m_soundEmitter.SoundId != this.BlockDefinition.InsideSound.Realistic))
          return;
        this.m_soundEmitter.Force2D = true;
        this.m_soundEmitter.Force3D = false;
        if (this.m_soundEmitter.SoundId == this.BlockDefinition.OutsideSound.Arcade || this.m_soundEmitter.SoundId != this.BlockDefinition.OutsideSound.Realistic)
          this.m_soundEmitter.PlaySound(this.BlockDefinition.InsideSound, true);
        else
          this.m_soundEmitter.PlaySound(this.BlockDefinition.InsideSound, true, true);
      }
      else
      {
        if (!(this.m_soundEmitter.SoundId != this.BlockDefinition.OutsideSound.Arcade) || !(this.m_soundEmitter.SoundId != this.BlockDefinition.OutsideSound.Realistic))
          return;
        this.m_soundEmitter.Force2D = false;
        this.m_soundEmitter.Force3D = true;
        this.m_soundEmitter.PlaySound(this.BlockDefinition.OutsideSound, true);
      }
    }

    public void CameraAttachedToChanged(
      IMyCameraController oldController,
      IMyCameraController newController)
    {
      if (oldController != this)
        return;
      MyRenderProxy.UpdateRenderObjectVisibility(this.Render.RenderObjectIDs[0], true, false);
    }

    protected override void OnControlAcquired_UpdateCamera() => base.OnControlAcquired_UpdateCamera();

    public override void UpdateCockpitModel() => base.UpdateCockpitModel();

    public bool TryToControlPilot(MyPlayer player)
    {
      if (this.Pilot == null)
        return false;
      MyPlayer.PlayerId id = player.Id;
      MyPlayer.PlayerId? currentPlayerId1 = this.m_currentPlayerId;
      if ((currentPlayerId1.HasValue ? (id != currentPlayerId1.GetValueOrDefault() ? 1 : 0) : 1) != 0)
        return false;
      MyPlayer.PlayerId? nullable = this.m_attachedPlayerId.Value;
      MyPlayer.PlayerId? currentPlayerId2 = this.m_currentPlayerId;
      if ((nullable.HasValue == currentPlayerId2.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == currentPlayerId2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
        this.AttachedPlayerChanged();
      else
        this.m_attachedPlayerId.Value = this.m_currentPlayerId;
      return true;
    }

    internal void OnPlayerLoaded()
    {
    }

    private void AttachedPlayerChanged()
    {
      if (!this.m_attachedPlayerId.Value.HasValue)
        return;
      MyPlayer playerById = Sandbox.Game.Multiplayer.Sync.Players.GetPlayerById(new MyPlayer.PlayerId(this.m_attachedPlayerId.Value.Value.SteamId, this.m_attachedPlayerId.Value.Value.SerialId));
      if (playerById != null)
      {
        if (this.Pilot != null)
        {
          if (playerById == MySession.Static.LocalHumanPlayer)
          {
            this.OnPlayerLoaded();
            if (MySession.Static.CameraController != this)
              MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
          }
          playerById.Controller.TakeControl((Sandbox.Game.Entities.IMyControllableEntity) this);
          playerById.Identity.ChangeCharacter(this.Pilot);
        }
        else
        {
          MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
        }
      }
      else
        this.m_retryAttachPilot = true;
    }

    protected class m_attachedPlayerId\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyPlayer.PlayerId?, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyPlayer.PlayerId?, SyncDirection.FromServer>(obj1, obj2));
        ((MyCryoChamber) obj0).m_attachedPlayerId = (VRage.Sync.Sync<MyPlayer.PlayerId?, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyCryoChamber\u003C\u003EActor : IActivator, IActivator<MyCryoChamber>
    {
      object IActivator.CreateInstance() => (object) new MyCryoChamber();

      MyCryoChamber IActivator<MyCryoChamber>.CreateInstance() => new MyCryoChamber();
    }
  }
}
