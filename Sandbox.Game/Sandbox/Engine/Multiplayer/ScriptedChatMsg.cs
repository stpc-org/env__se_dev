// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.ScriptedChatMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
  [Serializable]
  public struct ScriptedChatMsg
  {
    public string Text;
    public string Author;
    public long Target;
    public string Font;
    public Color Color;

    protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<ScriptedChatMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScriptedChatMsg owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScriptedChatMsg owner, out string value) => value = owner.Text;
    }

    protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg\u003C\u003EAuthor\u003C\u003EAccessor : IMemberAccessor<ScriptedChatMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScriptedChatMsg owner, in string value) => owner.Author = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScriptedChatMsg owner, out string value) => value = owner.Author;
    }

    protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg\u003C\u003ETarget\u003C\u003EAccessor : IMemberAccessor<ScriptedChatMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScriptedChatMsg owner, in long value) => owner.Target = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScriptedChatMsg owner, out long value) => value = owner.Target;
    }

    protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg\u003C\u003EFont\u003C\u003EAccessor : IMemberAccessor<ScriptedChatMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScriptedChatMsg owner, in string value) => owner.Font = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScriptedChatMsg owner, out string value) => value = owner.Font;
    }

    protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<ScriptedChatMsg, Color>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScriptedChatMsg owner, in Color value) => owner.Color = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScriptedChatMsg owner, out Color value) => value = owner.Color;
    }
  }
}
