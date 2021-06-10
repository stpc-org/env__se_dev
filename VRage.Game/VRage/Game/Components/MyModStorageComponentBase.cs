// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyModStorageComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Serialization;

namespace VRage.Game.Components
{
  public abstract class MyModStorageComponentBase : MyEntityComponentBase, IDictionary<Guid, string>, ICollection<KeyValuePair<Guid, string>>, IEnumerable<KeyValuePair<Guid, string>>, IEnumerable
  {
    protected IDictionary<Guid, string> m_storageData = (IDictionary<Guid, string>) new Dictionary<Guid, string>();

    public override string ComponentTypeDebugString => "Mod Storage";

    public abstract string GetValue(Guid guid);

    public abstract bool TryGetValue(Guid guid, out string value);

    public abstract void SetValue(Guid guid, string value);

    public abstract bool RemoveValue(Guid guid);

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_ModStorageComponent storageComponent = (MyObjectBuilder_ModStorageComponent) base.Serialize(copy);
      storageComponent.Storage = new SerializableDictionary<Guid, string>((Dictionary<Guid, string>) this.m_storageData);
      return (MyObjectBuilder_ComponentBase) storageComponent;
    }

    public string this[Guid key]
    {
      get => this.GetValue(key);
      set => this.SetValue(key, value);
    }

    public int Count => this.m_storageData.Count;

    public bool IsReadOnly => this.m_storageData.IsReadOnly;

    public ICollection<Guid> Keys => this.m_storageData.Keys;

    public ICollection<string> Values => this.m_storageData.Values;

    public void Add(KeyValuePair<Guid, string> item) => this.m_storageData.Add(item);

    public void Add(Guid key, string value) => this.SetValue(key, value);

    public void Clear() => this.m_storageData.Clear();

    public bool Contains(KeyValuePair<Guid, string> item) => this.m_storageData.Contains(item);

    public bool ContainsKey(Guid key) => this.m_storageData.ContainsKey(key);

    public void CopyTo(KeyValuePair<Guid, string>[] array, int arrayIndex) => this.m_storageData.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<Guid, string>> GetEnumerator() => this.m_storageData.GetEnumerator();

    public bool Remove(KeyValuePair<Guid, string> item) => ((ICollection<KeyValuePair<Guid, string>>) this.m_storageData).Remove(item);

    public bool Remove(Guid key) => this.RemoveValue(key);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.m_storageData.GetEnumerator();
  }
}
