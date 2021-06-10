// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractHunt
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractHunt))]
  public class MyContractHunt : MyContract
  {
    private bool m_targetKilled;
    private bool m_targetKilledDirectly;
    private Vector3D? m_deathLocation;

    public long Target { get; private set; }

    public MyTimeSpan TimerNextRemark { get; private set; }

    public MyTimeSpan RemarkPeriod { get; private set; }

    public float RemarkVariance { get; private set; }

    public Vector3D MarkPosition { get; private set; }

    public bool IsTargetInWorld { get; private set; }

    public double KillRange { get; private set; }

    public float KillRangeMultiplier { get; private set; }

    public int ReputationLossForTarget { get; private set; }

    public double RewardRadius { get; private set; }

    public override MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
      MyObjectBuilder_ContractHunt builderContractHunt = objectBuilder as MyObjectBuilder_ContractHunt;
      builderContractHunt.Target = this.Target;
      builderContractHunt.TimerNextRemark = this.TimerNextRemark.Ticks;
      builderContractHunt.RemarkPeriod = this.RemarkPeriod.Ticks;
      builderContractHunt.RemarkVariance = this.RemarkVariance;
      builderContractHunt.MarkPosition = (SerializableVector3D) this.MarkPosition;
      builderContractHunt.IsTargetInWorld = this.IsTargetInWorld;
      builderContractHunt.KillRange = this.KillRange;
      builderContractHunt.KillRangeMultiplier = this.KillRangeMultiplier;
      builderContractHunt.ReputationLossForTarget = this.ReputationLossForTarget;
      builderContractHunt.RewardRadius = this.RewardRadius;
      builderContractHunt.TargetKilled = this.m_targetKilled;
      builderContractHunt.TargetKilledDirectly = this.m_targetKilledDirectly;
      return objectBuilder;
    }

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      if (!(ob is MyObjectBuilder_ContractHunt builderContractHunt))
        return;
      this.Target = builderContractHunt.Target;
      this.TimerNextRemark = new MyTimeSpan(builderContractHunt.TimerNextRemark);
      this.RemarkPeriod = new MyTimeSpan(builderContractHunt.RemarkPeriod);
      this.RemarkVariance = builderContractHunt.RemarkVariance;
      this.MarkPosition = (Vector3D) builderContractHunt.MarkPosition;
      this.IsTargetInWorld = builderContractHunt.IsTargetInWorld;
      this.KillRange = builderContractHunt.KillRange;
      this.KillRangeMultiplier = builderContractHunt.KillRangeMultiplier;
      this.ReputationLossForTarget = builderContractHunt.ReputationLossForTarget;
      this.RewardRadius = builderContractHunt.RewardRadius;
      this.m_targetKilled = builderContractHunt.TargetKilled;
      this.m_targetKilledDirectly = builderContractHunt.TargetKilledDirectly;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (this.State != MyContractStateEnum.Active)
        return;
      MySession.Static.Factions.PlayerKilledByPlayer += new Action<long, long>(this.KilledByPlayer);
      MySession.Static.Factions.PlayerKilledByUnknown += new Action<long>(this.KilledByUnknown);
    }

    private void KilledByPlayer(long victimId, long killerId)
    {
      if (victimId != this.Target || !this.Owners.Contains(killerId))
        return;
      this.m_deathLocation = this.GetVictimLocation();
      if (!this.m_deathLocation.HasValue)
        return;
      this.m_targetKilled = true;
      this.m_targetKilledDirectly = true;
      int num = (int) this.Finish();
      long pirateFactionId = this.GetPirateFactionId();
      if (pirateFactionId == 0L)
        return;
      MySession.Static.Factions.AddFactionPlayerReputation(victimId, pirateFactionId, -this.ReputationLossForTarget);
    }

    private long GetPirateFactionId()
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null || !(MyDefinitionManager.Static.GetDefinition(component.EconomyDefinition.PirateId) is MyFactionDefinition definition))
        return 0;
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        if (faction.Value.Tag == definition.Tag)
          return faction.Value.FactionId;
      }
      return 0;
    }

    private Vector3D? GetVictimLocation()
    {
      MyPlayer.PlayerId result;
      if (!MySession.Static.Players.TryGetPlayerId(this.Target, out result))
        return new Vector3D?();
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      return playerById == null || playerById.Controller == null || playerById.Controller.ControlledEntity == null ? new Vector3D?() : new Vector3D?((playerById.Controller.ControlledEntity as MyEntity).PositionComp.GetPosition());
    }

    private void KilledByUnknown(long victimId)
    {
      if (victimId != this.Target)
        return;
      bool flag = false;
      this.m_deathLocation = this.GetVictimLocation();
      if (!this.m_deathLocation.HasValue)
        return;
      double num1 = this.KillRange * this.KillRange;
      foreach (long owner in this.Owners)
      {
        MyPlayer.PlayerId result;
        if (MySession.Static.Players.TryGetPlayerId(owner, out result))
        {
          MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
          if (playerById != null && playerById.Controller != null && playerById.Controller.ControlledEntity != null && (this.m_deathLocation.Value - (playerById.Controller.ControlledEntity as MyEntity).PositionComp.GetPosition()).LengthSquared() <= num1)
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      this.m_targetKilled = true;
      this.m_targetKilledDirectly = false;
      int num2 = (int) this.Finish();
      if (this.GetPirateFactionId() == 0L)
        return;
      MySession.Static.Factions.AddFactionPlayerReputation(victimId, this.StartFaction, (int) ((double) this.KillRangeMultiplier * (double) this.ReputationLossForTarget));
    }

    public override bool CanBeFinished_Internal() => base.CanBeFinished_Internal() && this.m_targetKilled;

    protected override bool CanPlayerReceiveReward(long identityId)
    {
      MyPlayer.PlayerId result;
      if (!this.m_deathLocation.HasValue || !MySession.Static.Players.TryGetPlayerId(identityId, out result))
        return false;
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      if (playerById == null || playerById.Controller == null || playerById.Controller.ControlledEntity == null)
        return false;
      MyEntity controlledEntity = playerById.Controller.ControlledEntity as MyEntity;
      double num = this.RewardRadius + controlledEntity.PositionComp.WorldAABB.HalfExtents.Length();
      return (controlledEntity.PositionComp.GetPosition() - this.m_deathLocation.Value).LengthSquared() <= num * num;
    }

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Hunt"));

    protected override MyActivationResults CanActivate_Internal(long playerId)
    {
      MyActivationResults activationResults = base.CanActivate_Internal(playerId);
      if (activationResults != MyActivationResults.Success)
        return activationResults;
      if (this.Target == playerId)
        return MyActivationResults.Fail_YouAreTargetOfThisHunt;
      bool flag = false;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Identity.IdentityId == this.Target)
        {
          flag = true;
          break;
        }
      }
      return !flag ? MyActivationResults.Fail_TargetNotOnline : MyActivationResults.Success;
    }

    protected override void Activate_Internal(MyTimeSpan timeOfActivation)
    {
      base.Activate_Internal(timeOfActivation);
      foreach (long owner in this.Owners)
        this.Mark(owner);
      MySession.Static.Factions.PlayerKilledByPlayer += new Action<long, long>(this.KilledByPlayer);
      MySession.Static.Factions.PlayerKilledByUnknown += new Action<long>(this.KilledByUnknown);
    }

    protected override void FailFor_Internal(long player, bool abandon = false)
    {
      base.FailFor_Internal(player, abandon);
      this.Unmark(player);
    }

    protected override void FinishFor_Internal(long player, int rewardeeCount)
    {
      base.FinishFor_Internal(player, rewardeeCount);
      this.Unmark(player);
    }

    public override void Update(MyTimeSpan currentTime)
    {
      base.Update(currentTime);
      if (!(this.TimerNextRemark < currentTime))
        return;
      foreach (long owner in this.Owners)
        this.Unmark(owner);
      this.ChangeMarkPosition();
      foreach (long owner in this.Owners)
        this.Mark(owner);
      this.TimerNextRemark = currentTime + this.RemarkPeriod;
    }

    private void ChangeMarkPosition()
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.Target);
      if (identity == null || identity.IsDead || identity.Character == null)
        this.IsTargetInWorld = false;
      else
        this.MarkPosition = new BoundingSphereD(identity.Character.PositionComp.GetPosition(), (double) this.RemarkVariance).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
    }

    protected override void CleanUp_Internal()
    {
      base.CleanUp_Internal();
      MySession.Static.Factions.PlayerKilledByPlayer -= new Action<long, long>(this.KilledByPlayer);
      MySession.Static.Factions.PlayerKilledByUnknown -= new Action<long>(this.KilledByUnknown);
    }

    private void Remark(long playerId)
    {
      this.Unmark(playerId);
      this.Mark(playerId);
    }

    private void Mark(long playerId)
    {
      MyGps gps = this.CreateGPS();
      MySession.Static.Gpss.SendAddGps(playerId, ref gps);
    }

    private void Unmark(long identityId)
    {
      MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, this.Id);
      if (gpsByContractId == null)
        return;
      MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
    }

    private MyGps CreateGPS() => new MyGps()
    {
      DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Hunt_GpsName),
      Name = MyTexts.GetString(MyCommonTexts.Contract_Hunt_GpsName),
      Description = MyTexts.GetString(MyCommonTexts.Contract_Hunt_GpsDescription),
      Coords = this.MarkPosition,
      ShowOnHud = true,
      DiscardAt = new TimeSpan?(),
      GPSColor = Color.DarkOrange,
      ContractId = this.Id
    };

    public override long GetMoneyRewardForPlayer(long playerId) => (long) ((this.m_targetKilledDirectly ? 1.0 : (double) this.KillRangeMultiplier) * (double) base.GetMoneyRewardForPlayer(playerId));

    public override int GetRepRewardForPlayer(long playerId) => (int) ((this.m_targetKilledDirectly ? 1.0 : (double) this.KillRangeMultiplier) * (double) base.GetRepRewardForPlayer(playerId));
  }
}
