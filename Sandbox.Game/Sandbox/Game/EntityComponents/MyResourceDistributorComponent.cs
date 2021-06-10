// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyResourceDistributorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
  public class MyResourceDistributorComponent : MyEntityComponentBase
  {
    public static readonly MyDefinitionId ElectricityId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Electricity");
    public static readonly MyDefinitionId HydrogenId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Hydrogen");
    public static readonly MyDefinitionId OxygenId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
    private int m_typeGroupCount;
    private bool m_forceRecalculation;
    private readonly List<MyResourceDistributorComponent.PerTypeData> m_dataPerType = new List<MyResourceDistributorComponent.PerTypeData>();
    protected readonly HashSet<MyDefinitionId> m_initializedTypes = new HashSet<MyDefinitionId>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly Dictionary<MyDefinitionId, int> m_typeIdToIndex = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly Dictionary<MyDefinitionId, bool> m_typeIdToConveyorConnectionRequired = new Dictionary<MyDefinitionId, bool>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly HashSet<MyDefinitionId> m_typesToRemove = new HashSet<MyDefinitionId>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly MyConcurrentHashSet<MyResourceSinkComponent> m_sinksToAdd = new MyConcurrentHashSet<MyResourceSinkComponent>();
    private readonly MyConcurrentHashSet<MyTuple<MyResourceSinkComponent, bool>> m_sinksToRemove = new MyConcurrentHashSet<MyTuple<MyResourceSinkComponent, bool>>();
    private readonly MyConcurrentHashSet<MyResourceSourceComponent> m_sourcesToAdd = new MyConcurrentHashSet<MyResourceSourceComponent>();
    private readonly MyConcurrentHashSet<MyResourceSourceComponent> m_sourcesToRemove = new MyConcurrentHashSet<MyResourceSourceComponent>();
    private readonly MyConcurrentDictionary<MyDefinitionId, int> m_changedTypes = new MyConcurrentDictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly List<string> m_changesDebug = new List<string>();
    public static bool ShowTrace = false;
    public string DebugName;
    private static int m_typeGroupCountTotal = -1;
    private static int m_sinkGroupPrioritiesTotal = -1;
    private static int m_sourceGroupPrioritiesTotal = -1;
    private static readonly Dictionary<MyDefinitionId, int> m_typeIdToIndexTotal = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private static readonly Dictionary<MyDefinitionId, bool> m_typeIdToConveyorConnectionRequiredTotal = new Dictionary<MyDefinitionId, bool>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private static readonly Dictionary<MyStringHash, int> m_sourceSubtypeToPriority = new Dictionary<MyStringHash, int>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
    private static readonly Dictionary<MyStringHash, int> m_sinkSubtypeToPriority = new Dictionary<MyStringHash, int>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
    private static readonly Dictionary<MyStringHash, bool> m_sinkSubtypeToAdaptability = new Dictionary<MyStringHash, bool>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
    public Action<bool> OnPowerGenerationChanged;
    private MyResourceStateEnum m_electricityState;
    private bool m_updateInProgress;
    private bool m_recomputeInProgress;

    public MyMultipleEnabledEnum SourcesEnabled => this.SourcesEnabledByType(MyResourceDistributorComponent.m_typeIdToIndexTotal.Keys.First<MyDefinitionId>());

    public MyResourceStateEnum ResourceState => this.ResourceStateByType(MyResourceDistributorComponent.m_typeIdToIndexTotal.Keys.First<MyDefinitionId>());

    public static int SinkGroupPrioritiesTotal => MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal;

    public static DictionaryReader<MyStringHash, int> SinkSubtypesToPriority => new DictionaryReader<MyStringHash, int>(MyResourceDistributorComponent.m_sinkSubtypeToPriority);

    public event Action SystemChanged;

    internal static void InitializeMappings()
    {
      lock (MyResourceDistributorComponent.m_typeIdToIndexTotal)
      {
        if (MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal >= 0 || MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal >= 0)
          return;
        ListReader<MyResourceDistributionGroupDefinition> definitionsOfType = MyDefinitionManager.Static.GetDefinitionsOfType<MyResourceDistributionGroupDefinition>();
        IOrderedEnumerable<MyResourceDistributionGroupDefinition> orderedEnumerable = definitionsOfType.OrderBy<MyResourceDistributionGroupDefinition, int>((Func<MyResourceDistributionGroupDefinition, int>) (def => def.Priority));
        if (definitionsOfType.Count > 0)
        {
          MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal = 0;
          MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal = 0;
        }
        foreach (MyResourceDistributionGroupDefinition distributionGroupDefinition in (IEnumerable<MyResourceDistributionGroupDefinition>) orderedEnumerable)
        {
          if (!distributionGroupDefinition.IsSource)
          {
            MyResourceDistributorComponent.m_sinkSubtypeToPriority.Add(distributionGroupDefinition.Id.SubtypeId, MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal++);
            MyResourceDistributorComponent.m_sinkSubtypeToAdaptability.Add(distributionGroupDefinition.Id.SubtypeId, distributionGroupDefinition.IsAdaptible);
          }
          else
            MyResourceDistributorComponent.m_sourceSubtypeToPriority.Add(distributionGroupDefinition.Id.SubtypeId, MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal++);
        }
        MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal = Math.Max(MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal, 1);
        MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal = Math.Max(MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal, 1);
        MyResourceDistributorComponent.m_sinkSubtypeToPriority.Add(MyStringHash.NullOrEmpty, MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal - 1);
        MyResourceDistributorComponent.m_sinkSubtypeToAdaptability.Add(MyStringHash.NullOrEmpty, false);
        MyResourceDistributorComponent.m_sourceSubtypeToPriority.Add(MyStringHash.NullOrEmpty, MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal - 1);
        MyResourceDistributorComponent.m_typeGroupCountTotal = 0;
        MyResourceDistributorComponent.m_typeIdToIndexTotal.Add(MyResourceDistributorComponent.ElectricityId, MyResourceDistributorComponent.m_typeGroupCountTotal++);
        MyResourceDistributorComponent.m_typeIdToConveyorConnectionRequiredTotal.Add(MyResourceDistributorComponent.ElectricityId, false);
        foreach (MyGasProperties myGasProperties in MyDefinitionManager.Static.GetDefinitionsOfType<MyGasProperties>())
        {
          MyResourceDistributorComponent.m_typeIdToIndexTotal.Add(myGasProperties.Id, MyResourceDistributorComponent.m_typeGroupCountTotal++);
          MyResourceDistributorComponent.m_typeIdToConveyorConnectionRequiredTotal.Add(myGasProperties.Id, true);
        }
      }
    }

    public static void Clear()
    {
      MyResourceDistributorComponent.m_typeGroupCountTotal = -1;
      MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal = -1;
      MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal = -1;
      MyResourceDistributorComponent.m_typeIdToIndexTotal.Clear();
      MyResourceDistributorComponent.m_typeIdToConveyorConnectionRequiredTotal.Clear();
      MyResourceDistributorComponent.m_sourceSubtypeToPriority.Clear();
      MyResourceDistributorComponent.m_sinkSubtypeToPriority.Clear();
      MyResourceDistributorComponent.m_sinkSubtypeToAdaptability.Clear();
    }

    private void InitializeNewType(ref MyDefinitionId typeId)
    {
      this.m_typeIdToIndex.Add(typeId, this.m_typeGroupCount++);
      this.m_typeIdToConveyorConnectionRequired.Add(typeId, MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref typeId));
      HashSet<MyResourceSinkComponent>[] resourceSinkComponentSetArray = new HashSet<MyResourceSinkComponent>[MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal];
      HashSet<MyResourceSourceComponent>[] resourceSourceComponentSetArray = new HashSet<MyResourceSourceComponent>[MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal];
      List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> myTupleList = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
      for (int index = 0; index < resourceSinkComponentSetArray.Length; ++index)
        resourceSinkComponentSetArray[index] = new HashSet<MyResourceSinkComponent>();
      for (int index = 0; index < resourceSourceComponentSetArray.Length; ++index)
        resourceSourceComponentSetArray[index] = new HashSet<MyResourceSourceComponent>();
      List<MyResourceDistributorComponent.MyPhysicalDistributionGroup> distributionGroupList = (List<MyResourceDistributorComponent.MyPhysicalDistributionGroup>) null;
      int num = 0;
      MyResourceDistributorComponent.MySinkGroupData[] mySinkGroupDataArray = (MyResourceDistributorComponent.MySinkGroupData[]) null;
      MyResourceDistributorComponent.MySourceGroupData[] mySourceGroupDataArray = (MyResourceDistributorComponent.MySourceGroupData[]) null;
      MyList<int> myList1 = (MyList<int>) null;
      MyList<int> myList2 = (MyList<int>) null;
      if (this.IsConveyorConnectionRequired(ref typeId))
      {
        distributionGroupList = new List<MyResourceDistributorComponent.MyPhysicalDistributionGroup>();
      }
      else
      {
        mySinkGroupDataArray = new MyResourceDistributorComponent.MySinkGroupData[MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal];
        mySourceGroupDataArray = new MyResourceDistributorComponent.MySourceGroupData[MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal];
        myList1 = new MyList<int>();
        myList2 = new MyList<int>();
      }
      this.m_dataPerType.Add(new MyResourceDistributorComponent.PerTypeData()
      {
        TypeId = typeId,
        SinkDataByPriority = mySinkGroupDataArray,
        SourceDataByPriority = mySourceGroupDataArray,
        InputOutputData = new MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData>(),
        SinksByPriority = resourceSinkComponentSetArray,
        SourcesByPriority = resourceSourceComponentSetArray,
        InputOutputList = myTupleList,
        StockpilingStorageIndices = myList1,
        OtherStorageIndices = myList2,
        DistributionGroups = distributionGroupList,
        DistributionGroupsInUse = num,
        NeedsRecompute = false,
        GroupsDirty = true,
        SourceCount = 0,
        RemainingFuelTime = 0.0f,
        RemainingFuelTimeDirty = true,
        MaxAvailableResource = 0.0f,
        SourcesEnabled = MyMultipleEnabledEnum.NoObjects,
        SourcesEnabledDirty = true,
        ResourceState = MyResourceStateEnum.NoPower
      });
      this.m_initializedTypes.Add(typeId);
      this.RaiseSystemChanged();
    }

    public MyResourceDistributorComponent(string debugName)
    {
      MyResourceDistributorComponent.InitializeMappings();
      this.DebugName = debugName;
      this.m_changesDebug.Clear();
    }

    public void MarkForUpdate() => this.m_forceRecalculation = true;

    private void RemoveTypesFromChanges(ListReader<MyDefinitionId> types)
    {
      foreach (MyDefinitionId type in types)
      {
        int num;
        if (this.m_changedTypes.TryGetValue(type, out num))
          this.m_changedTypes[type] = Math.Max(0, num - 1);
      }
    }

    public void AddSink(MyResourceSinkComponent sink)
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        this.m_changesDebug.Add(string.Format("+Sink: {0}", sink.Entity != null ? (object) sink.Entity.ToString() : (object) sink.Group.ToString()));
      MyTuple<MyResourceSinkComponent, bool> myTuple1 = new MyTuple<MyResourceSinkComponent, bool>();
      lock (this.m_sinksToRemove)
      {
        foreach (MyTuple<MyResourceSinkComponent, bool> myTuple2 in this.m_sinksToRemove)
        {
          if (myTuple2.Item1 == sink)
          {
            myTuple1 = myTuple2;
            break;
          }
        }
        if (myTuple1.Item1 != null)
        {
          this.m_sinksToRemove.Remove(myTuple1);
          this.RemoveTypesFromChanges(myTuple1.Item1.AcceptedResources);
          return;
        }
      }
      lock (this.m_sinksToAdd)
      {
        this.m_sinksToAdd.Add(sink);
        foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
        {
          if (this.m_initializedTypes.Contains(acceptedResource))
            this.GetSinksOfType(ref acceptedResource, sink.Group);
        }
      }
      foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
      {
        int num;
        if (!this.m_changedTypes.TryGetValue(acceptedResource, out num))
          this.m_changedTypes.Add(acceptedResource, 1);
        else
          this.m_changedTypes[acceptedResource] = num + 1;
      }
      this.RaiseSystemChanged();
    }

    public void RemoveSink(MyResourceSinkComponent sink, bool resetSinkInput = true, bool markedForClose = false)
    {
      if (markedForClose)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        this.m_changesDebug.Add(string.Format("-Sink: {0}", sink.Entity != null ? (object) sink.Entity.ToString() : (object) sink.Group.ToString()));
      lock (this.m_sinksToAdd)
      {
        if (this.m_sinksToAdd.Contains(sink))
        {
          this.m_sinksToAdd.Remove(sink);
          this.RemoveTypesFromChanges(sink.AcceptedResources);
          return;
        }
      }
      lock (this.m_sinksToRemove)
        this.m_sinksToRemove.Add(MyTuple.Create<MyResourceSinkComponent, bool>(sink, resetSinkInput));
      foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
      {
        int num;
        if (!this.m_changedTypes.TryGetValue(acceptedResource, out num))
          this.m_changedTypes.Add(acceptedResource, 1);
        else
          this.m_changedTypes[acceptedResource] = num + 1;
      }
      this.RaiseSystemChanged();
    }

    public void AddSource(MyResourceSourceComponent source)
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        this.m_changesDebug.Add(string.Format("+Source: {0}", source.Entity != null ? (object) source.Entity.ToString() : (object) source.Group.ToString()));
      lock (this.m_sourcesToRemove)
      {
        if (this.m_sourcesToRemove.Contains(source))
        {
          this.m_sourcesToRemove.Remove(source);
          this.RemoveTypesFromChanges(source.ResourceTypes);
          return;
        }
      }
      lock (this.m_sourcesToAdd)
        this.m_sourcesToAdd.Add(source);
      foreach (MyDefinitionId resourceType in source.ResourceTypes)
      {
        int num;
        if (!this.m_changedTypes.TryGetValue(resourceType, out num))
          this.m_changedTypes.Add(resourceType, 1);
        else
          this.m_changedTypes[resourceType] = num + 1;
      }
      this.RaiseSystemChanged();
    }

    public void RemoveSource(MyResourceSourceComponent source)
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        this.m_changesDebug.Add(string.Format("-Source: {0}", source.Entity != null ? (object) source.Entity.ToString() : (object) source.Group.ToString()));
      lock (this.m_sourcesToAdd)
      {
        if (this.m_sourcesToAdd.Contains(source))
        {
          this.m_sourcesToAdd.Remove(source);
          this.RemoveTypesFromChanges(source.ResourceTypes);
          return;
        }
      }
      lock (this.m_sourcesToRemove)
        this.m_sourcesToRemove.Add(source);
      foreach (MyDefinitionId resourceType in source.ResourceTypes)
      {
        int num;
        if (!this.m_changedTypes.TryGetValue(resourceType, out num))
          this.m_changedTypes.Add(resourceType, 1);
        else
          this.m_changedTypes[resourceType] = num + 1;
      }
      this.RaiseSystemChanged();
    }

    private void AddSinkLazy(MyResourceSinkComponent sink)
    {
      foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
      {
        if (!this.m_initializedTypes.Contains(acceptedResource))
          this.InitializeNewType(ref acceptedResource);
        HashSet<MyResourceSinkComponent> sinksOfType = this.GetSinksOfType(ref acceptedResource, sink.Group);
        if (sinksOfType != null)
        {
          MyResourceDistributorComponent.PerTypeData perTypeData = this.m_dataPerType[this.GetTypeIndex(ref acceptedResource)];
          MyResourceSourceComponent resourceSourceComponent1 = (MyResourceSourceComponent) null;
          if (sink.Container != null)
          {
            foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in perTypeData.SourcesByPriority)
            {
              foreach (MyResourceSourceComponent resourceSourceComponent2 in resourceSourceComponentSet)
              {
                if (resourceSourceComponent2.Container != null && resourceSourceComponent2.Container.Get<MyResourceSinkComponent>() == sink)
                {
                  perTypeData.InputOutputList.Add(MyTuple.Create<MyResourceSinkComponent, MyResourceSourceComponent>(sink, resourceSourceComponent2));
                  resourceSourceComponent1 = resourceSourceComponent2;
                  break;
                }
              }
              if (resourceSourceComponent1 != null)
              {
                resourceSourceComponentSet.Remove(resourceSourceComponent1);
                perTypeData.InvalidateGridForUpdateCache();
                break;
              }
            }
          }
          if (resourceSourceComponent1 == null)
          {
            sinksOfType.Add(sink);
            perTypeData.InvalidateGridForUpdateCache();
          }
          perTypeData.NeedsRecompute = true;
          perTypeData.GroupsDirty = true;
          perTypeData.RemainingFuelTimeDirty = true;
        }
      }
      sink.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.Sink_RequiredInputChanged);
      sink.ResourceAvailable += new MyResourceAvailableDelegate(this.Sink_IsResourceAvailable);
      sink.OnAddType += new Action<MyResourceSinkComponent, MyDefinitionId>(this.Sink_OnAddType);
      sink.OnRemoveType += new Action<MyResourceSinkComponent, MyDefinitionId>(this.Sink_OnRemoveType);
    }

    private void RemoveSinkLazy(MyResourceSinkComponent sink, bool resetSinkInput = true)
    {
      foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
      {
        HashSet<MyResourceSinkComponent> sinksOfType = this.GetSinksOfType(ref acceptedResource, sink.Group);
        if (sinksOfType != null)
        {
          MyResourceDistributorComponent.PerTypeData perTypeData = this.m_dataPerType[this.GetTypeIndex(ref acceptedResource)];
          if (!sinksOfType.Remove(sink))
          {
            int index1 = -1;
            for (int index2 = 0; index2 < perTypeData.InputOutputList.Count; ++index2)
            {
              if (perTypeData.InputOutputList[index2].Item1 == sink)
              {
                index1 = index2;
                break;
              }
            }
            if (index1 != -1)
            {
              MyResourceSourceComponent source = perTypeData.InputOutputList[index1].Item2;
              perTypeData.InputOutputList.RemoveAtFast<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>(index1);
              perTypeData.SourcesByPriority[MyResourceDistributorComponent.GetPriority(source)].Add(source);
              perTypeData.InvalidateGridForUpdateCache();
            }
          }
          else
            perTypeData.InvalidateGridForUpdateCache();
          perTypeData.NeedsRecompute = true;
          perTypeData.GroupsDirty = true;
          perTypeData.RemainingFuelTimeDirty = true;
          if (resetSinkInput)
            sink.SetInputFromDistributor(acceptedResource, 0.0f, MyResourceDistributorComponent.IsAdaptible(sink), true);
        }
      }
      sink.OnRemoveType -= new Action<MyResourceSinkComponent, MyDefinitionId>(this.Sink_OnRemoveType);
      sink.OnAddType -= new Action<MyResourceSinkComponent, MyDefinitionId>(this.Sink_OnAddType);
      sink.RequiredInputChanged -= new MyRequiredResourceChangeDelegate(this.Sink_RequiredInputChanged);
      sink.ResourceAvailable -= new MyResourceAvailableDelegate(this.Sink_IsResourceAvailable);
    }

    private void AddSourceLazy(MyResourceSourceComponent source)
    {
      foreach (MyDefinitionId resourceType in source.ResourceTypes)
      {
        if (!this.m_initializedTypes.Contains(resourceType))
          this.InitializeNewType(ref resourceType);
        HashSet<MyResourceSourceComponent> sourcesOfType = this.GetSourcesOfType(ref resourceType, source.Group);
        if (sourcesOfType != null)
        {
          MyResourceDistributorComponent.PerTypeData perTypeData = this.m_dataPerType[this.GetTypeIndex(ref resourceType)];
          MyResourceSinkComponent resourceSinkComponent1 = (MyResourceSinkComponent) null;
          if (source.Container != null)
          {
            foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in perTypeData.SinksByPriority)
            {
              foreach (MyResourceSinkComponent resourceSinkComponent2 in resourceSinkComponentSet)
              {
                if (resourceSinkComponent2.Container != null && resourceSinkComponent2.Container.Get<MyResourceSourceComponent>() == source)
                {
                  perTypeData.InputOutputList.Add(MyTuple.Create<MyResourceSinkComponent, MyResourceSourceComponent>(resourceSinkComponent2, source));
                  resourceSinkComponent1 = resourceSinkComponent2;
                  break;
                }
              }
              if (resourceSinkComponent1 != null)
              {
                resourceSinkComponentSet.Remove(resourceSinkComponent1);
                perTypeData.InvalidateGridForUpdateCache();
                break;
              }
            }
          }
          if (resourceSinkComponent1 == null)
          {
            sourcesOfType.Add(source);
            perTypeData.InvalidateGridForUpdateCache();
          }
          perTypeData.NeedsRecompute = true;
          perTypeData.GroupsDirty = true;
          ++perTypeData.SourceCount;
          if (perTypeData.SourceCount == 1)
            perTypeData.SourcesEnabled = source.Enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
          else if (perTypeData.SourcesEnabled == MyMultipleEnabledEnum.AllEnabled && !source.Enabled || perTypeData.SourcesEnabled == MyMultipleEnabledEnum.AllDisabled && source.Enabled)
            perTypeData.SourcesEnabled = MyMultipleEnabledEnum.Mixed;
          perTypeData.RemainingFuelTimeDirty = true;
        }
      }
      source.HasCapacityRemainingChanged += new MyResourceCapacityRemainingChangedDelegate(this.source_HasRemainingCapacityChanged);
      source.MaxOutputChanged += new MyResourceOutputChangedDelegate(this.source_MaxOutputChanged);
      source.ProductionEnabledChanged += new MyResourceCapacityRemainingChangedDelegate(this.source_ProductionEnabledChanged);
    }

    private void RemoveSourceLazy(MyResourceSourceComponent source)
    {
      foreach (MyDefinitionId resourceType in source.ResourceTypes)
      {
        HashSet<MyResourceSourceComponent> sourcesOfType = this.GetSourcesOfType(ref resourceType, source.Group);
        if (sourcesOfType != null)
        {
          MyResourceDistributorComponent.PerTypeData perTypeData = this.m_dataPerType[this.GetTypeIndex(ref resourceType)];
          if (!sourcesOfType.Remove(source))
          {
            int index1 = -1;
            for (int index2 = 0; index2 < perTypeData.InputOutputList.Count; ++index2)
            {
              if (perTypeData.InputOutputList[index2].Item2 == source)
              {
                index1 = index2;
                break;
              }
            }
            if (index1 != -1)
            {
              MyResourceSinkComponent sink = perTypeData.InputOutputList[index1].Item1;
              perTypeData.InputOutputList.RemoveAtFast<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>(index1);
              perTypeData.SinksByPriority[MyResourceDistributorComponent.GetPriority(sink)].Add(sink);
              perTypeData.InvalidateGridForUpdateCache();
            }
          }
          else
            perTypeData.InvalidateGridForUpdateCache();
          perTypeData.NeedsRecompute = true;
          perTypeData.GroupsDirty = true;
          --perTypeData.SourceCount;
          if (perTypeData.SourceCount == 0)
            perTypeData.SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
          else if (perTypeData.SourceCount == 1)
          {
            MyResourceSourceComponent firstSourceOfType = this.GetFirstSourceOfType(ref resourceType);
            if (firstSourceOfType != null)
            {
              this.ChangeSourcesState(resourceType, firstSourceOfType.Enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled, MySession.Static.LocalPlayerId);
            }
            else
            {
              --perTypeData.SourceCount;
              perTypeData.SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
            }
          }
          else if (perTypeData.SourcesEnabled == MyMultipleEnabledEnum.Mixed)
            perTypeData.SourcesEnabledDirty = true;
          perTypeData.RemainingFuelTimeDirty = true;
        }
      }
      source.ProductionEnabledChanged -= new MyResourceCapacityRemainingChangedDelegate(this.source_ProductionEnabledChanged);
      source.MaxOutputChanged -= new MyResourceOutputChangedDelegate(this.source_MaxOutputChanged);
      source.HasCapacityRemainingChanged -= new MyResourceCapacityRemainingChangedDelegate(this.source_HasRemainingCapacityChanged);
    }

    public int GetSourceCount(MyDefinitionId resourceTypeId, MyStringHash sourceGroupType)
    {
      int num = 0;
      int typeIndex;
      if (!this.TryGetTypeIndex(ref resourceTypeId, out typeIndex))
        return 0;
      int index = MyResourceDistributorComponent.m_sourceSubtypeToPriority[sourceGroupType];
      foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in this.m_dataPerType[typeIndex].InputOutputList)
      {
        if (inputOutput.Item2.Group == sourceGroupType && (double) inputOutput.Item2.CurrentOutputByType(resourceTypeId) > 0.0)
          ++num;
      }
      return this.m_dataPerType[typeIndex].SourceDataByPriority[index].ActiveCount + num;
    }

    public void UpdateBeforeSimulation()
    {
      this.CheckDistributionSystemChanges();
      foreach (MyDefinitionId key in this.m_typeIdToIndex.Keys)
      {
        if (this.m_forceRecalculation || this.NeedsRecompute(ref key))
          this.RecomputeResourceDistribution(ref key, false);
      }
      this.m_forceRecalculation = false;
      foreach (MyDefinitionId typeId in this.m_typesToRemove)
        this.RemoveType(ref typeId);
      int num = MyResourceDistributorComponent.ShowTrace ? 1 : 0;
    }

    public virtual void UpdateBeforeSimulation100()
    {
      MyResourceStateEnum state = this.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
      if (this.m_electricityState == state)
        return;
      if (this.PowerStateIsOk(state) != this.PowerStateIsOk(this.m_electricityState))
        this.ConveyorSystem_OnPoweredChanged();
      bool flag = this.PowerStateWorks(state);
      if (this.OnPowerGenerationChanged != null && flag != this.PowerStateWorks(this.m_electricityState))
        this.OnPowerGenerationChanged(flag);
      this.ConveyorSystem_OnPoweredChanged();
      this.m_electricityState = state;
    }

    protected bool PowerStateIsOk(MyResourceStateEnum state) => state == MyResourceStateEnum.Ok;

    protected bool PowerStateWorks(MyResourceStateEnum state) => state != MyResourceStateEnum.NoPower;

    public void UpdateHud(MyHudSinkGroupInfo info)
    {
      bool flag = true;
      int num = 0;
      int groupIndex = 0;
      int typeIndex;
      if (!this.TryGetTypeIndex(MyResourceDistributorComponent.ElectricityId, out typeIndex))
        return;
      for (; groupIndex < this.m_dataPerType[typeIndex].SinkDataByPriority.Length; ++groupIndex)
      {
        if (flag && (double) this.m_dataPerType[typeIndex].SinkDataByPriority[groupIndex].RemainingAvailableResource < (double) this.m_dataPerType[typeIndex].SinkDataByPriority[groupIndex].RequiredInput && !this.m_dataPerType[typeIndex].SinkDataByPriority[groupIndex].IsAdaptible)
          flag = false;
        if (flag)
          ++num;
        info.SetGroupDeficit(groupIndex, Math.Max(this.m_dataPerType[typeIndex].SinkDataByPriority[groupIndex].RequiredInput - this.m_dataPerType[typeIndex].SinkDataByPriority[groupIndex].RemainingAvailableResource, 0.0f));
      }
      info.WorkingGroupCount = num;
    }

    public void ChangeSourcesState(
      MyDefinitionId resourceTypeId,
      MyMultipleEnabledEnum state,
      long playerId)
    {
      int typeIndex;
      if (this.m_recomputeInProgress || !this.TryGetTypeIndex(resourceTypeId, out typeIndex) || (this.m_dataPerType[typeIndex].SourcesEnabled == state || this.m_dataPerType[typeIndex].SourcesEnabled == MyMultipleEnabledEnum.NoObjects))
        return;
      this.m_recomputeInProgress = true;
      this.m_dataPerType[typeIndex].SourcesEnabled = state;
      bool newValue = state == MyMultipleEnabledEnum.AllEnabled;
      IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(playerId);
      bool flag = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
      AdminSettingsEnum adminSettingsEnum;
      if (MySession.Static.RemoteAdminSettings.TryGetValue(MySession.Static.Players.TryGetSteamId(playerId), out adminSettingsEnum))
        flag |= adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
      foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in this.m_dataPerType[typeIndex].SourcesByPriority)
      {
        foreach (MyResourceSourceComponent resourceSourceComponent in resourceSourceComponentSet)
        {
          if (!flag && playerId >= 0L && (resourceSourceComponent.Entity != null && resourceSourceComponent.Entity is MyFunctionalBlock entity) && (entity.OwnerId != 0L && entity.OwnerId != playerId))
          {
            MyOwnershipShareModeEnum shareMode = entity.IDModule.ShareMode;
            IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(entity.OwnerId);
            switch (shareMode)
            {
              case MyOwnershipShareModeEnum.None:
                continue;
              case MyOwnershipShareModeEnum.Faction:
                if (playerFaction1 == null || playerFaction2 != null && playerFaction1.FactionId != playerFaction2.FactionId)
                  continue;
                break;
            }
          }
          if (playerId == -2L && resourceSourceComponent.Container.Entity is MyTerminalBlock entity)
          {
            string str = entity.CustomName.ToString();
            if (str != "Special Content" && str != "Special Content Power")
              continue;
          }
          resourceSourceComponent.MaxOutputChanged -= new MyResourceOutputChangedDelegate(this.source_MaxOutputChanged);
          resourceSourceComponent.SetEnabled(newValue, false);
          resourceSourceComponent.MaxOutputChanged += new MyResourceOutputChangedDelegate(this.source_MaxOutputChanged);
        }
      }
      foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in this.m_dataPerType[typeIndex].InputOutputList)
      {
        MyResourceSourceComponent resourceSourceComponent = inputOutput.Item2;
        if (!flag && playerId >= 0L && (resourceSourceComponent.Entity != null && resourceSourceComponent.Entity is MyFunctionalBlock entity) && (entity.OwnerId != 0L && entity.OwnerId != playerId))
        {
          MyOwnershipShareModeEnum shareMode = entity.IDModule.ShareMode;
          IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(entity.OwnerId);
          switch (shareMode)
          {
            case MyOwnershipShareModeEnum.None:
              continue;
            case MyOwnershipShareModeEnum.Faction:
              if (playerFaction1 == null || playerFaction2 != null && playerFaction1.FactionId != playerFaction2.FactionId)
                continue;
              break;
          }
        }
        resourceSourceComponent.MaxOutputChanged -= new MyResourceOutputChangedDelegate(this.source_MaxOutputChanged);
        resourceSourceComponent.SetEnabled(newValue, false);
        resourceSourceComponent.MaxOutputChanged += new MyResourceOutputChangedDelegate(this.source_MaxOutputChanged);
      }
      this.m_dataPerType[typeIndex].SourcesEnabledDirty = false;
      this.m_dataPerType[typeIndex].NeedsRecompute = true;
      this.m_recomputeInProgress = false;
    }

    private float ComputeRemainingFuelTime(MyDefinitionId resourceTypeId)
    {
      try
      {
        int typeIndex = this.GetTypeIndex(ref resourceTypeId);
        if ((double) this.m_dataPerType[typeIndex].MaxAvailableResource == 0.0)
          return 0.0f;
        float num1 = 0.0f;
        foreach (MyResourceDistributorComponent.MySinkGroupData mySinkGroupData in this.m_dataPerType[typeIndex].SinkDataByPriority)
        {
          if ((double) mySinkGroupData.RemainingAvailableResource >= (double) mySinkGroupData.RequiredInput)
            num1 += mySinkGroupData.RequiredInput;
          else if (mySinkGroupData.IsAdaptible)
            num1 += mySinkGroupData.RemainingAvailableResource;
          else
            break;
        }
        float num2 = (double) this.m_dataPerType[typeIndex].InputOutputData.Item1.RemainingAvailableResource <= (double) this.m_dataPerType[typeIndex].InputOutputData.Item1.RequiredInput ? num1 + this.m_dataPerType[typeIndex].InputOutputData.Item1.RemainingAvailableResource : num1 + this.m_dataPerType[typeIndex].InputOutputData.Item1.RequiredInput;
        bool flag1 = false;
        bool flag2 = false;
        float num3 = 0.0f;
        for (int index = 0; index < this.m_dataPerType[typeIndex].SourcesByPriority.Length; ++index)
        {
          MyResourceDistributorComponent.MySourceGroupData mySourceGroupData = this.m_dataPerType[typeIndex].SourceDataByPriority[index];
          if ((double) mySourceGroupData.UsageRatio > 0.0)
          {
            if (mySourceGroupData.InfiniteCapacity)
            {
              flag1 = true;
              num2 -= mySourceGroupData.UsageRatio * mySourceGroupData.MaxAvailableResource;
            }
            else
            {
              foreach (MyResourceSourceComponent resourceSourceComponent in this.m_dataPerType[typeIndex].SourcesByPriority[index])
              {
                if (resourceSourceComponent.Enabled && resourceSourceComponent.ProductionEnabledByType(resourceTypeId) && resourceSourceComponent.CountTowardsRemainingEnergyTime)
                {
                  flag2 = true;
                  num3 += resourceSourceComponent.RemainingCapacityByType(resourceTypeId);
                }
              }
            }
          }
        }
        if ((double) this.m_dataPerType[typeIndex].InputOutputData.Item2.UsageRatio > 0.0)
        {
          foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in this.m_dataPerType[typeIndex].InputOutputList)
          {
            if (inputOutput.Item2.Enabled && inputOutput.Item2.ProductionEnabledByType(resourceTypeId))
            {
              flag2 = true;
              num3 += inputOutput.Item2.RemainingCapacityByType(resourceTypeId);
            }
          }
        }
        return flag1 && !flag2 || (double) num2 <= 0.0 ? float.PositiveInfinity : num3 / num2;
      }
      finally
      {
      }
    }

    private void RefreshSourcesEnabled(MyDefinitionId resourceTypeId)
    {
      int typeIndex;
      if (!this.TryGetTypeIndex(ref resourceTypeId, out typeIndex))
        return;
      this.m_dataPerType[typeIndex].SourcesEnabledDirty = false;
      if (this.m_dataPerType[typeIndex].SourceCount == 0)
      {
        this.m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
      }
      else
      {
        bool flag1 = true;
        bool flag2 = true;
        foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in this.m_dataPerType[typeIndex].SourcesByPriority)
        {
          foreach (MyResourceSourceComponent resourceSourceComponent in resourceSourceComponentSet)
          {
            flag1 = flag1 && resourceSourceComponent.Enabled;
            flag2 = flag2 && !resourceSourceComponent.Enabled;
            if (!flag1 && !flag2)
            {
              this.m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.Mixed;
              return;
            }
          }
        }
        foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in this.m_dataPerType[typeIndex].InputOutputList)
        {
          flag1 = flag1 && inputOutput.Item2.Enabled;
          flag2 = flag2 && !inputOutput.Item2.Enabled;
          if (!flag1 && !flag2)
          {
            this.m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.Mixed;
            return;
          }
        }
        this.m_dataPerType[typeIndex].SourcesEnabled = flag2 ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled;
      }
    }

    internal void CubeGrid_OnFatBlockAddedOrRemoved(MyCubeBlock fatblock)
    {
      IMyConveyorEndpointBlock conveyorEndpointBlock = fatblock as IMyConveyorEndpointBlock;
      IMyConveyorSegmentBlock conveyorSegmentBlock = fatblock as IMyConveyorSegmentBlock;
      if (conveyorEndpointBlock == null && conveyorSegmentBlock == null)
        return;
      foreach (MyResourceDistributorComponent.PerTypeData perTypeData in this.m_dataPerType)
      {
        perTypeData.GroupsDirty = true;
        perTypeData.NeedsRecompute = true;
      }
    }

    private void CheckDistributionSystemChanges()
    {
      if (this.m_sinksToRemove.Count > 0)
      {
        lock (this.m_sinksToRemove)
        {
          foreach (MyTuple<MyResourceSinkComponent, bool> myTuple in this.m_sinksToRemove.ToArray<MyTuple<MyResourceSinkComponent, bool>>())
          {
            this.RemoveSinkLazy(myTuple.Item1, myTuple.Item2);
            foreach (MyDefinitionId acceptedResource in myTuple.Item1.AcceptedResources)
              this.m_changedTypes[acceptedResource] = Math.Max(0, this.m_changedTypes[acceptedResource] - 1);
          }
          this.m_sinksToRemove.Clear();
        }
      }
      if (this.m_sourcesToRemove.Count > 0)
      {
        lock (this.m_sourcesToRemove)
        {
          foreach (MyResourceSourceComponent source in this.m_sourcesToRemove)
          {
            this.RemoveSourceLazy(source);
            foreach (MyDefinitionId resourceType in source.ResourceTypes)
              this.m_changedTypes[resourceType] = Math.Max(0, this.m_changedTypes[resourceType] - 1);
          }
          this.m_sourcesToRemove.Clear();
        }
      }
      if (this.m_sourcesToAdd.Count > 0)
      {
        lock (this.m_sourcesToAdd)
        {
          foreach (MyResourceSourceComponent source in this.m_sourcesToAdd)
          {
            this.AddSourceLazy(source);
            foreach (MyDefinitionId resourceType in source.ResourceTypes)
              this.m_changedTypes[resourceType] = Math.Max(0, this.m_changedTypes[resourceType] - 1);
          }
          this.m_sourcesToAdd.Clear();
        }
      }
      if (this.m_sinksToAdd.Count <= 0)
        return;
      lock (this.m_sinksToAdd)
      {
        foreach (MyResourceSinkComponent sink in this.m_sinksToAdd)
        {
          this.AddSinkLazy(sink);
          foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
            this.m_changedTypes[acceptedResource] = Math.Max(0, this.m_changedTypes[acceptedResource] - 1);
        }
        this.m_sinksToAdd.Clear();
      }
    }

    private void RecomputeResourceDistribution(ref MyDefinitionId typeId, bool updateChanges = true)
    {
      if (this.m_recomputeInProgress)
        return;
      this.m_recomputeInProgress = true;
      if (updateChanges && !this.m_updateInProgress)
      {
        this.m_updateInProgress = true;
        this.CheckDistributionSystemChanges();
        this.m_updateInProgress = false;
      }
      MyResourceDistributorComponent.PerTypeData perTypeData = this.m_dataPerType[this.GetTypeIndex(ref typeId)];
      if (perTypeData.SinksByPriority.Length == 0 && perTypeData.SourcesByPriority.Length == 0 && perTypeData.InputOutputList.Count == 0)
      {
        this.m_typesToRemove.Add(typeId);
        this.m_recomputeInProgress = false;
      }
      else
      {
        if (MySession.Static.SimplifiedSimulation)
        {
          bool flag = false;
          if (this.PowerStateWorks(this.m_electricityState) || typeId == MyResourceDistributorComponent.ElectricityId)
          {
            foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in perTypeData.InputOutputList)
            {
              MyResourceSourceComponent resourceSourceComponent = inputOutput.Item2;
              if (resourceSourceComponent.Enabled && (double) resourceSourceComponent.MaxOutputByType(typeId) > 0.0)
              {
                flag = true;
                goto label_22;
              }
            }
            foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in perTypeData.SourcesByPriority)
            {
              foreach (MyResourceSourceComponent resourceSourceComponent in resourceSourceComponentSet)
              {
                if (resourceSourceComponent.Enabled && (double) resourceSourceComponent.MaxOutputByType(typeId) > 0.0)
                {
                  flag = true;
                  goto label_22;
                }
              }
            }
          }
label_22:
          foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in perTypeData.SinksByPriority)
          {
            foreach (MyResourceSinkComponent resourceSinkComponent in resourceSinkComponentSet)
            {
              float newResourceInput = flag ? resourceSinkComponent.RequiredInputByType(typeId) : 0.0f;
              resourceSinkComponent.SetInputFromDistributor(typeId, newResourceInput, true, true);
            }
          }
          foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in perTypeData.InputOutputList)
          {
            MyResourceSinkComponent resourceSinkComponent = inputOutput.Item1;
            float newResourceInput = flag ? resourceSinkComponent.RequiredInputByType(typeId) : 0.0f;
            resourceSinkComponent.SetInputFromDistributor(typeId, newResourceInput, true, true);
          }
          float num = flag ? float.PositiveInfinity : 0.0f;
          perTypeData.RemainingFuelTimeDirty = false;
          perTypeData.RemainingFuelTime = num;
          perTypeData.ResourceState = flag ? MyResourceStateEnum.Ok : MyResourceStateEnum.NoPower;
          MyResourceDistributorComponent.MySinkGroupData[] sinkDataByPriority = perTypeData.SinkDataByPriority;
          for (int index = 0; index < (sinkDataByPriority != null ? sinkDataByPriority.Length : 0); ++index)
            sinkDataByPriority[index].RemainingAvailableResource = num;
        }
        else if (!this.IsConveyorConnectionRequired(ref typeId))
        {
          MyResourceDistributorComponent.ComputeInitialDistributionData(ref typeId, perTypeData.SinkDataByPriority, perTypeData.SourceDataByPriority, ref perTypeData.InputOutputData, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList, perTypeData.StockpilingStorageIndices, perTypeData.OtherStorageIndices, out perTypeData.MaxAvailableResource);
          perTypeData.ResourceState = MyResourceDistributorComponent.RecomputeResourceDistributionPartial(ref typeId, 0, perTypeData.SinkDataByPriority, perTypeData.SourceDataByPriority, ref perTypeData.InputOutputData, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList, perTypeData.StockpilingStorageIndices, perTypeData.OtherStorageIndices, perTypeData.MaxAvailableResource);
        }
        else
        {
          if (perTypeData.GroupsDirty)
          {
            perTypeData.GroupsDirty = false;
            perTypeData.DistributionGroupsInUse = 0;
            this.RecreatePhysicalDistributionGroups(ref typeId, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList);
          }
          perTypeData.MaxAvailableResource = 0.0f;
          for (int index = 0; index < perTypeData.DistributionGroupsInUse; ++index)
          {
            MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = perTypeData.DistributionGroups[index];
            MyResourceDistributorComponent.ComputeInitialDistributionData(ref typeId, distributionGroup.SinkDataByPriority, distributionGroup.SourceDataByPriority, ref distributionGroup.InputOutputData, distributionGroup.SinksByPriority, distributionGroup.SourcesByPriority, distributionGroup.SinkSourcePairs, distributionGroup.StockpilingStorage, distributionGroup.OtherStorage, out distributionGroup.MaxAvailableResources);
            distributionGroup.ResourceState = MyResourceDistributorComponent.RecomputeResourceDistributionPartial(ref typeId, 0, distributionGroup.SinkDataByPriority, distributionGroup.SourceDataByPriority, ref distributionGroup.InputOutputData, distributionGroup.SinksByPriority, distributionGroup.SourcesByPriority, distributionGroup.SinkSourcePairs, distributionGroup.StockpilingStorage, distributionGroup.OtherStorage, distributionGroup.MaxAvailableResources);
            perTypeData.MaxAvailableResource += distributionGroup.MaxAvailableResources;
            perTypeData.DistributionGroups[index] = distributionGroup;
          }
          MyResourceStateEnum resourceStateEnum;
          if ((double) perTypeData.MaxAvailableResource == 0.0)
          {
            resourceStateEnum = MyResourceStateEnum.NoPower;
          }
          else
          {
            resourceStateEnum = MyResourceStateEnum.Ok;
            for (int index = 0; index < perTypeData.DistributionGroupsInUse; ++index)
            {
              if (perTypeData.DistributionGroups[index].ResourceState == MyResourceStateEnum.OverloadAdaptible)
              {
                resourceStateEnum = MyResourceStateEnum.OverloadAdaptible;
                break;
              }
              if (perTypeData.DistributionGroups[index].ResourceState == MyResourceStateEnum.OverloadBlackout)
              {
                resourceStateEnum = MyResourceStateEnum.OverloadAdaptible;
                break;
              }
            }
          }
          perTypeData.ResourceState = resourceStateEnum;
        }
        perTypeData.NeedsRecompute = false;
        this.m_recomputeInProgress = false;
      }
    }

    private void RecreatePhysicalDistributionGroups(
      ref MyDefinitionId typeId,
      HashSet<MyResourceSinkComponent>[] allSinksByPriority,
      HashSet<MyResourceSourceComponent>[] allSourcesByPriority,
      List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> allSinkSources)
    {
      foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in allSourcesByPriority)
      {
        foreach (MyResourceSourceComponent source in resourceSourceComponentSet)
        {
          if (source.Entity == null)
          {
            if (source.TemporaryConnectedEntity != null)
              this.SetEntityGroupForTempConnected(ref typeId, source);
          }
          else
            this.SetEntityGroup(ref typeId, source.Entity);
        }
      }
      foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in allSinksByPriority)
      {
        foreach (MyResourceSinkComponent sink in resourceSinkComponentSet)
        {
          if (sink.Entity == null)
          {
            if (sink.TemporaryConnectedEntity != null)
              this.SetEntityGroupForTempConnected(ref typeId, sink);
          }
          else
            this.SetEntityGroup(ref typeId, (IMyEntity) sink.Entity);
        }
      }
      foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> allSinkSource in allSinkSources)
      {
        if (allSinkSource.Item1.Entity != null)
          this.SetEntityGroup(ref typeId, (IMyEntity) allSinkSource.Item1.Entity);
      }
    }

    private void SetEntityGroup(ref MyDefinitionId typeId, IMyEntity entity)
    {
      if (!(entity is IMyConveyorEndpointBlock conveyorEndpointBlock))
        return;
      int typeIndex = this.GetTypeIndex(ref typeId);
      bool flag = false;
      for (int index = 0; index < this.m_dataPerType[typeIndex].DistributionGroupsInUse; ++index)
      {
        if (MyGridConveyorSystem.Reachable(this.m_dataPerType[typeIndex].DistributionGroups[index].FirstEndpoint, conveyorEndpointBlock.ConveyorEndpoint))
        {
          MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[typeIndex].DistributionGroups[index];
          distributionGroup.Add(typeId, conveyorEndpointBlock);
          this.m_dataPerType[typeIndex].DistributionGroups[index] = distributionGroup;
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      if (++this.m_dataPerType[typeIndex].DistributionGroupsInUse > this.m_dataPerType[typeIndex].DistributionGroups.Count)
      {
        this.m_dataPerType[typeIndex].DistributionGroups.Add(new MyResourceDistributorComponent.MyPhysicalDistributionGroup(typeId, conveyorEndpointBlock));
      }
      else
      {
        MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[typeIndex].DistributionGroups[this.m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
        distributionGroup.Init(typeId, conveyorEndpointBlock);
        this.m_dataPerType[typeIndex].DistributionGroups[this.m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = distributionGroup;
      }
    }

    private void SetEntityGroupForTempConnected(
      ref MyDefinitionId typeId,
      MyResourceSinkComponent sink)
    {
      IMyConveyorEndpointBlock temporaryConnectedEntity = sink.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
      int typeIndex = this.GetTypeIndex(ref typeId);
      bool flag1 = false;
      for (int index = 0; index < this.m_dataPerType[typeIndex].DistributionGroupsInUse; ++index)
      {
        if (temporaryConnectedEntity == null || !MyGridConveyorSystem.Reachable(this.m_dataPerType[typeIndex].DistributionGroups[index].FirstEndpoint, temporaryConnectedEntity.ConveyorEndpoint))
        {
          bool flag2 = false;
          if (temporaryConnectedEntity == null)
          {
            foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in this.m_dataPerType[typeIndex].DistributionGroups[index].SourcesByPriority)
            {
              foreach (MyResourceSourceComponent resourceSourceComponent in resourceSourceComponentSet)
              {
                if (sink.TemporaryConnectedEntity == resourceSourceComponent.TemporaryConnectedEntity)
                {
                  flag2 = true;
                  break;
                }
              }
              if (flag2)
                break;
            }
          }
          if (!flag2)
            continue;
        }
        MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[typeIndex].DistributionGroups[index];
        distributionGroup.AddTempConnected(typeId, sink);
        this.m_dataPerType[typeIndex].DistributionGroups[index] = distributionGroup;
        flag1 = true;
        break;
      }
      if (flag1)
        return;
      if (++this.m_dataPerType[typeIndex].DistributionGroupsInUse > this.m_dataPerType[typeIndex].DistributionGroups.Count)
      {
        this.m_dataPerType[typeIndex].DistributionGroups.Add(new MyResourceDistributorComponent.MyPhysicalDistributionGroup(typeId, sink));
      }
      else
      {
        MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[typeIndex].DistributionGroups[this.m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
        distributionGroup.InitFromTempConnected(typeId, sink);
        this.m_dataPerType[typeIndex].DistributionGroups[this.m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = distributionGroup;
      }
    }

    private void SetEntityGroupForTempConnected(
      ref MyDefinitionId typeId,
      MyResourceSourceComponent source)
    {
      IMyConveyorEndpointBlock temporaryConnectedEntity = source.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
      int typeIndex = this.GetTypeIndex(ref typeId);
      bool flag1 = false;
      for (int index = 0; index < this.m_dataPerType[typeIndex].DistributionGroupsInUse; ++index)
      {
        if (temporaryConnectedEntity == null || !MyGridConveyorSystem.Reachable(this.m_dataPerType[typeIndex].DistributionGroups[index].FirstEndpoint, temporaryConnectedEntity.ConveyorEndpoint))
        {
          bool flag2 = false;
          if (temporaryConnectedEntity == null)
          {
            foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in this.m_dataPerType[typeIndex].DistributionGroups[index].SinksByPriority)
            {
              foreach (MyResourceSinkComponent resourceSinkComponent in resourceSinkComponentSet)
              {
                if (source.TemporaryConnectedEntity == resourceSinkComponent.TemporaryConnectedEntity)
                {
                  flag2 = true;
                  break;
                }
              }
              if (flag2)
                break;
            }
          }
          if (!flag2)
            continue;
        }
        MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[typeIndex].DistributionGroups[index];
        distributionGroup.AddTempConnected(typeId, source);
        this.m_dataPerType[typeIndex].DistributionGroups[index] = distributionGroup;
        flag1 = true;
        break;
      }
      if (flag1)
        return;
      if (++this.m_dataPerType[typeIndex].DistributionGroupsInUse > this.m_dataPerType[typeIndex].DistributionGroups.Count)
      {
        this.m_dataPerType[typeIndex].DistributionGroups.Add(new MyResourceDistributorComponent.MyPhysicalDistributionGroup(typeId, source));
      }
      else
      {
        MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[typeIndex].DistributionGroups[this.m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
        distributionGroup.InitFromTempConnected(typeId, source);
        this.m_dataPerType[typeIndex].DistributionGroups[this.m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = distributionGroup;
      }
    }

    private static void ComputeInitialDistributionData(
      ref MyDefinitionId typeId,
      MyResourceDistributorComponent.MySinkGroupData[] sinkDataByPriority,
      MyResourceDistributorComponent.MySourceGroupData[] sourceDataByPriority,
      ref MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData> sinkSourceData,
      HashSet<MyResourceSinkComponent>[] sinksByPriority,
      HashSet<MyResourceSourceComponent>[] sourcesByPriority,
      List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs,
      MyList<int> stockpilingStorageList,
      MyList<int> otherStorageList,
      out float maxAvailableResource)
    {
      maxAvailableResource = 0.0f;
      for (int index = 0; index < sourceDataByPriority.Length; ++index)
      {
        HashSet<MyResourceSourceComponent> resourceSourceComponentSet = sourcesByPriority[index];
        MyResourceDistributorComponent.MySourceGroupData mySourceGroupData = sourceDataByPriority[index];
        mySourceGroupData.MaxAvailableResource = 0.0f;
        foreach (MyResourceSourceComponent resourceSourceComponent in resourceSourceComponentSet)
        {
          if (resourceSourceComponent.Enabled && resourceSourceComponent.HasCapacityRemainingByType(typeId))
          {
            mySourceGroupData.MaxAvailableResource += resourceSourceComponent.MaxOutputByType(typeId);
            mySourceGroupData.InfiniteCapacity = resourceSourceComponent.IsInfiniteCapacity;
          }
        }
        maxAvailableResource += mySourceGroupData.MaxAvailableResource;
        sourceDataByPriority[index] = mySourceGroupData;
      }
      float num1 = 0.0f;
      for (int index = 0; index < sinksByPriority.Length; ++index)
      {
        float num2 = 0.0f;
        bool flag = true;
        foreach (MyResourceSinkComponent sink in sinksByPriority[index])
        {
          num2 += sink.RequiredInputByType(typeId);
          flag = flag && MyResourceDistributorComponent.IsAdaptible(sink);
        }
        sinkDataByPriority[index].RequiredInput = num2;
        sinkDataByPriority[index].IsAdaptible = flag;
        num1 += num2;
        sinkDataByPriority[index].RequiredInputCumulative = num1;
      }
      MyResourceDistributorComponent.PrepareSinkSourceData(ref typeId, ref sinkSourceData, sinkSourcePairs, stockpilingStorageList, otherStorageList);
      maxAvailableResource += sinkSourceData.Item2.MaxAvailableResource;
    }

    private static void PrepareSinkSourceData(
      ref MyDefinitionId typeId,
      ref MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData> sinkSourceData,
      List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs,
      MyList<int> stockpilingStorageList,
      MyList<int> otherStorageList)
    {
      stockpilingStorageList.Clear();
      otherStorageList.Clear();
      sinkSourceData.Item1.IsAdaptible = true;
      sinkSourceData.Item1.RequiredInput = 0.0f;
      sinkSourceData.Item1.RequiredInputCumulative = 0.0f;
      sinkSourceData.Item2.MaxAvailableResource = 0.0f;
      for (int index = 0; index < sinkSourcePairs.Count; ++index)
      {
        MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> sinkSourcePair = sinkSourcePairs[index];
        bool flag = sinkSourcePair.Item2.ProductionEnabledByType(typeId);
        int num = !sinkSourcePair.Item2.Enabled || flag ? 0 : ((double) sinkSourcePair.Item1.RequiredInputByType(typeId) > 0.0 ? 1 : 0);
        sinkSourceData.Item1.IsAdaptible = sinkSourceData.Item1.IsAdaptible && MyResourceDistributorComponent.IsAdaptible(sinkSourcePair.Item1);
        sinkSourceData.Item1.RequiredInput += sinkSourcePair.Item1.RequiredInputByType(typeId);
        if (num != 0)
          sinkSourceData.Item1.RequiredInputCumulative += sinkSourcePair.Item1.RequiredInputByType(typeId);
        sinkSourceData.Item2.InfiniteCapacity = float.IsInfinity(sinkSourcePair.Item2.RemainingCapacityByType(typeId));
        if (num != 0)
        {
          stockpilingStorageList.Add(index);
        }
        else
        {
          otherStorageList.Add(index);
          if (sinkSourcePair.Item2.Enabled & flag)
            sinkSourceData.Item2.MaxAvailableResource += sinkSourcePair.Item2.MaxOutputByType(typeId);
        }
      }
    }

    private static MyResourceStateEnum RecomputeResourceDistributionPartial(
      ref MyDefinitionId typeId,
      int startPriorityIdx,
      MyResourceDistributorComponent.MySinkGroupData[] sinkDataByPriority,
      MyResourceDistributorComponent.MySourceGroupData[] sourceDataByPriority,
      ref MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData> sinkSourceData,
      HashSet<MyResourceSinkComponent>[] sinksByPriority,
      HashSet<MyResourceSourceComponent>[] sourcesByPriority,
      List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs,
      MyList<int> stockpilingStorageList,
      MyList<int> otherStorageList,
      float availableResource)
    {
      float num1 = availableResource;
      int index1;
      for (index1 = startPriorityIdx; index1 < sinksByPriority.Length; ++index1)
      {
        sinkDataByPriority[index1].RemainingAvailableResource = availableResource;
        if ((double) sinkDataByPriority[index1].RequiredInput <= (double) availableResource)
        {
          availableResource -= sinkDataByPriority[index1].RequiredInput;
          foreach (MyResourceSinkComponent resourceSinkComponent in sinksByPriority[index1])
            resourceSinkComponent.SetInputFromDistributor(typeId, resourceSinkComponent.RequiredInputByType(typeId), sinkDataByPriority[index1].IsAdaptible, true);
        }
        else if (sinkDataByPriority[index1].IsAdaptible && (double) availableResource > 0.0)
        {
          foreach (MyResourceSinkComponent resourceSinkComponent in sinksByPriority[index1])
          {
            float num2 = resourceSinkComponent.RequiredInputByType(typeId) / sinkDataByPriority[index1].RequiredInput;
            resourceSinkComponent.SetInputFromDistributor(typeId, num2 * availableResource, true, true);
          }
          availableResource = 0.0f;
        }
        else
        {
          foreach (MyResourceSinkComponentBase sinkComponentBase in sinksByPriority[index1])
            sinkComponentBase.SetInputFromDistributor(typeId, 0.0f, sinkDataByPriority[index1].IsAdaptible);
          sinkDataByPriority[index1].RemainingAvailableResource = availableResource;
        }
      }
      for (; index1 < sinkDataByPriority.Length; ++index1)
      {
        sinkDataByPriority[index1].RemainingAvailableResource = 0.0f;
        foreach (MyResourceSinkComponentBase sinkComponentBase in sinksByPriority[index1])
          sinkComponentBase.SetInputFromDistributor(typeId, 0.0f, sinkDataByPriority[index1].IsAdaptible);
      }
      float num3 = (float) ((double) num1 - (double) availableResource + (startPriorityIdx != 0 ? (double) sinkDataByPriority[0].RemainingAvailableResource - (double) sinkDataByPriority[startPriorityIdx].RemainingAvailableResource : 0.0));
      float num4 = Math.Max(num1 - num3, 0.0f);
      float num5 = num4;
      if (stockpilingStorageList.Count > 0)
      {
        float requiredInputCumulative = sinkSourceData.Item1.RequiredInputCumulative;
        if ((double) requiredInputCumulative <= (double) num5)
        {
          num5 -= requiredInputCumulative;
          foreach (int stockpilingStorage in stockpilingStorageList)
          {
            MyResourceSinkComponent resourceSinkComponent = sinkSourcePairs[stockpilingStorage].Item1;
            resourceSinkComponent.SetInputFromDistributor(typeId, resourceSinkComponent.RequiredInputByType(typeId), true, true);
          }
          sinkSourceData.Item1.RemainingAvailableResource = num5;
        }
        else
        {
          foreach (int stockpilingStorage in stockpilingStorageList)
          {
            MyResourceSinkComponent resourceSinkComponent = sinkSourcePairs[stockpilingStorage].Item1;
            float num2 = resourceSinkComponent.RequiredInputByType(typeId) / requiredInputCumulative;
            resourceSinkComponent.SetInputFromDistributor(typeId, num2 * num4, true, true);
          }
          num5 = 0.0f;
          sinkSourceData.Item1.RemainingAvailableResource = num5;
        }
      }
      float num6 = num4 - num5;
      float num7 = Math.Max(num1 - (sinkSourceData.Item2.MaxAvailableResource - sinkSourceData.Item2.MaxAvailableResource * sinkSourceData.Item2.UsageRatio) - num3 - num6, 0.0f);
      float num8 = num7;
      if (otherStorageList.Count > 0)
      {
        float num2 = sinkSourceData.Item1.RequiredInput - sinkSourceData.Item1.RequiredInputCumulative;
        if ((double) num2 <= (double) num8)
        {
          num8 -= num2;
          for (int index2 = 0; index2 < otherStorageList.Count; ++index2)
          {
            int otherStorage = otherStorageList[index2];
            MyResourceSinkComponent resourceSinkComponent = sinkSourcePairs[otherStorage].Item1;
            resourceSinkComponent.SetInputFromDistributor(typeId, resourceSinkComponent.RequiredInputByType(typeId), true, true);
          }
          sinkSourceData.Item1.RemainingAvailableResource = num8;
        }
        else
        {
          for (int index2 = 0; index2 < otherStorageList.Count; ++index2)
          {
            int otherStorage = otherStorageList[index2];
            MyResourceSinkComponent resourceSinkComponent = sinkSourcePairs[otherStorage].Item1;
            float num9 = resourceSinkComponent.RequiredInputByType(typeId) / num2;
            resourceSinkComponent.SetInputFromDistributor(typeId, num9 * num8, true, true);
          }
          num8 = 0.0f;
          sinkSourceData.Item1.RemainingAvailableResource = num8;
        }
      }
      float num10 = num7 - num8;
      float num11 = num6 + num3;
      if ((double) sinkSourceData.Item2.MaxAvailableResource > 0.0)
      {
        float val1 = num11;
        sinkSourceData.Item2.UsageRatio = Math.Min(1f, val1 / sinkSourceData.Item2.MaxAvailableResource);
        num11 -= Math.Min(val1, sinkSourceData.Item2.MaxAvailableResource);
      }
      else
        sinkSourceData.Item2.UsageRatio = 0.0f;
      float num12 = num4 - num5;
      float num13 = Math.Max(num1 - (sinkSourceData.Item2.MaxAvailableResource - sinkSourceData.Item2.MaxAvailableResource * sinkSourceData.Item2.UsageRatio) - num3 - num12, 0.0f);
      if (otherStorageList.Count > 0)
      {
        float num2 = sinkSourceData.Item1.RequiredInput - sinkSourceData.Item1.RequiredInputCumulative;
        if ((double) num2 <= (double) num13)
        {
          float num9 = num13 - num2;
          for (int index2 = 0; index2 < otherStorageList.Count; ++index2)
          {
            int otherStorage = otherStorageList[index2];
            MyResourceSinkComponent resourceSinkComponent = sinkSourcePairs[otherStorage].Item1;
            resourceSinkComponent.SetInputFromDistributor(typeId, resourceSinkComponent.RequiredInputByType(typeId), true, true);
          }
          sinkSourceData.Item1.RemainingAvailableResource = num9;
        }
        else
        {
          for (int index2 = 0; index2 < otherStorageList.Count; ++index2)
          {
            int otherStorage = otherStorageList[index2];
            MyResourceSinkComponent resourceSinkComponent = sinkSourcePairs[otherStorage].Item1;
            float num9 = resourceSinkComponent.RequiredInputByType(typeId) / num2;
            resourceSinkComponent.SetInputFromDistributor(typeId, num9 * num13, true, true);
          }
          float num14 = 0.0f;
          sinkSourceData.Item1.RemainingAvailableResource = num14;
        }
      }
      sinkSourceData.Item2.ActiveCount = 0;
      for (int index2 = 0; index2 < otherStorageList.Count; ++index2)
      {
        int otherStorage = otherStorageList[index2];
        MyResourceSourceComponent resourceSourceComponent = sinkSourcePairs[otherStorage].Item2;
        if (resourceSourceComponent.Enabled && resourceSourceComponent.ProductionEnabledByType(typeId) && resourceSourceComponent.HasCapacityRemainingByType(typeId))
        {
          ++sinkSourceData.Item2.ActiveCount;
          resourceSourceComponent.SetOutputByType(typeId, sinkSourceData.Item2.UsageRatio * resourceSourceComponent.MaxOutputByType(typeId));
        }
      }
      int index3 = 0;
      float val1_1 = num11 + num10;
      for (; index3 < sourcesByPriority.Length; ++index3)
      {
        if ((double) sourceDataByPriority[index3].MaxAvailableResource > 0.0)
        {
          float val1_2 = Math.Max(val1_1, 0.0f);
          sourceDataByPriority[index3].UsageRatio = Math.Min(1f, val1_2 / sourceDataByPriority[index3].MaxAvailableResource);
          val1_1 -= Math.Min(val1_2, sourceDataByPriority[index3].MaxAvailableResource);
        }
        else
          sourceDataByPriority[index3].UsageRatio = 0.0f;
        sourceDataByPriority[index3].ActiveCount = 0;
        foreach (MyResourceSourceComponent resourceSourceComponent in sourcesByPriority[index3])
        {
          if (resourceSourceComponent.Enabled && resourceSourceComponent.HasCapacityRemainingByType(typeId))
          {
            ++sourceDataByPriority[index3].ActiveCount;
            resourceSourceComponent.SetOutputByType(typeId, sourceDataByPriority[index3].UsageRatio * resourceSourceComponent.MaxOutputByType(typeId));
          }
        }
      }
      MyResourceStateEnum resourceStateEnum;
      if ((double) num1 == 0.0)
        resourceStateEnum = MyResourceStateEnum.NoPower;
      else if ((double) sinkDataByPriority[MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal - 1].RequiredInputCumulative > (double) num1)
      {
        MyResourceDistributorComponent.MySinkGroupData mySinkGroupData = ((IEnumerable<MyResourceDistributorComponent.MySinkGroupData>) sinkDataByPriority).Last<MyResourceDistributorComponent.MySinkGroupData>();
        resourceStateEnum = !mySinkGroupData.IsAdaptible || (double) mySinkGroupData.RemainingAvailableResource == 0.0 ? MyResourceStateEnum.OverloadBlackout : MyResourceStateEnum.OverloadAdaptible;
      }
      else
        resourceStateEnum = MyResourceStateEnum.Ok;
      return resourceStateEnum;
    }

    private bool MatchesAdaptability(
      HashSet<MyResourceSinkComponent> group,
      MyResourceSinkComponent referenceSink)
    {
      bool flag = MyResourceDistributorComponent.IsAdaptible(referenceSink);
      foreach (MyResourceSinkComponent sink in group)
      {
        if (MyResourceDistributorComponent.IsAdaptible(sink) != flag)
          return false;
      }
      return true;
    }

    private bool MatchesInfiniteCapacity(
      HashSet<MyResourceSourceComponent> group,
      MyResourceSourceComponent producer)
    {
      foreach (MyResourceSourceComponent resourceSourceComponent in group)
      {
        if (producer.IsInfiniteCapacity != resourceSourceComponent.IsInfiniteCapacity)
          return false;
      }
      return true;
    }

    [Conditional("DEBUG")]
    private void UpdateTrace()
    {
      for (int index1 = 0; index1 < this.m_typeGroupCount; ++index1)
      {
        for (int index2 = 0; index2 < this.m_dataPerType[index1].DistributionGroupsInUse; ++index2)
        {
          MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.m_dataPerType[index1].DistributionGroups[index2];
          int num = 0;
          while (num < distributionGroup.SinkSourcePairs.Count)
            ++num;
        }
      }
    }

    private HashSet<MyResourceSinkComponent> GetSinksOfType(
      ref MyDefinitionId typeId,
      MyStringHash groupType)
    {
      int typeIndex;
      int index;
      return !this.TryGetTypeIndex(typeId, out typeIndex) || !MyResourceDistributorComponent.m_sinkSubtypeToPriority.TryGetValue(groupType, out index) ? (HashSet<MyResourceSinkComponent>) null : this.m_dataPerType[typeIndex].SinksByPriority[index];
    }

    private HashSet<MyResourceSourceComponent> GetSourcesOfType(
      ref MyDefinitionId typeId,
      MyStringHash groupType)
    {
      int typeIndex;
      int index;
      return !this.TryGetTypeIndex(typeId, out typeIndex) || !MyResourceDistributorComponent.m_sourceSubtypeToPriority.TryGetValue(groupType, out index) ? (HashSet<MyResourceSourceComponent>) null : this.m_dataPerType[typeIndex].SourcesByPriority[index];
    }

    private MyResourceSourceComponent GetFirstSourceOfType(
      ref MyDefinitionId resourceTypeId)
    {
      int typeIndex = this.GetTypeIndex(ref resourceTypeId);
      for (int index = 0; index < this.m_dataPerType[typeIndex].SourcesByPriority.Length; ++index)
      {
        HashSet<MyResourceSourceComponent> hashset = this.m_dataPerType[typeIndex].SourcesByPriority[index];
        if (hashset.Count > 0)
          return hashset.FirstElement<MyResourceSourceComponent>();
      }
      return (MyResourceSourceComponent) null;
    }

    public MyMultipleEnabledEnum SourcesEnabledByType(
      MyDefinitionId resourceTypeId)
    {
      int typeIndex;
      if (!this.TryGetTypeIndex(ref resourceTypeId, out typeIndex))
        return MyMultipleEnabledEnum.NoObjects;
      if (this.m_dataPerType[typeIndex].SourcesEnabledDirty)
        this.RefreshSourcesEnabled(resourceTypeId);
      return this.m_dataPerType[typeIndex].SourcesEnabled;
    }

    public float RemainingFuelTimeByType(MyDefinitionId resourceTypeId)
    {
      int typeIndex;
      if (!this.TryGetTypeIndex(ref resourceTypeId, out typeIndex))
        return 0.0f;
      if (!this.m_dataPerType[typeIndex].RemainingFuelTimeDirty)
        return this.m_dataPerType[typeIndex].RemainingFuelTime;
      this.m_dataPerType[typeIndex].RemainingFuelTime = this.ComputeRemainingFuelTime(resourceTypeId);
      return this.m_dataPerType[typeIndex].RemainingFuelTime;
    }

    private bool NeedsRecompute(ref MyDefinitionId typeId)
    {
      int num;
      if (this.m_changedTypes.TryGetValue(typeId, out num) && num > 0)
        return true;
      int typeIndex;
      return this.TryGetTypeIndex(ref typeId, out typeIndex) && this.m_dataPerType[typeIndex].NeedsRecompute;
    }

    private bool NeedsRecompute(ref MyDefinitionId typeId, int typeIndex)
    {
      if (this.m_typeGroupCount > 0 && typeIndex >= 0 && (this.m_dataPerType.Count > typeIndex && this.m_dataPerType[typeIndex].NeedsRecompute))
        return true;
      int num;
      return this.m_changedTypes.TryGetValue(typeId, out num) && num > 0;
    }

    public MyResourceStateEnum ResourceStateByType(
      MyDefinitionId typeId,
      bool withRecompute = true)
    {
      int typeIndex = this.GetTypeIndex(ref typeId);
      if (withRecompute && this.NeedsRecompute(ref typeId) && !Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
        this.RecomputeResourceDistribution(ref typeId);
      return !withRecompute && (typeIndex < 0 || typeIndex >= this.m_dataPerType.Count) ? MyResourceStateEnum.NoPower : this.m_dataPerType[typeIndex].ResourceState;
    }

    private bool TryGetTypeIndex(MyDefinitionId typeId, out int typeIndex) => this.TryGetTypeIndex(ref typeId, out typeIndex);

    private bool TryGetTypeIndex(ref MyDefinitionId typeId, out int typeIndex)
    {
      typeIndex = 0;
      if (this.m_typeGroupCount == 0)
        return false;
      return this.m_typeGroupCount <= 1 || this.m_typeIdToIndex.TryGetValue(typeId, out typeIndex);
    }

    private int GetTypeIndex(ref MyDefinitionId typeId)
    {
      int num = 0;
      if (this.m_typeGroupCount > 1)
        num = this.m_typeIdToIndex[typeId];
      return num;
    }

    private static int GetTypeIndexTotal(ref MyDefinitionId typeId)
    {
      int num = 0;
      if (MyResourceDistributorComponent.m_typeGroupCountTotal > 1)
        num = MyResourceDistributorComponent.m_typeIdToIndexTotal[typeId];
      return num;
    }

    public static bool IsConveyorConnectionRequiredTotal(MyDefinitionId typeId) => MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref typeId);

    public static bool IsConveyorConnectionRequiredTotal(ref MyDefinitionId typeId)
    {
      try
      {
        return MyResourceDistributorComponent.m_typeIdToConveyorConnectionRequiredTotal[typeId];
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder("SLIME: IsConveyorConnectionRequiredTotal: " + (object) typeId + " -> ");
        foreach (KeyValuePair<MyDefinitionId, bool> keyValuePair in MyResourceDistributorComponent.m_typeIdToConveyorConnectionRequiredTotal)
        {
          string str = keyValuePair.Key.ToString() + "/" + keyValuePair.Value.ToString() + " ";
          stringBuilder.Append(str);
        }
        throw new Exception(stringBuilder.ToString());
      }
    }

    private bool IsConveyorConnectionRequired(ref MyDefinitionId typeId)
    {
      try
      {
        return this.m_typeIdToConveyorConnectionRequired[typeId];
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder("SLIME: IsConveyorConnectionRequiredTotal: " + (object) typeId + " -> ");
        foreach (KeyValuePair<MyDefinitionId, bool> keyValuePair in this.m_typeIdToConveyorConnectionRequired)
          keyValuePair.Key.ToString() + "/" + keyValuePair.Value.ToString() + " ";
        throw new Exception(stringBuilder.ToString());
      }
    }

    internal static int GetPriority(MyResourceSinkComponent sink) => MyResourceDistributorComponent.m_sinkSubtypeToPriority[sink.Group];

    internal static int GetPriority(MyResourceSourceComponent source) => MyResourceDistributorComponent.m_sourceSubtypeToPriority[source.Group];

    private static bool IsAdaptible(MyResourceSinkComponent sink) => MyResourceDistributorComponent.m_sinkSubtypeToAdaptability[sink.Group];

    private void RemoveType(ref MyDefinitionId typeId)
    {
      int typeIndex;
      if (!this.TryGetTypeIndex(ref typeId, out typeIndex))
        return;
      this.m_dataPerType.RemoveAt(typeIndex);
      this.m_initializedTypes.Remove(typeId);
      --this.m_typeGroupCount;
      this.m_typeIdToIndex.Remove(typeId);
      this.m_typeIdToConveyorConnectionRequired.Remove(typeId);
      this.RaiseSystemChanged();
    }

    private void RaiseSystemChanged()
    {
      Action systemChanged = this.SystemChanged;
      if (systemChanged == null)
        return;
      systemChanged();
    }

    private void Sink_OnAddType(MyResourceSinkComponent sink, MyDefinitionId resourceType) => this.RefreshSink(sink);

    private void Sink_OnRemoveType(MyResourceSinkComponent sink, MyDefinitionId resourceType) => this.RefreshSink(sink);

    private void RefreshSink(MyResourceSinkComponent sink)
    {
      if (Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
      {
        Sandbox.Game.Entities.MyEntities.InvokeLater((Action) (() => this.RefreshSink(sink)));
      }
      else
      {
        this.RemoveSinkLazy(sink, false);
        this.CheckDistributionSystemChanges();
        this.AddSinkLazy(sink);
      }
    }

    private void Sink_RequiredInputChanged(
      MyDefinitionId changedResourceTypeId,
      MyResourceSinkComponent changedSink,
      float oldRequirement,
      float newRequirement)
    {
      if (!this.m_typeIdToIndex.ContainsKey(changedResourceTypeId) || !MyResourceDistributorComponent.m_sinkSubtypeToPriority.ContainsKey(changedSink.Group))
        return;
      int typeIndex = this.GetTypeIndex(ref changedResourceTypeId);
      if (!this.TryGetTypeIndex(changedResourceTypeId, out typeIndex))
        return;
      this.m_dataPerType[typeIndex].NeedsRecompute = true;
    }

    private float Sink_IsResourceAvailable(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver)
    {
      int typeIndex = this.GetTypeIndex(ref resourceTypeId);
      int priority = MyResourceDistributorComponent.GetPriority(receiver);
      if (!this.IsConveyorConnectionRequired(ref resourceTypeId))
        return this.m_dataPerType[typeIndex].SinkDataByPriority[priority].RemainingAvailableResource - this.m_dataPerType[typeIndex].SinkDataByPriority[priority].RequiredInput;
      if (!(receiver.Entity is IMyConveyorEndpointBlock entity))
        return 0.0f;
      IMyConveyorEndpoint conveyorEndpoint = entity.ConveyorEndpoint;
      int index = 0;
      while (index < this.m_dataPerType[typeIndex].DistributionGroupsInUse && !this.m_dataPerType[typeIndex].DistributionGroups[index].SinksByPriority[priority].Contains(receiver))
        ++index;
      return index == this.m_dataPerType[typeIndex].DistributionGroupsInUse ? 0.0f : this.m_dataPerType[typeIndex].DistributionGroups[index].SinkDataByPriority[priority].RemainingAvailableResource - this.m_dataPerType[typeIndex].DistributionGroups[index].SinkDataByPriority[priority].RequiredInput;
    }

    private void source_HasRemainingCapacityChanged(
      MyDefinitionId changedResourceTypeId,
      MyResourceSourceComponent source)
    {
      int typeIndex = this.GetTypeIndex(ref changedResourceTypeId);
      this.m_dataPerType[typeIndex].NeedsRecompute = true;
      this.m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
    }

    public void ConveyorSystem_OnPoweredChanged() => MySandboxGame.Static.Invoke((Action) (() =>
    {
      for (int index = 0; index < this.m_dataPerType.Count; ++index)
      {
        this.m_dataPerType[index].GroupsDirty = true;
        this.m_dataPerType[index].NeedsRecompute = true;
        this.m_dataPerType[index].RemainingFuelTimeDirty = true;
        this.m_dataPerType[index].SourcesEnabledDirty = true;
      }
    }), nameof (ConveyorSystem_OnPoweredChanged));

    private void source_MaxOutputChanged(
      MyDefinitionId changedResourceTypeId,
      float oldOutput,
      MyResourceSourceComponent obj)
    {
      int typeIndex = this.GetTypeIndex(ref changedResourceTypeId);
      this.m_dataPerType[typeIndex].NeedsRecompute = true;
      this.m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
      this.m_dataPerType[typeIndex].SourcesEnabledDirty = true;
      if (this.m_dataPerType[typeIndex].SourceCount != 1 || Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
        return;
      this.RecomputeResourceDistribution(ref changedResourceTypeId);
    }

    private void source_ProductionEnabledChanged(
      MyDefinitionId changedResourceTypeId,
      MyResourceSourceComponent obj)
    {
      int typeIndex = this.GetTypeIndex(ref changedResourceTypeId);
      this.m_dataPerType[typeIndex].NeedsRecompute = true;
      this.m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
      this.m_dataPerType[typeIndex].SourcesEnabledDirty = true;
      if (this.m_dataPerType[typeIndex].SourceCount != 1 || Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
        return;
      this.RecomputeResourceDistribution(ref changedResourceTypeId);
    }

    public float MaxAvailableResourceByType(MyDefinitionId resourceTypeId)
    {
      int typeIndex;
      return !this.TryGetTypeIndex(ref resourceTypeId, out typeIndex) ? 0.0f : this.m_dataPerType[typeIndex].MaxAvailableResource;
    }

    public float TotalRequiredInputByType(MyDefinitionId resourceTypeId)
    {
      int typeIndex;
      return !this.TryGetTypeIndex(ref resourceTypeId, out typeIndex) ? 0.0f : ((IEnumerable<MyResourceDistributorComponent.MySinkGroupData>) this.m_dataPerType[typeIndex].SinkDataByPriority).Last<MyResourceDistributorComponent.MySinkGroupData>().RequiredInputCumulative;
    }

    public override string ComponentTypeDebugString => "Resource Distributor";

    public void DebugDraw(MyEntity entity)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        return;
      double num1 = 6.5;
      double num2 = num1 * 0.045;
      Vector3D worldCoord = entity.WorldMatrix.Translation;
      if (entity is MyCubeGrid myCubeGrid)
      {
        myCubeGrid.GetPhysicalGroupAABB();
        worldCoord = myCubeGrid.GetPhysicalGroupAABB().Center;
      }
      Vector3D position = MySector.MainCamera.Position;
      Vector3D up = MySector.MainCamera.WorldMatrix.Up;
      Vector3D right = MySector.MainCamera.WorldMatrix.Right;
      float num3 = (float) Math.Atan(num1 / Math.Max(Vector3D.Distance(worldCoord, position), 0.001));
      if ((double) num3 <= 0.270000010728836)
        return;
      MyRenderProxy.DebugDrawText3D(worldCoord, entity.ToString(), Color.Yellow, num3, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      if (this.m_initializedTypes == null || this.m_initializedTypes.Count == 0)
        return;
      int num4 = -1;
      foreach (MyDefinitionId initializedType in this.m_initializedTypes)
      {
        this.DebugDrawResource(initializedType, worldCoord + (double) num4 * up * num2, right, num3);
        --num4;
      }
      while (this.m_changesDebug.Count > 10)
        this.m_changesDebug.RemoveAt(0);
      int num5 = num4 - 1;
      MyRenderProxy.DebugDrawText3D(worldCoord + (double) num5 * up * num2 + right * 0.0649999976158142, "Recent changes:", Color.LightYellow, num3, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      int num6 = num5 - 1;
      foreach (string text in this.m_changesDebug)
      {
        MyRenderProxy.DebugDrawText3D(worldCoord + (double) num6 * up * num2 + right * 0.0649999976158142, text, Color.White, num3, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        --num6;
      }
    }

    private void DebugDrawResource(
      MyDefinitionId resourceId,
      Vector3D origin,
      Vector3D rightVector,
      float textSize)
    {
      Vector3D vector3D = 0.0500000007450581 * rightVector;
      Vector3D worldCoord = origin + vector3D + rightVector * 0.0149999996647239;
      int index = 0;
      string text = resourceId.SubtypeName;
      if (this.m_typeIdToIndex.TryGetValue(resourceId, out index))
      {
        MyResourceDistributorComponent.PerTypeData perTypeData = this.m_dataPerType[index];
        int num = 0;
        foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in perTypeData.SinksByPriority)
          num += resourceSinkComponentSet.Count;
        text = string.Format("{0} Sources:{1} Sinks:{2} Available:{3} State:{4}", (object) resourceId.SubtypeName, (object) perTypeData.SourceCount, (object) num, (object) perTypeData.MaxAvailableResource, (object) perTypeData.ResourceState);
      }
      MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, false);
      MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
    }

    public void ClearData()
    {
      foreach (MyResourceDistributorComponent.PerTypeData perTypeData in this.m_dataPerType)
        perTypeData.ClearData();
      this.m_sinksToAdd.Clear();
      this.m_sourcesToAdd.Clear();
      this.m_sinksToRemove.Clear();
      this.m_sourcesToRemove.Clear();
      this.OnPowerGenerationChanged = (Action<bool>) null;
    }

    private struct MyPhysicalDistributionGroup
    {
      public IMyConveyorEndpoint FirstEndpoint;
      public HashSet<MyResourceSinkComponent>[] SinksByPriority;
      public HashSet<MyResourceSourceComponent>[] SourcesByPriority;
      public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> SinkSourcePairs;
      public MyResourceDistributorComponent.MySinkGroupData[] SinkDataByPriority;
      public MyResourceDistributorComponent.MySourceGroupData[] SourceDataByPriority;
      public MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData> InputOutputData;
      public MyList<int> StockpilingStorage;
      public MyList<int> OtherStorage;
      public float MaxAvailableResources;
      public MyResourceStateEnum ResourceState;

      public MyPhysicalDistributionGroup(MyDefinitionId typeId, IMyConveyorEndpointBlock block)
      {
        this.SinksByPriority = (HashSet<MyResourceSinkComponent>[]) null;
        this.SourcesByPriority = (HashSet<MyResourceSourceComponent>[]) null;
        this.SinkSourcePairs = (List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>) null;
        this.FirstEndpoint = (IMyConveyorEndpoint) null;
        this.SinkDataByPriority = (MyResourceDistributorComponent.MySinkGroupData[]) null;
        this.SourceDataByPriority = (MyResourceDistributorComponent.MySourceGroupData[]) null;
        this.StockpilingStorage = (MyList<int>) null;
        this.OtherStorage = (MyList<int>) null;
        this.InputOutputData = new MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData>();
        this.MaxAvailableResources = 0.0f;
        this.ResourceState = MyResourceStateEnum.NoPower;
        this.AllocateData();
        this.Init(typeId, block);
      }

      public MyPhysicalDistributionGroup(
        MyDefinitionId typeId,
        MyResourceSinkComponent tempConnectedSink)
      {
        this.SinksByPriority = (HashSet<MyResourceSinkComponent>[]) null;
        this.SourcesByPriority = (HashSet<MyResourceSourceComponent>[]) null;
        this.SinkSourcePairs = (List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>) null;
        this.FirstEndpoint = (IMyConveyorEndpoint) null;
        this.SinkDataByPriority = (MyResourceDistributorComponent.MySinkGroupData[]) null;
        this.SourceDataByPriority = (MyResourceDistributorComponent.MySourceGroupData[]) null;
        this.StockpilingStorage = (MyList<int>) null;
        this.OtherStorage = (MyList<int>) null;
        this.InputOutputData = new MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData>();
        this.MaxAvailableResources = 0.0f;
        this.ResourceState = MyResourceStateEnum.NoPower;
        this.AllocateData();
        this.InitFromTempConnected(typeId, tempConnectedSink);
      }

      public MyPhysicalDistributionGroup(
        MyDefinitionId typeId,
        MyResourceSourceComponent tempConnectedSource)
      {
        this.SinksByPriority = (HashSet<MyResourceSinkComponent>[]) null;
        this.SourcesByPriority = (HashSet<MyResourceSourceComponent>[]) null;
        this.SinkSourcePairs = (List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>) null;
        this.FirstEndpoint = (IMyConveyorEndpoint) null;
        this.SinkDataByPriority = (MyResourceDistributorComponent.MySinkGroupData[]) null;
        this.SourceDataByPriority = (MyResourceDistributorComponent.MySourceGroupData[]) null;
        this.StockpilingStorage = (MyList<int>) null;
        this.OtherStorage = (MyList<int>) null;
        this.InputOutputData = new MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData>();
        this.MaxAvailableResources = 0.0f;
        this.ResourceState = MyResourceStateEnum.NoPower;
        this.AllocateData();
        this.InitFromTempConnected(typeId, tempConnectedSource);
      }

      public void Init(MyDefinitionId typeId, IMyConveyorEndpointBlock block)
      {
        this.ClearData();
        this.FirstEndpoint = block.ConveyorEndpoint;
        this.Add(typeId, block);
      }

      public void InitFromTempConnected(
        MyDefinitionId typeId,
        MyResourceSinkComponent tempConnectedSink)
      {
        this.ClearData();
        if (this.FirstEndpoint == null && tempConnectedSink.TemporaryConnectedEntity is IMyConveyorEndpointBlock temporaryConnectedEntity)
          this.FirstEndpoint = temporaryConnectedEntity.ConveyorEndpoint;
        this.AddTempConnected(typeId, tempConnectedSink);
      }

      public void InitFromTempConnected(
        MyDefinitionId typeId,
        MyResourceSourceComponent tempConnectedSource)
      {
        this.ClearData();
        if (tempConnectedSource.TemporaryConnectedEntity is IMyConveyorEndpointBlock temporaryConnectedEntity)
          this.FirstEndpoint = temporaryConnectedEntity.ConveyorEndpoint;
        this.AddTempConnected(typeId, tempConnectedSource);
      }

      public void Add(MyDefinitionId typeId, IMyConveyorEndpointBlock endpoint)
      {
        if (this.FirstEndpoint == null)
          this.FirstEndpoint = endpoint.ConveyorEndpoint;
        MyEntityComponentContainer components = (endpoint as IMyEntity).Components;
        MyResourceSinkComponent sink = components.Get<MyResourceSinkComponent>();
        MyResourceSourceComponent source = components.Get<MyResourceSourceComponent>();
        bool flag1 = sink != null && sink.AcceptedResources.Contains<MyDefinitionId>(typeId);
        bool flag2 = source != null && source.ResourceTypes.Contains<MyDefinitionId>(typeId);
        if (flag1 & flag2)
          this.SinkSourcePairs.Add(new MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>(sink, source));
        else if (flag1)
        {
          this.SinksByPriority[MyResourceDistributorComponent.GetPriority(sink)].Add(sink);
        }
        else
        {
          if (!flag2)
            return;
          this.SourcesByPriority[MyResourceDistributorComponent.GetPriority(source)].Add(source);
        }
      }

      public void AddTempConnected(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
      {
        if ((tempConnectedSink == null ? 0 : (tempConnectedSink.AcceptedResources.Contains<MyDefinitionId>(typeId) ? 1 : 0)) == 0)
          return;
        this.SinksByPriority[MyResourceDistributorComponent.GetPriority(tempConnectedSink)].Add(tempConnectedSink);
      }

      public void AddTempConnected(
        MyDefinitionId typeId,
        MyResourceSourceComponent tempConnectedSource)
      {
        if ((tempConnectedSource == null ? 0 : (tempConnectedSource.ResourceTypes.Contains<MyDefinitionId>(typeId) ? 1 : 0)) == 0)
          return;
        this.SourcesByPriority[MyResourceDistributorComponent.GetPriority(tempConnectedSource)].Add(tempConnectedSource);
      }

      private void AllocateData()
      {
        this.FirstEndpoint = (IMyConveyorEndpoint) null;
        this.SinksByPriority = new HashSet<MyResourceSinkComponent>[MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal];
        this.SourcesByPriority = new HashSet<MyResourceSourceComponent>[MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal];
        this.SinkSourcePairs = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
        this.SinkDataByPriority = new MyResourceDistributorComponent.MySinkGroupData[MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal];
        this.SourceDataByPriority = new MyResourceDistributorComponent.MySourceGroupData[MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal];
        this.StockpilingStorage = new MyList<int>();
        this.OtherStorage = new MyList<int>();
        for (int index = 0; index < MyResourceDistributorComponent.m_sinkGroupPrioritiesTotal; ++index)
          this.SinksByPriority[index] = new HashSet<MyResourceSinkComponent>();
        for (int index = 0; index < MyResourceDistributorComponent.m_sourceGroupPrioritiesTotal; ++index)
          this.SourcesByPriority[index] = new HashSet<MyResourceSourceComponent>();
      }

      public void ClearData()
      {
        foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in this.SinksByPriority)
          resourceSinkComponentSet.Clear();
        foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in this.SourcesByPriority)
          resourceSourceComponentSet.Clear();
        this.SinkSourcePairs.Clear();
        this.StockpilingStorage.ClearFast();
        this.OtherStorage.ClearFast();
        this.FirstEndpoint = (IMyConveyorEndpoint) null;
      }
    }

    private struct MySinkGroupData
    {
      public bool IsAdaptible;
      public float RequiredInput;
      public float RequiredInputCumulative;
      public float RemainingAvailableResource;

      public override string ToString() => string.Format("IsAdaptible: {0}, RequiredInput: {1}, RemainingAvailableResource: {2}", (object) this.IsAdaptible, (object) this.RequiredInput, (object) this.RemainingAvailableResource);
    }

    private struct MySourceGroupData
    {
      public float MaxAvailableResource;
      public float UsageRatio;
      public bool InfiniteCapacity;
      public int ActiveCount;

      public override string ToString() => string.Format("MaxAvailableResource: {0}, UsageRatio: {1}", (object) this.MaxAvailableResource, (object) this.UsageRatio);
    }

    private class PerTypeData
    {
      private bool m_needsRecompute;
      public MyDefinitionId TypeId;
      public MyResourceDistributorComponent.MySinkGroupData[] SinkDataByPriority;
      public MyResourceDistributorComponent.MySourceGroupData[] SourceDataByPriority;
      public MyTuple<MyResourceDistributorComponent.MySinkGroupData, MyResourceDistributorComponent.MySourceGroupData> InputOutputData;
      public HashSet<MyResourceSinkComponent>[] SinksByPriority;
      public HashSet<MyResourceSourceComponent>[] SourcesByPriority;
      public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> InputOutputList;
      public MyList<int> StockpilingStorageIndices;
      public MyList<int> OtherStorageIndices;
      public List<MyResourceDistributorComponent.MyPhysicalDistributionGroup> DistributionGroups;
      public int DistributionGroupsInUse;
      public bool GroupsDirty;
      public int SourceCount;
      public float RemainingFuelTime;
      public bool RemainingFuelTimeDirty;
      public float MaxAvailableResource;
      public MyMultipleEnabledEnum SourcesEnabled;
      public bool SourcesEnabledDirty;
      public MyResourceStateEnum ResourceState;
      private bool m_gridsForUpdateValid;
      private MyCubeGrid m_gridForUpdate;
      private bool m_gridUpdateScheduled;
      private Action m_UpdateGridsCallback;

      public bool NeedsRecompute
      {
        get => this.m_needsRecompute;
        set
        {
          if (this.m_needsRecompute == value)
            return;
          this.m_needsRecompute = value;
          if (!this.m_needsRecompute)
            return;
          this.ScheduleGridUpdate();
        }
      }

      public PerTypeData() => this.m_UpdateGridsCallback = new Action(this.UpdateGrids);

      public void InvalidateGridForUpdateCache()
      {
        this.m_gridForUpdate = (MyCubeGrid) null;
        this.m_gridsForUpdateValid = false;
      }

      private void ScheduleGridUpdate()
      {
        if (this.m_gridUpdateScheduled)
          return;
        this.m_gridUpdateScheduled = true;
        MySandboxGame.Static.Invoke(this.m_UpdateGridsCallback, "UpdateResourcesOnGrids");
      }

      private void UpdateGrids()
      {
        this.m_gridUpdateScheduled = false;
        if (!this.m_gridsForUpdateValid)
        {
          this.m_gridsForUpdateValid = true;
          this.m_gridForUpdate = FindGridForUpdate();
        }
        MyCubeGrid gridForUpdate = this.m_gridForUpdate;
        if ((gridForUpdate != null ? (!gridForUpdate.Closed ? 1 : 0) : 0) == 0)
          return;
        this.m_gridForUpdate.GridSystems?.ResourceDistributor?.Schedule();

        MyCubeGrid FindGridForUpdate()
        {
          foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in this.SourcesByPriority)
          {
            foreach (MyEntityComponentBase entityComponentBase in resourceSourceComponentSet)
            {
              MyCubeGrid myCubeGrid = entityComponentBase.Entity is MyCubeBlock entity ? entity.CubeGrid : (MyCubeGrid) null;
              if (myCubeGrid != null)
                return myCubeGrid;
            }
          }
          foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in this.SinksByPriority)
          {
            foreach (MyResourceSinkComponent resourceSinkComponent in resourceSinkComponentSet)
            {
              MyCubeGrid myCubeGrid = resourceSinkComponent.Entity is MyCubeBlock entity ? entity.CubeGrid : (MyCubeGrid) null;
              if (myCubeGrid != null)
                return myCubeGrid;
            }
          }
          return (MyCubeGrid) null;
        }
      }

      public void ClearData()
      {
        if (this.DistributionGroups != null)
        {
          for (int index = 0; index < this.DistributionGroups.Count; ++index)
          {
            MyResourceDistributorComponent.MyPhysicalDistributionGroup distributionGroup = this.DistributionGroups[index];
            distributionGroup.ClearData();
            this.DistributionGroups[index] = distributionGroup;
          }
          this.DistributionGroupsInUse = 0;
        }
        foreach (HashSet<MyResourceSinkComponent> resourceSinkComponentSet in this.SinksByPriority)
          resourceSinkComponentSet.Clear();
        foreach (HashSet<MyResourceSourceComponent> resourceSourceComponentSet in this.SourcesByPriority)
          resourceSourceComponentSet.Clear();
        this.InputOutputList.Clear();
        this.InvalidateGridForUpdateCache();
      }
    }

    private class Sandbox_Game_EntityComponents_MyResourceDistributorComponent\u003C\u003EActor
    {
    }
  }
}
