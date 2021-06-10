// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGlobalEventDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GlobalEventDefinition), null)]
  public class MyGlobalEventDefinition : MyDefinitionBase
  {
    public TimeSpan? MinActivationTime;
    public TimeSpan? MaxActivationTime;
    public TimeSpan? FirstActivationTime;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      if (builder.Id.TypeId == typeof (MyObjectBuilder_GlobalEventDefinition))
        builder.Id = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), builder.Id.SubtypeName);
      base.Init(builder);
      MyObjectBuilder_GlobalEventDefinition globalEventDefinition = builder as MyObjectBuilder_GlobalEventDefinition;
      if (globalEventDefinition.MinActivationTimeMs.HasValue && !globalEventDefinition.MaxActivationTimeMs.HasValue)
        globalEventDefinition.MaxActivationTimeMs = globalEventDefinition.MinActivationTimeMs;
      if (globalEventDefinition.MaxActivationTimeMs.HasValue && !globalEventDefinition.MinActivationTimeMs.HasValue)
        globalEventDefinition.MinActivationTimeMs = globalEventDefinition.MaxActivationTimeMs;
      if (globalEventDefinition.MinActivationTimeMs.HasValue)
        this.MinActivationTime = new TimeSpan?(TimeSpan.FromTicks(globalEventDefinition.MinActivationTimeMs.Value * 10000L));
      if (globalEventDefinition.MaxActivationTimeMs.HasValue)
        this.MaxActivationTime = new TimeSpan?(TimeSpan.FromTicks(globalEventDefinition.MaxActivationTimeMs.Value * 10000L));
      if (!globalEventDefinition.FirstActivationTimeMs.HasValue)
        return;
      this.FirstActivationTime = new TimeSpan?(TimeSpan.FromTicks(globalEventDefinition.FirstActivationTimeMs.Value * 10000L));
    }

    private class Sandbox_Definitions_MyGlobalEventDefinition\u003C\u003EActor : IActivator, IActivator<MyGlobalEventDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGlobalEventDefinition();

      MyGlobalEventDefinition IActivator<MyGlobalEventDefinition>.CreateInstance() => new MyGlobalEventDefinition();
    }
  }
}
