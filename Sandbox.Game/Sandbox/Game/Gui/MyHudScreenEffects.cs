// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudScreenEffects
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyHudScreenEffects
  {
    private float m_blackScreenCurrent = 1f;
    private float m_blackScreenStart;
    private float m_blackScreenTimeIncrement;
    private float m_blackScreenTimeTimer;
    private float m_blackScreenTarget = 1f;
    private bool m_blackScreenDataSaved;
    private Color m_blackScreenDataSavedLightColor = Color.Black;
    private Color m_blackScreenDataSavedDarkColor = Color.Black;
    private float m_blackScreenDataSavedStrength;
    public bool BlackScreenMinimalizeHUD = true;
    public Color BlackScreenColor = Color.Black;
    public Action OnBlackscreenFadeFinishedCallback;

    public float BlackScreenCurrent => this.m_blackScreenCurrent;

    public void Update() => this.UpdateBlackScreen();

    public void FadeScreen(float targetAlpha, float time = 0.0f)
    {
      targetAlpha = MathHelper.Clamp(targetAlpha, 0.0f, 1f);
      if ((double) time <= 0.0)
      {
        this.m_blackScreenTarget = targetAlpha;
        this.m_blackScreenCurrent = targetAlpha;
      }
      else
      {
        this.m_blackScreenTarget = targetAlpha;
        this.m_blackScreenStart = this.m_blackScreenCurrent;
        this.m_blackScreenTimeTimer = 0.0f;
        this.m_blackScreenTimeIncrement = 0.01666667f / time;
      }
      if ((double) targetAlpha >= 1.0 || this.m_blackScreenDataSaved)
        return;
      this.m_blackScreenDataSaved = true;
      this.m_blackScreenDataSavedLightColor = (Color) MyPostprocessSettingsWrapper.Settings.Data.LightColor;
      this.m_blackScreenDataSavedDarkColor = (Color) MyPostprocessSettingsWrapper.Settings.Data.DarkColor;
      this.m_blackScreenDataSavedStrength = MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength;
    }

    public void SwitchFadeScreen(float time = 0.0f) => this.FadeScreen(1f - this.m_blackScreenTarget, time);

    public bool IsBlackscreenFadeInProgress() => (double) this.m_blackScreenTimeTimer != 1.0;

    private void UpdateBlackScreen()
    {
      if ((double) this.m_blackScreenTimeTimer < 1.0 && (double) this.m_blackScreenCurrent != (double) this.m_blackScreenTarget)
      {
        this.m_blackScreenTimeTimer += this.m_blackScreenTimeIncrement;
        if ((double) this.m_blackScreenTimeTimer > 1.0)
          this.m_blackScreenTimeTimer = 1f;
        this.m_blackScreenCurrent = MathHelper.Lerp(this.m_blackScreenStart, this.m_blackScreenTarget, this.m_blackScreenTimeTimer);
        if ((double) this.m_blackScreenTimeTimer == 1.0 && this.OnBlackscreenFadeFinishedCallback != null)
        {
          this.OnBlackscreenFadeFinishedCallback();
          this.OnBlackscreenFadeFinishedCallback = (Action) null;
        }
      }
      if ((double) this.m_blackScreenCurrent < 1.0)
      {
        if (this.BlackScreenMinimalizeHUD)
          MyHud.CutsceneHud = true;
        if ((double) this.m_blackScreenTarget < (double) this.m_blackScreenStart)
          MyGuiScreenGamePlay.DisableInput = true;
        MyPostprocessSettingsWrapper.Settings.Data.LightColor = (Vector3) this.BlackScreenColor;
        MyPostprocessSettingsWrapper.Settings.Data.DarkColor = (Vector3) this.BlackScreenColor;
        MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = 1f - this.m_blackScreenCurrent;
        MyPostprocessSettingsWrapper.MarkDirty();
      }
      else
      {
        MyGuiScreenGamePlay.DisableInput = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
        if (!this.m_blackScreenDataSaved)
          return;
        this.m_blackScreenDataSaved = false;
        MyHud.CutsceneHud = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
        MyPostprocessSettingsWrapper.Settings.Data.LightColor = (Vector3) this.m_blackScreenDataSavedLightColor;
        MyPostprocessSettingsWrapper.Settings.Data.DarkColor = (Vector3) this.m_blackScreenDataSavedDarkColor;
        MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = this.m_blackScreenDataSavedStrength;
        MyPostprocessSettingsWrapper.MarkDirty();
      }
    }
  }
}
