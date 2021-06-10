// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyEntityThrustComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public abstract class MyEntityThrustComponent : MyEntityComponentBase
  {
    private static float MAX_DISTANCE_RELATIVE_DAMPENING = 100f;
    private static float MAX_DISTANCE_RELATIVE_DAMPENING_SQ = MyEntityThrustComponent.MAX_DISTANCE_RELATIVE_DAMPENING * MyEntityThrustComponent.MAX_DISTANCE_RELATIVE_DAMPENING;
    private static readonly MyEntityThrustComponent.DirectionComparer m_directionComparer = new MyEntityThrustComponent.DirectionComparer();
    protected float m_lastPlanetaryInfluence = -1f;
    protected bool m_lastPlanetaryInfluenceHasAtmosphere;
    protected float m_lastPlanetaryGravityMagnitude;
    private int m_nextPlanetaryInfluenceRecalculation = -1;
    private const int m_maxInfluenceRecalculationInterval = 10000;
    private Vector3 m_maxNegativeThrust;
    private Vector3 m_maxPositiveThrust;
    protected readonly List<MyEntityThrustComponent.FuelTypeData> m_dataByFuelType = new List<MyEntityThrustComponent.FuelTypeData>();
    private readonly MyResourceSinkComponent m_resourceSink;
    private Vector3 m_totalMaxNegativeThrust;
    private Vector3 m_totalMaxPositiveThrust;
    protected readonly List<MyDefinitionId> m_fuelTypes = new List<MyDefinitionId>();
    private readonly Dictionary<MyDefinitionId, int> m_fuelTypeToIndex = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    protected readonly List<MyEntityThrustComponent.MyConveyorConnectedGroup> m_connectedGroups = new List<MyEntityThrustComponent.MyConveyorConnectedGroup>();
    protected MyResourceSinkComponent m_lastSink;
    protected MyEntityThrustComponent.FuelTypeData m_lastFuelTypeData;
    protected MyEntityThrustComponent.MyConveyorConnectedGroup m_lastGroup;
    protected Vector3 m_totalThrustOverride;
    protected float m_totalThrustOverridePower;
    private readonly List<MyEntityThrustComponent.MyConveyorConnectedGroup> m_groupsToTrySplit = new List<MyEntityThrustComponent.MyConveyorConnectedGroup>();
    private bool m_mergeAllGroupsDirty = true;
    [ThreadStatic]
    private static List<int> m_tmpGroupIndicesPerThread;
    [ThreadStatic]
    private static List<MyTuple<MyEntity, Vector3I>> m_tmpEntitiesWithDirectionsPerThread;
    [ThreadStatic]
    private static List<MyEntityThrustComponent.MyConveyorConnectedGroup> m_tmpGroupsPerThread;
    protected readonly MyConcurrentQueue<MyTuple<MyEntity, Vector3I, Func<bool>>> m_thrustEntitiesPending = new MyConcurrentQueue<MyTuple<MyEntity, Vector3I, Func<bool>>>();
    protected readonly HashSet<MyEntity> m_thrustEntitiesRemovedBeforeRegister = new HashSet<MyEntity>();
    private MyConcurrentQueue<IMyConveyorEndpointBlock> m_conveyorEndpointsPending = new MyConcurrentQueue<IMyConveyorEndpointBlock>();
    private MyConcurrentQueue<IMyConveyorSegmentBlock> m_conveyorSegmentsPending = new MyConcurrentQueue<IMyConveyorSegmentBlock>();
    protected bool m_thrustsChanged;
    private Vector3 m_controlThrust;
    private bool m_lastControlThrustChanged;
    private bool m_controlThrustChanged;
    private long m_lastPowerUpdate;
    private Vector3? m_maxThrustOverride;
    private bool m_secondFrameUpdate;
    private bool m_dampenersEnabledLastFrame = true;
    private int m_counter;
    private bool m_enabled;
    private Vector3 m_autoPilotControlThrust;

    protected ListReader<MyEntityThrustComponent.MyConveyorConnectedGroup> ConnectedGroups => new ListReader<MyEntityThrustComponent.MyConveyorConnectedGroup>(this.m_connectedGroups);

    private static List<int> m_tmpGroupIndices => MyUtils.Init<List<int>>(ref MyEntityThrustComponent.m_tmpGroupIndicesPerThread);

    private static List<MyTuple<MyEntity, Vector3I>> m_tmpEntitiesWithDirections => MyUtils.Init<List<MyTuple<MyEntity, Vector3I>>>(ref MyEntityThrustComponent.m_tmpEntitiesWithDirectionsPerThread);

    private static List<MyEntityThrustComponent.MyConveyorConnectedGroup> m_tmpGroups => MyUtils.Init<List<MyEntityThrustComponent.MyConveyorConnectedGroup>>(ref MyEntityThrustComponent.m_tmpGroupsPerThread);

    protected bool ControlThrustChanged
    {
      get => this.m_controlThrustChanged;
      set => this.m_controlThrustChanged = value;
    }

    public Vector3? MaxThrustOverride
    {
      get => !MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT ? new Vector3?() : this.m_maxThrustOverride;
      set => this.m_maxThrustOverride = value;
    }

    public MyEntity Entity => base.Entity as MyEntity;

    public float MaxRequiredPowerInput { get; private set; }

    public float MinRequiredPowerInput { get; private set; }

    public float SlowdownFactor { get; set; }

    public int ThrustCount { get; private set; }

    public bool DampenersEnabled { get; set; }

    public Vector3 ControlThrust
    {
      get => this.m_controlThrust;
      set
      {
        if (!(value != this.m_controlThrust))
          return;
        this.m_controlThrustChanged = true;
        this.m_controlThrust = value;
        this.OnControlTrustChanged();
      }
    }

    public Vector3 FinalThrust { get; private set; }

    public Vector3 AutoPilotControlThrust
    {
      get => this.m_autoPilotControlThrust;
      set
      {
        lock (this.m_dataByFuelType)
        {
          if (!(value != this.m_autoPilotControlThrust))
            return;
          this.m_autoPilotControlThrust = value;
          this.m_controlThrustChanged = true;
          this.OnControlTrustChanged();
        }
      }
    }

    public bool AutopilotEnabled { get; set; }

    public bool Enabled
    {
      get => this.m_enabled;
      set => this.m_enabled = value;
    }

    private int InitializeType(
      MyDefinitionId fuelType,
      List<MyEntityThrustComponent.FuelTypeData> dataByTypeList,
      List<MyDefinitionId> fuelTypeList,
      Dictionary<MyDefinitionId, int> fuelTypeToIndex,
      MyResourceSinkComponent resourceSink)
    {
      dataByTypeList.Add(new MyEntityThrustComponent.FuelTypeData()
      {
        ThrustsByDirection = new Dictionary<Vector3I, HashSet<MyEntity>>(6, (IEqualityComparer<Vector3I>) MyEntityThrustComponent.m_directionComparer),
        MaxRequirementsByDirection = new Dictionary<Vector3I, float>(6, (IEqualityComparer<Vector3I>) MyEntityThrustComponent.m_directionComparer),
        CurrentRequiredFuelInput = 0.0001f,
        Efficiency = 0.0f,
        EnergyDensity = 0.0f
      });
      int typeIndex = dataByTypeList.Count - 1;
      fuelTypeToIndex.Add(fuelType, typeIndex);
      fuelTypeList.Add(fuelType);
      foreach (Vector3I intDirection in Base6Directions.IntDirections)
        dataByTypeList[typeIndex].ThrustsByDirection[intDirection] = new HashSet<MyEntity>();
      MyResourceSinkInfo sinkData = new MyResourceSinkInfo()
      {
        ResourceTypeId = fuelType,
        MaxRequiredInput = 0.0f,
        RequiredInputFunc = (Func<float>) (() => MyEntityThrustComponent.RequiredFuelInput(dataByTypeList[typeIndex]))
      };
      if (fuelTypeList.Count == 1)
      {
        resourceSink.Init(MyStringHash.GetOrCompute("Thrust"), sinkData);
        resourceSink.IsPoweredChanged += new Action(this.Sink_IsPoweredChanged);
        resourceSink.CurrentInputChanged += new MyCurrentResourceInputChangedDelegate(this.Sink_CurrentInputChanged);
        MyEntityThrustComponent.AddSinkToSystems(resourceSink, this.Container.Entity as MyCubeGrid);
      }
      else
        resourceSink.AddType(ref sinkData);
      return typeIndex;
    }

    protected MyEntityThrustComponent()
    {
      MyResourceDistributorComponent.InitializeMappings();
      this.m_resourceSink = new MyResourceSinkComponent();
    }

    public virtual void Init()
    {
      this.Enabled = true;
      this.ThrustCount = 0;
      this.DampenersEnabled = true;
      this.m_lastPowerUpdate = (long) MySession.Static.GameplayFrameCounter;
    }

    public virtual void Register(
      MyEntity entity,
      Vector3I forwardVector,
      Func<bool> onRegisteredCallback = null)
    {
      MyDefinitionId fuelType = this.FuelType(entity);
      int index = -1;
      int typeIndex = -1;
      IMyConveyorEndpointBlock conveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
      Dictionary<Vector3I, HashSet<MyEntity>> thrustsByDirection;
      MyResourceSinkComponent resourceSink;
      if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref fuelType) && conveyorEndpointBlock != null)
      {
        MyEntityThrustComponent.FindConnectedGroups(conveyorEndpointBlock, this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
        MyEntityThrustComponent.MyConveyorConnectedGroup conveyorConnectedGroup;
        if (MyEntityThrustComponent.m_tmpGroupIndices.Count >= 1)
        {
          if (MyEntityThrustComponent.m_tmpGroupIndices.Count > 1)
            this.MergeGroups(this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
          index = MyEntityThrustComponent.m_tmpGroupIndices[0];
          conveyorConnectedGroup = this.m_connectedGroups[index];
        }
        else
        {
          conveyorConnectedGroup = new MyEntityThrustComponent.MyConveyorConnectedGroup(conveyorEndpointBlock);
          this.m_connectedGroups.Add(conveyorConnectedGroup);
          index = this.m_connectedGroups.Count - 1;
        }
        if (!conveyorConnectedGroup.TryGetTypeIndex(ref fuelType, out typeIndex))
        {
          typeIndex = this.InitializeType(fuelType, conveyorConnectedGroup.DataByFuelType, conveyorConnectedGroup.FuelTypes, conveyorConnectedGroup.FuelTypeToIndex, conveyorConnectedGroup.ResourceSink);
          if (conveyorConnectedGroup.FuelTypes.Count == 1)
            entity.Components.Add<MyResourceSinkComponent>(conveyorConnectedGroup.ResourceSink);
        }
        ++conveyorConnectedGroup.ThrustCount;
        ++conveyorConnectedGroup.DataByFuelType[typeIndex].ThrustCount;
        thrustsByDirection = conveyorConnectedGroup.DataByFuelType[typeIndex].ThrustsByDirection;
        resourceSink = conveyorConnectedGroup.ResourceSink;
        MyEntityThrustComponent.m_tmpGroupIndices.Clear();
      }
      else
      {
        if (!this.TryGetTypeIndex(ref fuelType, out typeIndex))
        {
          typeIndex = this.InitializeType(fuelType, this.m_dataByFuelType, this.m_fuelTypes, this.m_fuelTypeToIndex, this.m_resourceSink);
          if (this.m_fuelTypes.Count == 1)
            entity.Components.Add<MyResourceSinkComponent>(this.m_resourceSink);
        }
        else
          entity.Components.Remove<MyResourceSinkComponent>();
        thrustsByDirection = this.m_dataByFuelType[typeIndex].ThrustsByDirection;
        resourceSink = this.m_resourceSink;
        ++this.m_dataByFuelType[typeIndex].ThrustCount;
      }
      this.m_lastSink = resourceSink;
      this.m_lastGroup = index == -1 ? (MyEntityThrustComponent.MyConveyorConnectedGroup) null : this.m_connectedGroups[index];
      this.m_lastFuelTypeData = index == -1 ? this.m_dataByFuelType[typeIndex] : this.m_connectedGroups[index].DataByFuelType[typeIndex];
      thrustsByDirection[forwardVector].Add(entity);
      ++this.ThrustCount;
      this.MarkDirty();
    }

    protected virtual bool RegisterLazy(
      MyEntity entity,
      Vector3I forwardVector,
      Func<bool> onRegisteredCallback)
    {
      return true;
    }

    public bool IsRegistered(MyEntity entity, Vector3I forwardVector)
    {
      bool flag = false;
      MyDefinitionId myDefinitionId = this.FuelType(entity);
      IMyConveyorEndpointBlock conveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
      if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref myDefinitionId) && conveyorEndpointBlock != null)
      {
        foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
        {
          int typeIndex;
          if (connectedGroup.TryGetTypeIndex(ref myDefinitionId, out typeIndex) && connectedGroup.DataByFuelType[typeIndex].ThrustsByDirection[forwardVector].Contains(entity))
          {
            flag = true;
            break;
          }
        }
      }
      else
      {
        int typeIndex;
        if (this.TryGetTypeIndex(ref myDefinitionId, out typeIndex))
          flag = this.m_dataByFuelType[typeIndex].ThrustsByDirection[forwardVector].Contains(entity);
      }
      return flag;
    }

    public virtual void Unregister(MyEntity entity, Vector3I forwardVector)
    {
      if (entity == null || this.Entity == null || this.Entity.MarkedForClose)
        return;
      if (!this.IsRegistered(entity, forwardVector))
      {
        this.m_thrustEntitiesRemovedBeforeRegister.Add(entity);
      }
      else
      {
        Dictionary<Vector3I, HashSet<MyEntity>> dictionary = (Dictionary<Vector3I, HashSet<MyEntity>>) null;
        int thrustsLeftInGroup = 0;
        MyResourceSinkComponent resourceSink = (MyResourceSinkComponent) null;
        MyDefinitionId myDefinitionId = this.FuelType(entity);
        List<MyEntityThrustComponent.FuelTypeData> fuelTypeDataList = (List<MyEntityThrustComponent.FuelTypeData>) null;
        int index1 = -1;
        int typeIndex = -1;
        IMyConveyorEndpointBlock conveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
        if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref myDefinitionId) && conveyorEndpointBlock != null)
        {
          MyEntityThrustComponent.MyConveyorConnectedGroup conveyorConnectedGroup = this.TrySplitGroup(conveyorEndpointBlock);
          if (!conveyorConnectedGroup.TryGetTypeIndex(ref myDefinitionId, out typeIndex))
            return;
          if (conveyorConnectedGroup.DataByFuelType[typeIndex].ThrustsByDirection[forwardVector].Contains(entity))
          {
            thrustsLeftInGroup = --conveyorConnectedGroup.ThrustCount;
            resourceSink = conveyorConnectedGroup.ResourceSink;
            dictionary = conveyorConnectedGroup.DataByFuelType[typeIndex].ThrustsByDirection;
            fuelTypeDataList = conveyorConnectedGroup.DataByFuelType;
            for (int index2 = 0; index2 < this.m_connectedGroups.Count; ++index2)
            {
              if (this.m_connectedGroups[index2] == conveyorConnectedGroup)
              {
                index1 = index2;
                break;
              }
            }
          }
        }
        else
        {
          if (!this.TryGetTypeIndex(ref myDefinitionId, out typeIndex))
            return;
          resourceSink = this.m_resourceSink;
          dictionary = this.m_dataByFuelType[typeIndex].ThrustsByDirection;
          fuelTypeDataList = this.m_dataByFuelType;
          int num = 0;
          foreach (MyEntityThrustComponent.FuelTypeData fuelTypeData in this.m_dataByFuelType)
            num += fuelTypeData.ThrustCount;
          thrustsLeftInGroup = Math.Max(num - 1, 0);
        }
        if (dictionary == null)
          return;
        MyEntityThrustComponent.MyConveyorConnectedGroup conveyorConnectedGroup1 = index1 != -1 ? this.m_connectedGroups[index1] : (MyEntityThrustComponent.MyConveyorConnectedGroup) null;
        this.MoveSinkToNewEntity(entity, fuelTypeDataList, typeIndex, thrustsLeftInGroup, resourceSink, conveyorConnectedGroup1);
        dictionary[forwardVector].Remove(entity);
        resourceSink.SetMaxRequiredInputByType(myDefinitionId, resourceSink.MaxRequiredInputByType(myDefinitionId) - this.PowerAmountToFuel(ref myDefinitionId, this.MaxPowerConsumption(entity), conveyorConnectedGroup1));
        if (--fuelTypeDataList[typeIndex].ThrustCount == 0)
        {
          fuelTypeDataList.RemoveAtFast<MyEntityThrustComponent.FuelTypeData>(typeIndex);
          if (conveyorConnectedGroup1 != null)
          {
            conveyorConnectedGroup1.FuelTypes.RemoveAtFast<MyDefinitionId>(typeIndex);
            conveyorConnectedGroup1.FuelTypeToIndex.Remove(myDefinitionId);
          }
          else
          {
            this.m_fuelTypes.RemoveAtFast<MyDefinitionId>(typeIndex);
            this.m_fuelTypeToIndex.Remove(myDefinitionId);
          }
        }
        if (thrustsLeftInGroup == 0)
        {
          MyEntityThrustComponent.RemoveSinkFromSystems((MyResourceSinkComponentBase) resourceSink, this.Container.Entity as MyCubeGrid);
          if (index1 != -1)
            this.m_connectedGroups.RemoveAt(index1);
        }
        if (entity is MyThrust myThrust)
          myThrust.ClearThrustComponent();
        --this.ThrustCount;
      }
    }

    private void MoveSinkToNewEntity(
      MyEntity entity,
      List<MyEntityThrustComponent.FuelTypeData> fuelData,
      int typeIndex,
      int thrustsLeftInGroup,
      MyResourceSinkComponent resourceSink,
      MyEntityThrustComponent.MyConveyorConnectedGroup containingGroup)
    {
      if (!(this.Container.Entity is MyCubeGrid) || entity.Components.Get<MyResourceSinkComponent>() != resourceSink)
        return;
      entity.Components.Remove<MyResourceSinkComponent>();
      if (thrustsLeftInGroup <= 0)
        return;
      foreach (HashSet<MyEntity> myEntitySet in fuelData[typeIndex].ThrustsByDirection.Values)
      {
        if (myEntitySet.Count > 0)
        {
          bool flag = false;
          foreach (MyEntity myEntity in myEntitySet)
          {
            if (myEntity != entity)
            {
              myEntity.Components.Add<MyResourceSinkComponent>(resourceSink);
              MyEntityThrustComponent.AddSinkToSystems(resourceSink, this.Entity as MyCubeGrid);
              flag = true;
              if (containingGroup != null)
              {
                containingGroup.FirstEndpoint = (myEntity as IMyConveyorEndpointBlock).ConveyorEndpoint;
                break;
              }
              break;
            }
          }
          if (flag)
            break;
        }
      }
    }

    private void MergeGroups(
      List<MyEntityThrustComponent.MyConveyorConnectedGroup> groups,
      List<int> connectedGroupIndices)
    {
      int index1 = int.MinValue;
      int num1 = int.MinValue;
      foreach (int connectedGroupIndex in connectedGroupIndices)
      {
        MyEntityThrustComponent.MyConveyorConnectedGroup group = groups[connectedGroupIndex];
        if (group.ThrustCount > num1)
        {
          index1 = connectedGroupIndex;
          num1 = group.ThrustCount;
        }
      }
      MyEntityThrustComponent.MyConveyorConnectedGroup group1 = groups[index1];
      foreach (int connectedGroupIndex in connectedGroupIndices)
      {
        if (connectedGroupIndex != index1)
        {
          MyEntityThrustComponent.MyConveyorConnectedGroup group2 = groups[connectedGroupIndex];
          foreach (MyDefinitionId fuelType in group2.FuelTypes)
          {
            MyDefinitionId fuelId = fuelType;
            int typeIndex;
            if (!group1.TryGetTypeIndex(ref fuelId, out typeIndex))
              typeIndex = this.InitializeType(fuelType, group1.DataByFuelType, group1.FuelTypes, group1.FuelTypeToIndex, group1.ResourceSink);
            MyEntityThrustComponent.FuelTypeData fuelTypeData1 = group1.DataByFuelType[typeIndex];
            MyEntityThrustComponent.FuelTypeData fuelTypeData2 = group2.DataByFuelType[typeIndex];
            fuelTypeData1.MaxRequiredPowerInput += fuelTypeData2.MaxRequiredPowerInput;
            fuelTypeData1.MinRequiredPowerInput += fuelTypeData2.MinRequiredPowerInput;
            fuelTypeData1.CurrentRequiredFuelInput += fuelTypeData2.CurrentRequiredFuelInput;
            fuelTypeData1.MaxNegativeThrust += fuelTypeData2.MaxNegativeThrust;
            fuelTypeData1.MaxPositiveThrust += fuelTypeData2.MaxPositiveThrust;
            fuelTypeData1.ThrustOverride += fuelTypeData2.ThrustOverride;
            fuelTypeData1.ThrustOverridePower += fuelTypeData2.ThrustOverridePower;
            fuelTypeData1.ThrustCount += fuelTypeData2.ThrustCount;
            foreach (Vector3I intDirection in Base6Directions.IntDirections)
            {
              float num2;
              if (fuelTypeData2.MaxRequirementsByDirection.TryGetValue(intDirection, out num2))
              {
                float num3;
                fuelTypeData1.MaxRequirementsByDirection[intDirection] = !fuelTypeData1.MaxRequirementsByDirection.TryGetValue(intDirection, out num3) ? num2 : num3 + num2;
              }
              if (!fuelTypeData1.ThrustsByDirection.ContainsKey(intDirection))
                fuelTypeData1.ThrustsByDirection[intDirection] = new HashSet<MyEntity>();
              HashSet<MyEntity> myEntitySet = fuelTypeData1.ThrustsByDirection[intDirection];
              if (fuelTypeData2.ThrustsByDirection.ContainsKey(intDirection))
              {
                foreach (MyEntity myEntity in fuelTypeData2.ThrustsByDirection[intDirection])
                {
                  myEntitySet.Add(myEntity);
                  myEntity.Components.Remove<MyResourceSinkComponent>();
                }
              }
            }
            group1.ThrustCount += group2.ThrustCount;
            group1.ThrustOverride += group2.ThrustOverride;
            group1.ThrustOverridePower += group2.ThrustOverridePower;
            group1.MaxNegativeThrust += group2.MaxNegativeThrust;
            group1.MaxPositiveThrust += group2.MaxPositiveThrust;
            MyEntityThrustComponent.RemoveSinkFromSystems((MyResourceSinkComponentBase) group2.ResourceSink, this.Container.Entity as MyCubeGrid);
          }
        }
      }
      connectedGroupIndices.Sort();
      for (int index2 = connectedGroupIndices.Count - 1; index2 >= 0; --index2)
      {
        if (connectedGroupIndices[index2] != index1)
        {
          if (connectedGroupIndices[index2] < index1)
            --index1;
          groups.RemoveAtFast<MyEntityThrustComponent.MyConveyorConnectedGroup>(connectedGroupIndices[index2]);
          connectedGroupIndices.RemoveAt(index2);
        }
      }
      group1.ResourceSink.Update();
      connectedGroupIndices[0] = index1;
    }

    public void MergeAllGroupsDirty() => this.m_mergeAllGroupsDirty = true;

    private void TryMergeAllGroups()
    {
      if (this.m_connectedGroups == null || this.m_connectedGroups.Count == 0)
        return;
      int index = 0;
      do
      {
        MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup = this.m_connectedGroups[index];
        IMyConveyorEndpointBlock block = connectedGroup.FirstEndpoint != null ? connectedGroup.FirstEndpoint.CubeBlock as IMyConveyorEndpointBlock : (IMyConveyorEndpointBlock) null;
        if (block != null)
        {
          MyEntityThrustComponent.FindConnectedGroups(block, this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
          if (MyEntityThrustComponent.m_tmpGroupIndices.Count > 1)
          {
            this.MergeGroups(this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
            --index;
          }
          MyEntityThrustComponent.m_tmpGroupIndices.Clear();
          ++index;
        }
      }
      while (index < this.m_connectedGroups.Count);
    }

    private static void FindConnectedGroups(
      IMyConveyorSegmentBlock block,
      List<MyEntityThrustComponent.MyConveyorConnectedGroup> groups,
      List<int> outConnectedGroupIndices)
    {
      if (block.ConveyorSegment.ConveyorLine == null)
        return;
      IMyConveyorEndpoint to = block.ConveyorSegment.ConveyorLine.GetEndpoint(0) ?? block.ConveyorSegment.ConveyorLine.GetEndpoint(1);
      if (to == null)
        return;
      for (int index = 0; index < groups.Count; ++index)
      {
        if (MyGridConveyorSystem.Reachable(groups[index].FirstEndpoint, to))
          outConnectedGroupIndices.Add(index);
      }
    }

    private static void FindConnectedGroups(
      IMyConveyorEndpointBlock block,
      List<MyEntityThrustComponent.MyConveyorConnectedGroup> groups,
      List<int> outConnectedGroupIndices)
    {
      for (int index = 0; index < groups.Count; ++index)
      {
        MyEntityThrustComponent.MyConveyorConnectedGroup group = groups[index];
        if (group.FirstEndpoint != null && MyGridConveyorSystem.Reachable(group.FirstEndpoint, block.ConveyorEndpoint))
          outConnectedGroupIndices.Add(index);
      }
    }

    private MyEntityThrustComponent.MyConveyorConnectedGroup TrySplitGroup(
      IMyConveyorEndpointBlock conveyorEndpointBlock,
      MyEntityThrustComponent.MyConveyorConnectedGroup groupOverride = null)
    {
      MyEntity thrustEntity1 = conveyorEndpointBlock as MyEntity;
      MyEntityThrustComponent.MyConveyorConnectedGroup group1 = groupOverride ?? this.FindEntityGroup(thrustEntity1);
      if (group1 == null)
        return (MyEntityThrustComponent.MyConveyorConnectedGroup) null;
      if (conveyorEndpointBlock != null && conveyorEndpointBlock.ConveyorEndpoint == group1.FirstEndpoint)
      {
        if (group1.ThrustCount == 1)
          return group1;
        foreach (MyEntityThrustComponent.FuelTypeData fuelTypeData in group1.DataByFuelType)
        {
          bool flag = false;
          foreach (HashSet<MyEntity> myEntitySet in fuelTypeData.ThrustsByDirection.Values)
          {
            foreach (MyEntity myEntity in myEntitySet)
            {
              if (myEntity != thrustEntity1)
              {
                group1.FirstEndpoint = (myEntity as IMyConveyorEndpointBlock).ConveyorEndpoint;
                thrustEntity1.Components.Remove<MyResourceSinkComponent>();
                myEntity.Components.Add<MyResourceSinkComponent>(group1.ResourceSink);
                MyEntityThrustComponent.AddSinkToSystems(group1.ResourceSink, this.Entity as MyCubeGrid);
                flag = true;
                break;
              }
            }
            if (flag)
              break;
          }
          if (flag)
            break;
        }
      }
      foreach (Vector3I intDirection in Base6Directions.IntDirections)
      {
        for (int index = 0; index < group1.FuelTypes.Count; ++index)
        {
          MyEntityThrustComponent.FuelTypeData fuelTypeData = group1.DataByFuelType[index];
          foreach (MyEntity thrustEntity2 in fuelTypeData.ThrustsByDirection[intDirection])
          {
            IMyConveyorEndpoint conveyorEndpoint = (thrustEntity2 as IMyConveyorEndpointBlock).ConveyorEndpoint;
            if (thrustEntity1 != thrustEntity2 && !MyGridConveyorSystem.Reachable(conveyorEndpoint, group1.FirstEndpoint))
            {
              MyDefinitionId fuelType = this.FuelType(thrustEntity2);
              group1.ResourceSink.SetMaxRequiredInputByType(fuelType, group1.ResourceSink.MaxRequiredInputByType(fuelType) - this.PowerAmountToFuel(ref fuelType, this.MaxPowerConsumption(thrustEntity2), group1));
              --fuelTypeData.ThrustCount;
              --group1.ThrustCount;
              MyEntityThrustComponent.m_tmpEntitiesWithDirections.Add(new MyTuple<MyEntity, Vector3I>(thrustEntity2, intDirection));
            }
          }
          foreach (MyTuple<MyEntity, Vector3I> entitiesWithDirection in MyEntityThrustComponent.m_tmpEntitiesWithDirections)
          {
            fuelTypeData.ThrustsByDirection[entitiesWithDirection.Item2].Remove(entitiesWithDirection.Item1);
            this.RemoveFromGroup(entitiesWithDirection.Item1, group1);
          }
        }
      }
      foreach (MyTuple<MyEntity, Vector3I> entitiesWithDirection in MyEntityThrustComponent.m_tmpEntitiesWithDirections)
      {
        MyEntity thrustEntity2 = entitiesWithDirection.Item1;
        Vector3I key = entitiesWithDirection.Item2;
        MyDefinitionId myDefinitionId = this.FuelType(thrustEntity2);
        bool flag = false;
        foreach (MyEntityThrustComponent.MyConveyorConnectedGroup mTmpGroup in MyEntityThrustComponent.m_tmpGroups)
        {
          if (MyGridConveyorSystem.Reachable((thrustEntity2 as IMyConveyorEndpointBlock).ConveyorEndpoint, mTmpGroup.FirstEndpoint))
          {
            int typeIndex;
            if (!mTmpGroup.TryGetTypeIndex(ref myDefinitionId, out typeIndex))
              typeIndex = this.InitializeType(myDefinitionId, mTmpGroup.DataByFuelType, mTmpGroup.FuelTypes, mTmpGroup.FuelTypeToIndex, mTmpGroup.ResourceSink);
            MyEntityThrustComponent.FuelTypeData fuelTypeData = mTmpGroup.DataByFuelType[typeIndex];
            fuelTypeData.ThrustsByDirection[key].Add(thrustEntity2);
            this.AddToGroup(thrustEntity2, mTmpGroup);
            ++fuelTypeData.ThrustCount;
            ++mTmpGroup.ThrustCount;
            mTmpGroup.ResourceSink.SetMaxRequiredInputByType(myDefinitionId, mTmpGroup.ResourceSink.MaxRequiredInputByType(myDefinitionId) + this.PowerAmountToFuel(ref myDefinitionId, this.MaxPowerConsumption(thrustEntity2), mTmpGroup));
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          MyEntityThrustComponent.MyConveyorConnectedGroup group2 = new MyEntityThrustComponent.MyConveyorConnectedGroup(thrustEntity2 as IMyConveyorEndpointBlock);
          MyEntityThrustComponent.m_tmpGroups.Add(group2);
          this.m_connectedGroups.Add(group2);
          int index = this.InitializeType(myDefinitionId, group2.DataByFuelType, group2.FuelTypes, group2.FuelTypeToIndex, group2.ResourceSink);
          thrustEntity2.Components.Add<MyResourceSinkComponent>(group2.ResourceSink);
          MyEntityThrustComponent.FuelTypeData fuelTypeData = group2.DataByFuelType[index];
          int typeIndex = group1.GetTypeIndex(ref myDefinitionId);
          fuelTypeData.Efficiency = group1.DataByFuelType[typeIndex].Efficiency;
          fuelTypeData.EnergyDensity = group1.DataByFuelType[typeIndex].EnergyDensity;
          fuelTypeData.ThrustsByDirection[key].Add(thrustEntity2);
          this.AddToGroup(thrustEntity2, group2);
          ++fuelTypeData.ThrustCount;
          ++group2.ThrustCount;
          group2.ResourceSink.SetMaxRequiredInputByType(myDefinitionId, group2.ResourceSink.MaxRequiredInputByType(myDefinitionId) + this.PowerAmountToFuel(ref myDefinitionId, this.MaxPowerConsumption(thrustEntity2), group2));
        }
      }
      MyEntityThrustComponent.m_tmpGroups.Clear();
      MyEntityThrustComponent.m_tmpEntitiesWithDirections.Clear();
      return group1;
    }

    private static void AddSinkToSystems(MyResourceSinkComponent resourceSink, MyCubeGrid cubeGrid)
    {
      if (cubeGrid == null)
        return;
      MyCubeGridSystems gridSystems = cubeGrid.GridSystems;
      if (gridSystems == null || gridSystems.ResourceDistributor == null)
        return;
      gridSystems.ResourceDistributor.AddSink(resourceSink);
    }

    private static void RemoveSinkFromSystems(
      MyResourceSinkComponentBase resourceSink,
      MyCubeGrid cubeGrid)
    {
      if (cubeGrid == null)
        return;
      MyCubeGridSystems gridSystems = cubeGrid.GridSystems;
      if (gridSystems == null || gridSystems.ResourceDistributor == null)
        return;
      gridSystems.ResourceDistributor.RemoveSink(resourceSink as MyResourceSinkComponent);
    }

    private void Sink_CurrentInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      this.m_controlThrustChanged = true;
    }

    private void Sink_IsPoweredChanged() => this.MarkDirty();

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      MyPlanets.Static.OnPlanetAdded += new Action<MyPlanet>(this.OnPlanetAddedOrRemoved);
      MyPlanets.Static.OnPlanetRemoved += new Action<MyPlanet>(this.OnPlanetAddedOrRemoved);
      this.Entity.OnTeleport += new Action<MyEntity>(this.OnTeleport);
      this.MarkDirty();
      if (!(this.Entity is MyCubeGrid entity))
        return;
      entity.OnBlockAdded += new Action<MySlimBlock>(this.CubeGrid_OnBlockAdded);
      entity.GridSystems.ConveyorSystem.OnBeforeRemoveSegmentBlock += new Action<IMyConveyorSegmentBlock>(this.ConveyorSystem_OnBeforeRemoveSegmentBlock);
      entity.GridSystems.ConveyorSystem.OnBeforeRemoveEndpointBlock += new Action<IMyConveyorEndpointBlock>(this.ConveyorSystem_OnBeforeRemoveEndpointBlock);
      entity.GridSystems.ConveyorSystem.ResourceSink.IsPoweredChanged += new Action(this.ConveyorSystem_OnPoweredChanged);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      MyPlanets.Static.OnPlanetAdded -= new Action<MyPlanet>(this.OnPlanetAddedOrRemoved);
      MyPlanets.Static.OnPlanetRemoved -= new Action<MyPlanet>(this.OnPlanetAddedOrRemoved);
      if (this.Container.Entity != null)
        ((MyEntity) this.Container.Entity).OnTeleport -= new Action<MyEntity>(this.OnTeleport);
      foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
        MyEntityThrustComponent.RemoveSinkFromSystems((MyResourceSinkComponentBase) connectedGroup.ResourceSink, this.Container.Entity as MyCubeGrid);
      MyEntityThrustComponent.RemoveSinkFromSystems((MyResourceSinkComponentBase) this.m_resourceSink, this.Container.Entity as MyCubeGrid);
      if (!(this.Entity is MyCubeGrid entity))
        return;
      entity.OnBlockAdded -= new Action<MySlimBlock>(this.CubeGrid_OnBlockAdded);
      entity.GridSystems.ConveyorSystem.OnBeforeRemoveSegmentBlock -= new Action<IMyConveyorSegmentBlock>(this.ConveyorSystem_OnBeforeRemoveSegmentBlock);
      entity.GridSystems.ConveyorSystem.OnBeforeRemoveEndpointBlock -= new Action<IMyConveyorEndpointBlock>(this.ConveyorSystem_OnBeforeRemoveEndpointBlock);
      entity.GridSystems.ConveyorSystem.ResourceSink.IsPoweredChanged -= new Action(this.ConveyorSystem_OnPoweredChanged);
    }

    public virtual void UpdateBeforeSimulation(
      bool updateDampeners,
      MyEntity relativeDampeningEntity)
    {
      if (this.Entity == null)
        return;
      if (this.Entity.InScene)
        this.UpdateConveyorSystemChanges();
      if (this.ThrustCount == 0)
      {
        this.Entity.Components.Remove<MyEntityThrustComponent>();
      }
      else
      {
        if (MySession.Static.GameplayFrameCounter >= this.m_nextPlanetaryInfluenceRecalculation)
          this.RecalculatePlanetaryInfluence();
        if (this.m_thrustsChanged)
        {
          this.RecomputeThrustParameters();
          if (this.Entity is MyCubeGrid && this.Entity.Physics != null && !this.Entity.Physics.RigidBody.IsActive)
            (this.Entity as MyCubeGrid).ActivatePhysics();
        }
        if (this.Enabled && this.Entity.Physics != null)
        {
          MatrixD matrix = this.Entity.PositionComp.WorldMatrixNormalizedInv;
          Vector3 dampeningVelocity = Vector3.TransformNormal(relativeDampeningEntity == null || relativeDampeningEntity.Physics == null ? Vector3.Zero : relativeDampeningEntity.Physics.LinearVelocity + 30f * relativeDampeningEntity.Physics.LinearAcceleration * 0.01666667f, matrix);
          if ((double) dampeningVelocity.LengthSquared() > 0.0 && this.Entity is MyCubeGrid && (this.Entity.Physics != null && !this.Entity.Physics.RigidBody.IsActive))
            (this.Entity as MyCubeGrid).ActivatePhysics();
          this.UpdateThrusts(updateDampeners, dampeningVelocity);
          if (this.m_thrustsChanged)
            this.RecomputeThrustParameters();
        }
        if (!this.DampenersEnabled && this.m_dampenersEnabledLastFrame)
        {
          foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
          {
            if (connectedGroup.DataByFuelType.Count > 0)
              this.TurnOffThrusterFlame(connectedGroup.DataByFuelType);
          }
          if (this.m_dataByFuelType.Count > 0)
            this.TurnOffThrusterFlame(this.m_dataByFuelType);
        }
        this.m_dampenersEnabledLastFrame = this.DampenersEnabled;
        this.m_thrustsChanged = false;
      }
    }

    private void TurnOffThrusterFlame(
      List<MyEntityThrustComponent.FuelTypeData> dataByFuelType)
    {
      foreach (MyEntityThrustComponent.FuelTypeData fuelTypeData in dataByFuelType)
      {
        foreach (KeyValuePair<Vector3I, HashSet<MyEntity>> keyValuePair in fuelTypeData.ThrustsByDirection)
        {
          foreach (MyEntity myEntity in keyValuePair.Value)
          {
            if (myEntity is MyThrust myThrust && (double) myThrust.ThrustOverride <= 0.0)
              myThrust.CurrentStrength = 0.0f;
          }
        }
      }
    }

    private void RecomputeThrustParameters()
    {
      this.m_secondFrameUpdate = true;
      if (!this.m_thrustsChanged && this.m_secondFrameUpdate)
        this.m_secondFrameUpdate = false;
      Vector3 zero = Vector3.Zero;
      float num = 0.0f;
      Vector3 vector3_1 = new Vector3();
      Vector3 vector3_2 = new Vector3();
      Vector3 vector3_3 = new Vector3();
      Vector3 vector3_4 = new Vector3();
      this.MaxRequiredPowerInput = 0.0f;
      this.MinRequiredPowerInput = 0.0f;
      foreach (MyEntityThrustComponent.FuelTypeData fuelData in this.m_dataByFuelType)
      {
        this.RecomputeTypeThrustParameters(fuelData);
        this.MaxRequiredPowerInput += fuelData.MaxRequiredPowerInput;
        this.MinRequiredPowerInput += fuelData.MinRequiredPowerInput;
        vector3_1 += fuelData.MaxPositiveThrust;
        vector3_2 += fuelData.MaxNegativeThrust;
        zero += fuelData.ThrustOverride;
        num += fuelData.ThrustOverridePower;
      }
      Vector3 vector3_5 = vector3_3 + this.m_maxNegativeThrust;
      Vector3 vector3_6 = vector3_4 + this.m_maxPositiveThrust;
      foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
      {
        connectedGroup.MaxPositiveThrust = new Vector3();
        connectedGroup.MaxNegativeThrust = new Vector3();
        connectedGroup.ThrustOverride = new Vector3();
        connectedGroup.ThrustOverridePower = 0.0f;
        foreach (MyEntityThrustComponent.FuelTypeData fuelData in connectedGroup.DataByFuelType)
        {
          this.RecomputeTypeThrustParameters(fuelData);
          this.MaxRequiredPowerInput += fuelData.MaxRequiredPowerInput;
          this.MinRequiredPowerInput += fuelData.MinRequiredPowerInput;
          connectedGroup.MaxPositiveThrust += fuelData.MaxPositiveThrust;
          connectedGroup.MaxNegativeThrust += fuelData.MaxNegativeThrust;
          connectedGroup.ThrustOverride += fuelData.ThrustOverride;
          connectedGroup.ThrustOverridePower += fuelData.ThrustOverridePower;
        }
        vector3_5 += connectedGroup.MaxNegativeThrust;
        vector3_6 += connectedGroup.MaxPositiveThrust;
      }
      this.m_totalThrustOverride = zero;
      this.m_totalThrustOverridePower = num;
      this.m_maxPositiveThrust = vector3_1;
      this.m_maxNegativeThrust = vector3_2;
      this.m_totalMaxNegativeThrust = vector3_5;
      this.m_totalMaxPositiveThrust = vector3_6;
    }

    public float GetMaxThrustInDirection(Base6Directions.Direction direction)
    {
      switch (direction)
      {
        case Base6Directions.Direction.Backward:
          return this.m_maxNegativeThrust.Z;
        case Base6Directions.Direction.Left:
          return this.m_maxNegativeThrust.X;
        case Base6Directions.Direction.Right:
          return this.m_maxPositiveThrust.X;
        case Base6Directions.Direction.Up:
          return this.m_maxPositiveThrust.Y;
        case Base6Directions.Direction.Down:
          return this.m_maxNegativeThrust.Y;
        default:
          return this.m_maxPositiveThrust.Z;
      }
    }

    private float GetMaxRequirementsByDirection(
      MyEntityThrustComponent.FuelTypeData fuelData,
      Vector3I direction)
    {
      float num;
      return fuelData.MaxRequirementsByDirection.TryGetValue(direction, out num) ? num : 0.0f;
    }

    private void RecomputeTypeThrustParameters(MyEntityThrustComponent.FuelTypeData fuelData)
    {
      fuelData.MaxRequiredPowerInput = 0.0f;
      fuelData.MinRequiredPowerInput = 0.0f;
      fuelData.MaxPositiveThrust = new Vector3();
      fuelData.MaxNegativeThrust = new Vector3();
      fuelData.MaxRequirementsByDirection.Clear();
      fuelData.ThrustOverride = new Vector3();
      fuelData.ThrustOverridePower = 0.0f;
      fuelData.CurrentRequiredFuelInput = 0.0f;
      foreach (KeyValuePair<Vector3I, HashSet<MyEntity>> keyValuePair in fuelData.ThrustsByDirection)
      {
        if (!fuelData.MaxRequirementsByDirection.ContainsKey(keyValuePair.Key))
          fuelData.MaxRequirementsByDirection[keyValuePair.Key] = 0.0f;
        float num1 = 0.0f;
        foreach (MyEntity thrustEntity in keyValuePair.Value)
        {
          if (!this.RecomputeOverriddenParameters(thrustEntity, fuelData) && this.IsUsed(thrustEntity))
          {
            float num2 = this.ForceMagnitude(thrustEntity, this.m_lastPlanetaryInfluence, this.m_lastPlanetaryInfluenceHasAtmosphere);
            float forceMultiplier = this.CalculateForceMultiplier(thrustEntity, this.m_lastPlanetaryInfluence, this.m_lastPlanetaryInfluenceHasAtmosphere);
            float consumptionMultiplier = this.CalculateConsumptionMultiplier(thrustEntity, this.m_lastPlanetaryGravityMagnitude);
            if (thrustEntity is MyThrust && !(thrustEntity as MyThrust).IsPowered)
            {
              fuelData.MaxPositiveThrust += 0.0f;
              fuelData.MaxNegativeThrust += 0.0f;
            }
            else
            {
              fuelData.MaxPositiveThrust += Vector3.Clamp(-keyValuePair.Key * num2, Vector3.Zero, Vector3.PositiveInfinity);
              fuelData.MaxNegativeThrust += -Vector3.Clamp(-keyValuePair.Key * num2, Vector3.NegativeInfinity, Vector3.Zero);
            }
            num1 += this.MaxPowerConsumption(thrustEntity) * forceMultiplier * consumptionMultiplier;
            fuelData.MinRequiredPowerInput += this.MinPowerConsumption(thrustEntity) * consumptionMultiplier;
          }
        }
        fuelData.MaxRequirementsByDirection[keyValuePair.Key] += num1;
      }
      fuelData.MaxRequiredPowerInput += Math.Max(this.GetMaxRequirementsByDirection(fuelData, Vector3I.Forward), this.GetMaxRequirementsByDirection(fuelData, Vector3I.Backward));
      fuelData.MaxRequiredPowerInput += Math.Max(this.GetMaxRequirementsByDirection(fuelData, Vector3I.Left), this.GetMaxRequirementsByDirection(fuelData, Vector3I.Right));
      fuelData.MaxRequiredPowerInput += Math.Max(this.GetMaxRequirementsByDirection(fuelData, Vector3I.Up), this.GetMaxRequirementsByDirection(fuelData, Vector3I.Down));
    }

    protected virtual void UpdateThrusts(bool applyDampeners, Vector3 dampeningVelocity)
    {
      for (int index = 0; index < this.m_dataByFuelType.Count; ++index)
      {
        MyEntityThrustComponent.FuelTypeData fuelData = this.m_dataByFuelType[index];
        if (this.AutopilotEnabled)
          this.ComputeAiThrust(this.AutoPilotControlThrust, fuelData);
        else
          this.ComputeBaseThrust(ref this.m_controlThrust, fuelData, applyDampeners, dampeningVelocity);
      }
      for (int index1 = 0; index1 < this.m_connectedGroups.Count; ++index1)
      {
        MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup = this.m_connectedGroups[index1];
        for (int index2 = 0; index2 < connectedGroup.DataByFuelType.Count; ++index2)
        {
          MyEntityThrustComponent.FuelTypeData fuelData = connectedGroup.DataByFuelType[index2];
          if (this.AutopilotEnabled)
            this.ComputeAiThrust(this.AutoPilotControlThrust, fuelData);
          else
            this.ComputeBaseThrust(ref this.m_controlThrust, fuelData, applyDampeners, dampeningVelocity);
        }
      }
      this.FinalThrust = new Vector3();
      bool flag = !MySession.Static.SimplifiedSimulation;
      for (int index = 0; index < this.m_dataByFuelType.Count; ++index)
      {
        MyDefinitionId fuelType = this.m_fuelTypes[index];
        MyEntityThrustComponent.FuelTypeData fuelTypeData = this.m_dataByFuelType[index];
        if (flag)
          this.UpdatePowerAndThrustStrength(fuelTypeData.CurrentThrust, fuelType, (MyEntityThrustComponent.MyConveyorConnectedGroup) null, true);
        Vector3 vector3 = this.m_maxPositiveThrust + this.m_maxNegativeThrust;
        Vector3 thrust;
        thrust.X = (double) vector3.X != 0.0 ? fuelTypeData.CurrentThrust.X * (fuelTypeData.MaxPositiveThrust.X + fuelTypeData.MaxNegativeThrust.X) / vector3.X : 0.0f;
        thrust.Y = (double) vector3.Y != 0.0 ? fuelTypeData.CurrentThrust.Y * (fuelTypeData.MaxPositiveThrust.Y + fuelTypeData.MaxNegativeThrust.Y) / vector3.Y : 0.0f;
        thrust.Z = (double) vector3.Z != 0.0 ? fuelTypeData.CurrentThrust.Z * (fuelTypeData.MaxPositiveThrust.Z + fuelTypeData.MaxNegativeThrust.Z) / vector3.Z : 0.0f;
        this.FinalThrust += this.ApplyThrustModifiers(ref fuelType, ref thrust, ref this.m_totalThrustOverride, (MyResourceSinkComponentBase) this.m_resourceSink);
      }
      for (int index1 = 0; index1 < this.m_connectedGroups.Count; ++index1)
      {
        MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup = this.m_connectedGroups[index1];
        for (int index2 = 0; index2 < connectedGroup.DataByFuelType.Count; ++index2)
        {
          MyDefinitionId fuelType = connectedGroup.FuelTypes[index2];
          MyEntityThrustComponent.FuelTypeData fuelTypeData = connectedGroup.DataByFuelType[index2];
          if (flag)
          {
            HkRigidBody rigidBody = this.Entity.Physics.RigidBody;
            if ((rigidBody != null ? (rigidBody.IsActive ? 1 : 0) : 1) != 0 || this.m_thrustsChanged || this.m_lastControlThrustChanged)
              this.UpdatePowerAndThrustStrength(fuelTypeData.CurrentThrust, fuelType, connectedGroup, true);
          }
          Vector3 vector3 = connectedGroup.MaxPositiveThrust + connectedGroup.MaxNegativeThrust;
          Vector3 thrust;
          thrust.X = (double) vector3.X != 0.0 ? fuelTypeData.CurrentThrust.X * (fuelTypeData.MaxPositiveThrust.X + fuelTypeData.MaxNegativeThrust.X) / vector3.X : 0.0f;
          thrust.Y = (double) vector3.Y != 0.0 ? fuelTypeData.CurrentThrust.Y * (fuelTypeData.MaxPositiveThrust.Y + fuelTypeData.MaxNegativeThrust.Y) / vector3.Y : 0.0f;
          thrust.Z = (double) vector3.Z != 0.0 ? fuelTypeData.CurrentThrust.Z * (fuelTypeData.MaxPositiveThrust.Z + fuelTypeData.MaxNegativeThrust.Z) / vector3.Z : 0.0f;
          this.FinalThrust += this.ApplyThrustModifiers(ref fuelType, ref thrust, ref connectedGroup.ThrustOverride, (MyResourceSinkComponentBase) connectedGroup.ResourceSink);
        }
      }
      this.m_lastControlThrustChanged = this.m_controlThrustChanged;
      this.m_controlThrustChanged = false;
    }

    public Vector3 GetAutoPilotThrustForDirection(Vector3 direction)
    {
      foreach (MyEntityThrustComponent.FuelTypeData fuelData in this.m_dataByFuelType)
        this.ComputeAiThrust(this.AutoPilotControlThrust, fuelData);
      foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
      {
        foreach (MyEntityThrustComponent.FuelTypeData fuelData in connectedGroup.DataByFuelType)
          this.ComputeAiThrust(this.AutoPilotControlThrust, fuelData);
      }
      Vector3 vector3_1 = new Vector3();
      for (int index = 0; index < this.m_dataByFuelType.Count; ++index)
      {
        MyDefinitionId fuelType = this.m_fuelTypes[index];
        MyEntityThrustComponent.FuelTypeData fuelTypeData = this.m_dataByFuelType[index];
        this.UpdatePowerAndThrustStrength(fuelTypeData.CurrentThrust, fuelType, (MyEntityThrustComponent.MyConveyorConnectedGroup) null, false);
        Vector3 vector3_2 = this.m_maxPositiveThrust + this.m_maxNegativeThrust;
        Vector3 thrust;
        thrust.X = (double) vector3_2.X != 0.0 ? fuelTypeData.CurrentThrust.X * (fuelTypeData.MaxPositiveThrust.X + fuelTypeData.MaxNegativeThrust.X) / vector3_2.X : 0.0f;
        thrust.Y = (double) vector3_2.Y != 0.0 ? fuelTypeData.CurrentThrust.Y * (fuelTypeData.MaxPositiveThrust.Y + fuelTypeData.MaxNegativeThrust.Y) / vector3_2.Y : 0.0f;
        thrust.Z = (double) vector3_2.Z != 0.0 ? fuelTypeData.CurrentThrust.Z * (fuelTypeData.MaxPositiveThrust.Z + fuelTypeData.MaxNegativeThrust.Z) / vector3_2.Z : 0.0f;
        vector3_1 += this.ApplyThrustModifiers(ref fuelType, ref thrust, ref this.m_totalThrustOverride, (MyResourceSinkComponentBase) this.m_resourceSink);
      }
      foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
      {
        for (int index = 0; index < connectedGroup.DataByFuelType.Count; ++index)
        {
          MyDefinitionId fuelType = connectedGroup.FuelTypes[index];
          MyEntityThrustComponent.FuelTypeData fuelTypeData = connectedGroup.DataByFuelType[index];
          this.UpdatePowerAndThrustStrength(fuelTypeData.CurrentThrust, fuelType, connectedGroup, false);
          Vector3 vector3_2 = connectedGroup.MaxPositiveThrust + connectedGroup.MaxNegativeThrust;
          Vector3 thrust;
          thrust.X = (double) vector3_2.X != 0.0 ? fuelTypeData.CurrentThrust.X * (fuelTypeData.MaxPositiveThrust.X + fuelTypeData.MaxNegativeThrust.X) / vector3_2.X : 0.0f;
          thrust.Y = (double) vector3_2.Y != 0.0 ? fuelTypeData.CurrentThrust.Y * (fuelTypeData.MaxPositiveThrust.Y + fuelTypeData.MaxNegativeThrust.Y) / vector3_2.Y : 0.0f;
          thrust.Z = (double) vector3_2.Z != 0.0 ? fuelTypeData.CurrentThrust.Z * (fuelTypeData.MaxPositiveThrust.Z + fuelTypeData.MaxNegativeThrust.Z) / vector3_2.Z : 0.0f;
          vector3_1 += this.ApplyThrustModifiers(ref fuelType, ref thrust, ref connectedGroup.ThrustOverride, (MyResourceSinkComponentBase) connectedGroup.ResourceSink);
        }
      }
      this.m_lastControlThrustChanged = this.m_controlThrustChanged;
      this.m_controlThrustChanged = false;
      return vector3_1;
    }

    private void ComputeBaseThrust(
      ref Vector3 controlThrust,
      MyEntityThrustComponent.FuelTypeData fuelData,
      bool applyDampeners,
      Vector3 dampeningVelocity)
    {
      MyPhysicsComponentBase physics = this.Entity.Physics;
      if (physics == null)
      {
        fuelData.CurrentThrust = Vector3.Zero;
      }
      else
      {
        Matrix matrix = (Matrix) ref this.Entity.PositionComp.WorldMatrixNormalizedInv;
        Vector3 vector3_1 = physics.Gravity * 0.5f;
        Vector3 vector3_2 = Vector3.TransformNormal((applyDampeners ? physics.LinearVelocity : Vector3.Zero) + vector3_1, matrix);
        if ((double) vector3_2.LengthSquared() < 1.00000011116208E-06)
          vector3_2 = Vector3.Zero;
        Vector3 vector3_3 = dampeningVelocity;
        Vector3 zero1 = Vector3.Zero;
        Vector3 vector3_4 = Vector3.Zero;
        Vector3 vector3_5 = vector3_3 - vector3_2;
        if (!Vector3.IsZero(vector3_5) || !Vector3.IsZero(controlThrust) || !Vector3.IsZero(fuelData.ThrustOverride))
        {
          Vector3 zero2 = Vector3.Zero;
          if (this.DampenersEnabled)
          {
            Vector3 vector3_6 = Vector3.Abs(Vector3.Clamp(controlThrust, -Vector3.One, Vector3.One));
            Vector3 vector3_7 = (Vector3.One - vector3_6) * Vector3.IsZeroVector(fuelData.ThrustOverride);
            Vector3 zero3 = Vector3.Zero;
            if ((double) vector3_2.X > (double) vector3_3.X)
              zero3.X = this.m_totalMaxNegativeThrust.X;
            else if ((double) vector3_2.X < (double) vector3_3.X)
              zero3.X = this.m_totalMaxPositiveThrust.X;
            if ((double) vector3_2.Y > (double) vector3_3.Y)
              zero3.Y = this.m_totalMaxNegativeThrust.Y;
            else if ((double) vector3_2.Y < (double) vector3_3.Y)
              zero3.Y = this.m_totalMaxPositiveThrust.Y;
            if ((double) vector3_2.Z > (double) vector3_3.Z)
              zero3.Z = this.m_totalMaxNegativeThrust.Z;
            else if ((double) vector3_2.Z < (double) vector3_3.Z)
              zero3.Z = this.m_totalMaxPositiveThrust.Z;
            Vector3 zero4 = Vector3.Zero;
            if ((double) vector3_2.X > (double) vector3_3.X)
              zero4.X = fuelData.MaxNegativeThrust.X;
            else if ((double) vector3_2.X < (double) vector3_3.X)
              zero4.X = fuelData.MaxPositiveThrust.X;
            if ((double) vector3_2.Y > (double) vector3_3.Y)
              zero4.Y = fuelData.MaxNegativeThrust.Y;
            else if ((double) vector3_2.Y < (double) vector3_3.Y)
              zero4.Y = fuelData.MaxPositiveThrust.Y;
            if ((double) vector3_2.Z > (double) vector3_3.Z)
              zero4.Z = fuelData.MaxNegativeThrust.Z;
            else if ((double) vector3_2.Z < (double) vector3_3.Z)
              zero4.Z = fuelData.MaxPositiveThrust.Z;
            Vector3 vector3_8 = zero4 / zero3;
            if (!vector3_8.X.IsValid())
              vector3_8.X = 1f;
            if (!vector3_8.Y.IsValid())
              vector3_8.Y = 1f;
            if (!vector3_8.Z.IsValid())
              vector3_8.Z = 1f;
            Vector3 vector3_9 = vector3_7 * vector3_8;
            vector3_4 = vector3_5 / 0.5f * this.CalculateMass() * vector3_9;
          }
        }
        Vector3 vector3_10 = Vector3.Zero;
        if (!Vector3.IsZero(fuelData.MaxPositiveThrust) || !Vector3.IsZero(fuelData.MaxNegativeThrust))
        {
          Vector3 vector3_6 = Vector3.Clamp(controlThrust, Vector3.Zero, Vector3.One);
          vector3_10 = Vector3.Clamp(Vector3.Clamp(controlThrust, -Vector3.One, Vector3.Zero) * fuelData.MaxNegativeThrust + vector3_6 * fuelData.MaxPositiveThrust, -fuelData.MaxNegativeThrust, fuelData.MaxPositiveThrust);
        }
        Vector3 vector3_11 = Vector3.Clamp(vector3_10 + vector3_4, -fuelData.MaxNegativeThrust * this.SlowdownFactor, fuelData.MaxPositiveThrust * this.SlowdownFactor);
        if (!Vector3.IsZero(vector3_11))
        {
          this.m_controlThrustChanged = true;
          this.m_lastControlThrustChanged = this.m_controlThrustChanged;
        }
        fuelData.CurrentThrust = vector3_11;
      }
    }

    private void ComputeAiThrust(Vector3 direction, MyEntityThrustComponent.FuelTypeData fuelData)
    {
      MatrixD matrixD = this.Entity.PositionComp.WorldMatrixNormalizedInv;
      matrixD = matrixD.GetOrientation();
      Matrix matrix = (Matrix) ref matrixD;
      Vector3 vector3_1 = Vector3.Clamp(direction, Vector3.Zero, Vector3.One);
      Vector3 vector3_2 = Vector3.Clamp(direction, -Vector3.One, Vector3.Zero);
      Vector3 vector3_3 = Vector3.Clamp(-Vector3.Transform(this.Entity.Physics.Gravity, ref matrix) * this.Entity.Physics.Mass, Vector3.Zero, Vector3.PositiveInfinity);
      Vector3 vector3_4 = Vector3.Clamp(-Vector3.Transform(this.Entity.Physics.Gravity, ref matrix) * this.Entity.Physics.Mass, Vector3.NegativeInfinity, Vector3.Zero);
      Vector3 max = this.MaxThrustOverride.HasValue ? this.MaxThrustOverride.Value * Vector3I.Sign(fuelData.MaxPositiveThrust) : fuelData.MaxPositiveThrust;
      Vector3 vector3_5 = this.MaxThrustOverride.HasValue ? this.MaxThrustOverride.Value * Vector3I.Sign(fuelData.MaxNegativeThrust) : fuelData.MaxNegativeThrust;
      Vector3 vector3_6 = Vector3.Clamp(max - vector3_3, Vector3.Zero, Vector3.PositiveInfinity);
      Vector3 vector3_7 = Vector3.Clamp(vector3_5 + vector3_4, Vector3.Zero, Vector3.PositiveInfinity);
      Vector3 vector3_8 = vector3_6 * vector3_1;
      Vector3 vector3_9 = vector3_7 * -vector3_2;
      double num1 = (double) Math.Max(vector3_8.Max(), vector3_9.Max());
      Vector3 vector3_10 = Vector3.Zero;
      if (num1 > 1.0 / 1000.0)
      {
        Vector3 vector3_11 = vector3_1 * vector3_8;
        Vector3 vector3_12 = -vector3_2 * vector3_9;
        Vector3 vector3_13 = vector3_6 / vector3_11;
        Vector3 vector3_14 = vector3_7 / vector3_12;
        if (!vector3_13.X.IsValid())
          vector3_13.X = 1f;
        if (!vector3_13.Y.IsValid())
          vector3_13.Y = 1f;
        if (!vector3_13.Z.IsValid())
          vector3_13.Z = 1f;
        if (!vector3_14.X.IsValid())
          vector3_14.X = 1f;
        if (!vector3_14.Y.IsValid())
          vector3_14.Y = 1f;
        if (!vector3_14.Z.IsValid())
          vector3_14.Z = 1f;
        vector3_10 = Vector3.Clamp(-vector3_12 * vector3_14 + vector3_11 * vector3_13 + (vector3_3 + vector3_4), -vector3_5, max);
      }
      float num2 = MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT ? 0.25f : 0.5f;
      Vector3 vector = Vector3.Transform(this.Entity.Physics.LinearVelocity + this.Entity.Physics.Gravity / 2f, ref matrix);
      Vector3D vector3D1;
      if (!Vector3.IsZero(direction))
      {
        Vector3D vector3D2 = (Vector3D) Vector3.Normalize(direction);
        vector3D1 = (Vector3D) Vector3.Reject(vector, (Vector3) vector3D2);
      }
      else
        vector3D1 = (Vector3D) vector;
      Vector3D vector3D3 = -vector3D1 / (double) num2 * (double) this.Entity.Physics.Mass;
      Vector3 vector3_15 = Vector3.Clamp((Vector3) (vector3_10 + vector3D3), -vector3_5 * this.SlowdownFactor, max * this.SlowdownFactor);
      if (!Vector3.IsZero(vector3_15))
      {
        this.m_controlThrustChanged = true;
        this.m_lastControlThrustChanged = this.m_controlThrustChanged;
      }
      fuelData.CurrentThrust = vector3_15;
    }

    private void FlipNegativeInfinity(ref Vector3 v)
    {
      if (float.IsNegativeInfinity(v.X))
        v.X = float.PositiveInfinity;
      if (float.IsNegativeInfinity(v.Y))
        v.Y = float.PositiveInfinity;
      if (!float.IsNegativeInfinity(v.Z))
        return;
      v.Z = float.PositiveInfinity;
    }

    protected virtual Vector3 ApplyThrustModifiers(
      ref MyDefinitionId fuelType,
      ref Vector3 thrust,
      ref Vector3 thrustOverride,
      MyResourceSinkComponentBase resourceSink)
    {
      thrust += thrustOverride;
      thrust *= resourceSink.SuppliedRatioByType(fuelType);
      thrust *= MyFakes.THRUST_FORCE_RATIO;
      return thrust;
    }

    private void UpdatePowerAndThrustStrength(
      Vector3 thrust,
      MyDefinitionId fuelType,
      MyEntityThrustComponent.MyConveyorConnectedGroup group,
      bool updateThrust)
    {
      if (!this.m_controlThrustChanged && !this.m_lastControlThrustChanged)
        return;
      MyResourceSinkComponent resourceSink;
      MyEntityThrustComponent.FuelTypeData typeData;
      float thrustOverridePower;
      if (group == null)
      {
        int typeIndex = this.GetTypeIndex(ref fuelType);
        resourceSink = this.m_resourceSink;
        typeData = this.m_dataByFuelType[typeIndex];
        thrustOverridePower = this.m_totalThrustOverridePower;
        this.m_lastPowerUpdate = (long) MySession.Static.GameplayFrameCounter;
      }
      else
      {
        int typeIndex = group.GetTypeIndex(ref fuelType);
        resourceSink = group.ResourceSink;
        typeData = group.DataByFuelType[typeIndex];
        thrustOverridePower = group.ThrustOverridePower;
        group.LastPowerUpdate = (long) MySession.Static.GameplayFrameCounter;
      }
      Vector3 vector3_1 = Vector3.Zero;
      Vector3 vector3_2 = Vector3.Zero;
      if (!Vector3.IsZero(thrust) || (double) thrustOverridePower != 0.0)
      {
        Vector3 vector3_3 = thrust / (typeData.MaxPositiveThrust + 1E-07f);
        Vector3 vector3_4 = -thrust / (typeData.MaxNegativeThrust + 1E-07f);
        vector3_1 = Vector3.Clamp(vector3_3, Vector3.Zero, Vector3.One);
        vector3_2 = Vector3.Clamp(vector3_4, Vector3.Zero, Vector3.One);
        float powerAmount = 0.0f;
        if (this.Enabled)
          powerAmount = Math.Max((float) ((double) powerAmount + ((double) vector3_1.X > 0.0 ? (double) vector3_1.X * (double) this.GetMaxPowerRequirement(typeData, ref Vector3I.Left) : 0.0) + ((double) vector3_1.Y > 0.0 ? (double) vector3_1.Y * (double) this.GetMaxPowerRequirement(typeData, ref Vector3I.Down) : 0.0) + ((double) vector3_1.Z > 0.0 ? (double) vector3_1.Z * (double) this.GetMaxPowerRequirement(typeData, ref Vector3I.Forward) : 0.0) + ((double) vector3_2.X > 0.0 ? (double) vector3_2.X * (double) this.GetMaxPowerRequirement(typeData, ref Vector3I.Right) : 0.0) + ((double) vector3_2.Y > 0.0 ? (double) vector3_2.Y * (double) this.GetMaxPowerRequirement(typeData, ref Vector3I.Up) : 0.0) + ((double) vector3_2.Z > 0.0 ? (double) vector3_2.Z * (double) this.GetMaxPowerRequirement(typeData, ref Vector3I.Backward) : 0.0)) + thrustOverridePower, typeData.MinRequiredPowerInput);
        this.SetRequiredFuelInput(ref fuelType, this.PowerAmountToFuel(ref fuelType, powerAmount, group), group);
        if ((double) powerAmount > 9.99999974737875E-06)
          resourceSink.Update();
      }
      else
      {
        this.SetRequiredFuelInput(ref fuelType, this.PowerAmountToFuel(ref fuelType, typeData.MinRequiredPowerInput, group), group);
        resourceSink.Update();
      }
      if (!updateThrust)
        return;
      this.UpdateThrustStrength(typeData.ThrustsByDirection[Vector3I.Left], vector3_1.X);
      this.UpdateThrustStrength(typeData.ThrustsByDirection[Vector3I.Down], vector3_1.Y);
      this.UpdateThrustStrength(typeData.ThrustsByDirection[Vector3I.Forward], vector3_1.Z);
      this.UpdateThrustStrength(typeData.ThrustsByDirection[Vector3I.Right], vector3_2.X);
      this.UpdateThrustStrength(typeData.ThrustsByDirection[Vector3I.Up], vector3_2.Y);
      this.UpdateThrustStrength(typeData.ThrustsByDirection[Vector3I.Backward], vector3_2.Z);
    }

    private void RecalculatePlanetaryInfluence()
    {
      BoundingBoxD worldAabb = this.Entity.PositionComp.WorldAABB;
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref worldAabb);
      float num = 0.0f;
      if (closestPlanet != null)
      {
        num = closestPlanet.GetAirDensity(worldAabb.Center);
        this.m_lastPlanetaryInfluenceHasAtmosphere = closestPlanet.HasAtmosphere;
        this.m_lastPlanetaryGravityMagnitude = closestPlanet.Components.Get<MyGravityProviderComponent>().GetGravityMultiplier(this.Entity.PositionComp.WorldMatrixRef.Translation);
        this.m_nextPlanetaryInfluenceRecalculation = MySession.Static.GameplayFrameCounter + Math.Min(100, 10000);
      }
      else
        this.m_nextPlanetaryInfluenceRecalculation = MySession.Static.GameplayFrameCounter + Math.Min(1000, 10000);
      if ((double) this.m_lastPlanetaryInfluence == (double) num)
        return;
      this.MarkDirty();
      this.m_lastPlanetaryInfluence = num;
    }

    private void UpdateConveyorSystemChanges()
    {
      while (this.m_thrustEntitiesPending.Count > 0)
      {
        MyTuple<MyEntity, Vector3I, Func<bool>> myTuple = this.m_thrustEntitiesPending.Dequeue();
        if (this.IsThrustEntityType(myTuple.Item1))
        {
          if (this.m_thrustEntitiesRemovedBeforeRegister.Contains(myTuple.Item1))
            this.m_thrustEntitiesRemovedBeforeRegister.Remove(myTuple.Item1);
          else
            this.RegisterLazy(myTuple.Item1, myTuple.Item2, myTuple.Item3);
        }
      }
      while (this.m_conveyorSegmentsPending.Count > 0)
      {
        MyEntityThrustComponent.FindConnectedGroups(this.m_conveyorSegmentsPending.Dequeue(), this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
        if (MyEntityThrustComponent.m_tmpGroupIndices.Count > 1)
          this.MergeGroups(this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
        MyEntityThrustComponent.m_tmpGroupIndices.Clear();
      }
      while (this.m_conveyorEndpointsPending.Count > 0)
      {
        MyEntityThrustComponent.FindConnectedGroups(this.m_conveyorEndpointsPending.Dequeue(), this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
        if (MyEntityThrustComponent.m_tmpGroupIndices.Count > 1)
          this.MergeGroups(this.m_connectedGroups, MyEntityThrustComponent.m_tmpGroupIndices);
        MyEntityThrustComponent.m_tmpGroupIndices.Clear();
      }
      foreach (MyEntityThrustComponent.MyConveyorConnectedGroup groupOverride in this.m_groupsToTrySplit)
        this.TrySplitGroup((IMyConveyorEndpointBlock) null, groupOverride);
      this.m_groupsToTrySplit.Clear();
      if (!this.m_mergeAllGroupsDirty)
        return;
      this.TryMergeAllGroups();
      this.m_mergeAllGroupsDirty = false;
    }

    private void ConveyorSystem_OnPoweredChanged() => this.MergeAllGroupsDirty();

    public MyResourceSinkComponent ResourceSink(MyEntity thrustEntity)
    {
      MyEntityThrustComponent.MyConveyorConnectedGroup entityGroup = this.FindEntityGroup(thrustEntity);
      return entityGroup != null ? entityGroup.ResourceSink : this.m_resourceSink;
    }

    public void ResourceSinks(HashSet<MyResourceSinkComponent> outResourceSinks)
    {
      if (this.m_resourceSink != null)
        outResourceSinks.Add(this.m_resourceSink);
      foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
      {
        if (connectedGroup.ResourceSink != null)
          outResourceSinks.Add(connectedGroup.ResourceSink);
      }
    }

    private MyEntityThrustComponent.MyConveyorConnectedGroup FindEntityGroup(
      MyEntity thrustEntity)
    {
      MyEntityThrustComponent.MyConveyorConnectedGroup conveyorConnectedGroup = (MyEntityThrustComponent.MyConveyorConnectedGroup) null;
      if (!this.IsThrustEntityType(thrustEntity))
      {
        IMyConveyorEndpoint to = (IMyConveyorEndpoint) null;
        IMyConveyorEndpointBlock conveyorEndpointBlock = thrustEntity as IMyConveyorEndpointBlock;
        IMyConveyorSegmentBlock conveyorSegmentBlock = thrustEntity as IMyConveyorSegmentBlock;
        if (conveyorEndpointBlock != null)
          to = conveyorEndpointBlock.ConveyorEndpoint;
        else if (conveyorSegmentBlock != null && conveyorSegmentBlock.ConveyorSegment.ConveyorLine != null)
          to = conveyorSegmentBlock.ConveyorSegment.ConveyorLine.GetEndpoint(0) ?? conveyorSegmentBlock.ConveyorSegment.ConveyorLine.GetEndpoint(1);
        if (to != null)
        {
          for (int index = 0; index < this.m_connectedGroups.Count; ++index)
          {
            MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup = this.m_connectedGroups[index];
            if (MyGridConveyorSystem.Reachable(connectedGroup.FirstEndpoint, to))
            {
              conveyorConnectedGroup = connectedGroup;
              break;
            }
          }
        }
      }
      else
      {
        MyDefinitionId fuelId = this.FuelType(thrustEntity);
        if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(fuelId))
        {
          foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.m_connectedGroups)
          {
            int typeIndex;
            if (connectedGroup.TryGetTypeIndex(ref fuelId, out typeIndex))
            {
              foreach (HashSet<MyEntity> myEntitySet in connectedGroup.DataByFuelType[typeIndex].ThrustsByDirection.Values)
              {
                if (myEntitySet.Contains(thrustEntity))
                {
                  conveyorConnectedGroup = connectedGroup;
                  break;
                }
              }
              if (conveyorConnectedGroup != null)
                break;
            }
          }
        }
      }
      return conveyorConnectedGroup;
    }

    protected float GetMaxPowerRequirement(
      MyEntityThrustComponent.FuelTypeData typeData,
      ref Vector3I direction)
    {
      return typeData.MaxRequirementsByDirection[direction];
    }

    public bool IsDirty => this.m_thrustsChanged || this.m_controlThrustChanged;

    public virtual void MarkDirty(bool recomputePlanetaryInfluence = false)
    {
      this.m_thrustsChanged = true;
      this.m_controlThrustChanged = true;
      this.m_nextPlanetaryInfluenceRecalculation = 0;
    }

    private static float RequiredFuelInput(MyEntityThrustComponent.FuelTypeData typeData) => typeData.CurrentRequiredFuelInput;

    internal void SetRequiredFuelInput(
      ref MyDefinitionId fuelType,
      float newFuelInput,
      MyEntityThrustComponent.MyConveyorConnectedGroup group)
    {
      int typeIndex = 0;
      if (group == null && !this.TryGetTypeIndex(ref fuelType, out typeIndex) || group != null && !group.TryGetTypeIndex(ref fuelType, out typeIndex))
        return;
      (group != null ? group.DataByFuelType : this.m_dataByFuelType)[typeIndex].CurrentRequiredFuelInput = newFuelInput;
    }

    protected float PowerAmountToFuel(
      ref MyDefinitionId fuelType,
      float powerAmount,
      MyEntityThrustComponent.MyConveyorConnectedGroup group)
    {
      int typeIndex = 0;
      if (group == null && !this.TryGetTypeIndex(ref fuelType, out typeIndex) || group != null && !group.TryGetTypeIndex(ref fuelType, out typeIndex))
        return 0.0f;
      List<MyEntityThrustComponent.FuelTypeData> fuelTypeDataList = group != null ? group.DataByFuelType : this.m_dataByFuelType;
      return powerAmount / (fuelTypeDataList[typeIndex].Efficiency * fuelTypeDataList[typeIndex].EnergyDensity);
    }

    private bool TryGetTypeIndex(ref MyDefinitionId fuelId, out int typeIndex)
    {
      typeIndex = 0;
      return (this.m_fuelTypeToIndex.Count <= 1 || this.m_fuelTypeToIndex.TryGetValue(fuelId, out typeIndex)) && this.m_fuelTypeToIndex.Count > 0;
    }

    public bool IsThrustPoweredByType(MyEntity thrustEntity, ref MyDefinitionId fuelId) => this.ResourceSink(thrustEntity).IsPoweredByType(fuelId);

    protected int GetTypeIndex(ref MyDefinitionId fuelId)
    {
      int num1 = 0;
      int num2;
      if (this.m_fuelTypeToIndex.Count > 1 && this.m_fuelTypeToIndex.TryGetValue(fuelId, out num2))
        num1 = num2;
      return num1;
    }

    private void CubeGrid_OnBlockAdded(MySlimBlock addedBlock)
    {
      MyCubeBlock fatBlock = addedBlock.FatBlock;
      if (fatBlock == null)
        return;
      IMyConveyorEndpointBlock instance1 = fatBlock as IMyConveyorEndpointBlock;
      IMyConveyorSegmentBlock instance2 = fatBlock as IMyConveyorSegmentBlock;
      if (instance1 != null && !this.IsThrustEntityType(instance1 as MyEntity))
      {
        this.m_conveyorEndpointsPending.Enqueue(instance1);
      }
      else
      {
        if (instance2 == null)
          return;
        this.m_conveyorSegmentsPending.Enqueue(instance2);
      }
    }

    private void ConveyorSystem_OnBeforeRemoveSegmentBlock(
      IMyConveyorSegmentBlock conveyorSegmentBlock)
    {
      if (conveyorSegmentBlock == null)
        return;
      MyEntityThrustComponent.MyConveyorConnectedGroup entityGroup = this.FindEntityGroup(conveyorSegmentBlock as MyEntity);
      if (entityGroup == null)
        return;
      this.m_groupsToTrySplit.Add(entityGroup);
    }

    private void ConveyorSystem_OnBeforeRemoveEndpointBlock(
      IMyConveyorEndpointBlock conveyorEndpointBlock)
    {
      if (conveyorEndpointBlock == null || !this.IsThrustEntityType(conveyorEndpointBlock as MyEntity))
        return;
      MyEntityThrustComponent.MyConveyorConnectedGroup entityGroup = this.FindEntityGroup(conveyorEndpointBlock as MyEntity);
      if (entityGroup == null)
        return;
      this.m_groupsToTrySplit.Add(entityGroup);
    }

    protected virtual void OnControlTrustChanged()
    {
    }

    protected abstract void UpdateThrustStrength(HashSet<MyEntity> entities, float thrustForce);

    protected abstract bool RecomputeOverriddenParameters(
      MyEntity thrustEntity,
      MyEntityThrustComponent.FuelTypeData fuelData);

    protected abstract bool IsUsed(MyEntity thrustEntity);

    protected abstract float ForceMagnitude(
      MyEntity thrustEntity,
      float planetaryInfluence,
      bool inAtmosphere);

    protected abstract float CalculateForceMultiplier(
      MyEntity thrustEntity,
      float planetaryInfluence,
      bool inAtmosphere);

    protected abstract float CalculateConsumptionMultiplier(
      MyEntity thrustEntity,
      float naturalGravityStrength);

    protected abstract float MaxPowerConsumption(MyEntity thrustEntity);

    protected abstract float MinPowerConsumption(MyEntity thrustEntity);

    protected abstract MyDefinitionId FuelType(MyEntity thrustEntity);

    protected abstract bool IsThrustEntityType(MyEntity thrustEntity);

    protected abstract void RemoveFromGroup(
      MyEntity thrustEntity,
      MyEntityThrustComponent.MyConveyorConnectedGroup group);

    protected abstract void AddToGroup(
      MyEntity thrustEntity,
      MyEntityThrustComponent.MyConveyorConnectedGroup group);

    public override string ComponentTypeDebugString => "Thrust Component";

    public float GetLastThrustMultiplier(MyEntity thrustEntity) => this.CalculateForceMultiplier(thrustEntity, this.m_lastPlanetaryInfluence, this.m_lastPlanetaryInfluenceHasAtmosphere);

    protected virtual float CalculateMass() => this.Entity.Physics.Mass;

    public bool HasPower => this.m_resourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    public bool HasThrustersInAllDirections(MyDefinitionId fuelId)
    {
      int index;
      if (!this.m_fuelTypeToIndex.TryGetValue(fuelId, out index))
        return false;
      MyEntityThrustComponent.FuelTypeData fuelTypeData = this.m_dataByFuelType[index];
      return (1 & (fuelTypeData.ThrustsByDirection[Vector3I.Backward].Count > 0 ? 1 : 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Forward].Count > 0 ? 1 : 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Up].Count > 0 ? 1 : 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Down].Count > 0 ? 1 : 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Left].Count > 0 ? 1 : 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Right].Count > 0 ? 1 : 0)) != 0;
    }

    private void OnPlanetAddedOrRemoved(MyPlanet planet)
    {
      if (this.Entity == null)
        return;
      BoundingBoxD worldAabb = this.Entity.PositionComp.WorldAABB;
      if (!planet.IntersectsWithGravityFast(ref worldAabb))
        return;
      this.MarkDirty(true);
    }

    private void OnTeleport(MyEntity entity) => this.MarkDirty(true);

    public static void UpdateRelativeDampeningEntity(
      IMyControllableEntity controlledEntity,
      MyEntity dampeningEntity)
    {
      if (!Sync.IsServer || dampeningEntity == null || dampeningEntity.PositionComp.WorldAABB.DistanceSquared(controlledEntity.Entity.PositionComp.GetPosition()) <= (double) MyEntityThrustComponent.MAX_DISTANCE_RELATIVE_DAMPENING_SQ)
        return;
      controlledEntity.RelativeDampeningEntity = (MyEntity) null;
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.ClearDampeningEntity)), controlledEntity.Entity.EntityId);
    }

    public virtual void SetRelativeDampeningEntity(MyEntity entity)
    {
    }

    private class DirectionComparer : IEqualityComparer<Vector3I>
    {
      public bool Equals(Vector3I x, Vector3I y) => x == y;

      public int GetHashCode(Vector3I obj) => obj.X + 8 * obj.Y + 64 * obj.Z;
    }

    public class FuelTypeData
    {
      public Dictionary<Vector3I, HashSet<MyEntity>> ThrustsByDirection;
      public Dictionary<Vector3I, float> MaxRequirementsByDirection;
      public float CurrentRequiredFuelInput;
      public Vector3 MaxNegativeThrust;
      public Vector3 MaxPositiveThrust;
      public float MinRequiredPowerInput;
      public float MaxRequiredPowerInput;
      public int ThrustCount;
      public float Efficiency;
      public float EnergyDensity;
      public Vector3 CurrentThrust;
      public Vector3 ThrustOverride;
      public float ThrustOverridePower;
    }

    public class MyConveyorConnectedGroup
    {
      public readonly List<MyEntityThrustComponent.FuelTypeData> DataByFuelType;
      public readonly MyResourceSinkComponent ResourceSink;
      public int ThrustCount;
      public Vector3 MaxNegativeThrust;
      public Vector3 MaxPositiveThrust;
      public Vector3 ThrustOverride;
      public float ThrustOverridePower;
      public readonly List<MyDefinitionId> FuelTypes;
      public readonly Dictionary<MyDefinitionId, int> FuelTypeToIndex;
      public long LastPowerUpdate;
      public IMyConveyorEndpoint FirstEndpoint;

      public MyConveyorConnectedGroup(IMyConveyorEndpointBlock endpointBlock)
      {
        this.FirstEndpoint = endpointBlock.ConveyorEndpoint;
        this.DataByFuelType = new List<MyEntityThrustComponent.FuelTypeData>();
        this.ResourceSink = new MyResourceSinkComponent();
        this.LastPowerUpdate = (long) MySession.Static.GameplayFrameCounter;
        this.FuelTypes = new List<MyDefinitionId>();
        this.FuelTypeToIndex = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
      }

      public bool TryGetTypeIndex(ref MyDefinitionId fuelId, out int typeIndex)
      {
        typeIndex = 0;
        return (this.FuelTypeToIndex.Count <= 1 || this.FuelTypeToIndex.TryGetValue(fuelId, out typeIndex)) && this.FuelTypeToIndex.Count > 0;
      }

      public int GetTypeIndex(ref MyDefinitionId fuelId)
      {
        int num1 = 0;
        int num2;
        if (this.FuelTypeToIndex.Count > 1 && this.FuelTypeToIndex.TryGetValue(fuelId, out num2))
          num1 = num2;
        return num1;
      }
    }
  }
}
