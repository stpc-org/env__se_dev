// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerHashSet`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerHashSet<TItem> : MySerializer<HashSet<TItem>>
  {
    private MySerializer<TItem> m_itemSerializer = MyFactory.GetSerializer<TItem>();

    public override void Clone(ref HashSet<TItem> value)
    {
      HashSet<TItem> objSet = new HashSet<TItem>();
      foreach (TItem obj in value)
      {
        this.m_itemSerializer.Clone(ref obj);
        objSet.Add(obj);
      }
      value = objSet;
    }

    public override bool Equals(ref HashSet<TItem> a, ref HashSet<TItem> b)
    {
      if (a == b)
        return true;
      if (MySerializer.AnyNull((object) a, (object) b) || a.Count != b.Count)
        return false;
      foreach (TItem obj in a)
      {
        if (!b.Contains(obj))
          return false;
      }
      return true;
    }

    public override void Read(BitStream stream, out HashSet<TItem> value, MySerializeInfo info)
    {
      int num = (int) stream.ReadUInt32Variant();
      value = new HashSet<TItem>();
      for (int index = 0; index < num; ++index)
      {
        TItem result;
        MySerializationHelpers.CreateAndRead<TItem>(stream, out result, this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
        value.Add(result);
      }
    }

    public override void Write(BitStream stream, ref HashSet<TItem> value, MySerializeInfo info)
    {
      int count = value.Count;
      stream.WriteVariant((uint) count);
      foreach (TItem obj in value)
        MySerializationHelpers.Write<TItem>(stream, ref obj, this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
    }
  }
}
