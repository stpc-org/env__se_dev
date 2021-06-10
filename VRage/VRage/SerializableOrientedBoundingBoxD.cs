// Decompiled with JetBrains decompiler
// Type: VRage.SerializableOrientedBoundingBoxD
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
  public struct SerializableOrientedBoundingBoxD
  {
    [ProtoMember(1)]
    public SerializableVector3D Center;
    [ProtoMember(4)]
    public SerializableVector3D HalfExtent;
    [ProtoMember(7)]
    public SerializableQuaternion Orientation;

    public static implicit operator MyOrientedBoundingBoxD(
      SerializableOrientedBoundingBoxD v)
    {
      return new MyOrientedBoundingBoxD((Vector3D) v.Center, (Vector3D) v.HalfExtent, (Quaternion) v.Orientation);
    }

    public static implicit operator SerializableOrientedBoundingBoxD(
      MyOrientedBoundingBoxD v)
    {
      return new SerializableOrientedBoundingBoxD()
      {
        Center = (SerializableVector3D) v.Center,
        HalfExtent = (SerializableVector3D) v.HalfExtent,
        Orientation = (SerializableQuaternion) v.Orientation
      };
    }

    protected class VRage_SerializableOrientedBoundingBoxD\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<SerializableOrientedBoundingBoxD, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableOrientedBoundingBoxD owner,
        in SerializableVector3D value)
      {
        owner.Center = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableOrientedBoundingBoxD owner,
        out SerializableVector3D value)
      {
        value = owner.Center;
      }
    }

    protected class VRage_SerializableOrientedBoundingBoxD\u003C\u003EHalfExtent\u003C\u003EAccessor : IMemberAccessor<SerializableOrientedBoundingBoxD, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableOrientedBoundingBoxD owner,
        in SerializableVector3D value)
      {
        owner.HalfExtent = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableOrientedBoundingBoxD owner,
        out SerializableVector3D value)
      {
        value = owner.HalfExtent;
      }
    }

    protected class VRage_SerializableOrientedBoundingBoxD\u003C\u003EOrientation\u003C\u003EAccessor : IMemberAccessor<SerializableOrientedBoundingBoxD, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableOrientedBoundingBoxD owner,
        in SerializableQuaternion value)
      {
        owner.Orientation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableOrientedBoundingBoxD owner,
        out SerializableQuaternion value)
      {
        value = owner.Orientation;
      }
    }
  }
}
