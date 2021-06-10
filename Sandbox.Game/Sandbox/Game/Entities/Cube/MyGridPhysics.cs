// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridPhysics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyGridPhysics : MyPhysicsBody
  {
    private static readonly float LargeGridDeformationRatio = 1f;
    private static readonly float SmallGridDeformationRatio = 2.5f;
    private static readonly int MaxEffectsPerFrame = 3;
    public static readonly float LargeShipMaxAngularVelocityLimit = MathHelper.ToRadians(18000f);
    public static readonly float SmallShipMaxAngularVelocityLimit = MathHelper.ToRadians(36000f);
    private const float SPEED_OF_LIGHT_IN_VACUUM = 2.997924E+08f;
    public const float MAX_SHIP_SPEED = 1.498962E+08f;
    public int DisableGravity;
    private static readonly int SparksEffectDelayPerContactMs = 1000;
    public const int COLLISION_SPARK_LIMIT_COUNT = 3;
    public const int COLLISION_SPARK_LIMIT_TIME = 20;
    private List<ushort> m_tmpContactId = new List<ushort>();
    private MyConcurrentDictionary<ushort, int> m_lastContacts = new MyConcurrentDictionary<ushort, int>();
    private MyCubeGrid m_grid;
    private MyGridShape m_shape;
    private List<MyGridPhysics.ExplosionInfo> m_explosions = new List<MyGridPhysics.ExplosionInfo>();
    private const int MAX_NUM_CONTACTS_PER_FRAME = 10;
    private const int MAX_NUM_CONTACTS_PER_FRAME_SIMPLE_GRID = 1;
    private readonly MyGridPhysics.MyDirtyBlocksInfo m_dirtyCubesInfo = new MyGridPhysics.MyDirtyBlocksInfo();
    [ThreadStatic]
    private static List<Vector3I> m_tmpCubeList;
    private bool m_isClientPredicted;
    private ulong m_isClientPredictedLastFrameCheck;
    private bool m_isServer;
    public bool LowSimulationQuality;
    public float DeformationRatio;
    private Vector3 m_cachedGravity;
    public const float MAX = 100f;
    public const int PLANET_CRASH_MIN_BLOCK_COUNT = 5;
    public const int ACCUMULATION_TIME = 30;
    public const int CRASH_LIMIT = 90;
    private MyParticleEffect m_planetCrash_Effect;
    private Vector3D? m_planetCrash_CenterPoint;
    private Vector3? m_planetCrash_Normal;
    private bool m_planetCrash_IsStarted;
    private int m_planetCrash_TimeBetweenPoints;
    private int m_planetCrash_CrashAccumulation;
    private float m_planetCrash_ScaleCurrent = 1f;
    private float m_planetCrash_ScaleGoal = 1f;
    private float m_planetCrash_generationMultiplier;
    private HashSet<MySlimBlock> m_blocksInContact = new HashSet<MySlimBlock>();
    private List<MyPhysics.HitInfo> m_hitList = new List<MyPhysics.HitInfo>();
    private const float BREAK_OFFSET_MULTIPLIER = 0.7f;
    private static readonly MyConcurrentPool<HashSet<Vector3I>> m_dirtyCubesPool = new MyConcurrentPool<HashSet<Vector3I>>(10, (Action<HashSet<Vector3I>>) (x => x.Clear()));
    private static readonly MyConcurrentPool<Dictionary<MySlimBlock, float>> m_damagedCubesPool = new MyConcurrentPool<Dictionary<MySlimBlock, float>>(10, (Action<Dictionary<MySlimBlock, float>>) (x => x.Clear()));
    private MyConcurrentQueue<MyGridPhysics.GridEffect> m_gridEffects = new MyConcurrentQueue<MyGridPhysics.GridEffect>();
    private MyConcurrentQueue<MyGridPhysics.GridCollisionHit> m_gridCollisionEffects = new MyConcurrentQueue<MyGridPhysics.GridCollisionHit>();
    private List<MyGridPhysics.CollisionParticleEffect> m_collisionParticles = new List<MyGridPhysics.CollisionParticleEffect>();
    private int m_debrisPerFrame;
    private const int MaxDebrisPerFrame = 3;
    private static float IMP = 1E-07f;
    private static float ACCEL = 0.1f;
    private static float RATIO = 0.2f;
    public static float PREDICTION_IMPULSE_SCALE = 0.005f;
    private const int TOIs_THRESHOLD = 1;
    private const int FRAMES_TO_REMEMBER_TOI_IMPACT = 10;
    private int m_toiCountThisFrame;
    private int m_lastTOIFrame;
    private HkCollidableQualityType m_savedQuality = HkCollidableQualityType.Invalid;
    private bool m_appliedSlowdownThisFrame;
    private int m_removeBlocksCallbackScheduled;
    private ulong m_frameCollided;
    private ulong m_frameFirstImpact;
    private float m_impactDot;
    private readonly List<Vector3D> m_contactPosCache = new List<Vector3D>();
    private readonly ConcurrentDictionary<MySlimBlock, byte> m_removedCubes = new ConcurrentDictionary<MySlimBlock, byte>();
    private readonly Dictionary<MyEntity, bool> m_predictedContactEntities = new Dictionary<MyEntity, bool>();
    private static readonly List<MyEntity> m_predictedContactEntitiesToRemove = new List<MyEntity>();
    private List<HkdBreakableBodyInfo> m_newBodies = new List<HkdBreakableBodyInfo>();
    private List<HkdShapeInstanceInfo> m_children = new List<HkdShapeInstanceInfo>();
    private static List<HkdShapeInstanceInfo> m_tmpChildren_RemoveShapes = new List<HkdShapeInstanceInfo>();
    private static List<HkdShapeInstanceInfo> m_tmpChildren_CompoundIds = new List<HkdShapeInstanceInfo>();
    private static List<string> m_tmpShapeNames = new List<string>();
    private static HashSet<MySlimBlock> m_tmpBlocksToDelete = new HashSet<MySlimBlock>();
    private static HashSet<MySlimBlock> m_tmpBlocksUpdateDamage = new HashSet<MySlimBlock>();
    private static HashSet<ushort> m_tmpCompoundIds = new HashSet<ushort>();
    private static List<MyDefinitionId> m_tmpDefinitions = new List<MyDefinitionId>();
    private bool m_recreateBody;
    private Vector3 m_oldLinVel;
    private Vector3 m_oldAngVel;
    private List<HkdBreakableBody> m_newBreakableBodies = new List<HkdBreakableBody>();
    private List<MyFracturedBlock.Info> m_fractureBlocksCache = new List<MyFracturedBlock.Info>();
    private Dictionary<Vector3I, List<HkdShapeInstanceInfo>> m_fracturedBlocksShapes = new Dictionary<Vector3I, List<HkdShapeInstanceInfo>>();
    private List<MyFractureComponentBase.Info> m_fractureBlockComponentsCache = new List<MyFractureComponentBase.Info>();
    private Dictionary<MySlimBlock, List<HkdShapeInstanceInfo>> m_fracturedSlimBlocksShapes = new Dictionary<MySlimBlock, List<HkdShapeInstanceInfo>>();
    private List<HkdShapeInstanceInfo> m_childList = new List<HkdShapeInstanceInfo>();

    public MyTimeSpan PredictedContactLastTime { get; private set; }

    public int PredictedContactsCounter { get; set; }

    public static float ShipMaxLinearVelocity() => Math.Max(MyGridPhysics.LargeShipMaxLinearVelocity(), MyGridPhysics.SmallShipMaxLinearVelocity());

    public static float LargeShipMaxLinearVelocity() => Math.Max(0.0f, Math.Min(1.498962E+08f, MySector.EnvironmentDefinition.LargeShipMaxSpeed));

    public static float SmallShipMaxLinearVelocity() => Math.Max(0.0f, Math.Min(1.498962E+08f, MySector.EnvironmentDefinition.SmallShipMaxSpeed));

    public static float GetShipMaxLinearVelocity(MyCubeSize size) => size != MyCubeSize.Large ? MyGridPhysics.SmallShipMaxLinearVelocity() : MyGridPhysics.LargeShipMaxLinearVelocity();

    public static float GetShipMaxAngularVelocity(MyCubeSize size) => size != MyCubeSize.Large ? MyGridPhysics.GetSmallShipMaxAngularVelocity() : MyGridPhysics.GetLargeShipMaxAngularVelocity();

    public static float GetLargeShipMaxAngularVelocity() => Math.Max(0.0f, Math.Min(MyGridPhysics.LargeShipMaxAngularVelocityLimit, MySector.EnvironmentDefinition.LargeShipMaxAngularSpeedInRadians));

    public static float GetSmallShipMaxAngularVelocity() => MyFakes.TESTING_VEHICLES ? float.MaxValue : Math.Max(0.0f, Math.Min(MyGridPhysics.SmallShipMaxAngularVelocityLimit, MySector.EnvironmentDefinition.SmallShipMaxAngularSpeedInRadians));

    public float GetMaxLinearVelocity() => MyGridPhysics.GetShipMaxLinearVelocity(this.m_grid.GridSizeEnum);

    public float GetMaxAngularVelocity() => MyGridPhysics.GetShipMaxAngularVelocity(this.m_grid.GridSizeEnum);

    public float GetMaxRelaxedLinearVelocity() => this.GetMaxLinearVelocity() * 10f;

    public float GetMaxRelaxedAngularVelocity() => this.GetMaxAngularVelocity() * 100f;

    public MyGridShape Shape => this.m_shape;

    public bool PredictCollisions
    {
      get
      {
        ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
        if ((long) Volatile.Read(ref this.m_isClientPredictedLastFrameCheck) != (long) simulationFrameCounter)
        {
          bool flag = (ulong) this.m_grid.ClosestParentId > 0UL;
          Volatile.Write(ref this.m_isClientPredicted, !Sync.IsServer && this.m_grid.IsClientPredicted && !flag);
          this.m_isClientPredictedLastFrameCheck = simulationFrameCounter;
        }
        return this.m_isClientPredicted;
      }
    }

    public override float Mass => (HkReferenceObject) this.RigidBody != (HkReferenceObject) null ? this.RigidBody.Mass : 0.0f;

    public override HkRigidBody RigidBody
    {
      get => base.RigidBody;
      protected set
      {
        base.RigidBody = value;
        this.UpdateContactCallbackLimit();
      }
    }

    public MyGridPhysics(MyCubeGrid grid, MyGridShape shape = null, bool staticPhysics = false)
      : base((IMyEntity) grid, MyGridPhysics.GetFlags(grid))
    {
      this.m_grid = grid;
      this.m_shape = shape;
      this.DeformationRatio = this.m_grid.GridSizeEnum == MyCubeSize.Large ? MyGridPhysics.LargeGridDeformationRatio : MyGridPhysics.SmallGridDeformationRatio;
      this.MaterialType = VRage.Game.MyMaterialType.METAL;
      if (staticPhysics)
        this.Flags = RigidBodyFlag.RBF_KINEMATIC;
      this.CreateBody();
      if (MyFakes.ENABLE_PHYSICS_HIGH_FRICTION)
        this.Friction = MyFakes.PHYSICS_HIGH_FRICTION;
      this.m_isServer = Sync.IsServer;
    }

    public override void Close()
    {
      base.Close();
      if (this.m_planetCrash_Effect != null)
        this.m_planetCrash_Effect.Stop(false);
      foreach (MyGridPhysics.CollisionParticleEffect collisionParticle in this.m_collisionParticles)
      {
        collisionParticle.RemainingTime = -1;
        this.FinalizeCollisionParticleEffect(ref collisionParticle);
      }
      if (this.m_shape == null)
        return;
      this.m_shape.Dispose();
      this.m_shape = (MyGridShape) null;
    }

    public override int HavokCollisionSystemID
    {
      get => base.HavokCollisionSystemID;
      protected set
      {
        if (this.HavokCollisionSystemID == value)
          return;
        base.HavokCollisionSystemID = value;
        this.m_grid.HavokSystemIDChanged(value);
      }
    }

    public override Vector3 Gravity
    {
      get => this.m_cachedGravity;
      set
      {
        this.m_cachedGravity = value;
        HkRigidBody rigidBody = this.RigidBody;
        if (!((HkReferenceObject) rigidBody != (HkReferenceObject) null))
          return;
        rigidBody.Gravity = value;
      }
    }

    private static RigidBodyFlag GetFlags(MyCubeGrid grid)
    {
      if (grid.IsStatic)
        return RigidBodyFlag.RBF_STATIC;
      return grid.GridSizeEnum != MyCubeSize.Large ? RigidBodyFlag.RBF_DEFAULT : MyPerGameSettings.LargeGridRBFlag;
    }

    private void CreateBody()
    {
      if (this.m_shape == null)
        this.m_shape = new MyGridShape(this.m_grid);
      if (this.m_grid.GridSizeEnum == MyCubeSize.Large && !this.IsStatic)
        this.InitialSolverDeactivation = HkSolverDeactivation.Off;
      this.ContactPointDelay = (ushort) 0;
      this.CreateFromCollisionObject((HkShape) this.m_shape, Vector3.Zero, MatrixD.Identity, this.m_shape.MassProperties);
      this.RigidBody.ContactPointCallbackEnabled = true;
      this.RigidBody.ContactSoundCallbackEnabled = true;
      if (!MyPerGameSettings.Destruction)
      {
        this.RigidBody.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
        if (!Sync.IsServer)
        {
          this.RigidBody.CollisionAddedCallback += new HkCollisionEventHandler(this.RigidBody_CollisionAddedCallbackClient);
          this.RigidBody.CollisionRemovedCallback += new HkCollisionEventHandler(this.RigidBody_CollisionRemovedCallbackClient);
        }
      }
      else
      {
        this.RigidBody.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback_Destruction);
        this.BreakableBody.BeforeControllerOperation += new BeforeBodyControllerOperation(this.BreakableBody_BeforeControllerOperation);
        this.BreakableBody.AfterControllerOperation += new AfterBodyControllerOperation(this.BreakableBody_AfterControllerOperation);
      }
      this.RigidBody.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
      this.RigidBody.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
      if (this.m_grid.BlocksDestructionEnabled)
      {
        this.RigidBody.BreakLogicHandler = new Havok.BreakLogicHandler(this.BreakLogicHandler);
        this.RigidBody.BreakPartsHandler = new Havok.BreakPartsHandler(this.BreakPartsHandler);
      }
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        this.RigidBody2.ContactPointCallbackEnabled = true;
        if (!MyPerGameSettings.Destruction)
          this.RigidBody2.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
        if (this.m_grid.BlocksDestructionEnabled)
        {
          this.RigidBody2.BreakPartsHandler = new Havok.BreakPartsHandler(this.BreakPartsHandler);
          this.RigidBody2.BreakLogicHandler = new Havok.BreakLogicHandler(this.BreakLogicHandler);
        }
      }
      RigidBodyFlag flags = MyGridPhysics.GetFlags(this.m_grid);
      this.SetDefaultRigidBodyMaxVelocities();
      if (this.IsStatic)
        this.RigidBody.Layer = 13;
      else if (this.m_grid.GridSizeEnum == MyCubeSize.Large)
        this.RigidBody.Layer = flags != RigidBodyFlag.RBF_DOUBLED_KINEMATIC || !MyFakes.ENABLE_DOUBLED_KINEMATIC ? 15 : 16;
      else if (this.m_grid.GridSizeEnum == MyCubeSize.Small)
        this.RigidBody.Layer = 15;
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
        this.RigidBody2.Layer = 17;
      if (MyPerGameSettings.BallFriendlyPhysics)
      {
        this.RigidBody.Restitution = 0.0f;
        if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
          this.RigidBody2.Restitution = 0.0f;
      }
      this.Enabled = true;
    }

    public void SetDefaultRigidBodyMaxVelocities()
    {
      if (this.IsStatic)
      {
        this.RigidBody.MaxLinearVelocity = MyGridPhysics.LargeShipMaxLinearVelocity();
        this.RigidBody.MaxAngularVelocity = MyGridPhysics.GetLargeShipMaxAngularVelocity();
      }
      else if (this.m_grid.GridSizeEnum == MyCubeSize.Large)
      {
        this.RigidBody.MaxLinearVelocity = MyGridPhysics.LargeShipMaxLinearVelocity();
        this.RigidBody.MaxAngularVelocity = MyGridPhysics.GetLargeShipMaxAngularVelocity();
      }
      else
      {
        if (this.m_grid.GridSizeEnum != MyCubeSize.Small)
          return;
        this.RigidBody.MaxLinearVelocity = MyGridPhysics.SmallShipMaxLinearVelocity();
        this.RigidBody.MaxAngularVelocity = MyGridPhysics.GetSmallShipMaxAngularVelocity();
      }
    }

    public void SetRelaxedRigidBodyMaxVelocities()
    {
      this.RigidBody.MaxLinearVelocity = this.GetMaxRelaxedLinearVelocity();
      this.RigidBody.MaxAngularVelocity = this.GetMaxRelaxedAngularVelocity();
    }

    private HkBreakOffLogicResult BreakLogicHandler(
      HkRigidBody otherBody,
      uint shapeKey,
      ref float maxImpulse)
    {
      if ((double) maxImpulse == 0.0)
        maxImpulse = this.Shape.BreakImpulse;
      ulong user = 0;
      IMyEntity entity1 = otherBody.GetEntity(0U);
      MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(entity1 as MyEntity);
      if (controllingPlayer != null)
        user = controllingPlayer.Id.SteamId;
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.m_grid, MySafeZoneAction.Damage, user: user) || !MySession.Static.Settings.EnableVoxelDestruction && entity1 is MyVoxelBase)
        return HkBreakOffLogicResult.DoNotBreakOff;
      HkBreakOffLogicResult breakOffLogicResult = HkBreakOffLogicResult.UseLimit;
      if (!Sync.IsServer)
        breakOffLogicResult = HkBreakOffLogicResult.DoNotBreakOff;
      else if ((HkReferenceObject) this.RigidBody == (HkReferenceObject) null || this.Entity.MarkedForClose || (HkReferenceObject) otherBody == (HkReferenceObject) null)
      {
        breakOffLogicResult = HkBreakOffLogicResult.DoNotBreakOff;
      }
      else
      {
        IMyEntity entity2 = otherBody.GetEntity(0U);
        switch (entity2)
        {
          case null:
            return HkBreakOffLogicResult.DoNotBreakOff;
          case Sandbox.Game.WorldEnvironment.MyEnvironmentSector _:
          case MyFloatingObject _:
          case MyDebrisBase _:
            breakOffLogicResult = HkBreakOffLogicResult.DoNotBreakOff;
            break;
          case MyCharacter _:
            breakOffLogicResult = HkBreakOffLogicResult.DoNotBreakOff;
            break;
          default:
            if (entity2.GetTopMostParent() == this.Entity)
            {
              breakOffLogicResult = HkBreakOffLogicResult.DoNotBreakOff;
              break;
            }
            MyCubeGrid nodeB = entity2 as MyCubeGrid;
            if (!MySession.Static.Settings.EnableSubgridDamage && nodeB != null && MyCubeGridGroups.Static.Physical.HasSameGroup(this.m_grid, nodeB))
            {
              breakOffLogicResult = HkBreakOffLogicResult.DoNotBreakOff;
              break;
            }
            if (this.Entity is MyCubeGrid || nodeB != null)
            {
              breakOffLogicResult = HkBreakOffLogicResult.UseLimit;
              break;
            }
            break;
        }
        if (this.WeldInfo.Children.Count > 0)
          this.HavokWorld.BreakOffPartsUtil.MarkEntityBreakable((HkEntity) this.RigidBody, this.Shape.BreakImpulse);
      }
      int num = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
      return breakOffLogicResult;
    }

    protected override void ActivateCollision()
    {
      if (this.m_world == null)
        return;
      this.HavokCollisionSystemID = this.m_world.GetCollisionFilter().GetNewSystemGroup();
    }

    public override void Activate(object world, ulong clusterObjectID)
    {
      if (MyPerGameSettings.Destruction && this.IsStatic)
        this.Shape.FindConnectionsToWorld();
      base.Activate(world, clusterObjectID);
      this.MarkBreakable((HkWorld) world);
    }

    public override void ActivateBatch(object world, ulong clusterObjectID)
    {
      if (MyPerGameSettings.Destruction && this.IsStatic)
        this.Shape.FindConnectionsToWorld();
      base.ActivateBatch(world, clusterObjectID);
      this.MarkBreakable((HkWorld) world);
    }

    public override void Deactivate(object world)
    {
      this.DisableTOIOptimization();
      this.UnmarkBreakable((HkWorld) world);
      base.Deactivate(world);
    }

    public override void DeactivateBatch(object world)
    {
      this.UnmarkBreakable((HkWorld) world);
      base.DeactivateBatch(world);
    }

    public override HkShape GetShape() => (HkShape) this.Shape;

    private void MarkBreakable(HkWorld world)
    {
      if (!this.m_grid.BlocksDestructionEnabled)
        return;
      this.m_shape.MarkBreakable(world, this.RigidBody);
      if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null))
        return;
      this.m_shape.MarkBreakable(world, this.RigidBody2);
    }

    private void UnmarkBreakable(HkWorld world)
    {
      if (!this.m_grid.BlocksDestructionEnabled)
        return;
      if (this.m_shape != null)
        this.m_shape.UnmarkBreakable(world, this.RigidBody);
      if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null))
        return;
      this.m_shape.UnmarkBreakable(world, this.RigidBody2);
    }

    private void PlanetCrashEffect_Update()
    {
      if (!MyFakes.PLANET_CRASH_ENABLED || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      ++this.m_planetCrash_TimeBetweenPoints;
      if (this.m_planetCrash_CrashAccumulation > 0)
        --this.m_planetCrash_CrashAccumulation;
      if (!this.IsPlanetCrashing())
        this.m_grid.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.PlanetCrashEffect_Update));
      this.PlanetCrashEffect_Reduce();
      if (this.m_planetCrash_Effect == null && (double) this.m_planetCrash_generationMultiplier > 0.00999999977648258)
        MyParticlesManager.TryCreateParticleEffect("PlanetCrash", (MatrixD) ref Matrix.Identity, out this.m_planetCrash_Effect);
      if (this.m_planetCrash_Effect != null)
      {
        this.m_planetCrash_Effect.UserBirthMultiplier = this.PlanetCrash_GetMultiplier();
        MyParticleEffect planetCrashEffect = this.m_planetCrash_Effect;
        Matrix world = Matrix.CreateWorld((Vector3) this.m_planetCrash_CenterPoint.Value, -this.m_planetCrash_Normal.Value, Vector3.CalculatePerpendicularVector(this.m_planetCrash_Normal.Value));
        MatrixD matrixD = (MatrixD) ref world;
        planetCrashEffect.WorldMatrix = matrixD;
        this.m_planetCrash_Effect.UserScale = this.m_planetCrash_ScaleCurrent * 0.1f;
      }
      if ((double) this.m_planetCrash_ScaleGoal > (double) this.m_planetCrash_ScaleCurrent)
        this.m_planetCrash_ScaleCurrent *= 1.06f;
      else
        this.m_planetCrash_ScaleCurrent *= 0.995f;
      if ((double) this.m_planetCrash_generationMultiplier >= 0.00999999977648258)
        return;
      this.m_planetCrash_generationMultiplier = 0.0f;
      this.m_planetCrash_ScaleGoal = 1f;
      this.m_planetCrash_ScaleCurrent = 1f;
      this.m_planetCrash_CrashAccumulation = 0;
      this.m_planetCrash_IsStarted = false;
      if (this.m_planetCrash_Effect == null)
        return;
      this.m_planetCrash_Effect.Stop(false);
      this.m_planetCrash_Effect = (MyParticleEffect) null;
    }

    private void PlanetCrashEffect_AddCollision(
      Vector3D position,
      float separationSpeed,
      Vector3 normal,
      MyVoxelBase voxel = null)
    {
      if (!MyFakes.PLANET_CRASH_ENABLED || this.IsStatic || (Sandbox.Engine.Platform.Game.IsDedicated || this.m_grid.BlocksCount < 5))
        return;
      float mass = MyGridPhysicalGroupData.GetGroupSharedProperties(this.m_grid, false).Mass;
      float num = separationSpeed * separationSpeed;
      bool planetCrashIsStarted = this.m_planetCrash_IsStarted;
      if (!planetCrashIsStarted && ((double) mass < 50000.0 && (double) num < 2500.0 || (double) mass < 500000.0 && (double) num < 900.0 || ((double) mass < 3000000.0 && (double) num < 200.0 || ((double) num < 50.0 || (double) mass < 15000.0))) || planetCrashIsStarted && ((double) mass < 15000.0 || (double) num < 50.0))
        return;
      Vector3 naturalGravityInPoint = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      if ((double) naturalGravityInPoint.LengthSquared() < 0.01)
        return;
      if (!this.m_planetCrash_IsStarted)
      {
        this.m_planetCrash_TimeBetweenPoints = 0;
        this.m_planetCrash_CrashAccumulation += 30;
        if (this.m_planetCrash_CrashAccumulation < 90)
          return;
      }
      this.m_planetCrash_ScaleGoal = 1.5f * Math.Max((float) Math.Log10((double) mass - 30000.0) - 3f, 0.3f) * Math.Max(0.0f, (float) Math.Log10((double) num) - 2f);
      if (!this.m_planetCrash_IsStarted)
        this.m_planetCrash_ScaleCurrent = 0.8f * this.m_planetCrash_ScaleGoal;
      this.m_planetCrash_IsStarted = true;
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.PlanetCrashEffect_Update), 22);
      if (!this.m_planetCrash_CenterPoint.HasValue)
      {
        this.m_planetCrash_CenterPoint = new Vector3D?(position);
        this.m_planetCrash_Normal = new Vector3?(Vector3.Normalize(naturalGravityInPoint));
      }
      else
      {
        (position - this.m_planetCrash_CenterPoint.Value).Length();
        this.m_planetCrash_CenterPoint = new Vector3D?(position);
        this.m_planetCrash_Normal = new Vector3?(Vector3.Normalize(0.85f * this.m_planetCrash_Normal.Value + 0.15f * normal));
      }
      this.m_planetCrash_generationMultiplier = 100f;
    }

    private float PlanetCrash_GetMultiplier() => this.m_planetCrash_generationMultiplier * 0.025f;

    private void PlanetCrashEffect_Reduce()
    {
      if (this.m_planetCrash_TimeBetweenPoints < 60)
        this.m_planetCrash_generationMultiplier -= 0.3f;
      else
        this.m_planetCrash_generationMultiplier *= 0.92f;
    }

    public bool IsPlanetCrashing() => this.m_planetCrash_IsStarted;

    public bool PlanetCrashingNeedsUpdates() => this.IsPlanetCrashing() || this.m_planetCrash_CrashAccumulation > 0;

    public bool IsPlanetCrashing_PointConcealed(Vector3D point)
    {
      if (!this.m_planetCrash_IsStarted)
        return false;
      Vector3 vector1 = (Vector3) (point - this.m_planetCrash_CenterPoint.Value);
      double num1 = (double) Math.Abs(Vector3.Dot(vector1, this.m_planetCrash_Normal.Value));
      float num2 = 2f * this.m_planetCrash_ScaleCurrent;
      float num3 = 4f * this.m_planetCrash_ScaleCurrent;
      double num4 = (double) num2;
      return num1 < num4 && (double) vector1.LengthSquared() < (double) num3 * (double) num3;
    }

    private void RigidBody_ContactPointCallback(ref HkContactPointEvent value) => this.RigidBody_ContactPointCallbackImpl(ref value);

    private void RigidBody_ContactPointCallbackImpl(ref HkContactPointEvent value)
    {
      if (this.m_grid == null || this.m_grid.Physics == null)
        return;
      if (value.IsToi && Interlocked.Increment(ref this.m_toiCountThisFrame) == 2)
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateTOIOptimizer), 26);
      HkCollisionEvent hkCollisionEvent;
      if ((double) Math.Abs(value.SeparatingVelocity) < 0.300000011920929)
      {
        hkCollisionEvent = value.Base;
        int num;
        if (!hkCollisionEvent.GetRigidBody(0).IsEnvironment)
        {
          hkCollisionEvent = value.Base;
          num = hkCollisionEvent.GetRigidBody(1).IsEnvironment ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
          return;
      }
      bool AisThis;
      IMyEntity otherEntity1 = value.GetOtherEntity((IMyEntity) this.m_grid, out AisThis);
      if (otherEntity1 == null)
        return;
      if (this.PredictCollisions)
        this.PredictContactImpulse(otherEntity1, ref value);
      MyGridContactInfo myGridContactInfo = new MyGridContactInfo(ref value, this.m_grid, otherEntity1 as MyEntity);
      if (myGridContactInfo.CollidingEntity is MyCharacter || myGridContactInfo.CollidingEntity.MarkedForClose)
        return;
      HkContactPoint contactPoint1;
      if (AisThis)
      {
        contactPoint1 = value.ContactPoint;
        contactPoint1.Flip();
      }
      HkContactPoint contactPoint2 = value.ContactPoint;
      bool flag1 = myGridContactInfo.CollidingEntity is MyVoxelPhysics || myGridContactInfo.CollidingEntity is MyVoxelMap;
      if (flag1)
      {
        if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTACT_MATERIAL)
        {
          MyVoxelMaterialDefinition voxelSurfaceMaterial = myGridContactInfo.VoxelSurfaceMaterial;
          if (voxelSurfaceMaterial != null)
            MyRenderProxy.DebugDrawText3D(this.ClusterToWorld(contactPoint2.Position), voxelSurfaceMaterial.Id.SubtypeName + "(" + voxelSurfaceMaterial.Friction.ToString("F2") + ";" + voxelSurfaceMaterial.Restitution.ToString("F2") + ")", Color.Red, 0.7f, false);
        }
        if (this.m_grid.Render != null)
          this.m_grid.Render.ResetLastVoxelContactTimer();
      }
      int num1 = !myGridContactInfo.IsKnown ? 1 : 0;
      bool flag2 = MyPerGameSettings.EnableCollisionSparksEffect && !this.IsPlanetCrashing() && myGridContactInfo.CollidingEntity is MyCubeGrid | flag1;
      myGridContactInfo.HandleEvents();
      if (MyDebugDrawSettings.DEBUG_DRAW_FRICTION)
      {
        Vector3D world = this.ClusterToWorld(contactPoint2.Position);
        Vector3 vector3 = -this.GetVelocityAtPoint(world) * 0.1f;
        float num2 = Math.Abs(this.Gravity.Dot(contactPoint2.Normal) * value.ContactProperties.Friction);
        if ((double) vector3.Length() > 0.5)
        {
          double num3 = (double) vector3.Normalize();
          MyRenderProxy.DebugDrawArrow3D(world, world + num2 * vector3, Color.Gray, new Color?(Color.Gray), persistent: true);
        }
      }
      if (num1 != 0)
      {
        MyVoxelMaterialDefinition voxelSurfaceMaterial = myGridContactInfo.VoxelSurfaceMaterial;
        if (voxelSurfaceMaterial != null)
        {
          HkContactPointProperties contactProperties = value.ContactProperties;
          contactProperties.Friction *= voxelSurfaceMaterial.Friction;
          contactProperties.Restitution *= voxelSurfaceMaterial.Restitution * (this.m_grid.GridSizeEnum == MyCubeSize.Small ? 0.4f : 0.25f);
        }
      }
      if (!this.LowSimulationQuality && this.m_isServer && otherEntity1 is MyCubeGrid | flag1)
      {
        MyEntity.ContactPointData.ContactPointDataTypes contactPointDataTypes = MyEntity.ContactPointData.ContactPointDataTypes.None;
        hkCollisionEvent = value.Base;
        HkRigidBody bodyA = hkCollisionEvent.BodyA;
        hkCollisionEvent = value.Base;
        HkRigidBody bodyB = hkCollisionEvent.BodyB;
        Vector3 position = contactPoint2.Position;
        Vector3 separatingVelocity = this.CalculateSeparatingVelocity(bodyA, bodyB, position);
        float num2 = separatingVelocity.Length();
        if (flag1)
          contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_PlanetCrash;
        HkContactPointProperties contactProperties;
        if (myGridContactInfo.EnableParticles)
        {
          if (flag2)
          {
            if ((double) num2 <= 1.0)
            {
              contactProperties = value.ContactProperties;
              if ((double) contactProperties.MaxImpulse <= 5000.0)
                goto label_48;
            }
            if (this.IsStatic || (double) MyGridPhysicalGroupData.GetGroupSharedProperties(this.m_grid, false).Mass > 5000.0)
            {
              int count = this.m_lastContacts.Count;
              if (this.m_lastContacts.TryAdd(value.ContactPointId, MySandboxGame.TotalGamePlayTimeInMilliseconds))
              {
                if ((double) num2 > 0.300000011920929)
                {
                  contactProperties = value.ContactProperties;
                  if ((double) contactProperties.MaxImpulse > 20000.0)
                    goto label_42;
                }
                if ((double) num2 <= 0.800000011920929)
                  goto label_46;
label_42:
                Vector3 vector2 = separatingVelocity / num2;
                if ((double) Math.Abs(Vector3.Dot(contactPoint2.Normal, vector2)) > 0.75 || (double) num2 < 2.0)
                  contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_Collision;
                else if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
                  contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_GridCollision;
label_46:
                if (count == 0)
                  this.m_grid.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.ProcessContacts), 23);
              }
            }
          }
