// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelDeliver
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
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractDeliver))]
  public class MyContractModelDeliver : MyContractModel
  {
    private static readonly MyDefinitionId m_definitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Deliver");
    private double m_deliverDistance;

    public double DeliverDistance
    {
      get => this.m_deliverDistance;
      set => this.SetProperty<double>(ref this.m_deliverDistance, value, nameof (DeliverDistance));
    }

    public string DeliverDistance_Formatted => this.DeliverDistance < 10000.0 ? this.DeliverDistance.ToString("F0", (IFormatProvider) CultureInfo.InvariantCulture) + " m" : (this.DeliverDistance / 1000.0).ToString("F1", (IFormatProvider) CultureInfo.InvariantCulture) + " km";

    public override string Description => MyTexts.GetString(string.Format("ContractScreen_Deliver_Description_{0}", (object) (this.Id % 3L)));

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      if (ob is MyObjectBuilder_ContractDeliver builderContractDeliver)
        this.DeliverDistance = builderContractDeliver.DeliverDistance;
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(MyContractModelDeliver.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Deliver_WithId), (object) id);

    protected override string BuildName() => MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Deliver);
  }
}
