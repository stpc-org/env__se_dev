// Decompiled with JetBrains decompiler
// Type: VRage.MyConsole
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.Text;

namespace VRage
{
  public static class MyConsole
  {
    private static StringBuilder m_displayScreen = new StringBuilder();
    private static MyCommandHandler m_handler = new MyCommandHandler();
    private static LinkedList<string> m_commandHistory = new LinkedList<string>();
    private static LinkedListNode<string> m_position = (LinkedListNode<string>) null;

    public static StringBuilder DisplayScreen => MyConsole.m_displayScreen;

    public static void ParseCommand(string command)
    {
      if (MyConsole.m_position == null)
      {
        MyConsole.m_commandHistory.AddLast(command);
      }
      else
      {
        MyConsole.m_commandHistory.AddAfter(MyConsole.m_position, command);
        MyConsole.m_position = MyConsole.m_position.Next;
      }
      MyConsole.m_displayScreen.Append((object) MyConsole.m_handler.Handle(command)).AppendLine();
    }

    public static void PreviousLine()
    {
      if (MyConsole.m_position == null)
      {
        MyConsole.m_position = MyConsole.m_commandHistory.Last;
      }
      else
      {
        if (MyConsole.m_position == MyConsole.m_commandHistory.First)
          return;
        MyConsole.m_position = MyConsole.m_position.Previous;
      }
    }

    public static void NextLine()
    {
      if (MyConsole.m_position == null)
        return;
      MyConsole.m_position = MyConsole.m_position.Next;
    }

    public static string GetLine() => MyConsole.m_position == null ? "" : MyConsole.m_position.Value;

    public static void Clear() => MyConsole.m_displayScreen.Clear();

    public static void AddCommand(MyCommand command) => MyConsole.m_handler.AddCommand(command);

    public static bool TryGetCommand(string commandName, out MyCommand command) => MyConsole.m_handler.TryGetCommand(commandName, out command);
  }
}
