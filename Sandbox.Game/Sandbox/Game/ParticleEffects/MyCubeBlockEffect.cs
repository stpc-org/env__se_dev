// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.ParticleEffects.MyCubeBlockEffect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System.Collections.Generic;
using VRage.Game.Entity;

namespace Sandbox.Game.ParticleEffects
{
  public class MyCubeBlockEffect
  {
    public readonly int EffectId;
    private CubeBlockEffectBase m_effectDefinition;
    public bool CanBeDeleted;
    private List<MyCubeBlockParticleEffect> m_particleEffects;
    private MyEntity m_entity;

    public MyCubeBlockEffect(int EffectId, CubeBlockEffectBase effectDefinition, MyEntity block)
    {
      this.EffectId = EffectId;
      this.m_entity = block;
      this.m_effectDefinition = effectDefinition;
      this.m_particleEffects = new List<MyCubeBlockParticleEffect>();
      if (this.m_effectDefinition.ParticleEffects == null)
        return;
      for (int index = 0; index < this.m_effectDefinition.ParticleEffects.Length; ++index)
        this.m_particleEffects.Add(new MyCubeBlockParticleEffect(this.m_effectDefinition.ParticleEffects[index], this.m_entity));
    }

    public void Stop()
    {
      for (int index = 0; index < this.m_particleEffects.Count; ++index)
        this.m_particleEffects[index].Stop();
      this.m_particleEffects.Clear();
    }

    public void Update()
    {
      for (int index = 0; index < this.m_particleEffects.Count; ++index)
      {
        if (this.m_particleEffects[index].CanBeDeleted)
        {
          this.m_particleEffects[index].Stop();
          this.m_particleEffects.RemoveAt(index);
          --index;
        }
        else
          this.m_particleEffects[index].Update();
      }
    }
  }
}
