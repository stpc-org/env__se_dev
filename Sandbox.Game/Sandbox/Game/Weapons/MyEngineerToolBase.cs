// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyEngineerToolBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace Sandbox.Game.Weapons
{
  public abstract class MyEngineerToolBase : MyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>, IMyEngineerToolBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    private const float LIGHT_MUZZLE_OFFSET = 0.0f;
    private const float PARTICLE_MUZZLE_OFFSET = 0.1f;
    public static float GLARE_SIZE = 0.068f;
    public static readonly float DEFAULT_REACH_DISTANCE = 2f;
    protected string m_effectId = "WelderContactPoint";
    protected float EffectScale = 1f;
    protected bool HasPrimaryEffect = true;
    protected bool HasSecondaryEffect;
    protected string SecondaryEffectName = "Dummy";
    protected Vector4 SecondaryLightColor = new Vector4(0.4f, 0.5f, 1f, 1f);
    protected float SecondaryLightFalloff = 2f;
    protected float SecondaryLightRadius = 7f;
    protected float SecondaryLightIntensityLower = 0.4f;
    protected float SecondaryLightIntensityUpper = 0.5f;
    protected float SecondaryLightGlareSize = MyEngineerToolBase.GLARE_SIZE;
    protected MyShootActionEnum? EffectAction;
    private MyShootActionEnum? m_previousEffect;
    protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();
    protected MyEntity3DSoundEmitter m_soundEmitter;
    protected MyCharacter Owner;
    protected MyToolBase m_gunBase;
    private int m_lastTimeShoot;
    protected int m_lastTimeSelected;
    protected bool m_activated;
    protected bool m_shooting;
    private MyParticleEffect m_toolEffect;
    private MyParticleEffect m_toolSecondaryEffect;
    private MyLight m_toolEffectLight;
    private int m_lastMarkTime = -1;
    private int m_markedComponent = -1;
    protected bool m_tryingToShoot;
    private bool m_wasPowered;
    protected MyCasterComponent m_raycastComponent;
    private MyResourceSinkComponent m_sinkComp;
    private int m_shootFrameCounter;
    private NumberFormatInfo m_oneDecimal = new NumberFormatInfo()
    {
      NumberDecimalDigits = 1,
      PercentDecimalDigits = 1
    };
    protected MyHandItemDefinition m_handItemDef;
    protected MyPhysicalItemDefinition m_physItemDef;
    protected float m_speedMultiplier = 1f;
    protected float m_distanceMultiplier = 1f;
    private MyFlareDefinition m_flare;
    protected object LastTargetObject;
    protected int LastTargetStamp;

    public bool IsDeconstructor => false;

    public int ToolCooldownMs { get; private set; }

    public int EffectStopMs => this.ToolCooldownMs * 2;

    public string EffectId => this.m_effectId;

    public long OwnerId => this.Owner != null ? this.Owner.EntityId : 0L;

    public long OwnerIdentityId => this.Owner != null ? this.Owner.GetPlayerIdentityId() : 0L;

    public MyToolBase GunBase => this.m_gunBase;

    public Vector3I TargetCube => this.m_raycastComponent == null || this.m_raycastComponent.HitBlock == null ? Vector3I.Zero : this.m_raycastComponent.HitBlock.Position;

    public bool HasHitBlock => this.m_raycastComponent.HitBlock != null;

    public MyResourceSinkComponent SinkComp
    {
      get => this.m_sinkComp;
      set
      {
        if (this.Components.Contains(typeof (MyResourceSinkComponent)))
          this.Components.Remove<MyResourceSinkComponent>();
        this.Components.Add<MyResourceSinkComponent>(value);
        this.m_sinkComp = value;
      }
    }

    public bool IsShooting => this.m_activated;

    public bool ForceAnimationInsteadOfIK => false;

    public bool IsBlocking => false;

    public bool IsPreheated => this.m_shootFrameCounter >= 2;

    protected bool WasJustSelected => this.m_lastTimeSelected == MySandboxGame.TotalGamePlayTimeInMilliseconds;

    public Vector3 SensorDisplacement { get; set; }

    protected MyInventory CharacterInventory { get; private set; }

    public MyObjectBuilder_PhysicalGunObject PhysicalObject { get; protected set; }

    public float BackkickForcePerSecond => 0.0f;

    public float ShakeAmount { get; protected set; }

    protected bool HasCubeHighlight { get; set; }

    public Color HighlightColor { get; set; }

    public MyStringId HighlightMaterial { get; set; }

    public bool EnabledInWorldRules => true;

    public abstract bool IsSkinnable { get; }

    public bool CanBeDrawn() => this.Owner != null && this.Owner == MySession.Static.ControlledEntity && (this.m_raycastComponent.HitCubeGrid != null && this.m_raycastComponent.HitCubeGrid != null) && this.HasCubeHighlight && !MyFakes.HIDE_ENGINEER_TOOL_HIGHLIGHT;

    public MyEngineerToolBase(int cooldownMs)
    {
      this.ToolCooldownMs = cooldownMs;
      this.m_activated = false;
      this.m_wasPowered = false;
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.Render.NeedsDraw = true;
      (this.PositionComp as MyPositionComponent).WorldPositionChanged = new Action<object>(this.WorldPositionChanged);
      this.Render = (MyRenderComponentBase) new MyRenderComponentEngineerTool();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentEngineerTool(this));
    }

    public void Init(MyObjectBuilder_EntityBase builder, MyDefinitionId id) => this.Init(builder, MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(id));

    public void Init(MyObjectBuilder_EntityBase builder, MyHandItemDefinition definition)
    {
      this.m_handItemDef = definition;
      if (definition != null)
      {
        this.m_physItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(definition.Id);
        this.m_gunBase = new MyToolBase(this.m_handItemDef.MuzzlePosition, this.WorldMatrix);
      }
      else
        this.m_gunBase = new MyToolBase(Vector3.Zero, this.WorldMatrix);
      this.Init(builder);
      if (this.PhysicalObject != null)
        this.PhysicalObject.GunEntity = builder;
      if (definition is MyEngineerToolBaseDefinition)
      {
        this.m_speedMultiplier = (this.m_handItemDef as MyEngineerToolBaseDefinition).SpeedMultiplier;
        this.m_distanceMultiplier = (this.m_handItemDef as MyEngineerToolBaseDefinition).DistanceMultiplier;
        string flare = (this.m_handItemDef as MyEngineerToolBaseDefinition).Flare;
        if (flare != "")
        {
          if (!(MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), flare)) is MyFlareDefinition myFlareDefinition))
            myFlareDefinition = new MyFlareDefinition();
          this.m_flare = myFlareDefinition;
        }
        else
          this.m_flare = new MyFlareDefinition();
      }
      this.m_raycastComponent = new MyCasterComponent((MyDrillSensorBase) new MyDrillSensorRayCast(0.0f, MyEngineerToolBase.DEFAULT_REACH_DISTANCE * this.m_distanceMultiplier, (MyDefinitionBase) this.PhysicalItemDefinition));
      this.m_raycastComponent.SetPointOfReference(this.m_gunBase.GetMuzzleWorldPosition());
      this.Components.Add<MyCasterComponent>(this.m_raycastComponent);
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), 0.0001f, new Func<float>(this.CalculateRequiredPower));
      this.SinkComp = resourceSinkComponent;
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
    }

    protected virtual bool ShouldBePowered() => this.m_tryingToShoot;

    protected float CalculateRequiredPower() => !this.ShouldBePowered() ? 1E-06f : this.SinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);

    private void UpdatePower()
    {
      bool flag = this.ShouldBePowered();
      if (flag == this.m_wasPowered)
        return;
      this.m_wasPowered = flag;
      this.SinkComp.Update();
    }

    protected IMyDestroyableObject GetTargetDestroyable() => this.m_raycastComponent.HitDestroyableObj;

    public MySlimBlock GetTargetBlock()
    {
      if (Sync.IsServer)
      {
        MyCharacter owner = this.Owner;
        if ((owner != null ? ((ulong) owner.AimedGrid > 0UL ? 1 : 0) : 1) != 0 && Sandbox.Game.Entities.MyEntities.GetEntityById(this.Owner.AimedGrid) is MyCubeGrid entityById)
        {
          MySlimBlock cubeBlock = entityById.GetCubeBlock(this.Owner.AimedBlock);
          if (cubeBlock != null && Vector3D.Distance(cubeBlock.WorldPosition, this.Owner.WorldMatrix.Translation) <= (double) MyEngineerToolBase.DEFAULT_REACH_DISTANCE * (double) this.m_distanceMultiplier * 3.0)
            return cubeBlock;
        }
      }
      return this.ReachesCube() && this.m_raycastComponent.HitCubeGrid != null ? this.m_raycastComponent.HitBlock : (MySlimBlock) null;
    }

    protected virtual MySlimBlock GetTargetBlockForShoot() => this.GetTargetBlock();

    public MyCubeGrid GetTargetGrid() => this.m_raycastComponent.HitCubeGrid;

    protected bool ReachesCube() => this.m_raycastComponent.HitBlock != null;

    public override void OnAddedToScene(object source) => base.OnAddedToScene(source);

    public override void OnRemovedFromScene(object source)
    {
      this.RemoveHudInfo();
      base.OnRemovedFromScene(source);
      this.StopSecondaryEffect();
      this.StopEffect();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.Owner == null)
        return;
      Vector3 localWeaponPosition = this.Owner.GetLocalWeaponPosition();
      Vector3D muzzleLocalPosition = this.m_gunBase.GetMuzzleLocalPosition();
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D result;
      Vector3D.Rotate(ref muzzleLocalPosition, ref worldMatrix, out result);
      this.m_raycastComponent.SetPointOfReference(this.Owner.PositionComp.GetPosition() + localWeaponPosition + result);
      if (this.IsShooting && this.IsPreheated)
      {
        if (this.GetTargetBlockForShoot() == null)
        {
          this.EffectAction = new MyShootActionEnum?(MyShootActionEnum.SecondaryAction);
          this.ShakeAmount = this.m_handItemDef.ShakeAmountNoTarget;
        }
        else
        {
          this.EffectAction = new MyShootActionEnum?(MyShootActionEnum.PrimaryAction);
          this.ShakeAmount = this.m_handItemDef.ShakeAmountTarget;
        }
      }
      this.SinkComp.Update();
      if (this.IsShooting && !this.SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        this.EndShoot(MyShootActionEnum.PrimaryAction);
      this.UpdateEffect();
      this.CheckEffectType();
      if (this.Owner == null || !this.Owner.ControllerInfo.IsLocallyHumanControlled())
        return;
      if (MySession.Static.SurvivalMode)
      {
        int cameraControllerEnum = (int) MySession.Static.GetCameraControllerEnum();
        MyCharacter owner = (MyCharacter) this.CharacterInventory.Owner;
        MyCubeBuilder.Static.MaxGridDistanceFrom = new Vector3D?(owner.PositionComp.GetPosition() + owner.WorldMatrix.Up * 1.79999995231628);
      }
      else
        MyCubeBuilder.Static.MaxGridDistanceFrom = new Vector3D?();
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      this.UpdateSoundEmitter();
    }

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      if (this.Owner != null)
      {
        Vector3 zero = Vector3.Zero;
        this.Owner.GetLinearVelocity(ref zero);
        this.m_soundEmitter.SetVelocity(new Vector3?(zero));
      }
      this.m_soundEmitter.Update();
    }

    protected virtual void WorldPositionChanged(object source)
    {
      this.m_gunBase.OnWorldPositionChanged(this.PositionComp.WorldMatrixRef);
      this.UpdateSensorPosition();
      if (this.m_toolEffect != null)
        this.m_toolEffect.WorldMatrix = this.GetEffectMatrix(0.1f, MyEngineerToolBase.EffectType.Effect);
      if (this.m_toolSecondaryEffect != null)
        this.m_toolSecondaryEffect.WorldMatrix = this.GetEffectMatrix(0.1f, MyEngineerToolBase.EffectType.EffectSecondary);
      if (this.m_toolEffectLight == null)
        return;
      this.m_toolEffectLight.Position = this.GetEffectMatrix(0.0f, MyEngineerToolBase.EffectType.Light).Translation;
    }

    public void UpdateSensorPosition()
    {
      if (this.Owner == null)
        return;
      MyCharacter owner = this.Owner;
      MatrixD identity = MatrixD.Identity;
      identity.Translation = owner.WeaponPosition.LogicalPositionWorld;
      identity.Right = owner.WorldMatrix.Right;
      identity.Forward = owner.WeaponPosition.LogicalOrientationWorld;
      identity.Up = (Vector3D) Vector3.Cross((Vector3) identity.Right, (Vector3) identity.Forward);
      this.m_raycastComponent.OnWorldPosChanged(ref identity);
    }

    public virtual bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (action == MyShootActionEnum.PrimaryAction)
      {
        if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot < this.ToolCooldownMs)
        {
          status = MyGunStatusEnum.Cooldown;
          return false;
        }
        status = MyGunStatusEnum.OK;
        return true;
      }
      status = MyGunStatusEnum.Failed;
      return false;
    }

    public virtual void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      this.m_shooting = true;
      if (action != MyShootActionEnum.PrimaryAction)
        return;
      this.m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      ++this.m_shootFrameCounter;
      this.m_tryingToShoot = true;
      this.SinkComp.Update();
      if (!this.SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        this.EffectAction = new MyShootActionEnum?();
      else
        this.m_activated = true;
    }

    public virtual void BeginShoot(MyShootActionEnum action)
    {
    }

    public bool ShouldEndShootOnPause(MyShootActionEnum action) => true;

    public bool CanDoubleClickToStick(MyShootActionEnum action) => false;

    public virtual void EndShoot(MyShootActionEnum action)
    {
      this.EffectAction = new MyShootActionEnum?();
      this.StopLoopSound();
      this.ShakeAmount = 0.0f;
      this.m_tryingToShoot = false;
      this.m_shooting = false;
      this.SinkComp.Update();
      this.m_activated = false;
      this.m_shootFrameCounter = 0;
      this.m_isActionDoubleClicked[action] = false;
    }

    public virtual void OnFailShoot(MyGunStatusEnum status)
    {
      if (status != MyGunStatusEnum.Failed)
        return;
      this.EffectAction = new MyShootActionEnum?(MyShootActionEnum.SecondaryAction);
    }

    protected virtual void StartLoopSound(bool effect)
    {
    }

    protected virtual void StopLoopSound()
    {
    }

    protected virtual void StopSound()
    {
    }

    protected virtual MatrixD GetEffectMatrix(
      float muzzleOffset,
      MyEngineerToolBase.EffectType effectType)
    {
      if (this.m_raycastComponent.HitCubeGrid == null || this.m_raycastComponent.HitBlock == null)
      {
        Vector3D muzzleWorldPosition = this.m_gunBase.GetMuzzleWorldPosition();
        MatrixD matrixD = this.PositionComp.WorldMatrixRef;
        Vector3D forward = matrixD.Forward;
        matrixD = this.PositionComp.WorldMatrixRef;
        Vector3D up = matrixD.Up;
        return MatrixD.CreateWorld(muzzleWorldPosition, forward, up);
      }
      float num = Vector3.Dot((Vector3) (this.m_raycastComponent.HitPosition - this.m_gunBase.GetMuzzleWorldPosition()), (Vector3) this.PositionComp.WorldMatrixRef.Forward);
      Vector3D muzzleWorldPosition1 = this.m_gunBase.GetMuzzleWorldPosition();
      MatrixD matrixD1 = this.PositionComp.WorldMatrixRef;
      Vector3D vector3D1 = matrixD1.Forward * ((double) num * (double) muzzleOffset);
      Vector3D vector3D2 = muzzleWorldPosition1 + vector3D1;
      Vector3D position = (double) num <= 0.0 || (double) muzzleOffset != 0.0 ? vector3D2 : this.m_gunBase.GetMuzzleWorldPosition();
      matrixD1 = this.PositionComp.WorldMatrixRef;
      Vector3D forward1 = matrixD1.Forward;
      matrixD1 = this.PositionComp.WorldMatrixRef;
      Vector3D up1 = matrixD1.Up;
      return MatrixD.CreateWorld(position, forward1, up1);
    }

    private void CheckEffectType()
    {
      if (this.m_previousEffect.HasValue && this.m_toolEffect == null)
        this.m_previousEffect = new MyShootActionEnum?();
      MyShootActionEnum? effectAction1 = this.EffectAction;
      MyShootActionEnum? previousEffect = this.m_previousEffect;
      if (effectAction1.GetValueOrDefault() == previousEffect.GetValueOrDefault() & effectAction1.HasValue == previousEffect.HasValue)
        return;
      if (this.m_previousEffect.HasValue)
        this.StopEffect();
      this.m_previousEffect = new MyShootActionEnum?();
      if (!this.EffectAction.HasValue || MySector.MainCamera.GetDistanceFromPoint(this.PositionComp.GetPosition()) >= 150.0)
        return;
      MyShootActionEnum? effectAction2 = this.EffectAction;
      MyShootActionEnum myShootActionEnum1 = MyShootActionEnum.PrimaryAction;
      if (effectAction2.GetValueOrDefault() == myShootActionEnum1 & effectAction2.HasValue && this.HasPrimaryEffect)
      {
        this.StartEffect();
        this.m_previousEffect = new MyShootActionEnum?(MyShootActionEnum.PrimaryAction);
      }
      else
      {
        effectAction2 = this.EffectAction;
        MyShootActionEnum myShootActionEnum2 = MyShootActionEnum.SecondaryAction;
        if (!(effectAction2.GetValueOrDefault() == myShootActionEnum2 & effectAction2.HasValue) || !this.HasSecondaryEffect)
          return;
        this.StartSecondaryEffect();
        this.m_previousEffect = new MyShootActionEnum?(MyShootActionEnum.SecondaryAction);
      }
    }

    public virtual bool CanStartEffect() => true;

    protected void StartEffect()
    {
      this.StopEffect();
      if (!string.IsNullOrEmpty(this.m_effectId) && this.CanStartEffect())
      {
        MyParticlesManager.TryCreateParticleEffect(this.m_effectId, this.GetEffectMatrix(0.1f, MyEngineerToolBase.EffectType.Effect), out this.m_toolEffect);
        if (this.m_toolEffect != null)
          this.m_toolEffect.UserScale = this.EffectScale;
        this.m_toolEffectLight = this.CreatePrimaryLight();
      }
      this.UpdateEffect();
    }

    protected virtual MyLight CreatePrimaryLight()
    {
      MyLight light = MyLights.AddLight();
      if (light != null)
      {
        light.Start((Vector3D) Vector3.Zero, this.m_handItemDef.LightColor, this.m_handItemDef.LightRadius, this.DisplayNameText + " Tool Primary");
        this.CreateGlare(light);
      }
      return light;
    }

    private void CreateGlare(MyLight light)
    {
      light.GlareOn = light.LightOn;
      light.GlareQuerySize = 0.2f;
      light.GlareType = MyGlareTypeEnum.Normal;
      if (this.m_flare == null)
        return;
      light.SubGlares = this.m_flare.SubGlares;
      light.GlareSize = this.m_flare.Size;
      light.GlareIntensity = this.m_flare.Intensity;
    }

    private void StartSecondaryEffect()
    {
      this.StopEffect();
      this.StopSecondaryEffect();
      MyParticlesManager.TryCreateParticleEffect(this.SecondaryEffectName, this.GetEffectMatrix(0.1f, MyEngineerToolBase.EffectType.EffectSecondary), out this.m_toolSecondaryEffect);
      this.m_toolEffectLight = this.CreateSecondaryLight();
      this.UpdateEffect();
    }

    protected virtual MyLight CreateSecondaryLight()
    {
      MyLight light = MyLights.AddLight();
      if (light != null)
      {
        light.Start((Vector3D) Vector3.Zero, this.SecondaryLightColor, this.SecondaryLightRadius, this.DisplayNameText + " Tool Secondary");
        this.CreateGlare(light);
      }
      return light;
    }

    private void UpdateEffect()
    {
      MyShootActionEnum? effectAction1 = this.EffectAction;
      MyShootActionEnum myShootActionEnum1 = MyShootActionEnum.PrimaryAction;
      if (effectAction1.GetValueOrDefault() == myShootActionEnum1 & effectAction1.HasValue && this.m_raycastComponent.HitCubeGrid == null)
        this.EffectAction = new MyShootActionEnum?(MyShootActionEnum.SecondaryAction);
      MyShootActionEnum? effectAction2 = this.EffectAction;
      MyShootActionEnum myShootActionEnum2 = MyShootActionEnum.SecondaryAction;
      if (effectAction2.GetValueOrDefault() == myShootActionEnum2 & effectAction2.HasValue && (this.m_raycastComponent.HitCharacter != null || this.m_raycastComponent.HitEnvironmentSector != null))
        this.EffectAction = new MyShootActionEnum?(MyShootActionEnum.PrimaryAction);
      if (!this.EffectAction.HasValue)
      {
        if (this.m_soundEmitter.IsPlaying)
          this.StopLoopSound();
      }
      else
      {
        switch (this.EffectAction.Value)
        {
          case MyShootActionEnum.PrimaryAction:
            this.StartLoopSound(true);
            break;
          case MyShootActionEnum.SecondaryAction:
            this.StartLoopSound(false);
            break;
        }
      }
      if (this.m_toolEffectLight == null)
        return;
      MyShootActionEnum? effectAction3 = this.EffectAction;
      MyShootActionEnum myShootActionEnum3 = MyShootActionEnum.PrimaryAction;
      if (effectAction3.GetValueOrDefault() == myShootActionEnum3 & effectAction3.HasValue)
      {
        this.m_toolEffectLight.Intensity = MyUtils.GetRandomFloat(this.m_handItemDef.LightIntensityLower, this.m_handItemDef.LightIntensityUpper);
        if (this.m_flare != null)
        {
          this.m_toolEffectLight.GlareIntensity = this.m_toolEffectLight.Intensity * this.m_handItemDef.LightGlareIntensity * this.m_flare.Intensity;
          this.m_toolEffectLight.GlareSize = this.m_toolEffectLight.Intensity * this.m_handItemDef.LightGlareSize * this.m_flare.Size;
        }
      }
      else
      {
        this.m_toolEffectLight.Intensity = MyUtils.GetRandomFloat(this.SecondaryLightIntensityLower, this.SecondaryLightIntensityUpper);
        if (this.m_flare != null)
        {
          this.m_toolEffectLight.GlareIntensity = this.m_toolEffectLight.Intensity * this.m_handItemDef.LightGlareIntensity * this.m_flare.Intensity;
          this.m_toolEffectLight.GlareSize = this.m_toolEffectLight.Intensity * this.SecondaryLightGlareSize * this.m_flare.Size;
        }
      }
      if (this.m_flare != null)
        this.m_toolEffectLight.SubGlares = this.m_flare.SubGlares;
      this.m_toolEffectLight.UpdateLight();
    }

    protected void StopEffect()
    {
      if (this.m_toolEffect != null)
      {
        this.m_toolEffect.Stop();
        this.m_toolEffect = (MyParticleEffect) null;
      }
      if (this.m_toolEffectLight == null)
        return;
      MyLights.RemoveLight(this.m_toolEffectLight);
      this.m_toolEffectLight = (MyLight) null;
    }

    protected void StopSecondaryEffect()
    {
      if (this.m_toolSecondaryEffect == null)
        return;
      this.m_toolSecondaryEffect.Stop();
      this.m_toolSecondaryEffect = (MyParticleEffect) null;
    }

    protected override void Closing()
    {
      this.StopEffect();
      this.StopSecondaryEffect();
      this.StopLoopSound();
      base.Closing();
    }

    protected abstract void AddHudInfo();

    protected abstract void RemoveHudInfo();

    public virtual void OnControlAcquired(MyCharacter owner)
    {
      this.Owner = owner;
      this.CharacterInventory = MyEntityExtensions.GetInventory(this.Owner);
      if (owner.ControllerInfo.IsLocallyHumanControlled())
        this.AddHudInfo();
      this.m_lastTimeSelected = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.LastTargetObject = (object) null;
      this.LastTargetStamp = 0;
      if (this.Owner != MySession.Static.LocalCharacter)
        return;
      MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public virtual void OnControlReleased()
    {
      this.RemoveHudInfo();
      if (this.Owner == MySession.Static.LocalCharacter)
        MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
      this.Owner = (MyCharacter) null;
      this.CharacterInventory = (MyInventory) null;
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      MyHud.Crosshair.Recenter();
      this.DrawHud();
      this.UpdateHudComponentMark();
    }

    protected virtual void DrawHud()
    {
      MySlimBlock block = this.m_raycastComponent.HitBlock;
      if (block == null)
      {
        if (this.LastTargetObject == this)
          return;
        MyHud.BlockInfo.MissingComponentIndex = -1;
        MyHud.BlockInfo.DefinitionId = this.PhysicalItemDefinition.Id;
        MyHud.BlockInfo.BlockName = this.PhysicalItemDefinition.DisplayNameText;
        MyHud.BlockInfo.PCUCost = 0;
        MyHud.BlockInfo.BlockIcons = this.PhysicalItemDefinition.Icons;
        MyHud.BlockInfo.BlockIntegrity = 1f;
        MyHud.BlockInfo.CriticalIntegrity = 0.0f;
        MyHud.BlockInfo.CriticalComponentIndex = 0;
        MyHud.BlockInfo.OwnershipIntegrity = 0.0f;
        MyHud.BlockInfo.BlockBuiltBy = 0L;
        MyHud.BlockInfo.GridSize = MyCubeSize.Small;
        MyHud.BlockInfo.Components.Clear();
        MyHud.BlockInfo.SetContextHelp((MyDefinitionBase) this.PhysicalItemDefinition);
        this.LastTargetObject = (object) this;
      }
      else
      {
        if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
        {
          MyCompoundCubeBlock fatBlock = block.FatBlock as MyCompoundCubeBlock;
          if (fatBlock.GetBlocksCount() > 0)
            block = fatBlock.GetBlocks().First<MySlimBlock>();
        }
        MyHud.BlockInfo.BlockIntegrity = block.Integrity / block.MaxIntegrity;
        int num = block.GetStockpileStamp() + block.ComponentStack.LastChangeStamp;
        if (this.LastTargetObject == block && num == this.LastTargetStamp)
          return;
        this.LastTargetStamp = num;
        MyHud.BlockInfo.MissingComponentIndex = -1;
        MySlimBlock.SetBlockComponents(MyHud.BlockInfo, block);
        if (this.LastTargetObject == block)
          return;
        MyHud.BlockInfo.DefinitionId = block.BlockDefinition.Id;
        MyHud.BlockInfo.BlockName = block.BlockDefinition.DisplayNameText;
        MyHud.BlockInfo.PCUCost = block.BlockDefinition.PCU;
        MyHud.BlockInfo.BlockIcons = block.BlockDefinition.Icons;
        MyHud.BlockInfo.CriticalIntegrity = block.BlockDefinition.CriticalIntegrityRatio;
        MyHud.BlockInfo.CriticalComponentIndex = (int) block.BlockDefinition.CriticalGroup;
        MyHud.BlockInfo.OwnershipIntegrity = block.BlockDefinition.OwnershipIntegrityRatio;
        MyHud.BlockInfo.BlockBuiltBy = block.BuiltBy;
        MyHud.BlockInfo.GridSize = block.CubeGrid.GridSizeEnum;
        MyHud.BlockInfo.SetContextHelp((MyDefinitionBase) block.BlockDefinition);
        this.LastTargetObject = (object) block;
      }
    }

    protected void UnmarkMissingComponent()
    {
      this.m_lastMarkTime = -1;
      this.m_markedComponent = -1;
    }

    protected void MarkMissingComponent(int componentIdx)
    {
      this.m_lastMarkTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_markedComponent = componentIdx;
    }

    private void UpdateHudComponentMark()
    {
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastMarkTime > 2500)
        this.UnmarkMissingComponent();
      else
        MyHud.BlockInfo.MissingComponentIndex = this.m_markedComponent;
    }

    public MyDefinitionId DefinitionId => this.m_handItemDef.Id;

    int IMyGunObject<MyToolBase>.ShootDirectionUpdateTime => 200;

    public bool NeedsShootDirectionWhileAiming => true;

    public float MaximumShotLength => this.m_raycastComponent.GetCastLength();

    public Vector3 DirectionToTarget(Vector3D target)
    {
      MyCharacterWeaponPositionComponent positionComponent = this.Owner.Components.Get<MyCharacterWeaponPositionComponent>();
      return (Vector3) (positionComponent == null ? Vector3D.Normalize(target - this.PositionComp.WorldMatrixRef.Translation) : Vector3D.Normalize(target - positionComponent.LogicalPositionWorld));
    }

    public virtual void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (this.Owner != MySession.Static.LocalCharacter)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
    }

    public virtual void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public virtual void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (action != MyShootActionEnum.PrimaryAction || status != MyGunStatusEnum.Failed)
        return;
      this.EffectAction = new MyShootActionEnum?(MyShootActionEnum.SecondaryAction);
    }

    public int GetTotalAmmunitionAmount() => 0;

    public int GetAmmunitionAmount() => 0;

    public int GetMagazineAmount() => 0;

    public MyPhysicalItemDefinition PhysicalItemDefinition => this.m_physItemDef;

    public int CurrentAmmunition { set; get; }

    public int CurrentMagazineAmmunition { set; get; }

    public int CurrentMagazineAmount { set; get; }

    public bool Reloadable => false;

    public bool IsReloading => false;

    public bool IsRecoiling => false;

    public bool NeedsReload => false;

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_EntityBase objectBuilder = base.GetObjectBuilder(copy);
      objectBuilder.SubtypeName = this.m_handItemDef.Id.SubtypeName;
      return objectBuilder;
    }

    public virtual bool SupressShootAnimation() => false;

    public void DoubleClicked(MyShootActionEnum action) => this.m_isActionDoubleClicked[action] = true;

    protected bool IsFriendlyFireReduced(MyCharacter target)
    {
      if (MySession.Static.Settings.EnableFriendlyFire || target == null || this.OwnerId == 0L)
        return false;
      MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(Sandbox.Game.Entities.MyEntities.GetEntityById(this.OwnerId));
      ulong steamId = controllingPlayer != null ? controllingPlayer.Id.SteamId : 0UL;
      if (steamId != 0UL)
      {
        long playerIdentityId = target.GetPlayerIdentityId();
        switch (MyIDModule.GetRelationPlayerPlayer(MySession.Static.Players.TryGetIdentityId(steamId, 0), playerIdentityId))
        {
          case MyRelationsBetweenPlayers.Self:
          case MyRelationsBetweenPlayers.Allies:
            return true;
        }
      }
      return false;
    }

    public bool CanReload() => false;

    public bool Reload() => false;

    public float GetReloadDuration() => 0.0f;

    public Vector3D GetMuzzlePosition() => this.m_gunBase.GetMuzzleWorldPosition();

    public void PlayReloadSound()
    {
    }

    public bool GetShakeOnAction(MyShootActionEnum action) => true;

    protected enum EffectType
    {
      Light,
      Effect,
      EffectSecondary,
    }
  }
}
