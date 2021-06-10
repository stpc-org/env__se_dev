// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityStat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Common;
using VRage.Game.ObjectBuilders;
using VRage.Library.Collections;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyFactoryTag(typeof (MyObjectBuilder_EntityStat), true)]
  public class MyEntityStat
  {
    protected float m_currentValue;
    private float m_lastSyncValue;
    protected float m_minValue;
    protected float m_maxValue;
    protected float m_defaultValue;
    private bool m_syncFlag;
    private Dictionary<int, MyEntityStatRegenEffect> m_effects = new Dictionary<int, MyEntityStatRegenEffect>();
    private int m_updateCounter;
    private float m_statRegenLeft;
    private float m_regenAmountMultiplier = 1f;
    private float m_regenAmountMultiplierDuration;
    private int m_regenAmountMultiplierTimeStart;
    private int m_regenAmountMultiplierTimeAlive;
    private bool m_regenAmountMultiplierActive;
    private MyStringHash m_statId;
    public MyEntityStatDefinition StatDefinition;

    public float Value
    {
      get => this.m_currentValue;
      set => this.SetValue(value, (object) null);
    }

    public float CurrentRatio => this.Value / (this.MaxValue - this.MinValue);

    public float MinValue => this.m_minValue;

    public float MaxValue => this.m_maxValue;

    public float DefaultValue => this.m_defaultValue;

    public bool ShouldSync => this.m_syncFlag;

    public float StatRegenLeft
    {
      get => this.m_statRegenLeft;
      set => this.m_statRegenLeft = value;
    }

    public MyStringHash StatId => this.m_statId;

    public event MyEntityStat.StatChangedDelegate OnStatChanged;

    public virtual void Init(MyObjectBuilder_Base objectBuilder)
    {
      MyObjectBuilder_EntityStat builderEntityStat = (MyObjectBuilder_EntityStat) objectBuilder;
      MyEntityStatDefinition definition;
      MyDefinitionManager.Static.TryGetDefinition<MyEntityStatDefinition>(new MyDefinitionId(builderEntityStat.TypeId, builderEntityStat.SubtypeId), out definition);
      this.StatDefinition = definition;
      this.m_maxValue = definition.MaxValue;
      this.m_minValue = definition.MinValue;
      this.m_currentValue = builderEntityStat.Value * this.m_maxValue;
      this.m_defaultValue = definition.DefaultValue;
      this.m_lastSyncValue = this.m_currentValue;
      this.m_statId = MyStringHash.GetOrCompute(definition.Name);
      this.m_regenAmountMultiplier = builderEntityStat.StatRegenAmountMultiplier;
      this.m_regenAmountMultiplierDuration = builderEntityStat.StatRegenAmountMultiplierDuration;
      this.m_regenAmountMultiplierTimeStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_regenAmountMultiplierTimeAlive = 0;
      this.m_regenAmountMultiplierActive = (double) this.m_regenAmountMultiplierDuration > 0.0;
      this.ClearEffects();
      if (builderEntityStat.Effects == null)
        return;
      foreach (MyObjectBuilder_EntityStatRegenEffect effect in builderEntityStat.Effects)
        this.AddEffect(effect);
    }

    public virtual MyObjectBuilder_EntityStat GetObjectBuilder()
    {
      MyObjectBuilder_EntityStat builderEntityStat = new MyObjectBuilder_EntityStat();
      MyEntityStatDefinition definition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId(builderEntityStat.TypeId, this.StatDefinition.Id.SubtypeId)) as MyEntityStatDefinition;
      builderEntityStat.SubtypeName = this.StatDefinition.Id.SubtypeName;
      if (definition != null)
      {
        builderEntityStat.Value = this.m_currentValue / ((double) definition.MaxValue != 0.0 ? definition.MaxValue : 1f);
        builderEntityStat.MaxValue = this.m_maxValue / ((double) definition.MaxValue != 0.0 ? definition.MaxValue : 1f);
      }
      else
      {
        builderEntityStat.Value = this.m_currentValue / this.m_maxValue;
        builderEntityStat.MaxValue = 1f;
      }
      if (this.m_regenAmountMultiplierActive)
      {
        builderEntityStat.StatRegenAmountMultiplier = this.m_regenAmountMultiplier;
        builderEntityStat.StatRegenAmountMultiplierDuration = this.m_regenAmountMultiplierDuration;
      }
      builderEntityStat.Effects = (MyObjectBuilder_EntityStatRegenEffect[]) null;
      if (this.m_effects != null && this.m_effects.Count > 0)
      {
        int count = this.m_effects.Count;
        foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in this.m_effects)
        {
          if ((double) effect.Value.Duration < 0.0)
            --count;
        }
        if (count > 0)
        {
          builderEntityStat.Effects = new MyObjectBuilder_EntityStatRegenEffect[count];
          int num = 0;
          foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in this.m_effects)
          {
            if ((double) effect.Value.Duration >= 0.0)
              builderEntityStat.Effects[num++] = effect.Value.GetObjectBuilder();
          }
        }
      }
      return builderEntityStat;
    }

    public void ApplyRegenAmountMultiplier(float amountMultiplier = 1f, float duration = 2f)
    {
      this.m_regenAmountMultiplier = amountMultiplier;
      this.m_regenAmountMultiplierDuration = duration;
      this.m_regenAmountMultiplierTimeStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_regenAmountMultiplierActive = (double) duration > 0.0;
    }

    public void ResetRegenAmountMultiplier()
    {
      this.m_regenAmountMultiplier = 1f;
      this.m_regenAmountMultiplierActive = false;
    }

    private void UpdateRegenAmountMultiplier()
    {
      if (!this.m_regenAmountMultiplierActive)
        return;
      this.m_regenAmountMultiplierTimeAlive = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_regenAmountMultiplierTimeStart;
      if ((double) this.m_regenAmountMultiplierTimeAlive < (double) this.m_regenAmountMultiplierDuration * 1000.0)
        return;
      this.m_regenAmountMultiplier = 1f;
      this.m_regenAmountMultiplierDuration = 0.0f;
      this.m_regenAmountMultiplierActive = false;
    }

    public float GetEfficiencyMultiplier(float multiplier, float threshold) => (double) this.CurrentRatio >= (double) threshold ? 1f : multiplier;

    public int AddEffect(
      float amount,
      float interval,
      float duration = -1f,
      float minRegenRatio = 0.0f,
      float maxRegenRatio = 1f)
    {
      return this.AddEffect(new MyObjectBuilder_EntityStatRegenEffect()
      {
        TickAmount = amount,
        Interval = interval,
        Duration = duration,
        MinRegenRatio = minRegenRatio,
        MaxRegenRatio = maxRegenRatio
      });
    }

    public int AddEffect(
      MyObjectBuilder_EntityStatRegenEffect objectBuilder)
    {
      MyEntityStatRegenEffect instance = MyEntityStatEffectFactory.CreateInstance(objectBuilder);
      instance.Init((MyObjectBuilder_Base) objectBuilder, this);
      int key = 0;
      while (key < this.m_effects.Count && this.m_effects.ContainsKey(key))
        ++key;
      this.m_effects.Add(key, instance);
      return key;
    }

    public bool HasAnyEffect() => this.m_effects.Count > 0;

    public virtual void Update()
    {
      this.m_syncFlag = false;
      this.UpdateRegenAmountMultiplier();
      List<int> poolObject = (List<int>) null;
      foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in this.m_effects)
      {
        MyEntityStatRegenEffect entityStatRegenEffect = effect.Value;
        if ((double) entityStatRegenEffect.Duration >= 0.0 && (double) entityStatRegenEffect.AliveTime >= (double) entityStatRegenEffect.Duration * 1000.0 || entityStatRegenEffect.RemoveWhenReachedMaxRegenRatio && (double) this.Value >= (double) entityStatRegenEffect.GetMaxRegenRatio() * 100.0)
        {
          if (poolObject == null)
            PoolManager.Get<List<int>>(out poolObject);
          poolObject.Add(effect.Key);
        }
        if (Sync.IsServer && entityStatRegenEffect.Enabled)
        {
          if (this.m_regenAmountMultiplierActive)
            entityStatRegenEffect.Update(this.m_regenAmountMultiplier);
          else
            entityStatRegenEffect.Update();
        }
      }
      if (poolObject != null)
      {
        foreach (int id in poolObject)
          this.RemoveEffect(id);
        PoolManager.Return<List<int>>(ref poolObject);
      }
      if (this.m_updateCounter++ % 10 != 0 && (double) Math.Abs(this.Value - this.MinValue) > 0.001 || (double) this.m_lastSyncValue == (double) this.m_currentValue)
        return;
      this.m_syncFlag = true;
      this.m_lastSyncValue = this.m_currentValue;
    }

    private void SetValue(float newValue, object statChangeData)
    {
      float currentValue = this.m_currentValue;
      this.m_currentValue = MathHelper.Clamp(newValue, this.MinValue, this.MaxValue);
      if (this.OnStatChanged == null || (double) newValue == (double) currentValue)
        return;
      this.OnStatChanged(newValue, currentValue, statChangeData);
    }

    public bool RemoveEffect(int id)
    {
      MyEntityStatRegenEffect entityStatRegenEffect = (MyEntityStatRegenEffect) null;
      if (this.m_effects.TryGetValue(id, out entityStatRegenEffect))
        entityStatRegenEffect.Closing();
      return this.m_effects.Remove(id);
    }

    public void ClearEffects()
    {
      foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in this.m_effects)
        effect.Value.Closing();
      this.m_effects.Clear();
    }

    public bool TryGetEffect(int id, out MyEntityStatRegenEffect outEffect) => this.m_effects.TryGetValue(id, out outEffect);

    public DictionaryReader<int, MyEntityStatRegenEffect> GetEffects() => (DictionaryReader<int, MyEntityStatRegenEffect>) this.m_effects;

    public MyEntityStatRegenEffect GetEffect(int id)
    {
      MyEntityStatRegenEffect entityStatRegenEffect = (MyEntityStatRegenEffect) null;
      return !this.m_effects.TryGetValue(id, out entityStatRegenEffect) ? (MyEntityStatRegenEffect) null : entityStatRegenEffect;
    }

    public override string ToString() => this.m_statId.ToString();

    public void Increase(float amount, object statChangeData) => this.SetValue(this.Value + amount, statChangeData);

    public void Decrease(float amount, object statChangeData) => this.SetValue(this.Value - amount, statChangeData);

    public float CalculateRegenLeftForLongestEffect()
    {
      MyEntityStatRegenEffect entityStatRegenEffect = (MyEntityStatRegenEffect) null;
      this.m_statRegenLeft = 0.0f;
      foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in this.m_effects)
      {
        if ((double) effect.Value.Duration > 0.0)
        {
          this.m_statRegenLeft += effect.Value.AmountLeftOverDuration;
          if (entityStatRegenEffect == null || effect.Value.DeathTime > entityStatRegenEffect.DeathTime)
            entityStatRegenEffect = effect.Value;
        }
      }
      if (entityStatRegenEffect == null)
        return this.m_statRegenLeft;
      foreach (KeyValuePair<int, MyEntityStatRegenEffect> effect in this.m_effects)
      {
        if ((double) effect.Value.Duration < 0.0)
          this.m_statRegenLeft += effect.Value.Amount * (float) effect.Value.CalculateTicksBetweenTimes(entityStatRegenEffect.LastRegenTime, entityStatRegenEffect.DeathTime);
      }
      return this.m_statRegenLeft;
    }

    public delegate void StatChangedDelegate(float newValue, float oldValue, object statChangeData);
  }
}
