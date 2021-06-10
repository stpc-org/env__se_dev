// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_SmileAndWave
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_SmileAndWave : MySteamAchievementBase
  {
    private const string WAVE_ANIMATION_NAME = "RightHand/Emote";
    private readonly string ANIMATION_CLIP_NAME = "AnimStack::Astronaut_character_wave_final";
    private MyStringId m_waveAnimationId;
    private MyCharacter m_localCharacter;

    private MyCharacter LocalCharacter
    {
      get => this.m_localCharacter;
      set
      {
        if (this.m_localCharacter == value)
          return;
        if (this.m_localCharacter != null)
        {
          this.m_localCharacter.CharacterDied -= new Action<MyCharacter>(this.OnTrackedCharacterDied);
          this.m_localCharacter.OnMarkForClose -= new Action<MyEntity>(this.OnTrackedCharacterClosed);
          this.m_localCharacter.AnimationController.ActionTriggered -= new Action<MyStringId>(this.AnimationControllerOnActionTriggered);
        }
        this.m_localCharacter = value;
        if (this.m_localCharacter == null)
          return;
        this.m_localCharacter.CharacterDied += new Action<MyCharacter>(this.OnTrackedCharacterDied);
        this.m_localCharacter.OnMarkForClose += new Action<MyEntity>(this.OnTrackedCharacterClosed);
        this.m_localCharacter.AnimationController.ActionTriggered += new Action<MyStringId>(this.AnimationControllerOnActionTriggered);
      }
    }

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_SmileAndWave), (string) null, 0.0f);

    public override bool NeedsUpdate => this.LocalCharacter == null;

    public override void SessionBeforeStart()
    {
      if (this.IsAchieved || MySession.Static.CreativeMode)
        return;
      this.m_waveAnimationId = MyStringId.GetOrCompute("Wave");
    }

    public override void SessionUpdate()
    {
      if (this.IsAchieved)
        return;
      this.LocalCharacter = MySession.Static.LocalCharacter;
    }

    private void OnTrackedCharacterClosed(MyEntity entity) => this.OnTrackedCharacterDied(entity as MyCharacter);

    private void OnTrackedCharacterDied(MyCharacter character) => this.LocalCharacter = (MyCharacter) null;

    private void AnimationControllerOnActionTriggered(MyStringId animationAction)
    {
      if (animationAction != this.m_waveAnimationId)
        return;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      Vector3D position1 = localCharacter.PositionComp.GetPosition();
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId);
      long factionId1 = playerFaction1 == null ? 0L : playerFaction1.FactionId;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Character != null && onlinePlayer.Character != localCharacter)
        {
          Vector3D position2 = onlinePlayer.Character.PositionComp.GetPosition();
          double result;
          Vector3D.DistanceSquared(ref position2, ref position1, out result);
          if (result < 25.0)
          {
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(onlinePlayer.Identity.IdentityId);
            long factionId2 = playerFaction2 == null ? 0L : playerFaction2.FactionId;
            if (MySession.Static.Factions.AreFactionsEnemies(factionId1, factionId2) && this.IsPlayerWaving(onlinePlayer.Character) && this.PlayersLookingFaceToFace(localCharacter, onlinePlayer.Character))
            {
              this.NotifyAchieved();
              localCharacter.AnimationController.ActionTriggered -= new Action<MyStringId>(this.AnimationControllerOnActionTriggered);
              break;
            }
          }
        }
      }
    }

    private bool PlayersLookingFaceToFace(MyCharacter firstCharacter, MyCharacter secondCharacter)
    {
      Vector3D forward1 = firstCharacter.GetHeadMatrix(false, true, false, false, false).Forward;
      Vector3D forward2 = secondCharacter.GetHeadMatrix(false, true, false, false, false).Forward;
      double result;
      Vector3D.Dot(ref forward1, ref forward2, out result);
      return result < -0.5;
    }

    private bool IsPlayerWaving(MyCharacter character)
    {
      MyAnimationController controller = character.AnimationController.Controller;
      for (int index = 0; index < controller.GetLayerCount(); ++index)
      {
        MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(index);
        if (layerByIndex.CurrentNode != null && layerByIndex.CurrentNode.Name != null && (layerByIndex.CurrentNode.Name == "RightHand/Emote" && layerByIndex.CurrentNode is MyAnimationStateMachineNode currentNode) && (currentNode.RootAnimationNode is MyAnimationTreeNodeTrack rootAnimationNode && rootAnimationNode.AnimationClip.Name == this.ANIMATION_CLIP_NAME))
          return true;
      }
      return false;
    }
  }
}
