// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyMemoryProfiler
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  internal class MyMemoryProfiler
  {
    private static List<MyMemoryLogs.MyMemoryEvent> m_managed;
    private static List<MyMemoryLogs.MyMemoryEvent> m_native;
    private static List<MyMemoryLogs.MyMemoryEvent> m_timed;
    private static List<MyMemoryLogs.MyMemoryEvent> m_events;
    private static bool m_initialized = false;
    private static bool Enabled = false;
    private static Vector2 GraphOffset = new Vector2(0.1f, 0.5f);
    private static Vector2 GraphSize = new Vector2(0.8f, -0.3f);

    private static void SaveSnapshot()
    {
    }

    private static MyMemoryLogs.MyMemoryEvent GetEventFromCursor(Vector2 screenPosition)
    {
      Vector2 vector2 = screenPosition;
      for (int index = 0; index < MyMemoryProfiler.m_events.Count; ++index)
      {
        float totalSeconds1 = (float) (MyMemoryProfiler.m_events[index].StartTime - MyMemoryProfiler.m_events[0].StartTime).TotalSeconds;
        float totalSeconds2 = (float) (MyMemoryProfiler.m_events[index].EndTime - MyMemoryProfiler.m_events[0].StartTime).TotalSeconds;
        if ((double) vector2.X >= (double) totalSeconds1 && (double) vector2.X <= (double) totalSeconds2 && ((double) vector2.Y >= 0.0 && (double) vector2.Y <= (double) MyMemoryProfiler.m_events[index].ProcessEndSize))
          return MyMemoryProfiler.m_events[index];
      }
      return (MyMemoryLogs.MyMemoryEvent) null;
    }

    public static void Draw()
    {
      if (!MyMemoryProfiler.m_initialized)
      {
        MyMemoryProfiler.m_managed = MyMemoryLogs.GetManaged();
        MyMemoryProfiler.m_native = MyMemoryLogs.GetNative();
        MyMemoryProfiler.m_timed = MyMemoryLogs.GetTimed();
        MyMemoryProfiler.m_events = MyMemoryLogs.GetEvents();
        MyMemoryProfiler.m_initialized = true;
      }
      float x = 0.0f;
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (MyMemoryProfiler.m_events.Count > 0)
        x = (float) (MyMemoryProfiler.m_events[MyMemoryProfiler.m_events.Count - 1].EndTime - MyMemoryProfiler.m_events[0].StartTime).TotalSeconds;
      for (int index = 0; index < MyMemoryProfiler.m_events.Count; ++index)
      {
        num2 += MyMemoryProfiler.m_events[index].ProcessDelta;
        num1 = Math.Max(Math.Max(num1, MyMemoryProfiler.m_events[index].ProcessStartSize), MyMemoryProfiler.m_events[index].ProcessEndSize);
      }
      MyMemoryLogs.MyMemoryEvent eventFromCursor = MyMemoryProfiler.GetEventFromCursor((MyGuiSandbox.MouseCursorPosition - MyMemoryProfiler.GraphOffset) * new Vector2(MyMemoryProfiler.GraphSize.X, MyMemoryProfiler.GraphSize.Y) * new Vector2(x, num1));
      if (eventFromCursor != null)
        new StringBuilder(100).Append(eventFromCursor.Name);
      float num3 = (double) num1 > 0.0 ? 1f / num1 : 0.0f;
      float num4 = (double) x > 0.0 ? 1f / x : 0.0f;
      int num5 = 0;
      foreach (MyMemoryLogs.MyMemoryEvent myMemoryEvent in MyMemoryProfiler.m_events)
      {
        float totalSeconds1 = (float) (myMemoryEvent.StartTime - MyMemoryProfiler.m_events[0].StartTime).TotalSeconds;
        double totalSeconds2 = (myMemoryEvent.EndTime - MyMemoryProfiler.m_events[0].StartTime).TotalSeconds;
        double managedStartSize = (double) myMemoryEvent.ManagedStartSize;
        double managedEndSize = (double) myMemoryEvent.ManagedEndSize;
        double processStartSize = (double) myMemoryEvent.ProcessStartSize;
        double processEndSize = (double) myMemoryEvent.ProcessEndSize;
        if (num5 % 2 != 1)
        {
          Color lightGreen = Color.LightGreen;
        }
        else
        {
          Color green = Color.Green;
        }
        if (num5++ % 2 != 1)
        {
          Color lightBlue = Color.LightBlue;
        }
        else
        {
          Color blue = Color.Blue;
        }
        if (myMemoryEvent == eventFromCursor)
        {
          Color yellow = Color.Yellow;
          Color orange = Color.Orange;
        }
      }
      StringBuilder stringBuilder1 = new StringBuilder();
      Vector2 vector2 = new Vector2(100f, 500f);
      for (int index = 0; index < 50 && index < MyMemoryProfiler.m_native.Count; ++index)
      {
        stringBuilder1.Clear();
        stringBuilder1.Append(MyMemoryProfiler.m_native[index].Name);
        StringBuilder stringBuilder2 = stringBuilder1;
        float num6 = 9.536743E-07f * MyMemoryProfiler.m_native[index].ManagedDelta;
        string str1 = num6.ToString("GC: 0.0 MB ");
        stringBuilder2.Append(str1);
        stringBuilder1.Clear();
        StringBuilder stringBuilder3 = stringBuilder1;
        num6 = 9.536743E-07f * MyMemoryProfiler.m_native[index].ProcessDelta;
        string str2 = num6.ToString("Process: 0.0 MB ");
        stringBuilder3.Append(str2);
        vector2.Y += 13f;
      }
      vector2 = new Vector2(1000f, 500f);
      vector2.Y += 10f;
      for (int index = 0; index < 50 && index < MyMemoryProfiler.m_timed.Count; ++index)
      {
        stringBuilder1.Clear();
        stringBuilder1.Append(MyMemoryProfiler.m_native[index].Name);
        stringBuilder1.Append(MyMemoryProfiler.m_timed[index].DeltaTime.ToString(" 0.000 s"));
        vector2.Y += 13f;
      }
    }
  }
}
