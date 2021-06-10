// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyWelder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MyEntityType(typeof (MyObjectBuilder_Welder), true)]
  public class MyWelder : MyEngineerToolBase, IMyWelder, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEngineerToolBase, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
  {
    private MySoundPair m_weldSoundIdle = new MySoundPair("ToolPlayWeldIdle");
    private MySoundPair m_weldSoundWeld = new MySoundPair("ToolPlayWeldMetal");
    private MySoundPair m_weldSoundFlame = new MySoundPair("ArcShipSmNuclearLrg");
    public static readonly float WELDER_AMOUNT_PER_SECOND = 1f;
    public static readonly float WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED = 0.6f;
    public static MatrixD WELDER_ANGLE = MatrixD.CreateRotationX(0.490000009536743);
    private static int SUPRESS_TIME_LIMIT = 180;
    private static MyHudNotificationBase m_missingComponentNotification = (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationMissingComponentToPlaceBlockFormat, font: "Red");
    private MyHudNotification m_safezoneNotification;
    private static MyDefinitionId m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "WelderItem");
    private MySlimBlock m_failedBlock;
    private bool m_playedFailSound;
    private MySlimBlock m_failedBlockSound;
    private float m_lastWeldingDistance = float.MaxValue;
    private bool m_lastWeldingDistanceCheck;
    private int m_timedShootSupression;
    private Vector3I m_targetProjectionCube;
    private MyCubeGrid m_targetProjectionGrid;
    private MyParticleEffect m_flameEffect;
    private string m_flameEffectName = "WelderFlame";
    private bool m_showContactSpark = true;

    private bool ShowContactSpark
    {
      get => this.m_showContactSpark;
      set
      {
        if (this.m_showContactSpark == value)
          return;
        this.m_showContactSpark = value;
        this.ShowContactSparkChanged();
      }
    }

    public override bool IsSkinnable => true;

    public MyWelder()
      : base(250)
    {
      this.HasCubeHighlight = true;
      this.HighlightColor = Color.Green * 0.75f;
      this.HighlightMaterial = MyStringId.GetOrCompute("GizmoDrawLine");
      this.SecondaryLightIntensityLower = 0.4f;
      this.SecondaryLightIntensityUpper = 0.4f;
      this.SecondaryEffectName = "WelderContactPoint";
      this.HasSecondaryEffect = false;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      MyWelder.m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "WelderItem");
      if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
        MyWelder.m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), objectBuilder.SubtypeName + "Item");
      this.PhysicalObject = (MyObjectBuilder_PhysicalGunObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) MyWelder.m_physicalItemId);
      this.Init(objectBuilder, MyWelder.m_physicalItemId);
      this.Init((StringBuilder) null, MyDefinitionManager.Static.GetPhysicalItemDefinition(MyWelder.m_physicalItemId).Model, (MyEntity) null, new float?());
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      this.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase) objectBuilder.Clone();
      this.PhysicalObject.GunEntity.EntityId = this.EntityId;
      if (MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(MyWelder.m_physicalItemId) is MyWelderDefinition itemForPhysicalItem)
        this.m_flameEffectName = itemForPhysicalItem.FlameEffect;
      foreach (ToolSound toolSound in this.m_handItemDef.ToolSounds)
      {
        if (toolSound.type != null && toolSound.subtype != null && (toolSound.sound != null && toolSound.type.Equals("Main")))
        {
          if (toolSound.subtype.Equals("Idle"))
            this.m_weldSoundIdle = new MySoundPair(toolSound.sound);
          if (toolSound.subtype.Equals("Weld"))
            this.m_weldSoundWeld = new MySoundPair(toolSound.sound);
          if (toolSound.subtype.Equals("Flame"))
            this.m_weldSoundFlame = new MySoundPair(toolSound.sound);
        }
      }
    }

    protected override bool ShouldBePowered() => base.ShouldBePowered();

    protected override void DrawHud()
    {
      Vector3I targetProjectionCube = this.m_targetProjectionCube;
      if (this.m_targetProjectionGrid == null)
      {
        base.DrawHud();
      }
      else
      {
        MySlimBlock mySlimBlock = this.m_targetProjectionGrid.GetCubeBlock(this.m_targetProjectionCube);
        if (mySlimBlock == null)
        {
          base.DrawHud();
        }
        else
        {
          if (MyFakes.ENABLE_COMPOUND_BLOCKS && mySlimBlock.FatBlock is MyCompoundCubeBlock)
          {
            MyCompoundCubeBlock fatBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
            if (fatBlock.GetBlocksCount() > 0)
              mySlimBlock = fatBlock.GetBlocks().First<MySlimBlock>();
          }
          int num = mySlimBlock.GetStockpileStamp() + mySlimBlock.ComponentStack.LastChangeStamp;
          if (this.LastTargetObject == mySlimBlock && num == this.LastTargetStamp)
            return;
          this.LastTargetStamp = num;
          MyHud.BlockInfo.MissingComponentIndex = 0;
          MyHud.BlockInfo.DefinitionId = mySlimBlock.BlockDefinition.Id;
          MyHud.BlockInfo.BlockName = mySlimBlock.BlockDefinition.DisplayNameText;
          MyHud.BlockInfo.PCUCost = mySlimBlock.BlockDefinition.PCU;
          MyHud.BlockInfo.BlockIcons = mySlimBlock.BlockDefinition.Icons;
          MyHud.BlockInfo.BlockIntegrity = 0.01f;
          MyHud.BlockInfo.CriticalIntegrity = mySlimBlock.BlockDefinition.CriticalIntegrityRatio;
          MyHud.BlockInfo.CriticalComponentIndex = (int) mySlimBlock.BlockDefinition.CriticalGroup;
          MyHud.BlockInfo.OwnershipIntegrity = mySlimBlock.BlockDefinition.OwnershipIntegrityRatio;
          MyHud.BlockInfo.BlockBuiltBy = mySlimBlock.BuiltBy;
          MyHud.BlockInfo.GridSize = mySlimBlock.CubeGrid.GridSizeEnum;
          MyHud.BlockInfo.Components.Clear();
          for (int index = 0; index < mySlimBlock.ComponentStack.GroupCount; ++index)
          {
            MyComponentStack.GroupInfo groupInfo = mySlimBlock.ComponentStack.GetGroupInfo(index);
            MyHud.BlockInfo.Components.Add(new MyHudBlockInfo.ComponentInfo()
            {
              DefinitionId = groupInfo.Component.Id,
              ComponentName = groupInfo.Component.DisplayNameText,
              Icons = groupInfo.Component.Icons,
              TotalCount = groupInfo.TotalCount,
              MountedCount = 0,
              StockpileCount = 0
            });
          }
          MyHud.BlockInfo.SetContextHelp((MyDefinitionBase) mySlimBlock.BlockDefinition);
          this.LastTargetObject = (object) mySlimBlock;
        }
      }
    }

    private float WeldAmount => (float) ((double) MySession.Static.WelderSpeedMultiplier * (double) this.m_speedMultiplier * (double) MyWelder.WELDER_AMOUNT_PER_SECOND * (double) this.ToolCooldownMs / 1000.0);

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.Owner, MySafeZoneAction.Welding))
      {
        status = MyGunStatusEnum.SafeZoneDenied;
        return false;
      }
      if (action == MyShootActionEnum.PrimaryAction)
      {
        if (!base.CanShoot(action, shooter, out status) || !this.SinkComp.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, 0.0001f) && MySession.Static != null && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
          return false;
      }
      else
        status = MyGunStatusEnum.OK;
      MySlimBlock targetBlock = this.GetTargetBlock();
      MyCharacter owner = this.Owner;
      if (targetBlock != null && !targetBlock.CanContinueBuild((MyInventoryBase) MyEntityExtensions.GetInventory(owner)) && (!targetBlock.IsFullIntegrity && this.Owner != null) && (this.Owner == MySession.Static.LocalCharacter && MySession.Static.Settings.GameMode == MyGameModeEnum.Survival && !MySession.Static.CreativeToolsEnabled(Sync.MyId)))
      {
        int groupIndex;
        int componentCount;
        targetBlock.ComponentStack.GetMissingInfo(out groupIndex, out componentCount);
        MyComponentStack.GroupInfo groupInfo = targetBlock.ComponentStack.GetGroupInfo(groupIndex);
        this.MarkMissingComponent(groupIndex);
        MyWelder.m_missingComponentNotification.SetTextFormatArguments((object) string.Format("{0} ({1}x)", (object) groupInfo.Component.DisplayNameText, (object) componentCount), (object) targetBlock.BlockDefinition.DisplayNameText.ToString());
        MyHud.Notifications.Add(MyWelder.m_missingComponentNotification);
        if (this.m_playedFailSound && this.m_failedBlockSound != targetBlock || !this.m_playedFailSound)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
          this.m_playedFailSound = true;
          this.m_failedBlockSound = targetBlock;
        }
      }
      return true;
    }

    private bool CanWeld(MySlimBlock block)
    {
      ulong user = 0;
      if (this.Owner != null && this.Owner.ControllerInfo != null && (this.Owner.ControllerInfo.Controller != null && this.Owner.ControllerInfo.Controller.Player != null))
        user = this.Owner.ControllerInfo.Controller.Player.Id.SteamId;
      return MySessionComponentSafeZones.IsActionAllowed(block.WorldAABB, MySafeZoneAction.Welding, user: user) && (!block.IsFullIntegrity || block.HasDeformation);
    }

    private MyProjectorBase GetProjector(MySlimBlock block)
    {
      MySlimBlock mySlimBlock = block.CubeGrid.GetBlocks().FirstOrDefault<MySlimBlock>((Func<MySlimBlock, bool>) (b => b.FatBlock is MyProjectorBase));
      return mySlimBlock != null ? mySlimBlock.FatBlock as MyProjectorBase : (MyProjectorBase) null;
    }

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      base.Shoot(action, direction, overrideWeaponPos, gunAction);
      this.ShowContactSpark = false;
      if (action != MyShootActionEnum.PrimaryAction)
        return;
      MySlimBlock targetBlock = this.GetTargetBlock();
      if (!this.IsPreheated)
      {
        if (targetBlock == null)
          return;
        this.FillStockpile();
      }
      else
      {
        if (targetBlock == null)
        {
          this.m_lastWeldingDistance = float.MaxValue;
          this.m_lastWeldingDistanceCheck = false;
        }
        if (targetBlock != null && this.m_activated && this.CanWeld(targetBlock))
        {
          if (!MySession.Static.CheckResearchAndNotify(this.Owner.GetPlayerIdentityId(), targetBlock.BlockDefinition.Id))
            return;
          this.Weld();
        }
        else
        {
          if (this.Owner == null || this.Owner != MySession.Static.LocalCharacter)
            return;
          MyWelder.ProjectionRaycastData projectedBlock = MyWelder.FindProjectedBlock(this.m_raycastComponent, this.m_distanceMultiplier);
          if (projectedBlock.raycastResult != BuildCheckResult.OK || !MySession.Static.CheckResearchAndNotify(this.Owner.GetPlayerIdentityId(), projectedBlock.hitCube.BlockDefinition.Id))
            return;
          bool creativeMode = MySession.Static.CreativeMode;
          MyPlayer.PlayerId result;
          if (MySession.Static.Players.TryGetPlayerId(this.OwnerIdentityId, out result) && MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
            creativeMode |= MySession.Static.CreativeToolsEnabled(Sync.MyId);
          if (!MySession.Static.CheckLimitsAndNotify(targetBlock != null ? targetBlock.BuiltBy : this.Owner.ControllerInfo.Controller.Player.Identity.IdentityId, projectedBlock.hitCube.BlockDefinition.BlockPairName, creativeMode ? projectedBlock.hitCube.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, blocksCount: projectedBlock.cubeProjector.CubeGrid.BlocksCount))
            return;
          if (MySession.Static.CreativeMode || MyBlockBuilderBase.SpectatorIsBuilding || (this.Owner.CanStartConstruction(projectedBlock.hitCube.BlockDefinition) || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
            projectedBlock.cubeProjector.Build(projectedBlock.hitCube, this.Owner.ControllerInfo.Controller.Player.Identity.IdentityId, this.Owner.EntityId, builtBy: this.Owner.ControllerInfo.Controller.Player.Identity.IdentityId);
          else
            MyBlockPlacerBase.OnMissingComponents(projectedBlock.hitCube.BlockDefinition);
        }
      }
    }

    public static void AddMissingComponentsToBuildPlanner(MySlimBlock block)
    {
      if (block == null)
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      else if (block.CubeGrid.IsPreview)
      {
        if (!MySession.Static.LocalCharacter.AddToBuildPlanner(block.BlockDefinition))
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerComponentsAdded);
        MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
      }
      else
      {
        Dictionary<string, int> addToDictionary = new Dictionary<string, int>();
        block.GetMissingComponents(addToDictionary);
        List<MyIdentity.BuildPlanItem.Component> components = new List<MyIdentity.BuildPlanItem.Component>();
        foreach (KeyValuePair<string, int> keyValuePair in addToDictionary)
        {
          MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), keyValuePair.Key));
          components.Add(new MyIdentity.BuildPlanItem.Component()
          {
            ComponentDefinition = componentDefinition,
            Count = keyValuePair.Value
          });
        }
        if (components.Count <= 0 || !MySession.Static.LocalCharacter.AddToBuildPlanner(block.BlockDefinition, components: components))
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerComponentsAdded);
        MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
      }
    }

    public override void EndShoot(MyShootActionEnum action)
    {
      if (this.m_shooting && action == MyShootActionEnum.SecondaryAction && (!Sync.IsDedicated && MySession.Static != null) && this.Owner == MySession.Static.LocalCharacter)
      {
        MySlimBlock targetBlock = this.GetTargetBlock();
        if (targetBlock != null)
          MyWelder.AddMissingComponentsToBuildPlanner(targetBlock);
        else if (this.Owner != null)
        {
          MyWelder.ProjectionRaycastData projectedBlock = MyWelder.FindProjectedBlock(this.m_raycastComponent, this.m_distanceMultiplier);
          if (projectedBlock.hitCube != null)
            MyWelder.AddMissingComponentsToBuildPlanner(projectedBlock.hitCube);
          else
            MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        }
      }
      this.m_playedFailSound = false;
      this.m_failedBlockSound = (MySlimBlock) null;
      base.EndShoot(action);
    }

    public override void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
      base.BeginFailReaction(action, status);
      this.FillStockpile();
    }

    public override void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status != MyGunStatusEnum.SafeZoneDenied)
        return;
      if (this.m_safezoneNotification == null)
        this.m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_WeldingDisabled, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneNotification);
    }

    protected override void AddHudInfo()
    {
    }

    protected override void RemoveHudInfo()
    {
    }

    private void FillStockpile()
    {
      MySlimBlock targetBlock = this.GetTargetBlock();
      if (targetBlock == null)
        return;
      this.FillStockpile(targetBlock);
    }

    private void FillStockpile(MySlimBlock block)
    {
      if (Sync.IsServer)
        block.MoveItemsToConstructionStockpile((MyInventoryBase) this.CharacterInventory);
      else
        block.RequestFillStockpile(this.CharacterInventory);
    }

    private void ShowContactSparkChanged()
    {
      if (this.m_showContactSpark)
        return;
      this.StopEffect();
    }

    public override bool CanStartEffect() => this.m_showContactSpark;

    private void Weld()
    {
      bool flag = false;
      MySlimBlock targetBlock = this.GetTargetBlock();
      if (targetBlock != null)
      {
        MyCubeBlockDefinition.PreloadConstructionModels(targetBlock.BlockDefinition);
        if (Sync.IsServer)
        {
          targetBlock.MoveItemsToConstructionStockpile((MyInventoryBase) this.CharacterInventory);
          targetBlock.MoveUnneededItemsFromConstructionStockpile((MyInventoryBase) this.CharacterInventory);
        }
        bool hasDeformation = targetBlock.HasDeformation;
        if (hasDeformation || (double) targetBlock.MaxDeformation > 0.0 || !targetBlock.IsFullIntegrity)
        {
          float maxAllowedBoneMovement = (float) ((double) MyWelder.WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED * (double) this.ToolCooldownMs * (1.0 / 1000.0));
          if (this.Owner != null && this.Owner.ControllerInfo != null)
          {
            bool? nullable = targetBlock.ComponentStack.WillFunctionalityRise(this.WeldAmount);
            if (nullable.HasValue && nullable.Value && !MySession.Static.CheckLimitsAndNotify(targetBlock.BuiltBy, targetBlock.BlockDefinition.BlockPairName, targetBlock.BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST))
              return;
            flag = (!Sync.IsServer ? (!targetBlock.IsFullIntegrity || targetBlock.HasDeformation) && this.m_failedBlockSound == null : targetBlock.IncreaseMountLevel(this.WeldAmount, this.Owner.ControllerInfo.ControllingIdentityId, (MyInventoryBase) this.CharacterInventory, maxAllowedBoneMovement, handWelded: true)) | hasDeformation;
            if (MySession.Static != null && this.Owner == MySession.Static.LocalCharacter && MyMusicController.Static != null)
              MyMusicController.Static.Building(250);
          }
        }
      }
      if (Sync.IsServer)
      {
        IMyDestroyableObject targetDestroyable = this.GetTargetDestroyable();
        if (targetDestroyable is MyCharacter && Sync.IsServer)
          targetDestroyable.DoDamage(20f, MyDamageType.Weld, true, attackerId: this.EntityId);
      }
      this.ShowContactSpark = flag;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.m_timedShootSupression > 0)
        --this.m_timedShootSupression;
      if (this.Owner != null && this.Owner == MySession.Static.LocalCharacter)
        this.CheckProjection();
      if (this.Owner == null || MySession.Static.ControlledEntity != this.Owner)
        this.RemoveHudInfo();
      this.UpdateFlameEffect();
    }

    private void UpdateFlameEffect()
    {
      MyShootActionEnum? effectAction1 = this.EffectAction;
      MyShootActionEnum myShootActionEnum1 = MyShootActionEnum.PrimaryAction;
      if (!(effectAction1.GetValueOrDefault() == myShootActionEnum1 & effectAction1.HasValue))
      {
        MyShootActionEnum? effectAction2 = this.EffectAction;
        MyShootActionEnum myShootActionEnum2 = MyShootActionEnum.SecondaryAction;
        if (!(effectAction2.GetValueOrDefault() == myShootActionEnum2 & effectAction2.HasValue))
        {
          if (this.m_flameEffect == null)
            return;
          this.m_flameEffect.Stop();
          this.m_flameEffect = (MyParticleEffect) null;
          return;
        }
      }
      if (this.m_flameEffect != null)
        return;
      MyParticlesManager.TryCreateParticleEffect("WelderFlame", this.GetEffectMatrix(0.0f, MyEngineerToolBase.EffectType.EffectSecondary), out this.m_flameEffect);
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      if (this.m_flameEffect == null)
        return;
      this.m_flameEffect.WorldMatrix = this.GetEffectMatrix(0.0f, MyEngineerToolBase.EffectType.EffectSecondary);
    }

    protected override MatrixD GetEffectMatrix(
      float muzzleOffset,
      MyEngineerToolBase.EffectType effectType)
    {
      Vector3D forward = this.PositionComp.WorldMatrixRef.Forward;
      Vector3D muzzleWorldPosition = this.m_gunBase.GetMuzzleWorldPosition();
      if (effectType != MyEngineerToolBase.EffectType.Effect)
        return MatrixD.CreateWorld(muzzleWorldPosition, forward, this.PositionComp.WorldMatrixRef.Up);
      Vector3D vector3D1 = Vector3D.Rotate(MyWelder.WELDER_ANGLE.Forward, this.PositionComp.WorldMatrixRef);
      Vector3D vector3D2 = muzzleWorldPosition + 0.0500000007450581 * this.PositionComp.WorldMatrixRef.Up;
      this.m_lastWeldingDistance = Vector3.Dot((Vector3) (this.m_raycastComponent.HitPosition - vector3D2), (Vector3) forward);
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(vector3D2 - 0.5 * vector3D1, vector3D2 + 1.5 * vector3D1, 15);
      Vector3D position;
      if (nullable.HasValue)
      {
        float num = Vector3.Dot((Vector3) (nullable.Value.Position - vector3D2), (Vector3) vector3D1);
        position = (double) num <= 0.100000001490116 ? vector3D2 + (double) num * vector3D1 : vector3D2 + 0.100000001490116 * vector3D1;
      }
      else
        position = vector3D2 + 0.100000001490116 * vector3D1;
      return MatrixD.CreateWorld(position, -vector3D1, this.PositionComp.WorldMatrixRef.Up);
    }

    protected override MySlimBlock GetTargetBlockForShoot()
    {
      MySlimBlock targetBlock = this.GetTargetBlock();
      return targetBlock != null && this.ShowContactSpark ? targetBlock : (MySlimBlock) null;
    }

    private void CheckProjection()
    {
      MySlimBlock targetBlock = this.GetTargetBlock();
      if (targetBlock != null && this.CanWeld(targetBlock))
      {
        this.m_targetProjectionGrid = (MyCubeGrid) null;
      }
      else
      {
        if (this.Owner != null)
        {
          MyWelder.ProjectionRaycastData projectedBlock = MyWelder.FindProjectedBlock(this.m_raycastComponent, this.m_distanceMultiplier);
          if (projectedBlock.raycastResult != BuildCheckResult.NotFound)
          {
            if (projectedBlock.raycastResult == BuildCheckResult.OK)
            {
              MyCubeBuilder.DrawSemiTransparentBox(projectedBlock.hitCube.CubeGrid, projectedBlock.hitCube, (Color) Color.Green.ToVector4(), true, new MyStringId?(MyStringId.GetOrCompute("GizmoDrawLine")));
              this.m_targetProjectionCube = projectedBlock.hitCube.Position;
              this.m_targetProjectionGrid = projectedBlock.hitCube.CubeGrid;
              return;
            }
            if (projectedBlock.raycastResult == BuildCheckResult.IntersectedWithGrid || projectedBlock.raycastResult == BuildCheckResult.IntersectedWithSomethingElse)
              MyCubeBuilder.DrawSemiTransparentBox(projectedBlock.hitCube.CubeGrid, projectedBlock.hitCube, (Color) Color.Red.ToVector4(), true);
            else if (projectedBlock.raycastResult == BuildCheckResult.NotConnected)
              MyCubeBuilder.DrawSemiTransparentBox(projectedBlock.hitCube.CubeGrid, projectedBlock.hitCube, (Color) Color.Yellow.ToVector4(), true);
          }
        }
        this.m_targetProjectionGrid = (MyCubeGrid) null;
      }
    }

    public static MyWelder.ProjectionRaycastData FindProjectedBlock(
      MyCasterComponent rayCaster,
      float distanceMultiplier = 1f)
    {
      Vector3D center = rayCaster.Caster.Center;
      Vector3D vector3D1 = rayCaster.Caster.FrontPoint - rayCaster.Caster.Center;
      vector3D1.Normalize();
      float num = MyEngineerToolBase.DEFAULT_REACH_DISTANCE * distanceMultiplier;
      Vector3D vector3D2 = center + vector3D1 * (double) num;
      LineD line = new LineD(center, vector3D2);
      MyCubeGrid grid;
      MyWelder.ProjectionRaycastData projectionRaycastData1;
      if (MyCubeGrid.GetLineIntersection(ref line, out grid, out Vector3I _, out double _, (Func<MyCubeGrid, bool>) (x => x.Projector != null)) && grid.Projector != null)
      {
        MyProjectorBase projector = grid.Projector;
        List<MyCube> myCubeList = grid.RayCastBlocksAllOrdered(center, vector3D2);
        MyWelder.ProjectionRaycastData? nullable = new MyWelder.ProjectionRaycastData?();
        for (int index = myCubeList.Count - 1; index >= 0; --index)
        {
          MyCube myCube = myCubeList[index];
          BuildCheckResult buildCheckResult = projector.CanBuild(myCube.CubeBlock, true);
          switch (buildCheckResult)
          {
            case BuildCheckResult.OK:
              ref MyWelder.ProjectionRaycastData? local = ref nullable;
              projectionRaycastData1 = new MyWelder.ProjectionRaycastData();
              projectionRaycastData1.raycastResult = buildCheckResult;
              projectionRaycastData1.hitCube = myCube.CubeBlock;
              projectionRaycastData1.cubeProjector = projector;
              MyWelder.ProjectionRaycastData projectionRaycastData2 = projectionRaycastData1;
              local = new MyWelder.ProjectionRaycastData?(projectionRaycastData2);
              break;
            case BuildCheckResult.AlreadyBuilt:
              nullable = new MyWelder.ProjectionRaycastData?();
              break;
          }
        }
        if (nullable.HasValue)
          return nullable.Value;
      }
      projectionRaycastData1 = new MyWelder.ProjectionRaycastData();
      projectionRaycastData1.raycastResult = BuildCheckResult.NotFound;
      return projectionRaycastData1;
    }

    protected override void StartLoopSound(bool effect)
    {
      bool force2D = this.Owner != null && this.Owner.IsInFirstPersonView && this.Owner == MySession.Static.LocalCharacter;
      MySoundPair soundId = effect ? this.m_weldSoundWeld : this.m_weldSoundFlame;
      if (this.m_soundEmitter.Sound != null && this.m_soundEmitter.Sound.IsPlaying)
      {
        if (force2D != this.m_soundEmitter.Force2D)
          this.m_soundEmitter.PlaySound(soundId, true, true, force2D);
        else
          this.m_soundEmitter.PlaySingleSound(soundId, true, true);
      }
      else
        this.m_soundEmitter.PlaySound(soundId, true, true, force2D);
    }

    protected override void StopLoopSound() => this.StopSound();

    protected override void StopSound() => this.m_soundEmitter.StopSound(true);

    protected override void Closing()
    {
      base.Closing();
      if (this.m_flameEffect == null)
        return;
      this.m_flameEffect.Stop();
      this.m_flameEffect = (MyParticleEffect) null;
    }

    public override bool SupressShootAnimation()
    {
      bool flag = (double) this.m_lastWeldingDistance < 0.0500000007450581;
      if (this.m_lastWeldingDistanceCheck != flag && this.m_timedShootSupression < MyWelder.SUPRESS_TIME_LIMIT)
      {
        if (this.m_timedShootSupression > 0)
          this.m_timedShootSupression += (int) ((double) MyRandom.Instance.GetRandomFloat(0.8f, 1.6f) * (double) MyWelder.SUPRESS_TIME_LIMIT);
        else
          this.m_timedShootSupression += MyWelder.SUPRESS_TIME_LIMIT;
      }
      this.m_lastWeldingDistanceCheck = flag;
      return flag || this.m_timedShootSupression > MyWelder.SUPRESS_TIME_LIMIT;
    }

    public new bool ShouldEndShootOnPause(MyShootActionEnum action) => !this.m_isActionDoubleClicked.ContainsKey(action) || !this.m_isActionDoubleClicked[action];

    public new bool CanDoubleClickToStick(MyShootActionEnum action) => true;

    public struct ProjectionRaycastData
    {
      public BuildCheckResult raycastResult;
      public MySlimBlock hitCube;
      public MyProjectorBase cubeProjector;

      public ProjectionRaycastData(
        BuildCheckResult result,
        MySlimBlock cubeBlock,
        MyProjectorBase projector)
      {
        this.raycastResult = result;
        this.hitCube = cubeBlock;
        this.cubeProjector = projector;
      }
    }

    private class Sandbox_Game_Weapons_MyWelder\u003C\u003EActor : IActivator, IActivator<MyWelder>
    {
      object IActivator.CreateInstance() => (object) new MyWelder();

      MyWelder IActivator<MyWelder>.CreateInstance() => new MyWelder();
    }
  }
}
