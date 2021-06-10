// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyUIString
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game
{
  [Serializable]
  public struct MyUIString
  {
    public string Text;
    public Vector2 NormalizedCoord;
    public float Scale;
    public string Font;
    public MyGuiDrawAlignEnum DrawAlign;

    protected class Sandbox_Game_MyUIString\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyUIString, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUIString owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUIString owner, out string value) => value = owner.Text;
    }

    protected class Sandbox_Game_MyUIString\u003C\u003ENormalizedCoord\u003C\u003EAccessor : IMemberAccessor<MyUIString, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUIString owner, in Vector2 value) => owner.NormalizedCoord = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUIString owner, out Vector2 value) => value = owner.NormalizedCoord;
    }

    protected class Sandbox_Game_MyUIString\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyUIString, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUIString owner, in float value) => owner.Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUIString owner, out float value) => value = owner.Scale;
    }

    protected class Sandbox_Game_MyUIString\u003C\u003EFont\u003C\u003EAccessor : IMemberAccessor<MyUIString, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUIString owner, in string value) => owner.Font = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUIString owner, out string value) => value = owner.Font;
    }

    protected class Sandbox_Game_MyUIString\u003C\u003EDrawAlign\u003C\u003EAccessor : IMemberAccessor<MyUIString, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUIString owner, in MyGuiDrawAlignEnum value) => owner.DrawAlign = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUIString owner, out MyGuiDrawAlignEnum value) => value = owner.DrawAlign;
    }
  }
}
