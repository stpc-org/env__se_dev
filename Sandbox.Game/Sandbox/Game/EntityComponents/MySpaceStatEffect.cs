// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MySpaceStatEffect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.EntityComponents
{
  [MyStatLogicDescriptor("SpaceStatEffect")]
  public class MySpaceStatEffect : MyStatLogic
  {
    private static MyStringHash HealthId = MyStringHash.GetOrCompute(nameof (Health));
    public static readonly float MAX_REGEN_HEALTH_RATIO = 0.7f;
    private int m_healthEffectId;
    private bool m_effectCreated;

    private MyEntityStat Health
    {
      get
      {
        MyEntityStat myEntityStat;
        return this.m_stats.TryGetValue(MySpaceStatEffect.HealthId, out myEntityStat) ? myEntityStat : (MyEntityStat) null;
      }
    }

    public override void Init(
      IMyCharacter character,
      Dictionary<MyStringHash, MyEntityStat> stats,
      string scriptName)
    {
      base.Init(character, stats, scriptName);
      this.InitActions();
      MyEntityStat health = this.Health;
      if (health == null)
        return;
      health.OnStatChanged += new MyEntityStat.StatChangedDelegate(this.OnHealthChanged);
    }

    public override void Close()
    {
      MyEntityStat health = this.Health;
      if (health != null)
        health.OnStatChanged -= new MyEntityStat.StatChangedDelegate(this.OnHealthChanged);
      this.ClearRegenEffect();
      base.Close();
    }

    public override void Update10()
    {
      base.Update10();
      if (!MySession.Static.Settings.EnableOxygen || !this.EnableAutoHealing)
        return;
      if ((MySession.Static.Settings.EnableOxygenPressurization ? (double) Math.Max(this.Character.EnvironmentOxygenLevel, this.Character.OxygenLevel) : (double) this.Character.EnvironmentOxygenLevel) > (double) MyEffectConstants.MinOxygenLevelForHealthRegeneration)
      {
        if ((double) this.Health.Value < (double) MySpaceStatEffect.MAX_REGEN_HEALTH_RATIO * 100.0)
        {
          if (this.m_effectCreated)
            return;
          this.CreateRegenEffect();
        }
        else
        {
          if (!this.m_effectCreated)
            return;
          this.ClearRegenEffect();
        }
      }
      else
      {
        if (!this.m_effectCreated)
          return;
        this.ClearRegenEffect();
      }
    }

    private void OnHealthChanged(float newValue, float oldValue, object statChangeData)
    {
      MyEntityStat health = this.Health;
      if (health == null || (double) health.Value - (double) health.MinValue >= 1.0 / 1000.0 || this.Character == null)
        return;
      this.Character.Kill(statChangeData);
    }

    private void CreateRegenEffect(bool removeWhenAtMaxRegen = true)
    {
      MyObjectBuilder_EntityStatRegenEffect objectBuilder = new MyObjectBuilder_EntityStatRegenEffect();
      MyEntityStat health = this.Health;
      if (health == null)
        return;
      objectBuilder.TickAmount = MyEffectConstants.HealthTick;
      objectBuilder.Interval = MyEffectConstants.HealthInterval;
      objectBuilder.MaxRegenRatio = MySpaceStatEffect.MAX_REGEN_HEALTH_RATIO;
      objectBuilder.MinRegenRatio = 0.0f;
      objectBuilder.RemoveWhenReachedMaxRegenRatio = removeWhenAtMaxRegen;
      this.m_healthEffectId = health.AddEffect(objectBuilder);
      this.m_effectCreated = true;
    }

    private void ClearRegenEffect()
    {
      MyEntityStat health = this.Health;
      if (health == null)
        return;
      health.RemoveEffect(this.m_healthEffectId);
      this.m_effectCreated = false;
    }

    private void InitActions()
    {
      MyStatLogic.MyStatAction action1 = new MyStatLogic.MyStatAction();
      string actionId1 = "MedRoomHeal";
      action1.StatId = MySpaceStatEffect.HealthId;
      action1.Cost = MyEffectConstants.MedRoomHeal;
      this.AddAction(actionId1, action1);
      MyStatLogic.MyStatAction action2 = new MyStatLogic.MyStatAction();
      string actionId2 = "GenericHeal";
      action2.StatId = MySpaceStatEffect.HealthId;
      action2.Cost = MyEffectConstants.GenericHeal;
      this.AddAction(actionId2, action2);
    }
  }
}
