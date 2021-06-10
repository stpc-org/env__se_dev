// Decompiled with JetBrains decompiler
// Type: BulletXNA.LinearMath.ObjectArray`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace BulletXNA.LinearMath
{
  public class ObjectArray<T> where T : new()
  {
    private const int _defaultCapacity = 4;
    private static T[] _emptyArray = new T[0];
    private T[] _items;
    private int _size;
    private int _version;

    public ObjectArray() => this._items = ObjectArray<T>._emptyArray;

    public T[] GetRawArray() => this._items;

    public ObjectArray(int capacity) => this._items = capacity >= 0 ? new T[capacity] : throw new Exception("ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_SmallCapacity");

    public void Add(T item)
    {
      if (this._size == this._items.Length)
        this.EnsureCapacity(this._size + 1);
      this._items[this._size++] = item;
      ++this._version;
    }

    public void Swap(int index0, int index1)
    {
      T obj = this._items[index0];
      this._items[index0] = this._items[index1];
      this._items[index1] = obj;
    }

    public void Resize(int newsize) => this.Resize(newsize, true);

    public void Resize(int newsize, bool allocate)
    {
      int count = this.Count;
      if (newsize < count)
      {
        if (allocate)
        {
          for (int index = newsize; index < count; ++index)
            this._items[index] = new T();
        }
        else
        {
          for (int index = newsize; index < count; ++index)
            this._items[index] = default (T);
        }
      }
      else
      {
        if (newsize > this.Count)
          this.Capacity = newsize;
        if (allocate)
        {
          for (int index = count; index < newsize; ++index)
            this._items[index] = new T();
        }
      }
      this._size = newsize;
    }

    public void Clear()
    {
      if (this._size > 0)
      {
        Array.Clear((Array) this._items, 0, this._size);
        this._size = 0;
      }
      ++this._version;
    }

    private void EnsureCapacity(int min)
    {
      if (this._items.Length >= min)
        return;
      int num = this._items.Length == 0 ? 4 : this._items.Length * 2;
      if (num < min)
        num = min;
      this.Capacity = num;
    }

    public int Capacity
    {
      get => this._items.Length;
      set
      {
        if (value == this._items.Length)
          return;
        if (value < this._size)
          throw new Exception("ExceptionResource ArgumentOutOfRange_SmallCapacity");
        if (value > 0)
        {
          T[] objArray = new T[value];
          if (this._size > 0)
            Array.Copy((Array) this._items, 0, (Array) objArray, 0, this._size);
          this._items = objArray;
        }
        else
          this._items = ObjectArray<T>._emptyArray;
      }
    }

    public int Count => this._size;

    public T this[int index]
    {
      get
      {
        int num = index + 1 - this._size;
        for (int index1 = 0; index1 < num; ++index1)
          this.Add(new T());
        return index < this._size ? this._items[index] : throw new Exception("ThrowHelper.ThrowArgumentOutOfRangeException()");
      }
      set
      {
        int num = index + 1 - this._size;
        for (int index1 = 0; index1 < num; ++index1)
          this.Add(new T());
        if (index >= this._size)
          throw new Exception("ThrowHelper.ThrowArgumentOutOfRangeException()");
        this._items[index] = value;
        ++this._version;
      }
    }
  }
}
