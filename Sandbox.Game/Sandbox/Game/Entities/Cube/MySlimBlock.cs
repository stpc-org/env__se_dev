// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MySlimBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  [StaticEventOwner]
  [GenerateFieldAccessors]
  [MyCubeBlockType(typeof (MyObjectBuilder_CubeBlock))]
  public class MySlimBlock : IMyDestroyableObject, IMyDecalProxy, VRage.Game.ModAPI.IMySlimBlock, VRage.Game.ModAPI.Ingame.IMySlimBlock
  {
    private static List<VertexArealBoneIndexWeight> m_boneIndexWeightTmp;
    private static MySoundPair CONSTRUCTION_START = new MySoundPair("PrgConstrPh01Start");
    private static MySoundPair CONSTRUCTION_PROG = new MySoundPair("PrgConstrPh02Proc");
    private static MySoundPair CONSTRUCTION_END = new MySoundPair("PrgConstrPh03Fin");
    private static MySoundPair DECONSTRUCTION_START = new MySoundPair("PrgDeconstrPh01Start");
    private static MySoundPair DECONSTRUCTION_PROG = new MySoundPair("PrgDeconstrPh02Proc");
    private static MySoundPair DECONSTRUCTION_END = new MySoundPair("PrgDeconstrPh03Fin");
    [ThreadStatic]
    private static Dictionary<string, int> m_tmpComponentsPerThread;
    [ThreadStatic]
    private static List<MyStockpileItem> m_tmpItemListPerThread;
    [ThreadStatic]
    private static List<Vector3I> m_tmpCubeNeighboursPerThread;
    [ThreadStatic]
    private static List<MySlimBlock> m_tmpBlocksPerThread;
    [ThreadStatic]
    private static List<MySlimBlock> m_tmpMultiBlocksPerThread;
    public static readonly MyTimedItemCache ConstructionParticlesTimedCache = new MyTimedItemCache(350);
    public static double ConstructionParticleSpaceMapping = 1.0;
    private float m_accumulatedDamage;
    public MyCubeBlockDefinition BlockDefinition;
    public Vector3I Min;
    public Vector3I Max;
    public MyBlockOrientation Orientation = MyBlockOrientation.Identity;
    public Vector3I Position;
    public float BlockGeneralDamageModifier = 1f;
    private MyCubeGrid m_cubeGrid;
    private Vector3 m_colorMaskHSV;
    public MyStringHash SkinSubtypeId;
    public float Dithering;
    public bool UsesDeformation = true;
    private float m_deformationRatio;
    private MyComponentStack m_componentStack;
    private MyConstructionStockpile m_stockpile;
    private float m_cachedMaxDeformation;
    private long m_builtByID;
    public List<MySlimBlock> Neighbours = new List<MySlimBlock>();
    public float m_lastDamage;
    public long m_lastAttackerId;
    public MyStringHash m_lastDamageType = MyDamageType.Unknown;
    private Action<MySlimBlock> m_isFunctionalChanged;
    public MyMultiBlockDefinition MultiBlockDefinition;
    public int MultiBlockId;
    public int MultiBlockIndex = -1;
    private static readonly Dictionary<string, int> m_modelTotalFracturesCount = new Dictionary<string, int>();
    public List<Vector3I> DisconnectFaces = new List<Vector3I>();
    [ThreadStatic]
    private static List<uint> m_tmpIds;
    [ThreadStatic]
    private static List<MyTuple<Vector3I, float>> m_batchCache;

    private static Dictionary<string, int> m_tmpComponents => MyUtils.Init<Dictionary<string, int>>(ref MySlimBlock.m_tmpComponentsPerThread);

    private static List<MyStockpileItem> m_tmpItemList => MyUtils.Init<List<MyStockpileItem>>(ref MySlimBlock.m_tmpItemListPerThread);

    private static List<Vector3I> m_tmpCubeNeighbours => MyUtils.Init<List<Vector3I>>(ref MySlimBlock.m_tmpCubeNeighboursPerThread);

    private static List<MySlimBlock> m_tmpBlocks => MyUtils.Init<List<MySlimBlock>>(ref MySlimBlock.m_tmpBlocksPerThread);

    private static List<MySlimBlock> m_tmpMultiBlocks => MyUtils.Init<List<MySlimBlock>>(ref MySlimBlock.m_tmpMultiBlocksPerThread);

    public static event Action<MyTerminalBlock, long> OnAnyBlockHackedChanged;

    public float AccumulatedDamage
    {
      get => this.m_accumulatedDamage;
      private set
      {
        this.m_accumulatedDamage = value;
        if ((double) this.m_accumulatedDamage <= 0.0)
          return;
        this.CubeGrid.AddForDamageApplication(this);
      }
    }

    public MyCubeBlock FatBlock { get; private set; }

    public Vector3D WorldPosition => this.CubeGrid.GridIntegerToWorld(this.Position);

    public BoundingBoxD WorldAABB => new BoundingBoxD((Vector3D) (this.Min * this.CubeGrid.GridSize - this.CubeGrid.GridSizeHalfVector), (Vector3D) (this.Max * this.CubeGrid.GridSize + this.CubeGrid.GridSizeHalfVector)).TransformFast(this.CubeGrid.PositionComp.WorldMatrixRef);

    public MyCubeGrid CubeGrid
    {
      get => this.m_cubeGrid;
      set
      {
        if (this.m_cubeGrid == value)
          return;
        bool flag = this.m_cubeGrid == null;
        MyCubeGrid cubeGrid = this.m_cubeGrid;
        this.m_cubeGrid = value;
        if (this.FatBlock == null || flag)
          return;
        this.FatBlock.OnCubeGridChanged(cubeGrid);
        if (this.CubeGridChanged == null)
          return;
        this.CubeGridChanged(this, cubeGrid);
      }
    }

    public Vector3 ColorMaskHSV
    {
      get => this.m_colorMaskHSV;
      set => this.m_colorMaskHSV = value;
    }

    public float DeformationRatio
    {
      get => this.m_deformationRatio * this.BlockGeneralDamageModifier * Math.Min((float) this.CubeGrid.GridGeneralDamageModifier, (float) MyGridPhysicalHierarchy.Static.GetRoot(this.CubeGrid).GridGeneralDamageModifier) * this.BlockDefinition.GeneralDamageMultiplier;
      set => this.m_deformationRatio = value;
    }

    public bool ShowParts { get; private set; }

    public bool IsFullIntegrity => this.m_componentStack == null || this.m_componentStack.IsFullIntegrity;

    public float BuildLevelRatio => this.m_componentStack.BuildRatio;

    public float BuildIntegrity => this.m_componentStack.BuildIntegrity;

    public bool IsFullyDismounted => this.m_componentStack.IsFullyDismounted;

    public bool IsDestroyed => this.m_componentStack.IsDestroyed;

    public bool UseDamageSystem { get; private set; }

    public float Integrity => this.m_componentStack.Integrity;

    public float MaxIntegrity => this.m_componentStack.MaxIntegrity;

    public float CurrentDamage => this.BuildIntegrity - this.Integrity;

    public float DamageRatio => (float) (2.0 - (double) this.m_componentStack.BuildIntegrity / (double) this.MaxIntegrity);

    public bool StockpileAllocated => this.m_stockpile != null;

    public bool StockpileEmpty => !this.StockpileAllocated || this.m_stockpile.IsEmpty();

    public bool HasDeformation => this.CubeGrid != null && this.CubeGrid.Skeleton.IsDeformed(this.Position, 0.0f, this.CubeGrid, true);

    public float MaxDeformation => this.m_cachedMaxDeformation;

    public int GetStockpileStamp() => this.m_stockpile == null ? 0 : this.m_stockpile.LastChangeStamp;

    public MyComponentStack ComponentStack => this.m_componentStack;

    public bool YieldLastComponent => this.m_componentStack.YieldLastComponent;

    public long BuiltBy => this.m_builtByID;

    public event Action<MySlimBlock, MyCubeGrid> CubeGridChanged;

    public void SubscribeForIsFunctionalChanged(Action<MySlimBlock> callback)
    {
      if (this.m_isFunctionalChanged == null && callback != null)
        this.ComponentStack.IsFunctionalChanged += new Action(this.IsFunctionalChanged);
      this.m_isFunctionalChanged += callback;
    }

    private void IsFunctionalChanged()
    {
      if (this.m_isFunctionalChanged == null)
        return;
      this.m_isFunctionalChanged(this);
    }

    public void UnsubscribeFromIsFunctionalChanged(Action<MySlimBlock> callback)
    {
      if (this.m_isFunctionalChanged == null)
        return;
      this.m_isFunctionalChanged -= callback;
      if (this.m_isFunctionalChanged != null)
        return;
      this.ComponentStack.IsFunctionalChanged -= new Action(this.IsFunctionalChanged);
    }

    public int UniqueId { get; private set; }

    public bool IsMultiBlockPart => MyFakes.ENABLE_MULTIBLOCK_PART_IDS && this.MultiBlockId != 0 && this.MultiBlockDefinition != null && this.MultiBlockIndex != -1;

    public bool ForceBlockDestructible => this.FatBlock != null && this.FatBlock.ForceBlockDestructible;

    public long OwnerId
    {
      get
      {
        if (this.FatBlock != null && this.FatBlock.OwnerId != 0L)
          return this.FatBlock.OwnerId;
        MyGridOwnershipComponentBase component;
        this.CubeGrid.Components.TryGet<MyGridOwnershipComponentBase>(out component);
        return component != null ? component.GetBlockOwnerId(this) : 0L;
      }
    }

    public MySlimBlock()
    {
      this.UniqueId = MyRandom.Instance.Next();
      this.UseDamageSystem = true;
    }

    public void DisableLastComponentYield() => this.m_componentStack.DisableLastComponentYield();

    public bool Init(
      MyObjectBuilder_CubeBlock objectBuilder,
      MyCubeGrid cubeGrid,
      MyCubeBlock fatBlock)
    {
      this.FatBlock = fatBlock;
      if (objectBuilder is MyObjectBuilder_CompoundCubeBlock)
        this.BlockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
      else if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(objectBuilder.GetId(), out this.BlockDefinition))
        return false;
      if (this.BlockDefinition == null || this.BlockDefinition.CubeSize != cubeGrid.GridSizeEnum && !MySession.Static.Settings.EnableSupergridding)
        return false;
      this.m_componentStack = new MyComponentStack(this.BlockDefinition, objectBuilder.IntegrityPercent, objectBuilder.BuildPercent);
      this.m_componentStack.IsFunctionalChanged += new Action(this.m_componentStack_IsFunctionalChanged);
      if (MyCubeGridDefinitions.GetCubeRotationOptions(this.BlockDefinition) == MyRotationOptionsEnum.None)
        objectBuilder.BlockOrientation = (SerializableBlockOrientation) MyBlockOrientation.Identity;
      this.UsesDeformation = this.BlockDefinition.UsesDeformation;
      this.DeformationRatio = this.BlockDefinition.DeformationRatio;
      this.Min = (Vector3I) objectBuilder.Min;
      this.Orientation = (MyBlockOrientation) objectBuilder.BlockOrientation;
      if (!this.Orientation.IsValid)
        this.Orientation = MyBlockOrientation.Identity;
      this.CubeGrid = cubeGrid;
      this.ColorMaskHSV = (Vector3) objectBuilder.ColorMaskHSV;
      this.SkinSubtypeId = MyStringHash.GetOrCompute(objectBuilder.SkinSubtypeId);
      if (this.BlockDefinition.CubeDefinition != null)
        this.Orientation = MyCubeGridDefinitions.GetTopologyUniqueOrientation(this.BlockDefinition.CubeDefinition.CubeTopology, this.Orientation);
      MySlimBlock.ComputeMax(this.BlockDefinition, this.Orientation, ref this.Min, out this.Max);
      this.Position = MySlimBlock.ComputePositionInGrid(new MatrixI(this.Orientation), this.BlockDefinition, this.Min);
      if (objectBuilder.MultiBlockId != 0 && objectBuilder.MultiBlockDefinition.HasValue && objectBuilder.MultiBlockIndex != -1)
      {
        this.MultiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition((MyDefinitionId) objectBuilder.MultiBlockDefinition.Value);
        if (this.MultiBlockDefinition != null)
        {
          this.MultiBlockId = objectBuilder.MultiBlockId;
          this.MultiBlockIndex = objectBuilder.MultiBlockIndex;
        }
      }
      this.UpdateShowParts(false);
      if (this.FatBlock == null && ((!string.IsNullOrEmpty(this.BlockDefinition.Model) ? 1 : 0) | (this.BlockDefinition.BlockTopology != MyBlockTopology.Cube ? (false ? 1 : 0) : (!this.ShowParts ? 1 : 0))) != 0)
        this.FatBlock = new MyCubeBlock();
      if (this.FatBlock != null)
      {
        this.FatBlock.SlimBlock = this;
        this.FatBlock.Init(objectBuilder, cubeGrid);
      }
      if (objectBuilder.ConstructionStockpile != null)
      {
        this.EnsureConstructionStockpileExists();
        this.m_stockpile.Init(objectBuilder.ConstructionStockpile);
      }
      else if (objectBuilder.ConstructionInventory != null)
      {
        this.EnsureConstructionStockpileExists();
        this.m_stockpile.Init(objectBuilder.ConstructionInventory);
      }
      if (MyFakes.SHOW_DAMAGE_EFFECTS && this.CubeGrid.CreatePhysics && (this.FatBlock != null && !this.BlockDefinition.RatioEnoughForDamageEffect(this.BuildIntegrity / this.MaxIntegrity)) && (this.BlockDefinition.RatioEnoughForDamageEffect(this.Integrity / this.MaxIntegrity) && (double) this.CurrentDamage > 0.00999999977648258))
        this.FatBlock.SetDamageEffectDelayed(true);
      this.UpdateMaxDeformation();
      this.m_builtByID = objectBuilder.BuiltBy;
      this.BlockGeneralDamageModifier = objectBuilder.BlockGeneralDamageModifier;
      return true;
    }

    private void m_componentStack_IsFunctionalChanged()
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.m_builtByID);
      int pcu = this.BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      if (identity == null)
        return;
      if (this.m_componentStack.IsFunctional)
      {
        identity.BlockLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid, false);
        MySession.Static.SessionBlockLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid, false);
        this.CubeGrid.BlocksPCU += pcu;
      }
      else
      {
        identity.BlockLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid, false);
        MySession.Static.SessionBlockLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid, false);
        this.CubeGrid.BlocksPCU -= pcu;
      }
    }

    public void ResumeDamageEffect()
    {
      if (this.FatBlock == null)
        return;
      if (MyFakes.SHOW_DAMAGE_EFFECTS && !this.BlockDefinition.RatioEnoughForDamageEffect(this.BuildIntegrity / this.MaxIntegrity) && this.BlockDefinition.RatioEnoughForDamageEffect(this.Integrity / this.MaxIntegrity))
      {
        if ((double) this.CurrentDamage <= 0.0)
          return;
        this.FatBlock.SetDamageEffect(true);
      }
      else
        this.FatBlock.SetDamageEffect(false);
    }

    public void InitOrientation(Base6Directions.Direction Forward, Base6Directions.Direction Up)
    {
      this.Orientation = MyCubeGridDefinitions.GetCubeRotationOptions(this.BlockDefinition) != MyRotationOptionsEnum.None ? new MyBlockOrientation(Forward, Up) : MyBlockOrientation.Identity;
      if (this.BlockDefinition.CubeDefinition == null)
        return;
      this.Orientation = MyCubeGridDefinitions.GetTopologyUniqueOrientation(this.BlockDefinition.CubeDefinition.CubeTopology, this.Orientation);
    }

    public void InitOrientation(MyBlockOrientation orientation)
    {
      if (!orientation.IsValid)
        this.Orientation = MyBlockOrientation.Identity;
      this.InitOrientation(orientation.Forward, orientation.Up);
    }

    public void InitOrientation(ref Vector3I forward, ref Vector3I up) => this.InitOrientation(Base6Directions.GetDirection(forward), Base6Directions.GetDirection(up));

    public MyObjectBuilder_CubeBlock GetObjectBuilder(bool copy = false) => this.GetObjectBuilderInternal(copy);

    public MyObjectBuilder_CubeBlock GetCopyObjectBuilder() => this.GetObjectBuilderInternal(true);

    private MyObjectBuilder_CubeBlock GetObjectBuilderInternal(bool copy)
    {
      MyObjectBuilder_CubeBlock builderCubeBlock = this.FatBlock == null ? (MyObjectBuilder_CubeBlock) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) this.BlockDefinition.Id) : this.FatBlock.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.SubtypeName = this.BlockDefinition.Id.SubtypeName;
      builderCubeBlock.Min = (SerializableVector3I) this.Min;
      builderCubeBlock.BlockOrientation = (SerializableBlockOrientation) this.Orientation;
      builderCubeBlock.IntegrityPercent = this.m_componentStack.Integrity / this.m_componentStack.MaxIntegrity;
      builderCubeBlock.BuildPercent = this.m_componentStack.BuildRatio;
      builderCubeBlock.ColorMaskHSV = (SerializableVector3) this.ColorMaskHSV;
      builderCubeBlock.SkinSubtypeId = this.SkinSubtypeId.String;
      builderCubeBlock.BuiltBy = this.m_builtByID;
      builderCubeBlock.ConstructionStockpile = this.m_stockpile == null || this.m_stockpile.GetItems().Count == 0 ? (MyObjectBuilder_ConstructionStockpile) null : this.m_stockpile.GetObjectBuilder();
      if (this.IsMultiBlockPart)
      {
        builderCubeBlock.MultiBlockDefinition = new SerializableDefinitionId?((SerializableDefinitionId) this.MultiBlockDefinition.Id);
        builderCubeBlock.MultiBlockId = this.MultiBlockId;
        builderCubeBlock.MultiBlockIndex = this.MultiBlockIndex;
      }
      builderCubeBlock.BlockGeneralDamageModifier = this.BlockGeneralDamageModifier;
      return builderCubeBlock;
    }

    public void AddNeighbours()
    {
      this.AddNeighbours(this.Min, new Vector3I(this.Min.X, this.Max.Y, this.Max.Z), -Vector3I.UnitX);
      this.AddNeighbours(this.Min, new Vector3I(this.Max.X, this.Min.Y, this.Max.Z), -Vector3I.UnitY);
      this.AddNeighbours(this.Min, new Vector3I(this.Max.X, this.Max.Y, this.Min.Z), -Vector3I.UnitZ);
      this.AddNeighbours(new Vector3I(this.Max.X, this.Min.Y, this.Min.Z), this.Max, Vector3I.UnitX);
      this.AddNeighbours(new Vector3I(this.Min.X, this.Max.Y, this.Min.Z), this.Max, Vector3I.UnitY);
      this.AddNeighbours(new Vector3I(this.Min.X, this.Min.Y, this.Max.Z), this.Max, Vector3I.UnitZ);
      if (this.FatBlock == null)
        return;
      this.FatBlock.OnAddedNeighbours();
    }

    private void AddNeighbours(Vector3I min, Vector3I max, Vector3I normalDirection)
    {
      Vector3I pos;
      for (pos.X = min.X; pos.X <= max.X; ++pos.X)
      {
        for (pos.Y = min.Y; pos.Y <= max.Y; ++pos.Y)
        {
          for (pos.Z = min.Z; pos.Z <= max.Z; ++pos.Z)
            this.AddNeighbour(pos, normalDirection);
        }
      }
    }

    private void AddNeighbour(Vector3I pos, Vector3I dir)
    {
      MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(pos + dir);
      if (cubeBlock == null || cubeBlock == this)
        return;
      if (MyFakes.ENABLE_COMPOUND_BLOCKS)
      {
        if (this.Neighbours.Contains(cubeBlock))
          return;
        MyCompoundCubeBlock fatBlock1 = this.FatBlock as MyCompoundCubeBlock;
        MyCompoundCubeBlock fatBlock2 = cubeBlock.FatBlock as MyCompoundCubeBlock;
        if (fatBlock1 != null)
        {
          foreach (MySlimBlock block1 in fatBlock1.GetBlocks())
          {
            MyCubeBlockDefinition.MountPoint[] modelMountPoints1 = block1.BlockDefinition.GetBuildProgressModelMountPoints(block1.BuildLevelRatio);
            if (fatBlock2 != null)
            {
              foreach (MySlimBlock block2 in fatBlock2.GetBlocks())
              {
                MyCubeBlockDefinition.MountPoint[] modelMountPoints2 = block2.BlockDefinition.GetBuildProgressModelMountPoints(block2.BuildLevelRatio);
                if (MySlimBlock.AddNeighbour(ref dir, block1, modelMountPoints1, block2, modelMountPoints2, this, cubeBlock))
                  return;
              }
            }
            else
            {
              MyCubeBlockDefinition.MountPoint[] modelMountPoints2 = cubeBlock.BlockDefinition.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
              if (MySlimBlock.AddNeighbour(ref dir, block1, modelMountPoints1, cubeBlock, modelMountPoints2, this, cubeBlock))
                break;
            }
          }
        }
        else
        {
          MyCubeBlockDefinition.MountPoint[] modelMountPoints1 = this.BlockDefinition.GetBuildProgressModelMountPoints(this.BuildLevelRatio);
          if (fatBlock2 != null)
          {
            foreach (MySlimBlock block in fatBlock2.GetBlocks())
            {
              MyCubeBlockDefinition.MountPoint[] modelMountPoints2 = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
              if (MySlimBlock.AddNeighbour(ref dir, this, modelMountPoints1, block, modelMountPoints2, this, cubeBlock))
                break;
            }
          }
          else
          {
            MyCubeBlockDefinition.MountPoint[] modelMountPoints2 = cubeBlock.BlockDefinition.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
            MySlimBlock.AddNeighbour(ref dir, this, modelMountPoints1, cubeBlock, modelMountPoints2, this, cubeBlock);
          }
        }
      }
      else
      {
        MyCubeBlockDefinition.MountPoint[] modelMountPoints1 = this.BlockDefinition.GetBuildProgressModelMountPoints(this.BuildLevelRatio);
        MyCubeBlockDefinition.MountPoint[] modelMountPoints2 = cubeBlock.BlockDefinition.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
        if (!MyCubeGrid.CheckMountPointsForSide(this.BlockDefinition, modelMountPoints1, ref this.Orientation, ref this.Position, ref dir, cubeBlock.BlockDefinition, modelMountPoints2, ref cubeBlock.Orientation, ref cubeBlock.Position) || !this.ConnectionAllowed(ref pos, ref dir, cubeBlock) || this.Neighbours.Contains(cubeBlock))
          return;
        this.Neighbours.Add(cubeBlock);
        cubeBlock.Neighbours.Add(this);
      }
    }

    private static bool AddNeighbour(
      ref Vector3I dir,
      MySlimBlock thisBlock,
      MyCubeBlockDefinition.MountPoint[] thisMountPoints,
      MySlimBlock otherBlock,
      MyCubeBlockDefinition.MountPoint[] otherMountPoints,
      MySlimBlock thisParentBlock,
      MySlimBlock otherParentBlock)
    {
      if (!MyCubeGrid.CheckMountPointsForSide(thisBlock.BlockDefinition, thisMountPoints, ref thisBlock.Orientation, ref thisBlock.Position, ref dir, otherBlock.BlockDefinition, otherMountPoints, ref otherBlock.Orientation, ref otherBlock.Position) || !thisBlock.ConnectionAllowed(ref otherBlock.Position, ref dir, otherBlock))
        return false;
      thisParentBlock.Neighbours.Add(otherParentBlock);
      otherParentBlock.Neighbours.Add(thisParentBlock);
      return true;
    }

    private bool ConnectionAllowed(
      ref Vector3I otherBlockPos,
      ref Vector3I faceNormal,
      MySlimBlock other)
    {
      if (this.DisconnectFaces.Count > 0 && this.DisconnectFaces.Contains(faceNormal))
        return false;
      return this.FatBlock == null || !this.FatBlock.CheckConnectionAllowed || this.FatBlock.ConnectionAllowed(ref otherBlockPos, ref faceNormal, other.BlockDefinition);
    }

    public void RemoveNeighbours()
    {
      bool flag = true;
      foreach (MySlimBlock neighbour in this.Neighbours)
        flag &= neighbour.Neighbours.Remove(this);
      this.Neighbours.Clear();
      if (this.FatBlock == null)
        return;
      this.FatBlock.OnRemovedNeighbours();
    }

    private void UpdateShowParts(bool fixSkeleton)
    {
      if (this.BlockDefinition.BlockTopology != MyBlockTopology.Cube)
      {
        this.ShowParts = false;
      }
      else
      {
        float buildLevelRatio = this.BuildLevelRatio;
        if (this.BlockDefinition.BuildProgressModels != null && this.BlockDefinition.BuildProgressModels.Length != 0)
        {
          MyCubeBlockDefinition.BuildProgressModel buildProgressModel = this.BlockDefinition.BuildProgressModels[this.BlockDefinition.BuildProgressModels.Length - 1];
          this.ShowParts = (double) buildLevelRatio >= (double) buildProgressModel.BuildRatioUpperBound;
        }
        else
          this.ShowParts = true;
        if (!fixSkeleton || this.ShowParts)
          return;
        this.CubeGrid.FixSkeleton(this, true);
      }
    }

    public void UpdateMaxDeformation() => this.m_cachedMaxDeformation = this.CubeGrid.Skeleton.MaxDeformation(this.Position, this.CubeGrid);

    public int CalculateCurrentModelID()
    {
      float buildLevelRatio = this.BuildLevelRatio;
      if ((double) buildLevelRatio < 1.0 && this.BlockDefinition.BuildProgressModels != null && this.BlockDefinition.BuildProgressModels.Length != 0)
      {
        for (int index = 0; index < this.BlockDefinition.BuildProgressModels.Length; ++index)
        {
          if ((double) this.BlockDefinition.BuildProgressModels[index].BuildRatioUpperBound >= (double) buildLevelRatio)
            return index;
        }
      }
      return -1;
    }

    public string CalculateCurrentModel(out Matrix orientation)
    {
      float buildLevelRatio = this.BuildLevelRatio;
      this.Orientation.GetMatrix(out orientation);
      if ((double) buildLevelRatio < 1.0 && this.BlockDefinition.BuildProgressModels != null && this.BlockDefinition.BuildProgressModels.Length != 0)
      {
        for (int index = 0; index < this.BlockDefinition.BuildProgressModels.Length; ++index)
        {
          if ((double) this.BlockDefinition.BuildProgressModels[index].BuildRatioUpperBound >= (double) buildLevelRatio)
          {
            if (this.BlockDefinition.BuildProgressModels[index].RandomOrientation)
              orientation = MyCubeGridDefinitions.AllPossible90rotations[Math.Abs(this.Position.GetHashCode()) % MyCubeGridDefinitions.AllPossible90rotations.Length].GetFloatMatrix();
            return this.BlockDefinition.BuildProgressModels[index].File;
          }
        }
      }
      return this.FatBlock == null ? this.BlockDefinition.Model : this.FatBlock.CalculateCurrentModel(out orientation);
    }

    public static Vector3I ComputePositionInGrid(
      MatrixI localMatrix,
      MyCubeBlockDefinition blockDefinition,
      Vector3I min)
    {
      Vector3I center = blockDefinition.Center;
      Vector3I normal = blockDefinition.Size - 1;
      Vector3I result1;
      Vector3I.TransformNormal(ref normal, ref localMatrix, out result1);
      Vector3I result2;
      Vector3I.TransformNormal(ref center, ref localMatrix, out result2);
      Vector3I vector3I1 = Vector3I.Abs(result1);
      Vector3I vector3I2 = result2 + min;
      if (result1.X != vector3I1.X)
        vector3I2.X += vector3I1.X;
      if (result1.Y != vector3I1.Y)
        vector3I2.Y += vector3I1.Y;
      if (result1.Z != vector3I1.Z)
        vector3I2.Z += vector3I1.Z;
      return vector3I2;
    }

    public void SpawnFirstItemInConstructionStockpile()
    {
      if (MySession.Static.CreativeMode)
        return;
      this.EnsureConstructionStockpileExists();
      MyComponentStack.GroupInfo groupInfo = this.ComponentStack.GetGroupInfo(0);
      this.m_stockpile.ClearSyncList();
      this.m_stockpile.AddItems(1, groupInfo.Component.Id);
      this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
      this.m_stockpile.ClearSyncList();
    }

    public void MoveItemsToConstructionStockpile(MyInventoryBase fromInventory)
    {
      if (MySession.Static.CreativeMode)
        return;
      MySlimBlock.m_tmpComponents.Clear();
      this.GetMissingComponents(MySlimBlock.m_tmpComponents);
      if (MySlimBlock.m_tmpComponents.Count == 0)
        return;
      this.EnsureConstructionStockpileExists();
      this.m_stockpile.ClearSyncList();
      foreach (KeyValuePair<string, int> mTmpComponent in MySlimBlock.m_tmpComponents)
      {
        MyDefinitionId myDefinitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), mTmpComponent.Key);
        int itemAmountCombined = (int) MyCubeBuilder.BuildComponent.GetItemAmountCombined(fromInventory, myDefinitionId);
        int num = Math.Min(mTmpComponent.Value, itemAmountCombined);
        if (num > 0)
        {
          MyCubeBuilder.BuildComponent.RemoveItemsCombined(fromInventory, num, myDefinitionId);
          this.m_stockpile.AddItems(num, new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), mTmpComponent.Key));
        }
      }
      this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
      this.m_stockpile.ClearSyncList();
    }

    public bool MoveItemsFromConstructionStockpile(MyInventoryBase toInventory, MyItemFlags flags = MyItemFlags.None)
    {
      bool flag = false;
      if (this.m_stockpile == null || toInventory == null)
        return flag;
      MySlimBlock.m_tmpItemList.Clear();
      foreach (MyStockpileItem myStockpileItem in this.m_stockpile.GetItems())
      {
        if (flags == MyItemFlags.None || (myStockpileItem.Content.Flags & flags) != MyItemFlags.None)
          MySlimBlock.m_tmpItemList.Add(myStockpileItem);
      }
      this.m_stockpile.ClearSyncList();
      foreach (MyStockpileItem mTmpItem in MySlimBlock.m_tmpItemList)
      {
        int count = Math.Min(toInventory.ComputeAmountThatFits(mTmpItem.Content.GetId()).ToIntSafe(), mTmpItem.Amount);
        toInventory.AddItems((MyFixedPoint) count, (MyObjectBuilder_Base) mTmpItem.Content);
        this.m_stockpile.RemoveItems(count, mTmpItem.Content);
        if (count <= 0)
          flag = true;
      }
      this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
      this.m_stockpile.ClearSyncList();
      return flag;
    }

    public void MoveUnneededItemsFromConstructionStockpile(MyInventoryBase toInventory)
    {
      if (this.m_stockpile == null || toInventory == null)
        return;
      MySlimBlock.m_tmpItemList.Clear();
      this.AcquireUnneededStockpileItems(MySlimBlock.m_tmpItemList);
      this.m_stockpile.ClearSyncList();
      foreach (MyStockpileItem mTmpItem in MySlimBlock.m_tmpItemList)
      {
        int count = Math.Min(toInventory.ComputeAmountThatFits(mTmpItem.Content.GetId()).ToIntSafe(), mTmpItem.Amount);
        toInventory.AddItems((MyFixedPoint) count, (MyObjectBuilder_Base) mTmpItem.Content);
        this.m_stockpile.RemoveItems(count, mTmpItem.Content);
      }
      this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
      this.m_stockpile.ClearSyncList();
    }

    public void ClearConstructionStockpile(MyInventoryBase outputInventory)
    {
      if (!this.StockpileEmpty)
      {
        MyEntity entity = (MyEntity) null;
        if (outputInventory != null && outputInventory.Container != null)
          entity = outputInventory.Container.Entity as MyEntity;
        if (entity != null && entity.InventoryOwnerType() == MyInventoryOwnerTypeEnum.Character)
        {
          this.MoveItemsFromConstructionStockpile(outputInventory);
        }
        else
        {
          this.m_stockpile.ClearSyncList();
          MySlimBlock.m_tmpItemList.Clear();
          foreach (MyStockpileItem myStockpileItem in this.m_stockpile.GetItems())
            MySlimBlock.m_tmpItemList.Add(myStockpileItem);
          foreach (MyStockpileItem mTmpItem in MySlimBlock.m_tmpItemList)
            this.RemoveFromConstructionStockpile(mTmpItem);
          this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
          this.m_stockpile.ClearSyncList();
        }
      }
      this.ReleaseConstructionStockpile();
    }

    private void RemoveFromConstructionStockpile(MyStockpileItem item) => this.m_stockpile.RemoveItems(item.Amount, item.Content.GetId(), item.Content.Flags);

    private void AcquireUnneededStockpileItems(List<MyStockpileItem> outputList)
    {
      if (this.m_stockpile == null)
        return;
      foreach (MyStockpileItem myStockpileItem in this.m_stockpile.GetItems())
      {
        bool flag = false;
        foreach (MyCubeBlockDefinition.Component component in this.BlockDefinition.Components)
        {
          if (component.Definition.Id.SubtypeId == myStockpileItem.Content.SubtypeId)
            flag = true;
        }
        if (!flag)
          outputList.Add(myStockpileItem);
      }
    }

    private void ReleaseUnneededStockpileItems()
    {
      if (this.m_stockpile == null || !Sync.IsServer)
        return;
      MySlimBlock.m_tmpItemList.Clear();
      this.AcquireUnneededStockpileItems(MySlimBlock.m_tmpItemList);
      this.m_stockpile.ClearSyncList();
      BoundingBoxD box = new BoundingBoxD(this.CubeGrid.GridIntegerToWorld(this.Min), this.CubeGrid.GridIntegerToWorld(this.Max));
      foreach (MyStockpileItem mTmpItem in MySlimBlock.m_tmpItemList)
      {
        if ((double) mTmpItem.Amount >= 0.00999999977648258)
        {
          MyEntity myEntity = MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) mTmpItem.Amount, mTmpItem.Content), box, (MyPhysicsComponentBase) this.CubeGrid.Physics);
          myEntity?.Physics.ApplyImpulse(MyUtils.GetRandomVector3Normalized() * myEntity.Physics.Mass / 5f, myEntity.PositionComp.GetPosition());
          this.m_stockpile.RemoveItems(mTmpItem.Amount, mTmpItem.Content);
        }
      }
      this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
      this.m_stockpile.ClearSyncList();
    }

    public int GetConstructionStockpileItemAmount(MyDefinitionId id) => this.m_stockpile == null ? 0 : this.m_stockpile.GetItemAmount(id);

    public void SetToConstructionSite() => this.m_componentStack.DestroyCompletely();

    public void GetMissingComponents(Dictionary<string, int> addToDictionary) => this.m_componentStack.GetMissingComponents(addToDictionary, this.m_stockpile);

    private void ReleaseConstructionStockpile()
    {
      if (this.m_stockpile == null)
        return;
      if (MyFakes.ENABLE_GENERATED_BLOCKS)
      {
        int num = this.BlockDefinition.IsGeneratedBlock ? 1 : 0;
      }
      this.m_stockpile = (MyConstructionStockpile) null;
    }

    private void EnsureConstructionStockpileExists()
    {
      if (this.m_stockpile != null)
        return;
      this.m_stockpile = new MyConstructionStockpile();
    }

    public void SpawnConstructionStockpile()
    {
      if (this.m_stockpile == null)
        return;
      MatrixD worldMatrix = this.CubeGrid.WorldMatrix;
      int num1 = this.Max.RectangularDistance(this.Min) + 3;
      Vector3D min = (Vector3D) this.Min;
      Vector3D max = (Vector3D) this.Max;
      Vector3D position1 = min * (double) this.CubeGrid.GridSize;
      Vector3D position2 = max * (double) this.CubeGrid.GridSize;
      Vector3D position3 = Vector3D.Transform(position1, worldMatrix);
      Vector3D vector3D1 = Vector3D.Transform(position2, worldMatrix);
      Vector3D vector3D2 = (position3 + vector3D1) / 2.0;
      Vector3 totalGravityInPoint = MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D2);
      if ((double) totalGravityInPoint.Length() != 0.0)
      {
        double num2 = (double) totalGravityInPoint.Normalize();
        Vector3I? nullable = this.CubeGrid.RayCastBlocks(vector3D2, vector3D2 + totalGravityInPoint * (float) num1 * this.CubeGrid.GridSize);
        position3 = nullable.HasValue ? Vector3D.Transform((Vector3D) nullable.Value * (double) this.CubeGrid.GridSize, worldMatrix) - totalGravityInPoint * this.CubeGrid.GridSize * 0.1f : vector3D2;
      }
      foreach (MyStockpileItem myStockpileItem in this.m_stockpile.GetItems())
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) myStockpileItem.Amount, myStockpileItem.Content), position3, worldMatrix.Forward, worldMatrix.Up, (MyPhysicsComponentBase) this.CubeGrid.Physics);
    }

    public bool CanContinueBuild(MyInventoryBase sourceInventory) => !this.IsFullIntegrity && (sourceInventory != null || MySession.Static.CreativeMode) && (this.FatBlock == null || this.FatBlock.CanContinueBuild()) && this.m_componentStack.CanContinueBuild(sourceInventory, this.m_stockpile);

    public void FixBones(float oldDamage, float maxAllowedBoneMovement)
    {
      float factor = this.CurrentDamage / oldDamage;
      if ((double) oldDamage == 0.0)
        factor = 0.0f;
      float num = (1f - factor) * this.MaxDeformation;
      if ((double) this.MaxDeformation != 0.0 && (double) num > (double) maxAllowedBoneMovement)
        factor = (float) (1.0 - (double) maxAllowedBoneMovement / (double) this.MaxDeformation);
      if ((double) factor == 0.0)
        this.CubeGrid.ResetBlockSkeleton(this, true);
      if ((double) factor <= 0.0)
        return;
      this.CubeGrid.MultiplyBlockSkeleton(this, factor, true);
    }

    public bool DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      if ((double) damage <= 0.0)
        return false;
      if (sync)
      {
        if (Sync.IsServer && (this.FatBlock != null ? (this.FatBlock.ReceivedDamage(damage, damageType, attackerId, realHitEntityId) ? 1 : 0) : 1) != 0)
        {
          MySlimBlock.DoDamageSynced(this, damage, damageType, hitInfo, attackerId);
          long attackerIdentityId = this.GetAttackerIdentityId(attackerId);
          long attackedIdentityId = this.CubeGrid.BigOwners.Count > 0 ? this.CubeGrid.BigOwners[0] : 0L;
          if (attackerIdentityId != 0L && attackedIdentityId != 0L && attackerIdentityId != attackedIdentityId)
            MySession.Static.Factions.DamageFactionPlayerReputation(attackerIdentityId, attackedIdentityId, MyReputationDamageType.Damaging);
          if (MyVisualScriptLogicProvider.BlockDamaged != null)
            MyVisualScriptLogicProvider.BlockDamaged(this.FatBlock != null ? this.FatBlock.Name : string.Empty, this.CubeGrid.Name, this.BlockDefinition.Id.TypeId.ToString(), this.BlockDefinition.Id.SubtypeName, damage, damageType.String, attackerId);
        }
      }
      else if ((this.FatBlock != null ? (this.FatBlock.ReceivedDamage(damage, damageType, attackerId, realHitEntityId) ? 1 : 0) : 1) != 0)
        this.DoDamage(damage, damageType, hitInfo, attackerId: attackerId);
      return true;
    }

    private long GetAttackerIdentityId(long attackerId)
    {
      MyEntity entity = (MyEntity) null;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(attackerId, out entity);
      switch (entity)
      {
        case Sandbox.Game.Entities.IMyControllableEntity controllableEntity:
          MyControllerInfo controllerInfo = controllableEntity.ControllerInfo;
          if (controllerInfo != null)
            return controllerInfo.ControllingIdentityId;
          break;
        case IMyGunBaseUser myGunBaseUser:
          return myGunBaseUser.OwnerId;
        case IMyHandheldGunObject<MyDeviceBase> handheldGunObject:
          return handheldGunObject.OwnerIdentityId;
      }
      return 0;
    }

    public void DoDamage(
      float damage,
      MyStringHash damageType,
      MyHitInfo? hitInfo = null,
      bool addDirtyParts = true,
      long attackerId = 0)
    {
      if (!this.CubeGrid.BlocksDestructionEnabled && !this.ForceBlockDestructible)
        return;
      float num = Math.Min((float) this.CubeGrid.GridGeneralDamageModifier, (float) MyGridPhysicalHierarchy.Static.GetRoot(this.CubeGrid).GridGeneralDamageModifier);
      damage = damage * this.BlockGeneralDamageModifier * num * this.BlockDefinition.GeneralDamageMultiplier;
      this.DoDamageInternal(damage, damageType, addDirtyParts, hitInfo, attackerId);
    }

    private void DoDamageInternal(
      float damage,
      MyStringHash damageType,
      bool addDirtyParts = true,
      MyHitInfo? hitInfo = null,
      long attackerId = 0)
    {
      damage *= this.DamageRatio;
      ulong steamId = MySession.Static.Players.TryGetSteamId(attackerId);
      if (steamId == 0UL)
      {
        MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(attackerId);
        if (entityById is MyAutomaticRifleGun automaticRifleGun && automaticRifleGun.Owner != null)
        {
          MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer((MyEntity) automaticRifleGun.Owner);
          if (controllingPlayer != null)
            steamId = controllingPlayer.Id.SteamId;
        }
        if (entityById is MyHandDrill myHandDrill && myHandDrill.Owner != null)
        {
          MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer((MyEntity) myHandDrill.Owner);
          if (controllingPlayer != null)
            steamId = controllingPlayer.Id.SteamId;
        }
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Damage, user: steamId))
        return;
      if (MyPerGameSettings.Destruction || MyFakes.ENABLE_VR_BLOCK_DEFORMATION_RATIO)
        damage *= this.DeformationRatio;
      try
      {
        if (this.FatBlock != null)
        {
          if (!this.FatBlock.Closed)
          {
            if (this.CubeGrid.Physics != null)
            {
              if (this.CubeGrid.Physics.Enabled)
              {
                if (this.FatBlock is IMyDestroyableObject fatBlock)
                {
                  double num = (double) damage;
                  MyStringHash damageSource = damageType;
                  MyHitInfo? hitInfo1 = new MyHitInfo?();
                  long attackerId1 = attackerId;
                  if (fatBlock.DoDamage((float) num, damageSource, false, hitInfo1, attackerId1))
                    return;
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      MyDamageInformation info = new MyDamageInformation(false, damage, damageType, attackerId);
      if (this.UseDamageSystem)
      {
        MyDamageSystem.Static.RaiseBeforeDamageApplied((object) this, ref info);
        damage = info.Amount;
      }
      MySession.Static.NegativeIntegrityTotal += damage;
      this.AccumulatedDamage += damage;
      if ((double) this.m_componentStack.Integrity - (double) this.AccumulatedDamage <= 1.52590218931437E-05)
      {
        this.ApplyAccumulatedDamage(addDirtyParts, attackerId);
        this.CubeGrid.RemoveFromDamageApplication(this);
      }
      else if (MyFakes.SHOW_DAMAGE_EFFECTS && this.FatBlock != null && (!this.FatBlock.Closed && !this.BlockDefinition.RatioEnoughForDamageEffect(this.BuildIntegrity / this.MaxIntegrity)) && this.BlockDefinition.RatioEnoughForDamageEffect((this.Integrity - damage) / this.MaxIntegrity))
        this.FatBlock.SetDamageEffect(true);
      if (this.UseDamageSystem)
        MyDamageSystem.Static.RaiseAfterDamageApplied((object) this, info);
      this.m_lastDamage = damage;
      this.m_lastAttackerId = attackerId;
      this.m_lastDamageType = damageType;
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
      if (!(customdata is MyCubeGrid.MyCubeGridHitInfo gridHitInfo))
        return;
      MyStringHash myStringHash = physicalMaterial.GetHashCode() == 0 ? MyStringHash.GetOrCompute(this.BlockDefinition.PhysicalMaterial.Id.SubtypeName) : physicalMaterial;
      MyDecalRenderInfo renderInfo = new MyDecalRenderInfo()
      {
        Source = source,
        PhysicalMaterial = myStringHash,
        VoxelMaterial = myStringHash,
        Forward = forwardDirection,
        IsTrail = isTrail
      };
      if (this.FatBlock == null)
      {
        renderInfo.Position = Vector3D.Transform(hitInfo.Position, this.CubeGrid.PositionComp.WorldMatrixInvScaled);
        renderInfo.Normal = (Vector3) Vector3D.TransformNormal(hitInfo.Normal, this.CubeGrid.PositionComp.WorldMatrixInvScaled);
        renderInfo.RenderObjectIds = this.CubeGrid.Render.RenderObjectIDs;
      }
      else
      {
        renderInfo.Position = Vector3D.Transform(hitInfo.Position, this.FatBlock.PositionComp.WorldMatrixInvScaled);
        renderInfo.Normal = (Vector3) Vector3D.TransformNormal(hitInfo.Normal, this.FatBlock.PositionComp.WorldMatrixInvScaled);
        renderInfo.RenderObjectIds = this.FatBlock.Render.RenderObjectIDs;
      }
      VertexBoneIndicesWeights? boneIndicesWeights = gridHitInfo.Triangle.GetAffectingBoneIndicesWeights(ref MySlimBlock.m_boneIndexWeightTmp);
      if (boneIndicesWeights.HasValue)
      {
        renderInfo.BoneIndices = boneIndicesWeights.Value.Indices;
        renderInfo.BoneWeights = boneIndicesWeights.Value.Weights;
      }
      if (MySlimBlock.m_tmpIds == null)
        MySlimBlock.m_tmpIds = new List<uint>();
      else
        MySlimBlock.m_tmpIds.Clear();
      decalHandler.AddDecal(ref renderInfo, MySlimBlock.m_tmpIds);
      foreach (uint tmpId in MySlimBlock.m_tmpIds)
        this.CubeGrid.RenderData.AddDecal(this.Position, gridHitInfo, tmpId);
    }

    public void ApplyDestructionDamage(float integrityRatioFromFracturedPieces)
    {
      if (!MyFakes.ENABLE_FRACTURE_COMPONENT || !Sync.IsServer || !MyPerGameSettings.Destruction)
        return;
      float damage = (this.ComponentStack.IntegrityRatio - integrityRatioFromFracturedPieces) * this.BlockDefinition.MaxIntegrity;
      if (this.CanApplyDestructionDamage(damage))
      {
        this.DoDamage(damage, MyDamageType.Destruction, true);
      }
      else
      {
        if (!this.CanApplyDestructionDamage(MyDefinitionManager.Static.DestructionDefinition.DestructionDamage))
          return;
        this.DoDamage(MyDefinitionManager.Static.DestructionDefinition.DestructionDamage, MyDamageType.Destruction, true);
      }
    }

    private bool CanApplyDestructionDamage(float damage)
    {
      if ((double) damage <= 0.0)
        return false;
      if (this.IsMultiBlockPart)
      {
        MyCubeGridMultiBlockInfo multiBlockInfo = this.CubeGrid.GetMultiBlockInfo(this.MultiBlockId);
        if (multiBlockInfo == null)
          return false;
        float totalMaxIntegrity = multiBlockInfo.GetTotalMaxIntegrity();
        foreach (MySlimBlock block in multiBlockInfo.Blocks)
        {
          float num = damage * block.MaxIntegrity / totalMaxIntegrity * block.DamageRatio * block.DeformationRatio + block.AccumulatedDamage;
          if ((double) block.Integrity - (double) num <= 1.52590218931437E-05)
            return false;
        }
        return true;
      }
      damage *= this.DamageRatio;
      damage *= this.DeformationRatio;
      damage += this.AccumulatedDamage;
      return (double) this.Integrity - (double) damage > 1.52590218931437E-05;
    }

    internal int GetTotalBreakableShapeChildrenCount()
    {
      if (this.FatBlock == null)
        return 0;
      string assetName = this.FatBlock.Model.AssetName;
      int num = 0;
      if (MySlimBlock.m_modelTotalFracturesCount.TryGetValue(assetName, out num))
        return num;
      MyModel modelOnlyData = MyModels.GetModelOnlyData(assetName);
      if (modelOnlyData.HavokBreakableShapes == null)
        MyDestructionData.Static.LoadModelDestruction(assetName, (MyPhysicalModelDefinition) this.BlockDefinition, Vector3.One);
      int totalChildrenCount = modelOnlyData.HavokBreakableShapes[0].GetTotalChildrenCount();
      MySlimBlock.m_modelTotalFracturesCount.Add(assetName, totalChildrenCount);
      return totalChildrenCount;
    }

    public void ApplyAccumulatedDamage(bool addDirtyParts = true, long attackerId = 0)
    {
      if (MySession.Static.SurvivalMode)
        this.EnsureConstructionStockpileExists();
      float integrity = this.Integrity;
      if (this.m_stockpile != null)
      {
        this.m_stockpile.ClearSyncList();
        this.m_componentStack.ApplyDamage(this.AccumulatedDamage, this.m_stockpile);
        if (Sync.IsServer)
          this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
        this.m_stockpile.ClearSyncList();
      }
      else
        this.m_componentStack.ApplyDamage(this.AccumulatedDamage);
      if (!this.BlockDefinition.RatioEnoughForDamageEffect(integrity / this.MaxIntegrity) && this.BlockDefinition.RatioEnoughForDamageEffect(this.Integrity / this.MaxIntegrity) && this.FatBlock != null)
        this.FatBlock.OnIntegrityChanged(this.BuildIntegrity, this.Integrity, false, MySession.Static.LocalPlayerId);
      this.AccumulatedDamage = 0.0f;
      if (!this.m_componentStack.IsDestroyed)
        return;
      if (MyFakes.SHOW_DAMAGE_EFFECTS && this.FatBlock != null)
        this.FatBlock.SetDamageEffect(false);
      this.CubeGrid.RemoveDestroyedBlock(this, attackerId);
      if (addDirtyParts)
        this.CubeGrid.Physics.AddDirtyBlock(this);
      if (!this.UseDamageSystem)
        return;
      MyDamageSystem.Static.RaiseDestroyed((object) this, new MyDamageInformation(false, this.m_lastDamage, this.m_lastDamageType, this.m_lastAttackerId));
    }

    public void UpdateVisual(bool updatePhysics = true)
    {
      bool flag = false;
      this.UpdateShowParts(true);
      if (!this.ShowParts)
      {
        if (this.FatBlock == null)
        {
          this.FatBlock = new MyCubeBlock();
          this.FatBlock.SlimBlock = this;
          this.FatBlock.Init();
          this.CubeGrid.Hierarchy.AddChild((VRage.ModAPI.IMyEntity) this.FatBlock);
          this.FatBlock.UpdateVisual();
        }
        else
          this.FatBlock.UpdateVisual();
      }
      else if (this.FatBlock != null)
      {
        Vector3D translation = this.FatBlock.WorldMatrix.Translation;
        this.CubeGrid.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) this.FatBlock);
        this.FatBlock.Close();
        this.FatBlock = (MyCubeBlock) null;
        flag = true;
      }
      this.CubeGrid.SetBlockDirty(this);
      if (flag)
        this.CubeGrid.UpdateDirty(immediate: true);
      if (!updatePhysics || this.CubeGrid.Physics == null)
        return;
      this.CubeGrid.Physics.AddDirtyArea(this.Min, this.Max);
    }

    public void IncreaseMountLevelToDesiredRatio(
      float desiredIntegrityRatio,
      long welderOwnerPlayerId,
      MyInventoryBase outputInventory = null,
      float maxAllowedBoneMovement = 0.0f,
      bool isHelping = false,
      MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction)
    {
      float num = desiredIntegrityRatio * this.MaxIntegrity - this.Integrity;
      if ((double) num <= 0.0)
        return;
      this.IncreaseMountLevel(num / this.BlockDefinition.IntegrityPointsPerSec, welderOwnerPlayerId, outputInventory, maxAllowedBoneMovement, isHelping, sharing);
    }

    public void DecreaseMountLevelToDesiredRatio(
      float desiredIntegrityRatio,
      MyInventoryBase outputInventory)
    {
      float num = this.Integrity - desiredIntegrityRatio * this.MaxIntegrity;
      if ((double) num <= 0.0)
        return;
      this.DecreaseMountLevel((this.FatBlock == null ? num * this.BlockDefinition.DisassembleRatio : num * this.FatBlock.DisassembleRatio) / this.BlockDefinition.IntegrityPointsPerSec, outputInventory, true);
    }

    public bool IncreaseMountLevel(
      float welderMountAmount,
      long welderOwnerIdentId,
      MyInventoryBase outputInventory = null,
      float maxAllowedBoneMovement = 0.0f,
      bool isHelping = false,
      MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction,
      bool handWelded = false,
      bool testingMode = false)
    {
      ulong user = 0;
      if (welderOwnerIdentId != 0L)
      {
        MyPlayer.PlayerId result;
        MySession.Static.Players.TryGetPlayerId(welderOwnerIdentId, out result);
        user = result.SteamId;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Welding, user: user))
        return false;
      double buildIntegrity1 = (double) this.ComponentStack.BuildIntegrity;
      float integrity = this.ComponentStack.Integrity;
      bool isFunctional = this.ComponentStack.IsFunctional;
      if (!testingMode)
        welderMountAmount *= this.BlockDefinition.IntegrityPointsPerSec;
      MySession.Static.PositiveIntegrityTotal += welderMountAmount;
      if (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(MySession.Static.Players.TryGetSteamId(welderOwnerIdentId)))
      {
        this.ClearConstructionStockpile(outputInventory);
      }
      else
      {
        MyEntity entity = (MyEntity) null;
        if (outputInventory != null && outputInventory.Container != null)
          entity = outputInventory.Container.Entity as MyEntity;
        if (entity != null && entity.InventoryOwnerType() == MyInventoryOwnerTypeEnum.Character)
          this.MoveItemsFromConstructionStockpile(outputInventory, MyItemFlags.Damaged);
      }
      float buildRatio = this.m_componentStack.BuildRatio;
      float currentDamage = this.CurrentDamage;
      if (this.BlockDefinition.RatioEnoughForOwnership(this.BuildLevelRatio) && this.FatBlock != null && (this.FatBlock.OwnerId != welderOwnerIdentId && outputInventory != null) && !isHelping)
        this.FatBlock.OnIntegrityChanged(this.BuildIntegrity, this.Integrity, true, welderOwnerIdentId, sharing);
      if (MyFakes.SHOW_DAMAGE_EFFECTS && !testingMode && (this.FatBlock != null && !this.BlockDefinition.RatioEnoughForDamageEffect((this.Integrity + welderMountAmount) / this.MaxIntegrity)))
        this.FatBlock.SetDamageEffect(false);
      bool flag = false;
      if (this.m_stockpile != null)
      {
        this.m_stockpile.ClearSyncList();
        this.m_componentStack.IncreaseMountLevel(welderMountAmount, this.m_stockpile);
        this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
        this.m_stockpile.ClearSyncList();
      }
      else
        this.m_componentStack.IncreaseMountLevel(welderMountAmount);
      if (this.m_componentStack.IsFullIntegrity)
      {
        this.ReleaseConstructionStockpile();
        flag = true;
      }
      MyIntegrityChangeEnum integrityChangeType = MyIntegrityChangeEnum.Damage;
      if (this.BlockDefinition.ModelChangeIsNeeded(buildRatio, this.m_componentStack.BuildRatio) || this.BlockDefinition.ModelChangeIsNeeded(this.m_componentStack.BuildRatio, buildRatio))
      {
        flag = true;
        if (this.FatBlock != null && this.m_componentStack.IsFunctional)
          integrityChangeType = MyIntegrityChangeEnum.ConstructionEnd;
        this.UpdateVisual(false);
        if (this.FatBlock != null)
        {
          if (this.CalculateCurrentModelID() == 0)
            integrityChangeType = MyIntegrityChangeEnum.ConstructionBegin;
          else if (!this.m_componentStack.IsFunctional)
            integrityChangeType = MyIntegrityChangeEnum.ConstructionProcess;
        }
        this.PlayConstructionSound(integrityChangeType, false);
        if (!testingMode)
          this.CreateConstructionSmokes();
        if (this.CubeGrid.GridSystems.GasSystem != null)
          this.CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioRaised((VRage.Game.ModAPI.IMySlimBlock) this);
      }
      else if (this.m_componentStack.IsFunctional && !isFunctional)
      {
        integrityChangeType = MyIntegrityChangeEnum.ConstructionEnd;
        this.PlayConstructionSound(integrityChangeType, false);
      }
      if (this.HasDeformation)
        this.CubeGrid.SetBlockDirty(this);
      if (flag)
        this.CubeGrid.RenderData.RemoveDecals(this.Position);
      this.CubeGrid.SendIntegrityChanged(this, integrityChangeType, 0L);
      this.CubeGrid.OnIntegrityChanged(this, handWelded);
      if (this.ComponentStack.IsFunctional && !isFunctional)
        MyCubeGrids.NotifyBlockFunctional(this.CubeGrid, this, handWelded);
      if ((double) maxAllowedBoneMovement != 0.0)
        this.FixBones(currentDamage, maxAllowedBoneMovement);
      if (MyFakes.ENABLE_GENERATED_BLOCKS && !this.BlockDefinition.IsGeneratedBlock && (this.BlockDefinition.GeneratedBlockDefinitions != null && this.BlockDefinition.GeneratedBlockDefinitions.Length != 0))
        this.UpdateProgressGeneratedBlocks(buildRatio);
      double buildIntegrity2 = (double) this.ComponentStack.BuildIntegrity;
      return buildIntegrity1 != buildIntegrity2 || (double) integrity != (double) this.ComponentStack.Integrity;
    }

    public void DecreaseMountLevel(
      float grinderAmount,
      MyInventoryBase outputInventory,
      bool useDefaultDeconstructEfficiency = false,
      long identityId = 0,
      bool testingMode = false)
    {
      if (!Sync.IsServer || this.m_componentStack.IsFullyDismounted)
        return;
      ulong user = 0;
      if (identityId != 0L)
      {
        MyPlayer.PlayerId result;
        MySession.Static.Players.TryGetPlayerId(identityId, out result);
        user = result.SteamId;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Grinding, user: user))
        return;
      if (!testingMode)
      {
        if (this.FatBlock != null)
          grinderAmount /= this.FatBlock.DisassembleRatio;
        else
          grinderAmount /= this.BlockDefinition.DisassembleRatio;
        grinderAmount *= this.BlockDefinition.IntegrityPointsPerSec;
      }
      float buildRatio1 = this.m_componentStack.BuildRatio;
      this.DeconstructStockpile(grinderAmount, outputInventory, useDefaultDeconstructEfficiency);
      float buildRatio2 = this.m_componentStack.BuildRatio;
      if (this.BlockDefinition.RatioEnoughForDamageEffect(this.BuildLevelRatio) && this.FatBlock != null && this.FatBlock.OwnerId != 0L)
        this.FatBlock.OnIntegrityChanged(this.BuildIntegrity, this.Integrity, false, 0L);
      long num1 = 0;
      MyIDModule component;
      if (outputInventory != null && outputInventory.Entity != null && (outputInventory.Entity is IMyComponentOwner<MyIDModule> entity && entity.GetComponent(out component)))
        num1 = component.Owner;
      this.UpdateHackingIndicator(buildRatio2, buildRatio1, num1);
      int num2 = this.BlockDefinition.ModelChangeIsNeeded(this.m_componentStack.BuildRatio, buildRatio1) ? 1 : 0;
      MyIntegrityChangeEnum integrityChangeType = MyIntegrityChangeEnum.Damage;
      if (num2 != 0)
      {
        this.UpdateVisual(false);
        if (this.FatBlock != null)
        {
          int currentModelId = this.CalculateCurrentModelID();
          integrityChangeType = currentModelId == -1 || (double) this.BuildLevelRatio == 0.0 ? MyIntegrityChangeEnum.DeconstructionEnd : (currentModelId != this.BlockDefinition.BuildProgressModels.Length - 1 ? MyIntegrityChangeEnum.DeconstructionProcess : MyIntegrityChangeEnum.DeconstructionBegin);
          this.FatBlock.SetDamageEffect(false);
        }
        this.PlayConstructionSound(integrityChangeType, true);
        if (!testingMode)
          this.CreateConstructionSmokes();
        if (this.CubeGrid.GridSystems.GasSystem != null)
          this.CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioLowered((VRage.Game.ModAPI.IMySlimBlock) this);
      }
      if (MyFakes.ENABLE_GENERATED_BLOCKS && !this.BlockDefinition.IsGeneratedBlock && (this.BlockDefinition.GeneratedBlockDefinitions != null && this.BlockDefinition.GeneratedBlockDefinitions.Length != 0))
        this.UpdateProgressGeneratedBlocks(buildRatio1);
      this.CubeGrid.SendIntegrityChanged(this, integrityChangeType, num1);
      this.CubeGrid.OnIntegrityChanged(this, false);
    }

    public void FullyDismount(MyInventory outputInventory)
    {
      if (!Sync.IsServer)
        return;
      this.DeconstructStockpile(this.BuildIntegrity, (MyInventoryBase) outputInventory);
      if (!this.BlockDefinition.ModelChangeIsNeeded(this.m_componentStack.BuildRatio, this.m_componentStack.BuildRatio))
        return;
      this.UpdateVisual(true);
      this.PlayConstructionSound(MyIntegrityChangeEnum.DeconstructionEnd, true);
      this.CreateConstructionSmokes();
      if (this.CubeGrid.GridSystems.GasSystem == null)
        return;
      this.CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioLowered((VRage.Game.ModAPI.IMySlimBlock) this);
    }

    private void DeconstructStockpile(
      float deconstructAmount,
      MyInventoryBase outputInventory,
      bool useDefaultDeconstructEfficiency = false)
    {
      if (MySession.Static.CreativeMode)
        this.ClearConstructionStockpile(outputInventory);
      else
        this.EnsureConstructionStockpileExists();
      if (this.m_stockpile != null)
      {
        this.m_stockpile.ClearSyncList();
        this.m_componentStack.DecreaseMountLevel(deconstructAmount, this.m_stockpile, useDefaultDeconstructEfficiency);
        this.CubeGrid.SendStockpileChanged(this, this.m_stockpile.GetSyncList());
        this.m_stockpile.ClearSyncList();
      }
      else
        this.m_componentStack.DecreaseMountLevel(deconstructAmount, useDefaultDeconstructEfficiency: useDefaultDeconstructEfficiency);
    }

    private void CreateConstructionSmokes()
    {
      Vector3 vector3_1 = new Vector3(this.CubeGrid.GridSize) / 2f;
      BoundingBox boundingBox1 = new BoundingBox(this.Min * this.CubeGrid.GridSize - vector3_1, this.Max * this.CubeGrid.GridSize + vector3_1);
      if (this.FatBlock != null && this.FatBlock.Model != null)
      {
        BoundingBox boundingBox2 = new BoundingBox(this.FatBlock.Model.BoundingBox.Min, this.FatBlock.Model.BoundingBox.Max);
        Matrix result;
        this.FatBlock.Orientation.GetMatrix(out result);
        BoundingBox boundingBox3 = BoundingBox.CreateInvalid();
        foreach (Vector3 corner in boundingBox2.GetCorners())
          boundingBox3 = boundingBox3.Include(Vector3.Transform(corner, result));
        boundingBox1 = new BoundingBox(boundingBox3.Min + boundingBox1.Center, boundingBox3.Max + boundingBox1.Center);
      }
      if (MySlimBlock.ConstructionParticlesTimedCache.IsPlaceUsed(this.WorldPosition, MySlimBlock.ConstructionParticleSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds))
        return;
      boundingBox1.Inflate(-0.3f);
      Vector3[] corners = boundingBox1.GetCorners();
      float num1 = 0.25f;
      for (int index = 0; index < MyOrientedBoundingBox.StartVertices.Length; ++index)
      {
        Vector3 position = corners[MyOrientedBoundingBox.StartVertices[index]];
        float num2 = 0.0f;
        float num3 = Vector3.Distance(position, corners[MyOrientedBoundingBox.EndVertices[index]]);
        Vector3 vector3_2 = num1 * Vector3.Normalize(corners[MyOrientedBoundingBox.EndVertices[index]] - corners[MyOrientedBoundingBox.StartVertices[index]]);
        while ((double) num2 < (double) num3)
        {
          MyParticleEffect effect;
          if (MyParticlesManager.TryCreateParticleEffect("Smoke_Construction", MatrixD.CreateTranslation(Vector3D.Transform(position, this.CubeGrid.WorldMatrix)), out effect))
            effect.Velocity = this.CubeGrid.Physics.LinearVelocity;
          num2 += num1;
          position += vector3_2;
        }
      }
    }

    public override string ToString() => this.FatBlock == null ? this.BlockDefinition.DisplayNameText.ToString() : this.FatBlock.ToString();

    public static void ComputeMax(
      MyCubeBlockDefinition definition,
      MyBlockOrientation orientation,
      ref Vector3I min,
      out Vector3I max)
    {
      Vector3I result = definition.Size - 1;
      MatrixI matrix = new MatrixI(orientation);
      Vector3I.TransformNormal(ref result, ref matrix, out result);
      Vector3I.Abs(ref result, out result);
      max = min + result;
    }

    public void SetIntegrity(
      float buildIntegrity,
      float integrity,
      MyIntegrityChangeEnum integrityChangeType,
      long grinderOwner)
    {
      float buildRatio = this.m_componentStack.BuildRatio;
      this.m_componentStack.SetIntegrity(buildIntegrity, integrity);
      if (this.FatBlock != null && !this.BlockDefinition.RatioEnoughForOwnership(buildRatio) && this.BlockDefinition.RatioEnoughForOwnership(this.m_componentStack.BuildRatio))
        this.FatBlock.OnIntegrityChanged(buildIntegrity, integrity, true, MySession.Static.LocalPlayerId);
      this.UpdateHackingIndicator(this.m_componentStack.BuildRatio, buildRatio, grinderOwner);
      if (MyFakes.SHOW_DAMAGE_EFFECTS && this.FatBlock != null && !this.BlockDefinition.RatioEnoughForDamageEffect(this.Integrity / this.MaxIntegrity))
        this.FatBlock.SetDamageEffect(false);
      bool flag = this.IsFullIntegrity;
      if (this.ModelChangeIsNeeded(this.m_componentStack.BuildRatio, buildRatio))
      {
        flag = true;
        this.UpdateVisual(true);
        if (integrityChangeType != MyIntegrityChangeEnum.Damage)
          this.CreateConstructionSmokes();
        this.PlayConstructionSound(integrityChangeType, false);
        if (this.CubeGrid.GridSystems.GasSystem != null)
        {
          if ((double) buildRatio > (double) this.m_componentStack.BuildRatio)
            this.CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioLowered((VRage.Game.ModAPI.IMySlimBlock) this);
          else
            this.CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioRaised((VRage.Game.ModAPI.IMySlimBlock) this);
        }
      }
      if (flag)
        this.CubeGrid.RenderData.RemoveDecals(this.Position);
      if (!MyFakes.ENABLE_GENERATED_BLOCKS || this.BlockDefinition.IsGeneratedBlock || (this.BlockDefinition.GeneratedBlockDefinitions == null || this.BlockDefinition.GeneratedBlockDefinitions.Length == 0))
        return;
      this.UpdateProgressGeneratedBlocks(buildRatio);
    }

    private void UpdateHackingIndicator(float newRatio, float oldRatio, long grinderOwner)
    {
      if ((double) newRatio >= (double) oldRatio || this.FatBlock == null || this.FatBlock.IDModule == null)
        return;
      switch (this.FatBlock.IDModule.GetUserRelationToOwner(grinderOwner))
      {
        case MyRelationsBetweenPlayerAndBlock.Neutral:
        case MyRelationsBetweenPlayerAndBlock.Enemies:
          if (!(this.FatBlock is MyTerminalBlock fatBlock))
            break;
          fatBlock.HackAttemptTime = new int?(MySandboxGame.TotalSimulationTimeInMilliseconds);
          if (MySlimBlock.OnAnyBlockHackedChanged == null)
            break;
          MySlimBlock.OnAnyBlockHackedChanged(fatBlock, grinderOwner);
          break;
      }
    }

    public void PlayConstructionSound(
      MyIntegrityChangeEnum integrityChangeType,
      bool deconstruction = false)
    {
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      if (this.FatBlock != null)
        soundEmitter.SetPosition(new Vector3D?(this.FatBlock.PositionComp.GetPosition()));
      else
        soundEmitter.SetPosition(new Vector3D?(this.CubeGrid.PositionComp.GetPosition() + (this.Position - 1) * this.CubeGrid.GridSize));
      switch (integrityChangeType)
      {
        case MyIntegrityChangeEnum.ConstructionBegin:
          if (deconstruction)
          {
            soundEmitter.PlaySound(MySlimBlock.DECONSTRUCTION_START, true, alwaysHearOnRealistic: true);
            break;
          }
          soundEmitter.PlaySound(MySlimBlock.CONSTRUCTION_START, true, alwaysHearOnRealistic: true);
          break;
        case MyIntegrityChangeEnum.ConstructionEnd:
          if (deconstruction)
          {
            soundEmitter.PlaySound(MySlimBlock.DECONSTRUCTION_END, true, alwaysHearOnRealistic: true);
            break;
          }
          soundEmitter.PlaySound(MySlimBlock.CONSTRUCTION_END, true, alwaysHearOnRealistic: true);
          break;
        case MyIntegrityChangeEnum.ConstructionProcess:
          if (deconstruction)
          {
            soundEmitter.PlaySound(MySlimBlock.DECONSTRUCTION_PROG, true, alwaysHearOnRealistic: true);
            break;
          }
          soundEmitter.PlaySound(MySlimBlock.CONSTRUCTION_PROG, true, alwaysHearOnRealistic: true);
          break;
        case MyIntegrityChangeEnum.DeconstructionBegin:
          soundEmitter.PlaySound(MySlimBlock.DECONSTRUCTION_START, true, alwaysHearOnRealistic: true);
          break;
        case MyIntegrityChangeEnum.DeconstructionEnd:
          soundEmitter.PlaySound(MySlimBlock.DECONSTRUCTION_END, true, alwaysHearOnRealistic: true);
          break;
        case MyIntegrityChangeEnum.DeconstructionProcess:
          soundEmitter.PlaySound(MySlimBlock.DECONSTRUCTION_PROG, true, alwaysHearOnRealistic: true);
          break;
        default:
          soundEmitter.PlaySound(MySoundPair.Empty);
          break;
      }
    }

    private bool ModelChangeIsNeeded(float a, float b) => (double) a > (double) b ? this.BlockDefinition.ModelChangeIsNeeded(b, a) : this.BlockDefinition.ModelChangeIsNeeded(a, b);

    public void UpgradeBuildLevel()
    {
      float buildRatio = this.m_componentStack.BuildRatio;
      float num1 = 1f;
      foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in this.BlockDefinition.BuildProgressModels)
      {
        if ((double) buildProgressModel.BuildRatioUpperBound > (double) buildRatio && (double) buildProgressModel.BuildRatioUpperBound <= (double) num1)
          num1 = buildProgressModel.BuildRatioUpperBound;
      }
      float num2 = MathHelper.Clamp(num1 * 1.001f, 0.0f, 1f);
      this.m_componentStack.SetIntegrity(num2 * this.BlockDefinition.MaxIntegrity, num2 * this.BlockDefinition.MaxIntegrity);
    }

    public void SetBuildLevel(int level = 0)
    {
      double buildRatio = (double) this.m_componentStack.BuildRatio;
      float num1 = 0.0f;
      float num2 = 1f;
      if (level == int.MaxValue)
      {
        float num3 = this.MaxIntegrity - this.Integrity;
        if ((double) num3 == 0.0)
          return;
        this.IncreaseMountLevel(Math.Abs(num3), MySession.Static.LocalCharacter.EntityId, (MyInventoryBase) null, 0.0f, false, MyOwnershipShareModeEnum.Faction, false, true);
      }
      else
      {
        if (level < 0)
          level = 0;
        if (this.BlockDefinition.BuildProgressModels.Length - 1 < level)
          level = this.BlockDefinition.BuildProgressModels.Length - 1;
        if (level < this.BlockDefinition.BuildProgressModels.Length)
        {
          num2 = this.BlockDefinition.BuildProgressModels[level].BuildRatioUpperBound;
          if (level > 0)
            num1 = this.BlockDefinition.BuildProgressModels[level - 1].BuildRatioUpperBound;
        }
        float num3 = MathHelper.Clamp((float) (((double) num1 + (double) num2) / 2.0), 0.0f, 1f) * this.BlockDefinition.MaxIntegrity - this.Integrity;
        if ((double) num3 < 0.0)
          this.DecreaseMountLevel(Math.Abs(num3), MySession.Static.LocalCharacter.GetInventoryBase(), true, testingMode: true);
        if ((double) num3 <= 0.0)
          return;
        this.IncreaseMountLevel(Math.Abs(num3), MySession.Static.LocalCharacter.EntityId, (MyInventoryBase) null, 0.0f, false, MyOwnershipShareModeEnum.Faction, false, true);
      }
    }

    public void RandomizeBuildLevel()
    {
      float num = MyUtils.GetRandomFloat(0.0f, 1f) * this.BlockDefinition.MaxIntegrity;
      this.m_componentStack.SetIntegrity(num, num);
    }

    internal void ChangeStockpile(List<MyStockpileItem> items)
    {
      this.EnsureConstructionStockpileExists();
      this.m_stockpile.Change(items);
      if (!this.m_stockpile.IsEmpty())
        return;
      this.ReleaseConstructionStockpile();
    }

    internal void GetConstructionStockpileItems(List<MyStockpileItem> m_cacheStockpileItems)
    {
      if (this.m_stockpile == null)
        return;
      foreach (MyStockpileItem myStockpileItem in this.m_stockpile.GetItems())
        m_cacheStockpileItems.Add(myStockpileItem);
    }

    internal void RequestFillStockpile(MyInventory SourceInventory)
    {
      MySlimBlock.m_tmpComponents.Clear();
      this.GetMissingComponents(MySlimBlock.m_tmpComponents);
      foreach (KeyValuePair<string, int> mTmpComponent in MySlimBlock.m_tmpComponents)
      {
        MyDefinitionId contentId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), mTmpComponent.Key);
        if (SourceInventory.ContainItems(new MyFixedPoint?((MyFixedPoint) 1), contentId))
        {
          this.CubeGrid.RequestFillStockpile(this.Position, SourceInventory);
          break;
        }
      }
    }

    public void ComputeWorldCenter(out Vector3D worldCenter)
    {
      this.ComputeScaledCenter(out worldCenter);
      MatrixD worldMatrix = this.CubeGrid.WorldMatrix;
      Vector3D.Transform(ref worldCenter, ref worldMatrix, out worldCenter);
    }

    public void ComputeScaledCenter(out Vector3D scaledCenter) => scaledCenter = (Vector3D) ((this.Max + this.Min) * this.CubeGrid.GridSizeHalf);

    public void ComputeScaledHalfExtents(out Vector3 scaledHalfExtents) => scaledHalfExtents = (this.Max + 1 - this.Min) * this.CubeGrid.GridSizeHalf;

    public float GetMass()
    {
      if (this.FatBlock != null)
        return this.FatBlock.GetMass();
      return MyDestructionData.Static != null ? MyDestructionData.Static.GetBlockMass(this.CalculateCurrentModel(out Matrix _), this.BlockDefinition) : this.BlockDefinition.Mass;
    }

    public void OnDestroyVisual()
    {
      if (!MyFakes.SHOW_DAMAGE_EFFECTS || this.CubeGrid.IsLargeDestroyInProgress)
        return;
      int num = this.FatBlock == null || !this.FatBlock.IsBuilt ? (this.FatBlock == null ? 1 : 0) : 1;
      string str = num != 0 ? this.BlockDefinition.DestroyEffect : (string) null;
      if (string.IsNullOrEmpty(str))
        str = this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? "BlockDestroyed_Large3X" : "BlockDestroyed_Large";
      MySoundPair mySoundPair = num != 0 ? this.BlockDefinition.DestroySound : (MySoundPair) null;
      if (mySoundPair == null || mySoundPair == MySoundPair.Empty)
        mySoundPair = this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? MyExplosion.LargePoofSound : MyExplosion.SmallPoofSound;
      bool flag1 = this.FatBlock != null && this.FatBlock.Model != null && (double) this.FatBlock.Model.BoundingSphere.Radius > 0.5 || this.FatBlock == null;
      Vector3D vector3D = Vector3D.Zero;
      if (this.BlockDefinition.DestroyEffectOffset.HasValue && !this.BlockDefinition.DestroyEffectOffset.Value.Equals(Vector3.Zero))
      {
        Matrix result;
        this.Orientation.GetMatrix(out result);
        vector3D = Vector3D.Rotate(new Vector3D(Vector3.RotateAndScale(this.BlockDefinition.DestroyEffectOffset.Value, result)), this.CubeGrid.WorldMatrix);
      }
      BoundingSphereD fromBoundingBox = BoundingSphereD.CreateFromBoundingBox(this.WorldAABB);
      fromBoundingBox.Center += vector3D;
      if (MyFakes.DEBUG_DISPLAY_DESTROY_EFFECT_OFFSET)
      {
        Matrix result;
        this.Orientation.GetMatrix(out result);
        MatrixD matrix = MatrixD.Multiply(new MatrixD(result), this.CubeGrid.WorldMatrix);
        matrix.Translation = this.WorldPosition;
        MyRenderProxy.DebugDrawAxis(matrix, 1f, false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(this.WorldPosition, fromBoundingBox.Center, Color.Red, Color.Yellow, false, true);
      }
      bool flag2 = this.CubeGrid.Physics != null && this.CubeGrid.Physics.IsPlanetCrashing_PointConcealed(this.WorldPosition);
      MyExplosionInfo explosionInfo = new MyExplosionInfo()
      {
        PlayerDamage = 0.0f,
        Damage = 0.0f,
        ExplosionType = MyExplosionTypeEnum.CUSTOM,
        ExplosionSphere = fromBoundingBox,
        LifespanMiliseconds = 700,
        HitEntity = (MyEntity) this.CubeGrid,
        ParticleScale = 1f,
        CustomEffect = str,
        CustomSound = mySoundPair,
        OwnerEntity = (MyEntity) this.CubeGrid,
        Direction = new Vector3?((Vector3) this.CubeGrid.WorldMatrix.Forward),
        VoxelExplosionCenter = this.WorldPosition + vector3D,
        ExplosionFlags = (MyExplosionFlags) ((flag1 ? 1 : 0) | 8 | (flag2 ? 0 : 32)),
        VoxelCutoutScale = 0.0f,
        PlaySound = true,
        ApplyForceAndDamage = true,
        ObjectsRemoveDelayInMiliseconds = 40
      };
      if (this.CubeGrid.Physics != null)
        explosionInfo.Velocity = this.CubeGrid.Physics.LinearVelocity;
      MyExplosions.AddExplosion(ref explosionInfo, false);
    }

    void IMyDestroyableObject.OnDestroy()
    {
      if (this.FatBlock != null)
        this.FatBlock.OnDestroy();
      this.OnDestroyVisual();
      this.m_componentStack.DestroyCompletely();
      this.ReleaseUnneededStockpileItems();
      this.CubeGrid.RemoveFromDamageApplication(this);
      this.AccumulatedDamage = 0.0f;
    }

    float IMyDestroyableObject.Integrity => this.Integrity;

    internal void Transform(ref MatrixI transform)
    {
      Vector3I result1;
      Vector3I.Transform(ref this.Min, ref transform, out result1);
      Vector3I result2;
      Vector3I.Transform(ref this.Max, ref transform, out result2);
      Vector3I result3;
      Vector3I.Transform(ref this.Position, ref transform, out result3);
      Vector3I intVector1 = Base6Directions.GetIntVector(transform.GetDirection(this.Orientation.Forward));
      Vector3I intVector2 = Base6Directions.GetIntVector(transform.GetDirection(this.Orientation.Up));
      this.InitOrientation(ref intVector1, ref intVector2);
      this.Min = Vector3I.Min(result1, result2);
      this.Max = Vector3I.Max(result1, result2);
      this.Position = result3;
      if (this.FatBlock == null)
        return;
      this.FatBlock.OnTransformed(ref transform);
    }

    public void GetWorldBoundingBox(out BoundingBoxD aabb, bool useAABBFromBlockCubes = false)
    {
      if (this.FatBlock != null && !useAABBFromBlockCubes)
      {
        aabb = this.FatBlock.PositionComp.WorldAABB;
      }
      else
      {
        float gridSize = this.CubeGrid.GridSize;
        aabb = new BoundingBoxD((Vector3D) (this.Min * gridSize - gridSize / 2f), (Vector3D) (this.Max * gridSize + gridSize / 2f));
        aabb = aabb.TransformFast(this.CubeGrid.WorldMatrix);
      }
    }

    public static void SetBlockComponents(
      MyHudBlockInfo hudInfo,
      MySlimBlock block,
      MyInventoryBase availableInventory = null)
    {
      MySlimBlock.SetBlockComponentsInternal(hudInfo, block.BlockDefinition, block, availableInventory);
    }

    public static void SetBlockComponents(
      MyHudBlockInfo hudInfo,
      MyCubeBlockDefinition blockDefinition,
      MyInventoryBase availableInventory = null)
    {
      MySlimBlock.SetBlockComponentsInternal(hudInfo, blockDefinition, (MySlimBlock) null, availableInventory);
    }

    private static void SetBlockComponentsInternal(
      MyHudBlockInfo hudInfo,
      MyCubeBlockDefinition blockDefinition,
      MySlimBlock block,
      MyInventoryBase availableInventory)
    {
      hudInfo.Components.Clear();
      hudInfo.InitBlockInfo(blockDefinition, block);
      hudInfo.ShowAvailable = MyPerGameSettings.AlwaysShowAvailableBlocksOnHud;
      if (!MyFakes.ENABLE_SMALL_GRID_BLOCK_COMPONENT_INFO && blockDefinition.CubeSize == MyCubeSize.Small)
        return;
      if (block != null)
        hudInfo.BlockIntegrity = block.Integrity / block.MaxIntegrity;
      if (block != null && block.IsMultiBlockPart)
      {
        MyCubeGridMultiBlockInfo multiBlockInfo = block.CubeGrid.GetMultiBlockInfo(block.MultiBlockId);
        if (multiBlockInfo == null)
          return;
        foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition1 in multiBlockInfo.MultiBlockDefinition.BlockDefinitions)
        {
          MyCubeBlockDefinition blockDefinition2;
          if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefinition1.Id, out blockDefinition2))
            hudInfo.AddComponentsForBlock(blockDefinition2);
        }
        hudInfo.MergeSameComponents();
        foreach (MySlimBlock block1 in multiBlockInfo.Blocks)
        {
          for (int index1 = 0; index1 < block1.BlockDefinition.Components.Length; ++index1)
          {
            MyCubeBlockDefinition.Component component1 = block1.BlockDefinition.Components[index1];
            MyComponentStack.GroupInfo groupInfo = block1.ComponentStack.GetGroupInfo(index1);
            for (int index2 = 0; index2 < hudInfo.Components.Count; ++index2)
            {
              if (hudInfo.Components[index2].DefinitionId == component1.Definition.Id)
              {
                MyHudBlockInfo.ComponentInfo component2 = hudInfo.Components[index2];
                component2.MountedCount += groupInfo.MountedCount;
                hudInfo.Components[index2] = component2;
                break;
              }
            }
          }
        }
        for (int index = 0; index < hudInfo.Components.Count; ++index)
        {
          if (availableInventory != null)
          {
            MyHudBlockInfo.ComponentInfo component = hudInfo.Components[index];
            component.AvailableAmount = (int) MyCubeBuilder.BuildComponent.GetItemAmountCombined(availableInventory, component.DefinitionId);
            hudInfo.Components[index] = component;
          }
          int amount = 0;
          foreach (MySlimBlock block1 in multiBlockInfo.Blocks)
          {
            if (!block1.StockpileEmpty)
              amount += block1.GetConstructionStockpileItemAmount(hudInfo.Components[index].DefinitionId);
          }
          if (amount > 0)
            MySlimBlock.SetHudInfoComponentAmount(hudInfo, amount, index);
        }
      }
      else if (block == null && blockDefinition.MultiBlock != null)
      {
        MyMultiBlockDefinition multiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_MultiBlockDefinition), blockDefinition.MultiBlock));
        if (multiBlockDefinition == null)
          return;
        foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition1 in multiBlockDefinition.BlockDefinitions)
        {
          MyCubeBlockDefinition blockDefinition2;
          if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefinition1.Id, out blockDefinition2))
            hudInfo.AddComponentsForBlock(blockDefinition2);
        }
        hudInfo.MergeSameComponents();
        for (int index = 0; index < hudInfo.Components.Count; ++index)
        {
          MyHudBlockInfo.ComponentInfo component = hudInfo.Components[index];
          component.AvailableAmount = (int) MyCubeBuilder.BuildComponent.GetItemAmountCombined(availableInventory, component.DefinitionId);
          hudInfo.Components[index] = component;
        }
      }
      else
      {
        for (int index = 0; index < blockDefinition.Components.Length; ++index)
        {
          MyComponentStack.GroupInfo groupInfo = new MyComponentStack.GroupInfo();
          if (block != null)
          {
            groupInfo = block.ComponentStack.GetGroupInfo(index);
          }
          else
          {
            MyCubeBlockDefinition.Component component = blockDefinition.Components[index];
            groupInfo.Component = component.Definition;
            groupInfo.TotalCount = component.Count;
            groupInfo.MountedCount = 0;
            groupInfo.AvailableCount = 0;
            groupInfo.Integrity = 0.0f;
            groupInfo.MaxIntegrity = (float) (component.Count * component.Definition.MaxIntegrity);
          }
          MySlimBlock.AddBlockComponent(hudInfo, groupInfo, availableInventory);
        }
        if (block == null || block.StockpileEmpty)
          return;
        foreach (MyCubeBlockDefinition.Component component in block.BlockDefinition.Components)
        {
          int amount = block.GetConstructionStockpileItemAmount(component.Definition.Id);
          if (amount > 0)
          {
            for (int index = 0; index < hudInfo.Components.Count; ++index)
            {
              if (block.ComponentStack.GetGroupInfo(index).Component == component.Definition)
              {
                if (block.ComponentStack.IsFullyDismounted)
                  return;
                amount = MySlimBlock.SetHudInfoComponentAmount(hudInfo, amount, index);
              }
            }
          }
        }
      }
    }

    private static int SetHudInfoComponentAmount(MyHudBlockInfo hudInfo, int amount, int i)
    {
      MyHudBlockInfo.ComponentInfo component = hudInfo.Components[i];
      int num = Math.Min(component.TotalCount - component.MountedCount, amount);
      component.StockpileCount = num;
      amount -= num;
      hudInfo.Components[i] = component;
      return amount;
    }

    private static void AddBlockComponent(
      MyHudBlockInfo hudInfo,
      MyComponentStack.GroupInfo groupInfo,
      MyInventoryBase availableInventory)
    {
      MyHudBlockInfo.ComponentInfo componentInfo = new MyHudBlockInfo.ComponentInfo();
      componentInfo.DefinitionId = groupInfo.Component.Id;
      componentInfo.ComponentName = groupInfo.Component.DisplayNameText;
      componentInfo.Icons = groupInfo.Component.Icons;
      componentInfo.TotalCount = groupInfo.TotalCount;
      componentInfo.MountedCount = groupInfo.MountedCount;
      if (availableInventory != null)
        componentInfo.AvailableAmount = (int) MyCubeBuilder.BuildComponent.GetItemAmountCombined(availableInventory, groupInfo.Component.Id);
      hudInfo.Components.Add(componentInfo);
    }

    private void UpdateProgressGeneratedBlocks(float oldBuildRatio)
    {
      float buildRatio = this.ComponentStack.BuildRatio;
      if ((double) oldBuildRatio == (double) buildRatio || (double) oldBuildRatio < (double) buildRatio || ((double) oldBuildRatio < (double) this.BlockDefinition.BuildProgressToPlaceGeneratedBlocks || (double) buildRatio >= (double) this.BlockDefinition.BuildProgressToPlaceGeneratedBlocks))
        return;
      MySlimBlock.m_tmpBlocks.Clear();
      this.CubeGrid.RazeGeneratedBlocks(MySlimBlock.m_tmpBlocks);
      MySlimBlock.m_tmpBlocks.Clear();
    }

    public MyFractureComponentCubeBlock GetFractureComponent() => this.FatBlock == null ? (MyFractureComponentCubeBlock) null : this.FatBlock.GetFractureComponent();

    private void RepairMultiBlock(long toolOwnerId)
    {
      MyCubeGridMultiBlockInfo multiBlockInfo = this.CubeGrid.GetMultiBlockInfo(this.MultiBlockId);
      if (multiBlockInfo == null || !multiBlockInfo.IsFractured())
        return;
      MySlimBlock.m_tmpMultiBlocks.AddRange((IEnumerable<MySlimBlock>) multiBlockInfo.Blocks);
      foreach (MySlimBlock mTmpMultiBlock in MySlimBlock.m_tmpMultiBlocks)
      {
        if (mTmpMultiBlock.GetFractureComponent() != null)
          mTmpMultiBlock.RepairFracturedBlock(toolOwnerId);
      }
      MySlimBlock.m_tmpMultiBlocks.Clear();
    }

    public void RepairFracturedBlockWithFullHealth(long toolOwnerId)
    {
      if (this.BlockDefinition.IsGeneratedBlock)
        return;
      if (MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION && this.IsMultiBlockPart)
      {
        this.RepairMultiBlock(toolOwnerId);
        if (MySession.Static.SurvivalMode)
          return;
        this.CubeGrid.AddMissingBlocksInMultiBlock(this.MultiBlockId, toolOwnerId);
      }
      else
      {
        if (this.GetFractureComponent() == null)
          return;
        this.RepairFracturedBlock(toolOwnerId);
      }
    }

    internal void RepairFracturedBlock(long toolOwnerId)
    {
      if (this.FatBlock == null)
        return;
      this.RemoveFractureComponent();
      foreach (MySlimBlock mTmpBlock in MySlimBlock.m_tmpBlocks)
      {
        mTmpBlock.RemoveFractureComponent();
        mTmpBlock.SetGeneratedBlockIntegrity(this);
      }
      MySlimBlock.m_tmpBlocks.Clear();
      this.UpdateProgressGeneratedBlocks(0.0f);
      if (!Sync.IsServer)
        return;
      BoundingBoxD worldAabb = this.FatBlock.PositionComp.WorldAABB;
      if (this.BlockDefinition.CubeSize == MyCubeSize.Large)
        worldAabb.Inflate(-0.16);
      else
        worldAabb.Inflate(-0.04);
      MyFracturedPiecesManager.Static.RemoveFracturesInBox(ref worldAabb, 0.0f);
      this.CubeGrid.SendFractureComponentRepaired(this, toolOwnerId);
    }

    internal void RemoveFractureComponent()
    {
      if (!this.FatBlock.Components.Has<MyFractureComponentBase>())
        return;
      this.FatBlock.Components.Remove<MyFractureComponentBase>();
      this.FatBlock.Render.UpdateRenderObject(false);
      this.FatBlock.CreateRenderer(this.FatBlock.Render.PersistentFlags, this.FatBlock.Render.ColorMaskHsv, this.FatBlock.Render.ModelStorage);
      this.UpdateVisual(true);
      this.FatBlock.Render.UpdateRenderObject(true);
      MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(this.Position);
      cubeBlock?.CubeGrid.UpdateBlockNeighbours(cubeBlock);
    }

    public void SetGeneratedBlockIntegrity(MySlimBlock generatingBlock)
    {
      if (!this.BlockDefinition.IsGeneratedBlock)
        return;
      float buildRatio = this.ComponentStack.BuildRatio;
      this.ComponentStack.SetIntegrity(generatingBlock.BuildLevelRatio * this.MaxIntegrity, generatingBlock.ComponentStack.IntegrityRatio * this.MaxIntegrity);
      if (!this.ModelChangeIsNeeded(this.ComponentStack.BuildRatio, buildRatio))
        return;
      this.UpdateVisual(true);
    }

    public void GetLocalMatrix(out Matrix localMatrix)
    {
      this.Orientation.GetMatrix(out localMatrix);
      localMatrix.Translation = (this.Min + this.Max) * 0.5f * this.CubeGrid.GridSize;
      Vector3 result;
      Vector3.TransformNormal(ref this.BlockDefinition.ModelOffset, ref localMatrix, out result);
      localMatrix.Translation += result;
    }

    private static void DoDamageSynced(
      MySlimBlock block,
      float damage,
      MyStringHash damageType,
      MyHitInfo? hitInfo,
      long attackerId)
    {
      MySlimBlock.SendDamage(block, damage, damageType, hitInfo, attackerId);
      block.DoDamage(damage, damageType, hitInfo, attackerId: attackerId);
    }

    public static void SendDamageBatch(
      Dictionary<MySlimBlock, float> blocks,
      MyStringHash damageType,
      long attackerId)
    {
      if (blocks.Count == 0)
        return;
      MyCubeGrid cubeGrid = blocks.FirstPair<MySlimBlock, float>().Key.CubeGrid;
      if (cubeGrid.MarkedForClose)
        return;
      using (MyUtils.ClearCollectionToken<List<MyTuple<Vector3I, float>>, MyTuple<Vector3I, float>> clearCollectionToken = MyUtils.ReuseCollection<MyTuple<Vector3I, float>>(ref MySlimBlock.m_batchCache))
      {
        List<MyTuple<Vector3I, float>> collection = clearCollectionToken.Collection;
        foreach (KeyValuePair<MySlimBlock, float> block in blocks)
        {
          MySlimBlock key = block.Key;
          if (cubeGrid.EntityId == key.CubeGrid.EntityId && !key.IsDestroyed)
            collection.Add(MyTuple.Create<Vector3I, float>(key.Position, block.Value));
        }
        MyMultiplayer.RaiseStaticEvent<long, List<MyTuple<Vector3I, float>>, MyStringHash, long>((Func<IMyEventOwner, Action<long, List<MyTuple<Vector3I, float>>, MyStringHash, long>>) (s => new Action<long, List<MyTuple<Vector3I, float>>, MyStringHash, long>(MySlimBlock.DoDamageSlimBlockBatch)), cubeGrid.EntityId, collection, damageType, attackerId);
      }
    }

    [Event(null, 2912)]
    [Reliable]
    [Broadcast]
    private static void DoDamageSlimBlockBatch(
      long gridId,
      List<MyTuple<Vector3I, float>> blocks,
      MyStringHash damageType,
      long attackerId)
    {
      MyCubeGrid entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(gridId, out entity))
        return;
      foreach (MyTuple<Vector3I, float> block in blocks)
      {
        MySlimBlock cubeBlock = entity.GetCubeBlock(block.Item1);
        if (cubeBlock == null || cubeBlock.IsDestroyed)
          break;
        float damage = block.Item2;
        cubeBlock.DoDamage(damage, damageType, attackerId: attackerId);
      }
    }

    public static void SendDamage(
      MySlimBlock block,
      float damage,
      MyStringHash damageType,
      MyHitInfo? hitInfo,
      long attackerId)
    {
      MyMultiplayer.RaiseStaticEvent<MySlimBlock.DoDamageSlimBlockMsg>((Func<IMyEventOwner, Action<MySlimBlock.DoDamageSlimBlockMsg>>) (s => new Action<MySlimBlock.DoDamageSlimBlockMsg>(MySlimBlock.DoDamageSlimBlock)), new MySlimBlock.DoDamageSlimBlockMsg()
      {
        GridEntityId = block.CubeGrid.EntityId,
        Position = block.Position,
        Damage = damage,
        HitInfo = hitInfo,
        AttackerEntityId = attackerId,
        CompoundBlockId = uint.MaxValue,
        Type = damageType
      });
    }

    [Event(null, 2956)]
    [Reliable]
    [Broadcast]
    private static void DoDamageSlimBlock(MySlimBlock.DoDamageSlimBlockMsg msg)
    {
      MyCubeGrid entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(msg.GridEntityId, out entity))
        return;
      MySlimBlock mySlimBlock = entity.GetCubeBlock(msg.Position);
      if (mySlimBlock == null || mySlimBlock.IsDestroyed)
        return;
      if (msg.CompoundBlockId != uint.MaxValue && mySlimBlock.FatBlock is MyCompoundCubeBlock)
      {
        MySlimBlock block = (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlock((ushort) msg.CompoundBlockId);
        if (block != null)
          mySlimBlock = block;
      }
      mySlimBlock.DoDamage(msg.Damage, msg.Type, msg.HitInfo, attackerId: msg.AttackerEntityId);
    }

    public void RemoveAuthorship()
    {
      int pcu = this.BlockDefinition.PCU;
      Interlocked.Add(ref MySession.Static.TotalSessionPCU, -pcu);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.m_builtByID);
      if (identity == null)
        return;
      if (!this.ComponentStack.IsFunctional)
        pcu = MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      identity.BlockLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
      MySession.Static.SessionBlockLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
    }

    public void AddAuthorship()
    {
      int pcu = this.BlockDefinition.PCU;
      Interlocked.Add(ref MySession.Static.TotalSessionPCU, pcu);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.m_builtByID);
      if (identity != null)
      {
        if (!this.ComponentStack.IsFunctional)
          pcu = MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
        identity.BlockLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
        MySession.Static.SessionBlockLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
      }
      else
      {
        if (this.m_builtByID == 0L)
          return;
        this.m_builtByID = 0L;
      }
    }

    public void TransferAuthorship(long newOwner)
    {
      if (this.m_builtByID == newOwner)
        return;
      MyIdentity identity1 = MySession.Static.Players.TryGetIdentity(this.m_builtByID);
      MyIdentity identity2 = MySession.Static.Players.TryGetIdentity(newOwner);
      int pcu = MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      if (this.ComponentStack.IsFunctional)
        pcu = this.BlockDefinition.PCU;
      identity1?.BlockLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
      this.m_builtByID = newOwner;
      identity2?.BlockLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
    }

    public void TransferAuthorshipClient(long newOwner) => this.m_builtByID = newOwner;

    public void TransferLimits(MyBlockLimits oldLimits, MyBlockLimits newLimits)
    {
      int pcu = MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
      if (this.ComponentStack.IsFunctional)
        pcu = this.BlockDefinition.PCU;
      oldLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
      newLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid);
    }

    internal void CleanUp()
    {
      if (this.ComponentStack == null)
        return;
      this.m_isFunctionalChanged = (Action<MySlimBlock>) null;
    }

    VRage.Game.ModAPI.IMyCubeBlock VRage.Game.ModAPI.IMySlimBlock.FatBlock => (VRage.Game.ModAPI.IMyCubeBlock) this.FatBlock;

    VRage.Game.ModAPI.Ingame.IMyCubeBlock VRage.Game.ModAPI.Ingame.IMySlimBlock.FatBlock => (VRage.Game.ModAPI.Ingame.IMyCubeBlock) this.FatBlock;

    void VRage.Game.ModAPI.IMySlimBlock.AddNeighbours() => this.AddNeighbours();

    public void GetNeighbours(ICollection<VRage.Game.ModAPI.IMySlimBlock> collection)
    {
      foreach (MySlimBlock neighbour in this.Neighbours)
        collection.Add((VRage.Game.ModAPI.IMySlimBlock) neighbour);
    }

    void VRage.Game.ModAPI.IMySlimBlock.ApplyAccumulatedDamage(bool addDirtyParts) => this.ApplyAccumulatedDamage(addDirtyParts, 0L);

    string VRage.Game.ModAPI.IMySlimBlock.CalculateCurrentModel(out Matrix orientation) => this.CalculateCurrentModel(out orientation);

    void VRage.Game.ModAPI.IMySlimBlock.ComputeScaledCenter(out Vector3D scaledCenter) => this.ComputeScaledCenter(out scaledCenter);

    void VRage.Game.ModAPI.IMySlimBlock.ComputeScaledHalfExtents(
      out Vector3 scaledHalfExtents)
    {
      this.ComputeScaledHalfExtents(out scaledHalfExtents);
    }

    void VRage.Game.ModAPI.IMySlimBlock.ComputeWorldCenter(out Vector3D worldCenter) => this.ComputeWorldCenter(out worldCenter);

    void VRage.Game.ModAPI.IMySlimBlock.FixBones(
      float oldDamage,
      float maxAllowedBoneMovement)
    {
      this.FixBones(oldDamage, maxAllowedBoneMovement);
    }

    void VRage.Game.ModAPI.IMySlimBlock.FullyDismount(VRage.Game.ModAPI.IMyInventory outputInventory) => this.FullyDismount(outputInventory as MyInventory);

    MyObjectBuilder_CubeBlock VRage.Game.ModAPI.IMySlimBlock.GetCopyObjectBuilder() => this.GetCopyObjectBuilder();

    MyObjectBuilder_CubeBlock VRage.Game.ModAPI.IMySlimBlock.GetObjectBuilder(
      bool copy)
    {
      return this.GetObjectBuilder(copy);
    }

    void VRage.Game.ModAPI.IMySlimBlock.InitOrientation(
      ref Vector3I forward,
      ref Vector3I up)
    {
      this.InitOrientation(ref forward, ref up);
    }

    void VRage.Game.ModAPI.IMySlimBlock.InitOrientation(
      Base6Directions.Direction Forward,
      Base6Directions.Direction Up)
    {
      this.InitOrientation(Forward, Up);
    }

    void VRage.Game.ModAPI.IMySlimBlock.InitOrientation(MyBlockOrientation orientation) => this.InitOrientation(orientation);

    void VRage.Game.ModAPI.IMySlimBlock.RemoveNeighbours() => this.RemoveNeighbours();

    void VRage.Game.ModAPI.IMySlimBlock.SetToConstructionSite() => this.SetToConstructionSite();

    void VRage.Game.ModAPI.IMySlimBlock.SpawnConstructionStockpile() => this.SpawnConstructionStockpile();

    void VRage.Game.ModAPI.IMySlimBlock.MoveItemsFromConstructionStockpile(
      VRage.Game.ModAPI.IMyInventory toInventory,
      MyItemFlags flags)
    {
      this.MoveItemsFromConstructionStockpile((MyInventoryBase) (toInventory as MyInventory), flags);
    }

    void VRage.Game.ModAPI.IMySlimBlock.SpawnFirstItemInConstructionStockpile() => this.SpawnFirstItemInConstructionStockpile();

    void VRage.Game.ModAPI.IMySlimBlock.UpdateVisual() => this.UpdateVisual(true);

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.AccumulatedDamage => this.AccumulatedDamage;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.BuildIntegrity => this.BuildIntegrity;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.BuildLevelRatio => this.BuildLevelRatio;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.CurrentDamage => this.CurrentDamage;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.DamageRatio => this.DamageRatio;

    void VRage.Game.ModAPI.Ingame.IMySlimBlock.GetMissingComponents(
      Dictionary<string, int> addToDictionary)
    {
      this.GetMissingComponents(addToDictionary);
    }

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.HasDeformation => this.HasDeformation;

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.IsDestroyed => this.IsDestroyed;

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.IsFullIntegrity => this.IsFullIntegrity;

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.IsFullyDismounted => this.IsFullyDismounted;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.MaxDeformation => this.MaxDeformation;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.MaxIntegrity => this.MaxIntegrity;

    float VRage.Game.ModAPI.Ingame.IMySlimBlock.Mass => this.GetMass();

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.ShowParts => this.ShowParts;

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.StockpileAllocated => this.StockpileAllocated;

    bool VRage.Game.ModAPI.Ingame.IMySlimBlock.StockpileEmpty => this.StockpileEmpty;

    Vector3I VRage.Game.ModAPI.Ingame.IMySlimBlock.Position => this.Position;

    VRage.Game.ModAPI.Ingame.IMyCubeGrid VRage.Game.ModAPI.Ingame.IMySlimBlock.CubeGrid => (VRage.Game.ModAPI.Ingame.IMyCubeGrid) this.CubeGrid;

    Vector3 VRage.Game.ModAPI.Ingame.IMySlimBlock.ColorMaskHSV => this.ColorMaskHSV;

    MyStringHash VRage.Game.ModAPI.Ingame.IMySlimBlock.SkinSubtypeId => this.SkinSubtypeId;

    VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMySlimBlock.CubeGrid => (VRage.Game.ModAPI.IMyCubeGrid) this.CubeGrid;

    MyDefinitionBase VRage.Game.ModAPI.IMySlimBlock.BlockDefinition => (MyDefinitionBase) this.BlockDefinition;

    Vector3I VRage.Game.ModAPI.IMySlimBlock.Max => this.Max;

    Vector3I VRage.Game.ModAPI.IMySlimBlock.Min => this.Min;

    MyBlockOrientation VRage.Game.ModAPI.IMySlimBlock.Orientation => this.Orientation;

    List<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMySlimBlock.Neighbours => this.Neighbours.Cast<VRage.Game.ModAPI.IMySlimBlock>().ToList<VRage.Game.ModAPI.IMySlimBlock>();

    Vector3 VRage.Game.ModAPI.IMySlimBlock.GetColorMask() => this.ColorMaskHSV;

    void VRage.Game.ModAPI.IMySlimBlock.DecreaseMountLevel(
      float grinderAmount,
      VRage.Game.ModAPI.IMyInventory outputInventory,
      bool useDefaultDeconstructEfficiency)
    {
      this.DecreaseMountLevel(grinderAmount, outputInventory as MyInventoryBase, useDefaultDeconstructEfficiency);
    }

    void VRage.Game.ModAPI.IMySlimBlock.IncreaseMountLevel(
      float welderMountAmount,
      long welderOwnerPlayerId,
      VRage.Game.ModAPI.IMyInventory outputInventory,
      float maxAllowedBoneMovement,
      bool isHelping,
      MyOwnershipShareModeEnum share)
    {
      this.IncreaseMountLevel(welderMountAmount, welderOwnerPlayerId, outputInventory as MyInventoryBase, maxAllowedBoneMovement, isHelping, share);
    }

    int VRage.Game.ModAPI.IMySlimBlock.GetConstructionStockpileItemAmount(
      MyDefinitionId id)
    {
      return this.GetConstructionStockpileItemAmount(id);
    }

    void VRage.Game.ModAPI.IMySlimBlock.MoveItemsToConstructionStockpile(
      VRage.Game.ModAPI.IMyInventory fromInventory)
    {
      this.MoveItemsToConstructionStockpile(fromInventory as MyInventoryBase);
    }

    void VRage.Game.ModAPI.IMySlimBlock.ClearConstructionStockpile(
      VRage.Game.ModAPI.IMyInventory outputInventory)
    {
      this.ClearConstructionStockpile(outputInventory as MyInventoryBase);
    }

    bool VRage.Game.ModAPI.IMySlimBlock.CanContinueBuild(VRage.Game.ModAPI.IMyInventory sourceInventory) => this.CanContinueBuild((MyInventoryBase) (sourceInventory as MyInventory));

    void VRage.Game.ModAPI.IMySlimBlock.GetWorldBoundingBox(
      out BoundingBoxD aabb,
      bool useAABBFromBlockCubes)
    {
      this.GetWorldBoundingBox(out aabb, useAABBFromBlockCubes);
    }

    float VRage.Game.ModAPI.IMySlimBlock.Dithering
    {
      get => this.Dithering;
      set
      {
        this.Dithering = value;
        this.UpdateVisual(false);
      }
    }

    long VRage.Game.ModAPI.Ingame.IMySlimBlock.OwnerId => this.OwnerId;

    SerializableDefinitionId VRage.Game.ModAPI.Ingame.IMySlimBlock.BlockDefinition => (SerializableDefinitionId) this.BlockDefinition.Id;

    long VRage.Game.ModAPI.IMySlimBlock.BuiltBy => this.m_builtByID;

    [ProtoContract]
    public struct DoDamageSlimBlockMsg
    {
      [ProtoMember(1)]
      public long GridEntityId;
      [ProtoMember(4)]
      public Vector3I Position;
      [ProtoMember(7)]
      public float Damage;
      [ProtoMember(10)]
      public MyStringHash Type;
      [ProtoMember(13)]
      public MyHitInfo? HitInfo;
      [ProtoMember(16)]
      public long AttackerEntityId;
      [ProtoMember(19)]
      public uint CompoundBlockId;

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EGridEntityId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in long value) => owner.GridEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out long value) => value = owner.GridEntityId;
      }

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in Vector3I value) => owner.Position = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out Vector3I value) => value = owner.Position;
      }

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EDamage\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in float value) => owner.Damage = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out float value) => value = owner.Damage;
      }

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in MyStringHash value) => owner.Type = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out MyStringHash value) => value = owner.Type;
      }

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EHitInfo\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, MyHitInfo?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in MyHitInfo? value) => owner.HitInfo = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out MyHitInfo? value) => value = owner.HitInfo;
      }

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EAttackerEntityId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in long value) => owner.AttackerEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out long value) => value = owner.AttackerEntityId;
      }

      protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003ECompoundBlockId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock.DoDamageSlimBlockMsg, uint>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySlimBlock.DoDamageSlimBlockMsg owner, in uint value) => owner.CompoundBlockId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySlimBlock.DoDamageSlimBlockMsg owner, out uint value) => value = owner.CompoundBlockId;
      }

      private class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg\u003C\u003EActor : IActivator, IActivator<MySlimBlock.DoDamageSlimBlockMsg>
      {
        object IActivator.CreateInstance() => (object) new MySlimBlock.DoDamageSlimBlockMsg();

        MySlimBlock.DoDamageSlimBlockMsg IActivator<MySlimBlock.DoDamageSlimBlockMsg>.CreateInstance() => new MySlimBlock.DoDamageSlimBlockMsg();
      }
    }

    protected sealed class DoDamageSlimBlockBatch\u003C\u003ESystem_Int64\u0023System_Collections_Generic_List`1\u003CVRage_MyTuple`2\u003CVRageMath_Vector3I\u0023System_Single\u003E\u003E\u0023VRage_Utils_MyStringHash\u0023System_Int64 : ICallSite<IMyEventOwner, long, List<MyTuple<Vector3I, float>>, MyStringHash, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridId,
        in List<MyTuple<Vector3I, float>> blocks,
        in MyStringHash damageType,
        in long attackerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySlimBlock.DoDamageSlimBlockBatch(gridId, blocks, damageType, attackerId);
      }
    }

    protected sealed class DoDamageSlimBlock\u003C\u003ESandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDoDamageSlimBlockMsg : ICallSite<IMyEventOwner, MySlimBlock.DoDamageSlimBlockMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MySlimBlock.DoDamageSlimBlockMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySlimBlock.DoDamageSlimBlock(msg);
      }
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_accumulatedDamage\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.m_accumulatedDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.m_accumulatedDamage;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EBlockDefinition\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyCubeBlockDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyCubeBlockDefinition value) => owner.BlockDefinition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyCubeBlockDefinition value) => value = owner.BlockDefinition;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in Vector3I value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out Vector3I value) => value = owner.Min;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in Vector3I value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out Vector3I value) => value = owner.Max;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EOrientation\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyBlockOrientation value) => owner.Orientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyBlockOrientation value) => value = owner.Orientation;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in Vector3I value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out Vector3I value) => value = owner.Position;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.BlockGeneralDamageModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.BlockGeneralDamageModifier;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_cubeGrid\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyCubeGrid>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyCubeGrid value) => owner.m_cubeGrid = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyCubeGrid value) => value = owner.m_cubeGrid;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_colorMaskHSV\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in Vector3 value) => owner.m_colorMaskHSV = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out Vector3 value) => value = owner.m_colorMaskHSV;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyStringHash value) => owner.SkinSubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyStringHash value) => value = owner.SkinSubtypeId;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDithering\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.Dithering = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.Dithering;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EUsesDeformation\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in bool value) => owner.UsesDeformation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out bool value) => value = owner.UsesDeformation;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_deformationRatio\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.m_deformationRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.m_deformationRatio;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_componentStack\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyComponentStack>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyComponentStack value) => owner.m_componentStack = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyComponentStack value) => value = owner.m_componentStack;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_stockpile\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyConstructionStockpile value) => owner.m_stockpile = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyConstructionStockpile value) => value = owner.m_stockpile;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_cachedMaxDeformation\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.m_cachedMaxDeformation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.m_cachedMaxDeformation;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_builtByID\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in long value) => owner.m_builtByID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out long value) => value = owner.m_builtByID;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003ENeighbours\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, List<MySlimBlock>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in List<MySlimBlock> value) => owner.Neighbours = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out List<MySlimBlock> value) => value = owner.Neighbours;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_lastDamage\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.m_lastDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.m_lastDamage;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_lastAttackerId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in long value) => owner.m_lastAttackerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out long value) => value = owner.m_lastAttackerId;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_lastDamageType\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyStringHash value) => owner.m_lastDamageType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyStringHash value) => value = owner.m_lastDamageType;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003Em_isFunctionalChanged\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, Action<MySlimBlock>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in Action<MySlimBlock> value) => owner.m_isFunctionalChanged = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out Action<MySlimBlock> value) => value = owner.m_isFunctionalChanged;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyMultiBlockDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyMultiBlockDefinition value) => owner.MultiBlockDefinition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyMultiBlockDefinition value) => value = owner.MultiBlockDefinition;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in int value) => owner.MultiBlockId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out int value) => value = owner.MultiBlockId;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in int value) => owner.MultiBlockIndex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out int value) => value = owner.MultiBlockIndex;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDisconnectFaces\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, List<Vector3I>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in List<Vector3I> value) => owner.DisconnectFaces = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out List<Vector3I> value) => value = owner.DisconnectFaces;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EAccumulatedDamage\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.AccumulatedDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.AccumulatedDamage;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EFatBlock\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyCubeBlock>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyCubeBlock value) => owner.FatBlock = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyCubeBlock value) => value = owner.FatBlock;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003ECubeGrid\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, MyCubeGrid>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in MyCubeGrid value) => owner.CubeGrid = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out MyCubeGrid value) => value = owner.CubeGrid;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in Vector3 value) => owner.ColorMaskHSV = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out Vector3 value) => value = owner.ColorMaskHSV;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.DeformationRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.DeformationRatio;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EShowParts\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in bool value) => owner.ShowParts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out bool value) => value = owner.ShowParts;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EUseDamageSystem\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in bool value) => owner.UseDamageSystem = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out bool value) => value = owner.UseDamageSystem;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EUniqueId\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in int value) => owner.UniqueId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out int value) => value = owner.UniqueId;
    }

    protected class Sandbox_Game_Entities_Cube_MySlimBlock\u003C\u003EVRage\u002EGame\u002EModAPI\u002EIMySlimBlock\u002EDithering\u003C\u003EAccessor : IMemberAccessor<MySlimBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySlimBlock owner, in float value) => owner.VRage\u002EGame\u002EModAPI\u002EIMySlimBlock\u002EDithering = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySlimBlock owner, out float value) => value = owner.VRage\u002EGame\u002EModAPI\u002EIMySlimBlock\u002EDithering;
    }
  }
}
