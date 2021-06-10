// Decompiled with JetBrains decompiler
// Type: VRage.Network.ChatMsg
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
  [Serializable]
  public struct ChatMsg
  {
    public string Text;
    public ulong Author;
    public byte Channel;
    public long TargetId;
    public string CustomAuthorName;

    protected class VRage_Network_ChatMsg\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<ChatMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ChatMsg owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ChatMsg owner, out string value) => value = owner.Text;
    }

    protected class VRage_Network_ChatMsg\u003C\u003EAuthor\u003C\u003EAccessor : IMemberAccessor<ChatMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ChatMsg owner, in ulong value) => owner.Author = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ChatMsg owner, out ulong value) => value = owner.Author;
    }

    protected class VRage_Network_ChatMsg\u003C\u003EChannel\u003C\u003EAccessor : IMemberAccessor<ChatMsg, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ChatMsg owner, in byte value) => owner.Channel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ChatMsg owner, out byte value) => value = owner.Channel;
    }

    protected class VRage_Network_ChatMsg\u003C\u003ETargetId\u003C\u003EAccessor : IMemberAccessor<ChatMsg, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ChatMsg owner, in long value) => owner.TargetId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ChatMsg owner, out long value) => value = owner.TargetId;
    }

    protected class VRage_Network_ChatMsg\u003C\u003ECustomAuthorName\u003C\u003EAccessor : IMemberAccessor<ChatMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ChatMsg owner, in string value) => owner.CustomAuthorName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ChatMsg owner, out string value) => value = owner.CustomAuthorName;
    }
  }
}
