// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntity3DSoundEmitter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MyEntity3DSoundEmitter : IMy3DSoundEmitter
  {
    internal static readonly ConcurrentDictionary<MyCueId, MyEntity3DSoundEmitter.LastTimePlayingData> LastTimePlaying = new ConcurrentDictionary<MyCueId, MyEntity3DSoundEmitter.LastTimePlayingData>();
    private static List<MyEntity3DSoundEmitter> m_entityEmitters = new List<MyEntity3DSoundEmitter>();
    private static int m_lastUpdate = int.MinValue;
    private static MyStringHash m_effectHasHelmetInOxygen = MyStringHash.GetOrCompute("LowPassHelmet");
    private static MyStringHash m_effectNoHelmetNoOxygen = MyStringHash.GetOrCompute("LowPassNoHelmetNoOxy");
    private static MyStringHash m_effectEnclosedCockpitInSpace = MyStringHash.GetOrCompute("LowPassCockpitNoOxy");
    private static MyStringHash m_effectEnclosedCockpitInAir = MyStringHash.GetOrCompute("LowPassCockpit");
    private MyCueId m_cueEnum = new MyCueId(MyStringHash.NullOrEmpty);
    private readonly MyCueId myEmptyCueId = new MyCueId(MyStringHash.NullOrEmpty);
    private MySoundPair m_soundPair = MySoundPair.Empty;
    private IMySourceVoice m_sound;
    private IMySourceVoice m_secondarySound;
    private MyCueId m_secondaryCueEnum = new MyCueId(MyStringHash.NullOrEmpty);
    private float m_secondaryVolumeRatio;
    private bool m_secondaryEnabled;
    private float m_secondaryBaseVolume = 1f;
    private float m_baseVolume = 1f;
    private MyEntity m_entity;
    private Vector3D? m_position;
    private Vector3? m_velocity;
    private List<MyCueId> m_soundsQueue = new List<MyCueId>();
    private bool m_playing2D;
    private bool m_usesDistanceSounds;
    private bool m_useRealisticByDefault;
    private bool m_alwaysHearOnRealistic;
    private MyCueId m_closeSoundCueId = new MyCueId(MyStringHash.NullOrEmpty);
    private MySoundPair m_closeSoundSoundPair = MySoundPair.Empty;
    private bool m_realistic;
    private float m_volumeMultiplier = 1f;
    private bool m_volumeChanging;
    private MySoundData m_lastSoundData;
    private readonly object m_soundUpdateLock = new object();
    private readonly object m_lastSoundDataLock = new object();
    private MyStringHash m_activeEffect = MyStringHash.NullOrEmpty;
    private int m_lastPlayedWaveNumber = -1;
    private float? m_customVolume;
    private readonly Action<IMySourceVoice> m_OnSoundVoiceStopped;
    public Dictionary<int, ConcurrentCachingList<Delegate>> EmitterMethods = new Dictionary<int, ConcurrentCachingList<Delegate>>();
    public bool CanPlayLoopSounds = true;
    private string m_lastNullSetter;

    bool IMy3DSoundEmitter.Realistic => this.m_realistic;

    public event Action<MyEntity3DSoundEmitter> StoppedPlaying;

    public bool Loop { get; private set; }

    public bool IsPlaying
    {
      get
      {
        IMySourceVoice sound = this.Sound;
        return sound != null && sound.IsPlaying;
      }
    }

    public MyCueId SoundId
    {
      get => this.m_cueEnum;
      set
      {
        if (!(this.m_cueEnum != value))
          return;
        this.m_cueEnum = value;
        if (!(this.m_cueEnum.Hash == MyStringHash.GetOrCompute("None")))
          return;
        Debugger.Break();
      }
    }

    public MySoundData LastSoundData => this.m_lastSoundData;

    private float RealisticVolumeChange => !this.m_realistic || this.m_lastSoundData == null ? 1f : this.m_lastSoundData.RealisticVolumeChange;

    public float VolumeMultiplier
    {
      get => this.m_volumeMultiplier;
      set
      {
        this.m_volumeMultiplier = value;
        if (this.Sound == null)
          return;
        this.Sound.VolumeMultiplier = this.m_volumeMultiplier;
      }
    }

    public MySoundPair SoundPair => this.m_closeSoundSoundPair;

    public IMySourceVoice Sound => this.m_sound;

    public void SetSound(IMySourceVoice value, [CallerMemberName] string caller = null)
    {
      if (value == null)
        this.m_lastNullSetter = caller;
      lock (this.m_soundUpdateLock)
      {
        if (this.m_sound != null && this.m_sound != value)
          this.m_sound.StoppedPlaying = (Action<IMySourceVoice>) null;
        this.m_sound = value;
      }
    }

    public Vector3D SourcePosition
    {
      get
      {
        if (this.m_position.HasValue)
          return this.m_position.Value;
        return this.m_entity != null && MySector.MainCamera != null ? this.m_entity.PositionComp.WorldAABB.Center - MySector.MainCamera.Position : Vector3D.Zero;
      }
    }

    public Vector3 Velocity
    {
      get
      {
        if (this.m_velocity.HasValue)
          return this.m_velocity.Value;
        if (this.m_entity != null)
        {
          if (this.m_entity.Physics != null)
            return this.m_entity.Physics.LinearVelocity;
          if (this.m_entity.Parent != null && this.m_entity.Parent.Physics != null)
            return this.m_entity.Parent.Physics.LinearVelocity;
        }
        return Vector3.Zero;
      }
    }

    public MyEntity Entity
    {
      get => this.m_entity;
      set => this.m_entity = value;
    }

    public float? CustomMaxDistance { get; set; }

    public float? CustomVolume
    {
      get => this.m_customVolume;
      set
      {
        this.m_customVolume = value;
        if (!this.m_customVolume.HasValue)
          return;
        this.Sound?.SetVolume(this.RealisticVolumeChange * this.m_customVolume.Value);
      }
    }

    public bool Force3D { get; set; }

    public bool Force2D { get; set; }

    public bool Plays2D => this.m_playing2D;

    public int SourceChannels { get; set; }

    int IMy3DSoundEmitter.LastPlayedWaveNumber
    {
      get => this.m_lastPlayedWaveNumber;
      set => this.m_lastPlayedWaveNumber = value;
    }

    object IMy3DSoundEmitter.SyncRoot => this.m_soundUpdateLock;

    public object DebugData { get; set; }

    public void SetPosition(Vector3D? position)
    {
      if (!position.HasValue)
        this.m_position = position;
      else
        this.m_position = new Vector3D?(position.Value - MySector.MainCamera.Position);
    }

    public void SetVelocity(Vector3? velocity) => this.m_velocity = velocity;

    public float DopplerScaler { get; private set; }

    public MyEntity3DSoundEmitter(MyEntity entity, bool useStaticList = false, float dopplerScaler = 1f)
    {
      this.m_entity = entity;
      this.DopplerScaler = dopplerScaler;
      this.m_OnSoundVoiceStopped = new Action<IMySourceVoice>(this.OnSoundVoiceStopped);
      foreach (int key in Enum.GetValues(typeof (MyEntity3DSoundEmitter.MethodsEnum)))
        this.EmitterMethods.Add(key, new ConcurrentCachingList<Delegate>());
      this.EmitterMethods[1].Add((Delegate) new Func<bool>(this.IsControlledEntity));
      if (MySession.Static != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
      {
        this.EmitterMethods[0].Add((Delegate) new Func<bool>(this.IsInAtmosphere));
        this.EmitterMethods[0].Add((Delegate) new Func<bool>(this.IsCurrentWeapon));
        this.EmitterMethods[0].Add((Delegate) new Func<bool>(this.IsOnSameGrid));
        this.EmitterMethods[0].Add((Delegate) new Func<bool>(this.IsControlledEntity));
        this.EmitterMethods[1].Add((Delegate) new Func<bool>(this.IsCurrentWeapon));
        this.EmitterMethods[2].Add((Delegate) new Func<MySoundPair, MyCueId>(this.SelectCue));
        this.EmitterMethods[3].Add((Delegate) new Func<MyStringHash>(this.SelectEffect));
      }
      this.UpdateEmitterMethods();
      this.m_useRealisticByDefault = MySession.Static != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS;
      if (((MySession.Static == null || !MySession.Static.Settings.RealisticSound ? 0 : (MyFakes.ENABLE_NEW_SOUNDS ? 1 : 0)) & (useStaticList ? 1 : 0)) == 0 || entity == null || !MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE)
        return;
      lock (MyEntity3DSoundEmitter.m_entityEmitters)
        MyEntity3DSoundEmitter.m_entityEmitters.Add(this);
    }

    public void Update()
    {
      this.UpdateEmitterMethods();
      bool isPlaying = this.IsPlaying;
      if (!this.CanHearSound())
      {
        if (!isPlaying)
          return;
        this.StopSound(true, false);
        this.SetSound((IMySourceVoice) null, nameof (Update));
      }
      else
      {
        if (!isPlaying && this.Loop)
          this.PlaySound(this.m_closeSoundSoundPair, true, true);
        else if (isPlaying && this.Loop && this.m_playing2D != this.ShouldPlay2D() && (this.Force2D && !this.m_playing2D || this.Force3D && this.m_playing2D))
        {
          this.StopSound(true, false);
          this.PlaySound(this.m_closeSoundSoundPair, true, true);
        }
        else if (isPlaying && this.Loop && (!this.m_playing2D && this.m_usesDistanceSounds))
        {
          MyCueId myCueId = this.m_secondaryEnabled ? this.m_secondaryCueEnum : this.myEmptyCueId;
          MyCueId soundId = this.CheckDistanceSounds(this.m_closeSoundCueId);
          if (soundId != this.m_cueEnum || myCueId != this.m_secondaryCueEnum)
            this.PlaySoundWithDistance(soundId, true, true, useDistanceCheck: false);
          else if (this.m_secondaryEnabled)
          {
            this.Sound?.SetVolume((float) ((double) this.RealisticVolumeChange * (double) this.m_baseVolume * (1.0 - (double) this.m_secondaryVolumeRatio)));
            this.m_secondarySound?.SetVolume(this.RealisticVolumeChange * this.m_secondaryBaseVolume * this.m_secondaryVolumeRatio);
          }
        }
        if (!isPlaying || !this.Loop)
          return;
        MyCueId soundId1 = this.SelectCue(this.m_soundPair);
        if (!soundId1.Equals(this.m_cueEnum))
          this.PlaySoundWithDistance(soundId1, true, true);
        if (!(this.m_activeEffect != this.SelectEffect()))
          return;
        this.PlaySoundWithDistance(soundId1, true, true);
      }
    }

    public bool FastUpdate(bool silenced)
    {
      if (silenced)
      {
        this.VolumeMultiplier = Math.Max(0.0f, this.m_volumeMultiplier - 0.01f);
        if ((double) this.m_volumeMultiplier == 0.0)
          return false;
      }
      else
      {
        this.VolumeMultiplier = Math.Min(1f, this.m_volumeMultiplier + 0.01f);
        if ((double) this.m_volumeMultiplier == 1.0)
          return false;
      }
      return true;
    }

    private void UpdateEmitterMethods()
    {
      foreach (ConcurrentCachingList<Delegate> concurrentCachingList in this.EmitterMethods.Values)
        concurrentCachingList.ApplyChanges();
    }

    private bool ShouldPlay2D()
    {
      bool flag = this.EmitterMethods[1].Count == 0;
      foreach (Delegate @delegate in this.EmitterMethods[1])
      {
        if ((object) @delegate != null)
          flag |= ((Func<bool>) @delegate)();
      }
      return flag;
    }

    private bool CanHearSound()
    {
      bool flag = false;
      ConcurrentCachingList<Delegate> concurrentCachingList;
      if (this.EmitterMethods != null && this.EmitterMethods.TryGetValue(0, out concurrentCachingList))
      {
        flag = concurrentCachingList.Count == 0 || MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS && this.m_alwaysHearOnRealistic;
        if (!flag)
        {
          foreach (Func<bool> func in concurrentCachingList)
          {
            if (func != null && func())
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
          flag = this.IsCloseEnough();
      }
      return flag;
    }

    private bool IsOnSameGrid()
    {
      if (this.Entity == null || this.Entity.EntityId == 0L)
        return false;
      if (this.Entity is MyCubeBlock || this.Entity is MyCubeGrid)
      {
        MyCubeGrid entity = (MyCubeGrid) null;
        if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity is MyCockpit)
          entity = (MySession.Static.ControlledEntity.Entity as MyCockpit).CubeGrid;
        else if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.SoundComp != null)
          entity = MySession.Static.LocalCharacter.SoundComp.StandingOnGrid;
        if (entity == null)
        {
          if (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.AtmosphereDetectorComp == null)
            return false;
          if (MySession.Static.LocalCharacter.AtmosphereDetectorComp.InShipOrStation)
            MyEntities.TryGetEntityById<MyCubeGrid>((long) MySession.Static.LocalCharacter.OxygenSourceGridEntityId, out entity);
        }
        MyCubeGrid nodeB = this.Entity is MyCubeBlock ? (this.Entity as MyCubeBlock).CubeGrid : this.Entity as MyCubeGrid;
        if (entity == null && MySession.Static.LocalCharacter != null && (MySession.Static.LocalCharacter.SoundComp != null && MySession.Static.LocalCharacter.SoundComp.StandingOnVoxel != null))
        {
          if (nodeB.IsStatic)
            return true;
          foreach (IMyEntity attachedEntity in nodeB.GridSystems.LandingSystem.GetAttachedEntities())
          {
            if (attachedEntity is MyVoxelBase && attachedEntity as MyVoxelBase == MySession.Static.LocalCharacter.SoundComp.StandingOnVoxel)
              return true;
          }
        }
        return entity != null && (entity == nodeB || MyCubeGridGroups.Static.Physical.HasSameGroup(entity, nodeB));
      }
      if (this.Entity is MyVoxelBase && (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity.Entity is MyCockpit)) && (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.SoundComp != null))
      {
        if (MySession.Static.LocalCharacter.SoundComp.StandingOnVoxel == this.Entity as MyVoxelBase)
          return true;
        if (MySession.Static.LocalCharacter.SoundComp.StandingOnGrid != null)
        {
          if (MySession.Static.LocalCharacter.SoundComp.StandingOnGrid.IsStatic)
            return true;
          foreach (IMyEntity attachedEntity in MySession.Static.LocalCharacter.SoundComp.StandingOnGrid.GridSystems.LandingSystem.GetAttachedEntities())
          {
            if (attachedEntity is MyVoxelBase && attachedEntity as MyVoxelBase == this.Entity as MyVoxelBase)
              return true;
          }
        }
      }
      return false;
    }

    private bool IsCurrentWeapon() => this.Entity is IMyHandheldGunObject<MyDeviceBase> && MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity is MyCharacter && (MySession.Static.ControlledEntity.Entity as MyCharacter).CurrentWeapon == this.Entity;

    private bool IsCloseEnough() => this.m_playing2D || MyAudio.Static.SourceIsCloseEnoughToPlaySound((Vector3) this.SourcePosition, this.SoundId, this.CustomMaxDistance);

    private bool IsInTerminal() => MyGuiScreenTerminal.IsOpen && MyGuiScreenTerminal.InteractedEntity != null && MyGuiScreenTerminal.InteractedEntity == this.Entity;

    private bool IsControlledEntity() => MySession.Static.ControlledEntity != null && this.m_entity == MySession.Static.ControlledEntity.Entity;

    private bool IsBeingWelded()
    {
      if (MySession.Static == null || MySession.Static.ControlledEntity == null || (!(MySession.Static.ControlledEntity.Entity is MyCharacter entity) || !(entity.CurrentWeapon is MyEngineerToolBase currentWeapon)))
        return false;
      MyCubeGrid targetGrid = currentWeapon.GetTargetGrid();
      MyCubeBlock entity1 = this.Entity as MyCubeBlock;
      if (targetGrid == null || entity1 == null || (targetGrid != entity1.CubeGrid || !currentWeapon.HasHitBlock))
        return false;
      MySlimBlock cubeBlock = targetGrid.GetCubeBlock(currentWeapon.TargetCube);
      return cubeBlock != null && cubeBlock.FatBlock == entity1 && currentWeapon.IsShooting;
    }

    private bool IsThereAir() => MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.AtmosphereDetectorComp != null && !MySession.Static.LocalCharacter.AtmosphereDetectorComp.InVoid;

    private bool IsInAtmosphere() => MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.AtmosphereDetectorComp != null && MySession.Static.LocalCharacter.AtmosphereDetectorComp.InAtmosphere;

    private MyCueId SelectCue(MySoundPair sound)
    {
      if (this.m_useRealisticByDefault)
      {
        if (this.m_lastSoundData == null)
          this.m_lastSoundData = MyAudio.Static.GetCue(sound.Realistic);
        if (this.m_lastSoundData != null && this.m_lastSoundData.AlwaysUseOneMode)
        {
          this.m_realistic = true;
          return sound.Realistic;
        }
        MyCockpit myCockpit = MySession.Static.LocalCharacter != null ? MySession.Static.LocalCharacter.Parent as MyCockpit : (MyCockpit) null;
        bool flag = myCockpit != null && myCockpit.CubeGrid.GridSizeEnum == MyCubeSize.Large && myCockpit.BlockDefinition.IsPressurized;
        if (this.IsThereAir() | flag)
        {
          this.m_realistic = false;
          return sound.Arcade;
        }
        this.m_realistic = true;
        return sound.Realistic;
      }
      this.m_realistic = false;
      return sound.Arcade;
    }

    private MyStringHash SelectEffect()
    {
      if (this.m_lastSoundData != null && !this.m_lastSoundData.ModifiableByHelmetFilters || (MySession.Static == null || MySession.Static.LocalCharacter == null) || (MySession.Static.LocalCharacter.OxygenComponent == null || !MyFakes.ENABLE_NEW_SOUNDS || !MySession.Static.Settings.RealisticSound))
        return MyStringHash.NullOrEmpty;
      bool flag1 = this.IsThereAir();
      bool flag2 = MySession.Static.LocalCharacter.Parent is MyCockpit parent && parent.BlockDefinition != null && parent.BlockDefinition.IsPressurized;
      if (flag1 & flag2)
        return MyEntity3DSoundEmitter.m_effectEnclosedCockpitInAir;
      if (!flag1 & flag2 && parent.CubeGrid != null && parent.CubeGrid.GridSizeEnum == MyCubeSize.Large)
        return MyEntity3DSoundEmitter.m_effectEnclosedCockpitInSpace;
      if (MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled & flag1)
        return MyEntity3DSoundEmitter.m_effectHasHelmetInOxygen;
      if (this.m_lastSoundData != null && MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && !flag1)
        return this.m_lastSoundData.RealisticFilter;
      if (!MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && !flag1 && (parent == null || parent.BlockDefinition == null || !parent.BlockDefinition.IsPressurized))
        return MyEntity3DSoundEmitter.m_effectNoHelmetNoOxygen;
      return this.m_lastSoundData != null && parent != null && (parent.BlockDefinition != null && parent.BlockDefinition.IsPressurized) && (parent.CubeGrid != null && parent.CubeGrid.GridSizeEnum == MyCubeSize.Small) ? this.m_lastSoundData.RealisticFilter : MyStringHash.NullOrEmpty;
    }

    private bool CheckForSynchronizedSounds()
    {
      if (this.m_lastSoundData != null && this.m_lastSoundData.PreventSynchronization >= 0)
      {
        MyEntity3DSoundEmitter.LastTimePlayingData lastTimePlayingData;
        bool flag = MyEntity3DSoundEmitter.LastTimePlaying.TryGetValue(this.SoundId, out lastTimePlayingData);
        if (!flag)
        {
          lastTimePlayingData.LastTime = 0;
          lastTimePlayingData.Emitter = this;
          MyEntity3DSoundEmitter.LastTimePlaying.TryAdd(this.SoundId, lastTimePlayingData);
        }
        int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
        if (sessionTotalFrames - lastTimePlayingData.LastTime <= this.m_lastSoundData.PreventSynchronization & flag)
        {
          Vector3D listenerPosition = (Vector3D) MyAudio.Static.GetListenerPosition();
          Vector3D sourcePosition = this.SourcePosition;
          double num1 = sourcePosition.LengthSquared();
          sourcePosition = lastTimePlayingData.Emitter.SourcePosition;
          double num2 = sourcePosition.LengthSquared();
          if (num1 > num2)
            return false;
        }
        lastTimePlayingData.LastTime = sessionTotalFrames;
        lastTimePlayingData.Emitter = this;
        MyEntity3DSoundEmitter.LastTimePlaying[this.SoundId] = lastTimePlayingData;
      }
      return true;
    }

    public void PlaySound(
      byte[] buffer,
      float volume = 1f,
      float maxDistance = 0.0f,
      MySoundDimensions dimension = MySoundDimensions.D3)
    {
      MyEntity entity = this.Entity;
      if ((entity != null ? (!entity.InScene ? 1 : 0) : 0) != 0)
        return;
      this.CustomMaxDistance = new float?(maxDistance);
      this.CustomVolume = new float?(volume);
      if (this.Sound == null || !this.Sound.IsValid)
        this.SetSound(MyAudio.Static.GetSound((IMy3DSoundEmitter) this, dimension), nameof (PlaySound));
      if (this.Sound == null)
        return;
      this.Sound.SubmitBuffer(buffer);
      if (this.Sound.IsPlaying)
        return;
      this.Sound.StartBuffered();
    }

    public void PlaySingleSound(MyCueId soundId, bool stopPrevious = false, bool skipIntro = false, bool? force3D = null)
    {
      if (this.m_cueEnum == soundId)
        return;
      this.PlaySoundWithDistance(soundId, stopPrevious, skipIntro, force3D: force3D);
    }

    public bool PlaySingleSound(
      MySoundPair soundId,
      bool stopPrevious = false,
      bool skipIntro = false,
      bool skipToEnd = false,
      bool? force3D = null)
    {
      MyEntity entity = this.Entity;
      if ((entity != null ? (!entity.InScene ? 1 : 0) : 0) != 0)
        return false;
      this.m_closeSoundSoundPair = soundId;
      this.m_soundPair = soundId;
      MyCueId soundId1 = this.m_useRealisticByDefault ? soundId.Realistic : soundId.Arcade;
      if (this.EmitterMethods[2].Count > 0)
        soundId1 = ((Func<MySoundPair, MyCueId>) this.EmitterMethods[2][0])(soundId);
      if (this.m_cueEnum.Equals(soundId1))
        return true;
      this.PlaySoundWithDistance(soundId1, stopPrevious, skipIntro, skipToEnd: skipToEnd, force3D: force3D);
      return true;
    }

    public bool PlaySound(
      MySoundPair soundId,
      bool stopPrevious = false,
      bool skipIntro = false,
      bool force2D = false,
      bool alwaysHearOnRealistic = false,
      bool skipToEnd = false,
      bool? force3D = null)
    {
      MyEntity entity = this.Entity;
      if ((entity != null ? (!entity.InScene ? 1 : 0) : 0) != 0)
        return false;
      this.m_closeSoundSoundPair = soundId;
      this.m_soundPair = soundId;
      MyCueId myCueId = this.m_useRealisticByDefault ? soundId.Realistic : soundId.Arcade;
      if (this.EmitterMethods[2].Count > 0)
        myCueId = ((Func<MySoundPair, MyCueId>) this.EmitterMethods[2][0])(soundId);
      MyCueId soundId1 = myCueId;
      int num1 = stopPrevious ? 1 : 0;
      int num2 = skipIntro ? 1 : 0;
      int num3 = force2D ? 1 : 0;
      bool? nullable = force3D;
      int num4 = alwaysHearOnRealistic ? 1 : 0;
      int num5 = skipToEnd ? 1 : 0;
      bool? force3D1 = nullable;
      this.PlaySoundWithDistance(soundId1, num1 != 0, num2 != 0, num3 != 0, alwaysHearOnRealistic: (num4 != 0), skipToEnd: (num5 != 0), force3D: force3D1);
      return true;
    }

    public bool PlaySoundWithDistance(
      MyCueId soundId,
      bool stopPrevious = false,
      bool skipIntro = false,
      bool force2D = false,
      bool useDistanceCheck = true,
      bool alwaysHearOnRealistic = false,
      bool skipToEnd = false,
      bool? force3D = null)
    {
      if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
        return false;
      this.m_lastSoundData = MyAudio.Static.GetCue(soundId);
      if (useDistanceCheck)
      {
        this.m_closeSoundCueId = soundId;
        soundId = this.CheckDistanceSounds(soundId);
      }
      bool usesDistanceSounds = this.m_usesDistanceSounds;
      if (this.Sound != null)
      {
        if (stopPrevious)
          this.StopSound(true);
        else if (this.Loop)
        {
          IMySourceVoice sound = this.Sound;
          this.StopSound(true);
          this.m_soundsQueue.Add(sound.CueEnum);
        }
      }
      if (this.m_secondarySound != null)
        this.m_secondarySound.Stop(true);
      this.SoundId = soundId;
      int num1 = skipIntro | skipToEnd ? 1 : 0;
      bool flag1 = force2D;
      bool? nullable = force3D;
      bool flag2 = alwaysHearOnRealistic;
      int num2 = skipToEnd ? 1 : 0;
      int num3 = flag1 ? 1 : 0;
      int num4 = flag2 ? 1 : 0;
      bool? force3D1 = nullable;
      this.PlaySoundInternal(num1 != 0, num2 != 0, num3 != 0, num4 != 0, force3D1);
      this.m_usesDistanceSounds = usesDistanceSounds;
      return true;
    }

    private MyCueId CheckDistanceSounds(MyCueId soundId)
    {
      if (!soundId.IsNull)
      {
        lock (this.m_lastSoundDataLock)
        {
          if (this.m_lastSoundData != null && this.m_lastSoundData.DistantSounds != null && this.m_lastSoundData.DistantSounds.Count > 0)
          {
            double num1 = this.SourcePosition.LengthSquared();
            int index1 = -1;
            this.m_usesDistanceSounds = true;
            this.m_secondaryEnabled = false;
            for (int index2 = 0; index2 < this.m_lastSoundData.DistantSounds.Count; ++index2)
            {
              double num2 = (double) this.m_lastSoundData.DistantSounds[index2].Distance * (double) this.m_lastSoundData.DistantSounds[index2].Distance;
              if (num1 > num2)
              {
                index1 = index2;
              }
              else
              {
                float num3 = (double) this.m_lastSoundData.DistantSounds[index2].DistanceCrossfade >= 0.0 ? this.m_lastSoundData.DistantSounds[index2].DistanceCrossfade * this.m_lastSoundData.DistantSounds[index2].DistanceCrossfade : float.MaxValue;
                if (num1 > (double) num3)
                {
                  this.m_secondaryVolumeRatio = (float) ((num1 - (double) num3) / (num2 - (double) num3));
                  this.m_secondaryEnabled = true;
                  MySoundPair sound = new MySoundPair(this.m_lastSoundData.DistantSounds[index2].Sound);
                  if (sound != MySoundPair.Empty)
                    this.m_secondaryCueEnum = this.SelectCue(sound);
                  else if (index1 >= 0)
                    this.m_secondaryCueEnum = new MyCueId(MyStringHash.GetOrCompute(this.m_lastSoundData.DistantSounds[index1].Sound));
                  else
                    this.m_secondaryEnabled = false;
                }
                else
                  break;
              }
            }
            if (index1 >= 0)
            {
              MySoundPair mySoundPair = new MySoundPair(this.m_lastSoundData.DistantSounds[index1].Sound);
              if (mySoundPair != MySoundPair.Empty)
              {
                this.m_soundPair = mySoundPair;
                soundId = this.SelectCue(this.m_soundPair);
              }
              else
                soundId = new MyCueId(MyStringHash.GetOrCompute(this.m_lastSoundData.DistantSounds[index1].Sound));
            }
            else
              this.m_soundPair = this.m_closeSoundSoundPair;
          }
          else
            this.m_usesDistanceSounds = false;
        }
      }
      if (!this.m_secondaryEnabled)
        this.m_secondaryCueEnum = this.myEmptyCueId;
      return soundId;
    }

    private void PlaySoundInternal(
      bool skipIntro = false,
      bool skipToEnd = false,
      bool force2D = false,
      bool alwaysHearOnRealistic = false,
      bool? force3D = null)
    {
      this.Force2D = force2D;
      if (force3D.HasValue)
        this.Force3D = force3D.Value;
      this.m_alwaysHearOnRealistic = alwaysHearOnRealistic;
      this.Loop = false;
      try
      {
        lock (this.m_soundUpdateLock)
        {
          if (!this.SoundId.IsNull && this.CheckForSynchronizedSounds())
          {
            this.m_playing2D = this.ShouldPlay2D() && !this.Force3D || this.Force2D;
            this.Loop = MyAudio.Static.IsLoopable(this.SoundId) && !skipToEnd && this.CanPlayLoopSounds;
            if (this.Loop && MySession.Static.ElapsedPlayTime.TotalSeconds < 6.0)
              skipIntro = true;
            if (this.m_playing2D)
              this.SetSound(MyAudio.Static.PlaySound(this.m_closeSoundCueId, (IMy3DSoundEmitter) this, skipIntro: skipIntro, skipToEnd: skipToEnd), nameof (PlaySoundInternal));
            else if (this.CanHearSound())
              this.SetSound(MyAudio.Static.PlaySound(this.SoundId, (IMy3DSoundEmitter) this, MySoundDimensions.D3, skipIntro, skipToEnd), nameof (PlaySoundInternal));
          }
          if (!this.IsPlaying)
            return;
          if (MyMusicController.Static != null && this.m_lastSoundData != null && (this.m_lastSoundData.DynamicMusicCategory != MyStringId.NullOrEmpty && this.m_lastSoundData.DynamicMusicAmount > 0))
            MyMusicController.Static.IncreaseCategory(this.m_lastSoundData.DynamicMusicCategory, this.m_lastSoundData.DynamicMusicAmount);
          this.m_baseVolume = this.Sound.Volume;
          this.Sound.SetVolume(this.Sound.Volume * this.RealisticVolumeChange);
          if (this.m_secondaryEnabled)
          {
            MyCueId secondaryCueEnum = this.m_secondaryCueEnum;
            this.m_secondarySound = MyAudio.Static.PlaySound(this.m_secondaryCueEnum, (IMy3DSoundEmitter) this, MySoundDimensions.D3, skipIntro, skipToEnd);
            if (this.Sound == null)
              return;
            if (this.m_secondarySound != null)
            {
              this.m_secondaryBaseVolume = this.m_secondarySound.Volume;
              this.Sound.SetVolume((float) ((double) this.RealisticVolumeChange * (double) this.m_baseVolume * (1.0 - (double) this.m_secondaryVolumeRatio)));
              this.m_secondarySound.SetVolume(this.RealisticVolumeChange * this.m_secondaryBaseVolume * this.m_secondaryVolumeRatio);
              this.m_secondarySound.VolumeMultiplier = this.m_volumeMultiplier;
            }
          }
          this.Sound.VolumeMultiplier = this.m_volumeMultiplier;
          this.Sound.StoppedPlaying = this.m_OnSoundVoiceStopped;
          if (this.EmitterMethods[3].Count <= 0)
            return;
          this.m_activeEffect = MyStringHash.NullOrEmpty;
          MyStringHash effect = ((Func<MyStringHash>) this.EmitterMethods[3][0])();
          if (!(effect != MyStringHash.NullOrEmpty))
            return;
          IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(this.Sound, effect);
          if (myAudioEffect == null)
            return;
          this.SetSound(myAudioEffect.OutputSound, nameof (PlaySoundInternal));
          this.m_activeEffect = effect;
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        MyLog.Default.WriteLine("Last null written by: " + this.m_lastNullSetter);
        throw;
      }
    }

    public void StopSound(bool forced, bool cleanUp = true, bool cleanupSound = false)
    {
      this.m_usesDistanceSounds = false;
      if (this.Sound != null)
      {
        this.Sound.Stop(forced);
        if (this.Loop && !forced)
          this.PlaySoundInternal(true, true);
        if (this.m_soundsQueue.Count == 0)
        {
          if (cleanupSound)
            this.Cleanup();
          this.SetSound((IMySourceVoice) null, nameof (StopSound));
          if (cleanUp)
          {
            this.Loop = false;
            this.SoundId = this.myEmptyCueId;
          }
        }
        else if (cleanUp)
        {
          this.SoundId = this.m_soundsQueue[0];
          this.PlaySoundInternal(true);
          this.m_soundsQueue.RemoveAt(0);
        }
      }
      else if (cleanUp)
      {
        this.Loop = false;
        this.SoundId = this.myEmptyCueId;
      }
      if (this.m_secondarySound == null)
        return;
      this.m_secondarySound.Stop(true);
    }

    public void Cleanup()
    {
      if (this.Sound != null)
      {
        this.Sound.Destroy();
        this.SetSound((IMySourceVoice) null, nameof (Cleanup));
      }
      if (this.m_secondarySound == null)
        return;
      this.m_secondarySound.Destroy();
      this.m_secondarySound = (IMySourceVoice) null;
    }

    internal void CleanEmitterMethods() => this.EmitterMethods.Clear();

    private void OnSoundVoiceStopped(IMySourceVoice voice)
    {
      voice.StoppedPlaying -= this.m_OnSoundVoiceStopped;
      this.OnStopPlaying();
      if (this.m_sound == voice)
      {
        this.SetSound((IMySourceVoice) null, nameof (OnSoundVoiceStopped));
      }
      else
      {
        if (this.m_secondarySound != voice)
          return;
        this.m_secondarySound = (IMySourceVoice) null;
      }
    }

    private void OnStopPlaying() => this.StoppedPlaying.InvokeIfNotNull<MyEntity3DSoundEmitter>(this);

    public static void PreloadSound(MySoundPair soundId)
    {
      IMySourceVoice sound = MyAudio.Static.GetSound(soundId.SoundId);
      if (sound == null)
        return;
      sound.Start(false);
      sound.Stop(true);
    }

    public static void UpdateEntityEmitters(
      bool removeUnused,
      bool updatePlaying,
      bool updateNotPlaying)
    {
      int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
      if (sessionTotalFrames == 0 || Math.Abs(MyEntity3DSoundEmitter.m_lastUpdate - sessionTotalFrames) < 5)
        return;
      MyEntity3DSoundEmitter.m_lastUpdate = sessionTotalFrames;
      lock (MyEntity3DSoundEmitter.m_entityEmitters)
      {
        for (int index = 0; index < MyEntity3DSoundEmitter.m_entityEmitters.Count; ++index)
        {
          if (MyEntity3DSoundEmitter.m_entityEmitters[index] != null && MyEntity3DSoundEmitter.m_entityEmitters[index].Entity != null && !MyEntity3DSoundEmitter.m_entityEmitters[index].Entity.Closed)
          {
            if (MyEntity3DSoundEmitter.m_entityEmitters[index].IsPlaying & updatePlaying || !MyEntity3DSoundEmitter.m_entityEmitters[index].IsPlaying & updateNotPlaying)
              MyEntity3DSoundEmitter.m_entityEmitters[index].Update();
          }
          else if (removeUnused)
          {
            MyEntity3DSoundEmitter.m_entityEmitters.RemoveAt(index);
            --index;
          }
        }
      }
    }

    public static void ClearEntityEmitters()
    {
      lock (MyEntity3DSoundEmitter.m_entityEmitters)
        MyEntity3DSoundEmitter.m_entityEmitters.Clear();
    }

    internal struct LastTimePlayingData
    {
      public int LastTime;
      public MyEntity3DSoundEmitter Emitter;
    }

    public enum MethodsEnum
    {
      CanHear,
      ShouldPlay2D,
      CueType,
      ImplicitEffect,
    }
  }
}
