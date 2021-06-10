// Decompiled with JetBrains decompiler
// Type: VRage.Exceptions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;

namespace VRage
{
  public static class Exceptions
  {
    [DebuggerStepThrough]
    public static void ThrowIf<TException>(bool condition) where TException : Exception
    {
      if (condition)
        throw (Exception) Activator.CreateInstance(typeof (TException));
    }

    [DebuggerStepThrough]
    public static void ThrowIf<TException>(bool condition, string arg1) where TException : Exception
    {
      if (condition)
        throw (Exception) Activator.CreateInstance(typeof (TException), (object) arg1);
    }

    [DebuggerStepThrough]
    public static void ThrowIf<TException>(bool condition, string arg1, string arg2) where TException : Exception
    {
      if (condition)
        throw (Exception) Activator.CreateInstance(typeof (TException), (object) arg1, (object) arg2);
    }

    [DebuggerStepThrough]
    public static void ThrowIf<TException>(bool condition, params object[] args) where TException : Exception
    {
      if (condition)
        throw (Exception) Activator.CreateInstance(typeof (TException), args);
    }

    [DebuggerStepThrough]
    public static void ThrowAny<TException>(bool[] conditions, params object[] args) where TException : Exception
    {
      for (uint index = 0; (long) index < (long) conditions.Length; ++index)
      {
        if (conditions[(int) index])
          throw (Exception) Activator.CreateInstance(typeof (TException), args);
      }
    }

    [DebuggerStepThrough]
    public static void ThrowAll<TException>(bool[] conditions, params object[] args) where TException : Exception
    {
      for (uint index = 0; (long) index < (long) conditions.Length; ++index)
      {
        if (!conditions[(int) index])
          return;
      }
      throw (Exception) Activator.CreateInstance(typeof (TException), args);
    }
  }
}
