// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerList`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerList<TItem> : MySerializer<List<TItem>>
  {
    private MySerializer<TItem> m_itemSerializer = MyFactory.GetSerializer<TItem>();

    public override void Clone(ref List<TItem> value)
    {
      List<TItem> objList = new List<TItem>(value.Count);
      for (int index = 0; index < value.Count; ++index)
      {
        TItem obj = value[index];
        this.m_itemSerializer.Clone(ref obj);
        objList.Add(obj);
      }
      value = objList;
    }

    public override bool Equals(ref List<TItem> a, ref List<TItem> b)
    {
      if (a == b)
        return true;
      if (MySerializer.AnyNull((object) a, (object) b) || a.Count != b.Count)
        return false;
      for (int index = 0; index < a.Count; ++index)
      {
        TItem a1 = a[index];
        TItem b1 = b[index];
        if (!this.m_itemSerializer.Equals(ref a1, ref b1))
          return false;
      }
      return true;
    }

    public override void Read(BitStream stream, out List<TItem> value, MySerializeInfo info)
    {
      int capacity = (int) stream.ReadUInt32Variant();
      value = new List<TItem>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        TItem result;
        MySerializationHelpers.CreateAndRead<TItem>(stream, out result, this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
        value.Add(result);
      }
    }

    public override void Write(BitStream stream, ref List<TItem> value, MySerializeInfo info)
    {
      int count = value.Count;
      stream.WriteVariant((uint) count);
      for (int index = 0; index < count; ++index)
      {
        TItem obj = value[index];
        MySerializationHelpers.Write<TItem>(stream, ref obj, this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
      }
    }
  }
}
