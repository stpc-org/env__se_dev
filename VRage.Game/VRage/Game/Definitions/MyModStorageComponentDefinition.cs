// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MyModStorageComponentDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace VRage.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ModStorageComponentDefinition), null)]
  public class MyModStorageComponentDefinition : MyComponentDefinitionBase
  {
    public Guid[] RegisteredStorageGuids;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.RegisteredStorageGuids = (builder as MyObjectBuilder_ModStorageComponentDefinition).RegisteredStorageGuids;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_ModStorageComponentDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_ModStorageComponentDefinition;
      objectBuilder.RegisteredStorageGuids = this.RegisteredStorageGuids;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class VRage_Game_Definitions_MyModStorageComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyModStorageComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyModStorageComponentDefinition();

      MyModStorageComponentDefinition IActivator<MyModStorageComponentDefinition>.CreateInstance() => new MyModStorageComponentDefinition();
    }
  }
}
