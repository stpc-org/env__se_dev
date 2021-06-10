// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyCubeGridReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRage;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal class MyCubeGridReplicable : MyEntityReplicableBaseEvent<MyCubeGrid>, IMyStreamableReplicable
  {
    private Action<MyCubeGrid> m_loadingDoneHandler;
    private MyStreamingEntityStateGroup<MyCubeGridReplicable> m_streamingGroup;
    private readonly HashSet<IMyReplicable> m_dependencies = new HashSet<IMyReplicable>();
    private readonly List<MyCubeGrid> m_tmpCubeGrids = new List<MyCubeGrid>();
    private readonly HashSet<IMyReplicable> m_criticalDependencies = new HashSet<IMyReplicable>();
    private MyPropertySyncStateGroup m_propertySync;

    private MyCubeGrid Grid => this.Instance;

    public override int? PCU => new int?(this.Grid.BlocksPCU);

    public override string InstanceName => this.Grid.DisplayName ?? base.InstanceName;

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      if (!this.Grid.IsSplit)
        return false;
      stream.WriteBool(true);
      stream.WriteInt64(this.Grid.EntityId);
      return true;
    }

    protected override void OnLoad(BitStream stream, Action<MyCubeGrid> loadingDoneHandler)
    {
      if (stream.ReadBool())
      {
        long gridId = stream.ReadInt64();
        Action<MyCubeGrid> findGrid = (Action<MyCubeGrid>) null;
        findGrid = (Action<MyCubeGrid>) (grid =>
        {
          if (grid.EntityId != gridId)
            return;
          loadingDoneHandler(grid);
          MyCubeGrid.OnSplitGridCreated -= findGrid;
        });
        MyCubeGrid.OnSplitGridCreated += findGrid;
      }
      else
      {
        MyObjectBuilder_EntityBase andRead = MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
        this.TryRemoveExistingEntity(andRead.EntityId);
        MyCubeGrid grid = MyEntities.CreateFromObjectBuilderNoinit(andRead) as MyCubeGrid;
        bool fadeIn = false;
        if (andRead.PositionAndOrientation.HasValue && ((Vector3D) andRead.PositionAndOrientation.Value.Position - MySector.MainCamera.Position).LengthSquared() > 1000000.0)
          fadeIn = true;
        double serializationTimestamp = stream.ReadDouble();
        MyEntities.InitAsync((MyEntity) grid, andRead, true, (Action<MyEntity>) (e => loadingDoneHandler(grid)), serializationTimestamp, fadeIn);
      }
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
      if (this.m_streamingGroup != null)
        resultList.Add((IMyStateGroup) this.m_streamingGroup);
      base.GetStateGroups(resultList);
      resultList.Add((IMyStateGroup) this.m_propertySync);
    }

    protected override void OnHook()
    {
      base.OnHook();
      this.m_propertySync = new MyPropertySyncStateGroup((IMyReplicable) this, this.Grid.SyncType)
      {
        GlobalValidate = (Func<MyEventContext, ValidationResult>) (context => this.HasRights(context.ClientState.EndpointId.Id, ValidationType.Access | ValidationType.Controlled))
      };
    }

    public void OnLoadBegin(Action<bool> loadingDoneHandler) => this.m_loadingDoneHandler = (Action<MyCubeGrid>) (instance => this.OnLoadDone(instance, loadingDoneHandler));

    public void CreateStreamingStateGroup() => this.m_streamingGroup = new MyStreamingEntityStateGroup<MyCubeGridReplicable>(this, (IMyReplicable) this);

    public IMyStreamingStateGroup GetStreamingStateGroup() => (IMyStreamingStateGroup) this.m_streamingGroup;

    public void Serialize(
      BitStream stream,
      HashSet<string> cachedData,
      Endpoint forClient,
      Action writeData)
    {
      if (this.Grid.Closed)
        return;
      stream.WriteBool(false);
      MyObjectBuilder_EntityBase builder;
      using (MyReplicationLayer.StartSerializingReplicable((IMyReplicable) this, forClient))
        builder = this.Grid.GetObjectBuilder(false);
      double time = MyMultiplayer.GetReplicationServer().GetClientRelevantServerTimestamp(forClient).Milliseconds;
      Parallel.Start((Action) (() =>
      {
        try
        {
          MySerializer.Write<MyObjectBuilder_EntityBase>(stream, ref builder, MyObjectBuilderSerializer.Dynamic);
        }
        catch (Exception ex)
        {
          XmlSerializer serializer = MyXmlSerializerManager.GetSerializer(builder.GetType());
          MyLog.Default.WriteLine("Grid data - START");
          try
          {
            serializer.Serialize(MyLog.Default.GetTextWriter(), (object) builder);
          }
          catch
          {
            MyLog.Default.WriteLine("Failed");
          }
          MyLog.Default.WriteLine("Grid data - END");
          throw;
        }
        stream.WriteDouble(time);
        writeData();
      }));
    }

    public void LoadDone(BitStream stream) => this.OnLoad(stream, this.m_loadingDoneHandler);

    public bool NeedsToBeStreamed => Sync.IsServer ? !this.Grid.IsSplit : this.m_streamingGroup != null;

    public void LoadCancel() => this.m_loadingDoneHandler((MyCubeGrid) null);

    public override HashSet<IMyReplicable> GetDependencies(bool forPlayer)
    {
      this.m_dependencies.Clear();
      if (!Sync.IsServer || this.Instance == null)
        return this.m_dependencies;
      foreach (MyDataReceiver laserReceiver in this.Instance.GridSystems.RadioSystem.LaserReceivers)
      {
        foreach (MyDataBroadcaster myDataBroadcaster in laserReceiver.BroadcastersInRange)
        {
          if (!myDataBroadcaster.Closed)
          {
            MyExternalReplicable byObject = MyExternalReplicable.FindByObject((object) myDataBroadcaster);
            if (byObject != null)
              this.m_dependencies.Add((IMyReplicable) byObject);
          }
        }
      }
      return this.m_dependencies;
    }

    public override HashSet<IMyReplicable> GetCriticalDependencies()
    {
      this.m_criticalDependencies.Clear();
      if (this.Instance == null)
        return this.m_criticalDependencies;
      MyGridPhysicalHierarchy.Static.GetGroupNodes(this.Instance, this.m_tmpCubeGrids);
      foreach (object tmpCubeGrid in this.m_tmpCubeGrids)
      {
        MyExternalReplicable byObject = MyExternalReplicable.FindByObject(tmpCubeGrid);
        if (byObject != null && byObject != this)
          this.m_criticalDependencies.Add((IMyReplicable) byObject);
      }
      this.m_tmpCubeGrids.Clear();
      return this.m_criticalDependencies;
    }

    public override ValidationResult HasRights(
      EndpointId endpointId,
      ValidationType validationFlags)
    {
      ValidationResult validationResult = ValidationResult.Passed;
      long identityId = MySession.Static.Players.TryGetIdentityId(endpointId.Value, 0);
      if (validationFlags.HasFlag((Enum) ValidationType.Controlled))
      {
        validationResult |= MyReplicableRightsValidator.GetControlled((MyEntity) this.Instance, endpointId);
        if (validationResult.HasFlag((Enum) ValidationResult.Kick))
          return validationResult;
      }
      if ((validationFlags.HasFlag((Enum) ValidationType.Ownership) || validationFlags.HasFlag((Enum) ValidationType.BigOwner)) && !MyReplicableRightsValidator.GetBigOwner(this.Instance, endpointId, identityId, false))
        return ValidationResult.Kick | ValidationResult.Ownership | ValidationResult.BigOwner;
      if (validationFlags.HasFlag((Enum) ValidationType.BigOwnerSpaceMaster) && !MyReplicableRightsValidator.GetBigOwner(this.Instance, endpointId, identityId, true))
        return ValidationResult.Kick | ValidationResult.BigOwnerSpaceMaster;
      if (validationFlags.HasFlag((Enum) ValidationType.Access))
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
        if (identity == null || identity.Character == null || (this.Grid == null || !(MyExternalReplicable.FindByObject((object) identity.Character) is MyCharacterReplicable byObject)))
          return ValidationResult.Kick | ValidationResult.Access;
        Vector3D position = identity.Character.PositionComp.GetPosition();
        MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this.Grid);
        bool access = MyReplicableRightsValidator.GetAccess(byObject, position, this.Grid, group, true);
        if (!access)
        {
          byObject.GetDependencies(true);
          access |= MyReplicableRightsValidator.GetAccess(byObject, position, this.Grid, group, false);
        }
        if (!access)
          return ValidationResult.Access;
      }
      return validationResult;
    }
  }
}
