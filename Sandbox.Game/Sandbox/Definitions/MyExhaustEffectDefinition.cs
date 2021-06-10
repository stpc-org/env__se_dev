// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyExhaustEffectDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_ExhaustEffectDefinition), null)]
  public class MyExhaustEffectDefinition : MyDefinitionBase
  {
    public List<MyObjectBuilder_ExhaustEffectDefinition.Pipe> ExhaustPipes;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ExhaustEffectDefinition effectDefinition = (MyObjectBuilder_ExhaustEffectDefinition) builder;
      this.ExhaustPipes = effectDefinition.ExhaustPipes != null ? effectDefinition.ExhaustPipes.ToList<MyObjectBuilder_ExhaustEffectDefinition.Pipe>() : (List<MyObjectBuilder_ExhaustEffectDefinition.Pipe>) null;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_ExhaustEffectDefinition objectBuilder = (MyObjectBuilder_ExhaustEffectDefinition) base.GetObjectBuilder();
      if (this.ExhaustPipes != null)
      {
        objectBuilder.ExhaustPipes = new List<MyObjectBuilder_ExhaustEffectDefinition.Pipe>();
        objectBuilder.ExhaustPipes.AddList<MyObjectBuilder_ExhaustEffectDefinition.Pipe>(this.ExhaustPipes);
      }
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Definitions_MyExhaustEffectDefinition\u003C\u003EActor : IActivator, IActivator<MyExhaustEffectDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyExhaustEffectDefinition();

      MyExhaustEffectDefinition IActivator<MyExhaustEffectDefinition>.CreateInstance() => new MyExhaustEffectDefinition();
    }
  }
}
