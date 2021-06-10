// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerArray`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerArray<TItem> : MySerializer<TItem[]>
  {
    private MySerializer<TItem> m_itemSerializer = MyFactory.GetSerializer<TItem>();

    public override void Clone(ref TItem[] value)
    {
      value = (TItem[]) value.Clone();
      for (int index = 0; index < value.Length; ++index)
        this.m_itemSerializer.Clone(ref value[index]);
    }

    public override bool Equals(ref TItem[] a, ref TItem[] b)
    {
      if (a == b)
        return true;
      if (MySerializer.AnyNull((object) a, (object) b) || a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if (!this.m_itemSerializer.Equals(ref a[index], ref b[index]))
          return false;
      }
      return true;
    }

    public override void Read(BitStream stream, out TItem[] value, MySerializeInfo info)
    {
      int length = (int) stream.ReadUInt32Variant();
      value = new TItem[length];
      for (int index = 0; index < value.Length; ++index)
        MySerializationHelpers.CreateAndRead<TItem>(stream, out value[index], this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
    }

    public override void Write(BitStream stream, ref TItem[] value, MySerializeInfo info)
    {
      stream.WriteVariant((uint) value.Length);
      for (int index = 0; index < value.Length; ++index)
        MySerializationHelpers.Write<TItem>(stream, ref value[index], this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
    }
  }
}
