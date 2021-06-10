// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityReloading
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityReloading : MyStatBase
  {
    private MyUserControllableGun m_lastConnected;
    private int m_reloadCompletionTime;
    private int m_reloadInterval;

    public MyStatControlledEntityReloading() => this.Id = MyStringHash.GetOrCompute("controlled_reloading");

    public override void Update()
    {
      MyUserControllableGun controlledEntity = MySession.Static.ControlledEntity as MyUserControllableGun;
      if (controlledEntity != this.m_lastConnected)
      {
        if (this.m_lastConnected != null)
          this.m_lastConnected.ReloadStarted -= new Action<int>(this.OnReloading);
        this.m_lastConnected = controlledEntity;
        if (controlledEntity != null)
          controlledEntity.ReloadStarted += new Action<int>(this.OnReloading);
        this.m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
      int num = this.m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (num > 0)
        this.CurrentValue = (float) (1.0 - (double) num / (double) this.m_reloadInterval);
      else
        this.CurrentValue = 0.0f;
    }

    private void OnReloading(int reloadTime)
    {
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_reloadCompletionTime > timeInMilliseconds)
        return;
      this.m_reloadCompletionTime = timeInMilliseconds + reloadTime;
      this.m_reloadInterval = reloadTime;
    }
  }
}
