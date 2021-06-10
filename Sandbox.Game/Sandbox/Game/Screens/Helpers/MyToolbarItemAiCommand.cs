﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemAiCommand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.AI;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemAiCommand))]
  public class MyToolbarItemAiCommand : MyToolbarItemDefinition
  {
    public override bool Init(MyObjectBuilder_ToolbarItem data)
    {
      int num = base.Init(data) ? 1 : 0;
      this.ActivateOnClick = false;
      return num != 0;
    }

    public override bool Activate()
    {
      if (this.Definition == null)
        return false;
      MyAIComponent.Static.CommandDefinition = this.Definition as MyAiCommandDefinition;
      MySession.Static.ControlledEntity?.SwitchToWeapon((MyToolbarItemWeapon) null);
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Spectator;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      MyAiCommandDefinition commandDefinition = MyAIComponent.Static.CommandDefinition;
      this.WantsToBeSelected = commandDefinition != null && commandDefinition.Id.SubtypeId == (this.Definition as MyAiCommandDefinition).Id.SubtypeId;
      return MyToolbarItem.ChangeInfo.None;
    }
  }
}
