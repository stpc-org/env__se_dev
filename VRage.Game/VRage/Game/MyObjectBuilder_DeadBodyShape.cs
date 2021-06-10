// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_DeadBodyShape
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyObjectBuilder_DeadBodyShape
  {
    [ProtoMember(23)]
    public SerializableVector3 BoxShapeScale;
    [ProtoMember(24)]
    public SerializableVector3 RelativeCenterOfMass;
    [ProtoMember(25)]
    public SerializableVector3 RelativeShapeTranslation;
    [ProtoMember(26)]
    public float Friction;

    protected class VRage_Game_MyObjectBuilder_DeadBodyShape\u003C\u003EBoxShapeScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DeadBodyShape owner, in SerializableVector3 value) => owner.BoxShapeScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DeadBodyShape owner,
        out SerializableVector3 value)
      {
        value = owner.BoxShapeScale;
      }
    }

    protected class VRage_Game_MyObjectBuilder_DeadBodyShape\u003C\u003ERelativeCenterOfMass\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DeadBodyShape owner, in SerializableVector3 value) => owner.RelativeCenterOfMass = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DeadBodyShape owner,
        out SerializableVector3 value)
      {
        value = owner.RelativeCenterOfMass;
      }
    }

    protected class VRage_Game_MyObjectBuilder_DeadBodyShape\u003C\u003ERelativeShapeTranslation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DeadBodyShape owner, in SerializableVector3 value) => owner.RelativeShapeTranslation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DeadBodyShape owner,
        out SerializableVector3 value)
      {
        value = owner.RelativeShapeTranslation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_DeadBodyShape\u003C\u003EFriction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DeadBodyShape, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DeadBodyShape owner, in float value) => owner.Friction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DeadBodyShape owner, out float value) => value = owner.Friction;
    }

    private class VRage_Game_MyObjectBuilder_DeadBodyShape\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DeadBodyShape>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DeadBodyShape();

      MyObjectBuilder_DeadBodyShape IActivator<MyObjectBuilder_DeadBodyShape>.CreateInstance() => new MyObjectBuilder_DeadBodyShape();
    }
  }
}
