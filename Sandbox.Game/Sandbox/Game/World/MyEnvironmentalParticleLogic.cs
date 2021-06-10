// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyEnvironmentalParticleLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
  [MyEnvironmentalParticleLogicType(typeof (MyObjectBuilder_EnvironmentalParticleLogic), true)]
  public class MyEnvironmentalParticleLogic
  {
    protected float m_particleDensity;
    protected float m_particleSpawnDistance;
    protected float m_particleDespawnDistance;
    private int m_maxParticles = 128;
    protected List<MyEnvironmentalParticleLogic.MyEnvironmentalParticle> m_nonActiveParticles;
    protected List<MyEnvironmentalParticleLogic.MyEnvironmentalParticle> m_activeParticles;
    protected List<int> m_particlesToRemove = new List<int>();

    protected float ParticleDensity => this.m_particleDensity;

    protected float ParticleSpawnDistance => this.m_particleSpawnDistance;

    protected float ParticleDespawnDistance => this.m_particleDespawnDistance;

    public virtual void Init(MyObjectBuilder_EnvironmentalParticleLogic builder)
    {
      this.m_particleDensity = builder.Density;
      this.m_particleSpawnDistance = builder.MaxSpawnDistance;
      this.m_particleDespawnDistance = builder.DespawnDistance;
      this.m_maxParticles = builder.MaxParticles;
      this.m_nonActiveParticles = new List<MyEnvironmentalParticleLogic.MyEnvironmentalParticle>(this.m_maxParticles);
      this.m_activeParticles = new List<MyEnvironmentalParticleLogic.MyEnvironmentalParticle>(this.m_maxParticles);
      string materialPlanet = builder.Material;
      Vector4 colorPlanet = builder.ParticleColor;
      if (builder is MyObjectBuilder_EnvironmentalParticleLogicSpace particleLogicSpace)
      {
        materialPlanet = particleLogicSpace.MaterialPlanet;
        colorPlanet = particleLogicSpace.ParticleColorPlanet;
      }
      for (int index = 0; index < this.m_maxParticles; ++index)
        this.m_nonActiveParticles.Add(new MyEnvironmentalParticleLogic.MyEnvironmentalParticle(builder.Material, materialPlanet, builder.ParticleColor, colorPlanet, builder.MaxLifeTime));
    }

    public virtual void UpdateBeforeSimulation()
    {
    }

    public virtual void Simulate()
    {
    }

    public virtual void UpdateAfterSimulation()
    {
      for (int index = 0; index < this.m_activeParticles.Count; ++index)
      {
        MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle = this.m_activeParticles[index];
        if (MySandboxGame.TotalGamePlayTimeInMilliseconds - activeParticle.BirthTime >= activeParticle.LifeTime || ((activeParticle.Position - MySector.MainCamera.Position).Length() > (double) this.m_particleDespawnDistance || !activeParticle.Active))
          this.m_particlesToRemove.Add(index);
      }
      for (int index1 = this.m_particlesToRemove.Count - 1; index1 >= 0; --index1)
      {
        int index2 = this.m_particlesToRemove[index1];
        this.m_nonActiveParticles.Add(this.m_activeParticles[index2]);
        this.m_activeParticles[index2].Deactivate();
        this.m_activeParticles.RemoveAt(index2);
      }
      this.m_particlesToRemove.Clear();
    }

    public virtual void Draw()
    {
    }

    protected MyEnvironmentalParticleLogic.MyEnvironmentalParticle Spawn(
      Vector3 position)
    {
      int count = this.m_nonActiveParticles.Count;
      if (count <= 0)
        return (MyEnvironmentalParticleLogic.MyEnvironmentalParticle) null;
      MyEnvironmentalParticleLogic.MyEnvironmentalParticle nonActiveParticle = this.m_nonActiveParticles[count - 1];
      this.m_activeParticles.Add(nonActiveParticle);
      this.m_nonActiveParticles.RemoveAtFast<MyEnvironmentalParticleLogic.MyEnvironmentalParticle>(count - 1);
      nonActiveParticle.Activate(position);
      return nonActiveParticle;
    }

    protected bool Despawn(
      MyEnvironmentalParticleLogic.MyEnvironmentalParticle particle)
    {
      if (particle == null)
        return false;
      foreach (MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle in this.m_activeParticles)
      {
        if (particle == activeParticle)
        {
          this.m_activeParticles.Remove(particle);
          particle.Deactivate();
          this.m_nonActiveParticles.Add(particle);
          return true;
        }
      }
      return false;
    }

    protected void DeactivateAll()
    {
      foreach (MyEnvironmentalParticleLogic.MyEnvironmentalParticle activeParticle in this.m_activeParticles)
      {
        this.m_nonActiveParticles.Add(activeParticle);
        activeParticle.Deactivate();
      }
      this.m_activeParticles.Clear();
    }

    public class MyEnvironmentalParticle
    {
      private Vector3 m_position;
      private MyStringId m_material;
      private Vector4 m_color;
      private MyStringId m_materialPlanet;
      private Vector4 m_colorPlanet;
      private int m_birthTime;
      private int m_lifeTime;
      private bool m_active;
      public object UserData;

      public Vector3 Position
      {
        get => this.m_position;
        set => this.m_position = value;
      }

      public MyStringId Material => this.m_material;

      public Vector4 Color => this.m_color;

      public MyStringId MaterialPlanet => this.m_materialPlanet;

      public Vector4 ColorPlanet => this.m_colorPlanet;

      public int BirthTime => this.m_birthTime;

      public int LifeTime => this.m_lifeTime;

      public bool Active => this.m_active;

      public MyEnvironmentalParticle(
        string material,
        string materialPlanet,
        Vector4 color,
        Vector4 colorPlanet,
        int lifeTime)
      {
        this.m_birthTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.m_material = material != null ? MyStringId.GetOrCompute(material) : MyTransparentMaterials.ErrorMaterial.Id;
        this.m_materialPlanet = materialPlanet != null ? MyStringId.GetOrCompute(materialPlanet) : MyTransparentMaterials.ErrorMaterial.Id;
        this.m_color = color;
        this.m_colorPlanet = colorPlanet;
        this.m_position = new Vector3();
        this.m_lifeTime = lifeTime;
        this.Deactivate();
      }

      public void Activate(Vector3 position)
      {
        this.m_birthTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.m_position = position;
        this.m_active = true;
      }

      public void Deactivate() => this.m_active = false;
    }
  }
}
