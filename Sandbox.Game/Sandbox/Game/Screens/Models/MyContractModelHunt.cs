// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelHunt
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Models
{
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractHunt))]
  public class MyContractModelHunt : MyContractModel
  {
    private static readonly MyDefinitionId m_definitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Hunt");
    private long m_targetId;

    public long TargetId
    {
      get => this.m_targetId;
      set => this.SetProperty<long>(ref this.m_targetId, value, nameof (TargetId));
    }

    public string TargetName_Formatted
    {
      get
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.TargetId);
        return identity == null ? "Missing Identity" : identity.DisplayName + (identity.IsDead ? " (Offline)" : string.Empty);
      }
    }

    public override string Description => MyTexts.GetString(string.Format("ContractScreen_Hunt_Description_{0}", (object) (this.Id % 3L)));

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      if (ob is MyObjectBuilder_ContractHunt builderContractHunt)
        this.TargetId = builderContractHunt.Target;
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(MyContractModelHunt.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Hunt_WithId), (object) id);

    protected override string BuildName() => MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Hunt);
  }
}
