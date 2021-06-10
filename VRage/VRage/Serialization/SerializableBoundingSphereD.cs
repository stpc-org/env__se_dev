// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.SerializableBoundingSphereD
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Serialization
{
  [ProtoContract]
  public struct SerializableBoundingSphereD
  {
    [ProtoMember(1)]
    public SerializableVector3D Center;
    [ProtoMember(4)]
    public double Radius;

    public static implicit operator BoundingSphereD(SerializableBoundingSphereD v) => new BoundingSphereD((Vector3D) v.Center, v.Radius);

    public static implicit operator SerializableBoundingSphereD(
      BoundingSphereD v)
    {
      return new SerializableBoundingSphereD()
      {
        Center = (SerializableVector3D) v.Center,
        Radius = v.Radius
      };
    }

    protected class VRage_Serialization_SerializableBoundingSphereD\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<SerializableBoundingSphereD, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBoundingSphereD owner, in SerializableVector3D value) => owner.Center = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBoundingSphereD owner, out SerializableVector3D value) => value = owner.Center;
    }

    protected class VRage_Serialization_SerializableBoundingSphereD\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<SerializableBoundingSphereD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBoundingSphereD owner, in double value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBoundingSphereD owner, out double value) => value = owner.Radius;
    }
  }
}
