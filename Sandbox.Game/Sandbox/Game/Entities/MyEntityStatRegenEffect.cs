// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityStatRegenEffect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyEntityStatEffectType(typeof (MyObjectBuilder_EntityStatRegenEffect))]
  public class MyEntityStatRegenEffect
  {
    protected float m_amount;
    protected float m_interval;
    protected float m_maxRegenRatio;
    protected float m_minRegenRatio;
    protected float m_duration;
    protected int m_lastRegenTime;
    private readonly int m_birthTime;
    private bool m_enabled;
    public bool RemoveWhenReachedMaxRegenRatio;
    private MyEntityStat m_parentStat;

    public float Amount
    {
      get => this.m_amount;
      set => this.m_amount = value;
    }

    public float AmountLeftOverDuration => this.m_amount * (float) this.TicksLeft + this.PartialEndAmount;

    public int TicksLeft => this.CalculateTicksBetweenTimes(this.m_lastRegenTime, this.DeathTime);

    private float PartialEndAmount
    {
      get
      {
        double num1;
        double num2 = Math.Truncate(num1 = (double) this.m_duration / (double) this.m_interval);
        return (float) (num1 - num2) * this.m_amount;
      }
    }

    public float Interval
    {
      get => this.m_interval;
      set => this.m_interval = value;
    }

    public float Duration => this.m_duration;

    public int LastRegenTime => this.m_lastRegenTime;

    public int BirthTime => this.m_birthTime;

    public int DeathTime => (double) this.Duration < 0.0 ? int.MaxValue : this.m_birthTime + (int) ((double) this.m_duration * 1000.0);

    public int AliveTime => MySandboxGame.TotalGamePlayTimeInMilliseconds - this.BirthTime;

    public bool Enabled
    {
      get => this.m_enabled;
      set => this.m_enabled = value;
    }

    public MyEntityStatRegenEffect()
    {
      this.m_lastRegenTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_birthTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.Enabled = true;
    }

    public virtual void Init(MyObjectBuilder_Base objectBuilder, MyEntityStat parentStat)
    {
      this.m_parentStat = parentStat;
      if (!(objectBuilder is MyObjectBuilder_EntityStatRegenEffect entityStatRegenEffect) || (double) entityStatRegenEffect.Interval <= 0.0)
        return;
      this.m_amount = entityStatRegenEffect.TickAmount;
      this.m_interval = entityStatRegenEffect.Interval;
      this.m_maxRegenRatio = entityStatRegenEffect.MaxRegenRatio;
      this.m_minRegenRatio = entityStatRegenEffect.MinRegenRatio;
      this.RemoveWhenReachedMaxRegenRatio = entityStatRegenEffect.RemoveWhenReachedMaxRegenRatio;
      this.m_duration = entityStatRegenEffect.Duration - entityStatRegenEffect.AliveTime / 1000f;
      this.ResetRegenTime();
    }

    public virtual MyObjectBuilder_EntityStatRegenEffect GetObjectBuilder() => new MyObjectBuilder_EntityStatRegenEffect()
    {
      TickAmount = this.m_amount,
      Interval = this.m_interval,
      MaxRegenRatio = this.m_maxRegenRatio,
      MinRegenRatio = this.m_minRegenRatio,
      Duration = this.m_duration,
      AliveTime = (float) this.AliveTime,
      RemoveWhenReachedMaxRegenRatio = this.RemoveWhenReachedMaxRegenRatio
    };

    public virtual void Closing()
    {
      if (!Sync.IsServer)
        return;
      this.IncreaseByRemainingValue();
    }

    public virtual void Update(float regenAmountMultiplier = 1f)
    {
      if ((double) this.m_interval <= 0.0)
        return;
      for (bool flag = (double) this.m_duration == 0.0; MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastRegenTime >= 0 | flag; flag = false)
      {
        if ((double) this.m_amount > 0.0 && (double) this.m_parentStat.Value < (double) this.m_parentStat.MaxValue * (double) this.m_maxRegenRatio)
          this.m_parentStat.Value = MathHelper.Clamp(this.m_parentStat.Value + this.m_parentStat.MaxValue * this.m_amount * regenAmountMultiplier, this.m_parentStat.Value, this.m_parentStat.MaxValue * this.m_maxRegenRatio);
        else if ((double) this.m_amount < 0.0 && (double) this.m_parentStat.Value > (double) Math.Max(this.m_parentStat.MinValue, this.m_parentStat.MaxValue * this.m_minRegenRatio))
          this.m_parentStat.Value = MathHelper.Clamp(this.m_parentStat.Value + this.m_parentStat.MaxValue * this.m_amount, Math.Max(this.m_parentStat.MaxValue * this.m_minRegenRatio, this.m_parentStat.MinValue), this.m_parentStat.Value);
        this.m_lastRegenTime += (int) Math.Round((double) this.m_interval * 1000.0);
      }
    }

    public int CalculateTicksBetweenTimes(int startTime, int endTime)
    {
      if (startTime < this.m_birthTime || startTime >= endTime)
        return 0;
      startTime = Math.Max(startTime, this.m_lastRegenTime);
      endTime = Math.Min(endTime, this.DeathTime);
      return Math.Max((int) ((double) (endTime - startTime) / Math.Round((double) this.m_interval * 1000.0)), 0);
    }

    public void SetAmountAndInterval(float amount, float interval, bool increaseByRemaining)
    {
      if ((double) amount == (double) this.Amount && (double) interval == (double) this.Interval)
        return;
      if (increaseByRemaining)
        this.IncreaseByRemainingValue();
      this.Amount = amount;
      this.Interval = interval;
      this.ResetRegenTime();
    }

    public void ResetRegenTime() => this.m_lastRegenTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + (int) Math.Round((double) this.m_interval * 1000.0);

    private void IncreaseByRemainingValue()
    {
      if ((double) this.m_interval <= 0.0 || !this.Enabled)
        return;
      float num = (float) (1.0 - (double) (this.m_lastRegenTime - MySandboxGame.TotalGamePlayTimeInMilliseconds) / ((double) this.m_interval * 1000.0));
      if ((double) num <= 0.0)
        return;
      if ((double) this.m_amount > 0.0 && (double) this.m_parentStat.Value < (double) this.m_parentStat.MaxValue)
      {
        this.m_parentStat.Value = MathHelper.Clamp(this.m_parentStat.Value + this.m_amount * num, this.m_parentStat.MinValue, Math.Max(this.m_parentStat.MaxValue * this.m_maxRegenRatio, this.m_parentStat.MaxValue));
      }
      else
      {
        if ((double) this.m_amount >= 0.0 || (double) this.m_parentStat.Value <= (double) this.m_parentStat.MinValue)
          return;
        this.m_parentStat.Value = MathHelper.Clamp(this.m_parentStat.Value + this.m_amount * num, Math.Max(this.m_parentStat.MaxValue * this.m_minRegenRatio, this.m_parentStat.MinValue), this.m_parentStat.MaxValue);
      }
    }

    public override string ToString() => this.m_parentStat.ToString() + ": (" + (object) this.m_amount + "/" + (object) this.m_interval + "/" + (object) this.m_duration + ")";

    public float GetMaxRegenRatio() => this.m_maxRegenRatio;
  }
}
