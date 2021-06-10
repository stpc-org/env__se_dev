// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudWarning
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using System;
using VRage.Audio;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  internal class MyHudWarning : MyHudNotification
  {
    public int RepeatInterval;
    public Func<bool> CanPlay;
    public Action Played;
    private MyWarningDetectionMethod m_warningDetectionMethod;
    private MyHudWarning.WarningState m_warningState;
    private bool m_warningDetected;
    private int m_msSinceLastStateChange;
    private int m_soundDelay;

    public int WarningPriority { get; private set; }

    public MyHudWarning(
      MyWarningDetectionMethod detectionMethod,
      int priority,
      int repeatInterval = 0,
      int soundDelay = 0,
      int disappearTime = 0)
      : base(disappearTimeMs: disappearTime, font: "Red", level: MyNotificationLevel.Important)
    {
      this.m_warningDetectionMethod = detectionMethod;
      this.RepeatInterval = repeatInterval;
      this.m_soundDelay = soundDelay;
      this.WarningPriority = priority;
      this.m_warningDetected = false;
    }

    public bool Update(bool isWarnedHigherPriority)
    {
      MyGuiSounds cue = MyGuiSounds.None;
      MyStringId text = MySpaceTexts.Blank;
      this.m_warningDetected = false;
      if (!isWarnedHigherPriority)
        this.m_warningDetected = this.m_warningDetectionMethod(out cue, out text) && MyHudWarnings.EnableWarnings;
      this.m_msSinceLastStateChange += 16 * MyHudWarnings.FRAMES_BETWEEN_UPDATE;
      if (this.m_warningDetected)
      {
        switch (this.m_warningState)
        {
          case MyHudWarning.WarningState.NOT_STARTED:
            this.Text = text;
            MyHud.Notifications.Add((MyHudNotificationBase) this);
            this.m_msSinceLastStateChange = 0;
            this.m_warningState = MyHudWarning.WarningState.STARTED;
            break;
          case MyHudWarning.WarningState.STARTED:
            if (this.m_msSinceLastStateChange >= this.m_soundDelay && this.CanPlay())
            {
              MyHudWarnings.EnqueueSound(cue);
              this.m_warningState = MyHudWarning.WarningState.PLAYED;
              this.Played();
              break;
            }
            break;
          case MyHudWarning.WarningState.PLAYED:
            if (this.RepeatInterval > 0 && this.CanPlay())
            {
              MyHud.Notifications.Remove((MyHudNotificationBase) this);
              MyHud.Notifications.Add((MyHudNotificationBase) this);
              MyHudWarnings.EnqueueSound(cue);
              this.Played();
              break;
            }
            break;
        }
      }
      else
      {
        MyHud.Notifications.Remove((MyHudNotificationBase) this);
        MyHudWarnings.RemoveSound(cue);
        this.m_warningState = MyHudWarning.WarningState.NOT_STARTED;
      }
      return this.m_warningDetected;
    }

    private enum WarningState
    {
      NOT_STARTED,
      STARTED,
      PLAYED,
    }
  }
}
