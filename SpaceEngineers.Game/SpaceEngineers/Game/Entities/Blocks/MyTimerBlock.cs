// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyTimerBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_TimerBlock))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyTimerBlock), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock)})]
  public class MyTimerBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyTimerBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock, IMyTriggerableBlock
  {
    private int m_countdownMsCurrent;
    private int m_countdownMsStart;
    private MySoundPair m_beepStart = MySoundPair.Empty;
    private MySoundPair m_beepMid = MySoundPair.Empty;
    private MySoundPair m_beepEnd = MySoundPair.Empty;
    private MyEntity3DSoundEmitter m_beepEmitter;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_silent;
    private static List<MyToolbar> m_openedToolbars;
    private static bool m_shouldSetOtherToolbars;
    private bool m_syncing;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_timerSync;

    public MyToolbar Toolbar { get; set; }

    public bool IsCountingDown { get; set; }

    public bool Silent
    {
      get => (bool) this.m_silent;
      private set => this.m_silent.Value = value;
    }

    public float TriggerDelay
    {
      get => (float) Math.Round((double) this.m_timerSync.Value) / 1000f;
      set => this.m_timerSync.Value = value * 1000f;
    }

    public MyTimerBlock()
    {
      this.CreateTerminalControls();
      MyTimerBlock.m_openedToolbars = new List<MyToolbar>();
      this.m_timerSync.ValueChanged += (Action<SyncBase>) (x => this.TimerChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyTimerBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyTimerBlock> checkbox = new MyTerminalControlCheckbox<MyTimerBlock>("Silent", MySpaceTexts.BlockPropertyTitle_Silent, MySpaceTexts.ToolTipTimerBlock_Silent);
      checkbox.Getter = (MyTerminalValueControl<MyTimerBlock, bool>.GetterDelegate) (x => x.Silent);
      checkbox.Setter = (MyTerminalValueControl<MyTimerBlock, bool>.SetterDelegate) ((x, v) => x.Silent = v);
      checkbox.EnableAction<MyTimerBlock>();
      MyTerminalControlFactory.AddControl<MyTimerBlock>((MyTerminalControl<MyTimerBlock>) checkbox);
      MyTerminalControlSlider<MyTimerBlock> slider = new MyTerminalControlSlider<MyTimerBlock>("TriggerDelay", MySpaceTexts.TerminalControlPanel_TimerDelay, MySpaceTexts.TerminalControlPanel_TimerDelay);
      slider.SetLogLimits(1f, 3600f);
      slider.DefaultValue = new float?(10f);
      slider.Enabled = (Func<MyTimerBlock, bool>) (x => !x.IsCountingDown);
      slider.Getter = (MyTerminalValueControl<MyTimerBlock, float>.GetterDelegate) (x => x.TriggerDelay);
      slider.Setter = (MyTerminalValueControl<MyTimerBlock, float>.SetterDelegate) ((x, v) => x.TriggerDelay = v);
      slider.Writer = (MyTerminalControl<MyTimerBlock>.WriterDelegate) ((x, sb) => MyValueFormatter.AppendTimeExact(Math.Max((int) x.TriggerDelay, 1), sb));
      slider.EnableActions<MyTimerBlock>();
      MyTerminalControlFactory.AddControl<MyTimerBlock>((MyTerminalControl<MyTimerBlock>) slider);
      MyTerminalControlFactory.AddControl<MyTimerBlock>((MyTerminalControl<MyTimerBlock>) new MyTerminalControlButton<MyTimerBlock>("OpenToolbar", MySpaceTexts.BlockPropertyTitle_TimerToolbarOpen, MySpaceTexts.BlockPropertyTitle_TimerToolbarOpen, (Action<MyTimerBlock>) (self =>
      {
        MyTimerBlock.m_openedToolbars.Add(self.Toolbar);
        if (MyGuiScreenToolbarConfigBase.Static != null)
          return;
        MyTimerBlock.m_shouldSetOtherToolbars = true;
        MyToolbarComponent.CurrentToolbar = self.Toolbar;
        MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) self, null);
        MyToolbarComponent.AutoUpdate = false;
        screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          MyToolbarComponent.AutoUpdate = true;
          MyTimerBlock.m_openedToolbars.Clear();
        });
        MyGuiSandbox.AddScreen(screen);
      })));
      MyTerminalControlButton<MyTimerBlock> button1 = new MyTerminalControlButton<MyTimerBlock>("TriggerNow", MySpaceTexts.BlockPropertyTitle_TimerTrigger, MySpaceTexts.BlockPropertyTitle_TimerTrigger, (Action<MyTimerBlock>) (x => x.OnTrigger()));
      button1.EnableAction<MyTimerBlock>();
      MyTerminalControlFactory.AddControl<MyTimerBlock>((MyTerminalControl<MyTimerBlock>) button1);
      MyTerminalControlButton<MyTimerBlock> button2 = new MyTerminalControlButton<MyTimerBlock>("Start", MySpaceTexts.BlockPropertyTitle_TimerStart, MySpaceTexts.BlockPropertyTitle_TimerStart, (Action<MyTimerBlock>) (x => x.StartBtn()));
      button2.EnableAction<MyTimerBlock>();
      MyTerminalControlFactory.AddControl<MyTimerBlock>((MyTerminalControl<MyTimerBlock>) button2);
      MyTerminalControlButton<MyTimerBlock> button3 = new MyTerminalControlButton<MyTimerBlock>("Stop", MySpaceTexts.BlockPropertyTitle_TimerStop, MySpaceTexts.BlockPropertyTitle_TimerStop, (Action<MyTimerBlock>) (x => x.StopBtn()));
      button3.EnableAction<MyTimerBlock>();
      MyTerminalControlFactory.AddControl<MyTimerBlock>((MyTerminalControl<MyTimerBlock>) button3);
    }

    private void TimerChanged() => this.SetTimer((int) this.TriggerDelay * 1000);

    private void StopBtn() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTimerBlock>(this, (Func<MyTimerBlock, Action>) (x => new Action(x.Stop)));

    private void StartBtn() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTimerBlock>(this, (Func<MyTimerBlock, Action>) (x => new Action(x.Start)));

    [Event(null, 155)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void Stop()
    {
      this.IsCountingDown = false;
      this.NeedsUpdate &= ~(MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
      this.ClearMemory();
      this.UpdateEmissivity();
    }

    private void ClearMemory()
    {
      this.m_countdownMsCurrent = 0;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    [Event(null, 171)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void Start()
    {
      this.IsCountingDown = true;
      this.m_countdownMsCurrent = this.m_countdownMsStart;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (this.m_beepEmitter != null && !this.Silent)
        this.m_beepEmitter.PlaySound(this.m_beepStart);
      this.UpdateEmissivity();
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_syncing)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTimerBlock, ToolbarItem, int>(this, (Func<MyTimerBlock, Action<ToolbarItem, int>>) (x => new Action<ToolbarItem, int>(x.SendToolbarItemChanged)), ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex)), index.ItemIndex);
      if (!MyTimerBlock.m_shouldSetOtherToolbars)
        return;
      MyTimerBlock.m_shouldSetOtherToolbars = false;
      foreach (MyToolbar openedToolbar in MyTimerBlock.m_openedToolbars)
      {
        if (openedToolbar != self)
          openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
      }
      MyTimerBlock.m_shouldSetOtherToolbars = true;
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      if (this.m_countdownMsCurrent == 0)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyTimerBlockDefinition blockDefinition = this.BlockDefinition as MyTimerBlockDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 1E-07f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      if (blockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
        blockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("Timer");
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_TimerBlock builderTimerBlock = objectBuilder as MyObjectBuilder_TimerBlock;
      this.Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, pageCount: 10);
      this.Toolbar.Init(builderTimerBlock.Toolbar, (MyEntity) this);
      this.Toolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      if (builderTimerBlock.JustTriggered)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.IsCountingDown = builderTimerBlock.IsCountingDown;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.Silent = builderTimerBlock.Silent;
        this.TriggerDelay = (float) (MathHelper.Clamp(builderTimerBlock.Delay, blockDefinition.MinDelay, blockDefinition.MaxDelay) / 1000);
      }
      this.m_countdownMsStart = MathHelper.Clamp(builderTimerBlock.Delay, blockDefinition.MinDelay, blockDefinition.MaxDelay);
      this.m_countdownMsCurrent = MathHelper.Clamp(builderTimerBlock.CurrentTime, 0, blockDefinition.MaxDelay);
      if (this.IsCountingDown)
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.m_beepStart = new MySoundPair(blockDefinition.TimerSoundStart);
      this.m_beepMid = new MySoundPair(blockDefinition.TimerSoundMid);
      this.m_beepEnd = new MySoundPair(blockDefinition.TimerSoundEnd);
      this.m_beepEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_TimerBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_TimerBlock;
      builderCubeBlock.Toolbar = this.Toolbar.GetObjectBuilder();
      builderCubeBlock.JustTriggered = this.NeedsUpdate.HasFlag((Enum) MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
      builderCubeBlock.Delay = this.m_countdownMsStart;
      builderCubeBlock.CurrentTime = this.m_countdownMsCurrent;
      builderCubeBlock.IsCountingDown = this.IsCountingDown;
      builderCubeBlock.Silent = this.Silent;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.IsCountingDown = false;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        for (int index = 0; index < this.Toolbar.ItemCount; ++index)
        {
          this.Toolbar.UpdateItem(index);
          this.Toolbar.ActivateItemAtIndex(index);
        }
        if (this.CubeGrid.Physics != null && MyVisualScriptLogicProvider.TimerBlockTriggered != null)
          MyVisualScriptLogicProvider.TimerBlockTriggered(this.CustomName.ToString(), this.CubeGrid.Name, this.BlockDefinition.Id.TypeId.ToString(), this.BlockDefinition.Id.SubtypeName);
        if (this.CubeGrid.Physics != null && !string.IsNullOrEmpty(this.Name) && MyVisualScriptLogicProvider.TimerBlockTriggeredEntityName != null)
          MyVisualScriptLogicProvider.TimerBlockTriggeredEntityName(this.Name, this.CubeGrid.Name, this.BlockDefinition.Id.TypeId.ToString(), this.BlockDefinition.Id.SubtypeName);
      }
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public void SetTimer(int p)
    {
      this.m_countdownMsStart = p;
      this.RaisePropertiesChanged();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (!this.IsWorking)
        return;
      int num1 = this.m_countdownMsCurrent % 1000;
      if (this.m_countdownMsCurrent > 0)
        this.m_countdownMsCurrent -= 166;
      int num2 = this.m_countdownMsCurrent % 1000;
      if (num1 > 800 && num2 <= 800 || num1 <= 800 && num2 > 800)
        this.UpdateEmissivity();
      if (this.m_countdownMsCurrent <= 0)
      {
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_countdownMsCurrent = 0;
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        if (this.m_beepEmitter != null && !this.Silent)
          this.m_beepEmitter.PlaySound(this.m_beepEnd, true);
      }
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_TimerToTrigger));
      MyValueFormatter.AppendTimeExact(this.m_countdownMsCurrent / 1000, detailedInfo);
    }

    public override void UpdateSoundEmitters()
    {
      base.UpdateSoundEmitters();
      if (this.m_beepEmitter == null)
        return;
      this.m_beepEmitter.Update();
    }

    public void StopCountdown()
    {
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.IsCountingDown = false;
      this.ClearMemory();
    }

    protected void OnTrigger()
    {
      if (!this.IsWorking)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTimerBlock>(this, (Func<MyTimerBlock, Action>) (x => new Action(x.Trigger)));
    }

    [Event(null, 383)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    protected void Trigger()
    {
      if (!this.IsWorking)
        return;
      this.StopCountdown();
      this.UpdateEmissivity();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    void IMyTriggerableBlock.Trigger() => this.OnTrigger();

    public override bool SetEmissiveStateWorking() => this.UpdateEmissivity();

    private bool UpdateEmissivity()
    {
      if (!this.InScene || !this.IsWorking)
        return false;
      if (!this.IsCountingDown)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
      if (this.m_countdownMsCurrent % 1000 <= 800)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]);
      if (this.m_beepEmitter != null && !this.Silent)
        this.m_beepEmitter.PlaySound(this.m_beepMid);
      return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]);
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.IsCountingDown => this.IsCountingDown;

    float SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.TriggerDelay
    {
      get => this.TriggerDelay;
      set => this.TriggerDelay = value;
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.Silent
    {
      get => this.Silent;
      set => this.Silent = value;
    }

    void SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.Trigger() => this.OnTrigger();

    void SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.StartCountdown() => this.StartBtn();

    void SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.StopCountdown() => this.StopBtn();

    [Event(null, 480)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
    {
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (sentItem.EntityID != 0L)
        myToolbarItem = ToolbarItem.ToItem(sentItem);
      this.Toolbar.SetItemAtIndex(index, myToolbarItem);
      this.m_syncing = false;
    }

    protected sealed class Stop\u003C\u003E : ICallSite<MyTimerBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTimerBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.Stop();
      }
    }

    protected sealed class Start\u003C\u003E : ICallSite<MyTimerBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTimerBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.Start();
      }
    }

    protected sealed class Trigger\u003C\u003E : ICallSite<MyTimerBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTimerBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.Trigger();
      }
    }

    protected sealed class SendToolbarItemChanged\u003C\u003ESandbox_Game_Entities_Blocks_ToolbarItem\u0023System_Int32 : ICallSite<MyTimerBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTimerBlock @this,
        in ToolbarItem sentItem,
        in int index,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemChanged(sentItem, index);
      }
    }

    protected class m_silent\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTimerBlock) obj0).m_silent = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_timerSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyTimerBlock) obj0).m_timerSync = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
