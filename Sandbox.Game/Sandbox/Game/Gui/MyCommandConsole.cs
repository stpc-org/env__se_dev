// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyCommandConsole
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Text;
using VRage;

namespace Sandbox.Game.GUI
{
  [PreloadRequired]
  public class MyCommandConsole : MyCommand
  {
    static MyCommandConsole() => MyConsole.AddCommand((MyCommand) new MyCommandConsole());

    public override string Prefix() => "Console";

    private MyCommandConsole()
    {
      this.m_methods.Add("Clear", new MyCommand.MyCommandAction()
      {
        Parser = (ParserDelegate) (x => (MyCommandArgs) null),
        CallAction = (ActionDelegate) (x => this.ClearConsole(x))
      });
      this.m_methods.Add("Script", new MyCommand.MyCommandAction()
      {
        Parser = (ParserDelegate) (x => (MyCommandArgs) null),
        CallAction = new ActionDelegate(this.ScriptConsole)
      });
    }

    private StringBuilder ScriptConsole(MyCommandArgs x) => new StringBuilder("Scripting mode. Send blank line to compile and run.");

    private StringBuilder ClearConsole(MyCommandArgs args)
    {
      MyConsole.Clear();
      return new StringBuilder("Console cleared...");
    }
  }
}
