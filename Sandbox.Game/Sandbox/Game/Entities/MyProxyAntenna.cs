// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyProxyAntenna
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_ProxyAntenna), true)]
  internal class MyProxyAntenna : MyEntity, IMyComponentOwner<MyIDModule>
  {
    private Dictionary<long, MyHudEntityParams> m_savedHudParams = new Dictionary<long, MyHudEntityParams>();
    private bool m_active;
    private MyIDModule m_IDModule = new MyIDModule();
    private bool m_registered;

    public bool Active
    {
      get => this.m_active;
      set
      {
        if (this.m_active == value)
          return;
        this.m_active = value;
        if (this.Receiver != null)
        {
          this.Receiver.Enabled = value;
          if (value)
            this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
          else
            this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
        }
        if (this.Broadcaster is MyRadioBroadcaster broadcaster)
        {
          broadcaster.Enabled = this.m_active;
        }
        else
        {
          if (!(this.Broadcaster is MyLaserBroadcaster broadcaster))
            return;
          if (this.m_active)
          {
            if (MyAntennaSystem.Static.LaserAntennas.ContainsKey(this.AntennaEntityId))
              return;
            MyAntennaSystem.Static.AddLaser(this.AntennaEntityId, broadcaster, false);
          }
          else
            MyAntennaSystem.Static.RemoveLaser(this.AntennaEntityId, false);
        }
      }
    }

    public bool IsCharacter { get; private set; }

    public MyDataBroadcaster Broadcaster
    {
      get => this.Components.Get<MyDataBroadcaster>();
      set => this.Components.Add<MyDataBroadcaster>(value);
    }

    public MyDataReceiver Receiver
    {
      get => this.Components.Get<MyDataReceiver>();
      set => this.Components.Add<MyDataReceiver>(value);
    }

    public MyAntennaSystem.BroadcasterInfo Info { get; set; }

    public long AntennaEntityId { get; private set; }

    public long? SuccessfullyContacting { get; set; }

    public bool HasRemoteControl { get; set; }

    public long? MainRemoteControlOwner { get; set; }

    public long? MainRemoteControlId { get; set; }

    public MyOwnershipShareModeEnum MainRemoteControlSharing { get; set; }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      MyObjectBuilder_ProxyAntenna builderProxyAntenna = objectBuilder as MyObjectBuilder_ProxyAntenna;
      this.AntennaEntityId = builderProxyAntenna.AntennaEntityId;
      this.PositionComp.SetPosition((Vector3D) builderProxyAntenna.Position);
      this.IsCharacter = builderProxyAntenna.IsCharacter;
      if (!builderProxyAntenna.IsLaser)
      {
        MyRadioBroadcaster radioBroadcaster = new MyRadioBroadcaster();
        this.Broadcaster = (MyDataBroadcaster) radioBroadcaster;
        radioBroadcaster.BroadcastRadius = builderProxyAntenna.BroadcastRadius;
        this.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.WorldPositionChanged);
        if (builderProxyAntenna.HasReceiver)
          this.Receiver = (MyDataReceiver) new MyRadioReceiver();
      }
      else
      {
        MyLaserBroadcaster laserBroadcaster = new MyLaserBroadcaster();
        this.Broadcaster = (MyDataBroadcaster) laserBroadcaster;
        this.SuccessfullyContacting = builderProxyAntenna.SuccessfullyContacting;
        laserBroadcaster.StateText.Clear().Append(builderProxyAntenna.StateText);
        this.Receiver = (MyDataReceiver) new MyLaserReceiver();
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_IDModule.Owner = builderProxyAntenna.Owner;
      this.m_IDModule.ShareMode = builderProxyAntenna.Share;
      this.Info = new MyAntennaSystem.BroadcasterInfo()
      {
        EntityId = builderProxyAntenna.InfoEntityId,
        Name = builderProxyAntenna.InfoName
      };
      foreach (MyObjectBuilder_HudEntityParams hudParam in builderProxyAntenna.HudParams)
        this.m_savedHudParams[hudParam.EntityId] = new MyHudEntityParams(hudParam);
      this.HasRemoteControl = builderProxyAntenna.HasRemote;
      this.MainRemoteControlOwner = builderProxyAntenna.MainRemoteOwner;
      this.MainRemoteControlId = builderProxyAntenna.MainRemoteId;
      this.MainRemoteControlSharing = builderProxyAntenna.MainRemoteSharing;
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_ProxyAntenna objectBuilder = (MyObjectBuilder_ProxyAntenna) base.GetObjectBuilder(copy);
      this.Broadcaster.InitProxyObjectBuilder(objectBuilder);
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      this.m_hudParams.Clear();
      foreach (MyHudEntityParams myHudEntityParams in this.m_savedHudParams.Values)
        this.m_hudParams.Add(new MyHudEntityParams()
        {
          EntityId = myHudEntityParams.EntityId,
          FlagsEnum = myHudEntityParams.FlagsEnum,
          Owner = myHudEntityParams.Owner,
          Share = myHudEntityParams.Share,
          Position = this.PositionComp.GetPosition(),
          Text = myHudEntityParams.Text,
          BlinkingTime = myHudEntityParams.BlinkingTime
        });
      return this.m_hudParams;
    }

    public override void UpdateOnceBeforeFrame()
    {
      this.m_registered = true;
      MyAntennaSystem.Static.RegisterAntenna(this.Broadcaster);
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.Receiver == null)
        return;
      this.Receiver.UpdateBroadcastersInRange();
    }

    private void WorldPositionChanged(object source)
    {
      if (!(this.Broadcaster is MyRadioBroadcaster broadcaster))
        return;
      broadcaster.MoveBroadcaster();
    }

    protected override void Closing()
    {
      if (this.m_registered)
        MyAntennaSystem.Static.UnregisterAntenna(this.Broadcaster);
      this.m_registered = false;
      base.Closing();
    }

    public void ChangeOwner(long newOwner, MyOwnershipShareModeEnum newShare)
    {
      this.m_IDModule.Owner = newOwner;
      this.m_IDModule.ShareMode = newShare;
    }

    public void ChangeHudParams(List<MyObjectBuilder_HudEntityParams> newHudParams)
    {
      foreach (MyObjectBuilder_HudEntityParams newHudParam in newHudParams)
        this.m_savedHudParams[newHudParam.EntityId] = new MyHudEntityParams(newHudParam);
    }

    bool IMyComponentOwner<MyIDModule>.GetComponent(
      out MyIDModule component)
    {
      component = this.m_IDModule;
      return this.m_IDModule != null;
    }

    private class Sandbox_Game_Entities_MyProxyAntenna\u003C\u003EActor : IActivator, IActivator<MyProxyAntenna>
    {
      object IActivator.CreateInstance() => (object) new MyProxyAntenna();

      MyProxyAntenna IActivator<MyProxyAntenna>.CreateInstance() => new MyProxyAntenna();
    }
  }
}
