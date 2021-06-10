// Decompiled with JetBrains decompiler
// Type: VRage.DistantSound
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage
{
  [ProtoContract]
  [XmlType("DistantSound")]
  public sealed class DistantSound
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public float Distance = 50f;
    [ProtoMember(4)]
    [XmlAttribute]
    public float DistanceCrossfade = -1f;
    [ProtoMember(7)]
    [XmlAttribute]
    public string Sound = "";

    protected class VRage_DistantSound\u003C\u003EDistance\u003C\u003EAccessor : IMemberAccessor<DistantSound, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref DistantSound owner, in float value) => owner.Distance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref DistantSound owner, out float value) => value = owner.Distance;
    }

    protected class VRage_DistantSound\u003C\u003EDistanceCrossfade\u003C\u003EAccessor : IMemberAccessor<DistantSound, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref DistantSound owner, in float value) => owner.DistanceCrossfade = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref DistantSound owner, out float value) => value = owner.DistanceCrossfade;
    }

    protected class VRage_DistantSound\u003C\u003ESound\u003C\u003EAccessor : IMemberAccessor<DistantSound, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref DistantSound owner, in string value) => owner.Sound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref DistantSound owner, out string value) => value = owner.Sound;
    }
  }
}
