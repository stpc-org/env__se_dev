// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyTest
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyTest : BindableBase
  {
    private string text;

    public string Text
    {
      get => this.text;
      set => this.SetProperty<string>(ref this.text, value, nameof (Text));
    }
  }
}
