// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyCommandCharacter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Text;
using VRage;

namespace Sandbox.Game.GUI
{
  [PreloadRequired]
  public class MyCommandCharacter : MyCommand
  {
    public override string Prefix() => "Character";

    static MyCommandCharacter() => MyConsole.AddCommand((MyCommand) new MyCommandCharacter());

    private MyCommandCharacter() => this.m_methods.Add("AddSomeValues", new MyCommand.MyCommandAction()
    {
      AutocompleteHint = new StringBuilder("int_val1 int_val2 ..."),
      Parser = (ParserDelegate) (x => this.ParseValues(x)),
      CallAction = (ActionDelegate) (x => this.PassValuesToCharacter(x))
    });

    private MyCommandArgs ParseValues(List<string> args)
    {
      MyCommandCharacter.MyCommandArgsValuesList commandArgsValuesList = new MyCommandCharacter.MyCommandArgsValuesList();
      commandArgsValuesList.values = new List<int>();
      foreach (string s in args)
        commandArgsValuesList.values.Add(int.Parse(s));
      return (MyCommandArgs) commandArgsValuesList;
    }

    private StringBuilder PassValuesToCharacter(MyCommandArgs args)
    {
      MyCommandCharacter.MyCommandArgsValuesList commandArgsValuesList = args as MyCommandCharacter.MyCommandArgsValuesList;
      if (commandArgsValuesList.values.Count == 0)
        return new StringBuilder("No values passed onto character");
      foreach (int num in commandArgsValuesList.values)
        ;
      StringBuilder stringBuilder = new StringBuilder().Append("Added values ");
      foreach (int num in commandArgsValuesList.values)
        stringBuilder.Append(num).Append(" ");
      stringBuilder.Append("to character");
      return stringBuilder;
    }

    private class MyCommandArgsValuesList : MyCommandArgs
    {
      public List<int> values;
    }
  }
}
