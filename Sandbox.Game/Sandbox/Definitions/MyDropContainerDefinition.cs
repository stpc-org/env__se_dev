// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDropContainerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DropContainerDefinition), null)]
  public class MyDropContainerDefinition : MyDefinitionBase
  {
    public MyPrefabDefinition Prefab;
    public MyContainerSpawnRules SpawnRules;
    public float Priority;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_DropContainerDefinition containerDefinition = builder as MyObjectBuilder_DropContainerDefinition;
      this.SpawnRules = containerDefinition.SpawnRules;
      this.Prefab = MyDefinitionManager.Static.GetPrefabDefinition(containerDefinition.Prefab);
      this.Priority = containerDefinition.Priority;
    }

    private class Sandbox_Definitions_MyDropContainerDefinition\u003C\u003EActor : IActivator, IActivator<MyDropContainerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDropContainerDefinition();

      MyDropContainerDefinition IActivator<MyDropContainerDefinition>.CreateInstance() => new MyDropContainerDefinition();
    }
  }
}
