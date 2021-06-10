// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelEscort
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Localization;
using System;
using System.Globalization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Models
{
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractEscort))]
  public class MyContractModelEscort : MyContractModel
  {
    private static readonly MyDefinitionId m_definitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Escort");
    private double m_pathLength;

    public double PathLength
    {
      get => this.m_pathLength;
      set => this.SetProperty<double>(ref this.m_pathLength, value, nameof (PathLength));
    }

    public string PathLength_Formatted => this.PathLength < 10000.0 ? this.PathLength.ToString("F0", (IFormatProvider) CultureInfo.InvariantCulture) + " m" : (this.PathLength / 1000.0).ToString("F1", (IFormatProvider) CultureInfo.InvariantCulture) + " km";

    public override string Description => MyTexts.GetString(string.Format("ContractScreen_Escort_Description_{0}", (object) (this.Id % 4L)));

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      if (ob is MyObjectBuilder_ContractEscort builderContractEscort)
        this.PathLength = builderContractEscort.PathLength;
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(MyContractModelEscort.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Escort_WithId), (object) id);

    protected override string BuildName() => MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Escort);
  }
}
