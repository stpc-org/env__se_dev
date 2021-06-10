// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.InputRecording.MySnapshot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
  [Obfuscation(Exclude = true, Feature = "cw symbol renaming")]
  [Serializable]
  public class MySnapshot
  {
    public MyMouseSnapshot MouseSnapshot { get; set; }

    public List<byte> KeyboardSnapshot { get; set; }

    public List<char> KeyboardSnapshotText { get; set; }

    public MyJoystickStateSnapshot JoystickSnapshot { get; set; }

    public int SnapshotTimestamp { get; set; }

    public Vector2 MouseCursorPosition { get; set; }

    public MyCameraSnapshot CameraSnapshot { get; set; }

    public MyBlockSnapshot BlockSnapshot { get; set; }

    public long TimerRepetitions { get; set; }

    public int TimerFrames { get; set; }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003EMouseSnapshot\u003C\u003EAccessor : IMemberAccessor<MySnapshot, MyMouseSnapshot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in MyMouseSnapshot value) => owner.MouseSnapshot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out MyMouseSnapshot value) => value = owner.MouseSnapshot;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003EKeyboardSnapshot\u003C\u003EAccessor : IMemberAccessor<MySnapshot, List<byte>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in List<byte> value) => owner.KeyboardSnapshot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out List<byte> value) => value = owner.KeyboardSnapshot;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003EKeyboardSnapshotText\u003C\u003EAccessor : IMemberAccessor<MySnapshot, List<char>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in List<char> value) => owner.KeyboardSnapshotText = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out List<char> value) => value = owner.KeyboardSnapshotText;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003EJoystickSnapshot\u003C\u003EAccessor : IMemberAccessor<MySnapshot, MyJoystickStateSnapshot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in MyJoystickStateSnapshot value) => owner.JoystickSnapshot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out MyJoystickStateSnapshot value) => value = owner.JoystickSnapshot;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003ESnapshotTimestamp\u003C\u003EAccessor : IMemberAccessor<MySnapshot, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in int value) => owner.SnapshotTimestamp = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out int value) => value = owner.SnapshotTimestamp;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003EMouseCursorPosition\u003C\u003EAccessor : IMemberAccessor<MySnapshot, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in Vector2 value) => owner.MouseCursorPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out Vector2 value) => value = owner.MouseCursorPosition;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003ECameraSnapshot\u003C\u003EAccessor : IMemberAccessor<MySnapshot, MyCameraSnapshot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in MyCameraSnapshot value) => owner.CameraSnapshot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out MyCameraSnapshot value) => value = owner.CameraSnapshot;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003EBlockSnapshot\u003C\u003EAccessor : IMemberAccessor<MySnapshot, MyBlockSnapshot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in MyBlockSnapshot value) => owner.BlockSnapshot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out MyBlockSnapshot value) => value = owner.BlockSnapshot;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003ETimerRepetitions\u003C\u003EAccessor : IMemberAccessor<MySnapshot, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in long value) => owner.TimerRepetitions = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out long value) => value = owner.TimerRepetitions;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot\u003C\u003ETimerFrames\u003C\u003EAccessor : IMemberAccessor<MySnapshot, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySnapshot owner, in int value) => owner.TimerFrames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySnapshot owner, out int value) => value = owner.TimerFrames;
    }
  }
}
