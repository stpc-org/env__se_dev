// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyGuiModHelpers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
  public class MyGuiModHelpers : IMyGui
  {
    public event Action<object> GuiControlCreated
    {
      add => MyGuiSandbox.GuiControlCreated += this.GetDelegate(value);
      remove => MyGuiSandbox.GuiControlCreated -= this.GetDelegate(value);
    }

    public event Action<object> GuiControlRemoved
    {
      add => MyGuiSandbox.GuiControlRemoved += this.GetDelegate(value);
      remove => MyGuiSandbox.GuiControlRemoved -= this.GetDelegate(value);
    }

    public string ActiveGamePlayScreen
    {
      get
      {
        MyGuiScreenBase activeGameplayScreen = MyGuiScreenGamePlay.ActiveGameplayScreen;
        if (activeGameplayScreen != null)
          return activeGameplayScreen.Name;
        return MyGuiScreenTerminal.GetCurrentScreen() != MyTerminalPageEnum.None ? "MyGuiScreenTerminal" : (string) null;
      }
    }

    public IMyEntity InteractedEntity => (IMyEntity) MyGuiScreenTerminal.InteractedEntity;

    public MyTerminalPageEnum GetCurrentScreen => MyGuiScreenTerminal.GetCurrentScreen();

    public bool ChatEntryVisible => MyGuiScreenChat.Static != null && MyGuiScreenChat.Static.ChatTextbox != null && MyGuiScreenChat.Static.ChatTextbox.Visible;

    public bool IsCursorVisible => MySandboxGame.Static.IsCursorVisible;

    private Action<object> GetDelegate(Action<object> value) => (Action<object>) Delegate.CreateDelegate(typeof (Action<object>), value.Target, value.Method);
  }
}
