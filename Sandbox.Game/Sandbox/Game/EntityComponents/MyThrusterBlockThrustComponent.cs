// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyThrusterBlockThrustComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  internal class MyThrusterBlockThrustComponent : MyEntityThrustComponent
  {
    private MyEntity m_relativeDampeningEntity;
    private bool m_scheduled;
    private float m_levitationPeriodLength = 1.3f;
    private float m_levitationTorqueCoeficient = 0.25f;

    private MyCubeGrid Entity => base.Entity as MyCubeGrid;

    private MyCubeGrid CubeGrid => this.Entity;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Entity.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.PositionCompOnPositionChanged);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      this.Entity.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.PositionCompOnPositionChanged);
      this.DeSchedule();
      base.OnBeforeRemovedFromContainer();
      foreach (MyEntityThrustComponent.FuelTypeData fuelTypeData in this.m_dataByFuelType)
      {
        foreach (HashSet<MyEntity> myEntitySet in fuelTypeData.ThrustsByDirection.Values)
        {
          foreach (MyEntity myEntity in myEntitySet)
          {
            if (!(myEntity is MyThrust myThrust))
              return;
            myThrust.SlimBlock.ComponentStack.IsFunctionalChanged -= new Action(this.ComponentStack_IsFunctionalChanged);
            myThrust.ThrustOverrideChanged -= new Action<MyThrust, float>(this.MyThrust_ThrustOverrideChanged);
            myThrust.EnabledChanged -= new Action<MyTerminalBlock>(this.thrust_EnabledChanged);
          }
        }
      }
    }

    private void PositionCompOnPositionChanged(MyPositionComponentBase obj)
    {
      if (!this.DampenersEnabled || this.m_scheduled || this.ThrustCount <= 0)
        return;
      this.Schedule();
    }

    private void Schedule()
    {
      if (this.m_scheduled || this.Entity == null)
        return;
      this.Entity.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateBeforeSimulation), 6, true);
      this.m_scheduled = true;
    }

    private void DeSchedule()
    {
      if (!this.m_scheduled || this.Entity == null)
        return;
      this.Entity.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateBeforeSimulation));
      this.m_scheduled = false;
    }

    protected override void OnControlTrustChanged() => this.MarkDirty(false);

    private void UpdateBeforeSimulation()
    {
      if (this.CubeGrid == null)
        return;
      if (this.CubeGrid.Physics != null && !Sync.IsServer && (double) this.CubeGrid.Physics.LinearVelocity.LengthSquared() < 9.99999974737875E-06 && (double) this.CubeGrid.Physics.LastLinearVelocity.LengthSquared() >= 9.99999974737875E-06)
        this.MarkDirty(false);
      this.UpdateBeforeSimulation(true, this.m_relativeDampeningEntity);
    }

    public override void UpdateBeforeSimulation(
      bool updateDampeners,
      MyEntity relativeDampeningEntity)
    {
      base.UpdateBeforeSimulation(updateDampeners, relativeDampeningEntity);
      if (!(this.FinalThrust == Vector3.Zero) || this.m_relativeDampeningEntity != null || this.Container == null)
        return;
      this.DeSchedule();
    }

    protected override void UpdateThrusts(bool enableDampeners, Vector3 dampeningVelocity)
    {
      base.UpdateThrusts(enableDampeners, dampeningVelocity);
      MyCubeGrid cubeGrid = this.CubeGrid;
      if (cubeGrid == null)
        return;
      MyGridPhysics physics1 = cubeGrid.Physics;
      if (physics1 == null || physics1.IsStatic || !physics1.Enabled)
        return;
      Vector3 normal = this.FinalThrust;
      if ((double) normal.LengthSquared() <= 9.99999974737875E-05)
        return;
      if (physics1.IsWelded)
      {
        normal = Vector3.TransformNormal(normal, this.CubeGrid.WorldMatrix);
        normal = Vector3.TransformNormal(normal, Matrix.Invert(this.CubeGrid.Physics.RigidBody.GetRigidBodyMatrix()));
      }
      MyGridPhysicalGroupData.GroupSharedPxProperties sharedProperties = MyGridPhysicalGroupData.GetGroupSharedProperties(cubeGrid);
      float? maxSpeed = new float?();
      if (sharedProperties.GridCount == 1)
      {
        maxSpeed = new float?(MyGridPhysics.GetShipMaxLinearVelocity(cubeGrid.GridSizeEnum));
      }
      else
      {
        MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(cubeGrid);
        MyGridPhysics physics2 = root.Physics;
        if (physics2 != null && !physics2.IsStatic)
        {
          Vector3D vector3D1 = Vector3D.TransformNormal(normal, cubeGrid.WorldMatrix);
          Vector3 linearVelocity = physics2.LinearVelocity;
          Vector3D vector3D2 = linearVelocity + vector3D1 * 0.0166666675359011 / (double) sharedProperties.Mass;
          float maxLinearVelocity = MyGridPhysics.GetShipMaxLinearVelocity(root.GridSizeEnum);
          if (vector3D2.LengthSquared() > (double) maxLinearVelocity * (double) maxLinearVelocity)
          {
            float num = Vector3.Dot((Vector3) vector3D1, linearVelocity) / linearVelocity.LengthSquared();
            if ((double) num > 0.0)
            {
              Vector3 vector3 = num * linearVelocity;
              normal = (Vector3) Vector3D.TransformNormal(vector3D1 - vector3, cubeGrid.PositionComp.WorldMatrixNormalizedInv);
            }
          }
        }
      }
      Vector3D vector3D = sharedProperties.ReferenceGrid != cubeGrid ? Vector3D.Transform(sharedProperties.CoMWorld, cubeGrid.PositionComp.WorldMatrixNormalizedInv) : (Vector3D) sharedProperties.PxProperties.CenterOfMass;
      physics1.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, new Vector3?(normal), new Vector3D?(vector3D), new Vector3?(), maxSpeed, false, true);
    }

    public override void Register(
      MyEntity entity,
      Vector3I forwardVector,
      Func<bool> onRegisteredCallback)
    {
      if (!(entity is MyThrust))
        return;
      this.m_thrustEntitiesPending.Enqueue(new MyTuple<MyEntity, Vector3I, Func<bool>>(entity, forwardVector, onRegisteredCallback));
      this.Schedule();
    }

    protected override bool RegisterLazy(
      MyEntity entity,
      Vector3I forwardVector,
      Func<bool> onRegisteredCallback)
    {
      base.RegisterLazy(entity, forwardVector, onRegisteredCallback);
      base.Register(entity, forwardVector, onRegisteredCallback);
      MyThrust myThrust = entity as MyThrust;
      MyDefinitionId fuelType = this.FuelType(entity);
      this.m_lastFuelTypeData.EnergyDensity = myThrust.FuelDefinition.EnergyDensity;
      this.m_lastFuelTypeData.Efficiency = myThrust.BlockDefinition.FuelConverter.Efficiency;
      this.m_lastSink.SetMaxRequiredInputByType(fuelType, this.m_lastSink.MaxRequiredInputByType(fuelType) + this.PowerAmountToFuel(ref fuelType, myThrust.MaxPowerConsumption, this.m_lastGroup));
      myThrust.EnabledChanged += new Action<MyTerminalBlock>(this.thrust_EnabledChanged);
      myThrust.ThrustOverrideChanged += new Action<MyThrust, float>(this.MyThrust_ThrustOverrideChanged);
      myThrust.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.SlowdownFactor = Math.Max(myThrust.BlockDefinition.SlowdownFactor, this.SlowdownFactor);
      if (onRegisteredCallback != null)
      {
        int num = onRegisteredCallback() ? 1 : 0;
      }
      return true;
    }

    public override void Unregister(MyEntity entity, Vector3I forwardVector)
    {
      base.Unregister(entity, forwardVector);
      if (!(entity is MyThrust myThrust))
        return;
      myThrust.SlimBlock.ComponentStack.IsFunctionalChanged -= new Action(this.ComponentStack_IsFunctionalChanged);
      myThrust.ThrustOverrideChanged -= new Action<MyThrust, float>(this.MyThrust_ThrustOverrideChanged);
      myThrust.EnabledChanged -= new Action<MyTerminalBlock>(this.thrust_EnabledChanged);
      this.SlowdownFactor = 0.0f;
      foreach (Vector3I intDirection in Base6Directions.IntDirections)
      {
        foreach (MyEntityThrustComponent.FuelTypeData fuelTypeData in this.m_dataByFuelType)
        {
          foreach (MyEntity myEntity in fuelTypeData.ThrustsByDirection[intDirection])
          {
            if (myEntity is MyThrust myThrust)
              this.SlowdownFactor = Math.Max(myThrust.BlockDefinition.SlowdownFactor, this.SlowdownFactor);
          }
        }
        foreach (MyEntityThrustComponent.MyConveyorConnectedGroup connectedGroup in this.ConnectedGroups)
        {
          foreach (MyEntityThrustComponent.FuelTypeData fuelTypeData in connectedGroup.DataByFuelType)
          {
            foreach (MyEntity myEntity in fuelTypeData.ThrustsByDirection[intDirection])
            {
              if (myEntity is MyThrust myThrust)
                this.SlowdownFactor = Math.Max(myThrust.BlockDefinition.SlowdownFactor, this.SlowdownFactor);
            }
          }
        }
      }
    }

    protected override void UpdateThrustStrength(HashSet<MyEntity> thrusters, float thrustForce)
    {
      foreach (MyEntity thruster in thrusters)
      {
        if (thruster is MyThrust thrust)
        {
          if ((double) thrustForce == 0.0 && !MyThrusterBlockThrustComponent.IsOverridden(thrust))
          {
            thrust.CurrentStrength = 0.0f;
          }
          else
          {
            float forceMultiplier = this.CalculateForceMultiplier((MyEntity) thrust, this.m_lastPlanetaryInfluence, this.m_lastPlanetaryInfluenceHasAtmosphere);
            MyResourceSinkComponent resourceSinkComponent = this.ResourceSink((MyEntity) thrust);
            thrust.CurrentStrength = !MyThrusterBlockThrustComponent.IsOverridden(thrust) ? (!this.IsUsed((MyEntity) thrust) ? 0.0f : forceMultiplier * thrustForce * resourceSinkComponent.SuppliedRatioByType(thrust.FuelDefinition.Id)) : (!MySession.Static.CreativeMode || !thrust.IsWorking ? forceMultiplier * thrust.ThrustOverride * resourceSinkComponent.SuppliedRatioByType(thrust.FuelDefinition.Id) / thrust.ThrustForce.Length() : forceMultiplier * thrust.ThrustOverrideOverForceLen);
          }
        }
      }
    }

    private void MyThrust_ThrustOverrideChanged(MyThrust block, float newValue) => this.MarkDirty(false);

    private void thrust_EnabledChanged(MyTerminalBlock obj)
    {
      if (this.CubeGrid == null)
        return;
      this.MarkDirty(false);
      if (this.CubeGrid.Physics == null || this.CubeGrid.Physics.RigidBody.IsActive)
        return;
      this.CubeGrid.ActivatePhysics();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      if (this.CubeGrid == null)
        return;
      this.MarkDirty(false);
      if (this.CubeGrid.Physics == null || this.CubeGrid.Physics.RigidBody.IsActive)
        return;
      this.CubeGrid.ActivatePhysics();
    }

    private static bool IsOverridden(MyThrust thrust)
    {
      MyEntityThrustComponent component;
      return thrust != null && thrust.IsOverridden && (thrust.Enabled && thrust.IsFunctional) && (!thrust.CubeGrid.Components.TryGet<MyEntityThrustComponent>(out component) || !component.AutopilotEnabled);
    }

    protected override bool RecomputeOverriddenParameters(
      MyEntity thrustEntity,
      MyEntityThrustComponent.FuelTypeData fuelData)
    {
      if (!(thrustEntity is MyThrust thrust) || !MyThrusterBlockThrustComponent.IsOverridden(thrust))
        return false;
      Vector3 vector3 = thrust.ThrustOverride * -thrust.ThrustForwardVector * this.CalculateForceMultiplier(thrustEntity, this.m_lastPlanetaryInfluence, this.m_lastPlanetaryInfluenceHasAtmosphere);
      float num = thrust.ThrustOverride / thrust.ThrustForce.Length() * thrust.MaxPowerConsumption;
      if (fuelData.ThrustsByDirection[thrust.ThrustForwardVector].Contains(thrustEntity))
      {
        fuelData.ThrustOverride += vector3;
        fuelData.ThrustOverridePower += num;
      }
      return true;
    }

    protected override bool IsUsed(MyEntity thrustEntity) => thrustEntity is MyThrust myThrust && myThrust.Enabled && myThrust.IsFunctional && !myThrust.IsOverridden;

    protected override float CalculateForceMultiplier(
      MyEntity thrustEntity,
      float planetaryInfluence,
      bool inAtmosphere)
    {
      MyThrust myThrust = thrustEntity as MyThrust;
      float num1 = 1f;
      MyThrustDefinition blockDefinition = myThrust.BlockDefinition;
      if (blockDefinition.NeedsAtmosphereForInfluence && !inAtmosphere)
        num1 = blockDefinition.EffectivenessAtMinInfluence;
      else if ((double) blockDefinition.MaxPlanetaryInfluence != (double) blockDefinition.MinPlanetaryInfluence)
      {
        float num2 = (planetaryInfluence - blockDefinition.MinPlanetaryInfluence) * blockDefinition.InvDiffMinMaxPlanetaryInfluence;
        num1 = MathHelper.Lerp(blockDefinition.EffectivenessAtMinInfluence, blockDefinition.EffectivenessAtMaxInfluence, MathHelper.Clamp(num2, 0.0f, 1f));
      }
      return num1;
    }

    protected override float CalculateConsumptionMultiplier(
      MyEntity thrustEntity,
      float naturalGravityStrength)
    {
      return !(thrustEntity is MyThrust myThrust) ? 1f : (float) (1.0 + (double) myThrust.BlockDefinition.ConsumptionFactorPerG * ((double) naturalGravityStrength / 9.8100004196167));
    }

    protected override float ForceMagnitude(
      MyEntity thrustEntity,
      float planetaryInfluence,
      bool inAtmosphere)
    {
      if (!(thrustEntity is MyThrust myThrust))
        return 0.0f;
      float num = thrustEntity is IMyThrust ? (thrustEntity as IMyThrust).ThrustMultiplier : 1f;
      return myThrust.BlockDefinition.ForceMagnitude * num * this.CalculateForceMultiplier((MyEntity) myThrust, planetaryInfluence, inAtmosphere);
    }

    protected override float MaxPowerConsumption(MyEntity thrustEntity) => (thrustEntity as MyThrust).MaxPowerConsumption;

    protected override float MinPowerConsumption(MyEntity thrustEntity) => (thrustEntity as MyThrust).MinPowerConsumption;

    protected override MyDefinitionId FuelType(MyEntity thrustEntity)
    {
      MyThrust myThrust = thrustEntity as MyThrust;
      return myThrust.FuelDefinition == null ? MyResourceDistributorComponent.ElectricityId : myThrust.FuelDefinition.Id;
    }

    protected override bool IsThrustEntityType(MyEntity thrustEntity) => thrustEntity is MyThrust;

    protected override void AddToGroup(
      MyEntity thrustEntity,
      MyEntityThrustComponent.MyConveyorConnectedGroup group)
    {
      if (!(thrustEntity is MyThrust myThrust))
        return;
      group.ResourceSink.IsPoweredChanged += new Action(myThrust.Sink_IsPoweredChanged);
    }

    protected override void RemoveFromGroup(
      MyEntity thrustEntity,
      MyEntityThrustComponent.MyConveyorConnectedGroup group)
    {
      if (!(thrustEntity is MyThrust myThrust))
        return;
      group.ResourceSink.IsPoweredChanged -= new Action(myThrust.Sink_IsPoweredChanged);
    }

    protected override float CalculateMass()
    {
      MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>.Group group = MyCubeGridGroups.Static.PhysicalDynamic.GetGroup(this.Entity);
      MyGridPhysics physics = this.Entity.Physics;
      float num1 = (HkReferenceObject) physics.WeldedRigidBody != (HkReferenceObject) null ? physics.WeldedRigidBody.Mass : this.GetGridMass(this.CubeGrid);
      MyCubeGrid myCubeGrid = (MyCubeGrid) null;
      float num2 = 0.0f;
      if (group != null)
      {
        float num3 = 0.0f;
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>.Node node in group.Nodes)
        {
          MyCubeGrid nodeData = node.NodeData;
          if (!nodeData.IsStatic && nodeData.Physics != null && !MyFixedGrids.IsRooted(nodeData))
          {
            MyEntityThrustComponent entityThrustComponent = nodeData.Components.Get<MyEntityThrustComponent>();
            if ((entityThrustComponent == null || !entityThrustComponent.Enabled ? 0 : (entityThrustComponent.HasPower ? 1 : 0)) == 0)
            {
              num2 += (HkReferenceObject) nodeData.Physics.WeldedRigidBody != (HkReferenceObject) null ? nodeData.Physics.WeldedRigidBody.Mass : this.GetGridMass(nodeData);
            }
            else
            {
              float radius = nodeData.PositionComp.LocalVolume.Radius;
              if ((double) radius > (double) num3 || (double) radius == (double) num3 && (myCubeGrid == null || nodeData.EntityId > myCubeGrid.EntityId))
              {
                num3 = radius;
                myCubeGrid = nodeData;
              }
            }
          }
        }
      }
      if (myCubeGrid == this.CubeGrid)
        num1 += num2;
      return num1;
    }

    private float GetGridMass(MyCubeGrid grid)
    {
      if (!Sync.IsServer)
      {
        if (MyFixedGrids.IsRooted(grid))
          return 0.0f;
        HkMassProperties? massProperties = grid.Physics.Shape.MassProperties;
        if (massProperties.HasValue)
          return massProperties.Value.Mass;
      }
      return grid.Physics.Mass;
    }

    public override void SetRelativeDampeningEntity(MyEntity entity)
    {
      if (entity != null)
      {
        this.Schedule();
      }
      else
      {
        MyGridPhysics physics = this.Entity.Physics;
        if ((physics != null ? (!physics.IsActive ? 1 : 0) : 1) != 0 && this.ControlThrust == Vector3.Zero && (this.m_totalThrustOverride == Vector3.Zero && !this.IsDirty))
          this.DeSchedule();
      }
      this.m_relativeDampeningEntity = entity;
    }

    public override void MarkDirty(bool recomputePlanetaryInfluence = false)
    {
      base.MarkDirty(recomputePlanetaryInfluence);
      this.Schedule();
    }

    private class Sandbox_Game_EntityComponents_MyThrusterBlockThrustComponent\u003C\u003EActor : IActivator, IActivator<MyThrusterBlockThrustComponent>
    {
      object IActivator.CreateInstance() => (object) new MyThrusterBlockThrustComponent();

      MyThrusterBlockThrustComponent IActivator<MyThrusterBlockThrustComponent>.CreateInstance() => new MyThrusterBlockThrustComponent();
    }
  }
}
