// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.HashSetExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace System.Collections.Generic
{
  public static class HashSetExtensions
  {
    public static T FirstElement<T>(this HashSet<T> hashset)
    {
      using (HashSet<T>.Enumerator enumerator = hashset.GetEnumerator())
        return enumerator.MoveNext() ? enumerator.Current : throw new InvalidOperationException();
    }
  }
}
