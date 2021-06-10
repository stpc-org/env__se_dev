// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContractBlockDefinition), null)]
  public class MyContractBlockDefinition : MyCubeBlockDefinition
  {
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ContractBlockDefinition contractBlockDefinition = builder as MyObjectBuilder_ContractBlockDefinition;
      this.ScreenAreas = contractBlockDefinition.ScreenAreas != null ? contractBlockDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MyContractBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyContractBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractBlockDefinition();

      MyContractBlockDefinition IActivator<MyContractBlockDefinition>.CreateInstance() => new MyContractBlockDefinition();
    }
  }
}
