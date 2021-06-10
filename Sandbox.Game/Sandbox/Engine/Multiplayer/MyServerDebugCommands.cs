// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyServerDebugCommands
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
  [PreloadRequired]
  public class MyServerDebugCommands
  {
    private static readonly char[] m_separators = new char[3]
    {
      ' ',
      '\r',
      '\n'
    };
    private static readonly Dictionary<string, Action<string[]>> m_commands = new Dictionary<string, Action<string[]>>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    private static ulong m_commandAuthor;

    private static MyReplicationServer Replication => (MyReplicationServer) MyMultiplayer.Static.ReplicationLayer;

    static MyServerDebugCommands()
    {
      foreach (MethodInfo method in typeof (MyServerDebugCommands).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
      {
        DisplayNameAttribute customAttribute = method.GetCustomAttribute<DisplayNameAttribute>();
        ParameterInfo[] parameters = method.GetParameters();
        if (customAttribute != null && method.ReturnType == typeof (void) && (parameters.Length == 1 && parameters[0].ParameterType == typeof (string[])))
          MyServerDebugCommands.m_commands[customAttribute.DisplayName] = method.CreateDelegate<Action<string[]>>();
      }
    }

    public static bool Process(string msg, ulong author)
    {
      MyServerDebugCommands.m_commandAuthor = author;
      string[] strArray = msg.Split(MyServerDebugCommands.m_separators, StringSplitOptions.RemoveEmptyEntries);
      Action<string[]> action;
      if (strArray.Length == 0 || !MyServerDebugCommands.m_commands.TryGetValue(strArray[0], out action))
        return false;
      action(((IEnumerable<string>) strArray).Skip<string>(1).ToArray<string>());
      return true;
    }

    [DisplayName("+stress")]
    private static void StressTest(string[] args)
    {
      if (args.Length > 1)
      {
        if (!(args[0] == "server") && !(args[0] == "all"))
          return;
        if (args.Length > 3)
        {
          MyReplicationServer.StressSleep.X = Convert.ToInt32(args[1]);
          MyReplicationServer.StressSleep.Y = Convert.ToInt32(args[2]);
          MyReplicationServer.StressSleep.Z = Convert.ToInt32(args[3]);
        }
        else if (args.Length > 2)
        {
          MyReplicationServer.StressSleep.X = Convert.ToInt32(args[1]);
          MyReplicationServer.StressSleep.Y = Convert.ToInt32(args[2]);
          MyReplicationServer.StressSleep.Z = 0;
        }
        else
        {
          MyReplicationServer.StressSleep.X = Convert.ToInt32(args[1]);
          MyReplicationServer.StressSleep.Y = MyReplicationServer.StressSleep.X;
          MyReplicationServer.StressSleep.Z = 0;
        }
      }
      else
      {
        MyReplicationServer.StressSleep.X = 0;
        MyReplicationServer.StressSleep.Y = 0;
      }
    }

    [DisplayName("+dump")]
    private static void Dump(string[] args) => MySession.InitiateDump();

    [DisplayName("+save")]
    private static void Save(string[] args)
    {
      MySandboxGame.Log.WriteLineAndConsole("Executing +save command");
      MyAsyncSaving.Start();
    }

    [DisplayName("+stop")]
    private static void Stop(string[] args)
    {
      MySandboxGame.Log.WriteLineAndConsole("Executing +stop command");
      MySandboxGame.ExitThreadSafe();
    }

    [DisplayName("+unban")]
    private static void Unban(string[] args)
    {
      if (args.Length == 0)
        return;
      ulong result = 0;
      if (!ulong.TryParse(args[0], out result))
        return;
      MyMultiplayer.Static.BanClient(result, false);
    }

    [DisplayName("+resetplayers")]
    private static void ResetPlayers(string[] args)
    {
      Vector3D zero = Vector3D.Zero;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        MatrixD translation = MatrixD.CreateTranslation(zero);
        entity.PositionComp.SetWorldMatrix(ref translation);
        entity.Physics.LinearVelocity = (Vector3) Vector3D.Forward;
        zero.X += 50.0;
      }
    }

    [DisplayName("+forcereorder")]
    private static void ForceReorder(string[] args) => MyPhysics.ForceClustersReorder();
  }
}
