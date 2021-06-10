// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MySoundBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SoundBlock))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMySoundBlock), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock)})]
  public class MySoundBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMySoundBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock
  {
    private static StringBuilder m_helperSB = new StringBuilder();
    protected readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_soundRadius;
    protected readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_volume;
    protected readonly VRage.Sync.Sync<string, SyncDirection.BothWays> m_cueIdString;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_loopPeriod;
    protected new MyEntity3DSoundEmitter m_soundEmitter;
    private bool m_willStartSound;
    protected bool m_isPlaying;
    private bool m_isLooping;
    private long m_soundStartTime;
    private string m_playingSoundName;
    private static MyTerminalControlButton<MySoundBlock> m_playButton;
    private static MyTerminalControlButton<MySoundBlock> m_stopButton;
    private static MyTerminalControlSlider<MySoundBlock> m_loopableTimeSlider;

    private MySoundBlockDefinition BlockDefinition => (MySoundBlockDefinition) base.BlockDefinition;

    public float Range
    {
      get => (float) this.m_soundRadius;
      set
      {
        if ((double) (float) this.m_soundRadius == (double) value)
          return;
        this.m_soundRadius.Value = value;
      }
    }

    public float Volume
    {
      get => (float) this.m_volume;
      set
      {
        if ((double) (float) this.m_volume == (double) value)
          return;
        this.m_volume.Value = value;
      }
    }

    public float LoopPeriod
    {
      get => (float) this.m_loopPeriod;
      set => this.m_loopPeriod.Value = value;
    }

    public bool IsLoopablePlaying => this.m_isPlaying && this.m_isLooping;

    public bool IsLoopPeriodUnderThreshold => (double) (float) this.m_loopPeriod < (double) this.BlockDefinition.LoopUpdateThreshold;

    public bool IsSoundSelected => !string.IsNullOrEmpty((string) this.m_cueIdString);

    public MySoundBlock()
    {
      this.CreateTerminalControls();
      this.m_volume.ValueChanged += (Action<SyncBase>) (x => this.VolumeChanged());
      this.m_soundRadius.ValueChanged += (Action<SyncBase>) (x => this.RadiusChanged());
      this.m_cueIdString.ValueChanged += (Action<SyncBase>) (x => this.SelectionChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySoundBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MySoundBlock> slider1 = new MyTerminalControlSlider<MySoundBlock>("VolumeSlider", MySpaceTexts.BlockPropertyTitle_SoundBlockVolume, MySpaceTexts.BlockPropertyDescription_SoundBlockVolume);
      slider1.SetLimits(0.0f, 100f);
      slider1.DefaultValue = new float?(100f);
      slider1.Getter = (MyTerminalValueControl<MySoundBlock, float>.GetterDelegate) (x => x.Volume * 100f);
      slider1.Setter = (MyTerminalValueControl<MySoundBlock, float>.SetterDelegate) ((x, v) => x.Volume = v * 0.01f);
      slider1.Writer = (MyTerminalControl<MySoundBlock>.WriterDelegate) ((x, result) => result.AppendInt32((int) ((double) x.Volume * 100.0)).Append(" %"));
      slider1.EnableActions<MySoundBlock>();
      MyTerminalControlFactory.AddControl<MySoundBlock>((MyTerminalControl<MySoundBlock>) slider1);
      MyTerminalControlSlider<MySoundBlock> slider2 = new MyTerminalControlSlider<MySoundBlock>("RangeSlider", MySpaceTexts.BlockPropertyTitle_SoundBlockRange, MySpaceTexts.BlockPropertyDescription_SoundBlockRange);
      slider2.SetLimits((MyTerminalValueControl<MySoundBlock, float>.GetterDelegate) (x => x.BlockDefinition.MinRange), (MyTerminalValueControl<MySoundBlock, float>.GetterDelegate) (x => x.BlockDefinition.MaxRange));
      slider2.DefaultValue = new float?(50f);
      slider2.Getter = (MyTerminalValueControl<MySoundBlock, float>.GetterDelegate) (x => x.Range);
      slider2.Setter = (MyTerminalValueControl<MySoundBlock, float>.SetterDelegate) ((x, v) => x.Range = v);
      slider2.Writer = (MyTerminalControl<MySoundBlock>.WriterDelegate) ((x, result) => result.AppendInt32((int) x.Range).Append(" m"));
      slider2.EnableActions<MySoundBlock>();
      MyTerminalControlFactory.AddControl<MySoundBlock>((MyTerminalControl<MySoundBlock>) slider2);
      MySoundBlock.m_playButton = new MyTerminalControlButton<MySoundBlock>("PlaySound", MySpaceTexts.BlockPropertyTitle_SoundBlockPlay, MySpaceTexts.Blank, (Action<MySoundBlock>) (x => x.RequestPlaySound()));
      MySoundBlock.m_playButton.Enabled = (Func<MySoundBlock, bool>) (x => x.IsSoundSelected);
      MySoundBlock.m_playButton.EnableAction<MySoundBlock>();
      MyTerminalControlFactory.AddControl<MySoundBlock>((MyTerminalControl<MySoundBlock>) MySoundBlock.m_playButton);
      MySoundBlock.m_stopButton = new MyTerminalControlButton<MySoundBlock>("StopSound", MySpaceTexts.BlockPropertyTitle_SoundBlockStop, MySpaceTexts.Blank, (Action<MySoundBlock>) (x => x.RequestStopSound()));
      MySoundBlock.m_stopButton.Enabled = (Func<MySoundBlock, bool>) (x => x.IsSoundSelected);
      MySoundBlock.m_stopButton.EnableAction<MySoundBlock>();
      MyTerminalControlFactory.AddControl<MySoundBlock>((MyTerminalControl<MySoundBlock>) MySoundBlock.m_stopButton);
      MySoundBlock.m_loopableTimeSlider = new MyTerminalControlSlider<MySoundBlock>("LoopableSlider", MySpaceTexts.BlockPropertyTitle_SoundBlockLoopTime, MySpaceTexts.Blank);
      MySoundBlock.m_loopableTimeSlider.DefaultValue = new float?(1f);
      MySoundBlock.m_loopableTimeSlider.Getter = (MyTerminalValueControl<MySoundBlock, float>.GetterDelegate) (x => x.LoopPeriod);
      MySoundBlock.m_loopableTimeSlider.Setter = (MyTerminalValueControl<MySoundBlock, float>.SetterDelegate) ((x, f) => x.LoopPeriod = f);
      MySoundBlock.m_loopableTimeSlider.Writer = (MyTerminalControl<MySoundBlock>.WriterDelegate) ((x, result) => MyValueFormatter.AppendTimeInBestUnit(x.LoopPeriod, result));
      MySoundBlock.m_loopableTimeSlider.Enabled = (Func<MySoundBlock, bool>) (x => x.IsSelectedSoundLoopable());
      MySoundBlock.m_loopableTimeSlider.Normalizer = (MyTerminalControlSlider<MySoundBlock>.FloatFunc) ((x, f) => x.NormalizeLoopPeriod(f));
      MySoundBlock.m_loopableTimeSlider.Denormalizer = (MyTerminalControlSlider<MySoundBlock>.FloatFunc) ((x, f) => x.DenormalizeLoopPeriod(f));
      MySoundBlock.m_loopableTimeSlider.EnableActions<MySoundBlock>();
      MyTerminalControlFactory.AddControl<MySoundBlock>((MyTerminalControl<MySoundBlock>) MySoundBlock.m_loopableTimeSlider);
      MyTerminalControlFactory.AddControl<MySoundBlock>((MyTerminalControl<MySoundBlock>) new MyTerminalControlListbox<MySoundBlock>("SoundsList", MySpaceTexts.BlockPropertyTitle_SoundBlockSoundList, MySpaceTexts.Blank)
      {
        ListContent = (MyTerminalControlListbox<MySoundBlock>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillListContent(list1, list2)),
        ItemSelected = (MyTerminalControlListbox<MySoundBlock>.SelectItemDelegate) ((x, y) => x.SelectSound(y, true))
      });
    }

    private void SelectionChanged()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MySoundBlock.m_loopableTimeSlider.UpdateVisual();
      MySoundBlock.m_playButton.UpdateVisual();
      MySoundBlock.m_stopButton.UpdateVisual();
    }

    private void RadiusChanged()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.CustomMaxDistance = new float?((float) this.m_soundRadius);
    }

    private void VolumeChanged()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.CustomVolume = new float?((float) this.m_volume);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MySoundBlockDefinition blockDefinition = this.BlockDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 0.0002f, new Func<float>(this.UpdateRequiredPowerInput));
      resourceSinkComponent.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, dopplerScaler: 0.0f);
        this.m_soundEmitter.Force3D = true;
      }
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_SoundBlock builderSoundBlock = (MyObjectBuilder_SoundBlock) objectBuilder;
      this.m_isPlaying = builderSoundBlock.IsPlaying;
      this.m_isLooping = builderSoundBlock.IsLoopableSound;
      this.m_volume.SetLocalValue(MathHelper.Clamp(builderSoundBlock.Volume, 0.0f, 1f));
      this.m_soundRadius.SetLocalValue(MathHelper.Clamp(builderSoundBlock.Range, blockDefinition.MinRange, blockDefinition.MaxRange));
      this.m_loopPeriod.SetLocalValue(MathHelper.Clamp(builderSoundBlock.LoopPeriod, 0.0f, blockDefinition.MaxLoopPeriod));
      if (builderSoundBlock.IsPlaying)
      {
        this.m_willStartSound = true;
        this.m_playingSoundName = builderSoundBlock.CueName;
        this.m_soundStartTime = Stopwatch.GetTimestamp() - (long) builderSoundBlock.ElapsedSoundSeconds * Stopwatch.Frequency;
      }
      this.InitCue(builderSoundBlock.CueName);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    private void InitCue(string cueName)
    {
      if (string.IsNullOrEmpty(cueName))
        this.m_cueIdString.SetLocalValue("");
      else if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.m_cueIdString.SetLocalValue(cueName);
      }
      else
      {
        MySoundPair mySoundPair = new MySoundPair(cueName);
        MySoundCategoryDefinition.SoundDescription soundDescription = (MySoundCategoryDefinition.SoundDescription) null;
        foreach (MySoundCategoryDefinition categoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
        {
          foreach (MySoundCategoryDefinition.SoundDescription sound in categoryDefinition.Sounds)
          {
            if (mySoundPair.SoundId.ToString().EndsWith(sound.SoundId.ToString()))
              soundDescription = sound;
          }
        }
        if (soundDescription != null)
          this.m_cueIdString.SetLocalValue(soundDescription.SoundId);
        else
          this.m_cueIdString.SetLocalValue("");
      }
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SoundBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_SoundBlock;
      builderCubeBlock.Volume = this.Volume;
      builderCubeBlock.Range = this.Range;
      builderCubeBlock.LoopPeriod = this.LoopPeriod;
      builderCubeBlock.IsLoopableSound = this.m_isLooping;
      builderCubeBlock.ElapsedSoundSeconds = (float) (Stopwatch.GetTimestamp() - this.m_soundStartTime) / (float) Stopwatch.Frequency;
      if (this.m_isPlaying && (this.m_soundEmitter != null && this.m_soundEmitter.IsPlaying || this.m_isLooping && (double) this.LoopPeriod > (double) builderCubeBlock.ElapsedSoundSeconds))
      {
        builderCubeBlock.IsPlaying = true;
        builderCubeBlock.CueName = this.m_playingSoundName;
      }
      else
      {
        builderCubeBlock.IsPlaying = false;
        builderCubeBlock.ElapsedSoundSeconds = 0.0f;
        builderCubeBlock.CueName = this.m_cueIdString.Value;
      }
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void RequestPlaySound() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySoundBlock, bool>(this, (Func<MySoundBlock, Action<bool>>) (x => new Action<bool>(x.PlaySound)), this.IsSelectedSoundLoopable());

    [Event(null, 322)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void PlaySound(bool isLoopable) => this.PlaySoundInternal(isLoopable);

    protected void PlaySoundInternal(bool isLoopable)
    {
      if (!this.Enabled || !this.IsWorking)
        return;
      string cueName = this.m_cueIdString.Value;
      if (string.IsNullOrEmpty(cueName))
        return;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MySoundPair cueId = new MySoundPair(cueName);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.RequestStopSound();
        this.StopSound();
        if (isLoopable)
          this.PlayLoopableSound(cueId);
        else
          this.PlaySingleSound(cueId);
      }
      this.m_isPlaying = true;
      this.m_isLooping = isLoopable;
      this.m_playingSoundName = cueName;
      this.m_soundStartTime = Stopwatch.GetTimestamp();
    }

    private void PlayLoopableSound(MySoundPair cueId)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySound(cueId, true);
    }

    protected void PlaySingleSound(MySoundPair cueId)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySingleSound(cueId, true);
    }

    protected virtual void SelectSound(List<MyGuiControlListbox.Item> cuesId, bool sync) => this.SelectSound(cuesId[0].UserData.ToString(), sync);

    public void SelectSound(string cueId, bool sync) => this.m_cueIdString.Value = cueId;

    public void RequestStopSound()
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySoundBlock>(this, (Func<MySoundBlock, Action>) (x => new Action(x.StopSound)));
      this.StopSoundInternal(true);
      this.m_willStartSound = false;
    }

    [Event(null, 400)]
    [Reliable]
    [Server]
    [BroadcastExcept]
    public void StopSound() => this.StopSoundInternal(true);

    protected void StopSoundInternal(bool force = false)
    {
      this.m_isPlaying = false;
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(force);
      if (!this.HasDamageEffect)
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      this.DetailedInfo.Clear();
      this.RaisePropertiesChanged();
    }

    protected virtual void FillListContent(
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
    {
      foreach (MySoundCategoryDefinition categoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
      {
        if (categoryDefinition.Public)
        {
          foreach (MySoundCategoryDefinition.SoundDescription sound in categoryDefinition.Sounds)
          {
            MySoundBlock.m_helperSB.Clear().Append(sound.SoundText);
            MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(MySoundBlock.m_helperSB, userData: ((object) sound.SoundId));
            listBoxContent.Add(obj);
            if (sound.SoundId.Equals((string) this.m_cueIdString))
              listBoxSelectedItems.Add(obj);
          }
        }
      }
    }

    private float NormalizeLoopPeriod(float value) => (double) value <= 1.0 ? 0.0f : MathHelper.InterpLogInv(value, 1f, this.BlockDefinition.MaxLoopPeriod);

    private float DenormalizeLoopPeriod(float value) => (double) value == 0.0 ? 1f : MathHelper.InterpLog(value, 1f, this.BlockDefinition.MaxLoopPeriod);

    public override void UpdateBeforeSimulation()
    {
      if (!this.IsWorking)
        return;
      base.UpdateBeforeSimulation();
      if (!this.IsLoopablePlaying)
        return;
      this.UpdateLoopableSoundEmitter();
    }

    public override void UpdateSoundEmitters()
    {
      if (!this.IsWorking || this.m_soundEmitter == null)
        return;
      if (MyAudio.Static.CanPlay && !Sandbox.Engine.Platform.Game.IsDedicated && (this.m_isPlaying && !this.m_soundEmitter.IsPlaying) && this.m_isLooping)
        this.RequestPlaySound();
      this.m_soundEmitter.Update();
    }

    private void UpdateLoopableSoundEmitter()
    {
      double num = (double) (Stopwatch.GetTimestamp() - this.m_soundStartTime) / (double) Stopwatch.Frequency;
      if (num > (double) (float) this.m_loopPeriod)
      {
        this.StopSoundInternal(true);
      }
      else
      {
        this.DetailedInfo.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_LoopTimer));
        MyValueFormatter.AppendTimeInBestUnit(Math.Max(0.0f, (float) this.m_loopPeriod - (float) num), this.DetailedInfo);
        this.RaisePropertiesChanged();
      }
    }

    private void PowerReceiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private float UpdateRequiredPowerInput() => this.Enabled && this.IsFunctional ? 0.0002f : 0.0f;

    protected override void Closing()
    {
      base.Closing();
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.m_soundEmitter?.Sound?.Resume();
      if (Sandbox.Engine.Platform.Game.IsDedicated || !this.m_willStartSound || this.CubeGrid.Physics == null)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.RequestStopSound();
      else
        this.StopSound();
      MySoundPair cueId = new MySoundPair(this.m_playingSoundName);
      if (this.m_isLooping)
        this.PlayLoopableSound(cueId);
      else
        this.PlaySingleSound(cueId);
      this.m_willStartSound = false;
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.m_soundEmitter?.Sound?.Pause();
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.ResourceSink.Update();
    }

    protected bool IsSelectedSoundLoopable() => MySoundBlock.IsSoundLoopable(this.GetSoundQueue(this.m_cueIdString.Value));

    private static bool IsSoundLoopable(MyCueId cueId)
    {
      MySoundData cue = MyAudio.Static.GetCue(cueId);
      return cue != null && cue.Loopable;
    }

    private MyCueId GetSoundQueue(string nameOrId)
    {
      MyCueId cueId = MySoundPair.GetCueId(nameOrId);
      if (cueId.IsNull)
      {
        foreach (MySoundCategoryDefinition categoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
        {
          foreach (MySoundCategoryDefinition.SoundDescription sound in categoryDefinition.Sounds)
          {
            if (nameOrId == sound.SoundText)
              return MySoundPair.GetCueId(sound.SoundId);
          }
        }
      }
      return cueId;
    }

    float SpaceEngineers.Game.ModAPI.IMySoundBlock.Volume
    {
      get => this.Volume;
      set => this.Volume = value;
    }

    float SpaceEngineers.Game.ModAPI.IMySoundBlock.Range
    {
      get => this.Range;
      set => this.Range = value;
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Volume
    {
      get => this.Volume;
      set => this.Volume = MathHelper.Clamp(value, 0.0f, 1f);
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Range
    {
      get => this.Range;
      set => this.Range = MathHelper.Clamp(value, this.BlockDefinition.MinRange, this.BlockDefinition.MaxRange);
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.IsSoundSelected => this.IsSoundSelected;

    float SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.LoopPeriod
    {
      get => this.LoopPeriod;
      set => this.LoopPeriod = value;
    }

    string SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.SelectedSound
    {
      get => (string) this.m_cueIdString;
      set => this.SelectSound(this.GetSoundQueue(value).ToString(), true);
    }

    void SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Play() => this.RequestPlaySound();

    void SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Stop() => this.RequestStopSound();

    void SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.GetSounds(List<string> list)
    {
      list.Clear();
      foreach (MySoundCategoryDefinition categoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
      {
        foreach (MySoundCategoryDefinition.SoundDescription sound in categoryDefinition.Sounds)
          list.Add(sound.SoundId);
      }
    }

    protected sealed class PlaySound\u003C\u003ESystem_Boolean : ICallSite<MySoundBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySoundBlock @this,
        in bool isLoopable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PlaySound(isLoopable);
      }
    }

    protected sealed class StopSound\u003C\u003E : ICallSite<MySoundBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySoundBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.StopSound();
      }
    }

    protected class m_soundRadius\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MySoundBlock) obj0).m_soundRadius = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_volume\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MySoundBlock) obj0).m_volume = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_cueIdString\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<string, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<string, SyncDirection.BothWays>(obj1, obj2));
        ((MySoundBlock) obj0).m_cueIdString = (VRage.Sync.Sync<string, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_loopPeriod\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MySoundBlock) obj0).m_loopPeriod = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
