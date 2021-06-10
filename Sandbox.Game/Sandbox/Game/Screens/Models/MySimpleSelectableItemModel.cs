// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MySimpleSelectableItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Entities;

namespace Sandbox.Game.Screens.Models
{
  public class MySimpleSelectableItemModel : BindableBase
  {
    private string m_name;
    private string m_displayName;
    private long m_id;

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public string DisplayName
    {
      get => this.m_displayName;
      set => this.SetProperty<string>(ref this.m_displayName, value, nameof (DisplayName));
    }

    public long Id
    {
      get => this.m_id;
      set => this.SetProperty<long>(ref this.m_id, value, nameof (Id));
    }

    public MySimpleSelectableItemModel()
    {
      this.Name = string.Empty;
      this.DisplayName = string.Empty;
      this.Id = 0L;
    }

    public MySimpleSelectableItemModel(MyCubeGrid grid)
    {
      this.Name = grid.Name;
      this.DisplayName = grid.DisplayName;
      this.Id = grid.EntityId;
    }

    public MySimpleSelectableItemModel(long id, string name, string displayName)
    {
      this.Name = name;
      this.DisplayName = displayName;
      this.Id = id;
    }
  }
}
