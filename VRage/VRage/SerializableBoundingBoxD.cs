// Decompiled with JetBrains decompiler
// Type: VRage.SerializableBoundingBoxD
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  public struct SerializableBoundingBoxD
  {
    [ProtoMember(1)]
    public SerializableVector3D Min;
    [ProtoMember(4)]
    public SerializableVector3D Max;

    public static implicit operator BoundingBoxD(SerializableBoundingBoxD v) => new BoundingBoxD((Vector3D) v.Min, (Vector3D) v.Max);

    public static implicit operator SerializableBoundingBoxD(BoundingBoxD v) => new SerializableBoundingBoxD()
    {
      Min = (SerializableVector3D) v.Min,
      Max = (SerializableVector3D) v.Max
    };

    protected class VRage_SerializableBoundingBoxD\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<SerializableBoundingBoxD, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBoundingBoxD owner, in SerializableVector3D value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBoundingBoxD owner, out SerializableVector3D value) => value = owner.Min;
    }

    protected class VRage_SerializableBoundingBoxD\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<SerializableBoundingBoxD, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBoundingBoxD owner, in SerializableVector3D value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBoundingBoxD owner, out SerializableVector3D value) => value = owner.Max;
    }
  }
}
