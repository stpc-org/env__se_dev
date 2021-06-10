// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyCharacterSoundComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_CharacterSoundComponent), true)]
  public class MyCharacterSoundComponent : MyCharacterComponent
  {
    private readonly Dictionary<int, MySoundPair> CharacterSounds = new Dictionary<int, MySoundPair>();
    private static readonly MySoundPair EmptySoundPair = new MySoundPair();
    private static MyStringHash LowPressure = MyStringHash.GetOrCompute(nameof (LowPressure));
    private List<MyEntity3DSoundEmitter> m_soundEmitters;
    private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();
    private int m_lastScreamTime;
    private float m_jetpackSustainTimer;
    private float m_jetpackMinIdleTime;
    private const float JETPACK_TIME_BETWEEN_SOUNDS = 0.25f;
    private bool m_jumpReady;
    private const int SCREAM_DELAY_MS = 800;
    private const float DEFAULT_ANKLE_HEIGHT = 0.2f;
    private int m_lastStepTime;
    private int m_lastFootSound;
    private MyCharacterMovementEnum m_lastUpdateMovementState;
    private MyCharacter m_character;
    private MyCubeGrid m_standingOnGrid;
    private int m_lastContactCounter;
    private MyVoxelBase m_standingOnVoxel;
    private MyStringHash m_characterPhysicalMaterial = VRage.Game.MyMaterialType.CHARACTER;
    private bool m_isWalking;
    private const float WIND_SPEED_LOW = 40f;
    private const float WIND_SPEED_HIGH = 80f;
    private const float WIND_SPEED_DIFF = 40f;
    private const float WIND_CHANGE_SPEED = 0.008333334f;
    private float m_windVolume;
    private bool m_windVolumeChanged;
    private float m_windTargetVolume;
    private bool m_inAtmosphere = true;
    private MyEntity3DSoundEmitter m_windEmitter;
    private bool m_windSystem;
    private MyEntity3DSoundEmitter m_oxygenEmitter;
    private MyEntity3DSoundEmitter m_movementEmitter;
    private MyEntity3DSoundEmitter m_magneticBootsEmitter;
    private MySoundPair m_lastActionSound;
    private MySoundPair m_lastPrimarySound;
    private MySoundPair m_selectedStateSound;
    private bool m_isFirstPerson;
    private bool m_isFirstPersonChanged;
    private bool m_needsUpdateEmitters;
    private int Update;

    public MyCubeGrid StandingOnGrid => this.m_standingOnGrid;

    public MyVoxelBase StandingOnVoxel => this.m_standingOnVoxel;

    private bool ShouldUpdateSoundEmitters => this.m_character == MySession.Static.LocalCharacter && this.m_character.AtmosphereDetectorComp != null && (!this.m_character.AtmosphereDetectorComp.InAtmosphere && MyFakes.ENABLE_NEW_SOUNDS) && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE;

    public MyCharacterSoundComponent()
    {
      this.m_soundEmitters = new List<MyEntity3DSoundEmitter>(Enum.GetNames(typeof (MyCharacterSoundComponent.MySoundEmitterEnum)).Length);
      foreach (string name in Enum.GetNames(typeof (MyCharacterSoundComponent.MySoundEmitterEnum)))
        this.m_soundEmitters.Add(new MyEntity3DSoundEmitter(this.Entity as MyEntity));
      for (int key = 0; key < Enum.GetNames(typeof (CharacterSoundsEnum)).Length; ++key)
        this.CharacterSounds.Add(key, MyCharacterSoundComponent.EmptySoundPair);
      if (MySession.Static == null || !MySession.Static.Settings.EnableOxygen && !MySession.Static.CreativeMode)
        return;
      this.m_oxygenEmitter = new MyEntity3DSoundEmitter(this.Entity as MyEntity);
    }

    private void InitSounds()
    {
      if (this.m_character.Definition.JumpSoundName != null)
        this.CharacterSounds[0] = new MySoundPair(this.m_character.Definition.JumpSoundName);
      if (this.m_character.Definition.JetpackIdleSoundName != null)
        this.CharacterSounds[1] = new MySoundPair(this.m_character.Definition.JetpackIdleSoundName);
      if (this.m_character.Definition.JetpackRunSoundName != null)
        this.CharacterSounds[2] = new MySoundPair(this.m_character.Definition.JetpackRunSoundName);
      if (this.m_character.Definition.CrouchDownSoundName != null)
        this.CharacterSounds[3] = new MySoundPair(this.m_character.Definition.CrouchDownSoundName);
      if (this.m_character.Definition.CrouchUpSoundName != null)
        this.CharacterSounds[4] = new MySoundPair(this.m_character.Definition.CrouchUpSoundName);
      if (this.m_character.Definition.PainSoundName != null)
        this.CharacterSounds[5] = new MySoundPair(this.m_character.Definition.PainSoundName);
      if (this.m_character.Definition.SuffocateSoundName != null)
        this.CharacterSounds[6] = new MySoundPair(this.m_character.Definition.SuffocateSoundName);
      if (this.m_character.Definition.DeathSoundName != null)
        this.CharacterSounds[7] = new MySoundPair(this.m_character.Definition.DeathSoundName);
      if (this.m_character.Definition.DeathBySuffocationSoundName != null)
        this.CharacterSounds[8] = new MySoundPair(this.m_character.Definition.DeathBySuffocationSoundName);
      if (this.m_character.Definition.IronsightActSoundName != null)
        this.CharacterSounds[9] = new MySoundPair(this.m_character.Definition.IronsightActSoundName);
      if (this.m_character.Definition.IronsightDeactSoundName != null)
        this.CharacterSounds[10] = new MySoundPair(this.m_character.Definition.IronsightDeactSoundName);
      if (this.m_character.Definition.FastFlySoundName != null)
      {
        this.m_windEmitter = new MyEntity3DSoundEmitter(this.Entity as MyEntity);
        this.m_windEmitter.Force3D = false;
        this.m_windSystem = true;
        this.CharacterSounds[11] = new MySoundPair(this.m_character.Definition.FastFlySoundName);
      }
      if (this.m_character.Definition.HelmetOxygenNormalSoundName != null)
        this.CharacterSounds[12] = new MySoundPair(this.m_character.Definition.HelmetOxygenNormalSoundName);
      if (this.m_character.Definition.HelmetOxygenLowSoundName != null)
        this.CharacterSounds[13] = new MySoundPair(this.m_character.Definition.HelmetOxygenLowSoundName);
      if (this.m_character.Definition.HelmetOxygenCriticalSoundName != null)
        this.CharacterSounds[14] = new MySoundPair(this.m_character.Definition.HelmetOxygenCriticalSoundName);
      if (this.m_character.Definition.HelmetOxygenNoneSoundName != null)
        this.CharacterSounds[15] = new MySoundPair(this.m_character.Definition.HelmetOxygenNoneSoundName);
      if (this.m_character.Definition.MovementSoundName != null)
      {
        this.CharacterSounds[16] = new MySoundPair(this.m_character.Definition.MovementSoundName);
        this.m_movementEmitter = new MyEntity3DSoundEmitter(this.Entity as MyEntity);
      }
      if (string.IsNullOrEmpty(this.m_character.Definition.MagnetBootsStepsSoundName) && string.IsNullOrEmpty(this.m_character.Definition.MagnetBootsStartSoundName) && (string.IsNullOrEmpty(this.m_character.Definition.MagnetBootsEndSoundName) && string.IsNullOrEmpty(this.m_character.Definition.MagnetBootsProximitySoundName)))
        return;
      this.CharacterSounds[17] = new MySoundPair(this.m_character.Definition.MagnetBootsStepsSoundName);
      this.CharacterSounds[18] = new MySoundPair(this.m_character.Definition.MagnetBootsStartSoundName);
      this.CharacterSounds[19] = new MySoundPair(this.m_character.Definition.MagnetBootsEndSoundName);
      this.CharacterSounds[20] = new MySoundPair(this.m_character.Definition.MagnetBootsProximitySoundName);
      this.m_magneticBootsEmitter = new MyEntity3DSoundEmitter(this.Entity as MyEntity);
    }

    public void Preload()
    {
      foreach (MySoundPair soundId in this.CharacterSounds.Values)
        MyEntity3DSoundEmitter.PreloadSound(soundId);
    }

    public void CharacterDied()
    {
      if (!this.m_windEmitter.IsPlaying)
        return;
      this.m_windEmitter.StopSound(true);
    }

    public void UpdateWindSounds()
    {
      if (this.m_character.IsDead && (double) this.m_windVolume > 0.0)
      {
        this.m_windVolume = 0.0f;
        this.m_windVolumeChanged = (double) this.m_windVolume != (double) this.m_windTargetVolume;
      }
      if (!this.m_windSystem || this.m_character.IsDead)
        return;
      if (this.m_inAtmosphere)
      {
        float num = this.m_character.Physics.LinearVelocity.Length();
        this.m_windTargetVolume = (double) num >= 40.0 ? ((double) num >= 80.0 ? 1f : (float) (((double) num - 40.0) / 40.0)) : 0.0f;
      }
      else
        this.m_windTargetVolume = 0.0f;
      this.m_windVolumeChanged = (double) this.m_windVolume != (double) this.m_windTargetVolume;
      if ((double) this.m_windVolume < (double) this.m_windTargetVolume)
      {
        this.m_windVolume = Math.Min(this.m_windVolume + 0.008333334f, this.m_windTargetVolume);
      }
      else
      {
        if ((double) this.m_windVolume <= (double) this.m_windTargetVolume)
          return;
        this.m_windVolume = Math.Max(this.m_windVolume - 0.008333334f, this.m_windTargetVolume);
      }
    }

    public override void UpdateAfterSimulationParallel()
    {
      if (this.Entity.MarkedForClose || this.m_character?.Physics == null)
        return;
      if (this.m_needsUpdateEmitters)
        MyEntity3DSoundEmitter.UpdateEntityEmitters(true, true, false);
      this.m_selectedStateSound = this.SelectSound();
      this.UpdateBreath();
      this.UpdateWindSounds();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.PlayStateSound();
      this.UpdateWindVolumeAndPlayback();
    }

    private void UpdateWindVolumeAndPlayback()
    {
      if (this.m_windEmitter.IsPlaying)
      {
        if ((double) this.m_windVolume <= 0.0)
          this.m_windEmitter.StopSound(true);
        else
          this.m_windEmitter.CustomVolume = new float?(this.m_windVolume);
      }
      else if ((double) this.m_windVolume > 0.0)
      {
        this.m_windEmitter.PlaySound(this.CharacterSounds[11], true, force2D: true);
        this.m_windEmitter.CustomVolume = new float?(this.m_windVolume);
      }
      if (!this.m_windVolumeChanged)
        return;
      this.m_windVolumeChanged = false;
      MySessionComponentPlanetAmbientSounds component = MySession.Static.GetComponent<MySessionComponentPlanetAmbientSounds>();
      if (component == null)
        return;
      component.VolumeModifierGlobal = 1f - this.m_windVolume;
    }

    public void UpdateAfterSimulation100()
    {
      this.UpdateOxygenSounds();
      this.m_soundEmitters[0].Update();
      this.m_soundEmitters[4].Update();
      if (this.m_windSystem)
      {
        this.m_inAtmosphere = this.m_character.AtmosphereDetectorComp != null && this.m_character.AtmosphereDetectorComp.InAtmosphere;
        this.m_windEmitter.Update();
      }
      if (this.m_oxygenEmitter == null)
        return;
      this.m_oxygenEmitter.Update();
    }

    public void PlayActionSound(MySoundPair actionSound, bool? force3D = null)
    {
      this.m_lastActionSound = actionSound;
      this.m_soundEmitters[3].PlaySound(this.m_lastActionSound, force3D: force3D);
    }

    private void PlayStateSound()
    {
      if (this.m_character.IsClientOnline.HasValue && !this.m_character.IsClientOnline.Value || this.Entity.MarkedForClose || this.m_character?.Physics == null)
        return;
      if (Sync.IsDedicated)
      {
        this.UpdateBreath();
      }
      else
      {
        int num1 = this.m_isFirstPerson ? 1 : 0;
        MyCharacter localCharacter = MySession.Static.LocalCharacter;
        int num2 = localCharacter != null ? (localCharacter.IsInFirstPersonView ? 1 : 0) : 0;
        if (num1 != num2)
        {
          this.m_isFirstPerson = !this.m_isFirstPerson;
          this.m_isFirstPersonChanged = true;
        }
        else
          this.m_isFirstPersonChanged = false;
        this.m_character.Breath?.Update();
        MySoundPair selectedStateSound = this.m_selectedStateSound;
        if (this.m_movementEmitter != null && !this.CharacterSounds[16].Equals((object) MySoundPair.Empty))
        {
          if (this.m_isWalking && !this.m_movementEmitter.IsPlaying)
            this.m_movementEmitter.PlaySound(this.CharacterSounds[16], force2D: MyFakes.FORCE_CHARACTER_2D_SOUND, force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
          if (!this.m_isWalking && this.m_movementEmitter.IsPlaying)
            this.m_movementEmitter.StopSound(false);
        }
        MyEntity3DSoundEmitter soundEmitter1 = this.m_soundEmitters[0];
        MyEntity3DSoundEmitter soundEmitter2 = this.m_soundEmitters[4];
        MyEntity3DSoundEmitter soundEmitter3 = this.m_soundEmitters[2];
        int num3 = !selectedStateSound.Equals((object) soundEmitter1.SoundPair) ? 0 : (soundEmitter1.IsPlaying ? 1 : 0);
        if (this.m_isFirstPersonChanged)
        {
          soundEmitter1.StopSound(true);
          bool isPlaying = soundEmitter2.IsPlaying;
          soundEmitter2.StopSound(true);
          if (isPlaying)
            soundEmitter2.PlaySound(this.CharacterSounds[1], skipIntro: true, force3D: new bool?(!this.m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND));
        }
        IMySourceVoice sound = soundEmitter1.Sound;
        MySoundData lastSoundData = soundEmitter1.LastSoundData;
        if (sound != null && lastSoundData != null)
        {
          float num4 = MathHelper.Clamp(this.m_character.Physics?.LinearVelocity.Length().Value / 7.5f, 0.1f, 1f);
          float num5 = lastSoundData.Volume * num4;
          sound.SetVolume(num5);
        }
        if (num3 == 0 && (!this.m_isWalking || this.m_character.Definition.LoopingFootsteps))
        {
          MyCharacter entity = this.Entity as MyCharacter;
          if (selectedStateSound != MyCharacterSoundComponent.EmptySoundPair && selectedStateSound == this.CharacterSounds[2])
          {
            if ((double) this.m_jetpackSustainTimer >= 0.25)
            {
              if (soundEmitter1.Loop)
                soundEmitter1.StopSound(true);
              soundEmitter1.PlaySound(selectedStateSound, force3D: new bool?(!this.m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND));
            }
          }
          else if (!soundEmitter2.IsPlaying && selectedStateSound != MyCharacterSoundComponent.EmptySoundPair && (entity != null && entity.JetpackRunning))
          {
            if ((double) this.m_jetpackSustainTimer <= 0.0 || selectedStateSound != this.CharacterSounds[1])
              soundEmitter2.PlaySound(this.CharacterSounds[1], force3D: new bool?(!this.m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND));
          }
          else if (selectedStateSound == MyCharacterSoundComponent.EmptySoundPair)
          {
            foreach (MyEntity3DSoundEmitter soundEmitter4 in this.m_soundEmitters)
            {
              if (soundEmitter4.Loop)
                soundEmitter4.StopSound(false);
            }
          }
          else if (selectedStateSound != this.m_lastPrimarySound || selectedStateSound != this.CharacterSounds[3] && selectedStateSound != this.CharacterSounds[4])
          {
            if (soundEmitter1.Loop)
              soundEmitter1.StopSound(false);
            if (selectedStateSound == this.CharacterSounds[2])
              soundEmitter1.PlaySound(selectedStateSound, true, force3D: new bool?(!this.m_isFirstPerson && !MyFakes.FORCE_CHARACTER_2D_SOUND));
            else if (selectedStateSound != this.CharacterSounds[1])
              soundEmitter1.PlaySound(selectedStateSound, true);
          }
        }
        else if (!this.m_character.Definition.LoopingFootsteps && soundEmitter3 != null && selectedStateSound != null)
          this.IKFeetStepSounds(soundEmitter3, selectedStateSound, this.m_isWalking && this.m_character.IsMagneticBootsEnabled);
        if (this.m_character.JetpackComp != null && !this.m_character.JetpackComp.IsFlying && this.m_character.JetpackComp.DampenersEnabled && !this.m_character.Physics.LinearVelocity.Equals(Vector3.Zero))
        {
          float num4 = 0.98f;
          soundEmitter1.VolumeMultiplier *= num4;
        }
        else
          soundEmitter1.VolumeMultiplier = 1f;
        this.m_lastPrimarySound = selectedStateSound;
      }
    }

    private void IKFeetStepSounds(
      MyEntity3DSoundEmitter walkEmitter,
      MySoundPair cueEnum,
      bool magneticBootsOn)
    {
      MyCharacterMovementEnum currentMovementState = this.m_character.GetCurrentMovementState();
      int num1 = this.m_character.IsCrouching ? 1 : 0;
      if ((int) currentMovementState.GetSpeed() != (int) this.m_lastUpdateMovementState.GetSpeed())
      {
        walkEmitter.StopSound(true);
        this.m_lastStepTime = 0;
      }
      int num2 = int.MaxValue;
      if (currentMovementState.GetDirection() != (ushort) 0)
      {
        if ((uint) currentMovementState <= 144U)
        {
          if ((uint) currentMovementState <= 66U)
          {
            if ((uint) currentMovementState <= 18U)
            {
              if (currentMovementState != MyCharacterMovementEnum.Crouching && currentMovementState != MyCharacterMovementEnum.Walking && currentMovementState != MyCharacterMovementEnum.CrouchWalking)
                goto label_27;
            }
            else if ((uint) currentMovementState <= 34U)
            {
              if (currentMovementState != MyCharacterMovementEnum.BackWalking && currentMovementState != MyCharacterMovementEnum.CrouchBackWalking)
                goto label_27;
            }
            else if (currentMovementState != MyCharacterMovementEnum.WalkStrafingLeft && currentMovementState != MyCharacterMovementEnum.CrouchStrafingLeft)
              goto label_27;
          }
          else if ((uint) currentMovementState <= 96U)
          {
            if (currentMovementState != MyCharacterMovementEnum.WalkingLeftFront && currentMovementState != MyCharacterMovementEnum.CrouchWalkingLeftFront && currentMovementState != MyCharacterMovementEnum.WalkingLeftBack)
              goto label_27;
          }
          else if ((uint) currentMovementState <= 128U)
          {
            if (currentMovementState != MyCharacterMovementEnum.CrouchWalkingLeftBack && currentMovementState != MyCharacterMovementEnum.WalkStrafingRight)
              goto label_27;
          }
          else if (currentMovementState != MyCharacterMovementEnum.CrouchStrafingRight && currentMovementState != MyCharacterMovementEnum.WalkingRightFront)
            goto label_27;
        }
        else if ((uint) currentMovementState <= 1104U)
        {
          if ((uint) currentMovementState <= 162U)
          {
            if (currentMovementState != MyCharacterMovementEnum.CrouchWalkingRightFront && currentMovementState != MyCharacterMovementEnum.WalkingRightBack && currentMovementState != MyCharacterMovementEnum.CrouchWalkingRightBack)
              goto label_27;
          }
          else if ((uint) currentMovementState <= 1056U)
          {
            if (currentMovementState != MyCharacterMovementEnum.Running && currentMovementState != MyCharacterMovementEnum.Backrunning)
              goto label_27;
          }
          else if (currentMovementState != MyCharacterMovementEnum.RunStrafingLeft && currentMovementState != MyCharacterMovementEnum.RunningLeftFront)
            goto label_27;
        }
        else if ((uint) currentMovementState <= 1168U)
        {
          if (currentMovementState != MyCharacterMovementEnum.RunningLeftBack && currentMovementState != MyCharacterMovementEnum.RunStrafingRight && currentMovementState != MyCharacterMovementEnum.RunningRightFront)
            goto label_27;
        }
        else if ((uint) currentMovementState <= 2064U)
        {
          if (currentMovementState != MyCharacterMovementEnum.RunningRightBack && currentMovementState != MyCharacterMovementEnum.Sprinting)
            goto label_27;
        }
        else if (currentMovementState != MyCharacterMovementEnum.CrouchRotatingLeft && currentMovementState != MyCharacterMovementEnum.CrouchRotatingRight)
          goto label_27;
        num2 = 100;
      }
label_27:
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastStepTime > num2)
      {
        MyCharacterBone myCharacterBone1 = this.m_character.AnimationController != null ? this.m_character.AnimationController.FindBone(this.m_character.Definition.LeftAnkleBoneName, out int _) : (MyCharacterBone) null;
        MyCharacterBone myCharacterBone2 = this.m_character.AnimationController != null ? this.m_character.AnimationController.FindBone(this.m_character.Definition.RightAnkleBoneName, out int _) : (MyCharacterBone) null;
        Matrix absoluteTransform;
        Vector3 vector3_1;
        if (myCharacterBone1 == null)
        {
          vector3_1 = this.m_character.PositionComp.LocalAABB.Center;
        }
        else
        {
          absoluteTransform = myCharacterBone1.AbsoluteTransform;
          vector3_1 = absoluteTransform.Translation;
        }
        Vector3 vector3_2 = vector3_1;
        Vector3 vector3_3;
        if (myCharacterBone2 == null)
        {
          vector3_3 = this.m_character.PositionComp.LocalAABB.Center;
        }
        else
        {
          absoluteTransform = myCharacterBone2.AbsoluteTransform;
          vector3_3 = absoluteTransform.Translation;
        }
        float heightWhileStanding = this.m_character.Definition.AnkleHeightWhileStanding;
        float num3 = 0.0f;
        if (this.m_character.AnimationController != null)
          this.m_character.AnimationController.Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeed, out num3);
        bool flag1 = (double) vector3_2.Y - (double) heightWhileStanding < (double) this.m_character.PositionComp.LocalAABB.Min.Y;
        bool flag2 = (double) vector3_3.Y - (double) heightWhileStanding < (double) this.m_character.PositionComp.LocalAABB.Min.Y;
        if (flag1 | flag2)
        {
          if ((double) num3 > 0.0)
          {
            if (!(flag1 & flag2))
            {
              if ((flag1 ? 1 : -1) != this.m_lastFootSound)
              {
                this.m_lastFootSound = flag1 ? 1 : -1;
                if (MyFakes.CHARACTER_FOOTS_DEBUG_DRAW)
                  MyRenderProxy.DebugDrawPoint(this.Character.WorldMatrix.Translation, flag1 ? Color.Green : Color.Yellow, true, true);
                walkEmitter.PlaySound(cueEnum, force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
                if (walkEmitter.Sound != null)
                {
                  if (magneticBootsOn)
                  {
                    walkEmitter.Sound.FrequencyRatio *= 0.95f;
                    if (this.m_magneticBootsEmitter != null && this.CharacterSounds[17] != MySoundPair.Empty)
                      this.m_magneticBootsEmitter.PlaySound(this.CharacterSounds[17], force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
                  }
                  else
                    walkEmitter.Sound.FrequencyRatio = 1f;
                }
                this.m_lastStepTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
              }
            }
            else
            {
              this.m_lastFootSound = 0;
              if (MyFakes.CHARACTER_FOOTS_DEBUG_DRAW)
                MyRenderProxy.DebugDrawPoint(this.Character.WorldMatrix.Translation, Color.Red, true, true);
            }
          }
        }
        else if (MyFakes.CHARACTER_FOOTS_DEBUG_DRAW)
          MyRenderProxy.DebugDrawPoint(this.Character.WorldMatrix.Translation, Color.Purple, true, true);
      }
      this.m_lastUpdateMovementState = currentMovementState;
    }

    public bool StopStateSound(bool forceStop = true)
    {
      this.m_soundEmitters[0].StopSound(forceStop);
      return true;
    }

    public void PlaySecondarySound(
      CharacterSoundsEnum soundEnum,
      bool stopPrevious = false,
      bool force2D = false,
      bool? force3D = null)
    {
      this.m_soundEmitters[1].PlaySound(this.CharacterSounds[(int) soundEnum], stopPrevious, force2D: force2D, force3D: force3D);
    }

    public void PlayDeathSound(MyStringHash damageType, bool stopPrevious = false)
    {
      if (damageType == MyCharacterSoundComponent.LowPressure)
        this.m_soundEmitters[1].PlaySound(this.CharacterSounds[8], stopPrevious);
      else
        this.m_soundEmitters[1].PlaySound(this.CharacterSounds[7], stopPrevious);
    }

    public void StartSecondarySound(string cueName, bool sync = false) => this.StartSecondarySound(MySoundPair.GetCueId(cueName), sync);

    public void StartSecondarySound(MyCueId cueId, bool sync = false)
    {
      if (cueId.IsNull)
        return;
      this.m_soundEmitters[1].PlaySoundWithDistance(cueId);
      if (!sync)
        return;
      this.m_character.PlaySecondarySound(cueId);
    }

    public bool StopSecondarySound(bool forceStop = true)
    {
      this.m_soundEmitters[1].StopSound(forceStop);
      return true;
    }

    private MySoundPair SelectSound()
    {
      MySoundPair mySoundPair = MyCharacterSoundComponent.EmptySoundPair;
      MyStringHash orCompute = MyStringHash.GetOrCompute(this.m_character.Definition.PhysicalMaterial);
      this.m_isWalking = false;
      MyCharacterMovementEnum currentMovementState1 = this.m_character.GetCurrentMovementState();
      if ((uint) currentMovementState1 <= 130U)
      {
        if ((uint) currentMovementState1 <= 64U)
        {
          if ((uint) currentMovementState1 <= 18U)
          {
            switch (currentMovementState1)
            {
              case MyCharacterMovementEnum.Standing:
              case MyCharacterMovementEnum.Crouching:
                if (this.m_character.Breath != null)
                  this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
                MyCharacterMovementEnum previousMovementState = this.m_character.GetPreviousMovementState();
                MyCharacterMovementEnum currentMovementState2 = this.m_character.GetCurrentMovementState();
                if (previousMovementState != currentMovementState2 && (previousMovementState == MyCharacterMovementEnum.Standing || previousMovementState == MyCharacterMovementEnum.Crouching))
                  mySoundPair = currentMovementState2 == MyCharacterMovementEnum.Standing ? this.CharacterSounds[4] : this.CharacterSounds[3];
                this.FindSupportingMaterial();
                goto label_69;
              case MyCharacterMovementEnum.Sitting:
                if (this.m_character.Breath != null)
                {
                  this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
                  goto label_69;
                }
                else
                  goto label_69;
              case MyCharacterMovementEnum.Flying:
                if (this.m_character.Breath != null)
                  this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
                if (this.m_character.JetpackComp != null && (double) this.m_jetpackMinIdleTime <= 0.0 && (double) this.m_character.JetpackComp.FinalThrust.LengthSquared() >= 50000.0)
                {
                  mySoundPair = this.CharacterSounds[2];
                  this.m_jetpackSustainTimer = Math.Min(0.25f, this.m_jetpackSustainTimer + 0.01666667f);
                }
                else
                {
                  mySoundPair = this.CharacterSounds[1];
                  this.m_jetpackSustainTimer = Math.Max(0.0f, this.m_jetpackSustainTimer - 0.01666667f);
                }
                this.m_jetpackMinIdleTime -= 0.01666667f;
                if ((this.m_standingOnGrid != null || this.m_standingOnVoxel != null) && this.ShouldUpdateSoundEmitters)
                  this.m_needsUpdateEmitters = true;
                this.ResetStandingSoundStates();
                goto label_69;
              case MyCharacterMovementEnum.Falling:
                if (this.m_character.Breath != null)
                  this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
                if ((this.m_standingOnGrid != null || this.m_standingOnVoxel != null) && this.ShouldUpdateSoundEmitters)
                  this.m_needsUpdateEmitters = true;
                this.ResetStandingSoundStates();
                goto label_69;
              case MyCharacterMovementEnum.Jump:
                if (this.m_jumpReady)
                {
                  this.m_jumpReady = false;
                  this.m_character.SetPreviousMovementState(this.m_character.GetCurrentMovementState());
                  MyEntity3DSoundEmitter soundEmitter = this.m_soundEmitters[5];
                  if (soundEmitter != null)
                  {
                    soundEmitter.Entity = (MyEntity) this.m_character;
                    soundEmitter.PlaySound(this.CharacterSounds[0], alwaysHearOnRealistic: true, force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
                  }
                  if ((this.m_standingOnGrid != null || this.m_standingOnVoxel != null) && this.ShouldUpdateSoundEmitters)
                    this.m_needsUpdateEmitters = true;
                  this.m_standingOnGrid = (MyCubeGrid) null;
                  this.m_standingOnVoxel = (MyVoxelBase) null;
                  goto label_69;
                }
                else
                  goto label_69;
              default:
                if (currentMovementState1 != MyCharacterMovementEnum.Walking)
                {
                  if (currentMovementState1 == MyCharacterMovementEnum.CrouchWalking)
                    goto label_37;
                  else
                    goto label_69;
                }
                else
                  break;
            }
          }
          else if (currentMovementState1 != MyCharacterMovementEnum.BackWalking)
          {
            if (currentMovementState1 != MyCharacterMovementEnum.CrouchBackWalking)
            {
              if (currentMovementState1 != MyCharacterMovementEnum.WalkStrafingLeft)
                goto label_69;
            }
            else
              goto label_37;
          }
        }
        else if ((uint) currentMovementState1 <= 82U)
        {
          if (currentMovementState1 != MyCharacterMovementEnum.CrouchStrafingLeft)
          {
            if (currentMovementState1 != MyCharacterMovementEnum.WalkingLeftFront)
            {
              if (currentMovementState1 == MyCharacterMovementEnum.CrouchWalkingLeftFront)
                goto label_37;
              else
                goto label_69;
            }
          }
          else
            goto label_37;
        }
        else if ((uint) currentMovementState1 <= 98U)
        {
          if (currentMovementState1 != MyCharacterMovementEnum.WalkingLeftBack)
          {
            if (currentMovementState1 == MyCharacterMovementEnum.CrouchWalkingLeftBack)
              goto label_37;
            else
              goto label_69;
          }
        }
        else if (currentMovementState1 != MyCharacterMovementEnum.WalkStrafingRight)
        {
          if (currentMovementState1 == MyCharacterMovementEnum.CrouchStrafingRight)
            goto label_37;
          else
            goto label_69;
        }
      }
      else
      {
        if ((uint) currentMovementState1 <= 1056U)
        {
          if ((uint) currentMovementState1 <= 160U)
          {
            if (currentMovementState1 != MyCharacterMovementEnum.WalkingRightFront)
            {
              if (currentMovementState1 != MyCharacterMovementEnum.CrouchWalkingRightFront)
              {
                if (currentMovementState1 == MyCharacterMovementEnum.WalkingRightBack)
                  goto label_31;
                else
                  goto label_69;
              }
              else
                goto label_37;
            }
            else
              goto label_31;
          }
          else if (currentMovementState1 != MyCharacterMovementEnum.CrouchWalkingRightBack)
          {
            if (currentMovementState1 != MyCharacterMovementEnum.Running && currentMovementState1 != MyCharacterMovementEnum.Backrunning)
              goto label_69;
          }
          else
            goto label_37;
        }
        else if ((uint) currentMovementState1 <= 1120U)
        {
          if (currentMovementState1 != MyCharacterMovementEnum.RunStrafingLeft && currentMovementState1 != MyCharacterMovementEnum.RunningLeftFront && currentMovementState1 != MyCharacterMovementEnum.RunningLeftBack)
            goto label_69;
        }
        else if ((uint) currentMovementState1 <= 1168U)
        {
          if (currentMovementState1 != MyCharacterMovementEnum.RunStrafingRight && currentMovementState1 != MyCharacterMovementEnum.RunningRightFront)
            goto label_69;
        }
        else if (currentMovementState1 != MyCharacterMovementEnum.RunningRightBack)
        {
          if (currentMovementState1 == MyCharacterMovementEnum.Sprinting)
          {
            if (this.m_character.Breath != null)
              this.m_character.Breath.CurrentState = MyCharacterBreath.State.VeryHeated;
            mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyCharacterSoundComponent.MovementSoundType.Sprint, orCompute, this.FindSupportingMaterial());
            this.m_isWalking = true;
            goto label_69;
          }
          else
            goto label_69;
        }
        if (this.m_character.Breath != null)
          this.m_character.Breath.CurrentState = MyCharacterBreath.State.Heated;
        mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyCharacterSoundComponent.MovementSoundType.Run, orCompute, this.FindSupportingMaterial());
        this.m_isWalking = true;
        goto label_69;
      }
label_31:
      if (this.m_character.Breath != null)
        this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
      mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyCharacterSoundComponent.MovementSoundType.Walk, orCompute, this.FindSupportingMaterial());
      this.m_isWalking = true;
      goto label_69;
label_37:
      if (this.m_character.Breath != null)
        this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
      mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyCharacterSoundComponent.MovementSoundType.CrouchWalk, orCompute, this.FindSupportingMaterial());
      this.m_isWalking = true;
