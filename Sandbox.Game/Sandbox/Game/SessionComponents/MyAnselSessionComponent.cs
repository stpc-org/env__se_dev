// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyAnselSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game.Components;
using VRage.Input;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyAnselSessionComponent : MySessionComponentBase
  {
    private MyAnselSessionComponent.MyScreenTool m_screenTool;
    private bool m_isSessionRunning;
    private readonly MyNullInput m_nullInput = new MyNullInput();
    private bool m_prevHeadRenderingEnabled;
    private IMyInput m_prevInput;
    private IAnsel m_ansel;

    private void SessionStarted()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null)
      {
        this.m_prevHeadRenderingEnabled = localCharacter.HeadRenderingEnabled;
        localCharacter.EnableHead(true);
      }
      this.m_prevInput = MyInput.Static;
      MyInput.Static = (IMyInput) this.m_nullInput;
      if (this.m_ansel.IsGamePausable)
        MySandboxGame.PausePush();
      MyRenderProxy.SetFrameTimeStep(-1f);
    }

    private void SessionEnded()
    {
      if (this.m_ansel != null && this.m_ansel.IsGamePausable)
        MySandboxGame.PausePop();
      MyInput.Static = this.m_prevInput;
      MySession.Static.LocalCharacter?.EnableHead(this.m_prevHeadRenderingEnabled);
      MyRenderProxy.SetFrameTimeStep();
    }

    public override void LoadData()
    {
      this.m_screenTool.Init();
      this.m_ansel = MyVRage.Platform.Ansel;
      this.m_ansel.IsGamePausable = !Sync.MultiplayerActive;
      this.m_ansel.IsSessionEnabled = true;
      if (MyFakes.ENABLE_ANSEL_IN_MULTIPLAYER)
        return;
      this.m_ansel.IsSessionEnabled = !Sync.MultiplayerActive;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_ansel.IsSessionEnabled = false;
      if (!this.m_isSessionRunning)
        return;
      this.m_ansel.StopSession();
      this.m_ansel = (IAnsel) null;
      this.SessionEnded();
    }

    public override void Draw()
    {
      base.Draw();
      bool isSessionRunning = this.m_ansel.IsSessionRunning;
      if (isSessionRunning != this.m_isSessionRunning)
      {
        if (this.m_isSessionRunning)
        {
          this.SessionEnded();
          this.m_screenTool.Restore();
        }
        else
          this.SessionStarted();
        this.m_isSessionRunning = isSessionRunning;
      }
      if (!isSessionRunning)
        return;
      if (this.m_ansel.IsOverlayEnabled)
        this.m_screenTool.Restore();
      else
        this.m_screenTool.Hide();
    }

    private struct MyScreenTool
    {
      private bool m_isHidden;
      private MyAnselGuiScreen m_anselGuiScreen;
      private bool m_prevMinimalHud;

      public void Init() => this.m_anselGuiScreen = new MyAnselGuiScreen();

      public void Hide()
      {
        if (this.m_isHidden)
          return;
        this.m_prevMinimalHud = MyHud.MinimalHud;
        this.m_isHidden = true;
        MySandboxGame.Static.Invoke(new Action(this.HideInternal), "AnselGuiHide");
      }

      private void HideInternal()
      {
        MyHud.MinimalHud = true;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) this.m_anselGuiScreen);
      }

      public void Restore()
      {
        if (!this.m_isHidden)
          return;
        this.m_isHidden = false;
        MySandboxGame.Static.Invoke(new Action(this.RestoreInternal), "AnselGuiRestore");
      }

      private void RestoreInternal()
      {
        MyGuiSandbox.RemoveScreen((MyGuiScreenBase) this.m_anselGuiScreen);
        MyHud.MinimalHud = this.m_prevMinimalHud;
      }
    }
  }
}
