// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerDictionary`2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerDictionary<TKey, TValue> : MySerializer<Dictionary<TKey, TValue>>
  {
    private MySerializer<TKey> m_keySerializer = MyFactory.GetSerializer<TKey>();
    private MySerializer<TValue> m_valueSerializer = MyFactory.GetSerializer<TValue>();

    public override void Clone(ref Dictionary<TKey, TValue> obj)
    {
      Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(obj.Count);
      foreach (KeyValuePair<TKey, TValue> keyValuePair in obj)
      {
        TKey key = keyValuePair.Key;
        TValue obj1 = keyValuePair.Value;
        this.m_keySerializer.Clone(ref key);
        this.m_valueSerializer.Clone(ref obj1);
        dictionary.Add(key, obj1);
      }
      obj = dictionary;
    }

    public override bool Equals(ref Dictionary<TKey, TValue> a, ref Dictionary<TKey, TValue> b)
    {
      if (a == b)
        return true;
      if (MySerializer.AnyNull((object) a, (object) b) || a.Count != b.Count)
        return false;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in a)
      {
        TValue a1 = keyValuePair.Value;
        TValue b1;
        if (!b.TryGetValue(keyValuePair.Key, out b1) || !this.m_valueSerializer.Equals(ref a1, ref b1))
          return false;
      }
      return true;
    }

    public override void Read(
      BitStream stream,
      out Dictionary<TKey, TValue> obj,
      MySerializeInfo info)
    {
      int capacity = (int) stream.ReadUInt32Variant();
      obj = new Dictionary<TKey, TValue>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        TKey result1;
        MySerializationHelpers.CreateAndRead<TKey>(stream, out result1, this.m_keySerializer, info.KeyInfo ?? MySerializeInfo.Default);
        TValue result2;
        MySerializationHelpers.CreateAndRead<TValue>(stream, out result2, this.m_valueSerializer, info.ItemInfo ?? MySerializeInfo.Default);
        obj.Add(result1, result2);
      }
    }

    public override void Write(
      BitStream stream,
      ref Dictionary<TKey, TValue> obj,
      MySerializeInfo info)
    {
      int count = obj.Count;
      stream.WriteVariant((uint) count);
      foreach (KeyValuePair<TKey, TValue> keyValuePair in obj)
      {
        TKey key = keyValuePair.Key;
        TValue obj1 = keyValuePair.Value;
        MySerializationHelpers.Write<TKey>(stream, ref key, this.m_keySerializer, info.KeyInfo ?? MySerializeInfo.Default);
        MySerializationHelpers.Write<TValue>(stream, ref obj1, this.m_valueSerializer, info.ItemInfo ?? MySerializeInfo.Default);
      }
    }
  }
}
