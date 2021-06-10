// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyModCategoryModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;

namespace Sandbox.Game.Screens.Models
{
  public class MyModCategoryModel : BindableBase
  {
    private string m_id;
    private string m_localizedName;
    private bool m_isChecked;
    private ICommand m_toggleCategoryCommand;

    public string Id
    {
      get => this.m_id;
      set => this.SetProperty<string>(ref this.m_id, value, nameof (Id));
    }

    public string LocalizedName
    {
      get => this.m_localizedName;
      set => this.SetProperty<string>(ref this.m_localizedName, value, nameof (LocalizedName));
    }

    public bool IsChecked
    {
      get => this.m_isChecked;
      set => this.SetProperty<bool>(ref this.m_isChecked, value, nameof (IsChecked));
    }

    public ICommand ToggleCategoryCommand
    {
      get => this.m_toggleCategoryCommand;
      set => this.SetProperty<ICommand>(ref this.m_toggleCategoryCommand, value, nameof (ToggleCategoryCommand));
    }

    public MyModCategoryModel() => this.ToggleCategoryCommand = (ICommand) new RelayCommand(new Action<object>(this.OnToggleCategory));

    private void OnToggleCategory(object obj) => this.IsChecked = !this.IsChecked;
  }
}
