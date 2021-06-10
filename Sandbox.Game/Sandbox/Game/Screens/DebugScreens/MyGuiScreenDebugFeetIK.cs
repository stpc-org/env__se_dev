// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugFeetIK
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
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Character feet IK")]
  internal class MyGuiScreenDebugFeetIK : MyGuiScreenDebugBase
  {
    private MyGuiControlSlider belowReachableDistance;
    private MyGuiControlSlider aboveReachableDistance;
    private MyGuiControlSlider verticalChangeUpGain;
    private MyGuiControlSlider verticalChangeDownGain;
    private MyGuiControlSlider ankleHeight;
    private MyGuiControlSlider footWidth;
    private MyGuiControlSlider footLength;
    private MyGuiControlCombobox characterMovementStateCombo;
    private MyGuiControlCheckbox enabledIKState;
    public static bool ikSettingsEnabled;
    private MyFeetIKSettings ikSettings;
    public bool updating;

    public MyRagdollMapper PlayerRagdollMapper => MySession.Static.LocalCharacter.Components.Get<MyCharacterRagdollComponent>()?.RagdollMapper;

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugFeetIK);

    public MyGuiScreenDebugFeetIK()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Character feet IK debug draw", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCheckBox("Draw IK Settings ", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_SETTINGS)));
      this.AddCheckBox("Draw ankle final position", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_ANKLE_FINALPOS)));
      this.AddCheckBox("Draw raycast lines and foot lines", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTLINE)));
      this.AddCheckBox("Draw bones", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_BONES)));
      this.AddCheckBox("Draw raycast hits", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTHITS)));
      this.AddCheckBox("Draw ankle desired positions", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_ANKLE_DESIREDPOSITION)));
      this.AddCheckBox("Draw closest support position", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_CLOSESTSUPPORTPOSITION)));
      this.AddCheckBox("Draw IK solvers debug", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)));
      this.AddCheckBox("Enable/Disable Feet IK", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_FOOT_IK)));
      this.enabledIKState = this.AddCheckBox("Enable IK for this state", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyGuiScreenDebugFeetIK.ikSettingsEnabled)));
      this.belowReachableDistance = this.AddSlider("Reachable distance below character", 0.0f, 0.0f, 2f);
      this.aboveReachableDistance = this.AddSlider("Reachable distance above character", 0.0f, 0.0f, 2f);
      this.verticalChangeUpGain = this.AddSlider("Shift Up Gain", 0.1f, 0.0f, 1f);
      this.verticalChangeDownGain = this.AddSlider("Sift Down Gain", 0.1f, 0.0f, 1f);
      this.ankleHeight = this.AddSlider("Ankle height", 0.1f, 1f / 1000f, 0.3f);
      this.footWidth = this.AddSlider("Foot width", 0.1f, 1f / 1000f, 0.3f);
      this.footLength = this.AddSlider("Foot length", 0.3f, 1f / 1000f, 0.2f);
      this.RegisterEvents();
    }

    private void ItemChanged(MyGuiControlSlider slider)
    {
      if (this.updating)
        return;
      this.ikSettings.Enabled = this.enabledIKState.IsChecked;
      this.ikSettings.BelowReachableDistance = this.belowReachableDistance.Value;
      this.ikSettings.AboveReachableDistance = this.aboveReachableDistance.Value;
      this.ikSettings.VerticalShiftUpGain = this.verticalChangeUpGain.Value;
      this.ikSettings.VerticalShiftDownGain = this.verticalChangeDownGain.Value;
      this.ikSettings.FootSize.Y = this.ankleHeight.Value;
      this.ikSettings.FootSize.X = this.footWidth.Value;
      this.ikSettings.FootSize.Z = this.footLength.Value;
      MySession.Static.LocalCharacter.Definition.FeetIKSettings[MyCharacterMovementEnum.Standing] = this.ikSettings;
    }

    private void RegisterEvents()
    {
      this.belowReachableDistance.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.aboveReachableDistance.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.verticalChangeUpGain.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.verticalChangeDownGain.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.ankleHeight.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.footWidth.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.footLength.ValueChanged += new Action<MyGuiControlSlider>(this.ItemChanged);
      this.enabledIKState.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.IsCheckedChanged);
    }

    private void IsCheckedChanged(MyGuiControlCheckbox obj) => this.ItemChanged((MyGuiControlSlider) null);

    private void characterMovementStateCombo_ItemSelected()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      MyCharacterMovementEnum selectedKey = (MyCharacterMovementEnum) this.characterMovementStateCombo.GetSelectedKey();
      MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_MOVEMENT_STATE = selectedKey;
      if (!localCharacter.Definition.FeetIKSettings.TryGetValue(selectedKey, out this.ikSettings))
      {
        this.ikSettings = new MyFeetIKSettings();
        this.ikSettings.Enabled = false;
        this.ikSettings.AboveReachableDistance = 0.1f;
        this.ikSettings.BelowReachableDistance = 0.1f;
        this.ikSettings.VerticalShiftDownGain = 0.1f;
        this.ikSettings.VerticalShiftUpGain = 0.1f;
        this.ikSettings.FootSize = new Vector3(0.1f, 0.1f, 0.2f);
      }
      this.updating = true;
      this.enabledIKState.IsChecked = this.ikSettings.Enabled;
      this.belowReachableDistance.Value = this.ikSettings.BelowReachableDistance;
      this.aboveReachableDistance.Value = this.ikSettings.AboveReachableDistance;
      this.verticalChangeUpGain.Value = this.ikSettings.VerticalShiftUpGain;
      this.verticalChangeDownGain.Value = this.ikSettings.VerticalShiftDownGain;
      this.ankleHeight.Value = this.ikSettings.FootSize.Y;
      this.footWidth.Value = this.ikSettings.FootSize.X;
      this.footLength.Value = this.ikSettings.FootSize.Z;
      this.updating = false;
    }

    private void UnRegisterEvents()
    {
      this.characterMovementStateCombo.ItemSelected -= new MyGuiControlCombobox.ItemSelectedDelegate(this.characterMovementStateCombo_ItemSelected);
      this.belowReachableDistance.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.aboveReachableDistance.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.verticalChangeUpGain.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.verticalChangeDownGain.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.ankleHeight.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.footWidth.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.footLength.ValueChanged -= new Action<MyGuiControlSlider>(this.ItemChanged);
      this.enabledIKState.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.IsCheckedChanged);
    }
  }
}
