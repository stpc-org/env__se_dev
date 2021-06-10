// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.IMyChatCommand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems.Chat
{
  public interface IMyChatCommand
  {
    string CommandText { get; }

    string HelpText { get; }

    string HelpSimpleText { get; }

    MyPromoteLevel VisibleTo { get; }

    void Handle(string[] args);
  }
}
