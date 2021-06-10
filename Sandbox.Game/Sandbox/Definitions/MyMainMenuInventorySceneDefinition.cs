// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMainMenuInventorySceneDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MainMenuInventorySceneDefinition), null)]
  public class MyMainMenuInventorySceneDefinition : MyDefinitionBase
  {
    public string SceneDirectory;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.SceneDirectory = (builder as MyObjectBuilder_MainMenuInventorySceneDefinition).SceneDirectory;
    }

    private class Sandbox_Definitions_MyMainMenuInventorySceneDefinition\u003C\u003EActor : IActivator, IActivator<MyMainMenuInventorySceneDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMainMenuInventorySceneDefinition();

      MyMainMenuInventorySceneDefinition IActivator<MyMainMenuInventorySceneDefinition>.CreateInstance() => new MyMainMenuInventorySceneDefinition();
    }
  }
}
