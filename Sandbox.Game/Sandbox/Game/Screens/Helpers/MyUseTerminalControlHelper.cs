// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyUseTerminalControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyUseTerminalControlHelper : MyAbstractControlMenuItem
  {
    private MyCharacter m_character;
    private string m_label;

    public MyUseTerminalControlHelper()
      : base(MyControlsSpace.TERMINAL)
    {
    }

    public void SetCharacter(MyCharacter character) => this.m_character = character;

    public override void Activate()
    {
      MyScreenManager.CloseScreen(typeof (MyGuiScreenControlMenu));
      this.m_character.UseTerminal();
    }

    public override string Label => this.m_label;

    public void SetLabel(MyStringId id) => this.m_label = MyTexts.GetString(id);
  }
}
