// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyCharacterReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.Replication
{
  internal class MyCharacterReplicable : MyEntityReplicableBaseEvent<MyCharacter>
  {
    private MyPropertySyncStateGroup m_propertySync;
    private readonly HashSet<IMyReplicable> m_dependencies = new HashSet<IMyReplicable>();
    private readonly HashSet<VRage.Game.ModAPI.Ingame.IMyEntity> m_dependencyParents = new HashSet<VRage.Game.ModAPI.Ingame.IMyEntity>();
    private long m_ownerId;
    private long m_characterId;

    public HashSetReader<VRage.Game.ModAPI.Ingame.IMyEntity> CachedParentDependencies => new HashSetReader<VRage.Game.ModAPI.Ingame.IMyEntity>(this.m_dependencyParents);

    protected override IMyStateGroup CreatePhysicsGroup() => (IMyStateGroup) new MyCharacterPhysicsStateGroup((MyEntity) this.Instance, (IMyReplicable) this);

    protected override void OnHook()
    {
      base.OnHook();
      if (this.Instance.Closed)
      {
        MyLog.Default.Error("Character is closed upon hooking in client.");
      }
      else
      {
        if (this.Instance == null)
          return;
        this.m_propertySync = new MyPropertySyncStateGroup((IMyReplicable) this, this.Instance.SyncType)
        {
          GlobalValidate = (Func<MyEventContext, ValidationResult>) (context => this.HasRights(context.ClientState.EndpointId.Id, ValidationType.Controlled))
        };
        this.Instance.Hierarchy.OnParentChanged += new Action<MyHierarchyComponentBase, MyHierarchyComponentBase>(this.OnParentChanged);
      }
    }

    private void OnParentChanged(
      MyHierarchyComponentBase oldParent,
      MyHierarchyComponentBase newParent)
    {
      if (!this.IsReadyForReplication)
        return;
      (MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy((IMyReplicable) this);
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
      base.GetStateGroups(resultList);
      if (this.m_propertySync == null || this.m_propertySync.PropertyCount <= 0)
        return;
      resultList.Add((IMyStateGroup) this.m_propertySync);
    }

    public override bool HasToBeChild => this.Instance.Parent != null;

    public override IMyReplicable GetParent()
    {
      if (this.Instance == null)
        return (IMyReplicable) null;
      return this.Instance.Parent != null ? (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Instance.GetTopMostParent((Type) null)) : (IMyReplicable) null;
    }

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      if (this.Instance == null)
        return false;
      stream.WriteBool(this.Instance.IsUsing is MyShipController);
      if (this.Instance.IsUsing is MyShipController)
      {
        long entityId1 = this.Instance.IsUsing.EntityId;
        MySerializer.Write<long>(stream, ref entityId1);
        long entityId2 = this.Instance.EntityId;
        MySerializer.Write<long>(stream, ref entityId2);
      }
      else
      {
        MyObjectBuilder_Character objectBuilder;
        using (MyReplicationLayer.StartSerializingReplicable((IMyReplicable) this, clientEndpoint))
          objectBuilder = (MyObjectBuilder_Character) this.Instance.GetObjectBuilder(false);
        MySerializer.Write<MyObjectBuilder_Character>(stream, ref objectBuilder, MyObjectBuilderSerializer.Dynamic);
      }
      return true;
    }

    protected override void OnLoad(BitStream stream, Action<MyCharacter> loadingDoneHandler)
    {
      bool flag = true;
      if (stream != null)
        MySerializer.CreateAndRead<bool>(stream, out flag);
      if (flag)
      {
        if (stream != null)
        {
          MySerializer.CreateAndRead<long>(stream, out this.m_ownerId);
          MySerializer.CreateAndRead<long>(stream, out this.m_characterId);
        }
        MyEntities.CallAsync((Action) (() => MyCharacterReplicable.LoadAsync(this.m_ownerId, this.m_characterId, loadingDoneHandler)));
      }
      else
      {
        MyObjectBuilder_Character andRead = (MyObjectBuilder_Character) MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
        this.TryRemoveExistingEntity(andRead.EntityId);
        MyCharacter character = MyEntities.CreateFromObjectBuilderNoinit((MyObjectBuilder_EntityBase) andRead) as MyCharacter;
        MyEntities.InitAsync((MyEntity) character, (MyObjectBuilder_EntityBase) andRead, true, (Action<MyEntity>) (e => loadingDoneHandler(character)));
      }
    }

    private static void LoadAsync(
      long ownerId,
      long characterId,
      Action<MyCharacter> loadingDoneHandler)
    {
      MyEntity entity1;
      MyEntities.TryGetEntityById(ownerId, out entity1);
      if (entity1 is MyShipController myShipController)
      {
        if (myShipController.Pilot != null)
        {
          loadingDoneHandler(myShipController.Pilot);
          MySession.Static.Players.UpdatePlayerControllers(ownerId);
        }
        else
        {
          MyEntity entity2;
          MyEntities.TryGetEntityById(characterId, out entity2);
          MyCharacter myCharacter = entity2 as MyCharacter;
          loadingDoneHandler(myCharacter);
        }
      }
      else
        loadingDoneHandler((MyCharacter) null);
    }

    public override HashSet<IMyReplicable> GetDependencies(bool forPlayer)
    {
      if (!forPlayer)
        return (HashSet<IMyReplicable>) null;
      this.m_dependencies.Clear();
      this.m_dependencyParents.Clear();
      if (!Sync.IsServer)
        return this.m_dependencies;
      foreach (MyDataBroadcaster relayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters((MyEntity) this.Instance, this.Instance.GetPlayerIdentityId(), false))
      {
        if (this.Instance.RadioBroadcaster != relayedBroadcaster && !relayedBroadcaster.Closed && MyExternalReplicable.FindByObject((object) relayedBroadcaster) is MyFarBroadcasterReplicable byObject)
        {
          this.m_dependencies.Add((IMyReplicable) byObject);
          if (byObject.Instance != null && byObject.Instance.Entity != null)
          {
            VRage.ModAPI.IMyEntity topMostParent = byObject.Instance.Entity.GetTopMostParent();
            if (topMostParent != null)
              this.m_dependencyParents.Add((VRage.Game.ModAPI.Ingame.IMyEntity) topMostParent);
          }
        }
      }
      return this.m_dependencies;
    }

    public override ValidationResult HasRights(
      EndpointId endpointId,
      ValidationType validationFlags)
    {
      bool flag = true;
      if (validationFlags.HasFlag((Enum) ValidationType.Controlled))
        flag &= (long) endpointId.Value == (long) this.Instance.GetClientIdentity().SteamId;
      return !flag ? ValidationResult.Kick | ValidationResult.Controlled : ValidationResult.Passed;
    }

    public override bool ShouldReplicate(MyClientInfo client) => !this.Instance.IsDead;
  }
}
