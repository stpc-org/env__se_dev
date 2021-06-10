// Decompiled with JetBrains decompiler
// Type: VRageMath.CurveKeyCollection
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [TypeConverter(typeof (ExpandableObjectConverter))]
  [Serializable]
  public class CurveKeyCollection : ICollection<CurveKey>, IEnumerable<CurveKey>, IEnumerable
  {
    private List<CurveKey> Keys = new List<CurveKey>();
    internal bool IsCacheAvailable = true;
    internal float TimeRange;
    internal float InvTimeRange;

    public CurveKey this[int index]
    {
      get => this.Keys[index];
      set
      {
        if (value == (CurveKey) null)
          throw new ArgumentNullException();
        if ((double) this.Keys[index].Position == (double) value.Position)
        {
          this.Keys[index] = value;
        }
        else
        {
          this.Keys.RemoveAt(index);
          this.Add(value);
        }
      }
    }

    public void Add(object tmp)
    {
    }

    public int Count => this.Keys.Count;

    public bool IsReadOnly => false;

    public int IndexOf(CurveKey item) => this.Keys.IndexOf(item);

    public void RemoveAt(int index)
    {
      this.Keys.RemoveAt(index);
      this.IsCacheAvailable = false;
    }

    public void Add(CurveKey item)
    {
      int index = !(item == (CurveKey) null) ? this.Keys.BinarySearch(item) : throw new ArgumentNullException();
      if (index >= 0)
      {
        while (index < this.Keys.Count && (double) item.Position == (double) this.Keys[index].Position)
          ++index;
      }
      else
        index = ~index;
      this.Keys.Insert(index, item);
      this.IsCacheAvailable = false;
    }

    public void Clear()
    {
      this.Keys.Clear();
      this.TimeRange = this.InvTimeRange = 0.0f;
      this.IsCacheAvailable = false;
    }

    public bool Contains(CurveKey item) => this.Keys.Contains(item);

    public void CopyTo(CurveKey[] array, int arrayIndex)
    {
      this.Keys.CopyTo(array, arrayIndex);
      this.IsCacheAvailable = false;
    }

    public bool Remove(CurveKey item)
    {
      this.IsCacheAvailable = false;
      return this.Keys.Remove(item);
    }

    public IEnumerator<CurveKey> GetEnumerator() => (IEnumerator<CurveKey>) this.Keys.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.Keys.GetEnumerator();

    public CurveKeyCollection Clone() => new CurveKeyCollection()
    {
      Keys = new List<CurveKey>((IEnumerable<CurveKey>) this.Keys),
      InvTimeRange = this.InvTimeRange,
      TimeRange = this.TimeRange,
      IsCacheAvailable = true
    };

    internal void ComputeCacheValues()
    {
      this.TimeRange = this.InvTimeRange = 0.0f;
      if (this.Keys.Count > 1)
      {
        this.TimeRange = this.Keys[this.Keys.Count - 1].Position - this.Keys[0].Position;
        if ((double) this.TimeRange > 1.40129846432482E-45)
          this.InvTimeRange = 1f / this.TimeRange;
      }
      this.IsCacheAvailable = true;
    }

    protected class VRageMath_CurveKeyCollection\u003C\u003EKeys\u003C\u003EAccessor : IMemberAccessor<CurveKeyCollection, List<CurveKey>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKeyCollection owner, in List<CurveKey> value) => owner.Keys = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKeyCollection owner, out List<CurveKey> value) => value = owner.Keys;
    }

    protected class VRageMath_CurveKeyCollection\u003C\u003EIsCacheAvailable\u003C\u003EAccessor : IMemberAccessor<CurveKeyCollection, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKeyCollection owner, in bool value) => owner.IsCacheAvailable = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKeyCollection owner, out bool value) => value = owner.IsCacheAvailable;
    }

    protected class VRageMath_CurveKeyCollection\u003C\u003ETimeRange\u003C\u003EAccessor : IMemberAccessor<CurveKeyCollection, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKeyCollection owner, in float value) => owner.TimeRange = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKeyCollection owner, out float value) => value = owner.TimeRange;
    }

    protected class VRageMath_CurveKeyCollection\u003C\u003EInvTimeRange\u003C\u003EAccessor : IMemberAccessor<CurveKeyCollection, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKeyCollection owner, in float value) => owner.InvTimeRange = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKeyCollection owner, out float value) => value = owner.InvTimeRange;
    }
  }
}
