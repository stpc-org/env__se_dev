// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MovementData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public struct MovementData
  {
    [ProtoMember(10)]
    public SerializableVector3 MoveVector;
    [ProtoMember(13)]
    public SerializableVector3 RotateVector;
    [ProtoMember(16)]
    public byte MovementFlags;

    protected class VRage_Game_ObjectBuilders_Components_MovementData\u003C\u003EMoveVector\u003C\u003EAccessor : IMemberAccessor<MovementData, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MovementData owner, in SerializableVector3 value) => owner.MoveVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MovementData owner, out SerializableVector3 value) => value = owner.MoveVector;
    }

    protected class VRage_Game_ObjectBuilders_Components_MovementData\u003C\u003ERotateVector\u003C\u003EAccessor : IMemberAccessor<MovementData, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MovementData owner, in SerializableVector3 value) => owner.RotateVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MovementData owner, out SerializableVector3 value) => value = owner.RotateVector;
    }

    protected class VRage_Game_ObjectBuilders_Components_MovementData\u003C\u003EMovementFlags\u003C\u003EAccessor : IMemberAccessor<MovementData, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MovementData owner, in byte value) => owner.MovementFlags = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MovementData owner, out byte value) => value = owner.MovementFlags;
    }

    private class VRage_Game_ObjectBuilders_Components_MovementData\u003C\u003EActor : IActivator, IActivator<MovementData>
    {
      object IActivator.CreateInstance() => (object) new MovementData();

      MovementData IActivator<MovementData>.CreateInstance() => new MovementData();
    }
  }
}
