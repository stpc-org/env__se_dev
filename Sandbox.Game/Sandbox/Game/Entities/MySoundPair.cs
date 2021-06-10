// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySoundPair
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage.Audio;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  public class MySoundPair
  {
    public static MySoundPair Empty = new MySoundPair();
    [ThreadStatic]
    private static StringBuilder m_cache;
    private MyCueId m_arcade;
    private MyCueId m_realistic;

    private static StringBuilder Cache
    {
      get
      {
        if (MySoundPair.m_cache == null)
          MySoundPair.m_cache = new StringBuilder();
        return MySoundPair.m_cache;
      }
    }

    public MyCueId Arcade => this.m_arcade;

    public MyCueId Realistic => this.m_realistic;

    public MySoundPair() => this.Init((string) null);

    public MySoundPair(string cueName, bool useLog = true) => this.Init(cueName, useLog);

    public void Init(string cueName, bool useLog = true)
    {
      if (string.IsNullOrEmpty(cueName) || Sandbox.Engine.Platform.Game.IsDedicated || MyAudio.Static == null)
      {
        this.m_arcade = new MyCueId(MyStringHash.NullOrEmpty);
        this.m_realistic = new MyCueId(MyStringHash.NullOrEmpty);
      }
      else
      {
        this.m_arcade = MyAudio.Static.GetCueId(cueName);
        if (this.m_arcade.Hash != MyStringHash.NullOrEmpty)
        {
          this.m_realistic = this.m_arcade;
        }
        else
        {
          MySoundPair.Cache.Clear();
          MySoundPair.Cache.Append("Arc").Append(cueName);
          this.m_arcade = MyAudio.Static.GetCueId(MySoundPair.Cache.ToString());
          MySoundPair.Cache.Clear();
          MySoundPair.Cache.Append("Real").Append(cueName);
          this.m_realistic = MyAudio.Static.GetCueId(MySoundPair.Cache.ToString());
          if (!useLog)
            return;
          if (this.m_arcade.Hash == MyStringHash.NullOrEmpty && this.m_realistic.Hash == MyStringHash.NullOrEmpty)
          {
            MySandboxGame.Log.WriteLine(string.Format("Could not find any sound for '{0}'", (object) cueName));
          }
          else
          {
            if (this.m_arcade.IsNull)
              string.Format("Could not find arcade sound for '{0}'", (object) cueName);
            if (!this.m_realistic.IsNull)
              return;
            string.Format("Could not find realistic sound for '{0}'", (object) cueName);
          }
        }
      }
    }

    public void Init(MyCueId cueId)
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
        {
          this.m_realistic = cueId;
          this.m_arcade = new MyCueId(MyStringHash.NullOrEmpty);
        }
        else
        {
          this.m_arcade = cueId;
          this.m_realistic = new MyCueId(MyStringHash.NullOrEmpty);
        }
      }
      else
      {
        this.m_arcade = new MyCueId(MyStringHash.NullOrEmpty);
        this.m_realistic = new MyCueId(MyStringHash.NullOrEmpty);
      }
    }

    public MyCueId SoundId => MySession.Static != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS ? this.m_realistic : this.m_arcade;

    public override bool Equals(object obj)
    {
      if (!(obj is MySoundPair))
        return base.Equals(obj);
      return this.Arcade == (obj as MySoundPair).Arcade && this.Realistic == (obj as MySoundPair).Realistic;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => this.SoundId.ToString();

    public static MyCueId GetCueId(string cueName)
    {
      if (string.IsNullOrEmpty(cueName))
        return new MyCueId(MyStringHash.NullOrEmpty);
      MyCueId cueId = MyAudio.Static.GetCueId(cueName);
      if (cueId.Hash != MyStringHash.NullOrEmpty)
        return cueId;
      MySoundPair.Cache.Clear();
      if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
      {
        MySoundPair.Cache.Append("Real").Append(cueName);
        return MyAudio.Static.GetCueId(MySoundPair.Cache.ToString());
      }
      MySoundPair.Cache.Append("Arc").Append(cueName);
      return MyAudio.Static.GetCueId(MySoundPair.Cache.ToString());
    }
  }
}
