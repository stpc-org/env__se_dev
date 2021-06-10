// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatHudMode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatHudMode : MyStatBase
  {
    public MyStatHudMode()
    {
      this.Id = MyStringHash.GetOrCompute("hud_mode");
      this.CurrentValue = (float) MySandboxGame.Config.HudState;
    }

    public override void Update()
    {
      this.CurrentValue = (float) MyHud.HudState;
      if (!MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOGGLE_HUD) || !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay) || MyInput.Static.IsAnyAltKeyPressed())
        return;
      ++this.CurrentValue;
      if ((double) this.CurrentValue > 2.0)
        this.CurrentValue = 0.0f;
      MyHud.HudState = (int) this.CurrentValue;
      MyHud.MinimalHud = MyHud.IsHudMinimal;
      MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
    }
  }
}
