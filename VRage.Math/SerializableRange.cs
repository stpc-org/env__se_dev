// Decompiled with JetBrains decompiler
// Type: VRageMath.SerializableRange
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
  public struct SerializableRange
  {
    [ProtoMember(1)]
    [XmlAttribute(AttributeName = "Min")]
    public float Min;
    [ProtoMember(4)]
    [XmlAttribute(AttributeName = "Max")]
    public float Max;

    public SerializableRange(float min, float max)
    {
      this.Max = max;
      this.Min = min;
    }

    public bool ValueBetween(float value) => (double) value >= (double) this.Min && (double) value <= (double) this.Max;

    public override string ToString() => string.Format("Range[{0}, {1}]", (object) this.Min, (object) this.Max);

    public SerializableRange ConvertToCosine()
    {
      float max = this.Max;
      this.Max = (float) Math.Cos((double) this.Min * Math.PI / 180.0);
      this.Min = (float) Math.Cos((double) max * Math.PI / 180.0);
      return this;
    }

    public SerializableRange ConvertToSine()
    {
      this.Max = (float) Math.Sin((double) this.Max * Math.PI / 180.0);
      this.Min = (float) Math.Sin((double) this.Min * Math.PI / 180.0);
      return this;
    }

    public SerializableRange ConvertToCosineLongitude()
    {
      this.Max = MathHelper.MonotonicCosine((float) ((double) this.Max * Math.PI / 180.0));
      this.Min = MathHelper.MonotonicCosine((float) ((double) this.Min * Math.PI / 180.0));
      return this;
    }

    public string ToStringAsin() => string.Format("Range[{0}, {1}]", (object) MathHelper.ToDegrees(Math.Asin((double) this.Min)), (object) MathHelper.ToDegrees(Math.Asin((double) this.Max)));

    public string ToStringAcos() => string.Format("Range[{0}, {1}]", (object) MathHelper.ToDegrees(Math.Acos((double) this.Min)), (object) MathHelper.ToDegrees(Math.Acos((double) this.Max)));

    public string ToStringLongitude() => string.Format("Range[{0}, {1}]", (object) MathHelper.ToDegrees(MathHelper.MonotonicAcos(this.Min)), (object) MathHelper.ToDegrees(MathHelper.MonotonicAcos(this.Max)));

    protected class VRageMath_SerializableRange\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<SerializableRange, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableRange owner, in float value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableRange owner, out float value) => value = owner.Min;
    }

    protected class VRageMath_SerializableRange\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<SerializableRange, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableRange owner, in float value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableRange owner, out float value) => value = owner.Max;
    }
  }
}
