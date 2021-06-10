// Decompiled with JetBrains decompiler
// Type: VRage.MyCommand
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRage
{
  public abstract class MyCommand
  {
    protected Dictionary<string, MyCommand.MyCommandAction> m_methods;

    public List<string> Methods => this.m_methods.Keys.ToList<string>();

    public abstract string Prefix();

    public MyCommand() => this.m_methods = new Dictionary<string, MyCommand.MyCommandAction>();

    public StringBuilder Execute(string method, List<string> args)
    {
      MyCommand.MyCommandAction myCommandAction;
      if (!this.m_methods.TryGetValue(method, out myCommandAction))
        throw new MyConsoleMethodNotFoundException();
      try
      {
        MyCommandArgs commandArgs = myCommandAction.Parser(args);
        return myCommandAction.CallAction(commandArgs);
      }
      catch
      {
        throw new MyConsoleInvalidArgumentsException();
      }
    }

    public StringBuilder GetHint(string method)
    {
      MyCommand.MyCommandAction myCommandAction;
      return this.m_methods.TryGetValue(method, out myCommandAction) ? myCommandAction.AutocompleteHint : (StringBuilder) null;
    }

    protected class MyCommandAction
    {
      public StringBuilder AutocompleteHint = new StringBuilder("");
      public ParserDelegate Parser;
      public ActionDelegate CallAction;
    }
  }
}
