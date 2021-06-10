// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyFunctionalBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents.Systems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entities;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyFunctionalBlock), typeof (Sandbox.ModAPI.Ingame.IMyFunctionalBlock)})]
  public class MyFunctionalBlock : MyTerminalBlock, IMyTieredUpdateBlock, IMyUpdateTimer, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    private MyTimerComponent m_timer;
    private bool m_isTimerInRestartMode;
    protected MySoundPair m_baseIdleSound = new MySoundPair();
    protected MySoundPair m_actionSound = new MySoundPair();
    public MyEntity3DSoundEmitter m_soundEmitter;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_enabled;

    internal MyEntity3DSoundEmitter SoundEmitter => this.m_soundEmitter;

    public virtual bool IsTieredUpdateSupported => false;

    public virtual bool AllowTimerForceUpdate => true;

    public override void OnRemovedFromScene(object source)
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      if (this.Components.Contains(typeof (MyTriggerAggregate)))
        this.Components.Remove(typeof (MyTriggerAggregate));
      base.OnRemovedFromScene(source);
    }

    private void EnabledSyncChanged()
    {
      if (this.Closed)
        return;
      if (!this.Enabled && this.m_timer != null && this.AllowTimerForceUpdate)
        this.UpdateTimer(true);
      this.UpdateIsWorking();
      this.OnEnabledChanged();
    }

    public bool Enabled
    {
      get => (bool) this.m_enabled;
      set => this.m_enabled.Value = value;
    }

    public event Action<MyTerminalBlock> EnabledChanged;

    public event Action<MyFunctionalBlock> UpdateTimerTriggered;

    public MyFunctionalBlock()
    {
      this.CreateTerminalControls();
      this.m_enabled.ValueChanged += (Action<SyncBase>) (x => this.EnabledSyncChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyFunctionalBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyFunctionalBlock> onOff = new MyTerminalControlOnOffSwitch<MyFunctionalBlock>("OnOff", MySpaceTexts.BlockAction_Toggle);
      onOff.Getter = (MyTerminalValueControl<MyFunctionalBlock, bool>.GetterDelegate) (x => x.Enabled);
      onOff.Setter = (MyTerminalValueControl<MyFunctionalBlock, bool>.SetterDelegate) ((x, v) => x.Enabled = v);
      onOff.EnableToggleAction<MyFunctionalBlock>();
      onOff.EnableOnOffActions<MyFunctionalBlock>();
      MyTerminalControlFactory.AddControl<MyFunctionalBlock>(0, (MyTerminalControl<MyFunctionalBlock>) onOff);
      MyTerminalControlFactory.AddControl<MyFunctionalBlock>(1, (MyTerminalControl<MyFunctionalBlock>) new MyTerminalControlSeparator<MyFunctionalBlock>());
    }

    protected override bool CheckIsWorking() => this.Enabled && base.CheckIsWorking();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_FunctionalBlock builderFunctionalBlock = (MyObjectBuilder_FunctionalBlock) objectBuilder;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_enabled.SetLocalValue(builderFunctionalBlock.Enabled);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.CubeBlock_IsWorkingChanged);
      this.m_baseIdleSound = this.BlockDefinition.PrimarySound;
      this.m_actionSound = this.BlockDefinition.ActionSound;
      this.SetDetailedInfoDirty();
    }

    private void CubeBlock_IsWorkingChanged(MyCubeBlock obj)
    {
      if (this.IsWorking)
        this.OnStartWorking();
      else
        this.OnStopWorking();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_FunctionalBlock builderCubeBlock = (MyObjectBuilder_FunctionalBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Enabled = this.Enabled;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected virtual void OnEnabledChanged()
    {
      if (this.IsWorking)
        this.OnStartWorking();
      else
        this.OnStopWorking();
      this.EnabledChanged.InvokeIfNotNull<MyTerminalBlock>((MyTerminalBlock) this);
      this.RaisePropertiesChanged();
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.m_soundEmitter == null || !this.SilenceInChange)
        return;
      this.SilenceInChange = this.m_soundEmitter.FastUpdate(this.IsSilenced);
      if (this.SilenceInChange || this.UsedUpdateEveryFrame || this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (this.m_soundEmitter == null || MySector.MainCamera == null)
        return;
      this.UpdateSoundEmitters();
    }

    public virtual void UpdateSoundEmitters()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_timer == null || this.m_timer.TimerType != MyTimerTypes.Frame10 || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.UpdateTimer(false);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.m_timer == null || this.m_timer.TimerType != MyTimerTypes.Frame100 || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.UpdateTimer(false);
    }

    protected virtual void OnStartWorking()
    {
      if (!this.InScene || this.CubeGrid.Physics == null || (this.m_soundEmitter == null || this.m_baseIdleSound == null) || this.m_baseIdleSound == MySoundPair.Empty)
        return;
      this.m_soundEmitter.PlaySound(this.m_baseIdleSound, true);
    }

    protected virtual void OnStopWorking()
    {
      if (this.m_soundEmitter == null || this.BlockDefinition.DamagedSound != null && !(this.m_soundEmitter.SoundId != this.BlockDefinition.DamagedSound.SoundId))
        return;
      this.m_soundEmitter.StopSound(false);
    }

    protected override void Closing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    public override void SetDamageEffect(bool show)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      base.SetDamageEffect(show);
      if (this.m_soundEmitter == null || this.BlockDefinition.DamagedSound == null)
        return;
      if (show)
      {
        this.m_soundEmitter.PlaySound(this.BlockDefinition.DamagedSound, true);
      }
      else
      {
        if (!(this.m_soundEmitter.SoundId == this.BlockDefinition.DamagedSound.SoundId))
          return;
        this.m_soundEmitter.StopSound(false);
      }
    }

    public override void StopDamageEffect(bool stopSound = true)
    {
      base.StopDamageEffect(stopSound);
      if (!stopSound || this.m_soundEmitter == null || this.BlockDefinition.DamagedSound == null || !(this.m_soundEmitter.SoundId == this.BlockDefinition.DamagedSound.Arcade) && !(this.m_soundEmitter.SoundId != this.BlockDefinition.DamagedSound.Realistic))
        return;
      this.m_soundEmitter.StopSound(true);
    }

    public override void OnDestroy() => base.OnDestroy();

    public virtual int GetBlockSpecificState() => -1;

    public void ChangeTier() => this.TiersChanged();

    protected virtual void TiersChanged()
    {
    }

    public void CreateUpdateTimer(uint startingTimeInFrames, MyTimerTypes timerType, bool start = false)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      if (!this.Components.TryGet<MyTimerComponent>(out this.m_timer))
      {
        this.m_timer = new MyTimerComponent();
        this.m_timer.IsSessionUpdateEnabled = false;
        this.m_timer.SetType(timerType);
        this.m_timer.SetTimer(startingTimeInFrames, new Action<MyEntityComponentContainer>(this.OnTimerTick), start, true);
        this.Components.Add<MyTimerComponent>(this.m_timer);
      }
      else
      {
        this.m_timer.SetType(timerType);
        this.m_timer.IsSessionUpdateEnabled = false;
        if (MyTimerComponentSystem.Static != null)
          MyTimerComponentSystem.Static.Unregister(this.m_timer);
        this.m_timer.EventToTrigger = new Action<MyEntityComponentContainer>(this.OnTimerTick);
      }
      this.m_isTimerInRestartMode = start;
    }

    private void OnTimerTick(MyEntityComponentContainer obj)
    {
      if (this.CubeGrid != null && this.CubeGrid.IsPreview)
        return;
      this.DoUpdateTimerTick();
      this.UpdateTimerTriggered.InvokeIfNotNull<MyFunctionalBlock>(this);
    }

    public uint GetFramesFromLastTrigger() => this.m_timer.FramesFromLastTrigger;

    public void ChangeTimerTick(uint timeTickInFrames)
    {
      this.m_timer.ChangeTimerTick(timeTickInFrames);
      if (timeTickInFrames != 0U)
        return;
      this.m_timer.Pause();
    }

    private void UpdateTimer(bool forceUpdate)
    {
      if (this.GetTimerEnabledState())
      {
        if (!this.m_timer.TimerEnabled)
        {
          if (this.m_timer.TimerTickInFrames > 0U)
            this.m_timer.Resume(this.m_isTimerInRestartMode);
          else
            this.m_timer.Pause();
        }
      }
      else if (this.m_timer.TimerEnabled)
        this.m_timer.Pause();
      this.m_timer.Update(forceUpdate);
    }

    public virtual bool GetTimerEnabledState() => false;

    public virtual void DoUpdateTimerTick()
    {
    }

    protected uint GetTimerTime(int index) => index < 0 || this.BlockDefinition == null || (this.BlockDefinition.TieredUpdateTimes == null || this.BlockDefinition.TieredUpdateTimes.Count <= index) ? this.GetDefaultTimeForUpdateTimer(index) : this.BlockDefinition.TieredUpdateTimes[index];

    protected virtual uint GetDefaultTimeForUpdateTimer(int index) => 0;

    event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyFunctionalBlock.EnabledChanged
    {
      add => this.EnabledChanged += this.GetDelegate(value);
      remove => this.EnabledChanged -= this.GetDelegate(value);
    }

    private Action<MyTerminalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock> value) => (Action<MyTerminalBlock>) Delegate.CreateDelegate(typeof (Action<MyTerminalBlock>), value.Target, value.Method);

    void Sandbox.ModAPI.Ingame.IMyFunctionalBlock.RequestEnable(bool enable) => this.Enabled = enable;

    uint Sandbox.ModAPI.IMyFunctionalBlock.GetFramesFromLastTrigger() => this.GetFramesFromLastTrigger();

    private Action<MyFunctionalBlock> GetDelegate(
      Action<Sandbox.ModAPI.IMyFunctionalBlock> value)
    {
      return (Action<MyFunctionalBlock>) Delegate.CreateDelegate(typeof (Action<MyFunctionalBlock>), value.Target, value.Method);
    }

    event Action<Sandbox.ModAPI.IMyFunctionalBlock> Sandbox.ModAPI.IMyFunctionalBlock.UpdateTimerTriggered
    {
      add => this.UpdateTimerTriggered += this.GetDelegate(value);
      remove => this.UpdateTimerTriggered -= this.GetDelegate(value);
    }

    bool Sandbox.ModAPI.IMyFunctionalBlock.IsUpdateTimerCreated => this.m_timer != null;

    bool Sandbox.ModAPI.IMyFunctionalBlock.IsUpdateTimerEnabled => this.m_timer != null && this.m_timer.TimerEnabled;

    protected class m_enabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyFunctionalBlock) obj0).m_enabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyFunctionalBlock\u003C\u003EActor : IActivator, IActivator<MyFunctionalBlock>
    {
      object IActivator.CreateInstance() => (object) new MyFunctionalBlock();

      MyFunctionalBlock IActivator<MyFunctionalBlock>.CreateInstance() => new MyFunctionalBlock();
    }
  }
}
