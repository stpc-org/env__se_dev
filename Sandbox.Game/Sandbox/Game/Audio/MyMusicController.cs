// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Audio.MyMusicController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Audio
{
  internal class MyMusicController
  {
    private const int METEOR_SHOWER_MUSIC_FREQUENCY = 43200;
    private const int METEOR_SHOWER_CROSSFADE_LENGTH = 2000;
    private const int DEFAULT_NO_MUSIC_TIME_MIN = 2;
    private const int DEFAULT_NO_MUSIC_TIME_MAX = 8;
    private const int FAST_NO_MUSIC_TIME_MIN = 1;
    private const int FAST_NO_MUSIC_TIME_MAX = 4;
    private const int BUILDING_NEED = 7000;
    private const int BUILDING_COOLDOWN = 45000;
    private const int BUILDING_CROSSFADE_LENGTH = 2000;
    private const int FIGHTING_NEED = 100;
    private const int FIGHTING_COOLDOWN_LIGHT = 15;
    private const int FIGHTING_COOLDOWN_HEAVY = 20;
    private const int FIGHTING_CROSSFADE_LENGTH = 2000;
    private static List<MyMusicController.MusicOption> m_defaultSpaceCategories = new List<MyMusicController.MusicOption>()
    {
      new MyMusicController.MusicOption("Space", 0.7f),
      new MyMusicController.MusicOption("Calm", 0.25f),
      new MyMusicController.MusicOption("Mystery", 0.05f)
    };
    private static List<MyMusicController.MusicOption> m_defaultPlanetCategory = new List<MyMusicController.MusicOption>()
    {
      new MyMusicController.MusicOption("Planet", 0.8f),
      new MyMusicController.MusicOption("Calm", 0.1f),
      new MyMusicController.MusicOption("Danger", 0.1f)
    };
    private static MyStringHash m_hashCrossfade = MyStringHash.GetOrCompute("CrossFade");
    private static MyStringHash m_hashFadeIn = MyStringHash.GetOrCompute("FadeIn");
    private static MyStringHash m_hashFadeOut = MyStringHash.GetOrCompute("FadeOut");
    private static MyStringId m_stringIdDanger = MyStringId.GetOrCompute("Danger");
    private static MyStringId m_stringIdBuilding = MyStringId.GetOrCompute("Building");
    private static MyStringId m_stringIdLightFight = MyStringId.GetOrCompute("LightFight");
    private static MyStringId m_stringIdHeavyFight = MyStringId.GetOrCompute("HeavyFight");
    private static MyCueId m_cueEmpty = new MyCueId();
    public bool Active;
    public bool CanChangeCategoryGlobal = true;
    private bool CanChangeCategoryLocal = true;
    private Dictionary<MyStringId, List<MyCueId>> m_musicCuesAll;
    private Dictionary<MyStringId, List<MyCueId>> m_musicCuesRemaining;
    private List<MyMusicController.MusicOption> m_actualMusicOptions = new List<MyMusicController.MusicOption>();
    private MyPlanet m_lastVisitedPlanet;
    private MySoundData m_lastMusicData;
    private int m_frameCounter;
    private float m_noMusicTimer;
    private MyRandom m_random = new MyRandom();
    private IMySourceVoice m_musicSourceVoice;
    private int m_lastMeteorShower = int.MinValue;
    private MyMusicController.MusicCategory m_currentMusicCategory;
    private int m_meteorShower;
    private int m_building;
    private int m_buildingCooldown;
    private int m_fightLight;
    private int m_fightLightCooldown;
    private int m_fightHeavy;
    private int m_fightHeavyCooldown;

    public static MyMusicController Static { get; set; }

    public MyStringId CategoryPlaying { get; private set; }

    public MyStringId CategoryLast { get; private set; }

    public MyCueId CueIdPlaying { get; private set; }

    public float NextMusicTrackIn => this.m_noMusicTimer;

    public bool CanChangeCategory => this.CanChangeCategoryGlobal && this.CanChangeCategoryLocal;

    public bool MusicIsPlaying => this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying;

    public MyMusicController(Dictionary<MyStringId, List<MyCueId>> musicCues = null)
    {
      this.CategoryPlaying = MyStringId.NullOrEmpty;
      this.CategoryLast = MyStringId.NullOrEmpty;
      this.Active = false;
      this.m_musicCuesAll = musicCues != null ? this.DuplicateMusicCues(musicCues) : new Dictionary<MyStringId, List<MyCueId>>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
      this.m_musicCuesRemaining = new Dictionary<MyStringId, List<MyCueId>>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    }

    private Dictionary<MyStringId, List<MyCueId>> DuplicateMusicCues(
      Dictionary<MyStringId, List<MyCueId>> source)
    {
      Dictionary<MyStringId, List<MyCueId>> dictionary = new Dictionary<MyStringId, List<MyCueId>>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
      foreach (KeyValuePair<MyStringId, List<MyCueId>> keyValuePair in source)
      {
        dictionary.Add(keyValuePair.Key, new List<MyCueId>());
        foreach (MyCueId myCueId in keyValuePair.Value)
          dictionary[keyValuePair.Key].Add(myCueId);
      }
      return dictionary;
    }

    private void Update_1s()
    {
      if (this.m_meteorShower > 0)
        --this.m_meteorShower;
      if (this.m_buildingCooldown > 0)
        this.m_buildingCooldown = Math.Max(0, this.m_buildingCooldown - 1000);
      else if (this.m_building > 0)
        this.m_building = Math.Max(0, this.m_building - 1000);
      if (this.m_fightHeavyCooldown > 0)
        this.m_fightHeavyCooldown = Math.Max(0, this.m_fightHeavyCooldown - 1);
      else if (this.m_fightHeavy > 0)
        this.m_fightHeavy = Math.Max(0, this.m_fightHeavy - 10);
      if (this.m_fightLightCooldown > 0)
      {
        this.m_fightLightCooldown = Math.Max(0, this.m_fightLightCooldown - 1);
      }
      else
      {
        if (this.m_fightLight <= 0)
          return;
        this.m_fightLight = Math.Max(0, this.m_fightLight - 10);
      }
    }

    public void Update()
    {
      if (this.m_frameCounter % 60 == 0)
        this.Update_1s();
      if (this.MusicIsPlaying)
      {
        if (MyAudio.Static.Mute)
          MyAudio.Static.Mute = false;
        this.m_musicSourceVoice.SetVolume(this.m_lastMusicData != null ? MyAudio.Static.VolumeMusic * this.m_lastMusicData.Volume : MyAudio.Static.VolumeMusic);
      }
      else if ((double) this.m_noMusicTimer > 0.0)
      {
        this.m_noMusicTimer -= 0.01666667f;
      }
      else
      {
        if (this.CanChangeCategory)
          this.m_currentMusicCategory = this.m_fightHeavy < 100 ? (this.m_fightLight < 100 ? (this.m_meteorShower <= 0 ? (this.m_building < 7000 ? MyMusicController.MusicCategory.location : MyMusicController.MusicCategory.building) : MyMusicController.MusicCategory.danger) : MyMusicController.MusicCategory.lightFight) : MyMusicController.MusicCategory.heavyFight;
        switch (this.m_currentMusicCategory)
        {
          case MyMusicController.MusicCategory.building:
            this.PlayBuildingMusic();
            break;
          case MyMusicController.MusicCategory.danger:
            this.PlayDangerMusic();
            break;
          case MyMusicController.MusicCategory.lightFight:
            this.PlayFightingMusic(true);
            break;
          case MyMusicController.MusicCategory.heavyFight:
            this.PlayFightingMusic(false);
            break;
          case MyMusicController.MusicCategory.custom:
            this.PlaySpecificMusicCategory(this.CategoryLast, !this.CanChangeCategoryLocal);
            break;
          default:
            this.CalculateNextCue();
            break;
        }
      }
      ++this.m_frameCounter;
    }

    public void Building(int amount)
    {
      this.m_building = Math.Min(7000, this.m_building + amount);
      this.m_buildingCooldown = Math.Min(45000, this.m_buildingCooldown + amount * 5);
      if (!this.CanChangeCategory || this.m_building < 7000)
        return;
      this.m_noMusicTimer = (float) this.m_random.Next(1, 4);
      if (this.m_currentMusicCategory >= MyMusicController.MusicCategory.building)
        return;
      this.PlayBuildingMusic();
    }

    public void MeteorShowerIncoming()
    {
      int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
      if (!this.CanChangeCategory || Math.Abs(this.m_lastMeteorShower - sessionTotalFrames) < 43200)
        return;
      this.m_meteorShower = 10;
      this.m_lastMeteorShower = sessionTotalFrames;
      this.m_noMusicTimer = (float) this.m_random.Next(1, 4);
      if (this.m_currentMusicCategory >= MyMusicController.MusicCategory.danger)
        return;
      this.PlayDangerMusic();
    }

    public void Fighting(bool heavy, int amount)
    {
      this.m_fightLight = Math.Min(this.m_fightLight + amount, 100);
      this.m_fightLightCooldown = 15;
      if (heavy)
      {
        this.m_fightHeavy = Math.Min(this.m_fightHeavy + amount, 100);
        this.m_fightHeavyCooldown = 20;
      }
      if (!this.CanChangeCategory)
        return;
      if (this.m_fightHeavy >= 100 && this.m_currentMusicCategory < MyMusicController.MusicCategory.heavyFight)
      {
        this.PlayFightingMusic(false);
      }
      else
      {
        if (this.m_fightLight < 100 || this.m_currentMusicCategory >= MyMusicController.MusicCategory.lightFight)
          return;
        this.PlayFightingMusic(true);
      }
    }

    public void IncreaseCategory(MyStringId category, int amount)
    {
      if (category == MyMusicController.m_stringIdLightFight)
        this.Fighting(false, amount);
      else if (category == MyMusicController.m_stringIdHeavyFight)
        this.Fighting(true, amount);
      else if (category == MyMusicController.m_stringIdBuilding)
      {
        this.Building(amount);
      }
      else
      {
        if (!(category == MyMusicController.m_stringIdDanger))
          return;
        this.MeteorShowerIncoming();
      }
    }

    private void PlayDangerMusic()
    {
      this.CategoryPlaying = MyMusicController.m_stringIdDanger;
      this.m_currentMusicCategory = MyMusicController.MusicCategory.danger;
      if (this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying)
        this.PlayMusic(this.CueIdPlaying, MyMusicController.m_hashCrossfade, cueIds: new MyCueId[1]
        {
          this.SelectCueFromCategory(MyMusicController.m_stringIdDanger)
        }, play: false);
      else
        this.PlayMusic(this.SelectCueFromCategory(this.CategoryPlaying), MyMusicController.m_hashFadeIn, 1000, new MyCueId[0]);
      this.m_noMusicTimer = (float) this.m_random.Next(2, 8);
    }

    private void PlayBuildingMusic()
    {
      this.CategoryPlaying = MyMusicController.m_stringIdBuilding;
      this.m_currentMusicCategory = MyMusicController.MusicCategory.building;
      if (this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying)
        this.PlayMusic(this.CueIdPlaying, MyMusicController.m_hashCrossfade, cueIds: new MyCueId[1]
        {
          this.SelectCueFromCategory(MyMusicController.m_stringIdBuilding)
        }, play: false);
      else
        this.PlayMusic(this.SelectCueFromCategory(this.CategoryPlaying), MyMusicController.m_hashFadeIn, 1000, new MyCueId[0]);
      this.m_noMusicTimer = (float) this.m_random.Next(2, 8);
    }

    private void PlayFightingMusic(bool light)
    {
      this.CategoryPlaying = light ? MyMusicController.m_stringIdLightFight : MyMusicController.m_stringIdHeavyFight;
      this.m_currentMusicCategory = light ? MyMusicController.MusicCategory.lightFight : MyMusicController.MusicCategory.heavyFight;
      if (this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying)
        this.PlayMusic(this.CueIdPlaying, MyMusicController.m_hashCrossfade, cueIds: new MyCueId[1]
        {
          this.SelectCueFromCategory(this.CategoryPlaying)
        }, play: false);
      else
        this.PlayMusic(this.SelectCueFromCategory(this.CategoryPlaying), MyMusicController.m_hashFadeIn, 1000, new MyCueId[0]);
      this.m_noMusicTimer = (float) this.m_random.Next(1, 4);
    }

    private void CalculateNextCue()
    {
      if (MySession.Static == null || MySession.Static.LocalCharacter == null)
        return;
      this.m_noMusicTimer = (float) this.m_random.Next(2, 8);
      Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
      MySphericalNaturalGravityComponent gravityComponent = closestPlanet != null ? closestPlanet.Components.Get<MyGravityProviderComponent>() as MySphericalNaturalGravityComponent : (MySphericalNaturalGravityComponent) null;
      if (closestPlanet != null && gravityComponent != null && Vector3D.Distance(position, closestPlanet.PositionComp.GetPosition()) <= (double) gravityComponent.GravityLimit * 0.649999976158142)
      {
        if (closestPlanet != this.m_lastVisitedPlanet)
        {
          this.m_lastVisitedPlanet = closestPlanet;
          if (closestPlanet.Generator.MusicCategories != null && closestPlanet.Generator.MusicCategories.Count > 0)
          {
            this.m_actualMusicOptions.Clear();
            foreach (MyMusicCategory musicCategory in closestPlanet.Generator.MusicCategories)
              this.m_actualMusicOptions.Add(new MyMusicController.MusicOption(musicCategory.Category, musicCategory.Frequency));
          }
          else
            this.m_actualMusicOptions = MyMusicController.m_defaultPlanetCategory;
        }
      }
      else
      {
        this.m_lastVisitedPlanet = (MyPlanet) null;
        this.m_actualMusicOptions = MyMusicController.m_defaultSpaceCategories;
      }
      float num1 = 0.0f;
      foreach (MyMusicController.MusicOption actualMusicOption in this.m_actualMusicOptions)
        num1 += Math.Max(actualMusicOption.Frequency, 0.0f);
      float num2 = (float) this.m_random.NextDouble() * num1;
      MyStringId category = this.m_actualMusicOptions[0].Category;
      for (int index = 0; index < this.m_actualMusicOptions.Count; ++index)
      {
        if ((double) num2 <= (double) this.m_actualMusicOptions[index].Frequency)
        {
          category = this.m_actualMusicOptions[index].Category;
          break;
        }
        num2 -= this.m_actualMusicOptions[index].Frequency;
      }
      this.CueIdPlaying = this.SelectCueFromCategory(category);
      this.CategoryPlaying = category;
      if (this.CueIdPlaying == MyMusicController.m_cueEmpty)
        return;
      this.PlayMusic(this.CueIdPlaying, MyStringHash.NullOrEmpty);
      this.m_currentMusicCategory = MyMusicController.MusicCategory.location;
    }

    public void PlaySpecificMusicTrack(MyCueId cue, bool playAtLeastOnce)
    {
      if (cue.IsNull)
        return;
      if (this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying)
        this.PlayMusic(this.CueIdPlaying, MyMusicController.m_hashCrossfade, cueIds: new MyCueId[1]
        {
          cue
        }, play: false);
      else
        this.PlayMusic(cue, MyMusicController.m_hashFadeIn, 1000, new MyCueId[0]);
      this.m_noMusicTimer = (float) this.m_random.Next(2, 8);
      this.CanChangeCategoryLocal = !playAtLeastOnce;
      this.m_currentMusicCategory = MyMusicController.MusicCategory.location;
    }

    public void PlaySpecificMusicCategory(MyStringId category, bool playAtLeastOnce)
    {
      if (category.Id == 0)
        return;
      this.CategoryPlaying = category;
      if (this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying)
        this.PlayMusic(this.CueIdPlaying, MyMusicController.m_hashCrossfade, cueIds: new MyCueId[1]
        {
          this.SelectCueFromCategory(this.CategoryPlaying)
        }, play: false);
      else
        this.PlayMusic(this.SelectCueFromCategory(this.CategoryPlaying), MyMusicController.m_hashFadeIn, 1000, new MyCueId[0]);
      this.m_noMusicTimer = (float) this.m_random.Next(2, 8);
      this.CanChangeCategoryLocal = !playAtLeastOnce;
      this.m_currentMusicCategory = MyMusicController.MusicCategory.custom;
    }

    public void SetSpecificMusicCategory(MyStringId category)
    {
      if (category.Id == 0)
        return;
      this.CategoryPlaying = category;
      this.m_currentMusicCategory = MyMusicController.MusicCategory.custom;
    }

    private void PlayMusic(
      MyCueId cue,
      MyStringHash effect,
      int effectDuration = 2000,
      MyCueId[] cueIds = null,
      bool play = true)
    {
      if (MyAudio.Static == null)
        return;
      if (play)
        this.m_musicSourceVoice = MyAudio.Static.PlayMusicCue(cue, true);
      if (this.m_musicSourceVoice != null)
      {
        if (effect != MyStringHash.NullOrEmpty)
          this.m_musicSourceVoice = MyAudio.Static.ApplyEffect(this.m_musicSourceVoice, effect, cueIds, new float?((float) effectDuration), true).OutputSound;
        if (this.m_musicSourceVoice != null)
          this.m_musicSourceVoice.StoppedPlaying += new Action<IMySourceVoice>(this.MusicStopped);
      }
      this.m_lastMusicData = MyAudio.Static.GetCue(cue);
    }

    private MyCueId SelectCueFromCategory(MyStringId category)
    {
      if (!this.m_musicCuesRemaining.ContainsKey(category))
        this.m_musicCuesRemaining.Add(category, new List<MyCueId>());
      if (this.m_musicCuesRemaining[category].Count == 0)
      {
        if (!this.m_musicCuesAll.ContainsKey(category) || this.m_musicCuesAll[category] == null || this.m_musicCuesAll[category].Count == 0)
          return MyMusicController.m_cueEmpty;
        foreach (MyCueId myCueId in this.m_musicCuesAll[category])
          this.m_musicCuesRemaining[category].Add(myCueId);
        this.m_musicCuesRemaining[category].ShuffleList<MyCueId>();
      }
      MyCueId myCueId1 = this.m_musicCuesRemaining[category][0];
      this.m_musicCuesRemaining[category].RemoveAt(0);
      return myCueId1;
    }

    public void MusicStopped(IMySourceVoice _)
    {
      if (this.m_musicSourceVoice != null && this.m_musicSourceVoice.IsPlaying)
        return;
      this.CategoryLast = this.CategoryPlaying;
    }

    public void ClearMusicCues()
    {
      this.m_musicCuesAll.Clear();
      this.m_musicCuesRemaining.Clear();
    }

    public void AddMusicCue(MyStringId category, MyCueId cueId)
    {
      if (!this.m_musicCuesAll.ContainsKey(category))
        this.m_musicCuesAll.Add(category, new List<MyCueId>());
      this.m_musicCuesAll[category].Add(cueId);
    }

    public void SetMusicCues(Dictionary<MyStringId, List<MyCueId>> musicCues)
    {
      this.ClearMusicCues();
      this.m_musicCuesAll = musicCues;
    }

    public void Unload()
    {
      if (this.m_musicSourceVoice != null)
      {
        this.m_musicSourceVoice.Stop();
        this.m_musicSourceVoice = (IMySourceVoice) null;
      }
      this.Active = false;
      this.ClearMusicCues();
    }

    private struct MusicOption
    {
      public MyStringId Category;
      public float Frequency;

      public MusicOption(string category, float frequency)
      {
        this.Category = MyStringId.GetOrCompute(category);
        this.Frequency = frequency;
      }
    }

    private enum MusicCategory
    {
      location,
      building,
      danger,
      lightFight,
      heavyFight,
      custom,
    }
  }
}
