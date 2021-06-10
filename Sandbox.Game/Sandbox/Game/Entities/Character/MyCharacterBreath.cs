// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyCharacterBreath
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using VRage.Audio;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.Entities.Character
{
  public class MyCharacterBreath
  {
    private readonly string BREATH_CALM = "PlayVocBreath1L";
    private readonly string BREATH_HEAVY = "PlayVocBreath2L";
    private readonly string OXYGEN_CHOKE_NORMAL = "PlayChokeA";
    private readonly string OXYGEN_CHOKE_LOW = "PlayChokeB";
    private readonly string OXYGEN_CHOKE_CRITICAL = "PlayChokeC";
    private const float CHOKE_TRESHOLD_LOW = 55f;
    private const float CHOKE_TRESHOLD_CRITICAL = 25f;
    private const float STAMINA_DRAIN_TIME_RUN = 25f;
    private const float STAMINA_DRAIN_TIME_SPRINT = 8f;
    private const float STAMINA_RECOVERY_EXHAUSTED_TO_CALM = 5f;
    private const float STAMINA_RECOVERY_CALM_TO_ZERO = 15f;
    private const float STAMINA_AMOUNT_RUN = 0.01f;
    private const float STAMINA_AMOUNT_SPRINT = 0.03125f;
    private const float STAMINA_AMOUNT_MAX = 20f;
    private IMySourceVoice m_sound;
    private MyCharacter m_character;
    private MyTimeSpan m_lastChange;
    private MyCharacterBreath.State m_state;
    private float m_staminaDepletion;
    private MySoundPair m_breathCalm;
    private MySoundPair m_breathHeavy;
    private MySoundPair m_oxygenChokeNormal;
    private MySoundPair m_oxygenChokeLow;
    private MySoundPair m_oxygenChokeCritical;

    public MyCharacterBreath(MyCharacter character)
    {
      this.CurrentState = MyCharacterBreath.State.NoBreath;
      this.m_character = character;
      this.m_breathCalm = new MySoundPair(string.IsNullOrEmpty(character.Definition.BreathCalmSoundName) ? this.BREATH_CALM : character.Definition.BreathCalmSoundName);
      this.m_breathHeavy = new MySoundPair(string.IsNullOrEmpty(character.Definition.BreathHeavySoundName) ? this.BREATH_HEAVY : character.Definition.BreathHeavySoundName);
      this.m_oxygenChokeNormal = new MySoundPair(string.IsNullOrEmpty(character.Definition.OxygenChokeNormalSoundName) ? this.OXYGEN_CHOKE_NORMAL : character.Definition.OxygenChokeNormalSoundName);
      this.m_oxygenChokeLow = new MySoundPair(string.IsNullOrEmpty(character.Definition.OxygenChokeLowSoundName) ? this.OXYGEN_CHOKE_LOW : character.Definition.OxygenChokeLowSoundName);
      this.m_oxygenChokeCritical = new MySoundPair(string.IsNullOrEmpty(character.Definition.OxygenChokeCriticalSoundName) ? this.OXYGEN_CHOKE_CRITICAL : character.Definition.OxygenChokeCriticalSoundName);
    }

    public MyCharacterBreath.State CurrentState
    {
      get => this.m_state;
      set => this.m_state = value;
    }

    public void ForceUpdate()
    {
      if (this.m_character == null || this.m_character.StatComp == null || (this.m_character.StatComp.Health == null || MySession.Static == null) || MySession.Static.LocalCharacter != this.m_character)
        return;
      this.SetHealth(this.m_character.StatComp.Health.Value);
    }

    private void SetHealth(float health)
    {
      if ((double) health <= 0.0)
        this.CurrentState = MyCharacterBreath.State.NoBreath;
      this.Update(true);
    }

    public void Update(bool force = false)
    {
      if (MySession.Static == null || MySession.Static.LocalCharacter != this.m_character)
        return;
      this.m_staminaDepletion = this.CurrentState != MyCharacterBreath.State.Heated ? (this.CurrentState != MyCharacterBreath.State.VeryHeated ? Math.Max(this.m_staminaDepletion - 0.01666667f, 0.0f) : Math.Min(this.m_staminaDepletion + 1f / 32f, 20f)) : Math.Min(this.m_staminaDepletion + 0.01f, 20f);
      if (this.CurrentState == MyCharacterBreath.State.NoBreath)
      {
        if (this.m_sound == null)
          return;
        this.m_sound.Stop();
        this.m_sound = (IMySourceVoice) null;
      }
      else
      {
        float num = this.m_character.StatComp.Health.Value;
        if (this.CurrentState == MyCharacterBreath.State.Choking)
        {
          if ((double) num >= 55.0 && (this.m_sound == null || !this.m_sound.IsPlaying || this.m_sound.CueEnum != this.m_oxygenChokeNormal.SoundId))
            this.PlaySound(this.m_oxygenChokeNormal.SoundId, false);
          else if ((double) num >= 25.0 && (double) num < 55.0 && (this.m_sound == null || !this.m_sound.IsPlaying || this.m_sound.CueEnum != this.m_oxygenChokeLow.SoundId))
          {
            this.PlaySound(this.m_oxygenChokeLow.SoundId, false);
          }
          else
          {
            if ((double) num <= 0.0 || (double) num >= 25.0 || this.m_sound != null && this.m_sound.IsPlaying && !(this.m_sound.CueEnum != this.m_oxygenChokeCritical.SoundId))
              return;
            this.PlaySound(this.m_oxygenChokeCritical.SoundId, false);
          }
        }
        else
        {
          if (this.CurrentState != MyCharacterBreath.State.Calm && this.CurrentState != MyCharacterBreath.State.Heated && this.CurrentState != MyCharacterBreath.State.VeryHeated)
            return;
          if ((double) this.m_staminaDepletion < 15.0 && (double) num > 20.0)
          {
            if (!this.m_breathCalm.SoundId.IsNull && (this.m_sound == null || !this.m_sound.IsPlaying || this.m_sound.CueEnum != this.m_breathCalm.SoundId))
            {
              this.PlaySound(this.m_breathCalm.SoundId, true);
            }
            else
            {
              if (this.m_sound == null || !this.m_sound.IsPlaying || !this.m_breathCalm.SoundId.IsNull)
                return;
              this.m_sound.Stop(true);
            }
          }
          else if (!this.m_breathHeavy.SoundId.IsNull && (this.m_sound == null || !this.m_sound.IsPlaying || this.m_sound.CueEnum != this.m_breathHeavy.SoundId))
          {
            this.PlaySound(this.m_breathHeavy.SoundId, true);
          }
          else
          {
            if (this.m_sound == null || !this.m_sound.IsPlaying || !this.m_breathHeavy.SoundId.IsNull)
              return;
            this.m_sound.Stop(true);
          }
        }
      }
    }

    private void PlaySound(MyCueId soundId, bool useCrossfade)
    {
      if (((this.m_sound == null ? 0 : (this.m_sound.IsPlaying ? 1 : 0)) & (useCrossfade ? 1 : 0)) != 0)
      {
        this.m_sound = MyAudio.Static.ApplyEffect(this.m_sound, MyStringHash.GetOrCompute("CrossFade"), new MyCueId[1]
        {
          soundId
        }, new float?(2000f)).OutputSound;
      }
      else
      {
        if (this.m_sound != null)
          this.m_sound.Stop(true);
        this.m_sound = MyAudio.Static.PlaySound(soundId);
      }
    }

    public void Close()
    {
      if (this.m_sound == null)
        return;
      this.m_sound.Stop(true);
    }

    public enum State
    {
      Calm,
      Heated,
      VeryHeated,
      NoBreath,
      Choking,
    }
  }
}
