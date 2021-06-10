// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractFind
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractFind))]
  public class MyContractFind : MyContract
  {
    public static readonly int DISPOSE_TIME_IN_S = 10;
    private bool m_isBeingDisposed;
    private float m_disposeTime;
    private MyTimeSpan? DisposeTime;

    public Vector3D GridPosition { get; private set; }

    public Vector3D GpsPosition { get; private set; }

    public long GridId { get; private set; }

    public double GpsDistance { get; private set; }

    public double TriggerRadius { get; private set; }

    public bool GridFound { get; private set; }

    public bool KeepGridAtTheEnd { get; private set; }

    public float MaxGpsOffset { get; private set; }

    public override MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
      MyObjectBuilder_ContractFind builderContractFind = objectBuilder as MyObjectBuilder_ContractFind;
      builderContractFind.GridPosition = (SerializableVector3D) this.GridPosition;
      builderContractFind.GpsPosition = (SerializableVector3D) this.GpsPosition;
      builderContractFind.GridId = this.GridId;
      builderContractFind.GpsDistance = this.GpsDistance;
      builderContractFind.MaxGpsOffset = this.MaxGpsOffset;
      builderContractFind.TriggerRadius = this.TriggerRadius;
      builderContractFind.GridFound = this.GridFound;
      builderContractFind.KeepGridAtTheEnd = this.KeepGridAtTheEnd;
      return objectBuilder;
    }

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      if (!(ob is MyObjectBuilder_ContractFind builderContractFind))
        return;
      this.GridPosition = (Vector3D) builderContractFind.GridPosition;
      this.GpsPosition = (Vector3D) builderContractFind.GpsPosition;
      this.GridId = builderContractFind.GridId;
      this.GpsDistance = builderContractFind.GpsDistance;
      this.MaxGpsOffset = builderContractFind.MaxGpsOffset;
      this.TriggerRadius = builderContractFind.TriggerRadius;
      this.GridFound = builderContractFind.GridFound;
      this.KeepGridAtTheEnd = builderContractFind.KeepGridAtTheEnd;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (this.State != MyContractStateEnum.Active)
        return;
      this.SubscribeToTrigger();
      this.SubscribePowerChange();
    }

    private void SubscribeToTrigger()
    {
      if (this.GridId == 0L)
        return;
      MyAreaTriggerComponent trigger = this.GetTrigger();
      if (trigger != null)
      {
        trigger.EntityEntered += new Action<long, string>(this.EntityEnteredTrigger);
      }
      else
      {
        MyLog.Default.WriteToLogAndAssert(string.Format("CONTRACT SEARCH - Critical fail. Grid {0} is not present in world or is missing the Trigger", (object) this.GridId));
        this.Fail();
      }
    }

    private void UnsubscribeFromTrigger()
    {
      if (this.GridId == 0L)
        return;
      MyAreaTriggerComponent trigger = this.GetTrigger();
      if (trigger == null)
        return;
      trigger.EntityEntered -= new Action<long, string>(this.EntityEnteredTrigger);
    }

    private MyAreaTriggerComponent GetTrigger()
    {
      if (this.GridId == 0L)
        return (MyAreaTriggerComponent) null;
      MyEntity entityById = MyEntities.GetEntityById(this.GridId);
      if (entityById == null)
        return (MyAreaTriggerComponent) null;
      if (!entityById.Components.Contains(typeof (MyTriggerAggregate)))
        return (MyAreaTriggerComponent) null;
      string triggerName = this.GetTriggerName();
      MyAggregateComponentList childList = entityById.Components.Get<MyTriggerAggregate>().ChildList;
      triggerComponent = (MyAreaTriggerComponent) null;
      foreach (MyComponentBase myComponentBase in childList.Reader)
      {
        if (myComponentBase is MyAreaTriggerComponent triggerComponent)
        {
          if (triggerComponent.Name == triggerName)
            break;
        }
        triggerComponent = (MyAreaTriggerComponent) null;
      }
      return triggerComponent ?? (MyAreaTriggerComponent) null;
    }

    protected override void Activate_Internal(MyTimeSpan timeOfActivation)
    {
      base.Activate_Internal(timeOfActivation);
      MyGps gps = new MyGps();
      gps.DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Find_GpsName);
      gps.Name = MyTexts.GetString(MyCommonTexts.Contract_Find_GpsName);
      gps.Description = MyTexts.GetString(MyCommonTexts.Contract_Find_GpsDescription);
      gps.Coords = this.GpsPosition;
      gps.ShowOnHud = true;
      gps.DiscardAt = new TimeSpan?();
      gps.ContractId = this.Id;
      gps.GPSColor = Color.DarkOrange;
      foreach (long owner in this.Owners)
        MySession.Static.Gpss.SendAddGps(owner, ref gps);
      MyContractTypeFindDefinition definition = this.GetDefinition() as MyContractTypeFindDefinition;
      if (this.GridId <= 0L)
      {
        string name = "Container_MK-19";
        if (definition != null && definition.PrefabsSearchableGrids != null && definition.PrefabsSearchableGrids.Count > 0)
          name = definition.PrefabsSearchableGrids[MyRandom.Instance.Next(0, definition.PrefabsSearchableGrids.Count)];
        this.SpawnPrefab(name);
      }
      else if (MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById)
      {
        this.AttachTrigger(entityById);
        if (entityById.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.NoPower)
          entityById.GridSystems.GridPowerStateChanged += new Action<long, bool, string>(this.GridPowerStateChanged);
        else
          this.Fail();
      }
      else
        this.Fail();
    }

    protected void SpawnPrefab(string name)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(this.StartFaction);
      if (factionById == null)
      {
        MyLog.Default.Error("Contract - Find: Starting faction is not in factions!!!\n Cannot spawn prefab.");
      }
      else
      {
        Vector3 v = Vector3.Normalize(MyUtils.GetRandomVector3());
        Vector3 perpendicularVector = Vector3.CalculatePerpendicularVector(v);
        MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties()
        {
          Position = this.GridPosition,
          Forward = v,
          Up = perpendicularVector,
          PrefabName = name,
          OwnerId = factionById.FounderId,
          Color = factionById.CustomColor,
          SpawningOptions = SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix,
          UpdateSync = true
        };
        MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, (Action) (() =>
        {
          if (spawnProperties.ResultList == null || spawnProperties.ResultList.Count == 0 || spawnProperties.ResultList.Count > 1)
            return;
          MyCubeGrid result = spawnProperties.ResultList[0];
          this.GridId = result.EntityId;
          this.AttachTrigger(result);
          result.GridSystems.GridPowerStateChanged += new Action<long, bool, string>(this.GridPowerStateChanged);
        }));
      }
    }

    private string GetTriggerName() => "Contract_Find_Trig_" + (object) this.Id;

    protected void AttachTrigger(MyCubeGrid grid)
    {
      MyAreaTriggerComponent triggerComponent = new MyAreaTriggerComponent(this.GetTriggerName());
      if (!grid.Components.Contains(typeof (MyTriggerAggregate)))
        grid.Components.Add(typeof (MyTriggerAggregate), (MyComponentBase) new MyTriggerAggregate());
      grid.Components.Get<MyTriggerAggregate>().AddComponent((MyComponentBase) triggerComponent);
      triggerComponent.Radius = this.TriggerRadius;
      triggerComponent.Center = grid.PositionComp.GetPosition();
      triggerComponent.EntityEntered += new Action<long, string>(this.EntityEnteredTrigger);
    }

    protected void DetachTrigger()
    {
      if (MySessionComponentTriggerSystem.Static == null)
        return;
      MyTriggerComponent foundTrigger;
      MyEntity triggersEntity = MySessionComponentTriggerSystem.Static.GetTriggersEntity(this.GetTriggerName(), out foundTrigger);
      if (triggersEntity == null || foundTrigger == null)
        return;
      MyTriggerAggregate component;
      if (triggersEntity.Components.TryGet<MyTriggerAggregate>(out component))
        component.RemoveComponent((MyComponentBase) foundTrigger);
      else
        triggersEntity.Components.Remove(typeof (MyAreaTriggerComponent), (MyComponentBase) (foundTrigger as MyAreaTriggerComponent));
    }

    private void GridPowerStateChanged(long entityId, bool isPowered, string entityName)
    {
      if (isPowered)
        return;
      this.Fail();
    }

    public override bool CanBeFinished_Internal() => base.CanBeFinished_Internal() && this.GridFound;

    private void EntityEnteredTrigger(long entityId, string entityName)
    {
      MyEntity entityById = MyEntities.GetEntityById(entityId);
      if (entityById == null)
        return;
      MyEntityController entityController = MySession.Static.Players.GetEntityController(entityById);
      if (entityController == null || entityController.Player == null || entityController.Player.Identity == null)
        return;
      long identityId = entityController.Player.Identity.IdentityId;
      bool flag = false;
      foreach (long owner in this.Owners)
      {
        if (owner == identityId)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      this.GridFound = true;
      int num = (int) this.Finish();
    }

    protected override void FailFor_Internal(long player, bool abandon = false)
    {
      base.FailFor_Internal(player, abandon);
      this.RemoveGpsForPlayer(player);
    }

    protected override void FinishFor_Internal(long player, int rewardeeCount)
    {
      base.FinishFor_Internal(player, rewardeeCount);
      this.RemoveGpsForPlayer(player);
    }

    protected override void CleanUp_Internal()
    {
      this.UnsubscribePowerChange();
      this.UnsubscribeFromTrigger();
      if (this.KeepGridAtTheEnd)
        this.DetachTrigger();
      float num = 0.0f;
      MyEntity entityById = MyEntities.GetEntityById(this.GridId);
      if (entityById != null)
      {
        if (this.State == MyContractStateEnum.Finished)
        {
          if (!this.KeepGridAtTheEnd)
          {
            string empty = string.Empty;
            bool offset = false;
            string name;
            if ((double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(entityById.PositionComp.GetPosition()).LengthSquared() > 0.001)
            {
              name = "BlockDestroyed_Large";
              num = 1f;
            }
            else
            {
              name = "Warp";
              num = 10f;
              offset = true;
            }
            this.CreateParticleEffectOnEntity(name, entityById.EntityId, offset);
          }
        }
        else if (!this.KeepGridAtTheEnd)
        {
          this.CreateParticleEffectOnEntity("Explosion_Warhead_30", entityById.EntityId, false);
          num = 2f;
        }
        else
        {
          this.CreateParticleEffectOnEntity("", entityById.EntityId, false);
          num = 0.0f;
        }
      }
      this.m_isBeingDisposed = true;
      this.m_disposeTime = num;
      this.State = MyContractStateEnum.ToBeDisposed;
    }

    private void RemoveTriggerFromEntity() => throw new NotImplementedException();

    public override void Update(MyTimeSpan currentTime)
    {
      base.Update(currentTime);
      if (this.State != MyContractStateEnum.ToBeDisposed)
        return;
      bool flag = false;
      if (this.m_isBeingDisposed)
      {
        if (!this.DisposeTime.HasValue)
          this.DisposeTime = new MyTimeSpan?(currentTime + MyTimeSpan.FromSeconds((double) this.m_disposeTime));
        if (this.DisposeTime.Value <= currentTime)
          flag = true;
      }
      else
        flag = true;
      if (!flag)
        return;
      this.State = MyContractStateEnum.Disposed;
      if (this.KeepGridAtTheEnd || !(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      entityById.DismountAllCockpits();
      entityById.Close();
    }

    private void UnsubscribePowerChange()
    {
      if (!(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      entityById.GridSystems.GridPowerStateChanged -= new Action<long, bool, string>(this.GridPowerStateChanged);
    }

    private void SubscribePowerChange()
    {
      if (!(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      entityById.GridSystems.GridPowerStateChanged += new Action<long, bool, string>(this.GridPowerStateChanged);
    }

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Find"));

    private void RemoveGpsForPlayer(long identityId)
    {
      MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, this.Id);
      if (gpsByContractId == null)
        return;
      MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
    }

    public override string ToDebugString()
    {
      StringBuilder stringBuilder = new StringBuilder(base.ToDebugString());
      stringBuilder.AppendLine(string.Format("Station<->Gps distance: {0}", (object) this.GpsDistance));
      return stringBuilder.ToString();
    }
  }
}
