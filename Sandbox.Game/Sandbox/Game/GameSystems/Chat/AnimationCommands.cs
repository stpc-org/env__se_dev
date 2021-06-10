// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Chat.AnimationCommands
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.GameServices;

namespace Sandbox.Game.GameSystems.Chat
{
  public static class AnimationCommands
  {
    internal static void LoadAnimations(MyChatCommandSystem system) => system.OnUnhandledCommand += new MyChatCommandSystem.HandleCommandDelegate(AnimationCommands.GetAnimationCommands);

    internal static void GetAnimationCommands(
      string command,
      string body,
      List<IMyChatCommand> executableCommands)
    {
      foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
      {
        MyAnimationDefinition def = animationDefinition;
        if (!string.IsNullOrWhiteSpace(def.ChatCommand) && def.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
        {
          MyChatCommand myChatCommand = new MyChatCommand(def.ChatCommand, def.ChatCommandName, def.ChatCommandDescription, (Action<string[]>) (x => MyAnimationActivator.Activate(def)));
          executableCommands.Add((IMyChatCommand) myChatCommand);
          return;
        }
      }
      if (Sync.IsDedicated || MyGameService.InventoryItems == null)
        return;
      foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
      {
        if (inventoryItem != null && inventoryItem.ItemDefinition != null && inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote)
        {
          MyEmoteDefinition def = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(inventoryItem.ItemDefinition.AssetModifierId);
          if (def != null && MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) def, MySession.Static.LocalHumanPlayer.Id.SteamId) && (!string.IsNullOrWhiteSpace(def.ChatCommand) && def.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase)))
          {
            MyChatCommand myChatCommand = new MyChatCommand(def.ChatCommand, def.ChatCommandName, def.ChatCommandDescription, (Action<string[]>) (x => MyAnimationActivator.Activate(def)));
            executableCommands.Add((IMyChatCommand) myChatCommand);
            break;
          }
        }
      }
    }
  }
}
