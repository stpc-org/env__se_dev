// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenFade
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenFade : MyGuiScreenBase
  {
    private uint m_fadeInTimeMs;
    private uint m_fadeOutTimeMs;

    public event Action<MyGuiScreenFade> Shown;

    public override string GetFriendlyName() => "Fade Screen";

    public override int GetTransitionOpeningTime() => (int) this.m_fadeInTimeMs;

    public override int GetTransitionClosingTime() => (int) this.m_fadeOutTimeMs;

    public MyGuiScreenFade(Color fadeColor, uint fadeInTimeMs = 5000, uint fadeOutTimeMs = 5000)
      : base(new Vector2?(Vector2.Zero), new Vector4?((Vector4) fadeColor), new Vector2?(Vector2.One * 10f), true)
    {
      this.m_fadeInTimeMs = fadeInTimeMs;
      this.m_fadeOutTimeMs = fadeOutTimeMs;
      this.m_backgroundFadeColor = fadeColor;
      this.EnabledBackgroundFade = true;
    }

    protected override void OnShow()
    {
      if (this.Shown == null)
        return;
      this.Shown(this);
    }

    public override bool CloseScreen(bool isUnloading = false) => base.CloseScreen(isUnloading);
  }
}
