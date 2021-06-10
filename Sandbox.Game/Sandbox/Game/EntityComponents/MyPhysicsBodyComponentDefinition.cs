// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyPhysicsBodyComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_PhysicsBodyComponentDefinition), null)]
  public class MyPhysicsBodyComponentDefinition : MyPhysicsComponentDefinitionBase
  {
    public bool CreateFromCollisionObject;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.CreateFromCollisionObject = (builder as MyObjectBuilder_PhysicsBodyComponentDefinition).CreateFromCollisionObject;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_PhysicsBodyComponentDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_PhysicsBodyComponentDefinition;
      objectBuilder.CreateFromCollisionObject = this.CreateFromCollisionObject;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Game_EntityComponents_MyPhysicsBodyComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyPhysicsBodyComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicsBodyComponentDefinition();

      MyPhysicsBodyComponentDefinition IActivator<MyPhysicsBodyComponentDefinition>.CreateInstance() => new MyPhysicsBodyComponentDefinition();
    }
  }
}
