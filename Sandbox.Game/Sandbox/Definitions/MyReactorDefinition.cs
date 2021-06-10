// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyReactorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game;
using Sandbox.Game.Localization;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ReactorDefinition), null)]
  public class MyReactorDefinition : MyFueledPowerProducerDefinition
  {
    public Vector3 InventorySize;
    public float InventoryMaxVolume;
    public MyInventoryConstraint InventoryConstraint;
    public MyReactorDefinition.FuelInfo[] FuelInfos;

    public float InventoryFillFactorMin { get; set; }

    public float InventoryFillFactorMax { get; set; }

    public float FuelPullAmountFromConveyorInMinutes { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ReactorDefinition reactorDefinition = builder as MyObjectBuilder_ReactorDefinition;
      this.InventorySize = reactorDefinition.InventorySize;
      this.InventoryMaxVolume = this.InventorySize.X * this.InventorySize.Y * this.InventorySize.Z;
      this.InventoryFillFactorMin = reactorDefinition.InventoryFillFactorMin;
      this.InventoryFillFactorMax = reactorDefinition.InventoryFillFactorMax;
      this.FuelPullAmountFromConveyorInMinutes = reactorDefinition.FuelPullAmountFromConveyorInMinutes;
      List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo> fuelInfoList = reactorDefinition.FuelInfos;
      if (reactorDefinition.FuelId.HasValue)
        fuelInfoList = new List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo>((IEnumerable<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo>) fuelInfoList)
        {
          new MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo()
          {
            Ratio = 1f,
            Id = reactorDefinition.FuelId.Value
          }
        };
      this.FuelInfos = new MyReactorDefinition.FuelInfo[fuelInfoList.Count];
      this.InventoryConstraint = new MyInventoryConstraint(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockInput), (object) this.DisplayNameText));
      for (int index = 0; index < fuelInfoList.Count; ++index)
      {
        MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo fuelInfo = fuelInfoList[index];
        this.InventoryConstraint.Add((MyDefinitionId) fuelInfo.Id);
        this.FuelInfos[index] = new MyReactorDefinition.FuelInfo(fuelInfo, this);
      }
    }

    public struct FuelInfo
    {
      public readonly float Ratio;
      public readonly MyDefinitionId FuelId;
      public readonly float ConsumptionPerSecond_Items;
      public readonly MyPhysicalItemDefinition FuelDefinition;
      public readonly MyObjectBuilder_PhysicalObject FuelItem;

      public FuelInfo(
        MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo fuelInfo,
        MyReactorDefinition blockDefinition)
      {
        this.FuelId = (MyDefinitionId) fuelInfo.Id;
        this.Ratio = fuelInfo.Ratio;
        this.FuelDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyDefinitionId) fuelInfo.Id);
        this.FuelItem = MyObjectBuilderSerializer.CreateNewObject(fuelInfo.Id) as MyObjectBuilder_PhysicalObject;
        this.ConsumptionPerSecond_Items = blockDefinition.MaxPowerOutput / blockDefinition.FuelProductionToCapacityMultiplier * this.Ratio / this.FuelDefinition.Mass;
      }
    }

    private class Sandbox_Definitions_MyReactorDefinition\u003C\u003EActor : IActivator, IActivator<MyReactorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyReactorDefinition();

      MyReactorDefinition IActivator<MyReactorDefinition>.CreateInstance() => new MyReactorDefinition();
    }
  }
}
