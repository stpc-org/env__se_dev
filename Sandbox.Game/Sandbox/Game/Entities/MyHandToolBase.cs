// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyHandToolBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [StaticEventOwner]
  [MyEntityType(typeof (MyObjectBuilder_HandToolBase), true)]
  public class MyHandToolBase : MyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>, IStoppableAttackingTool
  {
    private static MyStringId m_startCue = MyStringId.GetOrCompute("Start");
    private static MyStringId m_hitCue = MyStringId.GetOrCompute("Hit");
    private const float AFTER_SHOOT_HIT_DELAY = 0.4f;
    private MyDefinitionId m_handItemDefinitionId;
    private Sandbox.Definitions.MyToolActionDefinition? m_primaryToolAction;
    private MyToolHitCondition m_primaryHitCondition;
    private Sandbox.Definitions.MyToolActionDefinition? m_secondaryToolAction;
    private MyToolHitCondition m_secondaryHitCondition;
    private Sandbox.Definitions.MyToolActionDefinition? m_shotToolAction;
    private MyToolHitCondition m_shotHitCondition;
    protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();
    private bool m_wasShooting;
    private bool m_swingSoundPlayed;
    private bool m_isHit;
    protected Dictionary<string, IMyHandToolComponent> m_toolComponents = new Dictionary<string, IMyHandToolComponent>();
    private MyCharacter m_owner;
    protected int m_lastShot;
    private int m_lastHit;
    private int m_hitDelay;
    private MyPhysicalItemDefinition m_physItemDef;
    protected MyToolItemDefinition m_toolItemDef;
    private MyEntity3DSoundEmitter m_soundEmitter;
    private Dictionary<string, MySoundPair> m_toolSounds = new Dictionary<string, MySoundPair>();
    private static MyStringId BlockId = MyStringId.Get("Block");
    private MyHudNotification m_notEnoughStatNotification;

    public MyObjectBuilder_PhysicalGunObject PhysicalObject { get; private set; }

    public MyPhysicsBody Physics
    {
      get => base.Physics as MyPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public bool IsShooting
    {
      get
      {
        if (!this.m_shotToolAction.HasValue || this.m_lastShot > MySandboxGame.TotalGamePlayTimeInMilliseconds)
          return false;
        return (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastShot) < (double) this.m_shotToolAction.Value.HitDuration * 1000.0 || (double) this.m_shotToolAction.Value.HitDuration == 0.0;
      }
    }

    public int ShootDirectionUpdateTime => 0;

    public bool NeedsShootDirectionWhileAiming => false;

    public float MaximumShotLength => 0.0f;

    public bool EnabledInWorldRules => true;

    public float BackkickForcePerSecond => 0.0f;

    public float ShakeAmount
    {
      get => 2.5f;
      protected set
      {
      }
    }

    public MyDefinitionId DefinitionId => this.m_handItemDefinitionId;

    public MyToolBase GunBase { get; private set; }

    public virtual bool ForceAnimationInsteadOfIK => true;

    public bool IsBlocking => this.m_shotToolAction.HasValue && this.m_shotToolAction.Value.Name == MyStringId.GetOrCompute("Block");

    public MyPhysicalItemDefinition PhysicalItemDefinition => this.m_physItemDef;

    public MyCharacter Owner => this.m_owner;

    public long OwnerId => this.m_owner != null ? this.m_owner.EntityId : 0L;

    public long OwnerIdentityId => this.m_owner != null ? this.m_owner.GetPlayerIdentityId() : 0L;

    public bool IsSkinnable => false;

    public MyHandToolBase()
    {
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
      this.GunBase = new MyToolBase();
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      this.m_handItemDefinitionId = objectBuilder.GetId();
      this.m_physItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(this.m_handItemDefinitionId);
      base.Init(objectBuilder);
      this.Init((StringBuilder) null, this.PhysicalItemDefinition.Model, (MyEntity) null, new float?());
      this.Save = false;
      this.PhysicalObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(this.m_handItemDefinitionId.SubtypeName);
      this.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase) objectBuilder.Clone();
      this.PhysicalObject.GunEntity.EntityId = this.EntityId;
      this.m_toolItemDef = this.PhysicalItemDefinition as MyToolItemDefinition;
      this.m_notEnoughStatNotification = new MyHudNotification(MyCommonTexts.NotificationStatNotEnough, 1000, "Red", level: MyNotificationLevel.Important);
      this.InitToolComponents();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;
      MyObjectBuilder_HandToolBase builderHandToolBase = objectBuilder as MyObjectBuilder_HandToolBase;
      if (builderHandToolBase.DeviceBase == null)
        return;
      this.GunBase.Init(builderHandToolBase.DeviceBase);
    }

    protected virtual void InitToolComponents()
    {
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_HandToolBase objectBuilder = base.GetObjectBuilder(copy) as MyObjectBuilder_HandToolBase;
      objectBuilder.SubtypeName = this.m_handItemDefinitionId.SubtypeName;
      objectBuilder.DeviceBase = this.GunBase.GetObjectBuilder();
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    private void InitBlockingPhysics(MyEntity owner)
    {
      this.CloseBlockingPhysics();
      this.Physics = (MyPhysicsBody) new MyHandToolBase.MyBlockingBody(this, owner);
      HkShape shape = (HkShape) new HkBoxShape(0.5f * new Vector3(0.5f, 0.7f, 0.25f));
      this.Physics.CreateFromCollisionObject(shape, new Vector3(0.0f, 0.9f, -0.5f), this.WorldMatrix, collisionFilter: 19);
      this.Physics.MaterialType = this.m_physItemDef.PhysicalMaterial;
      shape.RemoveReference();
      this.Physics.Enabled = false;
      this.m_owner.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
    }

    private void CloseBlockingPhysics()
    {
      if (this.Physics == null)
        return;
      this.Physics.Close();
      this.Physics = (MyPhysicsBody) null;
    }

    public virtual bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastHit < this.m_hitDelay)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      status = MyGunStatusEnum.OK;
      if (this.IsShooting)
        status = MyGunStatusEnum.Cooldown;
      if (this.m_owner == null)
        status = MyGunStatusEnum.Failed;
      return status == MyGunStatusEnum.OK;
    }

    public bool ShouldEndShootOnPause(MyShootActionEnum action) => true;

    public bool CanDoubleClickToStick(MyShootActionEnum action) => false;

    public virtual void Shoot(
      MyShootActionEnum shootAction,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      this.m_shotToolAction = new Sandbox.Definitions.MyToolActionDefinition?();
      this.m_wasShooting = false;
      this.m_swingSoundPlayed = false;
      this.m_isHit = false;
      if (!string.IsNullOrEmpty(gunAction))
      {
        if (shootAction != MyShootActionEnum.PrimaryAction)
        {
          if (shootAction == MyShootActionEnum.SecondaryAction)
            this.GetPreferredToolAction(this.m_toolItemDef.SecondaryActions, gunAction, out this.m_secondaryToolAction, out this.m_secondaryHitCondition);
        }
        else
          this.GetPreferredToolAction(this.m_toolItemDef.PrimaryActions, gunAction, out this.m_primaryToolAction, out this.m_primaryHitCondition);
      }
      if (shootAction != MyShootActionEnum.PrimaryAction)
      {
        if (shootAction == MyShootActionEnum.SecondaryAction)
        {
          this.m_shotToolAction = this.m_secondaryToolAction;
          this.m_shotHitCondition = this.m_secondaryHitCondition;
        }
      }
      else
      {
        this.m_shotToolAction = this.m_primaryToolAction;
        this.m_shotHitCondition = this.m_primaryHitCondition;
      }
      MyTuple<ushort, MyStringHash> message;
      if (!string.IsNullOrEmpty(this.m_shotHitCondition.StatsAction) && this.m_owner.StatComp != null && !this.m_owner.StatComp.CanDoAction(this.m_shotHitCondition.StatsAction, out message))
      {
        if (MySession.Static == null || MySession.Static.LocalCharacter != this.m_owner || (message.Item1 != (ushort) 4 || message.Item2.String.CompareTo("Stamina") != 0))
          return;
        this.m_notEnoughStatNotification.SetTextFormatArguments((object) message.Item2);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_notEnoughStatNotification);
      }
      else
      {
        if (!this.m_shotToolAction.HasValue)
          return;
        IMyHandToolComponent handToolComponent;
        if (this.m_toolComponents.TryGetValue(this.m_shotHitCondition.Component, out handToolComponent))
          handToolComponent.Shoot();
        MyFrameOption frameOption = MyFrameOption.StayOnLastFrame;
        if ((double) this.m_shotToolAction.Value.HitDuration == 0.0)
          frameOption = MyFrameOption.JustFirstFrame;
        this.m_owner.StopUpperCharacterAnimation(0.1f);
        this.m_owner.PlayCharacterAnimation(this.m_shotHitCondition.Animation, MyBlendOption.Immediate, frameOption, 0.2f, this.m_shotHitCondition.AnimationTimeScale, excludeLegsWhenMoving: true);
        this.m_owner.TriggerCharacterAnimationEvent(this.m_shotHitCondition.Animation.ToLower(), false);
        if (this.m_owner.StatComp != null)
        {
          if (!string.IsNullOrEmpty(this.m_shotHitCondition.StatsAction))
            this.m_owner.StatComp.DoAction(this.m_shotHitCondition.StatsAction);
          if (!string.IsNullOrEmpty(this.m_shotHitCondition.StatsModifier))
            this.m_owner.StatComp.ApplyModifier(this.m_shotHitCondition.StatsModifier);
        }
        this.Physics.Enabled = this.m_shotToolAction.Value.Name == MyHandToolBase.BlockId;
        this.m_lastShot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
    }

    private void PlaySound(string soundName)
    {
      MyPhysicalMaterialDefinition definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyPhysicalMaterialDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalMaterialDefinition), this.m_physItemDef.PhysicalMaterial), out definition))
        return;
      MySoundPair soundId1;
      if (definition.GeneralSounds.TryGetValue(MyStringId.GetOrCompute(soundName), out soundId1) && !soundId1.SoundId.IsNull)
      {
        this.m_soundEmitter.PlaySound(soundId1);
      }
      else
      {
        MySoundPair soundId2;
        if (!this.m_toolSounds.TryGetValue(soundName, out soundId2))
        {
          soundId2 = new MySoundPair(soundName);
          this.m_toolSounds.Add(soundName, soundId2);
        }
        this.m_soundEmitter.PlaySound(soundId2);
      }
    }

    public virtual void OnControlAcquired(MyCharacter owner)
    {
      this.m_owner = owner;
      this.InitBlockingPhysics((MyEntity) this.m_owner);
      foreach (IMyHandToolComponent handToolComponent in this.m_toolComponents.Values)
        handToolComponent.OnControlAcquired(owner);
      this.RaiseEntityEvent(MyStringHash.GetOrCompute("ControlAcquired"), (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.ControlAcquiredParams((MyEntity) owner));
    }

    private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
    {
    }

    public virtual void OnControlReleased()
    {
      this.RaiseEntityEvent(MyStringHash.GetOrCompute("ControlReleased"), (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.ControlReleasedParams((MyEntity) this.m_owner));
      if (this.m_owner != null)
        this.m_owner.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      this.m_owner = (MyCharacter) null;
      this.CloseBlockingPhysics();
      foreach (IMyHandToolComponent handToolComponent in this.m_toolComponents.Values)
        handToolComponent.OnControlReleased();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      bool isShooting = this.IsShooting;
      if (!this.m_isHit && this.IsShooting && (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastShot) > (double) this.m_shotToolAction.Value.HitStart * 1000.0)
      {
        IMyHandToolComponent toolComponent;
        if (this.m_toolComponents.TryGetValue(this.m_shotHitCondition.Component, out toolComponent))
        {
          MyCharacterDetectorComponent detectorComponent1 = this.m_owner.Components.Get<MyCharacterDetectorComponent>();
          if (detectorComponent1 != null)
          {
            if ((double) this.m_shotToolAction.Value.CustomShapeRadius > 0.0 && detectorComponent1 is MyCharacterShapecastDetectorComponent)
            {
              MyCharacterShapecastDetectorComponent detectorComponent2 = detectorComponent1 as MyCharacterShapecastDetectorComponent;
              detectorComponent2.ShapeRadius = this.m_shotToolAction.Value.CustomShapeRadius;
              detectorComponent2.DoDetectionModel();
              detectorComponent2.ShapeRadius = 0.1f;
            }
            if (detectorComponent1.DetectedEntity != null)
            {
              MyHitInfo hitInfo = new MyHitInfo();
              hitInfo.Position = detectorComponent1.HitPosition;
              hitInfo.Normal = detectorComponent1.HitNormal;
              hitInfo.ShapeKey = detectorComponent1.ShapeKey;
              bool isBlock = false;
              float hitEfficiency = 1f;
              int num = this.CanHit(toolComponent, detectorComponent1, ref isBlock, out hitEfficiency) ? 1 : 0;
              if (num != 0)
              {
                if (!string.IsNullOrEmpty(this.m_shotToolAction.Value.StatsEfficiency) && this.Owner.StatComp != null)
                  hitEfficiency *= this.Owner.StatComp.GetEfficiencyModifier(this.m_shotToolAction.Value.StatsEfficiency);
                float efficiency = this.m_shotToolAction.Value.Efficiency * hitEfficiency;
                MyHandToolBase detectedEntity = detectorComponent1.DetectedEntity as MyHandToolBase;
                if ((!isBlock || detectedEntity == null ? toolComponent.Hit((MyEntity) detectorComponent1.DetectedEntity, hitInfo, detectorComponent1.ShapeKey, efficiency) : toolComponent.Hit((MyEntity) detectedEntity.Owner, hitInfo, detectorComponent1.ShapeKey, efficiency)) && Sync.IsServer && this.Owner.StatComp != null)
                {
                  if (!string.IsNullOrEmpty(this.m_shotHitCondition.StatsActionIfHit))
                    this.Owner.StatComp.DoAction(this.m_shotHitCondition.StatsActionIfHit);
                  if (!string.IsNullOrEmpty(this.m_shotHitCondition.StatsModifierIfHit))
                    this.Owner.StatComp.ApplyModifier(this.m_shotHitCondition.StatsModifierIfHit);
                }
              }
              if ((num | (isBlock ? 1 : 0)) != 0)
              {
                if (!string.IsNullOrEmpty(this.m_shotToolAction.Value.HitSound))
                {
                  this.PlaySound(this.m_shotToolAction.Value.HitSound);
                }
                else
                {
                  MyStringId type = MyMaterialPropertiesHelper.CollisionType.Hit;
                  bool flag = false;
                  if (MyAudioComponent.PlayContactSound(this.EntityId, MyHandToolBase.m_hitCue, detectorComponent1.HitPosition, this.m_toolItemDef.PhysicalMaterial, detectorComponent1.HitMaterial))
                    flag = true;
                  else if (MyAudioComponent.PlayContactSound(this.EntityId, MyHandToolBase.m_startCue, detectorComponent1.HitPosition, this.m_toolItemDef.PhysicalMaterial, detectorComponent1.HitMaterial))
                  {
                    flag = true;
                    type = MyMaterialPropertiesHelper.CollisionType.Start;
                  }
                  if (flag)
                    MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(type, detectorComponent1.HitPosition, detectorComponent1.HitNormal, this.m_toolItemDef.PhysicalMaterial, detectorComponent1.HitMaterial, (VRage.Game.ModAPI.Ingame.IMyEntity) null);
                }
                this.RaiseEntityEvent(MyStringHash.GetOrCompute("Hit"), (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.HitParams(MyStringHash.GetOrCompute(this.m_shotHitCondition.Component), detectorComponent1.HitMaterial));
                this.m_soundEmitter.StopSound(true);
              }
            }
          }
        }
        this.m_isHit = true;
      }
      if (!this.m_swingSoundPlayed && this.IsShooting && (!this.m_isHit && (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastShot) > (double) this.m_shotToolAction.Value.SwingSoundStart * 1000.0))
      {
        if (!string.IsNullOrEmpty(this.m_shotToolAction.Value.SwingSound))
          this.PlaySound(this.m_shotToolAction.Value.SwingSound);
        this.m_swingSoundPlayed = true;
      }
      if (!isShooting && this.m_wasShooting)
      {
        this.m_owner.TriggerCharacterAnimationEvent("stop_tool_action", false);
        this.m_owner.StopUpperCharacterAnimation(0.4f);
        this.m_shotToolAction = new Sandbox.Definitions.MyToolActionDefinition?();
      }
      this.m_wasShooting = isShooting;
      if (this.m_owner != null)
      {
        Vector3D position = ((MyEntity) this.m_owner.CurrentWeapon).PositionComp.GetPosition();
        MatrixD worldMatrix = this.m_owner.WorldMatrix;
        Vector3D forward = worldMatrix.Forward;
        worldMatrix = this.m_owner.WorldMatrix;
        Vector3D up = worldMatrix.Up;
        ((MyHandToolBase.MyBlockingBody) this.Physics).SetWorldMatrix(MatrixD.CreateWorld(position, forward, up));
      }
      foreach (IMyHandToolComponent handToolComponent in this.m_toolComponents.Values)
        handToolComponent.Update();
    }

    protected bool CanHit(
      IMyHandToolComponent toolComponent,
      MyCharacterDetectorComponent detectorComponent,
      ref bool isBlock,
      out float hitEfficiency)
    {
      bool flag1 = true;
      hitEfficiency = 1f;
      MyTuple<ushort, MyStringHash> message;
      if ((HkReferenceObject) detectorComponent.HitBody != (HkReferenceObject) null && detectorComponent.HitBody.UserObject is MyHandToolBase.MyBlockingBody)
      {
        MyHandToolBase.MyBlockingBody userObject = detectorComponent.HitBody.UserObject as MyHandToolBase.MyBlockingBody;
        if (userObject.HandTool.IsBlocking && userObject.HandTool.m_owner.StatComp != null && userObject.HandTool.m_owner.StatComp.CanDoAction(userObject.HandTool.m_shotHitCondition.StatsActionIfHit, out message))
        {
          userObject.HandTool.m_owner.StatComp.DoAction(userObject.HandTool.m_shotHitCondition.StatsActionIfHit);
          if (!string.IsNullOrEmpty(userObject.HandTool.m_shotHitCondition.StatsModifierIfHit))
            userObject.HandTool.m_owner.StatComp.ApplyModifier(userObject.HandTool.m_shotHitCondition.StatsModifierIfHit);
          isBlock = true;
          if (!string.IsNullOrEmpty(userObject.HandTool.m_shotToolAction.Value.StatsEfficiency))
            hitEfficiency = 1f - userObject.HandTool.m_owner.StatComp.GetEfficiencyModifier(userObject.HandTool.m_shotToolAction.Value.StatsEfficiency);
          flag1 = (double) hitEfficiency > 0.0;
          MyEntityContainerEventExtensions.RaiseEntityEventOn((MyEntity) userObject.HandTool, MyStringHash.GetOrCompute("Hit"), (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.HitParams(MyStringHash.GetOrCompute("Block"), this.PhysicalItemDefinition.Id.SubtypeId));
        }
      }
      if (!flag1)
      {
        hitEfficiency = 0.0f;
        return flag1;
      }
      if (!string.IsNullOrEmpty(this.m_shotHitCondition.StatsActionIfHit))
      {
        bool flag2 = this.m_owner.StatComp != null && this.m_owner.StatComp.CanDoAction(this.m_shotHitCondition.StatsActionIfHit, out message);
        if (!flag2)
        {
          hitEfficiency = 0.0f;
          return flag2;
        }
      }
      bool flag3 = (double) Vector3.Distance((Vector3) detectorComponent.HitPosition, (Vector3) detectorComponent.StartPosition) <= (double) this.m_toolItemDef.HitDistance;
      if (!flag3)
      {
        hitEfficiency = 0.0f;
        return flag3;
      }
      MyEntity entity = this.m_owner.Entity;
      if (MySession.Static.Factions.TryGetPlayerFaction(this.m_owner.GetPlayerIdentityId()) is MyFaction playerFaction && !playerFaction.EnableFriendlyFire && detectorComponent.DetectedEntity is MyCharacter detectedEntity)
      {
        flag3 = !playerFaction.IsMember(detectedEntity.GetPlayerIdentityId());
        hitEfficiency = flag3 ? hitEfficiency : 0.0f;
      }
      return flag3;
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.GetMostEffectiveToolAction(this.m_toolItemDef.PrimaryActions, out this.m_primaryToolAction, out this.m_primaryHitCondition);
      this.GetMostEffectiveToolAction(this.m_toolItemDef.SecondaryActions, out this.m_secondaryToolAction, out this.m_secondaryHitCondition);
      if (MySession.Static.ControlledEntity != this.m_owner)
        return;
      MyCharacterDetectorComponent detectorComponent = this.m_owner.Components.Get<MyCharacterDetectorComponent>();
      bool flag = false;
      float num = float.MaxValue;
      if (detectorComponent != null)
      {
        flag = detectorComponent.DetectedEntity != null;
        num = Vector3.Distance((Vector3) detectorComponent.HitPosition, (Vector3) this.PositionComp.GetPosition());
      }
      if ((double) num > (double) this.m_toolItemDef.HitDistance)
        flag = false;
      if (this.m_primaryToolAction.HasValue && this.m_primaryHitCondition.EntityType != null | flag)
        MyHud.Crosshair.ChangeDefaultSprite(this.m_primaryToolAction.Value.Crosshair);
      else if (this.m_secondaryToolAction.HasValue && this.m_secondaryHitCondition.EntityType != null | flag)
        MyHud.Crosshair.ChangeDefaultSprite(this.m_secondaryToolAction.Value.Crosshair);
      else
        MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.crosshair);
    }

    private void GetMostEffectiveToolAction(
      List<Sandbox.Definitions.MyToolActionDefinition> toolActions,
      out Sandbox.Definitions.MyToolActionDefinition? bestAction,
      out MyToolHitCondition bestCondition)
    {
      MyCharacterDetectorComponent detectorComponent = this.m_owner.Components.Get<MyCharacterDetectorComponent>();
      VRage.ModAPI.IMyEntity myEntity = (VRage.ModAPI.IMyEntity) null;
      uint shapeKey = 0;
      if (detectorComponent != null)
      {
        myEntity = detectorComponent.DetectedEntity;
        shapeKey = detectorComponent.ShapeKey;
        if ((double) Vector3.Distance((Vector3) detectorComponent.HitPosition, (Vector3) detectorComponent.StartPosition) > (double) this.m_toolItemDef.HitDistance)
          myEntity = (VRage.ModAPI.IMyEntity) null;
      }
      bestAction = new Sandbox.Definitions.MyToolActionDefinition?();
      bestCondition = new MyToolHitCondition();
      foreach (Sandbox.Definitions.MyToolActionDefinition toolAction in toolActions)
      {
        if (toolAction.HitConditions != null)
        {
          foreach (MyToolHitCondition hitCondition in toolAction.HitConditions)
          {
            if (hitCondition.EntityType != null)
            {
              if (myEntity != null)
              {
                string stateForTarget = this.GetStateForTarget((MyEntity) myEntity, shapeKey, hitCondition.Component);
                if (hitCondition.EntityType.Contains<string>(stateForTarget))
                {
                  bestAction = new Sandbox.Definitions.MyToolActionDefinition?(toolAction);
                  bestCondition = hitCondition;
                  return;
                }
              }
            }
            else
            {
              bestAction = new Sandbox.Definitions.MyToolActionDefinition?(toolAction);
              bestCondition = hitCondition;
              return;
            }
          }
        }
      }
    }

    private void GetPreferredToolAction(
      List<Sandbox.Definitions.MyToolActionDefinition> toolActions,
      string name,
      out Sandbox.Definitions.MyToolActionDefinition? bestAction,
      out MyToolHitCondition bestCondition)
    {
      bestAction = new Sandbox.Definitions.MyToolActionDefinition?();
      bestCondition = new MyToolHitCondition();
      MyStringId orCompute = MyStringId.GetOrCompute(name);
      foreach (Sandbox.Definitions.MyToolActionDefinition toolAction in toolActions)
      {
        if (toolAction.HitConditions.Length != 0 && toolAction.Name == orCompute)
        {
          bestAction = new Sandbox.Definitions.MyToolActionDefinition?(toolAction);
          bestCondition = toolAction.HitConditions[0];
          break;
        }
      }
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      if (!this.m_primaryToolAction.HasValue || !this.m_toolComponents.ContainsKey(this.m_primaryHitCondition.Component))
        return;
      this.m_toolComponents[this.m_primaryHitCondition.Component].DrawHud();
    }

    private string GetStateForTarget(MyEntity targetEntity, uint shapeKey, string actionType)
    {
      if (targetEntity == null)
        return (string) null;
      IMyHandToolComponent handToolComponent;
      if (this.m_toolComponents.TryGetValue(actionType, out handToolComponent))
      {
        string stateForTarget = handToolComponent.GetStateForTarget(targetEntity, shapeKey);
        if (!string.IsNullOrEmpty(stateForTarget))
          return stateForTarget;
      }
      foreach (KeyValuePair<string, IMyHandToolComponent> toolComponent in this.m_toolComponents)
      {
        string stateForTarget = toolComponent.Value.GetStateForTarget(targetEntity, shapeKey);
        if (!string.IsNullOrEmpty(stateForTarget))
          return stateForTarget;
      }
      return (string) null;
    }

    public Vector3 DirectionToTarget(Vector3D target) => (Vector3) target;

    public virtual void BeginShoot(MyShootActionEnum action)
    {
    }

    public virtual void EndShoot(MyShootActionEnum action)
    {
      if (this.m_shotToolAction.HasValue && (double) this.m_shotToolAction.Value.HitDuration == 0.0)
        this.m_shotToolAction = new Sandbox.Definitions.MyToolActionDefinition?();
      this.m_isActionDoubleClicked[action] = false;
    }

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public int GetTotalAmmunitionAmount() => 0;

    public int GetAmmunitionAmount() => 0;

    public int GetMagazineAmount() => 0;

    public void StopShooting(MyEntity attacker)
    {
      if (!this.IsShooting)
        return;
      float num = 0.0f;
      if (attacker is MyCharacter myCharacter && myCharacter.CurrentWeapon is MyHandToolBase currentWeapon && currentWeapon.m_shotToolAction.HasValue)
        num = currentWeapon.m_shotToolAction.Value.HitDuration - ((float) MySandboxGame.TotalGamePlayTimeInMilliseconds - (float) currentWeapon.m_lastShot / 1000f);
      float hitDelaySec = (double) num > 0.0 ? num : 0.4f;
      MyMultiplayer.RaiseStaticEvent<long, float>((Func<IMyEventOwner, Action<long, float>>) (s => new Action<long, float>(MyHandToolBase.StopShootingRequest)), this.EntityId, hitDelaySec);
      this.StopShooting(hitDelaySec);
    }

    internal void StopShooting(float hitDelaySec)
    {
      if (!this.IsShooting)
        return;
      this.m_lastHit = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_hitDelay = (int) ((double) hitDelaySec * 1000.0);
      this.m_owner.PlayCharacterAnimation(this.m_shotHitCondition.Animation, MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0.2f, this.m_shotHitCondition.AnimationTimeScale, excludeLegsWhenMoving: true);
      this.m_shotToolAction = new Sandbox.Definitions.MyToolActionDefinition?();
      this.m_wasShooting = false;
    }

    [Event(null, 879)]
    [Reliable]
    [Broadcast]
    private static void StopShootingRequest(long entityId, float attackDelay)
    {
      MyEntity entity = (MyEntity) null;
      MyEntities.TryGetEntityById(entityId, out entity);
      if (!(entity is MyHandToolBase myHandToolBase))
        return;
      myHandToolBase.StopShooting(attackDelay);
    }

    public int CurrentAmmunition { set; get; }

    public int CurrentMagazineAmmunition { set; get; }

    public int CurrentMagazineAmount { set; get; }

    public bool Reloadable => false;

    public bool IsReloading => false;

    public bool IsRecoiling => false;

    public bool NeedsReload => false;

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public bool SupressShootAnimation() => false;

    public void DoubleClicked(MyShootActionEnum action) => this.m_isActionDoubleClicked[action] = true;

    public bool CanReload() => false;

    public bool Reload() => false;

    public float GetReloadDuration() => 0.0f;

    public Vector3D GetMuzzlePosition() => this.PositionComp.GetPosition();

    public void PlayReloadSound()
    {
    }

    public bool GetShakeOnAction(MyShootActionEnum action) => true;

    public class MyBlockingBody : MyPhysicsBody
    {
      public MyHandToolBase HandTool { get; private set; }

      public MyBlockingBody(MyHandToolBase tool, MyEntity owner)
        : base((VRage.ModAPI.IMyEntity) owner, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_NO_POSITION_UPDATES)
        => this.HandTool = tool;

      public override void OnWorldPositionChanged(object source)
      {
      }

      public void SetWorldMatrix(MatrixD worldMatrix)
      {
        Vector3D objectOffset = MyPhysics.GetObjectOffset(this.ClusterObjectID);
        Matrix world = Matrix.CreateWorld((Vector3) (worldMatrix.Translation - objectOffset), (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up);
        if (!((HkReferenceObject) this.RigidBody != (HkReferenceObject) null))
          return;
        this.RigidBody.SetWorldMatrix(world);
      }

      private class Sandbox_Game_Entities_MyHandToolBase\u003C\u003EMyBlockingBody\u003C\u003EActor
      {
      }
    }

    protected sealed class StopShootingRequest\u003C\u003ESystem_Int64\u0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in float attackDelay,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyHandToolBase.StopShootingRequest(entityId, attackDelay);
      }
    }

    private class Sandbox_Game_Entities_MyHandToolBase\u003C\u003EActor : IActivator, IActivator<MyHandToolBase>
    {
      object IActivator.CreateInstance() => (object) new MyHandToolBase();

      MyHandToolBase IActivator<MyHandToolBase>.CreateInstance() => new MyHandToolBase();
    }
  }
}
