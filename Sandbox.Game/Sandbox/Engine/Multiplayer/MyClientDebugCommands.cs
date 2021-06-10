// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyClientDebugCommands
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  [PreloadRequired]
  public class MyClientDebugCommands
  {
    private static readonly char[] m_separators = new char[3]
    {
      ' ',
      '\r',
      '\n'
    };
    private static readonly Dictionary<string, Action<string[]>> m_commands = new Dictionary<string, Action<string[]>>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    private static ulong m_commandAuthor;

    static MyClientDebugCommands()
    {
      foreach (MethodInfo method in typeof (MyClientDebugCommands).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
      {
        DisplayNameAttribute customAttribute = method.GetCustomAttribute<DisplayNameAttribute>();
        ParameterInfo[] parameters = method.GetParameters();
        if (customAttribute != null && method.ReturnType == typeof (void) && (parameters.Length == 1 && parameters[0].ParameterType == typeof (string[])))
          MyClientDebugCommands.m_commands[customAttribute.DisplayName] = method.CreateDelegate<Action<string[]>>();
      }
    }

    public static bool Process(string msg, ulong author)
    {
      MyClientDebugCommands.m_commandAuthor = author;
      string[] strArray = msg.Split(MyClientDebugCommands.m_separators, StringSplitOptions.RemoveEmptyEntries);
      Action<string[]> action;
      if (strArray.Length == 0 || !MyClientDebugCommands.m_commands.TryGetValue(strArray[0], out action))
        return false;
      action(((IEnumerable<string>) strArray).Skip<string>(1).ToArray<string>());
      return true;
    }

    [DisplayName("+stress")]
    private static void StressTest(string[] args)
    {
      if (args.Length > 1)
      {
        if (!(args[0] == MySession.Static.LocalHumanPlayer.DisplayName) && !(args[0] == "all") && !(args[0] == "clients"))
          return;
        if (args.Length > 3)
        {
          MyReplicationClient.StressSleep.X = Convert.ToInt32(args[1]);
          MyReplicationClient.StressSleep.Y = Convert.ToInt32(args[2]);
          MyReplicationClient.StressSleep.Z = Convert.ToInt32(args[3]);
        }
        else if (args.Length > 2)
        {
          MyReplicationClient.StressSleep.X = Convert.ToInt32(args[1]);
          MyReplicationClient.StressSleep.Y = Convert.ToInt32(args[2]);
          MyReplicationClient.StressSleep.Z = 0;
        }
        else
        {
          MyReplicationClient.StressSleep.Y = Convert.ToInt32(args[1]);
          MyReplicationClient.StressSleep.X = MyReplicationClient.StressSleep.Y;
          MyReplicationClient.StressSleep.Z = 0;
        }
      }
      else
      {
        MyReplicationClient.StressSleep.X = 0;
        MyReplicationClient.StressSleep.Y = 0;
      }
    }

    [DisplayName("+vcadd")]
    private static void VirtualClientAdd(string[] args)
    {
      int num = 1;
      if (args.Length == 1)
        num = int.Parse(args[0]);
      int idx = 0;
      while (num > 0)
      {
        MySession.Static.VirtualClients.Add(idx);
        --num;
        ++idx;
      }
    }
  }
}
