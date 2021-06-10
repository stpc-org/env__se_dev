// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.NativeDictionary`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ParallelTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Library.Memory;
using VRage.Network;

namespace VRage.Library.Collections
{
  [DebuggerDisplay("Count = {Count}")]
  public class NativeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IDisposable
    where TKey : unmanaged
    where TValue : unmanaged
  {
    private static readonly NativeArrayAllocator m_nativeArrayAllocator = new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("NativeDictionaries"));
    private NativeDictionary<TKey, TValue>.UnmanagedMemory<int> buckets;
    private NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries;
    private int count;
    private int version;
    private int freeList;
    private int freeCount;
    private IEqualityComparer<TKey> comparer;
    private NativeDictionary<TKey, TValue>.KeyCollection keys;
    private NativeDictionary<TKey, TValue>.ValueCollection values;
    private object _syncRoot;
    private const string VersionName = "Version";
    private const string HashSizeName = "HashSize";
    private const string KeyValuePairsName = "KeyValuePairs";
    private const string ComparerName = "Comparer";

    public NativeDictionary()
      : this(0, (IEqualityComparer<TKey>) null)
    {
    }

    public NativeDictionary(int capacity)
      : this(capacity, (IEqualityComparer<TKey>) null)
    {
    }

    public NativeDictionary(IEqualityComparer<TKey> comparer)
      : this(0, comparer)
    {
    }

    public NativeDictionary(int capacity, IEqualityComparer<TKey> comparer)
    {
      if (capacity < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
      if (capacity > 0)
        this.Initialize(capacity);
      this.comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
    }

    public NativeDictionary(IDictionary<TKey, TValue> dictionary)
      : this(dictionary, (IEqualityComparer<TKey>) null)
    {
    }

    public NativeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
      : this(dictionary != null ? dictionary.Count : 0, comparer)
    {
      if (dictionary == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) dictionary)
        this.Add(keyValuePair.Key, keyValuePair.Value);
    }

    public IEqualityComparer<TKey> Comparer => this.comparer;

    public int Count => this.count - this.freeCount;

