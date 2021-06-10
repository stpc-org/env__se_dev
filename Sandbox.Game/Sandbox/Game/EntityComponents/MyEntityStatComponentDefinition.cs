// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityStatComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_EntityStatComponentDefinition), null)]
  public class MyEntityStatComponentDefinition : MyComponentDefinitionBase
  {
    public List<MyDefinitionId> Stats;
    public List<string> Scripts;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EntityStatComponentDefinition componentDefinition = builder as MyObjectBuilder_EntityStatComponentDefinition;
      this.Stats = new List<MyDefinitionId>();
      foreach (SerializableDefinitionId stat in componentDefinition.Stats)
        this.Stats.Add((MyDefinitionId) stat);
      this.Scripts = componentDefinition.Scripts;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_EntityStatComponentDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_EntityStatComponentDefinition;
      objectBuilder.Stats = new List<SerializableDefinitionId>();
      foreach (MyDefinitionId stat in this.Stats)
        objectBuilder.Stats.Add((SerializableDefinitionId) stat);
      objectBuilder.Scripts = this.Scripts;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Game_EntityComponents_MyEntityStatComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyEntityStatComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEntityStatComponentDefinition();

      MyEntityStatComponentDefinition IActivator<MyEntityStatComponentDefinition>.CreateInstance() => new MyEntityStatComponentDefinition();
    }
  }
}
