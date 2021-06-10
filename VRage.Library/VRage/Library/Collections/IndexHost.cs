// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.IndexHost
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections.__helper_namespace;

namespace VRage.Library.Collections
{
  public class IndexHost
  {
    private static readonly ComponentIndex NullIndex = new ComponentIndex(new TypeList());
    private readonly Dictionary<ITypeList, WeakReference> m_indexes;

    public IndexHost()
    {
      this.m_indexes = new Dictionary<ITypeList, WeakReference>((IEqualityComparer<ITypeList>) new TypeListComparer());
      this.m_indexes[(ITypeList) IndexHost.NullIndex.Types] = new WeakReference((object) IndexHost.NullIndex);
    }

    private ComponentIndex GetForTypes(ITypeList types)
    {
      WeakReference weakReference;
      ComponentIndex componentIndex;
      if (!this.m_indexes.TryGetValue(types, out weakReference) || (componentIndex = (ComponentIndex) weakReference.Target) == null)
      {
        if (weakReference == null)
          weakReference = new WeakReference((object) null);
        TypeList solidified = types.GetSolidified();
        componentIndex = new ComponentIndex(solidified);
        weakReference.Target = (object) componentIndex;
        this.m_indexes[(ITypeList) solidified] = weakReference;
      }
      return componentIndex;
    }

    public ComponentIndex GetAfterInsert(
      ComponentIndex current,
      Type newType,
      out int insertionPoint)
    {
      insertionPoint = ~current.Types.BinarySearch(newType, (IComparer<Type>) TypeComparer.Instance);
      return this.GetForTypes(current.Types.With(insertionPoint, newType));
    }

    public ComponentIndex GetAfterRemove(
      ComponentIndex current,
      Type oldType,
      out int removalPoint)
    {
      removalPoint = current.Index[oldType];
      return this.GetForTypes(current.Types.Without(removalPoint));
    }

    public ComponentIndex GetEmptyComponentIndex() => IndexHost.NullIndex;
  }
}
