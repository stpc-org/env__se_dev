// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyUpdateTriggerComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_UpdateTrigger), true)]
  public class MyUpdateTriggerComponent : MyTriggerComponent
  {
    private int m_size = 100;
    private Dictionary<MyEntity, MyEntityUpdateEnum> m_needsUpdate = new Dictionary<MyEntity, MyEntityUpdateEnum>();
    private bool m_isPirateStation;
    private MyObjectBuilder_CubeGrid m_serializedPirateStation;

    public int Size
    {
      get => this.m_size;
      set
      {
        this.m_size = value;
        if (this.Entity == null)
          return;
        this.m_AABB.Inflate((double) (value / 2));
      }
    }

    public MyUpdateTriggerComponent()
    {
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_UpdateTrigger builderUpdateTrigger = (MyObjectBuilder_UpdateTrigger) base.Serialize(copy);
      builderUpdateTrigger.Size = this.m_size;
      builderUpdateTrigger.IsPirateStation = this.m_isPirateStation;
      builderUpdateTrigger.SerializedPirateStation = this.m_serializedPirateStation;
      return (MyObjectBuilder_ComponentBase) builderUpdateTrigger;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      MyObjectBuilder_UpdateTrigger builderUpdateTrigger = (MyObjectBuilder_UpdateTrigger) builder;
      this.m_size = builderUpdateTrigger.Size;
      this.m_isPirateStation = builderUpdateTrigger.IsPirateStation;
      this.m_serializedPirateStation = builderUpdateTrigger.SerializedPirateStation;
    }

    private void grid_OnBlockOwnershipChanged(MyCubeGrid obj)
    {
      bool flag = false;
      foreach (long bigOwner in obj.BigOwners)
      {
        MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(bigOwner);
        if (playerFaction != null && !playerFaction.IsEveryoneNpc())
        {
          flag = true;
          break;
        }
      }
      foreach (long smallOwner in obj.SmallOwners)
      {
        MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(smallOwner);
        if (playerFaction != null && !playerFaction.IsEveryoneNpc())
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      obj.Components.Remove<MyUpdateTriggerComponent>();
      obj.OnBlockOwnershipChanged -= new Action<MyCubeGrid>(this.grid_OnBlockOwnershipChanged);
    }

    public MyUpdateTriggerComponent(int triggerSize) => this.m_size = triggerSize;

    protected override void UpdateInternal()
    {
      if (this.Entity.Physics == null && !(this.Entity is MyProxyAntenna))
        return;
      this.m_AABB = this.Entity.PositionComp.WorldAABB.Inflate((double) (this.m_size / 2));
      bool flag1 = (uint) this.m_needsUpdate.Count > 0U;
      for (int index = this.QueryResult.Count - 1; index >= 0; --index)
      {
        MyEntity myEntity = this.QueryResult[index];
        if (myEntity.Closed || (!myEntity.PositionComp.WorldAABB.Intersects(this.m_AABB) || myEntity is MyMeteor))
          this.QueryResult.RemoveAtFast<MyEntity>(index);
        else
          break;
      }
      this.DoQuery = this.QueryResult.Count == 0;
      base.UpdateInternal();
      if (!this.m_isPirateStation && this.Entity is MyCubeGrid entity && entity.DisplayName.Contains("Pirate Base"))
      {
        bool flag2 = false;
        MyPlayerCollection players = MySession.Static.Players;
        foreach (long smallOwner in entity.SmallOwners)
        {
          if (smallOwner != 0L && !players.IdentityIsNpc(smallOwner))
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
          this.m_isPirateStation = true;
      }
      if (this.QueryResult.Count == 0)
      {
        if (flag1)
          return;
        if (this.m_isPirateStation)
          this.DespawnPirateStation();
        else
          this.DisableRecursively((MyEntity) this.Entity);
      }
      else if (this.m_isPirateStation)
      {
        this.RespawnPirateStation();
      }
      else
      {
        if (!flag1)
          return;
        this.EnableRecursively((MyEntity) this.Entity);
        this.m_needsUpdate.Clear();
      }
    }

    private void RespawnPirateStation()
    {
      if (!Sync.IsServer || this.Entity is MyCubeGrid)
        return;
      if (this.m_serializedPirateStation != null)
        Sandbox.Game.Entities.MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) this.m_serializedPirateStation, true, fadeIn: true);
      this.Entity.Close();
    }

    private void DespawnPirateStation()
    {
      if (!Sync.IsServer)
        return;
      if (this.Entity is MyCubeGrid entity2)
      {
        HashSet<MyDataBroadcaster> output = new HashSet<MyDataBroadcaster>();
        MyAntennaSystem.Static.GetEntityBroadcasters((MyEntity) entity2, ref output);
        IEnumerable<MyRadioBroadcaster> source = output.OfType<MyRadioBroadcaster>();
        if (!source.Any<MyRadioBroadcaster>())
          return;
        MyRadioBroadcaster radioBroadcaster = source.MaxBy<MyRadioBroadcaster>((Func<MyRadioBroadcaster, float>) (x => x.BroadcastRadius));
        MyObjectBuilder_ProxyAntenna newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProxyAntenna>();
        MyObjectBuilder_ProxyAntenna ob = newObject;
        radioBroadcaster.InitProxyObjectBuilder(ob);
        MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid) entity2.GetObjectBuilder(false);
        MyObjectBuilder_ComponentContainer componentContainer = entity2.Components.Serialize();
        componentContainer.Components.RemoveAll((Predicate<MyObjectBuilder_ComponentContainer.ComponentData>) (x => !(x.Component is MyObjectBuilder_UpdateTrigger)));
        ((MyObjectBuilder_UpdateTrigger) componentContainer.Components[0].Component).SerializedPirateStation = objectBuilder;
        newObject.ComponentContainer = componentContainer;
        MySandboxGame.Static.Invoke("SpawnProxyPirate", (object) newObject, (Action<object>) (b => Sandbox.Game.Entities.MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) b, false)));
        entity2.Close();
      }
      else
      {
        MyProxyAntenna entity1 = this.Entity as MyProxyAntenna;
      }
    }

    protected override bool QueryEvaluator(MyEntity entity) => entity.Physics != null && !entity.Physics.IsStatic && (!(entity is MyFloatingObject) && !(entity is MyDebrisBase)) && entity != this.Entity.GetTopMostParent();

    private void DisableRecursively(MyEntity entity)
    {
      this.Enabled = false;
      this.m_needsUpdate[entity] = entity.NeedsUpdate;
      entity.NeedsUpdate = MyEntityUpdateEnum.NONE;
      entity.Render.Visible = false;
      if (entity.Hierarchy == null)
        return;
      foreach (MyEntityComponentBase child in entity.Hierarchy.Children)
        this.DisableRecursively((MyEntity) child.Entity);
    }

    private void EnableRecursively(MyEntity entity)
    {
      this.Enabled = true;
      if (this.m_needsUpdate.ContainsKey(entity))
        entity.NeedsUpdate = this.m_needsUpdate[entity];
      entity.Render.Visible = true;
      if (entity.Hierarchy == null)
        return;
      foreach (MyEntityComponentBase child in entity.Hierarchy.Children)
        this.EnableRecursively((MyEntity) child.Entity);
    }

    public override void Dispose()
    {
      base.Dispose();
      if (this.Entity != null && !this.Entity.MarkedForClose && this.QueryResult.Count != 0)
      {
        this.EnableRecursively((MyEntity) this.Entity);
        this.m_needsUpdate.Clear();
      }
      this.m_needsUpdate.Clear();
    }

    public override string ComponentTypeDebugString => "Pirate update trigger";

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (!(this.Entity is MyCubeGrid entity))
        return;
      entity.OnBlockOwnershipChanged += new Action<MyCubeGrid>(this.grid_OnBlockOwnershipChanged);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (!(this.Entity is MyCubeGrid entity))
        return;
      entity.OnBlockOwnershipChanged -= new Action<MyCubeGrid>(this.grid_OnBlockOwnershipChanged);
    }

    private class Sandbox_Game_Components_MyUpdateTriggerComponent\u003C\u003EActor : IActivator, IActivator<MyUpdateTriggerComponent>
    {
      object IActivator.CreateInstance() => (object) new MyUpdateTriggerComponent();

      MyUpdateTriggerComponent IActivator<MyUpdateTriggerComponent>.CreateInstance() => new MyUpdateTriggerComponent();
    }
  }
}
