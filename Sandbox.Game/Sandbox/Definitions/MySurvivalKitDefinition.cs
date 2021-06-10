// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySurvivalKitDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_SurvivalKitDefinition), null)]
  public class MySurvivalKitDefinition : MyAssemblerDefinition
  {
    public string ProgressSound = "BlockMedicalProgress";
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SurvivalKitDefinition survivalKitDefinition = (MyObjectBuilder_SurvivalKitDefinition) builder;
      this.ProgressSound = survivalKitDefinition.ProgressSound;
      this.ScreenAreas = survivalKitDefinition.ScreenAreas != null ? survivalKitDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MySurvivalKitDefinition\u003C\u003EActor : IActivator, IActivator<MySurvivalKitDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySurvivalKitDefinition();

      MySurvivalKitDefinition IActivator<MySurvivalKitDefinition>.CreateInstance() => new MySurvivalKitDefinition();
    }
  }
}
