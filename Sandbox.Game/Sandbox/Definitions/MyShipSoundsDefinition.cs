// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyShipSoundsDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ShipSoundsDefinition), null)]
  public class MyShipSoundsDefinition : MyDefinitionBase
  {
    public float MinWeight = 3000f;
    public bool AllowSmallGrid = true;
    public bool AllowLargeGrid = true;
    public Dictionary<ShipSystemSoundsEnum, MySoundPair> Sounds = new Dictionary<ShipSystemSoundsEnum, MySoundPair>();
    public List<MyTuple<float, float>> ThrusterVolumes = new List<MyTuple<float, float>>();
    public List<MyTuple<float, float>> EngineVolumes = new List<MyTuple<float, float>>();
    public List<MyTuple<float, float>> WheelsVolumes = new List<MyTuple<float, float>>();
    public float EnginePitchRangeInSemitones = 4f;
    public float EnginePitchRangeInSemitones_h = -2f;
    public float WheelsPitchRangeInSemitones = 4f;
    public float WheelsPitchRangeInSemitones_h = -2f;
    public float ThrusterPitchRangeInSemitones = 4f;
    public float ThrusterPitchRangeInSemitones_h = -2f;
    public float EngineTimeToTurnOn = 4f;
    public float EngineTimeToTurnOff = 3f;
    public float WheelsLowerThrusterVolumeBy = 0.33f;
    public float WheelsFullSpeed = 32f;
    public float WheelsSpeedCompensation = 3f;
    public float ThrusterCompositionMinVolume = 0.4f;
    public float ThrusterCompositionMinVolume_c = 0.6666666f;
    public float ThrusterCompositionChangeSpeed = 0.025f;
    public float SpeedUpSoundChangeVolumeTo = 1f;
    public float SpeedDownSoundChangeVolumeTo = 1f;
    public float SpeedUpDownChangeSpeed = 0.2f;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ShipSoundsDefinition soundsDefinition = builder as MyObjectBuilder_ShipSoundsDefinition;
      this.MinWeight = soundsDefinition.MinWeight;
      this.AllowSmallGrid = soundsDefinition.AllowSmallGrid;
      this.AllowLargeGrid = soundsDefinition.AllowLargeGrid;
      this.EnginePitchRangeInSemitones = soundsDefinition.EnginePitchRangeInSemitones;
      this.EnginePitchRangeInSemitones_h = soundsDefinition.EnginePitchRangeInSemitones * -0.5f;
      this.EngineTimeToTurnOn = soundsDefinition.EngineTimeToTurnOn;
      this.EngineTimeToTurnOff = soundsDefinition.EngineTimeToTurnOff;
      this.WheelsLowerThrusterVolumeBy = soundsDefinition.WheelsLowerThrusterVolumeBy;
      this.WheelsFullSpeed = soundsDefinition.WheelsFullSpeed;
      this.ThrusterCompositionMinVolume = soundsDefinition.ThrusterCompositionMinVolume;
      this.ThrusterCompositionMinVolume_c = soundsDefinition.ThrusterCompositionMinVolume / (1f - soundsDefinition.ThrusterCompositionMinVolume);
      this.ThrusterCompositionChangeSpeed = soundsDefinition.ThrusterCompositionChangeSpeed;
      this.SpeedDownSoundChangeVolumeTo = soundsDefinition.SpeedDownSoundChangeVolumeTo;
      this.SpeedUpSoundChangeVolumeTo = soundsDefinition.SpeedUpSoundChangeVolumeTo;
      this.SpeedUpDownChangeSpeed = soundsDefinition.SpeedUpDownChangeSpeed * 0.01666667f;
      foreach (ShipSound sound in soundsDefinition.Sounds)
      {
        if (sound.SoundName.Length != 0)
        {
          MySoundPair mySoundPair = new MySoundPair(sound.SoundName);
          if (mySoundPair != MySoundPair.Empty)
            this.Sounds.Add(sound.SoundType, mySoundPair);
        }
      }
      List<MyTuple<float, float>> source1 = new List<MyTuple<float, float>>();
      foreach (ShipSoundVolumePair thrusterVolume in soundsDefinition.ThrusterVolumes)
        source1.Add(new MyTuple<float, float>(Math.Max(0.0f, thrusterVolume.Speed), Math.Max(0.0f, thrusterVolume.Volume)));
      this.ThrusterVolumes = source1.OrderBy<MyTuple<float, float>, float>((Func<MyTuple<float, float>, float>) (o => o.Item1)).ToList<MyTuple<float, float>>();
      List<MyTuple<float, float>> source2 = new List<MyTuple<float, float>>();
      foreach (ShipSoundVolumePair engineVolume in soundsDefinition.EngineVolumes)
        source2.Add(new MyTuple<float, float>(Math.Max(0.0f, engineVolume.Speed), Math.Max(0.0f, engineVolume.Volume)));
      this.EngineVolumes = source2.OrderBy<MyTuple<float, float>, float>((Func<MyTuple<float, float>, float>) (o => o.Item1)).ToList<MyTuple<float, float>>();
      List<MyTuple<float, float>> source3 = new List<MyTuple<float, float>>();
      foreach (ShipSoundVolumePair wheelsVolume in soundsDefinition.WheelsVolumes)
        source3.Add(new MyTuple<float, float>(Math.Max(0.0f, wheelsVolume.Speed), Math.Max(0.0f, wheelsVolume.Volume)));
      this.WheelsVolumes = source3.OrderBy<MyTuple<float, float>, float>((Func<MyTuple<float, float>, float>) (o => o.Item1)).ToList<MyTuple<float, float>>();
    }

    private class Sandbox_Definitions_MyShipSoundsDefinition\u003C\u003EActor : IActivator, IActivator<MyShipSoundsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyShipSoundsDefinition();

      MyShipSoundsDefinition IActivator<MyShipSoundsDefinition>.CreateInstance() => new MyShipSoundsDefinition();
    }
  }
}
