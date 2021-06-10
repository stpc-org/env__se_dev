// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyNewGameScreenABTestHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyNewGameScreenABTestHelper
  {
    private static MyNewGameScreenABTestHelper m_instance;
    private bool m_isActive;
    private bool m_isApplied;

    public static MyNewGameScreenABTestHelper Instance
    {
      get
      {
        if (MyNewGameScreenABTestHelper.m_instance == null)
          MyNewGameScreenABTestHelper.m_instance = new MyNewGameScreenABTestHelper();
        return MyNewGameScreenABTestHelper.m_instance;
      }
    }

    private MyNewGameScreenABTestHelper() => this.Clear();

    public void Clear()
    {
      this.m_isActive = false;
      this.m_isApplied = false;
    }

    public void ActivateTest() => this.m_isActive = true;

    public bool IsActive() => this.m_isActive;

    public bool IsApplied() => this.m_isApplied;

    public bool IsA() => Sync.MyId % 2UL == 1UL || !MyPlatformGameSettings.ENABLE_NEWGAME_SCREEN_ABTEST;

    public bool ApplyTest()
    {
      if (!this.m_isActive)
        return false;
      MySandboxGame.Config.EnableNewNewGameScreen = this.IsA() && MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN;
      this.m_isApplied = true;
      return true;
    }
  }
}
