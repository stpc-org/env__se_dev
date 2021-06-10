// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugCharacterProperties
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Character properties")]
  internal class MyGuiScreenDebugCharacterProperties : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugCharacterProperties()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("System character properties", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      if (MySession.Static.LocalCharacter == null)
        return;
      this.AddLabel("Front light", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Reflector Distance CONST", 1f, 500f, (Func<float>) (() => 35f), (Action<float>) (s => {}));
      this.AddSlider("Reflector Intensity CONST", 0.0f, 2f, (Func<float>) (() => MyCharacter.REFLECTOR_INTENSITY), (Action<float>) (s => {}));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Movement", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Acceleration", 0.0f, 100f, (Func<float>) (() => MyPerGameSettings.CharacterMovement.WalkAcceleration), (Action<float>) (s => MyPerGameSettings.CharacterMovement.WalkAcceleration = s));
      this.AddSlider("Decceleration", 0.0f, 100f, (Func<float>) (() => MyPerGameSettings.CharacterMovement.WalkDecceleration), (Action<float>) (s => MyPerGameSettings.CharacterMovement.WalkDecceleration = s));
      this.AddSlider("Sprint acceleration", 0.0f, 100f, (Func<float>) (() => MyPerGameSettings.CharacterMovement.SprintAcceleration), (Action<float>) (s => MyPerGameSettings.CharacterMovement.SprintAcceleration = s));
      this.AddSlider("Sprint decceleration", 0.0f, 100f, (Func<float>) (() => MyPerGameSettings.CharacterMovement.SprintDecceleration), (Action<float>) (s => MyPerGameSettings.CharacterMovement.SprintDecceleration = s));
      this.AddSlider("Movement Speed Multiplier", 0.1f, 10f, (Func<float>) (() => MySession.Static.Settings.CharacterSpeedMultiplier), (Action<float>) (s => MySession.Static.Settings.CharacterSpeedMultiplier = s));
      this.AddSlider("Animation mutilplier", 0.1f, 10f, (Func<float>) (() => MyFakes.CHARACTER_ANIMATION_SPEED), (Action<float>) (s => MyFakes.CHARACTER_ANIMATION_SPEED = s));
      this.AddSlider("Foot height", 0.01f, 0.6f, (Func<float>) (() => MyFakes.CHARACTER_ANKLE_HEIGHT), (Action<float>) (s => MyFakes.CHARACTER_ANKLE_HEIGHT = s));
      this.AddCheckBox("Record character foot animation", (Func<bool>) (() => MyFakes.RECORD_CHARACTER_FOOT_ANIMATION), (Action<bool>) (s => MyFakes.RECORD_CHARACTER_FOOT_ANIMATION = s));
      this.AddCheckBox("Debug draw", (Func<bool>) (() => MyFakes.CHARACTER_FOOTS_DEBUG_DRAW), (Action<bool>) (s => MyFakes.CHARACTER_FOOTS_DEBUG_DRAW = s));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugCharacterProperties);
  }
}
