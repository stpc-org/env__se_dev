// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.CommandSave
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;

namespace Sandbox.Game.GameSystems.Chat
{
  [StaticEventOwner]
  public class CommandSave : IMyChatCommand
  {
    public string CommandText => "/save";

    public string HelpText => "ChatCommand_Help_Save";

    public string HelpSimpleText => "ChatCommand_HelpSimple_Save";

    public MyPromoteLevel VisibleTo => MyPromoteLevel.Admin;

    public void Handle(string[] args)
    {
      string empty = string.Empty;
      if (args != null && args.Length != 0)
        empty = args[0];
      MyMultiplayer.RaiseStaticEvent<ulong, string>((Func<IMyEventOwner, Action<ulong, string>>) (x => new Action<ulong, string>(CommandSave.Save)), Sync.MyId, empty);
    }

    [Event(null, 486)]
    [Reliable]
    [Server]
    public static void Save(ulong requester, string name)
    {
      if (MySession.Static.GetUserPromoteLevel(MyEventContext.Current.Sender.Value) < MyPromoteLevel.Admin)
      {
        MyEventContext.ValidationFailed();
      }
      else
      {
        MySandboxGame.Log.WriteLineAndConsole("Executing /save command");
        MyAsyncSaving.Start((Action) (() => CommandSave.SaveFinish(requester)), string.IsNullOrEmpty(name) ? (string) null : name);
      }
    }

    public static void SaveFinish(ulong requesting)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(requesting, 0);
      if (identityId <= 0L)
        return;
      if (MyMultiplayer.Static != null)
        MyMultiplayer.Static.SendChatMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_ExecutingSaveFinished), ChatChannel.GlobalScripted, identityId, MyTexts.GetString(MySpaceTexts.ChatBotName));
      else
        MyHud.Chat.ShowMessageScripted(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_ExecutingSaveFinished));
      MySandboxGame.Log.WriteLineAndConsole("Saving finished");
    }

    protected sealed class Save\u003C\u003ESystem_UInt64\u0023System_String : ICallSite<IMyEventOwner, ulong, string, DBNull, DBNull, DBNull, DBNull>
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
        CommandSave.Save(requester, name);
      }
    }
  }
}
