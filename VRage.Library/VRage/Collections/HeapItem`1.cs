// Decompiled with JetBrains decompiler
// Type: VRage.Collections.HeapItem`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Collections
{
  public abstract class HeapItem<K>
  {
    public int HeapIndex { get; internal set; }

    public K HeapKey { get; internal set; }
  }
}
