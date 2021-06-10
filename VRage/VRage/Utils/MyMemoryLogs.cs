// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyMemoryLogs
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Utils
{
  public class MyMemoryLogs
  {
    private static MyMemoryLogs.MyManagedComparer m_managedComparer = new MyMemoryLogs.MyManagedComparer();
    private static MyMemoryLogs.MyNativeComparer m_nativeComparer = new MyMemoryLogs.MyNativeComparer();
    private static MyMemoryLogs.MyTimedeltaComparer m_timeComparer = new MyMemoryLogs.MyTimedeltaComparer();
    private static List<MyMemoryLogs.MyMemoryEvent> m_events = new List<MyMemoryLogs.MyMemoryEvent>();
    private static List<string> m_consoleLogSTART = new List<string>();
    private static List<string> m_consoleLogEND = new List<string>();
    private static Stack<MyMemoryLogs.MyMemoryEvent> m_stack = new Stack<MyMemoryLogs.MyMemoryEvent>();
    private static int IdCounter = 1;

    public static List<MyMemoryLogs.MyMemoryEvent> GetManaged()
    {
      List<MyMemoryLogs.MyMemoryEvent> myMemoryEventList = new List<MyMemoryLogs.MyMemoryEvent>((IEnumerable<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_events);
      myMemoryEventList.Sort((IComparer<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_managedComparer);
      return myMemoryEventList;
    }

    public static List<MyMemoryLogs.MyMemoryEvent> GetNative()
    {
      List<MyMemoryLogs.MyMemoryEvent> myMemoryEventList = new List<MyMemoryLogs.MyMemoryEvent>((IEnumerable<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_events);
      myMemoryEventList.Sort((IComparer<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_nativeComparer);
      return myMemoryEventList;
    }

    public static List<MyMemoryLogs.MyMemoryEvent> GetTimed()
    {
      List<MyMemoryLogs.MyMemoryEvent> myMemoryEventList = new List<MyMemoryLogs.MyMemoryEvent>((IEnumerable<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_events);
      myMemoryEventList.Sort((IComparer<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_timeComparer);
      return myMemoryEventList;
    }

    public static List<MyMemoryLogs.MyMemoryEvent> GetEvents() => MyMemoryLogs.m_events;

    public static void StartEvent()
    {
      MyMemoryLogs.MyMemoryEvent myMemoryEvent = new MyMemoryLogs.MyMemoryEvent();
      if (MyMemoryLogs.m_consoleLogSTART.Count <= 0)
        return;
      myMemoryEvent.Name = MyMemoryLogs.m_consoleLogSTART[MyMemoryLogs.m_consoleLogSTART.Count - 1];
      myMemoryEvent.Id = MyMemoryLogs.IdCounter++;
      myMemoryEvent.StartTime = DateTime.Now;
      MyMemoryLogs.m_consoleLogSTART.Clear();
      MyMemoryLogs.m_stack.Push(myMemoryEvent);
    }

    public static void EndEvent(MyMemoryLogs.MyMemoryEvent ev)
    {
      if (MyMemoryLogs.m_stack.Count <= 0)
        return;
      MyMemoryLogs.MyMemoryEvent myMemoryEvent = MyMemoryLogs.m_stack.Peek();
      ev.Name = myMemoryEvent.Name;
      ev.Id = myMemoryEvent.Id;
      ev.StartTime = myMemoryEvent.StartTime;
      ev.EndTime = DateTime.Now;
      MyMemoryLogs.m_events.Add(ev);
      MyMemoryLogs.m_stack.Pop();
    }

    public static void AddConsoleLine(string line)
    {
      if (line.EndsWith("START"))
      {
        MyMemoryLogs.m_consoleLogEND.Clear();
        line = line.Substring(0, line.Length - 5);
        if (MyMemoryLogs.m_stack.Count > 0 && MyMemoryLogs.m_stack.Peek().HasStart)
        {
          MyMemoryLogs.m_events[MyMemoryLogs.m_events.Count].HasStart = true;
          MyMemoryLogs.m_events[MyMemoryLogs.m_events.Count].Name = line;
        }
        else
          MyMemoryLogs.m_consoleLogSTART.Add(line);
      }
      else
      {
        if (!line.EndsWith("END"))
          return;
        line = line.Substring(0, line.Length - 5);
        MyMemoryLogs.m_consoleLogEND.Add(line);
        MyMemoryLogs.m_consoleLogSTART.Clear();
      }
    }

    public static void DumpMemoryUsage()
    {
      MyMemoryLogs.m_events.Sort((IComparer<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_managedComparer);
      MyLog.Default.WriteLine("\n\n");
      MyLog.Default.WriteLine("Managed MemoryUsage: \n");
      float num1 = 0.0f;
      for (int index = 0; index < MyMemoryLogs.m_events.Count && index < 30; ++index)
      {
        float num2 = (float) ((double) MyMemoryLogs.m_events[index].ManagedDelta * 1.0 / 1048576.0);
        num1 += num2;
        MyLog.Default.WriteLine(MyMemoryLogs.m_events[index].Name + num2.ToString());
      }
      MyLog.Default.WriteLine("Total Managed MemoryUsage: " + (object) num1 + " [MB]");
      MyMemoryLogs.m_events.Sort((IComparer<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_nativeComparer);
      MyLog.Default.WriteLine("\n\n");
      MyLog.Default.WriteLine("Process MemoryUsage: \n");
      float num3 = 0.0f;
      for (int index = 0; index < MyMemoryLogs.m_events.Count && index < 30; ++index)
      {
        float num2 = (float) ((double) MyMemoryLogs.m_events[index].ProcessDelta * 1.0 / 1048576.0);
        num3 += num2;
        MyLog.Default.WriteLine(MyMemoryLogs.m_events[index].Name + num2.ToString());
      }
      MyLog.Default.WriteLine("Total Process MemoryUsage: " + (object) num3 + " [MB]");
      MyMemoryLogs.m_events.Sort((IComparer<MyMemoryLogs.MyMemoryEvent>) MyMemoryLogs.m_timeComparer);
      MyLog.Default.WriteLine("\n\n");
      MyLog.Default.WriteLine("Load time comparison: \n");
      float num4 = 0.0f;
      for (int index = 0; index < MyMemoryLogs.m_events.Count && index < 30; ++index)
      {
        float deltaTime = MyMemoryLogs.m_events[index].DeltaTime;
        num4 += deltaTime;
        MyLog.Default.WriteLine(MyMemoryLogs.m_events[index].Name + " " + deltaTime.ToString());
      }
      MyLog.Default.WriteLine("Total load time: " + (object) num4 + " [s]");
    }

    public class MyMemoryEvent
    {
      public string Name;
      public bool HasStart;
      public bool HasEnd;
      public float ManagedStartSize;
      public float ManagedEndSize;
      public float ProcessStartSize;
      public float ProcessEndSize;
      public float DeltaTime;
      public int Id;
      public bool Selected;
      public DateTime StartTime;
      public DateTime EndTime;
      private List<MyMemoryLogs.MyMemoryEvent> m_childs = new List<MyMemoryLogs.MyMemoryEvent>();

      public float ManagedDelta => this.ManagedEndSize - this.ManagedStartSize;

      public float ProcessDelta => this.ProcessEndSize - this.ProcessStartSize;
    }

    private class MyManagedComparer : IComparer<MyMemoryLogs.MyMemoryEvent>
    {
      public int Compare(MyMemoryLogs.MyMemoryEvent x, MyMemoryLogs.MyMemoryEvent y) => -1 * x.ManagedDelta.CompareTo(y.ManagedDelta);
    }

    private class MyNativeComparer : IComparer<MyMemoryLogs.MyMemoryEvent>
    {
      public int Compare(MyMemoryLogs.MyMemoryEvent x, MyMemoryLogs.MyMemoryEvent y) => -1 * x.ProcessDelta.CompareTo(y.ProcessDelta);
    }

    private class MyTimedeltaComparer : IComparer<MyMemoryLogs.MyMemoryEvent>
    {
      public int Compare(MyMemoryLogs.MyMemoryEvent x, MyMemoryLogs.MyMemoryEvent y) => -1 * x.DeltaTime.CompareTo(y.DeltaTime);
    }
  }
}
