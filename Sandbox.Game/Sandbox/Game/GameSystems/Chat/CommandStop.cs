// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.CommandStop
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;

namespace Sandbox.Game.GameSystems.Chat
{
  [StaticEventOwner]
  public class CommandStop : IMyChatCommand
  {
    public string CommandText => "/stop";

    public string HelpText => "ChatCommand_Help_Stop";

    public string HelpSimpleText => "ChatCommand_HelpSimple_Stop";

    public MyPromoteLevel VisibleTo => MyPromoteLevel.Admin;

    public void Handle(string[] args)
    {
      string empty = string.Empty;
      if (args != null && args.Length != 0)
        empty = args[0];
      MyMultiplayer.RaiseStaticEvent<ulong, string>((Func<IMyEventOwner, Action<ulong, string>>) (x => new Action<ulong, string>(CommandStop.Stop)), Sync.MyId, empty);
    }

    [Event(null, 550)]
    [Reliable]
    [Server]
    public static void Stop(ulong requester, string name)
    {
      if (!Sync.IsDedicated)
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_StopRequiresDS));
      else if (MySession.Static.GetUserPromoteLevel(MyEventContext.Current.Sender.Value) < MyPromoteLevel.Admin)
      {
        MyEventContext.ValidationFailed();
      }
      else
      {
        MySandboxGame.Log.WriteLineAndConsole("Executing /stop command");
        MySandboxGame.ExitThreadSafe();
      }
    }

    public static void SaveFinish(ulong requesting)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(requesting, 0);
      if (identityId == 0L)
        return;
      MyMultiplayer.Static.SendChatMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_StopExecuting), ChatChannel.GlobalScripted, identityId, MyTexts.GetString(MySpaceTexts.ChatBotName));
    }

    protected sealed class Stop\u003C\u003ESystem_UInt64\u0023System_String : ICallSite<IMyEventOwner, ulong, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong requester,
        in string name,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        CommandStop.Stop(requester, name);
      }
    }
  }
}
