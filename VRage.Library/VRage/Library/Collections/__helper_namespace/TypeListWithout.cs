// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.__helper_namespace.TypeListWithout
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Collections.__helper_namespace
{
  internal class TypeListWithout : ITypeList
  {
    private TypeList m_list;
    private int m_index;

    public void Set(TypeList source, int removalIndex)
    {
      this.m_list = source;
      this.m_index = removalIndex;
      this.HashCode = this.ComputeHashCode();
    }

    public int Count => this.m_list.Count - 1;

    public Type this[int index] => index < this.m_index ? this.m_list[index] : this.m_list[index + 1];

    public int HashCode { get; private set; }

    public TypeList GetSolidified()
    {
      TypeList typeList = new TypeList();
      typeList.Capacity = this.Count;
      typeList.AddRange(this.m_list.Take<Type>(this.m_index));
      typeList.AddRange(this.m_list.Skip<Type>(this.m_index + 1));
      typeList.UpdateHashCode();
      return typeList;
    }
  }
}
