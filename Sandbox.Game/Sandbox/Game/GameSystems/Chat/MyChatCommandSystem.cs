// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.MyChatCommandSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage.Utils;

namespace Sandbox.Game.GameSystems.Chat
{
  public class MyChatCommandSystem
  {
    public Dictionary<string, IMyChatCommand> ChatCommands = new Dictionary<string, IMyChatCommand>();
    private static char[] m_separators = new char[3]
    {
      ' ',
      '\r',
      '\n'
    };

    public event MyChatCommandSystem.HandleCommandDelegate OnUnhandledCommand;

    public MyChatCommandSystem() => this.ScanAssemblyForCommands(Assembly.GetExecutingAssembly());

    public void Init() => MyChatCommands.PreloadCommands(this);

    public void Unload() => this.OnUnhandledCommand = (MyChatCommandSystem.HandleCommandDelegate) null;

    public void ScanAssemblyForCommands(Assembly assembly)
    {
      foreach (TypeInfo definedType in assembly.DefinedTypes)
      {
        if (definedType.ImplementedInterfaces.Contains<Type>(typeof (IMyChatCommand)))
        {
          if (!((Type) definedType == typeof (MyChatCommand)))
          {
            IMyChatCommand instance = (IMyChatCommand) Activator.CreateInstance((Type) definedType);
            this.ChatCommands.Add(instance.CommandText, instance);
          }
        }
        else
        {
          foreach (MethodInfo method in definedType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
          {
            ChatCommandAttribute customAttribute = method.GetCustomAttribute<ChatCommandAttribute>();
            if (customAttribute != null && !customAttribute.DebugCommand)
            {
              Action<string[]> action = method.CreateDelegate<Action<string[]>>();
              if (action == null)
                MyLog.Default.WriteLine("Error creating delegate from " + definedType.FullName + "." + method.Name);
              else
                this.ChatCommands.Add(customAttribute.CommandText, (IMyChatCommand) new MyChatCommand(customAttribute.CommandText, customAttribute.HelpText, customAttribute.HelpSimpleText, action));
            }
          }
        }
      }
    }

    public bool CanHandle(string message)
    {
      if (string.IsNullOrEmpty(message))
        return false;
      string[] strArray = message.Split(MyChatCommandSystem.m_separators, 2, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 0)
        return false;
      if (this.ChatCommands.ContainsKey(strArray[0]))
        return true;
      List<IMyChatCommand> executableCommands = new List<IMyChatCommand>();
      MyChatCommandSystem.HandleCommandDelegate unhandledCommand = this.OnUnhandledCommand;
      if (unhandledCommand != null)
        unhandledCommand(strArray[0], strArray.Length > 1 ? strArray[1] : "", executableCommands);
      return executableCommands.Count > 0;
    }

    public void Handle(string message)
    {
      if (string.IsNullOrEmpty(message))
        return;
      string[] strArray = message.Split(MyChatCommandSystem.m_separators, 2, StringSplitOptions.RemoveEmptyEntries);
      IMyChatCommand command1;
      if (!this.ChatCommands.TryGetValue(strArray[0], out command1))
      {
        List<IMyChatCommand> executableCommands = new List<IMyChatCommand>();
        MyChatCommandSystem.HandleCommandDelegate unhandledCommand = this.OnUnhandledCommand;
        if (unhandledCommand != null)
          unhandledCommand(strArray[0], strArray.Length > 1 ? strArray[1] : "", executableCommands);
        if (executableCommands.Count == 0)
          return;
        foreach (IMyChatCommand command2 in executableCommands)
          command2.Handle(MyChatCommandSystem.ParseCommand(command2, message));
      }
      else
      {
        string[] command2 = MyChatCommandSystem.ParseCommand(command1, message);
        command1.Handle(command2);
      }
    }

    public static string[] ParseCommand(IMyChatCommand command, string input)
    {
      if (input.Length <= command.CommandText.Length + 1)
        return (string[]) null;
      MatchCollection matchCollection = Regex.Matches(input.Substring(command.CommandText.Length + 1), "(\"[^\"]+\"|\\S+)");
      string[] strArray = new string[matchCollection.Count];
      for (int i = 0; i < strArray.Length; ++i)
        strArray[i] = matchCollection[i].Value;
      return strArray;
    }

    public delegate void HandleCommandDelegate(
      string command,
      string body,
      List<IMyChatCommand> executableCommands);
  }
}
