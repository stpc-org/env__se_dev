// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.ParticleEffects.MyCubeBlockParticleEffect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.ParticleEffects
{
  internal class MyCubeBlockParticleEffect
  {
    private string m_particleName = string.Empty;
    private bool m_canBeDeleted;
    private MyParticleEffect m_effect;
    private bool m_loop;
    private bool m_playedOnce;
    private bool m_playing;
    private float m_delay;
    private float m_timer;
    private float m_spawnTimeMin;
    private float m_spawnTimeMax;
    private float m_duration;
    private MyModelDummy m_originPoint;
    private MyEntity m_entity;

    public string ParticleName => this.m_particleName;

    public bool CanBeDeleted => this.m_canBeDeleted;

    public bool EffectIsRunning => this.m_effect != null;

    public MyCubeBlockParticleEffect(Sandbox.Definitions.CubeBlockEffect effectData, MyEntity entity)
    {
      this.m_particleName = effectData.Name;
      if (string.IsNullOrEmpty(this.m_particleName))
      {
        this.m_canBeDeleted = true;
      }
      else
      {
        this.m_loop = effectData.Loop;
        this.m_delay = effectData.Delay;
        this.m_spawnTimeMin = Math.Max(0.0f, effectData.SpawnTimeMin);
        this.m_spawnTimeMax = Math.Max(this.m_spawnTimeMin, effectData.SpawnTimeMax);
        this.m_timer = this.m_delay;
        this.m_entity = entity;
        this.m_originPoint = this.GetEffectOrigin(effectData.Origin);
        this.m_duration = effectData.Duration;
        if ((double) this.m_spawnTimeMax <= 0.0)
          return;
        this.m_timer += MyUtils.GetRandomFloat(this.m_spawnTimeMin, this.m_spawnTimeMax);
      }
    }

    private MyModelDummy GetEffectOrigin(string origin) => this.m_entity.Model.Dummies.ContainsKey(origin) ? this.m_entity.Model.Dummies[origin] : (MyModelDummy) null;

    public void Stop()
    {
      if (this.m_effect == null)
        return;
      this.m_effect.Stop();
      this.m_effect = (MyParticleEffect) null;
    }

    public void Update()
    {
      if (string.IsNullOrEmpty(this.m_particleName))
        return;
      if (this.m_effect == null || this.m_effect.IsStopped)
      {
        if (this.m_playedOnce && !this.m_loop)
          this.m_canBeDeleted = true;
        else if ((double) this.m_timer > 0.0)
        {
          this.m_timer -= 0.01666667f;
        }
        else
        {
          this.m_playedOnce = true;
          this.m_canBeDeleted = !MyParticlesManager.TryCreateParticleEffect(this.m_particleName, this.m_entity.WorldMatrix, out this.m_effect);
          if ((double) this.m_spawnTimeMax > 0.0)
            this.m_timer = MyUtils.GetRandomFloat(this.m_spawnTimeMin, this.m_spawnTimeMax);
          else
            this.m_timer = 0.0f;
        }
      }
      else
      {
        if (this.m_effect == null)
          return;
        float elapsedTime = this.m_effect.GetElapsedTime();
        if ((double) this.m_duration > 0.0 && (double) elapsedTime >= (double) this.m_duration)
          this.m_effect.Stop();
        else if (this.m_originPoint != null)
          this.m_effect.WorldMatrix = MatrixD.Multiply(MatrixD.Normalize((MatrixD) ref this.m_originPoint.Matrix), this.m_entity.WorldMatrix);
        else
          this.m_effect.WorldMatrix = this.m_entity.WorldMatrix;
      }
    }
  }
}
