// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.BlitCollectionSerializer`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Serialization
{
  public class BlitCollectionSerializer<T, TData> : ISerializer<T>
    where T : ICollection<TData>, new()
    where TData : unmanaged
  {
    public static readonly BlitCollectionSerializer<T, TData> Default = new BlitCollectionSerializer<T, TData>();
    public static readonly BlitSerializer<TData> InnerSerializer = BlitSerializer<TData>.Default;

    public void Serialize(ByteStream destination, ref T data)
    {
      destination.Write7BitEncodedInt(data.Count);
      using (IEnumerator<TData> enumerator = data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TData current = enumerator.Current;
          BlitCollectionSerializer<T, TData>.InnerSerializer.Serialize(destination, ref current);
        }
      }
    }

    public void Deserialize(ByteStream source, out T data)
    {
      data = new T();
      int num = source.Read7BitEncodedInt();
      for (int index = 0; index < num; ++index)
      {
        TData data1;
        BlitCollectionSerializer<T, TData>.InnerSerializer.Deserialize(source, out data1);
        data.Add(data1);
      }
    }
  }
}
