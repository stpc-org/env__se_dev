// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Network;

namespace VRage.Library.Collections
{
  [DebuggerDisplay("Count = {Count}")]
  [Serializable]
  public class MyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
  {
    private const int DefaultCapacity = 4;
    private const int MaxLength = 2147483647;
    private T[] m_items;
    private int m_size;
    private int m_version;
    [NonSerialized]
    private object m_syncRoot;
    private static readonly T[] EmptyArray = new T[0];

    public void InsertFrom(IList<T> list, int sourceIndex, int destIndex, int count)
    {
      if (list.Count < sourceIndex + count)
        throw new ArgumentOutOfRangeException("", "Some element indices are out of the bounds of the source array.");
      if (destIndex < 0 || sourceIndex > this.m_size)
        throw new ArgumentOutOfRangeException(nameof (destIndex), "The destination index is out of the bounds of this list.");
      this.EnsureCapacity(this.m_size + count);
      this.MoveForward(destIndex, count);
      if (list is T[] objArray)
      {
        Array.Copy((Array) objArray, sourceIndex, (Array) this.m_items, destIndex, count);
      }
      else
      {
        for (int index = 0; index < count; ++index)
          this.m_items[destIndex + index] = list[sourceIndex + index];
      }
      ++this.m_version;
      this.m_size += count;
    }

    public void RemoveAtFast(int index)
    {
      this.m_items[index] = index >= 0 && index < this.m_size ? this.m_items[this.m_size - 1] : throw new ArgumentOutOfRangeException(nameof (index), "Index cannot be negative or greater than the size of the list.");
      this.m_items[this.m_size - 1] = default (T);
      --this.m_size;
      ++this.m_version;
    }

    public int RemoveAllFast(Predicate<T> match)
    {
      int num = 0;
      for (int index = this.m_size - 1; index >= 0; --index)
      {
        if (match(this.m_items[index]))
        {
          ++num;
          this.RemoveAtFast(index);
        }
      }
      return num;
    }

    public void ClearFast()
    {
      this.m_size = 0;
      ++this.m_version;
    }

    public void ClearForced()
    {
      if (this.m_items != null)
      {
        for (int index = 0; index < this.m_items.Length; ++index)
          this.m_items[index] = default (T);
      }
      this.m_size = 0;
      ++this.m_version;
    }

    public void Touch() => ++this.m_version;

    public void SetSize(int size)
    {
      this.m_size = size >= 0 && size <= this.Capacity ? size : throw new ArgumentOutOfRangeException(nameof (size), "Size must not be smaller than zero or larger than the current capacity.");
      ++this.m_version;
    }

    private void MoveForward(int index, int count)
    {
      this.EnsureCapacity(this.m_size + count);
      Array.Copy((Array) this.m_items, index, (Array) this.m_items, index + count, this.m_size - index);
    }

