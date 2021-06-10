// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyEnvironmentalParticles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation)]
  internal class MyEnvironmentalParticles : MySessionComponentBase
  {
    private List<MyEnvironmentalParticleLogic> m_particleHandlers = new List<MyEnvironmentalParticleLogic>();

    public override void LoadData()
    {
      base.LoadData();
      if (MySector.EnvironmentDefinition == null)
        return;
      foreach (MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings environmentalParticle in MySector.EnvironmentDefinition.EnvironmentalParticles)
      {
        if (MyObjectBuilderSerializer.CreateNewObject(environmentalParticle.Id) is MyObjectBuilder_EnvironmentalParticleLogic newObject)
        {
          newObject.Density = environmentalParticle.Density;
          newObject.DespawnDistance = environmentalParticle.DespawnDistance;
          newObject.ParticleColor = environmentalParticle.Color;
          newObject.ParticleColorPlanet = environmentalParticle.ColorPlanet;
          newObject.MaxSpawnDistance = environmentalParticle.MaxSpawnDistance;
          newObject.Material = environmentalParticle.Material;
          newObject.MaterialPlanet = environmentalParticle.MaterialPlanet;
          newObject.MaxLifeTime = environmentalParticle.MaxLifeTime;
          newObject.MaxParticles = environmentalParticle.MaxParticles;
          MyEnvironmentalParticleLogic environmentalParticleLogic = MyEnvironmentalParticleLogicFactory.CreateEnvironmentalParticleLogic(newObject);
          environmentalParticleLogic.Init(newObject);
          this.m_particleHandlers.Add(environmentalParticleLogic);
        }
      }
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (MyParticlesManager.Paused)
        return;
      foreach (MyEnvironmentalParticleLogic particleHandler in this.m_particleHandlers)
        particleHandler.UpdateBeforeSimulation();
    }

    public override void Simulate()
    {
      base.Simulate();
      if (MyParticlesManager.Paused)
        return;
      foreach (MyEnvironmentalParticleLogic particleHandler in this.m_particleHandlers)
        particleHandler.Simulate();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (MyParticlesManager.Paused)
        return;
      foreach (MyEnvironmentalParticleLogic particleHandler in this.m_particleHandlers)
        particleHandler.UpdateAfterSimulation();
    }

    public override void Draw()
    {
      base.Draw();
      foreach (MyEnvironmentalParticleLogic particleHandler in this.m_particleHandlers)
        particleHandler.Draw();
    }
  }
}
