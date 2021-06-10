// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridConveyorSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyGridConveyorSystem : MyUpdateableGridSystem
  {
    private const uint MAX_ITEMS_TO_PUSH_IN_ONE_REQUEST = 10;
    private static readonly float CONVEYOR_SYSTEM_CONSUMPTION = 1E-07f;
    private static readonly int MAX_COMPUTATION_TASKS_FOR_ALL_REQUESTS = 10;
    private readonly HashSet<MyCubeBlock> m_inventoryBlocks = new HashSet<MyCubeBlock>();
    private readonly HashSet<IMyConveyorEndpointBlock> m_conveyorEndpointBlocks = new HashSet<IMyConveyorEndpointBlock>();
    private readonly HashSet<MyConveyorLine> m_lines = new HashSet<MyConveyorLine>();
    private readonly HashSet<MyShipConnector> m_connectors = new HashSet<MyShipConnector>();
    private bool m_needsRecomputation = true;
    private HashSet<MyCubeGrid> m_tmpConnectedGrids = new HashSet<MyCubeGrid>();
    [ThreadStatic]
    private static List<MyPhysicalInventoryItem> m_tmpInventoryItems;
    [ThreadStatic]
    private static MyGridConveyorSystem.PullRequestItemSet m_tmpRequestedItemSetPerThread;
    [ThreadStatic]
    private static MyPathFindingSystem<IMyConveyorEndpoint> m_pathfinding = new MyPathFindingSystem<IMyConveyorEndpoint>();
    private static Dictionary<(IMyConveyorEndpointBlock, IMyConveyorEndpointBlock), Task> m_currentTransferComputationTasks = new Dictionary<(IMyConveyorEndpointBlock, IMyConveyorEndpointBlock), Task>();
    private Dictionary<ConveyorLinePosition, MyConveyorLine> m_lineEndpoints;
    private Dictionary<Vector3I, MyConveyorLine> m_linePoints;
    private HashSet<MyConveyorLine> m_deserializedLines;
    public bool IsClosing;
    public MyStringId HudMessage = MyStringId.NullOrEmpty;
    public string HudMessageCustom = string.Empty;
    [ThreadStatic]
    private static long m_playerIdForAccessiblePredicate;
    [ThreadStatic]
    private static MyDefinitionId m_inventoryItemDefinitionId;
    private static Predicate<IMyConveyorEndpoint> IsAccessAllowedPredicate = new Predicate<IMyConveyorEndpoint>(MyGridConveyorSystem.IsAccessAllowed);
    private static Predicate<IMyPathEdge<IMyConveyorEndpoint>> IsConveyorLargePredicate = new Predicate<IMyPathEdge<IMyConveyorEndpoint>>(MyGridConveyorSystem.IsConveyorLarge);
    private static Predicate<IMyPathEdge<IMyConveyorEndpoint>> IsConveyorSmallPredicate = new Predicate<IMyPathEdge<IMyConveyorEndpoint>>(MyGridConveyorSystem.IsConveyorSmall);
    [ThreadStatic]
    private static List<IMyConveyorEndpoint> m_reachableBuffer;
    private Dictionary<IMyConveyorEndpointBlock, MyGridConveyorSystem.ConveyorEndpointMapping> m_conveyorConnections = new Dictionary<IMyConveyorEndpointBlock, MyGridConveyorSystem.ConveyorEndpointMapping>();
    private bool m_isRecomputingGraph;
    private bool m_isRecomputationInterrupted;
    private bool m_isRecomputationIsAborted;
    private const double MAX_RECOMPUTE_DURATION_MILLISECONDS = 10.0;
    private Dictionary<IMyConveyorEndpointBlock, MyGridConveyorSystem.ConveyorEndpointMapping> m_conveyorConnectionsForThread = new Dictionary<IMyConveyorEndpointBlock, MyGridConveyorSystem.ConveyorEndpointMapping>();
    private IEnumerator<IMyConveyorEndpointBlock> m_endpointIterator;
    private FastResourceLock m_iteratorLock = new FastResourceLock();
    public bool NeedsUpdateLines;

    public event Action<MyCubeBlock> BlockAdded;

    public event Action<MyCubeBlock> BlockRemoved;

    public event Action<IMyConveyorEndpointBlock> OnBeforeRemoveEndpointBlock;

    public event Action<IMyConveyorSegmentBlock> OnBeforeRemoveSegmentBlock;

    private static MyGridConveyorSystem.PullRequestItemSet m_tmpRequestedItemSet
    {
      get
      {
        if (MyGridConveyorSystem.m_tmpRequestedItemSetPerThread == null)
          MyGridConveyorSystem.m_tmpRequestedItemSetPerThread = new MyGridConveyorSystem.PullRequestItemSet();
        return MyGridConveyorSystem.m_tmpRequestedItemSetPerThread;
      }
    }

    private static MyPathFindingSystem<IMyConveyorEndpoint> Pathfinding
    {
      get
      {
        if (MyGridConveyorSystem.m_pathfinding == null)
          MyGridConveyorSystem.m_pathfinding = new MyPathFindingSystem<IMyConveyorEndpoint>();
        return MyGridConveyorSystem.m_pathfinding;
      }
    }

    public MyResourceSinkComponent ResourceSink { get; private set; }

    public bool IsInteractionPossible
    {
      get
      {
        bool flag = false;
        foreach (MyShipConnector connector in this.m_connectors)
          flag |= connector.InConstraint;
        return flag;
      }
    }

    public bool Connected
    {
      get
      {
        bool flag = false;
        foreach (MyShipConnector connector in this.m_connectors)
          flag |= connector.Connected;
        return flag;
      }
    }

    public HashSetReader<IMyConveyorEndpointBlock> ConveyorEndpointBlocks => new HashSetReader<IMyConveyorEndpointBlock>(this.m_conveyorEndpointBlocks);

    public MyGridConveyorSystem(MyCubeGrid grid)
      : base(grid)
    {
      this.m_lineEndpoints = (Dictionary<ConveyorLinePosition, MyConveyorLine>) null;
      this.m_linePoints = (Dictionary<Vector3I, MyConveyorLine>) null;
      this.m_deserializedLines = (HashSet<MyConveyorLine>) null;
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSink.Init(MyStringHash.GetOrCompute("Conveyors"), MyGridConveyorSystem.CONVEYOR_SYSTEM_CONSUMPTION, (Func<float>) (() => MyGridConveyorSystem.CONVEYOR_SYSTEM_CONSUMPTION));
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
    }

    public void BeforeBlockDeserialization(List<MyObjectBuilder_ConveyorLine> lines)
    {
      if (lines == null)
        return;
      this.m_lineEndpoints = new Dictionary<ConveyorLinePosition, MyConveyorLine>(lines.Count * 2);
      this.m_linePoints = new Dictionary<Vector3I, MyConveyorLine>(lines.Count * 4);
      this.m_deserializedLines = new HashSet<MyConveyorLine>();
      foreach (MyObjectBuilder_ConveyorLine line in lines)
      {
        MyConveyorLine myConveyorLine = new MyConveyorLine();
        myConveyorLine.Init(line, this.Grid);
        if (myConveyorLine.CheckSectionConsistency())
        {
          ConveyorLinePosition key1 = new ConveyorLinePosition((Vector3I) line.StartPosition, line.StartDirection);
          ConveyorLinePosition key2 = new ConveyorLinePosition((Vector3I) line.EndPosition, line.EndDirection);
          try
          {
            this.m_lineEndpoints.Add(key1, myConveyorLine);
            this.m_lineEndpoints.Add(key2, myConveyorLine);
            foreach (Vector3I key3 in myConveyorLine)
              this.m_linePoints.Add(key3, myConveyorLine);
            this.m_deserializedLines.Add(myConveyorLine);
            this.m_lines.Add(myConveyorLine);
          }
          catch (ArgumentException ex)
          {
            this.m_lineEndpoints = (Dictionary<ConveyorLinePosition, MyConveyorLine>) null;
            this.m_deserializedLines = (HashSet<MyConveyorLine>) null;
            this.m_linePoints = (Dictionary<Vector3I, MyConveyorLine>) null;
            this.m_lines.Clear();
            break;
          }
        }
      }
    }

    public MyConveyorLine GetDeserializingLine(ConveyorLinePosition position)
    {
      if (this.m_lineEndpoints == null)
        return (MyConveyorLine) null;
      MyConveyorLine myConveyorLine;
      this.m_lineEndpoints.TryGetValue(position, out myConveyorLine);
      return myConveyorLine;
    }

    public MyConveyorLine GetDeserializingLine(Vector3I position)
    {
      if (this.m_linePoints == null)
        return (MyConveyorLine) null;
      MyConveyorLine myConveyorLine;
      this.m_linePoints.TryGetValue(position, out myConveyorLine);
      return myConveyorLine;
    }

    public void AfterBlockDeserialization()
    {
      this.m_lineEndpoints = (Dictionary<ConveyorLinePosition, MyConveyorLine>) null;
      this.m_linePoints = (Dictionary<Vector3I, MyConveyorLine>) null;
      this.m_deserializedLines = (HashSet<MyConveyorLine>) null;
      foreach (MyConveyorLine line in this.m_lines)
        line.UpdateIsFunctional();
    }

    public void SerializeLines(List<MyObjectBuilder_ConveyorLine> resultList)
    {
      foreach (MyConveyorLine line in this.m_lines)
      {
        if (!line.IsEmpty || !line.IsDisconnected || line.Length != 1)
          resultList.Add(line.GetObjectBuilder());
      }
    }

    public void AfterGridClose()
    {
      this.m_lines.Clear();
      this.m_conveyorConnections.Clear();
    }

    public void Add(MyCubeBlock block)
    {
      this.m_inventoryBlocks.Add(block);
      Action<MyCubeBlock> blockAdded = this.BlockAdded;
      if (blockAdded == null)
        return;
      blockAdded(block);
    }

    public void Remove(MyCubeBlock block)
    {
      this.m_inventoryBlocks.Remove(block);
      Action<MyCubeBlock> blockRemoved = this.BlockRemoved;
      if (blockRemoved == null)
        return;
      blockRemoved(block);
    }

    internal void GetGridInventories(
      MyEntity interactedAsEntity,
      List<MyEntity> outputInventories,
      long identityId)
    {
      this.GetGridInventories(interactedAsEntity, outputInventories, identityId, (List<long>) null);
    }

    internal void GetGridInventories(
      MyEntity interactedAsEntity,
      List<MyEntity> outputInventories,
      long identityId,
      List<long> inventoryIds = null)
    {
      foreach (MyCubeBlock inventoryBlock in this.m_inventoryBlocks)
      {
        if ((!(inventoryBlock is MyTerminalBlock) || (inventoryBlock as MyTerminalBlock).HasPlayerAccess(identityId)) && (interactedAsEntity == inventoryBlock || !(inventoryBlock is MyTerminalBlock) || (inventoryBlock as MyTerminalBlock).ShowInInventory))
        {
          outputInventories?.Add((MyEntity) inventoryBlock);
          inventoryIds?.Add(inventoryBlock.EntityId);
        }
      }
    }

    public void AddConveyorBlock(IMyConveyorEndpointBlock endpointBlock)
    {
      using (this.m_iteratorLock.AcquireExclusiveUsing())
      {
        this.m_endpointIterator = (IEnumerator<IMyConveyorEndpointBlock>) null;
        this.m_conveyorEndpointBlocks.Add(endpointBlock);
        if (endpointBlock is MyShipConnector)
          this.m_connectors.Add(endpointBlock as MyShipConnector);
        IMyConveyorEndpoint conveyorEndpoint = endpointBlock.ConveyorEndpoint;
        for (int index = 0; index < conveyorEndpoint.GetLineCount(); ++index)
        {
          ConveyorLinePosition position = conveyorEndpoint.GetPosition(index);
          MyConveyorLine conveyorLine = conveyorEndpoint.GetConveyorLine(index);
          if (this.m_deserializedLines == null || !this.m_deserializedLines.Contains(conveyorLine))
          {
            MySlimBlock cubeBlock = this.Grid.GetCubeBlock(position.NeighbourGridPosition);
            if (cubeBlock == null)
            {
              this.m_lines.Add(conveyorLine);
            }
            else
            {
              IMyConveyorEndpointBlock fatBlock1 = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
              if (cubeBlock.FatBlock is IMyConveyorSegmentBlock fatBlock)
              {
                if (!this.TryMergeEndpointSegment(endpointBlock, fatBlock, position))
                  this.m_lines.Add(conveyorLine);
              }
              else if (fatBlock1 != null)
              {
                if (!this.TryMergeEndpointEndpoint(endpointBlock, fatBlock1, position, position.GetConnectingPosition()))
                  this.m_lines.Add(conveyorLine);
              }
              else
                this.m_lines.Add(conveyorLine);
            }
          }
        }
      }
    }

    public void DebugDraw(MyCubeGrid grid)
    {
      foreach (MyConveyorLine line in this.m_lines)
        line.DebugDraw(grid);
      MyRenderProxy.DebugDrawText2D(new Vector2(1f, 1f), "Conveyor lines: " + (object) this.m_lines.Count, Color.Red, 1f);
    }

    public void DebugDrawLinePackets()
    {
      foreach (MyConveyorLine line in this.m_lines)
        line.DebugDrawPackets();
    }

    public void UpdateBeforeSimulation()
    {
      MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateBeforeSimulation));
      foreach (MyConveyorLine line in this.m_lines)
      {
        if (!line.IsEmpty)
          line.Update();
      }
      MySimpleProfiler.End(nameof (UpdateBeforeSimulation));
    }

    public void UpdateBeforeSimulation10()
    {
      MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateBeforeSimulation10));
      this.ResourceSink.Update();
      MySimpleProfiler.End(nameof (UpdateBeforeSimulation10));
    }

    public void FlagForRecomputation()
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group group = MyGridPhysicalHierarchy.Static.GetGroup(this.Grid);
      if (group == null)
        return;
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node in group.Nodes)
        node.NodeData.GridSystems.ConveyorSystem.m_needsRecomputation = true;
    }

    protected override void Update() => this.UpdateLinesLazy();

    public void UpdateAfterSimulation100()
    {
      if (!this.m_needsRecomputation)
        return;
      MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateAfterSimulation100));
      this.RecomputeConveyorEndpoints();
      this.m_needsRecomputation = false;
      MySimpleProfiler.End(nameof (UpdateAfterSimulation100));
    }

    private void Receiver_IsPoweredChanged() => this.UpdateLines();

    public void UpdateLines()
    {
      this.Schedule();
      this.NeedsUpdateLines = true;
    }

    public void UpdateLinesLazy()
    {
      if (!this.NeedsUpdateLines)
        return;
      this.NeedsUpdateLines = false;
      this.FlagForRecomputation();
    }

    public void RemoveConveyorBlock(IMyConveyorEndpointBlock block)
    {
      using (this.m_iteratorLock.AcquireExclusiveUsing())
      {
        this.m_endpointIterator = (IEnumerator<IMyConveyorEndpointBlock>) null;
        this.m_conveyorEndpointBlocks.Remove(block);
        if (block is MyShipConnector)
          this.m_connectors.Remove(block as MyShipConnector);
        if (this.IsClosing)
          return;
        if (this.OnBeforeRemoveEndpointBlock != null)
          this.OnBeforeRemoveEndpointBlock(block);
        for (int index = 0; index < block.ConveyorEndpoint.GetLineCount(); ++index)
        {
          MyConveyorLine conveyorLine = block.ConveyorEndpoint.GetConveyorLine(index);
          conveyorLine.DisconnectEndpoint(block.ConveyorEndpoint);
          if (conveyorLine.IsDegenerate)
            this.m_lines.Remove(conveyorLine);
        }
      }
    }

    public void AddSegmentBlock(IMyConveyorSegmentBlock segmentBlock)
    {
      this.AddSegmentBlockInternal(segmentBlock, segmentBlock.ConveyorSegment.ConnectingPosition1);
      this.AddSegmentBlockInternal(segmentBlock, segmentBlock.ConveyorSegment.ConnectingPosition2);
      if (this.m_lines.Contains(segmentBlock.ConveyorSegment.ConveyorLine) || segmentBlock.ConveyorSegment.ConveyorLine == null)
        return;
      this.m_lines.Add(segmentBlock.ConveyorSegment.ConveyorLine);
    }

    public void RemoveSegmentBlock(IMyConveyorSegmentBlock segmentBlock)
    {
      if (this.IsClosing)
        return;
      if (this.OnBeforeRemoveSegmentBlock != null)
        this.OnBeforeRemoveSegmentBlock(segmentBlock);
      MyConveyorLine conveyorLine = segmentBlock.ConveyorSegment.ConveyorLine;
      MyConveyorLine myConveyorLine = segmentBlock.ConveyorSegment.ConveyorLine.RemovePortion(segmentBlock.ConveyorSegment.ConnectingPosition1.NeighbourGridPosition, segmentBlock.ConveyorSegment.ConnectingPosition2.NeighbourGridPosition);
      if (conveyorLine.IsDegenerate)
        this.m_lines.Remove(conveyorLine);
      if (myConveyorLine == null)
        return;
      this.UpdateLineReferences(myConveyorLine, myConveyorLine);
      this.m_lines.Add(myConveyorLine);
    }

    private void AddSegmentBlockInternal(
      IMyConveyorSegmentBlock segmentBlock,
      ConveyorLinePosition connectingPosition)
    {
      MySlimBlock cubeBlock = this.Grid.GetCubeBlock(connectingPosition.LocalGridPosition);
      if (cubeBlock == null || this.m_deserializedLines != null && this.m_deserializedLines.Contains(segmentBlock.ConveyorSegment.ConveyorLine))
        return;
      IMyConveyorEndpointBlock fatBlock1 = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
      if (cubeBlock.FatBlock is IMyConveyorSegmentBlock fatBlock2)
      {
        MyConveyorLine conveyorLine = segmentBlock.ConveyorSegment.ConveyorLine;
        if (this.m_lines.Contains(conveyorLine))
          this.m_lines.Remove(conveyorLine);
        if (fatBlock2.ConveyorSegment.CanConnectTo(connectingPosition, segmentBlock.ConveyorSegment.ConveyorLine.Type))
          this.MergeSegmentSegment(segmentBlock, fatBlock2);
      }
      if (fatBlock1 == null)
        return;
      MyConveyorLine conveyorLine1 = fatBlock1.ConveyorEndpoint.GetConveyorLine(connectingPosition);
      if (!this.TryMergeEndpointSegment(fatBlock1, segmentBlock, connectingPosition))
        return;
      this.m_lines.Remove(conveyorLine1);
    }

    private bool TryMergeEndpointSegment(
      IMyConveyorEndpointBlock endpoint,
      IMyConveyorSegmentBlock segmentBlock,
      ConveyorLinePosition endpointPosition)
    {
      MyConveyorLine conveyorLine1 = endpoint.ConveyorEndpoint.GetConveyorLine(endpointPosition);
      if (conveyorLine1 == null || !segmentBlock.ConveyorSegment.CanConnectTo(endpointPosition.GetConnectingPosition(), conveyorLine1.Type))
        return false;
      MyConveyorLine conveyorLine2 = segmentBlock.ConveyorSegment.ConveyorLine;
      conveyorLine2.Merge(conveyorLine1, segmentBlock);
      endpoint.ConveyorEndpoint.SetConveyorLine(endpointPosition, conveyorLine2);
      conveyorLine1.RecalculateConductivity();
      conveyorLine2.RecalculateConductivity();
      return true;
    }

    private bool TryMergeEndpointEndpoint(
      IMyConveyorEndpointBlock endpointBlock1,
      IMyConveyorEndpointBlock endpointBlock2,
      ConveyorLinePosition pos1,
      ConveyorLinePosition pos2)
    {
      MyConveyorLine conveyorLine1 = endpointBlock1.ConveyorEndpoint.GetConveyorLine(pos1);
      if (conveyorLine1 == null)
        return false;
      MyConveyorLine conveyorLine2 = endpointBlock2.ConveyorEndpoint.GetConveyorLine(pos2);
      if (conveyorLine2 == null || conveyorLine1.Type != conveyorLine2.Type)
        return false;
      if (conveyorLine1.GetEndpoint(1) == null)
        conveyorLine1.Reverse();
      if (conveyorLine2.GetEndpoint(0) == null)
        conveyorLine2.Reverse();
      conveyorLine2.Merge(conveyorLine1);
      endpointBlock1.ConveyorEndpoint.SetConveyorLine(pos1, conveyorLine2);
      conveyorLine1.RecalculateConductivity();
      conveyorLine2.RecalculateConductivity();
      return true;
    }

    private void MergeSegmentSegment(
      IMyConveyorSegmentBlock newSegmentBlock,
      IMyConveyorSegmentBlock oldSegmentBlock)
    {
      MyConveyorLine conveyorLine1 = newSegmentBlock.ConveyorSegment.ConveyorLine;
      MyConveyorLine conveyorLine2 = oldSegmentBlock.ConveyorSegment.ConveyorLine;
      if (conveyorLine1 != conveyorLine2)
        conveyorLine2.Merge(conveyorLine1, newSegmentBlock);
      this.UpdateLineReferences(conveyorLine1, conveyorLine2);
      newSegmentBlock.ConveyorSegment.SetConveyorLine(conveyorLine2);
    }

    private void UpdateLineReferences(MyConveyorLine oldLine, MyConveyorLine newLine)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (oldLine.GetEndpoint(index) != null)
          oldLine.GetEndpoint(index).SetConveyorLine(oldLine.GetEndpointPosition(index), newLine);
      }
      foreach (Vector3I pos in oldLine)
      {
        MySlimBlock cubeBlock = this.Grid.GetCubeBlock(pos);
        if (cubeBlock != null && cubeBlock.FatBlock is IMyConveyorSegmentBlock fatBlock)
          fatBlock.ConveyorSegment.SetConveyorLine(newLine);
      }
      oldLine.RecalculateConductivity();
      newLine.RecalculateConductivity();
    }

    public void ToggleConnectors()
    {
      bool flag = false;
      foreach (MyShipConnector connector in this.m_connectors)
        flag |= connector.Connected;
      foreach (MyShipConnector connector in this.m_connectors)
      {
        if (connector.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies)
        {
          if (flag && connector.Connected)
          {
            connector.TryDisconnect();
            this.HudMessage = MySpaceTexts.NotificationConnectorsDisabled;
          }
          if (!flag)
          {
            if (connector.IsProtectedFromLockingByTrading() || connector.InConstraint && connector.Other.IsProtectedFromLockingByTrading())
            {
              this.HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_TemporaryBlock), (object) connector.GetProtectionFromLockingTime());
            }
            else
            {
              connector.TryConnect();
              if (connector.InConstraint)
              {
                this.HudMessage = MySpaceTexts.NotificationConnectorsEnabled;
                if ((double) (float) connector.AutoUnlockTime > 0.0 || (double) (float) connector.Other.AutoUnlockTime > 0.0)
                {
                  float num1 = (double) (float) connector.AutoUnlockTime <= 0.0 ? (float) connector.Other.AutoUnlockTime : ((double) (float) connector.Other.AutoUnlockTime <= 0.0 ? (float) connector.AutoUnlockTime : Math.Min((float) connector.AutoUnlockTime, (float) connector.Other.AutoUnlockTime));
                  int num2 = (int) ((double) num1 / 60.0);
                  int num3 = (int) ((double) num1 - (double) (num2 * 60));
                  this.HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_AutoUnlockWarning), (object) num2, (object) num3);
                }
              }
              else
                this.HudMessage = MyStringId.NullOrEmpty;
            }
          }
        }
      }
    }

    private static void SetTraversalPlayerId(long playerId) => MyGridConveyorSystem.m_playerIdForAccessiblePredicate = playerId;

    private static void SetTraversalInventoryItemDefinitionId(MyDefinitionId item = default (MyDefinitionId)) => MyGridConveyorSystem.m_inventoryItemDefinitionId = item;

    private static bool IsAccessAllowed(IMyConveyorEndpoint endpoint)
    {
      if (endpoint.CubeBlock.GetUserRelationToOwner(MyGridConveyorSystem.m_playerIdForAccessiblePredicate) == MyRelationsBetweenPlayerAndBlock.Enemies)
        return false;
      if (endpoint.CubeBlock is MyConveyorSorter cubeBlock && MyGridConveyorSystem.m_inventoryItemDefinitionId != new MyDefinitionId())
        return cubeBlock.IsAllowed(MyGridConveyorSystem.m_inventoryItemDefinitionId);
      return !(endpoint.CubeBlock is MyShipConnector cubeBlock) || !(bool) cubeBlock.TradingEnabled;
    }

    private static bool IsConveyorLarge(IMyPathEdge<IMyConveyorEndpoint> conveyorLine) => !(conveyorLine is MyConveyorLine) || (conveyorLine as MyConveyorLine).Type == MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE;

    private static bool IsConveyorSmall(IMyPathEdge<IMyConveyorEndpoint> conveyorLine) => !(conveyorLine is MyConveyorLine) || (conveyorLine as MyConveyorLine).Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;

    private static bool NeedsLargeTube(MyDefinitionId itemDefinitionId)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(itemDefinitionId);
      if (physicalItemDefinition == null)
        return true;
      return !(itemDefinitionId.TypeId == typeof (MyObjectBuilder_PhysicalGunObject)) && (double) physicalItemDefinition.Size.AbsMax() > 0.25;
    }

    public static void AppendReachableEndpoints(
      IMyConveyorEndpoint source,
      long playerId,
      List<IMyConveyorEndpoint> reachable,
      MyDefinitionId itemId,
      Predicate<IMyConveyorEndpoint> endpointFilter = null)
    {
      if (!(source.CubeBlock is IMyConveyorEndpointBlock cubeBlock))
        return;
      lock (MyGridConveyorSystem.Pathfinding)
      {
        MyGridConveyorSystem.SetTraversalPlayerId(playerId);
        MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId(itemId);
        MyGridConveyorSystem.Pathfinding.FindReachable(cubeBlock.ConveyorEndpoint, reachable, endpointFilter, MyGridConveyorSystem.IsAccessAllowedPredicate, MyGridConveyorSystem.NeedsLargeTube(itemId) ? MyGridConveyorSystem.IsConveyorLargePredicate : (Predicate<IMyPathEdge<IMyConveyorEndpoint>>) null);
      }
    }

    public static bool Reachable(
      IMyConveyorEndpoint source,
      IMyConveyorEndpoint endPoint,
      long playerId,
      MyDefinitionId itemId,
      Predicate<IMyConveyorEndpoint> endpointFilter = null)
    {
      if (!(source.CubeBlock is IMyConveyorEndpointBlock cubeBlock))
        return false;
      lock (MyGridConveyorSystem.Pathfinding)
      {
        MyGridConveyorSystem.SetTraversalPlayerId(playerId);
        MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId(itemId);
        return MyGridConveyorSystem.Pathfinding.Reachable(cubeBlock.ConveyorEndpoint, endPoint, endpointFilter, MyGridConveyorSystem.IsAccessAllowedPredicate, MyGridConveyorSystem.NeedsLargeTube(itemId) ? MyGridConveyorSystem.IsConveyorLargePredicate : (Predicate<IMyPathEdge<IMyConveyorEndpoint>>) null);
      }
    }

    public HashSetReader<MyCubeBlock> InventoryBlocks => new HashSetReader<MyCubeBlock>(this.m_inventoryBlocks);

    public static bool PullAllRequestForSorter(
      IMyConveyorEndpointBlock start,
      MyInventory destinationInventory,
      MyInventoryConstraint requestedTypeIds,
      int maxItemsToPull)
    {
      if (!(start is MyCubeBlock myCubeBlock) || MyGridConveyorSystem.IsComputationTasksCountOverLimit())
        return false;
      MyGridConveyorSystem.m_tmpRequestedItemSet.Set(requestedTypeIds);
      MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
      bool flag1 = false;
      bool flag2 = false;
      if (conveyorEndpointMapping.pullElements != null)
      {
        for (int index1 = 0; index1 < conveyorEndpointMapping.pullElements.Count && (double) destinationInventory.VolumeFillFactor < 0.990000009536743; ++index1)
        {
          IMyConveyorEndpointBlock pullElement = conveyorEndpointMapping.pullElements[index1];
          if (pullElement is MyCubeBlock thisEntity && start != pullElement && !MyGridConveyorSystem.HasComputationTask(start, pullElement))
          {
            int inventoryCount = thisEntity.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(thisEntity, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != (MyInventoryFlags) 0 && inventory != destinationInventory)
              {
                using (MyUtils.ReuseCollection<MyPhysicalInventoryItem>(ref MyGridConveyorSystem.m_tmpInventoryItems))
                {
                  foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
                  {
                    MyDefinitionId id = physicalInventoryItem.Content.GetId();
                    if (requestedTypeIds == null || MyGridConveyorSystem.m_tmpRequestedItemSet.Contains(id))
                    {
                      if (!MyGridConveyorSystem.CanTransfer(start, pullElement, id, false))
                      {
                        if (MyGridConveyorSystem.HasComputationTask(start, pullElement))
                        {
                          flag1 = true;
                          break;
                        }
                      }
                      else
                      {
                        flag2 = true;
                        MyGridConveyorSystem.m_tmpInventoryItems.Add(physicalInventoryItem);
                        if (MyGridConveyorSystem.m_tmpInventoryItems.Count == maxItemsToPull)
                          break;
                      }
                    }
                  }
                  foreach (MyPhysicalInventoryItem tmpInventoryItem in MyGridConveyorSystem.m_tmpInventoryItems)
                  {
                    if ((double) destinationInventory.VolumeFillFactor >= 1.0)
                      return flag1;
                    MyFixedPoint b = tmpInventoryItem.Amount;
                    if (!MySession.Static.CreativeMode)
                    {
                      MyFixedPoint a = destinationInventory.ComputeAmountThatFits(tmpInventoryItem.Content.GetId(), 0.0f, 0.0f);
                      if (tmpInventoryItem.Content.TypeId != typeof (MyObjectBuilder_Ore) && tmpInventoryItem.Content.TypeId != typeof (MyObjectBuilder_Ingot))
                        a = MyFixedPoint.Floor(a);
                      b = MyFixedPoint.Min(a, b);
                    }
                    if (!(b == (MyFixedPoint) 0))
                    {
                      MyInventory.Transfer(inventory, destinationInventory, tmpInventoryItem.Content.GetId(), amount: new MyFixedPoint?(b));
                      if ((double) destinationInventory.VolumeFillFactor >= 0.990000009536743)
                        break;
                    }
                  }
                }
                if (MyGridConveyorSystem.IsComputationTasksCountOverLimit() || (double) destinationInventory.VolumeFillFactor >= 0.990000009536743)
                  break;
              }
            }
          }
        }
      }
      else if (!conveyorSystem.m_isRecomputingGraph)
        conveyorSystem.RecomputeConveyorEndpoints();
      return flag1 && !flag2;
    }

    public static bool PullAllItemsForConnector(
      IMyConveyorEndpointBlock start,
      MyInventory destinationInventory,
      long playerId,
      float maxVolumeFillFactor)
    {
      if (!(start is MyCubeBlock myCubeBlock) || MyGridConveyorSystem.IsComputationTasksCountOverLimit())
        return false;
      bool flag = false;
      MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
      if (conveyorEndpointMapping.pullElements != null)
      {
        for (int index1 = 0; index1 < conveyorEndpointMapping.pullElements.Count && (double) destinationInventory.VolumeFillFactor < (double) maxVolumeFillFactor; ++index1)
        {
          IMyConveyorEndpointBlock pullElement = conveyorEndpointMapping.pullElements[index1];
          if (pullElement is MyCubeBlock thisEntity && start != pullElement && (!(thisEntity is MyShipConnector myShipConnector) || !(bool) myShipConnector.CollectAll) && !MyGridConveyorSystem.HasComputationTask(start, pullElement))
          {
            int inventoryCount = thisEntity.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(thisEntity, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != (MyInventoryFlags) 0 && inventory != destinationInventory)
              {
                List<MyPhysicalInventoryItem> items = inventory.GetItems();
                for (int index3 = 0; index3 < items.Count; ++index3)
                {
                  MyDefinitionId definitionId = items[index3].GetDefinitionId();
                  MyFixedPoint amountThatFits = destinationInventory.ComputeAmountThatFits(definitionId, 0.0f, 0.0f);
                  if (!(amountThatFits <= (MyFixedPoint) 0))
                  {
                    if (!MyGridConveyorSystem.CanTransfer(start, pullElement, definitionId, false))
                    {
                      if (MyGridConveyorSystem.HasComputationTask(start, pullElement))
                        break;
                    }
                    else
                    {
                      MyFixedPoint myFixedPoint = MyFixedPoint.Min(inventory.GetItemAmount(definitionId, MyItemFlags.None, false), amountThatFits);
                      MyInventory.Transfer(inventory, destinationInventory, definitionId, amount: new MyFixedPoint?(myFixedPoint));
                      flag = true;
                      if ((double) destinationInventory.VolumeFillFactor >= (double) maxVolumeFillFactor)
                        break;
                    }
                  }
                }
                if (MyGridConveyorSystem.IsComputationTasksCountOverLimit() || (double) destinationInventory.VolumeFillFactor >= (double) maxVolumeFillFactor)
                  break;
              }
            }
          }
        }
      }
      else if (!conveyorSystem.m_isRecomputingGraph)
        conveyorSystem.RecomputeConveyorEndpoints();
      return flag;
    }

    public static void PrepareTraversal(
      IMyConveyorEndpoint startingVertex,
      Predicate<IMyConveyorEndpoint> vertexFilter = null,
      Predicate<IMyConveyorEndpoint> vertexTraversable = null,
      Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null)
    {
      lock (MyGridConveyorSystem.Pathfinding)
        MyGridConveyorSystem.Pathfinding.PrepareTraversal(startingVertex, vertexFilter, vertexTraversable, edgeTraversable);
    }

    public static bool ComputeCanTransfer(
      IMyConveyorEndpointBlock start,
      IMyConveyorEndpointBlock end,
      MyDefinitionId? itemId)
    {
      using (MyUtils.ReuseCollection<IMyConveyorEndpoint>(ref MyGridConveyorSystem.m_reachableBuffer))
      {
        lock (MyGridConveyorSystem.Pathfinding)
        {
          MyGridConveyorSystem.SetTraversalPlayerId(start.ConveyorEndpoint.CubeBlock.OwnerId);
          if (itemId.HasValue)
            MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId(itemId.Value);
          else
            MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId();
          Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = (Predicate<IMyPathEdge<IMyConveyorEndpoint>>) null;
          if (itemId.HasValue && MyGridConveyorSystem.NeedsLargeTube(itemId.Value))
            edgeTraversable = MyGridConveyorSystem.IsConveyorLargePredicate;
          MyGridConveyorSystem.Pathfinding.FindReachable(start.ConveyorEndpoint, MyGridConveyorSystem.m_reachableBuffer, (Predicate<IMyConveyorEndpoint>) (b => b != null && b.CubeBlock == end), MyGridConveyorSystem.IsAccessAllowedPredicate, edgeTraversable);
        }
        return (uint) MyGridConveyorSystem.m_reachableBuffer.Count > 0U;
      }
    }

    private static bool CanTransfer(
      IMyConveyorEndpointBlock start,
      IMyConveyorEndpointBlock endPoint,
      MyDefinitionId itemId,
      bool isPush,
      bool calcImmediately = false)
    {
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = ((MyCubeBlock) start).CubeGrid.GridSystems.ConveyorSystem.GetConveyorEndpointMapping(start);
      if (calcImmediately)
      {
        bool canTransfer;
        if (conveyorEndpointMapping.TryGetTransfer(endPoint, itemId, isPush, out canTransfer))
          return canTransfer;
        MyGridConveyorSystem.TransferData transferData = new MyGridConveyorSystem.TransferData(start, endPoint, itemId, isPush);
        transferData.ComputeTransfer();
        transferData.StoreTransferState();
        return transferData.m_canTransfer;
      }
      bool canTransfer1;
      if (conveyorEndpointMapping.TryGetTransfer(endPoint, itemId, isPush, out canTransfer1))
        return canTransfer1;
      lock (MyGridConveyorSystem.m_currentTransferComputationTasks)
      {
        (IMyConveyorEndpointBlock, IMyConveyorEndpointBlock) key = (start, endPoint);
        if (!MyGridConveyorSystem.m_currentTransferComputationTasks.ContainsKey(key))
        {
          Task task = Parallel.Start(new Action<WorkData>(MyGridConveyorSystem.ComputeTransferData), new Action<WorkData>(MyGridConveyorSystem.OnTransferDataComputed), (WorkData) new MyGridConveyorSystem.TransferData(start, endPoint, itemId, isPush));
          MyGridConveyorSystem.m_currentTransferComputationTasks.Add(key, task);
        }
      }
      return false;
    }

    private static bool HasComputationTask(
      IMyConveyorEndpointBlock start,
      IMyConveyorEndpointBlock endPoint)
    {
      lock (MyGridConveyorSystem.m_currentTransferComputationTasks)
      {
        (IMyConveyorEndpointBlock, IMyConveyorEndpointBlock) key = (start, endPoint);
        return MyGridConveyorSystem.m_currentTransferComputationTasks.ContainsKey(key);
      }
    }

    private static bool IsComputationTasksCountOverLimit()
    {
      lock (MyGridConveyorSystem.m_currentTransferComputationTasks)
        return MyGridConveyorSystem.m_currentTransferComputationTasks.Count > MyGridConveyorSystem.MAX_COMPUTATION_TASKS_FOR_ALL_REQUESTS;
    }

    private static void ComputeTransferData(WorkData workData)
    {
      if (workData == null)
        return;
      if (!(workData is MyGridConveyorSystem.TransferData transferData))
        workData.FlagAsFailed();
      else
        transferData.ComputeTransfer();
    }

    private static void OnTransferDataComputed(WorkData workData)
    {
      if (workData == null && MyFakes.FORCE_NO_WORKER)
        MyLog.Default.WriteLine("OnTransferDataComputed: workData is null on MyGridConveyorSystem to Check");
      else if (!(workData is MyGridConveyorSystem.TransferData transferData))
      {
        workData.FlagAsFailed();
      }
      else
      {
        transferData.StoreTransferState();
        lock (MyGridConveyorSystem.m_currentTransferComputationTasks)
          MyGridConveyorSystem.m_currentTransferComputationTasks.Remove((transferData.m_start, transferData.m_endPoint));
      }
    }

    internal bool PullItem(
      MyDefinitionId itemId,
      MyFixedPoint amount,
      IMyConveyorEndpointBlock start,
      MyInventory destinationInventory,
      bool calcImmediately = false)
    {
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = this.GetConveyorEndpointMapping(start);
      if (conveyorEndpointMapping.pullElements != null)
      {
        List<Tuple<MyInventory, MyFixedPoint>> tupleList = new List<Tuple<MyInventory, MyFixedPoint>>();
        MyFixedPoint b = amount;
        for (int index1 = 0; index1 < conveyorEndpointMapping.pullElements.Count; ++index1)
        {
          if (conveyorEndpointMapping.pullElements[index1] is MyCubeBlock pullElement)
          {
            int inventoryCount = pullElement.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(pullElement, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != (MyInventoryFlags) 0 && inventory != destinationInventory && MyGridConveyorSystem.CanTransfer(start, conveyorEndpointMapping.pullElements[index1], itemId, false, calcImmediately))
              {
                MyFixedPoint myFixedPoint = MyFixedPoint.Min(inventory.GetItemAmount(itemId, MyItemFlags.None, false), b);
                if (!(myFixedPoint == (MyFixedPoint) 0))
                {
                  tupleList.Add(new Tuple<MyInventory, MyFixedPoint>(inventory, myFixedPoint));
                  b -= myFixedPoint;
                  if (b == (MyFixedPoint) 0)
                    break;
                }
              }
            }
            if (b == (MyFixedPoint) 0)
              break;
          }
        }
        if (b != (MyFixedPoint) 0)
          return false;
        foreach (Tuple<MyInventory, MyFixedPoint> tuple in tupleList)
          MyInventory.Transfer(tuple.Item1, destinationInventory, itemId, amount: new MyFixedPoint?(tuple.Item2));
      }
      else if (!this.m_isRecomputingGraph)
        this.RecomputeConveyorEndpoints();
      return true;
    }

    public MyFixedPoint PullItem(
      MyDefinitionId itemId,
      MyFixedPoint? amount,
      IMyConveyorEndpointBlock start,
      MyInventory destinationInventory,
      bool remove,
      bool calcImmediately)
    {
      MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = this.GetConveyorEndpointMapping(start);
      if (conveyorEndpointMapping.pullElements != null)
      {
        for (int index1 = 0; index1 < conveyorEndpointMapping.pullElements.Count; ++index1)
        {
          if (conveyorEndpointMapping.pullElements[index1] is MyCubeBlock pullElement)
          {
            int inventoryCount = pullElement.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(pullElement, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != (MyInventoryFlags) 0 && inventory != destinationInventory && MyGridConveyorSystem.CanTransfer(start, conveyorEndpointMapping.pullElements[index1], itemId, false, calcImmediately))
              {
                MyFixedPoint itemAmount = inventory.GetItemAmount(itemId, MyItemFlags.None, false);
                if (amount.HasValue)
                {
                  MyFixedPoint amount1 = amount.HasValue ? MyFixedPoint.Min(itemAmount, amount.Value) : itemAmount;
                  if (!(amount1 == (MyFixedPoint) 0))
                  {
                    if (remove)
                      myFixedPoint1 += inventory.RemoveItemsOfType(amount1, itemId, MyItemFlags.None, false);
                    else
                      myFixedPoint1 += MyInventory.Transfer(inventory, destinationInventory, itemId, amount: new MyFixedPoint?(amount1));
                    MyFixedPoint? nullable = amount;
                    MyFixedPoint myFixedPoint2 = amount1;
                    amount = nullable.HasValue ? new MyFixedPoint?(nullable.GetValueOrDefault() - myFixedPoint2) : new MyFixedPoint?();
                    if (amount.Value == (MyFixedPoint) 0)
                      return myFixedPoint1;
                  }
                  else
                    continue;
                }
                else if (remove)
                  myFixedPoint1 += inventory.RemoveItemsOfType(itemAmount, itemId, MyItemFlags.None, false);
                else
                  myFixedPoint1 += MyInventory.Transfer(inventory, destinationInventory, itemId, amount: new MyFixedPoint?(itemAmount));
                if ((double) destinationInventory.CargoPercentage >= 0.990000009536743)
                  break;
              }
            }
            if ((double) destinationInventory.CargoPercentage >= 0.990000009536743)
              break;
          }
        }
      }
      else if (!this.m_isRecomputingGraph)
        this.RecomputeConveyorEndpoints();
      return myFixedPoint1;
    }

    public MyFixedPoint PullItems(
      MyInventoryConstraint inventoryConstraint,
      MyFixedPoint amount,
      IMyConveyorEndpointBlock start,
      MyInventory destinationInventory)
    {
      MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
      if ((double) destinationInventory.VolumeFillFactor >= 0.990000009536743 || inventoryConstraint == null || amount == (MyFixedPoint) 0)
        return myFixedPoint1;
      MyGridConveyorSystem.m_tmpRequestedItemSet.Set(inventoryConstraint);
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = this.GetConveyorEndpointMapping(start);
      if (conveyorEndpointMapping.pullElements != null)
      {
        for (int index1 = 0; index1 < conveyorEndpointMapping.pullElements.Count; ++index1)
        {
          if (conveyorEndpointMapping.pullElements[index1] is MyCubeBlock pullElement)
          {
            int inventoryCount = pullElement.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(pullElement, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != (MyInventoryFlags) 0 && inventory != destinationInventory)
              {
                using (MyUtils.ReuseCollection<MyPhysicalInventoryItem>(ref MyGridConveyorSystem.m_tmpInventoryItems))
                {
                  foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
                  {
                    MyDefinitionId id = physicalInventoryItem.Content.GetId();
                    if (MyGridConveyorSystem.m_tmpRequestedItemSet.Contains(id) && (!(physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content) || (double) content.GasLevel < 1.0) && MyGridConveyorSystem.CanTransfer(start, conveyorEndpointMapping.pullElements[index1], id, false))
                      MyGridConveyorSystem.m_tmpInventoryItems.Add(physicalInventoryItem);
                  }
                  foreach (MyPhysicalInventoryItem tmpInventoryItem in MyGridConveyorSystem.m_tmpInventoryItems)
                  {
                    MyFixedPoint myFixedPoint2 = MyFixedPoint.Min(tmpInventoryItem.Amount, amount);
                    MyFixedPoint myFixedPoint3 = MyInventory.Transfer(inventory, destinationInventory, tmpInventoryItem.ItemId, amount: new MyFixedPoint?(myFixedPoint2));
                    myFixedPoint1 += myFixedPoint3;
                    amount -= myFixedPoint3;
                    if ((double) destinationInventory.VolumeFillFactor >= 0.990000009536743 || amount <= (MyFixedPoint) 0)
                      return myFixedPoint1;
                  }
                }
                if ((double) destinationInventory.VolumeFillFactor >= 0.990000009536743)
                  return myFixedPoint1;
              }
            }
          }
        }
      }
      else if (!this.m_isRecomputingGraph)
        this.RecomputeConveyorEndpoints();
      return myFixedPoint1;
    }

    public static MyFixedPoint ConveyorSystemItemAmount(
      IMyConveyorEndpointBlock start,
      MyInventory destinationInventory,
      long playerId,
      MyDefinitionId itemId)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      using (new MyConveyorLine.InvertedConductivity())
      {
        lock (MyGridConveyorSystem.Pathfinding)
        {
          MyGridConveyorSystem.SetTraversalPlayerId(playerId);
          MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId(itemId);
          MyGridConveyorSystem.PrepareTraversal(start.ConveyorEndpoint, vertexTraversable: MyGridConveyorSystem.IsAccessAllowedPredicate, edgeTraversable: (MyGridConveyorSystem.NeedsLargeTube(itemId) ? MyGridConveyorSystem.IsConveyorLargePredicate : (Predicate<IMyPathEdge<IMyConveyorEndpoint>>) null));
          foreach (IMyConveyorEndpoint conveyorEndpoint in MyGridConveyorSystem.Pathfinding)
          {
            MyCubeBlock thisEntity = conveyorEndpoint.CubeBlock == null || !conveyorEndpoint.CubeBlock.HasInventory ? (MyCubeBlock) null : conveyorEndpoint.CubeBlock;
            if (thisEntity != null)
            {
              for (int index = 0; index < thisEntity.InventoryCount; ++index)
              {
                MyInventory inventory = MyEntityExtensions.GetInventory(thisEntity, index);
                if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != (MyInventoryFlags) 0 && inventory != destinationInventory)
                  myFixedPoint += inventory.GetItemAmount(itemId, MyItemFlags.None, false);
              }
            }
          }
        }
      }
      return myFixedPoint;
    }

    public bool PushGenerateItem(
      MyDefinitionId itemDefId,
      MyFixedPoint? amount,
      IMyConveyorEndpointBlock start,
      bool partialPush = true,
      bool calcImmediately = false)
    {
      bool flag = false;
      List<Tuple<MyInventory, MyFixedPoint>> tupleList = new List<Tuple<MyInventory, MyFixedPoint>>();
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = this.GetConveyorEndpointMapping(start);
      if (conveyorEndpointMapping.pushElements != null)
      {
        MyFixedPoint b = (MyFixedPoint) 0;
        if (amount.HasValue)
          b = amount.Value;
        for (int index1 = 0; index1 < conveyorEndpointMapping.pushElements.Count; ++index1)
        {
          if (conveyorEndpointMapping.pushElements[index1] is MyCubeBlock pushElement)
          {
            int inventoryCount = pushElement.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(pushElement, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanReceive) != (MyInventoryFlags) 0)
              {
                MyFixedPoint myFixedPoint = MyFixedPoint.Min(inventory.ComputeAmountThatFits(itemDefId, 0.0f, 0.0f), b);
                if (inventory.CheckConstraint(itemDefId) && !(myFixedPoint == (MyFixedPoint) 0) && MyGridConveyorSystem.CanTransfer(start, conveyorEndpointMapping.pushElements[index1], itemDefId, true, calcImmediately))
                {
                  tupleList.Add(new Tuple<MyInventory, MyFixedPoint>(inventory, myFixedPoint));
                  b -= myFixedPoint;
                }
              }
            }
            if (b <= (MyFixedPoint) 0)
            {
              flag = true;
              break;
            }
          }
        }
      }
      else if (!this.m_isRecomputingGraph)
        this.RecomputeConveyorEndpoints();
      if (flag | partialPush)
      {
        MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) itemDefId);
        foreach (Tuple<MyInventory, MyFixedPoint> tuple in tupleList)
          tuple.Item1.AddItems(tuple.Item2, newObject);
      }
      return flag;
    }

    public static void PushAnyRequest(
      IMyConveyorEndpointBlock start,
      MyInventory srcInventory,
      uint maxItemsToPush = 10)
    {
      if (srcInventory.Empty() || maxItemsToPush == 0U)
        return;
      foreach (MyPhysicalInventoryItem toSend in srcInventory.GetItems().ToArray())
      {
        if (MyGridConveyorSystem.ItemPushRequest(start, srcInventory, toSend))
        {
          --maxItemsToPush;
          if (maxItemsToPush == 0U)
            break;
        }
      }
    }

    private static bool ItemPushRequest(
      IMyConveyorEndpointBlock start,
      MyInventory srcInventory,
      MyPhysicalInventoryItem toSend)
    {
      if (!(start is MyCubeBlock myCubeBlock))
        return false;
      bool flag = false;
      MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
      if (conveyorEndpointMapping.pushElements != null)
      {
        MyDefinitionId id = toSend.Content.GetId();
        for (int index1 = 0; index1 < conveyorEndpointMapping.pushElements.Count; ++index1)
        {
          if (conveyorEndpointMapping.pushElements[index1] is MyCubeBlock pushElement)
          {
            int inventoryCount = pushElement.InventoryCount;
            for (int index2 = 0; index2 < inventoryCount; ++index2)
            {
              MyInventory inventory = MyEntityExtensions.GetInventory(pushElement, index2);
              if ((inventory.GetFlags() & MyInventoryFlags.CanReceive) != (MyInventoryFlags) 0 && inventory != srcInventory)
              {
                MyFixedPoint amountThatFits = inventory.ComputeAmountThatFits(id, 0.0f, 0.0f);
                if (inventory.CheckConstraint(id) && !(amountThatFits == (MyFixedPoint) 0) && MyGridConveyorSystem.CanTransfer(start, conveyorEndpointMapping.pushElements[index1], toSend.GetDefinitionId(), true))
                {
                  MyInventory.Transfer(srcInventory, inventory, toSend.ItemId, amount: new MyFixedPoint?(amountThatFits));
                  flag = true;
                }
              }
            }
          }
        }
      }
      else if (!conveyorSystem.m_isRecomputingGraph)
        conveyorSystem.RecomputeConveyorEndpoints();
      return flag;
    }

    private void RecomputeConveyorEndpoints()
    {
      this.m_conveyorConnections.Clear();
      if (this.m_isRecomputingGraph)
        this.m_isRecomputationIsAborted = true;
      else
        this.StartRecomputationThread();
    }

    private void StartRecomputationThread()
    {
      this.m_conveyorConnectionsForThread.Clear();
      this.m_isRecomputingGraph = true;
      this.m_isRecomputationIsAborted = false;
      this.m_isRecomputationInterrupted = false;
      this.m_endpointIterator = (IEnumerator<IMyConveyorEndpointBlock>) null;
      Parallel.Start(new Action(this.UpdateConveyorEndpointMapping), new Action(this.OnConveyorEndpointMappingUpdateCompleted));
    }

    public static void RecomputeMappingForBlock(IMyConveyorEndpointBlock processedBlock)
    {
      if (!(processedBlock is MyCubeBlock myCubeBlock) || myCubeBlock.CubeGrid == null || (myCubeBlock.CubeGrid.GridSystems == null || myCubeBlock.CubeGrid.GridSystems.ConveyorSystem == null))
        return;
      MyGridConveyorSystem.ConveyorEndpointMapping mappingForBlock = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.ComputeMappingForBlock(processedBlock);
      if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections.ContainsKey(processedBlock))
        myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections[processedBlock] = mappingForBlock;
      else
        myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections.Add(processedBlock, mappingForBlock);
      if (!myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_isRecomputingGraph)
        return;
      if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread.ContainsKey(processedBlock))
        myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread[processedBlock] = mappingForBlock;
      else
        myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread.Add(processedBlock, mappingForBlock);
    }

    private MyGridConveyorSystem.ConveyorEndpointMapping ComputeMappingForBlock(
      IMyConveyorEndpointBlock processedBlock)
    {
      MyGridConveyorSystem.ConveyorEndpointMapping conveyorEndpointMapping = new MyGridConveyorSystem.ConveyorEndpointMapping();
      PullInformation pullInformation = processedBlock.GetPullInformation();
      if (pullInformation != null)
      {
        conveyorEndpointMapping.pullElements = new List<IMyConveyorEndpointBlock>();
        lock (MyGridConveyorSystem.Pathfinding)
        {
          MyGridConveyorSystem.SetTraversalPlayerId(pullInformation.OwnerID);
          if (pullInformation.ItemDefinition != new MyDefinitionId())
          {
            MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId(pullInformation.ItemDefinition);
            using (new MyConveyorLine.InvertedConductivity())
            {
              MyGridConveyorSystem.PrepareTraversal(processedBlock.ConveyorEndpoint, vertexTraversable: MyGridConveyorSystem.IsAccessAllowedPredicate, edgeTraversable: (MyGridConveyorSystem.NeedsLargeTube(pullInformation.ItemDefinition) ? MyGridConveyorSystem.IsConveyorLargePredicate : (Predicate<IMyPathEdge<IMyConveyorEndpoint>>) null));
              MyGridConveyorSystem.AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pullElements, MyInventoryFlags.CanSend);
            }
          }
          else if (pullInformation.Constraint != null)
          {
            MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId();
            using (new MyConveyorLine.InvertedConductivity())
            {
              MyGridConveyorSystem.PrepareTraversal(processedBlock.ConveyorEndpoint, vertexTraversable: MyGridConveyorSystem.IsAccessAllowedPredicate);
              MyGridConveyorSystem.AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pullElements, MyInventoryFlags.CanSend);
            }
          }
        }
      }
      PullInformation pushInformation = processedBlock.GetPushInformation();
      if (pushInformation != null)
      {
        conveyorEndpointMapping.pushElements = new List<IMyConveyorEndpointBlock>();
        lock (MyGridConveyorSystem.Pathfinding)
        {
          MyGridConveyorSystem.SetTraversalPlayerId(pushInformation.OwnerID);
          HashSet<MyDefinitionId> definitions = new HashSet<MyDefinitionId>();
          if (pushInformation.ItemDefinition != new MyDefinitionId())
            definitions.Add(pushInformation.ItemDefinition);
          if (pushInformation.Constraint != null)
          {
            foreach (MyDefinitionId constrainedId in pushInformation.Constraint.ConstrainedIds)
              definitions.Add(constrainedId);
            foreach (MyObjectBuilderType constrainedType in pushInformation.Constraint.ConstrainedTypes)
              MyDefinitionManager.Static.TryGetDefinitionsByTypeId(constrainedType, definitions);
          }
          if (definitions.Count == 0 && (pushInformation.Constraint == null || pushInformation.Constraint.Description == "Empty constraint"))
          {
            MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId();
            MyGridConveyorSystem.PrepareTraversal(processedBlock.ConveyorEndpoint, vertexTraversable: MyGridConveyorSystem.IsAccessAllowedPredicate);
            MyGridConveyorSystem.AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive);
          }
          else
          {
            foreach (MyDefinitionId itemDefinitionId in definitions)
            {
              MyGridConveyorSystem.SetTraversalInventoryItemDefinitionId(itemDefinitionId);
              if (MyGridConveyorSystem.NeedsLargeTube(itemDefinitionId))
                MyGridConveyorSystem.PrepareTraversal(processedBlock.ConveyorEndpoint, vertexTraversable: MyGridConveyorSystem.IsAccessAllowedPredicate, edgeTraversable: MyGridConveyorSystem.IsConveyorLargePredicate);
              else
                MyGridConveyorSystem.PrepareTraversal(processedBlock.ConveyorEndpoint, vertexTraversable: MyGridConveyorSystem.IsAccessAllowedPredicate);
              MyGridConveyorSystem.AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive, new MyDefinitionId?(itemDefinitionId));
            }
          }
        }
      }
      return conveyorEndpointMapping;
    }

    private static void AddReachableEndpoints(
      IMyConveyorEndpointBlock processedBlock,
      List<IMyConveyorEndpointBlock> resultList,
      MyInventoryFlags flagToCheck,
      MyDefinitionId? definitionId = null)
    {
      foreach (IMyConveyorEndpoint conveyorEndpoint in MyGridConveyorSystem.Pathfinding)
      {
        if ((conveyorEndpoint.CubeBlock != processedBlock || processedBlock.AllowSelfPulling()) && (conveyorEndpoint.CubeBlock != null && conveyorEndpoint.CubeBlock.HasInventory) && conveyorEndpoint.CubeBlock is IMyConveyorEndpointBlock cubeBlock)
        {
          MyCubeBlock cubeBlock = conveyorEndpoint.CubeBlock;
          bool flag = false;
          for (int index = 0; index < cubeBlock.InventoryCount; ++index)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(cubeBlock, index);
            if ((inventory.GetFlags() & flagToCheck) != (MyInventoryFlags) 0 && (!definitionId.HasValue || inventory.CheckConstraint(definitionId.Value)))
            {
              flag = true;
              break;
            }
          }
          if (flag && !resultList.Contains(cubeBlock))
            resultList.Add(cubeBlock);
        }
      }
    }

    private void UpdateConveyorEndpointMapping()
    {
      using (this.m_iteratorLock.AcquireExclusiveUsing())
      {
        long timestamp = Stopwatch.GetTimestamp();
        this.m_isRecomputationInterrupted = false;
        if (this.m_endpointIterator == null)
        {
          this.m_endpointIterator = (IEnumerator<IMyConveyorEndpointBlock>) this.m_conveyorEndpointBlocks.GetEnumerator();
          this.m_endpointIterator.MoveNext();
        }
        for (IMyConveyorEndpointBlock current = this.m_endpointIterator.Current; current != null; current = this.m_endpointIterator.Current)
        {
          if (this.m_isRecomputationIsAborted)
          {
            this.m_isRecomputationInterrupted = true;
            break;
          }
          if (new TimeSpan(Stopwatch.GetTimestamp() - timestamp).TotalMilliseconds > 10.0)
          {
            this.m_isRecomputationInterrupted = true;
            break;
          }
          MyGridConveyorSystem.ConveyorEndpointMapping mappingForBlock = this.ComputeMappingForBlock(current);
          this.m_conveyorConnectionsForThread.Add(current, mappingForBlock);
          if (this.m_endpointIterator != null)
          {
            this.m_endpointIterator.MoveNext();
          }
          else
          {
            this.m_isRecomputationIsAborted = true;
            this.m_isRecomputationInterrupted = true;
            break;
          }
        }
      }
    }

    private void OnConveyorEndpointMappingUpdateCompleted()
    {
      using (this.m_iteratorLock.AcquireExclusiveUsing())
      {
        if (this.m_isRecomputationIsAborted)
        {
          this.StartRecomputationThread();
        }
        else
        {
          foreach (KeyValuePair<IMyConveyorEndpointBlock, MyGridConveyorSystem.ConveyorEndpointMapping> keyValuePair in this.m_conveyorConnectionsForThread)
          {
            if (this.m_conveyorConnections.ContainsKey(keyValuePair.Key))
              this.m_conveyorConnections[keyValuePair.Key] = keyValuePair.Value;
            else
              this.m_conveyorConnections.Add(keyValuePair.Key, keyValuePair.Value);
          }
          this.m_conveyorConnectionsForThread.Clear();
          if (this.m_isRecomputationInterrupted)
          {
            Parallel.Start(new Action(this.UpdateConveyorEndpointMapping), new Action(this.OnConveyorEndpointMappingUpdateCompleted));
          }
          else
          {
            this.m_endpointIterator = (IEnumerator<IMyConveyorEndpointBlock>) null;
            this.m_isRecomputingGraph = false;
            foreach (MyConveyorLine line in this.m_lines)
            {
              int num = (int) line.UpdateIsWorking();
            }
          }
        }
      }
    }

    private MyGridConveyorSystem.ConveyorEndpointMapping GetConveyorEndpointMapping(
      IMyConveyorEndpointBlock block)
    {
      return this.m_conveyorConnections.ContainsKey(block) ? this.m_conveyorConnections[block] : new MyGridConveyorSystem.ConveyorEndpointMapping();
    }

    public static void FindReachable(
      IMyConveyorEndpoint from,
      List<IMyConveyorEndpoint> reachableVertices,
      Predicate<IMyConveyorEndpoint> vertexFilter = null,
      Predicate<IMyConveyorEndpoint> vertexTraversable = null,
      Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null)
    {
      lock (MyGridConveyorSystem.Pathfinding)
        MyGridConveyorSystem.Pathfinding.FindReachable(from, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
    }

    public static bool Reachable(IMyConveyorEndpoint from, IMyConveyorEndpoint to)
    {
      lock (MyGridConveyorSystem.Pathfinding)
        return MyGridConveyorSystem.Pathfinding.Reachable(from, to);
    }

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceAfterSimulation;

    private class PullRequestItemSet
    {
      private bool m_all;
      private MyObjectBuilderType? m_obType;
      private MyInventoryConstraint m_constraint;

      public void Clear()
      {
        this.m_all = false;
        this.m_obType = new MyObjectBuilderType?();
        this.m_constraint = (MyInventoryConstraint) null;
      }

      public void Set(bool all)
      {
        this.Clear();
        this.m_all = all;
      }

      public void Set(MyObjectBuilderType? itemTypeId)
      {
        this.Clear();
        this.m_obType = itemTypeId;
      }

      public void Set(MyInventoryConstraint inventoryConstraint)
      {
        this.Clear();
        this.m_constraint = inventoryConstraint;
      }

      public bool Contains(MyDefinitionId itemId) => this.m_all || this.m_obType.HasValue && this.m_obType.Value == itemId.TypeId || this.m_constraint != null && this.m_constraint.Check(itemId);
    }

    private class TransferData : WorkData
    {
      public IMyConveyorEndpointBlock m_start;
      public IMyConveyorEndpointBlock m_endPoint;
      public MyDefinitionId m_itemId;
      public bool m_isPush;
      public bool m_canTransfer;

      public TransferData(
        IMyConveyorEndpointBlock start,
        IMyConveyorEndpointBlock endPoint,
        MyDefinitionId itemId,
        bool isPush)
      {
        this.m_start = start;
        this.m_endPoint = endPoint;
        this.m_itemId = itemId;
        this.m_isPush = isPush;
      }

      public void ComputeTransfer()
      {
        IMyConveyorEndpointBlock start = this.m_start;
        IMyConveyorEndpointBlock endPoint = this.m_endPoint;
        if (!this.m_isPush)
          MyUtils.Swap<IMyConveyorEndpointBlock>(ref start, ref endPoint);
        this.m_canTransfer = MyGridConveyorSystem.ComputeCanTransfer(start, endPoint, new MyDefinitionId?(this.m_itemId));
      }

      public void StoreTransferState() => (this.m_start as MyCubeBlock).CubeGrid.GridSystems.ConveyorSystem.GetConveyorEndpointMapping(this.m_start).AddTransfer(this.m_endPoint, this.m_itemId, this.m_isPush, this.m_canTransfer);
    }

    private class ConveyorEndpointMapping
    {
      public List<IMyConveyorEndpointBlock> pullElements;
      public List<IMyConveyorEndpointBlock> pushElements;
      private readonly Dictionary<(IMyConveyorEndpointBlock, MyDefinitionId, bool), bool> testedTransfers = new Dictionary<(IMyConveyorEndpointBlock, MyDefinitionId, bool), bool>();

      public void AddTransfer(
        IMyConveyorEndpointBlock block,
        MyDefinitionId itemId,
        bool isPush,
        bool canTransfer)
      {
        this.testedTransfers[(block, itemId, isPush)] = canTransfer;
      }

      public bool TryGetTransfer(
        IMyConveyorEndpointBlock block,
        MyDefinitionId itemId,
        bool isPush,
        out bool canTransfer)
      {
        return this.testedTransfers.TryGetValue((block, itemId, isPush), out canTransfer);
      }
    }
  }
}
