// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudNotificationBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public abstract class MyHudNotificationBase
  {
    public static readonly int INFINITE;
    private int m_formatArgsCount;
    private object[] m_textFormatArguments = new object[20];
    private MyGuiDrawAlignEnum m_actualTextAlign;
    private int m_aliveTime;
    private string m_notificationText;
    private bool m_isTextDirty;
    public int m_lifespanMs;
    public MyNotificationLevel Level;
    public readonly int Priority;
    public string Font;
    public bool HasFog;

    public bool IsControlsHint => this.Level == MyNotificationLevel.Control;

    public bool Alive { get; private set; }

    public MyHudNotificationBase(
      int disapearTimeMs,
      string font = "Blue",
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
      int priority = 0,
      MyNotificationLevel level = MyNotificationLevel.Normal)
    {
      this.Font = font;
      this.Priority = priority;
      this.HasFog = false;
      this.m_isTextDirty = true;
      this.m_actualTextAlign = textAlign;
      this.AssignFormatArgs((object[]) null);
      this.Level = level;
      this.m_lifespanMs = disapearTimeMs;
      this.m_aliveTime = 0;
      this.Alive = false;
    }

    public void SetTextDirty() => this.m_isTextDirty = true;

    public string GetText()
    {
      if (string.IsNullOrEmpty(this.m_notificationText) || this.m_isTextDirty)
      {
        this.m_notificationText = this.m_formatArgsCount <= 0 ? this.GetOriginalText() : string.Format(this.GetOriginalText(), this.m_textFormatArguments);
        this.m_isTextDirty = false;
      }
      return this.m_notificationText;
    }

    public object[] GetTextFormatArguments() => this.m_textFormatArguments;

    public void SetTextFormatArguments(params object[] arguments)
    {
      this.AssignFormatArgs(arguments);
      this.m_notificationText = (string) null;
      this.GetText();
    }

    public void AddAliveTime(int timeStep)
    {
      this.m_aliveTime += timeStep;
      this.RefreshAlive();
    }

    public void ResetAliveTime()
    {
      this.m_aliveTime = 0;
      this.RefreshAlive();
    }

    protected abstract string GetOriginalText();

    private void RefreshAlive() => this.Alive = this.m_lifespanMs == MyHudNotificationBase.INFINITE || this.m_aliveTime < this.m_lifespanMs;

    private void AssignFormatArgs(object[] args)
    {
      int index = 0;
      this.m_formatArgsCount = 0;
      if (args != null)
      {
        if (this.m_textFormatArguments.Length < args.Length)
          this.m_textFormatArguments = new object[args.Length];
        for (; index < args.Length; ++index)
          this.m_textFormatArguments[index] = args[index];
        this.m_formatArgsCount = args.Length;
      }
      for (; index < this.m_textFormatArguments.Length; ++index)
        this.m_textFormatArguments[index] = (object) "<missing>";
    }

    public virtual void BeforeAdd()
    {
    }

    public virtual void BeforeRemove()
    {
    }
  }
}
