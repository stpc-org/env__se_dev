// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContract
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
  [StaticEventOwner]
  public abstract class MyContract
  {
    private MyContractStateEnum m_state;
    private int m_unfinishedConditionCount;
    private MyTimeSpan? m_lastTimeUpdate;
    public Action<long> OnContractAcquired;
    public Action OnContractFailed;
    public Action OnContractSucceeded;

    public Action<long, MyContractStateEnum, MyContractStateEnum> ContractChangedState { get; set; }

    public long Id { get; private set; }

    public bool IsPlayerMade { get; private set; }

    public MyContractStateEnum State
    {
      get => this.m_state;
      set
      {
        if (this.m_state == value)
          return;
        MyContractStateEnum state = this.m_state;
        this.m_state = value;
        this.ContractChangedState.InvokeIfNotNull<long, MyContractStateEnum, MyContractStateEnum>(this.Id, state, value);
      }
    }

    public List<long> Owners { get; private set; } = new List<long>();

    public List<long> AllConditions { get; private set; } = new List<long>();

    public MyContractCondition ContractCondition { get; private set; }

    public long RewardMoney { get; private set; }

    public int RewardReputation { get; private set; }

    public long StartingDeposit { get; private set; }

    public int FailReputationPrice { get; private set; }

    public long StartFaction { get; private set; }

    public long StartStation { get; private set; }

    public long StartBlock { get; private set; }

    public MyTimeSpan Creation { get; private set; }

    public MyTimeSpan? RemainingTime { get; private set; }

    public int? TicksToDiscard { get; private set; }

    public bool NeedsUpdate => this.NeedsUpdate_Internal();

    public bool CanBeShared => this.CanBeShared_Internal();

    public bool CanBeFinished => this.CanBeFinished_Internal();

    public bool CanBeFinishedInTerminal => this.CountRemainingConditions() > 0;

    public bool IsTimeLimited => this.RemainingTime.HasValue;

    public MyActivationResults CanActivate(long identityId)
    {
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      return component.GetContractLimitPerPlayer() <= identity.ActiveContracts.Count ? MyActivationResults.Fail_ContractLimitReachedHard : this.CanActivate_Internal(identityId);
    }

    public bool Activate(long playerId, MyTimeSpan timeOfActivation)
    {
      if (this.CanActivate_Internal(playerId) != MyActivationResults.Success)
        return false;
      if (this.m_state != MyContractStateEnum.Inactive)
        MyLog.Default.WriteToLogAndAssert("Contract - Cannot activate other than inactive contract\nCurrent state: " + this.m_state.ToString());
      this.State = MyContractStateEnum.Active;
      this.Owners.Add(playerId);
      MySession.Static.Players.TryGetIdentity(playerId)?.ActiveContracts.Add(this.Id);
      this.Activate_Internal(timeOfActivation);
      if (this.OnContractAcquired != null)
        this.OnContractAcquired(playerId);
      this.LogContract(MyContract.MyContractLogType.ACCEPT);
      if (MyVisualScriptLogicProvider.ContractAccepted != null)
      {
        MyDefinitionId? definitionId = this.GetDefinitionId();
        if (definitionId.HasValue)
          MyVisualScriptLogicProvider.ContractAccepted(this.Id, definitionId.Value, playerId, this.IsPlayerMade, this.StartBlock, this.StartFaction, this.StartStation);
        else
          MyLog.Default.WriteToLogAndAssert(string.Format("Contract definition not found. Something is really wrong. Contract type '{0}', contract id '{1}'.", (object) this.GetType().ToString(), (object) this.Id));
      }
      return true;
    }

    public bool Share(long playerId)
    {
      if (!this.CanBeShared)
        return false;
      foreach (long owner in this.Owners)
      {
        if (owner == playerId)
          return false;
      }
      this.Owners.Add(playerId);
      MySession.Static.Players.TryGetIdentity(playerId)?.ActiveContracts.Add(this.Id);
      this.Share_Internal(playerId);
      return true;
    }

    public MyContractResults Finish()
    {
      if (!this.CanBeFinished)
        return MyContractResults.Fail_FinishConditionsNotMet;
      if (this.m_state != MyContractStateEnum.Active)
        MyLog.Default.WriteToLogAndAssert("Contract - Cannot finish Contract that is not active\nCurrent state: " + this.m_state.ToString());
      this.State = MyContractStateEnum.Finished;
      this.LogContract(MyContract.MyContractLogType.FINISH);
      int rewardeeCount = 0;
      foreach (long owner in this.Owners)
      {
        if (this.CanPlayerReceiveReward(owner))
          ++rewardeeCount;
      }
      if (rewardeeCount == 0)
        MyLog.Default.WriteToLogAndAssert("No one to receive contract reward, is it correct?");
      this.Finish_Internal();
      long acceptingPlayerId = this.Owners.Count >= 1 ? this.Owners[0] : 0L;
      while (this.Owners.Count > 0)
      {
        long owner = this.Owners[0];
        this.Owners.RemoveAtFast<long>(0);
        MySession.Static.Players.TryGetIdentity(owner)?.ActiveContracts.Remove(this.Id);
        MySession.Static.GetComponent<MySessionComponentContractSystem>().SendNotificationToPlayer(MyContractNotificationTypes.ContractSuccessful, owner);
        this.FinishFor_Internal(owner, rewardeeCount);
      }
      if (this.m_unfinishedConditionCount > 0)
        MyLog.Default.WriteToLogAndAssert("MyContract - Not all conditions have been fulfilled, but contract still finished. Should this be happening?");
      if (this.OnContractSucceeded != null)
        this.OnContractSucceeded();
      if (MyVisualScriptLogicProvider.ContractFinished != null)
      {
        MyDefinitionId? definitionId = this.GetDefinitionId();
        if (definitionId.HasValue)
          MyVisualScriptLogicProvider.ContractFinished(this.Id, definitionId.Value, acceptingPlayerId, this.IsPlayerMade, this.StartBlock, this.StartFaction, this.StartStation);
        else
          MyLog.Default.WriteToLogAndAssert(string.Format("Contract definition not found. Something is really wrong. Contract type '{0}', contract id '{1}'.", (object) this.GetType().ToString(), (object) this.Id));
      }
      return MyContractResults.Success;
    }

    protected virtual void Finish_Internal()
    {
    }

    protected virtual void Fail_Internal()
    {
    }

    public bool Abandon(long playerId)
    {
      int num = this.FailFor(playerId, true) ? 1 : 0;
      if (this.Owners.Count <= 0)
        this.Fail(true);
      if (MyVisualScriptLogicProvider.ContractAbandoned == null)
        return num != 0;
      MyDefinitionId? definitionId = this.GetDefinitionId();
      if (definitionId.HasValue)
      {
        MyVisualScriptLogicProvider.ContractAbandoned(this.Id, definitionId.Value, playerId, this.IsPlayerMade, this.StartBlock, this.StartFaction, this.StartStation);
        return num != 0;
      }
      MyLog.Default.WriteToLogAndAssert(string.Format("Contract definition not found. Something is really wrong. Contract type '{0}', contract id '{1}'.", (object) this.GetType().ToString(), (object) this.Id));
      return num != 0;
    }

    public bool IsOwnerOfCondition(MyContractCondition cond) => this.ContractCondition == cond;

    public void Fail(bool abandon = false, bool punishOwner = true)
    {
      if (this.m_state != MyContractStateEnum.Active)
        MyLog.Default.WriteToLogAndAssert("Contract - Cannot fail Contract that is not active\nCurrent state: " + this.m_state.ToString());
      this.State = MyContractStateEnum.Failed;
      this.LogContract(abandon ? MyContract.MyContractLogType.ABANDON : MyContract.MyContractLogType.FAIL);
      if (this.IsPlayerMade && this.StartBlock != 0L && (MyEntities.GetEntityById(this.StartBlock) is MyCubeBlock entityById && entityById.OwnerId != 0L))
      {
        if (this.StartingDeposit > 0L)
        {
          if (punishOwner)
            MyBankingSystem.ChangeBalance(entityById.OwnerId, this.StartingDeposit);
          else if (this.Owners.Count > 0)
            MyBankingSystem.ChangeBalance(this.Owners[0], this.StartingDeposit);
        }
        if (this.RewardMoney > 0L)
          MyBankingSystem.ChangeBalance(entityById.OwnerId, this.RewardMoney);
      }
      if (((this.IsPlayerMade ? 0 : (this.StartingDeposit > 0L ? 1 : 0)) & (punishOwner ? 1 : 0)) != 0)
        MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddCurrencyDestroyed(this.StartingDeposit);
      this.Fail_Internal();
      long acceptingPlayerId = this.Owners.Count >= 1 ? this.Owners[0] : 0L;
      while (this.Owners.Count > 0)
        this.FailFor(this.Owners[0]);
      if (this.OnContractFailed != null)
        this.OnContractFailed();
      if (abandon || MyVisualScriptLogicProvider.ContractFailed == null)
        return;
      MyDefinitionId? definitionId = this.GetDefinitionId();
      if (definitionId.HasValue)
        MyVisualScriptLogicProvider.ContractFailed(this.Id, definitionId.Value, acceptingPlayerId, this.IsPlayerMade, this.StartBlock, this.StartFaction, this.StartStation, abandon);
      else
        MyLog.Default.WriteToLogAndAssert(string.Format("Contract definition not found. Something is really wrong. Contract type '{0}', contract id '{1}'.", (object) this.GetType().ToString(), (object) this.Id));
    }

    protected bool FailFor(long player, bool abandon = false)
    {
      int index = 0;
      foreach (long owner in this.Owners)
      {
        if (owner != player)
          ++index;
        else
          break;
      }
      if (index == this.Owners.Count)
        return false;
      this.Owners.RemoveAtFast<long>(index);
      MySession.Static.Players.TryGetIdentity(player)?.ActiveContracts.Remove(this.Id);
      if (this.StartFaction > 0L && Sync.IsServer && MySession.Static != null)
        MySession.Static.Factions.AddFactionPlayerReputation(player, this.StartFaction, -this.FailReputationPrice);
      this.FailFor_Internal(player, abandon);
      MySession.Static.GetComponent<MySessionComponentContractSystem>().SendNotificationToPlayer(MyContractNotificationTypes.ContractFailed, player);
      return true;
    }

    protected void CleanUp()
    {
      if (this.m_state != MyContractStateEnum.Finished && this.m_state != MyContractStateEnum.Failed && this.m_state != MyContractStateEnum.ToBeDisposed)
        MyLog.Default.WriteToLogAndAssert("Contract - Cannot cleanup Contract that is not finished/failed/marked-for-cleanup\nCurrent state: " + this.m_state.ToString());
      this.CleanUp_Internal();
    }

    protected virtual void Activate_Internal(MyTimeSpan timeOfActivation)
    {
      long owner = this.Owners[0];
      if (this.StartingDeposit > 0L)
        MyBankingSystem.ChangeBalance(owner, -this.StartingDeposit);
      this.ActivateCondition();
    }

    protected virtual void FinishFor_Internal(long player, int rewardeeCount)
    {
      if (Sync.IsServer && MySession.Static != null)
      {
        float num = 0.0f;
        if (rewardeeCount > 0)
          num = 1f / (float) rewardeeCount;
        if (this.CanPlayerReceiveReward(player))
        {
          if (this.StartFaction > 0L)
            MySession.Static.Factions.AddFactionPlayerReputation(player, this.StartFaction, (int) ((double) num * (double) this.GetRepRewardForPlayer(player)));
          long amount = (long) ((double) num * (double) this.GetMoneyRewardForPlayer(player));
          MyBankingSystem.ChangeBalance(player, amount);
          if (!this.IsPlayerMade)
            MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddCurrencyGenerated(amount);
        }
        if (this.StartingDeposit > 0L)
          MyBankingSystem.ChangeBalance(player, this.StartingDeposit);
      }
      this.CleanConditionForPlayer(player);
    }

    internal void DecreaseTicksToDiscard()
    {
      if (!this.TicksToDiscard.HasValue)
        return;
      int? ticksToDiscard = this.TicksToDiscard;
      this.TicksToDiscard = ticksToDiscard.HasValue ? new int?(ticksToDiscard.GetValueOrDefault() - 1) : new int?();
    }

    protected virtual bool CanPlayerReceiveReward(long player) => true;

    protected virtual void FailFor_Internal(long player, bool abandon = false) => this.CleanConditionForPlayer(player);

    private void ActivateCondition()
    {
      if (this.ContractCondition == null)
        return;
      if (this.ContractCondition is MyContractConditionDeliverPackage)
      {
        this.ActivateConditionDeliverPackage(this.ContractCondition);
      }
      else
      {
        if (!(this.ContractCondition is MyContractConditionDeliverItems))
          return;
        this.ActivateConditionDeliverItems(this.ContractCondition);
      }
    }

    private void ShareConditionWithPlayer(long identityId)
    {
      if (this.ContractCondition == null)
        return;
      if (this.ContractCondition is MyContractConditionDeliverPackage)
      {
        this.ShareConditionWithPlayerDeliverPackage(identityId, this.ContractCondition);
      }
      else
      {
        if (!(this.ContractCondition is MyContractConditionDeliverItems))
          return;
        this.ShareConditionWithPlayerDeliverItems(identityId, this.ContractCondition);
      }
    }

    private void CleanConditionForPlayer(long identityId)
    {
      if (this.ContractCondition == null)
        return;
      if (this.ContractCondition is MyContractConditionDeliverPackage)
      {
        this.CleanConditionForPlayerDeliverPackage(identityId, this.ContractCondition);
      }
      else
      {
        if (!(this.ContractCondition is MyContractConditionDeliverItems))
          return;
        this.CleanConditionForPlayerDeliverItems(identityId, this.ContractCondition);
      }
    }

    public void ReshareConditionWithAll()
    {
      foreach (long owner in this.Owners)
      {
        this.CleanConditionForPlayer(owner);
        this.ShareConditionWithPlayer(owner);
      }
    }

    protected virtual void Share_Internal(long identityId)
    {
    }

    protected virtual void CleanUp_Internal() => this.State = MyContractStateEnum.Disposed;

    protected virtual MyActivationResults CanActivate_Internal(long playerId) => this.CheckPlayerFunds(playerId);

    protected virtual bool NeedsUpdate_Internal()
    {
      if (this.State == MyContractStateEnum.Disposed)
        return false;
      return this.RemainingTime.HasValue || this.State == MyContractStateEnum.Failed || this.State == MyContractStateEnum.Finished || this.State == MyContractStateEnum.ToBeDisposed;
    }

    protected virtual bool CanBeShared_Internal() => false;

    public virtual bool CanBeFinished_Internal() => this.m_unfinishedConditionCount <= 0;

    public virtual void TimeRanOut_Internal()
    {
      if (this.m_state != MyContractStateEnum.Active)
        return;
      this.Fail();
    }

    public virtual long GetMoneyRewardForPlayer(long playerId) => this.RewardMoney;

    public virtual int GetRepRewardForPlayer(long playerId) => this.RewardReputation;

    public virtual MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = MyContractFactory.CreateObjectBuilder(this);
      objectBuilder.Id = this.Id;
      objectBuilder.IsPlayerMade = this.IsPlayerMade;
      objectBuilder.State = this.m_state;
      objectBuilder.Owners = new MySerializableList<long>((IEnumerable<long>) this.Owners);
      objectBuilder.RewardMoney = this.RewardMoney;
      objectBuilder.RewardReputation = this.RewardReputation;
      objectBuilder.StartingDeposit = this.StartingDeposit;
      objectBuilder.FailReputationPrice = this.FailReputationPrice;
      objectBuilder.StartFaction = this.StartFaction;
      objectBuilder.StartStation = this.StartStation;
      objectBuilder.StartBlock = this.StartBlock;
      objectBuilder.Creation = this.Creation.Ticks;
      objectBuilder.TicksToDiscard = this.TicksToDiscard;
      if (this.RemainingTime.HasValue)
        objectBuilder.RemainingTimeInS = new double?(this.RemainingTime.Value.Seconds);
      if (this.ContractCondition != null)
        objectBuilder.ContractCondition = this.ContractCondition.GetObjectBuilder();
      return objectBuilder;
    }

    public virtual void Init(MyObjectBuilder_Contract ob)
    {
      this.Id = ob.Id;
      this.IsPlayerMade = ob.IsPlayerMade;
      this.m_state = ob.State;
      this.Owners = (List<long>) ob.Owners;
      this.RewardMoney = ob.RewardMoney;
      this.RewardReputation = ob.RewardReputation;
      this.StartingDeposit = ob.StartingDeposit;
      this.FailReputationPrice = ob.FailReputationPrice;
      this.StartFaction = ob.StartFaction;
      this.StartStation = ob.StartStation;
      this.StartBlock = ob.StartBlock;
      this.Creation = new MyTimeSpan(ob.Creation);
      this.TicksToDiscard = ob.TicksToDiscard;
      if (ob.RemainingTimeInS.HasValue)
        this.RemainingTime = new MyTimeSpan?(MyTimeSpan.FromSeconds(ob.RemainingTimeInS.Value));
      if (ob.ContractCondition == null)
        return;
      this.ContractCondition = MyContractConditionFactory.CreateInstance(ob.ContractCondition);
      if (ob.ContractCondition.IsFinished)
        return;
      ++this.m_unfinishedConditionCount;
    }

    public virtual void BeforeStart()
    {
    }

    public void RecalculateUnfinishedCondiCount() => this.m_unfinishedConditionCount = this.ContractCondition.IsFinished ? 0 : 1;

    public virtual void Update(MyTimeSpan currentTime)
    {
      if (this.State == MyContractStateEnum.Failed || this.State == MyContractStateEnum.Finished)
        this.CleanUp();
      if (!this.IsTimeLimited || this.State != MyContractStateEnum.Active)
        return;
      this.UpdateTime(currentTime);
      this.CheckTimeOut();
    }

    private void UpdateTime(MyTimeSpan currentTime)
    {
      if (!this.RemainingTime.HasValue)
        return;
      if (!this.m_lastTimeUpdate.HasValue)
      {
        this.m_lastTimeUpdate = new MyTimeSpan?(currentTime);
      }
      else
      {
        MyTimeSpan myTimeSpan1 = currentTime - this.m_lastTimeUpdate.Value;
        MyTimeSpan? remainingTime = this.RemainingTime;
        MyTimeSpan myTimeSpan2 = myTimeSpan1;
        this.RemainingTime = remainingTime.HasValue ? new MyTimeSpan?(remainingTime.GetValueOrDefault() - myTimeSpan2) : new MyTimeSpan?();
        this.m_lastTimeUpdate = new MyTimeSpan?(currentTime);
      }
    }

    private void CheckTimeOut()
    {
      if (!this.RemainingTime.HasValue || !(this.RemainingTime.Value <= MyTimeSpan.Zero))
        return;
      this.TimeRanOut_Internal();
    }

    public int CountRemainingConditions() => this.m_unfinishedConditionCount;

    public virtual string ToDebugString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(string.Format("Name: {0}", (object) this.GetType().ToString()));
      return stringBuilder.ToString();
    }

    private MyActivationResults CheckPlayerFunds(long playerId)
    {
      if (this.StartingDeposit <= 0L)
        return MyActivationResults.Success;
      MyBankingSystem component = MySession.Static.GetComponent<MyBankingSystem>();
      MyAccountInfo account;
      if (component == null || !component.TryGetAccountInfo(playerId, out account))
        return MyActivationResults.Error;
      return account.Balance < this.StartingDeposit ? MyActivationResults.Fail_InsufficientFunds : MyActivationResults.Success;
    }

    public virtual MyDefinitionId? GetDefinitionId() => new MyDefinitionId?();

    public MyContractTypeDefinition GetDefinition()
    {
      MyDefinitionId? definitionId = this.GetDefinitionId();
      return !definitionId.HasValue ? (MyContractTypeDefinition) null : MyDefinitionManager.Static.GetContractType(definitionId.Value.SubtypeName);
    }

    private bool ActivateConditionDeliverPackage(MyContractCondition cond)
    {
      if (!(this.ContractCondition is MyContractConditionDeliverPackage contractCondition))
        return false;
      long owner1 = this.Owners[0];
      MyObjectBuilder_Package newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Package>("Package");
      newObject.ContractConditionId = contractCondition.Id;
      newObject.ContractId = this.Id;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(owner1);
      if (identity == null)
        return false;
      MyCharacter character = identity.Character;
      if (character == null || character.InventoryCount <= 0)
        return false;
      character.GetInventoryBase().AddItems((MyFixedPoint) 1, (MyObjectBuilder_Base) newObject);
      bool flag = true;
      foreach (long owner2 in this.Owners)
        flag &= this.ShareConditionWithPlayerDeliverPackage(owner2, cond);
      return flag;
    }

    private bool ActivateConditionDeliverItems(MyContractCondition cond)
    {
      bool flag = true;
      foreach (long owner in this.Owners)
        flag &= this.ShareConditionWithPlayerDeliverItems(owner, cond);
      return flag;
    }

    private bool ShareConditionWithPlayerDeliverPackage(long identityId, MyContractCondition cond)
    {
      if (!(cond is MyContractConditionDeliverPackage conditionDeliverPackage))
        return false;
      Vector3D vector3D = Vector3D.Zero;
      long entityId = 0;
      MyStation stationByStationId = MySession.Static.Factions.GetStationByStationId(conditionDeliverPackage.StationEndId);
      if (stationByStationId != null)
      {
        vector3D = stationByStationId.Position;
        entityId = 0L;
        if (stationByStationId.StationEntityId != 0L && MyEntities.GetEntityById(stationByStationId.StationEntityId) is MyCubeGrid entityById)
        {
          MyContractBlock firstBlockOfType = entityById.GetFirstBlockOfType<MyContractBlock>();
          if (firstBlockOfType != null)
          {
            vector3D = firstBlockOfType.PositionComp.GetPosition();
            entityId = firstBlockOfType.EntityId;
          }
        }
      }
      else
      {
        MyEntity entityById = MyEntities.GetEntityById(conditionDeliverPackage.BlockEndId);
        if (entityById != null)
        {
          vector3D = entityById.PositionComp.GetPosition();
          entityId = entityById.EntityId;
        }
      }
      MyGps gps = new MyGps();
      gps.DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Delivery_GpsName);
      gps.Name = MyTexts.GetString(MyCommonTexts.Contract_Delivery_GpsName);
      gps.Description = MyTexts.GetString(MyCommonTexts.Contract_Delivery_GpsDescription);
      gps.Coords = vector3D;
      gps.ShowOnHud = true;
      gps.DiscardAt = new TimeSpan?();
      gps.GPSColor = Color.DarkOrange;
      gps.ContractId = this.Id;
      gps.SetEntityId(entityId);
      MySession.Static.Gpss.SendAddGps(identityId, ref gps, entityId);
      return true;
    }

    private bool ShareConditionWithPlayerDeliverItems(long identityId, MyContractCondition cond)
    {
      if (!(cond is MyContractConditionDeliverItems conditionDeliverItems))
        return false;
      Vector3D vector3D = Vector3D.Zero;
      long entityId = 0;
      MyStation stationByStationId = MySession.Static.Factions.GetStationByStationId(conditionDeliverItems.StationEndId);
      if (stationByStationId != null)
      {
        vector3D = stationByStationId.Position;
        entityId = 0L;
        if (MyEntities.GetEntityById(stationByStationId.StationEntityId) is MyCubeGrid entityById)
        {
          MyContractBlock firstBlockOfType = entityById.GetFirstBlockOfType<MyContractBlock>();
          if (firstBlockOfType != null)
          {
            vector3D = firstBlockOfType.PositionComp.GetPosition();
            entityId = firstBlockOfType.EntityId;
          }
        }
      }
      else
      {
        MyEntity entityById = MyEntities.GetEntityById(conditionDeliverItems.BlockEndId);
        if (entityById != null)
        {
          vector3D = entityById.PositionComp.GetPosition();
          entityId = entityById.EntityId;
        }
      }
      MyGps gps = new MyGps();
      gps.DisplayName = MyTexts.GetString(MyCommonTexts.Contract_ObtainAndDelivery_GpsName);
      gps.Name = MyTexts.GetString(MyCommonTexts.Contract_ObtainAndDelivery_GpsName);
      gps.Description = MyTexts.GetString(MyCommonTexts.Contract_ObtainAndDelivery_GpsDescription);
      gps.Coords = vector3D;
      gps.ShowOnHud = true;
      gps.DiscardAt = new TimeSpan?();
      gps.GPSColor = Color.DarkOrange;
      gps.ContractId = this.Id;
      gps.SetEntityId(entityId);
      MySession.Static.Gpss.SendAddGps(identityId, ref gps, entityId);
      return true;
    }

    private void CleanConditionForPlayerDeliverPackage(long identityId, MyContractCondition cond)
    {
      if (!(cond is MyContractConditionDeliverPackage))
        return;
      MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, this.Id);
      if (gpsByContractId == null)
        return;
      MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
    }

    private void CleanConditionForPlayerDeliverItems(long identityId, MyContractCondition cond)
    {
      if (!(cond is MyContractConditionDeliverItems))
        return;
      MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, this.Id);
      if (gpsByContractId == null)
        return;
      MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
    }

    protected void CreateParticleEffectOnEntity(string name, long targetEntity, bool offset)
    {
      MyMultiplayer.RaiseStaticEvent<string, long, bool>((Func<IMyEventOwner, Action<string, long, bool>>) (x => new Action<string, long, bool>(MyContract.CreateParticleEffectOnEntityEvent)), name, targetEntity, offset);
      if (Sync.IsDedicated)
        return;
      MyContract.CreateParticleEffectOnEntityEvent(name, targetEntity, offset);
    }

    [Event(null, 923)]
    [Reliable]
    [Broadcast]
    private static void CreateParticleEffectOnEntityEvent(
      string name,
      long targetEntity,
      bool offset)
    {
      if (!(MyEntities.GetEntityById(targetEntity) is MyCubeGrid entityById))
        return;
      double num = entityById.PositionComp.WorldAABB.HalfExtents.AbsMax() * 2.0;
      MatrixD matrixD = entityById.PositionComp.WorldMatrixRef;
      Vector3D translation = matrixD.Translation;
      if (offset)
        translation += num * matrixD.Forward;
      MatrixD effectMatrix = (offset ? -1.0 : 1.0) * MatrixD.Identity;
      if (offset)
        effectMatrix.Translation = num * MatrixD.Identity.Forward;
      MyParticlesManager.TryCreateParticleEffect(name, ref effectMatrix, ref translation, entityById.Render.GetRenderObjectID(), out MyParticleEffect _);
    }

    private void LogContract(MyContract.MyContractLogType logType)
    {
      string str1 = string.Empty;
      if (this.StartFaction != 0L)
      {
        IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(this.StartFaction);
        if (factionById != null)
          str1 = factionById.Tag;
      }
      EndpointId endpointId = new EndpointId();
      if (this.Owners.Count > 0)
        endpointId = new EndpointId(MySession.Static.Players.TryGetSteamId(this.Owners[0]));
      ulong num1 = 0;
      if (this.StartBlock != 0L && MyEntities.GetEntityById(this.StartBlock) is MyCubeBlock entityById)
        num1 = MySession.Static.Players.TryGetSteamId(entityById.OwnerId);
      string str2 = string.Empty;
      int num2 = 0;
      if (this.ContractCondition is MyContractConditionDeliverItems contractCondition)
      {
        str2 = string.Format("{0}_{1}", (object) contractCondition.ItemType.TypeId.ToString(), (object) contractCondition.ItemType.SubtypeName);
        num2 = contractCondition.ItemAmount;
      }
      string msg1 = string.Format("CONTRACT LEGEND,change,id,type,playerMade,reputation,currency,factionTag,ownerId,playerId,ItemType,ItemAmount");
      string msg2 = string.Format("CONTRACT,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", (object) logType.ToString(), (object) this.Id, (object) this.GetType().ToString(), (object) this.IsPlayerMade, (object) this.RewardReputation, (object) this.RewardMoney, (object) str1, (object) num1, (object) endpointId, (object) str2, (object) num2);
      MyLog.Default.WriteLine(msg1);
      MyLog.Default.WriteLine(msg2);
    }

    internal void RefundRewardOnDelete(MyContractBlock startBlock)
    {
      if (this.State != MyContractStateEnum.Inactive || !this.IsPlayerMade || (startBlock == null || startBlock.EntityId != this.StartBlock))
        return;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, this.RewardMoney);
    }

    private enum MyContractLogType
    {
      ACCEPT,
      FINISH,
      FAIL,
      ABANDON,
    }

    protected sealed class CreateParticleEffectOnEntityEvent\u003C\u003ESystem_String\u0023System_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, string, long, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string name,
        in long targetEntity,
        in bool offset,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyContract.CreateParticleEffectOnEntityEvent(name, targetEntity, offset);
      }
    }
  }
}
