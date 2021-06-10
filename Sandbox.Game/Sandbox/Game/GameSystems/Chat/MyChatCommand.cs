// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.MyChatCommand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems.Chat
{
  internal class MyChatCommand : IMyChatCommand
  {
    private readonly Action<string[]> m_action;

    public string CommandText { get; private set; }

    public string HelpText { get; private set; }

    public string HelpSimpleText { get; private set; }

    public MyPromoteLevel VisibleTo { get; private set; }

    public void Handle(string[] args) => this.m_action(args);

    public MyChatCommand(
      string commandText,
      string helpText,
      string helpSimpleText,
      Action<string[]> action,
      MyPromoteLevel visibleTo = MyPromoteLevel.None)
    {
      this.CommandText = commandText;
      this.HelpText = helpText;
      this.HelpSimpleText = helpSimpleText;
      this.m_action = action;
      this.VisibleTo = visibleTo;
    }
  }
}
