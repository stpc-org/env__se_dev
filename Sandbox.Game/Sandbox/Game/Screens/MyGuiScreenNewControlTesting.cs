// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenNewControlTesting
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenNewControlTesting : MyGuiScreenBase
  {
    public override string GetFriendlyName() => "TESTING!";

    public MyGuiScreenNewControlTesting()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.9f, 0.97f)))
    {
      MyGuiControlSaveBrowser controlSaveBrowser = new MyGuiControlSaveBrowser();
      controlSaveBrowser.Size = this.Size.Value - new Vector2(0.1f);
      controlSaveBrowser.Position = -this.Size.Value / 2f + new Vector2(0.05f);
      controlSaveBrowser.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlSaveBrowser.VisibleRowsCount = 20;
      controlSaveBrowser.HeaderVisible = true;
      this.Controls.Add((MyGuiControlBase) controlSaveBrowser);
    }
  }
}
