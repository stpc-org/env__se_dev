// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemAnimation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemAnimation))]
  internal class MyToolbarItemAnimation : MyToolbarItemDefinition
  {
    public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
    {
      base.Init(objBuilder);
      this.ActivateOnClick = true;
      this.WantsToBeActivated = true;
      return true;
    }

    public override bool Activate()
    {
      if (this.Definition == null)
        return false;
      MyAnimationDefinition definition = (MyAnimationDefinition) this.Definition;
      bool flag = MySession.Static.ControlledEntity is MyCockpit;
      MyCharacter myCharacter = flag ? ((MyShipController) MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
      if (myCharacter != null)
      {
        if (myCharacter.UseNewAnimationSystem)
        {
          if (myCharacter.IsOnLadder || flag && !definition.AllowInCockpit)
            return true;
          string subtypeName = definition.Id.SubtypeName;
          myCharacter.TriggerCharacterAnimationEvent("emote", true, definition.InfluenceAreas);
          myCharacter.TriggerCharacterAnimationEvent(subtypeName, true, definition.InfluenceAreas);
        }
        else
          myCharacter.AddCommand(new MyAnimationCommand()
          {
            AnimationSubtypeName = definition.Id.SubtypeName,
            BlendTime = 0.2f,
            PlaybackCommand = MyPlaybackCommand.Play,
            FrameOption = definition.Loop ? MyFrameOption.Loop : MyFrameOption.PlayOnce,
            TimeScale = 1f
          }, true);
      }
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Ship || type == MyToolbarType.Seat;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0) => MyToolbarItem.ChangeInfo.None;
  }
}