label_69:
      if (currentMovementState1 != MyCharacterMovementEnum.Flying)
      {
        this.m_jetpackSustainTimer = 0.0f;
        this.m_jetpackMinIdleTime = 0.5f;
      }
      return mySoundPair;
    }

    private void ResetStandingSoundStates()
    {
      if (MyFakes.ENABLE_REALISTIC_ON_TOUCH)
      {
        if (this.m_standingOnGrid != null && this.m_lastContactCounter < 0)
        {
          this.m_standingOnGrid = (MyCubeGrid) null;
          MyEntity3DSoundEmitter.UpdateEntityEmitters(true, true, false);
        }
        else
          --this.m_lastContactCounter;
      }
      else
      {
        this.m_standingOnGrid = (MyCubeGrid) null;
        this.m_standingOnVoxel = (MyVoxelBase) null;
      }
    }

    private bool IsInvulnerable
    {
      get
      {
        MySession mySession = MySession.Static;
        MyPlayer.PlayerId? id = mySession.ControlledEntity?.ControllerInfo?.Controller?.Player?.Id;
        AdminSettingsEnum adminSettingsEnum;
        return (id.HasValue ? (id.GetValueOrDefault().SerialId == 0 ? 1 : 0) : 0) != 0 && mySession.RemoteAdminSettings.TryGetValue(id.Value.SteamId, out adminSettingsEnum) && (adminSettingsEnum & AdminSettingsEnum.Invulnerable) != AdminSettingsEnum.None;
      }
    }

    private void UpdateOxygenSounds()
    {
      if (this.m_oxygenEmitter == null)
        return;
      MySession mySession = MySession.Static;
      if (!this.m_character.IsDead && mySession != null && (mySession.Settings.EnableOxygen && !mySession.CreativeMode))
      {
        MyCharacterOxygenComponent oxygenComponent = this.m_character.OxygenComponent;
        if ((oxygenComponent != null ? (oxygenComponent.HelmetEnabled ? 1 : 0) : 0) != 0)
        {
          MySoundPair soundId = mySession.CreativeMode || this.IsInvulnerable ? this.CharacterSounds[12] : ((double) this.m_character.OxygenComponent.SuitOxygenLevel <= (double) MyCharacterOxygenComponent.LOW_OXYGEN_RATIO ? ((double) this.m_character.OxygenComponent.SuitOxygenLevel <= (double) MyCharacterOxygenComponent.LOW_OXYGEN_RATIO / 3.0 ? ((double) this.m_character.OxygenComponent.SuitOxygenLevel <= 0.0 ? this.CharacterSounds[15] : this.CharacterSounds[14]) : this.CharacterSounds[13]) : this.CharacterSounds[12]);
          if (this.m_oxygenEmitter.IsPlaying && this.m_oxygenEmitter.SoundPair == soundId)
            return;
          this.m_oxygenEmitter.PlaySound(soundId, true);
          return;
        }
      }
      if (!this.m_oxygenEmitter.IsPlaying)
        return;
      this.m_oxygenEmitter.StopSound(true);
    }

    private void UpdateBreath()
    {
      if (this.IsInvulnerable)
        return;
      MySession mySession = MySession.Static;
      if (this.m_character.OxygenComponent == null || this.m_character.Breath == null)
        return;
      if (mySession.Settings.EnableOxygen && !mySession.CreativeMode)
      {
        if (this.m_character.Parent is MyCockpit parent && parent.BlockDefinition.IsPressurized)
        {
          if (this.m_character.OxygenComponent.HelmetEnabled)
          {
            if ((double) this.m_character.OxygenComponent.SuitOxygenAmount > 0.0)
              this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
            else
              this.m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
          }
          else if ((double) this.m_character.EnvironmentOxygenLevel >= (double) MyCharacterOxygenComponent.LOW_OXYGEN_RATIO)
            this.m_character.Breath.CurrentState = MyCharacterBreath.State.NoBreath;
          else
            this.m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
        }
        else if (this.m_character.OxygenComponent.HelmetEnabled)
        {
          if ((double) this.m_character.OxygenComponent.SuitOxygenAmount > 0.0)
            return;
          this.m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
        }
        else if ((double) this.m_character.EnvironmentOxygenLevel >= (double) MyCharacterOxygenComponent.LOW_OXYGEN_RATIO)
          this.m_character.Breath.CurrentState = MyCharacterBreath.State.NoBreath;
        else if ((double) this.m_character.EnvironmentOxygenLevel > 0.0)
          this.m_character.Breath.CurrentState = MyCharacterBreath.State.VeryHeated;
        else
          this.m_character.Breath.CurrentState = MyCharacterBreath.State.Choking;
      }
      else
      {
        this.m_character.Breath.CurrentState = MyCharacterBreath.State.Calm;
        if (this.m_character.OxygenComponent.HelmetEnabled)
          return;
        this.m_character.Breath.CurrentState = MyCharacterBreath.State.NoBreath;
      }
    }

    public void PlayFallSound()
    {
      MyStringHash supportingMaterial = this.FindSupportingMaterial();
      if (!(supportingMaterial != MyStringHash.NullOrEmpty) || MyMaterialPropertiesHelper.Static == null)
        return;
      MySoundPair collisionCue = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyCharacterSoundComponent.MovementSoundType.Fall, this.m_characterPhysicalMaterial, supportingMaterial);
      if (collisionCue.SoundId.IsNull)
        return;
      MyEntity3DSoundEmitter soundEmitter = this.m_soundEmitters[6];
      if (soundEmitter == null)
        return;
      soundEmitter.Entity = (MyEntity) this.m_character;
      soundEmitter.PlaySound(collisionCue, alwaysHearOnRealistic: true, force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
    }

    private MyEntity UpdateStandingPhysics()
    {
      List<HkRigidBody> supportInfo = this.m_character.Physics?.CharacterProxy?.CharacterRigidBody.GetSupportInfo();
      MyEntity myEntity = (MyEntity) null;
      if (supportInfo != null)
      {
        for (int index = 0; index < supportInfo.Count; ++index)
        {
          myEntity = (MyEntity) supportInfo[index].GetSingleEntity();
          if (myEntity != null)
            break;
        }
      }
      MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
      MyVoxelBase myVoxelBase = myEntity as MyVoxelBase;
      bool flag1 = myCubeGrid != null && this.m_standingOnGrid != myCubeGrid;
      bool flag2 = myVoxelBase != null && this.m_standingOnVoxel != myVoxelBase;
      this.m_standingOnGrid = myCubeGrid;
      this.m_standingOnVoxel = myVoxelBase;
      if (this.ShouldUpdateSoundEmitters && flag1 | flag2)
        MyEntity3DSoundEmitter.UpdateEntityEmitters(true, true, true);
      if (myCubeGrid != null || myVoxelBase != null)
        this.m_jumpReady = true;
      return myEntity;
    }

    private MyStringHash FindSupportingMaterial()
    {
      MyEntity myEntity = this.UpdateStandingPhysics();
      MyStringHash myStringHash = new MyStringHash();
      if (myEntity != null)
      {
        Vector3D position = this.m_character.PositionComp.GetPosition();
        myStringHash = myEntity.Physics.GetMaterialAt(position + this.m_character.PositionComp.WorldMatrixRef.Down * 0.100000001490116);
        if (myStringHash == MyStringHash.NullOrEmpty && myEntity.Parent != null)
        {
          MyCubeGrid parent1 = myEntity.Parent as MyCubeGrid;
          MyCubeBlock parent2 = myEntity.Parent as MyCubeBlock;
          if (parent1 != null && parent1.Physics != null)
            myStringHash = myEntity.Parent.Physics.MaterialType;
          else if (parent2 != null)
            myStringHash = parent2.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
        }
      }
      if (myStringHash == MyStringHash.NullOrEmpty)
        myStringHash = VRage.Game.MyMaterialType.ROCK;
      return myStringHash;
    }

    internal void UpdateEntityEmitters(MyCubeGrid cubeGrid)
    {
      this.m_standingOnGrid = cubeGrid;
      this.m_lastContactCounter = 10;
      MyEntity3DSoundEmitter.UpdateEntityEmitters(true, true, true);
    }

    public void PlayDamageSound(float oldHealth)
    {
      if (!MyFakes.ENABLE_NEW_SOUNDS || MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastScreamTime <= 800)
        return;
      bool force2D = false;
      if (MySession.Static.LocalCharacter == this.Entity)
        force2D = true;
      this.m_lastScreamTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_character.StatComp != null && this.m_character.StatComp.LastDamage.Type == MyCharacterSoundComponent.LowPressure)
        this.PlaySecondarySound(CharacterSoundsEnum.SUFFOCATE_SOUND, force2D: force2D);
      else
        this.PlaySecondarySound(CharacterSoundsEnum.PAIN_SOUND, force2D: force2D);
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_character = this.Entity as MyCharacter;
      foreach (MyEntity3DSoundEmitter soundEmitter in this.m_soundEmitters)
        soundEmitter.Entity = this.Entity as MyEntity;
      if (this.m_windEmitter != null)
        this.m_windEmitter.Entity = this.Entity as MyEntity;
      if (this.m_oxygenEmitter != null)
        this.m_oxygenEmitter.Entity = this.Entity as MyEntity;
      this.m_lastUpdateMovementState = this.m_character.GetCurrentMovementState();
      this.m_characterPhysicalMaterial = MyStringHash.GetOrCompute(this.m_character.Definition.PhysicalMaterial);
      this.InitSounds();
      this.NeedsUpdateAfterSimulation = true;
      this.NeedsUpdateAfterSimulationParallel = true;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      this.StopStateSound();
      this.m_character = (MyCharacter) null;
      base.OnBeforeRemovedFromContainer();
    }

    public override string ComponentTypeDebugString => "CharacterSound";

    internal void PlayMagneticBootsStart()
    {
      if (this.m_magneticBootsEmitter == null || this.CharacterSounds[18] == MySoundPair.Empty)
        return;
      this.m_magneticBootsEmitter.PlaySound(this.CharacterSounds[18], force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
    }

    internal void PlayMagneticBootsEnd()
    {
      if (this.m_magneticBootsEmitter == null || this.CharacterSounds[19] == MySoundPair.Empty)
        return;
      this.m_magneticBootsEmitter.PlaySound(this.CharacterSounds[19], force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
    }

    internal void PlayMagneticBootsProximity()
    {
      if (this.m_magneticBootsEmitter == null || this.CharacterSounds[20] == MySoundPair.Empty)
        return;
      this.m_magneticBootsEmitter.PlaySound(this.CharacterSounds[20], force3D: new bool?(!MyFakes.FORCE_CHARACTER_2D_SOUND));
    }

    private enum MySoundEmitterEnum
    {
      PrimaryState,
      SecondaryState,
      WalkState,
      Action,
      IdleJetState,
      JumpState,
      FallState,
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct MovementSoundType
    {
      public static readonly MyStringId Walk = MyStringId.GetOrCompute(nameof (Walk));
      public static readonly MyStringId CrouchWalk = MyStringId.GetOrCompute(nameof (CrouchWalk));
      public static readonly MyStringId Run = MyStringId.GetOrCompute(nameof (Run));
      public static readonly MyStringId Sprint = MyStringId.GetOrCompute(nameof (Sprint));
      public static readonly MyStringId Fall = MyStringId.GetOrCompute(nameof (Fall));
    }

    private class Sandbox_Game_Components_MyCharacterSoundComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterSoundComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterSoundComponent();

      MyCharacterSoundComponent IActivator<MyCharacterSoundComponent>.CreateInstance() => new MyCharacterSoundComponent();
    }
  }
}
