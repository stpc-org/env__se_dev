// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
  public interface IMySoundBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Volume { get; set; }

    float Range { get; set; }

    bool IsSoundSelected { get; }

    float LoopPeriod { get; set; }

    string SelectedSound { get; set; }

    void Play();

    void Stop();

    void GetSounds(List<string> sounds);
  }
}
