// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.GameLogic.MySharedWindComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.EntityComponents.GameLogic
{
  public class MySharedWindComponent : MyEntityComponentBase
  {
    private float m_windSpeed = -1f;
    private MyWindTurbine m_updatingTurbine;
    private readonly HashSet<MyWindTurbine> m_windTurbines = new HashSet<MyWindTurbine>();
    private bool m_updating;

    public MyCubeGrid Entity => (MyCubeGrid) base.Entity;

    public Vector3D GravityNormal { get; private set; }

    public float WindSpeedModifier { get; private set; }

    public float WindSpeed
    {
      get => (double) this.m_windSpeed >= 0.0 ? this.m_windSpeed : 0.0f;
      private set
      {
        if ((double) this.m_windSpeed == (double) value)
          return;
        if ((double) value == 0.0 != (double) this.m_windSpeed <= 0.0)
          this.UpdatingTurbine.NeedsUpdate ^= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_windSpeed = value;
        MyGridPhysics physics = this.Entity.Physics;
        this.GravityNormal = (Vector3D) Vector3.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(physics != null ? physics.CenterOfMassWorld : this.Entity.PositionComp.GetPosition()));
        foreach (MyWindTurbine windTurbine in this.m_windTurbines)
          windTurbine.OnEnvironmentChanged();
      }
    }

    public bool IsEnabled => (double) this.WindSpeed > 0.0;

    private MyWindTurbine UpdatingTurbine
    {
      get => this.m_updatingTurbine;
      set
      {
        if (this.m_updatingTurbine != null)
          this.m_updatingTurbine.NeedsUpdate &= ~(MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
        this.m_updatingTurbine = value;
        if (this.m_updatingTurbine == null)
          return;
        MyEntityUpdateEnum entityUpdateEnum = MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        if (this.IsEnabled)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_updatingTurbine.NeedsUpdate |= entityUpdateEnum;
      }
    }

    public void Register(MyWindTurbine windTurbine)
    {
      this.m_windTurbines.Add(windTurbine);
      if (this.UpdatingTurbine != null)
        return;
      this.UpdatingTurbine = windTurbine;
    }

    public void Unregister(MyWindTurbine windTurbine)
    {
      this.m_windTurbines.Remove(windTurbine);
      if (this.UpdatingTurbine != windTurbine)
        return;
      if (this.m_windTurbines.Count == 0)
      {
        this.UpdatingTurbine = (MyWindTurbine) null;
        this.Entity.Components.Remove(typeof (MySharedWindComponent), (MyComponentBase) this);
      }
      else
        this.UpdatingTurbine = this.m_windTurbines.FirstElement<MyWindTurbine>();
    }

    public void Update10()
    {
      this.m_updating = true;
      this.WindSpeedModifier = MySession.Static.GetComponent<MySectorWeatherComponent>().GetWindMultiplier(this.Entity.PositionComp.GetPosition());
      foreach (MyWindTurbine windTurbine in this.m_windTurbines)
        windTurbine.UpdateNextRay();
      this.m_updating = false;
    }

    public void UpdateWindSpeed() => this.WindSpeed = this.ComputeWindSpeed();

    private float ComputeWindSpeed()
    {
      MyCubeGrid entity = this.Entity;
      if (entity.IsPreview || entity.Physics == null || !MyFixedGrids.IsRooted(entity))
        return 0.0f;
      Vector3D centerOfMassWorld = entity.Physics.CenterOfMassWorld;
      MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(centerOfMassWorld);
      return closestPlanet == null || closestPlanet.PositionComp.WorldAABB.Contains(centerOfMassWorld) == ContainmentType.Disjoint ? 0.0f : closestPlanet.GetWindSpeed(centerOfMassWorld);
    }

    public override string ComponentTypeDebugString => this.GetType().Name;
  }
}
