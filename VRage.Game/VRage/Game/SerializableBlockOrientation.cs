// Decompiled with JetBrains decompiler
// Type: VRage.Game.SerializableBlockOrientation
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public struct SerializableBlockOrientation
  {
    public static readonly SerializableBlockOrientation Identity = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
    [ProtoMember(1)]
    [XmlAttribute]
    public Base6Directions.Direction Forward;
    [ProtoMember(4)]
    [XmlAttribute]
    public Base6Directions.Direction Up;

    public SerializableBlockOrientation(
      Base6Directions.Direction forward,
      Base6Directions.Direction up)
    {
      this.Forward = forward;
      this.Up = up;
    }

    public SerializableBlockOrientation(ref Quaternion q)
    {
      this.Forward = Base6Directions.GetForward(q);
      this.Up = Base6Directions.GetUp(q);
    }

    public static implicit operator MyBlockOrientation(
      SerializableBlockOrientation v)
    {
      if (Base6Directions.IsValidBlockOrientation(v.Forward, v.Up))
        return new MyBlockOrientation(v.Forward, v.Up);
      return v.Up == Base6Directions.Direction.Forward ? new MyBlockOrientation(v.Forward, Base6Directions.Direction.Up) : MyBlockOrientation.Identity;
    }

    public static implicit operator SerializableBlockOrientation(
      MyBlockOrientation v)
    {
      return new SerializableBlockOrientation(v.Forward, v.Up);
    }

    public static bool operator ==(SerializableBlockOrientation a, SerializableBlockOrientation b) => a.Forward == b.Forward && a.Up == b.Up;

    public static bool operator !=(SerializableBlockOrientation a, SerializableBlockOrientation b) => a.Forward != b.Forward || a.Up != b.Up;

    protected class VRage_Game_SerializableBlockOrientation\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<SerializableBlockOrientation, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableBlockOrientation owner,
        in Base6Directions.Direction value)
      {
        owner.Forward = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableBlockOrientation owner,
        out Base6Directions.Direction value)
      {
        value = owner.Forward;
      }
    }

    protected class VRage_Game_SerializableBlockOrientation\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<SerializableBlockOrientation, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableBlockOrientation owner,
        in Base6Directions.Direction value)
      {
        owner.Up = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableBlockOrientation owner,
        out Base6Directions.Direction value)
      {
        value = owner.Up;
      }
    }

    private class VRage_Game_SerializableBlockOrientation\u003C\u003EActor : IActivator, IActivator<SerializableBlockOrientation>
    {
      object IActivator.CreateInstance() => (object) new SerializableBlockOrientation();

      SerializableBlockOrientation IActivator<SerializableBlockOrientation>.CreateInstance() => new SerializableBlockOrientation();
    }
  }
}
