// Decompiled with JetBrains decompiler
// Type: VRage.Game.AnimationItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct AnimationItem
  {
    [ProtoMember(1)]
    public float Ratio;
    [ProtoMember(4)]
    public string Animation;

    protected class VRage_Game_AnimationItem\u003C\u003ERatio\u003C\u003EAccessor : IMemberAccessor<AnimationItem, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationItem owner, in float value) => owner.Ratio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationItem owner, out float value) => value = owner.Ratio;
    }

    protected class VRage_Game_AnimationItem\u003C\u003EAnimation\u003C\u003EAccessor : IMemberAccessor<AnimationItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationItem owner, in string value) => owner.Animation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationItem owner, out string value) => value = owner.Animation;
    }

    private class VRage_Game_AnimationItem\u003C\u003EActor : IActivator, IActivator<AnimationItem>
    {
      object IActivator.CreateInstance() => (object) new AnimationItem();

      AnimationItem IActivator<AnimationItem>.CreateInstance() => new AnimationItem();
    }
  }
}
