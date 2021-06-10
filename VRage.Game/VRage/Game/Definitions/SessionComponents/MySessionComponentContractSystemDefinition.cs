// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MySessionComponentContractSystemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_SessionComponentContractSystemDefinition), null)]
  public class MySessionComponentContractSystemDefinition : MySessionComponentDefinition
  {
    public const int ActiveContractsLimitPerPlayer = 20;
    public const int ContractCreationLimitPerPlayer = 20;

    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    private class VRage_Game_Definitions_SessionComponents_MySessionComponentContractSystemDefinition\u003C\u003EActor : IActivator, IActivator<MySessionComponentContractSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySessionComponentContractSystemDefinition();

      MySessionComponentContractSystemDefinition IActivator<MySessionComponentContractSystemDefinition>.CreateInstance() => new MySessionComponentContractSystemDefinition();
    }
  }
}
