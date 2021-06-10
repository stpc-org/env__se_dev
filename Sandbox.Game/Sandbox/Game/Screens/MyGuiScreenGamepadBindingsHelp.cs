// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenGamepadBindingsHelp
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenGamepadBindingsHelp : MyGuiScreenBase
  {
    private ControlScheme m_scheme;
    private MyGuiControlGamepadBindings m_character;
    private MyGuiControlGamepadBindings m_jetpack;
    private MyGuiControlGamepadBindings m_ship;

    public MyGuiScreenGamepadBindingsHelp(ControlScheme scheme)
      : base()
    {
      this.CanHideOthers = false;
      this.m_scheme = scheme;
      this.m_backgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK.Center.Texture;
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 1f));
      this.EnabledBackgroundFade = true;
      this.Size = new Vector2?(new Vector2(1.05f, 0.75f));
      this.RecreateControls(true);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y, MyControlStateType.NEW_RELEASED))
        return;
      this.CloseScreen();
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_character = new MyGuiControlGamepadBindings(BindingType.Character, this.m_scheme, false);
      this.m_jetpack = new MyGuiControlGamepadBindings(BindingType.Jetpack, this.m_scheme, false);
      this.m_ship = new MyGuiControlGamepadBindings(BindingType.Ship, this.m_scheme, false);
      Vector2 size = this.m_character.Size;
      this.m_character.Position = new Vector2(-0.5f * size.X, -0.5f * size.Y);
      this.m_jetpack.Position = new Vector2(0.5f * size.X, -0.5f * size.Y);
      this.m_ship.Position = new Vector2(0.0f, 0.5f * size.Y);
      this.Controls.Add((MyGuiControlBase) this.m_character);
      this.Controls.Add((MyGuiControlBase) this.m_jetpack);
      this.Controls.Add((MyGuiControlBase) this.m_ship);
    }

    public override string GetFriendlyName() => "GameBindingsHelp";
  }
}