    public NativeDictionary<TKey, TValue>.KeyCollection Keys
    {
      get
      {
        if (this.keys == null)
          this.keys = new NativeDictionary<TKey, TValue>.KeyCollection(this);
        return this.keys;
      }
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
    {
      get
      {
        if (this.keys == null)
          this.keys = new NativeDictionary<TKey, TValue>.KeyCollection(this);
        return (ICollection<TKey>) this.keys;
      }
    }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
    {
      get
      {
        if (this.keys == null)
          this.keys = new NativeDictionary<TKey, TValue>.KeyCollection(this);
        return (IEnumerable<TKey>) this.keys;
      }
    }

    public NativeDictionary<TKey, TValue>.ValueCollection Values
    {
      get
      {
        if (this.values == null)
          this.values = new NativeDictionary<TKey, TValue>.ValueCollection(this);
        return this.values;
      }
    }

    ICollection<TValue> IDictionary<TKey, TValue>.Values
    {
      get
      {
        if (this.values == null)
          this.values = new NativeDictionary<TKey, TValue>.ValueCollection(this);
        return (ICollection<TValue>) this.values;
      }
    }

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
    {
      get
      {
        if (this.values == null)
          this.values = new NativeDictionary<TKey, TValue>.ValueCollection(this);
        return (IEnumerable<TValue>) this.values;
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        int entry = this.FindEntry(key);
        if (entry >= 0)
          return this.entries[entry].value;
        ThrowHelper.ThrowKeyNotFoundException();
        return default (TValue);
      }
      set => this.Insert(key, value, false);
    }

    public void Add(TKey key, TValue value) => this.Insert(key, value, true);

    void ICollection<KeyValuePair<TKey, TValue>>.Add(
      KeyValuePair<TKey, TValue> keyValuePair)
    {
      this.Add(keyValuePair.Key, keyValuePair.Value);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(
      KeyValuePair<TKey, TValue> keyValuePair)
    {
      int entry = this.FindEntry(keyValuePair.Key);
      return entry >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[entry].value, keyValuePair.Value);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
      KeyValuePair<TKey, TValue> keyValuePair)
    {
      int entry = this.FindEntry(keyValuePair.Key);
      if (entry < 0 || !EqualityComparer<TValue>.Default.Equals(this.entries[entry].value, keyValuePair.Value))
        return false;
      this.Remove(keyValuePair.Key);
      return true;
    }

    public void Clear()
    {
      if (this.count <= 0)
        return;
      for (int i = 0; i < this.buckets.Length; ++i)
        this.buckets[i] = -1;
      this.entries.Clear(this.count);
      this.freeList = -1;
      this.count = 0;
      this.freeCount = 0;
      ++this.version;
    }

    public bool ContainsKey(TKey key) => this.FindEntry(key) >= 0;

    public bool ContainsValue(TValue value)
    {
      EqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
      for (int i = 0; i < this.count; ++i)
      {
        if (this.entries[i].hashCode >= 0 && equalityComparer.Equals(this.entries[i].value, value))
          return true;
      }
      return false;
    }

    private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (index < 0 || index > array.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (array.Length - index < this.Count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
      int count = this.count;
      NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries = this.entries;
      for (int i = 0; i < count; ++i)
      {
        if (entries[i].hashCode >= 0)
          array[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
      }
    }

    public NativeDictionary<TKey, TValue>.Enumerator GetEnumerator() => new NativeDictionary<TKey, TValue>.Enumerator(this, 2);

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => (IEnumerator<KeyValuePair<TKey, TValue>>) new NativeDictionary<TKey, TValue>.Enumerator(this, 2);

    private int FindEntry(TKey key)
    {
      if (!this.buckets.IsNull)
      {
        int num = this.comparer.GetHashCode(key) & int.MaxValue;
        for (int i = this.buckets[num % this.buckets.Length]; i >= 0; i = this.entries[i].next)
        {
          if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
            return i;
        }
      }
      return -1;
    }

    private void Initialize(int capacity)
    {
      int prime = HashHelpers.GetPrime(capacity);
      this.buckets = NativeDictionary<TKey, TValue>.UnmanagedMemory<int>.Create(prime * 4);
      for (int i = 0; i < this.buckets.Length; ++i)
        this.buckets[i] = -1;
      this.entries = NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry>.Create(prime);
      this.entries.Clear();
      this.freeList = -1;
    }

    private void Insert(TKey key, TValue value, bool add)
    {
      if (this.buckets.IsNull)
        this.Initialize(0);
      int num = this.comparer.GetHashCode(key) & int.MaxValue;
      int i1 = num % this.buckets.Length;
      for (int i2 = this.buckets[i1]; i2 >= 0; i2 = this.entries[i2].next)
      {
        if (this.entries[i2].hashCode == num && this.comparer.Equals(this.entries[i2].key, key))
        {
          if (add)
            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
          this.entries[i2].value = value;
          ++this.version;
          return;
        }
      }
      int i3;
      if (this.freeCount > 0)
      {
        i3 = this.freeList;
        this.freeList = this.entries[i3].next;
        --this.freeCount;
      }
      else
      {
        if (this.count == this.entries.Length)
        {
          this.Resize();
          i1 = num % this.buckets.Length;
        }
        i3 = this.count;
        ++this.count;
      }
      this.entries[i3].hashCode = num;
      this.entries[i3].next = this.buckets[i1];
      this.entries[i3].key = key;
      this.entries[i3].value = value;
      this.buckets[i1] = i3;
      ++this.version;
    }

    private void Resize() => this.Resize(HashHelpers.ExpandPrime(this.count), false);

    private void Resize(int newSize, bool forceNewHashCodes)
    {
      NativeDictionary<TKey, TValue>.UnmanagedMemory<int> unmanagedMemory1 = NativeDictionary<TKey, TValue>.UnmanagedMemory<int>.Create(newSize);
      NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> unmanagedMemory2 = NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry>.Create(newSize);
      Span<int> span = unmanagedMemory1.Array.AsSpan<int>(newSize);
      for (int index = 0; index < span.Length; ++index)
        span[index] = -1;
      Span<NativeDictionary<TKey, TValue>.Entry> destination = unmanagedMemory2.Array.AsSpan<NativeDictionary<TKey, TValue>.Entry>(newSize);
      destination.Clear();
      this.entries.GetSpan().Slice(0, this.count).CopyTo(destination);
      if (forceNewHashCodes)
      {
        for (int index = 0; index < this.count; ++index)
        {
          if (destination[index].hashCode != -1)
            destination[index].hashCode = this.comparer.GetHashCode(destination[index].key) & int.MaxValue;
        }
      }
      for (int index1 = 0; index1 < this.count; ++index1)
      {
        if (destination[index1].hashCode >= 0)
        {
          int index2 = destination[index1].hashCode % newSize;
          destination[index1].next = span[index2];
          span[index2] = index1;
        }
      }
      this.buckets.Dispose();
      this.buckets = unmanagedMemory1;
      this.entries.Dispose();
      this.entries = unmanagedMemory2;
    }

    public bool Remove(TKey key)
    {
      if (!this.buckets.IsNull)
      {
        int num = this.comparer.GetHashCode(key) & int.MaxValue;
        int i1 = num % this.buckets.Length;
        int i2 = -1;
        for (int i3 = this.buckets[i1]; i3 >= 0; i3 = this.entries[i3].next)
        {
          if (this.entries[i3].hashCode == num && this.comparer.Equals(this.entries[i3].key, key))
          {
            if (i2 < 0)
              this.buckets[i1] = this.entries[i3].next;
            else
              this.entries[i2].next = this.entries[i3].next;
            this.entries[i3].hashCode = -1;
            this.entries[i3].next = this.freeList;
            this.entries[i3].key = default (TKey);
            this.entries[i3].value = default (TValue);
            this.freeList = i3;
            ++this.freeCount;
            ++this.version;
            return true;
          }
          i2 = i3;
        }
      }
      return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      int entry = this.FindEntry(key);
      if (entry >= 0)
      {
        value = this.entries[entry].value;
        return true;
      }
      value = default (TValue);
      return false;
    }

    internal TValue GetValueOrDefault(TKey key)
    {
      int entry = this.FindEntry(key);
      return entry >= 0 ? this.entries[entry].value : default (TValue);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
      KeyValuePair<TKey, TValue>[] array,
      int index)
    {
      this.CopyTo(array, index);
    }

    void ICollection.CopyTo(Array array, int index)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      if (array.GetLowerBound(0) != 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
      if (index < 0 || index > array.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (array.Length - index < this.Count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
      switch (array)
      {
        case KeyValuePair<TKey, TValue>[] array1:
          this.CopyTo(array1, index);
          break;
        case DictionaryEntry[] _:
          DictionaryEntry[] dictionaryEntryArray = array as DictionaryEntry[];
          NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries1 = this.entries;
          for (int i = 0; i < this.count; ++i)
          {
            if (entries1[i].hashCode >= 0)
              dictionaryEntryArray[index++] = new DictionaryEntry((object) entries1[i].key, (object) entries1[i].value);
          }
          break;
        case object[] objArray:
label_18:
          try
          {
            int count = this.count;
            NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries2 = this.entries;
            for (int i = 0; i < count; ++i)
            {
              if (entries2[i].hashCode >= 0)
              {
                int index1 = index++;
                // ISSUE: variable of a boxed type
                __Boxed<KeyValuePair<TKey, TValue>> local = (ValueType) new KeyValuePair<TKey, TValue>(entries2[i].key, entries2[i].value);
                objArray[index1] = (object) local;
              }
            }
            break;
          }
          catch (ArrayTypeMismatchException ex)
          {
            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            break;
          }
        default:
          ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
          goto label_18;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new NativeDictionary<TKey, TValue>.Enumerator(this, 2);

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    bool IDictionary.IsFixedSize => false;

    bool IDictionary.IsReadOnly => false;

    ICollection IDictionary.Keys => (ICollection) this.Keys;

    ICollection IDictionary.Values => (ICollection) this.Values;

    object IDictionary.this[object key]
    {
      get
      {
        if (NativeDictionary<TKey, TValue>.IsCompatibleKey(key))
        {
          int entry = this.FindEntry((TKey) key);
          if (entry >= 0)
            return (object) this.entries[entry].value;
        }
        return (object) null;
      }
      set
      {
        if (key == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
        ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
        try
        {
          TKey key1 = (TKey) key;
          try
          {
            this[key1] = (TValue) value;
          }
          catch (InvalidCastException ex)
          {
            ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof (TValue));
          }
        }
        catch (InvalidCastException ex)
        {
          ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof (TKey));
        }
      }
    }

    private static bool IsCompatibleKey(object key)
    {
      if (key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      return key is TKey;
    }

    void IDictionary.Add(object key, object value)
    {
      if (key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
      try
      {
        TKey key1 = (TKey) key;
        try
        {
          this.Add(key1, (TValue) value);
        }
        catch (InvalidCastException ex)
        {
          ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof (TValue));
        }
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof (TKey));
      }
    }

    bool IDictionary.Contains(object key) => NativeDictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey) key);

    IDictionaryEnumerator IDictionary.GetEnumerator() => (IDictionaryEnumerator) new NativeDictionary<TKey, TValue>.Enumerator(this, 1);

    void IDictionary.Remove(object key)
    {
      if (!NativeDictionary<TKey, TValue>.IsCompatibleKey(key))
        return;
      this.Remove((TKey) key);
    }

    public void Dispose()
    {
      this.Clear();
      this.buckets.Dispose();
      this.buckets = new NativeDictionary<TKey, TValue>.UnmanagedMemory<int>();
      this.entries.Dispose();
      this.entries = new NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry>();
    }

    private struct Entry
    {
      public int hashCode;
      public int next;
      public TKey key;
      public TValue value;
    }

    private struct UnmanagedMemory<T> : IDisposable
    {
      public readonly int Length;

      public NativeArray Array { get; private set; }

      public bool IsNull => this.Array == null;

      private UnmanagedMemory(NativeArray array, int length)
      {
        this.Array = array;
        this.Length = length;
      }

      public unsafe ref T this[int i] => ref Unsafe.Add<T>(ref Unsafe.AsRef<T>(this.Array.Ptr.ToPointer()), i);

      public static NativeDictionary<TKey, TValue>.UnmanagedMemory<T> Create(
        int size)
      {
        return new NativeDictionary<TKey, TValue>.UnmanagedMemory<T>(NativeDictionary<TKey, TValue>.m_nativeArrayAllocator.Allocate(size * Unsafe.SizeOf<T>()), size);
      }

      public void Dispose()
      {
        NativeDictionary<TKey, TValue>.m_nativeArrayAllocator.Dispose(this.Array);
        this.Array = (NativeArray) null;
      }

      public void Clear(int count)
      {
        for (int i = 0; i < count; ++i)
          this[i] = default (T);
      }

      public void Clear() => this.Clear(this.Length);

      public Span<T> GetSpan() => this.Array.AsSpan<T>(this.Length);
    }

    [Serializable]
    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable, IDictionaryEnumerator
    {
      private NativeDictionary<TKey, TValue> dictionary;
      private int version;
      private int index;
      private KeyValuePair<TKey, TValue> current;
      private int getEnumeratorRetType;
      internal const int DictEntry = 1;
      internal const int KeyValuePair = 2;

      internal Enumerator(NativeDictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
      {
        this.dictionary = dictionary;
        this.version = dictionary.version;
        this.index = 0;
        this.getEnumeratorRetType = getEnumeratorRetType;
        this.current = new KeyValuePair<TKey, TValue>();
      }

      public bool MoveNext()
      {
        if (this.version != this.dictionary.version)
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
        for (; (uint) this.index < (uint) this.dictionary.count; ++this.index)
        {
          if (this.dictionary.entries[this.index].hashCode >= 0)
          {
            this.current = new KeyValuePair<TKey, TValue>(this.dictionary.entries[this.index].key, this.dictionary.entries[this.index].value);
            ++this.index;
            return true;
          }
        }
        this.index = this.dictionary.count + 1;
        this.current = new KeyValuePair<TKey, TValue>();
        return false;
      }

      public KeyValuePair<TKey, TValue> Current => this.current;

      public void Dispose()
      {
      }

      object IEnumerator.Current
      {
        get
        {
          if (this.index == 0 || this.index == this.dictionary.count + 1)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
          return this.getEnumeratorRetType == 1 ? (object) new DictionaryEntry((object) this.current.Key, (object) this.current.Value) : (object) new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
        }
      }

      void IEnumerator.Reset()
      {
        if (this.version != this.dictionary.version)
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
        this.index = 0;
        this.current = new KeyValuePair<TKey, TValue>();
      }

      DictionaryEntry IDictionaryEnumerator.Entry
      {
        get
        {
          if (this.index == 0 || this.index == this.dictionary.count + 1)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
          return new DictionaryEntry((object) this.current.Key, (object) this.current.Value);
        }
      }

      object IDictionaryEnumerator.Key
      {
        get
        {
          if (this.index == 0 || this.index == this.dictionary.count + 1)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
          return (object) this.current.Key;
        }
      }

      object IDictionaryEnumerator.Value
      {
        get
        {
          if (this.index == 0 || this.index == this.dictionary.count + 1)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
          return (object) this.current.Value;
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EEnumerator\u003C\u003Edictionary\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.Enumerator, NativeDictionary<TKey, TValue>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          in NativeDictionary<TKey, TValue> value)
        {
          owner.dictionary = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          out NativeDictionary<TKey, TValue> value)
        {
          value = owner.dictionary;
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EEnumerator\u003C\u003Eversion\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.Enumerator, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          in int value)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(NativeDictionary<,>.Enumerator&) ref owner).version = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          out int value)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          value = (^(NativeDictionary<,>.Enumerator&) ref owner).version;
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EEnumerator\u003C\u003Eindex\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.Enumerator, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          in int value)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(NativeDictionary<,>.Enumerator&) ref owner).index = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          out int value)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          value = (^(NativeDictionary<,>.Enumerator&) ref owner).index;
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EEnumerator\u003C\u003Ecurrent\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.Enumerator, KeyValuePair<TKey, TValue>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          in KeyValuePair<TKey, TValue> value)
        {
          owner.current = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          out KeyValuePair<TKey, TValue> value)
        {
          value = owner.current;
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EEnumerator\u003C\u003EgetEnumeratorRetType\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.Enumerator, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          in int value)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(NativeDictionary<,>.Enumerator&) ref owner).getEnumeratorRetType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.Enumerator owner,
          out int value)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          value = (^(NativeDictionary<,>.Enumerator&) ref owner).getEnumeratorRetType;
        }
      }
    }

    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection, IReadOnlyCollection<TKey>
    {
      private NativeDictionary<TKey, TValue> dictionary;

      public KeyCollection(NativeDictionary<TKey, TValue> dictionary)
      {
        if (dictionary == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
        this.dictionary = dictionary;
      }

      public NativeDictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator() => new NativeDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);

      public void CopyTo(TKey[] array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (index < 0 || index > array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        if (array.Length - index < this.dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        int count = this.dictionary.count;
        NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries = this.dictionary.entries;
        for (int i = 0; i < count; ++i)
        {
          if (entries[i].hashCode >= 0)
            array[index++] = entries[i].key;
        }
      }

      public int Count => this.dictionary.Count;

      bool ICollection<TKey>.IsReadOnly => true;

      void ICollection<TKey>.Add(TKey item) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);

      void ICollection<TKey>.Clear() => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);

      bool ICollection<TKey>.Contains(TKey item) => this.dictionary.ContainsKey(item);

      bool ICollection<TKey>.Remove(TKey item)
      {
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
        return false;
      }

      IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => (IEnumerator<TKey>) new NativeDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new NativeDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);

      void ICollection.CopyTo(Array array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (array.Rank != 1)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
        if (array.GetLowerBound(0) != 0)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
        if (index < 0 || index > array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        if (array.Length - index < this.dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        switch (array)
        {
          case TKey[] array1:
            this.CopyTo(array1, index);
            break;
          case object[] objArray:
label_13:
            int count = this.dictionary.count;
            NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries = this.dictionary.entries;
            try
            {
              for (int i = 0; i < count; ++i)
              {
                if (entries[i].hashCode >= 0)
                {
                  int index1 = index++;
                  // ISSUE: variable of a boxed type
                  __Boxed<TKey> key = (ValueType) entries[i].key;
                  objArray[index1] = (object) key;
                }
              }
              break;
            }
            catch (ArrayTypeMismatchException ex)
            {
              ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
              break;
            }
          default:
            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            goto label_13;
        }
      }

      bool ICollection.IsSynchronized => false;

      object ICollection.SyncRoot => ((ICollection) this.dictionary).SyncRoot;

      [Serializable]
      public struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
      {
        private NativeDictionary<TKey, TValue> dictionary;
        private int index;
        private int version;
        private TKey currentKey;

        internal Enumerator(NativeDictionary<TKey, TValue> dictionary)
        {
          this.dictionary = dictionary;
          this.version = dictionary.version;
          this.index = 0;
          this.currentKey = default (TKey);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
          if (this.version != this.dictionary.version)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
          for (; (uint) this.index < (uint) this.dictionary.count; ++this.index)
          {
            if (this.dictionary.entries[this.index].hashCode >= 0)
            {
              this.currentKey = this.dictionary.entries[this.index].key;
              ++this.index;
              return true;
            }
          }
          this.index = this.dictionary.count + 1;
          this.currentKey = default (TKey);
          return false;
        }

        public TKey Current => this.currentKey;

        object IEnumerator.Current
        {
          get
          {
            if (this.index == 0 || this.index == this.dictionary.count + 1)
              ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
            return (object) this.currentKey;
          }
        }

        void IEnumerator.Reset()
        {
          if (this.version != this.dictionary.version)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
          this.index = 0;
          this.currentKey = default (TKey);
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EKeyCollection\u003C\u003EEnumerator\u003C\u003Edictionary\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.KeyCollection.Enumerator, NativeDictionary<TKey, TValue>>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            in NativeDictionary<TKey, TValue> value)
          {
            owner.dictionary = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            out NativeDictionary<TKey, TValue> value)
          {
            value = owner.dictionary;
          }
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EKeyCollection\u003C\u003EEnumerator\u003C\u003Eindex\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.KeyCollection.Enumerator, int>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            in int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(NativeDictionary<,>.KeyCollection.Enumerator&) ref owner).index = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            out int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            value = (^(NativeDictionary<,>.KeyCollection.Enumerator&) ref owner).index;
          }
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EKeyCollection\u003C\u003EEnumerator\u003C\u003Eversion\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.KeyCollection.Enumerator, int>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            in int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(NativeDictionary<,>.KeyCollection.Enumerator&) ref owner).version = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            out int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            value = (^(NativeDictionary<,>.KeyCollection.Enumerator&) ref owner).version;
          }
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EKeyCollection\u003C\u003EEnumerator\u003C\u003EcurrentKey\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.KeyCollection.Enumerator, TKey>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            in TKey value)
          {
            owner.currentKey = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.KeyCollection.Enumerator owner,
            out TKey value)
          {
            value = owner.currentKey;
          }
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EKeyCollection\u003C\u003Edictionary\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.KeyCollection, NativeDictionary<TKey, TValue>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.KeyCollection owner,
          in NativeDictionary<TKey, TValue> value)
        {
          owner.dictionary = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.KeyCollection owner,
          out NativeDictionary<TKey, TValue> value)
        {
          value = owner.dictionary;
        }
      }
    }

    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection, IReadOnlyCollection<TValue>
    {
      private NativeDictionary<TKey, TValue> dictionary;

      public ValueCollection(NativeDictionary<TKey, TValue> dictionary)
      {
        if (dictionary == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
        this.dictionary = dictionary;
      }

      public NativeDictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator() => new NativeDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);

      public void CopyTo(TValue[] array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (index < 0 || index > array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        if (array.Length - index < this.dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        int count = this.dictionary.count;
        NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries = this.dictionary.entries;
        for (int i = 0; i < count; ++i)
        {
          if (entries[i].hashCode >= 0)
            array[index++] = entries[i].value;
        }
      }

      public int Count => this.dictionary.Count;

      bool ICollection<TValue>.IsReadOnly => true;

      void ICollection<TValue>.Add(TValue item) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);

      bool ICollection<TValue>.Remove(TValue item)
      {
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
        return false;
      }

      void ICollection<TValue>.Clear() => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);

      bool ICollection<TValue>.Contains(TValue item) => this.dictionary.ContainsValue(item);

      IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => (IEnumerator<TValue>) new NativeDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new NativeDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);

      void ICollection.CopyTo(Array array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (array.Rank != 1)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
        if (array.GetLowerBound(0) != 0)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
        if (index < 0 || index > array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        if (array.Length - index < this.dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        switch (array)
        {
          case TValue[] array1:
            this.CopyTo(array1, index);
            break;
          case object[] objArray:
label_13:
            int count = this.dictionary.count;
            NativeDictionary<TKey, TValue>.UnmanagedMemory<NativeDictionary<TKey, TValue>.Entry> entries = this.dictionary.entries;
            try
            {
              for (int i = 0; i < count; ++i)
              {
                if (entries[i].hashCode >= 0)
                {
                  int index1 = index++;
                  // ISSUE: variable of a boxed type
                  __Boxed<TValue> local = (ValueType) entries[i].value;
                  objArray[index1] = (object) local;
                }
              }
              break;
            }
            catch (ArrayTypeMismatchException ex)
            {
              ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
              break;
            }
          default:
            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            goto label_13;
        }
      }

      bool ICollection.IsSynchronized => false;

      object ICollection.SyncRoot => ((ICollection) this.dictionary).SyncRoot;

      [Serializable]
      public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
      {
        private NativeDictionary<TKey, TValue> dictionary;
        private int index;
        private int version;
        private TValue currentValue;

        internal Enumerator(NativeDictionary<TKey, TValue> dictionary)
        {
          this.dictionary = dictionary;
          this.version = dictionary.version;
          this.index = 0;
          this.currentValue = default (TValue);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
          if (this.version != this.dictionary.version)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
          for (; (uint) this.index < (uint) this.dictionary.count; ++this.index)
          {
            if (this.dictionary.entries[this.index].hashCode >= 0)
            {
              this.currentValue = this.dictionary.entries[this.index].value;
              ++this.index;
              return true;
            }
          }
          this.index = this.dictionary.count + 1;
          this.currentValue = default (TValue);
          return false;
        }

        public TValue Current => this.currentValue;

        object IEnumerator.Current
        {
          get
          {
            if (this.index == 0 || this.index == this.dictionary.count + 1)
              ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
            return (object) this.currentValue;
          }
        }

        void IEnumerator.Reset()
        {
          if (this.version != this.dictionary.version)
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
          this.index = 0;
          this.currentValue = default (TValue);
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EValueCollection\u003C\u003EEnumerator\u003C\u003Edictionary\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.ValueCollection.Enumerator, NativeDictionary<TKey, TValue>>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            in NativeDictionary<TKey, TValue> value)
          {
            owner.dictionary = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            out NativeDictionary<TKey, TValue> value)
          {
            value = owner.dictionary;
          }
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EValueCollection\u003C\u003EEnumerator\u003C\u003Eindex\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.ValueCollection.Enumerator, int>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            in int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(NativeDictionary<,>.ValueCollection.Enumerator&) ref owner).index = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            out int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            value = (^(NativeDictionary<,>.ValueCollection.Enumerator&) ref owner).index;
          }
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EValueCollection\u003C\u003EEnumerator\u003C\u003Eversion\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.ValueCollection.Enumerator, int>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            in int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(NativeDictionary<,>.ValueCollection.Enumerator&) ref owner).version = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            out int value)
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            value = (^(NativeDictionary<,>.ValueCollection.Enumerator&) ref owner).version;
          }
        }

        protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EValueCollection\u003C\u003EEnumerator\u003C\u003EcurrentValue\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            in TValue value)
          {
            owner.currentValue = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref NativeDictionary<TKey, TValue>.ValueCollection.Enumerator owner,
            out TValue value)
          {
            value = owner.currentValue;
          }
        }
      }

      protected class VRage_Library_Collections_NativeDictionary`2\u003C\u003EValueCollection\u003C\u003Edictionary\u003C\u003EAccessor : IMemberAccessor<NativeDictionary<TKey, TValue>.ValueCollection, NativeDictionary<TKey, TValue>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref NativeDictionary<TKey, TValue>.ValueCollection owner,
          in NativeDictionary<TKey, TValue> value)
        {
          owner.dictionary = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref NativeDictionary<TKey, TValue>.ValueCollection owner,
          out NativeDictionary<TKey, TValue> value)
        {
          value = owner.dictionary;
        }
      }
    }
  }
}
