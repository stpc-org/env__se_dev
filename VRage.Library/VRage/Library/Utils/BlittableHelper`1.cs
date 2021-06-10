// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.BlittableHelper`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Runtime.InteropServices;

namespace VRage.Library.Utils
{
  public static class BlittableHelper<T>
  {
    public static readonly bool IsBlittable;

    static BlittableHelper()
    {
      try
      {
        if ((object) default (T) == null)
          return;
        GCHandle.Alloc((object) default (T), GCHandleType.Pinned).Free();
        BlittableHelper<T>.IsBlittable = true;
      }
      catch
      {
      }
    }
  }
}
