// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugCharacter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("VRage", "Character")]
  internal class MyGuiScreenDebugCharacter : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_animationComboA;
    private MyGuiControlCombobox m_animationComboB;
    private MyGuiControlSlider m_blendSlider;
    private MyGuiControlCombobox m_animationCombo;
    private MyGuiControlCheckbox m_loopCheckbox;

    public MyGuiScreenDebugCharacter()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Render Character", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      if (MySession.Static == null || MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyCharacter))
      {
        this.AddLabel("None active character", Color.Yellow.ToVector4(), 1.2f);
      }
      else
      {
        MyCharacter playerCharacter = MySession.Static.LocalCharacter;
        if (constructor)
        {
          MyAnimationControllerDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>("Debug");
          if (definition == null)
            return;
          playerCharacter.AnimationController.Clear();
          playerCharacter.AnimationController.InitFromDefinition(definition, true);
          if (playerCharacter.AnimationController.ReloadBonesNeeded != null)
            playerCharacter.AnimationController.ReloadBonesNeeded();
        }
        this.AddSlider("Max slope", playerCharacter.Definition.MaxSlope, 0.0f, 89f, (Action<MyGuiControlSlider>) (slider => playerCharacter.Definition.MaxSlope = slider.Value));
        string assetName = playerCharacter.Model.AssetName;
        Color yellow = Color.Yellow;
        Vector4 vector4 = yellow.ToVector4();
        this.AddLabel(assetName, vector4, 1.2f);
        yellow = Color.Yellow;
        this.AddLabel("Animation A:", yellow.ToVector4(), 1.2f);
        this.m_animationComboA = this.AddCombo();
        ListReader<MyAnimationDefinition> animationDefinitions = MyDefinitionManager.Static.GetAnimationDefinitions();
        int num1 = 0;
        foreach (MyAnimationDefinition animationDefinition in animationDefinitions)
          this.m_animationComboA.AddItem((long) num1++, new StringBuilder(animationDefinition.Id.SubtypeName));
        this.m_animationComboA.SelectItemByIndex(0);
        this.AddLabel("Animation B:", Color.Yellow.ToVector4(), 1.2f);
        this.m_animationComboB = this.AddCombo();
        int num2 = 0;
        foreach (MyAnimationDefinition animationDefinition in animationDefinitions)
          this.m_animationComboB.AddItem((long) num2++, new StringBuilder(animationDefinition.Id.SubtypeName));
        this.m_animationComboB.SelectItemByIndex(0);
        this.m_blendSlider = this.AddSlider("Blend time", 0.5f, 0.0f, 3f);
        this.AddButton(new StringBuilder("Play A->B"), new Action<MyGuiControlButton>(this.OnPlayBlendButtonClick));
        this.m_currentPosition.Y += 0.01f;
        this.m_animationCombo = this.AddCombo();
        int num3 = 0;
        foreach (MyAnimationDefinition animationDefinition in animationDefinitions)
          this.m_animationCombo.AddItem((long) num3++, new StringBuilder(animationDefinition.Id.SubtypeName));
        this.m_animationCombo.SortItemsByValueText();
        this.m_animationCombo.SelectItemByIndex(0);
        this.m_loopCheckbox = this.AddCheckBox("Loop", false, (Action<MyGuiControlCheckbox>) null);
        this.m_currentPosition.Y += 0.02f;
        foreach (string key in playerCharacter.Definition.BoneSets.Keys)
        {
          MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(key, false, (Action<MyGuiControlCheckbox>) null);
          guiControlCheckbox.UserData = (object) key;
          if (key == "Body")
            guiControlCheckbox.IsChecked = true;
        }
        this.AddButton(new StringBuilder("Play animation"), new Action<MyGuiControlButton>(this.OnPlayButtonClick));
        this.AddCheckBox("Draw damage and hit hapsules", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE), (Action<bool>) (s => MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE = s));
        this.m_currentPosition.Y += 0.01f;
        this.AddSlider("Gravity mult", MyPerGameSettings.CharacterGravityMultiplier, 0.0f, 5f, (Action<MyGuiControlSlider>) (slider => MyPerGameSettings.CharacterGravityMultiplier = slider.Value));
      }
    }

    private void OnPlayBlendButtonClick(MyGuiControlButton sender)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      localCharacter.PlayCharacterAnimation(this.m_animationComboA.GetSelectedKey().ToString(), MyBlendOption.Immediate, MyFrameOption.PlayOnce, this.m_blendSlider.Value);
      localCharacter.PlayCharacterAnimation(this.m_animationComboB.GetSelectedKey().ToString(), MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, this.m_blendSlider.Value);
    }

    private void OnPlayButtonClick(MyGuiControlButton sender)
    {
      if (MySession.Static.LocalCharacter.UseNewAnimationSystem)
      {
        MySession.Static.LocalCharacter.TriggerCharacterAnimationEvent("play", false);
        MySession.Static.LocalCharacter.TriggerCharacterAnimationEvent(this.m_animationCombo.GetSelectedValue().ToString(), false);
      }
      else
        MySession.Static.LocalCharacter.PlayCharacterAnimation(this.m_animationCombo.GetSelectedValue().ToString(), MyBlendOption.Immediate, this.m_loopCheckbox.IsChecked ? MyFrameOption.Loop : MyFrameOption.PlayOnce, this.m_blendSlider.Value);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugCharacter);
  }
}
