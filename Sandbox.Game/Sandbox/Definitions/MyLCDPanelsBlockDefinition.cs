// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLCDPanelsBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_LCDPanelsBlockDefinition), null)]
  public class MyLCDPanelsBlockDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_LCDPanelsBlockDefinition panelsBlockDefinition = (MyObjectBuilder_LCDPanelsBlockDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(panelsBlockDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = panelsBlockDefinition.RequiredPowerInput;
      this.ScreenAreas = panelsBlockDefinition.ScreenAreas != null ? panelsBlockDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MyLCDPanelsBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyLCDPanelsBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyLCDPanelsBlockDefinition();

      MyLCDPanelsBlockDefinition IActivator<MyLCDPanelsBlockDefinition>.CreateInstance() => new MyLCDPanelsBlockDefinition();
    }
  }
}
