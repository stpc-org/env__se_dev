// Decompiled with JetBrains decompiler
// Type: VRage.Game.RGBAColor
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct RGBAColor
  {
    [ProtoMember(4)]
    public int R;
    [ProtoMember(7)]
    public int G;
    [ProtoMember(10)]
    public int B;
    [ProtoMember(13)]
    public int A;

    protected class VRage_Game_RGBAColor\u003C\u003ER\u003C\u003EAccessor : IMemberAccessor<RGBAColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBAColor owner, in int value) => owner.R = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBAColor owner, out int value) => value = owner.R;
    }

    protected class VRage_Game_RGBAColor\u003C\u003EG\u003C\u003EAccessor : IMemberAccessor<RGBAColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBAColor owner, in int value) => owner.G = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBAColor owner, out int value) => value = owner.G;
    }

    protected class VRage_Game_RGBAColor\u003C\u003EB\u003C\u003EAccessor : IMemberAccessor<RGBAColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBAColor owner, in int value) => owner.B = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBAColor owner, out int value) => value = owner.B;
    }

    protected class VRage_Game_RGBAColor\u003C\u003EA\u003C\u003EAccessor : IMemberAccessor<RGBAColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBAColor owner, in int value) => owner.A = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBAColor owner, out int value) => value = owner.A;
    }

    private class VRage_Game_RGBAColor\u003C\u003EActor : IActivator, IActivator<RGBAColor>
    {
      object IActivator.CreateInstance() => (object) new RGBAColor();

      RGBAColor IActivator<RGBAColor>.CreateInstance() => new RGBAColor();
    }
  }
}
