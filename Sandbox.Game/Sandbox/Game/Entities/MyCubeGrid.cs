// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubeGrid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.GameServices;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageMath.PackedVector;
using VRageMath.Spatial;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
  [StaticEventOwner]
  [MyEntityType(typeof (MyObjectBuilder_CubeGrid), true)]
  public class MyCubeGrid : MyEntity, IMyGridConnectivityTest, IMyEventProxy, IMyEventOwner, IMySyncedEntity, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.Ingame.IMyCubeGrid
  {
    private static readonly int BLOCK_LIMIT_FOR_LARGE_DESTRUCTION = 3;
    private static readonly int TRASH_HIGHLIGHT = 300;
    private static MyCubeGrid.MyCubeGridHitInfo m_hitInfoTmp;
    private static HashSet<MyCubeGrid.MyBlockLocation> m_tmpBuildList = new HashSet<MyCubeGrid.MyBlockLocation>();
    private static List<Vector3I> m_tmpPositionListReceive = new List<Vector3I>();
    private static List<Vector3I> m_tmpPositionListSend = new List<Vector3I>();
    private List<Vector3I> m_removeBlockQueue = new List<Vector3I>();
    private List<Vector3I> m_destroyBlockQueue = new List<Vector3I>();
    private List<Vector3I> m_destructionDeformationQueue = new List<Vector3I>();
    private List<MyCubeGrid.BlockPositionId> m_destroyBlockWithIdQueue = new List<MyCubeGrid.BlockPositionId>();
    private List<MyCubeGrid.BlockPositionId> m_removeBlockWithIdQueue = new List<MyCubeGrid.BlockPositionId>();
    [ThreadStatic]
    private static List<byte> m_boneByteList;
    private List<long> m_tmpBlockIdList = new List<long>();
    private HashSet<MyCubeBlock> m_inventoryBlocks = new HashSet<MyCubeBlock>();
    private HashSet<MyCubeBlock> m_unsafeBlocks = new HashSet<MyCubeBlock>();
    private HashSet<MyDecoy> m_decoys;
    private bool m_isRazeBatchDelayed;
    private MyDelayedRazeBatch m_delayedRazeBatch;
    public HashSet<MyCockpit> m_occupiedBlocks = new HashSet<MyCockpit>();
    private Vector3 m_gravity = Vector3.Zero;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_handBrakeSync;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_dampenersEnabled;
    private static List<MyObjectBuilder_CubeGrid> m_recievedGrids = new List<MyObjectBuilder_CubeGrid>();
    public bool IsAccessibleForProgrammableBlock = true;
    private bool m_largeDestroyInProgress;
    private readonly VRage.Sync.Sync<bool, SyncDirection.FromServer> m_markedAsTrash;
    private int m_trashHighlightCounter;
    internal Action<MyInventoryBase> OnAnyBlockInventoryChanged;
    private MyUpdateTiersGridPresence m_gridPresenceTier;
    private MyUpdateTiersPlayerPresence m_playerPresenceTier;
    private float m_totalBoneDisplacement;
    private static float m_precalculatedCornerBonesDisplacementDistance = 0.0f;
    internal MyVoxelSegmentation BonesToSend = new MyVoxelSegmentation();
    private MyVoxelSegmentation m_bonesToSendSecond = new MyVoxelSegmentation();
    private int m_bonesSendCounter;
    private MyDirtyRegion m_dirtyRegion = new MyDirtyRegion();
    private MyDirtyRegion m_dirtyRegionParallel = new MyDirtyRegion();
    private MyCubeSize m_gridSizeEnum;
    private Vector3I m_min = Vector3I.MaxValue;
    private Vector3I m_max = Vector3I.MinValue;
    private readonly ConcurrentDictionary<Vector3I, MyCube> m_cubes = new ConcurrentDictionary<Vector3I, MyCube>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private readonly FastResourceLock m_cubeLock = new FastResourceLock();
    private bool m_canHavePhysics = true;
    private readonly HashSet<MySlimBlock> m_cubeBlocks = new HashSet<MySlimBlock>();
    private MyConcurrentList<MyCubeBlock> m_fatBlocks = new MyConcurrentList<MyCubeBlock>(100);
    private MyLocalityGrouping m_explosions = new MyLocalityGrouping(MyLocalityGrouping.GroupingMode.Overlaps);
    private Dictionary<Vector3, int> m_colorStatistics = new Dictionary<Vector3, int>();
    private int m_PCU;
    private bool m_IsPowered;
    private HashSet<MyCubeBlock> m_processedBlocks = new HashSet<MyCubeBlock>();
    private ConcurrentCachingHashSet<MyCubeBlock> m_blocksForDraw = new ConcurrentCachingHashSet<MyCubeBlock>();
    private List<MyCubeGrid> m_tmpGrids = new List<MyCubeGrid>();
    private MyCubeGrid.MyTestDisconnectsReason m_disconnectsDirty;
    private bool m_blocksForDamageApplicationDirty;
    private bool m_boundsDirty;
    private int m_lastUpdatedDirtyBounds;
    private HashSet<MySlimBlock> m_blocksForDamageApplication = new HashSet<MySlimBlock>();
    private List<MySlimBlock> m_blocksForDamageApplicationCopy = new List<MySlimBlock>();
    private bool m_updatingDirty;
    private int m_resolvingSplits;
    private HashSet<Vector3UByte> m_tmpBuildFailList = new HashSet<Vector3UByte>();
    private List<Vector3UByte> m_tmpBuildOffsets = new List<Vector3UByte>();
    private List<MySlimBlock> m_tmpBuildSuccessBlocks = new List<MySlimBlock>();
    private static List<Vector3I> m_tmpBlockPositions = new List<Vector3I>();
    [ThreadStatic]
    private static List<MySlimBlock> m_tmpBlockListReceive = new List<MySlimBlock>();
    [ThreadStatic]
    private static List<MyCockpit> m_tmpOccupiedCockpitsPerThread;
    [ThreadStatic]
    private static List<MyObjectBuilder_BlockGroup> m_tmpBlockGroupsPerThread;
    public bool HasShipSoundEvents;
    public int NumberOfReactors;
    public readonly VRage.Sync.Sync<float, SyncDirection.FromServer> GridGeneralDamageModifier;
    internal MyGridSkeleton Skeleton;
    public readonly MyCubeGrid.BlockTypeCounter BlockCounter = new MyCubeGrid.BlockTypeCounter();
    public Dictionary<MyObjectBuilderType, int> BlocksCounters = new Dictionary<MyObjectBuilderType, int>();
    private const float m_gizmoMaxDistanceFromCamera = 100f;
    private const float m_gizmoDrawLineScale = 0.002f;
    private bool m_isStatic;
    public Vector3I? XSymmetryPlane;
    public Vector3I? YSymmetryPlane;
    public Vector3I? ZSymmetryPlane;
    public bool XSymmetryOdd;
    public bool YSymmetryOdd;
    public bool ZSymmetryOdd;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isRespawnGrid;
    public int m_playedTime;
    public bool ControlledFromTurret;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_destructibleBlocks;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_immune;
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_editable;
    internal readonly List<MyBlockGroup> BlockGroups = new List<MyBlockGroup>();
    internal MyCubeGridOwnershipManager m_ownershipManager;
    public MyProjectorBase Projector;
    private bool m_isMarkedForEarlyDeactivation;
    public Action<bool> GridPresenceUpdate;
    public bool CreatePhysics;
    private static readonly HashSet<MyResourceSinkComponent> m_tmpSinks = new HashSet<MyResourceSinkComponent>();
    private static List<MyCubeGrid.LocationIdentity> m_tmpLocationsAndIdsSend = new List<MyCubeGrid.LocationIdentity>();
    private static List<Tuple<Vector3I, ushort>> m_tmpLocationsAndIdsReceive = new List<Tuple<Vector3I, ushort>>();
    private bool m_smallToLargeConnectionsInitialized;
    private bool m_enableSmallToLargeConnections = true;
    private MyCubeGrid.MyTestDynamicReason m_testDynamic;
    public MyTerminalBlock MainCockpit;
    public MyTerminalBlock MainRemoteControl;
    private Dictionary<int, MyCubeGridMultiBlockInfo> m_multiBlockInfos;
    private float PREDICTION_SWITCH_TIME = 5f;
    private int PREDICTION_SWITCH_MIN_COUNTER = 30;
    private bool m_inventoryMassDirty;
    private bool m_dirtyRegionScheduled;
    private static List<MyVoxelBase> m_overlappingVoxelsTmp;
    private static HashSet<MyVoxelBase> m_rootVoxelsToCutTmp;
    private static ConcurrentQueue<MyTuple<int, MyVoxelBase, Vector3I, Vector3I>> m_notificationQueue = new ConcurrentQueue<MyTuple<int, MyVoxelBase, Vector3I, Vector3I>>();
    private int m_standAloneBlockCount;
    private List<MyCubeGrid.DeformationPostponedItem> m_deformationPostponed = new List<MyCubeGrid.DeformationPostponedItem>();
    private static MyConcurrentPool<List<MyCubeGrid.DeformationPostponedItem>> m_postponedListsPool = new MyConcurrentPool<List<MyCubeGrid.DeformationPostponedItem>>();
    private Action m_OnUpdateDirtyCompleted;
    private Action m_UpdateDirtyInternal;
    private bool m_bonesSending;
    private WorkData m_workData = new WorkData();
    [ThreadStatic]
    private static HashSet<MyEntity> m_tmpQueryCubeBlocks;
    [ThreadStatic]
    private static HashSet<MySlimBlock> m_tmpQuerySlimBlocks;
    private static readonly Vector3I[] m_tmpBlockSurroundingOffsets = new Vector3I[27]
    {
      new Vector3I(0, 0, 0),
      new Vector3I(1, 0, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, -1),
      new Vector3I(1, 0, 1),
      new Vector3I(-1, 0, 1),
      new Vector3I(1, 0, -1),
      new Vector3I(-1, 0, -1),
      new Vector3I(0, 1, 0),
      new Vector3I(1, 1, 0),
      new Vector3I(-1, 1, 0),
      new Vector3I(0, 1, 1),
      new Vector3I(0, 1, -1),
      new Vector3I(1, 1, 1),
      new Vector3I(-1, 1, 1),
      new Vector3I(1, 1, -1),
      new Vector3I(-1, 1, -1),
      new Vector3I(0, -1, 0),
      new Vector3I(1, -1, 0),
      new Vector3I(-1, -1, 0),
      new Vector3I(0, -1, 1),
      new Vector3I(0, -1, -1),
      new Vector3I(1, -1, 1),
      new Vector3I(-1, -1, 1),
      new Vector3I(1, -1, -1),
      new Vector3I(-1, -1, -1)
    };
    private MyHudNotification m_inertiaDampenersNotification;
    private MyGridClientState m_lastNetState;
    private List<long> m_targetingList = new List<long>();
    private bool m_targetingListIsWhitelist;
    private bool m_usesTargetingList;
    private Action m_convertToShipResult;
    private long m_closestParentId;
    private bool m_isClientPredicted;
    private bool m_forceDisablePrediction;
    private bool m_allowPrediction = true;
    private Action m_pendingGridReleases;
    private Action<MatrixD> m_updateMergingGrids;
    private const double GRID_PLACING_AREA_FIX_VALUE = 0.11;
    private const string EXPORT_DIRECTORY = "ExportedModels";
    private const string SOURCE_DIRECTORY = "SourceModels";
    private static readonly List<MyObjectBuilder_CubeGrid[]> m_prefabs = new List<MyObjectBuilder_CubeGrid[]>();
    [ThreadStatic]
    private static List<MyEntity> m_tmpResultListPerThread;
    private static readonly List<MyVoxelBase> m_tmpVoxelList = new List<MyVoxelBase>();
    private static int materialID = 0;
    private static Vector2 tumbnailMultiplier = new Vector2();
    private static float m_maxDimensionPreviousRow = 0.0f;
    private static Vector3D m_newPositionForPlacedObject = new Vector3D(0.0, 0.0, 0.0);
    private const int m_numRowsForPlacedObjects = 4;
    private static List<MyLineSegmentOverlapResult<MyEntity>> m_lineOverlapList = new List<MyLineSegmentOverlapResult<MyEntity>>();
    [ThreadStatic]
    private static List<HkBodyCollision> m_physicsBoxQueryListPerThread;
    [ThreadStatic]
    private static Dictionary<Vector3I, MySlimBlock> m_tmpCubeSet = new Dictionary<Vector3I, MySlimBlock>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private readonly MyDisconnectHelper m_disconnectHelper = new MyDisconnectHelper();
    private static readonly List<MyCubeGrid.NeighborOffsetIndex> m_neighborOffsetIndices = new List<MyCubeGrid.NeighborOffsetIndex>(26);
    private static readonly List<float> m_neighborDistances = new List<float>(26);
    private static readonly List<Vector3I> m_neighborOffsets = new List<Vector3I>(26);
    [ThreadStatic]
    private static MyRandom m_deformationRng;
    [ThreadStatic]
    private static List<Vector3I> m_cacheRayCastCellsPerThread;
    [ThreadStatic]
    private static Dictionary<Vector3I, ConnectivityResult> m_cacheNeighborBlocksPerThread;
    [ThreadStatic]
    private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsAPerThread;
    [ThreadStatic]
    private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsBPerThread;
    private static readonly MyComponentList m_buildComponents = new MyComponentList();
    [ThreadStatic]
    private static List<MyPhysics.HitInfo> m_tmpHitListPerThread;
    private static readonly HashSet<Vector3UByte> m_tmpAreaMountpointPass = new HashSet<Vector3UByte>();
    private static readonly MyCubeGrid.AreaConnectivityTest m_areaOverlapTest = new MyCubeGrid.AreaConnectivityTest();
    private static readonly HashSet<Tuple<MySlimBlock, ushort?>> m_tmpBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();
    private static readonly List<MySlimBlock> m_tmpSlimBlocks = new List<MySlimBlock>();
    [ThreadStatic]
    private static List<int> m_tmpMultiBlockIndicesPerThread;
    private static readonly System.Type m_gridSystemsType = MyCubeGrid.ChooseGridSystemsType();
    private static readonly List<Tuple<Vector3I, ushort>> m_tmpRazeList = new List<Tuple<Vector3I, ushort>>();
    private static readonly List<Vector3I> m_tmpLocations = new List<Vector3I>();
    [ThreadStatic]
    private static Ref<HkBoxShape> m_lastQueryBoxPerThread;
    [ThreadStatic]
    private static MatrixD m_lastQueryTransform;
    private const double ROTATION_PRECISION = 0.00100000004749745;
    private static readonly List<MyCubeGrid.QueuedUpdateChange> m_pendingAddedUpdates = new List<MyCubeGrid.QueuedUpdateChange>();
    private readonly List<MyCubeGrid.Update>[] m_updateQueues = new List<MyCubeGrid.Update>[4];
    private int m_totalQueuedSynchronousUpdates;
    private int m_totalQueuedParallelUpdates;
    private MyCubeGrid.UpdateQueue m_updateInProgress;
    private bool m_hasDelayedUpdate;
    private static readonly ConcurrentDictionary<MethodInfo, string> m_methodNames = new ConcurrentDictionary<MethodInfo, string>();
    public static int DebugUpdateHistoryDuration = 120;

    public HashSet<MyCubeBlock> Inventories => this.m_inventoryBlocks;

    public HashSetReader<MyCubeBlock> UnsafeBlocks => (HashSetReader<MyCubeBlock>) this.m_unsafeBlocks;

    public HashSetReader<MyDecoy> Decoys => (HashSetReader<MyDecoy>) this.m_decoys;

    public HashSetReader<MyCockpit> OccupiedBlocks => (HashSetReader<MyCockpit>) this.m_occupiedBlocks;

    public event Action<SyncBase> SyncPropertyChanged
    {
      add => this.SyncType.PropertyChanged += value;
      remove => this.SyncType.PropertyChanged -= value;
    }

    public SyncType SyncType { get; set; }

    static MyCubeGrid()
    {
      for (int index = 0; index < 26; ++index)
      {
        MyCubeGrid.m_neighborOffsetIndices.Add((MyCubeGrid.NeighborOffsetIndex) index);
        MyCubeGrid.m_neighborDistances.Add(0.0f);
        MyCubeGrid.m_neighborOffsets.Add(new Vector3I(0, 0, 0));
      }
      MyCubeGrid.m_neighborOffsets[0] = new Vector3I(1, 0, 0);
      MyCubeGrid.m_neighborOffsets[1] = new Vector3I(-1, 0, 0);
      MyCubeGrid.m_neighborOffsets[2] = new Vector3I(0, 1, 0);
      MyCubeGrid.m_neighborOffsets[3] = new Vector3I(0, -1, 0);
      MyCubeGrid.m_neighborOffsets[4] = new Vector3I(0, 0, 1);
      MyCubeGrid.m_neighborOffsets[5] = new Vector3I(0, 0, -1);
      MyCubeGrid.m_neighborOffsets[6] = new Vector3I(1, 1, 0);
      MyCubeGrid.m_neighborOffsets[7] = new Vector3I(1, -1, 0);
      MyCubeGrid.m_neighborOffsets[8] = new Vector3I(-1, 1, 0);
      MyCubeGrid.m_neighborOffsets[9] = new Vector3I(-1, -1, 0);
      MyCubeGrid.m_neighborOffsets[10] = new Vector3I(0, 1, 1);
      MyCubeGrid.m_neighborOffsets[11] = new Vector3I(0, 1, -1);
      MyCubeGrid.m_neighborOffsets[12] = new Vector3I(0, -1, 1);
      MyCubeGrid.m_neighborOffsets[13] = new Vector3I(0, -1, -1);
      MyCubeGrid.m_neighborOffsets[14] = new Vector3I(1, 0, 1);
      MyCubeGrid.m_neighborOffsets[15] = new Vector3I(1, 0, -1);
      MyCubeGrid.m_neighborOffsets[16] = new Vector3I(-1, 0, 1);
      MyCubeGrid.m_neighborOffsets[17] = new Vector3I(-1, 0, -1);
      MyCubeGrid.m_neighborOffsets[18] = new Vector3I(1, 1, 1);
      MyCubeGrid.m_neighborOffsets[19] = new Vector3I(1, 1, -1);
      MyCubeGrid.m_neighborOffsets[20] = new Vector3I(1, -1, 1);
      MyCubeGrid.m_neighborOffsets[21] = new Vector3I(1, -1, -1);
      MyCubeGrid.m_neighborOffsets[22] = new Vector3I(-1, 1, 1);
      MyCubeGrid.m_neighborOffsets[23] = new Vector3I(-1, 1, -1);
      MyCubeGrid.m_neighborOffsets[24] = new Vector3I(-1, -1, 1);
      MyCubeGrid.m_neighborOffsets[25] = new Vector3I(-1, -1, -1);
      MyCubeGrid.GridCounter = 0;
    }

    public bool IsPowered => this.m_IsPowered;

    public bool SwitchPower()
    {
      this.m_IsPowered = !this.m_IsPowered;
      return this.m_IsPowered;
    }

    public int NumberOfGridColors => this.m_colorStatistics.Count;

    public ConcurrentCachingHashSet<Vector3I> DirtyBlocks
    {
      get
      {
        this.m_dirtyRegion.Cubes.ApplyChanges();
        return this.m_dirtyRegion.Cubes;
      }
    }

    public MyCubeGridRenderData RenderData => this.Render.RenderData;

    public ConcurrentCachingHashSet<MyCubeBlock> BlocksForDraw => this.m_blocksForDraw;

    public bool IsSplit { get; set; }

    private static List<MyCockpit> m_tmpOccupiedCockpits => MyUtils.Init<List<MyCockpit>>(ref MyCubeGrid.m_tmpOccupiedCockpitsPerThread);

    private static List<MyObjectBuilder_BlockGroup> m_tmpBlockGroups => MyUtils.Init<List<MyObjectBuilder_BlockGroup>>(ref MyCubeGrid.m_tmpBlockGroupsPerThread);

    public MyCubeGridSystems GridSystems { get; private set; }

    public bool IsStatic
    {
      get => this.m_isStatic;
      private set
      {
        if (this.m_isStatic == value)
          return;
        this.m_isStatic = value;
        this.NotifyIsStaticChanged(this.m_isStatic);
      }
    }

    public bool DampenersEnabled => (bool) this.m_dampenersEnabled;

    public bool MarkedAsTrash => (bool) this.m_markedAsTrash;

    public bool IsUnsupportedStation { get; private set; }

    public float GridSize { get; private set; }

    public float GridScale { get; private set; }

    public float GridSizeHalf { get; private set; }

    public Vector3 GridSizeHalfVector { get; private set; }

    public float GridSizeQuarter { get; private set; }

    public Vector3 GridSizeQuarterVector { get; private set; }

    public float GridSizeR { get; private set; }

    public Vector3I Min => this.m_min;

    public Vector3I Max => this.m_max;

    public bool IsRespawnGrid
    {
      get => (bool) this.m_isRespawnGrid;
      set => this.m_isRespawnGrid.Value = value;
    }

    public bool DestructibleBlocks
    {
      get => (bool) this.m_destructibleBlocks;
      set => this.m_destructibleBlocks.Value = value;
    }

    public bool Immune
    {
      get => (bool) this.m_immune;
      set => this.m_immune.Value = value;
    }

    public bool Editable
    {
      get => (bool) this.m_editable;
      set => this.m_editable.ValidateAndSet(value);
    }

    public bool BlocksDestructionEnabled => MySession.Static.Settings.DestructibleBlocks && (bool) this.m_destructibleBlocks && !(bool) this.m_immune;

    public List<long> SmallOwners
    {
      get
      {
        if (this.m_ownershipManager.NeedRecalculateOwners)
          this.m_ownershipManager.RecalculateOwnersThreadSafe();
        return this.m_ownershipManager.SmallOwners;
      }
    }

    public List<long> BigOwners
    {
      get
      {
        if (this.m_ownershipManager.NeedRecalculateOwners)
          this.m_ownershipManager.RecalculateOwnersThreadSafe();
        List<long> bigOwners = this.m_ownershipManager.BigOwners;
        if (bigOwners.Count == 0 && (MyEntities.IsAsyncUpdateInProgress || Thread.CurrentThread == MySandboxGame.Static.UpdateThread))
        {
          MyCubeGrid parent = MyGridPhysicalHierarchy.Static.GetParent(this);
          if (parent != null)
            bigOwners = parent.BigOwners;
        }
        return bigOwners;
      }
    }

    public MyCubeSize GridSizeEnum
    {
      get => this.m_gridSizeEnum;
      set
      {
        this.m_gridSizeEnum = value;
        this.GridSize = MyDefinitionManager.Static.GetCubeSize(value);
        this.GridSizeHalf = this.GridSize / 2f;
        this.GridSizeHalfVector = new Vector3(this.GridSizeHalf);
        this.GridSizeQuarter = this.GridSize / 4f;
        this.GridSizeQuarterVector = new Vector3(this.GridSizeQuarter);
        this.GridSizeR = 1f / this.GridSize;
      }
    }

    public MyGridPhysics Physics
    {
      get => (MyGridPhysics) base.Physics;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public int ShapeCount => this.Physics == null ? 0 : this.Physics.Shape.ShapeCount;

    public MyEntityThrustComponent EntityThrustComponent => this.Components.Get<MyEntityThrustComponent>();

    public bool IsMarkedForEarlyDeactivation
    {
      get => this.m_isMarkedForEarlyDeactivation;
      set
      {
        if (this.m_isMarkedForEarlyDeactivation == value)
          return;
        this.m_isMarkedForEarlyDeactivation = value;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.CheckEarlyDeactivation), 3);
      }
    }

    public bool IsBlockTrasferInProgress { get; private set; }

    public bool IsGenerated { get; internal set; }

    public MyUpdateTiersGridPresence GridPresenceTier
    {
      get => this.m_gridPresenceTier;
      set
      {
        if (this.m_gridPresenceTier == value)
          return;
        this.m_gridPresenceTier = value;
        this.GridPresenceTierChanged.InvokeIfNotNull<MyCubeGrid>(this);
      }
    }

    public MyUpdateTiersPlayerPresence PlayerPresenceTier
    {
      get => this.m_playerPresenceTier;
      set
      {
        if (this.m_playerPresenceTier == value)
          return;
        this.m_playerPresenceTier = value;
        this.PlayerPresenceTierChanged.InvokeIfNotNull<MyCubeGrid>(this);
      }
    }

    public event Action<MyCubeGrid> GridPresenceTierChanged;

    public event Action<MyCubeGrid> PlayerPresenceTierChanged;

    public event Action<MySlimBlock> OnBlockAdded;

    public event Action<MyCubeBlock> OnFatBlockAdded;

    public event Action<MySlimBlock> OnBlockRemoved;

    public event Action<MyCubeBlock> OnFatBlockRemoved;

    public event Action<MySlimBlock> OnBlockIntegrityChanged;

    public event Action<MySlimBlock> OnBlockClosed;

    public event Action<MyCubeBlock> OnFatBlockClosed;

    public event Action<MyCubeGrid> OnMassPropertiesChanged;

    public static event Action<MyCubeGrid> OnSplitGridCreated;

    internal void NotifyMassPropertiesChanged() => this.OnMassPropertiesChanged.InvokeIfNotNull<MyCubeGrid>(this);

    internal void NotifyBlockAdded(MySlimBlock block)
    {
      this.OnBlockAdded.InvokeIfNotNull<MySlimBlock>(block);
      if (block.FatBlock != null)
        this.OnFatBlockAdded.InvokeIfNotNull<MyCubeBlock>(block.FatBlock);
      this.GridSystems.OnBlockAdded(block);
    }

    internal void NotifyBlockRemoved(MySlimBlock block)
    {
      this.OnBlockRemoved.InvokeIfNotNull<MySlimBlock>(block);
      if (block.FatBlock != null)
        this.OnFatBlockRemoved.InvokeIfNotNull<MyCubeBlock>(block.FatBlock);
      if (!MyEntities.IsClosingAll && MyVisualScriptLogicProvider.BlockDestroyed != null)
        MyVisualScriptLogicProvider.BlockDestroyed(block.FatBlock != null ? block.FatBlock.Name : string.Empty, this.Name, block.BlockDefinition.Id.TypeId.ToString(), block.BlockDefinition.Id.SubtypeName);
      MyCubeGrids.NotifyBlockDestroyed(this, block);
      this.GridSystems.OnBlockRemoved(block);
    }

    internal void NotifyBlockClosed(MySlimBlock block)
    {
      this.OnBlockClosed.InvokeIfNotNull<MySlimBlock>(block);
      if (block.FatBlock == null)
        return;
      this.OnFatBlockClosed.InvokeIfNotNull<MyCubeBlock>(block.FatBlock);
    }

    internal void NotifyBlockIntegrityChanged(MySlimBlock block, bool handWelded)
    {
      this.OnBlockIntegrityChanged.InvokeIfNotNull<MySlimBlock>(block);
      this.GridSystems.OnBlockIntegrityChanged(block);
      if (MyVisualScriptLogicProvider.BlockIntegrityChanged != null)
        MyVisualScriptLogicProvider.BlockIntegrityChanged(block.FatBlock != null ? block.FatBlock.Name : string.Empty, this.Name, block.BlockDefinition.Id.TypeId.ToString(), block.BlockDefinition.Id.SubtypeName);
      if (!block.IsFullIntegrity)
        return;
      MyCubeGrids.NotifyBlockFinished(this, block, handWelded);
    }

    public event Action<MyCubeGrid> OnBlockOwnershipChanged;

    internal void NotifyBlockOwnershipChange()
    {
      if (this.OnBlockOwnershipChanged != null)
        this.OnBlockOwnershipChanged(this);
      this.GridSystems.OnBlockOwnershipChanged(this);
    }

    [Obsolete("Use OnStaticChanged")]
    public event Action<bool> OnIsStaticChanged;

    public event Action<MyCubeGrid, bool> OnStaticChanged;

    internal void NotifyIsStaticChanged(bool newIsStatic)
    {
      if (this.OnIsStaticChanged != null)
        this.OnIsStaticChanged(newIsStatic);
      if (this.OnStaticChanged == null)
        return;
      this.OnStaticChanged(this, newIsStatic);
    }

    public event Action<MyCubeGrid, MyCubeGrid> OnGridSplit;

    public event Action<MyCubeGrid> OnHierarchyUpdated;

    internal event Action<MyGridLogicalGroupData> AddedToLogicalGroup;

    internal event Action RemovedFromLogicalGroup;

    public event Action<int> OnHavokSystemIDChanged;

    public event Action<MyCubeGrid> OnNameChanged;

    public float Mass
    {
      get
      {
        if (this.Physics == null)
          return 0.0f;
        if (Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsStatic || (this.Physics == null || this.Physics.Shape == null))
          return this.Physics.Mass;
        return !this.Physics.Shape.MassProperties.HasValue ? 0.0f : this.Physics.Shape.MassProperties.Value.Mass;
      }
    }

    public static int GridCounter { get; private set; }

    public int BlocksCount => this.m_cubeBlocks.Count;

    public int BlocksPCU
    {
      get => this.m_PCU;
      set => this.m_PCU = value;
    }

    public HashSet<MySlimBlock> CubeBlocks => this.m_cubeBlocks;

    public event Action<MyCubeGrid> OnGridChanged;

    public void RaiseGridChanged() => this.OnGridChanged.InvokeIfNotNull<MyCubeGrid>(this);

    public void OnTerminalOpened() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.OnGridChangedRPC)));

    [Event(null, 740)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void OnGridChangedRPC() => this.RaiseGridChanged();

    internal bool SmallToLargeConnectionsInitialized => this.m_smallToLargeConnectionsInitialized;

    internal bool EnableSmallToLargeConnections => this.m_enableSmallToLargeConnections;

    internal MyCubeGrid.MyTestDynamicReason TestDynamic
    {
      get => this.m_testDynamic;
      set
      {
        if (this.m_testDynamic == value)
          return;
        this.m_testDynamic = value;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.CheckConvertToDynamic), 4);
      }
    }

    internal MyRenderComponentCubeGrid Render
    {
      get => (MyRenderComponentCubeGrid) base.Render;
      set => this.Render = (MyRenderComponentBase) value;
    }

    public long LocalCoordSystem { get; set; }

    public bool HasMainCockpit() => this.MainCockpit != null;

    public bool IsMainCockpit(MyTerminalBlock cockpit) => this.MainCockpit == cockpit;

    public void SetMainCockpit(MyTerminalBlock cockpit) => this.MainCockpit = cockpit;

    public bool HasMainRemoteControl() => this.MainRemoteControl != null;

    public bool IsMainRemoteControl(MyTerminalBlock remoteControl) => this.MainRemoteControl == remoteControl;

    public void SetMainRemoteControl(MyTerminalBlock remoteControl) => this.MainRemoteControl = remoteControl;

    public int GetFatBlockCount<T>() where T : MyCubeBlock
    {
      int num = 0;
      foreach (MyCubeBlock fatBlock in this.GetFatBlocks())
      {
        if (fatBlock is T)
          ++num;
      }
      return num;
    }

    public MyCubeGrid()
      : this(MyCubeSize.Large)
    {
      this.GridScale = 1f;
      this.Render = new MyRenderComponentCubeGrid();
      this.Render.NeedsDraw = true;
      this.IsUnsupportedStation = false;
      this.Hierarchy.QueryAABBImpl = new Action<BoundingBoxD, List<MyEntity>>(this.QueryAABB);
      this.Hierarchy.QuerySphereImpl = new Action<BoundingSphereD, List<MyEntity>>(this.QuerySphere);
      this.Hierarchy.QueryLineImpl = new Action<LineD, List<MyLineSegmentOverlapResult<MyEntity>>>(this.QueryLine);
      this.Components.Add<MyGridTargeting>(new MyGridTargeting());
      this.SyncType = SyncHelpers.Compose((object) this);
      this.m_handBrakeSync.ValueChanged += (Action<SyncBase>) (x => this.HandBrakeChanged());
      this.m_dampenersEnabled.ValueChanged += (Action<SyncBase>) (x => this.DampenersEnabledChanged());
      this.m_contactPoint.ValueChanged += (Action<SyncBase>) (x => this.OnContactPointChanged());
      this.m_markedAsTrash.ValueChanged += (Action<SyncBase>) (x => this.MarkedAsTrashChanged());
      this.m_UpdateDirtyInternal = new Action(this.UpdateDirtyInternal);
      this.m_OnUpdateDirtyCompleted = new Action(this.OnUpdateDirtyCompleted);
    }

    private MyCubeGrid(MyCubeSize gridSize)
    {
      this.GridScale = 1f;
      this.GridSizeEnum = gridSize;
      this.GridSize = MyDefinitionManager.Static.GetCubeSize(gridSize);
      this.GridSizeHalf = this.GridSize / 2f;
      this.GridSizeHalfVector = new Vector3(this.GridSizeHalf);
      this.GridSizeQuarter = this.GridSize / 4f;
      this.GridSizeQuarterVector = new Vector3(this.GridSizeQuarter);
      this.GridSizeR = 1f / this.GridSize;
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.Skeleton = new MyGridSkeleton();
      ++MyCubeGrid.GridCounter;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentCubeGrid(this));
      if (MyPerGameSettings.Destruction)
        this.OnPhysicsChanged += (Action<MyEntity>) (entity => MyPhysics.RemoveDestructions(entity));
      if (!MyFakes.ASSERT_CHANGES_IN_SIMULATION)
        return;
      this.OnPhysicsChanged += (Action<MyEntity>) (e => {});
      this.OnGridSplit += (Action<MyCubeGrid, MyCubeGrid>) ((g1, g2) => {});
    }

    private void CreateSystems() => this.GridSystems = (MyCubeGridSystems) Activator.CreateInstance(MyCubeGrid.m_gridSystemsType, (object) this);

    public override void Init(MyObjectBuilder_EntityBase objectBuilder) => this.InitInternal(objectBuilder, true);

    [Conditional("DEBUG")]
    private void AssertNonPublicBlock(MyObjectBuilder_CubeBlock block)
    {
      if (this.UpgradeCubeBlock(block, out MyCubeBlockDefinition _) is MyObjectBuilder_CompoundCubeBlock compoundCubeBlock)
      {
        foreach (MyObjectBuilder_CubeBlock block1 in compoundCubeBlock.Blocks)
          ;
      }
    }

    [Conditional("DEBUG")]
    private void AssertNonPublicBlocks(MyObjectBuilder_CubeGrid builder)
    {
      foreach (MyObjectBuilder_CubeBlock cubeBlock in builder.CubeBlocks)
        ;
    }

    private bool RemoveNonPublicBlock(MyObjectBuilder_CubeBlock block)
    {
      MyCubeBlockDefinition blockDefinition;
      if (this.UpgradeCubeBlock(block, out blockDefinition) is MyObjectBuilder_CompoundCubeBlock compoundCubeBlock)
      {
        MyCubeBlockDefinition def;
        compoundCubeBlock.Blocks = ((IEnumerable<MyObjectBuilder_CubeBlock>) compoundCubeBlock.Blocks).Where<MyObjectBuilder_CubeBlock>((Func<MyObjectBuilder_CubeBlock, bool>) (s => !MyDefinitionManager.Static.TryGetCubeBlockDefinition(s.GetId(), out def) || def.Public || def.IsGeneratedBlock)).ToArray<MyObjectBuilder_CubeBlock>();
        return compoundCubeBlock.Blocks.Length == 0;
      }
      return blockDefinition != null && !blockDefinition.Public;
    }

    private void RemoveNonPublicBlocks(MyObjectBuilder_CubeGrid builder) => builder.CubeBlocks.RemoveAll((Predicate<MyObjectBuilder_CubeBlock>) (s => this.RemoveNonPublicBlock(s)));

    private void InitInternal(MyObjectBuilder_EntityBase objectBuilder, bool rebuildGrid)
    {
      List<MyDefinitionId> myDefinitionIdList = new List<MyDefinitionId>();
      this.SyncFlag = true;
      MyObjectBuilder_CubeGrid builder = (MyObjectBuilder_CubeGrid) objectBuilder;
      if (builder != null)
        this.GridSizeEnum = builder.GridSizeEnum;
      this.GridScale = MyDefinitionManager.Static.GetCubeSize(this.GridSizeEnum) / MyDefinitionManager.Static.GetCubeSizeOriginal(this.GridSizeEnum);
      base.Init(objectBuilder);
      this.Init((StringBuilder) null, (string) null, (MyEntity) null, new float?());
      this.m_destructibleBlocks.SetLocalValue(builder.DestructibleBlocks);
      this.m_immune.SetLocalValue(builder.Immune);
      int num = MyFakes.ASSERT_NON_PUBLIC_BLOCKS ? 1 : 0;
      if (MyFakes.REMOVE_NON_PUBLIC_BLOCKS)
        this.RemoveNonPublicBlocks(builder);
      this.GridGeneralDamageModifier.SetLocalValue(1f);
      this.CreateSystems();
      if (builder != null)
      {
        this.IsStatic = builder.IsStatic;
        this.IsUnsupportedStation = builder.IsUnsupportedStation;
        this.CreatePhysics = builder.CreatePhysics;
        this.m_enableSmallToLargeConnections = builder.EnableSmallToLargeConnections;
        this.GridSizeEnum = builder.GridSizeEnum;
        this.Editable = builder.Editable;
        this.m_IsPowered = builder.IsPowered;
        this.GridSystems.BeforeBlockDeserialization(builder);
        this.m_cubes.Clear();
        this.m_cubeBlocks.Clear();
        this.m_fatBlocks.Clear();
        this.m_inventoryBlocks.Clear();
        if (builder.DisplayName == null)
          this.DisplayName = this.MakeCustomName();
        else
          this.DisplayName = builder.DisplayName;
        MyCubeGrid.m_tmpOccupiedCockpits.Clear();
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          this.GridPresenceTier = builder.GridPresenceTier;
          this.PlayerPresenceTier = !Sandbox.Game.Multiplayer.Sync.IsDedicated ? MyUpdateTiersPlayerPresence.Normal : builder.PlayerPresenceTier;
        }
        else
        {
          this.GridPresenceTier = MyUpdateTiersGridPresence.Normal;
          this.PlayerPresenceTier = MyUpdateTiersPlayerPresence.Normal;
        }
        for (int index = 0; index < builder.CubeBlocks.Count; ++index)
        {
          MySlimBlock mySlimBlock = this.AddBlock(builder.CubeBlocks[index], false);
          if (mySlimBlock != null)
          {
            if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
            {
              foreach (MySlimBlock block in (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
              {
                if (!myDefinitionIdList.Contains(block.BlockDefinition.Id))
                  myDefinitionIdList.Add(block.BlockDefinition.Id);
              }
            }
            else if (!myDefinitionIdList.Contains(mySlimBlock.BlockDefinition.Id))
              myDefinitionIdList.Add(mySlimBlock.BlockDefinition.Id);
            if (mySlimBlock.FatBlock is MyCockpit)
            {
              MyCockpit fatBlock = mySlimBlock.FatBlock as MyCockpit;
              if (fatBlock.Pilot != null)
                MyCubeGrid.m_tmpOccupiedCockpits.Add(fatBlock);
            }
          }
        }
        this.GridSystems.AfterBlockDeserialization();
        if (builder.Skeleton != null)
          this.Skeleton.Deserialize(builder.Skeleton, this.GridSize, this.GridSize);
        this.Render.RenderData.SetBasePositionHint(this.Min * this.GridSize - this.GridSize);
        if (rebuildGrid)
          this.RebuildGrid();
        foreach (MyObjectBuilder_BlockGroup blockGroup in builder.BlockGroups)
          this.AddGroup(blockGroup);
        if (this.Physics != null)
        {
          Vector3 linearVelocity = (Vector3) builder.LinearVelocity;
          Vector3 angularVelocity = (Vector3) builder.AngularVelocity;
          Vector3.ClampToSphere(ref linearVelocity, this.Physics.GetMaxRelaxedLinearVelocity());
          Vector3.ClampToSphere(ref angularVelocity, this.Physics.GetMaxRelaxedAngularVelocity());
          this.Physics.LinearVelocity = linearVelocity;
          this.Physics.AngularVelocity = angularVelocity;
          if (!this.IsStatic)
            this.Physics.Shape.BlocksConnectedToWorld.Clear();
          this.SetInventoryMassDirty();
        }
        SerializableVector3I? nullable = builder.XMirroxPlane;
        this.XSymmetryPlane = nullable.HasValue ? new Vector3I?((Vector3I) nullable.GetValueOrDefault()) : new Vector3I?();
        nullable = builder.YMirroxPlane;
        this.YSymmetryPlane = nullable.HasValue ? new Vector3I?((Vector3I) nullable.GetValueOrDefault()) : new Vector3I?();
        nullable = builder.ZMirroxPlane;
        this.ZSymmetryPlane = nullable.HasValue ? new Vector3I?((Vector3I) nullable.GetValueOrDefault()) : new Vector3I?();
        this.XSymmetryOdd = builder.XMirroxOdd;
        this.YSymmetryOdd = builder.YMirroxOdd;
        this.ZSymmetryOdd = builder.ZMirroxOdd;
        this.GridSystems.Init(builder);
        if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
        {
          this.m_ownershipManager = new MyCubeGridOwnershipManager();
          this.m_ownershipManager.Init(this);
        }
        if (this.Hierarchy != null)
          this.Hierarchy.OnChildRemoved += new Action<VRage.ModAPI.IMyEntity>(this.Hierarchy_OnChildRemoved);
      }
      this.ScheduleDirtyRegion();
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      foreach (MyCockpit tmpOccupiedCockpit in MyCubeGrid.m_tmpOccupiedCockpits)
        tmpOccupiedCockpit.GiveControlToPilot();
      MyCubeGrid.m_tmpOccupiedCockpits.Clear();
      if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
        this.PrepareMultiBlockInfos();
      this.m_isRespawnGrid.SetLocalValue(builder.IsRespawnGrid);
      this.m_playedTime = builder.playedTime;
      this.GridGeneralDamageModifier.SetLocalValue(builder.GridGeneralDamageModifier);
      this.LocalCoordSystem = builder.LocalCoordSys;
      this.m_dampenersEnabled.SetLocalValue(builder.DampenersEnabled);
      if (builder.TargetingTargets != null)
        this.m_targetingList = builder.TargetingTargets;
      this.m_targetingListIsWhitelist = builder.TargetingWhitelist;
      this.m_usesTargetingList = this.m_targetingList.Count > 0 || this.m_targetingListIsWhitelist;
      if (builder.TargetingTargets != null)
        this.m_targetingList = builder.TargetingTargets;
      this.m_targetingListIsWhitelist = builder.TargetingWhitelist;
      this.m_usesTargetingList = this.m_targetingList.Count > 0 || this.m_targetingListIsWhitelist;
      if (this.BlocksPCU <= 10000)
        return;
      MyLog.Default.WriteLine(string.Format("Initialized large grid {0} {1} PCU", (object) this.DisplayName, (object) this.BlocksPCU));
    }

    private void Hierarchy_OnChildRemoved(VRage.ModAPI.IMyEntity obj) => this.m_fatBlocks.Remove(obj as MyCubeBlock);

    private static MyCubeGrid CreateGridForSplit(
      MyCubeGrid originalGrid,
      long newEntityId)
    {
      if (!(MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) typeof (MyObjectBuilder_CubeGrid)) is MyObjectBuilder_CubeGrid newObject))
      {
        MyLog.Default.WriteLine("CreateForSplit builder shouldn't be null! Original Grid info: " + originalGrid.ToString());
        return (MyCubeGrid) null;
      }
      newObject.EntityId = newEntityId;
      newObject.GridSizeEnum = originalGrid.GridSizeEnum;
      newObject.IsStatic = originalGrid.IsStatic;
      newObject.PersistentFlags = originalGrid.Render.PersistentFlags;
      newObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(originalGrid.WorldMatrix));
      newObject.DampenersEnabled = (bool) originalGrid.m_dampenersEnabled;
      newObject.IsPowered = originalGrid.m_IsPowered;
      newObject.IsUnsupportedStation = originalGrid.IsUnsupportedStation;
      newObject.GridPresenceTier = originalGrid.GridPresenceTier;
      newObject.PlayerPresenceTier = originalGrid.PlayerPresenceTier;
      if (!(MyEntities.CreateFromObjectBuilderNoinit((MyObjectBuilder_EntityBase) newObject) is MyCubeGrid objectBuilderNoinit))
        return (MyCubeGrid) null;
      objectBuilderNoinit.InitInternal((MyObjectBuilder_EntityBase) newObject, false);
      MyCubeGrid.OnSplitGridCreated.InvokeIfNotNull<MyCubeGrid>(objectBuilderNoinit);
      return objectBuilderNoinit;
    }

    public static void RemoveSplit(
      MyCubeGrid originalGrid,
      List<MySlimBlock> blocks,
      int offset,
      int count,
      bool sync = true)
    {
      for (int index = offset; index < offset + count; ++index)
      {
        if (blocks.Count > index)
        {
          MySlimBlock block = blocks[index];
          if (block != null)
          {
            if (block.FatBlock != null)
              originalGrid.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) block.FatBlock);
            originalGrid.RemoveBlockInternal(block, true, false);
            originalGrid.Physics.AddDirtyBlock(block);
          }
        }
      }
      originalGrid.RemoveEmptyBlockGroups();
      if (!sync || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      originalGrid.AnnounceRemoveSplit(blocks);
    }

    public MyCubeGrid SplitByPlane(PlaneD plane)
    {
      MyCubeGrid.m_tmpSlimBlocks.Clear();
      MyCubeGrid myCubeGrid = (MyCubeGrid) null;
      PlaneD plane1 = PlaneD.Transform(plane, this.PositionComp.WorldMatrixNormalizedInv);
      foreach (MySlimBlock block in this.GetBlocks())
      {
        BoundingBoxD boundingBoxD = new BoundingBoxD((Vector3D) (block.Min * this.GridSize), (Vector3D) (block.Max * this.GridSize));
        boundingBoxD.Inflate((double) this.GridSize / 2.0);
        if (boundingBoxD.Intersects(plane1) == PlaneIntersectionType.Back)
          MyCubeGrid.m_tmpSlimBlocks.Add(block);
      }
      if (MyCubeGrid.m_tmpSlimBlocks.Count != 0)
      {
        myCubeGrid = MyCubeGrid.CreateSplit(this, MyCubeGrid.m_tmpSlimBlocks);
        MyCubeGrid.m_tmpSlimBlocks.Clear();
      }
      return myCubeGrid;
    }

    public static MyCubeGrid CreateSplit(
      MyCubeGrid originalGrid,
      List<MySlimBlock> blocks,
      bool sync = true,
      long newEntityId = 0)
    {
      MyCubeGrid gridForSplit = MyCubeGrid.CreateGridForSplit(originalGrid, newEntityId);
      if (gridForSplit == null)
        return (MyCubeGrid) null;
      Vector3 centerOfMassWorld = (Vector3) originalGrid.Physics.CenterOfMassWorld;
      MyEntities.Add((MyEntity) gridForSplit);
      MyCubeGrid.MoveBlocks(originalGrid, gridForSplit, blocks, 0, blocks.Count);
      gridForSplit.RebuildGrid();
      if (!gridForSplit.IsStatic)
        gridForSplit.Physics.UpdateMass();
      if (originalGrid.IsStatic)
      {
        gridForSplit.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
        originalGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
      }
      gridForSplit.Physics.AngularVelocity = originalGrid.Physics.AngularVelocity;
      gridForSplit.Physics.LinearVelocity = originalGrid.Physics.GetVelocityAtPoint(gridForSplit.Physics.CenterOfMassWorld);
      originalGrid.UpdatePhysicsShape();
      if (!originalGrid.IsStatic)
        originalGrid.Physics.UpdateMass();
      Vector3 vector3 = Vector3.Cross(originalGrid.Physics.AngularVelocity, (Vector3) (originalGrid.Physics.CenterOfMassWorld - centerOfMassWorld));
      originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + vector3;
      if (originalGrid.OnGridSplit != null)
        originalGrid.OnGridSplit(originalGrid, gridForSplit);
      if (sync && Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MyCubeGrid.m_tmpBlockPositions.Clear();
        foreach (MySlimBlock block in blocks)
          MyCubeGrid.m_tmpBlockPositions.Add(block.Position);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RemoveForClientIfIncomplete((IMyEventProxy) originalGrid);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<Vector3I>, long>(originalGrid, (Func<MyCubeGrid, Action<List<Vector3I>, long>>) (x => new Action<List<Vector3I>, long>(x.CreateSplit_Implementation)), MyCubeGrid.m_tmpBlockPositions, gridForSplit.EntityId);
      }
      return gridForSplit;
    }

    [Event(null, 1358)]
    [Reliable]
    [Broadcast]
    public void CreateSplit_Implementation(List<Vector3I> blocks, long newEntityId)
    {
      MyCubeGrid.m_tmpBlockListReceive.Clear();
      foreach (Vector3I block in blocks)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(block);
        if (cubeBlock == null)
          MySandboxGame.Log.WriteLine("Block was null when trying to create a grid split. Desync?");
        else
          MyCubeGrid.m_tmpBlockListReceive.Add(cubeBlock);
      }
      MyCubeGrid.CreateSplit(this, MyCubeGrid.m_tmpBlockListReceive, false, newEntityId);
      MyCubeGrid.m_tmpBlockListReceive.Clear();
    }

    public static void CreateSplits(
      MyCubeGrid originalGrid,
      List<MySlimBlock> splitBlocks,
      List<MyDisconnectHelper.Group> groups,
      MyCubeGrid.MyTestDisconnectsReason reason,
      bool sync = true)
    {
      if (originalGrid == null || originalGrid.Physics == null || (groups == null || splitBlocks == null))
        return;
      Vector3D centerOfMassWorld = originalGrid.Physics.CenterOfMassWorld;
      try
      {
        if (MyCubeGridSmallToLargeConnection.Static != null)
          MyCubeGridSmallToLargeConnection.Static.BeforeGridSplit_SmallToLargeGridConnectivity(originalGrid);
        for (int index = 0; index < groups.Count; ++index)
        {
          MyDisconnectHelper.Group group = groups[index];
          MyCubeGrid.CreateSplitForGroup(originalGrid, splitBlocks, ref group);
          groups[index] = group;
        }
        originalGrid.UpdatePhysicsShape();
        foreach (MyCubeGrid tmpGrid in originalGrid.m_tmpGrids)
        {
          tmpGrid.RebuildGrid();
          if (originalGrid.IsStatic && !MySession.Static.Settings.StationVoxelSupport)
          {
            tmpGrid.TestDynamic = reason == MyCubeGrid.MyTestDisconnectsReason.SplitBlock ? MyCubeGrid.MyTestDynamicReason.GridSplitByBlock : MyCubeGrid.MyTestDynamicReason.GridSplit;
            originalGrid.TestDynamic = reason == MyCubeGrid.MyTestDisconnectsReason.SplitBlock ? MyCubeGrid.MyTestDynamicReason.GridSplitByBlock : MyCubeGrid.MyTestDynamicReason.GridSplit;
          }
          tmpGrid.Physics.AngularVelocity = originalGrid.Physics.AngularVelocity;
          tmpGrid.Physics.LinearVelocity = originalGrid.Physics.GetVelocityAtPoint(tmpGrid.Physics.CenterOfMassWorld);
          Interlocked.Increment(ref originalGrid.m_resolvingSplits);
          tmpGrid.UpdateDirty((Action) (() => Interlocked.Decrement(ref originalGrid.m_resolvingSplits)));
          tmpGrid.UpdateGravity();
        }
        Vector3 vector3 = Vector3.Cross(originalGrid.Physics.AngularVelocity, (Vector3) (originalGrid.Physics.CenterOfMassWorld - centerOfMassWorld));
        originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + vector3;
        if (MyCubeGridSmallToLargeConnection.Static != null)
          MyCubeGridSmallToLargeConnection.Static.AfterGridSplit_SmallToLargeGridConnectivity(originalGrid, originalGrid.m_tmpGrids);
        Action<MyCubeGrid, MyCubeGrid> onGridSplit = originalGrid.OnGridSplit;
        if (onGridSplit != null)
        {
          foreach (MyCubeGrid tmpGrid in originalGrid.m_tmpGrids)
            onGridSplit(originalGrid, tmpGrid);
        }
        foreach (MyCubeGrid tmpGrid in originalGrid.m_tmpGrids)
        {
          tmpGrid.GridSystems.UpdatePower();
          if (tmpGrid.GridSystems.ResourceDistributor != null)
            tmpGrid.GridSystems.ResourceDistributor.MarkForUpdate();
        }
        if (!sync || !Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RemoveForClientIfIncomplete((IMyEventProxy) originalGrid);
        MyCubeGrid.m_tmpBlockPositions.Clear();
        foreach (MySlimBlock splitBlock in splitBlocks)
          MyCubeGrid.m_tmpBlockPositions.Add(splitBlock.Position);
        foreach (MyCubeGrid tmpGrid in originalGrid.m_tmpGrids)
        {
          tmpGrid.IsSplit = true;
          Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicateImmediatelly((IMyReplicable) MyExternalReplicable.FindByObject((object) tmpGrid), (IMyReplicable) MyExternalReplicable.FindByObject((object) originalGrid));
          tmpGrid.IsSplit = false;
        }
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<Vector3I>, List<MyDisconnectHelper.Group>>(originalGrid, (Func<MyCubeGrid, Action<List<Vector3I>, List<MyDisconnectHelper.Group>>>) (x => new Action<List<Vector3I>, List<MyDisconnectHelper.Group>>(x.CreateSplits_Implementation)), MyCubeGrid.m_tmpBlockPositions, groups);
      }
      finally
      {
        originalGrid.m_tmpGrids.Clear();
      }
    }

    [Event(null, 1499)]
    [Reliable]
    [Broadcast]
    public void CreateSplits_Implementation(
      List<Vector3I> blocks,
      List<MyDisconnectHelper.Group> groups)
    {
      if (this.MarkedForClose)
        return;
      MyCubeGrid.m_tmpBlockListReceive.Clear();
      for (int index = 0; index < groups.Count; ++index)
      {
        MyDisconnectHelper.Group group = groups[index];
        int blockCount = group.BlockCount;
        for (int firstBlockIndex = group.FirstBlockIndex; firstBlockIndex < group.FirstBlockIndex + group.BlockCount; ++firstBlockIndex)
        {
          MySlimBlock cubeBlock = this.GetCubeBlock(blocks[firstBlockIndex]);
          if (cubeBlock == null)
          {
            MySandboxGame.Log.WriteLine("Block was null when trying to create a grid split. Desync?");
            --blockCount;
            if (blockCount == 0)
              group.IsValid = false;
          }
          MyCubeGrid.m_tmpBlockListReceive.Add(cubeBlock);
        }
        groups[index] = group;
      }
      MyCubeGrid.CreateSplits(this, MyCubeGrid.m_tmpBlockListReceive, groups, MyCubeGrid.MyTestDisconnectsReason.BlockRemoved, false);
      MyCubeGrid.m_tmpBlockListReceive.Clear();
    }

    [Event(null, 1555)]
    [Reliable]
    [Client]
    public static void ShowMessageGridsRemovedWhilePasting() => MyCubeGrid.ShowMessageGridsRemovedWhilePastingInternal();

    public static void ShowMessageGridsRemovedWhilePastingInternal() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MyCommonTexts.GridsRemovedWhilePasting)));

    private static void CreateSplitForGroup(
      MyCubeGrid originalGrid,
      List<MySlimBlock> splitBlocks,
      ref MyDisconnectHelper.Group group)
    {
      bool flag1 = false;
      if (MySession.Static.SimplifiedSimulation && group.BlockCount < 10)
      {
        flag1 = true;
        for (int firstBlockIndex = group.FirstBlockIndex; firstBlockIndex < group.FirstBlockIndex + group.BlockCount; ++firstBlockIndex)
        {
          if (splitBlocks[firstBlockIndex].FatBlock != null)
          {
            flag1 = false;
            break;
          }
        }
      }
      if (flag1)
      {
        group.IsValid = false;
      }
      else
      {
        if (!originalGrid.IsStatic && Sandbox.Game.Multiplayer.Sync.IsServer && group.IsValid)
        {
          int num = 0;
          for (int firstBlockIndex = group.FirstBlockIndex; firstBlockIndex < group.FirstBlockIndex + group.BlockCount; ++firstBlockIndex)
          {
            if (MyDisconnectHelper.IsDestroyedInVoxels(splitBlocks[firstBlockIndex]))
            {
              ++num;
              if ((double) num / (double) group.BlockCount > 0.400000005960464)
              {
                group.IsValid = false;
                break;
              }
            }
          }
        }
        group.IsValid = group.IsValid && MyCubeGrid.CanHavePhysics(splitBlocks, group.FirstBlockIndex, group.BlockCount) && MyCubeGrid.HasStandAloneBlocks(splitBlocks, group.FirstBlockIndex, group.BlockCount);
        if (group.BlockCount == 1 && splitBlocks.Count > group.FirstBlockIndex && splitBlocks[group.FirstBlockIndex] != null)
        {
          MySlimBlock splitBlock = splitBlocks[group.FirstBlockIndex];
          if (splitBlock.FatBlock is MyFracturedBlock)
          {
            group.IsValid = false;
            if (Sandbox.Game.Multiplayer.Sync.IsServer)
              MyDestructionHelper.CreateFracturePiece(splitBlock.FatBlock as MyFracturedBlock, true);
          }
          else if (splitBlock.FatBlock != null && splitBlock.FatBlock.Components.Has<MyFractureComponentBase>())
          {
            group.IsValid = false;
            if (Sandbox.Game.Multiplayer.Sync.IsServer)
            {
              MyFractureComponentCubeBlock fractureComponent = splitBlock.GetFractureComponent();
              if (fractureComponent != null)
                MyDestructionHelper.CreateFracturePiece(fractureComponent, true);
            }
          }
          else if (splitBlock.FatBlock is MyCompoundCubeBlock)
          {
            MyCompoundCubeBlock fatBlock = splitBlock.FatBlock as MyCompoundCubeBlock;
            bool flag2 = true;
            foreach (MySlimBlock block in fatBlock.GetBlocks())
            {
              flag2 &= block.FatBlock.Components.Has<MyFractureComponentBase>();
              if (!flag2)
                break;
            }
            if (flag2)
            {
              group.IsValid = false;
              if (Sandbox.Game.Multiplayer.Sync.IsServer)
              {
                foreach (MySlimBlock block in fatBlock.GetBlocks())
                {
                  MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
                  if (fractureComponent != null)
                    MyDestructionHelper.CreateFracturePiece(fractureComponent, true);
                }
              }
            }
          }
        }
      }
      if (group.IsValid)
      {
        MyCubeGrid gridForSplit = MyCubeGrid.CreateGridForSplit(originalGrid, group.EntityId);
        if (gridForSplit != null)
        {
          originalGrid.m_tmpGrids.Add(gridForSplit);
          MyCubeGrid.MoveBlocks(originalGrid, gridForSplit, splitBlocks, group.FirstBlockIndex, group.BlockCount);
          gridForSplit.SetInventoryMassDirty();
          gridForSplit.Render.FadeIn = false;
          gridForSplit.RebuildGrid();
          MyEntities.Add((MyEntity) gridForSplit);
          group.EntityId = gridForSplit.EntityId;
          if (gridForSplit.IsStatic && Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            MatrixD worldMatrix = gridForSplit.WorldMatrix;
            bool flag2 = MyCoordinateSystem.Static.IsLocalCoordSysExist(ref worldMatrix, (double) gridForSplit.GridSize);
            if (gridForSplit.GridSizeEnum == MyCubeSize.Large)
            {
              if (flag2)
                MyCoordinateSystem.Static.RegisterCubeGrid(gridForSplit);
              else
                MyCoordinateSystem.Static.CreateCoordSys(gridForSplit, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, true);
            }
          }
        }
        else
          group.IsValid = false;
      }
      if (group.IsValid)
        return;
      MyCubeGrid.RemoveSplit(originalGrid, splitBlocks, group.FirstBlockIndex, group.BlockCount, false);
    }

    private void AddGroup(MyObjectBuilder_BlockGroup groupBuilder)
    {
      if (groupBuilder.Blocks.Count == 0)
        return;
      MyBlockGroup myBlockGroup = new MyBlockGroup();
      myBlockGroup.Init(this, groupBuilder);
      this.BlockGroups.Add(myBlockGroup);
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid) base.GetObjectBuilder(copy);
      this.GetObjectBuilderInternal(objectBuilder, copy);
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    private void GetObjectBuilderInternal(MyObjectBuilder_CubeGrid ob, bool copy)
    {
      ob.GridSizeEnum = this.GridSizeEnum;
      if (ob.Skeleton == null)
        ob.Skeleton = new List<VRage.Game.BoneInfo>();
      ob.Skeleton.Clear();
      this.Skeleton.Serialize(ob.Skeleton, this.GridSize, this);
      ob.IsStatic = this.IsStatic;
      ob.IsUnsupportedStation = this.IsUnsupportedStation;
      ob.Editable = this.Editable;
      ob.IsPowered = this.m_IsPowered;
      ob.CubeBlocks.Clear();
      foreach (MySlimBlock cubeBlock in this.m_cubeBlocks)
      {
        MyObjectBuilder_CubeBlock builderCubeBlock = !copy ? cubeBlock.GetObjectBuilder(false) : cubeBlock.GetCopyObjectBuilder();
        if (builderCubeBlock != null)
          ob.CubeBlocks.Add(builderCubeBlock);
      }
      ob.PersistentFlags = this.Render.PersistentFlags;
      if (this.Physics != null)
      {
        ob.LinearVelocity = (SerializableVector3) this.Physics.LinearVelocity;
        ob.AngularVelocity = (SerializableVector3) this.Physics.AngularVelocity;
      }
      MyObjectBuilder_CubeGrid objectBuilderCubeGrid1 = ob;
      Vector3I? xsymmetryPlane = this.XSymmetryPlane;
      SerializableVector3I? nullable1 = xsymmetryPlane.HasValue ? new SerializableVector3I?((SerializableVector3I) xsymmetryPlane.GetValueOrDefault()) : new SerializableVector3I?();
      objectBuilderCubeGrid1.XMirroxPlane = nullable1;
      MyObjectBuilder_CubeGrid objectBuilderCubeGrid2 = ob;
      Vector3I? ysymmetryPlane = this.YSymmetryPlane;
      SerializableVector3I? nullable2 = ysymmetryPlane.HasValue ? new SerializableVector3I?((SerializableVector3I) ysymmetryPlane.GetValueOrDefault()) : new SerializableVector3I?();
      objectBuilderCubeGrid2.YMirroxPlane = nullable2;
      MyObjectBuilder_CubeGrid objectBuilderCubeGrid3 = ob;
      Vector3I? zsymmetryPlane = this.ZSymmetryPlane;
      SerializableVector3I? nullable3 = zsymmetryPlane.HasValue ? new SerializableVector3I?((SerializableVector3I) zsymmetryPlane.GetValueOrDefault()) : new SerializableVector3I?();
      objectBuilderCubeGrid3.ZMirroxPlane = nullable3;
      ob.XMirroxOdd = this.XSymmetryOdd;
      ob.YMirroxOdd = this.YSymmetryOdd;
      ob.ZMirroxOdd = this.ZSymmetryOdd;
      if (copy)
        ob.Name = (string) null;
      ob.BlockGroups.Clear();
      foreach (MyBlockGroup blockGroup in this.BlockGroups)
        ob.BlockGroups.Add(blockGroup.GetObjectBuilder());
      ob.DisplayName = this.DisplayName;
      ob.DestructibleBlocks = this.DestructibleBlocks;
      ob.Immune = this.Immune;
      ob.IsRespawnGrid = this.IsRespawnGrid;
      ob.playedTime = this.m_playedTime;
      ob.GridGeneralDamageModifier = (float) this.GridGeneralDamageModifier;
      ob.LocalCoordSys = this.LocalCoordSystem;
      ob.TargetingWhitelist = this.m_targetingListIsWhitelist;
      ob.TargetingTargets = this.m_targetingList;
      ob.GridPresenceTier = this.GridPresenceTier;
      ob.PlayerPresenceTier = this.PlayerPresenceTier;
      this.GridSystems.GetObjectBuilder(ob);
    }

    internal void HavokSystemIDChanged(int id) => this.OnHavokSystemIDChanged.InvokeIfNotNull<int>(id);

    private void UpdatePhysicsShape() => this.Physics.UpdateShape();

    public List<HkShape> GetShapesFromPosition(Vector3I pos) => this.Physics.GetShapesFromPosition(pos);

    private void UpdateGravity()
    {
      if (this.IsStatic || this.Physics == null || (!this.Physics.Enabled || this.Physics.IsWelded))
        return;
      if (this.Physics.DisableGravity <= 0)
        this.RecalculateGravity();
      else
        --this.Physics.DisableGravity;
      if (this.Physics.IsWelded || this.Physics.RigidBody.Gravity.Equals(this.m_gravity, 0.01f))
        return;
      this.Physics.Gravity = this.m_gravity;
      this.ActivatePhysics();
    }

    public override void UpdateOnceBeforeFrame()
    {
      this.UpdateGravity();
      base.UpdateOnceBeforeFrame();
      if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE || MyFakes.ENABLE_GRID_SYSTEM_ONCE_BEFORE_FRAME_UPDATE)
        this.GridSystems.UpdateOnceBeforeFrame();
      this.ActivatePhysics();
    }

    public void CheckPredictionFlagScheduling()
    {
      if (!this.IsStatic && !this.ForceDisablePrediction && this.GridSystems?.ControlSystem?.GetShipController()?.TopGrid == this)
      {
        this.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdatePredictionFlag), 2, true);
      }
      else
      {
        this.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdatePredictionFlag));
        this.UpdatePredictionFlag();
      }
    }

    public void UpdatePredictionFlag()
    {
      if (!this.InScene)
        return;
      bool flag = false;
      this.IsClientPredictedCar = false;
      if (!this.IsStatic && !this.ForceDisablePrediction && this.AllowPrediction)
      {
        MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(this);
        if (root == this)
        {
          if (!Sandbox.Game.Multiplayer.Sync.IsServer && MySession.Static.TopMostControlledEntity == this || Sandbox.Game.Multiplayer.Sync.IsServer && Sandbox.Game.Multiplayer.Sync.Players.GetControllingPlayer((MyEntity) this) != null)
          {
            if (!MyGridPhysicalHierarchy.Static.HasChildren(this) && !MyFixedGrids.IsRooted(this))
            {
              flag = true;
              if (this.Physics.PredictedContactsCounter > this.PREDICTION_SWITCH_MIN_COUNTER)
              {
                if (this.Physics.AnyPredictedContactEntities())
                  flag = false;
                else if (this.Physics.PredictedContactLastTime + MyTimeSpan.FromSeconds((double) this.PREDICTION_SWITCH_TIME) < MySandboxGame.Static.SimulationTime)
                  this.Physics.PredictedContactsCounter = 0;
              }
            }
            else if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CAR)
            {
              bool car = true;
              MyGridPhysicalHierarchy.Static.ApplyOnChildren(this, (Action<MyCubeGrid>) (child =>
              {
                if (MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(child) is MyMotorSuspension)
                {
                  child.IsClientPredictedWheel = false;
                  foreach (MyCubeBlock fatBlock in child.GetFatBlocks())
                  {
                    if (fatBlock is MyWheel)
                    {
                      child.IsClientPredictedWheel = true;
                      break;
                    }
                  }
                  if (child.IsClientPredictedWheel)
                    return;
                  car = false;
                }
                else
                  car = false;
              }));
              flag = car;
              this.IsClientPredictedCar = car;
            }
          }
        }
        else if (root != this)
          flag = root.IsClientPredicted;
      }
      int num = this.IsClientPredicted != flag ? 1 : 0;
      this.IsClientPredicted = flag;
      if (num == 0)
        return;
      MyEntities.InvokeLater(new Action(((MyPhysicsBody) this.Physics).UpdateConstraintsForceDisable));
    }

    public void ClientPredictionStaticCheck()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.Physics == null || (this.IsStatic || this.IsClientPredicted != this.Physics.IsStatic))
        return;
      this.Physics.ConvertToDynamic(this.GridSizeEnum == MyCubeSize.Large, this.IsClientPredicted);
      this.UpdateGravity();
    }

    protected static float GetLineWidthForGizmo(IMyGizmoDrawableObject block, BoundingBox box)
    {
      float num1 = 100f;
      foreach (Vector3 corner in box.Corners)
        num1 = (float) Math.Min((double) num1, Math.Abs(MySector.MainCamera.GetDistanceFromPoint(Vector3.Transform(block.GetPositionInGrid() + corner, block.GetWorldMatrix()))));
      Vector3 vector3 = box.Max - box.Min;
      float num2 = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector3.X, vector3.Y), vector3.Z));
      return num1 * (1f / 500f) / num2;
    }

    public bool IsGizmoDrawingEnabled() => MyCubeGrid.ShowSenzorGizmos || MyCubeGrid.ShowGravityGizmos || MyCubeGrid.ShowAntennaGizmos;

    public override void PrepareForDraw()
    {
      base.PrepareForDraw();
      this.GridSystems.PrepareForDraw();
      if (this.IsGizmoDrawingEnabled())
      {
        foreach (MySlimBlock cubeBlock in this.m_cubeBlocks)
        {
          if (cubeBlock.FatBlock is IMyGizmoDrawableObject)
            MyCubeGrid.DrawObjectGizmo(cubeBlock);
        }
      }
      if (this.NeedsPerFrameDraw)
        return;
      this.Render.NeedsDraw = false;
    }

    public void StartReplay() => this.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateReplay), 27);

    public void StopReplay() => this.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateReplay));

    private void UpdateReplay()
    {
      PerFrameData perFrameData;
      if (MySessionComponentReplay.Static.IsEntityBeingReplayed(this.EntityId, out perFrameData))
      {
        if (perFrameData.MovementData.HasValue && !this.IsStatic && this.InScene)
        {
          MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
          if (shipController != null)
          {
            SerializableVector3 moveVector = perFrameData.MovementData.Value.MoveVector;
            Vector2 rotationIndicator = new Vector2(perFrameData.MovementData.Value.RotateVector.X, perFrameData.MovementData.Value.RotateVector.Y);
            float z = perFrameData.MovementData.Value.RotateVector.Z;
            shipController.MoveAndRotate((Vector3) moveVector, rotationIndicator, z);
          }
        }
        if (perFrameData.SwitchWeaponData.HasValue)
        {
          MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
          if (shipController != null && perFrameData.SwitchWeaponData.Value.WeaponDefinition.HasValue && !perFrameData.SwitchWeaponData.Value.WeaponDefinition.Value.TypeId.IsNull)
            shipController.SwitchToWeapon((MyDefinitionId) perFrameData.SwitchWeaponData.Value.WeaponDefinition.Value);
        }
        if (perFrameData.ShootData.HasValue)
        {
          MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
          if (shipController != null)
          {
            if (perFrameData.ShootData.Value.Begin)
              shipController.BeginShoot((MyShootActionEnum) perFrameData.ShootData.Value.ShootAction);
            else
              shipController.EndShoot((MyShootActionEnum) perFrameData.ShootData.Value.ShootAction);
          }
        }
        if (perFrameData.ControlSwitchesData.HasValue)
        {
          MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
          if (shipController != null)
          {
            if (perFrameData.ControlSwitchesData.Value.SwitchDamping)
              shipController.SwitchDamping();
            if (perFrameData.ControlSwitchesData.Value.SwitchLandingGears)
              shipController.SwitchLandingGears();
            if (perFrameData.ControlSwitchesData.Value.SwitchLights)
              shipController.SwitchLights();
            if (perFrameData.ControlSwitchesData.Value.SwitchReactors)
              shipController.SwitchReactors();
            if (perFrameData.ControlSwitchesData.Value.SwitchThrusts)
              shipController.SwitchThrusts();
          }
        }
        if (!perFrameData.UseData.HasValue)
          return;
        MyShipController shipController1 = this.GridSystems.ControlSystem.GetShipController();
        if (shipController1 == null)
          return;
        if (perFrameData.UseData.Value.Use)
          shipController1.Use();
        else if (perFrameData.UseData.Value.UseContinues)
        {
          shipController1.UseContinues();
        }
        else
        {
          if (!perFrameData.UseData.Value.UseFinished)
            return;
          shipController1.UseFinished();
        }
      }
      else
        this.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateReplay));
    }

    private static void DrawObjectGizmo(MySlimBlock block)
    {
      IMyGizmoDrawableObject fatBlock = block.FatBlock as IMyGizmoDrawableObject;
      if (!fatBlock.CanBeDrawn())
        return;
      Color gizmoColor = fatBlock.GetGizmoColor();
      MatrixD worldMatrix = fatBlock.GetWorldMatrix();
      BoundingBox? boundingBox = fatBlock.GetBoundingBox();
      if (boundingBox.HasValue)
      {
        float lineWidthForGizmo = MyCubeGrid.GetLineWidthForGizmo(fatBlock, boundingBox.Value);
        BoundingBoxD localbox = (BoundingBoxD) boundingBox.Value;
        MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref gizmoColor, MySimpleObjectRasterizer.SolidAndWireframe, 1, lineWidthForGizmo);
      }
      else
      {
        float radius = fatBlock.GetRadius();
        MySector.MainCamera.GetDistanceFromPoint(worldMatrix.Translation);
        float lineThickness = 1f / 500f * Math.Min(100f, Math.Abs(radius - (float) MySector.MainCamera.GetDistanceFromPoint(worldMatrix.Translation)));
        int customViewProjectionMatrix = -1;
        MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref gizmoColor, MySimpleObjectRasterizer.SolidAndWireframe, 20, lineThickness: lineThickness, customViewProjectionMatrix: customViewProjectionMatrix);
        if (!fatBlock.EnableLongDrawDistance() || !MyFakes.ENABLE_LONG_DISTANCE_GIZMO_DRAWING)
          return;
        MyBillboardViewProjection billboardViewProjection;
        billboardViewProjection.CameraPosition = MySector.MainCamera.Position;
        billboardViewProjection.ViewAtZero = new Matrix();
        billboardViewProjection.Viewport = MySector.MainCamera.Viewport;
        float aspectRatio = billboardViewProjection.Viewport.Width / billboardViewProjection.Viewport.Height;
        billboardViewProjection.Projection = Matrix.CreatePerspectiveFieldOfView(MySector.MainCamera.FieldOfView, aspectRatio, 1f, 100f);
        billboardViewProjection.Projection.M33 = -1f;
        billboardViewProjection.Projection.M34 = -1f;
        billboardViewProjection.Projection.M43 = 0.0f;
        billboardViewProjection.Projection.M44 = 0.0f;
        int num = 10;
        MyRenderProxy.AddBillboardViewProjection(num, billboardViewProjection);
        MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref gizmoColor, MySimpleObjectRasterizer.SolidAndWireframe, 20, lineThickness: lineThickness, customViewProjectionMatrix: num);
      }
    }

    public override void UpdateBeforeSimulation10()
    {
      MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateBeforeSimulation10));
      base.UpdateBeforeSimulation10();
      if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
        this.GridSystems.UpdateBeforeSimulation10();
      MySimpleProfiler.End(nameof (UpdateBeforeSimulation10));
    }

    public override void UpdateBeforeSimulation100()
    {
      MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateBeforeSimulation100));
      base.UpdateBeforeSimulation100();
      if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
        this.GridSystems.UpdateBeforeSimulation100();
      if (this.Physics != null)
        this.Physics.LowSimulationQuality = !MySession.Static.HighSimulationQuality;
      MySimpleProfiler.End(nameof (UpdateBeforeSimulation100));
    }

    internal void SetInventoryMassDirty()
    {
      if (this.m_inventoryMassDirty)
        return;
      this.m_inventoryMassDirty = true;
      this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.UpdateInventoryMass), 6, true);
    }

    internal void RaiseInventoryChanged(MyInventoryBase inventory, bool processGroup = true)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.OnAnyBlockInventoryChanged.InvokeIfNotNull<MyInventoryBase>(inventory);
      if (!processGroup)
        return;
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this);
      if (group == null)
        return;
      foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
      {
        if (node.NodeData != this && node.NodeData != null)
          node.NodeData.RaiseInventoryChanged(inventory, false);
      }
    }

    private void UpdateInventoryMass()
    {
      if (!this.m_inventoryMassDirty)
        return;
      this.m_inventoryMassDirty = false;
      if (this.Physics == null)
        return;
      this.Physics.Shape.UpdateMassFromInventories(this.m_inventoryBlocks, (MyPhysicsBody) this.Physics);
    }

    public int GetCurrentMass() => (int) this.GetCurrentMass(out float _, out float _);

    public float GetCurrentMass(out float baseMass, out float physicalMass)
    {
      baseMass = 0.0f;
      physicalMass = 0.0f;
      float num1 = 0.0f;
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
      if (group != null)
      {
        float inventorySizeMultiplier = MySession.Static.Settings.BlocksInventorySizeMultiplier;
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
        {
          MyCubeGrid nodeData = node.NodeData;
          if (nodeData != null && nodeData.Physics != null && nodeData.Physics.Shape != null)
          {
            HkMassProperties? massProperties = nodeData.Physics.Shape.MassProperties;
            HkMassProperties? baseMassProperties = nodeData.Physics.Shape.BaseMassProperties;
            if (!this.IsStatic && massProperties.HasValue && baseMassProperties.HasValue)
            {
              float mass1 = massProperties.Value.Mass;
              float mass2 = baseMassProperties.Value.Mass;
              foreach (MyShipController occupiedBlock in nodeData.OccupiedBlocks)
              {
                MyCharacter pilot = occupiedBlock.Pilot;
                if (pilot != null)
                {
                  float baseMass1 = pilot.BaseMass;
                  float num2 = pilot.CurrentMass - baseMass1;
                  mass2 += baseMass1;
                  mass1 += num2 / inventorySizeMultiplier;
                }
              }
              float num3 = (mass1 - mass2) * inventorySizeMultiplier;
              baseMass += mass2;
              num1 += mass2 + num3;
              if (nodeData.Physics.WeldInfo.Parent == null || nodeData.Physics.WeldInfo.Parent == nodeData.Physics)
                physicalMass += nodeData.Physics.Mass;
            }
          }
        }
      }
      return num1;
    }

    public override void UpdateAfterSimulation100()
    {
      MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateAfterSimulation100));
      base.UpdateAfterSimulation100();
      this.UpdateGravity();
      if (MyFakes.ENABLE_BOUNDINGBOX_SHRINKING && this.m_boundsDirty && MySandboxGame.TotalSimulationTimeInMilliseconds - this.m_lastUpdatedDirtyBounds > 30000)
      {
        Vector3I min = this.m_min;
        Vector3I max = this.m_max;
        this.RecalcBounds();
        this.m_boundsDirty = false;
        this.m_lastUpdatedDirtyBounds = MySandboxGame.TotalSimulationTimeInMilliseconds;
        if (this.GridSystems.GasSystem != null && (min != this.m_min || max != this.m_max))
          this.GridSystems.GasSystem.OnCubeGridShrinked();
      }
      if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
        this.GridSystems.UpdateAfterSimulation100();
      MySimpleProfiler.End(nameof (UpdateAfterSimulation100));
    }

    internal bool NeedsPerFrameDraw
    {
      get
      {
        if (!MyFakes.OPTIMIZE_GRID_UPDATES)
          return true;
        int num1 = 0 | (this.IsDirty() ? 1 : 0) | (MyCubeGrid.ShowCenterOfMass || MyCubeGrid.ShowGridPivot || (MyCubeGrid.ShowSenzorGizmos || MyCubeGrid.ShowGravityGizmos) ? 1 : (MyCubeGrid.ShowAntennaGizmos ? 1 : 0)) | (MyFakes.ENABLE_GRID_SYSTEM_UPDATE ? (this.GridSystems.NeedsPerFrameDraw ? 1 : 0) : 0);
        this.BlocksForDraw.ApplyChanges();
        int num2 = this.BlocksForDraw.Count > 0 ? 1 : 0;
        return (num1 | num2 | (this.MarkedAsTrash ? 1 : 0)) != 0;
      }
    }

    internal void MarkForDraw()
    {
      if (this.Closed || !this.NeedsPerFrameDraw || this.Render.NeedsDraw)
        return;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.Closed)
          return;
        this.Render.NeedsDraw = true;
      }), "MarkForDraw()");
    }

    private void CreateFractureBlockComponent(MyFractureComponentBase.Info info)
    {
      if (info.Entity.MarkedForClose)
        return;
      MyFractureComponentCubeBlock componentCubeBlock = new MyFractureComponentCubeBlock();
      info.Entity.Components.Add<MyFractureComponentBase>((MyFractureComponentBase) componentCubeBlock);
      componentCubeBlock.SetShape(info.Shape, info.Compound);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !(info.Entity is MyCubeBlock entity))
        return;
      MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(entity.SlimBlock);
      MySlimBlock cubeBlock = entity.CubeGrid.GetCubeBlock(entity.Position);
      MyCompoundCubeBlock compoundCubeBlock = cubeBlock != null ? cubeBlock.FatBlock as MyCompoundCubeBlock : (MyCompoundCubeBlock) null;
      if (compoundCubeBlock != null)
      {
        ushort? blockId = compoundCubeBlock.GetBlockId(entity.SlimBlock);
        if (blockId.HasValue)
        {
          MyObjectBuilder_FractureComponentBase component = (MyObjectBuilder_FractureComponentBase) componentCubeBlock.Serialize(false);
          MySyncDestructions.CreateFractureComponent(entity.CubeGrid.EntityId, entity.Position, blockId.Value, component);
        }
      }
      else
      {
        MyObjectBuilder_FractureComponentBase component = (MyObjectBuilder_FractureComponentBase) componentCubeBlock.Serialize(false);
        MySyncDestructions.CreateFractureComponent(entity.CubeGrid.EntityId, entity.Position, ushort.MaxValue, component);
      }
      entity.SlimBlock.ApplyDestructionDamage(componentCubeBlock.GetIntegrityRatioFromFracturedPieceCounts());
    }

    internal void RemoveGroup(MyBlockGroup group)
    {
      this.BlockGroups.Remove(group);
      this.GridSystems.RemoveGroup(group);
    }

    internal void RemoveGroupByName(string name)
    {
      MyBlockGroup group = this.BlockGroups.Find((Predicate<MyBlockGroup>) (g => g.Name.CompareTo(name) == 0));
      if (group == null)
        return;
      this.BlockGroups.Remove(group);
      this.GridSystems.RemoveGroup(group);
    }

    internal void AddGroup(MyBlockGroup group)
    {
      foreach (MyBlockGroup blockGroup in this.BlockGroups)
      {
        if (blockGroup.Name.CompareTo(group.Name) == 0)
        {
          this.BlockGroups.Remove(blockGroup);
          group.Blocks.UnionWith((IEnumerable<MyTerminalBlock>) blockGroup.Blocks);
          break;
        }
      }
      this.BlockGroups.Add(group);
      this.GridSystems.AddGroup(group);
    }

    internal void OnAddedToGroup(MyGridLogicalGroupData group)
    {
      this.GridSystems.OnAddedToGroup(group);
      if (this.AddedToLogicalGroup == null)
        return;
      this.AddedToLogicalGroup(group);
    }

    internal void OnRemovedFromGroup(MyGridLogicalGroupData group)
    {
      this.GridSystems.OnRemovedFromGroup(group);
      if (this.RemovedFromLogicalGroup == null)
        return;
      this.RemovedFromLogicalGroup();
    }

    internal void OnAddedToGroup(MyGridPhysicalGroupData groupData) => this.GridSystems.OnAddedToGroup(groupData);

    internal void OnRemovedFromGroup(MyGridPhysicalGroupData group) => this.GridSystems.OnRemovedFromGroup(group);

    private void TryReduceGroupControl()
    {
      MyEntityController entityController = Sandbox.Game.Multiplayer.Sync.Players.GetEntityController((MyEntity) this);
      if (entityController == null || !(entityController.ControlledEntity is MyCockpit))
        return;
      MyCockpit controlledEntity = entityController.ControlledEntity as MyCockpit;
      if (controlledEntity.CubeGrid != this)
        return;
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this);
      if (group == null)
        return;
      foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
      {
        if (node.NodeData != this)
        {
          if (MySession.Static == null)
            MyLog.Default.WriteLine("MySession.Static was null");
          else if (MySession.Static.SyncLayer == null)
            MyLog.Default.WriteLine("MySession.Static.SyncLayer was null");
          else if (Sandbox.Game.Multiplayer.Sync.Clients == null)
            MyLog.Default.WriteLine("Sync.Clients was null");
          Sandbox.Game.Multiplayer.Sync.Players.TryReduceControl((IMyControllableEntity) controlledEntity, (MyEntity) node.NodeData);
        }
      }
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Logical, this);
      MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Physical, this);
      MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Mechanical, this);
      if (!this.IsPreview)
        MyGridPhysicalHierarchy.Static.AddNode(this);
      if (this.IsStatic)
        MyFixedGrids.MarkGridRoot(this);
      this.RecalculateGravity();
      this.UpdateGravity();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (!MyEntities.IsClosingAll)
      {
        MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Physical, this);
        MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Logical, this);
        MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Mechanical, this);
      }
      if (!this.IsPreview)
        MyGridPhysicalHierarchy.Static.RemoveNode(this);
      MyFixedGrids.UnmarkGridRoot(this);
      this.ReleaseMerginGrids();
      if (this.m_unsafeBlocks.Count <= 0)
        return;
      MyUnsafeGridsSessionComponent.UnregisterGrid(this);
    }

    protected override void BeforeDelete()
    {
      this.SendRemovedBlocks();
      this.SendRemovedBlocksWithIds();
      this.RemoveAuthorshipAll();
      this.m_cubes.Clear();
      this.m_targetingList.Clear();
      if (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE)
        MyEntity3DSoundEmitter.UpdateEntityEmitters(true, false, false);
      MyEntities.Remove((MyEntity) this);
      this.UnregisterBlocksBeforeClose();
      base.BeforeDelete();
      --MyCubeGrid.GridCounter;
      this.m_cubeBlocks.Clear();
      this.m_fatBlocks.Clear();
    }

    private void UnregisterBlocks(List<MyCubeBlock> cubeBlocks)
    {
      foreach (MyCubeBlock cubeBlock in cubeBlocks)
        this.GridSystems.UnregisterFromSystems(cubeBlock);
    }

    private void UnregisterBlocksBeforeClose()
    {
      this.GridSystems.BeforeGridClose();
      this.UnregisterBlocks(this.m_fatBlocks.List);
      this.GridSystems.AfterGridClose();
    }

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? tri,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      int num = this.GetIntersectionWithLine(ref line, ref MyCubeGrid.m_hitInfoTmp, flags) ? 1 : 0;
      if (num != 0)
        tri = new MyIntersectionResultLineTriangleEx?(MyCubeGrid.m_hitInfoTmp.Triangle);
      else
        tri = new MyIntersectionResultLineTriangleEx?();
      MyCubeGrid.m_hitInfoTmp = (MyCubeGrid.MyCubeGridHitInfo) null;
      return num != 0;
    }

    public bool GetIntersectionWithLine(
      ref LineD line,
      ref MyCubeGrid.MyCubeGridHitInfo info,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      if (info == null)
        info = new MyCubeGrid.MyCubeGridHitInfo();
      info.Reset();
      if (this.IsPreview || this.Projector != null)
        return false;
      this.RayCastCells(line.From, line.To, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), false, true);
      if (MyCubeGrid.m_cacheRayCastCells.Count == 0)
        return false;
      foreach (Vector3I cacheRayCastCell in MyCubeGrid.m_cacheRayCastCells)
      {
        if (this.m_cubes.ContainsKey(cacheRayCastCell))
        {
          MyCube cube = this.m_cubes[cacheRayCastCell];
          MyIntersectionResultLineTriangleEx? t;
          int cubePartIndex;
          this.GetBlockIntersection(cube, ref line, flags, out t, out cubePartIndex);
          if (t.HasValue)
          {
            info.Position = cube.CubeBlock.Position;
            info.Triangle = t.Value;
            info.CubePartIndex = cubePartIndex;
            info.Triangle.UserObject = (object) cube;
            return true;
          }
        }
      }
      return false;
    }

    internal bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      out MySlimBlock slimBlock,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      t = new MyIntersectionResultLineTriangleEx?();
      slimBlock = (MySlimBlock) null;
      this.RayCastCells(line.From, line.To, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), false, true);
      if (MyCubeGrid.m_cacheRayCastCells.Count == 0)
        return false;
      foreach (Vector3I cacheRayCastCell in MyCubeGrid.m_cacheRayCastCells)
      {
        if (this.m_cubes.ContainsKey(cacheRayCastCell))
        {
          MyCube cube = this.m_cubes[cacheRayCastCell];
          this.GetBlockIntersection(cube, ref line, flags, out t, out int _);
          if (t.HasValue)
          {
            slimBlock = cube.CubeBlock;
            break;
          }
        }
      }
      if (slimBlock != null && slimBlock.FatBlock is MyCompoundCubeBlock)
      {
        ListReader<MySlimBlock> blocks = (slimBlock.FatBlock as MyCompoundCubeBlock).GetBlocks();
        double num1 = double.MaxValue;
        MySlimBlock mySlimBlock1 = (MySlimBlock) null;
        for (int index = 0; index < blocks.Count; ++index)
        {
          MySlimBlock mySlimBlock2 = blocks.ItemAt(index);
          MyIntersectionResultLineTriangleEx? t1;
          if (mySlimBlock2.FatBlock.GetIntersectionWithLine(ref line, out t1, IntersectionFlags.ALL_TRIANGLES) && t1.HasValue)
          {
            double num2 = (t1.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
            if (num2 < num1)
            {
              num1 = num2;
              mySlimBlock1 = mySlimBlock2;
            }
          }
        }
        slimBlock = mySlimBlock1;
      }
      return t.HasValue;
    }

    public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
    {
      try
      {
        BoundingBoxD boundingBoxD = new BoundingBoxD(sphere.Center - new Vector3D(sphere.Radius), sphere.Center + new Vector3D(sphere.Radius));
        MatrixD m = MatrixD.Invert(this.WorldMatrix);
        boundingBoxD = boundingBoxD.TransformFast(ref m);
        Vector3 min = (Vector3) boundingBoxD.Min;
        Vector3 max = (Vector3) boundingBoxD.Max;
        Vector3I vector3I1 = new Vector3I((int) Math.Round((double) min.X * (double) this.GridSizeR), (int) Math.Round((double) min.Y * (double) this.GridSizeR), (int) Math.Round((double) min.Z * (double) this.GridSizeR));
        Vector3I vector3I2 = new Vector3I((int) Math.Round((double) max.X * (double) this.GridSizeR), (int) Math.Round((double) max.Y * (double) this.GridSizeR), (int) Math.Round((double) max.Z * (double) this.GridSizeR));
        Vector3I vector3I3 = Vector3I.Min(vector3I1, vector3I2);
        Vector3I vector3I4 = Vector3I.Max(vector3I1, vector3I2);
        for (int x = vector3I3.X; x <= vector3I4.X; ++x)
        {
          for (int y = vector3I3.Y; y <= vector3I4.Y; ++y)
          {
            for (int z = vector3I3.Z; z <= vector3I4.Z; ++z)
            {
              if (this.m_cubes.ContainsKey(new Vector3I(x, y, z)))
              {
                MyCube cube = this.m_cubes[new Vector3I(x, y, z)];
                if (cube.CubeBlock.FatBlock == null || cube.CubeBlock.FatBlock.Model == null)
                {
                  if (cube.CubeBlock.BlockDefinition.CubeDefinition.CubeTopology == MyCubeTopology.Box)
                    return true;
                  foreach (MyCubePart part in cube.Parts)
                  {
                    MatrixD matrixD = part.InstanceData.LocalMatrix * this.WorldMatrix;
                    Matrix matrix1 = Matrix.Invert((Matrix) ref matrixD);
                    MatrixD matrix2 = (MatrixD) ref matrix1;
                    BoundingSphere localSphere = new BoundingSphere((Vector3) Vector3D.Transform(sphere.Center, matrix2), (float) sphere.Radius);
                    if (part.Model.GetTrianglePruningStructure().GetIntersectionWithSphere(ref localSphere))
                      return true;
                  }
                }
                else
                {
                  MatrixD worldMatrix = cube.CubeBlock.FatBlock.WorldMatrix;
                  Matrix matrix1 = Matrix.Invert((Matrix) ref worldMatrix);
                  MatrixD matrix2 = (MatrixD) ref matrix1;
                  BoundingSphereD boundingSphereD = (BoundingSphereD) new BoundingSphere((Vector3) Vector3D.Transform(sphere.Center, matrix2), (float) sphere.Radius);
                  bool intersectionWithSphere = cube.CubeBlock.FatBlock.Model.GetTrianglePruningStructure().GetIntersectionWithSphere((VRage.ModAPI.IMyEntity) cube.CubeBlock.FatBlock, ref sphere);
                  if (intersectionWithSphere)
                    return intersectionWithSphere;
                }
              }
            }
          }
        }
        return false;
      }
      finally
      {
      }
    }

    public override string ToString() => "Grid_" + (this.IsStatic ? "S" : "D") + "_" + this.GridSizeEnum.ToString() + "_" + (object) this.m_cubeBlocks.Count + " {" + this.EntityId.ToString("X8") + "}";

    public Vector3I WorldToGridInteger(Vector3D coords) => Vector3I.Round(Vector3D.Transform(coords, this.PositionComp.WorldMatrixNormalizedInv) * (double) this.GridSizeR);

    public Vector3D WorldToGridScaledLocal(Vector3D coords) => Vector3D.Transform(coords, this.PositionComp.WorldMatrixNormalizedInv) * (double) this.GridSizeR;

    public static Vector3D GridIntegerToWorld(
      float gridSize,
      Vector3I gridCoords,
      MatrixD worldMatrix)
    {
      return Vector3D.Transform((Vector3D) (Vector3) gridCoords * (double) gridSize, worldMatrix);
    }

    public Vector3D GridIntegerToWorld(Vector3I gridCoords) => MyCubeGrid.GridIntegerToWorld(this.GridSize, gridCoords, this.WorldMatrix);

    public Vector3D GridIntegerToWorld(Vector3D gridCoords) => Vector3D.Transform(gridCoords * (double) this.GridSize, this.WorldMatrix);

    public Vector3I LocalToGridInteger(Vector3 localCoords)
    {
      localCoords *= this.GridSizeR;
      return Vector3I.Round(localCoords);
    }

    public bool CanAddCubes(Vector3I min, Vector3I max)
    {
      Vector3I next = min;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref min, ref max);
      while (vector3IRangeIterator.IsValid())
      {
        if (this.m_cubes.ContainsKey(next))
          return false;
        vector3IRangeIterator.GetNext(out next);
      }
      return true;
    }

    public bool CanAddCubes(
      Vector3I min,
      Vector3I max,
      MyBlockOrientation? orientation,
      MyCubeBlockDefinition definition)
    {
      if (!MyFakes.ENABLE_COMPOUND_BLOCKS || definition == null)
        return this.CanAddCubes(min, max);
      Vector3I next = min;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref min, ref max);
      while (vector3IRangeIterator.IsValid())
      {
        if (!this.CanAddCube(next, orientation, definition))
          return false;
        vector3IRangeIterator.GetNext(out next);
      }
      return true;
    }

    public bool CanAddCube(
      Vector3I pos,
      MyBlockOrientation? orientation,
      MyCubeBlockDefinition definition,
      bool ignoreSame = false)
    {
      if (!MyFakes.ENABLE_COMPOUND_BLOCKS || definition == null)
        return !this.CubeExists(pos);
      if (!this.CubeExists(pos))
        return true;
      MySlimBlock cubeBlock = this.GetCubeBlock(pos);
      return cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock && fatBlock.CanAddBlock(definition, orientation, ignoreSame: ignoreSame);
    }

    public void ClearSymmetries()
    {
      this.XSymmetryPlane = new Vector3I?();
      this.YSymmetryPlane = new Vector3I?();
      this.ZSymmetryPlane = new Vector3I?();
    }

    public bool IsTouchingAnyNeighbor(Vector3I min, Vector3I max)
    {
      Vector3I min1 = min;
      --min1.X;
      Vector3I max1 = max;
      max1.X = min1.X;
      if (!this.CanAddCubes(min1, max1))
        return true;
      Vector3I min2 = min;
      --min2.Y;
      Vector3I max2 = max;
      max2.Y = min2.Y;
      if (!this.CanAddCubes(min2, max2))
        return true;
      Vector3I min3 = min;
      --min3.Z;
      Vector3I max3 = max;
      max3.Z = min3.Z;
      if (!this.CanAddCubes(min3, max3))
        return true;
      Vector3I max4 = max;
      ++max4.X;
      Vector3I min4 = min;
      min4.X = max4.X;
      if (!this.CanAddCubes(min4, max4))
        return true;
      Vector3I max5 = max;
      ++max5.Y;
      Vector3I min5 = min;
      min5.Y = max5.Y;
      if (!this.CanAddCubes(min5, max5))
        return true;
      Vector3I max6 = max;
      ++max6.Z;
      Vector3I min6 = min;
      min6.Z = max6.Z;
      return !this.CanAddCubes(min6, max6);
    }

    public bool CanPlaceBlock(
      Vector3I min,
      Vector3I max,
      MyBlockOrientation orientation,
      MyCubeBlockDefinition definition,
      ulong placingPlayer = 0,
      int? ignoreMultiblockId = null,
      bool ignoreFracturedPieces = false,
      bool isProjection = false)
    {
      MyGridPlacementSettings placementSettings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(this.GridSizeEnum, this.IsStatic);
      return this.CanPlaceBlock(min, max, orientation, definition, ref placementSettings, placingPlayer, ignoreMultiblockId, ignoreFracturedPieces, isProjection);
    }

    public bool CanPlaceBlock(
      Vector3I min,
      Vector3I max,
      MyBlockOrientation orientation,
      MyCubeBlockDefinition definition,
      ref MyGridPlacementSettings gridSettings,
      ulong placingPlayer = 0,
      int? ignoreMultiblockId = null,
      bool ignoreFracturedPieces = false,
      bool isProjection = false)
    {
      return this.CanAddCubes(min, max, new MyBlockOrientation?(orientation), definition) && (!MyFakes.ENABLE_MULTIBLOCKS || !MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION || this.CanAddOtherBlockInMultiBlock(min, max, orientation, definition, ignoreMultiblockId)) && MyCubeGrid.TestPlacementAreaCube(this, ref gridSettings, min, max, orientation, definition, placingPlayer, (MyEntity) this, ignoreFracturedPieces, isProjection);
    }

    private bool IsWithinWorldLimits(long ownerID, int blocksToBuild, int pcu, string name) => MySession.Static.IsWithinWorldLimits(out string _, ownerID, name, pcu, blocksToBuild, this.BlocksCount) == MySession.LimitResult.Passed;

    public void SetCubeDirty(Vector3I pos)
    {
      this.m_dirtyRegion.AddCube(pos);
      MySlimBlock cubeBlock = this.GetCubeBlock(pos);
      if (cubeBlock != null)
        this.Physics.AddDirtyBlock(cubeBlock);
      this.ScheduleDirtyRegion();
    }

    public void SetBlockDirty(MySlimBlock cubeBlock)
    {
      Vector3I next = cubeBlock.Min;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref cubeBlock.Min, ref cubeBlock.Max);
      while (vector3IRangeIterator.IsValid())
      {
        this.m_dirtyRegion.AddCube(next);
        vector3IRangeIterator.GetNext(out next);
      }
      this.ScheduleDirtyRegion();
    }

    public void DebugDrawRange(Vector3I min, Vector3I max)
    {
      Vector3I next = min;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref min, ref max);
      while (vector3IRangeIterator.IsValid())
      {
        Vector3I vector3I = next + 1;
        MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD((Vector3D) (next * this.GridSize), (Vector3D) this.GridSizeHalfVector, Quaternion.Identity);
        obb.Transform(this.WorldMatrix);
        MyRenderProxy.DebugDrawOBB(obb, Color.White, 0.5f, true, false);
        vector3IRangeIterator.GetNext(out next);
      }
    }

    public void DebugDrawPositions(List<Vector3I> positions)
    {
      foreach (Vector3I position in positions)
      {
        Vector3I vector3I = position + 1;
        MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD((Vector3D) (position * this.GridSize), (Vector3D) this.GridSizeHalfVector, Quaternion.Identity);
        obb.Transform(this.WorldMatrix);
        MyRenderProxy.DebugDrawOBB(obb, (Color) Color.White.ToVector3(), 0.5f, true, false);
      }
    }

    private MyObjectBuilder_CubeBlock UpgradeCubeBlock(
      MyObjectBuilder_CubeBlock block,
      out MyCubeBlockDefinition blockDefinition)
    {
      MyDefinitionId id = block.GetId();
      if (MyFakes.ENABLE_COMPOUND_BLOCKS)
      {
        if (block is MyObjectBuilder_CompoundCubeBlock)
        {
          MyObjectBuilder_CompoundCubeBlock compoundCubeBlock = block as MyObjectBuilder_CompoundCubeBlock;
          blockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
          if (blockDefinition == null)
            return (MyObjectBuilder_CubeBlock) null;
          if (compoundCubeBlock.Blocks.Length == 1)
          {
            MyObjectBuilder_CubeBlock block1 = compoundCubeBlock.Blocks[0];
            MyCubeBlockDefinition blockDefinition1;
            if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(block1.GetId(), out blockDefinition1) && !MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition1))
            {
              blockDefinition = blockDefinition1;
              return block1;
            }
          }
          return block;
        }
        if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition) && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
        {
          MyObjectBuilder_CompoundCubeBlock builder = MyCompoundCubeBlock.CreateBuilder(block);
          MyCubeBlockDefinition cubeBlockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
          if (cubeBlockDefinition != null)
          {
            blockDefinition = cubeBlockDefinition;
            return (MyObjectBuilder_CubeBlock) builder;
          }
        }
      }
      if (block is MyObjectBuilder_Ladder)
      {
        MyObjectBuilder_Passage newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Passage>(block.SubtypeName);
        newObject.BlockOrientation = block.BlockOrientation;
        newObject.BuildPercent = block.BuildPercent;
        newObject.EntityId = block.EntityId;
        newObject.IntegrityPercent = block.IntegrityPercent;
        newObject.Min = block.Min;
        blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Passage), block.SubtypeId));
        block = (MyObjectBuilder_CubeBlock) newObject;
        return block;
      }
      MyObjectBuilder_CubeBlock builderCubeBlock1 = block;
      string[] strArray = new string[7]
      {
        "Red",
        "Yellow",
        "Blue",
        "Green",
        "Black",
        "White",
        "Gray"
      };
      Vector3[] vector3Array = new Vector3[7]
      {
        MyRenderComponentBase.OldRedToHSV,
        MyRenderComponentBase.OldYellowToHSV,
        MyRenderComponentBase.OldBlueToHSV,
        MyRenderComponentBase.OldGreenToHSV,
        MyRenderComponentBase.OldBlackToHSV,
        MyRenderComponentBase.OldWhiteToHSV,
        MyRenderComponentBase.OldGrayToHSV
      };
      if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition))
      {
        builderCubeBlock1 = MyCubeGrid.FindDefinitionUpgrade(block, out blockDefinition);
        if (builderCubeBlock1 == null)
        {
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (id.SubtypeName.EndsWith(strArray[index], StringComparison.InvariantCultureIgnoreCase))
            {
              string subtypeName = id.SubtypeName.Substring(0, id.SubtypeName.Length - strArray[index].Length);
              if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(new MyDefinitionId(id.TypeId, subtypeName), out blockDefinition))
              {
                MyObjectBuilder_CubeBlock builderCubeBlock2 = block;
                builderCubeBlock2.ColorMaskHSV = (SerializableVector3) vector3Array[index];
                builderCubeBlock2.SubtypeName = subtypeName;
                return builderCubeBlock2;
              }
            }
          }
        }
        if (builderCubeBlock1 == null)
          return (MyObjectBuilder_CubeBlock) null;
      }
      return builderCubeBlock1;
    }

    private MySlimBlock AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge)
    {
      try
      {
        if (this.Skeleton == null)
          this.Skeleton = new MyGridSkeleton();
        MyCubeBlockDefinition blockDefinition;
        objectBuilder = this.UpgradeCubeBlock(objectBuilder, out blockDefinition);
        if (objectBuilder == null)
          return (MySlimBlock) null;
        try
        {
          return this.AddCubeBlock(objectBuilder, testMerge, blockDefinition);
        }
        catch (DuplicateIdException ex)
        {
          MyLog.Default.WriteLine("ERROR while adding cube " + blockDefinition.DisplayNameText.ToString() + ": " + ex.ToString());
          return (MySlimBlock) null;
        }
      }
      finally
      {
      }
    }

    private MySlimBlock AddCubeBlock(
      MyObjectBuilder_CubeBlock objectBuilder,
      bool testMerge,
      MyCubeBlockDefinition blockDefinition)
    {
      Vector3I min = (Vector3I) objectBuilder.Min;
      Vector3I max;
      MySlimBlock.ComputeMax(blockDefinition, (MyBlockOrientation) objectBuilder.BlockOrientation, ref min, out max);
      if (!this.CanAddCubes(min, max))
        return (MySlimBlock) null;
      object cubeBlock = MyCubeBlockFactory.CreateCubeBlock(objectBuilder);
      if (!(cubeBlock is MySlimBlock mySlimBlock))
        mySlimBlock = new MySlimBlock();
      if (!mySlimBlock.Init(objectBuilder, this, cubeBlock as MyCubeBlock))
        return (MySlimBlock) null;
      if (mySlimBlock.FatBlock is MyCompoundCubeBlock && (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlocksCount() == 0)
        return (MySlimBlock) null;
      if (mySlimBlock.FatBlock != null)
      {
        mySlimBlock.FatBlock.Render.FadeIn = this.Render.FadeIn;
        mySlimBlock.FatBlock.HookMultiplayer();
      }
      mySlimBlock.AddNeighbours();
      this.BoundsInclude(mySlimBlock);
      if (mySlimBlock.FatBlock != null)
      {
        this.Hierarchy.AddChild((VRage.ModAPI.IMyEntity) mySlimBlock.FatBlock);
        this.GridSystems.RegisterInSystems(mySlimBlock.FatBlock);
        if (mySlimBlock.FatBlock.Render.NeedsDrawFromParent)
        {
          this.m_blocksForDraw.Add(mySlimBlock.FatBlock);
          mySlimBlock.FatBlock.Render.SetVisibilityUpdates(true);
        }
        MyObjectBuilderType typeId = mySlimBlock.BlockDefinition.Id.TypeId;
        if (typeId != typeof (MyObjectBuilder_CubeBlock))
        {
          if (!this.BlocksCounters.ContainsKey(typeId))
            this.BlocksCounters.Add(typeId, 0);
          this.BlocksCounters[typeId]++;
        }
      }
      this.m_cubeBlocks.Add(mySlimBlock);
      if (mySlimBlock.FatBlock != null)
        this.m_fatBlocks.Add(mySlimBlock.FatBlock);
      if (!this.m_colorStatistics.ContainsKey(mySlimBlock.ColorMaskHSV))
        this.m_colorStatistics.Add(mySlimBlock.ColorMaskHSV, 0);
      this.m_colorStatistics[mySlimBlock.ColorMaskHSV]++;
      Matrix result;
      ((MyBlockOrientation) objectBuilder.BlockOrientation).GetMatrix(out result);
      MyCubeGridDefinitions.GetRotatedBlockSize(blockDefinition, ref result, out Vector3I _);
      Vector3I center = blockDefinition.Center;
      Vector3I.TransformNormal(ref center, ref result, out Vector3I _);
      bool flag = true;
      Vector3I next = mySlimBlock.Min;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref mySlimBlock.Min, ref mySlimBlock.Max);
      while (vector3IRangeIterator.IsValid())
      {
        flag &= this.AddCube(mySlimBlock, ref next, result, blockDefinition);
        vector3IRangeIterator.GetNext(out next);
      }
      if (mySlimBlock.BlockDefinition.IsStandAlone)
        ++this.m_standAloneBlockCount;
      if (this.Physics != null)
        this.Physics.AddBlock(mySlimBlock);
      this.FixSkeleton(mySlimBlock);
      mySlimBlock.AddAuthorship();
      if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
        this.AddMultiBlockInfo(mySlimBlock);
      if (testMerge)
      {
        MyCubeGrid myCubeGrid = this.DetectMerge(mySlimBlock);
        if (myCubeGrid != null && myCubeGrid != this)
          mySlimBlock = myCubeGrid.GetCubeBlock(mySlimBlock.Position);
        else
          this.NotifyBlockAdded(mySlimBlock);
      }
      else
        this.NotifyBlockAdded(mySlimBlock);
      this.m_PCU += mySlimBlock.ComponentStack.IsFunctional ? mySlimBlock.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      if (mySlimBlock.FatBlock is MyReactor)
        ++this.NumberOfReactors;
      this.MarkForDraw();
      return mySlimBlock;
    }

    public void FixSkeleton(MySlimBlock cubeBlock, bool simplePhysicsUpdateOnly = false)
    {
      float maxBoneError = MyGridSkeleton.GetMaxBoneError(this.GridSize);
      float num = maxBoneError * maxBoneError;
      Vector3I end = (cubeBlock.Min + Vector3I.One) * 2;
      Vector3I next = cubeBlock.Min * 2;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        Vector3 offsetWithNeighbours = this.Skeleton.GetDefinitionOffsetWithNeighbours(cubeBlock.Min, next, this);
        if ((double) offsetWithNeighbours.LengthSquared() < (double) num)
          this.Skeleton.Bones.Remove<Vector3I, Vector3>(next);
        else
          this.Skeleton.Bones[next] = offsetWithNeighbours;
        vector3IRangeIterator.GetNext(out next);
      }
      if (cubeBlock.BlockDefinition.Skeleton == null || cubeBlock.BlockDefinition.Skeleton.Count <= 0 || this.Physics == null)
        return;
      for (int x = -1; x <= 1; ++x)
      {
        for (int y = -1; y <= 1; ++y)
        {
          for (int z = -1; z <= 1; ++z)
          {
            if (simplePhysicsUpdateOnly)
            {
              MySlimBlock cubeBlock1 = this.GetCubeBlock(new Vector3I(x, y, z));
              if (cubeBlock1 != null && cubeBlock1.FatBlock != null)
                cubeBlock1.FatBlock.RaisePhysicsChanged();
            }
            else
              this.SetCubeDirty(new Vector3I(x, y, z) + cubeBlock.Min);
          }
        }
      }
      if (cubeBlock.FatBlock == null)
        return;
      cubeBlock.FatBlock.RaisePhysicsChanged();
    }

    public void EnqueueDestructionDeformationBlock(Vector3I position)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_destructionDeformationQueue.Add(position);
      this.ScheduleSendDirtyBlocks();
    }

    public void EnqueueDestroyedBlock(Vector3I position)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_destroyBlockQueue.Add(position);
      this.ScheduleSendDirtyBlocks();
    }

    public void EnqueueRemovedBlock(Vector3I position)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_removeBlockQueue.Add(position);
      this.ScheduleSendDirtyBlocks();
    }

    private void ScheduleSendDirtyBlocks()
    {
      if (this.m_destroyBlockQueue.Count + this.m_destructionDeformationQueue.Count + this.m_removeBlockQueue.Count != 1)
        return;
      this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.SendRemovedBlocks), 2);
    }

    public void SendRemovedBlocks()
    {
      if (this.m_destroyBlockQueue.Count <= 0 && this.m_destructionDeformationQueue.Count <= 0 && this.m_removeBlockQueue.Count <= 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<Vector3I>, List<Vector3I>, List<Vector3I>>(this, (Func<MyCubeGrid, Action<List<Vector3I>, List<Vector3I>, List<Vector3I>>>) (x => new Action<List<Vector3I>, List<Vector3I>, List<Vector3I>>(x.RemovedBlocks)), this.m_destroyBlockQueue, this.m_destructionDeformationQueue, this.m_removeBlockQueue);
      this.m_removeBlockQueue.Clear();
      this.m_destroyBlockQueue.Clear();
      this.m_destructionDeformationQueue.Clear();
    }

    public bool IsLargeDestroyInProgress => this.m_destroyBlockQueue.Count > MyCubeGrid.BLOCK_LIMIT_FOR_LARGE_DESTRUCTION || this.m_largeDestroyInProgress;

    [Event(null, 3471)]
    [Reliable]
    [Broadcast]
    private void RemovedBlocks(
      List<Vector3I> destroyLocations,
      List<Vector3I> DestructionDeformationLocation,
      List<Vector3I> LocationsWithoutGenerator)
    {
      if (destroyLocations.Count > 0)
        this.BlocksDestroyed(destroyLocations);
      if (LocationsWithoutGenerator.Count > 0)
        this.BlocksRemoved(LocationsWithoutGenerator);
      if (DestructionDeformationLocation.Count <= 0)
        return;
      this.BlocksDeformed(DestructionDeformationLocation);
    }

    public void EnqueueRemovedBlockWithId(Vector3I position, ushort? compoundId)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyCubeGrid.BlockPositionId blockPositionId = new MyCubeGrid.BlockPositionId();
      blockPositionId.Position = position;
      ref MyCubeGrid.BlockPositionId local = ref blockPositionId;
      ushort? nullable = compoundId;
      int num = nullable.HasValue ? (int) nullable.GetValueOrDefault() : -1;
      local.CompoundId = (uint) num;
      this.m_removeBlockWithIdQueue.Add(blockPositionId);
      this.ScheduleSendDirtyBlocksWithIds();
    }

    public void EnqueueDestroyedBlockWithId(Vector3I position, ushort? compoundId)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      List<MyCubeGrid.BlockPositionId> blockWithIdQueue = this.m_destroyBlockWithIdQueue;
      MyCubeGrid.BlockPositionId blockPositionId1 = new MyCubeGrid.BlockPositionId();
      blockPositionId1.Position = position;
      ref MyCubeGrid.BlockPositionId local = ref blockPositionId1;
      ushort? nullable = compoundId;
      int num = nullable.HasValue ? (int) nullable.GetValueOrDefault() : -1;
      local.CompoundId = (uint) num;
      MyCubeGrid.BlockPositionId blockPositionId2 = blockPositionId1;
      blockWithIdQueue.Add(blockPositionId2);
      this.ScheduleSendDirtyBlocksWithIds();
    }

    private void ScheduleSendDirtyBlocksWithIds()
    {
      if (this.m_destroyBlockQueue.Count + this.m_destructionDeformationQueue.Count + this.m_removeBlockQueue.Count != 1)
        return;
      this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.SendRemovedBlocksWithIds), 2);
    }

    public void SendRemovedBlocksWithIds()
    {
      if (this.m_removeBlockWithIdQueue.Count <= 0 && this.m_destroyBlockWithIdQueue.Count <= 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<MyCubeGrid.BlockPositionId>, List<MyCubeGrid.BlockPositionId>>(this, (Func<MyCubeGrid, Action<List<MyCubeGrid.BlockPositionId>, List<MyCubeGrid.BlockPositionId>>>) (x => new Action<List<MyCubeGrid.BlockPositionId>, List<MyCubeGrid.BlockPositionId>>(x.RemovedBlocksWithIds)), this.m_destroyBlockWithIdQueue, this.m_removeBlockWithIdQueue);
      this.m_removeBlockWithIdQueue.Clear();
      this.m_destroyBlockWithIdQueue.Clear();
    }

    [Event(null, 3531)]
    [Reliable]
    [Broadcast]
    private void RemovedBlocksWithIds(
      List<MyCubeGrid.BlockPositionId> destroyBlockWithIdQueueWithoutGenerators,
      List<MyCubeGrid.BlockPositionId> removeBlockWithIdQueueWithoutGenerators)
    {
      if (destroyBlockWithIdQueueWithoutGenerators.Count > 0)
        this.BlocksWithIdRemoved(destroyBlockWithIdQueueWithoutGenerators);
      if (removeBlockWithIdQueueWithoutGenerators.Count <= 0)
        return;
      this.BlocksWithIdRemoved(removeBlockWithIdQueueWithoutGenerators);
    }

    [Event(null, 3547)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    public void RemoveBlocksBuiltByID(long identityID)
    {
      foreach (MySlimBlock block in this.FindBlocksBuiltByID(identityID))
        this.RemoveBlock(block, true);
    }

    [Event(null, 3559)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    public void TransferBlocksBuiltByID(long oldAuthor, long newAuthor)
    {
      foreach (MySlimBlock mySlimBlock in this.FindBlocksBuiltByID(oldAuthor))
        mySlimBlock.TransferAuthorship(newAuthor);
    }

    public void TransferBlocksBuiltByIDClient(long oldAuthor, long newAuthor)
    {
      foreach (MySlimBlock mySlimBlock in this.FindBlocksBuiltByID(oldAuthor))
        mySlimBlock.TransferAuthorshipClient(newAuthor);
    }

    public void TransferBlockLimitsBuiltByID(
      long author,
      MyBlockLimits oldLimits,
      MyBlockLimits newLimits)
    {
      foreach (MySlimBlock mySlimBlock in this.FindBlocksBuiltByID(author))
        mySlimBlock.TransferLimits(oldLimits, newLimits);
    }

    public HashSet<MySlimBlock> FindBlocksBuiltByID(long identityID) => this.FindBlocksBuiltByID(identityID, new HashSet<MySlimBlock>());

    public HashSet<MySlimBlock> FindBlocksBuiltByID(
      long identityID,
      HashSet<MySlimBlock> builtBlocks)
    {
      foreach (MySlimBlock cubeBlock in this.m_cubeBlocks)
      {
        if (cubeBlock.BuiltBy == identityID)
          builtBlocks.Add(cubeBlock);
      }
      return builtBlocks;
    }

    public MySlimBlock BuildGeneratedBlock(
      MyCubeGrid.MyBlockLocation location,
      Vector3 colorMaskHsv,
      MyStringHash skinId)
    {
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) location.BlockDefinition);
      Quaternion result;
      location.Orientation.GetQuaternion(out result);
      return this.BuildBlock(cubeBlockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, (MyEntity) null);
    }

    [Event(null, 3613)]
    [Reliable]
    [Server]
    public void BuildBlockRequest(
      uint colorMaskHsv,
      MyCubeGrid.MyBlockLocation location,
      [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder,
      long builderEntityId,
      bool instantBuild,
      long ownerId)
    {
      this.BuildBlockRequestInternal(new MyCubeGrid.MyBlockVisuals(colorMaskHsv, MyStringHash.NullOrEmpty, applySkin: false), location, blockObjectBuilder, builderEntityId, instantBuild, ownerId, MyEventContext.Current.Sender.Value);
    }

    [Event(null, 3620)]
    [Reliable]
    [Server]
    public void BuildBlockRequest(
      MyCubeGrid.MyBlockVisuals visuals,
      MyCubeGrid.MyBlockLocation location,
      [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder,
      long builderEntityId,
      bool instantBuild,
      long ownerId)
    {
      this.BuildBlockRequestInternal(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId, MyEventContext.Current.Sender.Value);
    }

    public void BuildBlockRequestInternal(
      MyCubeGrid.MyBlockVisuals visuals,
      MyCubeGrid.MyBlockLocation location,
      MyObjectBuilder_CubeBlock blockObjectBuilder,
      long builderEntityId,
      bool instantBuild,
      long ownerId,
      ulong sender,
      bool isProjection = false)
    {
      MyEntity entity = (MyEntity) null;
      MyEntities.TryGetEntityById(builderEntityId, out entity);
      bool flag = (long) sender == (long) Sandbox.Game.Multiplayer.Sync.MyId || MySession.Static.HasPlayerCreativeRights(sender);
      if (entity == null && !flag && !MySession.Static.CreativeMode || !MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, isProjection ? MySafeZoneAction.BuildingProjections : MySafeZoneAction.Building, builderEntityId))
        return;
      if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionId) location.BlockDefinition, sender) || MySession.Static.ResearchEnabled && !flag && !MySessionComponentResearch.Static.CanUse(ownerId, (MyDefinitionId) location.BlockDefinition))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(sender, true, (string) null, true);
      }
      else
      {
        MyCubeGrid.MyBlockLocation? resultBlock = new MyCubeGrid.MyBlockLocation?();
        MyCubeBlockDefinition blockDefinition;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) location.BlockDefinition, out blockDefinition);
        MyBlockOrientation orientation = location.Orientation;
        Quaternion result;
        location.Orientation.GetQuaternion(out result);
        MyCubeBlockDefinition.MountPoint[] modelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
        int? ignoreMultiblockId = blockObjectBuilder == null || blockObjectBuilder.MultiBlockId == 0 ? new int?() : new int?(blockObjectBuilder.MultiBlockId);
        Vector3I centerPos = location.CenterPos;
        ref MyCubeGrid.MyBlockVisuals local = ref visuals;
        MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
        MyStringHash myStringHash = component != null ? component.ValidateArmor(visuals.SkinId, sender) : MyStringHash.NullOrEmpty;
        local.SkinId = myStringHash;
        if (!this.CanPlaceBlock(location.Min, location.Max, orientation, blockDefinition, ignoreMultiblockId: ignoreMultiblockId, isProjection: isProjection) || !MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) this, blockDefinition, modelMountPoints, ref result, ref centerPos))
          return;
        MySlimBlock mySlimBlock = this.BuildBlockSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, location, blockObjectBuilder, ref resultBlock, entity, flag & instantBuild, ownerId);
        if (mySlimBlock == null || !resultBlock.HasValue)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>(mySlimBlock.CubeGrid, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>>) (x => new Action<MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>(x.BuildBlockSucess)), visuals, location, blockObjectBuilder, builderEntityId, flag & instantBuild, ownerId);
        this.AfterBuildBlockSuccess(resultBlock.Value, instantBuild);
      }
    }

    [Event(null, 3676)]
    [Reliable]
    [Broadcast]
    public void BuildBlockSucess(
      MyCubeGrid.MyBlockVisuals visuals,
      MyCubeGrid.MyBlockLocation location,
      [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder,
      long builderEntityId,
      bool instantBuild,
      long ownerId)
    {
      MyEntity entity = (MyEntity) null;
      MyEntities.TryGetEntityById(builderEntityId, out entity);
      MyCubeGrid.MyBlockLocation? resultBlock = new MyCubeGrid.MyBlockLocation?();
      this.BuildBlockSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, location, blockObjectBuilder, ref resultBlock, entity, instantBuild, ownerId);
      if (!resultBlock.HasValue)
        return;
      this.AfterBuildBlockSuccess(resultBlock.Value, instantBuild);
    }

    public void BuildBlocks(
      ref MyCubeGrid.MyBlockBuildArea area,
      long builderEntityId,
      long ownerId)
    {
      int blocksToBuild = (int) area.BuildAreaSize.X * (int) area.BuildAreaSize.Y * (int) area.BuildAreaSize.Z;
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) area.DefinitionId);
      if (!MySession.Static.CheckLimitsAndNotify(ownerId, cubeBlockDefinition.BlockPairName, blocksToBuild * cubeBlockDefinition.PCU, blocksToBuild, this.BlocksCount))
        return;
      ulong steamId = MySession.Static.Players.TryGetSteamId(ownerId);
      if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) cubeBlockDefinition, steamId))
        return;
      bool flag = MySession.Static.CreativeToolsEnabled(Sandbox.Game.Multiplayer.Sync.MyId);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockBuildArea, long, bool, long, ulong>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockBuildArea, long, bool, long, ulong>>) (x => new Action<MyCubeGrid.MyBlockBuildArea, long, bool, long, ulong>(x.BuildBlocksAreaRequest)), area, builderEntityId, flag, ownerId, Sandbox.Game.Multiplayer.Sync.MyId);
    }

    public void BuildBlocks(
      Vector3 colorMaskHsv,
      MyStringHash skinId,
      HashSet<MyCubeGrid.MyBlockLocation> locations,
      long builderEntityId,
      long ownerId)
    {
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) locations.First<MyCubeGrid.MyBlockLocation>().BlockDefinition);
      string blockPairName = cubeBlockDefinition.BlockPairName;
      bool flag1 = MySession.Static.CreativeToolsEnabled(Sandbox.Game.Multiplayer.Sync.MyId);
      bool flag2 = flag1 || MySession.Static.CreativeMode;
      if (!MySession.Static.CheckLimitsAndNotify(ownerId, blockPairName, flag2 ? locations.Count * cubeBlockDefinition.PCU : locations.Count, locations.Count, this.BlocksCount))
        return;
      ulong steamId = MySession.Static.Players.TryGetSteamId(ownerId);
      if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) cubeBlockDefinition, steamId))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long>>) (x => new Action<MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long>(x.BuildBlocksRequest)), new MyCubeGrid.MyBlockVisuals(colorMaskHsv.PackHSVToUint(), skinId), locations, builderEntityId, flag1, ownerId);
    }

    [Event(null, 3728)]
    [Reliable]
    [Server]
    private void BuildBlocksRequest(
      MyCubeGrid.MyBlockVisuals visuals,
      HashSet<MyCubeGrid.MyBlockLocation> locations,
      long builderEntityId,
      bool instantBuild,
      long ownerId)
    {
      MyEventContext current;
      if (!MySession.Static.CreativeMode)
      {
        current = MyEventContext.Current;
        if (!current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
          instantBuild = false;
      }
      MyCubeGrid.m_tmpBuildList.Clear();
      MyEntity entity = (MyEntity) null;
      MyEntities.TryGetEntityById(builderEntityId, out entity);
      MyCubeBuilder.BuildComponent.GetBlocksPlacementMaterials(locations, this);
      bool flag1 = MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode;
      current = MyEventContext.Current;
      bool flag2 = current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
      if (entity == null && !flag2 && !MySession.Static.CreativeMode || (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Building, builderEntityId, MyEventContext.Current.Sender.Value) || !MyCubeBuilder.BuildComponent.HasBuildingMaterials(entity) && !flag2))
        return;
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) locations.First<MyCubeGrid.MyBlockLocation>().BlockDefinition);
      string blockPairName = cubeBlockDefinition.BlockPairName;
      if (!this.IsWithinWorldLimits(ownerId, locations.Count, flag1 ? locations.Count * cubeBlockDefinition.PCU : locations.Count, blockPairName))
        return;
      Vector3 colorMaskHsv = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
      ulong steamId = MyEventContext.Current.Sender.Value;
      ref MyCubeGrid.MyBlockVisuals local = ref visuals;
      MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
      MyStringHash myStringHash = component != null ? component.ValidateArmor(visuals.SkinId, steamId) : MyStringHash.NullOrEmpty;
      local.SkinId = myStringHash;
      this.BuildBlocksSuccess(colorMaskHsv, visuals.SkinId, locations, MyCubeGrid.m_tmpBuildList, entity, flag2 & instantBuild, ownerId, MyEventContext.Current.Sender.Value);
      if (MyCubeGrid.m_tmpBuildList.Count > 0)
      {
        MySession.Static.TotalBlocksCreated += (uint) MyCubeGrid.m_tmpBuildList.Count;
        if (MySession.Static.ControlledEntity is MyCockpit)
          MySession.Static.TotalBlocksCreatedFromShips += (uint) MyCubeGrid.m_tmpBuildList.Count;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long>>) (x => new Action<MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long>(x.BuildBlocksClient)), visuals, MyCubeGrid.m_tmpBuildList, builderEntityId, flag2 & instantBuild, ownerId);
        if (Sandbox.Game.Multiplayer.Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId == ownerId)
          MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.BuildBlocksFailedNotify)), new EndpointId(MyEventContext.Current.Sender.Value));
      this.AfterBuildBlocksSuccess(MyCubeGrid.m_tmpBuildList, instantBuild);
    }

    [Event(null, 3792)]
    [Reliable]
    [Client]
    public void BuildBlocksFailedNotify()
    {
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.NotifyPlacementUnable();
    }

    [Event(null, 3799)]
    [Reliable]
    [Broadcast]
    public void BuildBlocksClient(
      MyCubeGrid.MyBlockVisuals visuals,
      HashSet<MyCubeGrid.MyBlockLocation> locations,
      long builderEntityId,
      bool instantBuild,
      long ownerId)
    {
      MyCubeGrid.m_tmpBuildList.Clear();
      MyEntity entity = (MyEntity) null;
      MyEntities.TryGetEntityById(builderEntityId, out entity);
      this.BuildBlocksSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, locations, MyCubeGrid.m_tmpBuildList, entity, instantBuild, ownerId);
      if (ownerId == MySession.Static.LocalPlayerId)
        MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
      this.AfterBuildBlocksSuccess(MyCubeGrid.m_tmpBuildList, instantBuild);
    }

    [Event(null, 3813)]
    [Reliable]
    [Server]
    private void BuildBlocksAreaRequest(
      MyCubeGrid.MyBlockBuildArea area,
      long builderEntityId,
      bool instantBuild,
      long ownerId,
      ulong placingPlayer)
    {
      MyEventContext current;
      if (!MySession.Static.CreativeMode)
      {
        current = MyEventContext.Current;
        if (!current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
          instantBuild = false;
      }
      try
      {
        bool flag = MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode;
        current = MyEventContext.Current;
        bool isAdmin = current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
        if (ownerId == 0L && !isAdmin && !MySession.Static.CreativeMode || !MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Building, builderEntityId, placingPlayer))
          return;
        MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) area.DefinitionId);
        int blocksToBuild = (int) area.BuildAreaSize.X * (int) area.BuildAreaSize.Y * (int) area.BuildAreaSize.Z;
        if (!this.IsWithinWorldLimits(ownerId, blocksToBuild, flag ? blocksToBuild * cubeBlockDefinition.PCU : blocksToBuild, cubeBlockDefinition.BlockPairName))
          return;
        int amount = (int) area.BuildAreaSize.X * (int) area.BuildAreaSize.Y * (int) area.BuildAreaSize.Z;
        MyCubeBuilder.BuildComponent.GetBlockAmountPlacementMaterials(cubeBlockDefinition, amount);
        MyEntity entity = (MyEntity) null;
        MyEntities.TryGetEntityById(builderEntityId, out entity);
        if (!MyCubeBuilder.BuildComponent.HasBuildingMaterials(entity, true) && !isAdmin)
          return;
        this.GetValidBuildOffsets(ref area, this.m_tmpBuildOffsets, this.m_tmpBuildFailList, placingPlayer);
        MyCubeGrid.CheckAreaConnectivity(this, ref area, this.m_tmpBuildOffsets, this.m_tmpBuildFailList);
        int randomSeed = MyRandom.Instance.CreateRandomSeed();
        MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
        area.SkinId = component != null ? component.ValidateArmor(area.SkinId, MyEventContext.Current.Sender.Value) : MyStringHash.NullOrEmpty;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>>) (x => new Action<MyCubeGrid.MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>(x.BuildBlocksAreaClient)), area, randomSeed, this.m_tmpBuildFailList, builderEntityId, isAdmin, ownerId);
        this.BuildBlocksArea(ref area, this.m_tmpBuildOffsets, builderEntityId, isAdmin, ownerId, randomSeed);
      }
      finally
      {
        this.m_tmpBuildOffsets.Clear();
        this.m_tmpBuildFailList.Clear();
      }
    }

    [Event(null, 3869)]
    [Reliable]
    [Broadcast]
    private void BuildBlocksAreaClient(
      MyCubeGrid.MyBlockBuildArea area,
      int entityIdSeed,
      HashSet<Vector3UByte> failList,
      long builderEntityId,
      bool isAdmin,
      long ownerId)
    {
      try
      {
        this.GetAllBuildOffsetsExcept(ref area, failList, this.m_tmpBuildOffsets);
        this.BuildBlocksArea(ref area, this.m_tmpBuildOffsets, builderEntityId, isAdmin, ownerId, entityIdSeed);
      }
      finally
      {
        this.m_tmpBuildOffsets.Clear();
      }
    }

    private void BuildBlocksArea(
      ref MyCubeGrid.MyBlockBuildArea area,
      List<Vector3UByte> validOffsets,
      long builderEntityId,
      bool isAdmin,
      long ownerId,
      int entityIdSeed)
    {
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) area.DefinitionId);
      if (cubeBlockDefinition == null)
        return;
      Quaternion orientation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
      Vector3I stepDelta = (Vector3I) area.StepDelta;
      MyEntity entity = (MyEntity) null;
      MyEntities.TryGetEntityById(builderEntityId, out entity);
      try
      {
        bool flag = false;
        validOffsets.Sort((IComparer<Vector3UByte>) Vector3UByte.Comparer);
        using (MyRandom.Instance.PushSeed(entityIdSeed))
        {
          foreach (Vector3UByte validOffset in validOffsets)
          {
            Vector3I vector3I = area.PosInGrid + (Vector3I) validOffset * stepDelta;
            MySlimBlock block = this.BuildBlock(cubeBlockDefinition, ColorExtensions.UnpackHSVFromUint(area.ColorMaskHSV), area.SkinId, vector3I + (Vector3I) area.BlockMin, orientation, ownerId, MyEntityIdentifier.AllocateId(), entity, updateVolume: false, testMerge: false, buildAsAdmin: isAdmin);
            if (block != null)
            {
              MyCubeGrid.ChangeBlockOwner(block, ownerId);
              flag = true;
              this.m_tmpBuildSuccessBlocks.Add(block);
              if (ownerId == MySession.Static.LocalPlayerId)
              {
                ++MySession.Static.TotalBlocksCreated;
                if (MySession.Static.ControlledEntity is MyCockpit)
                  ++MySession.Static.TotalBlocksCreatedFromShips;
              }
            }
          }
        }
        BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
        foreach (MySlimBlock buildSuccessBlock in this.m_tmpBuildSuccessBlocks)
        {
          BoundingBoxD aabb;
          buildSuccessBlock.GetWorldBoundingBox(out aabb, false);
          invalid.Include(aabb);
          if (buildSuccessBlock.FatBlock != null)
            buildSuccessBlock.FatBlock.OnBuildSuccess(ownerId, isAdmin);
        }
        if (this.m_tmpBuildSuccessBlocks.Count > 0)
        {
          if (this.IsStatic && Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            List<MyEntity> entitiesInAabb = MyEntities.GetEntitiesInAABB(ref invalid);
            foreach (MySlimBlock buildSuccessBlock in this.m_tmpBuildSuccessBlocks)
              this.DetectMerge(buildSuccessBlock, nearEntities: entitiesInAabb);
            entitiesInAabb.Clear();
          }
          this.m_tmpBuildSuccessBlocks[0].PlayConstructionSound(MyIntegrityChangeEnum.ConstructionBegin, false);
          this.UpdateGridAABB();
        }
        if (MySession.Static.LocalPlayerId != ownerId)
          return;
        if (flag)
          MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
        else
          MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      }
      finally
      {
        this.m_tmpBuildSuccessBlocks.Clear();
      }
    }

    private void GetAllBuildOffsetsExcept(
      ref MyCubeGrid.MyBlockBuildArea area,
      HashSet<Vector3UByte> exceptList,
      List<Vector3UByte> resultOffsets)
    {
      Vector3UByte vector3Ubyte;
      for (vector3Ubyte.X = (byte) 0; (int) vector3Ubyte.X < (int) area.BuildAreaSize.X; ++vector3Ubyte.X)
      {
        for (vector3Ubyte.Y = (byte) 0; (int) vector3Ubyte.Y < (int) area.BuildAreaSize.Y; ++vector3Ubyte.Y)
        {
          for (vector3Ubyte.Z = (byte) 0; (int) vector3Ubyte.Z < (int) area.BuildAreaSize.Z; ++vector3Ubyte.Z)
          {
            if (!exceptList.Contains(vector3Ubyte))
              resultOffsets.Add(vector3Ubyte);
          }
        }
      }
    }

    private void GetValidBuildOffsets(
      ref MyCubeGrid.MyBlockBuildArea area,
      List<Vector3UByte> resultOffsets,
      HashSet<Vector3UByte> resultFailList,
      ulong placingPlayer = 0)
    {
      Vector3I stepDelta = (Vector3I) area.StepDelta;
      MyBlockOrientation orientation = new MyBlockOrientation(area.OrientationForward, area.OrientationUp);
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) area.DefinitionId);
      Vector3UByte vector3Ubyte;
      for (vector3Ubyte.X = (byte) 0; (int) vector3Ubyte.X < (int) area.BuildAreaSize.X; ++vector3Ubyte.X)
      {
        for (vector3Ubyte.Y = (byte) 0; (int) vector3Ubyte.Y < (int) area.BuildAreaSize.Y; ++vector3Ubyte.Y)
        {
          for (vector3Ubyte.Z = (byte) 0; (int) vector3Ubyte.Z < (int) area.BuildAreaSize.Z; ++vector3Ubyte.Z)
          {
            Vector3I vector3I = area.PosInGrid + (Vector3I) vector3Ubyte * stepDelta;
            if (this.CanPlaceBlock(vector3I + (Vector3I) area.BlockMin, vector3I + (Vector3I) area.BlockMax, orientation, cubeBlockDefinition, placingPlayer))
              resultOffsets.Add(vector3Ubyte);
            else
              resultFailList.Add(vector3Ubyte);
          }
        }
      }
    }

    private void BuildBlocksSuccess(
      Vector3 colorMaskHsv,
      MyStringHash skinId,
      HashSet<MyCubeGrid.MyBlockLocation> locations,
      HashSet<MyCubeGrid.MyBlockLocation> resultBlocks,
      MyEntity builder,
      bool instantBuilt,
      long ownerId,
      ulong placingPlayer = 0)
    {
      bool flag = true;
      while (locations.Count > 0 & flag)
      {
        flag = false;
        foreach (MyCubeGrid.MyBlockLocation location in locations)
        {
          Quaternion result;
          location.Orientation.GetQuaternion(out result);
          Vector3I centerPos = location.CenterPos;
          MyCubeBlockDefinition blockDefinition;
          MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) location.BlockDefinition, out blockDefinition);
          if (blockDefinition == null)
            return;
          MyCubeBlockDefinition.MountPoint[] modelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
          if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.CanPlaceWithConnectivity(location, ref result, ref centerPos, blockDefinition, modelMountPoints, placingPlayer))
          {
            MySlimBlock block = this.BuildBlock(blockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, builder, testMerge: false, buildAsAdmin: instantBuilt);
            if (block != null)
            {
              MyCubeGrid.ChangeBlockOwner(block, ownerId);
              MyCubeGrid.MyBlockLocation myBlockLocation = location;
              resultBlocks.Add(myBlockLocation);
            }
            flag = true;
            locations.Remove(location);
            break;
          }
        }
      }
    }

    private bool CanPlaceWithConnectivity(
      MyCubeGrid.MyBlockLocation location,
      ref Quaternion orientation,
      ref Vector3I center,
      MyCubeBlockDefinition blockDefinition,
      MyCubeBlockDefinition.MountPoint[] mountPoints,
      ulong placingPlayer = 0)
    {
      return this.CanPlaceBlock(location.Min, location.Max, location.Orientation, blockDefinition, placingPlayer) && MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) this, blockDefinition, mountPoints, ref orientation, ref center);
    }

    private MySlimBlock BuildBlockSuccess(
      Vector3 colorMaskHsv,
      MyStringHash skinId,
      MyCubeGrid.MyBlockLocation location,
      MyObjectBuilder_CubeBlock objectBuilder,
      ref MyCubeGrid.MyBlockLocation? resultBlock,
      MyEntity builder,
      bool instantBuilt,
      long ownerId)
    {
      Quaternion result;
      location.Orientation.GetQuaternion(out result);
      MyCubeBlockDefinition blockDefinition;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) location.BlockDefinition, out blockDefinition);
      if (blockDefinition == null)
        return (MySlimBlock) null;
      MySlimBlock block = this.BuildBlock(blockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, instantBuilt ? (MyEntity) null : builder, objectBuilder);
      if (block != null)
      {
        MyCubeGrid.ChangeBlockOwner(block, ownerId);
        resultBlock = new MyCubeGrid.MyBlockLocation?(location);
        block.PlayConstructionSound(MyIntegrityChangeEnum.ConstructionBegin, false);
      }
      else
        resultBlock = new MyCubeGrid.MyBlockLocation?();
      return block;
    }

    private static void ChangeBlockOwner(MySlimBlock block, long ownerId)
    {
      if (block.FatBlock == null)
        return;
      block.FatBlock.ChangeOwner(ownerId, MyOwnershipShareModeEnum.Faction);
    }

    private void AfterBuildBlocksSuccess(
      HashSet<MyCubeGrid.MyBlockLocation> builtBlocks,
      bool instantBuild)
    {
      foreach (MyCubeGrid.MyBlockLocation builtBlock in builtBlocks)
      {
        this.AfterBuildBlockSuccess(builtBlock, instantBuild);
        this.DetectMerge(this.GetCubeBlock(builtBlock.CenterPos));
      }
    }

    private void AfterBuildBlockSuccess(MyCubeGrid.MyBlockLocation builtBlock, bool instantBuild)
    {
      MySlimBlock cubeBlock = this.GetCubeBlock(builtBlock.CenterPos);
      if (cubeBlock == null || cubeBlock.FatBlock == null)
        return;
      cubeBlock.FatBlock.OnBuildSuccess(builtBlock.Owner, instantBuild);
    }

    public void RazeBlocksDelayed(ref Vector3I pos, ref Vector3UByte size, long builderEntityId)
    {
      bool flag = false;
      Vector3UByte vector3Ubyte;
      for (vector3Ubyte.X = (byte) 0; (int) vector3Ubyte.X <= (int) size.X; ++vector3Ubyte.X)
      {
        for (vector3Ubyte.Y = (byte) 0; (int) vector3Ubyte.Y <= (int) size.Y; ++vector3Ubyte.Y)
        {
          for (vector3Ubyte.Z = (byte) 0; (int) vector3Ubyte.Z <= (int) size.Z; ++vector3Ubyte.Z)
          {
            MySlimBlock cubeBlock = this.GetCubeBlock(pos + (Vector3I) vector3Ubyte);
            if (cubeBlock != null && cubeBlock.FatBlock != null && (!cubeBlock.FatBlock.IsSubBlock && cubeBlock.FatBlock is MyCockpit fatBlock) && fatBlock.Pilot != null)
            {
              if (!flag)
              {
                flag = true;
                this.m_isRazeBatchDelayed = true;
                this.m_delayedRazeBatch = new MyDelayedRazeBatch(pos, size);
                this.m_delayedRazeBatch.Occupied = new HashSet<MyCockpit>();
              }
              this.m_delayedRazeBatch.Occupied.Add(fatBlock);
            }
          }
        }
      }
      if (!flag)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3UByte, long, ulong>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3UByte, long, ulong>>) (x => new Action<Vector3I, Vector3UByte, long, ulong>(x.RazeBlocksAreaRequest)), pos, size, builderEntityId, Sandbox.Game.Multiplayer.Sync.MyId);
      else if (!MySession.Static.CreativeMode && Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null && MySession.Static.IsUserAdmin(Sandbox.Game.Multiplayer.Sync.MyId))
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.RemovePilotToo), callback: callback));
      }
      else
        this.OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum.NO);
    }

    public void OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (!this.m_isRazeBatchDelayed)
        return;
      if (this.Closed)
      {
        this.m_delayedRazeBatch.Occupied.Clear();
        this.m_delayedRazeBatch = (MyDelayedRazeBatch) null;
        this.m_isRazeBatchDelayed = false;
      }
      else
      {
        if (result == MyGuiScreenMessageBox.ResultEnum.NO)
        {
          foreach (MyCockpit myCockpit in this.m_delayedRazeBatch.Occupied)
          {
            if (myCockpit.Pilot != null && !myCockpit.MarkedForClose)
              myCockpit.RequestRemovePilot();
          }
        }
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3UByte, long, ulong>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3UByte, long, ulong>>) (x => new Action<Vector3I, Vector3UByte, long, ulong>(x.RazeBlocksAreaRequest)), this.m_delayedRazeBatch.Pos, this.m_delayedRazeBatch.Size, MySession.Static.LocalCharacterEntityId, Sandbox.Game.Multiplayer.Sync.MyId);
        this.m_delayedRazeBatch.Occupied.Clear();
        this.m_delayedRazeBatch = (MyDelayedRazeBatch) null;
        this.m_isRazeBatchDelayed = false;
      }
    }

    public void RazeBlocks(ref Vector3I pos, ref Vector3UByte size, long builderEntityId = 0)
    {
      ulong num = 0;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3UByte, long, ulong>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3UByte, long, ulong>>) (x => new Action<Vector3I, Vector3UByte, long, ulong>(x.RazeBlocksAreaRequest)), pos, size, builderEntityId, num);
    }

    [Event(null, 4205)]
    [Reliable]
    [Server]
    private void RazeBlocksAreaRequest(
      Vector3I pos,
      Vector3UByte size,
      long builderEntityId,
      ulong placingPlayer)
    {
      if (!MySession.Static.CreativeMode && (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value)))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        try
        {
          Vector3UByte vector3Ubyte;
          for (vector3Ubyte.X = (byte) 0; (int) vector3Ubyte.X <= (int) size.X; ++vector3Ubyte.X)
          {
            for (vector3Ubyte.Y = (byte) 0; (int) vector3Ubyte.Y <= (int) size.Y; ++vector3Ubyte.Y)
            {
              for (vector3Ubyte.Z = (byte) 0; (int) vector3Ubyte.Z <= (int) size.Z; ++vector3Ubyte.Z)
              {
                MySlimBlock cubeBlock = this.GetCubeBlock(pos + (Vector3I) vector3Ubyte);
                if (cubeBlock == null || cubeBlock.FatBlock != null && cubeBlock.FatBlock.IsSubBlock)
                  this.m_tmpBuildFailList.Add(vector3Ubyte);
              }
            }
          }
          if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Building, builderEntityId, placingPlayer))
            return;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3UByte, HashSet<Vector3UByte>>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3UByte, HashSet<Vector3UByte>>>) (x => new Action<Vector3I, Vector3UByte, HashSet<Vector3UByte>>(x.RazeBlocksAreaSuccess)), pos, size, this.m_tmpBuildFailList);
          this.RazeBlocksAreaSuccess(pos, size, this.m_tmpBuildFailList);
        }
        finally
        {
          this.m_tmpBuildFailList.Clear();
        }
      }
    }

    [Event(null, 4238)]
    [Reliable]
    [Broadcast]
    private void RazeBlocksAreaSuccess(
      Vector3I pos,
      Vector3UByte size,
      HashSet<Vector3UByte> resultFailList)
    {
      Vector3I min = Vector3I.MaxValue;
      Vector3I max = Vector3I.MinValue;
      if (MyFakes.ENABLE_MULTIBLOCKS)
      {
        Vector3UByte vector3Ubyte;
        for (vector3Ubyte.X = (byte) 0; (int) vector3Ubyte.X <= (int) size.X; ++vector3Ubyte.X)
        {
          for (vector3Ubyte.Y = (byte) 0; (int) vector3Ubyte.Y <= (int) size.Y; ++vector3Ubyte.Y)
          {
            for (vector3Ubyte.Z = (byte) 0; (int) vector3Ubyte.Z <= (int) size.Z; ++vector3Ubyte.Z)
            {
              if (!resultFailList.Contains(vector3Ubyte))
              {
                MySlimBlock cubeBlock = this.GetCubeBlock(pos + (Vector3I) vector3Ubyte);
                if (cubeBlock != null)
                {
                  if (cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock)
                  {
                    MyCubeGrid.m_tmpSlimBlocks.Clear();
                    MyCubeGrid.m_tmpSlimBlocks.AddRange((IEnumerable<MySlimBlock>) fatBlock.GetBlocks());
                    foreach (MySlimBlock tmpSlimBlock in MyCubeGrid.m_tmpSlimBlocks)
                    {
                      if (tmpSlimBlock.IsMultiBlockPart)
                      {
                        MyCubeGrid.m_tmpBlocksInMultiBlock.Clear();
                        this.GetBlocksInMultiBlock(tmpSlimBlock.MultiBlockId, MyCubeGrid.m_tmpBlocksInMultiBlock);
                        this.RemoveMultiBlocks(ref min, ref max, MyCubeGrid.m_tmpBlocksInMultiBlock);
                        MyCubeGrid.m_tmpBlocksInMultiBlock.Clear();
                      }
                      else
                      {
                        ushort? blockId = fatBlock.GetBlockId(tmpSlimBlock);
                        if (blockId.HasValue)
                          this.RemoveBlockInCompound(tmpSlimBlock.Position, blockId.Value, ref min, ref max);
                      }
                    }
                    MyCubeGrid.m_tmpSlimBlocks.Clear();
                  }
                  else if (cubeBlock.IsMultiBlockPart)
                  {
                    MyCubeGrid.m_tmpBlocksInMultiBlock.Clear();
                    this.GetBlocksInMultiBlock(cubeBlock.MultiBlockId, MyCubeGrid.m_tmpBlocksInMultiBlock);
                    this.RemoveMultiBlocks(ref min, ref max, MyCubeGrid.m_tmpBlocksInMultiBlock);
                    MyCubeGrid.m_tmpBlocksInMultiBlock.Clear();
                  }
                  else if (cubeBlock.FatBlock is MyFracturedBlock fatBlock && fatBlock.MultiBlocks != null && fatBlock.MultiBlocks.Count > 0)
                  {
                    foreach (MyFracturedBlock.MultiBlockPartInfo multiBlock in fatBlock.MultiBlocks)
                    {
                      if (multiBlock != null)
                      {
                        MyCubeGrid.m_tmpBlocksInMultiBlock.Clear();
                        if (MyDefinitionManager.Static.TryGetMultiBlockDefinition(multiBlock.MultiBlockDefinition) != null)
                        {
                          this.GetBlocksInMultiBlock(multiBlock.MultiBlockId, MyCubeGrid.m_tmpBlocksInMultiBlock);
                          this.RemoveMultiBlocks(ref min, ref max, MyCubeGrid.m_tmpBlocksInMultiBlock);
                        }
                        MyCubeGrid.m_tmpBlocksInMultiBlock.Clear();
                      }
                    }
                  }
                  else
                  {
                    min = Vector3I.Min(min, cubeBlock.Min);
                    max = Vector3I.Max(max, cubeBlock.Max);
                    this.RemoveBlockByCubeBuilder(cubeBlock);
                  }
                }
              }
            }
          }
        }
      }
      else
      {
        Vector3UByte vector3Ubyte;
        for (vector3Ubyte.X = (byte) 0; (int) vector3Ubyte.X <= (int) size.X; ++vector3Ubyte.X)
        {
          for (vector3Ubyte.Y = (byte) 0; (int) vector3Ubyte.Y <= (int) size.Y; ++vector3Ubyte.Y)
          {
            for (vector3Ubyte.Z = (byte) 0; (int) vector3Ubyte.Z <= (int) size.Z; ++vector3Ubyte.Z)
            {
              if (!resultFailList.Contains(vector3Ubyte))
              {
                MySlimBlock cubeBlock = this.GetCubeBlock(pos + (Vector3I) vector3Ubyte);
                if (cubeBlock != null)
                {
                  min = Vector3I.Min(min, cubeBlock.Min);
                  max = Vector3I.Max(max, cubeBlock.Max);
                  this.RemoveBlockByCubeBuilder(cubeBlock);
                }
              }
            }
          }
        }
      }
      if (this.Physics == null)
        return;
      this.Physics.AddDirtyArea(min, max);
    }

    private void RemoveMultiBlocks(
      ref Vector3I min,
      ref Vector3I max,
      HashSet<Tuple<MySlimBlock, ushort?>> tmpBlocksInMultiBlock)
    {
      foreach (Tuple<MySlimBlock, ushort?> tuple in tmpBlocksInMultiBlock)
      {
        ushort? nullable = tuple.Item2;
        if (nullable.HasValue)
        {
          Vector3I position = tuple.Item1.Position;
          nullable = tuple.Item2;
          int num = (int) nullable.Value;
          ref Vector3I local1 = ref min;
          ref Vector3I local2 = ref max;
          this.RemoveBlockInCompound(position, (ushort) num, ref local1, ref local2);
        }
        else
        {
          min = Vector3I.Min(min, tuple.Item1.Min);
          max = Vector3I.Max(max, tuple.Item1.Max);
          this.RemoveBlockByCubeBuilder(tuple.Item1);
        }
      }
    }

    public void RazeBlock(Vector3I position, ulong user = 0)
    {
      MyCubeGrid.m_tmpPositionListSend.Clear();
      MyCubeGrid.m_tmpPositionListSend.Add(position);
      this.RazeBlocks(MyCubeGrid.m_tmpPositionListSend, user: user);
    }

    public void RazeBlocks(List<Vector3I> locations, long builderEntityId = 0, ulong user = 0) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<Vector3I>, long, ulong>(this, (Func<MyCubeGrid, Action<List<Vector3I>, long, ulong>>) (x => new Action<List<Vector3I>, long, ulong>(x.RazeBlocksRequest)), locations, builderEntityId, user);

    [Event(null, 4406)]
    [Reliable]
    [Server]
    public void RazeBlocksRequest(List<Vector3I> locations, long builderEntityId = 0, ulong user = 0)
    {
      MyCubeGrid.m_tmpPositionListReceive.Clear();
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Grinding, builderEntityId, MyEventContext.Current.IsLocallyInvoked ? user : MyEventContext.Current.Sender.Value))
        return;
      this.RazeBlocksSuccess(locations, MyCubeGrid.m_tmpPositionListReceive);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<Vector3I>>(this, (Func<MyCubeGrid, Action<List<Vector3I>>>) (x => new Action<List<Vector3I>>(x.RazeBlocksClient)), MyCubeGrid.m_tmpPositionListReceive);
    }

    [Event(null, 4419)]
    [Reliable]
    [Broadcast]
    public void RazeBlocksClient(List<Vector3I> locations)
    {
      MyCubeGrid.m_tmpPositionListReceive.Clear();
      this.RazeBlocksSuccess(locations, MyCubeGrid.m_tmpPositionListReceive);
    }

    private void RazeBlocksSuccess(List<Vector3I> locations, List<Vector3I> removedBlocks)
    {
      Vector3I min = Vector3I.MaxValue;
      Vector3I max = Vector3I.MinValue;
      foreach (Vector3I location in locations)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(location);
        if (cubeBlock != null)
        {
          removedBlocks.Add(location);
          min = Vector3I.Min(min, cubeBlock.Min);
          max = Vector3I.Max(max, cubeBlock.Max);
          this.RemoveBlockByCubeBuilder(cubeBlock);
        }
      }
      if (this.Physics == null)
        return;
      this.Physics.AddDirtyArea(min, max);
    }

    public void RazeGeneratedBlocks(List<Vector3I> locations)
    {
      Vector3I min = Vector3I.MaxValue;
      Vector3I max = Vector3I.MinValue;
      foreach (Vector3I location in locations)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(location);
        if (cubeBlock != null)
        {
          min = Vector3I.Min(min, cubeBlock.Min);
          max = Vector3I.Max(max, cubeBlock.Max);
          this.RemoveBlockByCubeBuilder(cubeBlock);
        }
      }
      if (this.Physics == null)
        return;
      this.Physics.AddDirtyArea(min, max);
    }

    private void RazeBlockInCompoundBlockSuccess(
      List<MyCubeGrid.LocationIdentity> locationsAndIds,
      List<Tuple<Vector3I, ushort>> removedBlocks)
    {
      Vector3I maxValue = Vector3I.MaxValue;
      Vector3I minValue = Vector3I.MinValue;
      foreach (MyCubeGrid.LocationIdentity locationsAndId in locationsAndIds)
        this.RemoveBlockInCompound(locationsAndId.Location, locationsAndId.Id, ref maxValue, ref minValue, removedBlocks);
      this.m_dirtyRegion.AddCubeRegion(maxValue, minValue);
      this.ScheduleDirtyRegion();
      if (this.Physics != null)
        this.Physics.AddDirtyArea(maxValue, minValue);
      this.MarkForDraw();
    }

    private void RemoveBlockInCompound(
      Vector3I position,
      ushort compoundBlockId,
      ref Vector3I min,
      ref Vector3I max,
      List<Tuple<Vector3I, ushort>> removedBlocks = null)
    {
      MySlimBlock cubeBlock = this.GetCubeBlock(position);
      if (cubeBlock == null || !(cubeBlock.FatBlock is MyCompoundCubeBlock))
        return;
      MyCompoundCubeBlock fatBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
      this.RemoveBlockInCompoundInternal(position, compoundBlockId, ref min, ref max, removedBlocks, cubeBlock, fatBlock);
    }

    public void RazeGeneratedBlocksInCompoundBlock(List<Tuple<Vector3I, ushort>> locationsAndIds)
    {
      Vector3I maxValue = Vector3I.MaxValue;
      Vector3I minValue = Vector3I.MinValue;
      foreach (Tuple<Vector3I, ushort> locationsAndId in locationsAndIds)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(locationsAndId.Item1);
        if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
        {
          MyCompoundCubeBlock fatBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
          this.RemoveBlockInCompoundInternal(locationsAndId.Item1, locationsAndId.Item2, ref maxValue, ref minValue, (List<Tuple<Vector3I, ushort>>) null, cubeBlock, fatBlock);
        }
      }
      this.m_dirtyRegion.AddCubeRegion(maxValue, minValue);
      if (this.Physics != null)
        this.Physics.AddDirtyArea(maxValue, minValue);
      this.ScheduleDirtyRegion();
      this.MarkForDraw();
    }

    private void RemoveBlockInCompoundInternal(
      Vector3I position,
      ushort compoundBlockId,
      ref Vector3I min,
      ref Vector3I max,
      List<Tuple<Vector3I, ushort>> removedBlocks,
      MySlimBlock block,
      MyCompoundCubeBlock compoundBlock)
    {
      MySlimBlock block1 = compoundBlock.GetBlock(compoundBlockId);
      if (block1 != null && compoundBlock.Remove(block1))
      {
        removedBlocks?.Add(new Tuple<Vector3I, ushort>(position, compoundBlockId));
        min = Vector3I.Min(min, block.Min);
        max = Vector3I.Max(max, block.Max);
        if (MyCubeGridSmallToLargeConnection.Static != null && this.m_enableSmallToLargeConnections)
          MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(block1);
        this.NotifyBlockRemoved(block1);
      }
      if (compoundBlock.GetBlocksCount() != 0)
        return;
      this.RemoveBlockByCubeBuilder(block);
    }

    public void RazeGeneratedBlocks(List<MySlimBlock> generatedBlocks)
    {
      MyCubeGrid.m_tmpRazeList.Clear();
      MyCubeGrid.m_tmpLocations.Clear();
      foreach (MySlimBlock generatedBlock in generatedBlocks)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(generatedBlock.Position);
        if (cubeBlock != null)
        {
          if (cubeBlock.FatBlock is MyCompoundCubeBlock)
          {
            ushort? blockId = (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlockId(generatedBlock);
            if (blockId.HasValue)
              MyCubeGrid.m_tmpRazeList.Add(new Tuple<Vector3I, ushort>(generatedBlock.Position, blockId.Value));
          }
          else
            MyCubeGrid.m_tmpLocations.Add(generatedBlock.Position);
        }
      }
      if (MyCubeGrid.m_tmpLocations.Count > 0)
        this.RazeGeneratedBlocks(MyCubeGrid.m_tmpLocations);
      if (MyCubeGrid.m_tmpRazeList.Count > 0)
        this.RazeGeneratedBlocksInCompoundBlock(MyCubeGrid.m_tmpRazeList);
      MyCubeGrid.m_tmpRazeList.Clear();
      MyCubeGrid.m_tmpLocations.Clear();
    }

    private void ScheduleDirtyRegion()
    {
      if (this.m_dirtyRegionScheduled)
        return;
      this.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateDirtyRegion), 8, true);
      this.m_dirtyRegionScheduled = true;
    }

    private void UpdateDirtyRegion()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.ClearDirty();
        this.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateDirtyRegion));
        this.m_dirtyRegionScheduled = false;
      }
      else
      {
        if (this.m_updatingDirty || !this.m_dirtyRegion.IsDirty)
          return;
        this.UpdateDirty();
      }
    }

    public void ColorBlocks(
      Vector3I min,
      Vector3I max,
      Vector3 newHSV,
      bool playSound,
      bool validateOwnership)
    {
      long num = validateOwnership ? MySession.Static.LocalPlayerId : 0L;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3I, Vector3, bool, long>>) (x => new Action<Vector3I, Vector3I, Vector3, bool, long>(x.ColorBlockRequest)), min, max, newHSV, playSound, num);
    }

    public void ColorGrid(Vector3 newHSV, bool playSound, bool validateOwnership)
    {
      long num = validateOwnership ? MySession.Static.LocalPlayerId : 0L;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3, bool, long>(this, (Func<MyCubeGrid, Action<Vector3, bool, long>>) (x => new Action<Vector3, bool, long>(x.ColorGridFriendlyRequest)), newHSV, playSound, num);
    }

    [Event(null, 4654)]
    [Reliable]
    [Server]
    private void ColorGridFriendlyRequest(Vector3 newHSV, bool playSound, long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3, bool, long>(this, (Func<MyCubeGrid, Action<Vector3, bool, long>>) (x => new Action<Vector3, bool, long>(x.OnColorGridFriendly)), newHSV, playSound, player);
    }

    [Event(null, 4663)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnColorGridFriendly(Vector3 newHSV, bool playSound, long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      bool flag = false;
      foreach (MySlimBlock cubeBlock in this.CubeBlocks)
        flag |= this.ChangeColorAndSkin(cubeBlock, new Vector3?(newHSV));
      if (playSound & flag)
        MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
      this.GridSystems.EmissiveSystem?.UpdateEmissivity();
    }

    [Event(null, 4688)]
    [Reliable]
    [Server]
    private void ColorBlockRequest(
      Vector3I min,
      Vector3I max,
      Vector3 newHSV,
      bool playSound,
      long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3I, Vector3, bool, long>>) (x => new Action<Vector3I, Vector3I, Vector3, bool, long>(x.OnColorBlock)), min, max, newHSV, playSound, player);
    }

    [Event(null, 4697)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnColorBlock(
      Vector3I min,
      Vector3I max,
      Vector3 newHSV,
      bool playSound,
      long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      bool flag = false;
      Vector3I pos;
      for (pos.X = min.X; pos.X <= max.X; ++pos.X)
      {
        for (pos.Y = min.Y; pos.Y <= max.Y; ++pos.Y)
        {
          for (pos.Z = min.Z; pos.Z <= max.Z; ++pos.Z)
          {
            MySlimBlock cubeBlock = this.GetCubeBlock(pos);
            if (cubeBlock != null)
              flag |= this.ChangeColorAndSkin(cubeBlock, new Vector3?(newHSV));
          }
        }
      }
      if (playSound & flag && Vector3D.Distance(MySector.MainCamera.Position, Vector3D.Transform(min * this.GridSize, this.WorldMatrix)) < 200.0)
        MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
      this.GridSystems.EmissiveSystem?.UpdateEmissivity();
    }

    public static MyGameInventoryItem GetArmorSkinItem(MyStringHash skinId)
    {
      if (skinId == MyStringHash.NullOrEmpty || MyGameService.InventoryItems == null)
        return (MyGameInventoryItem) null;
      foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
      {
        if (inventoryItem.ItemDefinition != null && inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor && !(MyStringHash.GetOrCompute(inventoryItem.ItemDefinition.AssetModifierId) != skinId))
          return inventoryItem;
      }
      return (MyGameInventoryItem) null;
    }

    public void SkinBlocks(
      Vector3I min,
      Vector3I max,
      Vector3? newHSV,
      MyStringHash? newSkin,
      bool playSound,
      bool validateOwnership)
    {
      long num = validateOwnership ? MySession.Static.LocalPlayerId : 0L;
      MyCubeGrid.MyBlockVisuals myBlockVisuals = new MyCubeGrid.MyBlockVisuals(newHSV.HasValue ? newHSV.Value.PackHSVToUint() : 0U, newSkin.HasValue ? newSkin.Value : MyStringHash.NullOrEmpty, newHSV.HasValue, newSkin.HasValue);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long>>) (x => new Action<Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long>(x.SkinBlockRequest)), min, max, myBlockVisuals, playSound, num);
    }

    public void SkinGrid(
      Vector3 newHSV,
      MyStringHash newSkin,
      bool playSound,
      bool validateOwnership,
      bool applyColor,
      bool applySkin)
    {
      if (!applyColor && !applySkin)
        return;
      long num = validateOwnership ? MySession.Static.LocalPlayerId : 0L;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockVisuals, bool, long>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockVisuals, bool, long>>) (x => new Action<MyCubeGrid.MyBlockVisuals, bool, long>(x.SkinGridFriendlyRequest)), new MyCubeGrid.MyBlockVisuals(newHSV.PackHSVToUint(), newSkin, applyColor, applySkin), playSound, num);
    }

    [Event(null, 4789)]
    [Reliable]
    [Server]
    private void SkinGridFriendlyRequest(
      MyCubeGrid.MyBlockVisuals visuals,
      bool playSound,
      long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      ref MyCubeGrid.MyBlockVisuals local = ref visuals;
      MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
      MyStringHash myStringHash = component != null ? component.ValidateArmor(visuals.SkinId, MyEventContext.Current.Sender.Value) : MyStringHash.NullOrEmpty;
      local.SkinId = myStringHash;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockVisuals, bool, long>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockVisuals, bool, long>>) (x => new Action<MyCubeGrid.MyBlockVisuals, bool, long>(x.OnSkinGridFriendly)), visuals, playSound, player);
    }

    [Event(null, 4801)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnSkinGridFriendly(MyCubeGrid.MyBlockVisuals visuals, bool playSound, long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      Vector3 vector3 = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
      bool flag = false;
      foreach (MySlimBlock cubeBlock in this.CubeBlocks)
        flag |= this.ChangeColorAndSkin(cubeBlock, visuals.ApplyColor ? new Vector3?(vector3) : new Vector3?(), visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : new MyStringHash?());
      if (playSound & flag)
        MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
      this.GridSystems.EmissiveSystem?.UpdateEmissivity();
    }

    [Event(null, 4829)]
    [Reliable]
    [Server]
    private void SkinBlockRequest(
      Vector3I min,
      Vector3I max,
      MyCubeGrid.MyBlockVisuals visuals,
      bool playSound,
      long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      ref MyCubeGrid.MyBlockVisuals local = ref visuals;
      MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
      MyStringHash myStringHash = component != null ? component.ValidateArmor(visuals.SkinId, MyEventContext.Current.Sender.Value) : MyStringHash.NullOrEmpty;
      local.SkinId = myStringHash;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long>(this, (Func<MyCubeGrid, Action<Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long>>) (x => new Action<Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long>(x.OnSkinBlock)), min, max, visuals, playSound, player);
    }

    [Event(null, 4841)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnSkinBlock(
      Vector3I min,
      Vector3I max,
      MyCubeGrid.MyBlockVisuals visuals,
      bool playSound,
      long player)
    {
      if (!this.ColorGridOrBlockRequestValidation(player))
        return;
      Vector3 vector3 = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
      bool flag = false;
      Vector3I pos;
      for (pos.X = min.X; pos.X <= max.X; ++pos.X)
      {
        for (pos.Y = min.Y; pos.Y <= max.Y; ++pos.Y)
        {
          for (pos.Z = min.Z; pos.Z <= max.Z; ++pos.Z)
          {
            MySlimBlock cubeBlock = this.GetCubeBlock(pos);
            if (cubeBlock != null)
              flag |= this.ChangeColorAndSkin(cubeBlock, visuals.ApplyColor ? new Vector3?(vector3) : new Vector3?(), visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : new MyStringHash?());
          }
        }
      }
      if (playSound & flag && Vector3D.Distance(MySector.MainCamera.Position, Vector3D.Transform(min * this.GridSize, this.WorldMatrix)) < 200.0)
        MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
      this.GridSystems.EmissiveSystem?.UpdateEmissivity();
    }

    public bool ColorGridOrBlockRequestValidation(long player)
    {
      if (player == 0L || !Sandbox.Game.Multiplayer.Sync.IsServer || this.BigOwners.Count == 0)
        return true;
      foreach (long bigOwner in this.BigOwners)
      {
        if (MyIDModule.GetRelationPlayerPlayer(bigOwner, player) == MyRelationsBetweenPlayers.Self)
          return true;
      }
      return false;
    }

    private MySlimBlock BuildBlock(
      MyCubeBlockDefinition blockDefinition,
      Vector3 colorMaskHsv,
      MyStringHash skinId,
      Vector3I min,
      Quaternion orientation,
      long owner,
      long entityId,
      MyEntity builderEntity,
      MyObjectBuilder_CubeBlock blockObjectBuilder = null,
      bool updateVolume = true,
      bool testMerge = true,
      bool buildAsAdmin = false)
    {
      MyBlockOrientation orientation1 = new MyBlockOrientation(ref orientation);
      if (blockObjectBuilder == null)
      {
        blockObjectBuilder = MyCubeGrid.CreateBlockObjectBuilder(blockDefinition, min, orientation1, entityId, owner, ((builderEntity == null ? 1 : (!MySession.Static.SurvivalMode ? 1 : 0)) | (buildAsAdmin ? 1 : 0)) != 0);
        blockObjectBuilder.ColorMaskHSV = (SerializableVector3) colorMaskHsv;
        blockObjectBuilder.SkinSubtypeId = skinId.String;
      }
      else
      {
        blockObjectBuilder.Min = (SerializableVector3I) min;
        blockObjectBuilder.Orientation = (SerializableQuaternion) orientation;
      }
      MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, builderEntity, blockObjectBuilder, buildAsAdmin);
      block = (MySlimBlock) null;
      Vector3I positionInGrid = MySlimBlock.ComputePositionInGrid(new MatrixI(orientation1), blockDefinition, min);
      if (!MyEntities.IsInsideWorld(this.GridIntegerToWorld(positionInGrid)))
        return (MySlimBlock) null;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        MyCubeBuilder.BuildComponent.GetBlockPlacementMaterials(blockDefinition, positionInGrid, (MyBlockOrientation) blockObjectBuilder.BlockOrientation, this);
      if (MyFakes.ENABLE_COMPOUND_BLOCKS && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
      {
        MySlimBlock cubeBlock1 = this.GetCubeBlock(min);
        MyCompoundCubeBlock compoundCubeBlock = cubeBlock1 != null ? cubeBlock1.FatBlock as MyCompoundCubeBlock : (MyCompoundCubeBlock) null;
        if (compoundCubeBlock != null)
        {
          if (compoundCubeBlock.CanAddBlock(blockDefinition, new MyBlockOrientation?(new MyBlockOrientation(ref orientation))))
          {
            object cubeBlock2 = MyCubeBlockFactory.CreateCubeBlock(blockObjectBuilder);
            if (!(cubeBlock2 is MySlimBlock block))
              block = new MySlimBlock();
            block.Init(blockObjectBuilder, this, cubeBlock2 as MyCubeBlock);
            block.FatBlock.HookMultiplayer();
            if (compoundCubeBlock.Add(block, out ushort _))
            {
              this.BoundsInclude(block);
              this.m_dirtyRegion.AddCube(min);
              this.ScheduleDirtyRegion();
              if (this.Physics != null)
                this.Physics.AddDirtyBlock(cubeBlock1);
              this.NotifyBlockAdded(block);
            }
          }
        }
        else
          block = this.AddBlock((MyObjectBuilder_CubeBlock) MyCompoundCubeBlock.CreateBuilder(blockObjectBuilder), testMerge);
        this.MarkForDraw();
      }
      else
        block = this.AddBlock(blockObjectBuilder, testMerge);
      if (block != null)
      {
        block.CubeGrid.BoundsInclude(block);
        if (updateVolume)
          block.CubeGrid.UpdateGridAABB();
        if (MyCubeGridSmallToLargeConnection.Static != null && this.m_enableSmallToLargeConnections)
          MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builderEntity, buildAsAdmin);
        MyCubeGrids.NotifyBlockBuilt(this, block);
      }
      return block;
    }

    internal void PerformCutouts(List<MyGridPhysics.ExplosionInfo> explosions)
    {
      if (explosions.Count == 0 || !MySession.Static.Settings.EnableVoxelDestruction)
        return;
      BoundingSphereD sphere1 = new BoundingSphereD(explosions[0].Position, (double) explosions[0].Radius);
      for (int index = 0; index < explosions.Count; ++index)
        sphere1.Include(new BoundingSphereD(explosions[index].Position, (double) explosions[index].Radius));
      using (MyUtils.ReuseCollection<MyVoxelBase>(ref MyCubeGrid.m_rootVoxelsToCutTmp))
      {
        using (MyUtils.ReuseCollection<MyVoxelBase>(ref MyCubeGrid.m_overlappingVoxelsTmp))
        {
          MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref sphere1, MyCubeGrid.m_overlappingVoxelsTmp);
          foreach (MyVoxelBase myVoxelBase in MyCubeGrid.m_overlappingVoxelsTmp)
            MyCubeGrid.m_rootVoxelsToCutTmp.Add(myVoxelBase.RootVoxel);
          int skipCount = 0;
          Parallel.For(0, explosions.Count, (Action<int>) (i =>
          {
            MyGridPhysics.ExplosionInfo explosion = explosions[i];
            BoundingSphereD sphere2 = new BoundingSphereD(explosion.Position, (double) explosion.Radius);
            for (int index = 0; index < explosions.Count; ++index)
            {
              if (index != i && new BoundingSphereD(explosions[index].Position, (double) explosions[index].Radius).Contains(sphere2) == ContainmentType.Contains)
              {
                ++skipCount;
                return;
              }
            }
            foreach (MyVoxelBase voxelMap in MyCubeGrid.m_rootVoxelsToCutTmp)
            {
              Vector3I cacheMin;
              Vector3I cacheMax;
              if (MyVoxelGenerator.CutOutSphereFast(voxelMap, ref explosion.Position, explosion.Radius, out cacheMin, out cacheMax, false))
              {
                Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyVoxelBase, Vector3D, float, bool>(voxelMap, (Func<MyVoxelBase, Action<Vector3D, float, bool>>) (x => new Action<Vector3D, float, bool>(x.PerformCutOutSphereFast)), explosion.Position, explosion.Radius, true);
                MyCubeGrid.m_notificationQueue.Enqueue(MyTuple.Create<int, MyVoxelBase, Vector3I, Vector3I>(i, voxelMap, cacheMin, cacheMax));
              }
            }
          }), 1, WorkPriority.VeryHigh, new WorkOptions?(Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Voxels, "CutOutVoxel")), true);
        }
      }
      bool flag = false;
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      foreach (MyTuple<int, MyVoxelBase, Vector3I, Vector3I> notification in MyCubeGrid.m_notificationQueue)
      {
        flag = true;
        MyGridPhysics.ExplosionInfo explosion = explosions[notification.Item1];
        invalid.Include(new BoundingSphereD(explosion.Position, (double) explosion.Radius));
        Vector3I voxelRangeMin = notification.Item3;
        Vector3I voxelRangeMax = notification.Item4;
        notification.Item2.RootVoxel.Storage.NotifyRangeChanged(ref voxelRangeMin, ref voxelRangeMax, MyStorageDataTypeFlags.Content);
      }
      if (!flag)
        return;
      MyShapeBox myShapeBox = new MyShapeBox();
      myShapeBox.Boundaries = invalid;
      MyTuple<int, MyVoxelBase, Vector3I, Vector3I> result;
      while (MyCubeGrid.m_notificationQueue.TryDequeue(out result))
      {
        BoundingBoxD worldBoundaries = myShapeBox.GetWorldBoundaries();
        MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, result.Item2, ref worldBoundaries);
      }
    }

    public void ResetBlockSkeleton(MySlimBlock block, bool updateSync = false) => this.MultiplyBlockSkeleton(block, 0.0f, updateSync);

    public void MultiplyBlockSkeleton(MySlimBlock block, float factor, bool updateSync = false)
    {
      if (this.Skeleton == null)
        MyLog.Default.WriteLine("Skeleton null in MultiplyBlockSkeleton!" + (object) this);
      if (this.Physics == null)
        MyLog.Default.WriteLine("Physics null in MultiplyBlockSkeleton!" + (object) this);
      if (block == null || this.Skeleton == null || this.Physics == null)
        return;
      Vector3I vector3I1 = block.Min * 2;
      Vector3I vector3I2 = block.Max * 2 + 2;
      bool flag = false;
      Vector3I pos;
      for (pos.Z = vector3I1.Z; pos.Z <= vector3I2.Z; ++pos.Z)
      {
        for (pos.Y = vector3I1.Y; pos.Y <= vector3I2.Y; ++pos.Y)
        {
          for (pos.X = vector3I1.X; pos.X <= vector3I2.X; ++pos.X)
            flag |= this.Skeleton.MultiplyBone(ref pos, factor, ref block.Min, this);
        }
      }
      if (!flag)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer & updateSync)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, float>(this, (Func<MyCubeGrid, Action<Vector3I, float>>) (x => new Action<Vector3I, float>(x.OnBonesMultiplied)), block.Position, factor);
      Vector3I min = block.Min - Vector3I.One;
      Vector3I max = block.Max + Vector3I.One;
      for (pos.Z = min.Z; pos.Z <= max.Z; ++pos.Z)
      {
        for (pos.Y = min.Y; pos.Y <= max.Y; ++pos.Y)
        {
          for (pos.X = min.X; pos.X <= max.X; ++pos.X)
            this.m_dirtyRegion.AddCube(pos);
        }
      }
      this.Physics.AddDirtyArea(min, max);
      this.ScheduleDirtyRegion();
      this.MarkForDraw();
    }

    public void AddDirtyBone(Vector3I gridPosition, Vector3I boneOffset)
    {
      this.Skeleton.Wrap(ref gridPosition, ref boneOffset);
      Vector3I vector3I = boneOffset - new Vector3I(1, 1, 1);
      Vector3I start = Vector3I.Min(vector3I, new Vector3I(0, 0, 0));
      Vector3I end = Vector3I.Max(vector3I, new Vector3I(0, 0, 0));
      Vector3I next = start;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        this.m_dirtyRegion.AddCube(gridPosition + next);
        vector3IRangeIterator.GetNext(out next);
      }
      this.ScheduleDirtyRegion();
      this.MarkForDraw();
    }

    public MySlimBlock GetCubeBlock(Vector3I pos)
    {
      MyCube myCube;
      return this.m_cubes.TryGetValue(pos, out myCube) ? myCube.CubeBlock : (MySlimBlock) null;
    }

    public MySlimBlock GetCubeBlock(Vector3I pos, ushort? compoundId)
    {
      if (!compoundId.HasValue)
        return this.GetCubeBlock(pos);
      MyCube myCube;
      return this.m_cubes.TryGetValue(pos, out myCube) && myCube.CubeBlock.FatBlock is MyCompoundCubeBlock fatBlock ? fatBlock.GetBlock(compoundId.Value) : (MySlimBlock) null;
    }

    public T GetFirstBlockOfType<T>() where T : MyCubeBlock
    {
      foreach (MySlimBlock cubeBlock in this.m_cubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock is T)
          return cubeBlock.FatBlock as T;
      }
      return default (T);
    }

    public void FixTargetCubeLite(out Vector3I cube, Vector3D fractionalGridPosition) => cube = Vector3I.Round(fractionalGridPosition - 0.5);

    public void FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition)
    {
      cube = Vector3I.Round(fractionalGridPosition);
      fractionalGridPosition += new Vector3(0.5f);
      if (this.m_cubes.ContainsKey(cube))
        return;
      Vector3 vector3_1 = fractionalGridPosition - (Vector3) cube;
      Vector3 vector3_2 = new Vector3(1f) - vector3_1;
      MyCubeGrid.m_neighborDistances[1] = vector3_1.X;
      MyCubeGrid.m_neighborDistances[0] = vector3_2.X;
      MyCubeGrid.m_neighborDistances[3] = vector3_1.Y;
      MyCubeGrid.m_neighborDistances[2] = vector3_2.Y;
      MyCubeGrid.m_neighborDistances[5] = vector3_1.Z;
      MyCubeGrid.m_neighborDistances[4] = vector3_2.Z;
      Vector3 vector3_3 = vector3_1 * vector3_1;
      Vector3 vector3_4 = vector3_2 * vector3_2;
      MyCubeGrid.m_neighborDistances[9] = (float) Math.Sqrt((double) vector3_3.X + (double) vector3_3.Y);
      MyCubeGrid.m_neighborDistances[8] = (float) Math.Sqrt((double) vector3_3.X + (double) vector3_4.Y);
      MyCubeGrid.m_neighborDistances[7] = (float) Math.Sqrt((double) vector3_4.X + (double) vector3_3.Y);
      MyCubeGrid.m_neighborDistances[6] = (float) Math.Sqrt((double) vector3_4.X + (double) vector3_4.Y);
      MyCubeGrid.m_neighborDistances[17] = (float) Math.Sqrt((double) vector3_3.X + (double) vector3_3.Z);
      MyCubeGrid.m_neighborDistances[16] = (float) Math.Sqrt((double) vector3_3.X + (double) vector3_4.Z);
      MyCubeGrid.m_neighborDistances[15] = (float) Math.Sqrt((double) vector3_4.X + (double) vector3_3.Z);
      MyCubeGrid.m_neighborDistances[14] = (float) Math.Sqrt((double) vector3_4.X + (double) vector3_4.Z);
      MyCubeGrid.m_neighborDistances[13] = (float) Math.Sqrt((double) vector3_3.Y + (double) vector3_3.Z);
      MyCubeGrid.m_neighborDistances[12] = (float) Math.Sqrt((double) vector3_3.Y + (double) vector3_4.Z);
      MyCubeGrid.m_neighborDistances[11] = (float) Math.Sqrt((double) vector3_4.Y + (double) vector3_3.Z);
      MyCubeGrid.m_neighborDistances[10] = (float) Math.Sqrt((double) vector3_4.Y + (double) vector3_4.Z);
      Vector3 vector3_5 = vector3_3 * vector3_1;
      Vector3 vector3_6 = vector3_4 * vector3_2;
      MyCubeGrid.m_neighborDistances[25] = (float) Math.Pow((double) vector3_5.X + (double) vector3_5.Y + (double) vector3_5.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[24] = (float) Math.Pow((double) vector3_5.X + (double) vector3_5.Y + (double) vector3_6.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[23] = (float) Math.Pow((double) vector3_5.X + (double) vector3_6.Y + (double) vector3_5.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[22] = (float) Math.Pow((double) vector3_5.X + (double) vector3_6.Y + (double) vector3_6.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[21] = (float) Math.Pow((double) vector3_6.X + (double) vector3_5.Y + (double) vector3_5.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[20] = (float) Math.Pow((double) vector3_6.X + (double) vector3_5.Y + (double) vector3_6.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[19] = (float) Math.Pow((double) vector3_6.X + (double) vector3_6.Y + (double) vector3_5.Z, 1.0 / 3.0);
      MyCubeGrid.m_neighborDistances[18] = (float) Math.Pow((double) vector3_6.X + (double) vector3_6.Y + (double) vector3_6.Z, 1.0 / 3.0);
      for (int index1 = 0; index1 < 25; ++index1)
      {
        for (int index2 = 0; index2 < 25 - index1; ++index2)
        {
          if ((double) MyCubeGrid.m_neighborDistances[(int) MyCubeGrid.m_neighborOffsetIndices[index2]] > (double) MyCubeGrid.m_neighborDistances[(int) MyCubeGrid.m_neighborOffsetIndices[index2 + 1]])
          {
            MyCubeGrid.NeighborOffsetIndex neighborOffsetIndex = MyCubeGrid.m_neighborOffsetIndices[index2];
            MyCubeGrid.m_neighborOffsetIndices[index2] = MyCubeGrid.m_neighborOffsetIndices[index2 + 1];
            MyCubeGrid.m_neighborOffsetIndices[index2 + 1] = neighborOffsetIndex;
          }
        }
      }
      Vector3I vector3I = new Vector3I();
      for (int index = 0; index < MyCubeGrid.m_neighborOffsets.Count; ++index)
      {
        Vector3I neighborOffset = MyCubeGrid.m_neighborOffsets[(int) MyCubeGrid.m_neighborOffsetIndices[index]];
        if (this.m_cubes.ContainsKey(cube + neighborOffset))
        {
          cube += neighborOffset;
          break;
        }
      }
    }

    public HashSet<MySlimBlock> GetBlocks() => this.m_cubeBlocks;

    public ListReader<MyCubeBlock> GetFatBlocks() => this.m_fatBlocks.ListUnsafe;

    public MyFatBlockReader<T> GetFatBlocks<T>() where T : MyCubeBlock => new MyFatBlockReader<T>(this);

    public bool HasStandAloneBlocks() => this.m_standAloneBlockCount > 0;

    public static bool HasStandAloneBlocks(List<MySlimBlock> blocks, int offset, int count)
    {
      if (offset < 0)
      {
        MySandboxGame.Log.WriteLine(string.Format("Negative offset in HasStandAloneBlocks - {0}", (object) offset));
        return false;
      }
      for (int index = offset; index < offset + count && index < blocks.Count; ++index)
      {
        MySlimBlock block = blocks[index];
        if (block != null && block.BlockDefinition.IsStandAlone)
          return true;
      }
      return false;
    }

    private void CheckShouldCloseGrid()
    {
      if (this.HasStandAloneBlocks() || this.IsPreview || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.SetFadeOut(false);
      this.Close();
    }

    public bool CanHavePhysics()
    {
      if (this.m_canHavePhysics)
      {
        if (MyPerGameSettings.Game == GameEnum.SE_GAME)
        {
          foreach (MySlimBlock cubeBlock in this.m_cubeBlocks)
          {
            if (cubeBlock.BlockDefinition.HasPhysics)
              return true;
          }
          this.m_canHavePhysics = false;
        }
        else
          this.m_canHavePhysics = this.m_cubeBlocks.Count > 0;
      }
      return this.m_canHavePhysics;
    }

    public static bool CanHavePhysics(List<MySlimBlock> blocks, int offset, int count)
    {
      if (offset < 0)
      {
        MySandboxGame.Log.WriteLine(string.Format("Negative offset in CanHavePhysics - {0}", (object) offset));
        return false;
      }
      for (int index = offset; index < offset + count && index < blocks.Count; ++index)
      {
        MySlimBlock block = blocks[index];
        if (block != null && block.BlockDefinition.HasPhysics)
          return true;
      }
      return false;
    }

    private void RebuildGrid(bool staticPhysics = false)
    {
      if (!this.HasStandAloneBlocks() || !this.CanHavePhysics())
        return;
      this.RecalcBounds();
      this.RemoveRedundantParts();
      if (this.Physics != null)
      {
        this.Physics.Close();
        this.Physics = (MyGridPhysics) null;
      }
      if (!this.CreatePhysics)
        return;
      this.Physics = new MyGridPhysics(this, staticPhysics: staticPhysics);
      this.RaisePhysicsChanged();
      if (Sandbox.Game.Multiplayer.Sync.IsServer || this.IsClientPredicted)
        return;
      this.Physics.RigidBody.UpdateMotionType(HkMotionType.Fixed);
    }

    public new void RaisePhysicsChanged()
    {
      if (MyParallelEntityUpdateOrchestrator.ParallelUpdateInProgress)
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(((MyEntity) this).RaisePhysicsChanged), 0);
      else
        base.RaisePhysicsChanged();
    }

    [Event(null, 5489)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    public void OnConvertToDynamic()
    {
      if (MyCubeGridSmallToLargeConnection.Static != null && this.m_enableSmallToLargeConnections)
        MyCubeGridSmallToLargeConnection.Static.ConvertToDynamic(this);
      this.IsStatic = false;
      this.IsUnsupportedStation = false;
      if (MyCubeGridGroups.Static != null)
        MyCubeGridGroups.Static.UpdateDynamicState(this);
      if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID && !this.ForceDisablePrediction)
        this.CheckPredictionFlagScheduling();
      this.SetInventoryMassDirty();
      this.Physics.ConvertToDynamic(this.GridSizeEnum == MyCubeSize.Large, this.IsClientPredicted);
      this.RaisePhysicsChanged();
      this.Physics.RigidBody.AddGravity();
      this.RecalculateGravity();
      MyFixedGrids.UnmarkGridRoot(this);
    }

    [Event(null, 5524)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    public void ConvertToStatic()
    {
      if (this.IsStatic || this.Physics == null || (double) this.Physics.AngularVelocity.LengthSquared() > 0.0001 || (double) this.Physics.LinearVelocity.LengthSquared() > 0.0001)
        return;
      if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID && !this.ForceDisablePrediction)
        this.CheckPredictionFlagScheduling();
      this.IsStatic = true;
      this.IsUnsupportedStation = true;
      this.Physics.ConvertToStatic();
      this.RaisePhysicsChanged();
      MyFixedGrids.MarkGridRoot(this);
    }

    private void CheckConvertToDynamic()
    {
      if (this.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyTestDynamicReason>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyTestDynamicReason>>) (x => new Action<MyCubeGrid.MyTestDynamicReason>(x.OnConvertedToShipRequest)), this.TestDynamic);
      this.TestDynamic = MyCubeGrid.MyTestDynamicReason.NoReason;
    }

    public void DoDamage(float damage, MyHitInfo hitInfo, Vector3? localPos = null, long attackerId = 0)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.Damage))
        return;
      Vector3I cube;
      if (localPos.HasValue)
        this.FixTargetCube(out cube, localPos.Value * this.GridSizeR);
      else
        this.FixTargetCube(out cube, (Vector3) (Vector3D.Transform(hitInfo.Position, this.PositionComp.WorldMatrixInvScaled) * (double) this.GridSizeR));
      MySlimBlock block1 = this.GetCubeBlock(cube);
      if (block1 == null)
        return;
      if (MyFakes.ENABLE_FRACTURE_COMPONENT)
      {
        ushort? nullable = new ushort?();
        if (block1.FatBlock is MyCompoundCubeBlock fatBlock)
        {
          ushort? contactCompoundId = this.Physics.GetContactCompoundId(block1.Position, hitInfo.Position);
          if (!contactCompoundId.HasValue)
            return;
          MySlimBlock block2 = fatBlock.GetBlock(contactCompoundId.Value);
          if (block2 == null)
            return;
          block1 = block2;
        }
      }
      this.ApplyDestructionDeformation(block1, damage, new MyHitInfo?(hitInfo), attackerId);
    }

    public void ApplyDestructionDeformation(
      MySlimBlock block,
      float damage = 1f,
      MyHitInfo? hitInfo = null,
      long attackerId = 0)
    {
      if (MyPerGameSettings.Destruction)
      {
        block.DoDamage(damage, MyDamageType.Deformation, true, hitInfo, attackerId, 0L);
      }
      else
      {
        this.EnqueueDestructionDeformationBlock(block.Position);
        double num = (double) this.ApplyDestructionDeformationInternal(block, true, damage, attackerId);
      }
    }

    private void ApplyDeformationPostponed()
    {
      if (this.m_deformationPostponed.Count <= 0)
        return;
      List<MyCubeGrid.DeformationPostponedItem> cloned = this.m_deformationPostponed;
      Parallel.Start((Action) (() =>
      {
        foreach (MyCubeGrid.DeformationPostponedItem deformationPostponedItem in cloned)
          this.ApplyDestructionDeformationInternal(deformationPostponedItem);
        cloned.Clear();
        MyCubeGrid.m_postponedListsPool.Return(cloned);
      }));
      this.m_deformationPostponed = MyCubeGrid.m_postponedListsPool.Get();
      this.m_deformationPostponed.Clear();
    }

    private void ApplyDestructionDeformationInternal(MyCubeGrid.DeformationPostponedItem item)
    {
      if (!MySession.Static.HighSimulationQuality || this.Closed)
        return;
      if (MyCubeGrid.m_deformationRng == null)
        MyCubeGrid.m_deformationRng = new MyRandom();
      Vector3I maxValue1 = Vector3I.MaxValue;
      Vector3I max = Vector3I.MinValue;
      bool flag = false;
      for (int index1 = -1; index1 <= 1; index1 += 2)
      {
        for (int index2 = -1; index2 <= 1; index2 += 2)
          flag = flag | this.MoveCornerBones(item.Min, new Vector3I(index1, 0, index2), ref maxValue1, ref max) | this.MoveCornerBones(item.Min, new Vector3I(index1, index2, 0), ref maxValue1, ref max) | this.MoveCornerBones(item.Min, new Vector3I(0, index1, index2), ref maxValue1, ref max);
      }
      if (flag)
      {
        this.m_dirtyRegion.AddCubeRegion(maxValue1, max);
        this.ScheduleDirtyRegion();
      }
      MyCubeGrid.m_deformationRng.SetSeed(item.Position.GetHashCode());
      float angleDeviation = 0.3926991f;
      float gridSizeQuarter = this.GridSizeQuarter;
      Vector3I min = item.Min;
      for (int index = 0; index < 3; ++index)
      {
        Vector3I maxValue2 = Vector3I.MaxValue;
        Vector3I minValue = Vector3I.MinValue;
        if (false | this.ApplyTable(min, MyCubeGridDeformationTables.ThinUpper[index], ref maxValue2, ref minValue, MyCubeGrid.m_deformationRng, gridSizeQuarter, angleDeviation) | this.ApplyTable(min, MyCubeGridDeformationTables.ThinLower[index], ref maxValue2, ref minValue, MyCubeGrid.m_deformationRng, gridSizeQuarter, angleDeviation))
        {
          maxValue2 -= Vector3I.One;
          minValue += Vector3I.One;
          Vector3I cube = min;
          max = min;
          this.Skeleton.Wrap(ref cube, ref maxValue2);
          this.Skeleton.Wrap(ref max, ref minValue);
          this.m_dirtyRegion.AddCubeRegion(cube, max);
          this.ScheduleDirtyRegion();
        }
      }
      MySandboxGame.Static.Invoke((Action) (() => this.MarkForDraw()), "ApplyDestructionDeformationInternal::MarkForDraw");
    }

    private float ApplyDestructionDeformationInternal(
      MySlimBlock block,
      bool sync,
      float damage = 1f,
      long attackerId = 0,
      bool postponed = false)
    {
      if (!this.BlocksDestructionEnabled)
        return 0.0f;
      if (block.UseDamageSystem)
      {
        MyDamageInformation info = new MyDamageInformation(true, 1f, MyDamageType.Deformation, attackerId);
        MyDamageSystem.Static.RaiseBeforeDamageApplied((object) block, ref info);
        if ((double) info.Amount == 0.0)
          return 0.0f;
      }
      MyCubeGrid.DeformationPostponedItem deformationPostponedItem = new MyCubeGrid.DeformationPostponedItem()
      {
        Position = block.Position,
        Min = block.Min,
        Max = block.Max
      };
      this.m_totalBoneDisplacement = 0.0f;
      if (postponed)
      {
        this.m_deformationPostponed.Add(deformationPostponedItem);
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ApplyDeformationPostponed), 1, true);
      }
      else
        this.ApplyDestructionDeformationInternal(deformationPostponedItem);
      if (sync)
      {
        MyDamageInformation info = new MyDamageInformation(false, (float) ((double) this.m_totalBoneDisplacement * (double) this.GridSize * 10.0) * damage, MyDamageType.Deformation, attackerId);
        if (block.UseDamageSystem)
          MyDamageSystem.Static.RaiseBeforeDamageApplied((object) block, ref info);
        if ((double) info.Amount > 0.0)
          block.DoDamage(info.Amount, MyDamageType.Deformation, true, attackerId: attackerId);
      }
      return this.m_totalBoneDisplacement;
    }

    public void RemoveDestroyedBlock(MySlimBlock block, long attackerId = 0)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (MyFakes.ENABLE_FRACTURE_COMPONENT)
          return;
        block.OnDestroyVisual();
      }
      else
      {
        if (this.Physics == null)
          return;
        if (MyFakes.ENABLE_FRACTURE_COMPONENT)
        {
          MySlimBlock cubeBlock = this.GetCubeBlock(block.Position);
          if (cubeBlock == null)
            return;
          if (cubeBlock == block)
          {
            this.EnqueueDestroyedBlockWithId(block.Position, new ushort?());
            this.RemoveDestroyedBlockInternal(block);
            this.Physics.AddDirtyBlock(block);
          }
          else if (cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock)
          {
            ushort? blockId = fatBlock.GetBlockId(block);
            if (blockId.HasValue)
            {
              this.EnqueueDestroyedBlockWithId(block.Position, blockId);
              this.RemoveDestroyedBlockInternal(block);
              this.Physics.AddDirtyBlock(block);
            }
          }
          MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
          if (fractureComponent == null)
            return;
          MyDestructionHelper.CreateFracturePiece(fractureComponent, true);
        }
        else
        {
          this.EnqueueDestroyedBlock(block.Position);
          this.RemoveDestroyedBlockInternal(block);
          this.Physics.AddDirtyBlock(block);
        }
      }
    }

    private void RemoveDestroyedBlockInternal(MySlimBlock block)
    {
      double num = (double) this.ApplyDestructionDeformationInternal(block, false, postponed: true);
      ((IMyDestroyableObject) block).OnDestroy();
      MySlimBlock cubeBlock = this.GetCubeBlock(block.Position);
      if (cubeBlock == block)
      {
        this.RemoveBlockInternal(block, true);
      }
      else
      {
        if (cubeBlock == null || !(cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock))
          return;
        ushort? blockId = fatBlock.GetBlockId(block);
        if (!blockId.HasValue)
          return;
        Vector3I maxValue = Vector3I.MaxValue;
        Vector3I minValue = Vector3I.MinValue;
        this.RemoveBlockInCompound(block.Position, blockId.Value, ref maxValue, ref minValue);
      }
    }

    private bool ApplyTable(
      Vector3I cubePos,
      MyCubeGridDeformationTables.DeformationTable table,
      ref Vector3I dirtyMin,
      ref Vector3I dirtyMax,
      MyRandom random,
      float maxLinearDeviation,
      float angleDeviation)
    {
      if (this.m_cubes.ContainsKey(cubePos + table.Normal))
        return false;
      float maxValue = this.GridSize / 10f;
      using (MyUtils.ReuseCollection<Vector3I, MySlimBlock>(ref MyCubeGrid.m_tmpCubeSet))
      {
        this.GetExistingCubes(cubePos, (IEnumerable<Vector3I>) table.CubeOffsets, MyCubeGrid.m_tmpCubeSet);
        int num = 0;
        if (MyCubeGrid.m_tmpCubeSet.Count > 0)
        {
          foreach (KeyValuePair<Vector3I, Matrix> keyValuePair in table.OffsetTable)
          {
            Vector3I key1 = keyValuePair.Key >> 1;
            Vector3I key2 = keyValuePair.Key - Vector3I.One >> 1;
            if (MyCubeGrid.m_tmpCubeSet.ContainsKey(key1) || key1 != key2 && MyCubeGrid.m_tmpCubeSet.ContainsKey(key2))
            {
              Vector3I key3 = keyValuePair.Key;
              Vector3 clamp = new Vector3(this.GridSizeQuarter - random.NextFloat(0.0f, maxValue));
              Matrix matrix = keyValuePair.Value;
              Vector3 moveDirection = random.NextDeviatingVector(ref matrix, angleDeviation) * random.NextFloat(1f, maxLinearDeviation);
              float displacementLength = moveDirection.Max();
              this.MoveBone(ref cubePos, ref key3, ref moveDirection, ref displacementLength, ref clamp);
              ++num;
            }
          }
        }
        MyCubeGrid.m_tmpCubeSet.Clear();
      }
      dirtyMin = Vector3I.Min(dirtyMin, table.MinOffset);
      dirtyMax = Vector3I.Max(dirtyMax, table.MaxOffset);
      return true;
    }

    private void BlocksRemoved(List<Vector3I> blocksToRemove)
    {
      foreach (Vector3I pos in blocksToRemove)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(pos);
        if (cubeBlock != null)
        {
          this.RemoveBlockInternal(cubeBlock, true);
          this.Physics.AddDirtyBlock(cubeBlock);
        }
      }
    }

    private void BlocksWithIdRemoved(List<MyCubeGrid.BlockPositionId> blocksToRemove)
    {
      foreach (MyCubeGrid.BlockPositionId blockPositionId in blocksToRemove)
      {
        if (blockPositionId.CompoundId > (uint) ushort.MaxValue)
        {
          MySlimBlock cubeBlock = this.GetCubeBlock(blockPositionId.Position);
          if (cubeBlock != null)
          {
            this.RemoveBlockInternal(cubeBlock, true);
            this.Physics.AddDirtyBlock(cubeBlock);
          }
        }
        else
        {
          Vector3I maxValue = Vector3I.MaxValue;
          Vector3I minValue = Vector3I.MinValue;
          this.RemoveBlockInCompound(blockPositionId.Position, (ushort) blockPositionId.CompoundId, ref maxValue, ref minValue);
          if (maxValue != Vector3I.MaxValue)
            this.Physics.AddDirtyArea(maxValue, minValue);
        }
      }
    }

    private void BlocksDestroyed(List<Vector3I> blockToDestroy)
    {
      this.m_largeDestroyInProgress = blockToDestroy.Count > MyCubeGrid.BLOCK_LIMIT_FOR_LARGE_DESTRUCTION;
      foreach (Vector3I pos in blockToDestroy)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(pos);
        if (cubeBlock != null)
        {
          this.RemoveDestroyedBlockInternal(cubeBlock);
          this.Physics.AddDirtyBlock(cubeBlock);
        }
      }
      this.m_largeDestroyInProgress = false;
    }

    private void BlocksDeformed(List<Vector3I> blockToDestroy)
    {
      foreach (Vector3I pos in blockToDestroy)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(pos);
        if (cubeBlock != null)
        {
          double num = (double) this.ApplyDestructionDeformationInternal(cubeBlock, false);
          this.Physics.AddDirtyBlock(cubeBlock);
        }
      }
    }

    [Event(null, 5962)]
    [Reliable]
    [Broadcast]
    private void BlockIntegrityChanged(
      Vector3I pos,
      ushort subBlockId,
      float buildIntegrity,
      float integrity,
      MyIntegrityChangeEnum integrityChangeType,
      long grinderOwner)
    {
      MyCompoundCubeBlock compoundCubeBlock = (MyCompoundCubeBlock) null;
      MySlimBlock mySlimBlock = this.GetCubeBlock(pos);
      if (mySlimBlock != null)
        compoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
      if (compoundCubeBlock != null)
        mySlimBlock = compoundCubeBlock.GetBlock(subBlockId);
      mySlimBlock?.SetIntegrity(buildIntegrity, integrity, integrityChangeType, grinderOwner);
    }

    [Event(null, 5979)]
    [Reliable]
    [Broadcast]
    private void BlockStockpileChanged(
      Vector3I pos,
      ushort subBlockId,
      List<MyStockpileItem> items)
    {
      MySlimBlock mySlimBlock = this.GetCubeBlock(pos);
      MyCompoundCubeBlock compoundCubeBlock = (MyCompoundCubeBlock) null;
      if (mySlimBlock != null)
        compoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
      if (compoundCubeBlock != null)
        mySlimBlock = compoundCubeBlock.GetBlock(subBlockId);
      mySlimBlock?.ChangeStockpile(items);
    }

    [Event(null, 5998)]
    [Reliable]
    [Broadcast]
    private void FractureComponentRepaired(Vector3I pos, ushort subBlockId, long toolOwner)
    {
      MyCompoundCubeBlock compoundCubeBlock = (MyCompoundCubeBlock) null;
      MySlimBlock mySlimBlock = this.GetCubeBlock(pos);
      if (mySlimBlock != null)
        compoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
      if (compoundCubeBlock != null)
        mySlimBlock = compoundCubeBlock.GetBlock(subBlockId);
      if (mySlimBlock == null || mySlimBlock.FatBlock == null)
        return;
      mySlimBlock.RepairFracturedBlock(toolOwner);
    }

    private void RemoveBlockByCubeBuilder(MySlimBlock block)
    {
      this.RemoveBlockInternal(block, true);
      if (block.FatBlock == null)
        return;
      block.FatBlock.OnRemovedByCubeBuilder();
    }

    private void RemoveBlockInternal(MySlimBlock block, bool close, bool markDirtyDisconnects = true)
    {
      if (!this.m_cubeBlocks.Contains(block))
        return;
      if (block.BlockDefinition.IsStandAlone && --this.m_standAloneBlockCount == 0)
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.CheckShouldCloseGrid));
      if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
        this.RemoveMultiBlockInfo(block);
      this.RenderData.RemoveDecals(block.Position);
      if (block.FatBlock is MyTerminalBlock fatBlock)
      {
        for (int index = 0; index < this.BlockGroups.Count; ++index)
        {
          MyBlockGroup blockGroup = this.BlockGroups[index];
          if (blockGroup.Blocks.Contains(fatBlock))
            blockGroup.Blocks.Remove(fatBlock);
          if (blockGroup.Blocks.Count <= 0)
          {
            this.RemoveGroup(blockGroup);
            --index;
          }
        }
      }
      this.RemoveBlockParts(block);
      Parallel.Start((Action) (() => this.RemoveBlockEdges(block)));
      if (block.FatBlock != null)
      {
        if (block.FatBlock.InventoryCount > 0)
          this.UnregisterInventory(block.FatBlock);
        if (this.BlocksCounters.ContainsKey(block.BlockDefinition.Id.TypeId))
          this.BlocksCounters[block.BlockDefinition.Id.TypeId]--;
        block.FatBlock.IsBeingRemoved = true;
        this.GridSystems.UnregisterFromSystems(block.FatBlock);
        if (close)
          block.FatBlock.Close();
        else
          this.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) block.FatBlock);
        if (block.FatBlock.Render.NeedsDrawFromParent)
        {
          this.m_blocksForDraw.Remove(block.FatBlock);
          block.FatBlock.Render.SetVisibilityUpdates(false);
        }
      }
      block.RemoveNeighbours();
      block.RemoveAuthorship();
      this.m_PCU -= block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      this.m_cubeBlocks.Remove(block);
      if (block.FatBlock != null)
      {
        if (block.FatBlock is MyReactor)
          --this.NumberOfReactors;
        this.m_fatBlocks.Remove(block.FatBlock);
        block.FatBlock.IsBeingRemoved = false;
      }
      if (this.m_colorStatistics.ContainsKey(block.ColorMaskHSV))
      {
        this.m_colorStatistics[block.ColorMaskHSV]--;
        if (this.m_colorStatistics[block.ColorMaskHSV] <= 0)
          this.m_colorStatistics.Remove(block.ColorMaskHSV);
      }
      if (markDirtyDisconnects && this.m_disconnectsDirty == MyCubeGrid.MyTestDisconnectsReason.NoReason)
      {
        this.m_disconnectsDirty = MyCubeGrid.MyTestDisconnectsReason.BlockRemoved;
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.DetectDisconnects), 18, true);
      }
      Vector3I next = block.Min;
      bool flag = !this.Skeleton.HasUnusedBones;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref block.Min, ref block.Max);
      while (vector3IRangeIterator.IsValid())
      {
        this.Skeleton.MarkCubeRemoved(ref next);
        vector3IRangeIterator.GetNext(out next);
      }
      if (flag)
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.RemoveUnsusedBones), 19, true);
      if (block.FatBlock != null && block.FatBlock.IDModule != null)
        this.ChangeOwner(block.FatBlock, block.FatBlock.IDModule.Owner, 0L);
      if (MyCubeGridSmallToLargeConnection.Static != null && this.m_enableSmallToLargeConnections)
        MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(block);
      this.NotifyBlockRemoved(block);
      if (close)
        this.NotifyBlockClosed(block);
      this.m_boundsDirty = true;
      this.MarkForDraw();
    }

    public void RemoveBlock(MySlimBlock block, bool updatePhysics = false)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.m_cubeBlocks.Contains(block))
        return;
      this.EnqueueRemovedBlock(block.Min);
      this.RemoveBlockInternal(block, true);
      if (!updatePhysics)
        return;
      this.Physics.AddDirtyBlock(block);
    }

    public void RemoveBlockWithId(MySlimBlock block, bool updatePhysics = false)
    {
      MySlimBlock cubeBlock = this.GetCubeBlock(block.Min);
      if (cubeBlock == null)
        return;
      MyCompoundCubeBlock fatBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
      ushort? compoundId = new ushort?();
      if (fatBlock != null)
      {
        compoundId = fatBlock.GetBlockId(block);
        if (!compoundId.HasValue)
          return;
      }
      this.RemoveBlockWithId(block.Min, compoundId, updatePhysics);
    }

    public void RemoveBlockWithId(Vector3I position, ushort? compoundId, bool updatePhysics = false)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MySlimBlock cubeBlock = this.GetCubeBlock(position);
      if (cubeBlock == null)
        return;
      this.EnqueueRemovedBlockWithId(cubeBlock.Min, compoundId);
      if (compoundId.HasValue)
      {
        Vector3I zero1 = Vector3I.Zero;
        Vector3I zero2 = Vector3I.Zero;
        this.RemoveBlockInCompound(cubeBlock.Min, compoundId.Value, ref zero1, ref zero2);
      }
      else
        this.RemoveBlockInternal(cubeBlock, true);
      if (!updatePhysics)
        return;
      this.Physics.AddDirtyBlock(cubeBlock);
    }

    public void UpdateBlockNeighbours(MySlimBlock block)
    {
      if (!this.m_cubeBlocks.Contains(block))
        return;
      block.RemoveNeighbours();
      block.AddNeighbours();
      if (this.m_disconnectsDirty != MyCubeGrid.MyTestDisconnectsReason.NoReason)
        return;
      this.m_disconnectsDirty = MyCubeGrid.MyTestDisconnectsReason.SplitBlock;
      this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.DetectDisconnects), 18, true);
    }

    public Vector3 GetClosestCorner(Vector3I gridPos, Vector3 position) => gridPos * this.GridSize - Vector3.SignNonZero(gridPos * this.GridSize - position) * this.GridSizeHalf;

    public void DetectDisconnectsAfterFrame()
    {
      if (this.m_disconnectsDirty != MyCubeGrid.MyTestDisconnectsReason.NoReason)
        return;
      this.m_disconnectsDirty = MyCubeGrid.MyTestDisconnectsReason.BlockRemoved;
      this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.DetectDisconnects), 18, true);
    }

    private void DetectDisconnects()
    {
      if (!MyFakes.DETECT_DISCONNECTS || this.m_cubes.Count == 0 || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_disconnectHelper.Disconnect(this, this.m_disconnectsDirty);
      this.m_disconnectsDirty = MyCubeGrid.MyTestDisconnectsReason.NoReason;
    }

    public bool CubeExists(Vector3I pos) => this.m_cubes.ContainsKey(pos);

    public void UpdateDirty(Action callback = null, bool immediate = false)
    {
      if (this.m_updatingDirty || this.m_resolvingSplits != 0)
        return;
      this.m_updatingDirty = true;
      MyDirtyRegion dirtyRegion = this.m_dirtyRegion;
      this.m_dirtyRegion = this.m_dirtyRegionParallel;
      this.m_dirtyRegionParallel = dirtyRegion;
      if (immediate)
      {
        this.UpdateDirtyInternal();
        if (callback != null)
          callback();
        this.OnUpdateDirtyCompleted();
      }
      else
        Parallel.Start(this.m_UpdateDirtyInternal, callback += this.m_OnUpdateDirtyCompleted);
    }

    private void ClearDirty()
    {
      if (this.m_updatingDirty || this.m_resolvingSplits != 0)
        return;
      MyDirtyRegion dirtyRegion = this.m_dirtyRegion;
      this.m_dirtyRegion = this.m_dirtyRegionParallel;
      this.m_dirtyRegionParallel = dirtyRegion;
      this.m_dirtyRegionParallel.Cubes.Clear();
      do
        ;
      while (this.m_dirtyRegionParallel.PartsToRemove.TryDequeue(out MyCube _));
    }

    private void OnUpdateDirtyCompleted()
    {
      if (this.InScene)
        this.UpdateInstanceData();
      this.m_dirtyRegionParallel.Clear();
      this.m_updatingDirty = false;
      this.m_dirtyRegionScheduled = false;
      if (!this.m_dirtyRegion.IsDirty)
        this.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateDirtyRegion));
      this.MarkForDraw();
      this.ReleaseMerginGrids();
    }

    public void UpdateDirtyInternal()
    {
      using (this.Pin())
      {
        if (this.MarkedForClose)
          return;
        this.m_dirtyRegionParallel.Cubes.ApplyChanges();
        foreach (Vector3I cube in this.m_dirtyRegionParallel.Cubes)
          this.UpdateParts(cube);
        MyCube result;
        while (this.m_dirtyRegionParallel.PartsToRemove.TryDequeue(out result))
        {
          this.UpdateParts(result.CubeBlock.Position);
          foreach (MyCubePart part in result.Parts)
            this.Render.RenderData.RemoveCubePart(part);
        }
        foreach (Vector3I cube in this.m_dirtyRegionParallel.Cubes)
        {
          MySlimBlock cubeBlock = this.GetCubeBlock(cube);
          if (cubeBlock != null && cubeBlock.ShowParts && MyFakes.ENABLE_EDGES)
          {
            if ((double) cubeBlock.Dithering >= 0.0)
              this.AddBlockEdges(cubeBlock);
            else
              this.RemoveBlockEdges(cubeBlock);
            cubeBlock.UpdateMaxDeformation();
          }
          if (cubeBlock != null && cubeBlock.FatBlock != null && (cubeBlock.FatBlock.Render != null && cubeBlock.FatBlock.Render.NeedsDrawFromParent))
          {
            this.m_blocksForDraw.Add(cubeBlock.FatBlock);
            cubeBlock.FatBlock.Render.SetVisibilityUpdates(true);
          }
        }
      }
    }

    public bool IsDirty() => this.m_dirtyRegion.IsDirty;

    private void CheckEarlyDeactivation()
    {
      if (this.Physics == null)
        return;
      bool flag = false;
      if (this.IsMarkedForEarlyDeactivation)
      {
        if (!this.Physics.IsStatic)
        {
          flag = true;
          this.Physics.ConvertToStatic();
        }
      }
      else if (!this.IsStatic && this.Physics.IsStatic)
      {
        flag = true;
        this.Physics.ConvertToDynamic(this.GridSizeEnum == MyCubeSize.Large, false);
      }
      if (!flag)
        return;
      this.RaisePhysicsChanged();
    }

    public void UpdateInstanceData() => this.Render.RebuildDirtyCells();

    public bool TryGetCube(Vector3I position, out MyCube cube) => this.m_cubes.TryGetValue(position, out cube);

    private bool AddCube(
      MySlimBlock block,
      ref Vector3I pos,
      Matrix rotation,
      MyCubeBlockDefinition cubeBlockDefinition)
    {
      MyCube myCube = new MyCube()
      {
        Parts = MyCubeGrid.GetCubeParts(block.SkinSubtypeId, cubeBlockDefinition, pos, (MatrixD) ref rotation, this.GridSize, this.GridScale),
        CubeBlock = block
      };
      MyCube orAdd = this.m_cubes.GetOrAdd(pos, myCube);
      if (myCube != orAdd)
        return false;
      this.m_dirtyRegion.AddCube(pos);
      this.ScheduleDirtyRegion();
      this.MarkForDraw();
      return true;
    }

    private MyCube CreateCube(
      MySlimBlock block,
      Vector3I pos,
      Matrix rotation,
      MyCubeBlockDefinition cubeBlockDefinition)
    {
      return new MyCube()
      {
        Parts = MyCubeGrid.GetCubeParts(block.SkinSubtypeId, cubeBlockDefinition, pos, (MatrixD) ref rotation, this.GridSize, this.GridScale),
        CubeBlock = block
      };
    }

    public bool ChangeColorAndSkin(MySlimBlock block, Vector3? newHSV = null, MyStringHash? skinSubtypeId = null)
    {
      try
      {
        MyStringHash skinSubtypeId1 = block.SkinSubtypeId;
        MyStringHash? nullable1 = skinSubtypeId;
        if ((nullable1.HasValue ? (skinSubtypeId1 == nullable1.GetValueOrDefault() ? 1 : 0) : 0) != 0 || !skinSubtypeId.HasValue)
        {
          Vector3 colorMaskHsv = block.ColorMaskHSV;
          Vector3? nullable2 = newHSV;
          if ((nullable2.HasValue ? (colorMaskHsv == nullable2.GetValueOrDefault() ? 1 : 0) : 0) != 0 || !newHSV.HasValue)
            return false;
        }
        if (newHSV.HasValue)
        {
          int num;
          if (this.m_colorStatistics.TryGetValue(block.ColorMaskHSV, out num))
          {
            this.m_colorStatistics[block.ColorMaskHSV] = num - 1;
            if (this.m_colorStatistics[block.ColorMaskHSV] <= 0)
              this.m_colorStatistics.Remove(block.ColorMaskHSV);
          }
          block.ColorMaskHSV = newHSV.Value;
        }
        if (skinSubtypeId.HasValue)
          block.SkinSubtypeId = skinSubtypeId.Value;
        block.UpdateVisual(false);
        if (newHSV.HasValue)
        {
          if (!this.m_colorStatistics.ContainsKey(block.ColorMaskHSV))
            this.m_colorStatistics.Add(block.ColorMaskHSV, 0);
          this.m_colorStatistics[block.ColorMaskHSV]++;
        }
        return true;
      }
      finally
      {
      }
    }

    private void UpdatePartInstanceData(MyCubePart part, Vector3I cubePos)
    {
      MyCube myCube;
      if (!this.m_cubes.TryGetValue(cubePos, out myCube))
        return;
      MySlimBlock cubeBlock = myCube.CubeBlock;
      if (cubeBlock != null)
      {
        part.InstanceData.SetColorMaskHSV(new Vector4(cubeBlock.ColorMaskHSV, cubeBlock.Dithering));
        part.SkinSubtypeId = myCube.CubeBlock.SkinSubtypeId;
      }
      if (part.Model.BoneMapping == null)
        return;
      Matrix orientation = part.InstanceData.LocalMatrix.GetOrientation();
      bool flag = false;
      part.InstanceData.BoneRange = this.GridSize;
      for (int index = 0; index < Math.Min(part.Model.BoneMapping.Length, 9); ++index)
      {
        Vector3I bonePos = Vector3I.Round(Vector3.Transform((part.Model.BoneMapping[index] * 1f - Vector3.One) * 1f, orientation) + Vector3.One);
        Vector3UByte vec = Vector3UByte.Normalize(this.Skeleton.GetBone(cubePos, bonePos), this.GridSize);
        if (!Vector3UByte.IsMiddle(vec))
          flag = true;
        part.InstanceData[index] = vec;
      }
      part.InstanceData.EnableSkinning = flag;
    }

    private void UpdateParts(Vector3I pos)
    {
      MyCube myCube1;
      bool flag1 = this.m_cubes.TryGetValue(pos, out myCube1);
      if (flag1 && !myCube1.CubeBlock.ShowParts)
        this.RemoveBlockEdges(myCube1.CubeBlock);
      if (flag1 && myCube1.CubeBlock.ShowParts)
      {
        MyTileDefinition[] cubeTiles1 = MyCubeGridDefinitions.GetCubeTiles(myCube1.CubeBlock.BlockDefinition);
        Matrix result1;
        myCube1.CubeBlock.Orientation.GetMatrix(out result1);
        if (this.Skeleton.IsDeformed(pos, 0.004f * this.GridSize, this, false))
          this.RemoveBlockEdges(myCube1.CubeBlock);
        for (int index1 = 0; index1 < myCube1.Parts.Length; ++index1)
        {
          this.UpdatePartInstanceData(myCube1.Parts[index1], pos);
          this.Render.RenderData.AddCubePart(myCube1.Parts[index1]);
          MyTileDefinition myTileDefinition1 = cubeTiles1[index1];
          if (!myTileDefinition1.IsEmpty)
          {
            Vector3 vec = Vector3.TransformNormal(myTileDefinition1.Normal, result1);
            Vector3 vector3_1 = Vector3.TransformNormal(myTileDefinition1.Up, result1);
            MyCube myCube2;
            if (Base6Directions.IsBaseDirection(ref vec) && this.m_cubes.TryGetValue(pos + Vector3I.Round(vec), out myCube2) && myCube2.CubeBlock.ShowParts)
            {
              Matrix result2;
              myCube2.CubeBlock.Orientation.GetMatrix(out result2);
              MyTileDefinition[] cubeTiles2 = MyCubeGridDefinitions.GetCubeTiles(myCube2.CubeBlock.BlockDefinition);
              for (int index2 = 0; index2 < myCube2.Parts.Length; ++index2)
              {
                MyTileDefinition myTileDefinition2 = cubeTiles2[index2];
                if (!myTileDefinition2.IsEmpty)
                {
                  Vector3 vector3_2 = Vector3.TransformNormal(myTileDefinition2.Normal, result2);
                  Vector3 vector3_3 = vec + vector3_2;
                  if ((double) vector3_3.LengthSquared() < 1.0 / 1000.0)
                  {
                    if ((double) myCube2.CubeBlock.Dithering != (double) myCube1.CubeBlock.Dithering)
                    {
                      this.Render.RenderData.AddCubePart(myCube2.Parts[index2]);
                    }
                    else
                    {
                      bool flag2 = false;
                      if (myTileDefinition2.FullQuad && !myTileDefinition1.IsRounded)
                      {
                        this.Render.RenderData.RemoveCubePart(myCube1.Parts[index1]);
                        flag2 = true;
                      }
                      if (myTileDefinition1.FullQuad && !myTileDefinition2.IsRounded)
                      {
                        this.Render.RenderData.RemoveCubePart(myCube2.Parts[index2]);
                        flag2 = true;
                      }
                      if (!flag2)
                      {
                        vector3_3 = myTileDefinition2.Up * myTileDefinition1.Up;
                        if ((double) vector3_3.LengthSquared() > 1.0 / 1000.0)
                        {
                          vector3_3 = Vector3.TransformNormal(myTileDefinition2.Up, result2) - vector3_1;
                          if ((double) vector3_3.LengthSquared() < 1.0 / 1000.0)
                          {
                            if (!myTileDefinition1.IsRounded && myTileDefinition2.IsRounded)
                              this.Render.RenderData.RemoveCubePart(myCube1.Parts[index1]);
                            if (myTileDefinition1.IsRounded && !myTileDefinition2.IsRounded)
                              this.Render.RenderData.RemoveCubePart(myCube2.Parts[index2]);
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
      else
      {
        if (flag1)
        {
          foreach (MyCubePart part in myCube1.Parts)
            this.Render.RenderData.RemoveCubePart(part);
        }
        foreach (Vector3 direction in Base6Directions.Directions)
        {
          MyCube myCube2;
          if (this.m_cubes.TryGetValue(pos + Vector3I.Round(direction), out myCube2) && myCube2.CubeBlock.ShowParts)
          {
            Matrix result;
            myCube2.CubeBlock.Orientation.GetMatrix(out result);
            MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(myCube2.CubeBlock.BlockDefinition);
            for (int index = 0; index < myCube2.Parts.Length; ++index)
            {
              Vector3 vector3 = Vector3.Normalize(Vector3.TransformNormal(cubeTiles[index].Normal, result));
              if ((double) (direction + vector3).LengthSquared() < 1.0 / 1000.0)
                this.Render.RenderData.AddCubePart(myCube2.Parts[index]);
            }
          }
        }
      }
    }

    private void RemoveRedundantParts()
    {
      foreach (KeyValuePair<Vector3I, MyCube> cube in this.m_cubes)
        this.UpdateParts(cube.Key);
    }

    private void BoundsInclude(MySlimBlock block)
    {
      if (block == null)
        return;
      this.m_min = Vector3I.Min(this.m_min, block.Min);
      this.m_max = Vector3I.Max(this.m_max, block.Max);
    }

    private void BoundsIncludeUpdateAABB(MySlimBlock block)
    {
      this.BoundsInclude(block);
      this.UpdateGridAABB();
    }

    private void RecalcBounds()
    {
      this.m_min = Vector3I.MaxValue;
      this.m_max = Vector3I.MinValue;
      foreach (KeyValuePair<Vector3I, MyCube> cube in this.m_cubes)
      {
        this.m_min = Vector3I.Min(this.m_min, cube.Key);
        this.m_max = Vector3I.Max(this.m_max, cube.Key);
      }
      if (this.m_cubes.Count == 0)
      {
        this.m_min = -Vector3I.One;
        this.m_max = Vector3I.One;
      }
      this.UpdateGridAABB();
    }

    private void UpdateGridAABB() => this.PositionComp.LocalAABB = new BoundingBox(this.m_min * this.GridSize - this.GridSizeHalfVector, this.m_max * this.GridSize + this.GridSizeHalfVector);

    private void ResetSkeleton() => this.Skeleton = new MyGridSkeleton();

    private bool MoveCornerBones(
      Vector3I cubePos,
      Vector3I offset,
      ref Vector3I minCube,
      ref Vector3I maxCube)
    {
      Vector3I vector3I1 = Vector3I.Abs(offset);
      Vector3I vector3I2 = Vector3I.Shift(vector3I1);
      Vector3I vector3I3 = offset * vector3I2;
      Vector3I vector3I4 = offset * Vector3I.Shift(vector3I2);
      Vector3 sizeQuarterVector = this.GridSizeQuarterVector;
      int num = this.m_cubes.ContainsKey(cubePos + offset) & this.m_cubes.ContainsKey(cubePos + vector3I3) & this.m_cubes.ContainsKey(cubePos + vector3I4) ? 1 : 0;
      if (num == 0)
        return num != 0;
      Vector3I vector3I5 = Vector3I.One - vector3I1;
      Vector3I boneOffset1 = Vector3I.One + offset;
      Vector3I boneOffset2 = boneOffset1 + vector3I5;
      Vector3I boneOffset3 = boneOffset1 - vector3I5;
      Vector3 vector3 = -offset * 0.25f;
      if ((double) MyCubeGrid.m_precalculatedCornerBonesDisplacementDistance <= 0.0)
        MyCubeGrid.m_precalculatedCornerBonesDisplacementDistance = vector3.Length();
      float displacementLength = MyCubeGrid.m_precalculatedCornerBonesDisplacementDistance * this.GridSize;
      Vector3 moveDirection = vector3 * this.GridSize;
      this.MoveBone(ref cubePos, ref boneOffset1, ref moveDirection, ref displacementLength, ref sizeQuarterVector);
      this.MoveBone(ref cubePos, ref boneOffset2, ref moveDirection, ref displacementLength, ref sizeQuarterVector);
      this.MoveBone(ref cubePos, ref boneOffset3, ref moveDirection, ref displacementLength, ref sizeQuarterVector);
      minCube = Vector3I.Min(Vector3I.Min(cubePos, minCube), cubePos + offset - vector3I5);
      maxCube = Vector3I.Max(Vector3I.Max(cubePos, maxCube), cubePos + offset + vector3I5);
      return num != 0;
    }

    private void GetExistingCubes(
      Vector3I cubePos,
      IEnumerable<Vector3I> offsets,
      Dictionary<Vector3I, MySlimBlock> resultSet)
    {
      resultSet.Clear();
      foreach (Vector3I offset in offsets)
      {
        MyCube myCube;
        if (this.m_cubes.TryGetValue(cubePos + offset, out myCube) && !myCube.CubeBlock.IsDestroyed && myCube.CubeBlock.UsesDeformation)
          resultSet[offset] = myCube.CubeBlock;
      }
    }

    public void GetExistingCubes(
      Vector3I boneMin,
      Vector3I boneMax,
      Dictionary<Vector3I, MySlimBlock> resultSet,
      MyDamageInformation? damageInfo = null)
    {
      resultSet.Clear();
      Vector3I result1 = Vector3I.Floor((boneMin - Vector3I.One) / 2f);
      Vector3I result2 = Vector3I.Ceiling((boneMax - Vector3I.One) / 2f);
      MyDamageInformation info = damageInfo.HasValue ? damageInfo.Value : new MyDamageInformation();
      Vector3I.Max(ref result1, ref this.m_min, out result1);
      Vector3I.Min(ref result2, ref this.m_max, out result2);
      Vector3I key;
      for (key.X = result1.X; key.X <= result2.X; ++key.X)
      {
        for (key.Y = result1.Y; key.Y <= result2.Y; ++key.Y)
        {
          for (key.Z = result1.Z; key.Z <= result2.Z; ++key.Z)
          {
            MyCube myCube;
            if (this.m_cubes.TryGetValue(key, out myCube) && myCube.CubeBlock.UsesDeformation)
            {
              if (myCube.CubeBlock.UseDamageSystem && damageInfo.HasValue)
              {
                info.Amount = 1f;
                MyDamageSystem.Static.RaiseBeforeDamageApplied((object) myCube.CubeBlock, ref info);
                if ((double) info.Amount == 0.0)
                  continue;
              }
              resultSet[key] = myCube.CubeBlock;
            }
          }
        }
      }
    }

    public MySlimBlock GetExistingCubeForBoneDeformations(
      ref Vector3I cube,
      ref MyDamageInformation damageInfo)
    {
      MyCube myCube;
      if (this.m_cubes.TryGetValue(cube, out myCube))
      {
        MySlimBlock cubeBlock = myCube.CubeBlock;
        if (cubeBlock.UsesDeformation)
        {
          if (cubeBlock.UseDamageSystem)
          {
            damageInfo.Amount = 1f;
            MyDamageSystem.Static.RaiseBeforeDamageApplied((object) cubeBlock, ref damageInfo);
            if ((double) damageInfo.Amount == 0.0)
              return (MySlimBlock) null;
          }
          return cubeBlock;
        }
      }
      return (MySlimBlock) null;
    }

    private void MoveBone(
      ref Vector3I cubePos,
      ref Vector3I boneOffset,
      ref Vector3 moveDirection,
      ref float displacementLength,
      ref Vector3 clamp)
    {
      this.m_totalBoneDisplacement += displacementLength;
      Vector3I pos = cubePos * 2 + boneOffset;
      Vector3 vector3 = Vector3.Clamp(this.Skeleton[pos] + moveDirection, -clamp, clamp);
      this.Skeleton[pos] = vector3;
    }

    private void RemoveBlockParts(MySlimBlock block)
    {
      Vector3I key;
      for (key.X = block.Min.X; key.X <= block.Max.X; ++key.X)
      {
        for (key.Y = block.Min.Y; key.Y <= block.Max.Y; ++key.Y)
        {
          for (key.Z = block.Min.Z; key.Z <= block.Max.Z; ++key.Z)
          {
            MyCube myCube;
            if (this.m_cubes.TryRemove(key, out myCube))
              this.m_dirtyRegion.PartsToRemove.Enqueue(myCube);
          }
        }
      }
      this.ScheduleDirtyRegion();
      this.MarkForDraw();
    }

    public MyCubeGrid DetectMerge(
      MySlimBlock block,
      MyCubeGrid ignore = null,
      List<MyEntity> nearEntities = null,
      bool newGrid = false)
    {
      if (!this.IsStatic)
        return (MyCubeGrid) null;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return (MyCubeGrid) null;
      if (block == null)
        return (MyCubeGrid) null;
      MyCubeGrid myCubeGrid1 = (MyCubeGrid) null;
      BoundingBoxD boundingBox = (BoundingBoxD) new BoundingBox(block.Min * this.GridSize - this.GridSizeHalf, block.Max * this.GridSize + this.GridSizeHalf);
      boundingBox.Inflate((double) this.GridSizeHalf);
      boundingBox = boundingBox.TransformFast(this.WorldMatrix);
      bool flag = false;
      if (nearEntities == null)
      {
        flag = true;
        nearEntities = MyEntities.GetEntitiesInAABB(ref boundingBox);
      }
      for (int index = 0; index < nearEntities.Count; ++index)
      {
        MyCubeGrid nearEntity = nearEntities[index] as MyCubeGrid;
        MyCubeGrid myCubeGrid2 = myCubeGrid1 ?? this;
        if (nearEntity != null && nearEntity != this && (nearEntity != ignore && nearEntity.Physics != null) && (nearEntity.Physics.Enabled && nearEntity.IsStatic && (nearEntity.GridSizeEnum == myCubeGrid2.GridSizeEnum && myCubeGrid2.IsMergePossible_Static(block, nearEntity, out Vector3I _))))
        {
          MyCubeGrid myCubeGrid3 = myCubeGrid2;
          MyCubeGrid myCubeGrid4 = nearEntity;
          if (nearEntity.BlocksCount > myCubeGrid2.BlocksCount | newGrid)
          {
            myCubeGrid3 = nearEntity;
            myCubeGrid4 = myCubeGrid2;
          }
          Vector3I vector3I = Vector3I.Round(Vector3D.Transform(myCubeGrid4.PositionComp.GetPosition(), myCubeGrid3.PositionComp.WorldMatrixNormalizedInv) * (double) this.GridSizeR);
          if (myCubeGrid3.CanMoveBlocksFrom(myCubeGrid4, vector3I))
          {
            if (newGrid)
              Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicateImmediatelly((IMyReplicable) MyExternalReplicable.FindByObject((object) this), (IMyReplicable) MyExternalReplicable.FindByObject((object) myCubeGrid3));
            MyCubeGrid myCubeGrid5 = myCubeGrid3.MergeGrid_Static(myCubeGrid4, vector3I, block);
            if (myCubeGrid5 != null)
              myCubeGrid1 = myCubeGrid5;
          }
        }
      }
      if (flag)
        nearEntities.Clear();
      return myCubeGrid1;
    }

    private bool IsMergePossible_Static(
      MySlimBlock block,
      MyCubeGrid gridToMerge,
      out Vector3I gridOffset)
    {
      Vector3D vector3D = Vector3D.Transform(this.PositionComp.GetPosition(), gridToMerge.PositionComp.WorldMatrixNormalizedInv);
      gridOffset = -Vector3I.Round(vector3D * (double) this.GridSizeR);
      if (!MyCubeGrid.IsOrientationsAligned(gridToMerge.WorldMatrix, this.WorldMatrix))
        return false;
      MatrixI mergeTransform = gridToMerge.CalculateMergeTransform(this, -gridOffset);
      Vector3I result1;
      Vector3I.Transform(ref block.Position, ref mergeTransform, out result1);
      Quaternion result2;
      MatrixI.Transform(ref block.Orientation, ref mergeTransform).GetQuaternion(out result2);
      MyCubeBlockDefinition.MountPoint[] modelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
      return MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) gridToMerge, block.BlockDefinition, modelMountPoints, ref result2, ref result1);
    }

    public MatrixI CalculateMergeTransform(MyCubeGrid gridToMerge, Vector3I gridOffset)
    {
      Vector3 vec1 = (Vector3) Vector3D.TransformNormal(gridToMerge.WorldMatrix.Forward, this.PositionComp.WorldMatrixNormalizedInv);
      Vector3 vec2 = (Vector3) Vector3D.TransformNormal(gridToMerge.WorldMatrix.Up, this.PositionComp.WorldMatrixNormalizedInv);
      Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(vec1);
      Base6Directions.Direction up = Base6Directions.GetClosestDirection(vec2);
      if (up == closestDirection)
        up = Base6Directions.GetPerpendicular(closestDirection);
      return new MatrixI(ref gridOffset, closestDirection, up);
    }

    public bool CanMergeCubes(MyCubeGrid gridToMerge, Vector3I gridOffset)
    {
      MatrixI mergeTransform = this.CalculateMergeTransform(gridToMerge, gridOffset);
      foreach (KeyValuePair<Vector3I, MyCube> cube in gridToMerge.m_cubes)
      {
        Vector3I vector3I = Vector3I.Transform(cube.Key, mergeTransform);
        if (this.m_cubes.ContainsKey(vector3I))
        {
          MySlimBlock cubeBlock1 = this.GetCubeBlock(vector3I);
          if (cubeBlock1 != null && cubeBlock1.FatBlock is MyCompoundCubeBlock)
          {
            MyCompoundCubeBlock fatBlock1 = cubeBlock1.FatBlock as MyCompoundCubeBlock;
            MySlimBlock cubeBlock2 = gridToMerge.GetCubeBlock(cube.Key);
            if (cubeBlock2.FatBlock is MyCompoundCubeBlock)
            {
              MyCompoundCubeBlock fatBlock2 = cubeBlock2.FatBlock as MyCompoundCubeBlock;
              bool flag = true;
              foreach (MySlimBlock block in fatBlock2.GetBlocks())
              {
                MyBlockOrientation blockOrientation = MatrixI.Transform(ref block.Orientation, ref mergeTransform);
                if (!fatBlock1.CanAddBlock(block.BlockDefinition, new MyBlockOrientation?(blockOrientation)))
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                continue;
            }
            else
            {
              MyBlockOrientation blockOrientation = MatrixI.Transform(ref cubeBlock2.Orientation, ref mergeTransform);
              if (fatBlock1.CanAddBlock(cubeBlock2.BlockDefinition, new MyBlockOrientation?(blockOrientation)))
                continue;
            }
          }
          if (cubeBlock1.FatBlock != null && cube.Value != null && (cube.Value.CubeBlock != null && cube.Value.CubeBlock.FatBlock != null))
          {
            IMyPistonTop fatBlock1;
            if (cube.Value.CubeBlock.FatBlock is IMyPistonTop)
            {
              fatBlock1 = (IMyPistonTop) cube.Value.CubeBlock.FatBlock;
            }
            else
            {
              if (!(cubeBlock1.FatBlock is IMyPistonTop))
                return false;
              fatBlock1 = (IMyPistonTop) cubeBlock1.FatBlock;
            }
            IMyPistonBase fatBlock2;
            if (cube.Value.CubeBlock.FatBlock is IMyPistonBase)
            {
              fatBlock2 = (IMyPistonBase) cube.Value.CubeBlock.FatBlock;
            }
            else
            {
              if (!(cubeBlock1.FatBlock is IMyPistonBase))
                return false;
              fatBlock2 = (IMyPistonBase) cubeBlock1.FatBlock;
            }
            if (((VRage.ModAPI.IMyEntity) fatBlock2.Top).EntityId == ((VRage.ModAPI.IMyEntity) fatBlock1).EntityId && ((VRage.ModAPI.IMyEntity) fatBlock1.Base).EntityId == ((VRage.ModAPI.IMyEntity) fatBlock2).EntityId)
              continue;
          }
          return false;
        }
      }
      return true;
    }

    public void ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.ChangeGridOwner(playerId, shareMode);
    }

    private static void MoveBlocks(
      MyCubeGrid from,
      MyCubeGrid to,
      List<MySlimBlock> cubeBlocks,
      int offset,
      int count)
    {
      to.IsBlockTrasferInProgress = true;
      from.IsBlockTrasferInProgress = true;
      try
      {
        MyCubeGrid.m_tmpBlockGroups.Clear();
        foreach (MyBlockGroup blockGroup in from.BlockGroups)
          MyCubeGrid.m_tmpBlockGroups.Add(blockGroup.GetObjectBuilder());
        for (int index = offset; index < offset + count; ++index)
        {
          MySlimBlock cubeBlock = cubeBlocks[index];
          if (cubeBlock != null)
          {
            if (cubeBlock.FatBlock != null)
              from.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) cubeBlock.FatBlock);
            from.RemoveBlockInternal(cubeBlock, false, false);
          }
        }
        if (from.Physics != null)
        {
          for (int index = offset; index < offset + count; ++index)
          {
            MySlimBlock cubeBlock = cubeBlocks[index];
            if (cubeBlock != null)
              from.Physics.AddDirtyBlock(cubeBlock);
          }
        }
        for (int index = offset; index < offset + count; ++index)
        {
          MySlimBlock cubeBlock = cubeBlocks[index];
          if (cubeBlock != null)
          {
            to.AddBlockInternal(cubeBlock);
            from.Skeleton.CopyTo(to.Skeleton, cubeBlock.Position);
          }
        }
        foreach (MyObjectBuilder_BlockGroup mTmpBlockGroup in MyCubeGrid.m_tmpBlockGroups)
        {
          MyBlockGroup group = new MyBlockGroup();
          group.Init(to, mTmpBlockGroup);
          if (group.Blocks.Count > 0)
            to.AddGroup(group);
        }
        MyCubeGrid.m_tmpBlockGroups.Clear();
        from.RemoveEmptyBlockGroups();
      }
      finally
      {
        to.IsBlockTrasferInProgress = false;
        from.IsBlockTrasferInProgress = false;
      }
    }

    private static void MoveBlocksByObjectBuilders(
      MyCubeGrid from,
      MyCubeGrid to,
      List<MySlimBlock> cubeBlocks,
      int offset,
      int count)
    {
      try
      {
        List<MyObjectBuilder_CubeBlock> builderCubeBlockList = new List<MyObjectBuilder_CubeBlock>();
        for (int index = offset; index < offset + count; ++index)
        {
          MySlimBlock cubeBlock = cubeBlocks[index];
          builderCubeBlockList.Add(cubeBlock.GetObjectBuilder(true));
        }
        MyEntityIdRemapHelper entityIdRemapHelper = new MyEntityIdRemapHelper();
        foreach (MyObjectBuilder_CubeBlock builderCubeBlock in builderCubeBlockList)
          builderCubeBlock.Remap((IMyRemapHelper) entityIdRemapHelper);
        for (int index = offset; index < offset + count; ++index)
        {
          MySlimBlock cubeBlock = cubeBlocks[index];
          from.RemoveBlockInternal(cubeBlock, true, false);
        }
        foreach (MyObjectBuilder_CubeBlock objectBuilder in builderCubeBlockList)
          to.AddBlock(objectBuilder, false);
      }
      finally
      {
      }
    }

    private void RemoveEmptyBlockGroups()
    {
      for (int index = 0; index < this.BlockGroups.Count; ++index)
      {
        MyBlockGroup blockGroup = this.BlockGroups[index];
        if (blockGroup.Blocks.Count == 0)
        {
          this.RemoveGroup(blockGroup);
          --index;
        }
      }
    }

    private void AddBlockInternal(MySlimBlock block)
    {
      if (block.FatBlock != null)
      {
        block.FatBlock.UpdateWorldMatrix();
        if (block.FatBlock.InventoryCount > 0)
          this.RegisterInventory(block.FatBlock);
      }
      block.CubeGrid = this;
      if (block.BlockDefinition.IsStandAlone)
        ++this.m_standAloneBlockCount;
      if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
      {
        MyCompoundCubeBlock fatBlock = block.FatBlock as MyCompoundCubeBlock;
        MySlimBlock cubeBlock = this.GetCubeBlock(block.Min);
        MyCompoundCubeBlock compoundCubeBlock = cubeBlock != null ? cubeBlock.FatBlock as MyCompoundCubeBlock : (MyCompoundCubeBlock) null;
        if (compoundCubeBlock != null)
        {
          bool flag = false;
          fatBlock.UpdateWorldMatrix();
          MyCubeGrid.m_tmpSlimBlocks.Clear();
          foreach (MySlimBlock block1 in fatBlock.GetBlocks())
          {
            if (compoundCubeBlock.Add(block1, out ushort _))
            {
              this.BoundsInclude(block1);
              this.m_dirtyRegion.AddCube(block1.Min);
              this.Physics.AddDirtyBlock(cubeBlock);
              MyCubeGrid.m_tmpSlimBlocks.Add(block1);
              flag = true;
            }
          }
          this.ScheduleDirtyRegion();
          this.MarkForDraw();
          foreach (MySlimBlock tmpSlimBlock in MyCubeGrid.m_tmpSlimBlocks)
            fatBlock.Remove(tmpSlimBlock, true);
          if (flag)
          {
            if (MyCubeGridSmallToLargeConnection.Static != null && this.m_enableSmallToLargeConnections)
              MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
            foreach (MySlimBlock tmpSlimBlock in MyCubeGrid.m_tmpSlimBlocks)
              this.NotifyBlockAdded(tmpSlimBlock);
          }
          MyCubeGrid.m_tmpSlimBlocks.Clear();
          return;
        }
      }
      this.m_cubeBlocks.Add(block);
      if (block.FatBlock != null)
        this.m_fatBlocks.Add(block.FatBlock);
      if (!this.m_colorStatistics.ContainsKey(block.ColorMaskHSV))
        this.m_colorStatistics.Add(block.ColorMaskHSV, 0);
      this.m_colorStatistics[block.ColorMaskHSV]++;
      block.AddNeighbours();
      this.BoundsInclude(block);
      if (block.FatBlock != null)
      {
        this.Hierarchy.AddChild((VRage.ModAPI.IMyEntity) block.FatBlock);
        this.GridSystems.RegisterInSystems(block.FatBlock);
        if (block.FatBlock.Render.NeedsDrawFromParent)
        {
          this.m_blocksForDraw.Add(block.FatBlock);
          block.FatBlock.Render.SetVisibilityUpdates(true);
        }
        MyObjectBuilderType typeId = block.BlockDefinition.Id.TypeId;
        if (typeId != typeof (MyObjectBuilder_CubeBlock))
        {
          if (!this.BlocksCounters.ContainsKey(typeId))
            this.BlocksCounters.Add(typeId, 0);
          this.BlocksCounters[typeId]++;
        }
      }
      Matrix result;
      block.Orientation.GetMatrix(out result);
      bool flag1 = true;
      Vector3I pos = new Vector3I();
      for (pos.X = block.Min.X; pos.X <= block.Max.X; ++pos.X)
      {
        for (pos.Y = block.Min.Y; pos.Y <= block.Max.Y; ++pos.Y)
        {
          for (pos.Z = block.Min.Z; pos.Z <= block.Max.Z; ++pos.Z)
            flag1 &= this.AddCube(block, ref pos, result, block.BlockDefinition);
        }
      }
      if (this.Physics != null)
        this.Physics.AddBlock(block);
      if (block.FatBlock != null)
        this.ChangeOwner(block.FatBlock, 0L, block.FatBlock.OwnerId);
      if (((MyCubeGridSmallToLargeConnection.Static == null ? 0 : (this.m_enableSmallToLargeConnections ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
        MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
      if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
        this.AddMultiBlockInfo(block);
      this.NotifyBlockAdded(block);
      block.AddAuthorship();
      this.m_PCU += block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
    }

    private bool IsDamaged(Vector3I bonePos, float epsilon = 0.04f)
    {
      Vector3 bone;
      return this.Skeleton.TryGetBone(ref bonePos, out bone) && !MyUtils.IsZero(ref bone, epsilon * this.GridSize);
    }

    private void RemoveAuthorshipAll()
    {
      foreach (MySlimBlock block in this.GetBlocks())
      {
        block.RemoveAuthorship();
        this.m_PCU -= block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      }
    }

    public void DismountAllCockpits()
    {
      foreach (MySlimBlock block in this.GetBlocks())
      {
        if (block.FatBlock is MyCockpit fatBlock && fatBlock.Pilot != null)
          fatBlock.Use();
      }
    }

    private void AddBlockEdges(MySlimBlock block)
    {
      MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
      if (blockDefinition.BlockTopology != MyBlockTopology.Cube || blockDefinition.CubeDefinition == null || !blockDefinition.CubeDefinition.ShowEdges)
        return;
      Vector3 vector3_1 = block.Position * this.GridSize;
      Matrix result;
      block.Orientation.GetMatrix(out result);
      result.Translation = vector3_1;
      MyCubeGridDefinitions.TableEntry topologyInfo = MyCubeGridDefinitions.GetTopologyInfo(blockDefinition.CubeDefinition.CubeTopology);
      Vector3I vector3I = block.Position * 2 + Vector3I.One;
      foreach (MyEdgeDefinition edge in topologyInfo.Edges)
      {
        Vector3 vector3_2 = Vector3.TransformNormal(edge.Point0, block.Orientation);
        Vector3 point1 = Vector3.TransformNormal(edge.Point1, block.Orientation);
        Vector3 vector3_3 = (vector3_2 + point1) * 0.5f;
        if (!this.IsDamaged(vector3I + Vector3I.Round(vector3_2)) && !this.IsDamaged(vector3I + Vector3I.Round(vector3_3)) && !this.IsDamaged(vector3I + Vector3I.Round(point1)))
        {
          Vector3 point0 = Vector3.Transform(edge.Point0 * this.GridSizeHalf, ref result);
          point1 = Vector3.Transform(edge.Point1 * this.GridSizeHalf, ref result);
          if (edge.Side0 < topologyInfo.Tiles.Length && edge.Side1 < topologyInfo.Tiles.Length && (edge.Side0 >= 0 && edge.Side1 >= 0))
          {
            Vector3 normal0 = Vector3.TransformNormal(topologyInfo.Tiles[edge.Side0].Normal, block.Orientation);
            Vector3 normal1 = Vector3.TransformNormal(topologyInfo.Tiles[edge.Side1].Normal, block.Orientation);
            Vector3 colorMaskHsv = block.ColorMaskHSV;
            colorMaskHsv.Y = (float) (((double) colorMaskHsv.Y + 1.0) * 0.5);
            colorMaskHsv.Z = (float) (((double) colorMaskHsv.Z + 1.0) * 0.5);
            this.Render.RenderData.AddEdgeInfo(ref point0, ref point1, ref normal0, ref normal1, colorMaskHsv.HSVtoColor(), block);
          }
        }
      }
    }

    private void RemoveBlockEdges(MySlimBlock block)
    {
      using (this.Pin())
      {
        if (this.MarkedForClose)
          return;
        MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
        if (blockDefinition.BlockTopology != MyBlockTopology.Cube || blockDefinition.CubeDefinition == null)
          return;
        Vector3 vector3 = block.Position * this.GridSize;
        Matrix result;
        block.Orientation.GetMatrix(out result);
        result.Translation = vector3;
        foreach (MyEdgeDefinition edge in MyCubeGridDefinitions.GetTopologyInfo(blockDefinition.CubeDefinition.CubeTopology).Edges)
          this.Render.RenderData.RemoveEdgeInfo(Vector3.Transform(edge.Point0 * this.GridSizeHalf, result), Vector3.Transform(edge.Point1 * this.GridSizeHalf, result), block);
      }
    }

    private void SendBones()
    {
      if (this.BonesToSend.InputCount > 0)
      {
        if (this.m_bonesSendCounter++ <= 10 || this.m_bonesSending)
          return;
        this.m_bonesSendCounter = 0;
        lock (this.BonesToSend)
        {
          MyVoxelSegmentation bonesToSend = this.BonesToSend;
          this.BonesToSend = this.m_bonesToSendSecond;
          this.m_bonesToSendSecond = bonesToSend;
        }
        int inputCount = this.m_bonesToSendSecond.InputCount;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.m_bonesSending = true;
        this.m_workData.Priority = WorkPriority.Low;
        Parallel.Start(new Action<WorkData>(this.SendBonesAsync), (Action<WorkData>) null, this.m_workData);
      }
      else
        this.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.SendBones));
    }

    internal void AddBoneToSend(Vector3I boneIndex)
    {
      lock (this.BonesToSend)
      {
        this.BonesToSend.AddInput(boneIndex);
        this.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.SendBones), 16);
      }
    }

    private long SendBones(
      MyVoxelSegmentationType segmentationType,
      out int bytes,
      out int segmentsCount,
      out int emptyBones)
    {
      int inputCount = this.m_bonesToSendSecond.InputCount;
      long timestamp = Stopwatch.GetTimestamp();
      List<MyVoxelSegmentation.Segment> segments = this.m_bonesToSendSecond.FindSegments(segmentationType);
      if (MyCubeGrid.m_boneByteList == null)
        MyCubeGrid.m_boneByteList = new List<byte>();
      else
        MyCubeGrid.m_boneByteList.Clear();
      emptyBones = 0;
      foreach (MyVoxelSegmentation.Segment segment in segments)
        emptyBones += this.Skeleton.SerializePart(segment.Min, segment.Max, this.GridSize, MyCubeGrid.m_boneByteList) ? 0 : 1;
      if (emptyBones != segments.Count)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, int, List<byte>>(this, (Func<MyCubeGrid, Action<int, List<byte>>>) (x => new Action<int, List<byte>>(x.OnBonesReceived)), segments.Count, MyCubeGrid.m_boneByteList);
      bytes = MyCubeGrid.m_boneByteList.Count;
      segmentsCount = segments.Count;
      return Stopwatch.GetTimestamp() - timestamp;
    }

    private void SendBonesAsync(WorkData workData)
    {
      int inputCount = this.m_bonesToSendSecond.InputCount;
      MyTimeSpan.FromTicks(this.SendBones(MyVoxelSegmentationType.Simple, out int _, out int _, out int _));
      this.m_bonesToSendSecond.ClearInput();
      this.m_bonesSending = false;
    }

    private void RemoveUnsusedBones() => this.Skeleton.RemoveUnusedBones(this);

    internal void AddForDamageApplication(MySlimBlock block)
    {
      this.m_blocksForDamageApplication.Add(block);
      this.m_blocksForDamageApplicationDirty = true;
      if (this.m_blocksForDamageApplication.Count != 1)
        return;
      this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ProcessDamageApplication), 17);
    }

    internal void RemoveFromDamageApplication(MySlimBlock block)
    {
      this.m_blocksForDamageApplication.Remove(block);
      this.m_blocksForDamageApplicationDirty = this.m_blocksForDamageApplication.Count > 0;
    }

    private void ProcessDamageApplication()
    {
      if (!this.m_blocksForDamageApplicationDirty)
        return;
      this.m_blocksForDamageApplicationCopy.AddRange((IEnumerable<MySlimBlock>) this.m_blocksForDamageApplication);
      foreach (MySlimBlock mySlimBlock in this.m_blocksForDamageApplicationCopy)
      {
        if ((double) mySlimBlock.AccumulatedDamage > 0.0)
          mySlimBlock.ApplyAccumulatedDamage(true, 0L);
      }
      this.m_blocksForDamageApplication.Clear();
      this.m_blocksForDamageApplicationCopy.Clear();
      this.m_blocksForDamageApplicationDirty = false;
    }

    public bool GetLineIntersectionExactGrid(
      ref LineD line,
      ref Vector3I position,
      ref double distanceSquared)
    {
      return this.GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared, new MyPhysics.HitInfo?());
    }

    public bool GetLineIntersectionExactGrid(
      ref LineD line,
      ref Vector3I position,
      ref double distanceSquared,
      MyPhysics.HitInfo? hitInfo = null)
    {
      this.RayCastCells(line.From, line.To, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), true, true);
      if (MyCubeGrid.m_cacheRayCastCells.Count == 0)
        return false;
      MyCubeGrid.m_tmpHitList.Clear();
      if (hitInfo.HasValue)
        MyCubeGrid.m_tmpHitList.Add(hitInfo.Value);
      else
        MyPhysics.CastRay(line.From, line.To, MyCubeGrid.m_tmpHitList, 24);
      if (MyCubeGrid.m_tmpHitList.Count == 0)
        return false;
      bool flag = false;
      for (int index1 = 0; index1 < MyCubeGrid.m_cacheRayCastCells.Count; ++index1)
      {
        Vector3I cacheRayCastCell = MyCubeGrid.m_cacheRayCastCells[index1];
        MyCube cube;
        this.m_cubes.TryGetValue(cacheRayCastCell, out cube);
        double num1 = double.MaxValue;
        if (cube != null)
        {
          if (cube.CubeBlock.FatBlock != null && !cube.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
          {
            if (MyCubeGrid.m_tmpHitList.Count > 0)
            {
              int index2 = 0;
              if (MySession.Static.ControlledEntity != null)
              {
                while (index2 < MyCubeGrid.m_tmpHitList.Count - 1 && MyCubeGrid.m_tmpHitList[index2].HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity)
                  ++index2;
              }
              if (index2 <= 1 || MyCubeGrid.m_tmpHitList[index2].HkHitInfo.GetHitEntity() == this)
              {
                Vector3 gridSizeHalfVector = this.GridSizeHalfVector;
                Vector3D vector3D1 = Vector3D.Transform(MyCubeGrid.m_tmpHitList[index2].Position, this.PositionComp.WorldMatrixInvScaled);
                Vector3 vector3 = cacheRayCastCell * this.GridSize;
                Vector3D vector3D2 = vector3D1 - vector3;
                double num2 = vector3D2.Max() > Math.Abs(vector3D2.Min()) ? vector3D2.Max() : vector3D2.Min();
                vector3D2.X = vector3D2.X == num2 ? (num2 > 0.0 ? 1.0 : -1.0) : 0.0;
                vector3D2.Y = vector3D2.Y == num2 ? (num2 > 0.0 ? 1.0 : -1.0) : 0.0;
                vector3D2.Z = vector3D2.Z == num2 ? (num2 > 0.0 ? 1.0 : -1.0) : 0.0;
                Vector3D vector3D3 = vector3D1 - vector3D2 * 0.100000001490116;
                if (Vector3D.Max(vector3D3, (Vector3D) (vector3 - gridSizeHalfVector)) == vector3D3 && Vector3D.Min(vector3D3, (Vector3D) (vector3 + gridSizeHalfVector)) == vector3D3)
                {
                  num1 = Vector3D.DistanceSquared(line.From, MyCubeGrid.m_tmpHitList[index2].Position);
                  if (num1 < distanceSquared)
                  {
                    position = cacheRayCastCell;
                    distanceSquared = num1;
                    flag = true;
                    continue;
                  }
                }
              }
              else
                continue;
            }
          }
          else
          {
            MyIntersectionResultLineTriangleEx? t;
            this.GetBlockIntersection(cube, ref line, IntersectionFlags.ALL_TRIANGLES, out t, out int _);
            if (t.HasValue)
              num1 = (double) Vector3.DistanceSquared((Vector3) line.From, (Vector3) t.Value.IntersectionPointInWorldSpace);
          }
        }
        if (num1 < distanceSquared)
        {
          distanceSquared = num1;
          position = cacheRayCastCell;
          flag = true;
        }
      }
      if (!flag)
      {
        for (int index1 = 0; index1 < MyCubeGrid.m_cacheRayCastCells.Count; ++index1)
        {
          Vector3I key = MyCubeGrid.m_cacheRayCastCells[index1];
          MyCube cube1;
          this.m_cubes.TryGetValue(key, out cube1);
          double num1 = double.MaxValue;
          if (cube1 == null || cube1.CubeBlock.FatBlock == null || !cube1.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
          {
            if (MyCubeGrid.m_tmpHitList.Count > 0)
            {
              int index2 = 0;
              if (MySession.Static.ControlledEntity != null)
              {
                while (index2 < MyCubeGrid.m_tmpHitList.Count - 1 && MyCubeGrid.m_tmpHitList[index2].HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity)
                  ++index2;
              }
              if (index2 <= 1 || MyCubeGrid.m_tmpHitList[index2].HkHitInfo.GetHitEntity() == this)
              {
                Vector3 gridSizeHalfVector = this.GridSizeHalfVector;
                Vector3D vector3D1 = Vector3D.Transform(MyCubeGrid.m_tmpHitList[index2].Position, this.PositionComp.WorldMatrixInvScaled);
                Vector3 vector3 = key * this.GridSize;
                Vector3D vector3D2 = vector3D1 - vector3;
                double num2 = vector3D2.Max() > Math.Abs(vector3D2.Min()) ? vector3D2.Max() : vector3D2.Min();
                vector3D2.X = vector3D2.X == num2 ? (num2 > 0.0 ? 1.0 : -1.0) : 0.0;
                vector3D2.Y = vector3D2.Y == num2 ? (num2 > 0.0 ? 1.0 : -1.0) : 0.0;
                vector3D2.Z = vector3D2.Z == num2 ? (num2 > 0.0 ? 1.0 : -1.0) : 0.0;
                Vector3D vector3D3 = vector3D1 - vector3D2 * 0.0599999986588955;
                if (Vector3D.Max(vector3D3, (Vector3D) (vector3 - gridSizeHalfVector)) == vector3D3 && Vector3D.Min(vector3D3, (Vector3D) (vector3 + gridSizeHalfVector)) == vector3D3)
                {
                  if (cube1 == null)
                  {
                    Vector3I cube2;
                    this.FixTargetCube(out cube2, (Vector3) (vector3D3 * (double) this.GridSizeR));
                    if (this.m_cubes.TryGetValue(cube2, out cube1))
                      key = cube2;
                    else
                      continue;
                  }
                  num1 = Vector3D.DistanceSquared(line.From, MyCubeGrid.m_tmpHitList[index2].Position);
                  if (num1 < distanceSquared)
                  {
                    position = key;
                    distanceSquared = num1;
                    flag = true;
                    continue;
                  }
                }
              }
              else
                continue;
            }
          }
          else
          {
            MyIntersectionResultLineTriangleEx? t;
            this.GetBlockIntersection(cube1, ref line, IntersectionFlags.ALL_TRIANGLES, out t, out int _);
            if (t.HasValue)
              num1 = (double) Vector3.DistanceSquared((Vector3) line.From, (Vector3) t.Value.IntersectionPointInWorldSpace);
          }
          if (num1 < distanceSquared)
          {
            distanceSquared = num1;
            position = key;
            flag = true;
          }
        }
      }
      MyCubeGrid.m_tmpHitList.Clear();
      return flag;
    }

    private void GetBlockIntersection(
      MyCube cube,
      ref LineD line,
      IntersectionFlags flags,
      out MyIntersectionResultLineTriangleEx? t,
      out int cubePartIndex)
    {
      if (cube.CubeBlock.FatBlock != null)
      {
        if (cube.CubeBlock.FatBlock is MyCompoundCubeBlock)
        {
          MyCompoundCubeBlock fatBlock = cube.CubeBlock.FatBlock as MyCompoundCubeBlock;
          MyIntersectionResultLineTriangleEx? nullable = new MyIntersectionResultLineTriangleEx?();
          double num1 = double.MaxValue;
          foreach (MySlimBlock block in fatBlock.GetBlocks())
          {
            Matrix result1;
            block.Orientation.GetMatrix(out result1);
            Vector3 result2;
            Vector3.TransformNormal(ref block.BlockDefinition.ModelOffset, ref result1, out result2);
            result1.Translation = block.Position * this.GridSize + result2;
            MatrixD customInvMatrix1 = MatrixD.Invert(block.FatBlock.WorldMatrix);
            t = block.FatBlock.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, ref customInvMatrix1, flags);
            if (!t.HasValue && block.FatBlock.Subparts != null)
            {
              foreach (KeyValuePair<string, MyEntitySubpart> subpart in block.FatBlock.Subparts)
              {
                MatrixD customInvMatrix2 = MatrixD.Invert(subpart.Value.WorldMatrix);
                t = subpart.Value.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, ref customInvMatrix2, flags);
                if (t.HasValue)
                  break;
              }
            }
            if (t.HasValue)
            {
              MyIntersectionResultLineTriangleEx triangle = t.Value;
              double num2 = Vector3D.Distance(Vector3D.Transform(t.Value.IntersectionPointInObjectSpace, block.FatBlock.WorldMatrix), line.From);
              if (num2 < num1)
              {
                num1 = num2;
                MatrixD? cubeWorldMatrix = new MatrixD?(block.FatBlock.WorldMatrix);
                this.TransformCubeToGrid(ref triangle, ref result1, ref cubeWorldMatrix);
                nullable = new MyIntersectionResultLineTriangleEx?(triangle);
              }
            }
          }
          t = nullable;
        }
        else
        {
          cube.CubeBlock.FatBlock.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES);
          if (t.HasValue)
          {
            Matrix result;
            cube.CubeBlock.Orientation.GetMatrix(out result);
            MyIntersectionResultLineTriangleEx triangle = t.Value;
            MatrixD? cubeWorldMatrix = new MatrixD?(cube.CubeBlock.FatBlock.WorldMatrix);
            this.TransformCubeToGrid(ref triangle, ref result, ref cubeWorldMatrix);
            t = new MyIntersectionResultLineTriangleEx?(triangle);
          }
        }
        cubePartIndex = -1;
      }
      else
      {
        MyIntersectionResultLineTriangleEx? nullable = new MyIntersectionResultLineTriangleEx?();
        float num1 = float.MaxValue;
        int num2 = -1;
        for (int index = 0; index < cube.Parts.Length; ++index)
        {
          MyCubePart part = cube.Parts[index];
          MatrixD matrix = part.InstanceData.LocalMatrix * this.WorldMatrix;
          MatrixD customInvMatrix = MatrixD.Invert(matrix);
          t = part.Model.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, ref customInvMatrix, flags);
          if (t.HasValue)
          {
            MyIntersectionResultLineTriangleEx triangle = t.Value;
            float num3 = Vector3.Distance((Vector3) Vector3.Transform(t.Value.IntersectionPointInObjectSpace, matrix), (Vector3) line.From);
            if ((double) num3 < (double) num1)
            {
              num1 = num3;
              Matrix localMatrix = part.InstanceData.LocalMatrix;
              MatrixD? cubeWorldMatrix = new MatrixD?();
              this.TransformCubeToGrid(ref triangle, ref localMatrix, ref cubeWorldMatrix);
              Vector3 pointInWorldSpace = (Vector3) triangle.IntersectionPointInWorldSpace;
              nullable = new MyIntersectionResultLineTriangleEx?(triangle);
              num2 = index;
            }
          }
        }
        t = nullable;
        cubePartIndex = num2;
      }
    }

    public static bool GetLineIntersection(
      ref LineD line,
      out MyCubeGrid grid,
      out Vector3I position,
      out double distanceSquared,
      Func<MyCubeGrid, bool> condition = null)
    {
      grid = (MyCubeGrid) null;
      position = new Vector3I();
      distanceSquared = 3.40282346638529E+38;
      MyEntities.OverlapAllLineSegment(ref line, MyCubeGrid.m_lineOverlapList);
      foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlap in MyCubeGrid.m_lineOverlapList)
      {
        if (lineOverlap.Element is MyCubeGrid element && (condition == null || condition(element)))
        {
          Vector3I? nullable = element.RayCastBlocks(line.From, line.To);
          if (nullable.HasValue)
          {
            Vector3 closestCorner = element.GetClosestCorner(nullable.Value, (Vector3) line.From);
            float num = (float) Vector3D.DistanceSquared(line.From, Vector3D.Transform(closestCorner, element.WorldMatrix));
            if ((double) num < distanceSquared)
            {
              distanceSquared = (double) num;
              grid = element;
              position = nullable.Value;
            }
          }
        }
      }
      MyCubeGrid.m_lineOverlapList.Clear();
      return grid != null;
    }

    public static bool GetLineIntersectionExact(
      ref LineD line,
      out MyCubeGrid grid,
      out Vector3I position,
      out double distanceSquared)
    {
      grid = (MyCubeGrid) null;
      position = new Vector3I();
      distanceSquared = 3.40282346638529E+38;
      double num = double.MaxValue;
      MyEntities.OverlapAllLineSegment(ref line, MyCubeGrid.m_lineOverlapList);
      foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlap in MyCubeGrid.m_lineOverlapList)
      {
        double distance;
        if (lineOverlap.Element is MyCubeGrid element && (element.GetLineIntersectionExactAll(ref line, out distance, out MySlimBlock _).HasValue && distance < num))
        {
          grid = element;
          num = distance;
        }
      }
      MyCubeGrid.m_lineOverlapList.Clear();
      return grid != null;
    }

    public Vector3D? GetLineIntersectionExactAll(
      ref LineD line,
      out double distance,
      out MySlimBlock intersectedBlock)
    {
      intersectedBlock = (MySlimBlock) null;
      distance = 3.40282346638529E+38;
      Vector3I? nullable = new Vector3I?();
      Vector3I zero = Vector3I.Zero;
      double distanceSquared = double.MaxValue;
      if (this.GetLineIntersectionExactGrid(ref line, ref zero, ref distanceSquared))
      {
        distanceSquared = Math.Sqrt(distanceSquared);
        nullable = new Vector3I?(zero);
      }
      if (!nullable.HasValue)
        return new Vector3D?();
      distance = distanceSquared;
      intersectedBlock = this.GetCubeBlock(nullable.Value);
      return intersectedBlock == null ? new Vector3D?() : new Vector3D?((Vector3D) zero);
    }

    public void GetBlocksInsideSphere(
      ref BoundingSphereD sphere,
      HashSet<MySlimBlock> blocks,
      bool checkTriangles = false)
    {
      this.GetBlocksInsideSphereInternal(ref sphere, blocks, checkTriangles);
    }

    public void GetBlocksInsideSphereInternal(
      ref BoundingSphereD sphere,
      HashSet<MySlimBlock> blocks,
      bool checkTriangles = false,
      bool useOptimization = true)
    {
      blocks.Clear();
      if (this.PositionComp == null)
        return;
      BoundingBoxD fromSphere1 = BoundingBoxD.CreateFromSphere(sphere);
      MatrixD matrix = this.PositionComp.WorldMatrixNormalizedInv;
      Vector3D result;
      Vector3D.Transform(ref sphere.Center, ref matrix, out result);
      BoundingSphere localSphere = new BoundingSphere((Vector3) result, (float) sphere.Radius);
      BoundingBox fromSphere2 = BoundingBox.CreateFromSphere(localSphere);
      Vector3D min = (Vector3D) fromSphere2.Min;
      Vector3D max = (Vector3D) fromSphere2.Max;
      Vector3I vector3I1 = new Vector3I((int) Math.Round(min.X * (double) this.GridSizeR), (int) Math.Round(min.Y * (double) this.GridSizeR), (int) Math.Round(min.Z * (double) this.GridSizeR));
      Vector3I vector3I2 = new Vector3I((int) Math.Round(max.X * (double) this.GridSizeR), (int) Math.Round(max.Y * (double) this.GridSizeR), (int) Math.Round(max.Z * (double) this.GridSizeR));
      Vector3I start = Vector3I.Min(vector3I1, vector3I2);
      Vector3I end = Vector3I.Max(vector3I1, vector3I2);
      if (!useOptimization || (end - start).Volume() < this.m_cubes.Count)
      {
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
        Vector3I next = vector3IRangeIterator.Current;
        while (vector3IRangeIterator.IsValid())
        {
          MyCube cube;
          if (this.m_cubes.TryGetValue(next, out cube))
            this.AddBlockInSphere(ref fromSphere1, blocks, checkTriangles, ref localSphere, cube);
          vector3IRangeIterator.GetNext(out next);
        }
      }
      else
      {
        foreach (MyCube cube in (IEnumerable<MyCube>) this.m_cubes.Values)
          this.AddBlockInSphere(ref fromSphere1, blocks, checkTriangles, ref localSphere, cube);
      }
    }

    private void AddBlockInSphere(
      ref BoundingBoxD aabb,
      HashSet<MySlimBlock> blocks,
      bool checkTriangles,
      ref BoundingSphere localSphere,
      MyCube cube)
    {
      MySlimBlock cubeBlock = cube.CubeBlock;
      if (!new BoundingBox(cubeBlock.Min * this.GridSize - this.GridSizeHalf, cubeBlock.Max * this.GridSize + this.GridSizeHalf).Intersects(localSphere))
        return;
      if (checkTriangles)
      {
        if (cubeBlock.FatBlock != null && !cubeBlock.FatBlock.GetIntersectionWithAABB(ref aabb))
          return;
        blocks.Add(cubeBlock);
      }
      else
        blocks.Add(cubeBlock);
    }

    private void QuerySphere(BoundingSphereD sphere, List<MyEntity> blocks)
    {
      if (this.PositionComp == null)
        return;
      if (this.Closed)
        MyLog.Default.WriteLine("Grid was Closed in MyCubeGrid.QuerySphere!");
      if (sphere.Contains(this.PositionComp.WorldVolume) == ContainmentType.Contains)
      {
        foreach (MyCubeBlock fatBlock in this.m_fatBlocks)
        {
          if (!fatBlock.Closed)
          {
            blocks.Add((MyEntity) fatBlock);
            foreach (MyEntityComponentBase child in fatBlock.Hierarchy.Children)
            {
              MyEntity entity = (MyEntity) child.Entity;
              if (entity != null)
                blocks.Add(entity);
            }
          }
        }
      }
      else
      {
        BoundingBoxD boundingBoxD = new BoundingBoxD(sphere.Center - new Vector3D(sphere.Radius), sphere.Center + new Vector3D(sphere.Radius)).TransformFast(this.PositionComp.WorldMatrixNormalizedInv);
        Vector3D min = boundingBoxD.Min;
        Vector3D max = boundingBoxD.Max;
        Vector3I vector3I1 = new Vector3I((int) Math.Round(min.X * (double) this.GridSizeR), (int) Math.Round(min.Y * (double) this.GridSizeR), (int) Math.Round(min.Z * (double) this.GridSizeR));
        Vector3I vector3I2 = new Vector3I((int) Math.Round(max.X * (double) this.GridSizeR), (int) Math.Round(max.Y * (double) this.GridSizeR), (int) Math.Round(max.Z * (double) this.GridSizeR));
        Vector3I vector3I3 = Vector3I.Min(vector3I1, vector3I2);
        Vector3I vector3I4 = Vector3I.Max(vector3I1, vector3I2);
        Vector3I start = Vector3I.Max(vector3I3, this.Min);
        Vector3I end = Vector3I.Min(vector3I4, this.Max);
        if (start.X > end.X || start.Y > end.Y || start.Z > end.Z)
          return;
        Vector3 vector3 = new Vector3(0.5f);
        BoundingBox box = new BoundingBox();
        BoundingSphere boundingSphere = new BoundingSphere((Vector3) boundingBoxD.Center * this.GridSizeR, (float) sphere.Radius * this.GridSizeR);
        if ((end - start).Size > this.m_cubeBlocks.Count)
        {
          foreach (MyCubeBlock fatBlock in this.m_fatBlocks)
          {
            if (!fatBlock.Closed)
            {
              box.Min = (Vector3) fatBlock.Min - vector3;
              box.Max = (Vector3) fatBlock.Max + vector3;
              if (boundingSphere.Intersects(box))
              {
                blocks.Add((MyEntity) fatBlock);
                foreach (MyEntityComponentBase child in fatBlock.Hierarchy.Children)
                {
                  MyEntity entity = (MyEntity) child.Entity;
                  if (entity != null)
                    blocks.Add(entity);
                }
              }
            }
          }
        }
        else
        {
          if (MyCubeGrid.m_tmpQueryCubeBlocks == null)
            MyCubeGrid.m_tmpQueryCubeBlocks = new HashSet<MyEntity>();
          if (this.m_cubes == null)
            MyLog.Default.WriteLine("m_cubes null in MyCubeGrid.QuerySphere!");
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
          Vector3I next = vector3IRangeIterator.Current;
          while (vector3IRangeIterator.IsValid())
          {
            MyCube myCube;
            if (this.m_cubes.TryGetValue(next, out myCube) && myCube.CubeBlock.FatBlock != null && (myCube.CubeBlock.FatBlock != null && !myCube.CubeBlock.FatBlock.Closed) && !MyCubeGrid.m_tmpQueryCubeBlocks.Contains((MyEntity) myCube.CubeBlock.FatBlock))
            {
              box.Min = (Vector3) myCube.CubeBlock.Min - vector3;
              box.Max = (Vector3) myCube.CubeBlock.Max + vector3;
              if (boundingSphere.Intersects(box))
              {
                blocks.Add((MyEntity) myCube.CubeBlock.FatBlock);
                MyCubeGrid.m_tmpQueryCubeBlocks.Add((MyEntity) myCube.CubeBlock.FatBlock);
                foreach (MyEntityComponentBase child in myCube.CubeBlock.FatBlock.Hierarchy.Children)
                {
                  MyEntity entity = (MyEntity) child.Entity;
                  if (entity != null)
                  {
                    blocks.Add(entity);
                    MyCubeGrid.m_tmpQueryCubeBlocks.Add(entity);
                  }
                }
              }
            }
            vector3IRangeIterator.GetNext(out next);
          }
          MyCubeGrid.m_tmpQueryCubeBlocks.Clear();
        }
      }
    }

    private void TransformCubeToGrid(
      ref MyIntersectionResultLineTriangleEx triangle,
      ref Matrix cubeLocalMatrix,
      ref MatrixD? cubeWorldMatrix)
    {
      if (!cubeWorldMatrix.HasValue)
      {
        MatrixD worldMatrix = this.WorldMatrix;
        triangle.IntersectionPointInObjectSpace = Vector3.Transform(triangle.IntersectionPointInObjectSpace, ref cubeLocalMatrix);
        triangle.IntersectionPointInWorldSpace = Vector3D.Transform(triangle.IntersectionPointInObjectSpace, worldMatrix);
        triangle.NormalInObjectSpace = Vector3.TransformNormal(triangle.NormalInObjectSpace, ref cubeLocalMatrix);
        triangle.NormalInWorldSpace = Vector3.TransformNormal(triangle.NormalInObjectSpace, worldMatrix);
      }
      else
      {
        Vector3 pointInObjectSpace = triangle.IntersectionPointInObjectSpace;
        Vector3 normalInObjectSpace = triangle.NormalInObjectSpace;
        triangle.IntersectionPointInObjectSpace = Vector3.Transform(pointInObjectSpace, ref cubeLocalMatrix);
        triangle.IntersectionPointInWorldSpace = Vector3D.Transform(pointInObjectSpace, cubeWorldMatrix.Value);
        triangle.NormalInObjectSpace = Vector3.TransformNormal(normalInObjectSpace, ref cubeLocalMatrix);
        triangle.NormalInWorldSpace = Vector3.TransformNormal(normalInObjectSpace, cubeWorldMatrix.Value);
      }
      triangle.Triangle.InputTriangle.Transform(ref cubeLocalMatrix);
    }

    private void QueryLine(LineD line, List<MyLineSegmentOverlapResult<MyEntity>> blocks)
    {
      MyLineSegmentOverlapResult<MyEntity> segmentOverlapResult = new MyLineSegmentOverlapResult<MyEntity>();
      BoundingBoxD box = new BoundingBoxD();
      MatrixD matrix = this.PositionComp.WorldMatrixNormalizedInv;
      Vector3D result1;
      Vector3D.Transform(ref line.From, ref matrix, out result1);
      Vector3D result2;
      Vector3D.Transform(ref line.To, ref matrix, out result2);
      RayD rayD = new RayD(result1, Vector3D.Normalize(result2 - result1));
      this.RayCastCells(line.From, line.To, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), false, true);
      foreach (Vector3I cacheRayCastCell in MyCubeGrid.m_cacheRayCastCells)
      {
        MyCube myCube;
        if (this.m_cubes.TryGetValue(cacheRayCastCell, out myCube) && myCube.CubeBlock.FatBlock != null)
        {
          MyCubeBlock fatBlock = myCube.CubeBlock.FatBlock;
          segmentOverlapResult.Element = (MyEntity) fatBlock;
          box.Min = (Vector3D) (fatBlock.Min * this.GridSize - this.GridSizeHalfVector);
          box.Max = (Vector3D) (fatBlock.Max * this.GridSize + this.GridSizeHalfVector);
          double? nullable = rayD.Intersects(box);
          if (nullable.HasValue)
          {
            segmentOverlapResult.Distance = nullable.Value;
            blocks.Add(segmentOverlapResult);
          }
        }
      }
    }

    private void QueryAABB(BoundingBoxD box, List<MyEntity> blocks)
    {
      if (blocks == null || this.PositionComp == null)
        return;
      if (box.Contains(this.PositionComp.WorldAABB) == ContainmentType.Contains)
      {
        foreach (MyCubeBlock fatBlock in this.m_fatBlocks)
        {
          if (!fatBlock.Closed)
          {
            blocks.Add((MyEntity) fatBlock);
            if (fatBlock.Hierarchy != null)
            {
              foreach (MyHierarchyComponentBase child in fatBlock.Hierarchy.Children)
              {
                if (child.Container != null)
                  blocks.Add((MyEntity) child.Container.Entity);
              }
            }
          }
        }
      }
      else
      {
        MyOrientedBoundingBoxD orientedBoundingBoxD = MyOrientedBoundingBoxD.Create(box, this.PositionComp.WorldMatrixNormalizedInv);
        orientedBoundingBoxD.Center *= (double) this.GridSizeR;
        orientedBoundingBoxD.HalfExtent *= (double) this.GridSizeR;
        box = box.TransformFast(this.PositionComp.WorldMatrixNormalizedInv);
        Vector3D min = box.Min;
        Vector3D max = box.Max;
        Vector3I vector3I1 = new Vector3I((int) Math.Round(min.X * (double) this.GridSizeR), (int) Math.Round(min.Y * (double) this.GridSizeR), (int) Math.Round(min.Z * (double) this.GridSizeR));
        Vector3I vector3I2 = new Vector3I((int) Math.Round(max.X * (double) this.GridSizeR), (int) Math.Round(max.Y * (double) this.GridSizeR), (int) Math.Round(max.Z * (double) this.GridSizeR));
        Vector3I vector3I3 = Vector3I.Min(vector3I1, vector3I2);
        Vector3I vector3I4 = Vector3I.Max(vector3I1, vector3I2);
        Vector3I start = Vector3I.Max(vector3I3, this.Min);
        Vector3I end = Vector3I.Min(vector3I4, this.Max);
        if (start.X > end.X || start.Y > end.Y || start.Z > end.Z)
          return;
        Vector3 vector3 = new Vector3(0.5f);
        BoundingBoxD box1 = new BoundingBoxD();
        if ((end - start).Size > this.m_cubeBlocks.Count)
        {
          foreach (MyCubeBlock fatBlock in this.m_fatBlocks)
          {
            if (!fatBlock.Closed)
            {
              box1.Min = (Vector3D) ((Vector3) fatBlock.Min - vector3);
              box1.Max = (Vector3D) ((Vector3) fatBlock.Max + vector3);
              if (orientedBoundingBoxD.Intersects(ref box1))
              {
                blocks.Add((MyEntity) fatBlock);
                if (fatBlock.Hierarchy != null)
                {
                  foreach (MyHierarchyComponentBase child in fatBlock.Hierarchy.Children)
                  {
                    if (child.Container != null)
                      blocks.Add((MyEntity) child.Container.Entity);
                  }
                }
              }
            }
          }
        }
        else
        {
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
          Vector3I next = vector3IRangeIterator.Current;
          if (MyCubeGrid.m_tmpQueryCubeBlocks == null)
            MyCubeGrid.m_tmpQueryCubeBlocks = new HashSet<MyEntity>();
          while (vector3IRangeIterator.IsValid())
          {
            MyCube myCube;
            if (this.m_cubes != null && this.m_cubes.TryGetValue(next, out myCube) && myCube.CubeBlock.FatBlock != null)
            {
              MyCubeBlock fatBlock = myCube.CubeBlock.FatBlock;
              if (!MyCubeGrid.m_tmpQueryCubeBlocks.Contains((MyEntity) fatBlock))
              {
                box1.Min = (Vector3D) ((Vector3) myCube.CubeBlock.Min - vector3);
                box1.Max = (Vector3D) ((Vector3) myCube.CubeBlock.Max + vector3);
                if (orientedBoundingBoxD.Intersects(ref box1))
                {
                  MyCubeGrid.m_tmpQueryCubeBlocks.Add((MyEntity) fatBlock);
                  blocks.Add((MyEntity) fatBlock);
                  if (fatBlock.Hierarchy != null)
                  {
                    foreach (MyHierarchyComponentBase child in fatBlock.Hierarchy.Children)
                    {
                      if (child.Container != null)
                      {
                        blocks.Add((MyEntity) child.Container.Entity);
                        MyCubeGrid.m_tmpQueryCubeBlocks.Add((MyEntity) fatBlock);
                      }
                    }
                  }
                }
              }
            }
            vector3IRangeIterator.GetNext(out next);
          }
          MyCubeGrid.m_tmpQueryCubeBlocks.Clear();
        }
      }
    }

    public void GetBlocksIntersectingOBB(
      in BoundingBoxD box,
      in MatrixD boxTransform,
      List<MySlimBlock> blocks)
    {
      if (blocks == null || this.PositionComp == null)
        return;
      MatrixD m = boxTransform * this.PositionComp.WorldMatrixNormalizedInv;
      MyOrientedBoundingBox orientedBoundingBox = MyOrientedBoundingBox.Create((BoundingBox) box, (Matrix) ref m);
      BoundingBox localAabb = this.PositionComp.LocalAABB;
      if (orientedBoundingBox.Contains(ref localAabb) == ContainmentType.Contains)
      {
        foreach (MySlimBlock block in this.GetBlocks())
        {
          if (block.FatBlock == null || !block.FatBlock.Closed)
            blocks.Add(block);
        }
      }
      else
      {
        orientedBoundingBox.Center *= this.GridSizeR;
        orientedBoundingBox.HalfExtent *= this.GridSizeR;
        BoundingBoxD boundingBoxD = box.TransformFast(m);
        Vector3D min = boundingBoxD.Min;
        Vector3D max = boundingBoxD.Max;
        Vector3I vector3I1 = new Vector3I((int) Math.Round(min.X * (double) this.GridSizeR), (int) Math.Round(min.Y * (double) this.GridSizeR), (int) Math.Round(min.Z * (double) this.GridSizeR));
        Vector3I vector3I2 = new Vector3I((int) Math.Round(max.X * (double) this.GridSizeR), (int) Math.Round(max.Y * (double) this.GridSizeR), (int) Math.Round(max.Z * (double) this.GridSizeR));
        Vector3I vector3I3 = Vector3I.Min(vector3I1, vector3I2);
        Vector3I vector3I4 = Vector3I.Max(vector3I1, vector3I2);
        Vector3I start = Vector3I.Max(vector3I3, this.Min);
        Vector3I end = Vector3I.Min(vector3I4, this.Max);
        if (start.X > end.X || start.Y > end.Y || start.Z > end.Z)
          return;
        Vector3 vector3 = new Vector3(0.5f);
        if ((end - start).Size > this.m_cubeBlocks.Count)
        {
          foreach (MySlimBlock block in this.GetBlocks())
          {
            if (block.FatBlock == null || !block.FatBlock.Closed)
            {
              BoundingBox box1;
              box1.Min = (Vector3) block.Min - vector3;
              box1.Max = (Vector3) block.Max + vector3;
              if (orientedBoundingBox.Intersects(ref box1))
                blocks.Add(block);
            }
          }
        }
        else
        {
          if (MyCubeGrid.m_tmpQuerySlimBlocks == null)
            MyCubeGrid.m_tmpQuerySlimBlocks = new HashSet<MySlimBlock>();
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
          Vector3I next = vector3IRangeIterator.Current;
          while (vector3IRangeIterator.IsValid())
          {
            MyCube myCube;
            if (this.m_cubes != null && this.m_cubes.TryGetValue(next, out myCube) && myCube.CubeBlock != null)
            {
              MySlimBlock cubeBlock = myCube.CubeBlock;
              if (!MyCubeGrid.m_tmpQuerySlimBlocks.Contains(cubeBlock))
              {
                BoundingBox box1;
                box1.Min = (Vector3) cubeBlock.Min - vector3;
                box1.Max = (Vector3) cubeBlock.Max + vector3;
                if (orientedBoundingBox.Intersects(ref box1))
                {
                  MyCubeGrid.m_tmpQuerySlimBlocks.Add(cubeBlock);
                  blocks.Add(cubeBlock);
                }
              }
            }
            vector3IRangeIterator.GetNext(out next);
          }
          MyCubeGrid.m_tmpQuerySlimBlocks.Clear();
        }
      }
    }

    public void GetBlocksInsideSpheres(
      ref BoundingSphereD sphere1,
      ref BoundingSphereD sphere2,
      ref BoundingSphereD sphere3,
      HashSet<MySlimBlock> blocks1,
      HashSet<MySlimBlock> blocks2,
      HashSet<MySlimBlock> blocks3,
      bool respectDeformationRatio,
      float detectionBlockHalfSize,
      ref MatrixD invWorldGrid)
    {
      blocks1.Clear();
      blocks2.Clear();
      blocks3.Clear();
      this.m_processedBlocks.Clear();
      Vector3D result;
      Vector3D.Transform(ref sphere3.Center, ref invWorldGrid, out result);
      Vector3I vector3I1 = Vector3I.Round((result - sphere3.Radius) * (double) this.GridSizeR);
      Vector3I vector3I2 = Vector3I.Round((result + sphere3.Radius) * (double) this.GridSizeR);
      Vector3 vector3 = new Vector3(detectionBlockHalfSize);
      BoundingSphereD boundingSphereD1 = new BoundingSphereD(result, sphere1.Radius);
      BoundingSphereD boundingSphereD2 = new BoundingSphereD(result, sphere2.Radius);
      BoundingSphereD boundingSphereD3 = new BoundingSphereD(result, sphere3.Radius);
      if ((vector3I2.X - vector3I1.X) * (vector3I2.Y - vector3I1.Y) * (vector3I2.Z - vector3I1.Z) < this.m_cubes.Count)
      {
        Vector3I key = new Vector3I();
        for (key.X = vector3I1.X; key.X <= vector3I2.X; ++key.X)
        {
          for (key.Y = vector3I1.Y; key.Y <= vector3I2.Y; ++key.Y)
          {
            for (key.Z = vector3I1.Z; key.Z <= vector3I2.Z; ++key.Z)
            {
              MyCube myCube;
              if (this.m_cubes.TryGetValue(key, out myCube))
              {
                MySlimBlock cubeBlock = myCube.CubeBlock;
                if (cubeBlock.FatBlock == null || !this.m_processedBlocks.Contains(cubeBlock.FatBlock))
                {
                  this.m_processedBlocks.Add(cubeBlock.FatBlock);
                  if (respectDeformationRatio)
                  {
                    boundingSphereD1.Radius = sphere1.Radius * (double) cubeBlock.DeformationRatio;
                    boundingSphereD2.Radius = sphere2.Radius * (double) cubeBlock.DeformationRatio;
                    boundingSphereD3.Radius = sphere3.Radius * (double) cubeBlock.DeformationRatio;
                  }
                  BoundingBox boundingBox = cubeBlock.FatBlock == null ? new BoundingBox(cubeBlock.Position * this.GridSize - vector3, cubeBlock.Position * this.GridSize + vector3) : new BoundingBox(cubeBlock.Min * this.GridSize - this.GridSizeHalf, cubeBlock.Max * this.GridSize + this.GridSizeHalf);
                  if (boundingBox.Intersects((BoundingSphere) boundingSphereD3))
                  {
                    if (boundingBox.Intersects((BoundingSphere) boundingSphereD2))
                    {
                      if (boundingBox.Intersects((BoundingSphere) boundingSphereD1))
                        blocks1.Add(cubeBlock);
                      else
                        blocks2.Add(cubeBlock);
                    }
                    else
                      blocks3.Add(cubeBlock);
                  }
                }
              }
            }
          }
        }
      }
      else
      {
        foreach (MyCube myCube in (IEnumerable<MyCube>) this.m_cubes.Values)
        {
          MySlimBlock cubeBlock = myCube.CubeBlock;
          if (cubeBlock.FatBlock == null || !this.m_processedBlocks.Contains(cubeBlock.FatBlock))
          {
            this.m_processedBlocks.Add(cubeBlock.FatBlock);
            if (respectDeformationRatio)
            {
              boundingSphereD1.Radius = sphere1.Radius * (double) cubeBlock.DeformationRatio;
              boundingSphereD2.Radius = sphere2.Radius * (double) cubeBlock.DeformationRatio;
              boundingSphereD3.Radius = sphere3.Radius * (double) cubeBlock.DeformationRatio;
            }
            BoundingBox boundingBox = cubeBlock.FatBlock == null ? new BoundingBox(cubeBlock.Position * this.GridSize - vector3, cubeBlock.Position * this.GridSize + vector3) : new BoundingBox(cubeBlock.Min * this.GridSize - this.GridSizeHalf, cubeBlock.Max * this.GridSize + this.GridSizeHalf);
            if (boundingBox.Intersects((BoundingSphere) boundingSphereD3))
            {
              if (boundingBox.Intersects((BoundingSphere) boundingSphereD2))
              {
                if (boundingBox.Intersects((BoundingSphere) boundingSphereD1))
                  blocks1.Add(cubeBlock);
                else
                  blocks2.Add(cubeBlock);
              }
              else
                blocks3.Add(cubeBlock);
            }
          }
        }
      }
      this.m_processedBlocks.Clear();
    }

    internal HashSet<MyCube> RayCastBlocksAll(Vector3D worldStart, Vector3D worldEnd)
    {
      this.RayCastCells(worldStart, worldEnd, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), false, true);
      HashSet<MyCube> myCubeSet = new HashSet<MyCube>();
      foreach (Vector3I cacheRayCastCell in MyCubeGrid.m_cacheRayCastCells)
      {
        if (this.m_cubes.ContainsKey(cacheRayCastCell))
          myCubeSet.Add(this.m_cubes[cacheRayCastCell]);
      }
      return myCubeSet;
    }

    internal List<MyCube> RayCastBlocksAllOrdered(Vector3D worldStart, Vector3D worldEnd)
    {
      this.RayCastCells(worldStart, worldEnd, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), false, true);
      List<MyCube> myCubeList = new List<MyCube>();
      foreach (Vector3I cacheRayCastCell in MyCubeGrid.m_cacheRayCastCells)
      {
        if (this.m_cubes.ContainsKey(cacheRayCastCell) && !myCubeList.Contains(this.m_cubes[cacheRayCastCell]))
          myCubeList.Add(this.m_cubes[cacheRayCastCell]);
      }
      return myCubeList;
    }

    public Vector3I? RayCastBlocks(Vector3D worldStart, Vector3D worldEnd)
    {
      this.RayCastCells(worldStart, worldEnd, MyCubeGrid.m_cacheRayCastCells, new Vector3I?(), false, true);
      foreach (Vector3I cacheRayCastCell in MyCubeGrid.m_cacheRayCastCells)
      {
        if (this.m_cubes.ContainsKey(cacheRayCastCell))
          return new Vector3I?(cacheRayCastCell);
      }
      return new Vector3I?();
    }

    public void RayCastCells(
      Vector3D worldStart,
      Vector3D worldEnd,
      List<Vector3I> outHitPositions,
      Vector3I? gridSizeInflate = null,
      bool havokWorld = false,
      bool clearOutHitPositions = true)
    {
      MatrixD matrix = this.PositionComp.WorldMatrixNormalizedInv;
      Vector3D result1;
      Vector3D.Transform(ref worldStart, ref matrix, out result1);
      Vector3D result2;
      Vector3D.Transform(ref worldEnd, ref matrix, out result2);
      Vector3 gridSizeHalfVector = this.GridSizeHalfVector;
      Vector3D lineStart = result1 + gridSizeHalfVector;
      Vector3D lineEnd = result2 + gridSizeHalfVector;
      Vector3I min = this.Min - Vector3I.One;
      Vector3I max = this.Max + Vector3I.One;
      if (gridSizeInflate.HasValue)
      {
        min -= gridSizeInflate.Value;
        max += gridSizeInflate.Value;
      }
      if (clearOutHitPositions)
        outHitPositions.Clear();
      MyGridIntersection.Calculate(outHitPositions, this.GridSize, lineStart, lineEnd, min, max);
    }

    public static void RayCastStaticCells(
      Vector3D worldStart,
      Vector3D worldEnd,
      List<Vector3I> outHitPositions,
      float gridSize,
      Vector3I? gridSizeInflate = null,
      bool havokWorld = false)
    {
      Vector3D vector3D1 = worldStart;
      Vector3D vector3D2 = worldEnd;
      Vector3D vector3D3 = new Vector3D((double) gridSize * 0.5);
      Vector3D lineStart = vector3D1 + vector3D3;
      Vector3D lineEnd = vector3D2 + vector3D3;
      Vector3I min = -Vector3I.One;
      Vector3I one = Vector3I.One;
      if (gridSizeInflate.HasValue)
      {
        min -= gridSizeInflate.Value;
        one += gridSizeInflate.Value;
      }
      outHitPositions.Clear();
      if (havokWorld)
        MyGridIntersection.CalculateHavok(outHitPositions, gridSize, lineStart, lineEnd, min, one);
      else
        MyGridIntersection.Calculate(outHitPositions, gridSize, lineStart, lineEnd, min, one);
    }

    void IMyGridConnectivityTest.GetConnectedBlocks(
      Vector3I minI,
      Vector3I maxI,
      Dictionary<Vector3I, ConnectivityResult> outOverlappedCubeBlocks)
    {
      Vector3I pos = new Vector3I();
      for (pos.Z = minI.Z; pos.Z <= maxI.Z; ++pos.Z)
      {
        for (pos.Y = minI.Y; pos.Y <= maxI.Y; ++pos.Y)
        {
          for (pos.X = minI.X; pos.X <= maxI.X; ++pos.X)
          {
            MySlimBlock cubeBlock = this.GetCubeBlock(pos);
            if (cubeBlock != null)
              outOverlappedCubeBlocks[cubeBlock.Position] = new ConnectivityResult()
              {
                Definition = cubeBlock.BlockDefinition,
                FatBlock = cubeBlock.FatBlock,
                Orientation = cubeBlock.Orientation,
                Position = cubeBlock.Position
              };
          }
        }
      }
    }

    private string MakeCustomName()
    {
      StringBuilder stringBuilder = new StringBuilder();
      long num = MyMath.Mod(this.EntityId, 10000);
      string str = (string) null;
      if (this.IsStatic)
      {
        str = MyTexts.GetString(MyCommonTexts.DetailStaticGrid);
      }
      else
      {
        switch (this.GridSizeEnum)
        {
          case MyCubeSize.Large:
            str = MyTexts.GetString(MyCommonTexts.DetailLargeGrid);
            break;
          case MyCubeSize.Small:
            str = MyTexts.GetString(MyCommonTexts.DetailSmallGrid);
            break;
        }
      }
      stringBuilder.Append(str ?? "Grid").Append(" ").Append(num.ToString());
      return stringBuilder.ToString();
    }

    public void ChangeOwner(MyCubeBlock block, long oldOwner, long newOwner)
    {
      if (!MyFakes.ENABLE_TERMINAL_PROPERTIES)
        return;
      this.m_ownershipManager.ChangeBlockOwnership(block, oldOwner, newOwner);
    }

    public void RecalculateOwners()
    {
      if (!MyFakes.ENABLE_TERMINAL_PROPERTIES)
        return;
      this.m_ownershipManager.RecalculateOwnersThreadSafe();
    }

    public void UpdateOwnership(long ownerId, bool isFunctional)
    {
      if (!MyFakes.ENABLE_TERMINAL_PROPERTIES)
        return;
      this.m_ownershipManager.UpdateOnFunctionalChange(ownerId, isFunctional);
    }

    public override void Teleport(MatrixD worldMatrix, object source = null, bool ignoreAssert = false)
    {
      Dictionary<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> source1 = new Dictionary<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>();
      Dictionary<MyCubeGrid, Tuple<Vector3, Vector3>> dictionary = new Dictionary<MyCubeGrid, Tuple<Vector3, Vector3>>();
      HashSet<VRage.ModAPI.IMyEntity> myEntitySet = new HashSet<VRage.ModAPI.IMyEntity>();
      MyHashSetDictionary<MyCubeGrid, VRage.ModAPI.IMyEntity> hashSetDictionary = new MyHashSetDictionary<MyCubeGrid, VRage.ModAPI.IMyEntity>();
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes)
      {
        HashSet<VRage.ModAPI.IMyEntity> result = new HashSet<VRage.ModAPI.IMyEntity>();
        node.NodeData.Hierarchy.GetChildrenRecursive(result);
        foreach (VRage.ModAPI.IMyEntity myEntity in result)
        {
          if (myEntity is MyCubeBlock myCubeBlock)
            myCubeBlock.OnTeleport();
        }
      }
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
      {
        HashSet<VRage.ModAPI.IMyEntity> result = new HashSet<VRage.ModAPI.IMyEntity>();
        result.Add((VRage.ModAPI.IMyEntity) node.NodeData);
        node.NodeData.Hierarchy.GetChildrenRecursive(result);
        foreach (VRage.ModAPI.IMyEntity myEntity1 in result)
        {
          if (myEntity1.Physics != null)
          {
            foreach (HkConstraint constraint in ((MyPhysicsBody) myEntity1.Physics).Constraints)
            {
              VRage.ModAPI.IMyEntity entity1 = constraint.RigidBodyA.GetEntity(0U);
              VRage.ModAPI.IMyEntity entity2 = constraint.RigidBodyB.GetEntity(0U);
              VRage.ModAPI.IMyEntity myEntity2 = myEntity1 == entity1 ? entity2 : entity1;
              if (!result.Contains(myEntity2) && myEntity2 != null)
                hashSetDictionary.Add(node.NodeData, myEntity2);
            }
          }
        }
        source1.Add(node.NodeData, result);
      }
      foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> keyValuePair1 in source1)
      {
        foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> keyValuePair2 in source1)
        {
          HashSet<VRage.ModAPI.IMyEntity> list;
          if (hashSetDictionary.TryGet(keyValuePair1.Key, out list))
          {
            list.Remove((VRage.ModAPI.IMyEntity) keyValuePair2.Key);
            if (list.Count == 0)
              hashSetDictionary.Remove(keyValuePair1.Key);
          }
        }
      }
      foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> keyValuePair in source1.Reverse<KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>>())
      {
        if (keyValuePair.Key.Physics != null)
        {
          dictionary[keyValuePair.Key] = new Tuple<Vector3, Vector3>(keyValuePair.Key.Physics.LinearVelocity, keyValuePair.Key.Physics.AngularVelocity);
          foreach (VRage.ModAPI.IMyEntity myEntity in keyValuePair.Value.Reverse<VRage.ModAPI.IMyEntity>())
          {
            if (myEntity.Physics != null && myEntity.Physics is MyPhysicsBody && !((MyPhysicsBody) myEntity.Physics).IsWelded)
            {
              if (myEntity.Physics.Enabled)
                myEntity.Physics.Enabled = false;
              else
                myEntitySet.Add(myEntity);
            }
          }
        }
      }
      Vector3D vector3D = worldMatrix.Translation - this.PositionComp.GetPosition();
      foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> keyValuePair in source1)
      {
        MatrixD worldMatrix1 = keyValuePair.Key.PositionComp.WorldMatrixRef;
        worldMatrix1.Translation += vector3D;
        keyValuePair.Key.PositionComp.SetWorldMatrix(ref worldMatrix1, source, skipTeleportCheck: true);
        HashSet<VRage.ModAPI.IMyEntity> list;
        if (hashSetDictionary.TryGet(keyValuePair.Key, out list))
        {
          foreach (VRage.ModAPI.IMyEntity myEntity in list)
          {
            MatrixD worldMatrix2 = myEntity.PositionComp.WorldMatrixRef;
            worldMatrix2.Translation += vector3D;
            myEntity.PositionComp.SetWorldMatrix(ref worldMatrix2, source, skipTeleportCheck: true);
          }
        }
      }
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
        invalid.Include(node.NodeData.PositionComp.WorldAABB);
      MyPhysics.EnsurePhysicsSpace(invalid.GetInflated(MyClusterTree.MinimumDistanceFromBorder));
      HkWorld hkWorld = (HkWorld) null;
      foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> keyValuePair in source1)
      {
        if (keyValuePair.Key.Physics != null)
        {
          foreach (VRage.ModAPI.IMyEntity myEntity in keyValuePair.Value)
          {
            if (myEntity.Physics != null && !((MyPhysicsBody) myEntity.Physics).IsWelded && !myEntitySet.Contains(myEntity))
            {
              myEntity.Physics.LinearVelocity = dictionary[keyValuePair.Key].Item1;
              myEntity.Physics.AngularVelocity = dictionary[keyValuePair.Key].Item2;
              ((MyPhysicsBody) myEntity.Physics).EnableBatched();
              if (hkWorld == null)
                hkWorld = ((MyPhysicsBody) myEntity.Physics).HavokWorld;
            }
          }
        }
      }
      hkWorld?.FinishBatch();
      foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> keyValuePair in source1.Reverse<KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>>())
      {
        if (keyValuePair.Key.Physics != null)
        {
          foreach (VRage.ModAPI.IMyEntity myEntity in keyValuePair.Value.Reverse<VRage.ModAPI.IMyEntity>())
          {
            if (myEntity.Physics != null && myEntity.Physics is MyPhysicsBody && (!((MyPhysicsBody) myEntity.Physics).IsWelded && !myEntitySet.Contains(myEntity)))
              ((MyPhysicsBody) myEntity.Physics).FinishAddBatch();
          }
        }
      }
    }

    public bool CanBeTeleported(
      MyGridJumpDriveSystem jumpingSystem,
      out MyGridJumpDriveSystem.MyJumpFailReason reason)
    {
      reason = MyGridJumpDriveSystem.MyJumpFailReason.None;
      if (MyFixedGrids.IsRooted(this))
      {
        reason = MyGridJumpDriveSystem.MyJumpFailReason.Static;
        return false;
      }
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes)
      {
        if (node.NodeData.Physics != null)
        {
          if (node.NodeData.IsStatic)
          {
            reason = MyGridJumpDriveSystem.MyJumpFailReason.Locked;
            return false;
          }
          if (MyFixedGrids.IsRooted(node.NodeData))
          {
            reason = MyGridJumpDriveSystem.MyJumpFailReason.Static;
            return false;
          }
          if (node.NodeData.GridSystems.JumpSystem.IsJumping && node.NodeData.GridSystems.JumpSystem != jumpingSystem)
          {
            reason = MyGridJumpDriveSystem.MyJumpFailReason.AlreadyJumping;
            return false;
          }
        }
      }
      return true;
    }

    public BoundingBoxD GetPhysicalGroupAABB()
    {
      if (this.MarkedForClose)
        return BoundingBoxD.CreateInvalid();
      BoundingBoxD worldAabb = this.PositionComp.WorldAABB;
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
      if (group == null)
        return worldAabb;
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
      {
        if (node.NodeData.PositionComp != null)
          worldAabb.Include(node.NodeData.PositionComp.WorldAABB);
      }
      return worldAabb;
    }

    public MyFracturedBlock CreateFracturedBlock(
      MyObjectBuilder_FracturedBlock fracturedBlockBuilder,
      Vector3I position)
    {
      MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FracturedBlock), "FracturedBlockLarge"));
      MyCube myCube;
      if (this.m_cubes.TryGetValue(position, out myCube))
        this.RemoveBlockInternal(myCube.CubeBlock, true);
      fracturedBlockBuilder.CreatingFracturedBlock = true;
      MySlimBlock mySlimBlock = this.AddBlock((MyObjectBuilder_CubeBlock) fracturedBlockBuilder, false);
      if (mySlimBlock == null)
        return (MyFracturedBlock) null;
      MyFracturedBlock fatBlock = mySlimBlock.FatBlock as MyFracturedBlock;
      ((MyEntity) fatBlock).Render.UpdateRenderObject(true);
      this.UpdateBlockNeighbours(fatBlock.SlimBlock);
      return fatBlock;
    }

    private MyFracturedBlock CreateFracturedBlock(MyFracturedBlock.Info info)
    {
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FracturedBlock), "FracturedBlockLarge"));
      Vector3I position = info.Position;
      MyCube myCube;
      if (this.m_cubes.TryGetValue(position, out myCube))
        this.RemoveBlock(myCube.CubeBlock, false);
      Vector3I min = position;
      MyBlockOrientation orientation = new MyBlockOrientation(ref Quaternion.Identity);
      MyObjectBuilder_CubeBlock blockObjectBuilder = MyCubeGrid.CreateBlockObjectBuilder(cubeBlockDefinition, min, orientation, 0L, 0L, true);
      blockObjectBuilder.ColorMaskHSV = (SerializableVector3) Vector3.Zero;
      (blockObjectBuilder as MyObjectBuilder_FracturedBlock).CreatingFracturedBlock = true;
      MySlimBlock mySlimBlock = this.AddBlock(blockObjectBuilder, false);
      if (mySlimBlock == null)
      {
        info.Shape.RemoveReference();
        return (MyFracturedBlock) null;
      }
      MyFracturedBlock fatBlock = mySlimBlock.FatBlock as MyFracturedBlock;
      fatBlock.OriginalBlocks = info.OriginalBlocks;
      fatBlock.Orientations = info.Orientations;
      fatBlock.MultiBlocks = info.MultiBlocks;
      fatBlock.SetDataFromHavok(info.Shape, info.Compound);
      ((MyEntity) fatBlock).Render.UpdateRenderObject(true);
      this.UpdateBlockNeighbours(fatBlock.SlimBlock);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        MySyncDestructions.CreateFracturedBlock((MyObjectBuilder_FracturedBlock) fatBlock.GetObjectBuilderCubeBlock(false), this.EntityId, position);
      return fatBlock;
    }

    public void OnIntegrityChanged(MySlimBlock block, bool handWelded) => this.NotifyBlockIntegrityChanged(block, handWelded);

    public void PasteBlocksToGrid(
      List<MyObjectBuilder_CubeGrid> gridsToMerge,
      long inventoryEntityId,
      bool multiBlock,
      bool instantBuild)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<MyObjectBuilder_CubeGrid>, long, bool, bool>(this, (Func<MyCubeGrid, Action<List<MyObjectBuilder_CubeGrid>, long, bool, bool>>) (x => new Action<List<MyObjectBuilder_CubeGrid>, long, bool, bool>(x.PasteBlocksToGridServer_Implementation)), gridsToMerge, inventoryEntityId, multiBlock, instantBuild);
    }

    [Event(null, 9453)]
    [Reliable]
    [Server]
    private void PasteBlocksToGridServer_Implementation(
      List<MyObjectBuilder_CubeGrid> gridsToMerge,
      long inventoryEntityId,
      bool multiBlock,
      bool instantBuild)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        int num1 = MyEventContext.Current.IsLocallyInvoked ? 1 : (MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) ? 1 : 0);
        MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) gridsToMerge);
        MatrixI matrixI = this.PasteBlocksServer(gridsToMerge);
        int num2 = instantBuild ? 1 : 0;
        MyEntity entity;
        if ((num1 & num2) == 0 && MyEntities.TryGetEntityById(inventoryEntityId, out entity) && entity != null)
        {
          MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(entity);
          if (builderInventory != null)
          {
            if (multiBlock)
            {
              MyMultiBlockClipboard.TakeMaterialsFromBuilder(gridsToMerge, entity);
            }
            else
            {
              MyGridClipboard.CalculateItemRequirements(gridsToMerge, MyCubeGrid.m_buildComponents);
              foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in MyCubeGrid.m_buildComponents.TotalMaterials)
                builderInventory.RemoveItemsOfType((MyFixedPoint) totalMaterial.Value, totalMaterial.Key);
            }
          }
        }
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyObjectBuilder_CubeGrid, MatrixI>(this, (Func<MyCubeGrid, Action<MyObjectBuilder_CubeGrid, MatrixI>>) (x => new Action<MyObjectBuilder_CubeGrid, MatrixI>(x.PasteBlocksToGridClient_Implementation)), gridsToMerge[0], matrixI);
        Sandbox.Engine.Multiplayer.MyMultiplayer.GetReplicationServer()?.ResendMissingReplicableChildren((IMyEventProxy) this);
      }
    }

    [Event(null, 9497)]
    [Reliable]
    [Broadcast]
    private void PasteBlocksToGridClient_Implementation(
      MyObjectBuilder_CubeGrid gridToMerge,
      MatrixI mergeTransform)
    {
      this.PasteBlocksClient(gridToMerge, mergeTransform);
    }

    private void PasteBlocksClient(MyObjectBuilder_CubeGrid gridToMerge, MatrixI mergeTransform)
    {
      if (!(MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) gridToMerge, false) is MyCubeGrid fromObjectBuilder))
        return;
      MyEntities.Add((MyEntity) fromObjectBuilder);
      this.MergeGridInternal(fromObjectBuilder, ref mergeTransform);
    }

    private MatrixI PasteBlocksServer(List<MyObjectBuilder_CubeGrid> gridsToMerge)
    {
      MyCubeGrid gridToMerge = (MyCubeGrid) null;
      foreach (MyObjectBuilder_EntityBase objectBuilder in gridsToMerge)
      {
        if (MyEntities.CreateFromObjectBuilder(objectBuilder, false) is MyCubeGrid fromObjectBuilder)
        {
          if (gridToMerge == null)
            gridToMerge = fromObjectBuilder;
          MyEntities.Add((MyEntity) fromObjectBuilder);
        }
      }
      MatrixI mergeTransform = this.CalculateMergeTransform(gridToMerge, this.WorldToGridInteger(gridToMerge.PositionComp.GetPosition()));
      this.MergeGridInternal(gridToMerge, ref mergeTransform, false);
      return mergeTransform;
    }

    public static bool CanPasteGrid() => MySession.Static.IsCopyPastingEnabled;

    public MyCubeGrid GetBiggestGridInGroup()
    {
      MyCubeGrid myCubeGrid = this;
      double num = 0.0;
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes)
      {
        double volume = node.NodeData.PositionComp.WorldAABB.Size.Volume;
        if (volume > num)
        {
          num = volume;
          myCubeGrid = node.NodeData;
        }
      }
      return myCubeGrid;
    }

    public void ConvertFracturedBlocksToComponents()
    {
      List<MyFracturedBlock> myFracturedBlockList = new List<MyFracturedBlock>();
      foreach (MySlimBlock cubeBlock in this.m_cubeBlocks)
      {
        if (cubeBlock.FatBlock is MyFracturedBlock fatBlock)
          myFracturedBlockList.Add(fatBlock);
      }
      foreach (MyFracturedBlock myFracturedBlock in myFracturedBlockList)
      {
        MyObjectBuilder_CubeBlock fractureComponent = myFracturedBlock.ConvertToOriginalBlocksWithFractureComponent();
        this.RemoveBlockInternal(myFracturedBlock.SlimBlock, true, false);
        if (fractureComponent != null)
          this.AddBlock(fractureComponent, false);
      }
    }

    public void PrepareMultiBlockInfos()
    {
      foreach (MySlimBlock block in this.GetBlocks())
        this.AddMultiBlockInfo(block);
    }

    internal void AddMultiBlockInfo(MySlimBlock block)
    {
      if (block.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        foreach (MySlimBlock block1 in fatBlock.GetBlocks())
        {
          if (block1.IsMultiBlockPart)
            this.AddMultiBlockInfo(block1);
        }
      }
      else
      {
        if (!block.IsMultiBlockPart)
          return;
        if (this.m_multiBlockInfos == null)
          this.m_multiBlockInfos = new Dictionary<int, MyCubeGridMultiBlockInfo>();
        MyCubeGridMultiBlockInfo gridMultiBlockInfo;
        if (!this.m_multiBlockInfos.TryGetValue(block.MultiBlockId, out gridMultiBlockInfo))
        {
          gridMultiBlockInfo = new MyCubeGridMultiBlockInfo();
          gridMultiBlockInfo.MultiBlockId = block.MultiBlockId;
          gridMultiBlockInfo.MultiBlockDefinition = block.MultiBlockDefinition;
          gridMultiBlockInfo.MainBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinitionForMultiBlock(block.MultiBlockDefinition.Id.SubtypeName);
          this.m_multiBlockInfos.Add(block.MultiBlockId, gridMultiBlockInfo);
        }
        gridMultiBlockInfo.Blocks.Add(block);
      }
    }

    internal void RemoveMultiBlockInfo(MySlimBlock block)
    {
      if (this.m_multiBlockInfos == null)
        return;
      if (block.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        foreach (MySlimBlock block1 in fatBlock.GetBlocks())
        {
          if (block1.IsMultiBlockPart)
            this.RemoveMultiBlockInfo(block1);
        }
      }
      else
      {
        MyCubeGridMultiBlockInfo gridMultiBlockInfo;
        if (!block.IsMultiBlockPart || !this.m_multiBlockInfos.TryGetValue(block.MultiBlockId, out gridMultiBlockInfo) || (!gridMultiBlockInfo.Blocks.Remove(block) || gridMultiBlockInfo.Blocks.Count != 0) || (!this.m_multiBlockInfos.Remove(block.MultiBlockId) || this.m_multiBlockInfos.Count != 0))
          return;
        this.m_multiBlockInfos = (Dictionary<int, MyCubeGridMultiBlockInfo>) null;
      }
    }

    public MyCubeGridMultiBlockInfo GetMultiBlockInfo(int multiBlockId)
    {
      MyCubeGridMultiBlockInfo gridMultiBlockInfo;
      return this.m_multiBlockInfos != null && this.m_multiBlockInfos.TryGetValue(multiBlockId, out gridMultiBlockInfo) ? gridMultiBlockInfo : (MyCubeGridMultiBlockInfo) null;
    }

    public void GetBlocksInMultiBlock(
      int multiBlockId,
      HashSet<Tuple<MySlimBlock, ushort?>> outMultiBlocks)
    {
      if (multiBlockId == 0)
        return;
      MyCubeGridMultiBlockInfo multiBlockInfo = this.GetMultiBlockInfo(multiBlockId);
      if (multiBlockInfo == null)
        return;
      foreach (MySlimBlock block in multiBlockInfo.Blocks)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(block.Position);
        if (cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock)
        {
          ushort? blockId = fatBlock.GetBlockId(block);
          outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, blockId));
        }
        else
          outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, new ushort?()));
      }
    }

    public bool CanAddMultiBlocks(
      MyCubeGridMultiBlockInfo multiBlockInfo,
      ref MatrixI transform,
      List<int> multiBlockIndices)
    {
      foreach (int multiBlockIndex in multiBlockIndices)
      {
        if (multiBlockIndex < multiBlockInfo.MultiBlockDefinition.BlockDefinitions.Length)
        {
          MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition1 = multiBlockInfo.MultiBlockDefinition.BlockDefinitions[multiBlockIndex];
          MyCubeBlockDefinition blockDefinition2;
          if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefinition1.Id, out blockDefinition2) || blockDefinition2 == null)
            return false;
          Vector3I vector3I = Vector3I.Transform(blockDefinition1.Min, ref transform);
          MatrixI leftMatrix = new MatrixI(blockDefinition1.Forward, blockDefinition1.Up);
          MatrixI result;
          MatrixI.Multiply(ref leftMatrix, ref transform, out result);
          MyBlockOrientation blockOrientation = result.GetBlockOrientation();
          if (!this.CanPlaceBlock(vector3I, vector3I, blockOrientation, blockDefinition2, ignoreMultiblockId: new int?(multiBlockInfo.MultiBlockId), ignoreFracturedPieces: true))
            return false;
        }
      }
      return true;
    }

    public bool BuildMultiBlocks(
      MyCubeGridMultiBlockInfo multiBlockInfo,
      ref MatrixI transform,
      List<int> multiBlockIndices,
      long builderEntityId,
      MyStringHash skinId)
    {
      List<MyCubeGrid.MyBlockLocation> myBlockLocationList = new List<MyCubeGrid.MyBlockLocation>();
      List<MyObjectBuilder_CubeBlock> builderCubeBlockList = new List<MyObjectBuilder_CubeBlock>();
      foreach (int multiBlockIndex in multiBlockIndices)
      {
        if (multiBlockIndex < multiBlockInfo.MultiBlockDefinition.BlockDefinitions.Length)
        {
          MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition1 = multiBlockInfo.MultiBlockDefinition.BlockDefinitions[multiBlockIndex];
          MyCubeBlockDefinition blockDefinition2;
          if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefinition1.Id, out blockDefinition2) || blockDefinition2 == null)
            return false;
          Vector3I vector3I = Vector3I.Transform(blockDefinition1.Min, ref transform);
          MatrixI leftMatrix = new MatrixI(blockDefinition1.Forward, blockDefinition1.Up);
          MatrixI result;
          MatrixI.Multiply(ref leftMatrix, ref transform, out result);
          MyBlockOrientation blockOrientation = result.GetBlockOrientation();
          if (!this.CanPlaceBlock(vector3I, vector3I, blockOrientation, blockDefinition2, ignoreMultiblockId: new int?(multiBlockInfo.MultiBlockId)))
            return false;
          MyObjectBuilder_CubeBlock newObject = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) blockDefinition1.Id) as MyObjectBuilder_CubeBlock;
          newObject.Orientation = (SerializableQuaternion) Base6Directions.GetOrientation(blockOrientation.Forward, blockOrientation.Up);
          newObject.Min = (SerializableVector3I) vector3I;
          newObject.ColorMaskHSV = (SerializableVector3) MyPlayer.SelectedColor;
          newObject.SkinSubtypeId = MyPlayer.SelectedArmorSkin;
          newObject.MultiBlockId = multiBlockInfo.MultiBlockId;
          newObject.MultiBlockIndex = multiBlockIndex;
          newObject.MultiBlockDefinition = new SerializableDefinitionId?((SerializableDefinitionId) multiBlockInfo.MultiBlockDefinition.Id);
          builderCubeBlockList.Add(newObject);
          myBlockLocationList.Add(new MyCubeGrid.MyBlockLocation()
          {
            Min = vector3I,
            Max = vector3I,
            CenterPos = vector3I,
            Orientation = new MyBlockOrientation(blockOrientation.Forward, blockOrientation.Up),
            BlockDefinition = (DefinitionIdBlit) blockDefinition1.Id,
            EntityId = MyEntityIdentifier.AllocateId(),
            Owner = builderEntityId
          });
        }
      }
      if (MySession.Static.SurvivalMode)
      {
        MyEntity entityById = MyEntities.GetEntityById(builderEntityId);
        if (entityById == null)
          return false;
        MyCubeBuilder.BuildComponent.GetBlocksPlacementMaterials(new HashSet<MyCubeGrid.MyBlockLocation>((IEnumerable<MyCubeGrid.MyBlockLocation>) myBlockLocationList), this);
        if (!MyCubeBuilder.BuildComponent.HasBuildingMaterials(entityById))
          return false;
      }
      MyCubeGrid.MyBlockVisuals myBlockVisuals = new MyCubeGrid.MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), skinId);
      for (int index = 0; index < myBlockLocationList.Count && index < builderCubeBlockList.Count; ++index)
      {
        MyCubeGrid.MyBlockLocation myBlockLocation = myBlockLocationList[index];
        MyObjectBuilder_CubeBlock builderCubeBlock = builderCubeBlockList[index];
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>>) (x => new Action<MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>(x.BuildBlockRequest)), myBlockVisuals, myBlockLocation, builderCubeBlock, builderEntityId, false, MySession.Static.LocalPlayerId);
      }
      return true;
    }

    private bool GetMissingBlocksMultiBlock(
      int multiblockId,
      out MyCubeGridMultiBlockInfo multiBlockInfo,
      out MatrixI transform,
      List<int> multiBlockIndices)
    {
      transform = new MatrixI();
      multiBlockInfo = this.GetMultiBlockInfo(multiblockId);
      return multiBlockInfo != null && multiBlockInfo.GetMissingBlocks(out transform, multiBlockIndices);
    }

    public bool CanAddMissingBlocksInMultiBlock(int multiBlockId)
    {
      try
      {
        MyCubeGridMultiBlockInfo multiBlockInfo;
        MatrixI transform;
        return this.GetMissingBlocksMultiBlock(multiBlockId, out multiBlockInfo, out transform, MyCubeGrid.m_tmpMultiBlockIndices) && this.CanAddMultiBlocks(multiBlockInfo, ref transform, MyCubeGrid.m_tmpMultiBlockIndices);
      }
      finally
      {
        MyCubeGrid.m_tmpMultiBlockIndices.Clear();
      }
    }

    public void AddMissingBlocksInMultiBlock(int multiBlockId, long toolOwnerId)
    {
      try
      {
        MyCubeGridMultiBlockInfo multiBlockInfo;
        MatrixI transform;
        if (!this.GetMissingBlocksMultiBlock(multiBlockId, out multiBlockInfo, out transform, MyCubeGrid.m_tmpMultiBlockIndices))
          return;
        MyStringHash orCompute = MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin);
        this.BuildMultiBlocks(multiBlockInfo, ref transform, MyCubeGrid.m_tmpMultiBlockIndices, toolOwnerId, orCompute);
      }
      finally
      {
        MyCubeGrid.m_tmpMultiBlockIndices.Clear();
      }
    }

    public bool CanAddOtherBlockInMultiBlock(
      Vector3I min,
      Vector3I max,
      MyBlockOrientation orientation,
      MyCubeBlockDefinition definition,
      int? ignoreMultiblockId)
    {
      if (this.m_multiBlockInfos == null)
        return true;
      foreach (KeyValuePair<int, MyCubeGridMultiBlockInfo> multiBlockInfo in this.m_multiBlockInfos)
      {
        if ((!ignoreMultiblockId.HasValue || ignoreMultiblockId.Value != multiBlockInfo.Key) && !multiBlockInfo.Value.CanAddBlock(ref min, ref max, orientation, definition))
          return false;
      }
      return true;
    }

    public static bool IsGridInCompleteState(MyCubeGrid grid)
    {
      foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
      {
        if (!cubeBlock.IsFullIntegrity || (double) cubeBlock.BuildLevelRatio != 1.0)
          return false;
      }
      return true;
    }

    public bool WillRemoveBlockSplitGrid(MySlimBlock testBlock) => this.m_disconnectHelper.TryDisconnect(testBlock);

    public MySlimBlock GetTargetedBlock(Vector3D position)
    {
      Vector3I cube;
      this.FixTargetCube(out cube, (Vector3) (Vector3D.Transform(position, this.PositionComp.WorldMatrixNormalizedInv) * (double) this.GridSizeR));
      return this.GetCubeBlock(cube);
    }

    public MySlimBlock GetTargetedBlockLite(Vector3D position)
    {
      Vector3I cube;
      this.FixTargetCubeLite(out cube, Vector3D.Transform(position, this.PositionComp.WorldMatrixNormalizedInv) * (double) this.GridSizeR);
      return this.GetCubeBlock(cube);
    }

    [Event(null, 9920)]
    [Reliable]
    [Server]
    public static void TryCreateGrid_Implementation(
      MyCubeSize cubeSize,
      bool isStatic,
      MyPositionAndOrientation position,
      long inventoryEntityId,
      bool instantBuild)
    {
      bool flag1 = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
      string prefabName;
      MyDefinitionManager.Static.GetBaseBlockPrefabName(cubeSize, isStatic, MySession.Static.CreativeMode || instantBuild & flag1, out prefabName);
      if (prefabName == null)
        return;
      MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(prefabName);
      if (gridPrefab == null || gridPrefab.Length == 0)
        return;
      foreach (MyObjectBuilder_EntityBase builderEntityBase in gridPrefab)
        builderEntityBase.PositionAndOrientation = new MyPositionAndOrientation?(position);
      MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) gridPrefab);
      if (!(instantBuild & flag1))
      {
        MyEntity entity;
        if (MyEntities.TryGetEntityById(inventoryEntityId, out entity) && entity != null)
        {
          MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(entity);
          if (builderInventory != null)
          {
            MyGridClipboard.CalculateItemRequirements(gridPrefab, MyCubeGrid.m_buildComponents);
            foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in MyCubeGrid.m_buildComponents.TotalMaterials)
              builderInventory.RemoveItemsOfType((MyFixedPoint) totalMaterial.Value, totalMaterial.Key);
          }
        }
        else if (!flag1 && !MySession.Static.CreativeMode)
        {
          (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
          return;
        }
      }
      List<MyCubeGrid> grids = new List<MyCubeGrid>();
      foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in gridPrefab)
      {
        MySandboxGame.Log.WriteLine("CreateCompressedMsg: Type: " + objectBuilderCubeGrid.GetType().Name.ToString() + "  Name: " + objectBuilderCubeGrid.Name + "  EntityID: " + objectBuilderCubeGrid.EntityId.ToString("X8"));
        if (MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) objectBuilderCubeGrid, false) is MyCubeGrid fromObjectBuilder)
        {
          grids.Add(fromObjectBuilder);
          if (instantBuild & flag1)
            MyCubeGrid.ChangeOwnership(inventoryEntityId, fromObjectBuilder);
          MyLog log = MySandboxGame.Log;
          string[] strArray = new string[5]
          {
            "Status: Exists(",
            null,
            null,
            null,
            null
          };
          bool flag2 = MyEntities.EntityExists(objectBuilderCubeGrid.EntityId);
          strArray[1] = flag2.ToString();
          strArray[2] = ") InScene(";
          flag2 = (objectBuilderCubeGrid.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene;
          strArray[3] = flag2.ToString();
          strArray[4] = ")";
          string msg = string.Concat(strArray);
          log.WriteLine(msg);
        }
      }
      MyCubeGrid.AfterPaste(grids, Vector3.Zero, false);
    }

    public void SendGridCloseRequest() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyCubeGrid.OnGridClosedRequest)), this.EntityId);

    [Event(null, 9996)]
    [Reliable]
    [Client]
    private static void StationClosingDenied()
    {
      StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.Economy_CantRemoveStation_Caption);
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.Economy_CantRemoveStation_Text), messageCaption: messageCaption, canHideOthers: false));
    }

    [Event(null, 10006)]
    [Reliable]
    [Server]
    private static void OnGridClosedRequest(long entityId)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component != null && component.IsGridStation(entityId))
      {
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyCubeGrid.StationClosingDenied)), MyEventContext.Current.Sender);
      }
      else
      {
        MyLog.Default.WriteLineAndConsole("Closing grid request by user: " + (object) MyEventContext.Current.Sender);
        if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
        {
          (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        }
        else
        {
          MyEntity entity;
          MyEntities.TryGetEntityById(entityId, out entity);
          if (entity == null)
            return;
          if (entity is MyCubeGrid myCubeGrid)
          {
            long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
            bool flag1 = false;
            bool flag2 = false;
            IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(identityId);
            if (playerFaction1 != null)
              flag2 = playerFaction1.IsLeader(identityId);
            if (MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
              flag1 = true;
            else if (myCubeGrid.BigOwners.Count != 0)
            {
              foreach (long bigOwner in myCubeGrid.BigOwners)
              {
                if (bigOwner == identityId)
                {
                  flag1 = true;
                  break;
                }
                if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
                {
                  IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
                  if (playerFaction2 != null && playerFaction1.FactionId == playerFaction2.FactionId)
                  {
                    flag1 = true;
                    break;
                  }
                }
              }
            }
            else
              flag1 = true;
            if (!flag1)
              return;
          }
          MyLog.Default.Info(string.Format("OnGridClosedRequest removed entity '{0}:{1}' with entity id '{2}'", (object) entity.Name, (object) entity.DisplayName, (object) entity.EntityId));
          if (entity.MarkedForClose)
            return;
          entity.Close();
        }
      }
    }

    [Event(null, 10332)]
    [Reliable]
    [Server]
    public static void TryPasteGrid_Implementation(MyCubeGrid.MyPasteGridParameters parameters)
    {
      MyLog.Default.WriteLineAndConsole("Pasting grid request by user: " + (object) MyEventContext.Current.Sender);
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsCopyPastingEnabledForUser(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        bool shouldRemoveScripts = !MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value);
        Vector3D? offset = new Vector3D?();
        if (parameters.Offset.Use && parameters.Offset.RelativeToEntity)
        {
          VRage.ModAPI.IMyEntity entity;
          if (!MyEntityIdentifier.TryGetEntity(parameters.Offset.SpawnerId, out entity))
            return;
          offset = new Vector3D?((entity as MyEntity).WorldMatrix.Translation - parameters.Offset.OriginalSpawnPoint);
        }
        MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) parameters.Entities);
        MyCubeGrid.CleanCubeGridsBeforePaste(parameters.Entities);
        MyCubeGrid.PasteGridData pasteGridData = new MyCubeGrid.PasteGridData(parameters.Entities, parameters.DetectDisconnects, parameters.ObjectVelocity, parameters.MultiBlock, parameters.InstantBuild, shouldRemoveScripts, MyEventContext.Current.Sender, MyEventContext.Current.IsLocallyInvoked, offset, parameters.ClientsideDLCs);
        if (MySandboxGame.Config.SyncRendering)
        {
          MyEntityIdentifier.PrepareSwapData();
          MyEntityIdentifier.SwapPerThreadData();
        }
        Parallel.Start(new Action<WorkData>(MyCubeGrid.TryPasteGrid_ImplementationInternal), new Action<WorkData>(MyCubeGrid.OnPasteCompleted), (WorkData) pasteGridData);
        if (!MySandboxGame.Config.SyncRendering)
          return;
        MyEntityIdentifier.ClearSwapDataAndRestore();
      }
    }

    internal static void CleanCubeGridsBeforePaste(List<MyObjectBuilder_CubeGrid> grids)
    {
      foreach (MyObjectBuilder_CubeGrid grid in grids)
      {
        grid.SetupForGridPaste();
        foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
          cubeBlock.SetupForGridPaste();
      }
    }

    internal static void CleanCubeGridsBeforeSetupForProjector(List<MyObjectBuilder_CubeGrid> grids)
    {
      foreach (MyObjectBuilder_CubeGrid grid in grids)
      {
        grid.SetupForProjector();
        foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
          cubeBlock.SetupForProjector();
      }
    }

    private static void TryPasteGrid_ImplementationInternal(WorkData workData)
    {
      if (!(workData is MyCubeGrid.PasteGridData pasteGridData))
        workData.FlagAsFailed();
      else
        pasteGridData.TryPasteGrid();
    }

    private static void OnPasteCompleted(WorkData workData)
    {
      if (!(workData is MyCubeGrid.PasteGridData pasteGridData))
        workData.FlagAsFailed();
      else
        pasteGridData.Callback();
    }

    [Event(null, 10423)]
    [Reliable]
    [Client]
    public static void ShowPasteFailedOperation() => MyHud.Notifications.Add(MyNotificationSingletons.PasteFailed);

    [Event(null, 10429)]
    [Reliable]
    [Client]
    public static void SendHudNotificationAfterPaste() => MyHud.PopRotatingWheelVisible();

    internal static void RelocateGrids(
      List<MyObjectBuilder_CubeGrid> cubegrids,
      MatrixD worldMatrix0)
    {
      MatrixD matrix1 = cubegrids[0].PositionAndOrientation.Value.GetMatrix();
      MatrixD matrixD = Matrix.Invert((Matrix) ref matrix1) * worldMatrix0.GetOrientation();
      Matrix matrix2 = (Matrix) ref matrixD;
      for (int index = 0; index < cubegrids.Count; ++index)
      {
        if (cubegrids[index].PositionAndOrientation.HasValue)
        {
          MatrixD matrix3 = cubegrids[index].PositionAndOrientation.Value.GetMatrix();
          Vector3 vector3 = Vector3.TransformNormal(matrix3.Translation - matrix1.Translation, matrix2);
          matrix3 *= matrix2;
          Vector3D vector3D = worldMatrix0.Translation + vector3;
          matrix3.Translation = Vector3D.Zero;
          matrix3 = MatrixD.Orthogonalize(matrix3);
          matrix3.Translation = vector3D;
          cubegrids[index].PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(ref matrix3));
        }
      }
    }

    private static void ChangeOwnership(long inventoryEntityId, MyCubeGrid grid)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(inventoryEntityId, out entity) || entity == null || !(entity is MyCharacter myCharacter))
        return;
      grid.ChangeGridOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId, MyOwnershipShareModeEnum.Faction);
    }

    private static void AfterPaste(
      List<MyCubeGrid> grids,
      Vector3 objectVelocity,
      bool detectDisconnects)
    {
      foreach (MyCubeGrid grid in grids)
      {
        if (grid.IsStatic)
          grid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridCopied;
        MyEntities.Add((MyEntity) grid);
        if (grid.Physics != null)
        {
          if (!grid.IsStatic)
            grid.Physics.LinearVelocity = objectVelocity;
          if (!grid.IsStatic && MySession.Static.ControlledEntity != null && (MySession.Static.ControlledEntity.Entity.Physics != null && MySession.Static.ControlledEntity != null))
            grid.Physics.AngularVelocity = MySession.Static.ControlledEntity.Entity.Physics.AngularVelocity;
        }
        if (detectDisconnects)
          grid.DetectDisconnectsAfterFrame();
        if (grid.IsStatic)
        {
          foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
          {
            if (grid.DetectMerge(cubeBlock, newGrid: true) != null)
              break;
          }
        }
        if (MyVisualScriptLogicProvider.GridSpawned != null)
          MyVisualScriptLogicProvider.GridSpawned(grid.Name);
      }
      MatrixD tranform = grids[0].PositionComp.WorldMatrixRef;
      bool flag = MyCoordinateSystem.Static.IsLocalCoordSysExist(ref tranform, (double) grids[0].GridSize);
      if (grids[0].GridSizeEnum != MyCubeSize.Large)
        return;
      if (flag)
        MyCoordinateSystem.Static.RegisterCubeGrid(grids[0]);
      else
        MyCoordinateSystem.Static.CreateCoordSys(grids[0], MyClipboardComponent.ClipboardDefinition.PastingSettings.StaticGridAlignToCenter, true);
    }

    public void RecalculateGravity() => this.m_gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.Physics == null || !((HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null) ? this.PositionComp.GetPosition() : this.Physics.CenterOfMassWorld);

    public void ActivatePhysics()
    {
      if (MyEntities.IsAsyncUpdateInProgress)
      {
        MyEntities.InvokeLater(new Action(this.ActivatePhysics));
      }
      else
      {
        if (this.Physics == null || !this.Physics.Enabled)
          return;
        this.Physics.RigidBody.Activate();
        if (!((HkReferenceObject) this.Physics.RigidBody2 != (HkReferenceObject) null))
          return;
        this.Physics.RigidBody2.Activate();
      }
    }

    [Event(null, 10570)]
    [Reliable]
    [Broadcast]
    private void OnBonesReceived(int segmentsCount, List<byte> boneByteList)
    {
      byte[] array = boneByteList.ToArray();
      int dataIndex = 0;
      for (int index = 0; index < segmentsCount; ++index)
      {
        Vector3I minBone;
        Vector3I maxBone;
        this.Skeleton.DeserializePart(this.GridSize, array, ref dataIndex, out minBone, out maxBone);
        Vector3I zero1 = Vector3I.Zero;
        Vector3I zero2 = Vector3I.Zero;
        this.Skeleton.Wrap(ref zero1, ref minBone);
        this.Skeleton.Wrap(ref zero2, ref maxBone);
        Vector3I vector3I = zero1 - Vector3I.One;
        zero2 += Vector3I.One;
        Vector3I pos;
        for (pos.X = vector3I.X; pos.X <= zero2.X; ++pos.X)
        {
          for (pos.Y = vector3I.Y; pos.Y <= zero2.Y; ++pos.Y)
          {
            for (pos.Z = vector3I.Z; pos.Z <= zero2.Z; ++pos.Z)
              this.SetCubeDirty(pos);
          }
        }
      }
    }

    [Event(null, 10604)]
    [Reliable]
    [Broadcast]
    private void OnBonesMultiplied(Vector3I blockLocation, float multiplier)
    {
      MySlimBlock cubeBlock = this.GetCubeBlock(blockLocation);
      if (cubeBlock == null)
        return;
      this.MultiplyBlockSkeleton(cubeBlock, multiplier);
    }

    public void SendReflectorState(MyMultipleEnabledEnum value) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyMultipleEnabledEnum>(this, (Func<MyCubeGrid, Action<MyMultipleEnabledEnum>>) (x => new Action<MyMultipleEnabledEnum>(x.RelfectorStateRecived)), value);

    [Event(null, 10620)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Controlled)]
    [Broadcast]
    private void RelfectorStateRecived(MyMultipleEnabledEnum value) => this.GridSystems.ReflectorLightSystem.ReflectorStateChanged(value);

    public void SendIntegrityChanged(
      MySlimBlock mySlimBlock,
      MyIntegrityChangeEnum integrityChangeType,
      long toolOwner)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, ushort, float, float, MyIntegrityChangeEnum, long>(this, (Func<MyCubeGrid, Action<Vector3I, ushort, float, float, MyIntegrityChangeEnum, long>>) (x => new Action<Vector3I, ushort, float, float, MyIntegrityChangeEnum, long>(x.BlockIntegrityChanged)), mySlimBlock.Position, this.GetSubBlockId(mySlimBlock), mySlimBlock.BuildIntegrity, mySlimBlock.Integrity, integrityChangeType, toolOwner);
    }

    public void SendStockpileChanged(MySlimBlock mySlimBlock, List<MyStockpileItem> list)
    {
      if (list.Count <= 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, ushort, List<MyStockpileItem>>(this, (Func<MyCubeGrid, Action<Vector3I, ushort, List<MyStockpileItem>>>) (x => new Action<Vector3I, ushort, List<MyStockpileItem>>(x.BlockStockpileChanged)), mySlimBlock.Position, this.GetSubBlockId(mySlimBlock), list);
    }

    public void SendFractureComponentRepaired(MySlimBlock mySlimBlock, long toolOwner) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, ushort, long>(this, (Func<MyCubeGrid, Action<Vector3I, ushort, long>>) (x => new Action<Vector3I, ushort, long>(x.FractureComponentRepaired)), mySlimBlock.Position, this.GetSubBlockId(mySlimBlock), toolOwner);

    private ushort GetSubBlockId(MySlimBlock slimBlock)
    {
      MySlimBlock cubeBlock = slimBlock.CubeGrid.GetCubeBlock(slimBlock.Position);
      MyCompoundCubeBlock compoundCubeBlock = (MyCompoundCubeBlock) null;
      if (cubeBlock != null)
        compoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
      return compoundCubeBlock != null ? compoundCubeBlock.GetBlockId(slimBlock) ?? (ushort) 0 : (ushort) 0;
    }

    public void RequestFillStockpile(Vector3I blockPosition, MyInventory fromInventory) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, long, byte>(this, (Func<MyCubeGrid, Action<Vector3I, long, byte>>) (x => new Action<Vector3I, long, byte>(x.OnStockpileFillRequest)), blockPosition, fromInventory.Owner.EntityId, fromInventory.InventoryIdx);

    [Event(null, 10664)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void OnStockpileFillRequest(
      Vector3I blockPosition,
      long ownerEntityId,
      byte inventoryIndex)
    {
      MySlimBlock cubeBlock = this.GetCubeBlock(blockPosition);
      if (cubeBlock == null)
        return;
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(ownerEntityId, out entity))
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(entity == null || !entity.HasInventory ? (MyEntity) null : entity, (int) inventoryIndex);
      cubeBlock.MoveItemsToConstructionStockpile((MyInventoryBase) inventory);
    }

    public void RequestSetToConstruction(Vector3I blockPosition, MyInventory fromInventory) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, long, byte, long>(this, (Func<MyCubeGrid, Action<Vector3I, long, byte, long>>) (x => new Action<Vector3I, long, byte, long>(x.OnSetToConstructionRequest)), blockPosition, fromInventory.Owner.EntityId, fromInventory.InventoryIdx, MySession.Static.LocalPlayerId);

    [Event(null, 10692)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void OnSetToConstructionRequest(
      Vector3I blockPosition,
      long ownerEntityId,
      byte inventoryIndex,
      long requestingPlayer)
    {
      MySlimBlock cubeBlock = this.GetCubeBlock(blockPosition);
      if (cubeBlock == null)
        return;
      cubeBlock.SetToConstructionSite();
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(ownerEntityId, out entity))
        return;
      MyInventoryBase inventory = (MyInventoryBase) MyEntityExtensions.GetInventory(entity == null || !entity.HasInventory ? (MyEntity) null : entity, (int) inventoryIndex);
      cubeBlock.MoveItemsToConstructionStockpile(inventory);
      cubeBlock.IncreaseMountLevel(MyWelder.WELDER_AMOUNT_PER_SECOND * 0.01666667f, requestingPlayer, (MyInventoryBase) null, 0.0f, false, MyOwnershipShareModeEnum.Faction, false, false);
    }

    public void ChangePowerProducerState(MyMultipleEnabledEnum enabledState, long playerId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyMultipleEnabledEnum, long>(this, (Func<MyCubeGrid, Action<MyMultipleEnabledEnum, long>>) (x => new Action<MyMultipleEnabledEnum, long>(x.OnPowerProducerStateRequest)), enabledState, playerId);

    [Event(null, 10723)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnPowerProducerStateRequest(MyMultipleEnabledEnum enabledState, long playerId)
    {
      this.GridSystems.SyncObject_PowerProducerStateChanged(enabledState, playerId);
      if (enabledState != MyMultipleEnabledEnum.AllDisabled && enabledState != MyMultipleEnabledEnum.AllEnabled)
        return;
      this.m_IsPowered = enabledState == MyMultipleEnabledEnum.AllEnabled;
    }

    public void RequestConversionToShip(Action result)
    {
      this.m_convertToShipResult = result;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, MyCubeGrid.MyTestDynamicReason>(this, (Func<MyCubeGrid, Action<MyCubeGrid.MyTestDynamicReason>>) (x => new Action<MyCubeGrid.MyTestDynamicReason>(x.OnConvertedToShipRequest)), MyCubeGrid.MyTestDynamicReason.ConvertToShip);
    }

    public void RequestConversionToStation() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.OnConvertedToStationRequest)));

    [Event(null, 10743)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.BigOwnerSpaceMaster)]
    private void OnConvertedToShipRequest(MyCubeGrid.MyTestDynamicReason reason)
    {
      if (!this.IsStatic || this.Physics == null || (this.BlocksCount == 0 || MyCubeGrid.ShouldBeStatic(this, reason)))
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.OnConvertToShipFailed)), MyEventContext.Current.Sender);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.OnConvertToDynamic)));
    }

    [Event(null, 10756)]
    [Reliable]
    [Client]
    private void OnConvertToShipFailed()
    {
      if (this.m_convertToShipResult != null)
        this.m_convertToShipResult();
      this.m_convertToShipResult = (Action) null;
    }

    [Event(null, 10764)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.BigOwnerSpaceMaster)]
    public void OnConvertedToStationRequest()
    {
      if (this.IsStatic || !MySessionComponentSafeZones.IsActionAllowed((MyEntity) this, MySafeZoneAction.ConvertToStation, user: MyEventContext.Current.Sender.Value))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.ConvertToStatic)));
    }

    public void ChangeOwnerRequest(
      MyCubeGrid grid,
      MyCubeBlock block,
      long playerId,
      MyOwnershipShareModeEnum shareMode)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, long, long, MyOwnershipShareModeEnum>(this, (Func<MyCubeGrid, Action<long, long, MyOwnershipShareModeEnum>>) (x => new Action<long, long, MyOwnershipShareModeEnum>(x.OnChangeOwnerRequest)), block.EntityId, playerId, shareMode);
    }

    [Event(null, 10783)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void OnChangeOwnerRequest(long blockId, long owner, MyOwnershipShareModeEnum shareMode)
    {
      MyCubeBlock entity = (MyCubeBlock) null;
      if (!MyEntities.TryGetEntityById<MyCubeBlock>(blockId, out entity))
        return;
      MyEntityOwnershipComponent ownershipComponent = entity.Components.Get<MyEntityOwnershipComponent>();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && entity.IDModule != null && (entity.IDModule.Owner == 0L || entity.IDModule.Owner == owner || owner == 0L))
      {
        this.OnChangeOwner(blockId, owner, shareMode);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, long, long, MyOwnershipShareModeEnum>(this, (Func<MyCubeGrid, Action<long, long, MyOwnershipShareModeEnum>>) (x => new Action<long, long, MyOwnershipShareModeEnum>(x.OnChangeOwner)), blockId, owner, shareMode);
      }
      else if (Sandbox.Game.Multiplayer.Sync.IsServer && ownershipComponent != null && (ownershipComponent.OwnerId == 0L || ownershipComponent.OwnerId == owner || owner == 0L))
      {
        this.OnChangeOwner(blockId, owner, shareMode);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, long, long, MyOwnershipShareModeEnum>(this, (Func<MyCubeGrid, Action<long, long, MyOwnershipShareModeEnum>>) (x => new Action<long, long, MyOwnershipShareModeEnum>(x.OnChangeOwner)), blockId, owner, shareMode);
      }
      else
      {
        bool flag = entity.BlockDefinition.ContainsComputer();
        if (entity.UseObjectsComponent != null)
          flag = flag || entity.UseObjectsComponent.GetDetectors("ownership").Count > 0;
        int num = flag ? 1 : 0;
      }
    }

    [Event(null, 10812)]
    [Reliable]
    [Broadcast]
    private void OnChangeOwner(long blockId, long owner, MyOwnershipShareModeEnum shareMode)
    {
      MyCubeBlock entity = (MyCubeBlock) null;
      if (!MyEntities.TryGetEntityById<MyCubeBlock>(blockId, out entity))
        return;
      entity.ChangeOwner(owner, shareMode);
    }

    private void HandBrakeChanged() => this.GridSystems.WheelSystem.HandBrake = (bool) this.m_handBrakeSync;

    [Event(null, 10827)]
    [Reliable]
    [Server]
    public void SetHandbrakeRequest(bool v) => this.m_handBrakeSync.Value = v;

    internal void EnableDampingInternal(bool enableDampeners, bool updateProxy)
    {
      if (this.EntityThrustComponent == null || this.EntityThrustComponent.DampenersEnabled == enableDampeners)
        return;
      this.EntityThrustComponent.DampenersEnabled = enableDampeners;
      this.m_dampenersEnabled.Value = enableDampeners;
      if (this.Physics != null && (HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null && !this.Physics.RigidBody.IsActive)
        this.ActivatePhysics();
      if (MySession.Static.LocalHumanPlayer == null || !(MySession.Static.LocalHumanPlayer.Controller.ControlledEntity is MyCockpit controlledEntity) || controlledEntity.CubeGrid != this)
        return;
      if (this.m_inertiaDampenersNotification == null)
        this.m_inertiaDampenersNotification = new MyHudNotification();
      this.m_inertiaDampenersNotification.Text = this.EntityThrustComponent.DampenersEnabled ? MyCommonTexts.NotificationInertiaDampenersOn : MyCommonTexts.NotificationInertiaDampenersOff;
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_inertiaDampenersNotification);
      MyHud.ShipInfo.Reload();
      MyHud.SinkGroupInfo.Reload();
    }

    private void DampenersEnabledChanged() => this.EnableDampingInternal(this.m_dampenersEnabled.Value, false);

    public void ChangeGridOwner(long playerId, MyOwnershipShareModeEnum shareMode)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, long, MyOwnershipShareModeEnum>(this, (Func<MyCubeGrid, Action<long, MyOwnershipShareModeEnum>>) (x => new Action<long, MyOwnershipShareModeEnum>(x.OnChangeGridOwner)), playerId, shareMode);
      this.OnChangeGridOwner(playerId, shareMode);
    }

    [Event(null, 10874)]
    [Reliable]
    [Broadcast]
    private void OnChangeGridOwner(long playerId, MyOwnershipShareModeEnum shareMode)
    {
      foreach (MySlimBlock block in this.GetBlocks())
      {
        if (block.FatBlock != null && block.BlockDefinition.RatioEnoughForOwnership(block.BuildLevelRatio))
          block.FatBlock.ChangeOwner(playerId, shareMode);
      }
    }

    public void AnnounceRemoveSplit(List<MySlimBlock> blocks)
    {
      MyCubeGrid.m_tmpPositionListSend.Clear();
      foreach (MySlimBlock block in blocks)
        MyCubeGrid.m_tmpPositionListSend.Add(block.Position);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<Vector3I>>(this, (Func<MyCubeGrid, Action<List<Vector3I>>>) (x => new Action<List<Vector3I>>(x.OnRemoveSplit)), MyCubeGrid.m_tmpPositionListSend);
    }

    [Event(null, 10896)]
    [Reliable]
    [Broadcast]
    private void OnRemoveSplit(List<Vector3I> removedBlocks)
    {
      MyCubeGrid.m_tmpPositionListReceive.Clear();
      foreach (Vector3I removedBlock in removedBlocks)
      {
        MySlimBlock cubeBlock = this.GetCubeBlock(removedBlock);
        if (cubeBlock == null)
          MySandboxGame.Log.WriteLine("Block was null when trying to remove a grid split. Desync?");
        else
          MyCubeGrid.m_tmpBlockListReceive.Add(cubeBlock);
      }
      MyCubeGrid.RemoveSplit(this, MyCubeGrid.m_tmpBlockListReceive, 0, MyCubeGrid.m_tmpBlockListReceive.Count, false);
      MyCubeGrid.m_tmpBlockListReceive.Clear();
    }

    public void ChangeDisplayNameRequest(string displayName) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, string>(this, (Func<MyCubeGrid, Action<string>>) (x => new Action<string>(x.OnChangeDisplayNameRequest)), displayName);

    [Event(null, 10923)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.BigOwner)]
    [Broadcast]
    private void OnChangeDisplayNameRequest(string displayName)
    {
      this.DisplayName = displayName;
      if (this.OnNameChanged == null)
        return;
      this.OnNameChanged(this);
    }

    public void ModifyGroup(MyBlockGroup group)
    {
      this.m_tmpBlockIdList.Clear();
      foreach (MyEntity block in group.Blocks)
        this.m_tmpBlockIdList.Add(block.EntityId);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, string, List<long>>(this, (Func<MyCubeGrid, Action<string, List<long>>>) (x => new Action<string, List<long>>(x.OnModifyGroupSuccess)), group.Name.ToString(), this.m_tmpBlockIdList);
    }

    [Event(null, 10941)]
    [Reliable]
    [Server(ValidationType.Access)]
    [BroadcastExcept]
    private void OnModifyGroupSuccess(string name, List<long> blocks)
    {
      if (blocks == null || blocks.Count == 0)
      {
        foreach (MyBlockGroup blockGroup in this.BlockGroups)
        {
          if (blockGroup.Name.ToString().Equals(name))
          {
            this.RemoveGroup(blockGroup);
            break;
          }
        }
      }
      else
      {
        MyBlockGroup group = new MyBlockGroup();
        group.Name.Clear().Append(name);
        foreach (long block in blocks)
        {
          MyTerminalBlock myTerminalBlock = (MyTerminalBlock) null;
          ref MyTerminalBlock local = ref myTerminalBlock;
          if (MyEntities.TryGetEntityById<MyTerminalBlock>(block, out local))
            group.Blocks.Add(myTerminalBlock);
        }
        this.AddGroup(group);
      }
    }

    public void RazeBlockInCompoundBlock(List<Tuple<Vector3I, ushort>> locationsAndIds)
    {
      MyCubeGrid.ConvertToLocationIdentityList(locationsAndIds, MyCubeGrid.m_tmpLocationsAndIdsSend);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<MyCubeGrid.LocationIdentity>>(this, (Func<MyCubeGrid, Action<List<MyCubeGrid.LocationIdentity>>>) (x => new Action<List<MyCubeGrid.LocationIdentity>>(x.OnRazeBlockInCompoundBlockRequest)), MyCubeGrid.m_tmpLocationsAndIdsSend);
    }

    [Event(null, 10975)]
    [Reliable]
    [Server]
    private void OnRazeBlockInCompoundBlockRequest(List<MyCubeGrid.LocationIdentity> locationsAndIds)
    {
      this.OnRazeBlockInCompoundBlock(locationsAndIds);
      if (MyCubeGrid.m_tmpLocationsAndIdsReceive.Count <= 0)
        return;
      MyCubeGrid.ConvertToLocationIdentityList(MyCubeGrid.m_tmpLocationsAndIdsReceive, MyCubeGrid.m_tmpLocationsAndIdsSend);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid, List<MyCubeGrid.LocationIdentity>>(this, (Func<MyCubeGrid, Action<List<MyCubeGrid.LocationIdentity>>>) (x => new Action<List<MyCubeGrid.LocationIdentity>>(x.OnRazeBlockInCompoundBlockSuccess)), MyCubeGrid.m_tmpLocationsAndIdsSend);
    }

    public void OnGridPresenceUpdate(bool isAnyGridPresent) => this.GridPresenceUpdate.InvokeIfNotNull<bool>(isAnyGridPresent);

    [Event(null, 10993)]
    [Reliable]
    [Broadcast]
    private void OnRazeBlockInCompoundBlockSuccess(List<MyCubeGrid.LocationIdentity> locationsAndIds) => this.OnRazeBlockInCompoundBlock(locationsAndIds);

    private void OnRazeBlockInCompoundBlock(List<MyCubeGrid.LocationIdentity> locationsAndIds)
    {
      MyCubeGrid.m_tmpLocationsAndIdsReceive.Clear();
      this.RazeBlockInCompoundBlockSuccess(locationsAndIds, MyCubeGrid.m_tmpLocationsAndIdsReceive);
    }

    private static void ConvertToLocationIdentityList(
      List<Tuple<Vector3I, ushort>> locationsAndIdsFrom,
      List<MyCubeGrid.LocationIdentity> locationsAndIdsTo)
    {
      locationsAndIdsTo.Clear();
      locationsAndIdsTo.Capacity = locationsAndIdsFrom.Count;
      foreach (Tuple<Vector3I, ushort> tuple in locationsAndIdsFrom)
        locationsAndIdsTo.Add(new MyCubeGrid.LocationIdentity()
        {
          Location = tuple.Item1,
          Id = tuple.Item2
        });
    }

    public static void ChangeOwnersRequest(
      MyOwnershipShareModeEnum shareMode,
      List<MyCubeGrid.MySingleOwnershipRequest> requests,
      long requestingPlayer)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>, long>((Func<IMyEventOwner, Action<MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>, long>>) (s => new Action<MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>, long>(MyCubeGrid.OnChangeOwnersRequest)), shareMode, requests, requestingPlayer);
    }

    [Event(null, 11020)]
    [Reliable]
    [Server(ValidationType.Access)]
    private static void OnChangeOwnersRequest(
      MyOwnershipShareModeEnum shareMode,
      List<MyCubeGrid.MySingleOwnershipRequest> requests,
      long requestingPlayer)
    {
      MyCubeBlock entity = (MyCubeBlock) null;
      int index = 0;
      ulong steamId = MySession.Static.Players.TryGetSteamId(requestingPlayer);
      bool flag = false;
      if (MySession.Static.IsUserAdmin(steamId))
      {
        AdminSettingsEnum adminSettingsEnum1;
        if (steamId != 0UL && MySession.Static.RemoteAdminSettings.TryGetValue(steamId, out adminSettingsEnum1) && adminSettingsEnum1.HasFlag((Enum) AdminSettingsEnum.Untargetable))
          index = requests.Count;
        AdminSettingsEnum? nullable1 = new AdminSettingsEnum?();
        if ((long) steamId == (long) Sandbox.Game.Multiplayer.Sync.MyId)
          nullable1 = new AdminSettingsEnum?(MySession.Static.AdminSettings);
        else if (MySession.Static.RemoteAdminSettings.ContainsKey(steamId))
          nullable1 = new AdminSettingsEnum?(MySession.Static.RemoteAdminSettings[steamId]);
        AdminSettingsEnum? nullable2 = nullable1;
        AdminSettingsEnum? nullable3 = nullable2.HasValue ? new AdminSettingsEnum?(nullable2.GetValueOrDefault() & AdminSettingsEnum.UseTerminals) : new AdminSettingsEnum?();
        AdminSettingsEnum adminSettingsEnum2 = AdminSettingsEnum.None;
        if (!(nullable3.GetValueOrDefault() == adminSettingsEnum2 & nullable3.HasValue))
          flag = true;
      }
      while (index < requests.Count)
      {
        if (MyEntities.TryGetEntityById<MyCubeBlock>(requests[index].BlockId, out entity))
        {
          MyEntityOwnershipComponent ownershipComponent = entity.Components.Get<MyEntityOwnershipComponent>();
          if (Sandbox.Game.Multiplayer.Sync.IsServer & flag)
            ++index;
          else if (Sandbox.Game.Multiplayer.Sync.IsServer && entity.IDModule != null && (entity.IDModule.Owner == 0L || entity.IDModule.Owner == requestingPlayer))
            ++index;
          else if (Sandbox.Game.Multiplayer.Sync.IsServer && ownershipComponent != null && (ownershipComponent.OwnerId == 0L || ownershipComponent.OwnerId == requestingPlayer))
            ++index;
          else
            requests.RemoveAtFast<MyCubeGrid.MySingleOwnershipRequest>(index);
        }
        else
          ++index;
      }
      if (requests.Count <= 0)
        return;
      MyCubeGrid.OnChangeOwnersSuccess(shareMode, requests);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>>((Func<IMyEventOwner, Action<MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>>>) (s => new Action<MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>>(MyCubeGrid.OnChangeOwnersSuccess)), shareMode, requests);
    }

    [Event(null, 11086)]
    [Reliable]
    [Broadcast]
    private static void OnChangeOwnersSuccess(
      MyOwnershipShareModeEnum shareMode,
      List<MyCubeGrid.MySingleOwnershipRequest> requests)
    {
      foreach (MyCubeGrid.MySingleOwnershipRequest request in requests)
      {
        MyCubeBlock entity = (MyCubeBlock) null;
        if (MyEntities.TryGetEntityById<MyCubeBlock>(request.BlockId, out entity))
          entity.ChangeOwner(request.Owner, shareMode);
      }
    }

    public override void SerializeControls(BitStream stream)
    {
      MyShipController myShipController = (MyShipController) null;
      if (!this.IsStatic && this.InScene)
        myShipController = this.GridSystems.ControlSystem.GetShipController();
      if (myShipController != null)
      {
        stream.WriteBool(true);
        myShipController.GetNetState().Serialize(stream);
      }
      else
        stream.WriteBool(false);
    }

    public override void DeserializeControls(BitStream stream, bool outOfOrder)
    {
      if (stream.ReadBool())
      {
        MyGridClientState myGridClientState = new MyGridClientState(stream);
        if (!outOfOrder)
          this.m_lastNetState = myGridClientState;
        if (this.GridSystems.ControlSystem == null)
          return;
        MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
        if (shipController == null || shipController.ControllerInfo == null || shipController.ControllerInfo.IsLocallyControlled())
          return;
        shipController.SetNetState(this.m_lastNetState);
      }
      else
        this.m_lastNetState.Valid = false;
    }

    public override void ResetControls()
    {
      this.m_lastNetState.Valid = false;
      MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
      if (shipController == null || shipController.ControllerInfo.IsLocallyControlled())
        return;
      shipController.ClearMovementControl();
    }

    public override void ApplyLastControls()
    {
      if (!this.m_lastNetState.Valid)
        return;
      MyShipController shipController = this.GridSystems.ControlSystem.GetShipController();
      if (shipController == null || shipController.ControllerInfo.IsLocallyControlled())
        return;
      shipController.SetNetState(this.m_lastNetState);
    }

    public bool UsesTargetingList => this.m_usesTargetingList;

    public long ClosestParentId
    {
      get => this.m_closestParentId;
      set
      {
        if (this.m_closestParentId == value)
          return;
        MyCubeGrid entity;
        if (MyEntities.TryGetEntityById<MyCubeGrid>(this.m_closestParentId, out entity, true))
          MyGridPhysicalHierarchy.Static.RemoveNonGridNode(entity, (MyEntity) this);
        if (MyEntities.TryGetEntityById<MyCubeGrid>(value, out entity))
        {
          this.m_closestParentId = value;
          MyGridPhysicalHierarchy.Static.AddNonGridNode(entity, (MyEntity) this);
        }
        else
          this.m_closestParentId = 0L;
      }
    }

    public bool IsClientPredicted
    {
      get => this.m_isClientPredicted;
      private set
      {
        if (this.m_isClientPredicted == value)
          return;
        this.m_isClientPredicted = value;
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.ClientPredictionStaticCheck), 5);
      }
    }

    public bool IsClientPredictedWheel { get; private set; }

    public bool IsClientPredictedCar { get; private set; }

    public bool ForceDisablePrediction
    {
      get => this.m_forceDisablePrediction;
      set
      {
        if (this.m_forceDisablePrediction == value)
          return;
        this.m_forceDisablePrediction = value;
        this.CheckPredictionFlagScheduling();
      }
    }

    public bool AllowPrediction
    {
      get => this.m_allowPrediction;
      set
      {
        if (this.m_allowPrediction == value)
          return;
        this.m_allowPrediction = value;
        this.CheckPredictionFlagScheduling();
      }
    }

    public void TargetingAddId(long id)
    {
      if (!this.m_targetingList.Contains(id))
        this.m_targetingList.Add(id);
      this.m_usesTargetingList = this.m_targetingList.Count > 0 || this.m_targetingListIsWhitelist;
    }

    public void TargetingRemoveId(long id)
    {
      if (this.m_targetingList.Contains(id))
        this.m_targetingList.Remove(id);
      this.m_usesTargetingList = this.m_targetingList.Count > 0 || this.m_targetingListIsWhitelist;
    }

    public void TargetingSetWhitelist(bool whitelist)
    {
      this.m_targetingListIsWhitelist = whitelist;
      this.m_usesTargetingList = this.m_targetingList.Count > 0 || this.m_targetingListIsWhitelist;
    }

    public bool TargetingCanAttackGrid(long id) => this.m_targetingListIsWhitelist ? this.m_targetingList.Contains(id) : !this.m_targetingList.Contains(id);

    public void HierarchyUpdated(MyCubeGrid root)
    {
      MyGridPhysics physics = this.Physics;
      if (physics != null)
      {
        if (this != root)
          physics.SetRelaxedRigidBodyMaxVelocities();
        else
          physics.SetDefaultRigidBodyMaxVelocities();
      }
      this.OnHierarchyUpdated.InvokeIfNotNull<MyCubeGrid>(this);
    }

    public void RegisterInventory(MyCubeBlock block)
    {
      this.m_inventoryBlocks.Add(block);
      MyInventoryBase inventoryBase;
      if (!block.TryGetInventory(out inventoryBase) || !(inventoryBase.CurrentMass > (MyFixedPoint) 0))
        return;
      this.SetInventoryMassDirty();
    }

    public void UnregisterInventory(MyCubeBlock block)
    {
      this.m_inventoryBlocks.Remove(block);
      MyInventoryBase inventoryBase;
      if (!block.TryGetInventory(out inventoryBase) || !(inventoryBase.CurrentMass > (MyFixedPoint) 0))
        return;
      this.SetInventoryMassDirty();
    }

    public void RegisterUnsafeBlock(MyCubeBlock block)
    {
      if (!this.m_unsafeBlocks.Add(block))
        return;
      if (this.m_unsafeBlocks.Count == 1)
        MyUnsafeGridsSessionComponent.RegisterGrid(this);
      else
        MyUnsafeGridsSessionComponent.OnGridChanged(this);
    }

    public void UnregisterUnsafeBlock(MyCubeBlock block)
    {
      if (!this.m_unsafeBlocks.Remove(block))
        return;
      if (this.m_unsafeBlocks.Count == 0)
        MyUnsafeGridsSessionComponent.UnregisterGrid(this);
      else
        MyUnsafeGridsSessionComponent.OnGridChanged(this);
    }

    public void RegisterDecoy(MyDecoy block)
    {
      if (this.m_decoys == null)
        this.m_decoys = new HashSet<MyDecoy>();
      this.m_decoys.Add(block);
    }

    public void UnregisterDecoy(MyDecoy block) => this.m_decoys.Remove(block);

    public void RegisterOccupiedBlock(MyCockpit block) => this.m_occupiedBlocks.Add(block);

    public void UnregisterOccupiedBlock(MyCockpit block) => this.m_occupiedBlocks.Remove(block);

    private void OnContactPointChanged()
    {
      if (this.Physics == null || this.Closed || (this.MarkedForClose || Sandbox.Engine.Platform.Game.IsDedicated))
        return;
      MyEntity.ContactPointData contactPointData = this.m_contactPoint.Value;
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(contactPointData.EntityId, out entity) || entity.Physics == null)
        return;
      Vector3D worldPosition = contactPointData.LocalPosition + this.PositionComp.WorldMatrixRef.Translation;
      if ((contactPointData.ContactPointType & MyEntity.ContactPointData.ContactPointDataTypes.Sounds) != MyEntity.ContactPointData.ContactPointDataTypes.None)
        MyAudioComponent.PlayContactSoundInternal((VRage.ModAPI.IMyEntity) this, (VRage.ModAPI.IMyEntity) entity, worldPosition, contactPointData.Normal, contactPointData.SeparatingSpeed);
      if ((contactPointData.ContactPointType & MyEntity.ContactPointData.ContactPointDataTypes.AnyParticle) == MyEntity.ContactPointData.ContactPointDataTypes.None)
        return;
      this.Physics.PlayCollisionParticlesInternal((VRage.ModAPI.IMyEntity) entity, ref worldPosition, ref contactPointData.Normal, ref contactPointData.SeparatingVelocity, contactPointData.SeparatingSpeed, contactPointData.Impulse, contactPointData.ContactPointType);
    }

    public void UpdateParticleContactPoint(
      long entityId,
      ref Vector3 relativePosition,
      ref Vector3 normal,
      ref Vector3 separatingVelocity,
      float separatingSpeed,
      float impulse,
      MyEntity.ContactPointData.ContactPointDataTypes flags)
    {
      if (flags == MyEntity.ContactPointData.ContactPointDataTypes.None)
        return;
      this.m_contactPoint.SetLocalValue(new MyEntity.ContactPointData()
      {
        EntityId = entityId,
        LocalPosition = relativePosition,
        Normal = normal,
        ContactPointType = flags,
        SeparatingVelocity = separatingVelocity,
        SeparatingSpeed = separatingSpeed,
        Impulse = impulse
      });
    }

    public void MarkAsTrash() => this.m_markedAsTrash.Value = true;

    private void MarkedAsTrashChanged()
    {
      if (this.MarkedAsTrash)
      {
        this.MarkForDraw();
        this.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateTrash), 3);
        this.m_trashHighlightCounter = MyCubeGrid.TRASH_HIGHLIGHT;
      }
      else
        this.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateTrash));
    }

    private void UpdateTrash()
    {
      --this.m_trashHighlightCounter;
      if (this.TrashHighlightCounter > 0 || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MySessionComponentTrash.RemoveGrid(this);
    }

    public int TrashHighlightCounter => this.m_trashHighlightCounter;

    public void LogHierarchy()
    {
      this.OnLogHierarchy();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyCubeGrid>(this, (Func<MyCubeGrid, Action>) (x => new Action(x.OnLogHierarchy)));
    }

    [Event(null, 11483)]
    [Reliable]
    [Server]
    public void OnLogHierarchy() => MyGridPhysicalHierarchy.Static.Log(MyGridPhysicalHierarchy.Static.GetRoot(this));

    [Event(null, 11490)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void DepressurizeEffect(long gridId, Vector3I from, Vector3I to) => MySandboxGame.Static.Invoke((Action) (() => MyCubeGrid.DepressurizeEffect_Implementation(gridId, from, to)), "CubeGrid - DepressurizeEffect");

    public static void DepressurizeEffect_Implementation(long gridId, Vector3I from, Vector3I to)
    {
      if (!(MyEntities.GetEntityById(gridId) is MyCubeGrid entityById))
        return;
      MyGridGasSystem.AddDepressurizationEffects(entityById, from, to);
    }

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

    public MyCubeGrid MergeGrid_MergeBlock(
      MyCubeGrid gridToMerge,
      Vector3I gridOffset,
      bool checkMergeOrder = true)
    {
      if (checkMergeOrder && !this.ShouldBeMergedToThis(gridToMerge))
        return (MyCubeGrid) null;
      MatrixI mergeTransform = this.CalculateMergeTransform(gridToMerge, gridOffset);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseBlockingEvent<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, MyCubeGrid>(this, gridToMerge, (Func<MyCubeGrid, Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction>>) (x => new Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction>(x.MergeGrid_MergeBlockClient)), gridToMerge.EntityId, (SerializableVector3I) mergeTransform.Translation, mergeTransform.Forward, mergeTransform.Up);
      return this.MergeGridInternal(gridToMerge, ref mergeTransform);
    }

    private bool ShouldBeMergedToThis(MyCubeGrid gridToMerge)
    {
      bool flag1 = MyCubeGrid.IsRooted(this);
      bool flag2 = MyCubeGrid.IsRooted(gridToMerge);
      if (flag1 && !flag2)
        return true;
      return (!flag2 || flag1) && this.BlocksCount > gridToMerge.BlocksCount;
    }

    private static bool IsRooted(MyCubeGrid grid)
    {
      if (grid.IsStatic)
        return true;
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
      if (group == null)
        return false;
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
      {
        if (MyFixedGrids.IsRooted(node.NodeData))
          return true;
      }
      return false;
    }

    [Event(null, 96)]
    [Reliable]
    [Broadcast]
    [Blocking]
    private void MergeGrid_MergeClient(
      long gridId,
      SerializableVector3I gridOffset,
      Base6Directions.Direction gridForward,
      Base6Directions.Direction gridUp,
      Vector3I mergingBlockPos)
    {
      MyCubeGrid entity = (MyCubeGrid) null;
      if (!MyEntities.TryGetEntityById<MyCubeGrid>(gridId, out entity))
        return;
      MatrixI transform = new MatrixI((Vector3I) gridOffset, gridForward, gridUp);
      this.MergeGridInternal(entity, ref transform);
    }

    [Event(null, 110)]
    [Reliable]
    [Broadcast]
    [Blocking]
    private void MergeGrid_MergeBlockClient(
      long gridId,
      SerializableVector3I gridOffset,
      Base6Directions.Direction gridForward,
      Base6Directions.Direction gridUp)
    {
      MyCubeGrid entity = (MyCubeGrid) null;
      if (!MyEntities.TryGetEntityById<MyCubeGrid>(gridId, out entity))
        return;
      MatrixI transform = new MatrixI((Vector3I) gridOffset, gridForward, gridUp);
      this.MergeGridInternal(entity, ref transform);
    }

    private MyCubeGrid MergeGrid_Static(
      MyCubeGrid gridToMerge,
      Vector3I gridOffset,
      MySlimBlock triggeringMergeBlock)
    {
      MatrixI mergeTransform = this.CalculateMergeTransform(gridToMerge, gridOffset);
      Vector3I vector3I = triggeringMergeBlock.Position;
      if (triggeringMergeBlock.CubeGrid != this)
        vector3I = Vector3I.Transform(vector3I, mergeTransform);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseBlockingEvent<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I, MyCubeGrid>(this, gridToMerge, (Func<MyCubeGrid, Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I>>) (x => new Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I>(x.MergeGrid_MergeClient)), gridToMerge.EntityId, (SerializableVector3I) mergeTransform.Translation, mergeTransform.Forward, mergeTransform.Up, vector3I);
      return this.MergeGridInternal(gridToMerge, ref mergeTransform);
    }

    private MyCubeGrid MergeGridInternal(
      MyCubeGrid gridToMerge,
      ref MatrixI transform,
      bool disableBlockGenerators = true)
    {
      if (MyCubeGridSmallToLargeConnection.Static != null)
        MyCubeGridSmallToLargeConnection.Static.BeforeGridMerge_SmallToLargeGridConnectivity(this, gridToMerge);
      MyRenderComponentCubeGrid tmpRenderComponent = gridToMerge.Render;
      tmpRenderComponent.DeferRenderRelease = true;
      Matrix transformMatrix = transform.GetFloatMatrix();
      transformMatrix.Translation *= this.GridSize;
      Action<MatrixD> updateMergingComponentWM = (Action<MatrixD>) (matrix =>
      {
        MyRenderComponentCubeGrid componentCubeGrid = tmpRenderComponent;
        MatrixD matrixD = transformMatrix * matrix;
        Matrix matrix1 = (Matrix) ref matrixD;
        componentCubeGrid.UpdateRenderObjectMatrices(matrix1);
      });
      Action releaseRenderOldRenderComponent = (Action) null;
      releaseRenderOldRenderComponent = (Action) (() =>
      {
        tmpRenderComponent.DeferRenderRelease = false;
        this.m_updateMergingGrids -= updateMergingComponentWM;
        this.m_pendingGridReleases -= releaseRenderOldRenderComponent;
      });
      this.m_updateMergingGrids += updateMergingComponentWM;
      this.m_pendingGridReleases += releaseRenderOldRenderComponent;
      this.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateMergingGrids), 7);
      MyCubeGrid.MoveBlocksAndClose(gridToMerge, this, transform, disableBlockGenerators);
      this.UpdateGridAABB();
      if (this.Physics != null)
        this.UpdatePhysicsShape();
      if (MyCubeGridSmallToLargeConnection.Static != null)
        MyCubeGridSmallToLargeConnection.Static.AfterGridMerge_SmallToLargeGridConnectivity(this);
      updateMergingComponentWM(this.WorldMatrix);
      return this;
    }

    private static void MoveBlocksAndClose(
      MyCubeGrid from,
      MyCubeGrid to,
      MatrixI transform,
      bool disableBlockGenerators = true)
    {
      from.MarkedForClose = true;
      to.IsBlockTrasferInProgress = true;
      from.IsBlockTrasferInProgress = true;
      try
      {
        int num1 = disableBlockGenerators ? 1 : 0;
        MyEntities.Remove((MyEntity) from);
        foreach (MyBlockGroup group in from.BlockGroups.ToArray())
          to.AddGroup(group);
        from.BlockGroups.Clear();
        from.UnregisterBlocksBeforeClose();
        foreach (MySlimBlock cubeBlock in from.m_cubeBlocks)
        {
          if (cubeBlock.FatBlock != null)
            from.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) cubeBlock.FatBlock);
          cubeBlock.RemoveNeighbours();
          cubeBlock.RemoveAuthorship();
        }
        if (from.Physics != null)
        {
          from.Physics.Close();
          from.Physics = (MyGridPhysics) null;
          from.RaisePhysicsChanged();
        }
        foreach (MySlimBlock cubeBlock in from.m_cubeBlocks)
        {
          cubeBlock.Transform(ref transform);
          to.AddBlockInternal(cubeBlock);
        }
        from.Skeleton.CopyTo(to.Skeleton, transform, to);
        int num2 = disableBlockGenerators ? 1 : 0;
        from.m_blocksForDraw.Clear();
        from.m_cubeBlocks.Clear();
        from.m_fatBlocks.Clear();
        from.m_cubes.Clear();
        from.MarkedForClose = false;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        from.Close();
      }
      finally
      {
        to.IsBlockTrasferInProgress = false;
        from.IsBlockTrasferInProgress = false;
      }
    }

    private void UpdateMergingGrids()
    {
      if (this.m_updateMergingGrids != null)
        this.m_updateMergingGrids(this.WorldMatrix);
      else
        this.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.UpdateMergingGrids));
    }

    private void ReleaseMerginGrids()
    {
      if (this.m_pendingGridReleases == null)
        return;
      this.m_pendingGridReleases();
    }

    private bool CanMoveBlocksFrom(MyCubeGrid grid, Vector3I blockOffset)
    {
      try
      {
        MatrixI mergeTransform = this.CalculateMergeTransform(grid, blockOffset);
        foreach (KeyValuePair<Vector3I, MyCube> cube in grid.m_cubes)
        {
          if (this.m_cubes.ContainsKey(Vector3I.Transform(cube.Key, mergeTransform)))
            return false;
        }
        return true;
      }
      finally
      {
      }
    }

    public static void Preload()
    {
    }

    private static List<MyEntity> m_tmpResultList => MyUtils.Init<List<MyEntity>>(ref MyCubeGrid.m_tmpResultListPerThread);

    public static bool ShowSenzorGizmos { get; set; }

    public static bool ShowGravityGizmos { get; set; }

    public static bool ShowAntennaGizmos { get; set; }

    public static bool ShowCenterOfMass { get; set; }

    public static bool ShowGridPivot { get; set; }

    private static List<HkBodyCollision> m_physicsBoxQueryList => MyUtils.Init<List<HkBodyCollision>>(ref MyCubeGrid.m_physicsBoxQueryListPerThread);

    private static List<Vector3I> m_cacheRayCastCells => MyUtils.Init<List<Vector3I>>(ref MyCubeGrid.m_cacheRayCastCellsPerThread);

    private static Dictionary<Vector3I, ConnectivityResult> m_cacheNeighborBlocks => MyUtils.Init<Dictionary<Vector3I, ConnectivityResult>>(ref MyCubeGrid.m_cacheNeighborBlocksPerThread);

    private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsA => MyUtils.Init<List<MyCubeBlockDefinition.MountPoint>>(ref MyCubeGrid.m_cacheMountPointsAPerThread);

    private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsB => MyUtils.Init<List<MyCubeBlockDefinition.MountPoint>>(ref MyCubeGrid.m_cacheMountPointsBPerThread);

    private static List<MyPhysics.HitInfo> m_tmpHitList => MyUtils.Init<List<MyPhysics.HitInfo>>(ref MyCubeGrid.m_tmpHitListPerThread);

    private static List<int> m_tmpMultiBlockIndices => MyUtils.Init<List<int>>(ref MyCubeGrid.m_tmpMultiBlockIndicesPerThread);

    private static Ref<HkBoxShape> m_lastQueryBox
    {
      get
      {
        if (MyCubeGrid.m_lastQueryBoxPerThread == null)
        {
          MyCubeGrid.m_lastQueryBoxPerThread = new Ref<HkBoxShape>();
          MyCubeGrid.m_lastQueryBoxPerThread.Value = new HkBoxShape(Vector3.One);
        }
        return MyCubeGrid.m_lastQueryBoxPerThread;
      }
    }

    public static void GetCubeParts(
      MyCubeBlockDefinition block,
      Vector3I inputPosition,
      Matrix rotation,
      float gridSize,
      List<string> outModels,
      List<MatrixD> outLocalMatrices,
      List<Vector3> outLocalNormals,
      List<Vector4UByte> outPatternOffsets,
      bool topologyCheck)
    {
      outModels.Clear();
      outLocalMatrices.Clear();
      outLocalNormals.Clear();
      outPatternOffsets.Clear();
      if (block.CubeDefinition == null)
        return;
      if (topologyCheck)
      {
        Base6Directions.Direction direction1 = Base6Directions.GetDirection(Vector3I.Round(rotation.Forward));
        Base6Directions.Direction direction2 = Base6Directions.GetDirection(Vector3I.Round(rotation.Up));
        MyCubeGridDefinitions.GetTopologyUniqueOrientation(block.CubeDefinition.CubeTopology, new MyBlockOrientation(direction1, direction2)).GetMatrix(out rotation);
      }
      MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(block);
      int length = cubeTiles.Length;
      int num1 = 0;
      int num2 = 32768;
      float epsilon = 0.01f;
      for (int index = 0; index < length; ++index)
      {
        MyTileDefinition myTileDefinition1 = cubeTiles[num1 + index];
        MatrixD matrixD = (MatrixD) ref myTileDefinition1.LocalMatrix * rotation;
        Vector3 vector2 = Vector3.Transform(myTileDefinition1.Normal, rotation.GetOrientation());
        if (topologyCheck && myTileDefinition1.Id != MyStringId.NullOrEmpty)
        {
          Dictionary<Vector3I, MyTileDefinition> dictionary;
          MyCubeGridDefinitions.TileGridOrientations.TryGetValue(myTileDefinition1.Id, out dictionary);
          MyTileDefinition myTileDefinition2;
          if (dictionary.TryGetValue(new Vector3I(Vector3.Sign(vector2)), out myTileDefinition2))
            matrixD = (MatrixD) ref myTileDefinition2.LocalMatrix;
        }
        Vector3I vector3I1 = inputPosition;
        if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base && myTileDefinition1.Id == MyStringId.NullOrEmpty)
        {
          Vector3I vector3I2 = new Vector3I(-Vector3.Sign(vector2.MaxAbsComponent()));
          vector3I1 += vector3I2;
        }
        string modelAsset = block.CubeDefinition.Model[index];
        Vector2I vector2I1 = block.CubeDefinition.PatternSize[index];
        Vector2I vector2I2 = block.CubeDefinition.ScaleTile[index];
        int patternScale = (int) MyModels.GetModelOnlyData(modelAsset).PatternScale;
        vector2I1 = new Vector2I(vector2I1.X * patternScale, vector2I1.Y * patternScale);
        int num3 = 0;
        int num4 = 0;
        float num5 = Vector3.Dot(Vector3.UnitY, vector2);
        float num6 = Vector3.Dot(Vector3.UnitX, vector2);
        float num7 = Vector3.Dot(Vector3.UnitZ, vector2);
        if (MyUtils.IsZero(Math.Abs(num5) - 1f, epsilon))
        {
          int num8 = (vector3I1.X + num2) / vector2I1.Y;
          int num9 = MyMath.Mod(num8 + (int) ((double) num8 * Math.Sin((double) num8 * 10.0)), vector2I1.X);
          num3 = MyMath.Mod(vector3I1.Z + vector3I1.Y + num9 + num2, vector2I1.X);
          num4 = MyMath.Mod(vector3I1.X + num2, vector2I1.Y);
          if (Math.Sign(num5) == 1)
            num4 = vector2I1.Y - 1 - num4;
        }
        else if (MyUtils.IsZero(Math.Abs(num6) - 1f, epsilon))
        {
          int num8 = (vector3I1.Z + num2) / vector2I1.Y;
          int num9 = MyMath.Mod(num8 + (int) ((double) num8 * Math.Sin((double) num8 * 10.0)), vector2I1.X);
          num3 = MyMath.Mod(vector3I1.X + vector3I1.Y + num9 + num2, vector2I1.X);
          num4 = MyMath.Mod(vector3I1.Z + num2, vector2I1.Y);
          if (Math.Sign(num6) == 1)
            num4 = vector2I1.Y - 1 - num4;
        }
        else if (MyUtils.IsZero(Math.Abs(num7) - 1f, epsilon))
        {
          int num8 = (vector3I1.Y + num2) / vector2I1.Y;
          int num9 = MyMath.Mod(num8 + (int) ((double) num8 * Math.Sin((double) num8 * 10.0)), vector2I1.X);
          num3 = MyMath.Mod(vector3I1.X + num9 + num2, vector2I1.X);
          num4 = MyMath.Mod(vector3I1.Y + num2, vector2I1.Y);
          if (Math.Sign(num7) == 1)
            num3 = vector2I1.X - 1 - num3;
        }
        else if (MyUtils.IsZero(num6, epsilon))
        {
          num3 = MyMath.Mod(vector3I1.X * vector2I2.X + num2, vector2I1.X);
          num4 = MyMath.Mod(vector3I1.Z * vector2I2.Y + num2, vector2I1.Y);
          if (Math.Sign(num7) == -1)
          {
            if (Math.Sign(num5) == 1)
            {
              if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
              {
                if ((double) num7 < -0.5)
                {
                  num3 = MyMath.Mod(vector3I1.X * vector2I2.X + num2, vector2I1.X);
                  num4 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
                }
                else
                {
                  num4 = vector2I1.Y - 1 - num4;
                  num3 = vector2I1.X - 1 - num3;
                }
              }
              else
              {
                num4 = vector2I1.Y - 1 - num4;
                num3 = vector2I1.X - 1 - num3;
              }
            }
            else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
            {
              if ((double) num7 < -0.5)
              {
                int num8 = MyMath.Mod(vector3I1.X * vector2I2.X + num2, vector2I1.X);
                int num9 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
                num3 = vector2I1.X - 1 - num8;
                num4 = vector2I1.Y - 1 - num9;
              }
              else
                num4 = vector2I1.Y - 1 - num4;
            }
            else
              num4 = vector2I1.Y - 1 - num4;
          }
          else if (Math.Sign(num5) == -1)
          {
            if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
            {
              if ((double) num7 > 0.5)
              {
                num3 = MyMath.Mod(vector3I1.X * vector2I2.X + num2, vector2I1.X);
                int num8 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
                num4 = vector2I1.Y - 1 - num8;
              }
              else
                num3 = vector2I1.X - 1 - num3;
            }
            else
              num3 = vector2I1.X - 1 - num3;
          }
          else if ((block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip) && (double) num5 <= 0.5)
          {
            int num8 = MyMath.Mod(vector3I1.X * vector2I2.X + num2, vector2I1.X);
            num4 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
            num3 = vector2I1.X - 1 - num8;
          }
        }
        else if (MyUtils.IsZero(num7, epsilon))
        {
          num3 = MyMath.Mod(vector3I1.Z * vector2I2.X + num2, vector2I1.X);
          num4 = MyMath.Mod(vector3I1.X * vector2I2.Y + num2, vector2I1.Y);
          if (Math.Sign(num6) == 1)
          {
            if (Math.Sign(num5) == 1)
            {
              if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
              {
                if ((double) num6 > 0.5)
                {
                  num3 = MyMath.Mod(vector3I1.Z * vector2I2.X + num2, vector2I1.X);
                  num4 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
                }
                else
                  num3 = vector2I1.X - 1 - num3;
              }
              else
                num3 = vector2I1.X - 1 - num3;
            }
            else if ((block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip) && (double) num5 >= -0.5)
            {
              int num8 = MyMath.Mod(vector3I1.Z * vector2I2.X + num2, vector2I1.X);
              int num9 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
              num3 = vector2I1.X - 1 - num8;
              num4 = vector2I1.Y - 1 - num9;
            }
          }
          else if (Math.Sign(num5) == 1)
          {
            if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
            {
              if ((double) num5 > 0.5)
              {
                num4 = vector2I1.Y - 1 - num4;
              }
              else
              {
                int num8 = MyMath.Mod(vector3I1.Z * vector2I2.X + num2, vector2I1.X);
                num4 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
                num3 = vector2I1.X - 1 - num8;
              }
            }
            else
              num4 = vector2I1.Y - 1 - num4;
          }
          else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
          {
            if ((double) num5 < -0.5)
            {
              num3 = vector2I1.X - 1 - num3;
              num4 = vector2I1.Y - 1 - num4;
            }
            else
            {
              num3 = MyMath.Mod(vector3I1.Z * vector2I2.X + num2, vector2I1.X);
              int num8 = MyMath.Mod(vector3I1.Y * vector2I2.Y + num2, vector2I1.Y);
              num4 = vector2I1.Y - 1 - num8;
            }
          }
          else
          {
            num3 = vector2I1.X - 1 - num3;
            num4 = vector2I1.Y - 1 - num4;
          }
        }
        else if (MyUtils.IsZero(num5, epsilon))
        {
          num3 = MyMath.Mod(vector3I1.Y * vector2I2.X + num2, vector2I1.X);
          num4 = MyMath.Mod(vector3I1.Z * vector2I2.Y + num2, vector2I1.Y);
          if (Math.Sign(num7) == -1)
          {
            if (Math.Sign(num6) == 1)
            {
              if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
              {
                if ((double) num7 < -0.5)
                {
                  num3 = vector2I1.X - 1 - num3;
                  num4 = vector2I1.Y - 1 - num4;
                }
                else
                  num4 = vector2I1.Y - 1 - num4;
              }
              else
                num3 = vector2I1.X - 1 - num3;
            }
            else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
            {
              if ((double) num7 < -0.5)
              {
                num4 = vector2I1.Y - 1 - num4;
              }
              else
              {
                num3 = vector2I1.X - 1 - num3;
                num4 = vector2I1.Y - 1 - num4;
              }
            }
          }
          else if (Math.Sign(num6) == 1)
          {
            if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
            {
              if ((double) num6 > 0.5)
              {
                num3 = vector2I1.X - 1 - num3;
              }
              else
              {
                num3 = MyMath.Mod(vector3I1.Y * vector2I2.X + num2, vector2I1.X);
                num4 = MyMath.Mod(vector3I1.X * vector2I2.Y + num2, vector2I1.Y);
              }
            }
            else
              num4 = vector2I1.Y - 1 - num4;
          }
          else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
          {
            if ((double) num6 >= -0.5)
            {
              int num8 = MyMath.Mod(vector3I1.Y * vector2I2.X + num2, vector2I1.X);
              int num9 = MyMath.Mod(vector3I1.X * vector2I2.Y + num2, vector2I1.Y);
              num3 = vector2I1.X - 1 - num8;
              num4 = vector2I1.Y - 1 - num9;
            }
          }
          else
          {
            num3 = vector2I1.X - 1 - num3;
            num4 = vector2I1.Y - 1 - num4;
          }
        }
        matrixD.Translation = (Vector3D) (inputPosition * gridSize);
        if (myTileDefinition1.DontOffsetTexture)
        {
          num3 = 0;
          num4 = 0;
        }
        outPatternOffsets.Add(new Vector4UByte((byte) num3, (byte) num4, (byte) vector2I1.X, (byte) vector2I1.Y));
        outModels.Add(modelAsset);
        outLocalMatrices.Add(matrixD);
        outLocalNormals.Add(vector2);
      }
    }

    public static void CheckAreaConnectivity(
      MyCubeGrid grid,
      ref MyCubeGrid.MyBlockBuildArea area,
      List<Vector3UByte> validOffsets,
      HashSet<Vector3UByte> resultFailList)
    {
      try
      {
        MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) area.DefinitionId);
        if (cubeBlockDefinition == null)
          return;
        Quaternion orientation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
        Vector3I stepDelta = (Vector3I) area.StepDelta;
        MyCubeBlockDefinition.MountPoint[] modelMountPoints = cubeBlockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
        for (int index = validOffsets.Count - 1; index >= 0; --index)
        {
          Vector3I position = area.PosInGrid + (Vector3I) validOffsets[index] * stepDelta;
          if (MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) grid, cubeBlockDefinition, modelMountPoints, ref orientation, ref position))
          {
            MyCubeGrid.m_tmpAreaMountpointPass.Add(validOffsets[index]);
            validOffsets.RemoveAtFast<Vector3UByte>(index);
          }
        }
        MyCubeGrid.m_areaOverlapTest.Initialize(ref area, cubeBlockDefinition);
        foreach (Vector3UByte offset in MyCubeGrid.m_tmpAreaMountpointPass)
          MyCubeGrid.m_areaOverlapTest.AddBlock(offset);
        int num = int.MaxValue;
        while (validOffsets.Count > 0 && validOffsets.Count < num)
        {
          num = validOffsets.Count;
          for (int index = validOffsets.Count - 1; index >= 0; --index)
          {
            Vector3I position = area.PosInGrid + (Vector3I) validOffsets[index] * stepDelta;
            if (MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) MyCubeGrid.m_areaOverlapTest, cubeBlockDefinition, modelMountPoints, ref orientation, ref position))
            {
              MyCubeGrid.m_tmpAreaMountpointPass.Add(validOffsets[index]);
              MyCubeGrid.m_areaOverlapTest.AddBlock(validOffsets[index]);
              validOffsets.RemoveAtFast<Vector3UByte>(index);
            }
          }
        }
        foreach (Vector3UByte validOffset in validOffsets)
          resultFailList.Add(validOffset);
        validOffsets.Clear();
        validOffsets.AddRange((IEnumerable<Vector3UByte>) MyCubeGrid.m_tmpAreaMountpointPass);
      }
      finally
      {
        MyCubeGrid.m_tmpAreaMountpointPass.Clear();
      }
    }

    public static bool CheckMergeConnectivity(
      MyCubeGrid hitGrid,
      MyCubeGrid gridToMerge,
      Vector3I gridOffset)
    {
      MatrixI mergeTransform = hitGrid.CalculateMergeTransform(gridToMerge, gridOffset);
      Quaternion result1;
      mergeTransform.GetBlockOrientation().GetQuaternion(out result1);
      foreach (MySlimBlock block in gridToMerge.GetBlocks())
      {
        Vector3I position = Vector3I.Transform(block.Position, mergeTransform);
        Quaternion result2;
        block.Orientation.GetQuaternion(out result2);
        result2 = result1 * result2;
        MyCubeBlockDefinition.MountPoint[] modelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
        if (MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) hitGrid, block.BlockDefinition, modelMountPoints, ref result2, ref position))
          return true;
      }
      return false;
    }

    public static bool CheckConnectivity(
      IMyGridConnectivityTest grid,
      MyCubeBlockDefinition def,
      MyCubeBlockDefinition.MountPoint[] mountPoints,
      ref Quaternion rotation,
      ref Vector3I position)
    {
      try
      {
        if (mountPoints == null)
          return false;
        Vector3I center = def.Center;
        Vector3I size = def.Size;
        Vector3I.Transform(ref center, ref rotation, out Vector3I _);
        Vector3I.Transform(ref size, ref rotation, out Vector3I _);
        for (int index = 0; index < mountPoints.Length; ++index)
        {
          MyCubeBlockDefinition.MountPoint mountPoint = mountPoints[index];
          Vector3 vector3_1 = mountPoint.Start - (Vector3) center;
          Vector3 vector3_2 = mountPoint.End - (Vector3) center;
          if (MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK)
          {
            Vector3 vector3_3 = Vector3.Min(mountPoint.Start, mountPoint.End);
            Vector3 vector3_4 = Vector3.Max(mountPoint.Start, mountPoint.End);
            Vector3I vector3I1 = Vector3I.One - Vector3I.Abs(mountPoint.Normal);
            Vector3I vector3I2 = Vector3I.One - vector3I1;
            Vector3 vector3_5 = vector3I2 * vector3_3 + Vector3.Clamp(vector3_3, Vector3.Zero, (Vector3) size) * vector3I1 + 1f / 1000f * vector3I1;
            Vector3 vector3_6 = vector3I2 * vector3_4 + Vector3.Clamp(vector3_4, Vector3.Zero, (Vector3) size) * vector3I1 - 1f / 1000f * vector3I1;
            vector3_1 = vector3_5 - (Vector3) center;
            Vector3 vector3_7 = (Vector3) center;
            vector3_2 = vector3_6 - vector3_7;
          }
          Vector3I vector3I3 = Vector3I.Floor(vector3_1);
          Vector3I vector3I4 = Vector3I.Floor(vector3_2);
          Vector3 result1;
          Vector3.Transform(ref vector3_1, ref rotation, out result1);
          Vector3 result2;
          Vector3.Transform(ref vector3_2, ref rotation, out result2);
          Vector3I result3;
          Vector3I.Transform(ref vector3I3, ref rotation, out result3);
          Vector3I result4;
          Vector3I.Transform(ref vector3I4, ref rotation, out result4);
          Vector3I vector3I5 = Vector3I.Floor(result1);
          Vector3I vector3I6 = Vector3I.Floor(result2);
          Vector3I vector3I7 = result3 - vector3I5;
          Vector3I vector3I8 = result4 - vector3I6;
          result1 += (Vector3) vector3I7;
          result2 += (Vector3) vector3I8;
          Vector3 vector3_8 = (Vector3) position + result1;
          Vector3 vector3_9 = (Vector3) position + result2;
          MyCubeGrid.m_cacheNeighborBlocks.Clear();
          Vector3 currentMin = Vector3.Min(vector3_8, vector3_9);
          Vector3 currentMax = Vector3.Max(vector3_8, vector3_9);
          Vector3I minI = Vector3I.Floor(currentMin);
          Vector3I maxI = Vector3I.Floor(currentMax);
          grid.GetConnectedBlocks(minI, maxI, MyCubeGrid.m_cacheNeighborBlocks);
          if (MyCubeGrid.m_cacheNeighborBlocks.Count != 0)
          {
            Vector3I result5;
            Vector3I.Transform(ref mountPoint.Normal, ref rotation, out result5);
            Vector3I otherBlockMinPos = minI - result5;
            Vector3I otherBlockMaxPos = maxI - result5;
            Vector3I faceNormal = -result5;
            foreach (ConnectivityResult connectivityResult in MyCubeGrid.m_cacheNeighborBlocks.Values)
            {
              if (connectivityResult.Position == position)
              {
                if (MyFakes.ENABLE_COMPOUND_BLOCKS && (connectivityResult.FatBlock == null || !connectivityResult.FatBlock.CheckConnectionAllowed || connectivityResult.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def)) && connectivityResult.FatBlock is MyCompoundCubeBlock)
                {
                  foreach (MySlimBlock block in (connectivityResult.FatBlock as MyCompoundCubeBlock).GetBlocks())
                  {
                    MyCubeBlockDefinition.MountPoint[] modelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
                    if (MyCubeGrid.CheckNeighborMountPointsForCompound(currentMin, currentMax, mountPoint, ref result5, def, connectivityResult.Position, block.BlockDefinition, modelMountPoints, block.Orientation, MyCubeGrid.m_cacheMountPointsA))
                      return true;
                  }
                }
              }
              else if (connectivityResult.FatBlock == null || !connectivityResult.FatBlock.CheckConnectionAllowed || connectivityResult.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def))
              {
                if (connectivityResult.FatBlock is MyCompoundCubeBlock)
                {
                  foreach (MySlimBlock block in (connectivityResult.FatBlock as MyCompoundCubeBlock).GetBlocks())
                  {
                    MyCubeBlockDefinition.MountPoint[] modelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
                    if (MyCubeGrid.CheckNeighborMountPoints(currentMin, currentMax, mountPoint, ref result5, def, connectivityResult.Position, block.BlockDefinition, modelMountPoints, block.Orientation, MyCubeGrid.m_cacheMountPointsA))
                      return true;
                  }
                }
                else
                {
                  float currentIntegrityRatio = 1f;
                  if (connectivityResult.FatBlock != null && connectivityResult.FatBlock.SlimBlock != null)
                    currentIntegrityRatio = connectivityResult.FatBlock.SlimBlock.BuildLevelRatio;
                  MyCubeBlockDefinition.MountPoint[] modelMountPoints = connectivityResult.Definition.GetBuildProgressModelMountPoints(currentIntegrityRatio);
                  if (MyCubeGrid.CheckNeighborMountPoints(currentMin, currentMax, mountPoint, ref result5, def, connectivityResult.Position, connectivityResult.Definition, modelMountPoints, connectivityResult.Orientation, MyCubeGrid.m_cacheMountPointsA))
                    return true;
                }
              }
            }
          }
        }
        return false;
      }
      finally
      {
        MyCubeGrid.m_cacheNeighborBlocks.Clear();
      }
    }

    public static bool CheckConnectivitySmallBlockToLargeGrid(
      MyCubeGrid grid,
      MyCubeBlockDefinition def,
      ref Quaternion rotation,
      ref Vector3I addNormal)
    {
      try
      {
        MyCubeBlockDefinition.MountPoint[] mountPoints = def.MountPoints;
        if (mountPoints == null)
          return false;
        for (int index = 0; index < mountPoints.Length; ++index)
        {
          Vector3I result;
          Vector3I.Transform(ref mountPoints[index].Normal, ref rotation, out result);
          if (addNormal == -result)
            return true;
        }
        return false;
      }
      finally
      {
        MyCubeGrid.m_cacheNeighborBlocks.Clear();
      }
    }

    public static bool CheckNeighborMountPoints(
      Vector3 currentMin,
      Vector3 currentMax,
      MyCubeBlockDefinition.MountPoint thisMountPoint,
      ref Vector3I thisMountPointTransformedNormal,
      MyCubeBlockDefinition thisDefinition,
      Vector3I neighborPosition,
      MyCubeBlockDefinition neighborDefinition,
      MyCubeBlockDefinition.MountPoint[] neighborMountPoints,
      MyBlockOrientation neighborOrientation,
      List<MyCubeBlockDefinition.MountPoint> otherMountPoints)
    {
      if (!thisMountPoint.Enabled)
        return false;
      BoundingBox boundingBox = new BoundingBox(currentMin - (Vector3) neighborPosition, currentMax - (Vector3) neighborPosition);
      MyCubeGrid.TransformMountPoints(otherMountPoints, neighborDefinition, neighborMountPoints, ref neighborOrientation);
      foreach (MyCubeBlockDefinition.MountPoint otherMountPoint in otherMountPoints)
      {
        if ((((int) thisMountPoint.ExclusionMask & (int) otherMountPoint.PropertiesMask) == 0 && ((int) thisMountPoint.PropertiesMask & (int) otherMountPoint.ExclusionMask) == 0 || !(thisDefinition.Id != neighborDefinition.Id)) && (otherMountPoint.Enabled && (!MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK || !(thisMountPointTransformedNormal + otherMountPoint.Normal != Vector3I.Zero))))
        {
          BoundingBox box = new BoundingBox(Vector3.Min(otherMountPoint.Start, otherMountPoint.End), Vector3.Max(otherMountPoint.Start, otherMountPoint.End));
          if (boundingBox.Intersects(box))
            return true;
        }
      }
      return false;
    }

    public static bool CheckNeighborMountPointsForCompound(
      Vector3 currentMin,
      Vector3 currentMax,
      MyCubeBlockDefinition.MountPoint thisMountPoint,
      ref Vector3I thisMountPointTransformedNormal,
      MyCubeBlockDefinition thisDefinition,
      Vector3I neighborPosition,
      MyCubeBlockDefinition neighborDefinition,
      MyCubeBlockDefinition.MountPoint[] neighborMountPoints,
      MyBlockOrientation neighborOrientation,
      List<MyCubeBlockDefinition.MountPoint> otherMountPoints)
    {
      if (!thisMountPoint.Enabled)
        return false;
      BoundingBox boundingBox = new BoundingBox(currentMin - (Vector3) neighborPosition, currentMax - (Vector3) neighborPosition);
      MyCubeGrid.TransformMountPoints(otherMountPoints, neighborDefinition, neighborMountPoints, ref neighborOrientation);
      foreach (MyCubeBlockDefinition.MountPoint otherMountPoint in otherMountPoints)
      {
        if ((((int) thisMountPoint.ExclusionMask & (int) otherMountPoint.PropertiesMask) == 0 && ((int) thisMountPoint.PropertiesMask & (int) otherMountPoint.ExclusionMask) == 0 || !(thisDefinition.Id != neighborDefinition.Id)) && (otherMountPoint.Enabled && (!MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK || !(thisMountPointTransformedNormal - otherMountPoint.Normal != Vector3I.Zero))))
        {
          BoundingBox box = new BoundingBox(Vector3.Min(otherMountPoint.Start, otherMountPoint.End), Vector3.Max(otherMountPoint.Start, otherMountPoint.End));
          if (boundingBox.Intersects(box))
            return true;
        }
      }
      return false;
    }

    public static bool CheckMountPointsForSide(
      MyCubeBlockDefinition defA,
      MyCubeBlockDefinition.MountPoint[] mountPointsA,
      ref MyBlockOrientation orientationA,
      ref Vector3I positionA,
      ref Vector3I normalA,
      MyCubeBlockDefinition defB,
      MyCubeBlockDefinition.MountPoint[] mountPointsB,
      ref MyBlockOrientation orientationB,
      ref Vector3I positionB)
    {
      MyCubeGrid.TransformMountPoints(MyCubeGrid.m_cacheMountPointsA, defA, mountPointsA, ref orientationA);
      MyCubeGrid.TransformMountPoints(MyCubeGrid.m_cacheMountPointsB, defB, mountPointsB, ref orientationB);
      return MyCubeGrid.CheckMountPointsForSide(MyCubeGrid.m_cacheMountPointsA, ref orientationA, ref positionA, defA.Id, ref normalA, MyCubeGrid.m_cacheMountPointsB, ref orientationB, ref positionB, defB.Id);
    }

    public static bool CheckMountPointsForSide(
      List<MyCubeBlockDefinition.MountPoint> transormedA,
      ref MyBlockOrientation orientationA,
      ref Vector3I positionA,
      MyDefinitionId idA,
      ref Vector3I normalA,
      List<MyCubeBlockDefinition.MountPoint> transormedB,
      ref MyBlockOrientation orientationB,
      ref Vector3I positionB,
      MyDefinitionId idB)
    {
      Vector3I vector3I1 = positionB - positionA;
      Vector3I vector3I2 = -normalA;
      for (int index1 = 0; index1 < transormedA.Count; ++index1)
      {
        if (transormedA[index1].Enabled)
        {
          MyCubeBlockDefinition.MountPoint mountPoint1 = transormedA[index1];
          if (!(mountPoint1.Normal != normalA))
          {
            Vector3 vector3_1 = Vector3.Min(mountPoint1.Start, mountPoint1.End);
            Vector3 vector3_2 = Vector3.Max(mountPoint1.Start, mountPoint1.End);
            BoundingBox boundingBox = new BoundingBox(vector3_1 - (Vector3) vector3I1, vector3_2 - (Vector3) vector3I1);
            for (int index2 = 0; index2 < transormedB.Count; ++index2)
            {
              if (transormedB[index2].Enabled)
              {
                MyCubeBlockDefinition.MountPoint mountPoint2 = transormedB[index2];
                if (!(mountPoint2.Normal != vector3I2) && (((int) mountPoint1.ExclusionMask & (int) mountPoint2.PropertiesMask) == 0 && ((int) mountPoint1.PropertiesMask & (int) mountPoint2.ExclusionMask) == 0 || !(idA != idB)))
                {
                  BoundingBox box = new BoundingBox(Vector3.Min(mountPoint2.Start, mountPoint2.End), Vector3.Max(mountPoint2.Start, mountPoint2.End));
                  if (boundingBox.Intersects(box))
                    return true;
                }
              }
            }
          }
        }
      }
      return false;
    }

    private static void ConvertNextGrid(bool placeOnly) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertingObjs)), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MyCubeGrid.ConvertNextPrefab(MyCubeGrid.m_prefabs, placeOnly))), timeoutInMiliseconds: 1000));

    private static void ConvertNextPrefab(List<MyObjectBuilder_CubeGrid[]> prefabs, bool placeOnly)
    {
      if (prefabs.Count > 0)
      {
        MyObjectBuilder_CubeGrid[] prefab = prefabs[0];
        int count = prefabs.Count;
        prefabs.RemoveAt(0);
        if (placeOnly)
        {
          float radius = MyCubeGrid.GetBoundingSphereForGrids(prefab).Radius;
          MyCubeGrid.m_maxDimensionPreviousRow = MathHelper.Max(radius, MyCubeGrid.m_maxDimensionPreviousRow);
          if (prefabs.Count % 4 != 0)
          {
            MyCubeGrid.m_newPositionForPlacedObject.X += 2.0 * (double) radius + 10.0;
          }
          else
          {
            MyCubeGrid.m_newPositionForPlacedObject.X = -(2.0 * (double) radius + 10.0);
            MyCubeGrid.m_newPositionForPlacedObject.Z -= 2.0 * (double) MyCubeGrid.m_maxDimensionPreviousRow + 30.0;
            MyCubeGrid.m_maxDimensionPreviousRow = 0.0f;
          }
          MyCubeGrid.PlacePrefabToWorld(prefab, MySector.MainCamera.Position + MyCubeGrid.m_newPositionForPlacedObject);
          MyCubeGrid.ConvertNextPrefab(MyCubeGrid.m_prefabs, placeOnly);
        }
        else
        {
          List<MyCubeGrid> baseGrids = new List<MyCubeGrid>();
          foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in prefab)
            baseGrids.Add(MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) objectBuilderCubeGrid, false) as MyCubeGrid);
          MyCubeGrid.ExportToObjFile(baseGrids, true, false);
          foreach (MyEntity myEntity in baseGrids)
            myEntity.Close();
        }
      }
      else
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertToObjDone))));
    }

    private static BoundingSphere GetBoundingSphereForGrids(
      MyObjectBuilder_CubeGrid[] currentPrefab)
    {
      BoundingSphere boundingSphere1 = new BoundingSphere(Vector3.Zero, float.MinValue);
      foreach (MyObjectBuilder_CubeGrid grid in currentPrefab)
      {
        BoundingSphere boundingSphere2 = grid.CalculateBoundingSphere();
        MatrixD matrixD = grid.PositionAndOrientation.HasValue ? grid.PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity;
        boundingSphere1.Include(boundingSphere2.Transform((Matrix) ref matrixD));
      }
      return boundingSphere1;
    }

    public static void StartConverting(bool placeOnly)
    {
      string path = Path.Combine(MyFileSystem.UserDataPath, "SourceModels");
      if (!Directory.Exists(path))
        return;
      MyCubeGrid.m_prefabs.Clear();
      foreach (string file1 in Directory.GetFiles(path, "*.zip"))
      {
        foreach (string file2 in MyFileSystem.GetFiles(file1, "*.sbc", MySearchOption.AllDirectories))
        {
          if (MyFileSystem.FileExists(file2))
          {
            MyObjectBuilder_Definitions objectBuilder = (MyObjectBuilder_Definitions) null;
            MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(file2, out objectBuilder);
            if (objectBuilder.Prefabs[0].CubeGrids != null)
              MyCubeGrid.m_prefabs.Add(objectBuilder.Prefabs[0].CubeGrids);
          }
        }
      }
      MyCubeGrid.ConvertNextPrefab(MyCubeGrid.m_prefabs, placeOnly);
    }

    public static void ConvertPrefabsToObjs() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertingObjs)), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MyCubeGrid.StartConverting(false))), timeoutInMiliseconds: 1000));

    public static void PackFiles(string path, string objectName)
    {
      if (!Directory.Exists(path))
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjFailed), (object) path))));
      }
      else
      {
        using (MyZipArchive arc = MyZipArchive.OpenOnFile(Path.Combine(path, objectName + "_objFiles.zip"), ZipArchiveMode.Create))
        {
          MyCubeGrid.PackFilesToDirectory(path, "*.png", arc);
          MyCubeGrid.PackFilesToDirectory(path, "*.obj", arc);
          MyCubeGrid.PackFilesToDirectory(path, "*.mtl", arc);
        }
        using (MyZipArchive arc = MyZipArchive.OpenOnFile(Path.Combine(path, objectName + ".zip"), ZipArchiveMode.Create))
        {
          MyCubeGrid.PackFilesToDirectory(path, objectName + ".png", arc);
          MyCubeGrid.PackFilesToDirectory(path, "*.sbc", arc);
        }
        MyCubeGrid.RemoveFilesFromDirectory(path, "*.png");
        MyCubeGrid.RemoveFilesFromDirectory(path, "*.sbc");
        MyCubeGrid.RemoveFilesFromDirectory(path, "*.obj");
        MyCubeGrid.RemoveFilesFromDirectory(path, "*.mtl");
      }
    }

    private static void RemoveFilesFromDirectory(string path, string fileType)
    {
      foreach (string file in Directory.GetFiles(path, fileType))
        File.Delete(file);
    }

    private static void PackFilesToDirectory(string path, string searchString, MyZipArchive arc)
    {
      int startIndex = path.Length + 1;
      foreach (string file in Directory.GetFiles(path, searchString, SearchOption.AllDirectories))
      {
        using (FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          using (Stream stream = arc.AddFile(file.Substring(startIndex), CompressionLevel.Optimal).GetStream())
            fileStream.CopyTo(stream, 4096);
        }
      }
    }

    public static void ExportObject(
      MyCubeGrid baseGrid,
      bool convertModelsFromSBC,
      bool exportObjAndSBC = false)
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ExportingToObj)), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        List<MyCubeGrid> baseGrids = new List<MyCubeGrid>();
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(baseGrid).Nodes)
          baseGrids.Add(node.NodeData);
        MyCubeGrid.ExportToObjFile(baseGrids, convertModelsFromSBC, exportObjAndSBC);
      })), timeoutInMiliseconds: 1000));
    }

    private static void ExportToObjFile(
      List<MyCubeGrid> baseGrids,
      bool convertModelsFromSBC,
      bool exportObjAndSBC)
    {
      MyCubeGrid.materialID = 0;
      MyValueFormatter.GetFormatedDateTimeForFilename(DateTime.Now);
      string name = MyUtils.StripInvalidChars(baseGrids[0].DisplayName.Replace(' ', '_'));
      string path1 = MyFileSystem.UserDataPath;
      string path2 = "ExportedModels";
      if (!convertModelsFromSBC | exportObjAndSBC)
      {
        path1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        path2 = MyPerGameSettings.GameNameSafe + "_ExportedModels";
      }
      string folder = Path.Combine(path1, path2, name);
      int num = 0;
      for (; Directory.Exists(folder); folder = Path.Combine(path1, path2, string.Format("{0}_{1:000}", (object) name, (object) num)))
        ++num;
      MyUtils.CreateFolder(folder);
      if (!convertModelsFromSBC | exportObjAndSBC)
      {
        bool flag = false;
        string prefabPath = Path.Combine(folder, name + ".sbc");
        foreach (MyCubeGrid baseGrid in baseGrids)
        {
          foreach (MySlimBlock cubeBlock in baseGrid.CubeBlocks)
          {
            if (!cubeBlock.BlockDefinition.Context.IsBaseGame)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
        {
          MyCubeGrid.CreatePrefabFile(baseGrids, name, prefabPath);
          MyRenderProxy.TakeScreenshot(MyCubeGrid.tumbnailMultiplier, Path.Combine(folder, name + ".png"), false, true, false);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjComplete), (object) folder)), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MyCubeGrid.PackFiles(folder, name)))));
        }
        else
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjModded), (object) folder))));
      }
      if (!(exportObjAndSBC | convertModelsFromSBC))
        return;
      List<Vector3> vertices = new List<Vector3>();
      List<MyCubeGrid.TriangleWithMaterial> triangles = new List<MyCubeGrid.TriangleWithMaterial>();
      List<Vector2> uvs = new List<Vector2>();
      List<MyExportModel.Material> materials = new List<MyExportModel.Material>();
      int currVerticesCount = 0;
      try
      {
        MyCubeGrid.GetModelDataFromGrid(baseGrids, vertices, triangles, uvs, materials, currVerticesCount);
        string filename = Path.Combine(folder, name + ".obj");
        string matFilename = Path.Combine(folder, name + ".mtl");
        MyCubeGrid.CreateObjFile(name, filename, matFilename, vertices, triangles, uvs, materials, currVerticesCount);
        List<renderColoredTextureProperties> texturesToRender = new List<renderColoredTextureProperties>();
        MyCubeGrid.CreateMaterialFile(folder, matFilename, materials, texturesToRender);
        if (texturesToRender.Count > 0)
          MyRenderProxy.RenderColoredTextures(texturesToRender);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjComplete), (object) folder)), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MyCubeGrid.ConvertNextGrid(false))), timeoutInMiliseconds: 1000));
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine("Error while exporting to obj file.");
        MySandboxGame.Log.WriteLine(ex.ToString());
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjFailed), (object) folder))));
      }
    }

    private static void CreatePrefabFile(List<MyCubeGrid> baseGrid, string name, string prefabPath)
    {
      Vector2I bufferResolution = MyRenderProxy.BackBufferResolution;
      MyCubeGrid.tumbnailMultiplier.X = 400f / (float) bufferResolution.X;
      MyCubeGrid.tumbnailMultiplier.Y = 400f / (float) bufferResolution.Y;
      List<MyObjectBuilder_CubeGrid> copiedPrefab = new List<MyObjectBuilder_CubeGrid>();
      foreach (MyCubeGrid myCubeGrid in baseGrid)
        copiedPrefab.Add((MyObjectBuilder_CubeGrid) myCubeGrid.GetObjectBuilder(false));
      MyPrefabManager.SavePrefabToPath(name, prefabPath, copiedPrefab);
    }

    private static void GetModelDataFromGrid(
      List<MyCubeGrid> baseGrid,
      List<Vector3> vertices,
      List<MyCubeGrid.TriangleWithMaterial> triangles,
      List<Vector2> uvs,
      List<MyExportModel.Material> materials,
      int currVerticesCount)
    {
      MatrixD matrixD1 = MatrixD.Invert(baseGrid[0].WorldMatrix);
      foreach (MyCubeGrid myCubeGrid in baseGrid)
      {
        MatrixD matrixD2 = myCubeGrid.WorldMatrix * matrixD1;
        foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in myCubeGrid.RenderData.Cells)
        {
          foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
          {
            MyCubePart key = cubePart.Key;
            Vector3 colorMaskHSV = new Vector3(key.InstanceData.ColorMaskHSV.X, key.InstanceData.ColorMaskHSV.Y, key.InstanceData.ColorMaskHSV.Z);
            Vector2 offsetUV = new Vector2(key.InstanceData.GetTextureOffset(0), key.InstanceData.GetTextureOffset(1));
            MyCubeGrid.ExtractModelDataForObj(key.Model, key.InstanceData.LocalMatrix * (Matrix) ref matrixD2, vertices, triangles, uvs, ref offsetUV, materials, ref currVerticesCount, colorMaskHSV);
          }
        }
        foreach (MySlimBlock block in myCubeGrid.GetBlocks())
        {
          if (block.FatBlock != null)
          {
            if (block.FatBlock is MyPistonBase)
              block.FatBlock.UpdateOnceBeforeFrame();
            else if (block.FatBlock is MyCompoundCubeBlock)
            {
              using (List<MySlimBlock>.Enumerator enumerator = (block.FatBlock as MyCompoundCubeBlock).GetBlocks().GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  MySlimBlock current = enumerator.Current;
                  MyModel model = current.FatBlock.Model;
                  MatrixD matrixD3 = current.FatBlock.PositionComp.WorldMatrixRef * matrixD1;
                  Matrix matrix = (Matrix) ref matrixD3;
                  List<Vector3> vertices1 = vertices;
                  List<MyCubeGrid.TriangleWithMaterial> triangles1 = triangles;
                  List<Vector2> uvs1 = uvs;
                  ref Vector2 local1 = ref Vector2.Zero;
                  List<MyExportModel.Material> materials1 = materials;
                  ref int local2 = ref currVerticesCount;
                  Vector3 colorMaskHsv1 = current.ColorMaskHSV;
                  MyCubeGrid.ExtractModelDataForObj(model, matrix, vertices1, triangles1, uvs1, ref local1, materials1, ref local2, colorMaskHsv1);
                  List<Vector3> vertices2 = vertices;
                  List<MyCubeGrid.TriangleWithMaterial> triangles2 = triangles;
                  List<Vector2> uvs2 = uvs;
                  List<MyExportModel.Material> materials2 = materials;
                  ref int local3 = ref currVerticesCount;
                  MatrixD matrixD4 = current.FatBlock.PositionComp.WorldMatrixRef * matrixD1;
                  Matrix parentMatrix = (Matrix) ref matrixD4;
                  Vector3 colorMaskHsv2 = current.ColorMaskHSV;
                  ListReader<MyHierarchyComponentBase> children = current.FatBlock.Hierarchy.Children;
                  MyCubeGrid.ProcessChildrens(vertices2, triangles2, uvs2, materials2, ref local3, parentMatrix, colorMaskHsv2, children);
                }
                continue;
              }
            }
            MyModel model1 = block.FatBlock.Model;
            MatrixD matrixD5 = block.FatBlock.PositionComp.WorldMatrixRef * matrixD1;
            Matrix matrix1 = (Matrix) ref matrixD5;
            List<Vector3> vertices3 = vertices;
            List<MyCubeGrid.TriangleWithMaterial> triangles3 = triangles;
            List<Vector2> uvs3 = uvs;
            ref Vector2 local4 = ref Vector2.Zero;
            List<MyExportModel.Material> materials3 = materials;
            ref int local5 = ref currVerticesCount;
            Vector3 colorMaskHsv3 = block.ColorMaskHSV;
            MyCubeGrid.ExtractModelDataForObj(model1, matrix1, vertices3, triangles3, uvs3, ref local4, materials3, ref local5, colorMaskHsv3);
            List<Vector3> vertices4 = vertices;
            List<MyCubeGrid.TriangleWithMaterial> triangles4 = triangles;
            List<Vector2> uvs4 = uvs;
            List<MyExportModel.Material> materials4 = materials;
            ref int local6 = ref currVerticesCount;
            MatrixD matrixD6 = block.FatBlock.PositionComp.WorldMatrixRef * matrixD1;
            Matrix parentMatrix1 = (Matrix) ref matrixD6;
            Vector3 colorMaskHsv4 = block.ColorMaskHSV;
            ListReader<MyHierarchyComponentBase> children1 = block.FatBlock.Hierarchy.Children;
            MyCubeGrid.ProcessChildrens(vertices4, triangles4, uvs4, materials4, ref local6, parentMatrix1, colorMaskHsv4, children1);
          }
        }
      }
    }

    private static void CreateObjFile(
      string name,
      string filename,
      string matFilename,
      List<Vector3> vertices,
      List<MyCubeGrid.TriangleWithMaterial> triangles,
      List<Vector2> uvs,
      List<MyExportModel.Material> materials,
      int currVerticesCount)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
      {
        streamWriter.WriteLine("mtllib {0}", (object) Path.GetFileName(matFilename));
        streamWriter.WriteLine();
        streamWriter.WriteLine("#");
        streamWriter.WriteLine("# {0}", (object) name);
        streamWriter.WriteLine("#");
        streamWriter.WriteLine();
        streamWriter.WriteLine("# vertices");
        List<int> intList1 = new List<int>(vertices.Count);
        Dictionary<Vector3D, int> dictionary1 = new Dictionary<Vector3D, int>(vertices.Count / 5);
        int num1 = 1;
        foreach (Vector3 vertex in vertices)
        {
          int num2;
          if (!dictionary1.TryGetValue((Vector3D) vertex, out num2))
          {
            num2 = num1++;
            dictionary1.Add((Vector3D) vertex, num2);
            streamWriter.WriteLine("v {0} {1} {2}", (object) vertex.X, (object) vertex.Y, (object) vertex.Z);
          }
          intList1.Add(num2);
        }
        List<int> intList2 = new List<int>(vertices.Count);
        Dictionary<Vector2, int> dictionary2 = new Dictionary<Vector2, int>(vertices.Count / 5);
        streamWriter.WriteLine("# {0} vertices", (object) vertices.Count);
        streamWriter.WriteLine();
        streamWriter.WriteLine("# texture coordinates");
        int num3 = 1;
        foreach (Vector2 uv in uvs)
        {
          int num2;
          if (!dictionary2.TryGetValue(uv, out num2))
          {
            num2 = num3++;
            dictionary2.Add(uv, num2);
            streamWriter.WriteLine("vt {0} {1}", (object) uv.X, (object) uv.Y);
          }
          intList2.Add(num2);
        }
        streamWriter.WriteLine("# {0} texture coords", (object) uvs.Count);
        streamWriter.WriteLine();
        streamWriter.WriteLine("# faces");
        streamWriter.WriteLine("o {0}", (object) name);
        int num4 = 0;
        foreach (MyExportModel.Material material in materials)
        {
          ++num4;
          string exportedMaterialName = material.ExportedMaterialName;
          streamWriter.WriteLine();
          streamWriter.WriteLine("g {0}_part{1}", (object) name, (object) num4);
          streamWriter.WriteLine("usemtl {0}", (object) exportedMaterialName);
          streamWriter.WriteLine("s off");
          for (int index = 0; index < triangles.Count; ++index)
          {
            if (exportedMaterialName == triangles[index].material)
            {
              MyCubeGrid.TriangleWithMaterial triangle1 = triangles[index];
              MyTriangleVertexIndices triangle2 = triangle1.triangle;
              MyTriangleVertexIndices uvIndices = triangle1.uvIndices;
              streamWriter.WriteLine("f {0}/{3} {1}/{4} {2}/{5}", (object) intList1[triangle2.I0 - 1], (object) intList1[triangle2.I1 - 1], (object) intList1[triangle2.I2 - 1], (object) intList2[uvIndices.I0 - 1], (object) intList2[uvIndices.I1 - 1], (object) intList2[uvIndices.I2 - 1]);
            }
          }
        }
        streamWriter.WriteLine("# {0} faces", (object) triangles.Count);
      }
    }

    private static void CreateMaterialFile(
      string folder,
      string matFilename,
      List<MyExportModel.Material> materials,
      List<renderColoredTextureProperties> texturesToRender)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      using (StreamWriter streamWriter = new StreamWriter(matFilename))
      {
        foreach (MyExportModel.Material material in materials)
        {
          string exportedMaterialName = material.ExportedMaterialName;
          streamWriter.WriteLine("newmtl {0}", (object) exportedMaterialName);
          if (MyFakes.ENABLE_EXPORT_MTL_DIAGNOSTICS)
          {
            streamWriter.WriteLine("# HSV Mask: {0}", (object) material.ColorMaskHSV.ToString("F2"));
            streamWriter.WriteLine("# IsGlass: {0}", (object) material.IsGlass);
            streamWriter.WriteLine("# AddMapsMap: {0}", (object) (material.AddMapsTexture ?? "Null"));
            streamWriter.WriteLine("# AlphamaskMap: {0}", (object) (material.AlphamaskTexture ?? "Null"));
            streamWriter.WriteLine("# ColorMetalMap: {0}", (object) (material.ColorMetalTexture ?? "Null"));
            streamWriter.WriteLine("# NormalGlossMap: {0}", (object) (material.NormalGlossTexture ?? "Null"));
          }
          if (!material.IsGlass)
          {
            streamWriter.WriteLine("Ka 1.000 1.000 1.000");
            streamWriter.WriteLine("Kd 1.000 1.000 1.000");
            streamWriter.WriteLine("Ks 0.100 0.100 0.100");
            streamWriter.WriteLine(material.AlphamaskTexture == null ? "d 1.0" : "d 0.0");
          }
          else
          {
            streamWriter.WriteLine("Ka 0.000 0.000 0.000");
            streamWriter.WriteLine("Kd 0.000 0.000 0.000");
            streamWriter.WriteLine("Ks 0.900 0.900 0.900");
            streamWriter.WriteLine("d 0.350");
          }
          streamWriter.WriteLine("Ns 95.00");
          streamWriter.WriteLine("illum 2");
          if (material.ColorMetalTexture != null)
          {
            string format = exportedMaterialName + "_{0}.png";
            string path2_1 = string.Format(format, (object) "ca");
            string path2_2 = string.Format(format, (object) "ng");
            streamWriter.WriteLine("map_Ka {0}", (object) path2_1);
            streamWriter.WriteLine("map_Kd {0}", (object) path2_1);
            if (material.AlphamaskTexture != null)
              streamWriter.WriteLine("map_d {0}", (object) path2_1);
            bool flag = false;
            if (material.NormalGlossTexture != null)
            {
              string str;
              if (dictionary.TryGetValue(material.NormalGlossTexture, out str))
              {
                path2_2 = str;
              }
              else
              {
                flag = true;
                dictionary.Add(material.NormalGlossTexture, path2_2);
              }
              streamWriter.WriteLine("map_Bump {0}", (object) path2_2);
            }
            texturesToRender.Add(new renderColoredTextureProperties()
            {
              ColorMaskHSV = material.ColorMaskHSV,
              TextureAddMaps = material.AddMapsTexture,
              TextureAplhaMask = material.AlphamaskTexture,
              TextureColorMetal = material.ColorMetalTexture,
              TextureNormalGloss = flag ? material.NormalGlossTexture : (string) null,
              PathToSave_ColorAlpha = Path.Combine(folder, path2_1),
              PathToSave_NormalGloss = Path.Combine(folder, path2_2)
            });
          }
          streamWriter.WriteLine();
        }
      }
    }

    private static void ProcessChildrens(
      List<Vector3> vertices,
      List<MyCubeGrid.TriangleWithMaterial> triangles,
      List<Vector2> uvs,
      List<MyExportModel.Material> materials,
      ref int currVerticesCount,
      Matrix parentMatrix,
      Vector3 HSV,
      ListReader<MyHierarchyComponentBase> childrens)
    {
      foreach (MyEntityComponentBase children in childrens)
      {
        VRage.ModAPI.IMyEntity entity = children.Container.Entity;
        MyModel model = (entity as MyEntity).Model;
        if (model != null)
          MyCubeGrid.ExtractModelDataForObj(model, entity.LocalMatrix * parentMatrix, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, HSV);
        MyCubeGrid.ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, entity.LocalMatrix * parentMatrix, HSV, entity.Hierarchy.Children);
      }
    }

    public static void PlacePrefabsToWorld()
    {
      MyCubeGrid.m_newPositionForPlacedObject = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.PlacingObjectsToScene)), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MyCubeGrid.StartConverting(true))), timeoutInMiliseconds: 1000));
    }

    public static void PlacePrefabToWorld(
      MyObjectBuilder_CubeGrid[] currentPrefab,
      Vector3D position,
      List<MyCubeGrid> createdGrids = null)
    {
      Vector3D vector3D1 = Vector3D.Zero;
      Vector3D vector3D2 = Vector3D.Zero;
      bool flag = true;
      MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) currentPrefab);
      foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in currentPrefab)
      {
        if (objectBuilderCubeGrid.PositionAndOrientation.HasValue)
        {
          if (flag)
          {
            vector3D2 = position - (Vector3D) objectBuilderCubeGrid.PositionAndOrientation.Value.Position;
            flag = false;
            vector3D1 = position;
          }
          else
            vector3D1 = (Vector3D) objectBuilderCubeGrid.PositionAndOrientation.Value.Position + vector3D2;
        }
        MyPositionAndOrientation positionAndOrientation = objectBuilderCubeGrid.PositionAndOrientation.Value;
        positionAndOrientation.Position = (SerializableVector3D) vector3D1;
        objectBuilderCubeGrid.PositionAndOrientation = new MyPositionAndOrientation?(positionAndOrientation);
        if (MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) objectBuilderCubeGrid, false) is MyCubeGrid fromObjectBuilder)
        {
          fromObjectBuilder.ClearSymmetries();
          fromObjectBuilder.Physics.LinearVelocity = (Vector3) Vector3D.Zero;
          fromObjectBuilder.Physics.AngularVelocity = (Vector3) Vector3D.Zero;
          createdGrids?.Add(fromObjectBuilder);
          MyEntities.Add((MyEntity) fromObjectBuilder);
        }
      }
    }

    public static MyCubeGrid GetTargetGrid() => ((MyEntity) MyCubeBuilder.Static.FindClosestGrid() ?? MyCubeGrid.GetTargetEntity()) as MyCubeGrid;

    public static MyEntity GetTargetEntity()
    {
      LineD ray = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 10000f);
      MyCubeGrid.m_tmpHitList.AssertEmpty<MyPhysics.HitInfo>();
      try
      {
        MyPhysics.CastRay(ray.From, ray.To, MyCubeGrid.m_tmpHitList, 15);
        MyCubeGrid.m_tmpHitList.RemoveAll((Predicate<MyPhysics.HitInfo>) (hit => MySession.Static.ControlledEntity != null && hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity));
        if (MyCubeGrid.m_tmpHitList.Count != 0)
          return MyCubeGrid.m_tmpHitList[0].HkHitInfo.GetHitEntity() as MyEntity;
        using (MyUtils.ReuseCollection<MyLineSegmentOverlapResult<MyEntity>>(ref MyCubeGrid.m_lineOverlapList))
        {
          MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, MyCubeGrid.m_lineOverlapList);
          return MyCubeGrid.m_lineOverlapList.Count > 0 ? MyCubeGrid.m_lineOverlapList[0].Element.GetTopMostParent((System.Type) null) : (MyEntity) null;
        }
      }
      finally
      {
        MyCubeGrid.m_tmpHitList.Clear();
      }
    }

    public static bool TryRayCastGrid(
      ref LineD worldRay,
      out MyCubeGrid hitGrid,
      out Vector3D worldHitPos)
    {
      try
      {
        MyPhysics.CastRay(worldRay.From, worldRay.To, MyCubeGrid.m_tmpHitList);
        foreach (MyPhysics.HitInfo mTmpHit in MyCubeGrid.m_tmpHitList)
        {
          if (mTmpHit.HkHitInfo.GetHitEntity() is MyCubeGrid hitEntity)
          {
            worldHitPos = mTmpHit.Position;
            MyRenderProxy.DebugDrawAABB(new BoundingBoxD(worldHitPos - 0.01, worldHitPos + 0.01), (Color) Color.Wheat.ToVector3());
            hitGrid = hitEntity;
            return true;
          }
        }
        hitGrid = (MyCubeGrid) null;
        worldHitPos = new Vector3D();
        return false;
      }
      finally
      {
        MyCubeGrid.m_tmpHitList.Clear();
      }
    }

    public static bool TestBlockPlacementArea(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      MyBlockOrientation blockOrientation,
      MyCubeBlockDefinition blockDefinition,
      ref Vector3D translation,
      ref Quaternion rotation,
      ref Vector3 halfExtents,
      ref BoundingBoxD localAabb,
      ulong placingPlayer = 0,
      MyEntity ignoredEntity = null,
      bool isProjected = false)
    {
      return MyCubeGrid.TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translation, ref rotation, ref halfExtents, ref localAabb, out MyCubeGrid _, placingPlayer, ignoredEntity, isProjected: isProjected);
    }

    public static bool TestBlockPlacementArea(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      MyBlockOrientation blockOrientation,
      MyCubeBlockDefinition blockDefinition,
      ref Vector3D translationObsolete,
      ref Quaternion rotation,
      ref Vector3 halfExtentsObsolete,
      ref BoundingBoxD localAabb,
      out MyCubeGrid touchingGrid,
      ulong placingPlayer = 0,
      MyEntity ignoredEntity = null,
      bool ignoreFracturedPieces = false,
      bool testVoxel = true,
      bool isProjected = false)
    {
      touchingGrid = (MyCubeGrid) null;
      MatrixD matrix = targetGrid != null ? targetGrid.WorldMatrix : MatrixD.Identity;
      if (!MyEntities.IsInsideWorld(matrix.Translation))
        return false;
      Vector3 halfExtents = (Vector3) localAabb.HalfExtents + settings.SearchHalfExtentsDeltaAbsolute;
      if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
        halfExtents = (Vector3) (halfExtents - new Vector3D(0.11));
      Vector3D center = localAabb.TransformFast(ref matrix).Center;
      Quaternion.CreateFromRotationMatrix(in matrix).Normalize();
      if (testVoxel && settings.VoxelPlacement.HasValue && settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
      {
        bool flag = MyCubeGrid.IsAabbInsideVoxel(matrix, localAabb, settings);
        if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
          flag = !flag;
        if (flag)
          return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed(localAabb.TransformFast(ref matrix), isProjected ? MySafeZoneAction.BuildingProjections : MySafeZoneAction.Building, user: placingPlayer))
        return false;
      if (blockDefinition != null && blockDefinition.UseModelIntersection)
      {
        MyModel modelOnlyData = MyModels.GetModelOnlyData(blockDefinition.Model);
        if (modelOnlyData != null)
        {
          bool errorFound;
          modelOnlyData.CheckLoadingErrors(blockDefinition.Context, out errorFound);
          if (errorFound)
            MyDefinitionErrors.Add(blockDefinition.Context, "There was error during loading of model, please check log file.", TErrorSeverity.Error);
        }
        if (modelOnlyData != null && modelOnlyData.HavokCollisionShapes != null)
        {
          Matrix result1;
          blockOrientation.GetMatrix(out result1);
          Vector3 result2;
          Vector3.TransformNormal(ref blockDefinition.ModelOffset, ref result1, out result2);
          center += result2;
          int length = modelOnlyData.HavokCollisionShapes.Length;
          HkShape[] shapes = new HkShape[length];
          for (int index = 0; index < length; ++index)
            shapes[index] = modelOnlyData.HavokCollisionShapes[index];
          HkListShape hkListShape = new HkListShape(shapes, length, HkReferencePolicy.None);
          Quaternion fromForwardUp = Quaternion.CreateFromForwardUp(Base6Directions.GetVector(blockOrientation.Forward), Base6Directions.GetVector(blockOrientation.Up));
          rotation *= fromForwardUp;
          MyPhysics.GetPenetrationsShape((HkShape) hkListShape, ref center, ref rotation, MyCubeGrid.m_physicsBoxQueryList, 7);
          hkListShape.Base.RemoveReference();
        }
        else
          MyPhysics.GetPenetrationsBox(ref halfExtents, ref center, ref rotation, MyCubeGrid.m_physicsBoxQueryList, 7);
      }
      else
        MyPhysics.GetPenetrationsBox(ref halfExtents, ref center, ref rotation, MyCubeGrid.m_physicsBoxQueryList, 7);
      MyCubeGrid.m_lastQueryBox.Value.HalfExtents = halfExtents;
      MyCubeGrid.m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
      MyCubeGrid.m_lastQueryTransform.Translation = center;
      return MyCubeGrid.TestPlacementAreaInternal(targetGrid, ref settings, blockDefinition, new MyBlockOrientation?(blockOrientation), ref localAabb, ignoredEntity, ref matrix, out touchingGrid, ignoreFracturedPieces: ignoreFracturedPieces);
    }

    public static bool TestPlacementAreaCube(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      Vector3I min,
      Vector3I max,
      MyBlockOrientation blockOrientation,
      MyCubeBlockDefinition blockDefinition,
      ulong placingPlayer = 0,
      MyEntity ignoredEntity = null,
      bool ignoreFracturedPieces = false,
      bool isProjected = false)
    {
      MyCubeGrid touchingGrid = (MyCubeGrid) null;
      return MyCubeGrid.TestPlacementAreaCube(targetGrid, ref settings, min, max, blockOrientation, blockDefinition, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces, isProjected);
    }

    public static bool TestPlacementAreaCube(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      Vector3I min,
      Vector3I max,
      MyBlockOrientation blockOrientation,
      MyCubeBlockDefinition blockDefinition,
      out MyCubeGrid touchingGrid,
      ulong placingPlayer = 0,
      MyEntity ignoredEntity = null,
      bool ignoreFracturedPieces = false,
      bool isProjected = false)
    {
      touchingGrid = (MyCubeGrid) null;
      MatrixD matrixD = targetGrid != null ? targetGrid.WorldMatrix : MatrixD.Identity;
      if (!MyEntities.IsInsideWorld(matrixD.Translation))
        return false;
      float num = targetGrid != null ? targetGrid.GridSize : MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
      Vector3 vector3 = ((max - min) * num + num) / 2f;
      Vector3 halfExtentsObsolete = !MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA ? vector3 - new Vector3(0.03f, 0.03f, 0.03f) : (Vector3) (vector3 - new Vector3D(0.11));
      MatrixD matrix = MatrixD.CreateTranslation((max + min) * 0.5f * num) * matrixD;
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      invalid.Include((Vector3D) (min * num - num / 2f));
      invalid.Include((Vector3D) (max * num + num / 2f));
      Vector3D translation = matrix.Translation;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      return MyCubeGrid.TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translation, ref fromRotationMatrix, ref halfExtentsObsolete, ref invalid, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces, isProjected: isProjected);
    }

    public static bool TestPlacementAreaCubeNoAABBInflate(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      Vector3I min,
      Vector3I max,
      MyBlockOrientation blockOrientation,
      MyCubeBlockDefinition blockDefinition,
      out MyCubeGrid touchingGrid,
      ulong placingPlayer = 0,
      MyEntity ignoredEntity = null,
      bool isProjected = false)
    {
      touchingGrid = (MyCubeGrid) null;
      MatrixD matrixD = targetGrid != null ? targetGrid.WorldMatrix : MatrixD.Identity;
      if (!MyEntities.IsInsideWorld(matrixD.Translation))
        return false;
      float num = targetGrid != null ? targetGrid.GridSize : MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
      Vector3 halfExtentsObsolete = ((max - min) * num + num) / 2f;
      MatrixD matrix = MatrixD.CreateTranslation((max + min) * 0.5f * num) * matrixD;
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      invalid.Include((Vector3D) (min * num - num / 2f));
      invalid.Include((Vector3D) (max * num + num / 2f));
      Vector3D translation = matrix.Translation;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      return MyCubeGrid.TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translation, ref fromRotationMatrix, ref halfExtentsObsolete, ref invalid, out touchingGrid, placingPlayer, ignoredEntity, isProjected: isProjected);
    }

    public static bool TestPlacementArea(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      BoundingBoxD localAabb,
      bool dynamicBuildMode,
      MyEntity ignoredEntity = null)
    {
      MatrixD matrix = targetGrid.WorldMatrix;
      if (!MyEntities.IsInsideWorld(matrix.Translation))
        return false;
      Vector3 halfExtents = (Vector3) localAabb.HalfExtents + settings.SearchHalfExtentsDeltaAbsolute;
      if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
        halfExtents = (Vector3) (halfExtents - new Vector3D(0.11));
      Vector3D center = localAabb.TransformFast(ref matrix).Center;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      fromRotationMatrix.Normalize();
      MyPhysics.GetPenetrationsBox(ref halfExtents, ref center, ref fromRotationMatrix, MyCubeGrid.m_physicsBoxQueryList, 18);
      MyCubeGrid.m_lastQueryBox.Value.HalfExtents = halfExtents;
      MyCubeGrid.m_lastQueryTransform = MatrixD.CreateFromQuaternion(fromRotationMatrix);
      MyCubeGrid.m_lastQueryTransform.Translation = center;
      return MyCubeGrid.TestPlacementAreaInternal(targetGrid, ref settings, (MyCubeBlockDefinition) null, new MyBlockOrientation?(), ref localAabb, ignoredEntity, ref matrix, out MyCubeGrid _, dynamicBuildMode);
    }

    public static bool TestPlacementArea(
      MyCubeGrid targetGrid,
      bool targetGridIsStatic,
      ref MyGridPlacementSettings settings,
      BoundingBoxD localAabb,
      bool dynamicBuildMode,
      MyEntity ignoredEntity = null,
      bool testVoxel = true,
      bool testPhysics = true)
    {
      MatrixD matrix = targetGrid.WorldMatrix;
      if (!MyEntities.IsInsideWorld(matrix.Translation))
        return false;
      Vector3 halfExtents = (Vector3) localAabb.HalfExtents + settings.SearchHalfExtentsDeltaAbsolute;
      if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
        halfExtents = (Vector3) (halfExtents - new Vector3D(0.11));
      Vector3D center = localAabb.TransformFast(ref matrix).Center;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      fromRotationMatrix.Normalize();
      if (testVoxel && settings.VoxelPlacement.HasValue && settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
      {
        bool flag = MyCubeGrid.IsAabbInsideVoxel(matrix, localAabb, settings);
        if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
          flag = !flag;
        if (flag)
          return false;
      }
      bool flag1 = true;
      if (testPhysics)
      {
        MyPhysics.GetPenetrationsBox(ref halfExtents, ref center, ref fromRotationMatrix, MyCubeGrid.m_physicsBoxQueryList, 7);
        MyCubeGrid.m_lastQueryBox.Value.HalfExtents = halfExtents;
        MyCubeGrid.m_lastQueryTransform = MatrixD.CreateFromQuaternion(fromRotationMatrix);
        MyCubeGrid.m_lastQueryTransform.Translation = center;
        flag1 = MyCubeGrid.TestPlacementAreaInternal(targetGrid, targetGridIsStatic, ref settings, (MyCubeBlockDefinition) null, new MyBlockOrientation?(), ref localAabb, ignoredEntity, ref matrix, out MyCubeGrid _, dynamicBuildMode);
      }
      return flag1;
    }

    public static bool IsAabbInsideVoxel(
      MatrixD worldMatrix,
      BoundingBoxD localAabb,
      MyGridPlacementSettings settings)
    {
      if (!settings.VoxelPlacement.HasValue)
        return false;
      BoundingBoxD box = localAabb.TransformFast(ref worldMatrix);
      List<MyVoxelBase> result = new List<MyVoxelBase>();
      MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, result);
      foreach (MyVoxelBase voxelMap in result)
      {
        if (settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Volumetric && voxelMap.IsAnyAabbCornerInside(ref worldMatrix, localAabb) || settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.Volumetric && !MyCubeGrid.TestPlacementVoxelMapPenetration(voxelMap, settings, ref localAabb, ref worldMatrix))
          return true;
      }
      return false;
    }

    public static bool TestBlockPlacementArea(
      MyCubeBlockDefinition blockDefinition,
      MyBlockOrientation? blockOrientation,
      MatrixD worldMatrix,
      ref MyGridPlacementSettings settings,
      BoundingBoxD localAabb,
      bool dynamicBuildMode,
      MyEntity ignoredEntity = null,
      bool testVoxel = true)
    {
      if (!MyEntities.IsInsideWorld(worldMatrix.Translation))
        return false;
      Vector3 halfExtents = (Vector3) localAabb.HalfExtents + settings.SearchHalfExtentsDeltaAbsolute;
      if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
        halfExtents = (Vector3) (halfExtents - new Vector3D(0.11));
      Vector3D center = localAabb.TransformFast(ref worldMatrix).Center;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in worldMatrix);
      fromRotationMatrix.Normalize();
      MyGridPlacementSettings settings1 = settings;
      if (dynamicBuildMode && blockDefinition.CubeSize == MyCubeSize.Large)
        settings1.VoxelPlacement = new VoxelPlacementSettings?(new VoxelPlacementSettings()
        {
          PlacementMode = VoxelPlacementMode.Both
        });
      if (testVoxel && !MyCubeGrid.TestVoxelPlacement(blockDefinition, settings1, dynamicBuildMode, worldMatrix, localAabb))
        return false;
      MyPhysics.GetPenetrationsBox(ref halfExtents, ref center, ref fromRotationMatrix, MyCubeGrid.m_physicsBoxQueryList, 7);
      MyCubeGrid.m_lastQueryBox.Value.HalfExtents = halfExtents;
      MyCubeGrid.m_lastQueryTransform = MatrixD.CreateFromQuaternion(fromRotationMatrix);
      MyCubeGrid.m_lastQueryTransform.Translation = center;
      return MyCubeGrid.TestPlacementAreaInternal((MyCubeGrid) null, ref settings1, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref worldMatrix, out MyCubeGrid _, dynamicBuildMode);
    }

    public static bool TestVoxelPlacement(
      MyCubeBlockDefinition blockDefinition,
      MyGridPlacementSettings settingsCopy,
      bool dynamicBuildMode,
      MatrixD worldMatrix,
      BoundingBoxD localAabb)
    {
      if (blockDefinition.VoxelPlacement.HasValue)
        settingsCopy.VoxelPlacement = new VoxelPlacementSettings?(dynamicBuildMode ? blockDefinition.VoxelPlacement.Value.DynamicMode : blockDefinition.VoxelPlacement.Value.StaticMode);
      if (!MyEntities.IsInsideWorld(worldMatrix.Translation) || settingsCopy.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.None)
        return false;
      if (settingsCopy.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
      {
        bool flag = MyCubeGrid.IsAabbInsideVoxel(worldMatrix, localAabb, settingsCopy);
        if (settingsCopy.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
          flag = !flag;
        if (flag)
          return false;
      }
      return true;
    }

    private static void ExtractModelDataForObj(
      MyModel model,
      Matrix matrix,
      List<Vector3> vertices,
      List<MyCubeGrid.TriangleWithMaterial> triangles,
      List<Vector2> uvs,
      ref Vector2 offsetUV,
      List<MyExportModel.Material> materials,
      ref int currVerticesCount,
      Vector3 colorMaskHSV)
    {
      if (!model.HasUV)
      {
        model.LoadUV = true;
        model.UnloadData();
        model.LoadData();
      }
      MyExportModel renderModel = new MyExportModel(model);
      int verticesCount = renderModel.GetVerticesCount();
      List<HalfVector2> uvsForModel = MyCubeGrid.GetUVsForModel(renderModel, verticesCount);
      if (uvsForModel.Count != verticesCount)
        return;
      List<MyExportModel.Material> materialsForModel = MyCubeGrid.CreateMaterialsForModel(materials, colorMaskHSV, renderModel);
      for (int index = 0; index < verticesCount; ++index)
      {
        vertices.Add(Vector3.Transform(renderModel.GetVertex(index), matrix));
        Vector2 vector2 = uvsForModel[index].ToVector2() / renderModel.PatternScale + offsetUV;
        uvs.Add(new Vector2(vector2.X, -vector2.Y));
      }
      for (int index1 = 0; index1 < renderModel.GetTrianglesCount(); ++index1)
      {
        int index2 = -1;
        for (int index3 = 0; index3 < materialsForModel.Count; ++index3)
        {
          if (index1 <= materialsForModel[index3].LastTri)
          {
            index2 = index3;
            break;
          }
        }
        MyTriangleVertexIndices triangle = renderModel.GetTriangle(index1);
        string str = "EmptyMaterial";
        if (index2 != -1)
          str = materialsForModel[index2].ExportedMaterialName;
        triangles.Add(new MyCubeGrid.TriangleWithMaterial()
        {
          material = str,
          triangle = new MyTriangleVertexIndices(triangle.I0 + 1 + currVerticesCount, triangle.I1 + 1 + currVerticesCount, triangle.I2 + 1 + currVerticesCount),
          uvIndices = new MyTriangleVertexIndices(triangle.I0 + 1 + currVerticesCount, triangle.I1 + 1 + currVerticesCount, triangle.I2 + 1 + currVerticesCount)
        });
      }
      currVerticesCount += verticesCount;
    }

    private static List<HalfVector2> GetUVsForModel(
      MyExportModel renderModel,
      int modelVerticesCount)
    {
      HalfVector2[] texCoords = renderModel.GetTexCoords();
      return texCoords == null ? new List<HalfVector2>() : ((IEnumerable<HalfVector2>) texCoords).ToList<HalfVector2>();
    }

    private static List<MyExportModel.Material> CreateMaterialsForModel(
      List<MyExportModel.Material> materials,
      Vector3 colorMaskHSV,
      MyExportModel renderModel)
    {
      List<MyExportModel.Material> materials1 = renderModel.GetMaterials();
      List<MyExportModel.Material> materialList = new List<MyExportModel.Material>(materials1.Count);
      foreach (MyExportModel.Material material1 in materials1)
      {
        MyExportModel.Material? nullable = new MyExportModel.Material?();
        foreach (MyExportModel.Material material2 in materials)
        {
          if ((double) (colorMaskHSV - material2.ColorMaskHSV).AbsMax() < 0.01 && material1.EqualsMaterialWise(material2))
          {
            nullable = new MyExportModel.Material?(material2);
            break;
          }
        }
        MyExportModel.Material material3 = material1;
        material3.ColorMaskHSV = colorMaskHSV;
        if (nullable.HasValue)
        {
          material3.ExportedMaterialName = nullable.Value.ExportedMaterialName;
        }
        else
        {
          ++MyCubeGrid.materialID;
          material3.ExportedMaterialName = "material_" + (object) MyCubeGrid.materialID;
          materials.Add(material3);
        }
        materialList.Add(material3);
      }
      return materialList;
    }

    private static MyCubePart[] GetCubeParts(
      MyStringHash skinSubtypeId,
      MyCubeBlockDefinition block,
      Vector3I position,
      MatrixD rotation,
      float gridSize,
      float gridScale)
    {
      List<string> outModels = new List<string>();
      List<MatrixD> outLocalMatrices = new List<MatrixD>();
      List<Vector3> outLocalNormals = new List<Vector3>();
      List<Vector4UByte> outPatternOffsets = new List<Vector4UByte>();
      MyCubeGrid.GetCubeParts(block, position, (Matrix) ref rotation, gridSize, outModels, outLocalMatrices, outLocalNormals, outPatternOffsets, true);
      MyCubePart[] myCubePartArray = new MyCubePart[outModels.Count];
      for (int index = 0; index < myCubePartArray.Length; ++index)
      {
        MyCubePart myCubePart1 = new MyCubePart();
        MyModel modelOnlyData = MyModels.GetModelOnlyData(outModels[index]);
        modelOnlyData.Rescale(gridScale);
        MyCubePart myCubePart2 = myCubePart1;
        MyModel model = modelOnlyData;
        MyStringHash skinSubtypeId1 = skinSubtypeId;
        MatrixD matrixD = outLocalMatrices[index];
        Matrix matrix = (Matrix) ref matrixD;
        double num = (double) gridScale;
        myCubePart2.Init(model, skinSubtypeId1, matrix, (float) num);
        myCubePart1.InstanceData.SetTextureOffset(outPatternOffsets[index]);
        myCubePartArray[index] = myCubePart1;
      }
      return myCubePartArray;
    }

    private static bool TestPlacementAreaInternal(
      MyCubeGrid targetGrid,
      ref MyGridPlacementSettings settings,
      MyCubeBlockDefinition blockDefinition,
      MyBlockOrientation? blockOrientation,
      ref BoundingBoxD localAabb,
      MyEntity ignoredEntity,
      ref MatrixD worldMatrix,
      out MyCubeGrid touchingGrid,
      bool dynamicBuildMode = false,
      bool ignoreFracturedPieces = false)
    {
      return MyCubeGrid.TestPlacementAreaInternal(targetGrid, targetGrid != null ? targetGrid.IsStatic : !dynamicBuildMode, ref settings, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref worldMatrix, out touchingGrid, dynamicBuildMode, ignoreFracturedPieces);
    }

    private static bool TestPlacementAreaInternalWithEntities(
      MyCubeGrid targetGrid,
      bool targetGridIsStatic,
      ref MyGridPlacementSettings settings,
      ref BoundingBoxD localAabb,
      MyEntity ignoredEntity,
      ref MatrixD worldMatrix,
      bool dynamicBuildMode = false)
    {
      MyCubeGrid touchingGrid = (MyCubeGrid) null;
      float gridSize = targetGrid.GridSize;
      bool isStatic = targetGridIsStatic;
      localAabb.TransformFast(ref worldMatrix);
      bool entityOverlap = false;
      bool touchingStaticGrid = false;
      foreach (MyEntity mTmpResult in MyCubeGrid.m_tmpResultList)
      {
        if ((ignoredEntity == null || mTmpResult != ignoredEntity && mTmpResult.GetTopMostParent((System.Type) null) != ignoredEntity) && mTmpResult.Physics != null)
        {
          if (mTmpResult is MyCubeGrid grid)
          {
            if (isStatic != grid.IsStatic || (double) gridSize == (double) grid.GridSize)
            {
              MyCubeGrid.TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, gridSize, isStatic, ref localAabb, (MyCubeBlockDefinition) null, new MyBlockOrientation?(), ref entityOverlap, ref touchingStaticGrid, grid);
              if (entityOverlap)
                break;
            }
          }
          else if (mTmpResult is MyCharacter myCharacter && myCharacter.PositionComp.WorldAABB.Intersects(targetGrid.PositionComp.WorldAABB))
          {
            entityOverlap = true;
            break;
          }
        }
      }
      MyCubeGrid.m_tmpResultList.Clear();
      if (entityOverlap)
        return false;
      int num = targetGrid.IsStatic ? 1 : 0;
      return true;
    }

    private static void TestGridPlacement(
      ref MyGridPlacementSettings settings,
      ref MatrixD worldMatrix,
      ref MyCubeGrid touchingGrid,
      float gridSize,
      bool isStatic,
      ref BoundingBoxD localAABB,
      MyCubeBlockDefinition blockDefinition,
      MyBlockOrientation? blockOrientation,
      ref bool entityOverlap,
      ref bool touchingStaticGrid,
      MyCubeGrid grid)
    {
      BoundingBoxD boundingBoxD = localAABB.TransformFast(ref worldMatrix);
      MatrixD m = grid.PositionComp.WorldMatrixNormalizedInv;
      boundingBoxD.TransformFast(ref m);
      Vector3D position1 = Vector3D.Transform(localAABB.Min, worldMatrix);
      Vector3D position2 = Vector3D.Transform(localAABB.Max, worldMatrix);
      Vector3D vector3D1 = Vector3D.Transform(position1, m);
      MatrixD matrix = m;
      Vector3D vector3D2 = Vector3D.Transform(position2, matrix);
      Vector3D vector3D3 = Vector3D.Min(vector3D1, vector3D2);
      Vector3D vector3D4 = Vector3D.Max(vector3D1, vector3D2);
      Vector3D vector3D5 = (vector3D3 + gridSize / 2f) / (double) grid.GridSize;
      double num = (double) gridSize / 2.0;
      Vector3D vector3D6 = (vector3D4 - num) / (double) grid.GridSize;
      Vector3I vector3I1 = Vector3I.Round(vector3D5);
      Vector3I vector3I2 = Vector3I.Round(vector3D6);
      Vector3I min = Vector3I.Min(vector3I1, vector3I2);
      Vector3I max = Vector3I.Max(vector3I1, vector3I2);
      MyBlockOrientation? orientation = new MyBlockOrientation?();
      if (MyFakes.ENABLE_COMPOUND_BLOCKS & isStatic && grid.IsStatic && blockOrientation.HasValue)
      {
        Matrix result;
        blockOrientation.Value.GetMatrix(out result);
        MatrixD matrixD1 = result * worldMatrix;
        MatrixD matrixD2 = (Matrix) ref matrixD1 * m;
        Matrix rotation = (Matrix) ref matrixD2;
        rotation.Translation = Vector3.Zero;
        Base6Directions.Direction forward = Base6Directions.GetForward(ref rotation);
        Base6Directions.Direction up = Base6Directions.GetUp(ref rotation);
        if (Base6Directions.IsValidBlockOrientation(forward, up))
          orientation = new MyBlockOrientation?(new MyBlockOrientation(forward, up));
      }
      if (!grid.CanAddCubes(min, max, orientation, blockDefinition))
      {
        entityOverlap = true;
      }
      else
      {
        if (!settings.CanAnchorToStaticGrid || !grid.IsTouchingAnyNeighbor(min, max))
          return;
        touchingStaticGrid = true;
        if (touchingGrid != null)
          return;
        touchingGrid = grid;
      }
    }

    private static bool TestPlacementAreaInternal(
      MyCubeGrid targetGrid,
      bool targetGridIsStatic,
      ref MyGridPlacementSettings settings,
      MyCubeBlockDefinition blockDefinition,
      MyBlockOrientation? blockOrientation,
      ref BoundingBoxD localAabb,
      MyEntity ignoredEntity,
      ref MatrixD worldMatrix,
      out MyCubeGrid touchingGrid,
      bool dynamicBuildMode = false,
      bool ignoreFracturedPieces = false)
    {
      touchingGrid = (MyCubeGrid) null;
      float gridSize = targetGrid != null ? targetGrid.GridSize : (blockDefinition != null ? MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize) : MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large));
      bool isStatic = targetGridIsStatic;
      bool entityOverlap = false;
      bool touchingStaticGrid = false;
      foreach (HkBodyCollision mPhysicsBoxQuery in MyCubeGrid.m_physicsBoxQueryList)
      {
        if (mPhysicsBoxQuery.Body.GetEntity(0U) is MyEntity entity && entity.GetTopMostParent((System.Type) null).GetPhysicsBody() != null && (!ignoreFracturedPieces || !(entity is MyFracturedPiece)) && (!(entity.GetTopMostParent((System.Type) null).GetPhysicsBody().WeldInfo.Children.Count == 0 & ignoredEntity != null) || entity != ignoredEntity && !MyCubeGrid.ShouldEntityBeIgnored(ignoredEntity, entity)))
        {
          MyPhysicsComponentBase physics = entity.GetTopMostParent((System.Type) null).Physics;
          if (physics == null || !physics.IsPhantom)
          {
            MyCubeGrid topMostParent = entity.GetTopMostParent((System.Type) null) as MyCubeGrid;
            if (entity.GetTopMostParent((System.Type) null).GetPhysicsBody().WeldInfo.Children.Count > 0)
            {
              if (entity != ignoredEntity && MyCubeGrid.TestQueryIntersection(entity.GetPhysicsBody().GetShape(), entity.WorldMatrix))
              {
                entityOverlap = true;
                if (touchingGrid == null)
                {
                  touchingGrid = entity as MyCubeGrid;
                  break;
                }
                break;
              }
              foreach (MyPhysicsBody child in entity.GetPhysicsBody().WeldInfo.Children)
              {
                if (child.Entity != ignoredEntity && MyCubeGrid.TestQueryIntersection(child.WeldedRigidBody.GetShape(), child.Entity.WorldMatrix))
                {
                  if (touchingGrid == null)
                    touchingGrid = child.Entity as MyCubeGrid;
                  entityOverlap = true;
                  break;
                }
              }
              if (entityOverlap)
                break;
            }
            else if (topMostParent != null && (isStatic && topMostParent.IsStatic || MyFakes.ENABLE_DYNAMIC_SMALL_GRID_MERGING && !isStatic && (!topMostParent.IsStatic && blockDefinition != null) && blockDefinition.CubeSize == topMostParent.GridSizeEnum || isStatic && topMostParent.IsStatic && (blockDefinition != null && blockDefinition.CubeSize == topMostParent.GridSizeEnum)))
            {
              if (isStatic != topMostParent.IsStatic || (double) gridSize == (double) topMostParent.GridSize)
              {
                if (!MyCubeGrid.IsOrientationsAligned(topMostParent.WorldMatrix, worldMatrix))
                {
                  entityOverlap = true;
                }
                else
                {
                  MyCubeGrid.TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, gridSize, isStatic, ref localAabb, blockDefinition, blockOrientation, ref entityOverlap, ref touchingStaticGrid, topMostParent);
                  if (entityOverlap)
                    break;
                }
              }
            }
            else
            {
              entityOverlap = true;
              break;
            }
          }
        }
      }
      MyCubeGrid.m_tmpResultList.Clear();
      MyCubeGrid.m_physicsBoxQueryList.Clear();
      return !entityOverlap;
    }

    private static bool ShouldEntityBeIgnored(MyEntity ignorable, MyEntity entity) => entity != null && ignorable != null && !(entity is MyExtendedPistonBase) && entity.GetTopMostParent((System.Type) null) == ignorable;

    private static bool IsOrientationsAligned(MatrixD transform1, MatrixD transform2)
    {
      double num1 = Vector3D.Dot(transform1.Forward, transform2.Forward);
      if (num1 > 1.0 / 1000.0 && num1 < 999.0 / 1000.0 || num1 < -1.0 / 1000.0 && num1 > -999.0 / 1000.0)
        return false;
      double num2 = Vector3D.Dot(transform1.Up, transform2.Up);
      if (num2 > 1.0 / 1000.0 && num2 < 999.0 / 1000.0 || num2 < -1.0 / 1000.0 && num2 > -999.0 / 1000.0)
        return false;
      double num3 = Vector3D.Dot(transform1.Right, transform2.Right);
      return (num3 <= 1.0 / 1000.0 || num3 >= 999.0 / 1000.0) && (num3 >= -1.0 / 1000.0 || num3 <= -999.0 / 1000.0);
    }

    private static bool TestQueryIntersection(HkShape shape, MatrixD transform)
    {
      MatrixD lastQueryTransform = MyCubeGrid.m_lastQueryTransform;
      MatrixD matrixD = transform;
      matrixD.Translation -= lastQueryTransform.Translation;
      lastQueryTransform.Translation = Vector3D.Zero;
      Matrix transform1 = (Matrix) ref lastQueryTransform;
      Matrix transform2 = (Matrix) ref matrixD;
      return MyPhysics.IsPenetratingShapeShape((HkShape) MyCubeGrid.m_lastQueryBox.Value, ref transform1, shape, ref transform2);
    }

    public static bool TestPlacementVoxelMapOverlap(
      MyVoxelBase voxelMap,
      ref MyGridPlacementSettings settings,
      ref BoundingBoxD localAabb,
      ref MatrixD worldMatrix,
      bool touchingStaticGrid = false)
    {
      BoundingBoxD boundingBox = localAabb.TransformFast(ref worldMatrix);
      int num = 2;
      if (voxelMap == null)
        voxelMap = MySession.Static.VoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, (MyVoxelBase) null);
      if (voxelMap != null && voxelMap.IsAnyAabbCornerInside(ref worldMatrix, localAabb))
        num = 1;
      bool flag = true;
      switch (num)
      {
        case 1:
          flag = settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.Both;
          break;
        case 2:
          flag = settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.OutsideVoxel || settings.CanAnchorToStaticGrid & touchingStaticGrid;
          break;
      }
      return flag;
    }

    private static bool TestPlacementVoxelMapPenetration(
      MyVoxelBase voxelMap,
      MyGridPlacementSettings settings,
      ref BoundingBoxD localAabb,
      ref MatrixD worldMatrix,
      bool touchingStaticGrid = false)
    {
      float num = 0.0f;
      if (voxelMap != null)
      {
        MyTuple<float, float> inBoundingBoxFast = voxelMap.GetVoxelContentInBoundingBox_Fast(localAabb, worldMatrix);
        double volume = localAabb.Volume;
        num = inBoundingBoxFast.Item2.IsValid() ? inBoundingBoxFast.Item2 : 0.0f;
      }
      if ((double) num > (double) settings.VoxelPlacement.Value.MaxAllowed)
        return false;
      return (double) num >= (double) settings.VoxelPlacement.Value.MinAllowed || settings.CanAnchorToStaticGrid & touchingStaticGrid;
    }

    public static void TransformMountPoints(
      List<MyCubeBlockDefinition.MountPoint> outMountPoints,
      MyCubeBlockDefinition def,
      MyCubeBlockDefinition.MountPoint[] mountPoints,
      ref MyBlockOrientation orientation)
    {
      outMountPoints.Clear();
      if (mountPoints == null)
        return;
      Matrix result1;
      orientation.GetMatrix(out result1);
      Vector3I center = def.Center;
      for (int index = 0; index < mountPoints.Length; ++index)
      {
        MyCubeBlockDefinition.MountPoint mountPoint1 = mountPoints[index];
        MyCubeBlockDefinition.MountPoint mountPoint2 = new MyCubeBlockDefinition.MountPoint();
        Vector3 position1 = mountPoint1.Start - (Vector3) center;
        Vector3 position2 = mountPoint1.End - (Vector3) center;
        Vector3I.Transform(ref mountPoint1.Normal, ref result1, out mountPoint2.Normal);
        Vector3.Transform(ref position1, ref result1, out mountPoint2.Start);
        Vector3.Transform(ref position2, ref result1, out mountPoint2.End);
        mountPoint2.ExclusionMask = mountPoint1.ExclusionMask;
        mountPoint2.PropertiesMask = mountPoint1.PropertiesMask;
        mountPoint2.Enabled = mountPoint1.Enabled;
        mountPoint2.PressurizedWhenOpen = mountPoint1.PressurizedWhenOpen;
        Vector3I result2 = Vector3I.Floor(mountPoint1.Start) - center;
        Vector3I result3 = Vector3I.Floor(mountPoint1.End) - center;
        Vector3I.Transform(ref result2, ref result1, out result2);
        Vector3I.Transform(ref result3, ref result1, out result3);
        Vector3I vector3I1 = Vector3I.Floor(mountPoint2.Start);
        Vector3I vector3I2 = Vector3I.Floor(mountPoint2.End);
        Vector3I vector3I3 = result2 - vector3I1;
        Vector3I vector3I4 = result3 - vector3I2;
        mountPoint2.Start += (Vector3) vector3I3;
        mountPoint2.End += (Vector3) vector3I4;
        outMountPoints.Add(mountPoint2);
      }
    }

    internal static MyObjectBuilder_CubeBlock CreateBlockObjectBuilder(
      MyCubeBlockDefinition definition,
      Vector3I min,
      MyBlockOrientation orientation,
      long entityID,
      long owner,
      bool fullyBuilt)
    {
      MyObjectBuilder_CubeBlock newObject = (MyObjectBuilder_CubeBlock) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) definition.Id);
      newObject.BuildPercent = fullyBuilt ? 1f : 1.525902E-05f;
      newObject.IntegrityPercent = fullyBuilt ? 1f : 1.525902E-05f;
      newObject.EntityId = entityID;
      newObject.Min = (SerializableVector3I) min;
      newObject.BlockOrientation = (SerializableBlockOrientation) orientation;
      newObject.BuiltBy = owner;
      if (definition.ContainsComputer())
      {
        newObject.Owner = 0L;
        newObject.ShareMode = MyOwnershipShareModeEnum.All;
      }
      return newObject;
    }

    private static Vector3 ConvertVariantToHsvColor(Color variantColor)
    {
      switch (variantColor.PackedValue)
      {
        case 4278190080:
          return MyRenderComponentBase.OldBlackToHSV;
        case 4278190335:
          return MyRenderComponentBase.OldRedToHSV;
        case 4278222848:
          return MyRenderComponentBase.OldGreenToHSV;
        case 4278255615:
          return MyRenderComponentBase.OldYellowToHSV;
        case 4294901760:
          return MyRenderComponentBase.OldBlueToHSV;
        case uint.MaxValue:
          return MyRenderComponentBase.OldWhiteToHSV;
        default:
          return MyRenderComponentBase.OldGrayToHSV;
      }
    }

    internal static MyObjectBuilder_CubeBlock FindDefinitionUpgrade(
      MyObjectBuilder_CubeBlock block,
      out MyCubeBlockDefinition blockDefinition)
    {
      foreach (MyCubeBlockDefinition cubeBlockDefinition in MyDefinitionManager.Static.GetAllDefinitions().OfType<MyCubeBlockDefinition>())
      {
        if (cubeBlockDefinition.Id.SubtypeId == block.SubtypeId && !string.IsNullOrEmpty(block.SubtypeId.String))
        {
          blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(cubeBlockDefinition.Id);
          return MyObjectBuilder_CubeBlock.Upgrade(block, blockDefinition.Id.TypeId, block.SubtypeName);
        }
      }
      blockDefinition = (MyCubeBlockDefinition) null;
      return (MyObjectBuilder_CubeBlock) null;
    }

    public static Vector3I StaticGlobalGrid_WorldToUGInt(
      Vector3D worldPos,
      float gridSize,
      bool staticGridAlignToCenter)
    {
      return Vector3I.Round(MyCubeGrid.StaticGlobalGrid_WorldToUG(worldPos, gridSize, staticGridAlignToCenter));
    }

    public static Vector3D StaticGlobalGrid_WorldToUG(
      Vector3D worldPos,
      float gridSize,
      bool staticGridAlignToCenter)
    {
      Vector3D vector3D = worldPos / (double) gridSize;
      if (!staticGridAlignToCenter)
        vector3D += Vector3D.Half;
      return vector3D;
    }

    public static Vector3D StaticGlobalGrid_UGToWorld(
      Vector3D ugPos,
      float gridSize,
      bool staticGridAlignToCenter)
    {
      return staticGridAlignToCenter ? (double) gridSize * ugPos : (double) gridSize * (ugPos - Vector3D.Half);
    }

    private static System.Type ChooseGridSystemsType()
    {
      System.Type gridSystemsType = typeof (MyCubeGridSystems);
      MyCubeGrid.ChooseGridSystemsType(ref gridSystemsType, MyPlugins.GameAssembly);
      MyCubeGrid.ChooseGridSystemsType(ref gridSystemsType, MyPlugins.SandboxAssembly);
      MyCubeGrid.ChooseGridSystemsType(ref gridSystemsType, MyPlugins.UserAssemblies);
      return gridSystemsType;
    }

    private static void ChooseGridSystemsType(ref System.Type gridSystemsType, Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyCubeGrid.ChooseGridSystemsType(ref gridSystemsType, assembly);
    }

    private static void ChooseGridSystemsType(ref System.Type gridSystemsType, Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (System.Type type in assembly.GetTypes())
      {
        if (typeof (MyCubeGridSystems).IsAssignableFrom(type))
        {
          gridSystemsType = type;
          break;
        }
      }
    }

    private static bool ShouldBeStatic(MyCubeGrid grid, MyCubeGrid.MyTestDynamicReason testReason)
    {
      if (testReason == MyCubeGrid.MyTestDynamicReason.NoReason || grid.IsUnsupportedStation && testReason != MyCubeGrid.MyTestDynamicReason.ConvertToShip || grid.GridSizeEnum == MyCubeSize.Small && MyCubeGridSmallToLargeConnection.Static != null && MyCubeGridSmallToLargeConnection.Static.TestGridSmallToLargeConnection(grid))
        return true;
      if (testReason != MyCubeGrid.MyTestDynamicReason.GridSplitByBlock && testReason != MyCubeGrid.MyTestDynamicReason.ConvertToShip)
      {
        grid.RecalcBounds();
        MyGridPlacementSettings settings = new MyGridPlacementSettings();
        VoxelPlacementSettings placementSettings = new VoxelPlacementSettings()
        {
          PlacementMode = VoxelPlacementMode.Volumetric
        };
        settings.VoxelPlacement = new VoxelPlacementSettings?(placementSettings);
        if (!MyCubeGrid.IsAabbInsideVoxel(grid.WorldMatrix, (BoundingBoxD) grid.PositionComp.LocalAABB, settings))
          return false;
        if (grid.GetBlocks().Count > 1024)
          return grid.IsStatic;
      }
      BoundingBoxD worldAabb = grid.PositionComp.WorldAABB;
      if (MyGamePruningStructure.AnyVoxelMapInBox(ref worldAabb))
      {
        foreach (MySlimBlock block in grid.GetBlocks())
        {
          if (MyCubeGrid.IsInVoxels(block))
            return true;
        }
      }
      return false;
    }

    public static bool IsInVoxels(MySlimBlock block, bool checkForPhysics = true)
    {
      if (block.CubeGrid.Physics == null & checkForPhysics)
        return false;
      if (MyPerGameSettings.Destruction && block.CubeGrid.GridSizeEnum == MyCubeSize.Large)
        return block.CubeGrid.Physics.Shape.BlocksConnectedToWorld.Contains(block.Position);
      BoundingBoxD aabb1;
      block.GetWorldBoundingBox(out aabb1, false);
      MyCubeGrid.m_tmpVoxelList.Clear();
      MyGamePruningStructure.GetAllVoxelMapsInBox(ref aabb1, MyCubeGrid.m_tmpVoxelList);
      float gridSize = block.CubeGrid.GridSize;
      BoundingBoxD aabb2 = new BoundingBoxD((double) gridSize * ((Vector3D) block.Min - 0.5), (double) gridSize * ((Vector3D) block.Max + 0.5));
      MatrixD worldMatrix = block.CubeGrid.WorldMatrix;
      foreach (MyVoxelBase tmpVoxel in MyCubeGrid.m_tmpVoxelList)
      {
        if (tmpVoxel.IsAnyAabbCornerInside(ref worldMatrix, aabb2))
          return true;
      }
      return false;
    }

    public static void CreateGridGroupLink(
      GridLinkTypeEnum type,
      long linkId,
      MyCubeGrid parent,
      MyCubeGrid child)
    {
      MyCubeGridGroups.Static.CreateLink(type, linkId, parent, child);
    }

    public static bool BreakGridGroupLink(
      GridLinkTypeEnum type,
      long linkId,
      MyCubeGrid parent,
      MyCubeGrid child)
    {
      return MyCubeGridGroups.Static.BreakLink(type, linkId, parent, child);
    }

    public static void KillAllCharacters(MyCubeGrid grid)
    {
      if (grid == null || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      foreach (MyCockpit fatBlock in grid.GetFatBlocks<MyCockpit>())
      {
        if (fatBlock != null && fatBlock.Pilot != null && !fatBlock.Pilot.IsDead)
        {
          fatBlock.Pilot.DoDamage(1000f, MyDamageType.Suicide, true, fatBlock.Pilot.EntityId);
          fatBlock.RemovePilot();
        }
      }
    }

    public static void ResetInfoGizmos()
    {
      MyCubeGrid.ShowSenzorGizmos = false;
      MyCubeGrid.ShowGravityGizmos = false;
      MyCubeGrid.ShowCenterOfMass = false;
      MyCubeGrid.ShowGridPivot = false;
      MyCubeGrid.ShowAntennaGizmos = false;
    }

    private List<MyCubeGrid.Update> GetQueue(MyCubeGrid.UpdateQueue queue)
    {
      if (queue == MyCubeGrid.UpdateQueue.Invalid)
        throw new ArgumentException("Invalid queue.");
      ref List<MyCubeGrid.Update> local = ref this.m_updateQueues[(int) (queue - (byte) 1)];
      if (local == null)
        local = new List<MyCubeGrid.Update>();
      return local;
    }

    private void Dispatch(MyCubeGrid.UpdateQueue queue, bool parallel = false)
    {
      bool flag = queue.IsExecutedOnce();
      List<MyCubeGrid.Update> updateList = this.BeginUpdate(queue);
      int num = 0;
      for (int index = 0; index < updateList.Count; ++index)
      {
        MyCubeGrid.Update u = updateList[index];
        if (!u.Removed && u.Parallel == parallel)
          this.Invoke(in u, queue);
        if (u.Removed || flag && u.Parallel == parallel)
        {
          ++num;
          if (u.Parallel)
            Interlocked.Decrement(ref this.m_totalQueuedParallelUpdates);
          else
            Interlocked.Decrement(ref this.m_totalQueuedSynchronousUpdates);
        }
        else if (num > 0)
        {
          updateList[index - num] = u;
          updateList[index] = new MyCubeGrid.Update();
        }
      }
      while (num-- > 0)
        updateList.RemoveAt(updateList.Count - 1);
      this.EndUpdate();
    }

    private void DispatchOnce(MyCubeGrid.UpdateQueue queue, bool parallel = false) => this.Dispatch(queue, parallel);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Invoke(in MyCubeGrid.Update u, MyCubeGrid.UpdateQueue queue) => u.Callback();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<MyCubeGrid.Update> BeginUpdate(MyCubeGrid.UpdateQueue queue)
    {
      lock (this.m_updateQueues)
      {
        this.m_updateInProgress = this.m_updateInProgress == MyCubeGrid.UpdateQueue.Invalid ? queue : throw new InvalidOperationException("An update queue is already being dispatched for this entity.");
        return this.GetQueue(queue);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EndUpdate()
    {
      lock (this.m_updateQueues)
      {
        this.m_updateInProgress = MyCubeGrid.UpdateQueue.Invalid;
        if (this.m_hasDelayedUpdate)
        {
          lock (MyCubeGrid.m_pendingAddedUpdates)
          {
            for (int index = MyCubeGrid.m_pendingAddedUpdates.Count - 1; index >= 0; --index)
            {
              MyCubeGrid.QueuedUpdateChange pendingAddedUpdate = MyCubeGrid.m_pendingAddedUpdates[index];
              if (pendingAddedUpdate.Grid == this)
              {
                if (pendingAddedUpdate.Add)
                  this.Schedule(pendingAddedUpdate.Queue, pendingAddedUpdate.Callback, pendingAddedUpdate.Priority, pendingAddedUpdate.Parallel);
                else
                  this.DeSchedule(pendingAddedUpdate.Queue, pendingAddedUpdate.Callback);
                MyCubeGrid.m_pendingAddedUpdates.RemoveAtFast<MyCubeGrid.QueuedUpdateChange>(index);
              }
            }
          }
          this.m_hasDelayedUpdate = false;
        }
        if (this.m_totalQueuedSynchronousUpdates == 0)
        {
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
        }
        else
        {
          if (this.m_totalQueuedParallelUpdates != 0 || !this.InScene)
            return;
          MyEntities.Orchestrator.EntityFlagsChanged((MyEntity) this);
        }
      }
    }

    private static string GetProfilerKey(Action cb)
    {
      string str;
      if (!MyCubeGrid.m_methodNames.TryGetValue(cb.Method, out str))
        MyCubeGrid.m_methodNames.TryAdd(cb.Method, str = MyCubeGrid.DebugFormatMethodName(cb.Method));
      return str;
    }

    public static string DebugFormatMethodName(MethodInfo method) => typeof (MyCubeGrid).IsAssignableFrom(method.DeclaringType) ? method.Name : method.DeclaringType.Name + "." + method.Name;

    public void Schedule(
      MyCubeGrid.UpdateQueue queue,
      Action callback,
      int priority = 2147483647,
      bool parallel = false)
    {
      lock (this.m_updateQueues)
      {
        if (this.m_updateInProgress == queue)
        {
          lock (MyCubeGrid.m_pendingAddedUpdates)
          {
            MyCubeGrid.m_pendingAddedUpdates.Add(MyCubeGrid.QueuedUpdateChange.MakeAdd(callback, priority, queue, this, parallel));
            this.m_hasDelayedUpdate = true;
          }
        }
        else
        {
          List<MyCubeGrid.Update> queue1 = this.GetQueue(queue);
          for (int index = 0; index < queue1.Count; ++index)
          {
            if (queue1[index].Callback == callback)
              return;
          }
          queue1.Insert(MyCubeGrid.FindInsertion(queue1, priority), new MyCubeGrid.Update(callback, priority, parallel));
          if (parallel)
          {
            if (Interlocked.Increment(ref this.m_totalQueuedParallelUpdates) != 1 || !this.InScene)
              return;
            MyEntities.Orchestrator.EntityFlagsChanged((MyEntity) this);
          }
          else
          {
            if (Interlocked.Increment(ref this.m_totalQueuedSynchronousUpdates) != 1)
              return;
            this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
          }
        }
      }
    }

    public void DeSchedule(MyCubeGrid.UpdateQueue queue, Action callback)
    {
      lock (this.m_updateQueues)
      {
        List<MyCubeGrid.Update> queue1 = this.GetQueue(queue);
        if (this.m_updateInProgress == queue)
        {
          lock (MyCubeGrid.m_pendingAddedUpdates)
          {
            MyCubeGrid.m_pendingAddedUpdates.Add(MyCubeGrid.QueuedUpdateChange.MakeRemove(callback, queue, this));
            this.m_hasDelayedUpdate = true;
          }
        }
        else
        {
          int index = 0;
          while (index < queue1.Count && !(queue1[index].Callback == callback))
            ++index;
          if (index >= queue1.Count)
            return;
          MyCubeGrid.Update update = queue1[index];
          update.SetRemoved();
          queue1[index] = update;
        }
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int FindInsertion(List<MyCubeGrid.Update> list, int priority)
    {
      if (list.Count == 0)
        return 0;
      int index1 = 0;
      int num1 = list.Count;
      while (num1 - index1 > 1)
      {
        int index2 = (index1 + num1) / 2;
        if (priority >= list[index2].Priority)
          index1 = index2;
        else
          num1 = index2;
      }
      int num2 = index1;
      if (priority >= list[index1].Priority)
        num2 = num1;
      return num2;
    }

    public void GetDebugUpdateInfo(
      List<MyCubeGrid.DebugUpdateRecord> gridDebugUpdateInfo)
    {
      for (int index = 0; index < this.m_updateQueues.Length; ++index)
      {
        List<MyCubeGrid.Update> updateQueue = this.m_updateQueues[index];
        if (updateQueue != null && updateQueue.Count > 0)
        {
          MyCubeGrid.UpdateQueue queue = (MyCubeGrid.UpdateQueue) (index + 1);
          foreach (MyCubeGrid.Update update in updateQueue)
          {
            if (!update.Removed)
              gridDebugUpdateInfo.Add(new MyCubeGrid.DebugUpdateRecord(in update, queue));
          }
        }
      }
    }

    public IEnumerable<KeyValuePair<MyCubeGrid.DebugUpdateRecord, MyCubeGrid.DebugUpdateStats>> LastUpdates => throw new InvalidOperationException("Feature only available in DEBUG builds.");

    public override MyGameLogicComponent GameLogic
    {
      get => base.GameLogic;
      set
      {
        if (value != null && value.EntityUpdate)
        {
          this.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(value.UpdateBeforeSimulation));
          this.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(value.UpdateAfterSimulation));
        }
        else if (this.GameLogic != null)
        {
          this.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.GameLogic.UpdateBeforeSimulation));
          this.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, new Action(this.GameLogic.UpdateAfterSimulation));
        }
        base.GameLogic = value;
      }
    }

    public override void UpdateBeforeSimulation()
    {
      MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateBeforeSimulation));
      this.DispatchOnce(MyCubeGrid.UpdateQueue.OnceBeforeSimulation);
      this.Dispatch(MyCubeGrid.UpdateQueue.BeforeSimulation);
      MySimpleProfiler.End(nameof (UpdateBeforeSimulation));
    }

    public override void UpdateAfterSimulation()
    {
      MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateAfterSimulation));
      this.DispatchOnce(MyCubeGrid.UpdateQueue.OnceAfterSimulation);
      this.Dispatch(MyCubeGrid.UpdateQueue.AfterSimulation);
      MySimpleProfiler.End(nameof (UpdateAfterSimulation));
    }

    public void UpdateBeforeSimulationParallel()
    {
      this.DispatchOnce(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, true);
      this.Dispatch(MyCubeGrid.UpdateQueue.BeforeSimulation, true);
    }

    public void UpdateAfterSimulationParallel()
    {
      this.DispatchOnce(MyCubeGrid.UpdateQueue.OnceAfterSimulation, true);
      this.Dispatch(MyCubeGrid.UpdateQueue.AfterSimulation, true);
    }

    public MyParallelUpdateFlags UpdateFlags
    {
      get
      {
        MyParallelUpdateFlags parallel = this.NeedsUpdate.GetParallel();
        if (this.m_totalQueuedParallelUpdates > 0)
          parallel |= MyParallelUpdateFlags.EACH_FRAME_PARALLEL;
        return parallel;
      }
    }

    string VRage.Game.ModAPI.Ingame.IMyCubeGrid.CustomName
    {
      get => this.DisplayName;
      set
      {
        if (!this.IsAccessibleForProgrammableBlock)
          return;
        this.ChangeDisplayNameRequest(value);
      }
    }

    string VRage.Game.ModAPI.IMyCubeGrid.CustomName
    {
      get => this.DisplayName;
      set => this.ChangeDisplayNameRequest(value);
    }

    VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeGrid.AddBlock(
      MyObjectBuilder_CubeBlock objectBuilder,
      bool testMerge)
    {
      return (VRage.Game.ModAPI.IMySlimBlock) this.AddBlock(objectBuilder, testMerge);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.ApplyDestructionDeformation(VRage.Game.ModAPI.IMySlimBlock block)
    {
      if (!(block is MySlimBlock))
        return;
      this.ApplyDestructionDeformation(block as MySlimBlock, 1f, new MyHitInfo?(), 0L);
    }

    MatrixI VRage.Game.ModAPI.IMyCubeGrid.CalculateMergeTransform(
      VRage.Game.ModAPI.IMyCubeGrid gridToMerge,
      Vector3I gridOffset)
    {
      return gridToMerge is MyCubeGrid ? this.CalculateMergeTransform(gridToMerge as MyCubeGrid, gridOffset) : new MatrixI();
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.CanMergeCubes(
      VRage.Game.ModAPI.IMyCubeGrid gridToMerge,
      Vector3I gridOffset)
    {
      return gridToMerge is MyCubeGrid && this.CanMergeCubes(gridToMerge as MyCubeGrid, gridOffset);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.GetBlocks(
      List<VRage.Game.ModAPI.IMySlimBlock> blocks,
      Func<VRage.Game.ModAPI.IMySlimBlock, bool> collect)
    {
      foreach (MySlimBlock block in this.GetBlocks())
      {
        if (collect == null || collect((VRage.Game.ModAPI.IMySlimBlock) block))
          blocks.Add((VRage.Game.ModAPI.IMySlimBlock) block);
      }
    }

    List<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.GetBlocksInsideSphere(
      ref BoundingSphereD sphere)
    {
      HashSet<MySlimBlock> blocks = new HashSet<MySlimBlock>();
      this.GetBlocksInsideSphere(ref sphere, blocks);
      List<VRage.Game.ModAPI.IMySlimBlock> mySlimBlockList = new List<VRage.Game.ModAPI.IMySlimBlock>(blocks.Count);
      foreach (MySlimBlock mySlimBlock in blocks)
        mySlimBlockList.Add((VRage.Game.ModAPI.IMySlimBlock) mySlimBlock);
      return mySlimBlockList;
    }

    VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeGrid.GetCubeBlock(
      Vector3I pos)
    {
      return (VRage.Game.ModAPI.IMySlimBlock) this.GetCubeBlock(pos);
    }

    bool VRage.Game.ModAPI.Ingame.IMyCubeGrid.IsSameConstructAs(VRage.Game.ModAPI.Ingame.IMyCubeGrid other) => this.IsSameConstructAs((VRage.Game.ModAPI.IMyCubeGrid) other);

    Vector3D? VRage.Game.ModAPI.IMyCubeGrid.GetLineIntersectionExactAll(
      ref LineD line,
      out double distance,
      out VRage.Game.ModAPI.IMySlimBlock intersectedBlock)
    {
      MySlimBlock intersectedBlock1;
      Vector3D? intersectionExactAll = this.GetLineIntersectionExactAll(ref line, out distance, out intersectedBlock1);
      intersectedBlock = (VRage.Game.ModAPI.IMySlimBlock) intersectedBlock1;
      return intersectionExactAll;
    }

    VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.MergeGrid_MergeBlock(
      VRage.Game.ModAPI.IMyCubeGrid gridToMerge,
      Vector3I gridOffset)
    {
      return gridToMerge is MyCubeGrid ? (VRage.Game.ModAPI.IMyCubeGrid) this.MergeGrid_MergeBlock(gridToMerge as MyCubeGrid, gridOffset, true) : (VRage.Game.ModAPI.IMyCubeGrid) null;
    }

    void VRage.Game.ModAPI.IMyCubeGrid.RemoveBlock(
      VRage.Game.ModAPI.IMySlimBlock block,
      bool updatePhysics)
    {
      if (!(block is MySlimBlock))
        return;
      this.RemoveBlock(block as MySlimBlock, updatePhysics);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.RemoveDestroyedBlock(VRage.Game.ModAPI.IMySlimBlock block)
    {
      if (!(block is MySlimBlock))
        return;
      this.RemoveDestroyedBlock(block as MySlimBlock, 0L);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.UpdateBlockNeighbours(VRage.Game.ModAPI.IMySlimBlock block)
    {
      if (!(block is MySlimBlock))
        return;
      this.UpdateBlockNeighbours(block as MySlimBlock);
    }

    List<long> VRage.Game.ModAPI.IMyCubeGrid.BigOwners => this.BigOwners;

    List<long> VRage.Game.ModAPI.IMyCubeGrid.SmallOwners => this.SmallOwners;

    void VRage.Game.ModAPI.IMyCubeGrid.ChangeGridOwnership(
      long playerId,
      MyOwnershipShareModeEnum shareMode)
    {
      this.ChangeGridOwnership(playerId, shareMode);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.ClearSymmetries() => this.ClearSymmetries();

    void VRage.Game.ModAPI.IMyCubeGrid.ColorBlocks(
      Vector3I min,
      Vector3I max,
      Vector3 newHSV)
    {
      this.ColorBlocks(min, max, newHSV, false, false);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.SkinBlocks(
      Vector3I min,
      Vector3I max,
      Vector3? newHSV,
      string newSkin)
    {
      this.SkinBlocks(min, max, newHSV, new MyStringHash?(MyStringHash.GetOrCompute(newSkin)), false, false);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.FixTargetCube(
      out Vector3I cube,
      Vector3 fractionalGridPosition)
    {
      this.FixTargetCube(out cube, fractionalGridPosition);
    }

    Vector3 VRage.Game.ModAPI.IMyCubeGrid.GetClosestCorner(
      Vector3I gridPos,
      Vector3 position)
    {
      return this.GetClosestCorner(gridPos, position);
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.GetLineIntersectionExactGrid(
      ref LineD line,
      ref Vector3I position,
      ref double distanceSquared)
    {
      return this.GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared);
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.IsTouchingAnyNeighbor(Vector3I min, Vector3I max) => this.IsTouchingAnyNeighbor(min, max);

    Vector3I? VRage.Game.ModAPI.IMyCubeGrid.RayCastBlocks(
      Vector3D worldStart,
      Vector3D worldEnd)
    {
      return this.RayCastBlocks(worldStart, worldEnd);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.RayCastCells(
      Vector3D worldStart,
      Vector3D worldEnd,
      List<Vector3I> outHitPositions,
      Vector3I? gridSizeInflate,
      bool havokWorld)
    {
      this.RayCastCells(worldStart, worldEnd, outHitPositions, gridSizeInflate, havokWorld, true);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.RazeBlock(Vector3I position) => this.RazeBlock(position, 0UL);

    void VRage.Game.ModAPI.IMyCubeGrid.RazeBlocks(
      ref Vector3I pos,
      ref Vector3UByte size)
    {
      this.RazeBlocks(ref pos, ref size, 0L);
    }

    void VRage.Game.ModAPI.IMyCubeGrid.RazeBlocks(List<Vector3I> locations) => this.RazeBlocks(locations, 0L, 0UL);

    Vector3I VRage.Game.ModAPI.IMyCubeGrid.WorldToGridInteger(Vector3D coords) => this.WorldToGridInteger(coords);

    bool VRage.Game.ModAPI.IMyCubeGrid.WillRemoveBlockSplitGrid(VRage.Game.ModAPI.IMySlimBlock testBlock) => this.WillRemoveBlockSplitGrid((MySlimBlock) testBlock);

    private Action<MySlimBlock> GetDelegate(Action<VRage.Game.ModAPI.IMySlimBlock> value) => (Action<MySlimBlock>) Delegate.CreateDelegate(typeof (Action<MySlimBlock>), value.Target, value.Method);

    private Action<MyCubeGrid> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid> value) => (Action<MyCubeGrid>) Delegate.CreateDelegate(typeof (Action<MyCubeGrid>), value.Target, value.Method);

    private Action<MyCubeGrid, MyCubeGrid> GetDelegate(
      Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> value)
    {
      return (Action<MyCubeGrid, MyCubeGrid>) Delegate.CreateDelegate(typeof (Action<MyCubeGrid, MyCubeGrid>), value.Target, value.Method);
    }

    private Action<MyCubeGrid, bool> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid, bool> value) => (Action<MyCubeGrid, bool>) Delegate.CreateDelegate(typeof (Action<MyCubeGrid, bool>), value.Target, value.Method);

    event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockAdded
    {
      add => this.OnBlockAdded += this.GetDelegate(value);
      remove => this.OnBlockAdded -= this.GetDelegate(value);
    }

    event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockRemoved
    {
      add => this.OnBlockRemoved += this.GetDelegate(value);
      remove => this.OnBlockRemoved -= this.GetDelegate(value);
    }

    event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnBlockOwnershipChanged
    {
      add => this.OnBlockOwnershipChanged += this.GetDelegate(value);
      remove => this.OnBlockOwnershipChanged -= this.GetDelegate(value);
    }

    event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridChanged
    {
      add => this.OnGridChanged += this.GetDelegate(value);
      remove => this.OnGridChanged -= this.GetDelegate(value);
    }

    VRage.Game.ModAPI.Ingame.IMySlimBlock VRage.Game.ModAPI.Ingame.IMyCubeGrid.GetCubeBlock(
      Vector3I position)
    {
      VRage.Game.ModAPI.Ingame.IMySlimBlock cubeBlock = (VRage.Game.ModAPI.Ingame.IMySlimBlock) this.GetCubeBlock(position);
      return cubeBlock != null && cubeBlock.FatBlock != null && (cubeBlock.FatBlock is MyTerminalBlock && (cubeBlock.FatBlock as MyTerminalBlock).IsAccessibleForProgrammableBlock) ? cubeBlock : (VRage.Game.ModAPI.Ingame.IMySlimBlock) null;
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.IsRespawnGrid
    {
      get => this.IsRespawnGrid;
      set => this.IsRespawnGrid = value;
    }

    event Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridSplit
    {
      add => this.OnGridSplit += this.GetDelegate(value);
      remove => this.OnGridSplit -= this.GetDelegate(value);
    }

    event Action<VRage.Game.ModAPI.IMyCubeGrid, bool> VRage.Game.ModAPI.IMyCubeGrid.OnIsStaticChanged
    {
      add => this.OnStaticChanged += this.GetDelegate(value);
      remove => this.OnStaticChanged -= this.GetDelegate(value);
    }

    event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockIntegrityChanged
    {
      add => this.OnBlockIntegrityChanged += this.GetDelegate(value);
      remove => this.OnBlockIntegrityChanged -= this.GetDelegate(value);
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.CanAddCube(Vector3I pos) => this.CanAddCube(pos, new MyBlockOrientation?(), (MyCubeBlockDefinition) null);

    bool VRage.Game.ModAPI.IMyCubeGrid.CanAddCubes(Vector3I min, Vector3I max) => this.CanAddCubes(min, max);

    VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.SplitByPlane(
      PlaneD plane)
    {
      return (VRage.Game.ModAPI.IMyCubeGrid) this.SplitByPlane(plane);
    }

    VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.Split(
      List<VRage.Game.ModAPI.IMySlimBlock> blocks,
      bool sync)
    {
      return (VRage.Game.ModAPI.IMyCubeGrid) MyCubeGrid.CreateSplit(this, blocks.ConvertAll<MySlimBlock>((Converter<VRage.Game.ModAPI.IMySlimBlock, MySlimBlock>) (x => (MySlimBlock) x)), sync);
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.IsStatic
    {
      get => this.IsStatic;
      set
      {
        if (value)
          this.RequestConversionToStation();
        else
          this.RequestConversionToShip((Action) null);
      }
    }

    Vector3I? VRage.Game.ModAPI.IMyCubeGrid.XSymmetryPlane
    {
      get => this.XSymmetryPlane;
      set => this.XSymmetryPlane = value;
    }

    Vector3I? VRage.Game.ModAPI.IMyCubeGrid.YSymmetryPlane
    {
      get => this.YSymmetryPlane;
      set => this.YSymmetryPlane = value;
    }

    Vector3I? VRage.Game.ModAPI.IMyCubeGrid.ZSymmetryPlane
    {
      get => this.ZSymmetryPlane;
      set => this.ZSymmetryPlane = value;
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.XSymmetryOdd
    {
      get => this.XSymmetryOdd;
      set => this.XSymmetryOdd = value;
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.YSymmetryOdd
    {
      get => this.YSymmetryOdd;
      set => this.YSymmetryOdd = value;
    }

    bool VRage.Game.ModAPI.IMyCubeGrid.ZSymmetryOdd
    {
      get => this.ZSymmetryOdd;
      set => this.ZSymmetryOdd = value;
    }

    MyUpdateTiersGridPresence VRage.Game.ModAPI.IMyCubeGrid.GridPresenceTier => this.GridPresenceTier;

    MyUpdateTiersPlayerPresence VRage.Game.ModAPI.IMyCubeGrid.PlayerPresenceTier => this.PlayerPresenceTier;

    event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.GridPresenceTierChanged
    {
      add => this.GridPresenceTierChanged += this.GetDelegate(value);
      remove => this.GridPresenceTierChanged -= this.GetDelegate(value);
    }

    event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.PlayerPresenceTierChanged
    {
      add => this.PlayerPresenceTierChanged += this.GetDelegate(value);
      remove => this.PlayerPresenceTierChanged -= this.GetDelegate(value);
    }

    public bool IsInSameLogicalGroupAs(VRage.Game.ModAPI.IMyCubeGrid other) => this == other || MyCubeGridGroups.Static.Logical.GetGroup(this) == MyCubeGridGroups.Static.Logical.GetGroup((MyCubeGrid) other);

    public bool IsSameConstructAs(VRage.Game.ModAPI.IMyCubeGrid other) => this == other || MyCubeGridGroups.Static.Mechanical.GetGroup(this) == MyCubeGridGroups.Static.Mechanical.GetGroup((MyCubeGrid) other);

    public bool IsRoomAtPositionAirtight(Vector3I pos)
    {
      if (this.GridSystems.GasSystem == null)
        return false;
      MyOxygenRoom cubeGridPosition = this.GridSystems.GasSystem.GetOxygenRoomForCubeGridPosition(ref pos);
      return cubeGridPosition != null && cubeGridPosition.IsAirtight;
    }

    public enum MyTestDisconnectsReason
    {
      NoReason,
      BlockRemoved,
      SplitBlock,
    }

    internal enum MyTestDynamicReason
    {
      NoReason,
      GridCopied,
      GridSplit,
      GridSplitByBlock,
      ConvertToShip,
    }

    private struct DeformationPostponedItem
    {
      public Vector3I Position;
      public Vector3I Min;
      public Vector3I Max;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MyBlockBuildArea
    {
      public DefinitionIdBlit DefinitionId;
      public uint ColorMaskHSV;
      public Vector3I PosInGrid;
      public Vector3B BlockMin;
      public Vector3B BlockMax;
      public Vector3UByte BuildAreaSize;
      public Vector3B StepDelta;
      public Base6Directions.Direction OrientationForward;
      public Base6Directions.Direction OrientationUp;
      public MyStringHash SkinId;

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EDefinitionId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, DefinitionIdBlit>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in DefinitionIdBlit value) => owner.DefinitionId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out DefinitionIdBlit value) => value = owner.DefinitionId;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EColorMaskHSV\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, uint>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in uint value) => owner.ColorMaskHSV = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out uint value) => value = owner.ColorMaskHSV;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EPosInGrid\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in Vector3I value) => owner.PosInGrid = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out Vector3I value) => value = owner.PosInGrid;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EBlockMin\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Vector3B>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in Vector3B value) => owner.BlockMin = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out Vector3B value) => value = owner.BlockMin;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EBlockMax\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Vector3B>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in Vector3B value) => owner.BlockMax = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out Vector3B value) => value = owner.BlockMax;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EBuildAreaSize\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Vector3UByte>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in Vector3UByte value) => owner.BuildAreaSize = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out Vector3UByte value) => value = owner.BuildAreaSize;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EStepDelta\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Vector3B>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in Vector3B value) => owner.StepDelta = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out Vector3B value) => value = owner.StepDelta;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EOrientationForward\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Base6Directions.Direction>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCubeGrid.MyBlockBuildArea owner,
          in Base6Directions.Direction value)
        {
          owner.OrientationForward = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCubeGrid.MyBlockBuildArea owner,
          out Base6Directions.Direction value)
        {
          value = owner.OrientationForward;
        }
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003EOrientationUp\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, Base6Directions.Direction>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCubeGrid.MyBlockBuildArea owner,
          in Base6Directions.Direction value)
        {
          owner.OrientationUp = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCubeGrid.MyBlockBuildArea owner,
          out Base6Directions.Direction value)
        {
          value = owner.OrientationUp;
        }
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u003C\u003ESkinId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockBuildArea, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockBuildArea owner, in MyStringHash value) => owner.SkinId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockBuildArea owner, out MyStringHash value) => value = owner.SkinId;
      }
    }

    [ProtoContract]
    public struct MyBlockLocation
    {
      [ProtoMember(1)]
      public Vector3I Min;
      [ProtoMember(4)]
      public Vector3I Max;
      [ProtoMember(7)]
      public Vector3I CenterPos;
      [ProtoMember(10)]
      public MyBlockOrientation Orientation;
      [ProtoMember(13)]
      public long EntityId;
      [ProtoMember(16)]
      public DefinitionIdBlit BlockDefinition;
      [ProtoMember(19)]
      public long Owner;

      public MyBlockLocation(
        MyDefinitionId blockDefinition,
        Vector3I min,
        Vector3I max,
        Vector3I center,
        Quaternion orientation,
        long entityId,
        long owner)
      {
        this.BlockDefinition = (DefinitionIdBlit) blockDefinition;
        this.Min = min;
        this.Max = max;
        this.CenterPos = center;
        this.Orientation = new MyBlockOrientation(ref orientation);
        this.EntityId = entityId;
        this.Owner = owner;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in Vector3I value) => owner.Min = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out Vector3I value) => value = owner.Min;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in Vector3I value) => owner.Max = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out Vector3I value) => value = owner.Max;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003ECenterPos\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in Vector3I value) => owner.CenterPos = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out Vector3I value) => value = owner.CenterPos;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EOrientation\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, MyBlockOrientation>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in MyBlockOrientation value) => owner.Orientation = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out MyBlockOrientation value) => value = owner.Orientation;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out long value) => value = owner.EntityId;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EBlockDefinition\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, DefinitionIdBlit>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in DefinitionIdBlit value) => owner.BlockDefinition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out DefinitionIdBlit value) => value = owner.BlockDefinition;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockLocation, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockLocation owner, in long value) => owner.Owner = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockLocation owner, out long value) => value = owner.Owner;
      }

      private class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003C\u003EActor : IActivator, IActivator<MyCubeGrid.MyBlockLocation>
      {
        object IActivator.CreateInstance() => (object) new MyCubeGrid.MyBlockLocation();

        MyCubeGrid.MyBlockLocation IActivator<MyCubeGrid.MyBlockLocation>.CreateInstance() => new MyCubeGrid.MyBlockLocation();
      }
    }

    [ProtoContract]
    public struct BlockPositionId
    {
      [ProtoMember(22)]
      public Vector3I Position;
      [ProtoMember(25)]
      public uint CompoundId;

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EBlockPositionId\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.BlockPositionId, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.BlockPositionId owner, in Vector3I value) => owner.Position = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.BlockPositionId owner, out Vector3I value) => value = owner.Position;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EBlockPositionId\u003C\u003ECompoundId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.BlockPositionId, uint>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.BlockPositionId owner, in uint value) => owner.CompoundId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.BlockPositionId owner, out uint value) => value = owner.CompoundId;
      }

      private class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EBlockPositionId\u003C\u003EActor : IActivator, IActivator<MyCubeGrid.BlockPositionId>
      {
        object IActivator.CreateInstance() => (object) new MyCubeGrid.BlockPositionId();

        MyCubeGrid.BlockPositionId IActivator<MyCubeGrid.BlockPositionId>.CreateInstance() => new MyCubeGrid.BlockPositionId();
      }
    }

    [ProtoContract]
    public struct MyBlockVisuals
    {
      [ProtoMember(28)]
      public uint ColorMaskHSV;
      [ProtoMember(31)]
      public MyStringHash SkinId;
      [ProtoMember(33)]
      public bool ApplyColor;
      [ProtoMember(35)]
      public bool ApplySkin;

      public MyBlockVisuals(
        uint colorMaskHsv,
        MyStringHash skinId,
        bool applyColor = true,
        bool applySkin = true)
      {
        this.ColorMaskHSV = colorMaskHsv;
        this.SkinId = skinId;
        this.ApplyColor = applyColor;
        this.ApplySkin = applySkin;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u003C\u003EColorMaskHSV\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockVisuals, uint>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockVisuals owner, in uint value) => owner.ColorMaskHSV = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockVisuals owner, out uint value) => value = owner.ColorMaskHSV;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u003C\u003ESkinId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockVisuals, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockVisuals owner, in MyStringHash value) => owner.SkinId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockVisuals owner, out MyStringHash value) => value = owner.SkinId;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u003C\u003EApplyColor\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockVisuals, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockVisuals owner, in bool value) => owner.ApplyColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockVisuals owner, out bool value) => value = owner.ApplyColor;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u003C\u003EApplySkin\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyBlockVisuals, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyBlockVisuals owner, in bool value) => owner.ApplySkin = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyBlockVisuals owner, out bool value) => value = owner.ApplySkin;
      }

      private class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u003C\u003EActor : IActivator, IActivator<MyCubeGrid.MyBlockVisuals>
      {
        object IActivator.CreateInstance() => (object) new MyCubeGrid.MyBlockVisuals();

        MyCubeGrid.MyBlockVisuals IActivator<MyCubeGrid.MyBlockVisuals>.CreateInstance() => new MyCubeGrid.MyBlockVisuals();
      }
    }

    private enum NeighborOffsetIndex
    {
      XUP,
      XDOWN,
      YUP,
      YDOWN,
      ZUP,
      ZDOWN,
      XUP_YUP,
      XUP_YDOWN,
      XDOWN_YUP,
      XDOWN_YDOWN,
      YUP_ZUP,
      YUP_ZDOWN,
      YDOWN_ZUP,
      YDOWN_ZDOWN,
      XUP_ZUP,
      XUP_ZDOWN,
      XDOWN_ZUP,
      XDOWN_ZDOWN,
      XUP_YUP_ZUP,
      XUP_YUP_ZDOWN,
      XUP_YDOWN_ZUP,
      XUP_YDOWN_ZDOWN,
      XDOWN_YUP_ZUP,
      XDOWN_YUP_ZDOWN,
      XDOWN_YDOWN_ZUP,
      XDOWN_YDOWN_ZDOWN,
    }

    private struct MyNeighbourCachedBlock
    {
      public Vector3I Position;
      public MyCubeBlockDefinition BlockDefinition;
      public MyBlockOrientation Orientation;

      public override int GetHashCode() => this.Position.GetHashCode();
    }

    public class BlockTypeCounter
    {
      private Dictionary<MyDefinitionId, int> m_countById = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);

      internal int GetNextNumber(MyDefinitionId blockType)
      {
        int num1 = 0;
        this.m_countById.TryGetValue(blockType, out num1);
        int num2 = num1 + 1;
        this.m_countById[blockType] = num2;
        return num2;
      }
    }

    private class PasteGridData : WorkData
    {
      private List<MyObjectBuilder_CubeGrid> m_gridObjectBuilders;
      private bool m_detectDisconnects;
      private Vector3 m_objectVelocity;
      private bool m_multiBlock;
      private bool m_instantBuild;
      private List<MyCubeGrid> m_pastedGrids;
      private bool m_canPlaceGrid;
      private List<VRage.ModAPI.IMyEntity> m_resultIDs;
      private bool m_removeScripts;
      public readonly EndpointId SenderEndpointId;
      public readonly bool IsLocallyInvoked;
      private List<ulong> m_clientsideDLCs;
      public Vector3D? m_offset;

      public PasteGridData(
        List<MyObjectBuilder_CubeGrid> entities,
        bool detectDisconnects,
        Vector3 objectVelocity,
        bool multiBlock,
        bool instantBuild,
        bool shouldRemoveScripts,
        EndpointId senderEndpointId,
        bool isLocallyInvoked,
        Vector3D? offset,
        List<ulong> clientsideDLCs = null)
      {
        this.m_gridObjectBuilders = new List<MyObjectBuilder_CubeGrid>((IEnumerable<MyObjectBuilder_CubeGrid>) entities);
        this.m_detectDisconnects = detectDisconnects;
        this.m_objectVelocity = objectVelocity;
        this.m_multiBlock = multiBlock;
        this.m_instantBuild = instantBuild;
        this.SenderEndpointId = senderEndpointId;
        this.IsLocallyInvoked = isLocallyInvoked;
        this.m_removeScripts = shouldRemoveScripts;
        this.m_offset = offset;
        this.m_clientsideDLCs = clientsideDLCs;
      }

      public void TryPasteGrid()
      {
        bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(this.SenderEndpointId.Value);
        if (MySession.Static.SurvivalMode && !flag)
          return;
        for (int index = 0; index < this.m_gridObjectBuilders.Count; ++index)
          this.m_gridObjectBuilders[index] = (MyObjectBuilder_CubeGrid) this.m_gridObjectBuilders[index].Clone();
        MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) this.m_gridObjectBuilders);
        MySessionComponentDLC component1 = MySession.Static.GetComponent<MySessionComponentDLC>();
        MySessionComponentGameInventory component2 = MySession.Static.GetComponent<MySessionComponentGameInventory>();
        int num = -1;
        List<int> intList = new List<int>();
        foreach (MyObjectBuilder_CubeGrid gridObjectBuilder in this.m_gridObjectBuilders)
        {
          ++num;
          int index = 0;
          while (index < gridObjectBuilder.CubeBlocks.Count)
          {
            MyObjectBuilder_CubeBlock cubeBlock = gridObjectBuilder.CubeBlocks[index];
            if (this.m_removeScripts && cubeBlock is MyObjectBuilder_MyProgrammableBlock programmableBlock)
              programmableBlock.Program = (string) null;
            cubeBlock.SkinSubtypeId = component2.ValidateArmor(MyStringHash.GetOrCompute(cubeBlock.SkinSubtypeId), this.SenderEndpointId.Value).String;
            MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId));
            if (!(component1.HasDefinitionDLC(new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId), this.SenderEndpointId.Value) & component1.ContainsRequiredDLC(definition, this.m_clientsideDLCs)))
              gridObjectBuilder.CubeBlocks.RemoveAt(index);
            else
              ++index;
          }
          if (gridObjectBuilder.CubeBlocks.Count == 0)
            intList.Add(num);
        }
        if (intList.Count > 0)
        {
          for (int index = intList.Count - 1; index >= 0; --index)
            this.m_gridObjectBuilders.RemoveAt(intList[index]);
        }
        if (this.m_gridObjectBuilders.Count == 0)
        {
          if (MyEventContext.Current.IsLocallyInvoked)
            MyCubeGrid.ShowMessageGridsRemovedWhilePastingInternal();
          else
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyCubeGrid.ShowMessageGridsRemovedWhilePasting)), this.SenderEndpointId);
        }
        else
        {
          this.m_pastedGrids = new List<MyCubeGrid>();
          MyEntityIdentifier.InEntityCreationBlock = true;
          MyEntityIdentifier.LazyInitPerThreadStorage(2048);
          this.m_canPlaceGrid = true;
          foreach (MyObjectBuilder_CubeGrid gridObjectBuilder in this.m_gridObjectBuilders)
          {
            MySandboxGame.Log.WriteLine("CreateCompressedMsg: Type: " + gridObjectBuilder.GetType().Name.ToString() + "  Name: " + gridObjectBuilder.Name + "  EntityID: " + gridObjectBuilder.EntityId.ToString("X8"));
            if (MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) gridObjectBuilder, false) is MyCubeGrid fromObjectBuilder)
            {
              this.m_pastedGrids.Add(fromObjectBuilder);
              this.m_canPlaceGrid &= this.TestPastedGridPlacement(fromObjectBuilder, false);
              if (this.m_canPlaceGrid)
              {
                long inventoryEntityId = 0;
                if (this.m_instantBuild & flag)
                  MyCubeGrid.ChangeOwnership(inventoryEntityId, fromObjectBuilder);
                MySandboxGame.Log.WriteLine("Status: Exists(" + MyEntities.EntityExists(gridObjectBuilder.EntityId).ToString() + ") InScene(" + ((gridObjectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene).ToString() + ")");
              }
              else
                break;
            }
          }
          this.m_resultIDs = new List<VRage.ModAPI.IMyEntity>();
          MyEntityIdentifier.GetPerThreadEntities(this.m_resultIDs);
          MyEntityIdentifier.ClearPerThreadEntities();
          MyEntityIdentifier.InEntityCreationBlock = false;
        }
      }

      private bool TestPastedGridPlacement(MyCubeGrid grid, bool testPhysics)
      {
        MyGridPlacementSettings placementSettings = MyClipboardComponent.ClipboardDefinition.PastingSettings.GetGridPlacementSettings(grid.GridSizeEnum, grid.IsStatic);
        return MyCubeGrid.TestPlacementArea(grid, grid.IsStatic, ref placementSettings, (BoundingBoxD) grid.PositionComp.LocalAABB, !grid.IsStatic, testPhysics: testPhysics);
      }

      public void Callback()
      {
        if (!this.IsLocallyInvoked)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyCubeGrid.SendHudNotificationAfterPaste)), this.SenderEndpointId);
        else if (!Sandbox.Engine.Platform.Game.IsDedicated)
          MyHud.PopRotatingWheelVisible();
        if (this.m_canPlaceGrid)
        {
          foreach (MyCubeGrid pastedGrid in this.m_pastedGrids)
          {
            this.m_canPlaceGrid &= this.TestPastedGridPlacement(pastedGrid, true);
            if (!this.m_canPlaceGrid)
              break;
          }
        }
        if (this.m_offset.HasValue)
        {
          foreach (MyCubeGrid pastedGrid in this.m_pastedGrids)
          {
            MatrixD worldMatrix = pastedGrid.WorldMatrix;
            worldMatrix.Translation += this.m_offset.Value;
            pastedGrid.WorldMatrix = worldMatrix;
          }
        }
        if (this.m_canPlaceGrid && this.m_pastedGrids.Count > 0)
        {
          foreach (VRage.ModAPI.IMyEntity resultId in this.m_resultIDs)
          {
            VRage.ModAPI.IMyEntity entity;
            MyEntityIdentifier.TryGetEntity(resultId.EntityId, out entity);
            if (entity == null)
              MyEntityIdentifier.AddEntityWithId(resultId);
          }
          MyCubeGrid.AfterPaste(this.m_pastedGrids, this.m_objectVelocity, this.m_detectDisconnects);
        }
        else
        {
          if (this.m_pastedGrids != null)
          {
            foreach (MyCubeGrid pastedGrid in this.m_pastedGrids)
            {
              foreach (MySlimBlock block in pastedGrid.GetBlocks())
                block.RemoveAuthorship();
              pastedGrid.Close();
            }
          }
          if (this.IsLocallyInvoked)
            return;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyCubeGrid.ShowPasteFailedOperation)), this.SenderEndpointId);
        }
      }
    }

    [Serializable]
    public struct RelativeOffset
    {
      public bool Use;
      public bool RelativeToEntity;
      public long SpawnerId;
      public Vector3D OriginalSpawnPoint;

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ERelativeOffset\u003C\u003EUse\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.RelativeOffset, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.RelativeOffset owner, in bool value) => owner.Use = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.RelativeOffset owner, out bool value) => value = owner.Use;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ERelativeOffset\u003C\u003ERelativeToEntity\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.RelativeOffset, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.RelativeOffset owner, in bool value) => owner.RelativeToEntity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.RelativeOffset owner, out bool value) => value = owner.RelativeToEntity;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ERelativeOffset\u003C\u003ESpawnerId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.RelativeOffset, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.RelativeOffset owner, in long value) => owner.SpawnerId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.RelativeOffset owner, out long value) => value = owner.SpawnerId;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ERelativeOffset\u003C\u003EOriginalSpawnPoint\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.RelativeOffset, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.RelativeOffset owner, in Vector3D value) => owner.OriginalSpawnPoint = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.RelativeOffset owner, out Vector3D value) => value = owner.OriginalSpawnPoint;
      }
    }

    [Serializable]
    public struct MyPasteGridParameters
    {
      [Serialize(MyObjectFlags.DefaultZero)]
      public List<MyObjectBuilder_CubeGrid> Entities;
      [Serialize(MyObjectFlags.DefaultZero)]
      public List<ulong> ClientsideDLCs;
      public bool DetectDisconnects;
      public bool MultiBlock;
      public bool InstantBuild;
      public Vector3 ObjectVelocity;
      public MyCubeGrid.RelativeOffset Offset;

      public MyPasteGridParameters(
        List<MyObjectBuilder_CubeGrid> entities,
        bool detectDisconnects,
        bool multiBlock,
        Vector3 objectVelocity,
        bool instantBuild,
        MyCubeGrid.RelativeOffset offset,
        List<ulong> clientsideDLCs)
      {
        this.Entities = entities;
        this.ClientsideDLCs = clientsideDLCs;
        this.DetectDisconnects = detectDisconnects;
        this.MultiBlock = multiBlock;
        this.InstantBuild = instantBuild;
        this.ObjectVelocity = objectVelocity;
        this.Offset = offset;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EEntities\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, List<MyObjectBuilder_CubeGrid>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCubeGrid.MyPasteGridParameters owner,
          in List<MyObjectBuilder_CubeGrid> value)
        {
          owner.Entities = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCubeGrid.MyPasteGridParameters owner,
          out List<MyObjectBuilder_CubeGrid> value)
        {
          value = owner.Entities;
        }
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EClientsideDLCs\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, List<ulong>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyPasteGridParameters owner, in List<ulong> value) => owner.ClientsideDLCs = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyPasteGridParameters owner, out List<ulong> value) => value = owner.ClientsideDLCs;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EDetectDisconnects\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyPasteGridParameters owner, in bool value) => owner.DetectDisconnects = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyPasteGridParameters owner, out bool value) => value = owner.DetectDisconnects;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EMultiBlock\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyPasteGridParameters owner, in bool value) => owner.MultiBlock = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyPasteGridParameters owner, out bool value) => value = owner.MultiBlock;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EInstantBuild\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyPasteGridParameters owner, in bool value) => owner.InstantBuild = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyPasteGridParameters owner, out bool value) => value = owner.InstantBuild;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EObjectVelocity\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MyPasteGridParameters owner, in Vector3 value) => owner.ObjectVelocity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MyPasteGridParameters owner, out Vector3 value) => value = owner.ObjectVelocity;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters\u003C\u003EOffset\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MyPasteGridParameters, MyCubeGrid.RelativeOffset>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCubeGrid.MyPasteGridParameters owner,
          in MyCubeGrid.RelativeOffset value)
        {
          owner.Offset = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCubeGrid.MyPasteGridParameters owner,
          out MyCubeGrid.RelativeOffset value)
        {
          value = owner.Offset;
        }
      }
    }

    [ProtoContract]
    public struct MySingleOwnershipRequest
    {
      [ProtoMember(28)]
      public long BlockId;
      [ProtoMember(31)]
      public long Owner;

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMySingleOwnershipRequest\u003C\u003EBlockId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MySingleOwnershipRequest, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MySingleOwnershipRequest owner, in long value) => owner.BlockId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MySingleOwnershipRequest owner, out long value) => value = owner.BlockId;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMySingleOwnershipRequest\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.MySingleOwnershipRequest, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.MySingleOwnershipRequest owner, in long value) => owner.Owner = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.MySingleOwnershipRequest owner, out long value) => value = owner.Owner;
      }

      private class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMySingleOwnershipRequest\u003C\u003EActor : IActivator, IActivator<MyCubeGrid.MySingleOwnershipRequest>
      {
        object IActivator.CreateInstance() => (object) new MyCubeGrid.MySingleOwnershipRequest();

        MyCubeGrid.MySingleOwnershipRequest IActivator<MyCubeGrid.MySingleOwnershipRequest>.CreateInstance() => new MyCubeGrid.MySingleOwnershipRequest();
      }
    }

    [ProtoContract]
    public struct LocationIdentity
    {
      [ProtoMember(34)]
      public Vector3I Location;
      [ProtoMember(37)]
      public ushort Id;

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ELocationIdentity\u003C\u003ELocation\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.LocationIdentity, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.LocationIdentity owner, in Vector3I value) => owner.Location = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.LocationIdentity owner, out Vector3I value) => value = owner.Location;
      }

      protected class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ELocationIdentity\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyCubeGrid.LocationIdentity, ushort>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeGrid.LocationIdentity owner, in ushort value) => owner.Id = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeGrid.LocationIdentity owner, out ushort value) => value = owner.Id;
      }

      private class Sandbox_Game_Entities_MyCubeGrid\u003C\u003ELocationIdentity\u003C\u003EActor : IActivator, IActivator<MyCubeGrid.LocationIdentity>
      {
        object IActivator.CreateInstance() => (object) new MyCubeGrid.LocationIdentity();

        MyCubeGrid.LocationIdentity IActivator<MyCubeGrid.LocationIdentity>.CreateInstance() => new MyCubeGrid.LocationIdentity();
      }
    }

    public class MyCubeGridHitInfo
    {
      public MyIntersectionResultLineTriangleEx Triangle;
      public Vector3I Position;
      public int CubePartIndex = -1;

      public void Reset()
      {
        this.Triangle = new MyIntersectionResultLineTriangleEx();
        this.Position = new Vector3I();
        this.CubePartIndex = -1;
      }
    }

    private class AreaConnectivityTest : IMyGridConnectivityTest
    {
      private readonly Dictionary<Vector3I, Vector3I> m_lookup = new Dictionary<Vector3I, Vector3I>();
      private MyBlockOrientation m_orientation;
      private MyCubeBlockDefinition m_definition;
      private Vector3I m_posInGrid;
      private Vector3I m_blockMin;
      private Vector3I m_blockMax;
      private Vector3I m_stepDelta;

      public void Initialize(ref MyCubeGrid.MyBlockBuildArea area, MyCubeBlockDefinition definition)
      {
        this.m_definition = definition;
        this.m_orientation = new MyBlockOrientation(area.OrientationForward, area.OrientationUp);
        this.m_posInGrid = area.PosInGrid;
        this.m_blockMin = (Vector3I) area.BlockMin;
        this.m_blockMax = (Vector3I) area.BlockMax;
        this.m_stepDelta = (Vector3I) area.StepDelta;
        this.m_lookup.Clear();
      }

      public void AddBlock(Vector3UByte offset)
      {
        Vector3I vector3I1 = this.m_posInGrid + (Vector3I) offset * this.m_stepDelta;
        Vector3I vector3I2;
        for (vector3I2.X = this.m_blockMin.X; vector3I2.X <= this.m_blockMax.X; ++vector3I2.X)
        {
          for (vector3I2.Y = this.m_blockMin.Y; vector3I2.Y <= this.m_blockMax.Y; ++vector3I2.Y)
          {
            for (vector3I2.Z = this.m_blockMin.Z; vector3I2.Z <= this.m_blockMax.Z; ++vector3I2.Z)
              this.m_lookup.Add(vector3I1 + vector3I2, vector3I1);
          }
        }
      }

      public void GetConnectedBlocks(
        Vector3I minI,
        Vector3I maxI,
        Dictionary<Vector3I, ConnectivityResult> outOverlappedCubeBlocks)
      {
        Vector3I key1;
        for (key1.X = minI.X; key1.X <= maxI.X; ++key1.X)
        {
          for (key1.Y = minI.Y; key1.Y <= maxI.Y; ++key1.Y)
          {
            for (key1.Z = minI.Z; key1.Z <= maxI.Z; ++key1.Z)
            {
              Vector3I key2;
              if (this.m_lookup.TryGetValue(key1, out key2) && !outOverlappedCubeBlocks.ContainsKey(key2))
                outOverlappedCubeBlocks.Add(key2, new ConnectivityResult()
                {
                  Definition = this.m_definition,
                  FatBlock = (MyCubeBlock) null,
                  Position = key2,
                  Orientation = this.m_orientation
                });
            }
          }
        }
      }
    }

    private struct TriangleWithMaterial
    {
      public MyTriangleVertexIndices triangle;
      public MyTriangleVertexIndices uvIndices;
      public string material;
    }

    public enum UpdateQueue : byte
    {
      Invalid = 0,
      BeforeSimulation = 1,
      OnceBeforeSimulation = 2,
      AfterSimulation = 3,
      OnceAfterSimulation = 4,
      QueueCount = 4,
    }

    [DebuggerDisplay("{DebuggerDisplay}")]
    internal struct Update
    {
      public Action Callback;
      public int Priority;
      public bool Parallel;
      public static readonly MyCubeGrid.Update Empty = new MyCubeGrid.Update((Action) null, int.MaxValue, false);

      public Update(Action callback, int priority, bool parallel)
      {
        this.Callback = callback;
        this.Priority = priority;
        this.Parallel = parallel;
      }

      public bool Removed => this.Callback == null;

      private string DebuggerDisplay => !this.Removed ? string.Format("{0} ({1}) {2}", (object) MyCubeGrid.DebugFormatMethodName(this.Callback.Method), (object) this.Priority, this.Parallel ? (object) "P" : (object) "") : "Removed";

      public void SetRemoved() => this.Callback = (Action) null;
    }

    private struct QueuedUpdateChange
    {
      public Action Callback;
      public int Priority;
      public MyCubeGrid.UpdateQueue Queue;
      public MyCubeGrid Grid;
      public bool Parallel;
      public bool Add;

      public static MyCubeGrid.QueuedUpdateChange MakeAdd(
        Action callback,
        int priority,
        MyCubeGrid.UpdateQueue queue,
        MyCubeGrid grid,
        bool parallel)
      {
        return new MyCubeGrid.QueuedUpdateChange()
        {
          Callback = callback,
          Priority = priority,
          Queue = queue,
          Grid = grid,
          Parallel = parallel,
          Add = true
        };
      }

      public static MyCubeGrid.QueuedUpdateChange MakeRemove(
        Action callback,
        MyCubeGrid.UpdateQueue queue,
        MyCubeGrid grid)
      {
        return new MyCubeGrid.QueuedUpdateChange()
        {
          Callback = callback,
          Queue = queue,
          Grid = grid,
          Add = false
        };
      }
    }

    public struct DebugUpdateRecord
    {
      public MethodInfo Method;
      public MyCubeGrid.UpdateQueue Queue;
      public int Priority;

      public DebugUpdateRecord(MethodInfo method, MyCubeGrid.UpdateQueue queue, int priority)
      {
        this.Method = method;
        this.Queue = queue;
        this.Priority = priority;
      }

      internal DebugUpdateRecord(in MyCubeGrid.Update update, MyCubeGrid.UpdateQueue queue)
      {
        this.Method = update.Callback.Method;
        this.Priority = update.Priority;
        this.Queue = queue;
      }

      public override string ToString() => string.Format("{0}: {1} ({2})", (object) this.Queue, (object) MyCubeGrid.DebugFormatMethodName(this.Method), (object) this.Priority);

      public static IEqualityComparer<MyCubeGrid.DebugUpdateRecord> Comparer { get; } = (IEqualityComparer<MyCubeGrid.DebugUpdateRecord>) new MyCubeGrid.DebugUpdateRecord.MethodQueueEqualityComparer();

      private sealed class MethodQueueEqualityComparer : IEqualityComparer<MyCubeGrid.DebugUpdateRecord>
      {
        public bool Equals(MyCubeGrid.DebugUpdateRecord x, MyCubeGrid.DebugUpdateRecord y) => object.Equals((object) x.Method, (object) y.Method) && x.Queue == y.Queue;

        public int GetHashCode(MyCubeGrid.DebugUpdateRecord obj) => (int) ((MyCubeGrid.UpdateQueue) ((obj.Method != (MethodInfo) null ? obj.Method.GetHashCode() : 0) * 397) ^ obj.Queue);
      }
    }

    public struct DebugUpdateStats
    {
      public Queue<int> Calls;

      public int LastFrame => this.Calls.First<int>();

      public DebugUpdateStats(int frame)
      {
        this.Calls = new Queue<int>();
        this.Calls.Enqueue(frame);
      }

      public override string ToString() => string.Format("{0}, {1}", (object) this.LastFrame, (object) this.Calls.Count);
    }

    protected sealed class OnGridChangedRPC\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnGridChangedRPC();
      }
    }

    protected sealed class CreateSplit_Implementation\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E\u0023System_Int64 : ICallSite<MyCubeGrid, List<Vector3I>, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<Vector3I> blocks,
        in long newEntityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateSplit_Implementation(blocks, newEntityId);
      }
    }

    protected sealed class CreateSplits_Implementation\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Entities_Cube_MyDisconnectHelper\u003C\u003EGroup\u003E : ICallSite<MyCubeGrid, List<Vector3I>, List<MyDisconnectHelper.Group>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<Vector3I> blocks,
        in List<MyDisconnectHelper.Group> groups,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateSplits_Implementation(blocks, groups);
      }
    }

    protected sealed class ShowMessageGridsRemovedWhilePasting\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyCubeGrid.ShowMessageGridsRemovedWhilePasting();
      }
    }

    protected sealed class RemovedBlocks\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E\u0023System_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E\u0023System_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E : ICallSite<MyCubeGrid, List<Vector3I>, List<Vector3I>, List<Vector3I>, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<Vector3I> destroyLocations,
        in List<Vector3I> DestructionDeformationLocation,
        in List<Vector3I> LocationsWithoutGenerator,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RemovedBlocks(destroyLocations, DestructionDeformationLocation, LocationsWithoutGenerator);
      }
    }

    protected sealed class RemovedBlocksWithIds\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003EBlockPositionId\u003E\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003EBlockPositionId\u003E : ICallSite<MyCubeGrid, List<MyCubeGrid.BlockPositionId>, List<MyCubeGrid.BlockPositionId>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<MyCubeGrid.BlockPositionId> destroyBlockWithIdQueueWithoutGenerators,
        in List<MyCubeGrid.BlockPositionId> removeBlockWithIdQueueWithoutGenerators,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RemovedBlocksWithIds(destroyBlockWithIdQueueWithoutGenerators, removeBlockWithIdQueueWithoutGenerators);
      }
    }

    protected sealed class RemoveBlocksBuiltByID\u003C\u003ESystem_Int64 : ICallSite<MyCubeGrid, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long identityID,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RemoveBlocksBuiltByID(identityID);
      }
    }

    protected sealed class TransferBlocksBuiltByID\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<MyCubeGrid, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long oldAuthor,
        in long newAuthor,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.TransferBlocksBuiltByID(oldAuthor, newAuthor);
      }
    }

    protected sealed class BuildBlockRequest\u003C\u003ESystem_UInt32\u0023Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u0023VRage_Game_MyObjectBuilder_CubeBlock\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, uint, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in uint colorMaskHsv,
        in MyCubeGrid.MyBlockLocation location,
        in MyObjectBuilder_CubeBlock blockObjectBuilder,
        in long builderEntityId,
        in bool instantBuild,
        in long ownerId)
      {
        @this.BuildBlockRequest(colorMaskHsv, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
      }
    }

    protected sealed class BuildBlockRequest\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u0023VRage_Game_MyObjectBuilder_CubeBlock\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockVisuals visuals,
        in MyCubeGrid.MyBlockLocation location,
        in MyObjectBuilder_CubeBlock blockObjectBuilder,
        in long builderEntityId,
        in bool instantBuild,
        in long ownerId)
      {
        @this.BuildBlockRequest(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
      }
    }

    protected sealed class BuildBlockSucess\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u0023VRage_Game_MyObjectBuilder_CubeBlock\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockVisuals, MyCubeGrid.MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockVisuals visuals,
        in MyCubeGrid.MyBlockLocation location,
        in MyObjectBuilder_CubeBlock blockObjectBuilder,
        in long builderEntityId,
        in bool instantBuild,
        in long ownerId)
      {
        @this.BuildBlockSucess(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
      }
    }

    protected sealed class BuildBlocksRequest\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023System_Collections_Generic_HashSet`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003E\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockVisuals visuals,
        in HashSet<MyCubeGrid.MyBlockLocation> locations,
        in long builderEntityId,
        in bool instantBuild,
        in long ownerId,
        in DBNull arg6)
      {
        @this.BuildBlocksRequest(visuals, locations, builderEntityId, instantBuild, ownerId);
      }
    }

    protected sealed class BuildBlocksFailedNotify\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.BuildBlocksFailedNotify();
      }
    }

    protected sealed class BuildBlocksClient\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023System_Collections_Generic_HashSet`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockLocation\u003E\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockVisuals, HashSet<MyCubeGrid.MyBlockLocation>, long, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockVisuals visuals,
        in HashSet<MyCubeGrid.MyBlockLocation> locations,
        in long builderEntityId,
        in bool instantBuild,
        in long ownerId,
        in DBNull arg6)
      {
        @this.BuildBlocksClient(visuals, locations, builderEntityId, instantBuild, ownerId);
      }
    }

    protected sealed class BuildBlocksAreaRequest\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u0023System_Int64\u0023System_Boolean\u0023System_Int64\u0023System_UInt64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockBuildArea, long, bool, long, ulong, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockBuildArea area,
        in long builderEntityId,
        in bool instantBuild,
        in long ownerId,
        in ulong placingPlayer,
        in DBNull arg6)
      {
        @this.BuildBlocksAreaRequest(area, builderEntityId, instantBuild, ownerId, placingPlayer);
      }
    }

    protected sealed class BuildBlocksAreaClient\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockBuildArea\u0023System_Int32\u0023System_Collections_Generic_HashSet`1\u003CVRageMath_Vector3UByte\u003E\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockBuildArea area,
        in int entityIdSeed,
        in HashSet<Vector3UByte> failList,
        in long builderEntityId,
        in bool isAdmin,
        in long ownerId)
      {
        @this.BuildBlocksAreaClient(area, entityIdSeed, failList, builderEntityId, isAdmin, ownerId);
      }
    }

    protected sealed class RazeBlocksAreaRequest\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3UByte\u0023System_Int64\u0023System_UInt64 : ICallSite<MyCubeGrid, Vector3I, Vector3UByte, long, ulong, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I pos,
        in Vector3UByte size,
        in long builderEntityId,
        in ulong placingPlayer,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RazeBlocksAreaRequest(pos, size, builderEntityId, placingPlayer);
      }
    }

    protected sealed class RazeBlocksAreaSuccess\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3UByte\u0023System_Collections_Generic_HashSet`1\u003CVRageMath_Vector3UByte\u003E : ICallSite<MyCubeGrid, Vector3I, Vector3UByte, HashSet<Vector3UByte>, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I pos,
        in Vector3UByte size,
        in HashSet<Vector3UByte> resultFailList,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RazeBlocksAreaSuccess(pos, size, resultFailList);
      }
    }

    protected sealed class RazeBlocksRequest\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E\u0023System_Int64\u0023System_UInt64 : ICallSite<MyCubeGrid, List<Vector3I>, long, ulong, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<Vector3I> locations,
        in long builderEntityId,
        in ulong user,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RazeBlocksRequest(locations, builderEntityId, user);
      }
    }

    protected sealed class RazeBlocksClient\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E : ICallSite<MyCubeGrid, List<Vector3I>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<Vector3I> locations,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RazeBlocksClient(locations);
      }
    }

    protected sealed class ColorGridFriendlyRequest\u003C\u003EVRageMath_Vector3\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3 newHSV,
        in bool playSound,
        in long player,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ColorGridFriendlyRequest(newHSV, playSound, player);
      }
    }

    protected sealed class OnColorGridFriendly\u003C\u003EVRageMath_Vector3\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3 newHSV,
        in bool playSound,
        in long player,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnColorGridFriendly(newHSV, playSound, player);
      }
    }

    protected sealed class ColorBlockRequest\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3I\u0023VRageMath_Vector3\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I min,
        in Vector3I max,
        in Vector3 newHSV,
        in bool playSound,
        in long player,
        in DBNull arg6)
      {
        @this.ColorBlockRequest(min, max, newHSV, playSound, player);
      }
    }

    protected sealed class OnColorBlock\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3I\u0023VRageMath_Vector3\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I min,
        in Vector3I max,
        in Vector3 newHSV,
        in bool playSound,
        in long player,
        in DBNull arg6)
      {
        @this.OnColorBlock(min, max, newHSV, playSound, player);
      }
    }

    protected sealed class SkinGridFriendlyRequest\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockVisuals, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockVisuals visuals,
        in bool playSound,
        in long player,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SkinGridFriendlyRequest(visuals, playSound, player);
      }
    }

    protected sealed class OnSkinGridFriendly\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, MyCubeGrid.MyBlockVisuals, bool, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyBlockVisuals visuals,
        in bool playSound,
        in long player,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSkinGridFriendly(visuals, playSound, player);
      }
    }

    protected sealed class SkinBlockRequest\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3I\u0023Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I min,
        in Vector3I max,
        in MyCubeGrid.MyBlockVisuals visuals,
        in bool playSound,
        in long player,
        in DBNull arg6)
      {
        @this.SkinBlockRequest(min, max, visuals, playSound, player);
      }
    }

    protected sealed class OnSkinBlock\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3I\u0023Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals\u0023System_Boolean\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, MyCubeGrid.MyBlockVisuals, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I min,
        in Vector3I max,
        in MyCubeGrid.MyBlockVisuals visuals,
        in bool playSound,
        in long player,
        in DBNull arg6)
      {
        @this.OnSkinBlock(min, max, visuals, playSound, player);
      }
    }

    protected sealed class OnConvertToDynamic\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConvertToDynamic();
      }
    }

    protected sealed class ConvertToStatic\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ConvertToStatic();
      }
    }

    protected sealed class BlockIntegrityChanged\u003C\u003EVRageMath_Vector3I\u0023System_UInt16\u0023System_Single\u0023System_Single\u0023VRage_Game_ModAPI_MyIntegrityChangeEnum\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, ushort, float, float, MyIntegrityChangeEnum, long>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I pos,
        in ushort subBlockId,
        in float buildIntegrity,
        in float integrity,
        in MyIntegrityChangeEnum integrityChangeType,
        in long grinderOwner)
      {
        @this.BlockIntegrityChanged(pos, subBlockId, buildIntegrity, integrity, integrityChangeType, grinderOwner);
      }
    }

    protected sealed class BlockStockpileChanged\u003C\u003EVRageMath_Vector3I\u0023System_UInt16\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyStockpileItem\u003E : ICallSite<MyCubeGrid, Vector3I, ushort, List<MyStockpileItem>, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I pos,
        in ushort subBlockId,
        in List<MyStockpileItem> items,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.BlockStockpileChanged(pos, subBlockId, items);
      }
    }

    protected sealed class FractureComponentRepaired\u003C\u003EVRageMath_Vector3I\u0023System_UInt16\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, ushort, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I pos,
        in ushort subBlockId,
        in long toolOwner,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.FractureComponentRepaired(pos, subBlockId, toolOwner);
      }
    }

    protected sealed class PasteBlocksToGridServer_Implementation\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_MyObjectBuilder_CubeGrid\u003E\u0023System_Int64\u0023System_Boolean\u0023System_Boolean : ICallSite<MyCubeGrid, List<MyObjectBuilder_CubeGrid>, long, bool, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<MyObjectBuilder_CubeGrid> gridsToMerge,
        in long inventoryEntityId,
        in bool multiBlock,
        in bool instantBuild,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PasteBlocksToGridServer_Implementation(gridsToMerge, inventoryEntityId, multiBlock, instantBuild);
      }
    }

    protected sealed class PasteBlocksToGridClient_Implementation\u003C\u003EVRage_Game_MyObjectBuilder_CubeGrid\u0023VRageMath_MatrixI : ICallSite<MyCubeGrid, MyObjectBuilder_CubeGrid, MatrixI, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyObjectBuilder_CubeGrid gridToMerge,
        in MatrixI mergeTransform,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PasteBlocksToGridClient_Implementation(gridToMerge, mergeTransform);
      }
    }

    protected sealed class TryCreateGrid_Implementation\u003C\u003EVRage_Game_MyCubeSize\u0023System_Boolean\u0023VRage_MyPositionAndOrientation\u0023System_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, MyCubeSize, bool, MyPositionAndOrientation, long, bool, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyCubeSize cubeSize,
        in bool isStatic,
        in MyPositionAndOrientation position,
        in long inventoryEntityId,
        in bool instantBuild,
        in DBNull arg6)
      {
        MyCubeGrid.TryCreateGrid_Implementation(cubeSize, isStatic, position, inventoryEntityId, instantBuild);
      }
    }

    protected sealed class StationClosingDenied\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyCubeGrid.StationClosingDenied();
      }
    }

    protected sealed class OnGridClosedRequest\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCubeGrid.OnGridClosedRequest(entityId);
      }
    }

    protected sealed class TryPasteGrid_Implementation\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyPasteGridParameters : ICallSite<IMyEventOwner, MyCubeGrid.MyPasteGridParameters, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyCubeGrid.MyPasteGridParameters parameters,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCubeGrid.TryPasteGrid_Implementation(parameters);
      }
    }

    protected sealed class ShowPasteFailedOperation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyCubeGrid.ShowPasteFailedOperation();
      }
    }

    protected sealed class SendHudNotificationAfterPaste\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyCubeGrid.SendHudNotificationAfterPaste();
      }
    }

    protected sealed class OnBonesReceived\u003C\u003ESystem_Int32\u0023System_Collections_Generic_List`1\u003CSystem_Byte\u003E : ICallSite<MyCubeGrid, int, List<byte>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in int segmentsCount,
        in List<byte> boneByteList,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnBonesReceived(segmentsCount, boneByteList);
      }
    }

    protected sealed class OnBonesMultiplied\u003C\u003EVRageMath_Vector3I\u0023System_Single : ICallSite<MyCubeGrid, Vector3I, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I blockLocation,
        in float multiplier,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnBonesMultiplied(blockLocation, multiplier);
      }
    }

    protected sealed class RelfectorStateRecived\u003C\u003EVRage_MyMultipleEnabledEnum : ICallSite<MyCubeGrid, MyMultipleEnabledEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyMultipleEnabledEnum value,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RelfectorStateRecived(value);
      }
    }

    protected sealed class OnStockpileFillRequest\u003C\u003EVRageMath_Vector3I\u0023System_Int64\u0023System_Byte : ICallSite<MyCubeGrid, Vector3I, long, byte, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I blockPosition,
        in long ownerEntityId,
        in byte inventoryIndex,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnStockpileFillRequest(blockPosition, ownerEntityId, inventoryIndex);
      }
    }

    protected sealed class OnSetToConstructionRequest\u003C\u003EVRageMath_Vector3I\u0023System_Int64\u0023System_Byte\u0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, long, byte, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in Vector3I blockPosition,
        in long ownerEntityId,
        in byte inventoryIndex,
        in long requestingPlayer,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSetToConstructionRequest(blockPosition, ownerEntityId, inventoryIndex, requestingPlayer);
      }
    }

    protected sealed class OnPowerProducerStateRequest\u003C\u003EVRage_MyMultipleEnabledEnum\u0023System_Int64 : ICallSite<MyCubeGrid, MyMultipleEnabledEnum, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyMultipleEnabledEnum enabledState,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnPowerProducerStateRequest(enabledState, playerId);
      }
    }

    protected sealed class OnConvertedToShipRequest\u003C\u003ESandbox_Game_Entities_MyCubeGrid\u003C\u003EMyTestDynamicReason : ICallSite<MyCubeGrid, MyCubeGrid.MyTestDynamicReason, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in MyCubeGrid.MyTestDynamicReason reason,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConvertedToShipRequest(reason);
      }
    }

    protected sealed class OnConvertToShipFailed\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConvertToShipFailed();
      }
    }

    protected sealed class OnConvertedToStationRequest\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConvertedToStationRequest();
      }
    }

    protected sealed class OnChangeOwnerRequest\u003C\u003ESystem_Int64\u0023System_Int64\u0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long blockId,
        in long owner,
        in MyOwnershipShareModeEnum shareMode,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOwnerRequest(blockId, owner, shareMode);
      }
    }

    protected sealed class OnChangeOwner\u003C\u003ESystem_Int64\u0023System_Int64\u0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long blockId,
        in long owner,
        in MyOwnershipShareModeEnum shareMode,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOwner(blockId, owner, shareMode);
      }
    }

    protected sealed class SetHandbrakeRequest\u003C\u003ESystem_Boolean : ICallSite<MyCubeGrid, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in bool v,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetHandbrakeRequest(v);
      }
    }

    protected sealed class OnChangeGridOwner\u003C\u003ESystem_Int64\u0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long playerId,
        in MyOwnershipShareModeEnum shareMode,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeGridOwner(playerId, shareMode);
      }
    }

    protected sealed class OnRemoveSplit\u003C\u003ESystem_Collections_Generic_List`1\u003CVRageMath_Vector3I\u003E : ICallSite<MyCubeGrid, List<Vector3I>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<Vector3I> removedBlocks,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSplit(removedBlocks);
      }
    }

    protected sealed class OnChangeDisplayNameRequest\u003C\u003ESystem_String : ICallSite<MyCubeGrid, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in string displayName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDisplayNameRequest(displayName);
      }
    }

    protected sealed class OnModifyGroupSuccess\u003C\u003ESystem_String\u0023System_Collections_Generic_List`1\u003CSystem_Int64\u003E : ICallSite<MyCubeGrid, string, List<long>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in string name,
        in List<long> blocks,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnModifyGroupSuccess(name, blocks);
      }
    }

    protected sealed class OnRazeBlockInCompoundBlockRequest\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003ELocationIdentity\u003E : ICallSite<MyCubeGrid, List<MyCubeGrid.LocationIdentity>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<MyCubeGrid.LocationIdentity> locationsAndIds,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRazeBlockInCompoundBlockRequest(locationsAndIds);
      }
    }

    protected sealed class OnRazeBlockInCompoundBlockSuccess\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003ELocationIdentity\u003E : ICallSite<MyCubeGrid, List<MyCubeGrid.LocationIdentity>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in List<MyCubeGrid.LocationIdentity> locationsAndIds,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRazeBlockInCompoundBlockSuccess(locationsAndIds);
      }
    }

    protected sealed class OnChangeOwnersRequest\u003C\u003EVRage_Game_MyOwnershipShareModeEnum\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003EMySingleOwnershipRequest\u003E\u0023System_Int64 : ICallSite<IMyEventOwner, MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyOwnershipShareModeEnum shareMode,
        in List<MyCubeGrid.MySingleOwnershipRequest> requests,
        in long requestingPlayer,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCubeGrid.OnChangeOwnersRequest(shareMode, requests, requestingPlayer);
      }
    }

    protected sealed class OnChangeOwnersSuccess\u003C\u003EVRage_Game_MyOwnershipShareModeEnum\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyCubeGrid\u003C\u003EMySingleOwnershipRequest\u003E : ICallSite<IMyEventOwner, MyOwnershipShareModeEnum, List<MyCubeGrid.MySingleOwnershipRequest>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyOwnershipShareModeEnum shareMode,
        in List<MyCubeGrid.MySingleOwnershipRequest> requests,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCubeGrid.OnChangeOwnersSuccess(shareMode, requests);
      }
    }

    protected sealed class OnLogHierarchy\u003C\u003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnLogHierarchy();
      }
    }

    protected sealed class DepressurizeEffect\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023VRageMath_Vector3I : ICallSite<IMyEventOwner, long, Vector3I, Vector3I, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridId,
        in Vector3I from,
        in Vector3I to,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCubeGrid.DepressurizeEffect(gridId, from, to);
      }
    }

    protected sealed class MergeGrid_MergeClient\u003C\u003ESystem_Int64\u0023VRage_SerializableVector3I\u0023VRageMath_Base6Directions\u003C\u003EDirection\u0023VRageMath_Base6Directions\u003C\u003EDirection\u0023VRageMath_Vector3I : ICallSite<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long gridId,
        in SerializableVector3I gridOffset,
        in Base6Directions.Direction gridForward,
        in Base6Directions.Direction gridUp,
        in Vector3I mergingBlockPos,
        in DBNull arg6)
      {
        @this.MergeGrid_MergeClient(gridId, gridOffset, gridForward, gridUp, mergingBlockPos);
      }
    }

    protected sealed class MergeGrid_MergeBlockClient\u003C\u003ESystem_Int64\u0023VRage_SerializableVector3I\u0023VRageMath_Base6Directions\u003C\u003EDirection\u0023VRageMath_Base6Directions\u003C\u003EDirection : ICallSite<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyCubeGrid @this,
        in long gridId,
        in SerializableVector3I gridOffset,
        in Base6Directions.Direction gridForward,
        in Base6Directions.Direction gridUp,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.MergeGrid_MergeBlockClient(gridId, gridOffset, gridForward, gridUp);
      }
    }

    protected class m_handBrakeSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCubeGrid) obj0).m_handBrakeSync = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_dampenersEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCubeGrid) obj0).m_dampenersEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_markedAsTrash\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyCubeGrid) obj0).m_markedAsTrash = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class GridGeneralDamageModifier\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyCubeGrid) obj0).GridGeneralDamageModifier = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isRespawnGrid\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCubeGrid) obj0).m_isRespawnGrid = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_destructibleBlocks\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCubeGrid) obj0).m_destructibleBlocks = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_immune\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCubeGrid) obj0).m_immune = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_editable\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyCubeGrid) obj0).m_editable = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyCubeGrid\u003C\u003EActor : IActivator, IActivator<MyCubeGrid>
    {
      object IActivator.CreateInstance() => (object) new MyCubeGrid();

      MyCubeGrid IActivator<MyCubeGrid>.CreateInstance() => new MyCubeGrid();
    }
  }
}
