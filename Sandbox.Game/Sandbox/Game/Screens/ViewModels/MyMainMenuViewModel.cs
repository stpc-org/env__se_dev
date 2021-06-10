// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyMainMenuViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using System.Collections.ObjectModel;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyMainMenuViewModel : ViewModelBase
  {
    private ObservableCollection<MyTest> m_tests = new ObservableCollection<MyTest>();

    public ObservableCollection<MyTest> GridData
    {
      get => this.m_tests;
      set => this.SetProperty<ObservableCollection<MyTest>>(ref this.m_tests, value, nameof (GridData));
    }

    public MyMainMenuViewModel()
    {
      this.m_tests.Add(new MyTest() { Text = "Line 1" });
      this.m_tests.Add(new MyTest() { Text = "Line 2" });
      this.m_tests.Add(new MyTest() { Text = "Line 3" });
      this.m_tests.Add(new MyTest() { Text = "Line 4" });
      this.m_tests.Add(new MyTest() { Text = "Line 5" });
      this.m_tests.Add(new MyTest() { Text = "Line 6" });
      this.m_tests.Add(new MyTest() { Text = "Line 7" });
      this.m_tests.Add(new MyTest() { Text = "Line 8" });
      this.m_tests.Add(new MyTest() { Text = "Line 9" });
    }
  }
}
