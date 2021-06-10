// Decompiled with JetBrains decompiler
// Type: VRage.Game.RGBColor
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct RGBColor
  {
    [ProtoMember(16)]
    public int R;
    [ProtoMember(19)]
    public int G;
    [ProtoMember(22)]
    public int B;

    protected class VRage_Game_RGBColor\u003C\u003ER\u003C\u003EAccessor : IMemberAccessor<RGBColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBColor owner, in int value) => owner.R = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBColor owner, out int value) => value = owner.R;
    }

    protected class VRage_Game_RGBColor\u003C\u003EG\u003C\u003EAccessor : IMemberAccessor<RGBColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBColor owner, in int value) => owner.G = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBColor owner, out int value) => value = owner.G;
    }

    protected class VRage_Game_RGBColor\u003C\u003EB\u003C\u003EAccessor : IMemberAccessor<RGBColor, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RGBColor owner, in int value) => owner.B = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RGBColor owner, out int value) => value = owner.B;
    }

    private class VRage_Game_RGBColor\u003C\u003EActor : IActivator, IActivator<RGBColor>
    {
      object IActivator.CreateInstance() => (object) new RGBColor();

      RGBColor IActivator<RGBColor>.CreateInstance() => new RGBColor();
    }
  }
}
