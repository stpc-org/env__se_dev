// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyUnifiedChatHistory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Entities.Character
{
  public class MyUnifiedChatHistory
  {
    protected Queue<MyUnifiedChatItem> m_chat = new Queue<MyUnifiedChatItem>();

    public void EnqueueMessage(
      string text,
      ChatChannel channel,
      long senderId,
      long targetId = 0,
      DateTime? timestamp = null,
      string authorFont = "Blue")
    {
      DateTime timestamp1 = !timestamp.HasValue || !timestamp.HasValue ? DateTime.UtcNow : timestamp.Value;
      MyUnifiedChatItem myUnifiedChatItem;
      switch (channel)
      {
        case ChatChannel.Global:
        case ChatChannel.GlobalScripted:
          myUnifiedChatItem = MyUnifiedChatItem.CreateGlobalMessage(text, timestamp1, senderId, authorFont);
          break;
        case ChatChannel.Faction:
          myUnifiedChatItem = MyUnifiedChatItem.CreateFactionMessage(text, timestamp1, senderId, targetId, authorFont);
          break;
        case ChatChannel.Private:
          myUnifiedChatItem = MyUnifiedChatItem.CreatePrivateMessage(text, timestamp1, senderId, targetId, authorFont);
          break;
        case ChatChannel.ChatBot:
          myUnifiedChatItem = MyUnifiedChatItem.CreateChatbotMessage(text, timestamp1, senderId, targetId, authorFont: authorFont);
          break;
        default:
          myUnifiedChatItem = (MyUnifiedChatItem) null;
          break;
      }
      if (myUnifiedChatItem == null)
        return;
      this.EnqueueMessage(ref myUnifiedChatItem);
    }

    public void EnqueueMessageScripted(string text, string customAuthor, string authorFont = "Blue")
    {
      MyUnifiedChatItem scriptedMessage = MyUnifiedChatItem.CreateScriptedMessage(text, DateTime.UtcNow, customAuthor, authorFont);
      this.EnqueueMessage(ref scriptedMessage);
    }

    public void EnqueueMessage(ref MyUnifiedChatItem item) => this.m_chat.Enqueue(item);

    public void GetCompleteHistory(ref List<MyUnifiedChatItem> list)
    {
      foreach (MyUnifiedChatItem myUnifiedChatItem in this.m_chat)
        list.Add(myUnifiedChatItem);
    }

    public void GetGeneralHistory(ref List<MyUnifiedChatItem> list)
    {
      foreach (MyUnifiedChatItem myUnifiedChatItem in this.m_chat)
      {
        if (myUnifiedChatItem.Channel == ChatChannel.Global || myUnifiedChatItem.Channel == ChatChannel.GlobalScripted)
          list.Add(myUnifiedChatItem);
      }
    }

    public void GetFactionHistory(ref List<MyUnifiedChatItem> list, long factionId)
    {
      foreach (MyUnifiedChatItem myUnifiedChatItem in this.m_chat)
      {
        if (myUnifiedChatItem.Channel == ChatChannel.Faction && myUnifiedChatItem.TargetId == factionId)
          list.Add(myUnifiedChatItem);
      }
    }

    public void GetPrivateHistory(ref List<MyUnifiedChatItem> list, long playerId)
    {
      foreach (MyUnifiedChatItem myUnifiedChatItem in this.m_chat)
      {
        if (myUnifiedChatItem.Channel == ChatChannel.Private && (myUnifiedChatItem.TargetId == playerId || myUnifiedChatItem.SenderId == playerId))
          list.Add(myUnifiedChatItem);
      }
    }

    public void GetChatbotHistory(ref List<MyUnifiedChatItem> list)
    {
      foreach (MyUnifiedChatItem myUnifiedChatItem in this.m_chat)
      {
        if (myUnifiedChatItem.Channel == ChatChannel.ChatBot)
          list.Add(myUnifiedChatItem);
      }
    }

    public void ClearNonGlobalHistory()
    {
      Queue<MyUnifiedChatItem> myUnifiedChatItemQueue = new Queue<MyUnifiedChatItem>();
      foreach (MyUnifiedChatItem myUnifiedChatItem in this.m_chat)
      {
        switch (myUnifiedChatItem.Channel)
        {
          case ChatChannel.Global:
          case ChatChannel.GlobalScripted:
          case ChatChannel.ChatBot:
            myUnifiedChatItemQueue.Enqueue(myUnifiedChatItem);
            continue;
          default:
            continue;
        }
      }
      this.m_chat = myUnifiedChatItemQueue;
    }
  }
}
