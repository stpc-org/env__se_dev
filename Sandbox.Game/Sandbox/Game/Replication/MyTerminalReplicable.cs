// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyTerminalReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal class MyTerminalReplicable : MyExternalReplicableEvent<MySyncedBlock>
  {
    private MyPropertySyncStateGroup m_propertySync;
    private long m_blockEntityId;

    private MySyncedBlock Block => this.Instance;

    public override bool IsValid => this.Block != null && !this.Block.MarkedForClose;

    protected override void OnHook()
    {
      base.OnHook();
      this.m_propertySync = new MyPropertySyncStateGroup((IMyReplicable) this, this.Block.SyncType)
      {
        GlobalValidate = (Func<MyEventContext, ValidationResult>) (context => this.HasRights(context.ClientState.EndpointId.Id, ValidationType.Access | ValidationType.Ownership))
      };
      this.Block.OnClose += (Action<MyEntity>) (entity => this.RaiseDestroyed());
      this.Block.SlimBlock.CubeGridChanged += new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
      if (Sync.IsServer)
        this.Block.AddedToScene += new Action<MyEntity>(this.MarkDirty);
      this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Block.CubeGrid);
    }

    private void MarkDirty(MyEntity entity) => this.m_propertySync.MarkDirty();

    private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
    {
      this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Block.CubeGrid);
      (MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy((IMyReplicable) this);
    }

    public override ValidationResult HasRights(
      EndpointId endpointId,
      ValidationType validationFlags)
    {
      if (this.Block == null)
        return ValidationResult.Kick;
      ValidationResult validationResult = ValidationResult.Passed;
      long identityId = MySession.Static.Players.TryGetIdentityId(endpointId.Value, 0);
      AdminSettingsEnum adminSettingsEnum;
      if (validationFlags.HasFlag((Enum) ValidationType.Ownership) && (!MySession.Static.RemoteAdminSettings.TryGetValue(endpointId.Value, out adminSettingsEnum) || !adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.UseTerminals)))
      {
        switch (this.Block.GetUserRelationToOwner(identityId))
        {
          case MyRelationsBetweenPlayerAndBlock.NoOwnership:
          case MyRelationsBetweenPlayerAndBlock.Owner:
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
            break;
          default:
            return ValidationResult.Kick | ValidationResult.Ownership;
        }
      }
      if (validationFlags.HasFlag((Enum) ValidationType.BigOwner) && !MyReplicableRightsValidator.GetBigOwner(this.Block.CubeGrid, endpointId, identityId, false))
        return ValidationResult.Kick | ValidationResult.BigOwner;
      if (validationFlags.HasFlag((Enum) ValidationType.BigOwnerSpaceMaster) && !MyReplicableRightsValidator.GetBigOwner(this.Block.CubeGrid, endpointId, identityId, true))
        return ValidationResult.Kick | ValidationResult.BigOwnerSpaceMaster;
      if (validationFlags.HasFlag((Enum) ValidationType.Controlled))
      {
        validationResult = MyReplicableRightsValidator.GetControlled((MyEntity) this.Block.CubeGrid, endpointId);
        if (validationResult == ValidationResult.Kick)
          return validationResult | ValidationResult.Controlled;
      }
      if (validationFlags.HasFlag((Enum) ValidationType.Access))
      {
        if (this.Block.CubeGrid == null)
          return ValidationResult.Kick | ValidationResult.Access;
        MyCubeGrid cubeGrid = this.Block.CubeGrid;
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
        if (identity == null || identity.Character == null || !(MyExternalReplicable.FindByObject((object) identity.Character) is MyCharacterReplicable byObject))
          return ValidationResult.Kick | ValidationResult.Access;
        Vector3D position = identity.Character.PositionComp.GetPosition();
        MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(cubeGrid);
        bool access = MyReplicableRightsValidator.GetAccess(byObject, position, cubeGrid, group, true);
        if (!access)
        {
          byObject.GetDependencies(true);
          access |= MyReplicableRightsValidator.GetAccess(byObject, position, cubeGrid, group, false);
        }
        if (!access)
          return ValidationResult.Access;
      }
      return validationResult;
    }

    public override IMyReplicable GetParent() => this.m_parent;

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      stream.WriteInt64(this.Block.EntityId);
      return true;
    }

    private MySyncedBlock FindBlock()
    {
      MySyncedBlock entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MySyncedBlock>(this.m_blockEntityId, out entity);
      return entity != null && entity.GetTopMostParent((Type) null).MarkedForClose ? (MySyncedBlock) null : entity;
    }

    protected override void OnLoad(BitStream stream, Action<MySyncedBlock> loadingDoneHandler)
    {
      if (stream != null)
        this.m_blockEntityId = stream.ReadInt64();
      Sandbox.Game.Entities.MyEntities.CallAsync((Action) (() => loadingDoneHandler(this.FindBlock())));
    }

    public override void OnDestroyClient()
    {
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList) => resultList.Add((IMyStateGroup) this.m_propertySync);

    public override bool HasToBeChild => true;

    public override BoundingBoxD GetAABB() => this.Block.PositionComp.WorldAABB;
  }
}
