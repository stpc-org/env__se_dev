// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyUpgradeModuleDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_UpgradeModuleDefinition), null)]
  public class MyUpgradeModuleDefinition : MyCubeBlockDefinition
  {
    public MyUpgradeModuleInfo[] Upgrades;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.Upgrades = (builder as MyObjectBuilder_UpgradeModuleDefinition).Upgrades;
    }

    private class Sandbox_Definitions_MyUpgradeModuleDefinition\u003C\u003EActor : IActivator, IActivator<MyUpgradeModuleDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyUpgradeModuleDefinition();

      MyUpgradeModuleDefinition IActivator<MyUpgradeModuleDefinition>.CreateInstance() => new MyUpgradeModuleDefinition();
    }
  }
}
