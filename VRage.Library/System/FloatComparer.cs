// Decompiled with JetBrains decompiler
// Type: System.FloatComparer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace System
{
  public class FloatComparer : IComparer<float>
  {
    public static FloatComparer Instance = new FloatComparer();

    public int Compare(float x, float y) => x.CompareTo(y);
  }
}
