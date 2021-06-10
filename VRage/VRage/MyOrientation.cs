// Decompiled with JetBrains decompiler
// Type: VRage.MyOrientation
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  public struct MyOrientation
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public float Yaw;
    [ProtoMember(4)]
    [XmlAttribute]
    public float Pitch;
    [ProtoMember(7)]
    [XmlAttribute]
    public float Roll;

    public MyOrientation(float yaw, float pitch, float roll)
    {
      this.Yaw = yaw;
      this.Pitch = pitch;
      this.Roll = roll;
    }

    public Quaternion ToQuaternion() => Quaternion.CreateFromYawPitchRoll(this.Yaw, this.Pitch, this.Roll);

    public override bool Equals(object obj) => obj is MyOrientation myOrientation && this == myOrientation;

    public override int GetHashCode() => ((int) ((double) this.Yaw * 997.0) * 397 ^ (int) ((double) this.Pitch * 997.0)) * 397 ^ (int) ((double) this.Roll * 997.0);

    public static bool operator ==(MyOrientation value1, MyOrientation value2) => (double) value1.Yaw == (double) value2.Yaw && (double) value1.Pitch == (double) value2.Pitch && (double) value1.Roll == (double) value2.Roll;

    public static bool operator !=(MyOrientation value1, MyOrientation value2) => (double) value1.Yaw != (double) value2.Yaw || (double) value1.Pitch != (double) value2.Pitch || (double) value1.Roll != (double) value2.Roll;

    protected class VRage_MyOrientation\u003C\u003EYaw\u003C\u003EAccessor : IMemberAccessor<MyOrientation, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyOrientation owner, in float value) => owner.Yaw = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyOrientation owner, out float value) => value = owner.Yaw;
    }

    protected class VRage_MyOrientation\u003C\u003EPitch\u003C\u003EAccessor : IMemberAccessor<MyOrientation, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyOrientation owner, in float value) => owner.Pitch = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyOrientation owner, out float value) => value = owner.Pitch;
    }

    protected class VRage_MyOrientation\u003C\u003ERoll\u003C\u003EAccessor : IMemberAccessor<MyOrientation, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyOrientation owner, in float value) => owner.Roll = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyOrientation owner, out float value) => value = owner.Roll;
    }
  }
}
