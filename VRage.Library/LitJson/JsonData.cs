﻿// Decompiled with JetBrains decompiler
// Type: LitJson.JsonData
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
  public class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
  {
    private IList<JsonData> inst_array;
    private bool inst_boolean;
    private double inst_double;
    private int inst_int;
    private long inst_long;
    private IDictionary<string, JsonData> inst_object;
    private string inst_string;
    private string json;
    private JsonType type;
    private IList<KeyValuePair<string, JsonData>> object_list;

    public int Count => this.EnsureCollection().Count;

    public bool IsArray => this.type == JsonType.Array;

    public bool IsBoolean => this.type == JsonType.Boolean;

    public bool IsDouble => this.type == JsonType.Double;

    public bool IsInt => this.type == JsonType.Int;

    public bool IsLong => this.type == JsonType.Long;

    public bool IsObject => this.type == JsonType.Object;

    public bool IsString => this.type == JsonType.String;

    public ICollection<string> Keys
    {
      get
      {
        this.EnsureDictionary();
        return this.inst_object.Keys;
      }
    }

    public bool ContainsKey(string key)
    {
      this.EnsureDictionary();
      return this.inst_object.Keys.Contains(key);
    }

    int ICollection.Count => this.Count;

    bool ICollection.IsSynchronized => this.EnsureCollection().IsSynchronized;

    object ICollection.SyncRoot => this.EnsureCollection().SyncRoot;

    bool IDictionary.IsFixedSize => this.EnsureDictionary().IsFixedSize;

    bool IDictionary.IsReadOnly => this.EnsureDictionary().IsReadOnly;

    ICollection IDictionary.Keys
    {
      get
      {
        this.EnsureDictionary();
        IList<string> stringList = (IList<string>) new List<string>();
        foreach (KeyValuePair<string, JsonData> keyValuePair in (IEnumerable<KeyValuePair<string, JsonData>>) this.object_list)
          stringList.Add(keyValuePair.Key);
        return (ICollection) stringList;
      }
    }

    ICollection IDictionary.Values
    {
      get
      {
        this.EnsureDictionary();
        IList<JsonData> jsonDataList = (IList<JsonData>) new List<JsonData>();
        foreach (KeyValuePair<string, JsonData> keyValuePair in (IEnumerable<KeyValuePair<string, JsonData>>) this.object_list)
          jsonDataList.Add(keyValuePair.Value);
        return (ICollection) jsonDataList;
      }
    }

    bool IJsonWrapper.IsArray => this.IsArray;

    bool IJsonWrapper.IsBoolean => this.IsBoolean;

    bool IJsonWrapper.IsDouble => this.IsDouble;

    bool IJsonWrapper.IsInt => this.IsInt;

    bool IJsonWrapper.IsLong => this.IsLong;

    bool IJsonWrapper.IsObject => this.IsObject;

    bool IJsonWrapper.IsString => this.IsString;

    bool IList.IsFixedSize => this.EnsureList().IsFixedSize;

    bool IList.IsReadOnly => this.EnsureList().IsReadOnly;

    object IDictionary.this[object key]
    {
      get => this.EnsureDictionary()[key];
      set
      {
        if (!(key is string))
          throw new ArgumentException("The key has to be a string");
        JsonData jsonData = this.ToJsonData(value);
        this[(string) key] = jsonData;
      }
    }

    object IOrderedDictionary.this[int idx]
    {
      get
      {
        this.EnsureDictionary();
        return (object) this.object_list[idx].Value;
      }
      set
      {
        this.EnsureDictionary();
        JsonData jsonData = this.ToJsonData(value);
        KeyValuePair<string, JsonData> keyValuePair1 = this.object_list[idx];
        this.inst_object[keyValuePair1.Key] = jsonData;
        KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair1.Key, jsonData);
        this.object_list[idx] = keyValuePair2;
      }
    }

    object IList.this[int index]
    {
      get => this.EnsureList()[index];
      set
      {
        this.EnsureList();
        JsonData jsonData = this.ToJsonData(value);
        this[index] = jsonData;
      }
    }

    public JsonData this[string prop_name]
    {
      get
      {
        this.EnsureDictionary();
        return this.inst_object[prop_name];
      }
      set
      {
        this.EnsureDictionary();
        KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
        if (this.inst_object.ContainsKey(prop_name))
        {
          for (int index = 0; index < this.object_list.Count; ++index)
          {
            if (this.object_list[index].Key == prop_name)
            {
              this.object_list[index] = keyValuePair;
              break;
            }
          }
        }
        else
          this.object_list.Add(keyValuePair);
        this.inst_object[prop_name] = value;
        this.json = (string) null;
      }
    }

    public JsonData this[int index]
    {
      get
      {
        this.EnsureCollection();
        return this.type == JsonType.Array ? this.inst_array[index] : this.object_list[index].Value;
      }
      set
      {
        this.EnsureCollection();
        if (this.type == JsonType.Array)
        {
          this.inst_array[index] = value;
        }
        else
        {
          KeyValuePair<string, JsonData> keyValuePair1 = this.object_list[index];
          KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair1.Key, value);
          this.object_list[index] = keyValuePair2;
          this.inst_object[keyValuePair1.Key] = value;
        }
        this.json = (string) null;
      }
    }

    public JsonData()
    {
    }

    public JsonData(bool boolean)
    {
      this.type = JsonType.Boolean;
      this.inst_boolean = boolean;
    }

    public JsonData(double number)
    {
      this.type = JsonType.Double;
      this.inst_double = number;
    }

    public JsonData(int number)
    {
      this.type = JsonType.Int;
      this.inst_int = number;
    }

    public JsonData(long number)
    {
      this.type = JsonType.Long;
      this.inst_long = number;
    }

    public JsonData(object obj)
    {
      switch (obj)
      {
        case bool flag:
          this.type = JsonType.Boolean;
          this.inst_boolean = flag;
          break;
        case double num:
          this.type = JsonType.Double;
          this.inst_double = num;
          break;
        case int num:
          this.type = JsonType.Int;
          this.inst_int = num;
          break;
        case long num:
          this.type = JsonType.Long;
          this.inst_long = num;
          break;
        case string _:
          this.type = JsonType.String;
          this.inst_string = (string) obj;
          break;
        default:
          throw new ArgumentException("Unable to wrap the given object with JsonData");
      }
    }

    public JsonData(string str)
    {
      this.type = JsonType.String;
      this.inst_string = str;
    }

    public static implicit operator JsonData(bool data) => new JsonData(data);

    public static implicit operator JsonData(double data) => new JsonData(data);

    public static implicit operator JsonData(int data) => new JsonData(data);

    public static implicit operator JsonData(long data) => new JsonData(data);

    public static implicit operator JsonData(string data) => new JsonData(data);

    public static explicit operator bool(JsonData data) => data.type == JsonType.Boolean ? data.inst_boolean : throw new InvalidCastException("Instance of JsonData doesn't hold a double");

    public static explicit operator double(JsonData data) => data.type == JsonType.Double ? data.inst_double : throw new InvalidCastException("Instance of JsonData doesn't hold a double");

    public static explicit operator int(JsonData data)
    {
      if (data.type != JsonType.Int && data.type != JsonType.Long)
        throw new InvalidCastException("Instance of JsonData doesn't hold an int");
      return data.type != JsonType.Int ? (int) data.inst_long : data.inst_int;
    }

    public static explicit operator long(JsonData data)
    {
      if (data.type != JsonType.Long && data.type != JsonType.Int)
        throw new InvalidCastException("Instance of JsonData doesn't hold a long");
      return data.type != JsonType.Long ? (long) data.inst_int : data.inst_long;
    }

    public static explicit operator string(JsonData data) => data.type == JsonType.String ? data.inst_string : throw new InvalidCastException("Instance of JsonData doesn't hold a string");

    void ICollection.CopyTo(Array array, int index) => this.EnsureCollection().CopyTo(array, index);

    void IDictionary.Add(object key, object value)
    {
      JsonData jsonData = this.ToJsonData(value);
      this.EnsureDictionary().Add(key, (object) jsonData);
      this.object_list.Add(new KeyValuePair<string, JsonData>((string) key, jsonData));
      this.json = (string) null;
    }

    void IDictionary.Clear()
    {
      this.EnsureDictionary().Clear();
      this.object_list.Clear();
      this.json = (string) null;
    }

    bool IDictionary.Contains(object key) => this.EnsureDictionary().Contains(key);

    IDictionaryEnumerator IDictionary.GetEnumerator() => ((IOrderedDictionary) this).GetEnumerator();

    void IDictionary.Remove(object key)
    {
      this.EnsureDictionary().Remove(key);
      for (int index = 0; index < this.object_list.Count; ++index)
      {
        if (this.object_list[index].Key == (string) key)
        {
          this.object_list.RemoveAt(index);
          break;
        }
      }
      this.json = (string) null;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.EnsureCollection().GetEnumerator();

    bool IJsonWrapper.GetBoolean()
    {
      if (this.type != JsonType.Boolean)
        throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
      return this.inst_boolean;
    }

    double IJsonWrapper.GetDouble()
    {
      if (this.type != JsonType.Double)
        throw new InvalidOperationException("JsonData instance doesn't hold a double");
      return this.inst_double;
    }

    int IJsonWrapper.GetInt()
    {
      if (this.type != JsonType.Int)
        throw new InvalidOperationException("JsonData instance doesn't hold an int");
      return this.inst_int;
    }

    long IJsonWrapper.GetLong()
    {
      if (this.type != JsonType.Long)
        throw new InvalidOperationException("JsonData instance doesn't hold a long");
      return this.inst_long;
    }

    string IJsonWrapper.GetString()
    {
      if (this.type != JsonType.String)
        throw new InvalidOperationException("JsonData instance doesn't hold a string");
      return this.inst_string;
    }

    void IJsonWrapper.SetBoolean(bool val)
    {
      this.type = JsonType.Boolean;
      this.inst_boolean = val;
      this.json = (string) null;
    }

    void IJsonWrapper.SetDouble(double val)
    {
      this.type = JsonType.Double;
      this.inst_double = val;
      this.json = (string) null;
    }

    void IJsonWrapper.SetInt(int val)
    {
      this.type = JsonType.Int;
      this.inst_int = val;
      this.json = (string) null;
    }

    void IJsonWrapper.SetLong(long val)
    {
      this.type = JsonType.Long;
      this.inst_long = val;
      this.json = (string) null;
    }

    void IJsonWrapper.SetString(string val)
    {
      this.type = JsonType.String;
      this.inst_string = val;
      this.json = (string) null;
    }

    string IJsonWrapper.ToJson() => this.ToJson();

    void IJsonWrapper.ToJson(JsonWriter writer) => this.ToJson(writer);

    int IList.Add(object value) => this.Add(value);

    void IList.Clear()
    {
      this.EnsureList().Clear();
      this.json = (string) null;
    }

    bool IList.Contains(object value) => this.EnsureList().Contains(value);

    int IList.IndexOf(object value) => this.EnsureList().IndexOf(value);

    void IList.Insert(int index, object value)
    {
      this.EnsureList().Insert(index, value);
      this.json = (string) null;
    }

    void IList.Remove(object value)
    {
      this.EnsureList().Remove(value);
      this.json = (string) null;
    }

    void IList.RemoveAt(int index)
    {
      this.EnsureList().RemoveAt(index);
      this.json = (string) null;
    }

    IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
    {
      this.EnsureDictionary();
      return (IDictionaryEnumerator) new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
    }

    void IOrderedDictionary.Insert(int idx, object key, object value)
    {
      string str = (string) key;
      JsonData jsonData = this.ToJsonData(value);
      this[str] = jsonData;
      KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(str, jsonData);
      this.object_list.Insert(idx, keyValuePair);
    }

    void IOrderedDictionary.RemoveAt(int idx)
    {
      this.EnsureDictionary();
      this.inst_object.Remove(this.object_list[idx].Key);
      this.object_list.RemoveAt(idx);
    }

    private ICollection EnsureCollection()
    {
      if (this.type == JsonType.Array)
        return (ICollection) this.inst_array;
      if (this.type == JsonType.Object)
        return (ICollection) this.inst_object;
      throw new InvalidOperationException("The JsonData instance has to be initialized first");
    }

    private IDictionary EnsureDictionary()
    {
      if (this.type == JsonType.Object)
        return (IDictionary) this.inst_object;
      this.type = this.type == JsonType.None ? JsonType.Object : throw new InvalidOperationException("Instance of JsonData is not a dictionary");
      this.inst_object = (IDictionary<string, JsonData>) new Dictionary<string, JsonData>();
      this.object_list = (IList<KeyValuePair<string, JsonData>>) new List<KeyValuePair<string, JsonData>>();
      return (IDictionary) this.inst_object;
    }

    private IList EnsureList()
    {
      if (this.type == JsonType.Array)
        return (IList) this.inst_array;
      this.type = this.type == JsonType.None ? JsonType.Array : throw new InvalidOperationException("Instance of JsonData is not a list");
      this.inst_array = (IList<JsonData>) new List<JsonData>();
      return (IList) this.inst_array;
    }

    private JsonData ToJsonData(object obj)
    {
      if (obj == null)
        return (JsonData) null;
      return obj is JsonData ? (JsonData) obj : new JsonData(obj);
    }

    private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
    {
      if (obj == null)
        writer.Write((string) null);
      else if (obj.IsString)
        writer.Write(obj.GetString());
      else if (obj.IsBoolean)
        writer.Write(obj.GetBoolean());
      else if (obj.IsDouble)
        writer.Write(obj.GetDouble());
      else if (obj.IsInt)
        writer.Write(obj.GetInt());
      else if (obj.IsLong)
        writer.Write(obj.GetLong());
      else if (obj.IsArray)
      {
        writer.WriteArrayStart();
        foreach (IJsonWrapper jsonWrapper in (IEnumerable) obj)
          JsonData.WriteJson(jsonWrapper, writer);
        writer.WriteArrayEnd();
      }
      else
      {
        if (!obj.IsObject)
          return;
        writer.WriteObjectStart();
        foreach (DictionaryEntry dictionaryEntry in (IDictionary) obj)
        {
          writer.WritePropertyName((string) dictionaryEntry.Key);
          JsonData.WriteJson((IJsonWrapper) dictionaryEntry.Value, writer);
        }
        writer.WriteObjectEnd();
      }
    }

    public int Add(object value)
    {
      JsonData jsonData = this.ToJsonData(value);
      this.json = (string) null;
      return this.EnsureList().Add((object) jsonData);
    }

    public void Clear()
    {
      if (this.IsObject)
      {
        ((IDictionary) this).Clear();
      }
      else
      {
        if (!this.IsArray)
          return;
        ((IList) this).Clear();
      }
    }

    public bool Equals(JsonData x)
    {
      if (x == null || x.type != this.type && (x.type != JsonType.Int && x.type != JsonType.Long || this.type != JsonType.Int && this.type != JsonType.Long))
        return false;
      switch (this.type)
      {
        case JsonType.None:
          return true;
        case JsonType.Object:
          return this.inst_object.Equals((object) x.inst_object);
        case JsonType.Array:
          return this.inst_array.Equals((object) x.inst_array);
        case JsonType.String:
          return this.inst_string.Equals(x.inst_string);
        case JsonType.Int:
          if (!x.IsLong)
            return this.inst_int.Equals(x.inst_int);
          return x.inst_long >= (long) int.MinValue && x.inst_long <= (long) int.MaxValue && this.inst_int.Equals((int) x.inst_long);
        case JsonType.Long:
          if (!x.IsInt)
            return this.inst_long.Equals(x.inst_long);
          return this.inst_long >= (long) int.MinValue && this.inst_long <= (long) int.MaxValue && x.inst_int.Equals((int) this.inst_long);
        case JsonType.Double:
          return this.inst_double.Equals(x.inst_double);
        case JsonType.Boolean:
          return this.inst_boolean.Equals(x.inst_boolean);
        default:
          return false;
      }
    }

    public JsonType GetJsonType() => this.type;

    public void SetJsonType(JsonType type)
    {
      if (this.type == type)
        return;
      switch (type)
      {
        case JsonType.Object:
          this.inst_object = (IDictionary<string, JsonData>) new Dictionary<string, JsonData>();
          this.object_list = (IList<KeyValuePair<string, JsonData>>) new List<KeyValuePair<string, JsonData>>();
          break;
        case JsonType.Array:
          this.inst_array = (IList<JsonData>) new List<JsonData>();
          break;
        case JsonType.String:
          this.inst_string = (string) null;
          break;
        case JsonType.Int:
          this.inst_int = 0;
          break;
        case JsonType.Long:
          this.inst_long = 0L;
          break;
        case JsonType.Double:
          this.inst_double = 0.0;
          break;
        case JsonType.Boolean:
          this.inst_boolean = false;
          break;
      }
      this.type = type;
    }

    public string ToJson()
    {
      if (this.json != null)
        return this.json;
      StringWriter stringWriter = new StringWriter();
      JsonData.WriteJson((IJsonWrapper) this, new JsonWriter((TextWriter) stringWriter)
      {
        Validate = false
      });
      this.json = stringWriter.ToString();
      return this.json;
    }

    public void ToJson(JsonWriter writer)
    {
      bool validate = writer.Validate;
      writer.Validate = false;
      JsonData.WriteJson((IJsonWrapper) this, writer);
      writer.Validate = validate;
    }

    public override string ToString()
    {
      switch (this.type)
      {
        case JsonType.Object:
          return "JsonData object";
        case JsonType.Array:
          return "JsonData array";
        case JsonType.String:
          return this.inst_string;
        case JsonType.Int:
          return this.inst_int.ToString();
        case JsonType.Long:
          return this.inst_long.ToString();
        case JsonType.Double:
          return this.inst_double.ToString();
        case JsonType.Boolean:
          return this.inst_boolean.ToString();
        default:
          return "Uninitialized JsonData";
      }
    }
  }
}
