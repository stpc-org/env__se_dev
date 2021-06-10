// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugProjectiles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Projectiles")]
  internal class MyGuiScreenDebugProjectiles : MyGuiScreenDebugBase
  {
    private static MyGuiScreenDebugProjectiles m_instance;

    public MyGuiScreenDebugProjectiles()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugProjectiles);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      MyGuiScreenDebugProjectiles.m_instance = this;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddSlider("Impact snd timeout", (float) MyProjectile.CollisionSoundsTimedCache.EventTimeoutMs, 0.0f, 1000f, (Action<MyGuiControlSlider>) (x => MyProjectile.CollisionSoundsTimedCache.EventTimeoutMs = (int) x.Value));
      this.AddSlider("Impact part. timeout", (float) MyProjectile.CollisionParticlesTimedCache.EventTimeoutMs, 0.0f, 1000f, (Action<MyGuiControlSlider>) (x => MyProjectile.CollisionParticlesTimedCache.EventTimeoutMs = (int) x.Value));
      this.AddSlider("Impact part. cube size", (float) (1.0 / MyProjectile.CollisionParticlesSpaceMapping), 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyProjectile.CollisionParticlesSpaceMapping = 1.0 / (double) x.Value));
    }
  }
}
