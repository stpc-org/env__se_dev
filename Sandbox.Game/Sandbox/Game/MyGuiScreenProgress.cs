// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyGuiScreenProgress
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
  public class MyGuiScreenProgress : MyGuiScreenProgressBase
  {
    public event Action Tick;

    public MyGuiScreenProgress(
      StringBuilder text,
      MyStringId? cancelText = null,
      bool isTopMostScreen = true,
      bool enableBackgroundFade = true)
      : base(MySpaceTexts.Blank, cancelText, isTopMostScreen, enableBackgroundFade)
    {
      this.Text = new StringBuilder(text.ToString());
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_rotatingWheel.MultipleSpinningWheels = MyPerGameSettings.GUI.MultipleSpinningWheels;
    }

    protected override void ProgressStart()
    {
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenProgress);

    public StringBuilder Text
    {
      get => this.m_progressTextLabel.TextToDraw;
      set => this.m_progressTextLabel.TextToDraw = value;
    }

    public override bool Update(bool hasFocus)
    {
      Action tick = this.Tick;
      if (tick != null && !this.Cancelled)
        tick();
      return base.Update(hasFocus);
    }

    public override bool Draw()
    {
      if (this.SkipTransition)
      {
        this.RotatingWheel.ManualRotationUpdate = false;
        Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
        MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background.dds", fullscreenRectangle, new Color(new Vector4(1f, 1f, 1f, 1f)), true, true);
        MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Screens\\main_menu_overlay.dds", fullscreenRectangle, new Color(new Vector4(1f, 1f, 1f, 1f)), true, true);
      }
      return base.Draw();
    }

    public void DrawPaused()
    {
      this.m_transitionAlpha = 1f;
      this.SkipTransition = true;
      try
      {
        MyRenderProxy.PauseTimer(false);
        MyRenderProxy.AfterUpdate(new MyTimeSpan?(), false);
        MyRenderProxy.BeforeUpdate();
        MyGuiSandbox.Draw();
        MyRenderProxy.AfterUpdate(new MyTimeSpan?(), false);
        MyRenderProxy.BeforeUpdate();
      }
      catch
      {
      }
    }
  }
}
