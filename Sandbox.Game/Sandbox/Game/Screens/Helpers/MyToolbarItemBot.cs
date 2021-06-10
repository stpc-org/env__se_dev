// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemBot))]
  public class MyToolbarItemBot : MyToolbarItemDefinition
  {
    public override bool Init(MyObjectBuilder_ToolbarItem data)
    {
      base.Init(data);
      this.ActivateOnClick = false;
      return true;
    }

    public override bool Activate()
    {
      if (!MyFakes.ENABLE_BARBARIANS || !MyPerGameSettings.EnableAi || this.Definition == null)
        return false;
      MyAIComponent.Static.BotToSpawn = this.Definition as MyAgentDefinition;
      MySession.Static.ControlledEntity?.SwitchToWeapon((MyToolbarItemWeapon) null);
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Spectator;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      MyAgentDefinition botToSpawn = MyAIComponent.Static.BotToSpawn;
      this.WantsToBeSelected = botToSpawn != null && botToSpawn.Id.SubtypeId == (this.Definition as MyAgentDefinition).Id.SubtypeId;
      return MyToolbarItem.ChangeInfo.None;
    }
  }
}
