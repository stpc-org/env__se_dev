// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyRendererStatsComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using System.Text;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyRendererStatsComponent : MyDebugComponent
  {
    private static StringBuilder m_frameDebugText = new StringBuilder(1024);
    private static StringBuilder m_frameDebugText2 = new StringBuilder(1024);

    public override string GetName() => "RendererStats";

    public Vector2 GetScreenLeftTopPosition()
    {
      double num = 25.0 * (double) MyGuiManager.GetSafeScreenScale();
      MyGuiManager.GetSafeFullscreenRectangle();
      return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float) num, (float) num));
    }

    public override void Draw()
    {
    }
  }
}
