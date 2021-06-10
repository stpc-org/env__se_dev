// Decompiled with JetBrains decompiler
// Type: LitJson.JsonMockWrapper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
  public class JsonMockWrapper : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
  {
    public bool IsArray => false;

    public bool IsBoolean => false;

    public bool IsDouble => false;

    public bool IsInt => false;

    public bool IsLong => false;

    public bool IsObject => false;

    public bool IsString => false;

    public bool GetBoolean() => false;

    public double GetDouble() => 0.0;

    public int GetInt() => 0;

    public JsonType GetJsonType() => JsonType.None;

    public long GetLong() => 0;

    public string GetString() => "";

    public void SetBoolean(bool val)
    {
    }

    public void SetDouble(double val)
    {
    }

    public void SetInt(int val)
    {
    }

    public void SetJsonType(JsonType type)
    {
    }

    public void SetLong(long val)
    {
    }

    public void SetString(string val)
    {
    }

    public string ToJson() => "";

    public void ToJson(JsonWriter writer)
    {
    }

    bool IList.IsFixedSize => true;

    bool IList.IsReadOnly => true;

    object IList.this[int index]
    {
      get => (object) null;
      set
      {
      }
    }

    int IList.Add(object value) => 0;

    void IList.Clear()
    {
    }

    bool IList.Contains(object value) => false;

    int IList.IndexOf(object value) => -1;

    void IList.Insert(int i, object v)
    {
    }

    void IList.Remove(object value)
    {
    }

    void IList.RemoveAt(int index)
    {
    }

    int ICollection.Count => 0;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => (object) null;

    void ICollection.CopyTo(Array array, int index)
    {
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) null;

    bool IDictionary.IsFixedSize => true;

    bool IDictionary.IsReadOnly => true;

    ICollection IDictionary.Keys => (ICollection) null;

    ICollection IDictionary.Values => (ICollection) null;

    object IDictionary.this[object key]
    {
      get => (object) null;
      set
      {
      }
    }

    void IDictionary.Add(object k, object v)
    {
    }

    void IDictionary.Clear()
    {
    }

    bool IDictionary.Contains(object key) => false;

    void IDictionary.Remove(object key)
    {
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() => (IDictionaryEnumerator) null;

    object IOrderedDictionary.this[int idx]
    {
      get => (object) null;
      set
      {
      }
    }

    IDictionaryEnumerator IOrderedDictionary.GetEnumerator() => (IDictionaryEnumerator) null;

    void IOrderedDictionary.Insert(int i, object k, object v)
    {
    }

    void IOrderedDictionary.RemoveAt(int i)
    {
    }
  }
}
