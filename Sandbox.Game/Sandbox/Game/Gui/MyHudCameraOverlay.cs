// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyHudCameraOverlay
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  internal class MyHudCameraOverlay : MyGuiControlBase
  {
    private static string m_textureName;
    private static bool m_enabled;

    public static string TextureName
    {
      get => MyHudCameraOverlay.m_textureName;
      set => MyHudCameraOverlay.m_textureName = value;
    }

    public new static bool Enabled
    {
      get => MyHudCameraOverlay.m_enabled;
      set
      {
        if (MyHudCameraOverlay.m_enabled == value)
          return;
        MyHudCameraOverlay.m_enabled = value;
      }
    }

    public MyHudCameraOverlay()
      : base()
      => MyHudCameraOverlay.Enabled = false;

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
      if (!MyHudCameraOverlay.Enabled)
        return;
      MyHudCameraOverlay.DrawFullScreenSprite();
    }

    private static void DrawFullScreenSprite()
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      RectangleF destination = new RectangleF((float) fullscreenRectangle.X, (float) fullscreenRectangle.Y, (float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      Rectangle? sourceRectangle = new Rectangle?();
      MyRenderProxy.DrawSprite(MyHudCameraOverlay.m_textureName, ref destination, sourceRectangle, Color.White, 0.0f, true, true);
    }
  }
}
