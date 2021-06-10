// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelFind
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
  [MyContractModelDescriptor(typeof (MyObjectBuilder_ContractFind))]
  public class MyContractModelFind : MyContractModel
  {
    private static readonly MyDefinitionId m_definitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Find");
    private double m_gpsDistance;
    private float m_maxGpsOffset;

    public double GpsDistance
    {
      get => this.m_gpsDistance;
      set => this.SetProperty<double>(ref this.m_gpsDistance, value, nameof (GpsDistance));
    }

    public float MaxGpsOffset
    {
      get => this.m_maxGpsOffset;
      set
      {
        this.SetProperty<float>(ref this.m_maxGpsOffset, value, nameof (MaxGpsOffset));
        this.RaisePropertyChanged("MaxGpsOffset_Formatted");
      }
    }

    public string MaxGpsOffset_Formatted => (double) this.m_maxGpsOffset < 2000.0 ? string.Format("{0} m", (object) this.m_maxGpsOffset) : string.Format("{0} km", (object) (float) ((double) this.m_maxGpsOffset / 1000.0));

    public override string Description => MyTexts.GetString(string.Format("ContractScreen_Find_Description_{0}", (object) (this.Id % 5L)));

    public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      if (ob is MyObjectBuilder_ContractFind builderContractFind)
      {
        this.GpsDistance = builderContractFind.GpsDistance;
        this.MaxGpsOffset = builderContractFind.MaxGpsOffset;
      }
      this.ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(MyContractModelFind.m_definitionId.SubtypeName);
      base.Init(ob, showFactionIcons);
    }

    protected override string BuildNameWithId(long id) => string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Find_WithId), (object) id);

    protected override string BuildName() => MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Find);
  }
}
