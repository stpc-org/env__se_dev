// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyJetpackThrustComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  internal class MyJetpackThrustComponent : MyEntityThrustComponent
  {
    public MyCharacter Entity => base.Entity as MyCharacter;

    public MyCharacter Character => this.Entity;

    public MyCharacterJetpackComponent Jetpack => this.Character.JetpackComp;

    protected override void UpdateThrusts(bool enableDampers, Vector3 dampeningVelocity)
    {
      base.UpdateThrusts(enableDampers, dampeningVelocity);
      if (this.Character == null || this.Character.Physics == null || (this.Character.Physics.CharacterProxy == null || !this.Jetpack.TurnedOn))
        return;
      Vector3 vector3 = this.FinalThrust;
      if ((double) vector3.LengthSquared() > 9.99999974737875E-05 && this.Character.Physics.IsInWorld)
      {
        this.Character.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, new Vector3?(this.FinalThrust), new Vector3D?(), new Vector3?(), new float?(), true, false);
        Vector3 linearVelocityLocal = this.Character.Physics.LinearVelocityLocal;
        float num1 = Math.Max(this.Character.Definition.MaxSprintSpeed, Math.Max(this.Character.Definition.MaxRunSpeed, this.Character.Definition.MaxBackrunSpeed));
        float num2 = MyGridPhysics.ShipMaxLinearVelocity() + num1;
        if ((double) linearVelocityLocal.LengthSquared() > (double) num2 * (double) num2)
        {
          double num3 = (double) linearVelocityLocal.Normalize();
          linearVelocityLocal *= num2;
          this.Character.Physics.LinearVelocity = linearVelocityLocal;
        }
      }
      if (!this.Character.Physics.Enabled || !(this.Character.Physics.LinearVelocity != Vector3.Zero))
        return;
      vector3 = this.Character.Physics.LinearVelocity;
      if ((double) vector3.LengthSquared() >= 1.00000011116208E-06)
        return;
      this.Character.Physics.LinearVelocity = Vector3.Zero;
      this.ControlThrustChanged = true;
    }

    public override void Register(
      MyEntity entity,
      Vector3I forwardVector,
      Func<bool> onRegisteredCallback = null)
    {
      if (!(entity is MyCharacter myCharacter))
        return;
      base.Register(entity, forwardVector, onRegisteredCallback);
      MyDefinitionId fuelType = this.FuelType(entity);
      float num = 1f;
      if (MyFakes.ENABLE_HYDROGEN_FUEL)
        num = this.Jetpack.FuelConverterDefinition.Efficiency;
      this.m_lastFuelTypeData.Efficiency = num;
      this.m_lastFuelTypeData.EnergyDensity = this.Jetpack.FuelDefinition.EnergyDensity;
      this.m_lastSink.SetMaxRequiredInputByType(fuelType, this.m_lastSink.MaxRequiredInputByType(fuelType) + this.PowerAmountToFuel(ref fuelType, this.Jetpack.MaxPowerConsumption, this.m_lastGroup));
      this.SlowdownFactor = Math.Max(myCharacter.Definition.Jetpack.ThrustProperties.SlowdownFactor, this.SlowdownFactor);
    }

    protected override bool RecomputeOverriddenParameters(
      MyEntity thrustEntity,
      MyEntityThrustComponent.FuelTypeData fuelData)
    {
      return false;
    }

    protected override bool IsUsed(MyEntity thrustEntity) => this.Enabled;

    protected override float ForceMagnitude(
      MyEntity thrustEntity,
      float planetaryInfluence,
      bool inAtmosphere)
    {
      return this.Jetpack.ForceMagnitude * this.CalculateForceMultiplier(thrustEntity, planetaryInfluence, inAtmosphere);
    }

    protected override float CalculateForceMultiplier(
      MyEntity thrustEntity,
      float planetaryInfluence,
      bool inAtmosphere)
    {
      float num = 1f;
      if ((double) this.Jetpack.MaxPlanetaryInfluence != (double) this.Jetpack.MinPlanetaryInfluence && (inAtmosphere && this.Jetpack.NeedsAtmosphereForInfluence || !inAtmosphere))
        num = MathHelper.Lerp(this.Jetpack.EffectivenessAtMinInfluence, this.Jetpack.EffectivenessAtMaxInfluence, MathHelper.Clamp((float) (((double) planetaryInfluence - (double) this.Jetpack.MinPlanetaryInfluence) / ((double) this.Jetpack.MaxPlanetaryInfluence - (double) this.Jetpack.MinPlanetaryInfluence)), 0.0f, 1f));
      else if (this.Jetpack.NeedsAtmosphereForInfluence && !inAtmosphere)
        num = this.Jetpack.EffectivenessAtMinInfluence;
      return num;
    }

    protected override float CalculateConsumptionMultiplier(
      MyEntity thrustEntity,
      float naturalGravityStrength)
    {
      return (float) (1.0 + (double) this.Jetpack.ConsumptionFactorPerG * ((double) naturalGravityStrength / 9.8100004196167));
    }

    protected override float MaxPowerConsumption(MyEntity thrustEntity) => this.Jetpack.MaxPowerConsumption;

    protected override float MinPowerConsumption(MyEntity thrustEntity) => this.Jetpack.MinPowerConsumption;

    protected override void UpdateThrustStrength(HashSet<MyEntity> entities, float thrustForce) => this.ControlThrust = Vector3.Zero;

    protected override MyDefinitionId FuelType(MyEntity thrustEntity) => this.Jetpack == null || this.Jetpack.FuelDefinition == null ? MyResourceDistributorComponent.ElectricityId : this.Jetpack.FuelDefinition.Id;

    protected override bool IsThrustEntityType(MyEntity thrustEntity) => thrustEntity is MyCharacter;

    protected override void RemoveFromGroup(
      MyEntity thrustEntity,
      MyEntityThrustComponent.MyConveyorConnectedGroup group)
    {
    }

    protected override void AddToGroup(
      MyEntity thrustEntity,
      MyEntityThrustComponent.MyConveyorConnectedGroup group)
    {
    }

    protected override Vector3 ApplyThrustModifiers(
      ref MyDefinitionId fuelType,
      ref Vector3 thrust,
      ref Vector3 thrustOverride,
      MyResourceSinkComponentBase resourceSink)
    {
      thrust += thrustOverride;
      if ((this.Character.ControllerInfo.Controller == null || !MySession.Static.CreativeToolsEnabled(this.Character.ControllerInfo.Controller.Player.Id.SteamId)) && !MySession.Static.CreativeToolsEnabled(this.Character.ControlSteamId) || MySession.Static.LocalCharacter != this.Character && !Sync.IsServer)
        thrust *= resourceSink.SuppliedRatioByType(fuelType);
      thrust *= MyFakes.THRUST_FORCE_RATIO;
      return thrust;
    }

    private class Sandbox_Game_EntityComponents_MyJetpackThrustComponent\u003C\u003EActor : IActivator, IActivator<MyJetpackThrustComponent>
    {
      object IActivator.CreateInstance() => (object) new MyJetpackThrustComponent();

      MyJetpackThrustComponent IActivator<MyJetpackThrustComponent>.CreateInstance() => new MyJetpackThrustComponent();
    }
  }
}
