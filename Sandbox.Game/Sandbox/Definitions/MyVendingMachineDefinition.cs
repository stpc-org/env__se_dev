// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyVendingMachineDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_VendingMachineDefinition), null)]
  public class MyVendingMachineDefinition : MyStoreBlockDefinition
  {
    public List<string> AdditionalEmissiveMaterials;
    public List<MyObjectBuilder_StoreItem> DefaultItems;
    public string ThrowOutDummy;
    public Dictionary<string, float> ThrowOutItems;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_VendingMachineDefinition machineDefinition = (MyObjectBuilder_VendingMachineDefinition) builder;
      this.AdditionalEmissiveMaterials = machineDefinition.AdditionalEmissiveMaterials;
      this.DefaultItems = machineDefinition.DefaultItems;
      this.ThrowOutDummy = machineDefinition.ThrowOutDummy;
      if (machineDefinition.ThrowOutItems == null)
        return;
      this.ThrowOutItems = machineDefinition.ThrowOutItems.Dictionary;
    }

    private class Sandbox_Definitions_MyVendingMachineDefinition\u003C\u003EActor : IActivator, IActivator<MyVendingMachineDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVendingMachineDefinition();

      MyVendingMachineDefinition IActivator<MyVendingMachineDefinition>.CreateInstance() => new MyVendingMachineDefinition();
    }
  }
}
