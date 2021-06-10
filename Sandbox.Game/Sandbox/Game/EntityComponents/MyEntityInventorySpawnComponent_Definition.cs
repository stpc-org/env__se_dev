// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityInventorySpawnComponent_Definition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_InventorySpawnComponent_Definition), null)]
  public class MyEntityInventorySpawnComponent_Definition : MyComponentDefinitionBase
  {
    public MyDefinitionId ContainerDefinition;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.ContainerDefinition = (MyDefinitionId) (builder as MyObjectBuilder_InventorySpawnComponent_Definition).ContainerDefinition;
    }

    private class Sandbox_Game_EntityComponents_MyEntityInventorySpawnComponent_Definition\u003C\u003EActor : IActivator, IActivator<MyEntityInventorySpawnComponent_Definition>
    {
      object IActivator.CreateInstance() => (object) new MyEntityInventorySpawnComponent_Definition();

      MyEntityInventorySpawnComponent_Definition IActivator<MyEntityInventorySpawnComponent_Definition>.CreateInstance() => new MyEntityInventorySpawnComponent_Definition();
    }
  }
}
