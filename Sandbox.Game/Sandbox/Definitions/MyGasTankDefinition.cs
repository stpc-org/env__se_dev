// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGasTankDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GasTankDefinition), null)]
  public class MyGasTankDefinition : MyProductionBlockDefinition
  {
    public float Capacity;
    public MyDefinitionId StoredGasId;
    public MyStringHash ResourceSourceGroup;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GasTankDefinition gasTankDefinition = builder as MyObjectBuilder_GasTankDefinition;
      this.Capacity = gasTankDefinition.Capacity;
      this.StoredGasId = !gasTankDefinition.StoredGasId.IsNull() ? (MyDefinitionId) gasTankDefinition.StoredGasId : new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
      this.ResourceSourceGroup = MyStringHash.GetOrCompute(gasTankDefinition.ResourceSourceGroup);
    }

    private class Sandbox_Definitions_MyGasTankDefinition\u003C\u003EActor : IActivator, IActivator<MyGasTankDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGasTankDefinition();

      MyGasTankDefinition IActivator<MyGasTankDefinition>.CreateInstance() => new MyGasTankDefinition();
    }
  }
}
