// Decompiled with JetBrains decompiler
// Type: System.DelegateExtensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Library;

namespace System
{
  public static class DelegateExtensions
  {
    private const bool ProfileDelegateInvocations = false;
    private static Func<object, int> m_getInvocationCount;
    private static Func<object, object[]> m_getInvocationList;
    [ThreadStatic]
    private static List<Delegate> m_invocationList;
    private const bool __Dummy_ = false;
    private static Action<string> m_profilerBegin = (Action<string>) (x => {});
    private static Action<int> m_profilerEnd = (Action<int>) (x => {});
    private static ConcurrentDictionary<MethodInfo, string> m_methodNameCache = new ConcurrentDictionary<MethodInfo, string>(MyEnvironment.ProcessorCount * 4, 100);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvokeIfNotNull(this Action handler)
    {
      if (handler == null)
        return;
      handler();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvokeIfNotNull<T1>(this Action<T1> handler, T1 arg1)
    {
      if (handler == null)
        return;
      handler(arg1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvokeIfNotNull<T1, T2>(this Action<T1, T2> handler, T1 arg1, T2 arg2)
    {
      if (handler == null)
        return;
      handler(arg1, arg2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvokeIfNotNull<T1, T2, T3>(
      this Action<T1, T2, T3> handler,
      T1 arg1,
      T2 arg2,
      T3 arg3)
    {
      if (handler == null)
        return;
      handler(arg1, arg2, arg3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvokeIfNotNull<T1, T2, T3, T4>(
      this Action<T1, T2, T3, T4> handler,
      T1 arg1,
      T2 arg2,
      T3 arg3,
      T4 arg4)
    {
      if (handler == null)
        return;
      handler(arg1, arg2, arg3, arg4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvokeIfNotNull<T1, T2, T3, T4, T5>(
      this Action<T1, T2, T3, T4, T5> handler,
      T1 arg1,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5)
    {
      if (handler == null)
        return;
      handler(arg1, arg2, arg3, arg4, arg5);
    }

    public static void GetInvocationList(this MulticastDelegate @delegate, List<Delegate> result)
    {
      if (DelegateExtensions.m_getInvocationList == null)
      {
        FieldInfo field = typeof (MulticastDelegate).GetField("_invocationList", BindingFlags.Instance | BindingFlags.NonPublic);
        if (field == (FieldInfo) null)
          field = typeof (MulticastDelegate).GetField("delegates", BindingFlags.Instance | BindingFlags.NonPublic);
        Func<object, object> getter = FieldAccess.CreateGetter<object, object>(field);
        DelegateExtensions.m_getInvocationList = (Func<object, object[]>) (x => getter(x) as object[]);
      }
      if (DelegateExtensions.m_getInvocationCount == null)
      {
        FieldInfo field = typeof (MulticastDelegate).GetField("_invocationCount", BindingFlags.Instance | BindingFlags.NonPublic);
        if (field != (FieldInfo) null)
        {
          Func<object, IntPtr> getter = FieldAccess.CreateGetter<object, IntPtr>(field);
          DelegateExtensions.m_getInvocationCount = (Func<object, int>) (x => (int) getter(x));
        }
        else
          DelegateExtensions.m_getInvocationCount = (Func<object, int>) (x => DelegateExtensions.m_getInvocationList(x).Length);
      }
      object[] objArray = DelegateExtensions.m_getInvocationList((object) @delegate);
      if (objArray == null)
      {
        result.Add((Delegate) @delegate);
      }
      else
      {
        int num = DelegateExtensions.m_getInvocationCount((object) @delegate);
        for (int index = 0; index < num; ++index)
          result.Add((Delegate) objArray[index]);
      }
    }

    public static void SetupProfiler(Action<string> begin, Action<int> end)
    {
      DelegateExtensions.m_profilerBegin = begin;
      DelegateExtensions.m_profilerEnd = end;
    }

    private static void InvokeProfiled<TDelegate, T1, T2, T3, T4, T5>(
      Action<TDelegate, T1, T2, T3, T4, T5> handler,
      TDelegate @delegate,
      T1 _1,
      T2 _2,
      T3 _3,
      T4 _4,
      T5 _5)
    {
      if (DelegateExtensions.m_invocationList == null)
        DelegateExtensions.m_invocationList = new List<Delegate>();
      int count1 = DelegateExtensions.m_invocationList.Count;
      DelegateExtensions.m_profilerBegin("DelegateInvoke");
      try
      {
        ((MulticastDelegate) (object) @delegate).GetInvocationList(DelegateExtensions.m_invocationList);
        int count2 = DelegateExtensions.m_invocationList.Count;
        for (int index = count1; index < count2; ++index)
        {
          Delegate invocation = DelegateExtensions.m_invocationList[index];
          DelegateExtensions.m_profilerBegin(DelegateExtensions.GetMethodName(invocation.Method));
          try
          {
            handler((TDelegate) invocation, _1, _2, _3, _4, _5);
          }
          finally
          {
            DelegateExtensions.m_profilerEnd(0);
          }
        }
      }
      finally
      {
        int count2 = DelegateExtensions.m_invocationList.Count - count1;
        DelegateExtensions.m_invocationList.RemoveRange(count1, count2);
        DelegateExtensions.m_profilerEnd(count2);
      }
    }

    private static string GetMethodName(MethodInfo method)
    {
      string orAdd;
      if (!DelegateExtensions.m_methodNameCache.TryGetValue(method, out orAdd))
      {
        string str1 = method.IsStatic ? "." : "#";
        string str2 = method.DeclaringType.Name + str1 + method.Name;
        orAdd = DelegateExtensions.m_methodNameCache.GetOrAdd(method, str2);
      }
      return orAdd;
    }
  }
}
