// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyGuiAudio
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

namespace Sandbox.Game.GUI
{
  public class MyGuiAudio : IMyGuiAudio
  {
    public static bool HudWarnings;
    private static Dictionary<MyGuiSounds, MySoundPair> m_sounds = new Dictionary<MyGuiSounds, MySoundPair>(Enum.GetValues(typeof (MyGuiSounds)).Length);
    private static Dictionary<MyGuiSounds, int> m_lastTimePlaying = new Dictionary<MyGuiSounds, int>();

    public static IMyGuiAudio Static { get; set; }

    static MyGuiAudio()
    {
      MyGuiAudio.Static = (IMyGuiAudio) new MyGuiAudio();
      foreach (MyGuiSounds key in Enum.GetValues(typeof (MyGuiSounds)))
        MyGuiAudio.m_sounds.Add(key, new MySoundPair(key.ToString(), false));
    }

    public void PlaySound(GuiSounds sound)
    {
      if (sound == GuiSounds.None)
        return;
      MyGuiAudio.PlaySound(this.GetSound(sound));
    }

    public static IMySourceVoice PlaySound(MyGuiSounds sound)
    {
      if (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static != null && (MySession.Static.Settings.RealisticSound && MySession.Static.LocalCharacter != null) && (MySession.Static.LocalCharacter.OxygenComponent != null && !MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled))
      {
        MySoundData cue = MyAudio.Static.GetCue(MyGuiAudio.m_sounds[sound].SoundId);
        if (cue != null && cue.CanBeSilencedByVoid && (!(MySession.Static.LocalCharacter.Parent is MyCockpit parent) || !parent.BlockDefinition.IsPressurized) && (double) MySession.Static.LocalCharacter.EnvironmentOxygenLevel <= 0.0)
          return (IMySourceVoice) null;
      }
      return MyGuiAudio.CheckForSynchronizedSounds(sound) ? MyAudio.Static.PlaySound(MyGuiAudio.m_sounds[sound].SoundId) : (IMySourceVoice) null;
    }

    private MyGuiSounds GetSound(GuiSounds sound)
    {
      switch (sound)
      {
        case GuiSounds.MouseClick:
          return MyGuiSounds.HudMouseClick;
        case GuiSounds.MouseOver:
          return MyGuiSounds.HudMouseOver;
        case GuiSounds.Item:
          return MyGuiSounds.HudItem;
        default:
          return MyGuiSounds.HudClick;
      }
    }

    internal static MyCueId GetCue(MyGuiSounds sound) => MyGuiAudio.m_sounds[sound].SoundId;

    private static bool CheckForSynchronizedSounds(MyGuiSounds sound)
    {
      MySoundData cue = MyAudio.Static.GetCue(MyGuiAudio.m_sounds[sound].SoundId);
      if (cue != null && cue.PreventSynchronization >= 0)
      {
        int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
        int num;
        if (MyGuiAudio.m_lastTimePlaying.TryGetValue(sound, out num))
        {
          if (Math.Abs(sessionTotalFrames - num) <= cue.PreventSynchronization)
            return false;
          MyGuiAudio.m_lastTimePlaying[sound] = sessionTotalFrames;
        }
        else
          MyGuiAudio.m_lastTimePlaying.Add(sound, sessionTotalFrames);
      }
      return true;
    }
  }
}
