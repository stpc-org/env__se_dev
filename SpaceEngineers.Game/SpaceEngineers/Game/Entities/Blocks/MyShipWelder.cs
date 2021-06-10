// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyShipWelder
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ShipWelder))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyShipWelder), typeof (Sandbox.ModAPI.Ingame.IMyShipWelder)})]
  public class MyShipWelder : MyShipToolBase, Sandbox.ModAPI.IMyShipWelder, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyShipWelder
  {
    private static MySoundPair METAL_SOUND = new MySoundPair("ToolLrgWeldMetal");
    private static MySoundPair IDLE_SOUND = new MySoundPair("ToolLrgWeldIdle");
    private const string PARTICLE_EFFECT = "ShipWelderArc";
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_helpOthers;
    public static readonly float WELDER_AMOUNT_PER_SECOND = 4f;
    public static readonly float WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED = 0.6f;
    private Dictionary<string, int> m_missingComponents;
    private List<MyWelder.ProjectionRaycastData> m_raycastData = new List<MyWelder.ProjectionRaycastData>();
    private HashSet<MySlimBlock> m_projectedBlock = new HashSet<MySlimBlock>();
    private MyParticleEffect m_particleEffect;
    private MyFlareDefinition m_flare;
    private MyShipWelderDefinition m_welderDef;
    private Matrix m_particleDummyMatrix1;

    public bool HelpOthers
    {
      get => (bool) this.m_helpOthers;
      set => this.m_helpOthers.Value = value;
    }

    protected override bool CanInteractWithSelf => true;

    public MyShipWelder() => this.CreateTerminalControls();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyShipWelder>())
        return;
      base.CreateTerminalControls();
      if (!MyFakes.ENABLE_WELDER_HELP_OTHERS)
        return;
      MyTerminalControlCheckbox<MyShipWelder> checkbox = new MyTerminalControlCheckbox<MyShipWelder>("helpOthers", MyCommonTexts.ShipWelder_HelpOthers, MyCommonTexts.ShipWelder_HelpOthers);
      checkbox.Getter = (MyTerminalValueControl<MyShipWelder, bool>.GetterDelegate) (x => x.HelpOthers);
      checkbox.Setter = (MyTerminalValueControl<MyShipWelder, bool>.SetterDelegate) ((x, v) => x.m_helpOthers.Value = v);
      checkbox.EnableAction<MyShipWelder>();
      MyTerminalControlFactory.AddControl<MyShipWelder>((MyTerminalControl<MyShipWelder>) checkbox);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      base.Init(objectBuilder, cubeGrid);
      this.m_missingComponents = new Dictionary<string, int>();
      this.m_welderDef = this.BlockDefinition as MyShipWelderDefinition;
      if (this.m_welderDef != null)
      {
        if (this.m_welderDef.Flare != "")
          this.m_flare = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), this.m_welderDef.Flare)) as MyFlareDefinition;
        if (this.m_welderDef.EmissiveColorPreset == MyStringHash.NullOrEmpty)
          this.m_welderDef.EmissiveColorPreset = MyStringHash.GetOrCompute("Welder");
      }
      this.m_helpOthers.SetLocalValue(((MyObjectBuilder_ShipWelder) objectBuilder).HelpOthers);
      this.LoadParticleDummyMatrices();
    }

    private void LoadParticleDummyMatrices()
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        if (dummy.Key.ToLower().Contains("particles1"))
          this.m_particleDummyMatrix1 = dummy.Value.Matrix;
      }
    }

    public override void OnControlAcquired(MyCharacter owner) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.CubeGrid.Closed || (this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.ControlSystem == null || !this.CubeGrid.GridSystems.ControlSystem.IsLocallyControlled) && (owner == null || MySession.Static.LocalCharacter != owner))
        return;
      MyCharacter myCharacter = owner != null ? owner : this.CubeGrid.GridSystems.ControlSystem.GetController().Player.Character;
      if (myCharacter == null || myCharacter.Parent == null || myCharacter.Parent.Components.Contains(typeof (MyCasterComponent)))
        return;
      MyCasterComponent component = new MyCasterComponent((MyDrillSensorBase) new MyDrillSensorRayCast(0.0f, this.DEFAULT_REACH_DISTANCE, (MyDefinitionBase) this.BlockDefinition));
      myCharacter.Parent.Components.Add<MyCasterComponent>(component);
      this.m_controller = myCharacter;
    }), "MyShipWelder::OnControlAcquired");

    public override void OnControlReleased()
    {
      base.OnControlReleased();
      if (this.m_controller == null || this.m_controller.Parent == null || (this.m_controller != MySession.Static.LocalCharacter || !this.m_controller.Parent.Components.Contains(typeof (MyCasterComponent))))
        return;
      this.m_controller.Parent.Components.Remove(typeof (MyCasterComponent));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ShipWelder builderCubeBlock = (MyObjectBuilder_ShipWelder) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.HelpOthers = (bool) this.m_helpOthers;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public bool IsWithinWorldLimits(Sandbox.ModAPI.IMyProjector projector, string name, int pcuToBuild) => this.IsWithinWorldLimits(projector as MyProjectorBase, name, pcuToBuild);

    private bool IsWithinWorldLimits(MyProjectorBase projector, string name, int pcuToBuild)
    {
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
        return true;
      bool flag1 = true;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.BuiltBy);
      MyBlockLimits myBlockLimits = (MyBlockLimits) null;
      if (identity != null)
        myBlockLimits = identity.BlockLimits;
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && identity != null && MySession.Static.Factions.GetPlayerFaction(identity.IdentityId) == null)
        return false;
      bool flag2 = ((((flag1 ? 1 : 0) & (this.BuiltBy == 0L ? 1 : (this.IDModule.GetUserRelationToOwner(this.BuiltBy) != MyRelationsBetweenPlayerAndBlock.Enemies ? 1 : 0))) != 0 ? 1 : 0) & (projector.BuiltBy == 0L ? 1 : (this.IDModule.GetUserRelationToOwner(projector.BuiltBy) != MyRelationsBetweenPlayerAndBlock.Enemies ? 1 : 0))) != 0;
      if (identity != null)
      {
        if (MySession.Static.MaxBlocksPerPlayer > 0)
          flag2 &= myBlockLimits.BlocksBuilt < myBlockLimits.MaxBlocks;
        if (MySession.Static.TotalPCU != 0)
          flag2 &= myBlockLimits.PCU >= pcuToBuild;
      }
      bool flag3 = ((flag2 ? 1 : 0) & (MySession.Static.MaxGridSize == 0 ? 1 : (projector.CubeGrid.BlocksCount < MySession.Static.MaxGridSize ? 1 : 0))) != 0;
      short blockTypeLimit = MySession.Static.GetBlockTypeLimit(name);
      if (identity != null && blockTypeLimit > (short) 0)
      {
        MyBlockLimits.MyTypeLimitData myTypeLimitData;
        flag3 &= (myBlockLimits.BlockTypeBuilt.TryGetValue(name, out myTypeLimitData) ? myTypeLimitData.BlocksBuilt : 0) < (int) blockTypeLimit;
      }
      return flag3;
    }

    protected override bool Activate(HashSet<MySlimBlock> targets)
    {
      bool flag = false;
      int count = targets.Count;
      this.m_missingComponents.Clear();
      foreach (MySlimBlock target in targets)
      {
        if (target.IsFullIntegrity || target == this.SlimBlock)
        {
          --count;
        }
        else
        {
          MyCubeBlockDefinition.PreloadConstructionModels(target.BlockDefinition);
          target.GetMissingComponents(this.m_missingComponents);
        }
      }
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      foreach (KeyValuePair<string, int> missingComponent in this.m_missingComponents)
      {
        MyDefinitionId myDefinitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), missingComponent.Key);
        if (Math.Max(missingComponent.Value - (int) inventory.GetItemAmount(myDefinitionId, MyItemFlags.None, false), 0) != 0 && Sandbox.Game.Multiplayer.Sync.IsServer && this.UseConveyorSystem)
          this.CubeGrid.GridSystems.ConveyorSystem.PullItem(myDefinitionId, new MyFixedPoint?((MyFixedPoint) missingComponent.Value), (IMyConveyorEndpointBlock) this, MyEntityExtensions.GetInventory(this), false, false);
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        float num1 = 0.25f / (float) Math.Min(4, count > 0 ? count : 1);
        foreach (MySlimBlock target in targets)
        {
          if (target.CubeGrid.Physics != null && target.CubeGrid.Physics.Enabled && target != this.SlimBlock)
          {
            float num2 = MySession.Static.WelderSpeedMultiplier * MyShipWelder.WELDER_AMOUNT_PER_SECOND * num1;
            bool? nullable = target.ComponentStack.WillFunctionalityRise(num2);
            if (!nullable.HasValue || !nullable.Value || MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, target.BlockDefinition.BlockPairName, target.BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST))
            {
              if (target.CanContinueBuild((MyInventoryBase) inventory))
                flag = true;
              target.MoveItemsToConstructionStockpile((MyInventoryBase) inventory);
              target.MoveUnneededItemsFromConstructionStockpile((MyInventoryBase) inventory);
              if (target.HasDeformation || (double) target.MaxDeformation > 9.99999974737875E-05 || !target.IsFullIntegrity)
              {
                float maxAllowedBoneMovement = (float) ((double) MyShipWelder.WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED * 250.0 * (1.0 / 1000.0));
                target.IncreaseMountLevel(num2, this.OwnerId, (MyInventoryBase) inventory, maxAllowedBoneMovement, (bool) this.m_helpOthers, this.IDModule.ShareMode);
              }
            }
          }
        }
      }
      else
      {
        foreach (MySlimBlock target in targets)
        {
          if (target != this.SlimBlock && target.CanContinueBuild((MyInventoryBase) inventory))
            flag = true;
        }
      }
      this.m_missingComponents.Clear();
      if (!flag && Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MyWelder.ProjectionRaycastData[] projectedBlocks = this.FindProjectedBlocks();
        if (this.UseConveyorSystem)
        {
          foreach (MyWelder.ProjectionRaycastData projectionRaycastData in projectedBlocks)
          {
            MyCubeBlockDefinition.Component[] components = projectionRaycastData.hitCube.BlockDefinition.Components;
            if (components != null && components.Length != 0)
              this.CubeGrid.GridSystems.ConveyorSystem.PullItem(components[0].Definition.Id, new MyFixedPoint?((MyFixedPoint) 1), (IMyConveyorEndpointBlock) this, inventory, false, false);
          }
        }
        HashSet<MyCubeGrid.MyBlockLocation> myBlockLocationSet = new HashSet<MyCubeGrid.MyBlockLocation>();
        bool creativeMode = MySession.Static.CreativeMode;
        MyPlayer.PlayerId result;
        if (MySession.Static.Players.TryGetPlayerId(this.BuiltBy, out result) && MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
          creativeMode |= MySession.Static.CreativeToolsEnabled(Sandbox.Game.Multiplayer.Sync.MyId);
        foreach (MyWelder.ProjectionRaycastData projectionRaycastData in projectedBlocks)
        {
          if (this.IsWithinWorldLimits(projectionRaycastData.cubeProjector, projectionRaycastData.hitCube.BlockDefinition.BlockPairName, creativeMode ? projectionRaycastData.hitCube.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST) && (MySession.Static.CreativeMode || inventory.ContainItems(new MyFixedPoint?((MyFixedPoint) 1), projectionRaycastData.hitCube.BlockDefinition.Components[0].Definition.Id)))
          {
            MyWelder.ProjectionRaycastData invokedBlock = projectionRaycastData;
            MySandboxGame.Static.Invoke((Action) (() =>
            {
              if (invokedBlock.cubeProjector.Closed || invokedBlock.cubeProjector.CubeGrid.Closed || invokedBlock.hitCube.FatBlock != null && invokedBlock.hitCube.FatBlock.Closed)
                return;
              invokedBlock.cubeProjector.Build(invokedBlock.hitCube, this.OwnerId, this.EntityId, builtBy: this.BuiltBy);
            }), "ShipWelder BuildProjection");
            flag = true;
          }
        }
      }
      if (flag)
        this.SetBuildingMusic(150);
      return flag;
    }

    private MyWelder.ProjectionRaycastData[] FindProjectedBlocks()
    {
      BoundingSphereD boundingSphereD = new BoundingSphereD(Vector3D.Transform(this.m_detectorSphere.Center, this.CubeGrid.WorldMatrix), (double) this.m_detectorSphere.Radius);
      List<MyWelder.ProjectionRaycastData> projectionRaycastDataList = new List<MyWelder.ProjectionRaycastData>();
      List<MyEntity> entitiesInSphere = Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(ref boundingSphereD);
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity is MyCubeGrid myCubeGrid && myCubeGrid.Projector != null)
        {
          myCubeGrid.GetBlocksInsideSphere(ref boundingSphereD, this.m_projectedBlock);
          foreach (MySlimBlock projectedBlock in this.m_projectedBlock)
          {
            if (myCubeGrid.Projector.CanBuild(projectedBlock, true) == BuildCheckResult.OK)
            {
              MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(projectedBlock.Position);
              if (cubeBlock != null)
                projectionRaycastDataList.Add(new MyWelder.ProjectionRaycastData(BuildCheckResult.OK, cubeBlock, myCubeGrid.Projector));
            }
          }
          this.m_projectedBlock.Clear();
        }
      }
      this.m_projectedBlock.Clear();
      entitiesInSphere.Clear();
      return projectionRaycastDataList.ToArray();
    }

    protected override void StartShooting()
    {
      base.StartShooting();
      this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
    }

    protected override void StopShooting()
    {
      base.StopShooting();
      this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);
    }

    public override void EndShoot(MyShootActionEnum action)
    {
      if (action == MyShootActionEnum.SecondaryAction && MySession.Static.ControlledEntity != null && (!Sandbox.Game.Multiplayer.Sync.IsDedicated && this.GetTopMostParent((Type) null) == MySession.Static.ControlledEntity.Entity.GetTopMostParent((Type) null)) && this.CubeGrid.GridSystems.ControlSystem.GetShipController() != null)
      {
        MySlimBlock block = this.CubeGrid.GridSystems.ControlSystem.GetShipController().RaycasterHitBlock;
        if (block == null)
        {
          MyWelder.ProjectionRaycastData projectedBlock = this.CubeGrid.GridSystems.ControlSystem.GetShipController().FindProjectedBlock();
          if (projectedBlock.raycastResult == BuildCheckResult.OK)
            block = projectedBlock.hitCube;
        }
        MyWelder.AddMissingComponentsToBuildPlanner(block);
      }
      base.EndShoot(action);
    }

    public override bool SetEmissiveStateDamaged() => this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);

    public override bool SetEmissiveStateDisabled() => this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);

    public override bool SetEmissiveStateWorking() => this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);

    protected override void StartEffects()
    {
      Vector3D translation = this.WorldMatrix.Translation;
      Matrix matrix = this.m_particleDummyMatrix1 * this.PositionComp.LocalMatrixRef;
      MatrixD effectMatrix = (MatrixD) ref matrix;
      MyParticlesManager.TryCreateParticleEffect("ShipWelderArc", ref effectMatrix, ref translation, this.Render.ParentIDs[0], out this.m_particleEffect);
    }

    protected override void StopEffects()
    {
      if (this.m_particleEffect == null)
        return;
      this.m_particleEffect.Stop();
      this.m_particleEffect = (MyParticleEffect) null;
    }

    private Vector3 GetLightPosition()
    {
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      worldMatrix = this.WorldMatrix;
      Vector3D vector3D = worldMatrix.Forward * (this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.70000004768372 : 1.5);
      return (Vector3) (translation + vector3D);
    }

    protected override void StopLoopSound()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    protected override void PlayLoopSound(bool activated)
    {
      if (this.m_soundEmitter == null)
        return;
      if (activated)
        this.m_soundEmitter.PlaySingleSound(MyShipWelder.METAL_SOUND, true);
      else
        this.m_soundEmitter.PlaySingleSound(MyShipWelder.IDLE_SOUND, true);
    }

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Welding, shooter))
        return base.CanShoot(action, shooter, out status);
      status = MyGunStatusEnum.Failed;
      return false;
    }

    public override PullInformation GetPullInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty constraint")
    };

    public override PullInformation GetPushInformation() => (PullInformation) null;

    protected class m_helpOthers\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipWelder) obj0).m_helpOthers = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
