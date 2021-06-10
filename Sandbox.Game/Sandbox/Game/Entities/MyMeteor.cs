// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyMeteor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_Meteor), true)]
  public class MyMeteor : MyEntity, IMyDestroyableObject, IMyDecalProxy, IMyMeteor, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEventProxy, IMyEventOwner
  {
    private static readonly int MAX_TRAJECTORY_LENGTH = 10000;
    private static readonly int SPEED = 90;
    private MyMeteor.MyMeteorGameLogic m_logic;
    private bool m_hasModifiableDamage;

    public MyMeteor.MyMeteorGameLogic GameLogic
    {
      get => this.m_logic;
      set => this.GameLogic = (MyGameLogicComponent) value;
    }

    public MyMeteor()
    {
      this.Components.ComponentAdded += new Action<System.Type, MyEntityComponentBase>(this.Components_ComponentAdded);
      this.GameLogic = new MyMeteor.MyMeteorGameLogic();
      this.Render = (MyRenderComponentBase) new MyRenderComponentDebrisVoxel();
    }

    private void Components_ComponentAdded(System.Type arg1, MyComponentBase arg2)
    {
      if (!(arg1 == typeof (MyGameLogicComponent)))
        return;
      this.m_logic = arg2 as MyMeteor.MyMeteorGameLogic;
    }

    public static MyEntity SpawnRandom(Vector3D position, Vector3 direction)
    {
      string materialName = MyMeteor.GetMaterialName();
      MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem(500 * (MyFixedPoint) MyUtils.GetRandomFloat(1f, 3f), (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(materialName));
      return MyMeteor.Spawn(ref physicalInventoryItem, position, direction * (float) MyMeteor.SPEED);
    }

    private static string GetMaterialName()
    {
      string str = "Stone";
      bool flag = false;
      MyVoxelMaterialDefinition materialDefinition1 = (MyVoxelMaterialDefinition) null;
      foreach (MyVoxelMaterialDefinition materialDefinition2 in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
      {
        if (materialDefinition2.MinedOre == str)
        {
          flag = true;
          break;
        }
        materialDefinition1 = materialDefinition2;
      }
      if (!flag && materialDefinition1 != null)
        str = materialDefinition1.MinedOre;
      return str;
    }

    public static MyEntity Spawn(
      ref MyPhysicalInventoryItem item,
      Vector3D position,
      Vector3 speed)
    {
      return MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) MyMeteor.PrepareBuilder(ref item), true, (Action<MyEntity>) (x => MyMeteor.SetSpawnSettings(x, position, speed)));
    }

    private static void SetSpawnSettings(MyEntity meteorEntity, Vector3D position, Vector3 speed)
    {
      Vector3 vector3 = -MySector.DirectionToSunNormalized;
      Vector3 vector3Normalized = MyUtils.GetRandomVector3Normalized();
      while (vector3 == vector3Normalized)
        vector3Normalized = MyUtils.GetRandomVector3Normalized();
      Vector3 up = Vector3.Cross(Vector3.Cross(vector3, vector3Normalized), vector3);
      meteorEntity.WorldMatrix = MatrixD.CreateWorld(position, vector3, up);
      meteorEntity.Physics.RigidBody.MaxLinearVelocity = 500f;
      meteorEntity.Physics.LinearVelocity = speed;
      meteorEntity.Physics.AngularVelocity = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(1.5f, 3f);
    }

    private static MyObjectBuilder_Meteor PrepareBuilder(
      ref MyPhysicalInventoryItem item)
    {
      MyObjectBuilder_Meteor newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Meteor>();
      newObject.Item = item.GetObjectBuilder();
      newObject.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      return newObject;
    }

    public override bool IsCCDForProjectiles => true;

    public void OnDestroy() => this.GameLogic.OnDestroy();

    public bool DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      this.GameLogic.DoDamage(damage, damageType, sync, hitInfo, attackerId);
      return true;
    }

    void IMyDecalProxy.AddDecals(
      ref MyHitInfo hitInfo,
      MyStringHash source,
      Vector3 forwardDirection,
      object customdata,
      IMyDecalHandler decalHandler,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial,
      bool isTrail)
    {
    }

    public float Integrity => this.GameLogic.Integrity;

    public bool UseDamageSystem => this.m_hasModifiableDamage;

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false) => this.GameLogic.GetObjectBuilder(false);

    public class MyMeteorGameLogic : MyEntityGameLogic
    {
      private const int VISIBLE_RANGE_MAX_DISTANCE_SQUARED = 9000000;
      public MyPhysicalInventoryItem Item;
      private StringBuilder m_textCache;
      private float m_integrity = 100f;
      private string[] m_particleEffectNames = new string[2];
      private MyParticleEffect m_dustEffect;
      private int m_timeCreated;
      private Vector3 m_particleVectorForward = Vector3.Zero;
      private Vector3 m_particleVectorUp = Vector3.Zero;
      private MyMeteor.MyMeteorGameLogic.MeteorStatus m_meteorStatus = MyMeteor.MyMeteorGameLogic.MeteorStatus.InSpace;
      private MyEntity3DSoundEmitter m_soundEmitter;
      private bool m_closeAfterSimulation;
      private MySoundPair m_meteorFly = new MySoundPair("MeteorFly");
      private MySoundPair m_meteorExplosion = new MySoundPair("MeteorExplosion");

      internal MyMeteor Entity => this.Container == null ? (MyMeteor) null : this.Container.Entity as MyMeteor;

      public MyVoxelMaterialDefinition VoxelMaterial { get; set; }

      private bool InParticleVisibleRange
      {
        get
        {
          MyMeteor entity = this.Entity;
          if (entity != null)
            return (MySector.MainCamera.Position - entity.WorldMatrix.Translation).LengthSquared() < 9000000.0;
          MyLog.Default.WriteLine("Error: MyMeteor.Container should not be null!");
          return false;
        }
      }

      public MyMeteorGameLogic() => this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) null);

      public override void Init(MyObjectBuilder_EntityBase objectBuilder)
      {
        this.Entity.SyncFlag = true;
        base.Init(objectBuilder);
        MyObjectBuilder_Meteor objectBuilderMeteor = (MyObjectBuilder_Meteor) objectBuilder;
        this.Item = new MyPhysicalInventoryItem(objectBuilderMeteor.Item);
        this.m_particleEffectNames[0] = "Meteory_Fire_Atmosphere";
        this.m_particleEffectNames[1] = "Meteory_Fire_Space";
        this.InitInternal();
        this.Entity.Physics.LinearVelocity = objectBuilderMeteor.LinearVelocity;
        this.Entity.Physics.AngularVelocity = objectBuilderMeteor.AngularVelocity;
        this.m_integrity = objectBuilderMeteor.Integrity;
      }

      private void InitInternal()
      {
        MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) this.Item.Content);
        MyObjectBuilder_Ore content = this.Item.Content as MyObjectBuilder_Ore;
        string model = physicalItemDefinition.Model;
        float num = 1f;
        this.VoxelMaterial = (MyVoxelMaterialDefinition) null;
        if (content != null)
        {
          foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
          {
            if (materialDefinition.MinedOre == content.SubtypeName)
            {
              this.VoxelMaterial = materialDefinition;
              model = MyDebris.GetAmountBasedDebrisVoxel((float) this.Item.Amount);
              num = (float) Math.Pow((double) (float) this.Item.Amount * (double) physicalItemDefinition.Volume / (double) MyDebris.VoxelDebrisModelVolume, 0.333000004291534);
              break;
            }
          }
        }
        if ((double) num < 0.150000005960464)
          num = 0.15f;
        MyRenderComponentDebrisVoxel render = this.Entity.Render as MyRenderComponentDebrisVoxel;
        render.VoxelMaterialIndex = this.VoxelMaterial.Index;
        render.TexCoordOffset = 5f;
        render.TexCoordScale = 8f;
        this.Entity.Init(new StringBuilder("Meteor"), model, (MyEntity) null, new float?());
        this.Entity.PositionComp.Scale = new float?(num);
        HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(this.Entity.PositionComp.LocalVolume.Radius, (float) (4.18879032962207 * Math.Pow((double) this.Entity.PositionComp.LocalVolume.Radius, 3.0)) * 3.7f);
        HkSphereShape hkSphereShape = new HkSphereShape(this.Entity.PositionComp.LocalVolume.Radius);
        if (this.Entity.Physics != null)
          this.Entity.Physics.Close();
        this.Entity.Physics = (MyPhysicsComponentBase) new MyPhysicsBody((VRage.ModAPI.IMyEntity) this.Entity, RigidBodyFlag.RBF_BULLET);
        this.Entity.Physics.ReportAllContacts = true;
        this.Entity.GetPhysicsBody().CreateFromCollisionObject((HkShape) hkSphereShape, Vector3.Zero, MatrixD.Identity, new HkMassProperties?(volumeMassProperties));
        this.Entity.Physics.Enabled = true;
        this.Entity.Physics.RigidBody.ContactPointCallbackEnabled = true;
        hkSphereShape.Base.RemoveReference();
        this.Entity.Physics.PlayCollisionCueEnabled = true;
        this.m_timeCreated = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
        this.StartLoopSound();
      }

      public override void OnAddedToScene()
      {
        base.OnAddedToScene();
        this.Entity.GetPhysicsBody().ContactPointCallback += new MyPhysicsBody.PhysicsContactHandler(this.RigidBody_ContactPointCallback);
      }

      public override MyObjectBuilder_EntityBase GetObjectBuilder(
        bool copy = false)
      {
        MyObjectBuilder_Meteor objectBuilder = (MyObjectBuilder_Meteor) base.GetObjectBuilder(copy);
        if (this.Entity == null || this.Entity.Physics == null)
        {
          objectBuilder.LinearVelocity = Vector3.One * 10f;
          objectBuilder.AngularVelocity = Vector3.Zero;
        }
        else
        {
          objectBuilder.LinearVelocity = this.Entity.Physics.LinearVelocity;
          objectBuilder.AngularVelocity = this.Entity.Physics.AngularVelocity;
        }
        if (this.GameLogic != null)
        {
          objectBuilder.Item = this.Item.GetObjectBuilder();
          objectBuilder.Integrity = this.Integrity;
        }
        return (MyObjectBuilder_EntityBase) objectBuilder;
      }

      public override void OnAddedToContainer()
      {
        base.OnAddedToContainer();
        this.m_soundEmitter.Entity = this.Container.Entity as MyEntity;
      }

      public override void MarkForClose()
      {
        this.DestroyMeteor();
        base.MarkForClose();
      }

      public override void UpdateBeforeSimulation()
      {
        base.UpdateBeforeSimulation();
        if (this.m_dustEffect == null)
          return;
        this.UpdateParticlePosition();
      }

      public override void UpdateAfterSimulation()
      {
        if (this.m_closeAfterSimulation)
        {
          this.CloseMeteorInternal();
          this.m_closeAfterSimulation = false;
        }
        base.UpdateAfterSimulation();
      }

      private MatrixD GetParticlePosition() => this.m_particleVectorUp != Vector3.Zero ? MatrixD.CreateWorld(this.Entity.WorldMatrix.Translation, this.m_particleVectorForward, this.m_particleVectorUp) : this.Entity.WorldMatrix;

      private void UpdateParticlePosition()
      {
        if (this.m_particleVectorUp != Vector3.Zero)
          this.m_dustEffect.WorldMatrix = this.GetParticlePosition();
        else
          this.m_dustEffect.Stop();
      }

      public override void UpdateBeforeSimulation100()
      {
        base.UpdateBeforeSimulation100();
        if (this.m_particleVectorUp == Vector3.Zero)
        {
          this.m_particleVectorUp = !(this.Entity.Physics.LinearVelocity != Vector3.Zero) ? Vector3.Up : -Vector3.Normalize(this.Entity.Physics.LinearVelocity);
          this.m_particleVectorUp.CalculatePerpendicularVector(out this.m_particleVectorForward);
        }
        Vector3D position = this.Entity.PositionComp.GetPosition();
        MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
        int meteorStatus1 = (int) this.m_meteorStatus;
        this.m_meteorStatus = closestPlanet == null || !closestPlanet.HasAtmosphere || (double) closestPlanet.GetAirDensity(position) <= 0.5 ? MyMeteor.MyMeteorGameLogic.MeteorStatus.InSpace : MyMeteor.MyMeteorGameLogic.MeteorStatus.InAtmosphere;
        int meteorStatus2 = (int) this.m_meteorStatus;
        if (meteorStatus1 != meteorStatus2 && this.m_dustEffect != null)
        {
          this.m_dustEffect.Stop();
          this.m_dustEffect = (MyParticleEffect) null;
        }
        if (this.m_dustEffect != null && !this.InParticleVisibleRange)
        {
          this.m_dustEffect.Stop();
          this.m_dustEffect = (MyParticleEffect) null;
        }
        if (this.m_dustEffect == null && this.InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect(this.m_particleEffectNames[(int) this.m_meteorStatus], MatrixD.CreateWorld(this.Entity.WorldMatrix.Translation, this.m_particleVectorForward, this.m_particleVectorUp), out this.m_dustEffect))
        {
          this.UpdateParticlePosition();
          this.m_dustEffect.UserScale = this.Entity.PositionComp.Scale.Value;
        }
        this.m_soundEmitter.Update();
        if (!Sync.IsServer || (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_timeCreated) <= (double) Math.Min((float) (MyMeteor.MAX_TRAJECTORY_LENGTH / MyMeteor.SPEED), (float) MyMeteor.MAX_TRAJECTORY_LENGTH / this.Entity.Physics.LinearVelocity.Length()) * 1000.0)
          return;
        this.CloseMeteorInternal();
      }

      private void CloseMeteorInternal()
      {
        if (this.Entity.Physics != null)
        {
          this.Entity.Physics.Enabled = false;
          this.Entity.Physics.Deactivate();
        }
        this.MarkForClose();
      }

      public override void Close()
      {
        if (this.m_dustEffect != null)
        {
          this.m_dustEffect.Stop();
          this.m_dustEffect = (MyParticleEffect) null;
        }
        base.Close();
      }

      private void RigidBody_ContactPointCallback(ref MyPhysics.MyContactPointEvent value)
      {
        if (this.MarkedForClose || !this.Entity.Physics.Enabled || this.m_closeAfterSimulation)
          return;
        VRage.ModAPI.IMyEntity other = value.ContactPointEvent.GetOtherEntity((VRage.ModAPI.IMyEntity) this.Entity);
        MyMeteor.MyMeteorGameLogic.ContactProperties contactProperties = new MyMeteor.MyMeteorGameLogic.ContactProperties();
        contactProperties.Position = value.Position;
        ref MyMeteor.MyMeteorGameLogic.ContactProperties local1 = ref contactProperties;
        HkCollisionEvent hkCollisionEvent;
        HkRigidBody hkRigidBody;
        if (!((HkReferenceObject) value.ContactPointEvent.Base.BodyA == (HkReferenceObject) this.Entity.Physics.RigidBody))
        {
          hkCollisionEvent = value.ContactPointEvent.Base;
          hkRigidBody = hkCollisionEvent.BodyB;
        }
        else
        {
          hkCollisionEvent = value.ContactPointEvent.Base;
          hkRigidBody = hkCollisionEvent.BodyA;
        }
        local1.CollidingBody = hkRigidBody;
        ref MyMeteor.MyMeteorGameLogic.ContactProperties local2 = ref contactProperties;
        hkCollisionEvent = value.ContactPointEvent.Base;
        double num = (HkReferenceObject) hkCollisionEvent.BodyA == (HkReferenceObject) this.Entity.Physics.RigidBody ? -1.0 : 1.0;
        local2.Direction = (float) num;
        contactProperties.Normal = value.Normal;
        contactProperties.SeparatingVelocity = value.ContactPointEvent.SeparatingVelocity;
        MyMeteor.MyMeteorGameLogic.ContactProperties props = contactProperties;
        MyEntities.InvokeLater((Action) (() => this.ProcessCollision(props, other)));
      }

      private void ProcessCollision(
        MyMeteor.MyMeteorGameLogic.ContactProperties properties,
        VRage.ModAPI.IMyEntity other)
      {
        if (!MySessionComponentSafeZones.IsActionAllowed(properties.Position, MySafeZoneAction.Damage))
        {
          this.m_closeAfterSimulation = Sync.IsServer;
        }
        else
        {
          if (Sync.IsServer)
          {
            switch (other)
            {
              case MyCubeGrid _:
                MyCubeGrid grid = other as MyCubeGrid;
                if (grid.BlocksDestructionEnabled)
                {
                  this.DestroyGrid(in properties, grid);
                  break;
                }
                break;
              case MyCharacter _:
                (other as MyCharacter).DoDamage(50f * this.Entity.PositionComp.Scale.Value, MyDamageType.Environment, true, this.Entity.EntityId);
                break;
              case MyFloatingObject _:
                (other as MyFloatingObject).DoDamage(100f * this.Entity.PositionComp.Scale.Value, MyDamageType.Deformation, true, this.Entity.EntityId);
                break;
              case MyMeteor _:
                this.m_closeAfterSimulation = true;
                (other.GameLogic as MyMeteor.MyMeteorGameLogic).m_closeAfterSimulation = true;
                break;
            }
            this.m_closeAfterSimulation = true;
          }
          if (!(other is MyVoxelBase))
            return;
          this.CreateCrater(in properties, other as MyVoxelBase);
        }
      }

      private void DestroyMeteor()
      {
        MyParticleEffect effect;
        if (this.InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect("Meteorit_Smoke1AfterHit", this.GetParticlePosition(), out effect))
          effect.UserScale = MyUtils.GetRandomFloat(0.8f, 1.2f);
        if (this.m_dustEffect != null)
        {
          this.m_dustEffect.StopEmitting(10f);
          this.m_dustEffect.StopLights();
          this.m_dustEffect = (MyParticleEffect) null;
        }
        this.PlayExplosionSound();
      }

      private void CreateCrater(
        in MyMeteor.MyMeteorGameLogic.ContactProperties props,
        MyVoxelBase voxel)
      {
        if ((double) Math.Abs(Vector3.Normalize(-this.Entity.WorldMatrix.Forward).Dot(props.Normal)) < 0.1)
        {
          MyParticleEffect effect;
          if (this.InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect("Meteorit_Smoke1AfterHit", this.Entity.WorldMatrix, out effect))
            effect.UserScale = (float) this.Entity.PositionComp.WorldVolume.Radius * 2f;
          this.m_particleVectorUp = Vector3.Zero;
          this.m_closeAfterSimulation = Sync.IsServer;
        }
        else
        {
          if (Sync.IsServer)
          {
            float radius = this.Entity.PositionComp.Scale.Value * 5f;
            BoundingSphereD sphere = (BoundingSphereD) new BoundingSphere((Vector3) props.Position, radius);
            Vector3 vector3 = (double) props.SeparatingVelocity >= 0.0 ? Vector3.Normalize(Vector3.Reflect(this.Entity.Physics.LinearVelocity, props.Normal)) : Vector3.Normalize(this.Entity.Physics.LinearVelocity);
            MyVoxelMaterialDefinition material = this.VoxelMaterial;
            int num = MyDefinitionManager.Static.GetVoxelMaterialDefinitions().Count<MyVoxelMaterialDefinition>() * 2;
            for (; !material.IsRare || !material.SpawnsFromMeteorites || (material.MinVersion > MySession.Static.Settings.VoxelGeneratorVersion || material.MaxVersion < MySession.Static.Settings.VoxelGeneratorVersion); material = MyDefinitionManager.Static.GetVoxelMaterialDefinitions().ElementAt<MyVoxelMaterialDefinition>(MyUtils.GetRandomInt(MyDefinitionManager.Static.GetVoxelMaterialDefinitions().Count<MyVoxelMaterialDefinition>() - 1)))
            {
              if (--num < 0)
              {
                material = this.VoxelMaterial;
                break;
              }
            }
            voxel.CreateVoxelMeteorCrater(sphere.Center, (float) sphere.Radius, -vector3, material);
            MyVoxelGenerator.MakeCrater(voxel, sphere, -vector3, material);
          }
          this.m_soundEmitter.Entity = (MyEntity) voxel;
          this.m_soundEmitter.SetPosition(new Vector3D?(this.Entity.PositionComp.GetPosition()));
          this.m_closeAfterSimulation = Sync.IsServer;
        }
      }

      private void DestroyGrid(
        in MyMeteor.MyMeteorGameLogic.ContactProperties value,
        MyCubeGrid grid)
      {
        this.m_soundEmitter.Entity = (MyEntity) grid;
        this.m_soundEmitter.SetPosition(new Vector3D?(this.Entity.PositionComp.GetPosition()));
        grid.Physics.PerformMeteoriteDeformation(in value);
        this.m_closeAfterSimulation = Sync.IsServer;
      }

      private void StartLoopSound() => this.m_soundEmitter.PlaySingleSound(this.m_meteorFly);

      private void StopLoopSound() => this.m_soundEmitter.StopSound(true);

      private void PlayExplosionSound()
      {
        this.m_soundEmitter.SetVelocity(new Vector3?(Vector3.Zero));
        this.m_soundEmitter.SetPosition(new Vector3D?(this.Entity.PositionComp.GetPosition()));
        this.m_soundEmitter.PlaySingleSound(this.m_meteorExplosion, true);
      }

      public void DoDamage(
        float damage,
        MyStringHash damageType,
        bool sync,
        MyHitInfo? hitInfo,
        long attackerId)
      {
        if (sync)
        {
          if (!Sync.IsServer)
            return;
          MySyncDamage.DoDamageSynced((MyEntity) this.Entity, damage, damageType, attackerId);
        }
        else
        {
          MyDamageInformation info = new MyDamageInformation(false, damage, damageType, attackerId);
          if (this.Entity == null)
            return;
          if (this.Entity.UseDamageSystem)
            MyDamageSystem.Static.RaiseBeforeDamageApplied((object) this.Entity, ref info);
          this.m_integrity -= info.Amount;
          if (this.Entity.UseDamageSystem)
            MyDamageSystem.Static.RaiseAfterDamageApplied((object) this.Entity, info);
          if ((double) this.m_integrity > 0.0 || !Sync.IsServer)
            return;
          this.m_closeAfterSimulation = Sync.IsServer;
          if (!this.Entity.UseDamageSystem)
            return;
          MyDamageSystem.Static.RaiseDestroyed((object) this.Entity, info);
        }
      }

      public void OnDestroy()
      {
      }

      public float Integrity => this.m_integrity;

      protected virtual HkShape GetPhysicsShape(
        HkMassProperties massProperties,
        float mass,
        float scale)
      {
        Vector3 halfExtents = (this.Entity.Render.GetModel().BoundingBox.Max - this.Entity.Render.GetModel().BoundingBox.Min) / 2f;
        massProperties = this.VoxelMaterial == null ? HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(halfExtents, mass) : HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(this.Entity.Render.GetModel().BoundingSphere.Radius * scale, mass);
        return MyDebris.Static.GetDebrisShape(this.Entity.Render.GetModel(), HkShapeType.ConvexVertices, scale);
      }

      private enum MeteorStatus
      {
        InAtmosphere,
        InSpace,
      }

      public struct ContactProperties
      {
        public HkRigidBody CollidingBody;
        public Vector3D Position;
        public Vector3 Normal;
        public float Direction;
        public float SeparatingVelocity;
      }

      private class Sandbox_Game_Entities_MyMeteor\u003C\u003EMyMeteorGameLogic\u003C\u003EActor : IActivator, IActivator<MyMeteor.MyMeteorGameLogic>
      {
        object IActivator.CreateInstance() => (object) new MyMeteor.MyMeteorGameLogic();

        MyMeteor.MyMeteorGameLogic IActivator<MyMeteor.MyMeteorGameLogic>.CreateInstance() => new MyMeteor.MyMeteorGameLogic();
      }
    }

    private class Sandbox_Game_Entities_MyMeteor\u003C\u003EActor : IActivator, IActivator<MyMeteor>
    {
      object IActivator.CreateInstance() => (object) new MyMeteor();

      MyMeteor IActivator<MyMeteor>.CreateInstance() => new MyMeteor();
    }
  }
}
