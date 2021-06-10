// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyOxygenGeneratorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_OxygenGeneratorDefinition), null)]
  public class MyOxygenGeneratorDefinition : MyProductionBlockDefinition
  {
    public static readonly MyDefinitionId OxygenGasId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
    public float IceConsumptionPerSecond;
    public MySoundPair GenerateSound;
    public MySoundPair IdleSound;
    public MyStringHash ResourceSourceGroup;
    public List<MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo> ProducedGases;

    public bool IsOxygenOnly { get; private set; }

    public float InventoryFillFactorMin { get; set; }

    public float InventoryFillFactorMax { get; set; }

    public float FuelPullAmountFromConveyorInMinutes { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_OxygenGeneratorDefinition generatorDefinition = builder as MyObjectBuilder_OxygenGeneratorDefinition;
      this.IceConsumptionPerSecond = generatorDefinition.IceConsumptionPerSecond;
      this.InventoryFillFactorMin = generatorDefinition.InventoryFillFactorMin;
      this.InventoryFillFactorMax = generatorDefinition.InventoryFillFactorMax;
      this.FuelPullAmountFromConveyorInMinutes = generatorDefinition.FuelPullAmountFromConveyorInMinutes;
      this.GenerateSound = new MySoundPair(generatorDefinition.GenerateSound);
      this.IdleSound = new MySoundPair(generatorDefinition.IdleSound);
      this.ResourceSourceGroup = MyStringHash.GetOrCompute(generatorDefinition.ResourceSourceGroup);
      this.ProducedGases = (List<MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo>) null;
      if (generatorDefinition.ProducedGases == null)
        return;
      this.IsOxygenOnly = generatorDefinition.ProducedGases.Count > 0;
      this.ProducedGases = new List<MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo>(generatorDefinition.ProducedGases.Count);
      foreach (MyObjectBuilder_GasGeneratorResourceInfo producedGase in generatorDefinition.ProducedGases)
      {
        this.ProducedGases.Add(new MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo()
        {
          Id = (MyDefinitionId) producedGase.Id,
          IceToGasRatio = producedGase.IceToGasRatio
        });
        this.IsOxygenOnly &= (MyDefinitionId) producedGase.Id == MyOxygenGeneratorDefinition.OxygenGasId;
      }
    }

    public struct MyGasGeneratorResourceInfo
    {
      public MyDefinitionId Id;
      public float IceToGasRatio;
    }

    private class Sandbox_Definitions_MyOxygenGeneratorDefinition\u003C\u003EActor : IActivator, IActivator<MyOxygenGeneratorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyOxygenGeneratorDefinition();

      MyOxygenGeneratorDefinition IActivator<MyOxygenGeneratorDefinition>.CreateInstance() => new MyOxygenGeneratorDefinition();
    }
  }
}
