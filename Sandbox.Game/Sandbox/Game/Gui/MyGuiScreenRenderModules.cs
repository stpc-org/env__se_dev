// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenRenderModules
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenRenderModules : MyGuiScreenDebugBase
  {
    public MyGuiScreenRenderModules()
      : base()
    {
      this.m_closeOnEsc = true;
      this.m_drawEvenWithoutFocus = true;
      this.m_isTopMostScreen = false;
      this.CanHaveFocus = false;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption("Render modules", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_scale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenRenderModules);
  }
}
