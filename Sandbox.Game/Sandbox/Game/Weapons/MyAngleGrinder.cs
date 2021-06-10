// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyAngleGrinder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.ModAPI.Weapons;
using System;
using System.Text;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MyEntityType(typeof (MyObjectBuilder_AngleGrinder), true)]
  [StaticEventOwner]
  public class MyAngleGrinder : MyEngineerToolBase, IMyAngleGrinder, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEngineerToolBase, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
  {
    private MySoundPair m_idleSound = new MySoundPair("ToolPlayGrindIdle");
    private MySoundPair m_actualSound = new MySoundPair("ToolPlayGrindMetal");
    private MyStringHash m_source = MyStringHash.GetOrCompute("Grinder");
    private MyStringHash m_metal = MyStringHash.GetOrCompute("Metal");
    private static readonly float GRINDER_AMOUNT_PER_SECOND = 2f;
    private static readonly float GRINDER_MAX_SPEED_RPM = 500f;
    private static readonly float GRINDER_ACCELERATION_RPMPS = 700f;
    private static readonly float GRINDER_DECELERATION_RPMPS = 500f;
    public static float GRINDER_MAX_SHAKE = 1.5f;
    public static readonly float BASE_GRINDER_DAMAGE = 20f;
    public static readonly float BASE_GRINDER_CHARACTER_DAMAGE = 4f;
    private MyHudNotification m_safezoneNotification;
    private static int m_lastTimePlayedSound;
    private int m_lastUpdateTime;
    private float m_rotationSpeed;
    private int m_lastContactTime;
    private int m_lastItemId;
    private static MyDefinitionId m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "AngleGrinderItem");
    private double m_grinderCameraMeanShakeIntensity = 1.0;

    public override bool IsSkinnable => true;

    public MyAngleGrinder()
      : base(250)
    {
      this.SecondaryLightIntensityLower = 0.4f;
      this.SecondaryLightIntensityUpper = 0.4f;
      this.EffectScale = 0.6f;
      this.HasCubeHighlight = true;
      this.HighlightColor = Color.Red * 0.3f;
      this.HighlightMaterial = MyStringId.GetOrCompute("GizmoDrawLineRed");
      this.m_rotationSpeed = 0.0f;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      MyAngleGrinder.m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "AngleGrinderItem");
      if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
        MyAngleGrinder.m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), objectBuilder.SubtypeName + "Item");
      this.PhysicalObject = (MyObjectBuilder_PhysicalGunObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) MyAngleGrinder.m_physicalItemId);
      this.Init(objectBuilder, MyAngleGrinder.m_physicalItemId);
      this.m_effectId = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, this.m_handItemDef.ToolMaterial, this.m_metal);
      this.Init((StringBuilder) null, MyDefinitionManager.Static.GetPhysicalItemDefinition(MyAngleGrinder.m_physicalItemId).Model, (MyEntity) null, new float?());
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      this.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase) objectBuilder.Clone();
      this.PhysicalObject.GunEntity.EntityId = this.EntityId;
      foreach (ToolSound toolSound in this.m_handItemDef.ToolSounds)
      {
        if (toolSound.type != null && toolSound.subtype != null && (toolSound.sound != null && toolSound.type.Equals("Main")) && toolSound.subtype.Equals("Idle"))
          this.m_idleSound = new MySoundPair(toolSound.sound);
      }
    }

    private float GrinderAmount => (float) ((double) MySession.Static.GrinderSpeedMultiplier * (double) this.m_speedMultiplier * (double) MyAngleGrinder.GRINDER_AMOUNT_PER_SECOND * (double) this.ToolCooldownMs / 1000.0);

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime;
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (!this.m_activated)
        this.m_effectId = (string) null;
      if (this.m_activated && (double) this.m_rotationSpeed < (double) MyAngleGrinder.GRINDER_MAX_SPEED_RPM)
      {
        this.m_rotationSpeed += (float) num * (1f / 1000f) * MyAngleGrinder.GRINDER_ACCELERATION_RPMPS;
        if ((double) this.m_rotationSpeed > (double) MyAngleGrinder.GRINDER_MAX_SPEED_RPM)
          this.m_rotationSpeed = MyAngleGrinder.GRINDER_MAX_SPEED_RPM;
      }
      else if (!this.m_activated && (double) this.m_rotationSpeed > 0.0)
      {
        this.m_rotationSpeed -= (float) num * (1f / 1000f) * MyAngleGrinder.GRINDER_DECELERATION_RPMPS;
        if ((double) this.m_rotationSpeed < 0.0)
          this.m_rotationSpeed = 0.0f;
      }
      if (this.m_effectId != null && this.Owner != null && this.Owner.ControllerInfo.IsLocallyControlled())
        this.PerformCameraShake();
      MyEntitySubpart subpart = this.Subparts["grinder"];
      Matrix localMatrix = Matrix.CreateRotationY((float) ((double) -num * (double) this.m_rotationSpeed * 0.000104719758382998)) * subpart.PositionComp.LocalMatrixRef;
      subpart.PositionComp.SetLocalMatrix(ref localMatrix);
    }

    public override void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status != MyGunStatusEnum.SafeZoneDenied)
        return;
      if (this.m_safezoneNotification == null)
        this.m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_GrindingDisabled, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneNotification);
    }

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      base.Shoot(action, direction, overrideWeaponPos, gunAction);
      if (action != MyShootActionEnum.PrimaryAction || !this.IsPreheated || !this.m_activated)
        return;
      this.Grind();
    }

    protected override void AddHudInfo()
    {
    }

    protected override void RemoveHudInfo()
    {
    }

    public override void BeginShoot(MyShootActionEnum action)
    {
    }

    public override void EndShoot(MyShootActionEnum action) => base.EndShoot(action);

    protected override MatrixD GetEffectMatrix(
      float muzzleOffset,
      MyEngineerToolBase.EffectType effectType)
    {
      if (effectType == MyEngineerToolBase.EffectType.Light)
        return MatrixD.CreateWorld(this.m_gunBase.GetMuzzleWorldPosition(), this.WorldMatrix.Forward, this.WorldMatrix.Up);
      if (this.m_raycastComponent.HitCubeGrid != null && this.m_raycastComponent.HitBlock != null)
      {
        MyCharacter owner = this.Owner;
      }
      return MatrixD.CreateWorld(this.m_gunBase.GetMuzzleWorldPosition(), this.WorldMatrix.Forward, this.WorldMatrix.Up);
    }

    private void Grind()
    {
      MySlimBlock targetBlock = this.GetTargetBlock();
      MyStringHash materialType2 = this.m_metal;
      this.m_effectId = (string) null;
      ulong user1 = 0;
      if (this.Owner != null && this.Owner.ControllerInfo != null && (this.Owner.ControllerInfo.Controller != null && this.Owner.ControllerInfo.Controller.Player != null))
        user1 = this.Owner.ControllerInfo.Controller.Player.Id.SteamId;
      if (targetBlock != null && !MySessionComponentSafeZones.IsActionAllowed(targetBlock.WorldAABB, MySafeZoneAction.Grinding, user: user1))
        return;
      if (targetBlock != null && targetBlock.CubeGrid.Immune)
      {
        long num = (long) user1;
        ulong? steamId = MySession.Static?.LocalHumanPlayer?.Id.SteamId;
        long valueOrDefault = (long) steamId.GetValueOrDefault();
        if (num == valueOrDefault & steamId.HasValue)
          MyHud.Notifications.Add(MyNotificationSingletons.GridIsImmune);
      }
      if (targetBlock != null && !targetBlock.CubeGrid.Immune)
      {
        MyCubeBlockDefinition.PreloadConstructionModels(targetBlock.BlockDefinition);
        if (Sync.IsServer)
        {
          float num = 1f;
          if (targetBlock.FatBlock != null && this.Owner != null && (this.Owner.ControllerInfo.Controller != null && this.Owner.ControllerInfo.Controller.Player != null))
          {
            switch (targetBlock.FatBlock.GetUserRelationToOwner(this.Owner.ControllerInfo.Controller.Player.Identity.IdentityId))
            {
              case MyRelationsBetweenPlayerAndBlock.Neutral:
              case MyRelationsBetweenPlayerAndBlock.Enemies:
                num = MySession.Static.HackSpeedMultiplier;
                break;
            }
          }
          MyDamageInformation info = new MyDamageInformation(false, this.GrinderAmount * num, MyDamageType.Grind, this.EntityId);
          if (targetBlock.UseDamageSystem)
            MyDamageSystem.Static.RaiseBeforeDamageApplied((object) targetBlock, ref info);
          if (targetBlock.CubeGrid.Editable)
          {
            targetBlock.DecreaseMountLevel(info.Amount, (MyInventoryBase) this.CharacterInventory, identityId: this.Owner.ControllerInfo.ControllingIdentityId);
            if (targetBlock.MoveItemsFromConstructionStockpile((MyInventoryBase) this.CharacterInventory) && this.Owner.ControllerInfo != null && (this.Owner.ControllerInfo.Controller != null && this.Owner.ControllerInfo.Controller.Player != null))
              this.SendInventoryFullNotification(this.Owner.ControllerInfo.Controller.Player.Id.SteamId);
            long attackedIdentityId = targetBlock.CubeGrid.BigOwners.Count > 0 ? targetBlock.CubeGrid.BigOwners[0] : 0L;
            if (this.Owner.ControllerInfo.ControllingIdentityId != attackedIdentityId)
              MySession.Static.Factions.DamageFactionPlayerReputation(this.Owner.ControllerInfo.ControllingIdentityId, attackedIdentityId, MyReputationDamageType.GrindingWelding);
          }
          if (MySession.Static != null && this.Owner == MySession.Static.LocalCharacter && MyMusicController.Static != null)
            MyMusicController.Static.Building(250);
          if (targetBlock.UseDamageSystem)
            MyDamageSystem.Static.RaiseAfterDamageApplied((object) targetBlock, info);
          if (targetBlock.IsFullyDismounted)
          {
            if (targetBlock.UseDamageSystem)
              MyDamageSystem.Static.RaiseDestroyed((object) targetBlock, info);
            targetBlock.SpawnConstructionStockpile();
            ulong user2 = 0;
            MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer((MyEntity) this.Owner);
            if (controllingPlayer != null)
              user2 = controllingPlayer.Id.SteamId;
            targetBlock.CubeGrid.RazeBlock(targetBlock.Min, user2);
          }
        }
        if (targetBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeName.Length > 0)
          materialType2 = targetBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
      }
      IMyDestroyableObject targetDestroyable = this.GetTargetDestroyable();
      if (targetDestroyable != null)
      {
        if (targetDestroyable is MyEntity && !MySessionComponentSafeZones.IsActionAllowed((MyEntity) targetDestroyable, MySafeZoneAction.Grinding) || targetDestroyable is MyCharacter target && target == this.Owner)
          return;
        if (Sync.IsServer)
        {
          float num = target != null ? MyAngleGrinder.BASE_GRINDER_CHARACTER_DAMAGE : MyAngleGrinder.BASE_GRINDER_DAMAGE;
          float damage = this.IsFriendlyFireReduced(target) ? 0.0f : num;
          if (target != null && MySession.Static.ControlledEntity == this.Owner && !target.IsDead)
            MySession.Static.TotalDamageDealt += (uint) damage;
          targetDestroyable.DoDamage(damage, MyDamageType.Grind, true, attackerId: (this.Owner != null ? this.Owner.EntityId : 0L));
        }
        if (target != null)
          materialType2 = MyStringHash.GetOrCompute(target.Definition.PhysicalMaterial);
      }
      MyEnvironmentSector environmentSector = this.m_raycastComponent.HitEnvironmentSector;
      if (environmentSector != null)
      {
        if (Sync.IsServer)
        {
          int environmentItem = this.m_raycastComponent.EnvironmentItem;
          if (environmentItem != this.m_lastItemId)
          {
            this.m_lastItemId = environmentItem;
            this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
          }
          if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastContactTime) > 1500.0 / (double) this.m_speedMultiplier)
          {
            MyBreakableEnvironmentProxy module = environmentSector.GetModule<MyBreakableEnvironmentProxy>();
            Vector3D vector3D = this.Owner.WorldMatrix.Right + this.Owner.WorldMatrix.Forward;
            vector3D.Normalize();
            double num1 = 10.0;
            float num2 = (float) (num1 * num1) * this.Owner.Physics.Mass;
            int itemId = environmentItem;
            Vector3D hitPosition = this.m_raycastComponent.HitPosition;
            Vector3D hitnormal = vector3D;
            double impactEnergy = (double) num2;
            module.BreakAt(itemId, hitPosition, hitnormal, impactEnergy);
            this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
            this.m_lastItemId = 0;
          }
        }
        materialType2 = MyStringHash.GetOrCompute("Wood");
      }
      if (targetBlock == null && targetDestroyable == null && environmentSector == null)
        return;
      this.m_actualSound = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, this.m_handItemDef.ToolMaterial, materialType2);
      this.m_effectId = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, this.m_handItemDef.ToolMaterial, materialType2);
    }

    private void SendInventoryFullNotification(ulong clientId) => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyAngleGrinder.OnInventoryFulfilled)), new EndpointId(clientId));

    [Event(null, 416)]
    [Reliable]
    [Client]
    private static void OnInventoryFulfilled()
    {
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - MyAngleGrinder.m_lastTimePlayedSound > 2500)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudVocInventoryFull);
        MyAngleGrinder.m_lastTimePlayedSound = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
      MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
    }

    protected override void StartLoopSound(bool effect)
    {
      bool force2D = this.Owner != null && this.Owner.IsInFirstPersonView && this.Owner == MySession.Static.LocalCharacter;
      MySoundPair soundId = effect ? this.m_actualSound : this.m_idleSound;
      if (this.m_soundEmitter.Sound != null && this.m_soundEmitter.Sound.IsPlaying)
      {
        if (force2D != this.m_soundEmitter.Force2D)
          this.m_soundEmitter.PlaySound(soundId, true, true, force2D);
        else
          this.m_soundEmitter.PlaySingleSound(soundId, true);
      }
      else
        this.m_soundEmitter.PlaySound(soundId, true, force2D: force2D);
    }

    protected override void StopLoopSound() => this.m_soundEmitter.StopSound(false);

    protected override void StopSound()
    {
      if (this.m_soundEmitter.Sound == null || !this.m_soundEmitter.Sound.IsPlaying)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    public void PerformCameraShake()
    {
      if (MySector.MainCamera == null)
        return;
      MySector.MainCamera.CameraShake.AddShake(MathHelper.Clamp((float) (-Math.Log(MyRandom.Instance.NextDouble()) * this.m_grinderCameraMeanShakeIntensity) * MyAngleGrinder.GRINDER_MAX_SHAKE, 0.0f, MyAngleGrinder.GRINDER_MAX_SHAKE));
    }

    public override bool SupressShootAnimation() => false;

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.Owner, MySafeZoneAction.Grinding))
      {
        status = MyGunStatusEnum.SafeZoneDenied;
        return false;
      }
      this.SinkComp.Update();
      if (this.SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        return base.CanShoot(action, shooter, out status);
      status = MyGunStatusEnum.OutOfPower;
      return false;
    }

    public new bool ShouldEndShootOnPause(MyShootActionEnum action) => !this.m_isActionDoubleClicked.ContainsKey(action) || !this.m_isActionDoubleClicked[action];

    public new bool CanDoubleClickToStick(MyShootActionEnum action) => true;

    protected sealed class OnInventoryFulfilled\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAngleGrinder.OnInventoryFulfilled();
      }
    }

    private class Sandbox_Game_Weapons_MyAngleGrinder\u003C\u003EActor : IActivator, IActivator<MyAngleGrinder>
    {
      object IActivator.CreateInstance() => (object) new MyAngleGrinder();

      MyAngleGrinder IActivator<MyAngleGrinder>.CreateInstance() => new MyAngleGrinder();
    }
  }
}
