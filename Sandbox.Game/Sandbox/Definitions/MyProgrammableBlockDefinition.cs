// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyProgrammableBlockDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_ProgrammableBlockDefinition), null)]
  public class MyProgrammableBlockDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ProgrammableBlockDefinition programmableBlockDefinition = (MyObjectBuilder_ProgrammableBlockDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(programmableBlockDefinition.ResourceSinkGroup);
      this.ScreenAreas = programmableBlockDefinition.ScreenAreas != null ? programmableBlockDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MyProgrammableBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyProgrammableBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyProgrammableBlockDefinition();

      MyProgrammableBlockDefinition IActivator<MyProgrammableBlockDefinition>.CreateInstance() => new MyProgrammableBlockDefinition();
    }
  }
}