    private void MoveBackward(int index, int count)
    {
      this.EnsureCapacity(this.m_size + count);
      Array.Copy((Array) this.m_items, index + count, (Array) this.m_items, index, this.m_size - index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetInternalArray() => this.m_items;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan() => new Span<T>(this.m_items, 0, this.m_size);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start, int length = -1) => length != -1 ? this.AsSpan().Slice(start, length) : this.AsSpan().Slice(start);

    public void Insert(Span<T> data, int index = -1)
    {
      if (index < 0)
        index = this.Count;
      int length = data.Length;
      this.EnsureCapacity(index + length);
      this.m_size += length;
      Span<T> destination = this.Slice(index, length);
      data.CopyTo(destination);
    }

    public MyList() => this.m_items = MyList<T>.EmptyArray;

    public MyList(int capacity)
    {
      if (capacity < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (capacity == 0)
        this.m_items = MyList<T>.EmptyArray;
      else
        this.m_items = new T[capacity];
    }

    public MyList(IEnumerable<T> collection)
    {
      if (collection == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
      if (collection is ICollection<T> objs)
      {
        int count = objs.Count;
        if (count == 0)
        {
          this.m_items = MyList<T>.EmptyArray;
        }
        else
        {
          this.m_items = new T[count];
          objs.CopyTo(this.m_items, 0);
          this.m_size = count;
        }
      }
      else
      {
        this.m_size = 0;
        this.m_items = MyList<T>.EmptyArray;
        foreach (T obj in collection)
          this.Add(obj);
      }
    }

    public int Capacity
    {
      get => this.m_items.Length;
      set
      {
        if (value < this.m_size)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
        if (value == this.m_items.Length)
          return;
        if (value > 0)
        {
          T[] objArray = new T[value];
          if (this.m_size > 0)
            Array.Copy((Array) this.m_items, 0, (Array) objArray, 0, this.m_size);
          this.m_items = objArray;
        }
        else
          this.m_items = MyList<T>.EmptyArray;
      }
    }

    public int Count => this.m_size;

    bool IList.IsFixedSize => false;

    bool ICollection<T>.IsReadOnly => false;

    bool IList.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
      get
      {
        if (this.m_syncRoot == null)
          Interlocked.CompareExchange<object>(ref this.m_syncRoot, new object(), (object) null);
        return this.m_syncRoot;
      }
    }

    public T this[int index]
    {
      get
      {
        if ((uint) index >= (uint) this.m_size)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        return this.m_items[index];
      }
      set
      {
        if ((uint) index >= (uint) this.m_size)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        this.m_items[index] = value;
        ++this.m_version;
      }
    }

    private static bool IsCompatibleObject(object value)
    {
      if (value is T)
        return true;
      return value == null && (object) default (T) == null;
    }

    object IList.this[int index]
    {
      get => (object) this[index];
      set
      {
        ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
        try
        {
          this[index] = (T) value;
        }
        catch (InvalidCastException ex)
        {
          ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof (T));
        }
      }
    }

    public void Add(T item)
    {
      if (this.m_size == this.m_items.Length)
        this.EnsureCapacity(this.m_size + 1);
      this.m_items[this.m_size++] = item;
      ++this.m_version;
    }

    int IList.Add(object item)
    {
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
      try
      {
        this.Add((T) item);
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof (T));
      }
      return this.Count - 1;
    }

    public void AddRange(IEnumerable<T> collection) => this.InsertRange(this.m_size, collection);

    public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>((IList<T>) this);

    public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
    {
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.m_size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      return Array.BinarySearch<T>(this.m_items, index, count, item, comparer);
    }

    public int BinarySearch(T item) => this.BinarySearch(0, this.Count, item, (IComparer<T>) null);

    public int BinarySearch(T item, IComparer<T> comparer) => this.BinarySearch(0, this.Count, item, comparer);

    public void Clear()
    {
      if (this.m_size > 0)
      {
        Array.Clear((Array) this.m_items, 0, this.m_size);
        this.m_size = 0;
      }
      ++this.m_version;
    }

    public bool Contains(T item)
    {
      if ((object) item == null)
      {
        for (int index = 0; index < this.m_size; ++index)
        {
          if ((object) this.m_items[index] == null)
            return true;
        }
        return false;
      }
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index = 0; index < this.m_size; ++index)
      {
        if (equalityComparer.Equals(this.m_items[index], item))
          return true;
      }
      return false;
    }

    bool IList.Contains(object item) => MyList<T>.IsCompatibleObject(item) && this.Contains((T) item);

