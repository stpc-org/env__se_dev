// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugInput
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDebugInput : MyGuiScreenDebugBase
  {
    private static StringBuilder m_debugText = new StringBuilder(1000);

    public MyGuiScreenDebugInput()
      : base(new Vector2(0.5f, 0.5f), new Vector2?(new Vector2()), new Vector4?(), true)
    {
      this.m_isTopMostScreen = true;
      this.m_drawEvenWithoutFocus = true;
      this.CanHaveFocus = false;
    }

    public override string GetFriendlyName() => "DebugInputScreen";

    public Vector2 GetScreenLeftTopPosition()
    {
      double num = 25.0 * (double) MyGuiManager.GetSafeScreenScale();
      MyGuiManager.GetSafeFullscreenRectangle();
      return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float) num, (float) num));
    }

    public void SetTexts()
    {
      MyGuiScreenDebugInput.m_debugText.Clear();
      MyInput.Static.GetActualJoystickState(MyGuiScreenDebugInput.m_debugText);
    }

    public override bool Draw()
    {
      if (!base.Draw())
        return false;
      this.SetTexts();
      float statisticsTextScale = MyGuiConstants.DEBUG_STATISTICS_TEXT_SCALE;
      Vector2 screenLeftTopPosition = this.GetScreenLeftTopPosition();
      MyGuiManager.DrawString("White", MyGuiScreenDebugInput.m_debugText.ToString(), screenLeftTopPosition, statisticsTextScale, new Color?(Color.Yellow));
      return true;
    }
  }
}
