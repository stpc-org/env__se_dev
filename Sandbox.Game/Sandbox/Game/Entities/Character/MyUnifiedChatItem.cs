// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyUnifiedChatItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities.Character
{
  [Serializable]
  public class MyUnifiedChatItem
  {
    public string AuthorFont = "Blue";

    public string Text { set; get; }

    public DateTime Timestamp { set; get; }

    public ChatChannel Channel { set; get; }

    public string CustomAuthor { set; get; }

    public long SenderId { set; get; }

    public long TargetId { set; get; }

    public MyUnifiedChatItem()
    {
      this.Text = string.Empty;
      this.Timestamp = DateTime.UtcNow;
      this.Channel = ChatChannel.Global;
      this.CustomAuthor = string.Empty;
      this.AuthorFont = string.Empty;
      this.SenderId = 0L;
      this.TargetId = 0L;
    }

    public static MyUnifiedChatItem CreateGlobalMessage(
      string text,
      DateTime timestamp,
      long senderId,
      string authorFont = "Blue")
    {
      return new MyUnifiedChatItem()
      {
        Text = text,
        Timestamp = timestamp,
        Channel = ChatChannel.Global,
        CustomAuthor = string.Empty,
        SenderId = senderId,
        TargetId = 0,
        AuthorFont = authorFont
      };
    }

    public static MyUnifiedChatItem CreateFactionMessage(
      string text,
      DateTime timestamp,
      long senderId,
      long targetId,
      string authorFont = "Blue")
    {
      return new MyUnifiedChatItem()
      {
        Text = text,
        Timestamp = timestamp,
        Channel = ChatChannel.Faction,
        CustomAuthor = string.Empty,
        SenderId = senderId,
        TargetId = targetId,
        AuthorFont = authorFont
      };
    }

    public static MyUnifiedChatItem CreatePrivateMessage(
      string text,
      DateTime timestamp,
      long senderId,
      long targetId,
      string authorFont = "Blue")
    {
      return new MyUnifiedChatItem()
      {
        Text = text,
        Timestamp = timestamp,
        Channel = ChatChannel.Private,
        CustomAuthor = string.Empty,
        SenderId = senderId,
        TargetId = targetId,
        AuthorFont = authorFont
      };
    }

    public static MyUnifiedChatItem CreateScriptedMessage(
      string text,
      DateTime timestamp,
      string customAuthor,
      string authorFont = "Blue")
    {
      return new MyUnifiedChatItem()
      {
        Text = text,
        Timestamp = timestamp,
        Channel = ChatChannel.GlobalScripted,
        CustomAuthor = string.IsNullOrEmpty(customAuthor) ? string.Empty : customAuthor,
        SenderId = 0,
        TargetId = 0,
        AuthorFont = authorFont
      };
    }

    public static MyUnifiedChatItem CreateChatbotMessage(
      string text,
      DateTime timestamp,
      long senderId,
      long targetId = 0,
      string customAuthor = null,
      string authorFont = "Blue")
    {
      return new MyUnifiedChatItem()
      {
        Text = text,
        Timestamp = timestamp,
        Channel = ChatChannel.ChatBot,
        CustomAuthor = string.IsNullOrEmpty(customAuthor) ? string.Empty : customAuthor,
        SenderId = senderId,
        TargetId = targetId,
        AuthorFont = authorFont
      };
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003EAuthorFont\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in string value) => owner.AuthorFont = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out string value) => value = owner.AuthorFont;
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out string value) => value = owner.Text;
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003ETimestamp\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, DateTime>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in DateTime value) => owner.Timestamp = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out DateTime value) => value = owner.Timestamp;
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003EChannel\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, ChatChannel>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in ChatChannel value) => owner.Channel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out ChatChannel value) => value = owner.Channel;
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003ECustomAuthor\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in string value) => owner.CustomAuthor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out string value) => value = owner.CustomAuthor;
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003ESenderId\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in long value) => owner.SenderId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out long value) => value = owner.SenderId;
    }

    protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem\u003C\u003ETargetId\u003C\u003EAccessor : IMemberAccessor<MyUnifiedChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUnifiedChatItem owner, in long value) => owner.TargetId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUnifiedChatItem owner, out long value) => value = owner.TargetId;
    }
  }
}
