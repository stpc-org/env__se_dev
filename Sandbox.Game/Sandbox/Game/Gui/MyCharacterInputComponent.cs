// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyCharacterInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Game.SessionComponents;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Gui
{
  internal class MyCharacterInputComponent : MyDebugComponent
  {
    private bool m_toggleMovementState;
    private bool m_toggleShowSkeleton;
    private const int m_maxLastAnimationActions = 20;
    private List<string> m_lastAnimationActions = new List<string>(20);
    private Dictionary<MyCharacterBone, int> m_boneRefToIndex;
    private string m_animationControllerName;

    public override string GetName() => "Character";

    public MyCharacterInputComponent()
    {
      this.AddShortcut(MyKeys.U, true, false, false, false, (Func<string>) (() => "Spawn new character"), (Func<bool>) (() =>
      {
        MyCharacterInputComponent.SpawnCharacter();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, false, false, false, false, (Func<string>) (() => "Kill everyone around you"), (Func<bool>) (() =>
      {
        this.KillEveryoneAround();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Use next ship"), (Func<bool>) (() =>
      {
        MyCharacterInputComponent.UseNextShip();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Toggle skeleton view"), (Func<bool>) (() =>
      {
        this.ToggleSkeletonView();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "Reload animation tracks"), (Func<bool>) (() =>
      {
        this.ReloadAnimations();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Toggle character movement status"), (Func<bool>) (() =>
      {
        this.ShowMovementState();
        return true;
      }));
    }

    private void KillEveryoneAround()
    {
      if (MySession.Static.LocalCharacter == null || !Sync.IsServer || (!MySession.Static.HasCreativeRights || !MySession.Static.IsAdminMenuEnabled))
        return;
      Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
      Vector3D vector3D = new Vector3D(25.0, 25.0, 25.0);
      BoundingBoxD box = new BoundingBoxD(position - vector3D, position + vector3D);
      List<MyEntity> result = new List<MyEntity>();
      MyGamePruningStructure.GetAllEntitiesInBox(ref box, result);
      foreach (MyEntity myEntity in result)
      {
        if (myEntity is MyCharacter myCharacter && myEntity != MySession.Static.LocalCharacter)
          myCharacter.DoDamage(1000000f, MyDamageType.Debug, true, 0L);
      }
      MyRenderProxy.DebugDrawAABB(box, Color.Red, 0.5f, shaded: true);
    }

    public override bool HandleInput() => MySession.Static != null && base.HandleInput();

    private void ToggleSkeletonView() => this.m_toggleShowSkeleton = !this.m_toggleShowSkeleton;

    private void ReloadAnimations()
    {
      if (MySession.Static.LocalCharacter != null)
      {
        foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> allAnimationPlayer in MySession.Static.LocalCharacter.GetAllAnimationPlayers())
          MySession.Static.LocalCharacter.PlayerStop(allAnimationPlayer.Key, 0.0f);
      }
      foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
      {
        MyModels.GetModel(animationDefinition.AnimationModel)?.UnloadData();
        MyModels.GetModel(animationDefinition.AnimationModelFPS)?.UnloadData();
      }
      MySessionComponentAnimationSystem.Static.ReloadMwmTracks();
    }

    public static MyCharacter SpawnCharacter(string model = null)
    {
      MyCharacter myCharacter = MySession.Static.LocalHumanPlayer == null ? (MyCharacter) null : MySession.Static.LocalHumanPlayer.Identity.Character;
      Vector3? colorMask = new Vector3?();
      string characterName = MySession.Static.LocalHumanPlayer == null ? "" : MySession.Static.LocalHumanPlayer.Identity.DisplayName;
      string str = MySession.Static.LocalHumanPlayer == null ? MyCharacter.DefaultModel : MySession.Static.LocalHumanPlayer.Identity.Model;
      long identityId = MySession.Static.LocalHumanPlayer == null ? 0L : MySession.Static.LocalHumanPlayer.Identity.IdentityId;
      if (myCharacter != null)
        colorMask = new Vector3?(myCharacter.ColorMask);
      return MyCharacter.CreateCharacter(MatrixD.CreateTranslation(MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 6f + MySector.MainCamera.LeftVector * 3f), Vector3.Zero, characterName, model ?? str, colorMask, (MyBotDefinition) null, false, identityId: identityId);
    }

    public static void UseNextShip()
    {
      MyCockpit cockpit1 = (MyCockpit) null;
      object obj = (object) null;
      foreach (MyCubeGrid myCubeGrid in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCubeGrid>())
      {
        foreach (MyCockpit cockpit2 in myCubeGrid.GetBlocks().Select<MySlimBlock, MyCockpit>((Func<MySlimBlock, MyCockpit>) (s => s.FatBlock as MyCockpit)).Where<MyCockpit>((Func<MyCockpit, bool>) (s => s != null)))
        {
          if (cockpit1 == null && cockpit2.Pilot == null)
            cockpit1 = cockpit2;
          if (obj == MySession.Static.ControlledEntity)
          {
            if (cockpit2.Pilot == null)
            {
              MyCharacterInputComponent.UseCockpit(cockpit2);
              return;
            }
          }
          else
            obj = (object) cockpit2;
        }
      }
      if (cockpit1 == null)
        return;
      MyCharacterInputComponent.UseCockpit(cockpit1);
    }

    private static void UseCockpit(MyCockpit cockpit)
    {
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      if (MySession.Static.ControlledEntity is MyCockpit)
        MySession.Static.ControlledEntity.Use();
      cockpit.RequestUse(UseActionEnum.Manipulate, MySession.Static.LocalHumanPlayer.Identity.Character);
      cockpit.RemoveOriginalPilotPosition();
    }

    private void ShowMovementState() => this.m_toggleMovementState = !this.m_toggleMovementState;

    public override void Draw()
    {
      base.Draw();
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
        MyAnimationInverseKinematics.DebugTransform = MySession.Static.LocalCharacter.WorldMatrix;
      if (this.m_toggleMovementState)
      {
        IEnumerable<MyCharacter> myCharacters = Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCharacter>();
        Vector2 screenCoord = new Vector2(10f, 200f);
        foreach (MyCharacter myCharacter in myCharacters)
        {
          MyRenderProxy.DebugDrawText2D(screenCoord, myCharacter.GetCurrentMovementState().ToString(), Color.Green, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
          screenCoord += new Vector2(0.0f, 20f);
        }
      }
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
        this.Text("Character look speed: {0}", (object) MySession.Static.LocalCharacter.RotationSpeed);
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
      {
        this.Text("Character state: {0}", (object) MySession.Static.LocalCharacter.CurrentMovementState);
        this.Text("Character ground state: {0}", (object) MySession.Static.LocalCharacter.CharacterGroundState);
      }
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
        this.Text("Character head offset: {0} {1}", (object) MySession.Static.LocalCharacter.HeadMovementXOffset, (object) MySession.Static.LocalCharacter.HeadMovementYOffset);
      if (MySession.Static != null && MySession.Static.LocalCharacter != null)
      {
        MyAnimationControllerComponent animationController = MySession.Static.LocalCharacter.AnimationController;
        StringBuilder stringBuilder1 = new StringBuilder(1024);
        MyAnimationController controller = animationController.Controller;
        if (animationController != null && controller != null && controller.GetLayerByIndex(0) != null)
        {
          stringBuilder1.Clear();
          foreach (int num in controller.GetLayerByIndex(0).VisitedTreeNodesPath)
          {
            if (num != 0)
            {
              stringBuilder1.Append(num);
              stringBuilder1.Append(",");
            }
            else
              break;
          }
          this.Text(stringBuilder1.ToString());
        }
        if (animationController != null && animationController.Variables != null)
        {
          foreach (KeyValuePair<MyStringId, float> variable in (IEnumerable<KeyValuePair<MyStringId, float>>) animationController.Variables)
          {
            stringBuilder1.Clear();
            stringBuilder1.Append((object) variable.Key);
            stringBuilder1.Append(" = ");
            stringBuilder1.Append(variable.Value);
            this.Text(stringBuilder1.ToString());
          }
        }
        if (animationController != null)
        {
          if (animationController.LastFrameActions != null)
          {
            foreach (MyStringId lastFrameAction in animationController.LastFrameActions)
              this.m_lastAnimationActions.Add(lastFrameAction.ToString());
            if (this.m_lastAnimationActions.Count > 20)
              this.m_lastAnimationActions.RemoveRange(0, this.m_lastAnimationActions.Count - 20);
          }
          this.Text(Color.Red, "--- RECENTLY TRIGGERED ACTIONS ---");
          foreach (string lastAnimationAction in this.m_lastAnimationActions)
            this.Text(Color.Yellow, lastAnimationAction);
        }
        if (animationController != null && controller != null)
        {
          lock (controller)
          {
            int layerCount = controller.GetLayerCount();
            for (int index = 0; index < layerCount; ++index)
            {
              MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(index);
              if (layerByIndex != null && layerByIndex.CurrentNode != null)
              {
                StringBuilder stringBuilder2 = new StringBuilder();
                foreach (MyAnimationStateMachine.MyStateTransitionBlending transitionBlending in layerByIndex.StateTransitionBlending)
                  stringBuilder2.AppendFormat(" + {0}(+{1:0.0})", (object) transitionBlending.SourceState.Name, (object) transitionBlending.TimeLeftInSeconds);
                string text = string.Format("{0} ... {1}{2}", (object) layerByIndex.Name, (object) layerByIndex.CurrentNode.Name, (object) stringBuilder2);
                MyRenderProxy.DebugDrawText2D(new Vector2(250f, (float) (150 + index * 10)), text, Color.Lime, 0.5f);
              }
            }
          }
        }
      }
      if (this.m_toggleShowSkeleton)
        this.DrawSkeleton();
      MyRenderProxy.DebugDrawText2D(new Vector2(300f, 10f), "Debugging AC " + this.m_animationControllerName, Color.Yellow, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      if (MySession.Static == null || MySession.Static.LocalCharacter == null || (MySession.Static.LocalCharacter.Definition == null || MySession.Static.LocalCharacter.Definition.AnimationController != null))
        return;
      DictionaryReader<string, MyAnimationPlayerBlendPair> animationPlayers = MySession.Static.LocalCharacter.GetAllAnimationPlayers();
      float y = 40f;
      foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> keyValuePair in animationPlayers)
      {
        MyRenderProxy.DebugDrawText2D(new Vector2(400f, y), (keyValuePair.Key != "" ? keyValuePair.Key : "Body") + ": " + keyValuePair.Value.ActualPlayer.AnimationNameDebug + " (" + keyValuePair.Value.ActualPlayer.AnimationMwmPathDebug + ")", Color.Lime, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        y += 30f;
      }
    }

    private void DrawSkeleton()
    {
      if (this.m_boneRefToIndex == null)
        this.m_boneRefToIndex = new Dictionary<MyCharacterBone, int>(256);
      if (MySessionComponentAnimationSystem.Static == null)
        return;
      foreach (MyAnimationControllerComponent animationComponent in MySessionComponentAnimationSystem.Static.RegisteredAnimationComponents)
      {
        MyCharacter character = animationComponent != null ? animationComponent.Entity as MyCharacter : (MyCharacter) null;
        if (character == null)
          break;
        List<MyAnimationClip.BoneState> lastRawBoneResult = character.AnimationController.LastRawBoneResult;
        MyCharacterBone[] characterBones = character.AnimationController.CharacterBones;
        this.m_boneRefToIndex.Clear();
        for (int index = 0; index < characterBones.Length; ++index)
          this.m_boneRefToIndex.Add(character.AnimationController.CharacterBones[index], index);
        for (int boneIndex = 0; boneIndex < characterBones.Length; ++boneIndex)
        {
          if (characterBones[boneIndex].Parent == null)
          {
            MatrixD parentTransform = character.PositionComp.WorldMatrixRef;
            this.DrawBoneHierarchy(character, ref parentTransform, characterBones, lastRawBoneResult, boneIndex);
          }
        }
      }
    }

    private void DrawBoneHierarchy(
      MyCharacter character,
      ref MatrixD parentTransform,
      MyCharacterBone[] characterBones,
      List<MyAnimationClip.BoneState> rawBones,
      int boneIndex)
    {
      MatrixD matrixD = rawBones != null ? Matrix.CreateTranslation(rawBones[boneIndex].Translation) * parentTransform : MatrixD.Identity;
      MatrixD parentTransform1 = rawBones != null ? Matrix.CreateFromQuaternion(rawBones[boneIndex].Rotation) * matrixD : matrixD;
      if (rawBones != null)
        MyRenderProxy.DebugDrawLine3D(parentTransform1.Translation, parentTransform.Translation, Color.Green, Color.Green, false);
      bool flag = false;
      for (int childIndex = 0; characterBones[boneIndex].GetChildBone(childIndex) != null; ++childIndex)
      {
        MyCharacterBone childBone = characterBones[boneIndex].GetChildBone(childIndex);
        this.DrawBoneHierarchy(character, ref parentTransform1, characterBones, rawBones, this.m_boneRefToIndex[childBone]);
        flag = true;
      }
      if (!flag && rawBones != null)
        MyRenderProxy.DebugDrawLine3D(parentTransform1.Translation, parentTransform1.Translation + parentTransform1.Left * 0.0500000007450581, Color.Green, Color.Cyan, false);
      MyRenderProxy.DebugDrawText3D(Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation, character.PositionComp.WorldMatrixRef), characterBones[boneIndex].Name, Color.Lime, 0.4f, false);
      if (characterBones[boneIndex].Parent != null)
        MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation, character.PositionComp.WorldMatrixRef), Vector3D.Transform(characterBones[boneIndex].Parent.AbsoluteTransform.Translation, character.PositionComp.WorldMatrixRef), Color.Purple, Color.Purple, false);
      if (flag)
        return;
      Vector3D pointFrom = Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation, character.PositionComp.WorldMatrixRef);
      Matrix absoluteTransform = characterBones[boneIndex].AbsoluteTransform;
      Vector3 translation = absoluteTransform.Translation;
      absoluteTransform = characterBones[boneIndex].AbsoluteTransform;
      Vector3 vector3 = absoluteTransform.Left * 0.05f;
      Vector3D pointTo = Vector3D.Transform(translation + vector3, character.PositionComp.WorldMatrixRef);
      Color purple = Color.Purple;
      Color red = Color.Red;
      MyRenderProxy.DebugDrawLine3D(pointFrom, pointTo, purple, red, false);
    }
  }
}
