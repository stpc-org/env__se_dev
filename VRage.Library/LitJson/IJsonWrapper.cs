// Decompiled with JetBrains decompiler
// Type: LitJson.IJsonWrapper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
  public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
  {
    bool IsArray { get; }

    bool IsBoolean { get; }

    bool IsDouble { get; }

    bool IsInt { get; }

    bool IsLong { get; }

    bool IsObject { get; }

    bool IsString { get; }

    bool GetBoolean();

    double GetDouble();

    int GetInt();

    JsonType GetJsonType();

    long GetLong();

    string GetString();

    void SetBoolean(bool val);

    void SetDouble(double val);

    void SetInt(int val);

    void SetJsonType(JsonType type);

    void SetLong(long val);

    void SetString(string val);

    string ToJson();

    void ToJson(JsonWriter writer);
  }
}
