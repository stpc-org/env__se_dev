// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.GameLogic.MyLifeSupportingComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.World;
using System;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;

namespace SpaceEngineers.Game.EntityComponents.GameLogic
{
  public class MyLifeSupportingComponent : MyEntityComponentBase
  {
    private int m_lastTimeUsed;
    private readonly MySoundPair m_progressSound;
    private readonly MyEntity3DSoundEmitter m_progressSoundEmitter;
    private string m_actionName;
    private float m_rechargeMultiplier = 1f;

    public IMyLifeSupportingBlock Entity => (IMyLifeSupportingBlock) base.Entity;

    public MyCharacter User { get; private set; }

    public MyRechargeSocket RechargeSocket { get; private set; }

    public MyLifeSupportingComponent(
      MyEntity owner,
      MySoundPair progressSound,
      string actionName = "GenericHeal",
      float rechargeMultiplier = 1f)
    {
      this.RechargeSocket = new MyRechargeSocket();
      this.m_actionName = actionName;
      this.m_rechargeMultiplier = rechargeMultiplier;
      this.m_progressSound = progressSound;
      this.m_progressSoundEmitter = new MyEntity3DSoundEmitter(owner, true);
      this.m_progressSoundEmitter.EmitterMethods[1].Add((Delegate) (() => MySession.Static.ControlledEntity != null && this.User == MySession.Static.ControlledEntity.Entity));
      if (MySession.Static == null || !MyFakes.ENABLE_NEW_SOUNDS || !MySession.Static.Settings.RealisticSound)
        return;
      this.m_progressSoundEmitter.EmitterMethods[0].Add((Delegate) (() => MySession.Static.ControlledEntity != null && this.User == MySession.Static.ControlledEntity.Entity));
    }

    public void OnSupportRequested(MyCharacter user)
    {
      if (this.User != null && this.User != user)
        return;
      this.Entity.BroadcastSupportRequest(user);
    }

    public void ProvideSupport(MyCharacter user)
    {
      if (!this.Entity.IsWorking)
        return;
      bool flag = false;
      if (this.User == null)
      {
        this.User = user;
        if (this.Entity.RefuelAllowed)
        {
          user.SuitBattery.ResourceSink.TemporaryConnectedEntity = (IMyEntity) this.Entity;
          user.SuitBattery.RechargeMultiplier = this.m_rechargeMultiplier;
          this.RechargeSocket.PlugIn(user.SuitBattery.ResourceSink);
          flag = true;
          PlayerSuitRechargeEvent playerSuitRecharging = MyVisualScriptLogicProvider.PlayerSuitRecharging;
          if (playerSuitRecharging != null)
            playerSuitRecharging(this.User.GetPlayerIdentityId(), this.Entity.BlockType);
        }
      }
      this.m_lastTimeUsed = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.User.StatComp != null && this.Entity.HealingAllowed)
      {
        this.User.StatComp.DoAction(this.m_actionName);
        flag = true;
        PlayerHealthRechargeEvent healthRecharging = MyVisualScriptLogicProvider.PlayerHealthRecharging;
        if (healthRecharging != null)
        {
          float num = this.User.StatComp.Health != null ? this.User.StatComp.Health.Value : 0.0f;
          healthRecharging(this.User.GetPlayerIdentityId(), this.Entity.BlockType, num);
        }
      }
      if (!flag)
        return;
      this.PlayProgressLoopSound();
    }

    private void PlayProgressLoopSound()
    {
      if (this.m_progressSoundEmitter.IsPlaying)
        return;
      this.m_progressSoundEmitter.PlaySound(this.m_progressSound, true);
    }

    private void StopProgressLoopSound() => this.m_progressSoundEmitter.StopSound(false);

    public void UpdateSoundEmitters() => this.m_progressSoundEmitter.Update();

    public void Update10()
    {
      if (this.User == null || MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeUsed < 100)
        return;
      this.Unplug();
    }

    public override void OnRemovedFromScene()
    {
      this.Unplug();
      base.OnRemovedFromScene();
    }

    private void Unplug()
    {
      if (this.User == null)
        return;
      this.RechargeSocket.Unplug();
      this.User.SuitBattery.ResourceSink.TemporaryConnectedEntity = (IMyEntity) null;
      this.User.SuitBattery.RechargeMultiplier = 1f;
      this.User = (MyCharacter) null;
      this.StopProgressLoopSound();
    }

    public override string ComponentTypeDebugString => this.GetType().Name;
  }
}
