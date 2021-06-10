// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractConditionModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.Models
{
  public class MyContractConditionModel : BindableBase
  {
    private string m_name;
    private BitmapImage m_icon;
    private long m_id;
    private long m_contractId;
    private long m_stationEndId;
    private long m_blockEndId;

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public long Id
    {
      get => this.m_id;
      set => this.SetProperty<long>(ref this.m_id, value, nameof (Id));
    }

    public long ContractId
    {
      get => this.m_contractId;
      set => this.SetProperty<long>(ref this.m_contractId, value, nameof (ContractId));
    }

    public long StationEndId
    {
      get => this.m_stationEndId;
      set => this.SetProperty<long>(ref this.m_stationEndId, value, nameof (StationEndId));
    }

    public long BlockEndId
    {
      get => this.m_blockEndId;
      set => this.SetProperty<long>(ref this.m_blockEndId, value, nameof (BlockEndId));
    }

    protected virtual string BuildName(long id) => string.Format("DefaultCondition {0}", (object) id);

    protected virtual BitmapImage BuildImage() => new BitmapImage()
    {
      TextureAsset = "Textures\\GUI\\Icons\\WeaponDrill.dds"
    };

    public virtual void Init(MyObjectBuilder_ContractCondition ob)
    {
      this.Name = this.BuildName(ob.Id);
      this.Icon = this.BuildImage();
      this.Id = ob.Id;
      this.ContractId = ob.ContractId;
      this.StationEndId = ob.StationEndId;
      this.BlockEndId = ob.BlockEndId;
    }
  }
}
