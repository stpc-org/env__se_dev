// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.MyChatCommands
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.Chat
{
  public static class MyChatCommands
  {
    private static StringBuilder m_tempBuilder = new StringBuilder();

    public static void PreloadCommands(MyChatCommandSystem system) => AnimationCommands.LoadAnimations(system);

    [ChatCommand("/help", "ChatCommand_Help_Help", "ChatCommand_HelpSimple_Help", MyPromoteLevel.None)]
    private static void CommandHelp(string[] args)
    {
      MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(Sync.MyId);
      if (args == null || args.Length == 0)
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append(MyTexts.GetString(MyCommonTexts.ChatCommand_AvailableControls));
        stringBuilder1.Append("PageUp/Down - " + MyTexts.GetString(MyCommonTexts.ChatCommand_PageUpdown));
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.ChatCommand_AvailableCommands));
        stringBuilder2.Append("?, ");
        foreach (KeyValuePair<string, IMyChatCommand> chatCommand in MySession.Static.ChatSystem.CommandSystem.ChatCommands)
        {
          if (userPromoteLevel >= chatCommand.Value.VisibleTo)
          {
            stringBuilder2.Append(chatCommand.Key);
            stringBuilder2.Append(", ");
          }
        }
        stringBuilder2.Remove(stringBuilder2.Length - 2, 2);
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), stringBuilder1.ToString(), Color.Red);
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), stringBuilder2.ToString(), Color.Red);
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Help), Color.Red);
      }
      else
      {
        IMyChatCommand myChatCommand;
        if (!MySession.Static.ChatSystem.CommandSystem.ChatCommands.TryGetValue(args[0], out myChatCommand))
        {
          if (args[0] == "?")
          {
            MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Help_Question), Color.Red);
            return;
          }
          List<IMyChatCommand> executableCommands = new List<IMyChatCommand>();
          AnimationCommands.GetAnimationCommands(args[0], string.Empty, executableCommands);
          if (executableCommands.Count > 0)
          {
            myChatCommand = executableCommands[0];
          }
          else
          {
            MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NotFound), (object) args[0]), Color.Red);
            return;
          }
        }
        if (userPromoteLevel < myChatCommand.VisibleTo)
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoPermission), Color.Red);
        else
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), myChatCommand.CommandText + ": " + MyTexts.GetString(MyStringId.GetOrCompute(myChatCommand.HelpText)), Color.Red);
      }
    }

    [ChatCommand("/gps", "ChatCommand_Help_GPS", "ChatCommand_HelpSimple_GPS", MyPromoteLevel.None)]
    private static void CommandGPS(string[] args)
    {
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      MyGps gps = new MyGps();
      MySession.Static.Gpss.GetNameForNewCurrent(MyChatCommands.m_tempBuilder);
      gps.Name = MyChatCommands.m_tempBuilder.ToString();
      gps.Description = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromCurrent_Desc).ToString();
      gps.Coords = MySession.Static.LocalHumanPlayer.GetPosition();
      gps.ShowOnHud = true;
      gps.DiscardAt = new TimeSpan?();
      if (args != null && args.Length != 0)
      {
        if (args[0] == "share")
        {
          MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
          if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
            Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SendChatMessage(gps.ToString(), ChatChannel.Global);
          else
            MyHud.Chat.ShowMessageScripted(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_GPSRequireOnline));
        }
        else if (args[0] == "faction")
        {
          MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
          MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId);
          if (playerFaction == null)
            MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_GPSRequireFaction));
          else if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
            Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SendChatMessage(gps.ToString(), ChatChannel.Faction, playerFaction.FactionId);
          else
            MyHud.Chat.ShowMessageScripted(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_GPSRequireOnline));
        }
        else
        {
          string gpsName = args[0];
          for (int index = 1; index < args.Length; ++index)
            gpsName = gpsName + " " + args[index];
          if (MySession.Static.Gpss.GetGpsByName(MySession.Static.LocalPlayerId, gpsName) != null)
          {
            int num = 1;
            do
            {
              ++num;
            }
            while (MySession.Static.Gpss.GetGpsByName(MySession.Static.LocalPlayerId, gpsName + "_" + num.ToString()) != null);
            gpsName = gpsName + "_" + num.ToString();
          }
          gps.Name = gpsName;
          MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
        }
      }
      else
        MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
    }

    [ChatCommand("/g", "ChatCommand_Help_G", "ChatCommand_HelpSimple_G", MyPromoteLevel.None)]
    private static void CommandChannelGlobal(string[] args)
    {
      MySession.Static.ChatSystem.ChangeChatChannel_Global();
      if (args == null || args.Length == 0)
        return;
      string message = args[0];
      for (int index = 1; index < args.Length; ++index)
        message = message + " " + args[index];
      if (string.IsNullOrEmpty(message))
        return;
      MyGuiScreenChat.SendChatMessage(message);
    }

    [ChatCommand("/f", "ChatCommand_Help_F", "ChatCommand_HelpSimple_F", MyPromoteLevel.None)]
    private static void CommandChannelFaction(string[] args)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(Sync.MyId, 0);
      if (identityId == 0L)
        return;
      if (MySession.Static.Factions.GetPlayerFaction(identityId) == null)
      {
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_FactionChatTarget), Color.Red);
      }
      else
      {
        MySession.Static.ChatSystem.ChangeChatChannel_Faction();
        if (args == null || args.Length == 0)
          return;
        string message = args[0];
        for (int index = 1; index < args.Length; ++index)
          message = message + " " + args[index];
        if (string.IsNullOrEmpty(message))
          return;
        MyGuiScreenChat.SendChatMessage(message);
      }
    }

    [ChatCommand("/w", "ChatCommand_Help_W", "ChatCommand_HelpSimple_W", MyPromoteLevel.None)]
    private static void CommandChannelWhisper(string[] args)
    {
      string empty = string.Empty;
      string message = string.Empty;
      if (args == null || ((IEnumerable<string>) args).Count<string>() < 1)
      {
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_WhisperChatTarget));
      }
      else
      {
        string name;
        if (args[0].Length > 0 && args[0][0] == '"')
        {
          int index1 = 0;
          bool flag = false;
          for (; index1 < args.Length; ++index1)
          {
            if (args[index1][args[index1].Length - 1] == '"')
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            if (index1 == 0)
            {
              name = args[0].Length > 2 ? args[0].Substring(1, args[0].Length - 2) : string.Empty;
              if (index1 < args.Length - 1)
              {
                string str = args[1];
                for (int index2 = 2; index2 < args.Length; ++index2)
                  str = str + " " + args[index2];
                message = str;
              }
            }
            else
            {
              string str1 = args[0];
              for (int index2 = 1; index2 <= index1; ++index2)
                str1 = str1 + " " + args[index2];
              name = str1.Length > 2 ? str1.Substring(1, str1.Length - 2) : string.Empty;
              if (index1 < args.Length - 1)
              {
                string str2 = args[index1 + 1];
                for (int index2 = index1 + 2; index2 < args.Length; ++index2)
                  str2 = str2 + " " + args[index2];
                message = str2;
              }
            }
          }
          else
          {
            name = args[0];
            if (args.Length > 1)
            {
              string str = args[1];
              for (int index2 = 2; index2 < args.Length; ++index2)
                str = str + " " + args[index2];
              message = str;
            }
          }
        }
        else
        {
          name = args[0];
          if (args.Length > 1)
          {
            string str = args[1];
            for (int index = 2; index < args.Length; ++index)
              str = str + " " + args[index];
            if (string.IsNullOrEmpty(str))
              return;
            message = str;
          }
        }
        MyPlayer playerByName = MySession.Static.Players.GetPlayerByName(name);
        if (playerByName == null)
        {
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_WhisperTargetNotFound), (object) name));
        }
        else
        {
          MySession.Static.ChatSystem.ChangeChatChannel_Whisper(playerByName.Identity.IdentityId);
          if (string.IsNullOrEmpty(message))
            return;
          MyGuiScreenChat.SendChatMessage(message);
        }
      }
    }

    [ChatCommand("/timestamp", "ChatCommand_Help_Timestamp", "ChatCommand_HelpSimple_Timestamp", MyPromoteLevel.None)]
    private static void CommandTiemstampToggle(string[] args)
    {
      if (args == null || args.Length == 0)
        return;
      if (args[0].Equals("on") || args[0].Equals("true"))
      {
        MySandboxGame.Config.ShowChatTimestamp = true;
        MySandboxGame.Config.Save();
      }
      else
      {
        if (!args[0].Equals("off") && !args[0].Equals("false"))
          return;
        MySandboxGame.Config.ShowChatTimestamp = false;
        MySandboxGame.Config.Save();
      }
    }

    [ChatCommand("/smite", "ChatCommand_Help_Smite", "ChatCommand_HelpSimple_Smite", MyPromoteLevel.Admin)]
    private static void CommandSmite(string[] args)
    {
      IHitInfo hitInfo;
      MyAPIGateway.Physics.CastRay(MySector.MainCamera.WorldMatrix.Translation, MySector.MainCamera.WorldMatrix.Translation + MySector.MainCamera.WorldMatrix.Forward * 10000.0, out hitInfo);
      if (hitInfo == null)
        return;
      MySession.Static.GetComponent<MySectorWeatherComponent>().RequestLightning(MySector.MainCamera.WorldMatrix.Translation, hitInfo.Position);
    }

    [ChatCommand("/rweather", "ChatCommand_Help_RWeather", "ChatCommand_HelpSimple_RWeather", MyPromoteLevel.Admin)]
    private static void CommandRWeather(string[] args)
    {
      MyObjectBuilder_WeatherEffect weatherEffect;
      if (MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(MySector.MainCamera.Position, out weatherEffect))
        MySession.Static.GetComponent<MySectorWeatherComponent>().RemoveWeather(weatherEffect);
      MySession.Static.GetComponent<MySectorWeatherComponent>().CreateRandomWeather(MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position), true);
    }

    [ChatCommand("/weatherlist", "ChatCommand_Help_Weatherlist", "ChatCommand_HelpSimple_Weatherlist", MyPromoteLevel.Admin)]
    private static void CommandWeatherList(string[] args)
    {
      MyWeatherEffectDefinition[] array = MyDefinitionManager.Static.GetWeatherDefinitions().ToArray<MyWeatherEffectDefinition>();
      string str = "";
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index].Public)
        {
          str += array[index].Id.SubtypeName;
          if (index < array.Length - 1)
            str += ", ";
        }
      }
      MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_AvailableWeathers).ToString() + str, Color.Red);
    }

    [ChatCommand("/weather", "ChatCommand_Help_Weather", "ChatCommand_HelpSimple_Weather", MyPromoteLevel.Admin)]
    private static void CommandWeather(string[] args)
    {
      if (args == null || args.Length == 0)
      {
        string weather = MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(MySector.MainCamera.Position);
        if (weather != null)
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_CurrentWeather).ToString() + weather, Color.Red);
        else
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_NoWeather).ToString(), Color.Red);
      }
      else
      {
        if (args.Length != 1 && args.Length != 2)
          return;
        int result = 0;
        if (args.Length == 2)
          int.TryParse(args[1], out result);
        MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(args[0], (float) result, new Vector3D?(), true, (Vector3D) Vector3.Zero, 0, 1f);
      }
    }
  }
}
