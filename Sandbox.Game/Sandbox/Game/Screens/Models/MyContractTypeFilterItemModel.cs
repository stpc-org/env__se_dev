// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractTypeFilterItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Screens.Models
{
  public class MyContractTypeFilterItemModel : BindableBase
  {
    private string m_name;
    private string m_localizedName;
    private BitmapImage m_icon;
    private MyDefinitionId? m_contractTypeId;

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public string LocalizedName
    {
      get => this.m_localizedName;
      set => this.SetProperty<string>(ref this.m_localizedName, value, nameof (LocalizedName));
    }

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public MyDefinitionId? ContractTypeId
    {
      get => this.m_contractTypeId;
      set => this.SetProperty<MyDefinitionId?>(ref this.m_contractTypeId, value, nameof (ContractTypeId));
    }

    public class MyComparator_LocalizedName : IComparer<MyContractTypeFilterItemModel>
    {
      public int Compare(MyContractTypeFilterItemModel x, MyContractTypeFilterItemModel y)
      {
        if (y == null)
          return 1;
        return x == null ? -1 : string.Compare(x.LocalizedName, y.LocalizedName);
      }
    }
  }
}
