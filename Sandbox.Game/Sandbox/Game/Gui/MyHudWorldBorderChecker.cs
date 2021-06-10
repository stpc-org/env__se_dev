// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudWorldBorderChecker
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
  public class MyHudWorldBorderChecker
  {
    private static readonly float WARNING_DISTANCE = 600f;
    private MyHudNotification m_notification = new MyHudNotification(MyCommonTexts.NotificationLeavingWorld, MyHudNotificationBase.INFINITE, "Red");
    private MyHudNotification m_notificationCreative = new MyHudNotification(MyCommonTexts.NotificationLeavingWorld_Creative, MyHudNotificationBase.INFINITE, "Red");
    internal static MyHudEntityParams HudEntityParams = new MyHudEntityParams(MyTexts.Get(MyCommonTexts.HudMarker_ReturnToWorld), 0L, MyHudIndicatorFlagsEnum.SHOW_TEXT | MyHudIndicatorFlagsEnum.SHOW_BORDER_INDICATORS);

    public bool WorldCenterHintVisible { get; private set; }

    public void Update()
    {
      if (MySession.Static.ControlledEntity == null)
        return;
      float num1 = MyEntities.WorldHalfExtent();
      double num2 = MySession.Static.ControlledEntity.Entity != null ? MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition().AbsMax() : 0.0;
      if ((double) num1 != 0.0 && MySession.Static.ControlledEntity.Entity != null && (double) num1 - num2 < (double) MyHudWorldBorderChecker.WARNING_DISTANCE)
      {
        double num3 = (double) num1 - num2 > 0.0 ? (double) num1 - num2 : 0.0;
        if (MySession.Static.SurvivalMode)
        {
          this.m_notification.SetTextFormatArguments((object) num3);
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_notification);
        }
        else
        {
          this.m_notificationCreative.SetTextFormatArguments((object) num3);
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_notificationCreative);
        }
        this.WorldCenterHintVisible = true;
      }
      else
      {
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_notification);
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_notificationCreative);
        this.WorldCenterHintVisible = false;
      }
    }
  }
}
