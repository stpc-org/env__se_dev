// Decompiled with JetBrains decompiler
// Type: Sandbox.AppCode.MyExternalAppBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.AppCode
{
  public class MyExternalAppBase : IExternalApp
  {
    public static MySandboxGame Static;
    private static bool m_isEditorActive;
    private static bool m_isPresent;

    public static bool IsEditorActive
    {
      get => MyExternalAppBase.m_isEditorActive;
      set => MyExternalAppBase.m_isEditorActive = value;
    }

    public static bool IsPresent
    {
      get => MyExternalAppBase.m_isPresent;
      set => MyExternalAppBase.m_isPresent = value;
    }

    public void Run(IntPtr windowHandle, bool customRenderLoop = false, MySandboxGame game = null)
    {
      MyLog.Default = MySandboxGame.Log;
      MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = false;
      MyServiceManager.Instance.AddService<IMyGameService>((IMyGameService) new MyNullGameService());
      MyServiceManager.Instance.AddService<IMyUGCService>((IMyUGCService) new MyNullUGCService());
      MyExternalAppBase.Static = game != null ? game : (MySandboxGame) new MySandboxExternal((IExternalApp) this, (string[]) null, windowHandle);
      this.Initialize((Sandbox.Engine.Platform.Game) MyExternalAppBase.Static);
      MyExternalAppBase.Static.OnGameLoaded += new EventHandler(this.GameLoaded);
      MyExternalAppBase.Static.OnGameExit += new Action(this.GameExit);
      MySession.AfterLoading += new Action(this.MySession_AfterLoading);
      MySession.BeforeLoading += new Action(this.MySession_BeforeLoading);
      MyExternalAppBase.Static.Run(customRenderLoop);
      if (customRenderLoop)
        return;
      this.Dispose();
    }

    public virtual void GameExit()
    {
    }

    public void Dispose()
    {
      MyExternalAppBase.Static.Dispose();
      MyExternalAppBase.Static = (MySandboxGame) null;
    }

    public void RunSingleFrame() => MyExternalAppBase.Static.RunSingleFrame();

    public void EndLoop() => MyExternalAppBase.Static.EndLoop();

    void IExternalApp.Draw() => this.Draw(false);

    void IExternalApp.Update() => this.Update(true);

    void IExternalApp.UpdateMainThread() => this.UpdateMainThread();

    public virtual void Initialize(Sandbox.Engine.Platform.Game game)
    {
    }

    public virtual void UpdateMainThread()
    {
    }

    public virtual void Update(bool canDraw)
    {
    }

    public virtual void Draw(bool canDraw)
    {
    }

    public virtual void GameLoaded(object sender, EventArgs e)
    {
      MyExternalAppBase.IsEditorActive = true;
      MyExternalAppBase.IsPresent = true;
    }

    public virtual void MySession_AfterLoading()
    {
    }

    public virtual void MySession_BeforeLoading()
    {
    }

    public void RemoveEffect(MyParticleEffect effect) => MyParticlesManager.RemoveParticleEffect(effect);

    public void LoadDefinitions() => MyDefinitionManager.Static.LoadData(new List<MyObjectBuilder_Checkpoint.ModItem>());

    public float GetStepInSeconds() => 0.01666667f;
  }
}
