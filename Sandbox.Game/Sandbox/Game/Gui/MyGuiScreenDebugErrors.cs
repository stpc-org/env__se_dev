// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugErrors
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Linq;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDebugErrors : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugErrors);

    public MyGuiScreenDebugErrors()
      : base(new Vector2(0.5f, 0.5f), new Vector2?(), new Vector4?(), true)
    {
      this.EnabledBackgroundFade = true;
      this.m_backgroundTexture = (string) null;
      Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
      this.Size = new Vector2?(new Vector2((float) ((double) ((float) fullscreenRectangle.Width / (float) fullscreenRectangle.Height) * 3.0 / 4.0), 1f));
      this.CanHideOthers = true;
      this.m_isTopScreen = true;
      this.m_canShareInput = false;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      this.AddCaption(MyCommonTexts.ScreenDebugOfficial_ErrorLogCaption, captionOffset: new Vector2?(new Vector2(0.0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y * -0.5f)));
      this.m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
      Vector2? size1 = this.Size;
      Vector2 vector2 = new Vector2(0.0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
      Vector2? size2 = size1.HasValue ? new Vector2?(size1.GetValueOrDefault() - vector2) : new Vector2?();
      size1 = this.Size;
      float num = -0.5f;
      Vector2? offset = size1.HasValue ? new Vector2?(size1.GetValueOrDefault() * num) : new Vector2?();
      MyGuiControlMultilineText controlMultilineText = this.AddMultilineText(size2, offset, 0.7f);
      if (MyDefinitionErrors.GetErrors().Count<MyDefinitionErrors.Error>() == 0)
        controlMultilineText.AppendText(MyTexts.Get(MyCommonTexts.ScreenDebugOfficial_NoErrorText));
      foreach (MyDefinitionErrors.Error error in MyDefinitionErrors.GetErrors())
      {
        controlMultilineText.AppendText(error.ToString(), controlMultilineText.Font, controlMultilineText.TextScaleWithLanguage, error.GetSeverityColor().ToVector4());
        controlMultilineText.AppendLine();
        controlMultilineText.AppendLine();
      }
    }
  }
}
