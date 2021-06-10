// Decompiled with JetBrains decompiler
// Type: VRage.Trace.MyWintraceWrapper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using VRage.FileSystem;

namespace VRage.Trace
{
  internal class MyWintraceWrapper : ITrace
  {
    private static readonly Type m_winTraceType;
    private static Type m_ttraceType;
    private static readonly object m_winWatches;
    private readonly object m_trace;
    private readonly Action m_clearAll;
    private readonly Action<string, object> m_send;
    private readonly Action<string, string> m_debugSend;

    static MyWintraceWrapper()
    {
      Assembly assembly1 = MyWintraceWrapper.TryLoad("TraceTool.dll");
      if ((object) assembly1 == null)
      {
        Assembly assembly2 = MyWintraceWrapper.TryLoad(MyFileSystem.ExePath + "/../../../../../../3rd/TraceTool/TraceTool.dll");
        if ((object) assembly2 == null)
        {
          Assembly assembly3 = MyWintraceWrapper.TryLoad(MyFileSystem.ExePath + "/../../../3rd/TraceTool/TraceTool.dll");
          assembly1 = (object) assembly3 != null ? assembly3 : MyWintraceWrapper.TryLoad(Environment.CurrentDirectory + "/../../../../../3rd/TraceTool/TraceTool.dll");
        }
        else
          assembly1 = assembly2;
      }
      Assembly assembly4 = assembly1;
      if (!(assembly4 != (Assembly) null))
        return;
      MyWintraceWrapper.m_winTraceType = assembly4.GetType("TraceTool.WinTrace");
      MyWintraceWrapper.m_ttraceType = assembly4.GetType("TraceTool.TTrace");
      MyWintraceWrapper.m_winWatches = MyWintraceWrapper.m_ttraceType.GetProperty("Watches").GetGetMethod().Invoke((object) null, new object[0]);
    }

    private static Assembly TryLoad(string assembly)
    {
      if (!File.Exists(assembly))
        return (Assembly) null;
      try
      {
        return Assembly.LoadFrom(assembly);
      }
      catch (Exception ex)
      {
        return (Assembly) null;
      }
    }

    public static ITrace CreateTrace(string id, string name)
    {
      if (!(MyWintraceWrapper.m_winTraceType != (Type) null))
        return (ITrace) new MyNullTrace();
      return (ITrace) new MyWintraceWrapper(Activator.CreateInstance(MyWintraceWrapper.m_winTraceType, (object) id, (object) name));
    }

    private MyWintraceWrapper(object trace)
    {
      this.m_trace = trace;
      this.m_clearAll = ((Expression<Action>) (() => Expression.Call(this.m_trace, trace.GetType().GetMethod("ClearAll")))).Compile();
      this.m_clearAll();
      this.m_send = ((Expression<Action<string, object>>) ((str, obj) => Expression.Call(MyWintraceWrapper.m_winWatches, MyWintraceWrapper.m_winWatches.GetType().GetMethod("Send"), str, obj))).Compile();
      ParameterExpression parameterExpression3 = Expression.Parameter(typeof (string));
      ParameterExpression parameterExpression4 = Expression.Parameter(typeof (string));
      MemberExpression memberExpression = Expression.PropertyOrField((Expression) Expression.Constant(this.m_trace), "Debug");
      this.m_debugSend = ((Expression<Action<string, string>>) ((parameterExpression1, parameterExpression2) => Expression.Call((Expression) memberExpression, memberExpression.Expression.Type.GetMethod("Send", new Type[2]
      {
        typeof (string),
        typeof (string)
      }), parameterExpression1, parameterExpression2))).Compile();
    }

    public void Send(string msg, string comment = null)
    {
      if (!this.Enabled)
        return;
      try
      {
        this.m_debugSend(msg, comment);
      }
      catch
      {
      }
    }

    public void Flush() => MyWintraceWrapper.m_ttraceType.GetMethod(nameof (Flush)).Invoke((object) null, (object[]) null);

    public bool Enabled { get; set; } = true;

    public void Watch(string name, object value)
    {
      if (!this.Enabled)
        return;
      try
      {
        this.m_send(name, value);
      }
      catch
      {
      }
    }
  }
}
