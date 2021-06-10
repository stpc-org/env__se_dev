// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilder.MySerializableList`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.ObjectBuilder
{
  public class MySerializableList<TItem> : List<TItem>
  {
    public MySerializableList()
    {
    }

    public MySerializableList(int capacity)
      : base(capacity)
    {
    }

    public MySerializableList(IEnumerable<TItem> collection)
      : base(collection)
    {
    }

    public new void Add(TItem item)
    {
      if ((object) item == null)
        return;
      base.Add(item);
    }
  }
}
