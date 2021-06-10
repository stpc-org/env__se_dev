// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractTypeModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.Models
{
  public class MyContractTypeModel : BindableBase
  {
    private string m_name;

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }
  }
}
