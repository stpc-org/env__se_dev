// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.BlitSerializer`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Serialization
{
  public class BlitSerializer<T> : ISerializer<T> where T : unmanaged
  {
    public static int StructSize = sizeof (T);
    public static readonly BlitSerializer<T> Default = new BlitSerializer<T>();

    public BlitSerializer() => MyLibraryUtils.ThrowNonBlittable<T>();

    public unsafe void Serialize(ByteStream destination, ref T data)
    {
      destination.EnsureCapacity(destination.Position + (long) BlitSerializer<T>.StructSize);
      fixed (byte* numPtr = &destination.Data[destination.Position])
        Unsafe.Copy<T>((void*) numPtr, ref data);
      destination.Position += (long) BlitSerializer<T>.StructSize;
    }

    public unsafe void Deserialize(ByteStream source, out T data)
    {
      source.CheckCapacity(source.Position + (long) BlitSerializer<T>.StructSize);
      fixed (byte* numPtr = &source.Data[source.Position])
      {
        data = default (T);
        Unsafe.Copy<T>(ref data, (void*) numPtr);
      }
      source.Position += (long) BlitSerializer<T>.StructSize;
    }

    public void SerializeList(ByteStream destination, MyList<T> data)
    {
      int count = data.Count;
      destination.Write7BitEncodedInt(count);
      for (int index = 0; index < count; ++index)
      {
        T[] internalArray = data.GetInternalArray();
        this.Serialize(destination, ref internalArray[index]);
      }
    }

    public void DeserializeList(ByteStream source, MyList<T> resultList)
    {
      int num = source.Read7BitEncodedInt();
      if (resultList.Capacity < num)
        resultList.Capacity = num;
      for (int index = 0; index < num; ++index)
      {
        T data;
        this.Deserialize(source, out data);
        resultList.Add(data);
      }
    }
  }
}
