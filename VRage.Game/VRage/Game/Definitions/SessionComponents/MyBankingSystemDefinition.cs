// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MyBankingSystemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_BankingSystemDefinition), null)]
  public class MyBankingSystemDefinition : MySessionComponentDefinition
  {
    public MyStringId CurrencyFullName { get; private set; }

    public MyStringId CurrencyShortName { get; private set; }

    public long StartingBalance { get; private set; }

    public uint AccountLogLen { get; private set; }

    public MyDefinitionId PhysicalItemId { get; private set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_BankingSystemDefinition systemDefinition = builder as MyObjectBuilder_BankingSystemDefinition;
      this.CurrencyFullName = MyStringId.GetOrCompute(systemDefinition.CurrencyFullName);
      this.CurrencyShortName = MyStringId.GetOrCompute(systemDefinition.CurrencyShortName);
      this.StartingBalance = systemDefinition.StartingBalance;
      this.AccountLogLen = systemDefinition.AccountLogLen;
      this.PhysicalItemId = (MyDefinitionId) systemDefinition.PhysicalItemId;
    }

    private class VRage_Game_Definitions_SessionComponents_MyBankingSystemDefinition\u003C\u003EActor : IActivator, IActivator<MyBankingSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBankingSystemDefinition();

      MyBankingSystemDefinition IActivator<MyBankingSystemDefinition>.CreateInstance() => new MyBankingSystemDefinition();
    }
  }
}
