// Decompiled with JetBrains decompiler
// Type: VRageMath.SymmetricSerializableRange
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct SymmetricSerializableRange
  {
    [XmlAttribute(AttributeName = "Min")]
    public float Min;
    [XmlAttribute(AttributeName = "Max")]
    public float Max;
    private bool m_notMirror;

    [XmlAttribute(AttributeName = "Mirror")]
    public bool Mirror
    {
      get => !this.m_notMirror;
      set => this.m_notMirror = !value;
    }

    public SymmetricSerializableRange(float min, float max, bool mirror = true)
    {
      this.Max = max;
      this.Min = min;
      this.m_notMirror = !mirror;
    }

    public bool ValueBetween(float value)
    {
      if (!this.m_notMirror)
        value = Math.Abs(value);
      return (double) value >= (double) this.Min && (double) value <= (double) this.Max;
    }

    public override string ToString() => string.Format("{0}[{1}, {2}]", this.Mirror ? (object) "MirroredRange" : (object) "Range", (object) this.Min, (object) this.Max);

    public SymmetricSerializableRange ConvertToCosine()
    {
      float max = this.Max;
      this.Max = (float) Math.Cos((double) this.Min * Math.PI / 180.0);
      this.Min = (float) Math.Cos((double) max * Math.PI / 180.0);
      return this;
    }

    public SymmetricSerializableRange ConvertToSine()
    {
      this.Max = (float) Math.Sin((double) this.Max * Math.PI / 180.0);
      this.Min = (float) Math.Sin((double) this.Min * Math.PI / 180.0);
      return this;
    }

    public SymmetricSerializableRange ConvertToCosineLongitude()
    {
      this.Max = SymmetricSerializableRange.CosineLongitude(this.Max);
      this.Min = SymmetricSerializableRange.CosineLongitude(this.Min);
      return this;
    }

    private static float CosineLongitude(float angle) => (double) angle <= 0.0 ? (float) Math.Cos((double) angle * Math.PI / 180.0) : 2f - (float) Math.Cos((double) angle * Math.PI / 180.0);

    public string ToStringAsin() => string.Format("Range[{0}, {1}]", (object) MathHelper.ToDegrees(Math.Asin((double) this.Min)), (object) MathHelper.ToDegrees(Math.Asin((double) this.Max)));

    public string ToStringAcos() => string.Format("Range[{0}, {1}]", (object) MathHelper.ToDegrees(Math.Acos((double) this.Min)), (object) MathHelper.ToDegrees(Math.Acos((double) this.Max)));

    protected class VRageMath_SymmetricSerializableRange\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<SymmetricSerializableRange, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SymmetricSerializableRange owner, in float value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SymmetricSerializableRange owner, out float value) => value = owner.Min;
    }

    protected class VRageMath_SymmetricSerializableRange\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<SymmetricSerializableRange, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SymmetricSerializableRange owner, in float value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SymmetricSerializableRange owner, out float value) => value = owner.Max;
    }

    protected class VRageMath_SymmetricSerializableRange\u003C\u003Em_notMirror\u003C\u003EAccessor : IMemberAccessor<SymmetricSerializableRange, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SymmetricSerializableRange owner, in bool value) => owner.m_notMirror = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SymmetricSerializableRange owner, out bool value) => value = owner.m_notMirror;
    }

    protected class VRageMath_SymmetricSerializableRange\u003C\u003EMirror\u003C\u003EAccessor : IMemberAccessor<SymmetricSerializableRange, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SymmetricSerializableRange owner, in bool value) => owner.Mirror = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SymmetricSerializableRange owner, out bool value) => value = owner.Mirror;
    }
  }
}
