// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelRepair
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Models
{
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractRepair))]
  public class MyContractModelRepair : MyContractModel
  {
    private static readonly MyDefinitionId m_definitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Repair");

    public override string Description => MyTexts.GetString(string.Format("ContractScreen_Repair_Description_{0}", (object) (this.Id % 6L)));

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      MyObjectBuilder_ContractFind builderContractFind = ob as MyObjectBuilder_ContractFind;
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(MyContractModelRepair.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Repair_WithId), (object) id);

    protected override string BuildName() => MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Repair);
  }
}
