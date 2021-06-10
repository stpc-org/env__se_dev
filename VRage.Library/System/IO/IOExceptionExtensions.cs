// Decompiled with JetBrains decompiler
// Type: System.IO.IOExceptionExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Runtime.InteropServices;

namespace System.IO
{
  public static class IOExceptionExtensions
  {
    public static bool IsFileLocked(this IOException e)
    {
      int num = Marshal.GetHRForException((Exception) e) & (int) ushort.MaxValue;
      return num == 32 || num == 33;
    }
  }
}
