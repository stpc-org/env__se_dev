// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterJetpackComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
  public class MyCharacterJetpackComponent : MyCharacterComponent
  {
    public const float FuelLowThresholdPlayer = 0.1f;
    public const float FuelCriticalThresholdPlayer = 0.05f;
    public const float MinimumInputRequirement = 1E-06f;
    public const float ROTATION_FACTOR = 0.02f;
    private int m_lastPowerCheckFrame;
    private bool m_isPowered;
    private const float AUTO_ENABLE_JETPACK_INTERVAL = 1f;
    private bool m_isOnPlanetSurface;
    private int m_planetSurfaceRaycastCounter;
    public Action<bool> OnPoweredChanged;

    private MyJetpackThrustComponent ThrustComp => this.Character.Components.Get<MyEntityThrustComponent>() as MyJetpackThrustComponent;

    public float CurrentAutoEnableDelay { get; set; }

    public float ForceMagnitude { get; private set; }

    public float MinPowerConsumption { get; private set; }

    public float MaxPowerConsumption { get; private set; }

    public Vector3 FinalThrust => this.ThrustComp.FinalThrust;

    public bool CanDrawThrusts => this.Character.ActualUpdateFrame >= 2UL;

    public bool DampenersTurnedOn => this.ThrustComp.DampenersEnabled;

    public MyGasProperties FuelDefinition { get; private set; }

    public MyFuelConverterInfo FuelConverterDefinition { get; private set; }

    public bool IsPowered
    {
      get
      {
        int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
        if (this.m_lastPowerCheckFrame < gameplayFrameCounter)
        {
          this.m_lastPowerCheckFrame = gameplayFrameCounter;
          this.CheckPower();
        }
        return this.m_isPowered;
      }
    }

    public bool DampenersEnabled => this.ThrustComp != null && this.ThrustComp.DampenersEnabled;

    public bool Running => this.TurnedOn && this.IsPowered && !this.Character.IsDead;

    public bool TurnedOn { get; private set; }

    public float MinPlanetaryInfluence { get; private set; }

    public float MaxPlanetaryInfluence { get; private set; }

    public float EffectivenessAtMaxInfluence { get; private set; }

    public float EffectivenessAtMinInfluence { get; private set; }

    public bool NeedsAtmosphereForInfluence { get; private set; }

    public float ConsumptionFactorPerG { get; private set; }

    public bool IsFlying { get; private set; }

    public MyCharacterJetpackComponent()
    {
      this.CurrentAutoEnableDelay = 0.0f;
      this.TurnedOn = false;
      this.OnPoweredChanged += new Action<bool>(this.PowerChangedInternal);
    }

    private void PowerChangedInternal(bool obj)
    {
      if (this.m_isPowered || !this.TurnedOn)
        return;
      this.TurnOnJetpack(false);
    }

    public virtual void Init(MyObjectBuilder_Character characterBuilder)
    {
      if (characterBuilder == null)
        return;
      this.CurrentAutoEnableDelay = characterBuilder.AutoenableJetpackDelay;
      if (this.ThrustComp != null)
        this.Character.Components.Remove<MyJetpackThrustComponent>();
      MyObjectBuilder_ThrustDefinition thrustProperties = this.Character.Definition.Jetpack.ThrustProperties;
      this.FuelConverterDefinition = (MyFuelConverterInfo) null;
      MyFuelConverterInfo fuelConverterInfo;
      if (MyFakes.ENABLE_HYDROGEN_FUEL)
      {
        fuelConverterInfo = this.Character.Definition.Jetpack.ThrustProperties.FuelConverter;
      }
      else
      {
        fuelConverterInfo = new MyFuelConverterInfo();
        fuelConverterInfo.Efficiency = 1f;
      }
      this.FuelConverterDefinition = fuelConverterInfo;
      MyDefinitionId defId = new MyDefinitionId();
      if (!this.FuelConverterDefinition.FuelId.IsNull())
        defId = (MyDefinitionId) thrustProperties.FuelConverter.FuelId;
      MyGasProperties definition = (MyGasProperties) null;
      if (MyFakes.ENABLE_HYDROGEN_FUEL)
        MyDefinitionManager.Static.TryGetDefinition<MyGasProperties>(defId, out definition);
      MyGasProperties myGasProperties1 = definition;
      if (myGasProperties1 == null)
      {
        MyGasProperties myGasProperties2 = new MyGasProperties();
        myGasProperties2.Id = MyResourceDistributorComponent.ElectricityId;
        myGasProperties2.EnergyDensity = 1f;
        myGasProperties1 = myGasProperties2;
      }
      this.FuelDefinition = myGasProperties1;
      this.ForceMagnitude = thrustProperties.ForceMagnitude;
      this.MinPowerConsumption = thrustProperties.MinPowerConsumption;
      this.MaxPowerConsumption = thrustProperties.MaxPowerConsumption;
      this.MinPlanetaryInfluence = thrustProperties.MinPlanetaryInfluence;
      this.MaxPlanetaryInfluence = thrustProperties.MaxPlanetaryInfluence;
      this.EffectivenessAtMinInfluence = thrustProperties.EffectivenessAtMinInfluence;
      this.EffectivenessAtMaxInfluence = thrustProperties.EffectivenessAtMaxInfluence;
      this.NeedsAtmosphereForInfluence = thrustProperties.NeedsAtmosphereForInfluence;
      this.ConsumptionFactorPerG = thrustProperties.ConsumptionFactorPerG;
      MyEntityThrustComponent component = (MyEntityThrustComponent) new MyJetpackThrustComponent();
      component.Init();
      this.Character.Components.Add<MyEntityThrustComponent>(component);
      this.ThrustComp.DampenersEnabled = characterBuilder.DampenersEnabled;
      foreach (Vector3I intDirection in Base6Directions.IntDirections)
        this.ThrustComp.Register((MyEntity) this.Character, intDirection, (Func<bool>) null);
      component.ResourceSink((MyEntity) this.Character).TemporaryConnectedEntity = (IMyEntity) this.Character;
      this.Character.SuitRechargeDistributor.AddSink(component.ResourceSink((MyEntity) this.Character));
      this.TurnOnJetpack(characterBuilder.JetpackEnabled, true, true);
    }

    public virtual void GetObjectBuilder(MyObjectBuilder_Character characterBuilder)
    {
      characterBuilder.DampenersEnabled = this.DampenersTurnedOn;
      bool flag = this.TurnedOn;
      if (MySession.Static.ControlledEntity is MyCockpit)
        flag = (MySession.Static.ControlledEntity as MyCockpit).PilotJetpackEnabledBackup;
      characterBuilder.JetpackEnabled = flag;
      characterBuilder.AutoenableJetpackDelay = this.CurrentAutoEnableDelay;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      if (this.Entity.MarkedForClose)
        return;
      this.Character.SuitRechargeDistributor.RemoveSink(this.ThrustComp.ResourceSink((MyEntity) this.Character), markedForClose: this.Entity.MarkedForClose);
      base.OnBeforeRemovedFromContainer();
    }

    public override void Simulate() => this.ThrustComp.UpdateBeforeSimulation(Sync.IsServer || this.Character == MySession.Static.LocalCharacter, this.Character.RelativeDampeningEntity);

    public override void OnCharacterDead()
    {
      base.OnCharacterDead();
      this.TurnOnJetpack(false);
    }

    public void TurnOnJetpack(bool newState, bool fromInit = false, bool fromLoad = false)
    {
      MyEntityController controller = this.Character.ControllerInfo.Controller;
      newState = newState && MySession.Static.Settings.EnableJetpack;
      newState = newState && this.Character.Definition.Jetpack != null;
      newState = newState && (!MySession.Static.SurvivalMode || MyFakes.ENABLE_JETPACK_IN_SURVIVAL || controller == null || MySession.Static.CreativeToolsEnabled(controller.Player.Id.SteamId));
      bool flag1 = this.TurnedOn != newState;
      this.TurnedOn = newState;
      this.ThrustComp.Enabled = newState;
      this.ThrustComp.ControlThrust = Vector3.Zero;
      this.ThrustComp.MarkDirty();
      this.ThrustComp.UpdateBeforeSimulation(true, this.Character.RelativeDampeningEntity);
      if (!this.ThrustComp.Enabled)
        this.ThrustComp.SetRequiredFuelInput(ref this.FuelDefinition.Id, 1E-06f, (MyEntityThrustComponent.MyConveyorConnectedGroup) null);
      this.ThrustComp.ResourceSink((MyEntity) this.Character).Update();
      if (!this.Character.ControllerInfo.IsLocallyControlled() && !fromInit && !Sync.IsServer)
        return;
      MyCharacterMovementEnum currentMovementState = this.Character.GetCurrentMovementState();
      if (currentMovementState == MyCharacterMovementEnum.Sitting)
        return;
      if (this.TurnedOn)
        this.Character.StopFalling();
      bool flag2 = false;
      bool flag3 = newState;
      if (!this.IsPowered & flag3 && (this.Character.ControllerInfo.Controller != null && !MySession.Static.CreativeToolsEnabled(this.Character.ControllerInfo.Controller.Player.Id.SteamId) || MySession.Static.LocalCharacter != this.Character && !Sync.IsServer))
      {
        flag3 = false;
        flag2 = true;
      }
      if (flag3)
      {
        if (this.Character.IsOnLadder)
          this.Character.GetOffLadder();
        this.Character.IsUsing = (MyEntity) null;
      }
      if (flag1 && !this.Character.IsDead)
        this.Character.UpdateCharacterPhysics();
      if (MySession.Static.ControlledEntity == this.Character & flag1 && !fromLoad && flag2)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        this.TurnedOn = false;
        this.ThrustComp.Enabled = false;
        this.ThrustComp.ControlThrust = Vector3.Zero;
        this.ThrustComp.MarkDirty();
        this.ThrustComp.UpdateBeforeSimulation(true, this.Character.RelativeDampeningEntity);
        this.ThrustComp.SetRequiredFuelInput(ref this.FuelDefinition.Id, 1E-06f, (MyEntityThrustComponent.MyConveyorConnectedGroup) null);
        this.ThrustComp.ResourceSink((MyEntity) this.Character).Update();
      }
      MyCharacterProxy characterProxy = this.Character.Physics.CharacterProxy;
      if (characterProxy != null)
      {
        MatrixD worldMatrix = this.Character.WorldMatrix;
        characterProxy.SetForwardAndUp((Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up);
        characterProxy.EnableFlyingState(this.Running);
        if (currentMovementState != MyCharacterMovementEnum.Died && !this.Character.IsOnLadder)
        {
          if (!this.Running && (characterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_IN_AIR || characterProxy.GetState() == (HkCharacterStateType) 5))
            this.Character.StartFalling();
          else if (currentMovementState != MyCharacterMovementEnum.Standing && !newState)
          {
            this.Character.PlayCharacterAnimation("Idle", MyBlendOption.Immediate, MyFrameOption.Loop, 0.2f);
            this.Character.SetCurrentMovementState(MyCharacterMovementEnum.Standing);
            currentMovementState = this.Character.GetCurrentMovementState();
          }
        }
        if (this.Running && currentMovementState != MyCharacterMovementEnum.Died)
        {
          this.Character.PlayCharacterAnimation("Jetpack", MyBlendOption.Immediate, MyFrameOption.Loop, 0.0f);
          this.Character.SetCurrentMovementState(MyCharacterMovementEnum.Flying);
          this.Character.SetLocalHeadAnimation(new float?(0.0f), new float?(0.0f), 0.3f);
          characterProxy.PosX = 0.0f;
          characterProxy.PosY = 0.0f;
        }
        if (fromLoad || newState || (double) this.Character.Physics.Gravity.LengthSquared() > 0.100000001490116)
          return;
        this.CurrentAutoEnableDelay = -1f;
      }
      else
      {
        if (!this.Running || currentMovementState == MyCharacterMovementEnum.Died)
          return;
        this.Character.PlayCharacterAnimation("Jetpack", MyBlendOption.Immediate, MyFrameOption.Loop, 0.0f);
        this.Character.SetLocalHeadAnimation(new float?(0.0f), new float?(0.0f), 0.3f);
      }
    }

    public void UpdateFall()
    {
      if ((double) this.CurrentAutoEnableDelay < 1.0)
        return;
      this.ThrustComp.DampenersEnabled = true;
      this.TurnOnJetpack(true);
      this.CurrentAutoEnableDelay = -1f;
    }

    public void MoveAndRotate(
      ref Vector3 moveIndicator,
      ref Vector2 rotationIndicator,
      float roll,
      bool canRotate)
    {
      MyCharacterProxy characterProxy = this.Character.Physics.CharacterProxy;
      this.ThrustComp.ControlThrust = Vector3.Zero;
      this.Character.SwitchAnimation(MyCharacterMovementEnum.Flying);
      this.Character.SetCurrentMovementState(MyCharacterMovementEnum.Flying);
      this.IsFlying = (double) moveIndicator.LengthSquared() != 0.0;
      switch (characterProxy != null ? characterProxy.GetState() : HkCharacterStateType.HK_CHARACTER_ON_GROUND)
      {
        case HkCharacterStateType.HK_CHARACTER_IN_AIR:
        case (HkCharacterStateType) 5:
          this.Character.PlayCharacterAnimation("Jetpack", MyBlendOption.Immediate, MyFrameOption.Loop, 0.2f);
          this.Character.CanJump = true;
          break;
      }
      MatrixD worldMatrix = this.Character.WorldMatrix;
      if (canRotate)
      {
        MatrixD matrixD1 = MatrixD.Identity;
        MatrixD matrixD2 = MatrixD.Identity;
        MatrixD matrixD3 = MatrixD.Identity;
        if ((double) Math.Abs(rotationIndicator.X) > 1.40129846432482E-45)
        {
          if (this.Character.Definition.VerticalPositionFlyingOnly)
            this.Character.SetHeadLocalXAngle(this.Character.HeadLocalXAngle - rotationIndicator.X * this.Character.RotationSpeed);
          else
            matrixD1 = MatrixD.CreateFromAxisAngle(worldMatrix.Right, -(double) rotationIndicator.X * (double) this.Character.RotationSpeed * 0.0199999995529652);
        }
        if ((double) Math.Abs(rotationIndicator.Y) > 1.40129846432482E-45)
          matrixD2 = MatrixD.CreateFromAxisAngle(worldMatrix.Up, -(double) rotationIndicator.Y * (double) this.Character.RotationSpeed * 0.0199999995529652);
        if (!this.Character.Definition.VerticalPositionFlyingOnly && (double) Math.Abs(roll) > 1.40129846432482E-45)
          matrixD3 = MatrixD.CreateFromAxisAngle(worldMatrix.Forward, (double) roll * 0.0199999995529652);
        float y = this.Character.ModelCollision.BoundingBoxSizeHalf.Y;
        Vector3D vector3D = this.Character.Physics.GetWorldMatrix().Translation + worldMatrix.Up * (double) y;
        MatrixD matrixD4 = matrixD1 * matrixD2 * matrixD3;
        MatrixD matrixD5 = worldMatrix.GetOrientation() * matrixD4;
        matrixD5.Translation = vector3D - matrixD5.Up * (double) y;
        this.Character.WorldMatrix = matrixD5;
        this.Character.ClearShapeContactPoints();
      }
      Vector3 position = moveIndicator;
      if (this.Character.Definition.VerticalPositionFlyingOnly)
      {
        float num1 = (float) Math.Sign(this.Character.HeadLocalXAngle);
        double x = (double) Math.Abs(MathHelper.ToRadians(this.Character.HeadLocalXAngle));
        double y = 1.95;
        double num2 = Math.Pow(x, y) * (x / Math.Pow((double) MathHelper.ToRadians(89f), y));
        MatrixD fromAxisAngle = MatrixD.CreateFromAxisAngle(Vector3D.Right, (double) num1 * num2);
        position = (Vector3) Vector3D.Transform(position, fromAxisAngle);
      }
      MyJetpackThrustComponent thrustComp = this.ThrustComp;
      thrustComp.ControlThrust = thrustComp.ControlThrust + position;
    }

    public bool UpdatePhysicalMovement()
    {
      this.CheckPower();
      if (!this.Running)
        return false;
      MyPhysicsBody physics = this.Character.Physics;
      MyCharacterProxy characterProxy = physics.CharacterProxy;
      if (characterProxy != null && (double) characterProxy.LinearVelocity.Length() < 1.0 / 1000.0)
        characterProxy.LinearVelocity = Vector3.Zero;
      float num = 1f;
      HkRigidBody rigidBody = physics.RigidBody;
      if ((HkReferenceObject) rigidBody != (HkReferenceObject) null)
      {
        rigidBody.Gravity = Vector3.Zero;
        if (MySession.Static.SurvivalMode || MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE)
        {
          Vector3 vector3 = num * MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.Character.PositionComp.WorldAABB.Center);
          if (vector3 != Vector3.Zero)
            rigidBody.Gravity = vector3 * MyPerGameSettings.CharacterGravityMultiplier;
        }
        return true;
      }
      if (characterProxy == null)
        return false;
      characterProxy.Gravity = Vector3.Zero;
      if (MySession.Static.SurvivalMode || MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE)
      {
        Vector3 vector3 = num * MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.Character.PositionComp.WorldAABB.Center);
        if (vector3 != Vector3.Zero)
          characterProxy.Gravity = vector3 * MyPerGameSettings.CharacterGravityMultiplier;
      }
      return true;
    }

    private void CheckPower()
    {
      int num1 = this.m_isPowered ? 1 : 0;
      this.m_isPowered = (MySession.Static.LocalCharacter == this.Character || Sync.IsServer) && (this.Character.ControllerInfo.Controller != null && MySession.Static.CreativeToolsEnabled(this.Character.ControllerInfo.Controller.Player.Id.SteamId)) || MySession.Static.CreativeToolsEnabled(this.Character.ControlSteamId) || this.ThrustComp != null && this.ThrustComp.IsThrustPoweredByType((MyEntity) this.Character, ref this.FuelDefinition.Id);
      int num2 = this.m_isPowered ? 1 : 0;
      if (num1 == num2)
        return;
      this.OnPoweredChanged.InvokeIfNotNull<bool>(this.m_isPowered);
    }

    public void EnableDampeners(bool enable)
    {
      if (this.DampenersTurnedOn == enable)
        return;
      this.ThrustComp.DampenersEnabled = enable;
    }

    public void SwitchDamping()
    {
      if (this.Character.GetCurrentMovementState() == MyCharacterMovementEnum.Died)
        return;
      this.EnableDampeners(!this.DampenersTurnedOn);
    }

    public void SwitchThrusts()
    {
      if (this.Character.GetCurrentMovementState() == MyCharacterMovementEnum.Died)
        return;
      this.TurnOnJetpack(!this.TurnedOn);
    }

    public override string ComponentTypeDebugString => "Jetpack Component";

    public void ClearMovement() => this.ThrustComp.ControlThrust = Vector3.Zero;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.NeedsUpdateSimulation = true;
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterJetpackComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterJetpackComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterJetpackComponent();

      MyCharacterJetpackComponent IActivator<MyCharacterJetpackComponent>.CreateInstance() => new MyCharacterJetpackComponent();
    }
  }
}
