// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyShipGrinder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ShipGrinder))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyShipGrinder), typeof (Sandbox.ModAPI.Ingame.IMyShipGrinder)})]
  public class MyShipGrinder : MyShipToolBase, Sandbox.ModAPI.IMyShipGrinder, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyShipGrinder
  {
    private static MySoundPair IDLE_SOUND = new MySoundPair("ToolPlayGrindIdle");
    private static MySoundPair METAL_SOUND = new MySoundPair("ToolPlayGrindMetal");
    private const string PARTICLE_EFFECT = "ShipGrinder";
    private static string[] BLADE_SUBPART_IDs = new string[2]
    {
      "grinder1",
      "grinder2"
    };
    private MyParticleEffect m_particleEffect1;
    private MyParticleEffect m_particleEffect2;
    private MyFlareDefinition m_flare;
    private MyShipGrinderDefinition m_grinderDef;
    private const float RANDOM_IMPULSE_SCALE = 500f;
    private static List<MyPhysicalInventoryItem> m_tmpItemList = new List<MyPhysicalInventoryItem>();
    private bool m_wantsToShake;
    private MyCubeGrid m_otherGrid;
    private Matrix m_particleDummyMatrix1;
    private Matrix m_particleDummyMatrix2;
    private List<MyShipGrinder.MyShipGrinderSubpart> m_bladeSubparts = new List<MyShipGrinder.MyShipGrinderSubpart>();

    public override void InitComponents()
    {
      this.Render = (MyRenderComponentBase) new MyRenderComponentShipGrinder();
      base.InitComponents();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      if (this.CubeGrid.GridSizeEnum == MyCubeSize.Large)
      {
        MyShipGrinder.IDLE_SOUND.Init("ToolLrgGrindIdle");
        MyShipGrinder.METAL_SOUND.Init("ToolLrgGrindMetal");
      }
      this.m_grinderDef = this.BlockDefinition as MyShipGrinderDefinition;
      if (this.m_grinderDef != null && this.m_grinderDef.Flare != "")
        this.m_flare = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), this.m_grinderDef.Flare)) as MyFlareDefinition;
      this.HeatUpFrames = 15;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.LoadParticleDummyMatrices();
    }

    private void LoadParticleDummyMatrices()
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        if (dummy.Key.ToLower().Contains("particles1"))
          this.m_particleDummyMatrix1 = dummy.Value.Matrix;
        else if (dummy.Key.ToLower().Contains("particles2"))
          this.m_particleDummyMatrix2 = dummy.Value.Matrix;
      }
    }

    public override void OnControlAcquired(MyCharacter owner)
    {
      base.OnControlAcquired(owner);
      if (owner == null || owner.Parent == null || (owner != MySession.Static.LocalCharacter || owner.Parent.Components.Contains(typeof (MyCasterComponent))))
        return;
      MyCasterComponent component = new MyCasterComponent((MyDrillSensorBase) new MyDrillSensorRayCast(0.0f, this.DEFAULT_REACH_DISTANCE, (MyDefinitionBase) this.BlockDefinition));
      owner.Parent.Components.Add<MyCasterComponent>(component);
      this.m_controller = owner;
    }

    public override void OnControlReleased()
    {
      base.OnControlReleased();
      if (this.m_controller == null || this.m_controller.Parent == null || (this.m_controller != MySession.Static.LocalCharacter || !this.m_controller.Parent.Components.Contains(typeof (MyCasterComponent))))
        return;
      this.m_controller.Parent.Components.Remove(typeof (MyCasterComponent));
    }

    protected override bool Activate(HashSet<MySlimBlock> targets)
    {
      int count = targets.Count;
      this.m_otherGrid = (MyCubeGrid) null;
      if (targets.Count > 0)
        this.m_otherGrid = targets.FirstElement<MySlimBlock>().CubeGrid;
      float num = 0.25f / (float) Math.Min(4, targets.Count);
      foreach (MySlimBlock target in targets)
      {
        if (!target.CubeGrid.Immune)
        {
          this.m_otherGrid = target.CubeGrid;
          if ((this.m_otherGrid.Physics == null ? 1 : (!this.m_otherGrid.Physics.Enabled ? 1 : 0)) != 0)
          {
            --count;
          }
          else
          {
            MyCubeBlockDefinition.PreloadConstructionModels(target.BlockDefinition);
            if (Sync.IsServer)
            {
              MyDamageInformation info = new MyDamageInformation(false, MySession.Static.GrinderSpeedMultiplier * 4f * num, MyDamageType.Grind, this.EntityId);
              if (target.UseDamageSystem)
                MyDamageSystem.Static.RaiseBeforeDamageApplied((object) target, ref info);
              if (target.CubeGrid.Editable)
              {
                target.DecreaseMountLevel(info.Amount, (MyInventoryBase) MyEntityExtensions.GetInventory(this), identityId: this.OwnerId);
                target.MoveItemsFromConstructionStockpile((MyInventoryBase) MyEntityExtensions.GetInventory(this));
              }
              if (target.UseDamageSystem)
                MyDamageSystem.Static.RaiseAfterDamageApplied((object) target, info);
              if (target.IsFullyDismounted)
              {
                if (target.FatBlock != null && target.FatBlock.HasInventory)
                  this.EmptyBlockInventories(target.FatBlock);
                if (target.UseDamageSystem)
                  MyDamageSystem.Static.RaiseDestroyed((object) target, info);
                target.SpawnConstructionStockpile();
                target.CubeGrid.RazeBlock(target.Min, 0UL);
              }
            }
            if (count > 0)
              this.SetBuildingMusic(200);
          }
        }
      }
      this.m_wantsToShake = (uint) count > 0U;
      return (uint) count > 0U;
    }

    private void EmptyBlockInventories(MyCubeBlock block)
    {
      for (int index = 0; index < block.InventoryCount; ++index)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(block, index);
        if (!inventory.Empty())
        {
          MyShipGrinder.m_tmpItemList.Clear();
          MyShipGrinder.m_tmpItemList.AddRange((IEnumerable<MyPhysicalInventoryItem>) inventory.GetItems());
          foreach (MyPhysicalInventoryItem tmpItem in MyShipGrinder.m_tmpItemList)
            MyInventory.Transfer(inventory, MyEntityExtensions.GetInventory(this), tmpItem.ItemId);
        }
      }
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_wantsToShake && this.m_otherGrid != null && (this.m_otherGrid.Physics != null && !this.m_otherGrid.Physics.IsStatic) && (MySession.Static.EnableToolShake && MyFakes.ENABLE_TOOL_SHAKE))
      {
        Vector3 randomVector3 = MyUtils.GetRandomVector3();
        this.ApplyImpulse(this.m_otherGrid, randomVector3);
        if (this.CubeGrid.Physics != null && !this.CubeGrid.Physics.IsStatic)
          this.ApplyImpulse(this.CubeGrid, randomVector3);
      }
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeActivate < 250)
        return;
      this.m_wantsToShake = false;
      this.m_otherGrid = (MyCubeGrid) null;
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!Sync.IsServer || !this.IsFunctional || !this.UseConveyorSystem)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory.GetItemsCount() <= 0)
        return;
      MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, inventory);
    }

    protected override void StartAnimation()
    {
      base.StartAnimation();
      foreach (MyShipGrinder.MyShipGrinderSubpart bladeSubpart in this.m_bladeSubparts)
        bladeSubpart.Render.UpdateBladeSpeed(15.70796f);
    }

    protected override void StopAnimation()
    {
      base.StopAnimation();
      foreach (MyShipGrinder.MyShipGrinderSubpart bladeSubpart in this.m_bladeSubparts)
        bladeSubpart.Render.UpdateBladeSpeed(0.0f);
    }

    protected override void StartEffects()
    {
      Vector3D translation = this.WorldMatrix.Translation;
      Matrix matrix1 = this.m_particleDummyMatrix1 * this.PositionComp.LocalMatrixRef;
      MatrixD effectMatrix1 = (MatrixD) ref matrix1;
      MyParticlesManager.TryCreateParticleEffect("ShipGrinder", ref effectMatrix1, ref translation, this.Render.ParentIDs[0], out this.m_particleEffect1);
      Matrix matrix2 = this.m_particleDummyMatrix2 * this.PositionComp.LocalMatrixRef;
      MatrixD effectMatrix2 = (MatrixD) ref matrix2;
      MyParticlesManager.TryCreateParticleEffect("ShipGrinder", ref effectMatrix2, ref translation, this.Render.ParentIDs[0], out this.m_particleEffect2);
    }

    protected override void StopEffects()
    {
      if (this.m_particleEffect1 != null)
      {
        this.m_particleEffect1.Stop();
        this.m_particleEffect1 = (MyParticleEffect) null;
      }
      if (this.m_particleEffect2 == null)
        return;
      this.m_particleEffect2.Stop();
      this.m_particleEffect2 = (MyParticleEffect) null;
    }

    protected override void StopLoopSound()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(false);
    }

    protected override void PlayLoopSound(bool activated)
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySingleSound(activated ? MyShipGrinder.METAL_SOUND : MyShipGrinder.IDLE_SOUND, true, this.m_soundEmitter.Sound != null && this.m_soundEmitter.Sound.IsPlaying);
    }

    private void ApplyImpulse(MyCubeGrid grid, Vector3 force)
    {
      MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer((MyEntity) grid);
      if ((!Sync.IsServer || controllingPlayer != null) && MySession.Static.LocalHumanPlayer != controllingPlayer || grid.Physics == null)
        return;
      grid.Physics.ApplyImpulse(force * this.CubeGrid.GridSize * 500f, this.PositionComp.GetPosition());
    }

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Grinding, shooter))
        return base.CanShoot(action, shooter, out status);
      status = MyGunStatusEnum.Failed;
      return false;
    }

    public override void RefreshModels(string model, string modelCollision)
    {
      this.m_bladeSubparts.Clear();
      base.RefreshModels(model, modelCollision);
    }

    protected override MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      if (!MyShipGrinder.BLADE_SUBPART_IDs.Contains<string>(data.Name))
        return base.InstantiateSubpart(subpartDummy, ref data);
      MyShipGrinder.MyShipGrinderSubpart shipGrinderSubpart = new MyShipGrinder.MyShipGrinderSubpart();
      this.m_bladeSubparts.Add(shipGrinderSubpart);
      return (MyEntitySubpart) shipGrinderSubpart;
    }

    public override PullInformation GetPullInformation() => (PullInformation) null;

    public override PullInformation GetPushInformation()
    {
      PullInformation pullInformation = new PullInformation()
      {
        Inventory = MyEntityExtensions.GetInventory(this),
        OwnerID = this.OwnerId
      };
      pullInformation.Constraint = pullInformation.Inventory.Constraint;
      return pullInformation;
    }

    private class MyShipGrinderSubpart : MyEntitySubpart
    {
      public MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade Render => (MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade) base.Render;

      public override void InitComponents()
      {
        this.Render = (MyRenderComponentBase) new MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade();
        base.InitComponents();
      }

      private class Sandbox_Game_Weapons_MyShipGrinder\u003C\u003EMyShipGrinderSubpart\u003C\u003EActor : IActivator, IActivator<MyShipGrinder.MyShipGrinderSubpart>
      {
        object IActivator.CreateInstance() => (object) new MyShipGrinder.MyShipGrinderSubpart();

        MyShipGrinder.MyShipGrinderSubpart IActivator<MyShipGrinder.MyShipGrinderSubpart>.CreateInstance() => new MyShipGrinder.MyShipGrinderSubpart();
      }
    }

    private class Sandbox_Game_Weapons_MyShipGrinder\u003C\u003EActor : IActivator, IActivator<MyShipGrinder>
    {
      object IActivator.CreateInstance() => (object) new MyShipGrinder();

      MyShipGrinder IActivator<MyShipGrinder>.CreateInstance() => new MyShipGrinder();
    }
  }
}
