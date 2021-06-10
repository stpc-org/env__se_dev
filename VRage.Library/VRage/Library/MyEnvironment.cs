// Decompiled with JetBrains decompiler
// Type: VRage.Library.MyEnvironment
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library
{
  public static class MyEnvironment
  {
    public static bool Is64BitProcess => Environment.Is64BitProcess;

    public static string NewLine => Environment.NewLine;

    public static int ProcessorCount => Environment.ProcessorCount;

    public static int TickCount => Environment.TickCount;

    public static long WorkingSetForMyLog => Environment.WorkingSet;
  }
}
