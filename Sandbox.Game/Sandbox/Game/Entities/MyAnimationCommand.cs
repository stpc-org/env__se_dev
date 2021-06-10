// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyAnimationCommand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.Entities
{
  [Serializable]
  public struct MyAnimationCommand
  {
    [Serialize(MyObjectFlags.DefaultZero)]
    public string AnimationSubtypeName;
    public MyPlaybackCommand PlaybackCommand;
    public MyBlendOption BlendOption;
    public MyFrameOption FrameOption;
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Area;
    public float BlendTime;
    public float TimeScale;
    public bool ExcludeLegsWhenMoving;
    public bool KeepContinuingAnimations;

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EAnimationSubtypeName\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in string value) => owner.AnimationSubtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out string value) => value = owner.AnimationSubtypeName;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EPlaybackCommand\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, MyPlaybackCommand>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in MyPlaybackCommand value) => owner.PlaybackCommand = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out MyPlaybackCommand value) => value = owner.PlaybackCommand;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EBlendOption\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, MyBlendOption>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in MyBlendOption value) => owner.BlendOption = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out MyBlendOption value) => value = owner.BlendOption;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EFrameOption\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, MyFrameOption>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in MyFrameOption value) => owner.FrameOption = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out MyFrameOption value) => value = owner.FrameOption;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EArea\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in string value) => owner.Area = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out string value) => value = owner.Area;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EBlendTime\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in float value) => owner.BlendTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out float value) => value = owner.BlendTime;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003ETimeScale\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in float value) => owner.TimeScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out float value) => value = owner.TimeScale;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EExcludeLegsWhenMoving\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in bool value) => owner.ExcludeLegsWhenMoving = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out bool value) => value = owner.ExcludeLegsWhenMoving;
    }

    protected class Sandbox_Game_Entities_MyAnimationCommand\u003C\u003EKeepContinuingAnimations\u003C\u003EAccessor : IMemberAccessor<MyAnimationCommand, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAnimationCommand owner, in bool value) => owner.KeepContinuingAnimations = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAnimationCommand owner, out bool value) => value = owner.KeepContinuingAnimations;
    }
  }
}
