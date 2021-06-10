// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugPlayerShake
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Player Shake")]
  internal class MyGuiScreenDebugPlayerShake : MyGuiScreenDebugBase
  {
    private float m_forceShake = 5f;

    public MyGuiScreenDebugPlayerShake()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption("Player Head Shake", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_scale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      if (MySector.MainCamera != null)
      {
        MyCameraShake cameraShake = MySector.MainCamera.CameraShake;
        this.AddLabel("Camera shake", Color.Yellow.ToVector4(), 1f);
        this.AddSlider("MaxShake", 0.0f, 50f, (Func<float>) (() => cameraShake.MaxShake), (Action<float>) (s => cameraShake.MaxShake = s));
        this.AddSlider("MaxShakePosX", 0.0f, 3f, (Func<float>) (() => cameraShake.MaxShakePosX), (Action<float>) (s => cameraShake.MaxShakePosX = s));
        this.AddSlider("MaxShakePosY", 0.0f, 3f, (Func<float>) (() => cameraShake.MaxShakePosY), (Action<float>) (s => cameraShake.MaxShakePosY = s));
        this.AddSlider("MaxShakePosZ", 0.0f, 3f, (Func<float>) (() => cameraShake.MaxShakePosZ), (Action<float>) (s => cameraShake.MaxShakePosZ = s));
        this.AddSlider("MaxShakeDir", 0.0f, 1f, (Func<float>) (() => cameraShake.MaxShakeDir), (Action<float>) (s => cameraShake.MaxShakeDir = s));
        this.AddSlider("Reduction", 0.0f, 1f, (Func<float>) (() => cameraShake.Reduction), (Action<float>) (s => cameraShake.Reduction = s));
        this.AddSlider("Dampening", 0.0f, 1f, (Func<float>) (() => cameraShake.Dampening), (Action<float>) (s => cameraShake.Dampening = s));
        this.AddSlider("OffConstant", 0.0f, 1f, (Func<float>) (() => cameraShake.OffConstant), (Action<float>) (s => cameraShake.OffConstant = s));
        this.AddSlider("DirReduction", 0.0f, 2f, (Func<float>) (() => cameraShake.DirReduction), (Action<float>) (s => cameraShake.DirReduction = s));
        this.m_currentPosition.Y += 0.01f;
        Color yellow = Color.Yellow;
        this.AddLabel("Maximum shakes", yellow.ToVector4(), 1f);
        this.AddSlider("Character damage", 0.0f, 5000f, (Func<float>) (() => MyCharacter.MAX_SHAKE_DAMAGE), (Action<float>) (s => MyCharacter.MAX_SHAKE_DAMAGE = s));
        this.AddSlider("Grid damage", 0.0f, 5000f, (Func<float>) (() => MyCockpit.MAX_SHAKE_DAMAGE), (Action<float>) (s => MyCockpit.MAX_SHAKE_DAMAGE = s));
        this.AddSlider("Explosion shake time", 0.0f, 5000f, (Func<float>) (() => MyExplosionsConstants.CAMERA_SHAKE_TIME_MS), (Action<float>) (s => MyExplosionsConstants.CAMERA_SHAKE_TIME_MS = s));
        this.AddSlider("Grinder max shake", 0.0f, 50f, (Func<float>) (() => MyAngleGrinder.GRINDER_MAX_SHAKE), (Action<float>) (s => MyAngleGrinder.GRINDER_MAX_SHAKE = s));
        this.AddSlider("Rifle max shake", 0.0f, 50f, (Func<float>) (() => MyAutomaticRifleGun.RIFLE_MAX_SHAKE), (Action<float>) (s => MyAutomaticRifleGun.RIFLE_MAX_SHAKE = s));
        this.AddSlider("Rifle FOV shake", 0.0f, 1f, (Func<float>) (() => MyAutomaticRifleGun.RIFLE_FOV_SHAKE), (Action<float>) (s => MyAutomaticRifleGun.RIFLE_FOV_SHAKE = s));
        this.AddSlider("Drill max shake", 0.0f, 50f, (Func<float>) (() => MyDrillBase.DRILL_MAX_SHAKE), (Action<float>) (s => MyDrillBase.DRILL_MAX_SHAKE = s));
        this.m_currentPosition.Y += 0.01f;
        yellow = Color.Yellow;
        this.AddLabel("Testing", yellow.ToVector4(), 1f);
        this.AddSlider("Shake", 0.0f, 50f, (Func<float>) (() => this.m_forceShake), (Action<float>) (s => this.m_forceShake = s));
        this.AddButton("Force shake", new Action<MyGuiControlButton>(this.OnForceShakeClick));
      }
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugPlayerShake);

    private void OnForceShakeClick(MyGuiControlButton button)
    {
      if (MySector.MainCamera == null)
        return;
      MySector.MainCamera.CameraShake.AddShake(this.m_forceShake);
    }
  }
}
