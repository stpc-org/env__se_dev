// Decompiled with JetBrains decompiler
// Type: VRage.Meta.MyAttributeIndexerBase`2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Meta
{
  public class MyAttributeIndexerBase<TAttribute, TKey> : IMyAttributeIndexer, IMyMetadataIndexer
    where TAttribute : Attribute, IMyKeyAttribute<TKey>
  {
    protected Dictionary<TKey, Type> IndexedTypes = new Dictionary<TKey, Type>();
    protected MyAttributeIndexerBase<TAttribute, TKey> Parent;
    public static MyAttributeIndexerBase<TAttribute, TKey> Static;

    public bool TryGetType(TKey key, out Type indexedType)
    {
      if (this.IndexedTypes.TryGetValue(key, out indexedType))
        return true;
      return this.Parent != null && this.Parent.TryGetType(key, out indexedType);
    }

    public virtual void SetParent(IMyMetadataIndexer indexer) => this.Parent = (MyAttributeIndexerBase<TAttribute, TKey>) indexer;

    public virtual void Activate() => MyAttributeIndexerBase<TAttribute, TKey>.Static = this;

    public virtual void Close() => this.IndexedTypes.Clear();

    public virtual void Process()
    {
    }

    public virtual void Observe(Attribute attribute, Type type) => this.Observe((TAttribute) attribute, type);

    protected virtual void Observe(TAttribute attribute, Type type) => this.IndexedTypes.Add(attribute.Key, type);
  }
}
