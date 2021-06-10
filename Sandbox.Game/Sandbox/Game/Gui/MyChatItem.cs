// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyChatItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyChatItem
  {
    public string Sender;
    public string Message;
    public Color SenderColor;
    public Color MessageColor;
    public string Font;
    public DateTime Timestamp;

    public MyChatItem(string sender, string message, string font, Color senderColor)
      : this(sender, message, font, senderColor, Color.White)
    {
    }

    public MyChatItem(
      string sender,
      string message,
      string font,
      Color senderColor,
      Color messageColor)
    {
      this.Sender = sender;
      this.Message = message;
      this.SenderColor = senderColor;
      this.MessageColor = messageColor;
      this.Font = font;
      this.Timestamp = DateTime.Now;
    }
  }
}
