// Decompiled with JetBrains decompiler
// Type: VRage.Game.AnimationSet
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct AnimationSet
  {
    [ProtoMember(7)]
    public float Probability;
    [ProtoMember(10)]
    public bool Continuous;
    [ProtoMember(13)]
    public AnimationItem[] AnimationItems;

    protected class VRage_Game_AnimationSet\u003C\u003EProbability\u003C\u003EAccessor : IMemberAccessor<AnimationSet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationSet owner, in float value) => owner.Probability = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationSet owner, out float value) => value = owner.Probability;
    }

    protected class VRage_Game_AnimationSet\u003C\u003EContinuous\u003C\u003EAccessor : IMemberAccessor<AnimationSet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationSet owner, in bool value) => owner.Continuous = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationSet owner, out bool value) => value = owner.Continuous;
    }

    protected class VRage_Game_AnimationSet\u003C\u003EAnimationItems\u003C\u003EAccessor : IMemberAccessor<AnimationSet, AnimationItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationSet owner, in AnimationItem[] value) => owner.AnimationItems = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationSet owner, out AnimationItem[] value) => value = owner.AnimationItems;
    }

    private class VRage_Game_AnimationSet\u003C\u003EActor : IActivator, IActivator<AnimationSet>
    {
      object IActivator.CreateInstance() => (object) new AnimationSet();

      AnimationSet IActivator<AnimationSet>.CreateInstance() => new AnimationSet();
    }
  }
}
