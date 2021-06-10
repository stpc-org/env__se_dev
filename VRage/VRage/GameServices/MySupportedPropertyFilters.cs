// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MySupportedPropertyFilters
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VRage.GameServices
{
  public readonly struct MySupportedPropertyFilters : IReadOnlyList<MySupportedPropertyFilters.Entry>, IEnumerable<MySupportedPropertyFilters.Entry>, IEnumerable, IReadOnlyCollection<MySupportedPropertyFilters.Entry>
  {
    private readonly MySupportedPropertyFilters.Entry[] m_entries;
    public static readonly MySupportedPropertyFilters Empty = new MySupportedPropertyFilters(Array.Empty<MySupportedPropertyFilters.Entry>());

    public MySupportedPropertyFilters(
      IEnumerable<(string Property, MySearchConditionFlags SupportedConditions)> entries)
    {
      this.m_entries = entries.Select<(string, MySearchConditionFlags), MySupportedPropertyFilters.Entry>((Func<(string, MySearchConditionFlags), MySupportedPropertyFilters.Entry>) (x => (MySupportedPropertyFilters.Entry) ref x)).ToArray<MySupportedPropertyFilters.Entry>();
    }

    public MySupportedPropertyFilters(
      IEnumerable<MySupportedPropertyFilters.Entry> entries)
    {
      this.m_entries = entries.ToArray<MySupportedPropertyFilters.Entry>();
    }

    private MySupportedPropertyFilters(MySupportedPropertyFilters.Entry[] entries) => this.m_entries = entries;

    public int Count => this.m_entries.Length;

    public MySupportedPropertyFilters.Entry this[int index] => this.m_entries[index];

    public MySupportedPropertyFilters.Enumerator GetEnumerator() => new MySupportedPropertyFilters.Enumerator(this.m_entries);

    IEnumerator<MySupportedPropertyFilters.Entry> IEnumerable<MySupportedPropertyFilters.Entry>.GetEnumerator() => (IEnumerator<MySupportedPropertyFilters.Entry>) new MySupportedPropertyFilters.Enumerator(this.m_entries);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public readonly struct Entry
    {
      public readonly string Property;
      public readonly MySearchConditionFlags SupportedConditions;

      public Entry(string property, MySearchConditionFlags supportedConditions)
      {
        this.Property = property;
        this.SupportedConditions = supportedConditions;
      }

      public static implicit operator MySupportedPropertyFilters.Entry(
        in (string Property, MySearchConditionFlags SupportedConditions) tuple)
      {
        return new MySupportedPropertyFilters.Entry(tuple.Item1, tuple.Item2);
      }

      public void Deconstruct(out string property, out MySearchConditionFlags supportedConditions)
      {
        property = this.Property;
        supportedConditions = this.SupportedConditions;
      }
    }

    public struct Enumerator : IEnumerator<MySupportedPropertyFilters.Entry>, IEnumerator, IDisposable
    {
      private readonly MySupportedPropertyFilters.Entry[] m_entries;
      private int m_index;

      internal Enumerator(MySupportedPropertyFilters.Entry[] entries)
      {
        this.m_entries = entries;
        this.m_index = -1;
      }

      public bool MoveNext()
      {
        if (this.m_index >= this.m_entries.Length - 1)
          return false;
        ++this.m_index;
        return true;
      }

      public void Reset() => this.m_index = -1;

      public MySupportedPropertyFilters.Entry Current => this.m_entries[this.m_index];

      object IEnumerator.Current => (object) this.Current;

      public void Dispose()
      {
      }
    }
  }
}
