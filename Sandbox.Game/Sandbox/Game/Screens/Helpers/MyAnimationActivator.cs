// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyAnimationActivator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.Definitions.Animation;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyAnimationActivator
  {
    public static void Activate(MyEmoteDefinition emoteDefinition)
    {
      if (emoteDefinition == null)
        return;
      MyCharacter controlledObject = MySession.Static.ControlledEntity is MyCockpit ? ((MyShipController) MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
      if (controlledObject == null)
        return;
      MyAnimationActivator.Activate(controlledObject.Definition != null ? emoteDefinition.GetAnimationForCharacter(controlledObject.Definition.Id) : MyDefinitionManager.Static.TryGetAnimationDefinition(emoteDefinition.AnimationId.SubtypeName), controlledObject);
    }

    public static void Activate(MyAnimationDefinition animationDefinition)
    {
      if (animationDefinition == null)
        return;
      MyCharacter controlledObject = MySession.Static.ControlledEntity is MyCockpit ? ((MyShipController) MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
      MyAnimationActivator.Activate(animationDefinition, controlledObject);
    }

    public static void Activate(
      MyAnimationDefinition animationDefinition,
      MyCharacter controlledObject)
    {
      if (animationDefinition == null || controlledObject == null || controlledObject.IsOnLadder)
        return;
      if (controlledObject.UseNewAnimationSystem)
      {
        controlledObject.TriggerCharacterAnimationEvent("emote", true, animationDefinition.InfluenceAreas);
        controlledObject.TriggerCharacterAnimationEvent(animationDefinition.Id.SubtypeName, true, animationDefinition.InfluenceAreas);
      }
      else
        controlledObject.AddCommand(new MyAnimationCommand()
        {
          AnimationSubtypeName = animationDefinition.Id.SubtypeName,
          BlendTime = 0.2f,
          PlaybackCommand = MyPlaybackCommand.Play,
          FrameOption = animationDefinition.Loop ? MyFrameOption.Loop : MyFrameOption.PlayOnce,
          TimeScale = 1f
        }, true);
    }
  }
}
