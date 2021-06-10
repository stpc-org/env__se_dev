// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.HudViewers.MyHudControlChat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI.HudViewers
{
  public class MyHudControlChat : MyGuiControlMultilineText
  {
    public static readonly float FADE_OUT_TIME = 10000f;
    public static float SCROLL_SPEED = 0.03f;
    private int m_displayedMessageCount;
    private MyHudChat m_chat;
    private int m_lastTimestamp;
    private bool m_forceUpdate;
    private MyHudControlChat.MyChatVisibilityEnum m_visibility;
    private float m_fadeOut = 1f;
    private float m_scrollPosition = 1f;

    public MyHudControlChat.MyChatVisibilityEnum Visibility
    {
      get => this.m_visibility;
      set
      {
        this.m_visibility = value;
        this.m_forceUpdate = true;
        this.UpdateText();
        switch (value)
        {
          case MyHudControlChat.MyChatVisibilityEnum.Fade:
            break;
          case MyHudControlChat.MyChatVisibilityEnum.AlwaysVisible:
            break;
          case MyHudControlChat.MyChatVisibilityEnum.AlwaysHidden:
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public void ScrollDown()
    {
      this.m_scrollPosition += MyHudControlChat.SCROLL_SPEED;
      float num = this.m_scrollbarV.MaxSize - this.m_scrollbarV.PageSize;
      if ((double) this.m_scrollPosition >= (double) num)
        this.m_scrollPosition = num;
      this.SetScrollbarValueV = this.m_scrollPosition;
    }

    public void ScrollUp()
    {
      this.m_scrollPosition -= MyHudControlChat.SCROLL_SPEED;
      if ((double) this.m_scrollPosition <= 0.0)
        this.m_scrollPosition = 0.0f;
      this.SetScrollbarValueV = this.m_scrollPosition;
    }

    public MyHudControlChat(
      MyHudChat chat,
      Vector2? position = null,
      Vector2? size = null,
      Vector4? backgroundColor = null,
      string font = "White",
      float textScale = 0.5f,
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
      StringBuilder contents = null,
      MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM,
      int? visibleLinesCount = null,
      bool selectable = false)
      : base(position, size, backgroundColor, font, textScale, textAlign, contents, drawScrollbarH: false, textBoxAlign: textBoxAlign, selectable: selectable, showTextShadow: true)
    {
      this.m_forceUpdate = true;
      this.m_chat = chat;
      this.m_chat.ChatControl = this;
      this.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.VisibleChanged += new VisibleChangedDelegate(this.MyHudControlChat_VisibleChanged);
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      this.UpdateText();
      base.Draw(transitionAlpha * this.m_fadeOut, backgroundTransitionAlpha * this.m_fadeOut);
    }

    private void MyHudControlChat_VisibleChanged(object sender, bool isVisible)
    {
      if (isVisible)
        return;
      this.m_forceUpdate = true;
    }

    private void UpdateText()
    {
      if ((double) this.m_chat.TimeSinceLastUpdate > (double) MyHudControlChat.FADE_OUT_TIME)
      {
        this.m_fadeOut -= 0.01f;
        if ((double) this.m_fadeOut < 0.0)
          this.m_fadeOut = 0.0f;
      }
      else
        this.m_fadeOut = 1f;
      if (!this.m_forceUpdate && this.m_lastTimestamp == this.m_chat.Timestamp)
        return;
      float num1 = this.m_scrollbarV.Value;
      bool flag = true;
      float num2 = this.m_scrollbarV.MaxSize - this.m_scrollbarV.PageSize;
      if ((double) num2 > 0.0 && (double) this.m_scrollbarV.Value < (double) num2)
        flag = false;
      this.Clear();
      bool showChatTimestamp = MySandboxGame.Config.ShowChatTimestamp;
      for (int index = 0; index < this.m_chat.MessageHistory.Count; ++index)
      {
        MyChatItem myChatItem = this.m_chat.MessageHistory[index];
        StringBuilder text = new StringBuilder(myChatItem.Sender);
        if (showChatTimestamp)
          this.AppendText(new StringBuilder("[").Append(myChatItem.Timestamp.ToLongTimeString()).Append("] "), myChatItem.Font, this.TextScale, (Vector4) Color.LightGray);
        text.Append(": ");
        this.AppendText(text, myChatItem.Font, this.TextScale, (Vector4) myChatItem.SenderColor);
        this.AppendText(new StringBuilder(myChatItem.Message), "White", this.TextScale, (Vector4) myChatItem.MessageColor);
        this.AppendLine();
      }
      this.m_displayedMessageCount = this.m_chat.MessageHistory.Count;
      this.m_forceUpdate = false;
      this.m_lastTimestamp = this.m_chat.Timestamp;
      this.RecalculateScrollBar();
      if (flag)
        this.m_scrollbarV.Value = this.m_scrollbarV.MaxSize - this.m_scrollbarV.PageSize;
      else
        this.m_scrollbarV.Value = num1;
    }

    public enum MyChatVisibilityEnum
    {
      Fade,
      AlwaysVisible,
      AlwaysHidden,
    }
  }
}
