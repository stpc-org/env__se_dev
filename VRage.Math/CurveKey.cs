// Decompiled with JetBrains decompiler
// Type: VRageMath.CurveKey
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [TypeConverter(typeof (ExpandableObjectConverter))]
  [Serializable]
  public class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>
  {
    internal float position;
    internal float internalValue;
    internal float tangentOut;
    internal float tangentIn;
    internal CurveContinuity continuity;

    public CurveKey()
    {
    }

    public float Position => this.position;

    public float Value
    {
      get => this.internalValue;
      set => this.internalValue = value;
    }

    public float TangentIn
    {
      get => this.tangentIn;
      set => this.tangentIn = value;
    }

    public float TangentOut
    {
      get => this.tangentOut;
      set => this.tangentOut = value;
    }

    public CurveContinuity Continuity
    {
      get => this.continuity;
      set => this.continuity = value;
    }

    public CurveKey(float position, float value)
    {
      this.position = position;
      this.internalValue = value;
    }

    public CurveKey(float position, float value, float tangentIn, float tangentOut)
    {
      this.position = position;
      this.internalValue = value;
      this.tangentIn = tangentIn;
      this.tangentOut = tangentOut;
    }

    public CurveKey(
      float position,
      float value,
      float tangentIn,
      float tangentOut,
      CurveContinuity continuity)
    {
      this.position = position;
      this.internalValue = value;
      this.tangentIn = tangentIn;
      this.tangentOut = tangentOut;
      this.continuity = continuity;
    }

    public static bool operator ==(CurveKey a, CurveKey b)
    {
      bool flag1 = (object) a == null;
      bool flag2 = (object) b == null;
      return !(flag1 | flag2) ? a.Equals(b) : flag1 == flag2;
    }

    public static bool operator !=(CurveKey a, CurveKey b)
    {
      bool flag1 = a == (CurveKey) null;
      bool flag2 = b == (CurveKey) null;
      if (flag1 | flag2)
        return flag1 != flag2;
      return (double) a.position != (double) b.position || (double) a.internalValue != (double) b.internalValue || ((double) a.tangentIn != (double) b.tangentIn || (double) a.tangentOut != (double) b.tangentOut) || a.continuity != b.continuity;
    }

    public CurveKey Clone() => new CurveKey(this.position, this.internalValue, this.tangentIn, this.tangentOut, this.continuity);

    public bool Equals(CurveKey other) => other != (CurveKey) null && (double) other.position == (double) this.position && ((double) other.internalValue == (double) this.internalValue && (double) other.tangentIn == (double) this.tangentIn) && (double) other.tangentOut == (double) this.tangentOut && other.continuity == this.continuity;

    public override bool Equals(object obj) => this.Equals(obj as CurveKey);

    public override int GetHashCode() => this.position.GetHashCode() + this.internalValue.GetHashCode() + this.tangentIn.GetHashCode() + this.tangentOut.GetHashCode() + this.continuity.GetHashCode();

    public int CompareTo(CurveKey other)
    {
      if ((double) this.position == (double) other.position)
        return 0;
      return (double) this.position < (double) other.position ? -1 : 1;
    }

    protected class VRageMath_CurveKey\u003C\u003Eposition\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.position;
    }

    protected class VRageMath_CurveKey\u003C\u003EinternalValue\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.internalValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.internalValue;
    }

    protected class VRageMath_CurveKey\u003C\u003EtangentOut\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.tangentOut = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.tangentOut;
    }

    protected class VRageMath_CurveKey\u003C\u003EtangentIn\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.tangentIn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.tangentIn;
    }

    protected class VRageMath_CurveKey\u003C\u003Econtinuity\u003C\u003EAccessor : IMemberAccessor<CurveKey, CurveContinuity>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in CurveContinuity value) => owner.continuity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out CurveContinuity value) => value = owner.continuity;
    }

    protected class VRageMath_CurveKey\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.Value;
    }

    protected class VRageMath_CurveKey\u003C\u003ETangentIn\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.TangentIn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.TangentIn;
    }

    protected class VRageMath_CurveKey\u003C\u003ETangentOut\u003C\u003EAccessor : IMemberAccessor<CurveKey, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in float value) => owner.TangentOut = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out float value) => value = owner.TangentOut;
    }

    protected class VRageMath_CurveKey\u003C\u003EContinuity\u003C\u003EAccessor : IMemberAccessor<CurveKey, CurveContinuity>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CurveKey owner, in CurveContinuity value) => owner.Continuity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CurveKey owner, out CurveContinuity value) => value = owner.Continuity;
    }
  }
}
