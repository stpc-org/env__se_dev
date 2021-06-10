// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyShipSoundComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentBuilder(typeof (MyObjectBuilder_ShipSoundComponent), true)]
  public class MyShipSoundComponent : MyEntityComponentBase
  {
    private static Dictionary<MyDefinitionId, MyShipSoundsDefinition> m_categories = new Dictionary<MyDefinitionId, MyShipSoundsDefinition>();
    private static MyShipSoundSystemDefinition m_definition = new MyShipSoundSystemDefinition();
    private bool m_initialized;
    private bool m_shouldPlay2D;
    private bool m_shouldPlay2DChanged;
    private bool m_insideShip;
    private float m_distanceToShip = float.MaxValue;
    public bool ShipHasChanged = true;
    private MyEntity m_shipSoundSource;
    private MyCubeGrid m_shipGrid;
    private MyEntityThrustComponent m_shipThrusters;
    private MyGridWheelSystem m_shipWheels;
    private bool m_isDebris = true;
    private MyDefinitionId m_shipCategory;
    private MyShipSoundsDefinition m_groupData;
    private bool m_categoryChange;
    private bool m_forceSoundCheck;
    private float m_wheelVolumeModifierEngine;
    private float m_wheelVolumeModifierWheels;
    private HashSet<MySlimBlock> m_detectedBlocks = new HashSet<MySlimBlock>();
    private MyShipSoundComponent.ShipStateEnum m_shipState;
    private float m_shipEngineModifier;
    private float m_singleSoundsModifier = 1f;
    private bool m_playingSpeedUpOrDown;
    private MyEntity3DSoundEmitter[] m_emitters = new MyEntity3DSoundEmitter[Enum.GetNames(typeof (MyShipSoundComponent.ShipEmitters)).Length];
    private float[] m_thrusterVolumes;
    private float[] m_thrusterVolumeTargets;
    private bool m_singleThrusterTypeShip;
    private static MyStringHash m_thrusterIon = MyStringHash.GetOrCompute("Ion");
    private static MyStringHash m_thrusterHydrogen = MyStringHash.GetOrCompute("Hydrogen");
    private static MyStringHash m_thrusterAtmospheric = MyStringHash.GetOrCompute("Atmospheric");
    private static MyStringHash m_crossfade = MyStringHash.GetOrCompute("CrossFade");
    private static MyStringHash m_fadeOut = MyStringHash.GetOrCompute("FadeOut");
    private float[] m_timers = new float[Enum.GetNames(typeof (MyShipSoundComponent.ShipTimers)).Length];
    private float m_lastFrameShipSpeed;
    private int m_speedChange = 15;
    private float m_shipCurrentPower;
    private float m_shipCurrentPowerTarget;
    private const float POWER_CHANGE_SPEED_UP = 0.006666667f;
    private const float POWER_CHANGE_SPEED_DOWN = 0.01f;
    private bool m_lastWheelUpdateStart;
    private bool m_lastWheelUpdateStop;
    private DateTime m_lastContactWithGround = DateTime.UtcNow;
    private bool m_shipWheelsAction;
    private bool m_scheduled;

    public static void ClearShipSounds() => MyShipSoundComponent.m_categories.Clear();

    public static void SetDefinition(MyShipSoundSystemDefinition def) => MyShipSoundComponent.m_definition = def;

    public static void AddShipSounds(MyShipSoundsDefinition shipSoundGroup)
    {
      if (MyShipSoundComponent.m_categories.ContainsKey(shipSoundGroup.Id))
        MyShipSoundComponent.m_categories.Remove(shipSoundGroup.Id);
      MyShipSoundComponent.m_categories.Add(shipSoundGroup.Id, shipSoundGroup);
    }

    public static void ActualizeGroups()
    {
      foreach (MyShipSoundsDefinition soundsDefinition in MyShipSoundComponent.m_categories.Values)
        soundsDefinition.WheelsSpeedCompensation = MyShipSoundComponent.m_definition.FullSpeed / soundsDefinition.WheelsFullSpeed;
    }

    public MyShipSoundComponent()
    {
      for (int index = 0; index < this.m_emitters.Length; ++index)
        this.m_emitters[index] = (MyEntity3DSoundEmitter) null;
      for (int index = 0; index < this.m_timers.Length; ++index)
        this.m_timers[index] = 0.0f;
    }

    public bool InitComponent(MyCubeGrid shipGrid)
    {
      if (shipGrid.GridSizeEnum == MyCubeSize.Small && !MyFakes.ENABLE_NEW_SMALL_SHIP_SOUNDS || shipGrid.GridSizeEnum == MyCubeSize.Large && !MyFakes.ENABLE_NEW_LARGE_SHIP_SOUNDS || MyShipSoundComponent.m_definition == null)
        return false;
      this.m_shipGrid = shipGrid;
      this.m_shipThrusters = this.m_shipGrid.Components.Get<MyEntityThrustComponent>();
      this.m_shipWheels = this.m_shipGrid.GridSystems.WheelSystem;
      this.m_thrusterVolumes = new float[Enum.GetNames(typeof (MyShipSoundComponent.ShipThrusters)).Length];
      this.m_thrusterVolumeTargets = new float[Enum.GetNames(typeof (MyShipSoundComponent.ShipThrusters)).Length];
      for (int index = 1; index < this.m_thrusterVolumes.Length; ++index)
      {
        this.m_thrusterVolumes[index] = 0.0f;
        this.m_thrusterVolumeTargets[index] = 0.0f;
      }
      this.m_thrusterVolumes[0] = 1f;
      this.m_thrusterVolumeTargets[0] = 1f;
      for (int index = 0; index < this.m_emitters.Length; ++index)
      {
        this.m_emitters[index] = new MyEntity3DSoundEmitter((MyEntity) this.m_shipGrid, true);
        this.m_emitters[index].Force2D = this.m_shouldPlay2D;
        this.m_emitters[index].Force3D = !this.m_shouldPlay2D;
      }
      this.m_initialized = true;
      shipGrid.OnPhysicsChanged += new Action<MyEntity>(this.ShipGridOnOnPhysicsChanged);
      this.Container.ComponentAdded += new Action<System.Type, MyEntityComponentBase>(this.ContainerOnComponentAdded);
      this.Container.ComponentRemoved += new Action<System.Type, MyEntityComponentBase>(this.ContainerOnComponentRemoved);
      shipGrid.OnBlockRemoved += new Action<MySlimBlock>(this.ShipGridOnOnBlockRemoved);
      shipGrid.OnBlockAdded += new Action<MySlimBlock>(this.ShipGridOnOnBlockAdded);
      if (!this.m_shipGrid.IsStatic && (this.m_shipWheels.WheelCount > 0 || this.m_shipThrusters != null))
        this.Schedule();
      return true;
    }

    private void ShipGridOnOnPhysicsChanged(MyEntity obj)
    {
      if (this.m_shipGrid.IsStatic)
      {
        this.DeSchedule();
      }
      else
      {
        if (this.m_shipWheels.WheelCount <= 0 && this.m_shipThrusters == null)
          return;
        this.Schedule();
      }
    }

    private void ShipGridOnOnBlockAdded(MySlimBlock block)
    {
      if (this.m_shipGrid.IsStatic || this.m_shipWheels.WheelCount <= 0)
        return;
      this.Schedule();
    }

    private void ShipGridOnOnBlockRemoved(MySlimBlock block)
    {
      MyEntityThrustComponent shipThrusters = this.m_shipThrusters;
      if ((shipThrusters != null ? (shipThrusters.ThrustCount > 0 ? 1 : 0) : 0) != 0 || this.m_shipWheels.WheelCount != 0)
        return;
      this.DeSchedule();
    }

    private void ContainerOnComponentRemoved(System.Type type, MyEntityComponentBase component)
    {
      if (this.m_shipWheels.WheelCount != 0 || !(component is MyThrusterBlockThrustComponent))
        return;
      this.DeSchedule();
      this.m_shipThrusters = (MyEntityThrustComponent) null;
    }

    private void ContainerOnComponentAdded(System.Type type, MyEntityComponentBase component)
    {
      if (this.m_shipGrid.IsStatic || !(component is MyThrusterBlockThrustComponent blockThrustComponent))
        return;
      this.Schedule();
      this.m_shipThrusters = (MyEntityThrustComponent) blockThrustComponent;
    }

    protected void Schedule()
    {
      if (this.m_scheduled)
        return;
      this.m_shipGrid.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.Update), 13);
      this.m_scheduled = true;
    }

    protected void DeSchedule()
    {
      if (!this.m_scheduled)
        return;
      this.m_shipGrid.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.Update));
      this.m_scheduled = false;
    }

    public void Update()
    {
      if (!this.m_initialized || this.m_shipGrid.Physics == null || this.m_shipGrid.IsStatic || (this.m_shipThrusters == null && this.m_shipWheels == null || this.m_groupData == null))
        return;
      this.UpdateLastGroundContact();
      float shipSpeed;
      float originalSpeed;
      MyShipSoundComponent.ShipStateEnum lastState;
      bool driving = this.UpdateState(out shipSpeed, out originalSpeed, out lastState);
      this.UpdateSpeedBasedShipSound(driving);
      this.UpdateShouldPlay2D();
      this.CorrectThrusterVolumes();
      if (driving)
      {
        this.m_wheelVolumeModifierEngine = Math.Min(this.m_wheelVolumeModifierEngine + 0.01f, 1f);
        this.m_wheelVolumeModifierWheels = Math.Min(this.m_wheelVolumeModifierWheels + 0.03f, 1f);
      }
      else
      {
        this.m_wheelVolumeModifierEngine = Math.Max(this.m_wheelVolumeModifierEngine - 0.005f, 0.0f);
        this.m_wheelVolumeModifierWheels = Math.Max(this.m_wheelVolumeModifierWheels - 0.03f, 0.0f);
      }
      if (this.m_shipState != lastState || this.m_categoryChange || this.m_forceSoundCheck)
        this.PlaySoundOnChange(lastState);
      if (this.m_shouldPlay2DChanged)
        this.UpdateSoundDimension();
      if (this.m_shipState != MyShipSoundComponent.ShipStateEnum.NoPower)
        this.UpdateVolumes(shipSpeed, originalSpeed);
      else if ((double) this.m_shipEngineModifier > 0.0)
        this.m_shipEngineModifier = Math.Max(0.0f, this.m_shipEngineModifier - 0.01666667f / this.m_groupData.EngineTimeToTurnOff);
      if (this.m_shipThrusters != null && this.m_shipThrusters.ThrustCount <= 0)
        this.m_shipThrusters = (MyEntityThrustComponent) null;
      if ((double) Math.Abs(shipSpeed - this.m_lastFrameShipSpeed) > 0.00999999977648258 && (double) shipSpeed >= 3.0)
        this.m_speedChange = (int) MyMath.Clamp((float) (this.m_speedChange + ((double) shipSpeed > (double) this.m_lastFrameShipSpeed ? 1 : -1)), 0.0f, 30f);
      else if (this.m_speedChange != 15)
        this.m_speedChange += this.m_speedChange > 15 ? -1 : 1;
      if ((double) shipSpeed >= (double) this.m_lastFrameShipSpeed && (double) this.m_timers[1] > 0.0)
        this.m_timers[1] -= 0.01666667f;
      if ((double) shipSpeed <= (double) this.m_lastFrameShipSpeed && (double) this.m_timers[0] > 0.0)
        this.m_timers[0] -= 0.01666667f;
      this.m_lastFrameShipSpeed = shipSpeed;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UpdateVolumes(float shipSpeed, float originalSpeed)
    {
      if ((double) this.m_shipEngineModifier < 1.0)
        this.m_shipEngineModifier = Math.Min(1f, this.m_shipEngineModifier + 0.01666667f / this.m_groupData.EngineTimeToTurnOn);
      float speedRatio = Math.Min(shipSpeed / MyShipSoundComponent.m_definition.FullSpeed, 1f);
      float num1 = this.CalculateVolumeFromSpeed(speedRatio, ref this.m_groupData.EngineVolumes) * this.m_shipEngineModifier * this.m_singleSoundsModifier;
      if (this.m_emitters[0].IsPlaying)
      {
        this.m_emitters[0].VolumeMultiplier = num1;
        this.m_emitters[0].Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(this.m_groupData.EnginePitchRangeInSemitones_h + this.m_groupData.EnginePitchRangeInSemitones * speedRatio);
      }
      float num2 = Math.Max(Math.Min(this.CalculateVolumeFromSpeed(speedRatio, ref this.m_groupData.ThrusterVolumes), 1f) - this.m_wheelVolumeModifierEngine * this.m_groupData.WheelsLowerThrusterVolumeBy, 0.0f);
      float num3 = MyMath.Clamp((float) (1.20000004768372 - (double) num2 * 3.0), 0.0f, 1f) * this.m_shipEngineModifier * this.m_singleSoundsModifier;
      float num4 = num2 * (this.m_shipEngineModifier * this.m_singleSoundsModifier);
      this.m_emitters[11].VolumeMultiplier = MySandboxGame.Config.ShipSoundsAreBasedOnSpeed ? Math.Max(0.0f, num1 - num3) : originalSpeed;
      this.m_emitters[10].VolumeMultiplier = (MySandboxGame.Config.ShipSoundsAreBasedOnSpeed ? num3 : MyMath.Clamp((float) (1.20000004768372 - (double) originalSpeed * 3.0), 0.0f, 1f)) * this.m_shipEngineModifier * this.m_singleSoundsModifier;
      float frequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(this.m_groupData.ThrusterPitchRangeInSemitones_h + this.m_groupData.ThrusterPitchRangeInSemitones * num4);
      if (this.m_emitters[2].IsPlaying)
      {
        float thrusterVolume = this.m_thrusterVolumes[0];
        this.m_emitters[2].VolumeMultiplier = num4 * thrusterVolume;
        this.m_emitters[2].Sound.FrequencyRatio = frequencyRatio;
        MySoundPair shipSound = this.GetShipSound(ShipSystemSoundsEnum.IonThrusterPush);
        MyEntity3DSoundEmitter emitter = this.m_emitters[12];
        this.PlayThrusterPushSound(shipSpeed, thrusterVolume, shipSound, emitter);
      }
      if (this.m_emitters[5].IsPlaying)
        this.m_emitters[5].VolumeMultiplier = num3 * this.m_thrusterVolumes[0];
      if (this.m_emitters[3].IsPlaying)
      {
        float thrusterVolume = this.m_thrusterVolumes[1];
        this.m_emitters[3].VolumeMultiplier = num4 * thrusterVolume;
        this.m_emitters[3].Sound.FrequencyRatio = frequencyRatio;
        MySoundPair shipSound = this.GetShipSound(ShipSystemSoundsEnum.HydrogenThrusterPush);
        MyEntity3DSoundEmitter emitter = this.m_emitters[13];
        this.PlayThrusterPushSound(shipSpeed, thrusterVolume, shipSound, emitter);
      }
      if (this.m_emitters[6].IsPlaying)
        this.m_emitters[6].VolumeMultiplier = num3 * this.m_thrusterVolumes[1];
      if (this.m_emitters[4].IsPlaying)
      {
        this.m_emitters[4].VolumeMultiplier = num4 * this.m_thrusterVolumes[2];
        this.m_emitters[4].Sound.FrequencyRatio = frequencyRatio;
      }
      if (this.m_emitters[7].IsPlaying)
        this.m_emitters[7].VolumeMultiplier = num3 * this.m_thrusterVolumes[2];
      if (this.m_emitters[8].IsPlaying)
      {
        this.m_emitters[0].VolumeMultiplier = Math.Max(num1 - this.m_wheelVolumeModifierEngine * this.m_groupData.WheelsLowerThrusterVolumeBy, 0.0f);
        this.m_emitters[8].VolumeMultiplier = num4 * this.m_wheelVolumeModifierEngine * this.m_singleSoundsModifier;
        this.m_emitters[8].Sound.FrequencyRatio = frequencyRatio;
        this.m_emitters[9].VolumeMultiplier = this.CalculateVolumeFromSpeed(speedRatio, ref this.m_groupData.WheelsVolumes) * this.m_shipEngineModifier * this.m_wheelVolumeModifierWheels * this.m_singleSoundsModifier;
      }
      float num5 = (float) (0.5 + (double) num4 / 2.0);
      this.m_playingSpeedUpOrDown = this.m_playingSpeedUpOrDown && this.m_emitters[1].IsPlaying;
      if (this.m_speedChange >= 20 && (double) this.m_timers[0] <= 0.0 && (double) this.m_wheelVolumeModifierEngine <= 0.0)
      {
        this.m_timers[0] = this.m_shipGrid.GridSizeEnum == MyCubeSize.Large ? 8f : 1f;
        if (this.m_emitters[1].IsPlaying && this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)))
          this.FadeOutSound(duration: 1000);
        this.m_emitters[1].VolumeMultiplier = num5;
        this.PlayShipSound(MyShipSoundComponent.ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesSpeedUp, false, false);
        this.m_playingSpeedUpOrDown = true;
      }
      else if (this.m_speedChange <= 15 && this.m_emitters[1].IsPlaying && this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
        this.FadeOutSound(duration: 1000);
      if (this.m_speedChange <= 10 && (double) this.m_timers[1] <= 0.0 && (double) this.m_wheelVolumeModifierEngine <= 0.0)
      {
        this.m_timers[1] = this.m_shipGrid.GridSizeEnum == MyCubeSize.Large ? 8f : 2f;
        if (this.m_emitters[1].IsPlaying && this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
          this.FadeOutSound(duration: 1000);
        this.m_emitters[1].VolumeMultiplier = num5;
        this.PlayShipSound(MyShipSoundComponent.ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesSpeedDown, false, false);
        this.m_playingSpeedUpOrDown = true;
      }
      else if (this.m_speedChange >= 15 && this.m_emitters[1].IsPlaying && this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)))
        this.FadeOutSound(duration: 1000);
      float val2 = 1f;
      if (this.m_playingSpeedUpOrDown && this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)))
        val2 = this.m_groupData.SpeedDownSoundChangeVolumeTo;
      if (this.m_playingSpeedUpOrDown && this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
        val2 = this.m_groupData.SpeedUpSoundChangeVolumeTo;
      if ((double) this.m_singleSoundsModifier < (double) val2)
        this.m_singleSoundsModifier = Math.Min(this.m_singleSoundsModifier + this.m_groupData.SpeedUpDownChangeSpeed, val2);
      else if ((double) this.m_singleSoundsModifier > (double) val2)
        this.m_singleSoundsModifier = Math.Max(this.m_singleSoundsModifier - this.m_groupData.SpeedUpDownChangeSpeed, val2);
      if (!this.m_emitters[1].IsPlaying || !this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedDown)) && !this.m_emitters[1].SoundPair.Equals((object) this.GetShipSound(ShipSystemSoundsEnum.EnginesSpeedUp)))
        return;
      this.m_emitters[1].VolumeMultiplier = num5;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UpdateSoundDimension()
    {
      for (int index = 0; index < this.m_emitters.Length; ++index)
      {
        this.m_emitters[index].Force2D = this.m_shouldPlay2D;
        this.m_emitters[index].Force3D = !this.m_shouldPlay2D;
        if (this.m_emitters[index].IsPlaying && this.m_emitters[index].Plays2D != this.m_shouldPlay2D && this.m_emitters[index].Loop)
        {
          this.m_emitters[index].StopSound(true);
          this.m_emitters[index].PlaySound(this.m_emitters[index].SoundPair, true, true, this.m_shouldPlay2D);
        }
      }
      this.m_shouldPlay2DChanged = false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void PlaySoundOnChange(MyShipSoundComponent.ShipStateEnum lastState)
    {
      if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.NoPower)
      {
        if (this.m_shipState != lastState)
        {
          for (int index = 0; index < this.m_emitters.Length; ++index)
            this.m_emitters[index].StopSound(false);
          this.m_emitters[1].VolumeMultiplier = 1f;
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesEnd);
        }
      }
      else
      {
        if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.Slow)
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopSlow);
        else if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.Medium)
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopMedium);
        else if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.Fast)
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.MainSound, ShipSystemSoundsEnum.MainLoopFast);
        this.PlayShipSound(MyShipSoundComponent.ShipEmitters.ShipEngine, ShipSystemSoundsEnum.ShipEngine);
        this.PlayShipSound(MyShipSoundComponent.ShipEmitters.ShipIdle, ShipSystemSoundsEnum.ShipIdle);
        if ((double) this.m_thrusterVolumes[0] > 0.0)
        {
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.IonThrusters, ShipSystemSoundsEnum.IonThrusters);
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.IonThrustersIdle, ShipSystemSoundsEnum.IonThrustersIdle);
        }
        if ((double) this.m_thrusterVolumes[1] > 0.0)
        {
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.HydrogenThrusters, ShipSystemSoundsEnum.HydrogenThrusters);
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.HydrogenThrustersIdle, ShipSystemSoundsEnum.HydrogenThrustersIdle);
        }
        if ((double) this.m_thrusterVolumes[2] > 0.0)
        {
          if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.Slow)
            this.PlayShipSound(MyShipSoundComponent.ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersSlow);
          else if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.Medium)
            this.PlayShipSound(MyShipSoundComponent.ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersMedium);
          else if (this.m_shipState == MyShipSoundComponent.ShipStateEnum.Fast)
            this.PlayShipSound(MyShipSoundComponent.ShipEmitters.AtmosphericThrusters, ShipSystemSoundsEnum.AtmoThrustersFast);
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.AtmosphericThrustersIdle, ShipSystemSoundsEnum.AtmoThrustersIdle);
        }
        if (this.m_shipWheels.WheelCount > 0)
        {
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.WheelsMain, ShipSystemSoundsEnum.WheelsEngineRun);
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.WheelsSecondary, ShipSystemSoundsEnum.WheelsSecondary);
        }
        if (lastState == MyShipSoundComponent.ShipStateEnum.NoPower)
        {
          this.m_emitters[1].VolumeMultiplier = 1f;
          this.PlayShipSound(MyShipSoundComponent.ShipEmitters.SingleSounds, ShipSystemSoundsEnum.EnginesStart);
        }
      }
      this.m_categoryChange = false;
      this.m_forceSoundCheck = false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool UpdateState(
      out float shipSpeed,
      out float originalSpeed,
      out MyShipSoundComponent.ShipStateEnum lastState)
    {
      bool flag = (DateTime.UtcNow - this.m_lastContactWithGround).TotalSeconds <= 0.200000002980232;
      shipSpeed = !flag ? this.m_shipGrid.Physics.LinearVelocity.Length() : (this.m_shipGrid.Physics.LinearVelocity * this.m_groupData.WheelsSpeedCompensation).Length();
      originalSpeed = Math.Min(shipSpeed / MyShipSoundComponent.m_definition.FullSpeed, 1f);
      if (!MySandboxGame.Config.ShipSoundsAreBasedOnSpeed)
        shipSpeed = this.m_shipCurrentPower * MyShipSoundComponent.m_definition.FullSpeed;
      lastState = this.m_shipState;
      this.m_shipState = this.m_shipGrid.GridSystems.ResourceDistributor.ResourceState == MyResourceStateEnum.NoPower || this.m_isDebris || (this.m_shipThrusters == null || this.m_shipThrusters.ThrustCount <= 0) && (this.m_shipWheels == null || this.m_shipWheels.WheelCount <= 0) ? MyShipSoundComponent.ShipStateEnum.NoPower : ((double) shipSpeed >= (double) MyShipSoundComponent.m_definition.SpeedThreshold1 ? ((double) shipSpeed >= (double) MyShipSoundComponent.m_definition.SpeedThreshold2 ? MyShipSoundComponent.ShipStateEnum.Fast : MyShipSoundComponent.ShipStateEnum.Medium) : MyShipSoundComponent.ShipStateEnum.Slow);
      return flag;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UpdateLastGroundContact()
    {
      if (this.m_shipWheels == null)
        return;
      foreach (MyMechanicalConnectionBlockBase wheel in this.m_shipWheels.Wheels)
      {
        if (wheel.TopBlock is MyWheel topBlock && topBlock.LastContactTime > this.m_lastContactWithGround)
          this.m_lastContactWithGround = topBlock.LastContactTime;
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UpdateSpeedBasedShipSound(bool driving)
    {
      if (MySandboxGame.Config.ShipSoundsAreBasedOnSpeed)
        return;
      this.m_shipCurrentPowerTarget = 0.0f;
      if (driving)
      {
        if (this.m_shipWheels != null && this.m_shipWheels.WheelCount > 0)
        {
          if ((double) Math.Abs(this.m_shipWheels.AngularVelocity.Z) >= 0.899999976158142)
            this.m_shipCurrentPowerTarget = 1f;
          else if ((double) this.m_shipGrid.Physics.LinearVelocity.LengthSquared() > 5.0)
            this.m_shipCurrentPowerTarget = 0.33f;
        }
      }
      else if (this.m_shipThrusters != null)
        this.m_shipCurrentPowerTarget = (double) this.m_shipThrusters.FinalThrust.LengthSquared() < 100.0 ? (!(this.m_shipGrid.Physics.Gravity != Vector3.Zero) || !this.m_shipThrusters.DampenersEnabled || (double) this.m_shipGrid.Physics.LinearVelocity.LengthSquared() >= 4.0 ? 0.0f : 0.33f) : 1f;
      if ((double) this.m_shipCurrentPower < (double) this.m_shipCurrentPowerTarget)
      {
        this.m_shipCurrentPower = Math.Min(this.m_shipCurrentPower + 0.006666667f, this.m_shipCurrentPowerTarget);
      }
      else
      {
        if ((double) this.m_shipCurrentPower <= (double) this.m_shipCurrentPowerTarget)
          return;
        this.m_shipCurrentPower = Math.Max(this.m_shipCurrentPower - 0.01f, this.m_shipCurrentPowerTarget);
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void CorrectThrusterVolumes()
    {
      for (int index = 0; index < this.m_thrusterVolumes.Length; ++index)
      {
        if ((double) this.m_thrusterVolumes[index] < (double) this.m_thrusterVolumeTargets[index])
          this.m_thrusterVolumes[index] = Math.Min(this.m_thrusterVolumes[index] + this.m_groupData.ThrusterCompositionChangeSpeed, this.m_thrusterVolumeTargets[index]);
        else if ((double) this.m_thrusterVolumes[index] > (double) this.m_thrusterVolumeTargets[index])
          this.m_thrusterVolumes[index] = Math.Max(this.m_thrusterVolumes[index] - this.m_groupData.ThrusterCompositionChangeSpeed, this.m_thrusterVolumeTargets[index]);
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UpdateShouldPlay2D()
    {
      bool shouldPlay2D = this.m_shouldPlay2D;
      if (this.m_shipGrid.GridSizeEnum == MyCubeSize.Large)
      {
        this.m_shouldPlay2D = this.m_insideShip && MySession.Static.ControlledEntity != null && (MySession.Static.ControlledEntity.Entity is MyCockpit || MySession.Static.ControlledEntity.Entity is MyRemoteControl || MySession.Static.ControlledEntity.Entity is MyShipController);
        if (this.m_shouldPlay2D)
        {
          MyCubeBlock entity = MySession.Static.ControlledEntity.Entity as MyCubeBlock;
          this.m_shouldPlay2D = ((this.m_shouldPlay2D ? 1 : 0) & (entity == null || entity.CubeGrid == null ? 0 : (entity.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 1 : 0))) != 0;
        }
      }
      else
        this.m_shouldPlay2D = MySession.Static.ControlledEntity != null && !MySession.Static.IsCameraUserControlledSpectator() && (MySession.Static.ControlledEntity.Entity != null && MySession.Static.ControlledEntity.Entity.Parent == this.m_shipGrid) && (MySession.Static.ControlledEntity.Entity is MyCockpit && (MySession.Static.ControlledEntity.Entity as MyCockpit).IsInFirstPersonView || MySession.Static.ControlledEntity.Entity is MyRemoteControl && MySession.Static.LocalCharacter != null && (MySession.Static.LocalCharacter.IsUsing is MyCockpit && (MySession.Static.LocalCharacter.IsUsing as MyCockpit).Parent == this.m_shipGrid) || MySession.Static.CameraController is MyCameraBlock && (MySession.Static.CameraController as MyCameraBlock).Parent == this.m_shipGrid);
      this.m_shouldPlay2DChanged = shouldPlay2D != this.m_shouldPlay2D;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void PlayThrusterPushSound(
      float shipSpeed,
      float volume,
      MySoundPair soundPair,
      MyEntity3DSoundEmitter emiter)
    {
      if (this.m_shipThrusters != null && (double) this.m_shipThrusters.ControlThrust.LengthSquared() >= 1.0)
      {
        if (!emiter.IsPlaying)
        {
          emiter.VolumeMultiplier = volume;
          emiter.PlaySound(soundPair, true, true, this.m_shouldPlay2D);
        }
        else
          emiter.VolumeMultiplier *= 0.995f;
      }
      else
      {
        if (!emiter.IsPlaying)
          return;
        emiter.VolumeMultiplier *= 0.5f;
        if ((double) emiter.VolumeMultiplier >= 0.100000001490116)
          return;
        emiter.StopSound(true);
      }
    }

    public void Update100()
    {
      if (this.m_shipGrid.IsStatic || this.m_shipThrusters == null && this.m_shipWheels.WheelCount == 0)
      {
        this.DeSchedule();
      }
      else
      {
        this.m_distanceToShip = !this.m_initialized || this.m_shipGrid.Physics == null ? float.MaxValue : (this.m_shouldPlay2D ? 0.0f : (float) this.m_shipGrid.PositionComp.WorldAABB.DistanceSquared(MySector.MainCamera.Position));
        if ((double) this.m_distanceToShip < (double) MyShipSoundComponent.m_definition.MaxUpdateRange_sq)
        {
          this.Schedule();
          this.UpdateCategory();
          this.UpdateSounds();
          this.UpdateWheels();
        }
        else
          this.DeSchedule();
      }
    }

    private void UpdateCategory()
    {
      if (!this.m_initialized || this.m_shipGrid == null || (this.m_shipGrid.Physics == null || this.m_shipGrid.IsStatic) || (MyShipSoundComponent.m_definition == null || (double) this.m_distanceToShip >= (double) MyShipSoundComponent.m_definition.MaxUpdateRange_sq))
        return;
      if (this.m_shipWheels == null)
        this.m_shipWheels = this.m_shipGrid.GridSystems.WheelSystem;
      this.CalculateShipCategory();
      if (!this.m_isDebris && this.m_shipState != MyShipSoundComponent.ShipStateEnum.NoPower && (!this.m_singleThrusterTypeShip || this.ShipHasChanged || (this.m_shipThrusters == null || this.m_shipThrusters.FinalThrust == Vector3.Zero) || this.m_shipWheels != null && this.m_shipWheels.HasWorkingWheels(false)))
        this.CalculateThrusterComposition();
      if (this.m_shipSoundSource == null)
        this.m_shipSoundSource = (MyEntity) this.m_shipGrid;
      if (this.m_shipGrid.MainCockpit != null && this.m_shipGrid.GridSizeEnum == MyCubeSize.Small)
        this.m_shipSoundSource = (MyEntity) this.m_shipGrid.MainCockpit;
      if (this.m_shipGrid.GridSizeEnum == MyCubeSize.Large && MySession.Static != null && MySession.Static.LocalCharacter != null)
        this.m_insideShip = MySession.Static.LocalCharacter.ReverbDetectorComp != null && (!MySession.Static.Settings.RealisticSound || MySession.Static.LocalCharacter.AtmosphereDetectorComp != null && (MySession.Static.LocalCharacter.AtmosphereDetectorComp.InAtmosphere || MySession.Static.LocalCharacter.AtmosphereDetectorComp.InShipOrStation)) && MySession.Static.LocalCharacter.ReverbDetectorComp.Grids > 0;
      MyShipSoundsDefinition groupData = this.m_groupData;
    }

    private void UpdateSounds()
    {
      for (int index = 0; index < this.m_emitters.Length; ++index)
      {
        if (this.m_emitters[index] != null)
        {
          this.m_emitters[index].Entity = this.m_shipSoundSource;
          this.m_emitters[index].Update();
        }
      }
    }

    private void UpdateWheels()
    {
      if (this.m_shipGrid == null || this.m_shipGrid.Physics == null || (this.m_shipWheels == null || this.m_shipWheels.WheelCount <= 0))
        return;
      bool flag1 = (double) this.m_distanceToShip < (double) MyShipSoundComponent.m_definition.WheelsCallbackRangeCreate_sq && !this.m_isDebris;
      bool flag2 = (double) this.m_distanceToShip > (double) MyShipSoundComponent.m_definition.WheelsCallbackRangeRemove_sq || this.m_isDebris;
      if (!(flag1 | flag2) || this.m_lastWheelUpdateStart == flag1 && this.m_lastWheelUpdateStop == flag2)
        return;
      foreach (MyMotorSuspension wheel in this.m_shipWheels.Wheels)
      {
        if (wheel != null && wheel.RotorGrid != null && (wheel.RotorGrid.Physics != null && !((HkReferenceObject) wheel.RotorGrid.Physics.RigidBody == (HkReferenceObject) null)))
        {
          if (!wheel.RotorGrid.HasShipSoundEvents & flag1)
          {
            wheel.RotorGrid.Physics.RigidBody.ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
            wheel.RotorGrid.Physics.RigidBody.CallbackLimit = 1;
            wheel.RotorGrid.OnClosing += new Action<MyEntity>(this.RotorGrid_OnClosing);
            wheel.RotorGrid.HasShipSoundEvents = true;
          }
          else if (wheel.RotorGrid.HasShipSoundEvents & flag2)
          {
            wheel.RotorGrid.HasShipSoundEvents = false;
            wheel.RotorGrid.Physics.RigidBody.ContactPointCallback -= new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
            wheel.RotorGrid.OnClosing -= new Action<MyEntity>(this.RotorGrid_OnClosing);
          }
        }
      }
      this.m_lastWheelUpdateStart = flag1;
      this.m_lastWheelUpdateStop = flag2;
      if (flag1 && !this.m_shipWheelsAction)
      {
        this.m_shipWheels.OnMotorUnregister += new Action<MyCubeGrid>(this.m_shipWheels_OnMotorUnregister);
        this.m_shipWheelsAction = true;
      }
      else
      {
        if (!flag2 || !this.m_shipWheelsAction)
          return;
        this.m_shipWheels.OnMotorUnregister -= new Action<MyCubeGrid>(this.m_shipWheels_OnMotorUnregister);
        this.m_shipWheelsAction = false;
      }
    }

    private void m_shipWheels_OnMotorUnregister(MyCubeGrid obj)
    {
      if (!obj.HasShipSoundEvents)
        return;
      obj.HasShipSoundEvents = false;
      this.RotorGrid_OnClosing((MyEntity) obj);
    }

    private void RotorGrid_OnClosing(MyEntity obj)
    {
      if (obj.Physics == null)
        return;
      obj.Physics.RigidBody.ContactPointCallback -= new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
      obj.OnClose -= new Action<MyEntity>(this.RotorGrid_OnClosing);
    }

    private void RigidBody_ContactPointCallback(ref HkContactPointEvent A_0) => this.m_lastContactWithGround = DateTime.UtcNow;

    private void CalculateThrusterComposition()
    {
      if (this.m_shipThrusters == null)
      {
        this.m_thrusterVolumeTargets[0] = 0.0f;
        this.m_thrusterVolumeTargets[1] = 0.0f;
        this.m_thrusterVolumeTargets[2] = 0.0f;
      }
      else
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        foreach (MyThrust fatBlock in this.m_shipGrid.GetFatBlocks<MyThrust>())
        {
          if (fatBlock != null)
          {
            if (fatBlock.BlockDefinition.ThrusterType == MyShipSoundComponent.m_thrusterHydrogen)
            {
              num2 += fatBlock.CurrentStrength * (Math.Abs(fatBlock.ThrustForce.X) + Math.Abs(fatBlock.ThrustForce.Y) + Math.Abs(fatBlock.ThrustForce.Z));
              flag3 = flag3 || fatBlock.IsFunctional && fatBlock.Enabled && fatBlock.IsPowered;
            }
            else if (fatBlock.BlockDefinition.ThrusterType == MyShipSoundComponent.m_thrusterAtmospheric)
            {
              num3 += fatBlock.CurrentStrength * (Math.Abs(fatBlock.ThrustForce.X) + Math.Abs(fatBlock.ThrustForce.Y) + Math.Abs(fatBlock.ThrustForce.Z));
              flag2 = flag2 || fatBlock.IsFunctional && fatBlock.Enabled;
            }
            else
            {
              num1 += fatBlock.CurrentStrength * (Math.Abs(fatBlock.ThrustForce.X) + Math.Abs(fatBlock.ThrustForce.Y) + Math.Abs(fatBlock.ThrustForce.Z));
              flag1 = flag1 || fatBlock.IsFunctional && fatBlock.Enabled;
            }
          }
        }
        this.ShipHasChanged = false;
        this.m_singleThrusterTypeShip = !(flag1 & flag2) && !(flag1 & flag3) && !(flag3 & flag2);
        if (this.m_singleThrusterTypeShip)
        {
          this.m_thrusterVolumeTargets[0] = flag1 ? 1f : 0.0f;
          this.m_thrusterVolumeTargets[1] = flag3 ? 1f : 0.0f;
          this.m_thrusterVolumeTargets[2] = flag2 ? 1f : 0.0f;
          if (!flag1 && !flag3 && !flag2)
            this.ShipHasChanged = true;
        }
        else if ((double) num1 + (double) num2 + (double) num3 > 0.0)
        {
          float num4 = num2 + num1 + num3;
          float num5 = (double) num1 > 0.0 ? (float) (((double) this.m_groupData.ThrusterCompositionMinVolume_c + (double) num1 / (double) num4) / (1.0 + (double) this.m_groupData.ThrusterCompositionMinVolume_c)) : 0.0f;
          float num6 = (double) num2 > 0.0 ? (float) (((double) this.m_groupData.ThrusterCompositionMinVolume_c + (double) num2 / (double) num4) / (1.0 + (double) this.m_groupData.ThrusterCompositionMinVolume_c)) : 0.0f;
          float num7 = (double) num3 > 0.0 ? (float) (((double) this.m_groupData.ThrusterCompositionMinVolume_c + (double) num3 / (double) num4) / (1.0 + (double) this.m_groupData.ThrusterCompositionMinVolume_c)) : 0.0f;
          this.m_thrusterVolumeTargets[0] = num5;
          this.m_thrusterVolumeTargets[1] = num6;
          this.m_thrusterVolumeTargets[2] = num7;
        }
        if ((double) this.m_thrusterVolumes[0] <= 0.0 && this.m_emitters[2].IsPlaying)
        {
          this.m_emitters[2].StopSound(false);
          this.m_emitters[5].StopSound(false);
        }
        if ((double) this.m_thrusterVolumes[1] <= 0.0 && this.m_emitters[3].IsPlaying)
        {
          this.m_emitters[3].StopSound(false);
          this.m_emitters[6].StopSound(false);
        }
        if ((double) this.m_thrusterVolumes[2] <= 0.0 && this.m_emitters[4].IsPlaying)
        {
          this.m_emitters[4].StopSound(false);
          this.m_emitters[7].StopSound(false);
        }
        if (((double) this.m_thrusterVolumeTargets[0] <= 0.0 || this.m_emitters[2].IsPlaying) && ((double) this.m_thrusterVolumeTargets[1] <= 0.0 || this.m_emitters[3].IsPlaying) && ((double) this.m_thrusterVolumeTargets[2] <= 0.0 || this.m_emitters[4].IsPlaying))
          return;
        this.m_forceSoundCheck = true;
      }
    }

    private void CalculateShipCategory()
    {
      bool isDebris = this.m_isDebris;
      MyDefinitionId shipCategory = this.m_shipCategory;
      if (this.m_shipThrusters == null && (this.m_shipWheels == null || this.m_shipWheels.WheelCount <= 0))
      {
        this.m_isDebris = true;
      }
      else
      {
        bool flag = false;
        foreach (MyCubeBlock fatBlock in this.m_shipGrid.GetFatBlocks())
        {
          if (fatBlock is MyShipController)
          {
            if (this.m_shipGrid.MainCockpit == null && this.m_shipGrid.GridSizeEnum == MyCubeSize.Small)
              this.m_shipSoundSource = (MyEntity) fatBlock;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          int currentMass = this.m_shipGrid.GetCurrentMass();
          float num = float.MinValue;
          MyDefinitionId? nullable = new MyDefinitionId?();
          foreach (MyMotorSuspension wheel in this.m_shipWheels.Wheels)
          {
            if (wheel.BlockDefinition.SoundDefinitionId.HasValue)
              nullable = new MyDefinitionId?(wheel.BlockDefinition.SoundDefinitionId.Value);
          }
          foreach (MyShipSoundsDefinition soundsDefinition in MyShipSoundComponent.m_categories.Values)
          {
            if ((double) soundsDefinition.MinWeight < (double) currentMass && (soundsDefinition.AllowSmallGrid && this.m_shipGrid.GridSizeEnum == MyCubeSize.Small || soundsDefinition.AllowLargeGrid && this.m_shipGrid.GridSizeEnum == MyCubeSize.Large) && ((double) num == -3.40282346638529E+38 || (double) soundsDefinition.MinWeight > (double) num))
            {
              num = soundsDefinition.MinWeight;
              this.m_shipCategory = soundsDefinition.Id;
              this.m_groupData = soundsDefinition;
            }
            if (nullable.HasValue && soundsDefinition.Id.Equals(nullable.Value))
            {
              num = soundsDefinition.MinWeight;
              this.m_shipCategory = soundsDefinition.Id;
              this.m_groupData = soundsDefinition;
              break;
            }
          }
          this.m_isDebris = (double) num == -3.40282346638529E+38;
        }
        else
          this.m_isDebris = true;
      }
      if (this.m_groupData == null)
        this.m_isDebris = true;
      if (shipCategory != this.m_shipCategory || this.m_isDebris != isDebris)
      {
        this.m_categoryChange = true;
        if (this.m_isDebris)
        {
          for (int index = 0; index < this.m_emitters.Length; ++index)
          {
            if (this.m_emitters[index].IsPlaying && this.m_emitters[index].Loop)
            {
              if (index == 8 || index == 9)
                this.m_emitters[index].StopSound(this.m_shipWheels == null);
              else
                this.m_emitters[index].StopSound(this.m_shipThrusters == null);
            }
          }
        }
        else
        {
          for (int index = 0; index < this.m_emitters.Length; ++index)
          {
            if (this.m_emitters[index].IsPlaying && this.m_emitters[index].Loop)
              this.m_emitters[index].StopSound(true);
          }
        }
      }
      if (this.m_isDebris)
        this.SetGridSounds(false);
      else
        this.SetGridSounds(true);
    }

    private void SetGridSounds(bool silent)
    {
      foreach (MyCubeBlock fatBlock in this.m_shipGrid.GetFatBlocks())
      {
        if (fatBlock.BlockDefinition.SilenceableByShipSoundSystem && fatBlock.IsSilenced != silent)
        {
          int num = fatBlock.SilenceInChange ? 1 : 0;
          fatBlock.SilenceInChange = true;
          fatBlock.IsSilenced = silent;
          if (num == 0)
          {
            fatBlock.UsedUpdateEveryFrame = (uint) (fatBlock.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) > 0U;
            fatBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
          }
        }
      }
    }

    private float CalculateVolumeFromSpeed(float speedRatio, ref List<MyTuple<float, float>> pairs)
    {
      float num = 1f;
      if (pairs.Count > 0)
        num = pairs[pairs.Count - 1].Item2;
      for (int index = 1; index < pairs.Count; ++index)
      {
        if ((double) speedRatio < (double) pairs[index].Item1)
        {
          num = pairs[index - 1].Item2 + (float) (((double) pairs[index].Item2 - (double) pairs[index - 1].Item2) * (((double) speedRatio - (double) pairs[index - 1].Item1) / ((double) pairs[index].Item1 - (double) pairs[index - 1].Item1)));
          break;
        }
      }
      return num;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void FadeOutSound(MyShipSoundComponent.ShipEmitters emitter = MyShipSoundComponent.ShipEmitters.SingleSounds, int duration = 2000)
    {
      if (this.m_emitters[(int) emitter].IsPlaying)
      {
        IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(this.m_emitters[(int) emitter].Sound, MyShipSoundComponent.m_fadeOut, new MyCueId[0], new float?((float) duration));
        this.m_emitters[(int) emitter].SetSound(myAudioEffect.OutputSound, nameof (FadeOutSound));
      }
      if (emitter != MyShipSoundComponent.ShipEmitters.SingleSounds)
        return;
      this.m_playingSpeedUpOrDown = false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void PlayShipSound(
      MyShipSoundComponent.ShipEmitters emitter,
      ShipSystemSoundsEnum sound,
      bool checkIfAlreadyPlaying = true,
      bool stopPrevious = true,
      bool useForce2D = true,
      bool useFadeOut = false)
    {
      MySoundPair shipSound = this.GetShipSound(sound);
      if (shipSound == MySoundPair.Empty || this.m_emitters[(int) emitter] == null || checkIfAlreadyPlaying && this.m_emitters[(int) emitter].IsPlaying && this.m_emitters[(int) emitter].SoundPair == shipSound)
        return;
      if (this.m_emitters[(int) emitter].IsPlaying & useFadeOut)
      {
        IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(this.m_emitters[(int) emitter].Sound, MyStringHash.GetOrCompute("CrossFade"), new MyCueId[1]
        {
          shipSound.SoundId
        }, new float?(1500f));
        this.m_emitters[(int) emitter].SetSound(myAudioEffect.OutputSound, nameof (PlayShipSound));
      }
      else
        this.m_emitters[(int) emitter].PlaySound(shipSound, stopPrevious, force2D: (useForce2D && this.m_shouldPlay2D));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private MySoundPair GetShipSound(ShipSystemSoundsEnum sound)
    {
      MyShipSoundsDefinition soundsDefinition;
      MySoundPair mySoundPair;
      return this.m_isDebris || !MyShipSoundComponent.m_categories.TryGetValue(this.m_shipCategory, out soundsDefinition) || !soundsDefinition.Sounds.TryGetValue(sound, out mySoundPair) ? MySoundPair.Empty : mySoundPair;
    }

    public override string ComponentTypeDebugString => "ShipSoundSystem";

    public void DestroyComponent()
    {
      this.Container.ComponentAdded -= new Action<System.Type, MyEntityComponentBase>(this.ContainerOnComponentAdded);
      this.Container.ComponentRemoved -= new Action<System.Type, MyEntityComponentBase>(this.ContainerOnComponentRemoved);
      if (this.m_shipGrid != null)
      {
        this.m_shipGrid.OnPhysicsChanged -= new Action<MyEntity>(this.ShipGridOnOnPhysicsChanged);
        this.m_shipGrid.OnBlockRemoved -= new Action<MySlimBlock>(this.ShipGridOnOnBlockRemoved);
        this.m_shipGrid.OnBlockAdded -= new Action<MySlimBlock>(this.ShipGridOnOnBlockAdded);
      }
      for (int index = 0; index < this.m_emitters.Length; ++index)
      {
        if (this.m_emitters[index] != null)
        {
          this.m_emitters[index].StopSound(true);
          this.m_emitters[index].CleanEmitterMethods();
          this.m_emitters[index] = (MyEntity3DSoundEmitter) null;
        }
      }
      this.m_shipGrid = (MyCubeGrid) null;
      this.m_shipThrusters = (MyEntityThrustComponent) null;
      this.m_shipWheels = (MyGridWheelSystem) null;
      this.Container.Remove(this.GetType());
    }

    private enum ShipStateEnum
    {
      NoPower,
      Slow,
      Medium,
      Fast,
    }

    private enum ShipEmitters
    {
      MainSound,
      SingleSounds,
      IonThrusters,
      HydrogenThrusters,
      AtmosphericThrusters,
      IonThrustersIdle,
      HydrogenThrustersIdle,
      AtmosphericThrustersIdle,
      WheelsMain,
      WheelsSecondary,
      ShipIdle,
      ShipEngine,
      IonThrusterSpeedUp,
      HydrogenThrusterSpeedUp,
    }

    private enum ShipThrusters
    {
      Ion,
      Hydrogen,
      Atmospheric,
    }

    private enum ShipTimers
    {
      SpeedUp,
      SpeedDown,
    }

    private class Sandbox_Game_EntityComponents_MyShipSoundComponent\u003C\u003EActor : IActivator, IActivator<MyShipSoundComponent>
    {
      object IActivator.CreateInstance() => (object) new MyShipSoundComponent();

      MyShipSoundComponent IActivator<MyShipSoundComponent>.CreateInstance() => new MyShipSoundComponent();
    }
  }
}
