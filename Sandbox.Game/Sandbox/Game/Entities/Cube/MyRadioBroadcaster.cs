// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyRadioBroadcaster
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities.Cube
{
  public class MyRadioBroadcaster : MyDataBroadcaster
  {
    public Action OnBroadcastRadiusChanged;
    private float m_broadcastRadius;
    private bool m_enabled;
    public bool WantsToBeEnabled = true;
    public int m_radioProxyID = -1;
    private bool m_registered;

    public MyRadioBroadcaster(float broadcastRadius = 100f) => this.m_broadcastRadius = broadcastRadius;

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.Enabled = false;
    }

    public bool Enabled
    {
      get => this.m_enabled;
      set
      {
        if (this.m_enabled == value)
          return;
        if (!this.IsProjection())
        {
          if (value)
            MyRadioBroadcasters.AddBroadcaster(this);
          else
            MyRadioBroadcasters.RemoveBroadcaster(this);
        }
        this.m_enabled = value;
      }
    }

    public void MoveBroadcaster() => MyRadioBroadcasters.MoveBroadcaster(this);

    public float BroadcastRadius
    {
      get => this.m_broadcastRadius;
      set
      {
        if ((double) this.m_broadcastRadius == (double) value)
          return;
        this.m_broadcastRadius = value;
        if (this.m_enabled)
        {
          MyRadioBroadcasters.RemoveBroadcaster(this);
          MyRadioBroadcasters.AddBroadcaster(this);
        }
        Action broadcastRadiusChanged = this.OnBroadcastRadiusChanged;
        if (broadcastRadiusChanged == null)
          return;
        broadcastRadiusChanged();
      }
    }

    public int RadioProxyID
    {
      get => this.m_radioProxyID;
      set => this.m_radioProxyID = value;
    }

    private bool IsProjection() => this.Entity is MyCubeBlock entity && entity.CubeGrid.Physics == null;

    public override void InitProxyObjectBuilder(MyObjectBuilder_ProxyAntenna ob)
    {
      base.InitProxyObjectBuilder(ob);
      ob.IsLaser = false;
      ob.BroadcastRadius = this.BroadcastRadius;
    }

    public void RaiseBroadcastRadiusChanged()
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyRadioBroadcaster, float>(this, (Func<MyRadioBroadcaster, Action<float>>) (x => new Action<float>(x.ChangeBroadcastRadius)), this.BroadcastRadius);
    }

    [Event(null, 118)]
    [Reliable]
    [Broadcast]
    public void ChangeBroadcastRadius(float newRadius) => this.BroadcastRadius = newRadius;

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (this.Entity.GetTopMostParent().Physics == null)
        return;
      this.m_registered = true;
      MyAntennaSystem.Static.RegisterAntenna((MyDataBroadcaster) this);
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      if (this.m_registered)
        MyAntennaSystem.Static.UnregisterAntenna((MyDataBroadcaster) this);
      this.m_registered = false;
    }

    protected sealed class ChangeBroadcastRadius\u003C\u003ESystem_Single : ICallSite<MyRadioBroadcaster, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyRadioBroadcaster @this,
        in float newRadius,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ChangeBroadcastRadius(newRadius);
      }
    }

    private class Sandbox_Game_Entities_Cube_MyRadioBroadcaster\u003C\u003EActor
    {
    }
  }
}