    public MyList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
    {
      if (converter == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
      MyList<TOutput> myList = new MyList<TOutput>(this.m_size);
      for (int index = 0; index < this.m_size; ++index)
        myList.m_items[index] = converter(this.m_items[index]);
      myList.m_size = this.m_size;
      return myList;
    }

    public void CopyTo(T[] array) => this.CopyTo(array, 0);

    void ICollection.CopyTo(Array array, int arrayIndex)
    {
      if (array != null && array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      try
      {
        Array.Copy((Array) this.m_items, 0, array, arrayIndex, this.m_size);
      }
      catch (ArrayTypeMismatchException ex)
      {
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
      }
    }

    public void CopyTo(int index, T[] array, int arrayIndex, int count)
    {
      if (this.m_size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      Array.Copy((Array) this.m_items, index, (Array) array, arrayIndex, count);
    }

    public void CopyTo(T[] array, int arrayIndex) => Array.Copy((Array) this.m_items, 0, (Array) array, arrayIndex, this.m_size);

    public void EnsureCapacity(int min)
    {
      if (this.m_items.Length >= min)
        return;
      int num = this.m_items.Length == 0 ? 4 : this.m_items.Length * 2;
      if ((uint) num > (uint) int.MaxValue)
        num = int.MaxValue;
      if (num < min)
        num = min;
      this.Capacity = num;
    }

    public bool Exists(Predicate<T> match) => this.FindIndex(match) != -1;

    public T Find(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index]))
          return this.m_items[index];
      }
      return default (T);
    }

    public List<T> FindAll(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      List<T> objList = new List<T>();
      for (int index = 0; index < this.m_size; ++index)
      {
        if (match(this.m_items[index]))
          objList.Add(this.m_items[index]);
      }
      return objList;
    }

    public int FindIndex(Predicate<T> match) => this.FindIndex(0, this.m_size, match);

    public int FindIndex(int startIndex, Predicate<T> match) => this.FindIndex(startIndex, this.m_size - startIndex, match);

    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
      if ((uint) startIndex > (uint) this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
      if (count < 0 || startIndex > this.m_size - count)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      int num = startIndex + count;
      for (int index = startIndex; index < num; ++index)
      {
        if (match(this.m_items[index]))
          return index;
      }
      return -1;
    }

    public T FindLast(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = this.m_size - 1; index >= 0; --index)
      {
        if (match(this.m_items[index]))
          return this.m_items[index];
      }
      return default (T);
    }

    public int FindLastIndex(Predicate<T> match) => this.FindLastIndex(this.m_size - 1, this.m_size, match);

    public int FindLastIndex(int startIndex, Predicate<T> match) => this.FindLastIndex(startIndex, startIndex + 1, match);

    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      if (this.m_size == 0)
      {
        if (startIndex != -1)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
      }
      else if ((uint) startIndex >= (uint) this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
      if (count < 0 || startIndex - count + 1 < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
      int num = startIndex - count;
      for (int index = startIndex; index > num; --index)
      {
        if (match(this.m_items[index]))
          return index;
      }
      return -1;
    }

    public void ForEach(Action<T> action)
    {
      if (action == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      int version = this.m_version;
      for (int index = 0; index < this.m_size && version == this.m_version; ++index)
        action(this.m_items[index]);
      if (version == this.m_version)
        return;
      ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
    }

    public MyList<T>.Enumerator GetEnumerator() => new MyList<T>.Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) new MyList<T>.Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new MyList<T>.Enumerator(this);

    public MyList<T> GetRange(int index, int count)
    {
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.m_size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      MyList<T> myList = new MyList<T>(count);
      Array.Copy((Array) this.m_items, index, (Array) myList.m_items, 0, count);
      myList.m_size = count;
      return myList;
    }

    public int IndexOf(T item) => Array.IndexOf<T>(this.m_items, item, 0, this.m_size);

    int IList.IndexOf(object item) => MyList<T>.IsCompatibleObject(item) ? this.IndexOf((T) item) : -1;

    public int IndexOf(T item, int index)
    {
      if (index > this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
      return Array.IndexOf<T>(this.m_items, item, index, this.m_size - index);
    }

    public int IndexOf(T item, int index, int count)
    {
      if (index > this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
      if (count < 0 || index > this.m_size - count)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
      return Array.IndexOf<T>(this.m_items, item, index, count);
    }

    public void Insert(int index, T item)
    {
      if ((uint) index > (uint) this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
      if (this.m_size == this.m_items.Length)
        this.EnsureCapacity(this.m_size + 1);
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index, (Array) this.m_items, index + 1, this.m_size - index);
      this.m_items[index] = item;
      ++this.m_size;
      ++this.m_version;
    }

    void IList.Insert(int index, object item)
    {
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
      try
      {
        this.Insert(index, (T) item);
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof (T));
      }
    }

    public void InsertRange(int index, IEnumerable<T> collection)
    {
      if (collection == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
      if ((uint) index > (uint) this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
      if (collection is ICollection<T> objs)
      {
        int count = objs.Count;
        if (count > 0)
        {
          this.EnsureCapacity(this.m_size + count);
          if (index < this.m_size)
            Array.Copy((Array) this.m_items, index, (Array) this.m_items, index + count, this.m_size - index);
          if (this == objs)
          {
            Array.Copy((Array) this.m_items, 0, (Array) this.m_items, index, index);
            Array.Copy((Array) this.m_items, index + count, (Array) this.m_items, index * 2, this.m_size - index);
          }
          else
            objs.CopyTo(this.m_items, index);
          this.m_size += count;
        }
      }
      else
      {
        foreach (T obj in collection)
          this.Insert(index++, obj);
      }
      ++this.m_version;
    }

    public int LastIndexOf(T item) => this.m_size == 0 ? -1 : this.LastIndexOf(item, this.m_size - 1, this.m_size);

    public int LastIndexOf(T item, int index)
    {
      if (index >= this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
      return this.LastIndexOf(item, index, index + 1);
    }

    public int LastIndexOf(T item, int index, int count)
    {
      if (this.Count != 0 && index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.Count != 0 && count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.m_size == 0)
        return -1;
      if (index >= this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
      if (count > index + 1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
      return Array.LastIndexOf<T>(this.m_items, item, index, count);
    }

    public bool Remove(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAt(index);
      return true;
    }

    void IList.Remove(object item)
    {
      if (!MyList<T>.IsCompatibleObject(item))
        return;
      this.Remove((T) item);
    }

    public int RemoveAll(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      int index1 = 0;
      while (index1 < this.m_size && !match(this.m_items[index1]))
        ++index1;
      if (index1 >= this.m_size)
        return 0;
      int index2 = index1 + 1;
      while (index2 < this.m_size)
      {
        while (index2 < this.m_size && match(this.m_items[index2]))
          ++index2;
        if (index2 < this.m_size)
          this.m_items[index1++] = this.m_items[index2++];
      }
      Array.Clear((Array) this.m_items, index1, this.m_size - index1);
      int num = this.m_size - index1;
      this.m_size = index1;
      ++this.m_version;
      return num;
    }

    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this.m_size)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      --this.m_size;
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index + 1, (Array) this.m_items, index, this.m_size - index);
      this.m_items[this.m_size] = default (T);
      ++this.m_version;
    }

    public void RemoveRange(int index, int count)
    {
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.m_size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (count <= 0)
        return;
      int size = this.m_size;
      this.m_size -= count;
      if (index < this.m_size)
        Array.Copy((Array) this.m_items, index + count, (Array) this.m_items, index, this.m_size - index);
      Array.Clear((Array) this.m_items, this.m_size, count);
      ++this.m_version;
    }

    public void Reverse() => this.Reverse(0, this.Count);

    public void Reverse(int index, int count)
    {
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.m_size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      Array.Reverse((Array) this.m_items, index, count);
      ++this.m_version;
    }

    public void Sort() => this.Sort(0, this.Count, (IComparer<T>) null);

    public void Sort(IComparer<T> comparer) => this.Sort(0, this.Count, comparer);

    public void Sort(int index, int count, IComparer<T> comparer)
    {
      if (index < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this.m_size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      Array.Sort<T>(this.m_items, index, count, comparer);
      ++this.m_version;
    }

    public void Sort(Comparison<T> comparison)
    {
      if (comparison == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      if (this.m_size <= 0)
        return;
      Array.Sort<T>(this.m_items, 0, this.m_size, (IComparer<T>) new MyList<T>.FunctorComparer<T>(comparison));
    }

    public T[] ToArray()
    {
      T[] objArray = new T[this.m_size];
      Array.Copy((Array) this.m_items, 0, (Array) objArray, 0, this.m_size);
      return objArray;
    }

    public void TrimExcess()
    {
      if (this.m_size >= (int) ((double) this.m_items.Length * 0.9))
        return;
      this.Capacity = this.m_size;
    }

    public bool TrueForAll(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = 0; index < this.m_size; ++index)
      {
        if (!match(this.m_items[index]))
          return false;
      }
      return true;
    }

    internal static IList<T> Synchronized(List<T> list) => (IList<T>) new MyList<T>.SynchronizedList(list);

    internal sealed class FunctorComparer<TItem> : IComparer<TItem>
    {
      private readonly Comparison<TItem> m_comparison;

      public FunctorComparer(Comparison<TItem> comparison) => this.m_comparison = comparison;

      public int Compare(TItem x, TItem y) => this.m_comparison(x, y);
    }

    [Serializable]
    internal class SynchronizedList : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
      private List<T> _list;
      private object _root;

      internal SynchronizedList(List<T> list)
      {
        this._list = list;
        this._root = ((ICollection) list).SyncRoot;
      }

      public int Count
      {
        get
        {
          lock (this._root)
            return this._list.Count;
        }
      }

      public bool IsReadOnly => ((ICollection<T>) this._list).IsReadOnly;

      public void Add(T item)
      {
        lock (this._root)
          this._list.Add(item);
      }

      public void Clear()
      {
        lock (this._root)
          this._list.Clear();
      }

      public bool Contains(T item)
      {
        lock (this._root)
          return this._list.Contains(item);
      }

      public void CopyTo(T[] array, int arrayIndex)
      {
        lock (this._root)
          this._list.CopyTo(array, arrayIndex);
      }

      public bool Remove(T item)
      {
        lock (this._root)
          return this._list.Remove(item);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        lock (this._root)
          return (IEnumerator) this._list.GetEnumerator();
      }

      IEnumerator<T> IEnumerable<T>.GetEnumerator()
      {
        lock (this._root)
          return ((IEnumerable<T>) this._list).GetEnumerator();
      }

      public T this[int index]
      {
        get
        {
          lock (this._root)
            return this._list[index];
        }
        set
        {
          lock (this._root)
            this._list[index] = value;
        }
      }

      public int IndexOf(T item)
      {
        lock (this._root)
          return this._list.IndexOf(item);
      }

      public void Insert(int index, T item)
      {
        lock (this._root)
          this._list.Insert(index, item);
      }

      public void RemoveAt(int index)
      {
        lock (this._root)
          this._list.RemoveAt(index);
      }

      protected class VRage_Library_Collections_MyList`1\u003C\u003ESynchronizedList\u003C\u003E_list\u003C\u003EAccessor : IMemberAccessor<MyList<T>.SynchronizedList, List<T>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyList<T>.SynchronizedList owner, in List<T> value) => owner._list = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyList<T>.SynchronizedList owner, out List<T> value) => value = owner._list;
      }

      protected class VRage_Library_Collections_MyList`1\u003C\u003ESynchronizedList\u003C\u003E_root\u003C\u003EAccessor : IMemberAccessor<MyList<T>.SynchronizedList, object>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyList<T>.SynchronizedList owner, in object value) => ((MyList<>.SynchronizedList) owner)._root = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyList<T>.SynchronizedList owner, out object value) => value = ((MyList<>.SynchronizedList) owner)._root;
      }
    }

    [Serializable]
    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
      private MyList<T> list;
      private int index;
      private int version;
      private T current;

      internal Enumerator(MyList<T> list)
      {
        this.list = list;
        this.index = 0;
        this.version = list.m_version;
        this.current = default (T);
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        MyList<T> list = this.list;
        if (this.version != list.m_version || (uint) this.index >= (uint) list.m_size)
          return this.MoveNextRare();
        this.current = list.m_items[this.index];
        ++this.index;
        return true;
      }

      private bool MoveNextRare()
      {
        if (this.version != this.list.m_version)
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
        this.index = this.list.m_size + 1;
        this.current = default (T);
        return false;
      }

      public T Current => this.current;

      object IEnumerator.Current
      {
        get
        {
          if (this.index == 0 || this.index == this.list.m_size + 1)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
          return (object) this.Current;
        }
      }

      void IEnumerator.Reset()
      {
        if (this.version != this.list.m_version)
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
        this.index = 0;
        this.current = default (T);
      }

      protected class VRage_Library_Collections_MyList`1\u003C\u003EEnumerator\u003C\u003Elist\u003C\u003EAccessor : IMemberAccessor<MyList<T>.Enumerator, MyList<T>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyList<T>.Enumerator owner, in MyList<T> value) => owner.list = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyList<T>.Enumerator owner, out MyList<T> value) => value = owner.list;
      }

      protected class VRage_Library_Collections_MyList`1\u003C\u003EEnumerator\u003C\u003Eindex\u003C\u003EAccessor : IMemberAccessor<MyList<T>.Enumerator, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyList<T>.Enumerator owner, in int value) => (^(MyList<>.Enumerator&) ref owner).index = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyList<T>.Enumerator owner, out int value) => value = (^(MyList<>.Enumerator&) ref owner).index;
      }

      protected class VRage_Library_Collections_MyList`1\u003C\u003EEnumerator\u003C\u003Eversion\u003C\u003EAccessor : IMemberAccessor<MyList<T>.Enumerator, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyList<T>.Enumerator owner, in int value) => (^(MyList<>.Enumerator&) ref owner).version = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyList<T>.Enumerator owner, out int value) => value = (^(MyList<>.Enumerator&) ref owner).version;
      }

      protected class VRage_Library_Collections_MyList`1\u003C\u003EEnumerator\u003C\u003Ecurrent\u003C\u003EAccessor : IMemberAccessor<MyList<T>.Enumerator, T>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyList<T>.Enumerator owner, in T value) => owner.current = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyList<T>.Enumerator owner, out T value) => value = owner.current;
      }
    }

    protected class VRage_Library_Collections_MyList`1\u003C\u003Em_items\u003C\u003EAccessor : IMemberAccessor<MyList<T>, T[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyList<T> owner, in T[] value) => owner.m_items = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyList<T> owner, out T[] value) => value = owner.m_items;
    }

    protected class VRage_Library_Collections_MyList`1\u003C\u003Em_size\u003C\u003EAccessor : IMemberAccessor<MyList<T>, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyList<T> owner, in int value) => ((MyList<>) owner).m_size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyList<T> owner, out int value) => value = ((MyList<>) owner).m_size;
    }

    protected class VRage_Library_Collections_MyList`1\u003C\u003Em_version\u003C\u003EAccessor : IMemberAccessor<MyList<T>, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyList<T> owner, in int value) => ((MyList<>) owner).m_version = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyList<T> owner, out int value) => value = ((MyList<>) owner).m_version;
    }

    protected class VRage_Library_Collections_MyList`1\u003C\u003Em_syncRoot\u003C\u003EAccessor : IMemberAccessor<MyList<T>, object>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyList<T> owner, in object value) => ((MyList<>) owner).m_syncRoot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyList<T> owner, out object value) => value = ((MyList<>) owner).m_syncRoot;
    }

    protected class VRage_Library_Collections_MyList`1\u003C\u003ECapacity\u003C\u003EAccessor : IMemberAccessor<MyList<T>, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyList<T> owner, in int value) => ((MyList<>) owner).Capacity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyList<T> owner, out int value) => value = ((MyList<>) owner).Capacity;
    }
  }
}
