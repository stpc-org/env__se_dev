// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyThrustDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ThrustDefinition), null)]
  public class MyThrustDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public MyStringHash ThrusterType;
    public MyFuelConverterInfo FuelConverter;
    public float SlowdownFactor;
    public float ForceMagnitude;
    public float MaxPowerConsumption;
    public float MinPowerConsumption;
    public float FlameDamageLengthScale;
    public float FlameDamage;
    public float FlameLengthScale;
    public Vector4 FlameFullColor;
    public Vector4 FlameIdleColor;
    public string FlamePointMaterial;
    public string FlameLengthMaterial;
    public string FlameFlare;
    public float FlameVisibilityDistance;
    public float FlameGlareQuerySize;
    public float MinPlanetaryInfluence;
    public float MaxPlanetaryInfluence;
    public float InvDiffMinMaxPlanetaryInfluence;
    public float EffectivenessAtMaxInfluence;
    public float EffectivenessAtMinInfluence;
    public bool NeedsAtmosphereForInfluence;
    public float ConsumptionFactorPerG;
    public bool PropellerUse;
    public string PropellerEntity;
    public float PropellerFullSpeed;
    public float PropellerIdleSpeed;
    public float PropellerAcceleration;
    public float PropellerDeceleration;
    public float PropellerMaxDistance;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ThrustDefinition thrustDefinition = builder as MyObjectBuilder_ThrustDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(thrustDefinition.ResourceSinkGroup);
      this.FuelConverter = thrustDefinition.FuelConverter;
      this.SlowdownFactor = thrustDefinition.SlowdownFactor;
      this.ForceMagnitude = thrustDefinition.ForceMagnitude;
      this.ThrusterType = MyStringHash.GetOrCompute(thrustDefinition.ThrusterType);
      this.MaxPowerConsumption = thrustDefinition.MaxPowerConsumption;
      this.MinPowerConsumption = thrustDefinition.MinPowerConsumption;
      this.FlameDamageLengthScale = thrustDefinition.FlameDamageLengthScale;
      this.FlameDamage = thrustDefinition.FlameDamage;
      this.FlameLengthScale = thrustDefinition.FlameLengthScale;
      this.FlameFullColor = thrustDefinition.FlameFullColor;
      this.FlameIdleColor = thrustDefinition.FlameIdleColor;
      this.FlamePointMaterial = thrustDefinition.FlamePointMaterial;
      this.FlameLengthMaterial = thrustDefinition.FlameLengthMaterial;
      this.FlameFlare = thrustDefinition.FlameFlare;
      this.FlameVisibilityDistance = thrustDefinition.FlameVisibilityDistance;
      this.FlameGlareQuerySize = thrustDefinition.FlameGlareQuerySize;
      this.MinPlanetaryInfluence = thrustDefinition.MinPlanetaryInfluence;
      this.MaxPlanetaryInfluence = thrustDefinition.MaxPlanetaryInfluence;
      this.EffectivenessAtMinInfluence = thrustDefinition.EffectivenessAtMinInfluence;
      this.EffectivenessAtMaxInfluence = thrustDefinition.EffectivenessAtMaxInfluence;
      this.NeedsAtmosphereForInfluence = thrustDefinition.NeedsAtmosphereForInfluence;
      this.ConsumptionFactorPerG = thrustDefinition.ConsumptionFactorPerG;
      this.PropellerUse = thrustDefinition.PropellerUsesPropellerSystem;
      this.PropellerEntity = thrustDefinition.PropellerSubpartEntityName;
      this.PropellerFullSpeed = thrustDefinition.PropellerRoundsPerSecondOnFullSpeed;
      this.PropellerIdleSpeed = thrustDefinition.PropellerRoundsPerSecondOnIdleSpeed;
      this.PropellerAcceleration = thrustDefinition.PropellerAccelerationTime;
      this.PropellerDeceleration = thrustDefinition.PropellerDecelerationTime;
      this.PropellerMaxDistance = thrustDefinition.PropellerMaxVisibleDistance;
      this.InvDiffMinMaxPlanetaryInfluence = (float) (1.0 / ((double) this.MaxPlanetaryInfluence - (double) this.MinPlanetaryInfluence));
      if (this.InvDiffMinMaxPlanetaryInfluence.IsValid())
        return;
      this.InvDiffMinMaxPlanetaryInfluence = 0.0f;
    }

    private class Sandbox_Definitions_MyThrustDefinition\u003C\u003EActor : IActivator, IActivator<MyThrustDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyThrustDefinition();

      MyThrustDefinition IActivator<MyThrustDefinition>.CreateInstance() => new MyThrustDefinition();
    }
  }
}