label_48:
          if ((!(MyPerGameSettings.EnableCollisionSparksEffect & flag1) ? 0 : (!this.IsPlanetCrashing() ? 1 : 0)) != 0 && (double) Math.Abs(value.SeparatingVelocity * (this.m_grid.Mass / 100000f)) > 0.25)
            contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_Dust;
        }
        if (contactPointDataTypes != MyEntity.ContactPointData.ContactPointDataTypes.None)
        {
          Vector3 normal = contactPoint2.Normal;
          Vector3D world = this.ClusterToWorld(contactPoint2.Position);
          if (Sync.IsServer && MyMultiplayer.Static != null)
          {
            MyCubeGrid entity = this.Entity as MyCubeGrid;
            Vector3 vector3 = (Vector3) (world - entity.PositionComp.GetPosition());
            MyCubeGrid myCubeGrid = entity;
            long entityId = otherEntity1.EntityId;
            ref Vector3 local1 = ref vector3;
            ref Vector3 local2 = ref normal;
            ref Vector3 local3 = ref separatingVelocity;
            double num3 = (double) num2;
            contactProperties = value.ContactProperties;
            double maxImpulse = (double) contactProperties.MaxImpulse;
            int num4 = (int) contactPointDataTypes;
            myCubeGrid.UpdateParticleContactPoint(entityId, ref local1, ref local2, ref local3, (float) num3, (float) maxImpulse, (MyEntity.ContactPointData.ContactPointDataTypes) num4);
          }
          else
          {
            IMyEntity otherEntity2 = otherEntity1;
            ref Vector3D local1 = ref world;
            ref Vector3 local2 = ref normal;
            ref Vector3 local3 = ref separatingVelocity;
            double num3 = (double) num2;
            contactProperties = value.ContactProperties;
            double maxImpulse = (double) contactProperties.MaxImpulse;
            int num4 = (int) contactPointDataTypes;
            this.PlayCollisionParticlesInternal(otherEntity2, ref local1, ref local2, ref local3, (float) num3, (float) maxImpulse, (MyEntity.ContactPointData.ContactPointDataTypes) num4);
          }
        }
      }
      if (!AisThis)
        return;
      contactPoint1 = value.ContactPoint;
      contactPoint1.Flip();
    }

    private void ProcessContacts()
    {
      if (this.m_lastContacts.Count > 0)
      {
        this.m_tmpContactId.Clear();
        foreach (KeyValuePair<ushort, int> lastContact in this.m_lastContacts)
        {
          if (MySandboxGame.TotalGamePlayTimeInMilliseconds > lastContact.Value + MyGridPhysics.SparksEffectDelayPerContactMs)
            this.m_tmpContactId.Add(lastContact.Key);
        }
        foreach (ushort key in this.m_tmpContactId)
          this.m_lastContacts.Remove(key);
      }
      if (this.m_lastContacts.Count != 0)
        return;
      this.m_grid.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.ProcessContacts));
    }

    public void PlayCollisionParticlesInternal(
      IMyEntity otherEntity,
      ref Vector3D worldPosition,
      ref Vector3 normal,
      ref Vector3 separatingVelocity,
      float separatingSpeed,
      float impulse,
      MyEntity.ContactPointData.ContactPointDataTypes flags)
    {
      if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_PlanetCrash) != MyEntity.ContactPointData.ContactPointDataTypes.None)
        this.PlanetCrashEffect_AddCollision(worldPosition, separatingSpeed, normal, otherEntity as MyVoxelBase);
      if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_Collision) != MyEntity.ContactPointData.ContactPointDataTypes.None)
        this.AddCollisionEffect(worldPosition, normal, separatingSpeed, impulse);
      if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_GridCollision) != MyEntity.ContactPointData.ContactPointDataTypes.None)
        this.AddGridCollisionEffect(worldPosition - this.RigidBody.Position, normal, separatingVelocity, separatingSpeed, impulse);
      if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_Dust) == MyEntity.ContactPointData.ContactPointDataTypes.None)
        return;
      float scale = MathHelper.Clamp(Math.Abs(separatingSpeed * (this.m_grid.Mass / 100000f)) / 10f, 0.2f, 2f);
      this.AddDustEffect(worldPosition, scale);
    }

    private static Vector3 GetGridPosition(
      HkContactPoint contactPoint,
      HkRigidBody gridBody,
      MyCubeGrid grid,
      int body)
    {
      return Vector3.Transform(contactPoint.Position + (body == 0 ? 0.1f : -0.1f) * contactPoint.Normal, Matrix.Invert(gridBody.GetRigidBodyMatrix()));
    }

    private MyGridContactInfo ReduceVelocities(MyGridContactInfo info)
    {
      info.Event.AccessVelocities(0);
      info.Event.AccessVelocities(1);
      if (!info.CollidingEntity.Physics.IsStatic && (double) info.CollidingEntity.Physics.Mass < 600.0)
        info.CollidingEntity.Physics.LinearVelocity /= 2f;
      if (!this.IsStatic && (double) MyDestructionHelper.MassFromHavok(this.Mass) < 600.0)
        this.LinearVelocity = this.LinearVelocity / 2f;
      info.Event.UpdateVelocities(0);
      info.Event.UpdateVelocities(1);
      return info;
    }

    private bool BreakAtPoint(ref HkBreakOffPointInfo pt, ref HkArrayUInt32 brokenKeysOut)
    {
      pt.ContactPosition = this.ClusterToWorld(pt.ContactPoint.Position);
      IMyEntity entity = pt.CollidingBody.GetEntity(0U);
      if (entity.Physics == null || (HkReferenceObject) entity.Physics.RigidBody == (HkReferenceObject) null)
        return false;
      Vector3 separatingVelocity1 = this.CalculateSeparatingVelocity(this.RigidBody, pt.CollidingBody, ref pt.ContactPoint);
      float num1 = Math.Min(this.IsStatic ? entity.Physics.Mass : this.Mass, entity.Physics.IsStatic ? this.Mass : entity.Physics.Mass);
      float num2 = separatingVelocity1.Length();
      if (entity is MyVoxelBase && this.m_grid.BlocksCount == 1)
        num2 += pt.BreakingImpulse / num1;
      float num3 = num2 * num1;
      float separatingVelocity2 = num2;
      int num4 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
      if ((double) num3 <= 21000.0 || (double) separatingVelocity2 <= 0.0)
        return false;
      this.PerformDeformationOnGroup((MyEntity) this.Entity, (MyEntity) entity, ref pt, separatingVelocity2);
      pt.ContactPointDirection *= -1f;
      this.PerformDeformationOnGroup((MyEntity) entity, (MyEntity) this.Entity, ref pt, separatingVelocity2);
      return false;
    }

    private Vector3 CalculateSeparatingVelocity(
      HkRigidBody bodyA,
      HkRigidBody bodyB,
      ref HkContactPoint cp)
    {
      return this.CalculateSeparatingVelocity(bodyA, bodyB, cp.Position);
    }

    private Vector3 CalculateSeparatingVelocity(
      HkRigidBody bodyA,
      HkRigidBody bodyB,
      Vector3 position)
    {
      Vector3 vector3_1 = Vector3.Zero;
      if (!bodyA.IsFixed)
      {
        Vector3 vector2 = position - bodyA.CenterOfMassWorld;
        vector3_1 = Vector3.Cross(bodyA.AngularVelocity, vector2);
        vector3_1.Add(bodyA.LinearVelocity);
      }
      Vector3 vector3_2 = Vector3.Zero;
      if (!bodyB.IsFixed)
      {
        Vector3 vector2 = position - bodyB.CenterOfMassWorld;
        vector3_2 = Vector3.Cross(bodyB.AngularVelocity, vector2);
        vector3_2.Add(bodyB.LinearVelocity);
      }
      return vector3_1 - vector3_2;
    }

    private Vector3 CalculateSeparatingVelocity(
      MyCubeGrid first,
      MyCubeGrid second,
      Vector3 position)
    {
      Vector3D world = this.ClusterToWorld(position);
      Vector3 vector3_1 = Vector3.Zero;
      if (!first.IsStatic && first.Physics != null)
      {
        Vector3 vector2 = (Vector3) ((Vector3) world - first.Physics.CenterOfMassWorld);
        vector3_1 = Vector3.Cross(first.Physics.AngularVelocity, vector2);
        vector3_1.Add(first.Physics.LinearVelocity);
      }
      Vector3 vector3_2 = Vector3.Zero;
      if (!second.IsStatic && second.Physics != null)
      {
        Vector3 vector2 = (Vector3) (world - second.Physics.CenterOfMassWorld);
        vector3_2 = Vector3.Cross(second.Physics.AngularVelocity, vector2);
        vector3_2.Add(second.Physics.LinearVelocity);
      }
      return vector3_1 - vector3_2;
    }

    private bool PerformDeformationOnGroup(
      MyEntity entity,
      MyEntity other,
      ref HkBreakOffPointInfo pt,
      float separatingVelocity)
    {
      bool flag = false;
      if (!entity.MarkedForClose)
      {
        BoundingBoxD boundingBoxD = entity.PositionComp.WorldAABB;
        boundingBoxD = boundingBoxD.Inflate(0.100000001490116);
        if (boundingBoxD.Contains(pt.ContactPosition) != ContainmentType.Disjoint)
        {
          if (entity.Physics is MyGridPhysics physics)
            flag = physics.PerformDeformation(ref pt, false, separatingVelocity, other);
        }
        else
        {
          int num = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
        }
      }
      return flag;
    }

    private bool BreakPartsHandler(
      ref HkBreakOffPoints breakOffPoints,
      ref HkArrayUInt32 brokenKeysOut)
    {
      bool flag = false;
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.m_grid, MySafeZoneAction.Damage))
        return flag;
      for (int index = 0; index < breakOffPoints.Count; ++index)
      {
        HkBreakOffPointInfo pt = breakOffPoints[index];
        flag |= this.BreakAtPoint(ref pt, ref brokenKeysOut);
      }
      return false;
    }

    private static float CalculateSoften(
      float softAreaPlanarInv,
      float softAreaVerticalInv,
      ref Vector3 normal,
      Vector3 contactToTarget)
    {
      float result;
      Vector3.Dot(ref normal, ref contactToTarget, out result);
      if ((double) result < 0.0)
        result = -result;
      float num1 = (float) (1.0 - (double) result * (double) softAreaVerticalInv);
      if ((double) num1 <= 0.0)
        return 0.0f;
      float num2 = contactToTarget.LengthSquared() - result * result;
      if ((double) num2 <= 0.0)
        return num1;
      float num3 = (float) (1.0 - Math.Sqrt((double) num2) * (double) softAreaPlanarInv);
      return (double) num3 <= 0.0 ? 0.0f : num1 * num3;
    }

    public void PerformMeteoriteDeformation(in MyMeteor.MyMeteorGameLogic.ContactProperties pt)
    {
      float deformationOffset = Math.Min((0.3f + Math.Max(0.0f, (float) Math.Sqrt((double) Math.Abs(pt.SeparatingVelocity) + Math.Pow((double) pt.CollidingBody.Mass, 0.72)) / 10f)) * 6f, 5f);
      float softAreaPlanar = ((float) Math.Pow((double) pt.CollidingBody.Mass, 0.150000005960464) - 0.3f) * (this.m_grid.GridSizeEnum == MyCubeSize.Large ? 4f : 1f);
      float softAreaVertical = deformationOffset * (this.m_grid.GridSizeEnum == MyCubeSize.Large ? 1f : 0.2f);
      MatrixD matrix1 = this.m_grid.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D1 = Vector3D.Transform(pt.Position, matrix1);
      Vector3 vector3 = Vector3.TransformNormal(pt.Normal, matrix1) * pt.Direction;
      bool flag = this.ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, (Vector3) vector3D1, vector3, MyDamageType.Deformation, lowerRatioLimit: (this.m_grid.GridSizeEnum == MyCubeSize.Large ? 0.6f : 0.16f));
      MyPhysics.CastRay(pt.Position, pt.Position - softAreaVertical * Vector3.Normalize(pt.Normal), this.m_hitList);
      foreach (MyPhysics.HitInfo hit in this.m_hitList)
      {
        IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
        if (hitEntity != this.m_grid.Components && hitEntity is MyCubeGrid)
        {
          MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
          MatrixD matrix2 = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
          Vector3D vector3D2 = Vector3D.Transform(pt.Position, matrix2);
          vector3 = Vector3.TransformNormal(pt.Normal, matrix2) * pt.Direction;
          myCubeGrid.Physics.ApplyDeformation(deformationOffset, softAreaPlanar * (this.m_grid.GridSizeEnum == myCubeGrid.GridSizeEnum ? 1f : (myCubeGrid.GridSizeEnum == MyCubeSize.Large ? 2f : 0.25f)), softAreaVertical * (this.m_grid.GridSizeEnum == myCubeGrid.GridSizeEnum ? 1f : (myCubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5f : 0.2f)), (Vector3) vector3D2, vector3, MyDamageType.Deformation, lowerRatioLimit: (myCubeGrid.GridSizeEnum == MyCubeSize.Large ? 0.6f : 0.16f));
        }
      }
      this.m_hitList.Clear();
      float num = Math.Max(this.m_grid.GridSize, deformationOffset * (this.m_grid.GridSizeEnum == MyCubeSize.Large ? 0.25f : 0.05f));
      if ((((double) num <= 0.0 ? 0 : ((double) deformationOffset > (double) this.m_grid.GridSize / 2.0 ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        MyGridPhysics.ExplosionInfo info = new MyGridPhysics.ExplosionInfo()
        {
          Position = pt.Position,
          ExplosionType = MyExplosionTypeEnum.GRID_DESTRUCTION,
          Radius = num,
          ShowParticles = true,
          GenerateDebris = true
        };
        this.EnqueueExplosion(in info);
      }
      else
        this.AddCollisionEffect(pt.Position, vector3, 0.0f, 0.0f);
    }

    private bool PerformDeformation(
      ref HkBreakOffPointInfo pt,
      bool fromBreakParts,
      float separatingVelocity,
      MyEntity otherEntity)
    {
      if (!this.m_grid.BlocksDestructionEnabled)
      {
        int num = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
        return false;
      }
      bool flag1 = false;
      ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
      if ((long) this.m_frameCollided == (long) simulationFrameCounter)
      {
        foreach (Vector3D vector3D in this.m_contactPosCache)
        {
          if (Vector3D.DistanceSquared(pt.ContactPosition, vector3D) < (double) this.m_grid.GridSize * (double) this.m_grid.GridSize / 4.0)
          {
            flag1 = true;
            break;
          }
        }
      }
      else
      {
        if (simulationFrameCounter - this.m_frameCollided > 100UL)
        {
          this.m_frameFirstImpact = simulationFrameCounter;
          this.m_impactDot = 0.0f;
        }
        this.m_appliedSlowdownThisFrame = false;
        this.m_contactPosCache.Clear();
      }
      float num1 = separatingVelocity;
      bool flag2 = otherEntity is MyVoxelBase;
      bool flag3 = flag2 && !Vector3.IsZero(ref this.m_cachedGravity);
      bool otherEntityDeformable = !flag2 && !(otherEntity is MyTrees);
      float num2 = !this.IsStatic ? Math.Min(1f, this.Mass / MyFakes.DEFORMATION_MASS_THR) : (!otherEntity.Physics.IsStatic ? Math.Min(1f, otherEntity.Physics.Mass / MyFakes.DEFORMATION_MASS_THR) : 1f);
      float num3 = 1f;
      float val1 = num1 * num2 * MyFakes.DEFORMATION_OFFSET_RATIO;
      MyCubeGrid otherGrid = otherEntity as MyCubeGrid;
      bool isLargeGrid = this.m_grid.GridSizeEnum == MyCubeSize.Large;
      if (!this.IsStatic && !otherEntity.Physics.IsStatic)
      {
        if (otherGrid != null && this.m_grid.GridSizeEnum != otherGrid.GridSizeEnum)
        {
          if (isLargeGrid)
            val1 *= 0.105f;
          else
            val1 *= 1.6f;
        }
        else
          val1 *= 0.5f;
      }
      else if (otherEntityDeformable)
      {
        if (otherGrid != null && this.m_grid.GridSizeEnum != otherGrid.GridSizeEnum)
        {
          if (isLargeGrid)
          {
            val1 *= 0.09f;
          }
          else
          {
            num3 = 4.5f;
            val1 *= 0.22f;
          }
        }
        else
          val1 *= 0.5f;
      }
      else if ((double) this.m_grid.PositionComp.LocalAABB.Volume() < 20.0 && (double) num1 / 60.0 > (double) this.m_grid.GridSize / 5.0)
        val1 *= 30f;
      else if (isLargeGrid || !flag2)
        val1 *= 1.5f;
      float deformationOffset;
      if (flag2)
      {
        float num4 = isLargeGrid ? 6.8f : 8f;
        float val2 = MyFakes.DEFORMATION_OFFSET_MAX * 10f;
        deformationOffset = Math.Min(val1 / num4, val2);
      }
      else
        deformationOffset = Math.Min(val1, MyFakes.DEFORMATION_OFFSET_MAX);
      if ((double) deformationOffset <= 0.100000001490116)
      {
        int num4 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
        return false;
      }
      float softAreaPlanar = (isLargeGrid ? 6f : 1.2f) * num3;
      float softAreaVertical = (isLargeGrid ? 1.5f : 1f) * deformationOffset;
      MatrixD matrix = this.m_grid.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D1 = Vector3D.Transform(pt.ContactPosition, matrix);
      Vector3 normal = -this.GetVelocityAtPoint(pt.ContactPosition);
      Vector3 vector2 = pt.ContactPointDirection * pt.ContactPoint.Normal;
      if (!normal.IsValid() || (double) normal.LengthSquared() < 25.0)
        normal = vector2;
      Vector3 localNormal = Vector3.TransformNormal(normal, matrix);
      float num5 = localNormal.Normalize();
      bool flag4 = (double) num5 < 3.0;
      Vector3 vector1 = -normal;
      double num6 = (double) vector1.Normalize();
      float num7 = Math.Abs(Vector3.Dot(vector1, vector2));
      this.m_impactDot = (double) this.m_impactDot != 0.0 ? (float) ((double) this.m_impactDot * 0.5 + (double) num7 * 0.5) : num7;
      if (flag2 & isLargeGrid)
      {
        softAreaVertical *= MyFakes.DEFORMATION_DAMAGE_MULTIPLIER;
        softAreaPlanar *= MyFakes.DEFORMATION_DAMAGE_MULTIPLIER * 2f;
        if (flag3)
        {
          float num4 = (float) (1.0 + (double) this.m_impactDot * (double) this.m_impactDot / 2.0);
          softAreaVertical *= num4;
          softAreaPlanar *= num4;
        }
      }
      int destroyed;
      int num8 = this.ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, (Vector3) vector3D1, localNormal, MyDamageType.Deformation, out destroyed, attackerId: (otherEntity != null ? otherEntity.EntityId : 0L)) ? 1 : 0;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        MyRenderProxy.DebugDrawArrow3D(pt.ContactPosition, pt.ContactPosition + Vector3.Normalize(normal) * (float) destroyed, Color.Red, new Color?(Color.Red), text: this.Entity.DisplayName, persistent: true);
      int num9 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
      if (destroyed > 0)
      {
        if (!flag1)
          this.m_contactPosCache.Add(pt.ContactPosition);
        this.m_frameCollided = simulationFrameCounter;
      }
      if (!this.IsStatic)
      {
        Vector3 cpPosition = pt.ContactPoint.Position;
        if (MyFakes.DEFORMATION_APPLY_IMPULSE)
        {
          HkRigidBody rigidBody = this.RigidBody;
          Vector3 impulse = -(rigidBody.LinearVelocity * rigidBody.Mass) * (float) destroyed * MyFakes.DEFORMATION_IMPULSE_FACTOR;
          rigidBody.ApplyPointImpulse(impulse, cpPosition);
        }
        else if (otherEntity != null)
        {
          if (flag2)
          {
            if (!this.m_appliedSlowdownThisFrame)
            {
              this.m_appliedSlowdownThisFrame = true;
              MySandboxGame.Static.Invoke("ApplyColisionForce", (object) this, (Action<object>) (context =>
              {
                MyGridPhysics myGridPhysics = (MyGridPhysics) context;
                HkRigidBody rigidBody = myGridPhysics.RigidBody;
                if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
                  return;
                bool flag5 = myGridPhysics.m_grid.GridSizeEnum == MyCubeSize.Small;
                float impactDot = myGridPhysics.m_impactDot;
                ulong num4 = MySandboxGame.Static.SimulationFrameCounter - myGridPhysics.m_frameFirstImpact;
                int num10 = num4 >= 100UL ? 0 : ((double) rigidBody.LinearVelocity.LengthSquared() > 25.0 ? 1 : 0);
                bool flag6 = ((num4 > 50UL ? 1 : ((double) impactDot <= 0.800000011920929 ? 0 : (num4 > 10UL ? 1 : 0))) | (flag5 ? 1 : 0)) != 0;
                bool flag7 = num4 > 100UL | flag5;
                if (num10 != 0)
                {
                  Vector3 angularVelocity = rigidBody.AngularVelocity;
                  rigidBody.AngularVelocity -= angularVelocity * (1f - impactDot) / 2f;
                }
                if (!flag6)
                  return;
                float num11 = 1f - impactDot;
                float num12 = (float) ((1.0 - (double) num11 * (double) num11 + (double) impactDot) / 1.5);
                if ((double) impactDot < 0.5)
                  num12 /= 2f;
                Vector3 linearVelocity = rigidBody.LinearVelocity;
                float maxLinearVelocity = myGridPhysics.GetMaxLinearVelocity();
                float num13 = Math.Min((float) ((double) num12 * (double) linearVelocity.Length() / ((double) maxLinearVelocity * 1.5)), flag7 ? 0.2f : 0.1f);
                if ((double) impactDot > 0.5)
                  num13 *= (float) (1.0 + (double) impactDot * 0.5);
                if (!Vector3.IsZero(ref myGridPhysics.m_cachedGravity))
                {
                  Vector3 projectedVector = -linearVelocity;
                  Vector3 vector3_1 = myGridPhysics.m_cachedGravity.Project(projectedVector);
                  Vector3 vector3_2 = vector3_1 * num13 * 2f;
                  if (flag7)
                  {
                    Vector3 vector3_3 = (projectedVector - vector3_1) * num13;
                    vector3_2 += vector3_3;
                  }
                  rigidBody.LinearVelocity += vector3_2;
                }
                else
                {
                  if (!flag5)
                    return;
                  Vector3 vector3_1 = -linearVelocity;
                  Vector3 vector3_2 = num13 * 1f * vector3_1;
                  rigidBody.LinearVelocity += vector3_2;
                }
              }));
            }
          }
          else if (destroyed > 0)
          {
            HkRigidBody rigidBody = this.RigidBody;
            Vector3 linearVelocity = rigidBody.LinearVelocity;
            Vector3 velocityMass = linearVelocity * rigidBody.Mass;
            if (Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress && !MyPhysics.InsideSimulation)
              Sandbox.Game.Entities.MyEntities.InvokeLater(new Action(ApplyImpulses));
            else
              ApplyImpulses();

            void ApplyImpulses()
            {
              if (otherEntity.Closed)
                return;
              if (otherEntity.Physics.IsStatic)
              {
                if (otherEntityDeformable && otherGrid != null)
                {
                  float velocityRelayStatic = MyFakes.DEFORMATION_VELOCITY_RELAY_STATIC;
                  if (isLargeGrid)
                    velocityRelayStatic /= 8f;
                  else if (otherGrid.GridSizeEnum == MyCubeSize.Large)
                    velocityRelayStatic *= 4f;
                  rigidBody.LinearVelocity = Vector3.Lerp(linearVelocity, Vector3.Zero, velocityRelayStatic);
                }
                else
                {
                  if (isLargeGrid)
                    return;
                  rigidBody.ApplyPointImpulse(-velocityMass * ((float) destroyed / 40f), cpPosition);
                }
              }
              else
              {
                if (otherGrid == null || this.m_grid.GridSizeEnum == otherGrid.GridSizeEnum || isLargeGrid)
                  return;
                Vector3 vector3 = Vector3.Lerp(linearVelocity, otherGrid.Physics.LinearVelocity, MyFakes.DEFORMATION_VELOCITY_RELAY);
                int num = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
                rigidBody.LinearVelocity = vector3;
              }
            }
          }
        }
      }
      if (!Sync.IsServer)
        return num8 != 0;
      if (!MyFakes.DEFORMATION_EXPLOSIONS)
        return num8 != 0;
      if (this.m_grid.BlocksCount <= 10)
        return num8 != 0;
      if (flag4)
        return num8 != 0;
      bool flag8 = this.m_grid.GridSizeEnum == MyCubeSize.Large;
      float num14 = Math.Min(1f, num5 / 20f);
      float num15 = (float) ((1.0 - (double) this.m_impactDot) * 0.600000023841858 + (flag8 ? 1.39999997615814 : 1.5));
      float num16 = flag8 ? 0.25f : 0.06f;
      float num17 = flag8 ? 0.3f : 0.4f;
      bool flag9 = !Vector3.IsZero(this.m_cachedGravity);
      float num18 = this.m_grid.GridSizeEnum == MyCubeSize.Small ? MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER_SMALL_GRID : MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER;
      float num19 = Math.Min((this.m_grid.GridSize + (float) Math.Sqrt((double) destroyed) * num16) * num15 * num14 * num18, MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS * num15);
      Vector3D vector3D2 = pt.ContactPosition + vector2 * num19 * 0.5f;
      Vector3D vector3D3 = vector3D2 + vector1 * num19 * 0.95f / (flag9 ? 2f : 1f);
      float num20 = (float) ((double) num15 * 0.75 - (double) this.m_impactDot * (double) num17);
      float num21 = Math.Min(num19 * num20 * MathHelper.Lerp(0.4f, 1f, Math.Min(1f, num5 / 50f)), (float) ((double) MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS * (double) num15 * 1.5));
      if (flag9 && (double) this.m_impactDot > 0.7)
        num21 *= 1.35f;
      MyGridPhysics.ExplosionInfo info = new MyGridPhysics.ExplosionInfo();
      info.Position = vector3D2;
      info.ExplosionType = MyExplosionTypeEnum.GRID_DESTRUCTION;
      info.Radius = num19;
      info.ShowParticles = false;
      info.GenerateDebris = true;
      this.EnqueueExplosion(in info);
      ulong num22 = MySandboxGame.Static.SimulationFrameCounter - this.m_frameFirstImpact;
      if (flag9 && (double) this.m_impactDot < 0.7 && (isLargeGrid && num22 < 10UL || !isLargeGrid))
      {
        info = new MyGridPhysics.ExplosionInfo();
        info.Position = vector3D3;
        info.ExplosionType = MyExplosionTypeEnum.GRID_DESTRUCTION;
        info.Radius = num21;
        info.ShowParticles = false;
        info.GenerateDebris = true;
        this.EnqueueExplosion(in info);
      }
      int num23 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
      return num8 != 0;
    }

    public bool ApplyDeformation(
      float deformationOffset,
      float softAreaPlanar,
      float softAreaVertical,
      Vector3 localPos,
      Vector3 localNormal,
      MyStringHash damageType,
      float offsetThreshold = 0.0f,
      float lowerRatioLimit = 0.0f,
      long attackerId = 0)
    {
      return this.ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, localPos, localNormal, damageType, out int _, offsetThreshold, lowerRatioLimit, attackerId);
    }

    public bool ApplyDeformation(
      float deformationOffset,
      float softAreaPlanar,
      float softAreaVertical,
      Vector3 localPos,
      Vector3 localNormal,
      MyStringHash damageType,
      out int blocksDestroyedByThisCp,
      float offsetThreshold = 0.0f,
      float lowerRatioLimit = 0.0f,
      long attackerId = 0)
    {
      blocksDestroyedByThisCp = 0;
      bool flag = false;
      if (!this.m_grid.BlocksDestructionEnabled)
        return flag;
      float num1 = this.m_grid.GridSize * 0.7f;
      float num2 = localNormal.AbsMax() * deformationOffset;
      double num3 = 1.0 - (double) num1 / (double) num2;
      bool isServer = Sync.IsServer;
      float softAreaPlanarInv = 1f / softAreaPlanar;
      float softAreaVerticalInv = 1f / softAreaVertical;
      Vector3I gridPos = Vector3I.Round((localPos + this.m_grid.GridSize / 2f) / this.m_grid.GridSize);
      Vector3D axis = (Vector3D) localNormal;
      Vector3 up = (Vector3) MyUtils.GetRandomPerpendicularVector(ref axis);
      Vector3 vector3_1 = Vector3.Cross(up, localNormal);
      MyDamageInformation info = new MyDamageInformation(true, 1f, MyDamageType.Deformation, attackerId);
      if (num3 > 0.0)
      {
        float num4 = softAreaVertical;
        float num5 = softAreaPlanar - num1 * softAreaPlanar / num2;
        Vector3 vector3_2 = up * num5;
        Vector3 vector3_3 = vector3_1 * num5;
        Vector3 vector3_4 = localPos - vector3_2 - vector3_3;
        Vector3 vector3_5 = localPos - vector3_2 + vector3_3;
        Vector3 vector3_6 = localPos + vector3_2 - vector3_3;
        Vector3 vector3_7 = localPos + vector3_2 + vector3_3;
        Vector3 vector3_8 = localPos + localNormal * num4;
        BoundingBoxI invalid1 = BoundingBoxI.CreateInvalid();
        invalid1.Include(Vector3I.Round(vector3_8 / this.m_grid.GridSize));
        invalid1.Include(Vector3I.Round(vector3_4 / this.m_grid.GridSize));
        invalid1.Include(Vector3I.Round(vector3_6 / this.m_grid.GridSize));
        invalid1.Include(Vector3I.Round(vector3_5 / this.m_grid.GridSize));
        invalid1.Include(Vector3I.Round(vector3_7 / this.m_grid.GridSize));
        float val1 = 1f;
        BoundingBoxI invalid2 = BoundingBoxI.CreateInvalid();
        Vector3I vector3I1 = Vector3I.Max(invalid1.Min, this.m_grid.Min);
        Vector3I vector3I2 = Vector3I.Min(invalid1.Max, this.m_grid.Max);
        Vector3I vector3I3;
        for (vector3I3.X = vector3I1.X; vector3I3.X <= vector3I2.X; ++vector3I3.X)
        {
          for (vector3I3.Y = vector3I1.Y; vector3I3.Y <= vector3I2.Y; ++vector3I3.Y)
          {
            for (vector3I3.Z = vector3I1.Z; vector3I3.Z <= vector3I2.Z; ++vector3I3.Z)
            {
              float num6 = 1f;
              if (vector3I3 != gridPos)
              {
                Vector3 closestCorner = this.m_grid.GetClosestCorner(vector3I3, localPos);
                num6 = MyGridPhysics.CalculateSoften(softAreaPlanarInv, softAreaVerticalInv, ref localNormal, closestCorner - localPos);
                if ((double) num6 == 0.0)
                  continue;
              }
              float num7 = num2 * num6;
              if ((double) num7 > (double) num1)
              {
                MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(vector3I3);
                if (cubeBlock != null)
                {
                  if (isServer)
                  {
                    if (cubeBlock.UseDamageSystem)
                    {
                      info.Amount = 1f;
                      MyDamageSystem.Static.RaiseBeforeDamageApplied((object) cubeBlock, ref info);
                      if ((double) info.Amount == 0.0)
                        continue;
                    }
                    val1 = Math.Min(val1, cubeBlock.DeformationRatio);
                    if ((double) Math.Max(lowerRatioLimit, cubeBlock.DeformationRatio) * (double) num7 > (double) num1)
                    {
                      flag = true;
                      if (this.m_removedCubes.TryAdd(cubeBlock, (byte) 0))
                      {
                        int num8 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
                        ++blocksDestroyedByThisCp;
                        invalid2.Include(ref cubeBlock.Min);
                        invalid2.Include(ref cubeBlock.Max);
                      }
                    }
                  }
                  else
                  {
                    val1 = Math.Min(val1, cubeBlock.DeformationRatio);
                    if ((double) Math.Max(lowerRatioLimit, cubeBlock.DeformationRatio) * (double) num7 > (double) num1)
                    {
                      flag = true;
                      ++blocksDestroyedByThisCp;
                    }
                  }
                }
              }
            }
          }
        }
        if (blocksDestroyedByThisCp > 0)
        {
          if (isServer)
          {
            invalid2.Inflate(1);
            this.m_dirtyCubesInfo.DirtyParts.Add(invalid2);
            this.ScheduleRemoveBlocksCallbacks();
            this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateShape), 24, true);
          }
        }
        else
        {
          float num6 = Math.Max(val1, 0.2f);
          softAreaPlanar *= num6;
          softAreaVertical *= num6;
        }
      }
      if (blocksDestroyedByThisCp == 0 && MySession.Static.HighSimulationQuality)
        Parallel.Start((Action) (() => this.DeformBones(deformationOffset, gridPos, softAreaPlanar, softAreaVertical, localNormal, localPos, damageType, offsetThreshold, lowerRatioLimit, attackerId, up)), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Deformations, "DeformBones"));
      return flag;
    }

    private void ScheduleRemoveBlocksCallbacks()
    {
      if (Interlocked.Exchange(ref this.m_removeBlocksCallbackScheduled, 1) != 0)
        return;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        this.m_removeBlocksCallbackScheduled = 0;
        bool flag = true;
        while (flag)
        {
          flag = false;
          foreach (KeyValuePair<MySlimBlock, byte> removedCube in this.m_removedCubes)
          {
            flag = true;
            MySlimBlock key = removedCube.Key;
            this.m_removedCubes.Remove<MySlimBlock, byte>(key);
            if (!key.IsDestroyed)
              key.CubeGrid.RemoveDestroyedBlock(key, 0L);
          }
        }
      }), "ApplyDeformation/RemoveDestroyedBlock");
    }

    private void DeformBones(
      float deformationOffset,
      Vector3I gridPos,
      float softAreaPlanar,
      float softAreaVertical,
      Vector3 localNormal,
      Vector3 localPos,
      MyStringHash damageType,
      float offsetThreshold,
      float lowerRatioLimit,
      long attackerId,
      Vector3 up)
    {
      if (!MySession.Static.Ready)
        return;
      if (MyGridPhysics.m_tmpCubeList == null)
        MyGridPhysics.m_tmpCubeList = new List<Vector3I>(8);
      float softAreaVerticalInv = 1f / softAreaVertical;
      float softAreaPlanarInv = 1f / softAreaPlanar;
      float num1 = 1f / this.m_grid.GridSize;
      float num2 = this.m_grid.GridSize * 0.5f;
      Vector3I vector3I1 = Vector3I.Round((localPos + new Vector3(this.m_grid.GridSizeHalf)) / num2) - gridPos * 2;
      float num3 = (float) ((double) softAreaPlanar * (double) num1 * 2.0);
      float z = (float) ((double) softAreaVertical * (double) num1 * 2.0);
      BoundingBox aabb = new MyOrientedBoundingBox((Vector3) (gridPos * 2 + vector3I1), new Vector3(num3, num3, z), Quaternion.CreateFromForwardUp(localNormal, up)).GetAABB();
      Vector3I vector3I2 = Vector3I.Floor((Vector3I.Floor(aabb.Min) - Vector3I.One) * 0.5f);
      Vector3I vector3I3 = Vector3I.Ceiling((Vector3I.Ceiling(aabb.Max) - Vector3I.One) * 0.5f);
      Vector3I vector3I4 = Vector3I.Max(vector3I2, this.m_grid.Min);
      Vector3I vector3I5 = Vector3I.Min(vector3I3, this.m_grid.Max);
      bool isServer = Sync.IsServer;
      Vector3I vector3I6 = gridPos * 2;
      Vector3 vector3_1 = new Vector3(this.m_grid.GridSize * 0.25f);
      float num4 = this.m_grid.GridSize * 0.7f;
      offsetThreshold /= this.m_grid.GridSizeEnum == MyCubeSize.Large ? 1f : 5f;
      bool flag1 = false;
      HashSet<Vector3I> dirtyCubes = MyGridPhysics.m_dirtyCubesPool.Get();
      Dictionary<MySlimBlock, float> damagedCubes = MyGridPhysics.m_damagedCubesPool.Get();
      MyDamageInformation damageInfo = new MyDamageInformation(true, 1f, MyDamageType.Deformation, attackerId);
      Vector3I vector3I7 = Vector3I.Max(vector3I4, this.m_grid.Min);
      Vector3I vector3I8 = Vector3I.Min(vector3I5, this.m_grid.Max);
      Vector3I cube1;
      for (cube1.X = vector3I7.X; cube1.X <= vector3I8.X; ++cube1.X)
      {
        for (cube1.Y = vector3I7.Y; cube1.Y <= vector3I8.Y; ++cube1.Y)
        {
          for (cube1.Z = vector3I7.Z; cube1.Z <= vector3I8.Z; ++cube1.Z)
          {
            MySlimBlock boneDeformations = this.m_grid.GetExistingCubeForBoneDeformations(ref cube1, ref damageInfo);
            if (boneDeformations != null && !boneDeformations.IsDestroyed && !this.m_removedCubes.ContainsKey(boneDeformations))
            {
              bool flag2 = true;
              for (int index = 0; index < MyGridSkeleton.BoneOffsets.Length; ++index)
              {
                Vector3I boneOffset1 = MyGridSkeleton.BoneOffsets[index];
                Vector3I pos = cube1 * 2 + boneOffset1;
                Vector3 vector3_2 = pos * this.m_grid.GridSize * 0.5f - vector3_1;
                Vector3 bone;
                this.m_grid.Skeleton.GetBone(ref pos, out bone);
                float soften = MyGridPhysics.CalculateSoften(softAreaPlanarInv, softAreaVerticalInv, ref localNormal, bone + vector3_2 - localPos);
                if ((double) soften == 0.0)
                {
                  if (flag2 && index >= 7)
                    break;
                }
                else
                {
                  flag2 = false;
                  float num5 = Math.Max(lowerRatioLimit, boneDeformations.DeformationRatio);
                  if ((double) deformationOffset * (double) num5 >= (double) offsetThreshold)
                  {
                    float num6 = deformationOffset * soften;
                    float num7 = num6 * num5;
                    Vector3 vector3_3 = localNormal * num7;
                    bone += vector3_3;
                    float num8 = bone.AbsMax();
                    if ((!(damageType != MyDamageType.Bullet) || !(damageType != MyDamageType.Drill) ? ((double) num8 < (double) num4 ? 1 : 0) : 1) != 0 && (double) num7 > 0.0500000007450581)
                    {
                      Vector3I boneOffset2 = pos - vector3I6;
                      if ((double) num8 > (double) num4)
                      {
                        MyGridPhysics.m_tmpCubeList.Clear();
                        Vector3I boneOffset3 = boneOffset2;
                        Vector3I cube2 = gridPos;
                        this.m_grid.Skeleton.Wrap(ref cube2, ref boneOffset3);
                        this.m_grid.Skeleton.GetAffectedCubes(cube2, boneOffset3, MyGridPhysics.m_tmpCubeList, this.m_grid);
                        bool flag3 = true;
                        foreach (Vector3I tmpCube in MyGridPhysics.m_tmpCubeList)
                        {
                          MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(tmpCube);
                          if (cubeBlock != null && !cubeBlock.IsDestroyed)
                          {
                            float num9 = Math.Max(lowerRatioLimit, cubeBlock.DeformationRatio);
                            flag3 = ((flag3 ? 1 : 0) & ((double) num6 * (double) num9 <= (double) num4 ? 0 : (cubeBlock.UsesDeformation ? 1 : 0))) != 0;
                          }
                        }
                        if (flag3)
                        {
                          foreach (Vector3I tmpCube in MyGridPhysics.m_tmpCubeList)
                          {
                            MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(tmpCube);
                            if (cubeBlock != null && !cubeBlock.IsDestroyed)
                            {
                              int num9 = MyFakes.DEFORMATION_LOGGING ? 1 : 0;
                              if (isServer)
                              {
                                flag1 = true;
                                this.m_removedCubes.TryAdd(cubeBlock, (byte) 0);
                                damagedCubes.Remove(cubeBlock);
                              }
                              else
                                this.AddDirtyBlock(cubeBlock);
                            }
                          }
                        }
                      }
                      else
                      {
                        dirtyCubes.Add(cube1);
                        if (isServer)
                        {
                          this.m_grid.Skeleton.SetBone(ref pos, ref bone);
                          this.m_grid.AddDirtyBone(gridPos, boneOffset2);
                          this.m_grid.AddBoneToSend(pos);
                          if (damageType != MyDamageType.Bullet)
                          {
                            double num9 = (double) ((IMyDestroyableObject) boneDeformations).Integrity / (double) boneDeformations.MaxIntegrity;
                            float num10 = (float) (1.0 - (double) num8 / (double) num4);
                            float val1 = boneDeformations.MaxIntegrity * (1f - num10) - boneDeformations.CurrentDamage;
                            if ((double) val1 > 0.0)
                            {
                              float val2;
                              damagedCubes.TryGetValue(boneDeformations, out val2);
                              val2 = Math.Max(val1, val2);
                              damagedCubes[boneDeformations] = val2;
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      if (flag1)
        this.ScheduleRemoveBlocksCallbacks();
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        bool flag2 = true;
        if (MySession.Static.Ready)
        {
          this.m_dirtyCubesInfo.DirtyBlocks.UnionWith((IEnumerable<Vector3I>) dirtyCubes);
          this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateShape), 24, true);
          foreach (KeyValuePair<MySlimBlock, float> keyValuePair in damagedCubes)
          {
            if (!keyValuePair.Key.IsDestroyed && !keyValuePair.Key.CubeGrid.Closed)
              keyValuePair.Key.DoDamage(keyValuePair.Value, damageType, false, attackerId: attackerId);
          }
          if (isServer)
          {
            flag2 = false;
            Parallel.Start((Action) (() =>
            {
              MySlimBlock.SendDamageBatch(damagedCubes, damageType, attackerId);
              MyGridPhysics.m_damagedCubesPool.Return(damagedCubes);
            }));
          }
        }
        MyGridPhysics.m_dirtyCubesPool.Return(dirtyCubes);
        if (!flag2)
          return;
        MyGridPhysics.m_damagedCubesPool.Return(damagedCubes);
      }), nameof (DeformBones));
    }

    private void AddGridCollisionEffect(
      Vector3D relativePosition,
      Vector3 normal,
      Vector3 relativeVelocity,
      float separatingSpeed,
      float impulse)
    {
      if (!MyFakes.ENABLE_COLLISION_EFFECTS || this.m_gridEffects.Count >= MyGridPhysics.MaxEffectsPerFrame)
        return;
      this.m_gridCollisionEffects.Enqueue(new MyGridPhysics.GridCollisionHit()
      {
        RelativePosition = relativePosition,
        Normal = normal,
        RelativeVelocity = relativeVelocity,
        SeparatingSpeed = separatingSpeed,
        Impulse = impulse
      });
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.m_gridCollisionEffects.Count <= 0)
          return;
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ProcessGridCollisionEffects), 22);
      }), nameof (AddGridCollisionEffect));
    }

    private void AddCollisionEffect(
      Vector3D position,
      Vector3 normal,
      float separatingSpeed,
      float impulse)
    {
      if (!MyFakes.ENABLE_COLLISION_EFFECTS || this.m_gridEffects.Count >= MyGridPhysics.MaxEffectsPerFrame)
        return;
      this.m_gridEffects.Enqueue(new MyGridPhysics.GridEffect()
      {
        Type = MyGridPhysics.GridEffectType.Collision,
        Position = position,
        Normal = normal,
        Scale = 1f,
        SeparatingSpeed = separatingSpeed,
        Impulse = impulse
      });
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.m_gridEffects.Count <= 0)
          return;
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ProcessGridEffects), 22);
      }), nameof (AddCollisionEffect));
    }

    private void AddDustEffect(Vector3D position, float scale)
    {
      if (this.m_gridEffects.Count >= MyGridPhysics.MaxEffectsPerFrame)
        return;
      this.m_gridEffects.Enqueue(new MyGridPhysics.GridEffect()
      {
        Type = MyGridPhysics.GridEffectType.Dust,
        Position = position,
        Normal = Vector3.Forward,
        Scale = scale
      });
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.m_gridEffects.Count <= 0)
          return;
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ProcessGridEffects), 22);
      }), nameof (AddDustEffect));
    }

    private void AddDestructionEffect(Vector3D position, Vector3 direction)
    {
      if (!MyFakes.ENABLE_DESTRUCTION_EFFECTS || this.m_gridEffects.Count >= MyGridPhysics.MaxEffectsPerFrame)
        return;
      this.m_gridEffects.Enqueue(new MyGridPhysics.GridEffect()
      {
        Type = MyGridPhysics.GridEffectType.Destruction,
        Position = position,
        Normal = direction,
        Scale = 1f
      });
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.m_gridEffects.Count <= 0)
          return;
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ProcessGridEffects), 22);
      }), nameof (AddDestructionEffect));
    }

    public static float GetCollisionSparkMultiplier(float separatingVelocity, bool isLargeGrid)
    {
      float num1 = 0.1f;
      float num2 = 2f;
      float num3 = 110f;
      float num4 = separatingVelocity / num3;
      float num5 = (float) ((double) num4 * (double) num2 + (1.0 - (double) num4) * (double) num1);
      return isLargeGrid ? num5 * 2f : num5;
    }

    public static float GetCollisionSparkScale(float impulseApplied, bool isLargeGrid)
    {
      if ((double) impulseApplied < 1000.0)
        return (float) (0.0500000007450581 + 4.99999987368938E-05 * (double) impulseApplied);
      if ((double) impulseApplied < 60000.0)
        return (float) (0.100000001490116 + 9.0000003183377E-06 * ((double) impulseApplied - 1000.0));
      return (double) impulseApplied < 1000000.0 ? (float) (0.629999995231628 + 6.99999986863986E-07 * ((double) impulseApplied - 60000.0)) : 1.3f;
    }

    private void CreateGridCollisionEffect(MyGridPhysics.GridCollisionHit e)
    {
      double num = 0.5;
      for (int index = 0; index < this.m_collisionParticles.Count; ++index)
      {
        MyGridPhysics.CollisionParticleEffect collisionParticle = this.m_collisionParticles[index];
        if (Vector3D.DistanceSquared(collisionParticle.RelativePosition, e.RelativePosition) < num)
        {
          if (collisionParticle.RemainingTime >= 20 && (double) collisionParticle.Impulse >= (double) e.Impulse)
            return;
          collisionParticle.RelativePosition = e.RelativePosition;
          collisionParticle.SeparatingVelocity = e.RelativeVelocity;
          collisionParticle.Normal = e.Normal;
          collisionParticle.RemainingTime = 20;
          collisionParticle.Impulse = e.Impulse;
          return;
        }
      }
      if (this.m_collisionParticles.Count >= 3)
        return;
      this.m_collisionParticles.Add(new MyGridPhysics.CollisionParticleEffect()
      {
        Effect = (MyParticleEffect) null,
        RemainingTime = 20,
        Normal = e.Normal,
        SeparatingVelocity = e.RelativeVelocity,
        RelativePosition = e.RelativePosition,
        Impulse = e.Impulse
      });
      if (this.m_collisionParticles.Count != 1)
        return;
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateCollisionParticleEffects), 22);
    }

    private void CreateEffect(MyGridPhysics.GridEffect e)
    {
      Vector3D position = e.Position;
      Vector3 normal = e.Normal;
      float scale1 = e.Scale;
      switch (e.Type)
      {
        case MyGridPhysics.GridEffectType.Collision:
          float num1 = (float) Vector3D.DistanceSquared(MySector.MainCamera.Position, position);
          float scale2 = MyPerGameSettings.CollisionParticle.Scale;
          float collisionSparkMultiplier = MyGridPhysics.GetCollisionSparkMultiplier(e.SeparatingSpeed, this.m_grid.GridSizeEnum == MyCubeSize.Large);
          float num2 = 0.5f * MyGridPhysics.GetCollisionSparkScale(e.Impulse, this.m_grid.GridSizeEnum == MyCubeSize.Large);
          string effectName = this.m_grid.GridSizeEnum != MyCubeSize.Large ? MyPerGameSettings.CollisionParticle.SmallGridClose : ((double) num1 > (double) MyPerGameSettings.CollisionParticle.CloseDistanceSq ? MyPerGameSettings.CollisionParticle.LargeGridDistant : MyPerGameSettings.CollisionParticle.LargeGridClose);
          MatrixD fromDir = MatrixD.CreateFromDir((Vector3D) normal);
          MyParticleEffect effect1;
          if (!MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(position, fromDir.Forward, fromDir.Up), out effect1))
            break;
          effect1.UserScale = scale2 * num2;
          effect1.UserBirthMultiplier = collisionSparkMultiplier;
          break;
        case MyGridPhysics.GridEffectType.Destruction:
          float scale3 = MyPerGameSettings.DestructionParticle.Scale;
          if (this.m_grid.GridSizeEnum != MyCubeSize.Large)
            scale3 = 0.05f;
          MySyncDestructions.AddDestructionEffect(MyPerGameSettings.DestructionParticle.DestructionSmokeLarge, position, normal, scale3);
          break;
        case MyGridPhysics.GridEffectType.Dust:
          MyParticleEffect effect2;
          if (!MyParticlesManager.TryCreateParticleEffect("PlanetCrashDust", MatrixD.CreateTranslation(position), out effect2))
            break;
          effect2.UserScale = scale1;
          break;
      }
    }

    public static void CreateDestructionEffect(
      string effectName,
      Vector3D position,
      Vector3 direction,
      float scale)
    {
      MatrixD fromDir = MatrixD.CreateFromDir((Vector3D) direction);
      MyParticleEffect effect;
      if (!MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(position, fromDir.Forward, fromDir.Up), out effect))
        return;
      effect.UserScale = scale;
    }

    private void ProcessGridEffects()
    {
      while (this.m_gridEffects.Count > 0)
        this.CreateEffect(this.m_gridEffects.Dequeue());
    }

    private void ProcessGridCollisionEffects()
    {
      while (this.m_gridCollisionEffects.Count > 0)
        this.CreateGridCollisionEffect(this.m_gridCollisionEffects.Dequeue());
    }

    public void UpdateCollisionParticleEffects()
    {
      int index = 0;
      while (index < this.m_collisionParticles.Count)
      {
        MyGridPhysics.CollisionParticleEffect collisionParticle = this.m_collisionParticles[index];
        this.UpdateCollisionParticleEffect(ref collisionParticle);
        if (collisionParticle.RemainingTime < 0)
        {
          this.FinalizeCollisionParticleEffect(ref collisionParticle);
          this.m_collisionParticles.RemoveAt(index);
        }
        else
          ++index;
      }
      if (this.m_collisionParticles.Count != 0)
        return;
      this.m_grid.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateCollisionParticleEffects));
    }

    private float ComputeDirecionalSparkMultiplier(float speed)
    {
      float num1 = 1f;
      float num2 = 10f;
      float num3 = speed / 110f;
      return (float) ((double) num3 * (double) num2 + (1.0 - (double) num3) * (double) num1);
    }

    private float ComputeDirecionalSparkScale(float impulse)
    {
      if ((double) impulse < 1000.0)
        return 1f;
      if ((double) impulse < 10000.0)
        return (float) (1.0 + 4.99999987368938E-05 * ((double) impulse - 1000.0));
      return (double) impulse < 100000.0 ? (float) (1.45000004768372 + 1.49999996210681E-05 * ((double) impulse - 10000.0)) : 2.8f;
    }

    private void UpdateCollisionParticleEffect(
      ref MyGridPhysics.CollisionParticleEffect effect,
      bool countDown = true)
    {
      if (effect.RemainingTime >= 20)
      {
        float speed = effect.SeparatingVelocity.Length();
        Vector3 vector1 = effect.SeparatingVelocity / speed;
        Vector3 forward = -vector1 + 1.1f * Vector3.Dot(vector1, effect.Normal) * effect.Normal;
        if (effect.Effect == null)
          MyParticlesManager.TryCreateParticleEffect("Collision_Sparks_Directional", MatrixD.CreateWorld(effect.RelativePosition + this.RigidBody.Position, forward, effect.Normal), out effect.Effect);
        if (effect.Effect != null && (HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
        {
          effect.Effect.WorldMatrix = MatrixD.CreateWorld(effect.RelativePosition + this.RigidBody.Position, forward, effect.Normal);
          effect.Effect.UserBirthMultiplier = this.ComputeDirecionalSparkMultiplier(speed);
          effect.Effect.UserScale = 0.5f * this.ComputeDirecionalSparkScale(effect.Impulse);
        }
      }
      else if (effect.Effect != null && (HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        Vector3D trans = effect.RelativePosition + this.RigidBody.Position;
        effect.Effect.SetTranslation(ref trans);
      }
      if (!countDown)
        return;
      --effect.RemainingTime;
    }

    private void FinalizeCollisionParticleEffect(ref MyGridPhysics.CollisionParticleEffect effect)
    {
      if (effect.Effect == null)
        return;
      effect.Effect.Stop(false);
    }

    public override MyStringHash GetMaterialAt(Vector3D worldPos)
    {
      Vector3D fractionalGridPosition = Vector3.Transform((Vector3) worldPos, this.m_grid.PositionComp.WorldMatrixNormalizedInv) / (double) this.m_grid.GridSize;
      Vector3I cube;
      this.m_grid.FixTargetCubeLite(out cube, fractionalGridPosition);
      MySlimBlock mySlimBlock = this.m_grid.GetCubeBlock(cube);
      if (mySlimBlock == null)
        return base.GetMaterialAt(worldPos);
      if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
        mySlimBlock = ((MyCompoundCubeBlock) mySlimBlock.FatBlock).GetBlocks()[0];
      MyStringHash subtypeId = mySlimBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
      return !(subtypeId != MyStringHash.NullOrEmpty) ? base.GetMaterialAt(worldPos) : subtypeId;
    }

    public void AddDirtyBlock(MySlimBlock block)
    {
      this.m_dirtyCubesInfo.DirtyParts.Add(new BoundingBoxI()
      {
        Min = block.Min,
        Max = block.Max
      });
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateShape), 24, true);
    }

    public void AddDirtyArea(Vector3I min, Vector3I max)
    {
      this.m_dirtyCubesInfo.DirtyParts.Add(new BoundingBoxI()
      {
        Min = min,
        Max = max
      });
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateShape), 24, true);
    }

    public bool IsDirty() => this.m_dirtyCubesInfo.DirtyBlocks.Count > 0 || !this.m_dirtyCubesInfo.DirtyParts.IsEmpty;

    public List<HkShape> GetShapesFromPosition(Vector3I pos) => this.m_shape.GetShapesFromPosition(pos);

    public void UpdateShape()
    {
      if (!this.m_grid.CanHavePhysics())
        return;
      HashSet<Vector3I> dirtyBlocks = this.m_dirtyCubesInfo.DirtyBlocks;
      this.m_dirtyCubesInfo.DirtyParts.ApplyChanges();
      BoundingBox dirtyBox = BoundingBox.CreateInvalid();
      bool dirtyParts = this.m_dirtyCubesInfo.DirtyParts.Count > 0;
      foreach (BoundingBoxI dirtyPart in this.m_dirtyCubesInfo.DirtyParts)
      {
        Vector3I vector3I;
        for (vector3I.X = dirtyPart.Min.X; vector3I.X <= dirtyPart.Max.X; ++vector3I.X)
        {
          for (vector3I.Y = dirtyPart.Min.Y; vector3I.Y <= dirtyPart.Max.Y; ++vector3I.Y)
          {
            for (vector3I.Z = dirtyPart.Min.Z; vector3I.Z <= dirtyPart.Max.Z; ++vector3I.Z)
            {
              dirtyBlocks.Add(vector3I);
              dirtyBox = dirtyBox.Include(vector3I * this.m_grid.GridSize);
            }
          }
        }
      }
      bool flag = dirtyBlocks.Count > 0;
      if (flag)
        this.UpdateContactCallbackLimit();
      if (!this.m_recreateBody)
      {
        if (flag)
        {
          if (this.RigidBody.IsActive && !this.HavokWorld.ActiveRigidBodies.Contains(this.RigidBody))
            this.HavokWorld.RigidBodyActivated((HkEntity) this.RigidBody);
          this.m_shape.UnmarkBreakable((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null ? this.WeldedRigidBody : this.RigidBody);
          this.m_shape.RefreshBlocks((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null ? this.WeldedRigidBody : this.RigidBody, dirtyBlocks);
          if (Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
          {
            Sandbox.Game.Entities.MyEntities.InvokeLater((Action) (() =>
            {
              this.OnRefreshComplete();
              ActivateInArea();
            }));
          }
          else
          {
            this.OnRefreshComplete();
            ActivateInArea();
          }
        }
      }
      else
      {
        this.RecreateBreakableBody(dirtyBlocks);
        this.m_recreateBody = false;
        this.m_grid.RaisePhysicsChanged();
      }
      this.m_dirtyCubesInfo.Clear();

      void ActivateInArea()
      {
        if (!dirtyParts)
          return;
        dirtyBox.Inflate(0.5f + this.m_grid.GridSize);
        BoundingBoxD box = dirtyBox.Transform(this.m_grid.WorldMatrix);
        MyPhysics.ActivateInBox(ref box);
      }
    }

    private void EnqueueExplosion(in MyGridPhysics.ExplosionInfo info)
    {
      this.m_explosions.Add(info);
      if (this.m_explosions.Count != 1)
        return;
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateExplosions), 22);
    }

    private void UpdateExplosions()
    {
      this.m_debrisPerFrame = 0;
      if (this.m_explosions.Count <= 0)
        return;
      if (Sync.IsServer)
      {
        this.m_grid.PerformCutouts(this.m_explosions);
        float initialSpeed = this.m_grid.Physics.LinearVelocity.Length();
        foreach (MyGridPhysics.ExplosionInfo explosion in this.m_explosions)
        {
          if ((double) initialSpeed > 0.0 && explosion.GenerateDebris && this.m_debrisPerFrame < 3)
          {
            MyDebris.Static.CreateDirectedDebris((Vector3) explosion.Position, this.m_grid.Physics.LinearVelocity / initialSpeed, this.m_grid.GridSize, this.m_grid.GridSize * 1.5f, 0.0f, 1.570796f, 6, initialSpeed);
            ++this.m_debrisPerFrame;
          }
        }
      }
      this.m_explosions.Clear();
    }

    private void OnRefreshComplete()
    {
      this.m_shape.MarkBreakable((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null ? this.WeldedRigidBody : this.RigidBody);
      this.m_grid.SetInventoryMassDirty();
      this.m_shape.SetMass((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null ? this.WeldedRigidBody : this.RigidBody);
      this.m_shape.UpdateShape((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null ? this.WeldedRigidBody : this.RigidBody, (HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null ? (HkRigidBody) null : this.RigidBody2, this.BreakableBody);
      MyGridPhysicalHierarchy.Static.UpdateRoot(this.m_grid);
      this.m_grid.RaisePhysicsChanged();
    }

    public void UpdateMass()
    {
      if (this.RigidBody.GetMotionType() == HkMotionType.Keyframed)
        return;
      float mass = this.RigidBody.Mass;
      this.m_shape.RefreshMass();
      if ((double) this.RigidBody.Mass != (double) mass && !this.RigidBody.IsActive)
        this.RigidBody.Activate();
      this.m_grid.RaisePhysicsChanged();
      MyGridPhysicalHierarchy.Static.UpdateRoot(this.m_grid);
    }

    public void AddBlock(MySlimBlock block)
    {
      Vector3I vector3I;
      for (vector3I.X = block.Min.X; vector3I.X <= block.Max.X; ++vector3I.X)
      {
        for (vector3I.Y = block.Min.Y; vector3I.Y <= block.Max.Y; ++vector3I.Y)
        {
          for (vector3I.Z = block.Min.Z; vector3I.Z <= block.Max.Z; ++vector3I.Z)
            this.m_dirtyCubesInfo.DirtyBlocks.Add(vector3I);
        }
      }
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateShape), 24, true);
    }

    protected override void CreateBody(ref HkShape shape, HkMassProperties? massProperties)
    {
      if (MyPerGameSettings.Destruction)
      {
        shape = this.CreateBreakableBody(shape, massProperties);
      }
      else
      {
        HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
        hkRigidBodyCinfo.AngularDamping = this.m_angularDamping;
        hkRigidBodyCinfo.LinearDamping = this.m_linearDamping;
        hkRigidBodyCinfo.Shape = shape;
        hkRigidBodyCinfo.SolverDeactivation = this.InitialSolverDeactivation;
        hkRigidBodyCinfo.ContactPointCallbackDelay = this.ContactPointDelay;
        if (massProperties.HasValue)
          hkRigidBodyCinfo.SetMassProperties(massProperties.Value);
        MyPhysicsBody.GetInfoFromFlags(hkRigidBodyCinfo, this.Flags);
        if (this.m_grid.IsStatic)
        {
          hkRigidBodyCinfo.MotionType = HkMotionType.Dynamic;
          hkRigidBodyCinfo.QualityType = HkCollidableQualityType.Moving;
        }
        this.RigidBody = new HkRigidBody(hkRigidBodyCinfo);
        if (!this.m_grid.IsStatic)
          return;
        this.RigidBody.UpdateMotionType(HkMotionType.Fixed);
      }
    }

    private static void DisconnectBlock(MySlimBlock a)
    {
      a.DisconnectFaces.Add(Vector3I.Left);
      a.DisconnectFaces.Add(Vector3I.Right);
      a.DisconnectFaces.Add(Vector3I.Forward);
      a.DisconnectFaces.Add(Vector3I.Backward);
      a.DisconnectFaces.Add(Vector3I.Up);
      a.DisconnectFaces.Add(Vector3I.Down);
    }

    private void AddFaces(MySlimBlock a, Vector3I ab)
    {
      if (!a.DisconnectFaces.Contains(ab * Vector3I.UnitX))
        a.DisconnectFaces.Add(ab * Vector3I.UnitX);
      if (!a.DisconnectFaces.Contains(ab * Vector3I.UnitY))
        a.DisconnectFaces.Add(ab * Vector3I.UnitY);
      if (a.DisconnectFaces.Contains(ab * Vector3I.UnitZ))
        return;
      a.DisconnectFaces.Add(ab * Vector3I.UnitZ);
    }

    public override void DebugDraw()
    {
      if (MyDebugDrawSettings.BREAKABLE_SHAPE_CONNECTIONS && (HkReferenceObject) this.BreakableBody != (HkReferenceObject) null)
      {
        MySlimBlock mySlimBlock = (MySlimBlock) null;
        List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
        MyPhysics.CastRay(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 25f, toList, 30);
        foreach (MyPhysics.HitInfo hitInfo in toList)
        {
          if (hitInfo.HkHitInfo.GetHitEntity() is MyCubeGrid)
          {
            MyCubeGrid hitEntity = hitInfo.HkHitInfo.GetHitEntity() as MyCubeGrid;
            mySlimBlock = hitEntity.GetCubeBlock(hitEntity.WorldToGridInteger(hitInfo.Position + MySector.MainCamera.ForwardVector * 0.2f));
            break;
          }
        }
        int num = 0;
        List<HkdConnection> resultList = new List<HkdConnection>();
        this.BreakableBody.BreakableShape.GetConnectionList(resultList);
        foreach (HkdConnection hkdConnection in resultList)
        {
          Vector3D world1 = this.ClusterToWorld(Vector3.Transform(hkdConnection.PivotA, this.RigidBody.GetRigidBodyMatrix()));
          Vector3D world2 = this.ClusterToWorld(Vector3.Transform(hkdConnection.PivotB, this.RigidBody.GetRigidBodyMatrix()));
          if (mySlimBlock != null && mySlimBlock.CubeGrid.WorldToGridInteger(world1) == mySlimBlock.Position)
          {
            world1 += (world2 - world1) * 0.0500000007450581;
            MyRenderProxy.DebugDrawLine3D(world1, world2, Color.Red, Color.Blue, false);
            MyRenderProxy.DebugDrawSphere(world2, 0.075f, Color.White, depthRead: false);
          }
          if (mySlimBlock != null && mySlimBlock.CubeGrid.WorldToGridInteger(world2) == mySlimBlock.Position)
          {
            Vector3D pointFrom = world1 + Vector3.One * 0.02f;
            Vector3D vector3D = world2 + Vector3.One * 0.02f;
            MyRenderProxy.DebugDrawLine3D(pointFrom, vector3D, Color.Red, Color.Green, false);
            MyRenderProxy.DebugDrawSphere(vector3D, 0.025f, Color.Green, depthRead: false);
          }
          if (num > 1000)
            break;
        }
      }
      this.Shape.DebugDraw();
      base.DebugDraw();
    }

    protected override void OnWelded(MyPhysicsBody other)
    {
      base.OnWelded(other);
      this.Shape.RefreshMass();
      if (this.m_grid.BlocksDestructionEnabled)
      {
        if (this.HavokWorld != null)
          this.HavokWorld.BreakOffPartsUtil.MarkEntityBreakable((HkEntity) this.RigidBody, this.Shape.BreakImpulse);
        if (Sync.IsServer)
        {
          if (this.RigidBody.BreakLogicHandler == null)
            this.RigidBody.BreakLogicHandler = new Havok.BreakLogicHandler(this.BreakLogicHandler);
          if (this.RigidBody.BreakPartsHandler == null)
            this.RigidBody.BreakPartsHandler = new Havok.BreakPartsHandler(this.BreakPartsHandler);
        }
      }
      this.m_grid.HavokSystemIDChanged(other.HavokCollisionSystemID);
    }

    protected override void OnUnwelded(MyPhysicsBody other)
    {
      base.OnUnwelded(other);
      this.Shape.RefreshMass();
      this.m_grid.HavokSystemIDChanged(this.HavokCollisionSystemID);
      if (this.m_grid.IsStatic)
        return;
      this.m_grid.RecalculateGravity();
    }

    public void ConvertToDynamic(bool doubledKinematic, bool isPredicted)
    {
      if ((HkReferenceObject) this.RigidBody == (HkReferenceObject) null || this.Entity == null || (this.Entity.Closed || this.HavokWorld == null))
        return;
      this.Flags = doubledKinematic ? RigidBodyFlag.RBF_DOUBLED_KINEMATIC : RigidBodyFlag.RBF_DEFAULT;
      bool flag = true;
      if (this.IsWelded || this.WeldInfo.Children.Count > 0)
      {
        if ((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null && this.WeldedRigidBody.Quality == HkCollidableQualityType.Fixed)
        {
          this.WeldedRigidBody.UpdateMotionType(HkMotionType.Dynamic);
          this.WeldedRigidBody.Quality = HkCollidableQualityType.Moving;
          if (doubledKinematic && !MyPerGameSettings.Destruction)
            this.WeldedRigidBody.Layer = 16;
          else
            this.WeldedRigidBody.Layer = 15;
        }
        MyWeldingGroups.Static.GetGroup((MyEntity) this.Entity).GroupData.UpdateParent(this.WeldInfo.Parent != null ? (MyEntity) this.WeldInfo.Parent.Entity : (MyEntity) this.Entity);
        flag &= this.WeldInfo.Parent == null;
      }
      if (flag)
      {
        HkMotionType type = Sync.IsServer || isPredicted ? HkMotionType.Dynamic : HkMotionType.Fixed;
        if (type != this.RigidBody.GetMotionType())
        {
          this.NotifyConstraintsRemovedFromWorld();
          this.RigidBody.UpdateMotionType(type);
          this.NotifyConstraintsAddedToWorld();
        }
        this.RigidBody.Quality = HkCollidableQualityType.Moving;
        if (doubledKinematic && !MyPerGameSettings.Destruction)
        {
          this.Flags = RigidBodyFlag.RBF_DOUBLED_KINEMATIC;
          this.RigidBody.Layer = 16;
        }
        else
        {
          this.Flags = RigidBodyFlag.RBF_DEFAULT;
          this.RigidBody.Layer = 15;
        }
        this.UpdateContactCallbackLimit();
        this.RigidBody.AddGravity();
        this.ActivateCollision();
        MyPhysics.RefreshCollisionFilter((MyPhysicsBody) this);
        this.RigidBody.Activate();
        if (this.RigidBody.InWorld)
        {
          this.HavokWorld.RigidBodyActivated((HkEntity) this.RigidBody);
          this.InvokeOnBodyActiveStateChanged(true);
        }
      }
      this.UpdateMass();
    }

    public void ConvertToStatic()
    {
      this.Flags = RigidBodyFlag.RBF_STATIC;
      bool flag = true;
      if (this.IsWelded || this.WeldInfo.Children.Count > 0)
      {
        if ((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null && this.WeldedRigidBody.Quality != HkCollidableQualityType.Fixed)
        {
          this.WeldedRigidBody.UpdateMotionType(HkMotionType.Fixed);
          this.WeldedRigidBody.Quality = HkCollidableQualityType.Fixed;
          this.WeldedRigidBody.Layer = 13;
        }
        MyWeldingGroups.Static.GetGroup((MyEntity) this.Entity).GroupData.UpdateParent(this.WeldInfo.Parent != null ? (MyEntity) this.WeldInfo.Parent.Entity : (MyEntity) this.Entity);
        flag &= this.WeldInfo.Parent == null;
      }
      this.UpdateMass();
      if (flag)
      {
        bool isActive = this.RigidBody.IsActive;
        this.NotifyConstraintsRemovedFromWorld();
        this.RigidBody.UpdateMotionType(HkMotionType.Fixed);
        this.RigidBody.Quality = HkCollidableQualityType.Fixed;
        this.RigidBody.Layer = 13;
        this.NotifyConstraintsAddedToWorld();
        this.ActivateCollision();
        MyPhysics.RefreshCollisionFilter((MyPhysicsBody) this);
        this.RigidBody.Activate();
        if (this.RigidBody.InWorld)
        {
          if (isActive)
            this.InvokeOnBodyActiveStateChanged(false);
          this.HavokWorld.RigidBodyActivated((HkEntity) this.RigidBody);
        }
        HkGroupFilter.GetSystemGroupFromFilterInfo(this.RigidBody.GetCollisionFilterInfo());
      }
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        if (this.RigidBody2.InWorld)
          this.HavokWorld.RemoveRigidBody(this.RigidBody2);
        this.RigidBody2.Dispose();
      }
      this.RigidBody2 = (HkRigidBody) null;
    }

    private void RigidBody_CollisionAddedCallbackClient(ref HkCollisionEvent e)
    {
      if (!this.PredictCollisions)
        return;
      MyEntity otherEntity = (MyEntity) e.GetOtherEntity((IMyEntity) this.m_grid);
      MyGridContactInfo.ContactFlags flag;
      switch (this.GetEligibilityForPredictedImpulses((IMyEntity) otherEntity, out flag))
      {
        case MyGridPhysics.PredictionDisqualificationReason.None:
          e.Disable();
          int nrContactPoints = e.NrContactPoints;
          for (int i = 0; i < nrContactPoints; ++i)
            e.GetContactPointPropertiesAt(i).SetFlag(flag);
          break;
        case MyGridPhysics.PredictionDisqualificationReason.EntityIsNotMoving:
          this.m_predictedContactEntities[otherEntity] = true;
          break;
      }
    }

    private void RigidBody_CollisionRemovedCallbackClient(ref HkCollisionEvent e)
    {
      MyEntity otherEntity = (MyEntity) e.GetOtherEntity((IMyEntity) this.m_grid);
      if (otherEntity == null || !this.m_predictedContactEntities.ContainsKey(otherEntity))
        return;
      this.m_predictedContactEntities[otherEntity] = false;
    }

    public bool AnyPredictedContactEntities()
    {
      BoundingBoxD box = this.Entity.PositionComp.WorldAABB.Inflate(5.0);
      foreach (KeyValuePair<MyEntity, bool> predictedContactEntity in this.m_predictedContactEntities)
      {
        if (predictedContactEntity.Key.MarkedForClose || !predictedContactEntity.Value && !predictedContactEntity.Key.PositionComp.WorldAABB.Intersects(ref box))
          MyGridPhysics.m_predictedContactEntitiesToRemove.Add(predictedContactEntity.Key);
      }
      foreach (MyEntity key in MyGridPhysics.m_predictedContactEntitiesToRemove)
        this.m_predictedContactEntities.Remove(key);
      MyGridPhysics.m_predictedContactEntitiesToRemove.Clear();
      return this.m_predictedContactEntities.Count > 0;
    }

    private void PredictContactImpulse(IMyEntity otherEntity, ref HkContactPointEvent e)
    {
      if (!e.FirstCallbackForFullManifold)
        return;
      MyGridContactInfo.ContactFlags flag = e.ContactProperties.GetFlags();
      if ((flag & (MyGridContactInfo.ContactFlags.PredictedCollision | MyGridContactInfo.ContactFlags.PredictedCollision_Disabled)) == (MyGridContactInfo.ContactFlags) 0)
      {
        switch (this.GetEligibilityForPredictedImpulses(otherEntity, out flag))
        {
          case MyGridPhysics.PredictionDisqualificationReason.None:
            e.ContactProperties.SetFlag(flag);
            break;
          case MyGridPhysics.PredictionDisqualificationReason.EntityIsNotMoving:
            this.MarkPredictedContactImpulse();
            return;
          default:
            return;
        }
      }
      if ((flag & MyGridContactInfo.ContactFlags.PredictedCollision_Disabled) != (MyGridContactInfo.ContactFlags) 0)
        return;
      this.MarkPredictedContactImpulse();
      float num1 = MyGridPhysics.PredictContactMass(otherEntity);
      float mass = MyGridPhysicalGroupData.GetGroupSharedProperties(this.m_grid, false).Mass;
      if ((double) num1 <= 0.0 || (double) mass <= 0.0)
        return;
      HkRigidBody rigidBody = this.RigidBody;
      int num2 = (HkReferenceObject) e.Base.BodyA == (HkReferenceObject) rigidBody ? 1 : 0;
      int bodyIndex = num2 != 0 ? 0 : 1;
      e.AccessVelocities(bodyIndex);
      float num3 = mass + num1;
      float num4 = (float) (1.0 - (double) mass / (double) num3);
      float num5 = (otherEntity.Physics.LinearVelocity - this.LinearVelocity).Length() * num3 * num4 * MyGridPhysics.PREDICTION_IMPULSE_SCALE;
      Vector3 vector3_1 = e.ContactPoint.Normal;
      if (num2 == 0)
        vector3_1 = -vector3_1;
      Vector3 vector3_2 = vector3_1 * num5 / mass;
      HkRigidBody hkRigidBody = rigidBody;
      hkRigidBody.LinearVelocity = hkRigidBody.LinearVelocity + vector3_2;
      e.UpdateVelocities(bodyIndex);
    }

    private static float PredictContactMass(IMyEntity entity)
    {
      if (entity is MyEntitySubpart)
      {
        entity = entity.Parent;
        if (entity is MyCubeBlock myCubeBlock)
          entity = (IMyEntity) myCubeBlock.CubeGrid;
      }
      if (entity is MyCubeGrid localGrid)
        return MyGridPhysicalGroupData.GetGroupSharedProperties(localGrid, false).Mass;
      if (entity is MyCharacter myCharacter)
        return myCharacter.CurrentMass;
      if (entity is MyDebrisBase || entity is MyFloatingObject)
        return 1f;
      return entity is MyInventoryBagEntity inventoryBagEntity ? inventoryBagEntity.Physics.Mass : 0.0f;
    }

    private MyGridPhysics.PredictionDisqualificationReason GetEligibilityForPredictedImpulses(
      IMyEntity otherEntity,
      out MyGridContactInfo.ContactFlags flag)
    {
      flag = (MyGridContactInfo.ContactFlags) 0;
      switch (otherEntity)
      {
        case null:
          return MyGridPhysics.PredictionDisqualificationReason.NoEntity;
        case MyVoxelBase _:
        case Sandbox.Game.WorldEnvironment.MyEnvironmentSector _:
          return MyGridPhysics.PredictionDisqualificationReason.EntityIsStatic;
        case MyCubeGrid myCubeGrid:
          if (MyFixedGrids.IsRooted(myCubeGrid))
            return MyGridPhysics.PredictionDisqualificationReason.EntityIsStatic;
          if (MyCubeGridGroups.Static.Physical.HasSameGroup(this.m_grid, myCubeGrid))
          {
            flag = MyGridContactInfo.ContactFlags.PredictedCollision_Disabled;
            return MyGridPhysics.PredictionDisqualificationReason.None;
          }
          if ((double) otherEntity.Physics.LinearVelocity.LengthSquared() < 1.0)
            return MyGridPhysics.PredictionDisqualificationReason.EntityIsNotMoving;
          break;
      }
      flag = MyGridContactInfo.ContactFlags.PredictedCollision;
      return MyGridPhysics.PredictionDisqualificationReason.None;
    }

    private void MarkPredictedContactImpulse()
    {
      ++this.PredictedContactsCounter;
      this.PredictedContactLastTime = MySandboxGame.Static.SimulationTime;
    }

    private bool IsTOIOptimized => this.m_savedQuality != HkCollidableQualityType.Invalid;

    public void UpdateTOIOptimizer()
    {
      if (!this.Enabled || (HkReferenceObject) this.RigidBody == (HkReferenceObject) null)
      {
        this.m_lastTOIFrame = 0;
      }
      else
      {
        this.m_lastTOIFrame = MySession.Static.GameplayFrameCounter;
        this.m_toiCountThisFrame = 0;
      }
    }

    public void ConsiderDisablingTOIs()
    {
      if (MySession.Static.GameplayFrameCounter > this.m_lastTOIFrame + 10)
        return;
      this.m_savedQuality = this.RigidBody.Quality;
      this.RigidBody.Quality = HkCollidableQualityType.Debris;
      DisableGridTOIsOptimizer.Static.Register(this);
    }

    public void DisableTOIOptimization()
    {
      if (!this.IsTOIOptimized)
        return;
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        this.RigidBody.Quality = this.m_savedQuality;
        this.m_savedQuality = HkCollidableQualityType.Invalid;
      }
      DisableGridTOIsOptimizer.Static.Unregister(this);
    }

    private void UpdateContactCallbackLimit()
    {
      HkRigidBody rigidBody = this.RigidBody;
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        return;
      int num = this.m_grid.IsClientPredicted || Sync.IsServer ? (this.m_grid.BlocksCount <= 5 ? 1 : 10) : 0;
      rigidBody.CallbackLimit = num;
    }

    public override void FracturedBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
    {
      if (!MyFakes.ENABLE_AFTER_REPLACE_BODY || !Sync.IsServer || this.m_recreateBody)
        return;
      this.HavokWorld.DestructionWorld.RemoveBreakableBody(e.OldBody);
      this.m_oldLinVel = this.RigidBody.LinearVelocity;
      this.m_oldAngVel = this.RigidBody.AngularVelocity;
      MyPhysics.RemoveDestructions(this.RigidBody);
      e.GetNewBodies(this.m_newBodies);
      if (this.m_newBodies.Count == 0)
        return;
      bool flag1 = false;
      MyGridPhysics.m_tmpBlocksToDelete.Clear();
      MyGridPhysics.m_tmpBlocksUpdateDamage.Clear();
      MySlimBlock b = (MySlimBlock) null;
      foreach (HkdBreakableBodyInfo newBody in this.m_newBodies)
      {
        if (!newBody.IsFracture() || MyFakes.ENABLE_FRACTURE_COMPONENT && this.m_grid.BlocksCount == 1 && (this.m_grid.IsStatic && MyDestructionHelper.IsFixed(newBody)))
        {
          this.m_newBreakableBodies.Add(MyFracturedPiecesManager.Static.GetBreakableBody(newBody));
          this.FindFracturedBlocks(newBody);
          this.HavokWorld.DestructionWorld.RemoveBreakableBody(newBody);
        }
        else
        {
          HkdBreakableBody breakableBody = MyFracturedPiecesManager.Static.GetBreakableBody(newBody);
          Matrix rigidBodyMatrix = breakableBody.GetRigidBody().GetRigidBodyMatrix();
          Vector3D world = this.ClusterToWorld(rigidBodyMatrix.Translation);
          HkdBreakableShape breakableShape = breakableBody.BreakableShape;
          HkVec3IProperty property1 = (HkVec3IProperty) breakableShape.GetProperty((int) byte.MaxValue);
          if (!property1.IsValid() && breakableShape.IsCompound())
            property1 = (HkVec3IProperty) breakableShape.GetChild(0).Shape.GetProperty((int) byte.MaxValue);
          bool flag2 = false;
          MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(property1.Value);
          MyCompoundCubeBlock compoundCubeBlock = cubeBlock != null ? cubeBlock.FatBlock as MyCompoundCubeBlock : (MyCompoundCubeBlock) null;
          if (cubeBlock != null)
          {
            if (b == null)
              b = cubeBlock;
            if (!flag1)
            {
              this.AddDestructionEffect(this.m_grid.GridIntegerToWorld(cubeBlock.Position), Vector3.Forward);
              flag1 = true;
            }
            MatrixD worldMatrix = (MatrixD) ref rigidBodyMatrix;
            worldMatrix.Translation = world;
            if (MyFakes.ENABLE_FRACTURE_COMPONENT)
            {
              HkSimpleValueProperty property2 = (HkSimpleValueProperty) breakableShape.GetProperty(256);
              if (property2.IsValid())
                MyGridPhysics.m_tmpCompoundIds.Add((ushort) property2.ValueUInt);
              else if (!property2.IsValid() && breakableShape.IsCompound())
              {
                MyGridPhysics.m_tmpChildren_CompoundIds.Clear();
                breakableShape.GetChildren(MyGridPhysics.m_tmpChildren_CompoundIds);
                foreach (HkdShapeInstanceInfo childrenCompoundId in MyGridPhysics.m_tmpChildren_CompoundIds)
                {
                  HkSimpleValueProperty property3 = (HkSimpleValueProperty) childrenCompoundId.Shape.GetProperty(256);
                  if (property3.IsValid())
                    MyGridPhysics.m_tmpCompoundIds.Add((ushort) property3.ValueUInt);
                }
              }
              bool flag3 = true;
              if (MyGridPhysics.m_tmpCompoundIds.Count > 0)
              {
                foreach (ushort tmpCompoundId in MyGridPhysics.m_tmpCompoundIds)
                {
                  MySlimBlock block = compoundCubeBlock.GetBlock(tmpCompoundId);
                  if (block == null)
                  {
                    flag3 = false;
                  }
                  else
                  {
                    MyGridPhysics.m_tmpDefinitions.Add(block.BlockDefinition.Id);
                    flag3 &= this.RemoveShapesFromFracturedBlocks(breakableBody, block, new ushort?(tmpCompoundId), MyGridPhysics.m_tmpBlocksToDelete, MyGridPhysics.m_tmpBlocksUpdateDamage);
                  }
                }
              }
              else
              {
                MyGridPhysics.m_tmpDefinitions.Add(cubeBlock.BlockDefinition.Id);
                flag3 = this.RemoveShapesFromFracturedBlocks(breakableBody, cubeBlock, new ushort?(), MyGridPhysics.m_tmpBlocksToDelete, MyGridPhysics.m_tmpBlocksUpdateDamage);
              }
              if (flag3)
              {
                if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, MyGridPhysics.m_tmpDefinitions, compoundCubeBlock != null ? (MyCubeBlock) compoundCubeBlock : cubeBlock.FatBlock) == null)
                  flag2 = true;
              }
              else
                flag2 = true;
              MyGridPhysics.m_tmpChildren_CompoundIds.Clear();
              MyGridPhysics.m_tmpCompoundIds.Clear();
              MyGridPhysics.m_tmpDefinitions.Clear();
            }
            else if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, (List<MyDefinitionId>) null, compoundCubeBlock != null ? (MyCubeBlock) compoundCubeBlock : cubeBlock.FatBlock) == null)
              flag2 = true;
          }
          else
            flag2 = true;
          if (flag2)
          {
            this.HavokWorld.DestructionWorld.RemoveBreakableBody(newBody);
            MyFracturedPiecesManager.Static.ReturnToPool(breakableBody);
          }
        }
      }
      this.m_newBodies.Clear();
      if (b != null)
        MyAudioComponent.PlayDestructionSound(b);
      if (MyFakes.ENABLE_FRACTURE_COMPONENT)
      {
        this.FindFractureComponentBlocks();
        foreach (MyFractureComponentBase.Info info in this.m_fractureBlockComponentsCache)
          MyGridPhysics.m_tmpBlocksToDelete.Remove(((MyCubeBlock) info.Entity).SlimBlock);
        foreach (MySlimBlock mySlimBlock in MyGridPhysics.m_tmpBlocksToDelete)
          MyGridPhysics.m_tmpBlocksUpdateDamage.Remove(mySlimBlock);
        foreach (MySlimBlock block in MyGridPhysics.m_tmpBlocksToDelete)
        {
          if (block.IsMultiBlockPart)
          {
            MyCubeGridMultiBlockInfo multiBlockInfo = block.CubeGrid.GetMultiBlockInfo(block.MultiBlockId);
            if (multiBlockInfo != null && multiBlockInfo.Blocks.Count > 1 && block.GetFractureComponent() != null)
              block.ApplyDestructionDamage(0.0f);
          }
          if (block.FatBlock != null)
            block.FatBlock.OnDestroy();
          this.m_grid.RemoveBlockWithId(block, true);
        }
        foreach (MySlimBlock mySlimBlock in MyGridPhysics.m_tmpBlocksUpdateDamage)
        {
          MyFractureComponentCubeBlock fractureComponent = mySlimBlock.GetFractureComponent();
          if (fractureComponent != null)
            mySlimBlock.ApplyDestructionDamage(fractureComponent.GetIntegrityRatioFromFracturedPieceCounts());
        }
      }
      else
      {
        foreach (MySlimBlock mySlimBlock in MyGridPhysics.m_tmpBlocksToDelete)
        {
          MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(mySlimBlock.Position);
          if (cubeBlock != null)
          {
            if (cubeBlock.FatBlock != null)
              cubeBlock.FatBlock.OnDestroy();
            this.m_grid.RemoveBlock(cubeBlock, true);
          }
        }
      }
      MyGridPhysics.m_tmpBlocksToDelete.Clear();
      MyGridPhysics.m_tmpBlocksUpdateDamage.Clear();
      this.m_recreateBody = true;
    }

    private bool RemoveShapesFromFracturedBlocks(
      HkdBreakableBody bBody,
      MySlimBlock block,
      ushort? compoundId,
      HashSet<MySlimBlock> blocksToDelete,
      HashSet<MySlimBlock> blocksUpdateDamage)
    {
      MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
      if (fractureComponent != null)
      {
        bool flag = false;
        HkdBreakableShape breakableShape = bBody.BreakableShape;
        if (MyGridPhysics.IsBreakableShapeCompound(breakableShape))
        {
          MyGridPhysics.m_tmpShapeNames.Clear();
          MyGridPhysics.m_tmpChildren_RemoveShapes.Clear();
          breakableShape.GetChildren(MyGridPhysics.m_tmpChildren_RemoveShapes);
          int count = MyGridPhysics.m_tmpChildren_RemoveShapes.Count;
          for (int index = 0; index < count; ++index)
          {
            HkdShapeInstanceInfo childrenRemoveShape = MyGridPhysics.m_tmpChildren_RemoveShapes[index];
            if (string.IsNullOrEmpty(childrenRemoveShape.ShapeName))
              childrenRemoveShape.Shape.GetChildren(MyGridPhysics.m_tmpChildren_RemoveShapes);
          }
          MyGridPhysics.m_tmpChildren_RemoveShapes.ForEach((Action<HkdShapeInstanceInfo>) (c =>
          {
            string shapeName = c.ShapeName;
            if (string.IsNullOrEmpty(shapeName))
              return;
            MyGridPhysics.m_tmpShapeNames.Add(shapeName);
          }));
          if (MyGridPhysics.m_tmpShapeNames.Count != 0)
          {
            flag = fractureComponent.RemoveChildShapes((IEnumerable<string>) MyGridPhysics.m_tmpShapeNames);
            long entityId = block.CubeGrid.EntityId;
            Vector3I position = block.Position;
            ushort? nullable = compoundId;
            int num = nullable.HasValue ? (int) nullable.GetValueOrDefault() : (int) ushort.MaxValue;
            List<string> tmpShapeNames = MyGridPhysics.m_tmpShapeNames;
            MySyncDestructions.RemoveShapesFromFractureComponent(entityId, position, (ushort) num, tmpShapeNames);
          }
          MyGridPhysics.m_tmpChildren_RemoveShapes.Clear();
          MyGridPhysics.m_tmpShapeNames.Clear();
        }
        else
        {
          string name = bBody.BreakableShape.Name;
          flag = fractureComponent.RemoveChildShapes((IEnumerable<string>) new string[1]
          {
            name
          });
          long entityId = block.CubeGrid.EntityId;
          Vector3I position = block.Position;
          ushort? nullable = compoundId;
          int num = nullable.HasValue ? (int) nullable.GetValueOrDefault() : (int) ushort.MaxValue;
          string shapeName = name;
          MySyncDestructions.RemoveShapeFromFractureComponent(entityId, position, (ushort) num, shapeName);
        }
        if (flag)
          blocksToDelete.Add(block);
        else
          blocksUpdateDamage.Add(block);
      }
      else
        blocksToDelete.Add(block);
      return true;
    }

    private static bool IsBreakableShapeCompound(HkdBreakableShape shape) => string.IsNullOrEmpty(shape.Name) || shape.IsCompound() || shape.GetChildrenCount() > 0;

    public List<MyFracturedBlock.Info> GetFracturedBlocks() => this.m_fractureBlocksCache;

    public List<MyFractureComponentBase.Info> GetFractureBlockComponents() => this.m_fractureBlockComponentsCache;

    public void ClearFractureBlockComponents() => this.m_fractureBlockComponentsCache.Clear();

    private void BreakableBody_AfterControllerOperation(HkdBreakableBody b)
    {
      if (!this.m_recreateBody)
        return;
      b.BreakableShape.SetStrenghtRecursively(MyDestructionConstants.STRENGTH, 0.7f);
    }

    private void BreakableBody_BeforeControllerOperation(HkdBreakableBody b)
    {
      if (!this.m_recreateBody)
        return;
      b.BreakableShape.SetStrenghtRecursively(float.MaxValue, 0.7f);
    }

    private void RigidBody_ContactPointCallback_Destruction(ref HkContactPointEvent value)
    {
      MyGridContactInfo info = new MyGridContactInfo(ref value, this.m_grid);
      if (info.IsKnown)
        return;
      MyCubeGrid currentEntity = info.CurrentEntity;
      if (currentEntity == null || currentEntity.Physics == null || (HkReferenceObject) currentEntity.Physics.RigidBody == (HkReferenceObject) null)
        return;
      HkRigidBody rigidBody = currentEntity.Physics.RigidBody;
      MyPhysicsBody physicsBody1 = value.GetPhysicsBody(0);
      MyPhysicsBody physicsBody2 = value.GetPhysicsBody(1);
      if (physicsBody1 == null || physicsBody2 == null)
        return;
      IMyEntity entity1 = physicsBody1.Entity;
      IMyEntity entity2 = physicsBody2.Entity;
      if (entity1 == null || entity2 == null || (entity1.Physics == null || entity2.Physics == null) || entity1 is MyFracturedPiece && entity2 is MyFracturedPiece)
        return;
      HkRigidBody bodyA = value.Base.BodyA;
      HkRigidBody gridBody = value.Base.BodyB;
      info.HandleEvents();
      if (bodyA.HasProperty(254) || gridBody.HasProperty(254) || (info.CollidingEntity is MyCharacter || info.CollidingEntity == null) || info.CollidingEntity.MarkedForClose)
        return;
      MyCubeGrid myCubeGrid1 = entity1 as MyCubeGrid;
      if (!(entity2 is MyCubeGrid myCubeGrid2) && entity2 is MyEntitySubpart)
      {
        while (entity2 != null && !(entity2 is MyCubeGrid))
          entity2 = entity2.Parent;
        if (entity2 != null)
        {
          gridBody = (entity2.Physics as MyPhysicsBody).RigidBody;
          myCubeGrid2 = entity2 as MyCubeGrid;
        }
      }
      if (myCubeGrid1 != null && myCubeGrid2 != null && MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid1) == MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid2))
        return;
      double num1 = (double) Math.Abs(value.SeparatingVelocity);
      Vector3 velocityAtPoint1 = bodyA.GetVelocityAtPoint(info.Event.ContactPoint.Position);
      Vector3 velocityAtPoint2 = gridBody.GetVelocityAtPoint(info.Event.ContactPoint.Position);
      float num2 = velocityAtPoint1.Length();
      float num3 = velocityAtPoint2.Length();
      Vector3 vector1_1 = (double) num2 > 0.0 ? Vector3.Normalize(velocityAtPoint1) : Vector3.Zero;
      Vector3 vector2 = (double) num3 > 0.0 ? Vector3.Normalize(velocityAtPoint2) : Vector3.Zero;
      float num4 = MyDestructionHelper.MassFromHavok(bodyA.Mass);
      float num5 = MyDestructionHelper.MassFromHavok(gridBody.Mass);
      float num6 = num2 * num4;
      float num7 = num3 * num5;
      HkContactPoint contactPoint;
      double num8;
      if ((double) num2 <= 0.0)
      {
        num8 = 0.0;
      }
      else
      {
        Vector3 vector1_2 = vector1_1;
        contactPoint = value.ContactPoint;
        Vector3 normal = contactPoint.Normal;
        num8 = (double) Vector3.Dot(vector1_2, normal);
      }
      float num9 = (float) num8;
      double num10;
      if ((double) num3 <= 0.0)
      {
        num10 = 0.0;
      }
      else
      {
        Vector3 vector1_2 = vector2;
        contactPoint = value.ContactPoint;
        Vector3 normal = contactPoint.Normal;
        num10 = (double) Vector3.Dot(vector1_2, normal);
      }
      float num11 = (float) num10;
      float num12 = num2 * Math.Abs(num9);
      float num13 = num3 * Math.Abs(num11);
      bool flag1 = (double) num4 == 0.0;
      bool flag2 = (double) num5 == 0.0;
      bool flag3 = entity1 is MyFracturedPiece || myCubeGrid1 != null && myCubeGrid1.GridSizeEnum == MyCubeSize.Small;
      bool flag4 = entity2 is MyFracturedPiece || myCubeGrid2 != null && myCubeGrid2.GridSizeEnum == MyCubeSize.Small;
      double num14 = (double) Vector3.Dot(vector1_1, vector2);
      float num15 = 0.5f;
      float damage1 = num6 * info.ImpulseMultiplier;
      float num16 = num7 * info.ImpulseMultiplier;
      MyHitInfo hitInfo = new MyHitInfo();
      Vector3D contactPosition1 = info.ContactPosition;
      ref MyHitInfo local = ref hitInfo;
      contactPoint = value.ContactPoint;
      Vector3 normal1 = contactPoint.Normal;
      local.Normal = normal1;
      if ((double) num9 < 0.0)
      {
        if (entity1 is MyFracturedPiece)
          damage1 /= 10f;
        damage1 *= Math.Abs(num9);
        if (((double) damage1 > 2000.0 && (double) num12 > 2.0 && !flag4 || (double) damage1 > 500.0 && (double) num12 > 10.0) && (flag2 || (double) damage1 / (double) num16 > 10.0))
        {
          hitInfo.Position = contactPosition1 + 0.1f * hitInfo.Normal;
          damage1 -= num4;
          if (Sync.IsServer && (double) damage1 > 0.0)
          {
            if (myCubeGrid1 != null)
            {
              Vector3 gridPosition = MyGridPhysics.GetGridPosition(value.ContactPoint, bodyA, myCubeGrid1, 0);
              myCubeGrid1.DoDamage(damage1, hitInfo, new Vector3?(gridPosition), myCubeGrid2 != null ? myCubeGrid2.EntityId : 0L);
            }
            else
            {
              double num17 = (double) damage1;
              MyPhysicsBody physics = (MyPhysicsBody) entity1.Physics;
              Vector3D contactPosition2 = info.ContactPosition;
              contactPoint = value.ContactPoint;
              Vector3 normal2 = contactPoint.Normal;
              double num18 = (double) num15;
              MyDestructionHelper.TriggerDestruction((float) num17, physics, contactPosition2, normal2, (float) num18);
            }
            hitInfo.Position = contactPosition1 - 0.1f * hitInfo.Normal;
            if (myCubeGrid2 != null)
            {
              Vector3 gridPosition = MyGridPhysics.GetGridPosition(value.ContactPoint, gridBody, myCubeGrid2, 1);
              myCubeGrid2.DoDamage(damage1, hitInfo, new Vector3?(gridPosition), myCubeGrid1 != null ? myCubeGrid1.EntityId : 0L);
            }
            else
            {
              double num17 = (double) damage1;
              MyPhysicsBody physics = (MyPhysicsBody) entity2.Physics;
              Vector3D contactPosition2 = info.ContactPosition;
              contactPoint = value.ContactPoint;
              Vector3 normal2 = contactPoint.Normal;
              double num18 = (double) num15;
              MyDestructionHelper.TriggerDestruction((float) num17, physics, contactPosition2, normal2, (float) num18);
            }
            this.ReduceVelocities(info);
          }
          MyDecals.HandleAddDecal(entity1, hitInfo, Vector3.Zero);
          MyDecals.HandleAddDecal(entity2, hitInfo, Vector3.Zero);
        }
      }
      if ((double) num11 >= 0.0)
        return;
      if (entity2 is MyFracturedPiece)
        num16 /= 10f;
      float num19 = num16 * Math.Abs(num11);
      if (((double) num19 <= 2000.0 || (double) num13 <= 2.0 || flag3) && ((double) num19 <= 500.0 || (double) num13 <= 10.0) || !flag1 && (double) num19 / (double) damage1 <= 10.0)
        return;
      hitInfo.Position = contactPosition1 + 0.1f * hitInfo.Normal;
      float damage2 = num19 - num5;
      if (Sync.IsServer && (double) damage2 > 0.0)
      {
        if (myCubeGrid1 != null)
        {
          Vector3 gridPosition = MyGridPhysics.GetGridPosition(value.ContactPoint, bodyA, myCubeGrid1, 0);
          myCubeGrid1.DoDamage(damage2, hitInfo, new Vector3?(gridPosition), myCubeGrid2 != null ? myCubeGrid2.EntityId : 0L);
        }
        else
        {
          double num17 = (double) damage2;
          MyPhysicsBody physics = (MyPhysicsBody) entity1.Physics;
          Vector3D contactPosition2 = info.ContactPosition;
          contactPoint = value.ContactPoint;
          Vector3 normal2 = contactPoint.Normal;
          double num18 = (double) num15;
          MyDestructionHelper.TriggerDestruction((float) num17, physics, contactPosition2, normal2, (float) num18);
        }
        hitInfo.Position = contactPosition1 - 0.1f * hitInfo.Normal;
        if (myCubeGrid2 != null)
        {
          Vector3 gridPosition = MyGridPhysics.GetGridPosition(value.ContactPoint, gridBody, myCubeGrid2, 1);
          myCubeGrid2.DoDamage(damage2, hitInfo, new Vector3?(gridPosition), myCubeGrid1 != null ? myCubeGrid1.EntityId : 0L);
        }
        else
        {
          double num17 = (double) damage2;
          MyPhysicsBody physics = (MyPhysicsBody) entity2.Physics;
          Vector3D contactPosition2 = info.ContactPosition;
          contactPoint = value.ContactPoint;
          Vector3 normal2 = contactPoint.Normal;
          double num18 = (double) num15;
          MyDestructionHelper.TriggerDestruction((float) num17, physics, contactPosition2, normal2, (float) num18);
        }
        this.ReduceVelocities(info);
      }
      MyDecals.HandleAddDecal(entity1, hitInfo, Vector3.Zero);
      MyDecals.HandleAddDecal(entity2, hitInfo, Vector3.Zero);
    }

    private HkShape CreateBreakableBody(HkShape shape, HkMassProperties? massProperties)
    {
      HkMassProperties massProperties1 = massProperties.HasValue ? massProperties.Value : new HkMassProperties();
      if (!this.Shape.BreakableShape.IsValid())
        this.Shape.CreateBreakableShape();
      HkdBreakableShape breakableShape = this.Shape.BreakableShape;
      if (!breakableShape.IsValid())
      {
        breakableShape = new HkdBreakableShape(shape);
        if (massProperties.HasValue)
        {
          HkMassProperties massProperties2 = massProperties.Value;
          breakableShape.SetMassProperties(ref massProperties2);
        }
        else
          breakableShape.SetMassRecursively(50f);
      }
      else
        breakableShape.BuildMassProperties(ref massProperties1);
      shape = breakableShape.GetShape();
      HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
      hkRigidBodyCinfo.AngularDamping = this.m_angularDamping;
      hkRigidBodyCinfo.LinearDamping = this.m_linearDamping;
      hkRigidBodyCinfo.SolverDeactivation = this.m_grid.IsStatic ? this.InitialSolverDeactivation : HkSolverDeactivation.Low;
      hkRigidBodyCinfo.ContactPointCallbackDelay = this.ContactPointDelay;
      hkRigidBodyCinfo.Shape = shape;
      hkRigidBodyCinfo.SetMassProperties(massProperties1);
      MyPhysicsBody.GetInfoFromFlags(hkRigidBodyCinfo, this.Flags);
      if (this.m_grid.IsStatic)
      {
        hkRigidBodyCinfo.MotionType = HkMotionType.Dynamic;
        hkRigidBodyCinfo.QualityType = HkCollidableQualityType.Moving;
      }
      HkRigidBody body = new HkRigidBody(hkRigidBodyCinfo);
      if (this.m_grid.IsStatic)
        body.UpdateMotionType(HkMotionType.Fixed);
      body.EnableDeactivation = true;
      this.BreakableBody = new HkdBreakableBody(breakableShape, body, (HkdWorld) null, Matrix.Identity);
      this.BreakableBody.AfterReplaceBody += new BreakableBodyReplaced(((MyPhysicsBody) this).FracturedBody_AfterReplaceBody);
      return shape;
    }

    private void FindFracturedBlocks(HkdBreakableBodyInfo b)
    {
      HkdBreakableBodyHelper breakableBodyHelper = new HkdBreakableBodyHelper(b);
      breakableBodyHelper.GetRigidBodyMatrix();
      breakableBodyHelper.GetChildren(this.m_children);
      foreach (HkdShapeInstanceInfo child in this.m_children)
      {
        if (child.IsFracturePiece())
        {
          Vector3I vector3I = ((HkVec3IProperty) child.Shape.GetProperty((int) byte.MaxValue)).Value;
          if (this.m_grid.CubeExists(vector3I))
          {
            if (MyFakes.ENABLE_FRACTURE_COMPONENT)
            {
              MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(vector3I);
              if (cubeBlock == null || this.FindFractureComponentBlocks(cubeBlock, child))
                ;
            }
            else
            {
              if (!this.m_fracturedBlocksShapes.ContainsKey(vector3I))
                this.m_fracturedBlocksShapes[vector3I] = new List<HkdShapeInstanceInfo>();
              this.m_fracturedBlocksShapes[vector3I].Add(child);
            }
          }
        }
      }
      if (!MyFakes.ENABLE_FRACTURE_COMPONENT)
      {
        foreach (Vector3I key in this.m_fracturedBlocksShapes.Keys)
        {
          List<HkdShapeInstanceInfo> fracturedBlocksShape = this.m_fracturedBlocksShapes[key];
          foreach (HkdShapeInstanceInfo shapeInstanceInfo in fracturedBlocksShape)
          {
            Matrix transform = shapeInstanceInfo.GetTransform();
            transform.Translation = Vector3.Zero;
            shapeInstanceInfo.SetTransform(ref transform);
          }
          HkdBreakableShape hkdBreakableShape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, fracturedBlocksShape);
          ((HkdCompoundBreakableShape) hkdBreakableShape).RecalcMassPropsFromChildren();
          HkMassProperties hkMassProperties = new HkMassProperties();
          hkdBreakableShape.BuildMassProperties(ref hkMassProperties);
          HkdBreakableShape parent = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref hkMassProperties);
          foreach (HkdShapeInstanceInfo shapeInfo in fracturedBlocksShape)
            parent.AddShape(ref shapeInfo);
          hkdBreakableShape.RemoveReference();
          MyGridPhysics.ConnectPiecesInBlock(parent, fracturedBlocksShape);
          MyFracturedBlock.Info info = new MyFracturedBlock.Info()
          {
            Shape = parent,
            Position = key,
            Compound = true
          };
          MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(key);
          if (cubeBlock == null)
          {
            parent.RemoveReference();
          }
          else
          {
            if (cubeBlock.FatBlock is MyFracturedBlock)
            {
              MyFracturedBlock fatBlock = cubeBlock.FatBlock as MyFracturedBlock;
              info.OriginalBlocks = fatBlock.OriginalBlocks;
              info.Orientations = fatBlock.Orientations;
              info.MultiBlocks = fatBlock.MultiBlocks;
            }
            else if (cubeBlock.FatBlock is MyCompoundCubeBlock)
            {
              info.OriginalBlocks = new List<MyDefinitionId>();
              info.Orientations = new List<MyBlockOrientation>();
              MyCompoundCubeBlock fatBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
              bool flag = false;
              ListReader<MySlimBlock> blocks = fatBlock.GetBlocks();
              foreach (MySlimBlock mySlimBlock in blocks)
              {
                info.OriginalBlocks.Add(mySlimBlock.BlockDefinition.Id);
                info.Orientations.Add(mySlimBlock.Orientation);
                flag = flag || mySlimBlock.IsMultiBlockPart;
              }
              if (flag)
              {
                info.MultiBlocks = new List<MyFracturedBlock.MultiBlockPartInfo>();
                foreach (MySlimBlock mySlimBlock in blocks)
                {
                  if (mySlimBlock.IsMultiBlockPart)
                    info.MultiBlocks.Add(new MyFracturedBlock.MultiBlockPartInfo()
                    {
                      MultiBlockDefinition = mySlimBlock.MultiBlockDefinition.Id,
                      MultiBlockId = mySlimBlock.MultiBlockId
                    });
                  else
                    info.MultiBlocks.Add((MyFracturedBlock.MultiBlockPartInfo) null);
                }
              }
            }
            else
            {
              info.OriginalBlocks = new List<MyDefinitionId>();
              info.Orientations = new List<MyBlockOrientation>();
              info.OriginalBlocks.Add(cubeBlock.BlockDefinition.Id);
              info.Orientations.Add(cubeBlock.Orientation);
              if (cubeBlock.IsMultiBlockPart)
              {
                info.MultiBlocks = new List<MyFracturedBlock.MultiBlockPartInfo>();
                info.MultiBlocks.Add(new MyFracturedBlock.MultiBlockPartInfo()
                {
                  MultiBlockDefinition = cubeBlock.MultiBlockDefinition.Id,
                  MultiBlockId = cubeBlock.MultiBlockId
                });
              }
            }
            this.m_fractureBlocksCache.Add(info);
          }
        }
      }
      this.m_fracturedBlocksShapes.Clear();
      this.m_children.Clear();
    }

    private bool FindFractureComponentBlocks(MySlimBlock block, HkdShapeInstanceInfo shapeInst)
    {
      HkdBreakableShape shape = shapeInst.Shape;
      if (MyGridPhysics.IsBreakableShapeCompound(shape))
      {
        bool flag = false;
        List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
        shape.GetChildren(list);
        foreach (HkdShapeInstanceInfo shapeInst1 in list)
          flag |= this.FindFractureComponentBlocks(block, shapeInst1);
        return flag;
      }
      ushort? nullable = new ushort?();
      if (shape.HasProperty(256))
        nullable = new ushort?((ushort) ((HkSimpleValueProperty) shape.GetProperty(256)).ValueUInt);
      if (block.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        if (!nullable.HasValue)
          return false;
        MySlimBlock block1 = fatBlock.GetBlock(nullable.Value);
        if (block1 == null)
          return false;
        block = block1;
      }
      if (!this.m_fracturedSlimBlocksShapes.ContainsKey(block))
        this.m_fracturedSlimBlocksShapes[block] = new List<HkdShapeInstanceInfo>();
      this.m_fracturedSlimBlocksShapes[block].Add(shapeInst);
      return true;
    }

    private void FindFractureComponentBlocks()
    {
      foreach (KeyValuePair<MySlimBlock, List<HkdShapeInstanceInfo>> fracturedSlimBlocksShape in this.m_fracturedSlimBlocksShapes)
      {
        MySlimBlock key = fracturedSlimBlocksShape.Key;
        List<HkdShapeInstanceInfo> shapeInstanceInfoList = fracturedSlimBlocksShape.Value;
        if (!key.FatBlock.Components.Has<MyFractureComponentBase>())
        {
          int shapeChildrenCount = key.GetTotalBreakableShapeChildrenCount();
          if (!key.BlockDefinition.CreateFracturedPieces || shapeChildrenCount != shapeInstanceInfoList.Count)
          {
            foreach (HkdShapeInstanceInfo shapeInstanceInfo in shapeInstanceInfoList)
              shapeInstanceInfo.SetTransform(ref Matrix.Identity);
            HkdBreakableShape hkdBreakableShape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, shapeInstanceInfoList);
            ((HkdCompoundBreakableShape) hkdBreakableShape).RecalcMassPropsFromChildren();
            HkMassProperties hkMassProperties = new HkMassProperties();
            hkdBreakableShape.BuildMassProperties(ref hkMassProperties);
            HkdBreakableShape parent = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref hkMassProperties);
            foreach (HkdShapeInstanceInfo shapeInfo in shapeInstanceInfoList)
              parent.AddShape(ref shapeInfo);
            hkdBreakableShape.RemoveReference();
            MyGridPhysics.ConnectPiecesInBlock(parent, shapeInstanceInfoList);
            this.m_fractureBlockComponentsCache.Add(new MyFractureComponentBase.Info()
            {
              Entity = (MyEntity) key.FatBlock,
              Shape = parent,
              Compound = true
            });
          }
        }
      }
      this.m_fracturedSlimBlocksShapes.Clear();
    }

    private static void ConnectPiecesInBlock(
      HkdBreakableShape parent,
      List<HkdShapeInstanceInfo> shapeList)
    {
      for (int index1 = 0; index1 < shapeList.Count; ++index1)
      {
        for (int index2 = 0; index2 < shapeList.Count; ++index2)
        {
          if (index1 != index2)
          {
            HkdBreakableShape parent1 = parent;
            HkdShapeInstanceInfo shape1 = shapeList[index1];
            HkdBreakableShape shape2 = shape1.Shape;
            shape1 = shapeList[index2];
            HkdBreakableShape shape3 = shape1.Shape;
            MyGridShape.ConnectShapesWithChildren(parent1, shape2, shape3);
          }
        }
      }
    }

    private void RecreateBreakableBody(HashSet<Vector3I> dirtyBlocks)
    {
      bool fixedOrKeyframed = this.RigidBody.IsFixedOrKeyframed;
      int layer = this.RigidBody.Layer;
      HkWorld havokWorld = this.m_grid.Physics.HavokWorld;
      foreach (HkdBreakableBody newBreakableBody in this.m_newBreakableBodies)
        MyFracturedPiecesManager.Static.ReturnToPool(newBreakableBody);
      HkRigidBody rigidBody = this.BreakableBody.GetRigidBody();
      Vector3 linearVelocity = rigidBody.LinearVelocity;
      Vector3 angularVelocity = rigidBody.AngularVelocity;
      if (this.m_grid.BlocksCount > 0)
      {
        this.Shape.UnmarkBreakable(this.RigidBody);
        this.Shape.RefreshBlocks(this.RigidBody, dirtyBlocks);
        this.Shape.MarkBreakable(this.RigidBody);
        this.Shape.UpdateShape(this.RigidBody, this.RigidBody2, this.BreakableBody);
        this.CloseRigidBody();
        HkShape shape = (HkShape) this.m_shape;
        this.CreateBody(ref shape, new HkMassProperties?());
        this.RigidBody.Layer = layer;
        this.RigidBody.ContactPointCallbackEnabled = true;
        this.RigidBody.ContactSoundCallbackEnabled = true;
        this.RigidBody.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback_Destruction);
        this.BreakableBody.BeforeControllerOperation += new BeforeBodyControllerOperation(this.BreakableBody_BeforeControllerOperation);
        this.BreakableBody.AfterControllerOperation += new AfterBodyControllerOperation(this.BreakableBody_AfterControllerOperation);
        Matrix m = (Matrix) ref this.Entity.PositionComp.WorldMatrixRef;
        m.Translation = (Vector3) this.WorldToCluster(this.Entity.PositionComp.GetPosition());
        this.RigidBody.SetWorldMatrix(m);
        this.RigidBody.UserObject = (object) this;
        this.Entity.Physics.LinearVelocity = this.m_oldLinVel;
        this.Entity.Physics.AngularVelocity = this.m_oldAngVel;
        this.m_grid.DetectDisconnectsAfterFrame();
        this.Shape.CreateConnectionToWorld(this.BreakableBody, havokWorld);
        this.HavokWorld.DestructionWorld.AddBreakableBody(this.BreakableBody);
      }
      else
        this.m_grid.Close();
      this.m_newBreakableBodies.Clear();
      this.m_fractureBlocksCache.Clear();
    }

    public bool CheckLastDestroyedBlockFracturePieces()
    {
      if (!Sync.IsServer || this.m_grid.BlocksCount != 1 || this.m_grid.IsStatic)
        return false;
      MySlimBlock block1 = this.m_grid.GetBlocks().First<MySlimBlock>();
      if (block1.FatBlock == null)
        return false;
      if (block1.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        bool flag = true;
        List<MySlimBlock> mySlimBlockList = new List<MySlimBlock>((IEnumerable<MySlimBlock>) fatBlock.GetBlocks());
        foreach (MySlimBlock mySlimBlock in mySlimBlockList)
          flag = flag && mySlimBlock.FatBlock.Components.Has<MyFractureComponentBase>();
        if (flag)
        {
          foreach (MySlimBlock block2 in mySlimBlockList)
          {
            MyFractureComponentCubeBlock fractureComponent = block2.GetFractureComponent();
            ushort? blockId = fatBlock.GetBlockId(block2);
            if (fractureComponent != null)
              MyDestructionHelper.CreateFracturePiece(fractureComponent, true);
            this.m_grid.RemoveBlockWithId(block2.Position, new ushort?(blockId.Value), true);
          }
        }
        return flag;
      }
      MyFractureComponentCubeBlock fractureComponent1 = block1.GetFractureComponent();
      if (fractureComponent1 != null)
      {
        MyDestructionHelper.CreateFracturePiece(fractureComponent1, true);
        this.m_grid.RemoveBlock(block1, true);
      }
      return fractureComponent1 != null;
    }

    internal ushort? GetContactCompoundId(Vector3I position, Vector3D constactPos)
    {
      List<HkdBreakableShape> shapesIntersectingSphere = new List<HkdBreakableShape>();
      this.GetRigidBodyMatrix(out this.m_bodyMatrix);
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(this.m_bodyMatrix);
      if ((HkReferenceObject) this.BreakableBody == (HkReferenceObject) null)
        MyLog.Default.WriteLine("BreakableBody was null in GetContactCounpoundId!");
      if ((HkReferenceObject) this.HavokWorld.DestructionWorld == (HkReferenceObject) null)
        MyLog.Default.WriteLine("HavokWorld.DestructionWorld was null in GetContactCompoundId!");
      HkDestructionUtils.FindAllBreakableShapesIntersectingSphere(this.HavokWorld.DestructionWorld, this.BreakableBody, fromRotationMatrix, this.m_bodyMatrix.Translation, (Vector3) this.WorldToCluster(constactPos), 0.1f, shapesIntersectingSphere);
      ushort? nullable = new ushort?();
      foreach (HkdBreakableShape hkdBreakableShape in shapesIntersectingSphere)
      {
        if (hkdBreakableShape.IsValid())
        {
          HkVec3IProperty property1 = (HkVec3IProperty) hkdBreakableShape.GetProperty((int) byte.MaxValue);
          if (property1.IsValid() && !(position != property1.Value))
          {
            HkSimpleValueProperty property2 = (HkSimpleValueProperty) hkdBreakableShape.GetProperty(256);
            if (property2.IsValid())
            {
              nullable = new ushort?((ushort) property2.ValueUInt);
              break;
            }
          }
        }
      }
      return nullable;
    }

    public struct ExplosionInfo
    {
      public Vector3D Position;
      public MyExplosionTypeEnum ExplosionType;
      public float Radius;
      public bool ShowParticles;
      public bool GenerateDebris;
    }

    public class MyDirtyBlocksInfo
    {
      public ConcurrentCachingList<BoundingBoxI> DirtyParts = new ConcurrentCachingList<BoundingBoxI>();
      public HashSet<Vector3I> DirtyBlocks = new HashSet<Vector3I>();

      public void Clear()
      {
        this.DirtyParts.ClearList();
        this.DirtyBlocks.Clear();
      }
    }

    private enum GridEffectType
    {
      Collision,
      Destruction,
      Dust,
    }

    private struct GridCollisionHit
    {
      public Vector3D RelativePosition;
      public Vector3 Normal;
      public Vector3 RelativeVelocity;
      public float SeparatingSpeed;
      public float Impulse;
    }

    private struct GridEffect
    {
      public MyGridPhysics.GridEffectType Type;
      public Vector3D Position;
      public Vector3 Normal;
      public float Scale;
      public float SeparatingSpeed;
      public float Impulse;
    }

    private class CollisionParticleEffect
    {
      public MyParticleEffect Effect;
      public Vector3D RelativePosition;
      public Vector3 Normal;
      public Vector3 SeparatingVelocity;
      public int RemainingTime;
      public float Impulse;
    }

    private enum PredictionDisqualificationReason : byte
    {
      None,
      NoEntity,
      EntityIsStatic,
      EntityIsNotMoving,
    }

    private class Sandbox_Game_Entities_Cube_MyGridPhysics\u003C\u003EActor
    {
    }
  }
}
