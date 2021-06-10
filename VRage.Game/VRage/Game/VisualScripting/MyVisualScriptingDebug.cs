// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyVisualScriptingDebug
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Game.VisualScripting.Missions;

namespace VRage.Game.VisualScripting
{
  public static class MyVisualScriptingDebug
  {
    private static List<MyDebuggingNodeLog> m_nodes = new List<MyDebuggingNodeLog>();
    private static List<MyDebuggingStateMachine> m_stateMachines = new List<MyDebuggingStateMachine>();

    public static void LogNode(int nodeID, params object[] values)
    {
    }

    public static void LogStateMachine(MyVSStateMachine machine)
    {
    }

    public static IReadOnlyList<MyDebuggingNodeLog> LoggedNodes => (IReadOnlyList<MyDebuggingNodeLog>) MyVisualScriptingDebug.m_nodes;

    public static IReadOnlyList<MyDebuggingStateMachine> StateMachines => (IReadOnlyList<MyDebuggingStateMachine>) MyVisualScriptingDebug.m_stateMachines;

    public static void Clear()
    {
      MyVisualScriptingDebug.m_nodes.Clear();
      MyVisualScriptingDebug.m_stateMachines.Clear();
    }
  }
}
