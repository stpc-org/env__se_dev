// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelObtainAndDeliver
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
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractObtainAndDeliver))]
  public class MyContractModelObtainAndDeliver : MyContractModel
  {
    private static readonly MyDefinitionId m_definitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "ObtainAndDeliver");

    public override string Description => MyTexts.GetString(string.Format("ContractScreen_ObtainDeliver_Description_{0}", (object) (this.Id % 6L)));

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(MyContractModelObtainAndDeliver.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_ObtainAndDeliver_WithId), (object) id);

    protected override string BuildName() => MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_ObtainAndDeliver);
  }
}
