// Decompiled with JetBrains decompiler
// Type: VRage.Sync.SyncType
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Sync
{
  public class SyncType
  {
    private List<SyncBase> m_properties;
    private Action<SyncBase> m_registeredHandlers;

    public ListReader<SyncBase> Properties => new ListReader<SyncBase>(this.m_properties);

    public event Action<SyncBase> PropertyChanged
    {
      add
      {
        this.m_registeredHandlers += value;
        foreach (SyncBase property in this.m_properties)
          property.ValueChanged += value;
      }
      remove
      {
        foreach (SyncBase property in this.m_properties)
          property.ValueChanged -= value;
        this.m_registeredHandlers -= value;
      }
    }

    public event Action<SyncBase> PropertyChangedNotify
    {
      add
      {
        foreach (SyncBase property in this.m_properties)
          property.ValueChangedNotify += value;
      }
      remove
      {
        foreach (SyncBase property in this.m_properties)
          property.ValueChangedNotify -= value;
      }
    }

    public event Action PropertyCountChanged;

    public SyncType(List<SyncBase> properties) => this.m_properties = properties;

    public void Append(object obj)
    {
      int count = this.m_properties.Count;
      SyncHelpers.Compose(obj, this.m_properties.Count, this.m_properties);
      for (int index = count; index < this.m_properties.Count; ++index)
        this.m_properties[index].ValueChanged += this.m_registeredHandlers;
      this.PropertyCountChanged.InvokeIfNotNull();
    }
  }
}
