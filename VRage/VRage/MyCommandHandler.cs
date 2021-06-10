// Decompiled with JetBrains decompiler
// Type: VRage.MyCommandHandler
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRage
{
  internal class MyCommandHandler
  {
    private Dictionary<string, MyCommand> m_commands;

    public MyCommandHandler() => this.m_commands = new Dictionary<string, MyCommand>();

    public StringBuilder Handle(string input)
    {
      List<string> args = this.SplitArgs(input);
      if (args.Count <= 0)
        return new StringBuilder("Error: Empty string");
      string input1 = args[0];
      string commandKey = this.GetCommandKey(input1);
      if (commandKey == null)
        return new StringBuilder().AppendFormat("Error: Invalid method syntax '{0}'", (object) input);
      args.RemoveAt(0);
      MyCommand myCommand;
      if (!this.m_commands.TryGetValue(commandKey, out myCommand))
        return new StringBuilder().AppendFormat("Error: Unknown command {0}\n", (object) commandKey);
      string commandMethod = this.GetCommandMethod(input1);
      if (commandMethod == null)
        return new StringBuilder().AppendFormat("Error: Invalid method syntax '{0}'", (object) input);
      if (commandMethod == "")
        return new StringBuilder("Error: Empty Method");
      try
      {
        return new StringBuilder().Append(commandKey).Append(".").Append(commandMethod).Append(": ").Append((object) myCommand.Execute(commandMethod, args));
      }
      catch (MyConsoleInvalidArgumentsException ex)
      {
        return new StringBuilder().AppendFormat("Error: Invalid Argument for method {0}.{1}", (object) commandKey, (object) commandMethod);
      }
      catch (MyConsoleMethodNotFoundException ex)
      {
        return new StringBuilder().AppendFormat("Error: Command {0} does not contain method {1}", (object) commandKey, (object) commandMethod);
      }
    }

    public List<string> SplitArgs(string input) => ((IEnumerable<string>) input.Split('"')).Select<string, string[]>((Func<string, int, string[]>) ((element, index) => index % 2 != 0 ? new string[1]
    {
      element
    } : element.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries))).SelectMany<string[], string>((Func<string[], IEnumerable<string>>) (element => (IEnumerable<string>) element)).ToList<string>();

    public string GetCommandKey(string input) => !input.Contains(".") ? (string) null : input.Substring(0, input.IndexOf("."));

    public string GetCommandMethod(string input)
    {
      try
      {
        return input.Substring(input.IndexOf(".") + 1);
      }
      catch
      {
        return (string) null;
      }
    }

    public void AddCommand(MyCommand command)
    {
      if (this.m_commands.ContainsKey(command.Prefix()))
        this.m_commands.Remove(command.Prefix());
      this.m_commands.Add(command.Prefix(), command);
    }

    public void RemoveAllCommands() => this.m_commands.Clear();

    public bool ContainsCommand(string command) => this.m_commands.ContainsKey(command);

    public bool TryGetCommand(string commandName, out MyCommand command)
    {
      if (!this.m_commands.ContainsKey(commandName))
      {
        command = (MyCommand) null;
        return false;
      }
      command = this.m_commands[commandName];
      return true;
    }
  }
}
