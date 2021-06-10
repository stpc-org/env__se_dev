// Decompiled with JetBrains decompiler
// Type: VRage.SerializableVector2I
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  public struct SerializableVector2I
  {
    public int X;
    public int Y;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public SerializableVector2I(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    [ProtoMember(1)]
    [XmlAttribute]
    [NoSerialize]
    public int x
    {
      get => this.X;
      set => this.X = value;
    }

    [ProtoMember(4)]
    [XmlAttribute]
    [NoSerialize]
    public int y
    {
      get => this.Y;
      set => this.Y = value;
    }

    public static implicit operator Vector2I(SerializableVector2I v) => new Vector2I(v.X, v.Y);

    public static implicit operator SerializableVector2I(Vector2I v) => new SerializableVector2I(v.X, v.Y);

    protected class VRage_SerializableVector2I\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableVector2I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2I owner, in int value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2I owner, out int value) => value = owner.X;
    }

    protected class VRage_SerializableVector2I\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableVector2I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2I owner, in int value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2I owner, out int value) => value = owner.Y;
    }

    protected class VRage_SerializableVector2I\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableVector2I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2I owner, in int value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2I owner, out int value) => value = owner.x;
    }

    protected class VRage_SerializableVector2I\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableVector2I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2I owner, in int value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2I owner, out int value) => value = owner.y;
    }
  }
}
