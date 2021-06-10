// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlImage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  public class MyStatControlImage : MyStatControlBase
  {
    public MyObjectBuilder_GuiTexture Texture { get; set; }

    public override void Draw(float transitionAlpha)
    {
      Vector4 colorMask = this.ColorMask;
      bool flag = false;
      this.BlinkBehavior.UpdateBlink();
      if (this.BlinkBehavior.Blink)
      {
        transitionAlpha = MathHelper.Min(transitionAlpha, this.BlinkBehavior.CurrentBlinkAlpha);
        if (this.BlinkBehavior.ColorMask.HasValue)
        {
          this.ColorMask = this.BlinkBehavior.ColorMask.Value;
          flag = true;
        }
      }
      RectangleF destination = new RectangleF(this.Position, this.Size);
      Rectangle? sourceRectangle = new Rectangle?();
      Vector2 zero = Vector2.Zero;
      MyRenderProxy.DrawSprite(this.Texture.Path, ref destination, sourceRectangle, MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, true, transitionAlpha), 0.0f, false, true);
      if (!flag)
        return;
      this.ColorMask = colorMask;
    }

    public MyStatControlImage(MyStatControls parent)
      : base(parent)
    {
    }
  }
}
