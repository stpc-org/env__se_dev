// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Electricity.MyBattery
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.Electricity
{
  [StaticEventOwner]
  public class MyBattery
  {
    public static readonly string BATTERY_CHARGE_STAT_NAME = "BatteryCharge";
    public static float BATTERY_DEPLETION_MULTIPLIER = 1f;
    private int m_lastUpdateTime;
    private MyEntity m_lastParent;
    public const float EnergyCriticalThresholdCharacter = 0.1f;
    public const float EnergyLowThresholdCharacter = 0.25f;
    public const float EnergyCriticalThresholdShip = 0.05f;
    public const float EnergyLowThresholdShip = 0.125f;
    private const int m_productionUpdateInterval = 100;
    private readonly MyCharacter m_owner;
    private readonly MyStringHash m_resourceSinkGroup = MyStringHash.GetOrCompute("Charging");
    private readonly MyStringHash m_resourceSourceGroup = MyStringHash.GetOrCompute("Battery");
    public float RechargeMultiplier = 1f;
    private Dictionary<int, MyBattery.MyBatteryRegenerationEffect> m_regenEffects = new Dictionary<int, MyBattery.MyBatteryRegenerationEffect>();
    private static List<int> m_tmpRemoveEffects = new List<int>();

    public bool IsEnergyCriticalShip => (double) this.ResourceSource.RemainingCapacity / 9.99999974737875E-06 < 0.0500000007450581;

    public bool IsEnergyLowShip => (double) this.ResourceSource.RemainingCapacity / 9.99999974737875E-06 < 0.125;

    public MyCharacter Owner => this.m_owner;

    public MyResourceSinkComponent ResourceSink { get; private set; }

    public MyResourceSourceComponent ResourceSource { get; private set; }

    public bool OwnedByLocalPlayer { get; set; }

    public MyBattery(MyCharacter owner)
    {
      this.m_owner = owner;
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSource = new MyResourceSourceComponent();
    }

    public void Init(
      MyObjectBuilder_Battery builder,
      List<MyResourceSinkInfo> additionalSinks = null,
      List<MyResourceSourceInfo> additionalSources = null)
    {
      MyResourceSinkInfo sinkData = new MyResourceSinkInfo()
      {
        MaxRequiredInput = 0.0018f,
        ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
        RequiredInputFunc = new Func<float>(this.Sink_ComputeRequiredPower)
      };
      if (additionalSinks != null)
      {
        additionalSinks.Insert(0, sinkData);
        this.ResourceSink.Init(this.m_resourceSinkGroup, additionalSinks);
      }
      else
        this.ResourceSink.Init(this.m_resourceSinkGroup, sinkData);
      this.ResourceSink.TemporaryConnectedEntity = (IMyEntity) this.m_owner;
      MyResourceSourceInfo sourceResourceData = new MyResourceSourceInfo()
      {
        ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
        DefinedOutput = 0.009f,
        ProductionToCapacityMultiplier = 3600f
      };
      if (additionalSources != null)
      {
        additionalSources.Insert(0, sourceResourceData);
        this.ResourceSource.Init(this.m_resourceSourceGroup, additionalSources);
      }
      else
        this.ResourceSource.Init(this.m_resourceSourceGroup, sourceResourceData);
      this.ResourceSource.TemporaryConnectedEntity = (MyEntity) this.m_owner;
      this.m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
      if (builder == null)
      {
        this.ResourceSource.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, true);
        this.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, 1E-05f);
        this.ResourceSink.Update();
      }
      else
      {
        this.ResourceSource.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, builder.ProducerEnabled);
        if (MySession.Static.SurvivalMode)
          this.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, MathHelper.Clamp(builder.CurrentCapacity, 0.0f, 1E-05f));
        else
          this.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, 1E-05f);
        if (builder.RegenEffects != null)
        {
          foreach (MyObjectBuilder_BatteryRegenerationEffect regenEffect in builder.RegenEffects)
          {
            MyBattery.MyBatteryRegenerationEffect regenerationEffect = new MyBattery.MyBatteryRegenerationEffect();
            regenerationEffect.Init(regenEffect);
            this.m_regenEffects.Add(this.m_regenEffects.Count, regenerationEffect);
          }
        }
        this.ResourceSink.Update();
      }
    }

    public MyObjectBuilder_Battery GetObjectBuilder()
    {
      MyObjectBuilder_Battery newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Battery>();
      newObject.ProducerEnabled = this.ResourceSource.Enabled;
      newObject.CurrentCapacity = this.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId);
      newObject.RegenEffects = new List<MyObjectBuilder_BatteryRegenerationEffect>();
      foreach (KeyValuePair<int, MyBattery.MyBatteryRegenerationEffect> regenEffect in this.m_regenEffects)
        newObject.RegenEffects.Add(regenEffect.Value.GetObjectBuilder());
      return newObject;
    }

    public float Sink_ComputeRequiredPower() => Math.Min((float) ((9.99999974737875E-06 - (double) this.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId)) * 60.0 / 100.0 * (double) this.ResourceSource.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId) + (double) this.ResourceSource.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId) * (MySession.Static.CreativeMode ? 0.0 : 1.0)), 0.0018f);

    public void UpdateOnServer100()
    {
      if (!Sync.IsServer)
        return;
      MyEntity parent = this.m_owner.Parent;
      if (this.m_lastParent != parent)
      {
        this.ResourceSink.Update();
        this.m_lastParent = parent;
      }
      if (this.ResourceSource.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId) || (double) this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) > 0.0)
      {
        float num1 = (float) (MySession.Static.GameplayFrameCounter - this.m_lastUpdateTime) * 0.01666667f;
        this.m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
        float multiplierByType = this.ResourceSource.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId);
        float num2 = this.ResourceSource.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId) / multiplierByType;
        float num3 = MyFakes.ENABLE_BATTERY_SELF_RECHARGE ? this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId) : this.RechargeMultiplier * this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) / multiplierByType;
        float num4 = MySession.Static.CreativeMode ? 0.0f : num1 * num2;
        float num5 = num1 * num3 - num4;
        float num6 = this.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId) + num5;
        this.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, MathHelper.Clamp(num6, 0.0f, 1E-05f));
      }
      if (!this.ResourceSource.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
        this.ResourceSink.Update();
      this.UpdateEffects100();
      MyMultiplayer.RaiseStaticEvent<long, float>((Func<IMyEventOwner, Action<long, float>>) (s => new Action<long, float>(MyBattery.SyncCapacitySuccess)), this.Owner.EntityId, this.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId));
    }

    [Event(null, 219)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void SyncCapacitySuccess(long entityId, float remainingCapacity)
    {
      MyCharacter entity;
      MyEntities.TryGetEntityById<MyCharacter>(entityId, out entity);
      entity?.SuitBattery.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, remainingCapacity);
    }

    public void DebugDepleteBattery() => this.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, 0.0f);

    private void UpdateEffects100()
    {
      foreach (KeyValuePair<int, MyBattery.MyBatteryRegenerationEffect> regenEffect in this.m_regenEffects)
      {
        MyBattery.MyBatteryRegenerationEffect effect = regenEffect.Value;
        if (effect.RemainingTime <= MyTimeSpan.Zero)
          MyBattery.m_tmpRemoveEffects.Add(regenEffect.Key);
        else if (Sync.IsServer)
          this.UpdateEffect100(effect);
      }
      foreach (int tmpRemoveEffect in MyBattery.m_tmpRemoveEffects)
        this.RemoveEffect(tmpRemoveEffect);
      MyBattery.m_tmpRemoveEffects.Clear();
    }

    private void UpdateEffect100(MyBattery.MyBatteryRegenerationEffect effect)
    {
      MyTimeSpan myTimeSpan1 = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      MyTimeSpan myTimeSpan2 = myTimeSpan1 - effect.LastRegenTime;
      if (!(myTimeSpan2 > MyTimeSpan.Zero))
        return;
      float num1 = this.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId);
      float num2 = (float) (9.99999974737875E-06 * (Math.Min(myTimeSpan2.Seconds, effect.RemainingTime.Seconds) * (double) effect.ChargePerSecond));
      this.ResourceSource.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, Math.Min(1E-05f, num1 + num2));
      effect.LastRegenTime = myTimeSpan1;
      effect.RemainingTime -= myTimeSpan2;
    }

    public bool HasAnyComsumableEffect() => this.m_regenEffects.Count > 0;

    public int AddEffect(float chargePerSecond, double miliseconds)
    {
      MyBattery.MyBatteryRegenerationEffect regenerationEffect = new MyBattery.MyBatteryRegenerationEffect()
      {
        ChargePerSecond = chargePerSecond,
        RemainingTime = MyTimeSpan.FromMilliseconds(miliseconds),
        LastRegenTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds)
      };
      int key = 0;
      while (key < this.m_regenEffects.Count && this.m_regenEffects.ContainsKey(key))
        ++key;
      this.m_regenEffects.Add(key, regenerationEffect);
      return key;
    }

    public void RemoveEffect(int id) => this.m_regenEffects.Remove(id);

    internal void Consume(MyFixedPoint amount, MyConsumableItemDefinition consumableDef)
    {
      if (consumableDef == null)
        return;
      foreach (MyConsumableItemDefinition.StatValue stat in consumableDef.Stats)
      {
        if (string.Equals(stat.Name, MyBattery.BATTERY_CHARGE_STAT_NAME))
          this.AddEffect((float) amount.ToIntSafe() * stat.Value, (double) stat.Time * 1000.0);
      }
    }

    private class MyBatteryRegenerationEffect
    {
      public float ChargePerSecond;
      public MyTimeSpan RemainingTime;
      public MyTimeSpan LastRegenTime;

      public void Init(MyObjectBuilder_BatteryRegenerationEffect builder)
      {
        this.ChargePerSecond = builder.ChargePerSecond;
        this.RemainingTime = MyTimeSpan.FromMilliseconds((double) builder.RemainingTimeInMiliseconds);
        this.LastRegenTime = MyTimeSpan.FromMilliseconds((double) builder.LastRegenTimeInMiliseconds);
      }

      public MyObjectBuilder_BatteryRegenerationEffect GetObjectBuilder() => new MyObjectBuilder_BatteryRegenerationEffect()
      {
        ChargePerSecond = this.ChargePerSecond,
        RemainingTimeInMiliseconds = (long) this.RemainingTime.Milliseconds,
        LastRegenTimeInMiliseconds = (long) this.LastRegenTime.Milliseconds
      };
    }

    protected sealed class SyncCapacitySuccess\u003C\u003ESystem_Int64\u0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in float remainingCapacity,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBattery.SyncCapacitySuccess(entityId, remainingCapacity);
      }
    }
  }
}
