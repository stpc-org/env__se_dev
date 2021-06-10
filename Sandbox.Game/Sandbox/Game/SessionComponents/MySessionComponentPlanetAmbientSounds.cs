// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentPlanetAmbientSounds
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  internal class MySessionComponentPlanetAmbientSounds : MySessionComponentBase
  {
    private IMySourceVoice m_sound;
    private IMyAudioEffect m_effect;
    private readonly MyStringHash m_crossFade = MyStringHash.GetOrCompute("CrossFade");
    private readonly MyStringHash m_fadeIn = MyStringHash.GetOrCompute("FadeIn");
    private readonly MyStringHash m_fadeOut = MyStringHash.GetOrCompute("FadeOut");
    private MyPlanet m_nearestPlanet;
    private long m_nextPlanetRecalculation = -1;
    private int m_planetRecalculationIntervalInSpace = 300;
    private int m_planetRecalculationIntervalOnPlanet = 300;
    private float m_volumeModifier = 1f;
    private static float m_volumeModifierTarget = 1f;
    private float m_volumeOriginal = 1f;
    private const float VOLUME_CHANGE_SPEED = 0.25f;
    public float VolumeModifierGlobal = 1f;
    private MyPlanetEnvironmentalSoundRule[] m_nearestSoundRules;
    private readonly MyPlanetEnvironmentalSoundRule[] m_emptySoundRules = new MyPlanetEnvironmentalSoundRule[0];

    private MyPlanet Planet
    {
      get => this.m_nearestPlanet;
      set => this.SetNearestPlanet(value);
    }

    public override void LoadData() => base.LoadData();

    protected override void UnloadData()
    {
      base.UnloadData();
      if (this.m_sound != null)
        this.m_sound.Stop();
      this.m_nearestPlanet = (MyPlanet) null;
      this.m_nearestSoundRules = (MyPlanetEnvironmentalSoundRule[]) null;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if ((double) this.m_volumeModifier != (double) MySessionComponentPlanetAmbientSounds.m_volumeModifierTarget)
      {
        this.m_volumeModifier = (double) this.m_volumeModifier >= (double) MySessionComponentPlanetAmbientSounds.m_volumeModifierTarget ? MyMath.Clamp(this.m_volumeModifier - 0.004166667f, MySessionComponentPlanetAmbientSounds.m_volumeModifierTarget, 1f) : MyMath.Clamp(this.m_volumeModifier + 0.004166667f, 0.0f, MySessionComponentPlanetAmbientSounds.m_volumeModifierTarget);
        if (this.m_sound != null && this.m_sound.IsPlaying)
          this.m_sound.SetVolume(this.m_volumeOriginal * this.m_volumeModifier * this.VolumeModifierGlobal);
      }
      long gameplayFrameCounter = (long) MySession.Static.GameplayFrameCounter;
      if (gameplayFrameCounter >= this.m_nextPlanetRecalculation)
      {
        this.Planet = MySessionComponentPlanetAmbientSounds.FindNearestPlanet(MySector.MainCamera.Position);
        this.m_nextPlanetRecalculation = this.Planet != null ? gameplayFrameCounter + (long) this.m_planetRecalculationIntervalOnPlanet : gameplayFrameCounter + (long) this.m_planetRecalculationIntervalInSpace;
      }
      if (this.Planet == null || this.Planet.Provider == null || MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && !this.Planet.HasAtmosphere)
      {
        if (this.m_sound == null)
          return;
        this.m_sound.Stop(true);
      }
      else
      {
        Vector3D vector3D1 = MySector.MainCamera.Position - this.Planet.PositionComp.GetPosition();
        double num1 = vector3D1.Length();
        float ratio = this.Planet.Provider.Shape.DistanceToRatio((float) num1);
        if ((double) ratio < 0.0)
          return;
        Vector3D vector3D2 = -vector3D1 / num1;
        double num2 = -vector3D2.Y;
        float num3 = MySector.DirectionToSunNormalized.Dot((Vector3) -vector3D2);
        double num4 = (double) ratio;
        double num5 = (double) num3;
        MyPlanetEnvironmentalSoundRule[] nearestSoundRules = this.m_nearestSoundRules;
        int index;
        ref int local = ref index;
        if (!MySessionComponentPlanetAmbientSounds.FindSoundRuleIndex((float) num2, (float) num4, (float) num5, nearestSoundRules, out local))
          this.PlaySound(new MyCueId());
        else
          this.PlaySound(new MyCueId(this.m_nearestSoundRules[index].EnvironmentSound));
      }
    }

    private static MyPlanet FindNearestPlanet(Vector3D worldPosition)
    {
      BoundingBoxD box = new BoundingBoxD(worldPosition, worldPosition);
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref box);
      return closestPlanet != null && (double) closestPlanet.AtmosphereAltitude > Vector3D.Distance(worldPosition, closestPlanet.PositionComp.GetPosition()) ? (MyPlanet) null : closestPlanet;
    }

    private void SetNearestPlanet(MyPlanet planet)
    {
      this.m_nearestPlanet = planet;
      if (this.m_nearestPlanet == null || this.m_nearestPlanet.Generator == null)
        return;
      this.m_nearestSoundRules = this.m_nearestPlanet.Generator.SoundRules ?? this.m_emptySoundRules;
    }

    private static bool FindSoundRuleIndex(
      float angleFromEquator,
      float height,
      float sunAngleFromZenith,
      MyPlanetEnvironmentalSoundRule[] soundRules,
      out int outRuleIndex)
    {
      outRuleIndex = -1;
      if (soundRules == null)
        return false;
      for (int index = 0; index < soundRules.Length; ++index)
      {
        if (soundRules[index].Check(angleFromEquator, height, sunAngleFromZenith))
        {
          outRuleIndex = index;
          return true;
        }
      }
      return false;
    }

    private void PlaySound(MyCueId sound)
    {
      if (this.m_sound == null || !this.m_sound.IsPlaying)
      {
        this.m_sound = MyAudio.Static.PlaySound(sound);
        if (!sound.IsNull)
          this.m_effect = MyAudio.Static.ApplyEffect(this.m_sound, this.m_fadeIn);
        if (this.m_effect != null)
          this.m_sound = this.m_effect.OutputSound;
      }
      else if (this.m_effect != null && this.m_effect.Finished && sound.IsNull)
        this.m_sound.Stop(true);
      else if (this.m_sound.CueEnum != sound)
      {
        if (this.m_effect != null && !this.m_effect.Finished)
          this.m_effect.AutoUpdate = true;
        if (sound.IsNull)
          this.m_effect = MyAudio.Static.ApplyEffect(this.m_sound, this.m_fadeOut, duration: new float?(5000f));
        else
          this.m_effect = MyAudio.Static.ApplyEffect(this.m_sound, this.m_crossFade, new MyCueId[1]
          {
            sound
          }, new float?(5000f));
        if (this.m_effect != null && !this.m_effect.Finished)
        {
          this.m_effect.AutoUpdate = true;
          this.m_sound = this.m_effect.OutputSound;
        }
      }
      if (this.m_sound == null)
        return;
      MySoundData cue = MyAudio.Static.GetCue(sound);
      this.m_volumeOriginal = cue != null ? cue.Volume : 1f;
      this.m_sound.SetVolume(this.m_volumeOriginal * this.m_volumeModifier * this.VolumeModifierGlobal);
    }

    public static void SetAmbientOn() => MySessionComponentPlanetAmbientSounds.m_volumeModifierTarget = 1f;

    public static void SetAmbientOff() => MySessionComponentPlanetAmbientSounds.m_volumeModifierTarget = 0.0f;

    public override bool IsRequiredByGame => base.IsRequiredByGame && MyFakes.ENABLE_PLANETS;
  }
}
