// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyReputationNotification
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;

namespace Sandbox.Game.Multiplayer
{
  internal class MyReputationNotification
  {
    private string m_notificationTag = string.Empty;
    private int m_notificationValue;
    private MyHudNotification m_notification;

    internal MyReputationNotification(MyHudNotification notification) => this.m_notification = notification;

    internal void UpdateReputationNotification(in string newTag, in int valueChange)
    {
      if (!this.m_notification.Alive)
      {
        this.m_notificationTag = newTag;
        this.m_notificationValue = valueChange;
        this.m_notification.SetTextFormatArguments((object) this.m_notificationTag, (object) this.m_notificationValue);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_notification);
      }
      else
      {
        if (this.m_notificationTag == newTag)
        {
          this.m_notificationValue += valueChange;
          this.m_notification.ResetAliveTime();
        }
        else
        {
          this.m_notificationTag = newTag;
          this.m_notificationValue = valueChange;
        }
        this.m_notification.SetTextFormatArguments((object) this.m_notificationTag, (object) this.m_notificationValue);
        MyHud.Notifications.Update((MyHudNotificationBase) this.m_notification);
      }
    }
  }
}
