// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyCharacter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Common;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Graphics;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.Utils;
using VRage.GameServices;
using VRage.Generics;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Character
{
  [MyEntityType(typeof (MyObjectBuilder_Character), true)]
  [StaticEventOwner]
  public class MyCharacter : MySkinnedEntity, IMyCameraController, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyInventoryOwner, IMyUseObject, IMyDestroyableObject, IMyDecalProxy, IMyCharacter, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEventProxy, IMyEventOwner, IMyComponentOwner<MyIDModule>, IMySyncedEntity, IMyTrackTrails, IMyParallelUpdateable
  {
    private MyBlueprintClassDefinition m_buildPlannerBlueprintClass;
    private static float AIM_ASSIST_DISTANCE = 50f;
    private static float AIM_MULT_ENA = 0.33f;
    private static float AIM_MULT_DIS = 1f;
    public static float AIM_ASSIST_CAPSULE_HEIGHT = 1.1f;
    public static float AIM_ASSIST_CAPSULE_RADIUS = 0.75f;
    private const string ANIM_EVENT_NAME_MAGAZINE_PLUG = "MagazinePlug";
    private const string ANIM_EVENT_NAME_MAGAZINE_UNPLUG = "MagazineUnplug";
    private readonly float ENEMY_INDICATOR_TARGET_DISTANCE = 20f;
    private static readonly float DYNAMIC_RANGE_MAX_DISTANCE = 20f;
    private static List<VertexArealBoneIndexWeight> m_boneIndexWeightTmp;
    [ThreadStatic]
    private static MyCharacterHitInfo m_hitInfoTmp;
    public const float CAMERA_NEAR_DISTANCE = 60f;
    internal const float CHARACTER_X_ROTATION_SPEED = 0.13f;
    private const float CHARACTER_Y_ROTATION_FACTOR = 0.02f;
    private const float WALK_THRESHOLD = 0.4f;
    private const float SPRINT_THRESHOLD = 1.6f;
    private const float SPRINT_X_TOLERANCE = 0.1f;
    public const float MINIMAL_SPEED = 0.001f;
    private const float JUMP_DURATION = 0.55f;
    private const float JUMP_TIME = 1f;
    private const float SHOT_TIME = 0.1f;
    private const float FALL_TIME = 0.3f;
    private const float RESPAWN_TIME = 5f;
    internal const float MIN_HEAD_LOCAL_X_ANGLE = -89.9f;
    internal const float MAX_HEAD_LOCAL_X_ANGLE = 89f;
    private const float FOOTPRINT_DISTANCE = 5.6f;
    private bool m_forceStandingFootprints;
    internal const float MIN_HEAD_LOCAL_Y_ANGLE_ON_LADDER = -89.9f;
    internal const float MAX_HEAD_LOCAL_Y_ANGLE_ON_LADDER = 89f;
    public const int HK_CHARACTER_FLYING = 5;
    private const float AERIAL_CONTROL_FORCE_MULTIPLIER = 0.062f;
    public static float MAX_SHAKE_DAMAGE = 90f;
    private float m_currentShotTime;
    private float m_currentShootPositionTime;
    private float m_cameraDistance;
    private float m_currentSpeed;
    private Vector3 m_currentMovementDirection = Vector3.Zero;
    private float m_currentDecceleration;
    private float m_currentJumpTime;
    private float m_frictionBeforeJump = 1.3f;
    private bool m_assetModifiersLoaded;
    private Color m_relationMarkColor = Color.White;
    private bool m_snapTargetChanged;
    private long m_snapTarget;
    private MyPhysicsBody m_aimAssistPhysics;
    private bool m_canJump = true;
    private bool m_canSprint = true;
    public bool UpdateRotationsOverride;
    private float m_currentWalkDelay;
    private float m_canPlayImpact;
    private static MyStringId m_stringIdHit = MyStringId.GetOrCompute("Hit");
    private MyStringHash m_physicalMaterialHash;
    private long m_deadPlayerIdentityId = -1;
    private Vector3 m_gravity = Vector3.Zero;
    private bool m_resolveHighlightOverlap;
    private bool m_parallelDynamicRangeRunning;
    private Vector3D m_dynamicRangeStart = Vector3D.Zero;
    public VRage.Sync.Sync<bool, SyncDirection.FromServer> IsReloading;
    private VRage.Sync.Sync<MyRecoilDataCollection, SyncDirection.FromServer> m_recoilData;
    private VRage.Sync.Sync<bool, SyncDirection.FromServer> m_enableBroadcastingPlayerToggle;
    private VRage.Sync.Sync<(float distance, Vector3D hitPosition), SyncDirection.FromServer> m_dynamicRangeDistance;
    public static MyHudNotification OutOfAmmoNotification;
    private int m_weaponBone = -1;
    public float CharacterGeneralDamageModifier = 1f;
    private bool m_usingByPrimary;
    private float m_headLocalXAngle;
    private float m_headLocalYAngle;
    private bool m_headRenderingEnabled = true;
    private readonly VRage.Sync.Sync<MyBootsState, SyncDirection.FromServer> m_bootsState;
    public float RotationSpeed = 0.13f;
    private const double MIN_FORCE_PREDICTION_DURATION = 10.0;
    private bool m_forceDisablePrediction;
    private double m_forceDisablePredictionTime;
    private int m_headBoneIndex = -1;
    private int m_camera3rdBoneIndex = -1;
    private int m_leftHandIKStartBone = -1;
    private int m_leftHandIKEndBone = -1;
    private int m_rightHandIKStartBone = -1;
    private int m_rightHandIKEndBone = -1;
    private int m_leftUpperarmBone = -1;
    private int m_leftForearmBone = -1;
    private int m_rightUpperarmBone = -1;
    private int m_rightForearmBone = -1;
    private int m_leftHandItemBone = -1;
    private int m_rightHandItemBone = -1;
    private int m_spineBone = -1;
    protected bool m_characterBoneCapsulesReady;
    private bool m_animationCommandsEnabled = true;
    private float m_currentAnimationChangeDelay;
    private float SAFE_DELAY_FOR_ANIMATION_BLEND = 0.1f;
    private MyCharacterMovementEnum m_currentMovementState;
    private MyCharacterMovementEnum m_previousMovementState;
    private MyCharacterMovementEnum m_previousNetworkMovementState;
    private MyEntity m_leftHandItem;
    private MyHandItemDefinition m_handItemDefinition;
    private MyZoomModeEnum m_zoomMode;
    private float m_currentHandItemWalkingBlend;
    private float m_currentHandItemShootBlend;
    private CapsuleD[] m_bodyCapsules = new CapsuleD[1];
    private MatrixD m_headMatrix = MatrixD.CreateTranslation(0.0, 1.65, 0.0);
    private MyHudNotification m_pickupObjectNotification;
    private HkCharacterStateType m_currentCharacterState;
    private bool m_isFalling;
    private bool m_isFallingAnimationPlayed;
    private float m_currentFallingTime;
    private bool m_crouchAfterFall;
    private MyCharacterMovementFlags m_movementFlags;
    private MyCharacterMovementFlags m_netMovementFlags;
    private MyCharacterMovementFlags m_previousMovementFlags;
    private bool m_movementsFlagsChanged;
    private string m_characterModel;
    private MyBattery m_suitBattery;
    private MyResourceDistributorComponent m_suitResourceDistributor;
    private float m_outsideTemperature;
    private bool m_jetpackRunning;
    private List<MyPhysics.HitInfo> m_hitList = new List<MyPhysics.HitInfo>();
    private MyResourceSinkComponent m_sinkComp;
    private MyEntity m_topGrid;
    private MyEntity m_usingEntity;
    private bool m_enableBag = true;
    private static readonly float ROTATION_SPEED_CLASSIC = 1f;
    private static readonly float ROTATION_SPEED_IRONSIGHTS = 0.6f;
    public const float REFLECTOR_RANGE = 35f;
    public const float REFLECTOR_CONE_ANGLE = 0.373f;
    public const float REFLECTOR_BILLBOARD_LENGTH = 40f;
    public const float REFLECTOR_BILLBOARD_THICKNESS = 6f;
    public static Vector4 REFLECTOR_COLOR = Vector4.One;
    public static float REFLECTOR_FALLOFF = 1f;
    public static float REFLECTOR_GLOSS_FACTOR = 1f;
    public static float REFLECTOR_DIFFUSE_FACTOR = 3.14f;
    public static float REFLECTOR_INTENSITY = 25f;
    public static Vector4 POINT_COLOR = Vector4.One;
    public static float POINT_FALLOFF = 0.3f;
    public static float POINT_GLOSS_FACTOR = 1f;
    public static float POINT_DIFFUSE_FACTOR = 3.14f;
    public static float POINT_LIGHT_INTENSITY = 0.5f;
    public static float POINT_LIGHT_RANGE = 1.08f;
    public static bool LIGHT_PARAMETERS_CHANGED = false;
    public const float LIGHT_GLARE_MAX_DISTANCE_SQR = 1600f;
    private float m_currentLightPower;
    private float m_lightPowerFromProducer;
    private float m_lightTurningOnSpeed = 0.05f;
    private float m_lightTurningOffSpeed = 0.05f;
    private bool m_lightEnabled = true;
    private float m_currentHeadAnimationCounter;
    private float m_currentLocalHeadAnimation = -1f;
    private float m_localHeadAnimationLength = -1f;
    private Vector2? m_localHeadAnimationX;
    private Vector2? m_localHeadAnimationY;
    private List<MyBoneCapsuleInfo> m_bodyCapsuleInfo = new List<MyBoneCapsuleInfo>();
    private HashSet<uint> m_shapeContactPoints = new HashSet<uint>();
    private float m_currentRespawnCounter;
    private MyHudNotification m_respawnNotification;
    private MyHudNotification m_notEnoughStatNotification;
    private MyStringHash manipulationToolId = MyStringHash.GetOrCompute("ManipulationTool");
    private Queue<Vector3> m_bobQueue = new Queue<Vector3>();
    private bool m_dieAfterSimulation;
    private Vector3? m_deathLinearVelocityFromSever;
    private MyMagneticBootsOnGridSmoothing m_magBootsOnGridSmoothing;
    private float m_currentLootingCounter;
    private MyEntityCameraSettings m_cameraSettingsWhenAlive;
    private bool m_useAnimationForWeapon = true;
    private long m_relativeDampeningEntityInit;
    private MyCharacterDefinition m_characterDefinition;
    private bool m_isInFirstPersonView = true;
    private bool m_targetFromCamera;
    private bool m_forceFirstPersonCamera;
    private bool m_moveAndRotateStopped;
    private bool m_moveAndRotateCalled;
    private readonly VRage.Sync.Sync<int, SyncDirection.FromServer> m_currentAmmoCount;
    private readonly VRage.Sync.Sync<int, SyncDirection.FromServer> m_currentMagazineAmmoCount;
    private readonly VRage.Sync.Sync<long, SyncDirection.BothWays> m_aimedGrid;
    private readonly VRage.Sync.Sync<Vector3I, SyncDirection.BothWays> m_aimedBlock;
    private readonly VRage.Sync.Sync<MyPlayer.PlayerId, SyncDirection.FromServer> m_controlInfo;
    private readonly VRage.Sync.Sync<Vector3, SyncDirection.BothWays> m_localHeadPosition;
    private VRage.Sync.Sync<float, SyncDirection.BothWays> m_animLeaning;
    private List<IMyNetworkCommand> m_cachedCommands;
    private Vector3 m_previousLinearVelocity;
    private Vector3D m_previousPosition;
    private bool m_stepLeft;
    private bool[] m_isShooting;
    public Vector3 ShootDirection = Vector3.One;
    private bool m_shootDoubleClick;
    private long m_lastShootDirectionUpdate;
    private long m_closestParentId;
    private MyIDModule m_idModule = new MyIDModule(0L, MyOwnershipShareModeEnum.Faction);
    internal readonly VRage.Sync.Sync<float, SyncDirection.FromServer> EnvironmentOxygenLevelSync;
    internal readonly VRage.Sync.Sync<float, SyncDirection.FromServer> OxygenLevelAtCharacterLocation;
    internal readonly VRage.Sync.Sync<long, SyncDirection.FromServer> OxygenSourceGridEntityId;
    private static readonly Vector3[] m_defaultColors = new Vector3[7]
    {
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(0.0f, -0.96f, -0.5f),
      new Vector3(0.575f, 0.15f, 0.2f),
      new Vector3(0.333f, -0.33f, -0.05f),
      new Vector3(0.0f, 0.0f, 0.05f),
      new Vector3(0.0f, -0.8f, 0.6f),
      new Vector3(0.122f, 0.05f, 0.46f)
    };
    public static readonly string DefaultModel = "Default_Astronaut";
    private float? m_savedHealth;
    private float m_characterFeetOffset;
    private readonly MyStringHash m_footStepStringHash = MyStringHash.GetOrCompute("Footprint");
    private readonly MyStringHash m_footStepMirroredStringHash = MyStringHash.GetOrCompute("Footprintmirrored");
    private bool m_wasInFirstPerson;
    private bool m_isInFirstPerson;
    private bool m_wasInThirdPersonBeforeIronSight;
    private List<HkBodyCollision> m_physicsCollisionResults;
    private List<MyEntity> m_supportedEntitiesTmp = new List<MyEntity>();
    private bool m_needsUpdateBoots;
    private Vector3D m_crosshairPoint;
    private Vector3D m_aimedPoint;
    private List<HkBodyCollision> m_penetrationList = new List<HkBodyCollision>();
    private float m_headMovementXOffset;
    private float m_headMovementYOffset;
    private float m_maxHeadMovementOffset = 3f;
    private float m_headMovementStep = 0.1f;
    private bool m_lastGetViewWasDead;
    private Matrix m_getViewAliveWorldMatrix = Matrix.Identity;
    private Vector3D m_lastProceduralGeneratorPosition = Vector3D.PositiveInfinity;
    private static readonly List<uint> m_tmpIds = new List<uint>();
    private MyControllerInfo m_info = new MyControllerInfo();
    private MyDefinitionId? m_endShootAutoswitch;
    private MyDefinitionId? m_autoswitch;
    private List<MyPhysics.HitInfo> m_enemyIndicatorHitInfos = new List<MyPhysics.HitInfo>();
    private bool m_parallelEnemyIndicatorRunning;
    private MatrixD m_lastCorrectSpectatorCamera;
    private float m_squeezeDamageTimer;
    private const float m_weaponMinAmp = 1.123778f;
    private const float m_weaponMaxAmp = 1.217867f;
    private const float m_weaponMedAmp = 1.170823f;
    private const float m_weaponRunMedAmp = 1.128767f;
    private Quaternion m_weaponMatrixOrientationBackup;
    private MyCharacterBreath m_breath;
    public MyEntity ManipulatedEntity;
    private MyGuiScreenBase m_InventoryScreen;
    private MyCharacterClientState m_lastClientState;
    private MyEntity m_relativeDampeningEntity;
    private const float LadderSpeed = 2f;
    private const float MinHeadLadderLocalYAngle = -90f;
    private const float MaxHeadLadderLocalYAngle = 90f;
    private float m_stepIncrement;
    private int m_stepsPerAnimation;
    private Vector3 m_ladderIncrementToBase;
    private Vector3 m_ladderIncrementToBaseServer;
    private MatrixD m_baseMatrix;
    private int m_currentLadderStep;
    private MyLadder m_ladder;
    private MyHudNotification m_ladderOffNotification;
    private MyHudNotification m_ladderUpDownNotification;
    private MyHudNotification m_ladderJumpOffNotification;
    private MyHudNotification m_ladderBlockedNotification;
    private long? m_ladderIdInit;
    private MyObjectBuilder_Character.LadderInfo? m_ladderInfoInit;
    private HkConstraint m_constraintInstance;
    private HkFixedConstraintData m_constraintData;
    private HkBreakableConstraintData m_constraintBreakableData;
    private bool m_needReconnectLadder;
    private MyCubeGrid m_oldLadderGrid;
    public static readonly string TRACK_KEY_RELOAD = "reload";
    private bool m_shouldPositionMagazine;
    private bool m_arePlayerMarkersSet;
    private VRage.Sync.Sync<bool, SyncDirection.FromServer> m_isAimAssistSensitivityDecreased;
    private bool m_isAimAssistSnapAllowed;
    private static readonly string MAGAZINE_DUMMY_BONE_NAME = "MagazineDummy_01";
    private static readonly string CHEST_DUMMY_BONE_NAME = "SE_RigSpine4";
    private static string TopBody = "LeftHand RightHand LeftFingers RightFingers Head Spine";
    private bool m_resetWeaponAnimationState;
    private Quaternion m_lastRotation;
    private static Dictionary<Vector3D, MyParticleEffect> m_burrowEffectTable = new Dictionary<Vector3D, MyParticleEffect>();
    private int m_magazineDummyBone;
    private int m_chestDummyBone;
    private int m_wasOnLadder;
    private readonly Vector3[] m_animationSpeedFilter = new Vector3[4];
    private int m_animationSpeedFilterCursor;
    private float m_walkRunSpeed = 4f;
    private float m_runSprintSpeed = 8.5f;
    private bool m_reloadHandSimulationState = true;
    private bool m_isReloadingPrevious;
    private bool m_shouldPositionHandPrevious;
    private static List<MyEntity> m_supportingEntities;

    public IReadOnlyList<MyIdentity.BuildPlanItem> BuildPlanner => this.GetIdentity() == null ? (IReadOnlyList<MyIdentity.BuildPlanItem>) null : this.GetIdentity().BuildPlanner;

    private static MyBlueprintDefinitionBase MakeBlueprintFromBuildPlanItem(
      MyIdentity.BuildPlanItem buildPlanItem)
    {
      MyObjectBuilder_CompositeBlueprintDefinition newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompositeBlueprintDefinition>();
      if (buildPlanItem.BlockDefinition == null)
        return (MyBlueprintDefinitionBase) null;
      newObject.Id = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlueprintDefinition), buildPlanItem.BlockDefinition.Id.ToString().Replace("MyObjectBuilder_", "BuildPlanItem_"));
      Dictionary<MyDefinitionId, MyFixedPoint> dictionary = new Dictionary<MyDefinitionId, MyFixedPoint>();
      foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
      {
        MyDefinitionId id = component.ComponentDefinition.Id;
        if (!dictionary.ContainsKey(id))
          dictionary[id] = (MyFixedPoint) 0;
        dictionary[id] += (MyFixedPoint) component.Count;
      }
      newObject.Blueprints = new BlueprintItem[dictionary.Count];
      int index = 0;
      foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> keyValuePair in dictionary)
      {
        MyBlueprintDefinitionBase definitionByResultId;
        if ((definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(keyValuePair.Key)) == null)
          return (MyBlueprintDefinitionBase) null;
        newObject.Blueprints[index] = new BlueprintItem()
        {
          Id = new SerializableDefinitionId(definitionByResultId.Id.TypeId, definitionByResultId.Id.SubtypeName),
          Amount = keyValuePair.Value.ToString()
        };
        ++index;
      }
      newObject.Icons = buildPlanItem.BlockDefinition.Icons;
      newObject.DisplayName = buildPlanItem.BlockDefinition.DisplayNameEnum.HasValue ? buildPlanItem.BlockDefinition.DisplayNameEnum.Value.ToString() : buildPlanItem.BlockDefinition.DisplayNameText;
      newObject.Public = buildPlanItem.BlockDefinition.Public;
      MyCompositeBlueprintDefinition blueprintDefinition = new MyCompositeBlueprintDefinition();
      blueprintDefinition.Init((MyObjectBuilder_DefinitionBase) newObject, MyModContext.BaseGame);
      blueprintDefinition.Postprocess();
      return (MyBlueprintDefinitionBase) blueprintDefinition;
    }

    public bool AddToBuildPlanner(
      MyCubeBlockDefinition block,
      int index = -1,
      List<MyIdentity.BuildPlanItem.Component> components = null)
    {
      if (!this.GetIdentity().AddToBuildPlanner(block, index, components))
        return false;
      this.UpdateBuildPlanner();
      return true;
    }

    public void RemoveAtBuildPlanner(int index)
    {
      this.GetIdentity().RemoveAtBuildPlanner(index);
      this.UpdateBuildPlanner();
    }

    public void RemoveLastFromBuildPlanner()
    {
      this.GetIdentity().RemoveLastFromBuildPlanner();
      this.UpdateBuildPlanner();
    }

    public void CleanFinishedBuildPlanner()
    {
      this.GetIdentity().CleanFinishedBuildPlanner();
      this.UpdateBuildPlanner();
    }

    private void LoadBuildPlanner(
      MyObjectBuilder_Character.BuildPlanItem[] buildPlanner)
    {
      MyIdentity identity = this.GetIdentity();
      if (identity == null)
        return;
      identity.LoadBuildPlanner(buildPlanner);
      this.UpdateBuildPlanner(false);
    }

    private void UpdateBuildPlanner(bool synchronize = true)
    {
      if (this.m_buildPlannerBlueprintClass == null)
        this.m_buildPlannerBlueprintClass = MyDefinitionManager.Static.GetBlueprintClass("BuildPlanner");
      if (this.m_buildPlannerBlueprintClass == null)
        return;
      this.m_buildPlannerBlueprintClass.ClearBlueprints();
      foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) this.GetIdentity().BuildPlanner)
      {
        MyBlueprintDefinitionBase blueprint = MyCharacter.MakeBlueprintFromBuildPlanItem(buildPlanItem);
        if (!this.m_buildPlannerBlueprintClass.ContainsBlueprint(blueprint))
          this.m_buildPlannerBlueprintClass.AddBlueprint(blueprint);
      }
      if (!synchronize)
        return;
      this.SynchronizeBuildPlanner();
    }

    private MyObjectBuilder_Character.BuildPlanItem[] SaveBuildPlanner()
    {
      List<MyObjectBuilder_Character.BuildPlanItem> buildPlanItemList = new List<MyObjectBuilder_Character.BuildPlanItem>();
      foreach (MyIdentity.BuildPlanItem buildPlanItem1 in (IEnumerable<MyIdentity.BuildPlanItem>) this.GetIdentity().BuildPlanner)
      {
        if (buildPlanItem1.BlockDefinition != null)
        {
          MyObjectBuilder_Character.BuildPlanItem buildPlanItem2 = new MyObjectBuilder_Character.BuildPlanItem();
          buildPlanItem2.BlockId = (SerializableDefinitionId) buildPlanItem1.BlockDefinition.Id;
          buildPlanItem2.IsInProgress = buildPlanItem1.IsInProgress;
          buildPlanItem2.Components = new List<MyObjectBuilder_Character.ComponentItem>();
          foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem1.Components)
            buildPlanItem2.Components.Add(new MyObjectBuilder_Character.ComponentItem()
            {
              ComponentId = (SerializableDefinitionId) component.ComponentDefinition.Id,
              Count = component.Count
            });
          buildPlanItemList.Add(buildPlanItem2);
        }
      }
      return buildPlanItemList.ToArray();
    }

    internal void SynchronizeBuildPlanner()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyCharacter, MyObjectBuilder_Character.BuildPlanItem[]>(this, (Func<MyCharacter, Action<MyObjectBuilder_Character.BuildPlanItem[]>>) (x => new Action<MyObjectBuilder_Character.BuildPlanItem[]>(x.SynchronizeBuildPlanner_Implementation)), this.SaveBuildPlanner());
    }

    [Event(null, 196)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void SynchronizeBuildPlanner_Implementation(
      MyObjectBuilder_Character.BuildPlanItem[] buildPlanner)
    {
      this.GetIdentity().LoadBuildPlanner(buildPlanner);
      this.UpdateBuildPlanner(false);
    }

    public static event Action<MyCharacter> OnCharacterDied;

    internal bool CanJump
    {
      get => this.m_canJump;
      set => this.m_canJump = value;
    }

    public bool CanSprint
    {
      get => this.m_canSprint;
      set => this.m_canSprint = value;
    }

    internal float CurrentWalkDelay
    {
      get => this.m_currentWalkDelay;
      set => this.m_currentWalkDelay = value;
    }

    public Vector3 Gravity => this.m_gravity;

    public bool IsIronSighted => this.ZoomMode == MyZoomModeEnum.IronSight;

    public void RegisterRecoilDataChange(Action<SyncBase> callback)
    {
      this.m_recoilData.ValueChanged -= callback;
      this.m_recoilData.ValueChanged += callback;
    }

    public void UnregisterRecoilDataChange(Action<SyncBase> callback) => this.m_recoilData.ValueChanged -= callback;

    public void GetRecoilData(out float verticalValue, out float horizontalValue)
    {
      verticalValue = this.m_recoilData.Value.VerticalValue;
      horizontalValue = this.m_recoilData.Value.HorizontalValue;
    }

    public void SetRecoilData(float verticalValue, float horizontalValue)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyRecoilDataCollection recoilDataCollection = this.m_recoilData.Value;
      this.m_recoilData.Value = new MyRecoilDataCollection()
      {
        Id = recoilDataCollection.Id + 1,
        VerticalValue = verticalValue,
        HorizontalValue = horizontalValue
      };
    }

    public int WeaponBone => this.m_weaponBone;

    public event Action<IMyHandheldGunObject<MyDeviceBase>> WeaponEquiped;

    private IMyHandheldGunObject<MyDeviceBase> m_currentWeapon { get; set; }

    public bool IsClientPredicted { get; private set; }

    public bool ForceDisablePrediction
    {
      get => this.m_forceDisablePrediction;
      set
      {
        this.m_forceDisablePrediction = value;
        this.m_forceDisablePredictionTime = MySandboxGame.Static.SimulationTime.Seconds;
      }
    }

    public bool AlwaysDisablePrediction { get; set; }

    public bool HeadRenderingEnabled => this.m_headRenderingEnabled;

    public float HeadLocalXAngle
    {
      get => !this.m_headLocalXAngle.IsValid() ? 0.0f : this.m_headLocalXAngle;
      set => this.m_headLocalXAngle = value.IsValid() ? MathHelper.Clamp(value, -89.9f, 89f) : 0.0f;
    }

    public float HeadLocalYAngle
    {
      get => this.m_headLocalYAngle;
      set
      {
        if (this.IsOnLadder && this.IsInFirstPersonView)
          this.m_headLocalYAngle = MathHelper.Clamp(value, -89.9f, 89f);
        else
          this.m_headLocalYAngle = value;
      }
    }

    public MyCharacterMovementEnum CurrentMovementState
    {
      get => this.m_currentMovementState;
      set => this.SetCurrentMovementState(value);
    }

    public MyCharacterMovementEnum PreviousMovementState => this.m_previousMovementState;

    [Obsolete("OnMovementStateChanged is deprecated, use MovementStateChanged")]
    public event CharacterMovementStateDelegate OnMovementStateChanged;

    public event CharacterMovementStateChangedDelegate MovementStateChanged;

    public MyHandItemDefinition HandItemDefinition => this.m_handItemDefinition;

    public MyZoomModeEnum ZoomMode
    {
      get => this.m_zoomMode;
      set
      {
        if (this.m_zoomMode == value)
          return;
        this.m_zoomMode = value;
        if (this.m_zoomMode != MyZoomModeEnum.IronSight)
          return;
        this.IsAimAssistSnapAllowed = true;
      }
    }

    public bool ShouldSupressShootAnimation => this.m_currentWeapon != null && this.m_currentWeapon.SupressShootAnimation();

    public HkCharacterStateType CharacterGroundState => this.m_currentCharacterState;

    public bool JetpackRunning => this.JetpackComp != null && this.JetpackComp.Running;

    public bool DampenersEnabled => this.JetpackComp != null && this.JetpackComp.DampenersEnabled;

    internal MyResourceDistributorComponent SuitRechargeDistributor
    {
      get => this.m_suitResourceDistributor;
      set
      {
        if (this.Components.Contains(typeof (MyResourceDistributorComponent)))
          this.Components.Remove<MyResourceDistributorComponent>();
        this.Components.Add<MyResourceDistributorComponent>(value);
        this.m_suitResourceDistributor = value;
      }
    }

    public MyResourceSinkComponent SinkComp
    {
      get => this.m_sinkComp;
      set => this.m_sinkComp = value;
    }

    public bool EnabledBag => this.m_enableBag;

    public SyncType SyncType { get; set; }

    public float CurrentLightPower => this.m_currentLightPower;

    public float CurrentRespawnCounter => this.m_currentRespawnCounter;

    internal MyRadioReceiver RadioReceiver
    {
      get => (MyRadioReceiver) this.Components.Get<MyDataReceiver>();
      private set => this.Components.Add<MyDataReceiver>((MyDataReceiver) value);
    }

    internal MyRadioBroadcaster RadioBroadcaster
    {
      get => (MyRadioBroadcaster) this.Components.Get<MyDataBroadcaster>();
      private set => this.Components.Add<MyDataBroadcaster>((MyDataBroadcaster) value);
    }

    public StringBuilder CustomNameWithFaction { get; private set; }

    internal MyRenderComponentCharacter Render
    {
      get => base.Render as MyRenderComponentCharacter;
      set => this.Render = (MyRenderComponentBase) value;
    }

    public MyCharacterSoundComponent SoundComp
    {
      get => this.Components.Get<MyCharacterSoundComponent>();
      set
      {
        if (this.Components.Has<MyCharacterSoundComponent>())
          this.Components.Remove<MyCharacterSoundComponent>();
        this.Components.Add<MyCharacterSoundComponent>(value);
      }
    }

    public MyAtmosphereDetectorComponent AtmosphereDetectorComp
    {
      get => this.Components.Get<MyAtmosphereDetectorComponent>();
      set
      {
        if (this.Components.Has<MyAtmosphereDetectorComponent>())
          this.Components.Remove<MyAtmosphereDetectorComponent>();
        this.Components.Add<MyAtmosphereDetectorComponent>(value);
      }
    }

    public MyEntityReverbDetectorComponent ReverbDetectorComp
    {
      get => this.Components.Get<MyEntityReverbDetectorComponent>();
      set
      {
        if (this.Components.Has<MyEntityReverbDetectorComponent>())
          this.Components.Remove<MyEntityReverbDetectorComponent>();
        this.Components.Add<MyEntityReverbDetectorComponent>(value);
      }
    }

    public MyCharacterStatComponent StatComp
    {
      get => this.Components.Get<MyEntityStatComponent>() as MyCharacterStatComponent;
      set
      {
        if (this.Components.Has<MyEntityStatComponent>())
          this.Components.Remove<MyEntityStatComponent>();
        this.Components.Add<MyEntityStatComponent>((MyEntityStatComponent) value);
      }
    }

    public MyCharacterJetpackComponent JetpackComp
    {
      get => this.Components.Get<MyCharacterJetpackComponent>();
      set
      {
        if (this.Components.Has<MyCharacterJetpackComponent>())
          this.Components.Remove<MyCharacterJetpackComponent>();
        this.Components.Add<MyCharacterJetpackComponent>(value);
      }
    }

    float IMyCharacter.BaseMass => this.BaseMass;

    float IMyCharacter.CurrentMass => this.CurrentMass;

    public float BaseMass => this.Definition.Mass;

    public float CurrentMass
    {
      get
      {
        float num = 0.0f;
        if (this.ManipulatedEntity != null && this.ManipulatedEntity.Physics != null)
          num = this.ManipulatedEntity.Physics.Mass;
        return MyEntityExtensions.GetInventory(this) != null ? this.BaseMass + (float) MyEntityExtensions.GetInventory(this).CurrentMass + num : this.BaseMass + num;
      }
    }

    public MyCharacterDefinition Definition => this.m_characterDefinition;

    MyDefinitionBase IMyCharacter.Definition => (MyDefinitionBase) this.m_characterDefinition;

    public bool IsInFirstPersonView
    {
      get => this.m_isInFirstPersonView;
      set
      {
        if (!value && !MySession.Static.Settings.Enable3rdPersonView)
          this.m_isInFirstPersonView = true;
        else if (this.Definition.EnableFirstPersonView)
        {
          this.m_isInFirstPersonView = value;
          this.ResetHeadRotation();
          if (!this.m_isInFirstPersonView && this.ZoomMode == MyZoomModeEnum.IronSight)
            this.EnableIronsight(false, false, true);
          this.SwitchCameraIronSightChanges();
        }
        else
          this.m_isInFirstPersonView = false;
      }
    }

    public bool EnableFirstPersonView
    {
      get => this.Definition.EnableFirstPersonView;
      set
      {
      }
    }

    public bool TargetFromCamera
    {
      get
      {
        if (MySession.Static.ControlledEntity == this)
          return MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator;
        return !Sandbox.Engine.Platform.Game.IsDedicated && this.m_targetFromCamera;
      }
      set => this.m_targetFromCamera = value;
    }

    public MyToolbar Toolbar => MyToolbarComponent.CharacterToolbar;

    public bool ForceFirstPersonCamera
    {
      get
      {
        if (this.IsDead)
          return false;
        return this.m_forceFirstPersonCamera || MyThirdPersonSpectator.Static.IsCameraForced();
      }
      set => this.m_forceFirstPersonCamera = !this.IsDead & value;
    }

    public bool UpdateCalled()
    {
      int num = (long) this.m_actualUpdateFrame != (long) this.m_actualDrawFrame ? 1 : 0;
      this.m_actualDrawFrame = this.m_actualUpdateFrame;
      return num != 0;
    }

    public bool IsCameraNear
    {
      get
      {
        if (MyFakes.ENABLE_PERMANENT_SIMULATIONS_COMPUTATION)
          return true;
        return this.Render.IsVisible() && (double) this.m_cameraDistance <= 60.0;
      }
    }

    public event EventHandler OnWeaponChanged;

    public event Action<MyCharacter> CharacterDied;

    public MyInventoryAggregate InventoryAggregate
    {
      get => this.Components.Get<MyInventoryBase>() as MyInventoryAggregate;
      set
      {
        if (this.Components.Has<MyInventoryBase>())
          this.Components.Remove<MyInventoryBase>();
        this.Components.Add<MyInventoryBase>((MyInventoryBase) value);
      }
    }

    public MyCharacterOxygenComponent OxygenComponent { get; private set; }

    public MyCharacterWeaponPositionComponent WeaponPosition { get; private set; }

    public Vector3 MoveIndicator { get; set; }

    public Vector2 RotationIndicator { get; set; }

    public bool IsRotating { get; set; }

    public float RollIndicator { get; set; }

    public Vector3 RotationCenterIndicator { get; set; }

    public long AimedGrid
    {
      get => this.m_aimedGrid.Value;
      set => this.m_aimedGrid.Value = value;
    }

    public Vector3I AimedBlock
    {
      get => this.m_aimedBlock.Value;
      set => this.m_aimedBlock.Value = value;
    }

    public ulong ControlSteamId => this.m_controlInfo == null ? 0UL : this.m_controlInfo.Value.SteamId;

    public MyPlayer.PlayerId ControlInfo => this.m_controlInfo == null ? new MyPlayer.PlayerId() : this.m_controlInfo.Value;

    public MyPromoteLevel PromoteLevel => MySession.Static.GetUserPromoteLevel(this.m_controlInfo.Value.SteamId);

    public bool IsShooting(MyShootActionEnum action) => this.m_isShooting[(int) action];

    public MyShootActionEnum? GetShootingAction()
    {
      foreach (MyShootActionEnum myShootActionEnum in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.m_isShooting[(int) myShootActionEnum])
          return new MyShootActionEnum?(myShootActionEnum);
      }
      return new MyShootActionEnum?();
    }

    public long ClosestParentId
    {
      get => this.m_closestParentId;
      set
      {
        if (this.m_closestParentId == value && MyGridPhysicalHierarchy.Static.NonGridLinkExists(value, (MyEntity) this))
          return;
        MyCubeGrid entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(this.m_closestParentId, out entity, true))
          MyGridPhysicalHierarchy.Static.RemoveNonGridNode(entity, (MyEntity) this);
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(value, out entity))
        {
          this.m_closestParentId = value;
          MyGridPhysicalHierarchy.Static.AddNonGridNode(entity, (MyEntity) this);
        }
        else
          this.m_closestParentId = 0L;
      }
    }

    bool IMyComponentOwner<MyIDModule>.GetComponent(out MyIDModule module)
    {
      module = this.m_idModule;
      return true;
    }

    public bool IsPersistenceCharacter { get; set; }

    public bool? IsClientOnline { get; set; }

    public static MyObjectBuilder_Character Random()
    {
      MyObjectBuilder_Character builderCharacter = new MyObjectBuilder_Character();
      builderCharacter.CharacterModel = MyCharacter.DefaultModel;
      builderCharacter.SubtypeName = MyCharacter.DefaultModel;
      builderCharacter.ColorMaskHSV = (SerializableVector3) MyCharacter.m_defaultColors[MyUtils.GetRandomInt(0, MyCharacter.m_defaultColors.Length)];
      return builderCharacter;
    }

    public MyCharacter()
    {
      this.ControllerInfo.ControlAcquired += new Action<MyEntityController>(this.OnControlAcquired);
      this.ControllerInfo.ControlReleased += new Action<MyEntityController>(this.OnControlReleased);
      this.CustomNameWithFaction = new StringBuilder();
      this.PositionComp = (MyPositionComponentBase) new MyCharacter.MyCharacterPosition();
      (this.PositionComp as MyPositionComponent).WorldPositionChanged = new Action<object>(this.WorldPositionChanged);
      this.Render = new MyRenderComponentCharacter();
      this.Render.EnableColorMaskHsv = true;
      this.Render.NeedsDraw = true;
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      this.Render.SkipIfTooSmall = false;
      this.Render.DrawInAllCascades = true;
      this.Render.MetalnessColorable = true;
      this.SinkComp = new MyResourceSinkComponent();
      this.SyncType = SyncHelpers.Compose((object) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentCharacter(this));
      if (MyPerGameSettings.CharacterDetectionComponent != (System.Type) null)
        this.Components.Add<MyCharacterDetectorComponent>((MyCharacterDetectorComponent) Activator.CreateInstance(MyPerGameSettings.CharacterDetectionComponent));
      else if (MyFakes.ENABLE_AREA_INTERACTIONS)
        this.Components.Add<MyCharacterDetectorComponent>((MyCharacterDetectorComponent) new MyCharacterClosestDetectorComponent());
      else
        this.Components.Add<MyCharacterDetectorComponent>((MyCharacterDetectorComponent) new MyCharacterRaycastDetectorComponent());
      if (MyFakes.ENABLE_CHARACTER_IK_FEET_OFFSET)
        this.AnimationController.Controller.IkFeetOffset = MyPerGameSettings.PhysicsConvexRadius;
      this.m_currentAmmoCount.AlwaysReject<int, SyncDirection.FromServer>();
      this.m_currentMagazineAmmoCount.AlwaysReject<int, SyncDirection.FromServer>();
      this.m_controlInfo.ValueChanged += (Action<SyncBase>) (x => this.ControlChanged());
      this.m_controlInfo.AlwaysReject<MyPlayer.PlayerId, SyncDirection.FromServer>();
      this.m_isShooting = new bool[(int) (MyEnum<MyShootActionEnum>.Range.Max + (byte) 1)];
      this.OnClose += new Action<MyEntity>(this.MyCharacter_OnClose);
      this.OnClosing += new Action<MyEntity>(this.MyEntity_OnClosing);
    }

    private void MyCharacter_OnClose(MyEntity obj)
    {
      if (this.Render == null)
        return;
      this.Render.CleanLights();
    }

    private void MyEntity_OnClosing(MyEntity entity)
    {
      if ((entity as MyCharacter).DeadPlayerIdentityId != MySession.Static.LocalPlayerId)
        return;
      this.RadioReceiver.Clear();
    }

    private static string GetRealModel(string asset, ref Vector3 colorMask)
    {
      if (!string.IsNullOrEmpty(asset) && MyObjectBuilder_Character.CharacterModels.ContainsKey(asset))
      {
        SerializableVector3 characterModel = MyObjectBuilder_Character.CharacterModels[asset];
        if ((double) characterModel.X > -1.0 || (double) characterModel.Y > -1.0 || (double) characterModel.Z > -1.0)
          colorMask = (Vector3) characterModel;
        asset = MyCharacter.DefaultModel;
      }
      return asset;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      this.RadioReceiver = new MyRadioReceiver();
      this.Components.Add<MyDataBroadcaster>((MyDataBroadcaster) new MyRadioBroadcaster());
      this.RadioBroadcaster.BroadcastRadius = 200f;
      this.SyncFlag = true;
      MyObjectBuilder_Character builderCharacter = (MyObjectBuilder_Character) objectBuilder;
      if (builderCharacter.OwningPlayerIdentityId.HasValue)
        this.m_idModule.Owner = builderCharacter.OwningPlayerIdentityId.Value;
      this.Render.ColorMaskHsv = (Vector3) builderCharacter.ColorMaskHSV;
      Vector3 colorMaskHsv = this.Render.ColorMaskHsv;
      MyCharacter.GetModelAndDefinition(builderCharacter, out this.m_characterModel, out this.m_characterDefinition, ref colorMaskHsv);
      this.m_physicalMaterialHash = MyStringHash.GetOrCompute(this.m_characterDefinition.PhysicalMaterial);
      this.UseNewAnimationSystem = this.m_characterDefinition.UseNewAnimationSystem;
      if (this.UseNewAnimationSystem && (!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS))
      {
        this.AnimationController.Clear();
        MyAnimationControllerDefinition definition = MyDefinitionManager.Static.GetDefinition<MyAnimationControllerDefinition>(MyStringHash.GetOrCompute(this.m_characterDefinition.AnimationController));
        if (definition != null)
          this.AnimationController.InitFromDefinition(definition);
      }
      if (this.Render.ColorMaskHsv != colorMaskHsv)
        this.Render.ColorMaskHsv = colorMaskHsv;
      builderCharacter.SubtypeName = this.m_characterDefinition.Id.SubtypeName;
      base.Init(objectBuilder);
      this.m_recoilData.SetLocalValue(new MyRecoilDataCollection()
      {
        Id = 0,
        VerticalValue = 0.0f,
        HorizontalValue = 0.0f
      });
      this.m_currentAnimationChangeDelay = 0.0f;
      this.SoundComp = new MyCharacterSoundComponent();
      this.RadioBroadcaster.WantsToBeEnabled = builderCharacter.EnableBroadcasting && this.Definition.VisibleOnHud;
      this.Init(new StringBuilder(builderCharacter.DisplayName), this.m_characterDefinition.Model, (MyEntity) null, new float?(), (string) null);
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.SIMULATE;
      this.SetStandingLocalAABB();
      this.m_currentLootingCounter = builderCharacter.LootingCounter;
      if ((double) this.m_currentLootingCounter <= 0.0)
        this.UpdateCharacterPhysics();
      this.m_currentMovementState = builderCharacter.MovementState;
      if (this.Physics != null && this.Physics.CharacterProxy != null)
      {
        MyCharacterMovementEnum currentMovementState = this.m_currentMovementState;
        if ((uint) currentMovementState <= 263U)
        {
          switch (currentMovementState)
          {
            case MyCharacterMovementEnum.Flying:
            case MyCharacterMovementEnum.Falling:
              this.Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_IN_AIR);
              goto label_17;
            case MyCharacterMovementEnum.Jump:
              this.Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_JUMPING);
              goto label_17;
            case MyCharacterMovementEnum.Ladder:
            case MyCharacterMovementEnum.LadderUp:
              break;
            default:
              goto label_16;
          }
        }
        else if (currentMovementState != MyCharacterMovementEnum.LadderDown && currentMovementState != MyCharacterMovementEnum.LadderOut)
          goto label_16;
        this.Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_CLIMBING);
        goto label_17;
label_16:
        this.Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_ON_GROUND);
      }
label_17:
      this.InitAnimations();
      this.ValidateBonesProperties();
      this.CalculateTransforms(0.0f);
      this.InitAnimationCorrection();
      if ((double) this.m_currentLootingCounter > 0.0)
      {
        this.InitDeadBodyPhysics();
        if (this.m_currentMovementState != MyCharacterMovementEnum.Died)
          this.SetCurrentMovementState(MyCharacterMovementEnum.Died);
        this.SwitchAnimation(MyCharacterMovementEnum.Died, false);
      }
      this.InitInventory(builderCharacter);
      if (builderCharacter.BuildPlanner != null)
        this.LoadBuildPlanner(builderCharacter.BuildPlanner.ToArray());
      this.Physics.Enabled = true;
      this.SetHeadLocalXAngle(builderCharacter.HeadAngle.X);
      this.SetHeadLocalYAngle(builderCharacter.HeadAngle.Y);
      this.Render.InitLight(this.m_characterDefinition);
      this.Render.InitJetpackThrusts(this.m_characterDefinition);
      this.m_lightEnabled = builderCharacter.LightEnabled;
      this.Physics.LinearVelocity = (Vector3) builderCharacter.LinearVelocity;
      if (this.Physics.CharacterProxy != null)
      {
        this.Physics.CharacterProxy.ContactPointCallbackEnabled = true;
        this.Physics.CharacterProxy.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
      }
      this.Render.UpdateLightProperties(this.m_currentLightPower);
      this.IsInFirstPersonView = !MySession.Static.Settings.Enable3rdPersonView || builderCharacter.IsInFirstPersonView;
      this.m_breath = new MyCharacterBreath(this);
      this.m_notEnoughStatNotification = new MyHudNotification(MyCommonTexts.NotificationStatNotEnough, 1000, "Red", level: MyNotificationLevel.Important);
      if (this.InventoryAggregate != null)
        this.InventoryAggregate.Init();
      this.UseDamageSystem = true;
      if (builderCharacter.EnabledComponents == null)
        builderCharacter.EnabledComponents = new List<string>();
      foreach (string enabledComponent in this.m_characterDefinition.EnabledComponents)
      {
        string componentName = enabledComponent;
        if (builderCharacter.EnabledComponents.All<string>((Func<string, bool>) (x => x != componentName)))
          builderCharacter.EnabledComponents.Add(componentName);
      }
      foreach (string enabledComponent in builderCharacter.EnabledComponents)
      {
        Tuple<System.Type, System.Type> tuple;
        if (MyCharacterComponentTypes.CharacterComponents.TryGetValue(MyStringId.GetOrCompute(enabledComponent), out tuple))
        {
          MyEntityComponentBase instance = Activator.CreateInstance(tuple.Item1) as MyEntityComponentBase;
          this.Components.Add(tuple.Item2, (MyComponentBase) instance);
        }
      }
      if (this.m_characterDefinition.UsesAtmosphereDetector)
      {
        this.AtmosphereDetectorComp = new MyAtmosphereDetectorComponent();
        this.AtmosphereDetectorComp.InitComponent(true, this);
      }
      if (this.m_characterDefinition.UsesReverbDetector)
      {
        this.ReverbDetectorComp = new MyEntityReverbDetectorComponent();
        this.ReverbDetectorComp.InitComponent((MyEntity) this, true);
      }
      int num = this.Definition.SuitResourceStorage.Count > 0 ? 1 : 0;
      List<MyResourceSinkInfo> resourceSinkInfoList1 = new List<MyResourceSinkInfo>();
      List<MyResourceSourceInfo> resourceSourceInfoList = new List<MyResourceSourceInfo>();
      if (num != 0)
      {
        this.OxygenComponent = new MyCharacterOxygenComponent();
        this.Components.Add<MyCharacterOxygenComponent>(this.OxygenComponent);
        this.OxygenComponent.Init(builderCharacter);
        this.OxygenComponent.AppendSinkData(resourceSinkInfoList1);
        this.OxygenComponent.AppendSourceData(resourceSourceInfoList);
      }
      this.m_suitBattery = new MyBattery(this);
      this.m_suitBattery.Init(builderCharacter.Battery, resourceSinkInfoList1, resourceSourceInfoList);
      if (num != 0)
      {
        this.OxygenComponent.CharacterGasSink = this.m_suitBattery.ResourceSink;
        this.OxygenComponent.CharacterGasSource = this.m_suitBattery.ResourceSource;
      }
      resourceSinkInfoList1.Clear();
      List<MyResourceSinkInfo> resourceSinkInfoList2 = resourceSinkInfoList1;
      MyResourceSinkInfo resourceSinkInfo1 = new MyResourceSinkInfo();
      resourceSinkInfo1.ResourceTypeId = MyResourceDistributorComponent.ElectricityId;
      resourceSinkInfo1.MaxRequiredInput = 1.2E-05f;
      resourceSinkInfo1.RequiredInputFunc = new Func<float>(this.ComputeRequiredPower);
      MyResourceSinkInfo resourceSinkInfo2 = resourceSinkInfo1;
      resourceSinkInfoList2.Add(resourceSinkInfo2);
      if (num != 0)
      {
        List<MyResourceSinkInfo> resourceSinkInfoList3 = resourceSinkInfoList1;
        resourceSinkInfo1 = new MyResourceSinkInfo();
        resourceSinkInfo1.ResourceTypeId = MyCharacterOxygenComponent.OxygenId;
        resourceSinkInfo1.MaxRequiredInput = (float) (((double) this.OxygenComponent.OxygenCapacity + (!this.OxygenComponent.NeedsOxygenFromSuit ? (double) this.Definition.OxygenConsumption : 0.0)) * (double) this.Definition.OxygenConsumptionMultiplier * 60.0 / 100.0);
        resourceSinkInfo1.RequiredInputFunc = (Func<float>) (() => (float) ((this.OxygenComponent.HelmetEnabled ? (double) this.Definition.OxygenConsumption : 0.0) * (double) this.Definition.OxygenConsumptionMultiplier * 60.0 / 100.0));
        MyResourceSinkInfo resourceSinkInfo3 = resourceSinkInfo1;
        resourceSinkInfoList3.Add(resourceSinkInfo3);
      }
      this.SinkComp.Init(MyStringHash.GetOrCompute("Utility"), resourceSinkInfoList1);
      this.SinkComp.CurrentInputChanged += (MyCurrentResourceInputChangedDelegate) ((_param1, _param2, _param3) => this.SetPowerInput(this.SinkComp.CurrentInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.SinkComp.TemporaryConnectedEntity = (VRage.ModAPI.IMyEntity) this;
      this.SuitRechargeDistributor = new MyResourceDistributorComponent(this.ToString());
      this.SuitRechargeDistributor.AddSource(this.m_suitBattery.ResourceSource);
      this.SuitRechargeDistributor.AddSink(this.SinkComp);
      this.SinkComp.Update();
      if (this.m_characterDefinition.Jetpack != null)
      {
        this.JetpackComp = new MyCharacterJetpackComponent();
        this.JetpackComp.Init(builderCharacter);
      }
      this.WeaponPosition = new MyCharacterWeaponPositionComponent();
      this.Components.Add<MyCharacterWeaponPositionComponent>(this.WeaponPosition);
      this.WeaponPosition.Init(builderCharacter);
      this.InitWeapon(builderCharacter.HandWeapon);
      if (this.Definition.RagdollBonesMappings.Count > 0)
        this.CreateBodyCapsulesForHits(this.Definition.RagdollBonesMappings);
      else
        this.m_bodyCapsuleInfo.Clear();
      this.PlayCharacterAnimation(this.Definition.InitialAnimation, MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0.0f);
      this.m_savedHealth = builderCharacter.Health;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_previousLinearVelocity = (Vector3) builderCharacter.LinearVelocity;
      this.ControllerInfo.IsLocallyControlled();
      this.CheckExistingStatComponent();
      this.CharacterGeneralDamageModifier = builderCharacter.CharacterGeneralDamageModifier;
      this.m_resolveHighlightOverlap = true;
      this.IsPersistenceCharacter = builderCharacter.IsPersistenceCharacter;
      this.m_bootsState.ValueChanged += new Action<SyncBase>(this.OnBootsStateChanged);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_bootsState.Value = MyBootsState.Init;
      this.m_relativeDampeningEntityInit = builderCharacter.RelativeDampeningEntity;
      this.m_ladderIdInit = builderCharacter.UsingLadder;
      this.m_ladderInfoInit = builderCharacter.UsingLadderInfo;
      this.m_magBootsOnGridSmoothing = new MyMagneticBootsOnGridSmoothing(this);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_enableBroadcastingPlayerToggle.Value = builderCharacter.EnableBroadcastingPlayerToggle;
      else
        this.m_enableBroadcastingPlayerToggle.SetLocalValue(builderCharacter.EnableBroadcastingPlayerToggle);
      this.Physics?.CharacterProxy?.GetHitRigidBody()?.SetProperty(253, 0.0f);
      this.AnimationController.AttachAnimationEventCallback(new Action<List<string>>(this.AnimationEventHandler));
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_isAimAssistSensitivityDecreased.Value = false;
        this.IsAimAssistSnapAllowed = false;
        this.CreateAimAssistHelp();
      }
      else
      {
        this.m_isAimAssistSensitivityDecreased.SetLocalValue(false);
        this.IsAimAssistSnapAllowed = false;
      }
      this.m_isAimAssistSensitivityDecreased.ValueChanged += new Action<SyncBase>(this.AimAssistSensitivityChanged);
      this.IsReloading.ValueChanged += new Action<SyncBase>(this.OnReloadingValueChanged);
      this.m_dynamicRangeDistance.SetLocalValue((-1f, Vector3D.Zero));
    }

    private void OnReloadingValueChanged(SyncBase obj)
    {
      if ((bool) this.IsReloading)
        this.PlayReloadAnimation(true);
      VRage.Sync.Sync<bool, SyncDirection.FromServer> sync = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) obj;
      if (Sandbox.Game.Multiplayer.Sync.IsServer || sync.Value)
        return;
      this.m_isReloadingPrevious = false;
      this.m_shouldPositionHandPrevious = false;
      this.m_reloadHandSimulationState = true;
      this.ShouldPositionMagazine = false;
    }

    private void AimAssistSensitivityChanged(SyncBase obj)
    {
    }

    public bool IsGameAssistEnabled() => MySession.Static.Settings.EnableGamepadAimAssist || MyMultiplayer.Static == null;

    public void CreateAimAssistHelp()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsGameAssistEnabled())
        return;
      HkCapsuleShape hkCapsuleShape = new HkCapsuleShape(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, MyCharacter.AIM_ASSIST_CAPSULE_HEIGHT, 0.0f), MyCharacter.AIM_ASSIST_CAPSULE_RADIUS);
      MyPhysicsBody myPhysicsBody = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_KINEMATIC);
      myPhysicsBody.CreateFromCollisionObject((HkShape) hkCapsuleShape, Vector3.Zero, this.PositionComp.WorldMatrix, collisionFilter: 19);
      myPhysicsBody.Enabled = true;
      myPhysicsBody.Activate();
      this.m_aimAssistPhysics = myPhysicsBody;
    }

    public void AnimationEventHandler(List<string> events)
    {
      foreach (string str in events)
      {
        if (!(str == "MagazinePlug"))
        {
          if (str == "MagazineUnplug")
            this.ShouldPositionMagazine = true;
        }
        else
          this.ShouldPositionMagazine = false;
      }
    }

    private void CheckExistingStatComponent()
    {
      if (this.StatComp != null)
        return;
      bool flag = false;
      MyContainerDefinition definition = (MyContainerDefinition) null;
      MyComponentContainerExtension.TryGetContainerDefinition(this.m_characterDefinition.Id.TypeId, this.m_characterDefinition.Id.SubtypeId, out definition);
      if (definition != null)
      {
        foreach (MyContainerDefinition.DefaultComponent defaultComponent in definition.DefaultComponents)
        {
          if (defaultComponent.BuilderType == typeof (MyObjectBuilder_CharacterStatComponent))
          {
            flag = true;
            break;
          }
        }
      }
      MyLog.Default.WriteLine("Stat component has not been created for character: " + (object) this.m_characterDefinition.Id + ", container defined: " + (definition != null).ToString() + ", stat component defined: " + flag.ToString());
    }

    private void InitAnimationCorrection()
    {
      if (!this.IsDead || !this.UseNewAnimationSystem)
        return;
      this.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdDead, 1f);
    }

    public static void GetModelAndDefinition(
      MyObjectBuilder_Character characterOb,
      out string characterModel,
      out MyCharacterDefinition characterDefinition,
      ref Vector3 colorMask)
    {
      characterModel = MyCharacter.GetRealModel(characterOb.CharacterModel, ref colorMask);
      characterDefinition = (MyCharacterDefinition) null;
      if (!string.IsNullOrEmpty(characterModel) && MyDefinitionManager.Static.Characters.TryGetValue(characterModel, out characterDefinition))
        return;
      characterDefinition = MyDefinitionManager.Static.Characters.First<MyCharacterDefinition>();
      characterModel = characterDefinition.Model;
    }

    private void InitInventory(MyObjectBuilder_Character characterOb)
    {
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        if (this.m_characterDefinition.InventoryDefinition == null)
          this.m_characterDefinition.InventoryDefinition = new MyObjectBuilder_InventoryDefinition();
        MyInventory myInventory = new MyInventory(this.m_characterDefinition.InventoryDefinition, (MyInventoryFlags) 0);
        myInventory.Init((MyObjectBuilder_Inventory) null);
        if (this.InventoryAggregate != null)
          this.InventoryAggregate.AddComponent((MyComponentBase) myInventory);
        else
          this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(characterOb.Inventory);
        MyCubeBuilder.BuildComponent.AfterCharacterCreate(this);
        if (MyFakes.ENABLE_MEDIEVAL_INVENTORY && this.InventoryAggregate != null)
        {
          if (this.InventoryAggregate.GetInventory(MyStringHash.GetOrCompute("Internal")) is MyInventoryAggregate inventory)
            inventory.AddComponent((MyComponentBase) myInventory);
          else
            this.InventoryAggregate.AddComponent((MyComponentBase) myInventory);
        }
      }
      else if (MyPerGameSettings.ConstrainInventory())
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this);
        if (inventory.IsConstrained)
          inventory.FixInventoryVolume(this.m_characterDefinition.InventoryDefinition.InventoryVolume);
      }
      MyEntityExtensions.GetInventory(this).ContentsChanged -= new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      MyEntityExtensions.GetInventory(this).BeforeContentsChanged -= new Action<MyInventoryBase>(this.inventory_OnBeforeContentsChanged);
      MyEntityExtensions.GetInventory(this).BeforeRemovedFromContainer -= new Action<MyEntityComponentBase>(this.inventory_OnRemovedFromContainer);
      MyEntityExtensions.GetInventory(this).ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      MyEntityExtensions.GetInventory(this).BeforeContentsChanged += new Action<MyInventoryBase>(this.inventory_OnBeforeContentsChanged);
      MyEntityExtensions.GetInventory(this).BeforeRemovedFromContainer += new Action<MyEntityComponentBase>(this.inventory_OnRemovedFromContainer);
    }

    private void CreateBodyCapsulesForHits(
      Dictionary<string, MyCharacterDefinition.RagdollBoneSet> bonesMappings)
    {
      this.m_bodyCapsuleInfo.Clear();
      this.m_bodyCapsules = new CapsuleD[bonesMappings.Count];
      foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> bonesMapping in bonesMappings)
      {
        try
        {
          string[] bones = bonesMapping.Value.Bones;
          int index1;
          MyCharacterBone bone1 = this.AnimationController.FindBone(((IEnumerable<string>) bones).First<string>(), out index1);
          int index2;
          MyCharacterBone bone2 = this.AnimationController.FindBone(((IEnumerable<string>) bones).Last<string>(), out index2);
          if (bone1 != null)
          {
            if (bone2 != null)
            {
              if (bone1.Depth > bone2.Depth)
              {
                int num = index1;
                index1 = index2;
                index2 = num;
              }
              this.m_bodyCapsuleInfo.Add(new MyBoneCapsuleInfo()
              {
                Bone1 = bone1.Index,
                Bone2 = bone2.Index,
                AscendantBone = index1,
                DescendantBone = index2,
                Radius = bonesMapping.Value.CollisionRadius
              });
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      for (int index = 0; index < this.m_bodyCapsuleInfo.Count; ++index)
      {
        if (this.m_bodyCapsuleInfo[index].Bone1 == this.m_headBoneIndex)
        {
          this.m_bodyCapsuleInfo.Move<MyBoneCapsuleInfo>(index, 0);
          break;
        }
      }
    }

    private void Toolbar_ItemChanged(MyToolbar toolbar, MyToolbar.IndexArgs index, bool isGamepad)
    {
      MyToolbarItem itemAtIndex = toolbar.GetItemAtIndex(index.ItemIndex);
      if (itemAtIndex != null)
      {
        if (!(itemAtIndex is MyToolbarItemDefinition toolbarItemDefinition))
          return;
        MyDefinitionId id = toolbarItemDefinition.Definition.Id;
        if (id.TypeId != typeof (MyObjectBuilder_PhysicalGunObject))
          MyToolBarCollection.RequestChangeSlotItem(MySession.Static.LocalHumanPlayer.Id, index.ItemIndex, id);
        else
          MyToolBarCollection.RequestChangeSlotItem(MySession.Static.LocalHumanPlayer.Id, index.ItemIndex, itemAtIndex.GetObjectBuilder());
      }
      else
      {
        if (!MySandboxGame.IsGameReady)
          return;
        MyToolBarCollection.RequestClearSlot(MySession.Static.LocalHumanPlayer.Id, index.ItemIndex);
      }
    }

    private void inventory_OnRemovedFromContainer(MyEntityComponentBase component)
    {
      MyEntityExtensions.GetInventory(this).BeforeRemovedFromContainer -= new Action<MyEntityComponentBase>(this.inventory_OnRemovedFromContainer);
      MyEntityExtensions.GetInventory(this).ContentsChanged -= new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      MyEntityExtensions.GetInventory(this).BeforeContentsChanged -= new Action<MyInventoryBase>(this.inventory_OnBeforeContentsChanged);
    }

    private void inventory_OnContentsChanged(MyInventoryBase inventory)
    {
      if (this != MySession.Static.LocalCharacter)
        return;
      if (this.m_currentWeapon != null && this.WeaponTakesBuilderFromInventory(new MyDefinitionId?(this.m_currentWeapon.DefinitionId)) && (inventory != null && inventory is MyInventory) && !(inventory as MyInventory).ContainItems((MyFixedPoint) 1, (MyObjectBuilder_PhysicalObject) this.m_currentWeapon.PhysicalObject))
        this.SwitchToWeapon((MyToolbarItemWeapon) null);
      if (this.LeftHandItem == null || this.CanSwitchToWeapon(new MyDefinitionId?(this.LeftHandItem.DefinitionId)))
        return;
      this.LeftHandItem.OnControlReleased();
      this.m_leftHandItem.Close();
      this.m_leftHandItem = (MyEntity) null;
    }

    private void inventory_OnBeforeContentsChanged(MyInventoryBase inventory)
    {
      if (this != MySession.Static.LocalCharacter || this.m_currentWeapon == null || (!this.WeaponTakesBuilderFromInventory(new MyDefinitionId?(this.m_currentWeapon.DefinitionId)) || inventory == null) || (!(inventory is MyInventory) || !(inventory as MyInventory).ContainItems((MyFixedPoint) 1, (MyObjectBuilder_PhysicalObject) this.m_currentWeapon.PhysicalObject)))
        return;
      this.SaveAmmoToWeapon();
    }

    private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
    {
      if (this.IsDead || this.Physics == null || (this.Physics.CharacterProxy == null || MySession.Static == null))
        return;
      HkRigidBody bodyA = value.Base.BodyA;
      HkRigidBody bodyB = value.Base.BodyB;
      if ((HkReferenceObject) bodyA == (HkReferenceObject) null || (HkReferenceObject) bodyB == (HkReferenceObject) null || (bodyA.UserObject == null || bodyB.UserObject == null))
        return;
      MyVoxelPhysicsBody otherPhysicsBody = value.GetOtherEntity((VRage.ModAPI.IMyEntity) this).Physics as MyVoxelPhysicsBody;
      HkContactPoint contactPoint = value.ContactPoint;
      Vector3 contactPointPosition = contactPoint.Position;
      contactPoint = value.ContactPoint;
      Vector3 contactPointNormal = contactPoint.Normal;
      if (otherPhysicsBody != null)
      {
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          if (this.Render == null)
            return;
          this.Render.TrySpawnWalkingParticles(otherPhysicsBody, contactPointPosition, contactPointNormal);
        }), "MyCharacter.RigidBody_ContactPointCallback.TrySpawnWalkingParticles");
        MyVoxelBase otherEntity = (MyVoxelBase) value.GetOtherEntity((VRage.ModAPI.IMyEntity) this);
        if (this.CanProcessTrails(otherEntity))
        {
          MyPhysicsBody physics = this.Physics;
          contactPoint = value.ContactPoint;
          Vector3 position = contactPoint.Position;
          Vector3D world = physics.ClusterToWorld(position);
          MyVoxelMaterialDefinition materialAt = otherEntity.GetMaterialAt(ref world);
          if (materialAt != null)
            this.ProcessTrails(new MyCharacter.TrailContactProperties()
            {
              ContactEntityId = otherEntity.EntityId,
              ContactPosition = (Vector3) world,
              VoxelMaterial = materialAt.Id.SubtypeId,
              PhysicalMaterial = materialAt.MaterialTypeNameHash
            });
        }
      }
      MyPhysicsBody body1 = bodyA.GetBody();
      MyPhysicsBody body2 = bodyB.GetBody();
      int bodyIdx = 0;
      MyEntity other = body1.Entity as MyEntity;
      HkRigidBody hkRigidBody1 = bodyA;
      contactPoint = value.ContactPoint;
      Vector3 vector2 = contactPoint.Normal;
      if (other == this)
      {
        bodyIdx = 1;
        other = body2.Entity as MyEntity;
        hkRigidBody1 = bodyB;
        vector2 = -vector2;
      }
      if (other is MyCharacter myCharacter && myCharacter.Physics != null && !myCharacter.IsDead && (myCharacter.Physics.CharacterProxy == null || this.Physics.CharacterProxy.Supported && myCharacter.Physics.CharacterProxy.Supported))
        return;
      if (other is MyCubeGrid cubeGrid)
      {
        if (this.IsOnLadder)
        {
          uint shapeKey = value.GetShapeKey(bodyIdx);
          bool flag = shapeKey == uint.MaxValue;
          if (!flag)
          {
            MySlimBlock blockFromShapeKey = cubeGrid.Physics.Shape.GetBlockFromShapeKey(shapeKey);
            if (blockFromShapeKey != null)
              flag = blockFromShapeKey.FatBlock is MyLadder fatBlock && !this.ShouldCollideWith(fatBlock);
          }
          if (flag)
            value.ContactProperties.IsDisabled = true;
        }
        if (MyFakes.ENABLE_REALISTIC_ON_TOUCH && this.SoundComp != null)
          this.SoundComp.UpdateEntityEmitters(cubeGrid);
      }
      if ((double) Math.Abs(value.SeparatingVelocity) < 3.0)
        return;
      Vector3 linearVelocity = this.Physics.LinearVelocity;
      if ((double) (linearVelocity - this.m_previousLinearVelocity).Length() > 10.0)
        return;
      HkRigidBody hkRigidBody2 = hkRigidBody1;
      contactPoint = value.ContactPoint;
      Vector3 position1 = contactPoint.Position;
      Vector3 velocityAtPoint = hkRigidBody2.GetVelocityAtPoint(position1);
      float num1 = linearVelocity.Length();
      float num2 = velocityAtPoint.Length();
      Vector3 vector1_1 = (double) num1 > 0.0 ? Vector3.Normalize(linearVelocity) : Vector3.Zero;
      Vector3 vector1_2 = (double) num2 > 0.0 ? Vector3.Normalize(velocityAtPoint) : Vector3.Zero;
      float num3 = (double) num1 > 0.0 ? Vector3.Dot(vector1_1, vector2) : 0.0f;
      float num4 = (double) num2 > 0.0 ? -Vector3.Dot(vector1_2, vector2) : 0.0f;
      float num5 = num1 * num3;
      float num6 = num2 * num4;
      float num7 = Math.Min(num5 + num6, Math.Abs(value.SeparatingVelocity) - 17f);
      if ((double) num7 >= -8.0 && (double) this.m_canPlayImpact <= 0.0)
      {
        this.m_canPlayImpact = 0.3f;
        HkContactPointEvent hkContactPointEvent = value;
        Func<bool> canHear = (Func<bool>) (() =>
        {
          if (MySession.Static.ControlledEntity == null)
            return false;
          MyEntity topMostParent = MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null);
          return topMostParent == hkContactPointEvent.GetPhysicsBody(0).Entity || topMostParent == hkContactPointEvent.GetPhysicsBody(1).Entity;
        });
        MyPhysicsBody physics = this.Physics;
        contactPoint = value.ContactPoint;
        Vector3 position2 = contactPoint.Position;
        Vector3D world = physics.ClusterToWorld(position2);
        MyPhysicsBody myPhysicsBody = body2;
        Vector3D vector3D = world;
        contactPoint = value.ContactPoint;
        Vector3 vector3 = contactPoint.Normal * 0.1f;
        Vector3D worldPos = vector3D - vector3;
        MyStringHash materialAt = myPhysicsBody.GetMaterialAt(worldPos);
        float volume = (double) Math.Abs(value.SeparatingVelocity) < 15.0 ? (float) (0.5 + (double) Math.Abs(value.SeparatingVelocity) / 30.0) : 1f;
        MyAudioComponent.PlayContactSound(this.Entity.EntityId, MyCharacter.m_stringIdHit, world, this.m_physicalMaterialHash, materialAt, volume, canHear);
      }
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || (double) num7 < 0.0)
        return;
      float num8 = MyDestructionHelper.MassFromHavok(this.Physics.Mass);
      float num9 = MyDestructionHelper.MassFromHavok(hkRigidBody1.Mass);
      float m;
      if ((double) num8 > (double) num9 && !hkRigidBody1.IsFixedOrKeyframed)
      {
        m = num9;
      }
      else
      {
        m = MyDestructionHelper.MassToHavok(70f);
        if (this.Physics.CharacterProxy.Supported && !hkRigidBody1.IsFixedOrKeyframed)
          m += (float) ((double) Math.Abs(Vector3.Dot(Vector3.Normalize(velocityAtPoint), this.Physics.CharacterProxy.SupportNormal)) * (double) num9 / 10.0);
      }
      float impact = (float) ((double) MyDestructionHelper.MassFromHavok(m) * (double) num7 * (double) num7 / 2.0);
      if ((double) num6 > 2.0)
        impact -= 400f;
      else if ((double) num6 == 0.0 && (double) impact > 100.0)
        impact /= 80f;
      impact /= 10f;
      if ((double) impact < 1.0 || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      VRage.ModAPI.IMyEntity entity = value.GetPhysicsBody(0).Entity;
      if (entity == this)
        entity = value.GetPhysicsBody(1).Entity;
      if (!MySession.Static.Settings.EnableFriendlyFire && entity is MyMissile myMissile)
      {
        if (myMissile.IsCharacterIdFriendly(this.GetIdentity().IdentityId))
          return;
        MySandboxGame.Static.Invoke((Action) (() => this.DoDamage(impact, MyDamageType.Environment, true, other != null ? other.EntityId : 0L)), "MyCharacter.DoDamage");
      }
      else
        MySandboxGame.Static.Invoke((Action) (() => this.DoDamage(impact, MyDamageType.Environment, true, other != null ? other.EntityId : 0L)), "MyCharacter.DoDamage");
    }

    private bool CanProcessTrails(MyVoxelBase otherEntity)
    {
      if (otherEntity == null || this.JetpackRunning || !this.IsPlayer)
        return false;
      MyStringHash footprintDecal = this.Definition.FootprintDecal;
      return !string.IsNullOrEmpty(this.Definition.FootprintDecal.String);
    }

    private void ProcessTrails(
      MyCharacter.TrailContactProperties contactProperties)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      float forMovementState = this.GetMovementMultiplierForMovementState(this.m_currentMovementState);
      float num = (float) (5.59999990463257 * (this.IsCrouching ? 0.200000002980232 : 1.0)) * forMovementState;
      Vector3D vector3D1;
      if (!this.m_forceStandingFootprints && this.LastTrail != null)
      {
        vector3D1 = this.LastTrail.Position - contactProperties.ContactPosition;
        if (vector3D1.LengthSquared() < (double) num)
          return;
      }
      IReadOnlyList<MyDecalMaterial> decalMaterials = (IReadOnlyList<MyDecalMaterial>) null;
      MyStringHash footStepStringHash = this.m_footStepStringHash;
      bool decalMaterial = MyDecalMaterials.TryGetDecalMaterial(footStepStringHash.String, contactProperties.PhysicalMaterial.String, out decalMaterials, contactProperties.VoxelMaterial);
      if (!decalMaterial)
      {
        footStepStringHash = this.m_footStepStringHash;
        decalMaterial = MyDecalMaterials.TryGetDecalMaterial(footStepStringHash.String, "GenericMaterial", out decalMaterials, contactProperties.VoxelMaterial);
      }
      if (!decalMaterial || decalMaterials == null || decalMaterials.Count <= 0)
        return;
      IMyTrackTrails myTrackTrails = (IMyTrackTrails) this;
      Vector3 contactPosition1 = contactProperties.ContactPosition;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D up1 = worldMatrix.Up;
      Vector3D from = contactPosition1 + up1;
      Vector3 contactPosition2 = contactProperties.ContactPosition;
      worldMatrix = this.WorldMatrix;
      Vector3D up2 = worldMatrix.Up;
      Vector3D to = contactPosition2 - up2;
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(from, to, 28);
      if (nullable.HasValue)
      {
        if (this.m_forceStandingFootprints)
        {
          if (this.LastTrail != null)
          {
            Vector3D vector3D2 = this.LastTrail.Position - contactProperties.ContactPosition;
            worldMatrix = this.WorldMatrix;
            Vector3D vector3D3 = worldMatrix.Up * 0.0500000007450581;
            vector3D1 = vector3D2 + vector3D3;
            if (vector3D1.LengthSquared() <= 0.559999990463257)
            {
              this.LastTrail = (MyTrailProperties) null;
              myTrackTrails.AddTrails(nullable.Value.Position, (Vector3D) nullable.Value.HkHitInfo.Normal, (Vector3D) this.m_lastRotation.Forward, contactProperties.ContactEntityId, decalMaterials[0].Target, contactProperties.VoxelMaterial);
              goto label_15;
            }
          }
          this.LastTrail = (MyTrailProperties) null;
          myTrackTrails.AddTrails(nullable.Value.Position, (Vector3D) nullable.Value.HkHitInfo.Normal, (Vector3D) this.m_lastRotation.Forward, contactProperties.ContactEntityId, decalMaterials[0].Target, contactProperties.VoxelMaterial);
          this.LastTrail = (MyTrailProperties) null;
          myTrackTrails.AddTrails(nullable.Value.Position, (Vector3D) nullable.Value.HkHitInfo.Normal, (Vector3D) this.m_lastRotation.Forward, contactProperties.ContactEntityId, decalMaterials[0].Target, contactProperties.VoxelMaterial);
        }
        else
          myTrackTrails.AddTrails(nullable.Value.Position, (Vector3D) nullable.Value.HkHitInfo.Normal, (Vector3D) this.m_lastRotation.Forward, contactProperties.ContactEntityId, decalMaterials[0].Target, contactProperties.VoxelMaterial);
      }
label_15:
      this.m_forceStandingFootprints = false;
    }

    private void InitWeapon(MyObjectBuilder_EntityBase weapon)
    {
      if (weapon == null)
        return;
      if ((this.m_rightHandItemBone == -1 || weapon != null) && this.m_currentWeapon != null)
        this.DisposeWeapon();
      MyPhysicalItemDefinition physicalItemForHandItem = MyDefinitionManager.Static.GetPhysicalItemForHandItem(weapon.GetId());
      if (!(this.m_rightHandItemBone != -1 & (physicalItemForHandItem != null && (!MySession.Static.SurvivalMode || MyEntityExtensions.GetInventory(this).GetItemAmount(physicalItemForHandItem.Id, MyItemFlags.None, false) > (MyFixedPoint) 0))))
        return;
      this.m_currentWeapon = MyCharacter.CreateGun(weapon);
      ((MyEntity) this.m_currentWeapon).Render.DrawInAllCascades = true;
    }

    private void ValidateBonesProperties()
    {
      if (this.m_rightHandItemBone != -1 || this.m_currentWeapon == null)
        return;
      this.DisposeWeapon();
    }

    private void DisposeWeapon()
    {
      if (!(this.m_currentWeapon is MyEntity mCurrentWeapon))
        return;
      mCurrentWeapon.EntityId = 0L;
      mCurrentWeapon.Close();
      this.m_currentWeapon = (IMyHandheldGunObject<MyDeviceBase>) null;
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_Character objectBuilder = (MyObjectBuilder_Character) base.GetObjectBuilder(copy);
      objectBuilder.CharacterModel = this.m_characterModel;
      objectBuilder.ColorMaskHSV = (SerializableVector3) this.ColorMask;
      objectBuilder.Inventory = MyEntityExtensions.GetInventory(this) == null || MyFakes.ENABLE_MEDIEVAL_INVENTORY ? (MyObjectBuilder_Inventory) null : MyEntityExtensions.GetInventory(this).GetObjectBuilder();
      if (this.m_currentWeapon != null)
        objectBuilder.HandWeapon = ((MyEntity) this.m_currentWeapon).GetObjectBuilder(false);
      objectBuilder.Battery = this.m_suitBattery.GetObjectBuilder();
      objectBuilder.LightEnabled = this.m_lightEnabled;
      if (this.IsOnLadder)
      {
        objectBuilder.UsingLadder = new long?(this.m_ladder.EntityId);
        objectBuilder.UsingLadderInfo = new MyObjectBuilder_Character.LadderInfo?(new MyObjectBuilder_Character.LadderInfo()
        {
          BaseMatrix = new MyPositionAndOrientation(ref this.m_baseMatrix),
          IncrementToBase = (SerializableVector3) this.m_ladderIncrementToBase
        });
      }
      else
      {
        objectBuilder.UsingLadder = new long?();
        objectBuilder.UsingLadderInfo = new MyObjectBuilder_Character.LadderInfo?();
      }
      objectBuilder.HeadAngle = (SerializableVector2) new Vector2(this.m_headLocalXAngle, this.m_headLocalYAngle);
      objectBuilder.LinearVelocity = (SerializableVector3) (this.Physics != null ? this.Physics.LinearVelocity : Vector3.Zero);
      objectBuilder.Health = new float?();
      objectBuilder.LootingCounter = this.m_currentLootingCounter;
      objectBuilder.DisplayName = this.DisplayName;
      objectBuilder.CharacterGeneralDamageModifier = this.CharacterGeneralDamageModifier;
      objectBuilder.IsInFirstPersonView = Sandbox.Engine.Platform.Game.IsDedicated || this.m_isInFirstPersonView;
      objectBuilder.EnableBroadcasting = this.RadioBroadcaster.WantsToBeEnabled;
      objectBuilder.MovementState = this.m_currentMovementState;
      if (this.Components != null)
      {
        if (objectBuilder.EnabledComponents == null)
          objectBuilder.EnabledComponents = new List<string>();
        foreach (MyComponentBase component in (MyComponentContainer) this.Components)
        {
          foreach (KeyValuePair<MyStringId, Tuple<System.Type, System.Type>> characterComponent in MyCharacterComponentTypes.CharacterComponents)
          {
            if (characterComponent.Value.Item2 == component.GetType() && !objectBuilder.EnabledComponents.Contains(characterComponent.Key.ToString()))
              objectBuilder.EnabledComponents.Add(characterComponent.Key.ToString());
          }
        }
        if (this.JetpackComp != null)
          this.JetpackComp.GetObjectBuilder(objectBuilder);
        if (this.OxygenComponent != null)
          this.OxygenComponent.GetObjectBuilder(objectBuilder);
      }
      objectBuilder.OwningPlayerIdentityId = this.m_idModule.Owner == 0L ? new long?(Sandbox.Game.Multiplayer.Sync.Players.TryGetIdentityId(this.m_controlInfo.Value.SteamId, this.m_controlInfo.Value.SerialId)) : new long?(this.m_idModule.Owner);
      objectBuilder.IsPersistenceCharacter = this.IsPersistenceCharacter;
      objectBuilder.RelativeDampeningEntity = this.RelativeDampeningEntity != null ? this.RelativeDampeningEntity.EntityId : 0L;
      if (this.GetIdentity() != null && this.GetIdentity().BuildPlanner.Count > 0)
        objectBuilder.BuildPlanner = ((IEnumerable<MyObjectBuilder_Character.BuildPlanItem>) this.SaveBuildPlanner()).ToList<MyObjectBuilder_Character.BuildPlanItem>();
      objectBuilder.EnableBroadcastingPlayerToggle = (bool) this.m_enableBroadcastingPlayerToggle;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    protected override void Closing()
    {
      this.CloseInternal();
      if (this.m_breath != null)
        this.m_breath.Close();
      base.Closing();
    }

    private void CloseInternal()
    {
      if (this.m_currentWeapon != null)
      {
        ((MyEntity) this.m_currentWeapon).Close();
        this.m_currentWeapon = (IMyHandheldGunObject<MyDeviceBase>) null;
      }
      if (this.m_leftHandItem != null)
      {
        this.m_leftHandItem.Close();
        this.m_leftHandItem = (MyEntity) null;
      }
      if (this.m_magBootsOnGridSmoothing != null)
        this.m_magBootsOnGridSmoothing = (MyMagneticBootsOnGridSmoothing) null;
      this.RemoveNotifications();
      if (this.IsOnLadder)
      {
        this.CloseLadderConstraint(this.m_ladder.CubeGrid);
        this.m_ladder.IsWorkingChanged -= new Action<MyCubeBlock>(this.MyLadder_IsWorkingChanged);
      }
      this.RadioBroadcaster.Enabled = false;
      if (MyToolbarComponent.CharacterToolbar == null)
        return;
      MyToolbarComponent.CharacterToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
    }

    public bool InheritRotation => !this.JetpackRunning && !this.IsFalling && !this.IsJumping;

    public void UpdatePredictionFlag()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsDead)
      {
        this.IsClientPredicted = true;
      }
      else
      {
        if (this.ForceDisablePrediction && MySandboxGame.Static.SimulationTime.Seconds > this.m_forceDisablePredictionTime + 10.0)
          this.ForceDisablePrediction = false;
        bool flag1 = MySession.Static.TopMostControlledEntity == this;
        bool flag2 = MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER & flag1 && !this.IsDead && (!this.JetpackRunning || MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK) && !this.ForceDisablePrediction && !this.AlwaysDisablePrediction;
        if (this.ControllerInfo.IsLocallyControlled())
        {
          HkShape shape = HkShape.Empty;
          MyCharacterProxy characterProxy = this.Physics.CharacterProxy;
          if (characterProxy != null)
            shape = characterProxy.GetCollisionShape();
          if (!shape.IsZero)
          {
            using (MyUtils.ReuseCollection<HkBodyCollision>(ref this.m_physicsCollisionResults))
            {
              MatrixD matrix = this.Physics.GetWorldMatrix();
              matrix.Translation += Vector3D.TransformNormal((Vector3D) this.Physics.Center, ref matrix);
              Vector3D translation = matrix.Translation;
              Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
              MyPhysics.GetPenetrationsShape(shape, ref translation, ref fromRotationMatrix, this.m_physicsCollisionResults, 30);
              foreach (HkBodyCollision physicsCollisionResult in this.m_physicsCollisionResults)
              {
                if (physicsCollisionResult.Body.UserObject is MyGridPhysics userObject)
                {
                  if (this.IsOnLadder)
                  {
                    MySlimBlock blockFromShapeKey = userObject.Shape.GetBlockFromShapeKey(physicsCollisionResult.ShapeKey);
                    if (blockFromShapeKey != null && blockFromShapeKey.FatBlock is MyLadder && !this.ShouldCollideWith(blockFromShapeKey.FatBlock as MyLadder))
                    {
                      this.ForceDisablePrediction = false;
                      flag2 = true;
                      continue;
                    }
                  }
                  this.ForceDisablePrediction = true;
                  flag2 = false;
                  break;
                }
              }
            }
          }
        }
        if (this.IsClientPredicted != flag2)
          this.IsClientPredicted = flag2;
        Sandbox.Game.Entities.MyEntities.InvokeLater(new Action(this.UpdateCharacterPhysics));
      }
    }

    private void UpdateFirstPerson()
    {
      this.m_isInFirstPerson = MySession.Static.CameraController == this && this.IsInFirstPersonView;
      bool flag1 = this.ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == this;
      bool flag2 = ((this.m_isInFirstPerson ? 1 : (this.ForceFirstPersonCamera ? 1 : 0)) & (flag1 ? 1 : 0)) != 0;
      if (this.m_wasInFirstPerson != flag2 && this.m_currentMovementState != MyCharacterMovementEnum.Sitting)
      {
        MySector.MainCamera.Zoom.ApplyToFov = flag2;
        this.UpdateNearFlag();
      }
      this.m_wasInFirstPerson = flag2;
    }

    public override void Simulate()
    {
      base.Simulate();
      this.SimulateCachedCommands(true);
      this.MoveAndRotateInternal();
      this.SimulateCachedCommands(false);
      this.SimulateComponents();
      this.UpdateMovement();
    }

    private void MoveAndRotateInternal()
    {
      if (!this.ControllerInfo.IsLocallyControlled() && (this.IsUsing != null && !this.IsOnLadder || (this.m_cachedCommands == null || this.m_cachedCommands.Count != 0)) && (this.Parent != null || !MySessionComponentReplay.Static.IsEntityBeingReplayed(this.EntityId)))
        return;
      this.MoveAndRotateInternal(this.MoveIndicator, this.RotationIndicator, this.RollIndicator, this.RotationCenterIndicator);
    }

    private void SimulateCachedCommands(bool beforeMoveAndRotate)
    {
      if (this.m_cachedCommands == null)
        return;
      if (this.IsUsing != null && !this.IsOnLadder || this.IsDead)
        this.m_cachedCommands.Clear();
      foreach (IMyNetworkCommand cachedCommand in this.m_cachedCommands)
      {
        if (cachedCommand.ExecuteBeforeMoveAndRotate == beforeMoveAndRotate)
          cachedCommand.Apply();
      }
      if (beforeMoveAndRotate)
        return;
      this.m_cachedCommands.Clear();
    }

    private void SimulateComponents()
    {
      foreach (MyComponentBase component in (MyComponentContainer) this.Components)
      {
        if (component is MyCharacterComponent characterComponent && characterComponent.NeedsUpdateSimulation)
          characterComponent.Simulate();
      }
    }

    private void UpdateMovement()
    {
      if (this.IsDead || this.m_currentMovementState == MyCharacterMovementEnum.Sitting || (MySandboxGame.IsPaused || this.Physics.CharacterProxy == null))
        return;
      Vector3 linearVelocity1 = this.Physics.LinearVelocity;
      Vector3 angularVelocity1 = this.Physics.AngularVelocity;
      if (!this.JetpackRunning)
      {
        bool supported1 = this.Physics.CharacterProxy.Supported;
        this.Physics.CharacterProxy.GetSupportingEntities(this.m_supportedEntitiesTmp);
        this.Physics.CharacterProxy.StepSimulation(0.01666667f);
        bool supported2 = this.Physics.CharacterProxy.Supported;
        if (((Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (!supported2 ? 1 : 0)) & (supported1 ? 1 : 0)) != 0 && this.m_supportedEntitiesTmp.Count > 0 && (HkReferenceObject) this.m_supportedEntitiesTmp[0].Physics?.RigidBody != (HkReferenceObject) null)
        {
          Vector3D translation = this.WorldMatrix.Translation;
          Vector3 linearVelocity2;
          this.m_supportedEntitiesTmp[0].Physics.GetVelocityAtPointLocal(ref translation, out linearVelocity2);
          Vector3 vector3 = this.Physics.LinearVelocity - this.Physics.LinearVelocityLocal;
          this.Physics.LinearVelocity = this.Physics.LinearVelocityLocal + linearVelocity2 - vector3;
        }
        this.m_supportedEntitiesTmp.Clear();
      }
      else
      {
        this.Physics.CharacterProxy.UpdateSupport(0.01666667f);
        this.Physics.CharacterProxy.ApplyGravity(this.Physics.Gravity);
        MyCharacterDefinition definition = this.Definition;
        Vector3 angularVelocity2 = this.Physics.CharacterProxy.AngularVelocity;
        float num = angularVelocity2.Length();
        if ((double) num < (double) definition.RecoilJetpackDampeningRadPerFrame || this.m_currentWeapon == null || !this.m_currentWeapon.IsRecoiling)
          this.Physics.CharacterProxy.AngularVelocity = Vector3.Zero;
        else if ((double) num != 0.0)
          this.Physics.CharacterProxy.AngularVelocity = (num - definition.RecoilJetpackDampeningRadPerFrame) / num * angularVelocity2;
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsClientPredicted)
        return;
      this.Physics.LinearVelocity = linearVelocity1;
      this.Physics.AngularVelocity = angularVelocity1;
    }

    public void UpdateLightPower(bool chargeImmediately = false)
    {
      float currentLightPower = this.m_currentLightPower;
      this.m_currentLightPower = (double) this.m_lightPowerFromProducer <= 0.0 || !this.m_lightEnabled ? (chargeImmediately ? 0.0f : MathHelper.Clamp(this.m_currentLightPower - this.m_lightTurningOffSpeed, 0.0f, 1f)) : (!chargeImmediately ? MathHelper.Clamp(this.m_currentLightPower + this.m_lightTurningOnSpeed, 0.0f, 1f) : 1f);
      if (this.Render == null)
        return;
      this.Render.UpdateLight(this.m_currentLightPower, (double) currentLightPower != (double) this.m_currentLightPower, MyCharacter.LIGHT_PARAMETERS_CHANGED);
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      this.SuitRechargeDistributor.UpdateBeforeSimulation();
      this.RadioReceiver.UpdateBroadcastersInRange();
      if (this != MySession.Static.LocalCharacter)
        return;
      this.RadioReceiver.UpdateHud();
    }

    public bool HasAccessToLogicalGroup(MyGridLogicalGroupData group) => this.RadioReceiver.HasAccessToLogicalGroup(group);

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      this.m_suitBattery.UpdateOnServer100();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.m_suitBattery.ResourceSource.HasCapacityRemaining)
      {
        MyTemperatureLevel level = MySectorWeatherComponent.TemperatureToLevel(this.GetOutsideTemperature());
        float damage = 0.0f;
        switch (level)
        {
          case MyTemperatureLevel.ExtremeFreeze:
          case MyTemperatureLevel.ExtremeHot:
            damage = 5f;
            break;
          case MyTemperatureLevel.Freeze:
          case MyTemperatureLevel.Hot:
            damage = 2f;
            break;
        }
        if ((double) damage > 0.0)
          this.DoDamage(damage, MyDamageType.Temperature, true, 0L);
      }
      foreach (MyComponentBase component in (MyComponentContainer) this.Components)
      {
        if (component is MyCharacterComponent characterComponent && characterComponent.NeedsUpdateBeforeSimulation100)
          characterComponent.UpdateBeforeSimulation100();
      }
      if (this.AtmosphereDetectorComp != null)
        this.AtmosphereDetectorComp.UpdateAtmosphereStatus();
      if (this.m_relativeDampeningEntityInit != 0L && this.JetpackComp != null && !this.JetpackComp.DampenersTurnedOn)
        this.m_relativeDampeningEntityInit = 0L;
      if (this.RelativeDampeningEntity == null && this.m_relativeDampeningEntityInit != 0L)
      {
        this.RelativeDampeningEntity = Sandbox.Game.Entities.MyEntities.GetEntityByIdOrDefault(this.m_relativeDampeningEntityInit);
        if (this.RelativeDampeningEntity != null)
          this.m_relativeDampeningEntityInit = 0L;
      }
      if (this.RelativeDampeningEntity == null)
        return;
      MyEntityThrustComponent.UpdateRelativeDampeningEntity((Sandbox.Game.Entities.IMyControllableEntity) this, this.RelativeDampeningEntity);
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      foreach (MyComponentBase component in (MyComponentContainer) this.Components)
      {
        if (component is MyCharacterComponent characterComponent && characterComponent.NeedsUpdateAfterSimulation10)
          characterComponent.UpdateAfterSimulation10();
      }
      this.UpdateCameraDistance();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.UpdateBootsStateAndEmmisivity();
    }

    private void UpdateBootsStateAndEmmisivity()
    {
      this.m_needsUpdateBoots = false;
      if (this.IsMagneticBootsEnabled && !this.IsDead && (!this.IsSitting && this.Physics.CharacterProxy.Supported))
        this.m_bootsState.Value = MyBootsState.Enabled;
      else if ((this.JetpackRunning || this.IsFalling || this.IsJumping) && (this.Physics.CharacterProxy != null && this.Physics.CharacterProxy.Supported && (double) this.m_gravity.LengthSquared() < 1.0 / 1000.0))
        this.m_bootsState.Value = MyBootsState.Proximity;
      else
        this.m_bootsState.Value = MyBootsState.Disabled;
    }

    private void OnBootsStateChanged(SyncBase obj)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated && this.SoundComp != null && this.Render != null)
      {
        switch (this.m_bootsState.Value)
        {
          case MyBootsState.Disabled:
            MyRenderProxy.UpdateColorEmissivity(this.Render.RenderObjectIDs[0], 0, "Emissive", Color.White, 0.0f);
            this.SoundComp.PlayMagneticBootsEnd();
            break;
          case MyBootsState.Proximity:
            MyRenderProxy.UpdateColorEmissivity(this.Render.RenderObjectIDs[0], 0, "Emissive", Color.Yellow, 1f);
            this.SoundComp.PlayMagneticBootsProximity();
            break;
          case MyBootsState.Enabled:
            MyRenderProxy.UpdateColorEmissivity(this.Render.RenderObjectIDs[0], 0, "Emissive", Color.ForestGreen, 1f);
            this.SoundComp.PlayMagneticBootsStart();
            break;
        }
      }
      this.m_movementsFlagsChanged = true;
    }

    private void UpdateCameraDistance() => this.m_cameraDistance = (float) Vector3D.Distance(MySector.MainCamera.Position, this.WorldMatrix.Translation);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      MyHud.Crosshair.Recenter();
      if (this.m_currentWeapon == null)
        return;
      this.m_currentWeapon.DrawHud(camera, playerId);
    }

    public bool NeedsPerFrameUpdate => this.GetCurrentMovementState() != MyCharacterMovementEnum.Sitting || !(this.IsUsing is MyCryoChamber) || (Sandbox.Game.Multiplayer.Sync.Players.IsPlayerOnline(this.ControllerInfo.ControllingIdentityId) || MySession.Static.LocalCharacter == this) || this.StatComp != null && this.StatComp.Health != null && (double) this.StatComp.Health.Value < (double) MySpaceStatEffect.MAX_REGEN_HEALTH_RATIO;

    private void UpdateCharacterStateChange()
    {
      if (this.IsDead || this.Physics.CharacterProxy == null)
        return;
      this.OnCharacterStateChanged(this.Physics.CharacterProxy.GetState());
    }

    private void UpdateRespawnAndLooting()
    {
      if ((double) this.m_currentRespawnCounter > 0.0)
      {
        MyPlayer player = this.TryGetPlayer();
        this.m_currentRespawnCounter -= 0.01666667f;
        if (this.m_respawnNotification != null)
          this.m_respawnNotification.SetTextFormatArguments((object) (int) this.m_currentRespawnCounter);
        if ((double) this.m_currentRespawnCounter <= 0.0 && Sandbox.Game.Multiplayer.Sync.IsServer && player != null)
          Sandbox.Game.Multiplayer.Sync.Players.KillPlayer(player);
      }
      this.UpdateLooting(0.01666667f);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.UpdateAssetModifiers();
      this.SoundComp.UpdateAfterSimulation100();
      this.UpdateOutsideTemperature();
      if (MyMultiplayer.Static != null && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.IsClientOnline = new bool?(Sandbox.Game.Multiplayer.Sync.Players.IsPlayerOnline(this.GetPlayerIdentityId()));
      else
        this.IsClientOnline = new bool?();
    }

    private bool UpdateLooting(float amount)
    {
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
        MyRenderProxy.DebugDrawText3D(this.WorldMatrix.Translation, this.m_currentLootingCounter.ToString("n1"), Color.Green, 1f, false);
      if ((double) this.m_currentLootingCounter > 0.0)
      {
        this.m_currentLootingCounter -= amount;
        if ((double) this.m_currentLootingCounter <= 0.0 && Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          this.Close();
          this.Save = false;
          return true;
        }
      }
      return false;
    }

    private void UpdateBobQueue()
    {
      int index = this.IsInFirstPersonView ? this.m_headBoneIndex : this.m_camera3rdBoneIndex;
      if (index == -1)
        return;
      this.m_bobQueue.Enqueue(this.BoneAbsoluteTransforms[index].Translation);
      int num = this.m_currentMovementState == MyCharacterMovementEnum.Standing || this.m_currentMovementState == MyCharacterMovementEnum.Sitting || (this.m_currentMovementState == MyCharacterMovementEnum.Crouching || this.m_currentMovementState == MyCharacterMovementEnum.RotatingLeft) || (this.m_currentMovementState == MyCharacterMovementEnum.RotatingRight || this.m_currentMovementState == MyCharacterMovementEnum.Died) ? 5 : 20;
      if (this.WantsCrouch)
        num = 3;
      while (this.m_bobQueue.Count > num)
        this.m_bobQueue.Dequeue();
    }

    private void UpdateFallAndSpine()
    {
      MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
      if (this.JetpackComp != null)
        this.JetpackComp.UpdateFall();
      if (this.m_isFalling && !this.JetpackRunning)
      {
        this.m_currentFallingTime += 0.01666667f;
        if ((double) this.m_currentFallingTime > 0.300000011920929 && !this.m_isFallingAnimationPlayed)
        {
          this.SwitchAnimation(MyCharacterMovementEnum.Falling, false);
          this.m_isFallingAnimationPlayed = true;
        }
      }
      if ((!this.JetpackRunning || jetpackComp.Running && (this.IsLocalHeadAnimationInProgress() || this.Definition.VerticalPositionFlyingOnly)) && (!this.IsDead && !this.IsSitting && !this.IsOnLadder))
      {
        float num1 = this.IsInFirstPersonView ? this.m_characterDefinition.BendMultiplier1st : this.m_characterDefinition.BendMultiplier3rd;
        if (this.UseNewAnimationSystem)
        {
          float num2 = this.m_characterDefinition.BendMultiplier3rd * MathHelper.Clamp(-this.m_headLocalXAngle, -89.9f, 89f);
          if (MySession.Static.LocalCharacter == this && (!MyControllerHelper.IsControl(MyControllerHelper.CX_CHARACTER, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) || this.IsInFirstPersonView || (this.ForceFirstPersonCamera || this.CurrentWeapon != null)))
            this.m_animLeaning.Value = num2;
        }
        else
        {
          float num2 = MathHelper.Clamp(-this.m_headLocalXAngle, -45f, 89f);
          this.SetSpineAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(num1 * num2)), Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(this.m_characterDefinition.BendMultiplier3rd * num2)));
        }
      }
      else if (this.UseNewAnimationSystem)
        this.m_animLeaning.Value = 0.0f;
      else
        this.SetSpineAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Backward, 0.0f), Quaternion.CreateFromAxisAngle(Vector3.Backward, 0.0f));
      if (this.m_currentWeapon == null && !this.IsDead && (!this.JetpackRunning && !this.IsSitting))
      {
        double headLocalXangle1 = (double) this.m_headLocalXAngle;
        double headLocalXangle2 = (double) this.m_headLocalXAngle;
      }
      else
      {
        this.SetHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0.0f)));
        this.SetUpperHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0.0f)));
      }
    }

    private void UpdateShooting()
    {
      if (this.IsClientOnline.HasValue && !this.IsClientOnline.Value)
        return;
      if (this.m_currentWeapon != null)
      {
        if (MySession.Static.LocalCharacter == this && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay) && MyScreenManager.IsAnyScreenOpening() && (MyInput.Static.IsGameControlPressed(MyControlsSpace.PRIMARY_TOOL_ACTION) || MyInput.Static.IsGameControlPressed(MyControlsSpace.SECONDARY_TOOL_ACTION)))
          this.EndShootAll();
        this.UpdateAmmoState();
        if (this.m_currentWeapon.IsShooting)
          this.m_currentShootPositionTime = 0.1f;
        this.ShootInternal();
        if (this.m_currentWeapon != null)
        {
          if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_currentWeapon.CanReload() && this.m_currentWeapon.NeedsReload)
          {
            if (!this.m_currentWeapon.IsReloading)
            {
              this.m_currentWeapon.Reload();
              this.PlayReloadAnimation();
            }
          }
          else if (this.IsIronSighted && this.m_currentWeapon.IsReloading)
            this.EnableIronsight(false, true, true, forceChange: true);
        }
      }
      else if (this.m_usingByPrimary)
      {
        Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        if (!MyControllerHelper.IsControl(controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED))
          this.m_usingByPrimary = false;
        this.UseContinues();
      }
      if ((double) this.m_currentShotTime > 0.0)
      {
        this.m_currentShotTime -= 0.01666667f;
        if ((double) this.m_currentShotTime <= 0.0)
          this.m_currentShotTime = 0.0f;
      }
      if ((double) this.m_currentShootPositionTime <= 0.0)
        return;
      this.m_currentShootPositionTime -= 0.01666667f;
      if ((double) this.m_currentShootPositionTime > 0.0)
        return;
      this.m_currentShootPositionTime = 0.0f;
    }

    internal void UpdatePhysicalMovement()
    {
      if (!MySandboxGame.IsGameReady || this.Physics == null || (!this.Physics.Enabled || !MySession.Static.Ready) || this.Physics.HavokWorld == null)
        return;
      MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
      bool flag1 = jetpackComp != null && jetpackComp.UpdatePhysicalMovement();
      bool flag2 = MyGravityProviderSystem.IsGravityReady();
      this.m_gravity = MyGravityProviderSystem.CalculateTotalGravityInPoint(this.PositionComp.WorldAABB.Center) + this.Physics.HavokWorld.Gravity;
      if ((double) this.m_gravity.Length() > 100.0)
      {
        double num = (double) this.m_gravity.Normalize();
        this.m_gravity *= 100f;
      }
      MatrixD worldMatrix1 = this.WorldMatrix;
      bool flag3 = false;
      bool flag4 = true;
      if ((!flag1 || this.Definition.VerticalPositionFlyingOnly || this.IsMagneticBootsEnabled) && (!this.IsDead && !this.IsOnLadder))
      {
        Vector3 up = (Vector3) worldMatrix1.Up;
        Vector3 forward = (Vector3) worldMatrix1.Forward;
        if (this.Physics.CharacterProxy != null)
        {
          Vector3 vector3 = this.Physics.CharacterProxy.Up;
          if (vector3.IsValid())
          {
            vector3 = this.Physics.CharacterProxy.Forward;
            if (vector3.IsValid())
              goto label_9;
          }
          this.Physics.CharacterProxy.SetForwardAndUp((Vector3) worldMatrix1.Forward, (Vector3) worldMatrix1.Up);
label_9:
          up = this.Physics.CharacterProxy.Up;
          forward = this.Physics.CharacterProxy.Forward;
          this.Physics.CharacterProxy.Gravity = flag1 ? Vector3.Zero : this.m_gravity * MyPerGameSettings.CharacterGravityMultiplier;
        }
        if (MyFakes.ENABLE_CHARACTER_IK_FEET_OFFSET)
          this.m_characterFeetOffset = this.SoundComp.StandingOnGrid == null ? (this.SoundComp.StandingOnVoxel == null ? 0.0f : (float) (-(double) MyPerGameSettings.PhysicsConvexRadius * 0.5)) : MyPerGameSettings.PhysicsConvexRadius * 0.5f;
        if ((double) this.m_gravity.LengthSquared() > 0.100000001490116 && up != Vector3.Zero && this.m_gravity.IsValid())
        {
          this.UpdateStandup(ref this.m_gravity, ref up, ref forward);
          if (jetpackComp != null)
            jetpackComp.CurrentAutoEnableDelay = 0.0f;
        }
        else
        {
          if (this.IsMagneticBootsEnabled)
          {
            Vector3 gravity = !MyFakes.ENABLE_MAGBOOTS_ON_GRID_SMOOTHING || this.SoundComp.StandingOnVoxel != null || (this.m_magBootsOnGridSmoothing == null || !this.m_magBootsOnGridSmoothing.CanUseRayCastNormal()) ? -this.Physics.CharacterProxy.SupportNormal : -this.m_magBootsOnGridSmoothing.SupportNormal;
            this.UpdateStandup(ref gravity, ref up, ref forward);
            if (!this.IsMagneticBootsActive && Sandbox.Game.Multiplayer.Sync.IsServer)
              this.m_needsUpdateBoots = true;
          }
          else if (!this.IsJumping && !this.IsFalling && (!this.JetpackRunning && this.Physics.CharacterProxy == null))
          {
            MatrixD worldMatrix2 = this.Physics.GetWorldMatrix();
            MyPhysics.HitInfo? nullable = MyPhysics.CastRay(worldMatrix2.Translation + worldMatrix2.Up, worldMatrix2.Translation + worldMatrix2.Down * 0.5, 30);
            if (nullable.HasValue)
            {
              Vector3 gravity = -nullable.Value.HkHitInfo.Normal;
              this.UpdateStandup(ref gravity, ref up, ref forward);
            }
          }
          if (((jetpackComp == null || (double) jetpackComp.CurrentAutoEnableDelay == -1.0 ? 0 : (!this.IsMagneticBootsActive ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
            jetpackComp.CurrentAutoEnableDelay += 0.01666667f;
        }
        if (this.Physics.CharacterProxy != null)
        {
          this.Physics.CharacterProxy.SetForwardAndUp(forward, up);
        }
        else
        {
          flag4 = false;
          worldMatrix1 = MatrixD.CreateWorld(worldMatrix1.Translation, forward, up);
        }
      }
      else if (this.IsDead)
      {
        if (this.Physics.HasRigidBody && this.Physics.RigidBody.IsActive)
        {
          Vector3 vector3 = this.m_gravity;
          if (Sandbox.Game.Multiplayer.Sync.IsDedicated && MyFakes.ENABLE_RAGDOLL && !MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC)
            vector3 = Vector3.Zero;
          this.Physics.RigidBody.Gravity = vector3;
        }
      }
      else if (this.IsOnLadder && this.Physics.CharacterProxy != null)
      {
        this.Physics.CharacterProxy.Gravity = Vector3.Zero;
        MatrixD matrixD = this.m_baseMatrix * this.m_ladder.WorldMatrix;
        this.Physics.CharacterProxy.SetForwardAndUp((Vector3) matrixD.Forward, (Vector3) matrixD.Up);
      }
      if (flag4)
        worldMatrix1 = this.Physics.GetWorldMatrix();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        worldMatrix1.Translation += worldMatrix1.Up * (double) this.m_characterFeetOffset;
      Vector3D vector3D = worldMatrix1.Translation - this.WorldMatrix.Translation;
      if (vector3D.LengthSquared() > 9.99999974737875E-06 || Vector3D.DistanceSquared(this.WorldMatrix.Forward, worldMatrix1.Forward) > 9.99999974737875E-06 || Vector3D.DistanceSquared(this.WorldMatrix.Up, worldMatrix1.Up) > 9.99999974737875E-06)
        this.PositionComp.SetWorldMatrix(ref worldMatrix1, flag3 || !flag4 ? (object) (MyPhysicsBody) null : (object) this.Physics);
      else
        vector3D = Vector3D.Zero;
      MyCharacterProxy characterProxy = this.Physics.CharacterProxy;
      if (characterProxy != null)
      {
        HkCharacterRigidBody characterRigidBody = characterProxy.CharacterRigidBody;
        if ((HkReferenceObject) characterRigidBody != (HkReferenceObject) null)
          characterRigidBody.InterpolatedVelocity = (Vector3) (vector3D / 0.0166666675359011);
      }
      if (!this.IsClientPredicted && !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.Physics.UpdateAccelerations();
    }

    private void UpdateStandup(ref Vector3 gravity, ref Vector3 chUp, ref Vector3 chForward)
    {
      Vector3 vector3_1 = -Vector3.Normalize(gravity);
      Vector3 vector2 = vector3_1;
      if (this.Physics != null)
      {
        Vector3 supportNormal = this.Physics.SupportNormal;
        if (this.Definition.RotationToSupport == MyEnumCharacterRotationToSupport.OneAxis)
        {
          float num1 = vector3_1.Dot(ref supportNormal);
          if (!MyUtils.IsZero(num1 - 1f, 1E-05f) && !MyUtils.IsZero(num1 + 1f, 1E-05f))
          {
            Vector3 vector3_2 = vector3_1.Cross(supportNormal);
            double num2 = (double) vector3_2.Normalize();
            vector2 = Vector3.Lerp(supportNormal, vector3_1, Math.Abs(vector3_2.Dot((Vector3) this.WorldMatrix.Forward)));
          }
        }
        else if (this.Definition.RotationToSupport == MyEnumCharacterRotationToSupport.Full)
          vector2 = this.m_currentCharacterState == HkCharacterStateType.HK_CHARACTER_ON_GROUND ? supportNormal : vector2;
      }
      float f = Vector3.Dot(chUp, vector2) / (chUp.Length() * vector2.Length());
      if (float.IsNaN(f) || float.IsNegativeInfinity(f) || float.IsPositiveInfinity(f))
        f = 1f;
      float num3 = MathHelper.Clamp(f, -1f, 1f);
      if (MyUtils.IsZero(num3 - 1f, 1E-08f))
        return;
      float num4 = !MyUtils.IsZero(num3 + 1f, 1E-08f) ? (float) Math.Acos((double) num3) : 0.1f;
      float angle = Math.Min(Math.Abs(num4), 0.04f) * (float) Math.Sign(num4);
      Vector3 axis = Vector3.Cross(chUp, vector2);
      if ((double) axis.LengthSquared() <= 0.0)
        return;
      axis = Vector3.Normalize(axis);
      chUp = Vector3.TransformNormal(chUp, Matrix.CreateFromAxisAngle(axis, angle));
      chForward = Vector3.TransformNormal(chForward, Matrix.CreateFromAxisAngle(axis, angle));
    }

    private void UpdateShake()
    {
      if (MySession.Static.LocalHumanPlayer == null || this != MySession.Static.LocalHumanPlayer.Identity.Character)
        return;
      if (this.m_currentMovementState == MyCharacterMovementEnum.Standing || this.m_currentMovementState == MyCharacterMovementEnum.Crouching || this.m_currentMovementState == MyCharacterMovementEnum.Flying)
        this.m_currentHeadAnimationCounter += 0.01666667f;
      else
        this.m_currentHeadAnimationCounter = 0.0f;
      if ((double) this.m_currentLocalHeadAnimation < 0.0)
        return;
      this.m_currentLocalHeadAnimation += 0.01666667f;
      float amount = this.m_currentLocalHeadAnimation / this.m_localHeadAnimationLength;
      if ((double) this.m_currentLocalHeadAnimation > (double) this.m_localHeadAnimationLength)
      {
        this.m_currentLocalHeadAnimation = -1f;
        amount = 1f;
      }
      if (this.m_localHeadAnimationX.HasValue)
        this.SetHeadLocalXAngle(MathHelper.Lerp(this.m_localHeadAnimationX.Value.X, this.m_localHeadAnimationX.Value.Y, amount));
      if (!this.m_localHeadAnimationY.HasValue)
        return;
      this.SetHeadLocalYAngle(MathHelper.Lerp(this.m_localHeadAnimationY.Value.X, this.m_localHeadAnimationY.Value.Y, amount));
    }

    public void UpdateZeroMovement()
    {
      if (!this.ControllerInfo.IsLocallyControlled() || this.m_moveAndRotateCalled)
        return;
      this.MoveAndRotate(Vector3.Zero, Vector2.Zero, 0.0f);
    }

    private void UpdateDying()
    {
      if (!this.m_dieAfterSimulation)
        return;
      this.m_bootsState.ValueChanged -= new Action<SyncBase>(this.OnBootsStateChanged);
      this.DieInternal();
      this.m_dieAfterSimulation = false;
    }

    internal void SetHeadLocalXAngle(float angle) => this.HeadLocalXAngle = angle;

    internal void SetHeadLocalYAngle(float angle) => this.HeadLocalYAngle = angle;

    private bool ShouldUseAnimatedHeadRotation() => false;

    public Vector3D AimedPoint
    {
      get => this.m_aimedPoint;
      set => this.m_aimedPoint = value;
    }

    private Vector3D GetAimedPointFromHead()
    {
      MatrixD headMatrix = this.GetHeadMatrix(true, true, false, false, false);
      return headMatrix.Translation + headMatrix.Forward * 10.0;
    }

    private Vector3D GetAimedPointFromCamera()
    {
      if (!this.TargetFromCamera)
        return this.GetAimedPointFromHead();
      MatrixD viewMatrix = this.GetViewMatrix();
      MatrixD result;
      MatrixD.Invert(ref viewMatrix, out result);
      Vector3D forward = result.Forward;
      forward.Normalize();
      Vector3D translation1 = result.Translation;
      Vector3D translation2 = this.GetHeadMatrix(false, false, false, false, false).Translation;
      Vector3D from = translation1 + forward * (translation2 - translation1).Dot(forward);
      Vector3D vector3D = this.WeaponPosition != null ? this.WeaponPosition.LogicalPositionWorld + forward * 25000.0 : from + forward * 25000.0;
      if (MySession.Static.ControlledEntity == this && this.CurrentWeapon != null)
      {
        float num = Math.Min(this.CurrentWeapon.MaximumShotLength, 100f);
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(from, from + forward * (double) num, 30);
        if (nullable.HasValue)
          vector3D = nullable.Value.Position;
      }
      return vector3D;
    }

    public void Rotate(Vector2 rotationIndicator, float roll)
    {
      if (!this.IsInFirstPersonView)
      {
        this.RotateHead(rotationIndicator, 0.5f);
        MyThirdPersonSpectator.Static.Rotate(rotationIndicator, roll);
      }
      else
      {
        rotationIndicator.Y = 0.0f;
        this.RotateHead(rotationIndicator, 0.2f);
      }
    }

    public void RotateStopped()
    {
    }

    public void MoveAndRotateStopped()
    {
    }

    public void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      float num = 1f;
      switch (this.ZoomMode)
      {
        case MyZoomModeEnum.Classic:
          num = MyCharacter.ROTATION_SPEED_CLASSIC;
          break;
        case MyZoomModeEnum.IronSight:
          num = MyCharacter.ROTATION_SPEED_IRONSIGHTS * (!MyInput.Static.IsJoystickLastUsed || !this.IsAimAssistSensitivityDecreased ? MyCharacter.AIM_MULT_DIS : MyCharacter.AIM_MULT_ENA);
          break;
      }
      this.LastMotionIndicator = moveIndicator;
      this.LastRotationIndicator = new Vector3(rotationIndicator, rollIndicator);
      if (moveIndicator == Vector3.Zero && rotationIndicator == Vector2.Zero && (double) rollIndicator == 0.0)
      {
        if (this.MoveIndicator == moveIndicator && rotationIndicator == this.RotationIndicator && (double) this.RollIndicator == (double) rollIndicator)
          return;
        this.MoveIndicator = Vector3.Zero;
        this.RotationIndicator = Vector2.Zero;
        this.RollIndicator = 0.0f;
        this.m_moveAndRotateStopped = true;
      }
      else
      {
        this.MoveIndicator = moveIndicator;
        this.RotationIndicator = rotationIndicator * num;
        this.RollIndicator = rollIndicator * num;
        this.m_moveAndRotateCalled = true;
        if (this != MySession.Static.LocalCharacter || !MyInput.Static.IsAnyCtrlKeyPressed() || !MyInput.Static.IsAnyAltKeyPressed())
          return;
        if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
        {
          this.RotationSpeed = Math.Min(this.RotationSpeed * 1.5f, 0.13f);
        }
        else
        {
          if (MyInput.Static.PreviousMouseScrollWheelValue() <= MyInput.Static.MouseScrollWheelValue())
            return;
          this.RotationSpeed = Math.Max(this.RotationSpeed / 1.5f, 0.01f);
        }
      }
    }

    public void CacheMove(ref Vector3 moveIndicator, ref Quaternion rotate)
    {
      if (this.m_cachedCommands == null)
        this.m_cachedCommands = new List<IMyNetworkCommand>();
      this.m_cachedCommands.Add((IMyNetworkCommand) new MyMoveNetCommand(this, ref moveIndicator, ref rotate));
    }

    public void CacheMoveDelta(ref Vector3D moveDeltaIndicator)
    {
      if (this.m_cachedCommands == null)
        this.m_cachedCommands = new List<IMyNetworkCommand>();
      this.m_cachedCommands.Add((IMyNetworkCommand) new MyDeltaNetCommand(this, ref moveDeltaIndicator));
    }

    internal void MoveAndRotateInternal(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float roll,
      Vector3 rotationCenter)
    {
      if (this.Physics == null)
        return;
      if (this.Physics.CharacterProxy == null && this.IsDead && !this.JetpackRunning)
      {
        moveIndicator = Vector3.Zero;
        rotationIndicator = Vector2.Zero;
        roll = 0.0f;
      }
      PerFrameData perFrameData;
      if (MySessionComponentReplay.Static.IsEntityBeingReplayed(this.EntityId, out perFrameData))
      {
        if (perFrameData.MovementData.HasValue)
        {
          moveIndicator = (Vector3) perFrameData.MovementData.Value.MoveVector;
          rotationIndicator = new Vector2(perFrameData.MovementData.Value.RotateVector.X, perFrameData.MovementData.Value.RotateVector.Y);
          roll = perFrameData.MovementData.Value.RotateVector.Z;
          this.MovementFlags = (MyCharacterMovementFlags) perFrameData.MovementData.Value.MovementFlags;
        }
      }
      else if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
      {
        perFrameData = new PerFrameData()
        {
          MovementData = new MovementData?(new MovementData()
          {
            MoveVector = (SerializableVector3) moveIndicator,
            RotateVector = new SerializableVector3(rotationIndicator.X, rotationIndicator.Y, roll),
            MovementFlags = (byte) this.MovementFlags
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, perFrameData);
      }
      this.WantsSprint = this.m_movementFlags != MyCharacterMovementFlags.Crouch && ((this.WantsSprint || (double) this.MoveIndicator.X * (double) this.MoveIndicator.X + (double) this.MoveIndicator.Z * (double) this.MoveIndicator.Z > 2.56000018119812) && this.ZoomMode != MyZoomModeEnum.IronSight);
      bool sprint = (double) moveIndicator.Z != 0.0 && this.WantsSprint;
      bool walk = this.WantsWalk || (double) Math.Abs(moveIndicator.X) + (double) Math.Abs(moveIndicator.Z) < 0.400000005960464;
      bool wantsJump = this.WantsJump;
      bool canMove = !this.JetpackRunning && (this.m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_IN_AIR && this.m_currentCharacterState != (HkCharacterStateType) 5 || (double) this.m_currentJumpTime > 0.0) && this.m_currentMovementState != MyCharacterMovementEnum.Died && !this.IsFalling;
      bool canRotate = (this.JetpackRunning || this.m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_IN_AIR && this.m_currentCharacterState != (HkCharacterStateType) 5 || (double) this.m_currentJumpTime > 0.0) && this.m_currentMovementState != MyCharacterMovementEnum.Died;
      bool flag = !this.m_isFalling && (double) this.m_currentJumpTime <= 0.0 && this.Physics.CharacterProxy != null && this.Physics.CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_IN_AIR;
      if (this.IsOnLadder)
        moveIndicator = this.ProceedLadderMovement(moveIndicator);
      float acceleration = 0.0f;
      double currentSpeed = (double) this.m_currentSpeed;
      if (this.JetpackRunning)
        this.JetpackComp.MoveAndRotate(ref moveIndicator, ref rotationIndicator, roll, canRotate);
      else if (((canMove ? 1 : (this.m_movementsFlagsChanged ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        if ((double) moveIndicator.LengthSquared() > 0.0)
          moveIndicator = Vector3.Normalize(moveIndicator);
        MyCharacterMovementEnum newMovementState = this.GetNewMovementState(ref moveIndicator, ref rotationIndicator, ref acceleration, sprint, walk, canMove, this.m_movementsFlagsChanged);
        this.SwitchAnimation(newMovementState);
        this.m_movementsFlagsChanged = false;
        this.SetCurrentMovementState(newMovementState);
        if (newMovementState == MyCharacterMovementEnum.Sprinting && this.StatComp != null)
          this.StatComp.ApplyModifier("Sprint");
        if (!this.IsIdle)
          this.m_currentWalkDelay = MathHelper.Clamp(this.m_currentWalkDelay - 0.01666667f, 0.0f, this.m_currentWalkDelay);
        if (canMove)
          this.m_currentSpeed = this.LimitMaxSpeed(this.m_currentSpeed + ((double) this.m_currentWalkDelay <= 0.0 ? acceleration * 0.01666667f : 0.0f), this.m_currentMovementState);
        if (this.Physics.CharacterProxy != null)
        {
          this.Physics.CharacterProxy.PosX = this.m_currentMovementState != MyCharacterMovementEnum.Sprinting ? -moveIndicator.X : 0.0f;
          this.Physics.CharacterProxy.PosY = moveIndicator.Z;
          this.Physics.CharacterProxy.Elevate = 0.0f;
        }
        if (canMove && this.m_currentMovementState != MyCharacterMovementEnum.Jump)
        {
          int num = Math.Sign(this.m_currentSpeed);
          this.m_currentSpeed += (float) ((double) -num * (double) this.m_currentDecceleration * 0.0166666675359011);
          if (Math.Sign(num) != Math.Sign(this.m_currentSpeed))
            this.m_currentSpeed = 0.0f;
        }
        if (this.Physics.CharacterProxy != null)
          this.Physics.CharacterProxy.Speed = this.m_currentMovementState != MyCharacterMovementEnum.Died ? this.m_currentSpeed : 0.0f;
        this.m_currentMovementDirection = moveIndicator;
        if (this.Physics.CharacterProxy != null && (HkReferenceObject) this.Physics.CharacterProxy.GetHitRigidBody() != (HkReferenceObject) null)
        {
          if (wantsJump && this.m_currentMovementState != MyCharacterMovementEnum.Jump)
          {
            this.PlayCharacterAnimation("Jump", MyBlendOption.Immediate, MyFrameOption.StayOnLastFrame, 0.0f, 1.3f);
            if (this.UseNewAnimationSystem)
              this.TriggerCharacterAnimationEvent("jump", true);
            if (this.StatComp != null)
            {
              this.StatComp.DoAction("Jump");
              this.StatComp.ApplyModifier("Jump");
            }
            this.m_currentJumpTime = 0.55f;
            this.SetCurrentMovementState(MyCharacterMovementEnum.Jump);
            this.m_canJump = false;
            this.m_frictionBeforeJump = this.Physics.CharacterProxy.GetHitRigidBody().Friction;
            this.Physics.CharacterProxy.Jump = true;
          }
          if ((double) this.m_currentJumpTime > 0.0)
          {
            this.m_currentJumpTime -= 0.01666667f;
            this.Physics.CharacterProxy.GetHitRigidBody().Friction = 0.0f;
          }
          if ((double) this.m_currentJumpTime <= 0.0 && this.m_currentMovementState == MyCharacterMovementEnum.Jump)
          {
            this.Physics.CharacterProxy.GetHitRigidBody().Friction = this.m_frictionBeforeJump;
            if (this.m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_ON_GROUND)
              this.StartFalling();
            else if (this.Physics.CharacterProxy != null && (this.Physics.CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_IN_AIR || this.Physics.CharacterProxy.GetState() == (HkCharacterStateType) 5))
              this.StartFalling();
            else if (!this.IsFalling)
            {
              MyCharacterMovementEnum state;
              if ((double) moveIndicator.X != 0.0 || (double) moveIndicator.Z != 0.0)
              {
                if (!this.WantsCrouch)
                {
                  if ((double) moveIndicator.Z < 0.0)
                  {
                    if (sprint)
                    {
                      state = MyCharacterMovementEnum.Sprinting;
                      this.PlayCharacterAnimation("Sprint", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
                    }
                    else
                    {
                      state = MyCharacterMovementEnum.Walking;
                      this.PlayCharacterAnimation("Walk", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.5f);
                    }
                  }
                  else
                  {
                    state = MyCharacterMovementEnum.BackWalking;
                    this.PlayCharacterAnimation("WalkBack", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.5f);
                  }
                }
                else if ((double) moveIndicator.Z < 0.0)
                {
                  state = MyCharacterMovementEnum.CrouchWalking;
                  this.PlayCharacterAnimation("CrouchWalk", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
                }
                else
                {
                  state = MyCharacterMovementEnum.CrouchBackWalking;
                  this.PlayCharacterAnimation("CrouchWalkBack", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
                }
              }
              else
              {
                state = MyCharacterMovementEnum.Standing;
                this.PlayCharacterAnimation("Idle", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
              }
              if (!this.m_canJump)
                this.SoundComp.PlayFallSound();
              this.m_canJump = true;
              this.SetCurrentMovementState(state);
            }
            this.m_currentJumpTime = 0.0f;
          }
        }
      }
      else if (this.Physics.CharacterProxy != null)
        this.Physics.CharacterProxy.Elevate = 0.0f;
      this.UpdateHeadOffset();
      if (!this.JetpackRunning && !this.IsOnLadder)
      {
        if ((double) rotationIndicator.Y != 0.0 && (canRotate || this.m_isFalling || (double) this.m_currentJumpTime > 0.0))
        {
          if (this.Physics.CharacterProxy != null)
          {
            MatrixD matrixD = MatrixD.CreateRotationY(-(double) rotationIndicator.Y * (double) this.RotationSpeed * 0.0199999995529652) * MatrixD.CreateWorld((Vector3D) this.Physics.CharacterProxy.Position, this.Physics.CharacterProxy.Forward, this.Physics.CharacterProxy.Up);
            this.Physics.CharacterProxy.SetForwardAndUp((Vector3) matrixD.Forward, (Vector3) matrixD.Up);
          }
          else
          {
            MatrixD worldMatrix = MatrixD.CreateRotationY(-(double) rotationIndicator.Y * (double) this.RotationSpeed * 0.0199999995529652) * this.WorldMatrix;
            worldMatrix.Translation = this.WorldMatrix.Translation;
            this.PositionComp.SetWorldMatrix(ref worldMatrix);
          }
        }
        if ((double) rotationIndicator.X != 0.0 && (this.m_currentMovementState == MyCharacterMovementEnum.Died && !this.m_isInFirstPerson || this.m_currentMovementState != MyCharacterMovementEnum.Died))
        {
          float num = rotationIndicator.X * this.RotationSpeed;
          this.SetHeadLocalXAngle(this.m_headLocalXAngle - num);
          if (this.CurrentWeapon is MyAutomaticRifleGun currentWeapon)
            currentWeapon.ChangeRecoilVertAngles(-num);
          int index = this.IsInFirstPersonView ? this.m_headBoneIndex : this.m_camera3rdBoneIndex;
          if (index != -1)
          {
            this.m_bobQueue.Clear();
            this.m_bobQueue.Enqueue(this.BoneAbsoluteTransforms[index].Translation);
          }
        }
      }
      if (this.Physics.CharacterProxy != null && (double) this.Physics.CharacterProxy.LinearVelocity.LengthSquared() > 0.100000001490116)
        this.m_shapeContactPoints.Clear();
      this.WantsJump = false;
      this.WantsFlyUp = false;
      this.WantsFlyDown = false;
    }

    private void RotateHead(Vector2 rotationIndicator, float sensitivity)
    {
      if ((double) rotationIndicator.X != 0.0)
        this.SetHeadLocalXAngle(this.m_headLocalXAngle - rotationIndicator.X * sensitivity);
      if ((double) rotationIndicator.Y == 0.0)
        return;
      this.SetHeadLocalYAngle(this.m_headLocalYAngle + -rotationIndicator.Y * sensitivity);
    }

    public bool IsIdle => this.m_currentMovementState == MyCharacterMovementEnum.Standing || this.m_currentMovementState == MyCharacterMovementEnum.Crouching;

    public bool CanPlaceCharacter(
      ref MatrixD worldMatrix,
      bool useCharacterCenter = false,
      bool checkCharacters = false,
      MyEntity ignoreEntity = null)
    {
      Vector3D translation = worldMatrix.Translation;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in worldMatrix);
      if (this.Physics == null || this.Physics.CharacterProxy == null && (HkReferenceObject) this.Physics.RigidBody == (HkReferenceObject) null)
        return true;
      this.m_penetrationList.Clear();
      if (!useCharacterCenter)
      {
        Vector3D vector3D = Vector3D.TransformNormal(this.Physics.Center, worldMatrix);
        translation += vector3D;
      }
      this.m_penetrationList.Clear();
      MyPhysics.GetPenetrationsShape(this.Physics.CharacterProxy != null ? this.Physics.CharacterProxy.GetCollisionShape() : this.Physics.RigidBody.GetShape(), ref translation, ref fromRotationMatrix, this.m_penetrationList, 18);
      bool flag = false;
      foreach (HkBodyCollision penetration in this.m_penetrationList)
      {
        VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
        if (ignoreEntity != collisionEntity)
        {
          if (collisionEntity != null)
          {
            if (collisionEntity.Physics == null)
              MyLog.Default.WriteLine("CanPlaceCharacter found Entity with no physics: " + (object) collisionEntity);
            else if (!collisionEntity.Physics.IsPhantom)
            {
              flag = true;
              break;
            }
          }
          else if (checkCharacters)
          {
            flag = true;
            break;
          }
        }
      }
      if (MySession.Static.VoxelMaps == null)
        return true;
      if (!flag)
      {
        BoundingSphereD sphere = new BoundingSphereD(worldMatrix.Translation, 0.75);
        flag = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere) != null;
      }
      return !flag;
    }

    public MyCharacterMovementEnum GetCurrentMovementState() => this.m_currentMovementState;

    public MyCharacterMovementEnum GetPreviousMovementState() => this.m_previousMovementState;

    public MyCharacterMovementEnum GetNetworkMovementState() => this.m_previousNetworkMovementState;

    public void SetPreviousMovementState(MyCharacterMovementEnum previousMovementState) => this.m_previousMovementState = previousMovementState;

    private void SetStandingLocalAABB()
    {
      float num = this.Definition.CharacterCollisionWidth / 2f;
      this.PositionComp.LocalAABB = new BoundingBox(-new Vector3(num, 0.0f, num), new Vector3(num, this.Definition.CharacterCollisionHeight, num));
    }

    private void SetCrouchingLocalAABB()
    {
      float num = this.Definition.CharacterCollisionWidth / 2f;
      this.PositionComp.LocalAABB = new BoundingBox(-new Vector3(num, 0.0f, num), new Vector3(num, this.Definition.CharacterCollisionHeight / 2f, num));
    }

    internal void SetCurrentMovementState(MyCharacterMovementEnum state)
    {
      if (this.m_currentMovementState == state)
        return;
      if (!this.CanSprint && state == MyCharacterMovementEnum.Sprinting)
        state = MyCharacterMovementEnum.Running;
      this.m_previousMovementState = this.m_currentMovementState;
      this.m_currentMovementState = state;
      this.UpdateCrouchState();
      if (this.OnMovementStateChanged != null)
        this.OnMovementStateChanged(this.m_previousMovementState, this.m_currentMovementState);
      if (this.MovementStateChanged == null)
        return;
      this.MovementStateChanged((IMyCharacter) this, this.m_previousMovementState, this.m_currentMovementState);
    }

    private void UpdateCrouchState()
    {
      bool isCrouching = this.IsCrouching;
      bool flag = this.m_previousMovementState.GetMode() == (ushort) 2;
      MyCharacterProxy characterProxy = this.Physics.CharacterProxy;
      if (characterProxy != null && characterProxy.IsCrouching != isCrouching)
        characterProxy.SetShapeForCrouch(this.Physics.HavokWorld, isCrouching);
      if (isCrouching == flag)
        return;
      if (isCrouching)
        this.SetCrouchingLocalAABB();
      else
        this.SetStandingLocalAABB();
      if (characterProxy != null)
        return;
      this.UpdateCharacterPhysics(true);
    }

    private float GetMovementAcceleration(MyCharacterMovementEnum movement)
    {
      if ((uint) movement <= 128U)
      {
        if ((uint) movement <= 34U)
        {
          switch (movement)
          {
            case MyCharacterMovementEnum.Standing:
            case MyCharacterMovementEnum.Crouching:
              return MyPerGameSettings.CharacterMovement.WalkAcceleration;
            case MyCharacterMovementEnum.Sitting:
            case MyCharacterMovementEnum.Flying:
            case MyCharacterMovementEnum.Falling:
              goto label_29;
            case MyCharacterMovementEnum.Jump:
              return 0.0f;
            default:
              if ((uint) movement <= 18U)
              {
                if (movement == MyCharacterMovementEnum.Walking || movement == MyCharacterMovementEnum.CrouchWalking)
                  break;
                goto label_29;
              }
              else
              {
                if (movement == MyCharacterMovementEnum.BackWalking || movement == MyCharacterMovementEnum.CrouchBackWalking)
                  break;
                goto label_29;
              }
          }
        }
        else if ((uint) movement <= 80U)
        {
          if (movement == MyCharacterMovementEnum.WalkStrafingLeft || movement == MyCharacterMovementEnum.CrouchStrafingLeft || movement == MyCharacterMovementEnum.WalkingLeftFront)
            goto label_27;
          else
            goto label_29;
        }
        else if ((uint) movement <= 96U)
        {
          if (movement != MyCharacterMovementEnum.CrouchWalkingLeftFront && movement != MyCharacterMovementEnum.WalkingLeftBack)
            goto label_29;
        }
        else if (movement != MyCharacterMovementEnum.CrouchWalkingLeftBack && movement != MyCharacterMovementEnum.WalkStrafingRight)
          goto label_29;
      }
      else
      {
        if ((uint) movement <= 1056U)
        {
          if ((uint) movement <= 146U)
          {
            if (movement != MyCharacterMovementEnum.CrouchStrafingRight && movement != MyCharacterMovementEnum.WalkingRightFront && movement != MyCharacterMovementEnum.CrouchWalkingRightFront)
              goto label_29;
          }
          else if ((uint) movement <= 162U)
          {
            if (movement == MyCharacterMovementEnum.WalkingRightBack || movement == MyCharacterMovementEnum.CrouchWalkingRightBack)
              goto label_24;
            else
              goto label_29;
          }
          else if (movement == MyCharacterMovementEnum.Running || movement == MyCharacterMovementEnum.Backrunning)
            goto label_24;
          else
            goto label_29;
        }
        else if ((uint) movement <= 1120U)
        {
          if (movement == MyCharacterMovementEnum.RunStrafingLeft || movement == MyCharacterMovementEnum.RunningLeftFront || movement == MyCharacterMovementEnum.RunningLeftBack)
            goto label_27;
          else
            goto label_29;
        }
        else if ((uint) movement <= 1168U)
        {
          if (movement != MyCharacterMovementEnum.RunStrafingRight && movement != MyCharacterMovementEnum.RunningRightFront)
            goto label_29;
        }
        else if (movement != MyCharacterMovementEnum.RunningRightBack)
        {
          if (movement == MyCharacterMovementEnum.Sprinting)
            return MyPerGameSettings.CharacterMovement.SprintAcceleration;
          goto label_29;
        }
        else
          goto label_24;
        return MyPerGameSettings.CharacterMovement.WalkAcceleration;
      }
label_24:
      return MyPerGameSettings.CharacterMovement.WalkAcceleration;
label_27:
      return MyPerGameSettings.CharacterMovement.WalkAcceleration;
label_29:
      return 0.0f;
    }

    internal float HeadMovementXOffset => this.m_headMovementXOffset;

    internal float HeadMovementYOffset => this.m_headMovementYOffset;

    private void SlowDownX()
    {
      if ((double) Math.Abs(this.m_headMovementXOffset) <= 0.0)
        return;
      this.m_headMovementXOffset += (float) Math.Sign(-this.m_headMovementXOffset) * this.m_headMovementStep;
      if ((double) Math.Abs(this.m_headMovementXOffset) >= (double) this.m_headMovementStep)
        return;
      this.m_headMovementXOffset = 0.0f;
    }

    private void SlowDownY()
    {
      if ((double) Math.Abs(this.m_headMovementYOffset) <= 0.0)
        return;
      this.m_headMovementYOffset += (float) Math.Sign(-this.m_headMovementYOffset) * this.m_headMovementStep;
      if ((double) Math.Abs(this.m_headMovementYOffset) >= (double) this.m_headMovementStep)
        return;
      this.m_headMovementYOffset = 0.0f;
    }

    private void AccelerateX(float sign)
    {
      this.m_headMovementXOffset += sign * this.m_headMovementStep;
      if ((double) sign > 0.0)
      {
        if ((double) this.m_headMovementXOffset <= (double) this.m_maxHeadMovementOffset)
          return;
        this.m_headMovementXOffset = this.m_maxHeadMovementOffset;
      }
      else
      {
        if ((double) this.m_headMovementXOffset >= -(double) this.m_maxHeadMovementOffset)
          return;
        this.m_headMovementXOffset = -this.m_maxHeadMovementOffset;
      }
    }

    private void AccelerateY(float sign)
    {
      this.m_headMovementYOffset += sign * this.m_headMovementStep;
      if ((double) sign > 0.0)
      {
        if ((double) this.m_headMovementYOffset <= (double) this.m_maxHeadMovementOffset)
          return;
        this.m_headMovementYOffset = this.m_maxHeadMovementOffset;
      }
      else
      {
        if ((double) this.m_headMovementYOffset >= -(double) this.m_maxHeadMovementOffset)
          return;
        this.m_headMovementYOffset = -this.m_maxHeadMovementOffset;
      }
    }

    private void UpdateHeadOffset()
    {
      MyCharacterMovementEnum currentMovementState = this.m_currentMovementState;
      if ((uint) currentMovementState <= 146U)
      {
        if ((uint) currentMovementState <= 66U)
        {
          if ((uint) currentMovementState <= 18U)
          {
            switch (currentMovementState)
            {
              case MyCharacterMovementEnum.Standing:
              case MyCharacterMovementEnum.Crouching:
              case MyCharacterMovementEnum.Falling:
              case MyCharacterMovementEnum.Jump:
                break;
              case MyCharacterMovementEnum.Sitting:
                return;
              case MyCharacterMovementEnum.Flying:
                return;
              case MyCharacterMovementEnum.Walking:
              case MyCharacterMovementEnum.CrouchWalking:
                goto label_40;
              default:
                return;
            }
          }
          else if ((uint) currentMovementState <= 34U)
          {
            if (currentMovementState != MyCharacterMovementEnum.BackWalking && currentMovementState != MyCharacterMovementEnum.CrouchBackWalking)
              return;
            goto label_41;
          }
          else
          {
            if (currentMovementState != MyCharacterMovementEnum.WalkStrafingLeft && currentMovementState != MyCharacterMovementEnum.CrouchStrafingLeft)
              return;
            goto label_42;
          }
        }
        else
        {
          if ((uint) currentMovementState <= 98U)
          {
            if ((uint) currentMovementState <= 82U)
            {
              if (currentMovementState == MyCharacterMovementEnum.WalkingLeftFront)
                ;
              return;
            }
            if (currentMovementState == MyCharacterMovementEnum.WalkingLeftBack)
              ;
            return;
          }
          if ((uint) currentMovementState <= 130U)
          {
            if (currentMovementState != MyCharacterMovementEnum.WalkStrafingRight && currentMovementState != MyCharacterMovementEnum.CrouchStrafingRight)
              return;
            goto label_43;
          }
          else
          {
            if (currentMovementState == MyCharacterMovementEnum.WalkingRightFront)
              ;
            return;
          }
        }
      }
      else if ((uint) currentMovementState <= 1120U)
      {
        if ((uint) currentMovementState <= 1040U)
        {
          if (currentMovementState == MyCharacterMovementEnum.WalkingRightBack || currentMovementState == MyCharacterMovementEnum.CrouchWalkingRightBack || currentMovementState != MyCharacterMovementEnum.Running)
            return;
          goto label_40;
        }
        else if ((uint) currentMovementState <= 1088U)
        {
          if (currentMovementState != MyCharacterMovementEnum.Backrunning)
          {
            if (currentMovementState != MyCharacterMovementEnum.RunStrafingLeft)
              return;
            goto label_42;
          }
          else
            goto label_41;
        }
        else
        {
          if (currentMovementState == MyCharacterMovementEnum.RunningLeftFront)
            ;
          return;
        }
      }
      else if ((uint) currentMovementState <= 2064U)
      {
        if ((uint) currentMovementState <= 1168U)
        {
          if (currentMovementState != MyCharacterMovementEnum.RunStrafingRight)
            return;
          goto label_43;
        }
        else
        {
          if (currentMovementState == MyCharacterMovementEnum.RunningRightBack || currentMovementState != MyCharacterMovementEnum.Sprinting)
            return;
          goto label_40;
        }
      }
      else if ((uint) currentMovementState <= 4098U)
      {
        if (currentMovementState != MyCharacterMovementEnum.RotatingLeft && currentMovementState != MyCharacterMovementEnum.CrouchRotatingLeft)
          return;
      }
      else if (currentMovementState != MyCharacterMovementEnum.RotatingRight && currentMovementState != MyCharacterMovementEnum.CrouchRotatingRight)
        return;
      this.SlowDownX();
      this.SlowDownY();
      return;
label_40:
      this.AccelerateX(-1f);
      this.SlowDownY();
      return;
label_41:
      this.AccelerateX(1f);
      this.SlowDownY();
      return;
label_42:
      this.SlowDownX();
      this.AccelerateY(1f);
      return;
label_43:
      this.SlowDownX();
      this.AccelerateY(-1f);
    }

    public static bool IsWalkingState(MyCharacterMovementEnum state)
    {
      if ((uint) state <= 130U)
      {
        if ((uint) state <= 66U)
        {
          if ((uint) state <= 32U)
          {
            if (state != MyCharacterMovementEnum.Walking && state != MyCharacterMovementEnum.CrouchWalking && state != MyCharacterMovementEnum.BackWalking)
              goto label_18;
          }
          else if (state != MyCharacterMovementEnum.CrouchBackWalking && state != MyCharacterMovementEnum.WalkStrafingLeft && state != MyCharacterMovementEnum.CrouchStrafingLeft)
            goto label_18;
        }
        else if ((uint) state <= 96U)
        {
          if (state != MyCharacterMovementEnum.WalkingLeftFront && state != MyCharacterMovementEnum.CrouchWalkingLeftFront && state != MyCharacterMovementEnum.WalkingLeftBack)
            goto label_18;
        }
        else if (state != MyCharacterMovementEnum.CrouchWalkingLeftBack && state != MyCharacterMovementEnum.WalkStrafingRight && state != MyCharacterMovementEnum.CrouchStrafingRight)
          goto label_18;
      }
      else if ((uint) state <= 1056U)
      {
        if ((uint) state <= 160U)
        {
          if (state != MyCharacterMovementEnum.WalkingRightFront && state != MyCharacterMovementEnum.CrouchWalkingRightFront && state != MyCharacterMovementEnum.WalkingRightBack)
            goto label_18;
        }
        else if (state != MyCharacterMovementEnum.CrouchWalkingRightBack && state != MyCharacterMovementEnum.Running && state != MyCharacterMovementEnum.Backrunning)
          goto label_18;
      }
      else if ((uint) state <= 1120U)
      {
        if (state != MyCharacterMovementEnum.RunStrafingLeft && state != MyCharacterMovementEnum.RunningLeftFront && state != MyCharacterMovementEnum.RunningLeftBack)
          goto label_18;
      }
      else if ((uint) state <= 1168U)
      {
        if (state != MyCharacterMovementEnum.RunStrafingRight && state != MyCharacterMovementEnum.RunningRightFront)
          goto label_18;
      }
      else if (state != MyCharacterMovementEnum.RunningRightBack && state != MyCharacterMovementEnum.Sprinting)
        goto label_18;
      return true;
label_18:
      return false;
    }

    public static bool IsRunningState(MyCharacterMovementEnum state)
    {
      if ((uint) state <= 1104U)
      {
        if ((uint) state <= 1056U)
        {
          if (state != MyCharacterMovementEnum.Running && state != MyCharacterMovementEnum.Backrunning)
            goto label_8;
        }
        else if (state != MyCharacterMovementEnum.RunStrafingLeft && state != MyCharacterMovementEnum.RunningLeftFront)
          goto label_8;
      }
      else if ((uint) state <= 1152U)
      {
        if (state != MyCharacterMovementEnum.RunningLeftBack && state != MyCharacterMovementEnum.RunStrafingRight)
          goto label_8;
      }
      else if (state != MyCharacterMovementEnum.RunningRightFront && state != MyCharacterMovementEnum.RunningRightBack && state != MyCharacterMovementEnum.Sprinting)
        goto label_8;
      return true;
label_8:
      return false;
    }

    public float GetMovementMultiplierForMovementState(MyCharacterMovementEnum movementState)
    {
      if (this.ZoomMode == MyZoomModeEnum.IronSight && this.m_handItemDefinition != null)
        return this.m_handItemDefinition.AimingSpeedMultiplier * MySession.Static.Settings.CharacterSpeedMultiplier;
      if ((uint) movementState <= 130U)
      {
        if ((uint) movementState <= 64U)
        {
          if ((uint) movementState <= 18U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.Flying:
                break;
              case MyCharacterMovementEnum.Walking:
                MyHandItemDefinition handItemDefinition1 = this.m_handItemDefinition;
                return (handItemDefinition1 != null ? handItemDefinition1.WalkSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
              case MyCharacterMovementEnum.CrouchWalking:
                MyHandItemDefinition handItemDefinition2 = this.m_handItemDefinition;
                return (handItemDefinition2 != null ? handItemDefinition2.CrouchWalkSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
              default:
                goto label_31;
            }
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.BackWalking:
                goto label_23;
              case MyCharacterMovementEnum.CrouchBackWalking:
                goto label_29;
              case MyCharacterMovementEnum.WalkStrafingLeft:
                goto label_24;
              default:
                goto label_31;
            }
          }
        }
        else if ((uint) movementState <= 82U)
        {
          switch (movementState)
          {
            case MyCharacterMovementEnum.CrouchStrafingLeft:
            case MyCharacterMovementEnum.CrouchWalkingLeftFront:
              goto label_28;
            case MyCharacterMovementEnum.WalkingLeftFront:
              goto label_24;
            default:
              goto label_31;
          }
        }
        else if ((uint) movementState <= 98U)
        {
          switch (movementState)
          {
            case MyCharacterMovementEnum.WalkingLeftBack:
              goto label_23;
            case MyCharacterMovementEnum.CrouchWalkingLeftBack:
              goto label_29;
            default:
              goto label_31;
          }
        }
        else
        {
          switch (movementState)
          {
            case MyCharacterMovementEnum.WalkStrafingRight:
              goto label_24;
            case MyCharacterMovementEnum.CrouchStrafingRight:
              goto label_28;
            default:
              goto label_31;
          }
        }
      }
      else
      {
        if ((uint) movementState <= 1056U)
        {
          if ((uint) movementState <= 160U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.WalkingRightFront:
                goto label_24;
              case MyCharacterMovementEnum.CrouchWalkingRightFront:
                goto label_28;
              case MyCharacterMovementEnum.WalkingRightBack:
                goto label_23;
              default:
                goto label_31;
            }
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.CrouchWalkingRightBack:
                goto label_29;
              case MyCharacterMovementEnum.Running:
                goto label_21;
              case MyCharacterMovementEnum.Backrunning:
                break;
              default:
                goto label_31;
            }
          }
        }
        else
        {
          if ((uint) movementState <= 1120U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.RunStrafingLeft:
              case MyCharacterMovementEnum.RunningLeftFront:
                break;
              case MyCharacterMovementEnum.RunningLeftBack:
                goto label_25;
              default:
                goto label_31;
            }
          }
          else if ((uint) movementState <= 1168U)
          {
            if (movementState != MyCharacterMovementEnum.RunStrafingRight && movementState != MyCharacterMovementEnum.RunningRightFront)
              goto label_31;
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.RunningRightBack:
                goto label_25;
              case MyCharacterMovementEnum.Sprinting:
                MyHandItemDefinition handItemDefinition3 = this.m_handItemDefinition;
                return (handItemDefinition3 != null ? handItemDefinition3.SprintSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
              default:
                goto label_31;
            }
          }
          MyHandItemDefinition handItemDefinition4 = this.m_handItemDefinition;
          return (handItemDefinition4 != null ? handItemDefinition4.RunStrafingSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
        }
label_25:
        MyHandItemDefinition handItemDefinition5 = this.m_handItemDefinition;
        return (handItemDefinition5 != null ? handItemDefinition5.BackrunSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
      }
label_21:
      MyHandItemDefinition handItemDefinition6 = this.m_handItemDefinition;
      return (handItemDefinition6 != null ? handItemDefinition6.RunSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
label_23:
      MyHandItemDefinition handItemDefinition7 = this.m_handItemDefinition;
      return (handItemDefinition7 != null ? handItemDefinition7.BackwalkSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
label_24:
      MyHandItemDefinition handItemDefinition8 = this.m_handItemDefinition;
      return (handItemDefinition8 != null ? handItemDefinition8.WalkStrafingSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
label_28:
      MyHandItemDefinition handItemDefinition9 = this.m_handItemDefinition;
      return (handItemDefinition9 != null ? handItemDefinition9.CrouchStrafingSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
label_29:
      MyHandItemDefinition handItemDefinition10 = this.m_handItemDefinition;
      return (handItemDefinition10 != null ? handItemDefinition10.CrouchBackwalkSpeedMultiplier : 1f) * MySession.Static.Settings.CharacterSpeedMultiplier;
label_31:
      return 1f;
    }

    internal void SwitchAnimation(MyCharacterMovementEnum movementState, bool checkState = true)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated && MyPerGameSettings.DisableAnimationsOnDS || checkState && this.m_currentMovementState == movementState)
        return;
      if (!this.CanSprint && movementState == MyCharacterMovementEnum.Sprinting)
        movementState = MyCharacterMovementEnum.Running;
      if (MyCharacter.IsWalkingState(this.m_currentMovementState) != MyCharacter.IsWalkingState(movementState))
        this.m_currentHandItemWalkingBlend = 0.0f;
      if ((uint) movementState <= 146U)
      {
        if ((uint) movementState <= 66U)
        {
          if ((uint) movementState <= 18U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.Standing:
                this.PlayCharacterAnimation("Idle", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
                break;
              case MyCharacterMovementEnum.Crouching:
                this.PlayCharacterAnimation("CrouchIdle", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.1f), this.GetMovementMultiplierForMovementState(movementState));
                break;
              case MyCharacterMovementEnum.Flying:
                this.PlayCharacterAnimation("Jetpack", this.AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.0f), this.GetMovementMultiplierForMovementState(movementState));
                break;
              case MyCharacterMovementEnum.Falling:
                this.PlayCharacterAnimation("FreeFall", this.AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
                break;
              case MyCharacterMovementEnum.Jump:
                this.PlayCharacterAnimation("Jump", this.AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Default, this.AdjustSafeAnimationBlend(0.0f), 1.3f);
                break;
              case MyCharacterMovementEnum.Died:
                this.PlayCharacterAnimation("Died", this.AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Default, this.AdjustSafeAnimationBlend(0.5f), this.GetMovementMultiplierForMovementState(movementState));
                break;
              case MyCharacterMovementEnum.Walking:
                this.PlayCharacterAnimation("Walk", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.1f), this.GetMovementMultiplierForMovementState(movementState));
                break;
              case MyCharacterMovementEnum.CrouchWalking:
                this.PlayCharacterAnimation("CrouchWalk", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
                break;
            }
          }
          else if ((uint) movementState <= 34U)
          {
            if (movementState != MyCharacterMovementEnum.BackWalking)
            {
              if (movementState != MyCharacterMovementEnum.CrouchBackWalking)
                return;
              this.PlayCharacterAnimation("CrouchWalkBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
            }
            else
              this.PlayCharacterAnimation("WalkBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else if (movementState != MyCharacterMovementEnum.WalkStrafingLeft)
          {
            if (movementState != MyCharacterMovementEnum.CrouchStrafingLeft)
              return;
            this.PlayCharacterAnimation("CrouchStrafeLeft", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else
            this.PlayCharacterAnimation("StrafeLeft", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else if ((uint) movementState <= 98U)
        {
          if ((uint) movementState <= 82U)
          {
            if (movementState != MyCharacterMovementEnum.WalkingLeftFront)
            {
              if (movementState != MyCharacterMovementEnum.CrouchWalkingLeftFront)
                return;
              this.PlayCharacterAnimation("CrouchWalkLeftFront", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
            }
            else
              this.PlayCharacterAnimation("WalkLeftFront", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else if (movementState != MyCharacterMovementEnum.WalkingLeftBack)
          {
            if (movementState != MyCharacterMovementEnum.CrouchWalkingLeftBack)
              return;
            this.PlayCharacterAnimation("CrouchWalkLeftBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else
            this.PlayCharacterAnimation("WalkLeftBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else if ((uint) movementState <= 130U)
        {
          if (movementState != MyCharacterMovementEnum.WalkStrafingRight)
          {
            if (movementState != MyCharacterMovementEnum.CrouchStrafingRight)
              return;
            this.PlayCharacterAnimation("CrouchStrafeRight", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else
            this.PlayCharacterAnimation("StrafeRight", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else if (movementState != MyCharacterMovementEnum.WalkingRightFront)
        {
          if (movementState != MyCharacterMovementEnum.CrouchWalkingRightFront)
            return;
          this.PlayCharacterAnimation("CrouchWalkRightFront", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else
          this.PlayCharacterAnimation("WalkRightFront", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
      }
      else if ((uint) movementState <= 1120U)
      {
        if ((uint) movementState <= 1040U)
        {
          switch (movementState)
          {
            case MyCharacterMovementEnum.WalkingRightBack:
              this.PlayCharacterAnimation("WalkRightBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
              break;
            case MyCharacterMovementEnum.CrouchWalkingRightBack:
              this.PlayCharacterAnimation("CrouchWalkRightBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
              break;
            case MyCharacterMovementEnum.Running:
              this.PlayCharacterAnimation("Run", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
              break;
          }
        }
        else if ((uint) movementState <= 1088U)
        {
          if (movementState != MyCharacterMovementEnum.Backrunning)
          {
            if (movementState != MyCharacterMovementEnum.RunStrafingLeft)
              return;
            this.PlayCharacterAnimation("RunLeft", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else
            this.PlayCharacterAnimation("RunBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else if (movementState != MyCharacterMovementEnum.RunningLeftFront)
        {
          if (movementState != MyCharacterMovementEnum.RunningLeftBack)
            return;
          this.PlayCharacterAnimation("RunLeftBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else
          this.PlayCharacterAnimation("RunLeftFront", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
      }
      else if ((uint) movementState <= 2064U)
      {
        if ((uint) movementState <= 1168U)
        {
          if (movementState != MyCharacterMovementEnum.RunStrafingRight)
          {
            if (movementState != MyCharacterMovementEnum.RunningRightFront)
              return;
            this.PlayCharacterAnimation("RunRightFront", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
          }
          else
            this.PlayCharacterAnimation("RunRight", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else if (movementState != MyCharacterMovementEnum.RunningRightBack)
        {
          if (movementState != MyCharacterMovementEnum.Sprinting)
            return;
          this.PlayCharacterAnimation("Sprint", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.1f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else
          this.PlayCharacterAnimation("RunRightBack", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
      }
      else if ((uint) movementState <= 4098U)
      {
        if (movementState != MyCharacterMovementEnum.RotatingLeft)
        {
          if (movementState != MyCharacterMovementEnum.CrouchRotatingLeft)
            return;
          this.PlayCharacterAnimation("CrouchLeftTurn", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
        }
        else
          this.PlayCharacterAnimation("StandLeftTurn", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
      }
      else if (movementState != MyCharacterMovementEnum.RotatingRight)
      {
        if (movementState != MyCharacterMovementEnum.CrouchRotatingRight)
          return;
        this.PlayCharacterAnimation("CrouchRightTurn", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
      }
      else
        this.PlayCharacterAnimation("StandRightTurn", this.AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, this.AdjustSafeAnimationBlend(0.2f), this.GetMovementMultiplierForMovementState(movementState));
    }

    private MyCharacterMovementEnum GetNewMovementState(
      ref Vector3 moveIndicator,
      ref Vector2 rotationIndicator,
      ref float acceleration,
      bool sprint,
      bool walk,
      bool canMove,
      bool movementFlagsChanged)
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        return MyCharacterMovementEnum.Died;
      MyCharacterMovementEnum movement = this.m_currentMovementState;
      if (this.Definition.UseOnlyWalking)
        walk = true;
      if ((double) this.m_currentJumpTime > 0.0)
        return MyCharacterMovementEnum.Jump;
      if (this.JetpackRunning)
        return MyCharacterMovementEnum.Flying;
      bool flag1 = true;
      bool flag2 = true;
      bool flag3 = true;
      bool flag4 = true;
      bool continuous1 = false;
      bool continuous2 = false;
      bool continuous3 = false;
      MyCharacterMovementEnum currentMovementState1 = this.m_currentMovementState;
      if ((uint) currentMovementState1 <= 263U)
      {
        if ((uint) currentMovementState1 <= 80U)
        {
          if ((uint) currentMovementState1 <= 16U)
          {
            switch (currentMovementState1)
            {
              case MyCharacterMovementEnum.Ladder:
                goto label_26;
              case MyCharacterMovementEnum.Walking:
                break;
              default:
                goto label_27;
            }
          }
          else if (currentMovementState1 != MyCharacterMovementEnum.WalkStrafingLeft && currentMovementState1 != MyCharacterMovementEnum.WalkingLeftFront)
            goto label_27;
        }
        else if ((uint) currentMovementState1 <= 128U)
        {
          if (currentMovementState1 != MyCharacterMovementEnum.WalkingLeftBack && currentMovementState1 != MyCharacterMovementEnum.WalkStrafingRight)
            goto label_27;
        }
        else
        {
          switch (currentMovementState1)
          {
            case MyCharacterMovementEnum.WalkingRightFront:
            case MyCharacterMovementEnum.WalkingRightBack:
              break;
            case MyCharacterMovementEnum.LadderUp:
              goto label_26;
            default:
              goto label_27;
          }
        }
        continuous1 = true;
        goto label_27;
      }
      else
      {
        if ((uint) currentMovementState1 <= 1120U)
        {
          if ((uint) currentMovementState1 <= 1040U)
          {
            switch (currentMovementState1)
            {
              case MyCharacterMovementEnum.LadderDown:
                goto label_26;
              case MyCharacterMovementEnum.Running:
                break;
              default:
                goto label_27;
            }
          }
          else if (currentMovementState1 != MyCharacterMovementEnum.RunStrafingLeft && currentMovementState1 != MyCharacterMovementEnum.RunningLeftFront && currentMovementState1 != MyCharacterMovementEnum.RunningLeftBack)
            goto label_27;
        }
        else if ((uint) currentMovementState1 <= 1168U)
        {
          if (currentMovementState1 != MyCharacterMovementEnum.RunStrafingRight && currentMovementState1 != MyCharacterMovementEnum.RunningRightFront)
            goto label_27;
        }
        else
        {
          switch (currentMovementState1)
          {
            case MyCharacterMovementEnum.RunningRightBack:
              break;
            case MyCharacterMovementEnum.Sprinting:
              continuous3 = true;
              goto label_27;
            case MyCharacterMovementEnum.LadderOut:
              goto label_26;
            default:
              goto label_27;
          }
        }
        continuous2 = true;
        goto label_27;
      }
label_26:
      return movement;
label_27:
      if (this.StatComp != null)
      {
        MyTuple<ushort, MyStringHash> message;
        flag1 = this.StatComp.CanDoAction("Walk", out message, continuous1);
        flag2 = this.StatComp.CanDoAction("Run", out message, continuous2);
        flag3 = this.StatComp.CanDoAction("Sprint", out message, continuous3);
        if (MySession.Static != null && MySession.Static.LocalCharacter == this && (message.Item1 == (ushort) 4 && message.Item2.String.CompareTo("Stamina") == 0))
        {
          this.m_notEnoughStatNotification.SetTextFormatArguments((object) message.Item2);
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_notEnoughStatNotification);
        }
        flag4 = flag1 | flag2 | flag3;
      }
      bool flag5 = (((double) moveIndicator.X != 0.0 ? 1 : ((double) moveIndicator.Z != 0.0 ? 1 : 0)) & (canMove ? 1 : 0) & (flag4 ? 1 : 0)) != 0;
      bool flag6 = (double) rotationIndicator.X != 0.0 || (double) rotationIndicator.Y != 0.0;
      if (flag5 | movementFlagsChanged)
      {
        movement = !(sprint & flag3) ? (!flag5 ? this.GetIdleState() : (!(walk & flag1) ? (!flag2 ? this.GetWalkingState(ref moveIndicator) : this.GetRunningState(ref moveIndicator)) : this.GetWalkingState(ref moveIndicator))) : this.GetSprintState(ref moveIndicator);
        acceleration = this.GetMovementAcceleration(movement);
        this.m_currentDecceleration = 0.0f;
      }
      else if (flag6)
      {
        if ((double) Math.Abs(rotationIndicator.Y) > 20.0 && (this.m_currentMovementState == MyCharacterMovementEnum.Standing || this.m_currentMovementState == MyCharacterMovementEnum.Crouching))
          movement = !this.WantsCrouch ? ((double) rotationIndicator.Y <= 0.0 ? MyCharacterMovementEnum.RotatingLeft : MyCharacterMovementEnum.RotatingRight) : ((double) rotationIndicator.Y <= 0.0 ? MyCharacterMovementEnum.CrouchRotatingLeft : MyCharacterMovementEnum.CrouchRotatingRight);
      }
      else
      {
        MyCharacterMovementEnum currentMovementState2 = this.m_currentMovementState;
        if ((uint) currentMovementState2 <= 146U)
        {
          if ((uint) currentMovementState2 <= 66U)
          {
            if ((uint) currentMovementState2 <= 18U)
            {
              switch (currentMovementState2)
              {
                case MyCharacterMovementEnum.Standing:
                  if (this.WantsCrouch)
                    movement = this.GetIdleState();
                  this.m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
                  goto label_72;
                case MyCharacterMovementEnum.Crouching:
                  if (!this.WantsCrouch)
                    movement = this.GetIdleState();
                  this.m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
                  goto label_72;
                case MyCharacterMovementEnum.Walking:
                case MyCharacterMovementEnum.CrouchWalking:
                  break;
                default:
                  goto label_72;
              }
            }
            else if ((uint) currentMovementState2 <= 34U)
            {
              if (currentMovementState2 != MyCharacterMovementEnum.BackWalking && currentMovementState2 != MyCharacterMovementEnum.CrouchBackWalking)
                goto label_72;
            }
            else if (currentMovementState2 != MyCharacterMovementEnum.WalkStrafingLeft && currentMovementState2 != MyCharacterMovementEnum.CrouchStrafingLeft)
              goto label_72;
          }
          else if ((uint) currentMovementState2 <= 98U)
          {
            if ((uint) currentMovementState2 <= 82U)
            {
              if (currentMovementState2 != MyCharacterMovementEnum.WalkingLeftFront && currentMovementState2 != MyCharacterMovementEnum.CrouchWalkingLeftFront)
                goto label_72;
            }
            else if (currentMovementState2 != MyCharacterMovementEnum.WalkingLeftBack && currentMovementState2 != MyCharacterMovementEnum.CrouchWalkingLeftBack)
              goto label_72;
          }
          else if ((uint) currentMovementState2 <= 130U)
          {
            if (currentMovementState2 != MyCharacterMovementEnum.WalkStrafingRight && currentMovementState2 != MyCharacterMovementEnum.CrouchStrafingRight)
              goto label_72;
          }
          else if (currentMovementState2 != MyCharacterMovementEnum.WalkingRightFront && currentMovementState2 != MyCharacterMovementEnum.CrouchWalkingRightFront)
            goto label_72;
        }
        else if ((uint) currentMovementState2 <= 1120U)
        {
          if ((uint) currentMovementState2 <= 1040U)
          {
            if (currentMovementState2 != MyCharacterMovementEnum.WalkingRightBack && currentMovementState2 != MyCharacterMovementEnum.CrouchWalkingRightBack && currentMovementState2 != MyCharacterMovementEnum.Running)
              goto label_72;
          }
          else if ((uint) currentMovementState2 <= 1088U)
          {
            if (currentMovementState2 != MyCharacterMovementEnum.Backrunning && currentMovementState2 != MyCharacterMovementEnum.RunStrafingLeft)
              goto label_72;
          }
          else if (currentMovementState2 != MyCharacterMovementEnum.RunningLeftFront && currentMovementState2 != MyCharacterMovementEnum.RunningLeftBack)
            goto label_72;
        }
        else if ((uint) currentMovementState2 <= 2064U)
        {
          if ((uint) currentMovementState2 <= 1168U)
          {
            if (currentMovementState2 != MyCharacterMovementEnum.RunStrafingRight && currentMovementState2 != MyCharacterMovementEnum.RunningRightFront)
              goto label_72;
          }
          else
          {
            switch (currentMovementState2)
            {
              case MyCharacterMovementEnum.RunningRightBack:
                break;
              case MyCharacterMovementEnum.Sprinting:
                movement = this.GetIdleState();
                this.m_currentDecceleration = MyPerGameSettings.CharacterMovement.SprintDecceleration;
                goto label_72;
              default:
                goto label_72;
            }
          }
        }
        else
        {
          if ((uint) currentMovementState2 <= 4098U)
          {
            if (currentMovementState2 != MyCharacterMovementEnum.RotatingLeft && currentMovementState2 != MyCharacterMovementEnum.CrouchRotatingLeft)
              goto label_72;
          }
          else if (currentMovementState2 != MyCharacterMovementEnum.RotatingRight && currentMovementState2 != MyCharacterMovementEnum.CrouchRotatingRight)
            goto label_72;
          movement = this.GetIdleState();
          this.m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
          goto label_72;
        }
        movement = this.GetIdleState();
        this.m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
      }
label_72:
      return movement;
    }

    internal float LimitMaxSpeed(float currentSpeed, MyCharacterMovementEnum movementState)
    {
      float num = currentSpeed;
      float forMovementState = this.GetMovementMultiplierForMovementState(movementState);
      if ((uint) movementState <= 160U)
      {
        if ((uint) movementState <= 80U)
        {
          if ((uint) movementState <= 32U)
          {
            if ((uint) movementState <= 16U)
            {
              switch (movementState)
              {
                case MyCharacterMovementEnum.Flying:
                  goto label_32;
                case MyCharacterMovementEnum.Walking:
                  num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxWalkSpeed * forMovementState, this.Definition.MaxWalkSpeed * forMovementState);
                  goto label_42;
                default:
                  goto label_42;
              }
            }
            else
            {
              switch (movementState)
              {
                case MyCharacterMovementEnum.CrouchWalking:
                  num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxCrouchWalkSpeed * forMovementState, this.Definition.MaxCrouchWalkSpeed * forMovementState);
                  goto label_42;
                case MyCharacterMovementEnum.BackWalking:
                  break;
                default:
                  goto label_42;
              }
            }
          }
          else if ((uint) movementState <= 64U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.CrouchBackWalking:
                goto label_40;
              case MyCharacterMovementEnum.WalkStrafingLeft:
                goto label_35;
              default:
                goto label_42;
            }
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.CrouchStrafingLeft:
                goto label_39;
              case MyCharacterMovementEnum.WalkingLeftFront:
                goto label_35;
              default:
                goto label_42;
            }
          }
        }
        else if ((uint) movementState <= 128U)
        {
          if ((uint) movementState <= 96U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.CrouchWalkingLeftFront:
                goto label_39;
              case MyCharacterMovementEnum.WalkingLeftBack:
                break;
              default:
                goto label_42;
            }
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.CrouchWalkingLeftBack:
                goto label_40;
              case MyCharacterMovementEnum.WalkStrafingRight:
                goto label_35;
              default:
                goto label_42;
            }
          }
        }
        else if ((uint) movementState <= 144U)
        {
          switch (movementState)
          {
            case MyCharacterMovementEnum.CrouchStrafingRight:
              goto label_39;
            case MyCharacterMovementEnum.WalkingRightFront:
              goto label_35;
            default:
              goto label_42;
          }
        }
        else
        {
          switch (movementState)
          {
            case MyCharacterMovementEnum.CrouchWalkingRightFront:
              goto label_39;
            case MyCharacterMovementEnum.WalkingRightBack:
              break;
            default:
              goto label_42;
          }
        }
        num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxWalkStrafingSpeed * forMovementState, this.Definition.MaxWalkStrafingSpeed * forMovementState);
        goto label_42;
label_35:
        num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxWalkStrafingSpeed * forMovementState, this.Definition.MaxWalkStrafingSpeed * forMovementState);
        goto label_42;
label_39:
        num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxCrouchStrafingSpeed * forMovementState, this.Definition.MaxCrouchStrafingSpeed * forMovementState);
        goto label_42;
      }
      else
      {
        if ((uint) movementState <= 1120U)
        {
          if ((uint) movementState <= 1040U)
          {
            if ((uint) movementState <= 263U)
            {
              if (movementState != MyCharacterMovementEnum.CrouchWalkingRightBack)
              {
                if (movementState == MyCharacterMovementEnum.LadderUp)
                  goto label_42;
                else
                  goto label_42;
              }
              else
                goto label_40;
            }
            else if (movementState == MyCharacterMovementEnum.LadderDown || movementState != MyCharacterMovementEnum.Running)
              goto label_42;
            else
              goto label_32;
          }
          else if ((uint) movementState <= 1088U)
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.Backrunning:
                break;
              case MyCharacterMovementEnum.RunStrafingLeft:
                goto label_37;
              default:
                goto label_42;
            }
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.RunningLeftFront:
                goto label_37;
              case MyCharacterMovementEnum.RunningLeftBack:
                break;
              default:
                goto label_42;
            }
          }
        }
        else if ((uint) movementState <= 2064U)
        {
          if ((uint) movementState <= 1168U)
          {
            if (movementState == MyCharacterMovementEnum.RunStrafingRight || movementState == MyCharacterMovementEnum.RunningRightFront)
              goto label_37;
            else
              goto label_42;
          }
          else
          {
            switch (movementState)
            {
              case MyCharacterMovementEnum.RunningRightBack:
                break;
              case MyCharacterMovementEnum.Sprinting:
                num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxSprintSpeed * forMovementState, this.Definition.MaxSprintSpeed * forMovementState);
                goto label_42;
              default:
                goto label_42;
            }
          }
        }
        else if ((uint) movementState <= 4098U)
        {
          if (movementState == MyCharacterMovementEnum.RotatingLeft || movementState == MyCharacterMovementEnum.CrouchRotatingLeft)
            goto label_42;
          else
            goto label_42;
        }
        else if (movementState == MyCharacterMovementEnum.RotatingRight || movementState == MyCharacterMovementEnum.CrouchRotatingRight || movementState == MyCharacterMovementEnum.LadderOut)
          goto label_42;
        else
          goto label_42;
        num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxBackrunSpeed * forMovementState, this.Definition.MaxBackrunSpeed * forMovementState);
        goto label_42;
label_37:
        num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxRunStrafingSpeed * forMovementState, this.Definition.MaxRunStrafingSpeed * forMovementState);
        goto label_42;
      }
label_32:
      num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxRunSpeed * forMovementState, this.Definition.MaxRunSpeed * forMovementState);
      goto label_42;
label_40:
      num = MathHelper.Clamp(currentSpeed, -this.Definition.MaxCrouchBackwalkSpeed * forMovementState, this.Definition.MaxCrouchBackwalkSpeed * forMovementState);
label_42:
      return num;
    }

    private float AdjustSafeAnimationBlend(float idealBlend)
    {
      float num = 0.0f;
      if ((double) this.m_currentAnimationChangeDelay > (double) this.SAFE_DELAY_FOR_ANIMATION_BLEND)
        num = idealBlend;
      this.m_currentAnimationChangeDelay = 0.0f;
      return num;
    }

    private MyBlendOption AdjustSafeAnimationEnd(MyBlendOption idealEnd)
    {
      MyBlendOption myBlendOption = MyBlendOption.Immediate;
      if ((double) this.m_currentAnimationChangeDelay > (double) this.SAFE_DELAY_FOR_ANIMATION_BLEND)
        myBlendOption = idealEnd;
      return myBlendOption;
    }

    private MyCharacterMovementEnum GetWalkingState(ref Vector3 moveIndicator)
    {
      double num = Math.Tan((double) MathHelper.ToRadians(23f));
      return (double) Math.Abs(moveIndicator.X) < num * (double) Math.Abs(moveIndicator.Z) ? ((double) moveIndicator.Z < 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.Walking : MyCharacterMovementEnum.CrouchWalking) : (!this.WantsCrouch ? MyCharacterMovementEnum.BackWalking : MyCharacterMovementEnum.CrouchBackWalking)) : ((double) Math.Abs(moveIndicator.X) * num > (double) Math.Abs(moveIndicator.Z) ? ((double) moveIndicator.X > 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.WalkStrafingRight : MyCharacterMovementEnum.CrouchStrafingRight) : (!this.WantsCrouch ? MyCharacterMovementEnum.WalkStrafingLeft : MyCharacterMovementEnum.CrouchStrafingLeft)) : ((double) moveIndicator.X > 0.0 ? ((double) moveIndicator.Z < 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.WalkingRightFront : MyCharacterMovementEnum.CrouchWalkingRightFront) : (!this.WantsCrouch ? MyCharacterMovementEnum.WalkingRightBack : MyCharacterMovementEnum.CrouchWalkingRightBack)) : ((double) moveIndicator.Z < 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.WalkingLeftFront : MyCharacterMovementEnum.CrouchWalkingLeftFront) : (!this.WantsCrouch ? MyCharacterMovementEnum.WalkingLeftBack : MyCharacterMovementEnum.CrouchWalkingLeftBack))));
    }

    private MyCharacterMovementEnum GetRunningState(ref Vector3 moveIndicator)
    {
      double num = Math.Tan((double) MathHelper.ToRadians(23f));
      return (double) Math.Abs(moveIndicator.X) < num * (double) Math.Abs(moveIndicator.Z) ? ((double) moveIndicator.Z < 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.Running : MyCharacterMovementEnum.CrouchWalking) : (!this.WantsCrouch ? MyCharacterMovementEnum.Backrunning : MyCharacterMovementEnum.CrouchBackWalking)) : ((double) Math.Abs(moveIndicator.X) * num > (double) Math.Abs(moveIndicator.Z) ? ((double) moveIndicator.X > 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.RunStrafingRight : MyCharacterMovementEnum.CrouchStrafingRight) : (!this.WantsCrouch ? MyCharacterMovementEnum.RunStrafingLeft : MyCharacterMovementEnum.CrouchStrafingLeft)) : ((double) moveIndicator.X > 0.0 ? ((double) moveIndicator.Z < 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.RunningRightFront : MyCharacterMovementEnum.CrouchWalkingRightFront) : (!this.WantsCrouch ? MyCharacterMovementEnum.RunningRightBack : MyCharacterMovementEnum.CrouchWalkingRightBack)) : ((double) moveIndicator.Z < 0.0 ? (!this.WantsCrouch ? MyCharacterMovementEnum.RunningLeftFront : MyCharacterMovementEnum.CrouchWalkingLeftFront) : (!this.WantsCrouch ? MyCharacterMovementEnum.RunningLeftBack : MyCharacterMovementEnum.CrouchWalkingLeftBack))));
    }

    private MyCharacterMovementEnum GetSprintState(ref Vector3 moveIndicator) => (double) Math.Abs(moveIndicator.X) < 0.100000001490116 && (double) moveIndicator.Z < 0.0 && !this.IsShooting(MyShootActionEnum.PrimaryAction) ? MyCharacterMovementEnum.Sprinting : this.GetRunningState(ref moveIndicator);

    private MyCharacterMovementEnum GetIdleState() => !this.WantsCrouch ? MyCharacterMovementEnum.Standing : MyCharacterMovementEnum.Crouching;

    private bool UpdateCapsuleBones()
    {
      if (this.m_characterBoneCapsulesReady)
        return true;
      if (this.m_bodyCapsuleInfo == null || this.m_bodyCapsuleInfo.Count == 0)
        return false;
      MyRenderDebugInputComponent.Clear();
      MyCharacterBone[] characterBones = this.AnimationController.CharacterBones;
      if (this.Physics.Ragdoll != null && this.Components.Has<MyCharacterRagdollComponent>())
      {
        MyCharacterRagdollComponent ragdollComponent = this.Components.Get<MyCharacterRagdollComponent>();
        for (int index = 0; index < this.m_bodyCapsuleInfo.Count; ++index)
        {
          MyBoneCapsuleInfo myBoneCapsuleInfo = this.m_bodyCapsuleInfo[index];
          if (characterBones != null && myBoneCapsuleInfo.Bone1 < characterBones.Length && myBoneCapsuleInfo.Bone2 < characterBones.Length)
          {
            HkRigidBody bodyBindedToBone = ragdollComponent.RagdollMapper.GetBodyBindedToBone(characterBones[myBoneCapsuleInfo.Bone1]);
            MatrixD matrix = characterBones[myBoneCapsuleInfo.Bone1].AbsoluteTransform * this.WorldMatrix;
            HkShape shape = bodyBindedToBone.GetShape();
            this.m_bodyCapsules[index].P0 = matrix.Translation;
            this.m_bodyCapsules[index].P1 = (characterBones[myBoneCapsuleInfo.Bone2].AbsoluteTransform * this.WorldMatrix).Translation;
            Vector3 vector3 = (Vector3) (this.m_bodyCapsules[index].P0 - this.m_bodyCapsules[index].P1);
            if ((double) vector3.LengthSquared() < 0.0500000007450581)
            {
              if (shape.ShapeType == HkShapeType.Capsule)
              {
                HkCapsuleShape hkCapsuleShape = (HkCapsuleShape) shape;
                this.m_bodyCapsules[index].P0 = Vector3.Transform(hkCapsuleShape.VertexA, matrix);
                this.m_bodyCapsules[index].P1 = Vector3.Transform(hkCapsuleShape.VertexB, matrix);
                this.m_bodyCapsules[index].Radius = hkCapsuleShape.Radius * 0.8f;
                if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
                  MyRenderDebugInputComponent.AddCapsule(this.m_bodyCapsules[index], Color.Green);
              }
              else
              {
                Vector4 min;
                Vector4 max;
                shape.GetLocalAABB(0.0001f, out min, out max);
                float num = Math.Max(Math.Max(max.X - min.X, max.Y - min.Y), max.Z - min.Z) * 0.5f;
                this.m_bodyCapsules[index].P0 = matrix.Translation + matrix.Left * (double) num * 0.25;
                this.m_bodyCapsules[index].P1 = matrix.Translation + matrix.Left * (double) num * 0.5;
                this.m_bodyCapsules[index].Radius = num * 0.25f;
                if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
                  MyRenderDebugInputComponent.AddCapsule(this.m_bodyCapsules[index], Color.Blue);
              }
            }
            else
            {
              if ((double) myBoneCapsuleInfo.Radius != 0.0)
                this.m_bodyCapsules[index].Radius = myBoneCapsuleInfo.Radius;
              else if (shape.ShapeType == HkShapeType.Capsule)
              {
                HkCapsuleShape hkCapsuleShape = (HkCapsuleShape) shape;
                this.m_bodyCapsules[index].Radius = hkCapsuleShape.Radius;
              }
              else
                this.m_bodyCapsules[index].Radius = vector3.Length() * 0.28f;
              if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
              {
                MyRenderDebugInputComponent.AddCapsule(this.m_bodyCapsules[index], Color.Blue);
                MyRenderProxy.DebugDrawCapsule(this.m_bodyCapsules[index].P0, this.m_bodyCapsules[index].P1, this.m_bodyCapsules[index].Radius, Color.Yellow, false);
              }
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < this.m_bodyCapsuleInfo.Count; ++index)
        {
          MyBoneCapsuleInfo myBoneCapsuleInfo = this.m_bodyCapsuleInfo[index];
          if (characterBones != null && myBoneCapsuleInfo.Bone1 < characterBones.Length && myBoneCapsuleInfo.Bone2 < characterBones.Length)
          {
            ref CapsuleD local1 = ref this.m_bodyCapsules[index];
            MatrixD matrixD = characterBones[myBoneCapsuleInfo.Bone1].AbsoluteTransform * this.WorldMatrix;
            Vector3D translation1 = matrixD.Translation;
            local1.P0 = translation1;
            ref CapsuleD local2 = ref this.m_bodyCapsules[index];
            matrixD = characterBones[myBoneCapsuleInfo.Bone2].AbsoluteTransform * this.WorldMatrix;
            Vector3D translation2 = matrixD.Translation;
            local2.P1 = translation2;
            Vector3 vector3 = (Vector3) (this.m_bodyCapsules[index].P0 - this.m_bodyCapsules[index].P1);
            if ((double) myBoneCapsuleInfo.Radius != 0.0)
              this.m_bodyCapsules[index].Radius = myBoneCapsuleInfo.Radius;
            else if ((double) vector3.LengthSquared() < 0.0500000007450581)
            {
              ref CapsuleD local3 = ref this.m_bodyCapsules[index];
              Vector3D p0 = this.m_bodyCapsules[index].P0;
              matrixD = characterBones[myBoneCapsuleInfo.Bone1].AbsoluteTransform * this.WorldMatrix;
              Vector3D vector3D1 = matrixD.Left * 0.100000001490116;
              Vector3D vector3D2 = p0 + vector3D1;
              local3.P1 = vector3D2;
              this.m_bodyCapsules[index].Radius = 0.1f;
            }
            else
              this.m_bodyCapsules[index].Radius = vector3.Length() * 0.3f;
            if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
              MyRenderDebugInputComponent.AddCapsule(this.m_bodyCapsules[index], Color.Green);
          }
        }
      }
      this.m_characterBoneCapsulesReady = true;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
      {
        foreach (Tuple<CapsuleD, Color> tuple in MyRenderDebugInputComponent.CapsulesToDraw)
          MyRenderProxy.DebugDrawCapsule(tuple.Item1.P0, tuple.Item1.P1, tuple.Item1.Radius, tuple.Item2, false);
      }
      return true;
    }

    private MatrixD GetHeadMatrixInternal(
      int headBone,
      bool includeY,
      bool includeX = true,
      bool forceHeadAnim = false,
      bool forceHeadBone = false)
    {
      if (this.PositionComp == null)
        return MatrixD.Identity;
      MatrixD matrixD1 = MatrixD.Identity;
      bool flag = ((!this.ShouldUseAnimatedHeadRotation() ? 0 : (!this.JetpackRunning ? 1 : (this.IsLocalHeadAnimationInProgress() ? 1 : 0))) | (forceHeadAnim ? 1 : 0)) != 0;
      if (includeX && !flag)
        matrixD1 = MatrixD.CreateFromAxisAngle(Vector3D.Right, (double) MathHelper.ToRadians(this.m_headLocalXAngle));
      if (includeY)
        matrixD1 *= Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(this.m_headLocalYAngle));
      Vector3 vector3_1 = Vector3.Zero;
      if (headBone != -1)
      {
        vector3_1 = this.BoneAbsoluteTransforms[headBone].Translation;
        float num = 1f - (float) Math.Cos((double) MathHelper.ToRadians(this.m_headLocalXAngle));
        vector3_1.Y += num * this.AnimationController.InverseKinematics.RootBoneVerticalOffset;
      }
      if (flag && headBone != -1)
      {
        Vector3 vector3_2 = this.BoneAbsoluteTransforms[headBone].Right;
        if ((double) vector3_2.LengthSquared() > 1.40129846432482E-45)
        {
          vector3_2 = this.BoneAbsoluteTransforms[headBone].Up;
          if ((double) vector3_2.LengthSquared() > 1.40129846432482E-45)
          {
            vector3_2 = this.BoneAbsoluteTransforms[headBone].Forward;
            if ((double) vector3_2.LengthSquared() > 1.40129846432482E-45)
            {
              Matrix identity = Matrix.Identity;
              identity.Translation = vector3_1;
              this.m_headMatrix = MatrixD.CreateRotationX(-1.0 * Math.PI / 2.0) * identity;
              goto label_14;
            }
          }
        }
      }
      this.m_headMatrix = MatrixD.CreateTranslation(0.0, (double) vector3_1.Y, (double) vector3_1.Z);
label_14:
      if (this.IsInFirstPersonView && !MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER)
      {
        float num1 = Math.Abs(this.m_headMovementXOffset);
        float num2 = 0.03f;
        MyTimeSpan simulationTime;
        if ((double) num1 > 0.0 && (double) num1 < (double) this.m_maxHeadMovementOffset / 2.0)
        {
          ref MatrixD local = ref this.m_headMatrix;
          Vector3D translation = local.Translation;
          Vector3D vector3D1 = (double) num2 * this.m_headMatrix.Up * (double) num1;
          simulationTime = MySandboxGame.Static.SimulationTime;
          double num3 = Math.Sin(10.0 * simulationTime.Seconds) + 3.0;
          Vector3D vector3D2 = vector3D1 * num3;
          local.Translation = translation + vector3D2;
        }
        else if ((double) num1 > 0.0)
          this.m_headMatrix.Translation += (double) num2 * this.m_headMatrix.Up * ((double) this.m_maxHeadMovementOffset - (double) num1) * (Math.Sin(10.0 * MySandboxGame.Static.SimulationTime.Seconds) + 3.0);
        float num4 = Math.Abs(this.m_headMovementYOffset);
        if ((double) num4 > 0.0 && (double) num4 < (double) this.m_maxHeadMovementOffset / 2.0)
        {
          ref MatrixD local = ref this.m_headMatrix;
          Vector3D translation = local.Translation;
          Vector3D vector3D1 = (double) num2 * this.m_headMatrix.Up * (double) num4;
          simulationTime = MySandboxGame.Static.SimulationTime;
          double num3 = Math.Sin(10.0 * simulationTime.Seconds) + 3.0;
          Vector3D vector3D2 = vector3D1 * num3;
          local.Translation = translation + vector3D2;
        }
        else if ((double) num4 > 0.0)
        {
          ref MatrixD local = ref this.m_headMatrix;
          Vector3D translation = local.Translation;
          Vector3D vector3D1 = (double) num2 * this.m_headMatrix.Up * ((double) this.m_maxHeadMovementOffset - (double) num4);
          simulationTime = MySandboxGame.Static.SimulationTime;
          double num3 = Math.Sin(10.0 * simulationTime.Seconds) + 3.0;
          Vector3D vector3D2 = vector3D1 * num3;
          local.Translation = translation + vector3D2;
        }
      }
      MatrixD matrixD2 = matrixD1 * this.m_headMatrix * this.WorldMatrix;
      MatrixD fromDir = MatrixD.CreateFromDir(this.WorldMatrix.Forward, this.WorldMatrix.Up);
      MatrixD matrixD3 = this.m_headMatrix * matrixD1 * fromDir;
      matrixD3.Translation = matrixD2.Translation;
      return matrixD3;
    }

    public MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceHeadAnim = false,
      bool forceHeadBone = false,
      bool preferLocalOverSync = false)
    {
      return this.GetHeadMatrixInternal(this.IsInFirstPersonView | forceHeadBone ? this.m_headBoneIndex : this.m_camera3rdBoneIndex, includeY, includeX, forceHeadAnim, forceHeadBone);
    }

    public MatrixD Get3rdCameraMatrix(bool includeY, bool includeX = true)
    {
      MatrixD matrixD = this.Get3rdBoneMatrix(includeY, includeX);
      Matrix matrix = Matrix.Invert((Matrix) ref matrixD);
      return (MatrixD) ref matrix;
    }

    public MatrixD Get3rdBoneMatrix(bool includeY, bool includeX = true) => this.GetHeadMatrixInternal(this.m_camera3rdBoneIndex, includeY, includeX);

    public override MatrixD GetViewMatrix()
    {
      if (this.IsDead && MyPerGameSettings.SwitchToSpectatorCameraAfterDeath)
      {
        this.m_isInFirstPersonView = false;
        MatrixD matrixD;
        if (this.m_lastCorrectSpectatorCamera == MatrixD.Zero)
        {
          matrixD = this.WorldMatrix;
          Vector3D cameraPosition = matrixD.Translation + 2f * Vector3.Up - 2f * Vector3.Forward;
          matrixD = this.WorldMatrix;
          Vector3D translation = matrixD.Translation;
          Vector3 up = Vector3.Up;
          this.m_lastCorrectSpectatorCamera = MatrixD.CreateLookAt(cameraPosition, translation, up);
        }
        matrixD = MatrixD.Invert(this.m_lastCorrectSpectatorCamera);
        Vector3 translation1 = (Vector3) matrixD.Translation;
        matrixD = this.WorldMatrix;
        Vector3 vector3 = (Vector3) matrixD.Translation;
        if (this.m_headBoneIndex != -1)
          vector3 = (Vector3) Vector3.Transform(this.AnimationController.CharacterBones[this.m_headBoneIndex].AbsoluteTransform.Translation, this.WorldMatrix);
        MatrixD lookAt = MatrixD.CreateLookAt((Vector3D) translation1, (Vector3D) vector3, Vector3.Up);
        return !lookAt.IsValid() || !(lookAt != MatrixD.Zero) ? this.m_lastCorrectSpectatorCamera : lookAt;
      }
      if (this.IsDead)
      {
        MySpectator mySpectator = MySpectator.Static;
        Vector3D position = this.PositionComp.GetPosition();
        MatrixD worldMatrix = this.WorldMatrix;
        Vector3D up1 = worldMatrix.Up;
        Vector3D target = position + up1;
        worldMatrix = this.WorldMatrix;
        Vector3D? up2 = new Vector3D?(worldMatrix.Up);
        mySpectator.SetTarget(target, up2);
      }
      if ((!this.ForceFirstPersonCamera || !this.IsDead) && !this.m_isInFirstPersonView)
      {
        int num = this.ForceFirstPersonCamera ? 1 : 0;
        if (!this.ForceFirstPersonCamera & !MyThirdPersonSpectator.Static.IsCameraForced())
          return MyThirdPersonSpectator.Static.GetViewMatrix();
      }
      MatrixD headMatrix = this.GetHeadMatrix(true, true, false, this.ForceFirstPersonCamera, true);
      if (this.IsDead)
      {
        Vector3D translation = headMatrix.Translation;
        Vector3D vector3D = (Vector3D) -MyGravityProviderSystem.CalculateTotalGravityInPoint(translation);
        if (!Vector3D.IsZero(vector3D))
        {
          Vector3 halfExtents = new Vector3(this.Definition.CharacterHeadSize * 0.5f);
          this.m_penetrationList.Clear();
          MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref Quaternion.Identity, this.m_penetrationList, 0);
          foreach (HkBodyCollision penetration in this.m_penetrationList)
          {
            VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
            if (collisionEntity is MyVoxelBase || collisionEntity is MyCubeGrid)
            {
              vector3D.Normalize();
              headMatrix.Translation += vector3D;
              this.m_forceFirstPersonCamera = false;
              this.m_isInFirstPersonView = false;
              this.m_isInFirstPerson = false;
              break;
            }
          }
        }
      }
      this.m_lastCorrectSpectatorCamera = MatrixD.Zero;
      if (this.IsDead && this.m_lastGetViewWasDead)
      {
        MatrixD matrix = (MatrixD) ref this.m_getViewAliveWorldMatrix;
        matrix.Translation = headMatrix.Translation;
        return MatrixD.Invert(matrix);
      }
      this.m_getViewAliveWorldMatrix = (Matrix) ref headMatrix;
      this.m_getViewAliveWorldMatrix.Translation = Vector3.Zero;
      this.m_lastGetViewWasDead = this.IsDead;
      return MatrixD.Invert(headMatrix);
    }

    public MatrixD GetSpineInvertedWorldMatrix() => this.GetHeadMatrixInternal(this.m_spineBone, true);

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? tri,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      int num = this.GetIntersectionWithLine(ref line, ref MyCharacter.m_hitInfoTmp, flags) ? 1 : 0;
      tri = new MyIntersectionResultLineTriangleEx?(MyCharacter.m_hitInfoTmp.Triangle);
      return num != 0;
    }

    public void DebugCapsuleDraw()
    {
      for (int index = 0; index < this.m_bodyCapsules.Length; ++index)
      {
        CapsuleD bodyCapsule = this.m_bodyCapsules[index];
        MyRenderProxy.DebugDrawCapsule(bodyCapsule.P0, bodyCapsule.P1, bodyCapsule.Radius, this.Definition.WeakPointBoneIndices.Contains(index) ? Color.Yellow : Color.Green, false, true);
      }
    }

    public bool GetIntersectionWithLine(
      ref LineD line,
      ref MyCharacterHitInfo info,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      if (info == null)
        info = new MyCharacterHitInfo();
      info.Reset();
      if (!this.UpdateCapsuleBones())
        return false;
      double num1 = double.MaxValue;
      Vector3D zero1 = Vector3D.Zero;
      Vector3D zero2 = Vector3D.Zero;
      Vector3 zero3 = Vector3.Zero;
      Vector3 zero4 = Vector3.Zero;
      int capsuleIndex = -1;
      for (int index = 0; index < this.m_bodyCapsules.Length; ++index)
      {
        if (this.m_bodyCapsules[index].Intersect(line, ref zero1, ref zero2, ref zero3, ref zero4))
        {
          double num2 = (double) Vector3.Distance((Vector3) zero1, (Vector3) line.From);
          if (num2 < num1)
          {
            num1 = num2;
            capsuleIndex = index;
          }
        }
      }
      if (capsuleIndex != -1)
      {
        MatrixD matrix1 = this.PositionComp.WorldMatrixRef;
        int bestBone = this.FindBestBone(capsuleIndex, ref zero1, ref matrix1);
        MatrixD matrix2 = this.PositionComp.WorldMatrixNormalizedInv;
        Vector3D vector3D1 = Vector3D.Transform(line.From, ref matrix2);
        Vector3D vector3D2 = Vector3D.Transform(line.To, ref matrix2);
        Line line1 = new Line((Vector3) vector3D1, (Vector3) vector3D2);
        MyCharacterBone characterBone = this.AnimationController.CharacterBones[bestBone];
        characterBone.ComputeAbsoluteTransform();
        Matrix matrix3 = characterBone.SkinTransform * characterBone.AbsoluteTransform;
        Matrix matrix4 = Matrix.Invert(matrix3);
        Vector3D position1 = (Vector3D) Vector3.Transform((Vector3) vector3D1, ref matrix4);
        Vector3D position2 = (Vector3D) Vector3.Transform((Vector3) vector3D2, ref matrix4);
        LineD line2 = new LineD(Vector3D.Transform(position1, ref matrix1), Vector3D.Transform(position2, ref matrix1));
        MyIntersectionResultLineTriangleEx? t;
        if (base.GetIntersectionWithLine(ref line2, out t, flags))
        {
          MyIntersectionResultLineTriangleEx resultLineTriangleEx1 = t.Value;
          info.CapsuleIndex = capsuleIndex;
          info.BoneIndex = bestBone;
          info.Capsule = this.m_bodyCapsules[info.CapsuleIndex];
          info.HitHead = this.Definition.WeakPointBoneIndices.Contains(info.CapsuleIndex) && this.m_bodyCapsules.Length > 1;
          info.HitPositionBindingPose = resultLineTriangleEx1.IntersectionPointInObjectSpace;
          info.HitNormalBindingPose = resultLineTriangleEx1.NormalInObjectSpace;
          info.BindingTransformation = matrix3;
          MyTriangle_Vertices triangle = new MyTriangle_Vertices();
          triangle.Vertex0 = Vector3.Transform(resultLineTriangleEx1.Triangle.InputTriangle.Vertex0, ref matrix3);
          triangle.Vertex1 = Vector3.Transform(resultLineTriangleEx1.Triangle.InputTriangle.Vertex1, ref matrix3);
          triangle.Vertex2 = Vector3.Transform(resultLineTriangleEx1.Triangle.InputTriangle.Vertex2, ref matrix3);
          Vector3 triangleNormal = Vector3.TransformNormal(resultLineTriangleEx1.Triangle.InputTriangleNormal, matrix3);
          MyIntersectionResultLineTriangle resultLineTriangle = new MyIntersectionResultLineTriangle(resultLineTriangleEx1.Triangle.TriangleIndex, ref triangle, ref resultLineTriangleEx1.Triangle.BoneWeights, ref triangleNormal, resultLineTriangleEx1.Triangle.Distance);
          Vector3 vector3_1 = Vector3.Transform(resultLineTriangleEx1.IntersectionPointInObjectSpace, ref matrix3);
          Vector3 normal = Vector3.TransformNormal(resultLineTriangleEx1.NormalInObjectSpace, matrix3);
          MyIntersectionResultLineTriangleEx resultLineTriangleEx2 = new MyIntersectionResultLineTriangleEx();
          resultLineTriangleEx2.Triangle = resultLineTriangle;
          resultLineTriangleEx2.IntersectionPointInObjectSpace = vector3_1;
          resultLineTriangleEx2.NormalInObjectSpace = normal;
          resultLineTriangleEx2.IntersectionPointInWorldSpace = Vector3D.Transform((Vector3D) vector3_1, ref matrix1);
          resultLineTriangleEx2.NormalInWorldSpace = Vector3.TransformNormal(normal, matrix1);
          resultLineTriangleEx2.InputLineInObjectSpace = line1;
          resultLineTriangleEx2.Entity = t.Value.Entity;
          info.Triangle = resultLineTriangleEx2;
          if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
          {
            MyRenderProxy.DebugClearPersistentMessages();
            MyRenderProxy.DebugDrawCapsule(info.Capsule.P0, info.Capsule.P1, info.Capsule.Radius, Color.Aqua, false, persistent: true);
            Vector3 position3 = (Vector3) Vector3D.Transform(info.Capsule.P0, ref matrix2);
            Vector3 position4 = (Vector3) Vector3D.Transform(info.Capsule.P1, ref matrix2);
            Vector3 vector3_2 = Vector3.Transform(position3, ref matrix4);
            ref Matrix local = ref matrix4;
            Vector3 vector3_3 = Vector3.Transform(position4, ref local);
            MyRenderProxy.DebugDrawCapsule(Vector3D.Transform((Vector3D) vector3_2, ref matrix1), Vector3D.Transform((Vector3D) vector3_3, ref matrix1), info.Capsule.Radius, Color.Brown, false, persistent: true);
            MyRenderProxy.DebugDrawLine3D(line.From, line.To, Color.Blue, Color.Red, false, true);
            MyRenderProxy.DebugDrawLine3D(line2.From, line2.To, Color.Green, Color.Yellow, false, true);
            MyRenderProxy.DebugDrawSphere(resultLineTriangleEx2.IntersectionPointInWorldSpace, 0.02f, Color.Red, depthRead: false, persistent: true);
            MyRenderProxy.DebugDrawAxis((MatrixD) ref matrix3 * this.WorldMatrix, 0.1f, false, true, true);
          }
          return true;
        }
      }
      return false;
    }

    private int FindBestBone(int capsuleIndex, ref Vector3D hitPosition, ref MatrixD worldMatrix)
    {
      MyBoneCapsuleInfo myBoneCapsuleInfo = this.m_bodyCapsuleInfo[capsuleIndex];
      CapsuleD bodyCapsule = this.m_bodyCapsules[capsuleIndex];
      MyCharacterBone characterBone1 = this.AnimationController.CharacterBones[myBoneCapsuleInfo.AscendantBone];
      MyCharacterBone characterBone2 = this.AnimationController.CharacterBones[myBoneCapsuleInfo.DescendantBone];
      Vector3D vector2 = (Vector3D) Vector3.Normalize(bodyCapsule.P0 - bodyCapsule.P1);
      double num1 = vector2.Length();
      double num2 = Vector3D.Dot(hitPosition - bodyCapsule.P1, vector2) / num1;
      int index = characterBone2.Index;
      double num3 = 0.0;
      MyCharacterBone parent = characterBone2.Parent;
      while (num2 >= num3 && index != characterBone1.Index)
      {
        num3 = Vector3D.Dot(Vector3D.Transform((Vector3D) parent.AbsoluteTransform.Translation, ref worldMatrix) - bodyCapsule.P1, vector2) / num1;
        index = parent.Index;
        parent = parent.Parent;
        if (parent == null)
          break;
      }
      return index;
    }

    public void BeginShoot(MyShootActionEnum action)
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        return;
      if (this.m_currentWeapon == null)
      {
        if (action == MyShootActionEnum.SecondaryAction)
        {
          if (MyControllerHelper.IsControl(this.ControlContext, MyControlsSpace.BUILD_PLANNER, MyControlStateType.PRESSED))
            return;
          this.UseTerminal();
        }
        else
        {
          this.Use();
          this.m_usingByPrimary = true;
          if (!MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
            return;
          PerFrameData data = new PerFrameData()
          {
            UseData = new UseData?(new UseData()
            {
              Use = true
            })
          };
          MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, data);
        }
      }
      else
      {
        MyShootActionEnum? shootingAction = this.GetShootingAction();
        if (shootingAction.HasValue && action != shootingAction.Value)
        {
          MyShootActionEnum? nullable = shootingAction;
          MyShootActionEnum myShootActionEnum1 = MyShootActionEnum.PrimaryAction;
          if (!(nullable.GetValueOrDefault() == myShootActionEnum1 & nullable.HasValue) || action != MyShootActionEnum.SecondaryAction)
          {
            if (action == MyShootActionEnum.PrimaryAction)
            {
              nullable = shootingAction;
              MyShootActionEnum myShootActionEnum2 = MyShootActionEnum.SecondaryAction;
              if (nullable.GetValueOrDefault() == myShootActionEnum2 & nullable.HasValue)
                goto label_14;
            }
            this.EndShoot(shootingAction.Value);
          }
        }
label_14:
        if (!this.m_currentWeapon.EnabledInWorldRules)
        {
          MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
        }
        else
        {
          if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
          {
            PerFrameData data = new PerFrameData()
            {
              ShootData = new ShootData?(new ShootData()
              {
                Begin = true,
                ShootAction = (byte) action
              })
            };
            MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, data);
          }
          this.UpdateShootDirection(true, this.m_currentWeapon is MyUserControllableGun || this.m_currentWeapon is MyAutomaticRifleGun);
          this.BeginShootSync(this.ShootDirection, action, MyGuiScreenGamePlay.DoubleClickDetected != null && MyGuiScreenGamePlay.DoubleClickDetected[(int) action]);
        }
      }
    }

    public void OnBeginShoot(MyShootActionEnum action)
    {
      if (this.ControllerInfo == null || this.m_currentWeapon == null || this.m_isShooting[(int) action])
        return;
      MyGunStatusEnum status = MyGunStatusEnum.OK;
      this.m_currentWeapon.CanShoot(action, this.ControllerInfo.ControllingIdentityId, out status);
      if (this.m_shootDoubleClick && status == MyGunStatusEnum.Cooldown)
      {
        status = MyGunStatusEnum.OK;
        this.m_shootDoubleClick = false;
      }
      this.m_isShooting[(int) action] = status == MyGunStatusEnum.OK;
      if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
        return;
      this.ShootBeginFailed(action, status);
    }

    private void UpdateAmmoState()
    {
      if (this.ControllerInfo == null)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_currentAmmoCount.Value = this.m_currentWeapon.CurrentMagazineAmmunition;
        this.m_currentMagazineAmmoCount.Value = this.m_currentWeapon.CurrentMagazineAmount;
      }
      else
      {
        this.m_currentWeapon.CurrentMagazineAmmunition = (int) this.m_currentAmmoCount;
        this.m_currentWeapon.CurrentMagazineAmount = (int) this.m_currentMagazineAmmoCount;
      }
    }

    private void ShootInternal()
    {
      MyGunStatusEnum status = MyGunStatusEnum.OK;
      if (this.ControllerInfo == null)
        return;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer && MySession.Static.LocalCharacter == this && this.m_currentWeapon is MyEngineerToolBase mCurrentWeapon)
      {
        MySlimBlock targetBlock = mCurrentWeapon.GetTargetBlock();
        if (targetBlock != null)
        {
          this.AimedGrid = targetBlock.CubeGrid.EntityId;
          this.AimedBlock = targetBlock.Position;
        }
        else
          this.AimedGrid = 0L;
      }
      this.UpdateShootDirection(true, this.m_currentWeapon is MyUserControllableGun || this.m_currentWeapon is MyAutomaticRifleGun);
      Vector3 direction = this.ShootDirection;
      if (MyFakes.ENABLE_DYNAMIC_EFFECTIVE_RANGE && this.IsInFirstPersonView && (double) this.m_dynamicRangeDistance.Value.distance > 0.0)
      {
        (float, Vector3D) valueTuple = this.m_dynamicRangeDistance.Value;
        direction = Vector3.Normalize(this.m_dynamicRangeDistance.Value.hitPosition + this.PositionComp.GetPosition() - (Sandbox.Game.Multiplayer.Sync.IsDedicated ? this.WeaponPosition.LogicalPositionWorld : this.m_currentWeapon.GetMuzzlePosition()));
      }
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
      {
        this.m_aimedPoint = this.GetAimedPointFromCamera();
        float num = 20f;
        MatrixD matrix = this.GetViewMatrix();
        matrix = MatrixD.Invert(ref matrix);
        MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Forward * (double) num, Color.LightGreen, Color.LightGreen, false);
        MyDebugDrawHelper.DrawNamedPoint(matrix.Translation + matrix.Forward * 5.0, "crosshair", new Color?(Color.LightGreen));
        MyRenderProxy.DebugDrawLine3D(this.WeaponPosition.LogicalPositionWorld, this.WeaponPosition.LogicalPositionWorld + direction * num, Color.Red, Color.Red, false);
        MyDebugDrawHelper.DrawNamedPoint(this.WeaponPosition.LogicalPositionWorld + direction * 5f, "shootdir", new Color?(Color.Red));
        MyDebugDrawHelper.DrawNamedPoint(this.m_aimedPoint, "aimed", new Color?(Color.White));
      }
      foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.IsShooting(action))
        {
          if (this.m_currentWeapon.CanShoot(action, this.ControllerInfo.ControllingIdentityId, out status))
          {
            if (Sandbox.Engine.Platform.Game.IsDedicated)
              this.m_currentWeapon.Shoot(action, direction, new Vector3D?(this.WeaponPosition.LogicalPositionWorld));
            else
              this.m_currentWeapon.Shoot(action, direction, new Vector3D?());
          }
          if (this.m_currentWeapon != null)
          {
            if (MySession.Static.ControlledEntity == this)
            {
              if (status != MyGunStatusEnum.OK && status != MyGunStatusEnum.Cooldown)
                this.ShootFailedLocal(action, status);
              else if (this.m_currentWeapon.IsShooting && status == MyGunStatusEnum.OK)
                this.ShootSuccessfulLocal(action);
            }
            if (status != MyGunStatusEnum.OK && status != MyGunStatusEnum.Cooldown)
              this.m_isShooting[(int) action] = false;
          }
          if (this.m_autoswitch.HasValue)
          {
            this.SwitchToWeapon(this.m_autoswitch);
            this.m_autoswitch = new MyDefinitionId?();
          }
        }
      }
    }

    private void ShootFailedLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status == MyGunStatusEnum.OutOfAmmo)
        this.ShowOutOfAmmoNotification();
      this.m_currentWeapon.ShootFailReactionLocal(action, status);
    }

    private void ShootBeginFailed(MyShootActionEnum action, MyGunStatusEnum status)
    {
      this.m_currentWeapon.BeginFailReaction(action, status);
      this.m_isShooting[(int) action] = false;
      if (MySession.Static.ControlledEntity != this)
        return;
      this.m_currentWeapon.BeginFailReactionLocal(action, status);
    }

    private void ShootSuccessfulLocal(MyShootActionEnum action)
    {
      this.m_currentShotTime = 0.1f;
      this.WeaponPosition.AddBackkick(this.m_currentWeapon.BackkickForcePerSecond * 0.01666667f);
      MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
      if ((double) this.m_currentWeapon.BackkickForcePerSecond <= 0.0 || !this.JetpackRunning && !this.m_isFalling)
        return;
      this.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(-this.m_currentWeapon.BackkickForcePerSecond * (Vector3) (this.m_currentWeapon as MyEntity).WorldMatrix.Forward), new Vector3D?((Vector3D) (Vector3) this.PositionComp.GetPosition()), new Vector3?(), new float?(), true, false);
    }

    public void SetupAutoswitch(MyDefinitionId? switchToNow, MyDefinitionId? switchOnEndShoot)
    {
      this.m_autoswitch = switchToNow;
      this.m_endShootAutoswitch = switchOnEndShoot;
    }

    private void EndShootAll()
    {
      foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.IsShooting(action))
          this.EndShoot(action);
      }
    }

    public void EndShoot(MyShootActionEnum action)
    {
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
      {
        PerFrameData data = new PerFrameData()
        {
          ShootData = new ShootData?(new ShootData()
          {
            Begin = false,
            ShootAction = (byte) action
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, data);
      }
      if (MySession.Static.LocalCharacter == this && this.m_currentMovementState != MyCharacterMovementEnum.Died && this.m_currentWeapon != null)
      {
        if (MyGuiScreenGamePlay.DoubleClickDetected != null && MyGuiScreenGamePlay.DoubleClickDetected[(int) action] && this.m_currentWeapon.CanDoubleClickToStick(action))
          this.GunDoubleClickedSync(action);
        else
          this.EndShootSync(action);
      }
      if (!this.m_usingByPrimary)
        return;
      this.m_usingByPrimary = false;
      this.UseFinished();
    }

    public void OnEndShoot(MyShootActionEnum action)
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died || this.m_currentWeapon == null)
        return;
      this.m_currentWeapon.EndShoot(action);
      if (!this.m_endShootAutoswitch.HasValue)
        return;
      this.SwitchToWeapon(this.m_endShootAutoswitch);
      this.m_endShootAutoswitch = new MyDefinitionId?();
    }

    public void OnGunDoubleClicked(MyShootActionEnum action)
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died || this.m_currentWeapon == null)
        return;
      this.m_currentWeapon.DoubleClicked(action);
    }

    public bool ShouldEndShootingOnPause(MyShootActionEnum action) => this.m_currentMovementState == MyCharacterMovementEnum.Died || this.m_currentWeapon == null || this.m_currentWeapon.ShouldEndShootOnPause(action);

    public void Zoom(bool newKeyPress, bool hideCrosshairWhenAiming = true)
    {
      switch (this.ZoomMode)
      {
        case MyZoomModeEnum.Classic:
          if (!this.Definition.CanIronsight || this.m_currentWeapon == null || MySession.Static.CameraController != this && this.ControllerInfo.IsLocallyControlled())
            break;
          if (!this.IsInFirstPersonView)
          {
            MyGuiScreenGamePlay.Static.SwitchCamera();
            this.m_wasInThirdPersonBeforeIronSight = true;
          }
          this.SoundComp.PlaySecondarySound(CharacterSoundsEnum.IRONSIGHT_ACT_SOUND, true);
          this.EnableIronsight(true, newKeyPress, true, hideCrosshairWhenAiming);
          break;
        case MyZoomModeEnum.IronSight:
          if (MySession.Static.CameraController != this && this.ControllerInfo.IsLocallyControlled())
            break;
          this.SoundComp.PlaySecondarySound(CharacterSoundsEnum.IRONSIGHT_DEACT_SOUND, true);
          this.EnableIronsight(false, newKeyPress, true);
          if (!this.m_wasInThirdPersonBeforeIronSight)
            break;
          MyGuiScreenGamePlay.Static.SwitchCamera();
          break;
      }
    }

    public void EnableIronsight(
      bool enable,
      bool newKeyPress,
      bool changeCamera,
      bool hideCrosshairWhenAiming = true,
      bool forceChange = false)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.LocalCharacter == this)
        MyMultiplayer.RaiseEvent<MyCharacter, bool, bool, bool, bool, bool>(this, (Func<MyCharacter, Action<bool, bool, bool, bool, bool>>) (x => new Action<bool, bool, bool, bool, bool>(x.EnableIronsightCallback)), enable, newKeyPress, changeCamera, hideCrosshairWhenAiming, forceChange);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.EnableIronsightCallback(enable, newKeyPress, changeCamera, hideCrosshairWhenAiming, true);
    }

    public void Shoot(MyShootActionEnum action, Vector3 shootDirection) => this.m_currentWeapon.Shoot(action, shootDirection, new Vector3D?());

    [Event(null, 5692)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    public void EnableIronsightCallback(
      bool enable,
      bool newKeyPress,
      bool changeCamera,
      bool hideCrosshairWhenAiming = true,
      bool forceChangeCamera = false)
    {
      if (enable)
      {
        if (this.m_currentWeapon == null || this.ZoomMode == MyZoomModeEnum.IronSight)
          return;
        this.ZoomMode = MyZoomModeEnum.IronSight;
        if (!changeCamera || !(MyEventContext.Current.IsLocallyInvoked | forceChangeCamera))
          return;
        float headLocalXangle = this.m_headLocalXAngle;
        float headLocalYangle = this.m_headLocalYAngle;
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
        this.m_headLocalXAngle = headLocalXangle;
        this.m_headLocalYAngle = headLocalYangle;
        if (MySession.Static.LocalCharacter != this)
          return;
        if (hideCrosshairWhenAiming)
          MyHud.Crosshair.HideDefaultSprite();
        MySector.MainCamera.Zoom.SetZoom(MyCameraZoomOperationType.ZoomingIn);
      }
      else
      {
        this.ZoomMode = MyZoomModeEnum.Classic;
        this.ForceFirstPersonCamera = false;
        if (!changeCamera || !(MyEventContext.Current.IsLocallyInvoked | forceChangeCamera))
          return;
        if (MySession.Static.LocalCharacter == this)
        {
          MyHud.Crosshair.ResetToDefault();
          MySector.MainCamera.Zoom.SetZoom(MyCameraZoomOperationType.ZoomingOut);
        }
        float headLocalXangle = this.m_headLocalXAngle;
        float headLocalYangle = this.m_headLocalYAngle;
        this.m_headLocalXAngle = headLocalXangle;
        this.m_headLocalYAngle = headLocalYangle;
      }
    }

    private void SwitchCameraIronSightChanges()
    {
      this.m_wasInThirdPersonBeforeIronSight = false;
      if (this.ZoomMode != MyZoomModeEnum.IronSight)
        return;
      if (this.m_isInFirstPersonView)
        MyHud.Crosshair.HideDefaultSprite();
      else
        MyHud.Crosshair.ResetToDefault();
    }

    public static IMyHandheldGunObject<MyDeviceBase> CreateGun(
      MyObjectBuilder_EntityBase gunEntity,
      uint? inventoryItemId = null)
    {
      if (gunEntity == null)
        return (IMyHandheldGunObject<MyDeviceBase>) null;
      MyEntity entity = MyEntityFactory.CreateEntity((MyObjectBuilder_Base) gunEntity);
      try
      {
        entity.Init(gunEntity);
      }
      catch (Exception ex)
      {
        return (IMyHandheldGunObject<MyDeviceBase>) null;
      }
      IMyHandheldGunObject<MyDeviceBase> handheldGunObject = (IMyHandheldGunObject<MyDeviceBase>) entity;
      if (handheldGunObject != null && handheldGunObject.GunBase != null && (!handheldGunObject.GunBase.InventoryItemId.HasValue && inventoryItemId.HasValue))
        handheldGunObject.GunBase.InventoryItemId = new uint?(inventoryItemId.Value);
      return handheldGunObject;
    }

    public MyPhysicalInventoryItem? FindWeaponItemByDefinition(
      MyDefinitionId weaponDefinition)
    {
      MyPhysicalInventoryItem? nullable1 = new MyPhysicalInventoryItem?();
      MyDefinitionId? nullable2 = MyDefinitionManager.Static.ItemIdFromWeaponId(weaponDefinition);
      if (nullable2.HasValue && MyEntityExtensions.GetInventory(this) != null)
        nullable1 = MyEntityExtensions.GetInventory(this).FindUsableItem(nullable2.Value);
      return nullable1;
    }

    private void SaveAmmoToWeapon()
    {
    }

    public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition) => (!weaponDefinition.HasValue || !(weaponDefinition.Value.TypeId == typeof (MyObjectBuilder_CubePlacer)) || MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Building)) && !this.IsOnLadder && (!this.WeaponTakesBuilderFromInventory(weaponDefinition) || this.FindWeaponItemByDefinition(weaponDefinition.Value).HasValue);

    public bool WeaponTakesBuilderFromInventory(MyDefinitionId? weaponDefinition) => weaponDefinition.HasValue && !(weaponDefinition.Value.TypeId == typeof (MyObjectBuilder_CubePlacer)) && (!(weaponDefinition.Value.TypeId == typeof (MyObjectBuilder_PhysicalGunObject)) || !(weaponDefinition.Value.SubtypeId == this.manipulationToolId)) && !MySession.Static.CreativeMode && !MyFakes.ENABLE_SURVIVAL_SWITCHING;

    public void SwitchToWeapon(MyDefinitionId weaponDefinition) => this.SwitchToWeapon(new MyDefinitionId?(weaponDefinition));

    public void SwitchAmmoMagazine() => this.SwitchAmmoMagazineInternal(true);

    public bool CanSwitchAmmoMagazine() => this.m_currentWeapon != null && this.m_currentWeapon.GunBase != null && this.m_currentWeapon.GunBase.CanSwitchAmmoMagazine();

    private void SwitchAmmoMagazineInternal(bool sync)
    {
      if (sync)
      {
        MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.OnSwitchAmmoMagazineRequest)));
      }
      else
      {
        if (this.IsDead || this.CurrentWeapon == null)
          return;
        this.CurrentWeapon.GunBase.SwitchAmmoMagazineToNextAvailable();
      }
    }

    private void SwitchAmmoMagazineSuccess() => this.SwitchAmmoMagazineInternal(false);

    public void SwitchToWeapon(MyDefinitionId? weaponDefinition, bool sync = true)
    {
      if (weaponDefinition.HasValue && this.m_rightHandItemBone == -1)
        return;
      if (this.WeaponTakesBuilderFromInventory(weaponDefinition))
      {
        MyPhysicalInventoryItem? itemByDefinition = this.FindWeaponItemByDefinition(weaponDefinition.Value);
        if (!itemByDefinition.HasValue)
          return;
        if (itemByDefinition.Value.Content == null)
        {
          MySandboxGame.Log.WriteLine("item.Value.Content was null in MyCharacter.SwitchToWeapon");
          MySandboxGame.Log.WriteLine("item.Value = " + (object) itemByDefinition.Value);
          MySandboxGame.Log.WriteLine("weaponDefinition.Value = " + (object) weaponDefinition);
        }
        else
          this.SwitchToWeaponInternal(weaponDefinition, sync, new uint?(itemByDefinition.Value.ItemId), 0L);
      }
      else
        this.SwitchToWeaponInternal(weaponDefinition, sync, new uint?(), 0L);
    }

    public void SwitchToWeapon(MyToolbarItemWeapon weapon) => this.SwitchToWeapon(weapon, new uint?());

    public void SwitchToWeapon(MyToolbarItemWeapon weapon, uint? inventoryItemId, bool sync = true)
    {
      MyDefinitionId? weaponDefinition = new MyDefinitionId?();
      if (weapon != null)
        weaponDefinition = new MyDefinitionId?(weapon.Definition.Id);
      if (weaponDefinition.HasValue && this.m_rightHandItemBone == -1)
        return;
      if (this.WeaponTakesBuilderFromInventory(weaponDefinition))
      {
        MyPhysicalInventoryItem? nullable1 = new MyPhysicalInventoryItem?();
        MyPhysicalInventoryItem? nullable2 = !inventoryItemId.HasValue ? this.FindWeaponItemByDefinition(weaponDefinition.Value) : MyEntityExtensions.GetInventory(this).GetItemByID(inventoryItemId.Value);
        if (!nullable2.HasValue)
          return;
        if (nullable2.Value.Content == null)
        {
          MySandboxGame.Log.WriteLine("item.Value.Content was null in MyCharacter.SwitchToWeapon");
          MySandboxGame.Log.WriteLine("item.Value = " + (object) nullable2.Value);
          MySandboxGame.Log.WriteLine("weaponDefinition.Value = " + (object) weaponDefinition);
        }
        else
          this.SwitchToWeaponInternal(weaponDefinition, sync, new uint?(nullable2.Value.ItemId), 0L);
      }
      else
        this.SwitchToWeaponInternal(weaponDefinition, sync, new uint?(), 0L);
    }

    private void SwitchToWeaponInternal(
      MyDefinitionId? weaponDefinition,
      bool updateSync,
      uint? inventoryItemId,
      long weaponEntityId)
    {
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
      {
        PerFrameData perFrameData = new PerFrameData();
        ref PerFrameData local1 = ref perFrameData;
        SwitchWeaponData switchWeaponData = new SwitchWeaponData();
        ref SwitchWeaponData local2 = ref switchWeaponData;
        MyDefinitionId? nullable1 = weaponDefinition;
        SerializableDefinitionId? nullable2 = nullable1.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) nullable1.GetValueOrDefault()) : new SerializableDefinitionId?();
        local2.WeaponDefinition = nullable2;
        switchWeaponData.InventoryItemId = inventoryItemId;
        switchWeaponData.WeaponEntityId = weaponEntityId;
        SwitchWeaponData? nullable3 = new SwitchWeaponData?(switchWeaponData);
        local1.SwitchWeaponData = nullable3;
        PerFrameData data = perFrameData;
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, data);
      }
      if (updateSync)
      {
        this.UnequipWeapon();
        this.RequestSwitchToWeapon(weaponDefinition, inventoryItemId);
      }
      else
      {
        this.UnequipWeapon();
        this.StopCurrentWeaponShooting();
        if (!weaponDefinition.HasValue || !(weaponDefinition.Value.TypeId != MyObjectBuilderType.Invalid))
          return;
        this.EquipWeapon(MyCharacter.CreateGun(this.GetObjectBuilderForWeapon(weaponDefinition, ref inventoryItemId, weaponEntityId), inventoryItemId));
        this.UpdateShadowIgnoredObjects();
      }
    }

    private MyObjectBuilder_EntityBase GetObjectBuilderForWeapon(
      MyDefinitionId? weaponDefinition,
      ref uint? inventoryItemId,
      long weaponEntityId)
    {
      builderEntityBase = (MyObjectBuilder_EntityBase) null;
      if (inventoryItemId.HasValue && (Sandbox.Game.Multiplayer.Sync.IsServer || this.ControllerInfo.IsLocallyControlled()))
      {
        MyPhysicalInventoryItem? itemById = MyEntityExtensions.GetInventory(this).GetItemByID(inventoryItemId.Value);
        if (itemById.HasValue)
        {
          if (itemById.Value.Content is MyObjectBuilder_PhysicalGunObject content)
            builderEntityBase = content.GunEntity;
          if (builderEntityBase == null)
          {
            MyHandItemDefinition itemForPhysicalItem = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(content.GetId());
            if (itemForPhysicalItem != null)
            {
              builderEntityBase = (MyObjectBuilder_EntityBase) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemForPhysicalItem.Id);
              builderEntityBase.EntityId = weaponEntityId;
            }
          }
          else
            builderEntityBase.EntityId = weaponEntityId;
          if (content != null)
            content.GunEntity = builderEntityBase;
        }
      }
      else
      {
        bool flag = !Sandbox.Game.Multiplayer.Sync.IsServer && this.ControllerInfo.IsRemotelyControlled() || !this.WeaponTakesBuilderFromInventory(weaponDefinition);
        if (!weaponDefinition.HasValue)
          this.EquipWeapon((IMyHandheldGunObject<MyDeviceBase>) null);
        else if (flag && weaponDefinition.Value.TypeId == typeof (MyObjectBuilder_PhysicalGunObject))
        {
          MyHandItemDefinition itemForPhysicalItem = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(weaponDefinition.Value);
          if (itemForPhysicalItem != null)
          {
            builderEntityBase = (MyObjectBuilder_EntityBase) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemForPhysicalItem.Id);
            builderEntityBase.EntityId = weaponEntityId;
          }
        }
        else if (MyObjectBuilderSerializer.CreateNewObject(weaponDefinition.Value.TypeId, weaponDefinition.Value.SubtypeName) is MyObjectBuilder_EntityBase builderEntityBase)
        {
          builderEntityBase.EntityId = weaponEntityId;
          if (this.WeaponTakesBuilderFromInventory(weaponDefinition))
          {
            MyPhysicalInventoryItem? itemByDefinition = this.FindWeaponItemByDefinition(weaponDefinition.Value);
            if (itemByDefinition.HasValue)
            {
              if (itemByDefinition.Value.Content is MyObjectBuilder_PhysicalGunObject content)
                content.GunEntity = builderEntityBase;
              inventoryItemId = new uint?(itemByDefinition.Value.ItemId);
            }
          }
        }
      }
      if (builderEntityBase != null && builderEntityBase is IMyObjectBuilder_GunObject<MyObjectBuilder_DeviceBase> builderGunObject && builderGunObject.DeviceBase != null)
        builderGunObject.DeviceBase.InventoryItemId = inventoryItemId;
      return builderEntityBase;
    }

    private void StopCurrentWeaponShooting()
    {
      if (this.m_currentWeapon == null)
        return;
      foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
      {
        if (this.IsShooting(action))
          this.m_currentWeapon.EndShoot(action);
      }
    }

    private void UpdateShadowIgnoredObjects()
    {
      if (this.Render == null)
        return;
      this.Render.UpdateShadowIgnoredObjects();
      if (this.m_currentWeapon != null)
        this.UpdateShadowIgnoredObjects((VRage.ModAPI.IMyEntity) this.m_currentWeapon);
      if (this.m_leftHandItem == null)
        return;
      this.UpdateShadowIgnoredObjects((VRage.ModAPI.IMyEntity) this.m_leftHandItem);
    }

    private void UpdateShadowIgnoredObjects(VRage.ModAPI.IMyEntity parent)
    {
      this.Render.UpdateShadowIgnoredObjects(parent);
      foreach (MyEntityComponentBase child in parent.Hierarchy.Children)
        this.UpdateShadowIgnoredObjects(child.Container.Entity);
    }

    public void Use()
    {
      if (this.IsOnLadder)
      {
        if (this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderOut)
          return;
        Vector3D pos = this.PositionComp.GetPosition() + this.m_ladder.WorldMatrix.Forward * 1.20000004768372;
        this.GetOffLadder();
        this.PositionComp.SetPosition(pos);
      }
      else
      {
        if (this.IsDead)
          return;
        MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
        if (detectorComponent != null && detectorComponent.UseObject != null)
        {
          if (detectorComponent.UseObject.PrimaryAction != UseActionEnum.None)
          {
            if (detectorComponent.UseObject.PlayIndicatorSound)
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
              this.SoundComp.StopStateSound();
            }
            detectorComponent.RaiseObjectUsed();
            detectorComponent.UseObject.Use(detectorComponent.UseObject.PrimaryAction, (VRage.ModAPI.IMyEntity) this);
          }
          else
          {
            if (detectorComponent.UseObject.SecondaryAction == UseActionEnum.None)
              return;
            if (detectorComponent.UseObject.PlayIndicatorSound)
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
              this.SoundComp.StopStateSound();
            }
            detectorComponent.RaiseObjectUsed();
            detectorComponent.UseObject.Use(detectorComponent.UseObject.SecondaryAction, (VRage.ModAPI.IMyEntity) this);
          }
        }
        else
        {
          if (!(detectorComponent.DetectedEntity is MyEntity detectedEntity) || detectedEntity is MyCharacter && !(detectedEntity as MyCharacter).IsDead)
            return;
          MyInventoryBase inventoryBase = (MyInventoryBase) null;
          if (!detectedEntity.TryGetInventory(out inventoryBase))
            return;
          this.ShowAggregateInventoryScreen(inventoryBase);
        }
      }
    }

    public void UseContinues()
    {
      if (this.IsDead)
        return;
      MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent == null || detectorComponent.UseObject == null || (!detectorComponent.UseObject.IsActionSupported(UseActionEnum.Manipulate) || !detectorComponent.UseObject.ContinuousUsage))
        return;
      detectorComponent.UseObject.Use(UseActionEnum.Manipulate, (VRage.ModAPI.IMyEntity) this);
    }

    public void UseTerminal()
    {
      if (this.IsDead)
        return;
      MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent.UseObject == null || !detectorComponent.UseObject.IsActionSupported(UseActionEnum.OpenTerminal))
        return;
      if (detectorComponent.UseObject.PlayIndicatorSound)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
        this.SoundComp.StopStateSound();
      }
      detectorComponent.UseObject.Use(UseActionEnum.OpenTerminal, (VRage.ModAPI.IMyEntity) this);
      detectorComponent.UseContinues();
    }

    public void UseFinished()
    {
      if (this.IsDead)
        return;
      MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent.UseObject == null || !detectorComponent.UseObject.IsActionSupported(UseActionEnum.UseFinished))
        return;
      detectorComponent.UseObject.Use(UseActionEnum.UseFinished, (VRage.ModAPI.IMyEntity) this);
    }

    public void PickUp()
    {
      if (this.IsDead)
        return;
      this.Components.Get<MyCharacterPickupComponent>()?.PickUp();
    }

    public void PickUpContinues()
    {
      if (this.IsDead)
        return;
      this.Components.Get<MyCharacterPickupComponent>()?.PickUpContinues();
    }

    public void PickUpFinished()
    {
      if (this.IsDead)
        return;
      this.Components.Get<MyCharacterPickupComponent>()?.PickUpFinished();
    }

    private bool HasEnoughSpaceToStandUp()
    {
      if (!this.IsCrouching)
        return true;
      Vector3D from = this.WorldMatrix.Translation + (double) this.Definition.CharacterCollisionCrouchHeight * this.WorldMatrix.Up;
      return !MyPhysics.CastRay(from, from + (double) (this.Definition.CharacterCollisionHeight - this.Definition.CharacterCollisionCrouchHeight) * this.WorldMatrix.Up, 18).HasValue;
    }

    public void Crouch()
    {
      if (this.IsDead || !this.Definition.CanCrouch || (this.JetpackRunning || this.m_isFalling) || !this.HasEnoughSpaceToStandUp())
        return;
      this.WantsCrouch = !this.WantsCrouch;
    }

    public void Down()
    {
      if (this.WantsFlyUp)
      {
        this.WantsFlyDown = false;
        this.WantsFlyUp = false;
      }
      else
        this.WantsFlyDown = true;
    }

    public void Up()
    {
      if (this.WantsFlyDown)
      {
        this.WantsFlyUp = false;
        this.WantsFlyDown = false;
      }
      else
        this.WantsFlyUp = true;
    }

    public void Sprint(bool enabled)
    {
      if (this.WantsSprint == enabled)
        return;
      this.WantsSprint = enabled && this.ZoomMode != MyZoomModeEnum.IronSight;
    }

    public void SwitchWalk() => this.WantsWalk = !this.WantsWalk;

    [Event(null, 6335)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    public void Jump(Vector3 moveIndicator)
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died || !this.HasEnoughSpaceToStandUp())
        return;
      MyTuple<ushort, MyStringHash> message;
      if (this.StatComp != null && !this.StatComp.CanDoAction(nameof (Jump), out message, this.m_currentMovementState == MyCharacterMovementEnum.Jump))
      {
        if (MySession.Static == null || MySession.Static.LocalCharacter != this || (message.Item1 != (ushort) 4 || message.Item2.String.CompareTo("Stamina") != 0) || this.m_notEnoughStatNotification == null)
          return;
        this.m_notEnoughStatNotification.SetTextFormatArguments((object) message.Item2);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_notEnoughStatNotification);
      }
      else if (this.IsMagneticBootsActive)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsClientPredicted)
          this.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(1000f * this.Physics.SupportNormal), new Vector3D?(), new Vector3?(), new float?(), true, false);
        this.StartFalling();
      }
      else if (this.IsOnLadder)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.GetOffLadder();
        else
          this.GetOffLadder_Implementation();
        if (!Sandbox.Game.Multiplayer.Sync.IsServer && !this.IsClientPredicted)
          return;
        Vector3 jumpDirection = (Vector3) this.WorldMatrix.Backward;
        if ((double) this.MoveIndicator.X > 0.0)
          jumpDirection = (Vector3) this.WorldMatrix.Right;
        else if ((double) this.MoveIndicator.X < 0.0)
          jumpDirection = (Vector3) this.WorldMatrix.Left;
        MySandboxGame.Static.Invoke((Action) (() => this.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(1000f * jumpDirection), new Vector3D?(), new Vector3?(), new float?(), true, false)), "Ladder jump");
      }
      else
        this.WantsJump = true;
    }

    public void ShowInventory()
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        return;
      MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent.UseObject != null && detectorComponent.UseObject.IsActionSupported(UseActionEnum.OpenInventory))
        detectorComponent.UseObject.Use(UseActionEnum.OpenInventory, (VRage.ModAPI.IMyEntity) this);
      else if (MyPerGameSettings.TerminalEnabled)
      {
        MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, this, (MyEntity) null);
      }
      else
      {
        if (!this.HasInventory)
          return;
        MyInventory inventory = MyEntityExtensions.GetInventory(this);
        if (inventory == null)
          return;
        this.ShowAggregateInventoryScreen((MyInventoryBase) inventory);
      }
    }

    public MyGuiScreenBase ShowAggregateInventoryScreen(
      MyInventoryBase rightSelectedInventory = null)
    {
      if (MyPerGameSettings.GUI.InventoryScreen != (System.Type) null && this.InventoryAggregate != null)
      {
        this.InventoryAggregate.Init();
        this.m_InventoryScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.InventoryScreen, (object) this.InventoryAggregate, (object) rightSelectedInventory);
        MyGuiSandbox.AddScreen(this.m_InventoryScreen);
        this.m_InventoryScreen.Closed += (MyGuiScreenBase.ScreenHandler) ((scr, isUnloading) =>
        {
          if (this.InventoryAggregate != null)
            this.InventoryAggregate.DetachCallbacks();
          this.m_InventoryScreen = (MyGuiScreenBase) null;
        });
      }
      return this.m_InventoryScreen;
    }

    public void ShowTerminal()
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        return;
      MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
      if (MyToolbarComponent.CharacterToolbar != null && MyToolbarComponent.CharacterToolbar.SelectedItem is MyToolbarItemVoxelHand)
        return;
      if (detectorComponent.UseObject != null && detectorComponent.UseObject.IsActionSupported(UseActionEnum.OpenTerminal))
        detectorComponent.UseObject.Use(UseActionEnum.OpenTerminal, (VRage.ModAPI.IMyEntity) this);
      else if (MyPerGameSettings.TerminalEnabled)
        MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, this, (MyEntity) null);
      else if (MyFakes.ENABLE_QUICK_WARDROBE)
      {
        MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenWardrobe(this));
      }
      else
      {
        if (!(MyPerGameSettings.GUI.GameplayOptionsScreen != (System.Type) null) || MySession.Static.SurvivalMode)
          return;
        MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.GameplayOptionsScreen));
      }
    }

    public void SwitchLights()
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        return;
      this.EnableLights(!this.LightEnabled);
      this.RecalculatePowerRequirement();
    }

    public void SwitchReactors()
    {
    }

    public void SwitchBroadcasting()
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died || !(bool) this.m_enableBroadcastingPlayerToggle)
        return;
      this.RequestEnableBroadcasting(!this.RadioBroadcaster.WantsToBeEnabled);
    }

    public override void OnRemovedFromScene(object source)
    {
      MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
      if (highlightSystem != null)
        ((IEnumerable<uint>) this.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id => highlightSystem.RemoveHighlightOverlappingModel(id)));
      base.OnRemovedFromScene(source);
      if (this.m_currentWeapon != null)
      {
        if (highlightSystem != null)
          ((IEnumerable<uint>) ((MyEntity) this.m_currentWeapon).Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id => highlightSystem.RemoveHighlightOverlappingModel(id)));
        ((MyEntity) this.m_currentWeapon).OnRemovedFromScene(source);
      }
      if (this.m_leftHandItem != null)
        this.m_leftHandItem.OnRemovedFromScene(source);
      this.m_resolveHighlightOverlap = true;
    }

    public void RemoveNotification(ref MyHudNotification notification)
    {
      if (notification == null)
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) notification);
      notification = (MyHudNotification) null;
    }

    private void RemoveNotifications()
    {
      this.RemoveNotification(ref this.m_pickupObjectNotification);
      this.RemoveNotification(ref this.m_respawnNotification);
    }

    private void OnControlAcquired(MyEntityController controller)
    {
      MyPlayer player = controller.Player;
      this.m_idModule.Owner = player.Identity.IdentityId;
      this.SetPlayer(controller.Player);
      if (MyMultiplayer.Static != null && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.IsPersistenceCharacter = true;
      if (player.IsLocalPlayer)
      {
        if (player == MySession.Static.LocalHumanPlayer)
        {
          MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
          if (highlightSystem != null)
          {
            ((IEnumerable<uint>) this.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id => highlightSystem.AddHighlightOverlappingModel(id)));
            this.m_resolveHighlightOverlap = false;
          }
          MyHud.SetHudDefinition(this.Definition.HUD);
          MyHud.HideAll();
          MyHud.Crosshair.ResetToDefault();
          MyHud.Crosshair.Recenter();
          if (MyGuiScreenGamePlay.Static != null)
            MySession.Static.CameraAttachedToChanged += new Action<IMyCameraController, IMyCameraController>(this.Static_CameraAttachedToChanged);
          if (MySession.Static.CameraController is MyEntity && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
            MySession.Static.SetCameraController(this.IsInFirstPersonView ? MyCameraControllerEnum.Entity : MyCameraControllerEnum.ThirdPersonSpectator, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
          MyHud.GravityIndicator.Entity = (MyEntity) this;
          MyHud.GravityIndicator.Show((Action<MyHudGravityIndicator>) null);
          MyHud.OreMarkers.Visible = true;
          MyHud.LargeTurretTargets.Visible = true;
          if (MySession.Static.IsScenario)
            MyHud.ScenarioInfo.Show((Action<MyHudScenarioInfo>) null);
        }
        MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
        jetpackComp?.TurnOnJetpack(jetpackComp.TurnedOn);
        this.m_suitBattery.OwnedByLocalPlayer = true;
        this.DisplayName = player.Identity.DisplayName;
      }
      else
      {
        this.DisplayName = player.Identity.DisplayName;
        this.UpdateHudMarker();
      }
      if (this.StatComp != null && this.StatComp.Health != null && (double) this.StatComp.Health.Value <= 0.0)
      {
        this.m_dieAfterSimulation = true;
      }
      else
      {
        if (this.m_currentWeapon != null)
          this.m_currentWeapon.OnControlAcquired(this);
        this.UpdateCharacterPhysics();
        if (this != MySession.Static.ControlledEntity || MyToolbarComponent.CharacterToolbar == null)
          return;
        MyToolbarComponent.CharacterToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
        MyToolbarComponent.CharacterToolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      }
    }

    private void UpdateHudMarker()
    {
      if (MyFakes.ENABLE_RADIO_HUD)
        return;
      MyHud.LocationMarkers.RegisterMarker((MyEntity) this, new MyHudEntityParams()
      {
        FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_TEXT,
        Text = new StringBuilder(this.GetIdentity().DisplayName),
        ShouldDraw = new Func<bool>(MyHud.CheckShowPlayerNamesOnHud)
      });
    }

    public StringBuilder UpdateCustomNameWithFaction()
    {
      this.CustomNameWithFaction.Clear();
      MyIdentity identity = this.GetIdentity();
      if (identity == null)
      {
        this.CustomNameWithFaction.Append(this.DisplayName);
      }
      else
      {
        IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(identity.IdentityId);
        if (playerFaction != null)
        {
          this.CustomNameWithFaction.Append(playerFaction.Tag);
          this.CustomNameWithFaction.Append('.');
        }
        this.CustomNameWithFaction.Append(identity.DisplayName);
      }
      return this.CustomNameWithFaction;
    }

    internal void ClearShapeContactPoints() => this.m_shapeContactPoints.Clear();

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      this.UpdateCustomNameWithFaction();
      this.m_hudParams.Clear();
      if (MySession.Static.LocalHumanPlayer == null)
        return this.m_hudParams;
      this.m_hudParams.Add(new MyHudEntityParams()
      {
        FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_TEXT,
        Text = this.CustomNameWithFaction,
        ShouldDraw = new Func<bool>(MyHud.CheckShowPlayerNamesOnHud),
        Owner = this.GetPlayerIdentityId(),
        Share = MyOwnershipShareModeEnum.Faction,
        Entity = (VRage.ModAPI.IMyEntity) this
      });
      return this.m_hudParams;
    }

    private void OnControlReleased(MyEntityController controller)
    {
      if (!this.Closed)
        this.Static_CameraAttachedToChanged((IMyCameraController) null, (IMyCameraController) null);
      if (MySession.Static.LocalHumanPlayer == controller.Player)
      {
        MyHud.SelectedObjectHighlight.RemoveHighlight();
        this.RemoveNotifications();
        if (MyGuiScreenGamePlay.Static != null)
          MySession.Static.CameraAttachedToChanged -= new Action<IMyCameraController, IMyCameraController>(this.Static_CameraAttachedToChanged);
        MyHud.GravityIndicator.Hide();
        MyHud.LargeTurretTargets.Visible = false;
        MyHud.OreMarkers.Visible = false;
        if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
          MyGuiScreenGamePlay.ActiveGameplayScreen.CloseScreen();
        MyCubeBuilder.Static.Deactivate();
        if (!this.Closed)
        {
          this.RadioReceiver.Clear();
          this.ResetMovement();
          this.m_suitBattery.OwnedByLocalPlayer = false;
        }
      }
      else if (!MyFakes.ENABLE_RADIO_HUD)
        MyHud.LocationMarkers.UnregisterMarker((MyEntity) this);
      MyToolbarComponent.CharacterToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      if (this.Closed)
        return;
      this.SoundComp.StopStateSound();
    }

    private void Static_CameraAttachedToChanged(
      IMyCameraController oldController,
      IMyCameraController newController)
    {
      if (this.Closed)
        return;
      if (oldController != newController && MySession.Static.ControlledEntity == this && newController != this)
      {
        this.ResetMovement();
        this.EndShootAll();
      }
      this.UpdateNearFlag();
      if (!this.Render.NearFlag && MySector.MainCamera != null || oldController == newController)
        return;
      this.ResetHeadRotation();
    }

    public event MyCharacter.ControlStateChanged OnControlStateChanged;

    public void OnAssumeControl(IMyCameraController previousCameraController)
    {
      MyCharacter.ControlStateChanged controlStateChanged = this.OnControlStateChanged;
      if (controlStateChanged == null)
        return;
      controlStateChanged(true);
    }

    public void OnReleaseControl(IMyCameraController newCameraController)
    {
      MyCharacter.ControlStateChanged controlStateChanged = this.OnControlStateChanged;
      if (controlStateChanged == null)
        return;
      controlStateChanged(false);
    }

    public void ResetHeadRotation()
    {
      if (this.m_actualUpdateFrame <= 0UL)
        return;
      this.m_headLocalYAngle = 0.0f;
      this.m_headLocalXAngle = 0.0f;
    }

    private void UpdateNearFlag()
    {
      bool flag = (this.ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == this && (this.m_isInFirstPerson || this.ForceFirstPersonCamera) && !this.IsOnLadder) & this.CurrentMovementState != MyCharacterMovementEnum.Sitting;
      if (this.m_currentWeapon != null)
        ((MyEntity) this.m_currentWeapon).Render.NearFlag = flag;
      if (this.m_leftHandItem != null)
        this.m_leftHandItem.Render.NearFlag = flag;
      this.Render.NearFlag = flag;
      this.m_bobQueue.Clear();
    }

    private void WorldPositionChanged(object source)
    {
      if (this.RadioBroadcaster != null)
        this.RadioBroadcaster.MoveBroadcaster();
      this.Render.UpdateLightPosition();
    }

    private void OnCharacterStateChanged(HkCharacterStateType newState)
    {
      if (this.m_currentCharacterState == newState)
        return;
      if (this.m_currentMovementState != MyCharacterMovementEnum.Died)
      {
        if (!this.JetpackRunning && !this.IsOnLadder)
        {
          if ((double) this.m_currentJumpTime <= 0.0 && newState == HkCharacterStateType.HK_CHARACTER_IN_AIR || newState == (HkCharacterStateType) 5)
            this.StartFalling();
          else if (this.m_isFalling)
            this.StopFalling();
        }
        if (this.JetpackRunning)
          this.m_currentJumpTime = 0.0f;
      }
      this.m_currentCharacterState = newState;
    }

    internal void StartFalling()
    {
      if (this.JetpackRunning || this.m_currentMovementState == MyCharacterMovementEnum.Died || this.m_currentMovementState == MyCharacterMovementEnum.Sitting)
        return;
      this.m_currentFallingTime = this.m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_JUMPING ? 0.0f : -1f;
      this.m_isFalling = true;
      this.m_crouchAfterFall = this.WantsCrouch;
      this.WantsCrouch = false;
      this.SetCurrentMovementState(MyCharacterMovementEnum.Falling);
    }

    internal void StopFalling()
    {
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        return;
      MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
      if (this.m_isFalling && (jetpackComp == null || !jetpackComp.TurnedOn || !jetpackComp.IsPowered))
        this.SoundComp.PlayFallSound();
      if (this.m_isFalling)
      {
        this.m_forceStandingFootprints = true;
        this.m_movementsFlagsChanged = true;
      }
      this.m_isFalling = false;
      this.m_isFallingAnimationPlayed = false;
      this.m_currentFallingTime = 0.0f;
      this.m_currentJumpTime = 0.0f;
      this.m_canJump = true;
      this.WantsCrouch = this.m_crouchAfterFall;
      this.m_crouchAfterFall = false;
    }

    public bool CanStartConstruction(MyCubeBlockDefinition blockDefinition)
    {
      if (blockDefinition == null)
        return false;
      MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory((MyEntity) this);
      return builderInventory != null && builderInventory.GetItemAmount(blockDefinition.Components[0].Definition.Id) >= (MyFixedPoint) 1;
    }

    public bool CanStartConstruction(Dictionary<MyDefinitionId, int> constructionCost)
    {
      MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory((MyEntity) this);
      foreach (KeyValuePair<MyDefinitionId, int> keyValuePair in constructionCost)
      {
        if (builderInventory.GetItemAmount(keyValuePair.Key) < (MyFixedPoint) keyValuePair.Value)
          return false;
      }
      return true;
    }

    public MyEntity IsUsing
    {
      get => this.m_usingEntity;
      set => this.m_usingEntity = value;
    }

    [Event(null, 6946)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    public void UnequipWeapon()
    {
      for (int index = 0; index < this.m_isShooting.Length; ++index)
        this.m_isShooting[index] = false;
      if (this.m_leftHandItem != null && this.m_leftHandItem is IMyHandheldGunObject<MyDeviceBase>)
      {
        (this.m_leftHandItem as IMyHandheldGunObject<MyDeviceBase>).OnControlReleased();
        this.m_leftHandItem.Close();
        this.m_leftHandItem = (MyEntity) null;
        this.TriggerCharacterAnimationEvent("unequip_left_tool", this == MySession.Static.LocalCharacter);
      }
      if (this.m_currentWeapon != null)
      {
        if (this.ControllerInfo.IsLocallyControlled())
        {
          MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
          if (highlightSystem != null)
          {
            MyEntity mCurrentWeapon = (MyEntity) this.m_currentWeapon;
            ((IEnumerable<uint>) mCurrentWeapon.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id =>
            {
              if (id == uint.MaxValue)
                return;
              highlightSystem.RemoveHighlightOverlappingModel(id);
            }));
            if (mCurrentWeapon.Subparts != null)
            {
              foreach (KeyValuePair<string, MyEntitySubpart> subpart in mCurrentWeapon.Subparts)
                ((IEnumerable<uint>) subpart.Value.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id =>
                {
                  if (id == uint.MaxValue)
                    return;
                  highlightSystem.RemoveHighlightOverlappingModel(id);
                }));
            }
          }
        }
        if (MySession.Static.LocalCharacter == this && !MyInput.Static.IsGameControlPressed(MyControlsSpace.PRIMARY_TOOL_ACTION) && !MyInput.Static.IsGameControlPressed(MyControlsSpace.SECONDARY_TOOL_ACTION))
          this.EndShootAll();
        else if (Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          foreach (MyShootActionEnum action in MyEnum<MyShootActionEnum>.Values)
          {
            if (this.IsShooting(action))
              this.m_currentWeapon.EndShoot(action);
          }
        }
        MyEntity mCurrentWeapon1 = this.m_currentWeapon as MyEntity;
        if (this.UseNewAnimationSystem && this.m_handItemDefinition != null && !string.IsNullOrEmpty(this.m_handItemDefinition.Id.SubtypeName))
        {
          this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.Id.TypeId.ToString().ToLower()), 0.0f);
          this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.Id.SubtypeName.ToLower()), 0.0f);
          this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.WeaponType.ToString().ToLower()), 0.0f);
        }
        this.SaveAmmoToWeapon();
        this.m_currentWeapon.OnControlReleased();
        if (this.ZoomMode == MyZoomModeEnum.IronSight)
        {
          bool inFirstPersonView = this.IsInFirstPersonView;
          this.EnableIronsight(false, true, MySession.Static.CameraController == this);
          this.IsInFirstPersonView = inFirstPersonView;
        }
        MyResourceSinkComponent sink = mCurrentWeapon1.Components.Get<MyResourceSinkComponent>();
        if (sink != null)
          this.SuitRechargeDistributor.RemoveSink(sink);
        mCurrentWeapon1.SetFadeOut(false);
        mCurrentWeapon1.OnClose -= new Action<MyEntity>(this.gunEntity_OnClose);
        Sandbox.Game.Entities.MyEntities.Remove(mCurrentWeapon1);
        mCurrentWeapon1.Close();
        this.m_currentWeapon = (IMyHandheldGunObject<MyDeviceBase>) null;
        if (this.ControllerInfo.IsLocallyHumanControlled() && MySector.MainCamera != null)
          MySector.MainCamera.Zoom.ResetZoom();
        if (this.UseNewAnimationSystem)
        {
          bool sync = this == MySession.Static.LocalCharacter;
          this.TriggerCharacterAnimationEvent("unequip_left_tool", sync);
          this.TriggerCharacterAnimationEvent("unequip_right_tool", sync);
        }
        else
        {
          this.StopUpperAnimation(0.2f);
          this.SwitchAnimation(this.m_currentMovementState, false);
        }
        this.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdShooting, 0.0f);
        if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
        {
          if (this.AnimationController.Entity is IMySkinnedEntity entity)
            entity.UpdateControl(0.0f);
          this.AnimationController.Update();
        }
      }
      if ((double) this.m_currentShotTime <= 0.0)
      {
        this.StopUpperAnimation(0.0f);
        this.StopFingersAnimation(0.0f);
      }
      this.m_currentWeapon = (IMyHandheldGunObject<MyDeviceBase>) null;
      this.StopFingersAnimation(0.0f);
    }

    private void EquipWeapon(IMyHandheldGunObject<MyDeviceBase> newWeapon, bool showNotification = false)
    {
      if (newWeapon == null)
        return;
      MyEntity myEntity = (MyEntity) newWeapon;
      myEntity.Render.CastShadows = true;
      myEntity.Render.NeedsResolveCastShadow = false;
      myEntity.Save = false;
      myEntity.OnClose += new Action<MyEntity>(this.gunEntity_OnClose);
      MyAssetModifierComponent modifierComponent = new MyAssetModifierComponent();
      myEntity.Components.Add<MyAssetModifierComponent>(modifierComponent);
      Sandbox.Game.Entities.MyEntities.Add(myEntity);
      if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.SavedCharacters.Contains<MyCharacter>(this))
        MyLocalCache.LoadInventoryConfig(this, myEntity, modifierComponent);
      this.m_handItemDefinition = (MyHandItemDefinition) null;
      this.m_currentWeapon = newWeapon;
      this.m_currentWeapon.OnControlAcquired(this);
      ((MyEntity) this.m_currentWeapon).Render.DrawInAllCascades = true;
      if (this.ControllerInfo.IsLocallyControlled())
      {
        MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
        if (this.m_currentWeapon != null && highlightSystem != null)
        {
          MyEntity mCurrentWeapon = (MyEntity) this.m_currentWeapon;
          ((IEnumerable<uint>) mCurrentWeapon.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id =>
          {
            if (id == uint.MaxValue)
              return;
            highlightSystem.AddHighlightOverlappingModel(id);
          }));
          if (mCurrentWeapon.Subparts != null)
          {
            foreach (KeyValuePair<string, MyEntitySubpart> subpart in mCurrentWeapon.Subparts)
              ((IEnumerable<uint>) subpart.Value.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id =>
              {
                if (id == uint.MaxValue)
                  return;
                highlightSystem.AddHighlightOverlappingModel(id);
              }));
          }
        }
      }
      if (this.WeaponEquiped != null)
        this.WeaponEquiped(this.m_currentWeapon);
      if (MyVisualScriptLogicProvider.ToolEquipped != null && this.ControllerInfo != null)
      {
        long controllingIdentityId = this.ControllerInfo.ControllingIdentityId;
        MyVisualScriptLogicProvider.ToolEquipped(controllingIdentityId, newWeapon.DefinitionId.TypeId.ToString(), newWeapon.DefinitionId.SubtypeName);
      }
      if (this.m_currentWeapon.PhysicalObject != null)
        this.m_handItemDefinition = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(this.m_currentWeapon.PhysicalObject.GetId());
      else if (this.m_currentWeapon.DefinitionId.TypeId == typeof (MyObjectBuilder_CubePlacer))
      {
        MyDefinitionId id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer));
        this.m_handItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref id);
      }
      if (this.m_handItemDefinition != null && !string.IsNullOrEmpty(this.m_handItemDefinition.FingersAnimation))
      {
        string fingersAnimation;
        if (!this.m_characterDefinition.AnimationNameToSubtypeName.TryGetValue(this.m_handItemDefinition.FingersAnimation, out fingersAnimation))
          fingersAnimation = this.m_handItemDefinition.FingersAnimation;
        MyAnimationDefinition animationDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(fingersAnimation);
        if (!animationDefinition.LeftHandItem.TypeId.IsNull)
        {
          this.m_currentWeapon.OnControlReleased();
          (this.m_currentWeapon as MyEntity).Close();
          this.m_currentWeapon = (IMyHandheldGunObject<MyDeviceBase>) null;
        }
        this.PlayCharacterAnimation(this.m_handItemDefinition.FingersAnimation, MyBlendOption.Immediate, animationDefinition.Loop ? MyFrameOption.Loop : MyFrameOption.PlayOnce, 1f);
        if (this.UseNewAnimationSystem)
        {
          bool sync = this == MySession.Static.LocalCharacter;
          this.TriggerCharacterAnimationEvent("equip_left_tool", sync);
          this.TriggerCharacterAnimationEvent("equip_right_tool", sync);
          this.TriggerCharacterAnimationEvent(this.m_handItemDefinition.Id.SubtypeName.ToLower(), sync);
          this.TriggerCharacterAnimationEvent(this.m_handItemDefinition.FingersAnimation.ToLower(), sync);
          if (!string.IsNullOrEmpty(this.m_handItemDefinition.Id.SubtypeName))
          {
            this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.Id.TypeId.ToString().ToLower()), 1f);
            this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.Id.SubtypeName.ToLower()), 1f);
            this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.WeaponType.ToString().ToLower()), 1f);
          }
        }
        if (!animationDefinition.LeftHandItem.TypeId.IsNull)
        {
          if (this.m_leftHandItem != null)
          {
            (this.m_leftHandItem as IMyHandheldGunObject<MyDeviceBase>).OnControlReleased();
            this.m_leftHandItem.Close();
          }
          long weaponEntityId = MyEntityIdentifier.AllocateId();
          uint? inventoryItemId = new uint?();
          IMyHandheldGunObject<MyDeviceBase> gun = MyCharacter.CreateGun(this.GetObjectBuilderForWeapon(new MyDefinitionId?(animationDefinition.LeftHandItem), ref inventoryItemId, weaponEntityId), inventoryItemId);
          if (gun != null)
          {
            this.m_leftHandItem = gun as MyEntity;
            this.m_leftHandItem.Render.DrawInAllCascades = true;
            gun.OnControlAcquired(this);
            this.UpdateLeftHandItemPosition();
            Sandbox.Game.Entities.MyEntities.Add(this.m_leftHandItem);
          }
        }
      }
      else if (this.m_handItemDefinition != null)
      {
        if (this.UseNewAnimationSystem)
        {
          bool sync = this == MySession.Static.LocalCharacter;
          this.TriggerCharacterAnimationEvent("equip_left_tool", sync);
          this.TriggerCharacterAnimationEvent("equip_right_tool", sync);
          this.TriggerCharacterAnimationEvent(this.m_handItemDefinition.Id.SubtypeName.ToLower(), sync);
          if (!string.IsNullOrEmpty(this.m_handItemDefinition.Id.SubtypeName))
          {
            this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.Id.TypeId.ToString().ToLower()), 1f);
            this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.Id.SubtypeName.ToLower()), 1f);
            this.AnimationController.Variables.SetValue(MyStringId.GetOrCompute(this.m_handItemDefinition.WeaponType.ToString().ToLower()), 1f);
          }
        }
      }
      else
        this.StopFingersAnimation(0.0f);
      MyResourceSinkComponent sink = myEntity.Components.Get<MyResourceSinkComponent>();
      if (sink != null && this.SuitRechargeDistributor != null)
        this.SuitRechargeDistributor.AddSink(sink);
      if (showNotification)
      {
        MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationUsingWeaponType, 2000);
        myHudNotification.SetTextFormatArguments((object) MyDeviceBase.GetGunNotificationName(newWeapon.DefinitionId));
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      this.Static_CameraAttachedToChanged((IMyCameraController) null, (IMyCameraController) null);
      if (this.IsUsing is MyCockpit)
        return;
      MyHud.Crosshair.ResetToDefault(false);
    }

    private void gunEntity_OnClose(MyEntity obj)
    {
      if (this.m_currentWeapon != obj)
        return;
      this.m_currentWeapon = (IMyHandheldGunObject<MyDeviceBase>) null;
    }

    public float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    private void SetPowerInput(float input)
    {
      if (this.LightEnabled && (double) input >= 1.99999999495049E-06)
      {
        this.m_lightPowerFromProducer = 2E-06f;
        input -= 2E-06f;
      }
      else
        this.m_lightPowerFromProducer = 0.0f;
    }

    private float ComputeRequiredPower()
    {
      float num1 = 1E-05f;
      if (this.OxygenComponent != null && this.OxygenComponent.NeedsOxygenFromSuit)
        num1 = 1E-06f;
      if (this.m_lightEnabled)
        num1 += 2E-06f;
      float num2 = Math.Abs((float) (((double) this.GetOutsideTemperature() * 2.0 - 1.0) * ((double) this.Definition.SuitConsumptionInTemperatureExtreme / 100000.0)));
      return num1 + num2;
    }

    internal void RecalculatePowerRequirement(bool chargeImmediatelly = false)
    {
      this.SinkComp.Update();
      this.UpdateLightPower(chargeImmediatelly);
    }

    public bool LightEnabled => this.m_lightEnabled;

    public void EnableLights(bool enable)
    {
      MyMultiplayer.RaiseEvent<MyCharacter, bool>(this, (Func<MyCharacter, Action<bool>>) (x => new Action<bool>(x.EnableLightsCallback)), enable);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.EnableLightsCallback(enable);
    }

    [Event(null, 7331)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    private void EnableLightsCallback(bool enable)
    {
      if (this.m_lightEnabled == enable)
        return;
      this.m_lightEnabled = enable;
      this.RecalculatePowerRequirement();
      if (this.Render == null)
        return;
      this.Render.UpdateLightPosition();
    }

    public void EnableBroadcastingPlayerToggle(bool enable)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_enableBroadcastingPlayerToggle.Value = enable;
    }

    public void RequestEnableBroadcasting(bool enable)
    {
      if (!(bool) this.m_enableBroadcastingPlayerToggle)
        return;
      MyMultiplayer.RaiseEvent<MyCharacter, bool>(this, (Func<MyCharacter, Action<bool>>) (x => new Action<bool>(x.EnableBroadcasting)), enable);
    }

    [Event(null, 7360)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    public void EnableBroadcasting(bool enable)
    {
      if (!(bool) this.m_enableBroadcastingPlayerToggle && !MyEventContext.Current.IsLocallyInvoked)
        return;
      MyMultiplayer.RaiseEvent<MyCharacter, bool>(this, (Func<MyCharacter, Action<bool>>) (x => new Action<bool>(x.EnableBroadcastingCallback)), enable);
    }

    [Event(null, 7369)]
    [Reliable]
    [Server]
    [Broadcast]
    public void EnableBroadcastingCallback(bool enable) => this.EnableBroadcastingInternal(enable);

    private void EnableBroadcastingInternal(bool enable)
    {
      if (this.RadioBroadcaster == null || this.RadioBroadcaster.WantsToBeEnabled == enable)
        return;
      this.RadioBroadcaster.WantsToBeEnabled = enable;
      this.RadioBroadcaster.Enabled = enable;
    }

    public bool IsCrouching => this.m_currentMovementState.GetMode() == (ushort) 2;

    public bool IsSprinting => this.m_currentMovementState == MyCharacterMovementEnum.Sprinting;

    public bool IsFalling
    {
      get
      {
        MyCharacterMovementEnum currentMovementState = this.GetCurrentMovementState();
        return this.m_isFalling && currentMovementState != MyCharacterMovementEnum.Flying;
      }
    }

    public bool ShouldRecoilRotate
    {
      get
      {
        switch (this.GetCurrentMovementState())
        {
          case MyCharacterMovementEnum.Flying:
            return true;
          case MyCharacterMovementEnum.Falling:
            if (this.Physics != null && this.Physics.CharacterProxy != null)
              return (double) this.Physics.CharacterProxy.Gravity.LengthSquared() < 1.0 / 1000.0;
            break;
        }
        return false;
      }
    }

    public bool IsJumping => this.m_currentMovementState == MyCharacterMovementEnum.Jump;

    public bool IsMagneticBootsEnabled => !this.IsJumping && !this.IsOnLadder && (!this.IsFalling && this.Physics != null) && this.Physics.CharacterProxy != null && (double) this.Physics.CharacterProxy.Gravity.LengthSquared() < 1.0 / 1000.0 && !this.JetpackRunning;

    public bool IsMagneticBootsActive => this.IsMagneticBootsEnabled && (MyBootsState) this.m_bootsState == MyBootsState.Enabled;

    public void Sit(bool enableFirstPerson, bool playerIsPilot, bool enableBag, string animation)
    {
      this.EndShootAll();
      this.SwitchToWeaponInternal(new MyDefinitionId?(), false, new uint?(), 0L);
      this.Render.NearFlag = false;
      this.m_isFalling = false;
      this.PlayCharacterAnimation(animation, MyBlendOption.Immediate, MyFrameOption.Loop, 0.0f);
      this.StopUpperCharacterAnimation(0.0f);
      this.StopFingersAnimation(0.0f);
      this.SetHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0.0f)));
      this.SetUpperHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0.0f)));
      if (this.UseNewAnimationSystem)
        this.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdLean, 0.0f);
      this.SetSpineAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, 0.0f), Quaternion.CreateFromAxisAngle(Vector3.Forward, 0.0f));
      this.SetHeadAdditionalRotation(Quaternion.Identity, false);
      this.FlushAnimationQueue();
      this.SinkComp.Update();
      this.UpdateLightPower(true);
      this.EnableBag(enableBag);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_bootsState.Value = MyBootsState.Init;
      this.SetCurrentMovementState(MyCharacterMovementEnum.Sitting);
      if (!Sandbox.Engine.Platform.Game.IsDedicated && this.UseNewAnimationSystem)
      {
        this.TriggerCharacterAnimationEvent("sit", false);
        if (!string.IsNullOrEmpty(animation))
        {
          string eventName = string.Empty;
          if (!this.Definition.AnimationNameToSubtypeName.TryGetValue(animation, out eventName))
            eventName = animation;
          this.TriggerCharacterAnimationEvent(eventName, false);
        }
      }
      this.UpdateAnimation(0.0f);
    }

    public void EnableBag(bool enabled)
    {
      this.m_enableBag = enabled;
      if (!this.InScene || this.Render.RenderObjectIDs[0] == uint.MaxValue)
        return;
      MyRenderProxy.UpdateModelProperties(this.Render.RenderObjectIDs[0], "Backpack", enabled ? RenderFlags.Visible : RenderFlags.SkipInDepth, enabled ? RenderFlags.SkipInDepth : RenderFlags.Visible, new Color?(), new float?());
    }

    public void EnableHead(bool enabled)
    {
      if (!this.InScene || this.m_headRenderingEnabled == enabled)
        return;
      this.UpdateHeadModelProperties(enabled);
    }

    private void UpdateHeadModelProperties(bool enabled)
    {
      if (this.m_characterDefinition.MaterialsDisabledIn1st == null)
        return;
      this.m_headRenderingEnabled = enabled;
      if (this.Render.RenderObjectIDs[0] == uint.MaxValue)
        return;
      foreach (string materialName in this.m_characterDefinition.MaterialsDisabledIn1st)
        MyRenderProxy.UpdateModelProperties(this.Render.RenderObjectIDs[0], materialName, enabled ? RenderFlags.Visible : (RenderFlags) 0, enabled ? (RenderFlags) 0 : RenderFlags.Visible, new Color?(), new float?());
    }

    public void Stand()
    {
      this.PlayCharacterAnimation("Idle", MyBlendOption.Immediate, MyFrameOption.Loop, 0.0f);
      this.Render.NearFlag = false;
      this.StopUpperCharacterAnimation(0.0f);
      this.RecalculatePowerRequirement();
      this.EnableBag(true);
      this.UpdateHeadModelProperties(this.m_headRenderingEnabled);
      this.SetCurrentMovementState(MyCharacterMovementEnum.Standing);
      this.m_wasInFirstPerson = false;
      this.IsUsing = (MyEntity) null;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_bootsState.Value = MyBootsState.Init;
      if (this.Physics.CharacterProxy == null)
        return;
      this.Physics.CharacterProxy.Stand();
    }

    public void ForceUpdateBreath()
    {
      if (this.m_breath == null)
        return;
      this.m_breath.ForceUpdate();
    }

    public long GetPlayerIdentityId() => this.m_idModule.Owner;

    private MyPlayer TryGetPlayer() => this.ControllerInfo.Controller?.Player;

    public MyIdentity GetIdentity()
    {
      MyPlayer player = this.TryGetPlayer();
      return player != null ? player.Identity : MySession.Static.Players.TryGetIdentity(this.GetPlayerIdentityId());
    }

    public MyPlayer.PlayerId GetClientIdentity()
    {
      MyPlayer player = this.TryGetPlayer();
      if (player != null)
        return player.Id;
      MyPlayer.PlayerId result;
      MySession.Static.Players.TryGetPlayerId(this.GetPlayerIdentityId(), out result);
      return result;
    }

    public bool DoDamage(float damage, MyStringHash damageType, bool updateSync, long attackerId = 0)
    {
      damage *= damageType == MyDamageType.Suicide ? 1f : this.CharacterGeneralDamageModifier;
      damage *= damageType == MyDamageType.Environment ? MySession.Static.Settings.EnvironmentDamageMultiplier : 1f;
      if ((double) damage < 0.0 || (!(damageType != MyDamageType.Suicide) || !(damageType != MyDamageType.Asphyxia) || !(damageType != MyDamageType.LowPressure) ? 0 : (damageType != MyDamageType.Temperature ? 1 : 0)) != 0 && !MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Damage))
        return false;
      MyPlayer.PlayerId clientIdentity = this.GetClientIdentity();
      AdminSettingsEnum adminSettingsEnum;
      if (clientIdentity.SerialId == 0 && MySession.Static.RemoteAdminSettings.TryGetValue(clientIdentity.SteamId, out adminSettingsEnum) && (adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.Invulnerable) && damageType != MyDamageType.Suicide))
        return false;
      if (damageType != MyDamageType.Suicide && this.ControllerInfo.IsLocallyControlled() && (MySession.Static.CameraController == this && (double) MyCharacter.MAX_SHAKE_DAMAGE > 0.0))
        MySector.MainCamera.CameraShake.AddShake(MySector.MainCamera.CameraShake.MaxShake * MathHelper.Clamp(damage, 0.0f, MyCharacter.MAX_SHAKE_DAMAGE) / MyCharacter.MAX_SHAKE_DAMAGE);
      if (updateSync)
        this.TriggerCharacterAnimationEvent("hurt", true);
      else
        this.AnimationController.TriggerAction(MyAnimationVariableStorageHints.StrIdActionHurt);
      if (!this.CharacterCanDie && (!(damageType == MyDamageType.Suicide) || !MyPerGameSettings.CharacterSuicideEnabled) || this.StatComp == null)
        return false;
      MyPlayer playerFromCharacter = MyPlayer.GetPlayerFromCharacter(this);
      MyPlayer myPlayer = (MyPlayer) null;
      MyEntity entity;
      if (damageType != MyDamageType.Suicide && Sandbox.Game.Entities.MyEntities.TryGetEntityById(attackerId, out entity))
      {
        if (entity == this)
          return false;
        switch (entity)
        {
          case MyCharacter _:
            myPlayer = MyPlayer.GetPlayerFromCharacter(entity as MyCharacter);
            break;
          case IMyGunBaseUser _:
            myPlayer = MyPlayer.GetPlayerFromWeapon(entity as IMyGunBaseUser);
            break;
          case MyHandDrill _:
            myPlayer = MyPlayer.GetPlayerFromCharacter((entity as MyHandDrill).Owner);
            break;
        }
        if (playerFromCharacter != null && myPlayer != null && (MySession.Static.Factions.TryGetPlayerFaction(playerFromCharacter.Identity.IdentityId) is MyFaction playerFaction && !playerFaction.EnableFriendlyFire) && playerFaction.IsMember(myPlayer.Identity.IdentityId))
          return false;
        if ((double) damage >= 0.0 && MySession.Static != null && MyMusicController.Static != null)
        {
          if (this == MySession.Static.LocalCharacter)
          {
            switch (entity)
            {
              case MyVoxelPhysics _:
              case MyCubeGrid _:
                break;
              default:
                MyMusicController.Static.Fighting(false, (int) damage * 3);
                goto label_30;
            }
          }
          if (entity == MySession.Static.LocalCharacter)
            MyMusicController.Static.Fighting(false, (int) damage * 2);
          else if (entity is IMyGunBaseUser && (entity as IMyGunBaseUser).Owner as MyCharacter == MySession.Static.LocalCharacter)
            MyMusicController.Static.Fighting(false, (int) damage * 2);
          else if (MySession.Static.ControlledEntity == entity)
            MyMusicController.Static.Fighting(false, (int) damage);
        }
      }
label_30:
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.SIMULATE;
      MyDamageInformation info = new MyDamageInformation(false, damage, damageType, attackerId);
      if (this.UseDamageSystem && !this.m_dieAfterSimulation && !this.IsDead)
        MyDamageSystem.Static.RaiseBeforeDamageApplied((object) this, ref info);
      if ((double) info.Amount <= 0.0)
        return false;
      this.StatComp.DoDamage(damage, (object) info);
      bool flag = false;
      if (playerFromCharacter != null && (double) this.StatComp.Health.Value - (double) this.StatComp.Health.MinValue < 1.0 / 1000.0)
      {
        MyIdentity playerIdentity1 = MySession.Static.Players.TryGetPlayerIdentity(playerFromCharacter.Id);
        if (myPlayer != null)
        {
          MyIdentity playerIdentity2 = MySession.Static.Players.TryGetPlayerIdentity(myPlayer.Id);
          if (playerIdentity1 != null && playerIdentity2 != null)
          {
            if (Sandbox.Game.Multiplayer.Sync.IsServer && playerIdentity2.IdentityId != playerIdentity1.IdentityId)
              MySession.Static.Factions.DamageFactionPlayerReputation(playerIdentity2.IdentityId, playerIdentity1.IdentityId, MyReputationDamageType.Killing);
            if (MySession.Static.Factions.PlayerKilledByPlayer != null)
              MySession.Static.Factions.PlayerKilledByPlayer(playerIdentity1.IdentityId, playerIdentity2.IdentityId);
            flag = true;
          }
        }
        if (!flag && playerIdentity1 != null && MySession.Static.Factions.PlayerKilledByUnknown != null)
          MySession.Static.Factions.PlayerKilledByUnknown.InvokeIfNotNull<long>(playerIdentity1.IdentityId);
      }
      MySpaceAnalytics.Instance.SetLastDamageInformation(info);
      if (this.UseDamageSystem)
        MyDamageSystem.Static.RaiseAfterDamageApplied((object) this, info);
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
      if (!(customdata is MyCharacterHitInfo characterHitInfo) || characterHitInfo.BoneIndex == -1)
        return;
      MyDecalRenderInfo renderInfo = new MyDecalRenderInfo()
      {
        Position = (Vector3D) characterHitInfo.Triangle.IntersectionPointInObjectSpace,
        Normal = characterHitInfo.Triangle.NormalInObjectSpace,
        RenderObjectIds = this.Render.RenderObjectIDs,
        Source = source,
        Forward = forwardDirection,
        IsTrail = isTrail
      };
      renderInfo.PhysicalMaterial = physicalMaterial.GetHashCode() != 0 ? physicalMaterial : MyStringHash.GetOrCompute(this.m_characterDefinition.PhysicalMaterial);
      renderInfo.VoxelMaterial = voxelMaterial;
      VertexBoneIndicesWeights? boneIndicesWeights = characterHitInfo.Triangle.GetAffectingBoneIndicesWeights(ref MyCharacter.m_boneIndexWeightTmp);
      renderInfo.BoneIndices = boneIndicesWeights.Value.Indices;
      renderInfo.BoneWeights = boneIndicesWeights.Value.Weights;
      ref MyDecalRenderInfo local1 = ref renderInfo;
      MyDecalBindingInfo decalBindingInfo = new MyDecalBindingInfo();
      decalBindingInfo.Position = (Vector3D) characterHitInfo.HitPositionBindingPose;
      decalBindingInfo.Normal = characterHitInfo.HitNormalBindingPose;
      ref MyDecalBindingInfo local2 = ref decalBindingInfo;
      Matrix bindingTransformation = characterHitInfo.BindingTransformation;
      MatrixD matrixD = (MatrixD) ref bindingTransformation;
      local2.Transformation = matrixD;
      MyDecalBindingInfo? nullable = new MyDecalBindingInfo?(decalBindingInfo);
      local1.Binding = nullable;
      MyCharacter.m_tmpIds.Clear();
      decalHandler.AddDecal(ref renderInfo, MyCharacter.m_tmpIds);
      foreach (uint tmpId in MyCharacter.m_tmpIds)
        this.AddBoneDecal(tmpId, characterHitInfo.BoneIndex);
    }

    void IMyCharacter.Kill(object statChangeData)
    {
      MyDamageInformation damageInfo = new MyDamageInformation();
      if (statChangeData != null)
        damageInfo = (MyDamageInformation) statChangeData;
      this.Kill(true, damageInfo);
    }

    void IMyCharacter.TriggerCharacterAnimationEvent(string eventName, bool sync) => this.TriggerCharacterAnimationEvent(eventName, sync);

    private Action<MyCharacter> GetDelegate(Action<IMyCharacter> value) => (Action<MyCharacter>) Delegate.CreateDelegate(typeof (Action<MyCharacter>), value.Target, value.Method);

    event Action<IMyCharacter> IMyCharacter.CharacterDied
    {
      add => this.CharacterDied += this.GetDelegate(value);
      remove => this.CharacterDied -= this.GetDelegate(value);
    }

    bool IMyCharacter.IsDead => this.IsDead;

    public void Kill(bool sync, MyDamageInformation damageInfo)
    {
      if (this.m_dieAfterSimulation || this.IsDead || MyFakes.DEVELOPMENT_PRESET && damageInfo.Type != MyDamageType.Suicide)
        return;
      if (sync)
      {
        this.KillCharacter(damageInfo);
      }
      else
      {
        if (this.UseDamageSystem)
          MyDamageSystem.Static.RaiseDestroyed((object) this, damageInfo);
        MySpaceAnalytics.Instance.SetLastDamageInformation(damageInfo);
        this.StatComp.LastDamage = damageInfo;
        this.m_dieAfterSimulation = true;
      }
    }

    public void Die()
    {
      if (!this.CharacterCanDie && !MyPerGameSettings.CharacterSuicideEnabled || this.IsDead)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextSuicide), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
      {
        if (retval != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.OnSuicideRequest)));
      })), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
    }

    [Event(null, 7863)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    private void OnSuicideRequest() => this.DoDamage(1000f, MyDamageType.Suicide, true, this.EntityId);

    private void DieInternal()
    {
      if (!this.CharacterCanDie && !MyPerGameSettings.CharacterSuicideEnabled)
        return;
      MyPlayer player = this.TryGetPlayer();
      if (player != null && player.IsImmortal)
        return;
      bool flag = MySession.Static.LocalCharacter == this;
      this.SoundComp.PlayDeathSound(this.StatComp != null ? this.StatComp.LastDamage.Type : MyStringHash.NullOrEmpty);
      if (this.UseNewAnimationSystem)
        this.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdDead, 1f);
      if (this.m_InventoryScreen != null)
        this.m_InventoryScreen.CloseScreen();
      if (this.StatComp != null && this.StatComp.Health != null)
        this.StatComp.Health.OnStatChanged -= new MyEntityStat.StatChangedDelegate(this.StatComp.OnHealthChanged);
      if (this.m_breath != null)
        this.m_breath.CurrentState = MyCharacterBreath.State.NoBreath;
      if (this.IsOnLadder)
        this.GetOffLadder();
      if (this.CurrentRemoteControl != null)
      {
        if (this.CurrentRemoteControl is MyRemoteControl currentRemoteControl)
          currentRemoteControl.ForceReleaseControl();
        else if (this.CurrentRemoteControl is MyLargeTurretBase currentRemoteControl)
          currentRemoteControl.ForceReleaseControl();
      }
      if (this.ControllerInfo != null && this.ControllerInfo.IsLocallyHumanControlled())
      {
        if (MyGuiScreenTerminal.IsOpen)
          MyGuiScreenTerminal.Hide();
        if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
        {
          MyGuiScreenGamePlay.ActiveGameplayScreen.CloseScreen();
          MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
        }
        if (MyGuiScreenGamePlay.TmpGameplayScreenHolder != null)
        {
          MyGuiScreenGamePlay.TmpGameplayScreenHolder.CloseScreen();
          MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
        }
        if (this.ControllerInfo.Controller != null)
          this.ControllerInfo.Controller.SaveCamera();
      }
      if (this.Parent is MyCockpit)
      {
        MyCockpit parent = this.Parent as MyCockpit;
        if (parent.Pilot == this)
          parent.RemovePilot();
      }
      if (MySession.Static.ControlledEntity is MyRemoteControl)
      {
        MyRemoteControl controlledEntity = MySession.Static.ControlledEntity as MyRemoteControl;
        if (controlledEntity.PreviousControlledEntity == this)
          controlledEntity.ForceReleaseControl();
      }
      if (MySession.Static.ControlledEntity is MyLargeTurretBase && MySession.Static.LocalCharacter == this)
        (MySession.Static.ControlledEntity as MyLargeTurretBase).ForceReleaseControl();
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
      {
        this.StartRespawn(0.1f);
      }
      else
      {
        ulong endpointId = 0;
        if (this.ControllerInfo.Controller != null && this.ControllerInfo.Controller.Player != null)
        {
          endpointId = this.ControllerInfo.Controller.Player.Id.SteamId;
          if (!MySession.Static.Cameras.TryGetCameraSettings(this.ControllerInfo.Controller.Player.Id, this.EntityId, this.ControllerInfo.Controller.ControlledEntity is MyCharacter && MySession.Static.LocalCharacter == this.ControllerInfo.Controller.ControlledEntity, out this.m_cameraSettingsWhenAlive) && this.ControllerInfo.IsLocallyHumanControlled())
            this.m_cameraSettingsWhenAlive = new MyEntityCameraSettings()
            {
              Distance = MyThirdPersonSpectator.Static.GetViewerDistance(),
              IsFirstPerson = this.IsInFirstPersonView,
              HeadAngle = new Vector2?(new Vector2(this.HeadLocalXAngle, this.HeadLocalYAngle))
            };
        }
        MySandboxGame.Log.WriteLine("Player character died. Id : " + EndpointId.Format(endpointId));
        this.m_deadPlayerIdentityId = this.GetPlayerIdentityId();
        this.IsUsing = (MyEntity) null;
        this.Save = false;
        this.m_isFalling = false;
        this.SetCurrentMovementState(MyCharacterMovementEnum.Died);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.UnequipWeapon)));
        this.StopUpperAnimation(0.5f);
        this.m_animationCommandsEnabled = true;
        if (this.m_isInFirstPerson)
          this.PlayCharacterAnimation("DiedFps", MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0.5f);
        else
          this.PlayCharacterAnimation("Died", MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0.5f);
        this.InitDeadBodyPhysics();
        this.StartRespawn(5f);
        this.m_currentLootingCounter = this.m_characterDefinition.LootingTime;
        if (flag)
          this.EnableLights(false);
        if (this.CharacterDied != null)
          this.CharacterDied(this);
        foreach (MyComponentBase component in (MyComponentContainer) this.Components)
        {
          if (component is MyCharacterComponent characterComponent)
            characterComponent.OnCharacterDead();
        }
        this.SoundComp.CharacterDied();
        this.JetpackComp = (MyCharacterJetpackComponent) null;
        if (!this.Components.Has<MyCharacterRagdollComponent>())
          this.SyncFlag = true;
        Action<MyCharacter> onCharacterDied = MyCharacter.OnCharacterDied;
        if (onCharacterDied == null)
          return;
        onCharacterDied(this);
      }
    }

    private void StartRespawn(float respawnTime)
    {
      MyPlayer player = this.TryGetPlayer();
      if (player != null)
      {
        if (MyVisualScriptLogicProvider.PlayerDied != null && !this.IsBot)
          MyVisualScriptLogicProvider.PlayerDied(player.Identity.IdentityId);
        if (MyVisualScriptLogicProvider.NPCDied != null && this.IsBot)
          MyVisualScriptLogicProvider.NPCDied(this.DefinitionId.HasValue ? this.DefinitionId.Value.SubtypeName : "");
      }
      if ((double) this.m_currentRespawnCounter == -1.0)
        return;
      if (MySession.Static != null && this == MySession.Static.ControlledEntity)
      {
        MyGuiScreenTerminal.Hide();
        this.m_respawnNotification = new MyHudNotification(MyCommonTexts.NotificationRespawn, 5000, priority: 5);
        this.m_respawnNotification.Level = MyNotificationLevel.Important;
        this.m_respawnNotification.SetTextFormatArguments((object) (int) this.m_currentRespawnCounter);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_respawnNotification);
      }
      this.m_currentRespawnCounter = respawnTime;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    internal void DeactivateRespawn() => this.m_currentRespawnCounter = -1f;

    private void InitDeadBodyPhysics()
    {
      Vector3 vector3 = Vector3.Zero;
      this.EnableBag(false);
      this.RadioBroadcaster.BroadcastRadius = 5f;
      if (this.Physics != null)
      {
        vector3 = this.Physics.LinearVelocity;
        this.Physics.Enabled = false;
        this.Physics.Close();
        this.Physics = (MyPhysicsBody) null;
      }
      if (this.m_deathLinearVelocityFromSever.HasValue)
        vector3 = this.m_deathLinearVelocityFromSever.Value;
      HkMassProperties hkMassProperties = new HkMassProperties();
      hkMassProperties.Mass = 500f;
      int collisionFilter1 = !Sandbox.Game.Multiplayer.Sync.IsDedicated || !MyFakes.ENABLE_RAGDOLL || MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC ? 23 : 19;
      if (this.Definition.DeadBodyShape != null)
      {
        HkBoxShape hkBoxShape = new HkBoxShape(this.PositionComp.LocalAABB.HalfExtents * (Vector3) this.Definition.DeadBodyShape.BoxShapeScale);
        HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(hkBoxShape.HalfExtents, hkMassProperties.Mass);
        volumeMassProperties.CenterOfMass = hkBoxShape.HalfExtents * (Vector3) this.Definition.DeadBodyShape.RelativeCenterOfMass;
        HkShape shape = (HkShape) hkBoxShape;
        this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_DEFAULT);
        Vector3D position = (Vector3D) (this.PositionComp.LocalAABB.HalfExtents * (Vector3) this.Definition.DeadBodyShape.RelativeShapeTranslation);
        MatrixD translation = MatrixD.CreateTranslation(position);
        this.Physics.CreateFromCollisionObject(shape, (Vector3) (this.PositionComp.LocalVolume.Center + position), translation, new HkMassProperties?(volumeMassProperties), collisionFilter1);
        this.Physics.Friction = this.Definition.DeadBodyShape.Friction;
        this.Physics.RigidBody.MaxAngularVelocity = 1.570796f;
        this.Physics.LinearVelocity = vector3;
        shape.RemoveReference();
        this.Physics.Enabled = true;
      }
      else
      {
        BoundingBox localAabb = this.PositionComp.LocalAABB;
        Vector3 halfExtents = localAabb.HalfExtents;
        halfExtents.X *= 0.7f;
        halfExtents.Z *= 0.7f;
        HkBoxShape hkBoxShape = new HkBoxShape(halfExtents);
        HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(hkBoxShape.HalfExtents, hkMassProperties.Mass);
        volumeMassProperties.CenterOfMass = new Vector3(halfExtents.X, 0.0f, 0.0f);
        HkShape hkShape = (HkShape) hkBoxShape;
        this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_DEFAULT);
        MyPhysicsBody physics = this.Physics;
        HkShape shape = hkShape;
        localAabb = this.PositionComp.LocalAABB;
        Vector3 center = localAabb.Center;
        MatrixD identity = MatrixD.Identity;
        HkMassProperties? massProperties = new HkMassProperties?(volumeMassProperties);
        int collisionFilter2 = collisionFilter1;
        physics.CreateFromCollisionObject(shape, center, identity, massProperties, collisionFilter2);
        this.Physics.Friction = 0.5f;
        this.Physics.RigidBody.MaxAngularVelocity = 1.570796f;
        this.Physics.LinearVelocity = vector3;
        hkShape.RemoveReference();
        this.Physics.Enabled = true;
      }
      HkMassChangerUtil.Create(this.Physics.RigidBody, 65536, 1f, 0.0f);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      if (this.m_needReconnectLadder)
      {
        if (this.m_ladder != null)
        {
          this.ReconnectConstraint(this.m_oldLadderGrid, this.m_ladder.CubeGrid);
          if ((HkReferenceObject) this.m_constraintInstance != (HkReferenceObject) null)
            this.SetCharacterLadderConstraint(this.WorldMatrix);
        }
        this.m_needReconnectLadder = false;
        this.m_oldLadderGrid = (MyCubeGrid) null;
      }
      this.RecalculatePowerRequirement(true);
      MyEntityStat myEntityStat = this.StatComp != null ? this.StatComp.Health : (MyEntityStat) null;
      if (myEntityStat != null)
      {
        if (this.m_savedHealth.HasValue)
          myEntityStat.Value = this.m_savedHealth.Value;
        myEntityStat.OnStatChanged += new MyEntityStat.StatChangedDelegate(this.StatComp.OnHealthChanged);
      }
      if (this.m_breath != null)
        this.m_breath.ForceUpdate();
      if (this.m_currentMovementState == MyCharacterMovementEnum.Died)
        this.Physics.ForceActivate();
      base.UpdateOnceBeforeFrame();
      if (this.m_currentWeapon != null)
      {
        Sandbox.Game.Entities.MyEntities.Remove((MyEntity) this.m_currentWeapon);
        this.EquipWeapon(this.m_currentWeapon);
      }
      MyPlayer.PlayerId playerId;
      if (this.ControllerInfo.Controller == null && this.GetPlayerId(out playerId) && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_controlInfo.Value = playerId;
      if (this.m_relativeDampeningEntityInit != 0L)
        this.RelativeDampeningEntity = Sandbox.Game.Entities.MyEntities.GetEntityByIdOrDefault(this.m_relativeDampeningEntityInit);
      if (this.m_ladderIdInit.HasValue)
      {
        if (Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_ladderIdInit.Value) is MyLadder)
        {
          this.GetOnLadder_Implementation(this.m_ladderIdInit.Value, false, this.m_ladderInfoInit);
          this.m_ladderIdInit = new long?();
          this.m_ladderInfoInit = new MyObjectBuilder_Character.LadderInfo?();
        }
        else
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
      this.UpdateAssetModifiers();
    }

    private void UpdateAssetModifiers()
    {
      if (this.m_assetModifiersLoaded || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalHumanPlayer == null)
        return;
      long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
      long playerIdentityId = this.GetPlayerIdentityId();
      if (playerIdentityId == identityId)
      {
        if (this.IsDead || this.IsBot)
          return;
        MyLocalCache.LoadInventoryConfig(this, false);
        this.m_assetModifiersLoaded = true;
      }
      else
      {
        if (playerIdentityId == -1L)
          return;
        MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyCharacter.RefreshAssetModifiers)), playerIdentityId, this.EntityId);
        this.m_assetModifiersLoaded = true;
      }
    }

    [Event(null, 8302)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RefreshAssetModifiers(long playerId, long entityId)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalHumanPlayer == null || (MySession.Static.LocalHumanPlayer.Identity.IdentityId != playerId || MyGameService.InventoryItems == null))
        return;
      List<MyGameInventoryItem> items = new List<MyGameInventoryItem>();
      foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
      {
        if (inventoryItem.UsingCharacters.Contains(entityId))
          items.Add(inventoryItem);
      }
      MyGameService.GetItemsCheckData(items, (Action<byte[]>) (checkDataResult =>
      {
        if (checkDataResult == null)
          return;
        MyMultiplayer.RaiseStaticEvent<long, byte[]>((Func<IMyEventOwner, Action<long, byte[]>>) (x => new Action<long, byte[]>(MyCharacter.SendSkinData)), entityId, checkDataResult);
      }));
    }

    [Event(null, 8340)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SendSkinData(long entityId, byte[] checkDataResult)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !(Sandbox.Game.Entities.MyEntities.GetEntityById(entityId) is MyCharacter entityById))
        return;
      if (entityById.Components.TryGet<MyAssetModifierComponent>(out MyAssetModifierComponent _))
        MyAssetModifierComponent.ApplyAssetModifierSync(entityId, checkDataResult, true);
      if (!(entityById.CurrentWeapon is MyEntity currentWeapon) || entityById.CurrentWeapon == null || !entityById.CurrentWeapon.IsSkinnable)
        return;
      MyAssetModifierComponent.ApplyAssetModifierSync(currentWeapon.EntityId, checkDataResult, true);
    }

    public long DeadPlayerIdentityId => this.m_deadPlayerIdentityId;

    public Vector3 ColorMask
    {
      get => base.Render.ColorMaskHsv;
      set => this.ChangeModelAndColor(this.ModelName, value);
    }

    public string ModelName
    {
      get => this.m_characterModel;
      set => this.ChangeModelAndColor(value, this.ColorMask);
    }

    public IMyHandheldGunObject<MyDeviceBase> CurrentWeapon => this.m_currentWeapon;

    public IMyHandheldGunObject<MyDeviceBase> LeftHandItem => this.m_leftHandItem as IMyHandheldGunObject<MyDeviceBase>;

    internal Sandbox.Game.Entities.IMyControllableEntity CurrentRemoteControl { get; set; }

    public MyBattery SuitBattery => this.m_suitBattery;

    public override string DisplayNameText => this.DisplayName;

    public static bool CharactersCanDie => !MySession.Static.CreativeMode || MyFakes.CHARACTER_CAN_DIE_EVEN_IN_CREATIVE_MODE;

    public bool CharacterCanDie
    {
      get
      {
        if (MyCharacter.CharactersCanDie)
          return true;
        return this.ControllerInfo.Controller != null && (uint) this.ControllerInfo.Controller.Player.Id.SerialId > 0U;
      }
    }

    public override Vector3D LocationForHudMarker => base.LocationForHudMarker + this.WorldMatrix.Up * 2.1;

    public MyPhysicsBody Physics
    {
      get => base.Physics as MyPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public void SetLocalHeadAnimation(float? targetX, float? targetY, float length)
    {
      if ((double) length > 0.0)
      {
        if ((double) this.m_headLocalYAngle < 0.0)
        {
          this.m_headLocalYAngle = -this.m_headLocalYAngle;
          this.m_headLocalYAngle = (float) (((double) this.m_headLocalYAngle + 180.0) % 360.0 - 180.0);
          this.m_headLocalYAngle = -this.m_headLocalYAngle;
        }
        else
          this.m_headLocalYAngle = (float) (((double) this.m_headLocalYAngle + 180.0) % 360.0 - 180.0);
      }
      this.m_currentLocalHeadAnimation = 0.0f;
      this.m_localHeadAnimationLength = length;
      this.m_localHeadAnimationX = !targetX.HasValue ? new Vector2?() : new Vector2?(new Vector2(this.m_headLocalXAngle, targetX.Value));
      if (targetY.HasValue)
        this.m_localHeadAnimationY = new Vector2?(new Vector2(this.m_headLocalYAngle, targetY.Value));
      else
        this.m_localHeadAnimationY = new Vector2?();
    }

    public bool IsLocalHeadAnimationInProgress() => (double) this.m_currentLocalHeadAnimation >= 0.0;

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      int num = !(this.IsUsing is MyCockpit) ? 0 : (!(this.IsUsing as MyCockpit).BlockDefinition.EnableFirstPerson ? 1 : 0);
      if (this.m_currentMovementState == MyCharacterMovementEnum.Sitting)
        this.EnableBag(this.m_enableBag);
      if (this.m_currentWeapon != null)
      {
        Sandbox.Game.Entities.MyEntities.Remove((MyEntity) this.m_currentWeapon);
        Sandbox.Game.Entities.MyEntities.Add((MyEntity) this.m_currentWeapon);
      }
      this.UpdateShadowIgnoredObjects();
      if (this.Render != null)
        this.Render.UpdateLight(this.m_currentLightPower, true, MyCharacter.LIGHT_PARAMETERS_CHANGED);
      MyPlayerCollection.UpdateControl((MyEntity) this);
    }

    public static MyCharacter CreateCharacter(
      MatrixD worldMatrix,
      Vector3 velocity,
      string characterName,
      string model,
      Vector3? colorMask,
      MyBotDefinition botDefinition,
      bool findNearPos = true,
      bool AIMode = false,
      MyCockpit cockpit = null,
      bool useInventory = true,
      long identityId = 0,
      bool addDefaultItems = true)
    {
      Vector3D? nullable = new Vector3D?();
      if (findNearPos)
      {
        nullable = Sandbox.Game.Entities.MyEntities.FindFreePlace(worldMatrix.Translation, 2f, 200, stepSize: 0.5f);
        if (!nullable.HasValue)
          nullable = Sandbox.Game.Entities.MyEntities.FindFreePlace(worldMatrix.Translation, 2f, 200, stepSize: 5f);
      }
      if (nullable.HasValue)
        worldMatrix.Translation = nullable.Value;
      return MyCharacter.CreateCharacterBase(worldMatrix, ref velocity, characterName, model, colorMask, AIMode, useInventory, botDefinition, identityId, addDefaultItems);
    }

    private static MyCharacter CreateCharacterBase(
      MatrixD worldMatrix,
      ref Vector3 velocity,
      string characterName,
      string model,
      Vector3? colorMask,
      bool AIMode,
      bool useInventory = true,
      MyBotDefinition botDefinition = null,
      long identityId = 0,
      bool addDefaultItems = true)
    {
      MyCharacter myCharacter = new MyCharacter();
      MyObjectBuilder_Character builderCharacter = MyCharacter.Random();
      builderCharacter.CharacterModel = model ?? builderCharacter.CharacterModel;
      if (colorMask.HasValue)
        builderCharacter.ColorMaskHSV = (SerializableVector3) colorMask.Value;
      builderCharacter.JetpackEnabled = MySession.Static.CreativeMode;
      builderCharacter.Battery = new MyObjectBuilder_Battery()
      {
        CurrentCapacity = 1f
      };
      builderCharacter.AIMode = AIMode;
      builderCharacter.DisplayName = characterName;
      builderCharacter.LinearVelocity = (SerializableVector3) velocity;
      builderCharacter.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix));
      builderCharacter.CharacterGeneralDamageModifier = 1f;
      builderCharacter.OwningPlayerIdentityId = new long?(identityId);
      myCharacter.Init((MyObjectBuilder_EntityBase) builderCharacter);
      Sandbox.Game.Entities.MyEntities.RaiseEntityCreated((MyEntity) myCharacter);
      Sandbox.Game.Entities.MyEntities.Add((MyEntity) myCharacter);
      MyInventory inventory = MyEntityExtensions.GetInventory(myCharacter);
      if (useInventory)
      {
        if (inventory != null & addDefaultItems)
          MyWorldGenerator.InitInventoryWithDefaults(inventory);
      }
      else
        botDefinition?.AddItems(myCharacter);
      if ((double) velocity.Length() > 0.0)
        myCharacter.JetpackComp?.EnableDampeners(false);
      return myCharacter;
    }

    public override string ToString() => this.m_characterModel;

    public MyEntity Entity => (MyEntity) this;

    public MyControllerInfo ControllerInfo => this.m_info;

    public bool IsDead => this.m_currentMovementState == MyCharacterMovementEnum.Died;

    public bool IsSitting => this.m_currentMovementState == MyCharacterMovementEnum.Sitting;

    public void ShowOutOfAmmoNotification()
    {
      if (MyCharacter.OutOfAmmoNotification == null)
        MyCharacter.OutOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) MyCharacter.OutOfAmmoNotification);
    }

    public void UpdateCharacterPhysics() => this.UpdateCharacterPhysics(false);

    public void UpdateCharacterPhysics(bool forceUpdate = false)
    {
      if (this.Physics != null && !this.Physics.Enabled)
        return;
      float num1 = (float) (2.0 * (double) MyPerGameSettings.PhysicsConvexRadius + 0.0299999993294477);
      float maxSpeedRelativeToShip = Math.Max(this.Definition.MaxSprintSpeed, Math.Max(this.Definition.MaxRunSpeed, this.Definition.MaxBackrunSpeed));
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsClientPredicted && !this.ForceDisablePrediction)
      {
        if (((this.Physics == null ? 1 : (this.Physics.IsStatic ? 1 : 0)) | (forceUpdate ? 1 : 0)) != 0)
        {
          Vector3 vector3 = Vector3.Zero;
          if (this.Physics != null)
          {
            vector3 = this.Physics.LinearVelocityLocal;
            this.Physics.Close();
          }
          Vector3 center = new Vector3(0.0f, this.Definition.CharacterCollisionHeight / 2f, 0.0f);
          this.InitCharacterPhysics(VRage.Game.MyMaterialType.CHARACTER, center, this.Definition.CharacterCollisionWidth * this.Definition.CharacterCollisionScale, this.Definition.CharacterCollisionHeight - this.Definition.CharacterCollisionWidth * this.Definition.CharacterCollisionScale - num1, this.Definition.CharacterCollisionCrouchHeight - this.Definition.CharacterCollisionWidth, this.Definition.CharacterCollisionWidth - num1, this.Definition.CharacterHeadSize * this.Definition.CharacterCollisionScale, this.Definition.CharacterHeadHeight, 0.7f, 0.7f, (ushort) 18, RigidBodyFlag.RBF_DEFAULT, MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(this.Definition.Mass) : this.Definition.Mass, this.Definition.VerticalPositionFlyingOnly, this.Definition.MaxSlope, this.Definition.ImpulseLimit, maxSpeedRelativeToShip, false, this.Definition.MaxForce);
          if (this.Physics.CharacterProxy != null)
          {
            this.Physics.CharacterProxy.ContactPointCallback -= new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
            this.Physics.CharacterProxy.ContactPointCallbackEnabled = true;
            this.Physics.CharacterProxy.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
          }
          this.Physics.Enabled = true;
          this.Physics.LinearVelocity = vector3;
          this.UpdateCrouchState();
        }
      }
      else if (((this.Physics == null ? 1 : (!this.Physics.IsStatic ? 1 : 0)) | (forceUpdate ? 1 : 0)) != 0)
      {
        if (this.Physics != null)
          this.Physics.Close();
        float num2 = 1f;
        int num3 = 22;
        Vector3 center = new Vector3(0.0f, this.Definition.CharacterCollisionHeight / 2f, 0.0f);
        this.InitCharacterPhysics(VRage.Game.MyMaterialType.CHARACTER, center, this.Definition.CharacterCollisionWidth * this.Definition.CharacterCollisionScale * num2, this.Definition.CharacterCollisionHeight - this.Definition.CharacterCollisionWidth * this.Definition.CharacterCollisionScale * num2 - num1, this.Definition.CharacterCollisionCrouchHeight - this.Definition.CharacterCollisionWidth, this.Definition.CharacterCollisionWidth - num1, this.Definition.CharacterHeadSize * this.Definition.CharacterCollisionScale * num2, this.Definition.CharacterHeadHeight, 0.7f, 0.7f, (ushort) num3, RigidBodyFlag.RBF_STATIC, 0.0f, this.Definition.VerticalPositionFlyingOnly, this.Definition.MaxSlope, this.Definition.ImpulseLimit, maxSpeedRelativeToShip, true, this.Definition.MaxForce);
        this.Physics.Enabled = true;
      }
      this.Physics?.CharacterProxy?.GetHitRigidBody()?.SetProperty(253, 0.0f);
    }

    public void GetNetState(out MyCharacterClientState state)
    {
      state.HeadX = this.HeadLocalXAngle;
      state.HeadY = this.HeadLocalYAngle;
      state.MovementState = this.GetCurrentMovementState();
      state.MovementFlags = this.MovementFlags;
      bool flag = this.JetpackComp != null;
      state.Jetpack = flag && this.JetpackComp.TurnedOn;
      state.Dampeners = flag && this.JetpackComp.DampenersTurnedOn;
      state.TargetFromCamera = this.TargetFromCamera;
      state.MoveIndicator = this.MoveIndicator;
      MatrixD matrix = this.Entity.WorldMatrix;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      state.Rotation = fromRotationMatrix;
      state.CharacterState = this.m_currentCharacterState;
      state.SupportNormal = this.IsOnLadder ? this.m_ladderIncrementToBase : (this.Physics == null || this.Physics.CharacterProxy == null ? Vector3.Zero : this.Physics.CharacterProxy.SupportNormal);
      state.MovementSpeed = this.m_currentSpeed;
      state.MovementDirection = this.m_currentMovementDirection;
      state.IsOnLadder = this.IsOnLadder;
      state.Valid = true;
    }

    public void SetNetState(ref MyCharacterClientState state)
    {
      if (this.IsDead || this.IsUsing != null && !this.IsOnLadder || this.Closed)
        return;
      bool flag = this.ControllerInfo.IsLocallyControlled();
      if (Sandbox.Game.Multiplayer.Sync.IsServer || !flag)
      {
        if ((state.MovementState == MyCharacterMovementEnum.LadderUp || state.MovementState == MyCharacterMovementEnum.LadderOut) && (this.GetCurrentMovementState() == MyCharacterMovementEnum.Ladder && state.IsOnLadder))
          state.MoveIndicator.Z = -1f;
        if (state.MovementState == MyCharacterMovementEnum.LadderDown && this.GetCurrentMovementState() == MyCharacterMovementEnum.Ladder && state.IsOnLadder)
          state.MoveIndicator.Z = 1f;
        this.SetHeadLocalXAngle(state.HeadX);
        this.SetHeadLocalYAngle(state.HeadY);
        MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
        if (jetpackComp != null && !this.IsOnLadder)
        {
          if (state.Jetpack != this.JetpackComp.TurnedOn)
            jetpackComp.TurnOnJetpack(state.Jetpack, true);
          if (state.Dampeners != this.JetpackComp.DampenersTurnedOn)
            jetpackComp.EnableDampeners(state.Dampeners);
        }
        if (this.GetCurrentMovementState() != state.MovementState && state.MovementState == MyCharacterMovementEnum.LadderOut)
          this.TriggerCharacterAnimationEvent("LadderOut", false);
        if (this.IsOnLadder && state.IsOnLadder || !this.IsOnLadder && !state.IsOnLadder)
        {
          this.CacheMove(ref state.MoveIndicator, ref state.Rotation);
          if (this.IsOnLadder)
            this.m_ladderIncrementToBaseServer = state.SupportNormal;
        }
        this.MovementFlags = state.MovementFlags | this.MovementFlags & MyCharacterMovementFlags.Jump;
        if (!this.IsOnLadder && !this.WantsJump)
          this.SetCurrentMovementState(state.MovementState);
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.TargetFromCamera = state.TargetFromCamera;
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsClientPredicted && flag)
        return;
      if (this.m_previousMovementState == MyCharacterMovementEnum.Jump && state.CharacterState == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
        this.StopFalling();
      this.m_currentSpeed = state.MovementSpeed;
      this.m_currentMovementDirection = state.MovementDirection;
      this.OnCharacterStateChanged(state.CharacterState);
      if (this.IsOnLadder)
        return;
      this.Physics.SupportNormal = state.SupportNormal;
    }

    public void UpdateMovementAndFlags(
      MyCharacterMovementEnum movementState,
      MyCharacterMovementFlags flags)
    {
      if (this.m_currentMovementState == movementState || this.Physics == null)
        return;
      this.m_movementFlags = flags;
      this.SwitchAnimation(movementState);
      this.SetCurrentMovementState(movementState);
    }

    private void SwitchToWeaponSuccess(
      MyDefinitionId? weapon,
      uint? inventoryItemId,
      long weaponEntityId)
    {
      if (this.Closed)
        return;
      if (!this.IsDead)
        this.SwitchToWeaponInternal(weapon, false, inventoryItemId, weaponEntityId);
      if (this.OnWeaponChanged == null)
        return;
      this.OnWeaponChanged((object) this, (System.EventArgs) null);
    }

    private Vector3D DynamicEffectiveRangeStart(MatrixD? headMatrix = null)
    {
      MatrixD matrixD = headMatrix ?? this.GetHeadMatrix(true, true, false, false, false);
      if (!this.IsCrouching)
        return matrixD.Translation;
      MatrixD worldMatrix1 = this.PositionComp.WorldMatrix;
      MatrixD worldMatrix2 = this.WorldMatrix;
      Vector3D translation = worldMatrix2.Translation;
      worldMatrix2 = this.WorldMatrix;
      Vector3D vector3D = worldMatrix2.Up * (double) this.Definition.CrouchHeadServerOffset;
      return translation + vector3D;
    }

    private void UpdateDynamicRange()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !MyFakes.ENABLE_DYNAMIC_EFFECTIVE_RANGE || this.m_parallelDynamicRangeRunning)
        return;
      MatrixD headMatrix = this.GetHeadMatrix(true, true, false, false, false);
      Vector3D zero = Vector3D.Zero;
      Vector3D from = this.DynamicEffectiveRangeStart(new MatrixD?(headMatrix));
      Vector3D to = from + headMatrix.Forward * (double) MyCharacter.DYNAMIC_RANGE_MAX_DISTANCE;
      this.m_parallelDynamicRangeRunning = true;
      this.m_dynamicRangeStart = from;
      this.m_hitList.Clear();
      MyPhysics.CastRayParallel(ref from, ref to, this.m_hitList, 18, new Action<List<MyPhysics.HitInfo>>(this.OnHitRaycastDynamicRange));
    }

    private void OnHitRaycastDynamicRange(List<MyPhysics.HitInfo> hitInfos) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      this.ProcessHitDynamicRange(hitInfos);
      this.m_parallelDynamicRangeRunning = false;
    }), "Dynamic Range");

    private bool ProcessHitDynamicRange(List<MyPhysics.HitInfo> hitInfos)
    {
      if (hitInfos.Count > 0)
      {
        foreach (MyPhysics.HitInfo hitInfo in hitInfos)
        {
          if (hitInfo.HkHitInfo.GetHitEntity() as MyEntity != this)
          {
            Vector3D vector3D1 = this.DynamicEffectiveRangeStart();
            Vector3D vector3D2 = hitInfo.Position - this.m_dynamicRangeStart;
            Vector3D vector3D3 = vector3D2;
            Vector3D vector3D4 = vector3D1 + vector3D3 - this.PositionComp.GetPosition();
            this.m_dynamicRangeDistance.Value = ((float) vector3D2.Normalize(), vector3D4);
            return true;
          }
        }
      }
      else
        this.m_dynamicRangeDistance.Value = (-1f, Vector3D.Zero);
      return true;
    }

    private void UpdateCharacterMarkers()
    {
      if (this.m_parallelEnemyIndicatorRunning || this.ControllerInfo == null || this.ControllerInfo.Controller == null)
        return;
      this.m_parallelEnemyIndicatorRunning = true;
      Vector3D position = MySector.MainCamera.Position;
      double indicatorTargetDistance = (double) this.ENEMY_INDICATOR_TARGET_DISTANCE;
      Vector3D aimedPointFromCamera = this.GetAimedPointFromCamera();
      Vector3D vector3D1 = (Vector3D) Vector3.Normalize(aimedPointFromCamera - position);
      Vector3D vector3D2 = position + vector3D1 * (double) this.ENEMY_INDICATOR_TARGET_DISTANCE;
      Vector3D from = position;
      if (!MySession.Static.CameraController.IsInFirstPersonView)
      {
        float num = Vector3.Distance((Vector3) MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), (Vector3) MySector.MainCamera.Position);
        from = position + vector3D1 * (double) num;
      }
      Vector3D vector3D3 = Vector3D.Normalize(aimedPointFromCamera - from);
      Vector3D to = from + vector3D3 * (double) this.ENEMY_INDICATOR_TARGET_DISTANCE;
      MyPhysics.CastRayParallel(ref from, ref to, 18, new Action<MyPhysics.HitInfo?>(this.OnHitRaycastEnemyIndicator));
    }

    private void ProcessHitsEnemyIndicator(List<MyPhysics.HitInfo> infos) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      using (this.m_enemyIndicatorHitInfos.GetClearToken<MyPhysics.HitInfo>())
      {
        foreach (MyPhysics.HitInfo info in infos)
        {
          if (this.ProcessHitEnemyIndicator(info))
            break;
        }
      }
      this.m_parallelEnemyIndicatorRunning = false;
    }), "Enemy Indicator");

    private bool ProcessHitEnemyIndicator(MyPhysics.HitInfo hitInfo)
    {
      if (MyGuiScreenHudSpace.Static == null || !(hitInfo.HkHitInfo.GetHitEntity() is MyCharacter hitEntity) || MySession.Static.ControlledEntity == hitEntity)
        return false;
      long controllingIdentityId = this.ControllerInfo.ControllingIdentityId;
      long playerIdentityId = hitEntity.GetPlayerIdentityId();
      MyPlayer.PlayerId result;
      MySession.Static.Players.TryGetPlayerId(playerIdentityId, out result);
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      if (playerById == null || playerById.IsWildlifeAgent)
        return false;
      MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(controllingIdentityId, playerIdentityId);
      MyGuiScreenHudSpace.Static.AddPlayerMarker((MyEntity) hitEntity, relationPlayerPlayer, false);
      return true;
    }

    private void OnHitRaycastEnemyIndicator(MyPhysics.HitInfo? hitInfo)
    {
      if (hitInfo.HasValue)
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          this.ProcessHitEnemyIndicator(hitInfo.Value);
          this.m_parallelEnemyIndicatorRunning = false;
        }), "Enemy Indicator");
      else
        this.m_parallelEnemyIndicatorRunning = false;
    }

    private void UpdateLeftHandItemPosition()
    {
      MatrixD matrixD = this.AnimationController.CharacterBones[this.m_leftHandItemBone].AbsoluteTransform * this.WorldMatrix;
      Vector3D up = matrixD.Up;
      matrixD.Up = matrixD.Forward;
      matrixD.Forward = up;
      matrixD.Right = Vector3D.Cross(matrixD.Forward, matrixD.Up);
      this.m_leftHandItem.WorldMatrix = matrixD;
    }

    public float CurrentJump => this.m_currentJumpTime;

    public MyToolbarType ToolbarType => MyToolbarType.Character;

    public void ChangeModelAndColor(
      string model,
      Vector3 colorMaskHSV,
      bool resetToDefault = false,
      long caller = 0)
    {
      if (!this.ResponsibleForUpdate(Sandbox.Game.Multiplayer.Sync.Clients.LocalClient))
        return;
      MyMultiplayer.RaiseEvent<MyCharacter, string, Vector3, bool, long>(this, (Func<MyCharacter, Action<string, Vector3, bool, long>>) (x => new Action<string, Vector3, bool, long>(x.ChangeModel_Implementation)), model, colorMaskHSV, resetToDefault, caller);
    }

    [Event(null, 9044)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [Broadcast]
    private void ChangeModel_Implementation(
      string model,
      Vector3 colorMaskHSV,
      bool resetToDefault,
      long caller)
    {
      bool flag = this.m_characterModel != model;
      this.ChangeModelAndColorInternal(model, colorMaskHSV);
      if (!this.ResponsibleForUpdate(Sandbox.Game.Multiplayer.Sync.Clients.LocalClient))
        return;
      MyGuiScreenLoadInventory.ResetOnFinish(model, resetToDefault);
      if (!flag || this.m_characterDefinition == null || !(this.m_characterDefinition.Skeleton == "Humanoid"))
        return;
      MyLocalCache.LoadInventoryConfig(this, false, false);
      if (!(this.CurrentWeapon is MyEntity currentWeapon))
        return;
      MyAssetModifierComponent skinComponent = currentWeapon.Components.Get<MyAssetModifierComponent>();
      if (skinComponent == null)
        return;
      MyLocalCache.LoadInventoryConfig(this, currentWeapon, skinComponent);
    }

    public void UpdateStoredGas(MyDefinitionId gasId, float fillLevel) => MyMultiplayer.RaiseEvent<MyCharacter, SerializableDefinitionId, float>(this, (Func<MyCharacter, Action<SerializableDefinitionId, float>>) (x => new Action<SerializableDefinitionId, float>(x.UpdateStoredGas_Implementation)), (SerializableDefinitionId) gasId, fillLevel);

    [Event(null, 9072)]
    [Reliable]
    [Broadcast]
    private void UpdateStoredGas_Implementation(SerializableDefinitionId gasId, float fillLevel)
    {
      if (this.OxygenComponent == null)
        return;
      MyDefinitionId gasId1 = (MyDefinitionId) gasId;
      this.OxygenComponent.UpdateStoredGasLevel(ref gasId1, fillLevel);
    }

    public void UpdateOxygen(float oxygenAmount) => MyMultiplayer.RaiseEvent<MyCharacter, float>(this, (Func<MyCharacter, Action<float>>) (x => new Action<float>(x.OnUpdateOxygen)), oxygenAmount);

    [Event(null, 9087)]
    [Reliable]
    [Broadcast]
    private void OnUpdateOxygen(float oxygenAmount)
    {
      if (this.OxygenComponent == null)
        return;
      this.OxygenComponent.SuitOxygenAmount = oxygenAmount;
    }

    public void SendRefillFromBottle(MyDefinitionId gasId) => MyMultiplayer.RaiseEvent<MyCharacter, SerializableDefinitionId>(this, (Func<MyCharacter, Action<SerializableDefinitionId>>) (x => new Action<SerializableDefinitionId>(x.OnRefillFromBottle)), (SerializableDefinitionId) gasId);

    [Event(null, 9101)]
    [Reliable]
    [Broadcast]
    private void OnRefillFromBottle(SerializableDefinitionId gasId)
    {
      if (this != MySession.Static.LocalCharacter)
        return;
      MyCharacterOxygenComponent oxygenComponent = this.OxygenComponent;
    }

    public void PlaySecondarySound(MyCueId soundId) => MyMultiplayer.RaiseEvent<MyCharacter, MyCueId>(this, (Func<MyCharacter, Action<MyCueId>>) (x => new Action<MyCueId>(x.OnSecondarySoundPlay)), soundId);

    [Event(null, 9115)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    private void OnSecondarySoundPlay(MyCueId soundId)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.SoundComp.StartSecondarySound(soundId);
    }

    internal void ChangeModelAndColorInternal(string model, Vector3 colorMaskHSV)
    {
      if (this.Closed)
        return;
      MyCharacterDefinition result;
      if (model != this.m_characterModel && (MyDefinitionManager.Static.Characters.TryGetValue(model, out result) && !string.IsNullOrEmpty(result.Model)))
      {
        MyObjectBuilder_Character objectBuilder = (MyObjectBuilder_Character) this.GetObjectBuilder(false);
        this.Components.Remove<MyInventoryBase>();
        this.Components.Remove<MyCharacterJetpackComponent>();
        this.Components.Remove<MyCharacterRagdollComponent>();
        this.AnimationController.Clear();
        MyModel modelOnlyData = MyModels.GetModelOnlyData(result.Model);
        if (modelOnlyData == null)
          return;
        if (MySandboxGame.Static.UpdateThread == Thread.CurrentThread)
          this.Render.CleanLights();
        this.CloseInternal();
        if (!Sandbox.Game.Entities.MyEntities.Remove((MyEntity) this))
        {
          Sandbox.Game.Entities.MyEntities.UnregisterForUpdate((MyEntity) this);
          this.Render.RemoveRenderObjects();
        }
        if (this.Physics != null)
        {
          this.Physics.Close();
          this.Physics = (MyPhysicsBody) null;
        }
        this.m_characterModel = model;
        this.Render.ModelStorage = (object) modelOnlyData;
        objectBuilder.CharacterModel = model;
        objectBuilder.EntityId = 0L;
        if (objectBuilder.HandWeapon != null)
          objectBuilder.HandWeapon.EntityId = 0L;
        if (this.m_breath != null)
        {
          this.m_breath.Close();
          this.m_breath = (MyCharacterBreath) null;
        }
        float num = this.StatComp != null ? this.StatComp.HealthRatio : 1f;
        float headLocalXangle = this.m_headLocalXAngle;
        float headLocalYangle = this.m_headLocalYAngle;
        MatrixD worldMatrix = this.PositionComp.WorldMatrixRef;
        objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?();
        this.Init((MyObjectBuilder_EntityBase) objectBuilder);
        this.PositionComp.SetWorldMatrix(ref worldMatrix);
        MyEntityExtensions.GetInventory(this).ResetVolume();
        this.InitInventory(objectBuilder);
        this.m_headLocalXAngle = headLocalXangle;
        this.m_headLocalYAngle = headLocalYangle;
        if (this.StatComp != null && this.StatComp.Health != null)
          this.StatComp.Health.Value = this.StatComp.Health.MaxValue - this.StatComp.Health.MaxValue * (1f - num);
        this.SwitchAnimation(objectBuilder.MovementState, false);
        if (this.m_currentWeapon != null)
          this.m_currentWeapon.OnControlAcquired(this);
        if (this.Parent == null)
          Sandbox.Game.Entities.MyEntities.Add((MyEntity) this);
        else if (!this.InScene)
          this.OnAddedToScene((object) this);
        MyPlayer player = this.TryGetPlayer();
        if (player != null && player.Identity != null)
          player.Identity.ChangeCharacter(this);
        this.SuitRechargeDistributor.UpdateBeforeSimulation();
      }
      this.Render.ColorMaskHsv = colorMaskHSV;
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      MySession.Static.LocalHumanPlayer.Identity.SetColorMask(colorMaskHSV);
    }

    public void SetPhysicsEnabled(bool enabled) => MyMultiplayer.RaiseEvent<MyCharacter, bool>(this, (Func<MyCharacter, Action<bool>>) (x => new Action<bool>(x.EnablePhysics)), enabled);

    [Event(null, 9239)]
    [Reliable]
    [Broadcast]
    private void EnablePhysics(bool enabled) => this.Physics.Enabled = enabled;

    public MyRelationsBetweenPlayerAndBlock GetRelationTo(
      long playerId)
    {
      return MyPlayer.GetRelationBetweenPlayers(this.GetPlayerIdentityId(), playerId);
    }

    VRage.ModAPI.IMyEntity IMyUseObject.Owner => (VRage.ModAPI.IMyEntity) this;

    MyModelDummy IMyUseObject.Dummy => (MyModelDummy) null;

    float IMyUseObject.InteractiveDistance => 5f;

    MatrixD IMyUseObject.ActivationMatrix
    {
      get
      {
        if (this.PositionComp == null)
          return MatrixD.Zero;
        if (this.IsDead && this.Physics != null && this.Definition.DeadBodyShape != null)
        {
          float num = 0.8f;
          MatrixD worldMatrix = this.WorldMatrix;
          Matrix matrix = (Matrix) ref worldMatrix;
          matrix.Forward *= num;
          matrix.Up *= this.Definition.CharacterCollisionHeight * num;
          matrix.Right *= num;
          matrix.Translation = (Vector3) this.PositionComp.WorldAABB.Center;
          matrix.Translation += 0.5f * matrix.Right * this.Definition.DeadBodyShape.RelativeShapeTranslation.X;
          matrix.Translation += 0.5f * matrix.Up * this.Definition.DeadBodyShape.RelativeShapeTranslation.Y;
          matrix.Translation += 0.5f * matrix.Forward * this.Definition.DeadBodyShape.RelativeShapeTranslation.Z;
          return (MatrixD) ref matrix;
        }
        float num1 = 0.75f;
        MatrixD worldMatrix1 = this.WorldMatrix;
        Matrix matrix1 = (Matrix) ref worldMatrix1;
        matrix1.Forward *= num1;
        matrix1.Up *= this.Definition.CharacterCollisionHeight * num1;
        matrix1.Right *= num1;
        matrix1.Translation = (Vector3) this.PositionComp.WorldAABB.Center;
        return (MatrixD) ref matrix1;
      }
    }

    MatrixD IMyUseObject.WorldMatrix => this.WorldMatrix;

    uint IMyUseObject.RenderObjectID => base.Render.GetRenderObjectID();

    void IMyUseObject.SetRenderID(uint id)
    {
    }

    int IMyUseObject.InstanceID => -1;

    void IMyUseObject.SetInstanceID(int id)
    {
    }

    bool IMyUseObject.ShowOverlay => false;

    UseActionEnum IMyUseObject.SupportedActions => this.IsDead && !this.Definition.EnableSpawnInventoryAsContainer ? UseActionEnum.OpenTerminal | UseActionEnum.OpenInventory : UseActionEnum.None;

    UseActionEnum IMyUseObject.PrimaryAction => this.IsDead && !this.Definition.EnableSpawnInventoryAsContainer ? UseActionEnum.OpenInventory : UseActionEnum.None;

    UseActionEnum IMyUseObject.SecondaryAction => this.IsDead && !this.Definition.EnableSpawnInventoryAsContainer ? UseActionEnum.OpenTerminal : UseActionEnum.None;

    bool IMyUseObject.ContinuousUsage => false;

    void IMyUseObject.Use(UseActionEnum actionEnum, VRage.ModAPI.IMyEntity entity)
    {
      MyCharacter user = entity as MyCharacter;
      if (MyPerGameSettings.TerminalEnabled)
        MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, (MyEntity) this);
      if (!(MyPerGameSettings.GUI.InventoryScreen != (System.Type) null) || !this.IsDead)
        return;
      MyInventoryAggregate inventoryAggregate = this.Components.Get<MyInventoryAggregate>();
      if (inventoryAggregate == null)
        return;
      user.ShowAggregateInventoryScreen((MyInventoryBase) inventoryAggregate);
    }

    MyActionDescription IMyUseObject.GetActionInfo(
      UseActionEnum actionEnum)
    {
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToOpenInventory,
        FormatParams = new object[2]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY) + "]"),
          (object) this.DisplayName
        },
        IsTextControlHint = true,
        JoystickText = new MyStringId?(MyCommonTexts.NotificationHintJoystickPressToOpenInventory),
        JoystickFormatParams = new object[2]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
          (object) this.DisplayName
        },
        ShowForGamepad = true
      };
    }

    void IMyUseObject.OnSelectionLost()
    {
    }

    bool IMyUseObject.PlayIndicatorSound => true;

    public void SwitchLandingGears()
    {
    }

    public void SwitchHandbrake()
    {
    }

    public void OnInventoryBreak()
    {
    }

    public void OnDestroy() => this.Die();

    public bool UseDamageSystem { get; private set; }

    public float Integrity
    {
      get
      {
        float num = 100f;
        if (this.StatComp != null && this.StatComp.Health != null)
          num = this.StatComp.Health.Value;
        return num;
      }
    }

    void IMyCameraController.ControlCamera(MyCamera currentCamera)
    {
      MatrixD viewMatrix = this.GetViewMatrix();
      currentCamera.SetViewMatrix(viewMatrix);
      currentCamera.CameraSpring.Enabled = !this.IsInFirstPersonView && !this.ForceFirstPersonCamera;
      this.EnableHead(!this.ControllerInfo.IsLocallyControlled() || !this.IsInFirstPersonView && !this.ForceFirstPersonCamera);
    }

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.Rotate(rotationIndicator, rollIndicator);
    }

    void IMyCameraController.RotateStopped() => this.RotateStopped();

    void IMyCameraController.OnAssumeControl(
      IMyCameraController previousCameraController)
    {
      this.OnAssumeControl(previousCameraController);
    }

    void IMyCameraController.OnReleaseControl(
      IMyCameraController newCameraController)
    {
      this.OnReleaseControl(newCameraController);
      if (!this.InScene)
        return;
      this.EnableHead(true);
    }

    bool IMyCameraController.IsInFirstPersonView
    {
      get => this.IsInFirstPersonView;
      set => this.IsInFirstPersonView = value;
    }

    bool IMyCameraController.ForceFirstPersonCamera
    {
      get => this.ForceFirstPersonCamera;
      set => this.ForceFirstPersonCamera = value;
    }

    bool IMyCameraController.HandleUse() => false;

    bool IMyCameraController.HandlePickUp() => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ForceFirstPersonCamera
    {
      get => this.ForceFirstPersonCamera;
      set => this.ForceFirstPersonCamera = value;
    }

    bool IMyCameraController.AllowCubeBuilding => true;

    MatrixD VRage.Game.ModAPI.Interfaces.IMyControllableEntity.GetHeadMatrix(
      bool includeY,
      bool includeX,
      bool forceHeadAnim,
      bool forceHeadBone)
    {
      return this.GetHeadMatrix(includeY, includeX, forceHeadAnim, false, false);
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped() => this.MoveAndRotateStopped();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Use() => this.Use();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.UseContinues() => this.UseContinues();

    void Sandbox.Game.Entities.IMyControllableEntity.UseFinished() => this.UseFinished();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp() => this.PickUp();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUpContinues() => this.PickUpContinues();

    void Sandbox.Game.Entities.IMyControllableEntity.PickUpFinished() => this.PickUpFinished();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Jump(
      Vector3 moveIndicator)
    {
      this.Jump(moveIndicator);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyCharacter, Vector3>(this, (Func<MyCharacter, Action<Vector3>>) (x => new Action<Vector3>(x.Jump)), moveIndicator);
    }

    void Sandbox.Game.Entities.IMyControllableEntity.Sprint(bool enabled) => this.Sprint(enabled);

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Up() => this.Up();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Crouch() => this.Crouch();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Down() => this.Down();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowInventory() => this.ShowInventory();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowTerminal() => this.ShowTerminal();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchThrusts()
    {
      MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
      if (jetpackComp == null || !this.HasEnoughSpaceToStandUp())
        return;
      jetpackComp.SwitchThrusts();
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchDamping()
    {
      MyCharacterJetpackComponent jetpackComp = this.JetpackComp;
      if (jetpackComp == null)
        return;
      jetpackComp.SwitchDamping();
      if (jetpackComp.DampenersEnabled)
        return;
      this.RelativeDampeningEntity = (MyEntity) null;
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLights() => this.SwitchLights();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLandingGears() => this.SwitchLandingGears();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHandbrake() => this.SwitchHandbrake();

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors() => this.SwitchReactors();

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledThrusts => this.JetpackComp != null && this.JetpackComp.TurnedOn;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledDamping => this.JetpackComp != null && this.JetpackComp.DampenersTurnedOn;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLights => this.LightEnabled;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLeadingGears => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledReactors => false;

    bool Sandbox.Game.Entities.IMyControllableEntity.EnabledBroadcasting => this.RadioBroadcaster.Enabled;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledHelmet => this.OxygenComponent.HelmetEnabled;

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHelmet()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer && MySession.Static.LocalCharacter != this)
        return;
      MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.OnSwitchHelmet)));
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Die() => this.Die();

    void IMyDestroyableObject.OnDestroy() => this.OnDestroy();

    bool IMyDestroyableObject.DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      return this.DoDamage(damage, damageType, sync, attackerId);
    }

    float IMyDestroyableObject.Integrity => this.Integrity;

    public bool PrimaryLookaround => this.IsOnLadder && !this.IsInFirstPersonView;

    public static void Preload(WorkPriority priority)
    {
      ListReader<MyAnimationDefinition> animationDefinitions = MyDefinitionManager.Static.GetAnimationDefinitions();
      WorkOptions workOptions = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "CharacterLoadAnimations");
      workOptions.MaximumThreads = int.MaxValue;
      Parallel.ForEach<MyAnimationDefinition>((IEnumerable<MyAnimationDefinition>) animationDefinitions, (Action<MyAnimationDefinition>) (animation =>
      {
        string animationModel = animation.AnimationModel;
        if (string.IsNullOrEmpty(animationModel))
          return;
        MyModels.GetModelOnlyAnimationData(animationModel);
      }), priority, new WorkOptions?(workOptions));
      if (!MyModelImporter.LINEAR_KEYFRAME_REDUCTION_STATS)
        return;
      Dictionary<string, List<MyModelImporter.ReductionInfo>> reductionStats = MyModelImporter.ReductionStats;
      List<float> source = new List<float>();
      foreach (KeyValuePair<string, List<MyModelImporter.ReductionInfo>> keyValuePair in reductionStats)
      {
        foreach (MyModelImporter.ReductionInfo reductionInfo in keyValuePair.Value)
          source.Add((float) reductionInfo.OptimizedKeys / (float) reductionInfo.OriginalKeys);
      }
      double num = (double) source.Average();
    }

    public MyCharacterMovementFlags MovementFlags
    {
      get => this.m_movementFlags;
      internal set => this.m_movementFlags = value;
    }

    public MyCharacterMovementFlags PreviousMovementFlags => this.m_previousMovementFlags;

    public bool WantsJump
    {
      get => (this.m_movementFlags & MyCharacterMovementFlags.Jump) == MyCharacterMovementFlags.Jump;
      private set
      {
        if (value)
          this.m_movementFlags |= MyCharacterMovementFlags.Jump;
        else
          this.m_movementFlags &= ~MyCharacterMovementFlags.Jump;
      }
    }

    private bool WantsSprint
    {
      get => (this.m_movementFlags & MyCharacterMovementFlags.Sprint) == MyCharacterMovementFlags.Sprint;
      set
      {
        if (value)
          this.m_movementFlags |= MyCharacterMovementFlags.Sprint;
        else
          this.m_movementFlags &= ~MyCharacterMovementFlags.Sprint;
      }
    }

    public bool WantsWalk
    {
      get => (this.m_movementFlags & MyCharacterMovementFlags.Walk) == MyCharacterMovementFlags.Walk;
      private set
      {
        if (value)
          this.m_movementFlags |= MyCharacterMovementFlags.Walk;
        else
          this.m_movementFlags &= ~MyCharacterMovementFlags.Walk;
      }
    }

    private bool WantsFlyUp
    {
      get => (this.m_movementFlags & MyCharacterMovementFlags.FlyUp) == MyCharacterMovementFlags.FlyUp;
      set
      {
        if (value)
          this.m_movementFlags |= MyCharacterMovementFlags.FlyUp;
        else
          this.m_movementFlags &= ~MyCharacterMovementFlags.FlyUp;
      }
    }

    private bool WantsFlyDown
    {
      get => (this.m_movementFlags & MyCharacterMovementFlags.FlyDown) == MyCharacterMovementFlags.FlyDown;
      set
      {
        if (value)
          this.m_movementFlags |= MyCharacterMovementFlags.FlyDown;
        else
          this.m_movementFlags &= ~MyCharacterMovementFlags.FlyDown;
      }
    }

    private bool WantsCrouch
    {
      get => (this.m_movementFlags & MyCharacterMovementFlags.Crouch) == MyCharacterMovementFlags.Crouch;
      set
      {
        if (value)
          this.m_movementFlags |= MyCharacterMovementFlags.Crouch;
        else
          this.m_movementFlags &= ~MyCharacterMovementFlags.Crouch;
      }
    }

    public MyCharacterBreath Breath => this.m_breath;

    bool IMyUseObject.HandleInput()
    {
      MyCharacterDetectorComponent detectorComponent = this.Components.Get<MyCharacterDetectorComponent>();
      return detectorComponent != null && detectorComponent.UseObject != null && detectorComponent.UseObject.HandleInput();
    }

    public float CharacterAccumulatedDamage { get; set; }

    public MyEntityCameraSettings GetCameraEntitySettings() => this.m_cameraSettingsWhenAlive;

    public MyStringId ControlContext
    {
      get
      {
        if (MySession.Static.CameraController == MySpectator.Static)
          return MySpaceBindingCreator.CX_SPECTATOR;
        return this.JetpackRunning ? MySpaceBindingCreator.CX_JETPACK : MySpaceBindingCreator.CX_CHARACTER;
      }
    }

    public MyStringId AuxiliaryContext
    {
      get
      {
        if (MyCubeBuilder.Static.IsActivated)
        {
          if (MyCubeBuilder.Static.IsSymmetrySetupMode())
            return MySpaceBindingCreator.AX_SYMMETRY;
          if (MyCubeBuilder.Static.IsBuildToolActive())
            return MySpaceBindingCreator.AX_BUILD;
          if (MyCubeBuilder.Static.IsOnlyColorToolActive())
            return MySpaceBindingCreator.AX_COLOR_PICKER;
        }
        if (MySessionComponentVoxelHand.Static.Enabled)
          return MySpaceBindingCreator.AX_VOXEL;
        return MyClipboardComponent.Static.IsActive ? MySpaceBindingCreator.AX_CLIPBOARD : MySpaceBindingCreator.AX_TOOLS;
      }
    }

    private void ResetMovement()
    {
      this.MoveIndicator = Vector3.Zero;
      this.RotationIndicator = Vector2.Zero;
      this.RollIndicator = 0.0f;
    }

    public float EnvironmentOxygenLevel => (float) this.EnvironmentOxygenLevelSync;

    public float OxygenLevel => (float) this.OxygenLevelAtCharacterLocation;

    public float SuitEnergyLevel => this.SuitBattery.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId) / 1E-05f;

    public float GetSuitGasFillLevel(MyDefinitionId gasDefinitionId) => this.OxygenComponent.GetGasFillLevel(gasDefinitionId);

    public bool IsPlayer => !MySession.Static.Players.IdentityIsNpc(this.GetPlayerIdentityId());

    public bool IsBot => !this.IsPlayer;

    public int SpineBoneIndex => this.m_spineBone;

    public int HeadBoneIndex => this.m_headBoneIndex;

    public VRage.ModAPI.IMyEntity EquippedTool => this.m_currentWeapon as VRage.ModAPI.IMyEntity;

    private void KillCharacter(MyDamageInformation damageInfo)
    {
      this.Kill(false, damageInfo);
      MyMultiplayer.RaiseEvent<MyCharacter, MyDamageInformation, Vector3>(this, (Func<MyCharacter, Action<MyDamageInformation, Vector3>>) (x => new Action<MyDamageInformation, Vector3>(x.OnKillCharacter)), damageInfo, this.Physics.LinearVelocity);
    }

    [Event(null, 10081)]
    [Reliable]
    [Broadcast]
    private void OnKillCharacter(MyDamageInformation damageInfo, Vector3 lastLinearVelocity)
    {
      this.m_deathLinearVelocityFromSever = new Vector3?(lastLinearVelocity);
      this.Kill(false, damageInfo);
    }

    [Event(null, 10088)]
    [Reliable]
    [Broadcast]
    public void SpawnCharacterRelative(long RelatedEntity, Vector3 DeltaPosition)
    {
      MyEntity entity;
      if (RelatedEntity == 0L || !Sandbox.Game.Entities.MyEntities.TryGetEntityById(RelatedEntity, out entity))
        return;
      this.Physics.LinearVelocity = entity.Physics.LinearVelocity;
      this.Physics.AngularVelocity = entity.Physics.AngularVelocity;
      this.PositionComp.SetPosition((Matrix.CreateTranslation(DeltaPosition) * entity.WorldMatrix).Translation);
    }

    public void SetPlayer(MyPlayer player, bool update = true)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_controlInfo.Value = player.Id;
      if (!update)
        return;
      MyPlayerCollection.ChangePlayerCharacter(player, this, (MyEntity) this);
    }

    private void ControlChanged()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsDead)
        return;
      if (this.m_controlInfo.Value.SteamId != 0UL && (this.ControllerInfo.Controller == null || this.ControllerInfo.Controller.Player.Id != this.m_controlInfo.Value))
      {
        MyPlayer playerById = Sandbox.Game.Multiplayer.Sync.Players.GetPlayerById(this.m_controlInfo.Value);
        if (playerById != null)
        {
          MyPlayerCollection.ChangePlayerCharacter(playerById, this, (MyEntity) this);
          if (playerById.Controller != null && playerById.Controller.ControlledEntity != null)
            this.IsUsing = playerById.Controller.ControlledEntity as MyEntity;
          if (this.m_usingEntity != null && playerById != null && Sandbox.Game.Multiplayer.Sync.Players.GetControllingPlayer(this.m_usingEntity) != playerById)
            Sandbox.Game.Multiplayer.Sync.Players.SetControlledEntityLocally(playerById.Id, this.m_usingEntity);
        }
      }
      if (this.IsDead || this != MySession.Static.LocalCharacter)
        return;
      MySpectatorCameraController.Static.Position = this.PositionComp.GetPosition();
    }

    public bool ResponsibleForUpdate(MyNetworkClient player)
    {
      if (Sandbox.Game.Multiplayer.Sync.Players == null)
        return false;
      MyPlayer controllingPlayer = Sandbox.Game.Multiplayer.Sync.Players.GetControllingPlayer((MyEntity) this);
      if (controllingPlayer == null && this.CurrentRemoteControl != null)
        controllingPlayer = Sandbox.Game.Multiplayer.Sync.Players.GetControllingPlayer(this.CurrentRemoteControl as MyEntity);
      return controllingPlayer == null ? player.IsGameServer() : controllingPlayer.Client == player;
    }

    private void StartShooting(Vector3 direction, MyShootActionEnum action, bool doubleClick)
    {
      this.ShootDirection = direction;
      this.m_shootDoubleClick = doubleClick;
      this.OnBeginShoot(action);
    }

    public void BeginShootSync(Vector3 direction, MyShootActionEnum action = MyShootActionEnum.PrimaryAction, bool doubleClick = false)
    {
      this.StartShooting(direction, action, doubleClick);
      MyMultiplayer.RaiseEvent<MyCharacter, Vector3, MyShootActionEnum, bool>(this, (Func<MyCharacter, Action<Vector3, MyShootActionEnum, bool>>) (x => new Action<Vector3, MyShootActionEnum, bool>(x.ShootBeginCallback)), direction, action, doubleClick);
      if (!MyFakes.SIMULATE_QUICK_TRIGGER)
        return;
      this.EndShootInternal(action);
    }

    [Event(null, 10190)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    private void ShootBeginCallback(Vector3 direction, MyShootActionEnum action, bool doubleClick)
    {
      if ((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (MyEventContext.Current.IsLocallyInvoked ? 1 : 0)) != 0)
        return;
      this.StartShooting(direction, action, doubleClick);
    }

    private void StopShooting(MyShootActionEnum action)
    {
      this.m_isShooting[(int) action] = false;
      this.OnEndShoot(action);
    }

    private void GunDoubleClicked(MyShootActionEnum action) => this.OnGunDoubleClicked(action);

    public void EndShootSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
    {
      if (MyFakes.SIMULATE_QUICK_TRIGGER)
        return;
      this.EndShootInternal(action);
    }

    public void GunDoubleClickedSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction) => this.GunDoubleClickedInternal(action);

    private void EndShootInternal(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
    {
      MyMultiplayer.RaiseEvent<MyCharacter, MyShootActionEnum>(this, (Func<MyCharacter, Action<MyShootActionEnum>>) (x => new Action<MyShootActionEnum>(x.ShootEndCallback)), action);
      this.StopShooting(action);
    }

    private void GunDoubleClickedInternal(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
    {
      MyMultiplayer.RaiseEvent<MyCharacter, MyShootActionEnum>(this, (Func<MyCharacter, Action<MyShootActionEnum>>) (x => new Action<MyShootActionEnum>(x.GunDoubleClickedCallback)), action);
      this.GunDoubleClicked(action);
    }

    [Event(null, 10235)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    private void ShootEndCallback(MyShootActionEnum action)
    {
      if ((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (MyEventContext.Current.IsLocallyInvoked ? 1 : 0)) != 0)
        return;
      this.StopShooting(action);
    }

    [Event(null, 10245)]
    [Reliable]
    [Server]
    [BroadcastExcept]
    private void GunDoubleClickedCallback(MyShootActionEnum action)
    {
      if ((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (MyEventContext.Current.IsLocallyInvoked ? 1 : 0)) != 0)
        return;
      this.GunDoubleClicked(action);
    }

    public void UpdateShootDirection(bool forceUpdate, bool shootStraight = false)
    {
      int directionUpdateTime = this.m_currentWeapon.ShootDirectionUpdateTime;
      if (directionUpdateTime == 0)
        return;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (!forceUpdate && ((long) timeInMilliseconds - this.m_lastShootDirectionUpdate <= (long) directionUpdateTime || !this.m_currentWeapon.IsShooting && !this.m_currentWeapon.NeedsShootDirectionWhileAiming))
        return;
      this.m_aimedPoint = this.GetAimedPointFromCamera();
      Vector3 vector3 = this.m_currentWeapon.DirectionToTarget(this.m_aimedPoint);
      MatrixD headMatrix = this.GetHeadMatrix(false, !this.JetpackRunning, false, false, false);
      if (shootStraight)
        vector3 = (Vector3) headMatrix.Forward;
      if (this.ControllerInfo != null && this.ControllerInfo.IsLocallyControlled())
        MyMultiplayer.RaiseEvent<MyCharacter, Vector3>(this, (Func<MyCharacter, Action<Vector3>>) (x => new Action<Vector3>(x.ShootDirectionChangeCallback)), vector3);
      this.ShootDirection = vector3;
      this.m_lastShootDirectionUpdate = (long) timeInMilliseconds;
    }

    [Event(null, 10288)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [BroadcastExcept]
    private void ShootDirectionChangeCallback(Vector3 direction)
    {
      if (this.ControllerInfo != null && this.ControllerInfo.IsLocallyControlled())
        return;
      this.ShootDirection = direction;
    }

    [Event(null, 10297)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    private void OnSwitchAmmoMagazineRequest()
    {
      if (!this.CanSwitchAmmoMagazine())
        return;
      this.SwitchAmmoMagazineSuccess();
      MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.OnSwitchAmmoMagazineSuccess)));
    }

    [Event(null, 10309)]
    [Reliable]
    [Broadcast]
    private void OnSwitchAmmoMagazineSuccess() => this.SwitchAmmoMagazineSuccess();

    private void RequestSwitchToWeapon(MyDefinitionId? weapon, uint? inventoryItemId)
    {
      MyDefinitionId? nullable = weapon;
      MyMultiplayer.RaiseEvent<MyCharacter, SerializableDefinitionId?, uint?>(this, (Func<MyCharacter, Action<SerializableDefinitionId?, uint?>>) (x => new Action<SerializableDefinitionId?, uint?>(x.SwitchToWeaponMessage)), nullable.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) nullable.GetValueOrDefault()) : new SerializableDefinitionId?(), inventoryItemId);
    }

    [Event(null, 10321)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    private void SwitchToWeaponMessage(SerializableDefinitionId? weapon, uint? inventoryItemId)
    {
      SerializableDefinitionId? nullable1 = weapon;
      if (!this.CanSwitchToWeapon(nullable1.HasValue ? new MyDefinitionId?((MyDefinitionId) nullable1.GetValueOrDefault()) : new MyDefinitionId?()))
        return;
      if ((bool) this.IsReloading)
      {
        this.m_isReloadingPrevious = false;
        this.m_shouldPositionHandPrevious = false;
        this.m_reloadHandSimulationState = true;
        this.ShouldPositionMagazine = false;
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.IsReloading.Value = false;
      }
      if (inventoryItemId.HasValue)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this);
        if (inventory == null)
          return;
        MyPhysicalInventoryItem? itemById = inventory.GetItemByID(inventoryItemId.Value);
        if (!itemById.HasValue)
          return;
        MyDefinitionId? nullable2 = MyDefinitionManager.Static.ItemIdFromWeaponId((MyDefinitionId) weapon.Value);
        if (!nullable2.HasValue || !(itemById.Value.Content.GetObjectId() == nullable2.Value))
          return;
        long weaponEntityId = MyEntityIdentifier.AllocateId();
        nullable1 = weapon;
        this.SwitchToWeaponSuccess(nullable1.HasValue ? new MyDefinitionId?((MyDefinitionId) nullable1.GetValueOrDefault()) : new MyDefinitionId?(), inventoryItemId, weaponEntityId);
        MyMultiplayer.RaiseEvent<MyCharacter, SerializableDefinitionId?, uint?, long>(this, (Func<MyCharacter, Action<SerializableDefinitionId?, uint?, long>>) (x => new Action<SerializableDefinitionId?, uint?, long>(x.OnSwitchToWeaponSuccess)), weapon, inventoryItemId, weaponEntityId);
      }
      else if (weapon.HasValue)
      {
        long weaponEntityId = MyEntityIdentifier.AllocateId();
        nullable1 = weapon;
        this.SwitchToWeaponSuccess(nullable1.HasValue ? new MyDefinitionId?((MyDefinitionId) nullable1.GetValueOrDefault()) : new MyDefinitionId?(), inventoryItemId, weaponEntityId);
        MyMultiplayer.RaiseEvent<MyCharacter, SerializableDefinitionId?, uint?, long>(this, (Func<MyCharacter, Action<SerializableDefinitionId?, uint?, long>>) (x => new Action<SerializableDefinitionId?, uint?, long>(x.OnSwitchToWeaponSuccess)), weapon, inventoryItemId, weaponEntityId);
      }
      else
        MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.UnequipWeapon)));
    }

    [Event(null, 10370)]
    [Reliable]
    [Broadcast]
    private void OnSwitchToWeaponSuccess(
      SerializableDefinitionId? weapon,
      uint? inventoryItemId,
      long weaponEntityId)
    {
      SerializableDefinitionId? nullable = weapon;
      this.SwitchToWeaponSuccess(nullable.HasValue ? new MyDefinitionId?((MyDefinitionId) nullable.GetValueOrDefault()) : new MyDefinitionId?(), inventoryItemId, weaponEntityId);
    }

    public void SendAnimationCommand(ref MyAnimationCommand command) => MyMultiplayer.RaiseEvent<MyCharacter, MyAnimationCommand>(this, (Func<MyCharacter, Action<MyAnimationCommand>>) (x => new Action<MyAnimationCommand>(x.OnAnimationCommand)), command);

    [Event(null, 10381)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [Broadcast]
    private void OnAnimationCommand(MyAnimationCommand command) => this.AddCommand(command, false);

    public void SendAnimationEvent(string eventName, string[] layers = null) => MyMultiplayer.RaiseEvent<MyCharacter, string, string[]>(this, (Func<MyCharacter, Action<string, string[]>>) (x => new Action<string, string[]>(x.OnAnimationEvent)), eventName, layers);

    [Event(null, 10392)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [Broadcast]
    private void OnAnimationEvent(string eventName, [Nullable] string[] layers)
    {
      if (!this.UseNewAnimationSystem)
        return;
      this.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName), layers);
    }

    public void SendRagdollTransforms(Matrix world, Matrix[] localBodiesTransforms)
    {
      if (!this.ResponsibleForUpdate(Sandbox.Game.Multiplayer.Sync.Clients.LocalClient))
        return;
      Vector3 translation = world.Translation;
      int length = localBodiesTransforms.Length;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(world.GetOrientation());
      Vector3[] vector3Array = new Vector3[length];
      Quaternion[] quaternionArray = new Quaternion[length];
      for (int index = 0; index < localBodiesTransforms.Length; ++index)
      {
        vector3Array[index] = localBodiesTransforms[index].Translation;
        quaternionArray[index] = Quaternion.CreateFromRotationMatrix(localBodiesTransforms[index].GetOrientation());
      }
      MyMultiplayer.RaiseEvent<MyCharacter, int, Vector3[], Quaternion[], Quaternion, Vector3>(this, (Func<MyCharacter, Action<int, Vector3[], Quaternion[], Quaternion, Vector3>>) (x => new Action<int, Vector3[], Quaternion[], Quaternion, Vector3>(x.OnRagdollTransformsUpdate)), length, vector3Array, quaternionArray, fromRotationMatrix, translation);
    }

    [Event(null, 10419)]
    [Reliable]
    [Broadcast]
    private void OnRagdollTransformsUpdate(
      int transformsCount,
      Vector3[] transformsPositions,
      Quaternion[] transformsOrientations,
      Quaternion worldOrientation,
      Vector3 worldPosition)
    {
      MyCharacterRagdollComponent ragdollComponent = this.Components.Get<MyCharacterRagdollComponent>();
      if (ragdollComponent == null || this.Physics == null || (this.Physics.Ragdoll == null || ragdollComponent.RagdollMapper == null) || (!this.Physics.Ragdoll.InWorld || !ragdollComponent.RagdollMapper.IsActive))
        return;
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(worldOrientation);
      fromQuaternion.Translation = worldPosition;
      Matrix[] transforms = new Matrix[transformsCount];
      for (int index = 0; index < transformsCount; ++index)
      {
        transforms[index] = Matrix.CreateFromQuaternion(transformsOrientations[index]);
        transforms[index].Translation = transformsPositions[index];
      }
      ragdollComponent.RagdollMapper.UpdateRigidBodiesTransformsSynced(transformsCount, fromQuaternion, transforms);
    }

    [Event(null, 10448)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [Broadcast]
    private void OnSwitchHelmet()
    {
      if (this.OxygenComponent == null)
        return;
      this.OxygenComponent.SwitchHelmet();
      if (this.m_currentWeapon == null)
        return;
      this.m_currentWeapon.UpdateSoundEmitter();
    }

    public Vector3 GetLocalWeaponPosition() => (Vector3) this.WeaponPosition.LogicalPositionLocalSpace;

    public void SyncHeadToolTransform(ref MatrixD headMatrix)
    {
      if (!this.ControllerInfo.IsLocallyControlled())
        return;
      MatrixD matrixD = headMatrix * this.PositionComp.WorldMatrixInvScaled;
      MyTransform myTransform = new MyTransform((Matrix) ref matrixD);
      myTransform.Rotation = Quaternion.Normalize(myTransform.Rotation);
    }

    [Event(null, 10476)]
    [Reliable]
    [Client]
    public void SwitchJetpack()
    {
      if (this.JetpackComp == null)
        return;
      this.JetpackComp.SwitchThrusts();
    }

    public Quaternion GetRotation()
    {
      Quaternion result;
      if (this.JetpackRunning)
      {
        MatrixD worldMatrix = this.WorldMatrix;
        Quaternion.CreateFromRotationMatrix(ref worldMatrix, out result);
      }
      else
        result = this.Physics.CharacterProxy == null ? Quaternion.CreateFromForwardUp((Vector3) this.WorldMatrix.Forward, (Vector3) this.WorldMatrix.Up) : Quaternion.CreateFromForwardUp(this.Physics.CharacterProxy.Forward, this.Physics.CharacterProxy.Up);
      return result;
    }

    public void ApplyRotation(Quaternion rot)
    {
      if (this.IsOnLadder)
        return;
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(rot);
      if (this.JetpackRunning && this.Physics.CharacterProxy != null)
      {
        float y = this.ModelCollision.BoundingBoxSizeHalf.Y;
        MatrixD worldMatrix = this.Physics.GetWorldMatrix();
        Vector3D translation = worldMatrix.Translation;
        worldMatrix = this.WorldMatrix;
        Vector3D vector3D1 = worldMatrix.Up * (double) y;
        Vector3D vector3D2 = translation + vector3D1;
        fromQuaternion.Translation = vector3D2 - fromQuaternion.Up * (double) y;
        worldMatrix = this.WorldMatrix;
        this.IsRotating = !worldMatrix.EqualsFast(ref fromQuaternion);
        this.WorldMatrix = fromQuaternion;
        this.ClearShapeContactPoints();
      }
      else
      {
        if (this.Physics.CharacterProxy == null)
          return;
        this.Physics.CharacterProxy.SetForwardAndUp((Vector3) fromQuaternion.Forward, (Vector3) fromQuaternion.Up);
      }
    }

    public override void SerializeControls(BitStream stream)
    {
      if (!this.IsDead)
      {
        stream.WriteBool(true);
        MyCharacterClientState state;
        this.GetNetState(out state);
        state.Serialize(stream);
        if (!MyCompilationSymbols.EnableNetworkPositionTracking)
          return;
        this.MoveIndicator.Equals(Vector3.Zero, 1f / 1000f);
      }
      else
        stream.WriteBool(false);
    }

    public override void DeserializeControls(BitStream stream, bool outOfOrder)
    {
      if (stream.ReadBool())
      {
        MyCharacterClientState characterClientState = new MyCharacterClientState(stream);
        if (outOfOrder)
          return;
        this.m_lastClientState = characterClientState;
        this.SetNetState(ref this.m_lastClientState);
      }
      else
        this.m_lastClientState.Valid = false;
    }

    public override void ResetControls()
    {
      this.ResetMovement();
      this.m_lastClientState.Valid = false;
    }

    public override void ApplyLastControls()
    {
      if (!this.m_lastClientState.Valid || !Sandbox.Game.Multiplayer.Sync.IsServer && this.ControllerInfo.IsLocallyControlled())
        return;
      this.CacheMove(ref this.m_lastClientState.MoveIndicator, ref this.m_lastClientState.Rotation);
    }

    public MyEntity RelativeDampeningEntity
    {
      get => this.m_relativeDampeningEntity;
      set
      {
        if (this.m_relativeDampeningEntity == value)
          return;
        if (this.m_relativeDampeningEntity != null)
          this.m_relativeDampeningEntity.OnClose -= new Action<MyEntity>(this.relativeDampeningEntityClosed);
        this.m_relativeDampeningEntity = value;
        if (this.m_relativeDampeningEntity == null)
          return;
        this.m_relativeDampeningEntity.OnClose += new Action<MyEntity>(this.relativeDampeningEntityClosed);
      }
    }

    public MyTrailProperties LastTrail { get; set; }

    private void relativeDampeningEntityClosed(MyEntity entity) => this.m_relativeDampeningEntity = (MyEntity) null;

    private void SetRelativeDampening(MyEntity entity)
    {
      this.RelativeDampeningEntity = entity;
      this.JetpackComp.EnableDampeners(true);
      this.JetpackComp.TurnOnJetpack(true);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyPlayerCollection.SetDampeningEntityClient)), this.EntityId, this.RelativeDampeningEntity.EntityId);
    }

    private void UpdateOutsideTemperature()
    {
      if (this.Parent is MyCockpit parent && parent.BlockDefinition.IsPressurized && parent.IsWorking)
      {
        this.m_outsideTemperature = 0.5f;
      }
      else
      {
        float temperatureInPoint = MySectorWeatherComponent.GetTemperatureInPoint(this.PositionComp.GetPosition());
        float num = temperatureInPoint;
        if ((long) this.OxygenSourceGridEntityId != 0L)
          num = MathHelper.Lerp(temperatureInPoint, 0.5f, (float) this.OxygenLevelAtCharacterLocation);
        float outsideTemperature = this.m_outsideTemperature;
        this.m_outsideTemperature = num;
        if (MySectorWeatherComponent.TemperatureToLevel(this.m_outsideTemperature) == MySectorWeatherComponent.TemperatureToLevel(outsideTemperature))
          return;
        this.RecalculatePowerRequirement();
      }
    }

    public float GetOutsideTemperature() => this.m_outsideTemperature;

    public override void OnReplicationStarted()
    {
      base.OnReplicationStarted();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted((MyEntity) this);
    }

    public override void OnReplicationEnded()
    {
      base.OnReplicationEnded();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded((MyEntity) this);
    }

    void IMyTrackTrails.AddTrails(MyTrailProperties properties) => ((IMyTrackTrails) this).AddTrails(properties.Position, properties.Normal, properties.ForwardDirection, properties.EntityId, properties.PhysicalMaterial, properties.VoxelMaterial);

    void IMyTrackTrails.AddTrails(
      Vector3D position,
      Vector3D normal,
      Vector3D forwardDirection,
      long m_entity,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial)
    {
      float forMovementState = this.GetMovementMultiplierForMovementState(this.m_currentMovementState);
      float num = (float) (5.59999990463257 * (this.IsCrouching ? 0.200000002980232 : 1.0)) * forMovementState;
      if (this.LastTrail == null)
        this.LastTrail = new MyTrailProperties();
      else if ((this.LastTrail.Position - position).LengthSquared() < (double) num)
        return;
      this.LastTrail.Position = position;
      this.LastTrail.Normal = normal;
      this.LastTrail.ForwardDirection = forwardDirection;
      this.LastTrail.EntityId = m_entity;
      this.LastTrail.PhysicalMaterial = physicalMaterial;
      this.LastTrail.VoxelMaterial = voxelMaterial;
      Vector3D vector3D1 = Vector3D.Cross(normal, forwardDirection) * (this.m_stepLeft ? -0.100000001490116 : 0.100000001490116);
      Vector3D vector3D2 = position + vector3D1;
      MyHitInfo hitInfo = new MyHitInfo()
      {
        Position = vector3D2,
        Normal = (Vector3) normal
      };
      MyStringHash source;
      if (!this.m_stepLeft)
      {
        MyStringHash footprintMirroredDecal = this.Definition.FootprintMirroredDecal;
        source = this.Definition.FootprintMirroredDecal;
      }
      else
        source = this.Definition.FootprintDecal;
      this.m_stepLeft = !this.m_stepLeft;
      MyDecals.HandleAddDecal((VRage.ModAPI.IMyEntity) Sandbox.Game.Entities.MyEntities.GetEntityById(m_entity), hitInfo, (Vector3) forwardDirection, physicalMaterial, source, damage: 30f, voxelMaterial: voxelMaterial, isTrail: true);
    }

    public bool GetPlayerId(out MyPlayer.PlayerId playerId) => Sandbox.Game.Multiplayer.Sync.Players.TryGetPlayerId(this.GetPlayerIdentityId(), out playerId);

    public bool IsConnected(out bool isRealPlayer)
    {
      isRealPlayer = true;
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(this.GetPlayerIdentityId());
      if (playerFaction != null && !playerFaction.AcceptHumans || this.IsDead)
      {
        isRealPlayer = false;
        return false;
      }
      MyPlayer.PlayerId playerId;
      if (this.GetPlayerId(out playerId))
      {
        isRealPlayer = playerId.SerialId == 0;
        if (MySession.Static.Players.TryGetPlayerById(playerId, out MyPlayer _))
          return true;
      }
      return false;
    }

    public void GetOnLadder(MyLadder ladder)
    {
      if (!this.ResponsibleForUpdate(Sandbox.Game.Multiplayer.Sync.Clients.LocalClient))
        return;
      MyMultiplayer.RaiseEvent<MyCharacter, long>(this, (Func<MyCharacter, Action<long>>) (x => new Action<long>(x.GetOnLadder_Request)), ladder.EntityId);
    }

    [Event(null, 80)]
    [Reliable]
    [Server]
    private void GetOnLadder_Request(long ladderId)
    {
      MyLadder entity;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyLadder>(ladderId, out entity))
        return;
      MatrixD worldMatrix = entity.PositionComp.WorldMatrixRef;
      if (!this.CanPlaceCharacter(ref worldMatrix, true, true, (MyEntity) this))
        MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.GetOnLadder_Failed)), new EndpointId(MyEventContext.Current.Sender.Value));
      else
        MyMultiplayer.RaiseEvent<MyCharacter, long, bool, MyObjectBuilder_Character.LadderInfo?>(this, (Func<MyCharacter, Action<long, bool, MyObjectBuilder_Character.LadderInfo?>>) (x => new Action<long, bool, MyObjectBuilder_Character.LadderInfo?>(x.GetOnLadder_Implementation)), entity.EntityId, true, new MyObjectBuilder_Character.LadderInfo?());
    }

    [Event(null, 102)]
    [Reliable]
    [Client]
    private void GetOnLadder_Failed()
    {
      if (this.m_ladderBlockedNotification == null && this == MySession.Static.LocalCharacter)
        this.m_ladderBlockedNotification = new MyHudNotification(MySpaceTexts.NotificationHintLadderBlocked, font: "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_ladderBlockedNotification);
    }

    [Event(null, 112)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [Broadcast]
    private void GetOnLadder_Implementation(
      long ladderId,
      bool resetPosition = true,
      MyObjectBuilder_Character.LadderInfo? newLadderInfo = null)
    {
      MyLadder entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyLadder>(ladderId, out entity) || this.IsOnLadder)
        return;
      if (!this.IsClientPredicted)
      {
        this.ForceDisablePrediction = false;
        this.UpdatePredictionFlag();
      }
      this.MoveIndicator = Vector3.Zero;
      this.ChangeLadder(entity, resetPosition, newLadderInfo);
      this.StopFalling();
      this.SwitchToWeapon((MyToolbarItemWeapon) null, new uint?(), false);
      if (this.JetpackComp != null)
        this.JetpackComp.TurnOnJetpack(false);
      this.SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
      if (this.Physics.CharacterProxy != null)
        this.Physics.CharacterProxy.EnableLadderState(true);
      this.UpdateNearFlag();
      this.Physics.ClearSpeed();
      this.Physics.LinearVelocity = entity.CubeGrid.Physics.GetVelocityAtPoint(this.WorldMatrix.Translation);
      this.m_currentLadderStep = 0;
      this.m_stepsPerAnimation = 59;
      this.m_stepIncrement = 2f * entity.DistanceBetweenPoles / (float) this.m_stepsPerAnimation;
      this.StopUpperAnimation(0.0f);
      this.TriggerCharacterAnimationEvent("GetOnLadder", false);
      if (this.Physics.CharacterProxy != null)
        this.Physics.CharacterProxy.AtLadder = true;
      this.UpdateLadderNotifications();
    }

    public void GetOffLadder() => MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.GetOffLadder_Implementation)));

    [Event(null, 180)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    [Broadcast]
    public void GetOffLadder_Implementation()
    {
      if (!this.IsOnLadder || this.Physics == null || this.IsDead)
        return;
      MyLadder ladder = this.m_ladder;
      this.ChangeLadder((MyLadder) null);
      this.UpdateLadderNotifications();
      if (this.Physics.CharacterProxy != null)
      {
        this.Physics.CharacterProxy.AtLadder = false;
        this.Physics.CharacterProxy.EnableLadderState(false);
      }
      this.m_currentLadderStep = 0;
      this.TriggerCharacterAnimationEvent("GetOffLadder", false);
      if (this.m_currentMovementState == MyCharacterMovementEnum.LadderOut)
      {
        this.m_currentJumpTime = 0.2f;
        this.Stand();
      }
      else
        this.StartFalling();
      Vector3 linearVelocity = ladder.Parent.Physics.LinearVelocity;
      this.Physics.LinearVelocity = linearVelocity;
      if (Vector3.IsZero(linearVelocity))
        return;
      this.SetRelativeDampening(ladder.Parent);
    }

    private void AddLadderConstraint(MyCubeGrid ladderGrid)
    {
      MyCharacterProxy characterProxy = this.Physics.CharacterProxy;
      if (characterProxy == null)
        return;
      characterProxy.GetHitRigidBody().UpdateMotionType(HkMotionType.Dynamic);
      this.m_constraintData = new HkFixedConstraintData();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_constraintBreakableData = new HkBreakableConstraintData((HkConstraintData) this.m_constraintData);
        this.m_constraintBreakableData.ReapplyVelocityOnBreak = false;
        this.m_constraintBreakableData.RemoveFromWorldOnBrake = false;
        this.m_constraintBreakableData.Threshold = 200f;
      }
      else if ((HkReferenceObject) this.m_constraintBreakableData != (HkReferenceObject) null)
        this.m_constraintBreakableData = (HkBreakableConstraintData) null;
      this.m_constraintInstance = new HkConstraint(ladderGrid.Physics.RigidBody, characterProxy.GetHitRigidBody(), (HkConstraintData) this.m_constraintBreakableData ?? (HkConstraintData) this.m_constraintData);
      ladderGrid.Physics.AddConstraint(this.m_constraintInstance);
      this.m_constraintInstance.SetVirtualMassInverse(Vector4.Zero, Vector4.One);
    }

    private void CloseLadderConstraint(MyCubeGrid ladderGrid)
    {
      if (this.Physics.CharacterProxy == null)
        return;
      if ((HkReferenceObject) this.m_constraintInstance != (HkReferenceObject) null && ladderGrid != null)
      {
        ladderGrid.Physics?.RemoveConstraint(this.m_constraintInstance);
        this.m_constraintInstance.Dispose();
        this.m_constraintInstance = (HkConstraint) null;
      }
      if ((HkReferenceObject) this.m_constraintBreakableData != (HkReferenceObject) null)
      {
        this.m_constraintData.Dispose();
        this.m_constraintBreakableData = (HkBreakableConstraintData) null;
      }
      this.m_constraintData = (HkFixedConstraintData) null;
    }

    private void UpdateLadderNotifications()
    {
      if (this != MySession.Static.LocalCharacter)
        return;
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (this.IsOnLadder)
      {
        if (this.m_ladderOffNotification == null)
        {
          this.m_ladderOffNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToGetDownFromLadder, 0, level: MyNotificationLevel.Control);
          if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
            this.m_ladderOffNotification.SetTextFormatArguments((object) ("[" + MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]"));
          else
            this.m_ladderOffNotification.SetTextFormatArguments((object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE));
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_ladderOffNotification);
        }
        if (this.m_ladderUpDownNotification == null)
        {
          this.m_ladderUpDownNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToClimbUpDown, 0, level: MyNotificationLevel.Control);
          if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
            this.m_ladderUpDownNotification.SetTextFormatArguments((object) ("[" + MyInput.Static.GetGameControl(MyControlsSpace.FORWARD).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]"), (object) ("[" + MyInput.Static.GetGameControl(MyControlsSpace.BACKWARD).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]"));
          else
            this.m_ladderUpDownNotification.SetTextFormatArguments((object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.FORWARD), (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.BACKWARD));
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_ladderUpDownNotification);
        }
        if (this.m_ladderJumpOffNotification != null)
          return;
        this.m_ladderJumpOffNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToJumpOffLadder, 0, level: MyNotificationLevel.Control);
        if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
          this.m_ladderJumpOffNotification.SetTextFormatArguments((object) ("[" + MyInput.Static.GetGameControl(MyControlsSpace.JUMP).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]"));
        else
          this.m_ladderJumpOffNotification.SetTextFormatArguments((object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.JUMP));
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_ladderJumpOffNotification);
      }
      else
      {
        if (this.m_ladderOffNotification != null)
        {
          MyHud.Notifications.Remove((MyHudNotificationBase) this.m_ladderOffNotification);
          this.m_ladderOffNotification = (MyHudNotification) null;
        }
        if (this.m_ladderUpDownNotification != null)
        {
          MyHud.Notifications.Remove((MyHudNotificationBase) this.m_ladderUpDownNotification);
          this.m_ladderUpDownNotification = (MyHudNotification) null;
        }
        if (this.m_ladderJumpOffNotification == null)
          return;
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_ladderJumpOffNotification);
        this.m_ladderJumpOffNotification = (MyHudNotification) null;
      }
    }

    public bool IsOnLadder => this.m_ladder != null;

    public MyLadder Ladder => this.m_ladder;

    private void StartStep(bool forceStartAnimation)
    {
      if (this.m_currentLadderStep == 0 | forceStartAnimation)
        this.TriggerCharacterAnimationEvent(this.m_currentMovementState == MyCharacterMovementEnum.LadderUp ? "LadderUp" : "LadderDown", false);
      if (this.m_currentLadderStep != 0)
        return;
      this.m_currentLadderStep = this.m_stepsPerAnimation;
    }

    private void UpdateLadder()
    {
      if (!this.IsOnLadder || this.m_ladder.MarkedForClose || this.m_needReconnectLadder)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && (HkReferenceObject) this.m_constraintInstance != (HkReferenceObject) null && ((HkReferenceObject) this.m_constraintBreakableData != (HkReferenceObject) null && this.m_constraintBreakableData.getIsBroken(this.m_constraintInstance)))
        this.GetOffLadder();
      MatrixD matrixD;
      if (this.m_currentLadderStep > 0)
      {
        Vector3D translation1 = this.PositionComp.WorldMatrixRef.Translation;
        float num = this.m_stepIncrement;
        if (this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderDown)
          num = -num;
        if (this.Physics.CharacterProxy == null && (double) Vector3.Distance(this.m_ladderIncrementToBase, this.m_ladderIncrementToBaseServer) > 0.129999995231628)
          this.m_ladderIncrementToBase = this.m_ladderIncrementToBaseServer;
        this.m_ladderIncrementToBase.Y += num;
        matrixD = this.WorldMatrix;
        Vector3 movementDelta = (Vector3) (matrixD.Up * (double) num);
        bool isHit;
        MyLadder myLadder1 = this.CheckBottomLadder(translation1, ref movementDelta, out isHit);
        MyLadder myLadder2 = this.CheckTopLadder(translation1, ref movementDelta, out isHit);
        if (this.m_currentMovementState == MyCharacterMovementEnum.LadderUp && myLadder2 != null || this.m_currentMovementState == MyCharacterMovementEnum.LadderDown && myLadder1 != null || (this.Physics.CharacterProxy == null || this.m_currentMovementState == MyCharacterMovementEnum.LadderOut))
        {
          Vector3D vector3D1 = translation1;
          matrixD = this.WorldMatrix;
          Vector3D vector3D2 = matrixD.Up * 0.100000001490116;
          MyLadder myLadder3 = this.CheckMiddleLadder(vector3D1 + vector3D2, ref movementDelta);
          Vector3D vector3D3 = translation1;
          matrixD = this.WorldMatrix;
          Vector3D vector3D4 = matrixD.Up * 0.100000001490116;
          MyLadder newLadder = this.CheckMiddleLadder(vector3D3 - vector3D4, ref movementDelta);
          MyLadder myLadder4 = newLadder;
          if (myLadder3 == myLadder4 && newLadder != this.m_ladder && newLadder != null)
            this.ChangeLadder(newLadder);
          if (this.Physics.CharacterProxy != null && this.m_currentLadderStep < 20 && this.m_currentMovementState == MyCharacterMovementEnum.LadderOut)
          {
            Vector3 vector3 = new Vector3(0.0f, 1f / 1000f, 0.025f);
            this.m_ladderIncrementToBase.Y += vector3.Y - num;
            this.m_ladderIncrementToBase.Z += vector3.Z;
          }
          MatrixD characterWM = this.m_baseMatrix * this.m_ladder.WorldMatrix;
          ref MatrixD local1 = ref characterWM;
          Vector3D translation2 = local1.Translation;
          matrixD = this.WorldMatrix;
          Vector3D vector3D5 = matrixD.Up * (double) this.m_ladderIncrementToBase.Y;
          local1.Translation = translation2 + vector3D5;
          ref MatrixD local2 = ref characterWM;
          Vector3D translation3 = local2.Translation;
          matrixD = this.WorldMatrix;
          Vector3D vector3D6 = matrixD.Forward * (double) this.m_ladderIncrementToBase.Z;
          local2.Translation = translation3 + vector3D6;
          if (this.Physics.CharacterProxy != null && (HkReferenceObject) this.m_constraintInstance != (HkReferenceObject) null)
            this.SetCharacterLadderConstraint(characterWM);
        }
        --this.m_currentLadderStep;
        if (this.m_currentLadderStep == 0)
        {
          if (this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderUp || this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderDown)
            this.SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
          else if (this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderOut && Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            Vector3 linearVelocity = this.m_ladder.Parent.Physics.LinearVelocity;
            Vector3 position = this.m_ladder.StopMatrix.Translation;
            matrixD = this.WorldMatrix;
            Vector3 up1 = (Vector3) matrixD.Up;
            matrixD = this.m_ladder.PositionComp.WorldMatrixRef;
            Vector3 up2 = (Vector3) matrixD.Up;
            if ((double) Vector3.Dot(up1, up2) < 0.0)
              position = new Vector3(position.X, -position.Y, position.Z);
            Vector3D vector3D1 = Vector3D.Transform(position, this.m_ladder.WorldMatrix);
            matrixD = this.WorldMatrix;
            Vector3D vector3D2 = matrixD.Up * 0.200000002980232;
            Vector3D pos = vector3D1 - vector3D2;
            this.GetOffLadder();
            this.PositionComp.SetPosition(pos);
            if (Vector3.IsZero(this.Gravity))
            {
              MyPhysicsBody physics = this.Physics;
              Vector3 vector3_1 = linearVelocity;
              matrixD = this.WorldMatrix;
              Vector3D vector3D3 = matrixD.Down * 0.5;
              Vector3 vector3_2 = (Vector3) (vector3_1 + vector3D3);
              physics.LinearVelocity = vector3_2;
            }
          }
        }
      }
      if (this.Physics.CharacterProxy != null || !((HkReferenceObject) this.m_constraintInstance == (HkReferenceObject) null))
        return;
      MatrixD worldMatrix = this.m_baseMatrix * this.m_ladder.WorldMatrix;
      ref MatrixD local3 = ref worldMatrix;
      Vector3D translation4 = local3.Translation;
      matrixD = this.WorldMatrix;
      Vector3D vector3D7 = matrixD.Up * (double) this.m_ladderIncrementToBase.Y;
      local3.Translation = translation4 + vector3D7;
      ref MatrixD local4 = ref worldMatrix;
      Vector3D translation5 = local4.Translation;
      matrixD = this.WorldMatrix;
      Vector3D vector3D8 = matrixD.Forward * (double) this.m_ladderIncrementToBase.Z;
      local4.Translation = translation5 + vector3D8;
      this.PositionComp.SetWorldMatrix(ref worldMatrix);
    }

    private void GetOffLadderFromMovement()
    {
      Vector3D pos = this.PositionComp.GetPosition();
      if (this.IsOnLadder)
      {
        Vector3D position = this.m_ladder.PositionComp.GetPosition();
        MatrixD worldMatrix = this.WorldMatrix;
        Vector3D vector3D1 = worldMatrix.Up * (double) MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) * 0.600000023841858;
        Vector3D vector3D2 = position + vector3D1;
        worldMatrix = this.WorldMatrix;
        Vector3D vector3D3 = worldMatrix.Forward * 0.899999976158142;
        pos = vector3D2 + vector3D3;
      }
      this.GetOffLadder();
      this.PositionComp.SetPosition(pos);
    }

    private MyLadder CheckTopLadder(
      Vector3D position,
      ref Vector3 movementDelta,
      out bool isHit)
    {
      Vector3D from = position + movementDelta + this.WorldMatrix.Up * 1.75 - this.WorldMatrix.Forward * 0.200000002980232;
      Vector3D to = from + this.WorldMatrix.Up * 0.400000005960464 + this.WorldMatrix.Forward * 1.5;
      return this.FindLadder(ref from, ref to, out isHit);
    }

    private MyLadder CheckBottomLadder(
      Vector3D position,
      ref Vector3 movementDelta,
      out bool isHit)
    {
      Vector3D from = position + this.WorldMatrix.Up * 0.200000002980232 + movementDelta - this.WorldMatrix.Forward * 0.200000002980232;
      Vector3D to = from + this.WorldMatrix.Down * 0.400000005960464 + this.WorldMatrix.Forward * 1.5;
      return this.FindLadder(ref from, ref to, out isHit);
    }

    private MyLadder CheckMiddleLadder(Vector3D position, ref Vector3 movementDelta)
    {
      Vector3D from = position + movementDelta + this.WorldMatrix.Up * 0.800000011920929 - this.WorldMatrix.Forward * 0.200000002980232;
      Vector3D to = from + this.WorldMatrix.Forward * 1.5;
      return this.FindLadder(ref from, ref to);
    }

    private MyLadder FindLadder(ref Vector3D from, ref Vector3D to) => this.FindLadder(ref from, ref to, out bool _);

    private MyLadder FindLadder(ref Vector3D from, ref Vector3D to, out bool isHit)
    {
      isHit = false;
      LineD line = new LineD(from, to);
      MyIntersectionResultLineTriangleEx? intersectionWithLine = Sandbox.Game.Entities.MyEntities.GetIntersectionWithLine(ref line, (MyEntity) this, (MyEntity) null, ignoreFloatingObjects: false);
      MyLadder myLadder = (MyLadder) null;
      if (intersectionWithLine.HasValue)
      {
        isHit = true;
        if (intersectionWithLine.Value.Entity is MyCubeGrid)
        {
          if (intersectionWithLine.Value.UserObject != null)
          {
            MySlimBlock cubeBlock = (intersectionWithLine.Value.UserObject as MyCube).CubeBlock;
            if (cubeBlock != null && cubeBlock.FatBlock != null && cubeBlock.FatBlock is MyLadder fatBlock)
              myLadder = fatBlock;
          }
        }
        else if (intersectionWithLine.Value.Entity is MyLadder entity)
          myLadder = entity;
      }
      if (myLadder == null)
        return (MyLadder) null;
      return this.Ladder != null && myLadder != this.Ladder && (myLadder.GetTopMostParent((System.Type) null) == this.Ladder.GetTopMostParent((System.Type) null) && myLadder.Orientation.Forward != this.Ladder.Orientation.Forward) ? (MyLadder) null : myLadder;
    }

    private Vector3 ProceedLadderMovement(Vector3 moveIndicator)
    {
      Vector3D position = this.PositionComp.GetPosition();
      if (this.Physics.CharacterProxy == null)
      {
        MatrixD matrixD = this.m_baseMatrix * this.m_ladder.WorldMatrix;
        matrixD.Translation += this.WorldMatrix.Up * (double) this.m_ladderIncrementToBase.Y;
        matrixD.Translation += this.WorldMatrix.Forward * (double) this.m_ladderIncrementToBase.Z;
        position = matrixD.Translation;
      }
      Vector3 movementDelta = Vector3.Zero;
      if ((double) moveIndicator.Z != 0.0 && this.m_currentLadderStep == 0)
      {
        MatrixD worldMatrix;
        if ((double) moveIndicator.Z < 0.0)
        {
          worldMatrix = this.WorldMatrix;
          movementDelta = (Vector3) (worldMatrix.Up * (double) this.m_stepIncrement * (double) this.m_stepsPerAnimation);
        }
        if ((double) moveIndicator.Z > 0.0)
        {
          worldMatrix = this.WorldMatrix;
          movementDelta = (Vector3) (worldMatrix.Down * (double) this.m_stepIncrement * (double) this.m_stepsPerAnimation);
        }
        bool isHit1;
        MyLadder myLadder1 = this.CheckTopLadder(position, ref movementDelta, out isHit1);
        bool isHit2;
        MyLadder myLadder2 = this.CheckBottomLadder(position, ref movementDelta, out isHit2);
        bool flag = false;
        bool forceStartAnimation = false;
        MyCharacterMovementEnum state = this.GetCurrentMovementState();
        if ((double) moveIndicator.Z < 0.0)
        {
          flag = myLadder1 != null && myLadder1.IsFunctional;
          if (flag && this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderDown)
          {
            this.m_currentLadderStep = this.m_stepsPerAnimation - this.m_currentLadderStep;
            forceStartAnimation = this.m_currentLadderStep > this.m_stepsPerAnimation / 2;
          }
          state = MyCharacterMovementEnum.LadderUp;
        }
        if ((double) moveIndicator.Z > 0.0)
        {
          flag = myLadder2 != null && myLadder2.IsFunctional;
          if (flag && this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderUp)
          {
            this.m_currentLadderStep = this.m_stepsPerAnimation - this.m_currentLadderStep;
            forceStartAnimation = this.m_currentLadderStep > this.m_stepsPerAnimation / 2;
          }
          state = MyCharacterMovementEnum.LadderDown;
        }
        if (flag)
        {
          this.SetCurrentMovementState(state);
          this.StartStep(forceStartAnimation);
        }
        else if (this.Physics.CharacterProxy != null)
        {
          if ((double) moveIndicator.Z < 0.0 && !isHit1)
          {
            if (this.GetCurrentMovementState() != MyCharacterMovementEnum.LadderOut)
            {
              this.m_currentLadderStep = 2 * this.m_stepsPerAnimation + 50;
              this.SetCurrentMovementState(MyCharacterMovementEnum.LadderOut);
              this.TriggerCharacterAnimationEvent("LadderOut", false);
            }
            else
              this.SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
          }
          else if ((double) moveIndicator.Z > 0.0 && !isHit2)
          {
            if (Sandbox.Game.Multiplayer.Sync.IsServer)
              this.GetOffLadder();
          }
          else
            this.SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
        }
        else if (this.m_currentLadderStep == 0 && !isHit1)
          this.m_currentLadderStep = 2 * this.m_stepsPerAnimation + 50;
      }
      return moveIndicator;
    }

    private void SetCharacterLadderConstraint(MatrixD characterWM)
    {
      characterWM.Translation = this.Physics.WorldToCluster(characterWM.Translation) + Vector3D.TransformNormal(this.Physics.Center, characterWM);
      Matrix matrix1 = Matrix.Invert(this.m_ladder.Parent.Physics.RigidBody.GetRigidBodyMatrix());
      Matrix matrix2 = Matrix.Invert((Matrix) ref characterWM);
      Matrix world = Matrix.CreateWorld((Vector3) characterWM.Translation);
      Matrix pivotA = world * matrix1;
      Matrix pivotB = world * matrix2;
      this.m_constraintData.SetInBodySpaceInternal(ref pivotA, ref pivotB);
    }

    private void MyLadder_IsWorkingChanged(MyCubeBlock obj)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || obj.IsWorking)
        return;
      this.GetOffLadder();
    }

    public void ChangeLadder(
      MyLadder newLadder,
      bool resetPosition = false,
      MyObjectBuilder_Character.LadderInfo? newLadderInfo = null)
    {
      if (newLadder == this.m_ladder)
        return;
      MyLadder ladder = this.m_ladder;
      bool flag = true;
      if (ladder != null)
      {
        if (newLadder != null)
          flag = ladder.CubeGrid != newLadder.CubeGrid;
        ladder.IsWorkingChanged -= new Action<MyCubeBlock>(this.MyLadder_IsWorkingChanged);
        ladder.CubeGridChanged -= new Action<MyCubeGrid>(this.Ladder_OnCubeGridChanged);
        ladder.OnClose -= new Action<MyEntity>(this.m_ladder_OnClose);
      }
      if (ladder != null && newLadder != null)
        this.m_baseMatrix = this.m_baseMatrix * ladder.PositionComp.WorldMatrixRef * newLadder.PositionComp.WorldMatrixNormalizedInv;
      this.m_ladder = newLadder;
      if (newLadder != null)
      {
        newLadder.IsWorkingChanged += new Action<MyCubeBlock>(this.MyLadder_IsWorkingChanged);
        newLadder.CubeGridChanged += new Action<MyCubeGrid>(this.Ladder_OnCubeGridChanged);
        newLadder.OnClose += new Action<MyEntity>(this.m_ladder_OnClose);
      }
      if (!flag || this.Physics == null)
        return;
      this.ReconnectConstraint(ladder?.CubeGrid, newLadder?.CubeGrid);
      if (newLadder == null)
        return;
      this.PutCharacterOnLadder(newLadder, resetPosition, newLadderInfo);
    }

    private void m_ladder_OnClose(MyEntity obj)
    {
      if (obj != this.m_ladder)
        return;
      this.GetOffLadder_Implementation();
    }

    private void Ladder_OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      this.m_needReconnectLadder = true;
      this.m_oldLadderGrid = oldGrid;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void PutCharacterOnLadder(
      MyLadder ladder,
      bool resetPosition,
      MyObjectBuilder_Character.LadderInfo? newLadderInfo = null)
    {
      if (!newLadderInfo.HasValue)
      {
        Vector3 translation = (Vector3) (this.WorldMatrix * ladder.PositionComp.WorldMatrixInvScaled).Translation;
        Matrix startMatrix1 = ladder.StartMatrix;
        MatrixD matrixD = MatrixD.Normalize((MatrixD) ref startMatrix1) * MatrixD.CreateRotationY(3.14159297943115);
        float num1 = Vector3.Dot((Vector3) this.WorldMatrix.Up, (Vector3) ladder.PositionComp.WorldMatrixRef.Up);
        float num2 = ladder.StartMatrix.Translation.Y;
        if ((double) num1 < 0.0)
        {
          matrixD *= MatrixD.CreateRotationZ(3.14159297943115);
          num2 = -num2;
        }
        ref MatrixD local = ref matrixD;
        Matrix startMatrix2 = ladder.StartMatrix;
        double x = (double) startMatrix2.Translation.X;
        double y1 = resetPosition ? (double) num2 : (double) translation.Y;
        startMatrix2 = ladder.StartMatrix;
        double z = (double) startMatrix2.Translation.Z;
        Vector3D vector3D = new Vector3D(x, y1, z);
        local.Translation = vector3D;
        double y2 = matrixD.Translation.Y;
        float num3 = this.m_stepIncrement * (float) this.m_currentLadderStep;
        if ((double) num1 < 0.0)
          num3 *= -1f;
        float num4 = num2 + (this.GetCurrentMovementState() == MyCharacterMovementEnum.LadderUp ? -num3 : num3);
        double num5 = (double) num4;
        float num6 = (float) (int) ((y2 - num5) / (double) ladder.DistanceBetweenPoles) * ladder.DistanceBetweenPoles + num4;
        matrixD.Translation = (Vector3D) new Vector3(matrixD.Translation.X, (double) num6, matrixD.Translation.Z);
        if ((double) num1 < 0.0)
          num6 *= -1f;
        this.m_ladderIncrementToBase = Vector3.Zero;
        this.m_ladderIncrementToBase.Y = num6;
        MatrixD worldMatrix = matrixD * ladder.WorldMatrix;
        if (this.Physics.CharacterProxy != null)
          this.Physics.CharacterProxy.ImmediateSetWorldTransform = true;
        matrixD.Translation = new Vector3D(matrixD.Translation.X, 0.0, matrixD.Translation.Z);
        this.m_baseMatrix = matrixD;
        this.PositionComp.SetWorldMatrix(ref worldMatrix);
      }
      else
      {
        Matrix world = Matrix.CreateWorld((Vector3) (Vector3D) newLadderInfo.Value.BaseMatrix.Position, (Vector3) newLadderInfo.Value.BaseMatrix.Forward, (Vector3) newLadderInfo.Value.BaseMatrix.Up);
        this.m_baseMatrix = (MatrixD) ref world;
        this.m_ladderIncrementToBase = (Vector3) newLadderInfo.Value.IncrementToBase;
      }
      if (this.Physics.CharacterProxy == null)
        return;
      this.SetCharacterLadderConstraint(this.PositionComp.WorldMatrixRef);
      this.Physics.CharacterProxy.ImmediateSetWorldTransform = false;
    }

    private void ReconnectConstraint(MyCubeGrid oldLadderGrid, MyCubeGrid newLadderGrid)
    {
      this.CloseLadderConstraint(oldLadderGrid);
      if (newLadderGrid == null)
        return;
      this.AddLadderConstraint(newLadderGrid);
    }

    private bool ShouldCollideWith(MyLadder ladder) => false;

    private void CalculateHandIK(int startBoneIndex, int endBoneIndex, ref MatrixD targetTransform)
    {
      MyCharacterBone characterBone1 = this.AnimationController.CharacterBones[endBoneIndex];
      MyCharacterBone characterBone2 = this.AnimationController.CharacterBones[startBoneIndex];
      List<MyCharacterBone> bones = new List<MyCharacterBone>();
      for (int index = startBoneIndex; index <= endBoneIndex; ++index)
        bones.Add(this.AnimationController.CharacterBones[index]);
      MatrixD matrixD1 = this.PositionComp.WorldMatrixNormalizedInv;
      MatrixD matrixD2 = targetTransform * matrixD1;
      Matrix finalTransform = (Matrix) ref matrixD2;
      Vector3 translation1 = finalTransform.Translation;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
      {
        MyRenderProxy.DebugDrawText3D(targetTransform.Translation, "Hand target transform", Color.Purple, 1f, false);
        MyRenderProxy.DebugDrawSphere(targetTransform.Translation, 0.03f, Color.Purple, depthRead: false);
        MyRenderProxy.DebugDrawAxis(targetTransform, 0.03f, false);
      }
      Vector3 translation2 = (Vector3) targetTransform.Translation;
      MyInverseKinematics.SolveCCDIk(ref translation1, bones, 0.0005f, 5, 0.5f, ref finalTransform, characterBone1);
    }

    private void CalculateHandIK(
      int upperarmIndex,
      int forearmIndex,
      int palmIndex,
      ref MatrixD targetTransform)
    {
      MyCharacterBone[] characterBones = this.AnimationController.CharacterBones;
      MatrixD matrixD1 = this.PositionComp.WorldMatrixNormalizedInv;
      MatrixD matrixD2 = targetTransform * matrixD1;
      Matrix finalTransform = (Matrix) ref matrixD2;
      Vector3 translation = finalTransform.Translation;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
      {
        MyRenderProxy.DebugDrawText3D(targetTransform.Translation, "Hand target transform", Color.Purple, 1f, false);
        MyRenderProxy.DebugDrawSphere(targetTransform.Translation, 0.03f, Color.Purple, depthRead: false);
        MyRenderProxy.DebugDrawAxis(targetTransform, 0.03f, false);
      }
      if (!characterBones.IsValidIndex<MyCharacterBone>(upperarmIndex) || !characterBones.IsValidIndex<MyCharacterBone>(forearmIndex) || !characterBones.IsValidIndex<MyCharacterBone>(palmIndex))
        return;
      MatrixD worldMatrix = this.PositionComp.WorldMatrixRef;
      MyInverseKinematics.SolveTwoJointsIkCCD(characterBones, upperarmIndex, forearmIndex, palmIndex, ref finalTransform, ref worldMatrix, characterBones[palmIndex]);
    }

    public bool ShouldPositionMagazine
    {
      get => this.m_shouldPositionMagazine;
      set
      {
        if (value == this.m_shouldPositionMagazine)
          return;
        this.m_shouldPositionMagazine = value;
        if (value)
          return;
        this.ResetMagazinePosition();
      }
    }

    public bool IsAimAssistSensitivityDecreased
    {
      get => (bool) this.m_isAimAssistSensitivityDecreased;
      set
      {
        if ((bool) this.m_isAimAssistSensitivityDecreased == value)
          return;
        this.m_isAimAssistSensitivityDecreased.Value = value;
      }
    }

    public bool IsAimAssistSnapAllowed
    {
      get => this.m_isAimAssistSnapAllowed;
      private set
      {
        if (this.m_isAimAssistSnapAllowed == value)
          return;
        this.m_isAimAssistSnapAllowed = value;
      }
    }

    public override void UpdateBeforeSimulationParallel()
    {
      base.UpdateBeforeSimulationParallel();
      if (MySession.Static == null)
        return;
      this.UpdatePredictionFlag();
      this.m_previousMovementFlags = this.m_movementFlags;
      this.m_previousNetworkMovementState = this.GetCurrentMovementState();
      this.UpdateZeroMovement();
      this.m_moveAndRotateCalled = false;
      ++this.m_actualUpdateFrame;
      this.UpdateFirstPerson();
      this.UpdateLightPower();
      this.m_currentAnimationChangeDelay += 0.01666667f;
      this.UpdateComponentsBeforeSimulation(true);
      if ((double) this.m_canPlayImpact > 0.0)
        this.m_canPlayImpact -= 0.01666667f;
      if (this.ReverbDetectorComp == null || this != MySession.Static.LocalCharacter)
        return;
      this.ReverbDetectorComp.UpdateParallel();
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.RadioBroadcaster != null)
        this.RadioBroadcaster.Enabled = this.RadioBroadcaster.WantsToBeEnabled && this.m_suitBattery != null && (double) this.m_suitBattery.ResourceSource.CurrentOutput > 0.0;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.IsDead && (!Sandbox.Game.Entities.MyEntities.IsInsideWorld(this.PositionComp.GetPosition()) && MySession.Static.SurvivalMode))
        this.DoDamage(1000f, MyDamageType.Suicide, true, this.EntityId);
      this.UpdateComponentsBeforeSimulation(false);
      if (this.ReverbDetectorComp != null && this == MySession.Static.LocalCharacter)
        this.ReverbDetectorComp.Update();
      if (this.m_resolveHighlightOverlap)
      {
        if (this.ControllerInfo.IsLocallyControlled() && !(this.Parent is MyCockpit))
        {
          MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
          if (highlightSystem != null)
            ((IEnumerable<uint>) this.Render.RenderObjectIDs).ForEach<uint>((Action<uint>) (id => highlightSystem.AddHighlightOverlappingModel(id)));
        }
        this.m_resolveHighlightOverlap = false;
      }
      PerFrameData perFrameData;
      if (MySessionComponentReplay.Static.IsEntityBeingReplayed(this.EntityId, out perFrameData))
      {
        if (perFrameData.SwitchWeaponData.HasValue)
        {
          SerializableDefinitionId? weaponDefinition = perFrameData.SwitchWeaponData.Value.WeaponDefinition;
          this.SwitchToWeaponInternal(weaponDefinition.HasValue ? new MyDefinitionId?((MyDefinitionId) weaponDefinition.GetValueOrDefault()) : new MyDefinitionId?(), false, perFrameData.SwitchWeaponData.Value.InventoryItemId, perFrameData.SwitchWeaponData.Value.WeaponEntityId);
        }
        if (perFrameData.ShootData.HasValue)
        {
          if (perFrameData.ShootData.Value.Begin)
            this.BeginShoot((MyShootActionEnum) perFrameData.ShootData.Value.ShootAction);
          else
            this.EndShoot((MyShootActionEnum) perFrameData.ShootData.Value.ShootAction);
        }
        if (perFrameData.AnimationData.HasValue)
        {
          this.TriggerCharacterAnimationEvent(perFrameData.AnimationData.Value.Animation, false);
          if (!string.IsNullOrEmpty(perFrameData.AnimationData.Value.Animation2))
            this.TriggerCharacterAnimationEvent(perFrameData.AnimationData.Value.Animation2, false);
        }
        if (perFrameData.ControlSwitchesData.HasValue)
        {
          if (perFrameData.ControlSwitchesData.Value.SwitchDamping)
            ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this).SwitchDamping();
          if (perFrameData.ControlSwitchesData.Value.SwitchHelmet)
            ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this).SwitchHelmet();
          if (perFrameData.ControlSwitchesData.Value.SwitchLandingGears)
            ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this).SwitchLandingGears();
          if (perFrameData.ControlSwitchesData.Value.SwitchLights)
            ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this).SwitchLights();
          if (perFrameData.ControlSwitchesData.Value.SwitchReactors)
            ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this).SwitchReactors();
          if (perFrameData.ControlSwitchesData.Value.SwitchThrusts)
            ((VRage.Game.ModAPI.Interfaces.IMyControllableEntity) this).SwitchThrusts();
        }
        if (perFrameData.UseData.HasValue)
        {
          if (perFrameData.UseData.Value.Use)
            this.Use();
          else if (perFrameData.UseData.Value.UseContinues)
            this.UseContinues();
          else if (perFrameData.UseData.Value.UseFinished)
            this.UseFinished();
        }
      }
      this.ShowFactionMemberNames();
    }

    private void ShowFactionMemberNames()
    {
      if (this.m_arePlayerMarkersSet || !MySession.Static.Settings.EnableFactionPlayerNames || (MyGuiScreenHudSpace.Static == null || MySession.Static.LocalHumanPlayer == null))
        return;
      this.m_arePlayerMarkersSet = true;
      long playerIdentityId = this.GetPlayerIdentityId();
      MyPlayer.PlayerId result;
      MySession.Static.Players.TryGetPlayerId(playerIdentityId, out result);
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      if (playerById == null || playerById.IsWildlifeAgent)
        return;
      IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
      if (playerFaction1 == null)
        return;
      if (playerIdentityId == MySession.Static.LocalPlayerId)
      {
        foreach (long key in playerFaction1.Members.Keys)
        {
          if (key != MySession.Static.LocalPlayerId)
          {
            MyIdentity identity = MySession.Static.Players.TryGetIdentity(key);
            if (identity.Character != null && !identity.Character.IsDead)
              MyGuiScreenHudSpace.Static.AddPlayerMarker((MyEntity) identity.Character, MyRelationsBetweenPlayers.Allies, true);
          }
        }
      }
      else
      {
        IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(playerIdentityId);
        if (playerFaction2 == null || playerFaction2 != playerFaction1)
          return;
        MyGuiScreenHudSpace.Static.AddPlayerMarker((MyEntity) this, MyRelationsBetweenPlayers.Allies, true);
      }
    }

    public override void UpdateAfterSimulationParallel()
    {
      base.UpdateAfterSimulationParallel();
      if (!this.IsDead && this.StatComp != null)
        this.StatComp.Update();
      if ((!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS) && !this.IsDead)
        this.UpdateShake();
      if (this.JetpackRunning)
        this.JetpackComp.ClearMovement();
      if (!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS)
      {
        MyCharacterRagdollComponent ragdollComponent = this.Components.Get<MyCharacterRagdollComponent>();
        if (ragdollComponent != null)
          ragdollComponent.Distance = this.m_cameraDistance;
        this.Render.UpdateLightPosition();
        this.UpdateBobQueue();
      }
      else if (this.m_currentWeapon != null && this.WeaponPosition != null)
        this.WeaponPosition.Update();
      this.UpdateComponentsAfterSimulation(true);
      this.m_characterBoneCapsulesReady = false;
      if (this.Physics != null)
        this.m_previousLinearVelocity = this.Physics.LinearVelocity;
      this.m_previousPosition = this.WorldMatrix.Translation;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      MyCharacter.LIGHT_PARAMETERS_CHANGED = false;
      this.UpdateHeadAndWeapon();
      this.UpdateLadder();
      this.UpdateDying();
      if (this.m_aimAssistPhysics != null)
      {
        MatrixD worldMatrix = this.PositionComp.WorldMatrix;
        worldMatrix.Translation = this.m_aimAssistPhysics.WorldToCluster(worldMatrix.Translation);
        this.m_aimAssistPhysics.RigidBody.SetWorldMatrix((Matrix) ref worldMatrix);
        Vector3 zero = Vector3.Zero;
        this.GetLinearVelocity(ref zero);
        this.m_aimAssistPhysics.LinearVelocity = zero;
      }
      this.UpdateAim();
      if (this.IsDead || !this.IsClientPredicted && !Sandbox.Game.Multiplayer.Sync.IsServer && MySession.Static.TopMostControlledEntity == this)
        this.UpdatePhysicalMovement();
      if (!this.IsDead && !Sandbox.Game.Multiplayer.Sync.IsDedicated)
        this.UpdateFallAndSpine();
      if (this.m_needsUpdateBoots)
        this.UpdateBootsStateAndEmmisivity();
      this.UpdateCharacterStateChange();
      this.UpdateRespawnAndLooting();
      this.UpdateShooting();
      this.UpdateComponentsAfterSimulation(false);
      if (this.Physics != null && this.Physics.CharacterProxy == null)
        this.Render.UpdateWalkParticles();
      if (MySession.Static.ControlledEntity == this)
        this.UpdateCharacterMarkers();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.UpdateDynamicRange();
      if (MyControllerHelper.IsControl(this.ControlContext, MyControlsSpace.RELOAD) && MySession.Static.LocalCharacter == this && (this.m_currentWeapon != null && this.m_currentWeapon.CanReload()))
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_currentWeapon.Reload();
        else
          MyMultiplayer.RaiseEvent<MyCharacter>(this, (Func<MyCharacter, Action>) (x => new Action(x.ReloadServer)));
        this.PlayReloadAnimation();
      }
      if (this.ShouldPositionMagazine)
        this.RealignMagazineWithHand();
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated || !this.m_snapTargetChanged || !MyInput.Static.IsJoystickLastUsed)
        return;
      this.m_snapTargetChanged = false;
      this.AimAssistSnap();
    }

    private void UpdateAim()
    {
      if (this.m_aimAssistPhysics == null || !Sandbox.Game.Multiplayer.Sync.IsServer || (!this.IsGameAssistEnabled() || this.ControllerInfo == null) || (this.ControllerInfo.Controller == null || this.ControllerInfo.Controller.Player == null))
        return;
      bool flag1 = false;
      if (this.IsIronSighted)
      {
        MatrixD headMatrix = this.GetHeadMatrix(true, true, false, false, false);
        Vector3D translation = headMatrix.Translation;
        Vector3D to = translation + (double) MyCharacter.AIM_ASSIST_DISTANCE * headMatrix.Forward;
        List<MyPhysics.HitInfo> hitInfoList1 = MyPhysics.CastRay_AllHits(translation, to);
        MyPhysics.HitInfo? nullable = new MyPhysics.HitInfo?();
        MyCharacter myCharacter1 = (MyCharacter) null;
        foreach (MyPhysics.HitInfo hitInfo in hitInfoList1)
        {
          if (!((HkReferenceObject) hitInfo.HkHitInfo.Body == (HkReferenceObject) this.m_aimAssistPhysics.RigidBody))
          {
            VRage.ModAPI.IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
            if (hitEntity != this)
            {
              if (hitEntity is MyCharacter myCharacter)
              {
                if (!myCharacter.IsDead)
                {
                  switch (MyIDModule.GetRelationPlayerPlayer(this.GetPlayerIdentityId(), myCharacter.GetPlayerIdentityId()))
                  {
                    case MyRelationsBetweenPlayers.Self:
                    case MyRelationsBetweenPlayers.Allies:
                      continue;
                    default:
                      nullable = new MyPhysics.HitInfo?(hitInfo);
                      myCharacter1 = myCharacter;
                      goto label_12;
                  }
                }
              }
              else
                break;
            }
          }
        }
label_12:
        if (nullable.HasValue)
        {
          flag1 = true;
          if (this.IsAimAssistSnapAllowed)
          {
            Vector3D aimAssistSnapPoint = myCharacter1.GetAimAssistSnapPoint();
            List<MyPhysics.HitInfo> hitInfoList2 = MyPhysics.CastRay_AllHits(translation, aimAssistSnapPoint);
            bool flag2 = false;
            foreach (MyPhysics.HitInfo hitInfo in hitInfoList2)
            {
              if (!((HkReferenceObject) hitInfo.HkHitInfo.Body == (HkReferenceObject) this.m_aimAssistPhysics.RigidBody))
              {
                VRage.ModAPI.IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
                if (hitEntity != this)
                {
                  if (!(hitEntity is MyCharacter myCharacter))
                  {
                    flag2 = true;
                    break;
                  }
                  if (!myCharacter.IsDead)
                  {
                    if (myCharacter != myCharacter1)
                    {
                      flag2 = true;
                      break;
                    }
                    break;
                  }
                }
              }
            }
            if (!flag2)
              MyMultiplayer.RaiseEvent<MyCharacter, long>(this, (Func<MyCharacter, Action<long>>) (x => new Action<long>(x.ActivateSnap)), myCharacter1.EntityId, new EndpointId(this.ControllerInfo.Controller.Player.Id.SteamId));
          }
        }
        this.IsAimAssistSnapAllowed = false;
      }
      this.IsAimAssistSensitivityDecreased = flag1;
    }

    [Event(null, 566)]
    [Reliable]
    [Client]
    public void ActivateSnap(long targetId)
    {
      this.m_snapTargetChanged = true;
      this.m_snapTarget = targetId;
    }

    public Vector3D GetAimAssistSnapPoint()
    {
      MyCharacterBone chestBone = this.GetChestBone();
      return chestBone != null ? (chestBone.AbsoluteTransform * this.WorldMatrix).Translation : this.PositionComp.WorldAABB.Center;
    }

    private void AimAssistSnap()
    {
      if (this.m_snapTarget == 0L)
        return;
      MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_snapTarget);
      if (entityById == null)
        return;
      int num1 = this.JetpackRunning || this.m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_IN_AIR && this.m_currentCharacterState != (HkCharacterStateType) 5 || (double) this.m_currentJumpTime > 0.0 ? (this.m_currentMovementState != MyCharacterMovementEnum.Died ? 1 : 0) : 0;
      if (this.IsOnLadder)
        return;
      if (this.JetpackRunning)
      {
        MatrixD matrixD = this.GetHeadMatrix(false, false, false, false, false);
        Vector3D translation = matrixD.Translation;
        Vector3D position = this.PositionComp.GetPosition();
        matrixD = this.PositionComp.WorldMatrixRef;
        Vector3 up1 = (Vector3) matrixD.Up;
        matrixD = this.PositionComp.WorldMatrixRef;
        Vector3 forward1 = (Vector3) matrixD.Forward;
        Vector3D zero = Vector3D.Zero;
        Vector3 forward2 = Vector3.Normalize((!(entityById is MyCharacter myCharacter) ? entityById.PositionComp.WorldAABB.Center : myCharacter.GetAimAssistSnapPoint()) - translation);
        Vector3 up2 = up1;
        MatrixD world = MatrixD.CreateWorld(position, forward2, up2);
        this.PositionComp.SetWorldMatrix(ref world);
      }
      else
      {
        Vector3D position = this.PositionComp.GetPosition();
        Vector3D zero = Vector3D.Zero;
        Vector3D vector3D = !(entityById is MyCharacter myCharacter) ? entityById.PositionComp.WorldAABB.Center : myCharacter.GetAimAssistSnapPoint();
        Vector3 up = (Vector3) this.PositionComp.WorldMatrixRef.Up;
        Vector3 vector1 = (Vector3) (vector3D - position);
        Vector3 forward1 = Vector3.Normalize(vector1 - Vector3.Dot(vector1, up) * up);
        MatrixD world = MatrixD.CreateWorld(position, forward1, up);
        this.PositionComp.SetWorldMatrix(ref world);
        MatrixD headMatrix = this.GetHeadMatrix(true, true, false, false, false);
        Vector3 forward2 = (Vector3) headMatrix.Forward;
        Vector3 vector2_1 = Vector3.Normalize(vector3D - headMatrix.Translation);
        Vector3 vector2_2 = Vector3.Cross((Vector3) headMatrix.Right, vector2_1);
        float num2 = (float) Math.Acos((double) Vector3.Dot(forward2, vector2_1));
        this.SetHeadLocalXAngle(this.HeadLocalXAngle - (float) ((double) Math.Sign(Vector3.Dot(forward2, vector2_2)) * (double) num2 * 57.2957992553711));
      }
    }

    public void PlayReloadAnimation(bool forceAnimation = false)
    {
      this.TriggerCharacterAnimationEvent("reload_weapon", this == MySession.Static.LocalCharacter);
      if (this.m_currentWeapon == null || !forceAnimation && !this.m_currentWeapon.CanReload())
        return;
      float num = this.m_currentWeapon.GetReloadDuration();
      if ((double) num == 0.0)
        num = 1f;
      List<MyAnimationTreeNode> keyedAnimationTracks = this.AnimationController.GetKeyedAnimationTracks(MyCharacter.TRACK_KEY_RELOAD);
      if (keyedAnimationTracks != null && keyedAnimationTracks.Count > 0)
      {
        foreach (MyAnimationTreeNode animationTreeNode in keyedAnimationTracks)
        {
          if (animationTreeNode is MyAnimationTreeNodeTrack animationTreeNodeTrack)
          {
            float clipDuration = (float) animationTreeNodeTrack.GetClipDuration();
            animationTreeNodeTrack.Speed = (double) clipDuration / (double) num;
          }
        }
      }
      this.m_currentWeapon.PlayReloadSound();
    }

    [Event(null, 680)]
    [Reliable]
    [Server]
    public void ReloadServer()
    {
      ulong controlSteamId = this.ControlSteamId;
      if (controlSteamId <= 0UL || (long) controlSteamId != (long) MyEventContext.Current.Sender.Value || (this.m_currentWeapon == null || !this.m_currentWeapon.CanReload()))
        return;
      this.m_currentWeapon.Reload();
    }

    private void ResetMagazinePosition()
    {
      if (this.m_currentWeapon == null || !(this.m_currentWeapon is MyAutomaticRifleGun mCurrentWeapon))
        return;
      mCurrentWeapon.ResetMagazinePosition();
    }

    private void RealignMagazineWithHand()
    {
      if (this.m_currentWeapon == null || !(this.m_currentWeapon is MyAutomaticRifleGun mCurrentWeapon))
        return;
      MyCharacterBone leftHandBone = this.GetLeftHandBone();
      if (leftHandBone == null)
        return;
      MatrixD mat = leftHandBone.AbsoluteTransform * this.WorldMatrix;
      mCurrentWeapon.SetMagazinePosition(mat);
    }

    private void UpdateComponentsBeforeSimulation(bool parallel)
    {
      foreach (MyComponentBase component in (MyComponentContainer) this.Components)
      {
        if (component is MyCharacterComponent characterComponent)
        {
          if (parallel && characterComponent.NeedsUpdateBeforeSimulationParallel)
            characterComponent.UpdateBeforeSimulationParallel();
          else if (!parallel && characterComponent.NeedsUpdateBeforeSimulation)
            characterComponent.UpdateBeforeSimulation();
        }
      }
    }

    private void UpdateComponentsAfterSimulation(bool parallel)
    {
      foreach (MyComponentBase component in (MyComponentContainer) this.Components)
      {
        if (component is MyCharacterComponent characterComponent)
        {
          if (parallel && characterComponent.NeedsUpdateAfterSimulationParallel)
            characterComponent.UpdateAfterSimulationParallel();
          else if (!parallel && characterComponent.NeedsUpdateAfterSimulation)
            characterComponent.UpdateAfterSimulation();
        }
      }
    }

    private void InitAnimations()
    {
      this.m_animationSpeedFilterCursor = 0;
      for (int index = 0; index < this.m_animationSpeedFilter.Length; ++index)
        this.m_animationSpeedFilter[index] = Vector3.Zero;
      foreach (KeyValuePair<string, string[]> boneSet in this.m_characterDefinition.BoneSets)
        this.AddAnimationPlayer(boneSet.Key, boneSet.Value);
      this.SetBoneLODs(this.m_characterDefinition.BoneLODs);
      this.AnimationController.FindBone(this.m_characterDefinition.HeadBone, out this.m_headBoneIndex);
      this.AnimationController.FindBone(this.m_characterDefinition.Camera3rdBone, out this.m_camera3rdBoneIndex);
      if (this.m_camera3rdBoneIndex == -1)
        this.m_camera3rdBoneIndex = this.m_headBoneIndex;
      this.AnimationController.FindBone(this.m_characterDefinition.LeftHandIKStartBone, out this.m_leftHandIKStartBone);
      this.AnimationController.FindBone(this.m_characterDefinition.LeftHandIKEndBone, out this.m_leftHandIKEndBone);
      this.AnimationController.FindBone(this.m_characterDefinition.RightHandIKStartBone, out this.m_rightHandIKStartBone);
      this.AnimationController.FindBone(this.m_characterDefinition.RightHandIKEndBone, out this.m_rightHandIKEndBone);
      this.AnimationController.FindBone(this.m_characterDefinition.LeftUpperarmBone, out this.m_leftUpperarmBone);
      this.AnimationController.FindBone(this.m_characterDefinition.LeftForearmBone, out this.m_leftForearmBone);
      this.AnimationController.FindBone(this.m_characterDefinition.RightUpperarmBone, out this.m_rightUpperarmBone);
      this.AnimationController.FindBone(this.m_characterDefinition.RightForearmBone, out this.m_rightForearmBone);
      this.AnimationController.FindBone(this.m_characterDefinition.WeaponBone, out this.m_weaponBone);
      this.AnimationController.FindBone(this.m_characterDefinition.LeftHandItemBone, out this.m_leftHandItemBone);
      this.AnimationController.FindBone(this.m_characterDefinition.RighHandItemBone, out this.m_rightHandItemBone);
      this.AnimationController.FindBone(this.m_characterDefinition.SpineBone, out this.m_spineBone);
      this.AnimationController.Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeedLt, out this.m_walkRunSpeed);
      this.AnimationController.Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeedUt, out this.m_runSprintSpeed);
      this.AnimationController.FindBone(MyCharacter.MAGAZINE_DUMMY_BONE_NAME, out this.m_magazineDummyBone);
      this.AnimationController.FindBone(MyCharacter.CHEST_DUMMY_BONE_NAME, out this.m_chestDummyBone);
      this.UpdateAnimation(0.0f);
    }

    protected override void CalculateTransforms(float distance)
    {
      base.CalculateTransforms(distance);
      if (this.IsOnLadder)
        this.m_wasOnLadder = 100;
      else if (this.m_wasOnLadder > 0)
        --this.m_wasOnLadder;
      if (!this.Entity.InScene || this.m_wasOnLadder != 0 || !MySession.Static.HighSimulationQuality)
        return;
      this.AnimationController.UpdateInverseKinematics();
    }

    private void UpdateHeadAndWeapon()
    {
      bool flag1 = this.IsInFirstPersonView && MySession.Static.CameraController == this;
      bool flag2 = flag1 || this.ForceFirstPersonCamera;
      Vector3 vector3 = Vector3.Zero;
      if (((this.m_headBoneIndex < 0 ? 0 : (this.AnimationController.CharacterBones != null ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 && MySession.Static.CameraController == this && (!this.IsBot && !this.IsOnLadder))
      {
        vector3 = this.AnimationController.CharacterBones[this.m_headBoneIndex].AbsoluteTransform.Translation;
        vector3.Y = 0.0f;
        MyCharacterBone.TranslateAllBones(this.AnimationController.CharacterBones, -vector3);
      }
      if (this.m_leftHandItem != null)
        this.UpdateLeftHandItemPosition();
      if (this.m_currentWeapon == null || this.WeaponPosition == null || this.m_handItemDefinition == null)
        return;
      bool flag3 = false;
      this.WeaponPosition.Update();
      int num1 = flag2 ? (this.m_handItemDefinition.SimulateLeftHandFps ? 1 : 0) : (this.m_handItemDefinition.SimulateLeftHand ? 1 : 0);
      int num2 = this.m_currentWeapon.IsReloading != this.m_isReloadingPrevious ? 1 : 0;
      bool flag4 = this.ShouldPositionMagazine != this.m_shouldPositionHandPrevious;
      this.m_isReloadingPrevious = this.m_currentWeapon.IsReloading;
      this.m_shouldPositionHandPrevious = this.ShouldPositionMagazine;
      if (num2 != 0 && this.m_currentWeapon.IsReloading)
        this.m_reloadHandSimulationState = false;
      if (flag4 && !this.ShouldPositionMagazine)
        this.m_reloadHandSimulationState = true;
      int num3 = this.m_reloadHandSimulationState ? 1 : 0;
      if ((num1 & num3) != 0 && this.m_leftHandIKStartBone != -1 && this.m_leftHandIKEndBone != -1)
      {
        MatrixD targetTransform = (MatrixD) ref this.m_handItemDefinition.LeftHand * ((MyEntity) this.m_currentWeapon).WorldMatrix;
        this.CalculateHandIK(this.m_leftHandIKStartBone, this.m_leftForearmBone, this.m_leftHandIKEndBone, ref targetTransform);
        flag3 = true;
      }
      bool flag5 = flag2 ? this.m_handItemDefinition.SimulateRightHandFps : this.m_handItemDefinition.SimulateRightHand;
      if (this.m_rightHandIKStartBone != -1 && this.m_rightHandIKEndBone != -1 && !this.IsSitting)
      {
        if (flag5)
        {
          MatrixD targetTransform = (MatrixD) ref this.m_handItemDefinition.RightHand * ((MyEntity) this.m_currentWeapon).WorldMatrix;
          this.CalculateHandIK(this.m_rightHandIKStartBone, this.m_rightForearmBone, this.m_rightHandIKEndBone, ref targetTransform);
        }
        else if (((!this.m_handItemDefinition.SimulateRightHand ? 0 : (!this.m_handItemDefinition.SimulateRightHandFps ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
        {
          Matrix absoluteRigTransform = this.AnimationController.CharacterBones[this.SpineBoneIndex].GetAbsoluteRigTransform();
          absoluteRigTransform.Translation -= 2f * vector3;
          this.AnimationController.CharacterBones[this.m_rightHandIKEndBone].SetCompleteBindTransform();
          this.AnimationController.CharacterBones[this.m_rightForearmBone].SetCompleteBindTransform();
          this.AnimationController.CharacterBones[this.m_rightHandIKStartBone].SetCompleteTransformFromAbsoluteMatrix(ref absoluteRigTransform, false);
        }
        flag3 = true;
      }
      if (!flag3)
        return;
      this.AnimationController.UpdateTransformations();
    }

    public MyCharacterBone GetLeftHandBone() => this.m_magazineDummyBone < 0 || this.m_magazineDummyBone >= this.AnimationController.CharacterBones.Length ? (MyCharacterBone) null : this.AnimationController.CharacterBones[this.m_magazineDummyBone];

    public MyCharacterBone GetChestBone() => this.m_chestDummyBone < 0 || this.m_chestDummyBone >= this.AnimationController.CharacterBones.Length ? (MyCharacterBone) null : this.AnimationController.CharacterBones[this.m_chestDummyBone];

    public override void UpdateControl(float distance)
    {
      base.UpdateControl(distance);
      if ((double) distance >= (double) MyFakes.ANIMATION_UPDATE_DISTANCE || Sandbox.Engine.Platform.Game.IsDedicated || !this.UseNewAnimationSystem)
        return;
      this.UpdateAnimationNewSystem();
    }

    public override void UpdateAnimation(float distance)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated && MyPerGameSettings.DisableAnimationsOnDS)
        return;
      if ((double) distance < (double) MyFakes.ANIMATION_UPDATE_DISTANCE)
      {
        base.UpdateAnimation(distance);
        MyAnimationPlayerBlendPair player;
        if (this.TryGetAnimationPlayer("LeftHand", out player) && player.GetState() == MyAnimationPlayerBlendPair.AnimationBlendState.Stopped && (this.m_leftHandItem != null && !this.UseNewAnimationSystem))
        {
          this.m_leftHandItem.Close();
          this.m_leftHandItem = (MyEntity) null;
        }
        this.Render.UpdateThrustMatrices(this.BoneAbsoluteTransforms);
        if (!this.m_resetWeaponAnimationState)
          return;
        this.m_resetWeaponAnimationState = false;
      }
      else
        this.WeaponPosition.Update();
    }

    private void GetSpeeds(
      out float localSpeedX,
      out float localSpeedY,
      out float localSpeedZ,
      out float speed,
      out Vector3 localSpeedWorldRot)
    {
      Vector3 vector3_1 = this.Physics.LinearVelocity * Vector3.TransformNormal(this.m_currentMovementDirection, this.WorldMatrix);
      MyCharacterProxy characterProxy = this.Physics.CharacterProxy;
      if (Sandbox.Game.Multiplayer.Sync.IsServer || MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER)
      {
        if (characterProxy != null)
        {
          Vector3 linearVelocityLocal = this.Physics.LinearVelocityLocal;
          Vector3 groundVelocity = characterProxy.GroundVelocity;
          Vector3 interpolatedVelocity = characterProxy.CharacterRigidBody.InterpolatedVelocity;
          double num1 = (double) (interpolatedVelocity - groundVelocity).LengthSquared();
          Vector3 vector3_2 = linearVelocityLocal - groundVelocity;
          double num2 = (double) vector3_2.LengthSquared();
          vector3_1 = (num1 >= num2 ? linearVelocityLocal : interpolatedVelocity) - groundVelocity;
          if (this.GetCurrentMovementState() == MyCharacterMovementEnum.Standing)
          {
            vector3_2 = characterProxy.Up;
            float num3 = vector3_2.Dot(vector3_1);
            if ((double) num3 < 0.0)
              vector3_1 -= characterProxy.Up * num3;
          }
        }
        else
          vector3_1 = this.Physics.LinearVelocityLocal;
      }
      localSpeedWorldRot = this.FilterLocalSpeed(vector3_1);
      MatrixD matrixD = this.PositionComp.WorldMatrixRef;
      localSpeedX = localSpeedWorldRot.Dot((Vector3) matrixD.Right);
      localSpeedY = localSpeedWorldRot.Dot((Vector3) matrixD.Up);
      localSpeedZ = localSpeedWorldRot.Dot((Vector3) matrixD.Forward);
      speed = (float) Math.Sqrt((double) localSpeedX * (double) localSpeedX + (double) localSpeedZ * (double) localSpeedZ);
    }

    private void UpdateAnimationNewSystem()
    {
      IMyVariableStorage<float> variables = this.AnimationController.Variables;
      MyCharacterMovementEnum currentMovementState = this.GetCurrentMovementState();
      if (this.Physics != null)
      {
        float localSpeedX;
        float localSpeedY;
        float localSpeedZ;
        float speed;
        Vector3 localSpeedWorldRot;
        this.GetSpeeds(out localSpeedX, out localSpeedY, out localSpeedZ, out speed, out localSpeedWorldRot);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeed, speed);
        float num1 = MySession.Static.Settings.CharacterSpeedMultiplier * MyFakes.CHARACTER_ANIMATION_SPEED;
        this.AnimationController.MainLayerAnimationSpeed = num1;
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedLt, this.m_walkRunSpeed * num1);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedUt, this.m_runSprintSpeed * num1);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedX, localSpeedX);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedY, localSpeedY);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedZ, localSpeedZ);
        float newValue1 = (double) localSpeedWorldRot.LengthSquared() > 0.00250000017695129 ? (float) (-Math.Atan2((double) localSpeedZ, (double) localSpeedX) * 180.0 / Math.PI + 90.0) : 0.0f;
        while ((double) newValue1 < 0.0)
          newValue1 += 360f;
        variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedAngle, newValue1);
        if (this.Physics.CharacterProxy != null)
        {
          Vector3 axis = Vector3.Zero;
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            axis = this.Physics.CharacterProxy.GroundAngularVelocity;
          }
          else
          {
            using (MyUtils.ReuseCollection<MyEntity>(ref MyCharacter.m_supportingEntities))
            {
              int num2 = 0;
              this.Physics.CharacterProxy.GetSupportingEntities(MyCharacter.m_supportingEntities);
              foreach (MyEntity supportingEntity in MyCharacter.m_supportingEntities)
              {
                MyPhysicsComponentBase physics = supportingEntity.Physics;
                if (physics != null)
                {
                  ++num2;
                  axis += physics.AngularVelocityLocal;
                }
              }
              if (num2 != 0)
                axis /= (float) num2;
            }
          }
          this.m_lastRotation *= Quaternion.CreateFromAxisAngle(axis, 0.01666667f);
        }
        Quaternion rotation = this.GetRotation();
        float newValue2 = (float) ((double) (Quaternion.Inverse(rotation) * this.m_lastRotation).Y / 0.0012999998871237 * 180.0 / 3.14159274101257);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdTurningSpeed, newValue2);
        this.m_lastRotation = rotation;
        if (this.OxygenComponent != null)
          variables.SetValue(MyAnimationVariableStorageHints.StrIdHelmetOpen, this.OxygenComponent.HelmetEnabled ? 0.0f : 1f);
        if (this.Parent is MyCockpit || this.IsOnLadder)
          variables.SetValue(MyAnimationVariableStorageHints.StrIdLean, 0.0f);
        else
          variables.SetValue(MyAnimationVariableStorageHints.StrIdLean, (float) this.m_animLeaning);
      }
      bool flag1 = MySession.Static.CameraController == this;
      bool flag2 = ((this.m_isInFirstPerson ? 1 : (this.ForceFirstPersonCamera ? 1 : 0)) & (flag1 ? 1 : 0)) != 0;
      if (this.JetpackComp != null)
        variables.SetValue(MyAnimationVariableStorageHints.StrIdFlying, this.JetpackComp.Running ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdFlying, currentMovementState == MyCharacterMovementEnum.Flying ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdFalling, this.IsFalling || currentMovementState == MyCharacterMovementEnum.Falling ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdCrouch, !this.WantsCrouch || this.WantsSprint ? 0.0f : 1f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdSitting, currentMovementState == MyCharacterMovementEnum.Sitting ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdJumping, currentMovementState == MyCharacterMovementEnum.Jump ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdFirstPerson, flag2 ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdForcedFirstPerson, this.ForceFirstPersonCamera ? 1f : 0.0f);
      variables.SetValue(MyAnimationVariableStorageHints.StrIdHoldingTool, this.m_currentWeapon != null ? 1f : 0.0f);
      if (this.WeaponPosition != null)
      {
        variables.SetValue(MyAnimationVariableStorageHints.StrIdShooting, this.m_currentWeapon == null || !this.WeaponPosition.IsShooting || this.WeaponPosition.ShouldSupressShootAnimation ? 0.0f : 1f);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdIronsight, this.WeaponPosition.IsInIronSight ? 1f : 0.0f);
      }
      else
      {
        variables.SetValue(MyAnimationVariableStorageHints.StrIdShooting, 0.0f);
        variables.SetValue(MyAnimationVariableStorageHints.StrIdIronsight, 0.0f);
      }
      variables.SetValue(MyAnimationVariableStorageHints.StrIdLadder, this.IsOnLadder ? 1f : 0.0f);
    }

    private Vector3 FilterLocalSpeed(Vector3 localSpeedWorldRotUnfiltered) => localSpeedWorldRotUnfiltered;

    protected override void OnAnimationPlay(
      MyAnimationDefinition animDefinition,
      MyAnimationCommand command,
      ref string bonesArea,
      ref MyFrameOption frameOption,
      ref bool useFirstPersonVersion)
    {
      switch (this.GetCurrentMovementState())
      {
        case MyCharacterMovementEnum.Standing:
        case MyCharacterMovementEnum.RotatingLeft:
        case MyCharacterMovementEnum.RotatingRight:
          useFirstPersonVersion = this.IsInFirstPersonView;
          if (!animDefinition.AllowWithWeapon)
            break;
          this.m_resetWeaponAnimationState = true;
          break;
        default:
          if (command.ExcludeLegsWhenMoving)
          {
            bonesArea = MyCharacter.TopBody;
            frameOption = frameOption != MyFrameOption.JustFirstFrame ? MyFrameOption.PlayOnce : frameOption;
            goto case MyCharacterMovementEnum.Standing;
          }
          else
            goto case MyCharacterMovementEnum.Standing;
      }
    }

    private void StopUpperAnimation(float blendTime)
    {
      this.PlayerStop("Head", blendTime);
      this.PlayerStop("Spine", blendTime);
      this.PlayerStop("LeftHand", blendTime);
      this.PlayerStop("RightHand", blendTime);
    }

    private void StopFingersAnimation(float blendTime)
    {
      this.PlayerStop("LeftFingers", blendTime);
      this.PlayerStop("RightFingers", blendTime);
    }

    public override void AddCommand(MyAnimationCommand command, bool sync = false)
    {
      if (this.UseNewAnimationSystem)
        return;
      base.AddCommand(command, sync);
      if (!sync)
        return;
      this.SendAnimationCommand(ref command);
    }

    public void SetSpineAdditionalRotation(
      Quaternion rotation,
      Quaternion rotationForClients,
      bool updateSync = true)
    {
      if (string.IsNullOrEmpty(this.Definition.SpineBone) || !(this.GetAdditionalRotation(this.Definition.SpineBone) != rotation))
        return;
      this.m_additionalRotations[this.Definition.SpineBone] = rotation;
    }

    public void SetHeadAdditionalRotation(Quaternion rotation, bool updateSync = true)
    {
      if (string.IsNullOrEmpty(this.Definition.HeadBone) || !(this.GetAdditionalRotation(this.Definition.HeadBone) != rotation))
        return;
      this.m_additionalRotations[this.Definition.HeadBone] = rotation;
    }

    public void SetHandAdditionalRotation(Quaternion rotation, bool updateSync = true)
    {
      if (string.IsNullOrEmpty(this.Definition.LeftForearmBone) || !(this.GetAdditionalRotation(this.Definition.LeftForearmBone) != rotation))
        return;
      this.m_additionalRotations[this.Definition.LeftForearmBone] = rotation;
      this.m_additionalRotations[this.Definition.RightForearmBone] = Quaternion.Inverse(rotation);
    }

    public void SetUpperHandAdditionalRotation(Quaternion rotation, bool updateSync = true)
    {
      if (string.IsNullOrEmpty(this.Definition.LeftUpperarmBone) || !(this.GetAdditionalRotation(this.Definition.LeftUpperarmBone) != rotation))
        return;
      this.m_additionalRotations[this.Definition.LeftUpperarmBone] = rotation;
      this.m_additionalRotations[this.Definition.RightUpperarmBone] = Quaternion.Inverse(rotation);
    }

    public bool HasAnimation(string animationName) => this.Definition.AnimationNameToSubtypeName.ContainsKey(animationName);

    public void DisableAnimationCommands() => this.m_animationCommandsEnabled = false;

    public void EnableAnimationCommands() => this.m_animationCommandsEnabled = true;

    public void TriggerCharacterAnimationEvent(string eventName, bool sync)
    {
      if (!this.UseNewAnimationSystem || string.IsNullOrEmpty(eventName))
        return;
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
      {
        PerFrameData data = new PerFrameData()
        {
          AnimationData = new AnimationData?(new AnimationData()
          {
            Animation = eventName
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, data);
      }
      if (sync)
        this.SendAnimationEvent(eventName);
      else
        this.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName));
    }

    public void TriggerCharacterAnimationEvent(string eventName, bool sync, string[] layers)
    {
      if (!this.UseNewAnimationSystem || string.IsNullOrEmpty(eventName) || this.IsDead)
        return;
      if (MySessionComponentReplay.Static.IsEntityBeingRecorded(this.EntityId))
      {
        PerFrameData data = new PerFrameData()
        {
          AnimationData = new AnimationData?(new AnimationData()
          {
            Animation = eventName
          })
        };
        MySessionComponentReplay.Static.ProvideEntityRecordData(this.EntityId, data);
      }
      if (sync)
        this.SendAnimationEvent(eventName, layers);
      else
        this.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName), layers);
    }

    public void PlayCharacterAnimation(
      string animationName,
      MyBlendOption blendOption,
      MyFrameOption frameOption,
      float blendTime,
      float timeScale = 1f,
      bool sync = false,
      string influenceArea = null,
      bool excludeLegsWhenMoving = false)
    {
      if (this.UseNewAnimationSystem)
        return;
      bool flag = Sandbox.Engine.Platform.Game.IsDedicated && MyPerGameSettings.DisableAnimationsOnDS;
      if (flag && !sync || (!this.m_animationCommandsEnabled || animationName == null))
        return;
      string str = (string) null;
      if (!this.m_characterDefinition.AnimationNameToSubtypeName.TryGetValue(animationName, out str))
        str = animationName;
      MyAnimationCommand command = new MyAnimationCommand()
      {
        AnimationSubtypeName = str,
        PlaybackCommand = MyPlaybackCommand.Play,
        BlendOption = blendOption,
        FrameOption = frameOption,
        BlendTime = blendTime,
        TimeScale = timeScale,
        Area = influenceArea,
        ExcludeLegsWhenMoving = excludeLegsWhenMoving
      };
      if (sync)
      {
        this.SendAnimationCommand(ref command);
      }
      else
      {
        if (flag)
          return;
        this.AddCommand(command, sync);
      }
    }

    public void StopUpperCharacterAnimation(float blendTime)
    {
      if (this.UseNewAnimationSystem)
        return;
      this.AddCommand(new MyAnimationCommand()
      {
        AnimationSubtypeName = (string) null,
        PlaybackCommand = MyPlaybackCommand.Stop,
        Area = MyCharacter.TopBody,
        BlendTime = blendTime,
        TimeScale = 1f
      }, false);
    }

    public void StopLowerCharacterAnimation(float blendTime)
    {
      if (this.UseNewAnimationSystem)
        return;
      this.AddCommand(new MyAnimationCommand()
      {
        AnimationSubtypeName = (string) null,
        PlaybackCommand = MyPlaybackCommand.Stop,
        Area = "LowerBody",
        BlendTime = blendTime,
        TimeScale = 1f
      }, false);
    }

    [Event(null, 756)]
    [Reliable]
    [Broadcast]
    public void CreateBurrowingParticleFX_Client(Vector3D position)
    {
      MyParticleEffect effect;
      if (!MyParticlesManager.TryCreateParticleEffect("Burrowing", MatrixD.CreateTranslation(position), out effect))
        return;
      effect.UserScale = 2f;
      MyCharacter.m_burrowEffectTable[position] = effect;
    }

    [Event(null, 768)]
    [Reliable]
    [Broadcast]
    public void DeleteBurrowingParticleFX_Client(Vector3D position)
    {
      MyParticleEffect myParticleEffect;
      if (!MyCharacter.m_burrowEffectTable.TryGetValue(position, out myParticleEffect))
        return;
      myParticleEffect.StopEmitting();
      MyCharacter.m_burrowEffectTable.Remove(position);
    }

    [Event(null, 779)]
    [Broadcast]
    [Reliable]
    public void TriggerAnimationEvent(string eventName) => this.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName));

    VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => (VRage.ModAPI.IMyEntity) this.Entity;

    public Vector3 LastMotionIndicator { get; set; }

    public Vector3 LastRotationIndicator { get; set; }

    IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => (IMyControllerInfo) this.ControllerInfo;

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(
      IMyCameraController camera,
      long playerId)
    {
      if (camera == null)
        return;
      this.DrawHud(camera, playerId);
    }

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => false;
      set => throw new NotImplementedException();
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    [Serializable]
    protected struct TrailContactProperties
    {
      public long ContactEntityId;
      public Vector3 ContactPosition;
      public MyStringHash PhysicalMaterial;
      public MyStringHash VoxelMaterial;

      protected class Sandbox_Game_Entities_Character_MyCharacter\u003C\u003ETrailContactProperties\u003C\u003EContactEntityId\u003C\u003EAccessor : IMemberAccessor<MyCharacter.TrailContactProperties, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCharacter.TrailContactProperties owner, in long value) => owner.ContactEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCharacter.TrailContactProperties owner, out long value) => value = owner.ContactEntityId;
      }

      protected class Sandbox_Game_Entities_Character_MyCharacter\u003C\u003ETrailContactProperties\u003C\u003EContactPosition\u003C\u003EAccessor : IMemberAccessor<MyCharacter.TrailContactProperties, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCharacter.TrailContactProperties owner, in Vector3 value) => owner.ContactPosition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCharacter.TrailContactProperties owner, out Vector3 value) => value = owner.ContactPosition;
      }

      protected class Sandbox_Game_Entities_Character_MyCharacter\u003C\u003ETrailContactProperties\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : IMemberAccessor<MyCharacter.TrailContactProperties, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCharacter.TrailContactProperties owner, in MyStringHash value) => owner.PhysicalMaterial = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCharacter.TrailContactProperties owner,
          out MyStringHash value)
        {
          value = owner.PhysicalMaterial;
        }
      }

      protected class Sandbox_Game_Entities_Character_MyCharacter\u003C\u003ETrailContactProperties\u003C\u003EVoxelMaterial\u003C\u003EAccessor : IMemberAccessor<MyCharacter.TrailContactProperties, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCharacter.TrailContactProperties owner, in MyStringHash value) => owner.VoxelMaterial = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCharacter.TrailContactProperties owner,
          out MyStringHash value)
        {
          value = owner.VoxelMaterial;
        }
      }
    }

    public delegate void ControlStateChanged(bool hasControl);

    private class MyCharacterPosition : MyPositionComponent
    {
      private const int CHECK_FREQUENCY = 20;
      private int m_checkOutOfWorldCounter;

      protected override void OnWorldPositionChanged(
        object source,
        bool updateChildren,
        bool forceUpdateAllChildren)
      {
        this.ClampToWorld();
        base.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
      }

      private void ClampToWorld()
      {
        if (!MySession.Static.WorldBoundaries.HasValue)
          return;
        ++this.m_checkOutOfWorldCounter;
        if (this.m_checkOutOfWorldCounter <= 20)
          return;
        Vector3D position = this.GetPosition();
        Vector3D min = MySession.Static.WorldBoundaries.Value.Min;
        Vector3D max = MySession.Static.WorldBoundaries.Value.Max;
        Vector3D vector3D1 = position - Vector3.One * 10f;
        Vector3D vector3D2 = position + Vector3.One * 10f;
        if (vector3D1.X >= min.X && vector3D1.Y >= min.Y && (vector3D1.Z >= min.Z && vector3D2.X <= max.X) && (vector3D2.Y <= max.Y && vector3D2.Z <= max.Z))
        {
          this.m_checkOutOfWorldCounter = 0;
        }
        else
        {
          Vector3 linearVelocity = this.Container.Entity.Physics.LinearVelocity;
          bool flag = false;
          if (position.X < min.X || position.X > max.X)
          {
            flag = true;
            linearVelocity.X = 0.0f;
          }
          if (position.Y < min.Y || position.Y > max.Y)
          {
            flag = true;
            linearVelocity.Y = 0.0f;
          }
          if (position.Z < min.Z || position.Z > max.Z)
          {
            flag = true;
            linearVelocity.Z = 0.0f;
          }
          if (flag)
          {
            this.m_checkOutOfWorldCounter = 0;
            this.SetPosition((Vector3D) Vector3.Clamp((Vector3) position, (Vector3) min, (Vector3) max));
            this.Container.Entity.Physics.LinearVelocity = linearVelocity;
          }
          this.m_checkOutOfWorldCounter = 20;
        }
      }

      private class Sandbox_Game_Entities_Character_MyCharacter\u003C\u003EMyCharacterPosition\u003C\u003EActor : IActivator, IActivator<MyCharacter.MyCharacterPosition>
      {
        object IActivator.CreateInstance() => (object) new MyCharacter.MyCharacterPosition();

        MyCharacter.MyCharacterPosition IActivator<MyCharacter.MyCharacterPosition>.CreateInstance() => new MyCharacter.MyCharacterPosition();
      }
    }

    protected sealed class SynchronizeBuildPlanner_Implementation\u003C\u003EVRage_Game_MyObjectBuilder_Character\u003C\u003EBuildPlanItem\u003C\u0023\u003E : ICallSite<MyCharacter, MyObjectBuilder_Character.BuildPlanItem[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in MyObjectBuilder_Character.BuildPlanItem[] buildPlanner,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SynchronizeBuildPlanner_Implementation(buildPlanner);
      }
    }

    protected sealed class EnableIronsightCallback\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_Boolean\u0023System_Boolean\u0023System_Boolean : ICallSite<MyCharacter, bool, bool, bool, bool, bool, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in bool enable,
        in bool newKeyPress,
        in bool changeCamera,
        in bool hideCrosshairWhenAiming,
        in bool forceChangeCamera,
        in DBNull arg6)
      {
        @this.EnableIronsightCallback(enable, newKeyPress, changeCamera, hideCrosshairWhenAiming, forceChangeCamera);
      }
    }

    protected sealed class Jump\u003C\u003EVRageMath_Vector3 : ICallSite<MyCharacter, Vector3, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in Vector3 moveIndicator,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.Jump(moveIndicator);
      }
    }

    protected sealed class UnequipWeapon\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UnequipWeapon();
      }
    }

    protected sealed class EnableLightsCallback\u003C\u003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in bool enable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.EnableLightsCallback(enable);
      }
    }

    protected sealed class EnableBroadcasting\u003C\u003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in bool enable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.EnableBroadcasting(enable);
      }
    }

    protected sealed class EnableBroadcastingCallback\u003C\u003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in bool enable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.EnableBroadcastingCallback(enable);
      }
    }

    protected sealed class OnSuicideRequest\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSuicideRequest();
      }
    }

    protected sealed class RefreshAssetModifiers\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerId,
        in long entityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCharacter.RefreshAssetModifiers(playerId, entityId);
      }
    }

    protected sealed class SendSkinData\u003C\u003ESystem_Int64\u0023System_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, long, byte[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in byte[] checkDataResult,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCharacter.SendSkinData(entityId, checkDataResult);
      }
    }

    protected sealed class ChangeModel_Implementation\u003C\u003ESystem_String\u0023VRageMath_Vector3\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCharacter, string, Vector3, bool, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in string model,
        in Vector3 colorMaskHSV,
        in bool resetToDefault,
        in long caller,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ChangeModel_Implementation(model, colorMaskHSV, resetToDefault, caller);
      }
    }

    protected sealed class UpdateStoredGas_Implementation\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId\u0023System_Single : ICallSite<MyCharacter, SerializableDefinitionId, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in SerializableDefinitionId gasId,
        in float fillLevel,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UpdateStoredGas_Implementation(gasId, fillLevel);
      }
    }

    protected sealed class OnUpdateOxygen\u003C\u003ESystem_Single : ICallSite<MyCharacter, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in float oxygenAmount,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateOxygen(oxygenAmount);
      }
    }

    protected sealed class OnRefillFromBottle\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<MyCharacter, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in SerializableDefinitionId gasId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRefillFromBottle(gasId);
      }
    }

    protected sealed class OnSecondarySoundPlay\u003C\u003EVRage_Audio_MyCueId : ICallSite<MyCharacter, MyCueId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in MyCueId soundId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSecondarySoundPlay(soundId);
      }
    }

    protected sealed class EnablePhysics\u003C\u003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in bool enabled,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.EnablePhysics(enabled);
      }
    }

    protected sealed class OnKillCharacter\u003C\u003EVRage_Game_ModAPI_MyDamageInformation\u0023VRageMath_Vector3 : ICallSite<MyCharacter, MyDamageInformation, Vector3, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in MyDamageInformation damageInfo,
        in Vector3 lastLinearVelocity,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnKillCharacter(damageInfo, lastLinearVelocity);
      }
    }

    protected sealed class SpawnCharacterRelative\u003C\u003ESystem_Int64\u0023VRageMath_Vector3 : ICallSite<MyCharacter, long, Vector3, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in long RelatedEntity,
        in Vector3 DeltaPosition,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SpawnCharacterRelative(RelatedEntity, DeltaPosition);
      }
    }

    protected sealed class ShootBeginCallback\u003C\u003EVRageMath_Vector3\u0023Sandbox_Game_Entities_MyShootActionEnum\u0023System_Boolean : ICallSite<MyCharacter, Vector3, MyShootActionEnum, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in Vector3 direction,
        in MyShootActionEnum action,
        in bool doubleClick,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShootBeginCallback(direction, action, doubleClick);
      }
    }

    protected sealed class ShootEndCallback\u003C\u003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyCharacter, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in MyShootActionEnum action,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShootEndCallback(action);
      }
    }

    protected sealed class GunDoubleClickedCallback\u003C\u003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyCharacter, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in MyShootActionEnum action,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GunDoubleClickedCallback(action);
      }
    }

    protected sealed class ShootDirectionChangeCallback\u003C\u003EVRageMath_Vector3 : ICallSite<MyCharacter, Vector3, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in Vector3 direction,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShootDirectionChangeCallback(direction);
      }
    }

    protected sealed class OnSwitchAmmoMagazineRequest\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchAmmoMagazineRequest();
      }
    }

    protected sealed class OnSwitchAmmoMagazineSuccess\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchAmmoMagazineSuccess();
      }
    }

    protected sealed class SwitchToWeaponMessage\u003C\u003ESystem_Nullable`1\u003CVRage_ObjectBuilders_SerializableDefinitionId\u003E\u0023System_Nullable`1\u003CSystem_UInt32\u003E : ICallSite<MyCharacter, SerializableDefinitionId?, uint?, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in SerializableDefinitionId? weapon,
        in uint? inventoryItemId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SwitchToWeaponMessage(weapon, inventoryItemId);
      }
    }

    protected sealed class OnSwitchToWeaponSuccess\u003C\u003ESystem_Nullable`1\u003CVRage_ObjectBuilders_SerializableDefinitionId\u003E\u0023System_Nullable`1\u003CSystem_UInt32\u003E\u0023System_Int64 : ICallSite<MyCharacter, SerializableDefinitionId?, uint?, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in SerializableDefinitionId? weapon,
        in uint? inventoryItemId,
        in long weaponEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchToWeaponSuccess(weapon, inventoryItemId, weaponEntityId);
      }
    }

    protected sealed class OnAnimationCommand\u003C\u003ESandbox_Game_Entities_MyAnimationCommand : ICallSite<MyCharacter, MyAnimationCommand, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in MyAnimationCommand command,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAnimationCommand(command);
      }
    }

    protected sealed class OnAnimationEvent\u003C\u003ESystem_String\u0023System_String\u003C\u0023\u003E : ICallSite<MyCharacter, string, string[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in string eventName,
        in string[] layers,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAnimationEvent(eventName, layers);
      }
    }

    protected sealed class OnRagdollTransformsUpdate\u003C\u003ESystem_Int32\u0023VRageMath_Vector3\u003C\u0023\u003E\u0023VRageMath_Quaternion\u003C\u0023\u003E\u0023VRageMath_Quaternion\u0023VRageMath_Vector3 : ICallSite<MyCharacter, int, Vector3[], Quaternion[], Quaternion, Vector3, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in int transformsCount,
        in Vector3[] transformsPositions,
        in Quaternion[] transformsOrientations,
        in Quaternion worldOrientation,
        in Vector3 worldPosition,
        in DBNull arg6)
      {
        @this.OnRagdollTransformsUpdate(transformsCount, transformsPositions, transformsOrientations, worldOrientation, worldPosition);
      }
    }

    protected sealed class OnSwitchHelmet\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSwitchHelmet();
      }
    }

    protected sealed class SwitchJetpack\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SwitchJetpack();
      }
    }

    protected sealed class GetOnLadder_Request\u003C\u003ESystem_Int64 : ICallSite<MyCharacter, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in long ladderId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetOnLadder_Request(ladderId);
      }
    }

    protected sealed class GetOnLadder_Failed\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetOnLadder_Failed();
      }
    }

    protected sealed class GetOnLadder_Implementation\u003C\u003ESystem_Int64\u0023System_Boolean\u0023System_Nullable`1\u003CVRage_Game_MyObjectBuilder_Character\u003C\u003ELadderInfo\u003E : ICallSite<MyCharacter, long, bool, MyObjectBuilder_Character.LadderInfo?, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in long ladderId,
        in bool resetPosition,
        in MyObjectBuilder_Character.LadderInfo? newLadderInfo,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetOnLadder_Implementation(ladderId, resetPosition, newLadderInfo);
      }
    }

    protected sealed class GetOffLadder_Implementation\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetOffLadder_Implementation();
      }
    }

    protected sealed class ActivateSnap\u003C\u003ESystem_Int64 : ICallSite<MyCharacter, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in long targetId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ActivateSnap(targetId);
      }
    }

    protected sealed class ReloadServer\u003C\u003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReloadServer();
      }
    }

    protected sealed class CreateBurrowingParticleFX_Client\u003C\u003EVRageMath_Vector3D : ICallSite<MyCharacter, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in Vector3D position,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateBurrowingParticleFX_Client(position);
      }
    }

    protected sealed class DeleteBurrowingParticleFX_Client\u003C\u003EVRageMath_Vector3D : ICallSite<MyCharacter, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in Vector3D position,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DeleteBurrowingParticleFX_Client(position);
      }
    }

    protected sealed class TriggerAnimationEvent\u003C\u003ESystem_String : ICallSite<MyCharacter, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCharacter @this,
        in string eventName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.TriggerAnimationEvent(eventName);
      }
    }

    protected class IsReloading\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).IsReloading = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_recoilData\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyRecoilDataCollection, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyRecoilDataCollection, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_recoilData = (VRage.Sync.Sync<MyRecoilDataCollection, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_enableBroadcastingPlayerToggle\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_enableBroadcastingPlayerToggle = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_dynamicRangeDistance\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<(float, Vector3D), SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<(float, Vector3D), SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_dynamicRangeDistance = (VRage.Sync.Sync<(float, Vector3D), SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_bootsState\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyBootsState, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyBootsState, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_bootsState = (VRage.Sync.Sync<MyBootsState, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentAmmoCount\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_currentAmmoCount = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentMagazineAmmoCount\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_currentMagazineAmmoCount = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_aimedGrid\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<long, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<long, SyncDirection.BothWays>(obj1, obj2));
        ((MyCharacter) obj0).m_aimedGrid = (VRage.Sync.Sync<long, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_aimedBlock\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Vector3I, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Vector3I, SyncDirection.BothWays>(obj1, obj2));
        ((MyCharacter) obj0).m_aimedBlock = (VRage.Sync.Sync<Vector3I, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_controlInfo\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyPlayer.PlayerId, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyPlayer.PlayerId, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_controlInfo = (VRage.Sync.Sync<MyPlayer.PlayerId, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_localHeadPosition\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Vector3, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Vector3, SyncDirection.BothWays>(obj1, obj2));
        ((MyCharacter) obj0).m_localHeadPosition = (VRage.Sync.Sync<Vector3, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_animLeaning\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyCharacter) obj0).m_animLeaning = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class EnvironmentOxygenLevelSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).EnvironmentOxygenLevelSync = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class OxygenLevelAtCharacterLocation\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).OxygenLevelAtCharacterLocation = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class OxygenSourceGridEntityId\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<long, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<long, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).OxygenSourceGridEntityId = (VRage.Sync.Sync<long, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isAimAssistSensitivityDecreased\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyCharacter) obj0).m_isAimAssistSensitivityDecreased = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Character_MyCharacter\u003C\u003EActor : IActivator, IActivator<MyCharacter>
    {
      object IActivator.CreateInstance() => (object) new MyCharacter();

      MyCharacter IActivator<MyCharacter>.CreateInstance() => new MyCharacter();
    }
  }
}
