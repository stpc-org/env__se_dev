// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPlanetPrefabDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PlanetPrefabDefinition), null)]
  public class MyPlanetPrefabDefinition : MyDefinitionBase
  {
    public MyObjectBuilder_Planet PlanetBuilder;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.PlanetBuilder = (builder as MyObjectBuilder_PlanetPrefabDefinition).PlanetBuilder;
    }

    private class Sandbox_Definitions_MyPlanetPrefabDefinition\u003C\u003EActor : IActivator, IActivator<MyPlanetPrefabDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetPrefabDefinition();

      MyPlanetPrefabDefinition IActivator<MyPlanetPrefabDefinition>.CreateInstance() => new MyPlanetPrefabDefinition();
    }
  }
}
