// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.ParticleEffects;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents;
using VRage.Game.Entity.UseObject;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  public class MyCubeBlock : MyEntity, IMyComponentOwner<MyIDModule>, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.IMyUpgradableBlock, Sandbox.ModAPI.Ingame.IMyUpgradableBlock
  {
    protected static readonly string DUMMY_SUBBLOCK_ID = "subblock_";
    private static List<MyCubeBlockDefinition.MountPoint> m_tmpMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
    private static List<MyCubeBlockDefinition.MountPoint> m_tmpBlockMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
    private static List<MyCubeBlockDefinition.MountPoint> m_tmpOtherBlockMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
    protected static MyCubeBlock.EmissiveNames m_emissiveNames = new MyCubeBlock.EmissiveNames(true);
    public Dictionary<long, MyCubeBlock.AttachedUpgradeModule> CurrentAttachedUpgradeModules;
    private MyResourceSinkComponent m_sinkComp;
    public bool IsBeingRemoved;
    protected List<MyCubeBlockEffect> m_activeEffects;
    private bool? m_setDamagedEffectDelayed = new bool?(false);
    private bool m_checkConnectionAllowed;
    private int m_numberInGrid;
    public MySlimBlock SlimBlock;
    public bool IsSilenced;
    public bool SilenceInChange;
    public bool UsedUpdateEveryFrame;
    private MyIDModule m_IDModule;
    protected Dictionary<string, MySlimBlock> SubBlocks;
    private List<MyObjectBuilder_CubeBlock.MySubBlockId> m_loadedSubBlocks;
    private static MyCubeBlock.MethodDataIsConnectedTo m_methodDataIsConnectedTo = new MyCubeBlock.MethodDataIsConnectedTo();
    protected bool m_forceBlockDestructible;
    private MyParticleEffect m_damageEffect;
    private bool m_wasUpdatedEachFrame;
    private MyUpgradableBlockComponent m_upgradeComponent;
    private Dictionary<string, float> m_upgradeValues;
    private MyStringHash m_skinSubtypeId;

    public bool IsBeingHacked => this is MyTerminalBlock myTerminalBlock && myTerminalBlock.IsBeingHacked;

    public bool UsesEmissivePreset { get; private set; }

    protected static bool AllowExperimentalValues => MySession.Static.IsRunningExperimental;

    public virtual MyCubeBlockHighlightModes HighlightMode => MyCubeBlockHighlightModes.Default;

    public MyPhysicsBody Physics
    {
      get => base.Physics as MyPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public long OwnerId => this.IDModule != null ? this.IDModule.Owner : 0L;

    public long BuiltBy => this.SlimBlock != null ? this.SlimBlock.BuiltBy : 0L;

    public MyResourceSinkComponent ResourceSink
    {
      get => this.m_sinkComp;
      protected set
      {
        if (this.ContainsDebugRenderComponent(typeof (MyDebugRenderComponentDrawPowerReciever)))
          this.RemoveDebugRenderComponent(typeof (MyDebugRenderComponentDrawPowerReciever));
        if (this.Components.Contains(typeof (MyResourceSinkComponent)))
          this.Components.Remove<MyResourceSinkComponent>();
        this.Components.Add<MyResourceSinkComponent>(value);
        this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawPowerReciever(value, (VRage.ModAPI.IMyEntity) this));
        this.m_sinkComp = value;
      }
    }

    public string GetOwnerFactionTag()
    {
      if (this.IDModule == null || this.IDModule.Owner == 0L)
        return "";
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(this.IDModule.Owner);
      return playerFaction == null ? "" : playerFaction.Tag;
    }

    public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(
      long identityId)
    {
      return !MyFakes.SHOW_FACTIONS_GUI || this.IDModule == null ? MyRelationsBetweenPlayerAndBlock.NoOwnership : this.IDModule.GetUserRelationToOwner(identityId);
    }

    public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
    {
      if (!MyFakes.SHOW_FACTIONS_GUI || this.IDModule == null)
        return MyRelationsBetweenPlayerAndBlock.NoOwnership;
      return MySession.Static.LocalHumanPlayer != null ? this.IDModule.GetUserRelationToOwner(MySession.Static.LocalHumanPlayer.Identity.IdentityId) : MyRelationsBetweenPlayerAndBlock.Neutral;
    }

    public bool FriendlyWithBlock(MyCubeBlock block) => this.GetUserRelationToOwner(block.OwnerId) != MyRelationsBetweenPlayerAndBlock.Enemies && block.GetUserRelationToOwner(this.OwnerId) != MyRelationsBetweenPlayerAndBlock.Enemies;

    public MyCubeBlockDefinition BlockDefinition => this.SlimBlock.BlockDefinition;

    public Vector3I Min => this.SlimBlock.Min;

    public Vector3I Max => this.SlimBlock.Max;

    public MyBlockOrientation Orientation => this.SlimBlock.Orientation;

    public Vector3I Position => this.SlimBlock.Position;

    public MyCubeGrid CubeGrid => this.SlimBlock.CubeGrid;

    public MyUseObjectsComponentBase UseObjectsComponent => this.Components.Get<MyUseObjectsComponentBase>();

    public bool CheckConnectionAllowed
    {
      get => this.m_checkConnectionAllowed;
      set
      {
        this.m_checkConnectionAllowed = value;
        Action<MyCubeBlock> connectionChanged = this.CheckConnectionChanged;
        if (connectionChanged == null)
          return;
        connectionChanged(this);
      }
    }

    public event Action<MyCubeBlock> CheckConnectionChanged;

    public int NumberInGrid
    {
      get => this.m_numberInGrid;
      set
      {
        this.m_numberInGrid = value;
        if (this.m_numberInGrid > 1)
          this.DisplayNameText = this.BlockDefinition.DisplayNameText + " " + (object) this.m_numberInGrid;
        else
          this.DisplayNameText = this.BlockDefinition.DisplayNameText;
      }
    }

    public bool IsFunctional => this.SlimBlock.ComponentStack.IsFunctional;

    public bool IsBuilt => this.SlimBlock.ComponentStack.IsBuilt;

    public virtual float DisassembleRatio => this.BlockDefinition.DisassembleRatio;

    public bool IsWorking { get; private set; }

    public void UpdateIsWorking()
    {
      bool flag1 = this.CheckIsWorking();
      bool flag2 = flag1 != this.IsWorking;
      this.IsWorking = flag1;
      if (flag2 && this.IsWorkingChanged != null)
        this.IsWorkingChanged(this);
      if (!(this.UsesEmissivePreset & flag2))
        return;
      this.CheckEmissiveState();
    }

    protected virtual bool CheckIsWorking() => this.IsFunctional;

    public event Action<MyCubeBlock> IsWorkingChanged;

    public event Func<bool> CanContinueBuildCheck;

    public bool CanContinueBuild()
    {
      if (this.CanContinueBuildCheck == null)
        return true;
      bool flag = true;
      foreach (Delegate invocation in this.CanContinueBuildCheck.GetInvocationList())
      {
        Func<bool> func = invocation as Func<bool>;
        flag &= func();
      }
      return flag;
    }

    public MyIDModule IDModule => this.m_IDModule;

    public bool IsSubBlock => this.SubBlockName != null;

    public string SubBlockName { get; internal set; }

    public MySlimBlock OwnerBlock { get; internal set; }

    public IMyUseObject GetInteractiveObject(uint shapeKey) => !this.IsFunctional ? (IMyUseObject) null : this.UseObjectsComponent.GetInteractiveObject(shapeKey);

    public void ReleaseInventory(MyInventory inventory, bool damageContent = false)
    {
      if (inventory == null || !Sync.IsServer)
        return;
      MyEntityInventorySpawnComponent component = (MyEntityInventorySpawnComponent) null;
      if (this.Components.TryGet<MyEntityInventorySpawnComponent>(out component))
      {
        component.SpawnInventoryContainer();
        this.Components.Add<MyInventoryBase>((MyInventoryBase) new MyInventory(inventory.MaxVolume, inventory.MaxMass, Vector3.One, inventory.GetFlags()));
      }
      else
      {
        foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
        {
          MyPhysicalInventoryItem inventoryItem = physicalInventoryItem;
          if (damageContent && physicalInventoryItem.Content.TypeId == typeof (MyObjectBuilder_Component))
          {
            inventoryItem.Amount *= (MyFixedPoint) MyDefinitionManager.Static.GetComponentDefinition(physicalInventoryItem.Content.GetId()).DropProbability;
            inventoryItem.Amount = MyFixedPoint.Floor(inventoryItem.Amount);
            if (inventoryItem.Amount == (MyFixedPoint) 0)
              continue;
          }
          MyFloatingObjects.EnqueueInventoryItemSpawn(inventoryItem, this.PositionComp.WorldAABB, (Vector3D) (this.CubeGrid.Physics != null ? this.CubeGrid.Physics.GetVelocityAtPoint(this.PositionComp.GetPosition()) : Vector3.Zero));
        }
        inventory.Clear(true);
      }
    }

    protected virtual void OnConstraintAdded(GridLinkTypeEnum type, VRage.ModAPI.IMyEntity attachedEntity)
    {
      if (!(attachedEntity is MyCubeGrid myCubeGrid) || MyCubeGridGroups.Static.GetGroups(type).LinkExists(this.EntityId, this.CubeGrid, myCubeGrid))
        return;
      MyCubeGridGroups.Static.CreateLink(type, this.EntityId, this.CubeGrid, myCubeGrid);
    }

    protected virtual void OnConstraintRemoved(GridLinkTypeEnum type, VRage.ModAPI.IMyEntity detachedEntity)
    {
      if (!(detachedEntity is MyCubeGrid child))
        return;
      MyCubeGridGroups.Static.BreakLink(type, this.EntityId, this.CubeGrid, child);
    }

    public string DefinitionDisplayNameText => this.BlockDefinition.DisplayNameText;

    public bool ForceBlockDestructible => MyFakes.ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE && this.m_forceBlockDestructible;

    public MyCubeBlock()
    {
      this.Render.ShadowBoxLod = true;
      this.NeedsWorldMatrix = false;
      this.InvalidateOnMove = false;
    }

    public override void InitComponents()
    {
      if (this.Render == null)
        this.Render = (MyRenderComponentBase) new MyRenderComponentCubeBlock();
      if (this.PositionComp == null)
        this.PositionComp = (MyPositionComponentBase) new MyCubeBlock.MyBlockPosComponent();
      base.InitComponents();
    }

    public void Init()
    {
      this.PositionComp.LocalAABB = new BoundingBox(new Vector3((float) (-(double) this.SlimBlock.CubeGrid.GridSize / 2.0)), new Vector3(this.SlimBlock.CubeGrid.GridSize / 2f));
      this.Components.Add<MyUseObjectsComponentBase>((MyUseObjectsComponentBase) new MyUseObjectsComponent());
      if (this.BlockDefinition.CubeDefinition != null)
        this.SlimBlock.Orientation = MyCubeGridDefinitions.GetTopologyUniqueOrientation(this.BlockDefinition.CubeDefinition.CubeTopology, this.Orientation);
      Matrix localMatrix;
      string currModel;
      this.CalcLocalMatrix(out localMatrix, out currModel);
      if (!string.IsNullOrEmpty(currModel))
      {
        this.Init((StringBuilder) null, currModel, (MyEntity) null, new float?());
        this.OnModelChange();
      }
      this.Render.EnableColorMaskHsv = true;
      this.Render.FadeIn = this.CubeGrid.Render.FadeIn;
      this.Render.SkipIfTooSmall = false;
      this.CheckConnectionAllowed = false;
      this.PositionComp.SetLocalMatrix(ref localMatrix, (object) this.CubeGrid);
      this.Save = false;
      if (this.CubeGrid.CreatePhysics)
        this.UseObjectsComponent.LoadDetectorsFromModel();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      if (this.Subparts == null || this.Subparts.Count <= 0)
        return;
      bool flag = false;
      foreach (MyEntitySubpart myEntitySubpart in this.Subparts.Values)
      {
        if (!(myEntitySubpart.Render is MyParentedSubpartRenderComponent) && myEntitySubpart.InvalidateOnMove || myEntitySubpart.NeedsWorldMatrix)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      this.NeedsWorldMatrix = true;
    }

    public void GetLocalMatrix(out Matrix localMatrix) => this.SlimBlock.GetLocalMatrix(out localMatrix);

    public void CalcLocalMatrix(out Matrix localMatrix, out string currModel)
    {
      this.GetLocalMatrix(out localMatrix);
      Matrix orientation;
      currModel = this.SlimBlock.CalculateCurrentModel(out orientation);
      orientation.Translation = localMatrix.Translation;
      localMatrix = orientation;
    }

    public virtual void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      if (builder.EntityId == 0L)
        this.EntityId = MyEntityIdentifier.AllocateId();
      else if (builder.EntityId != 0L)
        this.EntityId = builder.EntityId;
      if (string.IsNullOrEmpty(builder.Name))
        this.Name = this.EntityId.ToString();
      else
        this.Name = builder.Name;
      this.NumberInGrid = cubeGrid.BlockCounter.GetNextNumber(builder.GetId());
      this.Render.ColorMaskHsv = (Vector3) builder.ColorMaskHSV;
      this.UpdateSkin();
      this.Render.FadeIn = cubeGrid.Render.FadeIn;
      if (MyFakes.ENABLE_SUBBLOCKS && (this.BlockDefinition.SubBlockDefinitions == null ? 0 : (this.BlockDefinition.SubBlockDefinitions.Count > 0 ? 1 : 0)) != 0)
      {
        if (builder.SubBlocks != null && builder.SubBlocks.Length != 0)
        {
          this.m_loadedSubBlocks = new List<MyObjectBuilder_CubeBlock.MySubBlockId>();
          foreach (MyObjectBuilder_CubeBlock.MySubBlockId subBlock in builder.SubBlocks)
            this.m_loadedSubBlocks.Add(subBlock);
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }
        else if (Sync.IsServer)
        {
          this.m_loadedSubBlocks = new List<MyObjectBuilder_CubeBlock.MySubBlockId>();
          this.SpawnSubBlocks();
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }
      }
      this.UsesEmissivePreset = this.BlockDefinition.EmissiveColorPreset != MyStringHash.NullOrEmpty && MyEmissiveColorPresets.ContainsPreset(this.BlockDefinition.EmissiveColorPreset);
      this.Components.InitComponents(builder.TypeId, builder.SubtypeId, builder.ComponentContainer);
      this.Init((MyObjectBuilder_EntityBase) null);
      this.Render.PersistentFlags |= MyPersistentEntityFlags2.CastShadows;
      this.Init();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentCubeBlock(this));
      this.InitOwnership(builder);
    }

    private void InitOwnership(MyObjectBuilder_CubeBlock builder)
    {
      MyEntityOwnershipComponent ownershipComponent = this.Components.Get<MyEntityOwnershipComponent>();
      bool flag = this.BlockDefinition.ContainsComputer();
      if (this.UseObjectsComponent != null)
        flag = flag || this.UseObjectsComponent.GetDetectors("ownership").Count > 0;
      if (flag)
      {
        this.m_IDModule = new MyIDModule();
        if ((!MySession.Static.Settings.ResetOwnership ? 0 : (Sync.IsServer ? 1 : 0)) != 0)
        {
          this.m_IDModule.Owner = 0L;
          this.m_IDModule.ShareMode = MyOwnershipShareModeEnum.None;
        }
        else
        {
          if (builder.ShareMode == ~MyOwnershipShareModeEnum.None)
            builder.ShareMode = MyOwnershipShareModeEnum.None;
          MyEntityIdentifier.ID_OBJECT_TYPE idObjectType = MyEntityIdentifier.GetIdObjectType(builder.Owner);
          if (builder.Owner != 0L && idObjectType != MyEntityIdentifier.ID_OBJECT_TYPE.NPC && (idObjectType != MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP && !Sync.Players.HasIdentity(builder.Owner)))
            builder.Owner = 0L;
          this.m_IDModule.Owner = builder.Owner;
          this.m_IDModule.ShareMode = builder.ShareMode;
        }
      }
      if (ownershipComponent == null || builder.Owner == 0L)
        return;
      ownershipComponent.OwnerId = builder.Owner;
      ownershipComponent.ShareMode = MyOwnershipShareModeEnum.None;
    }

    public override sealed MyObjectBuilder_EntityBase GetObjectBuilder(
      bool copy = false)
    {
      return base.GetObjectBuilder(copy);
    }

    public virtual MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CubeBlock objectBuilder = MyCubeBlockFactory.CreateObjectBuilder(this);
      objectBuilder.ColorMaskHSV = (SerializableVector3) this.Render.ColorMaskHsv;
      objectBuilder.SkinSubtypeId = this.m_skinSubtypeId.String;
      objectBuilder.EntityId = this.EntityId;
      objectBuilder.Min = (SerializableVector3I) this.Min;
      objectBuilder.Owner = 0L;
      objectBuilder.ShareMode = MyOwnershipShareModeEnum.None;
      objectBuilder.Name = this.Name;
      if (this.m_IDModule != null)
      {
        objectBuilder.Owner = this.m_IDModule.Owner;
        objectBuilder.ShareMode = this.m_IDModule.ShareMode;
      }
      if (MyFakes.ENABLE_SUBBLOCKS && this.SubBlocks != null && this.SubBlocks.Count != 0)
      {
        objectBuilder.SubBlocks = new MyObjectBuilder_CubeBlock.MySubBlockId[this.SubBlocks.Count];
        int index = 0;
        foreach (KeyValuePair<string, MySlimBlock> subBlock in this.SubBlocks)
        {
          objectBuilder.SubBlocks[index].SubGridId = subBlock.Value.CubeGrid.EntityId;
          objectBuilder.SubBlocks[index].SubGridName = subBlock.Key;
          objectBuilder.SubBlocks[index].SubBlockPosition = (SerializableVector3I) subBlock.Value.Min;
          ++index;
        }
      }
      objectBuilder.ComponentContainer = this.Components.Serialize(copy);
      if (copy)
        objectBuilder.Name = (string) null;
      return objectBuilder;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateIsWorking();
      if (this.UsesEmissivePreset)
        this.CheckEmissiveState();
      foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in this.BlockDefinition.BuildProgressModels)
        MyRenderProxy.PreloadModel(buildProgressModel.File);
      if (!MyFakes.SHOW_DAMAGE_EFFECTS || this.CubeGrid.Physics == null || (this.SlimBlock == null || this.BlockDefinition.RatioEnoughForDamageEffect(this.SlimBlock.BuildIntegrity / this.SlimBlock.MaxIntegrity)) || !this.BlockDefinition.RatioEnoughForDamageEffect(this.SlimBlock.Integrity / this.SlimBlock.MaxIntegrity))
        return;
      this.SetDamageEffect(true);
    }

    public override void OnRemovedFromScene(object source)
    {
      this.StopDamageEffect();
      base.OnRemovedFromScene(source);
    }

    public virtual bool ConnectionAllowed(
      ref Vector3I otherBlockPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      if (!MyFakes.ENABLE_FRACTURE_COMPONENT || !this.Components.Has<MyFractureComponentBase>())
        return true;
      MyFractureComponentCubeBlock fractureComponent1 = this.GetFractureComponent();
      if (fractureComponent1 == null || fractureComponent1.MountPoints == null)
        return true;
      MyCubeBlock.m_tmpBlockMountPoints.Clear();
      MyCubeGrid.TransformMountPoints(MyCubeBlock.m_tmpBlockMountPoints, this.BlockDefinition, fractureComponent1.MountPoints, ref this.SlimBlock.Orientation);
      MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(otherBlockPos);
      if (cubeBlock == null)
        return true;
      Vector3I position = this.Position;
      MyCubeBlock.m_tmpMountPoints.Clear();
      if (cubeBlock.FatBlock is MyCompoundCubeBlock)
      {
        foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
        {
          MyFractureComponentCubeBlock fractureComponent2 = block.GetFractureComponent();
          MyCubeBlockDefinition.MountPoint[] mountPoints = fractureComponent2 == null ? block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio) : fractureComponent2.MountPoints;
          MyCubeBlock.m_tmpOtherBlockMountPoints.Clear();
          MyCubeGrid.TransformMountPoints(MyCubeBlock.m_tmpOtherBlockMountPoints, block.BlockDefinition, mountPoints, ref block.Orientation);
          MyCubeBlock.m_tmpMountPoints.AddRange((IEnumerable<MyCubeBlockDefinition.MountPoint>) MyCubeBlock.m_tmpOtherBlockMountPoints);
        }
      }
      else
      {
        MyFractureComponentCubeBlock fractureComponent2 = cubeBlock.GetFractureComponent();
        MyCubeBlockDefinition.MountPoint[] mountPoints = fractureComponent2 == null ? def.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio) : fractureComponent2.MountPoints;
        MyCubeGrid.TransformMountPoints(MyCubeBlock.m_tmpMountPoints, def, mountPoints, ref cubeBlock.Orientation);
      }
      int num = MyCubeGrid.CheckMountPointsForSide(MyCubeBlock.m_tmpBlockMountPoints, ref this.SlimBlock.Orientation, ref position, this.BlockDefinition.Id, ref faceNormal, MyCubeBlock.m_tmpMountPoints, ref cubeBlock.Orientation, ref otherBlockPos, def.Id) ? 1 : 0;
      MyCubeBlock.m_tmpMountPoints.Clear();
      MyCubeBlock.m_tmpBlockMountPoints.Clear();
      MyCubeBlock.m_tmpOtherBlockMountPoints.Clear();
      return num != 0;
    }

    public virtual bool ConnectionAllowed(
      ref Vector3I otherBlockMinPos,
      ref Vector3I otherBlockMaxPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      Vector3I next = otherBlockMinPos;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref otherBlockMinPos, ref otherBlockMaxPos);
      while (vector3IRangeIterator.IsValid())
      {
        if (this.ConnectionAllowed(ref next, ref faceNormal, def))
          return true;
        vector3IRangeIterator.GetNext(out next);
      }
      return false;
    }

    protected virtual void WorldPositionChanged(object source)
    {
    }

    protected override void Closing()
    {
      if (this.UseObjectsComponent.DetectorPhysics != null)
        this.UseObjectsComponent.ClearPhysics();
      if (MyFakes.ENABLE_SUBBLOCKS && this.SubBlocks != null)
      {
        foreach (KeyValuePair<string, MySlimBlock> subBlock in this.SubBlocks)
        {
          MySlimBlock mySlimBlock = subBlock.Value;
          if (mySlimBlock.FatBlock != null)
          {
            mySlimBlock.FatBlock.OwnerBlock = (MySlimBlock) null;
            mySlimBlock.FatBlock.SubBlockName = (string) null;
            mySlimBlock.FatBlock.OnClosing -= new Action<MyEntity>(this.SubBlock_OnClosing);
          }
        }
      }
      this.SetDamageEffect(false);
      if (this.SlimBlock != null)
        this.SlimBlock.CleanUp();
      base.Closing();
    }

    public virtual bool SetEmissiveStateWorking() => this.Render != null && this.Render.RenderObjectIDs.Length != 0 && this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);

    public virtual bool SetEmissiveStateDisabled() => this.Render != null && this.Render.RenderObjectIDs.Length != 0 && this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);

    public virtual bool SetEmissiveStateDamaged() => this.Render != null && this.Render.RenderObjectIDs.Length != 0 && this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Damaged, this.Render.RenderObjectIDs[0]);

    public virtual void CheckEmissiveState(bool force = false)
    {
      if (this.IsWorking)
        this.SetEmissiveStateWorking();
      else if (this.IsFunctional)
        this.SetEmissiveStateDisabled();
      else
        this.SetEmissiveStateDamaged();
    }

    public bool SetEmissiveState(MyStringHash state, uint renderObjectId, string namedPart = null)
    {
      MyEmissiveColorStateResult result;
      if (renderObjectId == uint.MaxValue || !MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, state, out result))
        return false;
      if (string.IsNullOrEmpty(namedPart))
      {
        byte index = 0;
        while (true)
        {
          string defaultEmissiveParts = this.GetDefaultEmissiveParts(index);
          if (!string.IsNullOrEmpty(defaultEmissiveParts))
          {
            MyEntity.UpdateNamedEmissiveParts(renderObjectId, defaultEmissiveParts, result.EmissiveColor, result.Emissivity);
            ++index;
          }
          else
            break;
        }
      }
      else
        MyEntity.UpdateNamedEmissiveParts(renderObjectId, namedPart, result.EmissiveColor, result.Emissivity);
      return true;
    }

    public static void UpdateEmissiveParts(
      uint renderObjectId,
      float emissivity,
      Color emissivePartColor,
      Color displayPartColor)
    {
      if (renderObjectId == uint.MaxValue)
        return;
      MyEntity.UpdateNamedEmissiveParts(renderObjectId, "Emissive", emissivePartColor, emissivity);
      MyEntity.UpdateNamedEmissiveParts(renderObjectId, "Display", displayPartColor, emissivity);
    }

    protected virtual string GetDefaultEmissiveParts(byte index)
    {
      if (index == (byte) 0)
        return "Emissive";
      return index == (byte) 1 ? "Display" : (string) null;
    }

    private bool UpdateSkin()
    {
      int num = this.m_skinSubtypeId != this.SlimBlock.SkinSubtypeId ? 1 : 0;
      if (num != 0)
      {
        this.m_skinSubtypeId = this.SlimBlock.SkinSubtypeId;
        MyDefinitionManager.MyAssetModifiers myAssetModifiers = new MyDefinitionManager.MyAssetModifiers();
        if (this.m_skinSubtypeId != MyStringHash.NullOrEmpty)
          myAssetModifiers = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(this.m_skinSubtypeId);
        this.Render.TextureChanges = myAssetModifiers.SkinTextureChanges;
        this.Render.MetalnessColorable = myAssetModifiers.MetalnessColorable;
      }
      MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), this.m_skinSubtypeId));
      if (modifierDefinition == null)
        return num != 0;
      if (!modifierDefinition.DefaultColor.HasValue)
        return num != 0;
      this.SlimBlock.ColorMaskHSV = modifierDefinition.DefaultColor.Value.ColorToHSVDX11();
      this.Render.ColorMaskHsv = this.SlimBlock.ColorMaskHSV;
      return num != 0;
    }

    public virtual void UpdateVisual()
    {
      bool flag1 = this.UpdateSkin();
      Matrix orientation;
      string currentModel = this.SlimBlock.CalculateCurrentModel(out orientation);
      bool flag2 = this.Model != null && this.Model.AssetName != currentModel;
      if (((flag2 ? 1 : (this.Render.ColorMaskHsv != this.SlimBlock.ColorMaskHSV ? 1 : 0)) | (flag1 ? 1 : 0)) == 0 && (double) this.Render.Transparency == (double) this.SlimBlock.Dithering)
        return;
      this.Render.ColorMaskHsv = this.SlimBlock.ColorMaskHSV;
      this.m_skinSubtypeId = this.SlimBlock.SkinSubtypeId;
      MyDefinitionManager.MyAssetModifiers myAssetModifiers = new MyDefinitionManager.MyAssetModifiers();
      if (this.m_skinSubtypeId != MyStringHash.NullOrEmpty)
        myAssetModifiers = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(this.m_skinSubtypeId);
      this.Render.TextureChanges = myAssetModifiers.SkinTextureChanges;
      this.Render.MetalnessColorable = myAssetModifiers.MetalnessColorable;
      this.Render.Transparency = this.SlimBlock.Dithering;
      Vector3D translation = this.WorldMatrix.Translation;
      MatrixD worldMatrix = orientation * this.CubeGrid.WorldMatrix;
      worldMatrix.Translation = translation;
      this.PositionComp.SetWorldMatrix(ref worldMatrix, forceUpdate: true);
      this.RefreshModels(currentModel, (string) null);
      this.Render.RemoveRenderObjects();
      this.Render.AddRenderObjects();
      if (this.CubeGrid.CreatePhysics & flag2)
        this.UseObjectsComponent.LoadDetectorsFromModel();
      this.OnModelChange();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (MyFakes.ENABLE_SUBBLOCKS && this.m_loadedSubBlocks != null)
        this.InitSubBlocks();
      if (!this.m_setDamagedEffectDelayed.HasValue)
        return;
      this.SetDamageEffect(this.m_setDamagedEffectDelayed.Value);
      this.m_setDamagedEffectDelayed = new bool?();
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (!MyFakes.ENABLE_SUBBLOCKS || this.m_loadedSubBlocks == null)
        return;
      this.InitSubBlocks();
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.m_activeEffects == null || !MyPerGameSettings.UseNewDamageEffects)
        return;
      for (int index = 0; index < this.m_activeEffects.Count; ++index)
      {
        if (this.m_activeEffects[index].CanBeDeleted)
        {
          this.m_activeEffects[index].Stop();
          this.m_activeEffects.RemoveAt(index);
          --index;
        }
        else
          this.m_activeEffects[index].Update();
      }
    }

    public virtual void OnBuildSuccess(long builtBy, bool instantBuild)
    {
    }

    public virtual void OnRemovedByCubeBuilder()
    {
      this.SetFadeOut(false);
      if (MyFakes.ENABLE_SUBBLOCKS && this.SubBlocks != null)
      {
        foreach (KeyValuePair<string, MySlimBlock> subBlock in this.SubBlocks)
        {
          MySlimBlock block = subBlock.Value;
          block.CubeGrid.RemoveBlock(block, true);
        }
      }
      this.SetDamageEffect(false);
    }

    public virtual void OnRegisteredToGridSystems()
    {
      if (this.m_upgradeComponent == null)
        return;
      this.m_upgradeComponent.Refresh(this);
    }

    public virtual void OnUnregisteredFromGridSystems()
    {
    }

    public virtual void ContactPointCallback(ref MyGridContactInfo value)
    {
    }

    public virtual void OnDestroy() => this.SetDamageEffect(false);

    public virtual void OnModelChange()
    {
      if (!this.UsesEmissivePreset)
        return;
      this.CheckEmissiveState(true);
    }

    public virtual string CalculateCurrentModel(out Matrix orientation)
    {
      this.Orientation.GetMatrix(out orientation);
      return this.BlockDefinition.Model;
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.UpdateIsWorking();
      this.CubeGrid.UpdateOwnership(this.OwnerId, this.IsFunctional);
      if (this.UsesEmissivePreset)
        this.CheckEmissiveState();
      if (MyVisualScriptLogicProvider.BlockFunctionalityChanged == null)
        return;
      MyVisualScriptLogicProvider.BlockFunctionalityChanged(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name, this.SlimBlock.BlockDefinition.Id.TypeId.ToString(), this.SlimBlock.BlockDefinition.Id.SubtypeName, this.IsFunctional);
    }

    internal virtual void OnIntegrityChanged(
      float buildIntegrity,
      float integrity,
      bool setOwnership,
      long owner,
      MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction)
    {
      if (!this.BlockDefinition.ContainsComputer())
        return;
      MyEntityOwnershipComponent ownershipComponent = this.Components.Get<MyEntityOwnershipComponent>();
      if (setOwnership)
      {
        if (this.m_IDModule.Owner == 0L && Sync.IsServer)
          this.CubeGrid.ChangeOwnerRequest(this.CubeGrid, this, owner, sharing);
        if (ownershipComponent == null || ownershipComponent.OwnerId != 0L || !Sync.IsServer)
          return;
        this.CubeGrid.ChangeOwnerRequest(this.CubeGrid, this, owner, sharing);
      }
      else
      {
        if (this.m_IDModule.Owner != 0L && Sync.IsServer)
        {
          sharing = MyOwnershipShareModeEnum.None;
          this.CubeGrid.ChangeOwnerRequest(this.CubeGrid, this, 0L, sharing);
        }
        if (ownershipComponent == null || ownershipComponent.OwnerId == 0L || !Sync.IsServer)
          return;
        sharing = MyOwnershipShareModeEnum.None;
        this.CubeGrid.ChangeOwnerRequest(this.CubeGrid, this, 0L, sharing);
      }
    }

    public void ChangeBlockOwnerRequest(long playerId, MyOwnershipShareModeEnum shareMode) => this.CubeGrid.ChangeOwnerRequest(this.CubeGrid, this, playerId, shareMode);

    public bool SetEffect(string effectName, bool stopPrevious = false) => this.SetEffect(effectName, 0.0f, stopPrevious, true, false);

    public bool SetEffect(
      string effectName,
      float parameter,
      bool stopPrevious = false,
      bool ignoreParameter = false,
      bool removeSameNameEffects = false)
    {
      if (this.BlockDefinition == null || this.BlockDefinition.Effects == null)
        return false;
      int index1 = -1;
      for (int index2 = 0; index2 < this.BlockDefinition.Effects.Length; ++index2)
      {
        if (effectName.Equals(this.BlockDefinition.Effects[index2].Name) && (ignoreParameter || (double) parameter >= (double) this.BlockDefinition.Effects[index2].ParameterMin && (double) parameter <= (double) this.BlockDefinition.Effects[index2].ParameterMax))
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
        return false;
      if (this.m_activeEffects == null)
        this.m_activeEffects = new List<MyCubeBlockEffect>();
      for (int index2 = 0; index2 < this.m_activeEffects.Count; ++index2)
      {
        if (this.m_activeEffects[index2].EffectId == index1)
        {
          if (!stopPrevious)
            return false;
          this.m_activeEffects[index2].Stop();
          this.m_activeEffects.RemoveAt(index2);
          break;
        }
      }
      if (removeSameNameEffects)
        this.RemoveEffect(effectName, index1);
      if (this.m_activeEffects.Count == 0)
      {
        this.m_wasUpdatedEachFrame = (uint) (this.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) > 0U;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      }
      this.m_activeEffects.Add(new MyCubeBlockEffect(index1, this.BlockDefinition.Effects[index1], (MyEntity) this));
      return true;
    }

    public int RemoveEffect(string effectName, int exception = -1)
    {
      if (this.BlockDefinition == null || this.BlockDefinition.Effects == null || this.m_activeEffects == null)
        return 0;
      int num = 0;
      for (int index1 = 0; index1 < this.BlockDefinition.Effects.Length; ++index1)
      {
        if (effectName.Equals(this.BlockDefinition.Effects[index1].Name))
        {
          for (int index2 = 0; index2 < this.m_activeEffects.Count; ++index2)
          {
            if (this.m_activeEffects[index2].EffectId == index1 && index1 != exception)
            {
              this.m_activeEffects[index2].Stop();
              this.m_activeEffects.RemoveAt(index2);
              ++num;
            }
          }
        }
      }
      if (this.m_activeEffects.Count == 0 && !this.m_wasUpdatedEachFrame)
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      return num;
    }

    protected bool HasDamageEffect => this.m_damageEffect != null;

    public virtual void SetDamageEffectDelayed(bool show)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_setDamagedEffectDelayed = new bool?(true);
    }

    public virtual void SetDamageEffect(bool show)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if (MyPerGameSettings.UseNewDamageEffects & show)
        this.SetEffect("Damage", this.SlimBlock.Integrity / this.SlimBlock.MaxIntegrity, false, false, true);
      bool flag = this.m_activeEffects != null && MyPerGameSettings.UseNewDamageEffects && this.m_activeEffects.Count > 0;
      if (MyPerGameSettings.UseNewDamageEffects && !show)
        this.RemoveEffect("Damage", -1);
      if (!MyFakes.SHOW_DAMAGE_EFFECTS || string.IsNullOrEmpty(this.BlockDefinition.DamageEffectName) && !this.BlockDefinition.DamageEffectID.HasValue)
        return;
      if (!show && this.m_damageEffect != null)
      {
        this.m_damageEffect.Stop(false);
        this.m_damageEffect.StopLights();
        if (this.CubeGrid.Physics != null)
          this.m_damageEffect.Velocity = this.CubeGrid.Physics.LinearVelocity;
        this.m_damageEffect.OnDelete -= new Action<MyParticleEffect>(this.OnDamageEffectDeleted);
        this.m_damageEffect = (MyParticleEffect) null;
      }
      if (!show || this.m_damageEffect != null || (flag || !MySandboxGame.Static.EnableDamageEffects))
        return;
      string effectName = this.BlockDefinition.DamageEffectName;
      if (string.IsNullOrEmpty(effectName) && this.BlockDefinition.DamageEffectID.HasValue)
      {
        MyParticleEffectData particleEffectData;
        MyParticleEffectsLibrary.GetById().TryGetValue(this.BlockDefinition.DamageEffectID.Value, out particleEffectData);
        if (particleEffectData != null)
          effectName = particleEffectData.Name;
      }
      MatrixD damageLocalMatrix = this.GetDamageLocalMatrix();
      Vector3D translation = this.PositionComp.WorldMatrixRef.Translation;
      if (!MyParticlesManager.TryCreateParticleEffect(effectName, ref damageLocalMatrix, ref translation, this.Render.ParentIDs[0], out this.m_damageEffect))
        return;
      this.m_damageEffect.UserScale = this.Model.BoundingBox.Perimeter * 0.018f;
      this.m_damageEffect.OnDelete += new Action<MyParticleEffect>(this.OnDamageEffectDeleted);
    }

    public virtual void StopDamageEffect(bool stopSound = true)
    {
      if (MyPerGameSettings.UseNewDamageEffects)
        this.RemoveEffect("Damage", -1);
      if (!MyFakes.SHOW_DAMAGE_EFFECTS || string.IsNullOrEmpty(this.BlockDefinition.DamageEffectName) && !this.BlockDefinition.DamageEffectID.HasValue || this.m_damageEffect == null)
        return;
      this.m_damageEffect.StopEmitting(10f);
      this.m_damageEffect.StopLights();
      if (this.CubeGrid.Physics != null)
        this.m_damageEffect.Velocity = this.CubeGrid.Physics.LinearVelocity;
      this.m_damageEffect.OnDelete -= new Action<MyParticleEffect>(this.OnDamageEffectDeleted);
      this.m_damageEffect = (MyParticleEffect) null;
    }

    private MatrixD GetDamageWorldMatrix() => MatrixD.CreateTranslation(0.85f * this.PositionComp.LocalVolume.Center) * this.WorldMatrix;

    private MatrixD GetDamageLocalMatrix()
    {
      MatrixD translation = MatrixD.CreateTranslation(0.85f * this.PositionComp.LocalVolume.Center);
      return this.PositionComp == null ? translation : translation * this.PositionComp.LocalMatrixRef;
    }

    private void OnDamageEffectDeleted(MyParticleEffect effect)
    {
      if (effect != this.m_damageEffect)
        return;
      this.SetDamageEffect(false);
    }

    public void ChangeOwner(long owner, MyOwnershipShareModeEnum shareMode)
    {
      MyEntityOwnershipComponent ownershipComponent = this.Components.Get<MyEntityOwnershipComponent>();
      if (ownershipComponent != null)
      {
        if ((owner != ownershipComponent.OwnerId ? 1 : (shareMode != ownershipComponent.ShareMode ? 1 : 0)) == 0)
          return;
        long ownerId = ownershipComponent.OwnerId;
        ownershipComponent.OwnerId = owner;
        ownershipComponent.ShareMode = shareMode;
        if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
          this.CubeGrid.ChangeOwner(this, ownerId, owner);
        this.OnOwnershipChanged();
      }
      else
      {
        if (this.IDModule == null || (owner != this.m_IDModule.Owner ? 1 : (shareMode != this.m_IDModule.ShareMode ? 1 : 0)) == 0)
          return;
        long owner1 = this.m_IDModule.Owner;
        this.m_IDModule.Owner = owner;
        this.m_IDModule.ShareMode = shareMode;
        if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
          this.CubeGrid.ChangeOwner(this, owner1, owner);
        this.OnOwnershipChanged();
      }
    }

    protected virtual void OnOwnershipChanged()
    {
    }

    bool IMyComponentOwner<MyIDModule>.GetComponent(
      out MyIDModule component)
    {
      component = this.m_IDModule;
      return this.m_IDModule != null && this.IsFunctional;
    }

    public virtual void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      if (!MyFakes.ENABLE_FRACTURE_COMPONENT || !this.Components.Has<MyFractureComponentBase>())
        return;
      this.GetFractureComponent()?.OnCubeGridChanged();
    }

    public virtual void OnTeleport()
    {
    }

    internal virtual void OnAddedNeighbours()
    {
    }

    internal virtual void OnRemovedNeighbours()
    {
    }

    internal virtual void OnTransformed(ref MatrixI transform)
    {
    }

    internal virtual void UpdateWorldMatrix()
    {
      Matrix localMatrix;
      this.GetLocalMatrix(out localMatrix);
      MatrixD worldMatrix = (MatrixD) ref localMatrix;
      this.PositionComp.SetWorldMatrix(ref worldMatrix, forceUpdate: true);
    }

    private void InitSubBlocks()
    {
      if (!MyFakes.ENABLE_SUBBLOCKS || this.m_loadedSubBlocks == null)
        return;
      bool flag = this.AllSubBlocksInitialized();
      bool spawned = ((this.m_loadedSubBlocks.Count != 0 ? 0 : (Sync.IsServer ? 1 : 0)) & (flag ? 1 : 0)) != 0;
      if (!flag)
      {
        for (int index = this.m_loadedSubBlocks.Count - 1; index >= 0; --index)
        {
          MyObjectBuilder_CubeBlock.MySubBlockId loadedSubBlock = this.m_loadedSubBlocks[index];
          MyEntity entity;
          if (MyEntities.TryGetEntityById(loadedSubBlock.SubGridId, out entity))
          {
            if (entity is MyCubeGrid myCubeGrid)
            {
              MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock((Vector3I) loadedSubBlock.SubBlockPosition);
              if (cubeBlock != null)
                this.AddSubBlock(loadedSubBlock.SubGridName, cubeBlock);
            }
            this.m_loadedSubBlocks.RemoveAt(index);
          }
        }
      }
      if (!this.AllSubBlocksInitialized())
        return;
      this.m_loadedSubBlocks = (List<MyObjectBuilder_CubeBlock.MySubBlockId>) null;
      if (!spawned && flag)
        return;
      this.SubBlocksInitialized(spawned);
    }

    protected bool AllSubBlocksInitialized()
    {
      if ((this.BlockDefinition.SubBlockDefinitions == null ? 0 : ((uint) this.BlockDefinition.SubBlockDefinitions.Count > 0U ? 1 : 0)) == 0 || this.SubBlocks == null || this.SubBlocks.Count == 0)
        return false;
      return this.SubBlocks.Count == this.BlockDefinition.SubBlockDefinitions.Count || this.m_loadedSubBlocks == null || this.m_loadedSubBlocks.Count == 0;
    }

    protected void AddSubBlock(string dummyName, MySlimBlock subblock)
    {
      if (this.SubBlocks == null)
        this.SubBlocks = new Dictionary<string, MySlimBlock>();
      MySlimBlock mySlimBlock;
      if (this.SubBlocks.TryGetValue(dummyName, out mySlimBlock))
      {
        if (subblock == mySlimBlock)
          return;
        this.RemoveSubBlock(dummyName, false);
      }
      this.SubBlocks.Add(dummyName, subblock);
      subblock.FatBlock.SubBlockName = dummyName;
      subblock.FatBlock.OwnerBlock = this.SlimBlock;
      subblock.FatBlock.OnClosing += new Action<MyEntity>(this.SubBlock_OnClosing);
    }

    private void SpawnSubBlocks()
    {
      if (!MyFakes.ENABLE_SUBBLOCKS || !this.CubeGrid.CreatePhysics)
        return;
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        MyCubeBlockDefinition subBlockDefinition;
        MatrixD subBlockMatrix;
        if (MyCubeBlock.GetSubBlockDataFromDummy(this.BlockDefinition, dummy.Key, dummy.Value, true, out subBlockDefinition, out subBlockMatrix, out Vector3 _))
        {
          string dummyName = dummy.Key.Substring(MyCubeBlock.DUMMY_SUBBLOCK_ID.Length);
          Matrix localMatrix;
          this.GetLocalMatrix(out localMatrix);
          MatrixD matrixD = subBlockMatrix * localMatrix * this.CubeGrid.WorldMatrix;
          Matrix matrix = (Matrix) ref matrixD;
          MyCubeGrid myCubeGrid = MyCubeBuilder.SpawnDynamicGrid(subBlockDefinition, (MyEntity) null, (MatrixD) ref matrix, new Vector3(0.0f, -1f, 0.0f), MyStringHash.NullOrEmpty);
          if (myCubeGrid != null)
          {
            MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(Vector3I.Zero);
            if (cubeBlock != null && cubeBlock.FatBlock != null)
              this.AddSubBlock(dummyName, cubeBlock);
          }
        }
      }
    }

    protected virtual void SubBlocksInitialized(bool spawned)
    {
    }

    protected virtual void OnSubBlockClosing(MySlimBlock subBlock)
    {
      subBlock.FatBlock.OnClosing -= new Action<MyEntity>(this.SubBlock_OnClosing);
      if (this.SubBlocks == null)
        return;
      this.SubBlocks.Remove(subBlock.FatBlock.SubBlockName);
    }

    private void SubBlock_OnClosing(MyEntity obj)
    {
      MyCubeBlock subblock = obj as MyCubeBlock;
      if (subblock == null)
        return;
      KeyValuePair<string, MySlimBlock> keyValuePair = this.SubBlocks.FirstOrDefault<KeyValuePair<string, MySlimBlock>>((Func<KeyValuePair<string, MySlimBlock>, bool>) (p => p.Value == subblock.SlimBlock));
      if (keyValuePair.Value == null)
        return;
      this.OnSubBlockClosing(keyValuePair.Value);
    }

    protected bool RemoveSubBlock(string subBlockName, bool removeFromGrid = true)
    {
      MySlimBlock block;
      if (this.SubBlocks == null || !this.SubBlocks.TryGetValue(subBlockName, out block))
        return false;
      if (removeFromGrid)
        block.CubeGrid.RemoveBlock(block, true);
      if (!this.SubBlocks.Remove(subBlockName))
        return false;
      if (block.FatBlock != null)
      {
        block.FatBlock.OwnerBlock = (MySlimBlock) null;
        block.FatBlock.SubBlockName = (string) null;
      }
      return true;
    }

    public static Vector3 GetBlockGridOffset(MyCubeBlockDefinition blockDefinition)
    {
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize);
      Vector3 zero = Vector3.Zero;
      if (blockDefinition.Size.X % 2 == 0)
        zero.X = cubeSize / 2f;
      if (blockDefinition.Size.Y % 2 == 0)
        zero.Y = cubeSize / 2f;
      if (blockDefinition.Size.Z % 2 == 0)
        zero.Z = cubeSize / 2f;
      return zero;
    }

    public static bool GetSubBlockDataFromDummy(
      MyCubeBlockDefinition ownerBlockDefinition,
      string dummyName,
      MyModelDummy dummy,
      bool useOffset,
      out MyCubeBlockDefinition subBlockDefinition,
      out MatrixD subBlockMatrix,
      out Vector3 dummyPosition)
    {
      subBlockDefinition = (MyCubeBlockDefinition) null;
      subBlockMatrix = MatrixD.Identity;
      dummyPosition = Vector3.Zero;
      if (!dummyName.ToLower().StartsWith(MyCubeBlock.DUMMY_SUBBLOCK_ID) || ownerBlockDefinition.SubBlockDefinitions == null)
        return false;
      string key = dummyName.Substring(MyCubeBlock.DUMMY_SUBBLOCK_ID.Length);
      MyDefinitionId defId;
      if (!ownerBlockDefinition.SubBlockDefinitions.TryGetValue(key, out defId))
        return false;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out subBlockDefinition);
      if (subBlockDefinition == null)
        return false;
      subBlockMatrix = MatrixD.Normalize((MatrixD) ref dummy.Matrix);
      Vector3I intVector1 = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection((Vector3) subBlockMatrix.Forward));
      if (Math.Abs(1.0 - Vector3D.Dot(subBlockMatrix.Forward, (Vector3D) intVector1)) <= 1E-08)
        subBlockMatrix.Forward = (Vector3D) intVector1;
      Vector3I intVector2 = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection((Vector3) subBlockMatrix.Right));
      if (Math.Abs(1.0 - Vector3D.Dot(subBlockMatrix.Right, (Vector3D) intVector2)) <= 1E-08)
        subBlockMatrix.Right = (Vector3D) intVector2;
      Vector3I intVector3 = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection((Vector3) subBlockMatrix.Up));
      if (Math.Abs(1.0 - Vector3D.Dot(subBlockMatrix.Up, (Vector3D) intVector3)) <= 1E-08)
        subBlockMatrix.Up = (Vector3D) intVector3;
      dummyPosition = (Vector3) subBlockMatrix.Translation;
      if (useOffset)
      {
        Vector3 blockGridOffset = MyCubeBlock.GetBlockGridOffset(subBlockDefinition);
        subBlockMatrix.Translation -= Vector3D.TransformNormal(blockGridOffset, subBlockMatrix);
      }
      return true;
    }

    public virtual float GetMass() => MyDestructionData.Static != null ? MyDestructionData.Static.GetBlockMass(this.SlimBlock.CalculateCurrentModel(out Matrix _), this.BlockDefinition) : this.BlockDefinition.Mass;

    public virtual BoundingBox GetGeometryLocalBox() => this.Model != null ? this.Model.BoundingBox : new BoundingBox(new Vector3((float) (-(double) this.CubeGrid.GridSize / 2.0)), new Vector3(this.CubeGrid.GridSize / 2f));

    public DictionaryReader<string, MySlimBlock> GetSubBlocks() => new DictionaryReader<string, MySlimBlock>(this.SubBlocks);

    public bool TryGetSubBlock(string name, out MySlimBlock block)
    {
      if (this.SubBlocks != null)
        return this.SubBlocks.TryGetValue(name, out block);
      block = (MySlimBlock) null;
      return false;
    }

    public MyUpgradableBlockComponent GetComponent()
    {
      if (this.m_upgradeComponent == null)
        this.m_upgradeComponent = new MyUpgradableBlockComponent(this);
      return this.m_upgradeComponent;
    }

    public Dictionary<string, float> UpgradeValues
    {
      get
      {
        if (this.m_upgradeValues == null)
          this.m_upgradeValues = new Dictionary<string, float>();
        return this.m_upgradeValues;
      }
    }

    public void AddUpgradeValue(string name, float defaultValue)
    {
      float num;
      if (this.UpgradeValues.TryGetValue(name, out num))
      {
        if ((double) num == (double) defaultValue)
          return;
        MyLog.Default.WriteLine("ERROR while adding upgraded block " + this.DisplayNameText.ToString() + ". Duplicate with different default value found!");
      }
      else
        this.UpgradeValues.Add(name, defaultValue);
    }

    public event Action OnUpgradeValuesChanged;

    public void CommitUpgradeValues()
    {
      Action upgradeValuesChanged = this.OnUpgradeValuesChanged;
      if (upgradeValuesChanged == null)
        return;
      upgradeValuesChanged();
    }

    public virtual void CreateRenderer(
      MyPersistentEntityFlags2 persistentFlags,
      Vector3 colorMaskHsv,
      object modelStorage)
    {
      this.m_skinSubtypeId = MyStringHash.NullOrEmpty;
      this.Render = (MyRenderComponentBase) new MyRenderComponentCubeBlock();
      this.Render.ColorMaskHsv = colorMaskHsv;
      this.Render.ShadowBoxLod = true;
      this.Render.EnableColorMaskHsv = true;
      this.Render.SkipIfTooSmall = false;
      this.Render.PersistentFlags |= persistentFlags | MyPersistentEntityFlags2.CastShadows;
      this.Render.ModelStorage = modelStorage;
      this.Render.FadeIn = this.CubeGrid.Render.FadeIn;
      this.UpdateSkin();
    }

    public MyFractureComponentCubeBlock GetFractureComponent()
    {
      MyFractureComponentCubeBlock componentCubeBlock = (MyFractureComponentCubeBlock) null;
      if (MyFakes.ENABLE_FRACTURE_COMPONENT)
        componentCubeBlock = this.Components.Get<MyFractureComponentBase>() as MyFractureComponentCubeBlock;
      return componentCubeBlock;
    }

    public override void RefreshModels(string modelPath, string modelCollisionPath)
    {
      MyModels.GetModelOnlyData(modelPath)?.Rescale(this.CubeGrid.GridScale);
      if (modelCollisionPath != null)
        MyModels.GetModelOnlyData(modelCollisionPath)?.Rescale(this.CubeGrid.GridScale);
      base.RefreshModels(modelPath, modelCollisionPath);
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      this.CubeGrid.RegisterInventory(this);
      if (inventory == null)
        return;
      inventory.ContentsChanged += new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      this.CubeGrid.UnregisterInventory(this);
      if (inventory == null)
        return;
      inventory.ContentsChanged -= new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
    }

    private void Inventory_ContentsChanged(MyInventoryBase obj)
    {
      this.CubeGrid.SetInventoryMassDirty();
      this.CubeGrid.RaiseInventoryChanged(obj);
    }

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      t = new MyIntersectionResultLineTriangleEx?();
      if (this.ModelCollision == null || this.BlockDefinition == null)
        return false;
      Matrix result1;
      this.Orientation.GetMatrix(out result1);
      Vector3 result2;
      Vector3.TransformNormal(ref this.BlockDefinition.ModelOffset, ref result1, out result2);
      result1.Translation = this.Position * this.CubeGrid.GridSize + result2;
      MatrixD customInvMatrix1 = MatrixD.Invert(this.WorldMatrix);
      t = this.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, ref customInvMatrix1, flags);
      if (!t.HasValue && this.Subparts != null)
      {
        foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
        {
          if (subpart.Value != null && subpart.Value.ModelCollision != null)
          {
            MatrixD customInvMatrix2 = MatrixD.Invert(subpart.Value.WorldMatrix);
            t = subpart.Value.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, ref customInvMatrix2, flags);
            if (t.HasValue)
              break;
          }
        }
      }
      return t.HasValue;
    }

    public virtual void DisableUpdates() => this.NeedsUpdate &= ~(MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);

    protected static void ClampExperimentalValue(ref float? value, float maxSafeValue)
    {
      if (MyCubeBlock.AllowExperimentalValues)
        return;
      float? nullable = value;
      float num = maxSafeValue;
      if (!((double) nullable.GetValueOrDefault() > (double) num & nullable.HasValue))
        return;
      value = new float?(maxSafeValue);
    }

    public virtual bool ReceivedDamage(
      float damage,
      MyStringHash damageType,
      long attackerId,
      long realHitEntityId)
    {
      return true;
    }

    SerializableDefinitionId VRage.Game.ModAPI.Ingame.IMyCubeBlock.BlockDefinition => (SerializableDefinitionId) this.BlockDefinition.Id;

    public void Init(MyObjectBuilder_CubeBlock builder, VRage.Game.ModAPI.IMyCubeGrid cubeGrid)
    {
      if (!(cubeGrid is MyCubeGrid))
        return;
      this.Init(builder, cubeGrid as MyCubeGrid);
    }

    private Action<MyCubeBlock> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeBlock> value) => (Action<MyCubeBlock>) Delegate.CreateDelegate(typeof (Action<MyCubeBlock>), value.Target, value.Method);

    event Action<VRage.Game.ModAPI.IMyCubeBlock> VRage.Game.ModAPI.IMyCubeBlock.IsWorkingChanged
    {
      add => this.IsWorkingChanged += this.GetDelegate(value);
      remove => this.IsWorkingChanged -= this.GetDelegate(value);
    }

    VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeBlock.CubeGrid => (VRage.Game.ModAPI.IMyCubeGrid) this.CubeGrid;

    VRage.Game.ModAPI.Ingame.IMyCubeGrid VRage.Game.ModAPI.Ingame.IMyCubeBlock.CubeGrid => (VRage.Game.ModAPI.Ingame.IMyCubeGrid) this.CubeGrid;

    void VRage.Game.ModAPI.IMyCubeBlock.CalcLocalMatrix(
      out Matrix localMatrix,
      out string currModel)
    {
      this.CalcLocalMatrix(out localMatrix, out currModel);
    }

    string VRage.Game.ModAPI.IMyCubeBlock.CalculateCurrentModel(out Matrix orientation) => this.CalculateCurrentModel(out orientation);

    bool VRage.Game.ModAPI.IMyCubeBlock.CheckConnectionAllowed
    {
      get => this.CheckConnectionAllowed;
      set => this.CheckConnectionAllowed = value;
    }

    bool VRage.Game.ModAPI.IMyCubeBlock.DebugDraw()
    {
      this.DebugDraw();
      return true;
    }

    MyObjectBuilder_CubeBlock VRage.Game.ModAPI.IMyCubeBlock.GetObjectBuilderCubeBlock(
      bool copy)
    {
      return this.GetObjectBuilderCubeBlock(copy);
    }

    MyRelationsBetweenPlayerAndBlock VRage.Game.ModAPI.IMyCubeBlock.GetPlayerRelationToOwner() => this.GetPlayerRelationToOwner();

    MyRelationsBetweenPlayerAndBlock VRage.Game.ModAPI.IMyCubeBlock.GetUserRelationToOwner(
      long playerId)
    {
      return this.GetUserRelationToOwner(playerId);
    }

    void VRage.Game.ModAPI.IMyCubeBlock.Init() => this.Init();

    void VRage.Game.ModAPI.IMyCubeBlock.Init(
      MyObjectBuilder_CubeBlock builder,
      VRage.Game.ModAPI.IMyCubeGrid cubeGrid)
    {
      this.Init(builder, cubeGrid);
    }

    float VRage.Game.ModAPI.Ingame.IMyCubeBlock.Mass => this.GetMass();

    void VRage.Game.ModAPI.IMyCubeBlock.OnBuildSuccess(long builtBy) => this.OnBuildSuccess(builtBy, false);

    void VRage.Game.ModAPI.IMyCubeBlock.OnBuildSuccess(long builtBy, bool instantBuild) => this.OnBuildSuccess(builtBy, instantBuild);

    void VRage.Game.ModAPI.IMyCubeBlock.OnDestroy() => this.OnDestroy();

    void VRage.Game.ModAPI.IMyCubeBlock.OnModelChange() => this.OnModelChange();

    void VRage.Game.ModAPI.IMyCubeBlock.OnRegisteredToGridSystems() => this.OnRegisteredToGridSystems();

    void VRage.Game.ModAPI.IMyCubeBlock.OnRemovedByCubeBuilder() => this.OnRemovedByCubeBuilder();

    void VRage.Game.ModAPI.IMyCubeBlock.OnUnregisteredFromGridSystems() => this.OnUnregisteredFromGridSystems();

    string VRage.Game.ModAPI.IMyCubeBlock.RaycastDetectors(
      Vector3D worldFrom,
      Vector3D worldTo)
    {
      return this.Components.Get<MyUseObjectsComponentBase>().RaycastDetectors(worldFrom, worldTo);
    }

    void VRage.Game.ModAPI.IMyCubeBlock.ReloadDetectors(bool refreshNetworks) => this.Components.Get<MyUseObjectsComponentBase>().LoadDetectorsFromModel();

    void VRage.Game.ModAPI.IMyCubeBlock.UpdateIsWorking() => this.UpdateIsWorking();

    void VRage.Game.ModAPI.IMyCubeBlock.UpdateVisual() => this.UpdateVisual();

    void VRage.Game.ModAPI.IMyCubeBlock.SetDamageEffect(bool start) => this.SetDamageEffect(start);

    VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeBlock.SlimBlock => (VRage.Game.ModAPI.IMySlimBlock) this.SlimBlock;

    uint Sandbox.ModAPI.Ingame.IMyUpgradableBlock.UpgradeCount => (uint) this.UpgradeValues.Count;

    void Sandbox.ModAPI.Ingame.IMyUpgradableBlock.GetUpgrades(
      out Dictionary<string, float> upgrades)
    {
      upgrades = new Dictionary<string, float>();
      foreach (KeyValuePair<string, float> upgradeValue in this.UpgradeValues)
        upgrades.Add(upgradeValue.Key, upgradeValue.Value);
    }

    MyResourceSinkComponentBase VRage.Game.ModAPI.IMyCubeBlock.ResourceSink
    {
      get => (MyResourceSinkComponentBase) this.ResourceSink;
      set => this.ResourceSink = (MyResourceSinkComponent) value;
    }

    bool VRage.Game.ModAPI.IMyCubeBlock.SetEffect(string effectName, bool stopPrevious) => this.SetEffect(effectName, stopPrevious);

    bool VRage.Game.ModAPI.IMyCubeBlock.SetEffect(
      string effectName,
      float parameter,
      bool stopPrevious,
      bool ignoreParameter,
      bool removeSameNameEffects)
    {
      return this.SetEffect(effectName, parameter, stopPrevious, ignoreParameter, removeSameNameEffects);
    }

    int VRage.Game.ModAPI.IMyCubeBlock.RemoveEffect(string effectName, int exception) => this.RemoveEffect(effectName, exception);

    private class MethodDataIsConnectedTo
    {
      public List<MyCubeBlockDefinition.MountPoint> MyMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
      public List<MyCubeBlockDefinition.MountPoint> OtherMountPoints = new List<MyCubeBlockDefinition.MountPoint>();

      public void Clear()
      {
        this.MyMountPoints.Clear();
        this.OtherMountPoints.Clear();
      }
    }

    public class AttachedUpgradeModule
    {
      public Sandbox.ModAPI.IMyUpgradeModule Block;
      public int SlotCount = 1;
      public bool Compatible = true;

      public AttachedUpgradeModule(Sandbox.ModAPI.IMyUpgradeModule block) => this.Block = block;

      public AttachedUpgradeModule(Sandbox.ModAPI.IMyUpgradeModule block, int slotCount, bool compatible)
      {
        this.Block = block;
        this.SlotCount = slotCount;
        this.Compatible = compatible;
      }
    }

    public struct EmissiveNames
    {
      public MyStringHash Working;
      public MyStringHash Disabled;
      public MyStringHash Warning;
      public MyStringHash Damaged;
      public MyStringHash Alternative;
      public MyStringHash Locked;
      public MyStringHash Autolock;
      public MyStringHash Constraint;

      public EmissiveNames(bool ignore)
      {
        this.Working = MyStringHash.GetOrCompute(nameof (Working));
        this.Disabled = MyStringHash.GetOrCompute(nameof (Disabled));
        this.Damaged = MyStringHash.GetOrCompute(nameof (Damaged));
        this.Alternative = MyStringHash.GetOrCompute(nameof (Alternative));
        this.Locked = MyStringHash.GetOrCompute(nameof (Locked));
        this.Autolock = MyStringHash.GetOrCompute(nameof (Autolock));
        this.Warning = MyStringHash.GetOrCompute(nameof (Warning));
        this.Constraint = MyStringHash.GetOrCompute(nameof (Constraint));
      }
    }

    public class MyBlockPosComponent : MyPositionComponent
    {
      protected override void OnWorldPositionChanged(
        object source,
        bool updateChildren,
        bool forceUpdateAllChildren)
      {
        base.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
        (this.Container.Entity as MyCubeBlock).WorldPositionChanged(source);
      }

      private class Sandbox_Game_Entities_MyCubeBlock\u003C\u003EMyBlockPosComponent\u003C\u003EActor : IActivator, IActivator<MyCubeBlock.MyBlockPosComponent>
      {
        object IActivator.CreateInstance() => (object) new MyCubeBlock.MyBlockPosComponent();

        MyCubeBlock.MyBlockPosComponent IActivator<MyCubeBlock.MyBlockPosComponent>.CreateInstance() => new MyCubeBlock.MyBlockPosComponent();
      }
    }

    private class Sandbox_Game_Entities_MyCubeBlock\u003C\u003EActor : IActivator, IActivator<MyCubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyCubeBlock();

      MyCubeBlock IActivator<MyCubeBlock>.CreateInstance() => new MyCubeBlock();
    }
  }
}
