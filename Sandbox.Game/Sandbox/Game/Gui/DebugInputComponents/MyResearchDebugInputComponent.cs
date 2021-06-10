// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.DebugInputComponents.MyResearchDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Input;

namespace Sandbox.Game.GUI.DebugInputComponents
{
  public class MyResearchDebugInputComponent : MyDebugComponent
  {
    public MyResearchDebugInputComponent()
    {
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Show Your Research"), new Func<bool>(this.ShowResearch));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Toggle Pretty Mode"), new Func<bool>(this.ShowResearchPretty));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Unlock Your Research"), new Func<bool>(this.UnlockResearch));
      this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Unlock All Research"), new Func<bool>(this.UnlockAllResearch));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Reset Your Research"), new Func<bool>(this.ResetResearch));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "Reset All Research"), new Func<bool>(this.ResetAllResearch));
    }

    public override string GetName() => "Research";

    private bool ShowResearch()
    {
      MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH = !MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH;
      return true;
    }

    private bool ShowResearchPretty()
    {
      MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH_PRETTY = !MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH_PRETTY;
      return true;
    }

    private bool ResetResearch()
    {
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
        MySessionComponentResearch.Static.ResetResearch(MySession.Static.LocalCharacter);
      return true;
    }

    private bool ResetAllResearch()
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Controller.ControlledEntity is MyCharacter controlledEntity)
          MySessionComponentResearch.Static.ResetResearch(controlledEntity);
      }
      return true;
    }

    private bool UnlockResearch()
    {
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
        MySessionComponentResearch.Static.DebugUnlockAllResearch(MySession.Static.LocalCharacter.GetPlayerIdentityId());
      return true;
    }

    private bool UnlockAllResearch()
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        MySessionComponentResearch.Static.DebugUnlockAllResearch(onlinePlayer.Identity.IdentityId);
      return true;
    }

    public override bool HandleInput() => MySession.Static != null && base.HandleInput();
  }
}
