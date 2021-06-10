// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudNotification
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyHudNotification : MyHudNotificationBase, IMyHudNotification
  {
    private MyStringId m_originalText;

    public MyStringId Text
    {
      get => this.m_originalText;
      set
      {
        if (!(this.m_originalText != value))
          return;
        this.m_originalText = value;
        this.SetTextDirty();
      }
    }

    public MyHudNotification(
      MyStringId text = default (MyStringId),
      int disappearTimeMs = 2500,
      string font = "Blue",
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
      int priority = 0,
      MyNotificationLevel level = MyNotificationLevel.Normal)
      : base(disappearTimeMs, font, textAlign, priority, level)
    {
      this.m_originalText = text;
    }

    protected override string GetOriginalText() => MyTexts.Get(this.m_originalText).ToString();

    string IMyHudNotification.Text
    {
      get => this.GetText();
      set => this.SetTextFormatArguments((object) value);
    }

    int IMyHudNotification.AliveTime
    {
      get => this.m_lifespanMs;
      set
      {
        this.m_lifespanMs = value;
        this.ResetAliveTime();
      }
    }

    string IMyHudNotification.Font
    {
      get => this.Font;
      set => this.Font = value;
    }

    void IMyHudNotification.Show() => MyHud.Notifications.Add((MyHudNotificationBase) this);

    void IMyHudNotification.Hide() => MyHud.Notifications.Remove((MyHudNotificationBase) this);

    void IMyHudNotification.ResetAliveTime() => this.ResetAliveTime();
  }
}
