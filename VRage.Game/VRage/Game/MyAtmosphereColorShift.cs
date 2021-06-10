// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyAtmosphereColorShift
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public class MyAtmosphereColorShift
  {
    [ProtoMember(69)]
    public SerializableRange R;
    [ProtoMember(70)]
    public SerializableRange G;
    [ProtoMember(71)]
    public SerializableRange B;

    protected class VRage_Game_MyAtmosphereColorShift\u003C\u003ER\u003C\u003EAccessor : IMemberAccessor<MyAtmosphereColorShift, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAtmosphereColorShift owner, in SerializableRange value) => owner.R = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAtmosphereColorShift owner, out SerializableRange value) => value = owner.R;
    }

    protected class VRage_Game_MyAtmosphereColorShift\u003C\u003EG\u003C\u003EAccessor : IMemberAccessor<MyAtmosphereColorShift, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAtmosphereColorShift owner, in SerializableRange value) => owner.G = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAtmosphereColorShift owner, out SerializableRange value) => value = owner.G;
    }

    protected class VRage_Game_MyAtmosphereColorShift\u003C\u003EB\u003C\u003EAccessor : IMemberAccessor<MyAtmosphereColorShift, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAtmosphereColorShift owner, in SerializableRange value) => owner.B = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAtmosphereColorShift owner, out SerializableRange value) => value = owner.B;
    }

    private class VRage_Game_MyAtmosphereColorShift\u003C\u003EActor : IActivator, IActivator<MyAtmosphereColorShift>
    {
      object IActivator.CreateInstance() => (object) new MyAtmosphereColorShift();

      MyAtmosphereColorShift IActivator<MyAtmosphereColorShift>.CreateInstance() => new MyAtmosphereColorShift();
    }
  }
}
