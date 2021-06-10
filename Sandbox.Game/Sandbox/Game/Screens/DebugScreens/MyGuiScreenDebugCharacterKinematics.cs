// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugCharacterKinematics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Character kinematics")]
  internal class MyGuiScreenDebugCharacterKinematics : MyGuiScreenDebugBase
  {
    public bool updating;

    public MyRagdollMapper PlayerRagdollMapper => MySession.Static.LocalCharacter.Components.Get<MyCharacterRagdollComponent>()?.RagdollMapper;

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugCharacterKinematics);

    public MyGuiScreenDebugCharacterKinematics()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Character kinematics debug draw", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCheckBox("Enable permanent IK/Ragdoll simulation ", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_PERMANENT_SIMULATIONS_COMPUTATION)));
      this.AddCheckBox("Draw Ragdoll Rig Pose", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_ORIGINAL_RIG)));
      this.AddCheckBox("Draw Bones Rig Pose", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_ORIGINAL_RIG)));
      this.AddCheckBox("Draw Ragdoll Pose", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_POSE)));
      this.AddCheckBox("Draw Bones", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_COMPUTED_BONES)));
      this.AddCheckBox("Draw bones intended transforms", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_DESIRED)));
      this.AddCheckBox("Draw Hip Ragdoll and Char. Position", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_HIPPOSITIONS)));
      this.AddCheckBox("Enable Ragdoll", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyPerGameSettings.EnableRagdollModels)));
      this.AddCheckBox("Enable Bones Translation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_RAGDOLL_BONES_TRANSLATION)));
      this.AddSlider("Animation weighting", 0.0f, 5f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyFakes.RAGDOLL_ANIMATION_WEIGHTING)));
      this.AddSlider("Ragdoll gravity multiplier", 0.0f, 50f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyFakes.RAGDOLL_GRAVITY_MULTIPLIER)));
      this.AddButton(new StringBuilder("Kill Ragdoll"), new Action<MyGuiControlButton>(this.killRagdollAction));
      this.AddButton(new StringBuilder("Activate Ragdoll"), new Action<MyGuiControlButton>(this.activateRagdollAction));
      this.AddButton(new StringBuilder("Switch to Dynamic / Keyframed"), new Action<MyGuiControlButton>(this.switchRagdoll));
    }

    private void switchRagdoll(MyGuiControlButton obj)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (!this.PlayerRagdollMapper.IsActive)
        return;
      if (localCharacter.Physics.Ragdoll.IsKeyframed)
      {
        localCharacter.Physics.Ragdoll.EnableConstraints();
        this.PlayerRagdollMapper.SetRagdollToDynamic();
      }
      else
      {
        localCharacter.Physics.Ragdoll.DisableConstraints();
        this.PlayerRagdollMapper.SetRagdollToKeyframed();
      }
    }

    private void activateRagdollAction(MyGuiControlButton obj)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (this.PlayerRagdollMapper == null)
      {
        MyCharacterRagdollComponent component = new MyCharacterRagdollComponent();
        localCharacter.Components.Add<MyCharacterRagdollComponent>(component);
        component.InitRagdoll();
      }
      if (this.PlayerRagdollMapper.IsActive)
        this.PlayerRagdollMapper.Deactivate();
      localCharacter.Physics.SwitchToRagdollMode(false);
      this.PlayerRagdollMapper.Activate();
      this.PlayerRagdollMapper.SetRagdollToKeyframed();
      localCharacter.Physics.Ragdoll.DisableConstraints();
    }

    private void killRagdollAction(MyGuiControlButton obj)
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      MyFakes.CHARACTER_CAN_DIE_EVEN_IN_CREATIVE_MODE = true;
      MyStringHash suicide = MyDamageType.Suicide;
      localCharacter.DoDamage(1000000f, suicide, true, 0L);
    }
  }
}
