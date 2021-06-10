// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.ChatCommandAttribute
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems.Chat
{
  [AttributeUsage(AttributeTargets.Method)]
  public class ChatCommandAttribute : Attribute
  {
    public string CommandText;
    public string HelpText;
    public string HelpSimpleText;
    public MyPromoteLevel VisibleTo;
    internal bool DebugCommand;

    public ChatCommandAttribute(
      string commandText,
      string helpText,
      string helpSimpleText,
      MyPromoteLevel visibleTo = MyPromoteLevel.None)
    {
      this.CommandText = commandText;
      this.HelpText = helpText;
      this.HelpSimpleText = helpSimpleText;
      this.VisibleTo = visibleTo;
    }

    internal ChatCommandAttribute(
      string commandText,
      string helpText,
      string helpSimpleText,
      bool debugCommand,
      MyPromoteLevel visibleTo = MyPromoteLevel.None)
    {
      this.CommandText = commandText;
      this.HelpText = helpText;
      this.HelpSimpleText = helpSimpleText;
      this.DebugCommand = debugCommand;
      this.VisibleTo = visibleTo;
    }
  }
}
