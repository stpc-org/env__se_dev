// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelCustom
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.Models
{
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractCustom))]
  public class MyContractModelCustom : MyContractModel
  {
    private MyDefinitionId m_definitionId;
    private string m_name;
    private string m_description;

    public override string Description => this.m_description;

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      if (ob is MyObjectBuilder_ContractCustom builderContractCustom)
      {
        this.m_definitionId = (MyDefinitionId) builderContractCustom.DefinitionId;
        this.m_name = builderContractCustom.ContractName;
        this.m_description = builderContractCustom.ContractDescription;
      }
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(this.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format("{0} {1}", (object) this.m_name, (object) id);

    protected override string BuildName() => this.m_name;
  }
}
