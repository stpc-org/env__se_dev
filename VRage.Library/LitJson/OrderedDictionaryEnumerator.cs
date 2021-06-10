// Decompiled with JetBrains decompiler
// Type: LitJson.OrderedDictionaryEnumerator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
  internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
  {
    private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;

    public object Current => (object) this.Entry;

    public DictionaryEntry Entry
    {
      get
      {
        KeyValuePair<string, JsonData> current = this.list_enumerator.Current;
        return new DictionaryEntry((object) current.Key, (object) current.Value);
      }
    }

    public object Key => (object) this.list_enumerator.Current.Key;

    public object Value => (object) this.list_enumerator.Current.Value;

    public OrderedDictionaryEnumerator(
      IEnumerator<KeyValuePair<string, JsonData>> enumerator)
    {
      this.list_enumerator = enumerator;
    }

    public bool MoveNext() => this.list_enumerator.MoveNext();

    public void Reset() => this.list_enumerator.Reset();
  }
}
